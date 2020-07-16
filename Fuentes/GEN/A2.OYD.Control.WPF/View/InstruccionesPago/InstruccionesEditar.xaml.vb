Partial Public Class wcInstruccionesEditar
    Inherits Window

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private Sub cambioInstruccion(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        MessageBox.Show("Pintar instrucción según selección")
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

End Class
