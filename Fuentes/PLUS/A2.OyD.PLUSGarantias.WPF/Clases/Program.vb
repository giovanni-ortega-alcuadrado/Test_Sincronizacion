Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports A2.OyD.OYDServer.RIA.Web

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program
    Inherits A2Utilidades.Program

    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_PARAM_RUTA_SERV_GARANTIAS As String = "URLServicioPLUSGarantias"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS As String = "URLServicioPLUSUtilidades"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS As String = "DiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    Friend Const VERSION_APLICACION_CLIENTE As String = "VersionAplicacionCliente"
    Friend Const EXPRESIONEMAIL As String = "ExpresionEmail"
    Friend Const SERVICIO_REPORTE As String = "A2VServicioRS"
    Friend Const SERVICIO_VISOR_REPORTE As String = "A2VServicioParam"
    Friend Const SERVICIO_RUTA_GENERACION As String = "A2RutaFisicaGeneracion"
    Friend Const TIME_OUT_REPORTES_EN_MINUTOS As String = "TimeOutReportesEnMinutos"
    Friend Const REPORTE_GARANTIAS_RESUMIDO As String = "OYDPlusReporteGarantiasResumido"
    Friend Const REPORTE_GARANTIAS_DETALLADO As String = "OYDPlusReporteGarantiasDetallado"

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

    Public Shared ReadOnly Property RutaServicioNegocio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_GARANTIAS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_GARANTIAS).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_GARANTIAS))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_GARANTIAS))
            End If
        End Get
    End Property


    'Private _mobjInfoAuditoria As A2.OyD.OYDServer.RIA.Web.Auditoria = New A2.OyD.OYDServer.RIA.Web.Auditoria
    'Public ReadOnly Property InfoAuditoria As A2.OyD.OYDServer.RIA.Web.Auditoria
    '    Get
    '        _mobjInfoAuditoria.Usuario = Program.Usuario
    '        _mobjInfoAuditoria.Maquina = Program.Maquina
    '        _mobjInfoAuditoria.DirIPMaquina = Program.DireccionIP
    '        _mobjInfoAuditoria.Browser = Program.Browser
    '        _mobjInfoAuditoria.ErrorPersonalizado = 170
    '        Return (_mobjInfoAuditoria)
    '    End Get
    'End Property

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
            Return Program.UsuarioSinDomino
        End Get
    End Property


    Public Shared ReadOnly Property RetornarParametroApp(ByVal strParametro As String) As String
        Get
            Return RetornarClave(strParametro)
        End Get
    End Property

    Public Shared Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String)
        Dim objRetorno As String = String.Empty

        If Not String.IsNullOrEmpty(strProgram) Then
            objRetorno = strProgram
        Else
            objRetorno = strRetornoOpcional
        End If

        Return objRetorno
    End Function

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

    'Expresion regular correo electronico JBT20140507
    Public Shared ReadOnly Property ExpresionRegularEmail() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(EXPRESIONEMAIL) Then
                    If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(EXPRESIONEMAIL).ToString() = String.Empty Then
                        'Expresion regular por defecto JBT20140507
                        Return ("^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
                    Else
                        'se realiza el replace debido a que en el web.config cuando un value contiene un valor(,) lo interpreta como dos parametros independientes por eso en la expresion regular se colaca un (#) cuando es (,) y luego se reemplaza.
                        Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(EXPRESIONEMAIL).ToString().Replace("#", ",")
                    End If
                Else
                    'Expresion regular por defecto JBT20140507
                    Return ("^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
                End If
            Else
                'Expresion regular por defecto JBT20140507
                Return ("^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
            End If
        End Get
    End Property

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As OyDPLUSGarantiasDomainContext)
        If Not IsNothing(pobjProxy) Then

        End If
    End Sub

End Class
