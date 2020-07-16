Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes

Partial Public Class TipoOrdenGiro
    Inherits Window

    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        InitializeComponent()
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If rbtTipoOrdenDividendos.IsChecked = False And rbtTipoOrdenRecurrente.IsChecked = False Then
            mostrarMensaje("Debe seleccionar al menos una opción.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            Me.DialogResult = True
        End If
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

End Class
