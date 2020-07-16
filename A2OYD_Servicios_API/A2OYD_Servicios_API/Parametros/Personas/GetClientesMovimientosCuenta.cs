using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Personas
{
    public class GetClientesMovimientosCuenta
    {
        [Required]
        public Int64? intcodigooyd { get; set; }
        [Required]
        public DateTime? dtmfechadesde { get; set; }
        [Required]
        public DateTime? dtmfechahasta { get; set; }
        public string strcodtipomovimiento { get; set; }
    }
}
