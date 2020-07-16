using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Maestros
{
    public class Especie
    {
        public string codigo { get; set; }
        public string clase { get; set; }
        public string abreviatura  { get; set; }
        public string descripcion { get; set; }
        public string moneda { get; set; }
        public bool estaactivo { get; set; }

    }
}
