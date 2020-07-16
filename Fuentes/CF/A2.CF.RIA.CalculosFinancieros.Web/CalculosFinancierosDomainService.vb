Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Text
Imports System.Web
Imports System.Web.UI.Page
Imports System.Web.Configuration
Imports System.Data.SqlClient
Imports Microsoft.ServiceModel.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

''' <summary>
''' DomainServices para los maestros Calificadoras, EspeciesClaseInversion, CalificacionesCalificadora, PreciosEspecies,
''' Indicadores y ProcesarPortafolio, pertenecientes al proyecto Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Febrero 21/2014 
''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok
''' <remarks></remarks>

<EnableClientAccess()>
Partial Public Class CalculosFinancierosDomainService
    Inherits LinqToSqlDomainService(Of CalculosFinancierosDBML)


    'JAEZ funcion para tomar el tipo de modulo desde en web.config, que nos cambia el applicationaName 20161001
    Public Function Modulos() As String
        Try
            Dim strModuloPantalla As String = String.Empty
            Dim strDelimitador As Char = CChar(",")

            Dim strModulos As String = WebConfigurationManager.AppSettings("Modulos")

            If Not String.IsNullOrEmpty(strModulos) Then

                Dim strLista() As String = strModulos.Split(strDelimitador)

                For Each UnicoModulo In strLista
                    If CBool(InStr(1, UnicoModulo, "[OYDVALORAR]")) Then
                        strModuloPantalla = Right(UnicoModulo, Len(UnicoModulo) - InStrRev(UnicoModulo, "="))
                    End If
                Next

            End If

            Return strModuloPantalla
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "PreciosEspecies"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertPreciosEspecies(ByVal objPreciosEspecies As PreciosEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objPreciosEspecies.pstrUsuarioConexion, objPreciosEspecies.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objPreciosEspecies.InfoSesion = DemeInfoSesion(objPreciosEspecies.pstrUsuarioConexion, "InsertPreciosEspecies")
            Me.DataContext.PreciosEspecies.InsertOnSubmit(objPreciosEspecies)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPreciosEspecies")
        End Try
    End Sub

    Public Sub UpdatePreciosEspecies(ByVal currentPreciosEspecies As PreciosEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentPreciosEspecies.pstrUsuarioConexion, currentPreciosEspecies.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentPreciosEspecies.InfoSesion = DemeInfoSesion(currentPreciosEspecies.pstrUsuarioConexion, "UpdatePreciosEspecies")
            Me.DataContext.PreciosEspecies.Attach(currentPreciosEspecies, Me.ChangeSet.GetOriginal(currentPreciosEspecies))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePreciosEspecies")
        End Try
    End Sub

    Public Sub DeletePreciosEspecies(ByVal objPreciosEspecies As PreciosEspecies)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objPreciosEspecies.pstrUsuarioConexion, objPreciosEspecies.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objPreciosEspecies.InfoSesion = DemeInfoSesion(objPreciosEspecies.pstrUsuarioConexion, "DeletePreciosEspecies")
            Me.DataContext.PreciosEspecies.Attach(objPreciosEspecies)
            Me.DataContext.PreciosEspecies.DeleteOnSubmit(objPreciosEspecies)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePreciosEspecies")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarPreciosEspecies(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreciosEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_PreciosEspecies_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarPreciosEspecies"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarPreciosEspecies")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarPreciosEspecies(plngID As Integer, pstrIDEspecie As String, pstrISIN As String, pintNroEmision As Integer, pdtmFechaArchivo As System.Nullable(Of System.DateTime), pdtmEmision As System.Nullable(Of System.DateTime), pdtmVencimiento As System.Nullable(Of System.DateTime), pstrModalidad As String, pstrMoneda As String, pdblSpread As Double, pstrTasaRef As String, pdblPrecio As Double, pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, pdblPrecioLimpio As Double, pstrProveedor As String) As List(Of PreciosEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_PreciosEspecies_Consultar(String.Empty, plngID, pstrIDEspecie, pstrISIN, pintNroEmision, pdtmFechaArchivo, pdtmEmision, pdtmVencimiento, pstrModalidad, pstrMoneda, pdblSpread, pstrTasaRef, pdblPrecio, pstrTipoInversion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarPreciosEspecies"), 0, pdblPrecioLimpio, pstrProveedor).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPreciosEspecies")
            Return Nothing
        End Try
    End Function



    Public Function ConsultarPreciosEspeciesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PreciosEspecies
        Dim objPreciosEspecies As PreciosEspecies = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_PreciosEspecies_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, -1, String.Empty, String.Empty, 0, Nothing, Nothing, Nothing, String.Empty, String.Empty, 0, String.Empty, 0, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarPreciosEspeciesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER, 0, String.Empty).ToList
            If ret.Count > 0 Then
                objPreciosEspecies = ret.FirstOrDefault
            End If
            Return objPreciosEspecies
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPreciosEspeciesPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarPreciosEspeciesSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreciosEspecies)
        Dim objTask As Task(Of List(Of PreciosEspecies)) = Me.FiltrarPreciosEspeciesAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarPreciosEspeciesAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PreciosEspecies))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PreciosEspecies)) = New TaskCompletionSource(Of List(Of PreciosEspecies))()
        objTaskComplete.TrySetResult(FiltrarPreciosEspecies(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarPreciosEspeciesSync(ByVal plngID As Integer, ByVal pstrIDEspecie As String, ByVal pstrISIN As String, ByVal pintNroEmision As Integer, ByVal pdtmFechaArchivo As System.Nullable(Of System.DateTime), ByVal pdtmEmision As System.Nullable(Of System.DateTime), ByVal pdtmVencimiento As System.Nullable(Of System.DateTime), ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pdblSpread As Double, ByVal pstrTasaRef As String, ByVal pdblPrecio As Double, pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, pdblPrecioLimpio As Double, pstrProveedor As String) As List(Of PreciosEspecies)
        Dim objTask As Task(Of List(Of PreciosEspecies)) = Me.ConsultarPreciosEspeciesAsync(plngID, pstrIDEspecie, pstrISIN, pintNroEmision, pdtmFechaArchivo, pdtmEmision, pdtmVencimiento, pstrModalidad, pstrMoneda, pdblSpread, pstrTasaRef, pdblPrecio, pstrTipoInversion, pstrUsuario, pstrInfoConexion, pdblPrecioLimpio, pstrProveedor)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarPreciosEspeciesAsync(ByVal plngID As Integer, ByVal pstrIDEspecie As String, ByVal pstrISIN As String, ByVal pintNroEmision As Integer, ByVal pdtmFechaArchivo As System.Nullable(Of System.DateTime), ByVal pdtmEmision As System.Nullable(Of System.DateTime), ByVal pdtmVencimiento As System.Nullable(Of System.DateTime), ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pdblSpread As Double, ByVal pstrTasaRef As String, ByVal pdblPrecio As Double, pstrTipoInversion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, pdblPrecioLimpio As Double, pstrProveedor As String) As Task(Of List(Of PreciosEspecies))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PreciosEspecies)) = New TaskCompletionSource(Of List(Of PreciosEspecies))()
        objTaskComplete.TrySetResult(ConsultarPreciosEspecies(plngID, pstrIDEspecie, pstrISIN, pintNroEmision, pdtmFechaArchivo, pdtmEmision, pdtmVencimiento, pstrModalidad, pstrMoneda, pdblSpread, pstrTasaRef, pdblPrecio, pstrTipoInversion, pstrUsuario, pstrInfoConexion, pdblPrecioLimpio, pstrProveedor))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarPreciosEspeciesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PreciosEspecies
        Dim objTask As Task(Of PreciosEspecies) = Me.ConsultarPreciosEspeciesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarPreciosEspeciesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of PreciosEspecies)
        Dim objTaskComplete As TaskCompletionSource(Of PreciosEspecies) = New TaskCompletionSource(Of PreciosEspecies)()
        objTaskComplete.TrySetResult(ConsultarPreciosEspeciesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Indicadores"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertIndicadores(ByVal objIndicadores As Indicadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objIndicadores.pstrUsuarioConexion, objIndicadores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objIndicadores.InfoSesion = DemeInfoSesion(objIndicadores.pstrUsuarioConexion, "InsertIndicadores")
            Me.DataContext.Indicadores.InsertOnSubmit(objIndicadores)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertIndicadores")
        End Try
    End Sub

    Public Sub UpdateIndicadores(ByVal currentIndicadores As Indicadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentIndicadores.pstrUsuarioConexion, currentIndicadores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentIndicadores.InfoSesion = DemeInfoSesion(currentIndicadores.pstrUsuarioConexion, "UpdateIndicadores")
            Me.DataContext.Indicadores.Attach(currentIndicadores, Me.ChangeSet.GetOriginal(currentIndicadores))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateIndicadores")
        End Try
    End Sub

    Public Sub DeleteIndicadores(ByVal objIndicadores As Indicadores)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objIndicadores.pstrUsuarioConexion, objIndicadores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objIndicadores.InfoSesion = DemeInfoSesion(objIndicadores.pstrUsuarioConexion, "DeleteIndicadores")
            Me.DataContext.Indicadores.Attach(objIndicadores)
            Me.DataContext.Indicadores.DeleteOnSubmit(objIndicadores)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteIndicadores")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarIndicadores(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Indicadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Indicadores_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarIndicadores"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarIndicadores")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarIndicadores(pstrTipoIndicador As String, pstrDescripcion As String, pdtmFechaArchivo As Nullable(Of Date), pdblValor As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Indicadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Indicadores_Consultar(String.Empty, pstrTipoIndicador, pstrDescripcion, pdtmFechaArchivo, pdblValor, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarIndicadores"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarIndicadores")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarIndicadoresPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Indicadores
        Dim objIndicadores As Indicadores = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Indicadores_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, Nothing, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarIndicadoresPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objIndicadores = ret.FirstOrDefault
            End If
            Return objIndicadores
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarIndicadoresPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarIndicadoresSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Indicadores)
        Dim objTask As Task(Of List(Of Indicadores)) = Me.FiltrarIndicadoresAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarIndicadoresAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Indicadores))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Indicadores)) = New TaskCompletionSource(Of List(Of Indicadores))()
        objTaskComplete.TrySetResult(FiltrarIndicadores(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarIndicadoresSync(ByVal pstrTipoIndicador As String, ByVal pstrDescripcion As String, ByVal pdtmFechaArchivo As Nullable(Of Date), ByVal pdblValor As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Indicadores)
        Dim objTask As Task(Of List(Of Indicadores)) = Me.ConsultarIndicadoresAsync(pstrTipoIndicador, pstrDescripcion, pdtmFechaArchivo, pdblValor, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarIndicadoresAsync(ByVal pstrTipoIndicador As String, ByVal pstrDescripcion As String, ByVal pdtmFechaArchivo As Nullable(Of Date), ByVal pdblValor As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Indicadores))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Indicadores)) = New TaskCompletionSource(Of List(Of Indicadores))()
        objTaskComplete.TrySetResult(ConsultarIndicadores(pstrTipoIndicador, pstrDescripcion, pdtmFechaArchivo, pdblValor, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarIndicadoresPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Indicadores
        Dim objTask As Task(Of Indicadores) = Me.ConsultarIndicadoresPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarIndicadoresPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Indicadores)
        Dim objTaskComplete As TaskCompletionSource(Of Indicadores) = New TaskCompletionSource(Of Indicadores)()
        objTaskComplete.TrySetResult(ConsultarIndicadoresPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

    'JEPM20181010 Los métodos de ProcesarPortafolio se eliminan de este Domain y se pasan a ProcesarPortafolioDomainService.vb

#Region "CalculadoraBasica"

#Region "Métodos asincrónicos"

    ''' <summary>
    ''' Esta función se utiliza para obtener el resultado de la cálculadora básica
    ''' </summary>
    ''' <param name="strIDEspecie">Parámetro de tipo String</param>
    ''' <param name="strISIN">Parámetro de tipo String</param>
    ''' <param name="strModalidad">Parámetro de tipo String</param>
    ''' <param name="strBase">Parámetro de tipo String</param>
    ''' <param name="dtmEmision">Parámetro de tipo DateTime</param>
    ''' <param name="logEsAccion">Parámetro de tipo Booleano</param>
    ''' <param name="dtmVencimiento">Parámetro de tipo DateTime</param>
    ''' <param name="dtmCompra">Parámetro de tipo DateTime</param>
    ''' <param name="strIndicador">Parámetro de tipo String</param>
    ''' <param name="strTasaRef">Parámetro de tipo String</param>
    ''' <param name="strMoneda">Parámetro de tipo String</param>
    ''' <param name="strMetodoValoracion">Parámetro de tipo String</param>
    ''' <param name="dblValorNominal">Parámetro de tipo Double</param>
    ''' <param name="dblValorGiro">Parámetro de tipo Double</param>
    ''' <param name="dblTasaTIR">Parámetro de tipo Double</param>
    ''' <param name="dblTasaTitulo">Parámetro de tipo Double</param>
    ''' <param name="dtmProceso">Parámetro de tipo DateTime</param>
    ''' <param name="pstrUsuario">Parámetro de tipo String</param>
    ''' <returns>Retorna una entidad de tipo ResultadoCalculadoraBasica</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Fecha           : Julio 4/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Julio 4/2014 - Resultado OK
    ''' </history>
    Public Function ConsultarResultadoCalculadoraBasica(ByVal strIDEspecie As String,
                   ByVal strISIN As String,
                   ByVal strModalidad As String,
                   ByVal strBase As String,
                   ByVal dtmEmision As System.Nullable(Of System.DateTime),
                   ByVal logEsAccion As Boolean,
                   ByVal dtmVencimiento As System.Nullable(Of System.DateTime),
                   ByVal dtmCompra As System.Nullable(Of System.DateTime),
                   ByVal strIndicador As String,
                   ByVal strTasaRef As String,
                   ByVal strMoneda As String,
                   ByVal strMetodoValoracion As String,
                   ByVal dblValorNominal As Double,
                   ByVal dblValorGiro As Double,
                   ByVal dblTasaTIR As Double,
                   ByVal dblTasaTitulo As Double,
                   ByVal dtmProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ResultadoCalculadoraBasica)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculadoraBasica(strIDEspecie, strISIN, strModalidad,
                                                                               strBase, dtmEmision, logEsAccion, dtmVencimiento, dtmCompra,
                                                                               strIndicador, strTasaRef, strMoneda,
                                                                               strMetodoValoracion, dblValorNominal, dblValorGiro,
                                                                               dblTasaTIR, dblTasaTitulo, dtmProceso, pstrUsuario,
                                                                               DemeInfoSesion(pstrUsuario, "ConsultarResultadoCalculadoraBasica"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarResultadoCalculadoraBasica")
            Return Nothing
        End Try
    End Function

#End Region

#End Region

#Region "DeshacerCierrePortafolios"

#Region "Métodos asincrónicos"
    Public Function DeshacerCierrePortafolios(pdtmFechaProceso As Nullable(Of Date), plngIDComitente As String, ByVal plogTodosLosClientes As System.Nullable(Of Boolean), ByVal pstrTipoPortafolio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DeshacerCierrePortafolios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_DeshacerCierrePortafolios(pdtmFechaProceso, plngIDComitente, plogTodosLosClientes, pstrTipoPortafolio, pstrUsuario, DemeInfoSesion(pstrUsuario, "DeshacerCierrePortafolios"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeshacerCierrePortafolios")
            Return Nothing
        End Try
    End Function
#End Region

#End Region

#Region "ExportacionArchivos"

#Region "Métodos asincrónicos"

    ''' <summary>
    ''' Función para obtener el nombre del formato que se debe visualizar al exportar
    ''' </summary>
    ''' <param name="strRetorno">Nombre Anterior del Archivo</param>
    ''' <param name="strFechaExportacion">Fecha Seleccionada por el Usuario</param>
    ''' <param name="pstrUsuario">Nombre del usuario</param>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Fecha           : Agosto 19/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Agosto 19/2014 - Resultado OK
    ''' </history>
    Public Function ObtenerNombreFormato(ByVal strRetorno As String, ByVal strFechaExportacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim NombreFormato As String = String.Empty
            Dim ret = Me.DataContext.uspCalculosFinancieros_ObtenerNombreFormato(strRetorno,
                                                                                 strFechaExportacion,
                                                                                 pstrUsuario,
                                                                                 DemeInfoSesion(pstrUsuario, "ObtenerNombreFormato"), 0,
                                                                                 NombreFormato)
            Return NombreFormato
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerNombreFormato")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Private Function ObtenerNombreFormatoAsync(ByVal strRetorno As String, ByVal strFechaExportacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ObtenerNombreFormato(strRetorno, strFechaExportacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ObtenerNombreFormatoSync(ByVal strRetorno As String, ByVal strFechaExportacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ObtenerNombreFormatoAsync(strRetorno, strFechaExportacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

#End Region

#End Region

#Region "OperacionInterbancaria"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertOperacionInterbancaria(ByVal newOperacionInterbancaria As OperacionInterbancaria)

    End Sub

    Public Sub UpdateOperacionInterbancaria(ByVal currentOperacionInterbancaria As OperacionInterbancaria)

    End Sub

    Public Sub DeleteOperacionInterbancaria(ByVal deleteOperacionInterbancaria As OperacionInterbancaria)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteOperacionInterbancaria.pstrUsuarioConexion, deleteOperacionInterbancaria.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteOperacionInterbancaria.strInfoSesion = DemeInfoSesion(deleteOperacionInterbancaria.pstrUsuarioConexion, "DeleteOperacionInterbancaria")
            Me.DataContext.OperacionInterbancaria.Attach(deleteOperacionInterbancaria)
            Me.DataContext.OperacionInterbancaria.DeleteOnSubmit(deleteOperacionInterbancaria)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperacionInterbancaria")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarOperacionInterbancaria(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionInterbancaria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculoPagosInterbancarios_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarOperacionInterbancaria"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarOperacionInterbancaria")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarOperacionInterbancaria(ByVal pstrIDOperacion As String, ByVal plngIDComitente As String, ByVal plngIDBanco As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionInterbancaria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculoPagosInterbancarios_Consultar(String.Empty, pstrIDOperacion, plngIDComitente, plngIDBanco, pdtmFechaInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarOperacionInterbancaria"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarOperacionInterbancaria")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarOperacionInterbancariaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OperacionInterbancaria
        Dim objOperacionInterbancaria As OperacionInterbancaria = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculoPagosInterbancarios_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, 0, Nothing, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarOperacionInterbancariaPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objOperacionInterbancaria = ret.FirstOrDefault
            End If
            Return objOperacionInterbancaria
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarOperacionInterbancariaPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar la Operacion Interbancaria
    ''' </summary>
    Public Function ActualizarOperacionInterbancaria(ByVal pintIDOperacionInterbancaria As System.Nullable(Of Integer), ByVal pstrIDOperacion As String, ByVal plngIDComitente As String, ByVal plngIDBanco As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdblValorInicial As System.Nullable(Of Double), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pdblTasaPuntos As System.Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pdblSaldo As System.Nullable(Of Double), pstrNotas As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculoPagosInterbancarios_Actualizar(pintIDOperacionInterbancaria, pstrIDOperacion, plngIDComitente, plngIDBanco, pdtmFechaInicial, pdblValorInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pdblTasaPuntos, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pdblSaldo, pstrNotas, pxmlDetalleGrid, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarSegmentosNegocios"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarOperacionInterbancaria")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarOperacionInterbancariaSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionInterbancaria)
        Dim objTask As Task(Of List(Of OperacionInterbancaria)) = Me.FiltrarOperacionInterbancariaAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarOperacionInterbancariaAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OperacionInterbancaria))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OperacionInterbancaria)) = New TaskCompletionSource(Of List(Of OperacionInterbancaria))()
        objTaskComplete.TrySetResult(FiltrarOperacionInterbancaria(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarOperacionInterbancariaSync(ByVal pstrIDOperacion As String, ByVal plngIDComitente As String, ByVal plngIDBanco As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionInterbancaria)
        Dim objTask As Task(Of List(Of OperacionInterbancaria)) = Me.ConsultarOperacionInterbancariaAsync(pstrIDOperacion, plngIDComitente, plngIDBanco, pdtmFechaInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarOperacionInterbancariaAsync(ByVal pstrIDOperacion As String, ByVal plngIDComitente As String, ByVal plngIDBanco As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OperacionInterbancaria))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OperacionInterbancaria)) = New TaskCompletionSource(Of List(Of OperacionInterbancaria))()
        objTaskComplete.TrySetResult(ConsultarOperacionInterbancaria(pstrIDOperacion, plngIDComitente, plngIDBanco, pdtmFechaInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarOperacionInterbancariaPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OperacionInterbancaria
        Dim objTask As Task(Of OperacionInterbancaria) = Me.ConsultarOperacionInterbancariaPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarOperacionInterbancariaPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of OperacionInterbancaria)
        Dim objTaskComplete As TaskCompletionSource(Of OperacionInterbancaria) = New TaskCompletionSource(Of OperacionInterbancaria)()
        objTaskComplete.TrySetResult(ConsultarOperacionInterbancariaPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Función para realizar llamado sincronico al método ActualizarCodificacionContableAsync y controlar los mensajes de salida
    ''' </summary>
    Public Function ActualizarOperacionInterbancariaSync(ByVal pintIDOperacionInterbancaria As System.Nullable(Of Integer), ByVal pstrIDOperacion As String, ByVal plngIDComitente As String, ByVal plngIDBanco As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdblValorInicial As System.Nullable(Of Double), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pdblTasaPuntos As System.Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pdblSaldo As System.Nullable(Of Double), pstrNotas As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarOperacionInterbancariaAsync(pintIDOperacionInterbancaria, pstrIDOperacion, plngIDComitente, plngIDBanco, pdtmFechaInicial, pdblValorInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pdblTasaPuntos, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pdblSaldo, pstrNotas, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    ''' <summary>
    ''' Función para realizar el llamado al proceso ActualizarCodificacionContable 
    ''' </summary>
    Private Function ActualizarOperacionInterbancariaAsync(ByVal pintIDOperacionInterbancaria As System.Nullable(Of Integer), ByVal pstrIDOperacion As String, ByVal plngIDComitente As String, ByVal plngIDBanco As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdblValorInicial As System.Nullable(Of Double), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pdblTasaPuntos As System.Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pdblSaldo As System.Nullable(Of Double), pstrNotas As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarOperacionInterbancaria(pintIDOperacionInterbancaria, pstrIDOperacion, plngIDComitente, plngIDBanco, pdtmFechaInicial, pdblValorInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pdblTasaPuntos, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pdblSaldo, pstrNotas, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "OperacionInterbancariaDetalle"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertOperacionInterbancariaDetalle(ByVal objOperacionInterbancariaDetalle As OperacionInterbancariaDetalle)

    End Sub

    Public Sub UpdateOperacionInterbancariaDetalle(ByVal currentOperacionInterbancariaDetalle As OperacionInterbancariaDetalle)

    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarOperacionInterbancariaDetalle(plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionInterbancariaDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculoPagosInterbancarioDetalle_Consultar(String.Empty, plngID, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContable"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarOperacionInterbancariaDetalle")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarOperacionInterbancariaDetallePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OperacionInterbancariaDetalle
        Dim objOperacionInterbancariaDetalle As OperacionInterbancariaDetalle = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculoPagosInterbancarioDetalle_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContablePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objOperacionInterbancariaDetalle = ret.FirstOrDefault
            End If
            Return objOperacionInterbancariaDetalle
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarOperacionInterbancariaDetallePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Function CalculoPagosInterbancarioDetalle_Generar(ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdblValorInicial As System.Nullable(Of Double), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pdblTasaPuntos As System.Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrDetalleGridIDPago As String, ByVal pstrDetalleGridValorPagoAdicional As String, ByVal pstrDetalleGridPagado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionInterbancariaDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculoPagosInterbancarioDetalle_Generar(pdtmFechaInicial, pdblValorInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pdblTasaPuntos, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pstrDetalleGridIDPago, pstrDetalleGridValorPagoAdicional, pstrDetalleGridPagado, pstrUsuario, DemeInfoSesion(pstrUsuario, "CalculoPagosInterbancarioDetalle_Generar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalculoPagosInterbancarioDetalle_Generar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarOperacionInterbancariaDetalleSync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionInterbancariaDetalle)
        Dim objTask As Task(Of List(Of OperacionInterbancariaDetalle)) = Me.ConsultarOperacionInterbancariaDetalleAsync(plngID, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarOperacionInterbancariaDetalleAsync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OperacionInterbancariaDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OperacionInterbancariaDetalle)) = New TaskCompletionSource(Of List(Of OperacionInterbancariaDetalle))()
        objTaskComplete.TrySetResult(ConsultarOperacionInterbancariaDetalle(plngID, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarOperacionInterbancariaDetallePorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OperacionInterbancariaDetalle
        Dim objTask As Task(Of OperacionInterbancariaDetalle) = Me.ConsultarOperacionInterbancariaDetallePorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarOperacionInterbancariaDetallePorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of OperacionInterbancariaDetalle)
        Dim objTaskComplete As TaskCompletionSource(Of OperacionInterbancariaDetalle) = New TaskCompletionSource(Of OperacionInterbancariaDetalle)()
        objTaskComplete.TrySetResult(ConsultarOperacionInterbancariaDetallePorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function CalculoPagosInterbancarioDetalle_GenerarSync(ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdblValorInicial As System.Nullable(Of Double), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pdblTasaPuntos As System.Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrDetalleGridIDPago As String, ByVal pstrDetalleGridValorPagoAdicional As String, ByVal pstrDetalleGridPagado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionInterbancariaDetalle)
        Dim objTask As Task(Of List(Of OperacionInterbancariaDetalle)) = Me.CalculoPagosInterbancarioDetalle_GenerarAsync(pdtmFechaInicial, pdblValorInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pdblTasaPuntos, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pstrDetalleGridIDPago, pstrDetalleGridValorPagoAdicional, pstrDetalleGridPagado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function CalculoPagosInterbancarioDetalle_GenerarAsync(ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdblValorInicial As System.Nullable(Of Double), ByVal pstrTipoTasaFija As String, ByVal pstrBase As String, ByVal pstrIndicadorEconomico As String, ByVal pdblTasaPuntos As System.Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrMoneda As String, ByVal pstrPosicion As String, ByVal pstrTipo As String, ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrDetalleGridIDPago As String, ByVal pstrDetalleGridValorPagoAdicional As String, ByVal pstrDetalleGridPagado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OperacionInterbancariaDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OperacionInterbancariaDetalle)) = New TaskCompletionSource(Of List(Of OperacionInterbancariaDetalle))()
        objTaskComplete.TrySetResult(CalculoPagosInterbancarioDetalle_Generar(pdtmFechaInicial, pdblValorInicial, pstrTipoTasaFija, pstrBase, pstrIndicadorEconomico, pdblTasaPuntos, pstrModalidad, pstrMoneda, pstrPosicion, pstrTipo, pdtmFechaFinal, pstrDetalleGridIDPago, pstrDetalleGridValorPagoAdicional, pstrDetalleGridPagado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Portafolios"

#Region "Métodos asincrónicos"
    Public Function ConsultarDatosPortafolio(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_DatosPortafolio_Consultar(plngIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "DatosPortafolio"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DatosPortafolio")
            Return Nothing
        End Try
    End Function
#End Region

    Public Function ConsultarDatosPortafolioSync(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolios)
        Dim objTask As Task(Of List(Of DatosPortafolios)) = Me.ConsultarDatosPortafoliosAsync(plngIDComitente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarDatosPortafoliosAsync(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DatosPortafolios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DatosPortafolios)) = New TaskCompletionSource(Of List(Of DatosPortafolios))()
        objTaskComplete.TrySetResult(ConsultarDatosPortafolio(plngIDComitente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "MovimientosParticipacionesFondos"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertMovimientosParticipacionesFondos(ByVal newMovimientosParticipacionesFondos As MovimientosParticipacionesFondos)

    End Sub

    Public Sub UpdateMovimientosParticipacionesFondos(ByVal currentMovimientosParticipacionesFondos As MovimientosParticipacionesFondos)

    End Sub

    Public Sub DeleteMovimientosParticipacionesFondos(ByVal deleteMovimientosParticipacionesFondos As MovimientosParticipacionesFondos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteMovimientosParticipacionesFondos.pstrUsuarioConexion, deleteMovimientosParticipacionesFondos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteMovimientosParticipacionesFondos.strInfoSesion = DemeInfoSesion(deleteMovimientosParticipacionesFondos.pstrUsuarioConexion, "DeleteMovimientosParticipacionesFondo")
            Me.DataContext.MovimientosParticipacionesFondos.Attach(deleteMovimientosParticipacionesFondos)
            Me.DataContext.MovimientosParticipacionesFondos.DeleteOnSubmit(deleteMovimientosParticipacionesFondos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteMovimientosParticipacionesFondos")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarMovimientosParticipacionesFondos(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MovimientosParticipacionesFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_MovimientosParticipacionesFondos_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarMovimientosParticipacionesFondo"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarMovimientosParticipacionesFondo")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarMovimientosParticipacionesFondos(ByVal pintCuenta As String, pstrPortafolio As String, pstrFondo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MovimientosParticipacionesFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_MovimientosParticipacionesFondos_Consultar(String.Empty, pintCuenta, pstrPortafolio, pstrFondo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarMovimientosParticipacionesFondo"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarMovimientosParticipacionesFondo")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarMovimientosParticipacionesFondosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As MovimientosParticipacionesFondos
        Dim objMovimientosParticipacionesFondo As MovimientosParticipacionesFondos = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_MovimientosParticipacionesFondos_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarMovimientosParticipacionesFondoPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objMovimientosParticipacionesFondo = ret.FirstOrDefault
            End If
            Return objMovimientosParticipacionesFondo
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarMovimientosParticipacionesFondoPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar el Movimiento Participación Fondo
    ''' </summary>
    Public Function ActualizarMovimientosParticipacionesFondos(ByVal pintMovimientosParticipacionesFondos As System.Nullable(Of Integer), ByVal plogEsNuevoRegistro As System.Nullable(Of System.Boolean), ByVal pintCuenta As String, ByVal plngIDPortafolio As String, ByVal pstrFondo As String, ByVal pdblEntradas As System.Nullable(Of Double), ByVal pdblSalidas As System.Nullable(Of Double), ByVal pdblSaldo As System.Nullable(Of Double), ByVal pdblSaldoUnidades As System.Nullable(Of Double), ByVal pdblUnidadesFinales As System.Nullable(Of Double), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMoneda As String, pstrConfirmaciones As String, pstrConfirmacionesUsuario As String, pstrJustificaciones As String, pstrJustificacionUsuario As String, pstrAprobaciones As String, pstrAprobacionesUsuario As String, pstrIDReceptor As String, ByVal pstrInfoConexion As String) As List(Of CFCalculosFinancieros.Respuesta_Validaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Dim pstrMsgValidacion As CFCalculosFinancieros.Respuesta_Validaciones
            Dim ret = Me.DataContext.uspCalculosFinancieros_MovimientosParticipacionesFondo_Actualizar(pintMovimientosParticipacionesFondos, plogEsNuevoRegistro, pintCuenta, plngIDPortafolio, pstrFondo, pdblEntradas, pdblSalidas, pdblSaldo, pdblSaldoUnidades, pdblSaldoUnidades, pxmlDetalleGrid, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarSegmentosNegocios"), 0, "", pstrMoneda, pstrConfirmaciones, pstrConfirmacionesUsuario, pstrJustificaciones, pstrJustificacionUsuario, pstrAprobaciones, pstrAprobacionesUsuario, pstrIDReceptor).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarMovimientosParticipacionesFondo")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarMovimientosParticipacionesFondosSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MovimientosParticipacionesFondos)
        Dim objTask As Task(Of List(Of MovimientosParticipacionesFondos)) = Me.FiltrarMovimientosParticipacionesFondosAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarMovimientosParticipacionesFondosAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of MovimientosParticipacionesFondos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of MovimientosParticipacionesFondos)) = New TaskCompletionSource(Of List(Of MovimientosParticipacionesFondos))()
        objTaskComplete.TrySetResult(FiltrarMovimientosParticipacionesFondos(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarMovimientosParticipacionesFondosSync(ByVal pintCuenta As String, pstrPortafolio As String, pstrFondo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MovimientosParticipacionesFondos)
        Dim objTask As Task(Of List(Of MovimientosParticipacionesFondos)) = Me.ConsultarMovimientosParticipacionesFondosAsync(pintCuenta, pstrPortafolio, pstrFondo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarMovimientosParticipacionesFondosAsync(ByVal pintCuenta As String, pstrPortafolio As String, pstrFondo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of MovimientosParticipacionesFondos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of MovimientosParticipacionesFondos)) = New TaskCompletionSource(Of List(Of MovimientosParticipacionesFondos))()
        objTaskComplete.TrySetResult(ConsultarMovimientosParticipacionesFondos(pintCuenta, pstrPortafolio, pstrFondo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarMovimientosParticipacionesFondosPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As MovimientosParticipacionesFondos
        Dim objTask As Task(Of MovimientosParticipacionesFondos) = Me.ConsultarMovimientosParticipacionesFondosPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarMovimientosParticipacionesFondosPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of MovimientosParticipacionesFondos)
        Dim objTaskComplete As TaskCompletionSource(Of MovimientosParticipacionesFondos) = New TaskCompletionSource(Of MovimientosParticipacionesFondos)()
        objTaskComplete.TrySetResult(ConsultarMovimientosParticipacionesFondosPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Función para realizar llamado sincronico al método ActualizarMovimientosParticipacionesFondosAsync y controlar los mensajes de salida
    ''' </summary>
    ''' 
    'Se agrego linea de codigo para evitar que de el error de URI YAPP20151203
    <Query(HasSideEffects:=True)>
    Public Function ActualizarMovimientosParticipacionesFondosSync(ByVal pintMovimientosParticipacionesFondos As System.Nullable(Of Integer), ByVal plogEsNuevoRegistro As System.Nullable(Of System.Boolean), ByVal pintCuenta As String, ByVal plngIDPortafolio As String, ByVal pstrFondo As String, ByVal pdblEntradas As System.Nullable(Of Double), ByVal pdblSalidas As System.Nullable(Of Double), ByVal pdblSaldo As System.Nullable(Of Double), ByVal pdblSaldoUnidades As System.Nullable(Of Double), ByVal pdblUnidadesFinales As System.Nullable(Of Double), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMoneda As String, pstrConfirmaciones As String, pstrConfirmacionesUsuario As String, pstrJustificaciones As String, pstrJustificacionUsuario As String, pstrAprobaciones As String, pstrAprobacionesUsuario As String, pstrIDReceptor As String, ByVal pstrInfoConexion As String) As List(Of CFCalculosFinancieros.Respuesta_Validaciones)

        pxmlDetalleGrid = HttpUtility.UrlDecode(pxmlDetalleGrid)
        Dim objTask As Task(Of List(Of CFCalculosFinancieros.Respuesta_Validaciones)) = Me.ActualizarMovimientosParticipacionesFondosAsync(pintMovimientosParticipacionesFondos, plogEsNuevoRegistro, pintCuenta, plngIDPortafolio, pstrFondo, pdblEntradas, pdblSalidas, pdblSaldo, pdblSaldoUnidades, pdblSaldoUnidades, pxmlDetalleGrid, pstrUsuario, pstrMoneda, pstrConfirmaciones, pstrConfirmacionesUsuario, pstrJustificaciones, pstrJustificacionUsuario, pstrAprobaciones, pstrAprobacionesUsuario, pstrIDReceptor, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    ''' <summary>
    ''' Función para realizar el llamado al proceso ActualizarMovimientosParticipacionesFondos 
    ''' </summary>
    Private Function ActualizarMovimientosParticipacionesFondosAsync(ByVal pintMovimientosParticipacionesFondos As System.Nullable(Of Integer), ByVal plogEsNuevoRegistro As System.Nullable(Of System.Boolean), ByVal pintCuenta As String, ByVal plngIDPortafolio As String, ByVal pstrFondo As String, ByVal pdblEntradas As System.Nullable(Of Double), ByVal pdblSalidas As System.Nullable(Of Double), ByVal pdblSaldo As System.Nullable(Of Double), ByVal pdblSaldoUnidades As System.Nullable(Of Double), ByVal pdblUnidadesFinales As System.Nullable(Of Double), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMoneda As String, pstrConfirmaciones As String, pstrConfirmacionesUsuario As String, pstrJustificaciones As String, pstrJustificacionUsuario As String, pstrAprobaciones As String, pstrAprobacionesUsuario As String, pstrIDReceptor As String, pstrInfoConexion As String) As Task(Of List(Of CFCalculosFinancieros.Respuesta_Validaciones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CFCalculosFinancieros.Respuesta_Validaciones)) = New TaskCompletionSource(Of List(Of Respuesta_Validaciones))()
        objTaskComplete.TrySetResult(ActualizarMovimientosParticipacionesFondos(pintMovimientosParticipacionesFondos, plogEsNuevoRegistro, pintCuenta, plngIDPortafolio, pstrFondo, pdblEntradas, pdblSalidas, pdblSaldo, pdblSaldoUnidades, pdblSaldoUnidades, pxmlDetalleGrid, pstrUsuario, pstrMoneda, pstrConfirmaciones, pstrConfirmacionesUsuario, pstrJustificaciones, pstrJustificacionUsuario, pstrAprobaciones, pstrAprobacionesUsuario, pstrIDReceptor, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "MovimientosParticipacionesFondosDetalle"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertMovimientosParticipacionesFondosDetalle(ByVal objMovimientosParticipacionesFondosDetalle As MovimientosParticipacionesFondosDetalle)

    End Sub

    Public Sub UpdateMovimientosParticipacionesFondosDetalle(ByVal currentMovimientosParticipacionesFondosDetalle As MovimientosParticipacionesFondosDetalle)

    End Sub

    Public Sub DeleteMovimientosParticipacionesFondosDetalle(ByVal UpdateMovimientosParticipacionesFondosDetalle As MovimientosParticipacionesFondosDetalle)

    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarMovimientosParticipacionesFondoDetalle(pintCuenta As String, plngIDPortafolio As String, pstrFondo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MovimientosParticipacionesFondosDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_MovimientosParticipacionesFondosDetalle_Consultar(String.Empty, pintCuenta, plngIDPortafolio, pstrFondo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContable"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarMovimientosParticipacionesFondoDetalle")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarMovimientosParticipacionesFondoDetallePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As MovimientosParticipacionesFondosDetalle
        Dim objMovimientosParticipacionesFondoDetalle As MovimientosParticipacionesFondosDetalle = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_MovimientosParticipacionesFondosDetalle_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContablePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objMovimientosParticipacionesFondoDetalle = ret.FirstOrDefault
            End If
            Return objMovimientosParticipacionesFondoDetalle
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarMovimientosParticipacionesFondoDetallePorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarMovimientosParticipacionesFondoDetalleSync(pintCuenta As String, plngIDPortafolio As String, pstrFondo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MovimientosParticipacionesFondosDetalle)
        Dim objTask As Task(Of List(Of MovimientosParticipacionesFondosDetalle)) = Me.ConsultarMovimientosParticipacionesFondoDetalleAsync(pintCuenta, plngIDPortafolio, pstrFondo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarMovimientosParticipacionesFondoDetalleAsync(pintCuenta As String, plngIDPortafolio As String, pstrFondo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of MovimientosParticipacionesFondosDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of MovimientosParticipacionesFondosDetalle)) = New TaskCompletionSource(Of List(Of MovimientosParticipacionesFondosDetalle))()
        objTaskComplete.TrySetResult(ConsultarMovimientosParticipacionesFondoDetalle(pintCuenta, plngIDPortafolio, pstrFondo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarMovimientosParticipacionesFondoDetallePorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As MovimientosParticipacionesFondosDetalle
        Dim objTask As Task(Of MovimientosParticipacionesFondosDetalle) = Me.ConsultarMovimientosParticipacionesFondoDetallePorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarMovimientosParticipacionesFondoDetallePorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of MovimientosParticipacionesFondosDetalle)
        Dim objTaskComplete As TaskCompletionSource(Of MovimientosParticipacionesFondosDetalle) = New TaskCompletionSource(Of MovimientosParticipacionesFondosDetalle)()
        objTaskComplete.TrySetResult(ConsultarMovimientosParticipacionesFondoDetallePorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Companias"

    'JEPM20150728

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertCompanias(ByVal objCompanias As Companias)

    End Sub

    Public Sub UpdateCompanias(ByVal currentCompanias As Companias)

    End Sub

    Public Sub DeleteCompanias(ByVal objCompanias As Companias)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCompanias.pstrUsuarioConexion, objCompanias.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCompanias.strInfoSesion = DemeInfoSesion(objCompanias.pstrUsuarioConexion, "DeleteCompanias")
            Me.DataContext.Companias.Attach(objCompanias)
            Me.DataContext.Companias.DeleteOnSubmit(objCompanias)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCompanias")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function FiltrarCompanias(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Companias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Companias_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarCompanias"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarCompanias")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompanias(pstrTipoDocumento As String, pstrNumeroDocumento As String, pstrNombre As String, ByVal pstrTipoCompania As String, ByVal pstrTipoPlazo As String, ByVal pstrParticipacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Companias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Companias_Consultar(String.Empty, pstrTipoDocumento, pstrNumeroDocumento, pstrNombre, pstrTipoCompania, pstrTipoPlazo, pstrParticipacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompanias"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompanias")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompaniasPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Companias
        Dim objCompanias As Companias = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Companias_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompanias = ret.FirstOrDefault
            End If
            Return objCompanias
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar la Compañía
    ''' </summary>
    ''' <history>
    ''' Descripción      : Actualización. Nuevos campos strContabDividendos, dtmApertura, dtmUltimoCierre, strProveedorPrecios.
    ''' Fecha            : Agosto 12/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Agosto 12/2015 - Resultado Ok 
    ''' 'JEPM201500812
    ''' 
    ''' Descripción      : Actualización. Nuevos campos pintIDMonedaComision y pstrNombreMonedaComision para el manejo de la moneda para la comisión.
    ''' Fecha            : Marzo 17/2020
    ''' Responsable      : Germán Arbey González Osorio (A2)
    ''' ControlCambios   : 20200317
    ''' </history>
    Public Function ActualizarCompanias(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal strTipoDocumento As String, ByVal pstrNumeroDocumento As String, ByVal pstrNombre As String, ByVal pstrCompaniaContable As String, ByVal pstrTipoCompania As String, ByVal pstrCodigoSuper As String, ByVal plogActiva As System.Nullable(Of System.Boolean), ByVal pstrContabDividendos As String, ByVal pdtmApertura As System.Nullable(Of System.DateTime), ByVal pdtmUltimoCierre As System.Nullable(Of System.DateTime), ByVal pstrProveedorPrecios As String, ByVal pstrTipoEntidadSuper As String, ByVal pintIdDinamicaContableConfig As System.Nullable(Of Integer), ByVal pintIdGestor As System.Nullable(Of Integer), ByVal pstrNombreGestor As String, ByVal plogManejaValorUnidad As System.Nullable(Of System.Boolean), ByVal pintIDMoneda As System.Nullable(Of Integer), ByVal pstrNombreMoneda As String, ByVal pdblValorUnidadInicial As System.Nullable(Of Double), ByVal pdblValorUnidadVigente As System.Nullable(Of Double), ByVal pstrIdentificadorCuenta As String, ByVal pstrOtroPrefijo As String, ByVal pdtmFechaInicio As System.Nullable(Of System.DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of System.DateTime), ByVal pstrTipoPlazo As String, ByVal pstrPactoPermanencia As String, ByVal pintDiasPlazo As System.Nullable(Of Integer), ByVal pintDiasGracia As System.Nullable(Of Integer), ByVal pstrPenalidad As String, ByVal pdblFactorPenalidad As System.Nullable(Of Double), ByVal pstrFondoCapitalPrivado As String, ByVal pstrCompartimentos As String, ByVal pdblLimiteMontoInversion As System.Nullable(Of Double), ByVal pstrInscritoRNVE As String, ByVal pstrIdEspecie As String, ByVal pstrISIN As String, ByVal pstrParticipacion As String, ByVal pstrComisionAdministracion As String, ByVal pstrTipoComision As String, ByVal pdblTasaComision As System.Nullable(Of Double), ByVal pdblValorComision As System.Nullable(Of Double), ByVal pstrPeriodoComision As String, ByVal pstrOtrasComisiones As String, ByVal pstrDeExito As String, ByVal pdblComisionExito As System.Nullable(Of Double), ByVal pdblValorExito As System.Nullable(Of Double), ByVal pstrEntrada As String, ByVal pdblComisionEntrada As System.Nullable(Of Double), ByVal pdblValorComisionEntrada As System.Nullable(Of Double), ByVal pstrSalida As String, ByVal pdblComisionSalida As System.Nullable(Of Double), ByVal pdblValorComisionSalida As System.Nullable(Of Double), ByVal pstrNombreCorto As String, ByVal pstrTipoAutorizados As String, ByVal pstrNumeroDocumentoClientePartidas As String, ByVal pstrNombreclientepartidas As String, ByVal plngIDBancos As Integer, ByVal pstrSubtipoNegocio As String, ByVal pstrCategoria As String, ByVal pstrComisionGestor As String, ByVal pstrTipoComisionGestor As String, ByVal pdblTasaComisionGestor As System.Nullable(Of Double), ByVal pstrComisionMinima As String, ByVal pdblValorComisionMin As System.Nullable(Of Double), ByVal plngIDComitenteClientePartidas As String, ByVal pstrTipoAcumuladorComision As String, ByVal pstrIvaComisionAdmon As String, ByVal pstrIvaComisionGestor As String, ByVal pstrFondoRenovable As String, ByVal pstrBaseComisionAdmon As String, ByVal pstrPeriodoCobroComisionAdmon As String, ByVal pxmlDetalleGrid As String, ByVal pxmlDetalleGridPenalidad As String, pxmlDetalleGrdTesoreria As String, pxmlDetalleGridLimites As String, ByVal pxmlDetalleGridAutorizacion As String, ByVal pxmlDetalleGridCondicionesTesoreria As String, ByVal pxmlDetalleGridParametros As String, ByVal pxmlDetalleAcumuladoComisiones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByVal pstrCuentaOmnibus As String, ByVal pintIDMonedaComision As System.Nullable(Of Integer), ByVal pstrNombreMonedaComision As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_Companias_Actualizar(pintIDCompania, strTipoDocumento, pstrNumeroDocumento, pstrNombre, pstrCompaniaContable, pstrTipoCompania, pstrCodigoSuper, plogActiva, pstrContabDividendos, pdtmApertura, pdtmUltimoCierre, pstrProveedorPrecios, pstrTipoEntidadSuper, pintIdDinamicaContableConfig, pintIdGestor, pstrNombreGestor, plogManejaValorUnidad, pintIDMoneda, pstrNombreMoneda, pdblValorUnidadInicial, pdblValorUnidadVigente, pstrIdentificadorCuenta, pstrOtroPrefijo, pdtmFechaInicio, pdtmFechaVencimiento, pstrTipoPlazo, pstrPactoPermanencia, pintDiasPlazo, pintDiasGracia, pstrPenalidad, pdblFactorPenalidad, pstrFondoCapitalPrivado, pstrCompartimentos, pdblLimiteMontoInversion, pstrInscritoRNVE, pstrIdEspecie, pstrISIN, pstrParticipacion, pstrComisionAdministracion, pstrTipoComision, pdblTasaComision, pdblValorComision, pstrPeriodoComision, pstrOtrasComisiones, pstrDeExito, pdblComisionExito, pdblValorExito, pstrEntrada, pdblComisionEntrada, pdblValorComisionEntrada, pstrSalida, pdblComisionSalida, pdblValorComisionSalida, pstrNombreCorto, pstrTipoAutorizados, pstrNumeroDocumentoClientePartidas, pstrNombreclientepartidas, plngIDBancos, pstrSubtipoNegocio, pstrCategoria, pstrComisionGestor, pstrTipoComisionGestor, pdblTasaComisionGestor, pstrComisionMinima, pdblValorComisionMin, plngIDComitenteClientePartidas, pstrTipoAcumuladorComision, pstrIvaComisionAdmon, pstrIvaComisionGestor, pstrFondoRenovable, pstrBaseComisionAdmon, pstrPeriodoCobroComisionAdmon, pxmlDetalleGrid, pxmlDetalleGridPenalidad, pxmlDetalleGrdTesoreria, pxmlDetalleGridLimites, pxmlDetalleGridAutorizacion, pxmlDetalleGridCondicionesTesoreria, pxmlDetalleGridParametros, pxmlDetalleAcumuladoComisiones, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarCompanias"), 0, pstrMsgValidacion, pstrCuentaOmnibus, pintIDMonedaComision, pstrNombreMonedaComision)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarCompanias")
            Return Nothing
        End Try
    End Function

    'JEPM20150806
    ''' <summary>
    ''' FUnción encargada de validar la existencia de una compañía en encuenta.
    ''' </summary>
    ''' <param name="pstrCompaniaContable">Texto que ingresa el usuario en la caja de texto Compaía contable</param>
    ''' <param name="pstrUsuario">Nombre del usuario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function VerificarExistenciaCompaniaEnCuenta(ByVal pstrCompaniaContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pintCompaniaExiste As Integer = 0
            Me.DataContext.uspCalculosFinancieros_VerificarExistenciaCompaniaEnCuenta(pstrCompaniaContable, pintCompaniaExiste, pstrUsuario, DemeInfoSesion(pstrUsuario, "VerificarExistenciaCompaniaEnCuenta"), Constantes.ERROR_PERSONALIZADO_SQLSERVER)
            Return pintCompaniaExiste
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarExistenciaCompaniaEnCuenta")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCodigosOyD(ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasCodigos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Companias_ConsutarCodigosOyD(pstrNroDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodigosOyD"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCodigosOyD")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompaniaAsociadaComitente(ByVal pstrIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniaComitente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniaAsociadaCliente_Consultar(pstrIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniaAsociadaComitente"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniaAsociadaComitente")
            Return Nothing
        End Try
    End Function

    Public Function ObtenerCodigoOyDCliente(ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrComitente As String = ""
            Me.DataContext.uspCalculosFinancieros_Companias_ConsutarCodigosOyDComitente(pstrNroDocumento, pstrComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "ObtenerCodigoOyDCliente"), Constantes.ERROR_PERSONALIZADO_SQLSERVER)
            Return pstrComitente
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerCodigoOyDCliente")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function FiltrarCompaniasSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Companias)
        Dim objTask As Task(Of List(Of Companias)) = Me.FiltrarCompaniasAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarCompaniasAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Companias))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Companias)) = New TaskCompletionSource(Of List(Of Companias))()
        objTaskComplete.TrySetResult(FiltrarCompanias(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasSync(ByVal pstrTipoDocumento As String, ByVal pstrNumeroDocumento As String, ByVal pstrNombre As String, ByVal pstrTipoCompania As String, ByVal pstrTipoPlazo As String, ByVal pstrParticipacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Companias)
        Dim objTask As Task(Of List(Of Companias)) = Me.ConsultarCompaniasAsync(pstrTipoDocumento, pstrNumeroDocumento, pstrNombre, pstrTipoCompania, pstrTipoPlazo, pstrParticipacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasAsync(ByVal pstrTipoDocumento As String, ByVal pstrNumeroDocumento As String, ByVal pstrNombre As String, ByVal pstrTipoCompania As String, ByVal pstrTipoPlazo As String, ByVal pstrParticipacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Companias))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Companias)) = New TaskCompletionSource(Of List(Of Companias))()
        objTaskComplete.TrySetResult(ConsultarCompanias(pstrTipoDocumento, pstrNumeroDocumento, pstrNombre, pstrTipoCompania, pstrTipoPlazo, pstrParticipacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Companias
        Dim objTask As Task(Of Companias) = Me.ConsultarCompaniasPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Companias)
        Dim objTaskComplete As TaskCompletionSource(Of Companias) = New TaskCompletionSource(Of Companias)()
        objTaskComplete.TrySetResult(ConsultarCompaniasPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Función para realizar llamado sincrónico al método ActualizarCompaniasAsync y controlar los mensajes de salida
    ''' </summary><history>
    ''' Descripción      : Actualización. Nuevos campos strContabDividendos, dtmApertura, dtmUltimoCierre, strProveedorPrecios.
    ''' Fecha            : Agosto 12/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Agosto 12/2015 - Resultado Ok 
    ''' 'JEPM201500812
    ''' 
    ''' Descripción      : Actualización. Nuevos campos pintIDMonedaComision y pstrNombreMonedaComision para el manejo de la moneda para la comisión.
    ''' Fecha            : Marzo 17/2020
    ''' Responsable      : Germán Arbey González Osorio (A2)
    ''' ControlCambios   : 20200317
    ''' </history>
    Public Function ActualizarCompaniasSync(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal strTipoDocumento As String, ByVal pstrNumeroDocumento As String, ByVal pstrNombre As String, ByVal pstrCompaniaContable As String, ByVal pstrTipoCompania As String, ByVal pstrCodigoSuper As String, ByVal plogActiva As System.Nullable(Of System.Boolean), ByVal pstrContabDividendos As String, ByVal pdtmApertura As System.Nullable(Of System.DateTime), ByVal pdtmUltimoCierre As System.Nullable(Of System.DateTime), ByVal pstrProveedorPrecios As String, ByVal pstrTipoEntidadSuper As String, ByVal pintIdDinamicaContableConfig As System.Nullable(Of Integer), ByVal pintIdGestor As System.Nullable(Of Integer), ByVal pstrNombreGestor As String, ByVal plogManejaValorUnidad As System.Nullable(Of System.Boolean), ByVal pintIDMoneda As System.Nullable(Of Integer), ByVal pstrNombreMoneda As String, ByVal pdblValorUnidadInicial As System.Nullable(Of Double), ByVal pdblValorUnidadVigente As System.Nullable(Of Double), ByVal pstrIdentificadorCuenta As String, ByVal pstrOtroPrefijo As String, ByVal pdtmFechaInicio As System.Nullable(Of System.DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of System.DateTime), ByVal pstrTipoPlazo As String, ByVal pstrPactoPermanencia As String, ByVal pintDiasPlazo As System.Nullable(Of Integer), ByVal pintDiasGracia As System.Nullable(Of Integer), ByVal pstrPenalidad As String, ByVal pdblFactorPenalidad As System.Nullable(Of Double), ByVal pstrFondoCapitalPrivado As String, ByVal pstrCompartimentos As String, ByVal pdblLimiteMontoInversion As System.Nullable(Of Double), ByVal pstrInscritoRNVE As String, ByVal pstrIdEspecie As String, ByVal pstrIsin As String, ByVal pstrParticipacion As String, ByVal pstrComisionAdministracion As String, ByVal pstrTipoComision As String, ByVal pdblTasaComision As System.Nullable(Of Double), ByVal pdblValorComision As System.Nullable(Of Double), ByVal pstrPeriodoComision As String, ByVal pstrOtrasComisiones As String, ByVal pstrDeExito As String, ByVal pdblComisionExito As System.Nullable(Of Double), ByVal pdblValorExito As System.Nullable(Of Double), ByVal pstrEntrada As String, ByVal pdblComisionEntrada As System.Nullable(Of Double), ByVal pdblValorComisionEntrada As System.Nullable(Of Double), ByVal pstrSalida As String, ByVal pdblComisionSalida As System.Nullable(Of Double), ByVal pdblValorComisionSalida As System.Nullable(Of Double), ByVal pstrNombreCorto As String, ByVal pstrTipoAutorizados As String, ByVal pstrNumeroDocumentoClientePartidas As String, ByVal pstrNombreclientepartidas As String, ByVal plngIDBancos As Integer, ByVal pstrSubtipoNegocio As String, ByVal pstrCategoria As String, ByVal pstrComisionGestor As String, ByVal pstrTipoComisionGestor As String, ByVal pdblTasaComisionGestor As System.Nullable(Of Double), ByVal pstrComisionMinima As String, ByVal pdblValorComisionMin As System.Nullable(Of Double), ByVal plngIDComitenteClientePartidas As String, ByVal pstrTipoAcumuladorComision As String, ByVal pstrIvaComisionAdmon As String, ByVal pstrIvaComisionGestor As String, ByVal pstrFondoRenovable As String, ByVal pstrBaseComisionAdmon As String, ByVal pstrPeriodoCobroComisionAdmon As String, ByVal pxmlDetalleGrid As String, ByVal pxmlDetalleGridPenalidad As String, ByVal pxmlDetalleGrdTesoreria As String, ByVal pxmlDetalleGridLimites As String, ByVal pxmlDetalleGridAutorizacion As String, ByVal pxmlDetalleGridCondicionesTesoreria As String, ByVal pxmlDetalleGridParametros As String, ByVal pxmlDetalleAcumuladoComisiones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByVal pstrCuentaOmnibus As String, ByVal pintIDMonedaComision As System.Nullable(Of Integer), ByVal pstrNombreMonedaComision As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarCompaniasAsync(pintIDCompania, strTipoDocumento, pstrNumeroDocumento, pstrNombre, pstrCompaniaContable, pstrTipoCompania, pstrCodigoSuper, plogActiva, pstrContabDividendos, pdtmApertura, pdtmUltimoCierre, pstrProveedorPrecios, pstrTipoEntidadSuper, pintIdDinamicaContableConfig, pintIdGestor, pstrNombreGestor, plogManejaValorUnidad, pintIDMoneda, pstrNombreMoneda, pdblValorUnidadInicial, pdblValorUnidadVigente, pstrIdentificadorCuenta, pstrOtroPrefijo, pdtmFechaInicio, pdtmFechaVencimiento, pstrTipoPlazo, pstrPactoPermanencia, pintDiasPlazo, pintDiasGracia, pstrPenalidad, pdblFactorPenalidad, pstrFondoCapitalPrivado, pstrCompartimentos, pdblLimiteMontoInversion, pstrInscritoRNVE, pstrIdEspecie, pstrIsin, pstrParticipacion, pstrComisionAdministracion, pstrTipoComision, pdblTasaComision, pdblValorComision, pstrPeriodoComision, pstrOtrasComisiones, pstrDeExito, pdblComisionExito, pdblValorExito, pstrEntrada, pdblComisionEntrada, pdblComisionEntrada, pstrSalida, pdblComisionSalida, pdblValorComisionSalida, pstrNombreCorto, pstrTipoAutorizados, pstrNumeroDocumentoClientePartidas, pstrNombreclientepartidas, plngIDBancos, pstrSubtipoNegocio, pstrCategoria, pstrComisionGestor, pstrTipoComisionGestor, pdblTasaComisionGestor, pstrComisionMinima, pdblValorComisionMin, plngIDComitenteClientePartidas, pstrTipoAcumuladorComision, pstrIvaComisionAdmon, pstrIvaComisionGestor, pstrFondoRenovable, pstrBaseComisionAdmon, pstrPeriodoCobroComisionAdmon, pxmlDetalleGrid, pxmlDetalleGridPenalidad, pxmlDetalleGrdTesoreria, pxmlDetalleGridLimites, pxmlDetalleGridAutorizacion, pxmlDetalleGridCondicionesTesoreria, pxmlDetalleGridParametros, pxmlDetalleAcumuladoComisiones, pstrUsuario, pstrInfoConexion, pstrCuentaOmnibus, pintIDMonedaComision, pstrNombreMonedaComision)
        objTask.Wait()
        Return objTask.Result
    End Function
    ''' <summary>
    ''' Función para realizar el llamado al proceso ActualizarCompanias
    ''' </summary>
    ''' <history>
    ''' Descripción      : Actualización. Nuevos campos strContabDividendos, dtmApertura, dtmUltimoCierre, strProveedorPrecios.
    ''' Fecha            : Agosto 12/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Agosto 12/2015 - Resultado Ok 
    ''' 'JEPM201500812
    ''' 
    ''' Descripción      : Actualización. Nuevos campos pintIDMonedaComision y pstrNombreMonedaComision para el manejo de la moneda para la comisión.
    ''' Fecha            : Marzo 17/2020
    ''' Responsable      : Germán Arbey González Osorio (A2)
    ''' ControlCambios   : 20200317
    ''' </history>
    Private Function ActualizarCompaniasAsync(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal strTipoDocumento As String, ByVal pstrNumeroDocumento As String, ByVal pstrNombre As String, ByVal pstrCompaniaContable As String, ByVal pstrTipoCompania As String, ByVal pstrCodigoSuper As String, ByVal plogActiva As System.Nullable(Of System.Boolean), ByVal pstrContabDividendos As String, ByVal pdtmApertura As System.Nullable(Of System.DateTime), ByVal pdtmUltimoCierre As System.Nullable(Of System.DateTime), ByVal pstrProveedorPrecios As String, ByVal pstrTipoEntidadSuper As String, ByVal pintIdDinamicaContableConfig As System.Nullable(Of Integer), ByVal pintIdGestor As System.Nullable(Of Integer), ByVal pstrNombreGestor As String, ByVal plogManejaValorUnidad As System.Nullable(Of System.Boolean), ByVal pintIDMoneda As System.Nullable(Of Integer), ByVal pstrNombreMoneda As String, ByVal pdblValorUnidadInicial As System.Nullable(Of Double), ByVal pdblValorUnidadVigente As System.Nullable(Of Double), ByVal pstrIdentificadorCuenta As String, ByVal pstrOtroPrefijo As String, ByVal pdtmFechaInicio As System.Nullable(Of System.DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of System.DateTime), ByVal pstrTipoPlazo As String, ByVal pstrPactoPermanencia As String, ByVal pintDiasPlazo As System.Nullable(Of Integer), ByVal pintDiasGracia As System.Nullable(Of Integer), ByVal pstrPenalidad As String, ByVal pdblFactorPenalidad As System.Nullable(Of Double), ByVal pstrFondoCapitalPrivado As String, ByVal pstrCompartimentos As String, ByVal pdblLimiteMontoInversion As System.Nullable(Of Double), ByVal pstrInscritoRNVE As String, ByVal pstrIdEspecie As String, ByVal pstrIsin As String, ByVal pstrParticipacion As String, ByVal pstrComisionAdministracion As String, ByVal pstrTipoComision As String, ByVal pdblTasaComision As System.Nullable(Of Double), ByVal pdblValorComision As System.Nullable(Of Double), ByVal pstrPeriodoComision As String, ByVal pstrOtrasComisiones As String, ByVal pstrDeExito As String, ByVal pdblComisionExito As System.Nullable(Of Double), ByVal pdblValorExito As System.Nullable(Of Double), ByVal pstrEntrada As String, ByVal pdblComisionEntrada As System.Nullable(Of Double), ByVal pdblValorComisionEntrada As System.Nullable(Of Double), ByVal pstrSalida As String, ByVal pdblComisionSalida As System.Nullable(Of Double), ByVal pdblValorComisionSalida As System.Nullable(Of Double), ByVal pstrNombreCorto As String, ByVal pstrTipoAutorizados As String, ByVal pstrNumeroDocumentoClientePartidas As String, ByVal pstrNombreclientepartidas As String, ByVal plngIDBancos As Integer, ByVal pstrSubtipoNegocio As String, ByVal pstrCategoria As String, ByVal pstrComisionGestor As String, ByVal pstrTipoComisionGestor As String, ByVal pdblTasaComisionGestor As System.Nullable(Of Double), ByVal pstrComisionMinima As String, ByVal pdblValorComisionMin As System.Nullable(Of Double), ByVal plngIDComitenteClientePartidas As String, ByVal pstrTipoAcumuladorComision As String, ByVal pstrIvaComisionAdmon As String, ByVal pstrIvaComisionGestor As String, ByVal pstrFondoRenovable As String, ByVal pstrBaseComisionAdmon As String, ByVal pstrPeriodoCobroComisionAdmon As String, ByVal pxmlDetalleGrid As String, ByVal pxmlDetalleGridPenalidad As String, ByVal pxmlDetalleGrdTesoreria As String, ByVal pxmlDetalleGridLimites As String, ByVal pxmlDetalleGridAutorizacion As String, ByVal pxmlDetalleGridCondicionesTesoreria As String, ByVal pxmlDetalleGridParametros As String, ByVal pxmlDetalleAcumuladoComisiones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByVal pstrCuentaOmnibus As String, ByVal pintIDMonedaComision As System.Nullable(Of Integer), ByVal pstrNombreMonedaComision As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarCompanias(pintIDCompania, strTipoDocumento, pstrNumeroDocumento, pstrNombre, pstrCompaniaContable, pstrTipoCompania, pstrCodigoSuper, plogActiva, pstrContabDividendos, pdtmApertura, pdtmUltimoCierre, pstrProveedorPrecios, pstrTipoEntidadSuper, pintIdDinamicaContableConfig, pintIdGestor, pstrNombreGestor, plogManejaValorUnidad, pintIDMoneda, pstrNombreMoneda, pdblValorUnidadInicial, pdblValorUnidadVigente, pstrIdentificadorCuenta, pstrOtroPrefijo, pdtmFechaInicio, pdtmFechaVencimiento, pstrTipoPlazo, pstrPactoPermanencia, pintDiasPlazo, pintDiasGracia, pstrPenalidad, pdblFactorPenalidad, pstrFondoCapitalPrivado, pstrCompartimentos, pdblLimiteMontoInversion, pstrInscritoRNVE, pstrIdEspecie, pstrIsin, pstrParticipacion, pstrComisionAdministracion, pstrTipoComision, pdblTasaComision, pdblValorComision, pstrPeriodoComision, pstrOtrasComisiones, pstrDeExito, pdblComisionExito, pdblValorExito, pstrEntrada, pdblComisionEntrada, pdblComisionEntrada, pstrSalida, pdblComisionSalida, pdblValorComisionSalida, pstrNombreCorto, pstrTipoAutorizados, pstrNumeroDocumentoClientePartidas, pstrNombreclientepartidas, plngIDBancos, pstrSubtipoNegocio, pstrCategoria, pstrComisionGestor, pstrTipoComisionGestor, pdblTasaComisionGestor, pstrComisionMinima, pdblValorComisionMin, plngIDComitenteClientePartidas, pstrTipoAcumuladorComision, pstrIvaComisionAdmon, pstrIvaComisionGestor, pstrFondoRenovable, pstrBaseComisionAdmon, pstrPeriodoCobroComisionAdmon, pxmlDetalleGrid, pxmlDetalleGridPenalidad, pxmlDetalleGrdTesoreria, pxmlDetalleGridLimites, pxmlDetalleGridAutorizacion, pxmlDetalleGridCondicionesTesoreria, pxmlDetalleGridParametros, pxmlDetalleAcumuladoComisiones, pstrUsuario, pstrInfoConexion, pstrCuentaOmnibus, pintIDMonedaComision, pstrNombreMonedaComision))
        Return (objTaskComplete.Task)
    End Function

    Public Function VerificarExistenciaCompaniaEnCuentaSync(ByVal pstrCompaniaContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Dim objTask As Task(Of Integer) = Me.VerificarExistenciaCompaniaEnCuentaAsync(pstrCompaniaContable, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function VerificarExistenciaCompaniaEnCuentaAsync(ByVal pstrCompaniaContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Integer)
        Dim objTaskComplete As TaskCompletionSource(Of Integer) = New TaskCompletionSource(Of Integer)()
        objTaskComplete.TrySetResult(VerificarExistenciaCompaniaEnCuenta(pstrCompaniaContable, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniaAsociadaComitenteSync(ByVal pstrIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniaComitente)
        Dim objTask As Task(Of List(Of CompaniaComitente)) = Me.ConsultarCompaniaAsociadaComitenteAsync(pstrIDComitente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniaAsociadaComitenteAsync(ByVal pstrIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniaComitente))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniaComitente)) = New TaskCompletionSource(Of List(Of CompaniaComitente))()
        objTaskComplete.TrySetResult(ConsultarCompaniaAsociadaComitente(pstrIDComitente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
    Private Function ObtenerCodigoOyDClienteSync(ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of Integer) = Me.ObtenerCodigoOyDClienteAsync(pstrNroDocumento, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ObtenerCodigoOyDClienteAsync(ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Integer)
        Dim objTaskComplete As TaskCompletionSource(Of Integer) = New TaskCompletionSource(Of Integer)()
        objTaskComplete.TrySetResult(ObtenerCodigoOyDCliente(pstrNroDocumento, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


#End Region

#End Region

#Region "CompaniasCodigos"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCompaniasCodigos(ByVal objCompaniasCodigos As CompaniasCodigos)

    End Sub

    Public Sub UpdateCompaniasCodigos(ByVal currentCompaniasCodigos As CompaniasCodigos)

    End Sub

    Public Sub DeleteCompaniasCodigos(ByVal UpdateCompaniasCodigos As CompaniasCodigos)

    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarCompaniasCodigos(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasCodigos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasCodigos_Consultar(String.Empty, pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContable"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasCodigos")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompaniasCodigosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasCodigos
        Dim objCompaniasCodigos As CompaniasCodigos = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasCodigos_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodificacionContablePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasCodigos = ret.FirstOrDefault
            End If
            Return objCompaniasCodigos
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasCodigosPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarCompaniasCodigosSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasCodigos)
        Dim objTask As Task(Of List(Of CompaniasCodigos)) = Me.ConsultarCompaniasCodigosAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasCodigosAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniasCodigos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniasCodigos)) = New TaskCompletionSource(Of List(Of CompaniasCodigos))()
        objTaskComplete.TrySetResult(ConsultarCompaniasCodigos(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasCodigosPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasCodigos
        Dim objTask As Task(Of CompaniasCodigos) = Me.ConsultarCompaniasCodigosPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasCodigosPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CompaniasCodigos)
        Dim objTaskComplete As TaskCompletionSource(Of CompaniasCodigos) = New TaskCompletionSource(Of CompaniasCodigos)()
        objTaskComplete.TrySetResult(ConsultarCompaniasCodigosPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "DerechosPatrimoniales"

#Region "Métodos asincrónicos"

    Public Function DerechosPatrimoniales_Validar(pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_DerechosPatrimoniales_Validar(pdtmFechaProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "DerechosPatrimoniales_Validar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_Validar")
            Return Nothing
        End Try
    End Function

    Public Function DerechosPatrimoniales_Acciones_Procesar(pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_DerechosPatrimoniales_Acciones_Procesar(pdtmFechaProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "DerechosPatrimoniales_Procesar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_Procesar")
            Return Nothing
        End Try
    End Function

    Public Function DerechosPatrimoniales_RentaFija_Procesar(pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_DerechosPatrimoniales_RentaFija_Procesar(pdtmFechaProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "DerechosPatrimoniales_Procesar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_Procesar")
            Return Nothing
        End Try
    End Function

    Public Function DerechosPatrimoniales_Carga_Consultar(pdtmFechaProceso As System.Nullable(Of System.DateTime), pstrTipoArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_DerechosPatrimoniales_Carga_Consultar(pdtmFechaProceso, pstrTipoArchivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "DerechosPatrimoniales_Carga_Consultar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_Carga_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function DerechosPatrimoniales_Deceval_Importar(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrModulo, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim objStringordenes As StringBuilder = Nothing

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim ret = Me.DataContext.uspCalculosFinancieros_DerechosPatrimoniales_Deceval_Importar(pdtmFechaProceso, pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_Deceval_Importar")
            Return Nothing
        End Try
    End Function

    Public Function DerechosPatrimoniales_DCV_Importar(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrModulo, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim objStringordenes As StringBuilder = Nothing

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim ret = Me.DataContext.uspCalculosFinancieros_DerechosPatrimoniales_DCV_Importar(pdtmFechaProceso, pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_DCV_Importar")
            Return Nothing
        End Try
    End Function

    Public Function DerechosPatrimoniales_Otros_Importar(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrModulo, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim objStringordenes As StringBuilder = Nothing

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim ret = Me.DataContext.uspCalculosFinancieros_DerechosPatrimoniales_Otros_Importar(pdtmFechaProceso, pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_Otros_Importar")
            Return Nothing
        End Try
    End Function

    Public Function DerechosPatrimoniales_Pagar(pdtmFechaProceso As System.Nullable(Of System.DateTime), pstrTipoCliente As String, plngIDComitente As String, pstrTipoResultado As String, pdblRazonabilidad As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_DerechosPatrimoniales_Pagar(pdtmFechaProceso, pstrTipoCliente, plngIDComitente, pstrTipoResultado, pdblRazonabilidad, pstrUsuario, DemeInfoSesion(pstrUsuario, "DerechosPatrimoniales_Carga_Consultar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_Pagar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function DerechosPatrimoniales_ValidarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.DerechosPatrimoniales_ValidarAsync(pdtmFechaProceso, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function DerechosPatrimoniales_ValidarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(DerechosPatrimoniales_Validar(pdtmFechaProceso, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function DerechosPatrimoniales_Acciones_ProcesarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.DerechosPatrimoniales_Acciones_ProcesarAsync(pdtmFechaProceso, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function DerechosPatrimoniales_Acciones_ProcesarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(DerechosPatrimoniales_Acciones_Procesar(pdtmFechaProceso, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function DerechosPatrimoniales_RentaFija_ProcesarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.DerechosPatrimoniales_RentaFija_ProcesarAsync(pdtmFechaProceso, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function DerechosPatrimoniales_RentaFija_ProcesarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(DerechosPatrimoniales_RentaFija_Procesar(pdtmFechaProceso, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function DerechosPatrimoniales_Carga_ConsultarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.DerechosPatrimoniales_Carga_ConsultarAsync(pdtmFechaProceso, pstrTipoArchivo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function DerechosPatrimoniales_Carga_ConsultarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(DerechosPatrimoniales_Carga_Consultar(pdtmFechaProceso, pstrTipoArchivo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function DerechosPatrimoniales_Deceval_ImportarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Dim objTask As Task(Of List(Of RespuestaArchivoImportacion)) = Me.DerechosPatrimoniales_Deceval_ImportarAsync(pdtmFechaProceso, pstrModulo, pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function DerechosPatrimoniales_Deceval_ImportarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaArchivoImportacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaArchivoImportacion)) = New TaskCompletionSource(Of List(Of RespuestaArchivoImportacion))()
        objTaskComplete.TrySetResult(DerechosPatrimoniales_Deceval_Importar(pdtmFechaProceso, pstrModulo, pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function DerechosPatrimoniales_DCV_ImportarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Dim objTask As Task(Of List(Of RespuestaArchivoImportacion)) = Me.DerechosPatrimoniales_DCV_ImportarAsync(pdtmFechaProceso, pstrModulo, pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function DerechosPatrimoniales_DCV_ImportarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaArchivoImportacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaArchivoImportacion)) = New TaskCompletionSource(Of List(Of RespuestaArchivoImportacion))()
        objTaskComplete.TrySetResult(DerechosPatrimoniales_DCV_Importar(pdtmFechaProceso, pstrModulo, pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function DerechosPatrimoniales_Otros_ImportarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Dim objTask As Task(Of List(Of RespuestaArchivoImportacion)) = Me.DerechosPatrimoniales_Otros_ImportarAsync(pdtmFechaProceso, pstrModulo, pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function DerechosPatrimoniales_Otros_ImportarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal plogEliminarRegistrosTodos As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaArchivoImportacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaArchivoImportacion)) = New TaskCompletionSource(Of List(Of RespuestaArchivoImportacion))()
        objTaskComplete.TrySetResult(DerechosPatrimoniales_Otros_Importar(pdtmFechaProceso, pstrModulo, pstrNombreCompletoArchivo, pstrUsuario, plogEliminarRegistrosTodos, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function DerechosPatrimoniales_PagarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoCliente As String, ByVal plngIDComitente As String, ByVal pstrTipoResultado As String, ByVal pdblRazonabilidad As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.DerechosPatrimoniales_PagarAsync(pdtmFechaProceso, pstrTipoCliente, plngIDComitente, pstrTipoResultado, pdblRazonabilidad, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function DerechosPatrimoniales_PagarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoCliente As String, ByVal plngIDComitente As String, ByVal pstrTipoResultado As String, ByVal pdblRazonabilidad As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(DerechosPatrimoniales_Pagar(pdtmFechaProceso, pstrTipoCliente, plngIDComitente, pstrTipoResultado, pdblRazonabilidad, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "MvtoCustodiasAplicados"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertMvtoCustodiasAplicados(ByVal newMvtoCustodiasAplicados As MvtoCustodiasAplicados)

    End Sub

    Public Sub UpdateMvtoCustodiasAplicados(ByVal currentMvtoCustodiasAplicados As MvtoCustodiasAplicados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentMvtoCustodiasAplicados.pstrUsuarioConexion, currentMvtoCustodiasAplicados.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentMvtoCustodiasAplicados.strInfoSesion = DemeInfoSesion(currentMvtoCustodiasAplicados.pstrUsuarioConexion, "DeleteMvtoCustodiasAplicados")
            Me.DataContext.MvtoCustodiasAplicados.Attach(currentMvtoCustodiasAplicados)
            Me.DataContext.MvtoCustodiasAplicados.DeleteOnSubmit(currentMvtoCustodiasAplicados)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteMvtoCustodiasAplicados")
        End Try
    End Sub

    Public Sub DeleteMvtoCustodiasAplicados(ByVal deleteMvtoCustodiasAplicados As MvtoCustodiasAplicados)

    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarMvtoCustodiasAplicados(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MvtoCustodiasAplicados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AdministracionMovimientosTitulos_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarMvtoCustodiasAplicados"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarMvtoCustodiasAplicados")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarMvtoCustodiasAplicados(pstrTipo As String, plngIDComitente As String, pstrIDEspecie As String, pdtmFechaMovimiento As System.Nullable(Of System.DateTime), pstrISIN As String, pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MvtoCustodiasAplicados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AdministracionMovimientosTitulos_Consultar(String.Empty, pstrTipo, plngIDComitente, pstrIDEspecie, pdtmFechaMovimiento, pstrISIN, pstrEstado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarMvtoCustodiasAplicados"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarMvtoCustodiasAplicados")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarMvtoCustodiasAplicadosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As MvtoCustodiasAplicados
        Dim objMvtoCustodiasAplicados As MvtoCustodiasAplicados = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AdministracionMovimientosTitulos_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, String.Empty, Nothing, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarMvtoCustodiasAplicadosPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objMvtoCustodiasAplicados = ret.FirstOrDefault
            End If
            Return objMvtoCustodiasAplicados
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarMvtoCustodiasAplicadosPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarMvtoCustodiasAplicadosSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MvtoCustodiasAplicados)
        Dim objTask As Task(Of List(Of MvtoCustodiasAplicados)) = Me.FiltrarMvtoCustodiasAplicadosAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarMvtoCustodiasAplicadosAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of MvtoCustodiasAplicados))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of MvtoCustodiasAplicados)) = New TaskCompletionSource(Of List(Of MvtoCustodiasAplicados))()
        objTaskComplete.TrySetResult(FiltrarMvtoCustodiasAplicados(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarMvtoCustodiasAplicadosSync(ByVal pstrTipo As String, ByVal plngIDComitente As String, ByVal pstrIDEspecie As String, ByVal pdtmFechaMovimiento As System.Nullable(Of System.DateTime), ByVal pstrISIN As String, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MvtoCustodiasAplicados)
        Dim objTask As Task(Of List(Of MvtoCustodiasAplicados)) = Me.ConsultarMvtoCustodiasAplicadosAsync(pstrTipo, plngIDComitente, pstrIDEspecie, pdtmFechaMovimiento, pstrISIN, pstrEstado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarMvtoCustodiasAplicadosAsync(ByVal pstrTipo As String, ByVal plngIDComitente As String, ByVal pstrIDEspecie As String, ByVal pdtmFechaMovimiento As System.Nullable(Of System.DateTime), ByVal pstrISIN As String, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of MvtoCustodiasAplicados))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of MvtoCustodiasAplicados)) = New TaskCompletionSource(Of List(Of MvtoCustodiasAplicados))()
        objTaskComplete.TrySetResult(ConsultarMvtoCustodiasAplicados(pstrTipo, plngIDComitente, pstrIDEspecie, pdtmFechaMovimiento, pstrISIN, pstrEstado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarMvtoCustodiasAplicadosPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As MvtoCustodiasAplicados
        Dim objTask As Task(Of MvtoCustodiasAplicados) = Me.ConsultarMvtoCustodiasAplicadosPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarMvtoCustodiasAplicadosPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of MvtoCustodiasAplicados)
        Dim objTaskComplete As TaskCompletionSource(Of MvtoCustodiasAplicados) = New TaskCompletionSource(Of MvtoCustodiasAplicados)()
        objTaskComplete.TrySetResult(ConsultarMvtoCustodiasAplicadosPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CompaniasPenalidades"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCompaniasPenalidades(ByVal objCompaniasPenalidades As CompaniasPenalidades)

    End Sub

    Public Sub UpdateCompaniasPenalidades(ByVal currentCompaniasPenalidades As CompaniasPenalidades)

    End Sub

    Public Sub DeleteCompaniasPenalidades(ByVal UpdateCompaniasPenalidades As CompaniasPenalidades)

    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarCompaniasPenalidades(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasPenalidades)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasPenalidades_Consultar(String.Empty, pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasPenalidades"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasPenalidades")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompaniasPenalidadesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasPenalidades
        Dim objCompaniasPenalidades As CompaniasPenalidades = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasPenalidades_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasPenalidadesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasPenalidades = ret.FirstOrDefault
            End If
            Return objCompaniasPenalidades
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasPenalidadesPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarCompaniasPenalidadesSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasPenalidades)
        Dim objTask As Task(Of List(Of CompaniasPenalidades)) = Me.ConsultarCompaniasPenalidadesAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasPenalidadesAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniasPenalidades))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniasPenalidades)) = New TaskCompletionSource(Of List(Of CompaniasPenalidades))()
        objTaskComplete.TrySetResult(ConsultarCompaniasPenalidades(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasPenalidadesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasPenalidades
        Dim objTask As Task(Of CompaniasPenalidades) = Me.ConsultarCompaniasPenalidadesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasPenalidadesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CompaniasPenalidades)
        Dim objTaskComplete As TaskCompletionSource(Of CompaniasPenalidades) = New TaskCompletionSource(Of CompaniasPenalidades)()
        objTaskComplete.TrySetResult(ConsultarCompaniasPenalidadesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CompaniasTesoreria"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCompaniasTesoreria(ByVal objCompaniasTesoreria As CompaniasTesoreria)

    End Sub

    Public Sub UpdateCompaniasTesoreria(ByVal currentCompaniasTesoreria As CompaniasTesoreria)

    End Sub

    Public Sub DeleteCompaniasTesoreria(ByVal UpdateCompaniasTesoreria As CompaniasTesoreria)

    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function ConsultarCompaniasTesoreria(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasTesoreria_Consultar(String.Empty, pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasTesoreria"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasTesoreria")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompaniasTesoreriaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasTesoreria
        Dim objCompaniasTesoreria As CompaniasTesoreria = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasTesoreria_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasTesoreriaPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasTesoreria = ret.FirstOrDefault
            End If
            Return objCompaniasTesoreria
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasTesoreriaPorDefecto")
            Return Nothing
        End Try
    End Function



#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarCompaniasTesoreriaSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasTesoreria)
        Dim objTask As Task(Of List(Of CompaniasTesoreria)) = Me.ConsultarCompaniasTesoreriaAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasTesoreriaAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniasTesoreria))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniasTesoreria)) = New TaskCompletionSource(Of List(Of CompaniasTesoreria))()
        objTaskComplete.TrySetResult(ConsultarCompaniasTesoreria(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasTesoreriaPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasTesoreria
        Dim objTask As Task(Of CompaniasTesoreria) = Me.ConsultarCompaniasTesoreriaPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasTesoreriaPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CompaniasTesoreria)
        Dim objTaskComplete As TaskCompletionSource(Of CompaniasTesoreria) = New TaskCompletionSource(Of CompaniasTesoreria)()
        objTaskComplete.TrySetResult(ConsultarCompaniasTesoreriaPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CompaniasLimites"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCompaniasLimites(ByVal objCompaniasTesoreria As CompaniasLimites)

    End Sub

    Public Sub UpdateCompaniasLimites(ByVal currentCompaniasTesoreria As CompaniasLimites)

    End Sub

    Public Sub DeleteCompaniasLimites(ByVal UpdateCompaniasTesoreria As CompaniasLimites)

    End Sub


#End Region

#Region "Métodos asincrónicos"
    Public Function ConsultarCompaniasLimites(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasLimites)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasLimites_Consultar(String.Empty, pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasLimites"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasLimites")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompaniasLimitesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasLimites
        Dim objCompaniasLimites As CompaniasLimites = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasLimites_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "consultarCompaniasLimitesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasLimites = ret.FirstOrDefault
            End If
            Return objCompaniasLimites
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasLimitesPorDefecto")
            Return Nothing
        End Try
    End Function



#End Region

#Region "Métodos sincrónicos"
    Public Function ConsultarCompaniasLimitesSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasLimites)
        Dim objTask As Task(Of List(Of CompaniasLimites)) = Me.ConsultarCompaniasLimitesAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCompaniasLimitesAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniasLimites))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniasLimites)) = New TaskCompletionSource(Of List(Of CompaniasLimites))()
        objTaskComplete.TrySetResult(ConsultarCompaniasLimites(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasLimitesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasLimites
        Dim objTask As Task(Of CompaniasLimites) = Me.ConsultarCompaniasLimitesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCompaniasLimitesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CompaniasLimites)
        Dim objTaskComplete As TaskCompletionSource(Of CompaniasLimites) = New TaskCompletionSource(Of CompaniasLimites)()
        objTaskComplete.TrySetResult(ConsultarCompaniasLimitesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CompaniasAutorizaciones"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCompaniasAutorizaciones(ByVal objCompaniasAutorizaciones As CompaniasAutorizaciones)

    End Sub

    Public Sub UpdateCompaniasAutorizaciones(ByVal currentCompaniasAutorizaciones As CompaniasAutorizaciones)

    End Sub

    Public Sub DeleteCompaniasAutorizaciones(ByVal UpdateCompaniasAutorizaciones As CompaniasAutorizaciones)

    End Sub

#End Region

#Region "Métodos asincrónicos"
    Public Function ConsultarCompaniasAutorizaciones(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasAutorizaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasAutorizaciones_Consultar(String.Empty, pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasAutorizaciones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasAutorizaciones")
            Return Nothing
        End Try
    End Function


    Public Function ConsultarCompaniasAutorizacionesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasAutorizaciones
        Dim objCompaniasAutorizaciones As CompaniasAutorizaciones = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasAutorizaciones_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasAutorizacionesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasAutorizaciones = ret.FirstOrDefault
            End If
            Return objCompaniasAutorizaciones
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasAutorizacionesPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region


#Region "Métodos sincrónicos"
    Public Function ConsultarCompaniasAutorizacionesSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasAutorizaciones)
        Dim objTask As Task(Of List(Of CompaniasAutorizaciones)) = Me.ConsultarCompaniasAutorizacionesAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasAutorizacionesAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniasAutorizaciones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniasAutorizaciones)) = New TaskCompletionSource(Of List(Of CompaniasAutorizaciones))()
        objTaskComplete.TrySetResult(ConsultarCompaniasAutorizaciones(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasAutorizacionesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasAutorizaciones
        Dim objTask As Task(Of CompaniasAutorizaciones) = Me.ConsultarCompaniasAutorizacionesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasAutorizacionesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CompaniasAutorizaciones)
        Dim objTaskComplete As TaskCompletionSource(Of CompaniasAutorizaciones) = New TaskCompletionSource(Of CompaniasAutorizaciones)()
        objTaskComplete.TrySetResult(ConsultarCompaniasAutorizacionesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region



#End Region

#Region "ValoracionFondo"
#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertValoracionFondo(ByVal objValoracionFondo As ValoracionFondo)

    End Sub

    Public Sub UpdateValoracionFondo(ByVal currentValoracionFondo As ValoracionFondo)

    End Sub


    Public Sub DeleteValoracionFondo(ByVal UpdateValoracionFondo As ValoracionFondo)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function GetValoracion(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ValoracionFondo
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Nothing
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetValoracion")
            Return Nothing
        End Try
    End Function

    Public Function ActualizarValoracionFondo(ByVal pstrTipoProceso As String, ByVal pdtmFechaInicioProceso As DateTime, ByVal pdtmFechaFinalProceso As DateTime, ByVal plogCierreContinuo As Boolean, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_CierrePasivo(pstrTipoProceso, pdtmFechaInicioProceso, pdtmFechaFinalProceso, plogCierreContinuo, pxmlCodigosProductos, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarValoracionFondo"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarValoracionFondo")
            Return Nothing
        End Try
    End Function

    Public Function ActualizarDeshacerValoracionFondo(ByVal pdtmFechaProceso As DateTime, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_DeshacerCierrePasivo(pdtmFechaProceso, pxmlCodigosProductos, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarDeshacerValoracionFondo"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarDeshacerValoracionFondo")
            Return Nothing
        End Try
    End Function

    Public Function ValidarDeshacerValoracionFondo(ByVal pdtmFechaProceso As DateTime, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ValidarDeshacerCierrePasivo(pdtmFechaProceso, pxmlCodigosProductos, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarDeshacerValoracionFondo"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarDeshacerValoracionFondo")
            Return Nothing
        End Try
    End Function


#End Region



#Region "Métodos sincrónicos"

    Public Function ActualizarValoracionFondoSync(ByVal pstrTipoProceso As String, ByVal pdtmFechaInicioProceso As DateTime, ByVal pdtmFechaFinalProceso As DateTime, ByVal plogCierreContinuo As Boolean, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarValoracionFondoAsync(pstrTipoProceso, pdtmFechaInicioProceso, pdtmFechaFinalProceso, plogCierreContinuo, pxmlCodigosProductos, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Public Function ActualizarDeshacerValoracionFondoSync(ByVal pdtmFechaProceso As DateTime, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarDeshacerValoracionFondoAsync(pdtmFechaProceso, pxmlCodigosProductos, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Public Function ValidarDeshacerValoracionFondoSync(ByVal pdtmFechaProceso As DateTime, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ValidarDeshacerValoracionFondoAsync(pdtmFechaProceso, pxmlCodigosProductos, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function


    Private Function ActualizarValoracionFondoAsync(ByVal pstrTipoProceso As String, ByVal pdtmFechaInicioProceso As DateTime, ByVal pdtmFechaFinalProceso As DateTime, ByVal plogCierreContinuo As Boolean, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarValoracionFondoSync(pstrTipoProceso, pdtmFechaInicioProceso, pdtmFechaFinalProceso, plogCierreContinuo, pxmlCodigosProductos, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Private Function ActualizarDeshacerValoracionFondoAsync(ByVal pdtmFechaProceso As DateTime, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarDeshacerValoracionFondoSync(pdtmFechaProceso, pxmlCodigosProductos, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Private Function ValidarDeshacerValoracionFondoAsync(ByVal pdtmFechaProceso As DateTime, ByVal pxmlCodigosProductos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ValidarDeshacerValoracionFondoSync(pdtmFechaProceso, pxmlCodigosProductos, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


    Public Function ConsultarAvanceCierrePasivo(pdtmFechaProceso As Nullable(Of Date), pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProcesarPortafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CierrePasivo_AvanceProcesamiento(pdtmFechaProceso, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarAvanceCierrePasivo"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarAvanceCierrePasivo")
            Return Nothing
        End Try
    End Function


#End Region




#End Region

#Region "Libranzas"
#Region "Pantalla libranzas"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertLibranzas_Combos(ByVal currentRegistro As Libranzas_Combos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLibranzas_Combos")
        End Try
    End Sub

    Public Sub UpdateLibranzas_Combos(ByVal currentRegistro As Libranzas_Combos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLibranzas_Combos")
        End Try
    End Sub

    Public Sub DeleteLibranzas_Combos(ByVal currentRegistro As Libranzas_Combos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLibranzas_Combos")
        End Try
    End Sub

    Public Sub InsertLibranzas_RespuestaValidacion(ByVal currentRegistro As Libranzas_RespuestaValidacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLibranzas_RespuestaValidacion")
        End Try
    End Sub

    Public Sub UpdateLibranzas_RespuestaValidacion(ByVal currentRegistro As Libranzas_RespuestaValidacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLibranzas_RespuestaValidacion")
        End Try
    End Sub

    Public Sub DeleteLibranzas_RespuestaValidacion(ByVal currentRegistro As Libranzas_RespuestaValidacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLibranzas_RespuestaValidacion")
        End Try
    End Sub

    Public Sub InsertLibranzas(ByVal currentRegistro As Libranzas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLibranzas")
        End Try
    End Sub

    Public Sub UpdateLibranzas(ByVal currentRegistro As Libranzas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLibranzas")
        End Try
    End Sub

    Public Sub DeleteLibranzas(ByVal currentRegistro As Libranzas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLibranzas")
        End Try
    End Sub

    Public Sub InsertLibranzas_Flujos(ByVal currentRegistro As Libranzas_Flujos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLibranzas_Flujos")
        End Try
    End Sub

    Public Sub UpdateLibranzas_Flujos(ByVal currentRegistro As Libranzas_Flujos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLibranzas_Flujos")
        End Try
    End Sub

    Public Sub DeleteLibranzas_Flujos(ByVal currentRegistro As Libranzas_Flujos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLibranzas_Flujos")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function Libranzas_ConsultarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_Combos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRet = Me.DataContext.uspLibranzas_ConsultarCombos(pstrUsuario, DemeInfoSesion(pstrUsuario, "Libranzas_ConsultarCombos"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_ConsultarCombos")
            Return Nothing
        End Try
    End Function

    Public Function Libranzas_Filtrar(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRet = Me.DataContext.uspLibranzas_Filtrar(pstrEstado, pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "Libranzas_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function Libranzas_Consultar(ByVal pintIDLibranza As Nullable(Of Integer), ByVal pstrIDComitente As String, ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pintIDEmisor As Nullable(Of Integer), ByVal pintIDPagador As Nullable(Of Integer), ByVal pintIDCustodio As Nullable(Of Integer), ByVal pstrNroCredito As String, ByVal pstrTipoRegistro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRet = Me.DataContext.uspLibranzas_Consultar(pintIDLibranza, pstrIDComitente, pdtmFechaRegistro, pintIDEmisor, pintIDPagador, pintIDCustodio, pstrNroCredito, pstrTipoRegistro, pstrUsuario, DemeInfoSesion(pstrUsuario, "Libranzas_Consultar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_Consultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Libranzas_Validar(ByVal pintID As Integer,
                                      ByVal pdtmFechaRegistro As Nullable(Of DateTime),
                                      ByVal pstrNroCredito As String,
                                      ByVal pstrIDComitente As String,
                                      ByVal pintIDCompania As Nullable(Of Integer),
                                      ByVal pintIDEmisor As Nullable(Of Integer),
                                      ByVal pdtmFechaInicioCredito As Nullable(Of DateTime),
                                      ByVal pdtmFechaFinCredito As Nullable(Of DateTime),
                                      ByVal pintNroCuotas As Nullable(Of Integer),
                                      ByVal pdblValorCuotas As Nullable(Of Double),
                                      ByVal pstrPeriodoPago As String,
                                      ByVal pdblValorCreditoOriginal As Nullable(Of Double),
                                      ByVal pstrTipoPago As String,
                                      ByVal pdblTasaInteresCredito As Nullable(Of Double),
                                      ByVal pdblTasaDescuento As Nullable(Of Double),
                                      ByVal pdblValorOperacion As Nullable(Of Double),
                                      ByVal pdblValorCreditoActual As Nullable(Of Double),
                                      ByVal pstrNroDocumentoBeneficiario As String,
                                      ByVal pstrNombreBeneficiario As String,
                                      ByVal pstrTipoIdentificacionBeneficiario As String,
                                      ByVal pintIDPagador As Nullable(Of Integer),
                                      ByVal pintIDCustodio As Nullable(Of Integer),
                                      ByVal pstrTipoRegistro As String,
                                      ByVal pstrObservaciones As String,
                                      ByVal pstrXMLFlujos As String,
                                      ByVal pstrUsuario As String,
                                      ByVal plogRecalcularFlujos As Boolean, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrXMLFlujos = HttpUtility.UrlDecode(pstrXMLFlujos)
            pstrObservaciones = HttpUtility.UrlDecode(pstrObservaciones)

            Dim objRet = Me.DataContext.uspLibranzas_Validar(pintID, pdtmFechaRegistro, pstrNroCredito, pstrIDComitente,
                                                             pintIDCompania, pintIDEmisor, pdtmFechaInicioCredito, pdtmFechaFinCredito,
                                                             pintNroCuotas, pdblValorCuotas, pstrPeriodoPago, pdblValorCreditoOriginal,
                                                             pstrTipoPago, pdblTasaInteresCredito, pdblTasaDescuento, pdblValorOperacion,
                                                             pdblValorCreditoActual, pstrNroDocumentoBeneficiario, pstrNombreBeneficiario,
                                                             pstrTipoIdentificacionBeneficiario, pintIDPagador, pintIDCustodio,
                                                             pstrTipoRegistro, pstrObservaciones, pstrXMLFlujos, pstrUsuario, DemeInfoSesion(pstrUsuario, "Libranzas_Validar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER, plogRecalcularFlujos).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_Validar")
            Return Nothing
        End Try
    End Function

    Public Function Libranzas_Anular(ByVal pintID As Integer,
                                     ByVal pstrObservaciones As String,
                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRet = Me.DataContext.uspLibranzas_Anular(pintID, pstrObservaciones, pstrUsuario, DemeInfoSesion(pstrUsuario, "Libranzas_Anular"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_Anular")
            Return Nothing
        End Try
    End Function

    Public Function Libranzas_FlujosConsultar(ByVal pintIDLibranza As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_Flujos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRet = Me.DataContext.uspLibranzas_ConsultarFlujos(pintIDLibranza, pstrUsuario, DemeInfoSesion(pstrUsuario, "Libranzas_FlujosConsultar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_FlujosConsultar")
            Return Nothing
        End Try
    End Function

    Public Function Libranzas_ValidarEstado(ByVal pintID As Integer,
                                                           ByVal pstrAccion As String,
                                                           ByVal pstrUsuario As String,
                                                           ByVal pstrUsuarioWindows As String,
                                                           ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRet = Me.DataContext.uspLibranzas_ValidarEstado(pintID, pstrAccion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Libranzas_ValidarEstado"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_ValidarEstado")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Libranzas_Calcular(ByVal pstrTipoCalculo As String,
                                       ByVal pdtmFechaRegistro As Nullable(Of DateTime),
                                      ByVal pdtmFechaInicioCredito As Nullable(Of DateTime),
                                      ByVal pdtmFechaFinCredito As Nullable(Of DateTime),
                                      ByVal pintNroCuotas As Nullable(Of Integer),
                                      ByVal pdblValorCuotas As Nullable(Of Double),
                                      ByVal pstrPeriodoPago As String,
                                      ByVal pdblValorCreditoOriginal As Nullable(Of Double),
                                      ByVal pstrTipoPago As String,
                                      ByVal pdblTasaInteresCredito As Nullable(Of Double),
                                      ByVal pdblTasaDescuento As Nullable(Of Double),
                                      ByVal pdblValorOperacion As Nullable(Of Double),
                                      ByVal pdblValorCreditoActual As Nullable(Of Double),
                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_Calculos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRet = Me.DataContext.uspLibranzas_Calcular(pstrTipoCalculo, pdtmFechaRegistro, pdtmFechaInicioCredito,
                                                              pdtmFechaFinCredito, pintNroCuotas, pdblValorCuotas, pstrPeriodoPago,
                                                              pdblValorCreditoOriginal, pstrTipoPago, pdblTasaInteresCredito,
                                                              pdblTasaDescuento, pdblValorOperacion, pdblValorCreditoActual, pstrUsuario, DemeInfoSesion(pstrUsuario, "Libranzas_Calcular"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_Calcular")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function Libranzas_ConsultarCombosSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_Combos)
        Dim objTask As Task(Of List(Of Libranzas_Combos)) = Me.Libranzas_ConsultarCombosAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Libranzas_ConsultarCombosAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Libranzas_Combos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Libranzas_Combos)) = New TaskCompletionSource(Of List(Of Libranzas_Combos))()
        objTaskComplete.TrySetResult(Libranzas_ConsultarCombos(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Libranzas_FiltrarSync(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas)
        Dim objTask As Task(Of List(Of Libranzas)) = Me.Libranzas_FiltrarAsync(pstrEstado, pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Libranzas_FiltrarAsync(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Libranzas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Libranzas)) = New TaskCompletionSource(Of List(Of Libranzas))()
        objTaskComplete.TrySetResult(Libranzas_Filtrar(pstrEstado, pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Libranzas_ConsultarSync(ByVal pintIDLibranza As Nullable(Of Integer), ByVal pstrIDComitente As String, ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pintIDEmisor As Nullable(Of Integer), ByVal pintIDPagador As Nullable(Of Integer), ByVal pintIDCustodio As Nullable(Of Integer), ByVal pstrNroCredito As String, ByVal pstrTipoRegistro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas)
        Dim objTask As Task(Of List(Of Libranzas)) = Me.Libranzas_ConsultarAsync(pintIDLibranza, pstrIDComitente, pdtmFechaRegistro, pintIDEmisor, pintIDPagador, pintIDCustodio, pstrNroCredito, pstrTipoRegistro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Libranzas_ConsultarAsync(ByVal pintIDLibranza As Nullable(Of Integer), ByVal pstrIDComitente As String, ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pintIDEmisor As Nullable(Of Integer), ByVal pintIDPagador As Nullable(Of Integer), ByVal pintIDCustodio As Nullable(Of Integer), ByVal pstrNroCredito As String, ByVal pstrTipoRegistro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Libranzas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Libranzas)) = New TaskCompletionSource(Of List(Of Libranzas))()
        objTaskComplete.TrySetResult(Libranzas_Consultar(pintIDLibranza, pstrIDComitente, pdtmFechaRegistro, pintIDEmisor, pintIDPagador, pintIDCustodio, pstrNroCredito, pstrTipoRegistro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Libranzas_ValidarSync(ByVal pintID As Integer,
                                      ByVal pdtmFechaRegistro As Nullable(Of DateTime),
                                      ByVal pstrNroCredito As String,
                                      ByVal pstrIDComitente As String,
                                      ByVal pintIDCompania As Nullable(Of Integer),
                                      ByVal pintIDEmisor As Nullable(Of Integer),
                                      ByVal pdtmFechaInicioCredito As Nullable(Of DateTime),
                                      ByVal pdtmFechaFinCredito As Nullable(Of DateTime),
                                      ByVal pintNroCuotas As Nullable(Of Integer),
                                      ByVal pdblValorCuotas As Nullable(Of Double),
                                      ByVal pstrPeriodoPago As String,
                                      ByVal pdblValorCreditoOriginal As Nullable(Of Double),
                                      ByVal pstrTipoPago As String,
                                      ByVal pdblTasaInteresCredito As Nullable(Of Double),
                                      ByVal pdblTasaDescuento As Nullable(Of Double),
                                      ByVal pdblValorOperacion As Nullable(Of Double),
                                      ByVal pdblValorCreditoActual As Nullable(Of Double),
                                      ByVal pstrNroDocumentoBeneficiario As String,
                                      ByVal pstrNombreBeneficiario As String,
                                      ByVal pstrTipoIdentificacionBeneficiario As String,
                                      ByVal pintIDPagador As Nullable(Of Integer),
                                      ByVal pintIDCustodio As Nullable(Of Integer),
                                      ByVal pstrTipoRegistro As String,
                                      ByVal pstrObservaciones As String,
                                      ByVal pstrXMLFlujos As String,
                                      ByVal pstrUsuario As String,
                                      ByVal plogRecalcularFlujos As Boolean, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Libranzas_RespuestaValidacion)) = Me.Libranzas_ValidarAsync(pintID,
                                                                                                   pdtmFechaRegistro,
                                                                                                   pstrNroCredito,
                                                                                                   pstrIDComitente,
                                                                                                   pintIDCompania,
                                                                                                   pintIDEmisor,
                                                                                                   pdtmFechaInicioCredito,
                                                                                                   pdtmFechaFinCredito,
                                                                                                   pintNroCuotas,
                                                                                                   pdblValorCuotas,
                                                                                                   pstrPeriodoPago,
                                                                                                   pdblValorCreditoOriginal,
                                                                                                   pstrTipoPago,
                                                                                                   pdblTasaInteresCredito,
                                                                                                   pdblTasaDescuento,
                                                                                                   pdblValorOperacion,
                                                                                                   pdblValorCreditoActual,
                                                                                                   pstrNroDocumentoBeneficiario,
                                                                                                   pstrNombreBeneficiario,
                                                                                                   pstrTipoIdentificacionBeneficiario,
                                                                                                   pintIDPagador,
                                                                                                   pintIDCustodio,
                                                                                                   pstrTipoRegistro,
                                                                                                   pstrObservaciones,
                                                                                                   pstrXMLFlujos,
                                                                                                   pstrUsuario,
                                                                                                   plogRecalcularFlujos,
                                                                                                   pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Libranzas_ValidarAsync(ByVal pintID As Integer,
                                      ByVal pdtmFechaRegistro As Nullable(Of DateTime),
                                      ByVal pstrNroCredito As String,
                                      ByVal pstrIDComitente As String,
                                      ByVal pintIDCompania As Nullable(Of Integer),
                                      ByVal pintIDEmisor As Nullable(Of Integer),
                                      ByVal pdtmFechaInicioCredito As Nullable(Of DateTime),
                                      ByVal pdtmFechaFinCredito As Nullable(Of DateTime),
                                      ByVal pintNroCuotas As Nullable(Of Integer),
                                      ByVal pdblValorCuotas As Nullable(Of Double),
                                      ByVal pstrPeriodoPago As String,
                                      ByVal pdblValorCreditoOriginal As Nullable(Of Double),
                                      ByVal pstrTipoPago As String,
                                      ByVal pdblTasaInteresCredito As Nullable(Of Double),
                                      ByVal pdblTasaDescuento As Nullable(Of Double),
                                      ByVal pdblValorOperacion As Nullable(Of Double),
                                      ByVal pdblValorCreditoActual As Nullable(Of Double),
                                      ByVal pstrNroDocumentoBeneficiario As String,
                                      ByVal pstrNombreBeneficiario As String,
                                      ByVal pstrTipoIdentificacionBeneficiario As String,
                                      ByVal pintIDPagador As Nullable(Of Integer),
                                      ByVal pintIDCustodio As Nullable(Of Integer),
                                      ByVal pstrTipoRegistro As String,
                                      ByVal pstrObservaciones As String,
                                      ByVal pstrXMLFlujos As String,
                                      ByVal pstrUsuario As String,
                                      ByVal plogRecalcularFlujos As Boolean, ByVal pstrInfoConexion As String) As Task(Of List(Of Libranzas_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Libranzas_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Libranzas_RespuestaValidacion))()
        objTaskComplete.TrySetResult(Libranzas_Validar(pintID,
                                                        pdtmFechaRegistro,
                                                        pstrNroCredito,
                                                        pstrIDComitente,
                                                        pintIDCompania,
                                                        pintIDEmisor,
                                                        pdtmFechaInicioCredito,
                                                        pdtmFechaFinCredito,
                                                        pintNroCuotas,
                                                        pdblValorCuotas,
                                                        pstrPeriodoPago,
                                                        pdblValorCreditoOriginal,
                                                        pstrTipoPago,
                                                        pdblTasaInteresCredito,
                                                        pdblTasaDescuento,
                                                        pdblValorOperacion,
                                                        pdblValorCreditoActual,
                                                        pstrNroDocumentoBeneficiario,
                                                        pstrNombreBeneficiario,
                                                        pstrTipoIdentificacionBeneficiario,
                                                        pintIDPagador,
                                                        pintIDCustodio,
                                                        pstrTipoRegistro,
                                                        pstrObservaciones,
                                                        pstrXMLFlujos,
                                                        pstrUsuario,
                                                        plogRecalcularFlujos,
                                                        pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Libranzas_AnularSync(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Libranzas_RespuestaValidacion)) = Me.Libranzas_AnularAsync(pintID, pstrObservaciones, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Libranzas_AnularAsync(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Libranzas_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Libranzas_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Libranzas_RespuestaValidacion))()
        objTaskComplete.TrySetResult(Libranzas_Anular(pintID, pstrObservaciones, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Libranzas_FlujosConsultarSync(ByVal pintIDLibranza As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_Flujos)
        Dim objTask As Task(Of List(Of Libranzas_Flujos)) = Me.Libranzas_FlujosConsultarAsync(pintIDLibranza, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Libranzas_FlujosConsultarAsync(ByVal pintIDLibranza As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Libranzas_Flujos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Libranzas_Flujos)) = New TaskCompletionSource(Of List(Of Libranzas_Flujos))()
        objTaskComplete.TrySetResult(Libranzas_FlujosConsultar(pintIDLibranza, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Libranzas_ValidarEstadoSync(ByVal pintID As Integer,
                                                               ByVal pstrAccion As String,
                                                               ByVal pstrUsuario As String,
                                                               ByVal pstrUsuarioWindows As String,
                                                               ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Libranzas_RespuestaValidacion)) = Me.Libranzas_ValidarEstadoAsync(pintID, pstrAccion, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Libranzas_ValidarEstadoAsync(ByVal pintID As Integer,
                                                                 ByVal pstrAccion As String,
                                                                 ByVal pstrUsuario As String,
                                                                 ByVal pstrUsuarioWindows As String,
                                                                 ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Libranzas_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Libranzas_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Libranzas_RespuestaValidacion))()
        objTaskComplete.TrySetResult(Libranzas_ValidarEstado(pintID, pstrAccion, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Libranzas_CalcularSync(ByVal pstrTipoCalculo As String,
                                           ByVal pdtmFechaRegistro As Nullable(Of DateTime),
                                          ByVal pdtmFechaInicioCredito As Nullable(Of DateTime),
                                          ByVal pdtmFechaFinCredito As Nullable(Of DateTime),
                                          ByVal pintNroCuotas As Nullable(Of Integer),
                                          ByVal pdblValorCuotas As Nullable(Of Double),
                                          ByVal pstrPeriodoPago As String,
                                          ByVal pdblValorCreditoOriginal As Nullable(Of Double),
                                          ByVal pstrTipoPago As String,
                                          ByVal pdblTasaInteresCredito As Nullable(Of Double),
                                          ByVal pdblTasaDescuento As Nullable(Of Double),
                                          ByVal pdblValorOperacion As Nullable(Of Double),
                                          ByVal pdblValorCreditoActual As Nullable(Of Double),
                                          ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_Calculos)
        Dim objTask As Task(Of List(Of Libranzas_Calculos)) = Me.Libranzas_CalcularAsync(pstrTipoCalculo,
                                                                                         pdtmFechaRegistro,
                                                                                         pdtmFechaInicioCredito,
                                                                                         pdtmFechaFinCredito,
                                                                                         pintNroCuotas,
                                                                                         pdblValorCuotas,
                                                                                         pstrPeriodoPago,
                                                                                         pdblValorCreditoOriginal,
                                                                                         pstrTipoPago,
                                                                                         pdblTasaInteresCredito,
                                                                                         pdblTasaDescuento,
                                                                                         pdblValorOperacion,
                                                                                         pdblValorCreditoActual,
                                                                                         pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Libranzas_CalcularAsync(ByVal pstrTipoCalculo As String,
                                             ByVal pdtmFechaRegistro As Nullable(Of DateTime),
                                             ByVal pdtmFechaInicioCredito As Nullable(Of DateTime),
                                             ByVal pdtmFechaFinCredito As Nullable(Of DateTime),
                                             ByVal pintNroCuotas As Nullable(Of Integer),
                                             ByVal pdblValorCuotas As Nullable(Of Double),
                                             ByVal pstrPeriodoPago As String,
                                             ByVal pdblValorCreditoOriginal As Nullable(Of Double),
                                             ByVal pstrTipoPago As String,
                                             ByVal pdblTasaInteresCredito As Nullable(Of Double),
                                             ByVal pdblTasaDescuento As Nullable(Of Double),
                                             ByVal pdblValorOperacion As Nullable(Of Double),
                                             ByVal pdblValorCreditoActual As Nullable(Of Double),
                                             ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Libranzas_Calculos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Libranzas_Calculos)) = New TaskCompletionSource(Of List(Of Libranzas_Calculos))()
        objTaskComplete.TrySetResult(Libranzas_Calcular(pstrTipoCalculo,
                                                        pdtmFechaRegistro,
                                                        pdtmFechaInicioCredito,
                                                        pdtmFechaFinCredito,
                                                        pintNroCuotas,
                                                        pdblValorCuotas,
                                                        pstrPeriodoPago,
                                                        pdblValorCreditoOriginal,
                                                        pstrTipoPago,
                                                        pdblTasaInteresCredito,
                                                        pdblTasaDescuento,
                                                        pdblValorOperacion,
                                                        pdblValorCreditoActual,
                                                        pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region
#Region "Libranzas_ImportacionImportadas"
    <Query(HasSideEffects:=True)>
    Public Function Libranzas_ImportacionValidarManual(ByVal pdtmFechaRegistro As System.Nullable(Of DateTime),
                                                 ByVal pstrNroCredito As String,
                                                 ByVal plngIDComitente As String,
                                                 ByVal pintIDCompania As System.Nullable(Of Integer),
                                                 ByVal pintIDEmisor As System.Nullable(Of Integer),
                                                 ByVal pdtmFechaInicioCredito As System.Nullable(Of DateTime),
                                                 ByVal pdtmFechaFinCredito As System.Nullable(Of DateTime),
                                                 ByVal pintNroCuotas As System.Nullable(Of Integer),
                                                 ByVal pstrPeriodoPago As String,
                                                 ByVal pdblValorCreditoOriginal As System.Nullable(Of Double),
                                                 ByVal pstrTipoPago As String,
                                                 ByVal pdblTasaInteresCredito As System.Nullable(Of Double),
                                                 ByVal pdblTasaDescuento As System.Nullable(Of Double),
                                                 ByVal pdblValorOperacion As System.Nullable(Of Double),
                                                 ByVal pdblValorCreditoActual As System.Nullable(Of Double),
                                                 ByVal pstrNroDocumentoBeneficiario As String,
                                                 ByVal pstrNombreBeneficiario As String,
                                                 ByVal pstrTipoIdentificacionBeneficiario As String,
                                                 ByVal pintIDPagador As System.Nullable(Of Integer),
                                                 ByVal pintIDCustodio As System.Nullable(Of Integer),
                                                 ByVal pstrTipoRegistro As String,
                                                 ByVal pstrMaquina As String,
                                                 ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspLibranzas_ImportacionValidarDatos(pdtmFechaRegistro,
                                                                          pstrNroCredito,
                                                                          plngIDComitente,
                                                                          pintIDCompania,
                                                                          pintIDEmisor,
                                                                          pdtmFechaInicioCredito,
                                                                          pdtmFechaFinCredito,
                                                                          pintNroCuotas,
                                                                          pstrPeriodoPago,
                                                                          pdblValorCreditoOriginal,
                                                                          pstrTipoPago,
                                                                          pdblTasaInteresCredito,
                                                                          pdblTasaDescuento,
                                                                          pdblValorOperacion,
                                                                          pdblValorCreditoActual,
                                                                          pstrNroDocumentoBeneficiario,
                                                                          pstrNombreBeneficiario,
                                                                          pstrTipoIdentificacionBeneficiario,
                                                                          pintIDPagador,
                                                                          pintIDCustodio,
                                                                          pstrTipoRegistro,
                                                                          pstrUsuario,
                                                                          pstrMaquina,
                                                                          DemeInfoSesion(pstrUsuario, "CF_CargaMasivaValidarManual"),
                                                                          0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CF_CargaMasivaValidarManual")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Libranzas_ImportacionValidaciones"
    Public Function Libranzas_ImportacionRespuestaImportacion(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Dim ret = Me.DataContext.uspCalculosFinancieros_PreciosEspecies_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarPreciosEspecies"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return Nothing
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarLibranzas_ImportacionValidaciones")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Libranzas_ImportacionCampos"
    'Public Sub InsertLibranzas_ImportacionCampos(ByVal objLibranzas_ImportacionCampos As Libranzas_ImportacionCampos)
    '    Try
    '        objLibranzas_ImportacionCampos.InfoSesion = DemeInfoSesion(pstrUsuario, "InsertLibranzas_ImportacionCampos")
    '        Me.DataContext.Libranzas_ImportacionCampos.InsertOnSubmit(objLibranzas_ImportacionCampos)
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "InsertLibranzas_ImportacionCampos")
    '    End Try
    'End Sub
#End Region
#Region "LibranzasCargaMasivaCantidadProcesadas"
    Public Function Libranzas_ImportacionCantidadProcesadas(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_CantidadProcesadas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspLibranzas_ImportacionConsultarCantidadProcesados(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "CF_CargaMasivaConsultarCantidadProcesadas"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CF_CargaMasivaConsultarCantidadProcesadas")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Libranzas_RespuestaValidacion"
    <Query(HasSideEffects:=True)>
    Public Function Libranzas_ImportacionConsultarResultados(ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspLibranzas_ImportacionConsultarResultados(
                                                                                  pstrUsuario,
                                                                                  pstrMaquina,
                                                                                  DemeInfoSesion(pstrUsuario, "uspLibranzas_ImportacionConsultarResultados"),
                                                                                  0
                                                                                ).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "uspLibranzas_ImportacionConsultarResultados")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function Libranzas_ImportacionConfirmar(ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspLibranzas_ImportacionConfirmar(
                                                                            pstrUsuario,
                                                                            pstrMaquina,
                                                                            DemeInfoSesion(pstrUsuario, "uspLibranzas_ImportacionConfirmar"),
                                                                            0
                                                                        ).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "uspLibranzas_ImportacionConfirmar")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Libranzas_CamposEditables"
    <Query(HasSideEffects:=True)>
    Public Function Libranzas_ValidarHabilitarCampos(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Libranzas_CamposEditables)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspLibranzas_ImportacionValidarHabilitarCampos(
                                                                                      pstrUsuario,
                                                                                      pstrMaquina,
                                                                                      DemeInfoSesion(pstrUsuario, "Libranzas_ValidarHabilitarCampos"),
                                                                                      0
                                                                                    ).ToList
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Libranzas_ValidarHabilitarCampos")
            Return Nothing
        End Try
    End Function
#End Region
#End Region

#Region "PagosLibranzas"

#Region "Métodos asincrónicos"
    'Public Function MarcarPagados(pstrFiltrosExcluir As String, pstrTipoGeneracion As String, pdtmFechaVencimiento As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PagosLibranzas)
    '    Try
    '        ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
    '        Dim ret = Me.DataContext.uspLibranzas_MarcarPagados(pstrFiltrosExcluir, pstrTipoGeneracion, pdtmFechaVencimiento, pstrUsuario, DemeInfoSesion(pstrUsuario, "MarcarPagados"), 0).ToList
    '        Return ret
    '        Exit Function
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "MarcarPagados")
    '        Return Nothing
    '    End Try
    'End Function

    Public Function ConsultarDatosGenericos(ByVal pstrCondicionFiltro As String, ByVal pstrTipoItem As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosGenericos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_DatosGenericos_Consultar(pstrCondicionFiltro, pstrTipoItem, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarDatosGenericos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarDatosGenericos")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function PagosLibranzas_Consultar(ByVal strFiltrosExcluir As String, ByVal strTipoGeneracion As String, ByVal strFechaVencimiento As String, ByVal pstrUsuario As String, ByVal strNombreArchivo As String, ByVal pstrInfoConexion As String) As List(Of PagosLibranzas)

        Dim objRetorno As New List(Of PagosLibranzas)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(strNombreArchivo, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[CF].[uspLibranzas_MarcarPagados]"
            Dim objListaParametros As New List(Of SqlParameter)
            Dim strArchivoExcel As String = String.Empty
            Dim intCantidadMarcados As Integer = 0
            Dim intCantidadExcluidos As Integer = 0

            'Seccción de los parametros que recibe el sp
            objListaParametros.Add(CrearSQLParameter("pstrFiltrosExcluir", strFiltrosExcluir, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrTipoGeneracion", strTipoGeneracion, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pdtmFechaVencimiento", strFechaVencimiento, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "GenerarArchivoPlano"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)

            'Si no hay datos, retornar el objeto vacío
            If objDatosConsulta.Tables.Count = 0 Then
                'objRetorno.Add(New PagosLibranzas With {.intID = 0, .intCantidadExcluidos = 0, .intCantidadMarcados = 0, .NombreArchivo = "", .RutaArchivo = ""})
                Return objRetorno
            End If
            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                intCantidadMarcados = CInt(objRow("intCantidadMarcados"))
                intCantidadExcluidos = CInt(objRow("intCantidadExcluidos"))
            Next

            strArchivoExcel = GenerarExcel(objDatosConsulta.Tables(1), objRutas.RutaArchivosLocal, strNombreArchivo)


            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            objRetorno.Add(New PagosLibranzas With {.strNombreArchivo = strArchivoExcel,
                                                    .strRutaArchivo = objRutas.RutaCompartidaOWeb() & strArchivoExcel,
                                                    .intCantidadMarcados = intCantidadMarcados,
                                                    .intCantidadExcluidos = intCantidadExcluidos,
                                                    .strMensaje = "Generación de archivo exitoso.",
                                                    .logExitoso = True,
                                                    .intID = 1
                                                    })

            Return objRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PagosLibranzas_Consultar")

            objRetorno.First.logExitoso = False
            objRetorno.First.strMensaje = ex.Message
            objRetorno.First.strRutaArchivo = String.Empty
            objRetorno.First.strNombreArchivo = String.Empty

            Return objRetorno
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"

    'Public Function MarcarPagadosSync(ByVal pstrFiltrosExcluir As String, ByVal pstrTipoGeneracion As String, ByVal pdtmFechaVencimiento As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PagosLibranzas)
    '    Dim objTask As Task(Of List(Of PagosLibranzas)) = Me.MarcarPagadosAsync(pstrFiltrosExcluir, pstrTipoGeneracion, pdtmFechaVencimiento, pstrUsuario, pstrInfoConexion)
    '    objTask.Wait()
    '    Return objTask.Result
    'End Function
    'Private Function MarcarPagadosAsync(ByVal pstrFiltrosExcluir As String, ByVal pstrTipoGeneracion As String, ByVal pdtmFechaVencimiento As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PagosLibranzas))
    '    Dim objTaskComplete As TaskCompletionSource(Of List(Of PagosLibranzas)) = New TaskCompletionSource(Of List(Of PagosLibranzas))()
    '    objTaskComplete.TrySetResult(MarcarPagados(pstrFiltrosExcluir, pstrTipoGeneracion, pdtmFechaVencimiento, pstrUsuario, pstrInfoConexion))
    '    Return (objTaskComplete.Task)
    'End Function

    Public Function ConsultarDatosGenericosSync(ByVal pstrCondicionFiltro As String, ByVal pstrTipoItem As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosGenericos)
        Dim objTask As Task(Of List(Of DatosGenericos)) = Me.ConsultarDatosGenericossAsync(pstrCondicionFiltro, pstrTipoItem, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarDatosGenericossAsync(ByVal pstrCondicionFiltro As String, ByVal pstrTipoItem As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DatosGenericos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DatosGenericos)) = New TaskCompletionSource(Of List(Of DatosGenericos))()
        objTaskComplete.TrySetResult(ConsultarDatosGenericos(pstrCondicionFiltro, pstrTipoItem, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function PagosLibranzas_ConsultarSync(ByVal strFiltrosExcluir As String, ByVal strTipoGeneracion As String, ByVal strFechaVencimiento As String, ByVal pstrUsuario As String, ByVal strCarpetaProceso As String, ByVal pstrInfoConexion As String) As List(Of PagosLibranzas)
        Dim objTask As Task(Of List(Of PagosLibranzas)) = Me.PagosLibranzas_ConsultarAsync(strFiltrosExcluir, strTipoGeneracion, strFechaVencimiento, pstrUsuario, strCarpetaProceso, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function PagosLibranzas_ConsultarAsync(ByVal strFiltrosExcluir As String, ByVal strTipoGeneracion As String, ByVal strFechaVencimiento As String, ByVal pstrUsuario As String, ByVal strCarpetaProceso As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PagosLibranzas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PagosLibranzas)) = New TaskCompletionSource(Of List(Of PagosLibranzas))()
        objTaskComplete.TrySetResult(PagosLibranzas_Consultar(strFiltrosExcluir, strTipoGeneracion, strFechaVencimiento, pstrUsuario, strCarpetaProceso, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Traslados fondos"

    Public Sub InsertTrasladosFondosInformacion(ByVal objTrasladosFondosInformacion As TrasladosFondosInformacion)

    End Sub

    Public Sub UpdateTrasladosFondosInformacion(ByVal objTrasladosFondosInformacion As TrasladosFondosInformacion)

    End Sub

    Public Sub DeleteTrasladosFondosInformacion(ByVal objTrasladosFondosInformacion As TrasladosFondosInformacion)

    End Sub

    Public Sub InsertTrasladosFondosConfiguracion(ByVal objTrasladosFondosConfiguracion As TrasladosFondosConfiguracion)

    End Sub

    Public Sub UpdateTrasladosFondosConfiguracion(ByVal objTrasladosFondosConfiguracion As TrasladosFondosConfiguracion)

    End Sub

    Public Sub DeleteTrasladosFondosConfiguracion(ByVal objTrasladosFondosConfiguracion As TrasladosFondosConfiguracion)

    End Sub

#Region "Métodos asincrónicos"
    Public Function TrasladoFondos_ConsultarConfiguracion(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TrasladosFondosConfiguracion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspTrasladosFondos_Configuracion(pintIDCompania, pstrTipoRegistro, pstrUsuario, DemeInfoSesion(pstrUsuario, "TrasladoFondos_ConsultarConfiguracion"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladoFondos_ConsultarConfiguracion")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function TrasladoFondos_ConsultarInformacion(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pdtmFechaRegistro As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TrasladosFondosInformacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspTrasladosFondos_Consultar(pintIDCompania, pstrTipoRegistro, pdtmFechaRegistro, pstrUsuario, DemeInfoSesion(pstrUsuario, "TrasladoFondos_ConsultarInformacion"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladoFondos_ConsultarInformacion")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function TrasladoFondos_Generar(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pdtmFechaRegistro As DateTime, ByVal pstrRegistrosGenerar As String, ByVal plogTotalizarRegistros As Boolean, ByVal pstrDatosGeneracion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TrasladosFondosRespuestaGeneracion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspTrasladosFondos_Generar(pintIDCompania, pstrTipoRegistro, pdtmFechaRegistro, pstrRegistrosGenerar, plogTotalizarRegistros, pstrDatosGeneracion, pstrUsuario, DemeInfoSesion(pstrUsuario, "TrasladoFondos_Generar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladoFondos_Generar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"

    Public Function TrasladoFondos_ConsultarConfiguracionSync(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TrasladosFondosConfiguracion)
        Dim objTask As Task(Of List(Of TrasladosFondosConfiguracion)) = Me.TrasladoFondos_ConsultarConfiguracionAsync(pintIDCompania, pstrTipoRegistro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function TrasladoFondos_ConsultarConfiguracionAsync(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TrasladosFondosConfiguracion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TrasladosFondosConfiguracion)) = New TaskCompletionSource(Of List(Of TrasladosFondosConfiguracion))()
        objTaskComplete.TrySetResult(TrasladoFondos_ConsultarConfiguracion(pintIDCompania, pstrTipoRegistro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function TrasladoFondos_ConsultarInformacionSync(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pdtmFechaRegistro As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TrasladosFondosInformacion)
        Dim objTask As Task(Of List(Of TrasladosFondosInformacion)) = Me.TrasladoFondos_ConsultarInformacionAsync(pintIDCompania, pstrTipoRegistro, pdtmFechaRegistro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function TrasladoFondos_ConsultarInformacionAsync(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pdtmFechaRegistro As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TrasladosFondosInformacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TrasladosFondosInformacion)) = New TaskCompletionSource(Of List(Of TrasladosFondosInformacion))()
        objTaskComplete.TrySetResult(TrasladoFondos_ConsultarInformacion(pintIDCompania, pstrTipoRegistro, pdtmFechaRegistro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function TrasladoFondos_GenerarSync(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pdtmFechaRegistro As DateTime, ByVal pstrRegistrosGenerar As String, ByVal plogTotalizarRegistros As Boolean, ByVal pstrDatosGeneracion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TrasladosFondosRespuestaGeneracion)
        Dim objTask As Task(Of List(Of TrasladosFondosRespuestaGeneracion)) = Me.TrasladoFondos_GenerarAsync(pintIDCompania, pstrTipoRegistro, pdtmFechaRegistro, pstrRegistrosGenerar, plogTotalizarRegistros, pstrDatosGeneracion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function TrasladoFondos_GenerarAsync(ByVal pintIDCompania As Integer, ByVal pstrTipoRegistro As String, ByVal pdtmFechaRegistro As DateTime, ByVal pstrRegistrosGenerar As String, ByVal plogTotalizarRegistros As Boolean, ByVal pstrDatosGeneracion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TrasladosFondosRespuestaGeneracion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TrasladosFondosRespuestaGeneracion)) = New TaskCompletionSource(Of List(Of TrasladosFondosRespuestaGeneracion))()
        objTaskComplete.TrySetResult(TrasladoFondos_Generar(pintIDCompania, pstrTipoRegistro, pdtmFechaRegistro, pstrRegistrosGenerar, plogTotalizarRegistros, pstrDatosGeneracion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region



#End Region

#Region "ConfiguracionArbitraje"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertConfiguracionArbitraje(ByVal newConfiguracionArbitraje As ConfiguracionArbitraje)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, newConfiguracionArbitraje.pstrUsuarioConexion, newConfiguracionArbitraje.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newConfiguracionArbitraje.strInfoSesion = DemeInfoSesion(newConfiguracionArbitraje.pstrUsuarioConexion, "InsertConfiguracionArbitraje")
            Me.DataContext.ConfiguracionArbitraje.InsertOnSubmit(newConfiguracionArbitraje)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConfiguracionArbitraje")
        End Try
    End Sub

    Public Sub UpdateConfiguracionArbitraje(ByVal currentConfiguracionArbitraje As ConfiguracionArbitraje)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConfiguracionArbitraje.pstrUsuarioConexion, currentConfiguracionArbitraje.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConfiguracionArbitraje.strInfoSesion = DemeInfoSesion(currentConfiguracionArbitraje.pstrUsuarioConexion, "UpdateConfiguracionArbitraje")
            Me.DataContext.ConfiguracionArbitraje.Attach(currentConfiguracionArbitraje, Me.ChangeSet.GetOriginal(currentConfiguracionArbitraje))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfiguracionArbitraje")
        End Try
    End Sub

    Public Sub DeleteConfiguracionArbitraje(ByVal deleteConfiguracionArbitraje As ConfiguracionArbitraje)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteConfiguracionArbitraje.pstrUsuarioConexion, deleteConfiguracionArbitraje.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteConfiguracionArbitraje.strInfoSesion = DemeInfoSesion(deleteConfiguracionArbitraje.pstrUsuarioConexion, "DeleteConfiguracionArbitraje")
            Me.DataContext.ConfiguracionArbitraje.Attach(deleteConfiguracionArbitraje)
            Me.DataContext.ConfiguracionArbitraje.DeleteOnSubmit(deleteConfiguracionArbitraje)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConfiguracionArbitraje")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarConfiguracionArbitraje(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionArbitraje)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionArbitraje_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarConfiguracionArbitraje"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarConfiguracionArbitraje")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConfiguracionArbitraje(ByVal pstrIDEspecie As String, ByVal pstrTipo As String, ByVal pdtmFechaVigencia As Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionArbitraje)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionArbitraje_Consultar(String.Empty, pstrIDEspecie, pstrTipo, pdtmFechaVigencia, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionArbitraje"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionArbitraje")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConfiguracionArbitrajePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionArbitraje
        Dim objConfiguracionArbitraje As ConfiguracionArbitraje = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionArbitraje_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, String.Empty, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionArbitrajePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objConfiguracionArbitraje = ret.FirstOrDefault
            End If
            Return objConfiguracionArbitraje
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionArbitrajePorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar el encabezado y detalle
    ''' </summary>
    Public Function ActualizarConfiguracionArbitraje(ByVal pintConfiguracionArbitraje As System.Nullable(Of Integer), ByVal pstrIDEspecie As String, ByVal pstrTipo As String, ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pdtmFechaVigencia As System.Nullable(Of System.DateTime), ByVal pdblUnidades As Double, ByVal pintIDEstadosConceptoTitulos As Integer, ByVal pintIDEstadosConceptoTitulosD As Integer, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionArbitraje_Actualizar(pintConfiguracionArbitraje, pstrIDEspecie, pstrTipo, pdtmFechaRegistro, pdtmFechaVigencia, pdblUnidades, pintIDEstadosConceptoTitulos, pintIDEstadosConceptoTitulosD, pxmlDetalleGrid, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarConfiguracionArbitraje"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarConfiguracionArbitraje")
            Return Nothing
        End Try
    End Function

    Public Function ConfiguracionArbitrajeMasiva_Consultar(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionArbitrajeDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionArbitrajeMasiva_Consultar(pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "ConfiguracionArbitrajeMasiva_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionArbitrajeMasiva_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function FiltrarConfiguracionArbitrajeSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionArbitraje)
        Dim objTask As Task(Of List(Of ConfiguracionArbitraje)) = Me.FiltrarConfiguracionArbitrajeAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarConfiguracionArbitrajeAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionArbitraje))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionArbitraje)) = New TaskCompletionSource(Of List(Of ConfiguracionArbitraje))()
        objTaskComplete.TrySetResult(FiltrarConfiguracionArbitraje(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConfiguracionArbitrajeSync(ByVal pstrIDEspecie As String, ByVal pstrTipo As String, ByVal pdtmFechaVigencia As Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionArbitraje)
        Dim objTask As Task(Of List(Of ConfiguracionArbitraje)) = Me.ConsultarConfiguracionArbitrajeAsync(pstrIDEspecie, pstrTipo, pdtmFechaVigencia, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConfiguracionArbitrajeAsync(ByVal pstrIDEspecie As String, ByVal pstrTipo As String, ByVal pdtmFechaVigencia As Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionArbitraje))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionArbitraje)) = New TaskCompletionSource(Of List(Of ConfiguracionArbitraje))()
        objTaskComplete.TrySetResult(ConsultarConfiguracionArbitraje(pstrIDEspecie, pstrTipo, pdtmFechaVigencia, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConfiguracionArbitrajePorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionArbitraje
        Dim objTask As Task(Of ConfiguracionArbitraje) = Me.ConsultarConfiguracionArbitrajePorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConfiguracionArbitrajePorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ConfiguracionArbitraje)
        Dim objTaskComplete As TaskCompletionSource(Of ConfiguracionArbitraje) = New TaskCompletionSource(Of ConfiguracionArbitraje)()
        objTaskComplete.TrySetResult(ConsultarConfiguracionArbitrajePorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ActualizarConfiguracionArbitrajeSync(ByVal pintConfiguracionArbitraje As System.Nullable(Of Integer), ByVal pstrIDEspecie As String, ByVal pstrTipo As String, ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pdtmFechaVigencia As System.Nullable(Of System.DateTime), ByVal pdblUnidades As Double, ByVal pintIDEstadosConceptoTitulos As Integer, ByVal pintIDEstadosConceptoTitulosD As Integer, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarConfiguracionArbitrajeAsync(pintConfiguracionArbitraje, pstrIDEspecie, pstrTipo, pdtmFechaRegistro, pdtmFechaVigencia, pdblUnidades, pintIDEstadosConceptoTitulos, pintIDEstadosConceptoTitulosD, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ActualizarConfiguracionArbitrajeAsync(ByVal pintConfiguracionArbitraje As System.Nullable(Of Integer), ByVal pstrIDEspecie As String, ByVal pstrTipo As String, ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pdtmFechaVigencia As System.Nullable(Of System.DateTime), ByVal pdblUnidades As Double, ByVal pintIDEstadosConceptoTitulos As Integer, ByVal pintIDEstadosConceptoTitulosD As Integer, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarConfiguracionArbitraje(pintConfiguracionArbitraje, pstrIDEspecie, pstrTipo, pdtmFechaRegistro, pdtmFechaVigencia, pdblUnidades, pintIDEstadosConceptoTitulos, pintIDEstadosConceptoTitulosD, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ConfiguracionArbitrajeDetalle"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertConfiguracionArbitrajeDetalle(ByVal objConfiguracionArbitrajeDetalle As ConfiguracionArbitrajeDetalle)

    End Sub

    Public Sub UpdateConfiguracionArbitrajeDetalle(ByVal currentConfiguracionArbitrajeDetalle As ConfiguracionArbitrajeDetalle)

    End Sub

    Public Sub DeleteConfiguracionArbitrajeDetalle(ByVal UpdateConfiguracionArbitrajeDetalle As ConfiguracionArbitrajeDetalle)

    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarConfiguracionArbitrajeDetalle(ByVal pintIDConfiguracionArbitraje As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionArbitrajeDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionArbitrajeDetalle_Consultar(String.Empty, pintIDConfiguracionArbitraje, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionArbitrajeDetalle"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionArbitrajeDetalle")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConfiguracionArbitrajeDetallePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionArbitrajeDetalle
        Dim objConfiguracionArbitrajeDetalle As ConfiguracionArbitrajeDetalle = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionArbitrajeDetalle_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionArbitrajeDetallePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objConfiguracionArbitrajeDetalle = ret.FirstOrDefault
            End If
            Return objConfiguracionArbitrajeDetalle
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionArbitrajeDetallePorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarConfiguracionArbitrajeDetalleSync(ByVal pintIDConfiguracionArbitraje As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionArbitrajeDetalle)
        Dim objTask As Task(Of List(Of ConfiguracionArbitrajeDetalle)) = Me.ConsultarConfiguracionArbitrajeDetalleAsync(pintIDConfiguracionArbitraje, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConfiguracionArbitrajeDetalleAsync(ByVal pintIDConfiguracionArbitraje As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionArbitrajeDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionArbitrajeDetalle)) = New TaskCompletionSource(Of List(Of ConfiguracionArbitrajeDetalle))()
        objTaskComplete.TrySetResult(ConsultarConfiguracionArbitrajeDetalle(pintIDConfiguracionArbitraje, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConfiguracionArbitrajeDetallePorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionArbitrajeDetalle
        Dim objTask As Task(Of ConfiguracionArbitrajeDetalle) = Me.ConsultarConfiguracionArbitrajeDetallePorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConfiguracionArbitrajeDetallePorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ConfiguracionArbitrajeDetalle)
        Dim objTaskComplete As TaskCompletionSource(Of ConfiguracionArbitrajeDetalle) = New TaskCompletionSource(Of ConfiguracionArbitrajeDetalle)()
        objTaskComplete.TrySetResult(ConsultarConfiguracionArbitrajeDetallePorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Identificación de Aportes"

    Public Sub InsertIdentificacionAportes_Consultadas(ByVal currentRegistro As IdentificacionAportes_Consultadas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertIdentificacionAportes_Consultadas")
        End Try
    End Sub

    Public Sub UpdateIdentificacionAportes_Consultadas(ByVal currentRegistro As IdentificacionAportes_Consultadas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateIdentificacionAportes_Consultadas")
        End Try
    End Sub

    Public Sub DeleteIdentificacionAportes_Consultadas(ByVal currentRegistro As IdentificacionAportes_Consultadas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteIdentificacionAportes_Consultadas")
        End Try
    End Sub

    Public Sub InsertIdentificacionAportes_Procesadas(ByVal currentRegistro As IdentificacionAportes_Procesadas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertIdentificacionAportes_Procesadas")
        End Try
    End Sub

    Public Sub UpdateIdentificacionAportes_Procesadas(ByVal currentRegistro As IdentificacionAportes_Procesadas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateIdentificacionAportes_Procesadas")
        End Try
    End Sub

    Public Sub DeleteIdentificacionAportes_Procesadas(ByVal currentRegistro As IdentificacionAportes_Procesadas)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteIdentificacionAportes_Procesadas")
        End Try
    End Sub

    Public Sub InsertIdentificacionAportes_Consultadas_Pendientes(ByVal currentRegistro As IdentificacionAportes_Consultadas_Pendientes)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertIdentificacionAportes_Consultadas_Pendientes")
        End Try
    End Sub

    Public Sub UpdateIdentificacionAportes_Consultadas_Pendientes(ByVal currentRegistro As IdentificacionAportes_Consultadas_Pendientes)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateIdentificacionAportes_Consultadas_Pendientes")
        End Try
    End Sub

    Public Sub DeleteIdentificacionAportes_Consultadas_Pendientes(ByVal currentRegistro As IdentificacionAportes_Consultadas_Pendientes)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteIdentificacionAportes_Consultadas_Pendientes")
        End Try
    End Sub

    Public Function IdentificacionAportes_Consultar(ByVal pintIDCompania As Integer, ByVal pdtmFecha As Nullable(Of Date), ByVal pdblValor As Double, ByVal pintIDBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of IdentificacionAportes_Consultadas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspIdentificacionAportes_Consultar(pintIDCompania, pdtmFecha, pdblValor, pintIDBanco, pstrUsuario, DemeInfoSesion(pstrUsuario, "IdentificacionAportes_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "IdentificacionAportes_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function IdentificacionAportes_Generar(ByVal pintIDCompania As Integer, ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of IdentificacionAportes_Procesadas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspIdentificacionAportes_Generar(pintIDCompania, pstrRegistros, pstrUsuario, DemeInfoSesion(pstrUsuario, "IdentificacionAportes_Generar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "IdentificacionAportes_Generar")
            Return Nothing
        End Try
    End Function

    Public Function IdentificacionAportes_ConsultarPendientes(ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of IdentificacionAportes_Consultadas_Pendientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspIdentificacionAportes_ConsultarPendientes(pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "IdentificacionAportes_ConsultarPendientes"), 0, True).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "IdentificacionAportes_ConsultarPendientes")
            Return Nothing
        End Try
    End Function

    Public Function IdentificacionAportes_CancelarPendientes(ByVal pintIDCompania As Integer, ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of IdentificacionAportes_Procesadas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspIdentificacionAportes_CancelarPendientes(pintIDCompania, pstrRegistros, pstrUsuario, DemeInfoSesion(pstrUsuario, "IdentificacionAportes_CancelarPendientes"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "IdentificacionAportes_CancelarPendientes")
            Return Nothing
        End Try
    End Function
#End Region

#Region "CompaniasCondicionesTesoreria"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCompaniasCondicionesTesoreria(ByVal objCompaniasTesoreria As CompaniasCondicionesTesoreria)

    End Sub

    Public Sub UpdateCompaniasCondicionesTesoreria(ByVal currentCompaniasTesoreria As CompaniasCondicionesTesoreria)

    End Sub

    Public Sub DeleteCompaniasCondicionesTesoreria(ByVal UpdateCompaniasTesoreria As CompaniasCondicionesTesoreria)

    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function ConsultarCompaniasCondicionesTesoreria(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasCondicionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasCondicionesTesoreria_Consultar(String.Empty, pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasCondicionesTesoreria"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasCondicionesTesoreria")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompaniasCondicionesTesoreriaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasCondicionesTesoreria
        Dim objCompaniasCondicionesTesoreria As CompaniasCondicionesTesoreria = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasCondicionesTesoreria_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasCondicionesTesoreriaPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasCondicionesTesoreria = ret.FirstOrDefault
            End If
            Return objCompaniasCondicionesTesoreria
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasCondicionesTesoreriaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosTesoreriaPorDefectoCias(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasCondicionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasCondicionesTesoreria_PorDefecto(pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosTesoreriaPorDefectoCias"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosTesoreriaPorDefectoCias")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosTesoreriaPorDefectoCiasPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasCondicionesTesoreria
        Dim objCompaniasCondicionesTesoreria As CompaniasCondicionesTesoreria = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasCondicionesTesoreria_PorDefecto(0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosTesoreriaPorDefectoCiasPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasCondicionesTesoreria = ret.FirstOrDefault
            End If
            Return objCompaniasCondicionesTesoreria
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosTesoreriaPorDefectoCiasPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosTesoreriaPorDefectoCiasPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasCondicionesTesoreria
        Dim objTask As Task(Of CompaniasCondicionesTesoreria) = Me.ConceptosTesoreriaPorDefectoCiasPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConceptosTesoreriaPorDefectoCiasPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CompaniasCondicionesTesoreria)
        Dim objTaskComplete As TaskCompletionSource(Of CompaniasCondicionesTesoreria) = New TaskCompletionSource(Of CompaniasCondicionesTesoreria)()
        objTaskComplete.TrySetResult(ConceptosTesoreriaPorDefectoCiasPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function ConsultarCompaniasCondicionesTesoreriaSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasCondicionesTesoreria)
        Dim objTask As Task(Of List(Of CompaniasCondicionesTesoreria)) = Me.ConsultarCompaniasCondicionesTesoreriaAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCompaniasCondicionesTesoreriaAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniasCondicionesTesoreria))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniasCondicionesTesoreria)) = New TaskCompletionSource(Of List(Of CompaniasCondicionesTesoreria))()
        objTaskComplete.TrySetResult(ConsultarCompaniasCondicionesTesoreria(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasCondicionesTesoreriaPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasCondicionesTesoreria
        Dim objTask As Task(Of CompaniasCondicionesTesoreria) = Me.ConsultarCompaniasCondicionesTesoreriaPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCompaniasCondicionesTesoreriaPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CompaniasCondicionesTesoreria)
        Dim objTaskComplete As TaskCompletionSource(Of CompaniasCondicionesTesoreria) = New TaskCompletionSource(Of CompaniasCondicionesTesoreria)()
        objTaskComplete.TrySetResult(ConsultarCompaniasCondicionesTesoreriaPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConceptosTesoreriaPorDefectoCiasSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasCondicionesTesoreria)
        Dim objTask As Task(Of List(Of CompaniasCondicionesTesoreria)) = Me.ConceptosTesoreriaPorDefectoCiasAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConceptosTesoreriaPorDefectoCiasAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniasCondicionesTesoreria))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniasCondicionesTesoreria)) = New TaskCompletionSource(Of List(Of CompaniasCondicionesTesoreria))()
        objTaskComplete.TrySetResult(ConceptosTesoreriaPorDefectoCias(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


#End Region

#End Region

#Region "Arbitrajes"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertArbitrajes(ByVal newArbitrajes As Arbitrajes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, newArbitrajes.pstrUsuarioConexion, newArbitrajes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newArbitrajes.strInfoSesion = DemeInfoSesion(newArbitrajes.pstrUsuarioConexion, "InsertArbitrajes")
            Me.DataContext.Arbitrajes.InsertOnSubmit(newArbitrajes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertArbitrajes")
        End Try
    End Sub

    Public Sub UpdateArbitrajes(ByVal currentArbitrajes As Arbitrajes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentArbitrajes.pstrUsuarioConexion, currentArbitrajes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentArbitrajes.strInfoSesion = DemeInfoSesion(currentArbitrajes.pstrUsuarioConexion, "UpdateArbitrajes")
            Me.DataContext.Arbitrajes.Attach(currentArbitrajes, Me.ChangeSet.GetOriginal(currentArbitrajes))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateArbitrajes")
        End Try
    End Sub

    Public Sub DeleteArbitrajes(ByVal deleteArbitrajes As Arbitrajes)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteArbitrajes.pstrUsuarioConexion, deleteArbitrajes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteArbitrajes.strInfoSesion = DemeInfoSesion(deleteArbitrajes.pstrUsuarioConexion, "DeleteArbitrajes")
            Me.DataContext.Arbitrajes.Attach(deleteArbitrajes)
            Me.DataContext.Arbitrajes.DeleteOnSubmit(deleteArbitrajes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteArbitrajes")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function Arbitrajes_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Arbitrajes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Arbitrajes_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "Arbitrajes_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Arbitrajes_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function Arbitrajes_Consultar(ByVal pstrIDEspecie As String, ByVal plogConstruir As System.Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Arbitrajes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Arbitrajes_Consultar(String.Empty, pstrIDEspecie, plogConstruir, pdtmFechaProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "Arbitrajes_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Arbitrajes_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function Arbitrajes_ConsultarPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Arbitrajes
        Dim objArbitrajes As Arbitrajes = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Arbitrajes_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, False, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "Arbitrajes_ConsultarPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objArbitrajes = ret.FirstOrDefault
            End If
            Return objArbitrajes
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Arbitrajes_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar el encabezado y detalle
    ''' </summary>
    Public Function Arbitrajes_Actualizar(ByVal pintIDArbitrajes As Integer, ByVal pstrIDEspecie As String, ByVal plogConstruir As Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pintIDEstadosConceptoTitulos As System.Nullable(Of Integer), ByVal plogMGC As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_Arbitrajes_Actualizar(pintIDArbitrajes, pstrIDEspecie, plogConstruir, pdtmFechaProceso, pxmlDetalleGrid, pintIDEstadosConceptoTitulos, plogMGC, pstrUsuario, DemeInfoSesion(pstrUsuario, "Arbitrajes_Actualizar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Arbitrajes_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function Arbitrajes_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Arbitrajes)
        Dim objTask As Task(Of List(Of Arbitrajes)) = Me.Arbitrajes_FiltrarAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Arbitrajes_FiltrarAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Arbitrajes))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Arbitrajes)) = New TaskCompletionSource(Of List(Of Arbitrajes))()
        objTaskComplete.TrySetResult(Arbitrajes_Filtrar(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Arbitrajes_ConsultarSync(ByVal pstrIDEspecie As String, ByVal plogConstruir As Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Arbitrajes)
        Dim objTask As Task(Of List(Of Arbitrajes)) = Me.Arbitrajes_ConsultarAsync(pstrIDEspecie, plogConstruir, pdtmFechaProceso, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Arbitrajes_ConsultarAsync(ByVal pstrIDEspecie As String, ByVal plogConstruir As Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Arbitrajes))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Arbitrajes)) = New TaskCompletionSource(Of List(Of Arbitrajes))()
        objTaskComplete.TrySetResult(Arbitrajes_Consultar(pstrIDEspecie, plogConstruir, pdtmFechaProceso, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Arbitrajes_ConsultarPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Arbitrajes
        Dim objTask As Task(Of Arbitrajes) = Me.Arbitrajes_ConsultarPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Arbitrajes_ConsultarPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Arbitrajes)
        Dim objTaskComplete As TaskCompletionSource(Of Arbitrajes) = New TaskCompletionSource(Of Arbitrajes)()
        objTaskComplete.TrySetResult(Arbitrajes_ConsultarPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Arbitrajes_ActualizarSync(ByVal pintIDArbitrajes As Integer, ByVal pstrIDEspecie As String, ByVal plogConstruir As Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pintIDEstadosConceptoTitulos As System.Nullable(Of Integer), ByVal plogMGC As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.Arbitrajes_ActualizarAsync(pintIDArbitrajes, pstrIDEspecie, plogConstruir, pdtmFechaProceso, pxmlDetalleGrid, pintIDEstadosConceptoTitulos, plogMGC, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Arbitrajes_ActualizarAsync(ByVal pintIDArbitrajes As Integer, ByVal pstrIDEspecie As String, ByVal plogConstruir As Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pintIDEstadosConceptoTitulos As System.Nullable(Of Integer), ByVal plogMGC As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(Arbitrajes_Actualizar(pintIDArbitrajes, pstrIDEspecie, plogConstruir, pdtmFechaProceso, pxmlDetalleGrid, pintIDEstadosConceptoTitulos, plogMGC, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ArbitrajesDetalle"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertArbitrajesDetalle(ByVal newArbitrajesDetalle As ArbitrajesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, newArbitrajesDetalle.pstrUsuarioConexion, newArbitrajesDetalle.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newArbitrajesDetalle.strInfoSesion = DemeInfoSesion(newArbitrajesDetalle.pstrUsuarioConexion, "InsertArbitrajesDetalle")
            Me.DataContext.ArbitrajesDetalle.InsertOnSubmit(newArbitrajesDetalle)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertArbitrajesDetalle")
        End Try
    End Sub

    Public Sub UpdateArbitrajesDetalle(ByVal currentArbitrajesDetalle As ArbitrajesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentArbitrajesDetalle.pstrUsuarioConexion, currentArbitrajesDetalle.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentArbitrajesDetalle.strInfoSesion = DemeInfoSesion(currentArbitrajesDetalle.pstrUsuarioConexion, "UpdateArbitrajesDetalle")
            Me.DataContext.ArbitrajesDetalle.Attach(currentArbitrajesDetalle, Me.ChangeSet.GetOriginal(currentArbitrajesDetalle))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateArbitrajesDetalle")
        End Try
    End Sub

    Public Sub DeleteArbitrajesDetalle(ByVal deleteArbitrajesDetalle As ArbitrajesDetalle)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteArbitrajesDetalle.pstrUsuarioConexion, deleteArbitrajesDetalle.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteArbitrajesDetalle.strInfoSesion = DemeInfoSesion(deleteArbitrajesDetalle.pstrUsuarioConexion, "DeleteArbitrajesDetalle")
            Me.DataContext.ArbitrajesDetalle.Attach(deleteArbitrajesDetalle)
            Me.DataContext.ArbitrajesDetalle.DeleteOnSubmit(deleteArbitrajesDetalle)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteArbitrajesDetalle")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ArbitrajesDetalle_Consultar(ByVal pintIDArbitrajes As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ArbitrajesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ArbitrajesDetalle_Consultar(String.Empty, pintIDArbitrajes, pstrUsuario, DemeInfoSesion(pstrUsuario, "ArbitrajesDetalle_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ArbitrajesDetalle_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function ArbitrajesDetalle_ConsultarPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ArbitrajesDetalle
        Dim objArbitrajesDetalle As ArbitrajesDetalle = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ArbitrajesDetalle_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ArbitrajesDetalle_ConsultarPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objArbitrajesDetalle = ret.FirstOrDefault
            End If
            Return objArbitrajesDetalle
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ArbitrajesDetalle_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Function Arbitrajes_Operaciones_Consultar(ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrIDEspecie As String, ByVal plogConstruir As System.Nullable(Of System.Boolean), ByVal plogFiltradoDesdeBuscador As System.Nullable(Of System.Boolean), ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ArbitrajesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Arbitrajes_Operaciones_Consultar(pdtmFechaProceso, pstrIDEspecie, plogConstruir, plogFiltradoDesdeBuscador, plngIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "Arbitrajes_Operaciones_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Arbitrajes_Operaciones_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ArbitrajesDetalle_ConsultarSync(ByVal pintIDArbitrajes As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ArbitrajesDetalle)
        Dim objTask As Task(Of List(Of ArbitrajesDetalle)) = Me.ArbitrajesDetalle_ConsultarAsync(pintIDArbitrajes, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ArbitrajesDetalle_ConsultarAsync(ByVal pintIDArbitrajes As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ArbitrajesDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ArbitrajesDetalle)) = New TaskCompletionSource(Of List(Of ArbitrajesDetalle))()
        objTaskComplete.TrySetResult(ArbitrajesDetalle_Consultar(pintIDArbitrajes, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ArbitrajesDetalle_ConsultarPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ArbitrajesDetalle
        Dim objTask As Task(Of ArbitrajesDetalle) = Me.ArbitrajesDetalle_ConsultarPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ArbitrajesDetalle_ConsultarPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ArbitrajesDetalle)
        Dim objTaskComplete As TaskCompletionSource(Of ArbitrajesDetalle) = New TaskCompletionSource(Of ArbitrajesDetalle)()
        objTaskComplete.TrySetResult(ArbitrajesDetalle_ConsultarPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Arbitrajes_Operaciones_ConsultarSync(ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrIDEspecie As String, ByVal plogConstruir As System.Nullable(Of System.Boolean), ByVal plogFiltradoDesdeBuscador As System.Nullable(Of System.Boolean), ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ArbitrajesDetalle)
        Dim objTask As Task(Of List(Of ArbitrajesDetalle)) = Me.Arbitrajes_Operaciones_ConsultarAsync(pdtmFechaProceso, pstrIDEspecie, plogConstruir, plogFiltradoDesdeBuscador, plngIDComitente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Arbitrajes_Operaciones_ConsultarAsync(ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrIDEspecie As String, ByVal plogConstruir As System.Nullable(Of System.Boolean), ByVal plogFiltradoDesdeBuscador As System.Nullable(Of System.Boolean), ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ArbitrajesDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ArbitrajesDetalle)) = New TaskCompletionSource(Of List(Of ArbitrajesDetalle))()
        objTaskComplete.TrySetResult(Arbitrajes_Operaciones_Consultar(pdtmFechaProceso, pstrIDEspecie, plogConstruir, plogFiltradoDesdeBuscador, plngIDComitente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ControlLiquidezOperaciones"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertControlLiquidezOperaciones(ByVal newControlLiquidezOperaciones As ControlLiquidezOperaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, newControlLiquidezOperaciones.pstrUsuarioConexion, newControlLiquidezOperaciones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newControlLiquidezOperaciones.strInfoSesion = DemeInfoSesion(newControlLiquidezOperaciones.pstrUsuarioConexion, "InsertControlLiquidezOperaciones")
            Me.DataContext.ControlLiquidezOperaciones.InsertOnSubmit(newControlLiquidezOperaciones)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertControlLiquidezOperaciones")
        End Try
    End Sub

    Public Sub UpdateControlLiquidezOperaciones(ByVal currentControlLiquidezOperaciones As ControlLiquidezOperaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentControlLiquidezOperaciones.pstrUsuarioConexion, currentControlLiquidezOperaciones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentControlLiquidezOperaciones.strInfoSesion = DemeInfoSesion(currentControlLiquidezOperaciones.pstrUsuarioConexion, "UpdateControlLiquidezOperaciones")
            Me.DataContext.ControlLiquidezOperaciones.Attach(currentControlLiquidezOperaciones, Me.ChangeSet.GetOriginal(currentControlLiquidezOperaciones))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateControlLiquidezOperaciones")
        End Try
    End Sub

    Public Sub DeleteControlLiquidezOperaciones(ByVal deleteControlLiquidezOperaciones As ControlLiquidezOperaciones)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteControlLiquidezOperaciones.pstrUsuarioConexion, deleteControlLiquidezOperaciones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteControlLiquidezOperaciones.strInfoSesion = DemeInfoSesion(deleteControlLiquidezOperaciones.pstrUsuarioConexion, "DeleteControlLiquidezOperaciones")
            Me.DataContext.ControlLiquidezOperaciones.Attach(deleteControlLiquidezOperaciones)
            Me.DataContext.ControlLiquidezOperaciones.DeleteOnSubmit(deleteControlLiquidezOperaciones)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteControlLiquidezOperaciones")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ControlLiquidezOperaciones_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlLiquidezOperaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ControlLiquidezOperaciones_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlLiquidezOperaciones_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlLiquidezOperaciones_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function ControlLiquidezOperaciones_Consultar(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlLiquidezOperaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ControlLiquidezOperaciones_Consultar(String.Empty, pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, plngIDMoneda, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlLiquidezOperaciones_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlLiquidezOperaciones_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function ControlLiquidezOperaciones_ConsultarPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ControlLiquidezOperaciones
        Dim objControlLiquidezOperaciones As ControlLiquidezOperaciones = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ControlLiquidezOperaciones_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, String.Empty, Nothing, String.Empty, String.Empty, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlLiquidezOperaciones_ConsultarPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objControlLiquidezOperaciones = ret.FirstOrDefault
            End If
            Return objControlLiquidezOperaciones
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlLiquidezOperaciones_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar el encabezado y detalle
    ''' </summary>
    Public Function ControlLiquidezOperaciones_Actualizar(ByVal pintIDControlLiquidezOperaciones As System.Nullable(Of Integer), ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal pstrSubmodulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pdblSaldo As System.Nullable(Of System.Double), ByVal pdblCompras As System.Nullable(Of System.Double), ByVal pdblVentas As System.Nullable(Of System.Double), ByVal pdblTotal As System.Nullable(Of Double), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ControlLiquidezOperaciones_Actualizar(pintIDControlLiquidezOperaciones, pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, pstrSubmodulo, plngIDMoneda, pdblSaldo, pdblCompras, pdblVentas, pdblTotal, pxmlDetalleGrid, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlLiquidezOperaciones_Actualizar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlLiquidezOperaciones_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ControlLiquidezOperaciones_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlLiquidezOperaciones)
        Dim objTask As Task(Of List(Of ControlLiquidezOperaciones)) = Me.ControlLiquidezOperaciones_FiltrarAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlLiquidezOperaciones_FiltrarAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ControlLiquidezOperaciones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ControlLiquidezOperaciones)) = New TaskCompletionSource(Of List(Of ControlLiquidezOperaciones))()
        objTaskComplete.TrySetResult(ControlLiquidezOperaciones_Filtrar(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlLiquidezOperaciones_ConsultarSync(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlLiquidezOperaciones)
        Dim objTask As Task(Of List(Of ControlLiquidezOperaciones)) = Me.ControlLiquidezOperaciones_ConsultarAsync(pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, plngIDMoneda, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlLiquidezOperaciones_ConsultarAsync(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ControlLiquidezOperaciones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ControlLiquidezOperaciones)) = New TaskCompletionSource(Of List(Of ControlLiquidezOperaciones))()
        objTaskComplete.TrySetResult(ControlLiquidezOperaciones_Consultar(pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, plngIDMoneda, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlLiquidezOperaciones_ConsultarPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ControlLiquidezOperaciones
        Dim objTask As Task(Of ControlLiquidezOperaciones) = Me.ControlLiquidezOperaciones_ConsultarPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlLiquidezOperaciones_ConsultarPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ControlLiquidezOperaciones)
        Dim objTaskComplete As TaskCompletionSource(Of ControlLiquidezOperaciones) = New TaskCompletionSource(Of ControlLiquidezOperaciones)()
        objTaskComplete.TrySetResult(ControlLiquidezOperaciones_ConsultarPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlLiquidezOperaciones_ActualizarSync(ByVal pintIDControlLiquidezOperaciones As System.Nullable(Of Integer), ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal pstrSubmodulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pdblSaldo As System.Nullable(Of System.Double), ByVal pdblCompras As System.Nullable(Of System.Double), ByVal pdblVentas As System.Nullable(Of System.Double), ByVal pdblTotal As System.Nullable(Of Double), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ControlLiquidezOperaciones_ActualizarAsync(pintIDControlLiquidezOperaciones, pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, pstrSubmodulo, plngIDMoneda, pdblSaldo, pdblCompras, pdblVentas, pdblTotal, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlLiquidezOperaciones_ActualizarAsync(ByVal pintIDControlLiquidezOperaciones As System.Nullable(Of Integer), ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal pstrSubmodulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pdblSaldo As System.Nullable(Of System.Double), ByVal pdblCompras As System.Nullable(Of System.Double), ByVal pdblVentas As System.Nullable(Of System.Double), ByVal pdblTotal As System.Nullable(Of Double), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ControlLiquidezOperaciones_Actualizar(pintIDControlLiquidezOperaciones, pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, pstrSubmodulo, plngIDMoneda, pdblSaldo, pdblCompras, pdblVentas, pdblTotal, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ControlLiquidezOperacionesDetalle"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertControlLiquidezOperacionesDetalle(ByVal objControlLiquidezOperacionesDetalle As ControlLiquidezOperacionesDetalle)

    End Sub

    Public Sub UpdateControlLiquidezOperacionesDetalle(ByVal currentControlLiquidezOperacionesDetalle As ControlLiquidezOperacionesDetalle)

    End Sub

    Public Sub DeleteControlLiquidezOperacionesDetalle(ByVal UpdateControlLiquidezOperacionesDetalle As ControlLiquidezOperacionesDetalle)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function ControlLiquidezOperacionesDetalle_Consultar(ByVal pintIDControlLiquidezOperaciones As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlLiquidezOperacionesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ControlLiquidezOperacionesDetalle_Consultar(String.Empty, pintIDControlLiquidezOperaciones, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarControlLiquidezOperacionesDetalle"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarControlLiquidezOperacionesDetalle")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarControlLiquidezOperacionesDetallePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ControlLiquidezOperacionesDetalle
        Dim objControlLiquidezOperacionesDetalle As ControlLiquidezOperacionesDetalle = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ControlLiquidezOperacionesDetalle_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarControlLiquidezOperacionesDetallePorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objControlLiquidezOperacionesDetalle = ret.FirstOrDefault
            End If
            Return objControlLiquidezOperacionesDetalle
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarControlLiquidezOperacionesDetallePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Function ControlLiquidezOperaciones_Buscar(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal pstrSubmodulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlLiquidezOperacionesDatos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ControlLiquidezOperaciones_Buscar(pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, pstrSubmodulo, plngIDMoneda, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlLiquidezOperaciones_Buscar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlLiquidezOperaciones_Buscar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ControlLiquidezOperacionesDetalle_ConsultarSync(ByVal pintIDControlLiquidezOperaciones As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlLiquidezOperacionesDetalle)
        Dim objTask As Task(Of List(Of ControlLiquidezOperacionesDetalle)) = Me.ControlLiquidezOperacionesDetalle_ConsultarAsync(pintIDControlLiquidezOperaciones, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlLiquidezOperacionesDetalle_ConsultarAsync(ByVal pintIDControlLiquidezOperaciones As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ControlLiquidezOperacionesDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ControlLiquidezOperacionesDetalle)) = New TaskCompletionSource(Of List(Of ControlLiquidezOperacionesDetalle))()
        objTaskComplete.TrySetResult(ControlLiquidezOperacionesDetalle_Consultar(pintIDControlLiquidezOperaciones, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlLiquidezOperacionesDetalle_ConsultarPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ControlLiquidezOperacionesDetalle
        Dim objTask As Task(Of ControlLiquidezOperacionesDetalle) = Me.ControlLiquidezOperacionesDetalle_ConsultarPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlLiquidezOperacionesDetalle_ConsultarPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ControlLiquidezOperacionesDetalle)
        Dim objTaskComplete As TaskCompletionSource(Of ControlLiquidezOperacionesDetalle) = New TaskCompletionSource(Of ControlLiquidezOperacionesDetalle)()
        objTaskComplete.TrySetResult(ConsultarControlLiquidezOperacionesDetallePorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlLiquidezOperaciones_BuscarSync(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal pstrSubmodulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlLiquidezOperacionesDatos)
        Dim objTask As Task(Of List(Of ControlLiquidezOperacionesDatos)) = Me.ControlLiquidezOperaciones_BuscarAsync(pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, pstrSubmodulo, plngIDMoneda, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlLiquidezOperaciones_BuscarAsync(ByVal pintIDCompania As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrTipoFechaProceso As String, ByVal pstrModulo As String, ByVal pstrSubmodulo As String, ByVal plngIDMoneda As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ControlLiquidezOperacionesDatos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ControlLiquidezOperacionesDatos)) = New TaskCompletionSource(Of List(Of ControlLiquidezOperacionesDatos))()
        objTaskComplete.TrySetResult(ControlLiquidezOperaciones_Buscar(pintIDCompania, plngIDComitente, pdtmFechaProceso, pstrTipoFechaProceso, pstrModulo, pstrSubmodulo, plngIDMoneda, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ModulosSubmodulos"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertModulosSubmodulos(ByVal objModulosSubmodulos As ModulosSubmodulos)

    End Sub

    Public Sub UpdateModulosSubmodulos(ByVal currentModulosSubmodulos As ModulosSubmodulos)

    End Sub

    Public Sub DeleteModulosSubmodulos(ByVal UpdateModulosSubmodulos As ModulosSubmodulos)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function ControlLiquidezOperaciones_ModulosSubmodulos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ModulosSubmodulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ControlLiquidezOperaciones_ModulosSubmodulos(pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlLiquidezOperaciones_ModulosSubmodulos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlLiquidezOperaciones_ModulosSubmodulos")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ControlLiquidezOperaciones_ModulosSubmodulosSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ModulosSubmodulos)
        Dim objTask As Task(Of List(Of ModulosSubmodulos)) = Me.ControlLiquidezOperaciones_ModulosSubmodulosAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlLiquidezOperaciones_ModulosSubmodulosAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ModulosSubmodulos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ModulosSubmodulos)) = New TaskCompletionSource(Of List(Of ModulosSubmodulos))()
        objTaskComplete.TrySetResult(ControlLiquidezOperaciones_ModulosSubmodulos(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Parametros Compañia"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertParametrosCompania(ByVal objParametrosCompania As ParametrosCompania)

    End Sub

    Public Sub UpdateParametrosCompania(ByVal currentParametrosCompania As ParametrosCompania)

    End Sub

    Public Sub DeleteParametrosCompania(ByVal UpdateParametrosCompania As ParametrosCompania)

    End Sub

#End Region

#Region "Métodos asincrónicos"
    Public Function ConsultarParametrosCompania(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ParametrosCompania)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasParametros_Consultar(pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarParametrosCompania"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarParametrosCompania")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarParametrosCompaniaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ParametrosCompania
        Dim objCompaniasParametrosCompania As ParametrosCompania = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasParametros_Consultar(0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarParametrosCompaniaPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasParametrosCompania = ret.FirstOrDefault
            End If
            Return objCompaniasParametrosCompania
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarParametrosCompaniaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarParametrosCompaniaPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ParametrosCompania
        Dim objTask As Task(Of ParametrosCompania) = Me.ConsultarParametrosCompaniaPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarParametrosCompaniaPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ParametrosCompania)
        Dim objTaskComplete As TaskCompletionSource(Of ParametrosCompania) = New TaskCompletionSource(Of ParametrosCompania)()
        objTaskComplete.TrySetResult(ConsultarParametrosCompaniaPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function





#End Region


#Region "Métodos sincrónicos"
    Public Function ConsultarParametrosCompaniaSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ParametrosCompania)
        Dim objTask As Task(Of List(Of ParametrosCompania)) = Me.ConsultarParametrosCompaniaAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarParametrosCompaniaAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ParametrosCompania))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ParametrosCompania)) = New TaskCompletionSource(Of List(Of ParametrosCompania))()
        objTaskComplete.TrySetResult(ConsultarParametrosCompania(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function




#End Region
#End Region


    ''' <summary>
    ''' Descripción:     Proceso para importar archivo txt  desde la pantalla "Unificar Formatos"
    ''' Responsable:     Jeison Ramírez Pino (IOSoft S.A.S.)
    ''' Fecha:           Octubre 12/2016
    ''' </summary>
    ''' <param name="pstrNombreArchivoFormatoPrincipal"></param>
    ''' <param name="pstrNombreArchivoFormatoProducto"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="pstrMaquina"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
#Region "Unificar Formatos"

    Public Function UnificarFormatos_ImportarExportar(ByVal pstrNombreArchivoFormatoPrincipal As String, ByVal pstrNombreArchivoFormatoProducto As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RetornoInformacionArchivo)
        Dim objListaRetorno As New List(Of RetornoInformacionArchivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[CF].[uspCalculosFinancieros_UnificarFormatos]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            'Dim objListaRetorno As New List(Of RetornoInformacionArchivo)
            Dim strRutaCompletaArchivosPrincipal As String = String.Empty
            Dim strRutaCompletaArchivosProductos As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)

            objRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivosPrincipal = objRutas.RutaArchivoProceso & "\" & pstrNombreArchivoFormatoPrincipal
                strRutaCompletaArchivosProductos = objRutas.RutaArchivoProceso & "\" & pstrNombreArchivoFormatoProducto
            Else
                strRutaCompletaArchivosPrincipal = objRutas.RutaArchivoProceso & pstrNombreArchivoFormatoPrincipal
                strRutaCompletaArchivosProductos = objRutas.RutaArchivoProceso & pstrNombreArchivoFormatoProducto
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaFormatoPrincipal", strRutaCompletaArchivosPrincipal, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrRutaFormatoProductos", strRutaCompletaArchivosProductos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "UnificarFormatos_ImportarExportar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                Dim objItem As New RetornoInformacionArchivo

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.strInformacionGenerar = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            If Not IsNothing(objDatosConsulta.Tables(1)) Then
                Dim strArchivo As String = String.Empty
                strArchivo = GenerarTextoPlano(objDatosConsulta.Tables(1), objRutas.RutaArchivosLocal, "UnificarFormatos_" & Now.ToString("yyyyMMddhhmmss"), "txt", String.Empty, True, False, False)

                If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                    objRutas.RutaWeb = objRutas.RutaWeb & "/"
                End If

                objListaRetorno.Add(New RetornoInformacionArchivo With {.Campo = "",
                                                                        .Columna = 0,
                                                                        .Exitoso = True,
                                                                        .Fila = 1,
                                                                        .ID = -1,
                                                                        .strInformacionGenerar = "El archivo se generó exitosamente",
                                                                        .Tipo = "",
                                                                        .URLArchivo = objRutas.RutaCompartidaOWeb() & strArchivo})
            End If

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarFormatos_ImportarExportar")
            Return objListaRetorno
        End Try
    End Function
#End Region

#Region "AjustesManuales"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertAjustesContablesManual(ByVal currentItem As AjustesContablesManual)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertAjustesContablesManual")
        End Try
    End Sub
    Public Sub UpdateAjustesContablesManual(ByVal currentItem As AjustesContablesManual)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateAjustesContablesManual")
        End Try
    End Sub
    Public Sub DeleteAjustesContablesManual(ByVal currentItem As AjustesContablesManual)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteAjustesContablesManual")
        End Try
    End Sub

    Public Sub InsertAjustesContablesManual_Detalles(ByVal currentItem As AjustesContablesManual_Detalles)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertAjustesContablesManual_Detalles")
        End Try
    End Sub
    Public Sub UpdateAjustesContablesManual_Detalles(ByVal currentItem As AjustesContablesManual_Detalles)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateAjustesContablesManual_Detalles")
        End Try
    End Sub
    Public Sub DeleteAjustesContablesManual_Detalles(ByVal currentItem As AjustesContablesManual_Detalles)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteAjustesContablesManual_Detalles")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function AjustesContablesManual_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AjustesManuales_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlLiquidezOperaciones_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesContablesManual_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function AjustesContablesManual_Consultar(ByVal pintID As System.Nullable(Of Integer),
                                                     ByVal pstrTipoComprobante As String,
                                                     ByVal pstrUsuarioRegistro As String,
                                                     ByVal pdtmFechaRegistro As Nullable(Of System.DateTime),
                                                     ByVal plngIDComitente As String,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AjustesManuales_Consultar(pintID, pstrTipoComprobante, pstrUsuarioRegistro, pdtmFechaRegistro, plngIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "AjustesContablesManual_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesContablesManual_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function AjustesContablesManual_ConsultarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_Combos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AjustesManuales_ConsultarCombos(pstrUsuario, DemeInfoSesion(pstrUsuario, "AjustesContablesManual_ConsultarCombos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesContablesManual_ConsultarCombos")
            Return Nothing
        End Try
    End Function

    Public Function AjustesContablesManual_Detalles_Consultar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_Detalles)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AjustesManuales_Detalles_Consultar(pintID, pstrUsuario, DemeInfoSesion(pstrUsuario, "AjustesContablesManual_Detalles_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesContablesManual_Detalles_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function AjustesContablesManual_Anular(ByVal pintID As Integer, ByVal pstrObservaciones As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AjustesManuales_Anular(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "AjustesContablesManual_Anular"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesContablesManual_Anular")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function AjustesContablesManual_Validar(pintID As Integer,
                                                   pstrTipoComprobante As String,
                                                   pstrUsuarioRegistro As String,
                                                   pdtmFechaRegistro As Nullable(Of DateTime),
                                                   pstrNormaContable As String,
                                                   plngIDComitente As String,
                                                   pdtmFechaAplicacion As Nullable(Of DateTime),
                                                   pintIDMoneda As Nullable(Of Integer),
                                                   pstrCodMoneda As String,
                                                   pstrTipoAjuste As String,
                                                   pintIDEventoContable As Nullable(Of Integer),
                                                   pstrCodEventoContable As String,
                                                   pstrClaseContable As String,
                                                   pstrTipoInversion As String,
                                                   pstrTipoOrigen As String,
                                                   pintOrigen_Numero As Nullable(Of Integer),
                                                   pintOrigen_Secuencia As Nullable(Of Integer),
                                                   pstrOrigen_Tipo As String,
                                                   pstrOrigen_Clase As String,
                                                   pdtmOrigen_Fecha As Nullable(Of DateTime),
                                                   pstrUsuario As String,
                                                   pstrUsuarioWindows As String,
                                                   pstrMaquina As String,
                                                   pstrXMLMovientos As String,
                                                   pstrObservaciones As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrXMLMovientos = HttpUtility.UrlDecode(pstrXMLMovientos)

            Dim ret = Me.DataContext.uspCalculosFinancieros_AjustesManuales_Validar(pintID,
                                                                                    pstrTipoComprobante,
                                                                                    pstrUsuarioRegistro,
                                                                                    pdtmFechaRegistro,
                                                                                    pstrNormaContable,
                                                                                    plngIDComitente,
                                                                                    pdtmFechaAplicacion,
                                                                                    pintIDMoneda,
                                                                                    pstrCodMoneda,
                                                                                    pstrTipoAjuste,
                                                                                    pintIDEventoContable,
                                                                                    pstrCodEventoContable,
                                                                                    pstrClaseContable,
                                                                                    pstrTipoInversion,
                                                                                    pstrTipoOrigen,
                                                                                    pintOrigen_Numero,
                                                                                    pintOrigen_Secuencia,
                                                                                    pstrOrigen_Tipo,
                                                                                    pstrOrigen_Clase,
                                                                                    pdtmOrigen_Fecha,
                                                                                    pstrUsuario,
                                                                                    pstrUsuarioWindows,
                                                                                    pstrMaquina,
                                                                                    pstrXMLMovientos,
                                                                                    pstrObservaciones,
                                                                                    DemeInfoSesion(pstrUsuario, "AjustesContablesManual_Validar"),
                                                                                    0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesContablesManual_Validar")
            Return Nothing
        End Try
    End Function

    Public Function AjustesContablesManual_ValidarEstado(ByVal pintID As Integer, ByVal pstrAccion As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AjustesManuales_ValidarEstado(pintID, pstrAccion, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "AjustesContablesManual_ValidarEstado"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesContablesManual_ValidarEstado")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function AjustesContablesManual_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual)
        Dim objTask As Task(Of List(Of AjustesContablesManual)) = Me.AjustesContablesManual_FiltrarAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function AjustesContablesManual_FiltrarAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of AjustesContablesManual))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of AjustesContablesManual)) = New TaskCompletionSource(Of List(Of AjustesContablesManual))()
        objTaskComplete.TrySetResult(AjustesContablesManual_Filtrar(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function AjustesContablesManual_ConsultarSync(ByVal pintID As System.Nullable(Of Integer),
                                                     ByVal pstrTipoComprobante As String,
                                                     ByVal pstrUsuarioRegistro As String,
                                                     ByVal pdtmFechaRegistro As Nullable(Of System.DateTime),
                                                     ByVal plngIDComitente As String,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual)
        Dim objTask As Task(Of List(Of AjustesContablesManual)) = Me.AjustesContablesManual_ConsultarAsync(pintID, pstrTipoComprobante, pstrUsuarioRegistro, pdtmFechaRegistro, plngIDComitente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function AjustesContablesManual_ConsultarAsync(ByVal pintID As System.Nullable(Of Integer),
                                                     ByVal pstrTipoComprobante As String,
                                                     ByVal pstrUsuarioRegistro As String,
                                                     ByVal pdtmFechaRegistro As Nullable(Of System.DateTime),
                                                     ByVal plngIDComitente As String,
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of AjustesContablesManual))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of AjustesContablesManual)) = New TaskCompletionSource(Of List(Of AjustesContablesManual))()
        objTaskComplete.TrySetResult(AjustesContablesManual_Consultar(pintID, pstrTipoComprobante, pstrUsuarioRegistro, pdtmFechaRegistro, plngIDComitente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function AjustesContablesManual_ConsultarCombosSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_Combos)
        Dim objTask As Task(Of List(Of AjustesContablesManual_Combos)) = Me.AjustesContablesManual_ConsultarCombosAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function AjustesContablesManual_ConsultarCombosAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of AjustesContablesManual_Combos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of AjustesContablesManual_Combos)) = New TaskCompletionSource(Of List(Of AjustesContablesManual_Combos))()
        objTaskComplete.TrySetResult(AjustesContablesManual_ConsultarCombos(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function AjustesContablesManual_Detalles_ConsultarSync(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_Detalles)
        Dim objTask As Task(Of List(Of AjustesContablesManual_Detalles)) = Me.AjustesContablesManual_Detalles_ConsultarAsync(pintID, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function AjustesContablesManual_Detalles_ConsultarAsync(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of AjustesContablesManual_Detalles))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of AjustesContablesManual_Detalles)) = New TaskCompletionSource(Of List(Of AjustesContablesManual_Detalles))()
        objTaskComplete.TrySetResult(AjustesContablesManual_Detalles_Consultar(pintID, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function AjustesContablesManual_AnularSync(ByVal pintID As Integer, ByVal pstrObservaciones As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_RespuestaValidacion)
        Dim objTask As Task(Of List(Of AjustesContablesManual_RespuestaValidacion)) = Me.AjustesContablesManual_AnularAsync(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function AjustesContablesManual_AnularAsync(ByVal pintID As Integer, ByVal pstrObservaciones As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of AjustesContablesManual_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of AjustesContablesManual_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of AjustesContablesManual_RespuestaValidacion))()
        objTaskComplete.TrySetResult(AjustesContablesManual_Anular(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function AjustesContablesManual_ValidarSync(pintID As Integer,
                                                   pstrTipoComprobante As String,
                                                   pstrUsuarioRegistro As String,
                                                   pdtmFechaRegistro As Nullable(Of DateTime),
                                                   pstrNormaContable As String,
                                                   plngIDComitente As String,
                                                   pdtmFechaAplicacion As Nullable(Of DateTime),
                                                   pintIDMoneda As Nullable(Of Integer),
                                                   pstrCodMoneda As String,
                                                   pstrTipoAjuste As String,
                                                   pintIDEventoContable As Nullable(Of Integer),
                                                   pstrCodEventoContable As String,
                                                   pstrClaseContable As String,
                                                   pstrTipoInversion As String,
                                                   pstrTipoOrigen As String,
                                                   pintOrigen_Numero As Nullable(Of Integer),
                                                   pintOrigen_Secuencia As Nullable(Of Integer),
                                                   pstrOrigen_Tipo As String,
                                                   pstrOrigen_Clase As String,
                                                   pdtmOrigen_Fecha As Nullable(Of DateTime),
                                                   pstrUsuario As String,
                                                   pstrUsuarioWindows As String,
                                                   pstrMaquina As String,
                                                   pstrXMLMovientos As String,
                                                   pstrObservaciones As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_RespuestaValidacion)
        Dim objTask As Task(Of List(Of AjustesContablesManual_RespuestaValidacion)) = Me.AjustesContablesManual_ValidarAsync(pintID,
                                                                                                                             pstrTipoComprobante,
                                                                                                                             pstrUsuarioRegistro,
                                                                                                                             pdtmFechaRegistro,
                                                                                                                             pstrNormaContable,
                                                                                                                             plngIDComitente,
                                                                                                                             pdtmFechaAplicacion,
                                                                                                                             pintIDMoneda,
                                                                                                                             pstrCodMoneda,
                                                                                                                             pstrTipoAjuste,
                                                                                                                             pintIDEventoContable,
                                                                                                                             pstrCodEventoContable,
                                                                                                                             pstrClaseContable,
                                                                                                                             pstrTipoInversion,
                                                                                                                             pstrTipoOrigen,
                                                                                                                             pintOrigen_Numero,
                                                                                                                             pintOrigen_Secuencia,
                                                                                                                             pstrOrigen_Tipo,
                                                                                                                             pstrOrigen_Clase,
                                                                                                                             pdtmOrigen_Fecha,
                                                                                                                             pstrUsuario,
                                                                                                                             pstrUsuarioWindows,
                                                                                                                             pstrMaquina,
                                                                                                                             pstrXMLMovientos,
                                                                                                                             pstrObservaciones,
                                                                                                                             pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function AjustesContablesManual_ValidarAsync(pintID As Integer,
                                                   pstrTipoComprobante As String,
                                                   pstrUsuarioRegistro As String,
                                                   pdtmFechaRegistro As Nullable(Of DateTime),
                                                   pstrNormaContable As String,
                                                   plngIDComitente As String,
                                                   pdtmFechaAplicacion As Nullable(Of DateTime),
                                                   pintIDMoneda As Nullable(Of Integer),
                                                   pstrCodMoneda As String,
                                                   pstrTipoAjuste As String,
                                                   pintIDEventoContable As Nullable(Of Integer),
                                                   pstrCodEventoContable As String,
                                                   pstrClaseContable As String,
                                                   pstrTipoInversion As String,
                                                   pstrTipoOrigen As String,
                                                   pintOrigen_Numero As Nullable(Of Integer),
                                                   pintOrigen_Secuencia As Nullable(Of Integer),
                                                   pstrOrigen_Tipo As String,
                                                   pstrOrigen_Clase As String,
                                                   pdtmOrigen_Fecha As Nullable(Of DateTime),
                                                   pstrUsuario As String,
                                                   pstrUsuarioWindows As String,
                                                   pstrMaquina As String,
                                                   pstrXMLMovientos As String,
                                                   pstrObservaciones As String, ByVal pstrInfoConexion As String) As Task(Of List(Of AjustesContablesManual_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of AjustesContablesManual_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of AjustesContablesManual_RespuestaValidacion))()
        objTaskComplete.TrySetResult(AjustesContablesManual_Validar(pintID,
                                                                    pstrTipoComprobante,
                                                                    pstrUsuarioRegistro,
                                                                    pdtmFechaRegistro,
                                                                    pstrNormaContable,
                                                                    plngIDComitente,
                                                                    pdtmFechaAplicacion,
                                                                    pintIDMoneda,
                                                                    pstrCodMoneda,
                                                                    pstrTipoAjuste,
                                                                    pintIDEventoContable,
                                                                    pstrCodEventoContable,
                                                                    pstrClaseContable,
                                                                    pstrTipoInversion,
                                                                    pstrTipoOrigen,
                                                                    pintOrigen_Numero,
                                                                    pintOrigen_Secuencia,
                                                                    pstrOrigen_Tipo,
                                                                    pstrOrigen_Clase,
                                                                    pdtmOrigen_Fecha,
                                                                    pstrUsuario,
                                                                    pstrUsuarioWindows,
                                                                    pstrMaquina,
                                                                    pstrXMLMovientos,
                                                                    pstrObservaciones, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function AjustesContablesManual_ValidarEstadoSync(ByVal pintID As Integer, ByVal pstrAccion As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of AjustesContablesManual_RespuestaValidacion)
        Dim objTask As Task(Of List(Of AjustesContablesManual_RespuestaValidacion)) = Me.AjustesContablesManual_ValidarEstadoAsync(pintID, pstrAccion, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function AjustesContablesManual_ValidarEstadoAsync(ByVal pintID As Integer, ByVal pstrAccion As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of AjustesContablesManual_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of AjustesContablesManual_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of AjustesContablesManual_RespuestaValidacion))()
        objTaskComplete.TrySetResult(AjustesContablesManual_ValidarEstado(pintID, pstrAccion, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CompaniasAcumuladoComsiones"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCompaniasAcumuladoComisiones(ByVal objCompaniasAcumuladoComisiones As CompaniasAcumuladoComisiones)

    End Sub

    Public Sub UpdateCompaniasAcumuladoComisiones(ByVal currentCompaniasAcumuladoComisiones As CompaniasAcumuladoComisiones)

    End Sub

    Public Sub DeleteCompaniasAcumuladoComisiones(ByVal UpdateCompaniasAcumuladoComisiones As CompaniasAcumuladoComisiones)

    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function ConsultarCompaniasAcumuladoComisiones(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasAcumuladoComisiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasAcumuladoComisiones_Consultar(String.Empty, pintIDCompania, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasAcumuladoComisiones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasAcumuladoComisiones")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCompaniasAcumuladoComisionesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasAcumuladoComisiones
        Dim objCompaniasAcumuladosComisiones As CompaniasAcumuladoComisiones = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CompaniasAcumuladoComisiones_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniasAcumuladoComisionesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objCompaniasAcumuladosComisiones = ret.FirstOrDefault
            End If
            Return objCompaniasAcumuladosComisiones
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniasAcumuladoComisionesPorDefecto")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarCompaniasAcumuladoComisionesSync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CompaniasAcumuladoComisiones)
        Dim objTask As Task(Of List(Of CompaniasAcumuladoComisiones)) = Me.ConsultarCompaniasAcumuladoComisionesAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasAcumuladoComisionesAsync(pintIDCompania As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CompaniasAcumuladoComisiones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CompaniasAcumuladoComisiones)) = New TaskCompletionSource(Of List(Of CompaniasAcumuladoComisiones))()
        objTaskComplete.TrySetResult(ConsultarCompaniasAcumuladoComisiones(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniasAcumuladoComisionesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CompaniasAcumuladoComisiones
        Dim objTask As Task(Of CompaniasAcumuladoComisiones) = Me.ConsultarCompaniasAcumuladoComisionesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCompaniasAcumuladoComisionesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of CompaniasAcumuladoComisiones)
        Dim objTaskComplete As TaskCompletionSource(Of CompaniasAcumuladoComisiones) = New TaskCompletionSource(Of CompaniasAcumuladoComisiones)()
        objTaskComplete.TrySetResult(ConsultarCompaniasAcumuladoComisionesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region
#End Region

#Region "ConceptosRetencion"
#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertConceptosRetencion(ByVal newConceptosRetencion As tblConceptoRetencion)
        Try
            newConceptosRetencion.strInfoSesion = DemeInfoSesion(newConceptosRetencion.pstrUsuarioConexion, "InsertConceptosRetencion")
            Me.DataContext.tblConceptoRetencion.InsertOnSubmit(newConceptosRetencion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConceptosRetencion")
        End Try
    End Sub

    Public Sub UpdateConceptosRetencion(ByVal currentConceptosRetencion As tblConceptoRetencion)
        Try
            currentConceptosRetencion.strInfoSesion = DemeInfoSesion(currentConceptosRetencion.pstrUsuarioConexion, "UpdateConceptosRetencion")
            Me.DataContext.tblConceptoRetencion.Attach(currentConceptosRetencion, Me.ChangeSet.GetOriginal(currentConceptosRetencion))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConceptosRetencion")
        End Try
    End Sub

    Public Sub DeleteConceptosRetencion(ByVal deleteConceptosRetencion As tblConceptoRetencion)
        Try
            deleteConceptosRetencion.strInfoSesion = DemeInfoSesion(deleteConceptosRetencion.pstrUsuarioConexion, "DeleteConceptosRetencion")
            Me.DataContext.tblConceptoRetencion.Attach(deleteConceptosRetencion)
            Me.DataContext.tblConceptoRetencion.DeleteOnSubmit(deleteConceptosRetencion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConceptosRetencion")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function ConceptosRetencion_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of tblConceptoRetencion)
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConceptosRetencion_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosRetencion_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosRetencion_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosRetencion_Consultar(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pstrUsuario As String) As List(Of tblConceptoRetencion)
        Try
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConceptosRetencion_Consultar(String.Empty, pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosRetencion_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosRetencion_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosRetencion_ConsultarPorDefecto(ByVal pstrUsuario As String) As tblConceptoRetencion
        Dim objConceptosRetencion As tblConceptoRetencion = Nothing
        Try

            Dim ret = Me.DataContext.uspCalculosFinancieros_ConceptosRetencion_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, -1, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosRetencion_ConsultarPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList

            'If ret.Count > 0 Then
            '    objConceptosRetencion = ret.FirstOrDefault
            'End If

            Return New tblConceptoRetencion ' objConceptosRetencion
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosRetencion_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar el encabezado y detalle
    ''' </summary>
    Public Function ConceptosRetencion_Actualizar(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pdblPorcentajeRetencion As Double, ByVal pdblGravado As Double, ByVal pdblNoGravado As Double, ByVal pstrUsuario As String) As String
        Try
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConceptosRetencion_Actualizar(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pdblPorcentajeRetencion, pdblGravado, pdblNoGravado, pstrUsuario, DemeInfoSesion(pstrUsuario, "CuentasFondos_Actualizar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosRetencion_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConceptoRetencion_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of tblConceptoRetencion)
        Dim objTask As Task(Of List(Of tblConceptoRetencion)) = Me.ConceptoRetencion_FiltrarAsync(pstrFiltro, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConceptoRetencion_FiltrarAsync(ByVal pstrFiltro As String, pstrUsuario As String) As Task(Of List(Of tblConceptoRetencion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblConceptoRetencion)) = New TaskCompletionSource(Of List(Of tblConceptoRetencion))()
        objTaskComplete.TrySetResult(ConceptosRetencion_Filtrar(pstrFiltro, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConceptoRetencion_ConsultarSync(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pstrUsuario As String) As List(Of tblConceptoRetencion)
        Dim objTask As Task(Of List(Of tblConceptoRetencion)) = Me.ConceptoRetencion_ConsultarAsync(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConceptoRetencion_ConsultarAsync(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pstrUsuario As String) As Task(Of List(Of tblConceptoRetencion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblConceptoRetencion)) = New TaskCompletionSource(Of List(Of tblConceptoRetencion))()
        objTaskComplete.TrySetResult(ConceptosRetencion_Consultar(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConceptoRetencion_ConsultarPorDefectoSync(ByVal pstrUsuario As String) As tblConceptoRetencion
        Dim objTask As Task(Of tblConceptoRetencion) = Me.ConceptoRetencion_ConsultarPorDefectoAsync(pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConceptoRetencion_ConsultarPorDefectoAsync(pstrUsuario As String) As Task(Of tblConceptoRetencion)
        Dim objTaskComplete As TaskCompletionSource(Of tblConceptoRetencion) = New TaskCompletionSource(Of tblConceptoRetencion)()
        objTaskComplete.TrySetResult(ConceptosRetencion_ConsultarPorDefecto(pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConceptoRetencion_ActualizarSync(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pdblPorcentajeRetencion As Double, ByVal pdblGravado As Nullable(Of Double), ByVal pdblNoGravado As Nullable(Of Double), ByVal pstrUsuario As String) As String
        Dim objTask As Task(Of String) = Me.ConceptoRetencion_ActualizarAsync(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pdblPorcentajeRetencion, pdblGravado, pdblNoGravado, pstrUsuario)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConceptoRetencion_ActualizarAsync(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pdblPorcentajeRetencion As Double, ByVal pdblGravado As Double, ByVal pdblNoGravado As Double, ByVal pstrUsuario As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ConceptosRetencion_Actualizar(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pdblPorcentajeRetencion, pdblGravado, pdblNoGravado, pstrUsuario))
        Return (objTaskComplete.Task)
    End Function

#End Region
#End Region

#Region "Omnibus"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertOmnibus_Configuracion_Generacion(ByVal currentItem As Omnibus_Configuracion_Generacion)
    End Sub
    Public Sub UpdateOmnibus_Configuracion_Generacion(ByVal currentItem As Omnibus_Configuracion_Generacion)
    End Sub
    Public Sub DeleteOmnibus_Configuracion_Generacion(ByVal currentItem As Omnibus_Configuracion_Generacion)
    End Sub

    Public Sub InsertOmnibus_Combos(ByVal currentItem As Omnibus_Combos)
    End Sub
    Public Sub UpdateOmnibus_Combos(ByVal currentItem As Omnibus_Combos)
    End Sub
    Public Sub DeleteOmnibus_Combos(ByVal currentItem As Omnibus_Combos)
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function Omnibus_ConsultarConfiguracion(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Omnibus_Configuracion_Generacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOmnibus_ConsultarConfiguracionGeneracion(pstrUsuario, DemeInfoSesion(pstrUsuario, "Omnibus_ConsultarConfiguracion"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Omnibus_ConsultarConfiguracion")
            Return Nothing
        End Try
    End Function

    Public Function Omnibus_ConsultarConfiguracion_Importacion(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Omnibus_Configuracion_Importacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOmnibus_ConsultarConfiguracionArchivos(pstrUsuario, DemeInfoSesion(pstrUsuario, "Omnibus_ConsultarConfiguracion_Importacion"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Omnibus_ConsultarConfiguracion_Importacion")
            Return Nothing
        End Try
    End Function

    Public Function Omnibus_ConsultarCombos(ByVal pintIDGestor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Omnibus_Combos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOmnibus_ConsultarCombos(pintIDGestor, pstrUsuario, DemeInfoSesion(pstrUsuario, "Omnibus_ConsultarCombos"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Omnibus_ConsultarCombos")
            Return Nothing
        End Try
    End Function

    Public Function Omnibus_ImportarArchivo(ByVal pstrModulo As String, ByVal pstrNombreArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrTipoGeneracion As String, ByVal pintFilasADescartar As Integer, ByVal pintColumnas As Integer, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RetornoInformacionArchivo)
        Dim objListaRetorno As New List(Of RetornoInformacionArchivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[CF].[uspOmnibus_ImportarArchivos]"
            Dim strRutaCompleta As String = String.Empty
            Dim strInformacionRegistros As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)
            Dim strRutaCompletaArchivoLectura As String = String.Empty

            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompleta = objRutas.RutaArchivoProceso & "\" & pstrNombreArchivo
            Else
                strRutaCompleta = objRutas.RutaArchivoProceso & pstrNombreArchivo
            End If

            If Right(objRutas.RutaArchivosLocal, 1) <> "\" Then
                strRutaCompletaArchivoLectura = objRutas.RutaArchivosLocal & "\" & pstrNombreArchivo
            Else
                strRutaCompletaArchivoLectura = objRutas.RutaArchivosLocal & pstrNombreArchivo
            End If

            If pstrTipoGeneracion = "PLANO" Then
                If pintFilasADescartar > 0 Then
                    Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}
                    objImportar.EliminarFilas(objRutas.RutaArchivosLocal, pstrNombreArchivo, pintFilasADescartar)
                End If
            Else
                strInformacionRegistros = Omnibus_RecorrerArchivosExcel(strRutaCompletaArchivoLectura, pintFilasADescartar, pintColumnas)
            End If

            objListaParametros.Add(CrearSQLParameter("pstrModulo", pstrModulo, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrRutaArchivo", strRutaCompleta, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrRegistrosImporta", strInformacionRegistros, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "Omnibus_ImportarArchivo"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                Dim objItem As New RetornoInformacionArchivo

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.strInformacionGenerar = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Omnibus_ImportarExportar")
            Return objListaRetorno
        End Try
    End Function

    Private Function Omnibus_RecorrerArchivosExcel(ByVal pstrRutaCompletaArchivo As String, ByVal pintFilasDescartar As Integer, ByVal pintColumnas As Integer) As String
        Try
            Dim logError As Boolean = False
            'abro el libro de excel
            Dim workbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook(pstrRutaCompletaArchivo)
            workbook.Sheets(0).Select()

            Dim intFila As Integer = 0 + pintFilasDescartar
            Dim esValorVacio As Boolean = False
            Dim strTerminoFila As String = String.Empty
            Dim strRetorno As String = String.Empty
            Dim strContenidoFila As String = String.Empty

            While esValorVacio = False
                strTerminoFila = String.Empty
                strContenidoFila = String.Empty

                For columna As Integer = 0 To pintColumnas - 1

                    Dim rangoCelda As SpreadsheetGear.IRange = workbook.ActiveWorksheet.Cells(intFila, columna)

                    If String.IsNullOrEmpty(strContenidoFila) Then
                        strContenidoFila = Omnibus_RetornoValor(rangoCelda)
                    Else
                        strContenidoFila = String.Format("{0}*{1}", strContenidoFila, Omnibus_RetornoValor(rangoCelda))
                    End If

                    strTerminoFila = strTerminoFila & Omnibus_RetornoValor(rangoCelda)
                Next

                If String.IsNullOrEmpty(strTerminoFila) Then
                    esValorVacio = True
                Else
                    If String.IsNullOrEmpty(strRetorno) Then
                        strRetorno = strContenidoFila
                    Else
                        strRetorno = String.Format("{0}|{1}", strRetorno, strContenidoFila)
                    End If
                End If

                intFila = intFila + 1
            End While

            Return strRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Omnibus_RecorrerArchivosExcel")
            Return Nothing
        End Try

    End Function

    Private Function Omnibus_RetornoValor(ByVal pobjRangoCelda As SpreadsheetGear.IRange) As String
        If Not IsNothing(pobjRangoCelda.Value) Then
            If pobjRangoCelda.Text.Contains("/") Then
                Return pobjRangoCelda.Text
            Else
                Return pobjRangoCelda.Value.ToString
            End If
        Else
            Return String.Empty
        End If
    End Function

    Public Function Omnibus_ExportarArchivo(ByVal pintGestor As Integer, ByVal pstrCodigoFondos As String, ByVal pstrTiposMovimientos As String, ByVal pstrFiltrosAdcionales As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RetornoInformacionArchivo)
        Dim objListaRetorno As New List(Of RetornoInformacionArchivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[CF].[uspOmnibus_ExportarArchivos]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            'Dim objListaRetorno As New List(Of RetornoInformacionArchivo)
            Dim strRutaCompleta As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)

            objListaParametros.Add(CrearSQLParameter("pintGestor", pintGestor, SqlDbType.Int))
            objListaParametros.Add(CrearSQLParameter("pstrCodigoFondos", pstrCodigoFondos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrTiposMovimientos", pstrTiposMovimientos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrFiltrosAdicionales", pstrFiltrosAdcionales, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "Omnibus_ExportarArchivo"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1
            Dim logPermitirDescargaUsuario As Boolean = True
            Dim strRutaDescargaUsuario As String = String.Empty
            Dim strNombreArchivo As String = String.Empty
            Dim strTipoGeneracion As String = String.Empty
            Dim logNombreColumnasEncabezadoExcel As Boolean = True
            Dim strNombreHoja As String = String.Empty

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                If Not IsNothing(objRow("PermitirDescargaUsuario")) And Not IsDBNull(objRow("PermitirDescargaUsuario")) Then
                    logPermitirDescargaUsuario = CBool(objRow("PermitirDescargaUsuario"))
                End If
                If Not IsNothing(objRow("RutaDescargaArchivo")) And Not IsDBNull(objRow("RutaDescargaArchivo")) Then
                    strRutaDescargaUsuario = CStr(objRow("RutaDescargaArchivo"))
                End If
                If Not IsNothing(objRow("NombreArchivo")) And Not IsDBNull(objRow("NombreArchivo")) Then
                    strNombreArchivo = CStr(objRow("NombreArchivo"))
                End If
                If Not IsNothing(objRow("TipoGeneracion")) And Not IsDBNull(objRow("TipoGeneracion")) Then
                    strTipoGeneracion = CStr(objRow("TipoGeneracion"))
                End If
                If Not IsNothing(objRow("GenerarEncabezadoExcel")) And Not IsDBNull(objRow("GenerarEncabezadoExcel")) Then
                    logNombreColumnasEncabezadoExcel = CBool(objRow("GenerarEncabezadoExcel"))
                End If
                If Not IsNothing(objRow("NombreHoja")) And Not IsDBNull(objRow("NombreHoja")) Then
                    strNombreHoja = CStr(objRow("NombreHoja"))
                End If
            Next

            If Not IsNothing(objDatosConsulta.Tables(1)) Then
                Dim strArchivo As String = String.Empty
                If logPermitirDescargaUsuario Then
                    Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones("Omnibus", pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
                    If strTipoGeneracion.ToUpper = "EXCEL" Then
                        strArchivo = GenerarExcel(objDatosConsulta.Tables(1), objRutas.RutaArchivosLocal, strNombreArchivo, strNombreHoja, logNombreColumnasEncabezadoExcel)
                    Else
                        strArchivo = GenerarTextoPlano(objDatosConsulta.Tables(1), objRutas.RutaArchivosLocal, strNombreArchivo, "txt", String.Empty, True, False, False)
                    End If

                    If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                        objRutas.RutaWeb = objRutas.RutaWeb & "/"
                    End If

                    objListaRetorno.Add(New RetornoInformacionArchivo With {.Campo = "",
                                                                            .Columna = 0,
                                                                            .Exitoso = True,
                                                                            .Fila = 1,
                                                                            .ID = -1,
                                                                            .strInformacionGenerar = "El archivo se generó exitosamente",
                                                                            .Tipo = "",
                                                                            .URLArchivo = objRutas.RutaCompartidaOWeb() & strArchivo})
                Else
                    If strTipoGeneracion.ToUpper = "EXCEL" Then
                        strArchivo = GenerarExcel(objDatosConsulta.Tables(1), strRutaDescargaUsuario, strNombreArchivo, strNombreHoja, logNombreColumnasEncabezadoExcel)
                    Else
                        strArchivo = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaDescargaUsuario, strNombreArchivo, "txt", String.Empty, True, False, False)
                    End If

                    objListaRetorno.Add(New RetornoInformacionArchivo With {.Campo = "",
                                                                            .Columna = 0,
                                                                            .Exitoso = True,
                                                                            .Fila = 1,
                                                                            .ID = -1,
                                                                            .strInformacionGenerar = "El archivo se genero exitosamente en la ruta definida.",
                                                                            .Tipo = ""})
                End If
            End If

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarFormatos_ImportarExportar")
            Return objListaRetorno
        End Try
    End Function


    Public Function ConsultarCompaniaHabilitarInversionista(ByVal pstrNombreCorto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim strResultado As String = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ' Me.DataContext.uspOyDNet_Companias_TesoreriaValidarAnular(pintIDCompania, pdtmFecha, pstrNombreConsecutivo, plngIDdocumento, strResultado, pstrUsuario, DemeInfoSesion(pstrUsuario, "consultarCompaniaTesoreriaValidarAnular"), 0)
            Me.DataContext.uspOmnibus_ConsultarConfiguracionTipoInversionista(pstrNombreCorto, strResultado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCompaniaHabilitarInversionista"), 0)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCompaniaHabilitarInversionista")
        End Try
        Return strResultado
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function Omnibus_ConsultarConfiguracionSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Omnibus_Configuracion_Generacion)
        Dim objTask As Task(Of List(Of Omnibus_Configuracion_Generacion)) = Me.Omnibus_ConsultarConfiguracionAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Omnibus_ConsultarConfiguracionAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Omnibus_Configuracion_Generacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Omnibus_Configuracion_Generacion)) = New TaskCompletionSource(Of List(Of Omnibus_Configuracion_Generacion))()
        objTaskComplete.TrySetResult(Omnibus_ConsultarConfiguracion(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Omnibus_ConsultarConfiguracion_ImportacionSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Omnibus_Configuracion_Importacion)
        Dim objTask As Task(Of List(Of Omnibus_Configuracion_Importacion)) = Me.Omnibus_ConsultarConfiguracion_ImportacionAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Omnibus_ConsultarConfiguracion_ImportacionAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Omnibus_Configuracion_Importacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Omnibus_Configuracion_Importacion)) = New TaskCompletionSource(Of List(Of Omnibus_Configuracion_Importacion))()
        objTaskComplete.TrySetResult(Omnibus_ConsultarConfiguracion_Importacion(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Omnibus_ConsultarCombosSync(ByVal pintIDGestor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Omnibus_Combos)
        Dim objTask As Task(Of List(Of Omnibus_Combos)) = Me.Omnibus_ConsultarCombosAsync(pintIDGestor, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Omnibus_ConsultarCombosAsync(ByVal pintIDGestor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Omnibus_Combos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Omnibus_Combos)) = New TaskCompletionSource(Of List(Of Omnibus_Combos))()
        objTaskComplete.TrySetResult(Omnibus_ConsultarCombos(pintIDGestor, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCompaniaHabilitarInversionistaSync(ByVal pstrNombreCorto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        ' Dim objTask As Task(Of String) = Me.consultarCompaniaTesoreriaValidarAnularAsync(pintIDCompania, pdtmFecha, pstrNombreConsecutivo, plngIDdocumento, pstrUsuario, pstrInfoConexion)
        Dim objTask As Task(Of String) = Me.ConsultarCompaniaHabilitarInversionistaAsync(pstrNombreCorto, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarCompaniaHabilitarInversionistaAsync(ByVal pstrNombreCorto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ConsultarCompaniaHabilitarInversionista(pstrNombreCorto, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ParesDeDivisas"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertParesDeDivisas(ByVal objInsertParesDeDivisas As ParesDeDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objInsertParesDeDivisas.strUsuarioConexion, objInsertParesDeDivisas.strInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objInsertParesDeDivisas.strInfoSesion = DemeInfoSesion(objInsertParesDeDivisas.strUsuarioConexion, "InsertParesDeDivisas")
            Me.DataContext.ParesDeDivisas.InsertOnSubmit(objInsertParesDeDivisas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertParesDeDivisas")
        End Try
    End Sub

    Public Sub UpdateParesDeDivisas(ByVal objUpdateParesDeDivisas As ParesDeDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objUpdateParesDeDivisas.strUsuarioConexion, objUpdateParesDeDivisas.strInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objUpdateParesDeDivisas.strInfoSesion = DemeInfoSesion(objUpdateParesDeDivisas.strUsuarioConexion, "UpdateParesDeDivisas")
            Me.DataContext.ParesDeDivisas.Attach(objUpdateParesDeDivisas, Me.ChangeSet.GetOriginal(objUpdateParesDeDivisas))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateParesDeDivisas")
        End Try
    End Sub

    Public Sub DeleteParesDeDivisas(ByVal objDeleteParesDeDivisas As ParesDeDivisas)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objDeleteParesDeDivisas.pstrUsuarioConexion, objDeleteParesDeDivisas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objDeleteParesDeDivisas.strInfoSesion = DemeInfoSesion(objDeleteParesDeDivisas.strUsuarioConexion, "DeleteParesDeDivisas")
            Me.DataContext.ParesDeDivisas.Attach(objDeleteParesDeDivisas)
            Me.DataContext.ParesDeDivisas.DeleteOnSubmit(objDeleteParesDeDivisas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteParesDeDivisas")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ParesDeDivisas_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ParesDeDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ParesDeDivisas_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ParesDeDivisas_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ParesDeDivisas_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function ParesDeDivisas_Consultar(ByVal pintIDMonedaOrigen As System.Nullable(Of Integer), ByVal pintIDMonedaDestino As System.Nullable(Of Integer), ByVal plogCambioCruzado As System.Nullable(Of System.Boolean), ByVal pintIDMonedaCambioCruzado As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ParesDeDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ParesDeDivisas_Consultar(String.Empty, pintIDMonedaOrigen, pintIDMonedaDestino, plogCambioCruzado, pintIDMonedaCambioCruzado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ParesDeDivisas_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ParesDeDivisas_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function ParesDeDivisas_ConsultarPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ParesDeDivisas
        Dim objParesDeDivisas As ParesDeDivisas = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ParesDeDivisas_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, 0, False, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ParesDeDivisas_ConsultarPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objParesDeDivisas = ret.FirstOrDefault
            End If
            Return objParesDeDivisas
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ParesDeDivisas_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

    '''' <summary>
    '''' Función encargada de insertar o modificar el encabezado y detalle
    '''' </summary>
    'Public Function ParesDeDivisas_Actualizar(ByVal pintIDParesDeDivisas As Integer, ByVal pstrIDEspecie As String, ByVal plogConstruir As Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pintIDEstadosConceptoTitulos As System.Nullable(Of Integer), ByVal plogMGC As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
    '    Try
    '        ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
    '        Dim pstrMsgValidacion As String = ""
    '        Dim ret = Me.DataContext.uspCalculosFinancieros_ParesDeDivisas_Actualizar(pintIDParesDeDivisas, pintIDMonedaOrigen, pintIDMonedaDestino, plogCambioCruzado, pintIDMonedaCambioCruzado, pdblComisionMonedaOrigen, pdblComisionMonedaDestino, pstrCurvaMonedaOrigenNovado, pstrCurvaMonedaOrigenNoNovado, pstrCurvaMonedaDestinoNovado, pstrCurvaMonedaDestinoNoNovado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ParesDeDivisas_Actualizar"), 0, pstrMsgValidacion)
    '        Return pstrMsgValidacion
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "ParesDeDivisas_Actualizar")
    '        Return Nothing
    '    End Try
    'End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ParesDeDivisas_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ParesDeDivisas)
        Dim objTask As Task(Of List(Of ParesDeDivisas)) = Me.ParesDeDivisas_FiltrarAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ParesDeDivisas_FiltrarAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ParesDeDivisas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ParesDeDivisas)) = New TaskCompletionSource(Of List(Of ParesDeDivisas))()
        objTaskComplete.TrySetResult(ParesDeDivisas_Filtrar(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ParesDeDivisas_ConsultarSync(ByVal pintIDMonedaOrigen As System.Nullable(Of Integer), ByVal pintIDMonedaDestino As System.Nullable(Of Integer), ByVal plogCambioCruzado As System.Nullable(Of System.Boolean), ByVal pintIDMonedaCambioCruzado As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ParesDeDivisas)
        Dim objTask As Task(Of List(Of ParesDeDivisas)) = Me.ParesDeDivisas_ConsultarAsync(pintIDMonedaOrigen, pintIDMonedaDestino, plogCambioCruzado, pintIDMonedaCambioCruzado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ParesDeDivisas_ConsultarAsync(ByVal pintIDMonedaOrigen As System.Nullable(Of Integer), ByVal pintIDMonedaDestino As System.Nullable(Of Integer), ByVal plogCambioCruzado As System.Nullable(Of System.Boolean), ByVal pintIDMonedaCambioCruzado As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ParesDeDivisas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ParesDeDivisas)) = New TaskCompletionSource(Of List(Of ParesDeDivisas))()
        objTaskComplete.TrySetResult(ParesDeDivisas_Consultar(pintIDMonedaOrigen, pintIDMonedaDestino, plogCambioCruzado, pintIDMonedaCambioCruzado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ParesDeDivisas_ConsultarPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ParesDeDivisas
        Dim objTask As Task(Of ParesDeDivisas) = Me.ParesDeDivisas_ConsultarPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ParesDeDivisas_ConsultarPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ParesDeDivisas)
        Dim objTaskComplete As TaskCompletionSource(Of ParesDeDivisas) = New TaskCompletionSource(Of ParesDeDivisas)()
        objTaskComplete.TrySetResult(ParesDeDivisas_ConsultarPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    'Public Function ParesDeDivisas_ActualizarSync(ByVal pintIDParesDeDivisas As Integer, ByVal pstrIDEspecie As String, ByVal plogConstruir As Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pintIDEstadosConceptoTitulos As System.Nullable(Of Integer), ByVal plogMGC As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
    '    Dim objTask As Task(Of String) = Me.ParesDeDivisas_ActualizarAsync(pintIDParesDeDivisas, pstrIDEspecie, plogConstruir, pdtmFechaProceso, pxmlDetalleGrid, pintIDEstadosConceptoTitulos, plogMGC, pstrUsuario, pstrInfoConexion)
    '    objTask.Wait()
    '    Return objTask.Result
    'End Function
    'Private Function ParesDeDivisas_ActualizarAsync(ByVal pintIDParesDeDivisas As Integer, ByVal pstrIDEspecie As String, ByVal plogConstruir As Nullable(Of System.Boolean), ByVal pdtmFechaProceso As Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pintIDEstadosConceptoTitulos As System.Nullable(Of Integer), ByVal plogMGC As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
    '    Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
    '    objTaskComplete.TrySetResult(ParesDeDivisas_Actualizar(pintIDParesDeDivisas, pstrIDEspecie, plogConstruir, pdtmFechaProceso, pxmlDetalleGrid, pintIDEstadosConceptoTitulos, plogMGC, pstrUsuario, pstrInfoConexion))
    '    Return (objTaskComplete.Task)
    'End Function

#End Region

#End Region

End Class

