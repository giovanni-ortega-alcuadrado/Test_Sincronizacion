using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API_General.Parametros.Generales
{
    public class Parametros_ParametrosAplicacion
    {
        [Required]
        public string topico { get; set; }
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
