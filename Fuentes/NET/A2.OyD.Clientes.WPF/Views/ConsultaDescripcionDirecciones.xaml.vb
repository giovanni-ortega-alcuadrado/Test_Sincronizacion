Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel

Partial Public Class ConsultaDescripcionDirecciones
    Inherits Window
    Implements INotifyPropertyChanged
    Public Sub New(ByVal listadirecciones As List(Of OyDClientes.ClientesDireccione), ByVal tipo As String)
        Try
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            If tipo = "DE" Then
                listadirec = listadirecciones.Where(Function(li) li.DireccionEnvio = True).ToList
            Else
                listadirec = listadirecciones.Where(Function(li) li.Tipo = tipo).ToList
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recorrer la lista de direcciones", _
                     Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
        Me.Close()
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
        Me.Close()
    End Sub
    Private _listadirec As List(Of OyDClientes.ClientesDireccione)
    Public Property listadirec As List(Of OyDClientes.ClientesDireccione)
        Get
            Return _listadirec
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesDireccione))
            _listadirec = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("listadirec"))
        End Set
    End Property
    Private _Direccion As String
    Public Property Direccion As String
        Get
            Return _Direccion
        End Get
        Set(ByVal value As String)
            _Direccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Direccion"))
        End Set
    End Property

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

