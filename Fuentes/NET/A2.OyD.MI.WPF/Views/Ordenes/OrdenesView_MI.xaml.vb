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
Imports A2Utilidades

Partial Public Class OrdenesView_MI
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
    Private WithEvents mobjVM As OrdenesViewModel
    Dim objA2VMLocal As A2UtilsViewModelEsp
    'Private WithEvents mobjSeleccionarTipo As OrdenesTipoOrdenView

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
        Dim objA2VM As A2UtilsViewModelEsp

        Try
            objA2VM = New A2UtilsViewModelEsp()
            Me.Resources.Add("A2VM", objA2VM)
            objA2VMLocal = objA2VM

            _mintClaseOrden = -1
            A2Utilidades.Mensajes.mostrarMensaje("La inicialización del control no se puede realizar porque no se recibió el tipo de órden (acciones o renta fija)", Program.TituloSistema)
            objA2VM.IsBusy = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un error con la inicialización del control, además no se recibió el tipo de órden (acciones o renta fija)", Program.TituloSistema)
        End Try

        Me.DataContext = New OrdenesViewModel
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pintClaseOrden As OrdenesViewModel.ClasesOrden)

        Dim objA2VM As A2UtilsViewModelEsp

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
            objA2VM = New A2UtilsViewModelEsp(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)
            objA2VMLocal = objA2VM

            If strClaseCombos.Equals(String.Empty) Then
                mlogErrorInicializando = True
            End If

            mlogInicializado = True
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New OrdenesViewModel
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


            logcargainicial = False
            cm.GridTransicion = grdGridForma
            cm.GridViewRegistros = datapager1
            cm.DF = df

            If mobjVM Is Nothing Then
                mobjVM = CType(Me.DataContext, OrdenesViewModel)
                mobjVM.NombreView = Me.ToString
                mobjVM.ClaseOrden = Me.ClaseOrden
                mobjVM.ListaCombosEsp = mstrDicCombosEspecificos
                mobjVM.NombreInicioCombos = strClaseCombos
                mobjVM.objA2VMLocal = objA2VMLocal

                If mlogInicializado = False Then
                    Select Case _mintClaseOrden
                        Case OrdenesViewModel.ClasesOrden.A
                            '// Cargar los datos para los combos específicos de órdenes de acciones
                            CType(Me.Resources("A2VM"), A2UtilsViewModelEsp).actualizarCombosEspecificos("Ord_Acciones")
                        Case OrdenesViewModel.ClasesOrden.C
                            '// Cargar los datos para los combos específicos de órdenes de renta fija
                            CType(Me.Resources("A2VM"), A2UtilsViewModelEsp).actualizarCombosEspecificos("Ord_RentaFija")
                        Case Else

                    End Select

                    mlogInicializado = True
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

#End Region

#Region "Eventos controles"

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            df.CancelEdit()
            ''If Not IsNothing(df.ValidationSummary) Then
            '    df.ValidationSummary.DataContext = Nothing
            'End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cacelar la edición de la orden", Me.Name, "", "CancelarEditarRegistro_Click", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        Try
            df.ValidateItem()
            'If df.ValidationSummary.HasErrors Then
            '    df.CancelEdit()
            'Else
            '    df.CommitEdit()
            'End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al confirmar la edición de la orden", Me.Name, "", "CancelarEditarRegistro_Click", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtNroOrdenBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not (e.Key > 95 And e.Key < 106) And Not (e.Key = 9) Then
            e.Handled = True
        End If
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
                        Me.mobjVM.actualizarNemotecnicoOrden(pobjEspecie)
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

    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorEspecieLista

    Private Sub Button_Click_BuscadorLista(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, OrdenesViewModel).OrdenSelected) Then
            If Not (CType(Me.DataContext, OrdenesViewModel).OrdenSelected.IdBolsa).Equals(0) Then
                mobjBuscadorLst = New A2ComunesControl.BuscadorEspecieLista("", "", A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.A, A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A, "IdBolsa," + CStr(CType(Me.DataContext, OrdenesViewModel).OrdenSelected.IdBolsa), True, False, False)
                Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
                mobjBuscadorLst.ShowDialog()

            Else
                CType(Me.DataContext, OrdenesViewModel).validarBolsaParaEspecie()
            End If
        End If
    End Sub


    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        If Not mobjBuscadorLst.EspecieSeleccionada Is Nothing Then
            Me.mobjVM.actualizarNemotecnicoOrden(mobjBuscadorLst.EspecieSeleccionada)
            'Select Case pstrClaseControl.ToLower()
            '    Case "nemotecnico"
            '        Me.mobjVM.actualizarNemotecnicoOrden(pobjEspecie)
            '    Case "nemotecnicobuscar"
            '        Me.mobjVM.cb.NemotecnicoSeleccionado = mobjBuscadorLst.EspecieSeleccionada
            '        Me.mobjVM.cb.Nemotecnico = mobjBuscadorLst.EspecieSeleccionada.ISIN '.Nemotecnico
            '        Me.mobjVM.CambioItem("cb")
            'End Select
        End If
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

    'Private Sub mobjSeleccionarTipo_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjSeleccionarTipo.Closed
    '    If mobjSeleccionarTipo.DialogResult Then
    '        If mobjSeleccionarTipo.TipoOrden Is Nothing Then
    '            Me.mobjVM.NuevoRegistro()
    '        ElseIf mobjSeleccionarTipo.TipoOrden = OrdenesViewModel.TiposOrden.C.ToString() Then
    '            Me.mobjVM.generarNuevaCompra()
    '        Else
    '            Me.mobjVM.generarNuevaVenta()
    '        End If
    '    Else
    '        Me.mobjVM.CancelarEditarRegistro()
    '    End If
    '    mobjSeleccionarTipo = Nothing
    'End Sub

    'Private Sub mobjVM_seleccionarTipoOrden() Handles mobjVM.seleccionarTipoOrden
    '    Try
    '        'mobjSeleccionarTipo = New OrdenesTipoOrdenView(Me.ClaseOrden, Me.mobjVM.ListaCombosEsp)
    '        mobjSeleccionarTipo = New OrdenesTipoOrdenView()
    '        mobjSeleccionarTipo.Show()
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para activar la selección del tipo de orden de venta", Me.Name, "cmdVender_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón que permite ver el detalle de la orden en SAE
    ''' </summary>
    Private Sub verDetalleOrdenSAE(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub

#End Region

    Private Sub Precio_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, OrdenesViewModel).validarPrecioAPrecioInferior()
    End Sub

    Private Sub A2DatePicker_FechaOrden_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If Not IsNothing(CType(sender, A2DatePicker).SelectedDate) Then
                CType(Me.DataContext, OrdenesViewModel).VerificarFechaValida()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al salir del foco de la fecha de recepción", Me.Name, "A2DatePicker_FechaOrden_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Cuando se modifica la fecha de recepcion debe tomar la hora del sistema.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20130930</remarks>
    Private Sub A2DatePicker_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If Not IsNothing(CType(sender, A2DatePicker).SelectedDate) Then
                If CType(Me.DataContext, OrdenesViewModel).logActualizarFechaRecepcion Then
                    CType(Me.DataContext, OrdenesViewModel).logActualizarFechaRecepcion = False
                    CType(Me.DataContext, OrdenesViewModel).calcularFechaRecepcion()
                    CType(Me.DataContext, OrdenesViewModel).logActualizarFechaRecepcion = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al salir del foco de la fecha de recepción", Me.Name, "A2DatePicker_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not CType(Me.DataContext, OrdenesViewModel)._mlogMostrarTodosReceptores Then
            'df.FindName("Receptores").TipoItem = "receptoresclientes"
            'df.FindName("Receptores").Agrupamiento = CType(Me.DataContext, OrdenesViewModel).ComitenteSeleccionado.IdComitente

            Dim obj As A2ComunesControl.BuscadorGenericoListaButon = CType(sender, A2ComunesControl.BuscadorGenericoListaButon)

            obj.TipoItem = "receptoresclientes"
            If Not IsNothing(CType(Me.DataContext, OrdenesViewModel).ComitenteSeleccionado) Then
                obj.Agrupamiento = CType(Me.DataContext, OrdenesViewModel).ComitenteSeleccionado.IdComitente
            End If
        End If
    End Sub

End Class


