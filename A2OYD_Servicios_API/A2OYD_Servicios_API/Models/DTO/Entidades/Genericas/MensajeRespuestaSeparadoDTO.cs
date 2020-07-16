using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2OYD_Servicios_API.Entidades.Genericas;

namespace A2OYD_Servicios_API.Models.DTO.Entidades.Genericas
{
    public class MensajeRespuestaSeparadoDTO
    {
        public int cantidaderrores { get; set; }
        public int cantidadexitosos { get; set; }
        public List<MensajeRespuesta> errores { get; set; }
        public List<MensajeRespuesta> exitosos { get; set; }
    }
}
