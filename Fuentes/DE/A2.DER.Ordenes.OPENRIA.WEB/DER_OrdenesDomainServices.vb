
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports OpenRiaServices.DomainServices.EntityFramework
Imports A2.OyD.Infraestructura


'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class DER_OrdenesDomainServices
    Inherits DbDomainService(Of dbDER_OrdenesEntities)

    Public Sub New()

    End Sub

#Region "UTILIDADES"

    '<Invoke(HasSideEffects:=True)>
    'Public Function Utilitarios_ConsultarCombos(ByVal pstrUsuario As String,
    '                                            Optional ByVal pstrOpcion As String = "",
    '                                            Optional ByVal pstrTopico As String = "",
    '                                            Optional ByVal pstrParametroAdicional1 As String = "",
    '                                            Optional ByVal pstrParametroAdicional2 As String = "",
    '                                            Optional ByVal pstrParametroAdicional3 As String = "") As List(Of CPX_tblCombos)
    '    Try
    '        Dim strInfoSession As String = DemeInfoSesion("UTILITARIOS", pstrUsuario)
    '        Dim strUsuario As String = DemeUsuario(pstrUsuario)
    '        Dim objRetorno = DbContext.usp_Procesos_Utilitarios_CargarCombos(pstrOpcion, pstrTopico, pstrParametroAdicional1, pstrParametroAdicional2, pstrParametroAdicional3, strUsuario, strInfoSession).ToList
    '        Return objRetorno
    '    Catch ex As Exception
    '        ManejarError(ex, "UTILITARIOS", "Utilitarios_ConsultarCombos")
    '        Return Nothing
    '    End Try
    'End Function

#End Region



#Region "ORDENES"

    Public Sub InserttblOrdenesDV(ByVal pCurrentItem As tblOrdenesDV)
    End Sub
    Public Sub UpdatetblOrdenesDV(ByVal pCurrentItem As tblOrdenesDV)
    End Sub
    Public Sub DeletetblOrdenesDV(ByVal pCurrentItem As tblOrdenesDV)
    End Sub
    Public Function EntidadtblOrdenesDV() As tblOrdenesDV
        Return New tblOrdenesDV
    End Function


    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDV_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblOrdenesDV
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("ORDENESDV", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Procesos_Ordenes_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDV", "OrdenesDV_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDV_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblOrdenesDV)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("ORDENESDV", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Procesos_Ordenes_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDV", "OrdenesDV_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDV_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblOrdenesDV
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("ORDENESDV", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Procesos_Ordenes_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDV", "OrdenesDV_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDV_Consultar(ByVal pintIdOrdenEnc As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblOrdenesDV)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("ORDENESDV", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblOrdenesDV)

            objRetorno = DbContext.usp_Procesos_Ordenes_Consultar(pintIdOrdenEnc, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDV", "OrdenesDV_Consultar")
            Return Nothing
        End Try
    End Function

    '<Invoke(HasSideEffects:=True)>
    'Public Function OrdenesDV_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
    '    Try
    '        Dim strInfoSession As String = DemeInfoSesion("ORDENESDV", pstrUsuario)
    '        Dim strUsuario As String = DemeUsuario(pstrUsuario)
    '        Dim objRetorno = DbContext.usp_Procesos_Ordenes_Eliminar(pintID, strUsuario, strInfoSession).ToList
    '        Return objRetorno
    '    Catch ex As Exception
    '        ManejarError(ex, "ORDENESDV", "OrdenesDV_Eliminar")
    '        Return Nothing
    '    End Try
    'End Function

    '<Invoke(HasSideEffects:=True)>
    'Public Function DetalleDatosTributarios_Actualizar(ByVal pintID As Nullable(Of Integer),
    '                                                   ByVal pintIdDatosTributarios As Nullable(Of Integer),
    '                                                   ByVal pdtmFechaInicio As Date,
    '                                                   ByVal pdtmFechaFinal As Nullable(Of Date),
    '                                                   ByVal pnumValor As Decimal,
    '                                                   ByVal plogSoloValidar As Nullable(Of Boolean),
    '                                                   ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
    '    Try
    '        Dim strInfoSession As String = DemeInfoSesion("DETALLEDATOSTRIBUTARIOS", pstrUsuario)
    '        Dim strUsuario As String = DemeUsuario(pstrUsuario)
    '        Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
    '        objRetorno = DbContext.usp_Procesos_Ordenes_Validar(pintID, pintIdDatosTributarios, pdtmFechaInicio, pdtmFechaFinal, pnumValor, plogSoloValidar, strUsuario, strInfoSession).ToList
    '        Return objRetorno
    '    Catch ex As Exception
    '        ManejarError(ex, "DETALLEDATOSTRIBUTARIOS", "DetalleDatosTributarios_Actualizar")
    '        Return Nothing
    '    End Try
    'End Function

    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDV_Actualizar(ByVal pintID As Nullable(Of Integer),
                                         ByVal pintNroOrden As Nullable(Of Integer),
                                         ByVal pintIdOrdenEnc As Nullable(Of Integer),
                                         ByVal pintIdTipoOperacion As Nullable(Of Integer),
                                         ByVal pintIdCuenta As Nullable(Of Integer),
                                         ByVal pintIdSubcuenta As Nullable(Of Integer),
                                         ByVal pintIdProductoVencimiento As Nullable(Of Integer),
                                         ByVal pintIdVencimientoTramoFinal As Nullable(Of Integer),
                                         ByVal pintIdTipoOpcionProducto As Nullable(Of Short),
                                         ByVal pintIdComercial As Nullable(Of Integer),
                                         ByVal pbitRegistradaEnBolsa As Nullable(Of Boolean),
                                         ByVal pintIdTipoRegistro As Nullable(Of Short),
                                         ByVal pintIdContraparte As Nullable(Of Integer),
                                         ByVal pnumCantidad As Nullable(Of Decimal),
                                         ByVal pnumPrecio As Nullable(Of Decimal),
                                         ByVal pnumPrima As Nullable(Of Decimal),
                                         ByVal pnumComision As Nullable(Of Decimal),
                                         ByVal pbitComisionPorPorcentaje As Nullable(Of Boolean),
                                         ByVal pnumPorcentajeComision As Nullable(Of Decimal),
                                         ByVal pbitFacturaComisionInicio As Nullable(Of Boolean),
                                         ByVal pbitEsGiveOut As Nullable(Of Boolean),
                                         ByVal pintIdFirmaGiveOut As Nullable(Of Integer),
                                         ByVal pstrReferenciaGiveOut As String,
                                         ByVal pintIdTipoOrdenDuracion As Nullable(Of Short),
                                         ByVal pintIdTipoOrdenEjecucion As Nullable(Of Short),
                                         ByVal pintIdTipoOrdenNaturaleza As Nullable(Of Short),
                                         ByVal pnumCantidadMinima As Nullable(Of Decimal),
                                         ByVal pintIdSesion As Nullable(Of Integer),
                                         ByVal pintDiasAvisoCumplimiento As Nullable(Of Byte),
                                         ByVal pbitPrecioStop As Nullable(Of Boolean),
                                         ByVal pnumPrecioStop As Nullable(Of Decimal),
                                         ByVal pbitCantidadOculta As Nullable(Of Boolean),
                                         ByVal pnumCantidadVisible As Nullable(Of Decimal),
                                         ByVal pbitSeguimientoInstrumento As Nullable(Of Boolean),
                                         ByVal pintIdInstrumentoSeguimiento As Nullable(Of Integer),
                                         ByVal pintIdComponentePrecio As Nullable(Of Integer),
                                         ByVal pstrObservaciones As String,
                                         ByVal pstrComercialesComision As String,
                                         ByVal pstrInstruccionesPago As String,
                                         ByVal pintIdFinalidadOperacion As Nullable(Of Integer),
                                         ByVal pintIdTipoFinalidad As Nullable(Of Integer),
                                         ByVal pnumMontoCubrir As Nullable(Of Decimal),
                                         ByVal pnumCantidadCancelar As Nullable(Of Decimal),
                                         ByVal pnumPrecioSpot As Nullable(Of Decimal),
                                         ByVal pintIdTipoPrima As Nullable(Of Integer),
                                         ByVal pnumPrecioPrima As Nullable(Of Decimal),
                                         ByVal plogSoloValidar As Nullable(Of Boolean),
                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("ORDENESDV", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Procesos_Ordenes_Validar(pintID, pintNroOrden, pintIdOrdenEnc, pintIdTipoOperacion, pintIdCuenta, pintIdSubcuenta,
                                                                pintIdProductoVencimiento, pintIdVencimientoTramoFinal, pintIdTipoOpcionProducto, pintIdComercial,
                                                                pbitRegistradaEnBolsa, pintIdTipoRegistro, pintIdContraparte, pnumCantidad, pnumPrecio, pnumPrima, pnumComision,
                                                                pbitComisionPorPorcentaje, pnumPorcentajeComision, pbitFacturaComisionInicio, pbitEsGiveOut,
                                                                pintIdFirmaGiveOut, pstrReferenciaGiveOut, pintIdTipoOrdenDuracion, pintIdTipoOrdenEjecucion, pintIdTipoOrdenNaturaleza,
                                                                pnumCantidadMinima, pintIdSesion, pintDiasAvisoCumplimiento, pbitPrecioStop, pnumPrecioStop, pbitCantidadOculta,
                                                                pnumCantidadVisible, pbitSeguimientoInstrumento, pintIdInstrumentoSeguimiento, pintIdComponentePrecio, pstrObservaciones,
                                                                pstrComercialesComision, pstrInstruccionesPago, pintIdFinalidadOperacion, pintIdTipoFinalidad,
                                                                pnumCantidadCancelar, pnumPrecioSpot, pintIdTipoPrima,
                                                                pnumPrecioPrima, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDV", "OrdenesDV_Actualizar")
            Return Nothing
        End Try
    End Function



#End Region

#Region "FIRMA GIVEUP"

    Public Function EntidadtblFirmasGiveUp() As tblFirmasGiveUp
        Return New tblFirmasGiveUp
    End Function


    <Invoke(HasSideEffects:=True)>
    Public Function FirmasGiveUp_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblFirmasGiveUp
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FIRMASGIVEUP", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FirmasGiveUp_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FIRMASGIVEUP", "FirmasGiveUp_ID")
            Return Nothing
        End Try
    End Function

#End Region

#Region "INSTRUMENTOS"

    Public Function EntidadtblProductos() As tblProductos
        Return New tblProductos
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Productos_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblProductos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_Productos_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOS", "Productos_ID")
            Return Nothing
        End Try
    End Function

#End Region



End Class



