#Region "Imports"
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Windows.Data
Imports System.Web
Imports OpenRiaServices.DomainServices.Client
Imports A2Utilidades.Mensajes
#End Region

Partial Public Class ListarArchivosDirectorioView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Declaraciones"

    Private opcionSeleccionada As String = ""
    Private dcProxy As ImportacionesDomainContext

#End Region

    Public Sub New()
        InitializeComponent()
        Call Inicializar()
    End Sub

    Public Sub New(pstrNombreProceso As String)
        InitializeComponent()
        _strNombreProceso = pstrNombreProceso
        Call Inicializar()
    End Sub

    Private Sub Inicializar()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        Call CargarListaArchivos()
    End Sub


    Private Sub CargarListaArchivos()
        tbUsuario.Text = Program.Usuario
        LayoutRoot.DataContext = Me
        myBusyIndicator.IsBusy = True
        dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_strNombreProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    End Sub

    Private Sub BorrarArchivoSeleccionado_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(dg.SelectedItem) Then
            'C1.Silverlight.C1MessageBox.Show("¿Está seguro de borrar el archivo '" & CType(dg.SelectedItem, Archivo).Nombre & "'?", "Borrar", C1.Silverlight.C1MessageBoxButton.YesNo, AddressOf PreguntaBorrarCompleted)
            mostrarMensajePregunta("¿Está seguro de borrar el archivo '" & CType(dg.SelectedItem, Archivo).Nombre & "'?", _
                                   Program.TituloSistema, _
                                   "BORRARARCHIVO", _
                                   AddressOf PreguntaBorrarCompleted, False)
        End If
    End Sub

    Private Sub TerminoTraerArchivosAdjuntos(ByVal lo As LoadOperation(Of OyDImportaciones.Archivo))
        Try
            If IsNothing(lo.Error) Then
                ListaArchivos = dcProxy.Archivos.ToList.OrderByDescending(Function(i) i.FechaHora)
            Else
                'C1.Silverlight.C1MessageBox.Show(lo.Error.Message, "Error", C1.Silverlight.C1MessageBoxButton.OK)
                mostrarMensaje(lo.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
            myBusyIndicator.IsBusy = False
        Catch ex As Exception
            myBusyIndicator.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error en el momento de consultar los documentos.",
                                 Me.ToString(), "TerminoTraerArchivosAdjuntos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub PreguntaBorrarCompleted(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            dcProxy.BorrarArchivo(CType(dg.SelectedItem, Archivo).Nombre, _strNombreProceso, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarArchivo, Nothing)
        End If
    End Sub

    Private Sub TerminoBorrarArchivo(ByVal e As InvokeOperation)
        If Not IsNothing(e.Error) Then
            'C1.Silverlight.C1MessageBox.Show(e.Error.Message, "Error", C1.Silverlight.C1MessageBoxButton.OK)
            mostrarMensaje(e.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            e.MarkErrorAsHandled()
        End If
        dcProxy.Archivos.Clear()
        dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_strNombreProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    End Sub

    'Private Sub NavegarArchivoSeleccionado_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    If Not IsNothing(dg.SelectedItem) Then
    '        Dim ruta = CType(dg.SelectedItem, Archivo).RutaWeb
    '        Dim nombre = CType(dg.SelectedItem, Archivo).Nombre
    '        Dim extension = CType(dg.SelectedItem, Archivo).Extension

    '        'HtmlPage.Window.Navigate(New Uri(ruta), "_blank")
    '        If (Application.Current.IsRunningOutOfBrowser) Then
    '            'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
    '            Dim button As New MyHyperlinkButton
    '            button.NavigateUri = New Uri(ruta)
    '            button.TargetName = "_blank"
    '            button.ClickMe()
    '            'Me.webBrowser.Navigate(New Uri(strRuta & "&rs:Format=CSV"))
    '        Else
    '            HtmlPage.Window.Navigate(New Uri(ruta), "_blank")
    '        End If
    '    End If
    'End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.DialogResult = False
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.DialogResult = True
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'App.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

#Region "Propiedades"


    Private _strNombreProceso As String
    Public Property NombreProceso() As String
        Get
            Return _strNombreProceso
        End Get
        Set(ByVal value As String)
            _strNombreProceso = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreProceso"))
        End Set
    End Property

    Private _ListaArchivos As IEnumerable(Of OyDImportaciones.Archivo)
    Public Property ListaArchivos() As IEnumerable(Of OyDImportaciones.Archivo)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As IEnumerable(Of OyDImportaciones.Archivo))
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

    Public ReadOnly Property ArchivoSeleccionado As OyDImportaciones.Archivo
        Get
            If dg.Items.ItemCount > -1 Then
                Return dg.SelectedItem
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
