using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace A2OYD_Servicios_API.Parametros.Fondos
{
    public class PostFondos
    {
        [Required]
        public Int64 intcodigooyd { get; set; }
        [Required]
        public decimal dblvalor { get; set; }
        public int intnumeroencargo { get; set; }
        [Required]
        public int intcodigocartera { get; set; }
        [Required]
        public string strmoneda { get; set; }
        [Required]
        public string strtipomovimiento { get; set; }
        [Required]
        public DateTime dtmfecha { get; set; }
        public string strformapago { get; set; }
        public string strcuentabancaria { get; set; }
        public string strcodigofondodestino { get; set; }
        public int intnumeroencargodestino { get; set; }

    }
}
