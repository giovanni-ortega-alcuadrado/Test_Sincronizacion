using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Personas
{
    public class DireccionPersona
    {
        public bool principal { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public int codigociudad { get; set; }
        public string departamento { get; set; }
        public int codigodepartamento { get; set; }
        public string pais { get; set; }
        public int codigopais { get; set; }

    }
}
