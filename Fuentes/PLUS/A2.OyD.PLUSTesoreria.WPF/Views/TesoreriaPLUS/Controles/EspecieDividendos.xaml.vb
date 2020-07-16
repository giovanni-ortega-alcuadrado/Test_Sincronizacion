Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports A2Utilidades.Mensajes

Partial Public Class EspecieDividendos
    Inherits Window
    Implements INotifyPropertyChanged

    Private _EspecieSeleccionada As String = "TODAS"
    Public Property EspecieSeleccionada() As String
        Get
            Return _EspecieSeleccionada
        End Get
        Set(ByVal value As String)
            _EspecieSeleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EspecieSeleccionada"))
        End Set
    End Property

    Private _BorrarEspecie As Boolean
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("BorrarEspecie"))
        End Set
    End Property

    Private _MostrarBusquedaEspecies As Visibility = Visibility.Visible
    Public Property MostrarBusquedaEspecies() As Visibility
        Get
            Return _MostrarBusquedaEspecies
        End Get
        Set(ByVal value As Visibility)
            _MostrarBusquedaEspecies = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarBusquedaEspecies"))
        End Set
    End Property


    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        InitializeComponent()
        Me.DataContext = Me
    End Sub

    Public Sub New(ByVal pstrEspecieSeleccionada As String)
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        InitializeComponent()
        Me.DataContext = Me
        MostrarBusquedaEspecies = Visibility.Collapsed
        EspecieSeleccionada = pstrEspecieSeleccionada
        Me.Title = "Especie seleccionada"
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If String.IsNullOrEmpty(EspecieSeleccionada) Then
            mostrarMensaje("Debe seleccionar al menos una especie.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            Me.DialogResult = True
        End If
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private Sub btnLimpiarEspecie_Click_1(sender As Object, e As RoutedEventArgs)
        If BorrarEspecie Then
            BorrarEspecie = False
        End If

        BorrarEspecie = True

        EspecieSeleccionada = "TODAS"
    End Sub

    Private Sub ctlrEspecies_nemotecnicoAsignado_1(pstrNemotecnico As String, pstrNombreEspecie As String)
        EspecieSeleccionada = pstrNemotecnico
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub
End Class
