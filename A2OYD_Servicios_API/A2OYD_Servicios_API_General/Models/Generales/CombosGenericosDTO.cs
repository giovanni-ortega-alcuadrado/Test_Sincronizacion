using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API_General.Models.Generales
{
    public class CombosGenericosDTO
    {
        public int intidpk { get; set; }
        public int? intid { get; set; }
        public int? intiddependencia1 { get; set; }
        public int? intiddependencia2 { get; set; }
        public string strdependencia1 { get; set; }
        public string strdependencia2 { get; set; }
        public string strdescripcion { get; set; }
        public string strretorno { get; set; }
        public string strtopico { get; set; }
        public string strorigen { get; set; }
    }
}
