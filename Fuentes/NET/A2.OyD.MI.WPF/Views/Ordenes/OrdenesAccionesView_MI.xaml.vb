Imports Telerik.Windows.Controls
Partial Public Class OrdenesAccionView_MI
    Inherits UserControl

    Private mlogInicializado As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub OrdenesAccionView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                Me.Content = New OrdenesView_MI(OrdenesViewModel.ClasesOrden.A)
                mlogInicializado = True
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
