Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: FacturasBancaInvView.xaml.vb
'Generado el : 02/24/2012 07:45:33
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class FacturasValorCustodiasView
    Inherits UserControl
    Private WithEvents buscadorClientes As A2ComunesControl.BuscadorClienteLista
    Private mobjVM As FacturasValorCustodiasViewModel


    Public Sub New()
        Me.DataContext = New FacturasValorCustodiasViewModel
InitializeComponent()
    End Sub


    Private Sub BuscadorInicial_finalizoBusquedaespecies(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, FacturasValorCustodiasViewModel).EspecieInicial = pobjItem.Nemotecnico
        End If
    End Sub

    Private Sub BuscadorFinal_finalizoBusquedaespecies(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, FacturasValorCustodiasViewModel).EspecieFinal = pobjItem.Nemotecnico
        End If
    End Sub

    Private Sub BuscarInicial_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, FacturasValorCustodiasViewModel).CodigoOyDInicial = pobjComitente.IdComitente
        End If
    End Sub

    Private Sub BuscarFinal_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, FacturasValorCustodiasViewModel).CodigoOyDFinal = pobjComitente.IdComitente

        End If
      
    End Sub

    Private Sub TextCodFinal_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextCodInicial_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, FacturasValorCustodiasViewModel).ConsultarPagos()
    End Sub

Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, FacturasValorCustodiasViewModel).LimpiarControles()
    End Sub

Private Sub btnGenerar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, FacturasValorCustodiasViewModel).FacturarPagos()
    End Sub

Private Sub Click_Marcar(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        mobjVM = CType(Me.DataContext, FacturasValorCustodiasViewModel)
        mobjVM.FacturasValorCustodiasSelected.Marcar = (DirectCast(sender, System.Windows.Controls.CheckBox).IsChecked)
        mobjVM.HabilitarFacturar()
    End Sub
End Class


