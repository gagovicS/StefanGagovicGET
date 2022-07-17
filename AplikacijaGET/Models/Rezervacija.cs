using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplikacijaGET.Models
{
    public class Rezervacija
    {
        public int RezervacijaID { get; set; }
        public int Korisnik { get; set; }
        public int Let { get; set; }
        public bool StatusRezervacije { get; set; }
        public int BrojRezervisanihMesta { get; set; }
    }
}