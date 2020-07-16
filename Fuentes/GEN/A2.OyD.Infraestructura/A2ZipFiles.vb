Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip

''' <summary>
''' Clase creada para manejo de archivos en zip
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class A2ZipFiles

    ''' <summary>
    ''' Funcion que convierte un archivo en un zip
    ''' </summary>
    ''' <param name="objArchivo">FileInfo del archivo</param>
    ''' <param name="strPassword">Si debe manejar un password</param>
    ''' <returns>el stream del zip</returns>
    ''' <remarks></remarks>
    Public Shared Function ComprimeArchivo(ByVal objArchivo As FileInfo, Optional ByVal strPassword As String = "") As MemoryStream
        Dim outputMemStream As New MemoryStream()
        Dim zipStream As New ZipOutputStream(outputMemStream)

        Try
            'comprimo archivo
            zipStream.SetLevel(1)
            If Not String.IsNullOrEmpty(strPassword) Then
                zipStream.Password = strPassword
            End If
            Dim newEntry As New ZipEntry(objArchivo.Name)
            newEntry.DateTime = DateTime.Now
            zipStream.PutNextEntry(newEntry)
            Dim buffer As Byte() = New Byte(4095) {}
            objArchivo.OpenRead().CopyTo(zipStream, buffer.Length)
            zipStream.CloseEntry()
            zipStream.IsStreamOwner = False
            zipStream.Close()
            outputMemStream.Position = 0
        Catch ex As Exception
            If zipStream IsNot Nothing Then
                zipStream.IsStreamOwner = False
                zipStream.Close()
            End If
            Throw ex
        End Try
        Return outputMemStream
    End Function

    ''' <summary>
    ''' Funcion que toma el stream de un archivo zip y retorna el archivo descomprimido
    ''' </summary>
    ''' <param name="objArchivo">Archivo ZIP</param>
    ''' <param name="strPassword">Si maneja un password</param>
    ''' <returns>El Stream del archivo</returns>
    ''' <remarks></remarks>
    Public Shared Function DescomprimeArchivo(ByVal objArchivo As FileStream, Optional ByVal strPassword As String = "") As MemoryStream
        Dim outputMemStream As New MemoryStream()
        Dim zipInputStream As New ZipInputStream(objArchivo)
        Try
            If Not String.IsNullOrEmpty(strPassword) Then
                zipInputStream.Password = strPassword
            End If
            Dim zipEntry As ZipEntry = zipInputStream.GetNextEntry()
            Dim entryFileName As String = zipEntry.Name
            Dim buffer As Byte() = New Byte(4095) {}
            zipInputStream.CopyTo(outputMemStream, buffer.Length)
            outputMemStream.Position = 0
        Catch ex As Exception
            Throw ex
        Finally
            If zipInputStream IsNot Nothing Then
                zipInputStream.IsStreamOwner = True
                zipInputStream.Close()
            End If
        End Try
        Return outputMemStream
    End Function

End Class
