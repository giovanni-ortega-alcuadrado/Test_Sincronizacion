Imports Telerik.Windows.Controls
' Descripción:    View del formulario de Generar Plano Operaciones
' Creado por:     Sebastian Londoño Benitez
' Fecha:          Junio 7/2013

Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports C1.Silverlight.DataGrid
Imports C1.Silverlight.DataGrid.Summaries



Partial Public Class GenerarPlanoOperacionesView
    Inherits UserControl


#Region "Inicializaciones"

    Public Sub New()
        Me.DataContext = New GenerarArchivoOperacionesViewModel
InitializeComponent()
    End Sub

#End Region

#Region "Eventos"

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim objItemSeleccionado As OyDImportaciones.Archivo = CType(CType(sender, Button).Tag, OyDImportaciones.Archivo)
        CType(Me.DataContext, GenerarArchivoOperacionesViewModel).BorrarArchivo(objItemSeleccionado)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim objItemSeleccionado As OyDImportaciones.Archivo = CType(CType(sender, Button).Tag, OyDImportaciones.Archivo)
        CType(Me.DataContext, GenerarArchivoOperacionesViewModel).NavegarArchivo(objItemSeleccionado)
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarArchivoOperacionesViewModel).GuardarPlanoOperaciones()
    End Sub

#End Region

End Class