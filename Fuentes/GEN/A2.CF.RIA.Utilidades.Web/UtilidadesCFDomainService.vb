Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFUtilidades
Imports System.Web
Imports A2.OyD.OYDServer.RIA.Web.Constantes
Imports System.Data.SqlClient
Imports System.IO
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura
Imports System.Text

''' <summary>
''' DomainServices para las pantallas correspondientes a la migración de Titulos2008 a .NET
''' </summary>
''' Creado por       : Germán Arbey González Osorio (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Diciembre 15/2014
''' Pruebas CB       : Germán Arbey González Osorio - Diciembre 15/2014 - Resultado Ok
''' <remarks></remarks>

<EnableClientAccess()> _
Partial Public Class UtilidadesCFDomainService
    Inherits LinqToSqlDomainService(Of CF_UtilidadesDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub DeleteScriptsA2(ByVal ScriptA2 As ScriptsA2)

    End Sub
    Public Sub InsertScriptsA2(ByVal ScriptA2 As ScriptsA2)

    End Sub
    Public Sub UpdateScriptsA2(ByVal currentScriptA2 As ScriptsA2)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentScriptA2.pstrUsuarioConexion, currentScriptA2.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentScriptA2.InfoSesion = DemeInfoSesion(currentScriptA2.pstrUsuarioConexion, "UpdateTiposTransmision")
            Me.DataContext.ScriptsA2.Attach(currentScriptA2, Me.ChangeSet.GetOriginal(currentScriptA2))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTiposTransmision")
        End Try
    End Sub

    Public Sub DeleteScriptsA2Parametros(ByVal ScriptsA2Parametros As ScriptsA2Parametros)
    End Sub
    Public Sub InsertScriptsA2Parametros(ByVal ScriptsA2Parametros As ScriptsA2Parametros)
    End Sub
    Public Sub UpdateScriptsA2Parametros(ByVal currentScriptsA2Parametros As ScriptsA2Parametros)
    End Sub

#End Region

#Region "Métodos asincrónicos"

    ''' <summary>
    ''' Filtrar los scripts disponibles por una condición indicada en el parámetro filtro
    ''' </summary>
    ''' 
    Public Function filtrarScripts(ByVal pstrFiltro As String, ByVal pstrMostrarTodos As String, ByVal plngIDCia As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_EjecutarScripts_Scripts_Filtrar(HttpUtility.UrlDecode(pstrFiltro), pstrMostrarTodos, plngIDCia, pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "filtrarScripts"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "filtrarScripts")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consultar los scripts disponibles de acuerdo al conjunto de condiciones definido por los parámetros indicados
    ''' </summary>
    ''' 
    Public Function consultarScripts(ByVal pintIdScript As Integer, ByVal pstrMostrarTodos As String, ByVal plngIDCia As Integer, ByVal pstrGrupo As String, ByVal pstrNombre As String, ByVal pstrDescripcion As String, ByVal plogUsuario As Integer, ByVal pstrTipoResultado As String, ByVal pstrTipoProceso As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_EjecutarScripts_Scripts_Consultar(pintIdScript, pstrMostrarTodos, plngIDCia, pstrGrupo, pstrNombre, pstrDescripcion, plogUsuario, pstrTipoResultado, pstrTipoProceso, pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "consultarScripts"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "consultarScripts")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consultar los parámetros de un script
    ''' </summary>
    ''' 
    Public Function consultarParametrosScript(ByVal pintIdScript As Integer, ByVal plngIDCia As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2Parametros)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_EjecutarScripts_Scripts_Parametros(pintIdScript, plngIDCia, pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "consultarParametrosScripts"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "consultarParametrosScript")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarTablaInstalacion(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Instalacio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Instalacion_Consultar(DemeInfoSesion(pstrUsuario, "ConsultarTablaInstalacion"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarTablaInstalacion")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarFechaServidor(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DateTime
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim dtmFechaHoraServidor As DateTime = Now
            Me.DataContext.uspCalculosFinancieros_Utilidades_ObtenerFechaServidor(dtmFechaHoraServidor, DemeInfoSesion(pstrUsuario, "ConsultarFechaServidor"), 0)
            Return dtmFechaHoraServidor
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarFechaServidor")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' CCM20151107: Consultar la fecha de cierre de un portafolio
    ''' CCM20151112: Cambiar tipo de dato de DateTime a DateTime?
    ''' </summary>
    Public Function ConsultarFechaCierrePortafolio(ByVal plngIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DateTime?
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim dtmFechaCierre As DateTime? = Now
            Me.DataContext.uspCalculosFinancieros_Utilidades_ObtenerFechaCierre(plngIdComitente, dtmFechaCierre, DemeInfoSesion(pstrUsuario, "ConsultarFechaCierrePortafolio"), 0)
            Return dtmFechaCierre
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarFechaCierrePortafolio")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"

    Public Function filtrarScriptsSync(ByVal pstrFiltro As String, ByVal pstrMostrarTodos As String, ByVal plngIDCia As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2)
        Dim objTask As Task(Of List(Of ScriptsA2)) = Me.filtrarScriptsAsync(pstrFiltro, pstrMostrarTodos, plngIDCia, pstrMaquina, pstrUsuario, pstrUsuarioLlamado, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function filtrarScriptsAsync(ByVal pstrFiltro As String, ByVal pstrMostrarTodos As String, ByVal plngIDCia As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptsA2))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptsA2)) = New TaskCompletionSource(Of List(Of ScriptsA2))()
        objTaskComplete.TrySetResult(filtrarScripts(pstrFiltro, pstrMostrarTodos, plngIDCia, pstrMaquina, pstrUsuario, pstrUsuarioLlamado, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function consultarScriptsSync(ByVal pintIdScript As Integer, ByVal pstrMostrarTodos As String, ByVal plngIDCia As Integer, ByVal pstrGrupo As String, ByVal pstrNombre As String, ByVal pstrDescripcion As String, ByVal plogUsuario As Integer, ByVal pstrTipoResultado As String, ByVal pstrTipoProceso As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2)
        Dim objTask As Task(Of List(Of ScriptsA2)) = Me.consultarScriptsAsync(pintIdScript, pstrMostrarTodos, plngIDCia, pstrGrupo, pstrNombre, pstrDescripcion, plogUsuario, pstrTipoResultado, pstrTipoProceso, pstrMaquina, pstrUsuario, pstrUsuarioLlamado, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function consultarScriptsAsync(ByVal pintIdScript As Integer, ByVal pstrMostrarTodos As String, ByVal plngIDCia As Integer, ByVal pstrGrupo As String, ByVal pstrNombre As String, ByVal pstrDescripcion As String, ByVal plogUsuario As Integer, ByVal pstrTipoResultado As String, ByVal pstrTipoProceso As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptsA2))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptsA2)) = New TaskCompletionSource(Of List(Of ScriptsA2))()
        objTaskComplete.TrySetResult(consultarScripts(pintIdScript, pstrMostrarTodos, plngIDCia, pstrGrupo, pstrNombre, pstrDescripcion, plogUsuario, pstrTipoResultado, pstrTipoProceso, pstrMaquina, pstrUsuario, pstrUsuarioLlamado, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function consultarParametrosScriptSync(ByVal pintIdScript As Integer, ByVal plngIDCia As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2Parametros)
        Dim objTask As Task(Of List(Of ScriptsA2Parametros)) = Me.consultarParametrosScriptAsync(pintIdScript, plngIDCia, pstrMaquina, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function consultarParametrosScriptAsync(ByVal pintIdScript As Integer, ByVal plngIDCia As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptsA2Parametros))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptsA2Parametros)) = New TaskCompletionSource(Of List(Of ScriptsA2Parametros))()
        objTaskComplete.TrySetResult(consultarParametrosScript(pintIdScript, plngIDCia, pstrMaquina, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarTablaInstalacionSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Instalacio)
        Dim objTask As Task(Of List(Of Instalacio)) = Me.ConsultarTablaInstalacionAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarTablaInstalacionAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Instalacio))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Instalacio)) = New TaskCompletionSource(Of List(Of Instalacio))()
        objTaskComplete.TrySetResult(ConsultarTablaInstalacion(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarFechaServidorSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DateTime
        Dim objTask As Task(Of DateTime) = Me.ConsultarFechaServidorAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarFechaServidorAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of DateTime)
        Dim objTaskComplete As TaskCompletionSource(Of DateTime) = New TaskCompletionSource(Of DateTime)()
        objTaskComplete.TrySetResult(ConsultarFechaServidor(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' CCM20151107: Consultar la fecha de cierre de un portafolio
    ''' </summary>
    Public Function ConsultarFechaCierrePortafolioSync(ByVal plngIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DateTime?
        Dim objTask As Task(Of DateTime?) = Me.ConsultarFechaCierrePortafolioAsync(plngIdComitente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    ''' <summary>
    ''' CCM20151107: Consultar la fecha de cierre de un portafolio
    ''' </summary>
    Private Function ConsultarFechaCierrePortafolioAsync(ByVal plngIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of DateTime?)
        Dim objTaskComplete As TaskCompletionSource(Of DateTime?) = New TaskCompletionSource(Of DateTime?)()
        objTaskComplete.TrySetResult(ConsultarFechaCierrePortafolio(plngIdComitente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Ejecutar script"

    Public Function ejecutarScript(ByVal pintIdCompania As Integer, ByVal pintIdScript As Integer, ByVal pstrNombreScript As String, ByVal pstrParametros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of MensajesProceso)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return (ejecutarScriptBD(pintIdCompania, pintIdScript, pstrNombreScript, pstrParametros, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScript")
            Return Nothing
        End Try

    End Function

    Public Function ejecutarScriptSync(ByVal pintIdCompania As Integer, ByVal pintIdScript As Integer, ByVal pstrNombreScript As String, ByVal pstrParametros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of MensajesProceso)
        Dim objTask As Task(Of List(Of MensajesProceso)) = Me.ejecutarScriptAsync(pintIdCompania, pintIdScript, pstrNombreScript, pstrParametros, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ejecutarScriptAsync(ByVal pintIdCompania As Integer, ByVal pintIdScript As Integer, ByVal pstrNombreScript As String, ByVal pstrParametros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of MensajesProceso))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of MensajesProceso)) = New TaskCompletionSource(Of List(Of MensajesProceso))()
        objTaskComplete.TrySetResult(ejecutarScript(pintIdCompania, pintIdScript, pstrNombreScript, pstrParametros, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Private Function ejecutarScriptBD(ByVal pintIdCompania As Integer, ByVal pintIdScript As Integer, ByVal pstrNombreScript As String, ByVal pstrParametros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of MensajesProceso)

        Dim strCarpeta As String = "EjecutarScripts"
        Dim strNombreArchivo As String
        Dim strExtensionArchivo As String
        Dim strSeparador As String
        Dim strConexion As String
        Dim strResulatado As String = String.Empty

        Dim objDatos As List(Of MensajesProceso)
        Dim objScripts As List(Of ScriptsA2)
        Dim objRes As MensajesProceso = Nothing

        Try
            objRes = New MensajesProceso
            objRes.Linea = String.Empty
            objRes.URLArchivo = String.Empty
            objRes.TipoLinea = String.Empty
            objRes.NombreArchivo = String.Empty

            objScripts = Me.DataContext.usp_EjecutarScripts_Scripts_Consultar(pintIdScript, String.Empty, pintIdCompania, String.Empty, String.Empty, String.Empty, 0, String.Empty, String.Empty, pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "ejecutarScriptBD"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList

            If objScripts.Count = 0 Then
                objRes.Linea = "El script que se debe ejecutar (" & pstrNombreScript & ") no está registrado en el sistema."
                objRes.TipoLinea = "INCONSISTENCIA"
            Else
                strConexion = Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString



                If IsNothing(objScripts.Item(0).TipoResultado) OrElse LTrim(objScripts.Item(0).TipoResultado).Equals(String.Empty) Then
                    strExtensionArchivo = String.Empty
                Else
                    strExtensionArchivo = objScripts.Item(0).TipoResultado
                End If

                strSeparador = objScripts.Item(0).Separador

                strResulatado = ejecutarProcedimientoScript(strConexion, pintIdCompania, pintIdScript, pstrNombreScript, strCarpeta, pstrParametros, strSeparador, strExtensionArchivo, pstrMaquina, pstrUsuario, pstrInfoConexion, objRes)

                If strResulatado.Equals(String.Empty) Then
                    ' El script no retorna ningún resultado
                    objRes.Linea = "El script se ejecutó exitosamente."
                ElseIf Left(strResulatado, 20).ToUpper.IndexOf(SeparadorMensaje) >= 0 Then
                    ' El script no retorna ningún resultado
                    objRes.Linea = strResulatado.Substring(Len(SeparadorMensaje))
                    objRes.TipoLinea = TiposResultadoScript.Mensaje.ToString.ToUpper
                Else
                    ' El script retorna un archivo Excel o plano
                    If Left(strResulatado, 20).ToUpper.IndexOf(SeparadorError) < 0 Then
                        objRes.Linea = "Se generó el archivo con el resultado de la ejecución del script. Para descargar el archivo haga clic en el nombre del archivo."
                        objRes.URLArchivo = strResulatado & "?IdGen=" & CStr(Math.Round(CDbl(Now().ToOADate()) * 100000000, 0))
                        'El nombre del archivo se actualiza en el procedimiento ejecutarProcedimientoScript, esto es con el fin de poder alterar el nombre del archivo desde el procedimiento para generar nombres de archivos especificos
                        'objRes.NombreArchivo = strNombreArchivo
                    Else
                        objRes.Linea = "El script presentó un problema durante su ejecución. " & strResulatado
                        objRes.TipoLinea = "INCONSISTENCIA"
                    End If
                End If
            End If

            objDatos = New List(Of MensajesProceso)
            objDatos.Add(objRes)

            Return objDatos
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScriptBD")
            Return Nothing
        End Try

    End Function
    ''' <summary>
    ''' Genera el nombre del archivo de salida del ejecutar scripts
    ''' </summary>
    ''' <param name="objScripts"></param>
    Private Shared Function GenerarNombreArchivo(objScripts As List(Of ScriptsA2)) As String
        Dim intLongitud As Integer
        Dim strNombreArchivo As String

        strNombreArchivo = objScripts.Item(0).NombreArchivo
        If strNombreArchivo Is Nothing OrElse strNombreArchivo.Trim().Equals(String.Empty) Then
            If objScripts.Item(0).Nombre.Length > 30 Then
                intLongitud = 30
            Else
                intLongitud = objScripts.Item(0).Nombre.Length
            End If

            strNombreArchivo = objScripts.Item(0).Nombre.Substring(0, intLongitud) & "_" &
                                                Now.Year().ToString & Right("00" & Now.Month.ToString, 2) & Right("00" & Now.Day.ToString, 2) & "_" &
                                                Right("00" & Now.Hour.ToString, 2) & Right("00" & Now.Minute.ToString, 2) & Right("00" & Now.Second.ToString, 2) & Right("000" & Now.Millisecond.ToString, 3)
        Else
            If Not IsNothing(objScripts.Item(0).UsarSufijoFechaHora) AndAlso objScripts.Item(0).UsarSufijoFechaHora Then
                strNombreArchivo &= "_" &
                                Now.Year().ToString & Right("00" & Now.Month.ToString, 2) & Right("00" & Now.Day.ToString, 2) & "_" &
                                Right("00" & Now.Hour.ToString, 2) & Right("00" & Now.Minute.ToString, 2) & Right("00" & Now.Second.ToString, 2) & Right("000" & Now.Millisecond.ToString, 3)

            End If
        End If
        strNombreArchivo = strNombreArchivo.Replace(" ", "_")
        strNombreArchivo = ExtraerTildesTexto(strNombreArchivo)
        Return strNombreArchivo
    End Function

    Private Function ejecutarProcedimientoScript(ByVal pstrConexion As String, ByVal pintIdCompania As Integer, ByVal pintIDScript As Integer, ByVal pstrNombreScript As String,
                                                 ByVal pstrCarpetaProceso As String, ByVal pstrParametros As String, ByVal pstrSeparador As String,
                                                 ByVal pstrExtensionArchivo As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByRef objRes As MensajesProceso) As String

        Dim strProcedimiento As String = "[dbo].[uspEjecutarScripts_EjecutarProcedimiento]"
        Dim intTipoSeparador As TipoSeparadorArchivo = Nothing
        Dim strTipoSeparador As String = String.Empty
        Dim strConexionVlrs As String = String.Empty
        Dim strSeparadorVlrs As String = String.Empty
        Dim strResultado As String = String.Empty
        Dim intLongitudVlrs As Integer = 0
        Dim objScripts As List(Of ScriptsA2)
        Dim strNombreArchivo As String

        Try


            '// OJO-CCM: COmentado mientras se sabe que hacer
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrCarpetaProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(pstrConexion)

            If pstrSeparador = TipoSeparadorArchivo.TAB.ToString Then
                strTipoSeparador = vbTab
            ElseIf pstrSeparador = TipoSeparadorArchivo.COMA.ToString Then
                strTipoSeparador = ","
            ElseIf pstrSeparador = TipoSeparadorArchivo.PUNTOYCOMA.ToString Then
                strTipoSeparador = ";"
            ElseIf pstrSeparador = TipoSeparadorArchivo.PIPE.ToString Then
                strTipoSeparador = "|"
            Else
                strTipoSeparador = pstrSeparador
            End If

            strConexionVlrs = pstrConexion
            leerDatosConexion(strSeparadorVlrs, strConexionVlrs, intLongitudVlrs)

            Dim objListaParametros As New List(Of SqlParameter)
            objListaParametros.Add(CrearSQLParameter("pintIdCompania", pintIdCompania, SqlDbType.Int))
            objListaParametros.Add(CrearSQLParameter("pintIdScript", pintIDScript, SqlDbType.Int))
            objListaParametros.Add(CrearSQLParameter("pstrNombreScript", pstrNombreScript, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrParametros", pstrParametros, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrValoresArchivo", strConexionVlrs, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrSeparadorValores", strSeparadorVlrs, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintLongitud", intLongitudVlrs, SqlDbType.Int))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "ejecutarProcedimientoScript"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", Constantes.ERROR_PERSONALIZADO_SQLSERVER, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProcedimiento, objListaParametros, 60)

            'JERF20200421 Se vuelve recuperar la información del script para recuperar el nombre del archivo, esto es para poder generar nombres personalizados, como por ejemplo un archivo con los datos del cliente
            objScripts = Me.DataContext.usp_EjecutarScripts_Scripts_Consultar(pintIDScript, String.Empty, pintIdCompania, String.Empty, String.Empty, String.Empty, 0, String.Empty, String.Empty, pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "ejecutarScriptBD"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            strNombreArchivo = GenerarNombreArchivo(objScripts)
            'Actualiza el objeto de resultado con el nombre del archivo
            objRes.NombreArchivo = strNombreArchivo

            Dim strArchivo As String = String.Empty

            If TiposResultadoScript.Ninguno.ToString.ToUpper = pstrExtensionArchivo.ToUpper Then
                strResultado = String.Empty
            ElseIf TiposResultadoScript.Mensaje.ToString.ToUpper = pstrExtensionArchivo.ToUpper Then
                strResultado = SeparadorMensaje
                If objDatosConsulta.Tables(0).Rows.Count > 0 Then
                    strResultado &= objDatosConsulta.Tables(0).Rows(0).Item(0).ToString
                Else
                    strResultado &= "El proceso finalizó correctamente. No retornó datos ni mensajes informativos."
                End If
            Else
                '// OJO-CCM: COmentado mientras se sabe que hacer
                If pstrExtensionArchivo.ToUpper = TiposResultadoScript.Excel.ToString.ToUpper Then
                    strArchivo = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, strNombreArchivo)
                Else
                    strArchivo = GenerarTextoPlano(objDatosConsulta.Tables(0), objRutas.RutaArchivosLocal, strNombreArchivo, pstrExtensionArchivo, strTipoSeparador)
                End If

                If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                    objRutas.RutaWeb = objRutas.RutaWeb & "/"
                End If

                strResultado = objRutas.RutaCompartidaOWeb() & strArchivo
                '// FIN OJO-CCM: COmentado mientras se sabe que hacer
            End If

            Return (strResultado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarProcedimientoScript")

            Return (SeparadorError & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Extraer de la conexión la contraseña del usuario de base de datos
    ''' </summary>
    ''' 
    Private Sub leerDatosConexion(ByRef pstrSeparador As String, ByRef pstrConexion As String, ByRef pintLongitud As Integer)
        Dim intPos As Integer = -1
        Dim intPosFin As Integer = -1
        Dim intMaxPos As Integer = -1
        Dim intLongPwd As Integer = 0
        Dim dblValor As Double
        Dim vntVector As String() = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}

        Try
            ' Extraer la contraseña del usuario de la conexión y enviarla como parámetro para poder realizar la conexión en el procedimiento 
            ' utilizando el procedimiento OpenRowset
            If Not Me.DataContext Is Nothing Then
                pstrConexion = A2Utilidades.Cifrar.descifrar(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString)

                intMaxPos = vntVector.Length

                Randomize()
                dblValor = Rnd()
                pintLongitud = CInt(CDbl(Right((dblValor - CInt(dblValor)).ToString, 2)) Mod intMaxPos)
                pstrSeparador = vntVector(pintLongitud)
                dblValor = Rnd()
                pintLongitud = CInt(CDbl(Right((dblValor - CInt(dblValor)).ToString, 2)) Mod intMaxPos)
                pstrSeparador &= vntVector(pintLongitud)
                dblValor = Rnd()
                pintLongitud = CInt(CDbl(Right((dblValor - CInt(dblValor)).ToString, 2)) Mod intMaxPos)
                pstrSeparador &= vntVector(pintLongitud)
                pintLongitud = 3

                intPos = pstrConexion.ToLower.IndexOf("password")
                If intPos < 0 Then
                    intPos = pstrConexion.ToLower.IndexOf("pwd")
                End If

                If intPos >= 0 Then
                    intPos = pstrConexion.IndexOf("=", intPos) + 1
                    If intPos >= 0 Then
                        intPosFin = pstrConexion.IndexOf(";", intPos)
                        If intPosFin >= 0 Then
                            intLongPwd = pstrConexion.IndexOf(";", intPos) - intPos
                        Else
                            intLongPwd = pstrConexion.Length - intPos
                        End If
                        pstrConexion = pstrConexion.Substring(intPos, intLongPwd)
                    Else
                        pstrConexion = String.Empty
                    End If
                Else
                    pstrConexion = String.Empty
                End If

                pstrConexion = A2Utilidades.Cifrar.cifrar(pstrConexion, pstrSeparador)
            End If
        Catch ex As Exception
            Throw New ApplicationException("Se presentó un error al extraer la información de conectividad necesaria para generar el archivo solicitado." & vbNewLine & vbNewLine & ex.Message)
        End Try

    End Sub

    Private Function GenerarExcel(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try

            Dim strExtensionNombreArchivo As String = ".xlsx"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook
            Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(0)
            Dim objRange As SpreadsheetGear.IRange = objWorkSheet.Cells(0, 0)
            Dim strAgrupadorMiles As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberGroupSeparator
            Dim strSeparadorDecimal As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberDecimalSeparator
            Dim strFormatoNumerosExcel As String = "#" & strAgrupadorMiles & "##0" & strSeparadorDecimal & "00"
            Dim strFormatoFechasExcel As String = "dd/mm/yyyy"
            Dim strBackSlash As String = "\"

            Dim strNombreArchivo As String
            Dim strRutaCompletaArchivo As String

            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            strNombreArchivo = pstrNombreArchivo & strExtensionNombreArchivo
            strRutaCompletaArchivo = pstrRuta & strNombreArchivo


            objRange.CopyFromDataTable(pobjDatos, SpreadsheetGear.Data.SetDataFlags.None)


            For I As Integer = 0 To pobjDatos.Columns.Count - 1

                Select Case pobjDatos.Columns(I).DataType
                    Case GetType(Decimal), GetType(Double)
                        objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoNumerosExcel
                    Case GetType(Date)
                        objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoFechasExcel
                End Select

            Next

            objWorkSheet.UsedRange.Columns.AutoFit()

            objWorkSheet.UsedRange.Columns.AutoFilter()

            objWorkSheet.Cells(0, 0).EntireRow.Interior.Color = SpreadsheetGear.Color.FromArgb(51, 51, 153)

            objWorkSheet.Cells(0, 0).EntireRow.Font.Bold = True

            objWorkSheet.Cells(0, 0).EntireRow.Font.Color = SpreadsheetGear.Color.FromArgb(255, 255, 255)

            objWorkbook.SaveAs(strRutaCompletaArchivo, SpreadsheetGear.FileFormat.OpenXMLWorkbook)

            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

    ''' <summary>
    ''' Convertir un DataSet a un libro de Excel con cada DataTable como una hoja
    ''' </summary>
    ''' <param name="pobjDatos">Dataset con los datos a exportar</param>
    ''' <param name="pstrRuta">Ruta</param>
    ''' <param name="pstrNombreArchivo">Nombre del archivo Excel resultante</param>
    ''' <param name="strListaNombreHojas">Estable el nombre de las hojas separado por coma</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerarExcel(pobjDatos As DataSet, pstrRuta As String, pstrNombreArchivo As String, Optional ByVal strListaNombreHojas As String = "") As String
        Try

            Dim strExtensionNombreArchivo As String = ".xlsx"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook

            Dim strAgrupadorMiles As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberGroupSeparator
            Dim strSeparadorDecimal As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberDecimalSeparator
            Dim strFormatoNumerosExcel As String = "#" & strAgrupadorMiles & "##0" & strSeparadorDecimal & "00"
            Dim strFormatoFechasExcel As String = "dd/mm/yyyy"
            Dim strBackSlash As String = "\"
            Dim intHoja As Integer = 0

            Dim strNombreArchivo As String
            Dim strRutaCompletaArchivo As String
            Dim NombresHojas() As String

            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            strNombreArchivo = pstrNombreArchivo & strExtensionNombreArchivo
            strRutaCompletaArchivo = pstrRuta & strNombreArchivo

            'Convertir la cadena en array
            If strListaNombreHojas <> "" Then
                NombresHojas = strListaNombreHojas.Split(",")
            End If

            For Each objTabla In pobjDatos.Tables

                If pobjDatos.Tables.Count > (intHoja + 1) Then
                    objWorkbook.Worksheets.Add()
                End If

                Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(intHoja)

                If Not IsNothing(NombresHojas) Then
                    If NombresHojas.Count >= (intHoja + 1) Then
                        objWorkSheet.Name = NombresHojas(intHoja)
                    End If
                End If

                Dim objRange As SpreadsheetGear.IRange = objWorkSheet.Cells(0, 0)
                objRange.CopyFromDataTable(objTabla, SpreadsheetGear.Data.SetDataFlags.None)

                For I As Integer = 0 To objTabla.Columns.Count - 1

                    Select Case objTabla.Columns(I).DataType
                        Case GetType(Decimal), GetType(Double)
                            objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoNumerosExcel
                        Case GetType(Date)
                            objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoFechasExcel
                    End Select

                Next

                objWorkSheet.UsedRange.Columns.AutoFit()

                objWorkSheet.UsedRange.Columns.AutoFilter()

                objWorkSheet.Cells(0, 0).EntireRow.Interior.Color = SpreadsheetGear.Color.FromArgb(51, 51, 153)

                objWorkSheet.Cells(0, 0).EntireRow.Font.Bold = True

                objWorkSheet.Cells(0, 0).EntireRow.Font.Color = SpreadsheetGear.Color.FromArgb(255, 255, 255)

                intHoja += 1
            Next

            objWorkbook.SaveAs(strRutaCompletaArchivo, SpreadsheetGear.FileFormat.OpenXMLWorkbook)

            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

    Private Function GenerarTextoPlano(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, ByVal pstrExtension As String, pstrSeparador As String) As String

        Try
            Dim strBackSlash As String = "\"

            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            If Right(pstrNombreArchivo, 1) <> "." Then
                pstrNombreArchivo = pstrNombreArchivo & IIf(pstrExtension.Equals(String.Empty), String.Empty, ".").ToString() '--CCM201404: Validar si la extensión viene en blanco para no incluir el punto
            End If

            Dim strNombreArchivo As String = pstrNombreArchivo & pstrExtension
            Dim strRutaArchivoCompleta As String = pstrRuta & pstrNombreArchivo & pstrExtension
            'Nuevo objeto StreamWriter, para acceder al fichero y poder guardar las líneas  
            Using archivo As StreamWriter = New StreamWriter(strRutaArchivoCompleta)

                ' variable para almacenar la línea actual del dataview  
                Dim linea As String = String.Empty

                With pobjDatos
                    ' Recorrer las filas del dataGridView  
                    For fila As Integer = 0 To .Rows.Count - 1
                        ' vaciar la línea  
                        linea = String.Empty

                        ' Recorrer la cantidad de columnas que contiene el dataGridView  
                        For col As Integer = 0 To .Columns.Count - 1
                            ' Almacenar el valor de toda la fila , y cada campo separado por el delimitador  
                            linea = linea & .Rows(fila).Item(col).ToString & pstrSeparador
                        Next

                        ' Escribir una línea con el método WriteLine  
                        With archivo

                            ' eliminar el último caracter de la cadena siempre y cuando corresponda al separador (";" , "," , etc)
                            If pstrSeparador = linea.Substring(linea.Length - 1) And pstrSeparador <> "" Then 'JEPM20160330
                                linea = linea.Remove(linea.Length - 1).ToString
                            End If

                            ' escribir la fila  
                            .WriteLine(linea.ToString)
                        End With
                    Next
                End With
            End Using

            ' Abrir con Process.Start el archivo de texto  
            'Process.Start(strRutaArchivoCompleta)
            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

    ''' <summary>
    ''' Convertir cada datatable de un DataSet un archivo de texto independiente 
    ''' JEPM20160906
    ''' </summary>
    ''' <param name="pobjDatos">Dataset con los datos a exportar</param>
    ''' <param name="pstrRuta">Ruta</param>
    ''' <param name="pstrListaArchivosExporacion">Lista de los nombres de los archivos planos separados por coma</param>
    ''' <param name="pstrExtension">Extension del plano</param>
    ''' <param name="pstrSeparador">Separador</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por:     Javier Eduardo Pardo Moreno (Alcuadrado S.A.)
    ''' Descripción:        Se nombran los archivos según 
    ''' Fecha:              Marzo 28/2016
    ''' Id del cambio:      JEPM20160328
    ''' </history>
    ''' <history>
    ''' Modificado por:     Javier Eduardo Pardo Moreno (Alcuadrado S.A.)
    ''' Descripción:        Se establece la codificación de la exportación de planos como Default para que codifique el archivo como ANSI
    ''' Fecha:              Abril 06/2016
    ''' Id del cambio:      JEPM20180406
    ''' </history>
    ''' <history>
    ''' Modificado por:     Carlos Ferney Medina Medina
    ''' Descripción:        Se realiza el control para que no se genere información duplicada en los planos
    ''' Fecha:              2018-05-04
    ''' Id del cambio:      CFMM20180504
    ''' </history>
    Private Function GenerarTextoPlano(pobjDatos As DataSet, pstrRuta As String, pstrListaArchivosExporacion As String, ByVal pstrExtension As String, pstrSeparador As String) As List(Of String)

        Try
            Dim strBackSlash As String = "\"
            Dim intArchivo As Integer = 0
            Dim ListaArchivosExporacion() As String = Nothing
            Dim strRutasArchivosFinales As List(Of String) = New List(Of String)
            Dim pstrNombreArchivoFinal As String

            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            'Convertir la cadena en array
            If pstrListaArchivosExporacion <> "" Then
                ListaArchivosExporacion = pstrListaArchivosExporacion.Split(",")
            End If

            For Each objTabla In pobjDatos.Tables

                pstrNombreArchivoFinal = String.Empty

                If Not IsNothing(ListaArchivosExporacion) Then
                    If ListaArchivosExporacion.Count >= (intArchivo + 1) Then
                        'Nombre de archivo a exportar 
                        pstrNombreArchivoFinal = ListaArchivosExporacion(intArchivo) 'JEPM20180205 Se quita el nombre del formato para controlar desde SQL  & "_" & SufijosArchivos(intArchivo)
                    End If
                End If

                'añadir el punto a la extension
                If Right(pstrNombreArchivoFinal, 1) <> "." Then
                    pstrNombreArchivoFinal = pstrNombreArchivoFinal & IIf(pstrExtension.Equals(String.Empty), String.Empty, ".").ToString() '--CCM201404: Validar si la extensión viene en blanco para no incluir el punto
                End If

                pstrNombreArchivoFinal = pstrNombreArchivoFinal & pstrExtension
                Dim strRutaArchivoCompleta As String = pstrRuta & pstrNombreArchivoFinal


                'Nuevo objeto StreamWriter, para acceder al fichero y poder guardar las líneas  
                Using archivo As StreamWriter = New StreamWriter(strRutaArchivoCompleta, False, Encoding.Default) 'JEPM20180406, CFMM20180504 (se cambia True por False)

                    ' variable para almacenar la línea actual del dataview  
                    Dim linea As String = String.Empty

                    With objTabla
                        ' Recorrer las filas del dataGridView  
                        For fila As Integer = 0 To .Rows.Count - 1
                            ' vaciar la línea  
                            linea = String.Empty

                            ' Recorrer la cantidad de columnas que contiene el dataGridView  
                            For col As Integer = 0 To .Columns.Count - 1
                                ' Almacenar el valor de toda la fila , y cada campo separado por el delimitador  
                                linea = linea & .Rows(fila).Item(col).ToString & pstrSeparador
                            Next

                            ' Escribir una línea con el método WriteLine  
                            With archivo

                                ' eliminar el último caracter de la cadena siempre y cuando corresponda al separador (";" , "," , etc)
                                If pstrSeparador = linea.Substring(linea.Length - 1) And pstrSeparador <> "" Then 'JEPM20160330
                                    linea = linea.Remove(linea.Length - 1).ToString
                                End If

                                ' escribir la fila  
                                .WriteLine(linea.ToString)
                            End With
                        Next
                    End With
                End Using

                'añadir la ruta del archivo generado
                strRutasArchivosFinales.Add(pstrNombreArchivoFinal)

                intArchivo += 1
            Next

            ' Abrir con Process.Start el archivo de texto  
            'Process.Start(strRutaArchivoCompleta)

            Return strRutasArchivosFinales
        Catch ex As Exception
            Throw
            Return New List(Of String)
        End Try

    End Function

    ''' <history>
    ''' Fecha:          2015/10/19
    ''' Modificado por: Germán Arbey González Osorio
    ''' Descripción:    Se asigna 0 (cero) al parametro CommandTimeout para evitar inconsistencias de timeout en la pantalla de ejecutar scripts
    ''' </history>
    Private Function RetornarDataTable(ByVal pstrConexion As String, ByVal pstrNombreProcedimiento As String, ByVal pobjParametros As List(Of SqlParameter)) As DataTable

        Dim objDatos As DataTable = Nothing
        Dim objCommand As SqlCommand = Nothing
        Dim objDataReader As SqlDataReader

        Try
            objCommand = New SqlCommand()
            objCommand.Connection = New SqlConnection(pstrConexion)
            objCommand.CommandTimeout = 0
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.CommandText = pstrNombreProcedimiento

            For Each li In pobjParametros
                objCommand.Parameters.Add(li)
            Next

            If objCommand.Connection.State <> ConnectionState.Open Then
                objCommand.Connection.Open()
            End If

            objDataReader = objCommand.ExecuteReader

            objDatos = New DataTable()

            objDatos.Load(objDataReader)

            objDataReader.Close()

        Catch ex As Exception
            If objCommand IsNot Nothing AndAlso objCommand.Connection IsNot Nothing Then
                If objCommand.Connection.State <> ConnectionState.Closed Then
                    objCommand.Connection.Close()
                End If
            End If
            Throw
        End Try

        Return objDatos

    End Function

    Public Function ejecutarScriptDiseno_Consultar(ByVal pintIDScript As Integer, ByVal pstrUsuarioAPP As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2Diseno)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.usp_EjecutarScripts_Diseno_Consultar(pintIDScript, pstrUsuarioAPP, pstrUsuario, DemeInfoSesion(pstrUsuario, "ejecutarScriptDiseno_Consultar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScriptDiseno_Consultar")
            Return Nothing
        End Try
    End Function
    Public Function ejecutarScriptDiseno_ConsultarSync(ByVal pintIDScript As Integer, ByVal pstrUsuarioAPP As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2Diseno)
        Dim objTask As Task(Of List(Of ScriptsA2Diseno)) = Me.ejecutarScriptDiseno_ConsultarAsync(pintIDScript, pstrUsuarioAPP, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ejecutarScriptDiseno_ConsultarAsync(ByVal pintIDScript As Integer, ByVal pstrUsuarioAPP As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptsA2Diseno))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptsA2Diseno)) = New TaskCompletionSource(Of List(Of ScriptsA2Diseno))()
        objTaskComplete.TrySetResult(ejecutarScriptDiseno_Consultar(pintIDScript, pstrUsuarioAPP, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function ejecutarScriptDiseno_Actualizar(ByVal pintIDScriptDiseno As Integer, ByVal pintIDScript As Integer, ByVal pstrDiseno As String, ByVal pstrDescripcion As String, ByVal pstrFiltrosDiseno As String, ByVal pstrUsuarioAPP As String, ByVal pstrUsuarioCreacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2ResultadosGenericos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.usp_EjecutarScripts_Diseno_Actualizar(pintIDScriptDiseno, pintIDScript, pstrDiseno, pstrDescripcion, pstrFiltrosDiseno, pstrUsuarioAPP, pstrUsuarioCreacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ejecutarScriptDiseno_Actualizar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScriptDiseno_Actualizar")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function ejecutarScriptDiseno_ActualizarSync(ByVal pintIDScriptDiseno As Integer, ByVal pintIDScript As Integer, ByVal pstrDiseno As String, ByVal pstrDescripcion As String, ByVal pstrFiltrosDiseno As String, ByVal pstrUsuarioAPP As String, ByVal pstrUsuarioCreacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2ResultadosGenericos)
        Dim objTask As Task(Of List(Of ScriptsA2ResultadosGenericos)) = Me.ejecutarScriptDiseno_ActualizarAsync(pintIDScriptDiseno, pintIDScript, pstrDiseno, pstrDescripcion, pstrFiltrosDiseno, pstrUsuarioAPP, pstrUsuarioCreacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ejecutarScriptDiseno_ActualizarAsync(ByVal pintIDScriptDiseno As Integer, ByVal pintIDScript As Integer, ByVal pstrDiseno As String, ByVal pstrDescripcion As String, ByVal pstrFiltrosDiseno As String, ByVal pstrUsuarioAPP As String, ByVal pstrUsuarioCreacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptsA2ResultadosGenericos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptsA2ResultadosGenericos)) = New TaskCompletionSource(Of List(Of ScriptsA2ResultadosGenericos))()
        objTaskComplete.TrySetResult(ejecutarScriptDiseno_Actualizar(pintIDScriptDiseno, pintIDScript, pstrDiseno, pstrDescripcion, pstrFiltrosDiseno, pstrUsuarioAPP, pstrUsuarioCreacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ejecutarScriptDiseno_Eliminar(ByVal pintIDScriptDiseno As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2ResultadosGenericos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.usp_EjecutarScripts_Diseno_Eliminar(pintIDScriptDiseno, pstrUsuario, DemeInfoSesion(pstrUsuario, "ejecutarScriptDiseno_Eliminar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScriptDiseno_Eliminar")
            Return Nothing
        End Try
    End Function
    Public Function ejecutarScriptDiseno_EliminarSync(ByVal pintIDScriptDiseno As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2ResultadosGenericos)
        Dim objTask As Task(Of List(Of ScriptsA2ResultadosGenericos)) = Me.ejecutarScriptDiseno_EliminarAsync(pintIDScriptDiseno, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ejecutarScriptDiseno_EliminarAsync(ByVal pintIDScriptDiseno As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptsA2ResultadosGenericos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptsA2ResultadosGenericos)) = New TaskCompletionSource(Of List(Of ScriptsA2ResultadosGenericos))()
        objTaskComplete.TrySetResult(ejecutarScriptDiseno_Eliminar(pintIDScriptDiseno, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ejecutarScriptDiseno_ValidarPermisos(ByVal pintIDScript As Integer, ByVal pstrUsuarioApp As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim logAdministradorScript As Boolean = False
            Me.DataContext.usp_EjecutarScripts_Diseno_ValidarPermisos(pintIDScript, pstrUsuarioApp, pstrUsuario, DemeInfoSesion(pstrUsuario, "ejecutarScriptDiseno_ValidarPermisos"), Constantes.ERROR_PERSONALIZADO_SQLSERVER, logAdministradorScript)
            Return logAdministradorScript
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScriptDiseno_ValidarPermisos")
            Return Nothing
        End Try
    End Function
    Public Function ejecutarScriptDiseno_ValidarPermisosSync(ByVal pintIDScript As Integer, ByVal pstrUsuarioApp As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Dim objTask As Task(Of Boolean) = Me.ejecutarScriptDiseno_ValidarPermisosAsync(pintIDScript, pstrUsuarioApp, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ejecutarScriptDiseno_ValidarPermisosAsync(ByVal pintIDScript As Integer, ByVal pstrUsuarioApp As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)
        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(ejecutarScriptDiseno_ValidarPermisos(pintIDScript, pstrUsuarioApp, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ejecutarScriptDiseno_ConsultarPrincipales(ByVal pintIDScript As Integer, ByVal pstrUsuarioApp As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2DisenosPrincipales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.usp_EjecutarScripts_Diseno_ConsultarPrincipales(pintIDScript, pstrUsuarioApp, pstrUsuario, DemeInfoSesion(pstrUsuario, "ejecutarScriptDiseno_ConsultarPrincipales"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScriptDiseno_ConsultarPrincipales")
            Return Nothing
        End Try
    End Function
    Public Function ejecutarScriptDiseno_ConsultarPrincipalesSync(ByVal pintIDScript As Integer, ByVal pstrUsuarioApp As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2DisenosPrincipales)
        Dim objTask As Task(Of List(Of ScriptsA2DisenosPrincipales)) = Me.ejecutarScriptDiseno_ConsultarPrincipalesAsync(pintIDScript, pstrUsuarioApp, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ejecutarScriptDiseno_ConsultarPrincipalesAsync(ByVal pintIDScript As Integer, ByVal pstrUsuarioApp As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptsA2DisenosPrincipales))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptsA2DisenosPrincipales)) = New TaskCompletionSource(Of List(Of ScriptsA2DisenosPrincipales))()
        objTaskComplete.TrySetResult(ejecutarScriptDiseno_ConsultarPrincipales(pintIDScript, pstrUsuarioApp, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ejecutarScriptDiseno_ActualizarFechaSeleccion(ByVal pintIDScriptDiseno As Integer, ByVal pintIDScript As Integer, ByVal pstrUsuarioAPP As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2ResultadosGenericos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.usp_EjecutarScripts_Diseno_ActualizarFechaSeleccion(pintIDScriptDiseno, pintIDScript, pstrUsuarioAPP, pstrUsuario, DemeInfoSesion(pstrUsuario, "ejecutarScriptDiseno_ActualizarFechaSeleccion"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScriptDiseno_ActualizarFechaSeleccion")
            Return Nothing
        End Try
    End Function
    Public Function ejecutarScriptDiseno_ActualizarFechaSeleccionSync(ByVal pintIDScriptDiseno As Integer, ByVal pintIDScript As Integer, ByVal pstrUsuarioAPP As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ScriptsA2ResultadosGenericos)
        Dim objTask As Task(Of List(Of ScriptsA2ResultadosGenericos)) = Me.ejecutarScriptDiseno_ActualizarFechaSeleccionAsync(pintIDScriptDiseno, pintIDScript, pstrUsuarioAPP, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ejecutarScriptDiseno_ActualizarFechaSeleccionAsync(ByVal pintIDScriptDiseno As Integer, ByVal pintIDScript As Integer, ByVal pstrUsuarioAPP As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptsA2ResultadosGenericos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptsA2ResultadosGenericos)) = New TaskCompletionSource(Of List(Of ScriptsA2ResultadosGenericos))()
        objTaskComplete.TrySetResult(ejecutarScriptDiseno_ActualizarFechaSeleccion(pintIDScriptDiseno, pintIDScript, pstrUsuarioAPP, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


#End Region

#Region "Permisos de ejecutar script"

#Region "Usuarios"

#Region "Metodos Asincronicos"

    Private Function Consultar_EjecutarScripts_UsuariosApp(ByVal pintIdScript As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of Usuarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Permisos_EjecutarScripts_UsuarioApp_Consultar(pintIdScript, pstrMaquina, pstrUsuario,
                                                                                       DemeInfoSesion(pstrUsuario, "Consultar_EjecutarScripts_UsuariosApp"),
                                                                                        Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_EjecutarScripts_UsuariosApp")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Metodos Sincronicos"


    Public Function Consultar_EjecutarScripts_UsuariosAppSync(ByVal pintIdScript As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of Usuarios)
        Dim objTask As Task(Of List(Of Usuarios)) = Me.Consultar_EjecutarScripts_UsuariosAppAsync(pintIdScript, pstrMaquina, pstrUsuario, pstrUsuarioLlamado, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function Consultar_EjecutarScripts_UsuariosAppAsync(ByVal pintIdScript As Integer, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Usuarios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Usuarios)) = New TaskCompletionSource(Of List(Of Usuarios))()
        objTaskComplete.TrySetResult(Consultar_EjecutarScripts_UsuariosApp(pintIdScript, pstrMaquina, pstrUsuario, pstrUsuarioLlamado, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


#End Region

#End Region

#Region "Permisos Scripts Por Usuarios"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertScriptXUsuarios(ByVal obj As ScriptXUsuarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InserttblCiaAptMas")
        End Try
    End Sub

    Public Sub UpdateScriptXUsuarios(ByVal obj As ScriptXUsuarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Updateobj")
        End Try
    End Sub

    Public Sub DeleteScriptXUsuarios(ByVal obj As ScriptXUsuarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletetblCiaAptMas")
        End Try
    End Sub

    Public Function ActualizarScriptsPermisos(ByVal pstrIdScriptlista As String, ByVal pstrListaUsuarios As String,
                                            ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Permisos_EjecutarScripts_Actualizar(pstrIdScriptlista, pstrListaUsuarios, pstrMaquina, pstrUsuario,
                                                                      DemeInfoSesion(pstrUsuario, "ActualizarImprimirFacturas"),
                                                                      Constantes.ERROR_PERSONALIZADO_SQLSERVER)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarScriptsPermisos")
            Return Nothing
        End Try
    End Function

#End Region
#Region "Métodos asincrónicos"

    Private Function filtrarScriptXUsuarios(ByVal pstrUsuarioAutorizado As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ScriptXUsuarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Permisos_EjecutarScripts_ScriptsPorUsuario_Filtrar(pstrUsuarioAutorizado, pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "filtrarScriptXUsuarios"),
                                                                                       Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList

            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "filtrarScriptXUsuarios")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Metodos sincronicos"

    Public Function filtrarScriptXUsuariosSync(ByVal pstrUsuarioAutorizado As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ScriptXUsuarios)
        Dim objTask As Task(Of List(Of ScriptXUsuarios)) = Me.filtrarScriptXUsuariosAsync(pstrUsuarioAutorizado, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function filtrarScriptXUsuariosAsync(ByVal pstrUsuarioAutorizado As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ScriptXUsuarios))

        Dim objTaskComplete As TaskCompletionSource(Of List(Of ScriptXUsuarios)) = New TaskCompletionSource(Of List(Of ScriptXUsuarios))()
        objTaskComplete.TrySetResult(filtrarScriptXUsuarios(pstrUsuarioAutorizado, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#End Region

#Region "Metodos Autorizaciones reglas"
    ''' <summary>
    ''' Metodos para las autorizaciones de las reglas
    ''' </summary>
    ''' Creado por       : Yessid Andres Paniagua Pabon (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Enero 15/2016
    ''' Pruebas CB       : Yessid Andrés Paniagua Pabón - Enero 15/2016 - Resultado Ok
    ''' <remarks></remarks>
    Public Function OYDPLUS_ConsultarEstadosDocumento(ByVal pintIDNumeroUnico As Integer, ByVal pintIDDocumento As Integer, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRN_ContadorReglasAutorizacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarEstadosDocumento(pintIDNumeroUnico, pintIDDocumento, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarEstadosDocumento"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarEstadosDocumento")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatetblRN_ContadorReglasAutorizacion(ByVal currentEstado As tblRN_ContadorReglasAutorizacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatetblRN_ContadorReglasAutorizacion")
        End Try
    End Sub
#End Region


#Region "Generación de archivos reportería legal"
    ''' <summary>
    ''' Descripción:    Generar archivos de reportería legal. Se usa actualmente para Proforma(Excel), Plano o Ambos
    ''' Creado por:     Javier Eduardo Pardo Moreno
    ''' Fecha:          2016-03-30
    ''' Id del cambio:  JEPM20160330
    ''' </summary>
    ''' <param name="pstrCarpetaProceso">Ruta de la Carpeta en que se generan los archivos para el usuario</param>
    ''' <param name="pstrListaFormatos">Retorno del foramto en tblLista</param>
    ''' <param name="pdtmFechaProceso">Fecha de proceso</param>
    ''' <param name="pstrTipoCompania">Tipo de compañía PP, FIC, APT o TODAS</param>
    ''' <param name="pstrListaNombreArchivosExporacion">Nombre que se asigna al archivo exportado</param>
    ''' <param name="pstrSeparador">Separador ";" "," "|" etc. Puede venir vacío</param>
    ''' <param name="pstrExtensionArchivo">Retorno de la lista de extensión de archivo</param>
    ''' <param name="pstrMaquina">Máquina del usuario</param>
    ''' <param name="pstrUsuario">Nombre de usuario</param>
    ''' <returns></returns>
    <Query(HasSideEffects:=True)>
    Public Function GenerarArchivoReporteriaLegal(ByVal pstrCarpetaProceso As String, ByVal pstrListaFormatos As String, ByVal pdtmFechaProceso As String,
                                                  ByVal pstrTipoCompania As String, ByVal pstrListaNombreArchivosExporacion As String, ByVal pstrSeparador As String,
                                                  ByVal pstrExtensionArchivo As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String,
                                                  ByVal pintMinutosTimeOut As Integer, ByVal pstrInfoConexion As String, ByVal pstrNombreCircular As String) As List(Of GenerarArchivosReporteriaLegal)

        Dim objRetorno As New List(Of GenerarArchivosReporteriaLegal)
        objRetorno.Add(New GenerarArchivosReporteriaLegal With {.Mensaje = "", .RutaArchivo = "", .Exitoso = False})

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrCarpetaProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[CF].[USPFORMATOS_REPORTERIA_LEGAL]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty

            If pstrSeparador = TipoSeparador.TAB.ToString Then
                strTipoSeparador = vbTab
            ElseIf pstrSeparador = TipoSeparador.COMA.ToString Then
                strTipoSeparador = ","
            ElseIf pstrSeparador = TipoSeparador.PUNTOYCOMA.ToString Then
                strTipoSeparador = ";"
            Else
                strTipoSeparador = pstrSeparador
            End If

            Dim objListaParametros As New List(Of SqlParameter)
            objListaParametros.Add(CrearSQLParameter("pstrnombreformato", pstrListaFormatos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrtipogeneracion", pstrExtensionArchivo.ToLower, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pdtmfechaproceso", pdtmFechaProceso, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrTipoCompania", pstrTipoCompania, SqlDbType.VarChar))

            Dim objSQLPatameterRetorno As New SqlParameter("@pstrMsgValidacion", Data.SqlDbType.VarChar)
            objSQLPatameterRetorno.Direction = Data.ParameterDirection.Output
            objSQLPatameterRetorno.Size = 8000
            objSQLPatameterRetorno.Value = ""
            objListaParametros.Add(objSQLPatameterRetorno)

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros, pintMinutosTimeOut) 'JEPM20170123 Se añade parámetro pintMinutosTimeOut

            'si hay mensaje de validación terminar ejecución
            If objSQLPatameterRetorno.Value <> "" Then

                objRetorno.Clear()

                'Mensaje de validación del sp
                objRetorno.Add(New GenerarArchivosReporteriaLegal With {.NombreArchivo = "",
                                                                        .RutaArchivo = "",
                                                                        .Exitoso = False,
                                                                        .Mensaje = objSQLPatameterRetorno.Value,
                                                                        .ID = 1
                                                                       })

                Return objRetorno

            End If 'If objSQLPatameterRetorno.Value <> "" Then

            'Si no hay datos, retornar el objeto vacío
            If objDatosConsulta.Tables.Count = 0 Then
                Return objRetorno
            End If

            Dim strArchivosPlanos As List(Of String) = New List(Of String)
            Dim strRutaArchivoExcel As String = String.Empty
            Dim strNombreArchivoExcel As String = String.Format("{0}_{1}_({2})", pstrNombreCircular, pdtmFechaProceso, DateTime.Now.ToString("yyyyMMdd HHmmss"))

            If pstrExtensionArchivo = TipoExportacion.PLANO.ToString Then

                'Generar el plano
                strArchivosPlanos = GenerarTextoPlano(objDatosConsulta, objRutas.RutaArchivosLocal, pstrListaNombreArchivosExporacion, "txt", strTipoSeparador)

            ElseIf pstrExtensionArchivo = TipoExportacion.EXCEL.ToString Then
                'Generar Excel
                strRutaArchivoExcel = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, strNombreArchivoExcel, pstrListaFormatos)


                'ElseIf pstrExtensionArchivo = TipoExportacion.AMBOS.ToString Then

                '    'Generar plano primero y luego excel
                '    strArchivosPlanos = GenerarTextoPlano(objDatosConsulta, objRutas.RutaArchivosLocal, pstrListaNombreArchivosExporacion, "txt", strTipoSeparador)

                '    strArchivoExcel = GenerarExcel(objDatosConsulta.Tables(1), objRutas.RutaArchivosLocal, pstrListaFormatos)

                'ElseIf pstrExtensionArchivo = TipoExportacion.EXCELVIEJO.ToString Then

                '    strArchivoExcel = GenerarExcelVersionesViejas(objDatosConsulta.Tables(0), objRutas.RutaArchivosLocal, pstrListaFormatos)
            Else
                strArchivosPlanos = GenerarTextoPlano(objDatosConsulta, objRutas.RutaArchivosLocal, pstrListaNombreArchivosExporacion, "txt", strTipoSeparador)
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            'En caso que sea exportación a plano o excel solamente
            If pstrExtensionArchivo <> TipoExportacion.AMBOS.ToString Then

                Select Case pstrExtensionArchivo
                    Case TipoExportacion.EXCEL.ToString

                        objRetorno.First.NombreArchivo = strNombreArchivoExcel
                        objRetorno.First.RutaArchivo = objRutas.RutaCompartidaOWeb() & strRutaArchivoExcel
                        objRetorno.First.Exitoso = True
                        objRetorno.First.Mensaje = "Generación de archivo exitoso."

                    Case Else 'TipoExportacion.PLANO.ToString

                        objRetorno.Clear()

                        Dim i As Integer = 0

                        For Each strArchivo In strArchivosPlanos

                            objRetorno.Add(New GenerarArchivosReporteriaLegal With {.NombreArchivo = strArchivo,
                                                                        .RutaArchivo = objRutas.RutaCompartidaOWeb() & strArchivo,
                                                                        .Exitoso = True,
                                                                        .Mensaje = "Generación de archivo exitoso.",
                                                                        .ID = i + 1
                                                                       })
                            i = i + 1
                        Next

                End Select


            Else
                'El usuario quiere exportar tanto plano como excel. Para este caso se toma el primer DataTable como Plano y el segundo como Excel

                objRetorno.Clear()

                'Plano 
                'JEPM20160906
                Dim i As Integer = 0

                For Each strArchivo In strArchivosPlanos

                    objRetorno.Add(New GenerarArchivosReporteriaLegal With {.NombreArchivo = strArchivo,
                                                                .RutaArchivo = objRutas.RutaCompartidaOWeb() & strArchivo,
                                                                .Exitoso = True,
                                                                .Mensaje = "Generación de archivo exitoso.",
                                                                .ID = i + 1
                                                               })
                    i = i + 1
                Next

                'Excel
                objRetorno.Add(New GenerarArchivosReporteriaLegal With {.NombreArchivo = strNombreArchivoExcel,
                                                                        .RutaArchivo = objRutas.RutaCompartidaOWeb() & strRutaArchivoExcel,
                                                                        .Exitoso = True,
                                                                        .Mensaje = "Generación de archivo exitoso.",
                                                                        .ID = i
                                                                       })

            End If 'If pstrExtensionArchivo <> TipoExportacion.AMBOS.ToString Then

            Return objRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarArchivoReporteriaLegal")

            objRetorno.First.Exitoso = False
            objRetorno.First.Mensaje = ex.Message
            objRetorno.First.RutaArchivo = String.Empty
            objRetorno.First.NombreArchivo = String.Empty

            Return objRetorno
        End Try
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function GenerarArchivoReporteriaLegalSync(ByVal pstrCarpetaProceso As String, ByVal pstrNombreFormato As String, ByVal pdtmFechaProceso As String, ByVal pstrNombreArchivo As String, ByVal pstrTipoCompania As String, ByVal pstrSeparador As String, ByVal pstrExtensionArchivo As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pintMinutosTimeOut As Integer, ByVal pstrInfoConexion As String, ByVal pstrNombreCircular As String) As List(Of GenerarArchivosReporteriaLegal)
        Dim objTask As Task(Of List(Of GenerarArchivosReporteriaLegal)) = Me.GenerarArchivoReporteriaLegalAsync(pstrCarpetaProceso, pstrNombreFormato, pdtmFechaProceso, pstrTipoCompania, pstrNombreArchivo, pstrSeparador, pstrExtensionArchivo, pstrMaquina, pstrUsuario, pintMinutosTimeOut, pstrInfoConexion, pstrNombreCircular)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function GenerarArchivoReporteriaLegalAsync(ByVal pstrCarpetaProceso As String, ByVal pstrNombreFormato As String, ByVal pdtmFechaProceso As String, ByVal pstrTipoCompania As String, ByVal pstrNombreArchivo As String, ByVal pstrSeparador As String, ByVal pstrExtensionArchivo As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pintMinutosTimeOut As Integer, ByVal pstrInfoConexion As String, ByVal pstrNombreCircular As String) As Task(Of List(Of GenerarArchivosReporteriaLegal))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarArchivosReporteriaLegal)) = New TaskCompletionSource(Of List(Of GenerarArchivosReporteriaLegal))()
        objTaskComplete.TrySetResult(GenerarArchivoReporteriaLegal(pstrCarpetaProceso, pstrNombreFormato, pdtmFechaProceso, pstrTipoCompania, pstrNombreArchivo, pstrSeparador, pstrExtensionArchivo, pstrMaquina, pstrUsuario, pintMinutosTimeOut, pstrInfoConexion, pstrNombreCircular))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Ejecutar Script Cierre Pasivo"
    Public Function ejecutarScriptCierrePasivo(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MensajesProceso)

        Dim intLongitud As Integer
        Dim strCarpeta As String = "EjecutarScripts"
        Dim strNombreArchivo As String
        Dim strExtensionArchivo As String
        Dim strMensaje As String
        Dim strConexion As String
        Dim strResulatado As String = String.Empty


        Dim objScripts As List(Of ScriptsA2CierrePasivo)
        Dim objRes As MensajesProceso = Nothing
        Dim objDatos = New List(Of MensajesProceso)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objRes = New MensajesProceso
            objRes.Linea = String.Empty
            objRes.URLArchivo = String.Empty
            objRes.TipoLinea = String.Empty
            objRes.NombreArchivo = String.Empty

            objScripts = Me.DataContext.uspCalculosFinancieros_CierrePasivo_Ejecutar_Script(pdtmFechaProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "ScriptsA2CierrePasivo"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList


            If objScripts.Count = 0 Then
                objRes.Linea = "No existen datos para la fecha (" & pdtmFechaProceso & ")   ."
                objRes.TipoLinea = "INCONSISTENCIA"
                objDatos.Add(objRes)
            Else
                Dim pstrNombreProceso As String
                pstrNombreProceso = "CierrePasivo"
                Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
                For Each Dt In objScripts
                    'strConexion = Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString

                    objRes.Linea = objScripts.Item(0).Mensaje
                    objRes.NombreArchivo = objScripts.Item(0).NombreArchivo
                    objRes.URLArchivo = objDatosRutas.RutaCompartidaOWeb() & objRes.NombreArchivo
                    objDatos.Add(objRes)
                Next

                'If objScripts.Count = 0 Then
                '    objRes.Linea = "El script que se debe ejecutar (" & pstrNombreScript & ") no está registrado en el sistema."
                '    objRes.TipoLinea = "INCONSISTENCIA"
                'Else
                '    strConexion = Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString

                '    strNombreArchivo = objScripts.Item(0).NombreArchivo

                '    If strNombreArchivo Is Nothing OrElse strNombreArchivo.Trim().Equals(String.Empty) Then
                '        If objScripts.Item(0).Nombre.Length > 30 Then
                '            intLongitud = 30
                '        Else
                '            intLongitud = objScripts.Item(0).Nombre.Length
                '        End If

                '        strNombreArchivo = objScripts.Item(0).Nombre.Substring(0, intLongitud) & "_" &
                '                                            Now.Year().ToString & Right("00" & Now.Month.ToString, 2) & Right("00" & Now.Day.ToString, 2) & "_" &
                '                                            Right("00" & Now.Hour.ToString, 2) & Right("00" & Now.Minute.ToString, 2) & Right("00" & Now.Second.ToString, 2) & Right("000" & Now.Millisecond.ToString, 3)
                '    Else
                '   6     If Not IsNothing(objScripts.Item(0).UsarSufijoFechaHora) AndAlso objScripts.Item(0).UsarSufijoFechaHora Then
                '            strNombreArchivo &= "_" &
                '                            Now.Year().ToString & Right("00" & Now.Month.ToString, 2) & Right("00" & Now.Day.ToString, 2) & "_" &
                '                            Right("00" & Now.Hour.ToString, 2) & Right("00" & Now.Minute.ToString, 2) & Right("00" & Now.Second.ToString, 2) & Right("000" & Now.Millisecond.ToString, 3)

                '        End If
                '    End If
                '    strNombreArchivo = strNombreArchivo.Replace(" ", "_")

                '    If IsNothing(objScripts.Item(0).TipoResultado) OrElse LTrim(objScripts.Item(0).TipoResultado).Equals(String.Empty) Then
                '        strExtensionArchivo = String.Empty
                '    Else
                '        strExtensionArchivo = objScripts.Item(0).TipoResultado
                '    End If

                '    strSeparador = objScripts.Item(0).Separador

                '    strResulatado = ejecutarProcedimientoScript(strConexion, pintIdCompania, pintIdScript, pstrNombreScript, strCarpeta, pstrParametros, strNombreArchivo, strSeparador, strExtensionArchivo, pstrMaquina, pstrUsuario)

                '    If strResulatado.Equals(String.Empty) Then
                '        ' El script no retorna ningún resultado
                '        objRes.Linea = "El script se ejecutó exitosamente."
                '    ElseIf Left(strResulatado, 20).ToUpper.IndexOf(SeparadorMensaje) >= 0 Then
                '        ' El script no retorna ningún resultado
                '        objRes.Linea = strResulatado.Substring(Len(SeparadorMensaje))
                '        objRes.TipoLinea = TiposResultadoScript.Mensaje.ToString.ToUpper
                '    Else
                '        ' El script retorna un archivo Excel o plano
                '        If Left(strResulatado, 20).ToUpper.IndexOf(SeparadorError) < 0 Then
                '            objRes.Linea = "Se generó el archivo con el resultado de la ejecución del script. Para descargar el archivo haga clic en el nombre del archivo."
                '            objRes.URLArchivo = strResulatado & "?IdGen=" & CStr(Math.Round(CDbl(Now().ToOADate()) * 100000000, 0))
                '            objRes.NombreArchivo = strNombreArchivo
                '        Else
                '            objRes.Linea = "El script presentó un problema durante su ejecución. " & strResulatado
                '            objRes.TipoLinea = "INCONSISTENCIA"
                '        End If
                '    End If
                'End If
            End If



            Return objDatos


        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ejecutarScriptCierrePasivo")
            Return Nothing
        End Try

    End Function
#End Region



End Class
