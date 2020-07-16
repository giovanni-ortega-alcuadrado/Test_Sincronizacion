Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class FraccionarCustodiasView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New FraccionarCustodiasViewModel
InitializeComponent()
    End Sub

    Private Sub FraccionarCustodiasView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'vm = CType(Me.DataContext, ModIdentiClientesViewModel)
        'CType(Me.DataContext, ModIdentiClientesViewModel).NombreView = Me.ToString
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, FraccionarCustodiasViewModel)._mlogBuscarCliente = False
            CType(Me.DataContext, FraccionarCustodiasViewModel).CamposBusquedaSelected.CodigoCliente = pobjItem.CodigoOYD
            CType(Me.DataContext, FraccionarCustodiasViewModel).CamposBusquedaSelected.NombreCliente = pobjItem.Nombre
            CType(Me.DataContext, FraccionarCustodiasViewModel).CamposBusquedaSelected.TipoDcto = pobjItem.CodTipoIdentificacion
            CType(Me.DataContext, FraccionarCustodiasViewModel).CamposBusquedaSelected.NroDcto = pobjItem.NroDocumento
            CType(Me.DataContext, FraccionarCustodiasViewModel).CamposBusquedaSelected.TelefonoCliente = pobjItem.Telefono
            CType(Me.DataContext, FraccionarCustodiasViewModel).CamposBusquedaSelected.DireccionCliente = pobjItem.DireccionEnvio
            If Not IsNothing(CType(Me.DataContext, FraccionarCustodiasViewModel).ListaCustodiasCliente) Then
                CType(Me.DataContext, FraccionarCustodiasViewModel).ListaCustodiasCliente.Clear()
            End If
            CType(Me.DataContext, FraccionarCustodiasViewModel)._mlogBuscarCliente = True
        End If
    End Sub
    Private Sub BuscarCustodias_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, FraccionarCustodiasViewModel).BuscarCustadias()
    End Sub

    Private Sub Aceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
                'DataGrid1.EndEditRow(True)
        'DataGrid1.EndEdit()
        CType(Me.DataContext, FraccionarCustodiasViewModel).CambiarEstadoCustodias()
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, FraccionarCustodiasViewModel)._mlogBuscarEspecie = False
            CType(Me.DataContext, FraccionarCustodiasViewModel).CamposBusquedaSelected.Nemotecnico = pobjItem.IdItem
            CType(Me.DataContext, FraccionarCustodiasViewModel).CamposBusquedaSelected.Especie = pobjItem.Nombre
            CType(Me.DataContext, FraccionarCustodiasViewModel)._mlogBuscarEspecie = True
        End If
    End Sub

    Private Sub txtClienteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub
End Class
