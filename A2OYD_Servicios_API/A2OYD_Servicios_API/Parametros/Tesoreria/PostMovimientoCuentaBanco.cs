using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Tesoreria
{
    public class PostMovimientoCuentaBanco
    {
        [Required]
        public Int64 intidcodigooyd { get; set; }
        [Required]
        public decimal dblvalor { get; set; }
        [Required]
        public int intidcuenta { get; set; }
        [Required]
        public bool logescredito { get; set; }
    }
}
