using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacijaGET.Models
{
    public class LetRezervacija
    {
        public int LetID { get; set; }
        public String MestoPolaska { get; set; }
        public String MestoDolaska { get; set; }
        public DateTime DatumLeta { get; set; }
        public int BrojPresedanja { get; set; }
        public int BrojPraznih { get; set; }
        public bool Stanje { get; set; }
        public double Cena { get; set; }
        public int RezervacijaID { get; set; }
        public int Korisnik { get; set; }
        public bool StatusRezervacije { get; set; }
        public int BrojRezervisanihMesta { get; set; }



    }
}