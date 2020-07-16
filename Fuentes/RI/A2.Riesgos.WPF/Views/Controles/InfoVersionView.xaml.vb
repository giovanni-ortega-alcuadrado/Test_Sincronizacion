Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2MCCoreWPF

Partial Public Class InfoVersionView
    Inherits Window

    Public Sub New(ByVal _vm As clsInfoVersion)
        Try
            InitializeComponent()
            Me.DataContext = _vm

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de nueva versión", Me.Name, "New", "InfoVersionView", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles btnCancelar.Click
        Me.DialogResult = False
    End Sub


End Class
