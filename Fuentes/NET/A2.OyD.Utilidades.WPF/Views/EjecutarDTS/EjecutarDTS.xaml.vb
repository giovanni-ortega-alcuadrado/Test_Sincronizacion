Imports Telerik.Windows.Controls

Partial Public Class EjecutarDTS
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New EjecutarDTSViewModel
InitializeComponent()
    End Sub

    Private Sub Facturas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        CType(Me.DataContext, EjecutarDTSViewModel).NombreColeccionDetalle = Me.ToString
    End Sub

    Private Sub Ejecutardts_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Ejecutardts.Click

        CType(Me.DataContext, EjecutarDTSViewModel).EjecutarDTS()
    End Sub

End Class
