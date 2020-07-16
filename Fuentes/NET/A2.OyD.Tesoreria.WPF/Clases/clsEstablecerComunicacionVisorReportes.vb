Imports System.Net
Imports System.Net.Security
Imports System.ServiceModel

Public Class clsEstablecerComunicacionVisorReportes

    Public Shared Sub ServicioGenerales(ByRef pobjProxy As A2VisorReportes.A2.Visor.Servicios.GeneralesClient, ByVal pstrUrlServicio As String)
        Dim logEsHttps As Boolean = False
        If Not String.IsNullOrEmpty(pstrUrlServicio) Then
            If Left(pstrUrlServicio, 5).ToLower = "https" Then
                logEsHttps = True
            End If
        End If

        If logEsHttps Then
            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)
        End If

        Dim objBinding As BasicHttpBinding = New BasicHttpBinding()
        If logEsHttps Then
            objBinding.Security.Mode = BasicHttpSecurityMode.Transport
        Else
            objBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly
        End If
        objBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows
        objBinding.SendTimeout = New TimeSpan(0, 10, 0)
        objBinding.ReceiveTimeout = New TimeSpan(0, 10, 0)
        objBinding.MaxBufferSize = 2147483647
        objBinding.MaxReceivedMessageSize = 2147483647
        Dim objEndPoint As EndpointAddress = New EndpointAddress(pstrUrlServicio)

        pobjProxy = New A2VisorReportes.A2.Visor.Servicios.GeneralesClient(objBinding, objEndPoint)
        pobjProxy.InnerChannel.OperationTimeout = New TimeSpan(0, 30, 0)
    End Sub

End Class
