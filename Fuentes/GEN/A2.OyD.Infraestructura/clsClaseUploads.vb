Imports System.IO
Imports System.Web

Public Class clsClaseUploadsUnificado
    Shared Function FnRutasImportaciones(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrDirectorioUpload As String, ByVal pstrDirectorioCompartidoUploads As String) As RutasArchivosUnificado

        Dim objDatos As New RutasArchivosUnificado

        Try

            objDatos.NombreProceso = CStr(Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(pstrNombreProceso), "", "\" & pstrNombreProceso))

            objDatos.RutaArchivosUpload = pstrDirectorioUpload

            objDatos.RutaArchivosLocal = Hosting.HostingEnvironment.ApplicationPhysicalPath

            If HttpContext.Current.Request.ServerVariables("https").ToUpper = "ON" Then
                objDatos.RutaWeb = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("/", 8)) & HttpContext.Current.Request.ApplicationPath
            Else
                objDatos.RutaWeb = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("/", 7)) & HttpContext.Current.Request.ApplicationPath
            End If

            If Not objDatos.RutaWeb.EndsWith("/") Then
                objDatos.RutaWeb += "/"
            End If

            objDatos.MensajeDebbug += "rutaWeb='" & objDatos.RutaWeb & "'" & Microsoft.VisualBasic.vbCrLf

            objDatos.RutaArchivoProceso = pstrUsuario.Replace("\", "_").Replace(".", "_") & objDatos.NombreProceso
            objDatos.RutaArchivoUploadProceso = objDatos.RutaArchivosUpload & "\" & objDatos.RutaArchivoProceso
            objDatos.RutaArchivosLocal += objDatos.RutaArchivoUploadProceso

            'Se crea directorio sí no existe
            If Not Directory.Exists(objDatos.RutaArchivosLocal) Then
                Directory.CreateDirectory(objDatos.RutaArchivosLocal)
            End If

            objDatos.MensajeDebbug += "rutaArchivos='" & objDatos.RutaArchivosLocal & "'" & Microsoft.VisualBasic.vbCrLf

            objDatos.RutaWeb += objDatos.RutaArchivosUpload & "/" & pstrUsuario.Replace("\", "_").Replace(".", "_") & objDatos.NombreProceso.Replace("\", "/")

            If Not String.IsNullOrEmpty(pstrDirectorioCompartidoUploads) Then
                objDatos.RutaCompartidaUpload = pstrDirectorioCompartidoUploads

                If Not objDatos.RutaCompartidaUpload.EndsWith("\") Then
                    objDatos.RutaCompartidaUpload += "\"
                End If

                objDatos.RutaCompartidaUpload += pstrUsuario.Replace("\", "_").Replace(".", "_") & objDatos.NombreProceso
            End If

            objDatos.MensajeDebbug += "rutaWeb (Final)='" & objDatos.RutaWeb & "'" & Microsoft.VisualBasic.vbCrLf

            Return objDatos

        Catch ex As Exception
            ManejarError(ex, "clsClaseUploads.FnRutasImportaciones", "FnRutasImportaciones")
            Return Nothing
        End Try

    End Function
    Shared Function FnRutaUploads(ByVal pstrNombreCarpetaInterna As String, ByVal pstrDirectorioUpload As String) As RutasArchivosUnificado
        Dim objDatos As New RutasArchivosUnificado

        Try
            pstrNombreCarpetaInterna = CStr(Microsoft.VisualBasic.IIf(String.IsNullOrEmpty(pstrNombreCarpetaInterna), "", "\" & pstrNombreCarpetaInterna))

            objDatos.RutaArchivosUpload = pstrDirectorioUpload

            objDatos.RutaArchivosLocal = Hosting.HostingEnvironment.ApplicationPhysicalPath

            If HttpContext.Current.Request.ServerVariables("https").ToUpper = "ON" Then
                objDatos.RutaWeb = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("/", 8)) & HttpContext.Current.Request.ApplicationPath
            Else
                objDatos.RutaWeb = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("/", 7)) & HttpContext.Current.Request.ApplicationPath
            End If

            If Not objDatos.RutaWeb.EndsWith("/") Then
                objDatos.RutaWeb += "/"
            End If

            objDatos.RutaArchivosLocal += objDatos.RutaArchivosUpload & pstrNombreCarpetaInterna

            objDatos.RutaWeb += objDatos.RutaArchivosUpload & pstrNombreCarpetaInterna.Replace("\", "/")

            Return objDatos

        Catch ex As Exception
            ManejarError(ex, "clsClaseUploads.FnRutasImportaciones", "FnRutasImportaciones")
            Return Nothing
        End Try
    End Function
End Class
