using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Personas
{
    public class GetClientesSaldo
    {
        [Required]
        public Int64? intcodigooyd { get; set; }
        [Required]
        public string strmoneda { get; set; }
        [Required]
        public DateTime? dtmfecha { get; set; }
        [Required]
        public int intplazo { get; set; }
    }
}
