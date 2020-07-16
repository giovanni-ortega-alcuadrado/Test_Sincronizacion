
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices

Partial Public Class OrdenesReciboPLUSView
    Inherits UserControl

    Dim objVMA2Utils As A2UtilsViewModel
    Private WithEvents objVMOrdenesTesoreria As OrdenesReciboViewModel_OYDPLUS
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Private mstrDicCombosEspecificos As String = String.Empty

    Dim vmTesorero As TesoreroViewModel_OYDPLUS
    Public ViewPopPupEdicion As OrdenReciboConsultaView = Nothing
    Private IDOrdenRecibo As Integer = 0
    Private logXTesorero As Boolean = False

#Region "Inicializaciones"

    Public Sub New()
        Try
            Me.DataContext = New OrdenesReciboViewModel_OYDPLUS
            InitializeComponent()

            stackBotones.Visibility = Visibility.Visible
            stackBotonesTesorero.Visibility = Visibility.Collapsed

            objVMOrdenesTesoreria = Me.LayoutRoot.DataContext

            Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 275
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
            AddHandler Me.Unloaded, AddressOf View_Unloaded
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub New(ByVal plngIdEncabezado As Integer, ByVal pvmTesorero As TesoreroViewModel_OYDPLUS)
        Try
            Me.DataContext = New OrdenesReciboViewModel_OYDPLUS
            InitializeComponent()

            stackBotones.Visibility = Visibility.Collapsed
            stackBotonesTesorero.Visibility = Visibility.Visible

            objVMOrdenesTesoreria = Me.LayoutRoot.DataContext

            Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 275
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
            AddHandler Me.Unloaded, AddressOf View_Unloaded

            Me.IDOrdenRecibo = plngIdEncabezado
            Me.logXTesorero = True
            Me.vmTesorero = pvmTesorero
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


#End Region

    Private Sub OrdeneReciboPLUSView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        Try
            cm.GridTransicion = grdGridForma
            cm.GridViewRegistros = datapager
            cm.DF = df
            If objVMOrdenesTesoreria Is Nothing Then
                objVMOrdenesTesoreria = CType(Me.DataContext, OrdenesReciboViewModel_OYDPLUS)
            End If

            objVMOrdenesTesoreria.OrdenesReciboPLUSView = Me
            objVMOrdenesTesoreria.objViewOrdenReciboPopPup = Me.ViewPopPupEdicion
            objVMOrdenesTesoreria.logXTesorero = Me.logXTesorero
            objVMOrdenesTesoreria.objViewModelTesorero = Me.vmTesorero

            If mlogInicializado = False Then
                If Me.IDOrdenRecibo > 0 Then
                    objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Selected = Nothing
                    objVMOrdenesTesoreria.IdEncabezadoXTesorero = Me.IDOrdenRecibo
                    If Not IsNothing(objVMOrdenesTesoreria.dcProxy) Then
                        objVMOrdenesTesoreria.dcProxy.TesoreriaOrdenesEncabezados.Clear()
                    End If
                    objVMOrdenesTesoreria.dcProxy.Load(objVMOrdenesTesoreria.dcProxy.TesoreriaOrdenesConsultarQuery(Me.IDOrdenRecibo, Nothing, "TE", Nothing, Nothing, Nothing, Nothing, Program.Usuario, "CON", GSTR_ORDENRECIBO, Program.HashConexion),
                                                           AddressOf objVMOrdenesTesoreria.TerminoTraerTesoreriaOrdenesConsultar, GSTR_TESOREROEDITAR)
                    objVMOrdenesTesoreria.HabilitarInstrucciones = False
                    objVMOrdenesTesoreria.ConsultarCarterasColectivasFondos(String.Empty, True, "TODASLASCARTERAS")
                Else
                    objVMOrdenesTesoreria.Inicializar()
                End If

                mlogInicializado = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "OrdeneReciboPLUSView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsNothing(objVMOrdenesTesoreria) Then

            Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 275
        End If
    End Sub

    Private Sub ctrlCliente_comitenteAsignado(ByVal pstrIdComitente As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                Me.objVMOrdenesTesoreria.actualizarComitenteOrden(pobjComitente)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(df) Then
            df.CancelEdit()
            If Not IsNothing(df.ValidationSummary) Then
                If Not IsNothing(df.ValidationSummary) Then
                    df.ValidationSummary.DataContext = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(df) Then
            df.ValidateItem()
            If Not IsNothing(df.ValidationSummary) Then
                If df.ValidationSummary.HasErrors Then
                    df.CancelEdit()
                Else
                    df.CommitEdit()
                End If
            End If
        End If
    End Sub

    Private Sub ctlrEspecies_especieAsignada(ByVal pstrNemotecnico As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)

    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)

    End Sub



    Private Sub Expander_Collapsed(sender As System.Object, e As System.Windows.RoutedEventArgs)
        objVMOrdenesTesoreria.VerItemActual = Visibility.Visible
    End Sub

    Private Sub Expander_Expanded(sender As System.Object, e As System.Windows.RoutedEventArgs)
        objVMOrdenesTesoreria.VerItemActual = Visibility.Collapsed
    End Sub

    Private Sub TabControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim t = TryCast(sender, System.Windows.Controls.TabControl)
        If Not IsNothing(objVMOrdenesTesoreria) Then
            If t.SelectedItem.Name = "tabItemCheque" Then
                objVMOrdenesTesoreria.TabItemActual = "Cheques"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_CHEQUE)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Cheque"
            ElseIf t.SelectedItem.Name = "tabItemTransferencia" Then
                objVMOrdenesTesoreria.TabItemActual = "Transferencias"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_TRANSFERENCIA)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Transferencia"
            ElseIf t.SelectedItem.Name = "tabItemConsignacion" Then
                objVMOrdenesTesoreria.TabItemActual = "Consignaciones"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_CONSIGNACIONES)
            ElseIf t.SelectedItem.Name = "TabItemCargarPagoA" Then
                objVMOrdenesTesoreria.TabItemActual = "Cargar pagos A"
                objVMOrdenesTesoreria.ValorTotalGenerarActual = 0
            ElseIf t.SelectedItem.Name = "TabItemCargarPagoAFondos" Then
                objVMOrdenesTesoreria.TabItemActual = "Cargar pagos A Fondos"
                objVMOrdenesTesoreria.ValorTotalGenerarActual = 0
            End If
        End If

    End Sub

    Private Sub ComprobantesEgresoPLUSView_Unload(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        'objVMOrdenesTesoreria.pararTemporizador()
    End Sub

    Private Sub ctrlCliente_comitenteAsignado_1(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                Me.objVMOrdenesTesoreria.cb.ComitenteSeleccionadoBusqueda = pobjComitente
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de tesoreria
            If Not IsNothing(objVMOrdenesTesoreria) Then
                'objVMOrdenesTesoreria.dcOrdenes.OYDPLUS_CancelarOrdenOYDPLUS(objVMOrdenesTesoreria.TesoreriaOrdenesPlusAnterior.lngID, GSTR_OT, Program.Usuario, Program.HashConexion, AddressOf objVMOrdenesTesoreria.TerminoCancelarEditarRegistro, String.Empty)
                Me.objVMOrdenesTesoreria.pararTemporizador()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de ordenes de tesoreria.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CargarPopup(ByVal sender As Object, ByVal e As System.EventArgs, pstrOpcion As String) Handles objVMOrdenesTesoreria.LanzarPopupCheques
        Dim popup As New wppFrmCheque_RC(objVMOrdenesTesoreria)
        objVMOrdenesTesoreria.objWppChequeRC = popup
        objVMOrdenesTesoreria.mobjCtlSubirArchivoCheque = popup.ctlSubirArchivo
        If pstrOpcion = GSTR_NUEVODETALLE_Plus Then
            objVMOrdenesTesoreria.logEditarChequeRC = False
            objVMOrdenesTesoreria.ValoresXdefectoWpp()
            objVMOrdenesTesoreria.ValidarEncabezado()

            objVMOrdenesTesoreria.configurarDocumentos(objVMOrdenesTesoreria.mobjCtlSubirArchivoCheque, 0, GSTR_CHEQUE)
            If objVMOrdenesTesoreria.logHayEncabezado Then
                objVMOrdenesTesoreria.mstrArchivoCheque = String.Empty
                objVMOrdenesTesoreria.mstrRutaCheque = String.Empty
                objVMOrdenesTesoreria.mabytArchivoCheque = Nothing

                Program.Modal_OwnerMainWindowsPrincipal(popup)
                popup.ShowDialog()
            Else
                mostrarMensaje("Para ingresar un detalle por favor ingrese los datos correspondientes al encabezado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                objVMOrdenesTesoreria.HabilitarEncabezado = True
                objVMOrdenesTesoreria.HabilitarImportacion = False
            End If
        ElseIf pstrOpcion = GSTR_EDITARDETALLE_Plus Then
            objVMOrdenesTesoreria.HabilitarImportacion = True

            If objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID > 0 Then
                objVMOrdenesTesoreria.ValidarEstadoOrdenDetalleServidor(objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID, objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strEstado, GSTR_CHEQUE)
            Else
                If objVMOrdenesTesoreria.ObjetoEditarCheque Then
                    objVMOrdenesTesoreria.mstrArchivoCheque = String.Empty
                    objVMOrdenesTesoreria.mstrRutaCheque = String.Empty
                    objVMOrdenesTesoreria.mabytArchivoCheque = Nothing

                    Program.Modal_OwnerMainWindowsPrincipal(popup)
                    popup.ShowDialog()
                End If
            End If

        End If
    End Sub
    Private Sub CargarPopupPagoA(ByVal sender As Object, ByVal e As System.EventArgs, pstrOpcion As String) Handles objVMOrdenesTesoreria.LanzarPopupCargarPagoA
        If String.IsNullOrEmpty(objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Selected.strIDComitente) Then
            objVMOrdenesTesoreria.IDTipoClienteCargarPagosA = GSTR_TERCERO
        Else
            objVMOrdenesTesoreria.IDTipoClienteCargarPagosA = GSTR_CLIENTE
        End If

        Dim popup As New wppFrmCargarPagosA_RC(objVMOrdenesTesoreria)
        popup.Title = objVMOrdenesTesoreria.TituloPestanaCargarPagosA
        objVMOrdenesTesoreria.objWppCargarPagosA = popup
        If pstrOpcion = GSTR_NUEVODETALLE_Plus Then
            objVMOrdenesTesoreria.logEditarCargarPagosA = False
            objVMOrdenesTesoreria.ValoresXdefectoCargarPagosAWpp() ''''
            objVMOrdenesTesoreria.ValidarEncabezado()

            If objVMOrdenesTesoreria.logHayEncabezado Then
                objVMOrdenesTesoreria.CalcularTotales(String.Empty)
                objVMOrdenesTesoreria.CalcularValorDisponibleCargar()
                Program.Modal_OwnerMainWindowsPrincipal(popup)
                popup.ShowDialog()
            Else
                mostrarMensaje("Para ingresar un detalle por favor ingrese los datos correspondientes al encabezado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                objVMOrdenesTesoreria.HabilitarEncabezado = True
                objVMOrdenesTesoreria.HabilitarImportacion = False
            End If
        ElseIf pstrOpcion = GSTR_EDITARDETALLE_Plus Then
            objVMOrdenesTesoreria.HabilitarImportacion = True

            If Not IsNothing(objVMOrdenesTesoreria.TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngID) Then
                objVMOrdenesTesoreria.ValidarEstadoOrdenDetalleServidor(objVMOrdenesTesoreria.TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngID, GSTR_PENDIENTE_Plus_Detalle, GSTR_ORDENRECIBO_CARGOPAGOA)
            Else
                If objVMOrdenesTesoreria.ObjetoEditarCargarPagosA Then
                    Program.Modal_OwnerMainWindowsPrincipal(popup)
                    popup.ShowDialog()
                End If
            End If
        End If
    End Sub
    Private Sub CargarPopupPagoAFondos(ByVal sender As Object, ByVal e As System.EventArgs, pstrOpcion As String) Handles objVMOrdenesTesoreria.LanzarPopupCargarPagoAFondos
        Dim popup As New wppFrmCargarPagosAFondos_RC(objVMOrdenesTesoreria)
        popup.Title = objVMOrdenesTesoreria.TituloPestanaCargarPagosAFondos
        objVMOrdenesTesoreria.objWppCargarPagosAFondos = popup

        If pstrOpcion = GSTR_NUEVODETALLE_Plus Then
            objVMOrdenesTesoreria.logEditarCargarPagosAFondos = False
            objVMOrdenesTesoreria.ValoresXdefectoCargarPagosAFondosWpp()

            objVMOrdenesTesoreria.ValidarEncabezado()

            If objVMOrdenesTesoreria.logHayEncabezado Then
                Dim logLevantarPopupPagos As Boolean = True

                objVMOrdenesTesoreria.CalcularTotales(String.Empty)
                objVMOrdenesTesoreria.CalcularValorDisponibleCargar()
                Program.Modal_OwnerMainWindowsPrincipal(popup)
                popup.ShowDialog()
            Else
                mostrarMensaje("Para ingresar un detalle por favor ingrese los datos correspondientes al encabezado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                objVMOrdenesTesoreria.HabilitarEncabezado = True
                objVMOrdenesTesoreria.HabilitarImportacion = False
            End If
        ElseIf pstrOpcion = GSTR_EDITARDETALLE_Plus Then
            objVMOrdenesTesoreria.HabilitarImportacion = True

            If Not IsNothing(objVMOrdenesTesoreria.TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID) Then
                objVMOrdenesTesoreria.ValidarEstadoOrdenDetalleServidor(objVMOrdenesTesoreria.TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID, GSTR_PENDIENTE_Plus_Detalle, GSTR_ORDENRECIBO_CARGOPAGOAFONDOS)
            Else
                If objVMOrdenesTesoreria.ObjetoEditarCargarPagosFondos Then
                    Program.Modal_OwnerMainWindowsPrincipal(popup)
                    popup.ShowDialog()
                End If
            End If
        End If
    End Sub
    Private Sub CargarPopupTransferencias(ByVal sender As Object, ByVal e As System.EventArgs, pstrOpcion As String) Handles objVMOrdenesTesoreria.LanzarPopupTransferencias
        Dim popup As New wppFrmTransferencia_RC(objVMOrdenesTesoreria)
        objVMOrdenesTesoreria.objWppTransferencia_RC = popup
        objVMOrdenesTesoreria.mobjCtlSubirArchivoTransferencia = popup.ctlSubirArchivo

        If pstrOpcion = GSTR_NUEVODETALLE_Plus Then
            objVMOrdenesTesoreria.logEditarCargarPagosA = False
            objVMOrdenesTesoreria.ValoresXdefectoWppTransferencia()
            objVMOrdenesTesoreria.ValidarEncabezado()
            objVMOrdenesTesoreria.configurarDocumentos(objVMOrdenesTesoreria.mobjCtlSubirArchivoTransferencia, 0, GSTR_TRANSFERENCIA)
            If objVMOrdenesTesoreria.logHayEncabezado Then
                objVMOrdenesTesoreria.mstrArchivoTransferencia = String.Empty
                objVMOrdenesTesoreria.mstrRutaTransferencia = String.Empty
                objVMOrdenesTesoreria.mabytArchivoTransferencia = Nothing

                objVMOrdenesTesoreria.CalcularValorDisponibleCargar()
                Program.Modal_OwnerMainWindowsPrincipal(popup)
                popup.ShowDialog()
            Else
                mostrarMensaje("Para ingresar un detalle por favor ingrese los datos correspondientes al encabezado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                objVMOrdenesTesoreria.HabilitarEncabezado = True
                objVMOrdenesTesoreria.HabilitarImportacion = False
            End If
        ElseIf pstrOpcion = GSTR_EDITARDETALLE_Plus Then
            objVMOrdenesTesoreria.HabilitarImportacion = True
            objVMOrdenesTesoreria.logEditarCargarPagosA = True
            If objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID > 0 Then
                objVMOrdenesTesoreria.ValidarEstadoOrdenDetalleServidor(objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID, objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strEstado, GSTR_TRANSFERENCIA)
            Else
                If objVMOrdenesTesoreria.ObjetoEditarTransferencia Then
                    objVMOrdenesTesoreria.mstrArchivoTransferencia = String.Empty
                    objVMOrdenesTesoreria.mstrRutaTransferencia = String.Empty
                    objVMOrdenesTesoreria.mabytArchivoTransferencia = Nothing

                    Program.Modal_OwnerMainWindowsPrincipal(popup)
                    popup.ShowDialog()
                End If
            End If
        End If
    End Sub
    Private Sub CargarPopupConsignacion(ByVal sender As Object, ByVal e As System.EventArgs, pstrOpcion As String) Handles objVMOrdenesTesoreria.LanzarPopupConsignacion
        Dim popup As New wppFrmConsignaciones_RC(objVMOrdenesTesoreria)
        objVMOrdenesTesoreria.objWppConsignacion = popup
        objVMOrdenesTesoreria.mobjCtlSubirArchivoConsignacion = popup.ctlSubirArchivo
        If pstrOpcion = GSTR_NUEVODETALLE_Plus Then
            objVMOrdenesTesoreria.logNuevaConsignacion = True
            objVMOrdenesTesoreria.VerFormaChequeConsignacion = Visibility.Collapsed
            objVMOrdenesTesoreria.HabilitarEncabezadoConsignacion = True
            objVMOrdenesTesoreria.HabilitarEncabezadoConsignacionReferencia = True
            objVMOrdenesTesoreria.logEditarConsignacion = False
            objVMOrdenesTesoreria.ValoresXdefectoWppConsignacion()
            objVMOrdenesTesoreria.configurarDocumentos(objVMOrdenesTesoreria.mobjCtlSubirArchivoConsignacion, 0, GSTR_CONSIGNACIONES)
            objVMOrdenesTesoreria.ValidarEncabezado()
            If objVMOrdenesTesoreria.logHayEncabezado Then
                objVMOrdenesTesoreria.mstrArchivoConsignacion = String.Empty
                objVMOrdenesTesoreria.mstrRutaConsignacion = String.Empty
                objVMOrdenesTesoreria.mabytArchivoConsignacion = Nothing

                objVMOrdenesTesoreria.CalcularValorDisponibleCargar()
                Program.Modal_OwnerMainWindowsPrincipal(popup)
                popup.ShowDialog()
            Else
                mostrarMensaje("Para ingresar un detalle por favor ingrese los datos correspondientes al encabezado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                objVMOrdenesTesoreria.HabilitarEncabezado = True
                objVMOrdenesTesoreria.HabilitarImportacion = False
            End If
        ElseIf pstrOpcion = GSTR_EDITARDETALLE_Plus Then
            objVMOrdenesTesoreria.HabilitarImportacion = True
            objVMOrdenesTesoreria.logNuevaConsignacion = False
            If objVMOrdenesTesoreria.TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID > 0 Then
                objVMOrdenesTesoreria.ValidarEstadoOrdenDetalleServidor(objVMOrdenesTesoreria.TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID, objVMOrdenesTesoreria.TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strEstado, GSTR_CONSIGNACIONES)
            Else
                If objVMOrdenesTesoreria.ObjetoEditarConsignacion Then
                    objVMOrdenesTesoreria.mstrArchivoConsignacion = String.Empty
                    objVMOrdenesTesoreria.mstrRutaConsignacion = String.Empty
                    objVMOrdenesTesoreria.mabytArchivoConsignacion = Nothing

                    Program.Modal_OwnerMainWindowsPrincipal(popup)
                    popup.ShowDialog()
                End If
            End If
        End If
    End Sub


    Private Sub btnSubirRecibos_CargarArchivo(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog.FileName) Then
                If Path.GetExtension(objDialog.FileName).Equals(".csv") Or Path.GetExtension(objDialog.FileName).Equals(".xls") Or Path.GetExtension(objDialog.FileName).Equals(".xlsx") Then
                    Dim viewImportacion As New ImportarArchivoRecibos(objVMOrdenesTesoreria, System.IO.Path.GetFileName(objDialog.FileName))
                    Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                    viewImportacion.ShowDialog()
                Else
                    mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirRecibos_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub btnLimpiarClienteBusqueda_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.objVMOrdenesTesoreria.cb.ComitenteSeleccionadoBusqueda = Nothing
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name,
                                   "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Me.objVMOrdenesTesoreria.BorrarCliente Then
                Me.objVMOrdenesTesoreria.BorrarCliente = False
            End If
            Me.objVMOrdenesTesoreria.BorrarCliente = True
            Me.objVMOrdenesTesoreria.actualizarComitenteOrden(Nothing)
            Me.objVMOrdenesTesoreria.ClientePresente = Nothing
            objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Selected.strNombre = String.Empty
            objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Selected.strNroDocumento = Nothing
            objVMOrdenesTesoreria.TesoreriaOrdenesPlusRC_Selected.strIDComitente = Nothing
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name,
                                   "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenesTesoreria.Duplicar()
    End Sub

    Private Sub btnRefrescarPantalla_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenesTesoreria.RecargarPantalla()
    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                objVMOrdenesTesoreria.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                objVMOrdenesTesoreria.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim strUrlArchivo As String = CType(sender, Button).Tag
        Program.VisorArchivosWeb_DescargarURL(strUrlArchivo)
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenesTesoreria.EditarRegistro()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenesTesoreria.ActualizarRegistro()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenesTesoreria.CancelarEditarRegistro()
    End Sub

    Private Sub btnCerrarPopup_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenesTesoreria.CancelarEditarRegistro()
        objVMOrdenesTesoreria.CerrarPopup()
    End Sub
End Class
