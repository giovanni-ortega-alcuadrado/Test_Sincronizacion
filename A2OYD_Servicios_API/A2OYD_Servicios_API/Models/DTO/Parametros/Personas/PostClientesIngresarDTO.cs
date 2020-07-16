using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Models.DTO.Parametros.Personas
{
    public class PostClientesIngresarDTO
    {
        public bool? logpersonajuridica { get; set; }
        public string strnombre { get; set; }
        public string strapelido { get; set; }
        public string strtipoidentificacion { get; set; }
        public string stridentiicacion { get; set; }
        public string strestadocivil { get; set; }
        public DateTime? dtmfechanacimiento { get; set; }
        public int intlugarnacimiento { get; set; }
        public string strsexo { get; set; }
        public Int64? intcodigooydordenante { get; set; }
        public string stractividadeconomica { get; set; }
        public string stractividadUIF { get; set; }
        public bool? logpoliticamenteexpuesto { get; set; }
        public string telefonocontacto { get; set; }
        public string celularcontacto { get; set; }
        public string emailcontacto { get; set; }
    }
}
