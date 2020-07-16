Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl


Partial Public Class OrdenesRFOFView
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
    Private logcargainicial As Boolean = True
    Private WithEvents mobjVM As OrdenesOFViewModel
    Private WithEvents mobjSeleccionarTipo As OrdenesTipoOrdenView

#End Region

#Region "Propiedades"

    Private _mintClaseOrden As OrdenesOFViewModel.ClasesOrden
    Public Property ClaseOrden As OrdenesOFViewModel.ClasesOrden
        Get
            Return (_mintClaseOrden)
        End Get
        Set(ByVal value As OrdenesOFViewModel.ClasesOrden)
            If mlogInicializado = False Then
                '// Esta propiedad no se deja modificar después de inicializado el control
                _mintClaseOrden = value
            End If
        End Set
    End Property

#End Region

#Region "Inicializaciones"

    Public Sub New()

        Dim pintClaseOrden As OrdenesOFViewModel.ClasesOrden = OrdenesOFViewModel.ClasesOrden.C

        Dim objA2VM As A2UtilsViewModel

        Try
            _mintClaseOrden = pintClaseOrden

            Select Case _mintClaseOrden
                Case OrdenesOFViewModel.ClasesOrden.A
                    '// Cargar los datos para los combos específicos de órdenes de acciones
                    strClaseCombos = "Ord_Acciones"
                Case OrdenesOFViewModel.ClasesOrden.C
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

        Me.DataContext = New OrdenesOFViewModel
InitializeComponent()

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
            If logcargainicial Then
                logcargainicial = False
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1

                If mobjVM Is Nothing Then
                    mobjVM = CType(Me.DataContext, OrdenesOFViewModel)
                    mobjVM.NombreView = Me.ToString
                    mobjVM.ClaseOrden = Me.ClaseOrden
                    mobjVM.ListaCombosEsp = mstrDicCombosEspecificos
                    mobjVM.NombreInicioCombos = strClaseCombos

                    If mlogInicializado = False Then
                        Select Case _mintClaseOrden
                            Case OrdenesOFViewModel.ClasesOrden.A
                                '// Cargar los datos para los combos específicos de órdenes de acciones
                                CType(Me.Resources("A2VM"), A2UtilsViewModel).actualizarCombosEspecificos("Ord_Acciones")
                            Case OrdenesOFViewModel.ClasesOrden.C
                                '// Cargar los datos para los combos específicos de órdenes de renta fija
                                CType(Me.Resources("A2VM"), A2UtilsViewModel).actualizarCombosEspecificos("Ord_RentaFija")
                            Case Else

                        End Select

                        mlogInicializado = True
                    End If
                End If
            Else
                If Not CType(Me.DataContext, OrdenesOFViewModel).Editando Then
                    Me.mobjVM.RefrescarOrden()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

#End Region

#Region "Eventos controles"

    ''' <summary>
    ''' (SLB) cuando se modifica la fecha de la orden se debe verificar si la Fecha es un dia habil
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FechaOrden_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, OrdenesOFViewModel).VerificarFechaValida()
    End Sub

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

    Private Sub txtNroOrdenBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not (e.Key > 95 And e.Key < 106) And Not (e.Key = 9) Then
            e.Handled = True
        End If
    End Sub

    'Private Sub dgInstruccionesOrden_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
    '    If CType(Me.DataContext, OrdenesOFViewModel).Editando Then
    '        e.Handled = True
    '    Else
    '        e.Handled = False
    '    End If
    'End Sub

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
                        Me.mobjVM.actualizarNemotecnicoOrden(pobjEspecie)
                        Me.mobjVM.OrdenOFSelected.FechaEmision = pobjEspecie.Emision
                        Me.mobjVM.OrdenOFSelected.FechaVencimiento = pobjEspecie.Vencimiento
                        Me.mobjVM.OrdenOFSelected.Modalidad = pobjEspecie.CodModalidad
                        Me.mobjVM.OrdenOFSelected.TasaNominal = pobjEspecie.TasaEfectiva
                        Me.mobjVM.OrdenOFSelected.TasaInicial = pobjEspecie.TasaFacial
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
        If mobjVM.OrdenOFSelected.Clase = "C" Then
            Select Case mobjVM.OrdenOFSelected.Objeto
                Case "3", "4", "RP" '"1", "2","SI",
                    Buscador_Especies.ClaseOrden = 2 'Accion
                Case Else
                    Buscador_Especies.ClaseOrden = 1 'Renta Fija
            End Select
        End If
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not CType(Me.DataContext, OrdenesOFViewModel)._mlogMostrarTodosReceptores Then
            Dim obj As A2ComunesControl.BuscadorGenericoListaButon = CType(sender, A2ComunesControl.BuscadorGenericoListaButon)
            obj.TipoItem = "receptoresclientes"
            obj.Agrupamiento = CType(Me.DataContext, OrdenesOFViewModel).ComitenteSeleccionado.IdComitente
        End If
    End Sub

#End Region

#Region "Envetos acciones propias de la orden"

    Private Sub DatePicker_SelectedDateChanged_Orden(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Try
            Me.mobjVM.calcularDiasOrden(OrdenesOFViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
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
        If mobjSeleccionarTipo.DialogResult Then
            If mobjSeleccionarTipo.TipoOrden Is Nothing Then
                Me.mobjVM.NuevoRegistro()
            ElseIf mobjSeleccionarTipo.TipoOrden = OrdenesOFViewModel.TiposOrden.C.ToString() Then
                Me.mobjVM.generarNuevaCompra()
            Else
                Me.mobjVM.generarNuevaVenta()
            End If
        Else
            Me.mobjVM.CancelarEditarRegistro()
        End If
        mobjSeleccionarTipo = Nothing
    End Sub

    Private Sub mobjVM_seleccionarTipoOrden() Handles mobjVM.seleccionarTipoOrden
        Try
            'mobjSeleccionarTipo = New OrdenesTipoOrdenView(Me.ClaseOrden, Me.mobjVM.ListaCombosEsp)
            mobjSeleccionarTipo = New OrdenesTipoOrdenView()
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

    Private Sub Cantidad_LostFocus(sender As Object, e As RoutedEventArgs)
        If mobjVM.logRegistroLibroOrdenes And mobjVM.OrdenOFSelected.Cantidad < mobjVM.dblValorCantidadLibroOrden Then
            mostrarMensaje("No se puede modificar la cantidad por un valor inferior.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            mobjVM.OrdenOFSelected.Cantidad = mobjVM.dblValorCantidadLibroOrden
        End If
    End Sub

    Private Sub EfectivaInferior_LostFocus(sender As Object, e As RoutedEventArgs)
        If mobjVM.OrdenOFSelected.EfectivaInferior > mobjVM.OrdenOFSelected.EfectivaSuperior And mobjVM.OrdenOFSelected.EfectivaSuperior > 0 Then
            mostrarMensaje("Ingrese un valor menor o igual a la efectiva superior.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            mobjVM.OrdenOFSelected.EfectivaInferior = mobjVM.OrdenOFSelected.EfectivaSuperior
        End If
    End Sub

    Private Sub EfectivaSuperior_LostFocus(sender As Object, e As RoutedEventArgs)
        If mobjVM.OrdenOFSelected.EfectivaSuperior < mobjVM.OrdenOFSelected.EfectivaInferior And mobjVM.OrdenOFSelected.EfectivaInferior > 0 Then
            mostrarMensaje("Ingrese un valor mayor o igual a la efectiva inferior.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            mobjVM.OrdenOFSelected.EfectivaSuperior = mobjVM.OrdenOFSelected.EfectivaInferior
        End If
    End Sub

    Private Sub DiasVencimientoInferior_LostFocus(sender As Object, e As RoutedEventArgs)
        If mobjVM.OrdenOFSelected.DiasVencimientoInferior > mobjVM.OrdenOFSelected.DiasVencimientoSuperior And mobjVM.OrdenOFSelected.DiasVencimientoSuperior > 0 Then
            mostrarMensaje("Ingrese un valor menor o igual al día vencimiento superior.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            mobjVM.OrdenOFSelected.DiasVencimientoInferior = mobjVM.OrdenOFSelected.DiasVencimientoSuperior
        End If
    End Sub

    Private Sub DiasVencimientoSuperior_LostFocus(sender As Object, e As RoutedEventArgs)
        If mobjVM.OrdenOFSelected.DiasVencimientoSuperior < mobjVM.OrdenOFSelected.DiasVencimientoInferior And mobjVM.OrdenOFSelected.DiasVencimientoInferior > 0 Then
            mostrarMensaje("Ingrese un valor mayor o igual al día vencimiento inferior.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            mobjVM.OrdenOFSelected.DiasVencimientoSuperior = mobjVM.OrdenOFSelected.DiasVencimientoInferior
        End If
    End Sub

#End Region

    Private Sub Cumplimiento_CalendarOpened(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                If Not IsNothing(mobjVM.OrdenOFSelected) Then
                    If IsNothing(mobjVM.OrdenOFSelected.FechaCumplimiento) Then
                        CType(sender, DatePicker).DisplayDate = Now.Date
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el cumplimiento", Me.Name, "Cumplimiento_CalendarOpened", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class


