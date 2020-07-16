Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls


Partial Public Class UnificarISIN

    Inherits UserControl
    Dim vm As UnificarISINViewModel

    Public Sub New()

        Me.DataContext = New UnificarISINViewModel
InitializeComponent()
    End Sub
    Private Sub UnificarCuenta_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        vm = CType(Me.DataContext, UnificarISINViewModel)
        CType(Me.DataContext, UnificarISINViewModel).NombreColeccionDetalle = Me.ToString
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub


    Public Sub Rfocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub
    Public Sub Ufocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

    Private Sub btnUnificar_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnUnificar.Click
        vm.ActualizaRegistro()
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancelar.Click
        vm.cancelar()
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            vm.unifica.Nemotecnico = pobjItem.IdItem
            vm.unifica.HabilitaISIN = True
            'CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.Nemotecnico = pobjItem.IdItem
            'CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.Especie = pobjItem.Nombre
            'CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.EspeciesS = False
        End If
    End Sub

    Private Sub BuscadorEspecieListaButon_finalizoBusqueda_1(pstrClaseControl As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            vm.unifica.Nemotecnico = pobjEspecie.Nemotecnico
            vm.unifica.HabilitaISIN = True
            'CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.Nemotecnico = pobjItem.IdItem
            'CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.Especie = pobjItem.Nombre
            'CType(Me.DataContext, GenerarOrdenDesdeLiquidacionViewModel).SelectedParametrosSeleccion.EspeciesS = False
        End If
    End Sub
End Class
