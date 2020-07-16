Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class FormaOperacionesOtrosNegociosView
    Inherits UserControl
    Dim objVMOperaciones As OperacionesOtrosNegociosViewModel
    Dim logDigitoValor As Boolean = False
    Dim logCambioValor As Boolean = False
    Dim strNombreControlCambio As String = String.Empty

    Public Sub New(ByVal pobjVMOperaciones As OperacionesOtrosNegociosViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMOperaciones = pobjVMOperaciones

            If Me.Resources.Contains("VMOperaciones") Then
                Me.Resources.Remove("VMOperaciones")
            End If

            Me.Resources.Add("VMOperaciones", pobjVMOperaciones)
            InitializeComponent()

            Me.stackDistribucionComisiones.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.stackDistribucionComisiones.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.95
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMOperaciones) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl.ToUpper = "PAISES" Then
                        objVMOperaciones.EncabezadoSeleccionado.PaisNegociacion = pobjItem.IdItem
                        objVMOperaciones.EncabezadoSeleccionado.CodigoPaisNegociacion = pobjItem.CodItem
                        objVMOperaciones.EncabezadoSeleccionado.NombrePaisNegociacion = pobjItem.Nombre
                    ElseIf pstrClaseControl.ToUpper = "CONTRAPARTE" Then
                        objVMOperaciones.EncabezadoSeleccionado.IDContraparte = pobjItem.IdItem
                        objVMOperaciones.EncabezadoSeleccionado.NombreContraparte = pobjItem.Nombre
                        objVMOperaciones.EncabezadoSeleccionado.CodTipoIdentificacionContraparte = pobjItem.InfoAdicional01
                        objVMOperaciones.EncabezadoSeleccionado.TipoIdentificacionContraparte = pobjItem.InfoAdicional02
                        objVMOperaciones.EncabezadoSeleccionado.NumeroDocumentoContraparte = pobjItem.CodItem

                        If objVMOperaciones.VerificarTipoFuncionalidadTipoOrigen(objVMOperaciones.EncabezadoSeleccionado.TipoOrigen, OperacionesOtrosNegociosViewModel.TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACONTRAPARTE) Then
                            objVMOperaciones.consultarComitentesContraparte(objVMOperaciones.EncabezadoSeleccionado.IDContraparte)
                        End If
                    ElseIf pstrClaseControl.ToUpper = "PAGADOR" Then
                        objVMOperaciones.EncabezadoSeleccionado.IDPagador = pobjItem.IdItem
                        objVMOperaciones.EncabezadoSeleccionado.NombrePagador = pobjItem.Nombre
                        objVMOperaciones.EncabezadoSeleccionado.CodTipoIdentificacionPagador = pobjItem.InfoAdicional01
                        objVMOperaciones.EncabezadoSeleccionado.TipoIdentificacionPagador = pobjItem.InfoAdicional02
                        objVMOperaciones.EncabezadoSeleccionado.NumeroDocumentoPagador = pobjItem.CodItem
                    ElseIf pstrClaseControl.ToUpper = "MONEDA" Then
                        objVMOperaciones.EncabezadoSeleccionado.IDMoneda = pobjItem.IdItem
                        objVMOperaciones.EncabezadoSeleccionado.CodigoMoneda = pobjItem.CodItem
                        objVMOperaciones.EncabezadoSeleccionado.NombreMoneda = pobjItem.Nombre

                        If objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_RENTAVARIABLE Then
                            txtNominalRentaVariable.Focus()
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_RENTAFIJA Then
                            txtNominalRentaFija.Focus()
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_REPO Then
                            If objVMOperaciones.EncabezadoSeleccionado.Tipo = objVMOperaciones.CLASE_ACCIONES Then
                                txtNominalREPOAcciones.Focus()
                            Else
                                If objVMOperaciones.EncabezadoSeleccionado.TipoRepo = objVMOperaciones.TIPOREPO_ABIERTO Then
                                    txtNominalRepoAbierto.Focus()
                                Else
                                    txtNominalRepo.Focus()
                                End If
                            End If
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_SIMULTANEA Then
                            txtNominalSimultanea.Focus()
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_TTV Then
                            If objVMOperaciones.EncabezadoSeleccionado.Tipo = objVMOperaciones.CLASE_ACCIONES Then
                                txtNominalTTVAcciones.Focus()
                            Else
                                txtNominalTTV.Focus()
                            End If
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_DEPOSITOREMUNERADO Then
                            txtNominalDepositoRemunerado.Focus()
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_DIVISAS Then
                            txtNominalDivisas.Focus()
                        End If
                    ElseIf pstrClaseControl.ToUpper = "DETALLERECEPTOR" Then
                        objVMOperaciones.ReceptorSeleccionado.Receptor = pobjItem.CodItem
                        objVMOperaciones.ReceptorSeleccionado.NombreReceptor = pobjItem.Nombre
                    End If
                Else
                    If pstrClaseControl.ToUpper = "MONEDA" Then
                        If objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_RENTAVARIABLE Then
                            txtNominalRentaVariable.Focus()
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_RENTAFIJA Then
                            txtNominalRentaFija.Focus()
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_REPO Then
                            If objVMOperaciones.EncabezadoSeleccionado.Tipo = objVMOperaciones.CLASE_ACCIONES Then
                                txtNominalREPOAcciones.Focus()
                            Else
                                If objVMOperaciones.EncabezadoSeleccionado.TipoRepo = objVMOperaciones.TIPOREPO_ABIERTO Then
                                    txtNominalRepoAbierto.Focus()
                                Else
                                    txtNominalRepo.Focus()
                                End If
                            End If
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_SIMULTANEA Then
                            txtNominalSimultanea.Focus()
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_TTV Then
                            If objVMOperaciones.EncabezadoSeleccionado.Tipo = objVMOperaciones.CLASE_ACCIONES Then
                                txtNominalTTVAcciones.Focus()
                            Else
                                txtNominalTTV.Focus()
                            End If
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_DEPOSITOREMUNERADO Then
                            txtNominalDepositoRemunerado.Focus()
                        ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_DIVISAS Then
                            txtNominalDivisas.Focus()
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMOperaciones) Then
                If Not IsNothing(pobjComitente) Then
                    objVMOperaciones.ComitenteSeleccionado = pobjComitente
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_nemotecnicoAsignado(pstrNemotecnico As System.String, pstrNombreNemotecnico As System.String)
        Try
            If Not IsNothing(objVMOperaciones) Then
                If Not String.IsNullOrEmpty(pstrNemotecnico) Then
                    objVMOperaciones.logCalcularValores = False
                    objVMOperaciones.EncabezadoSeleccionado.Nemotecnico = pstrNemotecnico
                    objVMOperaciones.EncabezadoSeleccionado.ISIN = String.Empty
                    objVMOperaciones.EncabezadoSeleccionado.FechaEmision = Nothing
                    objVMOperaciones.EncabezadoSeleccionado.FechaVencimiento = Nothing
                    objVMOperaciones.EncabezadoSeleccionado.Modalidad = String.Empty
                    objVMOperaciones.EncabezadoSeleccionado.TasaFacial = 0
                    objVMOperaciones.EncabezadoSeleccionado.IndicadorEconomico = String.Empty
                    objVMOperaciones.EncabezadoSeleccionado.PuntosIndicador = 0

                    objVMOperaciones.LimpiarDatosTipoNegocio(objVMOperaciones.EncabezadoSeleccionado, OperacionesOtrosNegociosViewModel.LIMPIARDATOS_NEMOTECNICO)

                    objVMOperaciones.logCalcularValores = True
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_especieAsignada(pstrNemotecnico As System.String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(objVMOperaciones) Then
                If Not IsNothing(pobjEspecie) Then
                    objVMOperaciones.NemotecnicoSeleccionado = pobjEspecie
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_especieAsignada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOperaciones) Then
                Me.objVMOperaciones.ComitenteSeleccionado = Nothing
                If Me.objVMOperaciones.BorrarCliente Then
                    Me.objVMOperaciones.BorrarCliente = False
                End If
                Me.objVMOperaciones.BorrarCliente = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOperaciones) Then
                Me.objVMOperaciones.NemotecnicoSeleccionado = Nothing
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

    Private Sub txtCalculo_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMOperaciones) Then
                If objVMOperaciones.logEditarRegistro Or objVMOperaciones.logNuevoRegistro Then
                    Dim strTipoControl As String = String.Empty

                    If TypeOf sender Is TextBox Then
                        strTipoControl = CType(sender, TextBox).Tag
                    ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                        strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag
                    End If

                    If objVMOperaciones.logCalcularValores Then
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
            If Not IsNothing(objVMOperaciones) Then
                If objVMOperaciones.logEditarRegistro Or objVMOperaciones.logNuevoRegistro And objVMOperaciones.logCalcularValores Then
                    If logDigitoValor Then
                        Dim strTipoControl As String = String.Empty

                        strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag

                        logCambioValor = True
                    Else
                        If CType(sender, A2Utilidades.A2NumericBox).Name = strNombreControlCambio And
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
            If Not IsNothing(objVMOperaciones) Then
                If objVMOperaciones.logEditarRegistro Or objVMOperaciones.logNuevoRegistro Then
                    If logCambioValor And objVMOperaciones.logCalcularValores Then
                        Dim strTipoControl As String = String.Empty

                        If TypeOf sender Is TextBox Then
                            strTipoControl = CType(sender, TextBox).Tag
                        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                            strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag
                        End If

                        If objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_RENTAFIJA Or
                            objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_REPO Or
                            objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_SIMULTANEA Or
                            objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_TTV Then
                            If strTipoControl = OperacionesOtrosNegociosViewModel.LIMPIARDATOS_NOMINAL Then
                                objVMOperaciones.LimpiarDatosTipoNegocio(objVMOperaciones.EncabezadoSeleccionado, OperacionesOtrosNegociosViewModel.LIMPIARDATOS_NOMINAL)
                                If objVMOperaciones.dblPrecioConsultado = objVMOperaciones.EncabezadoSeleccionado.PrecioSucio And objVMOperaciones.EncabezadoSeleccionado.Mercado = objVMOperaciones.MERCADO_SECUNDARIO Then
                                    objVMOperaciones.strTipoCalculo = "PRECIO"
                                Else
                                    objVMOperaciones.strTipoCalculo = String.Empty
                                    logCambioValor = False
                                    logDigitoValor = False
                                    Exit Sub
                                End If
                            Else
                                If objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_RENTAFIJA Then
                                    objVMOperaciones.strTipoCalculo = objVMOperaciones.VerificarTipoCalculo(objVMOperaciones.EncabezadoSeleccionado, objVMOperaciones.strTipoCalculo, strTipoControl)
                                Else
                                    If Not String.IsNullOrEmpty(strTipoControl) Then
                                        objVMOperaciones.strTipoCalculo = strTipoControl
                                    End If
                                End If
                            End If
                        Else
                            objVMOperaciones.strTipoCalculo = strTipoControl
                        End If

                        logCambioValor = False
                        logDigitoValor = False
                        Await objVMOperaciones.CalcularValorRegistro()
                        ReubicarFocoControl(sender)
                    End If

                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtCalculo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

            If objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_RENTAVARIABLE Then
                If strNombreControl = "txtNominalRentaVariable" Then
                    txtPrecioRentaVariable.Focus()
                ElseIf strNombreControl = "txtPrecioRentaVariable" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString) Then
                        txtPorcentajeComisionRentaVariable.Focus()
                    Else
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                            txtTasaCambioConversionRentaVariable.Focus()
                        Else
                            txtObservaciones.Focus()
                        End If
                    End If
                ElseIf strNombreControl = "txtPorcentajeComisionRentaVariable" Then
                    txtValorComisionRentaVariable.Focus()
                ElseIf strNombreControl = "txtValorComisionRentaVariable" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                        txtTasaCambioConversionRentaVariable.Focus()
                    Else
                        txtObservaciones.Focus()
                    End If
                ElseIf strNombreControl = "txtTasaCambioConversionRentaVariable" Then
                    txtObservaciones.Focus()
                End If
            ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_RENTAFIJA Then
                If strNombreControl = "txtNominalRentaFija" Then
                    txtPrecioRentaFija.Focus()
                ElseIf strNombreControl = "txtPrecioRentaFija" Then
                    txtTasaRetornoYieldRentaFija.Focus()
                ElseIf strNombreControl = "txtTasaRetornoYieldRentaFija" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString) Then
                        txtPorcentajeComisionRentaFija.Focus()
                    Else
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                            txtTasaCambioConversionRentaFija.Focus()
                        Else
                            txtValorNetoRentaFija.Focus()
                        End If
                    End If
                ElseIf strNombreControl = "txtPorcentajeComisionRentaFija" Then
                    txtValorComisionRentaFija.Focus()
                ElseIf strNombreControl = "txtValorComisionRentaFija" Then
                    txtValorNetoRentaFija.Focus()
                ElseIf strNombreControl = "txtValorNetoRentaFija" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                        txtTasaCambioConversionRentaFija.Focus()
                    Else
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION.ToString) Then
                            txtRetencionRentaFija.Focus()
                        Else
                            txtObservaciones.Focus()
                        End If
                    End If
                ElseIf strNombreControl = "txtTasaCambioConversionRentaFija" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION.ToString) Then
                        txtRetencionRentaFija.Focus()
                    Else
                        txtObservaciones.Focus()
                    End If
                ElseIf strNombreControl = "txtRetencionRentaFija" Then
                    txtObservaciones.Focus()
                End If
            ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_REPO Then
                If objVMOperaciones.EncabezadoSeleccionado.Tipo = objVMOperaciones.CLASE_ACCIONES Then
                    If strNombreControl = "txtNominalREPOAcciones" Then
                        txtPrecioREPOAcciones.Focus()
                    ElseIf strNombreControl = "txtPrecioREPOAcciones" Then
                        txtHiarcutREPOAcciones.Focus()
                    ElseIf strNombreControl = "txtHiarcutREPOAcciones" Then
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString) Then
                            txtPorcentajeComisionREPOAcciones.Focus()
                        Else
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                                txtTasaCambioConversionREPOAcciones.Focus()
                            Else
                                dtpFechaVencimientoREPOAcciones.Focus()
                            End If
                        End If
                    ElseIf strNombreControl = "txtPorcentajeComisionREPOAcciones" Then
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                            txtTasaCambioConversionREPOAcciones.Focus()
                        Else
                            dtpFechaVencimientoREPOAcciones.Focus()
                        End If
                    ElseIf strNombreControl = "txtTasaCambioConversionREPOAcciones" Then
                        dtpFechaVencimientoREPOAcciones.Focus()
                    ElseIf strNombreControl = "txtVTasaPactadaREPOAcciones" Then
                        txtObservaciones.Focus()
                    End If
                Else
                    If objVMOperaciones.EncabezadoSeleccionado.TipoRepo = objVMOperaciones.TIPOREPO_ABIERTO Then
                        If strNombreControl = "txtNominalRepoAbierto" Then
                            txtPrecioRepoAbierto.Focus()
                        ElseIf strNombreControl = "txtPrecioRepoAbierto" Then
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO.ToString) Then
                                txtTasaRetornoYieldRepoAbierto.Focus()
                            Else
                                txtHiarcutRepoAbierto.Focus()
                            End If
                        ElseIf strNombreControl = "txtTasaRetornoYieldRepoAbierto" Then
                            txtHiarcutRepoAbierto.Focus()
                        ElseIf strNombreControl = "txtHiarcutRepoAbierto" Then
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString) Then
                                txtPorcentajeComisionRepoAbierto.Focus()
                            Else
                                txtValorGiroRepoAbierto.Focus()
                            End If
                        ElseIf strNombreControl = "txtPorcentajeComisionRepoAbierto" Then
                            txtValorGiroRepoAbierto.Focus()
                        ElseIf strNombreControl = "txtValorGiroRepoAbierto" Then
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                                txtTasaCambioConversionRepoAbierto.Focus()
                            Else
                                dtpFechaVencimientoRepoAbierto.Focus()
                            End If
                        ElseIf strNombreControl = "txtPorcentajeComisionRepoAbierto" Then
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                                txtTasaCambioConversionRepoAbierto.Focus()
                            Else
                                dtpFechaVencimientoRepoAbierto.Focus()
                            End If
                        ElseIf strNombreControl = "txtTasaCambioConversionRepoAbierto" Then
                            dtpFechaVencimientoRepoAbierto.Focus()
                        ElseIf strNombreControl = "txtVTasaPactadaRepoAbierto" Then
                            txtObservaciones.Focus()
                        End If
                    Else
                        If strNombreControl = "txtNominalRepo" Then
                            txtPrecioRepo.Focus()
                        ElseIf strNombreControl = "txtPrecioRepo" Then
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO.ToString) Then
                                txtTasaRetornoYieldRepo.Focus()
                            Else
                                txtHiarcutRepo.Focus()
                            End If
                        ElseIf strNombreControl = "txtTasaRetornoYieldRepo" Then
                            txtHiarcutRepo.Focus()
                        ElseIf strNombreControl = "txtHiarcutRepo" Then
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString) Then
                                txtPorcentajeComisionRepo.Focus()
                            Else
                                If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                                    txtTasaCambioConversionRepo.Focus()
                                Else
                                    dtpFechaVencimientoRepo.Focus()
                                End If
                            End If
                        ElseIf strNombreControl = "txtPorcentajeComisionRepo" Then
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                                txtTasaCambioConversionRepo.Focus()
                            Else
                                dtpFechaVencimientoRepo.Focus()
                            End If
                        ElseIf strNombreControl = "txtTasaCambioConversionRepo" Then
                            dtpFechaVencimientoRepo.Focus()
                        ElseIf strNombreControl = "txtVTasaPactadaRepo" Then
                            txtValorNetoRepoCOP.Focus()
                        ElseIf strNombreControl = "txtValorNetoRepoCOP" Then
                            txtObservaciones.Focus()
                        End If
                    End If
                End If
            ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_SIMULTANEA Then
                If strNombreControl = "txtNominalSimultanea" Then
                    txtPrecioSimultanea.Focus()
                ElseIf strNombreControl = "txtPrecioSimultanea" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO.ToString) Then
                        txtTasaRetornoYieldSimultanea.Focus()
                    Else
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                            txtTasaCambioConversionSimultanea.Focus()
                        Else
                            txtHiarcutSimultanea.Focus()
                        End If
                    End If
                ElseIf strNombreControl = "txtTasaRetornoYieldSimultanea" Then
                    txtHiarcutSimultanea.Focus()
                ElseIf strNombreControl = "txtHiarcutSimultanea" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString) Then
                        txtPorcentajeComisionSimultanea.Focus()
                    Else
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                            txtTasaCambioConversionSimultanea.Focus()
                        Else
                            dtpFechaVencimientoSimultanea.Focus()
                        End If
                    End If
                ElseIf strNombreControl = "txtPorcentajeComisionSimultanea" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                        txtTasaCambioConversionSimultanea.Focus()
                    Else
                        dtpFechaVencimientoSimultanea.Focus()
                    End If
                ElseIf strNombreControl = "txtTasaCambioConversionSimultanea" Then
                    dtpFechaVencimientoSimultanea.Focus()
                ElseIf strNombreControl = "txtVTasaPactadaSimultanea" Then
                    txtValorNetoSimultanea.Focus()
                ElseIf strNombreControl = "txtValorNetoSimultanea" Then
                    txtObservaciones.Focus()
                End If
            ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_TTV Then
                If objVMOperaciones.EncabezadoSeleccionado.Tipo = objVMOperaciones.CLASE_ACCIONES Then
                    If strNombreControl = "txtNominalTTVAcciones" Then
                        txtPrecioTTVAcciones.Focus()
                    ElseIf strNombreControl = "txtPrecioTTVAcciones" Then
                        txtHiarcutTTVAcciones.Focus()
                    ElseIf strNombreControl = "txtHiarcutTTVAcciones" Then
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString) Then
                            txtPorcentajeComisionTTVAcciones.Focus()
                        Else
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                                txtTasaCambioConversionTTVAcciones.Focus()
                            Else
                                dtpFechaVencimientoTTVAcciones.Focus()
                            End If
                        End If
                    ElseIf strNombreControl = "txtPorcentajeComisionTTVAcciones" Then
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                            txtTasaCambioConversionTTVAcciones.Focus()
                        Else
                            dtpFechaVencimientoTTVAcciones.Focus()
                        End If
                    ElseIf strNombreControl = "txtTasaCambioConversionTTVAcciones" Then
                        dtpFechaVencimientoTTVAcciones.Focus()
                    ElseIf strNombreControl = "txtVTasaPactadaTTVAcciones" Then
                        txtObservaciones.Focus()
                    End If
                Else
                    If strNombreControl = "txtNominalTTV" Then
                        txtPrecioTTV.Focus()
                    ElseIf strNombreControl = "txtPrecioTTV" Then
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO.ToString) Then
                            txtTasaRetornoYieldTTV.Focus()
                        Else
                            txtHiarcutTTV.Focus()
                        End If
                    ElseIf strNombreControl = "txtTasaRetornoYieldTTV" Then
                        txtHiarcutTTV.Focus()
                    ElseIf strNombreControl = "txtHiarcutTTV" Then
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString) Then
                            txtPorcentajeComisionTTV.Focus()
                        Else
                            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                                txtTasaCambioConversionTTV.Focus()
                            Else
                                dtpFechaVencimientoTTV.Focus()
                            End If
                        End If
                    ElseIf strNombreControl = "txtPorcentajeComisionTTV" Then
                        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                            txtTasaCambioConversionTTV.Focus()
                        Else
                            dtpFechaVencimientoTTV.Focus()
                        End If
                    ElseIf strNombreControl = "txtTasaCambioConversionTTV" Then
                        dtpFechaVencimientoTTV.Focus()
                    ElseIf strNombreControl = "txtVTasaPactadaTTV" Then
                        txtValorNetoCopTTV.Focus()
                    ElseIf strNombreControl = "txtValorNetoCopTTV" Then
                        txtObservaciones.Focus()
                    End If
                End If
            ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_DEPOSITOREMUNERADO Then
                If strNombreControl = "txtNominalDepositoRemunerado" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                        txtTasaCambioConversionDepositoRemunerado.Focus()
                    Else
                        dtpFechaVencimientoOperacionDeposito.Focus()
                    End If
                ElseIf strNombreControl = "txtTasaCambioConversionDepositoRemunerado" Then
                    dtpFechaVencimientoOperacionDeposito.Focus()
                ElseIf strNombreControl = "txtVTasaPactadaDepositoRemunerado" Then
                    txtObservaciones.Focus()
                End If
            ElseIf objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objVMOperaciones.TIPONEGOCIO_DIVISAS Then
                If strNombreControl = "txtNominalDivisas" Then
                    txtPorcentajeComisionDivisas.Focus()
                ElseIf strNombreControl = "txtPorcentajeComisionDivisas" Then
                    txtValorComisionDivisas.Focus()
                ElseIf strNombreControl = "txtValorComisionDivisas" Then
                    If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString) Then
                        txtTasaCambioConversionDivisas.Focus()
                    Else
                        txtObservaciones.Focus()
                    End If
                ElseIf strNombreControl = "txtTasaCambioConversionDivisas" Then
                    txtObservaciones.Focus()
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "ReubicarFocoControl", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub BuscarControlValidacion(ByVal pstrOpcion As String)
        Try
            If Not IsNothing(Me) Then
                If TypeOf Me.FindName(pstrOpcion) Is TabItem Then
                    CType(Me.FindName(pstrOpcion), TabItem).IsSelected = True
                ElseIf TypeOf Me.FindName(pstrOpcion) Is TextBox Then
                    CType(Me.FindName(pstrOpcion), TextBox).Focus()
                ElseIf TypeOf Me.FindName(pstrOpcion) Is ComboBox Then
                    CType(Me.FindName(pstrOpcion), ComboBox).Focus()


                ElseIf TypeOf Me.FindName(pstrOpcion) Is A2Utilidades.A2NumericBox Then
                    CType(Me.FindName(pstrOpcion), A2Utilidades.A2NumericBox).Focus()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control dentro del registro.", Me.ToString, "BuscarControlValidacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtPais_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARPAIS.ToString) Then
                ctlBuscadorPais.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtPais_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtContraparte_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorContraparte.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtContraparte_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'Private Sub txtPagador_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
    '    Try
    '        If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
    '            ctlBuscadorPagador.AbrirBuscador()
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtPagador_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '    End Try
    'End Sub

    Private Sub txtMonedaRentaVariable_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaRentaVariable.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaRentaVariable_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaRentaFija_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaRentafija.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaRentaFija_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaRepoCerrado_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaRepo.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaRepoCerrado_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaRepoAbierto_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaRepoAbierto.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaRepoAbierto_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaSimultanea_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaSimultanea.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaSimultanea_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaTTV_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaTTV.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaTTV_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaDepositoRemunerado_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDA.ToString) Then
                ctlBuscadorMonedaDeposito.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaDepositoRemunerado_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaDivisas_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARMONEDA.ToString) Then
                ctlBuscadorMonedaDivisas.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaDivisas_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaTTVAcciones_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaTTVAcciones.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaTTVAcciones_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMonedaREPOAcciones_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMOperaciones.DiccionarioHabilitarCampos(OperacionesOtrosNegociosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaREPOAcciones.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaREPOAcciones_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnAplazar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOperaciones) Then
                Me.objVMOperaciones.AplazarOperacion()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al presionar el botón aplazar.", Me.Name, "btnAplazar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.objVMOperaciones.PreguntarDuplicarRegistro()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al presionar el botón duplicar.", Me.Name, "btnDuplicar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLiqxDife_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.objVMOperaciones.PreguntarAplicarLiqxDife()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al presionar el botón LiqxDife.", Me.Name, "btnLiqxDife_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
