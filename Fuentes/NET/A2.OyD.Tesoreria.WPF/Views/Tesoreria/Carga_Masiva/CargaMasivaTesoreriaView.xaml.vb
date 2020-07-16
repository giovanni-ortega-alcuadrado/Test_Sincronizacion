Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class CargaMasivaTesoreriaView
    Inherits UserControl
    Dim objVMTesoreria As CargaMasivaTesoreriaViewModel
    Private mlogInicializado As Boolean = False

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New CargaMasivaTesoreriaViewModel
InitializeComponent()
            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "OrdenesPLUSView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub CargaMasivaTesoreriaView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                objVMTesoreria = CType(Me.DataContext, CargaMasivaTesoreriaViewModel)
                objVMTesoreria.viewCargaMasiva = Me

                mlogInicializado = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMTesoreria) Then
                Me.objVMTesoreria.pararTemporizador()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de ordenes.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.objVMTesoreria.ExportarExcel()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Me.objVMTesoreria.VolverAtras()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Me.objVMTesoreria.GrabarCarga()
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

End Class
