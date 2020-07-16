Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls

Partial Public Class DuplicarClientesView
    Inherits UserControl
    Dim objViewModel As DuplicarClientesViewModel

    Public Sub New()
        Me.DataContext = New DuplicarClientesViewModel
InitializeComponent()
    End Sub

    Private Sub Clientes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        objViewModel = CType(Me.DataContext, DuplicarClientesViewModel)
    End Sub


    Private Sub BuscadorClienteListaButon_finalizoBusqueda_1(pstrClaseControl As String, pobjComitente AS OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            objViewModel.ActualizarDatosCliente(pobjComitente)
            objViewModel.NroDocumentoConsulta = pobjComitente.NroDocumento
        End If
    End Sub

    Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        objViewModel.DuplicarRegistro()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        objViewModel.AceptarCambios()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        objViewModel.CancelarCambios()
    End Sub

    Private Sub txtNroIdentificacion_LostFocus(sender As Object, e As RoutedEventArgs)
        objViewModel.ConsultarDatosCliente()
    End Sub
End Class
