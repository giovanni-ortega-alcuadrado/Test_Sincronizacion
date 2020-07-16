
Imports Microsoft.Win32
Imports Telerik.Windows.Controls

Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.Infraestructura
Imports System.IO
Imports A2ComunesImportaciones
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Threading
Imports A2CFUtilitarios


Partial Public Class CompaniasView
    Inherits UserControl

    ''' <summary>
    ''' ID caso de prueba:  CP001, CP002. ventana con estilos metro y modo lista 
    ''' Eventos creados para la comunicación con las clases CompaniasView y CompaniasViewModel
    ''' Pantalla Compañía (Cálculos Financieros)
    ''' </summary>
    ''' <remarks>Javier Pardo (Alcuadrado S.A.) - 30 de Julio 2015</remarks>

#Region "Variables"

    Private mobjVM As CompaniasViewModel
    Private mlogInicializar As Boolean = True

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

        Me.DataContext = New CompaniasViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1


                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, CompaniasViewModel)
            mobjVM.NombreView = Me.ToString

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            Await mobjVM.inicializar()
        End If



    End Sub

#End Region

#Region "Eventos de controles"

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

    Private Sub seleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ctrlCliente_comitente_Dataform_Busqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                'Me.mobjVM.cb.lngIDComitente = pobjComitente.CodigoOYD
                Me.mobjVM.cb.strNombre = pobjComitente.Nombre
                Me.mobjVM.cb.strNumeroDocumento = pobjComitente.NroDocumento
                Me.mobjVM.cb.strTipoDocumento = pobjComitente.CodTipoIdentificacion
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitente_Dataform_Edicion(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                Me.mobjVM.strCodigoOYDEncabezado = pobjComitente.IdComitente
                Me.mobjVM.EncabezadoSeleccionado.strNombre = pobjComitente.Nombre
                'Me.mobjVM.strCodigoOyD = pobjComitente.CodigoOYD
                Me.mobjVM.EncabezadoSeleccionado.strNumeroDocumento = pobjComitente.NroDocumento
                Me.mobjVM.EncabezadoSeleccionado.strTipoDocumento = pobjComitente.CodTipoIdentificacion

                'Cargar los codigos oyd Referentes al documento del encabezado
                ' Await Me.mobjVM.ConsultarCodigosOyD(pobjComitente.NroDocumento)
                'Me.mobjVM.ValidarCodigosOyDExistentes(pobjComitente.NroDocumento)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlClientePartidas_comitente_Dataform_Edicion(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                'Me.mobjVM.EncabezadoSeleccionado.lngIDComitente = pobjComitente.CodigoOYD
                ' Me.mobjVM.strCodOyDPartidas = pobjComitente.IdComitente
                Me.mobjVM.EncabezadoSeleccionado.strNombreclientepartidas = pobjComitente.Nombre
                Me.mobjVM.EncabezadoSeleccionado.strNumeroDocumentoClientePartidas = pobjComitente.NroDocumento
                Me.mobjVM.EncabezadoSeleccionado.lngIDComitenteClientePartidas = pobjComitente.CodigoOYD
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del cliente partidas", Me.Name, "ctrlCliente_clientepartidasAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub btnLimpiarCliente_Dataform_Edicion_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.BorrarCliente = True
                'Me.mobjVM.EncabezadoSeleccionado.lngIDComitente = Nothing
                Me.mobjVM.EncabezadoSeleccionado.strNombre = Nothing
                Me.mobjVM.EncabezadoSeleccionado.strNumeroDocumento = Nothing
                Me.mobjVM.BorrarCliente = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Dataform_Busqueda_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.BorrarCliente = True
                'Me.mobjVM.cb.lngIDComitente = Nothing
                Me.mobjVM.cb.strNombre = Nothing
                Me.mobjVM.cb.strNumeroDocumento = Nothing
                Me.mobjVM.BorrarCliente = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos detalles"
    Private Sub Buscador_finalizoBusquedaClientesdetalle(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            If Me.mobjVM.HabilitarEdicionDetalle Then
                CType(Me.DataContext, CompaniasViewModel)._mLogBuscarClienteDetalle = False
                CType(Me.DataContext, CompaniasViewModel).DetalleSeleccionado.lngIDComitente = pobjItem.IdComitente
                CType(Me.DataContext, CompaniasViewModel).DetalleSeleccionado.strNombre = pobjItem.Nombre
                CType(Me.DataContext, CompaniasViewModel).DetalleSeleccionado.strNroDocumento = pobjItem.NroDocumento
                CType(Me.DataContext, CompaniasViewModel).DetalleSeleccionado.logCompania = False
                CType(Me.DataContext, CompaniasViewModel)._mLogBuscarClienteDetalle = True

            End If
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "gestores"
                    CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.intIDGestor = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.strNombreGestor = pobjItem.Nombre

                    'CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.lngIDPoblacion = CType(pobjItem.IdItem, Integer)
                    'CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.strDescripcionPoblacion = pobjItem.Nombre
                Case "monedas"
                    CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.intIDMoneda = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.strNombreMoneda = pobjItem.Nombre
                Case "monedascomision"
                    CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.intIDMonedaComision = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.strNombreMonedaComision = pobjItem.Nombre
                Case "compania"
                    If Me.mobjVM.HabilitarEdicionDetalle And Me.mobjVM.ValidarCompaniaRepetida(CInt(pobjItem.IdItem)) = True Then
                        CType(Me.DataContext, CompaniasViewModel)._mLogBuscarClienteDetalle = False
                        CType(Me.DataContext, CompaniasViewModel).DetalleSeleccionado.lngIDComitente = pobjItem.IdItem
                        CType(Me.DataContext, CompaniasViewModel).DetalleSeleccionado.strNroDocumento = pobjItem.CodItem
                        CType(Me.DataContext, CompaniasViewModel).DetalleSeleccionado.strNombre = pobjItem.CodigoAuxiliar
                        CType(Me.DataContext, CompaniasViewModel).DetalleSeleccionado.logCompania = True
                        CType(Me.DataContext, CompaniasViewModel)._mLogBuscarClienteDetalle = True
                    End If
                Case "idbanco"
                    CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.lngIDBancos = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.strBancosDescripcion = pobjItem.Descripcion

            End Select
        End If
    End Sub


    Private Sub Buscador_finalizoBusquedaClientesdetalleAutorizacion(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, CompaniasViewModel)._mLogBuscarClienteDetalle = False
            CType(Me.DataContext, CompaniasViewModel).DetalleAutorizacionesSeleccionado.lngIDComitente = pobjItem.IdComitente
            CType(Me.DataContext, CompaniasViewModel).DetalleAutorizacionesSeleccionado.strNombre = pobjItem.Nombre
            CType(Me.DataContext, CompaniasViewModel).DetalleAutorizacionesSeleccionado.strNroDocumento = pobjItem.NroDocumento
            CType(Me.DataContext, CompaniasViewModel)._mLogBuscarClienteDetalle = True
        End If
    End Sub



    ''' <history>
    ''' Descripción      : Se agrega método para validar que el usuario solo pueda ingresar Números en el campo Comitente.
    ''' </history>
    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
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



    ''' <summary>
    ''' Notifica que la propiedad lngIDPortafolio cambió sin tener que perder el foco.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtPortafolio_TextChanged(sender As Object, e As TextChangedEventArgs)
        CType(sender, TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource()
    End Sub



    Private Sub cbComisionAdministracion_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        'cbComisionAdministracion.IsDropDownOpen = False

    End Sub

    Private Sub cbTipoTabla_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        mobjVM.RefrescarDetalleAcumuladoComisiones(cbTipoTabla.SelectedValue.ToString())
        dgAcumuladoComisiones.ItemsSource = mobjVM.ListaDetallePaginadaListaAcumulacionComisiones
    End Sub

    Private Sub cbOtrasComisiones_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        cbOtrasComisiones.IsDropDownOpen = False
    End Sub


    Private Sub cbTipoCliente_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbPactoPermanencia_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbTipoPlazo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub



    Private Sub cbFCompartimentos_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbPenalidad_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbFTipoParticipacion_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        'Dim sd = CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.strParticipacion
    End Sub

    Private Sub cbIdentificadorCuenta_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub chkManejaValorUnidad_Checked(sender As Object, e As RoutedEventArgs) Handles chkManejaValorUnidad.Checked

    End Sub

    Private Sub cbTipoDeOperacion_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbTipoDeTransaccion_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub chkManejaValorUnidad_Unchecked(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnNuevoTes_Click(sender As Object, e As RoutedEventArgs)
        'If CType(Me.DataContext, CompaniasViewModel).ListaDetalleTesoreria Is Nothing Then
        '    CType(Me.DataContext, CompaniasViewModel).ListaDetalleTesoreria = New List(Of DetalleTesoreria)
        'End If
        'CType(Me.DataContext, CompaniasViewModel).ListaDetalleTesoreria.Add(New DetalleTesoreria With
        '                                                                     {
        '                                                                         .strTipoOperacion = CType(cbTipoDeOperacion.SelectedItem, ComboBoxItem).Content.ToString,
        '                                                                         .strTipoTransaccion = CType(cbTipoDeTransaccion.SelectedItem, ComboBoxItem).Content.ToString,
        '                                                                         .strOperador = CType(cbOperador.SelectedItem, ComboBoxItem).Content.ToString,
        '                                                                         .dtHoraInicial = timeHoraIncial.Value.Value,
        '                                                                         .dtHoraFinal = timeHoraFinal.Value.Value,
        '                                                                         .dblMontoMinimo = txtMontoMin.Text,
        '                                                                         .dblMontoMaximo = txtMontoMax.text,
        '                                                                         .logAplicaGMF = chkGMF.IsChecked.Value
        '                                                                     })
        'CType(Me.DataContext, CompaniasViewModel).ListaDetalleTesoreria = CType(Me.DataContext, CompaniasViewModel).ListaDetalleTesoreria
        'cbTipoDeOperacion.SelectedIndex = -1
        'cbTipoDeTransaccion.SelectedIndex = -1
        'cbOperador.SelectedIndex = -1
        'timeHoraIncial.Value = Nothing
        'timeHoraFinal.Value = Nothing
        'txtMontoMin.Text = String.Empty
        'txtMontoMax.Text = String.Empty
        ''txtMontoMax.Value = vbEmpty
        'chkGMF.IsChecked = False

        CType(Me.DataContext, CompaniasViewModel).IsBusyTipoOperacion = True
        Dim cwTesoreria As New cwConfiguracionTesoreriaCompañiaView(CType(Me.DataContext, CompaniasViewModel), -1)
        Program.Modal_OwnerMainWindowsPrincipal(cwTesoreria)
        cwTesoreria.ShowDialog()
    End Sub


    Private Sub cbEntrada_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    End Sub

    Private Sub cbSalida_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    End Sub

    Private Sub cbTipoComision_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        MostrarControlesTablasComision()
    End Sub

    Private Sub cbdeexito_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbInscritoRNVE_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbOperador_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbFCP_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbAutorizaciones_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        cbAutorizaciones.IsDropDownOpen = False
    End Sub

    Private Sub btnNuevoGridFactor_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnNuevoPenalidad_Click(sender As Object, e As RoutedEventArgs)
        'If mobjVM.ListaPenalidad Is Nothing Then
        '    mobjVM.ListaPenalidad = New Collections.ObjectModel.ObservableCollection(Of CFCalculosFinancieros.CompaniasPenalidades)
        'End If
        'mobjVM.ListaPenalidad.Add(New With {.strInicio = " ", .strFin = " ", .strFactor = " "})
        'dtgPenalidades.ItemsSource = mobjVM.ListaPenalidad

        '------------------------------------------------------------------------------------------------------------------------------------------------
        '-- Ingresar un nuevo detalle en el detalle de Penalidad
        'ID caso de prueba:  CP022
        '------------------------------------------------------------------------------------------------------------------------------------------------


        mobjVM.IngresarDetallePenalidad()
        dgPenalidades.ItemsSource = mobjVM.ListaDetallePaginadaPenalidad
    End Sub

    Private Sub btnNuevoAcumuladoComisiones_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.IngresarDetalleAcumuladoComisiones()
        dgAcumuladoComisiones.ItemsSource = mobjVM.ListaDetallePaginadaListaAcumulacionComisiones
    End Sub

    Private Sub Buscador_finalizoBusquedaespecies(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.strIdEspecie = pobjItem.Nemotecnico
            CType(Me.DataContext, CompaniasViewModel).EncabezadoSeleccionado.strISIN = pobjItem.ISIN

        End If
    End Sub

    Private Sub Buscador_Especie_GotFocus(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnBorrarPenalidad_Click(sender As Object, e As RoutedEventArgs)
        '------------------------------------------------------------------------------------------------------------------------------------------------
        '-- Elimina el detalle del registro seleccionado del detalle penalidad
        'ID caso de prueba:  CP023
        '------------------------------------------------------------------------------------------------------------------------------------------------

        mobjVM.DetallePenalidadSeleccionado = CType(dgPenalidades.SelectedItem, CFCalculosFinancieros.CompaniasPenalidades)
        mobjVM.BorrarDetallePenalidad()
        dgPenalidades.ItemsSource = mobjVM.ListaDetallePaginadaPenalidad

    End Sub

    Private Sub btnBorrarAcumuladoComisiones_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.DetalleAcumuladoComisionesSeleccionado = CType(dgAcumuladoComisiones.SelectedItem, CompaniasAcumuladoComisiones)
        mobjVM.BorrarDetalleAcumulacionComsiones()
        dgAcumuladoComisiones.ItemsSource = mobjVM.ListaDetallePaginadaListaAcumulacionComisiones
    End Sub

    Private Sub cboGridPenalidad_IsDropDownOpenChanged(sender As Object, e As Object)
        'mobjVM.IngresarDetallePenalidad()
        'dgPenalidades.ItemsSource = mobjVM.ListaDetallePaginadaPenalidad
    End Sub


    Private Sub dgPenalidades_Loaded(sender As Object, e As RoutedEventArgs)
        If mobjVM.MostrarCampoImportar = False Then
            btnImportarPenalidad.Visibility = Visibility.Collapsed
        Else
            btnImportarPenalidad.Visibility = Visibility.Visible
        End If
        dgPenalidades.ItemsSource = mobjVM.ListaDetallePaginadaPenalidad
        'Thread.Sleep(3000)
        'myBusyPenalidades.IsBusy = False
        If mobjVM.HabilitarCamposlectura = True Then
            'dgPenalidades.IsReadOnly = True
            dgPenalidades.IsEnabled = True
            btnNuevoPenalidad.IsEnabled = True
            btnBorrarPenalidad.IsEnabled = True
            btnImportarPenalidad.IsEnabled = True

        Else
            'dgPenalidades.IsReadOnly = False
            dgPenalidades.IsEnabled = False
            btnNuevoPenalidad.IsEnabled = False
            btnBorrarPenalidad.IsEnabled = False
            btnImportarPenalidad.IsEnabled = False
        End If

    End Sub



    Private Sub btnEditarTes_Click(sender As Object, e As RoutedEventArgs)



        Dim cwTesoreria As New cwConfiguracionTesoreriaCompañiaView(CType(Me.DataContext, CompaniasViewModel), 1)
        Program.Modal_OwnerMainWindowsPrincipal(cwTesoreria)
        cwTesoreria.ShowDialog()
    End Sub





    Private Sub btnImportarPenalidad_Click(sender As Object, e As RoutedEventArgs)

        Dim strMsg As String = String.Empty

        'If IsNothing(dgPenalidades.ItemsSource) Then
        If CType(dgPenalidades.ItemsSource, System.Windows.Data.PagedCollectionView).IsEmpty = False Then
            strMsg = "Solo quedarán los datos que se ingrese en la importación, ¿Desea continuar?"
            A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntaDetallePenalidad)
        Else
            ImportarArchivo()
        End If


    End Sub


    Private Sub TerminoPreguntaDetallePenalidad(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                ImportarArchivo()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de cargar el archivo", _
            Me.ToString(), "TerminoPreguntaDetallePenalidad", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ImportarArchivo()
        Try
            Dim fu As New OpenFileDialog()
            fu.Filter = "Archivo CSV|*.csv|Archivo de Texto|*.txt"
            If fu.ShowDialog().Value Then


                '*** realizo las validaciones de seguridad ***
                Dim objValidarArchivo As ValidacionesArchivo = A2OpenFileDialog.EsUnArchivoValido(fu)
                If Not objValidarArchivo.EsValido Then
                    A2Utilidades.Mensajes.mostrarMensaje(objValidarArchivo.MensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Exit Sub
                End If
                '****

                myBusyPenalidades.BusyContent = "Procesando archivo"
                myBusyPenalidades.IsBusy = True

                'BI_ProgresoUpload.BusyContent = "Procesando archivo"
                'BI_ProgresoUpload.IsBusy = True

                'TODO: Leemos el archivo
                Dim lstPenalidadesArchivo As New List(Of CompaniasPenalidades)
                Dim objLinea As String
                Dim objPenalidadArchivo As String()
                Dim lstValidaciones As New List(Of String)
                Using fileStream As System.IO.Stream = fu.OpenFile
                    'TODO: validamos el archivo
                    Dim intDiasInicial As Integer
                    Dim intIdPenalidad As Integer
                    Dim intDiasFinal As Integer
                    Dim intCompaniaActual As Integer
                    Dim dblFactor As Double
                    Dim Fila As Integer
                    Dim Validador As Boolean
                    Using reader As New System.IO.StreamReader(fileStream)
                        Fila = 0
                        Do While reader.EndOfStream = False

                            objLinea = reader.ReadLine

                            objPenalidadArchivo = objLinea.Split(CChar(";"))
                            If Fila = 0 Then
                                If objPenalidadArchivo.Count() <> 4 Then
                                    lstValidaciones.Add("No tiene el número de columnas requeridas!")
                                End If


                                If Integer.TryParse(objPenalidadArchivo(0), intDiasInicial) Then
                                    lstValidaciones.Add("La primera fila debe ser el encabezado")
                                End If

                            Else
                                'valido formatos y columnas
                                Validador = True
                                intDiasInicial = 0
                                intDiasFinal = 0
                                intCompaniaActual = 0
                                dblFactor = 0


                                If objPenalidadArchivo.Count() <> 4 Then
                                    lstValidaciones.Add("La fila" & " " & Fila & " " & "No tiene el número de columnas requeridas")
                                    Validador = False
                                End If


                                If objPenalidadArchivo.Count > 0 Then
                                    If Not Integer.TryParse(objPenalidadArchivo(0), intDiasInicial) Then
                                        lstValidaciones.Add("La columna {1} días inicial no tiene el formato correcto" & " " & "Fila:" & " " & Fila)
                                        Validador = False
                                    End If
                                End If

                                If objPenalidadArchivo.Count > 1 Then
                                    If Not Integer.TryParse(objPenalidadArchivo(1), intDiasFinal) Then
                                        lstValidaciones.Add("La columna {2} días final no tiene el formato correcto" & " " & "Fila:" & " " & Fila)
                                        Validador = False
                                    End If
                                End If

                                If objPenalidadArchivo.Count > 2 Then

                                    If Not Double.TryParse(objPenalidadArchivo(2), dblFactor) Then
                                        lstValidaciones.Add("La columna {3} Factor no tiene el formato correcto" & " " & "Fila:" & " " & Fila)
                                        Validador = False
                                    End If
                                End If

                                If objPenalidadArchivo.Count > 3 Then
                                    If Not Integer.TryParse(objPenalidadArchivo(3), intCompaniaActual) Then
                                        lstValidaciones.Add("La columna {4} número compañía no tiene el formato correcto" & " " & "Fila:" & " " & Fila)
                                        Validador = False
                                    Else
                                        If mobjVM.ValidarCompaniaPenalidad(intCompaniaActual, Fila) = False Then
                                            lstValidaciones.Add("La columna {4} número compañía no es la misma de la compañía actual" & " " & "Fila:" & " " & Fila)
                                            Validador = False
                                        End If
                                    End If
                                End If


                                'DARM 20171101 SE PASAN LAS VALIDACIONES PARA  LA BASE DE DATOS
                                'If intDiasInicial <= 0 Then
                                '    lstValidaciones.Add("La columna {1} días inicial debe ser un número entero mayor que 0" & " " & "Fila:" & " " & Fila)
                                '    Validador = False
                                'End If

                                'If intDiasFinal <= 0 Then
                                '    lstValidaciones.Add("La columna {2} días final debe ser un número entero mayor que 0" & " " & "Fila:" & " " & Fila)
                                '    Validador = False
                                'End If

                                'If dblFactor < 0 Then
                                '    lstValidaciones.Add("La columna {3} factor debe ser un número mayor o igual 0" & " " & "Fila:" & " " & Fila)
                                '    Validador = False
                                'End If

                                'If intCompaniaActual = 0 Then
                                '    lstValidaciones.Add("La columna {4} compañía es un campo numérico requerido mayor que 0" & " " & "Fila:" & " " & Fila)
                                '    Validador = False
                                'End If


                                'If intDiasInicial > intDiasFinal Then
                                '    lstValidaciones.Add("El día final debe ser un valor numérico mayor o igual que el día inicial" & " " & "Fila:" & " " & Fila)
                                '    Validador = False
                                'End If



                                If Validador = True Then
                                    intIdPenalidad = -New Random().Next(0, 1000000) + Fila
                                    lstPenalidadesArchivo.Add(New CompaniasPenalidades With {.intIDPenalidad = intIdPenalidad, .intDiasInicial = intDiasInicial, .intDiasFinal = intDiasFinal, .dblFactor = Decimal.Round(CDec(dblFactor), 2)})
                                End If


                            End If
                            Fila = Fila + 1
                        Loop


                        'If lstPenalidadesArchivo.Count > 0 Then
                        '    lstPenalidadesArchivo = lstPenalidadesArchivo.OrderBy(Function(i) i.intDiasInicial).ToList
                        '    Dim K As Integer
                        '    Dim l As Integer
                        '    For K = 0 To lstPenalidadesArchivo.Count - 1
                        '        For l = K + 1 To lstPenalidadesArchivo.Count - 1
                        '            If Not lstPenalidadesArchivo(l).intDiasInicial > lstPenalidadesArchivo(K).intDiasFinal Then
                        '                lstValidaciones.Add("Existe un conflicto ya que el día inicial" & " " & lstPenalidadesArchivo(l).intDiasFinal & "," & " " & "no es mayor que el día final" & " " & lstPenalidadesArchivo(K).intDiasFinal)
                        '            End If
                        '        Next l
                        '    Next K
                        'End If



                    End Using
                End Using

                'TODO: si es correcto lo paso al grid
                If Not lstValidaciones.Any Then
                    '------------------------------------------------------------------------------------------------------------------------------------------------
                    '-- Se importa archivo con inconsistencias
                    'ID caso de prueba:  CP024
                    '------------------------------------------------------------------------------------------------------------------------------------------------

                    lstValidaciones.Add("Archivo procesado correctamente!")
                    mobjVM.ListaPenalidad = lstPenalidadesArchivo
                    dgPenalidades.ItemsSource = mobjVM.ListaDetallePaginadaPenalidad
                Else
                    '------------------------------------------------------------------------------------------------------------------------------------------------
                    '-- Se importa archivo sin inconsistencias
                    'ID caso de prueba:  CP025
                    '------------------------------------------------------------------------------------------------------------------------------------------------

                    lstValidaciones.Insert(0, "Archivo no paso validaciones! Revise las siguientes inconsistencia(s):")
                End If


                myBusyPenalidades.IsBusy = False
                'TODO: mostramos el resultado
                'BI_ProgresoUpload.IsBusy = False
                Dim viewImportacion As New cwCargaArchivos() With {.ListaMensajes = lstValidaciones}
                Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                viewImportacion.ShowDialog()

            End If
        Catch fi As IOException
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "El archivo no se puede subir esta siendo utilizado por otro proceso, verifique que no este abierto", _
                                Me.ToString(), Me.Name, Program.TituloSistema, Program.Maquina, fi)


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del cuadro de dialogo de apertura", _
                                           Me.ToString(), Me.Name, Program.TituloSistema, Program.Maquina, ex)

        Finally
            'BI_ProgresoUpload.IsBusy = False
            myBusyPenalidades.IsBusy = False
        End Try




    End Sub


    Private Sub btnNuevoLimites_Click(sender As Object, e As RoutedEventArgs)
        Dim cwLimites As New cwConfiguracionLimitesCompañia(CType(Me.DataContext, CompaniasViewModel), -1)
        Program.Modal_OwnerMainWindowsPrincipal(cwLimites)
        cwLimites.ShowDialog()
    End Sub

    Private Sub btnEditarLimites_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(mobjVM.DetalleLimitesSeleccionado) = False Then
            Dim cwLimites As New cwConfiguracionLimitesCompañia(CType(Me.DataContext, CompaniasViewModel), 1)
            Program.Modal_OwnerMainWindowsPrincipal(cwLimites)
            cwLimites.ShowDialog()
        End If
    End Sub



    'Private Sub TxtNombreCorto_KeyDown(sender As Object, e As KeyEventArgs)
    '    If (e.Key = 32) Then
    '        e.Handled = True
    '    Else
    '        ' mobjVM.FormatoCambio()
    '        'FormatoTexto()
    '    End If
    'End Sub

    'Private Sub TxtNombreCorto_TextChanged(sender As Object, e As TextChangedEventArgs)
    '    Try
    '        If Not IsNothing(sender) Then
    '            Dim objTextBox As TextBox = CType(sender, TextBox)
    '            If Not String.IsNullOrWhiteSpace(objTextBox.Text) Then
    '                Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.LetrasNumeros)
    '                Dim NameCompania As String = objTextBox.Text
    '                Dim Indicador As Integer

    '                Indicador = CInt(InStr(NameCompania, " ").ToString)


    '                If Not IsNothing(objValidacion) And Indicador > 0 Then
    '                    If objValidacion.TextoValido = False Then
    '                        objTextBox.Text = objValidacion.CadenaNueva
    '                        objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
    '                    End If
    '                End If
    '                ' mobjVM.FormatoCambio()
    '                'FormatoTexto()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.", _
    '                             Me.ToString(), "TxtNombreCorto_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    'Private Sub FormatoTexto()
    '    Try
    '        TxtNombreCorto.Text = TxtNombreCorto.Text.ToString.ToUpper()
    '        'TxtNombreCorto.Text = StrReverse(TxtNombreCorto.Text.ToString)
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.", _
    '                             Me.ToString(), "FormatoTexto", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Private Sub TxtNombreCorto_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key >= 48 And e.Key <= 57 Then
            e.Handled = False
        ElseIf e.Key >= 65 And e.Key <= 90 Then
            e.Handled = False
        ElseIf e.Key >= 96 And e.Key <= 122 Then
            e.Handled = False
        ElseIf e.Key >= 164 And e.Key <= 165 Then
            e.Handled = False
        ElseIf e.Key = 192 Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtNombreCorto_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.LetrasNumeros)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        Else
                            TxtNombreCorto.Text = TxtNombreCorto.Text.ToUpper()
                            TxtNombreCorto.SelectionStart = TxtNombreCorto.Text.Length
                            TxtNombreCorto.Text = CStr(IIf(TxtNombreCorto.Text <> "", Replace(TxtNombreCorto.Text, " ", ""), ""))
                        End If
                    Else
                        TxtNombreCorto.Text = TxtNombreCorto.Text.ToUpper()
                        TxtNombreCorto.SelectionStart = TxtNombreCorto.Text.Length
                        TxtNombreCorto.Text = CStr(IIf(TxtNombreCorto.Text <> "", Replace(TxtNombreCorto.Text, " ", ""), ""))
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.",
                                 Me.ToString(), "TextBox_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnEditarCondicionesTesoreria_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(mobjVM.DetalleTesoreriaCondicioneSeleccionado) = False Then
            Dim cwCondicionesTesoreria As New cwCondicionesTesoreriaCompañiaView(CType(Me.DataContext, CompaniasViewModel), 1)
            Program.Modal_OwnerMainWindowsPrincipal(cwCondicionesTesoreria)
            cwCondicionesTesoreria.ShowDialog()
        End If
    End Sub

    Private Sub btnNuevoCondcionesTes_Click(sender As Object, e As RoutedEventArgs)
        Dim cwCondicionesTesoreria As New cwCondicionesTesoreriaCompañiaView(CType(Me.DataContext, CompaniasViewModel), -1)
        Program.Modal_OwnerMainWindowsPrincipal(cwCondicionesTesoreria)
        cwCondicionesTesoreria.ShowDialog()
    End Sub

    Private Sub btnNuevoParametros_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnEditarParametrosCia_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(mobjVM.DetalleParametrosCompaniaSeleccionado) = False Then
            Dim cwParametrosCompania As New cwConfiguracionParametrosCompaniaView(CType(Me.DataContext, CompaniasViewModel), 1)
            Program.Modal_OwnerMainWindowsPrincipal(cwParametrosCompania)
            cwParametrosCompania.ShowDialog()
        End If
    End Sub

    Private Sub cbComisionGestor_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbComisionminima_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnLimpiarFiltro_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.txtFiltro.Text = String.Empty
            mobjVM.FiltrarInformacion(Me.txtFiltro.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar la información.", Me.Name, "btnLimpiarFiltro_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnFiltro_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.FiltrarInformacion(Me.txtFiltro.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar la información.", Me.Name, "btnFiltro_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = mobjVM.strTipoAgrupacion

    End Sub


   
    Private Sub cboGridAcumuladoComisiones_IsDropDownOpenChanged(sender As Object, e As Object)

    End Sub

    Private Sub btnNuevoEstructura_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.IngresarDetalle()
    End Sub

Private Sub btnBorrarEstructura_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarDetalle()
    End Sub

    Private Sub btnBorrarTes_Click_1(sender As Object, e As RoutedEventArgs)
        mobjVM.ValidarBorrarConfirmacionDetalle()
    End Sub

    Private Sub btnBorrarTes_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ValidarBorrarConfirmacionTesoreria()
    End Sub

    Private Sub btnBorrarLimites_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ValidarBorrarConfirmacionLimites()
    End Sub

Private Sub btnNuevoAutorizados_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.IngresarDetalleAutorizaciones()
    End Sub

Private Sub btnBorrarAutorizados_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarDetalleAutorizaciones()
    End Sub

Private Sub btnBorrarCondicionesTes_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ValidarBorrarConfirmacionTesoreriaCondiciones()
    End Sub

Private Sub btnNuevoCodigos_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.IngresarDetalle()
    End Sub

Private Sub btnBorrarCodigos_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarDetalle()
    End Sub

Private Sub dgAcumuladoComisiones_Loaded(sender As Object, e As RoutedEventArgs)
        
        dgAcumuladoComisiones.ItemsSource = mobjVM.ListaDetallePaginadaListaAcumulacionComisiones

        If Not IsNothing(mobjVM.ListaAcumulacionComisiones) Then
            If mobjVM.ListaAcumulacionComisiones.Count > 0 Then
                mobjVM.DetalleAcumuladoComisionesSeleccionado = mobjVM.ListaAcumulacionComisiones.Last
            End If
        End If

        If mobjVM.HabilitarEncabezado = True Then
            dgAcumuladoComisiones.IsEnabled = True
            btnNuevoAcumuladoComisiones.IsEnabled = True
            btnBorrarAcumuladoComisiones.IsEnabled = True
        Else
            dgAcumuladoComisiones.IsEnabled = False
            btnNuevoAcumuladoComisiones.IsEnabled = False
            btnBorrarAcumuladoComisiones.IsEnabled = False
        End If

    End Sub

    Private Sub cbAcumuladoComisiones_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        MostrarControlesTablasComision()
    End Sub

    Private Sub MostrarControlesTablasComision()

        If Not IsNothing(mobjVM.EncabezadoSeleccionado) Then

            If mobjVM.EncabezadoSeleccionado.strTipoComision = "TR" Then
                mobjVM.MostrarComboTipoTabla = Visibility.Visible
                mobjVM.MostrarTablaAcumuladoComision = Visibility.Visible

                If cbAcumuladoComisiones.SelectedValue = "AD" Then
                    mobjVM.MostrarComboTipoTabla = Visibility.Visible
                Else
                    mobjVM.MostrarComboTipoTabla = Visibility.Collapsed
                End If
            Else
                mobjVM.MostrarComboTipoTabla = Visibility.Collapsed
                mobjVM.MostrarTablaAcumuladoComision = Visibility.Collapsed
            End If

            If Not IsNothing(mobjVM.ListaAcumulacionComisiones) Then
                mobjVM.ListaAcumulacionComisiones.Clear()
            End If

            If Not IsNothing(mobjVM.ListaAcumulacionComisionesPorTipoTabla) Then
                mobjVM.ListaAcumulacionComisionesPorTipoTabla.Clear()
            End If

            dgAcumuladoComisiones.ItemsSource = Nothing

        End If

    End Sub

End Class
