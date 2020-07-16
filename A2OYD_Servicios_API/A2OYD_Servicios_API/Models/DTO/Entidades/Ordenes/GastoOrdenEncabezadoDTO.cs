using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2OYD_Servicios_API.Entidades.Ordenes;

namespace A2OYD_Servicios_API.Models.DTO.Entidades.Ordenes
{
    public class GastoOrdenEncabezadoDTO
    {

            public List<GastoOrdenDetalle> costos { get; set; }
            public int id { get; set; }
            public decimal? valorbruto { get; set; }
            public decimal? valorneto { get; set; }

    }
}
