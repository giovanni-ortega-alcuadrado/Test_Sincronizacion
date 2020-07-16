using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Models.DTO.Parametros.Genericos
{
    public class DireccionDTO
    {
        
        public Int64 intcodigooyd { get; set; }
        public string stridentificacion { get; set; }
        public string strdireccion { get; set; }
        public int intidpais { get; set; }
        public int intidciudad { get; set; }
        public int intiddepartamento { get; set; }
    }
}
