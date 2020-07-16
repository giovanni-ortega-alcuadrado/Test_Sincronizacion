Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFReporteriaLegal
Imports System.Text
Imports System.Web
Imports System.Web.UI.Page
Imports System.Web.Configuration
Imports System.Data.SqlClient
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
Partial Public Class ReporteriaLegalDomainService
    Inherits LinqToSqlDomainService(Of ReporteriaLegalDBML)

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

#Region "ConfiguracionReportes"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertConfiguracionReportes(ByVal objConfiguracionReportes As ConfiguracionReportes)

    End Sub

    Public Sub UpdateConfiguracionReportes(ByVal currentConfiguracionReportes As ConfiguracionReportes)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    ''' <summary>
    ''' Descripción:   Método para consultar la configuración de reportes.
    ''' Responsable:   Javier Eduardo Pardo Moreno
    ''' ID del cambio: JEPM20160404
    ''' </summary>
    ''' <param name="pstrUsuario">Nombre de Usuario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)>
    Public Function ConsultarConfiguracionReportes(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionReportes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionReportes_Consultar(pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionReportes"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionReportes")
            Return Nothing
        End Try
    End Function

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
    ''' <history>
    ''' Modificado por  : Javier Eduardo Pardo Moreno
    ''' Fecha           : febrero 08/2018
    ''' Pruebas CB      : Javier Eduardo Pardo Moreno - Febrero 08/2018 - Resultado OK
    ''' </history>
    Public Function ObtenerNombreFormato(ByVal strRetorno As String, ByVal strFechaExportacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim NombreFormato As String = String.Empty
            Dim ListaArchivosAExportar As String = String.Empty

            Dim ret = Me.DataContext.uspCalculosFinancieros_ObtenerNombreFormato(strRetorno,
                                                                                 strFechaExportacion,
                                                                                 pstrUsuario,
                                                                                 DemeInfoSesion(pstrUsuario, "ObtenerNombreFormato"), 0,
                                                                                 NombreFormato,
                                                                                 ListaArchivosAExportar)
            Return ListaArchivosAExportar
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerNombreFormato")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Descripción:   Método para consultar las entidades de control del país.
    ''' Responsable:   Natalia Andrea Otalvaro
    ''' ID del cambio: JDCP20190521
    ''' </summary>
    ''' <param name="pstrCodigoISOPais">Código pais</param>
    ''' <param name="pstrUsuario">Nombre de Usuario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)>
    Public Function ConsultarConfiguracionReportes_EntidadesControl(ByVal pstrCodigoISOPais As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblEntidadControl)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionReportes_ConsultarEntidadControl(pstrCodigoISOPais, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionReportes_EntidadesControl"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionReportes_EntidadesControl")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Descripción:   Método para consultar los valores por defecto para la pantalla.
    ''' Responsable:   Natalia Andrea Otalvaro
    ''' ID del cambio: JDCP20190521
    ''' </summary>
    ''' <param name="pstrUsuario">Nombre de Usuario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)>
    Public Function ConsultarConfiguracionReportes_ValoresDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblValoresDefecto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionReportes_ValoresDefecto(pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionReportes_ValoresDefecto"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionReportes_ValoresDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Descripción:   Método para consultar los combos para la pantalla.
    ''' Responsable:   Natalia Andrea Otalvaro
    ''' ID del cambio: JDCP20190521
    ''' </summary>
    ''' <param name="pstrUsuario">Nombre de Usuario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)>
    Public Function ConsultarConfiguracionReportes_ConsultarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblCombosReporteria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionReportes_ConsultarCombos(pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionReportes_ConsultarCombos"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionReportes_ConsultarCombos")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ConsultarConfiguracionReportesSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionReportes)
        Dim objTask As Task(Of List(Of ConfiguracionReportes)) = Me.ConsultarConfiguracionReportesAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsultarConfiguracionReportesAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionReportes))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionReportes)) = New TaskCompletionSource(Of List(Of ConfiguracionReportes))()
        objTaskComplete.TrySetResult(ConsultarConfiguracionReportes(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Private Function ObtenerNombreFormatoAsync(ByVal strRetorno As String, ByVal strFechaExportacion As String, ByVal strUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ObtenerNombreFormato(strRetorno, strFechaExportacion, strUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ObtenerNombreFormatoSync(ByVal strRetorno As String, ByVal strFechaExportacion As String, ByVal strUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ObtenerNombreFormatoAsync(strRetorno, strFechaExportacion, strUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

#End Region

#End Region

End Class

Public Class RutasArchivos
    Public Property NombreProceso As String
    Public Property RutaWeb As String
    Public Property RutaArchivosLocal As String
    Public Property MensajeDebbug As String
    Public Property RutaArchivosUpload As String
    Public Property RutaArchivoUploadProceso As String
    Public Property RutaArchivoProceso As String
End Class
