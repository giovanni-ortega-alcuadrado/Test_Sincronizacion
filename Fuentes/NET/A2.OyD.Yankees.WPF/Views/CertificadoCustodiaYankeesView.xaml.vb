Imports Telerik.Windows.Controls
Partial Public Class CertificadoCustodiaYankeesView
    Inherits UserControl

    Public Sub New 
        Me.DataContext = New CertificadoCustodiaYankeesViewModel
    InitializeComponent()
    End Sub

Private Sub btnReporte_Click(sender As Object, e As RoutedEventArgs)
    CType(Me.DataContext, CertificadoCustodiaYankeesViewModel).Validaciones()
End Sub
End Class
