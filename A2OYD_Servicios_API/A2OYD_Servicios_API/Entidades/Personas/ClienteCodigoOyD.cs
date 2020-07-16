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
        public string estadocivilcodigo { get; set; }
        public DateTime? fechanacimiento { get; set; }
        public string lugarnacimiento { get; set; }
        public int lugarnacimientocodigo { get; set; }
        public string sexo { get; set; }
        private string _direccionresidenciastring;
        public string direccionresidencia
        {
            get
            {
                return this._direccionresidenciastring;
            }
            set
            {                
                this._direccionresidenciastring = value;
                if (direccionresidencia != null)
                {
                    direccionresidencialista = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DireccionPersona>>(_direccionresidenciastring);
                }
                //identiicacion = this._direccionresidenciastring;

            }
        }
        public List<DireccionPersona> direccionresidencialista { get; set; }
        public bool? activo { get; set; }
        public string actividadeconomica { get; set; }
        public string codigociiu{ get; set; }
        public bool? personajuridica { get; set; }
        public string politicamenteexpuesto { get; set; }
        public string politicamenteexpuestocodigo { get; set; }
        public DateTime? fechacreacion { get; set; }
        public string telefono { get; set; }
        public string tipovinculacion { get; set; }
        public string tipovinculacioncodigo { get; set; }
        public string email { get; set; }
        public string celular { get; set; }

        /// <summary>
        /// metodo que se ejecuta si en el set de la propiedad direccionresidencia no se asigno valor a la lista
        /// </summary>
        public void Asociar_Lista_Direccion()
        {
            if (direccionresidencia != null)
            {
                direccionresidencialista = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DireccionPersona>>(direccionresidencia);
            }
            
        }

    }
}
