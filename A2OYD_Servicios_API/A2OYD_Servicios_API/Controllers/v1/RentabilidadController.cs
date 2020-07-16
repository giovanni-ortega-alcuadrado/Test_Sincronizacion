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
    [Route("A2/V1/personas/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RentabilidadController : ControllerBase
    {
        private readonly ContextoDbOyd contextobdoyd;
        private readonly UtilidadesGenericas utilidadesgenericas;
        private readonly IMapper mapper;
        UtilidadesController utilidadcontroller;

        public RentabilidadController(ContextoDbOyd contextobdoyd, UtilidadesGenericas utilidadesgenericas, IMapper mapper)
        {
            this.contextobdoyd = contextobdoyd;
            this.utilidadesgenericas = utilidadesgenericas;
            this.mapper = mapper;
            utilidadcontroller = new UtilidadesController(contextobdoyd, mapper);
        }

        [HttpGet("Cliente")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Rentabilidad.RentabilidadDTO>>> Get_Cliente([FromBody] Parametros.Rentabilidad.GetRentabilidad parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("RentabilidadController", "A2/Rentabilidad/Cliente", parametros.ToString(), "Inicio ejecución.");
                var retorno = await contextobdoyd.rentabilidad.FromSql("[APIADCAP].[usp_RentabilidadController_Get_ClienteInstrumento] @strJsonEnvio, @pstrUsuario, @pstrAplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrUsuario", ""),
                        new SqlParameter("@pstrAplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("RentabilidadController", "A2/Rentabilidad/Cliente", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Rentabilidad.RentabilidadDTO>>(retorno);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "RentabilidadController", "Cliente", string.Empty));
            }
        }

        [HttpGet("ClienteInstrumento")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Rentabilidad.RentabilidadDTO>>> Get_ClienteInstrumento([FromBody] Parametros.Rentabilidad.GetClientesRentabilidadInstrumento parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("RentabilidadController", "A2/Rentabilidad/ClienteInstrumento", parametros.ToString(), "Inicio ejecución.");
                var retorno = await contextobdoyd.rentabilidad.FromSql("[APIADCAP].[usp_RentabilidadController_Get_ClienteInstrumento] @strJsonEnvio, @pstrUsuario, @pstrAplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrUsuario", ""),
                        new SqlParameter("@pstrAplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("RentabilidadController", "A2/Rentabilidad/ClienteInstrumento", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Rentabilidad.RentabilidadDTO>>(retorno);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "RentabilidadController", "ClienteInstrumento", string.Empty));
            }
        }
    }
}
