Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class CargaMasivaTeImportarResultadoView
    Inherits UserControl
    Dim objVMTesoreria As CargaMasivaTesoreriaViewModel

    Public Sub New(ByVal pobjVMTesoreria As CargaMasivaTesoreriaViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMTesoreria = pobjVMTesoreria

            If Me.Resources.Contains("VMTesoreria") Then
                Me.Resources.Remove("VMTesoreria")
            End If

            Me.Resources.Add("VMTesoreria", objVMTesoreria)
            InitializeComponent()

            objVMTesoreria.ConsultarCantidadProcesadas(String.Empty)
            Me.dg.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOrdenesView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.dg.Width = Application.Current.MainWindow.ActualWidth * 0.95
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
            If Not IsNothing(objVMTesoreria) Then
                objVMTesoreria.ConfirmarValoresResultado()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de confirmación.", Me.Name, "btnConfirmarContinuar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnRefrescar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesoreria) Then
                objVMTesoreria.ConsultarCantidadProcesadas(String.Empty)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los resultados de la importación.", Me.Name, "btnRefrescar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
