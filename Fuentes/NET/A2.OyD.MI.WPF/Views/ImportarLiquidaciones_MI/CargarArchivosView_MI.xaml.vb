Imports Telerik.Windows.Controls
#Region "Imports"
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Windows.Data
Imports System.Web
Imports OpenRiaServices.DomainServices.Client
Imports A2Utilidades.Mensajes
#End Region

Partial Public Class CargarArchivosView_MI
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Declaraciones"

    Private opcionSeleccionada As String = ""
    Private dcProxy AS ImportacionesDomainContext

#End Region
#Region "Procedimientos"

    Private Sub CargarListaArchivos()
                'ctlCargarArchivos.NombreProceso = _strNombreProceso
        'ctlCargarArchivos.Usuario = Program.Usuario.Replace("\", "_").Replace(".", "_")
        tbUsuario.Text = Program.Usuario
        LayoutRoot.DataContext = Me
        dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_strNombreProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    End Sub

#End Region
#Region "Contructores"

    Public Sub New()
        InitializeComponent()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext
        Else
            'SLB 20121011
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            'dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        Call CargarListaArchivos()


    End Sub

    Public Sub New(ByVal pstrNombreProceso As String)
        InitializeComponent()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext
        Else
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            'dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        _strNombreProceso = pstrNombreProceso
        Call CargarListaArchivos()
    End Sub

#End Region
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

    Private _ListaArchivos As IEnumerable(Of Archivo)
    Public Property ListaArchivos() As IEnumerable(Of Archivo)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As IEnumerable(Of Archivo))
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

    Public ReadOnly Property ArchivoSeleccionado As Archivo
        Get
            Return dg.SelectedItem
        End Get
    End Property

#End Region
#Region "Asincronos"

    Private Sub TerminoTraerArchivosAdjuntos(ByVal lo As LoadOperation(Of Archivo))
        If IsNothing(lo.Error) Then
            ListaArchivos = dcProxy.Archivos
        Else
            'C1.Silverlight.C1MessageBox.Show(lo.Error.Message, "Error", C1.Silverlight.C1MessageBoxButton.OK)
            A2Utilidades.Mensajes.mostrarMensaje(lo.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
    End Sub

    Private Sub TerminoBorrarArchivo(ByVal e As InvokeOperation)
        If Not IsNothing(e.Error) Then
            'C1.Silverlight.C1MessageBox.Show(e.Error.Message, "Error", C1.Silverlight.C1MessageBoxButton.OK)
            A2Utilidades.Mensajes.mostrarMensaje(e.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            e.MarkErrorAsHandled()
        End If
        dcProxy.Archivos.Clear()
        dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_strNombreProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    End Sub

    Private Sub PreguntaBorrarCompleted(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            dcProxy.BorrarArchivo(CType(dg.SelectedItem, Archivo).Nombre, _strNombreProceso, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarArchivo, Nothing)
        End If
    End Sub

#End Region
#Region "Sub y Eventos"

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private Sub Page_TerminoCargarArchivos(ByVal sender As System.Object, ByVal e As System.EventArgs)
        dcProxy.Archivos.Clear()
        dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_strNombreProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    End Sub

    Private Sub BorrarArchivoSeleccionado_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(dg.SelectedItem) Then
            'C1.Silverlight.C1MessageBox.Show("¿Está seguro de borrar el archivo '" & CType(dg.SelectedItem, Archivo).Nombre & "'?", "Borrar", C1.Silverlight.C1MessageBoxButton.YesNo,Program.Usuario, Program.HashConexion, AddressOf PreguntaBorrarCompleted)
            mostrarMensajePregunta("¿Está seguro de borrar el archivo '" & CType(dg.SelectedItem, Archivo).Nombre & "'?", _
                                   Program.TituloSistema, _
                                   "BORRARARCHIVO", _
                                   AddressOf PreguntaBorrarCompleted, False)
        End If
    End Sub

    Private Sub NavegarArchivoSeleccionado_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(dg.SelectedItem) Then
            Dim ruta = CType(dg.SelectedItem, Archivo).RutaWeb
            Program.VisorArchivosWeb_DescargarURL(ruta)
        End If
    End Sub

    'Private Sub ctlCargarArchivos_TerminoCargarArchivos(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    dcProxy.Archivos.Clear()
    '    dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_strNombreProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    'End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.DialogResult = False
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.DialogResult = True
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

#End Region

End Class

#Region "Clases"

Public Class itemCombo
    Public Property ID As String
    Public Property Texto As String
End Class

#End Region