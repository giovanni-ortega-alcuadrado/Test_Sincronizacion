Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class FrontOrdenesPLUSView
    Inherits UserControl
    Dim objVM As FrontOrdenesViewModel

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            Me.DataContext = New FrontOrdenesViewModel
InitializeComponent()

            'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla

            'Me.Width = Application.Current.MainWindow.ActualWidth * 0.97
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "FrontOrdenesPLUS", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub FrontOrdenesPLUS_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            objVM = CType(Me.DataContext, FrontOrdenesViewModel)
            objVM.viewFrontOrdenes = Me

            objVM.CargarControlesPantalla()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de front de ordenes", Me.Name, "", "FrontOrdenesPLUS_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        'Me.Width = Application.Current.MainWindow.ActualWidth * 0.97
    End Sub

End Class
