Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwCodificacionContableCCostos
    Inherits Window
    Implements INotifyPropertyChanged
    Dim mdcProxy As CodificacionContableDomainContext

    Public Sub New(strCentroCostoFijo As String)
        Try
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            IsBusy = True
            _strCentroCostoFijo = strCentroCostoFijo
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "cwCodificacionContableValor.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Métodos asincrónicos"

#End Region

#Region "Eventos de Controles"
    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub
#End Region

#Region "Propiedades"
    Private _IsBusy As Boolean = False
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _strCentroCostoFijo As String = String.Empty
    Public Property strCentroCostoFijo As String
        Get
            Return _strCentroCostoFijo
        End Get
        Set(value As String)
            _strCentroCostoFijo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCentroCostoFijo"))
        End Set
    End Property

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    
End Class
