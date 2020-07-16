Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class Libranzas_CargaMasivaView
    Inherits UserControl
    Dim objVM As CargaMasivaLibranzaCFViewModel
    Private mlogInicializado As Boolean = False

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New CargaMasivaLibranzaCFViewModel
InitializeComponent()
            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 135
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "Libranzas_CargaMasivaView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Async Sub Libranzas_CargaMasivaView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                objVM = CType(Me.DataContext, CargaMasivaLibranzaCFViewModel)
                Await objVM.inicializar()
                objVM.viewCargaMasiva = Me
                objVM.CargarContenido(CargaMasivaLibranzaCFViewModel.TipoOpcionCargar.ARCHIVO)
                mlogInicializado = True

            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de libranzas", Me.Name, "", "Libranzas_CargaMasivaView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 135
    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVM) Then
                Me.objVM.pararTemporizador()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de ordenes.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        objVM.ExportarExcel()
    End Sub

Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        objVM.VolverAtras()
    End Sub

Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        objVM.GrabarCarga()
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
