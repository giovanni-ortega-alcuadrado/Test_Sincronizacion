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
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSDeceval
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()> _
Partial Public Class OYDPLUSDecevalDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSDecevalDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "INVERSIONISTAS"
    <Query(HasSideEffects:=True)>
    Public Function InversionistasConsultar(ByVal pIdComitente As String, pstrEstado As String,
                                             ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inversionistas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_DecevalInversionistas_Consultar(pIdComitente, pstrEstado,
                                                                               pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarTesoreriaOydPlus"),
                                                                               0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InversionistasConsultar")
            Return Nothing
        End Try
    End Function

    Public Function InversionistasActualizar(pstrAccion As String, ByVal pstrRegistros As String,
                                            ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblResultadoEnvio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_DecevalInversionistas_Actualizar(pstrAccion, pstrRegistros, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarTesoreriaOydPlus"),
                                                                               0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InversionistasActualizar")
            Return Nothing
        End Try
    End Function

    Public Function DetalleInversionistasConsultar(ByVal pintIDInversionista As Integer,
                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleInversionistas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_DecevalDetalleInversionistas_Consultar(pintIDInversionista, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarTesoreriaOydPlus"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleInversionistasConsultar")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateInversionistas(ByVal currentInversionistas As Inversionistas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateInversionistas")
        End Try
    End Sub
    Public Sub InsertInversionistas(ByVal currentInversionistas As Inversionistas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertInversionistas")
        End Try
    End Sub

#End Region

#Region "ARCHIVOS"

    Public Function ConfiguracionArchivoConsultar(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionArchivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConfiguracionArchivo_Consultar(pstrUsuario, DemeInfoSesion(pstrUsuario, "ConfiguracionArchivoConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionArchivoConsultar")
            Return Nothing
        End Try
    End Function

    Public Function ConfiguracionArchivoSolicitar(ByVal pstrArchivo As String, ByVal pintTipoArchivo As Nullable(Of Integer), ByVal pstrCodigoISIN As String, ByVal pintCuentaInversionista As Nullable(Of Integer), ByVal pstrCodigoDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblResultadoEnvioArchivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_DecevalArchivos_Solicitar(pstrArchivo, pintTipoArchivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConfiguracionArchivoSolicitar"), 0, pstrCodigoISIN, pintCuentaInversionista, pstrCodigoDeposito)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionArchivoSolicitar")
            Return Nothing
        End Try
    End Function

    Public Function ArchivosConsultar(ByVal pdtmFecha As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Archivos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_DecevalArchivos_Consultar(pdtmFecha, pstrUsuario, DemeInfoSesion(pstrUsuario, "ArchivosConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ArchivosConsultar")
            Return Nothing
        End Try
    End Function

    Public Function DetalleArchivosConsultar(ByVal pintIDArchivo As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleArchivos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_DecevalDetalleArchivos_Consultar(pintIDArchivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "DetalleArchivosConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleArchivosConsultar")
            Return Nothing
        End Try
    End Function

#End Region



End Class