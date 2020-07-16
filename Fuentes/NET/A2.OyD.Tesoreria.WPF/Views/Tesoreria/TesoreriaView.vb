
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: TesoreriaView.xaml.vb
'Generado el : 07/08/2011 15:52:29
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class TesoreriaView
    Inherits UserControl
    Private mintNroIntento As Integer = 0
    Private WithEvents buscadorClientes As A2ComunesControl.BuscadorClienteLista
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Private logcargainicial As Boolean = True
    Dim logEsOrdenEmergente As Boolean = False
    Dim intIDDocumentoEmergente As Nullable(Of Integer) = Nothing
    Dim strNombreConsecutivoEmergente As String = String.Empty
    Dim objTesoreriaEmergente As TesoreriaEmergenteEncabezado = Nothing
    Dim objVentanaEmergente As TesoreriaVentanaEmergenteView = Nothing

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Private mobjVM As TesoreriaViewModel
    Dim objA2VM As A2UtilsViewModel
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty
#End Region

#Region "Propiedades"

    Private _mintClaseTesoreria As TesoreriaViewModel.ClasesTesoreria
    Public Property ClaseTesoreria As TesoreriaViewModel.ClasesTesoreria
        Get
            Return (_mintClaseTesoreria)
        End Get
        Set(ByVal value As TesoreriaViewModel.ClasesTesoreria)
            If mlogInicializado = False Then
                '// Esta propiedad no se deja modificar después de inicializado el control
                _mintClaseTesoreria = value
            End If
        End Set
    End Property

#End Region

#Region "Inicializaciones"

    Public Sub New()
        Try
            objA2VM = New A2UtilsViewModel()
            Me.Resources.Add("A2VM", objA2VM)
        Catch ex As Exception

        End Try
        Me.DataContext = New TesoreriaViewModel
InitializeComponent()
    End Sub

    Public Sub New(ByVal pintModuloTesoreria As TesoreriaViewModel.ClasesTesoreria)

        'Dim objA2VM As A2UtilsViewModel
        Dim strClaseCombos As String = ""
        Dim strModuloTesoreria As String = ""

        Try
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_Notas"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.N)

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
            Dim strClaseCombos As String = ""
            Dim strModuloTesoreria As String = ""

            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_Notas"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.N)

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

#Region "Eventos"

    Private Sub btnDuplicar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        mobjVM = CType(Me.DataContext, TesoreriaViewModel)
        mobjVM.duplicarTesoreria()
    End Sub

    Private Sub btnSubirArchivoTesoreria_CargarArchivo(sender As OpenFileDialog, e As IO.Stream)
        Try
            mobjVM = CType(Me.DataContext, TesoreriaViewModel)
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    If Path.GetExtension(objDialog.FileName).Equals(".csv") Or Path.GetExtension(objDialog.FileName).Equals(".xls") Or Path.GetExtension(objDialog.FileName).Equals(".xlsx") Then
                        mobjVM.NombreArchivo = Path.GetFileName(objDialog.FileName)
                        mobjVM.ImportarArchivo()
                    Else
                        mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivoTesoreria_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub Tesoreria_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
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
            '    'SLB20131021 Implementación del Autofresh
            '    If Not CType(Me.DataContext, TesoreriaViewModel).Editando And CType(Me.DataContext, TesoreriaViewModel).visNavegando = "Visible" Then
            '        CType(Me.DataContext, TesoreriaViewModel).RefrescarTesoreria()
            '    End If
        End If
    End Sub
    Public Sub inicializa(Optional ByVal plogConsultar As Boolean = True)
        CType(Me.DataContext, TesoreriaViewModel).inicializar(plogConsultar)
    End Sub

    

    'Evento Creado para Traer los valores de buscador en el grid del campo concepto JRP 2012/11/07
    'Private Sub Button_Click_BuscadorLista(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
    '        mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("ConceptoTeso", "Conceptos Asociados", "ConceptoTeso", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreConsecutivo, "", "")
    '        mobjBuscadorLst.Show()
    '        
    '    End If
    'End Sub
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
                                                                                                True,
                                                                                                False,
                                                                                                False,
                                                                                                False)

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

    Private Sub Button_Click_BuscadorLista(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
            Select Case e.OriginalSource.Name()
                Case "Sucursal"
                    mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("Nombresucursal", "Sucursales Banco", "nombresucursal", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IDTesoreria, "", "")
                    Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
                    mobjBuscadorLst.ShowDialog()

                Case "conceptoteso"
                    If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
                        mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("ConceptoTeso", "Conceptos Asociados", "ConceptoTeso", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreConsecutivo, "", "")
                        Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
                        mobjBuscadorLst.ShowDialog()

                    End If
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
    Private Sub btnAnularOrden_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected) Then
            Select Case e.OriginalSource.Name()
                Case "btnAnularOrden"
                    mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("AnularOrden", "Ordenes de Tesorería", "anularorden", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, "N%", "", "")
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
                Case Else
            End Select
        End If
    End Sub

    'SLB20130307 Evento para imprimir los reportes
    Private Sub btnImprimir_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, TesoreriaViewModel).ImprimirReporte("ImprimirNotaContable")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para lanzar la impresión del recibo de caja", Me.Name, "btnImprimir_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D9) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D8) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D7) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D6) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D5) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D4) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D3) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D2) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D1) Or _
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

    Private Sub txtBancoBusqueda_Lostfocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If CType(sender, TextBox).Text = "" Then
            mobjVM = CType(Me.DataContext, TesoreriaViewModel)
            If Not IsNothing(mobjVM.DetalleTesoreriSelected) Then
                If Not IsNothing(mobjVM.DetalleTesoreriSelected.IDBanco) Then
                    mobjVM.DetalleTesoreriSelected.IDBanco = Nothing
                    mobjVM.DetalleTesoreriSelected.Nombre = String.Empty
                    Exit Sub
                End If
            End If
        End If
    End Sub

    'SLB20130801 Refrescar documento de Tesorería.
    Private Sub cmdRefrescar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, TesoreriaViewModel).RefrescarTesoreria()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para lanzar la orden al mercado a través de SAE", Me.Name, "cmdLanzarSAE_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
                                                                           GridBusqueda.FindName("Modificacion").IsChecked, GridBusqueda.FindName("Retiro").IsChecked, GridBusqueda.FindName("Todos").IsChecked, Nothing)
        End If
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

#End Region

#Region "Buscadores Notas Tesoreria"

    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCliente = False
            'SLB20140620 Esta logica la contiene el metodo del ViewModel
            CType(Me.DataContext, TesoreriaViewModel).ComitenteSeleccionadoDetalle(pobjComitente)
            'CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarNIT = False
            'CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.IDComitente = pobjComitente.IdComitente
            'CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.Nombre = pobjComitente.Nombre
            'CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.NIT = pobjComitente.NroDocumento
            'CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.IDBanco = Nothing
            'CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarNIT = True
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCliente = True
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then

            If pobjItem.EtiquetaCodItem = "Cuenta Contable" Then
                CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCuentaContable = False
                CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.IDCuentaContable = pobjItem.IdItem
                CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCuentaContable = True
            ElseIf pstrClaseControl = "compania" Then
                CType(Me.DataContext, TesoreriaViewModel).logConsultarCompania = False
                CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.IDCompania = pobjItem.IdItem
                CType(Me.DataContext, TesoreriaViewModel).TesoreriSelected.NombreCompania = pobjItem.Nombre
                CType(Me.DataContext, TesoreriaViewModel).logConsultarCompania = True
                CType(Me.DataContext, TesoreriaViewModel).ConsultarConsecutivosCompania()
            ElseIf pstrClaseControl = "companiabusqueda" Then
                CType(Me.DataContext, TesoreriaViewModel).CamposBusquedaTesoreria.IDCompania = pobjItem.IdItem
                CType(Me.DataContext, TesoreriaViewModel).CamposBusquedaTesoreria.NombreCompania = pobjItem.Nombre
                CType(Me.DataContext, TesoreriaViewModel).ConsultarConsecutivosCompaniaBusqueda()
            Else
                CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = False
                'SLB20140620 Esta logica la contiene el metodo del ViewModel
                CType(Me.DataContext, TesoreriaViewModel).BancoSeleccionadoDetalle(pobjItem)
                CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = True
            End If
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaCCOSTO(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCentroCostos = False
            CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.CentroCosto = pobjItem.IdItem
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarCentroCostos = True
        End If
    End Sub

    'SLB20130225 Evento para recibir la selección del buscador de nits
    Private Sub BuscadorGenerico_finalizoBusquedaNITS(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarNIT = False
            CType(Me.DataContext, TesoreriaViewModel).DetalleTesoreriSelected.NIT = pobjItem.CodItem
            CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarNIT = True
        End If
    End Sub

#End Region

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

    Dim logPosocionoDebito As Boolean = False
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

    Dim logPosocionoCredito As Boolean = False
    Private Sub ValorCredito_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            logPosocionoCredito = True
            CType(sender, C1.WPF.C1NumericBox).Select(0, e.OriginalSource.Text.Length)
        Catch ex As Exception

        End Try
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


