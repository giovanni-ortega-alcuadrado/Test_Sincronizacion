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
    public class PersonasController : ControllerBase
    {
        private readonly ContextoDbOyd contextobdoyd;
        private readonly UtilidadesGenericas utilidadesgenericas;
        private readonly IMapper mapper;
        UtilidadesController utilidadcontroller;

        public PersonasController(ContextoDbOyd contextobdoyd, UtilidadesGenericas utilidadesgenericas, IMapper mapper)
        {
            this.contextobdoyd = contextobdoyd;
            this.utilidadesgenericas = utilidadesgenericas;
            this.mapper = mapper;
            utilidadcontroller = new UtilidadesController(contextobdoyd, mapper);
        }

        /// <summary>
        /// Consulta las cuentas bancarias asociadas a un cliente
        /// </summary>
        /// <param name="parametros">
        /// "intcodigooyd" Codigo oyd del cliente </param>
        /// <returns>idcuenta -- codigobanco -- nombrebanco -- numerocuenta -- tipocuenta -- descripcion -- activo</returns>
        [HttpGet("Cliente/CuentasBancarias/Consultar")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Personas.ClienteCuentaBancoDTO>>> Get_ClientesCuentasBancarias([FromBody] Parametros.Genericos.Oyd parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Consultar", parametros.ToString(), "Inicio ejecución.");
                var clientescuentasbancarias = await contextobdoyd.clientecuentasbancarias.FromSql("[APIADCAP].[usp_PersonasController_Get_ClientesCuentasBancarias] @strJsonEnvio ,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Consultar", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Personas.ClienteCuentaBancoDTO>>(clientescuentasbancarias);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "Cliente/CuentasBancarias/Consultar", string.Empty));
            }
        }

        /// <summary>
        /// Ingresa cuentas bancarias de un cliente al sistema
        /// </summary>
        /// <param name="parametros">
        /// "intcodigooyd" Codigo oyd del cliente 
        /// "strtipocuenta" Codigo oyd del cliente 
        /// "intcodigobanco" Id del banco al que pertenece la cuenta, debe corresponder con el id del maestro de bancos 
        /// "intnumerocuenta" Numero de cuenta asignado al cliente</param>
        /// <returns></returns>
        [HttpPost("Cliente/CuentasBancarias/Ingresar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Post_ClientesCuentasBancarias([FromBody] Parametros.Personas.PostClientesCuentasBancarias parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Ingresar", parametros.ToString(), "Inicio ejecución.");
                //Entidades.Genericas.MensajeRespuesta mensajes = new Entidades.Genericas.MensajeRespuesta();
                Task<string> valorespordefecto = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_ClientesCuentasBancarias");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_PersonasController_Post_ClientesCuentasBancarias] @strJsonEnvio,@pstrJsonValoresDefecto,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Ingresar", mensajes.ToString(), "Inicio ejecución.");
                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "Cliente/CuentasBancarias/Ingresar", string.Empty));
            }
        }

        /// <summary>
        /// Eliminar la cuenta bancaria ingresada al sistema
        /// </summary>
        /// <param name="parametros"> Tipo: int; Identificador unico asignado por el sistema al crear la cuenta
        /// "intid" Identificador unico asignado por el sistema al crear la cuenta</param>
        /// <returns></returns>
        [HttpDelete("Cliente/CuentasBancarias/Eliminar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Delete_ClientesCuentasBancarias([FromBody] Parametros.Genericos.Id parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Eliminar", parametros.ToString(), "Inicio ejecución.");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_PersonasController_Post_ClientesCuentasBancarias] @strJsonEnvio,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Eliminar", mensajes.ToString(), "Inicio ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "Cliente/CuentasBancarias/Eliminar", string.Empty));
            }
        }

        /// <summary>
        /// Consultar codigos oyd asociados a una identificacion
        /// </summary>
        /// <param name="parametros"> 
        /// "stridentificacion" Numero de identificacion
        /// "strtipoidentificacion" Tipo de identificacion, debe corresponder con el maestro de tipos de identificacion</param>
        /// <returns></returns>
        [HttpGet("ConsultarCodigosOyD")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Personas.ClienteCodigoOyDDTO>>> Get_CodigosOyD([FromBody] Parametros.Genericos.Idtipoid parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/ConsultarCodigosOyD", parametros.ToString(), "Inicio ejecución.");
                var codigosoyd = await contextobdoyd.clientecodigosoyd.FromSql("[APIADCAP].[usp_PersonasController_Get_CodigosOyD] @strJsonEnvio ,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/ConsultarCodigosOyD", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Personas.ClienteCodigoOyDDTO>>(codigosoyd);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "ConsultarCodigosOyD", string.Empty));
            }
        }

        /// <summary>
        /// Modificar las cuentas ingresadas al sistema, solo es necesario enviar los datos que se modifiquen
        /// </summary>
        /// <param name="parametros">
        /// "intidcuenta" identificador unico asignado por el sistema al ingresar la cuenta 
        /// "intcodigooyd" Codigo oyd del cliente 
        /// "strtipocuenta" Codigo oyd del cliente 
        /// "intcodigobanco" Id del banco al que pertenece la cuenta, debe corresponder con el id del maestro de bancos 
        /// "intnumerocuenta" Numero de cuenta asignado al cliente</param>
        /// <returns></returns>
        [HttpPut("Cliente/CuentasBancarias/Modificar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Put_ClientesCuentasBancarias([FromBody] Parametros.Personas.PutClientesCuentasBancarias parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Modificar", parametros.ToString(), "Inicio ejecución.");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_PersonasController_Put_ClientesCuentasBancarias] @strJsonEnvio,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Modificar", mensajes.ToString(), "Finaliza ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "Cliente/CuentasBancarias/Modificar", string.Empty));
            }
        }

        /// <summary>
        /// Ingresar una persona al sistema
        /// </summary>
        /// <param name="parametros"> 
        /// "logpersonajuridica" Indica si es persona natural o juridica
        /// "strnombre" Nombre de la persona
        /// "strapelido" Apellido de la persona
        /// "strtipoidentificacion" Tipo de identificacion, debe corresponder con el maestro de tipos de identificacion
        /// "stridentiicacion" Numero de identificacion
        /// "strestadocivil" Codigo del estado civil, debe corresponder con el maestro de estado civil
        /// "dtmfechanacimiento" Fecha de nacimiento
        /// "intlugarnacimiento" Ciudad de nacimiento, debe corresponder con el id(codigo unico asignado por el sistema) del maestro de ciudades 
        /// "strsexo" Sexo (M:Masculino, F:Femenino)
        /// "intcodigooydordenante" Codigo oyd del ordenante de la cuenta
        /// "direccion" Datos de la direccion de la persona
        /// "stractividadeconomica" Actividad economica
        /// "strcodigoCIIU" Codigo CIIU de la persona, debe corresponder con el maestro de codigos CIIU
        /// "logpoliticamenteexpuesto" Indica si el cliente es politicamente expuesto o no 
        /// "telefonocontacto" Numero de telefono del contacto del cliente 
        /// "celularcontacto"  Numero de celunar del contacto del cliente
        /// "emailcontacto"> Tipo: bool; Email del contacto del cliente</param>
        /// <returns></returns>
        [HttpPost("Ingresar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Post_Ingresar([FromBody] Parametros.Personas.PostClientesIngresar parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Ingresar", parametros.ToString(), "Inicio ejecución.");
                List<Models.DTO.Parametros.Personas.PostClientesIngresarDTO>  parametrosDTO = mapper.Map<List<Models.DTO.Parametros.Personas.PostClientesIngresarDTO>>(parametros);
                List<Models.DTO.Parametros.Genericos.DireccionDTO> direccionDTO = mapper.Map<List<Models.DTO.Parametros.Genericos.DireccionDTO>>(parametros.direccion);
                direccionDTO[1].stridentificacion = parametrosDTO[1].stridentiicacion;
                Task<string> valorespordefecto = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar","tblclientes");
                Task<string> valorespordefectoClientesOrdenantes = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesOrdenantes");
                Task<string> valorespordefectoClientesReceptores = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesReceptores");
                Task<string> valorespordefectoClientesCuentas = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesCuentas");
                Task<string> valorespordefectoClientesBeneficiarios = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesBeneficiarios");
                Task<string> valorespordefectoClientesAficiones = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesAficiones");
                Task<string> valorespordefectoClientesDeportes = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesDeportes");
                Task<string> valorespordefectoClientesDoctosClientes = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesDoctosClientes");
                Task<string> valorespordefectoClientesFicha = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesFicha");
                Task<string> valorespordefectoClientesTipoEntidad = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesTipoEntidad");
                Task<string> valorespordefectoClientesPaisesFATCA = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesPaisesFATCA");
                Task<string> valorespordefectoClientesDepEconomica = utilidadcontroller.Obtener_Valores_por_Defecto_json("PersonasController", "Post_Ingresar", "tblClientesDepEconomica");

                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_PersonasController_Post_Ingresar] @strJsonEnvio,@pstrJsonValoresDefecto,@pstrJsonClientesOrdenantes, @pstrJsonClientesReceptores," +
                                                                            "@pstrJsonClientesCuentas,@pstrJsonClientesBeneficiarios,@pstrJsonClientesDirecciones,@pstrJsonClientesAficiones," +
                                                                            "@pstrJsonClientesDeportes,@pstrJsonClientesDoctosClientes,@pstrJsonClientesFicha,@pstrJsonClientesTipoEntidad,@pstrJsonClientesPaisesFATCA," +
                                                                            "@pstrJsonClientesDepEconomica,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametrosDTO)),
                        new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                        new SqlParameter("@pstrJsonClientesOrdenantes", valorespordefectoClientesOrdenantes),
                        new SqlParameter("@pstrJsonClientesReceptores", valorespordefectoClientesReceptores),
                        new SqlParameter("@pstrJsonClientesCuentas", valorespordefectoClientesCuentas),
                        new SqlParameter("@pstrJsonClientesBeneficiarios", valorespordefectoClientesBeneficiarios),
                        new SqlParameter("@pstrJsonClientesDirecciones", Newtonsoft.Json.JsonConvert.SerializeObject(parametrosDTO)),
                        new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                        new SqlParameter("@strJsonEnvio", valorespordefecto),
                        new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                        new SqlParameter("@strJsonEnvio", valorespordefecto),
                        new SqlParameter("@pstrJsonValoresDefecto", valorespordefecto),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Ingresar", mensajes.ToString(), "Inicio ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "Ingresar", string.Empty));
            }
        }

        /// <summary>
        /// Modificar las personas ingresadas al sistema
        /// </summary>
        /// <param name="parametros">
        /// "logpersonajuridica" Indica si es persona natural o juridica
        /// "strnombre" Nombre de la persona
        /// "strapelido" Apellido de la persona
        /// "strtipoidentificacion" Tipo de identificacion, debe corresponder con el maestro de tipos de identificacion
        /// "stridentiicacion" Numero de identificacion
        /// "strestadocivil" Codigo del estado civil, debe corresponder con el maestro de estado civil
        /// "dtmfechanacimiento" Fecha de nacimiento
        /// "intlugarnacimiento" Ciudad de nacimiento, debe corresponder con el id(codigo unico asignado por el sistema) del maestro de ciudades 
        /// "strsexo" Sexo (M:Masculino, F:Femenino)
        /// "intcodigooydordenante" Codigo oyd del ordenante de la cuenta
        /// "direccion" Datos de la direccion de la persona
        /// "stractividadeconomica" Actividad economica
        /// "strcodigoCIIU" Codigo CIIU de la persona, debe corresponder con el maestro de codigos CIIU
        /// "logpoliticamenteexpuesto" Indica si el cliente es politicamente expuesto o no 
        /// "telefonocontacto" Numero de telefono del contacto del cliente 
        /// "celularcontacto"  Numero de celunar del contacto del cliente
        /// "emailcontacto" Email del contacto del cliente
        /// "Intidcodigooyd"> Tipo: Int64; Codigo oyd asignado por el sistema al ingresar la persona</param>
        /// <returns></returns>
        [HttpPut("Modificar")]
        public async Task<ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>> Put_Modificar([FromBody] Parametros.Personas.PutClientesModificar parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Modificar", parametros.ToString(), "Inicio ejecución.");
                var mensajes = await contextobdoyd.mensajerespuesta.FromSql("[APIADCAP].[usp_PersonasController_Put_Modificar] @strJsonEnvio,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Modificar", mensajes.ToString(), "Finaliza ejecución.");

                return utilidadcontroller.SepararErroresyExitosos(mensajes);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "Modificar", string.Empty));
            }
        }

        /// <summary>
        /// Consultar los datos de la persona ingresada filtrando por el codigo oyd
        /// </summary>
        /// <param name="parametros"> 
        /// "Intidcodigooyd"> Codigo oyd asignado por el sistema al ingresar la persona</param>
        /// <returns>codigooyd -- estado -- tipovinculacion -- Lista(personaasociada -- Lista(cuentasbancarias) -- Lista(mercadosoperacion) </returns>
        [HttpGet("Cuentas/ConsultarDatos")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Personas.ClienteDatosCuentaDetalleDTO>>> Get_consultardatoscuentas([FromBody] Parametros.Genericos.Oyd parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cuentas/ConsultarDatos", parametros.ToString(), "Inicio ejecución.");
                var datoscuentas = await contextobdoyd.clientesdatoscuentas.FromSql("[APIADCAP].[usp_PersonasController_Get_consultardatoscuentas] @strJsonEnvio ,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cuentas/ConsultarDatos", parametros.ToString(), "Finaliza ejecución.");

                List<Entidades.Personas.ClienteDatosCuentaDetalle> resultadoseparado = utilidadcontroller.ClienteDatosCuentaSepararEncabezadoyDetalle(datoscuentas);
                return mapper.Map<List<Models.DTO.Entidades.Personas.ClienteDatosCuentaDetalleDTO>>(resultadoseparado);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "Cuentas/ConsultarDatos", string.Empty));
            }
        }

        /// <summary>
        /// Consulta las cuentas bancarias asociadas a un cliente
        /// </summary>
        /// <param name="parametros">
        /// "intcodigooyd" Codigo oyd del cliente </param>
        /// <returns>idcuenta -- codigobanco -- nombrebanco -- numerocuenta -- tipocuenta -- descripcion -- activo</returns>
        [HttpGet("Cliente/Fondos/ConsultarEncargo")]
        public async Task<ActionResult<IEnumerable<Models.DTO.Entidades.Personas.ClienteEncargoDTO>>> Get_ClienteConsultarEncargo([FromBody] Parametros.Personas.GetClienteConsultarEncargo parametros)
        {
            try
            {
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Consultar", parametros.ToString(), "Inicio ejecución.");
                var clienteencargos = await contextobdoyd.clienteencargos.FromSql("[APIADCAP].[usp_PersonasController_Get_ClienteConsultarEncargo] @strJsonEnvio ,@pstrusuario ,@pstraplicacion",
                        new SqlParameter("@strJsonEnvio", Newtonsoft.Json.JsonConvert.SerializeObject(parametros)),
                        new SqlParameter("@pstrusuario", ""),
                        new SqlParameter("@pstraplicacion", "")).ToListAsync();
                utilidadesgenericas.CrearLogSeguimiento("PersonasController", "A2/Personas/Cliente/CuentasBancarias/Consultar", parametros.ToString(), "Finaliza ejecución.");
                return mapper.Map<List<Models.DTO.Entidades.Personas.ClienteEncargoDTO>>(clienteencargos);
            }
            catch (Exception errror)
            {
                return BadRequest(utilidadesgenericas.RetornarErrorControlado(errror, "PersonasController", "Cliente/CuentasBancarias/Consultar", string.Empty));
            }
        }
    }
}
