Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class ConstanciaOperacionYankeesView
    Inherits UserControl
    Private WithEvents buscadorClientes As A2ComunesControl.BuscadorClienteLista
    Private mobjVM As ConstanciaOperacionYankeesViewModel


    Public Sub New()
        Me.DataContext = New ConstanciaOperacionYankeesViewModel
InitializeComponent()
    End Sub

Private Sub btnReporte_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, ConstanciaOperacionYankeesViewModel).Validaciones()
    End Sub
End Class
