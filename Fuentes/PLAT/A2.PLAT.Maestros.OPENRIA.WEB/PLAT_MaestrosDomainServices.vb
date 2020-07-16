
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
Imports System.Threading.Tasks


'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class PLAT_MaestrosDomainServices
    Inherits DbDomainService(Of dbPLAT_MaestrosEntities)

    Public Sub New()

    End Sub

#Region "PAIS"

    Public Sub InserttblPais(ByVal pCurrentItem As tblPais)
    End Sub
    Public Sub UpdatetblPais(ByVal pCurrentItem As tblPais)
    End Sub
    Public Sub DeletetblPais(ByVal pCurrentItem As tblPais)
    End Sub

    Public Function GettblPais() As List(Of tblPais)
        Return New List(Of tblPais)
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblPais)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Filtrar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Consultar(ByVal pintID As Nullable(Of Integer), ByVal pstrNombre As String, ByVal pstrCodigoISOAlfa2 As String, ByVal pstrCodigoISOAlfa3 As String, ByVal pstrCodigoISONumerico As String, ByVal plogActivo As Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblPais)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrNombre = System.Web.HttpUtility.UrlDecode(pstrNombre)
            pstrCodigoISOAlfa2 = System.Web.HttpUtility.UrlDecode(pstrCodigoISOAlfa2)
            pstrCodigoISOAlfa3 = System.Web.HttpUtility.UrlDecode(pstrCodigoISOAlfa3)
            pstrCodigoISONumerico = System.Web.HttpUtility.UrlDecode(pstrCodigoISONumerico)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Consultar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Consultar(pintID, pstrNombre, pstrCodigoISOAlfa2, pstrCodigoISOAlfa3, pstrCodigoISONumerico, plogActivo, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_ConsultarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPais
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_ConsultarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_ConsultarID(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_ConsultarDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPais
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_ConsultarDefecto", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_ConsultarID(-1, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_ConsultarDefecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_EliminarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_EliminarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_EliminarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Validar(ByVal pintID As Nullable(Of Integer), ByVal pstrNombre As String, ByVal pstrCodigoISOAlfa2 As String, ByVal pstrCodigoISOAlfa3 As String, ByVal pstrCodigoISONumerico As String, ByVal plogActivo As Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Validar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Validar(pintID, pstrNombre, pstrCodigoISOAlfa2, pstrCodigoISOAlfa3, pstrCodigoISONumerico, plogActivo, False, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Validar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "PAIS ESTADOS"

    Public Sub InserttbltblPais_Estado(ByVal pCurrentItem As tblPais_Estado)
    End Sub
    Public Sub UpdatetbltblPais_Estado(ByVal pCurrentItem As tblPais_Estado)
    End Sub
    Public Sub DeletetbltblPais_Estado(ByVal pCurrentItem As tblPais_Estado)
    End Sub

    Public Function GettblPais_Estado() As List(Of tblPais_Estado)
        Return New List(Of tblPais_Estado)
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Estado_Consultar(ByVal pintIDPais As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblPais_Estado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Estado_Consultar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Estado_Consultar(pintIDPais, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Estado_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Estado_ConsultarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPais_Estado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Estado_ConsultarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Estado_ConsultarID(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Estado_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Estado_ConsultarDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPais_Estado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Estado_ConsultarDefecto", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Estado_ConsultarID(-1, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Estado_ConsultarDefecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Estado_EliminarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Estado_EliminarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Estado_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Estado_EliminarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Estado_Validar(ByVal pintID As Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrNombre As String, ByVal pintIDPais As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Estado_Validar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Estado_Validar(pintID, pstrCodigo, pstrNombre, pintIDPais, False, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Estado_Validar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "PAIS ESTADOS"

    Public Sub InserttbltblPais_Moneda(ByVal pCurrentItem As tblPais_Moneda)
    End Sub
    Public Sub UpdatetbltblPais_Moneda(ByVal pCurrentItem As tblPais_Moneda)
    End Sub
    Public Sub DeletetbltblPais_Moneda(ByVal pCurrentItem As tblPais_Moneda)
    End Sub

    Public Function GettblPais_Moneda() As List(Of tblPais_Moneda)
        Return New List(Of tblPais_Moneda)
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Moneda_Consultar(ByVal pintIDPais As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblPais_Moneda)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Moneda_Consultar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Moneda_Consultar(pintIDPais, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Moneda_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Moneda_ConsultarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPais_Moneda
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Moneda_ConsultarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Moneda_ConsultarID(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Moneda_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Moneda_ConsultarDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPais_Moneda
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Moneda_ConsultarDefecto", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Moneda_ConsultarID(-1, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Moneda_ConsultarDefecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Moneda_EliminarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Moneda_EliminarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Moneda_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Moneda_EliminarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Pais_Moneda_Validar(ByVal pintID As Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pintIDPais As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("Pais_Moneda_Validar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Pais_Moneda_Validar(pintID, pstrCodigo, pintIDPais, False, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "Pais_Moneda_Validar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "PARAMETRIZACION TRIBUTARIA"

    Public Sub InserttblParametrizacionTributaria(ByVal pCurrentItem As tblParametrizacionTributaria)
    End Sub
    Public Sub UpdatetblParametrizacionTributaria(ByVal pCurrentItem As tblParametrizacionTributaria)
    End Sub
    Public Sub DeletetblParametrizacionTributaria(ByVal pCurrentItem As tblParametrizacionTributaria)
    End Sub

    Public Function GettblParametrizacionTributaria() As List(Of tblParametrizacionTributaria)
        Return New List(Of tblParametrizacionTributaria)
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ParametrizacionTributaria_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblParametrizacionTributaria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("ParametrizacionTributaria_Filtrar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_ParametrizacionTributaria_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "ParametrizacionTributaria_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ParametrizacionTributaria_Consultar(ByVal pintID As Nullable(Of Integer), ByVal pstrNombre As String, ByVal pstrModulo As String, ByVal pstrFuncionalidad As String, ByVal pintIDPais As Nullable(Of Integer), ByVal pintIDCiudad As Nullable(Of Integer), ByVal plogActivo As Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblParametrizacionTributaria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrNombre = System.Web.HttpUtility.UrlDecode(pstrNombre)
            Dim strInfoSession As String = DemeInfoSesion("ParametrizacionTributaria_Consultar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_ParametrizacionTributaria_Consultar(pintID, pstrNombre, pstrModulo, pstrFuncionalidad, pintIDPais, pintIDCiudad, plogActivo, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "ParametrizacionTributaria_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ParametrizacionTributaria_ConsultarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblParametrizacionTributaria
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("ParametrizacionTributaria_ConsultarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_ParametrizacionTributaria_ConsultarID(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "ParametrizacionTributaria_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ParametrizacionTributaria_ConsultarDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblParametrizacionTributaria
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("ParametrizacionTributaria_ConsultarDefecto", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_ParametrizacionTributaria_ConsultarID(-1, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "ParametrizacionTributaria_ConsultarDefecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ParametrizacionTributaria_EliminarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("ParametrizacionTributaria_EliminarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_ParametrizacionTributaria_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "ParametrizacionTributaria_EliminarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ParametrizacionTributaria_Validar(ByVal pintID As Nullable(Of Integer), ByVal pstrNombre As String, ByVal pdblTasaImpositiva As Nullable(Of Double), ByVal pstrModulo As String, ByVal pstrFuncionalidad As String, ByVal pintIDPais As Nullable(Of Integer), ByVal pintIDCiudad As Nullable(Of Integer), ByVal plogActivo As Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("ParametrizacionTributaria_Validar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_ParametrizacionTributaria_Validar(pintID, pstrNombre, pdblTasaImpositiva, pstrModulo, pstrFuncionalidad, pintIDPais, pintIDCiudad, plogActivo, False, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MAESTROS_GEN", "ParametrizacionTributaria_Validar")
            Return Nothing
        End Try
    End Function
#End Region

End Class

