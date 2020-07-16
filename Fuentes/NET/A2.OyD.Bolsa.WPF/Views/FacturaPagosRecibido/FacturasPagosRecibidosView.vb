Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: FacturasBancaInvView.xaml.vb
'Generado el : 02/24/2012 07:45:33
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class FacturasPagosRecibidosView
    Inherits UserControl
    Private WithEvents buscadorClientes As A2ComunesControl.BuscadorClienteLista
    Private mobjVM As FacturasPagosRecibidosViewModel


    Public Sub New()
        Me.DataContext = New FacturasPagosRecibidosViewModel
InitializeComponent()
    End Sub
   
    Private Sub Click_btnConsultar(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, FacturasPagosRecibidosViewModel).ConsultarPagos()
    End Sub

Private Sub Click_btnLimpiar(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, FacturasPagosRecibidosViewModel).LimpiarControles()
    End Sub

Private Sub Click_btnGenerar(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, FacturasPagosRecibidosViewModel).FacturarPagos()
    End Sub

Private Sub Click_Marcar(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        mobjVM = CType(Me.DataContext, FacturasPagosRecibidosViewModel)
        mobjVM.FacturasPagosRecibidosSelected.Marcar = (DirectCast(sender, System.Windows.Controls.CheckBox).IsChecked)
        mobjVM.HabilitarFacturar()
    End Sub

End Class


