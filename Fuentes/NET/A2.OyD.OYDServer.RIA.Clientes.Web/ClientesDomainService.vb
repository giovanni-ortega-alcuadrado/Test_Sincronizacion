
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq
Imports System.IO
Imports System.Collections.ObjectModel
Imports System.Web
Imports System.Configuration
Imports A2Utilidades.Cifrar
Imports System.Transactions
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.OyDClientes
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class ClientesDomainService
    Inherits LinqToSqlDomainService(Of OyDClientesDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

    Public Overrides Function Submit(ByVal changeSet As OpenRiaServices.DomainServices.Server.ChangeSet) As Boolean
        Dim result As Boolean

        Using tx = New TransactionScope(
                TransactionScopeOption.Required,
                New TransactionOptions With {.IsolationLevel = IsolationLevel.ReadCommitted})

            result = MyBase.Submit(changeSet)
            If (Not Me.ChangeSet.HasError) Then
                tx.Complete()
            End If
        End Using
        Return result
    End Function

#Region "Clientes"
    Public Function ClientesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Clientes_Cliente_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClientesFiltrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception

            ManejarError(ex, Me.ToString(), "ClientesFiltrar")
            Return Nothing
        End Try
    End Function
    Public Function ClientesConsultar(ByVal filtro As Byte, ByVal pIDComitente As String, ByVal pNombre As String, ByVal NroDocumento As String, ByVal strTipoIdentificacion As String, ByVal strClasificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Clientes_Cliente_Consultar(filtro, RetornarValorDescodificado(pIDComitente), RetornarValorDescodificado(pNombre), RetornarValorDescodificado(NroDocumento), strTipoIdentificacion, strClasificacion, DemeInfoSesion(pstrUsuario, "BuscarClientes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClientes")
            Return Nothing
        End Try
    End Function
    Public Sub ClientesEnviarBus(ByVal pIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CreacionInversionista_BusIntegracion(pIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "EnviarBus"), ClsConstantes.GINT_ErrorPersonalizado)

            Exit Sub
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EnviarBus")

        End Try
    End Sub
    Public Function TraerClientePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IDComitente = "-1"
            'e.IDSucCliente = 
            'e.TipoIdentificacion = 
            'e.NroDocumento = 
            'e.NroDocumento = 
            'e.IDPoblacionDoc = 
            'e.IDDepartamentoDoc = 
            'e.IDPaisDoc = 
            'e.IDNacionalidad = 
            'e.Nombre = 
            'e.TipoVinculacion = 
            'e.Ingreso = 
            'e.Telefono1 = 
            'e.Telefono2 = 
            'e.Fax1 = 
            'e.Fax2 = 
            'e.Direccion = 
            'e.Internet = 
            'e.IDPoblacion = 
            'e.IDDepartamento = 
            'e.IdPais = 
            'e.MenorEdad = 
            'e.IDGrupo = 
            'e.IDSubGrupo = 
            'e.IdPatrimonio = 
            'e.OrigenPatrimonio = 
            'e.IdProfesion = 
            'e.RetFuente = 
            'e.IngresoMensual = 
            'e.Activo = 
            'e.IDConcepto = 
            'e.Concepto = 
            'e.Notas = 
            'e.SuperValores = 
            'e.Admonvalores = 
            'e.ContratoAV = 
            'e.NotasAdmon = 
            'e.Tercero = 
            'e.Contribuyente = 
            'e.ActividadEconomica = 
            'e.Excluido = 
            'e.Activos = 
            'e.Patrimonio = 
            'e.Utilidades = 
            'e.FactorComisionCliente = 
            'e.IDSector = 
            'e.IDSubSector = 
            'e.DireccionEnvio = 
            'e.EntregarA = 
            'e.EMail = 
            'e.Nacimiento = 
            'e.EstadoCivil = 
            'e.Sexo = 
            'e.Ocupacion = 
            'e.Cargo = 
            'e.Empresa = 
            'e.Secretaria = 
            'e.RepresentanteLegal = 
            'e.TipoReprLegal = 
            'e.IDReprLegal = 
            'e.CargoReprLegal = 
            'e.TelefonoReprLegal = 
            'e.DireccionReprLegal = 
            'e.IDCiudadReprLegal = 
            'e.ClaveInternet = 
            'e.ActivoRemoto = 
            'e.TipoReferencia = 
            'e.RazonVinculacion = 
            'e.RecomendadoPor = 
            'e.ActualizacionFicha = 
            'e.FormaPago = 
            'e.IdCuentaCtble = 
            'e.TituloCliente = 
            'e.NoLlamarAlCliente = 
            'e.EnviarInformeEconomico = 
            'e.EnviarPortafolio = 
            'e.Estrato = 
            'e.NumeroHijos = 
            'e.NacimientoRepresentanteLegal = 
            'e.OrigenIngresos = 
            'e.OrigenFondo = 
            'e.DireccionOficina = 
            'e.TelefonoOficina = 
            'e.FaxOficina = 
            'e.ApartadoAereo = 
            'e.EntregaCorrespondencia = 
            'e.TipoSegmento = 
            'e.RetornoNroOpTrimestre = 
            'e.RetornoVlrInversionesLiquidas = 
            'e.RetornoPorcInversionesLiquidas = 
            'e.Egresos = 
            'e.DetalleOIngresos = 
            'e.codigoCiiu = 
            'e.OpMonedaExtranjera = 
            'e.TipoOperacion = 
            'e.CualOtroTipoOper = 
            'e.NroCuentaExt = 
            'e.BancoExt = 
            'e.Moneda = 
            'e.CiudadExt = 
            'e.PaisExt = 
            'e.LugarEntrevista = 
            'e.ObservacionEnt = 
            'e.Entrevista = 
            'e.ReceptorEntrevista = 
            'e.TelefonoResidencia = 
            'e.direccionResidencia = 
            'e.RetornoValorSemestral = 
            'e.FiguraPublica = 
            'e.PersonaAltoRiesgo = 
            'e.Suitability = 
            'e.Apellido1 = 
            'e.Apellido2 = 
            'e.Nombre1 = 
            'e.Nombre2 = 
            'e.CodigoSwift = 
            'e.MovimientoEspecial = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.TIER = 
            'e.IndicadorAsalariado = 
            'e.Clasificacion = 
            'e.ValorActivoLiquido = 
            'e.TipoRelacion = 
            'e.SinAnimoDeLucro = 
            'e.ObjetivoInversion = 
            'e.HorizonteInversion = 
            'e.EdadCliente = 
            'e.ConocimientoExperiencia = 
            'e.ClasificacionInversionista = 
            'e.Superintendencia = 
            'e.IDSucursal = 
            'e.EstadoCliente = 
            'e.CtaMultipdto = 
            'e.Periodicidad = 
            'e.PerfilRiesgo = 
            'e.TipoProducto = 
            'e.TipoCliente = 
            'e.Perfil = 
            'e.Exceptuado = 
            'e.CategoriaCliente = 
            'e.ComisionAcciones = 
            'e.UltimoMov = 
            'e.NitDepositante = 
            'e.Apt = 
            'e.CoopExenta = 
            'e.PagoSurenta = 
            'e.PoderParaFirmar = 
            'e.PrestadorServExcIVA = 
            'e.SegmentoUsuarioRedes = 
            'e.Visacion = 
            'e.AtencionOperativa = 
            'e.CentroEjecucion = 
            'e.ClasificacionDocumento = 
            'e.DireccionEnvioDane = 
            'e.ExisteContratoAdmon = 
            'e.JustificacionFondos = 
            'e.PrioridadPortafolio = 
            'e.Referencia = 
            'e.RetencionFuente = 
            'e.Sigla = 
            'e.TipoMensajeria = 
            'e.TipoRegalo = 
            'e.UsuarioIngreso = 
            'e.FondoExtranjero = 
            'e.ReplicarSafyrFondos = 
            'e.ReplicarSafyrPortafolios = 
            'e.ReplicarSafyrClientes = 
            'e.ReplicarMercansoft = 
            'e.Declarante = 
            'e.AutoRetenedor = 
            'e.ExentoGMF = 
            'e.IdCiudadNacimiento = 
            'e.FechaExpedicionDoc = 
            'e.IndicativoTelefono = 
            'e.AceptaCruces = 
            'e.TipoIntermediario = 
            'e.CodReceptorSafyrFondos = 
            'e.CodReceptorSafyrPortafolio = 
            'e.CodReceptorSafyrClientes = 
            'e.CodReceptorMercansoft = 
            'e.TipoIdentificacionRB = 
            'e.NroDocumentoRB = 
            'e.NombreRB = 
            e.IDClientes = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientePorDefecto")
            Return Nothing
        End Try
    End Function
    Public Sub InsertCliente(ByVal Cliente As A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Cliente.pstrUsuarioConexion, Cliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Cliente.InfoSesion = DemeInfoSesion(Cliente.pstrUsuarioConexion, "InsertCliente")
            Me.DataContext.Clientes.InsertOnSubmit(Cliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCliente")
        End Try
    End Sub
    Public Sub UpdateCliente(ByVal currentCliente As A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)

        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCliente.pstrUsuarioConexion, currentCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If currentCliente.InactivaItem = True Then
                currentCliente.InfoSesion = DemeInfoSesion(currentCliente.pstrUsuarioConexion, "UpdateCliente")
                Me.DataContext.Clientes.Attach(currentCliente)
                Me.DataContext.Clientes.DeleteOnSubmit(currentCliente)
            Else
                If Not IsNothing(currentCliente.Estado) Then
                    If currentCliente.Estado.Equals("Retiro") Then
                        currentCliente.InfoSesion = DemeInfoSesion(currentCliente.pstrUsuarioConexion, "UpdateCliente")
                        Me.DataContext.Clientes.Attach(currentCliente)
                        Me.DataContext.Clientes.DeleteOnSubmit(currentCliente)
                    Else
                        currentCliente.InfoSesion = DemeInfoSesion(currentCliente.pstrUsuarioConexion, "UpdateCliente")
                        Me.DataContext.Clientes.Attach(currentCliente, Me.ChangeSet.GetOriginal(currentCliente))
                    End If
                Else
                    currentCliente.InfoSesion = DemeInfoSesion(currentCliente.pstrUsuarioConexion, "UpdateCliente")
                    Me.DataContext.Clientes.Attach(currentCliente, Me.ChangeSet.GetOriginal(currentCliente))

                End If
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCliente")
        End Try
    End Sub
    Public Sub DeleteCliente(ByVal Cliente As A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Cliente.pstrUsuarioConexion, Cliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Cliente.InfoSesion = DemeInfoSesion(Cliente.pstrUsuarioConexion, "DeleteCliente")
            Me.DataContext.Clientes.Attach(Cliente)
            Me.DataContext.Clientes.DeleteOnSubmit(Cliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCliente")
        End Try
    End Sub
    Public Function UnificarClientes(ByVal IdRetira As String, ByVal pstrAccion As Char, ByVal IdUnifica As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDClientes.ConsultaNombresCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspUnificarClientes_OyDNet(IdRetira, pstrAccion, IdUnifica, 0, "Ninguno", pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultaNombre"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarCuenta")
            Return Nothing
        End Try

    End Function
    Public Function TraerUnificarClientes(ByVal IdCliente As String, ByVal pstrAccion As Char, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDClientes.ConsultaNombresCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspUnificarClientes_OyDNet(IdCliente, pstrAccion, IdCliente, 0, "Ninguno", pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultaNombre"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarCuenta")
            Return Nothing
        End Try

    End Function
    Public Function ValidaConfiguracionClientesFirmas(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDClientes.ParametrosFirmaconfig)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ValidaConfiguracionClientesFirmas.ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ValidaConfiguracionClientesFirmas")
            Return Nothing
        End Try

    End Function

    Public Function ConsultaDocumMenor(ByVal strDescripcion As String, ByVal intmenor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_DocumEsMenorEdad(strDescripcion, intmenor)
            Return intmenor
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultaDocumMenor")
            Return Nothing
        End Try
    End Function
    Public Function ActualizarReceptorescliente(ByVal strReceptorant As String, ByVal strReceptoract As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.tblLogReceptores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ClientesReceptores_ActualizarOyDNet(strReceptorant, strReceptoract, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ActualizarReceptorescliente")
            Return Nothing
        End Try
    End Function

    Public Function ClientesAutorizacionesRegistrar(ByVal pIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesAutorizaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Clientes_Autorizaciones_Registrar(pIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesAutorizacionesRegistrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception

            ManejarError(ex, Me.ToString(), "ClientesAutorizacionesRegistrar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function ActualizarReceptoresclienteEspecifico(ByVal pstrNombreCompletoArchivo As String _
                                            , ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.tblLogReceptores)
        Dim archivo As System.IO.StreamReader = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploads.FnRutasImportaciones(pstrNombreProceso, pstrUsuario)

            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'Recorremos el archivo de encabezados
            archivo = New System.IO.StreamReader(directorioUsuario & "\" & pstrNombreCompletoArchivo)
            Dim xmlResultado = "<Receptores>"
            Do While archivo.Peek() <> -1
                Dim e = archivo.ReadLine().Split(CChar(","))
                If e.Length > 1 Then
                    xmlResultado = xmlResultado + "<actualizar codCliente=" + ChrW(34) + e(0).Trim + ChrW(34) + " receptorAnterior=" + ChrW(34) + e(1).Trim + ChrW(34) + " receptorActual=" + ChrW(34) + e(2).Trim + ChrW(34) + "/>"
                End If
            Loop
            xmlResultado = xmlResultado + "</Receptores>"
            archivo.Close()
            Return ActualizarEspecificos(xmlResultado, pstrUsuario)
            'Dim strxmldecodificado = HttpUtility.HtmlDecode(strxml)
            'Dim ret = Me.DataContext.usp_ClientesReceptores_Actualizar_xmlOyDNet(strxmldecodificado, strUsuario).ToList
            'Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ActualizarReceptoresclienteEspecifico")
            Return Nothing
        Finally
            archivo.Dispose()
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function ActualizarEstadosFinancieros(ByVal strxml As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDClientes.tblConsultaDatosfinancieros)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strxmldecodificado = HttpUtility.HtmlDecode(strxml)
            Dim ret = Me.DataContext.usp_ActualizarInfFinancieraClientes_OyDNet(strxmldecodificado, pstrUsuario)
            Return Nothing
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ActualizarEstadosFinancieros")
            Return Nothing
        End Try
    End Function
    Private Function ActualizarEspecificos(ByVal strxml As String, ByVal pstrUsuario As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.tblLogReceptores)

        Try
            Dim ret = Me.DataContext.usp_ClientesReceptores_Actualizar_xmlOyDNet(strxml, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarEspecificos")
            Return Nothing
        End Try
    End Function
    Public Function ConsultaRelacionResumida(ByVal strReceptorDesde As String, ByVal dtmCorte As Date, ByVal strOwner As String, ByVal intTipoLimite As Integer, ByVal dblValorLimite As Double, ByVal lngEstadoCliente As Integer, ByVal lngIDSucursalDesde As Integer, ByVal pstrUsuario As String, ByVal intTipoCartera As Integer, ByVal pstrConsecutivo As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.tblConsultaSaldoRes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOYDNet_RelacionSaldosClientesRes_OyDNet(strReceptorDesde, dtmCorte, strOwner, intTipoLimite, dblValorLimite, lngEstadoCliente, lngIDSucursalDesde, pstrUsuario, intTipoCartera, pstrConsecutivo, DemeInfoSesion(pstrUsuario, "ConsultaRelacionResumida")).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ConsultaRelacionResumida")
            Return Nothing
        End Try
    End Function
    <Update()>
    Public Sub UpdateConsultaSaldoRes(ByVal currentCliente As OyDClientes.tblConsultaSaldoRes)
        Try
            Throw New NotImplementedException("No se puede modificar")
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsultaSaldoRes")
        End Try
    End Sub
    Public Function insertarNotaCli(ByVal strNombreconsecutivo As String, ByVal dtmDocumento As Date, ByVal logcobtabilidad As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.spInsNotaContable_OyDNet(strNombreconsecutivo, dtmDocumento, logcobtabilidad, pstrUsuario)
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "insertarNotaCli")
            Return Nothing
        End Try
    End Function
    'JFSB 20180305 - Se pone el parametro lngidbanco para que reciba nulos
    Public Function insertarDetalleNotaCli(ByVal stripo As String, ByVal strNombreconsecutivo As String, ByVal lngiddocumento As Integer, ByVal lngsecuencia As Integer, ByVal lngidcomitente As String,
                                           ByVal curvalor As Decimal, ByVal stridcuentacontable As String, ByVal strdetalle As String, ByVal lngidbanco As Nullable(Of Integer), ByVal strNit As String, ByVal strCCosto As String, ByVal strusuario As String, ByVal strnombrenotagmf As String, ByVal lngiddocumentonotagmf As Integer, ByVal lngIDConcepto As Nullable(Of Integer),
                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strnitcliente As String
            Dim strCodigo = CampoTabla(lngidcomitente, "lngNrodocumento", "tblclientes", "lngid")
            Dim stridentificacion = CampoTabla(lngidcomitente, "strtipoidentificacion", "tblclientes", "lngid")

            'JFSB 20171130 Se limpia el comitente cuando sea la contrapartida de la nota a generar
            If (lngsecuencia Mod 2) = 0 Then
                lngidcomitente = Nothing
            End If

            If stridentificacion <> "N" Then
                strnitcliente = strCodigo
            Else
                strnitcliente = Mid(strCodigo, 1, Len(strCodigo) - 1)
            End If
            Dim e = Me.DataContext.spInsNotaContableDetalle_OyDNet(stripo, strNombreconsecutivo, lngiddocumento, lngsecuencia, lngidcomitente, curvalor, stridcuentacontable, strdetalle, lngidbanco, strnitcliente, strCCosto, strusuario, strnombrenotagmf, lngiddocumentonotagmf, lngIDConcepto)
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "insertarDetalleNotaCli")
            Return Nothing
        End Try
    End Function
    Public Function ClientesConsultarPorAprobar(ByVal pIDComitente As String, ByVal pNombre As String, ByVal NroDocumento As String, ByVal strTipoIdentificacion As String, ByVal strClasificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarPendientesPorAprobar_Clientes(pIDComitente, pNombre, NroDocumento, strTipoIdentificacion, strClasificacion, DemeInfoSesion(pstrUsuario, "BuscarClientes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClientesPorAprobar")
            Return Nothing
        End Try
    End Function
    Public Function ClientesConsultarVersion(ByVal filtro As Byte, ByVal pIDComitente As String, ByVal pNombre As String, ByVal NroDocumento As String, ByVal strTipoIdentificacion As String, ByVal strClasificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Clientes_Cliente_Consultar_Version(filtro, pIDComitente, pNombre, NroDocumento, strTipoIdentificacion, strClasificacion, DemeInfoSesion(pstrUsuario, "BuscarClientesVersion"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClientesVersion")
            Return Nothing
        End Try
    End Function
    Public Function TraerClientesDocumento(ByVal Documento As String, ByVal identificacion As String, ByVal sucCliente As String, IDCliente As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesSucursales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyDNet_ConsultarNroDocumento(Documento, identificacion, sucCliente, IDCliente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesDocumento")
            Return Nothing
        End Try
    End Function
    'Public Function TraerClientesDocumentoSync(ByVal Documento As String, ByVal identificacion As String, ByVal sucCliente As String, IDCliente As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS String
    '    Dim objTask As Task(Of String) = Me.TraerClientesDocumentoASync(Documento, identificacion, sucCliente, IDCliente)
    '    objTask.Wait()
    '    Return objTask.Result
    'End Function
    'Private Function TraerClientesDocumentoASync(ByVal Documento As String, ByVal identificacion As String, ByVal sucCliente As String, ByVal IDCliente As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS Task(Of String)
    '    Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
    '    objTaskComplete.TrySetResult(TraerClientesDocumento(Documento, identificacion, sucCliente, IDCliente))
    '    Return (objTaskComplete.Task)
    'End Function

    Public Function GrabarClientesInhabilitados(ByVal strArchivo As String, ByVal strFormato As String, ByVal intIDConcepto As Integer, ByVal strUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyDNet_Importacion_ClientesInhabilitados(strArchivo, strFormato, intIDConcepto, strUsuario, DemeInfoSesion(pstrUsuario, "GrabarClientesInhabilitados"), ClsConstantes.GINT_ErrorPersonalizado)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrabarClientesInhabilitados")
            Return Nothing
        End Try
    End Function

    Public Function ConsultaClientesActivos(ByVal lngidcomitente As String, ByVal lngresultado As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesActivos_OyDNet(lngidcomitente, lngresultado)
            Return lngresultado
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultaClientesActivos")
            Return Nothing
        End Try
    End Function

    Public Function ConsultaClientesSucursal(ByVal strNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Cliente
        Dim objCliente As Cliente = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_Clientes_Sucursal_Consultar(strNroDocumento).ToList
            If ret.Count > 0 Then
                objCliente = ret.FirstOrDefault
            End If
            Return objCliente
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultaClientesSucursal")
            Return Nothing
        End Try
    End Function

    Public Function ConsultaClientesSucursalSync(ByVal strNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Cliente
        Dim objTask As Task(Of Cliente) = Me.ConsultaClientesSucursalASync(strNroDocumento, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultaClientesSucursalASync(ByVal strNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Cliente)
        Dim objTaskComplete As TaskCompletionSource(Of Cliente) = New TaskCompletionSource(Of Cliente)()
        objTaskComplete.TrySetResult(ConsultaClientesSucursal(strNroDocumento, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Funcion que sirve para leer un archivo plano y realizar algunas validaciones  JBT20130418
    ''' </summary>
    ''' <remarks></remarks>
    Public Function LeerArchvioDatosFinancieros(ByVal pstrNombreCompletoArchivo As String _
                                           , ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDClientes.tblConsultaDatosfinancieros)
        Dim archivo As System.IO.StreamReader = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim bolcargarenpantalla As Boolean
            Dim bolcaractereserrados As Boolean
            Dim LIMITE_PARAMETROS_FINANCIEROS As Double = 922337203685477
            Dim lista As New List(Of OyDClientes.tblConsultaDatosfinancieros)
            Dim contador, lngRegbuenos As Integer
            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Inicio de la importación", .Nrodocumento = "inicioimportacion" + contador.ToString})
            Dim objDatosRutas = clsClaseUploads.FnRutasImportaciones(pstrNombreProceso, pstrUsuario)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'Dim archivo As System.IO.StreamReader = Nothing
            'Recorremos el archivo de encabezados
            archivo = New System.IO.StreamReader(directorioUsuario & "\" & pstrNombreCompletoArchivo)
            Do While archivo.Peek() <> -1
                bolcargarenpantalla = True
                contador = contador + 1
                Dim e = archivo.ReadLine().Split(CChar(","))
                If contador > 1 Then
                    bolcaractereserrados = False
                    If validarcaracteres(e(0)) Then
                        lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El número de documento tiene carácteres especiales" + vbTab + "(Dato:" + e(0) + ")", .Nrodocumento = "numero especiales" + contador.ToString})
                        bolcaractereserrados = True
                    End If
                    If validarcaracteres(e(1)) Then
                        lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "Los ingresos tiene carácteres especiales" + vbTab + "(Dato:" + e(1) + ")", .Nrodocumento = "ingresos especiales" + contador.ToString})
                        bolcaractereserrados = True
                    End If
                    If validarcaracteres(e(2)) Then
                        lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "Los egresos tiene carácteres especiales" + vbTab + "(Dato:" + e(2) + ")", .Nrodocumento = "egresos especiales" + contador.ToString})
                        bolcaractereserrados = True
                    End If
                    If validarcaracteres(e(3)) Then
                        lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "Los activos tiene carácteres especiales" + vbTab + "(Dato:" + e(3) + ")", .Nrodocumento = "activos especiales" + contador.ToString})
                        bolcaractereserrados = True
                    End If
                    If validarcaracteres(e(4)) Then
                        lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "Los pasivos tiene carácteres especiales" + vbTab + "(Dato:" + e(4) + ")", .Nrodocumento = "pasivos especiales" + contador.ToString})
                        bolcaractereserrados = True
                    End If

                    If validarcaracteres(e(5)) Then
                        lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El codigo CIIU tiene carácteres especiales" + vbTab + "(Dato:" + e(5) + ")", .Nrodocumento = "codigo CIIU especiales" + contador.ToString})
                        bolcaractereserrados = True
                    End If
                    If validarcaracteres(e(6)) Then
                        lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El segmento comercial tiene carácteres especiales" + vbTab + "(Dato:" + e(6) + ")", .Nrodocumento = "segmento comercial especiales" + contador.ToString})
                        bolcaractereserrados = True
                    End If

                    If bolcaractereserrados = False Then
                        If e(0) = String.Empty Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El número del documento es requerido " + vbTab + "(Dato:" + e(0) + ")", .Nrodocumento = "numero requerido" + contador.ToString})
                        Else
                            Dim documento = CampoTabla(e(0), "strNroDocumento", "tblclientes", "strNroDocumento")
                            If String.IsNullOrEmpty(documento) Then
                                bolcargarenpantalla = False
                                lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El número del documento no existe en OYD.net " + vbTab + "(Dato:" + e(0) + ")", .Nrodocumento = "numero no exitse" + contador.ToString})
                            End If
                        End If
                        If e(1) = String.Empty Then
                            e(1) = "0"
                        ElseIf Not IsNumeric(e(1)) Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "Los ingresos deben ser un valor numérico" + vbTab + "(Dato:" + e(1) + ")", .Nrodocumento = "ingresos numerico" + contador.ToString})
                        ElseIf CDbl(e(1)) > LIMITE_PARAMETROS_FINANCIEROS Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El valor de Los ingresos excede el limite permitido (" + Format(LIMITE_PARAMETROS_FINANCIEROS, "$#,##0") + ")" + vbTab + "(Dato:" + e(1) + ")", .Nrodocumento = "ingresos excede " + contador.ToString})
                        End If
                        If e(2) = String.Empty Then
                            e(2) = "0"
                        ElseIf Not IsNumeric(e(2)) Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "Los egresos deben ser un valor numérico" + vbTab + "(Dato:" + e(2) + ")", .Nrodocumento = "egresos numerico" + contador.ToString})
                        ElseIf CDbl(e(2)) > LIMITE_PARAMETROS_FINANCIEROS Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El valor de Los egresos excede el limite permitido (" + Format(LIMITE_PARAMETROS_FINANCIEROS, "$#,##0") + ")" + vbTab + "(Dato:" + e(2) + ")", .Nrodocumento = "egresos excede" + contador.ToString})
                        End If

                        If e(3) = String.Empty Then
                            e(3) = "0"
                        ElseIf Not IsNumeric(e(3)) Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "Los activos deben ser un valor numérico" + vbTab + "(Dato:" + e(3) + ")", .Nrodocumento = "activos numerico" + contador.ToString})
                        ElseIf CDbl(e(3)) > LIMITE_PARAMETROS_FINANCIEROS Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El valor de Los activos excede el limite permitido (" + Format(LIMITE_PARAMETROS_FINANCIEROS, "$#,##0") + ")" + vbTab + "(Dato:" + e(3) + ")", .Nrodocumento = "activos excede" + contador.ToString})
                        End If

                        If e(4) = String.Empty Then
                            e(4) = "0"
                        ElseIf Not IsNumeric(e(4)) Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "Los pasivos deben ser un valor numérico" + vbTab + "(Dato:" + e(4) + ")", .Nrodocumento = "pasivos numerico" + contador.ToString})
                        ElseIf CDbl(e(4)) > LIMITE_PARAMETROS_FINANCIEROS Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El valor de Los pasivos excede el limite permitido (" + Format(LIMITE_PARAMETROS_FINANCIEROS, "$#,##0") + ")" + vbTab + "(Dato:" + e(4) + ")", .Nrodocumento = "pasivos excede" + contador.ToString})
                        End If

                        If e(5) <> String.Empty And Not IsNumeric(e(5)) Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El código CIIU debe ser un valor numérico" + vbTab + "(Dato:" + e(5) + ")", .Nrodocumento = "código CIIU numerico" + contador.ToString})
                        Else
                            If e(5) <> String.Empty And IsNumeric(e(5)) Then
                                Dim codigocciu = CampoTabla(e(5), "strCodigo", "tblcodigos_ciiu", "strCodigo")
                                If String.IsNullOrEmpty(codigocciu) Then
                                    bolcargarenpantalla = False
                                    lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El codigo CIIU no existe en OYD.net " + vbTab + "(Dato:" + e(5) + ")", .Nrodocumento = "código CIIU no existe" + contador.ToString})
                                End If
                            Else
                                e(5) = "0"
                            End If
                        End If

                        If e(6) <> String.Empty And Not IsNumeric(e(6)) Then
                            bolcargarenpantalla = False
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El segmento comercial debe ser un valor numérico" + vbTab + "(Dato:" + e(6) + ")", .Nrodocumento = "segmento comercial numerico" + contador.ToString})
                        Else
                            If e(6) <> String.Empty And IsNumeric(e(6)) Then
                                Dim segmentocomercial = CampoTabla(e(6), "strretorno", "tblLista", "strretorno")
                                If String.IsNullOrEmpty(segmentocomercial) Then
                                    bolcargarenpantalla = False
                                    lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Línea " + contador.ToString + ":" + vbTab + "El segemento comercial no existe en OYD.net " + vbTab + "(Dato:" + e(6) + ")", .Nrodocumento = "segmento comercial no existe" + contador.ToString})
                                End If
                            Else
                                e(6) = "0"
                            End If
                        End If

                        If bolcargarenpantalla Then
                            lngRegbuenos = lngRegbuenos + 1
                            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.Nrodocumento = e(0), .Ingresos = CDbl(e(1)), .Egresos = CDbl(e(2)), .Activos = CDbl(e(3)), .Pasivos = CDbl(e(4)), .CodigoCIIU = e(5), .SegmentoComercial = e(6)})
                        End If
                    End If
                End If

            Loop
            archivo.Close()
            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Total de registros Leidos " + (contador - 1).ToString, .Nrodocumento = "Leidos" + contador.ToString})
            lista.Add(New OyDClientes.tblConsultaDatosfinancieros With {.ListaComentario = "Total de registros importados " + (lngRegbuenos).ToString, .Nrodocumento = "Importados" + lngRegbuenos.ToString})
            Return lista
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "LeerArchvioDatosFinancieros")
            Return Nothing
        Finally
            archivo.Dispose()
        End Try
    End Function
    Public Function CalcularDigitoVerificacion(ByVal pstrNit As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim intDigito As Nullable(Of Integer)
            Dim ret = Me.DataContext.spCalcularDigitoNIT(pstrNit, intDigito)
            Return CInt(intDigito)
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "CalcularDigitoVerificacion")
            Return Nothing
        End Try
    End Function
    Public Function ValidarClienteTipoProducto(ByVal pstrNroDocumento As String, ByVal pstrTipoProducto As String, ByVal plngid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pintCantTipoProducto As Nullable(Of Integer)
            Dim ret = Me.DataContext.usp_ValidarCliente_TipoProducto_OydNet(pstrNroDocumento, pstrTipoProducto, plngid, pintCantTipoProducto, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarClienteTipoProducto"), 0)
            Return CInt(pintCantTipoProducto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ValidarClienteTipoProducto")
            Return Nothing
        End Try
    End Function
    Public Function DuplicarCliente(ByVal pstrNroIdentificacion As String, ByVal pstrNombre1 As String, ByVal pstrNombre2 As String, ByVal pstrApellido1 As String, ByVal pstrApellido2 As String, ByVal pstrNombreCompleto As String, ByVal pstrTipoProducto As String, ByVal pstrTipoCliente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strCodigoClienteDestino As String = String.Empty
            Dim ret = Me.DataContext.spClientesDuplicar_OyDNet(pstrNroIdentificacion, pstrNombre1, pstrNombre2, pstrApellido1, pstrApellido2, pstrNombreCompleto, pstrTipoProducto, pstrTipoCliente, strCodigoClienteDestino, pstrUsuario, DemeInfoSesion(pstrUsuario, "DuplicarCliente"), 0)
            Return strCodigoClienteDestino
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "DuplicarCliente")
            Return Nothing
        End Try
    End Function
    'Jorge Andrés Bedoya 2013/09/11
    Public Sub UpdateEntidadesClientes(ByVal currentEntidadesCliente As A2.OyD.OYDServer.RIA.Web.OyDClientes.EntidadesClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentEntidadesCliente.pstrUsuarioConexion, currentEntidadesCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'currentEntidadesCliente.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateCuentaCliente")
            Me.DataContext.EntidadesClientes.Attach(currentEntidadesCliente, Me.ChangeSet.GetOriginal(currentEntidadesCliente))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEntidadesCliente")
        End Try
    End Sub
    Public Function ConsultaClientesEntidades(ByVal idComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.EntidadesClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarInfEntidadesClientes_OyDNet(idComitente).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultaClientesEntidades")
            Return Nothing
        End Try
    End Function
    Public Function ClientesValidaciones(ByVal plngIDCiudad As Integer, ByVal plngIDPais As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByVal plngIDComitente As String, ByVal pstrNroDocumento As String, ByVal pstrInstruccionCRCC As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.tblRespuestaValidacionesCientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Validaciones_Clientes(plngIDCiudad, plngIDPais, pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesValidaciones"), 0, plngIDComitente, pstrNroDocumento, pstrInstruccionCRCC).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesValidaciones")
            Return Nothing
        End Try
    End Function
    Public Function ClientesEnvioMail(ByVal plngIdComitente As String, ByVal PlngIDPais As Integer, ByVal PlngIDCiudad As Integer, ByVal pstrMensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_EnviarCorreos_Clasificacionriesgos_Generar(plngIdComitente, PlngIDPais, PlngIDCiudad, pstrMensaje)
            Return pstrMensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesEnvioMail")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarAsociacionOrdenantes(ByVal pstrOrdenante As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.RespuestaOrdenantesAsociados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOYDNet_ConsultarAsociacionOrdenantes(pstrOrdenante, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarAsociacionOrdenantes"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarAsociacionOrdenantes")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarClientesRelacionados(ByVal plngIdComitente As String, ByVal lngresultado As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spOyDNet_Consultar_ClientesRelacionados(plngIdComitente, lngresultado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarClientesRelacionados"), 0)
            Return lngresultado
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ConsultarClientesRelacionados")
            Return Nothing
        End Try
    End Function
#Region "Tablas  Hijas"
#Region "CuentasClientes"
    Public Function Traer_CuentasClientes(ByVal Filtro As Byte, ByVal pComitente As String, ByVal plogEditandoRegistro As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.CuentasCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_CuentasClientes_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_CuentasClientes"), ClsConstantes.GINT_ErrorPersonalizado, plogEditandoRegistro).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_CuentasClientes")
        End Try
        Return Nothing
    End Function

    Public Sub UpdateCuentaCliente(ByVal currentCuentaCliente As A2.OyD.OYDServer.RIA.Web.OyDClientes.CuentasCliente)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCuentaCliente.pstrUsuarioConexion, currentCuentaCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentCuentaCliente.Estado) Then
                If currentCuentaCliente.Estado.Equals("Retiro") Then
                    currentCuentaCliente.InfoSesion = DemeInfoSesion(currentCuentaCliente.pstrUsuarioConexion, "UpdateCuentaCliente")
                    Me.DataContext.CuentasClientes.Attach(currentCuentaCliente)
                    Me.DataContext.CuentasClientes.DeleteOnSubmit(currentCuentaCliente)
                Else
                    currentCuentaCliente.InfoSesion = DemeInfoSesion(currentCuentaCliente.pstrUsuarioConexion, "UpdateCuentaCliente")
                    Me.DataContext.CuentasClientes.Attach(currentCuentaCliente, Me.ChangeSet.GetOriginal(currentCuentaCliente))
                End If
            Else
                currentCuentaCliente.InfoSesion = DemeInfoSesion(currentCuentaCliente.pstrUsuarioConexion, "UpdateCuentaCliente")
                Me.DataContext.CuentasClientes.Attach(currentCuentaCliente, Me.ChangeSet.GetOriginal(currentCuentaCliente))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentaCliente")
        End Try
    End Sub

    Public Sub InsertCuentaCliente(ByVal CuentaCliente As A2.OyD.OYDServer.RIA.Web.OyDClientes.CuentasCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, CuentaCliente.pstrUsuarioConexion, CuentaCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CuentaCliente.InfoSesion = DemeInfoSesion(CuentaCliente.pstrUsuarioConexion, "InsertCuentaCliente")
            Me.DataContext.CuentasClientes.InsertOnSubmit(CuentaCliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCuentaCliente")
        End Try
    End Sub

    Public Sub DeleteCuentaCliente(ByVal CuentaCliente As A2.OyD.OYDServer.RIA.Web.OyDClientes.CuentasCliente)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CuentaCliente.pstrUsuarioConexion, CuentaCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            CuentaCliente.InfoSesion = DemeInfoSesion(CuentaCliente.pstrUsuarioConexion, "DeleteCuentaCliente")
            Me.DataContext.CuentasClientes.Attach(CuentaCliente)
            Me.DataContext.CuentasClientes.DeleteOnSubmit(CuentaCliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCuentaCliente")
        End Try
    End Sub
#End Region
#Region "ClientesReceptore"
    Public Function Traer_ClientesReceptore(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesReceptore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesReceptores_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesReceptore"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesReceptore")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesReceptores(ByVal currentClientesReceptores As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesReceptore)

        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesReceptores.pstrUsuarioConexion, currentClientesReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesReceptores.Estado) Then
                If currentClientesReceptores.Estado.Equals("Retiro") Then
                    currentClientesReceptores.InfoSesion = DemeInfoSesion(currentClientesReceptores.pstrUsuarioConexion, "UpdateClientesReceptores")
                    Me.DataContext.ClientesReceptores.Attach(currentClientesReceptores)
                    Me.DataContext.ClientesReceptores.DeleteOnSubmit(currentClientesReceptores)
                Else
                    currentClientesReceptores.InfoSesion = DemeInfoSesion(currentClientesReceptores.pstrUsuarioConexion, "UpdateClientesReceptores")
                    Me.DataContext.ClientesReceptores.Attach(currentClientesReceptores, Me.ChangeSet.GetOriginal(currentClientesReceptores))
                End If
            Else
                currentClientesReceptores.InfoSesion = DemeInfoSesion(currentClientesReceptores.pstrUsuarioConexion, "UpdateClientesReceptores")
                Me.DataContext.ClientesReceptores.Attach(currentClientesReceptores, Me.ChangeSet.GetOriginal(currentClientesReceptores))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesReceptores")
        End Try
    End Sub

    Public Sub InsertClientesReceptores(ByVal ClientesReceptores As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesReceptore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesReceptores.pstrUsuarioConexion, ClientesReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesReceptores.InfoSesion = DemeInfoSesion(ClientesReceptores.pstrUsuarioConexion, "InsertClientesReceptores")
            Me.DataContext.ClientesReceptores.InsertOnSubmit(ClientesReceptores)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesReceptores")
        End Try
    End Sub

    Public Sub DeleteClientesReceptores(ByVal ClientesReceptores As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesReceptore)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesReceptores.pstrUsuarioConexion, ClientesReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesReceptores.InfoSesion = DemeInfoSesion(ClientesReceptores.pstrUsuarioConexion, "DeleteClientesReceptores")
            Me.DataContext.ClientesReceptores.Attach(ClientesReceptores)
            Me.DataContext.ClientesReceptores.DeleteOnSubmit(ClientesReceptores)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesReceptores")
        End Try
    End Sub

#End Region
#Region "Clientes Ordenantes"

    Public Function Traer_ClientesOrdenante(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pOrdenante As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesOrdenantes_Consultar(Filtro, pComitente, pOrdenante, DemeInfoSesion(pstrUsuario, "Traer_ClientesOrdenante"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesOrdenante")
        End Try
        Return Nothing
    End Function

    Public Function Traer_ClientesOrdenanteSync(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pOrdenante As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante)
        Dim objTask As Task(Of List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante)) = Me.Traer_ClientesOrdenanteASync(Filtro, pComitente, pOrdenante, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Traer_ClientesOrdenanteASync(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pOrdenante As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante)) = New TaskCompletionSource(Of List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante))()
        objTaskComplete.TrySetResult(Traer_ClientesOrdenante(Filtro, pComitente, pOrdenante, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


    Public Sub UpdateClientesOrdenantes(ByVal currentClientesOrdenantes As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesOrdenantes.pstrUsuarioConexion, currentClientesOrdenantes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesOrdenantes.Estado) Then
                If currentClientesOrdenantes.Estado.Equals("Retiro") Then
                    currentClientesOrdenantes.InfoSesion = DemeInfoSesion(currentClientesOrdenantes.pstrUsuarioConexion, "UpdateClientesOrdenantes")
                    Me.DataContext.ClientesOrdenantes.Attach(currentClientesOrdenantes)
                    Me.DataContext.ClientesOrdenantes.DeleteOnSubmit(currentClientesOrdenantes)
                Else
                    currentClientesOrdenantes.InfoSesion = DemeInfoSesion(currentClientesOrdenantes.pstrUsuarioConexion, "UpdateClientesOrdenantes")
                    Me.DataContext.ClientesOrdenantes.Attach(currentClientesOrdenantes, Me.ChangeSet.GetOriginal(currentClientesOrdenantes))
                End If
            Else
                currentClientesOrdenantes.InfoSesion = DemeInfoSesion(currentClientesOrdenantes.pstrUsuarioConexion, "UpdateClientesOrdenantes")
                Me.DataContext.ClientesOrdenantes.Attach(currentClientesOrdenantes, Me.ChangeSet.GetOriginal(currentClientesOrdenantes))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesOrdenantes")
        End Try
    End Sub

    Public Sub InsertClientesOrdenantes(ByVal ClientesOrdenantes As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesOrdenantes.pstrUsuarioConexion, ClientesOrdenantes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesOrdenantes.InfoSesion = DemeInfoSesion(ClientesOrdenantes.pstrUsuarioConexion, "InsertClientesOrdenantes")
            Me.DataContext.ClientesOrdenantes.InsertOnSubmit(ClientesOrdenantes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesOrdenantes")
        End Try
    End Sub

    Public Sub DeleteClientesOrdenantes(ByVal ClientesOrdenantes As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenante)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesOrdenantes.pstrUsuarioConexion, ClientesOrdenantes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesOrdenantes.InfoSesion = DemeInfoSesion(ClientesOrdenantes.pstrUsuarioConexion, "DeleteClientesOrdenantes")
            Me.DataContext.ClientesOrdenantes.Attach(ClientesOrdenantes)
            Me.DataContext.ClientesOrdenantes.DeleteOnSubmit(ClientesOrdenantes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesOrdenantes")
        End Try
    End Sub

#End Region
#Region "Clientes Beneficiarios"

    Public Function Traer_ClientesBeneficiarios(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesBeneficiarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesBeneficiarios_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesBeneficiarios"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesBeneficiarios")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesBeneficiarios(ByVal currentClientesBeneficiarios As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesBeneficiarios)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesBeneficiarios.pstrUsuarioConexion, currentClientesBeneficiarios.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesBeneficiarios.Estado) Then
                If currentClientesBeneficiarios.Estado.Equals("Retiro") Then
                    currentClientesBeneficiarios.InfoSesion = DemeInfoSesion(currentClientesBeneficiarios.pstrUsuarioConexion, "UpdateClientesBeneficiarios")
                    Me.DataContext.ClientesBeneficiarios.Attach(currentClientesBeneficiarios)
                    Me.DataContext.ClientesBeneficiarios.DeleteOnSubmit(currentClientesBeneficiarios)
                Else
                    'currentClientesBeneficiarios.Nombre = currentClientesBeneficiarios.Apellido1 + " " + currentClientesBeneficiarios.Apellido2 + " " + currentClientesBeneficiarios.Nombre1 + " " + currentClientesBeneficiarios.Nombre2
                    currentClientesBeneficiarios.InfoSesion = DemeInfoSesion(currentClientesBeneficiarios.pstrUsuarioConexion, "UpdateClientesBeneficiarios")
                    Me.DataContext.ClientesBeneficiarios.Attach(currentClientesBeneficiarios, Me.ChangeSet.GetOriginal(currentClientesBeneficiarios))
                End If
            Else
                'currentClientesBeneficiarios.Nombre = currentClientesBeneficiarios.Apellido1 + " " + currentClientesBeneficiarios.Apellido2 + " " + currentClientesBeneficiarios.Nombre1 + " " + currentClientesBeneficiarios.Nombre2
                currentClientesBeneficiarios.InfoSesion = DemeInfoSesion(currentClientesBeneficiarios.pstrUsuarioConexion, "UpdateClientesBeneficiarios")
                Me.DataContext.ClientesBeneficiarios.Attach(currentClientesBeneficiarios, Me.ChangeSet.GetOriginal(currentClientesBeneficiarios))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesBeneficiarios")
        End Try
    End Sub

    Public Sub InsertClientesBeneficiarios(ByVal ClientesBeneficiarios As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesBeneficiarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesBeneficiarios.pstrUsuarioConexion, ClientesBeneficiarios.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesBeneficiarios.Nombre = ClientesBeneficiarios.Apellido1 + " " + ClientesBeneficiarios.Apellido2 + " " + ClientesBeneficiarios.Nombre1 + " " + ClientesBeneficiarios.Nombre2
            ClientesBeneficiarios.InfoSesion = DemeInfoSesion(ClientesBeneficiarios.pstrUsuarioConexion, "InsertClientesBeneficiarios")
            Me.DataContext.ClientesBeneficiarios.InsertOnSubmit(ClientesBeneficiarios)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesBeneficiarios")
        End Try
    End Sub

    Public Sub DeleteClientesBeneficiarios(ByVal ClientesBeneficiarios As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesBeneficiarios)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesBeneficiarios.pstrUsuarioConexion, ClientesBeneficiarios.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesBeneficiarios.InfoSesion = DemeInfoSesion(ClientesBeneficiarios.pstrUsuarioConexion, "DeleteClientesBeneficiarios")
            Me.DataContext.ClientesBeneficiarios.Attach(ClientesBeneficiarios)
            Me.DataContext.ClientesBeneficiarios.DeleteOnSubmit(ClientesBeneficiarios)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesBeneficiarios")
        End Try
    End Sub

#End Region
#Region "Clientes Aficiones"

    Public Function Traer_ClientesAficiones(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAficiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesAficiones_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesAficiones"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesAficiones")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesAficiones(ByVal currentClientesAficiones As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAficiones)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesAficiones.pstrUsuarioConexion, currentClientesAficiones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesAficiones.Estado) Then
                If currentClientesAficiones.Estado.Equals("Retiro") Then
                    currentClientesAficiones.InfoSesion = DemeInfoSesion(currentClientesAficiones.pstrUsuarioConexion, "UpdateCuentaCliente")
                    Me.DataContext.ClientesAficiones.Attach(currentClientesAficiones)
                    Me.DataContext.ClientesAficiones.DeleteOnSubmit(currentClientesAficiones)
                Else
                    currentClientesAficiones.InfoSesion = DemeInfoSesion(currentClientesAficiones.pstrUsuarioConexion, "UpdateCuentaCliente")
                    Me.DataContext.ClientesAficiones.Attach(currentClientesAficiones, Me.ChangeSet.GetOriginal(currentClientesAficiones))
                End If
            Else
                'SE implementa esta forma de guardar y eliminar debido a que existen grids que no tienen boton nuevo ni elimina solo se marcan y desmarcan JBT
                If currentClientesAficiones.Seleccion Then
                    currentClientesAficiones.InfoSesion = DemeInfoSesion(currentClientesAficiones.pstrUsuarioConexion, "UpdateClientesAficiones")
                    Me.DataContext.ClientesAficiones.Attach(currentClientesAficiones, Me.ChangeSet.GetOriginal(currentClientesAficiones))
                Else
                    currentClientesAficiones.InfoSesion = DemeInfoSesion(currentClientesAficiones.pstrUsuarioConexion, "DeleteClientesAficiones")
                    Me.DataContext.ClientesAficiones.Attach(currentClientesAficiones)
                    Me.DataContext.ClientesAficiones.DeleteOnSubmit(currentClientesAficiones)
                End If

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentaCliente")
        End Try
    End Sub

    Public Sub InsertClientesAficiones(ByVal ClientesAficiones As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAficiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesAficiones.pstrUsuarioConexion, ClientesAficiones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesAficiones.InfoSesion = DemeInfoSesion(ClientesAficiones.pstrUsuarioConexion, "InsertClientesAficiones")
            Me.DataContext.ClientesAficiones.InsertOnSubmit(ClientesAficiones)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesAficiones")
        End Try
    End Sub

    Public Sub DeleteClientesAficiones(ByVal ClientesAficiones As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAficiones)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesAficiones.pstrUsuarioConexion, ClientesAficiones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesAficiones.InfoSesion = DemeInfoSesion(ClientesAficiones.pstrUsuarioConexion, "DeleteClientesAficiones")
            Me.DataContext.ClientesAficiones.Attach(ClientesAficiones)
            Me.DataContext.ClientesAficiones.DeleteOnSubmit(ClientesAficiones)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesAficiones")
        End Try
    End Sub

#End Region
#Region "Clientes Deportes"

    Public Function Traer_ClientesDeportes(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDeportes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesDeportes_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesDeportes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesDeportes")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesDeportes(ByVal currentClientesDeportes As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDeportes)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesDeportes.pstrUsuarioConexion, currentClientesDeportes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesDeportes.Estado) Then
                If currentClientesDeportes.Estado.Equals("Retiro") Then
                    currentClientesDeportes.InfoSesion = DemeInfoSesion(currentClientesDeportes.pstrUsuarioConexion, "UpdateClientesDeportes")
                    Me.DataContext.ClientesDeportes.Attach(currentClientesDeportes)
                    Me.DataContext.ClientesDeportes.DeleteOnSubmit(currentClientesDeportes)
                Else
                    currentClientesDeportes.InfoSesion = DemeInfoSesion(currentClientesDeportes.pstrUsuarioConexion, "UpdateCuentaCliente")
                    Me.DataContext.ClientesDeportes.Attach(currentClientesDeportes, Me.ChangeSet.GetOriginal(currentClientesDeportes))
                End If
            Else
                'SE implementa esta forma de guardar y eliminar debido a que existen grids que no tienen boton nuevo ni elimina solo se marcan y desmarcan JBT
                If currentClientesDeportes.Seleccion Then
                    currentClientesDeportes.InfoSesion = DemeInfoSesion(currentClientesDeportes.pstrUsuarioConexion, "UpdateClientesDeportes")
                    Me.DataContext.ClientesDeportes.Attach(currentClientesDeportes, Me.ChangeSet.GetOriginal(currentClientesDeportes))
                Else
                    currentClientesDeportes.InfoSesion = DemeInfoSesion(currentClientesDeportes.pstrUsuarioConexion, "DeleteClientesDeportes")
                    Me.DataContext.ClientesDeportes.Attach(currentClientesDeportes)
                    Me.DataContext.ClientesDeportes.DeleteOnSubmit(currentClientesDeportes)
                End If
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentaCliente")
        End Try
    End Sub

    Public Sub InsertClientesDeportes(ByVal ClientesDeportes As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDeportes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesDeportes.pstrUsuarioConexion, ClientesDeportes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesDeportes.InfoSesion = DemeInfoSesion(ClientesDeportes.pstrUsuarioConexion, "InsertClientesDeportes")
            Me.DataContext.ClientesDeportes.InsertOnSubmit(ClientesDeportes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesDeportes")
        End Try
    End Sub

    Public Sub DeleteClientesDeportes(ByVal ClientesDeportes As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDeportes)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesDeportes.pstrUsuarioConexion, ClientesDeportes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesDeportes.InfoSesion = DemeInfoSesion(ClientesDeportes.pstrUsuarioConexion, "DeleteClientesDeportes")
            Me.DataContext.ClientesDeportes.Attach(ClientesDeportes)
            Me.DataContext.ClientesDeportes.DeleteOnSubmit(ClientesDeportes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesDeportes")
        End Try
    End Sub

#End Region
#Region "Clientes Direcciones"

    Public Function Traer_ClientesDirecciones(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDireccione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesDirecciones_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesDirecciones"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesDirecciones")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesDirecciones(ByVal currentClientesDirecciones As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDireccione)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesDirecciones.pstrUsuarioConexion, currentClientesDirecciones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesDirecciones.Estado) Then
                If currentClientesDirecciones.Estado.Equals("Retiro") Then
                    currentClientesDirecciones.InfoSesion = DemeInfoSesion(currentClientesDirecciones.pstrUsuarioConexion, "UpdateClientesDirecciones")
                    Me.DataContext.ClientesDirecciones.Attach(currentClientesDirecciones)
                    Me.DataContext.ClientesDirecciones.DeleteOnSubmit(currentClientesDirecciones)
                Else
                    currentClientesDirecciones.InfoSesion = DemeInfoSesion(currentClientesDirecciones.pstrUsuarioConexion, "UpdateClientesDirecciones")
                    Me.DataContext.ClientesDirecciones.Attach(currentClientesDirecciones, Me.ChangeSet.GetOriginal(currentClientesDirecciones))
                End If
            Else
                currentClientesDirecciones.InfoSesion = DemeInfoSesion(currentClientesDirecciones.pstrUsuarioConexion, "UpdateClientesDirecciones")
                Me.DataContext.ClientesDirecciones.Attach(currentClientesDirecciones, Me.ChangeSet.GetOriginal(currentClientesDirecciones))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesDirecciones")
        End Try
    End Sub

    Public Sub InsertClientesDireccione(ByVal ClientesDireccione As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDireccione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesDireccione.pstrUsuarioConexion, ClientesDireccione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesDireccione.InfoSesion = DemeInfoSesion(ClientesDireccione.pstrUsuarioConexion, "InsertClientesDireccione")
            Me.DataContext.ClientesDirecciones.InsertOnSubmit(ClientesDireccione)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesDireccione")
        End Try
    End Sub

    Public Sub DeleteClientesDirecciones(ByVal ClientesDireccione As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDireccione)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesDireccione.pstrUsuarioConexion, ClientesDireccione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesDireccione.InfoSesion = DemeInfoSesion(ClientesDireccione.pstrUsuarioConexion, "DeleteClientesDirecciones")
            Me.DataContext.ClientesDirecciones.Attach(ClientesDireccione)
            Me.DataContext.ClientesDirecciones.DeleteOnSubmit(ClientesDireccione)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesDirecciones")
        End Try
    End Sub

#End Region
#Region "Clientes DocumentosRequeridos"

    Public Function Traer_ClientesDocumentosRequeridos(ByVal pComitente As String, ByVal intTipoPersona As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDocumentosRequeridos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesDocumentosR_Consultar(pComitente, intTipoPersona, DemeInfoSesion(pstrUsuario, "Traer_ClientesDocumentosRequeridos2"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesDocumentosRequeridos")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesDocumentosRequeridos(ByVal currentClientesDocumentosRequeridos As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDocumentosRequeridos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesDocumentosRequeridos.pstrUsuarioConexion, currentClientesDocumentosRequeridos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'SE implementa esta forma de guardar y eliminar debido a que existen grids que no tienen boton nuevo ni elimina solo se marcan y desmarcan JBT
            If currentClientesDocumentosRequeridos.Entregado Then
                currentClientesDocumentosRequeridos.InfoSesion = DemeInfoSesion(currentClientesDocumentosRequeridos.pstrUsuarioConexion, "UpdateClientesDocumentosRequeridos")
                Me.DataContext.ClientesDocumentosRequeridos.Attach(currentClientesDocumentosRequeridos, Me.ChangeSet.GetOriginal(currentClientesDocumentosRequeridos))
            Else
                currentClientesDocumentosRequeridos.InfoSesion = DemeInfoSesion(currentClientesDocumentosRequeridos.pstrUsuarioConexion, "DeleteClientesDocumentosRequeridos")
                Me.DataContext.ClientesDocumentosRequeridos.Attach(currentClientesDocumentosRequeridos)
                Me.DataContext.ClientesDocumentosRequeridos.DeleteOnSubmit(currentClientesDocumentosRequeridos)
            End If

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesDocumentosRequeridos")
        End Try
    End Sub

    Public Sub InsertClientesDocumentosRequeridos(ByVal ClientesDocumentosRequeridos As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDocumentosRequeridos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesDocumentosRequeridos.pstrUsuarioConexion, ClientesDocumentosRequeridos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesDocumentosRequeridos.InfoSesion = DemeInfoSesion(ClientesDocumentosRequeridos.pstrUsuarioConexion, "InsertClientesDocumentosRequeridos")
            Me.DataContext.ClientesDocumentosRequeridos.InsertOnSubmit(ClientesDocumentosRequeridos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesDocumentosRequeridos")
        End Try
    End Sub

    Public Sub DeleteClientesDocumentosRequeridos(ByVal ClientesDocumentosRequeridos As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesDocumentosRequeridos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesDocumentosRequeridos.pstrUsuarioConexion, ClientesDocumentosRequeridos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesDocumentosRequeridos.InfoSesion = DemeInfoSesion(ClientesDocumentosRequeridos.pstrUsuarioConexion, "DeleteClientesDocumentosRequeridos")
            Me.DataContext.ClientesDocumentosRequeridos.Attach(ClientesDocumentosRequeridos)
            Me.DataContext.ClientesDocumentosRequeridos.DeleteOnSubmit(ClientesDocumentosRequeridos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesDocumentosRequeridos")
        End Try
    End Sub

#End Region
#Region "Clientes ClientesLOGHistoriaI"

    Public Function Traer_ClientesLOGHistoriaI(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesLOGHistoriaIR)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_LOGHistoriaIR_Consultar(pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesLOGHistoriaI"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesLOGHistoriaI")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesLOGHistoriaIs(ByVal currentClientesLOGHistoriaI As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesLOGHistoriaIR)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesLOGHistoriaI.pstrUsuarioConexion, currentClientesLOGHistoriaI.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClientesLOGHistoriaI.InfoSesion = DemeInfoSesion(currentClientesLOGHistoriaI.pstrUsuarioConexion, "UpdateClientesLOGHistoriaIs")
            Me.DataContext.ClientesLOGHistoriaIR.Attach(currentClientesLOGHistoriaI, Me.ChangeSet.GetOriginal(currentClientesLOGHistoriaI))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesLOGHistoriaIs")
        End Try
    End Sub

    Public Sub InsertClientesLOGHistoriaI(ByVal ClientesLOGHistoriaI As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesLOGHistoriaIR)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesLOGHistoriaI.pstrUsuarioConexion, ClientesLOGHistoriaI.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesLOGHistoriaI.InfoSesion = DemeInfoSesion(ClientesLOGHistoriaI.pstrUsuarioConexion, "InsertClientesLOGHistoriaI")
            Me.DataContext.ClientesLOGHistoriaIR.InsertOnSubmit(ClientesLOGHistoriaI)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesLOGHistoriaI")
        End Try
    End Sub

    Public Sub DeleteClientesLOGHistoriaI(ByVal ClientesLOGHistoriaI As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesLOGHistoriaIR)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesLOGHistoriaI.pstrUsuarioConexion, ClientesLOGHistoriaI.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesLOGHistoriaI.InfoSesion = DemeInfoSesion(ClientesLOGHistoriaI.pstrUsuarioConexion, "DeleteClientesLOGHistoriaI")
            Me.DataContext.ClientesLOGHistoriaIR.Attach(ClientesLOGHistoriaI)
            Me.DataContext.ClientesLOGHistoriaIR.DeleteOnSubmit(ClientesLOGHistoriaI)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesLOGHistoriaI")
        End Try
    End Sub

#End Region
#Region "Clientes ClientesConocimientoEspecifico"

    Public Function Traer_ClientesConocimientoEspecifico(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesConocimientoEspecifico)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ConocimientoEspecifico_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesConocimientoEspecifico"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesConocimientoEspecifico")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesConocimientoEspecifico(ByVal currentClientesConocimientoEspecifico As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesConocimientoEspecifico)


        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesConocimientoEspecifico.pstrUsuarioConexion, currentClientesConocimientoEspecifico.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesConocimientoEspecifico.Estado) Then
                If currentClientesConocimientoEspecifico.Estado.Equals("Retiro") Then
                    currentClientesConocimientoEspecifico.InfoSesion = DemeInfoSesion(currentClientesConocimientoEspecifico.pstrUsuarioConexion, "UpdateClientesConocimientoEspecifico")
                    Me.DataContext.ClientesConocimientoEspecifico.Attach(currentClientesConocimientoEspecifico)
                    Me.DataContext.ClientesConocimientoEspecifico.DeleteOnSubmit(currentClientesConocimientoEspecifico)
                Else
                    currentClientesConocimientoEspecifico.InfoSesion = DemeInfoSesion(currentClientesConocimientoEspecifico.pstrUsuarioConexion, "UpdateClientesConocimientoEspecifico")
                    Me.DataContext.ClientesConocimientoEspecifico.Attach(currentClientesConocimientoEspecifico, Me.ChangeSet.GetOriginal(currentClientesConocimientoEspecifico))
                End If
            Else
                currentClientesConocimientoEspecifico.InfoSesion = DemeInfoSesion(currentClientesConocimientoEspecifico.pstrUsuarioConexion, "UpdateClientesConocimientoEspecifico")
                Me.DataContext.ClientesConocimientoEspecifico.Attach(currentClientesConocimientoEspecifico, Me.ChangeSet.GetOriginal(currentClientesConocimientoEspecifico))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesConocimientoEspecifico")
        End Try
    End Sub

    Public Sub InsertClientesConocimientoEspecifico(ByVal ClientesConocimientoEspecifico As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesConocimientoEspecifico)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesConocimientoEspecifico.pstrUsuarioConexion, ClientesConocimientoEspecifico.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesConocimientoEspecifico.InfoSesion = DemeInfoSesion(ClientesConocimientoEspecifico.pstrUsuarioConexion, "InsertClientesConocimientoEspecifico")
            Me.DataContext.ClientesConocimientoEspecifico.InsertOnSubmit(ClientesConocimientoEspecifico)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesConocimientoEspecifico")
        End Try
    End Sub

    Public Sub DeleteClientesConocimientoEspecifico(ByVal ClientesConocimientoEspecifico As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesConocimientoEspecifico)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesConocimientoEspecifico.pstrUsuarioConexion, ClientesConocimientoEspecifico.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesConocimientoEspecifico.InfoSesion = DemeInfoSesion(ClientesConocimientoEspecifico.pstrUsuarioConexion, "DeleteClientesConocimientoEspecifico")
            Me.DataContext.ClientesConocimientoEspecifico.Attach(ClientesConocimientoEspecifico)
            Me.DataContext.ClientesConocimientoEspecifico.DeleteOnSubmit(ClientesConocimientoEspecifico)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesConocimientoEspecifico")
        End Try
    End Sub

#End Region
#Region "TipoCliente"

    Public Function TipoClienteFiltrar(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.TipoClient)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Clientes_TipoCliente_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "TipoClienteFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoClienteFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerTipoClientPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As A2.OyD.OYDServer.RIA.Web.OyDClientes.TipoClient
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New A2.OyD.OYDServer.RIA.Web.OyDClientes.TipoClient
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IDComitente = 
            'e.IDTipoEntidad = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.IDTipoCliente = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoClientPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTipoClient(ByVal TipoClient As A2.OyD.OYDServer.RIA.Web.OyDClientes.TipoClient)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, TipoClient.pstrUsuarioConexion, TipoClient.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TipoClient.InfoSesion = DemeInfoSesion(TipoClient.pstrUsuarioConexion, "InsertTipoClient")
            Me.DataContext.TipoCliente.InsertOnSubmit(TipoClient)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTipoClient")
        End Try
    End Sub

    Public Sub UpdateTipoClient(ByVal currentTipoClient As A2.OyD.OYDServer.RIA.Web.OyDClientes.TipoClient)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentTipoClient.pstrUsuarioConexion, currentTipoClient.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentTipoClient.Estado) Then
                If currentTipoClient.Estado.Equals("Retiro") Then
                    currentTipoClient.InfoSesion = DemeInfoSesion(currentTipoClient.pstrUsuarioConexion, "UpdateTipoClient")
                    Me.DataContext.TipoCliente.Attach(currentTipoClient)
                    Me.DataContext.TipoCliente.DeleteOnSubmit(currentTipoClient)
                Else
                    currentTipoClient.InfoSesion = DemeInfoSesion(currentTipoClient.pstrUsuarioConexion, "UpdateTipoClient")
                    Me.DataContext.TipoCliente.Attach(currentTipoClient, Me.ChangeSet.GetOriginal(currentTipoClient))
                End If
            Else
                currentTipoClient.InfoSesion = DemeInfoSesion(currentTipoClient.pstrUsuarioConexion, "UpdateTipoClient")
                Me.DataContext.TipoCliente.Attach(currentTipoClient, Me.ChangeSet.GetOriginal(currentTipoClient))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTipoClient")
        End Try
    End Sub

    Public Sub DeleteTipoClient(ByVal TipoClient As A2.OyD.OYDServer.RIA.Web.OyDClientes.TipoClient)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoClient.pstrUsuarioConexion, TipoClient.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_TipoCliente_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteTipoClient"),0).ToList
            TipoClient.InfoSesion = DemeInfoSesion(TipoClient.pstrUsuarioConexion, "DeleteTipoClient")
            Me.DataContext.TipoCliente.Attach(TipoClient)
            Me.DataContext.TipoCliente.DeleteOnSubmit(TipoClient)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTipoClient")
        End Try
    End Sub
#End Region
#Region "Portafolio"
    Public Function TraerPortafolio(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Portafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Portafolio_Clientes_Calcular_OyDNet(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerPortafolio")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Tesoreria"
    Public Function TraerClientesTesoreria(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerTesoreria_OyDNet(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesTesoreria")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Custodias"
    Public Function TraerClientesCustodias(ByVal pComitente As String, ByVal dtmRecibo As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesCustodias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerCustodias_OyDNet(pComitente, dtmRecibo).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesCustodias")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Deposito"
    Public Function TraerClientesDeposito(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.CuentasDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerCuentasDeceval_OyDNet(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesDeposito")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Acciones"
    Public Function TraerClientesAcciones(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAcciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerLiqAcciones_OyDNet(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesAcciones")
            Return Nothing
        End Try
    End Function

#End Region
#Region "Ordenes"
    Public Function TraerClientesOrdenes(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesOrdenes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerOrdenes_OyDNet(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesOrdenes")
            Return Nothing
        End Try
    End Function
#End Region
#Region "RentaFija"
    Public Function TraerClientesRentaFija(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesRentaFija)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerLiqRentaFija_OyDNet(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesRentaFija")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Facturas"
    Public Function TraerClientesFacturas(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.FacturasCli)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerFacturas_OyDNet(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesFacturas")
            Return Nothing
        End Try
    End Function

#End Region
#Region "Divisas Bancolombia"
    Public Function TraerClientesDivisas(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Divisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerDivisas_ValoresBC(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesDivisas")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Vencimientos"
    Public Function TraerClientesVencimientos(ByVal pComitente As String, ByVal pdesde As DateTime, ByVal phasta As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesVencimientos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerVencimientos_OyDNet(pComitente, pdesde, phasta).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesVencimientos")
            Return Nothing
        End Try
    End Function
#End Region
#Region "LiqxCumplir"
    Public Function TraerClientesLiqxCumplir(ByVal pComitente As String, ByVal Fcorte As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesLiqxCumplir)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerOperacionesPorCumplir_OyDNet(pComitente, Fcorte).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesLiqxCumplir")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Repos"
    Public Function TraerClientesRepos(ByVal pComitente As String, ByVal Fcorte As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesRepo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtener_Repos_Y_Simultaneas_Activas_OyDNet(pComitente, Fcorte).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesRepos")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Fondos"
    Public Function TraerSaldoClientesFondosDetallado(ByVal plngIDComitente As String, ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CompaniasClienteExtracto_Consultar_Detallado(plngIDComitente, pdtmFecha, False, pstrUsuario, DemeInfoSesion(pstrUsuario, "TraerSaldoClientesFondosDetallado"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerSaldoClientesFondosDetallado")
            Return Nothing
        End Try
    End Function
    Public Function TraerSaldoClientesFondosResumido(ByVal plngIDComitente As String, ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesFondosTotales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CompaniasClienteExtracto_Consultar_Resumido(plngIDComitente, pdtmFecha, True, pstrUsuario, DemeInfoSesion(pstrUsuario, "TraerSaldoClientesFondosResumido"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerSaldoClientesFondosResumido")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Totales"
    Public Function TraerClientesTotales(ByVal pComitente As String, ByVal Fecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesTotales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spTotalesCliente_OyDNet(pComitente, Fecha).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesTotales")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Saldos"
    'JFSB 20160926 - Se agrega el parametro pstrUsuario
    Public Function TraerClientesSaldos(ByVal pComitente As String, ByVal Fecha As DateTime, ByVal pstrsUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesSaldos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientesObtenerSaldos_OyDNet(pComitente, Fecha, pstrsUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesSaldos")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Encargos"
    Public Function TraerClientesEncargos(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesEncargos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarEncargosClientes_OyDNet(pComitente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesEncargos")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Clientes Accionistas"

    Public Function Traer_ClientesAccionistas(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAccionistas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesAccionistas_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesAccionistas"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesAccionistas")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesAccionistas(ByVal currentClientesAccionistas As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAccionistas)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesAccionistas.pstrUsuarioConexion, currentClientesAccionistas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesAccionistas.Estado) Then
                If currentClientesAccionistas.Estado.Equals("Retiro") Then
                    currentClientesAccionistas.InfoSesion = DemeInfoSesion(currentClientesAccionistas.pstrUsuarioConexion, "UpdateClientesAccionistas")
                    Me.DataContext.ClientesAccionistas.Attach(currentClientesAccionistas)
                    Me.DataContext.ClientesAccionistas.DeleteOnSubmit(currentClientesAccionistas)
                Else
                    'currentClientesBeneficiarios.Nombre = currentClientesBeneficiarios.Apellido1 + " " + currentClientesBeneficiarios.Apellido2 + " " + currentClientesBeneficiarios.Nombre1 + " " + currentClientesBeneficiarios.Nombre2
                    currentClientesAccionistas.InfoSesion = DemeInfoSesion(currentClientesAccionistas.pstrUsuarioConexion, "UpdateClientesAccionistas")
                    Me.DataContext.ClientesAccionistas.Attach(currentClientesAccionistas, Me.ChangeSet.GetOriginal(currentClientesAccionistas))
                End If
            Else
                'currentClientesBeneficiarios.Nombre = currentClientesBeneficiarios.Apellido1 + " " + currentClientesBeneficiarios.Apellido2 + " " + currentClientesBeneficiarios.Nombre1 + " " + currentClientesBeneficiarios.Nombre2
                currentClientesAccionistas.InfoSesion = DemeInfoSesion(currentClientesAccionistas.pstrUsuarioConexion, "UpdateClientesAccionistas")
                Me.DataContext.ClientesAccionistas.Attach(currentClientesAccionistas, Me.ChangeSet.GetOriginal(currentClientesAccionistas))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesAccionistas")
        End Try
    End Sub

    Public Sub InsertClientesAccionistas(ByVal ClientesAccionistas As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAccionistas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesAccionistas.pstrUsuarioConexion, ClientesAccionistas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesAccionistas.InfoSesion = DemeInfoSesion(ClientesAccionistas.pstrUsuarioConexion, "InsertClientesAccionistas")
            Me.DataContext.ClientesAccionistas.InsertOnSubmit(ClientesAccionistas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesAccionistas")
        End Try
    End Sub

    Public Sub DeleteClientesAccionistas(ByVal ClientesAccionistas As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesAccionistas)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesAccionistas.pstrUsuarioConexion, ClientesAccionistas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesAccionistas.InfoSesion = DemeInfoSesion(ClientesAccionistas.pstrUsuarioConexion, "DeleteClientesAccionistas")
            Me.DataContext.ClientesAccionistas.Attach(ClientesAccionistas)
            Me.DataContext.ClientesAccionistas.DeleteOnSubmit(ClientesAccionistas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesAccionistas")
        End Try
    End Sub

#End Region
#Region "ClientesFichaCliente"

    Public Function Traer_ClientesFichaCliente(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesFicha)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesFicha_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesFichaCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesFichaCliente")
        End Try
        Return Nothing
    End Function


    Public Sub UpdateClientesFichaCliente(ByVal currentClientesFichaCliente As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesFicha)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesFichaCliente.pstrUsuarioConexion, currentClientesFichaCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesFichaCliente.Estado) Then
                If currentClientesFichaCliente.Estado.Equals("Retiro") Then
                    currentClientesFichaCliente.InfoSesion = DemeInfoSesion(currentClientesFichaCliente.pstrUsuarioConexion, "UpdateClientesFichaCliente")
                    Me.DataContext.ClientesFicha.Attach(currentClientesFichaCliente)
                    Me.DataContext.ClientesFicha.DeleteOnSubmit(currentClientesFichaCliente)
                Else
                    'currentClientesBeneficiarios.Nombre = currentClientesBeneficiarios.Apellido1 + " " + currentClientesBeneficiarios.Apellido2 + " " + currentClientesBeneficiarios.Nombre1 + " " + currentClientesBeneficiarios.Nombre2
                    currentClientesFichaCliente.InfoSesion = DemeInfoSesion(currentClientesFichaCliente.pstrUsuarioConexion, "UpdateClientesFichaCliente")
                    Me.DataContext.ClientesFicha.Attach(currentClientesFichaCliente, Me.ChangeSet.GetOriginal(currentClientesFichaCliente))
                End If
            Else
                'currentClientesBeneficiarios.Nombre = currentClientesBeneficiarios.Apellido1 + " " + currentClientesBeneficiarios.Apellido2 + " " + currentClientesBeneficiarios.Nombre1 + " " + currentClientesBeneficiarios.Nombre2
                currentClientesFichaCliente.InfoSesion = DemeInfoSesion(currentClientesFichaCliente.pstrUsuarioConexion, "UpdateClientesFichaCliente")
                Me.DataContext.ClientesFicha.Attach(currentClientesFichaCliente, Me.ChangeSet.GetOriginal(currentClientesFichaCliente))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesFichaCliente")
        End Try
    End Sub

    Public Sub InsertClientesFichaCliente(ByVal ClientesFicha As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesFicha)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesFicha.pstrUsuarioConexion, ClientesFicha.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesFicha.InfoSesion = DemeInfoSesion(ClientesFicha.pstrUsuarioConexion, "InsertClientesFichaCliente")
            Me.DataContext.ClientesFicha.InsertOnSubmit(ClientesFicha)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesFichaCliente")
        End Try
    End Sub

    Public Sub DeleteClientesFichaCliente(ByVal ClientesFicha As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesFicha)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesFicha.pstrUsuarioConexion, ClientesFicha.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesFicha.InfoSesion = DemeInfoSesion(ClientesFicha.pstrUsuarioConexion, "DeleteClientesFichaCliente")
            Me.DataContext.ClientesFicha.Attach(ClientesFicha)
            Me.DataContext.ClientesFicha.DeleteOnSubmit(ClientesFicha)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesFichaCliente")
        End Try
    End Sub

#End Region
#Region "Clientes Personas"

    Public Function Traer_ClientesPersonas(ByVal Filtro As Byte, ByVal strComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPersonasParaConfirmar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(strComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesPersonas_Consultar(Filtro, strComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesPersonas"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesPersonas")
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
    ''' ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateClientesPersonas(ByVal currentClientesPersonas As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPersonasParaConfirmar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesPersonas.pstrUsuarioConexion, currentClientesPersonas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesPersonas.Estado) Then
                If currentClientesPersonas.Estado.Equals("Retiro") Then 'si el estado es retiro es por que se llama al procedimiento de eliminacion 
                    currentClientesPersonas.InfoSesion = DemeInfoSesion(currentClientesPersonas.pstrUsuarioConexion, "UpdateClientesPersonas")
                    Me.DataContext.ClientesPersonasParaConfirmar.Attach(currentClientesPersonas)
                    Me.DataContext.ClientesPersonasParaConfirmar.DeleteOnSubmit(currentClientesPersonas)
                Else
                    currentClientesPersonas.InfoSesion = DemeInfoSesion(currentClientesPersonas.pstrUsuarioConexion, "UpdateClientesPersonas")
                    Me.DataContext.ClientesPersonasParaConfirmar.Attach(currentClientesPersonas, Me.ChangeSet.GetOriginal(currentClientesPersonas))
                End If
            Else
                currentClientesPersonas.InfoSesion = DemeInfoSesion(currentClientesPersonas.pstrUsuarioConexion, "UpdateClientesPersonas")
                Me.DataContext.ClientesPersonasParaConfirmar.Attach(currentClientesPersonas, Me.ChangeSet.GetOriginal(currentClientesPersonas))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesPersonas")
        End Try
    End Sub

    Public Sub InsertClientesPersonas(ByVal ClientesPersonas As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPersonasParaConfirmar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesPersonas.pstrUsuarioConexion, ClientesPersonas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesPersonas.InfoSesion = DemeInfoSesion(ClientesPersonas.pstrUsuarioConexion, "InsertClientesPersonas")
            Me.DataContext.ClientesPersonasParaConfirmar.InsertOnSubmit(ClientesPersonas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesPersonas")
        End Try
    End Sub

    Public Sub DeleteClientesPersonas(ByVal ClientesPersonas As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPersonasParaConfirmar)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesPersonas.pstrUsuarioConexion, ClientesPersonas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesPersonas.InfoSesion = DemeInfoSesion(ClientesPersonas.pstrUsuarioConexion, "DeleteClientesPersonas")
            Me.DataContext.ClientesPersonasParaConfirmar.Attach(ClientesPersonas)
            Me.DataContext.ClientesPersonasParaConfirmar.DeleteOnSubmit(ClientesPersonas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesPersonas")
        End Try
    End Sub

#End Region
#Region "Clientes DepEconomica"

    Public Function Traer_ClientesDepEconomica(ByVal Filtro As Byte, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPersonasDepEconomica)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesPersonasDepEconomica_Consultar(Filtro, pComitente, DemeInfoSesion(pstrUsuario, "Traer_ClientesPersonas"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ClientesDepEconomica")
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
    ''' ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateClientesDepEconomica(ByVal currentClientesDepEconomica As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPersonasDepEconomica)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesDepEconomica.pstrUsuarioConexion, currentClientesDepEconomica.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentClientesDepEconomica.Estado) Then
                If currentClientesDepEconomica.Estado.Equals("Retiro") Then
                    currentClientesDepEconomica.InfoSesion = DemeInfoSesion(currentClientesDepEconomica.pstrUsuarioConexion, "UpdateClientesUpdateClientesDepEconomica")
                    Me.DataContext.ClientesPersonasDepEconomica.Attach(currentClientesDepEconomica)
                    Me.DataContext.ClientesPersonasDepEconomica.DeleteOnSubmit(currentClientesDepEconomica)
                Else
                    currentClientesDepEconomica.InfoSesion = DemeInfoSesion(currentClientesDepEconomica.pstrUsuarioConexion, "UpdateClientesDepEconomica")
                    Me.DataContext.ClientesPersonasDepEconomica.Attach(currentClientesDepEconomica, Me.ChangeSet.GetOriginal(currentClientesDepEconomica))
                End If
            Else
                currentClientesDepEconomica.InfoSesion = DemeInfoSesion(currentClientesDepEconomica.pstrUsuarioConexion, "UpdateClientesDepEconomica")
                Me.DataContext.ClientesPersonasDepEconomica.Attach(currentClientesDepEconomica, Me.ChangeSet.GetOriginal(currentClientesDepEconomica))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesDepEconomica")
        End Try
    End Sub

    Public Sub InsertClientesDepEconomica(ByVal ClientesDepEconomica As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPersonasDepEconomica)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesDepEconomica.pstrUsuarioConexion, ClientesDepEconomica.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesDepEconomica.InfoSesion = DemeInfoSesion(ClientesDepEconomica.pstrUsuarioConexion, "InsertClientesDepEconomica")
            Me.DataContext.ClientesPersonasDepEconomica.InsertOnSubmit(ClientesDepEconomica)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesDepEconomica")
        End Try
    End Sub

    Public Sub DeleteClientesDepEconomica(ByVal ClientesDepEconomica As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPersonasDepEconomica)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesDepEconomica.pstrUsuarioConexion, ClientesDepEconomica.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Clientes_Clientes_Eliminar( pIDComitente,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            ClientesDepEconomica.InfoSesion = DemeInfoSesion(ClientesDepEconomica.pstrUsuarioConexion, "DeleteClientesDepEconomica")
            Me.DataContext.ClientesPersonasDepEconomica.Attach(ClientesDepEconomica)
            Me.DataContext.ClientesPersonasDepEconomica.DeleteOnSubmit(ClientesDepEconomica)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesDepEconomica")
        End Try
    End Sub

#End Region
#Region "ClientesProductos"
    Public Sub InsertClientesProductos(ByVal ClientesProductos As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesProductos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesProductos.pstrUsuarioConexion, ClientesProductos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesProductos.strInfoSesion = DemeInfoSesion(ClientesProductos.pstrUsuarioConexion, "InsertClientesProductos")
            Me.DataContext.ClientesProductos.InsertOnSubmit(ClientesProductos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesProductos")
        End Try
    End Sub

    Public Sub UpdateClientesProductos(ByVal currentClientesProductos As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesProductos)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesProductos.pstrUsuarioConexion, currentClientesProductos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClientesProductos.strInfoSesion = DemeInfoSesion(currentClientesProductos.pstrUsuarioConexion, "UpdateClientesProductos")
            Me.DataContext.ClientesProductos.Attach(currentClientesProductos, Me.ChangeSet.GetOriginal(currentClientesProductos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesProductos")
        End Try
    End Sub

    Public Sub DeleteClientesProductos(ByVal ClientesProductos As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesProductos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesProductos.pstrUsuarioConexion, ClientesProductos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesProductos.strInfoSesion = DemeInfoSesion(ClientesProductos.pstrUsuarioConexion, "DeleteClientesProductos")
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesProductos")
        End Try
    End Sub

    Public Function ConsultarClientesProductos(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesProductos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(plngIDComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesProductos_Consultar(plngIDComitente, DemeInfoSesion(pstrUsuario, "ConsultarClientesProductos"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarClientesProductos")
        End Try
        Return Nothing
    End Function
#End Region
#Region "ClientesFATCA"
    Public Sub InsertClientesPaisesFATCA(ByVal currentClientesPaisesFATCA As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPaisesFATCA)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesPaisesFATCA.pstrUsuarioConexion, currentClientesPaisesFATCA.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClientesPaisesFATCA.InfoSesion = DemeInfoSesion(currentClientesPaisesFATCA.pstrUsuarioConexion, "InsertClientesPaisesFATCA")
            Me.DataContext.ClientesPaisesFATCA.InsertOnSubmit(currentClientesPaisesFATCA)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesPaisesFATCA")
        End Try
    End Sub

    Public Sub UpdateClientesPaisesFATCA(ByVal currentClientesPaisesFATCA As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPaisesFATCA)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesPaisesFATCA.pstrUsuarioConexion, currentClientesPaisesFATCA.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClientesPaisesFATCA.InfoSesion = DemeInfoSesion(currentClientesPaisesFATCA.pstrUsuarioConexion, "UpdateClientesProductos")
            Me.DataContext.ClientesPaisesFATCA.Attach(currentClientesPaisesFATCA, Me.ChangeSet.GetOriginal(currentClientesPaisesFATCA))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesProductos")
        End Try
    End Sub

    Public Sub DeleteClientesPaisesFATCA(ByVal DeleteClientesPaisesFATCA As A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPaisesFATCA)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DeleteClientesPaisesFATCA.pstrUsuarioConexion, DeleteClientesPaisesFATCA.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DeleteClientesPaisesFATCA.InfoSesion = DemeInfoSesion(DeleteClientesPaisesFATCA.pstrUsuarioConexion, "DeleteClientesPaisesFATCA")
            Me.DataContext.ClientesPaisesFATCA.Attach(DeleteClientesPaisesFATCA)
            Me.DataContext.ClientesPaisesFATCA.DeleteOnSubmit(DeleteClientesPaisesFATCA)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesPaisesFATCA")
        End Try
    End Sub

    Public Function ConsultarClientesPaisesFATCA(ByVal Filtro As Byte, ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ClientesPaisesFATCA)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(plngIDComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Clientes_ClientesPaisesFATCA_Consultar(Filtro, plngIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarClientesPaisesFATCA"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarClientesPaisesFATCA")
        End Try
        Return Nothing
    End Function
#End Region


#End Region
#End Region

#Region "PreClientes"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertPreClientes(ByVal newPreClientes As PreClientes)

    End Sub

    Public Sub UpdatePreClientes(ByVal currentPreClientes As PreClientes)

    End Sub

    Public Sub DeletePreClientes(ByVal deletePreClientes As PreClientes)

    End Sub

#End Region

#Region "Métodos"
    Public Function ConsultaPreclientes(ByVal pstrEstado As String, ByVal pstrNroDocumento As String, ByVal pstrTipoIdentificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.PreClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_PreClientes_Consultar_OyDNet(pstrEstado, pstrNroDocumento, pstrTipoIdentificacion).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ConsultaPreclientes")
            Return Nothing
        End Try
    End Function

    Public Function ModificarPreclientes(ByVal Idprecliente As Integer, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim pstrMsgValidacion As String = ""
            Me.DataContext.usp_PreClientes_ModificarEstado_OyDNet(Idprecliente, pstrEstado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ModificarPreclientes"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ModificarPreclientes")
            Return Nothing
        End Try
    End Function

    Private Function ModificarPreclientesAsync(ByVal Idprecliente As Integer, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ModificarPreclientes(Idprecliente, pstrEstado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ModificarPreclientesSync(ByVal Idprecliente As Integer, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ModificarPreclientesAsync(Idprecliente, pstrEstado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function


    Public Function PasarPreclientes(ByVal IDprecliente As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.PreClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_PreClientes_PasarCliente_OyDNet(IDprecliente).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "PasarPreclientes")
            Return Nothing
        End Try
    End Function

    Public Function PreClientes_Phoenix_Consultar(ByVal pstrClave As String, ByVal pstrNroDocumento As String, ByVal pstrTipoIdentificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim pstrMsgValidacion As String = ""
            Me.DataContext.usp_PreClientes_Phoenix_Consultar(pstrClave, pstrNroDocumento, pstrTipoIdentificacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "PreClientes_Phoenix_Consultar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PreClientes_Phoenix_Consultar")
            Return Nothing
        End Try
    End Function

    Private Function PreClientes_Phoenix_ConsultarAsync(ByVal pstrClave As String, ByVal pstrNroDocumento As String, ByVal pstrTipoIdentificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(PreClientes_Phoenix_Consultar(pstrClave, pstrNroDocumento, pstrTipoIdentificacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function PreClientes_Phoenix_ConsultarSync(ByVal pstrClave As String, ByVal pstrNroDocumento As String, ByVal pstrTipoIdentificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.PreClientes_Phoenix_ConsultarAsync(pstrClave, pstrNroDocumento, pstrTipoIdentificacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Public Function PreClientes_Phoenix_Actualizar(pstrNroDocumento As String, pstrTipoIdentificacion As String, ByVal pxmlDetalleClientes As String, ByVal pxmlDetalleDirecciones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim pstrMsgValidacion As String = ""
            Me.DataContext.usp_PreClientes_Phoenix_Actualizar(pstrNroDocumento, pstrTipoIdentificacion, pxmlDetalleClientes, pxmlDetalleDirecciones, pstrUsuario, DemeInfoSesion(pstrUsuario, "PreClientes_Phoenix_Actualizar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PreClientes_Phoenix_Actualizar")
            Return Nothing
        End Try
    End Function

    Private Function PreClientes_Phoenix_ActualizarAsync(pstrNroDocumento As String, pstrTipoIdentificacion As String, ByVal pxmlDetalleClientes As String, ByVal pxmlDetalleDirecciones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(PreClientes_Phoenix_Actualizar(pstrNroDocumento, pstrTipoIdentificacion, pxmlDetalleClientes, pxmlDetalleDirecciones, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function PreClientes_Phoenix_ActualizarSync(pstrNroDocumento As String, pstrTipoIdentificacion As String, ByVal pxmlDetalleClientes As String, ByVal pxmlDetalleDirecciones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.PreClientes_Phoenix_ActualizarAsync(pstrNroDocumento, pstrTipoIdentificacion, pxmlDetalleClientes, pxmlDetalleDirecciones, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function


#End Region

#End Region

#Region "Procesos Sincrónicos"

    'Santiago Vergara - Octubre 22/2013 - función para el llamado sincrónico de ValidarClienteTipoProducto
    Public Function ValidarClienteTipoProductoSync(ByVal pstrNroDocumento As String, ByVal pstrTipoProducto As String, ByVal plngid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Dim objTask As Task(Of Integer) = Me.ValidarClienteTipoProductoAsync(pstrNroDocumento, pstrTipoProducto, plngid, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    'Santiago Vergara - Octubre 22/2013 - función para el llamado sincrónico de ValidarClienteTipoProducto
    Private Function ValidarClienteTipoProductoAsync(ByVal pstrNroDocumento As String, ByVal pstrTipoProducto As String, ByVal plngid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Integer)
        Dim objTaskComplete As TaskCompletionSource(Of Integer) = New TaskCompletionSource(Of Integer)()
        objTaskComplete.TrySetResult(ValidarClienteTipoProducto(pstrNroDocumento, pstrTipoProducto, plngid, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

    ''' <summary>
    ''' Adicionada por Rafael Cordero
    ''' Julio-28-2 ClsConstantes.GINT_ErrorPersonalizado11 yy
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
#Region "Inactivación de Clientes"

    Public Function ListaClientesInactivar(ByVal pintIdGrupo As Integer, ByVal pintIdSucursal As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ListaClientesInactivar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.uspOyDNet_Clientes_ConsultarClientesInactivar(pintIdGrupo, pintIdSucursal, pstrUsuario, DemeInfoSesion(pstrUsuario, "ListaClientesInactivar"), ClsConstantes.GINT_ErrorPersonalizado).ToList()
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ListaClientesInactivar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function InactivarCliente(ByVal plngIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDClientes.RespuestaInactivacionClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Clientes_InactivarCliente(plngIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "InactivarCliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ListaClientesInactivar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "COMUNES"

    Public Function CargarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.spA2utils_CargarCombos("")
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarCombos")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Modificacion Identificacion Cliente"
    Public Function ConsultarClientesIdentificacin(ByVal strNroDocumento As String, ByVal strTipoIdentificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim mensaje As String = ""
            Dim ret = Me.DataContext.usp_ClientesIdentificacion_Consultar_OyDNet(strNroDocumento, strTipoIdentificacion, mensaje)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "IdentificacionCliente")
            Return Nothing
        End Try
    End Function

    Public Function ModificarClientesIdentificacion(ByVal pstrTipoIdentificacionActual As String, ByVal pstrNroDocumentoActual As String,
                                                    ByVal pstrTipoIdentificacionNuevo As String, ByVal pstrNroDocumentoNuevo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ClientesIdentificacion_Modificar_OyDNet(pstrTipoIdentificacionActual, pstrNroDocumentoActual, pstrTipoIdentificacionNuevo, pstrNroDocumentoNuevo, pstrUsuario)
            Return CStr(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ModificacionClienteIdentificacion")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarClienteCodigoPorIdentificacion(ByVal pstrNroDocumento As String, ByVal pstrTipoDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyDNet_ClienteCodigoPorIdentificacion_Consultar(pstrNroDocumento, pstrTipoDocumento).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarClienteCodigoPorIdentificacion")
            Return Nothing
        End Try
    End Function
#End Region

End Class


