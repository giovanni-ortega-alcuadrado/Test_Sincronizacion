
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.VisualBasic.CompilerServices


Partial Public Class ComprobantesEgresoView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Dim logcargainicial As Boolean = True
    Dim strcuentabancaria As String = ""
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty
    Dim logEsOrdenEmergente As Boolean = False
    Dim intIDDocumentoEmergente As Nullable(Of Integer) = Nothing
    Dim strNombreConsecutivoEmergente As String = String.Empty
    Dim objTesoreriaEmergente As TesoreriaEmergenteEncabezado = Nothing
    Public objVentanaEmergente As TesoreriaVentanaEmergenteView = Nothing

#Region "Variables"
    Private mlogInicializado As Boolean = False
    Private mobjVM As TesoreriaViewModel
#End Region
#Region "Inicializaciones"
    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        Try
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_ComprobantesEgreso"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.CE)


            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

            mlogInicializado = True
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New TesoreriaViewModel
InitializeComponent()
    End Sub
    Public Sub New(ByVal pobjVentanaEmergente As TesoreriaVentanaEmergenteView,
                   Optional ByVal plngIDDocumento As Nullable(Of Integer) = Nothing,
                   Optional ByVal pstrNombreConsecutivo As String = "",
                   Optional ByVal pobjTesoreriaEmergente As TesoreriaEmergenteEncabezado = Nothing)
        Try
            logEsOrdenEmergente = True
            objVentanaEmergente = pobjVentanaEmergente
            intIDDocumentoEmergente = plngIDDocumento
            strNombreConsecutivoEmergente = pstrNombreConsecutivo
            objTesoreriaEmergente = pobjTesoreriaEmergente

            'Dim objA2VM As A2UtilsViewModel
            Dim objA2VM As A2UtilsViewModel
            Dim strModuloTesoreria As String = ""

            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_ComprobantesEgreso"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.CE)


            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

            mlogInicializado = True

            Me.DataContext = New TesoreriaViewModel
InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region
    Private Sub btnDuplicar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        mobjVM = CType(Me.DataContext, TesoreriaViewModel)
        mobjVM.duplicarTesoreria()
    End Sub
    Private Sub ComprobantesEgresoView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If logcargainicial Then
            logcargainicial = False
            cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
            'cm.DF = df
            CType(Me.DataContext, TesoreriaViewModel).logVentanaEmergente = logEsOrdenEmergente
            CType(Me.DataContext, TesoreriaViewModel).objVentanaEmergente = objVentanaEmergente
            CType(Me.DataContext, TesoreriaViewModel).intIDDocumentoEmergente = intIDDocumentoEmergente
            CType(Me.DataContext, TesoreriaViewModel).strNombreConsecutivoEmergente = strNombreConsecutivoEmergente
            CType(Me.DataContext, TesoreriaViewModel).objTesoreriaEmergente = objTesoreriaEmergente
            CType(Me.DataContext, TesoreriaViewModel).NombreView = Me.ToString

            If logEsOrdenEmergente Then
                inicializa(False)
            Else
                inicializa()
            End If
            'Else
            'SLB20131021 Implementación del Autofresh
            'If Not CType(Me.DataContext, TesoreriaViewModel).Editando And CType(Me.DataContext, TesoreriaViewModel).visNavegando = "Visible" Then
            '    CType(Me.DataContext, TesoreriaViewModel).RefrescarTesoreria()
            'End If
        End If
    End Sub

    'RBP20160705_Proceso para cargar el archivo
    Private Sub btnSubirArchivoTesoreria_CargarArchivo(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    If Path.GetExtension(objDialog.FileName).Equals(".csv") Or Path.GetExtension(objDialog.FileName).Equals(".xls") Or Path.GetExtension(objDialog.FileName).Equals(".xlsx") Then
                        CType(Me.DataContext, TesoreriaViewModel).NombreArchivo = Path.GetFileName(objDialog.FileName)
                        CType(Me.DataContext, TesoreriaViewModel).ImportarArchivo()
                    Else
                        mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivoTesoreria_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub inicializa(Optional ByVal plogConsultar As Boolean = True)
        CType(Me.DataContext, TesoreriaViewModel).inicializar(plogConsultar)
    End Sub
    
#Region "Eventos"

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
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
    End Sub
    'JFSB 20171006 Se agrega metodo para limpiar el banco del encabezado
    Private Sub txtBancoBusqueda_Lostfocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If CType(sender, TextBox).Text = "" Then
            mobjVM = CType(Me.DataContext, TesoreriaViewModel)
            If Not IsNothing(mobjVM.TesoreriSelected) Then
                If Not IsNothing(mobjVM.TesoreriSelected.IDBanco) Then
                    mobjVM.TesoreriSelected.IDBanco = Nothing
                    mobjVM.TesoreriSelected.NombreBco = String.Empty
                    Exit Sub
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Evento para el manejo de los conceptos de Tesorería.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20130711</remarks>
    Private Sub Button_Click_BuscadorLista(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
            mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("ConceptoTeso", "Conceptos Asociados", "ConceptoTeso", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreConsecutivo, "", "")
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
            mobjBuscadorLst.ShowDialog()

        End If
    End Sub

    ''' <summary>
    ''' Evento para el manejo de los conceptos de Tesorería.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20130711</remarks>
    Private Sub Button_Click_BuscadorListaConceptos(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
                Dim intIDDetalle As Integer = CType(sender, Button).Tag
                Dim objDetalle As OyDTesoreria.DetalleTesoreri = Nothing

                If CType(Me.DataContext, TesoreriaViewModel).ListaDetalleTesoreria.Where(Function(i) i.IDDetalleTesoreria = intIDDetalle).Count > 0 Then
                    objDetalle = CType(Me.DataContext, TesoreriaViewModel).ListaDetalleTesoreria.Where(Function(i) i.IDDetalleTesoreria = intIDDetalle).First
                End If

                If Not IsNothing(objDetalle) Then
                    Dim objBuscadorConceptos As New A2ComunesControl.BuscarConceptoTesoreria(CType(Me.DataContext, TesoreriaViewModel).moduloTesoreria,
                                                                                                CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreConsecutivo,
                                                                                                CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IDCompania,
                                                                                                objDetalle.IDConcepto,
                                                                                                objDetalle.Detalle,
                                                                                                True)
                    AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto
                    Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                    objBuscadorConceptos.ShowDialog()

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al levantar el buscador de concepto", Me.Name, "Button_Click_BuscadorListaConceptos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub
    Private Sub Button_Click_BuscadorListaConceptosGenerico(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
                Dim intIDDetalle As Integer = CType(sender, Button).Tag
                Dim objDetalle As OyDTesoreria.DetalleTesoreri = Nothing

                If CType(Me.DataContext, TesoreriaViewModel).ListaDetalleTesoreria.Where(Function(i) i.IDDetalleTesoreria = intIDDetalle).Count > 0 Then
                    objDetalle = CType(Me.DataContext, TesoreriaViewModel).ListaDetalleTesoreria.Where(Function(i) i.IDDetalleTesoreria = intIDDetalle).First
                End If

                If Not IsNothing(objDetalle) Then
                    Dim objBuscadorConceptos As New A2ComunesControl.BuscarConceptoTesoreria(CType(Me.DataContext, TesoreriaViewModel).moduloTesoreria,
                                                                                                CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreConsecutivo,
                                                                                                CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IDCompania,
                                                                                                objDetalle.IDConcepto,
                                                                                                objDetalle.Detalle,
                                                                                                False,
                                                                                                False,
                                                                                                False,
                                                                                                False,
                                                                                                True)
                    AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConceptoGenerico
                    Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                    objBuscadorConceptos.ShowDialog()

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al levantar el buscador de concepto", Me.Name, "Button_Click_BuscadorListaConceptos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoSeleccionarConceptoGenerico(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2ComunesControl.BuscarConceptoTesoreria = CType(sender, A2ComunesControl.BuscarConceptoTesoreria)
            If objResultado.DialogResult Then
                CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.IDConcepto = objResultado.IDConcepto
                CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.Detalle = objResultado.DetalleConceptoCompleto

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar si se refrescan los datos de las órdenes en pantalla", Me.ToString(), "validarRefrescarOrdenes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoSeleccionarConcepto(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2ComunesControl.BuscarConceptoTesoreria = CType(sender, A2ComunesControl.BuscarConceptoTesoreria)
            If objResultado.DialogResult Then
                CType(Me.DataContext, TesoreriaViewModel).RecibirRespuestaBuscadorConceptos(objResultado.IDConcepto,
                                                                                            objResultado.DetalleConcepto,
                                                                                            objResultado.IDManejaCliente,
                                                                                            objResultado.IDTipoMovimiento,
                                                                                            objResultado.IDCuentaContable,
                                                                                            objResultado.IDRetencion,
                                                                                            objResultado.DetalleConceptoCompleto,
                                                                                            objResultado.ClienteSeleccionado,
                                                                                            objResultado.NombreCliente,
                                                                                            objResultado.NroDocumentoCliente)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar si se refrescan los datos de las órdenes en pantalla", Me.ToString(), "validarRefrescarOrdenes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    'SLB20130801 Refrescar documento de Tesorería.
    Private Sub cmdRefrescar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, TesoreriaViewModel).RefrescarTesoreria()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para lanzar la orden al mercado a través de SAE", Me.Name, "cmdLanzarSAE_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                CType(Me.DataContext, TesoreriaViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, TesoreriaViewModel).IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Edicion_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = 13 And CType(Me.DataContext, TesoreriaViewModel).Editando Then
            CType(Me.DataContext, TesoreriaViewModel).ActualizarRegistro()
        End If
    End Sub

    Private Sub Buscar_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = 13 Then
            CType(Me.DataContext, TesoreriaViewModel).ConfirmarBuscarEnter(GridBusqueda.FindName("NombreConsecutivo").SelectedValue, GridBusqueda.FindName("IDDocumento").Value, GridBusqueda.FindName("DocumentoB").SelectedDate, _
                                                                           GridBusqueda.FindName("Aprobados").IsChecked, GridBusqueda.FindName("NoAprobados").IsChecked, GridBusqueda.FindName("PorAprobar").IsChecked, GridBusqueda.FindName("Ingreso").IsChecked, _
                                                                           GridBusqueda.FindName("Modificacion").IsChecked, GridBusqueda.FindName("Retiro").IsChecked, GridBusqueda.FindName("Todos").IsChecked, GridBusqueda.FindName("IDBanco").Value)
        End If
    End Sub

#End Region
#Region "Buscadores Tesoreria Comprobantes Egreso"

    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCliente = False
            'SLB20140620 Esta logica la contiene el metodo del ViewModel
            CType(Me.DataContext, TesoreriaViewModel).ComitenteSeleccionadoM(pobjComitente)
            'If CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IdComitente Is Nothing Then
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.TipoCuenta = pobjComitente.TipoCuenta
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IdComitente = pobjComitente.IdComitente
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.Nombre = pobjComitente.Nombre
            '    'SLB20130625 Permite número de documento alfanumericos.
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NroDocumento = pobjComitente.NroDocumento
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.TipoIdentificacion = pobjComitente.CodTipoIdentificacion
            '    If CType(Me.DataContext, TesoreriaViewModel).ListaDetalleTesoreria.Count = 0 Then
            '        CType(Me.DataContext, TesoreriaViewModel).NombreColeccionDetalle = "cmDetalleTesoreri"
            '        CType(Me.DataContext, TesoreriaViewModel).NuevoRegistroDetalle()
            '    End If
            'Else
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.TipoCuenta = pobjComitente.TipoCuenta
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IdComitente = pobjComitente.IdComitente
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.Nombre = pobjComitente.Nombre
            '    'SLB20130625 Permite número de documento alfanumericos.
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NroDocumento = pobjComitente.NroDocumento
            '    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.TipoIdentificacion = pobjComitente.CodTipoIdentificacion
            'End If
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCliente = True
        End If
    End Sub

    Private Sub Buscar_finalizoBusquedaD(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCliente = False
            'SLB20140620 Esta logica la contiene el metodo del ViewModel
            CType(Me.DataContext, TesoreriaViewModel).ComitenteSeleccionadoDetalle(pobjComitente)
            'CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarNIT = False
            'CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.IDComitente = pobjComitente.IdComitente
            'CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.Nombre = pobjComitente.Nombre
            'CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.NIT = pobjComitente.NroDocumento
            'CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarNIT = True
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCliente = True
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "BancoConsultar"
                    CType(Me.DataContext, TesoreriaViewModel).CamposBusquedaTesoreria.IDBanco = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).CamposBusquedaTesoreria.NombreBanco = pobjItem.Nombre
                Case "IDBanco"
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = False
                    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IDBanco = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreBco = pobjItem.Nombre
                    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.ChequeraAutomatica = pobjItem.Estado
                    If Not String.IsNullOrEmpty(pobjItem.InfoAdicional03) Then
                        CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.CompaniaBanco = pobjItem.InfoAdicional03 'SV20160203
                    Else
                        CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.CompaniaBanco = Nothing 'SV20160203
                    End If

                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = True
                Case "CentrosCosto"
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCentroCostos = False
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.CentroCosto = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCentroCostos = True
                Case "compania"
                    CType(Me.DataContext, TesoreriaViewModel).logConsultarCompania = False
                    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IDCompania = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreCompania = pobjItem.Nombre
                    CType(Me.DataContext, TesoreriaViewModel).logConsultarCompania = True
                    CType(Me.DataContext, TesoreriaViewModel).ConsultarConsecutivosCompania()
                Case "FormaEntrega"
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarFormaEntrega = False
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.FormaEntrega = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.DescripcionFormaEntrega = pobjItem.Descripcion
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarFormaEntrega = True
                Case "TipoIdentBeneficiario"
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarTipoIdentBeneficiario = False
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.TipoIdentBeneficiario = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.DescripcionTipoIdentBeneficiario = pobjItem.Nombre
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarTipoIdentBeneficiario = True
                Case "EntidadBancoDestino"
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarEntidad = False
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.BancoDestino = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.NombreBancoDestino = pobjItem.Nombre
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarEntidad = True
                Case "TipoCuenta"
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarTipoCuenta = False
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.TipoCuenta = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.DescripcionTipoCuenta = pobjItem.Nombre
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarTipoCuenta = True
                Case "TipoIdTitular"
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarTipoIdTitular = False
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.TipoIdTitular = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.DescripcionTipoIdTitular = pobjItem.Nombre
                    CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarTipoIdTitular = True

                Case "companiabusqueda"
                    CType(Me.DataContext, TesoreriaViewModel).CamposBusquedaTesoreria.IDCompania = pobjItem.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).CamposBusquedaTesoreria.NombreCompania = pobjItem.Nombre
                    CType(Me.DataContext, TesoreriaViewModel).ConsultarConsecutivosCompaniaBusqueda()
            End Select
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaBancoDestino(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = False
            CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.lngBancoConsignacion = pobjItem.IdItem
            CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.strBancoDestino = pobjItem.Nombre
            'CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.ChequeraAutomatica = pobjItem.Estado
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = True
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaD(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCuentaContable = False
            CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.IDCuentaContable = pobjItem.IdItem
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCuentaContable = True
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaNITS(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarNIT = False
            CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.NIT = pobjItem.CodItem
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarNIT = True
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaSB(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IdSucursalBancaria = pobjItem.IdItem
            CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.SucursalBancaria = pobjItem.Nombre
        End If
    End Sub

    Private Sub btnAnularOrden_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
            Select Case e.OriginalSource.Name()
                Case "btnAnularOrden"
                    mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("AnularOrden", "Ordenes de Tesorería", "anularorden", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, "CE", "", "")
                    Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
                    mobjBuscadorLst.ShowDialog()

                Case Else
            End Select
        End If
    End Sub

    Private Sub btnPendienteOrden_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
            Select Case e.OriginalSource.Name()
                Case "btnPendienteOrden"
                    mobjVM = CType(Me.DataContext, TesoreriaViewModel)
                    mobjVM.OrdenesPendientesTesoreria_Mostar()
                    'mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("pendienteorden", "Ordenes de Tesorería Pendientes", "pendienteorden", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, "CE")
                    'mobjBuscadorLst.Show()
                    '
                Case Else
            End Select
        End If
    End Sub

    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then
            Select Case mobjBuscadorLst.CampoBusqueda.ToLower
                Case "conceptoteso"
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.IDConcepto = mobjBuscadorLst.ItemSeleccionado.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.Detalle = mobjBuscadorLst.ItemSeleccionado.Nombre
                Case "nombresucursal"
                    CType(Me.DataContext, TesoreriaViewModel).ChequeTesoreriSelected.SucursalesBancolombia = mobjBuscadorLst.ItemSeleccionado.IdItem
                Case "anularorden"
                    CType(Me.DataContext, TesoreriaViewModel).AnulandoOrden.ConsecutivoConsignacion = mobjBuscadorLst.ItemSeleccionado.IdItem
                    CType(Me.DataContext, TesoreriaViewModel).AnulandoOrden.NombreCliente = mobjBuscadorLst.ItemSeleccionado.Nombre
                    CType(Me.DataContext, TesoreriaViewModel).AnulandoOrden.ValorSaldo = mobjBuscadorLst.ItemSeleccionado.CodItem
                    mobjVM = CType(Me.DataContext, TesoreriaViewModel)
                    mobjVM.AnularOrden()
                Case Else
            End Select
        End If
    End Sub

#End Region

    Private Sub C1NumericBox_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim IDDigitado As Nullable(Of Integer) = CType(sender, A2Utilidades.A2NumericBox).Value

            If IDDigitado > 0 Then
                CType(Me.DataContext, TesoreriaViewModel).buscarCompania(IDDigitado, "buscarCompaniaEncabezado")
            Else
                CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreCompania = String.Empty
                CType(Me.DataContext, TesoreriaViewModel).LimpiarConsecutivoCompania()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del banco", Me.ToString(), "C1NumericBox_LostFocus", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub C1NumericBox_LostFocusBusqueda(sender As Object, e As RoutedEventArgs)
        Try
            Dim IDDigitado As Nullable(Of Integer) = CType(sender, A2Utilidades.A2NumericBox).Value

            If IDDigitado > 0 Then
                CType(Me.DataContext, TesoreriaViewModel).buscarCompaniaBusqueda(IDDigitado, "buscarCompaniaEncabezado")
            Else
                CType(Me.DataContext, TesoreriaViewModel).CamposBusquedaTesoreria.NombreCompania = String.Empty
                CType(Me.DataContext, TesoreriaViewModel).LimpiarConsecutivoCompaniaBusqueda()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del banco", Me.ToString(), "C1NumericBox_LostFocusBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Dim logPosocionoDebito As Boolean = True
    Private Sub ValorDebido_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            logPosocionoDebito = True
            CType(sender, C1.WPF.C1NumericBox).Select(0, e.OriginalSource.Text.Length)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ValorDebido_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected) And CType(Me.DataContext, TesoreriaViewModel).Editando Then
                If logPosocionoDebito Then
                    If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.Credito) Then
                        CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.Credito = Nothing
                    End If

                    logPosocionoDebito = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar los datos del detalle.", Me.ToString(), "ValorDebido_ValueChanged", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Dim logPosocionoCredito As Boolean = True
    Private Sub ValorCredito_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            logPosocionoCredito = True
            CType(sender, C1.WPF.C1NumericBox).Select(0, e.OriginalSource.Text.Length)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmbCuentaclientes_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbCuentaclientes.KeyDown
        'If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
        '    e.Handled = True
        'Else

        '    strcuentabancaria = strcuentabancaria + Convert.ToChar(e.ToString(e.Key))
        'End If
        'Dim TEXTO As String
        'TEXTO = e.Key.ToString

        'If Not (TEXTO = "D0" Or TEXTO = "D1" Or TEXTO = "D2" Or TEXTO = "D3" Or TEXTO = "D4" Or TEXTO = "D5" Or TEXTO = "D6" Or TEXTO = "D7" Or TEXTO = "D8" Or TEXTO = "D9") Then
        '    e.Handled = True
        'Else
        '    strcuentabancaria = strcuentabancaria + Replace(TEXTO, "D", "")
        'End If
    End Sub

    'Private Sub cmbCuentaclientes_KeyPress(Sender As Object, e As KeyEventArgs) Handles cmbCuentaclientes.KeyDown
    '    Dim TEXTO As String
    '    TEXTO = e.Key.ToString
    '    If e.Key.ToString = "D" Then

    '    End If

    '    strcuentabancaria = strcuentabancaria + Convert.ToChar(e.Key)
    'End Sub

    Private Sub cmbCuentaclientes_LostFocus(sender As Object, e As RoutedEventArgs) Handles cmbCuentaclientes.LostFocus
        If Not String.IsNullOrEmpty(strcuentabancaria) Then
            CType(Me.DataContext, TesoreriaViewModel).llenarcuentacombo(strcuentabancaria)
            strcuentabancaria = ""
            cmbCuentaclientes.Text = CType(Me.DataContext, TesoreriaViewModel).CuentaContableCliente
        End If
    End Sub


    Private Sub ValorCredito_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected) And CType(Me.DataContext, TesoreriaViewModel).Editando Then
                If logPosocionoCredito Then
                    If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.Debito) Then
                        CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.Debito = Nothing
                    End If

                    logPosocionoCredito = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar los datos del detalle.", Me.ToString(), "ValorCredito_ValueChanged", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
End Class
