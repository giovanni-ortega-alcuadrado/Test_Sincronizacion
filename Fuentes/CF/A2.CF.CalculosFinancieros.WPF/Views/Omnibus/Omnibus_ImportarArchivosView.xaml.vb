Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2ComunesImportaciones


Partial Public Class Omnibus_ImportarArchivosView
    Inherits UserControl

#Region "Variables"

    Private mobjVM As Omnibus_ImportarArchivosViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)

#End Region

#Region "Inicializacion"

    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            'Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New Omnibus_ImportarArchivosViewModel
InitializeComponent()
    End Sub



    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, Omnibus_ImportarArchivosViewModel)
            mobjVM.NombreView = Me.ToString

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region

    ''Executes when the user navigates to this page.
    'Protected Overrides Sub OnNavigatedTo(ByVal e As System.Windows.Navigation.NavigationEventArgs)

    'End Sub

#Region "Manejadores error"

    Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para presentar los datos del detalle.", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga de los datos", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

    Private Sub ucBtnDialogoImportar_SubirArchivo(sender As Object, e As RoutedEventArgs)
        mobjVM.IsBusy = True
    End Sub

    Private Sub ucBtnDialogoImportar_CargarArchivoGenerico(sender As ObjetoInformacionArchivo, pProceso As String)
        Try
            mobjVM.IsBusy = False
            Dim objDialog = CType(sender, ObjetoInformacionArchivo)
            If Not IsNothing(objDialog.pFile) Then
                'If objDialog.pFile.File.Extension.Equals(".csv") Or objDialog.pFile.File.Extension.Equals(".xls") Or objDialog.pFile.File.Extension.Equals(".xlsx") Then
                mobjVM.NombreArchivoSeleccionado = objDialog.pFile.FileName
                'Dim strRutaArchivo As String = objDialog.pFile.FileName
                Dim strRutaArchivo As String = System.IO.Path.GetFileName(objDialog.pFile.FileName)

                Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, Omnibus_ImportarArchivosViewModel), strRutaArchivo, pProceso)
                Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                viewImportacion.ShowDialog()
            End If
        Catch ex As Exception
            mobjVM.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "CargarArchivoGenerico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ucBtnDialogoImportar_ErrorImportandoArchivo(Metodo As String, objEx As Exception)
        mobjVM.IsBusy = False
    End Sub
End Class
