'Codigo Creado Por: Rafael Cordero
'Archivo: ImportarTitulosValorizadosView.vb
'Generado el : 07/31/2011 
'Propiedad de Alcuadrado S.A. 2011
Imports A2Utilidades.Mensajes
Imports System.Collections.ObjectModel
Imports Microsoft.Win32
Imports System.IO

Partial Public Class ImportarTitulosValorizadosView
    Inherits UserControl

    Dim vm As ImportarTitulosValorizadosViewModel

    Public Sub New()
        InitializeComponent()
        Me.DataContext = New ImportarTitulosValorizadosViewModel
    End Sub

    Private Sub ImportarTitulosValorizadosView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, ImportarTitulosValorizadosViewModel)
        ucbtnCargar.Proceso = ImportarTitulosValorizadosViewModel._STR_NOMBRE_PROCESO
        CType(Me.DataContext, ImportarTitulosValorizadosViewModel).NombreView = Me.ToString
        dtpFechaDesde.SelectedDate = Now.Date
    End Sub

    'Private Sub btnMostrarCargadorArchivos_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnMostrarCargadorArchivos.Click
    '    vm.MostrarCargadorArchivos()
    'End Sub

    Private Sub btnSubirArchivo_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnSubirArchivo.Click
        vm.SubirArchivo()
    End Sub

    Private Sub cboArchivo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboArchivo.SelectionChanged
        vm.CambioDeArchivo()
    End Sub
    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Dim objDialog = CType(sender, OpenFileDialog)
        'ArchivoSeleccionado.Nombre = Path.GetFileName(objDialog.pFile.FileName)
        'If String.IsNullOrEmpty(ArchivoSeleccionado.Nombre) Then
        '    habilitar = False
        'Else
        '    habilitar = True
        'End If
    End Sub
End Class


