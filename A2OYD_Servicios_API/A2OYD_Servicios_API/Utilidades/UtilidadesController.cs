using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2OYD_Servicios_API.Entidades.Genericas;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using A2OYD_Servicios_API.Models.DTO.Entidades;
using Microsoft.EntityFrameworkCore;
using A2OYD_Servicios_API.ContextosDB;
using System.Data.SqlClient;

namespace A2OYD_Servicios_API.Utilidades
{

    public  class UtilidadesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ContextoDbOyd contextobdoyd;

        public UtilidadesController(ContextoDbOyd contextobdoyd, IMapper mapper)
        {
            this.mapper = mapper;
            this.contextobdoyd = contextobdoyd;
        }
        public UtilidadesController()
        {
        }
        public string Obtener_Valores_por_Defecto_json(string strControlador = "", string strProceso = "", string strtabla = "")
        {
            try
            {
                
                var valores = contextobdoyd.valorespordefecto.FromSql("[dbo].[usp_Obtener_Valores_por_Defecto] @pstrControlador=@pstrControlador, @pstrProceso=@pstrProceso, @plogretornarstring=@plogretornarstring ",
                    new SqlParameter("@pstrControlador", strControlador),
                    new SqlParameter("@pstrProceso", strProceso),
                    //new SqlParameter("@pstrtabla", strtabla),
                    new SqlParameter("@plogretornarstring", true)
                    ).ToList();
                //La logica que esta comentada es para cuando el sp devuelve los campos de la tabala, pero si se envía el parametro @plogretornarstring en 1 el sp ya retorna el string en formato json
                //string json = "[{";
                //foreach (Entidades.Genericas.ValorporDefecto registro in valores)
                //{
                //    json = json + "\"" + registro.nombrevalor + "\":\"" + registro.valor + "\",";
                //}
                //json = json + "}]";
                //return json;
                if  (valores.Count > 0) 
                {
                    return valores[0].valor;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception errror)
            {
                throw new Exception(errror.ToString());
            }
        }
        public  ActionResult<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO> SepararErroresyExitosos(List<MensajeRespuesta> mensajes)
        {
            
            List<MensajeRespuesta> errores = new List<MensajeRespuesta>();
            List<MensajeRespuesta> exitosos = new List<MensajeRespuesta>();
            Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO mensajeseparadodto;
            bool hayerror = false;
            try
            { 
                foreach (MensajeRespuesta mensaje in mensajes)
                {
                    if (mensaje.exitoso == true)
                    {
                        exitosos.Add(mensaje);

                    }
                    else
                    {
                        errores.Add(mensaje);
                        hayerror = true;
                    }
                }
                MensajeRespuestaSeparado mensajeseparado = new MensajeRespuestaSeparado(errores,exitosos);
                mensajeseparado.cantidaderrores = errores.Count;
                mensajeseparado.cantidadexitosos = exitosos.Count;
                mensajeseparadodto = mapper.Map<Models.DTO.Entidades.Genericas.MensajeRespuestaSeparadoDTO>(mensajeseparado);
                if (hayerror==true)
                {

                    //ActionResult<Entidades.Genericas.MensajeRespuestaSeparado> respuesta = new ActionResult<Entidades.Genericas.MensajeRespuestaSeparado>(mensajeseparado);
                    return BadRequest(mensajeseparadodto);
                }
                else
                {
                    //ActionResult<Entidades.Genericas.MensajeRespuestaSeparado> respuesta = new ActionResult<Entidades.Genericas.MensajeRespuestaSeparado>(mensajeseparado);
                    return Ok(mensajeseparadodto);
                    //return BadRequest(mensajeseparado);
                }
                
            }
            catch(Exception error)
            {
                throw new Exception(error.Message);
            }

        }

        public List<Entidades.Ordenes.GastoOrdenEncabezado> GastoOrdenSepararEncabezadoyDetalle(List<Entidades.Ordenes.GastoOrden> lista)
        {
            List<Entidades.Ordenes.GastoOrdenEncabezado> respuesta = new List<Entidades.Ordenes.GastoOrdenEncabezado>();
            List<Entidades.Ordenes.GastoOrdenDetalle> detalles;
            Entidades.Ordenes.GastoOrdenEncabezado encabezado;
            Entidades.Ordenes.GastoOrdenDetalle detalle;
            foreach (Entidades.Ordenes.GastoOrden gasto in lista)
            {
                if (gasto.idencabezado==null)
                {
                    encabezado = new Entidades.Ordenes.GastoOrdenEncabezado();
                    encabezado.valorbruto = gasto.valorbruto;
                    encabezado.valorneto = gasto.valorneto;
                    encabezado.id = gasto.id;
                    respuesta.Add(encabezado);
                }
            }
            foreach (Entidades.Ordenes.GastoOrdenEncabezado enc in respuesta)
            {
                detalles = new List<Entidades.Ordenes.GastoOrdenDetalle>();
                foreach (Entidades.Ordenes.GastoOrden gasto in lista) 
                    {
                        if (enc.id==gasto.idencabezado)
                        {
                            detalle = new Entidades.Ordenes.GastoOrdenDetalle();
                            detalle.descripcion = gasto.descripcion;
                            detalle.valor = gasto.valorcosto;
                            detalles.Add(detalle);
                        }
                    }
                enc.costos = detalles;
            }
            return respuesta;
        }

        public List<Entidades.Personas.ClienteDatosCuentaDetalle> ClienteDatosCuentaSepararEncabezadoyDetalle(List<Entidades.Personas.ClienteDatosCuenta> lista)
        {
            List<Entidades.Personas.ClienteDatosCuentaDetalle> respuesta = new List<Entidades.Personas.ClienteDatosCuentaDetalle>();
            List<Entidades.Personas.PersonaAsociada> personasasociadas;
            List<Entidades.Personas.CuentaBancoAsociada> cuentasbanco;
            List<Entidades.Personas.MercadoAsociado> mercadosoperacion;
            Entidades.Personas.ClienteDatosCuentaDetalle detalle;
            Entidades.Personas.PersonaAsociada personaasociada;
            Entidades.Personas.CuentaBancoAsociada cuentabanco;
            Entidades.Personas.MercadoAsociado mercadooperacion;
            foreach (Entidades.Personas.ClienteDatosCuenta datoscuentas in lista)
            {
                if (datoscuentas.tiporegistro == "ENCABEZADO")
                {
                    detalle = new Entidades.Personas.ClienteDatosCuentaDetalle();
                    detalle.codigooyd  = datoscuentas.codigooyd;
                    detalle.estado = datoscuentas.estado;
                    detalle.tipovinculacion = datoscuentas.tipovinculacion;
                    respuesta.Add(detalle);
                }
            }
            foreach (Entidades.Personas.ClienteDatosCuentaDetalle det in respuesta)
            {
                personasasociadas = new List<Entidades.Personas.PersonaAsociada>();
                cuentasbanco = new List<Entidades.Personas.CuentaBancoAsociada>();
                mercadosoperacion = new List<Entidades.Personas.MercadoAsociado>();
                foreach (Entidades.Personas.ClienteDatosCuenta datoscuenta in lista)
                {
                    if (det.codigooyd == datoscuenta.codigooyd )
                    {
                        if (datoscuenta.tiporegistro == "PERSONA")
                        {
                            
                            personaasociada = new Entidades.Personas.PersonaAsociada();
                            personaasociada.apellido = datoscuenta.apellidoasociada;
                            personaasociada.identificacion = datoscuenta.identificacionasociada;
                            personaasociada.nombre = datoscuenta.nombreasociada;
                            personaasociada.tipoidentificacion = datoscuenta.tipoidentificacionasociada;
                            personaasociada.tiporelacion = datoscuenta.tiporelacion;
                            personasasociadas.Add(personaasociada);
                        }
                        else if (datoscuenta.tiporegistro == "CUENTABANCO")
                        {
                            
                            cuentabanco = new Entidades.Personas.CuentaBancoAsociada();
                            cuentabanco.codigobanco = datoscuenta.codigobanco;
                            cuentabanco.descripcion = datoscuenta.descripcioncuentabanco;
                            cuentabanco.idcuenta = datoscuenta.idcuenta;
                            cuentabanco.nombrebanco = datoscuenta.nombrebanco;
                            cuentabanco.numerocuenta = datoscuenta.numerocuentabanco;
                            cuentabanco.tipocuenta = datoscuenta.tipocuentabanco;
                            cuentasbanco.Add(cuentabanco);
                        }
                        else if (datoscuenta.tiporegistro == "MERCADO")
                        {
                            
                            mercadooperacion = new Entidades.Personas.MercadoAsociado();
                            mercadooperacion.codigomercado = datoscuenta.codigomercado;
                            mercadooperacion.descripcion = datoscuenta.descripcionmercado;
                            mercadosoperacion.Add(mercadooperacion);
                        }
                    }
                }
                det.cuentasbancarias = cuentasbanco;
                det.personasasociadas = personasasociadas;
                det.mercadosoperacion = mercadosoperacion;
            }
            return respuesta;
        }
    }
}
