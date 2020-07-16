using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Ordenes
{
    public class PostOrdenesIngresar: GetOrdenesGastos
    {
        [Required]
        public DateTime dtmfechaingreso{ get; set; }
    }
}
