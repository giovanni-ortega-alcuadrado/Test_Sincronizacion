using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Genericas
{
    public class MensajeRespuestaSeparado
    {
        public int cantidaderrores { get; set; }
        public int cantidadexitosos { get; set; }
        public List<MensajeRespuesta> errores { get; set; }
        public List<MensajeRespuesta> exitosos { get; set; }

        public MensajeRespuestaSeparado (List<MensajeRespuesta> errores, List<MensajeRespuesta> exitosos)
        {
            this.errores = errores;
            this.exitosos = exitosos;
        }
    }
}
