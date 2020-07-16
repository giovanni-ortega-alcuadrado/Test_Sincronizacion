'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ImportacionLiqView.xaml.vb
'Generado el : 07/19/2011 09:26:12
'Propiedad de Alcuadrado S.A. 2010

Imports Microsoft.Win32
Imports System.IO

Partial Public Class ImportacionDecevalView
    Inherits UserControl

    Dim vm As ImportacionDecevalViewModel
    Private Const _STR_NOMBRE_PROCESO As String = "DatosDeceval"


    Public Sub New()
        InitializeComponent()
        Me.DataContext = New ImportacionDecevalViewModel
        ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO
    End Sub

    Private Sub ImportacionDecevalView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, ImportacionDecevalViewModel)
        CType(Me.DataContext, ImportacionDecevalViewModel).NombreView = Me.ToString
    End Sub

    'Private Sub btnMostrarCargadorArchivos_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
    '    vm.MostrarCargadorArchivos()
    'End Sub

    Private Sub btnAceptar_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        vm.CargarArchivo()
    End Sub

    'Private Sub cbArchivosASubir_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbArchivosASubir.SelectionChanged
    '    vm.LimpiarResultados()
    'End Sub

    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Dim objDialog = CType(sender, OpenFileDialog)
        'ArchivoSeleccionado.Nombre = Path.GetFileName(objDialog.pFile.FileName)
        If Not String.IsNullOrEmpty(Path.GetFileName(objDialog.FileName)) Then
            vm.LimpiarResultados()
            vm.VentanaCargaArchivoControl(Path.GetFileName(objDialog.FileName))
        End If
    End Sub

End Class
