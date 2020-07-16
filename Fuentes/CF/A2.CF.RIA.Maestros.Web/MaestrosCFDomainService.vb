Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros
Imports System.Web
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

''' <summary>
''' DomainServices para las pantallas correspondientes a la migración de Titulos2008 a .NET
''' </summary>
''' Creado por       : Germán Arbey González Osorio (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Diciembre 15/2014
''' Pruebas CB       : Germán Arbey González Osorio - Diciembre 15/2014 - Resultado Ok
''' <remarks></remarks>

<EnableClientAccess()>
Partial Public Class MaestrosCFDomainService
    Inherits LinqToSqlDomainService(Of CF_MaestrosDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Calificadoras"

#Region "Métodos modelo para activar funcionalidad RIA"

    Private Property ret As List(Of ConfiguracionContableMultimoneda)

    Public Sub InsertCalificadoras(ByVal objCalificadoras As Calificadoras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCalificadoras.pstrUsuarioConexion, objCalificadoras.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCalificadoras.InfoSesion = DemeInfoSesion(objCalificadoras.pstrUsuarioConexion, "InsertCalificadoras")
            Me.DataContext.Calificadoras.InsertOnSubmit(objCalificadoras)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCalificadoras")
        End Try
    End Sub
    Public Sub UpdateCalificadoras(ByVal currentCalificadoras As Calificadoras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCalificadoras.pstrUsuarioConexion, currentCalificadoras.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCalificadoras.InfoSesion = DemeInfoSesion(currentCalificadoras.pstrUsuarioConexion, "UpdateCalificadoras")
            Me.DataContext.Calificadoras.Attach(currentCalificadoras, Me.ChangeSet.GetOriginal(currentCalificadoras))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCalificadoras")
        End Try
    End Sub
    Public Sub DeleteCalificadoras(ByVal objCalificadoras As Calificadoras)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCalificadoras.pstrUsuarioConexion, objCalificadoras.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCalificadoras.InfoSesion = DemeInfoSesion(objCalificadoras.pstrUsuarioConexion, "DeleteCalificadoras")
            Me.DataContext.Calificadoras.Attach(objCalificadoras)
            Me.DataContext.Calificadoras.DeleteOnSubmit(objCalificadoras)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCalificadoras")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarCalificadoras(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Calificadoras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Calificadoras_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarCalificadoras"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarCalificadoras")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCalificadoras(pintIdCalificadora As Integer, pintCodCalificadora As Integer, pstrNombreCalificadora As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Calificadoras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Calificadoras_Consultar(String.Empty, pintIdCalificadora, pintCodCalificadora, pstrNombreCalificadora, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCalificadoras"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCalificadoras")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarCalificadorasPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Calificadoras
        Dim objCalificadoras As Calificadoras = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Calificadoras_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, -1, 0, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCalificadorasPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCalificadoras = ret.FirstOrDefault
            End If
            Return objCalificadoras
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCalificadorasPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarCalificadorasSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Calificadoras)
        Dim objTask As Task(Of List(Of Calificadoras)) = Me.FiltrarCalificadorasAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarCalificadorasAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Calificadoras))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Calificadoras)) = New TaskCompletionSource(Of List(Of Calificadoras))()
        objTaskComplete.TrySetResult(FiltrarCalificadoras(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCalificadorasSync(ByVal pintIdCalificadora As Integer, ByVal pintCodCalificadora As Integer, ByVal pstrNombreCalificadora As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Calificadoras)
        Dim objTask As Task(Of List(Of Calificadoras)) = Me.ConsultarCalificadorasAsync(pintIdCalificadora, pintCodCalificadora, pstrNombreCalificadora, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCalificadorasAsync(ByVal pintIdCalificadora As Integer, ByVal pintCodCalificadora As Integer, ByVal pstrNombreCalificadora As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Calificadoras))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Calificadoras)) = New TaskCompletionSource(Of List(Of Calificadoras))()
        objTaskComplete.TrySetResult(ConsultarCalificadoras(pintIdCalificadora, pintCodCalificadora, pstrNombreCalificadora, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCalificadorasPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Calificadoras
        Dim objTask As Task(Of Calificadoras) = Me.ConsultarCalificadorasPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCalificadorasPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Calificadoras)
        Dim objTaskComplete As TaskCompletionSource(Of Calificadoras) = New TaskCompletionSource(Of Calificadoras)()
        objTaskComplete.TrySetResult(ConsultarCalificadorasPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CalificacionesInversiones"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCalificacionesInversiones(ByVal newCalificacionesInversiones As CalificacionesInversiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newCalificacionesInversiones.pstrUsuarioConexion, newCalificacionesInversiones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newCalificacionesInversiones.strInfoSesion = DemeInfoSesion(newCalificacionesInversiones.pstrUsuarioConexion, "InsertCalificacionesInversiones")
            Me.DataContext.CalificacionesInversiones.InsertOnSubmit(newCalificacionesInversiones)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCalificacionesInversiones")
        End Try
    End Sub

    Public Sub UpdateCalificacionesInversiones(ByVal currentCalificacionesInversiones As CalificacionesInversiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCalificacionesInversiones.pstrUsuarioConexion, currentCalificacionesInversiones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCalificacionesInversiones.strInfoSesion = DemeInfoSesion(currentCalificacionesInversiones.pstrUsuarioConexion, "UpdateCalificacionesInversiones")
            Me.DataContext.CalificacionesInversiones.Attach(currentCalificacionesInversiones, Me.ChangeSet.GetOriginal(currentCalificacionesInversiones))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCalificacionesInversiones")
        End Try
    End Sub

    Public Sub DeleteCalificacionesInversiones(ByVal deleteCalificacionesInversiones As CalificacionesInversiones)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteCalificacionesInversiones.pstrUsuarioConexion, deleteCalificacionesInversiones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteCalificacionesInversiones.strInfoSesion = DemeInfoSesion(deleteCalificacionesInversiones.pstrUsuarioConexion, "DeleteCalificacionesInversiones")
            Me.DataContext.CalificacionesInversiones.Attach(deleteCalificacionesInversiones)
            Me.DataContext.CalificacionesInversiones.DeleteOnSubmit(deleteCalificacionesInversiones)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCalificacionesInversiones")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarCalificacionesInversiones(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesInversiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalificacionesInversiones_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarCalificacionesInversiones"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarCalificacionesInversiones")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCalificacionesInversiones(ByVal pstrTipoCalificacion As String, ByVal pstrTipoCalificacionInversion As String, ByVal pstrNombreCalificacionInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesInversiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalificacionesInversiones_Consultar(String.Empty, pstrTipoCalificacion, pstrTipoCalificacionInversion, pstrNombreCalificacionInversion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCalificacionesInversiones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCalificacionesInversiones")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCalificacionesInversionesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CalificacionesInversiones
        Dim objCalificacionesInversiones As CalificacionesInversiones = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalificacionesInversiones_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCalificacionesInversionesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCalificacionesInversiones = ret.FirstOrDefault
            End If
            Return objCalificacionesInversiones
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCalificacionesInversionesPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarCalificacionesInversionesSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesInversiones)
        Dim objTask As Task(Of List(Of CalificacionesInversiones)) = Me.FiltrarCalificacionesInversionesAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarCalificacionesInversionesAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CalificacionesInversiones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CalificacionesInversiones)) = New TaskCompletionSource(Of List(Of CalificacionesInversiones))()
        objTaskComplete.TrySetResult(FiltrarCalificacionesInversiones(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCalificacionesInversionesSync(ByVal pstrTipoCalificacion As String, ByVal pstrTipoCalificacionInversion As String, ByVal pstrNombreCalificacionInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesInversiones)
        Dim objTask As Task(Of List(Of CalificacionesInversiones)) = Me.ConsultarCalificacionesInversionesAsync(pstrTipoCalificacion, pstrTipoCalificacionInversion, pstrNombreCalificacionInversion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCalificacionesInversionesAsync(ByVal pstrTipoCalificacion As String, ByVal pstrTipoCalificacionInversion As String, ByVal pstrNombreCalificacionInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CalificacionesInversiones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CalificacionesInversiones)) = New TaskCompletionSource(Of List(Of CalificacionesInversiones))()
        objTaskComplete.TrySetResult(ConsultarCalificacionesInversiones(pstrTipoCalificacion, pstrTipoCalificacionInversion, pstrNombreCalificacionInversion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCalificacionesInversionesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CalificacionesInversiones
        Dim objTask As Task(Of CalificacionesInversiones) = Me.ConsultarCalificacionesInversionesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCalificacionesInversionesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CalificacionesInversiones)
        Dim objTaskComplete As TaskCompletionSource(Of CalificacionesInversiones) = New TaskCompletionSource(Of CalificacionesInversiones)()
        objTaskComplete.TrySetResult(ConsultarCalificacionesInversionesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CalificacionesCalificadora"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCalificacionesCalificadora(ByVal objCalificacionesCalificadora As CalificacionesCalificadora)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCalificacionesCalificadora.pstrUsuarioConexion, objCalificacionesCalificadora.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCalificacionesCalificadora.InfoSesion = DemeInfoSesion(objCalificacionesCalificadora.pstrUsuarioConexion, "InsertCalificacionesCalificadora")
            Me.DataContext.CalificacionesCalificadora.InsertOnSubmit(objCalificacionesCalificadora)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCalificacionesCalificadora")
        End Try
    End Sub

    Public Sub UpdateCalificacionesCalificadora(ByVal currentCalificacionesCalificadora As CalificacionesCalificadora)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCalificacionesCalificadora.pstrUsuarioConexion, currentCalificacionesCalificadora.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCalificacionesCalificadora.InfoSesion = DemeInfoSesion(currentCalificacionesCalificadora.pstrUsuarioConexion, "UpdateCalificacionesCalificadora")
            Me.DataContext.CalificacionesCalificadora.Attach(currentCalificacionesCalificadora, Me.ChangeSet.GetOriginal(currentCalificacionesCalificadora))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCalificacionesCalificadora")
        End Try
    End Sub

    Public Sub DeleteCalificacionesCalificadora(ByVal objCalificacionesCalificadora As CalificacionesCalificadora)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCalificacionesCalificadora.pstrUsuarioConexion, objCalificacionesCalificadora.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCalificacionesCalificadora.InfoSesion = DemeInfoSesion(objCalificacionesCalificadora.pstrUsuarioConexion, "DeleteCalificacionesCalificadora")
            Me.DataContext.CalificacionesCalificadora.Attach(objCalificacionesCalificadora)
            Me.DataContext.CalificacionesCalificadora.DeleteOnSubmit(objCalificacionesCalificadora)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCalificacionesCalificadora")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarCalificacionesCalificadora(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesCalificadora)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalificacionesCalificadora_Filtrar(pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarCalificacionesCalificadora"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarCalificacionesCalificadora")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCalificacionesCalificadora(pintIdCalificaCalificadora As Integer, pintCodCalificadora As Integer, pintIDCalificacion As Integer, pintCodigoSuper As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesCalificadora)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalificacionesCalificadora_Consultar(String.Empty, pintIdCalificaCalificadora, pintCodCalificadora, pintIDCalificacion, pintCodigoSuper, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEspeciesClaseInversion"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCalificacionesCalificadora")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarCalificacionesCalificadoraPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CalificacionesCalificadora
        Dim objCalificacionesCalificadora As CalificacionesCalificadora = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalificacionesCalificadora_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, -1, 0, 0, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCalificacionesCalificadoraPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCalificacionesCalificadora = ret.FirstOrDefault
            End If
            Return objCalificacionesCalificadora
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCalificacionesCalificadoraPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarCalificacionesCalificadoraSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesCalificadora)
        Dim objTask As Task(Of List(Of CalificacionesCalificadora)) = Me.FiltrarCalificacionesCalificadoraAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarCalificacionesCalificadoraAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CalificacionesCalificadora))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CalificacionesCalificadora)) = New TaskCompletionSource(Of List(Of CalificacionesCalificadora))()
        objTaskComplete.TrySetResult(FiltrarCalificacionesCalificadora(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCalificacionesCalificadoraSync(ByVal pintIdCalificaCalificadora As Integer, ByVal pintCodCalificadora As Integer, ByVal pintIDCalificacion As Integer, ByVal pintCodigoSuper As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesCalificadora)
        Dim objTask As Task(Of List(Of CalificacionesCalificadora)) = Me.ConsultarCalificacionesCalificadoraAsync(pintIdCalificaCalificadora, pintCodCalificadora, pintIDCalificacion, pintCodigoSuper, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCalificacionesCalificadoraAsync(ByVal pintIdCalificaCalificadora As Integer, ByVal pintCodCalificadora As Integer, ByVal pintIDCalificacion As Integer, ByVal pintCodigoSuper As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CalificacionesCalificadora))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CalificacionesCalificadora)) = New TaskCompletionSource(Of List(Of CalificacionesCalificadora))()
        objTaskComplete.TrySetResult(ConsultarCalificacionesCalificadora(pintIdCalificaCalificadora, pintCodCalificadora, pintIDCalificacion, pintCodigoSuper, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCalificacionesCalificadoraPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CalificacionesCalificadora
        Dim objTask As Task(Of CalificacionesCalificadora) = Me.ConsultarCalificacionesCalificadoraPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCalificacionesCalificadoraPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CalificacionesCalificadora)
        Dim objTaskComplete As TaskCompletionSource(Of CalificacionesCalificadora) = New TaskCompletionSource(Of CalificacionesCalificadora)()
        objTaskComplete.TrySetResult(ConsultarCalificacionesCalificadoraPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ChoquesTasasInteres"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertChoquesTasasInteres(ByVal newChoquesTasasInteres As ChoquesTasasInteres)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newChoquesTasasInteres.pstrUsuarioConexion, newChoquesTasasInteres.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newChoquesTasasInteres.strInfoSesion = DemeInfoSesion(newChoquesTasasInteres.pstrUsuarioConexion, "InsertChoquesTasasInteres")
            Me.DataContext.ChoquesTasasInteres.InsertOnSubmit(newChoquesTasasInteres)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertChoquesTasasInteres")
        End Try
    End Sub

    Public Sub UpdateChoquesTasasInteres(ByVal currentChoquesTasasInteres As ChoquesTasasInteres)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentChoquesTasasInteres.pstrUsuarioConexion, currentChoquesTasasInteres.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentChoquesTasasInteres.strInfoSesion = DemeInfoSesion(currentChoquesTasasInteres.pstrUsuarioConexion, "UpdateChoquesTasasInteres")
            Me.DataContext.ChoquesTasasInteres.Attach(currentChoquesTasasInteres, Me.ChangeSet.GetOriginal(currentChoquesTasasInteres))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateChoquesTasasInteres")
        End Try
    End Sub

    Public Sub DeleteChoquesTasasInteres(ByVal deleteChoquesTasasInteres As ChoquesTasasInteres)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteChoquesTasasInteres.pstrUsuarioConexion, deleteChoquesTasasInteres.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteChoquesTasasInteres.strInfoSesion = DemeInfoSesion(deleteChoquesTasasInteres.pstrUsuarioConexion, "DeleteChoquesTasasInteres")
            Me.DataContext.ChoquesTasasInteres.Attach(deleteChoquesTasasInteres)
            Me.DataContext.ChoquesTasasInteres.DeleteOnSubmit(deleteChoquesTasasInteres)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteChoquesTasasInteres")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarChoquesTasasInteres(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ChoquesTasasInteres)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ChoquesTasasInteres_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarChoquesTasasInteres"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarChoquesTasasInteres")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarChoquesTasasInteres(pintZona As Integer, pintBanda As Integer, pstrTipoMoneda As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ChoquesTasasInteres)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ChoquesTasasInteres_Consultar(String.Empty, pintZona, pintBanda, pstrTipoMoneda, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarChoquesTasasInteres"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarChoquesTasasInteres")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarChoquesTasasInteresPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ChoquesTasasInteres
        Dim objChoquesTasasInteres As ChoquesTasasInteres = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ChoquesTasasInteres_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, 0, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarChoquesTasasInteresPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objChoquesTasasInteres = ret.FirstOrDefault
            End If
            Return objChoquesTasasInteres
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarChoquesTasasInteresPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarChoquesTasasInteresSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ChoquesTasasInteres)
        Dim objTask As Task(Of List(Of ChoquesTasasInteres)) = Me.FiltrarChoquesTasasInteresAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarChoquesTasasInteresAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ChoquesTasasInteres))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ChoquesTasasInteres)) = New TaskCompletionSource(Of List(Of ChoquesTasasInteres))()
        objTaskComplete.TrySetResult(FiltrarChoquesTasasInteres(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarChoquesTasasInteresSync(ByVal pintZona As Integer, ByVal pintBanda As Integer, ByVal pstrTipoMoneda As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ChoquesTasasInteres)
        Dim objTask As Task(Of List(Of ChoquesTasasInteres)) = Me.ConsultarChoquesTasasInteresAsync(pintZona, pintBanda, pstrTipoMoneda, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarChoquesTasasInteresAsync(ByVal pintZona As Integer, ByVal pintBanda As Integer, ByVal pstrTipoMoneda As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ChoquesTasasInteres))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ChoquesTasasInteres)) = New TaskCompletionSource(Of List(Of ChoquesTasasInteres))()
        objTaskComplete.TrySetResult(ConsultarChoquesTasasInteres(pintZona, pintBanda, pstrTipoMoneda, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarChoquesTasasInteresPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ChoquesTasasInteres
        Dim objTask As Task(Of ChoquesTasasInteres) = Me.ConsultarChoquesTasasInteresPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarChoquesTasasInteresPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ChoquesTasasInteres)
        Dim objTaskComplete As TaskCompletionSource(Of ChoquesTasasInteres) = New TaskCompletionSource(Of ChoquesTasasInteres)()
        objTaskComplete.TrySetResult(ConsultarChoquesTasasInteresPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Entidades"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertEntidades(ByVal newEntidades As Entidades)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newEntidades.pstrUsuarioConexion, newEntidades.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newEntidades.strInfoSesion = DemeInfoSesion(newEntidades.pstrUsuarioConexion, "InsertEntidades")
            Me.DataContext.Entidades.InsertOnSubmit(newEntidades)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEntidades")
        End Try
    End Sub

    Public Sub UpdateEntidades(ByVal currentEntidades As Entidades)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEntidades.pstrUsuarioConexion, currentEntidades.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEntidades.strInfoSesion = DemeInfoSesion(currentEntidades.pstrUsuarioConexion, "UpdateEntidades")
            Me.DataContext.Entidades.Attach(currentEntidades, Me.ChangeSet.GetOriginal(currentEntidades))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEntidades")
        End Try
    End Sub

    Public Sub DeleteEntidades(ByVal deleteEntidades As Entidades)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteEntidades.pstrUsuarioConexion, deleteEntidades.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteEntidades.strInfoSesion = DemeInfoSesion(deleteEntidades.pstrUsuarioConexion, "DeleteEntidades")
            Me.DataContext.Entidades.Attach(deleteEntidades)
            Me.DataContext.Entidades.DeleteOnSubmit(deleteEntidades)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEntidades")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarEntidades(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Entidades)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Entidades_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarEntidades"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarEntidades")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEntidades(ByVal pstrTipoIdentificacion As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pintIDTipoEntidad As System.Nullable(Of Integer), ByVal pintIDCodigoCIIU As System.Nullable(Of Integer), ByVal pintIDCalificacionInversion As System.Nullable(Of Integer), ByVal pintIDCalificadora As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Entidades)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Entidades_Consultar(String.Empty, pstrTipoIdentificacion, pstrNroDocumento, pstrNombre, pintIDTipoEntidad, pintIDCodigoCIIU, pintIDCalificacionInversion, pintIDCalificadora, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEntidades"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEntidades")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEntidadesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Entidades
        Dim objEntidades As Entidades = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Entidades_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, 0, 0, 0, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEntidadesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objEntidades = ret.FirstOrDefault
            End If
            Return objEntidades
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEntidadesPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Function ValidarEmisor(pstrNroDocumento As String, pintIDTipoEntidad As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_Entidades_ValidarEmisor(pstrNroDocumento, pintIDTipoEntidad, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarEmisor"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarEmisor")
            Return Nothing
        End Try
    End Function

    Public Function CalcularDigitoNIT(pstrNroDocumento As String, pintDigitoVerificacion As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspCalculosFinancieros_DigitoNIT_Calcular(pstrNroDocumento, pintDigitoVerificacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "CalcularDigitoNIT"), 0)
            Return pintDigitoVerificacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalcularDigitoNIT")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Descripción:   Nuevo funcion para retornar el listado de subgrupos dependiendo de la seleccion
    ''' Responsable:   Yessid Andres Paniagua Pabon (AlCuadrado)
    ''' Fecha:         Diciembre 28/2015
    ''' ID del cambio: YAPP20151228
    ''' </summary>
    Public Function ListarSubgrupos(ByVal pIntPadre As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of SubGrupo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_SubGruposListar(pIntPadre, pstrUsuario, DemeInfoSesion(pstrUsuario, "ListarSubGrupos"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ListarSubgrupos")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    ''' <summary>
    ''' Descripción:   Nuevo funcion para retornar el listado de subgrupos dependiendo de la seleccion
    ''' Responsable:   Yessid Andres Paniagua Pabon (AlCuadrado)
    ''' Fecha:         Diciembre 28/2015
    ''' ID del cambio: YAPP20151228
    ''' </summary>
    Public Function ListarSubgruposSync(ByVal pIntPadre As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of SubGrupo)
        Dim objTask As Task(Of List(Of SubGrupo)) = Me.ListarSubgruposAsync(pIntPadre, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ListarSubgruposAsync(ByVal pIntPadre As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of SubGrupo))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of SubGrupo)) = New TaskCompletionSource(Of List(Of SubGrupo))()
        objTaskComplete.TrySetResult(ListarSubgrupos(pIntPadre, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function




    Public Function FiltrarEntidadesSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Entidades)
        Dim objTask As Task(Of List(Of Entidades)) = Me.FiltrarEntidadesAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarEntidadesAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Entidades))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Entidades)) = New TaskCompletionSource(Of List(Of Entidades))()
        objTaskComplete.TrySetResult(FiltrarEntidades(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEntidadesSync(ByVal pstrTipoIdentificacion As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String,
                                           ByVal pintIDTipoEntidad As System.Nullable(Of Integer), ByVal pintIDCodigoCIIU As System.Nullable(Of Integer),
                                           ByVal pintIDCalificacionInversion As System.Nullable(Of Integer), ByVal pintIDCalificadora As System.Nullable(Of Integer),
                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Entidades)
        Dim objTask As Task(Of List(Of Entidades)) = Me.ConsultarEntidadesAsync(pstrTipoIdentificacion, pstrNroDocumento, pstrNombre, pintIDTipoEntidad, pintIDCodigoCIIU,
                                                                                pintIDCalificacionInversion, pintIDCalificadora, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEntidadesAsync(ByVal pstrTipoIdentificacion As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String,
                                             ByVal pintIDTipoEntidad As System.Nullable(Of Integer), ByVal pintIDCodigoCIIU As System.Nullable(Of Integer),
                                             ByVal pintIDCalificacionInversion As System.Nullable(Of Integer), ByVal pintIDCalificadora As System.Nullable(Of Integer),
                                             ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Entidades))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Entidades)) = New TaskCompletionSource(Of List(Of Entidades))()
        objTaskComplete.TrySetResult(ConsultarEntidades(pstrTipoIdentificacion, pstrNroDocumento, pstrNombre, pintIDTipoEntidad, pintIDCodigoCIIU,
                                                        pintIDCalificacionInversion, pintIDCalificadora, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEntidadesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Entidades
        Dim objTask As Task(Of Entidades) = Me.ConsultarEntidadesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEntidadesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Entidades)
        Dim objTaskComplete As TaskCompletionSource(Of Entidades) = New TaskCompletionSource(Of Entidades)()
        objTaskComplete.TrySetResult(ConsultarEntidadesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CalificacionesEmisor"
    Public Function CalificacionesEmisorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesEmisor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalificacionEmisor_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "CalificacionesEmisorFiltrar"), 0).ToList
            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalificacionesEmisorFiltrar")
            Return Nothing
        End Try
    End Function
    Public Function TraerCalificacionesEmisorPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CalificacionesEmisor
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CalificacionesEmisor
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Usuario_Consecutivo = 
            'e.Nombre_Consecutivo = 
            'e.Actualizacion = 
            e.strUsuario = HttpContext.Current.User.Identity.Name
            'e.IDConsecutivosUsuarios = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCalificacionesEmisorPorDefecto")
            Return Nothing
        End Try
    End Function
    Public Function CalificacionesEmisorConsultar(pintIdCalificacionEmisor As Integer,
                                                  pintIdEmisor As Integer,
                                                       pintIdCalificacionInversion As Integer,
                                                       ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CalificacionesEmisor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalificacionEmisor_Consultar(pintIdCalificacionEmisor, pintIdEmisor, pintIdCalificacionInversion, pstrUsuario, DemeInfoSesion(pstrUsuario, "CalificacionesEmisorConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalificacionesEmisorConsultar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCalificacionesEmisor(ByVal Obj As CalificacionesEmisor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Obj.pstrUsuarioConexion, Obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Obj.InfoSesion = DemeInfoSesion(Obj.pstrUsuarioConexion, "InsertCalificacionesEmisor")
            Me.DataContext.CalificacionesEmisor.InsertOnSubmit(Obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCalificacionesEmisor")
        End Try
    End Sub
    Public Sub UpdateCalificacionesEmisor(ByVal obj As CalificacionesEmisor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "UpdateCalificacionesEmisor")
            Me.DataContext.CalificacionesEmisor.Attach(obj, Me.ChangeSet.GetOriginal(obj))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCalificacionesEmisor")
        End Try
    End Sub
    Public Sub DeleteCalificacionesEmisor(ByVal obj As CalificacionesEmisor)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConsecutivosUsuarios_Eliminar( pUsuario, DemeInfoSesion(pstrUsuario, "DeleteConsecutivosUsuario"),0).ToList
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteCalificacionesEmisor")
            Me.DataContext.CalificacionesEmisor.Attach(obj)
            Me.DataContext.CalificacionesEmisor.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCalificacionesEmisor")
        End Try
    End Sub
#End Region

#Region "Parámetros"
    Public Function ParametrosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Parametro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Parametros_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ParametrosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ParametrosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ParametrosConsultar(ByVal pIDparametro As Integer, ByVal pParametro As String, ByVal pValor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Parametro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Parametros_Consultar(pIDparametro, pParametro, pValor, DemeInfoSesion(pstrUsuario, "BuscarParametros"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarParametros")
            Return Nothing
        End Try
    End Function

    Public Function TraerParametroPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Parametro
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Parametro
            e.IDparametro = -1
            e.Parametro = ""
            'e.Valor = 
            'e.Descripcion = 
            'e.Tipo = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerParametroPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertParametro(ByVal Parametro As Parametro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Parametro.pstrUsuarioConexion, Parametro.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Parametro.InfoSesion = DemeInfoSesion(Parametro.pstrUsuarioConexion, "InsertParametro")
            Me.DataContext.Parametros.InsertOnSubmit(Parametro)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertParametro")
        End Try
    End Sub

    Public Sub UpdateParametro(ByVal currentParametro As Parametro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentParametro.pstrUsuarioConexion, currentParametro.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentParametro.InfoSesion = DemeInfoSesion(currentParametro.pstrUsuarioConexion, "UpdateParametro")
            Me.DataContext.Parametros.Attach(currentParametro, Me.ChangeSet.GetOriginal(currentParametro))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateParametro")
        End Try
    End Sub

    Public Sub DeleteParametro(ByVal Parametro As Parametro)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Parametro.pstrUsuarioConexion, Parametro.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Parametros_Eliminar( parametro,  pParametro,  pValor, DemeInfoSesion(pstrUsuario, "DeleteParametro"),0).ToList
            Parametro.InfoSesion = DemeInfoSesion(Parametro.pstrUsuarioConexion, "DeleteParametro")
            Me.DataContext.Parametros.Attach(Parametro)
            Me.DataContext.Parametros.DeleteOnSubmit(Parametro)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteParametro")
        End Try
    End Sub
#End Region

#Region "Configuración Parámetros"
    'JEPM 20150710 Creada configuración de parámetros para que utilice los nuevos procedimientos almacenados de consultar y filtrar que incluye el campo Descripción

#Region "Métodos modelo para activar funcionalidad RIA"
    'JEPM 20150710 No utiliza nuevos métodos. En cambio utiliza los ya existentes de Parametros
#End Region

#Region "Métodos asincrónicos"

    Public Function FiltrarConfiguracionParametros(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Parametro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConfiguracionParametros_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "FiltrarConfiguracionParametros"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarConfiguracionParametros")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConfiguracionParametros(ByVal pIDparametro As Integer, ByVal pParametro As String, ByVal pValor As String, ByVal pDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Parametro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConfiguracionParametros_Consultar(pIDparametro, pParametro, pValor, pDescripcion, DemeInfoSesion(pstrUsuario, "BuscarParametros"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionParametros")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function FiltrarConfiguracionParametrosSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Parametro)
        Dim objTask As Task(Of List(Of Parametro)) = Me.FiltrarConfiguracionParametrosAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function FiltrarConfiguracionParametrosAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Parametro))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Parametro)) = New TaskCompletionSource(Of List(Of Parametro))()
        objTaskComplete.TrySetResult(FiltrarConfiguracionParametros(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConfiguracionParametrosSync(ByVal pIDparametro As Integer, ByVal pParametro As String, ByVal pValor As String, ByVal pDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Parametro)
        Dim objTask As Task(Of List(Of Parametro)) = Me.ConsultarConfiguracionParametrosAsync(pIDparametro, pParametro, pValor, pDescripcion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConfiguracionParametrosAsync(ByVal pIDparametro As Integer, ByVal pParametro As String, ByVal pValor As String, ByVal pDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Parametro))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Parametro)) = New TaskCompletionSource(Of List(Of Parametro))()
        objTaskComplete.TrySetResult(ConsultarConfiguracionParametros(pIDparametro, pParametro, pValor, pDescripcion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Paises"

    Public Function PaisesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Paise)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Paises_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "PaisesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PaisesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function PaisesConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Paise)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Paises_Consultar(pID, pNombre, DemeInfoSesion(pstrUsuario, "BuscarPaises"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarPaises")
            Return Nothing
        End Try
    End Function

    Public Function TraerPaisePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Paise
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Paise
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = -1
            'e.Nombre = 
            'e.Codigo_ISO = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDPais = 
            'e.CodigoDane = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerPaisePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertPaise(ByVal Paise As Paise)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Paise.pstrUsuarioConexion, Paise.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Paise.InfoSesion = DemeInfoSesion(Paise.pstrUsuarioConexion, "InsertPaise")
            Me.DataContext.Paises.InsertOnSubmit(Paise)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPaise")
        End Try
    End Sub

    Public Sub UpdatePaise(ByVal currentPaise As Paise)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentPaise.pstrUsuarioConexion, currentPaise.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentPaise.InfoSesion = DemeInfoSesion(currentPaise.pstrUsuarioConexion, "UpdatePaise")
            Me.DataContext.Paises.Attach(currentPaise, Me.ChangeSet.GetOriginal(currentPaise))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePaise")
        End Try
    End Sub

    Public Sub DeletePaise(ByVal Paise As Paise)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Paise.pstrUsuarioConexion, Paise.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Paises_Eliminar( pID,  pNombre, DemeInfoSesion(pstrUsuario, "DeletePaise"),0).ToList
            Paise.InfoSesion = DemeInfoSesion(Paise.pstrUsuarioConexion, "DeletePaise")
            Me.DataContext.Paises.Attach(Paise)
            Me.DataContext.Paises.DeleteOnSubmit(Paise)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePaise")
        End Try
    End Sub

    Public Function EliminarPaises(ByVal pID As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Paises_Eliminar(pID, mensaje, DemeInfoSesion(pstrUsuario, "EliminarPaises"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarPaises")
            Return Nothing
        End Try
    End Function

    Public Function Traer_Departamentos_Paise(ByVal pIDPais As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Departamento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pIDPais) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_DepartamentosPaise_Consultar(pIDPais, DemeInfoSesion(pstrUsuario, "Traer_Departamentos_Paise"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_Departamentos_Paise")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Departamentos"

    Public Function DepartamentosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Departamento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Departamentos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DepartamentosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DepartamentosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DepartamentosConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Departamento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Departamentos_Consultar(pID, pNombre, DemeInfoSesion(pstrUsuario, "BuscarDepartamentos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarDepartamentos")
            Return Nothing
        End Try
    End Function

    Public Function TraerDepartamentoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Departamento
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Departamento
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IDPais = 
            e.ID = -1
            'e.Nombre = 
            'e.Codigo_DaneDEPTO = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDDepartamento = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDepartamentoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDepartamento(ByVal Departamento As Departamento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Departamento.pstrUsuarioConexion, Departamento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Departamento.InfoSesion = DemeInfoSesion(Departamento.pstrUsuarioConexion, "InsertDepartamento")
            Me.DataContext.Departamentos.InsertOnSubmit(Departamento)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDepartamento")
        End Try
    End Sub

    Public Sub UpdateDepartamento(ByVal currentDepartamento As Departamento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentDepartamento.pstrUsuarioConexion, currentDepartamento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDepartamento.InfoSesion = DemeInfoSesion(currentDepartamento.pstrUsuarioConexion, "UpdateDepartamento")
            Me.DataContext.Departamentos.Attach(currentDepartamento, Me.ChangeSet.GetOriginal(currentDepartamento))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDepartamento")
        End Try
    End Sub

    Public Sub DeleteDepartamento(ByVal Departamento As Departamento)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Departamento.pstrUsuarioConexion, Departamento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Departamentos_Eliminar( pID,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteDepartamento"),0).ToList
            Departamento.InfoSesion = DemeInfoSesion(Departamento.pstrUsuarioConexion, "DeleteDepartamento")
            Me.DataContext.Departamentos.Attach(Departamento)
            Me.DataContext.Departamentos.DeleteOnSubmit(Departamento)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDepartamento")
        End Try
    End Sub
#End Region

#Region "Ciudades"

    Public Function CiudadesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Ciudade)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Ciudades_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CiudadesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CiudadesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CiudadesConsultar(ByVal pIDCodigo As Integer, ByVal pNombre As String, ByVal pCodigoDANE As String, ByVal pIDdepartamento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Ciudade)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Ciudades_Consultar(pIDCodigo, pNombre, pCodigoDANE, pIDdepartamento, DemeInfoSesion(pstrUsuario, "BuscarCiudades"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCiudades")
            Return Nothing
        End Try
    End Function

    Public Function TraerCiudadePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Ciudade
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Ciudade
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IDCodigo = 0
            'e.Nombre = 
            'e.EsCapital = 
            e.IDdepartamento = 0
            'e.CodigoDANE = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDCiudad = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCiudadePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCiudade(ByVal Ciudade As Ciudade)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Ciudade.pstrUsuarioConexion, Ciudade.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Ciudade.InfoSesion = DemeInfoSesion(Ciudade.pstrUsuarioConexion, "InsertCiudade")
            Me.DataContext.Ciudades.InsertOnSubmit(Ciudade)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCiudade")
        End Try
    End Sub

    Public Sub UpdateCiudade(ByVal currentCiudade As Ciudade)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCiudade.pstrUsuarioConexion, currentCiudade.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCiudade.InfoSesion = DemeInfoSesion(currentCiudade.pstrUsuarioConexion, "UpdateCiudade")
            Me.DataContext.Ciudades.Attach(currentCiudade, Me.ChangeSet.GetOriginal(currentCiudade))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCiudade")
        End Try
    End Sub

    Public Sub DeleteCiudade(ByVal Ciudade As Ciudade)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Ciudade.pstrUsuarioConexion, Ciudade.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Ciudades_Eliminar( pIDCodigo,  pNombre,  pCodigoDANE, DemeInfoSesion(pstrUsuario, "DeleteCiudade"),0).ToList
            Ciudade.InfoSesion = DemeInfoSesion(Ciudade.pstrUsuarioConexion, "DeleteCiudade")
            Me.DataContext.Ciudades.Attach(Ciudade)
            Me.DataContext.Ciudades.DeleteOnSubmit(Ciudade)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCiudade")
        End Try
    End Sub

    Public Function EliminarCiudad(ByVal IDCodigo As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Ciudades_Eliminar(IDCodigo, mensaje, DemeInfoSesion(pstrUsuario, "EliminarCiudad"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarCiudad")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Listas"

    Public Function ListasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Lista)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Listas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ListasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ListasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ListasConsultar(ByVal pTopico As String, ByVal pDescripcion As String, ByVal pRetorno As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Lista)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Listas_Consultar(pTopico, pDescripcion, pRetorno, DemeInfoSesion(pstrUsuario, "BuscarListas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarListas")
            Return Nothing
        End Try
    End Function

    Public Function TraerListaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Lista
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Lista
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Topico = 
            'e.Descripcion = 
            'e.Retorno = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.Activo = CBool(1)
            'e.IDLista = 
            'e.Comentario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerListaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertLista(ByVal Lista As Lista)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Lista.pstrUsuarioConexion, Lista.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Lista.InfoSesion = DemeInfoSesion(Lista.pstrUsuarioConexion, "InsertLista")
            Me.DataContext.Listas.InsertOnSubmit(Lista)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLista")
        End Try
    End Sub

    Public Sub UpdateLista(ByVal currentLista As Lista)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLista.pstrUsuarioConexion, currentLista.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentLista.InfoSesion = DemeInfoSesion(currentLista.pstrUsuarioConexion, "UpdateLista")
            Me.DataContext.Listas.Attach(currentLista, Me.ChangeSet.GetOriginal(currentLista))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLista")
        End Try
    End Sub

    Public Sub DeleteLista(ByVal Lista As Lista)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Lista.pstrUsuarioConexion, Lista.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Listas_Eliminar( pTopico,  pDescripcion,  pRetorno, DemeInfoSesion(pstrUsuario, "DeleteLista"),0).ToList
            Lista.InfoSesion = DemeInfoSesion(Lista.pstrUsuarioConexion, "DeleteLista")
            Me.DataContext.Listas.Attach(Lista)
            Me.DataContext.Listas.DeleteOnSubmit(Lista)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLista")
        End Try
    End Sub
#End Region

#Region "Configuracion Listas"
    Public Function ListaConfiguracionFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaConfiguracion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ListaConfiguracion_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ListaConfiguracionFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ListaConfiguracionFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ListaConfiguracionConsultar(ByVal pTopico As String, ByVal pDescripcion As String, ByVal pTipoDato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaConfiguracion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ListaConfiguracion_Consultar(pTopico, pDescripcion, pTipoDato, DemeInfoSesion(pstrUsuario, "BuscarListaConfiguracion"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarListaConfiguracion")
            Return Nothing
        End Try
    End Function

    'Public Function TraerListaConfiguracionPorDefecto( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS ListaConfiguracion
    '    Try
    '        Dim e As New ListaConfiguracion
    '        e.Topico = ""
    '        e.Descripcion = ""
    '        e.IDListaConfiguracion = 0
    '        e.Actualizacion = Now
    '        e.Usuario = HttpContext.Current.User.Identity.Name
    '        e.Modificable = CBool(1)
    '        e.TipoDato = ""
    '        Return e
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "TraerListaConfiguracionPorDefecto")
    '        Return Nothing
    '    End Try
    'End Function

    Public Sub InsertListaConfiguracion(ByVal Lista As ListaConfiguracion)

    End Sub

    Public Sub UpdateListaConfiguracion(ByVal currentLista As ListaConfiguracion)

    End Sub

    Public Sub DeleteListaConfiguracion(ByVal Lista As ListaConfiguracion)

    End Sub
#End Region

#Region "Validar Retornos"
#Region "Métodos modelo para activar funcionalidad RIA"
    'Public Sub InsertConfirmacion(ByVal Confirmacion As Confirmacion)

    'End Sub

    'Public Sub UpdateConfirmacion(ByVal currentConfirmacion As Confirmacion)

    'End Sub

    'Public Sub DeleteConfirmacion(ByVal Confirmacion As Confirmacion)

    'End Sub
#End Region
#Region "Metodos Sincronicos"
    Public Function ValidarRetornos(ByVal pstrTopico As String, ByVal pstrRetornos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Confirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConfiguracionListas_Validar_Retornos(pstrTopico, pstrRetornos, DemeInfoSesion(pstrUsuario, "ValidarRetornos"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarRetornos")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Metodos Sincronicos"
    Public Function ValidarRetornosSync(ByVal pstrTopico As String, ByVal pstrRetornos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Confirmacion)
        Dim objTask As Task(Of List(Of Confirmacion)) = Me.ValidarRetornosAsync(pstrTopico, pstrRetornos, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ValidarRetornosAsync(ByVal pstrTopico As String, pstrRetornos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Confirmacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Confirmacion)) = New TaskCompletionSource(Of List(Of Confirmacion))()
        objTaskComplete.TrySetResult(ValidarRetornos(pstrTopico, pstrRetornos, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
#End Region
#End Region

#Region "Validar Eliminar Registro"

    Public Function ValidarEliminarRegistro(ByVal pstrTablas As String, ByVal pstrCampos As String, ByVal pstrValorCampo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidacionEliminarRegistro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ValidarEliminarRegistro(pstrTablas, pstrCampos, pstrValorCampo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarEliminarRegistro"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarEliminarRegistro")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Validar Duplicidad Registro"

    Public Function ValidarDuplicidadRegistro(ByVal pstrTablas As String, ByVal pstrCampos As String, ByVal pstrValorCampo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidacionEliminarRegistro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ValidarDuplicidadRegistro(pstrTablas, pstrCampos, pstrValorCampo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarDuplicidadRegistro"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarDuplicidadRegistro")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Codigos_CIIU"

    Public Function Codigos_CIIUFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Codigos_CII)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Codigos_CIIU_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "Codigos_CIIUFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Codigos_CIIUFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function Codigos_CIIUConsultar(ByVal pCodigo As String, ByVal pDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Codigos_CII)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Codigos_CIIU_Consultar(pCodigo, pDescripcion, DemeInfoSesion(pstrUsuario, "BuscarCodigos_CIIU"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCodigos_CIIU")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCodigos_CII(ByVal Codigos_CII As Codigos_CII)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCodigos_CII")
        End Try
    End Sub

    Public Sub UpdateCodigos_CII(ByVal currentCodigos_CII As Codigos_CII)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCodigos_CII")
        End Try
    End Sub

    Public Sub DeleteCodigos_CII(ByVal Codigos_CII As Codigos_CII)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCodigos_CII")
        End Try
    End Sub
#End Region

#Region "TiposEntidad"

    Public Function TiposEntidadFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposEntida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TiposEntidad_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "TiposEntidadFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TiposEntidadFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TiposEntidadConsultar(ByVal pIDTipoEntidad As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposEntida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TiposEntidad_Consultar(pIDTipoEntidad, pNombre, DemeInfoSesion(pstrUsuario, "BuscarTiposEntidad"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarTiposEntidad")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTiposEntida(ByVal TiposEntida As TiposEntida)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTiposEntida")
        End Try
    End Sub

    Public Sub UpdateTiposEntida(ByVal currentTiposEntida As TiposEntida)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTiposEntida")
        End Try
    End Sub

    Public Sub DeleteTiposEntida(ByVal TiposEntida As TiposEntida)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTiposEntida")
        End Try
    End Sub
#End Region

#Region "Clasificaciones"

    Public Function ClasificacionesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Clasificacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Clasificaciones_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClasificacionesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClasificacionesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClasificacionesConsultar(ByVal pCódigo As Integer, ByVal pNombre As String, ByVal pstrAplicaA As String,
                                             ByVal plogEsGrupo As Boolean, ByVal plogEsSector As Boolean, ByVal plngIDPerteneceA As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Clasificacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Clasificaciones_Consultar(pCódigo, pNombre, pstrAplicaA, plogEsGrupo, plogEsSector, plngIDPerteneceA, DemeInfoSesion(pstrUsuario, "BuscarClasificaciones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClasificaciones")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClasificacion(ByVal Clasificacion As Clasificacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClasificacion")
        End Try
    End Sub

    Public Sub UpdateClasificacion(ByVal currentClasificacion As Clasificacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClasificacion")
        End Try
    End Sub

    Public Sub DeleteClasificacion(ByVal Clasificacion As Clasificacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClasificacion")
        End Try
    End Sub

#End Region

#Region "EstadosConceptoTitulos"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertEstadosConceptoTitulos(ByVal newEstadosConceptoTitulos As EstadosConceptoTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newEstadosConceptoTitulos.pstrUsuarioConexion, newEstadosConceptoTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newEstadosConceptoTitulos.strInfoSesion = DemeInfoSesion(newEstadosConceptoTitulos.pstrUsuarioConexion, "InsertEstadosConceptoTitulos")
            Me.DataContext.EstadosConceptoTitulos.InsertOnSubmit(newEstadosConceptoTitulos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEstadosConceptoTitulos")
        End Try
    End Sub

    Public Sub UpdateEstadosConceptoTitulos(ByVal currentEstadosConceptoTitulos As EstadosConceptoTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEstadosConceptoTitulos.pstrUsuarioConexion, currentEstadosConceptoTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEstadosConceptoTitulos.strInfoSesion = DemeInfoSesion(currentEstadosConceptoTitulos.pstrUsuarioConexion, "UpdateEstadosConceptoTitulos")
            Me.DataContext.EstadosConceptoTitulos.Attach(currentEstadosConceptoTitulos, Me.ChangeSet.GetOriginal(currentEstadosConceptoTitulos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEstadosConceptoTitulos")
        End Try
    End Sub

    Public Sub DeleteEstadosConceptoTitulos(ByVal deleteEstadosConceptoTitulos As EstadosConceptoTitulos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteEstadosConceptoTitulos.pstrUsuarioConexion, deleteEstadosConceptoTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteEstadosConceptoTitulos.strInfoSesion = DemeInfoSesion(deleteEstadosConceptoTitulos.pstrUsuarioConexion, "DeleteEstadosConceptoTitulos")
            Me.DataContext.EstadosConceptoTitulos.Attach(deleteEstadosConceptoTitulos)
            Me.DataContext.EstadosConceptoTitulos.DeleteOnSubmit(deleteEstadosConceptoTitulos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEstadosConceptoTitulos")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarEstadosConceptoTitulos(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadosConceptoTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_EstadosConceptoTitulos_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarEstadosConceptoTitulos"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarEstadosConceptoTitulos")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEstadosConceptoTitulos(pstrConceptoTitulo As String, pstrEstadoEntrada As String, pstrEstadoSalida As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadosConceptoTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_EstadosConceptoTitulos_Consultar(String.Empty, pstrConceptoTitulo, pstrEstadoEntrada, pstrEstadoSalida, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEstadosConceptoTitulos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEstadosConceptoTitulos")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEstadosConceptoTitulosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EstadosConceptoTitulos
        Dim objEstadosConceptoTitulos As EstadosConceptoTitulos = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_EstadosConceptoTitulos_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEstadosConceptoTitulosPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objEstadosConceptoTitulos = ret.FirstOrDefault
            End If
            Return objEstadosConceptoTitulos
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEstadosConceptoTitulosPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarEstadosConceptoTitulosSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadosConceptoTitulos)
        Dim objTask As Task(Of List(Of EstadosConceptoTitulos)) = Me.FiltrarEstadosConceptoTitulosAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarEstadosConceptoTitulosAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EstadosConceptoTitulos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EstadosConceptoTitulos)) = New TaskCompletionSource(Of List(Of EstadosConceptoTitulos))()
        objTaskComplete.TrySetResult(FiltrarEstadosConceptoTitulos(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEstadosConceptoTitulosSync(pstrConceptoTitulo As String, pstrEstadoEntrada As String, pstrEstadoSalida As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadosConceptoTitulos)
        Dim objTask As Task(Of List(Of EstadosConceptoTitulos)) = Me.ConsultarEstadosConceptoTitulosAsync(pstrConceptoTitulo, pstrEstadoEntrada, pstrEstadoSalida, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEstadosConceptoTitulosAsync(pstrConceptoTitulo As String, pstrEstadoEntrada As String, pstrEstadoSalida As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EstadosConceptoTitulos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EstadosConceptoTitulos)) = New TaskCompletionSource(Of List(Of EstadosConceptoTitulos))()
        objTaskComplete.TrySetResult(ConsultarEstadosConceptoTitulos(pstrConceptoTitulo, pstrEstadoEntrada, pstrEstadoSalida, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEstadosConceptoTitulosPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EstadosConceptoTitulos
        Dim objTask As Task(Of EstadosConceptoTitulos) = Me.ConsultarEstadosConceptoTitulosPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEstadosConceptoTitulosPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of EstadosConceptoTitulos)
        Dim objTaskComplete As TaskCompletionSource(Of EstadosConceptoTitulos) = New TaskCompletionSource(Of EstadosConceptoTitulos)()
        objTaskComplete.TrySetResult(ConsultarEstadosConceptoTitulosPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "EntidadesCuentasDeposito"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertEntidadesCuentasDeposito(ByVal newEntidadesCuentasDeposito As EntidadesCuentasDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newEntidadesCuentasDeposito.pstrUsuarioConexion, newEntidadesCuentasDeposito.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newEntidadesCuentasDeposito.strInfoSesion = DemeInfoSesion(newEntidadesCuentasDeposito.pstrUsuarioConexion, "InsertEntidadesCuentasDeposito")
            Me.DataContext.EntidadesCuentasDeposito.InsertOnSubmit(newEntidadesCuentasDeposito)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEntidadesCuentasDeposito")
        End Try
    End Sub

    Public Sub UpdateEntidadesCuentasDeposito(ByVal currentEntidadesCuentasDeposito As EntidadesCuentasDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEntidadesCuentasDeposito.pstrUsuarioConexion, currentEntidadesCuentasDeposito.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEntidadesCuentasDeposito.strInfoSesion = DemeInfoSesion(currentEntidadesCuentasDeposito.pstrUsuarioConexion, "UpdateEntidadesCuentasDeposito")
            Me.DataContext.EntidadesCuentasDeposito.Attach(currentEntidadesCuentasDeposito, Me.ChangeSet.GetOriginal(currentEntidadesCuentasDeposito))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEntidadesCuentasDeposito")
        End Try
    End Sub

    Public Sub DeleteEntidadesCuentasDeposito(ByVal deleteEntidadesCuentasDeposito As EntidadesCuentasDeposito)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteEntidadesCuentasDeposito.pstrUsuarioConexion, deleteEntidadesCuentasDeposito.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteEntidadesCuentasDeposito.strInfoSesion = DemeInfoSesion(deleteEntidadesCuentasDeposito.pstrUsuarioConexion, "DeleteEntidadesCuentasDeposito")
            Me.DataContext.EntidadesCuentasDeposito.Attach(deleteEntidadesCuentasDeposito)
            Me.DataContext.EntidadesCuentasDeposito.DeleteOnSubmit(deleteEntidadesCuentasDeposito)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEntidadesCuentasDeposito")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarEntidadesCuentasDeposito(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EntidadesCuentasDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Entidades_CuentasDeposito_Filtar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarEntidadesCuentasDeposito"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarEntidadesCuentasDeposito")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEntidadesCuentasDeposito(pintIDEntidad As Nullable(Of Integer), pstrDeposito As String, pstrCuentaDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EntidadesCuentasDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Entidades_CuentasDeposito_Consultar(True, String.Empty, pintIDEntidad, pstrDeposito, pstrCuentaDeposito, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEntidadesCuentasDeposito"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEntidadesCuentasDeposito")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEntidadesCuentasDepositoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EntidadesCuentasDeposito
        Dim objEntidadesCuentasDeposito As EntidadesCuentasDeposito = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Entidades_CuentasDeposito_Consultar(True, Constantes.CONSULTAR_DATOS_POR_DEFECTO, Nothing, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEntidadesCuentasDepositoPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objEntidadesCuentasDeposito = ret.FirstOrDefault
            End If
            Return objEntidadesCuentasDeposito
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEntidadesCuentasDepositoPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarEntidadesCuentasDepositoSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EntidadesCuentasDeposito)
        Dim objTask As Task(Of List(Of EntidadesCuentasDeposito)) = Me.FiltrarEntidadesCuentasDepositoAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarEntidadesCuentasDepositoAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EntidadesCuentasDeposito))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EntidadesCuentasDeposito)) = New TaskCompletionSource(Of List(Of EntidadesCuentasDeposito))()
        objTaskComplete.TrySetResult(FiltrarEntidadesCuentasDeposito(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEntidadesCuentasDepositoSync(pintIDEntidad As Nullable(Of Integer), pstrDeposito As String, pstrCuentaDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EntidadesCuentasDeposito)
        Dim objTask As Task(Of List(Of EntidadesCuentasDeposito)) = Me.ConsultarEntidadesCuentasDepositoAsync(pintIDEntidad, pstrDeposito, pstrCuentaDeposito, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEntidadesCuentasDepositoAsync(pintIDEntidad As Nullable(Of Integer), pstrDeposito As String, pstrCuentaDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EntidadesCuentasDeposito))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EntidadesCuentasDeposito)) = New TaskCompletionSource(Of List(Of EntidadesCuentasDeposito))()
        objTaskComplete.TrySetResult(ConsultarEntidadesCuentasDeposito(pintIDEntidad, pstrDeposito, pstrCuentaDeposito, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEntidadesCuentasDepositoPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EntidadesCuentasDeposito
        Dim objTask As Task(Of EntidadesCuentasDeposito) = Me.ConsultarEntidadesCuentasDepositoPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEntidadesCuentasDepositoPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of EntidadesCuentasDeposito)
        Dim objTaskComplete As TaskCompletionSource(Of EntidadesCuentasDeposito) = New TaskCompletionSource(Of EntidadesCuentasDeposito)()
        objTaskComplete.TrySetResult(ConsultarEntidadesCuentasDepositoPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "EstadosBloqueoTitulos"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertEstadosBloqueoTitulos(ByVal newEstadosBloqueoTitulos As EstadosBloqueoTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newEstadosBloqueoTitulos.pstrUsuarioConexion, newEstadosBloqueoTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newEstadosBloqueoTitulos.strInfoSesion = DemeInfoSesion(newEstadosBloqueoTitulos.pstrUsuarioConexion, "InsertEstadosBloqueoTitulos")
            Me.DataContext.EstadosBloqueoTitulos.InsertOnSubmit(newEstadosBloqueoTitulos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEstadosBloqueoTitulos")
        End Try
    End Sub

    Public Sub UpdateEstadosBloqueoTitulos(ByVal currentEstadosBloqueoTitulos As EstadosBloqueoTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEstadosBloqueoTitulos.pstrUsuarioConexion, currentEstadosBloqueoTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEstadosBloqueoTitulos.strInfoSesion = DemeInfoSesion(currentEstadosBloqueoTitulos.pstrUsuarioConexion, "UpdateEstadosBloqueoTitulos")
            Me.DataContext.EstadosBloqueoTitulos.Attach(currentEstadosBloqueoTitulos, Me.ChangeSet.GetOriginal(currentEstadosBloqueoTitulos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEstadosBloqueoTitulos")
        End Try
    End Sub

    Public Sub DeleteEstadosBloqueoTitulos(ByVal deleteEstadosBloqueoTitulos As EstadosBloqueoTitulos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteEstadosBloqueoTitulos.pstrUsuarioConexion, deleteEstadosBloqueoTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteEstadosBloqueoTitulos.strInfoSesion = DemeInfoSesion(deleteEstadosBloqueoTitulos.pstrUsuarioConexion, "DeleteEstadosBloqueoTitulos")
            Me.DataContext.EstadosBloqueoTitulos.Attach(deleteEstadosBloqueoTitulos)
            Me.DataContext.EstadosBloqueoTitulos.DeleteOnSubmit(deleteEstadosBloqueoTitulos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEstadosBloqueoTitulos")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarEstadosBloqueoTitulos(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadosBloqueoTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EstadosBloqueoTitulos_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarEstadosBloqueoTitulos"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarEstadosBloqueoTitulos")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEstadosBloqueoTitulos(pstrEstadoBloqueo As String, pstrEstadoDesBloqueo As String, pstrMecanismo As String, pstrMotivoBloqueo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadosBloqueoTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EstadosBloqueoTitulos_Consultar(String.Empty, pstrEstadoBloqueo, pstrEstadoDesBloqueo, pstrMecanismo, pstrMotivoBloqueo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEstadosBloqueoTitulos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEstadosBloqueoTitulos")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEstadosBloqueoTitulosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EstadosBloqueoTitulos
        Dim objEstadosBloqueoTitulos As EstadosBloqueoTitulos = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EstadosBloqueoTitulos_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEstadosBloqueoTitulosPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objEstadosBloqueoTitulos = ret.FirstOrDefault
            End If
            Return objEstadosBloqueoTitulos
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEstadosBloqueoTitulosPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarEstadosBloqueoTitulosSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadosBloqueoTitulos)
        Dim objTask As Task(Of List(Of EstadosBloqueoTitulos)) = Me.FiltrarEstadosBloqueoTitulosAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarEstadosBloqueoTitulosAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EstadosBloqueoTitulos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EstadosBloqueoTitulos)) = New TaskCompletionSource(Of List(Of EstadosBloqueoTitulos))()
        objTaskComplete.TrySetResult(FiltrarEstadosBloqueoTitulos(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEstadosBloqueoTitulosSync(pstrEstadoBloqueo As String, pstrEstadoDesBloqueo As String, pstrMecanismo As String, pstrMotivoBloqueo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadosBloqueoTitulos)
        Dim objTask As Task(Of List(Of EstadosBloqueoTitulos)) = Me.ConsultarEstadosBloqueoTitulosAsync(pstrEstadoBloqueo, pstrEstadoDesBloqueo, pstrMecanismo, pstrMotivoBloqueo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEstadosBloqueoTitulosAsync(pstrEstadoBloqueo As String, pstrEstadoDesBloqueo As String, pstrMecanismo As String, pstrMotivoBloqueo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EstadosBloqueoTitulos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EstadosBloqueoTitulos)) = New TaskCompletionSource(Of List(Of EstadosBloqueoTitulos))()
        objTaskComplete.TrySetResult(ConsultarEstadosBloqueoTitulos(pstrEstadoBloqueo, pstrEstadoDesBloqueo, pstrMecanismo, pstrMotivoBloqueo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEstadosBloqueoTitulosPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EstadosBloqueoTitulos
        Dim objTask As Task(Of EstadosBloqueoTitulos) = Me.ConsultarEstadosBloqueoTitulosPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEstadosBloqueoTitulosPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of EstadosBloqueoTitulos)
        Dim objTaskComplete As TaskCompletionSource(Of EstadosBloqueoTitulos) = New TaskCompletionSource(Of EstadosBloqueoTitulos)()
        objTaskComplete.TrySetResult(ConsultarEstadosBloqueoTitulosPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "maestro Clase Contable Titulo"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertClaseContableTitulo(ByVal objClaseContableTitulo As tblClaseContableTitulo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objClaseContableTitulo.pstrUsuarioConexion, objClaseContableTitulo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objClaseContableTitulo.strInfoSesion = DemeInfoSesion(objClaseContableTitulo.pstrUsuarioConexion, "InsertClaseContableTitulo")
            Me.DataContext.tblClaseContableTitulo.InsertOnSubmit(objClaseContableTitulo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClaseContableTitulo")
        End Try
    End Sub
    Public Sub UpdateClaseContableTitulo(ByVal currentClaseContableTitulo As tblClaseContableTitulo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentClaseContableTitulo.pstrUsuarioConexion, currentClaseContableTitulo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClaseContableTitulo.strInfoSesion = DemeInfoSesion(currentClaseContableTitulo.pstrUsuarioConexion, "UpdateClaseContableTitulo")
            Me.DataContext.tblClaseContableTitulo.Attach(currentClaseContableTitulo, Me.ChangeSet.GetOriginal(currentClaseContableTitulo))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClaseContableTitulo")
        End Try
    End Sub
    Public Sub DeleteClaseContableTitulo(ByVal objClaseContableTitulo As tblClaseContableTitulo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objClaseContableTitulo.pstrUsuarioConexion, objClaseContableTitulo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objClaseContableTitulo.strInfoSesion = DemeInfoSesion(objClaseContableTitulo.pstrUsuarioConexion, "DeleteClaseContableTitulo")
            Me.DataContext.tblClaseContableTitulo.Attach(objClaseContableTitulo)
            Me.DataContext.tblClaseContableTitulo.DeleteOnSubmit(objClaseContableTitulo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClaseContableTitulo")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarClaseContableTitulo(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblClaseContableTitulo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ClaseContableTitulo_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarClaseContableTitulo"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarClaseContableTitulo")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarClaseContableTitulo(pstrTipoTitulo As String, pstrReferencia As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblClaseContableTitulo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ClaseContableTitulo_Consultar(String.Empty, pstrTipoTitulo, pstrReferencia, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarClaseContableTitulo"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarClaseContableTitulo")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarClaseContableTituloPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblClaseContableTitulo
        Dim objClaseContableTitulo As tblClaseContableTitulo = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ClaseContableTitulo_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, "", "", pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarClaseContableTitulo"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objClaseContableTitulo = ret.FirstOrDefault
            End If
            Return objClaseContableTitulo
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarClaseContableTituloPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarClaseContableTituloSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblClaseContableTitulo)
        Dim objTask As Task(Of List(Of tblClaseContableTitulo)) = Me.FiltrarClaseContableTituloAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function FiltrarClaseContableTituloAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tblClaseContableTitulo))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblClaseContableTitulo)) = New TaskCompletionSource(Of List(Of tblClaseContableTitulo))()
        objTaskComplete.TrySetResult(FiltrarClaseContableTitulo(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarClaseContableTituloSync(pstrTipoTitulo As String, pstrReferencia As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblClaseContableTitulo)
        Dim objTask As Task(Of List(Of tblClaseContableTitulo)) = Me.ConsultarClaseContableTituloAsync(pstrTipoTitulo, pstrReferencia, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarClaseContableTituloAsync(pstrTipoTitulo As String, pstrReferencia As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tblClaseContableTitulo))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblClaseContableTitulo)) = New TaskCompletionSource(Of List(Of tblClaseContableTitulo))()
        objTaskComplete.TrySetResult(ConsultarClaseContableTitulo(pstrTipoTitulo, pstrReferencia, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarClaseContableTituloPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblClaseContableTitulo
        Dim objTask As Task(Of tblClaseContableTitulo) = Me.ConsultarClaseContableTituloPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarClaseContableTituloPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of tblClaseContableTitulo)
        Dim objTaskComplete As TaskCompletionSource(Of tblClaseContableTitulo) = New TaskCompletionSource(Of tblClaseContableTitulo)()
        objTaskComplete.TrySetResult(ConsultarClaseContableTituloPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region


#End Region

#Region "EspeciesExcluidasEncabezado"

    'JEPM20160119

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertEspeciesExcluidasEncabezado(ByVal objEspeciesExcluidasEncabezado As EspeciesExcluidasEncabezado)

    End Sub

    Public Sub UpdateEspeciesExcluidasEncabezado(ByVal currentEspeciesExcluidasEncabezado As EspeciesExcluidasEncabezado)

    End Sub

    Public Sub DeleteEspeciesExcluidasEncabezado(ByVal objEspeciesExcluidasEncabezado As EspeciesExcluidasEncabezado)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function FiltrarEspeciesExcluidasEncabezado(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesExcluidasEncabezado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EspeciesExcluidas_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarEspeciesExcluidasEncabezado"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarEspeciesExcluidasEncabezado")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEspeciesExcluidasEncabezado(ByVal pstrFormato As String, ByVal pstrIDEspecie As String, ByVal pstrExclusionFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesExcluidasEncabezado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EspeciesExcluidas_Consultar(String.Empty, pstrFormato, pstrIDEspecie, pstrExclusionFormato, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEspeciesExcluidasEncabezado"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesExcluidasEncabezado")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEspeciesExcluidasEncabezadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesExcluidasEncabezado
        Dim objEspeciesExcluidasEncabezado As EspeciesExcluidasEncabezado = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EspeciesExcluidas_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEspeciesExcluidasEncabezadoPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objEspeciesExcluidasEncabezado = ret.FirstOrDefault
            End If
            Return objEspeciesExcluidasEncabezado
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesExcluidasEncabezadoPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar la Especie excluida. Aunque realmente inserta o modifica únicamente los detalles.
    ''' </summary>
    Public Function ActualizarEspeciesExcluidasEncabezado(ByVal pstrFormato As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_EspeciesExcluidas_Actualizar(pstrFormato, pxmlDetalleGrid, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarEspeciesExcluidasEncabezado"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarEspeciesExcluidasEncabezado")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function FiltrarEspeciesExcluidasEncabezadoSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesExcluidasEncabezado)
        Dim objTask As Task(Of List(Of EspeciesExcluidasEncabezado)) = Me.FiltrarEspeciesExcluidasEncabezadoAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarEspeciesExcluidasEncabezadoAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EspeciesExcluidasEncabezado))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EspeciesExcluidasEncabezado)) = New TaskCompletionSource(Of List(Of EspeciesExcluidasEncabezado))()
        objTaskComplete.TrySetResult(FiltrarEspeciesExcluidasEncabezado(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEspeciesExcluidasEncabezadoSync(ByVal pstrFormato As String, ByVal pstrIDEspecie As String, ByVal pstrExclusionFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesExcluidasEncabezado)
        Dim objTask As Task(Of List(Of EspeciesExcluidasEncabezado)) = Me.ConsultarEspeciesExcluidasEncabezadoAsync(pstrFormato, pstrIDEspecie, pstrExclusionFormato, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEspeciesExcluidasEncabezadoAsync(ByVal pstrFormato As String, ByVal pstrIDEspecie As String, ByVal pstrExclusionFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EspeciesExcluidasEncabezado))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EspeciesExcluidasEncabezado)) = New TaskCompletionSource(Of List(Of EspeciesExcluidasEncabezado))()
        objTaskComplete.TrySetResult(ConsultarEspeciesExcluidasEncabezado(pstrFormato, pstrIDEspecie, pstrExclusionFormato, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEspeciesExcluidasEncabezadoPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesExcluidasEncabezado
        Dim objTask As Task(Of EspeciesExcluidasEncabezado) = Me.ConsultarEspeciesExcluidasEncabezadoPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEspeciesExcluidasEncabezadoPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of EspeciesExcluidasEncabezado)
        Dim objTaskComplete As TaskCompletionSource(Of EspeciesExcluidasEncabezado) = New TaskCompletionSource(Of EspeciesExcluidasEncabezado)()
        objTaskComplete.TrySetResult(ConsultarEspeciesExcluidasEncabezadoPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Función para realizar llamado sincrónico al método ActualizarEspeciesExcluidasEncabezadoAsync y controlar los mensajes de salida
    ''' </summary>
    Public Function ActualizarEspeciesExcluidasEncabezadoSync(ByVal pstrFormato As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarEspeciesExcluidasEncabezadoAsync(pstrFormato, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    ''' <summary>
    ''' Función para realizar el llamado al proceso ActualizarEspeciesExcluidasEncabezado
    ''' </summary>
    Private Function ActualizarEspeciesExcluidasEncabezadoAsync(ByVal pstrFormato As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarEspeciesExcluidasEncabezado(pstrFormato, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "EspeciesExcluidasDetalles"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertEspeciesExcluidasDetalles(ByVal objEspeciesExcluidasDetalles As EspeciesExcluidasDetalles)

    End Sub

    Public Sub UpdateEspeciesExcluidasDetalles(ByVal currentEspeciesExcluidasDetalles As EspeciesExcluidasDetalles)

    End Sub

    Public Sub DeleteEspeciesExcluidasDetalles(ByVal UpdateEspeciesExcluidasDetalles As EspeciesExcluidasDetalles)

    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarEspeciesExcluidasDetalles(pstrFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesExcluidasDetalles)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EspeciesExcluidas_Detalles_Consultar(String.Empty, pstrFormato, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContable"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesExcluidasDetalles")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEspeciesExcluidasDetallesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesExcluidasDetalles
        Dim objEspeciesExcluidasDetalles As EspeciesExcluidasDetalles = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EspeciesExcluidas_Detalles_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, "", pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContablePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objEspeciesExcluidasDetalles = ret.FirstOrDefault
            End If
            Return objEspeciesExcluidasDetalles
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesExcluidasDetallesPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarEspeciesExcluidasDetallesSync(pstrFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesExcluidasDetalles)
        Dim objTask As Task(Of List(Of EspeciesExcluidasDetalles)) = Me.ConsultarEspeciesExcluidasDetallesAsync(pstrFormato, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEspeciesExcluidasDetallesAsync(pstrFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EspeciesExcluidasDetalles))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EspeciesExcluidasDetalles)) = New TaskCompletionSource(Of List(Of EspeciesExcluidasDetalles))()
        objTaskComplete.TrySetResult(ConsultarEspeciesExcluidasDetalles(pstrFormato, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEspeciesExcluidasDetallesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesExcluidasDetalles
        Dim objTask As Task(Of EspeciesExcluidasDetalles) = Me.ConsultarEspeciesExcluidasDetallesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEspeciesExcluidasDetallesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of EspeciesExcluidasDetalles)
        Dim objTaskComplete As TaskCompletionSource(Of EspeciesExcluidasDetalles) = New TaskCompletionSource(Of EspeciesExcluidasDetalles)()
        objTaskComplete.TrySetResult(ConsultarEspeciesExcluidasDetallesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ConfiguracionContableMultimoneda"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertConfiguracionContableMultimoneda(ByVal obj As ConfiguracionContableMultimoneda)
    End Sub

    Public Sub UpdateConfiguracionContableMultimoneda(ByVal obj As ConfiguracionContableMultimoneda)
    End Sub

    Public Sub DeleteConfiguracionContableMultimoneda(ByVal obj As ConfiguracionContableMultimoneda)
    End Sub
#End Region


#Region "Métodos asincrónicos"

    Public Function ConsultarConfiguracionContableMultimoneda(ByVal pintIdEncabezado As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableMultimoneda)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ClaseContableTitulo_Detalle_Consultar(String.Empty, pintIdEncabezado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionContableMultimoneda"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionContableMultimoneda")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConfiguracionContableMultimonedaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionContableMultimoneda
        Dim objConfiguracionContableMultimoneda As ConfiguracionContableMultimoneda = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ClaseContableTitulo_Detalle_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionContableMultimonedaPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objConfiguracionContableMultimoneda = ret.FirstOrDefault
            End If
            Return objConfiguracionContableMultimoneda
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionContableMultimonedaPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarConfiguracionContableMultimonedaSync(pintIdEncabezado As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableMultimoneda)
        Dim objTask As Task(Of List(Of ConfiguracionContableMultimoneda)) = Me.ConsultarConfiguracionContableMultimonedaAsync(pintIdEncabezado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConfiguracionContableMultimonedaAsync(pintIdEncabezado As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionContableMultimoneda))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionContableMultimoneda)) = New TaskCompletionSource(Of List(Of ConfiguracionContableMultimoneda))()
        objTaskComplete.TrySetResult(ConsultarConfiguracionContableMultimoneda(pintIdEncabezado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConfiguracionContableMultimonedaPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionContableMultimoneda
        Dim objTask As Task(Of ConfiguracionContableMultimoneda) = Me.ConsultarConfiguracionContableMultimonedaPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConfiguracionContableMultimonedaPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ConfiguracionContableMultimoneda)
        Dim objTaskComplete As TaskCompletionSource(Of ConfiguracionContableMultimoneda) = New TaskCompletionSource(Of ConfiguracionContableMultimoneda)()
        objTaskComplete.TrySetResult(ConsultarConfiguracionContableMultimonedaPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region


#End Region

#Region "CuentasFondos"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertCuentasFondos(ByVal newCuentasFondos As CuentasFondos)
        Try
            newCuentasFondos.strInfoSesion = DemeInfoSesion(newCuentasFondos.pstrUsuarioConexion, "InsertCuentasFondos")
            Me.DataContext.CuentasFondos.InsertOnSubmit(newCuentasFondos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCuentasFondos")
        End Try
    End Sub

    Public Sub UpdateCuentasFondos(ByVal currentCuentasFondos As CuentasFondos)
        Try
            currentCuentasFondos.strInfoSesion = DemeInfoSesion(currentCuentasFondos.pstrUsuarioConexion, "UpdateCuentasFondos")
            Me.DataContext.CuentasFondos.Attach(currentCuentasFondos, Me.ChangeSet.GetOriginal(currentCuentasFondos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentasFondos")
        End Try
    End Sub

    Public Sub DeleteCuentasFondos(ByVal deleteCuentasFondos As CuentasFondos)
        Try
            deleteCuentasFondos.strInfoSesion = DemeInfoSesion(deleteCuentasFondos.pstrUsuarioConexion, "DeleteCuentasFondos")
            Me.DataContext.CuentasFondos.Attach(deleteCuentasFondos)
            Me.DataContext.CuentasFondos.DeleteOnSubmit(deleteCuentasFondos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCuentasFondos")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function CuentasFondos_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CuentasFondos)
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_CuentasFondos_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "CuentasFondos_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasFondos_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function CuentasFondos_Consultar(ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal plngIDComitente As String, ByVal pstrTipoIdComitente As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrPrefijo As String, ByVal pstrUsuario As String) As List(Of CuentasFondos)
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_CuentasFondos_Consultar(String.Empty, plngidCuentaDeceval, pstrDeposito, plngIDComitente, pstrTipoIdComitente, pstrNroDocumento, pstrNombre, pstrPrefijo, pstrUsuario, DemeInfoSesion(pstrUsuario, "CuentasFondos_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasFondos_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function CuentasFondos_ConsultarPorDefecto(ByVal pstrUsuario As String) As CuentasFondos
        Dim objCuentasFondos As CuentasFondos = Nothing
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_CuentasFondos_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "CuentasFondos_ConsultarPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCuentasFondos = ret.FirstOrDefault
            End If
            Return objCuentasFondos
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasFondos_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar el encabezado y detalle
    ''' </summary>
    Public Function CuentasFondos_Actualizar(ByVal pintIDCuentasFondos As System.Nullable(Of Integer), ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal plngIDComitente As String, ByVal pstrTipoIdComitente As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrPrefijo As String, ByVal pxmlDetalleGrid As String, ByVal pstrConector1 As String, ByVal pstrTipoIdBenef1 As String, ByVal plngNroDocBenef1 As String, ByVal pstrConector2 As String, ByVal pstrTipoIdBenef2 As String, ByVal plngNroDocBenef2 As String, ByVal pstrUsuario As String, ByVal pstrCuentaPrincipalDCV As String) As String
        Try
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_CuentasFondos_Actualizar(pintIDCuentasFondos, plngidCuentaDeceval, pstrDeposito, plngIDComitente, pstrTipoIdComitente, pstrNroDocumento, pstrNombre, pstrPrefijo, pxmlDetalleGrid, pstrConector1, pstrTipoIdBenef1, plngNroDocBenef1, pstrConector2, pstrTipoIdBenef2, plngNroDocBenef2, pstrUsuario, DemeInfoSesion(pstrUsuario, "CuentasFondos_Actualizar"), 0, pstrMsgValidacion, pstrCuentaPrincipalDCV)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasFondos_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function CuentasFondos_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CuentasFondos)
        Dim objTask As Task(Of List(Of CuentasFondos)) = Me.CuentasFondos_FiltrarAsync(pstrFiltro, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CuentasFondos_FiltrarAsync(ByVal pstrFiltro As String, pstrUsuario As String) As Task(Of List(Of CuentasFondos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CuentasFondos)) = New TaskCompletionSource(Of List(Of CuentasFondos))()
        objTaskComplete.TrySetResult(CuentasFondos_Filtrar(pstrFiltro, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function CuentasFondos_ConsultarSync(ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal plngIDComitente As String, ByVal pstrTipoIdComitente As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrPrefijo As String, ByVal pstrUsuario As String) As List(Of CuentasFondos)
        Dim objTask As Task(Of List(Of CuentasFondos)) = Me.CuentasFondos_ConsultarAsync(plngidCuentaDeceval, pstrDeposito, plngIDComitente, pstrTipoIdComitente, pstrNroDocumento, pstrNombre, pstrPrefijo, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CuentasFondos_ConsultarAsync(ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal plngIDComitente As String, ByVal pstrTipoIdComitente As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrPrefijo As String, ByVal pstrUsuario As String) As Task(Of List(Of CuentasFondos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CuentasFondos)) = New TaskCompletionSource(Of List(Of CuentasFondos))()
        objTaskComplete.TrySetResult(CuentasFondos_Consultar(plngidCuentaDeceval, pstrDeposito, plngIDComitente, pstrTipoIdComitente, pstrNroDocumento, pstrNombre, pstrPrefijo, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function CuentasFondos_ConsultarPorDefectoSync(ByVal pstrUsuario As String) As CuentasFondos
        Dim objTask As Task(Of CuentasFondos) = Me.CuentasFondos_ConsultarPorDefectoAsync(pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CuentasFondos_ConsultarPorDefectoAsync(pstrUsuario As String) As Task(Of CuentasFondos)
        Dim objTaskComplete As TaskCompletionSource(Of CuentasFondos) = New TaskCompletionSource(Of CuentasFondos)()
        objTaskComplete.TrySetResult(CuentasFondos_ConsultarPorDefecto(pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function CuentasFondos_ActualizarSync(pintIDCuentasFondos As System.Nullable(Of Integer), ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal plngIDComitente As String, ByVal pstrTipoIdComitente As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrPrefijo As String, ByVal pxmlDetalleGrid As String, ByVal pstrConector1 As String, ByVal pstrTipoIdBenef1 As String, ByVal plngNroDocBenef1 As String, ByVal pstrConector2 As String, ByVal pstrTipoIdBenef2 As String, ByVal plngNroDocBenef2 As String, ByVal pstrUsuario As String, ByVal pstrCuentaPrincipalDCV As String) As String
        Dim objTask As Task(Of String) = Me.CuentasFondos_ActualizarAsync(pintIDCuentasFondos, plngidCuentaDeceval, pstrDeposito, plngIDComitente, pstrTipoIdComitente, pstrNroDocumento, pstrNombre, pstrPrefijo, pxmlDetalleGrid, pstrConector1, pstrTipoIdBenef1, plngNroDocBenef1, pstrConector2, pstrTipoIdBenef2, plngNroDocBenef2, pstrUsuario, pstrCuentaPrincipalDCV)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CuentasFondos_ActualizarAsync(pintIDCuentasFondos As System.Nullable(Of Integer), ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal plngIDComitente As String, ByVal pstrTipoIdComitente As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrPrefijo As String, ByVal pxmlDetalleGrid As String, ByVal pstrConector1 As String, ByVal pstrTipoIdBenef1 As String, ByVal plngNroDocBenef1 As String, ByVal pstrConector2 As String, ByVal pstrTipoIdBenef2 As String, ByVal plngNroDocBenef2 As String, ByVal pstrUsuario As String, ByVal pstrCuentaPrincipalDCV As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(CuentasFondos_Actualizar(pintIDCuentasFondos, plngidCuentaDeceval, pstrDeposito, plngIDComitente, pstrTipoIdComitente, pstrNroDocumento, pstrNombre, pstrPrefijo, pxmlDetalleGrid, pstrConector1, pstrTipoIdBenef1, plngNroDocBenef1, pstrConector2, pstrTipoIdBenef2, plngNroDocBenef2, pstrUsuario, pstrCuentaPrincipalDCV))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CuentasFondosDetalle"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertCuentasFondosDetalle(ByVal newCuentasFondosDetalle As CuentasFondosDetalle)
        Try
            newCuentasFondosDetalle.strInfoSesion = DemeInfoSesion(newCuentasFondosDetalle.pstrUsuarioConexion, "InsertCuentasFondosDetalle")
            Me.DataContext.CuentasFondosDetalle.InsertOnSubmit(newCuentasFondosDetalle)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCuentasFondosDetalle")
        End Try
    End Sub

    Public Sub UpdateCuentasFondosDetalle(ByVal currentCuentasFondosDetalle As CuentasFondosDetalle)
        Try
            currentCuentasFondosDetalle.strInfoSesion = DemeInfoSesion(currentCuentasFondosDetalle.pstrUsuarioConexion, "UpdateCuentasFondosDetalle")
            Me.DataContext.CuentasFondosDetalle.Attach(currentCuentasFondosDetalle, Me.ChangeSet.GetOriginal(currentCuentasFondosDetalle))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentasFondosDetalle")
        End Try
    End Sub

    Public Sub DeleteCuentasFondosDetalle(ByVal deleteCuentasFondosDetalle As CuentasFondosDetalle)
        Try
            deleteCuentasFondosDetalle.strInfoSesion = DemeInfoSesion(deleteCuentasFondosDetalle.pstrUsuarioConexion, "DeleteCuentasFondosDetalle")
            Me.DataContext.CuentasFondosDetalle.Attach(deleteCuentasFondosDetalle)
            Me.DataContext.CuentasFondosDetalle.DeleteOnSubmit(deleteCuentasFondosDetalle)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCuentasFondosDetalle")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function CuentasFondosDetalle_Consultar(ByVal pintIDCuentasDeceval As System.Nullable(Of Integer), ByVal pstrNroDocumento As String, ByVal pstrTipoIdentificacion As String, ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal pstrUsuario As String) As List(Of CuentasFondosDetalle)
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_CuentasFondosDetalle_Consultar(String.Empty, pintIDCuentasDeceval, pstrNroDocumento, pstrTipoIdentificacion, plngidCuentaDeceval, pstrDeposito, pstrUsuario, DemeInfoSesion(pstrUsuario, "CuentasFondosDetalle_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasFondosDetalle_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function CuentasFondosDetalle_ConsultarSync(ByVal pintIDCuentasDeceval As System.Nullable(Of Integer), ByVal pstrNroDocumento As String, ByVal pstrTipoIdentificacion As String, ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal pstrUsuario As String) As List(Of CuentasFondosDetalle)
        Dim objTask As Task(Of List(Of CuentasFondosDetalle)) = Me.CuentasFondosDetalle_ConsultarAsync(pintIDCuentasDeceval, pstrNroDocumento, pstrTipoIdentificacion, plngidCuentaDeceval, pstrDeposito, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CuentasFondosDetalle_ConsultarAsync(ByVal pintIDCuentasDeceval As System.Nullable(Of Integer), ByVal pstrNroDocumento As String, ByVal pstrTipoIdentificacion As String, ByVal plngidCuentaDeceval As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal pstrUsuario As String) As Task(Of List(Of CuentasFondosDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CuentasFondosDetalle)) = New TaskCompletionSource(Of List(Of CuentasFondosDetalle))()
        objTaskComplete.TrySetResult(CuentasFondosDetalle_Consultar(pintIDCuentasDeceval, pstrNroDocumento, pstrTipoIdentificacion, plngidCuentaDeceval, pstrDeposito, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "EstadosEntradaSalidaTitulos"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertEstadosEntradaSalidaTitulos(ByVal newEstadosEntradaSalidaTitulos As EstadosEntradaSalidaTitulos)
        Try
            newEstadosEntradaSalidaTitulos.strInfoSesion = DemeInfoSesion(newEstadosEntradaSalidaTitulos.pstrUsuarioConexion, "InsertEstadosEntradaSalidaTitulos")
            Me.DataContext.EstadosEntradaSalidaTitulos.InsertOnSubmit(newEstadosEntradaSalidaTitulos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEstadosEntradaSalidaTitulos")
        End Try
    End Sub

    Public Sub UpdateEstadosEntradaSalidaTitulos(ByVal currentEstadosEntradaSalidaTitulos As EstadosEntradaSalidaTitulos)
        Try
            currentEstadosEntradaSalidaTitulos.strInfoSesion = DemeInfoSesion(currentEstadosEntradaSalidaTitulos.pstrUsuarioConexion, "UpdateEstadosEntradaSalidaTitulos")
            Me.DataContext.EstadosEntradaSalidaTitulos.Attach(currentEstadosEntradaSalidaTitulos, Me.ChangeSet.GetOriginal(currentEstadosEntradaSalidaTitulos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEstadosEntradaSalidaTitulos")
        End Try
    End Sub

    Public Sub DeleteEstadosEntradaSalidaTitulos(ByVal deleteEstadosEntradaSalidaTitulos As EstadosEntradaSalidaTitulos)
        Try
            deleteEstadosEntradaSalidaTitulos.strInfoSesion = DemeInfoSesion(deleteEstadosEntradaSalidaTitulos.pstrUsuarioConexion, "DeleteEstadosEntradaSalidaTitulos")
            Me.DataContext.EstadosEntradaSalidaTitulos.Attach(deleteEstadosEntradaSalidaTitulos)
            Me.DataContext.EstadosEntradaSalidaTitulos.DeleteOnSubmit(deleteEstadosEntradaSalidaTitulos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEstadosEntradaSalidaTitulos")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function EstadosEntradaSalidaTitulos_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of EstadosEntradaSalidaTitulos)
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_EstadosEntradaSalidaTitulos_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "EstadosEntradaSalidaTitulos_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EstadosEntradaSalidaTitulos_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function EstadosEntradaSalidaTitulos_Consultar(ByVal pstrDescripcion As String, ByVal pstrTopico As String, ByVal pstrMecanismo As String, ByVal pstrMotivoBloqueo As String, ByVal pstrEstadoActual As String, ByVal pstrUsuario As String) As List(Of EstadosEntradaSalidaTitulos)
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_EstadosEntradaSalidaTitulos_Consultar(String.Empty, pstrDescripcion, pstrTopico, pstrMecanismo, pstrMotivoBloqueo, pstrEstadoActual, pstrUsuario, DemeInfoSesion(pstrUsuario, "EstadosEntradaSalidaTitulos_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EstadosEntradaSalidaTitulos_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function EstadosEntradaSalidaTitulos_ConsultarPorDefecto(ByVal pstrUsuario As String) As EstadosEntradaSalidaTitulos
        Dim objEstadosEntradaSalidaTitulos As EstadosEntradaSalidaTitulos = Nothing
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_EstadosEntradaSalidaTitulos_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "EstadosEntradaSalidaTitulos_ConsultarPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objEstadosEntradaSalidaTitulos = ret.FirstOrDefault
            End If
            Return objEstadosEntradaSalidaTitulos
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EstadosEntradaSalidaTitulos_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar el encabezado y detalle
    ''' </summary>
    Public Function EstadosEntradaSalidaTitulos_Actualizar(ByVal pintIDEstadosEntradaSalidaTitulos As System.Nullable(Of Integer), ByVal pstrRetorno As String, ByVal pstrDescripcion As String, ByVal pstrTopico As String, ByVal pstrMecanismo As String, ByVal pstrMotivoBloqueo As String, ByVal pstrEstadoActual As String, ByVal pstrUsuario As String) As String
        Try
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_EstadosEntradaSalidaTitulos_Actualizar(pintIDEstadosEntradaSalidaTitulos, pstrRetorno, pstrDescripcion, pstrTopico, pstrMecanismo, pstrMotivoBloqueo, pstrEstadoActual, pstrUsuario, DemeInfoSesion(pstrUsuario, "EstadosEntradaSalidaTitulos_Actualizar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EstadosEntradaSalidaTitulos_Actualizar")
            Return Nothing
        End Try
    End Function

    Public Function EstadosEntradaSalidaTitulos_Consecutivo_Consultar(ByVal pstrTopico As String, ByVal pstrUsuario As String) As String
        Try
            Dim pstrRetorno As String = ""
            Me.DataContext.uspCalculosFinancieros_EstadosEntradaSalidaTitulos_Consecutivo_Consultar(pstrTopico, pstrUsuario, DemeInfoSesion(pstrUsuario, "EstadosEntradaSalidaTitulos_Consecutivo_Consultar"), 0, pstrRetorno)
            Return pstrRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EstadosEntradaSalidaTitulos_Consecutivo_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function EstadosEntradaSalidaTitulos_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of EstadosEntradaSalidaTitulos)
        Dim objTask As Task(Of List(Of EstadosEntradaSalidaTitulos)) = Me.EstadosEntradaSalidaTitulos_FiltrarAsync(pstrFiltro, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function EstadosEntradaSalidaTitulos_FiltrarAsync(ByVal pstrFiltro As String, pstrUsuario As String) As Task(Of List(Of EstadosEntradaSalidaTitulos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EstadosEntradaSalidaTitulos)) = New TaskCompletionSource(Of List(Of EstadosEntradaSalidaTitulos))()
        objTaskComplete.TrySetResult(EstadosEntradaSalidaTitulos_Filtrar(pstrFiltro, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function EstadosEntradaSalidaTitulos_ConsultarSync(ByVal pstrDescripcion As String, ByVal pstrTopico As String, ByVal pstrMecanismo As String, ByVal pstrMotivoBloqueo As String, ByVal pstrEstadoActual As String, ByVal pstrUsuario As String) As List(Of EstadosEntradaSalidaTitulos)
        Dim objTask As Task(Of List(Of EstadosEntradaSalidaTitulos)) = Me.EstadosEntradaSalidaTitulos_ConsultarAsync(pstrDescripcion, pstrTopico, pstrMecanismo, pstrMotivoBloqueo, pstrEstadoActual, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function EstadosEntradaSalidaTitulos_ConsultarAsync(ByVal pstrDescripcion As String, ByVal pstrTopico As String, ByVal pstrMecanismo As String, ByVal pstrMotivoBloqueo As String, ByVal pstrEstadoActual As String, ByVal pstrUsuario As String) As Task(Of List(Of EstadosEntradaSalidaTitulos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EstadosEntradaSalidaTitulos)) = New TaskCompletionSource(Of List(Of EstadosEntradaSalidaTitulos))()
        objTaskComplete.TrySetResult(EstadosEntradaSalidaTitulos_Consultar(pstrDescripcion, pstrTopico, pstrMecanismo, pstrMotivoBloqueo, pstrEstadoActual, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function EstadosEntradaSalidaTitulos_ConsultarPorDefectoSync(ByVal pstrUsuario As String) As EstadosEntradaSalidaTitulos
        Dim objTask As Task(Of EstadosEntradaSalidaTitulos) = Me.EstadosEntradaSalidaTitulos_ConsultarPorDefectoAsync(pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function EstadosEntradaSalidaTitulos_ConsultarPorDefectoAsync(pstrUsuario As String) As Task(Of EstadosEntradaSalidaTitulos)
        Dim objTaskComplete As TaskCompletionSource(Of EstadosEntradaSalidaTitulos) = New TaskCompletionSource(Of EstadosEntradaSalidaTitulos)()
        objTaskComplete.TrySetResult(EstadosEntradaSalidaTitulos_ConsultarPorDefecto(pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function EstadosEntradaSalidaTitulos_ActualizarSync(ByVal pintIDEstadosEntradaSalidaTitulos As System.Nullable(Of Integer), ByVal pstrRetorno As String, ByVal pstrDescripcion As String, ByVal pstrTopico As String, ByVal pstrMecanismo As String, ByVal pstrMotivoBloqueo As String, ByVal pstrEstadoActual As String, ByVal pstrUsuario As String) As String
        Dim objTask As Task(Of String) = Me.EstadosEntradaSalidaTitulos_ActualizarAsync(pintIDEstadosEntradaSalidaTitulos, pstrRetorno, pstrDescripcion, pstrTopico, pstrMecanismo, pstrMotivoBloqueo, pstrEstadoActual, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function EstadosEntradaSalidaTitulos_ActualizarAsync(ByVal pintIDEstadosEntradaSalidaTitulos As System.Nullable(Of Integer), ByVal pstrRetorno As String, ByVal pstrDescripcion As String, ByVal pstrTopico As String, ByVal pstrMecanismo As String, ByVal pstrMotivoBloqueo As String, ByVal pstrEstadoActual As String, ByVal pstrUsuario As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(EstadosEntradaSalidaTitulos_Actualizar(pintIDEstadosEntradaSalidaTitulos, pstrRetorno, pstrDescripcion, pstrTopico, pstrMecanismo, pstrMotivoBloqueo, pstrEstadoActual, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

End Class
