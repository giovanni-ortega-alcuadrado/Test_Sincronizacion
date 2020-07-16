Partial Public Class ErrorWindow
    Inherits Window

    Public Sub New(ByVal e As Exception)
        InitializeComponent()
        If e IsNot Nothing Then
            Dim strTextoMostrar As String = e.Message + Environment.NewLine + Environment.NewLine
            If Program.MostrarDetalleErrorUsuario() Then
                strTextoMostrar += e.StackTrace
            End If
            ErrorTextBox.Text = strTextoMostrar
        End If
    End Sub

    Public Sub New(ByVal uri As Uri)
        InitializeComponent()
        If uri IsNot Nothing Then
            ErrorTextBox.Text = "Page not found: """ + uri.ToString() + """"""
        End If
    End Sub

    Public Sub New(ByVal message As String, ByVal details As String)
        InitializeComponent()
        Dim strTextoMostrar As String = message + Environment.NewLine + Environment.NewLine
        If Program.MostrarDetalleErrorUsuario() Then
            strTextoMostrar += details
        End If
        ErrorTextBox.Text = strTextoMostrar
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.DialogResult = True
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed

    End Sub
End Class
