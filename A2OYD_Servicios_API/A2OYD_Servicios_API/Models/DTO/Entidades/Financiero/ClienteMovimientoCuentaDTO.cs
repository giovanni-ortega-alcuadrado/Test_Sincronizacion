using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Models.DTO.Entidades.Financiero
{
    public class ClienteMovimientoCuentaDTO
    {
        public DateTime? fechahora { get; set; }
        public decimal? monto { get; set; }
        public string tipomovimiento { get; set; }
        public bool? movimientomonetario { get; set; }
        public string codigoinstrumento { get; set; }
        public string descripcion { get; set; }
    }
}
