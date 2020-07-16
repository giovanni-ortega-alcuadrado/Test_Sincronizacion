
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
Imports System.Data.Entity.Core.Objects


'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class DER_OperacionesDomainServices
    Inherits DbDomainService(Of DER_OperacionesEntities)

    Public Sub New()

    End Sub

#Region "UTILIDADES"

    <Invoke(HasSideEffects:=True)>
    Public Function Utilitarios_ConsultarCombos(ByVal pstrUsuario As String,
                                                Optional ByVal pstrOpcion As String = "",
                                                Optional ByVal pstrTopico As String = "",
                                                Optional ByVal pstrParametroAdicional1 As String = "",
                                                Optional ByVal pstrParametroAdicional2 As String = "",
                                                Optional ByVal pstrParametroAdicional3 As String = "", Optional ByVal pstrInfoConexion As String = "") As List(Of CPX_tblCombos)
        Try
            Dim strInfoSession As String = DemeInfoSesion("UTILITARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Procesos_Utilitarios_CargarCombos(pstrOpcion, pstrTopico, pstrParametroAdicional1, pstrParametroAdicional2, pstrParametroAdicional3, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "UTILITARIOS", "Utilitarios_ConsultarCombos")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Cruce Operaciones"

    Public Sub InserttblCRCC_CTRADES(ByVal pCurrentItem As tblCRCC_CTRADES)
    End Sub
    Public Sub UpdatetblCRCC_CTRADES(ByVal pCurrentItem As tblCRCC_CTRADES)
    End Sub
    Public Sub DeletetblCRCC_CTRADES(ByVal pCurrentItem As tblCRCC_CTRADES)
    End Sub
    Public Function EntidadtblCRCC_CTRADES() As tblCRCC_CTRADES
        Return New tblCRCC_CTRADES
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CruceOperaciones_Consultar(ByVal pstrNemotecnico As String, ByVal pintFolioBVC As Long,
                                               ByVal pintFolioCamara As Integer, ByVal bitCruzada As Boolean,
                                               ByVal pdtmFechaImportacion As String, ByVal pstrGestionOperaciones As String,
                                               ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblCRCC_CTRADES)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            pstrNemotecnico = System.Web.HttpUtility.UrlDecode(pstrNemotecnico)
            pdtmFechaImportacion = System.Web.HttpUtility.UrlDecode(pdtmFechaImportacion)
            pstrGestionOperaciones = System.Web.HttpUtility.UrlDecode(pstrGestionOperaciones)

            Dim strInfoSession As String = DemeInfoSesion("CRUCEOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblCRCC_CTRADES)

            objRetorno = DbContext.usp_Procesos_CruceOperaciones_Consultar(pstrNemotecnico, pintFolioBVC, pintFolioCamara, bitCruzada, pdtmFechaImportacion, pstrGestionOperaciones, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CRUCEOPERACIONES", "CruceOperaciones_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CruceOperaciones_CruceAutomatico(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CRUCEOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Procesos_CruceOperaciones_CruceAutomatico(strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CRUCEOPERACIONES", "CruceOperaciones_CruceAutomatico")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CruceOperaciones_DescalzarLiquidaciones(ByVal pintIdImportacion As Integer, ByVal pbitCalcularCantidadPendiente As Boolean,
                                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CRUCEOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Procesos_CruceOperaciones_DescalzarLiquidaciones(pintIdImportacion, pbitCalcularCantidadPendiente, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CRUCEOPERACIONES", "CruceOperaciones_CruceAutomatico")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CruceOperaciones_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CRUCEOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Procesos_CruceOperaciones_Retirar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CRUCEOPERACIONES", "CruceOperaciones_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CruceOperaciones_CruceManual(ByVal pintID As Integer, ByVal pintTime As Integer, ByVal pstrTradeReference As String,
                                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CRUCEOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Procesos_CruceOperaciones_CruceManual(pintID, pintTime, pstrTradeReference, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CRUCEOPERACIONES", "CruceOperaciones_CruceManual")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ValidarOrdenes_Liquidacion_InicialTimeSpread(ByVal pstrIntIdImportacionesLiq As Integer, ByVal pstrTradeReference As String,
                                                                  ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CRUCEOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Procesos_ValidarOrdenes_Liquidacion_InicialTimeSpread(pstrIntIdImportacionesLiq, pstrTradeReference, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CRUCEOPERACIONES", "ValidarOrdenes_Liquidacion_InicialTimeSpread")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CruceOperaciones_TimeSpread_Consultar(ByVal pintOrden As String,
                                                            ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblCRCC_CTRADES_TimeSpread)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            'pintOrden = System.Web.HttpUtility.UrlDecode(pintOrden)

            Dim strInfoSession As String = DemeInfoSesion("CRUCEOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblCRCC_CTRADES_TimeSpread)

            objRetorno = DbContext.usp_Procesos_CruceOperaciones_TimeSpread_Consultar(pintOrden, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CRUCEOPERACIONES", "CruceOperaciones_TimeSpread_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CruceOperaciones_TipoOperacion_TimeSpread(pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name,
                                       Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion,
                                       Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            'pintOrden = System.Web.HttpUtility.UrlDecode(pintOrden)

            Dim strInfoSession As String = DemeInfoSesion("CRUCEOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New ObjectParameter("pstrTradeType", String.Empty)

            DbContext.usp_Procesos_CruceOperaciones_TipoOperacion_TimeSpread(objRetorno, pstrUsuario, strInfoSession)

            Return objRetorno.Value.ToString

        Catch ex As Exception
            ManejarError(ex, "CRUCEOPERACIONES", "CruceOperaciones_TipoOperacion_TimeSpread")
            Return Nothing
        End Try
    End Function


#End Region

End Class



