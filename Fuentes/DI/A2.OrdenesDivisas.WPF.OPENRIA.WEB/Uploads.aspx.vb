﻿Imports A2.OyD.OYDServer.RIA.Web
Imports System.IO
Imports A2Utilidades
Imports A2.OyD.Infraestructura

Public Class Uploads
    Inherits System.Web.UI.Page

    Private fileLock As New Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Nombre del archivo 
        Dim fileName As String = String.Empty
        Dim pstrNombreProceso As String = String.Empty
        Dim strUsuario As String = String.Empty

        Try

            'Elimina los archivos viejos del la carpeta uploads
            Try
                EliminarArchivosUploads()
            Catch ex As Exception

            End Try

            If Request.QueryString("nomarchivo") IsNot Nothing Then
                fileName = Request.QueryString("nomarchivo")
            Else
                Return
            End If
            If Request.QueryString("proceso") IsNot Nothing Then
                pstrNombreProceso = Request.QueryString("proceso")
            Else
                Return
            End If
            If Request.QueryString("usuario") IsNot Nothing Then
                strUsuario = Request.QueryString("usuario")
            Else
                Return
            End If
            Dim objDatosRutas = clsClaseUploads.FnRutasImportaciones(pstrNombreProceso, strUsuario)
            Dim strRutaCompleta As String = objDatosRutas.RutaArchivosLocal & "\" & fileName
            If Not Directory.Exists(objDatosRutas.RutaArchivosLocal) Then
                Directory.CreateDirectory(objDatosRutas.RutaArchivosLocal)
            End If

            'JWSJ: Valido si el archivo viene comprimido
            Dim bitArchivoFragmentado As Boolean = False
            If Request.QueryString("fragmentado") IsNot Nothing Then
                bitArchivoFragmentado = Request.QueryString("fragmentado") = "1"
            End If
            If bitArchivoFragmentado Then
                'JWSJ: garantizo que se procese el archivo secuencialmente ya que el proceso de descomprimir puede tomar tiempo
                SyncLock fileLock
                    'valido si se recibe el archivo
                    If Request.Files.Count > 0 AndAlso Request.Files(0).InputStream IsNot Nothing Then
                        'escribo el zip
                        Request.Files(0).SaveAs(strRutaCompleta & ".zip")
                        'descomprimo el zip
                        Using zipMemStream As MemoryStream = A2ZipFiles.DescomprimeArchivo(File.OpenRead(strRutaCompleta & ".zip"))
                            'escribo archivo final
                            Using fs As New FileStream(strRutaCompleta, FileMode.Create)
                                zipMemStream.CopyTo(fs)
                            End Using
                        End Using
                        'elimino el zip
                        File.Delete(strRutaCompleta & ".zip")
                    Else
                        Throw New ArgumentNullException("No Se recibio el archivo: " & fileName)
                    End If
                End SyncLock
            Else
                Using fs As New FileStream(strRutaCompleta, FileMode.Create)
                    Request.InputStream.CopyTo(fs)
                End Using
            End If


        Catch fileExceptionName As IOException
            A2Utilidades.Utilidades.registrarErrores(fileExceptionName, "Page_Load", "Page_Load")
            Throw fileExceptionName
        Catch generatedExceptionName As Exception
            A2Utilidades.Utilidades.registrarErrores(generatedExceptionName, "Page_Load", "Page_Load")
            Throw generatedExceptionName
        End Try
    End Sub

    Private Sub EliminarArchivosUploads()
        Try
            If Not IsNothing(My.Settings.EliminarArchivosUpload) Then
                If My.Settings.EliminarArchivosUpload = "1" Then
                    If Not IsNothing(My.Settings.DirectorioArchivosUpload) Then
                        If Not IsNothing(My.Settings.DiasArchivosUpload) Then
                            Dim NroDias As Integer = CInt(My.Settings.DiasArchivosUpload)
                            'Recorre los archivos que fueron subidos anteriormente y la fecha es menor a la fecha actual menos los días configurados.
                            Dim dtmFecha As DateTime = Now.AddDays(-NroDias)
                            Dim strRutaArchivos As String = Hosting.HostingEnvironment.ApplicationPhysicalPath
                            If Right(strRutaArchivos, 1) <> "\" Then
                                strRutaArchivos = strRutaArchivos & "\"
                            End If

                            strRutaArchivos = strRutaArchivos & My.Settings.DirectorioArchivosUpload

                            If Directory.Exists(strRutaArchivos) Then
                                ValidarDirectorio(strRutaArchivos, strRutaArchivos, dtmFecha)

                                For Each liArchivo As String In Directory.GetFiles(strRutaArchivos)
                                    If VerificarArchivoAEliminar(Path.GetFileName(liArchivo)) Then
                                        Dim dtmFechaArchivo As DateTime = File.GetLastWriteTime(liArchivo)
                                        Dim dtmFechaArchivoFormateada As DateTime = CDate(String.Format("{0}-{1}-{2}", dtmFechaArchivo.Day, dtmFechaArchivo.Month, dtmFechaArchivo.Year))
                                        Dim dtmFechaEliminacionFormateada As DateTime = CDate(String.Format("{0}-{1}-{2}", dtmFecha.Day, dtmFecha.Month, dtmFecha.Year))

                                        If dtmFechaArchivoFormateada < dtmFechaEliminacionFormateada Then
                                            IO.File.Delete(liArchivo)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ValidarDirectorio(ByVal pstrRuta As String, ByVal pstrRutaRaiz As String, ByVal dtmFechaMinima As DateTime)
        For Each liDirectorio As String In System.IO.Directory.GetDirectories(pstrRuta)
            Dim logContinuarEliminacion As Boolean = False

            If Not pstrRuta.Equals(pstrRutaRaiz) Then
                logContinuarEliminacion = True
            Else
                logContinuarEliminacion = VerificarCarpetaAEliminar(liDirectorio)
            End If

            If logContinuarEliminacion Then
                If Directory.GetDirectories(liDirectorio).Count > 0 Then
                    ValidarDirectorio(liDirectorio, String.Empty, dtmFechaMinima)
                End If

                For Each liArchivo As String In Directory.GetFiles(liDirectorio)
                    If VerificarArchivoAEliminar(Path.GetFileName(liArchivo)) Then
                        Dim dtmFechaArchivo As DateTime = File.GetLastWriteTime(liArchivo)
                        Dim dtmFechaArchivoFormateada As DateTime = CDate(String.Format("{0}-{1}-{2}", dtmFechaArchivo.Day, dtmFechaArchivo.Month, dtmFechaArchivo.Year))
                        Dim dtmFechaEliminacionFormateada As DateTime = CDate(String.Format("{0}-{1}-{2}", dtmFechaMinima.Day, dtmFechaMinima.Month, dtmFechaMinima.Year))

                        If dtmFechaArchivoFormateada < dtmFechaEliminacionFormateada Then
                            IO.File.Delete(liArchivo)
                        End If
                    End If
                Next

                If Directory.GetFiles(liDirectorio).Count = 0 Then
                    Directory.Delete(liDirectorio)
                End If
            End If
        Next
    End Sub

    Private Function VerificarCarpetaAEliminar(ByVal pstrNombreDirectorio As String) As Boolean
        Dim logPuedeEliminar As Boolean = True
        Dim strCarpetasExcluir As String = "Formatos"

        If Not String.IsNullOrEmpty(My.Settings.CarpetasExluirEliminarUpload) Then
            strCarpetasExcluir = My.Settings.CarpetasExluirEliminarUpload
        End If

        If Not String.IsNullOrEmpty(pstrNombreDirectorio) Then
            Dim strNombreDirectorioReal As String = String.Empty
            For Each li As String In pstrNombreDirectorio.Split("\")
                strNombreDirectorioReal = li
            Next

            For Each li As String In strCarpetasExcluir.Split("|")
                If li.ToUpper = strNombreDirectorioReal.ToUpper Then
                    logPuedeEliminar = False
                    Exit For
                End If
            Next
        End If

        Return logPuedeEliminar
    End Function

    Private Function VerificarArchivoAEliminar(ByVal pstrNombreArchivo As String) As Boolean
        Dim logPuedeEliminar As Boolean = True
        Dim strArchivoExcluir As String = "FuncionCarpetaUploads.txt|web.config"

        If Not String.IsNullOrEmpty(My.Settings.ArchivosExluirEliminarUpload) Then
            strArchivoExcluir = My.Settings.ArchivosExluirEliminarUpload
        End If

        If Not String.IsNullOrEmpty(pstrNombreArchivo) Then
            For Each li As String In strArchivoExcluir.Split("|")
                If li.ToUpper = pstrNombreArchivo.ToUpper Then
                    logPuedeEliminar = False
                    Exit For
                End If
            Next
        End If

        Return logPuedeEliminar
    End Function

End Class