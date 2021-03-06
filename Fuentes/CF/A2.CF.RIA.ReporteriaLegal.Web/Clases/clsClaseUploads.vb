﻿Imports System.Web
Imports System.IO
Imports A2.OyD.Infraestructura

Public Class clsClaseUploads
#Region "Funciones Generales"
    Shared Function FnRutasImportaciones(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String) As RutasArchivos

        Dim objDatos As New RutasArchivos

        Try

            objDatos.NombreProceso = CStr(IIf(String.IsNullOrEmpty(pstrNombreProceso), "", "\" & pstrNombreProceso))

            objDatos.RutaArchivosUpload = My.Settings.DirectorioArchivosUpload

            objDatos.RutaArchivosLocal = Hosting.HostingEnvironment.ApplicationPhysicalPath

            If HttpContext.Current.Request.ServerVariables("https").ToUpper = "ON" Then
                objDatos.RutaWeb = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("/", 8)) & HttpContext.Current.Request.ApplicationPath
            Else
                objDatos.RutaWeb = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("/", 7)) & HttpContext.Current.Request.ApplicationPath
            End If

            If Not objDatos.RutaWeb.EndsWith("/") Then
                objDatos.RutaWeb += "/"
            End If

            objDatos.MensajeDebbug += "rutaWeb='" & objDatos.RutaWeb & "'" & vbCrLf

            objDatos.RutaArchivoProceso = pstrUsuario.Replace("\", "_").Replace(".", "_") & objDatos.NombreProceso
            objDatos.RutaArchivoUploadProceso = objDatos.RutaArchivosUpload & "\" & objDatos.RutaArchivoProceso
            objDatos.RutaArchivosLocal += objDatos.RutaArchivoUploadProceso

            'Se crea directorio sí no existe
            If Not Directory.Exists(objDatos.RutaArchivosLocal) Then
                Directory.CreateDirectory(objDatos.RutaArchivosLocal)
            End If

            objDatos.MensajeDebbug += "rutaArchivos='" & objDatos.RutaArchivosLocal & "'" & vbCrLf

            objDatos.RutaWeb += objDatos.RutaArchivosUpload & "/" & pstrUsuario.Replace("\", "_").Replace(".", "_") & objDatos.NombreProceso.Replace("\", "/")

            objDatos.MensajeDebbug += "rutaWeb (Final)='" & objDatos.RutaWeb & "'" & vbCrLf

            Return objDatos

        Catch ex As Exception
            ManejarError(ex, "clsClaseUploads.FnRutasImportaciones", "FnRutasImportaciones")
            Return Nothing
        End Try

    End Function
#End Region
End Class
