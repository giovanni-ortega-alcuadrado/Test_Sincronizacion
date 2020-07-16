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
using A2Utilidades.Web.API.Generico.Utilidades;
using A2OYD_Servicios_API.Utilidades;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace A2OYD_Servicios_API.Controllers.v1
{
    [Route("A2/V1/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FinancieroController:ControllerBase
    {
        private readonly ContextoDbOyd contextobdoyd;
        private readonly UtilidadesGenericas utilidadesgenericas;
        private readonly IMapper mapper;
        UtilidadesController utilidadcontroller;

        public FinancieroController(ContextoDbOyd contextobdoyd, UtilidadesGenericas utilidadesgenericas, IMapper mapper)
        {
            this.contextobdoyd = contextobdoyd;
            this.utilidadesgenericas = utilidadesgenericas;
            this.mapper = mapper;
            utilidadcontroller = new UtilidadesController(contextobdoyd, mapper);
        }

        /// <summary>
        /// Consulta operaciones realizadas por un codigo oyd a una fecha agrupadas por especie
        /// </summary>
        /// <param name="parametros"> intcodigooyd: codigo oyd del cliente 
        /// dtmfecha: fecha para la cual se consultan las operaciones</param>
        /// <returns></returns>
        [HttpGet("Cliente/ConsultarTenencia")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Financiero.ClienteTenenciaDTO>>> Get_ClientesTenencia([FromBody] Parametros.Genericos.OydFecha parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("FinancieroController", "A2/Financiero/cliente/ConsultarTenencia", parametros.ToString(), "Inicio ejecución.");
                var clientestenencia1 = await contextobdoyd.clientestenencia.FromSql("[APIADCAP].[usp_FinancieroController_Get_ClientesTenencia] @strJsonEnvio ,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("FinancieroController", "A2/Financiero/cliente/ConsultarTenencia", parametros.ToString(), "Finaliza ejecución.");
                var dto = mapper.Map<List<Models.DTO.Entidades.Financiero.ClienteTenenciaDTO>>(clientestenencia1);
                return dto;
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "FinancieroController", "Cliente/ConsultarTenencia", string.Empty));
            }
        }

        /// <summary>
        /// Consulta el saldo de un codigo oyd a una fecha de corte
        /// </summary>
        /// <param name="parametros"> 
        /// "intcodigooyd" Codigo oyd del cliente
        /// "strmoneda" Codigo de la moneda del portafolio (COP,TRM..etc)
        /// "dtmfecha" Fecha de corte para calcular el saldo
        /// "intplazo" plazo para el calculo del saldo (24,48 o 72 horas)</param>
        /// <returns></returns>
        [HttpGet("Cliente/ConsultarSaldo")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Financiero.ClienteSaldoDTO>>> Get_ClientesSaldo([FromBody] Parametros.Personas.GetClientesSaldo parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("FinancieroController", "A2/Financiero/Cliente/ConsultarSaldo", parametros.ToString(), "Inicio ejecución.");
                var clientessaldo = await contextobdoyd.clientessaldo.FromSql("[APIADCAP].[usp_FinancieroController_Get_ClientesSaldo] @strJsonEnvio ,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("FinancieroController", "A2/Financiero/Cliente/ConsultarSaldo", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Financiero.ClienteSaldoDTO>>(clientessaldo);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "FinancieroController", "Cliente/ConsultarSaldo", string.Empty));
            }
        }

        /// <summary>
        /// Consulta operaciones realizadas por un codigo oyd en un rango de fechas
        /// </summary>
        /// <param name="parametros">
        /// "intcodigooyd" Codigo oyd del cliente
        /// "dtmfechadesde" Feha inicial del rango para consultar las opraciones
        /// "dtmfechahasta" Feha final del rango para consultar las opraciones
        /// "strcodtipomovimiento" El codigo del tipo movimiento según el maestro de tipos de movimiento (001:Compra, 002:Venta, 003:Recibo de caja...etc)</param>
        /// <returns></returns>
        [HttpGet("Cliente/ConsultarMovimientosCuenta")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Financiero.ClienteMovimientoCuentaDTO>>> Get_ClientesMovimientosCuenta([FromBody] Parametros.Personas.GetClientesMovimientosCuenta parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("FinancieroController", "A2/Financiero/Cliente/ConsultarMovimientosCuenta", parametros.ToString(), "Inicio ejecución.");
                var clientesmovimientoscuenta = await contextobdoyd.clientesmovimientoscuenta.FromSql("[APIADCAP].[usp_FinancieroController_Get_ClientesMovimientosCuenta] @strJsonEnvio,@pstrusuario,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("FinancieroController", "A2/Financiero/Cliente/ConsultarMovimientosCuenta", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Financiero.ClienteMovimientoCuentaDTO>>(clientesmovimientoscuenta);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "FinancieroController", "Cliente/ConsultarMovimientosCuenta", string.Empty));
            }
        }

    }
}
