using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Fondos
{
    public class EstadoOrdenFondos
    {
        public Int64? codigooyd { get; set; }
        public string estado { get; set; }
        public DateTime? fechaejecucion { get; set; }
        public decimal? cantidad { get; set; }
        public string moneda { get; set; }
        public string tipomovimiento { get; set; }
    }
}
