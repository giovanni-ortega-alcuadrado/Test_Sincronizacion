using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API_General.Entidades.Generales
{
    public class ProductoEtiquetas
    {
        public int intid { get; set; }
        public string strnombreforma { get; set; }
        public string strcampo { get; set; }
        public string strtitulo { get; set; }
        public string strtituloalterno { get; set; }
        public string strtooltip { get; set; }
        public string strtooltiporiginal { get; set; }
        public Boolean logcampovisible { get; set; }
        public Boolean logcampohabilitado { get; set; }
        public Boolean logcamporequerido { get; set; }
        public string strvalordefecto { get; set; }
    }
}
