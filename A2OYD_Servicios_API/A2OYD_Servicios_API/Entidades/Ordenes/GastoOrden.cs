using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Ordenes
{
    public class GastoOrden
    {
        public int id { get; set; }
        public decimal? valorbruto { get; set; }
        public decimal? valorneto { get; set; }
        public int? idencabezado { get; set; }
        public string descripcion { get; set; }
        public decimal? valorcosto { get; set; }

        public void prueba()
        {

        }
    }
}
