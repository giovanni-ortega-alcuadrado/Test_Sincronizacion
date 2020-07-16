Imports A2Utilidades

Public Class Program
    Inherits A2Utilidades.Program

    Friend Const PARAM_MODO_MULTIAPLICACION As String = "MA"
    Friend Const PARAM_SUGERIR_LOGIN As String = "SL"
    Friend Const MODO_MULTIAPLICACION As String = "A2Consola_Multiaplicacion"

    Friend Const NOMBRE_CONSOLA As String = "A2Consola"
    Friend Const TITULO_CONSOLA As String = "Consola de ejecución de aplicaciones"
    Friend Shared VERSION_CONSOLA As String = "(0.0)"

    Friend Shared GSTR_NOMBREPARAMETRO_RPTS As String = "A2VNombreReporte"
    Friend Shared GSTR_NOMBRERECURSO_RPTS As String = "A2VReporte"
    Friend Shared GSTR_MAQUINAUSUARIO As String = "UM"

    Public Shared Multiaplicacion As Boolean = False

    Friend Const VERSION_APLICACION_CLIENTE As String = "VersionAplicacionCliente"

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

    Public Shared ReadOnly Property ValidarVersionAssemblies() As Boolean
        Get
            If Application.Current.Resources.Contains("A2Consola_ValidarVersionAssemblies") Then
                Return CBool(Application.Current.Resources("A2Consola_ValidarVersionAssemblies"))
            Else
                Return (True)
            End If
        End Get
    End Property

End Class