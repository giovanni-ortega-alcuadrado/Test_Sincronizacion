Imports Telerik.Windows.Controls
Partial Public Class VisorASPResultadoValoracionQA
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub VisorASP_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim strPagina As String = String.Empty
        Dim strservidor As String = String.Empty

        Try
            strservidor = Program.RetornarParametroApp("URL_VisorASP")
            strPagina = strservidor & "/ResultadosValoracionPortafolio.aspx"
            vsrASP.Source = New Uri(strPagina)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarMensaje("No se pudo cargar la página " & strPagina & " en el visor.", Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End Try

    End Sub
End Class
