
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl



Partial Public Class InhabilitarClientes
    Inherits UserControl

    Dim objVMA2Utils As A2UtilsViewModel

    Public Sub New()
        Try
            Me.DataContext = New InhabilitarClientesViewModel
InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de Inhabilitar Clientes", Me.Name, "", "InhabilitarClientes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub cargarArchivo(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)

            If Not IsNothing(objDialog.FileName) Then
                If Path.GetExtension(objDialog.FileName).Equals(".xls") Or Path.GetExtension(objDialog.FileName).Equals(".xlsx") Then
                    Dim strRutaArchivo As String = Path.GetFileName(objDialog.FileName)
                    Dim strNombreArchivo As String = Path.GetFileName(objDialog.FileName)
                    CType(Me.DataContext, InhabilitarClientesViewModel).NombreArchivo = strRutaArchivo
                    CType(Me.DataContext, InhabilitarClientesViewModel).TipoCargaInhabilitado = False
                Else
                    mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo", Me.ToString(), "cargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class