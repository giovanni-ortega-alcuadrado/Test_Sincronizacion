using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2OYD_Servicios_API_General.Entidades.Generales;

namespace A2OYD_Servicios_API_General.ContextosDB
{
    public class ContextoDbOyd: DbContext
    {

        public ContextoDbOyd(DbContextOptions<ContextoDbOyd> opciones) : base(opciones)
        {
        }

        #region GENERALES

        public DbQuery<ProductoEtiquetas> ProductoEtiqueta { get; set; }
        public DbQuery<ProductoFuncionalidades> ProductoFuncionalidad { get; set; }
        public DbQuery<MensajesSistema> MensajeSistema { get; set; }
        public DbQuery<ParametrosAplicacion> ParametroAplicacion { get; set; }
        public DbQuery<CombosGenericos> ComboGenerico { get; set; }
        #endregion
    }


}


