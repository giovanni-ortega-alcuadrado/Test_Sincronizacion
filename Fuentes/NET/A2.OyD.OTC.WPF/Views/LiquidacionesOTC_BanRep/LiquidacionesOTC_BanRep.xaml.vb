
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl


Partial Public Class LiquidacionesOTC_BanRep
    Inherits UserControl

    Property checkBoxes As New List(Of CheckBox)

    Public Sub New()
        Try
            Me.DataContext = New LiquidacionesOTC_BanRepViewModel
InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de importación de liquidaciones OTC BanRep", Me.Name, "", "LiquidacionesOTC_BanRep", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub chkAll_Click(sender As Object, e As RoutedEventArgs)
        Try
            'If CType(Me.DataContext, LiquidacionesOTC_BanRepViewModel) IsNot Nothing Then
            Dim chk As CheckBox = CType(sender, CheckBox)
            Dim check As Boolean = chk.IsChecked.Value


            CType(Me.DataContext, LiquidacionesOTC_BanRepViewModel).SeleccionarTodasLEO(check)
            'End If

        Catch ex As Exception
            mostrarMensaje("Error chkAll_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub cargarArchivoOTC_BanRep(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)

            If Not IsNothing(objDialog.FileName) Then
                If Path.GetExtension(objDialog.FileName).Equals(".txt") Then
                    Dim strRutaArchivo As String = Path.GetFileName(objDialog.FileName)

                    Dim viewImportacion As New ImportarArchivosOTC_BanRep(CType(Me.DataContext, LiquidacionesOTC_BanRepViewModel), strRutaArchivo)
                    Me.txtRuta.Text = strRutaArchivo
                    Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                    viewImportacion.ShowDialog()
                Else
                    mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "cargarArchivoOTC_BanRep", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#Region "Resultados asíncronos"

#End Region

End Class


