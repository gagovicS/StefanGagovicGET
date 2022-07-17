using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AplikacijaGET.Config
{
    public class Konekcija
    {
        public static string GetConnection() {

            return ConfigurationManager.ConnectionStrings["GetBaza"].ConnectionString;
        }
    }
}