Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2ControlMenu
Imports A2Utilidades.Mensajes
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: LiquidacionesView.xaml.vb
'Generado el : 05/30/2011 09:18:58
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class EnviarAdministracionValoresView
    Inherits UserControl

    Public Sub New()
        Try
            Me.DataContext = New EnviarAdministracionValoresViewModel
InitializeComponent()

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de envío administración valores", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CancelarEditarRegistro_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub cm_EventoConfirmarGrabacion(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub chkAll_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim chk As CheckBox = CType(sender, CheckBox)
            Dim check As Boolean = chk.IsChecked.Value

            CType(Me.DataContext, EnviarAdministracionValoresViewModel).SeleccionarTodas(check)

        Catch ex As Exception
            mostrarMensaje("Error chkAll_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub
End Class
