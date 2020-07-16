Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web.CFUtilidades

Partial Public Class EjecutarScriptResultadoView
    Inherits Window


    Public Property ListaResultados As List(Of MensajesProceso)

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pobjDatosResultado As List(Of MensajesProceso))
        InitializeComponent()

        ListaResultados = pobjDatosResultado

        Me.dgResultado.ItemsSource = ListaResultados

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim strUrlArchivo As String = CType(sender, Button).Tag.ToString
        Program.VisorArchivosWeb_DescargarURL(strUrlArchivo)
    End Sub

    Private Sub cmdCerrar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles cmdCerrar.Click
        Me.DialogResult = True
    End Sub

End Class
