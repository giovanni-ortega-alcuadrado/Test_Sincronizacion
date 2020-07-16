using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2OYD_Servicios_API.Entidades.Personas;

namespace A2OYD_Servicios_API.Models.DTO.Entidades.Personas
{
    public class ClienteDatosCuentaDetalleDTO
    {
        public Int64? codigooyd { get; set; }
        public string estado { get; set; }
        public string tipovinculacion { get; set; }
        public List<PersonaAsociada> personasasociadas { get; set; }
        public List<CuentaBancoAsociada> cuentasbancarias { get; set; }
        public List<MercadoAsociado> mercadosoperacion { get; set; }
    }
}
