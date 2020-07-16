Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2ComunesImportaciones
Imports A2ComunesControl


Partial Public Class ProcesosTrasladosInternosView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases ProcesosTrasladosInternosView y ProcesosTrasladosInternosViewModel
    ''' Pantalla Procesar Portafolio (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As ProcesosTrasladosInternosViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New ProcesosTrasladosInternosViewModel
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

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, ProcesosTrasladosInternosViewModel)
            mobjVM.NombreView = Me.ToString

            'OJOOO
            'Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

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

    Private Sub btnLimpiarDelBanco_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.intDelBanco = Nothing
                Me.mobjVM.strNombreDelBanco = Nothing
                Me.mobjVM.strCuentaDelBanco = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarAlBanco_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.intAlBanco = Nothing
                Me.mobjVM.strNombreAlBanco = Nothing
                Me.mobjVM.strCuentaAlBanco = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem AS OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "IDAlBanco"
                    Me.mobjVM.intAlBanco = pobjItem.IdItem
                    Me.mobjVM.strNombreAlBanco = pobjItem.Nombre
                    Me.mobjVM.strCuentaAlBanco = pobjItem.CodItem
                Case "IDDelBanco"
                    Me.mobjVM.intDelBanco = pobjItem.IdItem
                    Me.mobjVM.strNombreDelBanco = pobjItem.Nombre
                    Me.mobjVM.strCuentaDelBanco = pobjItem.CodItem
            End Select
        End If
    End Sub

    Private Sub ucBtnDialogoImportar_SubirArchivo(sender As Object, e As RoutedEventArgs)
        'mobjVM.IsBusy = True
    End Sub

    Private Sub ucBtnDialogoImportar_CargarArchivo(sender As ObjetoInformacionArchivo, strProceso As String)
        Try
            Dim objDialog = CType(sender, ObjetoInformacionArchivo)

            If Not IsNothing(objDialog.pFile) Then
                'If objDialog.pFile.File.Extension.Equals(".csv") Or objDialog.pFile.File.Extension.Equals(".xls") Or objDialog.pFile.File.Extension.Equals(".xlsx") Then
                'Dim strRutaArchivo As String = objDialog.pFile.FileName
                Dim strRutaArchivo As String = System.IO.Path.GetFileName(objDialog.pFile.FileName)

                Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, ProcesosTrasladosInternosViewModel), strRutaArchivo, strProceso)
                Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                viewImportacion.ShowDialog()

                'Else
                '    A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores, Program.Usuario)
                'End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "CargarArchivoGenerico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtComitente_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Try
            'Jorge Andres Bedoya 20160329
            If (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D9) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D8) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D7) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D6) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D5) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D4) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D3) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D2) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D1) Or
                    (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D0) Then
                e.Handled = True
            Else
                If (((e.Key >= Key.D0 And e.Key <= Key.D9) Or (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) Or e.Key = Key.Back Or e.Key = Key.Tab)) Then
                    e.Handled = False
                Else
                    e.Handled = True
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el codigo ingresado.", _
                                Me.ToString(), "txtComitente_KeyDown", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtComitenteDetalle_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(mobjVM) Then
                If Not IsNothing(mobjVM.DetalleSeleccionado) Then
                    If mobjVM.DetalleSeleccionado.intID <> CInt(CType(sender, TextBox).Tag) Then
                        If mobjVM.ListaDetalle.Where(Function(i) i.intID = CInt(CType(sender, TextBox).Tag)).Count > 0 Then
                            mobjVM.DetalleSeleccionado = mobjVM.ListaDetalle.Where(Function(i) i.intID = CInt(CType(sender, TextBox).Tag)).First
                        End If
                    End If
                End If

                If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
                    If mobjVM.DetalleSeleccionado.lngIDComitente <> CType(sender, TextBox).Text Then
                        mobjVM.ConsultarDatosComitente(CType(sender, TextBox).Text, CInt(CType(sender, TextBox).Tag))
                    End If
                Else
                    mobjVM.DetalleSeleccionado.lngIDComitente = String.Empty
                    mobjVM.DetalleSeleccionado.strNombre = String.Empty
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información el comitente.", Me.Name, "txtComitenteEncabezado_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.IngresarDetalle()
    End Sub

    Private Sub btnBorrar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarDetalle()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Procesar()
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                mobjVM.DetalleSeleccionado.lngIDComitente = pobjComitente.IdComitente
                mobjVM.DetalleSeleccionado.strNombre = pobjComitente.Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información del comitente.", Me.Name, "BuscadorClienteListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class