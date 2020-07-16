using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Parametros.Ordenes
{
    public class GetOrdenesGastos
    {
        public Int64 intcodigooyd { get; set; }
        public decimal dblnominal { get; set; }
        public decimal dblvalortotal { get; set; }
        public string strnemotecnico { get; set; }
        public string strmoneda { get; set; }
        public string strtipomovimiento { get; set; }
        public decimal dblprecio { get; set; }
        public bool logincluirgastos { get; set; }
        public string strisin { get; set; }
    }
}
