Public Class RutasArchivosUnificado
    Public Property NombreProceso As String
    Public Property RutaWeb As String
    Public Property RutaArchivoProceso As String
    Public Property RutaArchivoUploadProceso As String
    Public Property RutaArchivosLocal As String
    Public Property MensajeDebbug As String
    Public Property RutaArchivosUpload As String
    Public Property RutaCompartidaUpload As String

    Public Function RutaCompartidaOWeb() As String
        If Not String.IsNullOrEmpty(RutaCompartidaUpload) Then
            If Not RutaCompartidaUpload.EndsWith("\") Then
                RutaCompartidaUpload += "\"
            End If
            Return RutaCompartidaUpload
        Else
            If Not RutaWeb.EndsWith("/") Then
                RutaWeb += "/"
            End If
            Return RutaWeb
        End If
    End Function
End Class