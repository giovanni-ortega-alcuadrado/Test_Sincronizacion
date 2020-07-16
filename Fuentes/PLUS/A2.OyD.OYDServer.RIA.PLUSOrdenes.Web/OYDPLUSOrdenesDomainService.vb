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
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenes
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()> _
Partial Public Class OYDPLUSOrdenesDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSOrdenesDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

    Public Function cargarModulosUsuario(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrUsuarioUtilidades As String, ByVal pstrClave As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of OyDPLUSOrdenes.ModulosUsuario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, A2Utilidades.CifrarSL.descifrar(pstrUsuario), pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not String.IsNullOrEmpty(pstrAplicacion) Then
                pstrAplicacion = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrAplicacion))
            End If
            If Not String.IsNullOrEmpty(pstrVersion) Then
                pstrVersion = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrVersion))
            End If
            If Not String.IsNullOrEmpty(pstrUsuarioUtilidades) Then
                pstrUsuarioUtilidades = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrUsuarioUtilidades))
            End If
            If Not String.IsNullOrEmpty(pstrClave) Then
                pstrClave = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrClave))
            End If
            If Not String.IsNullOrEmpty(pstrUsuario) Then
                pstrUsuario = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrUsuario))
            End If
            If Not String.IsNullOrEmpty(pstrMaquina) Then
                pstrMaquina = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrMaquina))
            End If

            Dim ret = Me.DataContext.uspOyDNet_CargarModulosUsuario(pstrAplicacion, pstrVersion, pstrUsuarioUtilidades, pstrClave, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "cargarModulosUsuario"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarModulosUsuario")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function cargarModulosUsuario_Prueba() As List(Of OyDPLUSOrdenes.ModulosUsuario)
        Return New List(Of OyDPLUSOrdenes.ModulosUsuario)
    End Function

    Public Function EsperarCargaControl(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EsperarCargaControl")
            Return Nothing
        End Try
    End Function

End Class