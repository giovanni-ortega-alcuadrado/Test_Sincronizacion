Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSTesoreria
Imports System.Web
Imports System.Data.Linq
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.Threading.Tasks
Imports System.Transactions
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()>
Partial Public Class OYDPLUSTesoreriaDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSTesoreriaDatacontext)
    Dim RETORNO As Boolean
    Dim Usuario As String = ""

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

#Region "Tesoreria OYDPLUS"

    '-----TESORERIA ORDENES ENCABEZADO----'
    Public Function TesoreriaOrdenesListar(ByVal pstrTipo As String, ByVal pstrEstado As String,
                                           ByVal pstrNroDocumento As String, ByVal pstrFormaPago As String,
                                           ByVal plngIdBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOrdenesEncabezado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenes_Listar(pstrTipo, pstrEstado, pstrNroDocumento, pstrFormaPago, plngIdBanco, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarTesoreriaOydPlus"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesListar")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesConsultar(ByVal plngId As Integer, ByVal pstrNroDocumento As String,
                                              ByVal pstrEstado As String, ByVal pstrIDComitente As String,
                                              ByVal pstrCodigoReceptor As String, ByVal pstrTipoProducto As String,
                                              ByVal pstrFiltro As String,
                                              ByVal pstrUsuario As String, pstrOpcion As String, pstrTipo As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOrdenesEncabezado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenes_Consultar(plngId, pstrNroDocumento, pstrEstado, pstrIDComitente, pstrCodigoReceptor, pstrTipoProducto, pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarTesoreriaOydPlus"), 0, pstrOpcion, pstrTipo)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesConsultar")
            Return Nothing
        End Try
    End Function

    Public Function OyDPlusListarCuentasClientes(pstrIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TempCuentasClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarComboCuentasRegistradas(pstrIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "OyDPlusListarCuentasClientes"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OyDPlusListarCuentasClientes")
            Return Nothing
        End Try
    End Function
    Public Function OyDPlusListarDireccionesClientes(pstrIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TempDireccionesClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarComboDireccionesRegistradas(pstrIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "OyDPlusListarDireccionesClientes"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OyDPlusListarDireccionesClientes")
            Return Nothing
        End Try
    End Function
    Public Sub InsertTesoreriaOrdenesEncabezado(ByVal objTesoreriaOrdenesEncabezado As TesoreriaOrdenesEncabezado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesEncabezado.pstrUsuarioConexion, objTesoreriaOrdenesEncabezado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesEncabezado.InfoSesion = DemeInfoSesion(objTesoreriaOrdenesEncabezado.pstrUsuarioConexion, "InsertTesoreriaOrdenesEncabezado")
            Me.DataContext.tblTesoreriaOrdenesEncabezados.InsertOnSubmit(objTesoreriaOrdenesEncabezado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOrdenesEncabezado")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOrdenesEncabezado(ByVal objTesoreriaOrdenesEncabezado As TesoreriaOrdenesEncabezado)
        Try
            'objTesoreriaOrdenesEncabezado.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateTesoreriaOrdenesEncabezado")
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.Attach(objTesoreriaOrdenesEncabezado)
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.DeleteOnSubmit(objTesoreriaOrdenesEncabezado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOrdenesEncabezado")
        End Try
    End Sub

    Public Sub UpdateTesorero(ByVal obj As Tesorero)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreros.Attach(obj, Me.ChangeSet.GetOriginal(obj))
            Me.DataContext.tblTesoreros.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesorero")
        End Try

    End Sub

    Public Sub UpdateTesoreroDetalleRecibo(ByVal obj As TesoreroDetalleRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreroDetalleRecibos.Attach(obj, Me.ChangeSet.GetOriginal(obj))
            Me.DataContext.tblTesoreroDetalleRecibos.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreroDetalleRecibo")
        End Try

    End Sub
    Public Function TesoreriaOrdenesEncabezado_Elminar(ByVal plngIdTesoreriaOrdenesEncabezado As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesEncabezado_Eliminar(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "EliminarDetalle"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesEncabezado_Elminar")
            Return Nothing
        End Try
    End Function

    Public Function VerificarAnulacionOrdenFondosOyD(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strMensajeInformativo As String = String.Empty
            Me.DataContext.uspOyDNet_VerificarAnulacionFondosOyD(plngIdTesoreriaOrdenesEncabezado, strMensajeInformativo, pstrUsuario, DemeInfoSesion(pstrUsuario, "VerificarAnulacionOrdenFondosOyD"), 0)
            Return strMensajeInformativo
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarAnulacionOrdenFondosOyD")
            Return Nothing
        End Try
    End Function

    Public Function VerificarRechazoOrdenFondosOyD(ByVal pstrIdsDocumentos As String,
                                                   ByVal pstrTipoNegocio As String,
                                                   ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strMensajeInformativo As String = String.Empty
            Me.DataContext.uspOyDNet_VerificarRechazosFondosOyD(pstrIdsDocumentos, pstrTipoNegocio, strMensajeInformativo, pstrUsuario, DemeInfoSesion(pstrUsuario, "VerificarRechazoOrdenFondosOyD"), 0)
            Return strMensajeInformativo
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarRechazoOrdenFondosOyD")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarComprobanteEgresoPlus(pstrRegistros As String, pstrUsuario As String, ByVal pstrNombreArchivo As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_ComprobantesEgresoPlus(pstrRegistros, pstrUsuario, DemeInfoSesion(pstrUsuario, "GenerarComprobanteEgresoPlus"), 0, pstrNombreArchivo).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarComprobanteEgresoPlus")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function GenerarNOTAPlus(pstrRegistros As String, pstrUsuario As String, pstrRutaServidorReporte As String, pstrRutaReporteCartaODocumento As String, ByVal pstrNombreArchivo As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Generar_NOTA(pstrRegistros, pstrUsuario, pstrRutaServidorReporte, pstrRutaReporteCartaODocumento, DemeInfoSesion(pstrUsuario, "GenerarNOTAPlus"), 0, pstrNombreArchivo).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarNOTAPlus")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function GenerarBloqueoPlus(pstrRegistros As String, pstrUsuario As String, ByVal pstrNombreArchivo As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarNOTABLOQUEO(pstrRegistros, pstrUsuario, pstrNombreArchivo).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarBloqueoPlus")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function ValidarListaRestrictiva(pstrRegistros As String, pstrUsuario As String, plogOpcion As Boolean, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarInhabilitados(pstrRegistros, pstrUsuario, plogOpcion).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarListaRestrictiva")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function GenerarRecibosPlus(pstrRegistros As String, pstrUsuario As String, pstrRutaServidorReporte As String, pstrRutaReporteCartaODocumento As String, ByVal pstrNombreArchivo As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Generar_AdicionConstitucion(pstrRegistros, pstrUsuario, pstrRutaServidorReporte, pstrRutaReporteCartaODocumento, DemeInfoSesion(pstrUsuario, "GenerarRecibosPlus"), 0, pstrNombreArchivo).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarRecibosPlus")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCarterasColectivasCliente(ByVal pstrCodigoOYD As String, ByVal plogConsultarTodasCarteras As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByVal pstrFiltroAdicionaConsulta As String) As List(Of CarterasColectivasClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CarterasColectivasCliente(pstrCodigoOYD, plogConsultarTodasCarteras, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCarterasColectivasCliente"), 0, pstrFiltroAdicionaConsulta).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCarterasColectivasCliente")
            Return Nothing
        End Try
    End Function

    Public Function ValidarExistenciaChequesOrdenesRecibo(ByVal pstrIDOrdenes As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim logRetorno As Nullable(Of Boolean) = False
            Me.DataContext.uspOyDNet_ValidarExistenciaChequeOrdenesRecibo(pstrIDOrdenes, pstrUsuario, logRetorno, DemeInfoSesion(pstrUsuario, "ValidarExistenciaChequesOrdenesRecibo"), 0)
            Return CBool(logRetorno)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarExistenciaChequesOrdenesRecibo")
            Return Nothing
        End Try
    End Function

    Public Function VerificarRestriccionesTipoCartera(ByVal pstrTipoAccionFondos As String,
                                                      ByVal plngIDComitente As String,
                                                      ByVal pstrCarteraColectiva As String,
                                                      ByVal pintEncargo As String,
                                                      ByVal pstrUsuario As String,
                                                      ByVal pdtmFechaValidacion As Nullable(Of DateTime), ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strRetorno As String = String.Empty
            Me.DataContext.uspOyDNet_ValidarTipoCartera(pstrTipoAccionFondos, plngIDComitente, pstrCarteraColectiva, pintEncargo, pstrUsuario, DemeInfoSesion(pstrUsuario, "VerificarRestriccionesTipoCartera"), 0, strRetorno, pdtmFechaValidacion)
            Return strRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarRestriccionesTipoCartera")
            Return Nothing
        End Try
    End Function

    Public Function ObtenerFechaAplicacionValida(ByVal pdtmFechaAplicacion As DateTime, ByVal pstrCarteraColectiva As String, ByVal pstrTipoRetiroFondos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DateTime
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim dtmFechaAplicacionValida As DateTime?
            Me.DataContext.uspOyDNet_FechaAplicacionCancelacion_Consultar(pdtmFechaAplicacion, dtmFechaAplicacionValida, pstrCarteraColectiva, pstrTipoRetiroFondos, pstrUsuario, DemeInfoSesion(pstrUsuario, "ObtenerFechaAplicacionValida"), 0)
            Return CDate(dtmFechaAplicacionValida)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerFechaAplicacionValida")
            Return Nothing
        End Try
    End Function

    Public Function ObtenerFechaAplicacionValidaSync(ByVal pdtmFechaAplicacion As DateTime, ByVal pstrCarteraColectiva As String, ByVal pstrTipoRetiroFondos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DateTime
        Dim objTask As Task(Of DateTime) = Me.ObtenerFechaAplicacionValidaAsync(pdtmFechaAplicacion, pstrCarteraColectiva, pstrTipoRetiroFondos, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ObtenerFechaAplicacionValidaAsync(ByVal pdtmFechaAplicacion As DateTime, ByVal pstrCarteraColectiva As String, ByVal pstrTipoRetiroFondos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of DateTime)
        Dim objTaskComplete As TaskCompletionSource(Of DateTime) = New TaskCompletionSource(Of DateTime)()
        objTaskComplete.TrySetResult(ObtenerFechaAplicacionValida(pdtmFechaAplicacion, pstrCarteraColectiva, pstrTipoRetiroFondos, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function OrdenPago_GuardarDetalle_Tesorero(ByVal pintIDEncabezado As Integer,
                                                      ByVal pintIDDetalle As Integer,
                                                      ByVal pstrFormaPago As String,
                                                      ByVal pstrTipoCheque As String,
                                                      ByVal pstrTipoCruce As String,
                                                      ByVal pstrDetalleConcepto As String,
                                                      ByVal pstrTipoGMF As String,
                                                      ByVal pdblValorBruto As Double,
                                                      ByVal pdblValorGMF As Double,
                                                      ByVal pdblValorNeto As Double,
                                                      ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrInfoConexion As String, ByVal pintIdConcepto As Nullable(Of Integer)) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("OrdenPago_GuardarDetalle_Tesorero", pstrUsuario)
            Dim objRetorno = Me.DataContext.uspOyDNet_OrdenPago_GuardarDetalle_Tesorero(pintIDEncabezado, pintIDDetalle, pstrFormaPago, pstrTipoCheque, pstrTipoCruce, pstrDetalleConcepto, pstrTipoGMF, pdblValorBruto, pdblValorGMF, pdblValorNeto, pstrUsuario, pstrUsuarioWindows, strInfoSession, 0, pintIdConcepto).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenPago_GuardarDetalle_Tesorero")
            Return Nothing
        End Try
    End Function

    Public Function Traer_TesoreriaOrdenesDetalle() As List(Of TesoreriaOrdenesDetalle)
        Return New List(Of TesoreriaOrdenesDetalle)
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function OrdenPago_ConsultarDetalle(ByVal pintIDEncabezado As Integer, ByVal pintIDDetalle As Integer, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOrdenesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("OrdenPago_ConsultarDetalle", pstrUsuario)
            Dim objRetorno = Me.DataContext.uspOyDNet_OrdenPago_ConsultarDetalle(pintIDEncabezado, pintIDDetalle, pstrUsuario, pstrUsuarioWindows, strInfoSession, 0).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenPago_ConsultarDetalle")
            Return Nothing
        End Try
    End Function

#Region "TESORERIA ORDENES DETALLE"
    Public Function TesoreriaOrdenesDetalle_ValidarEstado(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                          ByVal plngIdTesoreriaOrdenesDetalle As Integer, pstrFormaPago As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Eliminar(plngIdTesoreriaOrdenesEncabezado, plngIdTesoreriaOrdenesDetalle, pstrFormaPago, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalle_ValidarEstado"), 0, True)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalle_ValidarEstado")
            Return Nothing
        End Try
    End Function
#Region "GIRO"
#Region "Encabezado"

    <Query(HasSideEffects:=True)>
    Public Function OYDPLUS_ValidarIngresoOrdenTesoreria(pintIDOrdenTesoreriaEncabezado As Integer,
                                                         pdtmOrdenTesoreria As DateTime,
                                                         pstrIDComitente As String,
                                                         pstrTipoProducto As String,
                                                         pdblValorOrden As Decimal,
                                                         pstrReceptor As String,
                                                         pstrTipoIdentificacion As String,
                                                         pintNroDocumento As Integer,
                                                         pstrNroDocumento As String,
                                                         pstrNombre As String,
                                                         pstrTipo As String,
                                                         pstrConfirmaciones As String,
                                                         pstrConfirmacionesUsuario As String,
                                                         pstrJustificaciones As String,
                                                         pstrJustificacionUsuario As String,
                                                         pstrAprobaciones As String,
                                                         pstrAprobacionesUsuario As String,
                                                         pstrUsuario As String,
                                                         pstrOpcion As String,
                                                         pstrEstado As String,
                                                         plogClienteRecoge As Boolean,
                                                        plogClientePresente As Boolean,
                                                        plogllevarDireccion As Boolean,
                                                        plogRecogeTercero As Boolean,
                                                        plogConsignarCta As Boolean,
                                                        pstrTipoIdentificacion_Instrucciones As String,
                                                        pstrNroDocumento_Instrucciones As String,
                                                        pstrNombre_Instrucciones As String,
                                                        plogDireccionInscrita_Instrucciones As Boolean,
                                                        pstrCuenta_Instrucciones As String,
                                                        pstrDireccion_Instrucciones As String,
                                                        pstrCiudad_Instrucciones As String,
                                                        pstrSector_Instrucciones As String,
                                                        plogEsTercero_Instrucciones As Boolean,
                                                        plogEsCtaInscrita_Instrucciones As Boolean,
                                                        pstrTipoCta_Instrucciones As String,
                                                        plngIDBanco As Nullable(Of Integer),
                                                        pstrRegistroDetalle As String,
                                                        plogOpcion As Boolean,
                                                        pdblValorNotaBloqueo As Decimal,
                                                        plogEsFutura As Boolean,
                                                        pstrEspecieDividendos As String,
                                                        pstrCarteraColectivaFondos As String,
                                                        pintNroEncargo As String,
                                                        plogOtraInstrucciones As Boolean,
                                                        pstrOtraInstrucciones As String,
                                                        pstrIDOrdenante As String,
                                                        pstrDetalleCheques As String,
                                                        pstrDetalleTransferencias As String,
                                                        pstrDetalleCarteras As String,
                                                        pstrDetalleInternos As String,
                                                        pstrDetalleBloqueos As String,
                                                        pstrDetalleOYD As String,
                                                        pstrDetalleOperacionesEspeciales As String,
                                                        pstrTipoRetiroFondos As String,
                                                        pstrUsuarioWindows As String,
                                                        plogXTesorero As Boolean,
                                                        pdblValorAntesModificarlo As Nullable(Of Double),
                                                        pdblValorPenalizacion As Nullable(Of Double),
                                                        plogEsFondoOYD As Boolean,
                                                        pdtmFechaAplicacion As Nullable(Of DateTime),
                                                        plogEsOrdenFutura As Nullable(Of Boolean),
                                                        pstrObservaciones As String,
                                                        pdblValorBrutoOrden As Decimal,
                                                        pdblValorGMF As Decimal,
                                                        plogTesoreroConfirmoGrabacion As Boolean, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim p1 As Nullable(Of Integer)

            Dim ret = Me.DataContext.uspOyDNet_ValidarOrdenTesoreria(
                                                         pintIDOrdenTesoreriaEncabezado,
                                                         pdtmOrdenTesoreria,
                                                         pstrIDComitente,
                                                         pstrTipoProducto,
                                                         pdblValorOrden,
                                                         pstrReceptor,
                                                         pstrTipoIdentificacion,
                                                         pintNroDocumento,
                                                         pstrNroDocumento,
                                                         pstrNombre,
                                                         pstrTipo,
                                                         pstrConfirmaciones,
                                                         pstrConfirmacionesUsuario,
                                                         pstrJustificaciones,
                                                         pstrJustificacionUsuario,
                                                         pstrAprobaciones,
                                                         pstrAprobacionesUsuario,
                                                         pstrUsuario,
                                                         DemeInfoSesion(pstrUsuario, "OYDPLUS_ValidarIngresoOrden"), 0, pstrOpcion, pstrEstado,
                                                         plogClienteRecoge,
                                                        plogClientePresente,
                                                        plogllevarDireccion,
                                                        plogRecogeTercero,
                                                        plogConsignarCta,
                                                        pstrTipoIdentificacion_Instrucciones,
                                                        pstrNroDocumento_Instrucciones,
                                                        pstrNombre_Instrucciones,
                                                        plogDireccionInscrita_Instrucciones,
                                                        pstrCuenta_Instrucciones,
                                                        pstrDireccion_Instrucciones,
                                                        pstrCiudad_Instrucciones,
                                                        pstrSector_Instrucciones,
                                                        plogEsTercero_Instrucciones,
                                                        plogEsCtaInscrita_Instrucciones,
                                                        pstrTipoCta_Instrucciones,
                                                        plngIDBanco,
                                                        p1,
                                                        pstrRegistroDetalle,
                                                        plogOpcion,
                                                        pdblValorNotaBloqueo,
                                                        plogEsFutura,
                                                        pstrEspecieDividendos,
                                                        pstrCarteraColectivaFondos,
                                                        pintNroEncargo,
                                                        plogOtraInstrucciones,
                                                        pstrOtraInstrucciones,
                                                        pstrIDOrdenante,
                                                        pstrDetalleCheques,
                                                        pstrDetalleTransferencias,
                                                        pstrDetalleCarteras,
                                                        pstrDetalleInternos,
                                                        pstrDetalleBloqueos,
                                                        pstrDetalleOYD,
                                                        pstrDetalleOperacionesEspeciales,
                                                        pstrTipoRetiroFondos,
                                                        pstrUsuarioWindows,
                                                        plogXTesorero,
                                                        pdblValorAntesModificarlo,
                                                        pdblValorPenalizacion,
                                                        plogEsFondoOYD,
                                                        pdtmFechaAplicacion,
                                                        plogEsOrdenFutura,
                                                        pstrObservaciones,
                                                        pdblValorBrutoOrden,
                                                        pdblValorGMF,
                                                        plogTesoreroConfirmoGrabacion)

            intIdTesoreriaOrdenesEncabezado = p1

            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarIngresoOrdenTesoreria")
            Return Nothing
        End Try
    End Function

#End Region
#Region "CHEQUES OYD PLUS"
    Public Sub InsertTesoreriaOyDPlusCheques(ByVal objTesoreriaOrdenesDetalleCheques As TesoreriaOyDPlusCheques)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleCheques.pstrUsuarioConexion, objTesoreriaOrdenesDetalleCheques.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesDetalleCheques.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleCheques.pstrUsuarioConexion, "InsertTesoreriaOyDPlusCheques")
            Me.DataContext.tblTesoreriaOrdenesCheques.InsertOnSubmit(objTesoreriaOrdenesDetalleCheques)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusCheques")
        End Try
    End Sub

    Public Sub UpdateTesoreriaOyDPlusCheques(ByVal objTesoreriaOrdenesDetalleCheques As TesoreriaOyDPlusCheques)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleCheques.pstrUsuarioConexion, objTesoreriaOrdenesDetalleCheques.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'objTesoreriaOrdenesEncabezado.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateTesoreriaOrdenesEncabezado")
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.Attach(objTesoreriaOrdenesEncabezado)
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.DeleteOnSubmit(objTesoreriaOrdenesEncabezado)
            objTesoreriaOrdenesDetalleCheques.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleCheques.pstrUsuarioConexion, "UpdateTesoreriaOyDPlusCheques")
            Me.DataContext.tblTesoreriaOrdenesCheques.Attach(objTesoreriaOrdenesDetalleCheques, Me.ChangeSet.GetOriginal(objTesoreriaOrdenesDetalleCheques))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusCheques")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusCheques(ByVal obj As TesoreriaOyDPlusCheques)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusCheques")
            Me.DataContext.tblTesoreriaOrdenesCheques.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesCheques.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusCheques")
        End Try
    End Sub
#End Region
#Region "TRANSFERENCIA OYD PLUS"
    Public Sub UpdateTesoreriaOyDPlusTransferencia(ByVal objTesoreriaOrdenesDetalleTransferencia As TesoreriaOyDPlusTransferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleTransferencia.pstrUsuarioConexion, objTesoreriaOrdenesDetalleTransferencia.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'objTesoreriaOrdenesEncabezado.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateTesoreriaOrdenesEncabezado")
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.Attach(objTesoreriaOrdenesEncabezado)
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.DeleteOnSubmit(objTesoreriaOrdenesEncabezado)
            objTesoreriaOrdenesDetalleTransferencia.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleTransferencia.pstrUsuarioConexion, "UpdateTesoreriaOyDPlusTransferencia")
            Me.DataContext.tblTesoreriaOrdenesTransferencia.Attach(objTesoreriaOrdenesDetalleTransferencia, Me.ChangeSet.GetOriginal(objTesoreriaOrdenesDetalleTransferencia))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusTransferencia")
        End Try
    End Sub
    Public Sub InsertTesoreriaOyDPlusTransferencia(ByVal objTesoreriaOrdenesDetalleTransferencia As TesoreriaOyDPlusTransferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleTransferencia.pstrUsuarioConexion, objTesoreriaOrdenesDetalleTransferencia.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesDetalleTransferencia.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleTransferencia.pstrUsuarioConexion, "InsertTesoreriaOyDPlusTransferencia")
            Me.DataContext.tblTesoreriaOrdenesTransferencia.InsertOnSubmit(objTesoreriaOrdenesDetalleTransferencia)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusTransferencia")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusTransferencia(ByVal obj As TesoreriaOyDPlusTransferencia)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusTransferencia")
            Me.DataContext.tblTesoreriaOrdenesTransferencia.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesTransferencia.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusTransferencia")
        End Try
    End Sub
#End Region
#Region "CARTERAS COLECTIVAS OYD PLUS"
    Public Sub InsertTesoreriaOyDPlusCarterasColectivas(ByVal objTesoreriaOrdenesDetalleCarterasColectivas As TesoreriaOyDPlusCarterasColectivas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleCarterasColectivas.pstrUsuarioConexion, objTesoreriaOrdenesDetalleCarterasColectivas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesDetalleCarterasColectivas.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleCarterasColectivas.pstrUsuarioConexion, "InsertTesoreriaOyDPlusCarterasColectivas")
            Me.DataContext.tblTesoreriaOrdenesCarterasColectivas.InsertOnSubmit(objTesoreriaOrdenesDetalleCarterasColectivas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusCarterasColectivas")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusCarterasColectivas(ByVal objTesoreriaOrdenesDetalleCarteraColectivas As TesoreriaOyDPlusCarterasColectivas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleCarteraColectivas.pstrUsuarioConexion, objTesoreriaOrdenesDetalleCarteraColectivas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'objTesoreriaOrdenesEncabezado.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateTesoreriaOrdenesEncabezado")
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.Attach(objTesoreriaOrdenesEncabezado)
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.DeleteOnSubmit(objTesoreriaOrdenesEncabezado)

            objTesoreriaOrdenesDetalleCarteraColectivas.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleCarteraColectivas.pstrUsuarioConexion, "UpdateTesoreriaOyDPlusCarterasColectivas")
            Me.DataContext.tblTesoreriaOrdenesCarterasColectivas.Attach(objTesoreriaOrdenesDetalleCarteraColectivas, Me.ChangeSet.GetOriginal(objTesoreriaOrdenesDetalleCarteraColectivas))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusCarterasColectivas")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusCarterasColectivas(ByVal obj As TesoreriaOyDPlusCarterasColectivas)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusCarterasColectivas")
            Me.DataContext.tblTesoreriaOrdenesCarterasColectivas.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesCarterasColectivas.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusCarterasColectivas")
        End Try
    End Sub
#End Region
#Region "TRASLADO FONDOS OYD PLUS"
    Public Sub InsertTesoreriaOyDPlusTrasladoFondos(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusTrasladoFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "InsertTesoreriaOyDPlusTrasladoFondos")
            Me.DataContext.tblTesoreriaOrdenesTrasladoFondos.InsertOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusTrasladoFondos")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusTrasladoFondoss(ByVal objTesoreriaOrdenesDetalleTrasladoFondos As TesoreriaOyDPlusTrasladoFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleTrasladoFondos.pstrUsuarioConexion, objTesoreriaOrdenesDetalleTrasladoFondos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesDetalleTrasladoFondos.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleTrasladoFondos.pstrUsuarioConexion, "UpdateTesoreriaOyDPlusTrasladoFondoss")
            Me.DataContext.tblTesoreriaOrdenesTrasladoFondos.Attach(objTesoreriaOrdenesDetalleTrasladoFondos, Me.ChangeSet.GetOriginal(objTesoreriaOrdenesDetalleTrasladoFondos))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusTrasladoFondoss")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusTrasladoFondos(ByVal obj As TesoreriaOyDPlusTrasladoFondos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusTrasladoFondos")
            Me.DataContext.tblTesoreriaOrdenesTrasladoFondos.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesTrasladoFondos.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusCarterasColectivas")
        End Try
    End Sub
#End Region
#Region "INTERNOS"
    Public Sub InsertTesoreriaOyDPlusInternos(ByVal objTesoreriaOrdenesDetalleInternos As TesoreriaOyDPlusInternos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleInternos.pstrUsuarioConexion, objTesoreriaOrdenesDetalleInternos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesDetalleInternos.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleInternos.pstrUsuarioConexion, "InsertTesoreriaOyDPlusInternos")
            Me.DataContext.tblTesoreriaOrdenesInternos.InsertOnSubmit(objTesoreriaOrdenesDetalleInternos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusInternos")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusInternos(ByVal objTesoreriaOrdenesDetalleInternos As TesoreriaOyDPlusInternos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleInternos.pstrUsuarioConexion, objTesoreriaOrdenesDetalleInternos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'objTesoreriaOrdenesEncabezado.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateTesoreriaOrdenesEncabezado")
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.Attach(objTesoreriaOrdenesEncabezado)
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.DeleteOnSubmit(objTesoreriaOrdenesEncabezado)
            objTesoreriaOrdenesDetalleInternos.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleInternos.pstrUsuarioConexion, "UpdateTesoreriaOyDPlusInternos")
            Me.DataContext.tblTesoreriaOrdenesInternos.Attach(objTesoreriaOrdenesDetalleInternos, Me.ChangeSet.GetOriginal(objTesoreriaOrdenesDetalleInternos))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusInternos")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusInternos(ByVal obj As TesoreriaOyDPlusInternos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusInternos")
            Me.DataContext.tblTesoreriaOrdenesInternos.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesInternos.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusInternos")
        End Try
    End Sub
#End Region
#Region "BLOQUEOS"
    Public Sub InsertTesoreriaOyDPlusBloqueos(ByVal objTesoreriaOrdenesDetalleBloqueos As TesoreriaOyDPlusBloqueos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleBloqueos.pstrUsuarioConexion, objTesoreriaOrdenesDetalleBloqueos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesDetalleBloqueos.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleBloqueos.pstrUsuarioConexion, "InsertTesoreriaOyDPlusBloqueos")
            Me.DataContext.tblTesoreriaOrdenesBloqueos.InsertOnSubmit(objTesoreriaOrdenesDetalleBloqueos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusBloqueos")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusBloqueos(ByVal objTesoreriaOrdenesDetalleBloqueos As TesoreriaOyDPlusBloqueos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleBloqueos.pstrUsuarioConexion, objTesoreriaOrdenesDetalleBloqueos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'objTesoreriaOrdenesEncabezado.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateTesoreriaOrdenesEncabezado")
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.Attach(objTesoreriaOrdenesEncabezado)
            'Me.DataContext.tblTesoreriaOrdenesEncabezados.DeleteOnSubmit(objTesoreriaOrdenesEncabezado)
            objTesoreriaOrdenesDetalleBloqueos.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleBloqueos.pstrUsuarioConexion, "UpdateTesoreriaOyDPlusBloqueos")
            Me.DataContext.tblTesoreriaOrdenesBloqueos.Attach(objTesoreriaOrdenesDetalleBloqueos, Me.ChangeSet.GetOriginal(objTesoreriaOrdenesDetalleBloqueos))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusBloqueos")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusBloqueos(ByVal obj As TesoreriaOyDPlusBloqueos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusBloqueos")
            Me.DataContext.tblTesoreriaOrdenesBloqueos.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesBloqueos.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusBloqueos")
        End Try
    End Sub
#End Region
#Region "OYD- OYD PLUS"
    Public Sub InsertTesoreriaOyDPlusOYD(ByVal objTesoreriaOrdenesDetalleOYD As TesoreriaOyDPlusOYD)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleOYD.pstrUsuarioConexion, objTesoreriaOrdenesDetalleOYD.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesDetalleOYD.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleOYD.pstrUsuarioConexion, "InsertTesoreriaOyDPlusOYD")
            Me.DataContext.tblTesoreriaOrdenesOYD.InsertOnSubmit(objTesoreriaOrdenesDetalleOYD)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusOYD")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusOYD(ByVal objTesoreriaOrdenesDetalleOYD As TesoreriaOyDPlusOYD)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOrdenesDetalleOYD.pstrUsuarioConexion, objTesoreriaOrdenesDetalleOYD.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOrdenesDetalleOYD.infosesion = DemeInfoSesion(objTesoreriaOrdenesDetalleOYD.pstrUsuarioConexion, "UpdateTesoreriaOyDPlusCarterasColectivas")
            Me.DataContext.tblTesoreriaOrdenesOYD.Attach(objTesoreriaOrdenesDetalleOYD, Me.ChangeSet.GetOriginal(objTesoreriaOrdenesDetalleOYD))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusCarterasColectivas")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusOYD(ByVal obj As TesoreriaOyDPlusOYD)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusOYD")
            Me.DataContext.tblTesoreriaOrdenesOYD.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesOYD.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusOYD")
        End Try
    End Sub
#End Region
#Region "OPERACIONES ESPECIALES- OYD PLUS"
    Public Sub InsertTesoreriaOyDPlusOperacionesEspeciales(ByVal objTesoreriaOyDPlusOperacionesEspeciales As TesoreriaOyDPlusOperacionesEspeciales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOyDPlusOperacionesEspeciales.pstrUsuarioConexion, objTesoreriaOyDPlusOperacionesEspeciales.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOyDPlusOperacionesEspeciales.infosesion = DemeInfoSesion(objTesoreriaOyDPlusOperacionesEspeciales.pstrUsuarioConexion, "InsertTesoreriaOyDPlusOperacionesEspeciales")
            Me.DataContext.tblTesoreriaOrdenesOperacionesEspeciales.InsertOnSubmit(objTesoreriaOyDPlusOperacionesEspeciales)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusOperacionesEspeciales")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusOperacionesEspeciales(ByVal objTesoreriaOyDPlusOperacionesEspeciales As TesoreriaOyDPlusOperacionesEspeciales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTesoreriaOyDPlusOperacionesEspeciales.pstrUsuarioConexion, objTesoreriaOyDPlusOperacionesEspeciales.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTesoreriaOyDPlusOperacionesEspeciales.infosesion = DemeInfoSesion(objTesoreriaOyDPlusOperacionesEspeciales.pstrUsuarioConexion, "UpdateTesoreriaOyDPlusOperacionesEspeciales")
            Me.DataContext.tblTesoreriaOrdenesOperacionesEspeciales.Attach(objTesoreriaOyDPlusOperacionesEspeciales, Me.ChangeSet.GetOriginal(objTesoreriaOyDPlusOperacionesEspeciales))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusOperacionesEspeciales")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusOperacionesEspeciales(ByVal obj As TesoreriaOyDPlusOperacionesEspeciales)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.infosesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusOperacionesEspeciales")
            Me.DataContext.tblTesoreriaOrdenesOperacionesEspeciales.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesOperacionesEspeciales.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusOperacionesEspeciales")
        End Try
    End Sub
#End Region
#End Region
#Region "RECIBOS"
    Public Function GenerarReciboplus(pstrIDEncabezado As String, pstrnombreConsecutivo As String, plngIdBancoDestino As Integer, pstrUsuario As String, ByVal pstrNombreArchivo As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Generar_ReciboCaja(pstrIDEncabezado, pstrnombreConsecutivo, plngIdBancoDestino, pstrUsuario, DemeInfoSesion(pstrUsuario, "GenerarRecibolus"), 0, pstrNombreArchivo).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarRecibolus")
            Return Nothing
        End Try
    End Function
    Public Sub InsertTesoreriaOyDPlusChequesRecibo(ByVal obj As TesoreriaOyDPlusChequesRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesChequesRecibo.InsertOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusChequesRecibo")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusChequesRecibo(ByVal obj As TesoreriaOyDPlusChequesRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesChequesRecibo.Attach(obj, Me.ChangeSet.GetOriginal(obj))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusChequesRecibo")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusChequesRecibo(ByVal obj As TesoreriaOyDPlusChequesRecibo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusChequesRecibo")
            Me.DataContext.tblTesoreriaOrdenesChequesRecibo.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesChequesRecibo.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusChequesRecibo")
        End Try
    End Sub

    Public Sub InsertTesoreriaOyDPlusCargosPagosARecibo(ByVal obj As TesoreriaOyDPlusCargosPagosARecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesCargosPagosARecibo.InsertOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusCargosPagosARecibo")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusCargosPagosARecibo(ByVal obj As TesoreriaOyDPlusCargosPagosARecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesCargosPagosARecibo.Attach(obj, Me.ChangeSet.GetOriginal(obj))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusCargosPagosARecibo")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusCargosPagosARecibo(ByVal obj As TesoreriaOyDPlusCargosPagosARecibo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusCargosPagosARecibo")
            Me.DataContext.tblTesoreriaOrdenesCargosPagosARecibo.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesCargosPagosARecibo.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusCargosPagosARecibo")
        End Try
    End Sub

    Public Sub InsertTesoreriaOyDPlusCargosPagosAFondosRecibo(ByVal obj As TesoreriaOyDPlusCargosPagosAFondosRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesCargosPagosAFondosRecibo.InsertOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusCargosPagosAFondosRecibo")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusCargosPagosAFondosRecibo(ByVal obj As TesoreriaOyDPlusCargosPagosAFondosRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesCargosPagosAFondosRecibo.Attach(obj, Me.ChangeSet.GetOriginal(obj))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusCargosPagosAFondosRecibo")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusCargosPagosAFondosRecibo(ByVal obj As TesoreriaOyDPlusCargosPagosAFondosRecibo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusCargosPagosAFondosRecibo")
            Me.DataContext.tblTesoreriaOrdenesCargosPagosAFondosRecibo.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesCargosPagosAFondosRecibo.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusCargosPagosAFondosRecibo")
        End Try
    End Sub

    Public Sub InsertTesoreriaOyDPlusTransferencias(ByVal obj As TesoreriaOyDPlusTransferenciasRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesTransferenciasRecibo.InsertOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusTransferencias")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusTransferencias(ByVal obj As TesoreriaOyDPlusTransferenciasRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesTransferenciasRecibo.Attach(obj, Me.ChangeSet.GetOriginal(obj))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusTransferencias")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusTransferenciasRecibo(ByVal obj As TesoreriaOyDPlusTransferenciasRecibo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusTransferenciasRecibo")
            Me.DataContext.tblTesoreriaOrdenesTransferenciasRecibo.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesTransferenciasRecibo.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusTransferenciasRecibo")
        End Try
    End Sub

    Public Sub InsertTesoreriaOyDPlusConsignaciones(ByVal obj As TesoreriaOyDPlusConsignacionesRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesConsignacionesRecibo.InsertOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaOyDPlusConsignaciones")
        End Try
    End Sub
    Public Sub UpdateTesoreriaOyDPlusConsignaciones(ByVal obj As TesoreriaOyDPlusConsignacionesRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblTesoreriaOrdenesConsignacionesRecibo.Attach(obj, Me.ChangeSet.GetOriginal(obj))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaOyDPlusConsignaciones")
        End Try
    End Sub
    Public Sub DeleteTesoreriaOyDPlusConsignacionesRecibo(ByVal obj As TesoreriaOyDPlusConsignacionesRecibo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTesoreriaOyDPlusConsignacionesRecibo")
            Me.DataContext.tblTesoreriaOrdenesConsignacionesRecibo.Attach(obj)
            Me.DataContext.tblTesoreriaOrdenesConsignacionesRecibo.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaOyDPlusConsignacionesRecibo")
        End Try
    End Sub

    Public Function ActualizarEncabezadoOrdenesRecibo(pstrTipo As String, pstrCodigoReceptor As String, pstrNombreConsecutivo As String, pstrTipoProducto As String,
                                                    pstrCodigoOyD As String, plngIDDocumento As Nullable(Of Integer), pstrTipoIdentificacion As String, pintNroDocumento As Integer, pstrNroDocumento As String,
                                                    pstrNombre As String, plngIDBanco As Integer, pcurValor As Decimal, pstrUsuario As String, pstrOpcion As String,
                                                    plngID As Integer, pstrEstado As String, pdtmFechaOrden As DateTime, pstrTipoCliente As String, pstrTipoIdentificacion_Entregar As String, pstrNroDocumento_Entregar As String,
                                                    pstrNombre_Entregar As String, ByVal pstrConfirmaciones As String, ByVal pstrConfirmacionesUsuario As String, ByVal pstrJustificaciones As String,
                                                    ByVal pstrJustificacionesUsuario As String, ByVal pstrAprobaciones As String, ByVal pstrAprobacionesUsuario As String, ByVal pstrTipoRetiroFondos As String, ByVal pstrCarteraColectiva As String,
                                                    pstrUsuarioWindows As String, pstrObservaciones As String, ByVal pdblValorPenalizado As Double, ByVal plogEsFondosOYD As Boolean, ByVal pdtmFechaAplicacion As Nullable(Of DateTime), psrtObservaciones As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim p1 As Nullable(Of Integer)

            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesEncabezado_Actualizar(pstrTipo, pstrCodigoReceptor, pstrNombreConsecutivo, pstrTipoProducto, pstrCodigoOyD, plngIDDocumento, pstrTipoIdentificacion, pintNroDocumento, pstrNroDocumento, pstrNombre,
                                                                                     plngIDBanco, pcurValor, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarEncabezadoOrdenesRecibo"),
                                                                                     Nothing, pstrOpcion, plngID, pstrEstado, Nothing, Nothing, Nothing, Nothing, Nothing, pstrTipoIdentificacion_Entregar, pstrNroDocumento_Entregar,
                                                                                     pstrNombre_Entregar, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, p1, Nothing, pdtmFechaOrden, pstrTipoCliente,
                                                                                     pstrConfirmaciones, pstrConfirmacionesUsuario, pstrJustificaciones, pstrJustificacionesUsuario, pstrAprobaciones, pstrAprobacionesUsuario, String.Empty, pstrCarteraColectiva, String.Empty, False, String.Empty, String.Empty, pstrTipoRetiroFondos, pstrUsuarioWindows, pstrObservaciones,
                                                                                     pdblValorPenalizado, plogEsFondosOYD, pdtmFechaAplicacion)

            intIdTesoreriaOrdenesEncabezado = p1

            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarEncabezadoOrdenesRecibo")
            Return Nothing
        End Try
    End Function
#End Region
    Public Function TesoreriaOrdenesDetalleListar_Cheque(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusCheques)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_Cheques(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ListarCheques"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_Cheque")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_Transferencia(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusTransferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_Transferencia(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ListarTransferencias"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_Transferencia")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_CarterasColectivas(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusCarterasColectivas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_CarterasColectivas(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ListarCarterasColectivas"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_CarterasColectivas")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_OYD(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusOYD)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_OYD(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalleListar_OYD"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_OYD")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_Internos(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusInternos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_Internos(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ListarInternos"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_Internos")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_Bloqueos(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusBloqueos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_Bloqueo(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ListarInternos"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_Bloqueos")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_OperacionesEspeciales(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusOperacionesEspeciales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_OperacionesEspeciales(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalleListar_OperacionesEspeciales"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_OperacionesEspeciales")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_TrasladoFondos(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusTrasladoFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_TrasladoFondos(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalleListar_TrasladoFondos"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_TrasladoFondos")
            Return Nothing
        End Try
    End Function
#Region "Listar O.Recibo"
    Public Function TesoreriaOrdenesDetalleListar_Cheques_Recibo(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusChequesRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_Cheques_Recibo(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalleListar_Cheques_Recibo"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_Cheques_Recibo")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_CargosPagosA_Recibo(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusCargosPagosARecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_CargarPagosA_Recibos(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalleListar_CargosPagosA_Recibo"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_CargosPagosA_Recibo")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_TransferenciasRecibo(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                        ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusTransferenciasRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_Transferencias_Recibos(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalleListar_CargosPagosA_Recibo"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_TransferenciasRecibo")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_ConsignacionesRecibo(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                       ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusConsignacionesRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_Consignaciones_Recibos(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalleListar_CargosPagosA_Recibo"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_ConsignacionesRecibo")
            Return Nothing
        End Try
    End Function
    Public Function TesoreriaOrdenesDetalleListar_CargosPagosAFondos_Recibo(ByVal plngIdTesoreriaOrdenesEncabezado As Integer,
                                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaOyDPlusCargosPagosAFondosRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesDetalle_Listar_CargarPagosAFondos_Recibos(plngIdTesoreriaOrdenesEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreriaOrdenesDetalleListar_CargosPagosAFondos_Recibo"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaOrdenesDetalleListar_CargosPagosAFondos_Recibo")
            Return Nothing
        End Try
    End Function
#End Region
#End Region
#Region "Otros"
    Public Function CalcularDiaHabil(ByVal pstrTipoProducto As String, ByVal pstrCarteraColectiva As String, ByVal pdtmFechaOrden As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblFechasHabiles)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ValidarDiaHabil_OrdenesTesoreria(pstrTipoProducto, pstrCarteraColectiva, pdtmFechaOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "CalcularDiaHabil"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalcularDiaHabil")
            Return Nothing
        End Try
    End Function

    Public Function TesoreroListar(ByVal pstrReceptor As String, pstrTipo As String, ByVal pstrEstado As String,
                                          ByVal pstrFomaPago As String, ByVal pdtmFecha As DateTime, plngSucursal As Integer,
                                          ByVal pstrUsuario As String, ByVal pstrCarteraColectiva As String, ByVal pstrInfoConexion As String) As List(Of Tesorero)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesorero_Listar(pstrReceptor, pstrTipo, pstrEstado, pstrFomaPago, pdtmFecha, plngSucursal, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreroListar"), 0, pstrCarteraColectiva)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreroListar")
            Return Nothing
        End Try
    End Function
    Public Function TesoreroListarDetallesRecibo(ByVal pstrReceptor As String, pstrTipo As String, ByVal pstrEstado As String,
                                      ByVal plngIdTesoreriaOrdenesEncabezado As Integer, ByVal pdtmFecha As DateTime, plngSucursal As Integer,
                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreroDetalleRecibo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesorero_ListarDetallesRecibo(pstrReceptor, pstrTipo, pstrEstado, plngIdTesoreriaOrdenesEncabezado, pdtmFecha, plngSucursal, pstrUsuario, DemeInfoSesion(pstrUsuario, "TesoreroListar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreroListarDetallesRecibo")
            Return Nothing
        End Try
    End Function

    'DEMC20180320 
    <Query(HasSideEffects:=True)>
    Public Function ValidarEstadoOrdenTesoreriaEncabezado(pstrIDsOrden As String, pstrEstadoOrdenTesoreriaCliente As String, pstrUsuario As String, pstrOpcion As String, ByVal pstrInfoConexion As String) As List(Of TempValidarEstadoOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ValidarEstadoOrdenTesoreria(pstrIDsOrden, pstrEstadoOrdenTesoreriaCliente, pstrUsuario, pstrOpcion)
            Return ret.ToList

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarEstadoOrdenTesoreriaEncabezado")
            Return Nothing
        End Try
    End Function

    Public Function RechazarDocumentos(pstrIdsDocumentos As String, pstrTipoNegocio As String, pstrUsuario As String, pstrObservacion As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RechazarDocumentos(pstrIdsDocumentos, pstrTipoNegocio, pstrUsuario, pstrObservacion).ToList
            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RechazarDocumentos")
            Return Nothing
        End Try
    End Function

    Public Function Tesorero_ConsultarRegistros_RC(ByVal pstrReceptor As String, pstrTipo As String, ByVal pstrEstado As String,
                                                   ByVal pstrFomaPago As String, ByVal pdtmFecha As DateTime, plngSucursal As Integer,
                                                   ByVal pstrUsuario As String, ByVal pstrCarteraColectiva As String, ByVal pstrInfoConexion As String) As List(Of tblTesorero_Registros_RC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesorero_ConsultarRegistros_RC(pstrReceptor, pstrTipo, pstrEstado, pstrFomaPago, pdtmFecha, plngSucursal, pstrUsuario, DemeInfoSesion(pstrUsuario, "Tesorero_ConsultarRegistros_RC"), 0, pstrCarteraColectiva)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesorero_ConsultarRegistros_RC")
            Return Nothing
        End Try
    End Function

    Public Function Tesorero_ConsultarRegistros_CE(ByVal pstrReceptor As String, pstrTipo As String, ByVal pstrEstado As String,
                                                   ByVal pstrFomaPago As String, ByVal pdtmFecha As DateTime, plngSucursal As Integer,
                                                   ByVal pstrUsuario As String, ByVal pstrCarteraColectiva As String, ByVal pstrInfoConexion As String) As List(Of tblTesorero_Registros_CE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesorero_ConsultarRegistros_CE(pstrReceptor, pstrTipo, pstrEstado, pstrFomaPago, pdtmFecha, plngSucursal, pstrUsuario, DemeInfoSesion(pstrUsuario, "Tesorero_ConsultarRegistros_CE"), 0, pstrCarteraColectiva)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesorero_ConsultarRegistros_CE")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Tesorero_GenerarRegistros(ByVal pstrOrigenIngreso As String,
                                              ByVal pstrFormaPago As String,
                                              ByVal pstrRegistros As String,
                                              ByVal pstrConsecutivoNotas As String,
                                              ByVal pstrConsecutivoCE As String,
                                              ByVal pstrConsecutivoRC As String,
                                              ByVal pintIDBanco As Nullable(Of Integer),
                                              ByVal pintIDBancoFondo As Nullable(Of Integer),
                                              ByVal pintIDBancoDestinoFondo As Nullable(Of Integer),
                                              ByVal pstrMaquina As String,
                                              ByVal pstrUsuario As String,
                                              ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesorero_GenerarRegistros(pstrOrigenIngreso, pstrFormaPago, pstrRegistros, pstrConsecutivoNotas, pstrConsecutivoCE, pstrConsecutivoRC, pintIDBanco, pintIDBancoFondo, pintIDBancoDestinoFondo, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "Tesorero_GenerarRegistros"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesorero_GenerarRegistros")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Tesorero_RechazarRegistros(ByVal pstrOrigenIngreso As String,
                                              ByVal pstrFormaPago As String,
                                              ByVal pstrRegistros As String,
                                              ByVal pstrObservaciones As String,
                                              ByVal pstrMaquina As String,
                                              ByVal pstrUsuario As String,
                                              ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesorero_RechazarDocumentos(pstrOrigenIngreso, pstrFormaPago, pstrRegistros, pstrObservaciones, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "Tesorero_RechazarRegistros"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesorero_RechazarRegistros")
            Return Nothing
        End Try
    End Function

    Public Function Get_tbl_TabHabilitar() As List(Of tbl_TabHabilitar)
        Return New List(Of tbl_TabHabilitar)
    End Function
    <Query(HasSideEffects:=True)>
    Public Function OrdenPago_ValidarTabHabilitar(ByVal pstrTipoProducto As String,
                                              ByVal pstrCarteraColectiva As String,
                                              ByVal pstrMaquina As String,
                                              ByVal pstrUsuario As String,
                                              ByVal pstrUsuarioWindows As String,
                                              ByVal pstrInfoConexion As String) As List(Of tbl_TabHabilitar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenPago_VerificarTabHabilitar(pstrTipoProducto, pstrCarteraColectiva, pstrUsuario, pstrUsuarioWindows, DemeInfoSesion(pstrUsuario, "OrdenPago_ValidarTabHabilitar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenPago_ValidarTabHabilitar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function OrdenRecaudo_ValidarTabHabilitar(ByVal pstrTipoProducto As String,
                                              ByVal pstrCarteraColectiva As String,
                                              ByVal pstrMaquina As String,
                                              ByVal pstrUsuario As String,
                                              ByVal pstrUsuarioWindows As String,
                                              ByVal pstrInfoConexion As String) As List(Of tbl_TabHabilitar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenRecaudo_VerificarTabHabilitar(pstrTipoProducto, pstrCarteraColectiva, pstrUsuario, pstrUsuarioWindows, DemeInfoSesion(pstrUsuario, "OrdenRecaudo_ValidarTabHabilitar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenRecaudo_ValidarTabHabilitar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Tesorero_AutogestionarDocumento(ByVal pintIDOrden As Integer,
                                              ByVal pstrComentarios As String,
                                              ByVal pstrMaquina As String,
                                              ByVal pstrUsuario As String,
                                              ByVal pstrUsuarioWindows As String,
                                              ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesorero_AutogestionarDocumento(pintIDOrden, pstrComentarios, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "Tesorero_AutogestionarDocumento"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesorero_AutogestionarDocumento")
            Return Nothing
        End Try
    End Function

    Public Sub InserttblTesorero_Registros_RC(ByVal obj As tblTesorero_Registros_RC)

    End Sub
    Public Sub UpdatetblTesorero_Registros_RC(ByVal obj As tblTesorero_Registros_RC)

    End Sub

    Public Sub InserttblTesorero_Registros_CE(ByVal obj As tblTesorero_Registros_CE)

    End Sub
    Public Sub UpdatetblTesorero_Registros_CE(ByVal obj As tblTesorero_Registros_CE)

    End Sub
#End Region

#Region "OYD PLUS ORDENES RECIBO"
    <Query(HasSideEffects:=True)>
    Public Function OYDPLUS_ValidarOrdenesRecibo(pstrTipo As String, pstrCodigoReceptor As String, pstrNombreConsecutivo As String, pstrTipoProducto As String,
                                                 pstrCodigoOyD As String, plngIDDocumento As Nullable(Of Integer), pstrTipoIdentificacion As String, pintNroDocumento As Integer, pstrNroDocumento As String,
                                                 pstrNombre As String, plngIDBanco As Integer, pcurValor As Decimal, pstrOpcion As String,
                                                 plngID As Integer, pstrEstado As String, pdtmFechaOrden As DateTime, pstrTipoCliente As String, pstrTipoIdentificacion_Entregar As String, pstrNroDocumento_Entregar As String,
                                                 pstrNombre_Entregar As String, ByVal pstrConfirmaciones As String, ByVal pstrConfirmacionesUsuario As String, ByVal pstrJustificaciones As String,
                                                 ByVal pstrJustificacionesUsuario As String, ByVal pstrAprobaciones As String, ByVal pstrAprobacionesUsuario As String,
                                                 ByVal pstrDetalleCheques As String, ByVal pstrDetalleTransferencias As String, ByVal pstrDetalleConsignacion As String, ByVal pstrDetalleCargarPagosA As String, ByVal pstrDetalleCargarPagosAFondos As String,
                                                 ByVal pstrUsuario As String, ByVal pstrCarteraColectiva As String, ByVal pstrTipoAccionFondos As String, ByVal pstrUsuarioWindows As String, pstrObservaciones As String,
                                                 ByVal plogEsFondoOYD As Boolean, ByVal pdtmFechaAplicacion As Nullable(Of DateTime), ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim p1 As Nullable(Of Integer)

            Dim ret = Me.DataContext.uspOyDNet_ValidarIngresoOrdenesRecibo(pstrTipo, pstrCodigoReceptor, pstrNombreConsecutivo, pstrTipoProducto, pstrCodigoOyD, plngIDDocumento, pstrTipoIdentificacion, pintNroDocumento, pstrNroDocumento, pstrNombre,
                                                                                     plngIDBanco, pcurValor, pstrOpcion, plngID, pstrEstado, Nothing, Nothing, Nothing, Nothing, Nothing, pstrTipoIdentificacion_Entregar, pstrNroDocumento_Entregar,
                                                                                     pstrNombre_Entregar, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, p1, Nothing, pdtmFechaOrden, pstrTipoCliente,
                                                                                     pstrConfirmaciones, pstrConfirmacionesUsuario, pstrJustificaciones, pstrJustificacionesUsuario, pstrAprobaciones, pstrAprobacionesUsuario, String.Empty, pstrCarteraColectiva, String.Empty,
                                                                                     pstrDetalleCheques, pstrDetalleTransferencias, pstrDetalleConsignacion, pstrDetalleCargarPagosA, pstrDetalleCargarPagosAFondos, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarEncabezadoOrdenesRecibo"),
                                                                                     Nothing, pstrTipoAccionFondos, pstrUsuarioWindows, pstrObservaciones, plogEsFondoOYD, pdtmFechaAplicacion).ToList


            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarOrdenesRecibo")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_CancelarOrdenOYDPLUS(ByVal plngID As Integer, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CancelarEdicionOrden(plngID, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_CancelarOrdenOYDPLUS"), ClsConstantes.GINT_ErrorPersonalizado)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CancelarOrdenOYDPLUS")
            Return Nothing
        End Try
    End Function

    Public Function SucursalesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Sucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Sucursales_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "SucursalesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "SucursalesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function BancosNacionalesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BancosNacionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BancosNacionales_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "BancosNacionalesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BancosNacionalesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function BancosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Banco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Cuentasbancarias_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "BancosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BancosFiltrar")
            Return Nothing
        End Try
    End Function

#End Region
#Region "logTransaccionesFinonset"

    Public Function LogTransaccionesFinonset(ByVal pdtmFechaInicial As String, ByVal pdtmFechaFinal As String,
                                             ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LogTransaccionesFINONSET)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_logTransaccionesFinonset_Filtrar(pdtmFechaInicial, pdtmFechaFinal, pstrUsuario, DemeInfoSesion(pstrUsuario, "LogTransaccionesFinonset"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LogTransaccionesFinonset")
            Return Nothing
        End Try
    End Function
#End Region

#Region "OMNIBUS"

    Public Function VerificarTipoCuentaOmnibus_CarteraColectiva(ByVal pstrCarteraColectiva As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strRetorno As String = String.Empty
            Me.DataContext.uspOyDNet_VerificarCarteraColectivaOmnibus(pstrCarteraColectiva, strRetorno, pstrUsuario, DemeInfoSesion(pstrUsuario, "VerificarTipoCuentaOmnibus_CarteraColectiva"), 0)
            Return strRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarTipoCuentaOmnibus_CarteraColectiva")
            Return Nothing
        End Try
    End Function

#End Region

#End Region

#Region "COMUNES"
    Public Function GetAuditorias(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of Auditoria)
        Dim objResultado As IQueryable(Of Auditoria) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objResultado = Me.DataContext.Auditorias
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetAuditorias")
        End Try
        Return objResultado
    End Function
#End Region

End Class