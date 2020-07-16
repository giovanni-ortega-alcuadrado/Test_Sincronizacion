using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Genericas
{
    public class MensajeRespuesta
    {
        public string mensaje { get; set; }
        public string proceso { get; set; }
        public int? idregistro { get; set; }
        public bool exitoso { get; set; }
    }
}
