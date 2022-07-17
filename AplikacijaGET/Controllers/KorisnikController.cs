using AplikacijaGET.Config;
using AplikacijaGET.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplikacijaGET.Controllers
{
   
    public class KorisnikController : Controller
    {
        
        public Korisnik logovaniKorisnik = new Korisnik();
       
        // GET: Korisnik
        public ActionResult Login() {
            return View();
        }
        public ActionResult Logout() {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Login(Korisnik korisnik)
        {
            if (ModelState.IsValidField("Korisnickoime")&& ModelState.IsValidField("Sifra"))
            {
                Korisnik r = null;

                using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Korisnici", con))
                    {
                        if (con.State != System.Data.ConnectionState.Open)
                        { con.Open(); }
                        SqlDataReader sdr = cmd.ExecuteReader();

                        DataTable dtKorisnici = new DataTable();
                        dtKorisnici.Load(sdr);


                        foreach (DataRow row in dtKorisnici.Rows)
                        {
                            if (korisnik.Sifra.Equals(row["Sifra"].ToString()) && korisnik.KorisnickoIme.Equals(row["KorisnickoIme"].ToString()))
                            {
                                r = (
                                    new Korisnik
                                    {
                                        KorisnikID = Convert.ToInt32(row["KorisnikID"]),
                                        KorisnickoIme = row["KorisnickoIme"].ToString(),
                                        Sifra = row["Sifra"].ToString(),
                                        Ime = row["Ime"].ToString(),
                                        Prezime = row["Prezime"].ToString(),
                                        Uloga = Convert.ToInt32(row["Uloga"]),
                                        Email = row["Email"].ToString()
                                    }
                                    );
                            }

                        }

                    }
                }
                if (r != null) {
                    Session["korisnik"] = r.KorisnikID;
                    
                    Session["uloga"] = r.Uloga;
                    logovaniKorisnik  = r;
                    
                    return RedirectToAction("About", "Home");
                } else {
                    return RedirectToAction("Login", "Korisnik"); }
            }
            return RedirectToAction("Login", "Korisnik");
        }
        public ActionResult Create() {


            return View();
        }
        [HttpPost]
        public ActionResult Create(Korisnik korisnik) {
             List<String> korisnici = new List<String>();
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT KorisnickoIme FROM Korisnici", con))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    { con.Open(); }
                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtLetovi = new DataTable();
                    dtLetovi.Load(sdr);
                    foreach (DataRow row in dtLetovi.Rows)
                    {
                        string ime = row["KorisnickoIme"].ToString();
                        korisnici.Add(ime);
                    }

                }
            }
            if (korisnici.Contains(korisnik.KorisnickoIme)) {
                return RedirectToAction("Create");
            }
            if (korisnik.Uloga > 2) { return RedirectToAction("Create"); }
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection())) {
                using (SqlCommand cmd = new SqlCommand
                    ("INSERT INTO Korisnici(KorisnickoIme,Sifra,Ime,Prezime,Uloga,Email) VALUES('"+korisnik.KorisnickoIme+"','"+korisnik.Sifra+"','"+korisnik.Ime+"','"+korisnik.Prezime+"','"+korisnik.Uloga+ "','"+korisnik.Email+"') ", con)) {
                    if (con.State != System.Data.ConnectionState.Open)
                           { con.Open();  }
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("VratiKorisnike");
        }
        public ActionResult VratiKorisnike()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Korisnici", con))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    { con.Open(); }
                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtLetovi = new DataTable();
                    dtLetovi.Load(sdr);
                    foreach (DataRow row in dtLetovi.Rows)
                    {
                        
                        korisnici.Add(
                            new Korisnik
                            {
                                KorisnikID = Convert.ToInt32(row["KorisnikID"]),
                                KorisnickoIme = row["KorisnickoIme"].ToString(),
                                Sifra = row["Sifra"].ToString(),
                                Ime = row["Ime"].ToString(),
                                Prezime = row["Prezime"].ToString(),
                                Uloga = Convert.ToInt32(row["Uloga"]),
                                Email = row["Email"].ToString()
                            }
                            );
                    }

                }
            }

            return View(korisnici);
        }
        public ActionResult Delete(int id)
        {
            if (id < 0) { return HttpNotFound(); }
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE  FROM Korisnici WHERE KorisnikID = '" + id + "' ", con))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }

                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("VratiKorisnike");
            }
        }
    }
}