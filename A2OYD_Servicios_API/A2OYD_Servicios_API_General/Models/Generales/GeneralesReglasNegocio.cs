using A2OYD_Servicios_API_General.ContextosDB;
using A2OYD_Servicios_API_General.Entidades.Generales;
using A2OYD_Servicios_API_General.Parametros.Generales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API_General.Models.Generales
{
    public class GeneralesReglasNegocio
    {
        private readonly ContextoDbOyd contextobdoyd;

        public GeneralesReglasNegocio(ContextoDbOyd contextobdoyd)
        {
            this.contextobdoyd = contextobdoyd;
        }

        /// <summary>
        /// Proceso para llamar el procedimiento almacenado que consulta la lista de datos de definición de etiquetas y campos
        /// </summary>
        /// <param name="parametros_DefinicionPantalla"></param>
        /// <returns></returns>
        public async Task<List<ProductoEtiquetas>> consultar_DefinicionPantalla(Parametros_DefinicionPantalla parametros_DefinicionPantalla)  
        {
            var retorno = await contextobdoyd.ProductoEtiqueta.FromSql("[PLATAFORMA].[uspA2_Util_CargarDefinicionPantalla] @pstrNombrePantalla, @pstrUsuario, @pstrMaquina, @pstrInfosesion",
                                   new SqlParameter("@pstrNombrePantalla", parametros_DefinicionPantalla.nombrepantalla),
                                   new SqlParameter("@pstrUsuario", parametros_DefinicionPantalla.usuario),
                                   new SqlParameter("@pstrMaquina", parametros_DefinicionPantalla.maquina),
                                   new SqlParameter("@pstrInfosesion", parametros_DefinicionPantalla.infosesion)).ToListAsync();

            return retorno;
        }

        /// <summary>
        /// Proceso para llamar el procedimiento almacenado que consulta la información de la funcionalidad disponible en una pantalla
        /// </summary>
        /// <param name="parametros_FuncionalidadPantalla"></param>
        /// <returns></returns>
        public async Task<List<ProductoFuncionalidades>> consultar_FuncionalidadPantalla(Parametros_FuncionalidadPantalla parametros_FuncionalidadPantalla)
        {
            var retorno = await contextobdoyd.ProductoFuncionalidad.FromSql("[PLATAFORMA].[uspA2_Util_CargarFuncionalidadesPantalla] @pstrNombrePantalla, @pstrUsuario, @pstrMaquina, @pstrInfosesion",
                                 new SqlParameter("@pstrNombrePantalla", parametros_FuncionalidadPantalla.nombrepantalla),
                                 new SqlParameter("@pstrUsuario", parametros_FuncionalidadPantalla.usuario),
                                 new SqlParameter("@pstrMaquina", parametros_FuncionalidadPantalla.maquina),
                                 new SqlParameter("@pstrInfosesion", parametros_FuncionalidadPantalla.infosesion)).ToListAsync();

            return retorno;

        }


        /// <summary>
        /// Proceso para llamar el procedimiento almacenado que consulta la información de los mensajes del sistema
        /// </summary>
        /// <param name="parametros_MensajesSistema"></param>
        /// <returns></returns>
        public async Task<List<MensajesSistema>> consultar_MensajesSistema(Parametros_MensajesSistema parametros_MensajesSistema)
        {
            var retorno = await contextobdoyd.MensajeSistema.FromSql("[PLATAFORMA].[uspA2_Util_CargarMensajesSistema] @pstrGrupo, @pstrUsuario, @pstrMaquina, @pstrInfosesion",
                                 new SqlParameter("@pstrGrupo", parametros_MensajesSistema.grupo),
                                 new SqlParameter("@pstrUsuario", parametros_MensajesSistema.usuario),
                                 new SqlParameter("@pstrMaquina", parametros_MensajesSistema.maquina),
                                 new SqlParameter("@pstrInfosesion", parametros_MensajesSistema.infosesion)).ToListAsync();

            return retorno;

        }


        /// <summary>
        /// Proceso para llamar el procedimiento almacenado que consulta la información de los parámetros de la aplicación
        /// </summary>
        /// <param name="parametros_ParametrosAplicacion"></param>
        /// <returns></returns>
        public async Task<List<ParametrosAplicacion>> consultar_ParametrosAplicacion(Parametros_ParametrosAplicacion parametros_ParametrosAplicacion)
        {
            var retorno = await contextobdoyd.ParametroAplicacion.FromSql("[dbo].[uspA2utils_CargarCombos] @pstrListaNombreCombos, @pstrUsuario",
                                 new SqlParameter("@pstrListaNombreCombos", parametros_ParametrosAplicacion.topico),
                                 new SqlParameter("@pstrUsuario", parametros_ParametrosAplicacion.usuario)).ToListAsync();

            return retorno;

        }


        /// <summary>
        /// Proceso para llamar el procedimiento almacenado que consulta la información de los combosgnéricos de la aplicación
        /// </summary>
        /// <param name="parametros_CombosAplicacion"></param>
        /// <returns></returns>
        public async Task<List<CombosGenericos>> consultar_CombosAplicacion(Parametros_CombosAplicacion parametros_CombosAplicacion)
        {
            var retorno = await contextobdoyd.ComboGenerico.FromSql("[PLATAFORMA].[uspA2_Util_CargaCombosGenerico] @pstrProducto, @pstrCondicionTexto1, @pstrCondicionTexto2, @pstrCondicionEntero1, @pstrCondicionEntero2, @pstrModulo, @pstrUsuario, @pstrInfosesion",
                                 new SqlParameter("@pstrProducto", parametros_CombosAplicacion.producto),
                                 new SqlParameter("@pstrCondicionTexto1", parametros_CombosAplicacion.condiciontexto1),
                                 new SqlParameter("@pstrCondicionTexto2", parametros_CombosAplicacion.condiciontexto2),
                                 new SqlParameter("@pstrCondicionEntero1", parametros_CombosAplicacion.condicionentero1),
                                 new SqlParameter("@pstrCondicionEntero2", parametros_CombosAplicacion.condicionentero2),
                                 new SqlParameter("@pstrModulo", parametros_CombosAplicacion.modulo),
                                 new SqlParameter("@pstrUsuario", parametros_CombosAplicacion.usuario),
                                 new SqlParameter("@pstrInfosesion", parametros_CombosAplicacion.infosesion)).ToListAsync();

            return retorno;

        }

    }
}
