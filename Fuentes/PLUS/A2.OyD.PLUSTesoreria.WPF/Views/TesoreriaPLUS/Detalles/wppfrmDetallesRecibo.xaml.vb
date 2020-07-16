Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Globalization
Partial Public Class wppfrmDetallesRecibo
    Inherits Window
    Dim vm As TesoreroViewModel_OYDPLUS

    Public Sub New()
        Try
            InitializeComponent()
            Me.DataContext = vm
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppfrmDetallesRecibo", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub New(ByVal pvm As TesoreroViewModel_OYDPLUS, ByVal plogVerDatosFondosa As Boolean)
        Try
            vm = CType(pvm, TesoreroViewModel_OYDPLUS)
            InitializeComponent()
            Me.DataContext = vm

            If plogVerDatosFondosa Then
                tabCargarPagos.Header = "Cargar pagos a fondos"
                BorderCargarPagosAFondos.Visibility = Visibility.Visible
                BorderCargarPagosA.Visibility = Visibility.Collapsed
                dgCargarPagosAFondos.Visibility = Visibility.Visible
                dgCargarPagosA.Visibility = Visibility.Collapsed
            Else
                tabCargarPagos.Header = "Cargar pagos a"
                BorderCargarPagosAFondos.Visibility = Visibility.Collapsed
                BorderCargarPagosA.Visibility = Visibility.Visible
                dgCargarPagosAFondos.Visibility = Visibility.Collapsed
                dgCargarPagosA.Visibility = Visibility.Visible
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppfrmDetallesRecibo", Me.Name, "New(Sobrecargado)", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim strURLArchivo As String = CType(sender, Button).Tag
        Program.VisorArchivosWeb_DescargarURL(strURLArchivo)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
