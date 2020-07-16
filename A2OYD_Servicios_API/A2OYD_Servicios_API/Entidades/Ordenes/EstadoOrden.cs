using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Ordenes
{
    public class EstadoOrden
    {
        public string estado { get; set; }
        public DateTime? fechaliquidacion { get; set; }
        public int? numeroliquidacion { get; set; }
        public decimal? nominalliquidacion { get; set; }
        public decimal? nominalorden { get; set; }
        public string nemotecnico { get; set; }
        public string moneda { get; set; }
        public string tipooperacion { get; set; }
        public decimal? precio { get; set; }
        public bool? incluirgastos { get; set; }
        public DateTime? fechaorden { get; set; }
    }
}
