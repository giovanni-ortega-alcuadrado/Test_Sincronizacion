Imports Telerik.Windows.Controls
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports System.Threading.Tasks

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program
    Inherits A2Utilidades.Program

    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS As String = "URLServicioPLUSUtilidades"
    Public Const CLAVE_PARAM_CONEXION_DATOS As String = "A2CONSOLA_CNXBASEDATOS"
    Public Const URL_SERVICIOUPLOADS As String = "URL_SERVICIO_UPLOADS"

    ''----------------------------------------------------------------------------------------------------------------------------
    '' 
    '' Declaración de propiedades
    '' 
    ''----------------------------------------------------------------------------------------------------------------------------

    Public Shared ReadOnly Property ClaveEspecifica(pstrClave As String) As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrClave.ToString()) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrClave.ToString()).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
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

    Public Shared ReadOnly Property RutaServicioUtilidadesOYDPLUS() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS))
            End If
        End Get
    End Property




    ''' <summary>
    ''' Ruta donde se encuentra alojada la pagina para subir los archivos al servidor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property URLFileUploads() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(URL_SERVICIOUPLOADS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(URL_SERVICIOUPLOADS).ToString()
                Else
                    Dim strRuta As String = RutaServicioUtilidadesOYD.Substring(0, RutaServicioUtilidadesOYD.ToLower.ToString().IndexOf("/services")) & "/Uploads.aspx"
                    Return strRuta
                End If
            Else
                Dim strRuta As String = RutaServicioUtilidadesOYD.Substring(0, RutaServicioUtilidadesOYD.ToLower.ToString().IndexOf("/services")) & "/Uploads.aspx"
                Return strRuta
            End If
        End Get
    End Property

    Public Overloads Shared ReadOnly Property Usuario As String
        Get
            Return Program.UsuarioSinDomino
        End Get
    End Property

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    '''
    Friend Shared ReadOnly Property ejecutarAppSegunAmbiente() As Boolean
        Get
            Return (FuncionesCompartidas.ejecutarAppSegunAmbiente())
        End Get
    End Property

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As UtilidadesDomainContext)
        If Not IsNothing(pobjProxy) Then

        End If
    End Sub

End Class