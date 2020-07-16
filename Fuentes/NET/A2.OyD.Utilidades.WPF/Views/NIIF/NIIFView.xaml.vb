Imports Telerik.Windows.Controls
'-------------------------------------------------------------------------------------
'Descripción:       View para la pantalla de Codificación de normas contables
'Desarrollado por:  Ricardo Barrientos Pérez
'Fecha:             Noviembre 30/2013
'--------------------------------------------------------------------------------------

Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class NIIFView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New NIIFViewModel
InitializeComponent()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs) Handles btnCancelar.Click

    End Sub

    Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = 9 Then
            e.Handled = False
        ElseIf ((e.Key > 47 And e.Key < 58) Or (e.Key > 95 And e.Key < 106)) Then 'numeros sin shift
            e.Handled = False
        ElseIf ((e.Key > 96 And e.Key < 123) Or (e.Key > 64 And e.Key < 91)) Then 'Letras
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub
Private Sub btnGrabar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, NIIFViewModel).GuardarCambios()
    End Sub

    Private Sub btnCancelar_Click_1(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, NIIFViewModel).CancelarCambios()
    End Sub

    Private Sub btnBloquearCriterios_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, NIIFViewModel).ActivarDesactivarComboCriterio()
    End Sub
End Class


