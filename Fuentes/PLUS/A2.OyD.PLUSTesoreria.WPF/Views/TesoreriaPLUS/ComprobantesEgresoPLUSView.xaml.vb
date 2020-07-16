
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices


Partial Public Class ComprobantesEgresoPLUSView
    Inherits UserControl

    Dim objVMA2Utils As A2UtilsViewModel
    Private WithEvents objVMOrdenesTesoreria As TesoreriaViewModel_OYDPLUS
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Private mstrDicCombosEspecificos As String = String.Empty
    Private mstrRuta As String
    Private mabytArchivo As Byte()
    Private mstrArchivo As String
    Public ViewPopPupEdicion As OrdenGiroConsultaView = Nothing
    Private IDOrdenGiro As Integer = 0
    Private logXTesorero As Boolean = False
    Private logEditarOrdenPopup As Boolean = False
    Private logPendientePorAprobar As Boolean = False
    Private objViewModelTesorero As TesoreroViewModel_OYDPLUS = Nothing


#Region "Inicializaciones"

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            mlogInicializado = False
            Me.DataContext = New TesoreriaViewModel_OYDPLUS
            InitializeComponent()

            objVMOrdenesTesoreria = Me.LayoutRoot.DataContext

            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 275
            AddHandler Me.Unloaded, AddressOf View_Unloaded
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub New(ByVal pIdOrdenGiro As Integer, ByVal plogEditarOrden As Boolean, ByVal plogPendientePorAprobar As Boolean, ByVal pobjViewModelTesorero As TesoreroViewModel_OYDPLUS)
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            mlogInicializado = False
            Me.DataContext = New TesoreriaViewModel_OYDPLUS
            InitializeComponent()

            objVMOrdenesTesoreria = Me.LayoutRoot.DataContext

            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 275
            AddHandler Me.Unloaded, AddressOf View_Unloaded

            Me.IDOrdenGiro = pIdOrdenGiro
            Me.logXTesorero = True
            Me.logEditarOrdenPopup = plogEditarOrden
            Me.logPendientePorAprobar = plogPendientePorAprobar
            Me.objViewModelTesorero = pobjViewModelTesorero
            objVMOrdenesTesoreria.HabilitarTipoGMFTesorero = True 'JAPC20200424-CC20200364: para habilitar edicion GMF debido que se instancio desde el tesorero

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


#End Region


    Private Sub ComprobantesEgresoPLUSView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        Try
            cm.GridTransicion = grdGridForma
            cm.GridViewRegistros = datapager
            cm.DF = df
            If objVMOrdenesTesoreria Is Nothing Then
                objVMOrdenesTesoreria = CType(Me.DataContext, TesoreriaViewModel_OYDPLUS)
            End If

            objVMOrdenesTesoreria.ComprobantesEgresoPLUSView = Me

            If logXTesorero Then
                objVMOrdenesTesoreria.IsBusy = True
                If Me.logEditarOrdenPopup Then
                    objVMOrdenesTesoreria.MostrarControlMenuGuardar = True
                    objVMOrdenesTesoreria.logConfirmoTesorero = True
                Else
                    objVMOrdenesTesoreria.MostrarControlMenuGuardar = False
                End If

                objVMOrdenesTesoreria.ViewOrdenGiroPopPup = Me.ViewPopPupEdicion
                objVMOrdenesTesoreria.logXTesorero = Me.logXTesorero
                objVMOrdenesTesoreria.logEsOrdenPorAprobarPupup = Me.logPendientePorAprobar
                objVMOrdenesTesoreria.objViewModelTesorero = Me.objViewModelTesorero
                If Me.IDOrdenGiro > 0 Then
                    objVMOrdenesTesoreria.IdEncabezadoXTesorero = Me.IDOrdenGiro
                End If
            Else
                objVMOrdenesTesoreria.MostrarControlMenuGuardar = True
            End If

            'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "ComprobantesEgresoPLUSView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Cerrar(sender As Object, e As RoutedEventArgs)
        objVMOrdenesTesoreria.ViewOrdenGiroPopPup.Close()
    End Sub

    Private Sub CargarPopupCargaArchivoCheques(ByVal sender As Object, ByVal e As System.EventArgs, pstrNombreArchivo As String) Handles objVMOrdenesTesoreria.LanzarPopupCargaArchivoCheques
        Dim objImportarCheque As New ImportacionCheque(GSTR_CHEQUE, objVMOrdenesTesoreria, pstrNombreArchivo)
        If objVMOrdenesTesoreria.logHayEncabezado Then
            objVMOrdenesTesoreria.objWppImportacion = objImportarCheque
            Program.Modal_OwnerMainWindowsPrincipal(objImportarCheque)
            objImportarCheque.ShowDialog()

        Else
            mostrarMensaje("Primero Ingrese los datos de encabezado para poder Realizar el Proceso de Carga de Archivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            objVMOrdenesTesoreria.HabilitarEncabezado = True
            objVMOrdenesTesoreria.HabilitarImportacion = False
        End If

    End Sub

    Private Sub CargarPopupCargaArchivoTransferencia(ByVal sender As Object, ByVal e As System.EventArgs, pstrNombreArchivo As String) Handles objVMOrdenesTesoreria.LanzarPopupCargaArchivoTransferencia
        Dim objImportarTransferencia As New ImportacionCheque(GSTR_TRANSFERENCIA, objVMOrdenesTesoreria, pstrNombreArchivo)
        If objVMOrdenesTesoreria.logHayEncabezado Then
            objVMOrdenesTesoreria.objWppImportacion = objImportarTransferencia
            Program.Modal_OwnerMainWindowsPrincipal(objImportarTransferencia)
            objImportarTransferencia.ShowDialog()
        Else
            mostrarMensaje("Primero Ingrese los datos de encabezado para poder Realizar el Proceso de Carga de Archivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            objVMOrdenesTesoreria.HabilitarEncabezado = True
            objVMOrdenesTesoreria.HabilitarImportacion = False
        End If

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsNothing(objVMOrdenesTesoreria) Then
            If logXTesorero Then
                Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
                Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
                Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight + 275
            Else
                Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
                Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
                Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 275
            End If

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

    Private Sub btnSubirTransferencia_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            objVMOrdenesTesoreria.AbrirPopupCargaArchivosTransferencia(System.IO.Path.GetFileName(objDialog.FileName))
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cargar el archivo de Transferencia.", Me.Name, "btnSubirTransferencia_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnSubirCheque_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            objVMOrdenesTesoreria.AbrirPopupCargaArchivosCheques(System.IO.Path.GetFileName(objDialog.FileName))
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cargar el archivo de cheques.", Me.Name, "btnSubirCheque_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Expander_Collapsed(sender As System.Object, e As System.Windows.RoutedEventArgs)
        objVMOrdenesTesoreria.VerItemActual = Visibility.Visible
    End Sub

    Private Sub Expander_Expanded(sender As System.Object, e As System.Windows.RoutedEventArgs)
        objVMOrdenesTesoreria.VerItemActual = Visibility.Collapsed
    End Sub

    Private Sub TabControl_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim t = TryCast(sender, System.Windows.Controls.TabControl)
        If Not IsNothing(objVMOrdenesTesoreria) And Not IsNothing(t.SelectedItem) Then
            If t.SelectedItem.Name = "tabItemCheque" Then
                objVMOrdenesTesoreria.TabItemActual = "Cheques"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_CHEQUE)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Cheque"
            ElseIf t.SelectedItem.Name = "tabItemTransferencia" Then
                objVMOrdenesTesoreria.TabItemActual = "Transferencias"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_TRANSFERENCIA)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Transferencia"
            ElseIf t.SelectedItem.Name = "tabItemCarterasColectivas" Then
                objVMOrdenesTesoreria.TabItemActual = "Carteras Colectivas"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_CARTERASCOLECTIVAS)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Cartera"
            ElseIf t.SelectedItem.Name = "tabItemTrasladoFondos" Then
                objVMOrdenesTesoreria.TabItemActual = "Traslado Fondos"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_TRASLADOFONDOS)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Traslado Fondos"
            ElseIf t.SelectedItem.Name = "tabItemInternos" Then
                objVMOrdenesTesoreria.TabItemActual = "Internos"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_INTERNOS)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Interno"
            ElseIf t.SelectedItem.Name = "tabItemBloqueo" Then
                objVMOrdenesTesoreria.TabItemActual = "Bloqueo"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_BLOQUEO_RECURSOS)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Bloqueo"
            ElseIf t.SelectedItem.Name = "tabItemOYD" Then
                objVMOrdenesTesoreria.TabItemActual = "OYD"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_OYD)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "OYD"
            ElseIf t.SelectedItem.Name = "tabItemOperacionesEspeciales" Then
                objVMOrdenesTesoreria.TabItemActual = "OperacionesEspeciales"
                objVMOrdenesTesoreria.CalcularTotales(GSTR_OPERACIONES_ESPECIALES)
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "OperacionesEspeciales"
            ElseIf t.SelectedItem.Name = "tabItemInstrucciones" Then
                objVMOrdenesTesoreria.TabItemActual = "Instrucciones"
                objVMOrdenesTesoreria.ValorTotalGenerarActual = 0
                objVMOrdenesTesoreria.ValorTotalGMFActual = 0
                objVMOrdenesTesoreria.ValorTotalNETOActual = 0
                objVMOrdenesTesoreria.strOpcionConsutaSaldo = "Instrucciones"

                objVMOrdenesTesoreria.CambioItem("HabilitarDocumento")
                objVMOrdenesTesoreria.CambioItem("HabilitarCamposDireccion")
                objVMOrdenesTesoreria.CambioItem("HabilitarCamposConsignarInstrucciones")

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

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrClaseControl = "bancosnacionalesinstrucciones" Then
                    Me.objVMOrdenesTesoreria.strCodigoBancoInstrucciones = pobjItem.IdItem
                    Me.objVMOrdenesTesoreria.strDescripcionBancoInstrucciones = pobjItem.Descripcion
                End If
                If pstrClaseControl = "IdPoblacion" Then
                    Me.objVMOrdenesTesoreria.strCiudadInstrucciones = pobjItem.Nombre
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenerico_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'aca


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




    Private Sub configurarCtl(ByRef oobjCtlSubirArchivo As A2DocumentosWPF.A2SubirDocumento)
        'oobjCtlSubirArchivo.URLServicioDocumentos = "http://a2sql2005des:9292/A2ServicioDocs/"
        'oobjCtlSubirArchivo.Aplicacion = "OyD Plus"
        'oobjCtlSubirArchivo.Version = ""
        'oobjCtlSubirArchivo.Modulo = "OyD_Plus\Ordenes de Tesoreria\Ordenes de Giro"
        'oobjCtlSubirArchivo.Tema = "Ordenes de Giro"
        'oobjCtlSubirArchivo.Subtema = ""
        'oobjCtlSubirArchivo.TagsBusqueda = "Carta, autorización, pagos, giro, orden"
        'oobjCtlSubirArchivo.Titulo = "Carta de Autorización de pagos ordenes de giro"
        'oobjCtlSubirArchivo.Autor = ""
        'oobjCtlSubirArchivo.Descripcion = "Servicio documentos Ordenes de giro OyD Plus"
        'oobjCtlSubirArchivo.UsuarioActivo = Program.Usuario
        'oobjCtlSubirArchivo.Maquina = ""
        'If objVMOrdenesTesoreria.TesoreriaOrdenesPlusCE_Selected.lngID > 0 Then
        '    oobjCtlSubirArchivo.ClaveUnica = objVMOrdenesTesoreria.TesoreriaOrdenesPlusCE_Selected.lngID
        'End If

        'oobjCtlSubirArchivo.FiltroArchivos = "Todos los archivos (*.*)|*.*|Texto (*.txt)|*.txt|Excel 2010 (*.xlsx)|*.xlsx|Word 2010 (*.docx)|*.docx|Excel 2003 (.xls)|*.xls|Word 2003 (*.doc)|*.doc"
        'oobjCtlSubirArchivo.MostrarLog = False
        'oobjCtlSubirArchivo.TituloSistema = Application.Current.ToString
        'oobjCtlSubirArchivo.TextoBotonSubirArchivo = "Cargar archivo"
        'oobjCtlSubirArchivo.AnchoNombreArchivo = 200
        'oobjCtlSubirArchivo.MostrarNombreArchivo = True
        'oobjCtlSubirArchivo.MostrarMensajeFinalizacion = True
        'oobjCtlSubirArchivo.SoloCapturarRutaArchivo = True
    End Sub



    Private Sub ctlSubirArchivo_finalizoTransmisionArchivo(pstrArchivo As String, pstrRuta As String) Handles ctlSubirArchivo.finalizoTransmisionArchivo
        objVMOrdenesTesoreria.TraerOrdenes("REFRESCARPANTALLA", objVMOrdenesTesoreria.VistaSeleccionada)
    End Sub

    Private Sub ctlSubirArchivo_finalizoSeleccionArchivo(ByVal pstrArchivo As String, ByVal pstrRuta As String, ByVal pabytArchivo As Byte()) Handles ctlSubirArchivo.finalizoSeleccionArchivo
        Try
            objVMOrdenesTesoreria.mstrArchivo = pstrArchivo
            objVMOrdenesTesoreria.mstrRuta = pstrRuta

            If String.IsNullOrEmpty(pstrRuta) Then
                '/ Si no se recibe la ruta del archivo se guarda el arreglo de bytes que tiene el contenido del archivo.
                objVMOrdenesTesoreria.mabytArchivo = pabytArchivo
            Else
                '/ Si se recibe la ruta del archivo no se guarda el arreglo de bytes a menos que sea requerido porque consume más memoria y puede ser innecesario su almacenamiento.
                objVMOrdenesTesoreria.mabytArchivo = Nothing
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener la información del archivo.", Me.Name, "ctlSubirArchivo_finalizoSeleccionArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cm_EventoNuevoRegistro_1(sender As Object, e As RoutedEventArgs)
        'ConfigurarControlDocumentro()
    End Sub

    Private Sub NuevoTraslado(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenesTesoreria) Then
                objVMOrdenesTesoreria.NuevoWppSubTrasladoFondos()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al crear nuevo traslado fondos", Me.Name,
                                   "NuevoTraslado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub EditarTraslado(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenesTesoreria) Then
                objVMOrdenesTesoreria.EditarWppTrasladoFondos()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al editar traslado fondos", Me.Name,
                                   "EditarTraslado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BorrarTraslado(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenesTesoreria) Then
                objVMOrdenesTesoreria.BorrarTrasladoFondos()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al editar traslado fondos", Me.Name,
                                   "EditarTraslado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            objVMOrdenesTesoreria.OrdenanteSeleccionadoOYDPLUS = Nothing
            Me.objVMOrdenesTesoreria.ClientePresente = Nothing
            Me.objVMOrdenesTesoreria.ListaOrdenantesOYDPLUS = Nothing
            objVMOrdenesTesoreria.TesoreriaOrdenesPlusCE_Selected.strNombre = Nothing
            objVMOrdenesTesoreria.TesoreriaOrdenesPlusCE_Selected.strNroDocumento = Nothing
            objVMOrdenesTesoreria.TesoreriaOrdenesPlusCE_Selected.strIDComitente = Nothing
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name,
                                   "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCiudad_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If Me.objVMOrdenesTesoreria.HabilitarCamposDireccion Then
                ctlBuscadorCiudad.AbrirBuscador()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al abrir el buscador.", Me.Name,
                                   "txtCiudad_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCiudad_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.objVMOrdenesTesoreria.strCiudadInstrucciones = String.Empty
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name,
                                   "btnLimpiarCiudad_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtDescripcionBanco_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If Me.objVMOrdenesTesoreria.HabilitarCamposConsignarInstrucciones Then
                ctlBuscadorBanco.AbrirBuscador()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al abrir el buscador.", Me.Name,
                                   "txtDescripcionBanco_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarBanco_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.objVMOrdenesTesoreria.strCodigoBancoInstrucciones = String.Empty
            Me.objVMOrdenesTesoreria.strDescripcionBancoInstrucciones = String.Empty
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name,
                                   "btnLimpiarBanco_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.objVMOrdenesTesoreria.NuevaOrdenPrograma()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Me.objVMOrdenesTesoreria.Duplicar()
    End Sub

    Private Sub btnRefrescarPantalla_Click(sender As Object, e As RoutedEventArgs)
        Me.objVMOrdenesTesoreria.RecargarPantalla()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Me.objVMOrdenesTesoreria.AbrirAccionesOrdenGiro()
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
    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        Dim strURLArchivo As String = CType(sender, Button).Tag
        Program.VisorArchivosWeb_DescargarURL(strURLArchivo)
    End Sub
End Class
