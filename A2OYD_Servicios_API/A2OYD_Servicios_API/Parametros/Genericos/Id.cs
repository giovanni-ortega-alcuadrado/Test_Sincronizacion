using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Genericos
{
    public class Id
    {
        [Required]
        public int? intid { get; set; }
    }
}
