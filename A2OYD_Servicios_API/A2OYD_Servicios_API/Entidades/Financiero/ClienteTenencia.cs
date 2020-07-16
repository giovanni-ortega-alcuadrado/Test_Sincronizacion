using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Financiero
{
    public class ClienteTenencia
    {
        public string codigoinstrumento { get; set; }
        public decimal? cantidad { get; set; }
        public decimal? cantidadcomprometida { get; set; }
        public decimal? cantidaddisponible { get; set; }
        public DateTime? fecha { get; set; }
        public string codigomoneda { get; set; }
        public decimal? preciovaloracion { get; set; }
        public decimal? vpn { get; set; }
    }
}
