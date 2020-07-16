Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2ComunesImportaciones


Partial Public Class TitulosView
    Inherits UserControl


    ''' <summary>
    ''' Eventos creados para la comunicación con las clases TitulosView y TitulosViewModel
    ''' Pantalla Titulos
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 5 de Enero 2015</remarks>
#Region "Variables"

    Private mobjVM As TitulosViewModel
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
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New TitulosViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                

                inicializar()

                'AddHandler Application.Current.Exit, AddressOf View_UnloadedHost
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            If Not IsNothing(mlogInicializar) Then
                Await Me.mobjVM.CerrarPantalla()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de títulos desde la pestaña.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, TitulosViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.ViewTitulos = Me

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
            ElseIf TypeOf sender Is Telerik.Windows.Controls.RadNumericUpDown Then
                'CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Select(0, CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ucBtnDialogoImportar_SubirArchivo(sender As Object, e As RoutedEventArgs)
        'mobjVM.IsBusy = True
    End Sub

    Private Sub ucBtnDialogoImportar_CargarArchivo(sender As A2ComunesControl.ObjetoInformacionArchivo, strProceso As String)
        Try
            Dim objDialog = CType(sender, ObjetoInformacionArchivo)

            If Not IsNothing(objDialog.pFile) Then
                'If objDialog.pFile.File.Extension.Equals(".csv") Or objDialog.pFile.File.Extension.Equals(".xls") Or objDialog.pFile.File.Extension.Equals(".xlsx") Then
                Dim strRutaArchivo As String = objDialog.pFile.FileName

                Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, TitulosViewModel), strRutaArchivo, strProceso, mobjVM.STRARCH_TITULOS_MVTOS, False)
                viewImportacion.Show()

                'Else
                '    A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores, Program.Usuario)
                'End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "CargarArchivoGenerico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos para asignar cliente y especie"

    ' ''' <summary>
    ' ''' Se ejecuta cuando se dispara el evento comitenteAsignado en el buscador de clientes (control buscador clientes lista)
    ' ''' </summary>
    ' ''' <param name="pstrClaseControl">Permite identificar el llamado</param>
    ' ''' <param name="pobjComitente">Datos del comitente seleccionado en el buscador</param>
    ' ''' <remarks></remarks>
    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        'Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                Me.mobjVM.lngIDComitente = pobjComitente.CodigoOYD
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'Private Sub ctrlEspecie_nemotecnicoAsignado(pstrNemotecnico As System.String, pstrNombreNemotecnico As System.String)
    '    Try
    '        If Not IsNothing(mobjVM) Then
    '            Me.mobjVM.strIdEspecie = pstrNemotecnico
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del nemotécnico", Me.Name, "ctrlEspecie_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '    End Try
    'End Sub
    Private Sub ctrlEspecie_nemotecnicoAsignado(pstrClaseControl As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                Select Case pstrClaseControl.ToLower()
                    Case "nemotecnico"
                        Me.mobjVM.strIdEspecie = pobjEspecie.Nemotecnico
                    Case "nemotecnicobuscar"
                        'Me.mobjVM.cb.NemotecnicoSeleccionado = pobjEspecie
                        'Me.mobjVM.cb.Nemotecnico = pobjEspecie.Nemotecnico
                        Me.mobjVM.CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del nemotécnico", Me.Name, "ctrlEspecie_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.BorrarCliente = True
                Me.mobjVM.lngIDComitente = Nothing
                Me.mobjVM.BorrarCliente = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.BorrarEspecie = True
                Me.mobjVM.strIdEspecie = Nothing
                Me.mobjVM.BorrarEspecie = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtFiltroCompra_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                Await mobjVM.FiltrarCompras(Me.txtFiltroCompras.Text)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "txtFiltroCompra_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtFiltroCompra_KeyUp(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                Await mobjVM.FiltrarCompras(Me.txtFiltroCompras.Text)
            ElseIf e.Key = Key.Back Then
                If Not IsNothing(e) Then
                    If Not IsNothing(e.OriginalSource) Then
                        Dim strTexto As String = CType(e.OriginalSource, System.Windows.Controls.TextBox).Text
                        If Me.txtFiltroCompras.Text <> strTexto Then
                            Me.txtFiltroCompras.Text = strTexto
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "txtFiltroCompra_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub btnFiltroCompra_Click(sender As Object, e As RoutedEventArgs)
        Try
            Await mobjVM.FiltrarCompras(Me.txtFiltroCompras.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al aplicar el filtro del ISIN", Me.Name, "btnFiltroCompra_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtFiltroVenta_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                Await mobjVM.FiltrarVentas(Me.txtFiltroVentas.Text)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "txtFiltroVenta_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtFiltroVenta_KeyUp(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                Await mobjVM.FiltrarVentas(Me.txtFiltroVentas.Text)
            ElseIf e.Key = Key.Back Then
                If Not IsNothing(e) Then
                    If Not IsNothing(e.OriginalSource) Then
                        Dim strTexto As String = CType(e.OriginalSource, System.Windows.Controls.TextBox).Text
                        If Me.txtFiltroVentas.Text <> strTexto Then
                            Me.txtFiltroVentas.Text = strTexto
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "txtFiltroVenta_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub btnFiltroVenta_Click(sender As Object, e As RoutedEventArgs)
        Try
            Await mobjVM.FiltrarVentas(Me.txtFiltroVentas.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al aplicar el filtro del ISIN", Me.Name, "btnFiltroVenta_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtFiltroCruce_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                mobjVM.FiltrarCruces(Me.txtFiltroCruce.Text)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "txtFiltroCruce_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtFiltroCruce_KeyUp(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                mobjVM.FiltrarCruces(Me.txtFiltroCruce.Text)
            ElseIf e.Key = Key.Back Then
                If Not IsNothing(e) Then
                    If Not IsNothing(e.OriginalSource) Then
                        Dim strTexto As String = CType(e.OriginalSource, System.Windows.Controls.TextBox).Text
                        If Me.txtFiltroCruce.Text <> strTexto Then
                            Me.txtFiltroCruce.Text = strTexto
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar las especies que cumplan con el filtro indicado.", Me.Name, "txtFiltroCruce_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnFiltroCruce_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.FiltrarCruces(Me.txtFiltroCruce.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al aplicar el filtro del ISIN", Me.Name, "btnFiltroCruce_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarFiltroCompra_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.txtFiltroCompras.Text = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarFiltroCompra_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarFiltroVenta_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.txtFiltroVentas.Text = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarFiltroVenta_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarFiltroCruce_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.txtFiltroCruce.Text = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarFiltroVenta_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

End Class
