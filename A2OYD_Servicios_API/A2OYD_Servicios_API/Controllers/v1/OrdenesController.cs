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
    public class OrdenesController : ControllerBase
    {
        private readonly ContextoDbOyd contextobdoyd;
        private readonly UtilidadesGenericas utilidadesgenericas;
        private readonly IMapper mapper;
        UtilidadesController utilidadcontroller;

        public OrdenesController(ContextoDbOyd contextobdoyd, UtilidadesGenericas utilidadesgenericas, IMapper mapper)
        {
            this.contextobdoyd = contextobdoyd;
            this.utilidadesgenericas = utilidadesgenericas;
            this.mapper = mapper;
            utilidadcontroller = new UtilidadesController(contextobdoyd, mapper);
        }
        /// <summary>
        /// Consulta los gastos asociados a la orden (Iva,Comision)
        /// </summary>
        /// <param name="parametros">
        /// "intcodigooyd" Codigo oyd del cliente
        /// "dblnominal" Cantidad nominal de la orden
        /// "dblvalortotal" Valor de la orden en pesos
        /// "strnemotecnico" Nemotecnico de la especie asociada a la orden
        /// "strmoneda" Codigo de la moneda de la orden, correspondiente con el maestro de monedas (COP, TRM...etc)
        /// "strtipomovimiento"Tipo de movimiento a realizar, correspondiente con el maestro de tipos de movimiento(001:Compra, 002:Venta...etc)
        /// "dblprecio" Precio de mercado de la orden
        /// "logincluirgastos" Indica si en el calculo se incluyen los gastos
        /// "strisin" Isin asociado a la especie de la orden (solo en caso que la especie tenga asociado mas de un isin)</param>
        /// 
        /// <returns>Lista(costos) -- id -- valorbruto -- valorneto</returns>
        [HttpGet("ConsultarGastos")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Ordenes.GastoOrdenEncabezadoDTO>>> Get_OrdenesGastos([FromBody] Parametros.Ordenes.GetOrdenesGastos parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/ConsultarGastos", parametros.ToString(), "Inicio ejecución.");
                var gastosorden = await contextobdoyd.gastosorden.FromSql("[APIADCAP].[usp_OrdenesController_Get_OrdenesGastos] @pstrJsonEnvio=@pstrJsonEnvio, @pstrusuario=@pstrusuario,@pstraplicacion=@pstraplicacion ",
                    new SqlParameter("@pstrJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                    new SqlParameter("@pstrusuario", ""),
                    new SqlParameter("@pstraplicacion", "")).ToListAsync();
                Utilidades.UtilidadesController utilidad = new Utilidades.UtilidadesController();
                List<Entidades.Ordenes.GastoOrdenEncabezado> resultadoseparado = utilidad.GastoOrdenSepararEncabezadoyDetalle(gastosorden);
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/ConsultarGastos", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Ordenes.GastoOrdenEncabezadoDTO>>(resultadoseparado);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "OrdenesController", "ConsultarGastos", string.Empty));
            }
        }

        /// <summary>
        /// Ingresa ordenes al sistema
        /// </summary>
        /// <param name="parametros">
        /// "intcodigooyd"  Codigo oyd del cliente
        /// "dblnominal" decimal; Cantidad nominal de la orden
        /// "dblvalortotal" Valor de la orden en pesos
        /// "strnemotecnico" Nemotecnico de la especie asociada a la orden
        /// "strmoneda" Codigo de la moneda de la orden, correspondiente con el maestro de monedas (COP, TRM...etc)
        /// "strtipomovimiento" Tipo de movimiento a realizar, correspondiente con el maestro de tipos de movimiento(001:Compra, 002:Venta...etc)
        /// "dblprecio" Precio de mercado de la orden
        /// "logincluirgastos" Indica si en el calculo se incluyen los gastos
        /// "strisin" Isin asociado a la especie de la orden (solo en caso que la especie tenga asociado mas de un isin)
        /// "dtmfechaingreso"  Fecha de ingreso de la orden</param>
        /// <returns></returns>
        [HttpPost("Ingresar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Post_Ingresar([FromBody] Parametros.Ordenes.PostOrdenesIngresar parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/Ingresar", parametros.ToString(), "Inicio ejecución.");
                string valorespordefecto = utilidadcontroller.Obtener_Valores_por_Defecto_json("OrdenesController", "Post_Ingresar");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_OrdenesController_Post_Ingresar] @pstrJsonEnvio=@pstrJsonEnvio,@pstrJsonValoresDefecto=@pstrJsonValoresDefecto, @pstrusuario=@pstrusuario,@pstraplicacion=@pstraplicacion ",
                    new SqlParameter("@pstrJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                    new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                    new SqlParameter("@pstrusuario", ""),
                    new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/Ingresar", mensajes.ToString(), "Finaliza ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "OrdenesController", "Ingresar", string.Empty));
            }
        }

        /// <summary>
        /// Consulta es estado de las ordenes ingresadas al sistema
        /// </summary>
        /// <param name="parametros">
        /// "intnumeroorden" Numero de la orden asignada por el sistema
        /// "strclaseorden" Clase de la orden (C:Renta fija, A:Renta Variable)</param>
        /// <returns></returns>
        [HttpGet("ConsultarEstado")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Ordenes.EstadoOrdenDTO>>> Get_OrdenesEstado ([FromBody] Parametros.Ordenes.GetOrdenesEstados parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/ConsultarEstado", parametros.ToString(), "Inicio ejecución.");
                var ordenesestado = await contextobdoyd.estadosorden.FromSql("[APIADCAP].[usp_OrdenesController_Get_OrdenesEstado] 	@pstrJsonEnvio=@pstrJsonEnvio, @pstrusuario=@pstrusuario, @pstraplicacion=@pstraplicacion",
                    new SqlParameter("@pstrJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                    new SqlParameter("@pstrusuario", ""),
                    new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/ConsultarEstado", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Ordenes.EstadoOrdenDTO>>(ordenesestado);

            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "OrdenesController", "ConsultarEstado", string.Empty));
            }
        }

        /// <summary>
        /// Cancela una orden pendiente en el sistema (el sistema valida si es posible cancelar o no la orden)
        /// </summary>
        /// <param name = "parametros" > Tipo: int; Numero de la orden asignada por el sistema
        /// "intnumeroorden" Numero de la orden asignada por el sistema
        /// "strclaseorden" Clase de la orden (C:Renta fija, A:Renta Variable)</param>
        /// <returns></returns>
        [HttpDelete("Cancelar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Delete_Cancelar([FromBody] Parametros.Ordenes.GetOrdenesEstados parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/Cancelar", parametros.ToString(), "Inicio ejecución.");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_OrdenesController_Delete_Cancelar] @pstrJsonEnvio=@pstrJsonEnvio, @pstrusuario=@pstrusuario, @pstraplicacion=@pstraplicacion",
                    new SqlParameter("@pstrJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                    new SqlParameter("@pstrusuario", ""),
                    new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/Cancelar", mensajes.ToString(), "Inicio ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "OrdenesController", "Cancelar", string.Empty));
            }
        }

        /// <summary>
        /// Modifica las ordenes ingresadas al sistema (solo se envían los datos que se modifiquen)
        /// </summary>
        /// <param name="parametros"> 
        /// intcodigooyd" Codigo oyd del cliente
        /// dblnominal" Cantidad nominal de la orden
        /// dblvalortotal" Valor de la orden en pesos
        /// strnemotecnico" Nemotecnico de la especie asociada a la orden
        /// strmoneda" Codigo de la moneda de la orden, correspondiente con el maestro de monedas (COP, TRM...etc)
        /// strtipomovimiento" Tipo de movimiento a realizar, correspondiente con el maestro de tipos de movimiento(001:Compra, 002:Venta...etc)
        /// dblprecio" Precio de mercado de la orden
        /// logincluirgastos" Indica si en el calculo se incluyen los gastos
        /// strisin" Isin asociado a la especie de la orden (solo en caso que la especie tenga asociado mas de un isin)
        /// intnumeroorden" Numero de la orden asignada por el sistema
        /// strclaseorden" Clase de la orden (C:Renta fija, A:Renta Variable)</param>
        /// <returns></returns>
        [HttpPut("Modificar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Put_Modificar([FromBody] Parametros.Ordenes.PutOrdenesModificar parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/Modificar", parametros.ToString(), "Inicio ejecución.");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_OrdenesController_Put_Modificar] @pstrJsonEnvio=@pstrJsonEnvio, @pstrusuario=@pstrusuario,@pstraplicacion=@pstraplicacion ",
                    new SqlParameter("@pstrJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                    new SqlParameter("@pstrusuario", ""),
                    new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("OrdenesController", "A2/V1/Ordenes/Modificar", mensajes.ToString(), "Finaliza ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "OrdenesController", "Modificar", string.Empty));
            }
        }



    }
}
