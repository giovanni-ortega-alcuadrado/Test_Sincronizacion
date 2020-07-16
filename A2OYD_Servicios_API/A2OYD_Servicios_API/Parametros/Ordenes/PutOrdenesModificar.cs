using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace A2OYD_Servicios_API.Parametros.Ordenes
{
    public class PutOrdenesModificar:GetOrdenesGastos
    {
        [Required]
        public int intnumeroorden { get; set; }
        [Required]
        public string strclaseorden { get; set; }

        //public PostOrdenesIngresar datosmodificar { get; set; } // se comenta porque por ahora a los sp no se les pueden enviar datos complejos
    }
}
