
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.Riesgos.RIA.Web
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura
Imports System.Data.SqlClient

'Implements application logic using the RiesgosModelDataContext context.
' TODO: Add your application logic to these methods or in additional methods.
' TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
' Also consider adding roles to restrict access as appropriate.
'<RequiresAuthentication> _
<EnableClientAccess()> _
Public Class RiesgosDomainService
    Inherits LinqToSqlDomainService(Of RiesgosModelDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Consultas"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertConsultas(ByVal newConsultas As Consultas)
        Try
            newConsultas.strInfoSesion = DemeInfoSesion(newConsultas.pstrUsuarioConexion, "InsertConsultas")
            Me.DataContext.Consultas.InsertOnSubmit(newConsultas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConsultas")
        End Try
    End Sub

    Public Sub UpdateConsultas(ByVal currentConsultas As Consultas)
        Try
            currentConsultas.strInfoSesion = DemeInfoSesion(currentConsultas.pstrUsuarioConexion, "UpdateConsultas")
            Me.DataContext.Consultas.Attach(currentConsultas, Me.ChangeSet.GetOriginal(currentConsultas))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsultas")
        End Try
    End Sub

    Public Sub DeleteConsultas(ByVal deleteConsultas As Consultas)
        Try
            deleteConsultas.strInfoSesion = DemeInfoSesion(deleteConsultas.pstrUsuarioConexion, "DeleteConsultas")
            Me.DataContext.Consultas.Attach(deleteConsultas)
            Me.DataContext.Consultas.DeleteOnSubmit(deleteConsultas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConsultas")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarConsultas(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Consultas)
        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_MC_Consultas_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarConsultas"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarConsultas")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConsultas(ByVal pstrConsulta As String, ByVal pstrProcedimiento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Consultas)
        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_MC_Consultas_Consultar(String.Empty, pstrConsulta, pstrProcedimiento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConsultas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConsultas")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConsultasPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Consultas
        Dim objConsultas As Consultas = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_MC_Consultas_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConsultasPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objConsultas = ret.FirstOrDefault
            End If
            Return objConsultas
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConsultasPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarConsultasSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Consultas)
        Dim objTask As Task(Of List(Of Consultas)) = Me.FiltrarConsultasAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarConsultasAsync(ByVal pstrFiltro As String, pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Consultas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Consultas)) = New TaskCompletionSource(Of List(Of Consultas))()
        objTaskComplete.TrySetResult(FiltrarConsultas(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConsultasSync(ByVal pstrConsulta As String, ByVal pstrProcedimiento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Consultas)
        Dim objTask As Task(Of List(Of Consultas)) = Me.ConsultarConsultasAsync(pstrConsulta, pstrProcedimiento, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConsultasAsync(ByVal pstrConsulta As String, ByVal pstrProcedimiento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Consultas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Consultas)) = New TaskCompletionSource(Of List(Of Consultas))()
        objTaskComplete.TrySetResult(ConsultarConsultas(pstrConsulta, pstrProcedimiento, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConsultasPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Consultas
        Dim objTask As Task(Of Consultas) = Me.ConsultarConsultasPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConsultasPorDefectoAsync(pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Consultas)
        Dim objTaskComplete As TaskCompletionSource(Of Consultas) = New TaskCompletionSource(Of Consultas)()
        objTaskComplete.TrySetResult(ConsultarConsultasPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Alertas"

#Region "Métodos modelo para activar funcionalidad RIA"

#End Region

#Region "Métodos asincrónicos"
    Public Function ConsultarAlertas(ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Alertas)
        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_MC_Alertas_Consultar(pdtmFecha, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConsultas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarAlertas")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"
    Public Function ConsultarAlertasSync(ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Alertas)
        Dim objTask As Task(Of List(Of Alertas)) = Me.ConsultarAlertasAsync(pdtmFecha, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarAlertasAsync(ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Alertas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Alertas)) = New TaskCompletionSource(Of List(Of Alertas))()
        objTaskComplete.TrySetResult(ConsultarAlertas(pdtmFecha, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
#End Region

#End Region

#Region "Botones X Toolbar"

#Region "Métodos modelo para activar funcionalidad RIA"

#End Region

#Region "Métodos asincrónicos"
    Public Function ConsultarBotonesXToolbar(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrToolbar As String, ByVal pstrUsuario As String, ByVal pstrClave As String, ByVal pstrInfoConexion As String) As List(Of ToolbarsPorAplicacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim A2LIB30 As New A2LIB_v30.clsA2Lib_v30

            Dim ret = Me.DataContext.uspA2CA_consultarToolbarsUsuario(pstrAplicacion, pstrVersion, pstrUsuario, A2LIB30.EncryptPwd(pstrClave), pstrToolbar, String.Empty, _
                                                                      True, String.Empty, String.Empty, String.Empty, String.Empty, Nothing, _
                                                                      Nothing, Nothing, 0, False).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarBotonesXToolbar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"
    Public Function ConsultarBotonesXToolbarSync(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrToolbar As String, ByVal pstrUsuario As String, ByVal pstrClave As String, ByVal pstrInfoConexion As String) As List(Of ToolbarsPorAplicacion)
        Dim objTask As Task(Of List(Of ToolbarsPorAplicacion)) = Me.ConsultarBotonesXToolbarAsync(pstrAplicacion, pstrVersion, pstrToolbar, pstrUsuario, pstrClave, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarBotonesXToolbarAsync(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrToolbar As String, ByVal pstrUsuario As String, ByVal pstrClave As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ToolbarsPorAplicacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ToolbarsPorAplicacion)) = New TaskCompletionSource(Of List(Of ToolbarsPorAplicacion))()
        objTaskComplete.TrySetResult(ConsultarBotonesXToolbar(pstrAplicacion, pstrVersion, pstrToolbar, pstrUsuario, pstrClave, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
#End Region

#End Region

#Region "ParametrosConsola"

    Public Function leerParametrosAppConsola(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ItemCombo)
        Dim lstParametros As List(Of ItemCombo) = Nothing
        Dim strTopico As String = "ParametrosAppConsola"

        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            '/ Parámetros definidos en la base de datos
            'Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos(strTopico, pstrUsuario)
            'lstParametros = ret.ToList ' Retornar los parámetros que vienen de base de datos

            'TODO: leer los parametros de la forma como lo hace la consola en oyd y encuenta
            lstParametros = New List(Of ItemCombo)
            lstParametros.AddRange(New List(Of ItemCombo) From
                                   {
                                       New ItemCombo With {.ID = "APLICACIONDERECHOSAUTOR", .Descripcion = "Copyright © Alcuadrado SA 2015", .Categoria = "AP_DER_AUT"},
                                       New ItemCombo With {.ID = "APLICACIONFECHA", .Descripcion = "1/Mar/2015", .Categoria = "AP_DER_AUT"},
                                       New ItemCombo With {.ID = "APLICACIONNOMBRE", .Descripcion = "Riesgos", .Categoria = "AP_DER_AUT"},
                                       New ItemCombo With {.ID = "APLICACIONVERSION", .Descripcion = "1.0.0.0", .Categoria = "AP_DER_AUT"},
                                       New ItemCombo With {.ID = "A2.Riesgos.SL", .Descripcion = "1.0.0.0", .Categoria = "VER_AP_NET"},
                                       New ItemCombo With {.ID = "MOSTRAR_DATOS_TECNICOS", .Descripcion = "SI", .Categoria = "VER_AP_NET"}
                                   })



            '/ Cadena de conexión a la base de datos
            lstParametros.Add(New ItemCombo With {.Categoria = strTopico, .Descripcion = A2Utilidades.CifrarSL.cifrar(A2Utilidades.Cifrar.descifrar(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.A2RiesgosConnectionString)), .ID = "A2Consola_CnxBaseDatos".ToUpper})
            lstParametros.Add(New ItemCombo With {.Categoria = strTopico, .Descripcion = "N/A", .ID = "A2Consola_CarpetaUploads".ToUpper})
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "leerParametrosApp")
        End Try

        Return lstParametros
    End Function

    Public Function leerParametrosAppConsolaSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ItemCombo)
        Dim objTask As Task(Of List(Of ItemCombo)) = Me.leerParametrosAppConsolaAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function leerParametrosAppConsolaAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ItemCombo))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ItemCombo)) = New TaskCompletionSource(Of List(Of ItemCombo))()
        objTaskComplete.TrySetResult(leerParametrosAppConsola(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Riesgo - Administrador de Seguridad"

#Region "Métodos modelo para activar funcionalidad RIA"

#End Region

#Region "Métodos asincrónicos"
    Public Function InsertarRiesgo(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrTitulo As String, _
                                    ByVal pstrNombre As String, ByVal pstrTooltip As String, ByVal pstrRoles As String, _
                                    ByVal pstrDescripcion As String, ByVal pstrNombreTipoObjeto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.usp_MC_Riesgo_Insertar(pstrAplicacion, pstrVersion, pstrTitulo, pstrNombre, pstrTooltip, pstrRoles, pstrDescripcion,
                                                            String.Empty, pstrNombreTipoObjeto, pstrUsuario, Nothing, 0, Nothing)

            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarRiesgo")
            Return False
        End Try
    End Function

    Public Function EliminarRiesgo(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrTitulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.usp_MC_Riesgo_Eliminar(pstrAplicacion, pstrVersion, pstrTitulo, pstrUsuario, Nothing, 0, Nothing)

            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarRiesgo")
            Return False
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"
    Public Function InsertarRiesgoSync(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrTitulo As String, _
                                    ByVal pstrNombre As String, ByVal pstrTooltip As String, ByVal pstrRoles As String, _
                                    ByVal pstrDescripcion As String, ByVal pstrNombreTipoObjeto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean

        Dim objTask As Task(Of Boolean) = Me.InsertarRiesgoAsync(pstrAplicacion, pstrVersion, pstrTitulo, pstrNombre, pstrTooltip, pstrRoles, pstrDescripcion,
                                                            pstrNombreTipoObjeto, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function InsertarRiesgoAsync(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrTitulo As String, _
                                    ByVal pstrNombre As String, ByVal pstrTooltip As String, ByVal pstrRoles As String, _
                                    ByVal pstrDescripcion As String, ByVal pstrNombreTipoObjeto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)

        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(InsertarRiesgo(pstrAplicacion, pstrVersion, pstrTitulo, pstrNombre, pstrTooltip, pstrRoles, pstrDescripcion,
                                                            pstrNombreTipoObjeto, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function EliminarRiesgoSync(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrTitulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean

        Dim objTask As Task(Of Boolean) = Me.EliminarRiesgoAsync(pstrAplicacion, pstrVersion, pstrTitulo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function EliminarRiesgoAsync(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrTitulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)

        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(EliminarRiesgo(pstrAplicacion, pstrVersion, pstrTitulo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
#End Region

#End Region

#Region "Actualización Cache SQL"

#Region "Métodos modelo para activar funcionalidad RIA"

#End Region

#Region "Métodos asincrónicos"
    Public Function RecargarCacheSQL(ByVal pstrMetodo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.Riesgos.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objParameters As New List(Of SqlParameter)
            Dim strConexion As String = ObtenerCadenaConexion(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.dbOYDConnectionString)
            objParameters.Add(CrearSQLParameter("pstrMetodos", pstrMetodo, SqlDbType.VarChar))
            objParameters.Add(CrearSQLParameter("plogLimpiarMetodo", True, SqlDbType.Bit))

            ExecuteNonQuery(strConexion, "uspMC_CargarLibrosCacheSQL", objParameters)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RecargarCacheSQL")
            Return False
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"
    Public Function RecargarCacheSQLSync(ByVal pstrMetodo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean

        Dim objTask As Task(Of Boolean) = Me.RecargarCacheSQLAsync(pstrMetodo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function RecargarCacheSQLAsync(ByVal pstrMetodo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)

        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(RecargarCacheSQL(pstrMetodo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

End Class

