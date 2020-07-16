Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl

Partial Public Class NombrePlantillaOYDPLUSView
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
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "NombrePlantillaOYDPLUSView", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub NombrePlantillaOYDPLUSView_Load(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes cruzadas", Me.Name, "", "NombrePlantillaOYDPLUSView_Load", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If

    End Sub

    Private Sub btnCancelar_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = False
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnCancelar_Click_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnGuardar_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.ValidarExistenciaNombrePlantillaOrden(txtNombrePlantilla.Text, Me)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnGuardar_Click_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub
End Class
