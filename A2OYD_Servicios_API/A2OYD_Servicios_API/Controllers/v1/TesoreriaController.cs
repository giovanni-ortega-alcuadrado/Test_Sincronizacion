using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2OYD_Servicios_API.ContextosDB;
using A2OYD_Servicios_API.Utilidades;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using A2Utilidades.Web.API.Generico.Utilidades;

namespace A2OYD_Servicios_API.Controllers.v1
{
    [Route ("A2/V1/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TesoreriaController:ControllerBase
    {
        private readonly ContextoDbOyd contextobdoyd;
        private readonly UtilidadesGenericas utilidadesgenericas;
        UtilidadesController utilidadcontroller;

        public TesoreriaController(ContextoDbOyd contextobdoyd, UtilidadesGenericas utilidadesgenericas, IMapper mapper)
        {
            this.contextobdoyd = contextobdoyd;
            this.utilidadesgenericas = utilidadesgenericas;
            utilidadcontroller = new UtilidadesController(contextobdoyd, mapper);
        }
        /// <summary>
        /// Ingresar un movimiento credito a la cuenta
        /// </summary>
        /// <param name="parametros">
        /// "intidcodigooyd" Codigo oyd del cliente
        /// "dblvalor" Valor del movimiento
        /// "intidcuenta" Id unico del banco matriculado en el sistema 
        /// </param>
        /// <returns></returns>
        [HttpPost("PostMovimientoCredito")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Post_MovimientoCredito([FromBody] Parametros.Tesoreria.PostMovimientoCuentaBanco parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("TesoreriaController", "A2/V1/Tesoreria/PostMovimientoCredito", parametros.ToString(), "Inicio ejecución.");
                string valorespordefecto = utilidadcontroller.Obtener_Valores_por_Defecto_json("TesoreriaController", "Post_MovimientoCredito");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_TesoreriaController_Post_MovimientoCreditoDebito] @pstrJsonEnvio=@pstrJsonEnvio, @pstrUsuario=@pstrUsuario, @pstrAplicacion=@pstrAplicacion",
                        new SqlParameter("@pstrJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                        new SqlParameter("@pstrUsuario", ""),
                        new SqlParameter("@pstrAplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("TesoreriaController", "A2/V1/Tesoreria/PostMovimientoCredito", mensajes.ToString(), "Finaliza ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "TesoreriaController", "Post_MovimientoCredito", string.Empty));
            }

        }

        /// <summary>
        /// Ingresar un movimiento debito a la cuenta
        /// </summary>
        /// <param name="parametros">
        /// "intidcodigooyd" Codigo oyd del cliente
        /// "dblvalor" Valor del movimiento
        /// "intidcuenta" Id unico del banco matriculado en el sistema 
        /// </param>
        /// <returns></returns>
        [HttpPost("PostMovimientoDebito")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Post_MovimientoDebito([FromBody] Parametros.Tesoreria.PostMovimientoCuentaBanco parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("TesoreriaController", "A2/V1/Tesoreria/PostMovimientoDebito", parametros.ToString(), "Inicio ejecución.");
                string valorespordefecto = utilidadcontroller.Obtener_Valores_por_Defecto_json("TesoreriaController", "Post_MovimientoDebito");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_TesoreriaController_Post_MovimientoCreditoDebito] @pstrJsonEnvio=@pstrJsonEnvio, @pstrUsuario=@pstrUsuario, @pstrAplicacion=@pstrAplicacion",
                        new SqlParameter("@pstrJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                        new SqlParameter("@pstrUsuario", ""),
                        new SqlParameter("@pstrAplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("TesoreriaController", "A2/V1/Tesoreria/PostMovimientoDebito", mensajes.ToString(), "Finaliza ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "TesoreriaController", "Post_MovimientoDebito", string.Empty));
            }
        }
    }
}
