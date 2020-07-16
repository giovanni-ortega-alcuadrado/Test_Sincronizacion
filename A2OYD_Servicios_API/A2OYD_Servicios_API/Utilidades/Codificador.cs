using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
namespace A2OYD_Servicios_API.Utilidades
{
    public class Codificador : JsonOutputFormatter
    {
        string coding;
        public Codificador(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool,string encoding) 
            : base(serializerSettings, charPool)
        {
            coding = encoding;
        }

        public override Encoding SelectCharacterEncoding(OutputFormatterWriteContext context)
        {
            if (coding == "UTF-16")
            {
                return Encoding.Unicode;
            }
            else if (coding == "UTF-32")
            {
                return Encoding.UTF32;
            }
            else if (coding == "ASCII")
            {
                return Encoding.ASCII;
            }
            else
            {
                return Encoding.UTF8;
            }

        }
    }
}
