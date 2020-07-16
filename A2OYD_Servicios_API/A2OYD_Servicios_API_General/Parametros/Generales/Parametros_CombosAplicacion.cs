using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API_General.Parametros.Generales
{
    public class Parametros_CombosAplicacion
    {

        public string producto { get; set; }

        public string condiciontexto1 { get; set; }

        public string condiciontexto2 { get; set; }

        public int? condicionentero1 { get; set; }

        public int? condicionentero2 { get; set; }

        public string modulo { get; set; }

        [Required]
        public string usuario { get; set; }
        [Required]
        public string infosesion { get; set; }
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

    }
}
