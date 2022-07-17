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
    public class LetController : Controller
    {
       
       
        // GET: Let
        public ActionResult Create()
        {
            
            return View(new Let {LetID=0 });
        }

        [HttpPost]
        public ActionResult Create(Let let)
        {
            if (ModelState.IsValid)
            {

                int stanje = 0;
                if (let.Stanje) { stanje = 1; }
                DateTime myDateTime = let.DatumLeta;
                string sqlFormattedDate = myDateTime.ToString("d");

                string insertSQL = "INSERT INTO Letovi(MestoPolaska,MestoDolaska,DatumLeta,BrojPresedanja,BrojMestaNaLetu,BrojPraznih,Stanje,Cena) VALUES('" + let.MestoPolaska + "', '" + let.MestoDolaska + "','" + sqlFormattedDate + "','" + let.BrojPresedanja + "','" + let.BrojMestaNaLetu + "','" + let.BrojPraznih + "','" + stanje + "','" + let.Cena + "')";
                string updateSQL = "UPDATE Letovi SET MestoPolaska = '" + let.MestoPolaska + "',MestoDolaska= '" + let.MestoDolaska + "', DatumLeta= '" + sqlFormattedDate + "',BrojPresedanja= '" + let.BrojPresedanja + "',BrojMestaNaLetu= '" + let.BrojMestaNaLetu + "',BrojPraznih= '" + let.BrojPraznih + "', Stanje= '" + stanje + "', Cena= '" + let.Cena + "' WHERE LetID=  '" + let.LetID + "' ";


                using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand
                   ((let.LetID > 0) ? updateSQL : insertSQL, con))
                    {
                        if (con.State != System.Data.ConnectionState.Open)
                        { con.Open(); }
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("VratiLetove");
            }
            return View("Create", let);
        }
        public ActionResult VratiLetove()
        {
                List<Let> letovi = new List<Let>();

                using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand("select* from Letovi order by DatumLeta desc", con))
                    {
                        if (con.State != System.Data.ConnectionState.Open)
                        { con.Open(); }
                        SqlDataReader sdr = cmd.ExecuteReader();

                        DataTable dtLetovi = new DataTable();
                        dtLetovi.Load(sdr);
                        foreach (DataRow row in dtLetovi.Rows)
                        {
                            bool r = false;
                            if (Convert.ToInt32(row["Stanje"]) == 1) { r = true; }

                            letovi.Add(
                                new Let
                                {
                                    LetID = Convert.ToInt32(row["LetID"]),
                                    MestoPolaska = row["MestoPolaska"].ToString(),
                                    MestoDolaska = row["MestoDolaska"].ToString(),
                                    DatumLeta = Convert.ToDateTime(row["DatumLeta"]),
                                    BrojPresedanja = Convert.ToInt32(row["BrojPresedanja"]),
                                    BrojMestaNaLetu = Convert.ToInt32(row["BrojMestaNaLetu"]),
                                    BrojPraznih = Convert.ToInt32(row["BrojPraznih"]),
                                    Stanje = r,
                                    Cena = Convert.ToDouble(row["Cena"])
                                }
                                );
                        }

                    }
                }
                return View(letovi);

           
           
        }
        
        [HttpPost]
        public ActionResult VratiFiltrirane( string polazak, string dolazak, string presedanje)

        {
                List<Let> letovi = new List<Let>();
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Letovi", con))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    { con.Open(); }
                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtLetovi = new DataTable();
                    dtLetovi.Load(sdr);
                    if (presedanje.Equals("Bez Presedanja"))
                    {
                        foreach (DataRow row in dtLetovi.Rows)
                        {
                            bool r = false;
                            if (Convert.ToInt32(row["Stanje"]) == 1) { r = true; }
                            if (polazak.Equals(row["MestoPolaska"].ToString()) && dolazak.Equals(row["MestoDolaska"].ToString()) && Convert.ToInt32(row["BrojPraznih"]) > 0 && Convert.ToInt32(row["BrojPresedanja"])==0)
                            {
                                letovi.Add(
                                new Let
                                {
                                    LetID = Convert.ToInt32(row["LetID"]),
                                    MestoPolaska = row["MestoPolaska"].ToString(),
                                    MestoDolaska = row["MestoDolaska"].ToString(),
                                    DatumLeta = Convert.ToDateTime(row["DatumLeta"]),
                                    BrojPresedanja = Convert.ToInt32(row["BrojPresedanja"]),
                                    BrojMestaNaLetu = Convert.ToInt32(row["BrojMestaNaLetu"]),
                                    BrojPraznih = Convert.ToInt32(row["BrojPraznih"]),
                                    Stanje = r,
                                    Cena = Convert.ToDouble(row["Cena"])
                                }
                                );
                            }
                        }
                    }
                    else {
                        foreach (DataRow row in dtLetovi.Rows)
                        {
                            bool r = false;
                            if (Convert.ToInt32(row["Stanje"]) == 1) { r = true; }
                            if (polazak.Equals(row["MestoPolaska"].ToString()) && dolazak.Equals(row["MestoDolaska"].ToString()) && Convert.ToInt32(row["BrojPraznih"]) > 0)
                            {
                                letovi.Add(
                                new Let
                                {
                                    LetID = Convert.ToInt32(row["LetID"]),
                                    MestoPolaska = row["MestoPolaska"].ToString(),
                                    MestoDolaska = row["MestoDolaska"].ToString(),
                                    DatumLeta = Convert.ToDateTime(row["DatumLeta"]),
                                    BrojPresedanja = Convert.ToInt32(row["BrojPresedanja"]),
                                    BrojMestaNaLetu = Convert.ToInt32(row["BrojMestaNaLetu"]),
                                    BrojPraznih = Convert.ToInt32(row["BrojPraznih"]),
                                    Stanje = r,
                                    Cena = Convert.ToDouble(row["Cena"])
                                }
                                );


                            }
                        }
                    }
                }
            }
            return View("VratiLetove",letovi);
        }
        public ActionResult Delete(int id)
        {
            if (id < 1) { return HttpNotFound(); }
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE  FROM Letovi WHERE LetID = '"+id+ "' ", con))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }

                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("VratiLetove");
            }
        }
        public ActionResult Edit(int id)
        {
            if (id < 1) { return HttpNotFound(); }
            var _let = new Let();
            using (SqlConnection con = new SqlConnection(Konekcija.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Letovi WHERE LetID = '" + id + "' ", con))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }

                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    if (sdr.HasRows) 
                    { dt.Load(sdr);
                        DataRow row = dt.Rows[0];
                        bool r = false;
                        if (Convert.ToInt32(row["Stanje"]) == 1) { r = true; }

                        _let.LetID = Convert.ToInt32(row["LetID"]);
                        _let.MestoPolaska = row["MestoPolaska"].ToString();
                        _let.MestoDolaska = row["MestoDolaska"].ToString();
                        _let.DatumLeta = Convert.ToDateTime(row["DatumLeta"]);
                        _let.BrojPresedanja = Convert.ToInt32(row["BrojPresedanja"]);
                        _let.BrojMestaNaLetu = Convert.ToInt32(row["BrojMestaNaLetu"]);
                        _let.BrojPraznih = Convert.ToInt32(row["BrojPraznih"]);
                        _let.Stanje = r;
                        _let.Cena = Convert.ToDouble(row["Cena"]);
                        return View("Create", _let);
                    } 
                    else { return HttpNotFound(); }
                }
            }
        }
        public ActionResult Rezervisi(int id) {
            int letId = id;
            TempData["letId"] = id;
          return  RedirectToAction("Rezervisi", "Rezervacija");
        }
    }

       
}