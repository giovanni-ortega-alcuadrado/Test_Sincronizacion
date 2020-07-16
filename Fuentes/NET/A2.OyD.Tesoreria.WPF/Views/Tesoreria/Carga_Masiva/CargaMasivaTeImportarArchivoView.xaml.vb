
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class CargaMasivaTeImportarArchivoView
    Inherits UserControl
    Dim objVMTesoreria As CargaMasivaTesoreriaViewModel

    Public Sub New(ByVal pobjVMTesoreria As CargaMasivaTesoreriaViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMTesoreria = pobjVMTesoreria

            If Me.Resources.Contains("VMTesoreria") Then
                Me.Resources.Remove("VMTesoreria")
            End If

            Me.Resources.Add("VMTesoreria", objVMTesoreria)
            InitializeComponent()

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOrdenesView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Private Sub btnSubirArchivoTesoreria_CargarArchivo(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    If Path.GetExtension(objDialog.FileName).ToLower.Equals(".csv") Or Path.GetExtension(objDialog.FileName).ToLower.Equals(".txt") Then
                        objVMTesoreria.NombreArchivo = Path.GetFileName(objDialog.FileName)
                    Else
                        mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivoTesoreria_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnImportarArchivo_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesoreria) Then
                objVMTesoreria.ImportarArchivo()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar importar el archivo.", Me.ToString(), "btnImportarArchivo_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
