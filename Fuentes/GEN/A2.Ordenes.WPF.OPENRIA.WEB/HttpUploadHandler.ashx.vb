Imports System
Imports System.Web
Imports System.IO
Imports System.Web.Hosting
Imports System.Diagnostics
Imports System.Web.Services
Imports A2Utilidades.Utilidades

Public Class HttpUploadHandler
    Implements System.Web.IHttpHandler

    Private _httpContext As HttpContext
    Private _tempExtension As String = "_temp"
    Private _fileName As String
    Private _parameters As String
    Private _lastChunk As Boolean
    Private _firstChunk As Boolean
    Private _startByte As Long
    Private _strnomProc As String
    Private _strUsuario As String

    Private _debugFileStreamWriter As StreamWriter
    Private _debugListener As TextWriterTraceListener

    ''' <summary>
    ''' Start method
    ''' </summary>
    ''' <param name="context"></param>
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        _httpContext = context

        A2Utilidades.Utilidades.registrarLog("Iniciando upload", "", "ProcessRequest", "")
        A2Utilidades.Utilidades.registrarLog("Usuario activo: ", context.User.Identity.Name, "ProcessRequest", "")
        A2Utilidades.Utilidades.registrarLog("Upload folder: ", GetUploadFolder(), "ProcessRequest", "")
        A2Utilidades.Utilidades.registrarLog("Path: ", Hosting.HostingEnvironment.ApplicationPhysicalPath, "ProcessRequest", "")

        If context.Request.InputStream.Length = 0 Then
            Throw New ArgumentException("No se recibió archivo para procesar")
        End If

        Try
            GetQueryStringParameters()

            Dim uploadFolder As String = GetUploadFolder().Trim()
            'uploadFolder = context.Request.QueryString("param")

            Dim rutaArchivos As String = Hosting.HostingEnvironment.ApplicationPhysicalPath
            rutaArchivos += uploadFolder & IIf(Right(uploadFolder, 1) = "\", "", "\") & _strUsuario & _strnomProc

            If Not Directory.Exists(rutaArchivos) Then
                Directory.CreateDirectory(rutaArchivos)
            End If

            'uploadFolder = My.Settings.DirectorioArchivosUpload
            uploadFolder = rutaArchivos

            A2Utilidades.Utilidades.registrarLog("Ruta upload", uploadFolder, "ProcessRequest", "")

            Dim tempFileName As String = _fileName + _tempExtension

            'Is it the first chunk? Prepare by deleting any existing files with the same name
            If _firstChunk Then
                Debug.WriteLine("First chunk arrived at webservice")

                'Delete temp file
                If File.Exists(uploadFolder + "/" + tempFileName) Then
                    File.Delete(uploadFolder + "/" + tempFileName)
                End If

                'Delete target file
                'rutaArchivos
                'If File.Exists(rutaArchivos + "/" + _fileName) Then
                '    File.Delete(rutaArchivos + "/" + _fileName)
                'End If

                If File.Exists(uploadFolder + "/" + _fileName) Then
                    File.Delete(uploadFolder + "/" + _fileName)
                End If
            End If

            'Write the file
            A2Utilidades.Utilidades.registrarLog("Guardar archivo: ", tempFileName, "ProcessRequest", "")

            Using fs As FileStream = File.Open(uploadFolder + "/" + tempFileName, FileMode.Append)
                SaveFile(context.Request.InputStream, fs)
                fs.Close()
            End Using

            A2Utilidades.Utilidades.registrarLog("Guardó archivo: ", tempFileName, "ProcessRequest", "")

            'Is it the last chunk? Then finish up...
            If _lastChunk Then
                A2Utilidades.Utilidades.registrarLog("Último segmento upload: ", tempFileName, "ProcessRequest", "")

                'Rename file to original file
                File.Move(uploadFolder + "/" + tempFileName, uploadFolder + "/" + _fileName)

                A2Utilidades.Utilidades.registrarLog("Archivo temporal renombrado: ", _fileName, "ProcessRequest", "")

                'Finish stuff....
                FinishedFileUpload(_fileName, _parameters)
            End If
        Catch e As Exception
            A2Utilidades.Utilidades.registrarErrores(e, "HttpUploadHandler.ashx", "ProcessRequest: " & _fileName, False)

            Throw
        Finally

        End Try
    End Sub

    ''' <summary>
    ''' Get the querystring parameters
    ''' </summary>
    Private Sub GetQueryStringParameters()
        _fileName = _httpContext.Request.QueryString("file")
        _parameters = _httpContext.Request.QueryString("param")
        _lastChunk = If(String.IsNullOrEmpty(_httpContext.Request.QueryString("last")), True, Boolean.Parse(_httpContext.Request.QueryString("last")))
        _firstChunk = If(String.IsNullOrEmpty(_httpContext.Request.QueryString("first")), True, Boolean.Parse(_httpContext.Request.QueryString("first")))
        _startByte = If(String.IsNullOrEmpty(_httpContext.Request.QueryString("offset")), 0, Long.Parse(_httpContext.Request.QueryString("offset")))
        _strnomProc = If(String.IsNullOrEmpty(_httpContext.Request.QueryString("nomproc")), "", "\" & _httpContext.Request.QueryString("nomproc"))
        _strUsuario = If(String.IsNullOrEmpty(_httpContext.Request.QueryString("strusr")), "", "\" & _httpContext.Request.QueryString("strusr"))
    End Sub

    ''' <summary>
    ''' Save the contents of the Stream to a file
    ''' </summary>
    ''' <param name="stream"></param>
    ''' <param name="fs"></param>
    Private Sub SaveFile(ByVal stream As Stream, ByVal fs As FileStream)
        Dim buffer As Byte() = New Byte(4096) {}
        Dim bytesRead As Integer
        bytesRead = stream.Read(buffer, 0, buffer.Length)
        While bytesRead <> 0
            fs.Write(buffer, 0, bytesRead)
            bytesRead = stream.Read(buffer, 0, buffer.Length)
        End While
    End Sub

    ''' <summary>
    ''' Do your own stuff here when the file is finished uploading
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="parameters"></param>
    Protected Overridable Sub FinishedFileUpload(ByVal fileName As String, ByVal parameters As String)
    End Sub

    Protected Overridable Function GetUploadFolder() As String
        Dim folder As String = My.Settings.DirectorioArchivosUpload
        If String.IsNullOrEmpty(folder) Then
            folder = "Uploads"
        End If
        Return folder
    End Function

    ''' <summary>
    ''' Write debug output to a textfile in debug mode
    ''' </summary>
    <Conditional("DEBUG")> _
    Private Sub StartDebugListener()
        Try
            _debugFileStreamWriter = System.IO.File.AppendText("debug.txt")
            _debugListener = New TextWriterTraceListener(_debugFileStreamWriter)
            Debug.Listeners.Add(_debugListener)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Clean up the debug listener
    ''' </summary>
    <Conditional("DEBUG")> _
    Private Sub StopDebugListener()
        Try
            Debug.Flush()
            _debugFileStreamWriter.Close()
            Debug.Listeners.Remove(_debugListener)
        Catch
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class