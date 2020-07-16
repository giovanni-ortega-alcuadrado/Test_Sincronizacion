Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: OrdenesView.xaml.vb
'Generado el : 07/21/2011 08:36:53
'Propiedad de Alcuadrado S.A. 2010

' Adaptar código generado a necesidades del sistema: Cristian Ciceri Muñetón - Julio/2011
'           - Modificación del diseño
'           - Propiedad para identificar si trabaja órdenes de acciones o de renta fija
'           - Referencia al view model mediante la variable mobjVM
'           - Incluir carga de combos específicos
'           - Manejador de error en el control
'           - Ajustes a funcionalidad del negocio

Imports A2Utilidades.Mensajes
Imports A2ComunesControl


Partial Public Class OrdenesRFView
    Inherits UserControl

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Private mstrNemotecnicoOriginal As String = String.Empty
    Private mstrComitenteOriginal As String = String.Empty
    Private mstrComitenteDigitado As String = String.Empty
    Private mstrModoMVVM_Actual As String = String.Empty
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty
    Public WithEvents mobjVM As OrdenesViewModel
    Private WithEvents mobjSeleccionarTipo As OrdenesTipoOrdenView
    Private logEsModal As Boolean = False

#End Region

#Region "Propiedades"

    Private _mintClaseOrden As OrdenesViewModel.ClasesOrden
    Public Property ClaseOrden As OrdenesViewModel.ClasesOrden
        Get
            Return (_mintClaseOrden)
        End Get
        Set(ByVal value As OrdenesViewModel.ClasesOrden)
            If mlogInicializado = False Then
                '// Esta propiedad no se deja modificar después de inicializado el control
                _mintClaseOrden = value
            End If
        End Set
    End Property

#End Region

#Region "Inicializaciones"

    Public Sub New()
        IniciarFormulario(False)
    End Sub

    Public Sub New(ByVal plogEsModal As Boolean)
        logEsModal = plogEsModal
        IniciarFormulario(True)
    End Sub

    Private Sub IniciarFormulario(ByVal plogInicializarModal As Boolean)

        'CambiarEstilosAplicacion estilos OYD.Net a Plus NAOC
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Dim pintClaseOrden As OrdenesViewModel.ClasesOrden = OrdenesViewModel.ClasesOrden.C

        Dim objA2VM As A2UtilsViewModel

        Try
            _mintClaseOrden = pintClaseOrden

            Select Case _mintClaseOrden
                Case OrdenesViewModel.ClasesOrden.A
                    '// Cargar los datos para los combos específicos de órdenes de acciones
                    strClaseCombos = "Ord_Acciones"
                Case OrdenesViewModel.ClasesOrden.C
                    '// Cargar los datos para los combos específicos de órdenes de renta fija
                    strClaseCombos = "Ord_RentaFija"
                Case Else
                    strClaseCombos = String.Empty
            End Select

            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

            If strClaseCombos.Equals(String.Empty) Then
                mlogErrorInicializando = True
            End If

            mlogInicializado = True
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New OrdenesViewModel
        If plogInicializarModal Then
            mobjVM = CType(Me.DataContext, OrdenesViewModel)
        End If
        InitializeComponent()

        AddHandler Me.SizeChanged, AddressOf CambioDePantalla
        Me.stackMenu.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Me.grdGridForma.Width = Application.Current.MainWindow.ActualWidth * 0.96

        If mlogErrorInicializando Then
            mostrarMensaje("No se puede inicializar el formulario de órdenes porque no se recibieron los parámetros de inicialización", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            cm.Visibility = Visibility.Collapsed
            cm.IsEnabled = False
            myBusyIndicator.BusyContent = "No se puede continuar la inicialización del formulario de órdenes"
            myBusyIndicator.IsBusy = True
        End If
    End Sub

    Private Sub Ordenes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            cm.GridTransicion = grdGridForma
            cm.GridViewRegistros = datapager1

            If mobjVM Is Nothing Or logEsModal Then
                mobjVM = CType(Me.DataContext, OrdenesViewModel)
                mobjVM.NombreView = Me.ToString
                'SE INDICA QUE ES LA VENTANA MODAL PARA CREAR DIRECTAMENTE EL REGISTRO
                mobjVM.logEsModal = logEsModal
                mobjVM.ClaseOrden = Me.ClaseOrden
                mobjVM.ListaCombosEsp = mstrDicCombosEspecificos
                mobjVM.NombreInicioCombos = strClaseCombos

                If mlogInicializado = False Then
                    Select Case _mintClaseOrden
                        Case OrdenesViewModel.ClasesOrden.A
                            '// Cargar los datos para los combos específicos de órdenes de acciones
                            CType(Me.Resources("A2VM"), A2UtilsViewModel).actualizarCombosEspecificos("Ord_Acciones")
                        Case OrdenesViewModel.ClasesOrden.C
                            '// Cargar los datos para los combos específicos de órdenes de renta fija
                            CType(Me.Resources("A2VM"), A2UtilsViewModel).actualizarCombosEspecificos("Ord_RentaFija")
                        Case Else

                    End Select

                    mlogInicializado = True
                End If
                'Else
                '    'SLB20131101 Implementación del Autofresh
                '    If Not CType(Me.DataContext, OrdenesViewModel).Editando And CType(Me.DataContext, OrdenesViewModel).visNavegando = "Visible" Then
                '        CType(Me.DataContext, OrdenesViewModel).RefrescarOrden()
                '    End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.stackMenu.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Me.grdGridForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
    End Sub

#End Region

#Region "Eventos controles"

    Private Sub A2DatePicker_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If CType(Me.DataContext, OrdenesViewModel).Editando Then
                If CType(Me.DataContext, OrdenesViewModel).logActualizarFechaRecepcion Then
                    CType(Me.DataContext, OrdenesViewModel).logActualizarFechaRecepcion = False
                    CType(Me.DataContext, OrdenesViewModel).calcularFechaRecepcion()
                    CType(Me.DataContext, OrdenesViewModel).logActualizarFechaRecepcion = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al salir de la fecha de recepción", Me.Name, "A2DatePicker_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub FechaOrden_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If CType(Me.DataContext, OrdenesViewModel).Editando Then
                If CType(Me.DataContext, OrdenesViewModel).logActualizarFechaElaboracion Then
                    CType(Me.DataContext, OrdenesViewModel).logActualizarFechaElaboracion = False
                    CType(Me.DataContext, OrdenesViewModel).VerificarFechaValida()
                    CType(Me.DataContext, OrdenesViewModel).logActualizarFechaElaboracion = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al salir de la fecha de la orden", Me.Name, "FechaOrden_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtNroOrdenBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Try
            If Not (e.Key > 47 And e.Key < 58) And Not (e.Key > 95 And e.Key < 106) And Not (e.Key = 9) And Not (e.Key = 13) Then
                e.Handled = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al salir ingresar el número de la orden en la búsqueda", Me.Name, "txtNroOrdenBusqueda_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'Private Sub dgInstruccionesOrden_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
    '    If CType(Me.DataContext, OrdenesViewModel).Editando Then
    '        e.Handled = True
    '    Else
    '        e.Handled = False
    '    End If
    'End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                mobjVM.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                mobjVM.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub cmdModificarIns_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, OrdenesViewModel).ModificarInstrucciones()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema con el bontón de modificar instrucciones", Me.Name, "cmdModificarIns_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Edicion_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = 13 And CType(Me.DataContext, OrdenesViewModel).Editando Then
                CType(Me.DataContext, OrdenesViewModel).ActualizarRegistro()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar modificar la orden", Me.Name, "Edicion_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Buscar_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = 13 Then
                Dim NroOrden As Integer? = Nothing
                Dim Version As Integer? = Nothing

                If Not String.IsNullOrEmpty(CType(dfBuscar.FindName("NroOrden").Text, String)) Then
                    NroOrden = CInt(dfBuscar.FindName("NroOrden").Text)
                End If

                If Not String.IsNullOrEmpty(CType(dfBuscar.FindName("Version").Text, String)) Then
                    Version = CInt(dfBuscar.FindName("Version").Text)
                End If

                CType(Me.DataContext, OrdenesViewModel).ConfirmarBuscarEnter(dfBuscar.FindName("Tipo").SelectedValue, NroOrden, Version, dfBuscar.FindName("IDComitente").Text, dfBuscar.FindName("IDOrdenante").Text,
                                                                             dfBuscar.FindName("FechaOrdenB").SelectedDate, String.Empty, dfBuscar.FindName("FormaPago").SelectedValue, dfBuscar.FindName("Objeto").SelectedValue, dfBuscar.FindName("CondicionesNegociacion").SelectedValue,
                                                                             dfBuscar.FindName("TipoInversion").SelectedValue, dfBuscar.FindName("TipoTransaccion").SelectedValue, dfBuscar.FindName("CanalRecepcion").SelectedValue, dfBuscar.FindName("MedioVerificable").SelectedValue, Nothing,
                                                                             dfBuscar.FindName("TipoLimite").SelectedValue, dfBuscar.FindName("VigenciaHasta").SelectedDate, String.Empty, String.Empty, dfBuscar.FindName("Estado").SelectedValue, String.Empty,
                                                                             Nothing, dfBuscar.FindName("EstadoMakerChecker").SelectedValue, dfBuscar.FindName("AnoOrden").SelectedValue, dfBuscar.FindName("AccionMakerChecker").SelectedValue)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la orden", Me.Name, "Buscar_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos para asignar cliente y especie"

    ''' <summary>
    ''' Se ejecuta cuando se dispara el evento comitenteAsignado en el buscador de clientes (control buscador clientes lista)
    ''' </summary>
    ''' <param name="pstrClaseControl">Permite identificar el llamado</param>
    ''' <param name="pobjComitente">Datos del comitente seleccionado en el buscador</param>
    ''' <remarks></remarks>
    Private Sub BuscadorClienteListaButon_comitenteAsignado(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                Select Case pstrClaseControl.ToLower()
                    Case "idcomitente"
                        Me.mobjVM.actualizarComitenteOrden(pobjComitente)
                    Case "idcomitentebuscar"
                        Me.mobjVM.cb.ComitenteSeleccionado = pobjComitente
                        Me.mobjVM.cb.IDComitente = pobjComitente.CodigoOYD
                        Me.mobjVM.CambioItem("cb")
                    Case "idordenantebuscar"
                        Me.mobjVM.cb.OrdenanteSeleccionado = pobjComitente
                        Me.mobjVM.cb.IDOrdenante = pobjComitente.CodigoOYD
                        Me.mobjVM.CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el comitente seleccionado", Me.Name, "BuscadorClienteLista_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorEspecieListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                Select Case pstrClaseControl.ToLower()
                    Case "nemotecnico"
                        If Me.mobjVM.HabilitarSeleccionISIN Then
                            Me.mobjVM.actualizarNemotecnicoOrden(pobjEspecie, True)
                        Else
                            If Me.mobjVM.SeleccionarUnISIN Then
                                If pobjEspecie.CantidadISIN = 1 Then
                                    Me.mobjVM.actualizarNemotecnicoOrden(pobjEspecie, True)
                                Else
                                    Me.mobjVM.actualizarNemotecnicoOrden(pobjEspecie, False)
                                End If
                            Else
                                Me.mobjVM.actualizarNemotecnicoOrden(pobjEspecie, True)
                            End If
                        End If

                    Case "nemotecnicobuscar"
                        Me.mobjVM.cb.NemotecnicoSeleccionado = pobjEspecie
                        Me.mobjVM.cb.Nemotecnico = pobjEspecie.Nemotecnico
                        Me.mobjVM.CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_nemotecnicoAsignado(pstrNemotecnico As System.String, pstrNombreNemotecnico As System.String)
        Try
            If Not IsNothing(Me.mobjVM) Then
                If Not String.IsNullOrEmpty(pstrNemotecnico) Then
                    Dim objEspecie As New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorEspecies
                    objEspecie.Nemotecnico = pstrNemotecnico
                    objEspecie.Especie = pstrNombreNemotecnico
                    'Me.mobjVM.actualizarNemotecnicoOrden(objEspecie, False)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            Me.mobjVM.actualizarItemOrden(pstrClaseControl, pobjItem)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' (SLB) Este evento permite que en Ordenes RF cuando se seleccione una Orden de Tipo REPO o Simultaneas busque las especies de Acciones,
    ''' para los demas Tipo de Ordenes de RF que busque las Especies de Renta Fija
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Buscador_Especies_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If mobjVM.OrdenSelected.Clase = "C" Then
                Select Case mobjVM.OrdenSelected.Objeto
                    Case "3", "4", "RP"
                        'Buscador_Especies.ClaseOrden = 2 'Accion
                        Buscador_Especies.ClaseOrden = 0 'Accion y Renta Fija SLB20131122
                    Case Else
                        Buscador_Especies.ClaseOrden = 1 'Renta Fija
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la especie de la orden", Me.Name, "Buscador_Especies_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Not CType(Me.DataContext, OrdenesViewModel)._mlogMostrarTodosReceptores Then
                Dim obj As A2ComunesControl.BuscadorGenericoListaButon = CType(sender, A2ComunesControl.BuscadorGenericoListaButon)
                obj.TipoItem = "receptoresclientes"
                obj.Agrupamiento = CType(Me.DataContext, OrdenesViewModel).ComitenteSeleccionado.IdComitente
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el receptor del cliente", Me.Name, "BuscadorGenericoListaButon_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Envetos acciones propias de la orden"

    Private Sub DatePicker_SelectedDateChanged_Orden(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Try
            Me.mobjVM.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular los días de vencimiento de la orden", Me.Name, "DatePicker_SelectedDateChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdComprar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mobjVM.generarNuevaCompra()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para generar la nueva orden de compra", Me.Name, "cmdComprar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdVender_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mobjVM.generarNuevaVenta()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para ala nueva orden de venta", Me.Name, "cmdVender_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdDuplicar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Me.mobjVM.duplicarOrden()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para duplicar la orden", Me.Name, "cmdDuplicar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdLanzarSAE_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Me.mobjVM.enviarPorSAE()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para lanzar la orden al mercado a través de SAE", Me.Name, "cmdLanzarSAE_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdRefrescar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Me.mobjVM.RefrescarOrden()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para lanzar la orden al mercado a través de SAE", Me.Name, "cmdLanzarSAE_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <sumary>
    ''' Ejecuta la impresión de la orden
    ''' </sumary>
    Private Sub cmdImprimir_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Me.mobjVM.imprimirOrden()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para lanzar la impresión de la orden", Me.Name, "cmdImprimir_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub mobjSeleccionarTipo_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjSeleccionarTipo.Closed
        Try
            If mobjSeleccionarTipo.DialogResult Then
                If mobjSeleccionarTipo.TipoOrden Is Nothing Then
                    Me.mobjVM.NuevoRegistro()
                ElseIf mobjSeleccionarTipo.TipoOrden = OrdenesViewModel.TiposOrden.C.ToString() Then
                    Me.mobjVM.generarNuevaCompra()
                Else
                    Me.mobjVM.generarNuevaVenta()
                End If
            Else
                Me.mobjVM.CancelarEditarRegistro()
            End If
            mobjSeleccionarTipo = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el tipo de la orden", Me.Name, "mobjSeleccionarTipo_Closed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub mobjVM_seleccionarTipoOrden() Handles mobjVM.seleccionarTipoOrden
        Try
            'mobjSeleccionarTipo = New OrdenesTipoOrdenView(Me.ClaseOrden, Me.mobjVM.ListaCombosEsp)
            mobjSeleccionarTipo = New OrdenesTipoOrdenView()
            Program.Modal_OwnerMainWindowsPrincipal(mobjSeleccionarTipo)
            mobjSeleccionarTipo.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para activar la selección del tipo de orden de venta", Me.Name, "cmdVender_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón que permite ver el detalle de la orden en SAE
    ''' </summary>
    Private Sub verDetalleOrdenSAE(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub

#End Region

#Region "Eventos para el Buscador de Comisiones"

    Private Sub BuscadorIdComision_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                CType(Me.DataContext, OrdenesViewModel).AdicionalesOrdeneSelected.IdOperacion = pobjItem.Nombre
                CType(Me.DataContext, OrdenesViewModel).AdicionalesOrdeneSelected.ComisionSugerida = CDbl(pobjItem.IdItem)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al finalizar la búsqueda de la comisión", Me.Name, "BuscadorIdComision_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub BuscadorIdComision_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If IsNothing(CType(Me.DataContext, OrdenesViewModel).OrdenSelected.FechaEmision) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la Fecha de Emisión", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(CType(Me.DataContext, OrdenesViewModel).OrdenSelected.FechaVencimiento) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la Fecha de Vencimiento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If CType(Me.DataContext, OrdenesViewModel).NemotecnicoSeleccionado.Nemotecnico = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la Especie de la Orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim obj As A2ComunesControl.BuscadorGenericoListaButon = CType(sender, A2ComunesControl.BuscadorGenericoListaButon)
            obj.Agrupamiento = Format(CType(Me.DataContext, OrdenesViewModel).OrdenSelected.FechaEmision, "MM/dd/yyyy") & "," &
                Format(CType(Me.DataContext, OrdenesViewModel).OrdenSelected.FechaVencimiento, "MM/dd/yyyy") & "."
            obj.TipoItem = "Comision" & "," & CType(Me.DataContext, OrdenesViewModel).NemotecnicoSeleccionado.Nemotecnico
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de levantar el buscador de Liquidaciones", Me.Name, "BuscadorIdComision_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            'MessageBox.Show(ex.ToString)
        End Try
    End Sub

#End Region


End Class


