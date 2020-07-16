Imports A2Utilidades.Mensajes
Imports A2.OYD.OYDServer.RIA.Web
Imports System.Globalization
Imports System.IO
Imports System.Net.Http
Imports A2.OYD.Infraestructura
Imports System.Net.Http.Headers
Imports System.Threading
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.Win32

Partial Public Class ucBtnDialogoImportar
    Inherits UserControl

    Private Shared ReadOnly ProcesoDep As DependencyProperty = DependencyProperty.Register("Proceso", GetType(String), GetType(ucBtnDialogoImportar), New PropertyMetadata("", New PropertyChangedCallback(AddressOf CambioPropiedadDep)))
    Private Shared ReadOnly FiltrosDep As DependencyProperty = DependencyProperty.Register("Filtros", GetType(String), GetType(ucBtnDialogoImportar), New PropertyMetadata("", New PropertyChangedCallback(AddressOf CambioPropiedadDep)))


    Public Sub New()
        InitializeComponent()
        If Program.MostrarMensajeLog.Equals("1") Then A2Utilidades.Mensajes.mostrarMensaje("Ruta Uploads: [" & Program.URLFileUploads & "].", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
        btnSubirArchivo.Content = DescripcionTextoBoton
    End Sub

    Public Event CargarArchivo(sender As OpenFileDialog, e As Stream)
    Public Event CargarArchivoGenerico(sender As ObjetoInformacionArchivo, pProceso As String)
    Public Event SubirArchivo(sender As System.Object, e As System.Windows.RoutedEventArgs)
    Public Event ErrorImportandoArchivo(Metodo As String, objEx As Exception)

    Dim archivo As Stream
    Dim bytes As Byte()
    Private fu As New OpenFileDialog()

    Private STR_MSG_CARGA_EXITOSA As String = "El archivo {0} ha sido cargado correctamente"
    Private STR_MSG_CARGA_ERRONEA As String = "El archivo {0} no pudo ser cargado correctamente."

#Region "Propiedades Publicas"

    ''' <summary>
    ''' Nombre del objeto remoto web que recibe el llamado para grabar el archivo de destino
    ''' </summary>
    ''' <remarks></remarks>
    Private _strURLWebUploadFiles As String = Program.URLFileUploads
    Public Property URLWebUploadFiles() As String
        Get
            Return _strURLWebUploadFiles
        End Get
        Set(ByVal value As String)
            _strURLWebUploadFiles = value
        End Set
    End Property

    ''' <summary>
    ''' Ejemplo "Delimitado por comas|*.csv"
    ''' </summary>
    ''' <remarks></remarks>
    '''Private _strFiltros As String
    Public Property Filtros() As String
        Get
            'Return _strFiltros
            Return CStr(GetValue(FiltrosDep))
        End Get
        Set(ByVal value As String)
            '_strFiltros = value
            SetValue(FiltrosDep, value)
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    '''Private _strProceso As String
    Public Property Proceso() As String
        Get
            'Return _strProceso
            Return CStr(GetValue(ProcesoDep))
        End Get
        Set(ByVal value As String)
            '_strProceso = value
            SetValue(ProcesoDep, value)
        End Set
    End Property

    Public ReadOnly Property ArchivoSeleccionado As String
        Get
            Return Path.GetFileName(fu.FileName)
        End Get
    End Property

    Public ReadOnly Property ArchivoRutaLocal As String
        Get
            Dim strRutaCompleta As String = fu.FileName
            'If AutomationFactory.IsAvailable Then
            '    strRutaCompleta = fu..File.FullName
            'Else
            '    strRutaCompleta = String.Empty
            'End If
            Return strRutaCompleta
        End Get
    End Property

    Public ReadOnly Property RutaArchivoEnServidor() As String
        Get
            Dim strRegreso As String = ""
            'Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Return strRegreso
        End Get
    End Property

    Private _MostrarMensajeExitoso As Boolean = False
    Public Property MostrarMensajeExitoso() As Boolean
        Get
            Return _MostrarMensajeExitoso
        End Get
        Set(ByVal value As Boolean)
            _MostrarMensajeExitoso = value
        End Set
    End Property

    Private _generico As Boolean = False
    Public Property Generico() As Boolean
        Get
            Return _generico
        End Get
        Set(ByVal value As Boolean)
            _generico = value
        End Set
    End Property

    Private _DescripcionTextoBoton As String = "Seleccionar archivo"
    Public Property DescripcionTextoBoton() As String
        Get
            Return _DescripcionTextoBoton
        End Get
        Set(ByVal value As String)
            _DescripcionTextoBoton = value
            If Not String.IsNullOrEmpty(_DescripcionTextoBoton) Then
                btnSubirArchivo.Content = _DescripcionTextoBoton
            End If
        End Set
    End Property


#End Region

    ''' <history>
    ''' Se dispara un evento cuando hay inconsistencias para controlarlo desde las pantallas
    ''' Se cierran los archivos manejados para evitar dejar dichos archivos en uso
    ''' </history>
    Private Async Sub btnSubirArchivo_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Await SeleccionarArchivo(sender, e)
    End Sub

    Public Async Function SeleccionarArchivo(Optional ByVal sender As System.Object = Nothing, Optional ByVal e As System.Windows.RoutedEventArgs = Nothing) As Tasks.Task
        Try
            Dim strActualParaWeb As String = Program.Usuario.Replace("\", "_").Replace(".", "_").Replace("/", "_")
            fu = Nothing
            fu = New OpenFileDialog()
            fu.Filter = Filtros
            If fu.ShowDialog().Value Then

                '*** realizo las validaciones de seguridad ***
                Dim objValidarArchivo As ValidacionesArchivo = A2OpenFileDialog.EsUnArchivoValido(fu)
                If Not objValidarArchivo.EsValido Then
                    MensajesControl(objValidarArchivo.MensajeValidacion)
                    Exit Function
                End If
                '****

                If Not Generico Then
                    BI_ProgresoUpload.BusyContent = "Subiendo archivo"
                    BI_ProgresoUpload.IsBusy = True
                Else
                    If Not IsNothing(sender) Then
                        RaiseEvent SubirArchivo(sender, e)
                    End If
                End If

                Dim strRuta As String = String.Empty

                'comprimo el archivo
                archivo = fu.OpenFile()

                'Se modifica logica para subir archivos para tener la opción mediante el XML de configuraciones de comprimir los archivos o subirlos sin comprimir
                If Program.ComprimirArchivoImportar() Then
                    Dim objArchivo = New MemoryStream
                    Dim objFileInfo As New FileInfo(fu.FileName)
                    A2ZipFiles.ComprimeArchivo(objFileInfo).CopyTo(objArchivo)
                    objArchivo.Position = 0

                    strRuta = String.Format("{0}?nomarchivo={1}&usuario={2}&proceso={3}&fragmentado=1", URLWebUploadFiles, Path.GetFileName(fu.FileName), strActualParaWeb, Proceso)

                    'armo la peticion
                    Dim strUrl As Uri = New Uri(strRuta, UriKind.RelativeOrAbsolute)
                    Dim objPeticion = New HttpRequestMessage()
                    Dim objContenidoPeticion = New MultipartFormDataContent()
                    objContenidoPeticion.Add(New StreamContent(objArchivo), Path.GetFileName(fu.FileName), Path.GetFileName(fu.FileName))
                    objContenidoPeticion.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data")
                    objPeticion.Method = HttpMethod.Post
                    objPeticion.Content = objContenidoPeticion
                    objPeticion.RequestUri = strUrl

                    'envio el Archvo
                    Dim objResponse As HttpResponseMessage
                    Try
                        Dim objHandler As New HttpClientHandler With {.UseDefaultCredentials = True}
                        Using client = New HttpClient(objHandler)
                            client.DefaultRequestHeaders.ExpectContinue = False

                            objResponse = Await client.SendAsync(objPeticion)

                            Dim strRutaCompleta As String = fu.FileName

                            RaiseEvent CargarArchivo(fu, archivo)
                            If Generico Then
                                Dim objArchivoCargado As New ObjetoInformacionArchivo
                                objArchivoCargado.pFile = fu
                                objArchivoCargado.pStream = archivo

                                RaiseEvent CargarArchivoGenerico(objArchivoCargado, Proceso)
                            End If
                            BI_ProgresoUpload.IsBusy = False

                            archivo.Flush()
                            archivo.Close()

                            objArchivo.Flush()
                            objArchivo.Close()

                        End Using
                    Catch ex As IOException
                        Throw New Exception("Ocurrió un problema al leer el archivo: " & ex.ToString())
                    Catch ex As ArgumentNullException
                        Throw New Exception("Ocurrió un problema al procesar el archivo: " & ex.ToString())
                    Catch ex As Exception
                        Throw New Exception("Ocurrió un problema al enviar el archivo: " & ex.ToString())
                    End Try
                Else
                    bytes = New Byte(CInt(archivo.Length - 1)) {}
                    archivo.Read(bytes, 0, bytes.Length)

                    strRuta = String.Format("{0}?nomarchivo={1}&usuario={2}&proceso={3}&fragmentado=0", URLWebUploadFiles, Path.GetFileName(fu.FileName), strActualParaWeb, Proceso)

                    'MessageBox.Show(strRuta.ToString)
                    Dim client As New WebClient()
                    'AddHandler client.WriteStreamClosed, AddressOf TerminaSubir
                    AddHandler client.OpenWriteCompleted, AddressOf TerminaAbrir
                    client.OpenWriteAsync(New Uri(strRuta, UriKind.RelativeOrAbsolute))
                End If
            End If
        Catch fi As IOException

            RaiseEvent ErrorImportandoArchivo("btnSubirArchivo_Click", fi)

            MensajesControl("El archivo no se puede subir esta siendo utilizado por otro proceso, verifique que no este abierto")
        Catch ex As Exception

            RaiseEvent ErrorImportandoArchivo("btnSubirArchivo_Click", ex)

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del cuadro de dialogo de apertura", _
                                 Me.ToString(), "ucBtnDialogoImportar.btnSubirArchivo_Click", Program.TituloSistema, Program.Maquina, ex)

            If Not IsNothing(BI_ProgresoUpload) Then
                If Not Generico Then
                    BI_ProgresoUpload.IsBusy = False
                End If
            End If
        End Try
    End Function

    Private Sub MensajesControl(pstrmensaje As String)
        mostrarMensaje(pstrmensaje, Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        If Not Generico Then
            BI_ProgresoUpload.IsBusy = False
        End If
    End Sub

    ''' <history>
    ''' Se dispara un evento cuando hay inconsistencias para controlarlo desde las pantallas
    ''' </history>
    Private Async Sub ComprimirArchivo()
        Try
            Dim strActualParaWeb As String = Program.Usuario.Replace("\", "_").Replace(".", "_").Replace("/", "_")
            Dim strRuta As String = String.Format("{0}?nomarchivo={1}&usuario={2}&proceso={3}&fragmentado=1", URLWebUploadFiles, Path.GetFileName(fu.FileName), strActualParaWeb, Proceso)
            'comprimo el archivo
            archivo = New MemoryStream
            Dim objFileInfo As New FileInfo(fu.FileName)
            A2ZipFiles.ComprimeArchivo(objFileInfo).CopyTo(archivo)
            archivo.Position = 0

            'armo la peticion
            Dim strUrl As Uri = New Uri(strRuta, UriKind.RelativeOrAbsolute)
            Dim objPeticion = New HttpRequestMessage()
            Dim objContenidoPeticion = New MultipartFormDataContent()
            objContenidoPeticion.Add(New StreamContent(archivo), Path.GetFileName(fu.FileName), Path.GetFileName(fu.FileName))
            objContenidoPeticion.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data")
            objPeticion.Method = HttpMethod.Post
            objPeticion.Content = objContenidoPeticion
            objPeticion.RequestUri = strUrl

            'envio el Archvo
            Dim objResponse As HttpResponseMessage
            Try
                Dim objHandler As New HttpClientHandler With {.UseDefaultCredentials = True}
                Using client = New HttpClient(objHandler)
                    objResponse = Await client.SendAsync(objPeticion)

                    Dim strRutaCompleta As String = fu.FileName

                    RaiseEvent CargarArchivo(fu, archivo)
                    If Generico Then
                        Dim objArchivoCargado As New ObjetoInformacionArchivo
                        objArchivoCargado.pFile = fu
                        objArchivoCargado.pStream = archivo

                        RaiseEvent CargarArchivoGenerico(objArchivoCargado, Proceso)
                    End If
                    BI_ProgresoUpload.IsBusy = False
                End Using
            Catch ex As IOException
                Throw New Exception("Ocurrió un problema al leer el archivo: " & ex.ToString())
            Catch ex As ArgumentNullException
                Throw New Exception("Ocurrió un problema al procesar el archivo: " & ex.ToString())
            Catch ex As Exception
                Throw New Exception("Ocurrió un problema al enviar el archivo: " & ex.ToString())
            End Try
        Catch fi As IOException

            RaiseEvent ErrorImportandoArchivo("ComprimirArchivo", fi)

            MensajesControl("El archivo no se puede subir esta siendo utilizado por otro proceso, verifique que no este abierto")
            BI_ProgresoUpload.IsBusy = False
        Catch ex As Exception

            RaiseEvent ErrorImportandoArchivo("ComprimirArchivo", ex)

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del cuadro de dialogo de apertura", _
                                 Me.ToString(), "ucBtnDialogoImportar.btnSubirArchivo_Click", Program.TituloSistema, Program.Maquina, ex)

            If Not IsNothing(BI_ProgresoUpload) Then
                If Not Generico Then
                    BI_ProgresoUpload.IsBusy = False
                End If
            End If
        End Try
    End Sub

    Private Sub TerminaAbrir(sender As Object, e As OpenWriteCompletedEventArgs)
        Try
            Dim strRutaCompleta As String = fu.FileName

            If e.[Error] Is Nothing Then

                'El Stream ha sido abierto correctamente  
                'Escribe la secuencia de bytes en el Stream abierto  
                Dim stream As Stream = e.Result
                stream.Write(bytes, 0, bytes.Length)
                stream.Flush()
                stream.Close()
                archivo.Flush()
                archivo.Close()

                'Se traslada codigo de Termina subir a metodo ya que el archivo se sube sincronicamente
                RaiseEvent CargarArchivo(fu, archivo)
                If Generico Then
                    Dim objArchivoCargado As New ObjetoInformacionArchivo
                    objArchivoCargado.pFile = fu
                    objArchivoCargado.pStream = archivo

                    RaiseEvent CargarArchivoGenerico(objArchivoCargado, Proceso)
                End If
                If IsNothing(e.Error) Then
                    If _MostrarMensajeExitoso Then
                        A2Utilidades.Mensajes.mostrarMensaje(String.Format(STR_MSG_CARGA_EXITOSA, strRutaCompleta), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, String.Format(STR_MSG_CARGA_ERRONEA, ArchivoSeleccionado), "ucBtnDialogoImportar", "TerminaSubir", Program.TituloSistema, Program.Maquina, e.Error.InnerException)
                End If
                BI_ProgresoUpload.IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, String.Format("El archivo {0} no pudo ser cargado correctamente.", ArchivoSeleccionado), "ucBtnDialogoImportar", "TerminaAbrir", Program.TituloSistema, Program.Maquina, e.Error.InnerException)
            End If
        Catch ex As Exception
            BI_ProgresoUpload.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema el callback TerminaAbrir", _
                                 Me.ToString(), "ucBtnDialogoImportar.TerminaAbrir", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#Region "Callback"

    ''' <summary>
    ''' Procedimiento de Call back que se lanza cuando alguna de las dependency properties se modifica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Shared Sub CambioPropiedadDep(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)

    End Sub

#End Region

End Class

Public Class ObjetoInformacionArchivo
    Public Property pFile As OpenFileDialog
    Public Property pStream As Stream
End Class


