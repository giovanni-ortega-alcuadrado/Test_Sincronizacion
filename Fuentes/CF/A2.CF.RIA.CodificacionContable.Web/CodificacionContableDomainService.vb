Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFCodificacionContable
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura


<EnableClientAccess()>
Partial Public Class CodificacionContableDomainService
    Inherits LinqToSqlDomainService(Of CodificacionContableDBML)

    Private _consultarConfiguracionContableXValor As List(Of ConfiguracionContableXValor)
    Private _consultarCodificacionContableDetallePorDefecto As CodificacionContableDetalle

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "ConfiguracionContableXModulo"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertConfiguracionContableXModulo(ByVal newConfiguracionContableXModulo As ConfiguracionContableXModulo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newConfiguracionContableXModulo.pstrUsuarioConexion, newConfiguracionContableXModulo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newConfiguracionContableXModulo.strInfoSesion = DemeInfoSesion(newConfiguracionContableXModulo.pstrUsuarioConexion, "InsertConfiguracionContableXModulo")
            Me.DataContext.ConfiguracionContableXModulo.InsertOnSubmit(newConfiguracionContableXModulo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConfiguracionContableXModulo")
        End Try
    End Sub

    Public Sub UpdateConfiguracionContableXModulo(ByVal currentConfiguracionContableXModulo As ConfiguracionContableXModulo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentConfiguracionContableXModulo.pstrUsuarioConexion, currentConfiguracionContableXModulo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConfiguracionContableXModulo.strInfoSesion = DemeInfoSesion(currentConfiguracionContableXModulo.pstrUsuarioConexion, "UpdateConfiguracionContableXModulo")
            Me.DataContext.ConfiguracionContableXModulo.Attach(currentConfiguracionContableXModulo, Me.ChangeSet.GetOriginal(currentConfiguracionContableXModulo))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfiguracionContableXModulo")
        End Try
    End Sub

    Public Sub DeleteConfiguracionContableXModulo(ByVal deleteConfiguracionContableXModulo As ConfiguracionContableXModulo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteConfiguracionContableXModulo.pstrUsuarioConexion, deleteConfiguracionContableXModulo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteConfiguracionContableXModulo.strInfoSesion = DemeInfoSesion(deleteConfiguracionContableXModulo.pstrUsuarioConexion, "DeleteConfiguracionContableXModulo")
            Me.DataContext.ConfiguracionContableXModulo.Attach(deleteConfiguracionContableXModulo)
            Me.DataContext.ConfiguracionContableXModulo.DeleteOnSubmit(deleteConfiguracionContableXModulo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConfiguracionContableXModulo")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarConfiguracionContableXModulo(pintPadre As Integer, pstrTopico As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableXModulo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CodificacionContable_LlenarCombosConfiguracion(pintPadre, pstrTopico, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionContableXModulo"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionContableXModulo")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarConfiguracionContableXModuloSync(ByVal pintPadre As Integer, ByVal pstrTopico As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableXModulo)
        Dim objTask As Task(Of List(Of ConfiguracionContableXModulo)) = Me.ConsultarConfiguracionContableXModuloAsync(pintPadre, pstrTopico, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConfiguracionContableXModuloAsync(ByVal pintPadre As Integer, ByVal pstrTopico As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionContableXModulo))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionContableXModulo)) = New TaskCompletionSource(Of List(Of ConfiguracionContableXModulo))()
        objTaskComplete.TrySetResult(ConsultarConfiguracionContableXModulo(pintPadre, pstrTopico, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CodificacionContable"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertCodificacionContable(ByVal currentCodificacionContables As CodificacionContable)

    End Sub

    Public Sub UpdateCodificacionContable(ByVal currentCodificacionContables As CodificacionContable)

    End Sub

    Public Sub DeleteCodificacionContable(ByVal currentCodificacionContables As CodificacionContable)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCodificacionContables.pstrUsuarioConexion, currentCodificacionContables.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCodificacionContables.InfoSesion = DemeInfoSesion(currentCodificacionContables.pstrUsuarioConexion, "DeleteCodificacionContable")
            Me.DataContext.CodificacionContable.Attach(currentCodificacionContables)
            Me.DataContext.CodificacionContable.DeleteOnSubmit(currentCodificacionContables)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCodificacionContable")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function FiltrarCodificacionContable(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodificacionContable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContable_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarCodificacionContable"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarCodificacionContable")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCodificacionContable(plngID As Integer, plngModulo As Integer, plngNegocio As Integer, plngOperacion As Integer, plngDuracion As Integer, plngTipoFecha As Integer, pintTipoProducto As Integer, pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodificacionContable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContable_Consultar(String.Empty, plngID, plngModulo, plngNegocio, plngOperacion, plngDuracion, plngTipoFecha, pintTipoProducto, pstrTipoInversion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContable"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCodificacionContable")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCodificacionContablePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CodificacionContable
        Dim objCodificacionContable As CodificacionContable = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContable_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, 0, 0, 0, 0, 0, 0, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContablePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCodificacionContable = ret.FirstOrDefault
            End If
            Return objCodificacionContable
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCodificacionContablePorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar la codificación contable
    ''' </summary>
    ''' <returns>Si es True es porque inserto correctamente</returns>
    ''' <remarks>JEP</remarks>
    ''' <history>
    ''' Modificado por   : Germán González (Alcuadrado S.A.)
    ''' Descripción      : Se cambia el retorno booleano de la funciónpor una cadena con el mensaje de error
    ''' Fecha            : Abril 8/2014
    ''' Pruebas CB       : Germán González - Abril 8/2014 - Pendientes
    ''' </history>
    Public Function ActualizarCodificacionContable(ByVal plngID As Integer?, ByVal pstrNormaContable As String, ByVal plngModulo As Integer, ByVal plngNegocio As Integer,
                                                ByVal plngOperacion As Integer, ByVal plngDuracion As Integer, ByVal plngTipoFecha As Integer, ByVal pintTipoProducto As Integer,
                                                ByVal pstrTipoInversion As String, ByVal plngIdMoneda As Integer, ByVal pdtmFechaInicio As Date, ByVal plogActivo As Integer,
                                                ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContable_Actualizar(plngID, pstrNormaContable, plngModulo, plngNegocio, plngOperacion, plngDuracion, plngTipoFecha, pintTipoProducto, pstrTipoInversion, plngIdMoneda, pdtmFechaInicio, plogActivo, pxmlDetalleGrid, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarSegmentosNegocios"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarCodificacionContable")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarCodificacionContableSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodificacionContable)
        Dim objTask As Task(Of List(Of CodificacionContable)) = Me.FiltrarIndicadoresAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function FiltrarIndicadoresAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CodificacionContable))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CodificacionContable)) = New TaskCompletionSource(Of List(Of CodificacionContable))()
        objTaskComplete.TrySetResult(FiltrarCodificacionContable(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCodificacionContableSync(ByVal plngID As Integer, ByVal plngModulo As Integer, ByVal plngNegocio As Integer, ByVal plngOperacion As Integer, ByVal plngDuracion As Integer, ByVal plngTipoFecha As Integer, ByVal pintTipoProducto As Integer, ByVal pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodificacionContable)
        Dim objTask As Task(Of List(Of CodificacionContable)) = Me.ConsultarCodificacionContableAsync(plngID, plngModulo, plngNegocio, plngOperacion, plngDuracion, plngTipoFecha, pintTipoProducto, pstrTipoInversion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCodificacionContableAsync(ByVal plngID As Integer, ByVal plngModulo As Integer, ByVal plngNegocio As Integer, ByVal plngOperacion As Integer, ByVal plngDuracion As Integer, ByVal plngTipoFecha As Integer, ByVal pintTipoProducto As Integer, ByVal pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CodificacionContable))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CodificacionContable)) = New TaskCompletionSource(Of List(Of CodificacionContable))()
        objTaskComplete.TrySetResult(ConsultarCodificacionContable(plngID, plngModulo, plngNegocio, plngOperacion, plngDuracion, plngTipoFecha, pintTipoProducto, pstrTipoInversion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCodificacionContablePorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CodificacionContable
        Dim objTask As Task(Of CodificacionContable) = Me.ConsultarCodificacionContablePorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCodificacionContablePorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CodificacionContable)
        Dim objTaskComplete As TaskCompletionSource(Of CodificacionContable) = New TaskCompletionSource(Of CodificacionContable)()
        objTaskComplete.TrySetResult(ConsultarCodificacionContablePorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Función para realizar llamado sincronico al método ActualizarCodificacionContableAsync y controlar los mensajes de salida
    ''' </summary>
    ''' <param name="plngID">Parámetro tipo Integer</param>
    ''' <param name="pstrNormaContable">Parámetro tipo String</param>
    ''' <param name="plngModulo">Parámetro tipo Integer</param>
    ''' <param name="plngNegocio">Parámetro tipo Integer</param>
    ''' <param name="plngOperacion">Parámetro tipo Integer</param>
    ''' <param name="plngDuracion">Parámetro tipo Integer</param>
    ''' <param name="plngTipoFecha">Parámetro tipo Integer</param>
    ''' <param name="pstrTipoInversion">Parámetro tipo String</param>
    ''' <param name="pstrTipoProducto">Parámetro tipo String</param>
    ''' <param name="plngIdMoneda">Parámetro tipo Integer</param>
    ''' <param name="pdtmFechaInicio">Parámetro tipo Date</param>
    ''' <param name="plogActivo">Parámetro tipo Integer</param>
    ''' <param name="pxmlDetalleGrid">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna una cadena con el mensaje de validación del sp</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Descripción  : Llamado sincronico al método  ActualizarCodificacionContableAsync
    ''' Fecha        : Abril 08/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Pendiente
    ''' </history>
    Public Function ActualizarCodificacionContableSync(ByVal plngID As Integer, ByVal pstrNormaContable As String, ByVal plngModulo As Integer, ByVal plngNegocio As Integer,
                                                        ByVal plngOperacion As Integer, ByVal plngDuracion As Integer, ByVal plngTipoFecha As Integer, ByVal pintTipoProducto As Integer,
                                                        ByVal pstrTipoInversion As String, ByVal plngIdMoneda As Integer, ByVal pdtmFechaInicio As Date, ByVal plogActivo As Integer,
                                                        ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarCodificacionContableAsync(plngID, pstrNormaContable, plngModulo, plngNegocio,
                                                                                plngOperacion, plngDuracion, plngTipoFecha, pintTipoProducto,
                                                                                pstrTipoInversion, plngIdMoneda, pdtmFechaInicio, plogActivo,
                                                                                pxmlDetalleGrid, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    ''' <summary>
    ''' Función para realizar el llamado al proceso ActualizarCodificacionContable 
    ''' </summary>
    ''' <param name="plngID">Parámetro tipo Integer</param>
    ''' <param name="pstrNormaContable">Parámetro tipo String</param>
    ''' <param name="plngModulo">Parámetro tipo Integer</param>
    ''' <param name="plngNegocio">Parámetro tipo Integer</param>
    ''' <param name="plngOperacion">Parámetro tipo Integer</param>
    ''' <param name="plngDuracion">Parámetro tipo Integer</param>
    ''' <param name="plngTipoFecha">Parámetro tipo Integer</param>
    ''' <param name="plngTipoProducto">Parámetro tipo Integer</param>
    ''' <param name="plngTipoInversion">Parámetro tipo Integer</param>
    ''' <param name="plngIdMoneda">Parámetro tipo Integer</param>
    ''' <param name="pdtmFechaInicio">Parámetro tipo Date</param>
    ''' <param name="plogActivo">Parámetro tipo Integer</param>
    ''' <param name="pxmlDetalleGrid">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna una cadena con el mensaje de validación del sp</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Descripción  : Llamado sincronico al método  ActualizarCodificacionContableAsync
    ''' Fecha        : Abril 08/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Pendiente
    ''' </history>
    Private Function ActualizarCodificacionContableAsync(ByVal plngID As Integer, ByVal pstrNormaContable As String, ByVal plngModulo As Integer, ByVal plngNegocio As Integer,
                                                        ByVal plngOperacion As Integer, ByVal plngDuracion As Integer, ByVal plngTipoFecha As Integer, ByVal pintTipoProducto As Integer,
                                                        ByVal pstrTipoInversion As String, ByVal plngIdMoneda As Integer, ByVal pdtmFechaInicio As Date, ByVal plogActivo As Integer,
                                                        ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarCodificacionContable(plngID, pstrNormaContable, plngModulo, plngNegocio,
                                                                    plngOperacion, plngDuracion, plngTipoFecha, pintTipoProducto,
                                                                    pstrTipoInversion, plngIdMoneda, pdtmFechaInicio, plogActivo,
                                                                    pxmlDetalleGrid, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CodificacionContableDetalle"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertCodificacionContableDetalle(ByVal objCodificacionContableDetalle As CodificacionContableDetalle)

    End Sub

    Public Sub UpdateCodificacionContableDetalle(ByVal currentCodificacionContableDetalle As CodificacionContableDetalle)

    End Sub

    Public Sub DeleteCodificacionContableDetalle(ByVal objCodificacionContableDetalle As CodificacionContableDetalle)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarCodificacionContableDetalle(plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodificacionContableDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContableDetalle_Consultar(String.Empty, plngID, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContable"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCodificacionContableDetalle")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCodificacionContableDetallePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CodificacionContableDetalle
        Dim objCodificacionContableDetalle As CodificacionContableDetalle = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContableDetalle_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContablePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCodificacionContableDetalle = ret.FirstOrDefault
            End If
            Return objCodificacionContableDetalle
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCodificacionContableDetallePorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarCodificacionContableDetalleSync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodificacionContableDetalle)
        Dim objTask As Task(Of List(Of CodificacionContableDetalle)) = Me.ConsultarCodificacionContableDetalleAsync(plngID, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCodificacionContableDetalleAsync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CodificacionContableDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CodificacionContableDetalle)) = New TaskCompletionSource(Of List(Of CodificacionContableDetalle))()
        objTaskComplete.TrySetResult(ConsultarCodificacionContableDetalle(plngID, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCodificacionContableDetallePorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CodificacionContableDetalle
        Dim objTask As Task(Of CodificacionContableDetalle) = Me.ConsultarCodificacionContableDetallePorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCodificacionContableDetallePorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CodificacionContableDetalle)
        Dim objTaskComplete As TaskCompletionSource(Of CodificacionContableDetalle) = New TaskCompletionSource(Of CodificacionContableDetalle)()
        objTaskComplete.TrySetResult(ConsultarCodificacionContableDetallePorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ConfiguracionContableXValor"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertConfiguracionContableXValor(ByVal newConfiguracionContableXValor As ConfiguracionContableXValor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newConfiguracionContableXValor.pstrUsuarioConexion, newConfiguracionContableXValor.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newConfiguracionContableXValor.InfoSesion = DemeInfoSesion(newConfiguracionContableXValor.pstrUsuarioConexion, "InsertConfiguracionContableXValor")
            Me.DataContext.ConfiguracionContableXValor.InsertOnSubmit(newConfiguracionContableXValor)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConfiguracionContableXValor")
        End Try
    End Sub

    Public Sub UpdateConfiguracionContableXValor(ByVal currentConfiguracionContableXValors As ConfiguracionContableXValor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentConfiguracionContableXValors.pstrUsuarioConexion, currentConfiguracionContableXValors.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ConfiguracionContableXValor.InsertOnSubmit(currentConfiguracionContableXValors)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfiguracionContableXValor")
        End Try
    End Sub

    Public Sub DeleteConfiguracionContableXValor(ByVal currentConfiguracionContableXValors As ConfiguracionContableXValor)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentConfiguracionContableXValors.pstrUsuarioConexion, currentConfiguracionContableXValors.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ConfiguracionContableXValor.Attach(currentConfiguracionContableXValors)
            Me.DataContext.ConfiguracionContableXValor.DeleteOnSubmit(currentConfiguracionContableXValors)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConfiguracionContableXValor")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarConfiguracionContableXValor(plogTotalizado As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableXValor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspConfiguracionContableXValor_Consultar(plogTotalizado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionContableXValor"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionContableXValor")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarConfiguracionContableXValorSync(ByVal plogTotalizado As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableXValor)
        Dim objTask As Task(Of List(Of ConfiguracionContableXValor)) = Me.ConsultarConfiguracionContableXValorAsync(plogTotalizado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConfiguracionContableXValorAsync(ByVal plogTotalizado As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionContableXValor))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionContableXValor)) = New TaskCompletionSource(Of List(Of ConfiguracionContableXValor))()
        objTaskComplete.TrySetResult(ConsultarConfiguracionContableXValor(plogTotalizado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
#End Region

#End Region

#Region "ConfiguracionContableConcepto"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertConfiguracionContableConcepto(ByVal newConfiguracionContableConceptos As ConfiguracionContableConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newConfiguracionContableConceptos.pstrUsuarioConexion, newConfiguracionContableConceptos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newConfiguracionContableConceptos.strInfoSesion = DemeInfoSesion(newConfiguracionContableConceptos.pstrUsuarioConexion, "InsertConfiguracionContableConcepto")
            Me.DataContext.ConfiguracionContableConcepto.InsertOnSubmit(newConfiguracionContableConceptos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConfiguracionContableConcepto")
        End Try
    End Sub

    Public Sub UpdateConfiguracionContableConcepto(ByVal currentConfiguracionContableConceptos As ConfiguracionContableConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentConfiguracionContableConceptos.pstrUsuarioConexion, currentConfiguracionContableConceptos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConfiguracionContableConceptos.strInfoSesion = DemeInfoSesion(currentConfiguracionContableConceptos.pstrUsuarioConexion, "UpdateConfiguracionContableConcepto")
            Me.DataContext.ConfiguracionContableConcepto.Attach(currentConfiguracionContableConceptos, Me.ChangeSet.GetOriginal(currentConfiguracionContableConceptos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfiguracionContableConcepto")
        End Try
    End Sub

    Public Sub DeleteConfiguracionContableConcepto(ByVal deleteConfiguracionContableConceptos As ConfiguracionContableConcepto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteConfiguracionContableConceptos.pstrUsuarioConexion, deleteConfiguracionContableConceptos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteConfiguracionContableConceptos.strInfoSesion = DemeInfoSesion(deleteConfiguracionContableConceptos.pstrUsuarioConexion, "DeleteConfiguracionContableConcepto")
            Me.DataContext.ConfiguracionContableConcepto.Attach(deleteConfiguracionContableConceptos)
            Me.DataContext.ConfiguracionContableConcepto.DeleteOnSubmit(deleteConfiguracionContableConceptos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConfiguracionContableConcepto")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function FiltrarConfiguracionContableConcepto(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContableConcepto_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarConfiguracionContableConcepto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarConfiguracionContableConcepto")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConfiguracionContableConcepto(pstrConcepto As String, pstrNormaContable As String, pstrCuentaContableDBPositiva As String, pstrCuentaContableDBNegativa As String, pstrCuentaContableCRPositiva As String, pstrCuentaContableCRNegativa As String, pstrDetalleTipoTitulos As String, pintTipoProducto As Integer, pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContableConcepto_Consultar(String.Empty, pstrConcepto, pstrNormaContable, pstrCuentaContableDBPositiva, pstrCuentaContableDBNegativa, pstrCuentaContableCRPositiva, pstrCuentaContableCRNegativa, pstrDetalleTipoTitulos, pintTipoProducto, pstrTipoInversion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionContableConcepto"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionContableConcepto")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConfiguracionContableConceptoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionContableConcepto
        Dim objConfiguracionContableConcepto As ConfiguracionContableConcepto = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodificacionContableConcepto_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, 0, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionContableConceptoPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objConfiguracionContableConcepto = ret.FirstOrDefault
            End If
            Return objConfiguracionContableConcepto
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionContableConceptoPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarConfiguracionContableConceptoSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableConcepto)
        Dim objTask As Task(Of List(Of ConfiguracionContableConcepto)) = Me.FiltrarConfiguracionContableConceptoAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function FiltrarConfiguracionContableConceptoAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionContableConcepto))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionContableConcepto)) = New TaskCompletionSource(Of List(Of ConfiguracionContableConcepto))()
        objTaskComplete.TrySetResult(FiltrarConfiguracionContableConcepto(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConfiguracionContableConceptoSync(ByVal pstrConcepto As String, ByVal pstrNormaContable As String, ByVal pstrCuentaContableDBPositiva As String, ByVal pstrCuentaContableDBNegativa As String, ByVal pstrCuentaContableCRPositiva As String, ByVal pstrCuentaContableCRNegativa As String, ByVal pstrDetalleTipoTitulos As String, pintTipoProducto As Integer, pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionContableConcepto)
        Dim objTask As Task(Of List(Of ConfiguracionContableConcepto)) = Me.ConsultarConfiguracionContableConceptoAsync(pstrConcepto, pstrNormaContable, pstrCuentaContableDBPositiva, pstrCuentaContableDBNegativa, pstrCuentaContableCRPositiva, pstrCuentaContableCRNegativa, pstrDetalleTipoTitulos, pintTipoProducto, pstrTipoInversion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConfiguracionContableConceptoAsync(ByVal pstrConcepto As String, ByVal pstrNormaContable As String, ByVal pstrCuentaContableDBPositiva As String, ByVal pstrCuentaContableDBNegativa As String, ByVal pstrCuentaContableCRPositiva As String, ByVal pstrCuentaContableCRNegativa As String, ByVal pstrDetalleTipoTitulos As String, pintTipoProducto As Integer, pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionContableConcepto))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionContableConcepto)) = New TaskCompletionSource(Of List(Of ConfiguracionContableConcepto))()
        objTaskComplete.TrySetResult(ConsultarConfiguracionContableConcepto(pstrConcepto, pstrNormaContable, pstrCuentaContableDBPositiva, pstrCuentaContableDBNegativa, pstrCuentaContableCRPositiva, pstrCuentaContableCRNegativa, pstrDetalleTipoTitulos, pintTipoProducto, pstrTipoInversion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConfiguracionContableConceptoPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionContableConcepto
        Dim objTask As Task(Of ConfiguracionContableConcepto) = Me.ConsultarConfiguracionContableConceptoPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConfiguracionContableConceptoPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ConfiguracionContableConcepto)
        Dim objTaskComplete As TaskCompletionSource(Of ConfiguracionContableConcepto) = New TaskCompletionSource(Of ConfiguracionContableConcepto)()
        objTaskComplete.TrySetResult(ConsultarConfiguracionContableConceptoPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ModificacionOperacionesCumplidas"

#Region "Métodos modelo para activar funcionalidad RIA"

    ''' <summary>
    ''' Este metodo vacío se implementa para que el sistema permita modificar las entidades desde el grid de Operaciones Cumplidas
    ''' </summary>
    ''' <param name="currentOperacionesCumplidas">Objeto tipo OperacionesCumplidas</param>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Fecha           : Abril 22/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 22/2014 - Resultado OK
    ''' </history>
    Public Sub UpdateOperacionesCumplidas(ByVal currentOperacionesCumplidas As OperacionesCumplidas)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    ''' <summary>
    ''' Consulta las operaciones (liquidaciones) cumplidas importadas
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una lista de la entidad OperacionesCumplidas</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Public Function OperacionesCumplidasConsultar(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesCumplidas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_OperacionesCumplidasConsultar(pstrUsuario, DemeInfoSesion(pstrUsuario, "OperacionesCumplidasConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OperacionesCumplidasConsultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Actualiza las operaciones (liquidaciones) cumplidas que se han importado
    ''' </summary>
    ''' <param name="pxmlOperacionesCumplidas">Parametro de tipo string que contiene un xml con los datos del grid a actualizar</param>
    ''' <param name="pstrUsuario">Parametro tipo string</param>
    ''' <returns></returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Public Function OperacionesCumplidasActualizar(ByVal pxmlOperacionesCumplidas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_OperacionesCumplidasActualizar(pxmlOperacionesCumplidas,
                                                                                           pstrUsuario,
                                                                                           DemeInfoSesion(pstrUsuario, "UtilidadesCustodiasActualizar"),
                                                                                           0,
                                                                                           "")
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OperacionesCumplidasActualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    ''' <summary>
    ''' Se utiliza para llamar el método OperacionesCumplidasConsultar
    ''' </summary>
    ''' <param name="pstrUsuario">Parametro tipo string</param>
    ''' <returns>Retorna una lista de entidades tipo OperacionesCumplidas</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private Function OperacionesCumplidasConsultarAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OperacionesCumplidas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OperacionesCumplidas)) = New TaskCompletionSource(Of List(Of OperacionesCumplidas))()
        objTaskComplete.TrySetResult(OperacionesCumplidasConsultar(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Metodo para llamar de forma sincronica la función OperacionesCumplidasConsultarAsync
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una lista de entidades tipo OperacionesCumplidas</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Public Function OperacionesCumplidasConsultarSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesCumplidas)
        Dim objTask As Task(Of List(Of OperacionesCumplidas)) = Me.OperacionesCumplidasConsultarAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    ''' <summary>
    ''' Se utiliza para llamar el método OperacionesCumplidasActualizar
    ''' </summary>
    ''' <param name="pxmlOperacionesCumplidas">Parametro de tipo string que contiene un xml con los datos del grid a actualizar</param>
    ''' <param name="pstrUsuario">Parametro tipo string</param>
    ''' <returns>Retorna un dato Task(Of Integer) indicando si es exitoso el proceso</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private Function OperacionesCumplidasActualizarAsync(ByVal pxmlOperacionesCumplidas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Integer)
        Dim objTaskComplete As TaskCompletionSource(Of Integer) = New TaskCompletionSource(Of Integer)()
        objTaskComplete.TrySetResult(OperacionesCumplidasActualizar(pxmlOperacionesCumplidas, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Metodo para llamar de forma sincronica la función OperacionesCumplidasActualizarAsync
    ''' </summary>
    ''' <param name="pxmlOperacionesCumplidas">Parametro de tipo string que contiene un xml con los datos del grid a actualizar</param>
    ''' <param name="pstrUsuario">Parametro tipo string</param>
    ''' <returns>Retorna un valor Integer que indica si el proceso es exitoso</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Public Function OperacionesCumplidasActualizarSync(ByVal pxmlOperacionesCumplidas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Dim objTask As Task(Of Integer) = Me.OperacionesCumplidasActualizarAsync(pxmlOperacionesCumplidas, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

#End Region

#End Region

#Region "Dinámica contable"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertDinamicaContable(ByVal currentDinamicaContable As DinamicaContable)

    End Sub

    Public Sub UpdateDinamicaContable(ByVal currentDinamicaContable As DinamicaContable)

    End Sub

    Public Sub DeleteDinamicaContable(ByVal currentDinamicaContable As DinamicaContable)

    End Sub

    Public Sub InsertDinamicaContableConfig(ByVal currentDinamicaContableConfig As DinamicaContableConfig)

    End Sub

    Public Sub UpdateDinamicaContableConfig(ByVal currentDinamicaContableConfig As DinamicaContableConfig)

    End Sub

    Public Sub DeleteDinamicaContableConfig(ByVal currentDinamicaContableConfig As DinamicaContableConfig)

    End Sub

    Public Sub InsertDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigDinamica)

    End Sub

    Public Sub UpdateDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigDinamica)

    End Sub

    Public Sub DeleteDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigDinamica)

    End Sub

    Public Sub InsertDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigConceptosComunes)

    End Sub

    Public Sub UpdateDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigConceptosComunes)

    End Sub

    Public Sub DeleteDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigConceptosComunes)

    End Sub

    Public Sub InsertDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigConceptosEspecificos)

    End Sub

    Public Sub UpdateDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigConceptosEspecificos)

    End Sub

    Public Sub DeleteDinamicaContableConfigDinamica(ByVal currentDinamicaContableConfig As DinamicaContableConfigConceptosEspecificos)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    ''' <summary>
    ''' Retorna las dinámicas contables que se tienen definidas
    ''' </summary>
    ''' 
    Public Function FiltrarDinamicaContable(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContable_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarCodificacionContable"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarDinamicaContable")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna las dinámicas contables que se tienen definidas que cumplen con las condiciones dadas en los diferentes campos enviados por el usuario
    ''' </summary>
    ''' 
    Public Function ConsultarDinamicaContable(ByVal pintIdDinamicaContable As Integer, ByVal pstrNormaContable As String, ByVal pstrCodDinamicaContable As String, ByVal pstrDescripcion As String, ByVal pstrFechaAplicacion As String, ByVal plogActiva As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContable_Consultar(String.Empty, pintIdDinamicaContable, pstrNormaContable, pstrCodDinamicaContable, pstrDescripcion, pstrFechaAplicacion, plogActiva, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContable"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarDinamicaContable")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna los valores por defecto para una nueva dinámica contable
    ''' </summary>
    ''' 
    Public Function ConsultarDinamicaContablePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DinamicaContable
        Dim objCodificacionContable As DinamicaContable = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContable_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, -1, String.Empty, String.Empty, String.Empty, String.Empty, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContablePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCodificacionContable = ret.FirstOrDefault
            End If
            Return objCodificacionContable
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarDinamicaContablePorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna los tipos de codificaciones contables que se han creado para los diferentes tipos de compañía
    ''' </summary>
    ''' 
    Public Function FiltrarDinamicaContableConfig(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfig)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContableConfig_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarCodificacionContableConfig"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarDinamicaContableConfig")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna los tipos de codificaciones contables que se han creado para los diferentes tipos de compañía según los valores enviados por el usuario
    ''' </summary>
    ''' 
    Public Function ConsultarDinamicaContableConfig(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrTipoCompania As String, ByVal pstrNombreConfiguracion As String, ByVal pstrDescripcion As String, ByVal pstrActiva As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfig)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContableConfig_Consultar(String.Empty, pintIdDinamicaContableConfig, pstrTipoCompania, pstrNombreConfiguracion, pstrDescripcion, pstrActiva, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContableConfig"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarDinamicaContableConfig")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna los valores por defecto para un nuevo tipo de configuración contable
    ''' </summary>
    ''' 
    Public Function ConsultarDinamicaContableConfigPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DinamicaContableConfig
        Dim objCodificacionContable As DinamicaContableConfig = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContableConfig_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, -1, String.Empty, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContableConfigPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCodificacionContable = ret.FirstOrDefault
            End If
            Return objCodificacionContable
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarDinamicaContableConfigPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna los conceptos contables que pueden configurarse en forma común para todas las clases contables
    ''' </summary>
    ''' 
    Public Function ConsultarDinamicaContableConfigDinamica(ByVal pintIdDinamicaContableConfigNorma As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfigDinamica)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContableConfig_ConsultarDinamica(pintIdDinamicaContableConfigNorma, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarDinamicaContableConfigDinamica"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarDinamicaContableConfigDinamica")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna los conceptos contables que pueden configurarse en forma común para todas las clases contables
    ''' </summary>
    ''' 
    Public Function ConsultarConceptosComunes(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodNormaContableTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfigConceptosComunes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContableConfig_ConsultarConceptosComunes(pintIdDinamicaContableConfig, pstrCodNormaContableTipoInversion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConceptosComunes"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConceptosComunes")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna los conceptos contables que deben configurarse en forma específica para cada clase contable
    ''' </summary>
    ''' 
    Public Function ConsultarConceptosClaseContable(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodNormaContableTipoInversion As String, ByVal pstrIdClaseContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfigConceptosEspecificos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContableConfig_ConsultarConceptosClaseContable(pintIdDinamicaContableConfig, pstrCodNormaContableTipoInversion, pstrIdClaseContable, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConceptosClaseContable"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConceptosClaseContable")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Retorna las listas de datos para cargar combos
    ''' </summary>
    ''' 
    Public Function ConsultarListasDinamicaContable(ByVal pstrTopico As String, ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodigo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableListas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContableConfig_Combos(pstrTopico, pintIdDinamicaContableConfig, pstrCodigo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarListasDinamicaContable"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarListasDinamicaContable")
            Return Nothing
        End Try
    End Function

    Public Function ActualizarDinamicaContableConfig(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrNombreConfig As String, ByVal pstrActiva As String, ByVal pstrDescripcion As String, ByVal pstrCodNormaContableTipoInversion As String,
                                                    ByVal pstrDinamicaContable As String, ByVal pstrConceptosComunes As String, ByVal pstrConceptosEspecifico As String, ByVal pstrUsuario As String,
                                                    ByVal pintIdCompaniaBasePUC As Integer?, ByVal pstrTipoCompania As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCF_Maestros_DinamicaContableConfig_Actualizar(pintIdDinamicaContableConfig, pstrNombreConfig, pstrActiva, pstrDescripcion,
                                                                                      pstrCodNormaContableTipoInversion, pstrDinamicaContable, pstrConceptosComunes, pstrConceptosEspecifico,
                                                                                      pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarSegmentosNegocios"),
                                                                                      pintIdCompaniaBasePUC, pstrTipoCompania, 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarDinamicaContableConfig")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"

    Public Function FiltrarDinamicaContableSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContable)
        Dim objTask As Task(Of List(Of DinamicaContable)) = Me.FiltrarDinamicaContableAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function FiltrarDinamicaContableAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DinamicaContable))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DinamicaContable)) = New TaskCompletionSource(Of List(Of DinamicaContable))()
        objTaskComplete.TrySetResult(FiltrarDinamicaContable(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarDinamicaContableSync(ByVal pintIdDinamicaContable As Integer, ByVal pstrNormaContable As String, ByVal pstrCodDinamicaContable As String, ByVal pstrDescripcion As String, ByVal pstrFechaAplicacion As String, ByVal plogActiva As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContable)
        Dim objTask As Task(Of List(Of DinamicaContable)) = Me.ConsultarDinamicaContableAsync(pintIdDinamicaContable, pstrNormaContable, pstrCodDinamicaContable, pstrDescripcion, pstrFechaAplicacion, plogActiva, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarDinamicaContableAsync(ByVal pintIdDinamicaContable As Integer, ByVal pstrNormaContable As String, ByVal pstrCodDinamicaContable As String, ByVal pstrDescripcion As String, ByVal pstrFechaAplicacion As String, ByVal plogActiva As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DinamicaContable))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DinamicaContable)) = New TaskCompletionSource(Of List(Of DinamicaContable))()
        objTaskComplete.TrySetResult(ConsultarDinamicaContable(pintIdDinamicaContable, pstrNormaContable, pstrCodDinamicaContable, pstrDescripcion, pstrFechaAplicacion, plogActiva, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarDinamicaContablePorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DinamicaContable
        Dim objTask As Task(Of DinamicaContable) = Me.ConsultarDinamicaContablePorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarDinamicaContablePorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of DinamicaContable)
        Dim objTaskComplete As TaskCompletionSource(Of DinamicaContable) = New TaskCompletionSource(Of DinamicaContable)()
        objTaskComplete.TrySetResult(ConsultarDinamicaContablePorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function FiltrarDinamicaContableConfigSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfig)
        Dim objTask As Task(Of List(Of DinamicaContableConfig)) = Me.FiltrarDinamicaContableConfigAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function FiltrarDinamicaContableConfigAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DinamicaContableConfig))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DinamicaContableConfig)) = New TaskCompletionSource(Of List(Of DinamicaContableConfig))()
        objTaskComplete.TrySetResult(FiltrarDinamicaContableConfig(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarDinamicaContableConfigSync(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrTipoCompania As String, ByVal pstrNombreConfiguracion As String, ByVal pstrDescripcion As String, ByVal pstrActiva As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfig)
        Dim objTask As Task(Of List(Of DinamicaContableConfig)) = Me.ConsultarDinamicaContableConfigAsync(pintIdDinamicaContableConfig, pstrTipoCompania, pstrNombreConfiguracion, pstrDescripcion, pstrActiva, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarDinamicaContableConfigAsync(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrTipoCompania As String, ByVal pstrNombreConfiguracion As String, ByVal pstrDescripcion As String, ByVal pstrActiva As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DinamicaContableConfig))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DinamicaContableConfig)) = New TaskCompletionSource(Of List(Of DinamicaContableConfig))()
        objTaskComplete.TrySetResult(ConsultarDinamicaContableConfig(pintIdDinamicaContableConfig, pstrTipoCompania, pstrNombreConfiguracion, pstrDescripcion, pstrActiva, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarDinamicaContableConfigPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DinamicaContableConfig
        Dim objTask As Task(Of DinamicaContableConfig) = Me.ConsultarDinamicaContableConfigPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarDinamicaContableConfigPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of DinamicaContableConfig)
        Dim objTaskComplete As TaskCompletionSource(Of DinamicaContableConfig) = New TaskCompletionSource(Of DinamicaContableConfig)()
        objTaskComplete.TrySetResult(ConsultarDinamicaContableConfigPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarDinamicaContableConfigDinamicaSync(ByVal pintIdDinamicaContableConfigNorma As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfigDinamica)
        Dim objTask As Task(Of List(Of DinamicaContableConfigDinamica)) = Me.ConsultarDinamicaContableConfigDinamicaAsync(pintIdDinamicaContableConfigNorma, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarDinamicaContableConfigDinamicaAsync(ByVal pintIdDinamicaContableConfigNorma As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DinamicaContableConfigDinamica))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DinamicaContableConfigDinamica)) = New TaskCompletionSource(Of List(Of DinamicaContableConfigDinamica))()
        objTaskComplete.TrySetResult(ConsultarDinamicaContableConfigDinamica(pintIdDinamicaContableConfigNorma, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConceptosComunesSync(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodNormaContableTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfigConceptosComunes)
        Dim objTask As Task(Of List(Of DinamicaContableConfigConceptosComunes)) = Me.ConsultarConceptosComunesAsync(pintIdDinamicaContableConfig, pstrCodNormaContableTipoInversion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConceptosComunesAsync(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodNormaContableTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DinamicaContableConfigConceptosComunes))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DinamicaContableConfigConceptosComunes)) = New TaskCompletionSource(Of List(Of DinamicaContableConfigConceptosComunes))()
        objTaskComplete.TrySetResult(ConsultarConceptosComunes(pintIdDinamicaContableConfig, pstrCodNormaContableTipoInversion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConceptosClaseContableSync(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodNormaContableTipoInversion As String, ByVal pstrIdClaseContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableConfigConceptosEspecificos)
        Dim objTask As Task(Of List(Of DinamicaContableConfigConceptosEspecificos)) = Me.ConsultarConceptosClaseContableAsync(pintIdDinamicaContableConfig, pstrCodNormaContableTipoInversion, pstrIdClaseContable, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConceptosClaseContableAsync(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodNormaContableTipoInversion As String, ByVal pstrIdClaseContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DinamicaContableConfigConceptosEspecificos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DinamicaContableConfigConceptosEspecificos)) = New TaskCompletionSource(Of List(Of DinamicaContableConfigConceptosEspecificos))()
        objTaskComplete.TrySetResult(ConsultarConceptosClaseContable(pintIdDinamicaContableConfig, pstrCodNormaContableTipoInversion, pstrIdClaseContable, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarListasDinamicaContableSync(ByVal pstrTopico As String, ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodigo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DinamicaContableListas)
        Dim objTask As Task(Of List(Of DinamicaContableListas)) = Me.ConsultarListasDinamicaContableAsync(pstrTopico, pintIdDinamicaContableConfig, pstrCodigo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarListasDinamicaContableAsync(ByVal pstrTopico As String, ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodigo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DinamicaContableListas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DinamicaContableListas)) = New TaskCompletionSource(Of List(Of DinamicaContableListas))()
        objTaskComplete.TrySetResult(ConsultarListasDinamicaContable(pstrTopico, pintIdDinamicaContableConfig, pstrCodigo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ActualizarDinamicaContableConfigSync(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrNombreConfig As String, ByVal pstrActiva As String, ByVal pstrDescripcion As String, ByVal pstrCodNormaContableTipoInversion As String,
                                                        ByVal pstrDinamicaContable As String, ByVal pstrConceptosComunes As String, ByVal pstrConceptosEspecifico As String, ByVal pstrUsuario As String,
                                                        ByVal pintIdCompaniaBasePUC As Integer?, ByVal pstrTipoCompania As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarDinamicaContableConfigAsync(pintIdDinamicaContableConfig, pstrNombreConfig, pstrActiva, pstrDescripcion,
                                                                                pstrCodNormaContableTipoInversion, pstrDinamicaContable, pstrConceptosComunes, pstrConceptosEspecifico, pstrUsuario,
                                                                                pintIdCompaniaBasePUC, pstrTipoCompania, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ActualizarDinamicaContableConfigAsync(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrNombreConfig As String, ByVal pstrActiva As String, ByVal pstrDescripcion As String, ByVal pstrCodNormaContableTipoInversion As String,
                                                        ByVal pstrDinamicaContable As String, ByVal pstrConceptosComunes As String, ByVal pstrConceptosEspecifico As String, ByVal pstrUsuario As String,
                                                        ByVal pintIdCompaniaBasePUC As Integer?, ByVal pstrTipoCompania As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarDinamicaContableConfig(pintIdDinamicaContableConfig, pstrNombreConfig, pstrActiva, pstrDescripcion,
                                                                        pstrCodNormaContableTipoInversion, pstrDinamicaContable, pstrConceptosComunes, pstrConceptosEspecifico, pstrUsuario,
                                                                        pintIdCompaniaBasePUC, pstrTipoCompania, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

End Class

