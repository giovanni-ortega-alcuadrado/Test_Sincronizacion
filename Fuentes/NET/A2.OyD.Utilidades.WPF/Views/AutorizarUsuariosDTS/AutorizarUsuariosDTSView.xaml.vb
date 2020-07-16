Imports Telerik.Windows.Controls
'-------------------------------------------------------------------------------------
'Descripción:       View para la pantalla de asignación de DTS a usuarios
'Desarrollado por:  Santiago Alexander Vergara Orrego
'Fecha:             Octubre 30/2013
'--------------------------------------------------------------------------------------

Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class AutorizarUsuariosDTSView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New AutorizarUsuariosDTSViewModel
InitializeComponent()
    End Sub

Private Sub btnGrabar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, AutorizarUsuariosDTSViewModel).GuardarCambios()
    End Sub

    Private Sub btnAgregarUno_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, AutorizarUsuariosDTSViewModel).AgregarUno()
    End Sub

    Private Sub btnQuitarUno_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, AutorizarUsuariosDTSViewModel).QuitarUno()
    End Sub

    Private Sub btnAgregarTodos_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, AutorizarUsuariosDTSViewModel).AgregarTodos()
    End Sub

    Private Sub btnQuitarTodos_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, AutorizarUsuariosDTSViewModel).QuitarTodos()
    End Sub
End Class


