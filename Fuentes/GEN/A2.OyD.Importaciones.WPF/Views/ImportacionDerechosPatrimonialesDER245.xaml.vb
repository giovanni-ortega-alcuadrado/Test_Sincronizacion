'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ImportacionLiqView.xaml.vb
'Generado el : 07/19/2011 09:26:12
'Propiedad de Alcuadrado S.A. 2010

Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports Microsoft.Win32
Imports System.IO

Partial Public Class ImportacionDerechosPatrimonialesDER245
    Inherits UserControl

    Dim vm As ImportacionDerechosPatrimonialesViewModel
    Private Const _STR_NOMBRE_PROCESO As String = "DatosDeceval"

    Public Sub New()
        Try
            CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            InitializeComponent()
            Me.DataContext = New ImportacionDerechosPatrimonialesViewModel
            ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar el formulario.",
                                 Me.ToString(), "ImportacionDerechosPatrimonialesDER245", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ImportacionDecevalView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, ImportacionDerechosPatrimonialesViewModel)
        CType(Me.DataContext, ImportacionDerechosPatrimonialesViewModel).NombreView = Me.ToString
    End Sub

    'Private Sub btnMostrarCargadorArchivos_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
    '    vm.MostrarCargadorArchivos()
    'End Sub

    Private Sub btnAceptar_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnImportar.Click
        vm.CargarArchivo()
    End Sub

    Private Sub btnAcciones_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAcciones.Click
        vm.ConsultarResultados("A")
    End Sub

    Private Sub btnRentafija_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnRentafija.Click
        vm.ConsultarResultados("C")
    End Sub

    Private Sub btnExportarResultado_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnExportarResultado.Click
        vm.GenerarResultadoImportacion()
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
