using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2Utilidades.Web.API.Generico.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using A2OYD_Servicios_API_General.Parametros.Generales;
using Microsoft.EntityFrameworkCore;
using A2OYD_Servicios_API_General.Entidades.Generales;
using A2OYD_Servicios_API_General.Models.Generales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace A2OYD_Servicios_API_General.Controllers.v1
{
    [Route("A2General/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class GeneralesController : ControllerBase
    {
        private readonly UtilidadesGenericas objUtilidades;
        private readonly IMapper mapper;
        private readonly GeneralesReglasNegocio reglasNegocio;
        public IConfiguration Configuration { get; }

        public GeneralesController( UtilidadesGenericas objUtilidad, IMapper mapper, GeneralesReglasNegocio reglasNegocio, IConfiguration configuration)
        {
            this.objUtilidades = objUtilidad;
            this.mapper = mapper;
            this.reglasNegocio = reglasNegocio;
            this.Configuration = configuration;
        }
        /// <summary>
        /// Devuelve los datos de la defnición de etiquetas de las pantallas
        /// </summary>
        /// <param name="parametros_DefinicionPantalla">Datos necesarios para la consulta</param>
        /// <returns>Lista de definición de datos y etiquetas de la pantalla</returns>
        [HttpPost("Retornar_DefinicionPantalla")]
        public async Task<ActionResult<IEnumerable<ProductoEtiquetasDTO>>> Retornar_DefinicionPantalla([FromBody] Parametros_DefinicionPantalla parametros_DefinicionPantalla)
        {
            try
            {
                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_DefinicionPantalla", parametros_DefinicionPantalla.ToString(), "Inicio ejecución.");

                List<ProductoEtiquetas> lstProductosEtiquetas = await reglasNegocio.consultar_DefinicionPantalla(parametros_DefinicionPantalla);

                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_DefinicionPantalla", parametros_DefinicionPantalla.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<ProductoEtiquetasDTO>>(lstProductosEtiquetas);
            }
            catch (Exception ex)
            {
                return BadRequest(objUtilidades.RetornarErrorControlado(ex, "GeneralesController", "Retornar_DefinicionPantalla", parametros_DefinicionPantalla.ToString()));
            }
        }

        /// <summary>
        /// Devuelve los datos de la funcionalidad de la pantalla
        /// </summary>
        /// <param name="parametros_FuncionalidadPantalla">Datos necesarios para la consulta</param>
        /// <returns>Lista de la configuración de la funcionalidad de la pantalla</returns>
        [HttpPost("Retornar_FuncionalidadPantalla")]
        public async Task<ActionResult<IEnumerable<Models.Generales.ProductoFuncionalidadesDTO>>> Retornar_FuncionalidadPantalla([FromBody] Parametros_FuncionalidadPantalla parametros_FuncionalidadPantalla)
        {
            try
            {
                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_FuncionalidadPantalla", parametros_FuncionalidadPantalla.ToString(), "Inicio ejecución.");

                List<ProductoFuncionalidades> lstProductoFuncionalidades = await reglasNegocio.consultar_FuncionalidadPantalla(parametros_FuncionalidadPantalla);

                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_FuncionalidadPantalla", parametros_FuncionalidadPantalla.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.Generales.ProductoFuncionalidadesDTO>>(lstProductoFuncionalidades);
            }
            catch (Exception ex)
            {
                return BadRequest(objUtilidades.RetornarErrorControlado(ex, "GeneralesController", "Retornar_FuncionalidadPantalla", parametros_FuncionalidadPantalla.ToString()));
            }
        }

        /// <summary>
        /// Devuelve los datos de los mensajes del sistema
        /// </summary>
        /// <param name="parametros_MensajesSistema">Datos necesarios para la consulta</param>
        /// <returns>Lista de mensajes del sistema</returns>
        [HttpPost("Retornar_MensajesSistema")]
        public async Task<ActionResult<IEnumerable<Models.Generales.MensajesSistemaDTO>>> Retornar_MensajesSistema([FromBody] Parametros_MensajesSistema parametros_MensajesSistema)
        {
            try
            {
                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_MensajesSistema", parametros_MensajesSistema.ToString(), "Inicio ejecución.");

                List<MensajesSistema> lstMensajesSistema = await reglasNegocio.consultar_MensajesSistema(parametros_MensajesSistema);

                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_MensajesSistema", parametros_MensajesSistema.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.Generales.MensajesSistemaDTO>>(lstMensajesSistema);
            }
            catch (Exception ex)
            {
                return BadRequest(objUtilidades.RetornarErrorControlado(ex, "GeneralesController", "Retornar_MensajesSistema", parametros_MensajesSistema.ToString()));
            }
        }


        /// <summary>
        /// Devuelve los datos de los parámetros de la aplicación
        /// </summary>
        /// <param name="parametros_ParametrosAplicacion">Datos necesarios para la consulta</param>
        /// <returns>Lista de mensajes del sistema</returns>
        [HttpPost("Retornar_ParametrosAplicacion")]
        public async Task<ActionResult<IEnumerable<Models.Generales.ParametrosAplicacionDTO>>> Retornar_ParametrosAplicacion([FromBody] Parametros_ParametrosAplicacion parametros_ParametrosAplicacion)
        {
            try
            {
                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_ParametrosAplicacion", parametros_ParametrosAplicacion.ToString(), "Inicio ejecución.");

                List<ParametrosAplicacion> lstParametrosAplicacion = await reglasNegocio.consultar_ParametrosAplicacion(parametros_ParametrosAplicacion);

                lstParametrosAplicacion?.Add(new ParametrosAplicacion() { categoria = parametros_ParametrosAplicacion.topico, descripcion = A2Utilidades.CifrarSL.cifrar(A2Utilidades.Cifrar.descifrar(Configuration.GetConnectionString("dbOYDConnectionString"))), id = "A2Consola_CnxBaseDatos".ToUpper() });
                lstParametrosAplicacion?.Add(new ParametrosAplicacion() { categoria = parametros_ParametrosAplicacion.topico, descripcion = A2Utilidades.CifrarSL.cifrar(Configuration["ConfiguracionParametros:DirectorioArchivosUpload"]), id = "A2Consola_CarpetaUploads".ToUpper() });

                string strRutaDirectorioAssembly = Path.Combine(Configuration["ConfiguracionParametros:DirectorioBaseServiciosRIA"], "bin");

                String[] ArrayArchivos = null; 

                try
                { 
                  ArrayArchivos = Directory.GetFiles(strRutaDirectorioAssembly);
                }
                catch (Exception ex)
                {
                    objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_ParametrosAplicacion", parametros_ParametrosAplicacion.ToString(), ex.Message);
                }

                if (ArrayArchivos != null)
                {
                    foreach (string objArchivo in ArrayArchivos)
                    {
                        FileInfo objInfoArchivoCompleta = new FileInfo(objArchivo);
                        if (objInfoArchivoCompleta.Name.ToLower().Contains("a2"))
                        {
                            if (objInfoArchivoCompleta.Extension.ToLower() == ".dll")
                            {

                                System.Reflection.Assembly objInfoArchivo = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(objArchivo));

                                if (objInfoArchivo != null)
                                {
                                    lstParametrosAplicacion?.Add(new ParametrosAplicacion() { categoria = "DLLSERVICIORIA", descripcion = objInfoArchivo.GetName().Version.ToString(), id = objInfoArchivoCompleta.Name });

                                }
                            }
                        }
                    }
                }

                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_ParametrosAplicacion", parametros_ParametrosAplicacion.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.Generales.ParametrosAplicacionDTO>>(lstParametrosAplicacion);

            }
            catch (Exception ex)
            {
                return BadRequest(objUtilidades.RetornarErrorControlado(ex, "GeneralesController", "Retornar_ParametrosAplicacion", parametros_ParametrosAplicacion.ToString()));
            }
        }


        /// <summary>
        /// Devuelve los datos de los mensajes del sistema
        /// </summary>
        /// <param name="parametros_CombosAplicacion">Datos necesarios para la consulta</param>
        /// <returns>Lista de mensajes del sistema</returns>
        [HttpPost("Retornar_CombosAplicacion")]
        public async Task<ActionResult<IEnumerable<Models.Generales.CombosGenericosDTO>>> Retornar_CombosAplicacion([FromBody] Parametros_CombosAplicacion parametros_CombosAplicacion)
        {
            try
            {
                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_CombosAplicacion", parametros_CombosAplicacion.ToString(), "Inicio ejecución.");

                List<CombosGenericos> lstCombosAplicacion = await reglasNegocio.consultar_CombosAplicacion(parametros_CombosAplicacion);

                objUtilidades.CrearLogSeguimiento("GeneralesController", "Retornar_CombosAplicacion", parametros_CombosAplicacion.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.Generales.CombosGenericosDTO>>(lstCombosAplicacion);
            }
            catch (Exception ex)
            {
                return BadRequest(objUtilidades.RetornarErrorControlado(ex, "GeneralesController", "Retornar_CombosAplicacion", parametros_CombosAplicacion.ToString()));
            }
        }

    }
}