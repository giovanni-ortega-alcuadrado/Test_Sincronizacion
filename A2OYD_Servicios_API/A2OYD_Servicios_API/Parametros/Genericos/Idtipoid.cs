using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Genericos
{
    public class Idtipoid
    {
        [Required]
        public string stridentificacion { get; set; }
        [Required]
        public string strtipoidentificacion { get; set; }
    }
}
