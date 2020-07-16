Imports Telerik.Windows.Controls
'-------------------------------------------------------------------------------------
'Descripción:       View para la pantalla de Reliquidación de Operaciones
'Desarrollado por:  Santiago Alexander Vergara Orrego
'Fecha:             Noviembre 05/2013
'--------------------------------------------------------------------------------------

Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class ReliquidacionOperacionesView
    Inherits UserControl
    Private WithEvents buscadorClientes As A2ComunesControl.BuscadorClienteLista

    Public Sub New()
        Me.DataContext = New ReliquidacionOperacionesViewModel
InitializeComponent()
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, ReliquidacionOperacionesViewModel).ConsultarLiquidaciones()
    End Sub

Private Sub BuscadorClienteListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, ReliquidacionOperacionesViewModel).lngIdComitente = pobjComitente.IdComitente
            CType(Me.DataContext, ReliquidacionOperacionesViewModel).strNombre = pobjComitente.NombreCodigoOYD
        End If
    End Sub

End Class


