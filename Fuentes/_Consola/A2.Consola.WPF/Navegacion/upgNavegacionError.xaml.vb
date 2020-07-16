Partial Public Class upgNavegacionError
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub upgNavegacionError_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim strURL As String = ""
        Try
            'If Not Me.NavigationContext Is Nothing Then
            '    If Me.NavigationContext.QueryString.Keys.Contains("URL") Then
            '        strURL = Me.NavigationContext.QueryString.Item("URL")
            '    End If
            'End If
            Me.txtMsgError.Text = "La página solicitada " & IIf(strURL = String.Empty, "", "(" & strURL & ")").ToString() & " no pudo ser cargada."
        Catch ex As Exception
            Me.txtMsgError.Text = "La página solicitada no pudo ser cargada."
        End Try
    End Sub
End Class
