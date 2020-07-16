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

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace A2OYD_Servicios_API.Controllers.v1
{
    [Route("A2/V1/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FondosController:ControllerBase
    {
        private readonly ContextoDbOyd contextobdoyd;
        private readonly UtilidadesGenericas utilidadesgenericas;
        private readonly IMapper mapper;
        UtilidadesController utilidadcontroller;

        public FondosController(ContextoDbOyd contextobdoyd, UtilidadesGenericas utilidadesgenericas, IMapper mapper)
        {
            this.contextobdoyd = contextobdoyd;
            this.utilidadesgenericas = utilidadesgenericas;
            this.mapper = mapper;
            utilidadcontroller = new UtilidadesController(contextobdoyd, mapper);
        }

        /// <summary>
        /// Ingresar ordenes de fondos
        /// </summary>
        /// <param name="parametros"> 
        /// "intcodigooyd" Codigo oyd del cliente
        /// "dblvalor" Valor de la orden
        /// "intnumeroencargo" Numero del encargo afectado por la orden
        /// "intcodigocartera" Fondo afectado por la orden
        /// "strmoneda" Moneda de la orden
        /// "strtipomovimiento" El codigo del tipo movimiento según el maestro de tipos de movimiento (014:Cancelacion, 013:Retiro, 012:Adicion...etc)
        /// "dtmfecha"  Fecha de ejecucion de la orden
        /// "strformapago"  Forma de pago de la orden, debe corresponder con el maestro de formas de pago (C: Cheque, E: Efectivo...etc)</param>
        /// "strcuentabancaria" Si la forma de pago es transferencia bancaria se debe enviar el numero de la cuenta hacia la cual se realiza la transferecia
        /// "strcodigofondodestino" Si la forma de pago es traslado a otra cartera colectiva se debe enviar el codigo del fondo destino
        /// "intnumeroencargodestino" Si la forma de pago es traslado a otra cartera colectiva se debe enviar el numero del encargo destino
        /// <returns></returns>
        [HttpPost("Ordenes/Ingresar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Post_IngresarOrden([FromBody] Parametros.Fondos.PostFondos parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("FondosController", "A2/Fondos/Ordenes/Ingresar", parametros.ToString(), "Inicio ejecución.");
                Task<string> valorespordefecto = utilidadcontroller.Obtener_Valores_por_Defecto_json("FondosController", "Post_IngresarOrden");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_FondosController_Post_IngresarOrden] @strJsonEnvio, @pstrusuario,@pstraplicacion ",
                    new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                    new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                    new SqlParameter("@pstrusuario", ""),
                    new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("FondosController", "A2/Fondos/Ordenes/Ingresar", mensajes.ToString(), "Finaliza ejecución.");
                return utilidadcontroller.SepararErroresyExitosos(mensajes);
                
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "FondosController", "Ordenes/Ingresar", string.Empty));
            }
        }

        /// <summary>
        /// Consulta el estado de las ordenes
        /// </summary>
        /// <param name="parametros"> "intid" identificador unico de la orden en el sistema</param>
        /// <returns></returns>
        [HttpGet("ConsultarEstado")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Fondos.EstadoOrdenFondosDTO>>> Get_EstadoOrden([FromBody] Parametros.Genericos.Id parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("FondosController", "A2/Fondos/Ordenes/ConsultarEstado", parametros.ToString(), "Inicio ejecución.");
                var ordenes = await contextobdoyd.estadoordenesfondos.FromSql("[APIADCAP].[usp_FondosController_Get_EstadoOrden] @strJsonEnvio, @pstrusuario,@pstraplicacion ",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("FondosController", "A2/Fondos/Ordenes/ConsultarEstado", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Fondos.EstadoOrdenFondosDTO>>(ordenes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "FondosController", "Ordenes/ConsultarEstado", string.Empty));
            }
        }

        /// <summary>
        /// Consultar los fondos 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarFondosFirma")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Fondos.FondosFirmaDTO>>> Get_FondosFirma()
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/Maestros/ConsultarFormasdePago", "", "Inicio ejecución.");
                var fondosfirma = await contextobdoyd.intodigodescripcion.FromSql("[APIADCAP].[usp_OrdenesController_Get_FondosFirma] @pstrusuario ,@pstraplicacion",
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("MaestrosController", "A2/Maestros/ConsultarFormasdePago", "", "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Fondos.FondosFirmaDTO>>(fondosfirma);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "MaestrosController", "ConsultarFormasdePago", string.Empty));
            }
        }

    }

}
