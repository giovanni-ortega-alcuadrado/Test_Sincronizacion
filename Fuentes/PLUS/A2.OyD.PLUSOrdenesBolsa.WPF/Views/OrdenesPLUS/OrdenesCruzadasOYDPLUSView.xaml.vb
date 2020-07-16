Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl

Partial Public Class OrdenesCruzadasOYDPLUSView
    Inherits Window
    Dim objVMOrdenes As OrdenesOYDPLUSViewModel
    Dim logCambioValor As Boolean = False
    Dim strNombreControlCambio As String = String.Empty
    Dim logDigitoValor As Boolean = False

    Public Sub New(ByVal objViewModel As OrdenesOYDPLUSViewModel)
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.Resources.Add("VMOrdenes", objViewModel)
            Me.DataContext = objViewModel

            objVMOrdenes = objViewModel
            objVMOrdenes.logOrdenCruzada = True

            InitializeComponent()

            objVMOrdenes.ViewOrdenesCruzadas = Me
            Me.scrollPrinicipal.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.WindowStartupLocation = WindowStartupLocation.CenterScreen
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub OrdenesCruzadasOYDPLUSView_Load(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            'Carga los combos especificos de la aplicación.
            objVMOrdenes.CargarTipoNegocioReceptor(objVMOrdenes.OPCION_ORDENCRUZADA, objVMOrdenes.OrdenOYDPLUSSelected.Receptor, objVMOrdenes.Modulo, objVMOrdenes.OPCION_ORDENCRUZADA)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes cruzadas", Me.Name, "", "OrdenesCruzadasOYDPLUSView_Load", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.actualizarItemOrden(pstrClaseControl, pobjItem)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.ComitenteSeleccionadoCruzada = pobjComitente
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignadoADR(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjComitente) Then
                    Me.objVMOrdenes.OrdenCruzadaSelected.IDComitenteADR = pobjComitente.IdComitente
                    Me.objVMOrdenes.OrdenCruzadaSelected.TipoIdentificacionADR = pobjComitente.TipoIdentificacion
                    Me.objVMOrdenes.OrdenCruzadaSelected.NroDocumentoADR = pobjComitente.NroDocumento
                    Me.objVMOrdenes.OrdenCruzadaSelected.NombreClienteADR = pobjComitente.Nombre
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.ComitenteSeleccionadoCruzada = Nothing
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarClienteADR_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.OrdenCruzadaSelected.IDComitenteADR = Nothing
                Me.objVMOrdenes.OrdenCruzadaSelected.TipoIdentificacionADR = String.Empty
                Me.objVMOrdenes.OrdenCruzadaSelected.NroDocumentoADR = String.Empty
                Me.objVMOrdenes.OrdenCruzadaSelected.NombreClienteADR = String.Empty
                If Me.objVMOrdenes.BorrarClienteADR Then
                    Me.objVMOrdenes.BorrarClienteADR = False
                End If
                Me.objVMOrdenes.BorrarClienteADR = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarClienteADR_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        strNombreControlCambio = String.Empty
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            strNombreControlCambio = CType(sender, TextBox).Name
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            strNombreControlCambio = CType(sender, A2Utilidades.A2NumericBox).Name
        End If
    End Sub

    Private Sub btnCancelar_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            objVMOrdenes.logOrdenCruzada = False
            Me.DialogResult = False
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub btnGuardar_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
                Me.objVMOrdenes.mdtmFechaCierreSistema = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaCierreSistema(Me.objVMOrdenes.MSTR_MODULO_OYD_ORDENES, Program.Usuario)

                If Me.objVMOrdenes.ValidarFechaCierreSistema(Me.objVMOrdenes.OrdenCruzadaSelected, "actualizar") Then
                    Me.objVMOrdenes.IsBusyCruzada = True
                    Me.objVMOrdenes.LimpiarVariablesConfirmadas()
                    Me.objVMOrdenes.CalcularDiasOrdenOYDPLUS(Me.objVMOrdenes.MSTR_CALCULAR_DIAS_ORDEN, Me.objVMOrdenes.OrdenCruzadaSelected, -1, True)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Private Sub txtCalculo_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMOrdenes) Then
                If objVMOrdenes.logEditarRegistro Or objVMOrdenes.logNuevoRegistro Then
                    Dim strTipoControl As String = String.Empty

                    If TypeOf sender Is TextBox Then
                        strTipoControl = CType(sender, TextBox).Tag
                    ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                        strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag
                    End If

                    If objVMOrdenes.logCalcularValores Then
                        logDigitoValor = True
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub txtCalculo_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMOrdenes) Then
                If objVMOrdenes.logEditarRegistro Or objVMOrdenes.logNuevoRegistro Then
                    Dim strTipoControl As String = String.Empty

                    If TypeOf sender Is TextBox Then
                        strTipoControl = CType(sender, TextBox).Tag
                    ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                        strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag
                    End If

                    If objVMOrdenes.logCalcularValores Then
                        logDigitoValor = True
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub txtCalculo_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(objVMOrdenes) Then
                If objVMOrdenes.logEditarRegistro Or objVMOrdenes.logNuevoRegistro And objVMOrdenes.logCalcularValores Then
                    If logDigitoValor Then
                        Dim strTipoControl As String = String.Empty

                        strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag

                        logCambioValor = True
                    Else
                        If CType(sender, A2Utilidades.A2NumericBox).Name = strNombreControlCambio And _
                            e.NewValue <> e.OldValue Then
                            logCambioValor = True
                            strNombreControlCambio = String.Empty
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtCalculo_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If objVMOrdenes.logEditarRegistro Or objVMOrdenes.logNuevoRegistro Then
                    If logCambioValor And objVMOrdenes.logCalcularValores Then
                        Dim strTipoControl As String = String.Empty

                        If TypeOf sender Is TextBox Then
                            strTipoControl = CType(sender, TextBox).Tag
                        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                            strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag
                        End If

                        If strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.CANTIDAD.ToString Then
                            If objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_SIMULTANEA Or objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPOC Then
                                objVMOrdenes.LimpiarControlesOYDPLUS(OrdenesOYDPLUSViewModel.OPCION_CANTIDAD, objVMOrdenes.OrdenCruzadaSelected)
                                strTipoControl = String.Empty
                            End If
                        ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.TASAREGISTRO.ToString Then
                            'objVMOrdenes.VerificarTasaRegistro_TasaCliente(objVMOrdenes.OrdenCruzadaSelected)
                            If objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_SIMULTANEA Or objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPOC Then
                                strTipoControl = String.Empty
                            End If
                            'ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.TASACLIENTE.ToString Then
                            'objVMOrdenes.VerificarTasaRegistro_TasaCliente(objVMOrdenes.OrdenCruzadaSelected)
                        ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.COMISION.ToString Then
                            objVMOrdenes.VerificarComision(objVMOrdenes.OrdenCruzadaSelected)
                        ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.PRECIOMAXIMOMINIMO.ToString Then
                            If objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_ACCIONES Or _
                                objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_ADR Then
                                If objVMOrdenes.OrdenCruzadaSelected.Precio = 0 Or _
                                    IsNothing(objVMOrdenes.OrdenCruzadaSelected.Precio) Then
                                    objVMOrdenes.OrdenCruzadaSelected.Precio = objVMOrdenes.OrdenCruzadaSelected.PrecioMaximoMinimo
                                End If
                                strTipoControl = String.Empty
                            End If
                        End If

                        objVMOrdenes.objTipoCalculo = objVMOrdenes.VerificarTipoCalculo(objVMOrdenes.OrdenCruzadaSelected, objVMOrdenes.objTipoCalculo, strTipoControl)


                        logCambioValor = False
                        logDigitoValor = False
                        Await objVMOrdenes.CalcularValorOrden(objVMOrdenes.OrdenCruzadaSelected)
                        ReubicarFocoControl(sender)
                    End If

                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtCalculo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtDiasCumplimiento_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If objVMOrdenes.logEditarRegistro Or objVMOrdenes.logNuevoRegistro Then
                    If logCambioValor And objVMOrdenes.logCalcularValores Then
                        logCambioValor = False
                        objVMOrdenes.CalcularFechaCumplimientoXDias(objVMOrdenes.OrdenCruzadaSelected)
                        ReubicarFocoControl(sender)
                    End If

                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtDiasCumplimiento_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ReubicarFocoControl(ByVal pobjControl As Object)
        Try
            Dim strNombreControl As String = String.Empty

            If TypeOf pobjControl Is TextBox Then
                strNombreControl = CType(pobjControl, TextBox).Name
            ElseIf TypeOf pobjControl Is A2Utilidades.A2NumericBox Then
                strNombreControl = CType(pobjControl, A2Utilidades.A2NumericBox).Name
            End If

            If objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_ACCIONES Or objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_ADR Then
                If strNombreControl = "txtCantidadAcciones" Then
                    txtPrecioAcciones.Focus()
                ElseIf strNombreControl = "txtPrecioAcciones" Then
                    txtPrecioMaximoMinimo.Focus()
                ElseIf strNombreControl = "txtPrecioMaximoMinimo" Then
                    txtComisionAcciones.Focus()
                ElseIf strNombreControl = "txtComisionAcciones" Then
                    txtValorComisionAcciones.Focus()
                ElseIf strNombreControl = "txtValorComisionAcciones" Then
                    If objVMOrdenes.HabilitarValorOrden Then
                        txtValorAcciones.Focus()
                    Else
                        dtpFechaRecepcionAcciones.Focus()
                    End If
                ElseIf strNombreControl = "txtValorAcciones" Then
                    dtpFechaRecepcionAcciones.Focus()
                End If
            ElseIf objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_RENTAFIJA Then
                If strNombreControl = "txtCantidadRentaFija" Then
                    txtPrecioRentaFija.Focus()
                ElseIf strNombreControl = "txtPrecioRentaFija" Then
                    txtTasaRegistroRentaFija.Focus()
                ElseIf strNombreControl = "txtTasaRegistroRentaFija" Then
                    txtTasaClienteRentaFija.Focus()
                ElseIf strNombreControl = "txtTasaClienteRentaFija" Then
                    txtComisionRentaFija.Focus()
                ElseIf strNombreControl = "txtComisionRentaFija" Then
                    txtValorComisionRentaFija.Focus()
                ElseIf strNombreControl = "txtValorComisionRentaFija" Then
                    If objVMOrdenes.HabilitarValorOrden Then
                        txtValorRentaFija.Focus()
                    Else
                        dtpFechaRecepcionRentaFija.Focus()
                    End If
                ElseIf strNombreControl = "txtValorRentaFija" Then
                    dtpFechaRecepcionRentaFija.Focus()
                End If
            ElseIf objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPO Then
                If strNombreControl = "txtCantidadRepo" Then
                    txtPrecioRepo.Focus()
                ElseIf strNombreControl = "txtPrecioRepo" Then
                    txtCastigoRepo.Focus()
                ElseIf strNombreControl = "txtCastigoRepo" Then
                    txtTasaRegistroRepo.Focus()
                ElseIf strNombreControl = "txtTasaRegistroRepo" Then
                    txtTasaClienteRepo.Focus()
                ElseIf strNombreControl = "txtTasaClienteRepo" Then
                    txtComisionRepo.Focus()
                ElseIf strNombreControl = "txtComisionRepo" Then
                    txtValorComisionRepo.Focus()
                ElseIf strNombreControl = "txtValorComisionRepo" Then
                    If objVMOrdenes.HabilitarValorOrden Then
                        txtValorRepo.Focus()
                    Else
                        dtpFechaRecepcionRepo.Focus()
                    End If
                ElseIf strNombreControl = "txtValorRepo" Then
                    dtpFechaRecepcionRepo.Focus()
                End If
            ElseIf objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPOC Then
                If strNombreControl = "txtCantidadRepoRentaFija" Then
                    txtPrecioRepoRentaFija.Focus()
                ElseIf strNombreControl = "txtPrecioRepoRentaFija" Then
                    txtPrecioConGarantiaRepoRentaFija.Focus()
                ElseIf strNombreControl = "txtPrecioConGarantiaRepoRentaFija" Then
                    txtTasaRegistroRepoRentaFija.Focus()
                ElseIf strNombreControl = "txtTasaRegistroRepoRentaFija" Then
                    txtTasaClienteRepoRentaFija.Focus()
                ElseIf strNombreControl = "txtTasaClienteRepoRentaFija" Then
                    txtComisionRepoRentaFija.Focus()
                ElseIf strNombreControl = "txtComisionRepoRentaFija" Then
                    txtValorComisionRepoRentaFija.Focus()
                ElseIf strNombreControl = "txtValorComisionRepoRentaFija" Then
                    dtpFechaRecepcionRepoRentaFija.Focus()
                End If
            ElseIf objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_SIMULTANEA Then
                If strNombreControl = "txtCantidadSimultanea" Then
                    txtPrecioSimultanea.Focus()
                ElseIf strNombreControl = "txtPrecioSimultanea" Then
                    txtPrecioConGarantiaSimultanea.Focus()
                ElseIf strNombreControl = "txtPrecioConGarantiaSimultanea" Then
                    txtTasaRegistroSimultanea.Focus()
                ElseIf strNombreControl = "txtTasaRegistroSimultanea" Then
                    txtTasaClienteSimultanea.Focus()
                ElseIf strNombreControl = "txtTasaClienteSimultanea" Then
                    txtComisionSimultanea.Focus()
                ElseIf strNombreControl = "txtComisionSimultanea" Then
                    txtValorComisionSimultanea.Focus()
                ElseIf strNombreControl = "txtValorComisionSimultanea" Then
                    dtpFechaRecepcionSimultanea.Focus()
                End If
            ElseIf objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_TTV Then
                If strNombreControl = "txtCantidadTTV" Then
                    txtPrecioTTV.Focus()
                ElseIf strNombreControl = "txtPrecioTTV" Then
                    txtPorcentajeGarantiaTTV.Focus()
                ElseIf strNombreControl = "txtPorcentajeGarantiaTTV" Then
                    txtTasaRegistroTTV.Focus()
                ElseIf strNombreControl = "txtTasaRegistroTTV" Then
                    txtTasaClienteTTV.Focus()
                ElseIf strNombreControl = "txtTasaClienteTTV" Then
                    txtComisionTTV.Focus()
                ElseIf strNombreControl = "txtComisionTTV" Then
                    txtValorComisionTTV.Focus()
                ElseIf strNombreControl = "txtValorComisionTTV" Then
                    dtpFechaRecepcionTTV.Focus()
                End If
            ElseIf objVMOrdenes.OrdenCruzadaSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_TTVC Then
                If strNombreControl = "txtCantidadTTVRentaFija" Then
                    txtPrecioTTVRentaFija.Focus()
                ElseIf strNombreControl = "txtPrecioTTVRentaFija" Then
                    txtPrecioMaximoMinimoTTVRentaFija.Focus()
                ElseIf strNombreControl = "txtPrecioMaximoMinimoTTVRentaFija" Then
                    txtTasaRegistroTTVRentaFija.Focus()
                ElseIf strNombreControl = "txtTasaRegistroTTVRentaFija" Then
                    txtTasaClienteTTVRentaFija.Focus()
                ElseIf strNombreControl = "txtTasaClienteTTVRentaFija" Then
                    txtComisionTTVRentaFija.Focus()
                ElseIf strNombreControl = "txtComisionTTVRentaFija" Then
                    txtValorComisionTTVRentaFija.Focus()
                ElseIf strNombreControl = "txtValorComisionSTTVRentaFija" Then
                    dtpFechaRecepcionTTVRentaFija.Focus()
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "ReubicarFocoControl", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TimePicker_MouseLeave_1(sender As Object, e As MouseEventArgs)
        timepicker.Focus()
    End Sub

End Class
