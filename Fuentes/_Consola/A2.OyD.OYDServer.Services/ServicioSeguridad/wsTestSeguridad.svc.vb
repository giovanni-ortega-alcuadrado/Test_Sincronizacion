Imports System.ServiceModel.Activation

' NOTE: You can use the "Rename" command on the context menu to change the class name "wsTestSeguridad" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select wsTestSeguridad.svc or wsTestSeguridad.svc.vb at the Solution Explorer and start debugging.
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class wsTestSeguridad
    Implements IwsTestSeguridad

    Public Function ValidacionSeguridad(ByVal pstrUsuario As String, ByVal pstrToken As String) As String Implements IwsTestSeguridad.ValidacionSeguridad
        Try
            Dim strRespuesta As String = A2.OyD.Infraestructura.RealizarValidacionSeguridadUsuario(pstrUsuario, pstrToken, My.Settings.Seguridad_TiempoLlamado)
            Return strRespuesta
        Catch ex As Exception
            Return "Ocurrio un error intentando validar."
        End Try
    End Function

End Class
