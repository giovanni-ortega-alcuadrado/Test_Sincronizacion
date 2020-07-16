Imports System.Threading
Imports A2.OyD.Infraestructura

Public Class A2SBMessages

    Public Shared Function EnviarMensajeConRespuesta(Of ResponseSB)(ByVal strSistemaOrigen As String, ByVal strSistemaDestino As String, _
                                                      ByVal strAccion As String, ByVal strIDRegistro As String, ByVal strMensaje As String) As ResponseSB
        Dim Conversation As Guid?
        Dim ctx As OyDPLUSOrdenesDerivadosDataContext = New OyDPLUSOrdenesDerivadosDataContext(Utilidades.ObtenerCadenaConexionUtilidades())
        ctx.uspSSBEnviarMensaje(FormatearMensaje(strSistemaOrigen, strSistemaDestino, strAccion, strIDRegistro, strMensaje), Conversation)
        If Conversation.HasValue Then
            Try
                Thread.Sleep(2000)
                Dim QNMensajeRespuesta = New A2SBQueryNotifications(Of ResponseSB)
                QNMensajeRespuesta.GetResponse(Conversation.Value)
                If Not QNMensajeRespuesta.HasErrors Then
                    Return QNMensajeRespuesta.Result
                Else
                    ManejarError(QNMensajeRespuesta.Exception, "EnviarMensajeConRespuesta")
                    Return Nothing
                End If
            Catch ex As Exception
                ManejarError(ex, "EnviarMensajeConRespuesta")
                Return Nothing
            End Try
        Else
            ManejarError(New Exception("Conversacion no iniciada!"), "EnviarMensajeConRespuesta")
            Return Nothing
        End If
    End Function

    Public Shared Function EnviarMensaje(ByVal strSistemaOrigen As String, ByVal strSistemaDestino As String, _
                                                      ByVal strAccion As String, ByVal strIDRegistro As String, ByVal strMensaje As String) As Guid
        Dim Conversation As Guid?
        Dim ctx As OyDPLUSOrdenesDerivadosDataContext = New OyDPLUSOrdenesDerivadosDataContext(Utilidades.ObtenerCadenaConexionUtilidades())
        ctx.uspSSBEnviarMensaje(FormatearMensaje(strSistemaOrigen, strSistemaDestino, strAccion, strIDRegistro, strMensaje), Conversation)
        If Conversation.HasValue Then
            Return Conversation
        Else
            ManejarError(New Exception("Conversacion no iniciada!"), "EnviarMensaje")
            Return Nothing
        End If
    End Function

    Public Shared Function FormatearMensaje(ByVal strSistemaOrigen As String, ByVal strSistemaDestino As String, _
                                                      ByVal strAccion As String, ByVal strIDRegistro As String, ByVal strMensaje As String) As String
        Return String.Format("<MensajeSolicitud><SistemaOrigen>{0}</SistemaOrigen><SistemaDestino>{1}</SistemaDestino>" & _
                             "<Accion>{2}</Accion><IDRegistro>{3}</IDRegistro><Data>{4}</Data></MensajeSolicitud>", strSistemaOrigen, strSistemaDestino, strAccion, strIDRegistro, strMensaje)
    End Function

    Public Shared Function FormatearMensaje(ByVal strMensaje As String) As String
        Dim xmlDocumento As XDocument = XDocument.Parse(strMensaje)
        'Valida sí tiene un error en el sobre
        If Not IsNothing(xmlDocumento.Descendants("Error")) Then
            If Not IsNothing(xmlDocumento.Descendants("Error").FirstOrDefault) Then
                If xmlDocumento.Descendants("Error").FirstOrDefault().Value.Length > 0 Then
                    ManejarError(New Exception(xmlDocumento.Descendants("Error").FirstOrDefault().Value.ToString()), "FormatearMensaje")
                    Return Nothing
                End If
            End If
        End If

        Dim strRetorno As String = "<?xml version=""1.0"" encoding=""Windows-1252""?>" & xmlDocumento.Descendants("Data").FirstOrDefault().ToString()

        Return strRetorno
    End Function

    Private Shared Sub ManejarError(ByVal exception As Exception, ByVal pstrFuncion As String)
        Debug.WriteLine(exception.ToString())
        A2.OyD.Infraestructura.ManejarError(exception, "A2SBMessages", pstrFuncion)
    End Sub

End Class
