
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web

''' <summary>
''' Métodos creados para la comunicación con el OPENRIA (PLAT_PreordenesDomainServices.vb y dbPLAT_Preordenes.edmx)
''' Pantalla País (Maestros)
''' </summary>
''' <remarks>Natalia Andrea Otalvaro (Alcuadrado S.A.) - 21 de Febrero 2019</remarks>
''' <history>
'''
'''</history>
Public Class VisorPreordenesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As PLAT_PreordenesDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public viewPreOrdenes As VisorPreordenesView = Nothing

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                PrepararNuevaBusqueda()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarPreOrdenes("RECARGARTODO")
                Await ConsultarCruzadas()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _ListaCompras As List(Of CPX_tblPreOrdenes_Visor)
    Public Property ListaCompras() As List(Of CPX_tblPreOrdenes_Visor)
        Get
            Return _ListaCompras
        End Get
        Set(ByVal value As List(Of CPX_tblPreOrdenes_Visor))
            _ListaComprasPaginada = Nothing
            _ListaCompras = value
            MyBase.CambioItem("ListaCompras")
            MyBase.CambioItem("ListaComprasPaginada")
        End Set
    End Property

    Private _ListaComprasPaginada As PagedCollectionView = Nothing
    Public ReadOnly Property ListaComprasPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCompras) Then
                If IsNothing(_ListaComprasPaginada) Then
                    Dim view = New PagedCollectionView(_ListaCompras)
                    _ListaComprasPaginada = view
                    Return view
                Else
                    Return (_ListaComprasPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CompraSeleccionado As CPX_tblPreOrdenes_Visor
    Public Property CompraSeleccionado() As CPX_tblPreOrdenes_Visor
        Get
            Return _CompraSeleccionado
        End Get
        Set(ByVal value As CPX_tblPreOrdenes_Visor)
            _CompraSeleccionado = value
            MyBase.CambioItem("CompraSeleccionado")
        End Set
    End Property

    Private _ListaVentas As List(Of CPX_tblPreOrdenes_Visor)
    Public Property ListaVentas() As List(Of CPX_tblPreOrdenes_Visor)
        Get
            Return _ListaVentas
        End Get
        Set(ByVal value As List(Of CPX_tblPreOrdenes_Visor))
            _ListaVentasPaginada = Nothing
            _ListaVentas = value
            MyBase.CambioItem("ListaVentas")
            MyBase.CambioItem("ListaVentasPaginada")
        End Set
    End Property

    Private _ListaVentasPaginada As PagedCollectionView = Nothing
    Public ReadOnly Property ListaVentasPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaVentas) Then
                If IsNothing(_ListaVentasPaginada) Then
                    Dim view = New PagedCollectionView(_ListaVentas)
                    _ListaVentasPaginada = view
                    Return view
                Else
                    Return (_ListaVentasPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _VentaSeleccionado As CPX_tblPreOrdenes_Visor
    Public Property VentaSeleccionado() As CPX_tblPreOrdenes_Visor
        Get
            Return _VentaSeleccionado
        End Get
        Set(ByVal value As CPX_tblPreOrdenes_Visor)
            _VentaSeleccionado = value
            MyBase.CambioItem("VentaSeleccionado")
        End Set
    End Property

    Private _ListaCruzadas As List(Of CPX_tblPreOrdenes_VisorCruces)
    Public Property ListaCruzadas() As List(Of CPX_tblPreOrdenes_VisorCruces)
        Get
            Return _ListaCruzadas
        End Get
        Set(ByVal value As List(Of CPX_tblPreOrdenes_VisorCruces))
            _ListaCruzadasPaginada = Nothing
            _ListaCruzadas = value
            MyBase.CambioItem("ListaCruzadas")
            MyBase.CambioItem("ListaCruzadasPaginada")
        End Set
    End Property

    Private _ListaCruzadasPaginada As PagedCollectionView = Nothing
    Public ReadOnly Property ListaCruzadasPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCruzadas) Then
                If IsNothing(_ListaCruzadasPaginada) Then
                    Dim view = New PagedCollectionView(_ListaCruzadas)
                    _ListaCruzadasPaginada = view
                    Return view
                Else
                    Return (_ListaCruzadasPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CruzadaSeleccionado As CPX_tblPreOrdenes_VisorCruces
    Public Property CruzadaSeleccionado() As CPX_tblPreOrdenes_VisorCruces
        Get
            Return _CruzadaSeleccionado
        End Get
        Set(ByVal value As CPX_tblPreOrdenes_VisorCruces)
            _CruzadaSeleccionado = value
            MyBase.CambioItem("CruzadaSeleccionado")
        End Set
    End Property

    Private _FechaInicial As DateTime = Now
    Public Property FechaInicial() As DateTime
        Get
            Return _FechaInicial
        End Get
        Set(ByVal value As DateTime)
            _FechaInicial = value
            MyBase.CambioItem("FechaInicial")
        End Set
    End Property

    Private _FechaFinal As DateTime = Now
    Public Property FechaFinal() As DateTime
        Get
            Return _FechaFinal
        End Get
        Set(ByVal value As DateTime)
            _FechaFinal = value
            MyBase.CambioItem("FechaFinal")
        End Set
    End Property

    Private _intIDPreOrden As Nullable(Of Integer)
    Public Property intIDPreOrden() As Nullable(Of Integer)
        Get
            Return _intIDPreOrden
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDPreOrden = value
            MyBase.CambioItem("intIDPreOrden")
        End Set
    End Property

    Private _logSoloUsuario As Boolean = True
    Public Property logSoloUsuario() As Boolean
        Get
            Return _logSoloUsuario
        End Get
        Set(ByVal value As Boolean)
            _logSoloUsuario = value
            MyBase.CambioItem("logSoloUsuario")
        End Set
    End Property

    Private _ColorCompra As Brush
    Public Property ColorCompra() As Brush
        Get
            Return _ColorCompra
        End Get
        Set(ByVal value As Brush)
            _ColorCompra = value
            MyBase.CambioItem("ColorCompra")
        End Set
    End Property

    Private _dblValorTotalCompra As Double
    Public Property dblValorTotalCompra() As Double
        Get
            Return _dblValorTotalCompra
        End Get
        Set(ByVal value As Double)
            _dblValorTotalCompra = value
            MyBase.CambioItem("dblValorTotalCompra")
        End Set
    End Property

    Private _ColorVenta As Brush
    Public Property ColorVenta() As Brush
        Get
            Return _ColorVenta
        End Get
        Set(ByVal value As Brush)
            _ColorVenta = value
            MyBase.CambioItem("ColorVenta")
        End Set
    End Property

    Private _dblValorTotalVenta As String
    Public Property dblValorTotalVenta() As String
        Get
            Return _dblValorTotalVenta
        End Get
        Set(ByVal value As String)
            _dblValorTotalVenta = value
            MyBase.CambioItem("dblValorTotalVenta")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Sub PrepararNuevaBusqueda()
        Try
            FechaInicial = Now
            FechaFinal = Now
            intIDPreOrden = Nothing
            logSoloUsuario = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub GenerarCrucesPreordenes()
        Try
            If ValidarCruces() Then
                A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("VISORPREORDENES_CONFIRMARCRUCE"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmacionCruce, False, String.Empty, False)
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "GenerarCrucesPreordenes", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Async Sub ConsultarPreOrdenesCruzadas()
        Try
            Await ConsultarCruzadas()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarPreOrdenesCruzadas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub RecargarTodo()
        Try
            Await ConsultarPreOrdenes("RECARGARTODO")
            Await ConsultarCruzadas()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "RecargarTodo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub RecargarCompras()
        Try
            Await ConsultarPreOrdenes("RECARGARCOMPRAS")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "RecargarCompra", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub RecargarVentas()
        Try
            Await ConsultarPreOrdenes("RECARGARVENTAS")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "RecargarVentas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarCompra(ByVal pintIDRegistro As Integer)
        Try
            Dim logSoloPermitirSeleccionarUnRegistro As Boolean = False

            If IsNothing(ListaVentas) Then
                logSoloPermitirSeleccionarUnRegistro = True
            Else
                If ListaCompras.Where(Function(i) i.logSeleccionado).Count > 1 Then
                    If ListaVentas.Where(Function(i) i.logSeleccionado).Count = 0 Then
                        logSoloPermitirSeleccionarUnRegistro = True
                    ElseIf ListaVentas.Where(Function(i) i.logSeleccionado).Count > 1 Then
                        logSoloPermitirSeleccionarUnRegistro = True
                    End If
                End If
            End If

            If logSoloPermitirSeleccionarUnRegistro Then
                RecargarSeleccionadoComprasVentas(ListaCompras, pintIDRegistro)
            End If

            RecargarTotales()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "SeleccionarCompra", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarVenta(ByVal pintIDRegistro As Integer)
        Try
            Dim logSoloPermitirSeleccionarUnRegistro As Boolean = False

            If IsNothing(ListaCompras) Then
                logSoloPermitirSeleccionarUnRegistro = True
            Else
                If ListaVentas.Where(Function(i) i.logSeleccionado).Count > 1 Then
                    If ListaCompras.Where(Function(i) i.logSeleccionado).Count = 0 Then
                        logSoloPermitirSeleccionarUnRegistro = True
                    ElseIf ListaCompras.Where(Function(i) i.logSeleccionado).Count > 1 Then
                        logSoloPermitirSeleccionarUnRegistro = True
                    End If
                End If
            End If

            If logSoloPermitirSeleccionarUnRegistro Then
                RecargarSeleccionadoComprasVentas(ListaVentas, pintIDRegistro)
            End If

            RecargarTotales()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "SeleccionarVenta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub RecalcularTotales()
        Try
            RecargarTotales()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "RecalcularTotales", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub VerDetalleRegistro(ByVal pintID As Integer)
        Try
            Dim objVistaModalDetalle As New ModalFormaPreordenesView(pintID, "PreOrden")
            Program.Modal_OwnerMainWindowsPrincipal(objVistaModalDetalle)
            objVistaModalDetalle.ShowDialog()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "VerDetalleRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub VerificarCreacionOrden(ByVal pintID As Integer)
        Try
            IsBusy = True

            Dim strTituloModalCompra As String = String.Empty
            Dim strTituloModalVenta As String = String.Empty
            Dim strTituloEtiqueta As String = String.Empty
            Dim strMensajeRegistro As String = String.Empty
            Dim objListaComboSeleccion As New List(Of ProductoCombos)
            Dim objRegistroSeleccionadoPreOrdenes As CPX_tblPreOrdenes_VisorCruces = Nothing
            Dim strEtiquetaAceptar As String = String.Empty
            Dim strEtiquetaCancelar As String = String.Empty

            If DiccionarioEtiquetasPantalla.ContainsKey("SELECTORORDEN_TITULOCOMPRA") Then
                strTituloModalCompra = DiccionarioEtiquetasPantalla("SELECTORORDEN_TITULOCOMPRA").Titulo
            End If
            If DiccionarioEtiquetasPantalla.ContainsKey("SELECTORORDEN_TITULOVENTA") Then
                strTituloModalVenta = DiccionarioEtiquetasPantalla("SELECTORORDEN_TITULOVENTA").Titulo
            End If
            If DiccionarioEtiquetasPantalla.ContainsKey("SELECTORORDEN_ETIQUETA") Then
                strTituloEtiqueta = DiccionarioEtiquetasPantalla("SELECTORORDEN_ETIQUETA").Titulo
            End If
            If DiccionarioEtiquetasPantalla.ContainsKey("SELECTORORDEN_MENSAJE") Then
                strMensajeRegistro = DiccionarioEtiquetasPantalla("SELECTORORDEN_MENSAJE").Titulo
            End If
            If DiccionarioEtiquetasPantalla.ContainsKey("SELECTORORDEN_ACEPTAR") Then
                strEtiquetaAceptar = DiccionarioEtiquetasPantalla("SELECTORORDEN_ACEPTAR").Titulo
            End If
            If DiccionarioEtiquetasPantalla.ContainsKey("SELECTORORDEN_CANCELAR") Then
                strEtiquetaCancelar = DiccionarioEtiquetasPantalla("SELECTORORDEN_CANCELAR").Titulo
            End If
            If DiccionarioCombosPantalla.ContainsKey("VISORPREORDENES_TIPOORDEN") Then
                objListaComboSeleccion = DiccionarioCombosPantalla("VISORPREORDENES_TIPOORDEN").ToList
            End If

            If Not IsNothing(_ListaCruzadas) Then
                If _ListaCruzadas.Where(Function(i) i.intID = pintID).Count > 0 Then
                    objRegistroSeleccionadoPreOrdenes = _ListaCruzadas.Where(Function(i) i.intID = pintID).First
                End If
            End If

            If Not IsNothing(objRegistroSeleccionadoPreOrdenes) Then
				Dim objRegistroPreOrdenCompra As CPX_tblPreOrdenes_Cruzadas = Await ConsultarDetallePreOrdenCruzada(objRegistroSeleccionadoPreOrdenes.intID, "C")
				Dim objRegistroPreOrdenVenta As CPX_tblPreOrdenes_Cruzadas = Await ConsultarDetallePreOrdenCruzada(objRegistroSeleccionadoPreOrdenes.intID, "V")
                If Not IsNothing(objRegistroPreOrdenCompra) And Not IsNothing(objRegistroPreOrdenVenta) Then
                    Dim logCrearCompra As Boolean = Await VerificarAsociarOrden_PreOrdenCruzada(objRegistroPreOrdenCompra.intID)
					Dim logContinuarCreacionVenta As Boolean = True

					If logCrearCompra Then
						'Dim objNuevaOrdenCompra As New ModalSelectorTipoOrdenView(strTituloModalCompra, strTituloEtiqueta, strMensajeRegistro, objListaComboSeleccion, strEtiquetaAceptar, strEtiquetaCancelar)
						'Program.Modal_OwnerMainWindowsPrincipal(objNuevaOrdenCompra)
						'If objNuevaOrdenCompra.ShowDialog() Then
						'	Select Case objNuevaOrdenCompra.TipoOrdenSeleccionado.ToUpper
						'		Case "OYD"
						'			Await Orden_CrearRegistrosOYD("OYD", objRegistroPreOrdenCompra)
						'		Case "ON"
						'			Await Orden_CrearRegistrosOtrosNegocios("ON", objRegistroPreOrdenCompra)
						'		Case "OYDPLUS"
						'			Await Orden_CrearRegistrosOYDPLUS("OYDPLUS", objRegistroPreOrdenCompra)
						'	End Select
						'End If
						logContinuarCreacionVenta = Await Orden_CrearRegistrosOYDPLUS("OYDPLUS", objRegistroPreOrdenCompra)
					End If

					Dim logCrearVenta As Boolean = Await VerificarAsociarOrden_PreOrdenCruzada(objRegistroPreOrdenVenta.intID)

					If logContinuarCreacionVenta Then
						If logCrearVenta Then
							'Dim objNuevaOrdenVenta As New ModalSelectorTipoOrdenView(strTituloModalVenta, strTituloEtiqueta, strMensajeRegistro, objListaComboSeleccion, strEtiquetaAceptar, strEtiquetaCancelar)
							'Program.Modal_OwnerMainWindowsPrincipal(objNuevaOrdenVenta)
							'If objNuevaOrdenVenta.ShowDialog() Then
							'    Select Case objNuevaOrdenVenta.TipoOrdenSeleccionado.ToUpper
							'        Case "OYD"
							'            Await Orden_CrearRegistrosOYD("OYD", objRegistroPreOrdenVenta)
							'        Case "ON"
							'            Await Orden_CrearRegistrosOtrosNegocios("ON", objRegistroPreOrdenVenta)
							'        Case "OYDPLUS"
							'            Await Orden_CrearRegistrosOYDPLUS("OYDPLUS", objRegistroPreOrdenVenta)
							'    End Select
							'End If
							Await Orden_CrearRegistrosOYDPLUS("OYDPLUS", objRegistroPreOrdenVenta)
						End If
					End If

					If logCrearCompra = False And logCrearVenta = False Then
						A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("VISORPREORDENES_VALIDACION_ORDENNOPERMITIDA"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
					End If

					Await ConsultarCruzadas()
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "VerificarCreacionOrden", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Sub RecargarComprasVentas(ByRef pobjListaRecargar As List(Of CPX_tblPreOrdenes_Visor), ByVal pobjListaRegistros As List(Of CPX_tblPreOrdenes_Visor), Optional ByVal plogRespetarRegistrosMarcados As Boolean = True)
        If IsNothing(pobjListaRegistros) Then
            pobjListaRecargar = Nothing
        Else
            Dim objListaNueva As List(Of CPX_tblPreOrdenes_Visor) = Nothing

            Dim objListaRegistrosMarcadas As List(Of CPX_tblPreOrdenes_Visor) = Nothing

            If plogRespetarRegistrosMarcados Then
                If Not IsNothing(pobjListaRecargar) Then
                    objListaRegistrosMarcadas = pobjListaRecargar.Where(Function(i) i.logSeleccionado).ToList
                End If
            End If

            objListaNueva = pobjListaRegistros

            If Not IsNothing(objListaRegistrosMarcadas) Then
                For Each li In objListaRegistrosMarcadas
                    If objListaNueva.Where(Function(i) i.intID = li.intID).Count > 0 Then
                        objListaNueva.Where(Function(i) i.intID = li.intID).First.logSeleccionado = True
                    End If
                Next
            End If

            pobjListaRecargar = objListaNueva
        End If
    End Sub

    Private Sub RecargarTotales()
        Dim logCompraUnValor As Boolean = False
        Dim logVentaUnValor As Boolean = False

        dblValorTotalCompra = 0
        dblValorTotalVenta = 0
        ColorCompra = Brushes.Gray
        ColorVenta = Brushes.Gray

        If Not IsNothing(ListaCompras) Then
            dblValorTotalCompra = ListaCompras.Where(Function(i) i.logSeleccionado).Sum(Function(a) a.dblValorPendiente)
            If ListaCompras.Where(Function(i) i.logSeleccionado).Count = 1 Then
                logCompraUnValor = True
            End If
        End If
        If Not IsNothing(ListaVentas) Then
            dblValorTotalVenta = ListaVentas.Where(Function(i) i.logSeleccionado).Sum(Function(a) a.dblValorPendiente)
            If ListaVentas.Where(Function(i) i.logSeleccionado).Count = 1 Then
                logVentaUnValor = True
            End If
        End If

        If logCompraUnValor And logVentaUnValor = False Then
            If dblValorTotalVenta > dblValorTotalCompra Or dblValorTotalVenta = 0 Then
                ColorVenta = Brushes.Red
            End If
        ElseIf logVentaUnValor And logCompraUnValor = False Then
            If dblValorTotalCompra > dblValorTotalVenta Or dblValorTotalCompra = 0 Then
                ColorCompra = Brushes.Red
            End If
        End If
    End Sub

    Private Function ValidarCruces() As Boolean
        Try
            If IsNothing(ListaCompras) Or IsNothing(ListaVentas) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("VISORPREORDENES_VALIDACION_SINREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If ListaCompras.Where(Function(i) i.logSeleccionado).Count = 0 Or ListaVentas.Where(Function(i) i.logSeleccionado).Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("VISORPREORDENES_VALIDACION_SINSELECCION"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If ListaCompras.Where(Function(i) i.logSeleccionado).Count > 1 And ListaVentas.Where(Function(i) i.logSeleccionado).Count > 1 Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("VISORPREORDENES_VALIDACION_SOLOUNO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            Dim dblValorACruzar As Double = 0
            Dim dblValorOtros As Double = 0

            If ListaCompras.Where(Function(i) i.logSeleccionado).Count = 1 And ListaVentas.Where(Function(i) i.logSeleccionado).Count > 1 Then
                dblValorACruzar = ListaCompras.Where(Function(i) i.logSeleccionado).First.dblValorPendiente
                dblValorOtros = ListaVentas.Sum(Function(i) i.dblValorPendiente)

                If dblValorOtros > dblValorACruzar Then
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("VISORPREORDENES_VALIDACION_SUMAVENTAS"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    Return False
                End If
            ElseIf ListaCompras.Where(Function(i) i.logSeleccionado).Count > 1 Then
                dblValorACruzar = ListaVentas.Where(Function(i) i.logSeleccionado).First.dblValorPendiente
                dblValorOtros = ListaCompras.Sum(Function(i) i.dblValorPendiente)

                If dblValorOtros > dblValorACruzar Then
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("VISORPREORDENES_VALIDACION_SUMACOMPRAS"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ValidarCruces", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Private Function RetornarPreOrdenCruzar() As CPX_tblPreOrdenes_Visor
        Dim objRegistroCruzar As CPX_tblPreOrdenes_Visor = Nothing

        If dblValorTotalCompra >= dblValorTotalVenta Then
            objRegistroCruzar = ListaCompras.Where(Function(i) i.logSeleccionado).First
        Else
            objRegistroCruzar = ListaVentas.Where(Function(i) i.logSeleccionado).First
        End If

        Return objRegistroCruzar
    End Function

    Private Function RetornarPreOrdenesACruzar() As String
        Dim strRegistrosCruzar As String = String.Empty

        If dblValorTotalCompra >= dblValorTotalVenta Then
            For Each li In ListaVentas.Where(Function(i) i.logSeleccionado)
                strRegistrosCruzar = String.Format("{0}{1}*{2}", IIf(String.IsNullOrEmpty(strRegistrosCruzar), "", strRegistrosCruzar & "|"),
                                                   li.intIDPreOrdenVisualizar,
                                                   li.dblValorPendiente)
            Next
        Else
            For Each li In ListaCompras.Where(Function(i) i.logSeleccionado)
                strRegistrosCruzar = String.Format("{0}{1}*{2}", IIf(String.IsNullOrEmpty(strRegistrosCruzar), "", strRegistrosCruzar & "|"),
                                                   li.intIDPreOrdenVisualizar,
                                                   li.dblValorPendiente)
            Next
        End If

        Return strRegistrosCruzar
    End Function

    Private Sub RecargarSeleccionadoComprasVentas(ByRef pobjListaRefrescar As List(Of CPX_tblPreOrdenes_Visor), ByVal pintIDRegistro As Integer)
        Dim objListaRecargar As New List(Of CPX_tblPreOrdenes_Visor)
        For Each li In pobjListaRefrescar
            objListaRecargar.Add(li)
        Next

        For Each li In objListaRecargar
            If li.intID = pintIDRegistro Then
                li.logSeleccionado = True
            Else
                li.logSeleccionado = False
            End If
        Next

        pobjListaRefrescar = objListaRecargar
    End Sub

    Private Async Function Orden_CrearRegistrosOYD(ByVal pstrOrigenOrden As String, ByVal pobjRegistroPreOrden As CPX_tblPreOrdenes_Cruzadas) As Task(Of Boolean)
        Try
            If pobjRegistroPreOrden.strTipoInversion = "A" Then
                Dim objDatos_ModalAccionesCompra As New A2OYDOrdenes.Datos_ModalOrdenesAcciones
                objDatos_ModalAccionesCompra.TipoOperacion = pobjRegistroPreOrden.strTipoPreOrden
                objDatos_ModalAccionesCompra.IDComitente = pobjRegistroPreOrden.intIDComitente
                objDatos_ModalAccionesCompra.Especie = pobjRegistroPreOrden.strInstrumento
                objDatos_ModalAccionesCompra.Precio = pobjRegistroPreOrden.dblPrecio
                objDatos_ModalAccionesCompra.Nominal = pobjRegistroPreOrden.dblValor

                Dim objModalAccionesCompra As New A2OYDOrdenes.ModalOrdenesAcciones(objDatos_ModalAccionesCompra)
                Program.Modal_OwnerMainWindowsPrincipal(objModalAccionesCompra)
                If objModalAccionesCompra.ShowDialog() Then
                    Await AsociarOrden_PreOrdenCruzada(pobjRegistroPreOrden.intID, pstrOrigenOrden, objModalAccionesCompra.intNroOrden, objModalAccionesCompra.strTipoOrden, objModalAccionesCompra.strTipoOperacion, String.Empty)
                End If
            Else
                Dim objDatos_ModalRentaFijaCompra As New A2OYDOrdenes.Datos_ModalOrdenesRentaFija
                objDatos_ModalRentaFijaCompra.TipoOperacion = pobjRegistroPreOrden.strTipoPreOrden
                objDatos_ModalRentaFijaCompra.IDComitente = pobjRegistroPreOrden.intIDComitente
                objDatos_ModalRentaFijaCompra.Especie = pobjRegistroPreOrden.strInstrumento
                objDatos_ModalRentaFijaCompra.Precio = pobjRegistroPreOrden.dblPrecio
                objDatos_ModalRentaFijaCompra.Nominal = pobjRegistroPreOrden.dblValor

                Dim objModalRentaFijaCompra As New A2OYDOrdenes.ModalOrdenesRentaFija(objDatos_ModalRentaFijaCompra)
                Program.Modal_OwnerMainWindowsPrincipal(objModalRentaFijaCompra)
                If objModalRentaFijaCompra.ShowDialog() Then
                    Await AsociarOrden_PreOrdenCruzada(pobjRegistroPreOrden.intID, pstrOrigenOrden, objModalRentaFijaCompra.intNroOrden, objModalRentaFijaCompra.strTipoOrden, objModalRentaFijaCompra.strTipoOperacion, String.Empty)
                End If
            End If

            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Orden_CrearRegistrosOYD", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Private Async Function Orden_CrearRegistrosOtrosNegocios(ByVal pstrOrigenOrden As String, ByVal pobjRegistroPreOrden As CPX_tblPreOrdenes_Cruzadas) As Task(Of Boolean)
        Try
            Dim objDatos_ModalOtrosNegocios As New A2CFOperaciones.Datos_ModalOperacionesOtrosNegocios
            objDatos_ModalOtrosNegocios.Clase = pobjRegistroPreOrden.strTipoInversion
            objDatos_ModalOtrosNegocios.TipoOperacion = pobjRegistroPreOrden.strTipoPreOrden
            objDatos_ModalOtrosNegocios.IDComitente = pobjRegistroPreOrden.intIDComitente
            objDatos_ModalOtrosNegocios.Especie = pobjRegistroPreOrden.strInstrumento
            objDatos_ModalOtrosNegocios.Precio = pobjRegistroPreOrden.dblPrecio
            objDatos_ModalOtrosNegocios.Nominal = pobjRegistroPreOrden.dblValor

            Dim objModalOtrosNegocios As New A2CFOperaciones.ModalOperacionesOtrosNegocios(objDatos_ModalOtrosNegocios)
            Program.Modal_OwnerMainWindowsPrincipal(objModalOtrosNegocios)
            If objModalOtrosNegocios.ShowDialog() Then
                Await AsociarOrden_PreOrdenCruzada(pobjRegistroPreOrden.intID, pstrOrigenOrden, objModalOtrosNegocios.intNroOrden, objModalOtrosNegocios.strTipoOrden, objModalOtrosNegocios.strTipoOperacion, objModalOtrosNegocios.strTipoOrigen)
            End If

            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Orden_CrearRegistrosOtrosNegocios", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

	Private Async Function Orden_CrearRegistrosOYDPLUS(ByVal pstrOrigenOrden As String, ByVal pobjRegistroPreOrden As CPX_tblPreOrdenes_Cruzadas) As Task(Of Boolean)
		Try
			Dim objDatos_ModalOrdenes As New A2OYDPLUSOrdenesBolsa.Datos_ModalOrdenesPLUSView
			objDatos_ModalOrdenes.Receptor = pobjRegistroPreOrden.strIDReceptor
			objDatos_ModalOrdenes.TipoNegocio = pobjRegistroPreOrden.strTipoInversion
			objDatos_ModalOrdenes.TipoProducto = pobjRegistroPreOrden.strTipoProducto
			objDatos_ModalOrdenes.TipoOperacion = pobjRegistroPreOrden.strTipoPreOrden
			objDatos_ModalOrdenes.IDComitente = pobjRegistroPreOrden.intIDComitente
			objDatos_ModalOrdenes.Especie = pobjRegistroPreOrden.strInstrumento
			objDatos_ModalOrdenes.Precio = pobjRegistroPreOrden.dblPrecio
			objDatos_ModalOrdenes.Nominal = pobjRegistroPreOrden.dblValor

			Dim objModalOYDPLUS As New A2OYDPLUSOrdenesBolsa.ModalOrdenesPLUSView(objDatos_ModalOrdenes)
			Program.Modal_OwnerMainWindowsPrincipal(objModalOYDPLUS)
			If objModalOYDPLUS.ShowDialog() Then
				Await AsociarOrden_PreOrdenCruzada(pobjRegistroPreOrden.intID, pstrOrigenOrden, objModalOYDPLUS.intNroOrden, objModalOYDPLUS.strTipoOrden, objModalOYDPLUS.strTipoOperacion, String.Empty)
				Return True
			Else
				Return False
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Orden_CrearRegistrosOtrosNegocios", Application.Current.ToString(), Program.Maquina, ex)
			Return False
		End Try
	End Function

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

	Private Async Function ConsultarPreOrdenes(ByVal pstrOpcion As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblPreOrdenes_Visor)) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.VisorPreOrdenes_ConsultarAsync(Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaCompras As List(Of CPX_tblPreOrdenes_Visor)
                    Dim objListaVentas As List(Of CPX_tblPreOrdenes_Visor)

                    objListaCompras = objRespuesta.Value.Where(Function(i) i.strTipoPreOrden = "C").ToList
                    objListaVentas = objRespuesta.Value.Where(Function(i) i.strTipoPreOrden = "V").ToList

                    If pstrOpcion = "RECARGAAUTOMATICA" Then
                        RecargarComprasVentas(ListaCompras, objListaCompras)
                        RecargarComprasVentas(ListaVentas, objListaVentas)
                    ElseIf pstrOpcion = "RECARGARTODO" Then
                        RecargarComprasVentas(ListaCompras, objListaCompras, False)
                        RecargarComprasVentas(ListaVentas, objListaVentas, False)
                    ElseIf pstrOpcion = "RECARGARCOMPRAS" Then
                        RecargarComprasVentas(ListaCompras, objListaCompras, False)
                    ElseIf pstrOpcion = "RECARGARVENTAS" Then
                        RecargarComprasVentas(ListaVentas, objListaVentas, False)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarPreOrdenes", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Private Async Function ConsultarCruzadas() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblPreOrdenes_VisorCruces)) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.VisorPreOrdenes_ConsultarCruzadasAsync(FechaInicial, FechaFinal, intIDPreOrden, logSoloUsuario, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    ListaCruzadas = objRespuesta.Value
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarCruzadas", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Private Async Sub ConfirmacionCruce(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                IsBusy = True
                Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Nothing
                Dim objRegistroSeleccionado As CPX_tblPreOrdenes_Visor = RetornarPreOrdenCruzar()
                Dim strRegistrosSeleccionadosCruzar As String = RetornarPreOrdenesACruzar()

                ErrorForma = String.Empty
                objRespuesta = Await mdcProxy.VisorPreOrdenes_GenerarAsync(objRegistroSeleccionado.intIDPreOrdenVisualizar, objRegistroSeleccionado.dblValorPendiente, strRegistrosSeleccionadosCruzar, Program.Usuario, Program.HashConexion, Program.Maquina)

                If Not IsNothing(objRespuesta) Then
                    If Not IsNothing(objRespuesta.Value) Then
                        If objRespuesta.Value.Count > 0 Then
                            IsBusy = False
                            A2Utilidades.Mensajes.mostrarMensaje(objRespuesta.Value.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        Else
                            Await ConsultarPreOrdenes("RECARGARTODO")
                            Await ConsultarCruzadas()
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                        IsBusy = False
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ConsultarDetallePreOrdenCruzada(ByVal pintID As Integer, ByVal pstrTipoPreOrden As String) As Task(Of CPX_tblPreOrdenes_Cruzadas)

        Dim objResultado As CPX_tblPreOrdenes_Cruzadas = Nothing

        Try
            Dim objRespuesta As InvokeResult(Of CPX_tblPreOrdenes_Cruzadas) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.PreOrdenes_ConsultarCruzadaIDAsync(pintID, pstrTipoPreOrden, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    objResultado = objRespuesta.Value
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarOrdenCruzadaCruzadas", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objResultado)
    End Function

    Private Async Function VerificarAsociarOrden_PreOrdenCruzada(ByVal pintID As Integer) As Task(Of Boolean)

        Dim objResultado As Boolean = Nothing

        Try
            Dim objRespuesta As InvokeResult(Of Boolean) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.VisorPreOrdenes_VerificarAsociarOrdenAsync(pintID, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    objResultado = objRespuesta.Value
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarOrdenCruzadaCruzadas", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objResultado)
    End Function

    Private Async Function AsociarOrden_PreOrdenCruzada(ByVal pintID As Integer, ByVal pstrOrigenOrden As String, ByVal pintNroOrdenRegistro As Integer, ByVal pstrClaseRegistro As String, ByVal pstrTipoOperacionRegistro As String, ByVal pstrTipoOrigenRegistro As String) As Task(Of Boolean)

        Dim objResultado As Boolean = Nothing

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.VisorPreOrdenes_AsociarOrdenAsync(pintID, pstrOrigenOrden, pintNroOrdenRegistro, pstrClaseRegistro, pstrTipoOperacionRegistro, pstrTipoOrigenRegistro, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    If objRespuesta.Value.Count > 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje(objRespuesta.Value.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        objResultado = False
                    Else
                        objResultado = True
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarOrdenCruzadaCruzadas", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objResultado)
    End Function

#End Region

#Region "Notificaciones"

    Private Const TIPOMENSAJE_NOTIFICACIONPREORDEN = "VISORPREORDENES_NOTIFICACION"
    Private Const TIPOMENSAJE_NOTIFICACIONPREORDEN_CRUCE = "VISORPREORDENES_CRUCE_NOTIFICACION"

    Dim NroOrdenEditar As Integer = 0

    Public Overrides Sub LlegoNotificacion(pobjInfoNotificacion As A2.Notificaciones.Cliente.clsNotificacion)
        Try
            Dim objNotificacion As List(Of clsNotificacionPreordenes)

            If Not String.IsNullOrEmpty(pobjInfoNotificacion.strTipoMensaje) Then

                If pobjInfoNotificacion.strTipoMensaje.ToUpper = TIPOMENSAJE_NOTIFICACIONPREORDEN Then

                    If Not IsNothing(pobjInfoNotificacion.strInfoMensaje) Then

                        objNotificacion = New List(Of clsNotificacionPreordenes)

                        objNotificacion = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of clsNotificacionPreordenes))(pobjInfoNotificacion.strInfoMensaje)

                        If Not IsNothing(objNotificacion) Then

                            If objNotificacion.Count > 0 Then

                                Dim strCodigoMensajeMostrar As String = "VISORPREORDENES_MODPREORDEN"
                                Dim logRecargarRegistros As Boolean = True
                                Dim strMensajeUsuario As String = String.Empty

                                If pobjInfoNotificacion.strUsuario <> Program.Usuario And pobjInfoNotificacion.strMaquina = Program.Maquina Then
                                    If objNotificacion.Where(Function(i) i.Estado = "CRUCE").Count > 0 Then
                                        strCodigoMensajeMostrar = "VISORPREORDENES_CRUZADA"

                                        For Each li In objNotificacion
                                            If Notificacion_VerificarDisponibilidadPreOrden(li.ID) = False Then
                                                strMensajeUsuario = DiccionarioMensajesPantalla(strCodigoMensajeMostrar).Replace("[PREORDEN]", li.ID)
                                                A2Utilidades.Mensajes.mostrarMensaje(strMensajeUsuario, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        strCodigoMensajeMostrar = "VISORPREORDENES_MODPREORDEN"
                                        If Notificacion_VerificarDisponibilidadPreOrden(objNotificacion.First.ID) = False Then
                                            strMensajeUsuario = DiccionarioMensajesPantalla(strCodigoMensajeMostrar).Replace("[PREORDEN]", objNotificacion.First.ID)
                                            A2Utilidades.Mensajes.mostrarMensaje(strMensajeUsuario, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                        End If
                                    End If
                                End If

                                If logRecargarRegistros Then
                                    RecargarTodo()
                                End If
                            End If
                        End If

                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function Notificacion_VerificarDisponibilidadPreOrden(ByVal pintIDPreOrdenNotificacion As Integer) As Boolean
        Try
            Dim logValidarVentas As Boolean = True
            Dim logRegistroValido As Boolean = True

            If Not IsNothing(ListaCompras) Then
                If ListaCompras.Where(Function(i) i.logSeleccionado).Count > 0 Then
                    If ListaCompras.Where(Function(i) i.logSeleccionado And i.intID = pintIDPreOrdenNotificacion).Count > 0 Then
                        logValidarVentas = False
                        logRegistroValido = False
                    End If
                End If
            End If

            If logValidarVentas = False Then
                If Not IsNothing(ListaVentas) Then
                    If ListaVentas.Where(Function(i) i.logSeleccionado).Count > 0 Then
                        If ListaVentas.Where(Function(i) i.logSeleccionado And i.intID = pintIDPreOrdenNotificacion).Count > 0 Then
                            logRegistroValido = False
                        End If
                    End If
                End If
            End If

            Return logRegistroValido
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

#End Region

End Class
