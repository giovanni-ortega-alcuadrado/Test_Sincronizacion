Imports System.ComponentModel

Partial Public Class PoPupConfirmarGeneracionProcesosMasivos
    Inherits Window
    Implements INotifyPropertyChanged

    Public Sub New()
        Try
            InitializeComponent()
            Me.DataContext = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

    Private _listaRegistrosProcesar As List(Of ListaConfirmarGeneracionProcesosMasivos)
    Public Property listaRegistrosProcesar() As List(Of ListaConfirmarGeneracionProcesosMasivos)
        Get
            Return _listaRegistrosProcesar
        End Get
        Set(ByVal value As List(Of ListaConfirmarGeneracionProcesosMasivos))
            _listaRegistrosProcesar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("listaRegistrosProcesar"))
        End Set
    End Property

    Private _listaMensajes As List(Of String)
    Public Property listaMensajes() As List(Of String)
        Get
            Return _listaMensajes
        End Get
        Set(ByVal value As List(Of String))
            _listaMensajes = value

            Dim objListaNueva As New List(Of ListaConfirmarGeneracionProcesosMasivos)

            If Not IsNothing(_listaMensajes) Then
                For Each li In _listaMensajes
                    objListaNueva.Add(New ListaConfirmarGeneracionProcesosMasivos With {.Mensaje = li})
                Next
            End If

            listaRegistrosProcesar = objListaNueva

        End Set
    End Property

    Private _MensajeConfirmacion As String
    Public Property MensajeConfirmacion() As String
        Get
            Return _MensajeConfirmacion
        End Get
        Set(ByVal value As String)
            _MensajeConfirmacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MensajeConfirmacion"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class ListaConfirmarGeneracionProcesosMasivos
    Implements INotifyPropertyChanged

    Private _Mensaje As String
    Public Property Mensaje() As String
        Get
            Return _Mensaje
        End Get
        Set(ByVal value As String)
            _Mensaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Mensaje"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class


