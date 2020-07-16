#Region "Imports"

Imports System.ComponentModel
Imports System.Windows.Browser
Imports System.Windows.Data
Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.OYDUtilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.IO

#End Region

Partial Public Class ListarArchivosDirectorioInternoUploads
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Declaraciones"

    Private opcionSeleccionada As String = ""
    Private objProxyUtilidades As UtilidadesDomainContext
    Private strRutaArchivos As String
    Private strRutaArchivosWeb As String

#End Region

    Public Sub New()
        Try
            InitializeComponent()
            Call Inicializar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el formulario", _
                                                             Me.ToString(), "ListarArchivosDirectorioView", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub New(ByVal pstrCarteraInternaUploads As String)
        Try
            InitializeComponent()
            RutaInternaUploads = pstrCarteraInternaUploads
            tbCarpetaInterna.Text = RutaInternaUploads
            Call Inicializar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el formulario", _
                                                            Me.ToString(), "ListarArchivosDirectorioView", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub New(ByVal pstrCarteraInternaUploads As String, ByVal plogHabilitarSeleccionArchivo As Boolean, ByVal pstrProceso As String, ByVal pstrExtensionesPermitidas As String)
        Try
            InitializeComponent()
            RutaInternaUploads = pstrCarteraInternaUploads
            tbCarpetaInterna.Text = RutaInternaUploads

            If plogHabilitarSeleccionArchivo Then
                MostrarSeleccionArchivo = Visibility.Visible
                ucBtnDialogoImportarArchivo.Proceso = pstrProceso
                ucBtnDialogoImportarArchivo.Filtros = pstrExtensionesPermitidas
            End If

            Call Inicializar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el formulario", _
                                                            Me.ToString(), "ListarArchivosDirectorioView", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub Inicializar()
        Try
            objProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            Me.DataContext = Me
            Call CargarListaArchivos()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el formulario", _
                                                           Me.ToString(), "Inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub


    Private Sub CargarListaArchivos()
        Try
            IsBusy = True
            objProxyUtilidades.Load(objProxyUtilidades.ListarArchivosDirectorioEnDirectorioQuery(_RutaInternaUploads, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los archivos", _
                                                           Me.ToString(), "CargarListaArchivos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    Private Sub Refrescar_Click(sender As Object, e As RoutedEventArgs)
        Call CargarListaArchivos()
    End Sub

    Private Sub TerminoTraerArchivosAdjuntos(ByVal lo As LoadOperation(Of OYDUtilidades.ArchivosDirectorio))
        Try
            If Not lo.HasError Then
                ListaArchivos = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de archivos", Me.ToString(), "TerminoTraerArchivosAdjuntos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de archivos", Me.ToString(), "TerminoTraerArchivosAdjuntos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Sub NavegarArchivoSeleccionado_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(dg.SelectedItem) Then

                Dim ruta = CType(dg.SelectedItem, OYDUtilidades.ArchivosDirectorio).RutaWeb

                Program.VisorArchivosWeb_DescargarURL(ruta)
                'If (Application.Current.IsRunningOutOfBrowser) Then
                '    'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
                '    Dim button As New MyHyperlinkButton
                '    button.NavigateUri = New Uri(ruta)
                '    button.TargetName = "_blank"
                '    button.ClickMe()
                'Else
                '    HtmlPage.Window.Navigate(New Uri(ruta), "_blank")
                'End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar abrir el archivo", Me.ToString(), "NavegarArchivoSeleccionado_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.DialogResult = False
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If IsNothing(ArchivoSeleccionado) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar un archivo para generar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Me.DialogResult = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar el archivo seleccionado.", Me.ToString(), "OKButton_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Try
            Dim intIDSeleccionado As Integer = CInt(CType(sender, CheckBox).Tag)

            For Each li In ListaArchivos
                If li.ID = intIDSeleccionado Then
                    li.Seleccionado = True
                Else
                    li.Seleccionado = False
                End If
            Next

            chkSeleccionarArchivoLocal.IsChecked = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el archivo.", Me.ToString(), "CheckBox_Checked", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ucBtnDialogoImportar_SubirArchivo(sender As Object, e As RoutedEventArgs)
        'mobjVM.IsBusy = True
    End Sub

    Private Sub ucBtnDialogoImportar_CargarArchivo(sender As ObjetoInformacionArchivo, strProceso As String)
        Try
            Dim objDialog = CType(sender, ObjetoInformacionArchivo)

            If Not IsNothing(objDialog.pFile) Then
                InformacionArchivoLocal = objDialog
                txtArchivoSeleccionadoLocal.Text = System.IO.Path.GetFileName(objDialog.pFile.FileName)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "CargarArchivoGenerico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub chkSeleccionarArchivoLocal_Checked(sender As Object, e As RoutedEventArgs)
        txtArchivoSeleccionadoLocal.IsEnabled = True
        ucBtnDialogoImportarArchivo.IsEnabled = True
        For Each li In ListaArchivos
            li.Seleccionado = False
        Next
    End Sub

    Private Sub chkSeleccionarArchivoLocal_Unchecked(sender As Object, e As RoutedEventArgs)
        txtArchivoSeleccionadoLocal.IsEnabled = False
        ucBtnDialogoImportarArchivo.IsEnabled = False
    End Sub

#Region "Propiedades"

    Private _IsBusy As Boolean = False
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _RutaInternaUploads As String
    Public Property RutaInternaUploads() As String
        Get
            Return _RutaInternaUploads
        End Get
        Set(ByVal value As String)
            _RutaInternaUploads = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RutaInternaUploads"))
        End Set
    End Property

    Private _ListaArchivos As IEnumerable(Of OYDUtilidades.ArchivosDirectorio)
    Public Property ListaArchivos() As IEnumerable(Of OYDUtilidades.ArchivosDirectorio)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As IEnumerable(Of OYDUtilidades.ArchivosDirectorio))
            _ListaArchivos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaArchivosPaged"))
        End Set
    End Property

    Public ReadOnly Property ListaArchivosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaArchivos) Then
                Dim view = New PagedCollectionView(_ListaArchivos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property ArchivoSeleccionado As ResultadoArchivoSeleccionado
        Get

            Dim objArchivoSeleccionado As ResultadoArchivoSeleccionado = Nothing

            If chkSeleccionarArchivoLocal.IsChecked Then
                If Not IsNothing(InformacionArchivoLocal) Then
                    objArchivoSeleccionado = New ResultadoArchivoSeleccionado
                    objArchivoSeleccionado.EsArchivoLocal = True
                    objArchivoSeleccionado.Nombre = System.IO.Path.GetFileName(InformacionArchivoLocal.pFile.FileName)
                    objArchivoSeleccionado.Extension = Path.GetExtension(InformacionArchivoLocal.pFile.FileName)
                    objArchivoSeleccionado.Ruta = String.Empty
                    objArchivoSeleccionado.RutaWeb = String.Empty
                    objArchivoSeleccionado.objStream = InformacionArchivoLocal.pStream
                End If
            Else
                If dg.Items.ItemCount > -1 Then
                    For Each li As OYDUtilidades.ArchivosDirectorio In dg.ItemsSource
                        If li.Seleccionado Then
                            objArchivoSeleccionado = New ResultadoArchivoSeleccionado
                            objArchivoSeleccionado.EsArchivoLocal = False
                            objArchivoSeleccionado.Nombre = li.Nombre
                            objArchivoSeleccionado.Extension = li.Extension
                            objArchivoSeleccionado.Ruta = li.Ruta
                            objArchivoSeleccionado.RutaWeb = li.RutaWeb
                            If Right(RutaInternaUploads, 1) <> "\" Then
                                objArchivoSeleccionado.RutaInterna = RutaInternaUploads & "\" & li.Nombre
                            Else
                                objArchivoSeleccionado.RutaInterna = RutaInternaUploads & li.Nombre
                            End If

                            objArchivoSeleccionado.objStream = Nothing
                            Exit For
                        End If
                    Next
                End If
            End If

            Return objArchivoSeleccionado

        End Get
    End Property

    Private _MostrarSeleccionArchivo As Visibility = Visibility.Collapsed
    Public Property MostrarSeleccionArchivo() As Visibility
        Get
            Return _MostrarSeleccionArchivo
        End Get
        Set(ByVal value As Visibility)
            _MostrarSeleccionArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarSeleccionArchivo"))
        End Set
    End Property

    Private _InformacionArchivoLocal As ObjetoInformacionArchivo
    Public Property InformacionArchivoLocal() As ObjetoInformacionArchivo
        Get
            Return _InformacionArchivoLocal
        End Get
        Set(ByVal value As ObjetoInformacionArchivo)
            _InformacionArchivoLocal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("InformacionArchivoLocal"))
        End Set
    End Property



#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

Public Class ResultadoArchivoSeleccionado
    Public Property EsArchivoLocal As Boolean
    Public Property Nombre As String
    Public Property Extension As String
    Public Property Ruta As String
    Public Property RutaInterna As String
    Public Property RutaWeb As String
    Public Property objStream As Stream
End Class
