using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Personas
{
    public class PutClientesCuentasBancarias
    {
        [Required]
        public int intidcuenta { get; set; }
        public Int64? intcodigooyd { get; set; }
        public string strtipocuenta { get; set; }
        public int? intcodigobanco { get; set; }
        public string intnumerocuenta { get; set; }
        //public PostClientesCuentasBancarias datosmodificar { get; set; } // se comenta porque por ahora a los sp no se les pueden enviar datos complejos
    }
}
