using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AplikacijaGET.Models
{

    
    public class Let
    {
        public int LetID { get; set; }

        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public String MestoPolaska { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public String MestoDolaska { get; set; }

        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        [DataType(DataType.Date)]
        public DateTime DatumLeta { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public int BrojPresedanja { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public int BrojMestaNaLetu { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public int BrojPraznih { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public bool Stanje { get; set; }
        [Required(ErrorMessage = "Polje mora biti adekvatno popunjeno")]
        public double Cena { get; set; }


    }
}