using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Models.DTO.Parametros.Personas
{
    public class PutClientesModificarDTO:PostClientesIngresarDTO
    {
        public Int64 Intidcodigooyd { get; set; }
    }

}
