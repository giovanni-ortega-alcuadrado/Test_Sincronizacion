﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Ordenes
{
    public class GastoOrdenEncabezado
    {
        public List<GastoOrdenDetalle>  costos { get; set; }
        public int id { get; set; }
        public decimal? valorbruto { get; set; }
        public decimal? valorneto { get; set; }

    }
}
