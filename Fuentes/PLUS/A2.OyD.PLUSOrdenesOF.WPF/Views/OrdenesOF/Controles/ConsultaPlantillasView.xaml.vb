Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class ConsultaPlantillasView
    Inherits Window
    Dim objVMOrdenes As OrdenesOYDPLUSOFViewModel

    Public Sub New(ByVal objViewModel As OrdenesOYDPLUSOFViewModel)
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = objViewModel

            objVMOrdenes = objViewModel

            InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "ConsultaPlantillasView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub ConsultaPlantillasView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not IsNothing(objVMOrdenes) Then
                objVMOrdenes.ConsultarPlantillasOrden()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de plantillas", Me.Name, "", "ConsultaPlantillasView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        'scrollEdicion.MaxWidth = Application.Current.MainWindow.ActualWidth - 100
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Private Sub btnEliminar_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVMOrdenes) Then
            objVMOrdenes.EliminarPlantillasOrden()
        End If
    End Sub

    Private Sub btnCrearOrden_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVMOrdenes) Then
            objVMOrdenes.CrearOrdenAPartirPlantilla(Me)
        End If
    End Sub

    Private Sub btnCancelar_Click_1(sender As Object, e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub

    Private Sub btnConsultar_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVMOrdenes) Then
            objVMOrdenes.ConsultarPlantillasOrden()
        End If
    End Sub
End Class
