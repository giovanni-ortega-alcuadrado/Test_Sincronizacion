using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Genericos
{
    public class Direccion
    {
        [Required]
        public string strdireccion { get; set; }
        public int intidpais { get; set; }
        public int intidciudad { get; set; }
        public int intiddepartamento { get; set; }
    }
}
