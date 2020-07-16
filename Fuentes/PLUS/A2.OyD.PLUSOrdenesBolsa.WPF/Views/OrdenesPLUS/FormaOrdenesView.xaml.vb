Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class FormaOrdenesView
    Inherits UserControl
    Dim objVMOrdenes As OrdenesOYDPLUSViewModel
    Dim logCambioValor As Boolean = False
    Dim strNombreControlCambio As String = String.Empty
    Dim logDigitoValor As Boolean = False
    Private strClaseCombos As String = String.Empty
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New(ByVal pobjVMOrdenes As OrdenesOYDPLUSViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Dim objA2VM As A2UtilsViewModel
            objVMOrdenes = pobjVMOrdenes

            If Me.Resources.Contains("VMOrdenes") Then
                Me.Resources.Remove("VMOrdenes")
            End If

            Me.Resources.Add("VMOrdenes", objVMOrdenes)
            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)
            CType(Me.Resources("A2VM"), A2UtilsViewModel).actualizarCombosEspecificos("Ord_Acciones")
            InitializeComponent()
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
            Me.stackDistribucionComisiones.Width = Application.Current.MainWindow.ActualWidth * 0.94
            Me.StackReceptoresCruzarCon.Width = Application.Current.MainWindow.ActualWidth * 0.94


        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOrdenesView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.stackDistribucionComisiones.Width = Application.Current.MainWindow.ActualWidth * 0.94
        Me.StackReceptoresCruzarCon.Width = Application.Current.MainWindow.ActualWidth * 0.94
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

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda_CruzadaCon(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjItem) Then
                    objVMOrdenes.ObtenerReceptorCruzada(pobjItem.IdItem, pobjItem.Nombre)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Dim logRecalcularValoresCambioComitente As Boolean = False

                If Not IsNothing(pobjComitente) And Not IsNothing(Me.objVMOrdenes.OrdenOYDPLUSSelected) Then
                    If (Me.objVMOrdenes.logEditarRegistro Or Me.objVMOrdenes.logDuplicarRegistro) And Me.objVMOrdenes.Editando And Me.objVMOrdenes.logCalculosValoresBancolombia Then
                        If Trim(Me.objVMOrdenes.OrdenOYDPLUSSelected.IDComitente) <> Trim(pobjComitente.IdComitente) Then
                            logRecalcularValoresCambioComitente = True
                        End If
                    End If
                End If

                Me.objVMOrdenes.ComitenteSeleccionadoOYDPLUS = pobjComitente

                If logRecalcularValoresCambioComitente Then
                    Await Me.objVMOrdenes.CalcularValorOrden(Me.objVMOrdenes.OrdenOYDPLUSSelected)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignadoADR(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjComitente) Then
                    Me.objVMOrdenes.OrdenOYDPLUSSelected.IDComitenteADR = pobjComitente.IdComitente
                    Me.objVMOrdenes.OrdenOYDPLUSSelected.TipoIdentificacionADR = pobjComitente.TipoIdentificacion
                    Me.objVMOrdenes.OrdenOYDPLUSSelected.NroDocumentoADR = pobjComitente.NroDocumento
                    Me.objVMOrdenes.OrdenOYDPLUSSelected.NombreClienteADR = pobjComitente.Nombre
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_EspecieAgrupadaAsignado(pobjNemotecnico As EspeciesAgrupadas, pstrNemotecnico As String)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjNemotecnico) Then
                    objVMOrdenes.OrdenOYDPLUSSelected.Especie = pstrNemotecnico
                    objVMOrdenes.OrdenOYDPLUSSelected.TipoTasaFija = pobjNemotecnico.CodTipoTasaFija
                    objVMOrdenes.OrdenOYDPLUSSelected.EspecieEsAccion = pobjNemotecnico.EsAccion
                    objVMOrdenes.HabilitarControlesEspecieOYDPLUS(pobjNemotecnico)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_especieAsignada(pstrNemotecnico As System.String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjEspecie) Then
                    Me.objVMOrdenes.NemotecnicoSeleccionadoOYDPLUS = pobjEspecie
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_especieAsignada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.ComitenteSeleccionadoOYDPLUS = Nothing
                If Me.objVMOrdenes.BorrarCliente Then
                    Me.objVMOrdenes.BorrarCliente = False
                End If
                Me.objVMOrdenes.BorrarCliente = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarClienteADR_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.OrdenOYDPLUSSelected.IDComitenteADR = Nothing
                Me.objVMOrdenes.OrdenOYDPLUSSelected.TipoIdentificacionADR = String.Empty
                Me.objVMOrdenes.OrdenOYDPLUSSelected.NroDocumentoADR = String.Empty
                Me.objVMOrdenes.OrdenOYDPLUSSelected.NombreClienteADR = String.Empty
                If Me.objVMOrdenes.BorrarClienteADR Then
                    Me.objVMOrdenes.BorrarClienteADR = False
                End If
                Me.objVMOrdenes.BorrarClienteADR = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarClienteADR_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then

                'Si la orden es directa se refresca de nuevo la lista y se controla con el IsBusy hasta que regrese del servidor
                'Julian Rincon (Alcuadrado S.A)
                If (objVMOrdenes.OrdenOYDPLUSSelected.TipoOrden = objVMOrdenes.TIPOORDEN_DIRECTA) Then
                    objVMOrdenes.IsBusy = True
                    Await objVMOrdenes.ConsultarOrdenesSAEControl()
                    objVMOrdenes.IsBusy = False
                End If

                Me.objVMOrdenes.NemotecnicoSeleccionadoOYDPLUS = Nothing

            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
                        logCambioValor = True
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCalculo_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(objVMOrdenes) Then
                If objVMOrdenes.logEditarRegistro Or objVMOrdenes.logNuevoRegistro Then
                    If objVMOrdenes.logCalcularValores Then
                        logCambioValor = True
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

                        If CType(sender, A2Utilidades.A2NumericBox).Name = "txtCantidadRepo" Then
                            objVMOrdenes.IsBusy = True
                            If (objVMOrdenes.logNuevoRegistro) Or (objVMOrdenes.logDuplicarRegistro) Then
                                If Not IsNothing(objVMOrdenes.ListaOrdenSAERentaFija) Then
                                    If objVMOrdenes.ListaOrdenSAERentaFija.Count > 0 Then
                                        If objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPO And
                                            objVMOrdenes.OrdenOYDPLUSSelected.TipoOperacion = objVMOrdenes.TIPOOPERACION_VENTA And
                                            objVMOrdenes.OrdenOYDPLUSSelected.TipoOrden = objVMOrdenes.TIPOORDEN_DIRECTA Then
                                            If Not IsNothing(objVMOrdenes.dblCantidadAnterior) Then
                                                If objVMOrdenes.OrdenOYDPLUSSelected.Cantidad <> objVMOrdenes.ListaOrdenSAERentaFija.Sum(Function(i) i.Cantidad And i.Seleccionada = True).Value Then
                                                    mostrarMensaje("En una orden Repo venta la cantidad no puede ser diferente a la suma de las cantidades de los folios seleccionados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                    'objVMOrdenes.OrdenOYDPLUSSelected.Cantidad = objVMOrdenes.dblCantidadAnterior
                                                    CType(sender, A2Utilidades.A2NumericBox).Value = objVMOrdenes.ListaOrdenSAERentaFija.Sum(Function(i) i.Cantidad And i.Seleccionada = True).Value
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Else
                                If objVMOrdenes.logEditarRegistro And objVMOrdenes.logDuplicarRegistro = False Then
                                    If Not IsNothing(objVMOrdenes.dblCantidadAnterior) Then
                                        If objVMOrdenes.OrdenOYDPLUSSelected.Cantidad > objVMOrdenes.dblCantidadAnterior And objVMOrdenes.OrdenOYDPLUSSelected.TipoOperacion = objVMOrdenes.TIPOOPERACION_COMPRA And objVMOrdenes.OrdenOYDPLUSSelected.TipoOrden = objVMOrdenes.TIPOORDEN_DIRECTA Then
                                            mostrarMensaje("La cantidad no se puede modificar por un valor mayor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                            CType(sender, A2Utilidades.A2NumericBox).Value = objVMOrdenes.dblCantidadAnterior
                                        ElseIf objVMOrdenes.OrdenOYDPLUSSelected.Cantidad <> objVMOrdenes.dblCantidadAnterior And objVMOrdenes.OrdenOYDPLUSSelected.TipoOperacion = objVMOrdenes.TIPOOPERACION_VENTA And objVMOrdenes.OrdenOYDPLUSSelected.TipoOrden = objVMOrdenes.TIPOORDEN_DIRECTA Then
                                            mostrarMensaje("La cantidad no se puede modificar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                            CType(sender, A2Utilidades.A2NumericBox).Value = objVMOrdenes.dblCantidadAnterior
                                        End If
                                    End If
                                End If
                            End If

                        End If

                        If strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.CANTIDAD.ToString Then
                            If objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_SIMULTANEA Or objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPOC Then
                                objVMOrdenes.LimpiarControlesOYDPLUS(OrdenesOYDPLUSViewModel.OPCION_CANTIDAD, objVMOrdenes.OrdenOYDPLUSSelected)
                                strTipoControl = String.Empty
                            End If
                        ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.TASAREGISTRO.ToString Then
                            'objVMOrdenes.VerificarTasaRegistro_TasaCliente(objVMOrdenes.OrdenOYDPLUSSelected)
                            If objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_SIMULTANEA Or objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPOC Then
                                strTipoControl = String.Empty
                            End If
                            'ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.TASACLIENTE.ToString Then
                            'objVMOrdenes.VerificarTasaRegistro_TasaCliente(objVMOrdenes.OrdenOYDPLUSSelected)
                        ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.COMISION.ToString Then
                            objVMOrdenes.VerificarComision(objVMOrdenes.OrdenOYDPLUSSelected)
                        ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.PRECIOMAXIMOMINIMO.ToString Then
                            If objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_ACCIONES Or
                                objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_ADR Then
                                If objVMOrdenes.OrdenOYDPLUSSelected.Precio = 0 Or
                                    IsNothing(objVMOrdenes.OrdenOYDPLUSSelected.Precio) Then
                                    objVMOrdenes.OrdenOYDPLUSSelected.Precio = objVMOrdenes.OrdenOYDPLUSSelected.PrecioMaximoMinimo
                                End If
                                strTipoControl = String.Empty
                            End If
                        ElseIf strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.TASACLIENTE.ToString Then
                            If objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_TTV And (String.IsNullOrEmpty(objVMOrdenes.OrdenOYDPLUSSelected.TasaCliente.ToString) Or (objVMOrdenes.OrdenOYDPLUSSelected.TasaCliente.ToString = "0") And Not (String.IsNullOrEmpty(objVMOrdenes.OrdenOYDPLUSSelected.TasaRegistro.ToString) Or (objVMOrdenes.OrdenOYDPLUSSelected.TasaRegistro.ToString = "0"))) Then
                                strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.COMISION.ToString
                            ElseIf objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_TTV And (String.IsNullOrEmpty(objVMOrdenes.OrdenOYDPLUSSelected.TasaRegistro.ToString) Or (objVMOrdenes.OrdenOYDPLUSSelected.TasaRegistro.ToString = "0") And Not (String.IsNullOrEmpty(objVMOrdenes.OrdenOYDPLUSSelected.TasaCliente.ToString) Or (objVMOrdenes.OrdenOYDPLUSSelected.TasaCliente.ToString = "0"))) Then
                                mostrarMensaje("No se puede modificar la TASA NETA, sin haber modificado la TASA OPERACIÓN", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                                'ElseIf objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_TTV And Not (String.IsNullOrEmpty(objVMOrdenes.OrdenOYDPLUSSelected.TasaRegistro.ToString) Or (objVMOrdenes.OrdenOYDPLUSSelected.TasaRegistro.ToString = "0") And Not (String.IsNullOrEmpty(objVMOrdenes.OrdenOYDPLUSSelected.TasaCliente.ToString) Or (objVMOrdenes.OrdenOYDPLUSSelected.TasaCliente.ToString = "0"))) Then
                                'strTipoControl = OrdenesOYDPLUSViewModel.TIPOCALCULOS_MOTOR.TASACLIENTE.ToString
                            End If
                        End If

                        objVMOrdenes.objTipoCalculo = objVMOrdenes.VerificarTipoCalculo(objVMOrdenes.OrdenOYDPLUSSelected, objVMOrdenes.objTipoCalculo, strTipoControl)

                        logCambioValor = False
                        Await objVMOrdenes.CalcularValorOrden(objVMOrdenes.OrdenOYDPLUSSelected)
                        If CType(sender, A2Utilidades.A2NumericBox).Name = "txtCantidadRepo" Then
                            If objVMOrdenes.IsBusy Then
                                objVMOrdenes.IsBusy = False
                            End If
                        End If
                        ReubicarFocoControl(sender)
                    End If

                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtCalculo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtDiasCumplimiento_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If objVMOrdenes.logEditarRegistro Or objVMOrdenes.logNuevoRegistro Then
                    If logCambioValor And objVMOrdenes.logCalcularValores Then
                        logCambioValor = False
                        objVMOrdenes.CalcularFechaCumplimientoXDias(objVMOrdenes.OrdenOYDPLUSSelected)
                        ReubicarFocoControl(sender)
                        Me.objVMOrdenes.CalcularFechaCumplimientoPorDias(objVMOrdenes.OrdenOYDPLUSSelected)

                        'Await Me.objVMOrdenes.CalcularValorOrden(objVMOrdenes.OrdenOYDPLUSSelected)
                        'Await CalcularValorOrden(_OrdenOYDPLUSSelected)
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

            If objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_ACCIONES Or objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_ADR Then
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
            ElseIf objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_RENTAFIJA Then
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
            ElseIf objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPO Then
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
            ElseIf objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_REPOC Then
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
            ElseIf objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_SIMULTANEA Then
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
            ElseIf objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_TTV Then
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
            ElseIf objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_TTVC Then
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



    '*****************************************************************************************************************
    'Inicio Segmento código obtener ordenes SAE - Genérico pantalla Ordenes 
    'Julian Rincón (Alcuadrado S.A)
    '*****************************************************************************************************************

    Private Async Sub btnRefrescar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then

                Await objVMOrdenes.ConsultarOrdenesSAEControl()

                objVMOrdenes.FiltrarOperacionesEspecieCumplimiento()

            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutarmMétodo ConsultarOrdenesSAEControl", Me.Name, "btnLimpiarClienteADR_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

	'*****************************************************************************************************************
	'Inicio Segmento código obtener ordenes SAE - Genérico pantalla Ordenes 
	'Julian Rincón (Alcuadrado S.A)
	'*****************************************************************************************************************
	Private Sub btnConsultarPlantillas_Click(sender As Object, e As RoutedEventArgs)
		objVMOrdenes.PreguntarAbrirConsultaPlantillasOrden()
	End Sub

	Private Sub btnGenerarPlantilla_Click(sender As Object, e As RoutedEventArgs)
		objVMOrdenes.PreguntarGenerarPlantillaOrden()
	End Sub

	Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
		objVMOrdenes.PreguntarDuplicarOrden()
	End Sub

End Class
