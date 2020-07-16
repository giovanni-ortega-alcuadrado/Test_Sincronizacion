using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Personas
{
    public class CuentaBancoAsociada
    {
        public int? idcuenta { get; set; }
        public int? codigobanco  { get; set; }
        public string nombrebanco { get; set; }
        public string numerocuenta { get; set; }
        public string tipocuenta { get; set; }
        public string descripcion { get; set; }
    }
}
