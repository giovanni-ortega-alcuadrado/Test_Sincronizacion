using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Rentabilidad
{
    public class GetRentabilidad
    {
        [Required]
        public Int64? intcodigooyd { get; set; }
        [Required]
        public DateTime? dtmfechadesde { get; set; }
        [Required]
        public DateTime? dtmfechahasta { get; set; }
        public Int64? intidrecibo { get; set; }
    }

    public class GetClientesRentabilidadInstrumento
    {
        [Required]
        public Int64? intcodigooyd { get; set; }
        [Required]
        public DateTime? dtmfechadesde { get; set; }
        [Required]
        public DateTime? dtmfechahasta { get; set; }
        [Required]
        public string stridespecie { get; set; }
        public Int64? intidrecibo { get; set; }

    }
}
