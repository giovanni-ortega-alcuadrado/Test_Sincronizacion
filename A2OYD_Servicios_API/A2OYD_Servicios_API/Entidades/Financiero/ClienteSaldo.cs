using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Entidades.Financiero
{
    public class ClienteSaldo
    {
        public string moneda { get; set; }
        public decimal? saldo { get; set; }
        public int plazo { get; set; }
        public bool? disponible { get; set; }
    }
}
