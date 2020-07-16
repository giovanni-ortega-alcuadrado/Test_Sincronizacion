
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices

Partial Public Class ImportarArchivoFondosPLUSView
    Inherits UserControl

    Private objVMImportacion As SubirArchivoFondosViewModel_OYDPLUS

#Region "Inicializaciones"
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            InitializeComponent()

            Me.DataContext = CType(Me.Resources("VMSubirArchivo"), SubirArchivoFondosViewModel_OYDPLUS)
            objVMImportacion = CType(Me.Resources("VMSubirArchivo"), SubirArchivoFondosViewModel_OYDPLUS)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

    Private Sub ImportarArchivoFondosPLUSView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "ImportarArchivoFondosPLUSView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnSubirArchivoFondos_CargarArchivo_1(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog.FileName) Then
                If Path.GetExtension(objDialog.FileName).Equals(".csv") Or Path.GetExtension(objDialog.FileName).Equals(".xls") Or Path.GetExtension(objDialog.FileName).Equals(".xlsx") Then
                    objVMImportacion.NombreArchivo = Path.GetFileName(objDialog.FileName)
                Else
                    mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirRecibos_CargarArchivo_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnImportarArchivo_Click_1(sender As Object, e As RoutedEventArgs)
        objVMImportacion.ImportarArchivo()
    End Sub
End Class
