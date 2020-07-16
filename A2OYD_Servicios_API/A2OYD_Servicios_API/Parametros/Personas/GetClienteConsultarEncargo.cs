using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Parametros.Personas
{
    public class GetClienteConsultarEncargo
    {
        [Required]
        public Int64 intcodigooyd { get; set; }
        [Required]
        public string strcodigofondo { get; set; }
    }
}
