Partial Public Class AccesoDenegado
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub verTituloApp()
        Try
            Me.txtTitulo.Text = Program.TITULO_CONSOLA & " - versión " & Program.VERSION_CONSOLA
            Me.txtMsgAcceso.Text = "Acceso denegado"
        Catch ex As Exception
            Me.txtTitulo.Text = ""
        End Try
    End Sub

    Private Sub Inicio_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        verTituloApp()
    End Sub
End Class
