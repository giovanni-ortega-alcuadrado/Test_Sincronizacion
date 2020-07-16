Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: OrdenesViewModel.vb
'Generado el : 07/21/2011 08:36:53
'Propiedad de Alcuadrado S.A. 2010
'
' Adaptar código generado a necesidades del sistema: Cristian Ciceri Muñetón - Julio/2011
'           - Modificación del diseño
'           - Propiedad para identificar si trabaja órdenes de acciones o de renta fija
'           - Referencia al view model mediante la variable mobjVM
'           - Incluir carga de combos específicos
'           - Manejador de error en el control
'           - Ajustes a funcionalidad del negocio
'
' ATENCIÓN: El maker and checker de este control tiene algunas diferencias con el estándar de otros controles
' ========
'             - Para saber si se tiene maker and checker se consulta en la carga de combos específicos y no se retorna con cada registro
'             - La barra de herramientas (a2controlmenu) es una versión diferente
'

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class OrdenesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"


    Public Sub New()
        cb = New CamposBusquedaOrden(Me)
    End Sub


    Private Sub inicializarServicio()

        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OrdenesDomainContext()
                dcProxy1 = New OrdenesDomainContext()
                dcproxy2 = New OrdenesDomainContext()
                dcProxyConsultas = New OrdenesDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
                'dcProxyImportaciones = New ImportacionesDomainContext()
            Else
                dcProxy = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcproxy2 = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyConsultas = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                'dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OrdenesDomainContext.IOrdenesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
            DirectCast(dcProxy1.DomainClient, WebDomainClient(Of OrdenesDomainContext.IOrdenesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
            DirectCast(dcproxy2.DomainClient, WebDomainClient(Of OrdenesDomainContext.IOrdenesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
            DirectCast(dcProxyConsultas.DomainClient, WebDomainClient(Of OrdenesDomainContext.IOrdenesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)


            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de orden que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If IsNothing(Me.ClaseOrden) Then
                _mstrClaseOrden = String.Empty
            End If

            Select Case Me.ClaseOrden
                Case ClasesOrden.A
                    Me._mintClaseEspecie = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
                    Me._mlogEsRentaFija = False
                    Me._mlogEsRentaVariable = True
                    FECHATOMA_IGUAL_FECHASISTEMA_ACC = Program.FECHATOMA_IGUAL_FECHASISTEMA.ToUpper
                    HabilitarSeleccionISIN = False
                    SeleccionarUnISIN = True
                    PermitirSeleccionarEspecieOIsin = False
                Case ClasesOrden.C
                    Me._mintClaseEspecie = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                    Me._mlogEsRentaFija = True
                    Me._mlogEsRentaVariable = False
                    If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                        RF_TTV_Regreso = "4"
                        RF_TTV_Salida = "3"
                        RF_REPO = "RP"
                        RF_Simulatena_Regreso = "2"
                        RF_Simulatena_Salida = "1"
                        RF_Simultanea_Regreso_CRCC = "6"
                        RF_Simultanea_Salida_CRCC = "5"
                        RF_SWAP = "SW"
                        RF_MERCADO_REPO = "E"
                        RF_MERCADO_PRIMARIO = "P"
                        RF_MERCADO_RENOVACION = "R"
                        RF_MERCADO_SECUNDARIO = "S"
                    Else
                        RF_TTV_Regreso = Program.RF_TTV_Regreso
                        RF_TTV_Salida = Program.RF_TTV_Salida
                        RF_REPO = Program.RF_REPO
                        RF_Simulatena_Regreso = Program.RF_Simulatena_Regreso
                        RF_Simulatena_Salida = Program.RF_Simulatena_Salida
                        RF_Simultanea_Regreso_CRCC = Program.RF_Simultanea_Regreso_CRCC
                        RF_Simultanea_Salida_CRCC = Program.RF_Simultanea_Salida_CRCC
                        RF_SWAP = Program.RF_SWAP
                        RF_MERCADO_REPO = Program.RF_Mercado_Repo
                        RF_MERCADO_PRIMARIO = Program.RF_Mercado_Primario
                        RF_MERCADO_RENOVACION = Program.RF_Mercado_Renovacion
                        RF_MERCADO_SECUNDARIO = Program.RF_Mercado_Secundario
                    End If
                    HabilitarSeleccionISIN = True
                    SeleccionarUnISIN = True
                    PermitirSeleccionarEspecieOIsin = True
                Case Else
                    Me._mintClaseEspecie = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
                    Me._mlogEsRentaFija = False
                    Me._mlogEsRentaVariable = False
            End Select

            mdtmFechaCierreSistema = DateAdd(DateInterval.Year, -5, Now()).Date

            '---------------------------------------------------------------------------------------------------------------------
            '-- Cargar datos iniciales de la lista de órdenes
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                IsBusy = True
                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.listaVerificaparametroQuery("", "Ordenes", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerParametros, Nothing)

                mlogInicializado = True

                HabilitarBotones()
            End If

            ' If Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then 'Si es City
            '_HabilitarSeleccionISIN = True
            ' End If

            MyBase.CambioItem("RentaFijaVisible")
            MyBase.CambioItem("RentaVariableVisible")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "OrdenesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        validarPermisosAdicionar()
        ValidarPermisosDuplicar()

    End Sub

    Private Sub TerminoTraerParametros(ByVal lo As LoadOperation(Of OYDUtilidades.valoresparametro))
        Try
            If lo.HasError = False Then
                objListaParametros = lo.Entities.ToList

                If logEsModal = False Then
                    dcProxy.Load(dcProxy.OrdenesFiltrarQuery(Me.ClaseOrden.ToString(), "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "")
                    dcProxy1.Load(dcProxy1.TraerOrdenPorDefectoQuery(Me.ClaseOrden.ToString(), Program.Usuario, "", Program.HashConexion), AddressOf TerminoTraerOrdenesPorDefecto_Completed, "Default")
                Else
                    ListaOrdenes = dcProxy.Ordens
                    dcProxy1.Load(dcProxy1.TraerOrdenPorDefectoQuery(Me.ClaseOrden.ToString(), Program.Usuario, "", Program.HashConexion), AddressOf TerminoTraerOrdenesPorDefecto_Completed, "Default")
                End If
            Else
                lo.MarkErrorAsHandled()
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los parametros de ordenes.", Me.ToString(), "TerminoTraerParametros", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Eventos"
    Friend Event seleccionarTipoOrden()
    Public Event TerminoConfigurarNuevoRegistro()
    Public Event TerminoGuardarRegistro(ByVal plogGuardoRegistro As Boolean, ByVal plngIDOrden As Integer, ByVal pstrTipoOrden As String, ByVal pstrTipoOperacion As String)
#End Region

#Region "Constantes"

    ''' <summary>
    ''' A : Acciones
    ''' C : Crediticia
    ''' </summary>
    Public Enum ClasesOrden
        A '// Acciones
        C '// Crediticia
    End Enum

    ''' <summary>
    ''' C : Compra
    ''' R : Recompra
    ''' V : Venta
    ''' S : Reventa
    ''' </summary>
    Public Enum TiposOrden
        C '// Compra
        R '// Recompra
        V '// Venta
        S '// Reventa
    End Enum

    ''' <summary>
    ''' L : Límite
    ''' M : Mercado
    ''' C : Condicionada
    ''' </summary>
    Public Enum TipoLimite As Byte
        L
        M
        C
    End Enum

    ''' <summary>
    ''' C : Cantidad Minima
    ''' K : Fill and Kill
    ''' F : Fill or Kill
    ''' N : Ninguna
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TipoEjecucion As Byte
        C
        K
        F
        N
    End Enum

    ''' <summary>
    ''' A - Hasta Hora
    ''' C - Hasta Cancelación
    ''' D - Día
    ''' F - Hasta Fecha
    ''' I - Inmediata
    ''' S - Sesion
    ''' </summary>
    Public Enum TipoDuracion As Byte
        A
        C
        D
        F
        I
        S
    End Enum

    ''' <summary>
    ''' PA : Pendiente por aprobar
    ''' NA : No aprobado (rechazado)
    ''' A  : Aprobado
    ''' PV : Con versión por aprobbar
    ''' </summary>
    Public Enum EstadoMakerChecker As Byte
        PA
        NA
        A
        PV
    End Enum

    Private Enum OrdenTabs As Byte
        LEO
        Caracteristicas
        Otros
        Receptores
        Benficiarios
        Instrucciones
        Pago
        Comisiones
        LiqPosibles
    End Enum

    Private Const MSTR_MC_BOTON_TEXTO_VERSION_APROBADA As String = "Ver versión aprobada"
    Private Const MSTR_MC_BOTON_TEXTO_VERSION_POR_APROBAR As String = "Ver versión por aprobar"

    Private Const MSTR_MC_ACCION_INGRESAR As String = "I"
    Private Const MSTR_MC_ACCION_ACTUALIZAR As String = "U"
    Private Const MSTR_MC_ACCION_BORRAR As String = "D"

    Private Const MSTR_ACCION_EDITAR As String = "editar"
    Private Const MSTR_ACCION_ANULAR As String = "anular"
    Private Const MSTR_ACCION_APROBAR As String = "aprobar"
    Private Const MSTR_ACCION_BUSCAR As String = "buscar"
    Private Const MSTR_ACCION_FILTRAR As String = "filtrar"
    Private Const MSTR_ACCION_CALCULAR_DIAS As String = "dias"
    Private Const MSTR_ACCION_CALCULAR_FECHA As String = "fecha"

    Private Const MSTR_ACC_CONSULTA_RECEPTORES_CLT As String = "receptorescliente"
    Friend Const MSTR_CALCULAR_DIAS_ORDEN As String = "vencimiento_orden"
    Friend Const MSTR_CALCULAR_DIAS_TITULO As String = "vencimiento_titulo"

    Private MSTR_MODULO_OYD_ORDENES As String = "O"

    Private MSTR_MENSAJE_EXCEPCION_RDIP As String = "El rating del producto no corresponde al perfil del Cliente." & vbNewLine & vbNewLine & "¿Desea continuar?"

    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17
    Private MINT_LONG_MAX_NEMOTECNICO As Byte = 15

    '(SLB) Constante para el Tópico de Instrucciones Ordenes
    Public Const STR_TOPICO_INST_ORDENES_COMPRA As String = "INST_ORDENES_C"
    Public Const STR_TOPICO_INST_ORDENES_VENTA As String = "INST_ORDENES_V"

    Public Const MSTR_TITULO_INSTRUCCION_MODIFICAR As String = "Mod Instr."
    Public Const MSTR_TITULO_INSTRUCCION_GUARDAR As String = "Grabar Instr."

    Public Const MSTR_CTADEPOSITO_TITULO_FISICO As String = "F"
    Public Const MSTR_CTADEPOSITO_TITULO_EXTERIOR As String = "X"

#End Region

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Private mlogRecalcularFechas As Boolean = True 'CCM20120305. Pemrite activar o desactivar el recálculo de las fechas de vigencia y vencimiento y días de vigencia y palzo
    Private mlogRecalcularFechaRecepcion As Boolean = True

    Private mlogDuplicarOrden As Boolean = False ' Indica si se está duplicando una orden
    Private mlogPuedeDuplicarOrden As Boolean = False ' Indica si el usuario tiene autorización para duplicar órdenes

    Private mlogDuplicarDatosParam As Boolean = False 'CCM20120305. Indica si ya se inicializaron los parámetros que definen algunas características de configuración de las órdenes
    Private mlogDuplicarDatosLeo As Boolean = False 'CCM20120305. Por defecto no duplica datos de LEO
    Private mlogDuplicarFechaLeo As Boolean = False
    Private mlogDuplicarDatosPrecio As Boolean = True 'CCM20120305. Por defecto duplica el precio de la orden
    Private mlogIngresarFechaTomaCierre As Boolean = False 'NAOC20150203. Indica si se puede ingresar una fecha toma mayor o inferior al cierre.
    Private mlogModificarechaTomaIgualSistema As Boolean = False 'NAOC20150710. Indica si se permite modificar a fecha y hora de toma en la ordenes menor o igual a la del usuario.

    Private mlogAplicaTipoTransaccion As Boolean = False 'SLB20120801 - Parámetros para verificar si aplica Tipo de Transaccion y Excepciones RDIP
    Private mlogAplicaExcepcionRDIP As Boolean = False
    Private mlogGuardarPagoIncompleto As Boolean = True
    Private mlogGuardarSwap As Boolean = True

    Private _mlogHabilitarFechaElaboracion As Boolean = True 'SLB20121128. Habilitar Fecha Elaboración
    Public _mlogMostrarTodosReceptores As Boolean = True 'SLB20121128. Por defecto debe mostrar todos los receptores
    Private _mlogCargarReceptorClientes As Boolean = True
    Private _mlogRequiereIngresoOrdenantes As Boolean = False

    Private mlogUsuarioPuedeIngresar As Boolean = True 'CCM20120305. Indica que el usuario tiene el botón Nuevo activo en la barra de herramientas

    Private mlogSAEActivo As Boolean = True ' Indica si la funcionalidad para enrutar órdenes por SAE está activa
    Private mlogSAEActivoClaseOrden As Boolean = True ' Indica si la funcionalidad para enrutar órdenes por SAE está activa para la clase de orden (renta fija, acciones)
    Private mlogPuedeEnrutarSAE As Boolean = True ' Indica si el usuario está autorizado para enrutar órdenes por SAE

    Private mintUltimoId As Integer = -200000000

    Private mstrAccionOrden As String = String.Empty '// Este indicador ayuda a controla la ejecución de consultas inutiles durante el ingreso especialmente
    Private mstrTipoLimitePorDef As String = "M" 'CCM20120305. El tipo de natruraleza o tipo límite es Mercado (M)
    Private mstrFormaPagoPorDef As String = "C" 'SLB20130621. la forma de pago por defecto es Cheque (C)
    Private intPorcentajeCantidadVisible As Integer = 50 'SLB20121126. El porcentaje minimo de la Cantidad Visible sola para acciones
    Private mstrPago As String = "NO" 'SLB20130726 Validar la información incompleta del tab Pago en Renta Fija
    Private mstrValidarCuidadSeteo As String = "NO" 'SLB20130726 Validar la información de la cuidad de Seteo de las Órdenes
    Private mstrCuidadSeteo As String
    Private dblPatrimonioTecnico As Double = 0 'SLB20130731 Validar el patrimonio técnico de la firma
    Private logValidarCuentaDeposito As Boolean = False 'adicionado por juan david correa para validar la cuenta deposito
    Private logBorrarCamposLeo As Boolean = False 'SLB20130930 Manejo campos de LEO
    Private mstrDuracion As String = "I" 'SLB20140129 La Duración por defecto (I - Inmediata)

    Private mlogEspecieAccion As Boolean     'SLB20130731 Manejo de la Especies en Renta Fija
    Private mstrEspecieTipoTasa As String    'SLB20130731 Manejo de la Especies en Renta Fija

    Private logRegistroLibroOrdenes As Boolean = False 'SLB20130801 Registro en Libro de Órdenes
    Private dblValorCantidadLibroOrden As Double = 0

    Private mdtmFechaCierreSistema As Date

    Public Property cb As CamposBusquedaOrden

    Private OrdenPorDefecto As OyDOrdenes.Orden
    Private OrdenAnterior As OyDOrdenes.Orden

    Private dcProxy As OrdenesDomainContext
    Private dcProxy1 As OrdenesDomainContext
    Private dcproxy2 As OrdenesDomainContext
    Private dcProxyConsultas As OrdenesDomainContext

    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext

    Private _mobjReceptorTomaSeleccionadoAntes As OYDUtilidades.BuscadorGenerico
    Private _mobjNemotecnicoSeleccionadoAntes As OYDUtilidades.BuscadorEspecies
    Private _mobjComitenteSeleccionadoAntes As OYDUtilidades.BuscadorClientes

    Private _strObjetoAnterior As String
    Private _strObjetoAnteriorValidar As String = " "

    'SLB se adiciona estas variables, para asignarles el valor del archivo de parametros.
    Private RF_TTV_Regreso As String = ""
    Private RF_TTV_Salida As String = ""
    Private RF_REPO As String = ""
    Private RF_Simulatena_Regreso As String = ""
    Private RF_Simulatena_Salida As String = ""
    Private RF_Simultanea_Regreso_CRCC As String = ""
    Private RF_Simultanea_Salida_CRCC As String = ""
    Private RF_SWAP As String = ""

    Private RF_MERCADO_REPO As String = ""
    Private RF_MERCADO_PRIMARIO As String = ""
    Private RF_MERCADO_RENOVACION As String = ""
    Private RF_MERCADO_SECUNDARIO As String = ""

    Private FECHATOMA_IGUAL_FECHASISTEMA_ACC As String = "NO" 'SLB20140513 Si es igual a SI se comparta diferente a VB.6

    Private _mlogFechaCierreSistema As Boolean = False

    Private _EstadoMakerCheckerConsultar As String = ""
    Private _EstadoMakerCheckerConsultarInstrucciones As String = ""
    Private _EstadoMakerCheckerConsultarAdicionales As String = ""

    Dim objDicUsuarioOperador As List(Of OYDUtilidades.ItemCombo) = Nothing
    Dim ExisteUsuarioOperador As Boolean = True
    Dim ActivoExento As String = ""
    Dim EsBono As Boolean = False
    Dim EsEdicion As Boolean = False


    'Private dcProxyImportaciones As ImportacionesDomainContext
    'Private _NroProceso As System.Nullable(Of Double)
    'Private _DispatcherTimerOrdenesLEO As System.Windows.Threading.DispatcherTimer
    'Private lstMensajeEstadoGeneracionOrdenLEO As New List(Of String)

    Dim intDiasVigenciaDuracionInmediata As Integer

    Private logValorCompromisoFuturoRequerido As Boolean = False 'Adicionado por Jorge Andres Bedoya para validar el campo valor compromiso futuro 2014/12/29

    Private mlogMsgDuplicarDatosLeo As Boolean = False 'JABG20160615. Validar por mensaje la fecha de recepcion al duplicar la orden

    Private objListaParametros As List(Of OYDUtilidades.valoresparametro)
    Dim intIDOrdenActualizada As Integer = 0
    Public logActualizarFechaElaboracion As Boolean = True
    Public logActualizarFechaRecepcion As Boolean = True

    'VARIABLES CREADAS PARA MANEJAR EL MODAL
    Public logEsModal As Boolean = False

#End Region

#Region "Propiedades para manejo según la clase de la orden"

    Private Sub validarClaseOrden()
        If (_mlogEsRentaFija And _mlogEsRentaVariable) Or _mstrClaseOrden.Equals(String.Empty) Then
            '/ Validar que por algún problema no se habiliten las dos opciones simultáneamente
            _mlogEsRentaFija = False
            _mlogEsRentaVariable = False
            A2Utilidades.Mensajes.mostrarMensaje("Existe un problema con la orden y el sistema no puede identificarla. Por favor cierre la forma de órdenes e ingrese nuevamente", Program.TituloSistema)
            IsBusy = True
        End If
    End Sub

    Private _mstrClaseOrden As ClasesOrden
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de acciones o de renta fija. Solamente se asigna valor cuando se inicializa el control.
    ''' </summary>
    Public Property ClaseOrden As ClasesOrden
        Get
            validarClaseOrden()
            Return (_mstrClaseOrden)
        End Get
        Set(ByVal value As ClasesOrden)
            If mlogInicializado = False Then ' Esta propiedad no se deja modificar despúes de inicializado el control
                _mstrClaseOrden = value
                inicializarServicio()
            End If
        End Set
    End Property

    Private _mintClaseEspecie As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie
    ''' <summary>
    ''' Propiedad para filtrar las especies disponibles en la orden
    ''' </summary>
    Public ReadOnly Property ClaseEspecie As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie
        Get
            Return (_mintClaseEspecie)
        End Get
    End Property

    ''' <summary>
    ''' Propiedad que define el título de la forma según la calse de orden
    ''' </summary>
    Public ReadOnly Property TituloForma As String
        Get
            If _mlogEsRentaFija Then
                Return ("Órdenes de Renta Fija")
            ElseIf _mlogEsRentaVariable Then
                Return ("Órdenes de Acciones")
            Else
                Return ("Órdenes")
            End If
        End Get
    End Property

    Private _mlogEsRentaVariable As Boolean = False
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de renta variable (true) o no (false)
    ''' </summary>
    Public ReadOnly Property EsRentaVariable As Boolean
        Get
            validarClaseOrden()
            Return (_mlogEsRentaVariable)
        End Get
    End Property

    Private _mlogEsRentaFija As Boolean = False
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de renta fija (true) o no (false)
    ''' </summary>
    Public ReadOnly Property EsRentaFija As Boolean
        Get
            validarClaseOrden()
            Return (_mlogEsRentaFija)
        End Get
    End Property

    ''' <summary>
    ''' Permite visualizar o no los controles propios de renta fija
    ''' </summary>
    Public ReadOnly Property RentaFijaVisible As System.Windows.Visibility
        Get
            validarClaseOrden()

            If (_mlogEsRentaFija) Then
                Return (System.Windows.Visibility.Visible)
            Else
                Return (System.Windows.Visibility.Collapsed)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica si se está trabajando con órdenes de renta variable (true) o no (false)
    ''' </summary>
    Public ReadOnly Property RentaVariableVisible As System.Windows.Visibility
        Get
            validarClaseOrden()

            If (_mlogEsRentaVariable) Then
                Return (System.Windows.Visibility.Visible)
            Else
                Return (System.Windows.Visibility.Collapsed)
            End If
        End Get
    End Property

#End Region

#Region "Propiedades que definen atributos de la orden"

    Private _mstrIdComitente As String
    Public Property ComitenteOrden() As String
        Get
            Return (_mstrIdComitente)
        End Get
        Set(ByVal value As String)
            If value.Equals(String.Empty) Then
                _mstrIdComitente = value
                ComitenteSeleccionado = Nothing
            ElseIf Not Versioned.IsNumeric(value) Then
                A2Utilidades.Mensajes.mostrarMensaje("El código del comitente debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _ComitenteSeleccionado Is Nothing Then
                    _mstrIdComitente = String.Empty
                Else
                    _mstrIdComitente = _ComitenteSeleccionado.IdComitente
                End If
            ElseIf value.ToString.Length() > MINT_LONG_MAX_CODIGO_OYD Then
                A2Utilidades.Mensajes.mostrarMensaje("La longitud máxima del código del comitente es de 17 caracteres", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _ComitenteSeleccionado Is Nothing Then
                    _mstrIdComitente = String.Empty
                Else
                    _mstrIdComitente = _ComitenteSeleccionado.IdComitente
                End If
            Else
                _mstrIdComitente = value

                If Not _OrdenSelected Is Nothing AndAlso (_ComitenteSeleccionado Is Nothing OrElse Not value.Equals(_ComitenteSeleccionado.IdComitente)) Then
                    'SLB se le adiciona buscarComitenteOrden para poder identificar si el Comitente esta inactivo.
                    buscarComitente(Right(Space(17) & value, MINT_LONG_MAX_CODIGO_OYD), "buscarComitenteOrden")
                End If
            End If

            MyBase.CambioItem("ComitenteOrden")
        End Set
    End Property

    Private _mstrNemotecnicoOrden As String
    Public Property NemotecnicoOrden() As String
        Get
            Return (_mstrNemotecnicoOrden)
        End Get
        Set(ByVal value As String)
            If value.Equals(String.Empty) Then
                _mstrNemotecnicoOrden = value
                NemotecnicoSeleccionado = Nothing
            ElseIf value.ToString.Length() > MINT_LONG_MAX_NEMOTECNICO Then
                A2Utilidades.Mensajes.mostrarMensaje("La longitud máxima del nemotécnico de la especie es de 15 caracteres", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _NemotecnicoSeleccionado Is Nothing Then
                    _mstrNemotecnicoOrden = String.Empty
                Else
                    _mstrNemotecnicoOrden = _NemotecnicoSeleccionado.Nemotecnico
                End If
            Else
                _mstrNemotecnicoOrden = value

                If Not _OrdenSelected Is Nothing AndAlso (_NemotecnicoSeleccionado Is Nothing OrElse Not value.ToUpper().Equals(_NemotecnicoSeleccionado.Nemotecnico.ToUpper())) Then
                    buscarNemotecnico(value)
                End If
            End If

            MyBase.CambioItem("NemotecnicoOrden")
        End Set
    End Property

    Private _mintDiasVigencia As Integer = 0
    ''' <summary>
    ''' Días de vigencia de la orden. Es un dato calculado que no se almacena con la orden. El usuario lo puede modificar directamente o se 
    ''' calcula con la fecha de la orden (elaboración) y la fecha de vigencia.
    ''' </summary>
    Public Property DiasVigencia() As Integer
        Get
            Return (_mintDiasVigencia)
        End Get
        Set(ByVal value As Integer)
            If Not _mintDiasVigencia.Equals(value) Then
                _mintDiasVigencia = value

                If Not IsNothing(Me.OrdenSelected) Then
                    Try
                        mlogRecalcularFechas = False 'CCM20120305
                        calcularDiasOrden(MSTR_CALCULAR_DIAS_ORDEN, _mintDiasVigencia)
                    Catch ex As Exception
                        mlogRecalcularFechas = True 'CCM20120305
                    End Try
                End If

                MyBase.CambioItem("DiasVigencia")
            End If
        End Set
    End Property

    Private _mintDiasPlazo As Integer = 0
    ''' <summary>
    ''' Días de plazo del título de la orden. Es un dato calculado que no se almacena con la orden. Solamente para renta fija y no puede ser modificado por el usuario.
    ''' Se calcula con la fecha de emisión del título y la fecha de vencimiento.
    ''' </summary>
    Public ReadOnly Property DiasPlazo() As Integer
        Get
            Return (_mintDiasPlazo)
        End Get
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

    Private _mstrEstadoOrden As String = String.Empty
    Public Property EstadoOrden As String
        Get
            Return (_mstrEstadoOrden)
        End Get
        Set(ByVal value As String)
            Dim objEstados As List(Of OYDUtilidades.ItemCombo)
            If Application.Current.Resources.Contains(Me.ListaCombosEsp) Then
                objEstados = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_ESTADOS_ORDEN"))

                If IsNothing(objEstados) Then
                    _mstrEstadoOrden = ""
                Else
                    If objEstados.Where(Function(i) i.ID = value).Count > 0 Then
                        _mstrEstadoOrden = (From obj In objEstados Where obj.ID = value Select obj).First.Descripcion
                    End If
                End If
            Else
                _mstrEstadoOrden = ""
            End If
            MyBase.CambioItem("EstadoOrden")
        End Set
    End Property

    Private _mstrEstadoOrdenLeo As String = String.Empty
    Public Property EstadoOrdenLeo As String
        Get
            Return (_mstrEstadoOrdenLeo)
        End Get
        Set(ByVal value As String)
            Dim objEstados As List(Of OYDUtilidades.ItemCombo)
            If Application.Current.Resources.Contains(Me.ListaCombosEsp) Then
                objEstados = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_ESTADOS_ORDEN_LEO"))

                If IsNothing(objEstados) Then
                    _mstrEstadoOrdenLeo = ""
                Else
                    If objEstados.Where(Function(i) i.ID = value).Count > 0 Then
                        _mstrEstadoOrdenLeo = (From obj In objEstados Where obj.ID = value Select obj).First.Descripcion
                    End If
                End If
            Else
                _mstrEstadoOrdenLeo = ""
            End If
            MyBase.CambioItem("EstadoOrdenLeo")
        End Set
    End Property

    'Private _mdblSaldo As Double = 0
    'Public ReadOnly Property SaldoOrden() As Double
    '    Get
    '        Return (_mdblSaldo)
    '    End Get
    'End Property

    Private _SaldoOrden As Double? = 0
    Public Property SaldoOrden As Double?
        Get
            Return _SaldoOrden
        End Get
        Set(ByVal value As Double?)
            _SaldoOrden = value
            MyBase.CambioItem("SaldoOrden")
        End Set
    End Property

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            Dim logIgual As Boolean = False

            'If Not _ComitenteSeleccionado Is Nothing Then
            '    If _ComitenteSeleccionado.Equals(value) Then
            '        logIgual = True
            '    End If
            'Else
            '    Ordenantes = Nothing
            '    CuentasDeposito = Nothing
            'End If

            If Not IsNothing(_ComitenteSeleccionado) AndAlso _ComitenteSeleccionado.Equals(value) Then
                Exit Property
            Else
                Ordenantes = Nothing
                CuentasDeposito = Nothing
            End If

            If logIgual Then
                'buscarOrdenanteSeleccionado()
                'buscarCuentaDepositoSeleccionada()
            Else
                _ComitenteSeleccionado = value

                If Not value Is Nothing Then
                    If mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                        '// Solamente si se está ingresando o actualizando se debe cambiar el código del comitente y buscar los nuevos receptores de la orden
                        _OrdenSelected.IDComitente = _ComitenteSeleccionado.CodigoOYD
                        _OrdenSelected.FormaPago = _ComitenteSeleccionado.CodFormaPago

                        '// Consultar los receptores que se pueden asignar a la orden
                        consultarReceptoresComitente()
                    ElseIf mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Then
                        _OrdenSelected.IDComitente = _ComitenteSeleccionado.CodigoOYD
                        consultarReceptoresComitente()
                    End If

                    '// Actualizar el campo para que se vea el código del comitente seleccionado
                    ComitenteOrden = _ComitenteSeleccionado.IdComitente

                    consultarOrdenantes()
                    consultarCuentasDeposito()

                    If Editando = True Then
                        If Not IsNothing(OrdenSelected) Then
                            _OrdenSelected.ComisionPactada = _ComitenteSeleccionado.FactorComisionCliente
                        End If
                    End If

                    If Me.ClaseOrden = ClasesOrden.C Then
                        '(SLB) Consultar cuentas deposito del Detalle de Pagos
                        consultarCuentasDepositoPago()
                        If Me._OrdenSelected.Tipo = "V" Or Me._OrdenSelected.Tipo = "S" Or (Me._OrdenSelected.Objeto = RF_REPO And Not Me._OrdenSelected.Tipo = "C") Then
                            HabilitarDetalleComisiones = True
                            '(SLB) Consultar el tipo de comisión del Detalle de Comisiones
                            consultarTiposComisionesOrdenes()
                            '(SLB) Consultar las Comisiones de la Orden
                            consultarAdicionalesOrdenes()
                        Else
                            If Not IsNothing(ListaAdicionalesOrdenes) Then
                                ListaAdicionalesOrdenes.Clear()
                            End If
                            HabilitarDetalleComisiones = False
                        End If
                    End If

                    '(SLB) Consultar las Cuentas de los Clientes para las Instrucciones de la Orden
                    consultarCuentasCliente()
                    If mstrAccionOrden = "" Then
                        '(SLB) Consultar las Instrucciones de la Orden
                        consultarInstruccionesOrden()
                    Else
                        '(SLB) Consultar las Instrucciones del Cliente
                        consultarInstruccionesCliente()
                    End If
                End If
                MyBase.CambioItem("ComitenteSeleccionado")
            End If
        End Set
    End Property

    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            Dim logIgual As Boolean = False

            If Not _NemotecnicoSeleccionado Is Nothing Then
                If _NemotecnicoSeleccionado.Equals(value) Then
                    logIgual = True
                End If
            End If

            If Not logIgual Then
                _NemotecnicoSeleccionado = value

                If Not value Is Nothing Then
                    If mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Or mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Then
                        _OrdenSelected.Nemotecnico = _NemotecnicoSeleccionado.Nemotecnico
                    End If

                    '// Actualizar el campo para que se vea el nemotecnico seleccionado
                    NemotecnicoOrden = _NemotecnicoSeleccionado.Nemotecnico
                End If

                MyBase.CambioItem("NemotecnicoSeleccionado")
            End If
        End Set
    End Property

    Private _ReceptorTomaSeleccionado As OYDUtilidades.BuscadorGenerico
    Public Property ReceptorTomaSeleccionado As OYDUtilidades.BuscadorGenerico
        Get
            Return (_ReceptorTomaSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
            _mobjReceptorTomaSeleccionadoAntes = _ReceptorTomaSeleccionado
            _ReceptorTomaSeleccionado = value
            MyBase.CambioItem("ReceptorTomaSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades para presentación del ordenante"

    Private _Ordenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
    Public Property Ordenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
        Get
            Return (_Ordenantes)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorOrdenantes))
            _Ordenantes = value

            If value Is Nothing Then
                OrdenanteSeleccionado = Nothing
            Else
                buscarOrdenanteSeleccionado()
            End If

            MyBase.CambioItem("Ordenantes")
        End Set
    End Property

    Private _mobjOrdenanteSeleccionado As OYDUtilidades.BuscadorOrdenantes
    Public Property OrdenanteSeleccionado() As OYDUtilidades.BuscadorOrdenantes
        Get
            Return (_mobjOrdenanteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorOrdenantes)
            _mobjOrdenanteSeleccionado = value
            If mstrAccionOrden.Equals(MSTR_MC_ACCION_INGRESAR) Or mstrAccionOrden.Equals(MSTR_MC_ACCION_ACTUALIZAR) Then
                If Not _mobjOrdenanteSeleccionado Is Nothing Then
                    _OrdenSelected.IDOrdenante = _mobjOrdenanteSeleccionado.IdOrdenante
                End If
            End If
            MyBase.CambioItem("OrdenanteSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades para presentación de la cuenta depósito"

    Private _CuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property CuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return (_CuentasDeposito)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _CuentasDeposito = value

            If value Is Nothing Then
                CtaDepositoSeleccionada = Nothing
            Else
                buscarCuentaDepositoSeleccionada()
            End If

            MyBase.CambioItem("CuentasDeposito")
        End Set
    End Property

    Private _mobjCtaDepositoSeleccionada As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CtaDepositoSeleccionada() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCtaDepositoSeleccionada)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            If Not IsNothing(_mobjCtaDepositoSeleccionada) AndAlso _mobjCtaDepositoSeleccionada.Equals(value) Then
                Exit Property
            End If

            _mobjCtaDepositoSeleccionada = value

            If mstrAccionOrden.Equals(MSTR_MC_ACCION_INGRESAR) Or mstrAccionOrden.Equals(MSTR_MC_ACCION_ACTUALIZAR) Then
                If Not _mobjCtaDepositoSeleccionada Is Nothing Then
                    _OrdenSelected.UBICACIONTITULO = _mobjCtaDepositoSeleccionada.Deposito
                    _OrdenSelected.CuentaDeposito = _mobjCtaDepositoSeleccionada.NroCuentaDeposito
                    If Editando Then
                        consultarBeneficiariosCliente()
                    End If
                End If
            End If

            MyBase.CambioItem("CtaDepositoSeleccionada")
        End Set
    End Property


    Private _CuentaDepositoDeceval As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property CuentaDepositoDeceval() As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return _CuentaDepositoDeceval
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _CuentaDepositoDeceval = value
            MyBase.CambioItem("CuentaDepositoDeceval")
        End Set
    End Property

    Private _CuentasDepositoPago As List(Of OyDOrdenes.CuentasDepositoPago)
    Public Property CuentasDepositoPago() As List(Of OyDOrdenes.CuentasDepositoPago)
        Get
            Return _CuentasDepositoPago
        End Get
        Set(ByVal value As List(Of OyDOrdenes.CuentasDepositoPago))
            _CuentasDepositoPago = value
            MyBase.CambioItem("CuentasDepositoPago")
        End Set
    End Property

    Private _CuentasDepositoPagoSeleccionada As List(Of OyDOrdenes.CuentasDepositoPago)

    Private _CuentasClientes As List(Of OyDOrdenes.CuentasClientes)
    Public Property CuentasClientes() As List(Of OyDOrdenes.CuentasClientes)
        Get
            Return _CuentasClientes
        End Get
        Set(ByVal value As List(Of OyDOrdenes.CuentasClientes))
            _CuentasClientes = value
            MyBase.CambioItem("CuentasClientes")
        End Set
    End Property

    Private _TiposComisiones As List(Of OyDOrdenes.Comisiones)
    Public Property TiposComisiones() As List(Of OyDOrdenes.Comisiones)
        Get
            Return _TiposComisiones
        End Get
        Set(ByVal value As List(Of OyDOrdenes.Comisiones))
            _TiposComisiones = value
            MyBase.CambioItem("TiposComisiones")
        End Set
    End Property

#End Region

#Region "Propiedades adicionales para control de visualización"

    Private _mlogOpcionActiva As Boolean = False
    ''' <summary>
    ''' Permite habilitar o deshabilitar controles del formulario (IsEnabled, IsReadonly)
    ''' </summary>
    Public ReadOnly Property OpcionActiva() As Boolean
        Get
            'If _mlogOpcionActiva Then
            '    MessageBox.Show("Opcion activa:True")
            'End If
            Return (_mlogOpcionActiva)
        End Get
    End Property

    ''' <summary>
    ''' Permite habilitar o deshabilitar el botón de enviar por SAE (enrutar orden)
    ''' </summary>
    Public ReadOnly Property SAEActivo() As Visibility
        Get
            If Me.mlogSAEActivo And Me.mlogSAEActivoClaseOrden And Me.mlogPuedeEnrutarSAE Then
                Return (Visibility.Visible)
            Else
                Return (Visibility.Collapsed)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Permite habilitar o deshabilitar el botón de enviar por SAE (enrutar orden)
    ''' </summary>
    Public ReadOnly Property IngresoOrdenActivo() As Visibility
        Get
            If mlogUsuarioPuedeIngresar Then
                Return (Visibility.Visible)
            Else
                Return (Visibility.Collapsed)
            End If
        End Get
    End Property


    Private _DuplicarOrdenBoton As Visibility = Visibility.Visible
    Public Property DuplicarOrdenBoton() As Visibility
        Get
            Return _DuplicarOrdenBoton
        End Get
        Set(ByVal value As Visibility)
            _DuplicarOrdenBoton = value
            MyBase.CambioItem("DuplicarOrdenBoton")
        End Set
    End Property

    'Private _UsuarioOperadorCombo As Visibility = Visibility.Collapsed
    'Public Property UsuarioOperadorCombo As Visibility
    '    Get
    '        Return _UsuarioOperadorCombo
    '    End Get
    '    Set(ByVal value As Visibility)
    '        _UsuarioOperadorCombo = value
    '        MyBase.CambioItem("UsuarioOperadorCombo")
    '    End Set
    'End Property

    'Private _UsuarioOperadorText As Visibility = Visibility.Visible
    'Public Property UsuarioOperadorText As Visibility
    '    Get
    '        Return _UsuarioOperadorText
    '    End Get
    '    Set(ByVal value As Visibility)
    '        _UsuarioOperadorText = value
    '        MyBase.CambioItem("UsuarioOperadorText")
    '    End Set
    'End Property

    Private _UsuarioOperador As String
    <Display(Name:="Usuario Operador")>
    Public Property UsuarioOperador As String
        Get
            Return _UsuarioOperador
        End Get
        Set(ByVal value As String)
            _UsuarioOperador = value
            MyBase.CambioItem("UsuarioOperador")
        End Set
    End Property


    Private _TabSeleccionado As Byte = 0
    ''' <summary>
    ''' Propiedad para controlar el tab activo del tab control principal
    ''' </summary>
    Public Property TabSeleccionado As Byte
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Byte)
            _TabSeleccionado = value

            Select Case _TabSeleccionado
                Case 1
                    cargarLiquidaciones()
            End Select

            MyBase.CambioItem("TabSeleccionado")

        End Set
    End Property

    Private _TabSeleccionadoGeneral As Integer = 0
    ''' <summary>
    ''' Propiedad para controlar el tab activo del tab control que contiene los datos generales de la orden
    ''' </summary>
    Public Property TabSeleccionadoGeneral As Integer
        Get
            Return _TabSeleccionadoGeneral
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionadoGeneral = value
            MyBase.CambioItem("TabSeleccionadoGeneral")
        End Set
    End Property

    Private _mstrListaCombosEsp As String = String.Empty
    ''' <summary>
    ''' Nombre de la lista de datos para los combos específicos de órdenes
    ''' </summary>
    Public Property ListaCombosEsp As String
        Get
            Return (_mstrListaCombosEsp)
        End Get
        Set(ByVal value As String)
            _mstrListaCombosEsp = value
        End Set
    End Property

    Private _mstrNombreInicioCombos As String = String.Empty
    ''' <summary>
    ''' Nombre de la lista de datos para los combos específicos de órdenes
    ''' </summary>
    Public Property NombreInicioCombos As String
        Get
            Return (_mstrNombreInicioCombos)
        End Get
        Set(ByVal value As String)
            _mstrNombreInicioCombos = value
        End Set
    End Property

    ''' <summary>
    ''' Permite controlar si los botones para nueva orden de compra y nueva orden de venta están o no activos
    ''' </summary>
    Private _ActivarSeleccionTipoOrden As Boolean = False
    Public Property ActivarSeleccionTipoOrden As Boolean
        Get
            Return (_ActivarSeleccionTipoOrden)
        End Get
        Set(ByVal value As Boolean)
            _ActivarSeleccionTipoOrden = value
            MyBase.CambioItem("ActivarSeleccionTipoOrden")
        End Set
    End Property

    ''' <summary>
    ''' Permite controlar si el botón para duplicar una orden está o no activo
    ''' </summary>
    Private _ActivarDuplicarOrden As Boolean = False
    Public Property ActivarDuplicarOrden As Boolean
        Get
            Return (_ActivarDuplicarOrden)
        End Get
        Set(ByVal value As Boolean)
            _ActivarDuplicarOrden = value
            MyBase.CambioItem("ActivarDuplicarOrden")
        End Set
    End Property

    ''' <summary>
    ''' Permite controlar si el botón para envío de una orden a través de SAE está o no activo
    ''' </summary>
    Private _ActivarEnvioSAE As Boolean = False
    Public Property ActivarEnvioSAE As Boolean
        Get
            Return (_ActivarEnvioSAE)
        End Get
        Set(ByVal value As Boolean)
            _ActivarEnvioSAE = value
            MyBase.CambioItem("ActivarEnvioSAE")
        End Set
    End Property

    ''' <summary>
    ''' Permite controlar si el botón para envío de una orden a través de SAE está o no activo
    ''' </summary>
    Private _ActivarModInstruccion As Boolean = False
    Public Property ActivarModInstruccion As Boolean
        Get
            Return (_ActivarModInstruccion)
        End Get
        Set(ByVal value As Boolean)
            _ActivarModInstruccion = value
            MyBase.CambioItem("ActivarModInstruccion")
        End Set
    End Property

    Private _habilitarFechaElaboracion As Boolean = False
    Public Property habilitarFechaElaboracion() As Boolean
        Get
            Return _habilitarFechaElaboracion
        End Get
        Set(ByVal value As Boolean)
            _habilitarFechaElaboracion = value
            MyBase.CambioItem("habilitarFechaElaboracion")
        End Set
    End Property

    Private _EditandoRegistro As Boolean = False
    ''' <summary>
    ''' Se crea esta variable para el registro en Libro de Órdenes.
    ''' </summary>
    ''' <remarks>SLB20130801</remarks>
    Public Property EditandoRegistro As Boolean
        Get
            Return _EditandoRegistro
        End Get
        Set(ByVal value As Boolean)
            _EditandoRegistro = value
            MyBase.CambioItem("EditandoRegistro")
        End Set
    End Property

    Private _HabilitarExento As Boolean = False
    Public Property HabilitarExento As Boolean
        Get
            Return _HabilitarExento
        End Get
        Set(ByVal value As Boolean)
            _HabilitarExento = value
            MyBase.CambioItem("HabilitarExento")
        End Set
    End Property

    Private _VisibilidadExento As Visibility = Visibility.Collapsed
    Public Property VisibilidadExento As Visibility
        Get
            Return _VisibilidadExento
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadExento = value
            MyBase.CambioItem("VisibilidadExento")
        End Set
    End Property

    Private _VisibilidadCampoExento As Visibility = Visibility.Collapsed
    Public Property VisibilidadCampoExento As Visibility
        Get
            Return _VisibilidadCampoExento
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadCampoExento = value
            MyBase.CambioItem("VisibilidadCampoExento")
        End Set
    End Property

    Private _VisibilidadOfertaPublica As Visibility = Visibility.Collapsed
    Public Property VisibilidadOfertaPublica As Visibility
        Get
            Return _VisibilidadOfertaPublica
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadOfertaPublica = value
            MyBase.CambioItem("VisibilidadOfertaPublica")
        End Set
    End Property


    ''' <summary>
    ''' Pone en modo de solo lectura los detalles de la orden.
    ''' </summary>
    ''' <remarks></remarks>
    Private _EditandoDetalle As Boolean = True
    Public Property EditandoDetalle() As Boolean
        Get
            Return _EditandoDetalle
        End Get
        Set(ByVal value As Boolean)
            _EditandoDetalle = value
            MyBase.CambioItem("EditandoDetalle")
        End Set
    End Property

    ''' <summary>
    ''' Pone en modo de solo lectura los detalles de la orden.
    ''' </summary>
    ''' <remarks></remarks>
    Private _EditandoInstrucciones As Boolean = False
    Public Property EditandoInstrucciones() As Boolean
        Get
            Return _EditandoInstrucciones
        End Get
        Set(ByVal value As Boolean)
            _EditandoInstrucciones = value
            MyBase.CambioItem("EditandoInstrucciones")
        End Set
    End Property

    ''' <summary>
    ''' Pone en modo de solo lectura los detalles de la orden.
    ''' </summary>
    ''' <remarks></remarks>
    Private _HabilitarDetalleComisiones As Boolean = True
    Public Property HabilitarDetalleComisiones() As Boolean
        Get
            Return _HabilitarDetalleComisiones
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDetalleComisiones = value
            MyBase.CambioItem("HabilitarDetalleComisiones")
        End Set
    End Property

    ''' <summary>
    ''' Permite habilitar o deshabilitar el campo Tipo Transaccion
    ''' </summary>
    Public ReadOnly Property TipoTransaccionActivo() As Visibility
        Get
            If Me.mlogAplicaTipoTransaccion Then
                Return (Visibility.Visible)
            Else
                Return (Visibility.Collapsed)
            End If
        End Get
    End Property

    Private _MostrarTabInstrucciones As Visibility = Visibility.Collapsed
    ''' <summary>
    ''' Permite habilitar o deshabilitar el tab de Instrucciones tan en Acciones como en Renta Fija
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>SLB20130930</remarks>
    Public Property MostrarTabInstrucciones As Visibility
        Get
            Return _MostrarTabInstrucciones
        End Get
        Set(ByVal value As Visibility)
            _MostrarTabInstrucciones = value
            MyBase.CambioItem("MostrarTabInstrucciones")
        End Set
    End Property

    Private _HabilitarSeleccionISIN As Boolean = False
    ''' <summary>
    ''' Permite seleccionar el ISIN en el buscador de especies, si lo permite debe llenar las faciales de la especie.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>SLB20131101</remarks>
    Public Property HabilitarSeleccionISIN As Boolean
        Get
            Return _HabilitarSeleccionISIN
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionISIN = value
            MyBase.CambioItem("HabilitarSeleccionISIN")
        End Set
    End Property

    Private _SeleccionarUnISIN As Boolean = False
    ''' <summary>
    ''' Indica que cuando la propiedad HabilitarConsultaISIN sea = false, se seleccione el ISIN si la especie tiene solo 1 relacionado.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>NAOC20151703</remarks>
    Public Property SeleccionarUnISIN As Boolean
        Get
            Return _SeleccionarUnISIN
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarUnISIN = value
            MyBase.CambioItem("SeleccionarUnISIN")
        End Set
    End Property

    Private _PermitirSeleccionarEspecieOIsin As Boolean = False
    ''' <summary>
    ''' Indica que cuando la propiedad HabilitarConsultaISIN sea = true, se permita seleccionar especie o isin en el buscador.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>NAOC20151703</remarks>
    Public Property PermitirSeleccionarEspecieOIsin As Boolean
        Get
            Return _PermitirSeleccionarEspecieOIsin
        End Get
        Set(ByVal value As Boolean)
            _PermitirSeleccionarEspecieOIsin = value
            MyBase.CambioItem("PermitirSeleccionarEspecieOIsin")
        End Set
    End Property

#End Region

#Region "Propiedades para manejo de horas"

    Private _HoraFinVigencia As Date = Now()
    ''' <summary>
    ''' Esta propiedad hace binding con la propiedad Hora fin de vigencia de la orden cuando su duración es a hora
    ''' </summary>
    Public Property HoraFinVigencia As Date
        Get
            Return (_HoraFinVigencia)
        End Get
        Set(ByVal value As Date)
            _HoraFinVigencia = value
            If Editando Then
                If Not DatePart(DateInterval.Year, _HoraFinVigencia.Date) = 1 Then
                    Me._OrdenSelected.VigenciaHasta = _HoraFinVigencia.Date
                End If
            End If
            MyBase.CambioItem("HoraFinVigencia")
        End Set
    End Property

    ''' <summary>
    ''' Convierte el valor de la propiedad HoraFinVigencia en string para enviar a la base de datos o el valor de la base de datos en una fecha para mostrar. Esto según
    ''' el valor del parámetro plogMostrar.
    ''' </summary>
    ''' <param name="pstrHoraMinutos">Valor que viene de la base de datos (hora y minutos)</param>
    ''' <param name="plogMostrar">Si es True convierte el valor de entrada en un tipo fecha válido para mostrarla y si es falso extrae la hora para actualizar el campo</param>
    ''' <remarks></remarks>
    Private Function asignarHoraVigencia(ByVal pstrHoraMinutos As String, ByVal plogMostrar As Boolean)
        Dim intSepHora As Integer = -1
        Dim intSepMin As Integer = -1
        Dim strHora As String = String.Empty
        Dim strMinutos As String = String.Empty
        Dim strSegundos As String = String.Empty

        If plogMostrar Then
            If pstrHoraMinutos Is Nothing Then
                _HoraFinVigencia = Nothing
            Else
                intSepHora = pstrHoraMinutos.IndexOf(":")
                intSepMin = pstrHoraMinutos.LastIndexOf(":")
                If intSepHora > 0 Then
                    strHora = Left(pstrHoraMinutos, intSepHora - 1)
                Else
                    strHora = "0"
                End If

                If intSepMin > intSepHora + 1 Then
                    strMinutos = Mid(pstrHoraMinutos, intSepHora + 1, intSepMin - intSepHora)
                Else
                    strMinutos = "0"
                End If

                If intSepMin > 1 Then
                    strSegundos = Right(pstrHoraMinutos, Len(pstrHoraMinutos) - intSepMin + 1) ' Se suma 1 porque las posiciones se toman desde cero
                Else
                    strSegundos = "0"
                End If

                If Versioned.IsNumeric(strHora) And Versioned.IsNumeric(strMinutos) And Versioned.IsNumeric(strSegundos) Then
                    _HoraFinVigencia = New DateTime(Year(Now), Month(Now), Day(Now), CInt(strHora), CInt(strMinutos), CInt(strSegundos))
                Else
                    _HoraFinVigencia = New DateTime(Year(Now), Month(Now), Day(Now), 0, 0, 0)
                End If
            End If
        ElseIf plogMostrar = False Then
            pstrHoraMinutos = Right("00" & Hour(_HoraFinVigencia).ToString, 2) & ":" & Right("00" & Minute(_HoraFinVigencia).ToString(), 2) & ":" & Right("00" & Second(_HoraFinVigencia).ToString(), 2)
        End If

        Return (pstrHoraMinutos)
    End Function
#End Region

#Region "Propiedades Maker and checker"

#Region "Propiedades generales que no cambian por registro"

    ''' <summary>
    ''' Propiedad que define si se maneja o no maker and checker. Es necesaria para activar los botones de Aprobar, Rechazar y Ver versión
    ''' </summary>
    ''' <remarks></remarks>
    Private _mlogManejaMakerAndChecker As Boolean = False
    Public ReadOnly Property ManejaMakerAndChecker As Boolean
        Get
            Return (_mlogManejaMakerAndChecker)
        End Get
    End Property

    ' La propiedad se hace de solo lectura para que no pueda ser manipulada desde fuera del view model
    Private Sub cambiarManejaMakerAndChecker(ByVal plogManejaMakerAndChecker As Boolean)
        If Not _mlogManejaMakerAndChecker.Equals(plogManejaMakerAndChecker) Then
            _mlogManejaMakerAndChecker = plogManejaMakerAndChecker
            MyBase.CambioItem("ManejaMakerAndChecker")
        End If
    End Sub

    ''' <summary>
    ''' Se ocultan los controles que no aplican cuando no se utiliza el Maker and Checker
    ''' </summary>
    ''' <param name="plogManejaMakerAndChecker"></param>
    ''' <remarks>SLB20120704</remarks>
    Private Sub mostrarCamposMakerAndChecker(ByVal plogManejaMakerAndChecker As Boolean)
        If Not plogManejaMakerAndChecker Then
            cb.VisibleMakerAndCheker = Visibility.Collapsed
        End If
    End Sub

#End Region

#Region "Propiedades que cambian por registro"

    ''' <summary>
    ''' Propiedad que define el texto del botón que permite intercambiar desde la versión aprobada del registro a la no aprobada
    ''' </summary>
    ''' <remarks></remarks>
    Private _mstrTextoCambioARegistroOriginal As String = MSTR_MC_BOTON_TEXTO_VERSION_APROBADA
    Public ReadOnly Property TextoCambioARegistroOriginal As String
        Get
            Return (_mstrTextoCambioARegistroOriginal)
        End Get
    End Property

    Private Sub cambiarTextoCambioARegistroOriginal(ByVal pstrTextoCambioARegistroOriginal As String)
        If Not _mstrTextoCambioARegistroOriginal.Equals(pstrTextoCambioARegistroOriginal) AndAlso Not pstrTextoCambioARegistroOriginal.Equals(String.Empty) Then
            _mstrTextoCambioARegistroOriginal = pstrTextoCambioARegistroOriginal
            MyBase.CambioItem("TextoCambioARegistroOriginal")
        End If
    End Sub

    ''' <summary>
    ''' Propiedad que define el texto del botón que permite intercambiar desde la versión no aprobada del registro a la aprobada
    ''' </summary>
    ''' <remarks></remarks>
    Private _mstrTextoCambioARegistroModificado As String = MSTR_MC_BOTON_TEXTO_VERSION_POR_APROBAR
    Public ReadOnly Property TextoCambioARegistroModificado As String
        Get
            Return (_mstrTextoCambioARegistroModificado)
        End Get
    End Property

    Private Sub cambiarTextoCambioARegistroModificado(ByVal pstrTextoCambioARegistroModificado As String)
        If Not _mstrTextoCambioARegistroModificado.Equals(pstrTextoCambioARegistroModificado) AndAlso Not pstrTextoCambioARegistroModificado.Equals(String.Empty) Then
            _mstrTextoCambioARegistroModificado = pstrTextoCambioARegistroModificado
            MyBase.CambioItem("TextoCambioARegistroModificado")
        End If
    End Sub

    ''' <summary>
    ''' Propiedad que define si activan o no los botones de aprobar y rechazar para el registro seleccionado. Cuando el registro
    ''' seleccionado es el que está pendiente de aprobar, es decir, es el que tiene los cambios, los botones de aprobar y rechazar
    ''' se deben activar pero si el registro activo es el orginal (sin los cambios) estos botones no se deben activar.
    ''' </summary>
    ''' <remarks></remarks>
    Private _mlogRegistroActivoPorAprobar As Boolean = False
    Public ReadOnly Property RegistroActivoPorAprobar As Boolean
        Get
            Return (_mlogRegistroActivoPorAprobar)
        End Get
    End Property

    ' La propiedad se hace de solo lectura para que no pueda ser manipulada desde fuera del view model
    Private Sub cambiarRegistroActivoPorAprobar(ByVal pstrEstadoMakerChecker As String, ByVal pstrAccionMakerCheker As String, ByVal plogTieneOrdenRelacionada As Boolean)
        Dim logRegistroActivoPorAprobar As Boolean = False '// Activar o no botones de aprobar y rechazar
        Dim logRegistroTieneCambios As Boolean = False '// Ver o no el botón para cambiar entre versiones (aprobda y pendiente)
        Dim logVerDescripcionMakerChecker As Boolean = False '// Ver o no el texto en el formulario que muestra el estado del maker and checker
        'Dim strTextoBotonVersionAprobada As String = String.Empty '// Texto del botón para cambiar entre versiones aprobada y pendiente

        '// Cuando no se tiene estado definido (es nothing) se asume que el documento está aprobado y no tiene versione por aprobar. 
        '// No se activan los botones de aprobar y rechazar ni ver versión. Tampoco se muestra el estado del maker and checker
        If Not IsNothing(pstrEstadoMakerChecker) Then
            '// Cuando se tiene estado definido se debe evaluar que hacer.

            If pstrEstadoMakerChecker.ToUpper.Equals(EstadoMakerChecker.PA.ToString()) Then
                '// Cuando los datos están en proceso de aprobación el registro está en estado pendiente. OJO: Solamente se considera este estado, los ya aprobados y rechazados se deben descartar en la consulta.
                If pstrAccionMakerCheker.ToUpper().Equals(MSTR_MC_ACCION_INGRESAR) Then
                    '// Cuando la acción es ingrerso no se muestra el botón de ver versión aprobada porque no existe una versión previa
                    logRegistroTieneCambios = False
                Else
                    '// Cuando la acción NO es ingrerso se muestra el botón de ver versión aprobada porque debe existir una versión previa sobre la cual se hicieron las modificaciones
                    logRegistroTieneCambios = True
                End If

                logRegistroActivoPorAprobar = True '// Se deben ver los botones de aprobar y rechazar porque el registro esta por aprobar
                logVerDescripcionMakerChecker = True '// Ver el texto en el formulario que muestra el estado del maker and checker
            ElseIf plogTieneOrdenRelacionada And (pstrEstadoMakerChecker.ToUpper.Equals(EstadoMakerChecker.A.ToString()) Or pstrEstadoMakerChecker.ToUpper.Equals(String.Empty)) Then
                '// Cuando se tiene una orden relacionada (versión pendiente de aprobar) y no es la versión pendiente de aprobar significa que está en la versión aprobada pero
                ' que tiene una versión por aprobar.
                logRegistroActivoPorAprobar = False
                logRegistroTieneCambios = True
                'logVerDescripcionMakerChecker = True '// Ver el texto en el formulario que muestra el estado del maker and checker
            ElseIf pstrEstadoMakerChecker.ToUpper.Equals(EstadoMakerChecker.NA.ToString()) Then
                '// Cuando la orden ha sido rechazada no se deba activar ninguna opción y no se debe dejar modificar
                logVerDescripcionMakerChecker = True '// Ver el texto en el formulario que muestra el estado del maker and checker
            Else
                '// Cuando se tiene un estado diferente se asume que el registro está aprobado y no tiene versiones por aprobar. 
                '// No se activan los botones de aprobar y rechazar ni de ver versión. Tampoco se muestra el estado del maker and checker.
                logRegistroActivoPorAprobar = False
            End If
        End If

        If Not _mlogRegistroActivoPorAprobar.Equals(logRegistroActivoPorAprobar) Then
            _mlogRegistroActivoPorAprobar = logRegistroActivoPorAprobar
            MyBase.CambioItem("RegistroActivoPorAprobar")
        End If

        cambiarRegistroTieneCambios(logRegistroTieneCambios)
        cambiarVerDescripcionMakerChecker(logVerDescripcionMakerChecker)
    End Sub

    ''' <summary>
    ''' Propiedad que define si activa o no el botón para ver la versión aprobada o no aprobada del registro activos cuando 
    ''' este tiene una versión no aprobada.  Cuando el registro seleccionado es el que está pendiente de aprobar, 
    ''' es decir, es el que tiene los cambios, o el registro activo es la versión orginal del modificado (sin los cambios) 
    ''' este botón se deben activar.
    ''' 
    ''' IMPORTANTE: En el caso de un nuevo registro, éste queda por aprobar pero no tiene versión aprobada y 
    '''             por lo tanto el botón nodebe verse.
    ''' </summary>
    ''' <remarks></remarks>
    Private _mlogRegistroTieneCambios As Boolean = False
    Public ReadOnly Property RegistroTieneCambios As Boolean
        Get
            Return (_mlogRegistroTieneCambios)
        End Get
    End Property

    Private Sub cambiarRegistroTieneCambios(ByVal plogVerBotonVersionAprobada As Boolean)
        If Not _mlogRegistroTieneCambios.Equals(plogVerBotonVersionAprobada) Then
            _mlogRegistroTieneCambios = plogVerBotonVersionAprobada
            MyBase.CambioItem("RegistroTieneCambios")
        End If
    End Sub

    ''' <summary>
    ''' Texto que muestra el estado del registro activo según la funcionalidad del Maker and Checker.
    ''' </summary>
    ''' <remarks></remarks>
    Private _mintVerDescMakerChecker As System.Windows.Visibility = Visibility.Collapsed
    Public ReadOnly Property VerDescripcionMakerChecker As System.Windows.Visibility
        Get
            Return (_mintVerDescMakerChecker)
        End Get
    End Property

    Private Sub cambiarVerDescripcionMakerChecker(ByVal plogVerDescMakerChecker As Boolean)
        If plogVerDescMakerChecker Then
            _mintVerDescMakerChecker = Visibility.Visible
        Else
            _mintVerDescMakerChecker = Visibility.Collapsed
        End If
        MyBase.CambioItem("VerDescripcionMakerChecker")
    End Sub

#End Region

#End Region

#Region "Propiedades ordenes"

    Private _objTipoId As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property objTipoId() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _objTipoId
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _objTipoId = value
            MyBase.CambioItem("objTipoId")
        End Set
    End Property

    Private _EditandoComitente As Boolean = False
    Public Property EditandoComitente As Boolean
        Get

            Return _EditandoComitente
        End Get
        Set(ByVal value As Boolean)
            _EditandoComitente = value
            MyBase.CambioItem("EditandoComitente")
        End Set
    End Property

    Private _HabilitarMercado As Boolean = False
    Public Property HabilitarMercado() As Boolean
        Get
            Return _HabilitarMercado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarMercado = value
            MyBase.CambioItem("HabilitarMercado")
        End Set
    End Property

    Private _MostrarValorFuturoRepo As Visibility = Visibility.Collapsed
    Public Property MostrarValorFuturoRepo As Visibility
        Get
            Return _MostrarValorFuturoRepo
        End Get
        Set(ByVal value As Visibility)
            _MostrarValorFuturoRepo = value
            MyBase.CambioItem("MostrarValorFuturoRepo")
        End Set
    End Property

    Private _IsBusyLiquidaciones As Boolean
    Public Property IsBusyLiquidaciones() As Boolean
        Get
            Return _IsBusyLiquidaciones
        End Get
        Set(ByVal value As Boolean)
            _IsBusyLiquidaciones = value
            MyBase.CambioItem("IsBusyLiquidaciones")
        End Set
    End Property

    Private _ListaOrdenes As EntitySet(Of OyDOrdenes.Orden)
    Public Property ListaOrdenes() As EntitySet(Of OyDOrdenes.Orden)
        Get
            Return _ListaOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.Orden))
            _ListaOrdenes = value
            MyBase.CambioItem("ListaOrdenes")
            MyBase.CambioItem("ListaOrdenesPaged")
        End Set
    End Property

    Public _ListaTrazabilidadOrdenes As EntitySet(Of OyDOrdenes.Trazabilidad)
    Public Property ListaTrazabilidadOrdenes() As EntitySet(Of OyDOrdenes.Trazabilidad)
        Get
            Return _ListaTrazabilidadOrdenes
        End Get
        Set(value As EntitySet(Of OyDOrdenes.Trazabilidad))
            _ListaTrazabilidadOrdenes = value
            MyBase.CambioItem("ListaTrazabilidadOrdenes")
            MyBase.CambioItem("ListaTrazabilidadOrdenesPaged")

        End Set
    End Property

    Public ReadOnly Property ListaTrazabilidadOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTrazabilidadOrdenes) Then
                Dim view = New PagedCollectionView(_ListaTrazabilidadOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property


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

    Private WithEvents _OrdenSelected As OyDOrdenes.Orden
    Public Property OrdenSelected() As OyDOrdenes.Orden
        Get
            Return _OrdenSelected
        End Get
        Set(ByVal value As OyDOrdenes.Orden)
            Try
                If Not IsNothing(_OrdenSelected) AndAlso _OrdenSelected.Equals(value) Then
                    Exit Property
                End If

                _OrdenSelected = value
                leerParametros()
                If Not IsNothing(_OrdenSelected) Then
                    If Not IsNothing(_OrdenSelected.Nemotecnico) Then
                        consultarClaseEspecie(OrdenSelected.Nemotecnico)
                    End If

                    If ActivoExento = "SI" Then
                        If OrdenSelected.ExentoRetencion = True Or OrdenSelected.ExentoRetencion = False Then
                            If mlogDuplicarOrden = False Then
                                HabilitarExento = False
                            Else
                                HabilitarExento = True
                            End If
                            If EsBono = True Then
                                VisibilidadCampoExento = Visibility.Visible
                            Else
                                VisibilidadCampoExento = Visibility.Collapsed
                            End If

                        Else
                            HabilitarExento = False
                            VisibilidadCampoExento = Visibility.Collapsed
                        End If
                    Else
                        HabilitarExento = False
                        VisibilidadCampoExento = Visibility.Collapsed
                    End If
                End If



                _mlogOpcionActiva = False
                If Not value Is Nothing Then
                    If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = String.Empty Then
                        'Me.buscarComitente("buscar")
                        Me.buscarComitente(, "buscar")

                        _mobjComitenteSeleccionadoAntes = _ComitenteSeleccionado

                        Me.buscarNemotecnico()
                        _mobjNemotecnicoSeleccionadoAntes = _NemotecnicoSeleccionado

                        Me.buscarItem("receptores")

                        '-- Actualizar el estado del maker and checker para la orden
                        If Me.ManejaMakerAndChecker Then
                            cambiarRegistroActivoPorAprobar(Me._OrdenSelected.EstadoMakerChecker, Me._OrdenSelected.AccionMakerAndChecker, Me._OrdenSelected.TieneOrdenRelacionada)
                        End If

                        '// Consultar los beneficiarios de la cuenta depósito asignada a la orden
                        consultarBeneficiariosOrden()
                        '// Consultar los receptores asignados a la orden
                        consultarReceptoresOrden(_OrdenSelected.Clase, _OrdenSelected.Tipo, _OrdenSelected.NroOrden, _OrdenSelected.Version, String.Empty)

                        '// Consultar órden SAE
                        consultarOrdenSAE()

                        '(SLB) Consultar Liquidaciones Probables
                        consultarLiqProbablesOrden()

                        '(SLB) Consultar Pagos de Ordenes
                        If Me.ClaseOrden = ClasesOrden.C Then
                            consultarPagosOrden()
                        End If

                        ValidarUsuarioOperador()
                        '// Calcular días vigencia
                        mlogRecalcularFechas = False 'CCM20120305
                        Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)

                        ConsultarSaldoOrden()

                        'Jorge Andres Bedoya 20150406
                        If Me.ClaseOrden = ClasesOrden.A Then
                            If Me._OrdenSelected.Objeto = "AD" Then
                                Me.MostrarComitenteADR = Visibility.Visible
                            Else
                                Me.MostrarComitenteADR = Visibility.Collapsed
                            End If

                            If Me._OrdenSelected.Objeto = "OPA" Then
                                Me.VisibilidadOfertaPublica = Visibility.Visible
                            Else
                                Me.VisibilidadOfertaPublica = Visibility.Collapsed
                            End If

                        End If


                    Else
                        If mlogDuplicarOrden = False Then
                            '-- Se está ingresando una nueva orden pero no por la opción de duplicar
                            dcProxy.BeneficiariosOrdens.Clear()
                            dcProxy1.ReceptoresOrdens.Clear()

                            'SLB se adiciona el borrado de las liquidaciones probables
                            dcProxy1.LiqAsociadasOrdens.Clear()
                            ListaLiqAsociadasOrdenes = dcProxy1.LiqAsociadasOrdens
                            'SLB se adiciona el borrado de las Instrucciones de la Orden
                            If Not IsNothing(ListaInstruccionesOrdenes) Then
                                ListaInstruccionesOrdenes.Clear()
                            End If

                            If Me.ClaseOrden = ClasesOrden.C Then
                                If Not IsNothing(CuentaDepositoDeceval) Then
                                    'CFMA20180815 instanciamos la propiedad para que al momento de ingresar un registro nuevo se limpie la pestaña 
                                    'CuentaDepositoDeceval.Clear()
                                    Dim listaDepositonew As List(Of OYDUtilidades.BuscadorCuentasDeposito) = Nothing
                                    CuentaDepositoDeceval = listaDepositonew
                                    'CFMA20180815
                                End If
                                If Not IsNothing(ListaAdicionalesOrdenes) Then
                                    ListaAdicionalesOrdenes.Clear()
                                End If
                                'If IsNothing(OrdenesPagoSelected) Then
                                dcProxy1.OrdenesPagos.Clear()
                                ListaOrdenesPagos = dcProxy1.OrdenesPagos
                                NombreColeccionDetalle = "cmPagosOrdenes"
                                NuevoRegistroDetalle()
                                'End If
                            End If

                            _mlogOpcionActiva = True
                        End If
                    End If

                    AccionInstrucciones()

                    '-- Propiedades que tienen el estado de la orden
                    Me.EstadoOrden = _OrdenSelected.Estado
                    Me.EstadoOrdenLeo = _OrdenSelected.EstadoLEO

                    'SLB20130614 Manejo del combo de duración
                    Select Case Me._OrdenSelected.Duracion
                        Case "F" 'HASTA FECHA
                            HoraFinVigencia = Me._OrdenSelected.VigenciaHasta
                            VisibleDuracionFecha = Visibility.Visible
                            VisibleDuracionHora = Visibility.Collapsed
                        Case "A" ' HASTA HORA
                            VisibleDuracionFecha = Visibility.Collapsed
                            VisibleDuracionHora = Visibility.Visible
                            HoraFinVigencia = Me._OrdenSelected.HoraVigencia
                        Case Else
                            VisibleDuracionFecha = Visibility.Collapsed
                            VisibleDuracionHora = Visibility.Collapsed
                            asignarHoraVigencia(_OrdenSelected.HoraVigencia, True)
                    End Select


                    If Me.ClaseOrden = ClasesOrden.C Then

                        If logValorCompromisoFuturoRequerido = False Then
                            If _OrdenSelected.Tipo = TiposOrden.V.ToString() And (Me._OrdenSelected.Repo Or Me._OrdenSelected.Objeto = RF_Simulatena_Salida Or Me._OrdenSelected.Objeto = RF_Simultanea_Salida_CRCC) Then
                                MostrarValorFuturoRepo = Visibility.Visible
                            Else
                                MostrarValorFuturoRepo = Visibility.Collapsed
                            End If
                        Else
                            'Jorge Andres Bedoya 2014/12/29
                            Select Case Me.OrdenSelected.Objeto
                                Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO, RF_Simultanea_Salida_CRCC, RF_Simultanea_Regreso_CRCC
                                    MostrarValorFuturoRepo = Visibility.Visible
                                Case Else
                                    MostrarValorFuturoRepo = Visibility.Collapsed
                            End Select
                        End If

                    End If

                    'SLB20140512 Cuando el tab seleccionado es Liquidaciones debe recargar la información.
                    If _TabSeleccionado = 1 Then
                        cargarLiquidaciones()
                    End If
                    'TabSeleccionado = 0
                    'TabSeleccionadoGeneral = 0
                    'asignarHoraVigencia(_OrdenSelected.HoraVigencia, True)

                    consultarTrazabilidadOrden(_OrdenSelected.NroOrden, _OrdenSelected.Clase, _OrdenSelected.Tipo)


                End If

            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Falló la selección de la orden actual", Me.ToString, "OrdenSelected", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioNegocio)
            End Try

            MyBase.CambioItem("OrdenSelected")
        End Set
    End Property

    Private _VisibleDuracionFecha As Visibility = Visibility.Collapsed
    Public Property VisibleDuracionFecha As Visibility
        Get
            Return _VisibleDuracionFecha
        End Get
        Set(ByVal value As Visibility)
            _VisibleDuracionFecha = value
            MyBase.CambioItem("VisibleDuracionFecha")
        End Set
    End Property

    Private _VisibleDuracionHora As Visibility = Visibility.Collapsed
    Public Property VisibleDuracionHora As Visibility
        Get
            Return _VisibleDuracionHora
        End Get
        Set(ByVal value As Visibility)
            _VisibleDuracionHora = value
            MyBase.CambioItem("VisibleDuracionHora")
        End Set
    End Property

    'Jorge Andres Bedoya 20150406
    Private _IsEnableComitenteADR As Boolean
    Public Property IsEnableComitenteADR() As Boolean
        Get
            Return _IsEnableComitenteADR
        End Get
        Set(ByVal value As Boolean)
            _IsEnableComitenteADR = value
            MyBase.CambioItem("IsEnableComitenteADR")
        End Set
    End Property

    'Jorge Andres Bedoya 20150406
    Private _MostrarComitenteADR As Visibility = Visibility.Collapsed
    Public Property MostrarComitenteADR As Visibility
        Get
            Return _MostrarComitenteADR
        End Get
        Set(ByVal value As Visibility)
            _MostrarComitenteADR = value
            MyBase.CambioItem("MostrarComitenteADR")
        End Set
    End Property

#End Region

#Region "Resultados Asincrónicos"

    Private Sub MensajeDatosLeo(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                OrdenSelected.FechaRecepcion = Date.Now
            End If



        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al al generar mensaje de datos Leo", Me.ToString(), "MensajeDatosLeo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' SLB - Este metodo arroja el resultado si la fecha de orden es valida, si se esta Guardando invoca el Metodo calcularDiasOrden.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoVerificarFechaValida(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarDiaHabil))
        Try
            Dim objValidarDiaHabil As OyDOrdenes.ValidarDiaHabil
            If Not lo.HasError Then
                objValidarDiaHabil = dcProxy.ValidarDiaHabils.First
                If Not objValidarDiaHabil.EsDiaHabil Then
                    MostrarMensajeFechaNoValida(objValidarDiaHabil.MenorFechaHabilOrden)
                ElseIf lo.UserState.ToString.Equals("GuardarOrden") Then
                    calcularDiasOrden(MSTR_CALCULAR_DIAS_ORDEN, -1, True)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al completar la validación de los días de vigencia para guardar la orden", Me.ToString(), "calcularDiasHabilesGuardarCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub MostrarMensajeFechaNoValida(ByVal pdtmFechaOrden As Date)
        A2Utilidades.Mensajes.mostrarMensaje("La Fecha de Elebaroción de la orden (" + _OrdenSelected.FechaOrden.Value.Date.ToLongDateString + ") es un día no hábil", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        _OrdenSelected.FechaOrden = pdtmFechaOrden
        IsBusy = False
    End Sub

    ''' <summary>
    ''' SLB - Verificar de la fecha de elaboración de la Orden es Valida, invoca el SP uspOyDNet_Ordenes_CalcularDiasHabiles
    ''' </summary>
    ''' <param name="plogGuardarOrden"></param>
    ''' <remarks>SLB20120704</remarks>
    Public Sub VerificarFechaValida(Optional ByVal plogGuardarOrden As Boolean = False)
        Try
            If IsNothing(_OrdenSelected.FechaOrden) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de ingresar la fecha de elaboración de la orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _OrdenSelected.FechaOrden.Value.Date > Now.Date Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de elaboración de la orden debe ser menor o igual a la fecha actual ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                OrdenSelected.FechaOrden = Now
                IsBusy = False
                Exit Sub
            End If

            Dim Accion As String = String.Empty
            If plogGuardarOrden Then
                Accion = "GuardarOrden"
            End If
            dcProxy.ValidarDiaHabils.Clear()
            dcProxy.Load(dcProxy.ValidarDiaHabilQuery(Me._OrdenSelected.FechaOrden.Value.Date, Program.Usuario, Program.HashConexion), AddressOf TerminoVerificarFechaValida, Accion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la fecha de elaboracion de la orden", Me.ToString(), "VerificarFechaValida", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub calcularFechaRecepcion(Optional ByVal plogGuardarOrden As Boolean = False)
        Try
            'SLB Manejo de la fecha de recepción
            'If e.PropertyName.ToLower.Equals("fecharecepcion") Then

            'If mlogRecalcularFechaRecepcion = False Then
            '    mlogRecalcularFechaRecepcion = True
            '    Exit Sub
            'End If

            'mlogRecalcularFechaRecepcion = False
            If IsNothing(_OrdenSelected.FechaRecepcion) Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione una fecha de toma", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                _OrdenSelected.FechaRecepcion = CType(_OrdenSelected.FechaRecepcion, Date).AddHours(_OrdenSelected.FechaSistema.Hour).AddMinutes(_OrdenSelected.FechaSistema.Minute).AddSeconds(_OrdenSelected.FechaSistema.Second)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la fecha de recepción de la orden", Me.ToString(), "calcularFechaRecepcion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento se ejecuta cuando el usuario va a guardar la orden y se lanza la validación de la fecha de la orden (elaboración) y de vigencia de la orden, además 
    ''' la fecha de emisión y vencimiento del título
    ''' </summary>
    Private Sub calcularDiasHabilesGuardarCompleted(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarFecha))
        Try
            If calcularDiasHabilesValidar(lo) Then
                validarOrden()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al completar la validación de los días de vigencia para guardar la orden", Me.ToString(), "calcularDiasHabilesGuardarCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento se ejecuta cuando se lanza la validación de la fecha de la orden (elaboración) y de vigencia de la orden, además 
    ''' la fecha de emisión y vencimiento del título, pero siempre y cuando no se esté guardando la orden
    ''' </summary>
    Private Sub calcularDiasHabilesCompleted(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarFecha))
        Try
            calcularDiasHabilesValidar(lo)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al completar la validación de los días de vigencia al editar la orden", Me.ToString(), "calcularDiasHabilesCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento valida el resultado recibido desde el servidor cuando se modifican las fechas de la orden y/o vigencia de la orden y las 
    ''' fechas de emisión y vencimiento del título
    ''' </summary>
    Private Function calcularDiasHabilesValidar(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarFecha))
        Dim objDatos As OyDOrdenes.ValidarFecha
        Dim strMsg As String = String.Empty
        Dim strAccion As String = String.Empty
        Dim logResultado As Boolean = True

        Try
            If Not lo.HasError Then
                objDatos = lo.Entities.FirstOrDefault
                strAccion = lo.UserState.ToString.ToLower

                If Not objDatos Is Nothing Then
                    If objDatos.EsDiaHabil Then
                        '/ Guardar la fecha de cierre del sistema
                        mdtmFechaCierreSistema = objDatos.FechaCierre

                        If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                            If objDatos.FechaFinal <= objDatos.FechaCierre And (mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR) Then
                                A2Utilidades.Mensajes.mostrarMensaje("La fecha de vigencia no puede ser menor a la fecha de cierre del sistema (" & objDatos.FechaCierre.ToLongDateString() & ")." & vbNewLine & vbNewLine & "Por favor modifique la fecha de vigencia de la orden para que sea posterior a la fecha de cierre.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                _mintDiasVigencia = 0
                                Return (False)
                            End If
                        End If

                        If strAccion = MSTR_ACCION_CALCULAR_DIAS Then
                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                                _mintDiasVigencia = objDatos.NroDias
                                MyBase.CambioItem("DiasVigencia")
                            Else
                                _mintDiasPlazo = objDatos.NroDias
                                MyBase.CambioItem("DiasPlazo")
                            End If
                        Else
                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_TITULO Then
                                _OrdenSelected.FechaVencimiento = objDatos.FechaFinal
                            Else
                                _OrdenSelected.VigenciaHasta = objDatos.FechaFinal
                            End If
                        End If

                        logResultado = True
                    Else 'If objDatos.EsDiaHabil Then
                        If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                            If Not objDatos.FechaHabilMayor Is Nothing Then
                                strMsg = "La fecha hábil siguiente es " & FormatDateTime(objDatos.FechaHabilMayor, Microsoft.VisualBasic.DateFormat.LongDate) & "."
                            End If
                            If Not objDatos.FechaHabilMenor Is Nothing Then
                                strMsg &= "La fecha hábil anterior es " & FormatDateTime(objDatos.FechaHabilMenor, Microsoft.VisualBasic.DateFormat.LongDate) & "."
                            End If

                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                                strMsg = "La fecha de vigencia no es un día hábil. " & strMsg & "." & vbNewLine & vbNewLine &
                                        "Debe seleccionar un día hábil."
                            Else
                                strMsg = "La fecha de vencimiento no es un día hábil. " & strMsg & "." & vbNewLine & vbNewLine &
                                        "Debe seleccionar un día hábil."
                            End If

                            A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                                If IsNothing(objDatos.NroDias) Then
                                    If IsNothing(OrdenSelected.VigenciaHasta) Then
                                        _mintDiasVigencia = 0
                                    Else
                                        ' Se suma uno porque se deben incluir la fecha de la orden y la fecha de vigencia
                                        _mintDiasVigencia = DateDiff(DateInterval.Day, _OrdenSelected.FechaOrden.Value, CType(OrdenSelected.VigenciaHasta, Date)) + 1
                                    End If
                                Else
                                    _mintDiasVigencia = objDatos.NroDias
                                End If
                                MyBase.CambioItem("DiasVigencia")
                            Else
                                If IsNothing(objDatos.NroDias) Then
                                    _mintDiasPlazo = 0
                                Else
                                    _mintDiasPlazo = objDatos.NroDias
                                End If
                                MyBase.CambioItem("DiasPlazo")
                            End If
                        End If

                        logResultado = False
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación de los días al vencimiento", Me.ToString(), "calcularDiasHabilesCompleted", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la validación de los días al vencimiento", Me.ToString(), "calcularDiasHabilesCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            mlogRecalcularFechas = True 'CCM20120305
            Me.IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Calcula el número de días entre dos fechas. 
    ''' Se suma uno a la diferencia entre las fechas para incluir la fecha inicial y final, si el parámetro Incluir extremos es verdadero.
    ''' </summary>
    ''' <param name="pdtmFechaDesde">Fecha inicial del rango</param>
    ''' <param name="pdtmFechaHasta">Fecha final del rango</param>
    ''' <param name="plogIncluirExtremos">Indica si en la diferencia en días se deben contar la fecha inicial y final (True) o no (False)</param>
    ''' <returns>Número de días entre las fechas.</returns>
    ''' <remarks></remarks>
    Private Function calcularDiasRango(ByVal pdtmFechaDesde As Date, ByVal pdtmFechaHasta As Date, Optional ByVal plogIncluirExtremos As Boolean = True) As Integer
        Dim intDias As Integer
        Try
            intDias = DateDiff(DateInterval.Day, pdtmFechaDesde, pdtmFechaHasta) + IIf(plogIncluirExtremos, 1, 0)
        Catch ex As Exception
            intDias = 0
        End Try
        Return (intDias)
    End Function

    Private Sub TerminoVerificarOrdenModificable(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenModificable))
        Dim objDatos As OyDOrdenes.OrdenModificable
        Dim logEditar As Boolean = False
        Dim strAccion As String = String.Empty
        Dim strMsg As String = String.Empty

        Try
            If Not lo.HasError Then
                objDatos = lo.Entities.FirstOrDefault
                strAccion = lo.UserState.ToString.ToLower

                If Not objDatos Is Nothing Then
                    If objDatos.Modificable Then
                        If objDatos.UltimaModificacion.Equals(_OrdenSelected.FechaActualizacion) Then
                            logEditar = True
                        Else
                            MyBase.RetornarValorEdicionNavegacion()
                            EditandoComitente = False
                            'C1.Silverlight.C1MessageBox.Show("La orden fue modificada después que se realizó la consulta para desplegarla en pantalla. Debe actualizarla para poder modificarla." & vbNewLine & vbNewLine & "¿Desea refrescar los datos de las ordenes actuales?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarRefrescarOrdenes)
                            mostrarMensajePregunta("La orden fue modificada después que se realizó la consulta para desplegarla en pantalla. Debe actualizarla para poder modificarla." & vbNewLine & vbNewLine & "¿Desea refrescar los datos de las ordenes actuales?", _
                                                   Program.TituloSistema, _
                                                   "TERMINOVERIFICARORDENMODIFICABLE", _
                                                   AddressOf validarRefrescarOrdenes, False)
                        End If
                    Else
                        strMsg = objDatos.Mensaje
                        If strAccion = MSTR_ACCION_ANULAR Then
                            strMsg &= vbNewLine & "La orden no puede ser anulada."
                        End If
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación del estado de la orden", Me.ToString(), "TerminoVerificarOrdenModificable", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If

            If logEditar Then
                If strAccion = MSTR_ACCION_EDITAR Then
                    If Not IsNothing(_OrdenSelected) Then
                        If objDatos.TieneLiquidaciones And (_OrdenSelected.Estado = "P" Or _OrdenSelected.Estado = "M") Then '
                            mostrarMensajePregunta("¿Desea registrar la orden en el libro de órdenes?", _
                                                   Program.TituloSistema, _
                                                   "LIBROORDENES", _
                                                   AddressOf TerminoPreguntarLibroOrdenes, False)
                        Else
                            logRegistroLibroOrdenes = False
                            dblValorCantidadLibroOrden = 0

                            Editando = True
                            EditandoInstrucciones = True
                            EditandoDetalle = False
                            EditandoRegistro = True
                            'HabilitarExento = False

                            If Me.ClaseOrden = ClasesOrden.A Then
                                'Jorge Andres Bedoya 20150406
                                If Me._OrdenSelected.Objeto = "AD" Then
                                    Me.IsEnableComitenteADR = True
                                Else
                                    Me.IsEnableComitenteADR = False
                                End If

                                If Me._OrdenSelected.Objeto = "OPA" Then
                                    Me.VisibilidadOfertaPublica = Visibility.Visible
                                Else
                                    Me.VisibilidadOfertaPublica = Visibility.Collapsed
                                End If

                            End If

                            mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR

                            If Me.ClaseOrden = ClasesOrden.C Then
                                Select Case Me.OrdenSelected.Objeto
                                    Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO, RF_Simultanea_Salida_CRCC, RF_Simultanea_Regreso_CRCC
                                        Me.HabilitarMercado = False
                                    Case Else
                                        Me.HabilitarMercado = True
                                End Select
                            End If
                            _strObjetoAnterior = _OrdenSelected.Objeto
                            _strObjetoAnteriorValidar = _OrdenSelected.Objeto

                            DeshabilitarBotones()
                            HabiliarComboUsuarioOperador("editar")
                        End If
                    Else
                        mstrAccionOrden = String.Empty
                    End If
                Else
                    'C1.Silverlight.C1PromptBox.Show("Comentario para la anulación de la orden: ", Program.TituloSistema,Program.Usuario, Program.HashConexion, AddressOf TerminoComentariosAnulacion)
                    mostrarMensajePregunta("Comentario para la anulación de la orden: ", _
                                           Program.TituloSistema, _
                                           "ANULARORDEN", _
                                           AddressOf TerminoComentariosAnulacion, True, "¿Anular orden?", False, True, True, False)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la validación del estado de la orden", Me.ToString(), "TerminoVerificarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            Me.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoPreguntarLibroOrdenes(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            EditandoComitente = False

            If objResultado.DialogResult Then
                logRegistroLibroOrdenes = True
                dblValorCantidadLibroOrden = _OrdenSelected.Cantidad
                Editando = True
                DeshabilitarBotones()
            Else
                MyBase.RetornarValorEdicionNavegacion()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del libro de ordenes.", Me.ToString(), "TerminoPreguntarLibroOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerOrdenesPorDefecto_Completed(ByVal lo As LoadOperation(Of OyDOrdenes.Orden))
        If Not lo.HasError Then
            OrdenPorDefecto = lo.Entities.FirstOrDefault

            If logEsModal Then
                NuevoRegistro()
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Orden por defecto", _
                                             Me.ToString(), "TerminoTraerOrdenPorDefecto_Completed", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.Orden))
        Try
            If Not lo.HasError Then

                '// Este indicador ayuda a controla la ejecución de consultas inutiles durante el ingreso especialmente
                mstrAccionOrden = String.Empty

                If dcProxy.Ordens.Count > 0 Then
                    '// Antes de asignar la colección se verifica el maker and checker para asegurar que la orden seleccionada ya esté actualizada
                    cambiarManejaMakerAndChecker(dcProxy.Ordens.Last.ManejaMakerAndChecker)

                    '// Se activan o inactiva acá estos botónes porque siempre que se guarda se actualiza la lista de órdenes
                    ActivarDuplicarOrden = True
                    ActivarEnvioSAE = True
                    ActivarSeleccionTipoOrden = True

                Else
                    '// Si no hay registros se desativa el maker and checker
                    cambiarManejaMakerAndChecker(False)

                    '// Se activan o inactiva acá estos botónes porque siempre que se guarda se actualiza la lista de órdenes
                    ActivarDuplicarOrden = False
                    ActivarEnvioSAE = False
                    ActivarSeleccionTipoOrden = False


                End If

                ListaOrdenes = dcProxy.Ordens

                If dcProxy.Ordens.Count = 0 AndAlso (lo.UserState.ToString.ToLower() = MSTR_ACCION_BUSCAR Or lo.UserState.ToString.ToLower() = MSTR_ACCION_FILTRAR) Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron registros que cumplan con las condiciones indicadas", Program.TituloSistema)
                    visNavegando = "Collapsed"
                    MyBase.CambioItem("visNavegando")
                Else
                    If lo.UserState = "TERMINOACTUALIZAR" Then
                        If ListaOrdenes.Where(Function(i) i.IDOrden = intIDOrdenActualizada).Count > 0 Then
                            OrdenSelected = ListaOrdenes.Where(Function(i) i.IDOrden = intIDOrdenActualizada).First
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la lista de Ordenes", Me.ToString(), "TerminoTraerOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            lo.MarkErrorAsHandled()   '????
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes", Me.ToString(), "TerminoTraerOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
        Finally
            IsBusy = False
            _EstadoMakerCheckerConsultar = ""
        End Try
    End Sub

    Private Sub TerminoTraerBeneficiariosOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.BeneficiariosOrden))
        If Not lo.HasError Then
            ListaBeneficiariosOrdenes = dcProxy.BeneficiariosOrdens
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenes", _
                                             Me.ToString(), "TerminoTraerBeneficiariosOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerReceptoresOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.ReceptoresOrden))
        If Not lo.HasError Then
            ListaReceptoresOrdenes = dcProxy1.ReceptoresOrdens
            'SLB20121130 Se adiciona esta logica para saber si los receptores de la orden duplicada se inactivaron
            If lo.UserState.ToString = "verificarReceptoresDuplicar" And ListaReceptoresOrdenes.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Receptor inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenes", _
                                             Me.ToString(), "TerminoTraerReceptoresOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoConsultarTrazabilidadorden(ByVal lo As LoadOperation(Of OyDOrdenes.Trazabilidad))
        If Not lo.HasError Then
            ListaTrazabilidadOrdenes = dcProxy1.Trazabilidads

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Trazabilidad", _
                                             Me.ToString(), "TerminoConsultarTrazabilidadorden", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerLiqProbablesOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.LiqAsociadasOrden))
        If Not lo.HasError Then
            ListaLiqAsociadasOrdenes = dcProxy1.LiqAsociadasOrdens
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenes", _
                                             Me.ToString(), "TerminoTraerLiqProbablesOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerPagosOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenesPago))
        If Not lo.HasError Then
            ListaOrdenesPagos = dcProxy1.OrdenesPagos
            OrdenesPagoSelected = ListaOrdenesPagos.FirstOrDefault
            If IsNothing(OrdenesPagoSelected) Then
                NombreColeccionDetalle = "cmPagosOrdenes"
                NuevoRegistroDetalle()
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenes", _
                                             Me.ToString(), "TerminoTraerPagosOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerOrdenantes(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        If Not lo.HasError Then
            Ordenantes = mdcProxyUtilidad01.BuscadorOrdenantes.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes", _
                                             Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasDeposito(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        If Not lo.HasError Then
            CuentasDeposito = mdcProxyUtilidad02.BuscadorCuentasDepositos.ToList

            If Editando = True Then
                If Me.ClaseOrden = ClasesOrden.C Then
                    'CuentaDepositoDeceval = (From Cuentas In CuentasDeposito Where Cuentas.Deposito = "D").ToList
                    CuentaDepositoDeceval = (From Cuentas In CuentasDeposito).ToList
                End If
                If Me.ClaseOrden = ClasesOrden.A Then
                    If CuentasDeposito.Where(Function(i) i.Deposito = "D" And i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).Count = 1 Then
                        CtaDepositoSeleccionada = CuentasDeposito.Where(Function(i) i.Deposito = "D" And i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).First
                    End If
                Else
                    If CuentasDeposito.Where(Function(i) i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).Count = 1 Then
                        CtaDepositoSeleccionada = CuentasDeposito.Where(Function(i) i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).First
                    End If
                End If
            End If


        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                             Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasDepositoPago(ByVal lo As LoadOperation(Of OyDOrdenes.CuentasDepositoPago))
        If Not lo.HasError Then
            CuentasDepositoPago = dcProxy.CuentasDepositoPagos.ToList
            _CuentasDepositoPagoSeleccionada = _CuentasDepositoPago
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                             Me.ToString(), "TerminoTraerCuentasDepositoPago", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerInstrucciones(ByVal lo As LoadOperation(Of OyDOrdenes.InstruccionesOrdene))
        If Not lo.HasError Then
            ListaInstruccionesOrdenes = dcProxy1.InstruccionesOrdenes
            _EstadoMakerCheckerConsultarInstrucciones = ""
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                             Me.ToString(), "TerminoTraerInstrucciones", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerComisionesOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.Comisiones))
        If Not lo.HasError Then
            TiposComisiones = dcProxy1.Comisiones.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                             Me.ToString(), "TerminoTraerComisionesOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerAdicionalesOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.AdicionalesOrdene))
        If Not lo.HasError Then
            ListaAdicionalesOrdenes = dcProxy1.AdicionalesOrdenes
            _EstadoMakerCheckerConsultarAdicionales = ""
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                             Me.ToString(), "TerminoTraerAdicionalesOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasClientes(ByVal lo As LoadOperation(Of OyDOrdenes.CuentasClientes))
        If Not lo.HasError Then
            CuentasClientes = dcProxy.CuentasClientes.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                             Me.ToString(), "TerminoTraerCuentasClientes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClaseOrden(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then
            If Not IsNothing(lo.Value) Then
                EsBono = lo.Value
                If EsBono = True Then
                    'If EsEdicion Then
                    HabilitarExento = True
                    'Else
                    '    HabilitarExento = False
                    'End If
                    '_OrdenSelected.ExentoRetencion = 0
                    VisibilidadCampoExento = Visibility.Visible
                Else
                    HabilitarExento = False
                    _OrdenSelected.ExentoRetencion = 0
                    VisibilidadCampoExento = Visibility.Collapsed
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la clase de la orden", _
                     Me.ToString(), "TerminoTraerClaseOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoEnrutarOrdenSAE(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenSAE))
        Dim objOrdenSAE As OyDOrdenes.OrdenSAE
        Dim logConsultarSAE As Boolean = False

        Try
            Me._mstrNroOrdenSAE = String.Empty
            If Not lo.HasError Then
                If dcProxy1.OrdenSAEs.ToList.Count > 0 Then
                    objOrdenSAE = dcProxy1.OrdenSAEs.First
                    If objOrdenSAE.TipoMensaje > 1 Then
                        'Me._mstrEstadoSAE = String.Empty
                        Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE
                        A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema & " - Error en enrutamiento", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf objOrdenSAE.TipoMensaje > 0 Then
                        Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE
                        A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema & " - Advertencias en enrutamiento", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Orden enrutada exitosamente a través de SAE y se marcara como " & objOrdenSAE.EstadoSAE & ".", Program.TituloSistema)
                        Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE
                    End If
                Else
                    Me._mstrEstadoSAE = String.Empty
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir respueta del enrutamiento de la orden por SAE", Me.ToString(), "TerminoEnrutarOrdenSAE", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????

                Me._mstrEstadoSAE = String.Empty
            End If

            MyBase.CambioItem("NroOrdenSAE")
            MyBase.CambioItem("EstadoSAE")

            If logConsultarSAE Then
                consultarOrdenSAE()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el enrutamiento de la orden por SAE", Me.ToString(), "TerminoEnrutarOrdenSAE", Program.TituloSistema, Program.Maquina, ex)
        Finally
            Me.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarOrdenSAE(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenSAE))
        Dim objOrdenSAE As OyDOrdenes.OrdenSAE
        Try
            If Not lo.HasError Then
                If dcProxy1.OrdenSAEs.ToList.Count > 0 Then
                    objOrdenSAE = dcProxy1.OrdenSAEs.First
                    Me._mstrNroOrdenSAE = objOrdenSAE.NroOrdenSAE
                    Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE

                    Me.mlogPuedeEnrutarSAE = objOrdenSAE.SAEUsrTienePermisos
                    Me.mlogSAEActivoClaseOrden = objOrdenSAE.SAEActivoClaseOrden
                    Me.mlogSAEActivo = objOrdenSAE.SAEActivo

                    MyBase.CambioItem("SAEActivo")
                Else
                    Me._mstrNroOrdenSAE = String.Empty
                    Me._mstrEstadoSAE = String.Empty

                    Me.mlogSAEActivo = False
                    Me.mlogSAEActivoClaseOrden = False
                    Me.mlogPuedeEnrutarSAE = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la consulta de la orden en SAE", Me.ToString(), "TerminoConsultarOrdenSAE", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????

                Me._mstrNroOrdenSAE = String.Empty
                Me._mstrEstadoSAE = String.Empty

                Me.mlogSAEActivo = False
                Me.mlogSAEActivoClaseOrden = False
                Me.mlogPuedeEnrutarSAE = False
            End If

            MyBase.CambioItem("NroOrdenSAE")
            MyBase.CambioItem("EstadoSAE")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de la orden en SAE", Me.ToString(), "TerminoConsultarOrdenSAE", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando termina de guardar los cambios de la orden.
    ''' </summary>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)

        Dim strMsg As String = String.Empty
        Try
            IsBusy = False
            If So.HasError Then
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next

                    If Not strMsg.Equals(String.Empty) Then
                        CrearLogOrdenesReceptores("TerminoSubmitChanges", strMsg)
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    End If
                Else
                    CrearLogOrdenesReceptores("TerminoSubmitChanges", So.Error.ToString())
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                                   Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                    So.MarkErrorAsHandled()
                End If

                Exit Try
            Else
                Editando = False
                EditandoInstrucciones = False
                EditandoDetalle = True
                EditandoRegistro = False
                HabilitarExento = False
                EsEdicion = False
                IsEnableComitenteADR = False 'Jorge Andres Bedoya 20150406
                habilitarFechaElaboracion = False
                HabilitarMercado = False
                EditandoComitente = False
                ' (SLB) Mostar mensajes cuando no se utiliza al Maker and Checker
                If Not _mlogManejaMakerAndChecker Then
                    Select Case So.UserState
                        Case "I"
                            CrearLogOrdenesReceptores("TerminoSubmitChanges", "Se ha creado correctamente la Orden Nro " & CStr(_OrdenSelected.NroOrden))
                            A2Utilidades.Mensajes.mostrarMensaje("Se ha creado correctamente la Orden Nro " & CStr(_OrdenSelected.NroOrden), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

                            ' SLB si la orden es una Simultanea, TTV o una REPO, se debe recargar la Lista para mostrar la punta generada
                            'If (_OrdenSelected.Objeto = RF_Simulatena_Salida Or _OrdenSelected.Objeto <> RF_TTV_Salida Or _OrdenSelected.Objeto <> RF_REPO) Then
                            '    Filtrar()
                            'End If
                        Case "U"
                            CrearLogOrdenesReceptores("TerminoSubmitChanges", "Se ha modificado correctamente la Orden Nro " & CStr(_OrdenSelected.NroOrden))
                            A2Utilidades.Mensajes.mostrarMensaje("Se ha modificado correctamente la Orden Nro " & CStr(_OrdenSelected.NroOrden), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                            ValidarUsuarioOperador("cancelar")

                    End Select

                    intIDOrdenActualizada = _OrdenSelected.IDOrden
                    MyBase.QuitarFiltroDespuesGuardar()

                    If logEsModal Then
                        RaiseEvent TerminoGuardarRegistro(True, _OrdenSelected.NroOrden, _OrdenSelected.Clase, _OrdenSelected.Tipo)
                    Else
                        RefrescarOrden() 'SLB20140320 Por temas de rendimiento se cambio Filtrar()

                        HabilitarBotones()
                        AccionInstrucciones()
                    End If
                Else
                    CrearLogOrdenesReceptores("TerminoSubmitChanges", "Maker and Checker")
                    MyBase.QuitarFiltroDespuesGuardar()

                    If logEsModal Then
                        RaiseEvent TerminoGuardarRegistro(True, _OrdenSelected.NroOrden, _OrdenSelected.Clase, _OrdenSelected.Tipo)
                    Else
                        '(SLB) Cuando se utiliza el Maker and Checker se debe recargar la lista de ordenes para refresacar con los cambios realizados en base de datos.
                        Filtrar()
                    End If
                End If
            End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            CrearLogOrdenesReceptores("TerminoSubmitChanges", ex.ToString())
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", Me.ToString(), "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el proceso de autorización de la orden finaliza
    ''' </summary>
    Private Sub TerminoAutorizarOrden(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenSAE))
        Dim objOrdenSAE As OyDOrdenes.OrdenSAE
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se cambió el estado de la orden porque se presentó un problema durante el proceso.", Me.ToString(), "Autorizar_OrdenCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                IsBusy = False
            Else
                objOrdenSAE = dcProxy.OrdenSAEs.First
                If objOrdenSAE.TipoMensaje > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    IsBusy = False
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    Filtrar()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la aprobación de la orden.", Me.ToString(), "TerminoAutorizarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarLiquidacionesOrden(ByVal lo As LoadOperation(Of OyDOrdenes.LiquidacionesOrden))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se cargaron las liquidaciones de la orden porque se presentó un problema durante el proceso.", Me.ToString(), "Autorizar_OrdenCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                IsBusy = False
            Else
                LiquidacionesOrden = dcProxyConsultas.LiquidacionesOrdens
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir las liquidaciones de la orden", Me.ToString(), "TerminoConsultarLiquidacionesOrden", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
            IsBusyLiquidaciones = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando finaliza la anulación de la orden en el servidor
    ''' </summary>
    Private Sub TerminoAnularOrden(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenSAE))
        Dim objOrdenSAE As OyDOrdenes.OrdenSAE
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se anuló la orden porque se presentó un problema durante el proceso.", Me.ToString(), "TerminoAnularOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
            Else
                ' (SLB) Modifcado para que trabaje con el Maker and Checker
                objOrdenSAE = dcProxy.OrdenSAEs.First
                If objOrdenSAE.TipoMensaje > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                ElseIf objOrdenSAE.Msg.Contains("OrdenAnuladaSinMaker,") Then
                    Dim Mensaje = Split(objOrdenSAE.Msg, ",")
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(1), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    RefrescarOrden() 'SLB20140320 Por temas de rendimiento se cambio Filtrar()
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("La anulación de la orden está pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    Filtrar()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la anulación de la orden.", Me.ToString(), "TerminoAnularOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la orden
    ''' Se dispara cuando la busqueda del comitente iniciada desde el procedimiento buscarComitente finaliza
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.UserState.ToString = "buscarComitenteOrden" Then

                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        Me.ComitenteSeleccionado = Nothing
                        If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                            A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                        ComitenteOrden = String.Empty
                        dcProxy1.ReceptoresOrdens.Clear()
                        ListaReceptoresOrdenes = dcProxy1.ReceptoresOrdens
                    Else
                        Me.ComitenteSeleccionado = lo.Entities.ToList.Item(0)
                    End If

                Else
                    Me.ComitenteSeleccionado = lo.Entities.ToList.Item(0)

                    If lo.UserState.ToString = "buscar" Then
                        _mobjComitenteSeleccionadoAntes = _ComitenteSeleccionado
                    End If
                End If
            Else
                Me.ComitenteSeleccionado = Nothing
                If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                ComitenteOrden = String.Empty
                dcProxy1.ReceptoresOrdens.Clear()
                ListaReceptoresOrdenes = dcProxy1.ReceptoresOrdens
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del nemotecnico que tiene asignada la orden
    ''' Se dispara cuando la busqueda del nemotecnico iniciada desde el procedimiento buscarNemotecnico finaliza
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub buscarNemotecnicoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If Editando = False Then
                    Me.NemotecnicoSeleccionado = lo.Entities.ToList.Item(0)
                End If

                'SLB20130731 Manejo de la Especies en Renta Fija
                If Me.ClaseOrden.ToString() = ClasesOrden.C.ToString() Then
                    mlogEspecieAccion = lo.Entities.ToList.Item(0).EsAccion
                    mstrEspecieTipoTasa = lo.Entities.ToList.Item(0).CodTipoTasaFija
                End If

                If Editando Then
                    If SeleccionarUnISIN Then
                        If lo.Entities.ToList.Count = 1 Then
                            actualizarNemotecnicoOrden(lo.Entities.ToList.Item(0), True)
                        Else
                            actualizarNemotecnicoOrden(lo.Entities.ToList.Item(0), False)
                        End If
                    Else
                        actualizarNemotecnicoOrden(lo.Entities.ToList.Item(0), True)
                    End If


                    If Not Me.NemotecnicoSeleccionado.Activo Then
                        A2Utilidades.Mensajes.mostrarMensaje("El nemotécnico ingresado se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Me.NemotecnicoSeleccionado = Nothing
                        NemotecnicoOrden = String.Empty
                        Exit Sub
                    End If

                    If Me.ClaseOrden.ToString = ClasesOrden.A.ToString Then
                        If Me._OrdenSelected.Objeto <> "IC" Then
                            If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                                A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    Else
                        If Me._OrdenSelected.Objeto <> "IC" Or (Not IsNothing(Me.ComitenteSeleccionado) AndAlso Me.ComitenteSeleccionado.CodCategoria <> "2") Then
                            If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                                A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado y el cliente debe ser Inversionista Profesional", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If
                _mobjNemotecnicoSeleccionadoAntes = _NemotecnicoSeleccionado
            Else
                Me.NemotecnicoSeleccionado = Nothing
                If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                    A2Utilidades.Mensajes.mostrarMensaje("El nemotécnico ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                NemotecnicoOrden = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del nemotécnico de la orden", Me.ToString(), "buscarNemotecnicoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del receptor toma que tiene asignada la orden
    ''' Se dispara cuando la busqueda del receptor toma iniciada desde el procedimiento buscarItem finaliza
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub buscarGenericoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Select Case strTipoItem.ToLower()
                    Case "receptores"
                        Me.ReceptorTomaSeleccionado = lo.Entities.ToList.Item(0)
                        Me._mobjReceptorTomaSeleccionadoAntes = Me._ReceptorTomaSeleccionado
                End Select
            Else
                Select Case strTipoItem.ToLower()
                    Case "receptores"
                        Me.ReceptorTomaSeleccionado = Nothing
                        Me._mobjReceptorTomaSeleccionadoAntes = Me._ReceptorTomaSeleccionado
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario si aprueba la orden con una excepción RDIP
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarExcepcionRDIPAprobar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                aprobarOrden(True)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el riesgo en la aprobación de la orden", Me.ToString(), "validarExcepcionRDIPAprobar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario guarda la orden con una excepción RDIP
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarExcepcionRDIPGuardar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                guardarOrden()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el guardado de la orden", Me.ToString(), "validarExcepcionRDIPGuardar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario guarda la orden con una excepción RDIP
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarGuardarPagoIncompleto(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                mlogGuardarPagoIncompleto = False
                validarOrden()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el guardado de la orden", Me.ToString(), "validarExcepcionRDIPGuardar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si se refrescan los datos de las órdenes que están en pantalla
    ''' </summary>
    Private Sub validarRefrescarOrdenes(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                Me.FiltroVM = Me.OrdenSelected.NroOrden
                Filtrar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar si se refrescan los datos de las órdenes en pantalla", Me.ToString(), "validarRefrescarOrdenes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario aprueba la orden 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarAprobarOrden(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                aprobarOrdenConfirmado()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar la aprobación de la orden", Me.ToString(), "validarAprobarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario rechaza la orden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarRechazoOrden(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                aprobarOrden(False)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el rechazo de la orden", Me.ToString(), "validarRechazoOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario ingresa el comentario a la anulación de la orden
    ''' </summary>
    Private Sub TerminoComentariosAnulacion(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                dcProxy.OrdenSAEs.Clear()
                dcProxy.Load(dcProxy.AnularOrdenQuery(_OrdenSelected.Clase, _OrdenSelected.Tipo, _OrdenSelected.NroOrden, _OrdenSelected.Version, objResultado.Observaciones, "", Program.Usuario, Program.HashConexion), AddressOf TerminoAnularOrden, MSTR_ACCION_ANULAR)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el rechazo de la orden", Me.ToString(), "validarRechazoOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <sumary>
    ''' (SLB) Se ejecuta cuando retorna de confirmar si el usuario desea duplicar la orden
    ''' </sumary>
    Private Sub validarDuplicarOrden(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                EditandoComitente = True
                configurarNuevaOrden(_OrdenSelected, True, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el duplicar una orden", Me.ToString(), "validarDuplicarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoValidarPermisosDuplicar(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then
            If lo.Value Then
                DuplicarOrdenBoton = Visibility.Visible
            Else
                DuplicarOrdenBoton = Visibility.Collapsed
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar permisos para Duplicar una Orden", _
                     Me.ToString(), "TerminoValidarPermisosDuplicar", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        Try
            If obj.HasError Then
                If obj.UserState = "EDITAR" Then
                    MyBase.RetornarValorEdicionNavegacion()
                End If
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                mdtmFechaCierreSistema = obj.Value

                If obj.UserState = "EDITAR" Then
                    ContinuarEdicionDocumento()
                ElseIf obj.UserState = "GUARDAR" Then
                    ContinuarGuardadoDocumento()
                ElseIf obj.UserState = "BORRAR" Then
                    ContinuarBorradoDocumento()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", _
                                                 Me.ToString(), "consultarFechaCierreCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


#Region "Métodos"

    Private Sub ValidarPermisosDuplicar()
        Try
            dcProxy1.ValidarUsuario_DuplicarOrden(Program.Usuario, Program.HashConexion, AddressOf TerminoValidarPermisosDuplicar, "consulta")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar permisos para Duplicar una Orden", Me.ToString(), "ValidarPermisosDuplicar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub ValidarUsuarioOperador(Optional ByVal pstrAccion As String = "")
        Try
            If Not IsNothing(Me._OrdenSelected) Then
                UsuarioOperador = _OrdenSelected.UsuarioOperador
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el usuario operador", Me.ToString(), "ValidarUsuarioOperador", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub HabiliarComboUsuarioOperador(Optional ByVal pstrAccion As String = "")
        Try
            If pstrAccion = "editar" Or mlogDuplicarOrden Then
                ExisteUsuarioOperador = False
                If Not IsNothing(objDicUsuarioOperador) Then
                    For Each objOperador In objDicUsuarioOperador
                        If objOperador.ID = UsuarioOperador Then
                            ExisteUsuarioOperador = True
                            Exit For
                        End If
                    Next

                    If Not ExisteUsuarioOperador Then
                        Me._OrdenSelected.UsuarioOperador = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar el combo usuario operador", Me.ToString(), "ValidarUsuarioOperador", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Habilitar los botones de Duplicar, Enviar SAE, Compra, Venta y Modificación Instrucción.
    ''' </summary>
    ''' <remarks>SLB20130202</remarks>
    Friend Sub HabilitarBotones()
        ActivarDuplicarOrden = True
        ActivarEnvioSAE = True
        ActivarSeleccionTipoOrden = True
        ActivarModInstruccion = True
    End Sub

    ''' <summary>
    ''' Deshabilitar los botones de Duplicar, Enviar SAE, Compra, Venta y Modificación Instrucción.
    ''' </summary>
    ''' <remarks>SLB20130202</remarks>
    Friend Sub DeshabilitarBotones(Optional ByVal strOpcion As String = "")
        ActivarDuplicarOrden = False
        ActivarEnvioSAE = False
        ActivarSeleccionTipoOrden = False
        If Not strOpcion = "ModificarInstrucciones" Then
            ActivarModInstruccion = False
        End If
    End Sub

    Public Sub ModificarInstrucciones()
        Try
            If Not Me._OrdenSelected.Version <> 0 And Me._OrdenSelected.Modificable Then
                If TituloInstruccion = MSTR_TITULO_INSTRUCCION_MODIFICAR Then
                    EditandoInstrucciones = True
                    Me.TabSeleccionadoGeneral = OrdenTabs.Instrucciones
                    TituloInstruccion = MSTR_TITULO_INSTRUCCION_GUARDAR
                    DeshabilitarBotones("ModificarInstrucciones")
                Else
                    ValidarInstruccionOrden()
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Solo se pueden modificar las instrucciones de las órdenes con estado pendiente y con version 0.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al modificar la Instrucción", Me.ToString(), "ModificarInstrucciones", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub AccionInstrucciones()
        EditandoInstrucciones = False
        TituloInstruccion = MSTR_TITULO_INSTRUCCION_MODIFICAR
    End Sub

    Private Sub ValidarInstruccionOrden()

        Try
            Dim strInstruccionesOrdenes As String = String.Empty
            Dim lstLiqAsociadas As List(Of Integer)
            Dim lstLiqAsociadasRep As List(Of Object)

            '(SLB) Se adiciona la lista de Instrucciones para la orden
            lstLiqAsociadas = New List(Of Integer)
            lstLiqAsociadasRep = New List(Of Object)

            strInstruccionesOrdenes = "<instrucciones>"
            If ListaInstruccionesOrdenes.Count > 0 Then
                For Each obj In ListaInstruccionesOrdenes
                    If IsNothing(obj.Valor) Then
                        obj.Valor = 0
                    End If
                    If obj.Cuenta <> "" Or obj.Valor <> 0 Or obj.Seleccionado Then
                        strInstruccionesOrdenes &= "<instruccione strRetorno=""" & CStr(obj.Retorno) & """ strInstruccion=""" & CStr(obj.Instruccion) & """ strCuenta=""" & CStr(obj.Cuenta) & """  dblValor=""" & CStr(obj.Valor) & """ logSeleccionado=""" & CStr(obj.Seleccionado) & """ />"
                    End If
                Next
            End If
            strInstruccionesOrdenes &= "</instrucciones>"

            If lstLiqAsociadasRep.Count > 0 Then
                For Each obj In lstLiqAsociadasRep
                    ListaInstruccionesOrdenes.Remove(obj)
                Next
            End If

            IsBusy = True
            dcProxy1.Actualizar_InstruccionesOrdenes(_OrdenSelected.Tipo, _OrdenSelected.Clase, _OrdenSelected.NroOrden, _OrdenSelected.Version, _OrdenSelected.IdBolsa, strInstruccionesOrdenes, Program.Usuario, Program.HashConexion, AddressOf TerminoGrabarInstrucciones, "insert")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la Instrucción Orden", Me.ToString(), "ValidarInstruccionOrden", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoGrabarInstrucciones(ByVal lo As InvokeOperation(Of Boolean))
        IsBusy = False
        If Not lo.HasError Then
            AccionInstrucciones()
            HabilitarBotones()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar permisos para Duplicar una Orden", _
                     Me.ToString(), "TerminoValidarPermisosDuplicar", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' Valida si el usuario tienen permisos para adicionar órdenes. 
    ''' La verificación se hace comprobando si el botón Nuevo está o no presente en la barra de herramientas
    ''' </summary>
    ''' <remarks>CCM20120305</remarks>
    Private Sub validarPermisosAdicionar()
        Dim objToolbar As List(Of String) = Nothing
        Dim strDatosBoton As String = String.Empty

        Try
            If Application.Current.Resources.Contains("A2Consola_ToolbarActivo") Then
                objToolbar = CType(Application.Current.Resources("A2Consola_ToolbarActivo"), List(Of String))

                For Each strDatosBoton In objToolbar
                    If strDatosBoton.Substring(0, 5).ToLower() = "nuevo" Then
                        mlogUsuarioPuedeIngresar = True
                        Exit For
                    Else
                        mlogUsuarioPuedeIngresar = False
                    End If
                Next
            Else
                mlogUsuarioPuedeIngresar = False
            End If
            MyBase.CambioItem("IngresoOrdenActivo")
        Catch ex As Exception
            mlogUsuarioPuedeIngresar = False
            MyBase.CambioItem("IngresoOrdenActivo")
        End Try
    End Sub

    ''' <summary>
    ''' Consulta en los datos de los combos el grupo de datos PARAM_ORDENES que tiene varios parámetros para configurar el comportamiento de la orden
    ''' </summary>
    ''' <remarks>CCM20120305</remarks>
    Private Sub leerParametros()

        If mlogDuplicarDatosParam Then
            Exit Sub
        End If

        Dim objParametrosDiccionario As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim strValor As String = ""


        If Application.Current.Resources.Contains(Me.ListaCombosEsp) Then
            Try
                If CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey(NombreCategoriaEnDiccionario("O_PARAM_CONFIG_ORDENES")) Then
                    objParametrosDiccionario = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_PARAM_CONFIG_ORDENES"))
                End If
            Catch ex As Exception
                Exit Sub
            End Try

            If Not IsNothing(objParametrosDiccionario) Then

                If CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey(NombreCategoriaEnDiccionario("O_USUARIO_OPERADOR")) Then
                    objDicUsuarioOperador = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_USUARIO_OPERADOR"))
                End If

                Try
                    mstrTipoLimitePorDef = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_TIPO_LIMITE_DEFAULT" Select obj).First.Valor
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_DUPLICAR_DATOS_LEO" Select obj).First.Valor
                    If strValor = "S" Then
                        mlogDuplicarDatosLeo = True
                    Else
                        mlogDuplicarDatosLeo = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_DUPLICAR_FECHA_LEO" Select obj).First.Valor
                    If strValor = "S" Then
                        mlogDuplicarFechaLeo = True
                    Else
                        mlogDuplicarFechaLeo = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_DUPLICAR_DATOS_PRECIO_ACC" Select obj).First.Valor
                    If strValor = "S" Then
                        mlogDuplicarDatosPrecio = True
                    Else
                        mlogDuplicarDatosPrecio = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                'Sebastian Londoño, se adiciona los parametros el Tipo de Transaccion y la excepcionesRDIP
                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_APLICA_TIPO_TRANSACCION" Select obj).First.Valor
                    If strValor = "S" Then
                        mlogAplicaTipoTransaccion = True
                    Else
                        mlogAplicaTipoTransaccion = False
                    End If
                Catch ex As Exception

                Finally
                    MyBase.CambioItem("TipoTransaccionActivo")
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_APLICA_EXCEPCION_RDIP" Select obj).First.Valor
                    If strValor = "S" Then
                        mlogAplicaExcepcionRDIP = True
                    Else
                        mlogAplicaExcepcionRDIP = False
                    End If
                Catch ex As Exception

                End Try

                Try
                    If ClasesOrden.A.ToString = Me._OrdenSelected.Clase Then
                        strValor = (From obj In objListaParametros Where obj.Parametro = "PORC_CANT_VISIBLE" Select obj).First.Valor
                        If strValor <> String.Empty Then
                            intPorcentajeCantidadVisible = CInt(strValor)
                        End If
                    End If

                Catch ex As Exception

                End Try

                Try
                    strValor = (From obj In objParametrosDiccionario Where obj.ID = "HABILITAR_FECHA_ELABORACION" Select obj).First.Descripcion
                    If strValor = "1" Then
                        _mlogHabilitarFechaElaboracion = True
                    Else
                        _mlogHabilitarFechaElaboracion = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objParametrosDiccionario Where obj.ID = "COMBO_RECEPTORES_CLIENTES" Select obj).First.Descripcion
                    If strValor = "1" Then
                        _mlogMostrarTodosReceptores = False
                    Else
                        _mlogMostrarTodosReceptores = True
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objParametrosDiccionario Where obj.ID = "CARGAR_RECEPTORES_CLIENTES" Select obj).First.Descripcion
                    If strValor = "1" Then
                        _mlogCargarReceptorClientes = True
                    Else
                        _mlogCargarReceptorClientes = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objParametrosDiccionario Where obj.ID = "REQUIERO_INGRESO_ORDENANTES" Select obj).First.Descripcion
                    If strValor = "1" Then
                        _mlogRequiereIngresoOrdenantes = True
                    Else
                        _mlogRequiereIngresoOrdenantes = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "DIAS_VIGENCIA_ORDEN_INMEDIATA" Select obj).First.Valor
                    intDiasVigenciaDuracionInmediata = IIf(IsNothing(strValor), 1, CInt(strValor))
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    mstrFormaPagoPorDef = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_TIPO_FORMAPAGO_DEFAULT" Select obj).First.Valor
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    mstrPago = (From obj In objListaParametros Where obj.Parametro = "CtaPesosOrdrentaFija" Select obj).First.Valor.ToUpper
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    mstrValidarCuidadSeteo = (From obj In objListaParametros Where obj.Parametro = "ValidarCiudadSeteo" Select obj).First.Valor.ToUpper
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "PATRIMONIOTECNICO" Select obj).First.Valor
                    If String.IsNullOrEmpty(strValor) Then
                        strValor = "0"
                    End If
                    dblPatrimonioTecnico = CDbl(strValor)
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "CUENTADEPOSITOORDENES" Select obj).First.Valor
                    If strValor = "1" Or strValor = "SI" Then
                        logValidarCuentaDeposito = True
                    Else
                        logValidarCuentaDeposito = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_APLICA_TAB_INSTRUCCIONES" Select obj).First.Valor
                    If strValor.ToUpper.Equals("S") Then
                        MostrarTabInstrucciones = Visibility.Visible
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "DefectoEnBlancoCamposLEO" Select obj).First.Valor
                    If strValor.ToUpper.Equals("SI") Then
                        logBorrarCamposLeo = True
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_DURACION_DEFAULT" Select obj).First.Valor
                    If Not String.IsNullOrEmpty(strValor) Then
                        mstrDuracion = strValor
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                'Jorge Andres Bedoya 2014/12/29
                Try
                    strValor = ""
                    strValor = (From obj In objListaParametros Where obj.Parametro = "ORDENES_VALOR_COMPROMISO_FUTURO_REQUERIDO" Select obj).First.Valor
                    If strValor.ToUpper.Equals("SI") Then
                        logValorCompromisoFuturoRequerido = True
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try


                'JABG2016/06/15
                Try
                    strValor = ""
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_DUPLICAR_MSG_LEO" Select obj).First.Valor
                    If strValor.ToUpper.Equals("SI") Then
                        mlogMsgDuplicarDatosLeo = True
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try


                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "PERMITIR_ORDEN_FECHA_TOMA" Select obj).First.Valor
                    If strValor = "SI" Then
                        mlogIngresarFechaTomaCierre = True
                    Else
                        mlogIngresarFechaTomaCierre = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "MODIFICAR_FECHATOMA_FECHASISTEMA" Select obj).First.Valor
                    If strValor = "SI" Then
                        mlogModificarechaTomaIgualSistema = True
                    Else
                        mlogModificarechaTomaIgualSistema = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                If ClaseOrden = ClasesOrden.C Then
                    Try
                        strValor = (From obj In objListaParametros Where obj.Parametro = "PERMITIR_ORDEN_CONSULTAR_ISIN" Select obj).First.Valor
                        If strValor = "SI" Then
                            HabilitarSeleccionISIN = True
                            SeleccionarUnISIN = False
                        Else
                            HabilitarSeleccionISIN = False

                            Try
                                strValor = (From obj In objListaParametros Where obj.Parametro = "PERMITIR_ORDEN_CONSULTAR_UN_SOLO_ISIN" Select obj).First.Valor
                                If strValor = "SI" Then
                                    SeleccionarUnISIN = True
                                Else
                                    SeleccionarUnISIN = False
                                End If
                            Catch ex As Exception
                                '-- Si hay error se trabaja con el valor por defecto del parámetro
                            End Try

                        End If
                    Catch ex As Exception
                        '-- Si hay error se trabaja con el valor por defecto del parámetro
                    End Try
                End If

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "HABILITAREXENTO_ORDENES" Select obj).First.Valor
                    If strValor = "SI" Then
                        'VisibilidadExento = Visibility.Visible
                        VisibilidadCampoExento = Visibility.Visible
                        ActivoExento = "SI"
                    Else
                        'VisibilidadExento = Visibility.Collapsed
                        VisibilidadCampoExento = Visibility.Collapsed
                        ActivoExento = "NO"
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                mlogDuplicarDatosParam = True

            End If
        End If
    End Sub

    ''' <summary>
    ''' Calcula el número de días hábiles entre dos fechas o a partir de la fecha inicial y un número de días calcula la fecha hábil final.
    ''' </summary>
    ''' <param name="pstrTipoCalculo">Indica si el cálculo es para los días de la orden o los días de vencimiento del título</param>
    ''' <param name="pintDias">Indica el número de días entre la fecha inicial y final. Si no se recibe o es menor que cero se calcula el número de días, si se recibe se calcula la fecha final.</param>
    ''' <param name="plogGuardarOrden">Si es verdadero indica que la orden debe ser guardada si pasa las validaciones. Si es falso solamente se hacen las validaciones</param>
    ''' 
    Public Sub calcularDiasOrden(ByVal pstrTipoCalculo As String, Optional ByVal pintDias As Integer = -1, Optional ByVal plogGuardarOrden As Boolean = False)
        Try
            _mlogFechaCierreSistema = False
            If _OrdenSelected Is Nothing Then
                Exit Sub
                'ElseIf mstrAccionOrden.Equals(String.Empty) And _mintDiasVigencia > 0 Then 'CCM20120306 - Desactivar esta condición porque no recalcula los días de vigencia
                'Exit Sub
            ElseIf pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO And Me.ClaseOrden = ClasesOrden.A Then
                '/ Los días de vencimiento del título solamente se calculan para renta fija
                Exit Sub
            ElseIf (IsNothing(_OrdenSelected.FechaOrden) Or (IsNothing(_OrdenSelected.VigenciaHasta) And pintDias <= 0)) And pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_ORDEN Then
                '/ Para calcular la fecha de vigencia o los días de vigencia de la orden es necesario tener la fecha de la orden y los días de vigencia o la fecha de vigencia respectivamente
                Exit Sub
            ElseIf (IsNothing(_OrdenSelected.FechaVencimiento) Or (IsNothing(_OrdenSelected.FechaEmision) And pintDias <= 0)) And pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                '/ Para calcular la fecha de vencimiento o los días de plazo del título es necesario tener la fecha de emisión y el plazo o la fecha de vencimiento respectivamente
                Exit Sub
            ElseIf mdtmFechaCierreSistema >= _OrdenSelected.VigenciaHasta Then
                _mlogFechaCierreSistema = True
                'CambiarAForma()
                'A2Utilidades.Mensajes.mostrarMensaje("La fecha de la orden no puede ser menor a la fecha de cierre del sistema (" & mdtmFechaCierreSistema.ToLongDateString() & ")." & vbNewLine & vbNewLine & "Por favor modifique la fecha de elaboración de la orden para que sea posterior a la fecha de cierre.", Program.TituloSistema)
                If Editando = False Then
                    Exit Sub
                End If
            ElseIf _OrdenSelected.VigenciaHasta < _OrdenSelected.FechaOrden.Value.Date Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de la orden no puede ser mayor a la fecha de vigencia." & vbNewLine & vbNewLine & "Por favor modifique la fecha de elaboración o la fecha de vigencia de la orden.", Program.TituloSistema)
                IsBusy = False
                Exit Sub
            ElseIf Not IsNothing(_OrdenSelected.FechaEmision) And Not IsNothing(_OrdenSelected.FechaVencimiento) Then
                If _OrdenSelected.FechaVencimiento < _OrdenSelected.FechaEmision Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de emisión del título no puede ser mayor a la fecha de vencimiento." & vbNewLine & vbNewLine & "Por favor modifique la fecha de emisión o la fecha de vencimiento del título.", Program.TituloSistema)
                    IsBusy = False
                    Exit Sub
                End If
            End If

            'IsBusy = True

            dcProxy.ValidarFechas.Clear()
            If pintDias <= 0 Then
                ' Calcular los días al vencimiento de la orden a partir de la fecha de elaboración y vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaEmision, _OrdenSelected.FechaVencimiento, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaEmision, _OrdenSelected.FechaVencimiento, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    End If
                Else
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaOrden.Value.Date, _OrdenSelected.VigenciaHasta, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaOrden.Value.Date, _OrdenSelected.VigenciaHasta, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    End If
                End If
            Else
                ' Calcular la fecha de vencimiento de la orden a partir de la fecha de elaboración y los días al vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaEmision, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaEmision, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    End If
                Else
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaOrden.Value.Date, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaOrden.Value.Date, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    End If
                End If
            End If
        Catch ex As Exception
            mlogRecalcularFechas = True 'CCM20120305
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular los días al vencimiento", Me.ToString(), "calcularDiasOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa un nuevo objeto de tipo orden con los valores por defecto para iniciar el ingreso de una nueva orden. El ingreso de una nueva orden se da
    ''' porque el usuario utilice la funcionalidad para crear una nueva orden desde cero o porque duplique una existente.
    ''' </summary>
    ''' <param name="pobjDatosOrden">Datos por defecto para inicalizar la orden</param>
    ''' <param name="plogDuplicar">Si es verdadero indica que se está ducplicando la orden actual, de lo contrario se ingresa una orden desde cero</param>
    ''' <param name="plogCompra">Si es verdadero se ingresa una nueva orden de compra, si es falso una orden de venta</param>
    ''' <remarks>
    ''' Algunos valores para la inicialización de la nueva orden no se toman de la orden recibida como parámetro, sino que siempre se inicializan 
    ''' siempre de la orden por defecto.
    ''' </remarks>
    Private Sub configurarNuevaOrden(ByVal pobjDatosOrden As OyDOrdenes.Orden, ByVal plogDuplicar As Boolean, ByVal plogCompra As System.Nullable(Of Boolean))

        Dim intDias As Integer = 0

        Try
            mlogRecalcularFechas = False ' 'CCM20120305 - Desactivar cálculo de fechas 

            '// Indicar que se está ingresando una nueva orden para no ejecutar algunas consultas y ejecutar otras que son solamente para el ingreso
            mstrAccionOrden = MSTR_MC_ACCION_INGRESAR

            Dim NewOrden As New OyDOrdenes.Orden

            If plogDuplicar Then
                EditandoComitente = True
                NewOrden.Tipo = pobjDatosOrden.Tipo
                verificarReceptoresOrdenDuplicar()
            Else
                ComitenteSeleccionado = Nothing
                NemotecnicoSeleccionado = Nothing
                ReceptorTomaSeleccionado = Nothing

                ComitenteOrden = String.Empty
                NemotecnicoOrden = String.Empty

                If plogCompra Is Nothing Then
                    NewOrden.Tipo = Nothing
                ElseIf plogCompra Then
                    NewOrden.Tipo = TiposOrden.C.ToString
                    HabilitarDetalleComisiones = False
                Else
                    NewOrden.Tipo = TiposOrden.V.ToString
                    HabilitarDetalleComisiones = True
                End If
            End If

            NewOrden.FechaOrden = Now()
            NewOrden.FechaEstado = Now()
            NewOrden.FechaSistema = Now()
            EditandoComitente = True

            If plogDuplicar Then
                NewOrden.Objeto = pobjDatosOrden.Objeto
                NewOrden.Mercado = pobjDatosOrden.Mercado
                NewOrden.FormaPago = pobjDatosOrden.FormaPago
            Else
                NewOrden.Objeto = ""
                If Me.ClaseOrden = ClasesOrden.C Then
                    NewOrden.Mercado = RF_MERCADO_SECUNDARIO
                    Me.HabilitarMercado = True
                Else
                    NewOrden.Mercado = Nothing
                End If
                NewOrden.FormaPago = mstrFormaPagoPorDef
            End If

            'SLB 20121126
            NewOrden.FechaActualizacion = Now()

            NewOrden.Clase = pobjDatosOrden.Clase
            NewOrden.IDComitente = pobjDatosOrden.IDComitente
            NewOrden.IDOrdenante = pobjDatosOrden.IDOrdenante
            NewOrden.Nemotecnico = pobjDatosOrden.Nemotecnico
            NewOrden.UBICACIONTITULO = pobjDatosOrden.UBICACIONTITULO
            NewOrden.CuentaDeposito = pobjDatosOrden.CuentaDeposito
            NewOrden.Cantidad = pobjDatosOrden.Cantidad
            SaldoOrden = pobjDatosOrden.Cantidad
            NewOrden.CantidadMinima = pobjDatosOrden.CantidadMinima
            NewOrden.CantidadVisible = pobjDatosOrden.CantidadVisible
            NewOrden.FechaEmision = pobjDatosOrden.FechaEmision
            NewOrden.FechaVencimiento = pobjDatosOrden.FechaVencimiento
            'SLB20130731 Manejo de las tasas
            NewOrden.TasaNominal = pobjDatosOrden.TasaNominal
            NewOrden.TasaInicial = pobjDatosOrden.TasaInicial

            NewOrden.Modalidad = pobjDatosOrden.Modalidad
            'NewOrden.Mercado = pobjDatosOrden.Mercado
            NewOrden.TasaCompraVenta = pobjDatosOrden.TasaCompraVenta
            NewOrden.IndicadorEconomico = pobjDatosOrden.IndicadorEconomico
            NewOrden.PuntosIndicador = pobjDatosOrden.PuntosIndicador
            NewOrden.EfectivaInferior = pobjDatosOrden.EfectivaInferior
            NewOrden.EfectivaSuperior = pobjDatosOrden.EfectivaSuperior
            NewOrden.DiasVencimientoInferior = pobjDatosOrden.DiasVencimientoInferior
            NewOrden.DiasVencimientoSuperior = pobjDatosOrden.DiasVencimientoSuperior
            NewOrden.ValorFuturoRepo = pobjDatosOrden.ValorFuturoRepo

            NewOrden.Version = 0
            NewOrden.Ordinaria = pobjDatosOrden.Ordinaria
            NewOrden.Repo = pobjDatosOrden.Repo
            NewOrden.CondicionesNegociacion = pobjDatosOrden.CondicionesNegociacion
            NewOrden.Instrucciones = pobjDatosOrden.Instrucciones
            NewOrden.Notas = pobjDatosOrden.Notas
            NewOrden.TipoInversion = pobjDatosOrden.TipoInversion
            NewOrden.TipoTransaccion = pobjDatosOrden.TipoTransaccion
            NewOrden.Ejecucion = pobjDatosOrden.Ejecucion
            NewOrden.ExentoRetencion = pobjDatosOrden.ExentoRetencion
            If ActivoExento = "SI" Then
                If NewOrden.ExentoRetencion = True Or NewOrden.ExentoRetencion = False Then
                    HabilitarExento = True
                    VisibilidadCampoExento = Visibility.Visible
                Else
                    HabilitarExento = False
                    VisibilidadCampoExento = Visibility.Collapsed
                End If
            Else
                HabilitarExento = False
                VisibilidadCampoExento = Visibility.Collapsed
            End If
            If plogDuplicar = False Then
                VisibilidadOfertaPublica = Visibility.Collapsed
            Else
                If Me._OrdenSelected.Objeto = "OPA" Then
                    Me.VisibilidadOfertaPublica = Visibility.Visible
                Else
                    Me.VisibilidadOfertaPublica = Visibility.Collapsed
                End If
            End If

            NewOrden.ExistePreacuerdo = pobjDatosOrden.ExistePreacuerdo
            NewOrden.VendeTodo = pobjDatosOrden.VendeTodo
            NewOrden.PorcentajePagoEfectivo = pobjDatosOrden.PorcentajePagoEfectivo
            'SLB20140129 Parametrización de la Duración            
            If pobjDatosOrden.Clase = ClasesOrden.A.ToString Then
                If plogDuplicar Then
                    NewOrden.Duracion = pobjDatosOrden.Duracion
                Else
                    NewOrden.Duracion = mstrDuracion
                End If
            Else
                NewOrden.Duracion = String.Empty
            End If

            If NewOrden.Duracion = TipoDuracion.D.ToString Then
                NewOrden.VigenciaHasta = Now.Date
            Else
                NewOrden.VigenciaHasta = OrdenPorDefecto.VigenciaHasta
            End If

            NewOrden.HoraVigencia = pobjDatosOrden.HoraVigencia
            NewOrden.CostoAdicionalesOrden = pobjDatosOrden.CostoAdicionalesOrden
            NewOrden.IdBolsa = pobjDatosOrden.IdBolsa
            NewOrden.Eca = pobjDatosOrden.Eca
            NewOrden.OrdenEscrito = pobjDatosOrden.OrdenEscrito
            'NewOrden.ConsecutivoSwap = pobjDatosOrden.ConsecutivoSwap
            NewOrden.ConsecutivoSwap = Nothing
            NewOrden.NegocioEspecial = pobjDatosOrden.NegocioEspecial
            NewOrden.ComisionPactada = pobjDatosOrden.ComisionPactada
            NewOrden.IDProducto = pobjDatosOrden.IDProducto
            NewOrden.IdCiudadSeteo = pobjDatosOrden.IdCiudadSeteo

            'Santiago Vergara - Octubre 28/2013  
            NewOrden.BrokenTrader = pobjDatosOrden.BrokenTrader

            '/ Estos campos siempre se inicializan con los por defecto
            NewOrden.TieneOrdenRelacionada = OrdenPorDefecto.TieneOrdenRelacionada
            NewOrden.DescripcionOrden = OrdenPorDefecto.DescripcionOrden
            NewOrden.NombreCliente = OrdenPorDefecto.NombreCliente
            NewOrden.SitioIngreso = OrdenPorDefecto.SitioIngreso
            NewOrden.IpOrigen = OrdenPorDefecto.IpOrigen
            NewOrden.AccionMakerAndChecker = OrdenPorDefecto.AccionMakerAndChecker
            NewOrden.EstadoMakerChecker = OrdenPorDefecto.EstadoMakerChecker
            NewOrden.EstadoLEO = OrdenPorDefecto.EstadoLEO
            NewOrden.EstadoOrdenBus = OrdenPorDefecto.EstadoOrdenBus
            NewOrden.Estado = OrdenPorDefecto.Estado
            'SLB20140312 NewOrden.EnPesos = OrdenPorDefecto.EnPesos
            NewOrden.EnPesos = pobjDatosOrden.EnPesos
            NewOrden.DividendoCompra = OrdenPorDefecto.DividendoCompra
            NewOrden.Renovacion = OrdenPorDefecto.Renovacion

            '/ CCM20120305: Para acciones esto campo se inicializan con el por defecto o el del parámetro OYDNet_ORDEN_TIPO_LIMITE_DEFAULT
            If pobjDatosOrden.Clase = ClasesOrden.A.ToString Then
                If Not mstrTipoLimitePorDef.Trim().Equals(String.Empty) And (Not plogDuplicar) Then
                    NewOrden.TipoLimite = mstrTipoLimitePorDef.Trim()
                Else
                    NewOrden.TipoLimite = pobjDatosOrden.TipoLimite
                End If
            Else
                NewOrden.TipoLimite = pobjDatosOrden.TipoLimite
            End If

            '/ CCM20120305: Para acciones estos campos se inicializan con los por defecto o se mantienen los de la orden según parámetro OYDNet_ORDEN_DUPLICAR_DATOS_PRECIO_ACC
            If pobjDatosOrden.Clase = ClasesOrden.A.ToString Then
                If mlogDuplicarDatosPrecio Then
                    NewOrden.Precio = pobjDatosOrden.Precio
                    NewOrden.PrecioStop = pobjDatosOrden.PrecioStop
                Else
                    NewOrden.Precio = OrdenPorDefecto.Precio
                    NewOrden.PrecioStop = OrdenPorDefecto.PrecioStop
                End If
            Else
                NewOrden.Precio = pobjDatosOrden.Precio
                NewOrden.PrecioStop = pobjDatosOrden.PrecioStop
            End If

            '/ CCM20120305: Estos campos se inicializan con los por defecto o se mantienen con los de la orden según parámetro OYDNet_ORDEN_DUPLICAR_DATOS_LEO
            If mlogDuplicarDatosLeo Then
                NewOrden.CanalRecepcion = pobjDatosOrden.CanalRecepcion
                NewOrden.UsuarioOperador = pobjDatosOrden.UsuarioOperador
                NewOrden.MedioVerificable = pobjDatosOrden.MedioVerificable
                NewOrden.IdReceptorToma = pobjDatosOrden.IdReceptorToma
                NewOrden.NroExtensionToma = pobjDatosOrden.NroExtensionToma
            Else
                NewOrden.CanalRecepcion = OrdenPorDefecto.CanalRecepcion
                NewOrden.FechaRecepcion = OrdenPorDefecto.FechaRecepcion
                NewOrden.UsuarioOperador = OrdenPorDefecto.UsuarioOperador
                NewOrden.MedioVerificable = OrdenPorDefecto.MedioVerificable
                NewOrden.IdReceptorToma = OrdenPorDefecto.IdReceptorToma
                NewOrden.NroExtensionToma = OrdenPorDefecto.NroExtensionToma
            End If

            'SLB20130930 Parametrización para BTG.
            If plogDuplicar Then
                If Not logBorrarCamposLeo Then
                    'SLB20140513 Manejo de la fecha de recepción como VB.6  NewOrden.FechaRecepcion = NewOrden.FechaSistema
                    'JDCP20141219 Se quita la condición
                    'If pobjDatosOrden.Clase = ClasesOrden.A.ToString And FECHATOMA_IGUAL_FECHASISTEMA_ACC = "NO" Then
                    'y se reemplaza por el parametro OYDNet_ORDEN_DUPLICAR_FECHA_LEO

                    If mlogDuplicarDatosLeo And mlogDuplicarFechaLeo Then
                        NewOrden.FechaRecepcion = pobjDatosOrden.FechaRecepcion
                    Else
                        NewOrden.FechaRecepcion = NewOrden.FechaSistema
                    End If

                    'JABG20160615
                    If mlogMsgDuplicarDatosLeo Then
                        NewOrden.FechaRecepcion = pobjDatosOrden.FechaRecepcion
                        mostrarMensajePregunta("¿Desea Actualizar la Fecha y Hora de Toma LEO por la actual?", _
                                               Program.TituloSistema, _
                                               "MSGDATOSLEO", _
                                               AddressOf MensajeDatosLeo, False)
                    End If

                End If
            Else
                If Not logBorrarCamposLeo Then
                    NewOrden.FechaRecepcion = NewOrden.FechaSistema
                End If
            End If

            NewOrden.Usuario = Program.Usuario
            NewOrden.UsuarioIngreso = Program.Usuario

            '// Si se está duplicando la orden se activa este indicador para no borrar la lista de receptores y beneficiarios
            mlogDuplicarOrden = plogDuplicar

            OrdenAnterior = OrdenSelected
            OrdenSelected = NewOrden

            'Actualizar los días de vigencia. Se suma uno para incluir la fecha de la orden y la de vigencia
            '_mintDiasVigencia = calcularDiasRango(Me._OrdenSelected.FechaOrden, Me._OrdenSelected.VigenciaHasta)

            'SLB20130621
            If _OrdenSelected.Duracion = TipoDuracion.I.ToString And pobjDatosOrden.Clase = ClasesOrden.A.ToString Then
                DiasVigencia = intDiasVigenciaDuracionInmediata
            Else
                Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
            End If

            MyBase.CambioItem("Ordenes")

            DeshabilitarBotones()
            HabiliarComboUsuarioOperador()

            '// Se desactiva el indicador de cuplicar orden
            mlogDuplicarOrden = False

            Me._mstrNroOrdenSAE = ""
            MyBase.CambioItem("NroOrdenSAE")
            Me._mstrEstadoSAE = ""
            MyBase.CambioItem("EstadoSAE")

            TabSeleccionadoGeneral = OrdenTabs.LEO

            Editando = True
            EditandoInstrucciones = True
            EditandoDetalle = False
            EditandoRegistro = True
            habilitarFechaElaboracion = _mlogHabilitarFechaElaboracion

            MyBase.CambioItem("Editando")

            'SLB
            _strObjetoAnterior = Me._OrdenSelected.Objeto
            _strObjetoAnteriorValidar = " "

            mlogGuardarPagoIncompleto = True
            mlogGuardarSwap = True

            If plogDuplicar And Me.ClaseOrden = ClasesOrden.C Then
                Select Case pobjDatosOrden.Objeto
                    Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_Simultanea_Salida_CRCC, RF_Simultanea_Regreso_CRCC
                        'Case "1", "2", "SI"
                        Me.HabilitarMercado = False
                    Case RF_TTV_Regreso, RF_TTV_Salida, RF_REPO
                        'Case "RP", "3", "4"
                        Me.HabilitarMercado = False
                        Me.OrdenSelected.Repo = True
                    Case Else
                        Me.HabilitarMercado = True
                End Select

                Select Case _OrdenSelected.Tipo
                    Case "V", "S"
                        HabilitarDetalleComisiones = True
                    Case "C", "R"
                        HabilitarDetalleComisiones = False
                        If TabSeleccionadoGeneral = OrdenTabs.Comisiones Then
                            TabSeleccionadoGeneral = OrdenTabs.Pago
                        End If
                End Select
            End If

            logRegistroLibroOrdenes = False
            dblValorCantidadLibroOrden = 0

            If Me.ClaseOrden = ClasesOrden.A Then
                'Jorge Andres Bedoya 20150406
                If Me._OrdenSelected.Objeto = "AD" Then
                    Me.IsEnableComitenteADR = True
                    Me.MostrarComitenteADR = Visibility.Visible
                Else
                    Me.IsEnableComitenteADR = False
                    Me.MostrarComitenteADR = Visibility.Collapsed
                End If
            End If

            If visNavegando = "Collapsed" Then
                MyBase.CambiarFormulario_Forma_Manual()
            End If

            RaiseEvent TerminoConfigurarNuevoRegistro()

        Catch ex As Exception
            mlogRecalcularFechas = True 'CCM20120305. Esta variable controla el cálculo de días entre fechas y solamente se debe actualizar cuando finaliza el cálculo o por error
            mlogDuplicarOrden = False
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en configurar la nueva orden", Me.ToString(), "configurarNuevaOrden", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            '// Presenta la forma para seleccionar si se genera una orden de venta o de compra
            'RaiseEvent seleccionarTipoOrden()
            If Not dcProxy.IsLoading() And Not dcProxy1.IsLoading() And Not mdcProxyUtilidad01.IsLoading() And Not mdcProxyUtilidad02.IsLoading Then
                generarNuevaCompra()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            dcProxy.Ordens.Clear()
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.OrdenesFiltrarQuery(Me.ClaseOrden.ToString, TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, MSTR_ACCION_FILTRAR)
            Else
                dcProxy.Load(dcProxy.OrdenesFiltrarQuery(Me.ClaseOrden.ToString, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "")
            End If
            IsBusy = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaOrden(Me)
        cb.NroOrden = Nothing
        cb.Version = Nothing
        '(SLB) Se adiciona
        mostrarCamposMakerAndChecker(_mlogManejaMakerAndChecker)
        CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Dim strMsg As String = String.Empty
        Dim strAno As String = String.Empty

        Try
            If Not (IsNothing(cb.Tipo) And IsNothing(cb.NroOrden) And IsNothing(cb.Version) And IsNothing(cb.IDComitente) And IsNothing(cb.IDOrdenante) And
                        IsNothing(cb.FechaOrden) And IsNothing(cb.Nemotecnico) And IsNothing(cb.FormaPago) And IsNothing(cb.Objeto) And IsNothing(cb.CondicionesNegociacion) And
                        cb.TipoInversion <> "  " And IsNothing(cb.TipoTransaccion) And IsNothing(cb.CanalRecepcion) And IsNothing(cb.MedioVerificable) And IsNothing(cb.FechaHoraRecepcion) And
                        IsNothing(cb.TipoLimite) And IsNothing(cb.VigenciaHasta) And IsNothing(cb.Ejecucion) And IsNothing(cb.Duracion) And IsNothing(cb.Estado) And IsNothing(cb.EstadoOrdenBus) And
                        IsNothing(cb.IdBolsa) And IsNothing(cb.EstadoMakerChecker)) Then 'IsNothing(cb.TipoInversion)

                If Not IsNothing(cb.NroOrden) Then
                    If Versioned.IsNumeric(cb.NroOrden) = False Then
                        strMsg = "El número de la orden debe ser numérico."
                    ElseIf cb.NroOrden <= 0 Then
                        strMsg = "El número de la orden debe ser mayor que cero."
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    Else
                        If IsNothing(cb.AnoOrden) Then
                            strAno = Now.Year()
                        Else
                            strAno = cb.AnoOrden.ToString
                        End If
                        cb.NroOrden = strAno & Right("000000" & cb.NroOrden.ToString, 6)
                    End If
                End If

                If Not IsNothing(cb.AnoOrden) And IsNothing(cb.NroOrden) Then
                    cb.NroOrden = cb.AnoOrden
                End If

                If Not IsNothing(cb.Version) Then
                    If Versioned.IsNumeric(cb.Version) = False Then
                        strMsg = "La versión de la orden debe ser numérico."
                    ElseIf cb.NroOrden < 0 Then
                        strMsg = "La versión de la orden debe ser mayor o igual que cero."
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                ErrorForma = ""
                dcProxy.Ordens.Clear()
                OrdenAnterior = Nothing
                IsBusy = True
                DescripcionFiltroVM = "" 'PENDIENTE DESCRIPCIÓN FILTRO
                cb.Clase = _mstrClaseOrden.ToString()

                Dim consultaNroOrden As Nullable(Of Integer) = Nothing
                Dim consultaVersion As Nullable(Of Integer) = Nothing
                If Not IsNothing(cb.NroOrden) Then
                    consultaNroOrden = cb.NroOrden
                End If
                If Not IsNothing(cb.Version) Then
                    consultaVersion = cb.Version
                End If

                dcProxy.Load(dcProxy.OrdenesConsultarQuery(cb.Clase, cb.Tipo, consultaNroOrden, consultaVersion, cb.IDComitente, cb.IDOrdenante, cb.FechaOrden, cb.FormaPago, cb.Objeto,
                                                            cb.CondicionesNegociacion, cb.TipoInversion, cb.CanalRecepcion, cb.MedioVerificable,
                                                            cb.FechaHoraRecepcion, cb.TipoLimite, cb.VigenciaHasta, cb.Estado, cb.EstadoOrdenBus, cb.EstadoMakerChecker, cb.AccionMakerChecker,
                                                            Program.Usuario(), Program.HashConexion), AddressOf TerminoTraerOrdenes, MSTR_ACCION_BUSCAR)
                MyBase.ConfirmarBuscar()
            End If
        Catch ex As Exception
            cb = New CamposBusquedaOrden(Me)
            CambioItem("cb")
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConfirmarBuscarEnter(ByVal Tipo As String, ByVal NroOrden As Integer?, ByVal Version As Integer?, ByVal IDComitente As String, ByVal IDOrdenante As String, _
                                    ByVal FechaOrden As Date?, ByVal Nemotecnico As String, ByVal FormaPago As String, ByVal Objeto As String, ByVal CondicionesNegociacion As String, _
                                    ByVal TipoInversion As String, ByVal TipoTransaccion As String, ByVal CanalRecepcion As String, ByVal MedioVerificable As String, ByVal FechaHoraRecepcion As Date?, _
                                    ByVal TipoLimite As String, ByVal VigenciaHasta As Date?, ByVal Ejecucion As String, ByVal Duracion As String, ByVal Estado As String, ByVal EstadoOrdenBus As String, _
                                    ByVal IdBolsa As Integer?, ByVal EstadoMakerChecker As String, ByVal AnoOrden As String, ByVal AccionMakerChecker As String)
        Dim strMsg As String = String.Empty
        Dim strAno As String = String.Empty

        Try
            If Not (IsNothing(Tipo) And IsNothing(NroOrden) And IsNothing(Version) And IsNothing(IDComitente) And IsNothing(IDOrdenante) And
                        IsNothing(FechaOrden) And IsNothing(Nemotecnico) And IsNothing(FormaPago) And IsNothing(Objeto) And IsNothing(CondicionesNegociacion) And
                        TipoInversion <> "  " And IsNothing(TipoTransaccion) And IsNothing(CanalRecepcion) And IsNothing(MedioVerificable) And IsNothing(FechaHoraRecepcion) And
                        IsNothing(TipoLimite) And IsNothing(VigenciaHasta) And IsNothing(Ejecucion) And IsNothing(Duracion) And IsNothing(Estado) And IsNothing(EstadoOrdenBus) And
                        IsNothing(IdBolsa) And IsNothing(EstadoMakerChecker)) Then 'IsNothing(cb.TipoInversion)

                If Not IsNothing(NroOrden) Then
                    If Versioned.IsNumeric(NroOrden) = False Then
                        strMsg = "El número de la orden debe ser numérico."
                    ElseIf NroOrden <= 0 Then
                        strMsg = "El número de la orden debe ser mayor que cero."
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    Else
                        If IsNothing(AnoOrden) Then
                            strAno = Now.Year()
                        Else
                            strAno = AnoOrden.ToString
                        End If
                        NroOrden = strAno & Right("000000" & NroOrden.ToString, 6)
                    End If
                End If

                If Not IsNothing(AnoOrden) And IsNothing(NroOrden) Then
                    NroOrden = AnoOrden
                End If

                If Not IsNothing(Version) Then
                    If Versioned.IsNumeric(Version) = False Then
                        strMsg = "La versión de la orden debe ser numérico."
                    ElseIf NroOrden < 0 Then
                        strMsg = "La versión de la orden debe ser mayor o igual que cero."
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                ErrorForma = ""
                dcProxy.Ordens.Clear()
                OrdenAnterior = Nothing
                IsBusy = True
                DescripcionFiltroVM = "" 'PENDIENTE DESCRIPCIÓN FILTRO
                cb.Clase = _mstrClaseOrden.ToString()
                dcProxy.Load(dcProxy.OrdenesConsultarQuery(cb.Clase, Tipo, NroOrden, Version, IDComitente, IDOrdenante, FechaOrden, FormaPago, Objeto,
                                                            CondicionesNegociacion, TipoInversion, CanalRecepcion, MedioVerificable,
                                                            FechaHoraRecepcion, TipoLimite, VigenciaHasta, Estado, EstadoOrdenBus, EstadoMakerChecker, AccionMakerChecker,
                                                            Program.Usuario(), Program.HashConexion), AddressOf TerminoTraerOrdenes, MSTR_ACCION_BUSCAR)
                MyBase.ConfirmarBuscar()
            End If
        Catch ex As Exception
            cb = New CamposBusquedaOrden(Me)
            CambioItem("cb")
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub guardarOrden(Optional ByVal YaBuscoCuidaSeteo As Boolean = False, Optional ByVal YaValidoCuidadSeteo As Boolean = False,
                             Optional ByVal YaValidoServidor As Boolean = False)

        Dim strLiqAsociadas As String = String.Empty
        Dim strAccion As String = MSTR_MC_ACCION_ACTUALIZAR

        Try
            If mstrValidarCuidadSeteo = "SI" Then
                If Not YaBuscoCuidaSeteo Then
                    If IsNothing(_OrdenSelected.IdCiudadSeteo) Then
                        IsBusy = True
                        dcProxy.ConsultarCuidadSeteo(ListaReceptoresOrdenes.First.IDReceptor, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarCuidadSeteo, "Consulta")
                        Exit Sub
                    Else
                        If CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_CIUDAD_SETEO")).Where(Function(l) l.ID = _OrdenSelected.IdCiudadSeteo).Count > 0 Then
                            mstrCuidadSeteo = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_CIUDAD_SETEO")).Where(Function(l) l.ID = _OrdenSelected.IdCiudadSeteo).First.Descripcion
                        End If
                    End If
                End If

                If Not YaValidoCuidadSeteo Then
                    mostrarMensajePregunta("Ciudad Seteo de la orden: " & mstrCuidadSeteo, _
                       Program.TituloSistema, _
                       "Ciudad Seteo", _
                       AddressOf TerminoPreguntaCiudadSeteo, False)
                    Exit Sub
                End If
            End If

            'SBL20130613 Validaciones de Órdenes en el servidor.
            If Not YaValidoServidor Then
                IsBusy = True
                dcProxy1.tblRespuestaValidaciones.Clear()

                'JWSJ Log para validacion de ordenes
                CrearLogOrdenesValidar("ValidarIngresoOrden_BolsaQuery")

                dcProxy1.Load(dcProxy1.ValidarIngresoOrden_BolsaQuery(_OrdenSelected.IDOrden, _OrdenSelected.NroOrden, _OrdenSelected.Tipo, _OrdenSelected.Clase,
                    _OrdenSelected.IDComitente, _OrdenSelected.FechaOrden, _OrdenSelected.Ordinaria, _OrdenSelected.Objeto, _OrdenSelected.Cantidad,
                    _OrdenSelected.Nemotecnico, _OrdenSelected.UBICACIONTITULO, _OrdenSelected.FechaEmision, _OrdenSelected.FechaVencimiento,
                    _OrdenSelected.Modalidad, _OrdenSelected.TasaInicial, _OrdenSelected.IndicadorEconomico, _OrdenSelected.PuntosIndicador,
                    _OrdenSelected.ValorFuturoRepo, _OrdenSelected.CondicionesNegociacion, _OrdenSelected.FechaCumplimiento, Program.Usuario, _OrdenSelected.VigenciaHasta, Program.HashConexion,
                    _OrdenSelected.TipoLimite, _OrdenSelected.Ejecucion, _OrdenSelected.Duracion), AddressOf TerminoValidarIngresoOrden, "Validar")
                Exit Sub
            End If

            _OrdenSelected.HoraVigencia = asignarHoraVigencia("", False)

            If Not ListaOrdenes.Contains(OrdenSelected) Then
                dcProxy.RejectChanges()
                ListaOrdenes.Add(OrdenSelected)
                strAccion = MSTR_MC_ACCION_INGRESAR
            End If

            'strAccion = MSTR_MC_ACCION_INGRESAR

            If dcProxy.IsLoading OrElse dcProxy1.IsLoading OrElse dcProxyConsultas.IsLoading OrElse mdcProxyUtilidad01.IsLoading OrElse mdcProxyUtilidad02.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema tiene operaciones pendientes antes de guardar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            CrearLogOrdenesReceptores("guardarOrden")

            'JWSJ - codifico en HTML
            _OrdenSelected.ReceptoresXML = HttpUtility.HtmlEncode(_OrdenSelected.ReceptoresXML)
            _OrdenSelected.LiqAsociadasXML = HttpUtility.HtmlEncode(_OrdenSelected.LiqAsociadasXML)
            _OrdenSelected.PagosOrdenesXML = HttpUtility.HtmlEncode(_OrdenSelected.PagosOrdenesXML)
            _OrdenSelected.InstruccionesOrdenesXML = HttpUtility.HtmlEncode(_OrdenSelected.InstruccionesOrdenesXML)
            _OrdenSelected.ComisionesOrdenesXML = HttpUtility.HtmlEncode(_OrdenSelected.ComisionesOrdenesXML)

            If String.IsNullOrEmpty(_OrdenSelected.ReceptoresXML) Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema tiene operaciones pendientes (validar receptores) antes de guardar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la iniciar el proceso de actualización de la orden", Me.ToString(), "guardarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para generar un log unicamente cuando la consola se ejecuta en modo windows
    ''' Error BTG 5263
    ''' </summary>
    ''' <param name="strMetodo"></param>
    ''' <remarks></remarks>
    Private Sub CrearLogOrdenesValidar(ByVal strMetodo As String, Optional ByVal strMasInfo As String = "")
        'creo el log
        CrearLog(String.Format("{3}*** {0} - {1} - {2} - Orden No. {5} {4} {3}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), strMetodo, Program.Usuario, Environment.NewLine, IIf(strMasInfo.Length > 0, String.Concat(Environment.NewLine, strMasInfo), strMasInfo), IIf(_OrdenSelected Is Nothing, "0", _OrdenSelected.NroOrden)))
    End Sub

    ''' <summary>
    ''' Metodo para generar un log unicamente cuando la consola se ejecuta en modo windows
    ''' Error BTG 1292
    ''' </summary>
    ''' <param name="strMetodo">Metodo desde donde se registra el log</param>
    ''' <param name="strMasInfo">Informacion cuando se ejecuta correctamente o el error si este ocurre</param>
    ''' <remarks></remarks>
    Private Sub CrearLogOrdenesReceptores(ByVal strMetodo As String, Optional ByVal strMasInfo As String = "")
        Try
            If _OrdenSelected IsNot Nothing Then
                'genero el mensaje
                Dim strResponse As String = String.Empty
                'cabecera
                strResponse = String.Format("{4}*** {0} - {1} - {2} - {3} - Ordenes:{4}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), strMetodo, Program.Usuario, Me.NombreView, Environment.NewLine)
                'detalle
                strResponse &= String.Format("Orden No. {0} - Receptores:{1}{2}{3}", _OrdenSelected.NroOrden, Environment.NewLine, _OrdenSelected.ReceptoresXML, Environment.NewLine)
                'mas info
                If strMasInfo.Length > 0 Then
                    strResponse &= String.Format("{1}Mas Info:{0}{1}", strMasInfo, Environment.NewLine)
                End If
                'creo el log
                CrearLog(strResponse)
            End If
        Catch ex As Exception
            'se controla el error del log
            System.Diagnostics.Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para escribir el Log
    ''' </summary>
    ''' <param name="strMensaje">Texto a Grabar</param>
    ''' <remarks></remarks>
    Private Sub CrearLog(ByVal strMensaje As String)
        Try
            Dim strFileName As String
                'obtengo el nombre de archivo
                Try
                    strFileName = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Log_OrdenesOyD_" & DateTime.Now.ToString("ddMMyyyy") & ".txt")
                Catch ex As Exception
                    strFileName = String.Empty
                End Try
                If strFileName.Length > 0 Then
                    IO.File.AppendAllText(strFileName, strMensaje)
                End If
        Catch ex As Exception
            'se controla el error del log
            System.Diagnostics.Debug.WriteLine(ex.ToString())
        End Try
    End Sub


    Private Sub TerminoConsultarCuidadSeteo(ByVal lo As InvokeOperation(Of String))
        Try
            IsBusy = False
            If Not lo.HasError Then
                If Not IsNothing(lo.Value) Then
                    Dim res = lo.Value.Split("+")
                    If Not String.IsNullOrEmpty(res(0)) Then
                        If Versioned.IsNumeric(res(0)) Then _OrdenSelected.IdCiudadSeteo = CInt(res(0))
                    End If
                    mstrCuidadSeteo = res(1)
                End If
                guardarOrden(True)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar la ciudad de seteo.", Me.ToString(), "TerminoConsultarCuidadSeteo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar la ciudad de seteo.", Me.ToString(), "TerminoConsultarCuidadSeteo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Private Sub TerminoPreguntaCiudadSeteo(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                guardarOrden(True, True)
            Else
                Me.TabSeleccionadoGeneral = OrdenTabs.Otros
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el guardado de la orden", Me.ToString(), "TerminoPreguntaCiudadSeteo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private _ListaResultadoValidacion As List(Of OyDOrdenes.tblRespuestaValidaciones)
    Public Property ListaResultadoValidacion() As List(Of OyDOrdenes.tblRespuestaValidaciones)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of OyDOrdenes.tblRespuestaValidaciones))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
        End Set
    End Property

    Private Sub TerminoValidarIngresoOrden(ByVal lo As LoadOperation(Of OyDOrdenes.tblRespuestaValidaciones))
        Try
            IsBusy = False
            If lo.HasError = False Then

                ListaResultadoValidacion = lo.Entities.ToList

                If ListaResultadoValidacion.Count > 0 Then
                    Dim logExitoso As Boolean = False
                    Dim logContinuaMostrandoMensaje As Boolean = False
                    Dim logRequiereConfirmacion As Boolean = False
                    Dim logRequiereJustificacion As Boolean = False
                    Dim logRequiereAprobacion As Boolean = False
                    Dim logConsultaListaJustificacion As Boolean = False
                    Dim logError As Boolean = False
                    Dim strMensajeExitoso As String = "La orden se actualizó correctamente."
                    Dim strMensajeError As String = "La orden no se pudo actualizar."

                    For Each li In ListaResultadoValidacion
                        If li.Exitoso Then
                            logExitoso = True
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logRequiereAprobacion = False
                            strMensajeExitoso = String.Format("{0}{1} {2}", strMensajeExitoso, vbCrLf, li.Mensaje)
                        ElseIf li.RequiereConfirmacion Then
                            logExitoso = False
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereConfirmacion = True
                        ElseIf li.RequiereJustificacion Then
                            logExitoso = False
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereJustificacion = True
                        ElseIf li.RequiereAprobacion Then
                            logExitoso = False
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = True
                        ElseIf li.DetieneIngreso Then
                            logError = True
                            logExitoso = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                        Else
                            logError = True
                            logExitoso = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                        End If

                    Next

                    If logExitoso And _
                        logContinuaMostrandoMensaje = False And _
                        logRequiereConfirmacion = False And _
                        logRequiereJustificacion = False And _
                        logRequiereAprobacion = False And _
                        logError = False Then


                        'JWSJ Log para validacion de ordenes
                        CrearLogOrdenesValidar("TerminoValidarIngresoOrden")

                        guardarOrden(True, True, True)
                        'strMensajeExitoso = strMensajeExitoso.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        'strMensajeExitoso = strMensajeExitoso.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        'strMensajeExitoso = strMensajeExitoso.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        'strMensajeExitoso = strMensajeExitoso.Replace("--", String.Format("{0}", vbCrLf))
                    ElseIf logError Then
                        strMensajeError = strMensajeError.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("--", String.Format("{0}", vbCrLf))

                        'JWSJ Log para validacion de ordenes
                        CrearLogOrdenesValidar("TerminoValidarIngresoOrden", strMensajeError)

                        mostrarMensaje(strMensajeError, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                    End If
                Else

                    'JWSJ Log para validacion de ordenes
                    CrearLogOrdenesValidar("TerminoValidarIngresoOrden")

                    guardarOrden(True, True, True)
                End If
            Else
                'JWSJ Log para validacion de ordenes
                CrearLogOrdenesValidar("TerminoValidarIngresoOrden", lo.Error.ToString())

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception

            'JWSJ Log para validacion de ordenes
            CrearLogOrdenesValidar("TerminoValidarIngresoOrden", ex.ToString())

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub


    Private Sub validarOrden()
        Dim strMsg As String = String.Empty
        Dim strReceptores As String = String.Empty
        Dim strLiqProbables As String = String.Empty
        Dim strPagosOrdenes As String = String.Empty
        Dim strInstruccionesOrdenes As String = String.Empty
        Dim strAdicionalesOrdenes As String = String.Empty
        Dim dblPorcentajeComision As Double = 0
        Dim intNroLideres As Integer = 0 'CCM20120305 - Validar que exista un receptor líder seleccionado
        Dim lstReceptores As List(Of String) 'CCM20120305
        Dim lstReceptoresRep As List(Of Object) 'CCM20120305
        Dim lstLiqAsociadas As List(Of Integer)
        Dim lstLiqAsociadasRep As List(Of Object)
        Dim strMsgPago As String = String.Empty
        Dim _strDescTipoOrden As String = String.Empty
        Dim _strDescTipoLimite As String = String.Empty

        Try
            ErrorForma = ""

            If Not IsNothing(_OrdenSelected) Then

                'CORREC_CITI_SV_2014
                If IsNothing(ListaReceptoresOrdenes) Then
                    ListaReceptoresOrdenes = dcProxy1.ReceptoresOrdens
                End If

                If mlogGuardarSwap Then 'SLB Cuando se ejecuta la validacion de Swap el parametro mlogGuardarSwap se pone el False y realiza nuevamente las validaciones de este If

                    If mlogGuardarPagoIncompleto Then 'SLB Cuando se ejecuta la validacion de guardar incompleto los Pagos el parametro mlogGuardarPagoIncompleto se pone el False y realiza nuevamente las validaciones de If

                        If IsNothing(_OrdenSelected.Tipo) Or String.IsNullOrEmpty(_OrdenSelected.Tipo) Then
                            strMsg &= "+ Debe ingresar el tipo de orden (Compra-Venta)." & vbNewLine
                        End If

                        If ComitenteSeleccionado Is Nothing Then
                            strMsg &= "+ Verifique el comitente seleccionado porque no se han podido validar sus datos." & vbNewLine
                        End If

                        If _mobjOrdenanteSeleccionado Is Nothing Then
                            strMsg &= "+ Debe seleccionar el ordenante de la orden." & vbNewLine
                        Else
                            'SLB Validación para saber si el Ordenante de la orden se encuentra Inactivo cuando se esta modificando 
                            If _mobjOrdenanteSeleccionado.IdOrdenante = "-1" Then
                                A2Utilidades.Mensajes.mostrarMensaje("El ordenante se encuentra Inactivo, seleccione otro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                                Exit Sub
                            End If
                            If Not IsNothing(OrdenanteSeleccionado) Then
                                _OrdenSelected.IDOrdenante = OrdenanteSeleccionado.IdOrdenante
                            End If
                        End If

                        If IsNothing(_OrdenSelected.FechaOrden) Then
                            strMsg &= "+ Debe de ingresar la fecha de elaboración." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.VigenciaHasta) Then
                            strMsg &= "+ Debe de ingresar la fecha de vigencia." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.Objeto) Then
                            strMsg &= "+ Debe ingresar la clasificación de la orden." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.TipoLimite) Or String.IsNullOrEmpty(_OrdenSelected.TipoLimite) Then
                            strMsg &= "+ Debe ingresar el tipo límite de la orden." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.CondicionesNegociacion) Or String.IsNullOrEmpty(_OrdenSelected.CondicionesNegociacion) Then
                            strMsg &= "+ Debe ingresar la condición de negociación." & vbNewLine
                        End If

                        If Not strMsg.Equals(String.Empty) Then
                            A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                            Exit Sub
                        End If

                        strMsg = String.Empty

                        If IsNothing(_OrdenSelected.FormaPago) Or String.IsNullOrEmpty(_OrdenSelected.FormaPago) Then
                            'strMsg &= "+ Debe ingresar la forma de pago." & vbNewLine
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la forma de pago.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas

                            Exit Sub
                        End If

                        If NemotecnicoSeleccionado Is Nothing Then
                            strMsg &= "+ Verifique la especie seleccionada porque no se han podido validar sus datos." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.Cantidad) Then 'SLB20132711  Or _OrdenSelected.Cantidad = 0 
                            strMsg &= "+ Debe ingresar " & IIf(Me.ClaseOrden = ClasesOrden.A, "la cantidad.", "el valor nominal") & vbNewLine
                        End If

                        'SLB20130801 Registro de Libro de Órdenes

                        'Santiago Vergara - Octubre 28/2013 - Se modfifica la condición para que permita modificar la cantidad por una que sea igual o mayor a la que se encuentre asociada a una liquidación
                        If _OrdenSelected.Cantidad < (dblValorCantidadLibroOrden - SaldoOrden) And logRegistroLibroOrdenes Then
                            strMsg &= "+ No se puede modificar " & IIf(Me.ClaseOrden = ClasesOrden.A, "la cantidad.", "el valor nominal") & " por un valor inferior al negociado (" & (dblValorCantidadLibroOrden - SaldoOrden) & ")." & vbNewLine
                        End If

                        'SLB20130730 Validación hay que modificarla
                        If _mobjCtaDepositoSeleccionada Is Nothing Then
                            strMsg &= "+ Debe seleccionar una cuenta depósito o ubicación del título para la orden." & vbNewLine
                        Else
                            If logValidarCuentaDeposito Then
                                If _mobjCtaDepositoSeleccionada.Deposito <> MSTR_CTADEPOSITO_TITULO_FISICO And _
                                    _mobjCtaDepositoSeleccionada.Deposito <> MSTR_CTADEPOSITO_TITULO_EXTERIOR And _
                                    IsNothing(_mobjCtaDepositoSeleccionada.NroCuentaDeposito) Then
                                    strMsg &= "+ Debe seleccionar un nro de cuenta depósito para la orden." & vbNewLine
                                Else
                                    _OrdenSelected.CuentaDeposito = CtaDepositoSeleccionada.NroCuentaDeposito
                                    _OrdenSelected.UBICACIONTITULO = CtaDepositoSeleccionada.Deposito
                                End If
                            Else
                                _OrdenSelected.CuentaDeposito = CtaDepositoSeleccionada.NroCuentaDeposito
                                _OrdenSelected.UBICACIONTITULO = CtaDepositoSeleccionada.Deposito
                            End If
                        End If

                        If Not strMsg.Equals(String.Empty) Then
                            A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If

                        strMsg = String.Empty

                        'Inicio LEO
                        If IsNothing(_OrdenSelected.FechaRecepcion) Then
                            strMsg &= "+ Debe de ingresar la fecha de toma." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.CanalRecepcion) Or String.IsNullOrEmpty(_OrdenSelected.CanalRecepcion) Then
                            strMsg &= "+ Debe ingresar el canal de recepción." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.IdReceptorToma) Or String.IsNullOrEmpty(_OrdenSelected.IdReceptorToma) Then
                            strMsg &= "+ Debe ingresar el receptor toma." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.UsuarioOperador) Or String.IsNullOrEmpty(_OrdenSelected.UsuarioOperador) Then
                            strMsg &= "+ Debe ingresar el usuario operador." & vbNewLine
                        End If

                        If IsNothing(_OrdenSelected.MedioVerificable) Or String.IsNullOrEmpty(_OrdenSelected.MedioVerificable) Then
                            strMsg &= "+ Debe ingresar el medio verificable." & vbNewLine
                        Else
                            'SLB20130806
                            If _OrdenSelected.MedioVerificable.Equals("T") Then
                                If IsNothing(_OrdenSelected.NroExtensionToma) Or String.IsNullOrEmpty(_OrdenSelected.NroExtensionToma) Then
                                    strMsg &= "+ Debe ingresar el Nro extensión." & vbNewLine
                                End If
                            End If
                        End If

                        If Not strMsg.Equals(String.Empty) Then
                            Me.TabSeleccionadoGeneral = OrdenTabs.LEO
                            A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                        'Fin LEO

                        If Me._OrdenSelected.FechaOrden() >= Now() Then
                            A2Utilidades.Mensajes.mostrarMensaje("La fecha de elaboración de la orden no puede ser mayor a la fecha actual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If

                        'Especifico Citi
                        If mlogAplicaTipoTransaccion Then
                            If IsNothing(Me._OrdenSelected.TipoTransaccion) Or Me._OrdenSelected.TipoTransaccion = "" Then
                                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el tipo de transacción", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        End If

                        'SLB Validacion Productos Valores
                        If Me._OrdenSelected.FormaPago = "E" Then
                            If IsNothing(Me._OrdenSelected.IDProducto) Then
                                A2Utilidades.Mensajes.mostrarMensaje("El producto financiero es requerido cuando es realizan ordenes en Efectivo. Por favor ingreselo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Me.TabSeleccionadoGeneral = OrdenTabs.Otros
                                Exit Sub
                            ElseIf Me._OrdenSelected.IDProducto.Equals(0) Then
                                A2Utilidades.Mensajes.mostrarMensaje("El producto financiero es requerido cuando es realizan ordenes en Efectivo. Por favor ingreselo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Me.TabSeleccionadoGeneral = OrdenTabs.Otros
                                Exit Sub
                            End If
                        End If

                        If Me.ClaseOrden = ClasesOrden.A Then

                            strMsg = String.Empty

                            If Me._OrdenSelected.Duracion Is Nothing Then
                                strMsg &= "+ La duración de la orden en la bolsa en requerida." & vbNewLine
                            ElseIf Me._OrdenSelected.Duracion.Trim().Equals(String.Empty) Then
                                strMsg &= "+ La duración de la orden en la bolsa en requerida." & vbNewLine
                            End If

                            If Me._OrdenSelected.Ejecucion Is Nothing Then
                                strMsg &= "+ El tipo de ejecución de la orden en la bolsa en requerido." & vbNewLine
                            ElseIf Me._OrdenSelected.Ejecucion.Trim().Equals(String.Empty) Then
                                strMsg &= "+ El tipo de ejecución de la orden en la bolsa en requerido." & vbNewLine
                            End If

                            If Not strMsg.Equals(String.Empty) Then
                                Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If

                            'SLB20130730 
                            If _OrdenSelected.Duracion = "A" And IsNothing(HoraFinVigencia) Then
                                A2Utilidades.Mensajes.mostrarMensaje("Debe llenar el campo hora vigencia (hh:mm:ss).", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                Exit Sub
                            End If

                            'SLB Validar el precio Stop con es una orden Con Límite (L) 'GVB20150910
                            If Not IsNothing(Me._OrdenSelected.TipoLimite) Then
                                If (Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.L.ToString() Or
                                    Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.C.ToString()) And
                                    (IsNothing(_OrdenSelected.Precio) Or _OrdenSelected.Precio = 0) Then

                                    If Me._OrdenSelected.Tipo.ToUpper() = TiposOrden.C.ToString Then
                                        _strDescTipoOrden = "Compra"
                                    ElseIf Me._OrdenSelected.Tipo.ToUpper() = TiposOrden.V.ToString Then
                                        _strDescTipoOrden = "Venta"
                                    Else
                                        _strDescTipoOrden = "N/A"
                                    End If

                                    If Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.L.ToString Then
                                        _strDescTipoLimite = " Con Límite"
                                    ElseIf Me._OrdenSelected.Tipo.ToUpper() = TipoLimite.C.ToString Then
                                        _strDescTipoLimite = "Condicionada"
                                    Else
                                        _strDescTipoLimite = "N/A"
                                    End If

                                    A2Utilidades.Mensajes.mostrarMensaje("La orden de " & _strDescTipoOrden.ToString & " con tipo limite:  " & _strDescTipoLimite.ToString & ", tiene como campo obligatorio el precio. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If

                                'GVB20150910
                                If Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.L.ToString() And (IsNothing(_OrdenSelected.PrecioStop)) Then 'GVB20150910  Me._OrdenSelected.PrecioStop = 0 Or
                                    A2Utilidades.Mensajes.mostrarMensaje("Para Naturaleza Con Límite, el Precio Stop es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If

                                'GVB20150910
                                If Me._OrdenSelected.Tipo.ToUpper() = TiposOrden.C.ToString() And (_OrdenSelected.PrecioStop < _OrdenSelected.Precio) Then 'GVB20150910  Me._OrdenSelected.PrecioStop = 0 Or
                                    A2Utilidades.Mensajes.mostrarMensaje("El precio Stop debe ser mayor o igual al precio de la orden para compras.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If

                                'GVB20150910
                                If Me._OrdenSelected.Tipo.ToUpper() = TiposOrden.V.ToString() And (_OrdenSelected.PrecioStop > _OrdenSelected.Precio) Then 'GVB20150910  Me._OrdenSelected.PrecioStop = 0 Or
                                    A2Utilidades.Mensajes.mostrarMensaje("El precio Stop debe ser menor o igual al precio de la orden para ventas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If


                                If Not IsNothing(Me._OrdenSelected.Ejecucion) And Not IsNothing(Me._OrdenSelected.Duracion) Then
                                    If Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.M.ToString() And (Me._OrdenSelected.Ejecucion.ToUpper() <> TipoEjecucion.N.ToString Or Me._OrdenSelected.Duracion.ToUpper() <> TipoDuracion.I.ToString()) Then
                                        A2Utilidades.Mensajes.mostrarMensaje("Para órdenes a mercado la duración debe ser Inmediata y el tipo de ejecución Ninguna. El sistema asignó estos valores a estas propiedades de la orden." & vbNewLine & vbNewLine & "Si está de acuerdo guarde nuevamente la orden o de lo contrario haga las modificaciones necesarias en la naturaleza, duración y ejecución de la orden y luego guardela.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        _OrdenSelected.Duracion = TipoDuracion.I.ToString()
                                        _OrdenSelected.Ejecucion = TipoEjecucion.N.ToString()
                                        Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                        Exit Sub
                                    End If
                                End If

                            End If

                            'SLB20130111
                            If Me._OrdenSelected.Objeto <> "IC" Then
                                If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                                    A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If
                            End If

                            'Jorge Andres Bedoya 20150406
                            If Me._OrdenSelected.Objeto = "AD" Then
                                If Me._OrdenSelected.IDComitenteADR = "" Then
                                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ADR es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Me.TabSeleccionadoGeneral = OrdenTabs.Otros
                                    Exit Sub
                                End If
                            End If

                            If Me._OrdenSelected.Objeto = "OPA" Then
                                If Me._OrdenSelected.ExistePreacuerdo = "" Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Debe escoger un valor en Existe Preacuerdo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                    Exit Sub
                                End If
                            End If

                            If Me._OrdenSelected.Objeto = "OPA" Then
                                If Me._OrdenSelected.VendeTodo = "" Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Debe escoger un valor en Vende Todo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                    Exit Sub
                                End If
                            End If

                            If Me._OrdenSelected.Objeto = "OPA" Then
                                If Me._OrdenSelected.PorcentajePagoEfectivo Is Nothing Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Debe digitar un porcentaje de pago efectivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                    Exit Sub
                                End If
                            End If

                            If Me._OrdenSelected.Objeto = "OPA" Then
                                If Me._OrdenSelected.PorcentajePagoEfectivo < 0 Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Debe digitar un porcentaje de pago efectivo entre 0 y 100.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                    Exit Sub
                                End If
                            End If

                            'CCM20120305 - Validar precio stop solamente si es mayor que cero. Si es cero no se ejecutan las validaciones
                            If Me._OrdenSelected.PrecioStop > 0 Then
                                If Me._OrdenSelected.Precio < Me._OrdenSelected.PrecioStop And Me._OrdenSelected.Tipo.ToUpper() = TiposOrden.V.ToString() Then
                                    A2Utilidades.Mensajes.mostrarMensaje("El precio stop para una orden de venta debe ser menor que el precio de venta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                ElseIf Me._OrdenSelected.Precio > Me._OrdenSelected.PrecioStop And Me._OrdenSelected.Tipo.ToUpper() = TiposOrden.C.ToString() Then
                                    A2Utilidades.Mensajes.mostrarMensaje("El precio stop para una orden de compra debe ser mayor que el precio de compra.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If
                            End If

                            If Not IsNothing(Me._OrdenSelected.Ejecucion) Then
                                If Me._OrdenSelected.Ejecucion.ToUpper() = TipoEjecucion.C.ToString() And (IsNothing(_OrdenSelected.CantidadMinima)) Then 'SLB20131127 Me._OrdenSelected.CantidadMinima = 0 Or
                                    A2Utilidades.Mensajes.mostrarMensaje("Cuando el tipo de ejecución es cantidad mínima se debe indicar el valor de la cantidad mínima", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                    Exit Sub
                                ElseIf Me._OrdenSelected.Ejecucion.ToUpper() <> TipoEjecucion.C.ToString() Then
                                    Me._OrdenSelected.CantidadMinima = 0
                                End If

                                If Me._OrdenSelected.Ejecucion.ToUpper() = TipoEjecucion.C.ToString() And Me._OrdenSelected.CantidadMinima > Me._OrdenSelected.Cantidad Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Cuando el tipo de ejecución es cantidad mínima, la cantidad mínima debe ser menor o igual a la cantidad de la orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                    Exit Sub
                                End If
                            End If

                            If Me._OrdenSelected.CantidadVisible <> 0 Then

                                If Me._OrdenSelected.CantidadVisible < (Me._OrdenSelected.Cantidad) * (intPorcentajeCantidadVisible / 100.0#) Then
                                    A2Utilidades.Mensajes.mostrarMensaje("La cantidad visible no puede ser menor al " & CStr(intPorcentajeCantidadVisible) & "% de la cantidad de la Orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If

                                If Me._OrdenSelected.CantidadVisible >= Me._OrdenSelected.Cantidad Then
                                    A2Utilidades.Mensajes.mostrarMensaje("La cantidad visible debe ser menor que la cantidad de la orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If
                            End If

                            If Not IsNothing(Me._OrdenSelected.TipoLimite) Then
                                If Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.M.ToString() And (Me._OrdenSelected.Precio > 0) Then 'Or IsNothing(_OrdenSelected.Precio)
                                    A2Utilidades.Mensajes.mostrarMensaje("Para órdenes a mercado no se debe establecer precio. Se asignará cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    _OrdenSelected.Precio = 0
                                    _OrdenSelected.PrecioStop = 0
                                End If
                            End If

                        Else 'Validaciones de Renta Fija

                            If _OrdenSelected.Tipo = TiposOrden.R.ToString() Or _OrdenSelected.Tipo = TiposOrden.S.ToString() Then
                                strMsg = "Los tipos de orden Recompra y Reventa son generadas en forma automática por el sistema cuando la orden lo requiere (simultáneas, repos, etc.)." & vbNewLine & vbNewLine & "Debe cambiar el tipo de la orden a compra o venta."
                                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If

                            'If Me._OrdenSelected.Cantidad <= 0 Then
                            '    strMsg = "+ El valor nominal debe ser mayor que cero."
                            'End If

                            If Me._OrdenSelected.Mercado Is Nothing Then
                                strMsg = "+ El mercado en el cual se negocia la orden es requerido."
                            End If

                            'SLB 
                            If Me._OrdenSelected.Objeto = "  " Then
                                strMsg = "Debe seleccionar la clasificación de la orden"
                            End If

                            If Me._OrdenSelected.Objeto = RF_TTV_Regreso Then
                                strMsg = "No se permiten hacer ordenes de tipo TTV de regreso, por favor cree primero la orden de salida"
                            End If

                            If Me._OrdenSelected.Objeto = RF_Simulatena_Regreso Then
                                strMsg = "No se permiten hacer ordenes de tipo Simultanea de regreso, por favor cree primero la orden de salida"
                            End If

                            If Me._OrdenSelected.Objeto = RF_Simultanea_Regreso_CRCC Then
                                strMsg = "No se permiten hacer ordenes de tipo Simultanea de regreso CRCC, por favor cree primero la orden de salida"
                            End If

                            Select Case _OrdenSelected.Objeto
                                Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_Simultanea_Salida_CRCC, RF_Simultanea_Regreso_CRCC
                                    'Case "1", "2", "SI", "3", "4"
                                    If Not Me.OrdenSelected.Mercado = RF_MERCADO_SECUNDARIO Then
                                        strMsg = "Este tipo de Orden debe ser de Mercado Secundario"
                                        Me.OrdenSelected.Mercado = RF_MERCADO_SECUNDARIO
                                        Me.HabilitarMercado = False
                                    End If
                                Case RF_REPO
                                    'Case "RP"
                                    If Not Me.OrdenSelected.Mercado = RF_MERCADO_REPO Then
                                        strMsg = "Este tipo de Orden debe ser de Mercado REPO"
                                        Me.OrdenSelected.Mercado = RF_MERCADO_REPO
                                        Me.HabilitarMercado = False
                                    End If
                                Case Else
                                    If Me.OrdenSelected.Mercado = RF_MERCADO_REPO Then
                                        strMsg = "Este tipo de Orden no puede ser de Mercado REPO"
                                        Me.OrdenSelected.Mercado = RF_MERCADO_PRIMARIO
                                        Me.HabilitarMercado = True
                                    End If
                            End Select

                            If _strObjetoAnteriorValidar <> " " Then
                                If _strObjetoAnteriorValidar <> _OrdenSelected.Objeto Then
                                    Select Case _strObjetoAnteriorValidar
                                        Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO, RF_Simultanea_Salida_CRCC, RF_Simultanea_Regreso_CRCC
                                            'Case "1", "2", "SI", "3", Program.RF_TTV_Regreso, "RP"
                                            If (_OrdenSelected.Objeto <> RF_Simulatena_Salida Or _OrdenSelected.Objeto <> RF_Simulatena_Regreso Or _OrdenSelected.Objeto <> "SI" Or _OrdenSelected.Objeto <> RF_TTV_Salida _
                                                Or _OrdenSelected.Objeto <> RF_TTV_Regreso Or _OrdenSelected.Objeto <> RF_REPO Or _OrdenSelected.Objeto <> RF_Simultanea_Salida_CRCC Or _OrdenSelected.Objeto <> RF_Simultanea_Regreso_CRCC) Then
                                                strMsg = "EL tipo de Clasificación de esta Orden no se puede modificar debido a que tiene una punta asociada (Orden No. " & CStr(_OrdenSelected.NroOrden + 1) & ")"
                                            End If
                                        Case Else
                                            If (_OrdenSelected.Objeto = RF_Simulatena_Salida Or _OrdenSelected.Objeto = RF_Simulatena_Regreso Or _OrdenSelected.Objeto = "SI" Or _OrdenSelected.Objeto = RF_TTV_Salida _
                                                Or _OrdenSelected.Objeto = RF_TTV_Regreso Or _OrdenSelected.Objeto = RF_REPO Or _OrdenSelected.Objeto = RF_Simultanea_Salida_CRCC Or _OrdenSelected.Objeto = RF_Simultanea_Regreso_CRCC) Then
                                                strMsg = "EL tipo de Clasificación de esta Orden no se puede modificar debido a que no tiene una punta asociada."
                                            End If
                                    End Select
                                End If
                            End If

                            'SLB20130621
                            If IsNothing(_OrdenSelected.TasaInicial) And _OrdenSelected.Objeto = RF_REPO Then 'SLB20131127 _OrdenSelected.TasaNominal = 0 
                                strMsg = "La tasa nominal es requerida"
                            End If

                            If Not strMsg.Equals(String.Empty) Then
                                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If

                            'SLB Validar efectiva inferior o superior si es una orden con Tipo Limite 'Con Límite (L)'
                            If Not IsNothing(Me._OrdenSelected.TipoLimite) Then
                                If Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.L.ToString() Then
                                    If Me._OrdenSelected.Tipo = TiposOrden.C.ToString() And (IsNothing(Me._OrdenSelected.EfectivaSuperior)) Then 'SLB20131127 Me._OrdenSelected.EfectivaSuperior = 0 Or 
                                        A2Utilidades.Mensajes.mostrarMensaje("La orden de compra con tipo limite 'Con Límite' tiene como campo obligatoria efectiva superior", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                        Exit Sub
                                    End If

                                    If Me._OrdenSelected.Tipo = TiposOrden.V.ToString() And (IsNothing(Me._OrdenSelected.EfectivaInferior)) Then 'SLB20131127 Me._OrdenSelected.EfectivaInferior = 0 Or 
                                        A2Utilidades.Mensajes.mostrarMensaje("La orden de venta con tipo limite 'Con Límite' tiene como campo obligatoria efectiva inferior", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                        Exit Sub
                                    End If
                                End If
                            End If

                            'SLB20130730
                            If _OrdenSelected.Objeto <> RF_TTV_Salida And _OrdenSelected.Objeto <> RF_TTV_Regreso And IsNothing(_OrdenSelected.TasaInicial) Then
                                A2Utilidades.Mensajes.mostrarMensaje("La tasa nominal es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If

                            'SLB20130731 Manejo de las faciales dependiendo el tipo de Especie Seleccionada.
                            If (_OrdenSelected.Tipo = TiposOrden.V.ToString() Or _OrdenSelected.Tipo = TiposOrden.S.ToString()) And Not mlogEspecieAccion Then
                                If IsNothing(_OrdenSelected.FechaEmision) Then
                                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de emisión es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If

                                If IsNothing(_OrdenSelected.FechaVencimiento) Then
                                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de vencimiento es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If


                                'SLB20131018 Inicio Revisión con usuario estos campos no son obligatorios en VB.6 no se utilizan para complementar.

                                'If String.IsNullOrEmpty(_OrdenSelected.Modalidad) Then
                                '    A2Utilidades.Mensajes.mostrarMensaje("La modalidad es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                '    Exit Sub
                                'End If

                                'Select Case mstrEspecieTipoTasa
                                '    Case "F"
                                '        If _OrdenSelected.TasaNominal = 0 Then
                                '            A2Utilidades.Mensajes.mostrarMensaje("La tasa nominal debe ser mayor a cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                '            Exit Sub
                                '        End If
                                '    Case "V"
                                '        If String.IsNullOrEmpty(_OrdenSelected.IndicadorEconomico) Then
                                '            A2Utilidades.Mensajes.mostrarMensaje("El indicador económico es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                '            Exit Sub
                                '        End If
                                '        If IsNothing(_OrdenSelected.PuntosIndicador) Then
                                '            A2Utilidades.Mensajes.mostrarMensaje("Los puntos indicador son requeridos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                '            Exit Sub
                                '        End If
                                'End Select

                            End If

                            'SLB20130111
                            If Me._OrdenSelected.Objeto <> "IC" Or Me.ComitenteSeleccionado.CodCategoria <> "2" Then
                                If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                                    A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado  y el cliente debe ser Inversionista Profesional.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                End If
                            End If

                            'SLB20130731 
                            If Me._OrdenSelected.Mercado = RF_MERCADO_PRIMARIO And Me._OrdenSelected.Objeto <> "MP" Then
                                A2Utilidades.Mensajes.mostrarMensaje("Cuando el mercado es primario, la clasificación debe ser Mercado Primario", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Me.OrdenSelected.Objeto = "MP"
                                Exit Sub
                            End If


                            If logValorCompromisoFuturoRequerido = False Then
                                'Si la Orden es de Venta-Repo y el patrimonio técnico de la Firma fue defindo en tblParametros, realizamos estas validaciones:
                                If _OrdenSelected.Tipo = TiposOrden.V.ToString() And (Me._OrdenSelected.Repo Or Me._OrdenSelected.Objeto = RF_Simulatena_Salida Or Me.RF_Simultanea_Salida_CRCC) And dblPatrimonioTecnico > 0 Then
                                    If IsNothing(_OrdenSelected.ValorFuturoRepo) Then 'SLB20131127 _OrdenSelected.ValorFuturoRepo = 0 Or  
                                        A2Utilidades.Mensajes.mostrarMensaje("Debe especificar el valor de compromiso futuro en pesos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        MostrarValorFuturoRepo = Visibility.Visible
                                        Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                        Exit Sub
                                    End If
                                End If
                            Else

                                'Jorge Andrés Bedoya 2014/12/29
                                If (Me._OrdenSelected.Repo Or Me._OrdenSelected.Objeto = RF_Simulatena_Salida Or Me._OrdenSelected.Objeto = RF_Simulatena_Regreso _
                                    Or Me._OrdenSelected.Objeto = RF_TTV_Salida Or Me._OrdenSelected.Objeto = RF_TTV_Regreso Or Me._OrdenSelected.Objeto = RF_Simultanea_Salida_CRCC Or Me._OrdenSelected.Objeto = RF_Simultanea_Regreso_CRCC) And dblPatrimonioTecnico > 0 Then

                                    If IsNothing(_OrdenSelected.ValorFuturoRepo) Or _OrdenSelected.ValorFuturoRepo = 0 Then
                                        A2Utilidades.Mensajes.mostrarMensaje("Debe especificar el valor de compromiso futuro en pesos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        MostrarValorFuturoRepo = Visibility.Visible
                                        Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                                        Exit Sub
                                    End If

                                End If

                            End If

                        End If

                        If mlogIngresarFechaTomaCierre = False Then
                            If Not Me._OrdenSelected.FechaRecepcion Is Nothing AndAlso Me._OrdenSelected.FechaRecepcion < mdtmFechaCierreSistema Then
                                strMsg = "La fecha de toma (fecha de recepción de la orden) no puede ser menor a la fecha de cierre del sistema."
                                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        End If

                        If mlogModificarechaTomaIgualSistema = False Then
                            If Me._OrdenSelected.FechaRecepcion > Me._OrdenSelected.FechaSistema Then
                                strMsg = "La fecha ó hora LEO de ingreso es mayor que la fecha ó hora del sistema."
                                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Me.TabSeleccionadoGeneral = OrdenTabs.LEO
                                Exit Sub
                            End If
                        Else
                            If Me._OrdenSelected.FechaRecepcion > Now Then
                                strMsg = "La fecha ó hora LEO de ingreso es mayor que la fecha ó hora actual."
                                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Me.TabSeleccionadoGeneral = OrdenTabs.LEO
                                Exit Sub
                            End If

                        End If



                        lstReceptores = New List(Of String) 'CCM20120305 
                        lstReceptoresRep = New List(Of Object) 'CCM20120305 

                        strReceptores = "<receptores>"
                        If Not IsNothing(ListaReceptoresOrdenes) Then
                            If ListaReceptoresOrdenes.Count > 0 Then
                                For Each obj In ListaReceptoresOrdenes
                                    If Not lstReceptores.Contains(obj.IDReceptor) And (obj.Porcentaje > 0 Or obj.Lider) Then 'CCM20120305 - Validar receptores duplicados y solamente considerar el primero
                                        lstReceptores.Add(obj.IDReceptor)
                                        dblPorcentajeComision += obj.Porcentaje

                                        If IsNothing(obj.IDReceptor) Then
                                            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el receptor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                            Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                                            Exit Sub
                                        End If

                                        If obj.Lider Then 'CCM20120305 - Validar que solamente exista un lider
                                            intNroLideres += 1
                                        End If

                                        strReceptores &= "<receptor Id=""" & obj.IDReceptor & """ Lider=""" & IIf(obj.Lider, "1", "0") & """ Porcentaje=""" & obj.Porcentaje.ToString() & """ />"
                                    Else
                                        lstReceptoresRep.Add(obj) 'CCM20120305 - Guardar receptores duplicados
                                    End If
                                Next
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los receptores de la orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                                Exit Sub
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los receptores de la orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                            Exit Sub
                        End If


                        strReceptores &= "</receptores>"

                        If dblPorcentajeComision <> 100.0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("El porcentaje de distribución de comisión entre los receptores de la orden debe ser cien (100). En este momento está en " & dblPorcentajeComision.ToString("N0"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                            Exit Sub
                        End If

                        'CCM20120305 - Validar que solamente exista un lider y que por lo menos exista uno seleccionado
                        If intNroLideres = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se debe seleccionar un receptor líder para la distribución de la comisión.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                            Exit Sub
                        ElseIf intNroLideres > 1 Then
                            A2Utilidades.Mensajes.mostrarMensaje("Solamente puede seleccionarse un receptor líder para la distribución de la comisión. En este momento hay " & intNroLideres.ToString("N0") & " receptores seleccionados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                            Exit Sub
                        End If

                        'CCM20120305 - Eliminar receptores duplicados (se deja solamnete el primero en el grid)
                        If lstReceptoresRep.Count > 0 Then
                            For Each obj In lstReceptoresRep
                                ListaReceptoresOrdenes.Remove(obj)
                            Next
                        End If

                        _OrdenSelected.ReceptoresXML = strReceptores

                        '(SLB) Se adiciona la lista de Liquidaciones Probables para la orden
                        lstLiqAsociadas = New List(Of Integer)
                        lstLiqAsociadasRep = New List(Of Object)

                        strLiqProbables = "<liqprobables>"
                        If Not IsNothing(ListaLiqAsociadasOrdenes) Then
                            If ListaLiqAsociadasOrdenes.Count > 0 Then

                                'SLB20130730
                                If Me.ClaseOrden = ClasesOrden.A And ListaLiqAsociadasOrdenes.Count > 1 Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Para las Ordenes de Acciones no se pueden ingresar mas de una liquidación probable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Me.TabSeleccionadoGeneral = 7
                                    Exit Sub
                                End If

                                For Each obj In ListaLiqAsociadasOrdenes
                                    If IsNothing(obj.FechaLiquidacion) Then
                                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la Fecha de Liquidacion probable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        If Me.ClaseOrden = ClasesOrden.A Then
                                            Me.TabSeleccionadoGeneral = 7
                                        Else
                                            Me.TabSeleccionadoGeneral = OrdenTabs.LiqPosibles
                                        End If
                                        Exit Sub
                                    End If
                                    If obj.NroLiquidacion = 0 Then
                                        A2Utilidades.Mensajes.mostrarMensaje("El número de liquidación debe ser diferente a Cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        If Me.ClaseOrden = ClasesOrden.A Then
                                            Me.TabSeleccionadoGeneral = 7
                                        Else
                                            Me.TabSeleccionadoGeneral = OrdenTabs.LiqPosibles
                                        End If
                                        Exit Sub
                                    End If

                                    If Not (lstLiqAsociadas.Contains(obj.NroLiquidacion)) Then 'CCM20120305 - Validar receptores duplicados y solamente considerar el primero
                                        lstLiqAsociadas.Add(obj.NroLiquidacion)

                                        strLiqProbables &= "<liqprobable Id=""" & CStr(obj.NroLiquidacion) & """ Parcial=""" & CStr(obj.Parcial) & """ FechaLiq=""" & String.Format("{0:s}", obj.FechaLiquidacion) & """ />"
                                    Else
                                        lstLiqAsociadasRep.Add(obj) 'CCM20120305 - Guardar receptores duplicados
                                    End If
                                Next
                            Else
                                strLiqProbables &= "<liqprobable Id=""" & CStr(0) & """ Parcial=""" & CStr(-1) & """ FechaLiq=""" & String.Format("{0:s}", Now) & """ />"
                            End If
                        Else
                            strLiqProbables &= "<liqprobable Id=""" & CStr(0) & """ Parcial=""" & CStr(-1) & """ FechaLiq=""" & String.Format("{0:s}", Now) & """ />"
                        End If

                        strLiqProbables &= "</liqprobables>"

                        If Not IsNothing(lstLiqAsociadasRep) Then
                            If lstLiqAsociadasRep.Count > 0 Then
                                For Each obj In lstLiqAsociadasRep
                                    ListaLiqAsociadasOrdenes.Remove(obj)
                                Next
                            End If
                        End If

                        _OrdenSelected.LiqAsociadasXML = strLiqProbables

                        '(SLB) Se adiciona la lista de Instrucciones para la orden
                        lstLiqAsociadas = New List(Of Integer)
                        lstLiqAsociadasRep = New List(Of Object)

                        strInstruccionesOrdenes = "<instrucciones>"
                        If Not IsNothing(ListaInstruccionesOrdenes) Then
                            If ListaInstruccionesOrdenes.Count > 0 Then
                                For Each obj In ListaInstruccionesOrdenes
                                    'If Not IsNothing(obj.Cuenta) And Not IsNothing(obj.Valor) Then
                                    If IsNothing(obj.Valor) Then
                                        obj.Valor = 0
                                    End If
                                    If obj.Cuenta <> "" Or obj.Valor <> 0 Or obj.Seleccionado Then
                                        strInstruccionesOrdenes &= "<instruccione strRetorno=""" & CStr(obj.Retorno) & """ strInstruccion=""" & CStr(obj.Instruccion) & """ strCuenta=""" & CStr(obj.Cuenta) & """  dblValor=""" & CStr(obj.Valor) & """ logSeleccionado=""" & CStr(obj.Seleccionado) & """ />"
                                    End If
                                    'End If
                                Next
                            End If
                        End If

                        strInstruccionesOrdenes &= "</instrucciones>"

                        If Not IsNothing(lstLiqAsociadasRep) Then
                            If lstLiqAsociadasRep.Count > 0 Then
                                For Each obj In lstLiqAsociadasRep
                                    ListaInstruccionesOrdenes.Remove(obj)
                                Next
                            End If
                        End If

                        _OrdenSelected.InstruccionesOrdenesXML = strInstruccionesOrdenes

                    End If


                    If Me.ClaseOrden = ClasesOrden.C Then
                        '(SLB) Se adiciona la lista de Pagos para la orden
                        If Not IsNothing(ListaOrdenesPagos) Then
                            For Each obj In ListaOrdenesPagos
                                obj.CtaSebraPesos = IIf(IsNothing(obj.CtaSebraPesos), String.Empty, obj.CtaSebraPesos)
                                obj.CtaTitulo = IIf(IsNothing(obj.CtaTitulo), String.Empty, obj.CtaTitulo)
                                obj.DeposPesos = IIf(IsNothing(obj.DeposPesos), String.Empty, obj.DeposPesos)

                                If Not obj.CtaSebraPesos.Equals(String.Empty) Or Not obj.CtaTitulo.Equals(String.Empty) _
                                     Or Not obj.CtaSebraPesos.Equals(String.Empty) Or mstrPago = "SI" Then
                                    If obj.CtaSebraPesos.Equals(String.Empty) And mlogGuardarPagoIncompleto Then
                                        strMsgPago = "Información del Pago de la orden esta incompleta."
                                        'C1.Silverlight.C1MessageBox.Show(strMsgPago, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarGuardarPagoIncompleto)
                                        mostrarMensajePregunta(strMsgPago, _
                                                               Program.TituloSistema, _
                                                               "INFORMACIONPAGO", _
                                                               AddressOf validarGuardarPagoIncompleto, True, "¿Desea continuar?")
                                        Exit Sub
                                    Else
                                        mlogGuardarPagoIncompleto = True
                                        strPagosOrdenes = "<pagosordenes>"
                                        strPagosOrdenes &= "<pagosordene strFormaPago=""" & _OrdenesPagoSelected.FormaPago & """ strCumpPesos=""" & _OrdenesPagoSelected.CumpPesos & """ strCtaSebraPesos=""" & _OrdenesPagoSelected.CtaSebraPesos & """ strCtaTitulo=""" & _OrdenesPagoSelected.CtaTitulo & """ strDeposPesos=""" & _OrdenesPagoSelected.DeposPesos & """ />"
                                        strPagosOrdenes &= "</pagosordenes>"
                                    End If
                                End If
                            Next
                        End If

                        '(SLB) Se adiciona la lista de Comisiones para la orden
                        Select Case _OrdenSelected.Tipo
                            Case "V", "S"
                                If Not IsNothing(ListaAdicionalesOrdenes) Then
                                    If ListaAdicionalesOrdenes.Count > 0 Then
                                        For Each obj In ListaAdicionalesOrdenes
                                            If obj.IdComision.Equals(String.Empty) Or obj.IdOperacion.Equals(0) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("La información de las comisiones es toda requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                Me.TabSeleccionadoGeneral = OrdenTabs.Comisiones
                                                Exit Sub
                                            End If
                                            If (obj.PorcCompra + obj.PorcVenta + obj.PorcOtro) <> 100 Then
                                                A2Utilidades.Mensajes.mostrarMensaje("La suma de las comisiones debe totalizar 100%.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                Me.TabSeleccionadoGeneral = OrdenTabs.Comisiones
                                                Exit Sub
                                            End If
                                            strAdicionalesOrdenes = "<comisiones>"
                                            strAdicionalesOrdenes &= "<comisione lngIdComision=""" & CStr(obj.IdComision) & """ dblPorcCompra=""" & CStr(obj.PorcCompra) & _
                                                                    """ dblPorcVenta=""" & CStr(obj.PorcVenta) & """  dblPorcOtro=""" & CStr(obj.PorcOtro) & _
                                                                    """ lngIdOperacion=""" & CStr(obj.IdOperacion) & """ dblComisionSugerida=""" & CStr(obj.ComisionSugerida) & """ />"
                                            strAdicionalesOrdenes &= "</comisiones>"
                                        Next
                                    End If
                                End If
                            Case "C", "R"
                                If Not IsNothing(ListaAdicionalesOrdenes) Then
                                    ListaAdicionalesOrdenes.Clear()
                                End If
                        End Select

                    End If

                    _OrdenSelected.PagosOrdenesXML = strPagosOrdenes
                    _OrdenSelected.ComisionesOrdenesXML = strAdicionalesOrdenes

                End If

                'SLB se adiciona la pregunta si la Orden de Renta Fija es Swap
                If Me.ClaseOrden = ClasesOrden.C And Me._OrdenSelected.Objeto = RF_SWAP Then
                    If Not IsNothing(ListaOrdenes) Then
                        If (Not ListaOrdenes.Contains(OrdenSelected)) And mlogGuardarSwap Then
                            'C1.Silverlight.C1MessageBox.Show("Si esta Orden es la última del Swap." & vbCrLf & "Este se debe numerar" & vbCrLf & "Desea realizar este proceso?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaSwap)
                            mostrarMensajePregunta("Si esta Orden es la última del Swap." & vbCrLf & "Este se debe numerar" & vbCrLf & "Desea realizar este proceso?", _
                                                   Program.TituloSistema, _
                                                   "SWAP", _
                                                   AddressOf TerminoPreguntaSwap, False)
                            Exit Sub
                        Else
                            mlogGuardarSwap = True
                        End If
                    End If
                End If

                'SLB si el parametro mlogAplicaExcepcionRDIP esta en True valida la Excepcion RDIP (Especifico Citi)
                If mlogAplicaExcepcionRDIP Then
                    strMsg = validarExcepcionRiesgo()
                End If


                If strMsg.Equals(String.Empty) Then
                    mstrCuidadSeteo = String.Empty
                    guardarOrden()
                Else
                    'C1.Silverlight.C1MessageBox.Show(strMsg, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarExcepcionRDIPGuardar)
                    mostrarMensajePregunta(strMsg, _
                                           Program.TituloSistema, _
                                           "INFORMACIONPAGO", _
                                           AddressOf validarExcepcionRDIPGuardar, True, "¿Desea continuar?")
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la orden", Me.ToString(), "validarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TerminoPreguntaSwap(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                mlogGuardarSwap = False
                validarOrden()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el guardado de la orden", Me.ToString(), "validarExcepcionRDIPGuardar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim strMsg As String = String.Empty

            If IsNothing(_OrdenSelected.VigenciaHasta) Then
                strMsg &= "+Debe de ingresar la fecha de vigencia." & vbNewLine
            End If

            If Not strMsg.Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            If dcProxy.IsLoading OrElse dcProxy1.IsLoading OrElse dcProxyConsultas.IsLoading OrElse mdcProxyUtilidad01.IsLoading OrElse mdcProxyUtilidad02.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema tiene operaciones pendientes antes de guardar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            ConsultarFechaCierreSistema("GUARDAR")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarGuardadoDocumento()
        Try
            If _OrdenSelected.VigenciaHasta.Value.ToShortDateString <= mdtmFechaCierreSistema Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La orden no se puede actualizar porque la fecha de vigencia es menor o igual a la fecha de cierre (" & mdtmFechaCierreSistema.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            mlogRecalcularFechas = False 'CCM20120305
            VerificarFechaValida(True)   'SLB20120704 Primero se Valida si la Fecha es Valida.
            'calcularDiasOrden(MSTR_CALCULAR_DIAS_ORDEN, -1, True)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden", Me.ToString(), "ContinuarGuardadoDocumento", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        Try

            EsEdicion = True
            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(Me._OrdenSelected) Then
                ConsultarFechaCierreSistema("EDITAR")
            End If

            consultarClaseEspecie(OrdenSelected.Nemotecnico)

            If ActivoExento = "SI" Then
                If OrdenSelected.ExentoRetencion = True Or OrdenSelected.ExentoRetencion = False Then
                    HabilitarExento = True
                    If EsBono = True Then
                        VisibilidadCampoExento = Visibility.Visible
                    Else
                        VisibilidadCampoExento = Visibility.Collapsed
                    End If

                Else
                    HabilitarExento = False
                    VisibilidadCampoExento = Visibility.Collapsed
                End If
            Else
                HabilitarExento = False
                VisibilidadCampoExento = Visibility.Collapsed
            End If



        Catch ex As Exception
            Me.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro", _
                                 Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub consultarClaseEspecie(Nemotecnico As String)
        Try
            If Not IsNothing(Nemotecnico) Then
                dcproxy2.ConsultarClaseEspecie(Nemotecnico, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerClaseOrden, "consulta")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de la clase de la especie", _
                                 Me.ToString(), "consultarClaseEspecie", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarEdicionDocumento()
        Try
            If _OrdenSelected.VigenciaHasta.Value.ToShortDateString <= mdtmFechaCierreSistema Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La orden no se puede editar porque la fecha de vigencia es menor o igual a la fecha de cierre (" & mdtmFechaCierreSistema.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(Me._OrdenSelected) Then
                validarEstadoOrden(MSTR_ACCION_EDITAR)
            End If
            'TabSeleccionadoGeneral = OrdenTabs.LEO
            habilitarFechaElaboracion = False
            HabilitarMercado = False
            _mlogOpcionActiva = False
            EditandoComitente = True
            MyBase.CambioItem("OpcionActiva")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro", _
                                 Me.ToString(), "ContinuarEdicionDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            If logEsModal Then
                RaiseEvent TerminoGuardarRegistro(False, 0, String.Empty, String.Empty)
                Editando = False
            Else
                IsBusy = True
                ErrorForma = ""
                mstrAccionOrden = String.Empty
                If Not IsNothing(_OrdenSelected) Then
                    Editando = False
                    dcProxy.RejectChanges()
                    dcProxy1.RejectChanges()
                    mdcProxyUtilidad01.RejectChanges()

                    EditandoInstrucciones = False
                    EditandoDetalle = True
                    EditandoRegistro = False
                    HabilitarExento = False
                    IsEnableComitenteADR = False 'Jorge Andres Bedoya 20150406               

                    habilitarFechaElaboracion = False
                    HabilitarMercado = False

                    If _OrdenSelected.EntityState = EntityState.Detached Then
                        OrdenSelected = OrdenAnterior
                    End If

                    If Not IsNothing(OrdenSelected) Then
                        'Jorge Andres Bedoya 20150406
                        If Me.ClaseOrden = ClasesOrden.A Then
                            If Me._OrdenSelected.Objeto = "AD" Then
                                Me.MostrarComitenteADR = Visibility.Visible
                            Else
                                Me.MostrarComitenteADR = Visibility.Collapsed
                            End If

                            If Me._OrdenSelected.Objeto = "OPA" Then
                                Me.VisibilidadOfertaPublica = Visibility.Visible
                            Else
                                Me.VisibilidadOfertaPublica = Visibility.Collapsed
                            End If

                        End If
                    End If

                    If Me.ClaseOrden = ClasesOrden.C Then
                        consultarPagosOrden()
                    End If

                    ValidarUsuarioOperador("cancelar")
                    HabilitarBotones()
                    AccionInstrucciones()

                    '// Si se cancela se asignan los valores anteriores de los buscadores
                    If ListaOrdenes.Count > 0 Then
                        ComitenteSeleccionado = _mobjComitenteSeleccionadoAntes
                        buscarOrdenanteSeleccionado()
                        buscarCuentaDepositoSeleccionada()
                        NemotecnicoSeleccionado = _mobjNemotecnicoSeleccionadoAntes
                        ReceptorTomaSeleccionado = _mobjReceptorTomaSeleccionadoAntes
                    End If

                    logRegistroLibroOrdenes = False
                    dblValorCantidadLibroOrden = 0
                    EditandoComitente = False

                    If ActivoExento = "SI" Then
                        If OrdenSelected.ExentoRetencion = True Or OrdenSelected.ExentoRetencion = False Then
                            HabilitarExento = False
                            VisibilidadCampoExento = Visibility.Visible
                        Else
                            HabilitarExento = False
                            VisibilidadCampoExento = Visibility.Collapsed
                        End If
                    Else
                        HabilitarExento = False
                        VisibilidadCampoExento = Visibility.Collapsed
                    End If

                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_OrdenSelected) Then
                ConsultarFechaCierreSistema("BORRAR")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular la orden", Me.ToString(), "BorrarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarBorradoDocumento()
        Try
            If _OrdenSelected.VigenciaHasta.Value.ToShortDateString <= mdtmFechaCierreSistema Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La orden no se puede borrar porque la fecha de vigencia es menor o igual a la fecha de cierre (" & mdtmFechaCierreSistema.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_OrdenSelected) Then
                validarEstadoOrden(MSTR_ACCION_ANULAR)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro", _
                                 Me.ToString(), "ContinuarBorradoDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub VersionRegistro()
        Try
            If Not Me.OrdenSelected Is Nothing Then
                cb = New CamposBusquedaOrden(Me)
                cb.NroOrden = Right(Me.OrdenSelected.NroOrden, 6)
                cb.AnoOrden = Left(Me.OrdenSelected.NroOrden, 4)
                cb.Version = 0
                cb.Clase = Me.OrdenSelected.Clase

                If RegistroActivoPorAprobar = False And RegistroTieneCambios Then
                    cb.EstadoMakerChecker = EstadoMakerChecker.PA.ToString()
                    _EstadoMakerCheckerConsultar = EstadoMakerChecker.PA.ToString
                    _EstadoMakerCheckerConsultarAdicionales = EstadoMakerChecker.PA.ToString
                    _EstadoMakerCheckerConsultarInstrucciones = EstadoMakerChecker.PA.ToString
                ElseIf RegistroActivoPorAprobar Then
                    cb.EstadoMakerChecker = EstadoMakerChecker.A.ToString()
                    _EstadoMakerCheckerConsultar = EstadoMakerChecker.A.ToString()
                    _EstadoMakerCheckerConsultarAdicionales = EstadoMakerChecker.A.ToString()
                    _EstadoMakerCheckerConsultarInstrucciones = EstadoMakerChecker.A.ToString()
                End If

                ConfirmarBuscar()
            End If

            MyBase.VersionRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar consulta para ver la versión de la orden", Me.ToString(), "VersionRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub aprobarOrden(ByVal plogAprobar As Boolean)
        Try
            IsBusy = True
            dcProxy.OrdenSAEs.Clear()
            dcProxy.Load(dcProxy.AutorizarOrdenQuery(_OrdenSelected.Clase, _OrdenSelected.Tipo, _OrdenSelected.NroOrden, _OrdenSelected.Version, plogAprobar, _OrdenSelected.CalificacionEspecie, _OrdenSelected.PerfilComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoAutorizarOrden, MSTR_ACCION_APROBAR)
            MyBase.AprobarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el proceso de aprobación de la orden", Me.ToString(), "aprobarOrden", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario ha confirmado la aprobación de la orden y se pasa a validar si cumple o no con las excepciones RDIP. 
    ''' </summary>
    Private Sub aprobarOrdenConfirmado()
        Dim strMsg As String
        Try
            strMsg = validarExcepcionRiesgo()
            If Not strMsg.Equals(String.Empty) Then
                'C1.Silverlight.C1MessageBox.Show(strMsg, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarExcepcionRDIPAprobar)
                mostrarMensajePregunta(strMsg, _
                                       Program.TituloSistema, _
                                       "APROBARCONFIRMADO", _
                                       AddressOf validarExcepcionRDIPAprobar, True, "¿Desea continuar?")

            Else
                aprobarOrden(True)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el proceso de aprobación de la orden", Me.ToString(), "aprobarOrdenConfirmado", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón aprobar
    ''' </summary>
    Public Overrides Sub AprobarRegistro()
        Try
            If _OrdenSelected Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la orden que desea aprobar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If _OrdenSelected.VigenciaHasta < mdtmFechaCierreSistema Then
                    A2Utilidades.Mensajes.mostrarMensaje("La orden no puede ser aprobada porque su fecha de vigencia es anterior a la fecha de cierre del sistema (" & Format(mdtmFechaCierreSistema, "dd/MMM/yyyy") & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    'C1.Silverlight.C1MessageBox.Show("¿Aprobar la orden " & _OrdenSelected.NroOrden & "?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarExcepcionRDIPAprobar)
                    mostrarMensajePregunta("¿Aprobar la orden " & _OrdenSelected.NroOrden & "?", _
                                           Program.TituloSistema, _
                                           "APROBARREGISTRO", _
                                           AddressOf validarExcepcionRDIPAprobar, False)

                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de aprobación de la orden", Me.ToString(), "AprobarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida si el nivel de riesgo del cliente es mayor o no al nivel de riesgo asociado a la especie que se negocia.
    ''' </summary>
    ''' <returns>
    ''' Cuando el nivel del cliente es mayor al de la especie se retorna una cadena vacia (no se incumple con la regla), de lo contrario se retorna el mensaje de validación correspondiente.
    ''' </returns>
    Private Function validarExcepcionRiesgo() As String
        Dim intRiesgoEspecie As Integer = -1
        Dim intRiesgoCliente As Integer = -2
        Dim strMsg As String = String.Empty

        If Not IsNothing(NemotecnicoSeleccionado) Then
            If Not NemotecnicoSeleccionado.CodClasificacionRiesgo Is Nothing AndAlso Not NemotecnicoSeleccionado.CodClasificacionRiesgo.Trim.Equals(String.Empty) AndAlso Versioned.IsNumeric(NemotecnicoSeleccionado.CodClasificacionRiesgo) Then
                intRiesgoEspecie = CInt(NemotecnicoSeleccionado.CodClasificacionRiesgo)
            End If
        End If

        If Not IsNothing(ComitenteSeleccionado) Then
            If Not ComitenteSeleccionado.CodClasificacionInversionista Is Nothing AndAlso Not ComitenteSeleccionado.CodClasificacionInversionista.Trim.Equals(String.Empty) AndAlso Versioned.IsNumeric(ComitenteSeleccionado.CodClasificacionInversionista) Then
                intRiesgoCliente = CInt(ComitenteSeleccionado.CodClasificacionInversionista)
            End If
        End If

        If intRiesgoEspecie < 0 And intRiesgoCliente < 0 Then
            strMsg = "Ni la calificación de riesgo para la especie y ni para el cliente están definidas." & vbNewLine & vbNewLine
        ElseIf intRiesgoCliente < 0 Then
            intRiesgoCliente = intRiesgoEspecie - 1
            strMsg = "La calificación de riesgo para el cliente no está definida. Se asumira un nivel menor que el asociado a la especie." & vbNewLine & vbNewLine
        ElseIf intRiesgoCliente < 0 Then
            intRiesgoEspecie = intRiesgoCliente + 1
            strMsg = "La calificación de riesgo para la especie no está definida. Se asumira un nivel de riesgo mayor que el asociado al cliente." & vbNewLine & vbNewLine
        End If

        If intRiesgoEspecie > intRiesgoCliente Then
            strMsg &= MSTR_MENSAJE_EXCEPCION_RDIP
            _OrdenSelected.PerfilComitente = intRiesgoCliente
            _OrdenSelected.CalificacionEspecie = intRiesgoEspecie
        End If

        Return (strMsg)
    End Function

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón rechazar
    ''' </summary>
    Public Overrides Sub RechazarRegistro()
        Try
            If _OrdenSelected Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la orden que desea rechazar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                'C1.Silverlight.C1MessageBox.Show("¿Rechazar la orden " & _OrdenSelected.NroOrden & "?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarRechazoOrden)
                mostrarMensajePregunta("¿Rechazar la orden " & _OrdenSelected.NroOrden & "?", _
                                       Program.TituloSistema, _
                                       "RECHAZARREGISTRO", _
                                       AddressOf validarRechazoOrden, False)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de rechazo de la orden", Me.ToString(), "RechazarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
    End Sub

    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()
    End Sub

    ''' <summary>
    ''' Validar si el estado de la orden permite que la orden sea modificada o anulada. Se tiene en cuenta el estado (campo strEstado), el estodo LEO y el estado SAE.
    ''' </summary>
    ''' <param name="pstrAccion">Indica si se valida la edición o anlación de la orden</param>
    Private Sub validarEstadoOrden(ByVal pstrAccion As String)

        Dim strMsg As String = String.Empty

        If Me._OrdenSelected.OrdenOyDPlus Then
            strMsg = "Esta orden solo puede ser modificada desde OyD Plus."
            If pstrAccion = MSTR_ACCION_EDITAR Then
                MyBase.RetornarValorEdicionNavegacion()
            End If
            A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            If ActivoExento = "SI" Then
                If OrdenSelected.ExentoRetencion = True Or OrdenSelected.ExentoRetencion = False Then
                    HabilitarExento = False
                    VisibilidadCampoExento = Visibility.Visible
                Else
                    HabilitarExento = False
                    VisibilidadCampoExento = Visibility.Collapsed
                End If
            Else
                HabilitarExento = False
                VisibilidadCampoExento = Visibility.Collapsed
            End If
            EditandoComitente = False
            Exit Sub
        End If

        If Me._OrdenSelected.Modificable = False Then
            If pstrAccion = MSTR_ACCION_EDITAR Then
                MyBase.RetornarValorEdicionNavegacion()
            End If

            If Me._OrdenSelected.EstadoMakerChecker = EstadoMakerChecker.PA.ToString() Then
                If pstrAccion = MSTR_ACCION_EDITAR Then
                    strMsg = "La orden está pendiente por aprobar y no puede ser modificada"
                Else
                    strMsg = "La orden está pendiente por aprobar y no puede ser anulada"
                End If

                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            ElseIf Me._OrdenSelected.EstadoMakerChecker = EstadoMakerChecker.NA.ToString() Then
                If pstrAccion = MSTR_ACCION_EDITAR Then
                    strMsg = "La orden fue rechazada y no puede ser modificada"
                Else
                    strMsg = "La orden fue rechazada y no puede ser anulada"
                End If

                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If pstrAccion = MSTR_ACCION_EDITAR Then
                    strMsg = "Solamente las órdenes en estado pendiente pueden ser modificadas"
                Else
                    strMsg = "Solamente las órdenes en estado pendiente pueden ser anuladas"
                End If

                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            'SLB (Inicio) No permite la edición de las ordenes Simultaneas, TTV y REPO de Regreso.
            If Me.ClaseOrden = ClasesOrden.C Then
                If _OrdenSelected.Objeto = RF_Simulatena_Regreso Then
                    strMsg = "La Orden No. " + _OrdenSelected.NroOrden.ToString + " es una Simultanea de Regreso no puede ser modificada. Modifique la Orden No. " + (_OrdenSelected.NroOrden - 1).ToString + " que es la correspondiente Simúltanea de Salida."
                End If

                If _OrdenSelected.Objeto = RF_Simultanea_Regreso_CRCC Then
                    strMsg = "La Orden No. " + _OrdenSelected.NroOrden.ToString + " es una Simultanea de Regreso CRCC no puede ser modificada. Modifique la Orden No. " + (_OrdenSelected.NroOrden - 1).ToString + " que es la correspondiente Simúltanea de Salida CRCC."
                End If

                If _OrdenSelected.Repo And _OrdenSelected.Objeto = RF_TTV_Regreso Then
                    strMsg = "La Orden No. " + _OrdenSelected.NroOrden.ToString + " es una TTV de Regreso no puede ser modificada. Modifique la Orden No. " + (_OrdenSelected.NroOrden - 1).ToString + " que es la correspondiente TTV de Salida."
                End If

                If _OrdenSelected.Repo And _OrdenSelected.Objeto = RF_REPO Then
                    If _OrdenSelected.Tipo = TiposOrden.S.ToString Or _OrdenSelected.Tipo = TiposOrden.R.ToString Then
                        strMsg = "La Orden No. " + _OrdenSelected.NroOrden.ToString + " es una REPO de Regreso no puede ser modificada. Modifique la Orden No. " + (_OrdenSelected.NroOrden - 1).ToString + " que es la correspondiente REPO de Salida."
                    End If
                End If
            End If
            'SLB (Fin)

            If strMsg.Equals(String.Empty) Then
                Me.IsBusy = True
                validarOrdenModificable(pstrAccion)
            Else
                If pstrAccion = MSTR_ACCION_EDITAR Then
                    MyBase.RetornarValorEdicionNavegacion()
                End If
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Valida el estado de la orden en el servidor
    ''' </summary>
    ''' <param name="pstrAccion">Indica si se valida la edición o anulación de la orden</param>
    Private Sub validarOrdenModificable(ByVal pstrAccion As String)
        Try
            dcProxy.OrdenModificables.Clear()
            dcProxy.Load(dcProxy.verificarOrdenModificableQuery(Me._OrdenSelected.Clase, Me._OrdenSelected.Tipo, Me._OrdenSelected.NroOrden, Me._OrdenSelected.Version, MSTR_MODULO_OYD_ORDENES, pstrAccion, Program.Usuario, Nothing, Program.HashConexion), AddressOf TerminoVerificarOrdenModificable, pstrAccion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la validación del estado de la orden", Me.ToString(), "ValidarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Me.IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Consulta el Saldo de la Orden
    ''' </summary>
    ''' <remarks>SLB20130815</remarks>
    Private Sub ConsultarSaldoOrden()
        Try
            'Santiago Vergara - Octubre 28/2013 - Se quita  la condicón de tipo A para que se consulte tambien el saldo cuando es renta fija
            'If Me.ClaseOrden = ClasesOrden.A Then
            If Not (Me._OrdenSelected.Version > 0 Or Me._OrdenSelected.Estado.Equals("C") Or Me._OrdenSelected.Estado.Equals("A")) Then
                dcProxy.ConsultarSaldoOrden(Me._OrdenSelected.Clase, Me._OrdenSelected.Tipo, Me._OrdenSelected.NroOrden, Me._OrdenSelected.Version, "BO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarSaldoOrden, "")
            Else
                SaldoOrden = 0
            End If
            'End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la validación del estado de la orden", Me.ToString(), "ValidarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Me.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarSaldoOrden(ByVal lo As InvokeOperation(Of Double))
        If Not lo.HasError Then
            SaldoOrden = lo.Value
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta del saldo de la Orden", Me.ToString(), "TerminoConsultarSaldoOrden", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub ConsultarFechaCierreSistema(Optional ByVal pstrUserState As String = "")
        Try
            mdcProxyUtilidad01.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del sistema.", _
                               Me.ToString(), "ConsultarFechaCierreSistema", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function NombreCategoriaEnDiccionario(ByVal pstrCategoria As String) As String
        Return String.Format("{0}_{1}", NombreInicioCombos, pstrCategoria)
    End Function

#End Region

#Region "Eventos de acciones adicionales sobre la orden"

    Public Sub duplicarOrden()
        Try
            If IsNothing(_OrdenSelected) Then
                Exit Sub
            End If
            'SLB20140526 Validación al momento de duplicar.
            If Me._OrdenSelected.OrdenOyDPlus Then
                A2Utilidades.Mensajes.mostrarMensaje("Esta orden solo puede ser duplicada desde OyD Plus.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            ' Sebastian Londoño 
            'C1.Silverlight.C1MessageBox.Show("¿Esta seguro de Duplicar la Orden?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, _
            '                     C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarDuplicarOrden)
            mostrarMensajePregunta("¿Esta seguro de Duplicar la Orden?", _
                                   Program.TituloSistema, _
                                   "DUPLICARORDEN", _
                                   AddressOf validarDuplicarOrden, False)
            'configurarNuevaOrden(_OrdenSelected, True, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la duplicación de la orden", Me.ToString(), "duplicarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub enviarPorSAE()
        Dim strMsg As String = String.Empty

        Try
            If Me._OrdenSelected Is Nothing Then
                Exit Sub
            End If

            'SLB20140526 Validación al enviarse a SAE.
            If Me._OrdenSelected.OrdenOyDPlus Then
                A2Utilidades.Mensajes.mostrarMensaje("Esta orden solo puede ser enviada a SAE desde OyD Plus.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Me.mlogSAEActivo And Me.mlogSAEActivoClaseOrden And Me.mlogPuedeEnrutarSAE Then
                If Me._OrdenSelected.Modificable = False Then
                    If Me._OrdenSelected.EstadoMakerChecker = EstadoMakerChecker.PA.ToString() Then
                        strMsg = "La orden está pendiente por aprobar y no puede ser enrutada a la Bolsa"
                    ElseIf Me._OrdenSelected.EstadoMakerChecker = EstadoMakerChecker.NA.ToString() Then
                        strMsg = "La orden fue rechazada y no puede ser enrutada a la Bolsa"
                    Else
                        strMsg = "Solamente las órdenes en estado pendiente pueden ser enrutadas a la Bolsa"
                    End If

                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    'C1.Silverlight.C1MessageBox.Show("Esta opción enviará la orden número: " & Me._OrdenSelected.NroOrden & " al sistema de negociación de la BVC, ¿Está seguro de realizar esta operación?", "Órdenes Bus de Integración", C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaSAE)
                    mostrarMensajePregunta("Esta opción enviará la orden número: " & Me._OrdenSelected.NroOrden & " al sistema de negociación de la BVC, ¿Está seguro de realizar esta operación?", _
                                           Program.TituloSistema, _
                                           "ENVIARORDENBUS", _
                                           AddressOf TerminoPreguntaSAE, False)
                    'Me.IsBusy = True
                    'dcProxy1.OrdenSAEs.Clear()
                    'dcProxy1.Load(dcProxy1.enrutar_OrdenSAEQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, False, Program.Usuario, Program.HashConexion), AddressOf TerminoEnrutarOrdenSAE, "enrutarSAE")
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enrutar la orden por SAE", Me.ToString(), "enviarPorSAE", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoPreguntaSAE(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                Me.IsBusy = True
                dcProxy1.OrdenSAEs.Clear()
                dcProxy1.Load(dcProxy1.enrutar_OrdenSAEQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, False, Program.Usuario, Program.HashConexion), AddressOf TerminoEnrutarOrdenSAE, "enrutarSAE")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enrutar la orden por SAE", Me.ToString(), "enviarPorSAE", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub RefrescarOrden()
        Try
            If Not Me.OrdenSelected Is Nothing Then
                Dim strEstadoMakerChecker As String = String.Empty
                Dim strClase As String = OrdenSelected.Clase
                Dim NroOrden As Integer = OrdenSelected.NroOrden
                Dim Version As Integer = OrdenSelected.Version
                If RegistroActivoPorAprobar Then
                    strEstadoMakerChecker = EstadoMakerChecker.PA.ToString()
                Else
                    strEstadoMakerChecker = EstadoMakerChecker.A.ToString()
                End If

                dcProxy.Ordens.Clear()
                dcProxy.Load(dcProxy.OrdenesConsultarQuery(strClase, "T", NroOrden, Version, Nothing, Nothing, Nothing, Nothing, Nothing,
                                                            Nothing, Nothing, Nothing, Nothing,
                                                            Nothing, Nothing, Nothing, Nothing, Nothing, strEstadoMakerChecker, Nothing,
                                                            Program.Usuario(), Program.HashConexion), AddressOf TerminoTraerOrdenes, "TERMINOACTUALIZAR")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de actualización de la orden", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub generarNuevaCompra()
        Try
            IsBusy = True
            configurarNuevaOrden(OrdenPorDefecto, False, True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la generación de una nueva orden de venta", Me.ToString(), "generarNuevaCompra", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub generarNuevaVenta()
        Try
            IsBusy = True
            configurarNuevaOrden(OrdenPorDefecto, False, False)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la generación de una nueva orden de compra", Me.ToString(), "generarNuevaVenta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub imprimirOrden()
        Dim strParametros As String = String.Empty
        Dim strReporte As String = String.Empty
        Dim strNroVentana As String = String.Empty

        Try
            If Not _OrdenSelected Is Nothing And mstrAccionOrden = String.Empty Then
                If Application.Current.Resources.Contains("A2VReporteOrdenesRS") = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("El reporte para imprimir la orden no está configurado", Program.TituloSistema)
                    Exit Sub
                End If

                strReporte = Application.Current.Resources("A2VReporteOrdenesRS").ToString.Trim()
                strParametros = "&pstrClaseOrden=" & _OrdenSelected.Clase & "&pintIdOrden=" & _OrdenSelected.NroOrden & "&pintVersion=" & _OrdenSelected.Version & "&pdblSaldo=" & Me.SaldoOrden
                MostrarReporte(strParametros, Me.ToString, strReporte)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la impresión del reporte", Me.ToString(), "imprimirReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

#Region "Métodos para controlar cambio de campos asociados a buscadores"

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la orden.
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdComitente As String = String.Empty

        Try
            If Not Me.OrdenSelected Is Nothing Then
                If Not Me.ComitenteSeleccionado Is Nothing And pstrIdComitente.Equals(String.Empty) Then
                    strIdComitente = Me.ComitenteSeleccionado.IdComitente
                End If

                If Not strIdComitente.Equals(Me.OrdenSelected.IDComitente) Then
                    If pstrIdComitente.Trim.Equals(String.Empty) Then
                        strIdComitente = Me.OrdenSelected.IDComitente
                    Else
                        strIdComitente = pstrIdComitente
                    End If

                    If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                        mdcProxyUtilidad01.BuscadorClientes.Clear()
                        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "idcomitente_condigito", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del nemotecnico que tiene asignado la orden.
    ''' </summary>
    ''' <param name="pstrNemotecnico">Nemotécnico que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarNemotecnico(Optional ByVal pstrNemotecnico As String = "")
        Dim strNemotecnico As String = String.Empty

        Try
            If Not Me.OrdenSelected Is Nothing Then
                If Not Me.NemotecnicoSeleccionado Is Nothing And pstrNemotecnico.Equals(String.Empty) Then
                    strNemotecnico = Me.NemotecnicoSeleccionado.Nemotecnico
                End If

                If Not strNemotecnico.Equals(Me.OrdenSelected.Nemotecnico) Then
                    If pstrNemotecnico.Trim.Equals(String.Empty) Then
                        strNemotecnico = Me.OrdenSelected.Nemotecnico
                    Else
                        strNemotecnico = pstrNemotecnico
                    End If
                    mdcProxyUtilidad01.BuscadorEspecies.Clear()
                    '-- CCM20120108: Incluir la clase para evitar que al digitar se traigan especies de una clase diferente a la de la orden
                    strNemotecnico = System.Web.HttpUtility.UrlEncode(strNemotecnico)

                    'SLB Cuando es Renta Fija se debe verificar si la clasificación es REPO o TTV.
                    If Me.ClaseOrden = ClasesOrden.C Then
                        Select Case Me.OrdenSelected.Objeto
                            Case RF_TTV_Salida, RF_TTV_Regreso, RF_REPO
                                'Case "3", "4", "RP" '"1", "2","SI",
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery("T", strNemotecnico, Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "") 'Accion
                            Case Else
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery("C", strNemotecnico, Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "") 'Renta Fija
                        End Select
                    Else
                        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery(Me.ClaseOrden.ToString(), strNemotecnico, Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "")
                    End If

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del nemotécnico de la orden", Me.ToString(), "buscarNemotecnico", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del receptor toma que tiene asignado la orden.
    ''' </summary>
    ''' <param name="pstrIdItem">Receptor toma que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.OrdenSelected Is Nothing Then
                Select Case pstrTipoItem.ToLower()
                    Case "receptores"
                        If Not Me.ReceptorTomaSeleccionado Is Nothing Then
                            strIdItem = Me.ReceptorTomaSeleccionado.IdItem
                        End If

                        pstrIdItem = pstrIdItem.Trim()
                        If Not strIdItem.Equals(Me.OrdenSelected.IdReceptorToma) Then
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.OrdenSelected.IdReceptorToma
                            Else
                                strIdItem = pstrIdItem
                            End If

                            If Not IsNothing(strIdItem) AndAlso Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                        End If
                    Case Else
                        logConsultar = False
                End Select

                If logConsultar Then
                    mdcProxyUtilidad01.BuscadorGenericos.Clear()
                    mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el buscador de " & pstrTipoItem, Me.ToString(), "buscarItem", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Actualizar el comitente de la orden con los datos del comitente recibido como parámetro
    ''' </summary>
    ''' <param name="pobjComitente">Comitente enviado como parámetro</param>
    Public Sub actualizarComitenteOrden(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Me.ComitenteSeleccionado = pobjComitente
    End Sub

    ''' <summary>
    ''' Actualizar elemento buscado con los datos recibidos como parámetro
    ''' </summary>
    ''' <param name="pstrTipoItem">Tipo de objeto que se recibe</param>
    ''' <param name="pobjItem">Item enviado como parámetro</param>
    Public Sub actualizarItemOrden(ByVal pstrTipoItem As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrTipoItem.ToLower
                Case "idreceptortoma"
                    Me.ReceptorTomaSeleccionado = pobjItem
                    Me.OrdenSelected.IdReceptorToma = pobjItem.IdItem
                Case "idreceptor"
                    Me.ReceptoresOrdenSelected.IDReceptor = pobjItem.IdItem
                    Me.ReceptoresOrdenSelected.Nombre = pobjItem.Nombre
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Actualizar el nemotécnico de la orden con los datos del nemotecnico recibido como parámetro
    ''' </summary>
    ''' <param name="pobjNemotecnico">Nemotécnico enviado como parámetro</param>
    Public Sub actualizarNemotecnicoOrden(ByVal pobjNemotecnico As OYDUtilidades.BuscadorEspecies, Optional ByVal plogActualizarCaracteristicas As Boolean = True)
        Me.NemotecnicoSeleccionado = pobjNemotecnico
        Me._OrdenSelected.Nemotecnico = pobjNemotecnico.Nemotecnico

        If Me._OrdenSelected.Objeto <> "IC" And Me.ClaseOrden.ToString = ClasesOrden.A.ToString Then
            If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If

        If Me.ClaseOrden.ToString() = ClasesOrden.C.ToString() Then
            If _HabilitarSeleccionISIN Then
                If plogActualizarCaracteristicas Then
                    Me._OrdenSelected.FechaEmision = pobjNemotecnico.Emision
                    Me._OrdenSelected.FechaVencimiento = pobjNemotecnico.Vencimiento
                    Me._OrdenSelected.Modalidad = pobjNemotecnico.CodModalidad
                    Me._OrdenSelected.IndicadorEconomico = IIf(pobjNemotecnico.IdIndicador Is Nothing, Nothing, pobjNemotecnico.IdIndicador) ' Se hace por conversión de tipo de datos
                    Me._OrdenSelected.PuntosIndicador = pobjNemotecnico.PuntosIndicador
                    Me._OrdenSelected.TasaInicial = pobjNemotecnico.TasaFacial
                End If
            Else
                If plogActualizarCaracteristicas Then
                    If Not IsNothing(pobjNemotecnico.Emision) Then
                        Me._OrdenSelected.FechaEmision = pobjNemotecnico.Emision
                    End If
                    If Not IsNothing(pobjNemotecnico.Vencimiento) Then
                        Me._OrdenSelected.FechaVencimiento = pobjNemotecnico.Vencimiento
                    End If
                    If Not IsNothing(pobjNemotecnico.CodModalidad) Then
                        Me._OrdenSelected.Modalidad = pobjNemotecnico.CodModalidad
                    End If
                    If Not IsNothing(pobjNemotecnico.IdIndicador) Then
                        Me._OrdenSelected.IndicadorEconomico = IIf(pobjNemotecnico.IdIndicador Is Nothing, Nothing, pobjNemotecnico.IdIndicador) ' Se hace por conversión de tipo de datos
                    End If
                    If Not IsNothing(pobjNemotecnico.PuntosIndicador) Then
                        Me._OrdenSelected.PuntosIndicador = pobjNemotecnico.PuntosIndicador
                    End If
                    If Not IsNothing(pobjNemotecnico.TasaFacial) Then
                        Me._OrdenSelected.TasaInicial = pobjNemotecnico.TasaFacial
                    End If
                End If
            End If

            mlogEspecieAccion = pobjNemotecnico.EsAccion
            mstrEspecieTipoTasa = pobjNemotecnico.CodTipoTasaFija

            If Me._OrdenSelected.Objeto <> "IC" Or (Not IsNothing(Me.ComitenteSeleccionado) AndAlso Me.ComitenteSeleccionado.CodCategoria <> "2") Then
                If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                    A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado  y el cliente debe ser Inversionista Profesional", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If

            If ActivoExento = "SI" Then
                consultarClaseEspecie(pobjNemotecnico.Nemotecnico)
            Else
                _OrdenSelected.ExentoRetencion = 0
                VisibilidadCampoExento = Visibility.Collapsed
            End If
        End If

    End Sub
#End Region

#Region "Tablas hijas"

#Region "Liquidaciones de la Orden"

    Private _ListaLiquidacionesOrden As EntitySet(Of OyDOrdenes.LiquidacionesOrden)
    Public Property LiquidacionesOrden() As EntitySet(Of OyDOrdenes.LiquidacionesOrden)
        Get
            Return (_ListaLiquidacionesOrden)
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.LiquidacionesOrden))
            _ListaLiquidacionesOrden = value
            MyBase.CambioItem("LiquidacionesOrden")
        End Set
    End Property

#End Region

#Region "Beneficarios"

    '******************************************************** BeneficiariosOrdenes 
    Private _ListaBeneficiariosOrdenes As EntitySet(Of OyDOrdenes.BeneficiariosOrden)
    Public Property ListaBeneficiariosOrdenes() As EntitySet(Of OyDOrdenes.BeneficiariosOrden)
        Get
            Return _ListaBeneficiariosOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.BeneficiariosOrden))
            _ListaBeneficiariosOrdenes = value
            MyBase.CambioItem("ListaBeneficiariosOrdenes")
            MyBase.CambioItem("ListaBeneficiariosOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property BeneficiariosOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBeneficiariosOrdenes) Then
                Dim view = New PagedCollectionView(_ListaBeneficiariosOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _BeneficiariosOrdenSelected As OyDOrdenes.BeneficiariosOrden
    Public Property BeneficiariosOrdenSelected() As OyDOrdenes.BeneficiariosOrden
        Get
            Return _BeneficiariosOrdenSelected
        End Get
        Set(ByVal value As OyDOrdenes.BeneficiariosOrden)
            _BeneficiariosOrdenSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("BeneficiariosOrdenSelected")
            End If
        End Set
    End Property

#End Region

#Region "Receptores"

    '******************************************************** ReceptoresOrdenes 
    Private _ListaReceptoresOrdenes As EntitySet(Of OyDOrdenes.ReceptoresOrden)
    Public Property ListaReceptoresOrdenes() As EntitySet(Of OyDOrdenes.ReceptoresOrden)
        Get
            Return _ListaReceptoresOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.ReceptoresOrden))
            _ListaReceptoresOrdenes = value
            If Not IsNothing(value) Then
                ReceptoresOrdenSelected = _ListaReceptoresOrdenes.FirstOrDefault
            End If
            MyBase.CambioItem("ListaReceptoresOrdenes")
            MyBase.CambioItem("ListaReceptoresOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property ReceptoresOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReceptoresOrdenes) Then
                Dim view = New PagedCollectionView(_ListaReceptoresOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ReceptoresOrdenSelected As OyDOrdenes.ReceptoresOrden
    Public Property ReceptoresOrdenSelected() As OyDOrdenes.ReceptoresOrden
        Get
            Return _ReceptoresOrdenSelected
        End Get
        Set(ByVal value As OyDOrdenes.ReceptoresOrden)
            If Not IsNothing(value) Then
                _ReceptoresOrdenSelected = value
                MyBase.CambioItem("ReceptoresOrdenSelected")
            End If
        End Set
    End Property

    'Private _ListaReceptoresOrdenesAnterior As New EntitySet(Of OyDOrdenes.ReceptoresOrden)

#End Region

#Region "Liquidaciones Probables"
    '******************************************************** LiqProbablesOrdenes 
    Private _ListaLiqAsociadasOrdenes As EntitySet(Of OyDOrdenes.LiqAsociadasOrden)
    Public Property ListaLiqAsociadasOrdenes() As EntitySet(Of OyDOrdenes.LiqAsociadasOrden)
        Get
            Return _ListaLiqAsociadasOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.LiqAsociadasOrden))
            _ListaLiqAsociadasOrdenes = value
            If Not IsNothing(value) Then
                LiqAsociadasOrdenSelected = ListaLiqAsociadasOrdenes.FirstOrDefault
            End If
            MyBase.CambioItem("ListaLiqAsociadasOrdenes")
            MyBase.CambioItem("ListaLiqAsociadasOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property ListaLiqAsociadasOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiqAsociadasOrdenes) Then
                Dim view = New PagedCollectionView(_ListaLiqAsociadasOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _LiqAsociadasOrdenSelected As OyDOrdenes.LiqAsociadasOrden
    Public Property LiqAsociadasOrdenSelected() As OyDOrdenes.LiqAsociadasOrden
        Get
            Return _LiqAsociadasOrdenSelected
        End Get
        Set(ByVal value As OyDOrdenes.LiqAsociadasOrden)
            If Not IsNothing(value) Then
                _LiqAsociadasOrdenSelected = value
                MyBase.CambioItem("LiqAsociadasOrdenSelected")
            End If
        End Set
    End Property

#End Region

#Region "Propiedades Instrucciones"

    Private _ListaInstruccionesOrdenes As EntitySet(Of OyDOrdenes.InstruccionesOrdene)
    Public Property ListaInstruccionesOrdenes() As EntitySet(Of OyDOrdenes.InstruccionesOrdene)
        Get
            Return _ListaInstruccionesOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.InstruccionesOrdene))
            _ListaInstruccionesOrdenes = value
            MyBase.CambioItem("ListaInstruccionesOrdenes")
            MyBase.CambioItem("ListaInstruccionesOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property ListaInstruccionesOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaInstruccionesOrdenes) Then
                Dim view = New PagedCollectionView(_ListaInstruccionesOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _InstruccionesOrdeneSelected As OyDOrdenes.InstruccionesOrdene
    Public Property InstruccionesOrdeneSelected() As OyDOrdenes.InstruccionesOrdene
        Get
            Return _InstruccionesOrdeneSelected
        End Get
        Set(ByVal value As OyDOrdenes.InstruccionesOrdene)
            _InstruccionesOrdeneSelected = value
            MyBase.CambioItem("InstruccionesOrdeneSelected")
        End Set
    End Property

    Private _TituloInstruccion As String = MSTR_TITULO_INSTRUCCION_MODIFICAR
    Public Property TituloInstruccion As String
        Get
            Return _TituloInstruccion
        End Get
        Set(ByVal value As String)
            _TituloInstruccion = value
            MyBase.CambioItem("TituloInstruccion")
        End Set
    End Property

#End Region

#Region "Propiedades Ordenes Pago"

    Private _ListaOrdenesPagos As EntitySet(Of OyDOrdenes.OrdenesPago)
    Public Property ListaOrdenesPagos() As EntitySet(Of OyDOrdenes.OrdenesPago)
        Get
            Return _ListaOrdenesPagos
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.OrdenesPago))
            _ListaOrdenesPagos = value
            MyBase.CambioItem("ListaOrdenesPagos")
        End Set
    End Property

    Private WithEvents _OrdenesPagoSelected As New OyDOrdenes.OrdenesPago
    Public Property OrdenesPagoSelected() As OyDOrdenes.OrdenesPago
        Get
            Return _OrdenesPagoSelected
        End Get
        Set(ByVal value As OyDOrdenes.OrdenesPago)
            _OrdenesPagoSelected = value
            MyBase.CambioItem("OrdenesPagoSelected")
        End Set
    End Property

    Private _mobjCtaDepositoPagoSeleccionada As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CtaDepositoPagoSeleccionada() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCtaDepositoPagoSeleccionada)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            _mobjCtaDepositoPagoSeleccionada = value

            If mstrAccionOrden.Equals(MSTR_MC_ACCION_INGRESAR) Or mstrAccionOrden.Equals(MSTR_MC_ACCION_ACTUALIZAR) Then
                If Not _mobjCtaDepositoPagoSeleccionada Is Nothing Then
                    If Not _mobjCtaDepositoPagoSeleccionada.NroCuentaDeposito Is Nothing Then
                        _OrdenesPagoSelected.CtaTitulo = _mobjCtaDepositoPagoSeleccionada.NroCuentaDeposito
                    Else
                        _OrdenesPagoSelected.CtaTitulo = Nothing
                    End If
                End If
            End If
            MyBase.CambioItem("CtaDepositoPagoSeleccionada")
        End Set
    End Property

#End Region

#Region "Propiedades Comisiones(Adicionales)"

    Private _ListaAdicionalesOrdenes As EntitySet(Of OyDOrdenes.AdicionalesOrdene)
    Public Property ListaAdicionalesOrdenes() As EntitySet(Of OyDOrdenes.AdicionalesOrdene)
        Get
            Return _ListaAdicionalesOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.AdicionalesOrdene))
            _ListaAdicionalesOrdenes = value
            If Not IsNothing(value) Then
                AdicionalesOrdeneSelected = _ListaAdicionalesOrdenes.FirstOrDefault
            End If
            MyBase.CambioItem("ListaAdicionalesOrdenes")
            MyBase.CambioItem("ListaAdicionalesOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property ListaAdicionalesOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAdicionalesOrdenes) Then
                Dim view = New PagedCollectionView(_ListaAdicionalesOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _AdicionalesOrdeneSelected As OyDOrdenes.AdicionalesOrdene
    Public Property AdicionalesOrdeneSelected() As OyDOrdenes.AdicionalesOrdene
        Get
            Return _AdicionalesOrdeneSelected
        End Get
        Set(ByVal value As OyDOrdenes.AdicionalesOrdene)
            If Not IsNothing(value) Then
                _AdicionalesOrdeneSelected = value
                MyBase.CambioItem("AdicionalesOrdeneSelected")
            End If
        End Set
    End Property

#End Region

#Region "Adicionar Hijos"

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptoresOrden"
                Dim NewReceptoresOrden As New OyDOrdenes.ReceptoresOrden

                NewReceptoresOrden.ClaseOrden = Me._OrdenSelected.Clase
                NewReceptoresOrden.TipoOrden = Me._OrdenSelected.Tipo
                NewReceptoresOrden.NroOrden = Me._OrdenSelected.NroOrden
                NewReceptoresOrden.Version = Me._OrdenSelected.Version
                NewReceptoresOrden.IDComisionista = Me._OrdenSelected.IDComisionista
                NewReceptoresOrden.IDSucComisionista = Me._OrdenSelected.IdSucComisionista
                NewReceptoresOrden.FechaActualizacion = Now()
                NewReceptoresOrden.Usuario = Program.Usuario
                NewReceptoresOrden.Lider = False
                NewReceptoresOrden.Porcentaje = 0
                NewReceptoresOrden.Nombre = ""

                ListaReceptoresOrdenes.Add(NewReceptoresOrden)
                ReceptoresOrdenSelected = NewReceptoresOrden

                MyBase.CambioItem("ListaReceptoresOrdenes")
            Case "cmOrdenLiqAsociadas"
                Dim NewLiqProbables As New OyDOrdenes.LiqAsociadasOrden

                NewLiqProbables.ClaseOrden = Me._OrdenSelected.Clase
                NewLiqProbables.TipoOrden = Me._OrdenSelected.Tipo
                NewLiqProbables.NroOrden = Me._OrdenSelected.NroOrden
                NewLiqProbables.Version = Me._OrdenSelected.Version
                NewLiqProbables.IDComisionista = Me._OrdenSelected.IDComisionista
                NewLiqProbables.IDSucComisionista = Me._OrdenSelected.IdSucComisionista
                NewLiqProbables.Actualizacion = Now()
                NewLiqProbables.FechaLiquidacion = Now.Date
                NewLiqProbables.Usuario = Program.Usuario
                'CORREC_CITI_SV_2014
                If IsNothing(ListaLiqAsociadasOrdenes) Then
                    ListaLiqAsociadasOrdenes = dcProxy1.LiqAsociadasOrdens
                End If
                ListaLiqAsociadasOrdenes.Add(NewLiqProbables)
                LiqAsociadasOrdenSelected = NewLiqProbables

                MyBase.CambioItem("ListaLiqAsociadasOrdenes")
            Case "cmComisionesOrden"
                If Not ListaAdicionalesOrdenes.Count > 0 Then
                    Dim NewAdicionalesOrdene As New OyDOrdenes.AdicionalesOrdene
                    'TODO: Verificar cuales son los campos que deben inicializarse
                    NewAdicionalesOrdene.IDComisionista = Me._OrdenSelected.IDComisionista
                    NewAdicionalesOrdene.IdSucComisionista = Me._OrdenSelected.IdSucComisionista
                    NewAdicionalesOrdene.Tipo = Me._OrdenSelected.Tipo
                    NewAdicionalesOrdene.Clase = Me._OrdenSelected.Clase
                    NewAdicionalesOrdene.ID = Me._OrdenSelected.NroOrden
                    NewAdicionalesOrdene.Version = Me._OrdenSelected.Version
                    NewAdicionalesOrdene.IdBolsa = Me._OrdenSelected.IdBolsa
                    'NewAdicionalesOrdene.IdComision = AdicionalesOrdenePorDefecto.IdComision
                    'NewAdicionalesOrdene.PorcCompra = AdicionalesOrdenePorDefecto.PorcCompra
                    'NewAdicionalesOrdene.PorcVenta = AdicionalesOrdenePorDefecto.PorcVenta
                    'NewAdicionalesOrdene.PorcOtro = AdicionalesOrdenePorDefecto.PorcOtro
                    'NewAdicionalesOrdene.IdOperacion = AdicionalesOrdenePorDefecto.IdOperacion
                    'NewAdicionalesOrdene.ComisionSugerida = AdicionalesOrdenePorDefecto.ComisionSugerida
                    NewAdicionalesOrdene.Actualizacion = Now()
                    NewAdicionalesOrdene.Usuario = Program.Usuario

                    ListaAdicionalesOrdenes.Add(NewAdicionalesOrdene)
                    AdicionalesOrdeneSelected = NewAdicionalesOrdene

                    MyBase.CambioItem("ListaAdicionalesOrdenes")
                End If
            Case "cmPagosOrdenes"
                Dim NewOrdenesPagoSelected As New OyDOrdenes.OrdenesPago

                NewOrdenesPagoSelected.FormaPago = "P"
                NewOrdenesPagoSelected.CumpPesos = "S"
                NewOrdenesPagoSelected.CtaSebraPesos = String.Empty
                NewOrdenesPagoSelected.CtaTitulo = String.Empty
                NewOrdenesPagoSelected.DeposPesos = String.Empty

                ListaOrdenesPagos.Add(NewOrdenesPagoSelected)
                OrdenesPagoSelected = NewOrdenesPagoSelected

        End Select
    End Sub

#End Region

#Region "Borrar Hijos"

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptoresOrden"
                If Not IsNothing(ListaReceptoresOrdenes) Then
                    If Not _ReceptoresOrdenSelected Is Nothing Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresOrdenSelected, ListaReceptoresOrdenes.ToList)

                        If ListaReceptoresOrdenes.Contains(_ReceptoresOrdenSelected) Then
                            ListaReceptoresOrdenes.Remove(_ReceptoresOrdenSelected)
                        End If
                        If ListaReceptoresOrdenes.Count > 0 Then
                            Program.PosicionarItemLista(ReceptoresOrdenSelected, ListaReceptoresOrdenes.ToList, intRegistroPosicionar)
                        Else
                            ReceptoresOrdenSelected = Nothing
                        End If
                        MyBase.CambioItem("ListaReceptoresOrdenes")
                    End If
                End If
            Case "cmOrdenLiqAsociadas"
                If Not IsNothing(ListaLiqAsociadasOrdenes) Then
                    If Not _LiqAsociadasOrdenSelected Is Nothing Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(LiqAsociadasOrdenSelected, ListaLiqAsociadasOrdenes.ToList)

                        If ListaLiqAsociadasOrdenes.Contains(_LiqAsociadasOrdenSelected) Then
                            ListaLiqAsociadasOrdenes.Remove(_LiqAsociadasOrdenSelected)
                        End If
                        If ListaLiqAsociadasOrdenes.Count > 0 Then
                            Program.PosicionarItemLista(LiqAsociadasOrdenSelected, ListaLiqAsociadasOrdenes.ToList, intRegistroPosicionar)
                        Else
                            LiqAsociadasOrdenSelected = Nothing
                        End If
                        MyBase.CambioItem("ListaLiqAsociadasOrdenes")
                    End If
                End If
            Case "cmComisionesOrden"
                If Not IsNothing(ListaAdicionalesOrdenes) Then
                    If Not _AdicionalesOrdeneSelected Is Nothing Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(AdicionalesOrdeneSelected, ListaAdicionalesOrdenes.ToList)

                        If ListaAdicionalesOrdenes.Contains(_AdicionalesOrdeneSelected) Then
                            ListaAdicionalesOrdenes.Remove(_AdicionalesOrdeneSelected)
                        End If
                        If ListaAdicionalesOrdenes.Count > 0 Then
                            Program.PosicionarItemLista(AdicionalesOrdeneSelected, ListaAdicionalesOrdenes.ToList, intRegistroPosicionar)
                        Else
                            AdicionalesOrdeneSelected = Nothing
                        End If
                        MyBase.CambioItem("ListaAdicionalesOrdenes")
                    End If
                End If
        End Select
    End Sub

#End Region

#End Region

#Region "Consultar datos asociados a la orden o al comitente"

    ''' <summary>
    ''' Consultar las liquidaciones asociadas a la orden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cargarLiquidaciones()
        If mstrAccionOrden <> MSTR_MC_ACCION_INGRESAR And mlogDuplicarOrden = False Then
            IsBusyLiquidaciones = True
            dcProxyConsultas.LiquidacionesOrdens.Clear()
            dcProxyConsultas.Load(dcProxyConsultas.ConsultarLiquidacionesAsociadasOrdenQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, Program.Usuario, Nothing, Program.HashConexion), AddressOf TerminoConsultarLiquidacionesOrden, "consultarLiquidaciones")
        Else
            dcProxyConsultas.LiquidacionesOrdens.Clear()
        End If
    End Sub

    ''' <summary>
    ''' Consultar los receptores asociados a la orden
    ''' </summary>
    Private Sub consultarOrdenSAE()
        dcProxy1.OrdenSAEs.Clear()
        dcProxy1.Load(dcProxy1.ConsultarOrdenSAEQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarOrdenSAE, "consultarSAE")
    End Sub

    ''' <summary>
    ''' Se verifica si los receptores estan activos de la orden duplica 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub verificarReceptoresOrdenDuplicar()
        dcProxy1.ReceptoresOrdens.Clear()
        dcProxy1.Load(dcProxy1.Verificar_ReceptoresOrdenes_OrdenQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, OrdenSelected.Version, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, "verificarReceptoresDuplicar")
    End Sub

    ''' <summary>
    ''' Consultar los receptores asociados a la orden
    ''' </summary>
    Private Sub consultarReceptoresOrden(ByVal pstrClaseOrden As String, ByVal pstrTipo As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUserState As String)
        dcProxy1.ReceptoresOrdens.Clear()
        dcProxy1.Load(dcProxy1.Traer_ReceptoresOrdenes_OrdenQuery(pstrClaseOrden, pstrTipo, pintNroOrden, pintVersion, Program.Usuario, _EstadoMakerCheckerConsultar, Nothing, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, "")
    End Sub

    ''' <summary>
    ''' Consultar la trazabilidad de la orden
    ''' </summary>
    Private Sub consultarTrazabilidadOrden(ByVal plngIdOrden As Integer, ByVal pstrClaseOrden As String, ByVal pstrTipo As String)
        dcProxy1.Trazabilidads.Clear()
        dcProxy1.Load(dcProxy1.TraerClientesTrazabilidadQuery(plngIdOrden, pstrClaseOrden, pstrTipo, "C", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTrazabilidadorden, "")
    End Sub

    ''' <summary>
    ''' Consultar los receptores del cliente para seleccionar los asociados a la orden
    ''' </summary>
    Private Sub consultarReceptoresComitente()
        If _mlogCargarReceptorClientes Then
            dcProxy1.ReceptoresOrdens.Clear()
            If Not IsNothing(ListaReceptoresOrdenes) Then
                ListaReceptoresOrdenes.Clear()
            End If
            dcProxy1.Load(dcProxy1.Traer_ReceptoresOrdenes_ClienteQuery(_OrdenSelected.Clase, _OrdenSelected.Tipo, _ComitenteSeleccionado.CodigoOYD, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, MSTR_ACC_CONSULTA_RECEPTORES_CLT)
        End If
    End Sub

    ''' <summary>
    ''' Consultar los receptores asociados a la orden
    ''' </summary>
    Private Sub consultarLiqProbablesOrden()
        dcProxy1.LiqAsociadasOrdens.Clear()
        dcProxy1.Load(dcProxy1.Traer_LiqProbables_OrdenQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, OrdenSelected.Version, _EstadoMakerCheckerConsultar, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiqProbablesOrdenes, "")
    End Sub

    ''' <summary>
    ''' Consultar los receptores asociados a la orden
    ''' </summary>
    Private Sub consultarPagosOrden()
        If Not IsNothing(OrdenSelected) Then
            dcProxy1.OrdenesPagos.Clear()
            dcProxy1.Load(dcProxy1.OrdenesPagosConsultarQuery(OrdenSelected.Tipo, OrdenSelected.Clase, OrdenSelected.NroOrden, OrdenSelected.Version, _EstadoMakerCheckerConsultar, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPagosOrdenes, "")
        End If
    End Sub

    ''' <summary>
    ''' Consultar los beneficiarios de la cuenta depósito asignada a la orden
    ''' </summary>
    Private Sub consultarBeneficiariosOrden()
        dcProxy.BeneficiariosOrdens.Clear()
        dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenes_OrdenQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, OrdenSelected.Version, Program.Usuario, Nothing, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "")
    End Sub

    ''' <summary>
    ''' Consultar los beneficiarios de la cuenta depósito asignada al cliente
    ''' </summary>
    Private Sub consultarBeneficiariosCliente()
        dcProxy.BeneficiariosOrdens.Clear()
        If Not IsNothing(OrdenSelected.CuentaDeposito) Then
            dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenes_ClienteQuery(OrdenSelected.IDComitente, OrdenSelected.CuentaDeposito, OrdenSelected.UBICACIONTITULO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "")
        End If
    End Sub

    ''' <summary>
    ''' Consultar los ordenantes del comitente asociado a la orden
    ''' </summary>
    Private Sub consultarOrdenantes()
        mdcProxyUtilidad01.BuscadorOrdenantes.Clear()
        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarOrdenantesComitenteQuery(_ComitenteSeleccionado.IdComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenantes, "")
    End Sub

    ''' <summary>
    ''' Consultar las cuentas depósito del comitente asociado a la orden
    ''' </summary>
    Private Sub consultarCuentasDeposito()
        mdcProxyUtilidad02.BuscadorCuentasDepositos.Clear()
        mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarCuentasDepositoComitenteQuery(_ComitenteSeleccionado.IdComitente, True, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDeposito, "")
    End Sub

    ''' <summary>
    ''' Consultar las cuentas depósito del comitente asociado a la orden
    ''' </summary>
    Private Sub consultarCuentasDepositoPago()
        dcProxy.CuentasDepositoPagos.Clear()
        dcProxy.Load(dcProxy.Traer_CuentasDepositoPagoQuery(_ComitenteSeleccionado.IdComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDepositoPago, "")
    End Sub

    ''' <summary>
    ''' Consultar las cuentas depósito del comitente asociado a la orden
    ''' </summary>
    Private Sub consultarInstruccionesCliente()
        dcProxy1.InstruccionesOrdenes.Clear()
        Dim strTopico As String = String.Empty
        Select Case Me._OrdenSelected.Tipo
            Case "C", "R"
                strTopico = STR_TOPICO_INST_ORDENES_COMPRA
            Case "V", "S"
                strTopico = STR_TOPICO_INST_ORDENES_VENTA
        End Select
        If Not IsNothing(_ComitenteSeleccionado) Then
            dcProxy1.Load(dcProxy1.Traer_InstruccionesClientesQuery(_ComitenteSeleccionado.IdComitente, strTopico, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstrucciones, "")
        End If
    End Sub

    ''' <summary>
    ''' Consultar las instrucciones de la Orden
    ''' </summary>
    Private Sub consultarInstruccionesOrden()
        dcProxy1.InstruccionesOrdenes.Clear()
        Dim strTopico As String = String.Empty
        Select Case Me._OrdenSelected.Tipo
            Case "C", "R"
                strTopico = STR_TOPICO_INST_ORDENES_COMPRA
            Case "V", "S"
                strTopico = STR_TOPICO_INST_ORDENES_VENTA
        End Select
        dcProxy1.Load(dcProxy1.Consultar_InstruccionesOrdenesQuery(_OrdenSelected.Tipo, _OrdenSelected.Clase, _OrdenSelected.NroOrden, _OrdenSelected.Version, strTopico, _EstadoMakerCheckerConsultarInstrucciones, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstrucciones, "")
    End Sub

    ''' <summary>
    ''' Consultar los  tipos de comisiones del comitente seleccionado de la Orden
    ''' </summary>
    Private Sub consultarTiposComisionesOrdenes()
        dcProxy1.Comisiones.Clear()
        If Not IsNothing(_ComitenteSeleccionado) Then
            dcProxy1.Load(dcProxy1.Consultar_ComisionesOrdenesQuery(_ComitenteSeleccionado.IdComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionesOrdenes, "")
        End If
    End Sub

    ''' <summary>
    ''' Consultar los  tipos de comisiones del comitente seleccionado de la Orden
    ''' </summary>
    Private Sub consultarAdicionalesOrdenes()
        dcProxy1.AdicionalesOrdenes.Clear()
        dcProxy1.Load(dcProxy1.Consultar_AdicionalesOrdenesQuery(_OrdenSelected.Tipo, _OrdenSelected.Clase, _OrdenSelected.NroOrden, _OrdenSelected.Version, _EstadoMakerCheckerConsultarAdicionales, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAdicionalesOrdenes, "")
    End Sub

    ''' <summary>
    ''' Consultar las cuentas del cliente para las instrucciones
    ''' </summary>
    Private Sub consultarCuentasCliente()
        dcProxy.CuentasClientes.Clear()
        dcProxy.Load(dcProxy.Consultar_CuentasClientesQuery(_ComitenteSeleccionado.IdComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasClientes, "")
    End Sub

    ''' <summary>
    ''' Seleccionar los datos del ordenante asociado a la orden de la lista de ordenantes
    ''' </summary>
    Private Sub buscarOrdenanteSeleccionado()
        If Not _Ordenantes Is Nothing And Not _OrdenSelected Is Nothing Then
            If Not Editando Then
                If Not _OrdenSelected.IDOrdenante Is Nothing Then
                    If (From obj In _Ordenantes Where obj.IdOrdenante = _OrdenSelected.IDOrdenante Select obj).ToList.Count > 0 Then
                        OrdenanteSeleccionado = (From cta In _Ordenantes Where cta.IdOrdenante.Trim() = _OrdenSelected.IDOrdenante.Trim() Select cta).ToList.ElementAt(0)
                    Else
                        'SLB20121205 Cuando el Ordenante se encuentra Inactivo se debe mostrar en el pantalla, a pesar de que en la lista no exista.
                        Dim strDescripcionOrdenante = LTrim(_OrdenSelected.IDOrdenante) & " - " & _OrdenSelected.NombreOrdenante.ToString
                        Ordenantes.Add(New OYDUtilidades.BuscadorOrdenantes With {.DescripcionOrdenante = strDescripcionOrdenante, .IdOrdenante = "-1"})
                        OrdenanteSeleccionado = Ordenantes.Last
                        Ordenantes.Remove(Ordenantes.Last)
                        'OrdenanteSeleccionado = Nothing
                    End If
                Else
                    If _Ordenantes.Count = 1 Then
                        OrdenanteSeleccionado = _Ordenantes.First
                    End If
                End If
            Else 'SLB se adiciona la seleccion del Ordenante Lider por defecto.
                If (From obj In _Ordenantes Select obj).ToList.Count > 0 Then
                    If _mlogRequiereIngresoOrdenantes Then
                        If (From obj In _Ordenantes Select obj).ToList.Count = 1 Then
                            OrdenanteSeleccionado = _Ordenantes.First
                        Else
                            OrdenanteSeleccionado = Nothing
                        End If
                    Else
                        If (From cta In _Ordenantes Where cta.Lider = True Select cta).ToList.Count > 0 Then
                            OrdenanteSeleccionado = (From cta In _Ordenantes Where cta.Lider = True Select cta).ToList.ElementAt(0)
                        Else
                            OrdenanteSeleccionado = _Ordenantes.First
                        End If
                    End If
                Else
                    OrdenanteSeleccionado = Nothing
                    A2Utilidades.Mensajes.mostrarMensaje("Ordenante Inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            OrdenanteSeleccionado = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Seleccionar los datos de la cuenta depósito asociada a la orden de la lista de cuentas depósito
    ''' </summary>
    Private Sub buscarCuentaDepositoSeleccionada()
        If Not _OrdenSelected Is Nothing And Not CuentasDeposito Is Nothing Then
            If Not Editando Then
                If Not _OrdenSelected.CuentaDeposito Is Nothing Then
                    If (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenSelected.CuentaDeposito And cta.Deposito = _OrdenSelected.UBICACIONTITULO Select cta).ToList.Count > 0 Then
                        CtaDepositoSeleccionada = (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenSelected.CuentaDeposito And cta.Deposito = _OrdenSelected.UBICACIONTITULO Select cta).ToList.ElementAt(0)
                    Else
                        CtaDepositoSeleccionada = Nothing
                    End If
                Else
                    If _CuentasDeposito.Count = 1 Then
                        CtaDepositoSeleccionada = _CuentasDeposito.First
                    ElseIf (From cta In CuentasDeposito Where IsNothing(cta.NroCuentaDeposito) And cta.Deposito = _OrdenSelected.UBICACIONTITULO Select cta).ToList.Count > 0 Then
                        CtaDepositoSeleccionada = (From cta In CuentasDeposito Where IsNothing(cta.NroCuentaDeposito) And cta.Deposito = _OrdenSelected.UBICACIONTITULO Select cta).ToList.ElementAt(0)
                    End If
                End If



                ' 'SLB se adiciona la seleccion de la primera cuenta Deposito por defecto.
                If Not _OrdenesPagoSelected.CtaTitulo Is Nothing And Not _OrdenesPagoSelected.CtaTitulo = "" Then
                    If (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenesPagoSelected.CtaTitulo Select cta).ToList.Count > 0 Then
                        CtaDepositoPagoSeleccionada = (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenesPagoSelected.CtaTitulo Select cta).ToList.ElementAt(0)
                    Else
                        CtaDepositoPagoSeleccionada = Nothing
                    End If
                Else
                    CtaDepositoPagoSeleccionada = Nothing
                End If
            Else
                If (From cta In CuentasDeposito Select cta).ToList.Count = 1 Then
                    CtaDepositoSeleccionada = CuentasDeposito.First
                Else
                    CtaDepositoSeleccionada = Nothing
                End If
            End If
        Else
            CtaDepositoSeleccionada = Nothing
        End If
    End Sub

#End Region


#Region "Property Changed"

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Se utiliza para calcular los días de vigencia y/o días de vencimiento cuando cambia alguna de las fechas involucradas en el cálculo
    ''' </summary>
    Private Sub _OrdenSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenSelected.PropertyChanged

        If Editando Then
            'If e.PropertyName.ToLower.Equals("fecharecepcion") Then
            '    If logActualizarFechaRecepcion Then
            '        logActualizarFechaRecepcion = False
            '        _OrdenSelected.FechaRecepcion = CType(_OrdenSelected.FechaRecepcion, Date).AddHours(_OrdenSelected.FechaSistema.Hour).AddMinutes(_OrdenSelected.FechaSistema.Minute).AddSeconds(_OrdenSelected.FechaSistema.Second)
            '        logActualizarFechaRecepcion = True
            '    End If
            'End If
            If e.PropertyName.ToLower.Equals("tipo") Then
                If Me.ClaseOrden = ClasesOrden.C Then

                    'Jorge Andres Bedoya 2014/12/29
                    If logValorCompromisoFuturoRequerido = True Then
                        Select Case Me.OrdenSelected.Objeto
                            Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO, RF_Simultanea_Salida_CRCC, RF_Simultanea_Regreso_CRCC
                                MostrarValorFuturoRepo = Visibility.Visible
                            Case Else
                                MostrarValorFuturoRepo = Visibility.Collapsed
                        End Select
                    End If

                    Select Case _OrdenSelected.Tipo
                        Case "V", "S"
                            HabilitarDetalleComisiones = True
                            '(SLB) Consultar el tipo de comisión del Detalle de Comisiones
                            consultarTiposComisionesOrdenes()
                            '(SLB) Consultar las Comisiones de la Orden
                            consultarAdicionalesOrdenes()

                            'SLB20131127 Manejo del campo de Valor Futuro REPO
                            If logValorCompromisoFuturoRequerido = False Then  'Jorge Andres Bedoya 2014/12/29
                                If _OrdenSelected.Tipo = TiposOrden.V.ToString() And (Me._OrdenSelected.Repo Or Me._OrdenSelected.Objeto = RF_Simulatena_Salida Or Me._OrdenSelected.Objeto = RF_Simultanea_Salida_CRCC) Then
                                    MostrarValorFuturoRepo = Visibility.Visible
                                Else
                                    MostrarValorFuturoRepo = Visibility.Collapsed
                                End If
                            End If

                        Case "C", "R"
                            HabilitarDetalleComisiones = False
                            If TabSeleccionadoGeneral = OrdenTabs.Comisiones Then
                                TabSeleccionadoGeneral = OrdenTabs.Pago
                            End If
                    End Select
                End If
                If Editando Then
                    consultarInstruccionesCliente()
                    'Else
                    '    consultarInstruccionesOrden()
                End If
            End If

            If e.PropertyName.ToLower.Equals("objeto") Then
                If Not _OrdenSelected.Objeto = String.Empty Then
                    _OrdenSelected.Ordinaria = False
                Else
                    _OrdenSelected.Ordinaria = True
                End If
            End If

            If Me.ClaseOrden = ClasesOrden.C Then
                If e.PropertyName.ToLower.Equals("objeto") Then
                    Me.OrdenSelected.Repo = False
                    Select Case _OrdenSelected.Objeto
                        Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_Simultanea_Salida_CRCC, RF_Simultanea_Regreso_CRCC
                            Me.OrdenSelected.Mercado = RF_MERCADO_SECUNDARIO
                            Me.HabilitarMercado = False
                            Me.OrdenSelected.Repo = False
                        Case RF_TTV_Salida, RF_TTV_Regreso
                            Me.OrdenSelected.Mercado = RF_MERCADO_SECUNDARIO
                            Me.HabilitarMercado = False
                            Me._OrdenSelected.Repo = True
                        Case RF_REPO
                            Me.OrdenSelected.Mercado = RF_MERCADO_REPO
                            Me.HabilitarMercado = False
                            Me.OrdenSelected.Repo = True
                        Case "MP"
                            Me.OrdenSelected.Mercado = RF_MERCADO_PRIMARIO '"P"
                            Me.HabilitarMercado = True
                            Me.OrdenSelected.Repo = False
                        Case Else
                            'objTipoId = CType(Application.Current.Resources("OrdenesView_Ord_RentaFija"), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_MERCADO").Where(Function(li) li.ID.Equals("P") Or li.ID.Equals("S") Or li.ID.Equals("R")).ToList
                            Me.OrdenSelected.Mercado = RF_MERCADO_SECUNDARIO
                            Me.HabilitarMercado = True
                            Me.OrdenSelected.Repo = False
                    End Select

                    If _strObjetoAnterior <> _OrdenSelected.Objeto Then
                        Select Case _strObjetoAnterior
                            Case RF_TTV_Salida, RF_TTV_Regreso, RF_REPO
                                'Case "3", "4", "RP"
                                If (_OrdenSelected.Objeto <> RF_TTV_Salida And _OrdenSelected.Objeto <> RF_TTV_Regreso And _OrdenSelected.Objeto <> RF_REPO) Then
                                    NemotecnicoOrden = String.Empty
                                End If
                            Case Else
                                If (_OrdenSelected.Objeto = RF_TTV_Salida Or _OrdenSelected.Objeto = RF_TTV_Regreso Or _OrdenSelected.Objeto = RF_REPO) Then
                                    NemotecnicoOrden = String.Empty
                                End If
                        End Select
                    End If
                    _strObjetoAnterior = _OrdenSelected.Objeto

                    'SLB20131127 Manejo del campo de Valor Futuro REPO
                    If logValorCompromisoFuturoRequerido = False Then
                        If _OrdenSelected.Tipo = TiposOrden.V.ToString() And (Me._OrdenSelected.Repo Or Me._OrdenSelected.Objeto = RF_Simulatena_Salida Or Me._OrdenSelected.Objeto = RF_Simultanea_Salida_CRCC) Then
                            MostrarValorFuturoRepo = Visibility.Visible
                        Else
                            MostrarValorFuturoRepo = Visibility.Collapsed
                        End If
                    Else
                        'Jorge Andres Bedoya 2014/12/29
                        Select Case Me.OrdenSelected.Objeto
                            Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO, RF_Simultanea_Salida_CRCC, RF_Simultanea_Regreso_CRCC
                                MostrarValorFuturoRepo = Visibility.Visible
                            Case Else
                                MostrarValorFuturoRepo = Visibility.Collapsed
                        End Select
                    End If
                End If
            End If

            If Me.ClaseOrden = ClasesOrden.A Then

                'Jorge Andres Bedoya 20150406
                If Me._OrdenSelected.Objeto = "AD" Then
                    Me.IsEnableComitenteADR = True
                    Me.MostrarComitenteADR = Visibility.Visible
                Else
                    Me.IsEnableComitenteADR = False
                    Me._OrdenSelected.IDComitenteADR = Nothing
                    Me._OrdenSelected.NombreComitenteADR = Nothing
                    Me._OrdenSelected.TipoIdentificacionADR = Nothing
                    Me._OrdenSelected.NroDocumentoADR = Nothing
                    Me.MostrarComitenteADR = Visibility.Collapsed
                End If

                If Me._OrdenSelected.Objeto = "OPA" Then
                    Me.VisibilidadOfertaPublica = Visibility.Visible
                Else
                    Me.VisibilidadOfertaPublica = Visibility.Collapsed
                    Me._OrdenSelected.ExistePreacuerdo = Nothing
                    Me._OrdenSelected.VendeTodo = Nothing
                    Me._OrdenSelected.PorcentajePagoEfectivo = 0
                End If

                If e.PropertyName.ToLower.Equals("preciostop") Then
                    Me.OrdenSelected.Precio = Me.OrdenSelected.PrecioStop
                End If
                If e.PropertyName.ToLower.Equals("duracion") Then
                    Select Case Me._OrdenSelected.Duracion
                        Case "D" 'DÍA
                            Me._OrdenSelected.VigenciaHasta = Now.Date
                            VisibleDuracionFecha = Visibility.Collapsed
                            VisibleDuracionHora = Visibility.Collapsed
                            Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                        Case "C" ' HASTA CANCELACION
                            Me._OrdenSelected.VigenciaHasta = OrdenPorDefecto.VigenciaHasta
                            VisibleDuracionFecha = Visibility.Collapsed
                            VisibleDuracionHora = Visibility.Collapsed
                            Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                        Case "I" 'INMEDIATA
                            DiasVigencia = intDiasVigenciaDuracionInmediata
                            VisibleDuracionFecha = Visibility.Collapsed
                            VisibleDuracionHora = Visibility.Collapsed
                            Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                        Case "S" 'SESION
                            Me._OrdenSelected.VigenciaHasta = Now.Date
                            VisibleDuracionFecha = Visibility.Collapsed
                            VisibleDuracionHora = Visibility.Collapsed
                            Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                        Case "F" 'HASTA FECHA
                            HoraFinVigencia = Me._OrdenSelected.VigenciaHasta
                            VisibleDuracionFecha = Visibility.Visible
                            VisibleDuracionHora = Visibility.Collapsed
                            Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                        Case "A" ' HASTA HORA
                            Me._OrdenSelected.VigenciaHasta = Now.Date
                            VisibleDuracionFecha = Visibility.Collapsed
                            VisibleDuracionHora = Visibility.Visible
                            HoraFinVigencia = Now
                            Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                    End Select
                End If
            End If

            If mlogRecalcularFechas = False Then
                mlogRecalcularFechas = True
                Exit Sub
            End If

            Try
                mlogRecalcularFechas = False 'CCM20120305

                Select Case e.PropertyName.ToLower()
                    Case "fechaorden"
                        Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1) 'DEMC20160121
                    Case "vigenciahasta"
                        Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                    Case "fechavencimiento"
                        Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_TITULO, -1)
                    Case "fechaemision"
                        Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_TITULO, -1)
                End Select
            Catch ex As Exception
                mlogRecalcularFechas = True 'CCM20120305
            End Try
        Else
            consultarInstruccionesOrden()
        End If
    End Sub

    Private Sub _OrdenesPagoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenesPagoSelected.PropertyChanged
        If Me.ClaseOrden = ClasesOrden.C Then
            If e.PropertyName.ToLower.Equals("cumppesos") Then
                _OrdenesPagoSelected.DeposPesos = String.Empty
                _OrdenesPagoSelected.CtaSebraPesos = String.Empty
                If _OrdenesPagoSelected.CumpPesos = "S" Then
                    CuentasDepositoPago = (From cuenta In _CuentasDepositoPagoSeleccionada Where cuenta.EsFirma = 1).ToList
                Else
                    CuentasDepositoPago = (From cuenta In _CuentasDepositoPagoSeleccionada Where cuenta.EsFirma = 0).ToList
                End If
            End If
        End If
    End Sub

    Private Sub _AdicionalesOrdeneSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _AdicionalesOrdeneSelected.PropertyChanged
        If Me.ClaseOrden = ClasesOrden.C Then
            If e.PropertyName.ToLower.Equals("idcomision") Then
                Dim TitoComisionSeleccionado = (From comision In _TiposComisiones Where comision.ID = _AdicionalesOrdeneSelected.IdComision).FirstOrDefault
                If Not IsNothing(TitoComisionSeleccionado) Then
                    _AdicionalesOrdeneSelected.PorcCompra = TitoComisionSeleccionado.PorcCompra
                    _AdicionalesOrdeneSelected.PorcVenta = TitoComisionSeleccionado.PorcVenta
                    _AdicionalesOrdeneSelected.PorcOtro = TitoComisionSeleccionado.PorcOtro
                End If
            End If
        End If
    End Sub

#End Region



End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaOrden
    Implements INotifyPropertyChanged

#Region "Eventos"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE EVENTOS
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- Indicar cualquier cambio en una propiedad del objeto
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region

#Region "Variables"
    Private mobjVM As OrdenesViewModel
#End Region

#Region "Propiedades"

    <Description("Renta fija o renta variable")> _
    Public Property Clase As String = String.Empty
    <Description("Tipo de operación de la orden: compra, venta, ...")> _
    Public Property Tipo As String = String.Empty
    Public Property AnoOrden As String = Nothing
    Public Property NroOrden As String
    Public Property Version As String ' = 0
    Public Property FechaOrden As System.Nullable(Of Date)
    Public Property Nemotecnico As String = String.Empty
    Public Property FormaPago As String = String.Empty
    Public Property Objeto As String = Nothing
    Public Property CondicionesNegociacion As String
    Public Property TipoInversion As String = "  "
    Public Property TipoTransaccion As String = String.Empty
    Public Property CanalRecepcion As String = String.Empty
    Public Property MedioVerificable As String = String.Empty
    Public Property FechaHoraRecepcion As System.Nullable(Of Date)
    Public Property TipoLimite As String = String.Empty
    Public Property VigenciaHasta As System.Nullable(Of Date)
    Public Property Ejecucion As String = String.Empty
    Public Property Duracion As String = String.Empty
    Public Property Estado As String = String.Empty
    Public Property EstadoOrdenBus As String = String.Empty
    Public Property IdBolsa As System.Nullable(Of Integer)
    Public Property EstadoMakerChecker As String = String.Empty
    Public Property AccionMakerChecker As String = String.Empty

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ComitenteSeleccionado"))
            mobjVM.CambioItem("cb")
        End Set
    End Property

    Private _OrdenanteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property OrdenanteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_OrdenanteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _OrdenanteSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OrdenanteSeleccionado"))
            mobjVM.CambioItem("cb")
        End Set
    End Property

    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NemotecnicoSeleccionado"))
            mobjVM.CambioItem("cb")
        End Set
    End Property

    Private _VisibleMakerAndCheker As Visibility = Visibility.Visible
    Public Property VisibleMakerAndCheker() As Visibility
        Get
            Return _VisibleMakerAndCheker
        End Get
        Set(ByVal value As Visibility)
            _VisibleMakerAndCheker = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibleMakerAndCheker"))
        End Set
    End Property

    Private _IDComitente As String = String.Empty
    Public Property IDComitente() As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property

    Private _IDOrdenante As String = String.Empty
    Public Property IDOrdenante() As String
        Get
            Return _IDOrdenante
        End Get
        Set(ByVal value As String)
            _IDOrdenante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDOrdenante"))
        End Set
    End Property

#End Region

#Region "Inicializaciones"

    Public Sub New(ByRef pobjVMPadre As OrdenesViewModel)
        mobjVM = pobjVMPadre
    End Sub

#End Region
End Class

Public Class TipoClasificacion
    Private _id As Integer
    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property
    Private _Nombre As String
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property

End Class

''' <summary>
''' Clase para manejo en vista de datos de entidad de datos de archivo excel
''' </summary>
''' <remarks></remarks>
Public Class ArchivoOrdenesLeoVista
    Implements INotifyPropertyChanged

    Private _intID As Integer
    Public Property intID() As Integer
        Get
            Return _intID
        End Get
        Set(ByVal value As Integer)
            _intID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intID"))
        End Set
    End Property

    Private _id As Integer
    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("id"))
        End Set
    End Property

    Private _idProceso As System.Nullable(Of Double)
    Public Property idProceso() As System.Nullable(Of Double)
        Get
            Return _idProceso
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _idProceso = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("idProceso"))
        End Set
    End Property

    Private _Clase As System.Nullable(Of Char)
    Public Property Clase() As System.Nullable(Of Char)
        Get
            Return _Clase
        End Get
        Set(ByVal value As System.Nullable(Of Char))
            _Clase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clase"))
        End Set
    End Property

    Private _Cliente As String
    Public Property Cliente() As String
        Get
            Return _Cliente
        End Get
        Set(ByVal value As String)
            _Cliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Cliente"))
        End Set
    End Property

    Private _NombreCliente As String
    Public Property NombreCliente() As String
        Get
            Return _NombreCliente
        End Get
        Set(ByVal value As String)
            _NombreCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCliente"))
        End Set
    End Property

    Private _Ordenante As String
    Public Property Ordenante() As String
        Get
            Return _Ordenante
        End Get
        Set(ByVal value As String)
            _Ordenante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ordenante"))
        End Set
    End Property

    Private _Tipo As System.Nullable(Of Char)
    Public Property Tipo() As System.Nullable(Of Char)
        Get
            Return _Tipo
        End Get
        Set(ByVal value As System.Nullable(Of Char))
            _Tipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo"))
        End Set
    End Property

    Private _Usuario As String
    Public Property Usuario() As String
        Get
            Return _Usuario
        End Get
        Set(ByVal value As String)
            _Usuario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Usuario"))
        End Set
    End Property

    Private _Cantidad As System.Nullable(Of Decimal)
    Public Property Cantidad() As System.Nullable(Of Decimal)
        Get
            Return _Cantidad
        End Get
        Set(ByVal value As System.Nullable(Of Decimal))
            _Cantidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Cantidad"))
        End Set
    End Property

    Private _Deposito As System.Nullable(Of Char)
    Public Property Deposito() As System.Nullable(Of Char)
        Get
            Return _Deposito
        End Get
        Set(ByVal value As System.Nullable(Of Char))
            _Deposito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Deposito"))
        End Set
    End Property

    Private _descDeposito As String
    Public Property descDeposito() As String
        Get
            Return _descDeposito
        End Get
        Set(ByVal value As String)
            _descDeposito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("descDeposito"))
        End Set
    End Property

    Private _CuentaDeposito As String
    Public Property CuentaDeposito() As String
        Get
            Return _CuentaDeposito
        End Get
        Set(ByVal value As String)
            _CuentaDeposito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaDeposito"))
        End Set
    End Property

    Private _lngidCuentaDeceval As System.Nullable(Of Integer)
    Public Property lngidCuentaDeceval() As System.Nullable(Of Integer)
        Get
            Return _lngidCuentaDeceval
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngidCuentaDeceval = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngidCuentaDeceval"))
        End Set
    End Property

    Private _TipoClasificacion As System.Nullable(Of Integer)
    Public Property TipoClasificacion() As System.Nullable(Of Integer)
        Get
            Return _TipoClasificacion
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _TipoClasificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoClasificacion"))
        End Set
    End Property

    Private _DescTipoClasificacion As String
    Public Property DescTipoClasificacion() As String
        Get
            Return _DescTipoClasificacion
        End Get
        Set(ByVal value As String)
            _DescTipoClasificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescTipoClasificacion"))
        End Set
    End Property

    Private _ObjetoClasificacion As String
    Public Property ObjetoClasificacion() As String
        Get
            Return _ObjetoClasificacion
        End Get
        Set(ByVal value As String)
            _ObjetoClasificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ObjetoClasificacion"))
        End Set
    End Property

    Private _Clasificacion As String
    Public Property Clasificacion() As String
        Get
            Return _Clasificacion
        End Get
        Set(ByVal value As String)
            _Clasificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clasificacion"))
        End Set
    End Property

    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property

    Private _NombreEspecie As String
    Public Property NombreEspecie() As String
        Get
            Return _NombreEspecie
        End Get
        Set(ByVal value As String)
            _NombreEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreEspecie"))
        End Set
    End Property

    Private _FechaIngreso As Date
    Public Property FechaIngreso() As Date
        Get
            Return _FechaIngreso
        End Get
        Set(ByVal value As Date)
            _FechaIngreso = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaIngreso"))
        End Set
    End Property

    Private _FechaVigencia As System.Nullable(Of Date)
    Public Property FechaVigencia() As System.Nullable(Of Date)
        Get
            Return _FechaVigencia
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _FechaVigencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaVigencia"))
        End Set
    End Property

    Private _FechaEmision As String
    Public Property FechaEmision() As String
        Get
            Return _FechaEmision
        End Get
        Set(ByVal value As String)
            _FechaEmision = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaEmision"))
        End Set
    End Property

    Private _FechaVencimiento As String
    Public Property FechaVencimiento() As String
        Get
            Return _FechaVencimiento
        End Get
        Set(ByVal value As String)
            _FechaVencimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaVencimiento"))
        End Set
    End Property

    Private _Receptor As String
    Public Property Receptor() As String
        Get
            Return _Receptor
        End Get
        Set(ByVal value As String)
            _Receptor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Receptor"))
        End Set
    End Property

    Private _Modalidad As String
    Public Property Modalidad() As String
        Get
            Return _Modalidad
        End Get
        Set(ByVal value As String)
            _Modalidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modalidad"))
        End Set
    End Property

    Private _TasaFacial As System.Nullable(Of Double)
    Public Property TasaFacial() As System.Nullable(Of Double)
        Get
            Return _TasaFacial
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _TasaFacial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TasaFacial"))
        End Set
    End Property

    Private _TLimite As String
    Public Property TLimite() As String
        Get
            Return _TLimite
        End Get
        Set(ByVal value As String)
            _TLimite = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TLimite"))
        End Set
    End Property

    Private _DescTLimite As String
    Public Property DescTLimite() As String
        Get
            Return _DescTLimite
        End Get
        Set(ByVal value As String)
            _DescTLimite = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescTLimite"))
        End Set
    End Property

    Private _CondNegociacion As String
    Public Property CondNegociacion() As String
        Get
            Return _CondNegociacion
        End Get
        Set(ByVal value As String)
            _CondNegociacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CondNegociacion"))
        End Set
    End Property

    Private _DescCondNegociacion As String
    Public Property DescCondNegociacion() As String
        Get
            Return _DescCondNegociacion
        End Get
        Set(ByVal value As String)
            _DescCondNegociacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescCondNegociacion"))
        End Set
    End Property

    Private _FormaPago As String
    Public Property FormaPago() As String
        Get
            Return _FormaPago
        End Get
        Set(ByVal value As String)
            _FormaPago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FormaPago"))
        End Set
    End Property

    Private _DescFormaPago As String
    Public Property DescFormaPago() As String
        Get
            Return _DescFormaPago
        End Get
        Set(ByVal value As String)
            _DescFormaPago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescFormaPago"))
        End Set
    End Property

    Private _TipoInversion As String
    Public Property TipoInversion() As String
        Get
            Return _TipoInversion
        End Get
        Set(ByVal value As String)
            _TipoInversion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoInversion"))
        End Set
    End Property

    Private _DescTipoInversion As String
    Public Property DescTipoInversion() As String
        Get
            Return _DescTipoInversion
        End Get
        Set(ByVal value As String)
            _DescTipoInversion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescTipoInversion"))
        End Set
    End Property

    Private _Ejecucion As String
    Public Property Ejecucion() As String
        Get
            Return _Ejecucion
        End Get
        Set(ByVal value As String)
            _Ejecucion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ejecucion"))
        End Set
    End Property

    Private _DescEjecucion As String
    Public Property DescEjecucion() As String
        Get
            Return _DescEjecucion
        End Get
        Set(ByVal value As String)
            _DescEjecucion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescEjecucion"))
        End Set
    End Property

    Private _Duracion As String
    Public Property Duracion() As String
        Get
            Return _Duracion
        End Get
        Set(ByVal value As String)
            _Duracion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Duracion"))
        End Set
    End Property

    Private _DescDuracion As String
    Public Property DescDuracion() As String
        Get
            Return _DescDuracion
        End Get
        Set(ByVal value As String)
            _DescDuracion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescDuracion"))
        End Set
    End Property

    Private _ReceptorLeo As String
    Public Property ReceptorLeo() As String
        Get
            Return _ReceptorLeo
        End Get
        Set(ByVal value As String)
            _ReceptorLeo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ReceptorLeo"))
        End Set
    End Property

    Private _CanalLeo As String
    Public Property CanalLeo() As String
        Get
            Return _CanalLeo
        End Get
        Set(ByVal value As String)
            _CanalLeo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CanalLeo"))
        End Set
    End Property

    Private _DescCanalLeo As String
    Public Property DescCanalLeo() As String
        Get
            Return _DescCanalLeo
        End Get
        Set(ByVal value As String)
            _DescCanalLeo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescCanalLeo"))
        End Set
    End Property

    Private _MedioVerificable As String
    Public Property MedioVerificable() As String
        Get
            Return _MedioVerificable
        End Get
        Set(ByVal value As String)
            _MedioVerificable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MedioVerificable"))
        End Set
    End Property

    Private _DescMedioVerificable As String
    Public Property DescMedioVerificable() As String
        Get
            Return _DescMedioVerificable
        End Get
        Set(ByVal value As String)
            _DescMedioVerificable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescMedioVerificable"))
        End Set
    End Property

    Private _Comision As System.Nullable(Of Double)
    Public Property Comision() As System.Nullable(Of Double)
        Get
            Return _Comision
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _Comision = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comision"))
        End Set
    End Property

    Private _strUsuarioArchivo As String
    Public Property strUsuarioArchivo() As String
        Get
            Return _strUsuarioArchivo
        End Get
        Set(ByVal value As String)
            _strUsuarioArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strUsuarioArchivo"))
        End Set
    End Property

    Private _bitGenerarOrden As Boolean
    Public Property bitGenerarOrden() As Boolean
        Get
            Return _bitGenerarOrden
        End Get
        Set(ByVal value As Boolean)
            _bitGenerarOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitGenerarOrden"))
        End Set
    End Property

    Private _lstComboClasificacion As List(Of OyDImportaciones.ObjetoClasificacion)
    Public Property lstComboClasificacion() As List(Of OyDImportaciones.ObjetoClasificacion)
        Get
            Return _lstComboClasificacion
        End Get
        Set(ByVal value As List(Of OyDImportaciones.ObjetoClasificacion))
            _lstComboClasificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lstComboClasificacion"))
        End Set
    End Property
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
