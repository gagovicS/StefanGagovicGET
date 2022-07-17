using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplikacijaGET.Models
{   
    
    public class Korisnik
    {
        public int KorisnikID { get; set; }

        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public string KorisnickoIme { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        [DataType(DataType.Password)]
        public string Sifra { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        
        public string Ime { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public string Prezime { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public int Uloga { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}