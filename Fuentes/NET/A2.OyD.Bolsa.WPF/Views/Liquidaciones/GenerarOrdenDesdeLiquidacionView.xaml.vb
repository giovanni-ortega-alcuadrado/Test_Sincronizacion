Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades

Partial Public Class GenerarOrdenDesdeLiquidacionView
    Inherits UserControl



    Public Sub New()
        Me.DataContext = New GenerarOrdenDesdeLiquidacionViewModel
InitializeComponent()
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.CodigoCliente = pobjItem.CodigoOYD
            CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.NombreCliente = pobjItem.Nombre
            CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.ClientesS = False
        End If
    End Sub

    'Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
    '    df.ValidateItem()
    '    If df.ValidationSummary.HasErrors Then
    '        df.CancelEdit()
    '    Else
    '        df.CommitEdit()
    '        df.FindName("dgEspeciesISINFungible").EndEdit()
    '    End If
    'End Sub

    Private Sub Aceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'DataGrid1.EndEditRow(True)
        'DataGrid1.EndEdit()
        CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).GenerarOrden()
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.Nemotecnico = pobjItem.IdItem
            CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.Especie = pobjItem.Nombre
            CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.EspeciesS = False
        End If
    End Sub

    Private Sub SeleccionarTodos_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SeleccionarTodos()
    End Sub

    Private Sub DesselecionarTodos_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).DesseleccionarTodos()
    End Sub

    Private Sub SeleccionPorPametros_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SeleccionarPorParametros()
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaCuentaDeposito(ByVal pintCuentaDeposito As Integer, ByVal pobjCuentaDeposito As BuscadorCuentasDeposito)

        If Not IsNothing(pobjCuentaDeposito) Then
            CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).LiquidacionGapOrdenesSelected.CuentaDeceval = pobjCuentaDeposito.NroCuentaDeposito
            CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).LiquidacionGapOrdenesSelected.UBICACIONTITULO = pobjCuentaDeposito.NombreDeposito
        End If
    End Sub



End Class