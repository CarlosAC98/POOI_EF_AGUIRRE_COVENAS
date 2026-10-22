using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POOI_EF_AGUIRRE_COVEÑAS.Models
{
    public class Consumidor
    {
        [Display(Name = "DNI")]
        public string Dnicont { get; set; }
        
        [Display(Name = "Nombre")]
        public string Nomcont { get; set; }
        
        [Display(Name = "Apellido")]
        public string Apecont { get; set; }
        
        [Display(Name = "Correo")]
        public string Emailcont { get; set; }
        
        [Display(Name = "País")]
        public int Idpais { get; set; }

        public string Nompais { get; set; }

        public Consumidor() { }
        public Consumidor(string dni, string nom, string ape, string email, int pais)
        {
            Dnicont = dni; Nomcont = nom; Apecont = ape; Emailcont = email; Idpais = pais;
        }
    }
}