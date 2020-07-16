using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Personas
{
    public class ClienteCodigoOyD
    {
        public Int64 codigooyd { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string identiicacion { get; set; }
        public string estadocivil { get; set; }
        public DateTime? fechanacimiento { get; set; }
        public string lugarnacimiento { get; set; }
        public string sexo { get; set; }
        public string direccionresidencia { get; set; }
        public bool? activo { get; set; }
        public string actividadeconomica { get; set; }
        public string codigociiu{ get; set; }
        public bool? personajuridica { get; set; }
        public string politicamenteexpuesto { get; set; }
        public DateTime? fechacreacion { get; set; }
        public string telefono { get; set; }
        public string tipovinculacion { get; set; }
        public string email { get; set; }

    }
}
