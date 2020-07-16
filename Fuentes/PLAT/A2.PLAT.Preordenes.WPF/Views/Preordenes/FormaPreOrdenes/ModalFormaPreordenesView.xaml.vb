Public Class ModalFormaPreordenesView
    Dim intIDRegistro As Integer
    Public Sub New(ByVal pintIDRegistro As Integer, ByVal pstrTitulo As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtTituloVista.Text = pstrTitulo
        intIDRegistro = pintIDRegistro
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        objFormaPreordenesView.logMostrarBotones = False
        objFormaPreordenesView.intIDRegistro = intIDRegistro
    End Sub
End Class
