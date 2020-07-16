Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class Libranzas_CargaMasivaImportarResultadoView
    Inherits UserControl
    Dim objVM As CargaMasivaLibranzaCFViewModel

    Public Sub New(ByVal pobjVM As CargaMasivaLibranzaCFViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVM = pobjVM

            If Me.Resources.Contains("VM") Then
                Me.Resources.Remove("VM")
            End If

            Me.Resources.Add("VM", objVM)
            InitializeComponent()

            objVM.ConsultarCantidadProcesadas(String.Empty)
            Me.dg.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "Libranzas_CargaMasivaImportarResultadoView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            If Not IsNothing(objVM) Then
                objVM.ConfirmarValoresResultado()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de confirmación.", Me.Name, "btnConfirmarContinuar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnRefrescar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVM) Then
                objVM.ConsultarCantidadProcesadas(String.Empty)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los resultados de la importación.", Me.Name, "btnRefrescar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
