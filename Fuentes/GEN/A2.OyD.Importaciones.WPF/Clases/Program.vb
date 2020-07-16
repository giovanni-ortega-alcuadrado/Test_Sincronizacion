Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports System.Globalization
Imports A2.OyD.OYDServer.RIA.Web

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program

    Inherits A2Utilidades.Program

    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_PARAM_RUTA_SERV_IMPORTACION As String = "URLServicioImportaciones"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS As String = "DiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    Friend Const NOMBRE_PARAM_RUTA_SERV_TESORERIA As String = "URLServicioTesoreria"
    Friend Const NOMBRE_PARAM_RUTA_SERV_BOLSA As String = "URLServicioBolsa"
    Friend Const NOMBRE_PARAM_RUTA_SERV_MAESTROS As String = "URLServicioMaestros"
    Friend Const VERSION_APLICACION_CLIENTE As String = "VersionAplicacionCliente"
    Friend Const COLOR_NORMAL_DECEVAL As String = "ColorFilaNormalDeceval"
    Friend Const COLOR_RESALTADO_DECEVAL As String = "ColorFilaResaltadaDeceval"

    Friend Enum TipoServicio
        URLServicioImportaciones
    End Enum

    Public Shared ReadOnly Property URLFileUploads() As String
        Get
            Dim strRuta As String = RutaServicioNegocio.Substring(0, RutaServicioNegocio.ToLower.ToString().IndexOf("/services")) & "/Uploads.aspx"
            Return strRuta
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioUtilidadesOYD() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioNegocio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_IMPORTACION) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_IMPORTACION).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_IMPORTACION))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_IMPORTACION))
            End If
        End Get
    End Property

    ''' <summary>
    ''' Ruta del servicio web RIA que expone la funcionalidad de negocio para un módulo específico del sistema
    ''' </summary>
    ''' 
    Friend Shared ReadOnly Property RutaServicioImportaciones(ByVal pstrTipoServicio As String) As String
        Get
            Return (A2Ctl.FuncionesCompartidas.RutaServicioNegocio(pstrTipoServicio))
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioTesoreria() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_TESORERIA) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_TESORERIA).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioBolsa() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_BOLSA) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_BOLSA).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property


    Public Shared ReadOnly Property RutaServicioImportaciones() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_IMPORTACION) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_IMPORTACION).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioMaestros() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_MAESTROS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    ' Indica la versión o cliente de la aplicación para determinar que opcioines cambian en las pantallas
    Public Shared ReadOnly Property VersionAplicacionCliente() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(VERSION_APLICACION_CLIENTE) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(VERSION_APLICACION_CLIENTE).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Overloads Shared ReadOnly Property Usuario As String
        Get
            If Program.UsaUsuarioSinDomino Then
                Return Program.UsuarioSinDomino
            Else
                Return A2Utilidades.Program.Usuario
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    '''
    Friend Shared ReadOnly Property ejecutarAppSegunAmbiente() As Boolean
        Get
            Return (A2Ctl.FuncionesCompartidas.ejecutarAppSegunAmbiente())
        End Get
    End Property

    Public Shared ReadOnly Property Color_Grid_Normal_Deceval() As String
        Get
            Return RetornarClave(COLOR_NORMAL_DECEVAL)
        End Get
    End Property

    Public Shared ReadOnly Property Color_Grid_Resaltado_Deceval() As String
        Get
            Return RetornarClave(COLOR_RESALTADO_DECEVAL)
        End Get
    End Property

    Private Shared Function RetornarClave(ByVal pstrConstante As String) As String
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrConstante) Then
                Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrConstante).ToString()
            Else
                Return ("")
            End If
        Else
            Return ("")
        End If
    End Function

    Public Shared Function colorFromHex(pstrHex As String) As Color
        Try
            pstrHex = pstrHex.Replace("#", String.Empty)
            Dim r, g, b As Byte

            r = Byte.Parse(pstrHex.Substring(0, 2), NumberStyles.HexNumber)
            g = Byte.Parse(pstrHex.Substring(2, 2), NumberStyles.HexNumber)
            b = Byte.Parse(pstrHex.Substring(4, 2), NumberStyles.HexNumber)
            Return Color.FromArgb(255, r, g, b)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de asignar color desde hexadecimal", Program.TituloSistema, "colorFromHex", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Shared Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String) As String
        Dim objRetorno As String = String.Empty

        If Not String.IsNullOrEmpty(strProgram) Then
            objRetorno = strProgram
        Else
            objRetorno = strRetornoOpcional
        End If

        Return objRetorno
    End Function

    Public Shared Function VerificarRecurso(ByVal pstrNombreRecurso As String, ByVal pstrValor As String) As Boolean
        Dim objResultado As Boolean = False

        If Application.Current.Resources.Contains(pstrNombreRecurso) Then
            If Application.Current.Resources(pstrNombreRecurso).ToString.ToUpper = pstrValor.ToUpper Then
                objResultado = True
            End If
        End If

        Return objResultado
    End Function

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As ImportacionesDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.ImportacionLis.HasChanges Then
                For Each li In pobjProxy.ImportacionLis
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.clsChequesOyDPlus.HasChanges Then
                For Each li In pobjProxy.clsChequesOyDPlus
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.clsTransferenciaOyDPlus.HasChanges Then
                For Each li In pobjProxy.clsTransferenciaOyDPlus
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ArchivoOrdenesLeos.HasChanges Then
                For Each li In pobjProxy.ArchivoOrdenesLeos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ArchivoReposBanReps.HasChanges Then
                For Each li In pobjProxy.ArchivoReposBanReps
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
        End If
    End Sub

End Class
