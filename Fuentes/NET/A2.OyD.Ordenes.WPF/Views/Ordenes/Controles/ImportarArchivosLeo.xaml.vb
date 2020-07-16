Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports A2Utilidades.Mensajes

Partial Public Class ImportarArchivosLeo
    Inherits Window
    Implements INotifyPropertyChanged

    Dim objViewModelOrdenes As ImportarOrdenesLEOViewModel

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pobjViewModelOrdenes As ImportarOrdenesLEOViewModel, ByVal pstrNombreArchivo As String)
        Try
            objViewModelOrdenes = pobjViewModelOrdenes
            NombreArchivo = pstrNombreArchivo

            InitializeComponent()

            objViewModelOrdenes.ViewImportarArchivo = Me
            Me.DataContext = Me

            objViewModelOrdenes.CargarArchivoLEO(_NombreArchivo)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                Me.ToString(), "ImportarArchivoRecibos.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreArchivo"))
        End Set
    End Property

    Private _ListaMensajes As List(Of String) = New List(Of String)
    Public Property ListaMensajes() As List(Of String)
        Get
            Return _ListaMensajes
        End Get
        Set(ByVal value As List(Of String))
            _ListaMensajes = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaMensajes"))
        End Set
    End Property

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

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private Sub btnCerrar_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub
End Class
