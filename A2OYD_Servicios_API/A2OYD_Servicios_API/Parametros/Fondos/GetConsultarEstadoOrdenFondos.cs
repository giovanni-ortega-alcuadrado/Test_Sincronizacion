using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Parametros.Fondos
{
    public class GetConsultarEstadoOrdenFondos
    {
        public int? intid { get; set; }
        public DateTime? dtmfechainicial { get; set; }
        public DateTime? dtmfechafinal { get; set; }
    }
}
