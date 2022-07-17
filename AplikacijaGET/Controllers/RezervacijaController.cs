using AplikacijaGET.Config;
using AplikacijaGET.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplikacijaGET.Controllers
{
    
    public class RezervacijaController : Controller
    {
        // GET: Rezervacija
        public ActionResult vratiRezervacije() {
            List<LetRezervacija> rezervacije = new List<LetRezervacija>();
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("select * FROM Letovi as l  join Rezervacije r ON(l.LetID=r.LetID)", con))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    { con.Open(); }
                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtLetovi = new DataTable();
                    dtLetovi.Load(sdr);
                    foreach (DataRow row in dtLetovi.Rows)
                    {
                        bool r = false;
                        bool s = false;
                        if (Convert.ToInt32(row["StatusRezervacije"]) == 1) { r = true; } 
                        if (Convert.ToInt32(row["Stanje"]) == 1) { s = true; } 
                        rezervacije.Add(
                             new LetRezervacija
                                {
                                LetID = Convert.ToInt32(row["LetID"]),
                                MestoPolaska = row["MestoPolaska"].ToString(),
                                MestoDolaska = row["MestoDolaska"].ToString(),
                                DatumLeta = Convert.ToDateTime(row["DatumLeta"]),
                                BrojPresedanja = Convert.ToInt32(row["BrojPresedanja"]),
                                BrojPraznih = Convert.ToInt32(row["BrojPraznih"]),
                                Stanje = s,
                                Cena = Convert.ToDouble(row["Cena"]),
                                RezervacijaID = Convert.ToInt32(row["RezervacijaID"]),
                                Korisnik = Convert.ToInt32(row["KorisnikID"]),
                                StatusRezervacije = r,
                                BrojRezervisanihMesta = Convert.ToInt32(row["BrojRezervisanihMesta"]),
                                }
                            );
                    }
                }
            }
            return View(rezervacije);
        }
        public ActionResult Delete(int id, int let, int brojMesta, int prazna) {
            if (id < 0) { return HttpNotFound(); }
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE  FROM Rezervacije WHERE RezervacijaID = '" + id + "' ", con))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }

                    cmd.ExecuteNonQuery();
                }
                prazna = prazna + brojMesta;

                string osvezi = "UPDATE Letovi SET BrojPraznih = '" + prazna + "' Where LetID = '" + let + "'";
                
                using (SqlCommand cm = new SqlCommand("UPDATE Letovi SET BrojPraznih = '" + prazna + "' Where LetID = '" +let+"'", con))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    cm.ExecuteNonQuery();
                }
            }
            return RedirectToAction("vratiRezervacije");
        }   
        public ActionResult Edit(int id, bool status,int let,int brojMesta,int prazna ) {

            string prvi = "UPDATE Rezervacije SET StatusRezervacije = '1' Where RezervacijaID = '"+id+"' AND StatusRezervacije = '0'";
            string drugi= "UPDATE Rezervacije SET StatusRezervacije = '0' Where RezervacijaID = '"+id+"' AND StatusRezervacije = '1'";
            if (!status)
            {
                if (brojMesta > prazna) { return RedirectToAction("vratiRezervacije"); }
                using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand(prvi, con))
                    {
                        if (con.State != System.Data.ConnectionState.Open) { con.Open(); }
                        cmd.ExecuteNonQuery();
                    }
                    prazna = prazna - brojMesta;

                    string dodaj = "UPDATE Letovi SET BrojPraznih = '" + prazna + "' Where LetID = '" + let + "'";

                    using (SqlCommand cm = new SqlCommand(dodaj, con))
                    {
                        if (con.State != System.Data.ConnectionState.Open) { con.Open(); }
                        cm.ExecuteNonQuery();
                    }
                }
                     }else{
               
                    prazna = prazna + brojMesta;

                    string osvezi = "UPDATE Letovi SET BrojPraznih = '" + prazna + "' Where LetID = '" + let + "'";

                    using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
                    {
                        using (SqlCommand cmd = new SqlCommand(osvezi, con))
                        {
                            if (con.State != System.Data.ConnectionState.Open) { con.Open(); }
                            cmd.ExecuteNonQuery();
                        }
                    using (SqlCommand cm = new SqlCommand(drugi, con))
                    {
                        if (con.State != System.Data.ConnectionState.Open) { con.Open(); }
                        cm.ExecuteNonQuery();
                    }
                }   
            }
            return RedirectToAction("vratiRezervacije");
        }
        public ActionResult VratiLetove() {
            return RedirectToAction("VratiLetove", "Let");
        }
        public ActionResult Rezervisi( ) {

            int idLet = Convert.ToInt32(TempData["letId"]);
            TempData["let"] = idLet;
            return View();
        }
        [HttpPost]
        public ActionResult Sacuvaj(Rezervacija rez) {
            rez.Let = Convert.ToInt32(TempData["let"]);
            rez.Korisnik = Convert.ToInt32(Session["korisnik"]);
           
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand
                    (" SELECT MAX (RezervacijaID) Rezervacija FROM Rezervacije ", con))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }

                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    if (sdr.HasRows) {
                        dt.Load(sdr);
                        DataRow row = dt.Rows[0];
                        rez.RezervacijaID= Convert.ToInt32(row["Rezervacija"]);
                        rez.RezervacijaID++;
                    }
                }

                using (SqlCommand cmd = new SqlCommand
                    ("INSERT INTO Rezervacije(RezervacijaID,KorisnikID,LetID,StatusRezervacije,BrojRezervisanihMesta) VALUES('" + rez.RezervacijaID + "','" + rez.Korisnik + "','" + rez.Let + "','" + 0 + "','" + rez.BrojRezervisanihMesta + "') ", con))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    { con.Open(); }
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("VratiLetove","Let");
        }
       
     

       
    }
}