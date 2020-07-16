using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Personas
{
    public class ClienteDatosCuenta
    {
        public string tiporegistro { get; set; }
        public Int64? codigooyd { get; set; }
        public string estado { get; set; }
        public string tipovinculacion { get; set; }
        public string nombreasociada { get; set; }
        public string apellidoasociada { get; set; }
        public string identificacionasociada { get; set; }
        public string tipoidentificacionasociada { get; set; }
        public string tiporelacion { get; set; }
        public int? idcuenta { get; set; }
        public int? codigobanco { get; set; }
        public string nombrebanco { get; set; }
        public string numerocuentabanco { get; set; }
        public string tipocuentabanco { get; set; }
        public string descripcioncuentabanco { get; set; }
        public string codigomercado { get; set; }
        public string descripcionmercado { get; set; }
    }
}
