Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class CargaMasivaLiquidacionesDisponiblesView
    Inherits UserControl
    Dim objVMOrdenes As CargaMasivaOrdenesOYDPLUSViewModel

    Public Sub New(ByVal pobjVMOrdenes As CargaMasivaOrdenesOYDPLUSViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMOrdenes = pobjVMOrdenes

            If Me.Resources.Contains("VMOrdenes") Then
                Me.Resources.Remove("VMOrdenes")
            End If

            Me.Resources.Add("VMOrdenes", objVMOrdenes)
            InitializeComponent()

            objVMOrdenes.ConsultarCantidadProcesadas(String.Empty)
            ' Me.dg.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOrdenesView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        '  Me.dg.Width = Application.Current.MainWindow.ActualWidth * 0.95
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Private Sub btnConfirmarContinuar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                objVMOrdenes.ConfirmarValoresResultado()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de confirmación.", Me.Name, "btnConfirmarContinuar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnRefrescar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                objVMOrdenes.ConsultarCantidadProcesadas(String.Empty)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los resultados de la importación.", Me.Name, "btnRefrescar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
