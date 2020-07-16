Imports Telerik.Windows.Controls
Imports SilverlightFX.UserInterface

Partial Public Class PlanoComEgresosView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New PlanoComEgresosViewModel
        InitializeComponent()
    End Sub

Private Sub CheckBox_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, PlanoComEgresosViewModel).CambioItemSeleccionado()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, PlanoComEgresosViewModel).Generar()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim objItemSeleccionado As A2.OyD.OYDServer.RIA.Web.OyDImportaciones.Archivo = CType(CType(sender, Button).Tag, A2.OyD.OYDServer.RIA.Web.OyDImportaciones.Archivo)
        CType(Me.DataContext, PlanoComEgresosViewModel).BorrarArchivo(objItemSeleccionado)
    End Sub
End Class
