Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2ComunesImportaciones
Imports A2.OYD.OYDServer.RIA.Web.CFCalculosFinancieros


Partial Public Class DerechosPatrimonialesView
    Inherits UserControl


    ''' <summary>
    ''' Eventos creados para la comunicación con las clases DerechosPatrimonialesView y DerechosPatrimonialesViewModel
    ''' Pantalla Derechos Patrimoniales (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 3 de septiembre/2015</remarks>
#Region "Variables"

    Private mobjVM As DerechosPatrimonialesViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)
    Dim strRutaArchivo As String
    Dim strProceso As String

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New DerechosPatrimonialesViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()

        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, DerechosPatrimonialesViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.ViewDerechosPatrimoniales = Me

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            mobjVM.inicializar()
        End If

    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub seleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Eventos para asignar cliente"

    ' ''' <summary>
    ' ''' Se ejecuta cuando se dispara el evento comitenteAsignado en el buscador de clientes (control buscador clientes lista)
    ' ''' </summary>
    ' ''' <param name="pstrClaseControl">Permite identificar el llamado</param>
    ' ''' <param name="pobjComitente">Datos del comitente seleccionado en el buscador</param>
    ' ''' <remarks></remarks>
    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                Me.mobjVM.lngIDComitente = pobjComitente.CodigoOYD
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.lngIDComitente = Nothing
                Me.mobjVM.strNombreComitente = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ucBtnDialogoImportar_SubirArchivo(sender As Object, e As RoutedEventArgs)
        mobjVM.IsBusy = True
    End Sub

    ''' <summary>
    ''' Se crea este evento para controlar el isbusy cuando hay error en la importación de archivos
    ''' </summary>
    ''' <param name="Metodo">Nombre del método que genera la inconsistencia</param>
    ''' <param name="objEx">Error capturado</param>
    ''' <remarks></remarks>
    Private Sub CapturarErrorImportandoArchivo(Metodo As String, objEx As Exception)
        mobjVM.IsBusy = False
    End Sub

    Private Sub ucBtnDialogoImportar_CargarArchivo(sender As ObjetoInformacionArchivo, pstrProceso As String)
        Try
            Dim objDialog = CType(sender, ObjetoInformacionArchivo)

            If Not IsNothing(objDialog.pFile) Then
                'If objDialog.pFile.File.Extension.Equals(".csv") Or objDialog.pFile.File.Extension.Equals(".xls") Or objDialog.pFile.File.Extension.Equals(".xlsx") Then
                strRutaArchivo = System.IO.Path.GetFileName(objDialog.pFile.FileName)
                strProceso = pstrProceso

                A2Utilidades.Mensajes.mostrarMensajePregunta("¿ Desea eliminar todos los registros cargados previamente ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf TerminoPreguntarRegistrosCargados, False)

                'Else
                '    A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores, Program.Usuario)
                'End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "CargarArchivoGenerico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub TerminoPreguntarRegistrosCargados(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Me.mobjVM.ValidarDatos() Then
                'Cuando presiona el botón SI
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, DerechosPatrimonialesViewModel), strRutaArchivo, strProceso, Nothing, True)
                    Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                    viewImportacion.ShowDialog()
                Else
                    Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, DerechosPatrimonialesViewModel), strRutaArchivo, strProceso, Nothing, False)
                    Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                    viewImportacion.ShowDialog()
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método TerminoPreguntarRegistrosCargados", Me.ToString(), "TerminoPreguntarRegistrosCargados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Manejadores error"

    Private Sub btnConsultarAcciones_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ValidarAcciones()
    End Sub

Private Sub btnReportePaso1_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarInformeAcciones()
    End Sub

Private Sub btnSiguiente_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Siguiente()
    End Sub

Private Sub btnConsultarRentaFija_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarRentaFija()
    End Sub

Private Sub btnReportePaso2_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarInformeRentaFija()
    End Sub

Private Sub btnAtrasPaso2_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Atras()
    End Sub

Private Sub btnCruce_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarCarga()
    End Sub

Private Sub btnPagar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Pagar()
    End Sub

Private Sub btnGenerar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarInformePaso4()
    End Sub

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

End Class

