

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
Imports A2Utilidades.Utilidades
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Xml.Linq
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.Globalization
Imports System.Threading
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports System.Web.UI.Page
Imports System.Web.Configuration
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura
Imports System.Reflection

'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class UtilidadesDomainService
    Inherits LinqToSqlDomainService(Of OYDUtilidadesDataContext)


    'JAEZ funcion para tomar el tipo de modulo desde en web.config 20161003
    Public Function Modulos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            Dim strModuloPantalla As String = String.Empty
            Dim strDelimitador As Char = CChar(",")

            Dim strModulos As String = WebConfigurationManager.AppSettings("Modulos")

            If Not String.IsNullOrEmpty(strModulos) Then

                Dim strLista() As String = strModulos.Split(strDelimitador)

                For Each UnicoModulo In strLista
                    If CBool(InStr(1, UnicoModulo, "[CONTABILIDAD]")) Then
                        strModuloPantalla = Right(UnicoModulo, Len(UnicoModulo) - InStrRev(UnicoModulo, "="))
                    End If
                Next

            End If

            Return strModuloPantalla
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Sub New() 'JBT20140328
        Me.DataContext.CommandTimeout = 0
    End Sub
    Public Const CSTR_NOMBREPROCESO_AUDITORIA_TABLAS = "AUDITORIATABLAS"
    Dim RETORNO As Boolean

#Region "ValidacionSeguridad"

    Public Function Seguridad_Validacion(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            Dim objRetorno As String = String.Empty

            Dim strTextoConcatenado As String = String.Empty
            Dim strRespuesta As String = A2.OyD.Infraestructura.RealizarValidacionSeguridadUsuario(True, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado, strTextoConcatenado)
            If Not String.IsNullOrEmpty(strTextoConcatenado) Then
                If String.IsNullOrEmpty(strRespuesta) Then
                    strRespuesta = "OK"
                End If
                objRetorno = strRespuesta & vbCrLf & strTextoConcatenado
            Else
                objRetorno = strRespuesta
            End If

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Seguridad_Validacion")
            Return Nothing
        End Try
    End Function
    Public Function Seguridad_ValidacionSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.Seguridad_ValidacionAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Seguridad_ValidacionAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(Seguridad_Validacion(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Seguridad_ObtenerIPCliente() As String
        Try
            Return clsCifradoServer.IPV4
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Seguridad_ObtenerIPCliente")
            Return Nothing
        End Try
    End Function
    Public Function Seguridad_ObtenerIPClienteSync() As String
        Dim objTask As Task(Of String) = Me.Seguridad_ObtenerIPClienteAsync()
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Seguridad_ObtenerIPClienteAsync() As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(Seguridad_ObtenerIPCliente())
        Return (objTaskComplete.Task)
    End Function

    Public Function Seguridad_ObtenerUsuarioCliente() As String
        Try
            Return HttpContext.Current.Request.ServerVariables("logon_user")
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Seguridad_ObtenerIPCliente")
            Return Nothing
        End Try
    End Function
    Public Function Seguridad_ObtenerUsuarioClienteSync() As String
        Dim objTask As Task(Of String) = Me.Seguridad_ObtenerUsuarioClienteAsync()
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Seguridad_ObtenerUsuarioClienteAsync() As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(Seguridad_ObtenerUsuarioCliente())
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Comunes"

#Region "Combos Genericos"

    ''' <summary>
    ''' Método para cargar los combos genericos enfocado en wpf
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function Plataforma_Util_CargarCombos(ByVal pstrProducto As String, ByVal pstrCondicionTexto1 As String, ByVal pstrCondicionTexto2 As String, ByVal pstrCondicionEntero1 As Nullable(Of Integer), ByVal pstrCondicionEntero2 As Nullable(Of Integer), ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.PLATAFORMA_CombosGenericos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspA2_Util_CargaCombosGenerico(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "Plataforma_Util_CargarCombos"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Plataforma_Util_CargarCombos")
            Return Nothing
        End Try
    End Function

    Public Function Plataforma_Util_CargarCombosSync(ByVal pstrProducto As String, ByVal pstrCondicionTexto1 As String, ByVal pstrCondicionTexto2 As String, ByVal pstrCondicionEntero1 As Nullable(Of Integer), ByVal pstrCondicionEntero2 As Nullable(Of Integer), ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.PLATAFORMA_CombosGenericos)
        Dim objTask As Task(Of List(Of OYDUtilidades.PLATAFORMA_CombosGenericos)) = Me.Plataforma_Util_CargarCombosAsync(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, pstrModulo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Plataforma_Util_CargarCombosAsync(ByVal pstrProducto As String, ByVal pstrCondicionTexto1 As String, ByVal pstrCondicionTexto2 As String, ByVal pstrCondicionEntero1 As Nullable(Of Integer), ByVal pstrCondicionEntero2 As Nullable(Of Integer), ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OYDUtilidades.PLATAFORMA_CombosGenericos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OYDUtilidades.PLATAFORMA_CombosGenericos)) = New TaskCompletionSource(Of List(Of OYDUtilidades.PLATAFORMA_CombosGenericos))()
        objTaskComplete.TrySetResult(Plataforma_Util_CargarCombos(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, pstrModulo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


#End Region

#Region "Combos"

    ''' <summary>
    ''' Método para cargar los combos generales
    ''' </summary>
    ''' <history>
    ''' Modificado por   : Jhonatan Acevedo (Alcuadrado S.A.)
    ''' Descripción      : Modificado para enviar el usuario sl sp cargarCombos, necesario para otorgar permisos sobre formatos.
    ''' Fecha            : Marzo 30/2015
    ''' Pruebas CB       : Jhonatan Acevedo (Alcuadrado S.A.) - Marzo 30/2015 - Resultado Ok 
    ''' </history>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function cargarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos("", "")
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombos")
            Return Nothing
        End Try
    End Function

    Public Function cargarCombos_Usuario(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos("", pstrUsuario)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Método para cargar combos específicos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function cargarCombosEspecificos(ByVal pstrListasCombos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos(pstrListasCombos, pstrUsuario)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosEspecificos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Método para cargar combos específicos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function cargarCombosEspecificos_SinUsuario(ByVal pstrListasCombos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos(pstrListasCombos, Nothing)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosEspecificos_ValorOriginalUsuario")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Método para cargar combos específicos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function cargarCombosEspecificos_ConUsuario(ByVal pstrListasCombos As String, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos(pstrListasCombos, pstrUsuario)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosEspecificos_ValorOriginalUsuario")
            Return Nothing
        End Try
    End Function

    'Se adicionan los metodos InsertItemCombo, UpdateItemCombo y DeleteItemCombo 
    'para que en los viewmodels se puedan hacer modificaciones a los listados de combos

    Public Sub InsertItemCombo(ByVal itemCombo As OYDUtilidades.ItemCombo)
        'Metodo Generado automaticamente para poder realizar cambios en los listado de combos
    End Sub

    Public Sub UpdateItemCombo(ByVal currentItemCombo As OYDUtilidades.ItemCombo)
        'Metodo Generado automaticamente para poder realizar cambios en los listado de combos
    End Sub

    Public Sub DeleteItemCombo(ByVal itemCombo As OYDUtilidades.ItemCombo)
        'Metodo Generado automaticamente para poder realizar cambios en los listado de combos
    End Sub


#End Region

    ''' <summary>
    ''' Método para consultar parametros
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''
    Public Function Verificaparametro(ByVal strparametro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strretorno As String = String.Empty
            Dim ret = Me.DataContext.usp_Validaparametro_OyDNet(strparametro, strretorno)
            Return strretorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Verificaparametro")
            Return Nothing
        End Try
    End Function
    Public Function VerificaparametroSync(ByVal strparametro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.VerificaparametroAsync(strparametro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function VerificaparametroAsync(ByVal strparametro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(Verificaparametro(strparametro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function listaVerificaparametro(ByVal strparametro As String, ByVal strretorno As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of valoresparametro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_listaValidaparametro_OyDNet(strparametro, strretorno).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "listaVerificaparametro")
            Return Nothing
        End Try
    End Function
    Public Function listaVerificaparametroSync(ByVal strparametro As String, ByVal strretorno As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of valoresparametro)
        Dim objTask As Task(Of List(Of valoresparametro)) = Me.listaVerificaparametroAsync(strparametro, strretorno, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function listaVerificaparametroAsync(ByVal strparametro As String, ByVal strretorno As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of valoresparametro))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of valoresparametro)) = New TaskCompletionSource(Of List(Of valoresparametro))()
        objTaskComplete.TrySetResult(listaVerificaparametro(strparametro, strretorno, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Función para consultar los campos obligatorios de la tabla indicada
    ''' </summary>
    ''' <param name="pstrNombreTabla">Nombre de la tabla</param>
    ''' <param name="pstrNombreCampoObligado">Campos obligatorios</param>
    ''' <param name="pstrNombreCampoCondicionante">Nombre del campo condicionante</param>
    ''' <param name="pstrValorCampoCondicionante">Valor del campo condicionante</param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130301</remarks>
    Public Function ConsularCamposObligatorios(ByVal pstrNombreTabla As String, ByVal pstrNombreCampoObligado As String, ByVal pstrNombreCampoCondicionante As String, ByVal pstrValorCampoCondicionante As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.CamposObligatorios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_CamposObligatorios_Consultar_OyDNet(pstrNombreTabla, pstrNombreCampoObligado, pstrNombreCampoCondicionante, pstrValorCampoCondicionante, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsularCamposObligatorios"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsularCamposObligatorios")
            Return Nothing
        End Try
    End Function


    Public Sub InsertCamposObligatorio(ByVal CampoObligatorio As CamposObligatorios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, CampoObligatorio.pstrUsuarioConexion, CampoObligatorio.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CampoObligatorio.InfoSesion = DemeInfoSesion(CampoObligatorio.pstrUsuarioConexion, "InsertCamposObligatorio")
            Me.DataContext.CamposObligatorios.InsertOnSubmit(CampoObligatorio)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCamposObligatorios")
        End Try
    End Sub


    Public Sub DeleteCampoObligatorio(ByVal CampoObligatorio As CamposObligatorios)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CampoObligatorio.pstrUsuarioConexion, CampoObligatorio.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CampoObligatorio.InfoSesion = DemeInfoSesion(CampoObligatorio.pstrUsuarioConexion, "DeleteCampoObligatorio")
            Me.DataContext.CamposObligatorios.Attach(CampoObligatorio)
            Me.DataContext.CamposObligatorios.DeleteOnSubmit(CampoObligatorio)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCampoObligatorio")

        End Try
    End Sub

    ''' <summary>
    ''' Función que verifica si el Cliente esta Inhabilitado o tiene alguna semejanza
    ''' </summary>
    ''' <param name="pstrNroDocumento">Nro documento del Cliente</param>
    ''' <param name="pstrNombre">Nombre del Cliente</param>
    ''' <returns>Retorno una Lista de Tipo ClienteInhabilitado</returns>
    ''' <remarks>SLB20130204</remarks>
    <Query(HasSideEffects:=True)>
    Public Function ValidarClienteInhabilitado(ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClienteInhabilitado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarInhabilitados_OyDNet(pstrNroDocumento, pstrNombre, False, DemeInfoSesion(pstrUsuario, "ValidarClienteInhabilitado"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarClienteInhabilitado")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function ValidarClienteInhabilitadoNombre(ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClienteInhabilitado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarInhabilitados_OyDNet(pstrNroDocumento, pstrNombre, False, DemeInfoSesion(pstrUsuario, "ValidarClienteInhabilitado"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarClienteInhabilitado")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function ValidarClienteInhabilitadoSegundoMensaje(ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal psegundomensaje As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClienteInhabilitado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarInhabilitados_OyDNet(pstrNroDocumento, pstrNombre, psegundomensaje, DemeInfoSesion(pstrUsuario, "ValidarClienteInhabilitado"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarClienteInhabilitado")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar el Log en tblLogListaClinton cuando hay semejanza con un cliente Inhabilitado
    ''' </summary>
    ''' <param name="pstrForma">Nombre de la Forma de la se insertando el Log</param>
    ''' <param name="plngIDComitente">Codigo OyD</param>
    ''' <param name="pstrDocumento">Nro Documento</param>
    ''' <param name="pstrCliente">Nombre del Cliente</param>
    ''' <param name="pstrClienteInhabilitado">Cliente con el cual tiene la semejanza</param>
    ''' <param name="pdblPorcentajeCercania">Porcentaje de semejanza</param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Si es True es porque inserto correctamente el Log</returns>
    ''' <remarks>SLB20130205</remarks>
    Public Function GrabarListaClinton(ByVal pstrForma As String, ByVal plngIDComitente As String, ByVal pstrDocumento As String, ByVal pstrCliente As String,
                                       ByVal pstrClienteInhabilitado As String, ByVal pdblPorcentajeCercania As Decimal, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_InsertarlogListaClinton_OyDNet(pstrForma, plngIDComitente, pstrDocumento, pstrCliente, pstrClienteInhabilitado, pdblPorcentajeCercania, pstrUsuario)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrabarListaClinton")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Consulta de listas especificas con condicional 
    ''' </summary>
    ''' <param name="pstrListasCombos">Topico listas a consultar</param>
    ''' <param name="pstrCondicionTexto">Condición en texto</param>
    ''' <param name="pintCondicionNumerica">Condicion numérica</param>
    ''' <param name="pstrUsuario">Usuario que ejecuta el proceso</param>
    ''' <returns>Lista de ItemCombo</returns>
    ''' <remarks>Santiago Vergara - Octubre 30/2013</remarks>
    Public Function cargarCombosCondicional(ByVal pstrListasCombos As String, ByVal pstrCondicionTexto As String, ByVal pintCondicionNumerica As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombosCondicional(pstrListasCombos, pstrCondicionTexto, pintCondicionNumerica, pstrUsuario)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosCondicional")
            Return Nothing
        End Try
    End Function



#End Region

#Region "Buscadores"

    ''' <summary>
    ''' Buscar los clientes que cumplan con los criterios definidos por los parámetros
    ''' </summary>
    ''' <param name="pstrCondicionFiltro">Condición que digita el usuario o que se asigna por programa</param>
    ''' <param name="pstrEstadoCliente">Indica sobre que grupo de clientes buscar (activos, inactivos, bloqueados, todos)</param>
    ''' <param name="pstrAgrupacion">Indica sobre que grupo de clientes buscar (todos, comitentes, ordenenates, ambos, etc.)</param>
    ''' <param name="pstrUsuario">Usuario que realiza la operación</param>
    ''' <returns>Lista de clientes</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function buscarClientes(ByVal pstrCondicionFiltro As String, ByVal pstrEstadoCliente As String, ByVal pstrTipoVinculacion As String, ByVal pstrAgrupacion As String, ByVal pstrUsuario As String, ByVal plogExcluirCodigosCompania As Boolean, ByVal pintIDCompania As Nullable(Of Integer), ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Clientes(HttpUtility.UrlDecode(pstrCondicionFiltro), pstrEstadoCliente, pstrTipoVinculacion, pstrAgrupacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarClientes"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogExcluirCodigosCompania, pintIDCompania, Nothing, Nothing, Nothing).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarClientes")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Buscar los clientes que cumplan con los criterios definidos por los parámetros
    ''' Se adiciona un nuevo parametro para habilitar la busqueda de clientes de un receptor tercero
    ''' </summary>
    ''' <param name="pstrCondicionFiltro">Condición que digita el usuario o que se asigna por programa</param>
    ''' <param name="pstrEstadoCliente">Indica sobre que grupo de clientes buscar (activos, inactivos, bloqueados, todos)</param>
    ''' <param name="pstrAgrupacion">Indica sobre que grupo de clientes buscar (todos, comitentes, ordenenates, ambos, etc.)</param>
    ''' <param name="pstrUsuario">Usuario que realiza la operación</param>
    ''' <param name="plogCargarClientesTerecero">Indica si carga los clientes de otro receptor.</param>
    ''' <returns>Lista de clientes</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function buscarClientesOYDPLUS(ByVal pstrCondicionFiltro As String, ByVal pstrEstadoCliente As String, ByVal pstrTipoVinculacion As String,
                                          ByVal pstrAgrupacion As String, ByVal plogCargarUsuariosRestriccion As Boolean, ByVal plogCargarClientesTerecero As Boolean,
                                          ByVal pstrIDReceptor As String, ByVal pstrTipoNegocio As String, ByVal pstrTipoProducto As String, ByVal pstrPerfilRiesgo As String, ByVal pstrUsuario As String, ByVal plogExcluirCodigosCompania As Boolean, ByVal pintIDCompania As Nullable(Of Integer), ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_ClientesOYDPLUS(HttpUtility.UrlDecode(pstrCondicionFiltro), pstrEstadoCliente, pstrTipoVinculacion, pstrAgrupacion, plogCargarUsuariosRestriccion, plogCargarClientesTerecero, pstrIDReceptor, pstrTipoNegocio, pstrTipoProducto, pstrPerfilRiesgo, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarClientesOYDPLUS"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogExcluirCodigosCompania, pintIDCompania, Nothing, Nothing, Nothing).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarClientesOYDPLUS")
            Return Nothing
        End Try
    End Function

    Public Function buscarClienteEspecifico(ByVal pstrIdCliente As String, ByVal pstrUsuario As String, ByVal pstrAgrupacion As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Clientes(pstrIdCliente, "", "", pstrAgrupacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarClienteEspecifico"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, False, 0, Nothing, Nothing, Nothing).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarClienteEspecifico")
            Return Nothing
        End Try
    End Function

    Public Function buscarClienteEspecificoCompania(ByVal pstrIdCliente As String, ByVal pstrUsuario As String, ByVal pstrAgrupacion As String, ByVal plogExcluirCodigosCompania As Boolean, ByVal pintIDCompania As Nullable(Of Integer), ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Clientes(pstrIdCliente, "", "", pstrAgrupacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarClienteEspecificoCompania"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogExcluirCodigosCompania, pintIDCompania, Nothing, Nothing, Nothing).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarClienteEspecificoCompania")
            Return Nothing
        End Try
    End Function


    Public Function buscarClientesconFiltros(ByVal pstrCondicionFiltro As String, ByVal pstrEstadoCliente As String, ByVal pstrTipoVinculacion As String, ByVal pstrAgrupacion As String, ByVal pstrUsuario As String, ByVal plogExcluirCodigosCompania As Boolean, ByVal pintIDCompania As Nullable(Of Integer), ByVal pstrInfoConexion As String, ByVal pstrfiltroAdicional1 As String, ByVal pstrfiltroAdicional2 As String, ByVal pstrfiltroAdicional3 As String) As List(Of OYDUtilidades.BuscadorClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Clientes(HttpUtility.UrlDecode(pstrCondicionFiltro), pstrEstadoCliente, pstrTipoVinculacion, pstrAgrupacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarClientes"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogExcluirCodigosCompania, pintIDCompania, pstrfiltroAdicional1, pstrfiltroAdicional2, pstrfiltroAdicional3).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarClientes")
            Return Nothing
        End Try
    End Function

    Public Function buscarClientesOYDPLUSconFiltros(ByVal pstrCondicionFiltro As String, ByVal pstrEstadoCliente As String, ByVal pstrTipoVinculacion As String,
                                          ByVal pstrAgrupacion As String, ByVal plogCargarUsuariosRestriccion As Boolean, ByVal plogCargarClientesTerecero As Boolean,
                                          ByVal pstrIDReceptor As String, ByVal pstrTipoNegocio As String, ByVal pstrTipoProducto As String, ByVal pstrPerfilRiesgo As String, ByVal pstrUsuario As String, ByVal plogExcluirCodigosCompania As Boolean, ByVal pintIDCompania As Nullable(Of Integer), ByVal pstrInfoConexion As String, ByVal pstrfiltroAdicional1 As String, ByVal pstrfiltroAdicional2 As String, ByVal pstrfiltroAdicional3 As String) As List(Of OYDUtilidades.BuscadorClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_ClientesOYDPLUS(HttpUtility.UrlDecode(pstrCondicionFiltro), pstrEstadoCliente, pstrTipoVinculacion, pstrAgrupacion, plogCargarUsuariosRestriccion, plogCargarClientesTerecero, pstrIDReceptor, pstrTipoNegocio, pstrTipoProducto, pstrPerfilRiesgo, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarClientesOYDPLUS"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogExcluirCodigosCompania, pintIDCompania, pstrfiltroAdicional1, pstrfiltroAdicional2, pstrfiltroAdicional3).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarClientesOYDPLUS")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Buscar las especies que cumplan con los criterios definidos por los parámetros
    ''' </summary>
    ''' <param name="pstrCondicionFiltro">Condición que digita el usuario o que se asigna por programa</param>
    ''' <param name="pstrMercado">Indica sobre que grupo de especies buscar (renta variable, renta fija, todas)</param>
    ''' <param name="pstrEstadoEspecie">Indica sobre que grupo de especies buscar (activas, inactivos, todas)</param>
    ''' <param name="pstrAgrupacion">Indica sobre que grupo de especies buscar (todas, etc.)</param>
    ''' <param name="pstrUsuario">Usuario que realiza la operación</param>
    ''' <returns>Lista de especies</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function buscarEspecies(ByVal pstrCondicionFiltro As String, ByVal pstrMercado As String, ByVal pstrEstadoEspecie As String, ByVal pstrAgrupacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Especies(HttpUtility.UrlDecode(pstrCondicionFiltro), pstrMercado, pstrEstadoEspecie, pstrAgrupacion, False, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarEspecies"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, True).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarEspecies")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Buscar las especies que cumplan con los criterios definidos por los parámetros
    ''' </summary>
    ''' <param name="pstrCondicionFiltro">Condición que digita el usuario o que se asigna por programa</param>
    ''' <param name="pstrMercado">Indica sobre que grupo de especies buscar (renta variable, renta fija, todas)</param>
    ''' <param name="pstrEstadoEspecie">Indica sobre que grupo de especies buscar (activas, inactivos, todas)</param>
    ''' <param name="pstrAgrupacion">Indica sobre que grupo de especies buscar (todas, etc.)</param>
    ''' <param name="pstrUsuario">Usuario que realiza la operación</param>
    ''' <returns>Lista de especies</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function buscarEspeciesControl(ByVal pstrCondicionFiltro As String, ByVal pstrMercado As String, ByVal pstrEstadoEspecie As String, ByVal pstrAgrupacion As String, ByVal pstrUsuario As String, ByVal plogTraerEspeciesVencidas As Boolean, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Especies(pstrCondicionFiltro, pstrMercado, pstrEstadoEspecie, pstrAgrupacion, False, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarEspeciesControl"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogTraerEspeciesVencidas).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarEspeciesControl")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Buscar las especies que cumplan con los criterios definidos por los parámetros
    ''' Se adiciona el tipo de negocio para que solo filtre por los tipos de negocio permitidos para negociar la especie.
    ''' </summary>
    ''' <param name="pstrCondicionFiltro">Condición que digita el usuario o que se asigna por programa</param>
    ''' <param name="pstrMercado">Indica sobre que grupo de especies buscar (renta variable, renta fija, todas)</param>
    ''' <param name="pstrEstadoEspecie">Indica sobre que grupo de especies buscar (activas, inactivos, todas)</param>
    ''' <param name="pstrAgrupacion">Indica sobre que grupo de especies buscar (todas, etc.)</param>
    ''' <param name="pstrUsuario">Usuario que realiza la operación</param>
    ''' <returns>Lista de especies</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function buscarEspeciesOyDPLUS(ByVal pstrCondicionFiltro As String, ByVal pstrMercado As String, ByVal pstrEstadoEspecie As String, ByVal pstrAgrupacion As String, ByVal pstrTipoNegocio As String, ByVal pstrTipoProducto As String, ByVal pstrUsuario As String, ByVal plogTraerEspeciesVencidas As Boolean, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Especies(pstrCondicionFiltro, pstrMercado, pstrEstadoEspecie, pstrAgrupacion, True, pstrTipoNegocio, pstrTipoProducto, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarEspeciesOyDPLUS"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogTraerEspeciesVencidas).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarEspeciesOyDPLUS")
            Return Nothing
        End Try
    End Function

    '-- CCM20120108: Incluir la clase para filtrar el nemotécnico (Mercado)
    Public Function buscarNemotecnicoEspecifico(ByVal pstrMercado As String, ByVal pstrNemotecnico As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Especies(RetornarValorDescodificado(pstrNemotecnico), pstrMercado, "", "nemotecnico", False, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarNemotecnicoEspecifico"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, True).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarNemotecnicoEspecifico")
            Return Nothing
        End Try
    End Function

    Public Sub InsertBuscadorEspecies(ByVal objBuscadorEspecies As OYDUtilidades.BuscadorEspecies)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBuscadorEspecies")
        End Try
    End Sub

    Public Sub UpdateBuscadorEspecies(ByVal objBuscadorEspecies As OYDUtilidades.BuscadorEspecies)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBuscadorEspecies")
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los items que cumplan con los criterios definidos por los parámetros
    ''' </summary>
    ''' <param name="pstrCondicionFiltro">Condición que digita el usuario o que se asigna por programa</param>
    ''' <param name="pstrTipoItem">Indica sobre que grupo de datos buscar</param>
    ''' <param name="pstrEstadoItem">Indica sobre que grupo de datos buscar (activas, inactivos, todos)</param>
    ''' <param name="pstrAgrupacion">Indica sobre que grupo de datos buscar (todas, etc.)</param>
    ''' <param name="pstrUsuario">Usuario que realiza la operación</param>
    ''' <returns>Lista de items según condiciones</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function buscarItems(ByVal pstrCondicionFiltro As String, ByVal pstrTipoItem As String, ByVal pstrEstadoItem As String, ByVal pstrAgrupacion As String, ByVal pstrCondicion1 As String, ByVal pstrCondicion2 As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorGenerico)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Generico(HttpUtility.UrlDecode(pstrCondicionFiltro), pstrTipoItem, pstrEstadoItem, pstrAgrupacion, pstrCondicion1, pstrCondicion2, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarItems"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarItems")
            Return Nothing
        End Try
    End Function

    Public Function buscarItemEspecifico(ByVal pstrTipoItem As String, ByVal pstrIdItem As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorGenerico)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_Generico(pstrIdItem, pstrTipoItem, "", "IdItem", Nothing, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarItemEspecifico"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarItemEspecifico")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Buscar los ordenantes de un comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Id del comitente para el cual se deben buscar sus ordenantes</param>
    ''' <param name="pstrUsuario">Usuario que realiza la operación</param>
    ''' <returns>Lista de items según condiciones</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function buscarOrdenantesComitente(ByVal pstrIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorOrdenantes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_ClientesOrdenantes(pstrIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarOrdenantesComitente"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarOrdenantesComitente")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Buscar las cuentas de depósito de un comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Id del comitente para el cual se deben buscar sus ordenantes</param>
    ''' <param name="pstrUsuario">Usuario que realiza la operación</param>
    ''' <returns>Lista de items según condiciones</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function buscarCuentasDepositoComitente(ByVal pstrIdComitente As String, ByVal plogRetornarTodasLosDepositos As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_ClientesCuentasDeposito(pstrIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "buscarCuentasDepositoComitente"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogRetornarTodasLosDepositos).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "buscarCuentasDepositoComitente")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Buscar la fecha de cierre del sistema. La fecha de cierre depende del módulo y usuario
    ''' </summary>
    ''' <param name="pstrModulo">Módulo para el cual se consulta la fecha de cierre</param>
    ''' <param name="pstrUsuario">Usuario que ejecuta las acciones en el módulo y para el cual se verifica la fecha de cierre</param>
    ''' <returns>Fecha de cierre del sistema. Si hay un error retorna Nothing.</returns>
    ''' <remarks></remarks>
    Public Function consultarFechaCierre(ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Date)
        Dim dtmFechaCierre As System.Nullable(Of Date) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Buscador_FechaCierre(pstrModulo, pstrUsuario, dtmFechaCierre, DemeInfoSesion(pstrUsuario, ""), GINT_INICIO_MSGERR_SQL_PERSONALIZADO)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "consultarFechaCierre")
        End Try
        Return dtmFechaCierre
    End Function
    Public Function consultarFechaCierreSync(ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Date)
        Dim objTask As Task(Of System.Nullable(Of Date)) = Me.consultarFechaCierreAsync(pstrModulo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function consultarFechaCierreAsync(ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of System.Nullable(Of Date))
        Dim objTaskComplete As TaskCompletionSource(Of System.Nullable(Of Date)) = New TaskCompletionSource(Of System.Nullable(Of Date))()
        objTaskComplete.TrySetResult(consultarFechaCierre(pstrModulo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Función que permite consultar el ID del consecutivo segun el nombre del consecutivo
    ''' </summary>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="pstrIdOwner"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function consultarConsecutivo(pstrNombreConsecutivo As String, pstrIdOwner As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)
        Dim intIdConsecutivo As System.Nullable(Of Integer) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            intIdConsecutivo = Me.DataContext.ufnOyDNet_Utilidades_IdConsecutivo(pstrNombreConsecutivo, pstrIdOwner)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "consultarConsecutivo")
        End Try
        Return intIdConsecutivo
    End Function


    ''' <summary>
    ''' Permite consultar si se impreme en forma preimpresa o mediante lineas.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PreimpresoOLineas(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Boolean)
        Dim bolPreimpresoOLineas As System.Nullable(Of Boolean) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            bolPreimpresoOLineas = Me.DataContext.ufnOyDNet_Utilidades_PreimpresoOLineas()
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PreimpresoOLineas")
        End Try
        Return bolPreimpresoOLineas
    End Function

    Public Function consultarFechaHabilPosteriorCierreCompania(ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Date)
        Dim dtmFechaHabil As System.Nullable(Of Date) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Compiania_DiaHabilPorterioFechaCierre(pintIDCompania, dtmFechaHabil, pstrUsuario, DemeInfoSesion(pstrUsuario, "consultarFechaHabilPosteriorCierreCompania"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, Nothing)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "consultarFechaHabilPosteriorCierreCompania")
        End Try
        Return dtmFechaHabil
    End Function
    Public Function consultarFechaHabilPosteriorCierreCompaniaSync(ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Date)
        Dim objTask As Task(Of System.Nullable(Of Date)) = Me.consultarFechaHabilPosteriorCierreCompaniaAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function consultarFechaHabilPosteriorCierreCompaniaAsync(ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of System.Nullable(Of Date))
        Dim objTaskComplete As TaskCompletionSource(Of System.Nullable(Of Date)) = New TaskCompletionSource(Of System.Nullable(Of Date))()
        objTaskComplete.TrySetResult(consultarFechaHabilPosteriorCierreCompania(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function consultarFechaCierreCompania(ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Date)
        Dim dtmFechaCierre As System.Nullable(Of Date) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Compiania_DiaHabilPorterioFechaCierre(pintIDCompania, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "consultarFechaHabilPosteriorCierreCompania"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, dtmFechaCierre)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "consultarFechaHabilPosteriorCierreCompania")
        End Try
        Return dtmFechaCierre
    End Function
    Public Function consultarFechaCierreCompaniaSync(ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Date)
        Dim objTask As Task(Of System.Nullable(Of Date)) = Me.consultarFechaHabilPosteriorCierreCompaniaAsync(pintIDCompania, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function consultarFechaCierreCompaniaAsync(ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of System.Nullable(Of Date))
        Dim objTaskComplete As TaskCompletionSource(Of System.Nullable(Of Date)) = New TaskCompletionSource(Of System.Nullable(Of Date))()
        objTaskComplete.TrySetResult(consultarFechaHabilPosteriorCierreCompania(pintIDCompania, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "DTS"

    Public Function DTSFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Utilidades_DTS_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "DTSFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DTSFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DTSConsultar(ByVal pIDDTS As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Utilidades_DTS_Consultar(pIDDTS, DemeInfoSesion(pstrUsuario, "BuscarDTS"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarDTS")
            Return Nothing
        End Try
    End Function

    Public Function TraerDTPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DT
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DT
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IDDTS = 
            'e.Descripcion = 
            'e.NomSP = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.IDDTS = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDTPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDT(ByVal DT As DT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, DT.pstrUsuarioConexion, DT.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DT.InfoSesion = DemeInfoSesion(DT.pstrUsuarioConexion, "InsertDT")
            Me.DataContext.DTS.InsertOnSubmit(DT)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDT")
        End Try
    End Sub

    Public Sub UpdateDT(ByVal currentDT As DT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentDT.pstrUsuarioConexion, currentDT.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDT.InfoSesion = DemeInfoSesion(currentDT.pstrUsuarioConexion, "UpdateDT")
            Me.DataContext.DTS.Attach(currentDT, Me.ChangeSet.GetOriginal(currentDT))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDT")
        End Try
    End Sub

    ''' <summary>
    ''' Recibe una cadena con todos lod usuarios seleccionados para una DTS especifica
    ''' </summary>
    ''' <param name="pintIdDTS">Id de DTS</param>
    ''' <param name="pstrUsuariosConsecutivos">Cadenas concatenando los ususarios</param>
    ''' <param name="pstrUsuario">Usuario que ejecuta el proceso</param>
    ''' <returns>Valor True O False</returns>
    ''' <remarks>Santiago Vergara - Octubre 30/2013</remarks>
    Public Function UsuariosDTS_Actualizar(ByVal pintIdDTS As Integer, ByVal pstrUsuariosConsecutivos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_UsuariosDTS_Actualizar(pintIdDTS, pstrUsuariosConsecutivos, pstrUsuario, DemeInfoSesion(pstrUsuario, "UsuariosDTS_Actualizar"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UsuariosDTS_Actualizar")
            Return False
        End Try
    End Function

#End Region

#Region "Auditoria"

    Public Function AuditoriaFiltrar(ByVal pstrOpcion As String, ByVal pstrNombreTabla As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AuditoriaTabla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            If pstrNombreTabla = String.Empty Then
                Dim ret = Me.DataContext.uspOyDNet_Utilidades_Auditorias_Consulta(pstrOpcion, Nothing, Nothing, Nothing, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "AuditoriaFiltrar"), 0).ToList
                Return ret
            Else
                Dim ret = Me.DataContext.uspOyDNet_Utilidades_Auditorias_Consulta(pstrOpcion, pstrNombreTabla, Nothing, Nothing, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "AuditoriaFiltrar"), 0).ToList
                Return ret
            End If


        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AuditoriaFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function AuditoriaConsultar(ByVal pstrOpcion As String, ByVal pstrNombreTabla As String, ByVal plogIngreso As Boolean,
                                       ByVal plogModificacion As Boolean, ByVal plogEliminacion As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AuditoriaTabla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Utilidades_Auditorias_Consulta(pstrOpcion, pstrNombreTabla, plogIngreso, plogModificacion, plogEliminacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_Auditoria"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AuditoriaConsultar")
            Return Nothing
        End Try
    End Function

    Public Function AuditoriaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As AuditoriaTabla
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New AuditoriaTabla
            e.NombreTabla = String.Empty
            e.Ingreso = False
            e.Modificacion = False
            e.Eliminacion = False
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AuditoriaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertAuditoriaTabla(ByVal Auditori As AuditoriaTabla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Auditori.pstrUsuarioConexion, Auditori.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Auditori.strOpcion = "ING"
            Auditori.InfoSesion = DemeInfoSesion(Auditori.pstrUsuarioConexion, "InsertAuditori")
            Me.DataContext.AuditoriaTablas.InsertOnSubmit(Auditori)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertAuditoriaTabla")
        End Try
    End Sub

    Public Sub UpdateAuditoriaTabla(ByVal currentAuditori As AuditoriaTabla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentAuditori.pstrUsuarioConexion, currentAuditori.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentAuditori.strOpcion = "MOD"
            currentAuditori.InfoSesion = DemeInfoSesion(currentAuditori.pstrUsuarioConexion, "UpdateAuditori")
            Me.DataContext.AuditoriaTablas.Attach(currentAuditori, Me.ChangeSet.GetOriginal(currentAuditori))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateAuditoriaTabla")
        End Try
    End Sub

    Public Sub DeleteAuditoriaTabla(ByVal Auditori As AuditoriaTabla)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Auditori.pstrUsuarioConexion, Auditori.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Utilidades_Auditoria_Eliminar(DemeInfoSesion(pstrUsuario, "DeleteAuditori"),0).ToList
            Auditori.strOpcion = "RET"
            Auditori.InfoSesion = DemeInfoSesion(Auditori.pstrUsuarioConexion, "DeleteAuditori")
            Me.DataContext.AuditoriaTablas.Attach(Auditori)
            Me.DataContext.AuditoriaTablas.DeleteOnSubmit(Auditori)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteAuditoriaTabla")
        End Try
    End Sub

    Public Function cargarCombosAuditoria(ByVal pstrListasCombos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombosAuditoria(pstrListasCombos, pstrUsuario, DemeInfoSesion(pstrUsuario, "cargarCombosAuditoria"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosEspecificos")
            Return Nothing
        End Try
    End Function

    Public Function cargarCombosEspeciesCalculosFinancieros(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos_Especies_Calculos_Financieros()
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosEspeciesCalculosFinancieros")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Utilidades"

    Public Function cargarCombosSistemasCargaArchivos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos_SistemasCargaArchivo()
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosEspeciesCalculosFinancieros")
            Return Nothing
        End Try
    End Function
#End Region

#Region "InformeAuditoria"


    Private _ListaAuditoria As List(Of InformeAuditoria)
    Public Property ListaAuditoria() As List(Of InformeAuditoria)
        Get
            Return _ListaAuditoria
        End Get
        Set(ByVal value As List(Of InformeAuditoria))
            _ListaAuditoria = value
        End Set
    End Property

    Public Function DatosInforme(ByVal pdtmFechainicial As DateTime,
   pdtmFechafin As DateTime,
   pstrTabla As String,
   pstrFiltroClave As String,
   pstrFiltroContenido As String,
   pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Boolean)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.Usp_AuditoriaInforme_OyDNet(pdtmFechainicial, pdtmFechafin,
   pstrTabla, pstrFiltroClave, pstrFiltroContenido, pstrUsuario, DemeInfoSesion(pstrUsuario, "DatosInforme"), 0).ToList

            RETORNO = False
            ListaAuditoria = ret.ToList
            Dim grid As New DataGrid
            grid.AutoGenerateColumns = False
            CrearColumnasAuditoria(grid)
            Dim strMensaje As String = "csv"
            exportDataGrid(grid, strMensaje.ToUpper(), pstrUsuario)

            Return RETORNO

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DatosInforme")
            Return Nothing
        End Try
    End Function
    Private Sub CrearColumnasAuditoria(ByVal pgrid As DataGrid)

        Dim Tabla As New BoundColumn
        Tabla.HeaderText = "Tabla"
        pgrid.Columns.Add(Tabla)

        Dim FechaSuceso As New BoundColumn
        FechaSuceso.HeaderText = "Fecha del suceso"
        pgrid.Columns.Add(FechaSuceso)

        Dim Operacion As New BoundColumn
        Operacion.HeaderText = "Operación"
        pgrid.Columns.Add(Operacion)

        Dim ClaveSuceso As New BoundColumn
        ClaveSuceso.HeaderText = "Clave del suceso"
        pgrid.Columns.Add(ClaveSuceso)

        Dim Datos As New BoundColumn
        Datos.HeaderText = "Datos"
        pgrid.Columns.Add(Datos)

        Dim Usuario As New BoundColumn
        Usuario.HeaderText = "Usuario"
        pgrid.Columns.Add(Usuario)

        Dim Aplicacion As New BoundColumn
        Aplicacion.HeaderText = "Aplicación"
        pgrid.Columns.Add(Aplicacion)

        Dim Maquina As New BoundColumn
        Maquina.HeaderText = "Máquina"
        pgrid.Columns.Add(Maquina)


    End Sub
    Private Sub exportDataGrid(ByVal dGrid As DataGrid, ByVal strFormat As String, ByVal pstrUsuario As String)

        Dim strBuilder As New StringBuilder()
        Dim strLineas As New List(Of String)

        If IsNothing(dGrid) Then Return
        Dim lstFields As List(Of String) = New List(Of String)()

        'SLB20131030 Manejo del separador ","
        lstFields.Add(formatField("sep=,", strFormat, False))
        buildStringOfRow(strBuilder, lstFields, strFormat)
        strLineas.Add(strBuilder.ToString())
        lstFields.Clear()
        strBuilder.Clear()

        'If dGrid.HeaderStyle= Then  'es para saber si es la columna de el header y q le de el formato
        For Each dgcol As DataGridColumn In dGrid.Columns
            lstFields.Add(formatField(dgcol.HeaderText.ToString(), strFormat, True))
        Next
        buildStringOfRow(strBuilder, lstFields, strFormat)
        strLineas.Add(strBuilder.ToString())
        'End If
        dGrid.DataSource = ListaAuditoria

        For a = 0 To ListaAuditoria.Count - 1
            lstFields.Clear()
            strBuilder.Clear()
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaAuditoria(a).Tabla), String.Empty, ListaAuditoria(a).Tabla)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaAuditoria(a).FechaSuceso), String.Empty, ListaAuditoria(a).FechaSuceso)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaAuditoria(a).Operacion), String.Empty, ListaAuditoria(a).Operacion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaAuditoria(a).ClaveSuceso), String.Empty, ListaAuditoria(a).ClaveSuceso)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaAuditoria(a).Datos), String.Empty, ListaAuditoria(a).Datos)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaAuditoria(a).Usuario), String.Empty, ListaAuditoria(a).Usuario)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaAuditoria(a).Aplicacion), String.Empty, ListaAuditoria(a).Aplicacion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaAuditoria(a).Maquina), String.Empty, ListaAuditoria(a).Maquina)), strFormat, False))


            buildStringOfRow(strBuilder, lstFields, strFormat)
            strLineas.Add(strBuilder.ToString())
        Next

        RETORNO = Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_AUDITORIA_TABLAS, pstrUsuario, String.Format("InformeAuditoriatablas{0}.csv", Now.ToString("yyyy-mm-dd")), strLineas)

    End Sub
    Private Shared Function formatField(ByVal data As String, ByVal format As String, ByVal encabezado As Boolean) As String
        Select Case format
            Case "XML"
                Return String.Format("<Cell><Data ss:Type=""String" & """>{0}</Data></Cell>", data)
            Case "CSV"
                If encabezado = True Then
                    Return String.Format("""   {0}   """, data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                Else
                    Return String.Format("""{0}""", data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                End If
        End Select
        Return data

    End Function
    Private Shared Sub buildStringOfRow(ByVal strBuilder As StringBuilder, ByVal lstFields As List(Of String), ByVal strFormat As String)
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
        Select Case strFormat
            Case "XML"
                strBuilder.AppendLine("<Row>")
                strBuilder.AppendLine(String.Join("" & vbCrLf & "", lstFields.ToArray()))
                strBuilder.AppendLine("</Row>")
            Case "CSV"
                strBuilder.AppendLine(String.Join(CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator, lstFields.ToArray()))
                'strBuilder.AppendLine(String.Join(SEPARATOR_FORMAT_CVS, lstFields.ToArray()))
        End Select

    End Sub
    Public Function Guardar_ArchivoServidor(ByVal NombreProceso As String, ByVal pstrUsuario As String, ByVal NombreArchivo As String, ByVal Lista As List(Of String)) As Boolean
        Try
            GuardarArchivo(NombreProceso, pstrUsuario, NombreArchivo, Lista, False)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Guardar_ArchivoServidor")
            Return False
        End Try
    End Function
#End Region

#Region "ParametrosConsola"

    Public Function leerParametrosAppConsola(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Dim lstParametros As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim strTopico As String = "ParametrosAppConsola"

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            '/ Parámetros definidos en la base de datos
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos(strTopico, pstrUsuario)

            lstParametros = ret.ToList ' Retornar los parámetros que vienen de base de datos

            '/ Cadena de conexión a la base de datos de Encuenta
            lstParametros.Add(New OYDUtilidades.ItemCombo With {.Categoria = strTopico, .Descripcion = A2Utilidades.CifrarSL.cifrar(A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)), .ID = "A2Consola_CnxBaseDatos".ToUpper})
            lstParametros.Add(New OYDUtilidades.ItemCombo With {.Categoria = strTopico, .Descripcion = A2Utilidades.CifrarSL.cifrar(My.MySettings.Default.DirectorioArchivosUpload), .ID = "A2Consola_CarpetaUploads".ToUpper})
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "leerParametrosApp")
        End Try

        Return lstParametros
    End Function

    Public Function leerParametrosAppConsolaSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Dim objTask As Task(Of List(Of OYDUtilidades.ItemCombo)) = Me.leerParametrosAppConsolaAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function leerParametrosAppConsolaAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OYDUtilidades.ItemCombo))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OYDUtilidades.ItemCombo)) = New TaskCompletionSource(Of List(Of OYDUtilidades.ItemCombo))()
        objTaskComplete.TrySetResult(leerParametrosAppConsola(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function leerParametrosAppConsolaWPF(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Dim lstParametros As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim strTopico As String = "ParametrosAppConsolaWPF"

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            '/ Parámetros definidos en la base de datos
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos(strTopico, pstrUsuario)

            lstParametros = ret.ToList ' Retornar los parámetros que vienen de base de datos

            '/ Cadena de conexión a la base de datos de Encuenta
            lstParametros.Add(New OYDUtilidades.ItemCombo With {.Categoria = strTopico, .Descripcion = A2Utilidades.CifrarSL.cifrar(A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)), .ID = "A2Consola_CnxBaseDatos".ToUpper})
            lstParametros.Add(New OYDUtilidades.ItemCombo With {.Categoria = strTopico, .Descripcion = A2Utilidades.CifrarSL.cifrar(My.MySettings.Default.DirectorioArchivosUpload), .ID = "A2Consola_CarpetaUploads".ToUpper})

            Dim strRutaDirectorioAssembly As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin")
            Dim objArchivos() As String = Directory.GetFiles(strRutaDirectorioAssembly)
            If Not IsNothing(objArchivos) Then
                For Each Archivo In objArchivos
                    Dim objInfoArchivoCompleta = New FileInfo(Archivo)
                    If objInfoArchivoCompleta.Name.ToLower.Contains("a2") And objInfoArchivoCompleta.Extension.ToLower = ".dll" Then
                        Dim objInfoArchivo As Assembly = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(Archivo))

                        If Not IsNothing(objInfoArchivo) Then
                            lstParametros.Add(New OYDUtilidades.ItemCombo With {.Categoria = "DLLSERVICIORIA", .Descripcion = objInfoArchivo.GetName().Version.ToString, .ID = objInfoArchivoCompleta.Name})
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "leerParametrosAppConsolaWPF")
        End Try

        Return lstParametros
    End Function

    Public Function leerParametrosAppConsolaWPFSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Dim objTask As Task(Of List(Of OYDUtilidades.ItemCombo)) = Me.leerParametrosAppConsolaWPFAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function leerParametrosAppConsolaWPFAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OYDUtilidades.ItemCombo))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OYDUtilidades.ItemCombo)) = New TaskCompletionSource(Of List(Of OYDUtilidades.ItemCombo))()
        objTaskComplete.TrySetResult(leerParametrosAppConsolaWPF(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "NIIF"
    ''' <summary>
    ''' Por: Ricardo Barrientos Pérez
    ''' Método para cargar combos de NIIF
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function cargarCombosNiff(ByVal pstrCargarParametro As String, ByVal pintItems As Int32, ByVal pstrCriterio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDUtilidades.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspOyDNet_NIIF_ConsultarItemsPorEspecie_Cliente(pstrCargarParametro, pintItems, pstrCriterio, pstrUsuario, DemeInfoSesion(pstrUsuario, "cargarCombosNiff"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosNIIF")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar los segmentos de negocio de NIIF
    ''' </summary>
    ''' <param name="pxmlCodigosSegmentos">Lleva el detalle del Grid en XML</param>
    ''' <param name="intIDConcepto">Identifica el código para la tabla tblConceptosNIIF si es cliente o Especie</param>
    ''' <param name="logOperacionesActivo">Identifica si operaciones está activo</param>
    ''' <param name="plogTesoreriaActivo">Identifica si tesoreria está activo</param>
    ''' <param name="pstrSegmentoDefecto">para Guaradar el código por defecto</param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Si es True es porque inserto correctamente el Log</returns>
    ''' <remarks>RBPP20131129</remarks>
    Public Function ActualizarSegmentosNegocios(ByVal pxmlCodigosSegmentos As String, ByVal intIDConcepto As Int32, ByVal logOperacionesActivo As Boolean,
                                                ByVal plogTesoreriaActivo As Boolean, ByVal pstrSegmentoDefecto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Actualizar_SegmentosNegocios(pxmlCodigosSegmentos, intIDConcepto, logOperacionesActivo, plogTesoreriaActivo, pstrSegmentoDefecto, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarSegmentosNegocios"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarSegmentosNegocios")
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Función de carga inicial para la pantalla
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Si es True es porque inserto correctamente el Log</returns>
    ''' <remarks>RBPP20131203</remarks>
    Public Function NIIF_ConsultaInicial(ByVal pintConcepto As Int32, pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of NIIFInicial)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_NIIF_ConsultaInicial(pintConcepto, pstrUsuario, DemeInfoSesion(pstrUsuario, "NIIF_ConsultaInicial"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "NIIF_ConsultaInicial")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Geeración de archivos planos"

    <Query(HasSideEffects:=True)>
    Public Function GenerarArchivoPlano(ByVal pstrCarpetaProceso As String, ByVal pstrProceso As String, ByVal pstrParametros As String, ByVal pstrNombreArchivo As String, ByVal pstrSeparador As String, ByVal pstrExtensionArchivo As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of GenerarArchivosPlanos)
        Dim objRetorno As New List(Of GenerarArchivosPlanos)
        objRetorno.Add(New GenerarArchivosPlanos With {.Mensaje = "", .RutaArchivoPlano = "", .Exitoso = False})

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrCarpetaProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspA2utils_ConsultarGenerarTextos]"
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
            objListaParametros.Add(CrearSQLParameter("pstrNombreProceso", pstrProceso, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrParametrosProceso", pstrParametros, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "GenerarArchivoPlano"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim strArchivo As String = String.Empty

            If pstrExtensionArchivo = TipoExportacion.EXCEL.ToString Then
                If pstrProceso = "GENERARINFOFONDOS" Then
                    strArchivo = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, pstrNombreArchivo, strTipoSeparador)
                Else
                    strArchivo = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, pstrNombreArchivo)
                End If
            ElseIf pstrExtensionArchivo = TipoExportacion.AMBOS.ToString Then
                strArchivo = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, pstrNombreArchivo)
                GenerarTextoPlano(objDatosConsulta, objRutas.RutaArchivosLocal, pstrNombreArchivo, pstrExtensionArchivo, strTipoSeparador)
            ElseIf pstrExtensionArchivo = TipoExportacion.EXCELVIEJO.ToString Then
                If pstrProceso = "GENERARINFOFONDOS" Then
                    strArchivo = GenerarExcelVersionesViejas(objDatosConsulta, objRutas.RutaArchivosLocal, pstrNombreArchivo, strTipoSeparador)
                Else
                    strArchivo = GenerarExcelVersionesViejas(objDatosConsulta, objRutas.RutaArchivosLocal, pstrNombreArchivo)
                End If
            Else
                strArchivo = GenerarTextoPlano(objDatosConsulta, objRutas.RutaArchivosLocal, pstrNombreArchivo, pstrExtensionArchivo, strTipoSeparador)
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            objRetorno.First.NombreArchivoPlano = strArchivo
            objRetorno.First.RutaArchivoPlano = objRutas.RutaCompartidaOWeb() & strArchivo
            objRetorno.First.Exitoso = True
            objRetorno.First.Mensaje = "Generación de archivo exitoso."
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarArchivoPlano")

            objRetorno.First.Exitoso = False
            objRetorno.First.Mensaje = ex.Message
            objRetorno.First.RutaArchivoPlano = String.Empty
            objRetorno.First.NombreArchivoPlano = String.Empty

            Return objRetorno
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function GenerarArchivoPlanoSync(ByVal pstrCarpetaProceso As String, ByVal pstrProceso As String, ByVal pstrParametros As String, ByVal pstrNombreArchivo As String, ByVal pstrSeparador As String, ByVal pstrExtensionArchivo As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of GenerarArchivosPlanos)
        Dim objTask As Task(Of List(Of GenerarArchivosPlanos)) = Me.GenerarArchivoPlanoAsync(pstrCarpetaProceso, pstrProceso, pstrParametros, pstrNombreArchivo, pstrSeparador, pstrExtensionArchivo, pstrMaquina, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarArchivoPlanoAsync(ByVal pstrCarpetaProceso As String, ByVal pstrProceso As String, ByVal pstrParametros As String, ByVal pstrNombreArchivo As String, ByVal pstrSeparador As String, ByVal pstrExtensionArchivo As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarArchivosPlanos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarArchivosPlanos)) = New TaskCompletionSource(Of List(Of GenerarArchivosPlanos))()
        objTaskComplete.TrySetResult(GenerarArchivoPlano(pstrCarpetaProceso, pstrProceso, pstrParametros, pstrNombreArchivo, pstrSeparador, pstrExtensionArchivo, pstrMaquina, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Descargar Archivo Reporte"

    <Query(HasSideEffects:=True)>
    Public Function GenerarArchivoReporte(ByVal pstrRutaServidorReportes As String,
                                          ByVal pstrReporte As String,
                                          ByVal pstrParametrosReporte As String,
                                          ByVal pstrSeparadorParametros As String,
                                          ByVal pstrFormatoReporte As String,
                                          ByVal pstrCarpetaProceso As String,
                                          ByVal pstrNombreArchivo As String,
                                          ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of GenerarArchivosPlanos)
        Dim objRetorno As New List(Of GenerarArchivosPlanos)
        objRetorno.Add(New GenerarArchivosPlanos With {.Mensaje = "", .RutaArchivoPlano = "", .Exitoso = False})

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrCarpetaProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim objGeneracionReporte As New A2.Documentos.clsGenerarDocumento()

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            objGeneracionReporte.generarArchivo(pstrRutaServidorReportes,
                                                pstrReporte,
                                                objRutas.RutaArchivosLocal,
                                                pstrNombreArchivo,
                                                pstrFormatoReporte,
                                                pstrParametrosReporte,
                                                pstrUsuario,
                                                "", pstrSeparadorParametros, True)

            objRetorno.First.NombreArchivoPlano = pstrNombreArchivo
            objRetorno.First.RutaArchivoPlano = objRutas.RutaCompartidaOWeb() & pstrNombreArchivo & "." & pstrFormatoReporte
            objRetorno.First.Exitoso = True
            objRetorno.First.Mensaje = "Generación de archivo exitoso."
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarArchivoPlano")

            objRetorno.First.Exitoso = False
            objRetorno.First.Mensaje = ex.Message
            objRetorno.First.RutaArchivoPlano = String.Empty
            objRetorno.First.NombreArchivoPlano = String.Empty

            Return objRetorno
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function GenerarArchivoReporteSync(ByVal pstrRutaServidorReportes As String,
                                          ByVal pstrReporte As String,
                                          ByVal pstrParametrosReporte As String,
                                          ByVal pstrSeparadorParametros As String,
                                          ByVal pstrFormatoReporte As String,
                                          ByVal pstrCarpetaProceso As String,
                                          ByVal pstrNombreArchivo As String,
                                          ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of GenerarArchivosPlanos)
        Dim objTask As Task(Of List(Of GenerarArchivosPlanos)) = Me.GenerarArchivoReporteAsync(pstrRutaServidorReportes, pstrReporte, pstrParametrosReporte, pstrSeparadorParametros, pstrFormatoReporte, pstrCarpetaProceso, pstrNombreArchivo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarArchivoReporteAsync(ByVal pstrRutaServidorReportes As String,
                                          ByVal pstrReporte As String,
                                          ByVal pstrParametrosReporte As String,
                                          ByVal pstrSeparadorParametros As String,
                                          ByVal pstrFormatoReporte As String,
                                          ByVal pstrCarpetaProceso As String,
                                          ByVal pstrNombreArchivo As String,
                                          ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarArchivosPlanos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarArchivosPlanos)) = New TaskCompletionSource(Of List(Of GenerarArchivosPlanos))()
        objTaskComplete.TrySetResult(GenerarArchivoReporte(pstrRutaServidorReportes, pstrReporte, pstrParametrosReporte, pstrSeparadorParametros, pstrFormatoReporte, pstrCarpetaProceso, pstrNombreArchivo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


#End Region

#Region "Procesos Masivos"
    ''' <summary>
    ''' Función encargada de consultar el tipo de proceso de la pantalla de procesos masivos
    ''' </summary>
    ''' 
    Public Function Procesos_ConsultarTipos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProcesoTipos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Proceso_ConsultarTipos(pstrUsuario, DemeInfoSesion(pstrUsuario, "Procesos_ConsultarTipos"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Procesos_ConsultarTipos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de el proceso del tipo seleccionado
    ''' </summary>

    Public Function Procesos_Consultar(ByVal pintIDTipoProceso As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Proceso)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Proceso_Consultar(pintIDTipoProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "Procesos_Consultar"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Procesos_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de consultar el detalle del proceso seleccionado
    ''' </summary>
    ''' 
    Public Function Procesos_ConsultarDetalle(ByVal pintIDProceso As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProcesoDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Proceso_ConsultarDetalle(pintIDProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "Procesos_ConsultarDetalle"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Procesos_ConsultarDetalle")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de iniciar el proceso. 
    ''' </summary>
    ''' 
    Public Function Procesos_Iniciar(ByVal pintIDTipoProceso As Integer, ByVal pstrUsuario As String, ByVal plogSolicitarFecha As Boolean, ByVal pdtmFechaProceso As Nullable(Of DateTime), ByVal pstrInfoConexion As String) As List(Of ResultadoProceso)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            'JAEZ 20161003
            Me.DataContext.ActualizarModulo(Modulos(pstrUsuario, pstrInfoConexion))

            Dim ret = Me.DataContext.uspOyDNet_Proceso_Iniciar(pintIDTipoProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "Procesos_Iniciar"), GINT_INICIO_MSGERR_SQL_PERSONALIZADO, plogSolicitarFecha, pdtmFechaProceso).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Procesos_Iniciar")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConsecutivoExistente(pstrNombreConsecutivo As String, pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Boolean)
        Dim bolConsultarConsecutivo As System.Nullable(Of Boolean) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.spA2utils_ConsultarConsecutivo(pstrNombreConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConsecutivoExistente"), 0, bolConsultarConsecutivo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConsecutivoExistente")
        End Try
        Return bolConsultarConsecutivo
    End Function

#End Region

#Region "Lista archivos Directorio"

    Public Function ListarArchivosDirectorioEnDirectorio(ByVal pstrDirectorioUploads As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ArchivosDirectorio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ValidarCaracteresInvalidosEnRuta(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_CaracteresInvalidosCarpeta, pstrDirectorioUploads)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutaUploads(pstrDirectorioUploads, My.Settings.DirectorioArchivosUpload)
            Dim archivos As New List(Of ArchivosDirectorio)
            Dim files As ReadOnlyCollection(Of String)
            Dim Contador As Integer = 1

            If Directory.Exists(objDatosRutas.RutaArchivosLocal) Then
                files = My.Computer.FileSystem.GetFiles(objDatosRutas.RutaArchivosLocal, FileIO.SearchOption.SearchTopLevelOnly, "*.*")

                For I = 0 To files.Count - 1
                    Dim f = files(I)
                    Dim fileData As FileInfo = My.Computer.FileSystem.GetFileInfo(f)

                    archivos.Add(New ArchivosDirectorio With {.ID = Contador, .Ruta = f, .Nombre = fileData.Name, .Extension = fileData.Extension, .Bytes = CInt(fileData.Length / 1024),
                                                              .FechaHora = fileData.LastWriteTime, .RutaWeb = objDatosRutas.RutaCompartidaOWeb() & "/" & fileData.Name,
                                                              .Seleccionado = False})
                    Contador += 1
                Next
            End If

            Return archivos.OrderByDescending(Function(i) i.FechaHora).ToList
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Function ListarArchivosDirectorioEnDirectorioSync(ByVal pstrDirectorioUploads As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ArchivosDirectorio)
        Dim objTask As Task(Of List(Of ArchivosDirectorio)) = Me.ListarArchivosDirectorioEnDirectorioAsync(pstrDirectorioUploads, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ListarArchivosDirectorioEnDirectorioAsync(ByVal pstrDirectorioUploads As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ArchivosDirectorio))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ArchivosDirectorio)) = New TaskCompletionSource(Of List(Of ArchivosDirectorio))()
        objTaskComplete.TrySetResult(ListarArchivosDirectorioEnDirectorio(pstrDirectorioUploads, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Sub InsertArchivosDirectorio(ByVal objArchivosDirectorio As OYDUtilidades.ArchivosDirectorio)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertArchivosDirectorio")
        End Try
    End Sub

    Public Sub UpdateArchivosDirectorio(ByVal objArchivosDirectorio As OYDUtilidades.ArchivosDirectorio)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateArchivosDirectorio")
        End Try
    End Sub

    Public Sub DeleteArchivosDirectorio(ByVal objArchivosDirectorio As OYDUtilidades.ArchivosDirectorio)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteArchivosDirectorio")
        End Try
    End Sub

#End Region

#Region "Validaciones GMF"
    <Query(HasSideEffects:=True)>
    Public Function VerificarCobroGMF(ByVal pstrNombreConsecutivo As String, ByVal pintIDBanco As Nullable(Of Integer), ByVal pstrNombreBeneficiario As String, ByVal pstrDocumentoBeneficiario As String, ByVal pstrTipoDocumentoBeneficiario As String, ByVal pstrFormaPago As String, ByVal pstrTipoCheque As String, ByVal pstrCuentaBancaria As String, ByVal pstrDetalleInstruccion As String, ByVal pstrDetalles As String, ByVal pstrTipo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TempRetornoCalculo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strxmlDetTesoreria = HttpUtility.HtmlDecode(pstrDetalles)
            Dim ret = Me.DataContext.uspOyDNet_VerificarCobroGMF(pstrNombreConsecutivo, pintIDBanco, pstrNombreBeneficiario, pstrDocumentoBeneficiario, pstrTipoDocumentoBeneficiario, pstrFormaPago, pstrTipoCheque, pstrCuentaBancaria, pstrDetalleInstruccion, strxmlDetTesoreria, pstrTipo, pstrUsuario, DemeInfoSesion(pstrUsuario, "Procesos_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarCobroGMF")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function VerificarCobroGMFSync(ByVal pstrNombreConsecutivo As String, ByVal pintIDBanco As Nullable(Of Integer), ByVal pstrNombreBeneficiario As String, ByVal pstrDocumentoBeneficiario As String, ByVal pstrTipoDocumentoBeneficiario As String, ByVal pstrFormaPago As String, ByVal pstrTipoCheque As String, ByVal pstrCuentaBancaria As String, ByVal pstrDetalleInstruccion As String, ByVal pstrDetalles As String, ByVal pstrTipo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TempRetornoCalculo)
        Dim objTask As Task(Of List(Of TempRetornoCalculo)) = Me.VerificarCobroGMFAsync(pstrNombreConsecutivo, pintIDBanco, pstrNombreBeneficiario, pstrDocumentoBeneficiario, pstrTipoDocumentoBeneficiario, pstrFormaPago, pstrTipoCheque, pstrCuentaBancaria, pstrDetalleInstruccion, pstrDetalles, pstrTipo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function VerificarCobroGMFAsync(ByVal pstrNombreConsecutivo As String, ByVal pintIDBanco As Nullable(Of Integer), ByVal pstrNombreBeneficiario As String, ByVal pstrDocumentoBeneficiario As String, ByVal pstrTipoDocumentoBeneficiario As String, ByVal pstrFormaPago As String, ByVal pstrTipoCheque As String, ByVal pstrCuentaBancaria As String, ByVal pstrDetalleInstruccion As String, ByVal pstrDetalles As String, ByVal pstrTipo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TempRetornoCalculo))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TempRetornoCalculo)) = New TaskCompletionSource(Of List(Of TempRetornoCalculo))()
        objTaskComplete.TrySetResult(VerificarCobroGMF(pstrNombreConsecutivo, pintIDBanco, pstrNombreBeneficiario, pstrDocumentoBeneficiario, pstrTipoDocumentoBeneficiario, pstrFormaPago, pstrTipoCheque, pstrCuentaBancaria, pstrDetalleInstruccion, pstrDetalles, pstrTipo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


#End Region

#Region "Validacion Fecha cierre"
    'CFMA20180115
    Public Function ValidarFechaCierre(ByVal pstrModulo As String, ByVal pstrAccionCliente As String, ByVal pstrDescripcionModulo As String, ByVal pdtmFechaAValidar As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblvalidarFechaCierre)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Validar_FechaCierre(pstrModulo, pstrAccionCliente, pstrDescripcionModulo, pdtmFechaAValidar, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarFechaCierre"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarFechaCierre")
            Return Nothing
        End Try
    End Function
    Public Function ValidarFechaCierreSync(ByVal pstrModulo As String, ByVal pstrAccionCliente As String, ByVal pstrDescripcionModulo As String, ByVal pdtmFechaAValidar As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblvalidarFechaCierre)
        Dim objTask As Task(Of List(Of tblvalidarFechaCierre)) = Me.ValidarFechaCierreAsync(pstrModulo, pstrAccionCliente, pstrDescripcionModulo, pdtmFechaAValidar, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ValidarFechaCierreAsync(ByVal pstrModulo As String, ByVal pstrAccionCliente As String, ByVal pstrDescripcionModulo As String, ByVal pdtmFechaAValidar As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tblvalidarFechaCierre))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblvalidarFechaCierre)) = New TaskCompletionSource(Of List(Of tblvalidarFechaCierre))()
        objTaskComplete.TrySetResult(ValidarFechaCierre(pstrModulo, pstrAccionCliente, pstrDescripcionModulo, pdtmFechaAValidar, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "PantallasParametrizacion"

#Region "Métodos asincrónicos"

    Public Function PantallasParametrizacion_Consultar(pstrPantalla As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PantallasParametrizacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_PantallasParametrizacion_Consultar(pstrPantalla, pstrUsuario, DemeInfoSesion(pstrUsuario, "PantallasParametrizacion_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PantallasParametrizacion_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function PantallasParametrizacion_ConsultarSync(ByVal pstrPantalla As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PantallasParametrizacion)
        Dim objTask As Task(Of List(Of PantallasParametrizacion)) = Me.PantallasParametrizacion_ConsultarAsync(pstrPantalla, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function PantallasParametrizacion_ConsultarAsync(ByVal pstrPantalla As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PantallasParametrizacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PantallasParametrizacion)) = New TaskCompletionSource(Of List(Of PantallasParametrizacion))()
        objTaskComplete.TrySetResult(PantallasParametrizacion_Consultar(pstrPantalla, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

End Class

