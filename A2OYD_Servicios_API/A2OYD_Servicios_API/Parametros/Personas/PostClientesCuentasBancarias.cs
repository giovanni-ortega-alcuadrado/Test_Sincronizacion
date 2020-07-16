using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Personas
{
    public class PostClientesCuentasBancarias
    {
        [Required]
        public Int64? intcodigooyd { get; set; }
        [Required]
        public string strtipocuenta { get; set; }
        [Required]
        public int? intcodigobanco { get; set; }
        [Required]
        public string intnumerocuenta { get; set; }

    }
}
