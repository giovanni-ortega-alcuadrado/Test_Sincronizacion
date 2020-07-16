Imports Telerik.Windows.Controls
'Desarrollado por Edgar Muñoz
'Fecha Septiembre de 2012
'VIew model Utilizado para manejar las ordenes del seteador

Imports System.ComponentModel
Imports System.Linq
Imports System.Xml.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Threading
Imports System.Windows.Interop
Imports System.Runtime.InteropServices

#If HAY_NOTIFICACIONES = 1 Then
Imports A2Consola.Interfaces
#End If


Public Class OrdenSeteadorViewModel
    Inherits A2ControlMenu.A2ViewModel
#Region "Inicialización"


    Public Event ActualizaVisor(pstrRuta As String)

    Public Sub New()
        MyBase.New()

        Try

#If HAY_NOTIFICACIONES = 1 Then
            ListaTopicos = New List(Of String)

            ListaTopicos.Add(STR_TOPICO_NOTIFICACION_SETEADOR)    ' pendiente por registrar tópicos por cuestiones de seguridad
            ListaTopicos.Add(STR_TOPICO_NOTIFICACION_ORDENES)
#End If

            'Try
            '    objEmisor = New LocalMessageReceiver(GSTR_NOMBRE_CANAL_ORDENES, ReceiverNameScope.Global, LocalMessageReceiver.AnyDomain)
            '    objEmisor.DisableSenderTrustCheck = True
            '    AddHandler objEmisor.MessageReceived, AddressOf messageReceiver_MessageReceived
            'Catch ex As Exception
            '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó generar el canal (""" & GSTR_NOMBRE_CANAL_ORDENES & """).", _
            '                         Me.ToString(), "OrdenSeteadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
            'End Try

            'If Not objEmisor Is Nothing Then
            '    Try
            '        objEmisor.Listen()
            '    Catch objExMsg As ListenFailedException
            '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Existe otra instancia de la aplicación que ha registrado el canal de comunicación (""" & GSTR_NOMBRE_CANAL_ORDENES & """).", _
            '                             Me.ToString(), "OrdenSeteadorViewModel.New", Application.Current.ToString(), Program.Maquina, objExMsg)

            '        'Try
            '        '    objEmisor.Dispose()
            '        '    objEmisor = New LocalMessageReceiver(GSTR_NOMBRE_CANAL_ORDENES, ReceiverNameScope.Global, LocalMessageReceiver.AnyDomain)
            '        '    objEmisor.DisableSenderTrustCheck = True
            '        '    AddHandler objEmisor.MessageReceived, AddressOf messageReceiver_MessageReceived
            '        '    Try
            '        '        objEmisor.Listen()
            '        '    Catch objExMsgIn As ListenFailedException
            '        '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Existe otra instancia de la aplicación ejecutando que ha registrado este canal de comunicación (""" & GSTR_NOMBRE_CANAL_ORDENES & """).", _
            '        '                             Me.ToString(), "OrdenSeteadorViewModel.New", Application.Current.ToString(), Program.Maquina, objExMsgIn)
            '        '    End Try
            '        'Catch ex As Exception

            '        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó generar el canal (""" & GSTR_NOMBRE_CANAL_ORDENES & """).", _
            '        '                         Me.ToString(), "OrdenSeteadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
            '        'End Try
            '    Catch ex As Exception
            '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cargar el canal (""" & GSTR_NOMBRE_CANAL_ORDENES & """).", _
            '                             Me.ToString(), "OrdenSeteadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
            '    End Try
            'End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext()
                dcProxy1 = New OYDPLUSOrdenesBolsaDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de orden que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                IsBusySeteador = True
                cargarVisorPorVista(1)
                If MotivosRechazo Is Nothing Then CargarMotivosDeRechazo()
            End If

            '_notifyQueue = New Queue(Of NotificationWindow)()
            ReiniciaTimer()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "OrdenSeteadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Propiedades Ordenes Seteador"

#Region "constantes"
    Private Const MSTR_ACCION_RECHAZAR As String = "rechazar"
    Private Const STR_TOPICO_NOTIFICACION_SETEADOR As String = "SETEADOR"
    Private Const STR_TOPICO_NOTIFICACION_ORDENES As String = "ORDENES"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_DEVUELTA As String = "EL SETEADOR DEVOLVIO LA ORDEN"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_MARCAR_LANZADA As String = "EL SETEADOR MARCO UNA ORDEN COMO LANZADA"
    Public Const STR_MENSAJE_CONSOLA_ORDEN_ENVIADA_SAE As String = "EL SETEADOR ENVIO ORDEN AL SISTEMA SAE"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_RECIBIDA_SAE As String = "EL SISTEMA SAE HA RECIBIDO UNA ORDEN"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_ENVIADA_BVC_SIN_RESPUESTA As String = "SE HA ENVIADO UNA ORDEN A LA BVC, SE ESEPRA RESPUESTA"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_RECIBIDA_BVC As String = "LA BVC HA RECIBIDO UNA ORDEN"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_DEVUELTA_BVC As String = "LA BVC HA DEVUELTO UNA ORDEN"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_ACEPTADA_BVC As String = "LA BVC HA ACEPTADO UNA ORDEN"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_CALZADA As String = "SE CALZARON ORDENES CON OPERACIONES"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_COMPLEMENTADA_ENVIADA As String = "SE ENVIO UNA ORDEN COMPLEMENTADA A LA BVC"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_COMPLEMENTADA_RECIBIDA_BVC As String = "LA BVC HA RECIBIDO UNA ORDEN COMPLEMENTADA"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_COMPLEMENTADA_DEVUELTA As String = "SE HA DEVUELTO UNA ORDEN QUE YA ESTABA COMPLEMENTADA"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_ELIMINADA As String = "SE HA ELIMINADO UNA ORDEN DESDE EL SISTEMA DEL SETEADOR"
    Private Const STR_MENSAJE_CONSOLA_ORDEN_ACTUALMENTE_EN_VISOR As String = "SE HA SELECCIONADO UNA ORDEN PARA MOSTRAR EN EL VISOR"

    Private Const GSTR_NOMBRE_CANAL_ORDENES As String = "A2.OYD.OrdenesSeteador.Canal"
    Private Const GSTR_NOMBRE_CANAL_VISOR As String = "A2.OYD.VisorSeteador.Canal"

#End Region

#Region "Variables"

    Private dcProxy As OYDPLUSOrdenesBolsaDomainContext
    Private dcProxy1 As OYDPLUSOrdenesBolsaDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Private _observacionesRechazo As String
    Private _myDispatcherTimerSeteador As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer
    Private _listaLiquidaciones As New List(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)
    Private _wd As Window
    Private _wdLiq As Window
    Private _cwd As Window
    Private _cambiaVista As Boolean = False
    Private _alqControl As RechazosSeteadorView

    Private logCerrarVisorDespuesRecargo As Boolean = False

    Public pobjViewVisorSeteadorView As VisorSeteadorView
#End Region

#End Region

#Region "Propiedades genéricas"

    Public ReadOnly Property Titulo() As String
        Get
            Return "Ordenes seteador"
        End Get
    End Property

    Public ReadOnly Property HayNotificaciones As Visibility
        Get
            Dim MuestraControl As Visibility = Visibility.Visible

#If HAY_NOTIFICACIONES = 1 Then
            MuestraControl = Visibility.Collapsed
#End If
            Return MuestraControl
        End Get
    End Property

#End Region

#Region "Propiedades para el tipo de orden"

    ''' <summary>
    ''' Id de registro actualmente seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Private _idOrdenSeleccionada As Integer
    Public Property idOrdenSeleccionada() As Integer
        Get
            Return _idOrdenSeleccionada
        End Get
        Set(ByVal value As Integer)
            _idOrdenSeleccionada = value
            'If _ordenSeteadorSelected Is Nothing Then
            '    SeleccionarOrden(value)
            'ElseIf value <> _ordenSeteadorSelected.lngID Then
            '    SeleccionarOrden(value)
            'End If
            MyBase.CambioItem("idOrdenSeleccionada")
        End Set
    End Property

    Private _isBusy As Boolean
    Public Property IsBusySeteador() As Boolean
        Get
            Return _isBusy
        End Get
        Set(ByVal value As Boolean)
            _isBusy = value
            MyBase.CambioItem("IsBusySeteador")
        End Set
    End Property


    ''' <summary>
    ''' Listado de ordenes del seteador devuelto por el servicio RIA
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaOrdenes As List(Of OyDPLUSOrdenesBolsa.OrdenSeteador)
    Public Property ListaOrdenes() As List(Of OyDPLUSOrdenesBolsa.OrdenSeteador)
        Get
            Return _ListaOrdenes
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.OrdenSeteador))
            _ListaOrdenes = value
            MyBase.CambioItem("ListaOrdenes")
            MyBase.CambioItem("ListaOrdenesPaged")
        End Set
    End Property

    ''' <summary>
    ''' Registro actualmente seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Private _ordenSeteadorSelected As OyDPLUSOrdenesBolsa.OrdenSeteador
    Public Property OrdenSeteadorSelected() As OyDPLUSOrdenesBolsa.OrdenSeteador
        Get
            Return _ordenSeteadorSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesBolsa.OrdenSeteador)
            _ordenSeteadorSelected = value

            If Not IsNothing(_ordenSeteadorSelected) Then
                'porRecargaAutomatica = False
                If _ordenSeteadorSelected.lngID <> _idOrdenSeleccionada Then idOrdenSeleccionada = _ordenSeteadorSelected.lngID
                CargarCommandos(_ordenSeteadorSelected)
                VisibilidadNumeroReferencia = PuedeAsignarreferencia()
                CargarRutaVisor()
                If OrdenEnvisor Then CargarVisor()
                ' eomc -- 2014/02/21 -- Verificar resultado de pruebas
            Else
                If OrdenEnvisor And logCerrarVisorDespuesRecargo Then CerrarVisor()
                ' eomc -- 2014/02/21 -- Verificar resultado de pruebas
            End If

            MyBase.CambioItem("OrdenSeteadorSelected")
            MyBase.CambioItem("Commands")
        End Set
    End Property

    ''' <summary>
    ''' Listado de descripciones para definición de los filtros de las ordenes
    ''' </summary>
    ''' <remarks></remarks>
    Private _vistas As ObservableCollection(Of String)
    Public Property Vistas() As ObservableCollection(Of String)
        Get
            CargarVistas()
            Return _vistas
        End Get
        Set(ByVal value As ObservableCollection(Of String))
            _vistas = value
        End Set
    End Property

    ''' <summary>
    ''' Listado de ordenes en modo paginado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ListaOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOrdenes) Then
                Dim view = New PagedCollectionView(_ListaOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _mensajes As List(Of OyDPLUSOrdenesBolsa.tblMensajes)
    ''' <summary>
    ''' Listado con los datos de los cambios en una orden, para el visor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Mensajes() As List(Of OyDPLUSOrdenesBolsa.tblMensajes)
        Get
            Return _mensajes
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.tblMensajes))
            _mensajes = value
        End Set
    End Property


    Private _motivosRechazo As List(Of OyDPLUSOrdenesBolsa.tblMensajes)
    ''' <summary>
    ''' Listado de motivos de rechazo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MotivosRechazo() As List(Of OyDPLUSOrdenesBolsa.tblMensajes)
        Get
            Return _motivosRechazo
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.tblMensajes))
            _motivosRechazo = value
        End Set
    End Property


    Private _mstrEstadoSAE As String = String.Empty
    ''' <summary>
    ''' Estado de la orden cuando ha sido enviada por SAE (Bus de integración)
    ''' </summary>
    Public ReadOnly Property EstadoSAE As String
        Get
            Return (_mstrEstadoSAE)
        End Get
    End Property

    Private _mstrNroOrdenSAE As String = String.Empty
    ''' <summary>
    ''' Número de la orden cuando ha sido enviada por SAE (Bus de integración)
    ''' </summary>
    Public ReadOnly Property NroOrdenSAE As String
        Get
            Return (_mstrNroOrdenSAE)
        End Get
    End Property

    Private _commands As IEnumerable(Of MenuItemCommand)
    ''' <summary>
    ''' Listado de items para menú contextual
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Commands As IEnumerable(Of MenuItemCommand)
        Set(value As IEnumerable(Of MenuItemCommand))
            _commands = value
        End Set
        Get
            Return _commands
        End Get
    End Property

    Private _vistaSeleccionada As String = "Pendientes"
    ''' <summary>
    ''' Filtro actual seleccionado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VistaSeleccionada() As String
        Get
            Return _vistaSeleccionada
        End Get
        Set(ByVal value As String)
            If value <> _vistaSeleccionada Then
                _vistaSeleccionada = value
                enVista2 = CType(IIf(value = "Procesadas", Visibility.Visible, Visibility.Collapsed), Visibility)
                _cambiaVista = True
                porRecargaAutomatica = True
                cargarVisorPorVista(_vistas.IndexOf(_vistaSeleccionada) + 1)
                MyBase.CambioItem("VistaSeleccionada")
            End If
        End Set
    End Property

    Private _enVista2 As Visibility = Visibility.Collapsed
    ''' <summary>
    ''' Propiedad de visibilidad para ocultar o mostrar columnas en el grid de ordenes
    ''' también se usas para mostrar u ocultar el botón para calces automáticos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property enVista2() As Visibility
        Get
            Return _enVista2
        End Get
        Set(ByVal value As Visibility)
            _enVista2 = value
            MyBase.CambioItem("enVista2")
        End Set
    End Property

    Private _nroReferencia As Integer
    ''' <summary>
    ''' Número de referencia para acciones
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NumeroReferencia() As Integer
        Get
            Return _nroReferencia
        End Get
        Set(ByVal value As Integer)
            _nroReferencia = value
            MyBase.CambioItem("NumeroReferencia")
        End Set
    End Property

    Private _visibilidadNroRef As Visibility
    ''' <summary>
    ''' Indica si existe número de referencia para acciones y expone propiedad visible para mostrar o colapsar objetos en la vista
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VisibilidadNumeroReferencia() As Visibility
        Get
            Return _visibilidadNroRef
        End Get
        Set(ByVal value As Visibility)
            _visibilidadNroRef = value
            MyBase.CambioItem("VisibilidadNumeroReferencia")
        End Set
    End Property

    Private _xmlLiquidaciones As String
    ''' <summary>
    ''' String con definición de XML para ingreso de liquidaciones probables
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property XMLLiquidaciones() As String
        Get
            Return _xmlLiquidaciones
        End Get
        Set(ByVal value As String)
            _xmlLiquidaciones = value
        End Set
    End Property

    Private _cargaAutomaticaActiva As Boolean
    ''' <summary>
    ''' Activa o desactiva la recarga de ordenes mediante temporizador
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property cargaAutomaticaActiva() As Boolean
        Get
            Return _cargaAutomaticaActiva
        End Get
        Set(ByVal value As Boolean)
            _cargaAutomaticaActiva = value
        End Set
    End Property

    Private _rutaVisor As String
    ''' <summary>
    ''' Propiedad que alamcena la url de la página del visor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RutaVisor() As String
        Get
            Return _rutaVisor
        End Get
        Set(ByVal value As String)
            '_rutaVisor = HttpUtility.UrlEncode(value)
            _rutaVisor = value
            MyBase.CambioItem("RutaVisor")
        End Set
    End Property

    Private _ListaNotificaciones As ObservableCollection(Of String) = New ObservableCollection(Of String)()
    ''' <summary>
    ''' Listado que permite acumular mensajes, para luego hacerlos visibles desde ventana emergente
    ''' evita problemas de concurrencia al instanciar ventana emergente
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListaNotificaciones() As ObservableCollection(Of String)
        Get
            Return _ListaNotificaciones
        End Get
        Set(ByVal value As ObservableCollection(Of String))
            _ListaNotificaciones = value
            CambioItem("ListaNotificaciones")
        End Set
    End Property

    Private _visorVisible As Boolean
    ''' <summary>
    ''' Indica si el visor está actualmente visible o no
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VisorVisible() As Boolean
        Get
            Return _visorVisible
        End Get
        Set(ByVal value As Boolean)
            _visorVisible = value
            CambioItem("VisorVisible")
        End Set
    End Property

    Private _ordenEnvisor As Boolean = False
    ''' <summary>
    ''' indica si la orden seleccionada está cargada en el visor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OrdenEnvisor() As Boolean
        Get
            Return _ordenEnvisor
        End Get
        Set(ByVal value As Boolean)
            _ordenEnvisor = value
        End Set
    End Property


    Private _lstLiquidaciones As New ObservableCollection(Of OyDPLUSOrdenesBolsa.Liquidacion)
    ''' <summary>
    ''' listado de liquidaciones disponibles
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LiquidacionesDisponibles() As ObservableCollection(Of OyDPLUSOrdenesBolsa.Liquidacion)
        Get
            Return _lstLiquidaciones
        End Get
        Set(ByVal value As ObservableCollection(Of OyDPLUSOrdenesBolsa.Liquidacion))
            _lstLiquidaciones = value
            CambioItem("LiquidacionesDisponibles")
        End Set
    End Property

    Private _subTotal As Double
    ''' <summary>
    ''' Suma de valores de las liquidaciones seleccionadas
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property subTotal() As Double
        Get
            Return _subTotal
        End Get
        Set(ByVal value As Double)
            _subTotal = value
            CambioItem("subTotal")
        End Set
    End Property

    Private _puedeLanzarSAE As Boolean
    ''' <summary>
    ''' Habilia/deshabilita botón Lanzar SAE
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property puedeLanzarSAE() As Boolean
        Get
            Return _puedeLanzarSAE
        End Get
        Set(ByVal value As Boolean)
            _puedeLanzarSAE = value
            CambioItem("puedeLanzarSAE")
        End Set
    End Property

    Private _puedeMarcarLanzada As Boolean
    ''' <summary>
    ''' Habilia/deshabilita botón Marcar como lanzada (Lanzar)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property puedeMarcarLanzada() As Boolean
        Get
            Return _puedeMarcarLanzada
        End Get
        Set(ByVal value As Boolean)
            _puedeMarcarLanzada = value
            CambioItem("puedeMarcarLanzada")
        End Set
    End Property

    Private _puedeRechazar As Boolean
    ''' <summary>
    ''' Habilia/deshabilita botón Rechazar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property puedeRechazar() As Boolean
        Get
            Return _puedeRechazar
        End Get
        Set(ByVal value As Boolean)
            _puedeRechazar = value
            CambioItem("puedeRechazar")
        End Set
    End Property


    Private _porRecargaAutoamtica As Boolean
    ''' <summary>
    ''' Para que no envíe mensajes al visor cuando es por recarga automática
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property porRecargaAutomatica() As Boolean
        Get
            Return _porRecargaAutoamtica
        End Get
        Set(ByVal value As Boolean)
            _porRecargaAutoamtica = value
        End Set
    End Property


    ReadOnly Property Mensaje() As String
        Get
            Dim strMesanje As String = String.Empty
            strMesanje &= "RutaVisor*" & RutaVisor & "|"
            strMesanje &= "puedeLanzarSAE*" & puedeLanzarSAE.ToString & "|"
            strMesanje &= "puedeMarcarLanzada*" & puedeMarcarLanzada.ToString & "|"
            strMesanje &= "puedeRechazar*" & puedeRechazar.ToString & "|"
            strMesanje &= "VisibilidadNumeroReferencia*" & VisibilidadNumeroReferencia.ToString & "|"
            strMesanje &= "NumeroReferencia*" & NumeroReferencia.ToString
            Return strMesanje
        End Get
    End Property

    Private _accionEjecutada As Accion
    Public Property AccionEjecutada() As Accion
        Get
            Return _accionEjecutada
        End Get
        Set(ByVal value As Accion)
            _accionEjecutada = value
        End Set
    End Property



#End Region

#Region "Resultados asincrónicos seteador"


    ''' <summary>
    ''' Método que se ejecuta cuando la consulta asíncrona de carga de ordenes finaliza
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.OrdenSeteador))
        Try
            If Not lo.HasError Then
                Finalizar(lo)
                'If Not _rutaVisor Is Nothing Then
                '    If OrdenEnvisor Then actualizarURL()
                'End If
                IsBusySeteador = False
                ReiniciaTimer()
                'pararTemporizador()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las ordenes del seteador", Me.ToString(), "TerminoTraerOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusySeteador = False
            End If
        Catch ex As Exception
            IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las ordenes del seteador", Me.ToString(), "TerminoTraerOrdenes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se ejecuta cuando finaliza el método de borrado de ordenes del seteador
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoBorrarOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.OrdenSeteador))
        Try
            ActualizarBloqueoOrden()

            If Not lo.HasError Then

                Dim strOrden = lo.EntityQuery.Parameters("plngIDOrden").ToString

                idOrdenSeleccionada = Nothing
                Dim objOrden As New OyDPLUSOrdenesBolsa.OrdenSeteador

                If dcProxy.OrdenSeteadors.ToList.Count > 0 Then
                    objOrden = dcProxy.OrdenSeteadors.ToList.First
                    If objOrden.intIDOrdenes = -1 Then
                        A2Utilidades.Mensajes.mostrarMensaje(objOrden.strConfiguracionMenu, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        Finalizar(lo)
                        Notificaciones("Orden eliminada", "Se eliminó la orden de la vista del seteador.")
                        AccionEjecutada = Accion.Rechazada
                        EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.Rechazada.ToString(), _observacionesRechazo)
                        A2Utilidades.Mensajes.mostrarMensaje(String.Format(AccionEjecutada.nombreAccion.ToString, strOrden), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al rechazar una orden", Me.ToString(), "TerminoBorrarOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusySeteador = False
            End If

            cargarVisorPorVista(_vistas.IndexOf(_vistaSeleccionada) + 1)

        Catch ex As Exception
            IsBusySeteador = False

            If ex.Message.Contains("ErrorPersonalizado|") Then
                Dim strMensaje As String = ex.Message.Split("|")(1)
                A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al rechazar una orden", Me.ToString(), "TerminoBorrarOrden", Application.Current.ToString(), Program.Maquina, ex)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Método que se ejecuta cuando finaliza el método actualización de ordenes del seteador
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub ActualizoEstadoOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.OrdenSeteador))
        Try
            If Not lo.HasError Then

                Dim strOrden = OrdenSeteadorSelected.lngID.ToString()

                Dim objOrden As New OyDPLUSOrdenesBolsa.OrdenSeteador

                If dcProxy.OrdenSeteadors.ToList.Count > 0 Then
                    objOrden = dcProxy.OrdenSeteadors.ToList.First
                    If objOrden.intIDOrdenes = -1 Then
                        If OrdenEnvisor = False Then
                            ActualizarBloqueoOrden()
                        End If
                        A2Utilidades.Mensajes.mostrarMensaje(objOrden.strConfiguracionMenu, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        ActualizarBloqueoOrden()
                        ' eomc -- 2014/02/21 -- Verificar resultado de pruebas
                        Finalizar(lo)
                        ' eomc -- 2014/02/21 -- Verificar resultado de pruebas
                        Notificaciones("Orden modificada", "Se ha modificado una orden por parte del seteador.")
                        If Not AccionEjecutada Is Accion.Ninguna Then A2Utilidades.Mensajes.mostrarMensaje(String.Format(AccionEjecutada.nombreAccion.ToString, strOrden), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.Rechazada.ToString(), _observacionesRechazo)
                    End If
                Else
                    ActualizarBloqueoOrden()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al modificar la orden", Me.ToString(), "ActualizoEstadoOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusySeteador = False
            End If

            cargarVisorPorVista(_vistas.IndexOf(_vistaSeleccionada) + 1)
        Catch ex As Exception
            IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al modificar la orden", Me.ToString(), "ActualizoEstadoOrden", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se llama desde los proceosos de terminación de carga de orden
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub Finalizar(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.OrdenSeteador))

        Try
            If Not lo.HasError Then

                If dcProxy.OrdenSeteadors.Count > 0 Then
                    logCerrarVisorDespuesRecargo = False
                Else
                    logCerrarVisorDespuesRecargo = True
                End If

                If lo.UserState Is "TERMINOGUARDAR" Then
                    Dim nroOrden As Integer = OrdenSeteadorSelected.intIDOrdenes
                    Dim objListaOrden As New List(Of OyDPLUSOrdenesBolsa.OrdenSeteador)

                    objListaOrden.Add(dcProxy.OrdenSeteadors.ToList.Where(Function(i) i.intIDOrdenes = nroOrden).FirstOrDefault)

                    For Each li In dcProxy.OrdenSeteadors.ToList.Where(Function(i) i.intIDOrdenes <> nroOrden).ToList
                        objListaOrden.Add(li)
                    Next

                    ListaOrdenes = dcProxy.OrdenSeteadors.ToList
                Else
                    ListaOrdenes = dcProxy.OrdenSeteadors.ToList
                    IsBusySeteador = False
                End If

                If Not IsNothing(_ListaOrdenes) Then
                    If _ListaOrdenes.Count > 0 Then
                        OrdenSeteadorSelected = _ListaOrdenes.FirstOrDefault()
                    End If
                Else
                    CerrarVisor()
                End If
                If Not IsNothing(_ordenSeteadorSelected) Then
                    CargarCommandos(_ordenSeteadorSelected)
                    MyBase.CambioItem("Commands")
                End If

                If dcProxy.OrdenSeteadors.Count = 0 Then
                    If Not _wd Is Nothing Then
                        _wd.Visibility = Visibility.Collapsed
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la lista de Ordenes del seteador", Me.ToString(), "TerminoTraerOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusySeteador = False
            End If

        Catch ex As Exception
            IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes", Me.ToString(), "TerminoTraerOrden", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Metodo para el proceso de las liquidaciones disponibles consultadas
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoTraerLiquidaciones(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.Liquidacion))
        Try

            If Not lo.HasError Then

                _lstLiquidaciones.Clear()

                For Each lq In dcProxy.Liquidacions
                    _listaLiquidaciones.Add(New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo _
                            With {.ID = lq.intNroFolio.ToString(), .Descripcion = "Folio: " + lq.intNroFolio.ToString() + ", Nemotécnico : " + lq.strNemotecnico, .Categoria = "LIQUIDACIONES"})

                    LiquidacionesDisponibles.Add(New OyDPLUSOrdenesBolsa.Liquidacion _
                            With {.dtmFechaCumplimiento = lq.dtmFechaCumplimiento,
                                 .dtmFechaEmision = lq.dtmFechaEmision,
                                 .dtmFechaVencimiento = lq.dtmFechaVencimiento,
                                 .intDiasDeCumplimiento = lq.intDiasDeCumplimiento,
                                 .intNroFolio = lq.intNroFolio,
                                 .lngIdOrden = lq.lngIdOrden,
                                 .numCantidad = lq.numCantidad,
                                 .numMonto = lq.numMonto,
                                 .numPrecio = lq.numPrecio,
                                 .numTasa = lq.numTasa,
                                 .strCodigoISIN = lq.strCodigoISIN,
                                 .strMercado = lq.strMercado,
                                 .strModalidad = lq.strModalidad,
                                 .strNemotecnico = lq.strNemotecnico,
                                 .strRuedaNegocio = lq.strRuedaNegocio,
                                 .strTipoMercado = lq.strTipoMercado,
                                 .strTrader = lq.strTrader
                                })

                Next

                If OrdenSeteadorSelected.strTipoNegocio = Program.TN_Acciones Then
                    If _lstLiquidaciones.Count = 0 Then
                        NumeroReferencia = 0
                    Else
                        Dim ord = OrdenSeteadorSelected
                        Dim objLiquidacion = _lstLiquidaciones.Where(Function(i As OyDPLUSOrdenesBolsa.Liquidacion) i.lngIdOrden = ord.lngID And i.strNemotecnico = ord.strIDEspecie).FirstOrDefault
                        'NumeroReferencia = CInt(_lstLiquidaciones.Where(Function(i As OyDPLUSOrdenesBolsa.Liquidacion) i.lngIdOrden = ord.lngID And i.strNemotecnico = ord.strIDEspecie).FirstOrDefault.intNroFolio())
                        If Not objLiquidacion Is Nothing Then
                            NumeroReferencia = Integer.Parse(objLiquidacion.intNroFolio.ToString)
                        Else
                            NumeroReferencia = 0
                        End If
                    End If
                End If

                'If Not _visorVisible Then
                MostrarLiquidacionesDisponibles()
                IsBusySeteador = False
            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de liquidaciones disponibles", Me.ToString(), "TerminoTraerLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusySeteador = False

            End If

        Catch ex As Exception

            IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención  de liquidaciones disponibles", Me.ToString(), "TerminoTraerLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    ''' <summary>
    ''' Método que actualiza los parametros de la url del visor indicando los campos que cambiaron de valor para una orden
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminaConsultaDatosModificados(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.tblMensajes))
        Try
            If Not lo.HasError Then
                Mensajes = dcProxy.tblMensajes.ToList

                RutaVisor += Mensajes(0).Valor
                enviarMensajeApp()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de datos modificados para el visor", Me.ToString(), "TerminaConsultaDatosModificados", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusySeteador = False

            End If

        Catch ex As Exception
            IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de datos modificados para el visor", Me.ToString(), "TerminaConsultaDatosModificados", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    ''' <summary>
    ''' Método que se ejecuta cuando termina de enviar la orden al sistema SAE
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoEnrutarOrdenSAE(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.OrdenSAE))

        Dim objOrdenSAE As OyDPLUSOrdenesBolsa.OrdenSAE
        Dim logConsultarSAE As Boolean = False
        Dim blnError As Boolean = False

        Try
            Me._mstrNroOrdenSAE = String.Empty
            If Not lo.HasError Then
                If dcProxy1.OrdenSAEs.ToList.Count > 0 Then
                    objOrdenSAE = dcProxy1.OrdenSAEs.First
                    If objOrdenSAE.TipoMensaje > 1 Then
                        If OrdenEnvisor = False Then
                            ActualizarBloqueoOrden()
                        End If
                        Me._mstrEstadoSAE = String.Empty
                        A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema & " - Error en enrutamiento", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        blnError = True
                    ElseIf objOrdenSAE.TipoMensaje > 0 Then
                        If OrdenEnvisor = False Then
                            ActualizarBloqueoOrden()
                        End If
                        Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE
                        A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema & " - Advertencias en enrutamiento", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        'A2Utilidades.Mensajes.mostrarMensaje("Orden enrutada exitosamente a través de SAE", Program.TituloSistema)
                        Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE
                        AccionEjecutada = Accion.LanzadaSAE
                        ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Lanzada, Program.ES_Estado_Visor_En_BVC_Sin_Respuesta, _vistas.IndexOf(_vistaSeleccionada) + 1, blnError)
                        EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.EnviadaSAE.ToString(), STR_MENSAJE_CONSOLA_ORDEN_ENVIADA_SAE)

                    End If
                Else
                    Me._mstrEstadoSAE = String.Empty
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir respueta del enrutamiento de la orden por SAE", Me.ToString(), "TerminoEnrutarOrdenSAE", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????

                Me._mstrEstadoSAE = String.Empty
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el enrutamiento de la orden por SAE", Me.ToString(), "TerminoEnrutarOrdenSAE", Program.TituloSistema, Program.Maquina, ex)
        Finally
            Me.IsBusySeteador = False
        End Try

    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el proceso de autorización de la orden finaliza
    ''' </summary>
    Private Sub TerminoAutorizarOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.OrdenSAE))

        Dim objOrdenSAE As OyDPLUSOrdenesBolsa.OrdenSAE

        Try

            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se cambió el estado de la orden porque se presentó un problema durante el proceso.", Me.ToString(), "Autorizar_OrdenCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                IsBusySeteador = False
            Else
                objOrdenSAE = dcProxy.OrdenSAEs.First
                If objOrdenSAE.TipoMensaje > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    IsBusySeteador = False
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    BorrarOrdenSeteador(_vistas.IndexOf(_vistaSeleccionada) + 1)
                    cargarVisorPorVista(_vistas.IndexOf(_vistaSeleccionada) + 1)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la aprobación de la orden.", Me.ToString(), "TerminoAutorizarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusySeteador = False
        End Try

    End Sub

    ''' <summary>
    ''' se ejecuta cuando termina la asignación de liquidaciones probables
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub AsignoLiquidacionesProbables(ByVal lo As InvokeOperation(Of String))

        Try
            'IsBusySeteador = True
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al asignar liquidaciones probables",
                Me.ToString(), "AddressOf AsignoLiquidacionesProbables", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            Else
                'If VisorVisible Then
                '    _wd.Close()
                'End If
                cargarVisorPorVista(_vistas.IndexOf(_vistaSeleccionada) + 1)
                A2Utilidades.Mensajes.mostrarMensaje("Se han ingresado liquidaciónes probables correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la carga de liquidaciones probables.", Me.ToString(), "AsignoLiquidacionesProbables", Application.Current.ToString(), Program.Maquina, ex)
            IsBusySeteador = False
        End Try

    End Sub

    ''' <summary>
    ''' Método asigna listado de motivos de rechazo
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminaConsultaMotivosRechazo(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.tblMensajes))
        Try

            If Not lo.HasError Then
                MotivosRechazo = dcProxy.tblMensajes.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de motivos de rechazo", Me.ToString(), "TerminaConsultaMotivosRechazo", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusySeteador = False
            End If

        Catch ex As Exception
            IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de motivos de rechazo", Me.ToString(), "TerminaConsultaMotivosRechazo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    ''' <summary>
    ''' maneja respuesta de validación de estado de orden y decide si llama al visor o informa el bloqueo
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoValidarOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.EstadoFlujoOrden))
        Dim objEstadoOrden As OyDPLUSOrdenesBolsa.EstadoFlujoOrden
        Dim logConsultarSAE As Boolean = False
        Dim blnError As Boolean = False

        Try

            Me._mstrNroOrdenSAE = String.Empty
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    objEstadoOrden = dcProxy.EstadoFlujoOrdens.First
                    RutaVisor += objEstadoOrden.strCamposModificados
                    If objEstadoOrden.logExitoso Then

                        If lo.UserState = "MOSTRARVISOR" Then
                            If OrdenEnvisor = False Then
                                Dim objModalPrincipal As Window = Program.Modal_ObtenerWindowPrincipal()

                                If Not IsNothing(objModalPrincipal) Then
                                    Dim modalwin As VisorSeteadorView = New VisorSeteadorView(Me)
                                    modalwin.Owner = objModalPrincipal
                                    modalwin.Show()
                                    Me.IsBusySeteador = False
                                Else
                                    Me.IsBusySeteador = False
                                    A2Utilidades.Mensajes.mostrarMensaje("No se pudo obtener el Windows Principal.", Program.TituloSistema & " - Orden no disponible", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                            Else
                                enviarMensajeApp()
                                Me.IsBusySeteador = False
                            End If
                        ElseIf lo.UserState = "ENRUTARSAE" Then
                            LlamarEnrutarSAE()
                        ElseIf lo.UserState = "MARCARLANZADA" Then
                            LlamarMarcarLanzada()
                        ElseIf lo.UserState = "RECHAZARORDEN" Then
                            CausasRechazo()
                        ElseIf lo.UserState = "CARGARLIQUIDACIONES" Then
                            ObtenerLiquidacionesDisponibles()
                        End If
                    Else
                        Me.IsBusySeteador = False
                        A2Utilidades.Mensajes.mostrarMensaje(objEstadoOrden.strMensajeValidacion, Program.TituloSistema & " - Orden no disponible", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    Me.IsBusySeteador = False
                    A2Utilidades.Mensajes.mostrarMensaje("La validación de disponibilidad de la orden no retornó datos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                Me.IsBusySeteador = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la disponibilidad de la orden", Me.ToString(), "TerminoValidarOrden", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If

        Catch ex As Exception
            Me.IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la disponibilidad de la orden", Me.ToString(), "TerminoValidarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Informa de los resultados del calce automatico
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoCalceAutomatico(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.Liquidacion))
        Dim objLiquidacion As OyDPLUSOrdenesBolsa.Liquidacion
        Dim strMensaje As String = String.Empty

        Try

            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then

                    strMensaje = "Se han calzado las siguientes liquidaciones:" + vbNewLine + vbNewLine

                    For Each objLiquidacion In lo.Entities.ToList
                        strMensaje += String.Format("   -> Orden número: {0}, folio: {1}, especie {2}, monto:  {3}", objLiquidacion.lngIdOrden.ToString, objLiquidacion.intNroFolio.ToString, objLiquidacion.strNemotecnico, objLiquidacion.numMonto.ToString) + vbNewLine
                    Next

                    strMensaje += vbNewLine + "Las ordenes pasan a estado 'Pendientes en bolsa'" + vbNewLine

                Else

                    strMensaje = "No se encontraron ordenes disponibles para calce automático."

                End If

                A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                cargarVisorPorVista(_vistas.IndexOf(_vistaSeleccionada) + 1)

            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la disponibilidad de la orden", Me.ToString(), "TerminoCalceAutomatico", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la disponibilidad de la orden", Me.ToString(), "TerminoCalceAutomatico", Program.TituloSistema, Program.Maquina, ex)
        Finally
            Me.IsBusySeteador = False
        End Try
    End Sub

#End Region

#Region "Metodos Orden Seteador"

    ''' <summary>
    ''' Enrutar por SAE
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Sub enviarPorSAE(ByVal sender As Object, ByVal Optional plogVerificarDisponibilidad As Boolean = False)

        Try
            IsBusySeteador = True
            _cambiaVista = True
            If Me.OrdenSeteadorSelected Is Nothing Then
                Exit Sub
            Else
                Me.IsBusySeteador = True
                If plogVerificarDisponibilidad Then
                    ordenHabilitada("ENRUTARSAE")
                Else
                    LlamarEnrutarSAE()
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enrutar la orden por SAE", Me.ToString(), "enviarPorSAE", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusySeteador = False
        End Try

    End Sub

    Private Sub LlamarEnrutarSAE()
        Try
            dcProxy1.OrdenSAEs.Clear()
            dcProxy1.Load(dcProxy1.enrutar_OrdenSAE_SeteadorQuery(OrdenSeteadorSelected.Clase, OrdenSeteadorSelected.strTipo, OrdenSeteadorSelected.lngID, False, Program.Usuario, Program.HashConexion), AddressOf TerminoEnrutarOrdenSAE, "enrutarSAE")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enrutar la orden por SAE", Me.ToString(), "LlamarEnrutarSAE", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusySeteador = False
        End Try
    End Sub

    ''' <summary>
    ''' Rechaza de orden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Sub rechazarOrden(sender As Object, ByVal Optional plogVerificarDisponibilidad As Boolean = False)
        Try
            IsBusySeteador = True

            If _ordenSeteadorSelected Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la orden que desea rechazar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusySeteador = False
            Else
                If plogVerificarDisponibilidad Then
                    ordenHabilitada("RECHAZARORDEN")
                Else
                    CausasRechazo()
                End If

            End If

        Catch ex As Exception
            IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de rechazo de la orden", Me.ToString(), "RechazarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Listado de causas de rechazo para mostrar cuando se rechaza una orden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CausasRechazo()
        Try

            Dim ListaJustificacion As New List(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)
            _alqControl = New RechazosSeteadorView
            AddHandler _alqControl.mensajeAsignado, AddressOf TerminoRechazarOrden

            ListaJustificacion.Clear()

            Dim objMotivoRechazo = From item In MotivosRechazo
                                   Select New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo With {.ID = item.ID.ToString,
                                                                                    .Descripcion = item.Valor,
                                                                                    .Categoria = item.Titulo}
            ListaJustificacion = objMotivoRechazo.ToList

            _alqControl.MostrarListaObservaciones = True
            _alqControl.ListaCausas = ListaJustificacion
            _alqControl.PermitirGuardarSinItem = False

            _alqControl.PermitirGuardarSinObservacion = False
            _alqControl.MostrarTextoObservaciones = True
            _alqControl.MensajeUsuario = "Causas de rechazo para la orden " + OrdenSeteadorSelected.lngID.ToString
            _alqControl.MensajeRegla = String.Empty
            _alqControl.CodConfirmacion = String.Empty
            _alqControl.CodRegla = String.Empty
            _alqControl.NombreRegla = String.Empty

            _cwd = New Window

            _cwd.Content = _alqControl
            _cwd.SizeToContent = SizeToContent.WidthAndHeight
            _cwd.WindowStartupLocation = WindowStartupLocation.CenterScreen

            AddHandler _cwd.Closed, AddressOf Cierra
            Program.Modal_OwnerMainWindowsPrincipal(_cwd)
            _cwd.ShowDialog()
        Catch ex As Exception
            IsBusySeteador = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al levantar la ventana de rechazo de la orden", Me.ToString(), "CausasRechazo", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Método para carga de motivos de rechazo
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarMotivosDeRechazo()
        Try
            If Not IsNothing(dcProxy.OrdenSeteadors) Then
                dcProxy.OrdenSeteadors.Clear()
            End If
            dcProxy.Load(dcProxy.OrdenesSeteadorObtenerListaTopicoQuery(Program.tpc_motivo_rechazo, Program.Usuario, Program.HashConexion), AddressOf TerminaConsultaMotivosRechazo, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los motivos de rechazo.", Me.ToString(), "CargarMotivosDeRechazo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub cargarLiquidaciones(ByVal Optional plogVerificarDisponibilidad As Boolean = False)
        Try
            If Not IsNothing(OrdenSeteadorSelected) Then
                If plogVerificarDisponibilidad Then
                    ordenHabilitada("CARGARLIQUIDACIONES")
                Else
                    ObtenerLiquidacionesDisponibles()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al levantar la asociación de liquidaciones", Me.ToString(), "cargarLiquidaciones", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra ventana con liquidaciones disponibles
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MostrarLiquidacionesDisponibles()
        Try

            If Application.Current.Resources.Contains("VM") Then Application.Current.Resources.Remove("VM")
            Application.Current.Resources.Add("VM", Me)

            _subTotal = 0

            Dim objMensaje As New LiquidacionesProbables
            Dim cwLiquidaciones As New Window
            cwLiquidaciones.Title = "Oden OyD " + OrdenSeteadorSelected.lngID.ToString()
            'objMensaje.Header = "Oden OyD " + OrdenSeteadorSelected.lngID.ToString()
            'AddHandler objMensaje.Closed, AddressOf TerminoSeleccionLiquidaciones

            AddHandler cwLiquidaciones.Closed, AddressOf TerminoSeleccionLiquidaciones

            If _lstLiquidaciones.Count > 0 Then
                'objMensaje.CenterOnScreen()
                'objMensaje.ShowModal()
                cwLiquidaciones.SizeToContent = SizeToContent.WidthAndHeight
                cwLiquidaciones.WindowStartupLocation = WindowStartupLocation.CenterScreen
                cwLiquidaciones.Content = objMensaje
                Program.Modal_OwnerMainWindowsPrincipal(cwLiquidaciones)
                cwLiquidaciones.ShowDialog()
            Else
                ActualizarBloqueoOrden()
                A2Utilidades.Mensajes.mostrarMensaje("No se encontraron liquidaciones disponibles.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
            IsBusySeteador = False

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las liquidaciones disponibles.", Me.ToString(), "cargarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
            IsBusySeteador = False
        End Try
    End Sub

    ''' <summary>
    ''' consulta de liquidaciones disponibles
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ObtenerLiquidacionesDisponibles()

        Try
            IsBusySeteador = True

            If Not IsNothing(dcProxy.Liquidacions) Then
                dcProxy.Liquidacions.Clear()
            End If
            Dim ord = OrdenSeteadorSelected
            Dim punta As String = If(ord.strTipoNegocio = Program.TN_Renta_Fija, "1", "2")
            dcProxy.Load(dcProxy.OrdenesSeteadorObtenerLiquidacionesDisponiblesQuery(ord.lngID, ord.strIdReceptorToma, ord.strTipoNegocio, ord.strTipo, ord.strIDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las liquidaciones disponibles.", Me.ToString(), "ObtenerLiquidacionesDisponibles", Application.Current.ToString(), Program.Maquina, ex)
            IsBusySeteador = False
        End Try

    End Sub

    ''' <summary>
    ''' proceso cuando se han seleccionado las liquidaciones disponibles, para enviar a guardar en liquidaciones probables
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TerminoSeleccionLiquidaciones(sender As Object, e As EventArgs)

        Try
            'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
            ActualizarBloqueoOrden()

            Dim cwLiqSeleccionadas As Window
            Dim objResultado As LiquidacionesProbables
            cwLiqSeleccionadas = CType(sender, Window)
            objResultado = CType(cwLiqSeleccionadas.Content, LiquidacionesProbables)
            If CBool(objResultado.cwParent.DialogResult) Then
                'Notificaciones("Liquidaciones probables", "Se seleccionaron las liquidaciones " + objResultado.TextoConfirmacion)
                'asignaLiquidacionesProbables(objResultado.TextoConfirmacion)
                asignaLiquidacionesProbables(objResultado.LiquidacionesProbables)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al asignar liquidaciones probables", Me.ToString(), "TerminoSeleccionLiquidaciones", Program.TituloSistema, Program.Maquina, ex)
            IsBusySeteador = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario rechaza la orden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TerminoPreguntarAprobacion(ByVal sender As Object, ByVal e As EventArgs)

        Try

            'Dim objResultado As A2Utilidades.wppMensajePregunta
            'objResultado = CType(sender, A2Utilidades.wppMensajePregunta)

            'Dim objParentresultado As cwMensajePregunta
            'Dim objResultado As A2Utilidades.wppMensajePregunta
            'objParentresultado = CType(sender, cwMensajePregunta)
            'objResultado = CType(objParentresultado.Content, A2Utilidades.wppMensajePregunta)

            'If CBool(objResultado.cwParent.DialogResult) Then
            '    _observacionesRechazo = objResultado.Observaciones + "|" + objResultado.TextoConfirmacion.Replace("|", "*")
            '    'aprobarOrden(False)
            '    BorrarOrdenSeteador(_vistas.IndexOf(_vistaSeleccionada) + 1)
            '    cargarVisorPorVista(_vistas.IndexOf(_vistaSeleccionada) + 1)
            'End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el rechazo de la orden", Me.ToString(), "TerminoPreguntarAprobacion", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se aceptan las razones de rechazo y se rechaza la orden
    ''' </summary>
    ''' <param name="str"></param>
    ''' <remarks></remarks>
    Private Sub TerminoRechazarOrden(str As String)
        Try
            If str.ToUpper <> "CANCEL" Then
                Dim strOrden = OrdenSeteadorSelected.lngID.ToString()
                If _alqControl.TextoConfirmacion.Length > 0 Then
                    _observacionesRechazo = _alqControl.Observaciones + "|" + _alqControl.TextoConfirmacion.Replace("|", "*")
                    BorrarOrdenSeteador(_vistas.IndexOf(_vistaSeleccionada) + 1)
                End If
            Else
                IsBusySeteador = False
            End If
            _cwd.Close()
            _cwd = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el rechazo de la orden", Me.ToString(), "TerminoRechazarOrden", Program.TituloSistema, Program.Maquina, ex)
            IsBusySeteador = False
        End Try
    End Sub

    ''' <summary>
    ''' Aprueba o rechaza una orden, desde el seteador solo se rechaza
    ''' </summary>
    ''' <param name="plogAprobar"></param>
    ''' <remarks></remarks>
    Private Sub aprobarOrden(ByVal plogAprobar As Boolean)

        Try

            IsBusySeteador = True
            dcProxy.OrdenSAEs.Clear()
            dcProxy.Load(dcProxy.AutorizarOrdenQuery(_ordenSeteadorSelected.strTipoNegocio, _ordenSeteadorSelected.strTipo, _ordenSeteadorSelected.lngID, _ordenSeteadorSelected.lngVersion, False, CInt(_ordenSeteadorSelected.CalificacionEspecie), CInt(_ordenSeteadorSelected.PerfilComitente), Program.Usuario, Program.HashConexion), AddressOf TerminoAutorizarOrden, MSTR_ACCION_RECHAZAR)
            MyBase.AprobarRegistro()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el proceso de aprobación de la orden", Me.ToString(), "aprobarOrden", Program.TituloSistema, Program.Maquina, ex)
            IsBusySeteador = False
        End Try

    End Sub

    ''' <summary>
    ''' Muestra datos de orden seleccionada en el visor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Sub MostrarPopupVisor(sender As Object)
        Try
            If Not IsNothing(sender) And TypeOf sender Is Integer Then
                If ListaOrdenes.Where(Function(i) i.lngID = Integer.Parse(sender.ToString())).Count > 0 Then
                    OrdenSeteadorSelected = ListaOrdenes.Where(Function(i) i.lngID = Integer.Parse(sender.ToString())).FirstOrDefault
                End If
            End If

            ordenHabilitada("MOSTRARVISOR")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el visor",
                                 Me.ToString(), "MostrarPopupVisor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarVisor()
        MostrarPopupVisor(Nothing)
    End Sub

    ''' <summary>
    ''' Marca la orden como lanzada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Sub marcarLanzada(sender As Object, ByVal Optional plogVerificarDisponibilidad As Boolean = False)
        Try
            If plogVerificarDisponibilidad Then
                ordenHabilitada("MARCARLANZADA")
            Else
                LlamarMarcarLanzada()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema marcar la orden como lanzada.", Me.ToString(), "marcarLanzada", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LlamarMarcarLanzada()
        Try
            _cambiaVista = True
            AccionEjecutada = Accion.MarcarLanzada
            ActualizarEstadoOrden(_ordenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Recibida, Program.ES_Estado_Visor_Mostrada_En_Visor, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.MarcarComoLanzada.ToString(), STR_MENSAJE_CONSOLA_ORDEN_MARCAR_LANZADA)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema marcar la orden como lanzada.", Me.ToString(), "LlamarMarcarLanzada", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Genera listado de comandos para menú contextual
    ''' </summary>
    ''' <param name="ord"></param>
    ''' <remarks></remarks>
    Private Sub CargarCommandos(ord As OyDPLUSOrdenesBolsa.OrdenSeteador)

        Try

            puedeLanzarSAE = False
            puedeMarcarLanzada = False
            puedeRechazar = False

            Dim commandos As List(Of MenuItemCommand) = New List(Of MenuItemCommand)
            _commands = Nothing

            Dim strComandos = ord.strConfiguracionMenu.Split(CChar("|"))
            Dim cmd As MenuItemCommand
            For Each Str As String In strComandos
                Dim itm = Str.Split(CChar(";"))

                If PuedeGenerarComando(itm(0)) Then

                    cmd = New MenuItemCommand

                    For i As Integer = 0 To itm.Count
                        Select Case i
                            Case 0
                                Select Case itm(0)
                                    Case Program.CMND_Mostrar_En_Visor
                                        cmd.id = 0
                                        cmd.Command = New RelayCommand(AddressOf MostrarPopupVisor)
                                    Case Program.CMND_Lanzar_SAE
                                        puedeLanzarSAE = True
                                        cmd.id = 1
                                        cmd.Command = New RelayCommand(AddressOf enviarPorSAE)
                                    Case Program.CMND_Marcar_Lanzada
                                        puedeMarcarLanzada = True
                                        cmd.id = 2
                                        cmd.Command = New RelayCommand(AddressOf marcarLanzada)
                                    Case Program.CMND_Rechazar
                                        puedeRechazar = True
                                        cmd.id = 3
                                        cmd.Command = New RelayCommand(AddressOf rechazarOrden)
                                End Select
                            Case 1
                                cmd.esSeparador = CBool(Integer.Parse(itm(i)))
                            Case 2
                                cmd.nombre = itm(2)
                            Case 3
                                cmd.Descripcion = itm(3)
                        End Select

                    Next

                    cmd.parametro = ord
                    commandos.Add(cmd)
                    If _vistaSeleccionada = "Procesadas" And itm(0) = Program.CMND_Mostrar_En_Visor Then
                        cmd = New MenuItemCommand() With {.id = 4, .esSeparador = False, .nombre = "Asociar liquidaciones", .Descripcion = "Asociar liquidaciones disponibles a esta orden."}
                        commandos.Add(cmd)
                    End If
                End If

            Next
            _commands = commandos.ToArray

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al asignar comandos para menú contextual.", Me.ToString(), "CargarCommandos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Listado para las opciones de carga de las ordenes del seteador
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarVistas()
        _vistas = New ObservableCollection(Of String)
        _vistas.Add("Pendientes")
        _vistas.Add("Procesadas")
        _vistas.Add("Pendientes en bolsa")
    End Sub

    ''' <summary>
    ''' Se filtran las ordenes dependiendo de la vista (folder) seleccioando
    ''' </summary>
    ''' <param name="pintVista"></param>
    ''' <remarks></remarks>
    Private Sub cargarVisorPorVista(pintVista As Integer)

        Try

            If Not IsNothing(dcProxy.OrdenSeteadors) Then
                dcProxy.OrdenSeteadors.Clear()
            End If
            dcProxy.Load(dcProxy.ObtenerOrdenesSeteadorQuery(Program.Usuario, pintVista, Program.HashConexion), AddressOf TerminoTraerOrdenes, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar las ordenes del seteador.", Me.ToString(), "cargarVisorPorVista", Application.Current.ToString(), Program.Maquina, ex)
            IsBusySeteador = False
        End Try

    End Sub

    ''' <summary>
    ''' Actualiza estado en tabla de ordenes del seteador
    ''' </summary>
    ''' <param name="pstrEstado"></param>
    ''' <param name="pstrEstadoLEO"></param>
    ''' <param name="pstrEstadoVisor"></param>
    ''' <param name="pintVista"></param>
    ''' <remarks></remarks>
    Private Sub ActualizarEstadoOrden(pstrEstado As String, pstrEstadoLEO As String, pstrEstadoVisor As String, pintVista As Integer, blnError As Boolean)

        Try
            If Not IsNothing(OrdenSeteadorSelected) Then

                If Not IsNothing(dcProxy.OrdenSeteadors) Then
                    dcProxy.OrdenSeteadors.Clear()
                End If
                dcProxy.Load(dcProxy.OrdenesSeteadorEstadoActualizarQuery(OrdenSeteadorSelected.intIDOrdenes, Program.Usuario, pstrEstado, pstrEstadoLEO, pstrEstadoVisor, pintVista, blnError, Program.HashConexion), AddressOf ActualizoEstadoOrden, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el estado de la orden del seteador.", Me.ToString(), "ActualizarEstadoOrden", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Elimina orden de la tabla de ordenes del seteador
    ''' </summary>
    ''' <param name="pintVista"></param>
    ''' <remarks></remarks>
    Private Sub BorrarOrdenSeteador(pintVista As Integer)

        Try

            If Not IsNothing(dcProxy.OrdenSeteadors) Then
                dcProxy.OrdenSeteadors.Clear()
            End If
            dcProxy.Load(dcProxy.OrdenesSeteadorBorrarQuery(OrdenSeteadorSelected.intIDOrdenes, OrdenSeteadorSelected.lngID, Program.Usuario, pintVista, _observacionesRechazo.Replace("*", "|"), Program.HashConexion), AddressOf TerminoBorrarOrden, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar orden del seteador.", Me.ToString(), "BorrarOrdenSeteador", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Método que consulta los posibles cambios en una orden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub actualizarURL()
        Try

            If Not dcProxy.tblMensajes Is Nothing Then
                dcProxy.tblMensajes.Clear()
            End If

            dcProxy.Load(dcProxy.OrdenesSeteadorObtenerCambiosVisorQuery(idOrdenSeleccionada, Program.Usuario, Program.HashConexion), AddressOf TerminaConsultaDatosModificados, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos modifcados de la orden.", Me.ToString(), "actualizarURL", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Ingreso de liquidaciones probables
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub asignaLiquidacionesProbables(sender As Object)

        Try
            IsBusySeteador = True
            If Not IsNothing(dcProxy.OrdenSeteadors) Then
                dcProxy.OrdenSeteadors.Clear()
            End If

            If sender Is Nothing Then
                generarXMLLiquidaciones(String.Empty)
            Else
                'generarXMLLiquidaciones(sender.ToString())
                generarXMLLiquidaciones(CType(sender, List(Of OyDPLUSOrdenesBolsa.Liquidacion)))
            End If

            dcProxy.OrdenesSeteadorAsignarLiquidacionesProbables(XMLLiquidaciones, OrdenSeteadorSelected.lngID, Program.Usuario, Date.Today, Program.HashConexion, AddressOf AsignoLiquidacionesProbables, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar liquidaciones probables.", Me.ToString(), "asignaLiquidacionesProbables", Application.Current.ToString(), Program.Maquina, ex)
            IsBusySeteador = False
        End Try

    End Sub

    ''' <summary>
    ''' Comando seleccionado en menú contextual
    ''' </summary>
    ''' <param name="pstrAccion"></param>
    ''' <remarks></remarks>
    Public Sub AccionElegida(pstrAccion As Short)
        Try

            Select Case pstrAccion
                Case CShort(MenuItemCommand.accionesMenu.mostrarVisor)
                    porRecargaAutomatica = False
                    MostrarPopupVisor(Nothing)
                Case CShort(MenuItemCommand.accionesMenu.lanzarSAE)
                    enviarPorSAE(Nothing, True)
                    EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.EnviadaSAE.ToString(), STR_MENSAJE_CONSOLA_ORDEN_ENVIADA_SAE)
                Case CShort(MenuItemCommand.accionesMenu.MarcarComoLanzada)
                    marcarLanzada(Nothing, True)
                Case CShort(MenuItemCommand.accionesMenu.Rechazar)
                    rechazarOrden(Nothing, True)
                Case CShort(MenuItemCommand.accionesMenu.AsociarLiquidaciones)
                    cargarLiquidaciones(True)
            End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el comando seleccionado del menú contextual", Me.ToString(), "AccionElegida", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Comando seleccionado en menú contextual
    ''' </summary>
    ''' <param name="pstrAccion"></param>
    ''' <remarks></remarks>
    Public Sub AccionElegida(pstrAccion As String)
        Try

            Select Case pstrAccion
                Case "LanzarSAE"
                    enviarPorSAE(Nothing)
                    EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.EnviadaSAE.ToString(), STR_MENSAJE_CONSOLA_ORDEN_ENVIADA_SAE)
                    MostrarPopupVisor(Nothing)
                Case "LanzarManual"
                    marcarLanzada(Nothing)
                    OrdenSeteadorSelected = ListaOrdenes.Item(0)
                Case "Rechazar"
                    rechazarOrden(Nothing)
                Case "AsignarLiquidacionesProbables"
                    cargarLiquidaciones()
                Case "ReenviarMensaje"
                    Me.IsBusySeteador = False
                    OrdenEnvisor = True
                    MostrarPopupVisor("DESDEVISOR")
                Case "Cerrando"
                    AccionEjecutada = Accion.Ninguna
                    ActualizarEstadoOrden(String.Empty, Program.ES_Estado_Bolsa_Recibida, Program.ES_Estado_Visor_Mostrada_En_Visor, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
                    OrdenEnvisor = False
            End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el comando seleccionado desde el visor", Me.ToString(), "AccionElegida", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Si la orden está pendiente de respuesta en bolsa, no debe generar el item
    ''' </summary>
    ''' <param name="itm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PuedeGenerarComando(itm As String) As Boolean

        Try

            If VistaSeleccionada = "Pendientes" Then
                'If itm = Program.CMND_Marcar_Lanzada Then
                If itm = Program.CMND_Marcar_Lanzada Or itm = Program.CMND_Rechazar Then
                    Return Not OrdenSeteadorSelected.strEstadoVisor = Program.ES_Estado_Visor_En_BVC_Sin_Respuesta
                Else
                    Return Not (itm = Program.CMND_Lanzar_SAE And (OrdenSeteadorSelected.strEstadoVisor = Program.ES_Estado_Visor_En_BVC_Sin_Respuesta Or OrdenSeteadorSelected.strEstadoVisor = Program.ES_Estado_Visor_Enviada_SAE))
                End If

            Else
                Return itm = Program.CMND_Mostrar_En_Visor
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema definiendo la visibilidad de comandos de menú contextual", Me.ToString(), "PuedeGenerarComando", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Mensajes de notificaciones
    ''' </summary>
    ''' <param name="pstrHeader"></param>
    ''' <param name="pstrMensage"></param>
    ''' <remarks></remarks>
    Public Sub Notificaciones(pstrHeader As String, pstrMensage As String)

        'Try

        '    Dim nw As NotificationWindow = New NotificationWindow()
        '    nw.Width = 350
        '    nw.Height = 75
        '    AddHandler nw.Closed, AddressOf OnNotificationClosed

        '    Dim cw As Notificaiones = New Notificaiones()
        '    cw.HeaderText.Text = pstrHeader
        '    cw.BodyText.Text = pstrMensage

        '    nw.Content = cw
        '    AddNotificationToQueue(nw)

        'Catch ex As Exception
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema levantando mensaje de notificación", Me.ToString(), "Notificaciones", Application.Current.ToString(), Program.Maquina, ex)
        'End Try

    End Sub

    '''' <summary>
    '''' Desapilar notificaciones
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub OnNotificationClosed(ByVal sender As Object, ByVal e As EventArgs)

    '    _currentWindow = Nothing

    '    If _notifyQueue.Count > 0 Then
    '        Dim notifyWindow As NotificationWindow = _notifyQueue.Dequeue()
    '        _currentWindow = notifyWindow
    '        notifyWindow.Show(3000)
    '    End If

    'End Sub

    '''' <summary>
    '''' Apilar notificaciones
    '''' </summary>
    '''' <param name="notification"></param>
    '''' <remarks></remarks>
    'Private Sub AddNotificationToQueue(ByVal notification As NotificationWindow)

    '    If _currentWindow Is Nothing Then
    '        _currentWindow = notification
    '        notification.Show(3000)
    '    Else
    '        _notifyQueue.Enqueue(notification)
    '    End If

    'End Sub

    ''' <summary>
    ''' Lógica para decidir si el control de dato de referencia se muestra o se oculta
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PuedeAsignarreferencia() As Visibility
        Return CType(IIf(OrdenSeteadorSelected.strTipoNegocio = Program.TN_Acciones And VistaSeleccionada = "Procesadas", Visibility.Visible, Visibility.Collapsed), Visibility)
    End Function

    ''' <summary>
    ''' asigna la url del visor
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarRutaVisor()
        RutaVisor = Program.URLWebPageSeteador + OrdenSeteadorSelected.intIDOrdenes.ToString()
    End Sub
#If HAY_NOTIFICACIONES = 1 Then
    ''' <summary>
    ''' Método sobreescrito para recibir notificaciones
    ''' </summary>
    ''' <param name="pobjInfoNotificacion"></param>
    ''' <remarks></remarks>
    Public Overrides Sub Notificacion(pobjInfoNotificacion As A2Consola.Interfaces.clsNotificacion)
        MyBase.Notificacion(pobjInfoNotificacion)

        Try

            If pobjInfoNotificacion.strTopico = STR_TOPICO_NOTIFICACION_ORDENES Then

                If pobjInfoNotificacion.intConsecutivo > 0 Then SeleccionarOrden(pobjInfoNotificacion.intConsecutivo)

                Dim strData = pobjInfoNotificacion.strInfoMensaje.Split(CChar("|"))

                If strData.Length = 2 Then

                    Dim intIdOrden As Integer = pobjInfoNotificacion.intConsecutivo

                    If strData(1) <> Program.Usuario Then

                        Select Case strData(0)
                            Case MensajeNotificacion.AccionEjecutada.Creada.ToString()                      ' "Ingreso"
                                recargar("Ingreso")
                            Case MensajeNotificacion.AccionEjecutada.Modificada.ToString()                  ' "Modificacion"
                                OrdenEnvisor = intIdOrden = _idOrdenSeleccionada And VisorVisible
                                recargar("Modificacion")
                            Case MensajeNotificacion.AccionEjecutada.Rechazada.ToString()                   ' "Rechazo" 
                                rechazarOrden(MensajeNotificacion.AccionEjecutada.Rechazada.ToString())
                            Case MensajeNotificacion.AccionEjecutada.Cancelada.ToString()                   ' "Cancelada" 
                                rechazarOrden(MensajeNotificacion.AccionEjecutada.Cancelada.ToString())
                            Case MensajeNotificacion.AccionEjecutada.EnVisor.ToString()                     ' "En visor" 
                                MostrarPopupVisor(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.EnviadaSAE.ToString()                  ' "Enviada SAE"
                                EnviadaPorSAE(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.RecibidaSAE.ToString()                 ' "Recibida SAE"
                                RecibeSAE(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.EnviadaSAESinRespuesta.ToString()      ' "Enviada SAE sin respuesta"
                                EnviadaBVCSinRespuesta(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.RecibidaBUS.ToString()                 ' "Recibida BUS"
                                RecibidaPorBVC(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.DevueltaBUS.ToString()                 ' "Devuelta BUS"
                                DevueltaPorBVC(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.LanzadaRecibidaBVC.ToString()          ' "Lanzada Recibida BVC"
                                LanRecibidaXBVC(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.CalzadaRecibidaBVC.ToString()          ' "Calzada Recibida BVC"
                                CalzRecibidaXBVC(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.ComplementadaEnviada.ToString()        ' "Complementada Enviada"
                                ComplementadaEnviada(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.ComplementadaRecibida.ToString()       ' "Complementada recibida BVC"
                                ComplementadaRecibidaBVC(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.ComplementadaDevuelta.ToString()       ' "Complementada devuelta"
                                ComplementadaDevuelta(Nothing)
                            Case MensajeNotificacion.AccionEjecutada.LiquidacionesDisponibles.ToString()    ' "liquidaciones disponibles"
                                cargarLiquidaciones()
                        End Select

                        ReiniciaTimer()

                        Notificaciones("Ordenes seteador", "Desde notificaciones: " + pobjInfoNotificacion.strMensajeConsola)
                    End If
                Else
                    Throw New Exception("No se pudo interpretar el mensaje de la notificación: " & vbNewLine & pobjInfoNotificacion.strInfoMensaje)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema procesando el mensaje de notificación", Me.ToString(), "Notificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
#End If

    ''' <summary>
    ''' Método para envío de notificaciones
    ''' </summary>
    ''' <param name="pstrMensaje"></param>
    ''' <param name="pstrMensajeConsola"></param>
    ''' <remarks></remarks>
    Public Sub EnviarMensajeCliente(pstrMensaje As String, pstrMensajeConsola As String)

#If HAY_NOTIFICACIONES = 1 Then
        Try

            If pstrMensaje Is Nothing Then pstrMensaje = MensajeNotificacion.AccionEjecutada.Modificada.ToString()

            Dim objNC As clsNotificacionCliente = New clsNotificacionCliente
            objNC.objInfoNotificacion = New clsNotificacion With {.strTopico = STR_TOPICO_NOTIFICACION_SETEADOR,
                                                                  .strInfoMensaje = pstrMensaje + "|" + OrdenSeteadorSelected.strUsuarioIngreso,
                                                                  .strMensajeConsola = pstrMensajeConsola,
                                                                  .dtmFechaEnvio = Date.Now,
                                                                  .intConsecutivo = OrdenSeteadorSelected.intIDOrdenes}

            EnviarNotificacion(objNC)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema enviando mensaje de notificación", Me.ToString(), "EnviarMensajeCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
#End If

    End Sub

    '''' <summary>
    '''' Pendiente de mensajes del visor
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub messageReceiver_MessageReceived(sender As Object, e As MessageReceivedEventArgs)
    '    'e.Response = "response to " & e.Message
    '    'txtMensajeVisor = "Message: " & e.Message & Environment.NewLine & _
    '    '    "NameScope: " & e.NameScope.ToString() & Environment.NewLine & _
    '    '    "ReceiverName: " & e.ReceiverName & Environment.NewLine & _
    '    '    "SenderDomain: " & e.SenderDomain & Environment.NewLine & _
    '    '    "Response: " & e.Response
    '    AccionElegida(e.Message)
    'End Sub

    '''' <summary>
    '''' Verifica respuesta de mensaje y si no existe, lanza la aplicación visor
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub transmisor_SendCompleted(sender As Object, e As SendCompletedEventArgs)
    '    Try
    '        If Not e Is Nothing Then
    '            If Not e.Error Is Nothing Then
    '                If e.UserState <> muqiMsgValidacion Then
    '                    If Not IsNothing(objEmisor) Then
    '                        objEmisor.Dispose()
    '                    End If
    '                Else
    '                    AccionElegida("Cerrando")
    '                End If
    '                'Else
    '                'NotificarBloqueo(OrdenSeteadorSelected.lngID)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al procesar respuesta del canal (""" & GSTR_NOMBRE_CANAL_ORDENES & """).", _
    '                                   Me.ToString(), "transmisor_SendCompleted", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub


    ''' <summary>
    ''' Llama proceso de validación de estado de bloqueo de la orden
    ''' </summary>
    ''' <param name="plngID"></param>
    ''' <remarks></remarks>
    Private Sub ordenHabilitada(ByVal pstrUserState As String)
        Try

            Me.IsBusySeteador = True

            If Not IsNothing(dcProxy.EstadoFlujoOrdens) Then
                dcProxy.EstadoFlujoOrdens.Clear()
            End If

            dcProxy.Load(dcProxy.ValidarDisponibilidadOrdenQuery(OrdenSeteadorSelected.lngID, "VS", Program.Usuario, Program.HashConexion), AddressOf TerminoValidarOrden, pstrUserState)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema validar la disponibilidad de la orden.", Me.ToString(), "ordenHabilitada", Application.Current.ToString(), Program.Maquina, ex)
            IsBusySeteador = False
        End Try
    End Sub

    ''' <summary>
    ''' Llamada al procesos para calces automáticos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub calzarLiquidacionesProbables()
        Try

            Me.IsBusySeteador = True

            If Not IsNothing(dcProxy.Liquidacions) Then
                dcProxy.Liquidacions.Clear()
            End If

            dcProxy.Load(dcProxy.OrdenesSeteadorCalceAutomaticoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoCalceAutomatico, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el calce automático de las ordenes.", Me.ToString(), "calzarLiquidacionesProbables", Application.Current.ToString(), Program.Maquina, ex)
            IsBusySeteador = False
        End Try
    End Sub

    Public Sub ActualizarBloqueoOrden()
        Try
            If Not IsNothing(_ordenSeteadorSelected) Then
                dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(_ordenSeteadorSelected.lngID, "SETEADOR", Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al desbloquear la orden.", Me.ToString(), "ActualizarBloqueoOrden", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCancelarEditarRegistro(ByVal lo As InvokeOperation(Of Integer))
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al desbloquear la orden.",
                     Me.ToString(), "TerminoCancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#Region "Emulador"

    ''' <summary>
    ''' Emulador de notificaciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub MostrarEmulador(sender As Object)
        If Application.Current.Resources.Contains("VM") Then Application.Current.Resources.Remove("VM")
        Application.Current.Resources.Add("VM", Me)
        Dim wd As New Window()
        wd.Content = New EmuladorNotificaciones()
        wd.Show()
    End Sub

    ''' <summary>
    ''' Método para consultar las ordenes del seteador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub recargar(sender As Object)
        cargarVisorPorVista(_vistas.IndexOf(_vistaSeleccionada) + 1)
        EnviarMensajeCliente(CStr(sender), String.Empty)
    End Sub

    ''' <summary>
    ''' Elimina ordenes en la tabla de órdenes del seteador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub descartarOrden(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            BorrarOrdenSeteador(_vistas.IndexOf(_vistaSeleccionada) + 1)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.Cancelada.ToString(), STR_MENSAJE_CONSOLA_ORDEN_ELIMINADA)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Orden devuelta desde el seteador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub devolverOrdenSeteador(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            BorrarOrdenSeteador(_vistas.IndexOf(_vistaSeleccionada) + 1)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.Cancelada.ToString(), STR_MENSAJE_CONSOLA_ORDEN_DEVUELTA)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    Private Sub consultarLiquidaciones(sender As Object)
        cargarLiquidaciones()
    End Sub

    ' ''' <summary>
    ' ''' Dado un identificador de orden, asigna esa orden como orden seleccionada
    ' ''' </summary>
    ' ''' <param name="value"></param>
    ' ''' <remarks></remarks>
    'Private Sub SeleccionarOrden(value As Integer)
    '    Try
    '        If ListaOrdenes.Count > 0 Then
    '            'If (Not _cambiaVista) And value <> 0 Then
    '            '    OrdenSeteadorSelected = ListaOrdenes.Where(Function(i) i.lngID = value).FirstOrDefault
    '            'Else
    '            OrdenSeteadorSelected = ListaOrdenes.Item(0)
    '            '    _cambiaVista = False
    '            'End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se encontraron órdenes con el número " + value.ToString() + " en el sistema del seteador", Me.ToString(), "SeleccionarOrden", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Envía orden al sistema SAE
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub EnviadaPorSAE(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            enviarPorSAE(Nothing)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.EnviadaSAE.ToString(), STR_MENSAJE_CONSOLA_ORDEN_ENVIADA_SAE)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se recibe notificación de que una orden se envió desde SAE
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub RecibeSAE(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Recibida, Program.ES_Estado_Visor_Mostrada_En_Visor, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.EnviadaSAE.ToString(), STR_MENSAJE_CONSOLA_ORDEN_RECIBIDA_SAE)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Cuando se notifica de que una orden se ha enviado a la BVC, pero aún no se recibe respuesta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub EnviadaBVCSinRespuesta(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Lanzada, Program.ES_Estado_Visor_En_BVC_Sin_Respuesta, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.EnviadaSAESinRespuesta.ToString(), STR_MENSAJE_CONSOLA_ORDEN_ENVIADA_BVC_SIN_RESPUESTA)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Cuando la BVC informa que una orden fué recibida
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub RecibidaPorBVC(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Recibida, Program.ES_Estado_Visor_Mostrada_En_Visor, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.RecibidaBUS.ToString(), STR_MENSAJE_CONSOLA_ORDEN_RECIBIDA_BVC)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Cuando la BVC informa que una orden fué devuelta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub DevueltaPorBVC(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Recibida, Program.ES_Estado_Visor_Devuelta_SAE, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.DevueltaBUS.ToString(), STR_MENSAJE_CONSOLA_ORDEN_DEVUELTA_BVC)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Cuando la BVC informa que una orden fué lanzada y recibida por la BVC
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub LanRecibidaXBVC(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Lanzada, Program.ES_Estado_Visor_Recibida_SAE, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.LanzadaRecibidaBVC.ToString(), STR_MENSAJE_CONSOLA_ORDEN_ACEPTADA_BVC)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Cuando la BVC informa que hay ordenes calzadas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub CalzRecibidaXBVC(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Lanzada, Program.ES_Estado_Visor_Recibida_BVC_Calzada, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.CalzadaRecibidaBVC.ToString(), STR_MENSAJE_CONSOLA_ORDEN_CALZADA)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Cuando se notifica que una orden ha sido complementada 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub ComplementadaEnviada(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Complementada, Program.ES_Estado_Visor_Enviada_SAE, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.ComplementadaEnviada.ToString(), STR_MENSAJE_CONSOLA_ORDEN_COMPLEMENTADA_ENVIADA)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Cuando se notifica que una orden ha sido complementada y recibida por la BVC
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub ComplementadaRecibidaBVC(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Complementada, Program.ES_Estado_Visor_Recibida_BVC, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.ComplementadaRecibida.ToString(), STR_MENSAJE_CONSOLA_ORDEN_COMPLEMENTADA_RECIBIDA_BVC)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Cuando se notifica que una orden complementada ha sido devuelta por la BVC
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub ComplementadaDevuelta(sender As Object)
        If Not OrdenSeteadorSelected Is Nothing Then
            ActualizarEstadoOrden(OrdenSeteadorSelected.strEstado, Program.ES_Estado_Bolsa_Complementada, Program.ES_Estado_Visor_Devuelta, _vistas.IndexOf(_vistaSeleccionada) + 1, False)
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.ComplementadaDevuelta.ToString(), STR_MENSAJE_CONSOLA_ORDEN_COMPLEMENTADA_DEVUELTA)
        Else
            Notificaciones("Orden Seteador", "Se debe seleccionar una orden.")
        End If
    End Sub

    ''' <summary>
    ''' Inicia e informa mediante ventana emergente que la consulta de ordenes del seteador se realiza mediante temporizador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Private Sub ActivarRecargaAutomatica(sender As Object)
        If Program.Recarga_Automatica_Activa Then
            _myDispatcherTimerSeteador.Interval = New TimeSpan(0, 0, 0, 0, Program.Par_lapso_recarga)
            AddHandler _myDispatcherTimerSeteador.Tick, AddressOf Me.Each_Tick
            _myDispatcherTimerSeteador.Start()
            Notificaciones("Orden Seteador", "Se ha iniciado recarga automática de órdenes.")
        Else
            If Not IsNothing(_myDispatcherTimerSeteador) Then
                _myDispatcherTimerSeteador.Stop()
            End If
            Notificaciones("Orden Seteador", "La recarga automática se ha detenido")
        End If
    End Sub

    ''' <summary>
    ''' recarga de ordenes cada que se cumple el tiempo del temporizador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Each_Tick(sender As Object, e As EventArgs)

        porRecargaAutomatica = True
        recargar(Nothing)
        Notificaciones("Orden Seteador", "Ordenes cargadas mediante recarga automática.")
    End Sub

    ''' <summary>
    ''' Reinicia el tiempo para recargar consultas en caso de que no lleguen notificaciones
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ReiniciaTimer()
        Try
            If Program.Recarga_Automatica_Activa Then
                If _myDispatcherTimerSeteador Is Nothing Then
                    _myDispatcherTimerSeteador = New System.Windows.Threading.DispatcherTimer
                    _myDispatcherTimerSeteador.Interval = New TimeSpan(0, 0, 0, 0, Program.Par_lapso_recarga)
                    AddHandler _myDispatcherTimerSeteador.Tick, AddressOf Me.Each_Tick
                End If
                _myDispatcherTimerSeteador.Start()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al iniciar temporizador de recarga.",
                                       Me.ToString(), "ReiniciaTimer", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador()
        Try
            If Not _myDispatcherTimerSeteador Is Nothing Then
                _myDispatcherTimerSeteador.Stop()
                RemoveHandler _myDispatcherTimerSeteador.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerSeteador = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al detener temporizador de recarga.",
                                       Me.ToString(), "pararTemporizador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' proceso que genera el xml apropiado para el ingreso de liquidaciones probables
    ''' </summary>
    ''' <param name="strItems"></param>
    ''' <remarks></remarks>
    Private Sub generarXMLLiquidaciones(ByVal strItems As String)
        Try
            Dim strXmlLiquidaciones As String = "<Liqs>"

            If strItems.Length > 0 Then

                Dim strLquidaciones = strItems.Split(CChar("|"))
                Dim intParcial As Integer = 0

                For Each strItem As String In strLquidaciones

                    For Each liq In _lstLiquidaciones
                        If liq.numCantidad > OrdenSeteadorSelected.dblCantidad Then
                            If "Folio: " + liq.intNroFolio.ToString() + ", Nemotécnico : " + liq.strNemotecnico = strItem Then

                                strXmlLiquidaciones += "<Liq strTipo='" + OrdenSeteadorSelected.strTipo + "' strClaseOrden='" + OrdenSeteadorSelected.Clase +
                                    "' lngVersion='" + OrdenSeteadorSelected.lngVersion.ToString + "' intIDLiquidacion='" + liq.intNroFolio.ToString + "' lngParcial='" + intParcial.ToString + "' numMonto='" + OrdenSeteadorSelected.dblCantidad.ToString() + "' />"



                                Exit For
                            End If
                        Else
                            If "Folio: " + liq.intNroFolio.ToString() + ", Nemotécnico : " + liq.strNemotecnico = strItem Then

                                strXmlLiquidaciones += "<Liq strTipo='" + OrdenSeteadorSelected.strTipo + "' strClaseOrden='" + OrdenSeteadorSelected.Clase +
                                    "' lngVersion='" + OrdenSeteadorSelected.lngVersion.ToString + "' intIDLiquidacion='" + liq.intNroFolio.ToString + "' lngParcial='" + intParcial.ToString + "' numMonto='" + liq.numCantidad.ToString() + "' />"



                                Exit For
                            End If
                        End If

                    Next
                Next
            Else
                strXmlLiquidaciones += "<Liq strTipo='" + OrdenSeteadorSelected.strTipo + "' strClaseOrden='" + OrdenSeteadorSelected.Clase +
                               "' lngVersion='" + OrdenSeteadorSelected.lngVersion.ToString + "' intIDLiquidacion='" + NumeroReferencia.ToString + "' lngParcial='0' numMonto='" + OrdenSeteadorSelected.dblCantidad.ToString() + "' />"
            End If

            strXmlLiquidaciones += "</Liqs>"

            XMLLiquidaciones = strXmlLiquidaciones
        Catch
            Throw New Exception("Se presentó un problema al definir el archivo de liquidaciones probables 'generarXMLLiquidaciones'")
        End Try

    End Sub

    ''' <summary>
    ''' Método sobrecargado para la generación de XML para liquidaciones probables recorriendo una lista
    ''' </summary>
    ''' <param name="lstLiquidaciones"></param>
    ''' <remarks></remarks>
    Private Sub generarXMLLiquidaciones(lstLiquidaciones As List(Of OyDPLUSOrdenesBolsa.Liquidacion))
        Try

            Dim intParcial As Integer = 0

            Dim liqAEnviar = New List(Of OyDPLUSOrdenesBolsa.Liquidacion)

            Dim liDisponibles = From liqSeleccionadas In lstLiquidaciones
                                From liqDisponibles In _lstLiquidaciones
                                Where liqSeleccionadas.intNroFolio = liqDisponibles.intNroFolio
                                Select New OyDPLUSOrdenesBolsa.Liquidacion With {
                                    .dtmFechaCumplimiento = liqDisponibles.dtmFechaCumplimiento,
                                    .dtmFechaEmision = liqDisponibles.dtmFechaEmision,
                                    .dtmFechaVencimiento = liqDisponibles.dtmFechaVencimiento,
                                    .intDiasDeCumplimiento = liqDisponibles.intDiasDeCumplimiento,
                                    .intNroFolio = liqDisponibles.intNroFolio,
                                    .lngIdOrden = liqDisponibles.lngIdOrden,
                                    .numCantidad = liqDisponibles.numCantidad,
                                    .numMonto = liqDisponibles.numMonto,
                                    .numPrecio = liqDisponibles.numPrecio,
                                    .numTasa = liqDisponibles.numTasa,
                                    .strCodigoISIN = liqDisponibles.strCodigoISIN,
                                    .strMercado = liqDisponibles.strMercado,
                                    .strModalidad = liqDisponibles.strModalidad,
                                    .strNemotecnico = liqDisponibles.strNemotecnico,
                                    .strRuedaNegocio = liqDisponibles.strRuedaNegocio,
                                    .strTipoMercado = liqDisponibles.strTipoMercado,
                                    .strTrader = liqDisponibles.strTrader
                                }

            liqAEnviar = liDisponibles.ToList()

            Dim strXmlLiquidaciones As String = "<Liqs>"

            'For Each liq In liqAEnviar
            '    strXmlLiquidaciones += "<Liq strTipo='" + OrdenSeteadorSelected.strTipo +
            '                           "' strClaseOrden='" + OrdenSeteadorSelected.Clase +
            '                           "' lngVersion='" + OrdenSeteadorSelected.lngVersion.ToString +
            '                           "' intIDLiquidacion='" + liq.intNroFolio.ToString +
            '                           "' lngParcial='" + intParcial.ToString +
            '                           "' numMonto='" + liq.numMonto.ToString +
            '                           "' />"
            'Next
            For Each liq In liqAEnviar
                If liq.numCantidad > OrdenSeteadorSelected.dblCantidad Then
                    strXmlLiquidaciones += "<Liq strTipo='" + OrdenSeteadorSelected.strTipo + "' strClaseOrden='" + OrdenSeteadorSelected.Clase +
                               "' lngVersion='" + OrdenSeteadorSelected.lngVersion.ToString + "' intIDLiquidacion='" + liq.intNroFolio.ToString + "' lngParcial='" + intParcial.ToString + "' numMonto='" + OrdenSeteadorSelected.dblCantidad.ToString() + "' />"
                Else
                    strXmlLiquidaciones += "<Liq strTipo='" + OrdenSeteadorSelected.strTipo + "' strClaseOrden='" + OrdenSeteadorSelected.Clase +
                               "' lngVersion='" + OrdenSeteadorSelected.lngVersion.ToString + "' intIDLiquidacion='" + liq.intNroFolio.ToString + "' lngParcial='" + intParcial.ToString + "' numMonto='" + liq.numCantidad.ToString() + "' />"


                End If
            Next

            strXmlLiquidaciones += "</Liqs>"

            XMLLiquidaciones = strXmlLiquidaciones
        Catch
            Throw New Exception("Se presentó un problema al definir el archivo de liquidaciones probables 'generarXMLLiquidaciones'")
        End Try

    End Sub

    '''' <summary>
    '''' Indica que el visor fue cerrado (out of browser)
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub Termina(sender As Object, e As ClosingEventArgs)
    '    Try
    '        _wd = Nothing
    '        VisorVisible = False
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó establecer propiedades cuando se cierra el visor.", _
    '                                   Me.ToString(), "Termina", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Indica que el visor fue cerrado (ejecuntándose en browser)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Cierra(sender As Object, e As EventArgs)
        'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
        If OrdenEnvisor = False Then
            ActualizarBloqueoOrden()
        End If
        IsBusySeteador = False
        VisorVisible = False
    End Sub

    ''' <summary>
    ''' Envía mensaje al visor para que termine su ejecución
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CerrarVisor()
        Try
            If Not IsNothing(pobjViewVisorSeteadorView) Then
                pobjViewVisorSeteadorView.Close()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cerrar el visor.",
                                       Me.ToString(), "CerrarVisor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub enviarMensajeApp()
        Try
            If Not IsNothing(pobjViewVisorSeteadorView) Then
                pobjViewVisorSeteadorView.EstablecerPropiedades(RutaVisor, puedeLanzarSAE, puedeMarcarLanzada, puedeRechazar, VisibilidadNumeroReferencia, NumeroReferencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cerrar el visor.",
                                       Me.ToString(), "CerrarVisor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#End Region

#Region "Commands"

    ' Diferentes comandos llamados desde las vistas

    Private WithEvents _mostrarVisor As RelayCommand
    Public ReadOnly Property mostrarVisor() As RelayCommand
        Get
            If _mostrarVisor Is Nothing Then
                _mostrarVisor = New RelayCommand(AddressOf MostrarPopupVisor)
            End If
            Return _mostrarVisor
        End Get
    End Property

    Private WithEvents _lanzarSAE As RelayCommand
    Public ReadOnly Property lanzarSAE() As RelayCommand
        Get
            If _lanzarSAE Is Nothing Then
                _lanzarSAE = New RelayCommand(AddressOf enviarPorSAE)
            End If
            Return _lanzarSAE
        End Get
    End Property

    Private WithEvents _rechazar As RelayCommand
    Public ReadOnly Property Rechazar() As RelayCommand
        Get
            If _rechazar Is Nothing Then
                _rechazar = New RelayCommand(AddressOf rechazarOrden)
            End If
            Return _rechazar
        End Get
    End Property

    Private WithEvents _marcarLanzada As RelayCommand
    Public ReadOnly Property MarcarComoLanzada() As RelayCommand
        Get
            If _marcarLanzada Is Nothing Then
                _marcarLanzada = New RelayCommand(AddressOf marcarLanzada)
            End If
            Return _marcarLanzada
        End Get
    End Property

    Private WithEvents _liquidacionesProbables As RelayCommand
    Public ReadOnly Property AsignarLiquidacionesProbables() As RelayCommand
        Get
            If _liquidacionesProbables Is Nothing Then
                _liquidacionesProbables = New RelayCommand(AddressOf asignaLiquidacionesProbables)
            End If
            Return _liquidacionesProbables
        End Get
    End Property

    Private WithEvents _calzarLiquidaciones As RelayCommand
    Public ReadOnly Property CalzarLiquidaciones() As RelayCommand
        Get
            If _calzarLiquidaciones Is Nothing Then
                _calzarLiquidaciones = New RelayCommand(AddressOf calzarLiquidacionesProbables)
            End If
            Return _calzarLiquidaciones
        End Get
    End Property


#Region "Emulador"

    Private WithEvents _mostrarEmulador As RelayCommand
    Public ReadOnly Property emularNotificaciones() As RelayCommand
        Get
            If _mostrarEmulador Is Nothing Then
                _mostrarEmulador = New RelayCommand(AddressOf MostrarEmulador)
            End If
            Return _mostrarEmulador
        End Get
    End Property

    Private WithEvents _refrescar As RelayCommand
    Public ReadOnly Property Refrescar() As RelayCommand
        Get
            If _refrescar Is Nothing Then
                _refrescar = New RelayCommand(AddressOf recargar)
            End If
            Return _refrescar
        End Get
    End Property

    Private WithEvents _excluirOrden As RelayCommand
    Public ReadOnly Property excluirOrden() As RelayCommand
        Get
            If _excluirOrden Is Nothing Then
                _excluirOrden = New RelayCommand(AddressOf descartarOrden)
            End If
            Return _excluirOrden
        End Get
    End Property

    Private WithEvents _devolverOrden As RelayCommand
    Public ReadOnly Property devolverOrden() As RelayCommand
        Get
            If _devolverOrden Is Nothing Then
                _devolverOrden = New RelayCommand(AddressOf devolverOrdenSeteador)
            End If
            Return _devolverOrden
        End Get
    End Property

    Private WithEvents _consultaLiq As RelayCommand
    Public ReadOnly Property consultaLiquidaciones() As RelayCommand
        Get
            If _consultaLiq Is Nothing Then
                _consultaLiq = New RelayCommand(AddressOf consultarLiquidaciones)
            End If
            Return _consultaLiq
        End Get
    End Property

    Private WithEvents _enviadaSAE As RelayCommand
    Public ReadOnly Property EnviadaXSAE() As RelayCommand
        Get
            If _enviadaSAE Is Nothing Then
                _enviadaSAE = New RelayCommand(AddressOf EnviadaPorSAE)
            End If
            Return _enviadaSAE
        End Get
    End Property

    Private WithEvents _recibeSAE As RelayCommand
    Public ReadOnly Property SAERecibe() As RelayCommand
        Get
            If _recibeSAE Is Nothing Then
                _recibeSAE = New RelayCommand(AddressOf RecibeSAE)
            End If
            Return _recibeSAE
        End Get
    End Property

    Private WithEvents _enviaBolsaNoRespuesta As RelayCommand
    Public ReadOnly Property EnviadaBVCNoRespuesta() As RelayCommand
        Get
            If _enviaBolsaNoRespuesta Is Nothing Then
                _enviaBolsaNoRespuesta = New RelayCommand(AddressOf EnviadaBVCSinRespuesta)
            End If
            Return _enviaBolsaNoRespuesta
        End Get
    End Property

    Private WithEvents _recibidaXBVC As RelayCommand
    Public ReadOnly Property RecibidaXBVC() As RelayCommand
        Get
            If _recibidaXBVC Is Nothing Then
                _recibidaXBVC = New RelayCommand(AddressOf RecibidaPorBVC)
            End If
            Return _recibidaXBVC
        End Get
    End Property

    Private WithEvents _devueltaXBVC As RelayCommand
    Public ReadOnly Property DevueltaBVC() As RelayCommand
        Get
            If _devueltaXBVC Is Nothing Then
                _devueltaXBVC = New RelayCommand(AddressOf DevueltaPorBVC)
            End If
            Return _devueltaXBVC
        End Get
    End Property

    Private WithEvents _lanzRecibidaBVC As RelayCommand
    Public ReadOnly Property LanRecibidaBVC() As RelayCommand
        Get
            If _lanzRecibidaBVC Is Nothing Then
                _lanzRecibidaBVC = New RelayCommand(AddressOf LanRecibidaXBVC)
            End If
            Return _lanzRecibidaBVC
        End Get
    End Property

    Private WithEvents _calzRecibidaBVC As RelayCommand
    Public ReadOnly Property CalzRecibidaBVC() As RelayCommand
        Get
            If _calzRecibidaBVC Is Nothing Then
                _calzRecibidaBVC = New RelayCommand(AddressOf CalzRecibidaXBVC)
            End If
            Return _calzRecibidaBVC
        End Get
    End Property

    Private WithEvents _compEnviada As RelayCommand
    Public ReadOnly Property CompEnviada() As RelayCommand
        Get
            If _compEnviada Is Nothing Then
                _compEnviada = New RelayCommand(AddressOf ComplementadaEnviada)
            End If
            Return _compEnviada
        End Get
    End Property

    Private WithEvents _compRecibidaBVC As RelayCommand
    Public ReadOnly Property CompRecibidaBVC() As RelayCommand
        Get
            If _compRecibidaBVC Is Nothing Then
                _compRecibidaBVC = New RelayCommand(AddressOf ComplementadaRecibidaBVC)
            End If
            Return _compRecibidaBVC
        End Get
    End Property

    Private WithEvents _compDevuelta As RelayCommand
    Public ReadOnly Property CompDevuelta() As RelayCommand
        Get
            If _compDevuelta Is Nothing Then
                _compDevuelta = New RelayCommand(AddressOf ComplementadaDevuelta)
            End If
            Return _compDevuelta
        End Get
    End Property


    Private WithEvents _autoRecarga As RelayCommand
    Public ReadOnly Property autoRecarga() As RelayCommand
        Get
            If _autoRecarga Is Nothing Then
                _autoRecarga = New RelayCommand(AddressOf ActivarRecargaAutomatica)
            End If
            Return _autoRecarga
        End Get
    End Property


#End Region

#End Region

End Class

Friend NotInheritable Class ShowAndWaitHelper
    Private ReadOnly _window As Window
    Private _dispatcherFrame As DispatcherFrame

    Friend Sub New(ByVal window As Window)
        If window Is Nothing Then
            Throw New ArgumentNullException("panel")
        End If

        Me._window = window
    End Sub

    Friend Sub ShowAndWait()
        If Me._dispatcherFrame IsNot Nothing Then
            Throw New InvalidOperationException("Cannot call ShowAndWait while waiting for a previous call to ShowAndWait to return.")
        End If

        AddHandler Me._window.Closed, New EventHandler(AddressOf Me.OnPanelClosed)
        _window.ShowDialog()
        Me._dispatcherFrame = New DispatcherFrame()
        Dispatcher.PushFrame(Me._dispatcherFrame)
    End Sub

    Private Sub OnPanelClosed(ByVal source As Object, ByVal eventArgs As EventArgs)
        If Me._dispatcherFrame Is Nothing Then
            Return
        End If

        RemoveHandler Me._window.Closed, New EventHandler(AddressOf Me.OnPanelClosed)
        Me._dispatcherFrame.[Continue] = False
        Me._dispatcherFrame = Nothing
    End Sub
End Class