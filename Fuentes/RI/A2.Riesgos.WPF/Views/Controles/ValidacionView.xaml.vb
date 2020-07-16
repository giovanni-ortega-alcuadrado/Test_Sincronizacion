Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes

Partial Public Class ValidacionView
    Inherits Window


    Public Sub New()
        Try
            InitializeComponent()

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar la ventana de validación del libro", Me.Name, "New", "ValidacionView", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub MostrarMensaje(MensajeValidacion)
        txtMensaje.Text = MensajeValidacion
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
