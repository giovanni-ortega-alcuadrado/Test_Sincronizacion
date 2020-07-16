Imports System.Net
Imports System.Security.Cryptography
Imports System.Web
Imports Newtonsoft.Json

Public Module Utilidades



    Public Sub ManejarError(ByVal ex As Exception, ByVal Modulo As String, ByVal Funcion As String)
        'Si hay que generar log, entonces llamo al componente encargado de la generación de lo log
        A2Utilidades.Utilidades.registrarErrores(ex, Modulo, Funcion)
    End Sub

    Public Sub GrabarLog(ByVal Mensaje As String, ByVal Modulo As String, ByVal Funcion As String)
        Dim strSistema As String = "Riesgos"
        Dim strVersion As String = "1.0"
        Dim strFuente As String = "RIA"
        Dim strSufijoArchivo As String = "riaweb"
        Dim strRuta As String = ""

        A2Utilidades.Utilidades.generarLog(Mensaje, strSistema, strVersion, Modulo, Funcion, strFuente, strSufijoArchivo, strRuta)

    End Sub

End Module
