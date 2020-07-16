using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2OYD_Servicios_API.ContextosDB;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using A2OYD_Servicios_API.Models.DTO.Entidades;
using A2Utilidades.Web.API.Generico.Utilidades;
using A2OYD_Servicios_API.Utilidades;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace A2OYD_Servicios_API.Controllers.v1
{
    [Route("A2/V1/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MaestrosController: ControllerBase
    {
        private readonly ContextoDbOyd contextobdoyd ;
        private readonly UtilidadesGenericas utilidadesgenericas;
        private readonly IMapper mapper;
        UtilidadesController utilidadcontroller;

        public MaestrosController (ContextoDbOyd contextobdoyd, UtilidadesGenericas utilidadesgenericas, IMapper mapper)
        {
            this.contextobdoyd = contextobdoyd;
            this.utilidadesgenericas = utilidadesgenericas;
            this.mapper = mapper;
            utilidadcontroller = new UtilidadesController(contextobdoyd, mapper);
        }

        /// <summary>
        /// Consulta el listado de tipos de movimientos disponibles en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarTiposMovimientos")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.TipoMovimientoDTO>>> Get_TiposMovimientos()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/MaestrosController", "", "Inicio ejecución.");
                var movimientos = await contextobdoyd.codigodescripcion.FromSql("[APIADCAP].[usp_MaestrosController_Get_TiposMovimientos] @pstrusuario=@pstrusuario,@pstraplicacion=@pstraplicacion ",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/MaestrosController", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Maestros.TipoMovimientoDTO>>(movimientos);
            }
            catch(Exception  errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarTiposMovimientos", string.Empty));
            }
        }

        /// <summary>
        /// Consulta el listado de monedas parametrizado en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarMonedas")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.MonedaDTO>>> Get_Monedas()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarMonedas", "", "Inicio ejecución.");
                var monedas = await contextobdoyd.codigodescripcion.FromSql("[APIADCAP].[usp_MaestrosController_Get_Monedas]  @pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarMonedas", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Maestros.MonedaDTO>>(monedas);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarMonedas", string.Empty));
            }
        }

        /// <summary>
        /// Consulta el listado de paises parametrizados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarPaises")]
        public async Task< ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.PaisDTO>>> Get_Paises()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarPaises", "", "Inicio ejecución.");
                var paises = await contextobdoyd.paises.FromSql("[APIADCAP].[usp_MaestrosController_Get_Paises] @pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarPaises", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Maestros.PaisDTO>>(paises);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarPaises", string.Empty));
            }
        }

        /// <summary>
        /// Consulta los tipos de documentos manejados por el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarTiposDocumentos")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.TipoDocumentoDTO>>> Get_TiposDocumentos()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarTiposDocumentos", "", "Inicio ejecución.");
                var tiposdocumentos = await contextobdoyd.codigodescripcion.FromSql("[APIADCAP].[usp_MaestrosController_Get_TiposDocumentos] @pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarTiposDocumentos", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Maestros.TipoDocumentoDTO>>(tiposdocumentos);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarTiposDocumentos", string.Empty));
            }
        }

        /// <summary>
        /// Consulta las ciudades perametrizadas en el sistema
        /// </summary>
        /// <param name="parametros"> intcodigooyd: codigo oyd del cliente 
        /// dtmfecha: fecha para la cual se consultan las operaciones</param>
        /// <returns></returns>
        [HttpGet("ConsultarCiudades")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.CiudadDTO>>> Get_Ciudades()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarCiudades", "", "Inicio ejecución.");
                var ciudades = await contextobdoyd.ciudades.FromSql("[APIADCAP].[usp_MaestrosController_Get_Ciudades] @pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarCiudades", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Maestros.CiudadDTO>>(ciudades);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarCiudades", string.Empty));
            }
        }

        /// <summary>
        /// Consultar los departamentos parametrizados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarDepartamentos")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.DepartamentoDTO>>> Get_Departamentos()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarDepartamentos", "", "Inicio ejecución.");
                var departamentos = await contextobdoyd.departamentos.FromSql("[APIADCAP].[usp_MaestrosController_Get_Departamentos] @pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarDepartamentos", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Maestros.DepartamentoDTO>>(departamentos);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarDepartamentos", string.Empty));
            }
        }

        [HttpGet("ConsultarCodigosCIIU")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.CodigoCIIUDTO>>> Get_CodigosCIIU()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarCodigosCIIU", "", "Inicio ejecución.");
                var codigosCIIU = await contextobdoyd.codigodescripcion.FromSql("[APIADCAP].[usp_MaestrosController_Get_CodigosCIIU] @pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarCodigosCIIU", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Maestros.CodigoCIIUDTO>>(codigosCIIU);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarCodigosCIIU", string.Empty));
            }
        }

        /// <summary>
        /// Consultar las formas de pago parametrizados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarFormasdePago")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.FormaPagoDTO>>> Get_FormasPago()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarFormasdePago", "", "Inicio ejecución.");
                var formaspago = await contextobdoyd.codigodescripcion.FromSql("[APIADCAP].[usp_MaestrosController_Get_FormasPago] @pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarFormasdePago", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Maestros.FormaPagoDTO>>(formaspago);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarFormasdePago", string.Empty));
            }
        }

        /// <summary>
        /// Consultar las especies ingresadas al sistema 
        /// </summary>
        /// <param name="parametros"> 
        /// "strcodigo" (opcional) Nemotecnico de la especie
        /// "strclase"  (opcional) Se envía A: Acciones - R: Renta Fija
        /// "strmoneda" (opcional) Codigo de la moneda de la especies (COP,TRM..etc)
        /// "logestaactivo" (opcional) indica si se muestran especies activas o inactivas</param>
        /// <returns></returns>
        [HttpGet("ConsultarEspecies")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Maestros.EspecieDTO>>> Get_Especies([FromBody] Parametros.Maestros.GetEspecies parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("FinancieroController", "A2/V1/Maestros/ConsultarEspecies", parametros.ToString(), "Inicio ejecución.");
                var especies = await contextobdoyd.especies.FromSql("[APIADCAP].[usp_MaestrosController_Get_Especies] @pstrJsonEnvio=@pstrJsonEnvio ,@pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("FinancieroController", "A2/V1/Maestros/ConsultarEspecies", parametros.ToString(), "Finaliza ejecución.");
                var dto = mapper.Map<List<Models.DTO.Entidades.Maestros.EspecieDTO>>(especies);
                return dto;
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarEspecies", string.Empty));
            }
        }

        /// <summary>
        /// Consulta los tipos de documentos manejados por el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarEstadosCivil")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Genericas.CodigoDescripcionDTO>>> Get_EstadosCivil()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarTiposDocumentos", "", "Inicio ejecución.");
                var tiposdocumentos = await contextobdoyd.codigodescripcion.FromSql("[APIADCAP].[usp_MaestrosController_Get_EstadosCivil] @pstrusuario=@pstrusuario ,@pstraplicacion=@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/V1/Maestros/ConsultarTiposDocumentos", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Genericas.CodigoDescripcionDTO>>(tiposdocumentos);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarTiposDocumentos", string.Empty));
            }
        }

    }
}
