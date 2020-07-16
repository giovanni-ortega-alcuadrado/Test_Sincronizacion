Imports Telerik.Windows.Controls
Partial Public Class TesoreriaNotasView
    Inherits UserControl

    Private mlogInicializado As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub TesoreriaNotasView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                Me.Content = New TesoreriaView(TesoreriaViewModel.ClasesTesoreria.N)
                mlogInicializado = True
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
