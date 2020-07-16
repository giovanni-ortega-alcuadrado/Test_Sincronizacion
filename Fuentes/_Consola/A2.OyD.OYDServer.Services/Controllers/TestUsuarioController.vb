Imports System.Net
Imports System.Web.Http
Imports A2.OyD.Infraestructura

Namespace Controllers
    Public Class TestUsuarioController
        Inherits ApiController

        ' GET: api/TestUsuario
        Public Function GetValues() As String
            Dim strIP As String = ObtenerIP()
            Dim strUsuario As String = ObtenerUsuarioLogueado()
            Dim strRetorno As String = String.Empty

            strRetorno = String.Format("{0} - {1}", strIP, strUsuario)
            Return strRetorno
        End Function

    End Class
End Namespace