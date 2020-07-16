Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: FacturasBancaInvView.xaml.vb
'Generado el : 02/24/2012 07:45:33
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ConstanciaOperacionView
    Inherits UserControl
    Private WithEvents buscadorClientes As A2ComunesControl.BuscadorClienteLista
    Private mobjVM As ConstanciaOperacionViewModel


    Public Sub New()
        Me.DataContext = New ConstanciaOperacionViewModel
InitializeComponent()
    End Sub


Private Sub btnReporte_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, ConstanciaOperacionViewModel).ConsultarLiquidaciones()
    End Sub

End Class


