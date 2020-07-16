Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.Globalization
Imports System.ComponentModel
Imports Newtonsoft.Json

Partial Public Class ListaNotificacionesView
    Inherits Window
    Implements INotifyPropertyChanged


#Region "Inicializadores"

    Public Sub New(ByVal pstrListaMensajesMostrar As String)
        Try
            InitializeComponent()

            Dim objListaDescripciones = DetalleInformacionMostrar.DeserializeLista(pstrListaMensajesMostrar)
            ListaMensajesMostrar = objListaDescripciones
            Me.DataContext = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la lista de notificaciones.", _
                                Me.ToString(), "ListaNotificacionesView", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedad Dependencias"

    Private _ListaMensajesMostrar As List(Of DetalleInformacionMostrar)
    Public Property ListaMensajesMostrar() As List(Of DetalleInformacionMostrar)
        Get
            Return _ListaMensajesMostrar
        End Get
        Set(ByVal value As List(Of DetalleInformacionMostrar))
            _ListaMensajesMostrar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaMensajesMostrar"))
        End Set
    End Property

#End Region

    Private Sub btnCerrar_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class