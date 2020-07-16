Imports System.IO
Imports A2Utilidades.Utilidades
Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports A2.OyD.Infraestructura




Module Utilidades

    Public Function GuardarArchivo(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal NombreArchivo As String, ByVal Lineas As IEnumerable(Of String), ByVal UtilizaSaltoLinea As Boolean) As Boolean
        Dim archivoServidor As FileStream = Nothing
        Dim writer As StreamWriter = Nothing

        Try
            Dim objDatosRutas = clsClaseUploads.FnRutasImportaciones(pstrNombreProceso, pstrUsuario)

            If Not System.IO.Directory.Exists(objDatosRutas.RutaArchivosLocal) Then
                System.IO.Directory.CreateDirectory(objDatosRutas.RutaArchivosLocal)
            End If

            archivoServidor = File.Create(objDatosRutas.RutaArchivosLocal + "\" + NombreArchivo)
            writer = New StreamWriter(archivoServidor, Encoding.GetEncoding(1252))

            For Each linea As String In Lineas
                If UtilizaSaltoLinea Then
                    writer.WriteLine(linea)
                Else
                    writer.Write(linea)
                End If
            Next

            writer.Close()
            archivoServidor.Close()

            Return True
        Catch ex As Exception
            If Not archivoServidor Is Nothing Then
                archivoServidor.Close()
            End If
            If Not writer Is Nothing Then
                writer.Close()
            End If
            ManejarError(ex, "Tesoreria", "GuardarArchivo")
            Return False
        End Try
    End Function


End Module
