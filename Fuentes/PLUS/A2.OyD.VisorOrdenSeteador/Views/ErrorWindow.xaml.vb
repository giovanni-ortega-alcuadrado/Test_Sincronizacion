Partial Public Class ErrorWindow
    Inherits ChildWindow
    
    Public Sub New(ByVal e As Exception)
        InitializeComponent()
        If e IsNot Nothing Then
            ErrorTextBox.Text = e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace
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
        ErrorTextBox.Text = message + Environment.NewLine + Environment.NewLine + details
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.DialogResult = True
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        App.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub
End Class
