using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Maestros
{
    public class Ciudad
    {
        /// <summary>
        /// id unico del registro en el sistema
        /// </summary>
        public int idciudad { get; set; }
        public string nombre { get; set; }
        public  string departamento { get; set; }
        public string codigodane { get; set; }
    }
}
