Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class CW_TreeView
    Inherits Window
    Implements INotifyPropertyChanged

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            If Not Program.IsDesignMode() Then
                InitializeComponent()
                LayoutRoot.DataContext = Me
            End If
        Catch
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        If Not Selected = 0 Then
            Me.DialogResult = True
        Else
            'MessageBox.Show("Debe seleccionar un Nivel")
            mostrarMensaje("Debe seleccionar un Nivel", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
        'Messenger.Default.Send(New clsCategoria With {.Texto = TextBlock1.Text, .Catego = Catego})
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
        'DataForm1.CancelEdit()
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#Region "Metodos"

 

#End Region

    Private _ListaNodos As New ObservableCollection(Of Nivel)
    Public Property ListaNodos As ObservableCollection(Of Nivel)
        Get
            Return _ListaNodos
        End Get
        Set(ByVal value As ObservableCollection(Of Nivel))
            _ListaNodos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaNodos"))
        End Set
    End Property

    Private _Selected As Integer
    Public Property Selected As Integer
        Get
            Return _Selected
        End Get
        Set(ByVal value As Integer)
            _Selected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Selected"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property



    Private Sub CambioSelected(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)

        Dim itemSelected As Nivel = NivelesTreeView.SelectedItem
        If Not IsNothing(itemSelected) Then
            Selected = itemSelected.intId
            Descripcion = itemSelected.strNivel
        End If
     
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

End Class


Public Class Nivel
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Public Sub New(ByVal pintId As Integer, ByVal pstrNivel As String, ByVal ParamArray pListaNiveles As Nivel())
        Dim ObsCollection As New ObservableCollection(Of Nivel)()
        For Each item In pListaNiveles
            ObsCollection.Add(item)
        Next
        Items = ObsCollection
        strNivel = pstrNivel
        intId = pintId
        FirePropertyChanged("Items")
    End Sub

    Private _intId As Integer
    Public Property intId As Integer
        Get
            Return _intId
        End Get
        Set(ByVal value As Integer)
            _intId = value
        End Set
    End Property

    Private _strNivel As String
    Public Property strNivel() As String
        Get
            Return _strNivel
        End Get
        Set(ByVal value As String)
            _strNivel = value
        End Set
    End Property

    Private _Items As ObservableCollection(Of Nivel)
    Public Property Items() As ObservableCollection(Of Nivel)
        Get
            Return _Items
        End Get
        Set(ByVal value As ObservableCollection(Of Nivel))
            _Items = value
        End Set
    End Property

    Private Sub FirePropertyChanged(ByVal [property] As String)
        If Not String.IsNullOrEmpty([property]) Then
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs([property]))
        End If
    End Sub

End Class
