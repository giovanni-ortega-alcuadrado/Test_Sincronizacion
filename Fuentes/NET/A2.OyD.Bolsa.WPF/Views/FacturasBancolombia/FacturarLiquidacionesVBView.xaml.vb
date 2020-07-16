Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OYD.OYDServer.RIA.Web


Partial Public Class FacturarLiquidacionesVBView
    Inherits UserControl

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Private mstrDicCombosEspecificos As String = String.Empty

#End Region

    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Me.DataContext = New FacturarLiquidacionesVBViewModel
InitializeComponent()
    End Sub

    Private Sub Buscar_finalizoBusquedaComitenteDesde(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, FacturarLiquidacionesVBViewModel).IDComitenteDesde = pobjComitente.IdComitente
        End If
    End Sub

    Private Sub Buscar_finalizoBusquedaComitenteHasta(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, FacturarLiquidacionesVBViewModel).IDComitenteHasta = pobjComitente.IdComitente
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, FacturarLiquidacionesVBViewModel).LimpiarPantalla()
    End Sub

Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, FacturarLiquidacionesVBViewModel).ConsultarReportefacturas()
    End Sub

Private Sub btnEnviar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, FacturarLiquidacionesVBViewModel).EnviarCadena()
    End Sub

Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, FacturarLiquidacionesVBViewModel).ConsultaClientes_APT()
    End Sub

Private Sub cmbSucursal_LostFocus(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
