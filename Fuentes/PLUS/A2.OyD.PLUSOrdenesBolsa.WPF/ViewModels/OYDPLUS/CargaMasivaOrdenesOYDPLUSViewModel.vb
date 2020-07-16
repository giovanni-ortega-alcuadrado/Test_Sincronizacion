Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
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
Imports A2MCCOREWPF
Imports A2OYDPLUSOrdenesBolsa
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Public Class CargaMasivaOrdenesOYDPLUSViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext()
                dcProxyConsulta = New OYDPLUSOrdenesBolsaDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
                mdcProxyUtilidadPLUS = New OyDPLUSutilidadesDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext
                mdcProxyUtilidad03 = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyConsulta = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidadPLUS = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
                mdcProxyUtilidad03 = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxyConsulta.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidadPLUS.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If System.Diagnostics.Debugger.IsAttached Then
                RF_TTV_Regreso = "4"
                RF_TTV_Salida = "3"
                RF_REPO = "RP"
                RF_Simulatena_Regreso = "2"
                RF_Simulatena_Salida = "1"
                RF_MERCADO_REPO = "E"
                RF_MERCADO_PRIMARIO = "P"
                RF_MERCADO_RENOVACION = "R"
                RF_MERCADO_SECUNDARIO = "S"

                TIPONEGOCIO_ACCIONES = "A"
                TIPONEGOCIO_RENTAFIJA = "C"
                TIPONEGOCIO_REPO = "R"
                TIPONEGOCIO_SIMULTANEA = "S"
                TIPONEGOCIO_TTV = "TTV"
                TIPONEGOCIO_ADR = "ADR"

                TIPONEGOCIO_TTVC = "TTVC"
                TIPONEGOCIO_REPOC = "RC"

                'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añaden los tipos de negocio para otras firmas acciones y renta fija
                TIPONEGOCIO_ACCIONESOF = "AO"
                TIPONEGOCIO_RENTAFIJAOF = "CO"


                TIPOORDEN_DIRECTA = "D"
                TIPOORDEN_INDIRECTA = "I"

                TIPOMERCADO_PRECIO_ESPECIE = "M"
                TIPOMERCADO_PORLOMEJOR_PRECIO_ESPECIE = "P"
                CLASIFICACIONXDEFECTO = "O"
                TIPOLIMITEXDEFECTO = "M"
                CONDNEGOCIACIONXDEFECTO = "C"
                TIPOINVERSIONXDEFECTO = "N"
                EJECUCIONXDEFECTO = "N"
                DURACIONXDEFECTO = "I"
                MERCADOXDEFECTO = "S"

                TIPOPRODUCTO_CUENTAPROPIA = "CP"

                VELOCIDAD_TEXTO = "0:00:40"
            Else
                RF_TTV_Regreso = RetornarValorProgram(Program.RF_TTV_Regreso, "4")
                RF_TTV_Salida = RetornarValorProgram(Program.RF_TTV_Salida, "3")
                RF_REPO = RetornarValorProgram(Program.RF_REPO, "RP")
                RF_Simulatena_Regreso = RetornarValorProgram(Program.RF_Simulatena_Regreso, "2")
                RF_Simulatena_Salida = RetornarValorProgram(Program.RF_Simulatena_Salida, "1")
                RF_MERCADO_REPO = RetornarValorProgram(Program.RF_Mercado_Repo, "E")
                RF_MERCADO_PRIMARIO = RetornarValorProgram(Program.RF_Mercado_Primario, "P")
                RF_MERCADO_RENOVACION = RetornarValorProgram(Program.RF_Mercado_Renovacion, "R")
                RF_MERCADO_SECUNDARIO = RetornarValorProgram(Program.RF_Mercado_Secundario, "S")

                TIPONEGOCIO_ACCIONES = RetornarValorProgram(Program.TN_Acciones, "A")
                TIPONEGOCIO_RENTAFIJA = RetornarValorProgram(Program.TN_Renta_Fija, "C")
                TIPONEGOCIO_REPO = RetornarValorProgram(Program.TN_REPO, "R")
                TIPONEGOCIO_SIMULTANEA = RetornarValorProgram(Program.TN_Simultaneas, "S")
                TIPONEGOCIO_TTV = RetornarValorProgram(Program.TN_TTV, "TTV")
                TIPONEGOCIO_ADR = RetornarValorProgram(Program.TN_ADR, "ADR")

                TIPONEGOCIO_TTVC = RetornarValorProgram(Program.TN_TTVC, "TTVC")
                TIPONEGOCIO_REPOC = RetornarValorProgram(Program.TN_REPOC, "RC")

                'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añaden los tipos de negocio para otras firmas acciones y renta fija
                TIPONEGOCIO_ACCIONESOF = RetornarValorProgram(Program.TN_Acciones_OF, "AO")
                TIPONEGOCIO_RENTAFIJAOF = RetornarValorProgram(Program.TN_Renta_Fija_OF, "CO")

                TIPOORDEN_DIRECTA = RetornarValorProgram(Program.TO_Directa, "D")
                TIPOORDEN_INDIRECTA = RetornarValorProgram(Program.TO_Indirecta, "I")

                TIPOMERCADO_PRECIO_ESPECIE = RetornarValorProgram(Program.TM_PRECIO_ESPECIE, "M")
                TIPOMERCADO_PORLOMEJOR_PRECIO_ESPECIE = RetornarValorProgram(Program.TM_PORLOMEJOR_PRECIO_ESPECIE, "P")
                CLASIFICACIONXDEFECTO = RetornarValorProgram(Program.CLASIFICACIONXDEFECTO_ORDEN, "O")
                TIPOLIMITEXDEFECTO = RetornarValorProgram(Program.TIPOLIMITEXDEFECTO_ORDEN, "M")
                CONDNEGOCIACIONXDEFECTO = RetornarValorProgram(Program.CONDNEGOCIACIONXDEFECTO_ORDEN, "C")
                TIPOINVERSIONXDEFECTO = RetornarValorProgram(Program.TIPOINVERSIONXDEFECTO_ORDEN, "N")
                EJECUCIONXDEFECTO = RetornarValorProgram(Program.EJECUCIONXDEFECTO_ORDEN, "N")
                DURACIONXDEFECTO = RetornarValorProgram(Program.DURACIONXDEFECTO_ORDEN, "I")
                MERCADOXDEFECTO = RetornarValorProgram(Program.MERCADOXDEFECTO_ORDEN, "S")

                TIPOPRODUCTO_CUENTAPROPIA = RetornarValorProgram(Program.TIPOPRODUCTO_CUENTAPROPIA_ORDEN, "CP")

                VELOCIDAD_TEXTO = RetornarValorProgram(Program.VELOCIDADTEXTO_TICKER_ORDEN, "0:00:40")
            End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de orden que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                IsBusy = True
                mdtmFechaCierreSistema = DateAdd(DateInterval.Year, -5, Now()).Date
                If Not IsNothing(mdcProxyUtilidadPLUS.CombosReceptors) Then
                    mdcProxyUtilidadPLUS.CombosReceptors.Clear()
                End If
                mdcProxyUtilidadPLUS.Load(mdcProxyUtilidadPLUS.OYDPLUS_ConsultarCombosReceptorQuery(String.Empty, Program.Usuario, Program.HashConexion),
                                          AddressOf TerminoConsultarCombosOYDCompleto, String.Empty)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CargaMasivaOrdenesOYDPLUSViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String)
        Dim objRetorno As String = String.Empty

        If Not String.IsNullOrEmpty(strProgram) Then
            objRetorno = strProgram
        Else
            objRetorno = strRetornoOpcional
        End If

        Return objRetorno
    End Function

#End Region

#Region "Constantes"

    Private Enum TIPOMENSAJEUSUARIO
        CONFIRMACION
        JUSTIFICACION
        APROBACION
        TODOS
    End Enum
    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17
    Private MINT_LONG_MAX_NEMOTECNICO As Byte = 15
    Public MSTR_CALCULAR_DIAS_ORDEN As String = "vencimiento_orden"
    Friend Const MSTR_CALCULAR_DIAS_TITULO As String = "vencimiento_titulo"
    Private Const MSTR_ACCION_CALCULAR_DIAS As String = "dias"
    Private Const MSTR_ACCION_VALIDACION_GUARDADO_ORDEN As String = "guardado_orden"
    Private Const MSTR_ACCION_VALIDACION_FECHA_CIERRE As String = "fecha_cierre"
    Private Const MSTR_ACCION_CALCULAR_FECHA As String = "fecha"

    Private RF_TTV_Regreso As String = ""
    Private RF_TTV_Salida As String = ""
    Private RF_REPO As String = ""
    Private RF_Simulatena_Regreso As String = ""
    Private RF_Simulatena_Salida As String = ""

    Private RF_MERCADO_REPO As String = ""
    Private RF_MERCADO_PRIMARIO As String = ""
    Private RF_MERCADO_RENOVACION As String = ""
    Private RF_MERCADO_SECUNDARIO As String = ""

    Public TIPONEGOCIO_ACCIONES As String = ""
    Public TIPONEGOCIO_REPO As String = ""
    Public TIPONEGOCIO_SIMULTANEA As String = ""
    Public TIPONEGOCIO_RENTAFIJA As String = ""
    Public TIPONEGOCIO_TTV As String = ""
    Public TIPONEGOCIO_REPOC As String = ""
    Public TIPONEGOCIO_TTVC As String = ""
    Public TIPONEGOCIO_ADR As String = ""

    'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añaden los tipos de negocio para otras firmas acciones y renta fija
    Public TIPONEGOCIO_ACCIONESOF As String = ""
    Public TIPONEGOCIO_RENTAFIJAOF As String = ""

    Private TIPOORDEN_DIRECTA As String = ""
    Private TIPOORDEN_INDIRECTA As String = ""

    Private CLASE_ACCIONES As String = "A"
    Private CLASE_RENTAFIJA As String = "C"

    Private TIPOOPERACION_COMPRA As String = "C"
    Private TIPOOPERACION_VENTA As String = "V"
    Private TIPOOPERACION_RECOMPRA As String = "R"
    Private TIPOOPERACION_REVENTA As String = "S"

    Private TIPOTASA_VARIABLE As String = "V"
    Private TIPOTASA_FIJA As String = "F"

    Private ESTADOORDEN_PENDIENTE As String = "P"
    Private ESTADOORDENLEO_RECIBIDA As String = "R"
    Private ESTADOORDENLEO_LANZADA As String = "L"

    Private ESTADOPENDIENTE_CONSULTA As String = "D"
    Private ESTADOAPROBADA_CONSULTA As String = "P"

    Private TIPOMERCADO_PRECIO_ESPECIE As String = ""
    Private TIPOMERCADO_PORLOMEJOR_PRECIO_ESPECIE As String = ""
    Private CLASIFICACIONXDEFECTO As String = ""
    Private TIPOLIMITEXDEFECTO As String = ""
    Private CONDNEGOCIACIONXDEFECTO As String = ""
    Private TIPOINVERSIONXDEFECTO As String = ""
    Private EJECUCIONXDEFECTO As String = ""
    Private DURACIONXDEFECTO As String = ""
    Private MERCADOXDEFECTO As String = ""

    Private DURACION_HASTAHORA As String = "A"
    Private DURACION_DIA As String = "D"
    Private DURACION_INMEDIATA As String = "I"
    Private DURACION_SESSION As String = "S"
    Private DURACION_CANCELACION As String = "C"
    Private DURACION_FECHA As String = "F"

    Private TIPOPRODUCTO_CUENTAPROPIA As String = ""

    Private MSTR_MODULO_OYD_ORDENES As String = "O"

    Public OPCION_ORDENCRUZADA As String = "ORDENCRUZADA"

    Private VELOCIDAD_TEXTO As String = ""

    Private STR_URLMOTORCALCULOS As String = ""
    Private LOG_HACERLOGMOTORCALCULOS As Boolean = False
    Private STR_RUTALOGMOTORCALCULOS As String = ""

    Private Const VISTA_APROBADAS As String = "Aprobadas"
    Private Const VISTA_PENDIENTESAPROBAR As String = "Pendientes aprobar"
    Private Const VISTA_PENDIENTESCRUZAR As String = "Pendientes por cruzar"

    Private Const MOD_BOLSA As String = "ORDENES"
    Private Const MOD_OTRAS_FIRMAS As String = "ORDENES_OF"

    Private Const OPCION_EDITAR As String = "EDITAR"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    Private Const OPCION_TIPONEGOCIO As String = "TIPONEGOCIO"
    Private Const OPCION_RECEPTOR As String = "RECEPTOR"
    Private Const OPCION_TIPOOPERACION As String = "TIPOOPERACION"
    Private Const OPCION_PLANTILLA As String = "PLANTILLA"
    Private Const OPCION_CREARORDENPLANTILLA As String = "CREARORDENPLANTILLA"
    Private Const OPCION_COMBOSRECEPTOR As String = "COMBOSRECEPTOR"
    Public Enum TipoOpcionCargar
        ARCHIVO
        CAMPOSORDENES
        RESULTADO
        COMPLEMENTACION_PRECIOPROMEDIO
    End Enum

#End Region

#Region "Variables"
    Dim mdtmFechaCierreSistema As Date

    Dim logCargoForma As Boolean = False
    Private dcProxy As OYDPLUSOrdenesBolsaDomainContext
    Private dcProxyConsulta As OYDPLUSOrdenesBolsaDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private mdcProxyUtilidadPLUS As OyDPLUSutilidadesDomainContext
    Private mdcProxyUtilidad03 As OyDPLUSutilidadesDomainContext
    Private dcProxyImportaciones As ImportacionesDomainContext
    Dim logConsultarCliente As Boolean = True
    Public logNuevoRegistro As Boolean = False
    Public logEditarRegistro As Boolean = False
    Dim logCancelarRegistro As Boolean = False
    Dim logDuplicarRegistro As Boolean = False
    Dim logPlantillaRegistro As Boolean = False
    Public logModificarDatosTipoNegocio As Boolean = True
    Public viewCargaMasiva As CargaMasivaView
    Dim cantidadTotalConfirmacion As Integer = 0
    Dim cantidadTotalJustificacion As Integer = 0
    Dim CantidadTotalAprobaciones As Integer = 0
    Dim intCantidadMaximaDetalles As Integer = 30
    Dim dblValorIva As Double = 0
    Dim dblValorBase As Double = 0
    Dim dblValorBaseRepo As Double = 0
    Dim logRecargarResultados As Boolean = False
    Dim logCargarContenido As Boolean = True
    Dim strTemporalTipoNegocio As String = String.Empty
    Dim strTemporalTipoOperacion As String = String.Empty
    Dim logCalculoValores As Boolean = True
    Dim dtmFechaServidor As DateTime
    Dim logLlevarHoraActualRecepcion As Boolean = True
    Dim logLlevarPorDefectoReceptorTomaDeReceptor As Boolean = False
    Dim logLlevarPorDefectoUsuarioOperador As Boolean = False
    Dim strLiquidacionesProbables As String = String.Empty
#End Region

#Region "Propiedades Ordenes OYDPLUS"

    'Propiedades Complementacion por Precio Promedio JDOL20170307
    '****************************************************************************************************************************************************************
    Private _PrecioPromedioLiquidaciones As Double = 0
    Public Property PrecioPromedioLiquidaciones() As Double
        Get
            Return _PrecioPromedioLiquidaciones
        End Get
        Set(ByVal value As Double)
            _PrecioPromedioLiquidaciones = value
            MyBase.CambioItem("PrecioPromedioLiquidaciones")
        End Set
    End Property

    Private _SumatoriaCantidadLiquidaciones As Double = 0
    Public Property SumatoriaCantidadLiquidaciones() As Double
        Get
            Return _SumatoriaCantidadLiquidaciones
        End Get
        Set(ByVal value As Double)
            _SumatoriaCantidadLiquidaciones = value
            MyBase.CambioItem("SumatoriaCantidadLiquidaciones")
        End Set
    End Property
    Private _SumatoriaCantidadOrdenes As Double = 0
    Public Property SumatoriaCantidadOrdenes() As Double
        Get
            Return _SumatoriaCantidadOrdenes
        End Get
        Set(ByVal value As Double)
            _SumatoriaCantidadOrdenes = value
            MyBase.CambioItem("SumatoriaCantidadOrdenes")
        End Set
    End Property



    Private _SeleccionarTodos As Boolean = False
    Public Property SeleccionarTodos() As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value
            If Not IsNothing(_SeleccionarTodos) Then
                If Not IsNothing(_ListaOrdenSAEAcciones) Then
                    If _ListaOrdenSAEAcciones.Count > 0 Then
                        If _SeleccionarTodos Then
                            For Each li In ListaOrdenSAEAcciones
                                li.Seleccionada = True
                            Next
                            CalcularSumatoriaLiquidaciones()
                        Else
                            For Each li In ListaOrdenSAEAcciones
                                li.Seleccionada = False
                            Next
                            CalcularSumatoriaLiquidaciones()
                        End If
                    End If

                End If
            End If

            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property

    Private _AccionCargaMasiva As String = ">>Seleccionando Archivo"
    Public Property AccionCargaMasiva() As String
        Get
            Return _AccionCargaMasiva
        End Get
        Set(ByVal value As String)
            _AccionCargaMasiva = value
            MyBase.CambioItem("AccionCargaMasiva")
        End Set
    End Property

    Private _EspecieComplementacionPrecioPromedio As String = ""
    Public Property EspecieComplementacionPrecioPromedio() As String
        Get
            Return _EspecieComplementacionPrecioPromedio
        End Get
        Set(ByVal value As String)
            _EspecieComplementacionPrecioPromedio = value
            MyBase.CambioItem("EspecieComplementacionPrecioPromedio")
        End Set
    End Property

    Private _ConsultarOrdenesSAE As Boolean = False
    Public Property ConsultarOrdenesSAE() As Boolean
        Get
            Return _ConsultarOrdenesSAE
        End Get
        Set(ByVal value As Boolean)
            _ConsultarOrdenesSAE = value
            MyBase.CambioItem("ConsultarOrdenesSAE")
        End Set
    End Property

    Private _OrdenSAEAccionesSelected As OYDPLUSUtilidades.tblOrdenesSAEAcciones
    Public Property OrdenSAEAccionesSelected() As OYDPLUSUtilidades.tblOrdenesSAEAcciones
        Get
            Return _OrdenSAEAccionesSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOrdenesSAEAcciones)
            _OrdenSAEAccionesSelected = value
            CalcularSumatoriaLiquidaciones()
            MyBase.CambioItem("OrdenSAEAccionesSelected")
        End Set
    End Property
    Private _OrdenSAESeleccionada As Boolean
    Public Property OrdenSAESeleccionada() As Boolean
        Get
            Return _OrdenSAESeleccionada
        End Get
        Set(ByVal value As Boolean)
            _OrdenSAESeleccionada = value
            CalcularSumatoriaLiquidaciones()
            MyBase.CambioItem("OrdenSAESeleccionada")
        End Set
    End Property
    Private _ListaOrdenSAEAcciones As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
    Public Property ListaOrdenSAEAcciones() As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
        Get
            Return _ListaOrdenSAEAcciones
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones))
            _ListaOrdenSAEAcciones = value
            MyBase.CambioItem("ListaOrdenSAEAcciones")
        End Set
    End Property
    Private _VerContinuarComplementacionPrecioPromedio As Visibility = Visibility.Collapsed
    Public Property VerContinuarComplementacionPrecioPromedio() As Visibility
        Get
            Return _VerContinuarComplementacionPrecioPromedio
        End Get
        Set(ByVal value As Visibility)
            _VerContinuarComplementacionPrecioPromedio = value
            MyBase.CambioItem("VerContinuarComplementacionPrecioPromedio")
        End Set
    End Property

    Private _HabilitarComplementacionPrecioPromedio As Boolean = False
    Public Property HabilitarComplementacionPrecioPromedio() As Boolean
        Get
            Return _HabilitarComplementacionPrecioPromedio
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComplementacionPrecioPromedio = value
            MyBase.CambioItem("HabilitarComplementacionPrecioPromedio")
        End Set
    End Property
    Private _ComplementacionPrecioPromedio As Boolean = False
    Public Property ComplementacionPrecioPromedio() As Boolean
        Get
            Return _ComplementacionPrecioPromedio
        End Get
        Set(ByVal value As Boolean)
            _ComplementacionPrecioPromedio = value
            MyBase.CambioItem("ComplementacionPrecioPromedio")
        End Set
    End Property
    Private _VerComplementacionPrecioPromedio As Visibility = Visibility.Collapsed
    Public Property VerComplementacionPrecioPromedio() As Visibility
        Get
            Return _VerComplementacionPrecioPromedio
        End Get
        Set(ByVal value As Visibility)
            _VerComplementacionPrecioPromedio = value
            MyBase.CambioItem("VerComplementacionPrecioPromedio")
        End Set
    End Property
    '***************************************************************************************************************************************************************
    Private _MensajeComision As String = String.Empty
    Public Property MensajeComision() As String
        Get
            Return _MensajeComision
        End Get
        Set(ByVal value As String)
            _MensajeComision = value
            MyBase.CambioItem("MensajeComision")
        End Set
    End Property
    Private _MostrarMensajeComision As Visibility = Visibility.Collapsed
    Public Property MostrarMensajeComision() As Visibility
        Get
            Return _MostrarMensajeComision
        End Get
        Set(ByVal value As Visibility)
            _MostrarMensajeComision = value
            MyBase.CambioItem("MostrarMensajeComision")
        End Set
    End Property

    Private _IsbusyResultados As Boolean = False
    Public Property IsbusyResultados() As Boolean
        Get
            Return _IsbusyResultados
        End Get
        Set(ByVal value As Boolean)
            _IsbusyResultados = value
            MyBase.CambioItem("IsbusyResultados")
        End Set
    End Property


    ''' <summary>
    ''' Propiedad que permite manejar las opciones del modulo del combo cboModulo
    ''' </summary>
    ''' <remarks>Por: Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _Modulo As String = "ORDENES"
    <Display(Name:="Módulo")> _
    Public Property Modulo() As String
        Get
            Return _Modulo
        End Get
        Set(ByVal value As String)
            _Modulo = value
            MyBase.CambioItem("Modulo")
        End Set
    End Property

    Private _CantidadJustificaciones As Integer = 0
    Public Property CantidadJustificaciones() As Integer
        Get
            Return _CantidadJustificaciones
        End Get
        Set(ByVal value As Integer)
            _CantidadJustificaciones = value
            If CantidadJustificaciones > 0 Then
                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
                    EjecutarConfirmacion()
                End If
            End If
        End Set
    End Property

    Private _CantidadAprobaciones As Integer = 0
    Public Property CantidadAprobaciones() As Integer
        Get
            Return _CantidadAprobaciones
        End Get
        Set(ByVal value As Integer)
            _CantidadAprobaciones = value
            If CantidadAprobaciones > 0 Then
                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
                    EjecutarConfirmacion()
                End If
            End If
        End Set
    End Property

    Private _Confirmaciones As String = String.Empty
    Public Property Confirmaciones() As String
        Get
            Return _Confirmaciones
        End Get
        Set(ByVal value As String)
            _Confirmaciones = value
        End Set
    End Property

    Private _ConfirmacionesUsuario As String = String.Empty
    Public Property ConfirmacionesUsuario() As String
        Get
            Return _ConfirmacionesUsuario
        End Get
        Set(ByVal value As String)
            _ConfirmacionesUsuario = value
        End Set
    End Property

    Private _Justificaciones As String = String.Empty
    Public Property Justificaciones() As String
        Get
            Return _Justificaciones
        End Get
        Set(ByVal value As String)
            _Justificaciones = value
        End Set
    End Property

    Private _JustificacionesUsuario As String = String.Empty
    Public Property JustificacionesUsuario() As String
        Get
            Return _JustificacionesUsuario
        End Get
        Set(ByVal value As String)
            _JustificacionesUsuario = value
        End Set
    End Property

    Private _Aprobaciones As String
    Public Property Aprobaciones() As String
        Get
            Return _Aprobaciones
        End Get
        Set(ByVal value As String)
            _Aprobaciones = value
        End Set
    End Property

    Private _AprobacionesUsuario As String
    Public Property AprobacionesUsuario() As String
        Get
            Return _AprobacionesUsuario
        End Get
        Set(ByVal value As String)
            _AprobacionesUsuario = value
        End Set
    End Property
    Private _CantidadConfirmaciones As Integer = 0
    Public Property CantidadConfirmaciones() As Integer
        Get
            Return _CantidadConfirmaciones
        End Get
        Set(ByVal value As Integer)
            _CantidadConfirmaciones = value
            If CantidadConfirmaciones > 0 Then
                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
                    EjecutarConfirmacion()
                End If
            End If
        End Set
    End Property
    Private _ListaResultadoValidacion As List(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones)
    Public Property ListaResultadoValidacion() As List(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
        End Set
    End Property
    Private _ListaResultadosImportacion As List(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones)
    Public Property ListaResultadosImportacion() As List(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones)
        Get
            Return _ListaResultadosImportacion
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones))
            _ListaResultadosImportacion = value
            MyBase.CambioItem("ListaResultadosImportacion")
            MyBase.CambioItem("ListaResultadosImportacion_Paged")
        End Set
    End Property

    Public ReadOnly Property ListaResultadosImportacion_Paged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultadosImportacion) Then
                Dim view = New PagedCollectionView(_ListaResultadosImportacion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
#Region "Propiedades para los textos de los combos y titulos"

    Private _TextoCantidad As String = "Cantidad"
    Public Property TextoCantidad() As String
        Get
            Return _TextoCantidad
        End Get
        Set(ByVal value As String)
            _TextoCantidad = value
            MyBase.CambioItem("TextoCantidad")
        End Set
    End Property

    Private _TextoPrecio As String = "Precio"
    Public Property TextoPrecio() As String
        Get
            Return _TextoPrecio
        End Get
        Set(ByVal value As String)
            _TextoPrecio = value
            MyBase.CambioItem("TextoPrecio")
        End Set
    End Property

    Private _TextoPrecioMaximoMinimo As String = "Precio"
    Public Property TextoPrecioMaximoMinimo() As String
        Get
            Return _TextoPrecioMaximoMinimo
        End Get
        Set(ByVal value As String)
            _TextoPrecioMaximoMinimo = value
            MyBase.CambioItem("TextoPrecioMaximoMinimo")
        End Set
    End Property

    Private _TextoValorCaptacionGiro As String = "Valor"
    Public Property TextoValorCaptacionGiro() As String
        Get
            Return _TextoValorCaptacionGiro
        End Get
        Set(ByVal value As String)
            _TextoValorCaptacionGiro = value
            MyBase.CambioItem("TextoValorCaptacionGiro")
        End Set
    End Property

    Private _TextoSaldoPortafolio As String = "Saldo cliente"
    Public Property TextoSaldoPortafolio() As String
        Get
            Return _TextoSaldoPortafolio
        End Get
        Set(ByVal value As String)
            _TextoSaldoPortafolio = value
            MyBase.CambioItem("TextoSaldoPortafolio")
        End Set
    End Property

    Private _TextoComision As String = "% comsión"
    Public Property TextoComision() As String
        Get
            Return _TextoComision
        End Get
        Set(ByVal value As String)
            _TextoComision = value
            MyBase.CambioItem("TextoComision")
        End Set
    End Property

    Private _TextoTasaNominal As String = "Tasa nominal"
    Public Property TextoTasaNominal() As String
        Get
            Return _TextoTasaNominal
        End Get
        Set(ByVal value As String)
            _TextoTasaNominal = value
            MyBase.CambioItem("TextoTasaNominal")
        End Set
    End Property


    Private _TextoRomperCruzada As String = "* La orden cruzada ya no tiene cruce activo."
    Public Property TextoRomperCruzada() As String
        Get
            Return _TextoRomperCruzada
        End Get
        Set(ByVal value As String)
            _TextoRomperCruzada = value
            MyBase.CambioItem("TextoRomperCruzada")
        End Set
    End Property

    Private _TextoCruzadaReceptor As String = "Orden cruzada con otro receptor"
    Public Property TextoCruzadaReceptor() As String
        Get
            Return _TextoCruzadaReceptor
        End Get
        Set(ByVal value As String)
            _TextoCruzadaReceptor = value
            MyBase.CambioItem("TextoCruzadaReceptor")
        End Set
    End Property

    Private _TextoCruzadaCliente As String = "Orden cruzada con uno de mis clientes"
    Public Property TextoCruzadaCliente() As String
        Get
            Return _TextoCruzadaCliente
        End Get
        Set(ByVal value As String)
            _TextoCruzadaCliente = value
            MyBase.CambioItem("TextoCruzadaCliente")
        End Set
    End Property


#End Region
    Private _DiccionarioEdicionCamposOYDPLUS As Dictionary(Of String, Boolean)
    Public Property DiccionarioEdicionCamposOYDPLUS() As Dictionary(Of String, Boolean)
        Get
            Return _DiccionarioEdicionCamposOYDPLUS
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            _DiccionarioEdicionCamposOYDPLUS = value
            MyBase.CambioItem("DiccionarioEdicionCamposOYDPLUS")
        End Set
    End Property
    Private _ListaComboCamposPermitidosEdicion As List(Of OyDPLUSOrdenesBolsa.CamposEditablesOrden)
    Public Property ListaComboCamposPermitidosEdicion() As List(Of OyDPLUSOrdenesBolsa.CamposEditablesOrden)
        Get
            Return _ListaComboCamposPermitidosEdicion
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.CamposEditablesOrden))
            _ListaComboCamposPermitidosEdicion = value
            MyBase.CambioItem("ListaComboCamposPermitidosEdicion")
        End Set
    End Property
    Private _VerAtras As Visibility = Visibility.Collapsed
    Public Property VerAtras() As Visibility
        Get
            Return _VerAtras
        End Get
        Set(ByVal value As Visibility)
            _VerAtras = value
            MyBase.CambioItem("VerAtras")
        End Set
    End Property
    Private _VerGrabar As Visibility = Visibility.Collapsed
    Public Property VerGrabar() As Visibility
        Get
            Return _VerGrabar
        End Get
        Set(ByVal value As Visibility)
            _VerGrabar = value
            MyBase.CambioItem("VerGrabar")
        End Set
    End Property



    Private _HabilitarTipoNegocio As Boolean = True
    Public Property HabilitarTipoNegocio() As Boolean
        Get
            Return _HabilitarTipoNegocio
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoNegocio = value
            MyBase.CambioItem("HabilitarTipoNegocio")
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
    Private _ListaReceptoresCompleta As List(Of OYDPLUSUtilidades.tblReceptoresUsuario)
    Public Property ListaReceptoresCompleta() As List(Of OYDPLUSUtilidades.tblReceptoresUsuario)
        Get
            Return _ListaReceptoresCompleta
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblReceptoresUsuario))
            _ListaReceptoresCompleta = value
            MyBase.CambioItem("ListaReceptoresCompleta")
        End Set
    End Property
    Private _mlogEsOrdenRENTAFIJAOYDPLUS As Boolean = False
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de renta variable (true) o no (false)
    ''' </summary>
    Public ReadOnly Property EsOrdenRENTAFIJAOYDPLUS As Boolean
        Get
            Return (_mlogEsOrdenRENTAFIJAOYDPLUS)
        End Get
    End Property
    Private _HabilitarPrecioRepo As Boolean = False
    Public Property HabilitarPrecioRepo() As Boolean
        Get
            Return _HabilitarPrecioRepo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarPrecioRepo = value
            MyBase.CambioItem("HabilitarPrecioRepo")
        End Set
    End Property
    Private _MostrarCamposRentaFija As Visibility = Visibility.Collapsed
    Public Property MostrarCamposRentaFija() As Visibility
        Get
            Return _MostrarCamposRentaFija
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposRentaFija = value
            MyBase.CambioItem("MostrarCamposRentaFija")
        End Set
    End Property

    Private _MostrarTipoNegocioAcciones As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioAcciones() As Visibility
        Get
            Return _MostrarTipoNegocioAcciones
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioAcciones = value
            MyBase.CambioItem("MostrarTipoNegocioAcciones")
        End Set
    End Property

    Private _MostrarTipoNegocioRentaFija As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioRentaFija() As Visibility
        Get
            Return _MostrarTipoNegocioRentaFija
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioRentaFija = value
            MyBase.CambioItem("MostrarTipoNegocioRentaFija")
        End Set
    End Property

    Private _MostrarTipoNegocioRepo As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioRepo() As Visibility
        Get
            Return _MostrarTipoNegocioRepo
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioRepo = value
            MyBase.CambioItem("MostrarTipoNegocioRepo")
        End Set
    End Property

    Private _MostrarTipoNegocioSimultanea As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioSimultanea() As Visibility
        Get
            Return _MostrarTipoNegocioSimultanea
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioSimultanea = value
            MyBase.CambioItem("MostrarTipoNegocioSimultanea")
        End Set
    End Property

    Private _MostrarTipoNegocioTTV As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioTTV() As Visibility
        Get
            Return _MostrarTipoNegocioTTV
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioTTV = value
            MyBase.CambioItem("MostrarTipoNegocioTTV")
        End Set
    End Property

    'Santiago Vergara - Marzo 04/2014
    Private _MostrarTipoNegocioAccionesOF As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioAccionesOF() As Visibility
        Get
            Return _MostrarTipoNegocioAccionesOF
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioAccionesOF = value
            MyBase.CambioItem("MostrarTipoNegocioAccionesOF")
        End Set
    End Property

    'Santiago Vergara - Marzo 04/2014
    Private _MostrarTipoNegocioRentaFijaOF As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioRentaFijaOF() As Visibility
        Get
            Return _MostrarTipoNegocioRentaFijaOF
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioRentaFijaOF = value
            MyBase.CambioItem("MostrarTipoNegocioRentaFijaOF")
        End Set
    End Property

    Private _MostrarCamposFaciales As Visibility = Visibility.Collapsed
    Public Property MostrarCamposFaciales() As Visibility
        Get
            Return _MostrarCamposFaciales
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposFaciales = value
            MyBase.CambioItem("MostrarCamposFaciales")
        End Set
    End Property

    Private _MostrarAdicionalesEspecie As Visibility = Visibility.Collapsed
    Public Property MostrarAdicionalesEspecie() As Visibility
        Get
            Return _MostrarAdicionalesEspecie
        End Get
        Set(ByVal value As Visibility)
            _MostrarAdicionalesEspecie = value
            MyBase.CambioItem("MostrarAdicionalesEspecie")
        End Set
    End Property

    Private _HabilitarHoraVigencia As Boolean = False
    Public Property HabilitarHoraVigencia() As Boolean
        Get
            Return _HabilitarHoraVigencia
        End Get
        Set(ByVal value As Boolean)
            _HabilitarHoraVigencia = value
            MyBase.CambioItem("HabilitarHoraVigencia")
        End Set
    End Property

    Private _MostrarControlMensajes As Visibility = Visibility.Collapsed
    Public Property MostrarControlMensajes() As Visibility
        Get
            Return _MostrarControlMensajes
        End Get
        Set(ByVal value As Visibility)
            _MostrarControlMensajes = value
            MyBase.CambioItem("MostrarControlMensajes")
        End Set
    End Property

    Private _MostrarControles As Visibility = Visibility.Collapsed
    Public Property MostrarControles() As Visibility
        Get
            Return _MostrarControles
        End Get
        Set(ByVal value As Visibility)
            _MostrarControles = value
            MyBase.CambioItem("MostrarControles")
        End Set
    End Property

    Private _HabilitarFechaVigencia As Boolean = False
    Public Property HabilitarFechaVigencia() As Boolean
        Get
            Return _HabilitarFechaVigencia
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaVigencia = value
            MyBase.CambioItem("HabilitarFechaVigencia")
        End Set
    End Property

    Private _HabilitarDias As Boolean = False
    Public Property HabilitarDias() As Boolean
        Get
            Return _HabilitarDias
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDias = value
            MyBase.CambioItem("HabilitarDias")
        End Set
    End Property

    Private _HabilitarValor As Boolean
    Public Property HabilitarValor() As Boolean
        Get
            Return _HabilitarValor
        End Get
        Set(ByVal value As Boolean)
            _HabilitarValor = value
            MyBase.CambioItem("HabilitarValor")
        End Set
    End Property

    Private _MostrarCamposCuentaPropia As Visibility = Visibility.Collapsed
    Public Property MostrarCamposCuentaPropia() As Visibility
        Get
            Return _MostrarCamposCuentaPropia
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposCuentaPropia = value
            MyBase.CambioItem("MostrarCamposCuentaPropia")
        End Set
    End Property
    Private _MostrarOrdenesSAE As Visibility = Visibility.Collapsed
    Public Property MostrarOrdenesSAE() As Visibility
        Get
            Return _MostrarOrdenesSAE
        End Get
        Set(ByVal value As Visibility)
            _MostrarOrdenesSAE = value
            MyBase.CambioItem("MostrarOrdenesSAE")
        End Set
    End Property
    Private _NumeroColumnaDia As Integer = 2
    Public Property NumeroColumnaDia() As Integer
        Get
            Return _NumeroColumnaDia
        End Get
        Set(ByVal value As Integer)
            _NumeroColumnaDia = value
            MyBase.CambioItem("NumeroColumnaDia")
        End Set
    End Property

    Private _NumeroColumnaTipoInversion As Integer
    Public Property NumeroColumnaTipoInversion() As Integer
        Get
            Return _NumeroColumnaTipoInversion
        End Get
        Set(ByVal value As Integer)
            _NumeroColumnaTipoInversion = value
        End Set
    End Property

    Private _HabilitarUsuarioOperador As Boolean
    Public Property HabilitarUsuarioOperador() As Boolean
        Get
            Return _HabilitarUsuarioOperador
        End Get
        Set(ByVal value As Boolean)
            _HabilitarUsuarioOperador = value
            MyBase.CambioItem("HabilitarUsuarioOperador")
        End Set
    End Property

    Private _NombreDescripcion As String = "Cliente"
    Public Property NombreDescripcion() As String
        Get
            Return _NombreDescripcion
        End Get
        Set(ByVal value As String)
            _NombreDescripcion = value
            MyBase.CambioItem("NombreDescripcion")
        End Set
    End Property

    Private _MostrarDescripcionOrdenCruzada As Visibility = Visibility.Collapsed
    Public Property MostrarDescripcionOrdenCruzada() As Visibility
        Get
            Return _MostrarDescripcionOrdenCruzada
        End Get
        Set(ByVal value As Visibility)
            _MostrarDescripcionOrdenCruzada = value
            MyBase.CambioItem("MostrarDescripcionOrdenCruzada")
        End Set
    End Property
    Private _CargaMasivaCamposOrdenesOYDPLUSView As CargaMasivaCamposOrdenesView
    Public Property CargaMasivaCamposOrdenesOYDPLUSView() As CargaMasivaCamposOrdenesView
        Get
            Return _CargaMasivaCamposOrdenesOYDPLUSView
        End Get
        Set(ByVal value As CargaMasivaCamposOrdenesView)
            _CargaMasivaCamposOrdenesOYDPLUSView = value
        End Set
    End Property

    Private WithEvents _OrdenOYDPLUSSelected As OyDPLUSOrdenesBolsa.OrdenOYDPLUS
    Public Property OrdenOYDPLUSSelected() As OyDPLUSOrdenesBolsa.OrdenOYDPLUS
        Get
            Return _OrdenOYDPLUSSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesBolsa.OrdenOYDPLUS)
            _OrdenOYDPLUSSelected = value
            MyBase.CambioItem("OrdenOYDPLUSSelected")
            BuscarControlValidacion(CargaMasivaCamposOrdenesOYDPLUSView, "tabItemValoresComisiones")
            Try
                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                    HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al realizar las consultas de la orden.", _
                                Me.ToString(), "OrdenOYDPLUSSelected", Application.Current.ToString(), Program.Maquina, ex)
                IsBusy = False
            End Try
        End Set
    End Property

    Private _NemotecnicoSeleccionadoOYDPLUS As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionadoOYDPLUS As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionadoOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionadoOYDPLUS = value
            Try
                SeleccionarEspecieOYDPLUS(NemotecnicoSeleccionadoOYDPLUS)
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad de la especie seleccionada.", Me.ToString, "NemotecnicoSeleccionadoOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try
            MyBase.CambioItem("NemotecnicoSeleccionadoOYDPLUS")
        End Set
    End Property


    Private _HabilitarEjecucion As Boolean = False
    Public Property HabilitarEjecucion() As Boolean
        Get
            Return _HabilitarEjecucion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEjecucion = value
            MyBase.CambioItem("HabilitarEjecucion")
        End Set
    End Property
    Private _MostrarCamposAcciones As Visibility = Visibility.Collapsed
    Public Property MostrarCamposAcciones() As Visibility
        Get
            Return _MostrarCamposAcciones
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposAcciones = value
            MyBase.CambioItem("MostrarCamposAcciones")
        End Set
    End Property

    Private _MostrarCamposVenta As Visibility
    Public Property MostrarCamposVenta() As Visibility
        Get
            Return _MostrarCamposVenta
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposVenta = value
            MyBase.CambioItem("MostrarCamposVenta")
        End Set
    End Property
    Private _MostrarCamposCompra As Visibility
    Public Property MostrarCamposCompra() As Visibility
        Get
            Return _MostrarCamposCompra
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposCompra = value
            MyBase.CambioItem("MostrarCamposCompra")
        End Set
    End Property

    Private _MostrarNegocio As Visibility = Visibility.Visible
    Public Property MostrarNegocio() As Visibility
        Get
            Return _MostrarNegocio
        End Get
        Set(ByVal value As Visibility)
            _MostrarNegocio = value
            MyBase.CambioItem("MostrarNegocio")
        End Set
    End Property
    Private _HabilitarDuracion As Boolean = False
    Public Property HabilitarDuracion() As Boolean
        Get
            Return _HabilitarDuracion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDuracion = value
            MyBase.CambioItem("HabilitarDuracion")
        End Set
    End Property
    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            MyBase.CambioItem("HabilitarEncabezado")
        End Set
    End Property
    Private _HabilitarNegocio As Boolean = False
    Public Property HabilitarNegocio() As Boolean
        Get
            Return _HabilitarNegocio
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNegocio = value
            MyBase.CambioItem("HabilitarNegocio")
        End Set
    End Property
    Private _DiccionarioCombosOYDPlus As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlus() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlus
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlus = value
            MyBase.CambioItem("DiccionarioCombosOYDPlus")
        End Set
    End Property

    Private _DiccionarioCombosOYDPlusCompletos As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlusCompletos() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlusCompletos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlusCompletos = value
            MyBase.CambioItem("DiccionarioCombosOYDPlusCompletos")
        End Set
    End Property

    Private _TipoNegocio As String = String.Empty
    Public Property TipoNegocio() As String
        Get
            Return _TipoNegocio
        End Get
        Set(ByVal value As String)
            _TipoNegocio = value

            If logCargarContenido And Not String.IsNullOrEmpty(_TipoNegocio) And Not String.IsNullOrEmpty(_TipoOperacion) Then
                CargarContenido(TipoOpcionCargar.ARCHIVO)
            Else
                NombreArchivo = String.Empty
            End If
            If _TipoNegocio = TIPONEGOCIO_ACCIONES Then
                VerComplementacionPrecioPromedio = Visibility.Visible
                HabilitarComplementacionPrecioPromedio = True
            Else
                VerComplementacionPrecioPromedio = Visibility.Collapsed
                HabilitarComplementacionPrecioPromedio = False
            End If
            MyBase.CambioItem("TipoNegocio")
        End Set
    End Property

    Private _TipoOperacion As String = String.Empty
    Public Property TipoOperacion() As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            If logCargarContenido And Not String.IsNullOrEmpty(_TipoNegocio) And Not String.IsNullOrEmpty(_TipoOperacion) Then
                CargarContenido(TipoOpcionCargar.ARCHIVO)
            Else
                NombreArchivo = String.Empty
            End If
            MyBase.CambioItem("TipoOperacion")
        End Set
    End Property


    Private _ListaTipoNegocio As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaTipoNegocio() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaTipoNegocio
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaTipoNegocio = value
            MyBase.CambioItem("ListaTipoNegocio")
        End Set
    End Property


    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property
    Private _ComitenteSeleccionadoOYDPLUS As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionadoOYDPLUS As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionadoOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionadoOYDPLUS = value
            Try
                If logCancelarRegistro = False Then
                    SeleccionarClienteOYDPLUS(_ComitenteSeleccionadoOYDPLUS, _OrdenOYDPLUSSelected)
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad del cliente seleccionado.", Me.ToString, "ComitenteSeleccionadoOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try

            MyBase.CambioItem("ComitenteSeleccionadoOYDPLUS")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    Private _BorrarClienteADR As Boolean = False
    Public Property BorrarClienteADR() As Boolean
        Get
            Return _BorrarClienteADR
        End Get
        Set(ByVal value As Boolean)
            _BorrarClienteADR = value
            MyBase.CambioItem("BorrarClienteADR")
        End Set
    End Property

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _MensajeCantidadProcesadas As String = String.Empty
    Public Property MensajeCantidadProcesadas() As String
        Get
            Return _MensajeCantidadProcesadas
        End Get
        Set(ByVal value As String)
            _MensajeCantidadProcesadas = value
            MyBase.CambioItem("MensajeCantidadProcesadas")
        End Set
    End Property

    Private _MensajeTasas As String = String.Empty
    Public Property MensajeTasas() As String
        Get
            Return _MensajeTasas
        End Get
        Set(ByVal value As String)
            _MensajeTasas = value
            MyBase.CambioItem("MensajeTasas")
        End Set
    End Property

    Private _MostrarMensajeTasas As Visibility = Visibility.Collapsed
    Public Property MostrarMensajeTasas() As Visibility
        Get
            Return _MostrarMensajeTasas
        End Get
        Set(ByVal value As Visibility)
            _MostrarMensajeTasas = value
            MyBase.CambioItem("MostrarMensajeTasas")
        End Set
    End Property

    Private _HabilitarTasaClienteComisionValor As Boolean
    Public Property HabilitarTasaClienteComisionValor() As Boolean
        Get
            Return _HabilitarTasaClienteComisionValor
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTasaClienteComisionValor = value
            MyBase.CambioItem("HabilitarTasaClienteComisionValor")
        End Set
    End Property

    Private _MostrarCamposADR As Visibility = Visibility.Collapsed
    Public Property MostrarCamposADR() As Visibility
        Get
            Return _MostrarCamposADR
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposADR = value
            MyBase.CambioItem("MostrarCamposADR")
        End Set
    End Property


#End Region

#Region "Metodos OYDPlus"
    '****************************************************Metodo para Precio Promedio JDOL20171403
    Sub CalcularSumatoriaLiquidaciones()
        Try
            SumatoriaCantidadLiquidaciones = 0
            PrecioPromedioLiquidaciones = 0
            Dim precio As Double = 0
            Dim cantidad As Double = 0
            If Not IsNothing(ListaOrdenSAEAcciones) Then
                If ListaOrdenSAEAcciones.Where(Function(i) i.Seleccionada = True).Count > 0 Then

                    For Each li In ListaOrdenSAEAcciones.Where(Function(i) i.Seleccionada = True)
                        cantidad = CDbl(cantidad + li.Cantidad)
                        precio = CDbl(precio + (li.Cantidad * li.Precio))
                    Next
                    precio = precio / cantidad
                    PrecioPromedioLiquidaciones = precio
                    SumatoriaCantidadLiquidaciones = cantidad
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al sumar liquidaciones.",
                                                         Me.ToString(), "CalcularSumatoriaLiquidaciones", Program.TituloSistema,
                                                         Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    '************************************************************************************++
    Public Sub VerificarComision(ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.OrdenOYDPLUS)
        Try
            MensajeComision = String.Empty
            MostrarMensajeComision = Visibility.Collapsed

            If logEditarRegistro Or logNuevoRegistro Then
                If Not IsNothing(pobjOrdenSelected) Then
                    If Not IsNothing(pobjOrdenSelected.Comision) Then
                        If pobjOrdenSelected.Comision > 0 Then
                            Dim Cadena As String = pobjOrdenSelected.Comision.ToString
                            Dim posicion As Integer = InStrRev(Cadena, ".")
                            Dim posicionComa As Integer = InStrRev(Cadena, ",")

                            If posicionComa > 0 And posicion <= 0 Then
                                posicion = posicionComa
                            End If

                            Dim CadenaNueva = Cadena.Substring(posicion)

                            If Not String.IsNullOrEmpty(CadenaNueva) Then
                                If Len(CadenaNueva) = 4 And posicion > 0 Then
                                    MensajeComision = String.Format("Tener en cuenta que este porcentaje de comisión {0}es el valor más cercano para calcular el valor de la comisión, {0}no obstante la complementación en MEC es a tres decimales.", vbCrLf)
                                    MostrarMensajeComision = Visibility.Visible
                                Else
                                    MostrarMensajeComision = Visibility.Collapsed
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los días del plazo.", Me.ToString(), "VerificarComision", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Function ValidarCamposEditables() As Boolean
        Try
            Dim logRespuesta As Boolean = False
            Dim strMensajeValidacion = String.Empty



            If Not IsNothing(DiccionarioEdicionCamposOYDPLUS) Then


                For Each li In DiccionarioEdicionCamposOYDPLUS
                    If li.Key = "TipoOrden" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.TipoOrden))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Tipo Orden", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "Clasificacion" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.Clasificacion))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Clasificacion", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "Mercado" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.Mercado))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Mercado", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "TipoLimite" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.TipoLimite))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Tipo Limite", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "CondicionesNegociacion" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.CondicionesNegociacion))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Condiciones Negociacion", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "FormaPago" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.FormaPago))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Forma Pago", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "Ejecucion" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.Ejecucion))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Ejecucion", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "Duracion" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.Duracion))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Duracion", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "FechaVigencia" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.FechaVigencia))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Fecha Vigencia", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "IDComitente" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.IDComitente))) Or OrdenOYDPLUSSelected.IDComitente = "-9999999999" Then
                            strMensajeValidacion = String.Format("{0}{1} - Cliente", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "Deposito" And li.Value = True Then
                        If String.IsNullOrEmpty(OrdenOYDPLUSSelected.UBICACIONTITULO) Then
                            strMensajeValidacion = String.Format("{0}{1} - Deposito", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "Especie" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.Especie))) Or OrdenOYDPLUSSelected.Especie = "(No Seleccionado)" Then
                            strMensajeValidacion = String.Format("{0}{1} - Especie", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                        If OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                            If String.IsNullOrEmpty(LTrim(RTrim(OrdenOYDPLUSSelected.ISIN))) Then
                                strMensajeValidacion = String.Format("{0}{1} - ISIN", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                            If IsNothing(OrdenOYDPLUSSelected.FechaEmision) Then
                                strMensajeValidacion = String.Format("{0}{1} - Fecha Emision", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                            If IsNothing(OrdenOYDPLUSSelected.FechaVencimiento) Then
                                strMensajeValidacion = String.Format("{0}{1} - Fecha Vencimiento", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                            If String.IsNullOrEmpty(OrdenOYDPLUSSelected.Modalidad) Then
                                strMensajeValidacion = String.Format("{0}{1} - Modalidad", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                            If Not IsNothing(_NemotecnicoSeleccionadoOYDPLUS) Then
                                If _NemotecnicoSeleccionadoOYDPLUS.CodTipoTasaFija = TIPOTASA_VARIABLE Then
                                    If IsNothing(OrdenOYDPLUSSelected.PuntosIndicador) Or OrdenOYDPLUSSelected.PuntosIndicador = 0 Then
                                        strMensajeValidacion = String.Format("{0}{1} - Puntos indicador", strMensajeValidacion, vbCrLf)
                                        logRespuesta = True
                                    Else
                                        If OrdenOYDPLUSSelected.PuntosIndicador > 99 Or OrdenOYDPLUSSelected.PuntosIndicador < -99 Then
                                            strMensajeValidacion = String.Format("{0}{1} - Los Puntos indicador estan fuera del rango permitido (-99->99) por favor corrija.", strMensajeValidacion, vbCrLf)
                                            logRespuesta = True
                                        End If
                                    End If

                                    If String.IsNullOrEmpty(OrdenOYDPLUSSelected.Indicador) Then
                                        strMensajeValidacion = String.Format("{0}{1} - Indicador", strMensajeValidacion, vbCrLf)
                                        logRespuesta = True
                                    End If
                                Else
                                    If IsNothing(OrdenOYDPLUSSelected.TasaFacial) Or OrdenOYDPLUSSelected.TasaFacial = 0 Then
                                        strMensajeValidacion = String.Format("{0}{1} - Tasa Facial", strMensajeValidacion, vbCrLf)
                                        logRespuesta = True
                                    End If
                                End If
                            End If
                        End If
                    ElseIf li.Key = "Cantidad" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_ACCIONES Or TipoNegocio = TIPONEGOCIO_TTV Or TipoNegocio = TIPONEGOCIO_ADR Then
                            If IsNothing(OrdenOYDPLUSSelected.Cantidad) Or OrdenOYDPLUSSelected.Cantidad = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} - Cantidad", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    ElseIf li.Key = "Precio" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_ACCIONES Or TipoNegocio = TIPONEGOCIO_RENTAFIJA Or TipoNegocio = TIPONEGOCIO_TTV Or TipoNegocio = TIPONEGOCIO_ADR Then
                            If IsNothing(OrdenOYDPLUSSelected.Precio) Or OrdenOYDPLUSSelected.Precio = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} - Precio", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    ElseIf li.Key = "PrecioMaximoMinimo" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_ACCIONES Or TipoNegocio = TIPONEGOCIO_ADR Then
                            If OrdenOYDPLUSSelected.Precio > 0 And OrdenOYDPLUSSelected.PrecioMaximoMinimo > 0 Then
                                If TipoOperacion = TIPOOPERACION_COMPRA Then
                                    If OrdenOYDPLUSSelected.Precio > OrdenOYDPLUSSelected.PrecioMaximoMinimo Then
                                        strMensajeValidacion = String.Format("{0}{1} - Precio no puede ser mayor que el precio maximo", strMensajeValidacion, vbCrLf)
                                        logRespuesta = True
                                    End If
                                Else
                                    If OrdenOYDPLUSSelected.Precio < OrdenOYDPLUSSelected.PrecioMaximoMinimo Then
                                        strMensajeValidacion = String.Format("{0}{1} - Precio no puede ser menor que el precio minimo", strMensajeValidacion, vbCrLf)
                                        logRespuesta = True
                                    End If
                                End If
                            End If
                        End If
                    ElseIf li.Key = "TasaMercado" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_RENTAFIJA Or TipoNegocio = TIPONEGOCIO_TTV Or TipoNegocio = TIPONEGOCIO_REPO Or TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                            If IsNothing(OrdenOYDPLUSSelected.TasaRegistro) Or OrdenOYDPLUSSelected.TasaRegistro = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} - Tasa registro", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    ElseIf li.Key = "TasaCliente" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_RENTAFIJA Or TipoNegocio = TIPONEGOCIO_TTV Then
                            If IsNothing(OrdenOYDPLUSSelected.TasaCliente) Or OrdenOYDPLUSSelected.TasaCliente = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} - Tasa cliente", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    ElseIf li.Key = "NroAcciones" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_REPO Then
                            If IsNothing(OrdenOYDPLUSSelected.Cantidad) Or OrdenOYDPLUSSelected.Cantidad = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} -  Nro acciones", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    ElseIf li.Key = "ValorAccion" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_REPO And TipoOperacion = TIPOOPERACION_VENTA Then
                            If IsNothing(OrdenOYDPLUSSelected.ValorAccion) Or OrdenOYDPLUSSelected.ValorAccion = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} -  Valor acción", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    ElseIf li.Key = "PorcentajeCastigo" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_REPO And (TipoOperacion = TIPOOPERACION_VENTA Or TipoOperacion = TIPOOPERACION_REVENTA) Then
                            If IsNothing(OrdenOYDPLUSSelected.Castigo) Or OrdenOYDPLUSSelected.Castigo = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} -  Castigo", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    ElseIf li.Key = "PrecioConGarantia" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                            If IsNothing(OrdenOYDPLUSSelected.PrecioMaximoMinimo) Or OrdenOYDPLUSSelected.PrecioMaximoMinimo = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} -  Precio con garantia", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    ElseIf li.Key = "CanalRecepcion" And li.Value = True Then
                        If String.IsNullOrEmpty(OrdenOYDPLUSSelected.CanalRecepcion) Then
                            strMensajeValidacion = String.Format("{0}{1} - Canal Recepcion  ", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "UsuarioOperador" And li.Value = True Then
                        If String.IsNullOrEmpty(OrdenOYDPLUSSelected.UsuarioOperador) Then
                            strMensajeValidacion = String.Format("{0}{1} - Usuario operador  ", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "ExtensionTelefono" And li.Value = True Then
                        If String.IsNullOrEmpty(OrdenOYDPLUSSelected.NroExtensionToma) Then
                            strMensajeValidacion = String.Format("{0}{1} - Extensión Telefono  ", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "MedioVerificable" And li.Value = True Then
                        If String.IsNullOrEmpty(OrdenOYDPLUSSelected.MedioVerificable) Then
                            strMensajeValidacion = String.Format("{0}{1} - Medio Verificable  ", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "IDComitenteADR" And li.Value = True Then
                        If TipoNegocio = TIPONEGOCIO_ADR Then
                            If String.IsNullOrEmpty(OrdenOYDPLUSSelected.IDComitenteADR) Then
                                strMensajeValidacion = String.Format("{0}{1} - Cliente ADR", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    End If
                Next

                If HabilitarTasaClienteComisionValor Then
                    If TipoNegocio = TIPONEGOCIO_REPO Or TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                        If (IsNothing(_OrdenOYDPLUSSelected.TasaCliente) Or _OrdenOYDPLUSSelected.TasaCliente = 0) And _
                            (IsNothing(_OrdenOYDPLUSSelected.Comision) Or _OrdenOYDPLUSSelected.Comision = 0) And _
                            (IsNothing(_OrdenOYDPLUSSelected.ValorComision) Or _OrdenOYDPLUSSelected.ValorComision = 0) Then
                            strMensajeValidacion = String.Format("{0}{1} - Tasa cliente o Comisión o Valor comisión", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If

                        If TipoOperacion = TIPOOPERACION_COMPRA Then
                            If _OrdenOYDPLUSSelected.TasaCliente > _OrdenOYDPLUSSelected.TasaRegistro Then
                                strMensajeValidacion = String.Format("{0}{1} - Señor Usuario, la tasa cliente no puede ser mayor a la tasa registro.", strMensajeValidacion, vbCrLf, intCantidadMaximaDetalles)
                                logRespuesta = True
                            End If
                        ElseIf TipoOperacion = TIPOOPERACION_VENTA Then
                            If _OrdenOYDPLUSSelected.TasaCliente < _OrdenOYDPLUSSelected.TasaRegistro Then
                                strMensajeValidacion = String.Format("{0}{1} - Señor Usuario, la tasa cliente no puede ser menor a la tasa registro.", strMensajeValidacion, vbCrLf, intCantidadMaximaDetalles)
                                logRespuesta = True
                            End If
                        End If

                    End If
                End If

            End If

            If logRespuesta Then
                strMensajeValidacion = String.Format("Es necesario completar todos los campos :{0}{1}", vbCrLf, strMensajeValidacion)
                mostrarMensaje(strMensajeValidacion, "Carga Masivas Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else
                If HabilitarTasaClienteComisionValor Then
                    If DiccionarioEdicionCamposOYDPLUS("TasaCliente") Then
                        If TipoNegocio = TIPONEGOCIO_REPO Or TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                            If TipoOperacion = TIPOOPERACION_COMPRA Then
                                If _OrdenOYDPLUSSelected.TasaCliente > _OrdenOYDPLUSSelected.TasaRegistro Then
                                    strMensajeValidacion = String.Format("{0}{1} - Señor Usuario, la tasa cliente no puede ser mayor a la tasa registro.", strMensajeValidacion, vbCrLf, intCantidadMaximaDetalles)
                                    logRespuesta = True
                                End If
                            ElseIf TipoOperacion = TIPOOPERACION_VENTA Then
                                If _OrdenOYDPLUSSelected.TasaCliente < _OrdenOYDPLUSSelected.TasaRegistro Then
                                    strMensajeValidacion = String.Format("{0}{1} - Señor Usuario, la tasa cliente no puede ser menor a la tasa registro.", strMensajeValidacion, vbCrLf, intCantidadMaximaDetalles)
                                    logRespuesta = True
                                End If
                            End If
                        End If
                    End If
                End If

                If logRespuesta Then
                    mostrarMensaje(strMensajeValidacion, "Carga Masivas Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                Else
                    Return True
                End If
            End If

            Return logRespuesta
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al validar los campos", Me.ToString, "ValidarCamposEditables",
                                                         Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Return False
        End Try
    End Function


#Region "Funciones Generales"
    ''' <summary>
    ''' Metodo para obtener los valores de los combos.
    ''' Cuando el parametro esta en true es para obtener los combos por completo.
    ''' Solo se necesita para que cuando este en modo vista.
    ''' Cuando se encuentre en modo edición se cargan los combos dependiendo de las caracteristicas que tenga el receptor habilitadas
    ''' Fecha 23 de agosto del 2012
    ''' </summary>
    ''' <param name="ValoresCompletos"></param>
    ''' <remarks></remarks>
    Private Function AsignarValorTopicoCategoria(ByVal pobjValor As String, ByVal pstrTopicoDiccionario As String, ByVal pstrTopicoParametros As String, pstrValorDefecto As String) As String
        Dim objRetorno As String = String.Empty
        Try
            'Valida que la opción no se encuentre llena para no sobreescribirla
            If String.IsNullOrEmpty(pobjValor) Then
                'Valida que el topico exista
                If DiccionarioCombosOYDPlusCompletos.ContainsKey(pstrTopicoDiccionario) Then
                    'Valida sí el diccionario tiene solo un valor para asignarselo por defecto
                    If DiccionarioCombosOYDPlusCompletos(pstrTopicoDiccionario).Count = 1 Then
                        objRetorno = DiccionarioCombosOYDPlusCompletos(pstrTopicoDiccionario).FirstOrDefault.Retorno
                    Else
                        objRetorno = pstrValorDefecto
                    End If
                Else
                    objRetorno = String.Empty
                End If
            Else
                objRetorno = pobjValor
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valor por defecto del combo.",
                                Me.ToString(), "ExtraerValorTopicoCategoria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return objRetorno
    End Function

    Public Sub PrRemoverValoresDic(ByRef pobjDiccionario As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)), ByVal pstrArray As String())
        Try
            For i = 0 To pstrArray.Count - 1
                If pobjDiccionario.ContainsKey(pstrArray(i)) Then pobjDiccionario.Remove(pstrArray(i))
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.", _
                                 Me.ToString(), "PrRemoverValoresDic", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ExtraerListaPorCategoria(pobjDiccionario As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)), pstrTopico As String, pstrCategoria As String) As List(Of OYDPLUSUtilidades.CombosReceptor)
        ExtraerListaPorCategoria = New List(Of OYDPLUSUtilidades.CombosReceptor)
        Try
            If pobjDiccionario.ContainsKey(pstrTopico) Then
                Dim objRetorno = From item In pobjDiccionario(pstrTopico)
                                 Select New OYDPLUSUtilidades.CombosReceptor With {.ID = item.ID, _
                                                                            .Retorno = item.Retorno, _
                                                                            .Descripcion = item.Descripcion, _
                                                                            .Categoria = pstrCategoria}
                If objRetorno.Count > 0 Then
                    ExtraerListaPorCategoria = objRetorno.ToList()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.", _
                                 Me.ToString(), "ExtraerListaPorCategoria", Application.Current.ToString(), Program.Maquina, ex)
            Return New List(Of OYDPLUSUtilidades.CombosReceptor)
        End Try
    End Function


#End Region

    ''' <summary>
    ''' Seleccionar especie elegida en el buscador.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    Public Async Sub SeleccionarEspecieOYDPLUS(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    '_OrdenOYDPLUSSelected.Especie = pobjEspecie.Nemotecnico

                    If pobjEspecie.EsAccion Then
                        MostrarAdicionalesEspecie = Visibility.Collapsed
                        _OrdenOYDPLUSSelected.Indicador = String.Empty
                        _OrdenOYDPLUSSelected.PuntosIndicador = 0
                        _OrdenOYDPLUSSelected.ISIN = String.Empty
                        _OrdenOYDPLUSSelected.FechaEmision = Nothing
                        _OrdenOYDPLUSSelected.FechaVencimiento = Nothing
                        _OrdenOYDPLUSSelected.TasaFacial = Nothing
                        _OrdenOYDPLUSSelected.Modalidad = String.Empty
                    Else

                        _OrdenOYDPLUSSelected.ISIN = pobjEspecie.ISIN
                        _OrdenOYDPLUSSelected.FechaEmision = pobjEspecie.Emision
                        _OrdenOYDPLUSSelected.FechaVencimiento = pobjEspecie.Vencimiento
                        _OrdenOYDPLUSSelected.Modalidad = pobjEspecie.CodModalidad

                        If Not IsNothing(pobjEspecie.TasaFacial) Then
                            _OrdenOYDPLUSSelected.TasaFacial = pobjEspecie.TasaFacial
                        Else
                            _OrdenOYDPLUSSelected.TasaFacial = 0
                        End If

                        If pobjEspecie.CodTipoTasaFija = TIPOTASA_VARIABLE Then
                            MostrarAdicionalesEspecie = Visibility.Visible
                            If Not IsNothing(pobjEspecie.IdIndicador) Then
                                _OrdenOYDPLUSSelected.Indicador = pobjEspecie.IdIndicador.ToString
                            Else
                                _OrdenOYDPLUSSelected.Indicador = String.Empty
                            End If
                            If Not IsNothing(pobjEspecie.PuntosIndicador) Then
                                If pobjEspecie.PuntosIndicador > 99 Or pobjEspecie.PuntosIndicador < -99 Then
                                    mostrarMensaje("Los puntos del indicador estan en un rango no permitido (-99->99), por favor corrija los valores", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                                _OrdenOYDPLUSSelected.PuntosIndicador = pobjEspecie.PuntosIndicador
                            Else
                                _OrdenOYDPLUSSelected.PuntosIndicador = 0
                            End If

                        Else
                            MostrarAdicionalesEspecie = Visibility.Collapsed
                            _OrdenOYDPLUSSelected.Indicador = String.Empty
                            _OrdenOYDPLUSSelected.PuntosIndicador = 0
                        End If
                    End If

                    _OrdenOYDPLUSSelected.Estandarizada = pobjEspecie.Estandarizada

                    If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                        If DiccionarioEdicionCamposOYDPLUS("ValorAccion") Then
                            Await ConsultarUltimoPrecioEspecieAsync(_OrdenOYDPLUSSelected.Especie, _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.TipoNegocio)
                        End If
                    ElseIf _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_REPOC Or _
                        _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_TTV Or _
                        _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_ACCIONES Or _
                        _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_TTVC Then
                        If DiccionarioEdicionCamposOYDPLUS("Precio") Then
                            Await ConsultarUltimoPrecioEspecieAsync(_OrdenOYDPLUSSelected.Especie, _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.TipoNegocio)
                        End If
                    End If

                    BorrarEspecie = False


                End If
            Else
                '_OrdenOYDPLUSSelected.Especie = "(No Seleccionado)"
                _OrdenOYDPLUSSelected.ISIN = "(No Seleccionado)"
                _OrdenOYDPLUSSelected.FechaEmision = Nothing
                _OrdenOYDPLUSSelected.FechaVencimiento = Nothing
                _OrdenOYDPLUSSelected.TasaFacial = 0
                _OrdenOYDPLUSSelected.Modalidad = String.Empty
                _OrdenOYDPLUSSelected.Indicador = String.Empty
                _OrdenOYDPLUSSelected.PuntosIndicador = 0
                _OrdenOYDPLUSSelected.Estandarizada = False

                MostrarAdicionalesEspecie = Visibility.Collapsed

                BorrarEspecie = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos de la especie.", Me.ToString, "SeleccionarEspecie", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para obtener los valores de los combos.
    ''' Cuando el parametro esta en true es para obtener los combos por completo.
    ''' Solo se necesita para que cuando este en modo vista.
    ''' Cuando se encuentre en modo edición se cargan los combos dependiendo de las caracteristicas que tenga el receptor habilitadas
    ''' Fecha 23 de agosto del 2012
    ''' </summary>
    ''' <param name="ValoresCompletos"></param>
    ''' <remarks></remarks>
    Public Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.OrdenOYDPLUS, Optional ByVal Opcion As String = "")
        Try
            Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
            Dim objListaCategoria As New List(Of OYDPLUSUtilidades.CombosReceptor)
            Dim objListaCategoria1 As New List(Of OYDPLUSUtilidades.CombosReceptor)
            Dim objListaCategoria2 As New List(Of OYDPLUSUtilidades.CombosReceptor)
            Dim objListaCategoria3 As New List(Of OYDPLUSUtilidades.CombosReceptor)

            For Each li In DiccionarioCombosOYDPlus
                objDiccionario.Add(li.Key, li.Value)
            Next
            DiccionarioCombosOYDPlus = Nothing
            If Not IsNothing(objDiccionario) Then
                If ValoresCompletos Then 'Cuando ValoresCompletos = True
                    Call PrRemoverValoresDic(objDiccionario, {"CLASIFICACION", "TIPOOPERACION", "TIPOORDEN", "DURACION", "MERCADO"})

                    'Valores por defecto para el tipo de orden
                    '************************************************************************************
                    If objDiccionario.ContainsKey("TIPOORDENGENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOORDENGENERAL", "TIPOORDEN"))
                    If objListaCategoria.Count > 0 Then objDiccionario.Add("TIPOORDEN", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)
                    '************************************************************************************
                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    'Valores para el combo de clasificación
                    '************************************************************************************
                    If TipoNegocio = TIPONEGOCIO_ACCIONES Then
                        If objDiccionario.ContainsKey("CLASIFICACIONACCIONES") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONACCIONES", "CLASIFICACION"))
                    ElseIf TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                        If objDiccionario.ContainsKey("CLASIFICACIONRENTAFIJA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONRENTAFIJA", "CLASIFICACION"))
                    ElseIf TipoNegocio = TIPONEGOCIO_REPO Then
                        If objDiccionario.ContainsKey("CLASIFICACIONREPO") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONREPO", "CLASIFICACION"))
                    ElseIf TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                        If objDiccionario.ContainsKey("CLASIFICACIONSIMULTANEASCARGA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONSIMULTANEASCARGA", "CLASIFICACION"))
                    ElseIf TipoNegocio = TIPONEGOCIO_TTV Then
                        If objDiccionario.ContainsKey("CLASIFICACIONTTVCARGA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONTTVCARGA", "CLASIFICACION"))
                    ElseIf TipoNegocio = TIPONEGOCIO_ADR Then
                        If objDiccionario.ContainsKey("CLASIFICACIONADR") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONADR", "CLASIFICACION"))
                    End If

                    If objListaCategoria.Count > 0 Then objDiccionario.Add("CLASIFICACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)
                    If objDiccionario.ContainsKey("TIPOOPERACIONGENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOOPERACIONGENERAL", "TIPOOPERACION"))

                    '************************************************************************************
                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    'Valores para el tipo de Operación
                    '************************************************************************************
                    If objDiccionario.ContainsKey("TIPOOPERACIONGENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOOPERACIONGENERAL", "TIPOOPERACION"))

                    If objListaCategoria.Count > 0 Then objDiccionario.Add("TIPOOPERACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                    '************************************************************************************
                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    'Valores para el combo de duración
                    '************************************************************************************
                    If objDiccionario.ContainsKey("DURACIONGENERAL") Then
                        Dim obj = ExtraerListaPorCategoria(objDiccionario, "DURACIONGENERAL", "DURACION")
                        If TipoNegocio <> TIPONEGOCIO_ACCIONES And TipoNegocio.ToUpper <> TIPONEGOCIO_ADR Then
                            objListaCategoria3 = obj.Where(Function(w) w.Retorno.ToLower.Equals("c") Or w.Retorno.ToLower.Equals("f")).ToList
                        Else
                            objListaCategoria3 = obj
                        End If

                        objDiccionario.Add("DURACION", objListaCategoria3.OrderBy(Function(i) i.Descripcion).ToList)
                    End If
                    '************************************************************************************
                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    'Valores para el combo de Mercado
                    '************************************************************************************
                    If objDiccionario.ContainsKey("MERCADO_GENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "MERCADO_GENERAL", "MERCADO"))

                    If objListaCategoria.Count > 0 Then objDiccionario.Add("MERCADO", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    If Not IsNothing(objDiccionario) Then
                        If objDiccionario.ContainsKey("CANTIDADMAXDETALLEORDENES") Then
                            If objDiccionario("CANTIDADMAXDETALLEORDENES").Count > 0 Then
                                intCantidadMaximaDetalles = CInt(objDiccionario("CANTIDADMAXDETALLEORDENES").First.Retorno)
                            End If
                        End If

                        'If objDiccionario.ContainsKey("HABILITARPLANTILLASORDENES") Then
                        '    If objDiccionario("HABILITARPLANTILLASORDENES").Count > 0 Then
                        '        If objDiccionario("HABILITARPLANTILLASORDENES").First.Retorno = "1" Then
                        '            HabilitarPlantillaOrdenes = True
                        '        Else
                        '            HabilitarPlantillaOrdenes = False
                        '        End If
                        '    End If
                        'End If

                        If objDiccionario.ContainsKey("MOTORCALCULOS_RUTASERVICIO") Then
                            If objDiccionario("MOTORCALCULOS_RUTASERVICIO").Count > 0 Then
                                STR_URLMOTORCALCULOS = objDiccionario("MOTORCALCULOS_RUTASERVICIO").First.Retorno
                            End If
                        End If

                        If objDiccionario.ContainsKey("MOTORCALCULOS_HACERLOGMOTOR") Then
                            If objDiccionario("MOTORCALCULOS_HACERLOGMOTOR").Count > 0 Then
                                If objDiccionario("MOTORCALCULOS_HACERLOGMOTOR").First.Retorno = "SI" Then
                                    LOG_HACERLOGMOTORCALCULOS = True
                                End If
                            End If
                        End If

                        If objDiccionario.ContainsKey("MOTORCALCULOS_RUTALOGMOTOR") Then
                            If objDiccionario("MOTORCALCULOS_RUTALOGMOTOR").Count > 0 Then
                                STR_RUTALOGMOTORCALCULOS = objDiccionario("MOTORCALCULOS_RUTALOGMOTOR").First.Retorno
                            End If
                        End If

                        If objDiccionario.ContainsKey("VALORIVA") Then
                            If objDiccionario("VALORIVA").Count > 0 Then
                                dblValorIva = CDbl(objDiccionario("VALORIVA").First.Retorno)
                            End If
                        End If

                        If objDiccionario.ContainsKey("VALORBASE") Then
                            If objDiccionario("VALORBASE").Count > 0 Then
                                dblValorBase = CDbl(objDiccionario("VALORBASE").First.Retorno)
                            End If
                        End If

                        If objDiccionario.ContainsKey("VALORBASEREPO") Then
                            If objDiccionario("VALORBASEREPO").Count > 0 Then
                                dblValorBaseRepo = CDbl(objDiccionario("VALORBASEREPO").First.Retorno)
                            End If
                        End If

                        If objDiccionario.ContainsKey("LLEVARHORAACTUALRECEPCIONTOMA_ORDENOYDPLUS") Then
                            If objDiccionario("LLEVARHORAACTUALRECEPCIONTOMA_ORDENOYDPLUS").Count > 0 Then
                                If objDiccionario("LLEVARHORAACTUALRECEPCIONTOMA_ORDENOYDPLUS").First.Retorno = "SI" Then
                                    logLlevarHoraActualRecepcion = True
                                Else
                                    logLlevarHoraActualRecepcion = False
                                End If
                            End If
                        End If

                        If objDiccionario.ContainsKey("DEFECTORECEPTORTOMA_ORDENOYDPLUS") Then
                            If objDiccionario("DEFECTORECEPTORTOMA_ORDENOYDPLUS").Count > 0 Then
                                If objDiccionario("DEFECTORECEPTORTOMA_ORDENOYDPLUS").First.Retorno = "SI" Then
                                    logLlevarPorDefectoReceptorTomaDeReceptor = True
                                Else
                                    logLlevarPorDefectoReceptorTomaDeReceptor = False
                                End If
                            End If
                        End If

                        If objDiccionario.ContainsKey("DEFECTOUSUARIOOPERADOR_ORDENOYDPLUS") Then
                            If objDiccionario("DEFECTOUSUARIOOPERADOR_ORDENOYDPLUS").Count > 0 Then
                                If objDiccionario("DEFECTOUSUARIOOPERADOR_ORDENOYDPLUS").First.Retorno = "SI" Then
                                    logLlevarPorDefectoUsuarioOperador = True
                                Else
                                    logLlevarPorDefectoUsuarioOperador = False
                                End If
                            End If
                        End If
                    End If

                    DiccionarioCombosOYDPlus = objDiccionario

                    pobjOrdenSelected.UBICACIONTITULO = "D"
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto de los combos.", _
                                 Me.ToString(), "ObtenerValoresCombos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Function VerificarValorParaDivision(ByVal pdblValor1 As Double) As Double
        If pdblValor1 = 0 Then
            Return 1
        Else
            Return pdblValor1
        End If
    End Function

    Private Function ConvertirValorConDecimales(ByRef pdblValor As Double, ByVal pintNroDecimales As Integer)
        pdblValor = Math.Round(CDbl(pdblValor), pintNroDecimales)
        Return pdblValor
    End Function
    ''' <summary>
    ''' Valida los campos editables de la orden.
    ''' Desarrollado por: Juan David Osorio
    ''' </summary>
    Public Sub ValidarCamposEditablesOrden(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.CamposEditablesOrdens) Then
                dcProxyConsulta.CamposEditablesOrdens.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_CargaMasivaValidarHabilitarCamposQuery(Program.Usuario, Program.Maquina, Program.HashConexion),
                         AddressOf TerminoValidarCamposEditablesOrden, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los campos editables de la orden.", _
                                 Me.ToString(), "ValidarCamposEditablesOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub
    Public Sub VolverAtras()
        Try
            If TipoNegocio = TIPONEGOCIO_ACCIONES Then
                HabilitarComplementacionPrecioPromedio = True
                ComplementacionPrecioPromedio = False
                VerContinuarComplementacionPrecioPromedio = Visibility.Collapsed
                AccionCargaMasiva = "Seleccionando Archivo"
                SeleccionarTodos = False
            End If
            HabilitarTipoNegocio = True
            VerAtras = Visibility.Collapsed
            VerGrabar = Visibility.Collapsed
            MostrarMensajeComision = Visibility.Collapsed
            MensajeComision = String.Empty
            OrdenOYDPLUSSelected = Nothing
            CargarContenido(TipoOpcionCargar.ARCHIVO)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub ContinuarPrecioPromedio()
        Try
            If Not IsNothing(ListaOrdenSAEAcciones) Then
                If ListaOrdenSAEAcciones.Where(Function(i) i.Seleccionada = True).Count > 0 And (SumatoriaCantidadLiquidaciones = SumatoriaCantidadOrdenes) Then
                    strLiquidacionesProbables = "<liqprobables>"
                    For Each li In ListaOrdenSAEAcciones.Where(Function(x) x.Seleccionada = True).OrderBy(Function(x) x.ID).ToList
                        strLiquidacionesProbables = String.Format("{0}<liqprobable Id=""{1}"" Parcial=""{2}"" FechaLiq=""{3}"" Monto=""{4}"" Precio=""{5}"" PrecioPromedio=""{6}"" />",
                                                                           strLiquidacionesProbables,
                                                                           li.NroLiquidacion,
                                                                           0,
                                                                           li.FechaReferencia.ToString(),
                                                                           li.Cantidad,
                                                                           li.Precio,
                                                                           PrecioPromedioLiquidaciones)
                    Next
                    strLiquidacionesProbables = String.Format("{0}</liqprobables>", strLiquidacionesProbables)
                    CargarContenido(TipoOpcionCargar.CAMPOSORDENES)

                Else
                    mostrarMensaje("Para continuar es necesario seleccionar liquidaciones y que la sumatoria de estas sean igual a la sumatoria de las Ordenes cargadas ", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de continuar con la seleccion de liquidaciones", Me.ToString(), "ContinuarPrecioPromedio", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Sub GrabarCarga()
        Try

            If ValidarCamposEditables() Then
                IsBusy = True
                If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                    dcProxy.tblRespuestaValidaciones.Clear()
                End If



                dcProxy.Load(dcProxy.OYDPLUS_CargaMasivaValidarOrdenManualQuery(
                             _OrdenOYDPLUSSelected.Receptor, _OrdenOYDPLUSSelected.TipoOrden, _OrdenOYDPLUSSelected.TipoNegocio,
                             _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.Clasificacion, _OrdenOYDPLUSSelected.TipoLimite,
                             _OrdenOYDPLUSSelected.Duracion, _OrdenOYDPLUSSelected.FechaVigencia, _OrdenOYDPLUSSelected.HoraVigencia,
                             _OrdenOYDPLUSSelected.Dias, _OrdenOYDPLUSSelected.CondicionesNegociacion, _OrdenOYDPLUSSelected.FormaPago, _OrdenOYDPLUSSelected.TipoInversion,
                             _OrdenOYDPLUSSelected.Ejecucion, _OrdenOYDPLUSSelected.Mercado, _OrdenOYDPLUSSelected.IDComitente, _OrdenOYDPLUSSelected.UBICACIONTITULO,
                             _OrdenOYDPLUSSelected.CuentaDeposito, _OrdenOYDPLUSSelected.UsuarioOperador, _OrdenOYDPLUSSelected.CanalRecepcion,
                             _OrdenOYDPLUSSelected.MedioVerificable, _OrdenOYDPLUSSelected.FechaRecepcion, _OrdenOYDPLUSSelected.NroExtensionToma,
                             _OrdenOYDPLUSSelected.Especie, _OrdenOYDPLUSSelected.ISIN, _OrdenOYDPLUSSelected.FechaEmision, _OrdenOYDPLUSSelected.FechaVencimiento,
                             _OrdenOYDPLUSSelected.FechaCumplimiento, _OrdenOYDPLUSSelected.TasaFacial, _OrdenOYDPLUSSelected.Modalidad,
                             _OrdenOYDPLUSSelected.Indicador, _OrdenOYDPLUSSelected.PuntosIndicador, _OrdenOYDPLUSSelected.EnPesos, _OrdenOYDPLUSSelected.Cantidad,
                             _OrdenOYDPLUSSelected.Precio, _OrdenOYDPLUSSelected.PrecioMaximoMinimo, _OrdenOYDPLUSSelected.ValorCaptacionGiro,
                             _OrdenOYDPLUSSelected.TasaRegistro, _OrdenOYDPLUSSelected.TasaCliente,
                             _OrdenOYDPLUSSelected.Castigo, _OrdenOYDPLUSSelected.ValorAccion, _OrdenOYDPLUSSelected.Comision, _OrdenOYDPLUSSelected.ValorComision,
                             _OrdenOYDPLUSSelected.ProductoValores,
                             _OrdenOYDPLUSSelected.CostosAdicionales, _OrdenOYDPLUSSelected.Instrucciones, _OrdenOYDPLUSSelected.Notas, _OrdenOYDPLUSSelected.BrokenTrader,
                             _OrdenOYDPLUSSelected.Entidad, _OrdenOYDPLUSSelected.Estrategia, Program.Maquina, Program.Usuario, Program.UsuarioWindows,
                             _OrdenOYDPLUSSelected.IDComitenteADR, ComplementacionPrecioPromedio, strLiquidacionesProbables, Program.HashConexion), AddressOf TerminoValidarIngresoOrden, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de validar campos habilitados", Me.ToString(), "GrabarCarga", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Cargar los receptores activos del usuario logueado.
    ''' Cuando el parametro opción se encuentra vacio carga todos los receptores del usuario.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>

    ''' <summary>
    ''' Crea el objeto nuevo de la orden despues de realizar la carga de los receptores y combos del usuario.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Sub NuevaOrden()
        Try
            dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()

            HabilitarEncabezado = True
            HabilitarNegocio = False
            HabilitarDuracion = False
            MostrarNegocio = Visibility.Collapsed
            MostrarCamposAcciones = Visibility.Collapsed
            HabilitarEjecucion = False
            MostrarCamposRentaFija = Visibility.Collapsed
            MostrarCamposCuentaPropia = Visibility.Collapsed
            MostrarCamposCompra = Visibility.Collapsed
            MostrarCamposVenta = Visibility.Collapsed
            MostrarControles = Visibility.Collapsed
            HabilitarHoraVigencia = False
            HabilitarUsuarioOperador = True

            Dim objNewOrdenOYDPLUS As New OyDPLUSOrdenesBolsa.OrdenOYDPLUS
            objNewOrdenOYDPLUS.IDNroOrden = 0
            objNewOrdenOYDPLUS.NroOrden = 0
            objNewOrdenOYDPLUS.Receptor = String.Empty
            objNewOrdenOYDPLUS.TipoOrden = String.Empty
            objNewOrdenOYDPLUS.NombreTipoOrden = String.Empty
            objNewOrdenOYDPLUS.TipoNegocio = TipoNegocio
            objNewOrdenOYDPLUS.NombreTipoNegocio = String.Empty
            objNewOrdenOYDPLUS.TipoProducto = String.Empty
            objNewOrdenOYDPLUS.NombreTipoProducto = String.Empty
            objNewOrdenOYDPLUS.TipoOperacion = TipoOperacion
            objNewOrdenOYDPLUS.NombreTipoOperacion = String.Empty
            objNewOrdenOYDPLUS.Clase = String.Empty
            objNewOrdenOYDPLUS.FechaOrden = Now
            objNewOrdenOYDPLUS.Clasificacion = String.Empty
            objNewOrdenOYDPLUS.TipoLimite = String.Empty
            objNewOrdenOYDPLUS.Duracion = String.Empty
            objNewOrdenOYDPLUS.FechaVigencia = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, Now.Date))
            objNewOrdenOYDPLUS.HoraVigencia = "23:59:59"
            objNewOrdenOYDPLUS.Dias = 1
            objNewOrdenOYDPLUS.CondicionesNegociacion = String.Empty
            objNewOrdenOYDPLUS.FormaPago = String.Empty
            objNewOrdenOYDPLUS.TipoInversion = String.Empty
            objNewOrdenOYDPLUS.Ejecucion = String.Empty
            objNewOrdenOYDPLUS.Mercado = String.Empty
            objNewOrdenOYDPLUS.IDComitente = "-9999999999"
            objNewOrdenOYDPLUS.NombreCliente = "(No Seleccionado)"
            objNewOrdenOYDPLUS.NroDocumento = String.Empty
            objNewOrdenOYDPLUS.CategoriaCliente = String.Empty
            objNewOrdenOYDPLUS.IDOrdenante = String.Empty
            objNewOrdenOYDPLUS.NombreOrdenante = String.Empty
            objNewOrdenOYDPLUS.UBICACIONTITULO = "D"
            objNewOrdenOYDPLUS.CuentaDeposito = 0
            objNewOrdenOYDPLUS.DescripcionCta = String.Empty
            objNewOrdenOYDPLUS.UsuarioOperador = String.Empty
            objNewOrdenOYDPLUS.CanalRecepcion = String.Empty
            objNewOrdenOYDPLUS.MedioVerificable = String.Empty

            If logLlevarHoraActualRecepcion Then
                objNewOrdenOYDPLUS.FechaRecepcion = dtmFechaServidor
            Else
                objNewOrdenOYDPLUS.FechaRecepcion = dtmFechaServidor.Date
            End If

            objNewOrdenOYDPLUS.NroExtensionToma = String.Empty
            objNewOrdenOYDPLUS.Especie = "(No Seleccionado)"
            objNewOrdenOYDPLUS.ISIN = "(No Seleccionado)"
            objNewOrdenOYDPLUS.FechaEmision = Nothing
            objNewOrdenOYDPLUS.FechaVencimiento = Nothing
            objNewOrdenOYDPLUS.Estandarizada = False
            objNewOrdenOYDPLUS.FechaCumplimiento = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, Now.Date))
            objNewOrdenOYDPLUS.TasaFacial = 0
            objNewOrdenOYDPLUS.Modalidad = String.Empty
            objNewOrdenOYDPLUS.Indicador = String.Empty
            objNewOrdenOYDPLUS.PuntosIndicador = 0
            objNewOrdenOYDPLUS.EnPesos = False
            objNewOrdenOYDPLUS.Cantidad = 0
            objNewOrdenOYDPLUS.Precio = 0
            objNewOrdenOYDPLUS.PrecioMaximoMinimo = 0
            objNewOrdenOYDPLUS.ValorCaptacionGiro = 0
            objNewOrdenOYDPLUS.ValorFuturoRepo = 0
            objNewOrdenOYDPLUS.TasaRegistro = 0
            objNewOrdenOYDPLUS.TasaCliente = 0
            objNewOrdenOYDPLUS.TasaNominal = 0
            objNewOrdenOYDPLUS.Castigo = 0
            objNewOrdenOYDPLUS.ValorAccion = 0
            objNewOrdenOYDPLUS.Comision = 0
            objNewOrdenOYDPLUS.ValorComision = 0
            objNewOrdenOYDPLUS.ValorOrden = 0
            objNewOrdenOYDPLUS.DiasRepo = 0
            objNewOrdenOYDPLUS.ProductoValores = 0
            objNewOrdenOYDPLUS.CostosAdicionales = 0
            objNewOrdenOYDPLUS.Instrucciones = String.Empty
            objNewOrdenOYDPLUS.Notas = String.Empty
            objNewOrdenOYDPLUS.Custodia = 0
            objNewOrdenOYDPLUS.CustodiaSecuencia = 0
            objNewOrdenOYDPLUS.DiasCumplimiento = 0
            objNewOrdenOYDPLUS.RuedaNegocio = String.Empty
            objNewOrdenOYDPLUS.PrecioLimpio = 0
            objNewOrdenOYDPLUS.EstadoTitulo = String.Empty
            objNewOrdenOYDPLUS.Usuario = Program.Usuario
            objNewOrdenOYDPLUS.Modificable = True
            objNewOrdenOYDPLUS.FechaActualizacion = Now
            objNewOrdenOYDPLUS.EstadoLEO = String.Empty
            objNewOrdenOYDPLUS.NombreEstadoLEO = String.Empty
            objNewOrdenOYDPLUS.ComisionesOrdenesXML = String.Empty
            objNewOrdenOYDPLUS.InstruccionesOrdenesXML = String.Empty
            objNewOrdenOYDPLUS.LiqAsociadasXML = String.Empty
            objNewOrdenOYDPLUS.PagosOrdenesXML = String.Empty
            objNewOrdenOYDPLUS.ReceptoresCruzadasXML = String.Empty
            objNewOrdenOYDPLUS.ReceptoresXML = String.Empty
            objNewOrdenOYDPLUS.OrdenCruzada = False
            objNewOrdenOYDPLUS.OrdenCruzadaCliente = False
            objNewOrdenOYDPLUS.OrdenCruzadaReceptor = False
            objNewOrdenOYDPLUS.IDOrdenOriginal = Nothing

            If Not IsNothing(DiccionarioCombosOYDPlusCompletos) Then
                If DiccionarioCombosOYDPlusCompletos.ContainsKey("O_ESTADOS_ORDEN") Then
                    If DiccionarioCombosOYDPlusCompletos("O_ESTADOS_ORDEN").Count > 0 Then
                        objNewOrdenOYDPLUS.EstadoOrden = DiccionarioCombosOYDPlusCompletos("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Retorno
                        objNewOrdenOYDPLUS.NombreEstadoOrden = DiccionarioCombosOYDPlusCompletos("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Descripcion
                    Else
                        objNewOrdenOYDPLUS.EstadoOrden = "P"
                        objNewOrdenOYDPLUS.NombreEstadoOrden = "Pendiente"
                    End If
                Else
                    objNewOrdenOYDPLUS.EstadoOrden = "P"
                    objNewOrdenOYDPLUS.NombreEstadoOrden = "Pendiente"
                End If

                If DiccionarioCombosOYDPlusCompletos.ContainsKey("O_ESTADOS_ORDEN_LEO") Then
                    If DiccionarioCombosOYDPlusCompletos("O_ESTADOS_ORDEN_LEO").Count > 0 Then
                        objNewOrdenOYDPLUS.EstadoLEO = DiccionarioCombosOYDPlusCompletos("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Retorno
                        objNewOrdenOYDPLUS.NombreEstadoLEO = DiccionarioCombosOYDPlusCompletos("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Descripcion
                    Else
                        objNewOrdenOYDPLUS.EstadoLEO = "R"
                        objNewOrdenOYDPLUS.NombreEstadoLEO = "Recibida"
                    End If
                Else
                    objNewOrdenOYDPLUS.EstadoLEO = "R"
                    objNewOrdenOYDPLUS.NombreEstadoLEO = "Recibida"
                End If

            End If

            'If Not IsNothing(Application.Current.Resources(Me.ListaCombosEsp)) Then
            '    If CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_ESTADOS_ORDEN").Count > 0 Then
            '        objNewOrdenOYDPLUS.EstadoOrden = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_ESTADOS_ORDEN").Where(Function(i) i.ID = ESTADOORDEN_PENDIENTE).FirstOrDefault.ID
            '        objNewOrdenOYDPLUS.NombreEstadoOrden = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_ESTADOS_ORDEN").Where(Function(i) i.ID = ESTADOORDEN_PENDIENTE).FirstOrDefault.Descripcion
            '    End If

            '    If CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_ESTADOS_ORDEN_LEO").Count > 0 Then
            '        objNewOrdenOYDPLUS.EstadoLEO = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.ID = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.ID
            '        objNewOrdenOYDPLUS.NombreEstadoLEO = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.ID = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Descripcion
            '    End If
            'End If

            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                If DiccionarioCombosOYDPlus("BOLSA").Count > 0 Then
                    ' EOMC -- 11/20/2012
                    ' Retorno identifica la bolsa, se estaba asignando el id de la tabla de combosReceptor
                    objNewOrdenOYDPLUS.Bolsa = DiccionarioCombosOYDPlus("BOLSA").FirstOrDefault.Retorno
                    objNewOrdenOYDPLUS.NombreBolsa = DiccionarioCombosOYDPlus("BOLSA").FirstOrDefault.Descripcion
                End If
            End If


            MostrarOrdenesSAE = Visibility.Collapsed
            MostrarCamposCompra = Visibility.Collapsed
            MostrarCamposVenta = Visibility.Collapsed

            Confirmaciones = String.Empty

            'Limpia la lista de combos cuando es una nueva orden
            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                DiccionarioCombosOYDPlus.Clear()
            End If

            OrdenOYDPLUSSelected = objNewOrdenOYDPLUS

            Editando = True
            If BorrarCliente Then
                BorrarCliente = False
            End If
            BorrarCliente = True
            If BorrarEspecie Then
                BorrarEspecie = False
            End If

            BorrarEspecie = True

            BuscarControlValidacion(CargaMasivaCamposOrdenesOYDPLUSView, "tabItemValoresComisiones")
            'Se posiciona en el primer registro
            BuscarControlValidacion(CargaMasivaCamposOrdenesOYDPLUSView, "cboReceptores")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevaOrden", Program.TituloSistema, Program.Maquina, ex)
            HabilitarEncabezado = False

            HabilitarNegocio = False
            HabilitarDuracion = False
            MostrarNegocio = Visibility.Visible

            Editando = False
        End Try
    End Sub
    Public Overrides Sub NuevoRegistro()
        Try


            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            If IsNothing(OrdenOYDPLUSSelected) Then
                OrdenOYDPLUSSelected = New OyDPLUSOrdenesBolsa.OrdenOYDPLUS
            End If
            NuevaOrden()
            logNuevoRegistro = True
            logEditarRegistro = False
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logCancelarRegistro = False

            CalcularDiasOrdenOYDPLUS(MSTR_ACCION_VALIDACION_FECHA_CIERRE, _OrdenOYDPLUSSelected, -1, False, True)
            ValidarHabilitarNegocio()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Metodo para calcular los dias de vigencia de la orden.
    ''' Desarrollado por Juan David Correa
    ''' Fecha Agosto 29 del 2012
    ''' </summary>
    ''' <param name="pstrTipoCalculo"></param>
    ''' <param name="pintDias"></param>
    ''' <param name="plogGuardarOrden"></param>
    ''' <remarks></remarks>
    Public Sub CalcularDiasOrdenOYDPLUS(ByVal pstrTipoCalculo As String, ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.OrdenOYDPLUS, Optional ByVal pintDias As Integer = -1, Optional ByVal plogGuardarOrden As Boolean = False, Optional ByVal plogNuevoRegistro As Boolean = False)
        Try
            If (logNuevoRegistro Or logEditarRegistro) And pstrTipoCalculo <> MSTR_ACCION_VALIDACION_FECHA_CIERRE Then
                If IsNothing(pobjOrdenSelected) Then
                    Exit Sub
                ElseIf mdtmFechaCierreSistema >= pobjOrdenSelected.FechaOrden Then
                    mostrarMensaje("La fecha de la orden no puede ser menor a la fecha de cierre del sistema (" & mdtmFechaCierreSistema.ToLongDateString() & ")." & vbNewLine & "No se puede crear o modificar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                ElseIf pobjOrdenSelected.FechaVigencia <= Now And pobjOrdenSelected.TipoNegocio <> TIPONEGOCIO_ACCIONESOF And pobjOrdenSelected.TipoNegocio <> TIPONEGOCIO_RENTAFIJAOF Then
                    'Santiago Alexander Vergara Orrego - Mayo 27/2014 - Se añade la condición para que sólo se valide la fecha de vigencia cuando el tipo de negocio no sea de otras firmas
                    mostrarMensaje("La fecha de vigencia no puede ser menor a la fecha actual." & vbNewLine & vbNewLine & "Por favor modifique la fecha de vigencia de la orden.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                ElseIf Not IsNothing(pobjOrdenSelected.FechaEmision) And Not IsNothing(pobjOrdenSelected.FechaVencimiento) Then
                    If pobjOrdenSelected.FechaVencimiento < pobjOrdenSelected.FechaEmision Then
                        mostrarMensaje("La fecha de emisión del título no puede ser mayor a la fecha de vencimiento." & vbNewLine & vbNewLine & "Por favor modifique la fecha de emisión o la fecha de vencimiento del título.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            End If

            'IsBusy = True

            dcProxy.ValidarFechas.Clear()
            Dim FechaInicial As Date = Now.Date
            Dim FechaFinal As Date = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, Now.Date))

            If pintDias <= 0 Then
                ' Calcular los días al vencimiento de la orden a partir de la fecha de elaboración y vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                    FechaInicial = pobjOrdenSelected.FechaEmision.Value.Date
                    FechaFinal = pobjOrdenSelected.FechaVencimiento.Value.Date

                    dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, FechaInicial, FechaFinal, -1, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_CALCULAR_DIAS)
                Else
                    If plogNuevoRegistro Then
                        FechaInicial = Now
                        FechaFinal = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, Now.Date))
                    Else
                        If Not IsNothing(pobjOrdenSelected) Then
                            If Not IsNothing(pobjOrdenSelected.FechaOrden) Then
                                FechaInicial = pobjOrdenSelected.FechaOrden
                            Else
                                FechaInicial = Now
                            End If

                            If Not IsNothing(pobjOrdenSelected.FechaVigencia) Then
                                FechaFinal = pobjOrdenSelected.FechaVigencia.Value.Date
                            Else
                                FechaFinal = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, Now.Date))
                            End If
                        Else
                            FechaInicial = Now
                            FechaFinal = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, Now.Date))
                        End If
                    End If

                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, FechaInicial, FechaFinal, -1, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_VALIDACION_GUARDADO_ORDEN)
                    Else
                        If pstrTipoCalculo = MSTR_ACCION_VALIDACION_FECHA_CIERRE Then
                            dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(MSTR_ACCION_VALIDACION_FECHA_CIERRE, Nothing, Nothing, -1, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_VALIDACION_FECHA_CIERRE)
                        Else
                            dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, FechaInicial, FechaFinal, -1, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_CALCULAR_DIAS)
                        End If
                    End If
                End If
            Else
                ' Calcular la fecha de vencimiento de la orden a partir de la fecha de elaboración y los días al vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                    FechaInicial = pobjOrdenSelected.FechaEmision.Value.Date
                    FechaFinal = Nothing
                    dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, FechaInicial, FechaFinal, pintDias, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_CALCULAR_FECHA)
                Else
                    FechaInicial = pobjOrdenSelected.FechaOrden.Value.Date
                    FechaFinal = Nothing
                    dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, FechaInicial, FechaFinal, pintDias, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_CALCULAR_FECHA)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular los días al vencimiento", Me.ToString(), "CalcularDiasOrdenOYDPLUS", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para habilitar o deshabilitar los campos dependiendo del tipo de orden que sea.
    ''' Desarrollado por Juan david Correa
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub HabilitarCamposOYDPLUS(ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.OrdenOYDPLUS)
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                'Llevar el texto al campo precio dependiendo de lo seleccionado en la Orden.
                If Not IsNothing(pobjOrdenSelected.TipoNegocio) Then

                    If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONES Then

                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposAcciones = Visibility.Visible
                            HabilitarEjecucion = True
                            HabilitarDuracion = True
                        End If
                        MostrarCamposRentaFija = Visibility.Collapsed
                        NumeroColumnaDia = 2
                        pobjOrdenSelected.Clase = CLASE_ACCIONES

                        MostrarTipoNegocioAcciones = Visibility.Visible
                        MostrarTipoNegocioRentaFija = Visibility.Collapsed
                        MostrarTipoNegocioRepo = Visibility.Collapsed
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarCamposFaciales = Visibility.Collapsed
                        MostrarAdicionalesEspecie = Visibility.Collapsed
                        MostrarTipoNegocioAccionesOF = Visibility.Collapsed
                        MostrarTipoNegocioRentaFijaOF = Visibility.Collapsed
                        MostrarCamposADR = Visibility.Collapsed

                        If pobjOrdenSelected.EnPesos Then
                            TextoCantidad = "Valor en pesos"
                        Else
                            TextoCantidad = "Cantidad"
                        End If

                        TextoPrecio = "Precio"

                        'Validar si se muestra la pestaña del portafolio o el saldo
                        'C:Compra
                        'V:Venta
                        If pobjOrdenSelected.TipoOperacion = TIPOOPERACION_COMPRA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                            TextoPrecioMaximoMinimo = "Precio máximo"
                        ElseIf pobjOrdenSelected.TipoOperacion = TIPOOPERACION_VENTA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_REVENTA Then
                            TextoPrecioMaximoMinimo = "Precio mínimo"
                        End If

                    ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_REPO Then
                        MostrarCamposAcciones = Visibility.Collapsed

                        HabilitarEjecucion = False
                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposRentaFija = Visibility.Visible

                            If pobjOrdenSelected.TipoOperacion = TIPOOPERACION_COMPRA Then
                                HabilitarPrecioRepo = True
                            Else
                                HabilitarPrecioRepo = False
                            End If

                            HabilitarDuracion = True
                        Else
                            HabilitarDuracion = False
                            HabilitarPrecioRepo = False
                        End If

                        NumeroColumnaDia = 0
                        pobjOrdenSelected.Clase = CLASE_RENTAFIJA

                        MostrarTipoNegocioRepo = Visibility.Visible

                        MostrarTipoNegocioRentaFija = Visibility.Collapsed
                        MostrarTipoNegocioAcciones = Visibility.Collapsed
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarCamposFaciales = Visibility.Collapsed
                        MostrarAdicionalesEspecie = Visibility.Collapsed
                        MostrarTipoNegocioAccionesOF = Visibility.Collapsed
                        MostrarTipoNegocioRentaFijaOF = Visibility.Collapsed
                        MostrarCamposADR = Visibility.Collapsed

                        TextoCantidad = "# Acciones"
                        TextoPrecioMaximoMinimo = "Precio con garantia"
                        TextoValorCaptacionGiro = "Valor captación"

                        'Validar si se muestra la pestaña del portafolio o el saldo
                        'C:Compra
                        'V:Venta
                        If pobjOrdenSelected.TipoOperacion = TIPOOPERACION_COMPRA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                            TextoPrecio = "Valor colocacación"
                        ElseIf pobjOrdenSelected.TipoOperacion = TIPOOPERACION_VENTA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_REVENTA Then
                            TextoPrecio = "Valor venta"
                        End If
                        If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                            _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(String.Empty, "CLASIFICACIONREPO", "CLASIFICACIONREPO", String.Empty)
                        End If

                    ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_SIMULTANEA Then
                        MostrarCamposAcciones = Visibility.Collapsed
                        HabilitarEjecucion = False

                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposRentaFija = Visibility.Visible
                            HabilitarDuracion = True
                        End If
                        NumeroColumnaDia = 0
                        pobjOrdenSelected.Clase = CLASE_RENTAFIJA

                        MostrarTipoNegocioSimultanea = Visibility.Visible
                        MostrarTipoNegocioRepo = Visibility.Collapsed
                        MostrarTipoNegocioRentaFija = Visibility.Collapsed
                        MostrarTipoNegocioAcciones = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarTipoNegocioAccionesOF = Visibility.Collapsed
                        MostrarTipoNegocioRentaFijaOF = Visibility.Collapsed
                        MostrarCamposADR = Visibility.Collapsed

                        If Not IsNothing(DiccionarioEdicionCamposOYDPLUS) Then
                            If DiccionarioEdicionCamposOYDPLUS.ContainsKey("Especie") Then
                                If DiccionarioEdicionCamposOYDPLUS("Especie") Then
                                    MostrarCamposFaciales = Visibility.Visible
                                Else
                                    MostrarCamposFaciales = Visibility.Collapsed
                                End If
                            End If
                        End If

                        TextoCantidad = "Nominal"
                        TextoPrecio = "Precio"
                        TextoPrecioMaximoMinimo = "Precio con garantia"
                        TextoValorCaptacionGiro = "Valor Captacion"

                        If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                            _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(String.Empty, "CLASIFICACIONSIMULTANEASCARGA", "CLASIFICACIONSIMULTANEASCARGA", String.Empty)
                        End If
                    ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_TTV Then
                        MostrarCamposAcciones = Visibility.Collapsed
                        HabilitarEjecucion = False

                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposRentaFija = Visibility.Visible
                            HabilitarDuracion = True
                        End If
                        NumeroColumnaDia = 0
                        pobjOrdenSelected.Clase = CLASE_RENTAFIJA

                        MostrarTipoNegocioTTV = Visibility.Visible
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioRepo = Visibility.Collapsed
                        MostrarTipoNegocioRentaFija = Visibility.Collapsed
                        MostrarTipoNegocioAcciones = Visibility.Collapsed
                        MostrarCamposFaciales = Visibility.Collapsed
                        MostrarAdicionalesEspecie = Visibility.Collapsed
                        MostrarTipoNegocioAccionesOF = Visibility.Collapsed
                        MostrarTipoNegocioRentaFijaOF = Visibility.Collapsed
                        MostrarCamposADR = Visibility.Collapsed

                        TextoCantidad = "Cantidad"
                        TextoPrecio = "Precio"
                        TextoValorCaptacionGiro = "Valor giro"
                        If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                            _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(String.Empty, "CLASIFICACIONTTVCARGA", "CLASIFICACIONTTVCARGA", String.Empty)
                        End If
                    ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJA Then
                        MostrarCamposAcciones = Visibility.Collapsed
                        HabilitarEjecucion = False

                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposRentaFija = Visibility.Visible
                            HabilitarDuracion = True
                        End If
                        NumeroColumnaDia = 0
                        pobjOrdenSelected.Clase = CLASE_RENTAFIJA

                        MostrarTipoNegocioRentaFija = Visibility.Visible
                        MostrarTipoNegocioAcciones = Visibility.Collapsed
                        MostrarTipoNegocioRepo = Visibility.Collapsed
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarTipoNegocioAccionesOF = Visibility.Collapsed
                        MostrarTipoNegocioRentaFijaOF = Visibility.Collapsed
                        MostrarCamposADR = Visibility.Collapsed

                        If Not IsNothing(DiccionarioEdicionCamposOYDPLUS) Then
                            If DiccionarioEdicionCamposOYDPLUS.ContainsKey("Especie") Then
                                If DiccionarioEdicionCamposOYDPLUS("Especie") Then
                                    MostrarCamposFaciales = Visibility.Visible
                                Else
                                    MostrarCamposFaciales = Visibility.Collapsed
                                End If
                            End If
                        End If

                        TextoCantidad = "Nominal"
                        TextoPrecio = "Precio"
                        TextoValorCaptacionGiro = "Valor Bruto Operación"

                        'Santiago Alexander Vergara Orrego - Mayo 15/2014 - lógica para habilitar los campos necesario para el tipo de negocio otras firmas acciones
                    ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then
                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposAcciones = Visibility.Visible
                        End If
                        HabilitarEjecucion = False
                        HabilitarDuracion = False
                        MostrarCamposRentaFija = Visibility.Collapsed
                        NumeroColumnaDia = 2
                        pobjOrdenSelected.Clase = CLASE_ACCIONES

                        MostrarTipoNegocioAccionesOF = Visibility.Visible
                        MostrarTipoNegocioAcciones = Visibility.Collapsed
                        MostrarTipoNegocioRentaFija = Visibility.Collapsed
                        MostrarTipoNegocioRepo = Visibility.Collapsed
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarCamposFaciales = Visibility.Collapsed
                        MostrarAdicionalesEspecie = Visibility.Collapsed
                        MostrarTipoNegocioRentaFijaOF = Visibility.Collapsed
                        MostrarCamposADR = Visibility.Collapsed

                        If pobjOrdenSelected.EnPesos Then
                            TextoCantidad = "Valor en pesos"
                        Else
                            TextoCantidad = "Cantidad"
                        End If

                        TextoPrecio = "Precio"
                        TextoComision = "% comisión pactada"

                        'Santiago Alexander Vergara Orrego - Mayo 15/2014 - lógica para habilitar los campos necesario para el tipo de negocio otras firmas renta fija
                    ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJAOF Then
                        MostrarCamposAcciones = Visibility.Collapsed
                        HabilitarEjecucion = False
                        HabilitarDuracion = False
                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposRentaFija = Visibility.Visible
                        End If
                        NumeroColumnaDia = 0
                        pobjOrdenSelected.Clase = CLASE_RENTAFIJA

                        MostrarTipoNegocioRentaFijaOF = Visibility.Visible
                        MostrarTipoNegocioRentaFija = Visibility.Collapsed
                        MostrarTipoNegocioAcciones = Visibility.Collapsed
                        MostrarTipoNegocioRepo = Visibility.Collapsed
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarTipoNegocioAccionesOF = Visibility.Collapsed
                        MostrarCamposADR = Visibility.Collapsed

                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposFaciales = Visibility.Visible
                        End If

                        TextoCantidad = "Nominal"
                        TextoPrecio = "Precio"
                        TextoTasaNominal = "Tasa efectiva"
                        TextoComision = "% comisión pactada"

                    ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ADR Then

                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposAcciones = Visibility.Visible
                            HabilitarEjecucion = True
                            HabilitarDuracion = True
                        End If
                        MostrarCamposRentaFija = Visibility.Collapsed
                        NumeroColumnaDia = 2
                        pobjOrdenSelected.Clase = CLASE_ACCIONES

                        MostrarTipoNegocioAcciones = Visibility.Visible
                        MostrarTipoNegocioRentaFija = Visibility.Collapsed
                        MostrarTipoNegocioRepo = Visibility.Collapsed
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarCamposFaciales = Visibility.Collapsed
                        MostrarAdicionalesEspecie = Visibility.Collapsed
                        MostrarTipoNegocioAccionesOF = Visibility.Collapsed
                        MostrarTipoNegocioRentaFijaOF = Visibility.Collapsed
                        MostrarCamposADR = Visibility.Visible

                        If pobjOrdenSelected.EnPesos Then
                            TextoCantidad = "Valor en pesos"
                        Else
                            TextoCantidad = "Cantidad"
                        End If

                        TextoPrecio = "Precio"

                        'Validar si se muestra la pestaña del portafolio o el saldo
                        'C:Compra
                        'V:Venta
                        If pobjOrdenSelected.TipoOperacion = TIPOOPERACION_COMPRA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                            TextoPrecioMaximoMinimo = "Precio máximo"
                        ElseIf pobjOrdenSelected.TipoOperacion = TIPOOPERACION_VENTA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_REVENTA Then
                            TextoPrecioMaximoMinimo = "Precio mínimo"
                        End If

                    End If

                End If

                'Se lleva a la variable para saber si la orden de OYDPLUS es de RENTA FIJA o ACCIONES
                If Not IsNothing(pobjOrdenSelected.Clase) Then
                    If pobjOrdenSelected.Clase.ToUpper <> CLASE_ACCIONES Then
                        _mlogEsOrdenRENTAFIJAOYDPLUS = True
                    Else
                        _mlogEsOrdenRENTAFIJAOYDPLUS = False
                    End If
                End If

                'Validar si se muestra la pestaña del portafolio o el saldo
                'C:Compra
                'V:Venta
                If pobjOrdenSelected.TipoOperacion = TIPOOPERACION_COMPRA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                    If logEditarRegistro = True Or logNuevoRegistro = True Then
                        MostrarCamposCompra = Visibility.Visible
                    End If
                    MostrarCamposVenta = Visibility.Collapsed
                    If Not IsNothing(pobjOrdenSelected.TipoNegocio) Then
                        'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Cambio de label para los campos de acciones de otras firmas 
                        If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then
                            TextoPrecioMaximoMinimo = "Precio inferior"
                        End If
                    End If

                    TextoSaldoPortafolio = "Saldo"
                ElseIf pobjOrdenSelected.TipoOperacion = TIPOOPERACION_VENTA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_REVENTA Then
                    If logEditarRegistro = True Or logNuevoRegistro = True Then
                        MostrarCamposVenta = Visibility.Visible
                    End If
                    MostrarCamposCompra = Visibility.Collapsed

                    If Not IsNothing(pobjOrdenSelected.TipoNegocio) Then
                        'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Cambio de label para los campos de acciones de otras firmas 
                        If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then
                            TextoPrecioMaximoMinimo = "Precio inferior"
                        End If
                    End If
                    TextoSaldoPortafolio = "Portafolio"
                End If

                If pobjOrdenSelected.TipoProducto = TIPOPRODUCTO_CUENTAPROPIA Then
                    If String.IsNullOrEmpty(pobjOrdenSelected.TipoInversion) Then
                        pobjOrdenSelected.TipoInversion = TIPOINVERSIONXDEFECTO        ' EOMC -- Dato por defecto si hay más de un item en el combo -- 11/20/2012
                    End If
                    If pobjOrdenSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposVenta = Visibility.Visible
                        End If
                        MostrarCamposCompra = Visibility.Collapsed
                    End If
                End If



                If Not IsNothing(pobjOrdenSelected.TipoOrden) Then
                    If pobjOrdenSelected.TipoOrden.ToUpper = TIPOORDEN_DIRECTA Then
                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarOrdenesSAE = Visibility.Visible
                        End If
                    Else
                        MostrarOrdenesSAE = Visibility.Collapsed
                    End If
                Else
                    MostrarOrdenesSAE = Visibility.Collapsed
                End If

                If Not IsNothing(pobjOrdenSelected.TipoProducto) Then
                    If pobjOrdenSelected.TipoProducto.ToUpper = TIPOPRODUCTO_CUENTAPROPIA Then
                        'BuscarControlValidacion(ViewOrdenesOYDPLUS, "TabOperacionesXCumplir")
                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposCuentaPropia = Visibility.Visible
                        End If
                    Else
                        MostrarCamposCuentaPropia = Visibility.Collapsed
                    End If
                    If pobjOrdenSelected.TipoOrden = TIPOORDEN_DIRECTA Then
                        BuscarControlValidacion(CargaMasivaCamposOrdenesOYDPLUSView, "TabOrdenSAE")
                    Else
                        BuscarControlValidacion(CargaMasivaCamposOrdenesOYDPLUSView, "TabSaldoCliente")
                    End If
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al habilitar los controles dependiendo del tipo de orden.", Me.ToString, "HabilitarCamposOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
    ''' <summary>
    ''' Desarrollado por Juan David Osorio
    ''' Se crea un metodo para ubicarse en la opción o campo que se encuentra vacio.
    '''
    ''' </summary>
    ''' <param name="pViewOrdenes"></param>
    ''' <param name="pstrOpcion"></param>
    ''' <remarks></remarks>
    Public Sub BuscarControlValidacion(ByVal pViewOrdenes As CargaMasivaCamposOrdenesView, ByVal pstrOpcion As String)
        Try
            If Not IsNothing(pViewOrdenes) Then
                If TypeOf pViewOrdenes.FindName(pstrOpcion) Is TabItem Then
                    CType(pViewOrdenes.FindName(pstrOpcion), TabItem).IsSelected = True
                ElseIf TypeOf pViewOrdenes.FindName(pstrOpcion) Is TextBox Then
                    CType(pViewOrdenes.FindName(pstrOpcion), TextBox).Focus()
                ElseIf TypeOf pViewOrdenes.FindName(pstrOpcion) Is ComboBox Then
                    CType(pViewOrdenes.FindName(pstrOpcion), ComboBox).Focus()
                ElseIf TypeOf pViewOrdenes.FindName(pstrOpcion) Is Telerik.Windows.Controls.RadComboBox Then
                    CType(pViewOrdenes.FindName(pstrOpcion), Telerik.Windows.Controls.RadComboBox).Focus()
                ElseIf TypeOf pViewOrdenes.FindName(pstrOpcion) Is A2Utilidades.A2NumericBox Then
                    CType(pViewOrdenes.FindName(pstrOpcion), A2Utilidades.A2NumericBox).Focus()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control dentro de la orden.", Me.ToString, "BuscarControlValidacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Seleccionar el cliente elegido en el buscador.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    ''' <param name="pobjCliente"></param>
    ''' <remarks></remarks>

    ''' <summary>
    ''' Seleccionar el cliente elegido en el buscador.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    ''' <param name="pobjCliente"></param>
    ''' <remarks></remarks>
    Public Sub SeleccionarClienteOYDPLUS(ByVal pobjCliente As OYDUtilidades.BuscadorClientes, ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.OrdenOYDPLUS, Optional ByVal pstrOpcion As String = "")
        Try
            If Not IsNothing(pobjCliente) Then
                If logEditarRegistro Or logNuevoRegistro Then

                    If Not IsNothing(pobjOrdenSelected) Then

                        pobjOrdenSelected.IDComitente = pobjCliente.IdComitente
                        pobjOrdenSelected.NombreCliente = pobjCliente.Nombre
                        pobjOrdenSelected.NroDocumento = pobjCliente.NroDocumento
                        pobjOrdenSelected.CategoriaCliente = pobjCliente.Categoria
                        If String.IsNullOrEmpty(pobjOrdenSelected.TipoProducto) Then
                            pobjOrdenSelected.TipoProducto = pobjCliente.CodTipoProducto
                        End If
                    End If
                End If
            Else
                If Not IsNothing(pobjOrdenSelected) Then
                    pobjOrdenSelected.IDComitente = "-9999999999"
                    pobjOrdenSelected.NombreCliente = "(No Seleccionado)"
                    pobjOrdenSelected.NroDocumento = 0
                    pobjOrdenSelected.CategoriaCliente = "(No Seleccionado)"
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarClienteOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Public Sub CargarResultadosImportacion()

        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.tblRespuestaValidaciones) Then
                dcProxyConsulta.tblRespuestaValidaciones.Clear()
            End If
            ListaResultadosImportacion = Nothing
            dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_CargaMasivaConsultarConsultarResultadosQuery(Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion),
                         AddressOf TerminoConsultarResultado, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los campos editables de la orden.", _
                                 Me.ToString(), "ValidarCamposEditablesOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub
    Private Sub CargarContenido(ByVal pobjTipoOpcionCargar As TipoOpcionCargar)
        Try
            If Not IsNothing(viewCargaMasiva.gridContenido.Children) Then
                viewCargaMasiva.gridContenido.Children.Clear()
            End If

            logRecargarResultados = False
            pararTemporizador()

            Dim objContenidoCargar As Object = Nothing

            If pobjTipoOpcionCargar = TipoOpcionCargar.ARCHIVO Then
                objContenidoCargar = New CargaMasivaImportarArchivoView(Me)
                HabilitarTipoNegocio = True
                NombreArchivo = String.Empty
                VerGrabar = Visibility.Collapsed
                LimpiarVariablesConfirmadas()
            ElseIf pobjTipoOpcionCargar = TipoOpcionCargar.CAMPOSORDENES Then
                CondicionesTipoNegocio(TipoNegocio)
                objContenidoCargar = New CargaMasivaCamposOrdenesView(Me)
                HabilitarTipoNegocio = False
                LimpiarVariablesConfirmadas()
                AccionCargaMasiva = "Complementando Campos"
                VerContinuarComplementacionPrecioPromedio = Visibility.Collapsed
            ElseIf pobjTipoOpcionCargar = TipoOpcionCargar.RESULTADO Then
                VerAtras = Visibility.Collapsed
                VerGrabar = Visibility.Collapsed
                LimpiarVariablesConfirmadas()
                HabilitarTipoNegocio = False
                objContenidoCargar = New CargaMasivaImportarResultadoView(Me)
                logRecargarResultados = True
                AccionCargaMasiva = ""
            ElseIf pobjTipoOpcionCargar = TipoOpcionCargar.COMPLEMENTACION_PRECIOPROMEDIO Then
                VerAtras = Visibility.Visible
                VerGrabar = Visibility.Visible
                LimpiarVariablesConfirmadas()
                HabilitarTipoNegocio = False
                objContenidoCargar = New CargaMasivaLiquidacionesDisponiblesView(Me)
                logRecargarResultados = True
                VerContinuarComplementacionPrecioPromedio = Visibility.Visible
                VerGrabar = Visibility.Collapsed
                ConsultarOrdenesSAE = False
                ConsultarOrdenesSAE = True
                AccionCargaMasiva = ">>Seleccionando Liquidaciones"
            End If

            viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar el contenido.", Me.ToString(), "CargarContenido", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Public Sub CondicionesTipoNegocio(pobjTipoNegocio As String)
        Try
            If Not IsNothing(pobjTipoNegocio) Then
                NuevoRegistro()
                HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                ValidarHabilitarNegocio()
                ObtenerInformacionCombosCompletos()
            End If

            IsBusy = False
        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' Metodo para validar si se debe de habilitar la negociación de la pantalla de ordenes.
    ''' </summary>
    Public Sub ValidarHabilitarNegocio(Optional ByVal CambioTipoOperacion As Boolean = False)
        Try
            If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoNegocio) And _
                Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOperacion) Then
                ' Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoProducto) Then
                HabilitarNegocio = True

                If logNuevoRegistro Or logEditarRegistro Then
                    HabilitarDuracion = True
                End If

                MostrarNegocio = Visibility.Visible
                MostrarControles = Visibility.Visible

                If CambioTipoOperacion = False Then
                    ComitenteSeleccionadoOYDPLUS = Nothing
                    NemotecnicoSeleccionadoOYDPLUS = Nothing
                    If BorrarCliente Then
                        BorrarCliente = False
                    End If
                    BorrarCliente = True
                    If BorrarEspecie Then
                        BorrarEspecie = False
                    End If
                    BorrarEspecie = True
                End If

                HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
            Else
                HabilitarNegocio = False
                HabilitarDuracion = False
                MostrarNegocio = Visibility.Collapsed
                MostrarCamposCompra = Visibility.Collapsed
                MostrarCamposVenta = Visibility.Collapsed
                MostrarCamposAcciones = Visibility.Collapsed
                HabilitarEjecucion = False
                MostrarCamposRentaFija = Visibility.Collapsed
                MostrarAdicionalesEspecie = Visibility.Collapsed
                MostrarCamposCuentaPropia = Visibility.Collapsed
                MostrarControles = Visibility.Collapsed
                ComitenteSeleccionadoOYDPLUS = Nothing
                NemotecnicoSeleccionadoOYDPLUS = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la habilitación del negocio.", _
                                Me.ToString(), "ValidarHabilitarNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub ImportarArchivo()
        Try
            If String.IsNullOrEmpty(_NombreArchivo) Then
                mostrarMensaje("Debe de seleccionar un archivo para la importación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            IsBusy = True
            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            End If
            'If ComplementacionPrecioPromedio Then 
            '    CargarContenido(TipoOpcionCargar.COMPLEMENTACION_PRECIOPROMEDIO)
            'Else
            dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionOrdenesMasivasQuery(NombreArchivo, "ImpOrdenesMA", TipoNegocio, TipoOperacion, Program.Usuario, Program.UsuarioWindows, Program.Maquina, ComplementacionPrecioPromedio, Program.HashConexion),
                                      AddressOf TerminoCargarArchivoOrdenes, String.Empty)
            'End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar el archivo.", _
                                 Me.ToString(), "ImportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub
    ''' <summary>
    '''Metodo para cargar los combos especificos para la pantalla de OYDPLUS.
    ''' Se realiza la consulta en base de datos dependiendo 
    ''' </summary>

    ''' <summary>
    ''' Limpia las variables de tipo string Confirmacion, Justificacion, Aprobacion y los contadores acumulados de cada uno.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LimpiarVariablesConfirmadas()
        Try
            CantidadAprobaciones = 0
            CantidadConfirmaciones = 0
            CantidadJustificaciones = 0

            CantidadTotalAprobaciones = 0
            cantidadTotalConfirmacion = 0
            cantidadTotalJustificacion = 0

            Aprobaciones = String.Empty
            AprobacionesUsuario = String.Empty
            Confirmaciones = String.Empty
            Justificaciones = String.Empty
            JustificacionesUsuario = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los valores de las confirmaciones.", Me.ToString(), "LimpiarVariablesConfirmadas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub ValidarMensajesMostrarUsuario(ByVal pobjTipoMensaje As TIPOMENSAJEUSUARIO, Optional ByVal pobjResultaUsuario As A2Utilidades.wppMensajePregunta = Nothing)
        Try
            If Not IsNothing(_ListaResultadosImportacion) Then
                Dim logEsHtml As Boolean = False
                Dim strMensajeDetallesHtml As String = String.Empty
                Dim strMensajeRetornoHtml As String = String.Empty
                Dim ListaJustificacion As New List(Of A2Utilidades.CausasJustificacionMensajePregunta)
                Dim strMensajeEnviar As String = String.Empty
                Dim strConfirmacion As String = String.Empty
                Dim strRegla As String = String.Empty
                Dim strNombreRegla As String = String.Empty
                Dim strTipoRespuesta As String = String.Empty
                Dim logPermiteSinItemLista As Boolean = True
                Dim logPermiteSinObservacion As Boolean = True

                If pobjTipoMensaje <> TIPOMENSAJEUSUARIO.TODOS Then

                    If pobjResultaUsuario.DialogResult Then
                        If pobjTipoMensaje = TIPOMENSAJEUSUARIO.CONFIRMACION Then
                            CantidadConfirmaciones += 1
                        ElseIf pobjTipoMensaje = TIPOMENSAJEUSUARIO.JUSTIFICACION Then
                            If String.IsNullOrEmpty(Justificaciones) Then
                                Justificaciones = pobjResultaUsuario.CodConfirmacion
                                JustificacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'**'{3}'", pobjResultaUsuario.NombreRegla, pobjResultaUsuario.CodRegla, pobjResultaUsuario.MensajeRegla, String.Format("{0}++{1}", pobjResultaUsuario.Observaciones, pobjResultaUsuario.TextoConfirmacion.Replace("|", "++")))
                            Else
                                Justificaciones = String.Format("{0}|{1}", Justificaciones, pobjResultaUsuario.CodConfirmacion)
                                JustificacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'", JustificacionesUsuario, pobjResultaUsuario.NombreRegla, pobjResultaUsuario.CodRegla, pobjResultaUsuario.MensajeRegla, String.Format("{0}++{1}", pobjResultaUsuario.Observaciones, pobjResultaUsuario.TextoConfirmacion.Replace("|", "++")))
                            End If

                            CantidadJustificaciones += 1
                        ElseIf pobjTipoMensaje = TIPOMENSAJEUSUARIO.APROBACION Then
                            If String.IsNullOrEmpty(Aprobaciones) Then
                                Aprobaciones = pobjResultaUsuario.CodConfirmacion
                                AprobacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'**'{3}'", pobjResultaUsuario.NombreRegla, pobjResultaUsuario.CodRegla, pobjResultaUsuario.MensajeRegla, String.Format("{0}++{1}", pobjResultaUsuario.Observaciones, pobjResultaUsuario.TextoConfirmacion.Replace("|", "++")))
                            Else
                                Aprobaciones = String.Format("{0}|{1}", Aprobaciones, pobjResultaUsuario.CodConfirmacion)
                                AprobacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'", AprobacionesUsuario, pobjResultaUsuario.NombreRegla, pobjResultaUsuario.CodRegla, pobjResultaUsuario.MensajeRegla, String.Format("{0}++{1}", pobjResultaUsuario.Observaciones, pobjResultaUsuario.TextoConfirmacion.Replace("|", "++")))
                            End If

                            CantidadAprobaciones += 1
                        End If
                    Else
                        IsBusy = False
                        LimpiarVariablesConfirmadas()
                        Exit Sub
                    End If
                Else
                    If _ListaResultadosImportacion.Where(Function(i) i.RequiereConfirmacion = True).Count > 0 Then
                        cantidadTotalConfirmacion = 1
                    End If
                    cantidadTotalJustificacion = _ListaResultadosImportacion.Where(Function(i) i.RequiereJustificacion = True).Count
                    CantidadTotalAprobaciones = _ListaResultadosImportacion.Where(Function(i) i.RequiereAprobacion = True).Count
                    Aprobaciones = String.Empty
                    Justificaciones = String.Empty
                    Confirmaciones = String.Empty
                    JustificacionesUsuario = String.Empty
                    AprobacionesUsuario = String.Empty
                    ConfirmacionesUsuario = String.Empty
                End If

                If CantidadConfirmaciones < cantidadTotalConfirmacion Then
                    strTipoRespuesta = "PREGUNTARCONFIRMACION"
                    Dim MensajeConfirmacion As String = String.Empty

                    If _ListaResultadosImportacion.Where(Function(i) i.RequiereConfirmacion = True).Count > 0 Then
                        cantidadTotalConfirmacion = 1
                    End If

                    For Each li In _ListaResultadosImportacion.Where(Function(i) i.RequiereConfirmacion = True).ToList
                        If String.IsNullOrEmpty(Confirmaciones) Then
                            Confirmaciones = String.Format("'{0}'", li.Confirmacion)
                            ConfirmacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'", li.NombreRegla, li.Regla, li.Mensaje)
                            MensajeConfirmacion = String.Format(" -> {0}", li.Mensaje)
                        Else
                            Confirmaciones = String.Format("{0}|'{1}'", Confirmaciones, li.Confirmacion)
                            ConfirmacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'", ConfirmacionesUsuario, li.NombreRegla, li.Regla, li.Mensaje)
                            MensajeConfirmacion = String.Format("{0}{1} -> {2}", MensajeConfirmacion, vbCrLf, li.Mensaje)
                        End If

                        strMensajeRetornoHtml = String.Format("{0}{1}", strMensajeRetornoHtml, li.DetalleRegla)
                    Next

                    If Not String.IsNullOrEmpty(strMensajeRetornoHtml) Then
                        logEsHtml = True
                        strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", strMensajeRetornoHtml)
                    Else
                        logEsHtml = False
                        strMensajeDetallesHtml = String.Empty
                    End If

                    MensajeConfirmacion = Replace(MensajeConfirmacion, "-", vbCrLf)
                    MensajeConfirmacion = Replace(MensajeConfirmacion, "--", vbCrLf)

                    strMensajeEnviar = MensajeConfirmacion
                    strConfirmacion = Confirmaciones
                    strRegla = String.Empty
                    strNombreRegla = String.Empty

                ElseIf CantidadJustificaciones < cantidadTotalJustificacion Then
                    strTipoRespuesta = "PREGUNTARJUSTIFICACION"

                    For Each li In _ListaResultadosImportacion.Where(Function(i) i.RequiereJustificacion = True).ToList
                        If Not Justificaciones.Contains(li.Confirmacion) Then
                            ListaJustificacion.Clear()

                            If Not String.IsNullOrEmpty(li.CausasJustificacion) Then
                                For Each item In li.CausasJustificacion.Split("|")
                                    ListaJustificacion.Add(New A2Utilidades.CausasJustificacionMensajePregunta With {.ID = item, _
                                                                                                                     .Descripcion = item})
                                Next
                            End If

                            If Not String.IsNullOrEmpty(li.DetalleRegla) Then
                                logEsHtml = True
                                strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", li.DetalleRegla)
                            Else
                                logEsHtml = False
                                strMensajeDetallesHtml = String.Empty
                            End If

                            strMensajeEnviar = li.Mensaje
                            strConfirmacion = li.Confirmacion
                            strRegla = li.Regla
                            strNombreRegla = li.NombreRegla

                            Exit For
                        End If
                    Next
                ElseIf CantidadAprobaciones < CantidadTotalAprobaciones Then
                    strTipoRespuesta = "PREGUNTARAPROBACION"


                    For Each li In _ListaResultadosImportacion.Where(Function(i) i.RequiereAprobacion = True).ToList
                        If Not Aprobaciones.Contains(li.Confirmacion) Then
                            ListaJustificacion.Clear()

                            If Not String.IsNullOrEmpty(li.CausasJustificacion) Then
                                For Each item In li.CausasJustificacion.Split("|")
                                    ListaJustificacion.Add(New A2Utilidades.CausasJustificacionMensajePregunta With {.ID = item, _
                                                                                                                     .Descripcion = item})
                                Next
                            End If

                            If Not String.IsNullOrEmpty(li.DetalleRegla) Then
                                logEsHtml = True
                                strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", li.DetalleRegla)
                            Else
                                logEsHtml = False
                                strMensajeDetallesHtml = String.Empty
                            End If

                            strMensajeEnviar = li.Mensaje
                            strConfirmacion = li.Confirmacion
                            strRegla = li.Regla
                            strNombreRegla = li.NombreRegla

                            Exit For
                        End If
                    Next
                End If
                If Not String.IsNullOrEmpty(strMensajeEnviar) Then
                    If strTipoRespuesta <> "PREGUNTARCONFIRMACION" Then
                        If (ListaJustificacion.Count > 0) Then
                            logPermiteSinItemLista = False
                            logPermiteSinObservacion = True
                        Else
                            logPermiteSinItemLista = True
                            logPermiteSinObservacion = False
                        End If
                    Else
                        logPermiteSinItemLista = True
                        logPermiteSinObservacion = True
                    End If

                    mostrarMensajePregunta(strMensajeEnviar,
                                       Program.TituloSistema,
                                       strTipoRespuesta,
                                       AddressOf TerminoMensajePregunta,
                                       True,
                                       "¿Desea continuar?",
                                       IIf(ListaJustificacion.Count > 0, True, False),
                                       IIf(ListaJustificacion.Count > 0, True, False),
                                       logPermiteSinItemLista,
                                       logPermiteSinObservacion,
                                       strConfirmacion,
                                       strRegla,
                                       strNombreRegla,
                                       IIf(ListaJustificacion.Count > 0, ListaJustificacion, Nothing),
                                       "Reglas incumplidas en los detalles de las ordenes",
                                       logEsHtml,
                                       strMensajeDetallesHtml)
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de validar el mensaje para mostrar al usuario.", Me.ToString(), "ValidarMensajesMostrarUsuario", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Sub ConsultarUltimoPrecioEspecie(ByVal pstrEspecie As String, ByVal pstrTipoOperacion As String, ByVal pstrTipoNegocio As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not String.IsNullOrEmpty(pstrEspecie) And Not String.IsNullOrEmpty(pstrTipoOperacion) Then
                If pstrEspecie <> "(No Seleccionado)" Then
                    If Not IsNothing(dcProxy.MejorPrecioEspecieOrdens) Then
                        dcProxy.MejorPrecioEspecieOrdens.Clear()
                    End If

                    dcProxy.Load(dcProxy.OYDPLUS_ConsultarMejorPrecioEspecie_OrdenQuery(pstrTipoOperacion, pstrEspecie, Program.Usuario, pstrTipoNegocio, Program.HashConexion), AddressOf TerminoConsultarMejorPrecioEspecie, pstrUserState)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el ultimo precio de la especie.", Me.ToString(), "ConsultarUltimoPrecioEspecie", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ConfirmarValoresResultado()
        Try
            logRecargarResultados = False
            LimpiarVariablesConfirmadas()

            If Not IsNothing(ListaResultadosImportacion) Then
                If ListaResultadosImportacion.Count > 0 Then
                    Dim logRequiereConfirmacion As Boolean = False
                    Dim logRequiereJustificacion As Boolean = False
                    Dim logRequiereAprobacion As Boolean = False
                    Dim logConsultaListaJustificacion As Boolean = False
                    Dim logError As Boolean = False
                    Dim strMensajeExitoso As String = "La orden se actualizó correctamente."
                    Dim strMensajeError As String = "La orden no se pudo actualizar."
                    Dim logEsHtml As Boolean = False
                    Dim strMensajeDetallesHtml As String = String.Empty
                    Dim strMensajeRetornoHtml As String = String.Empty

                    For Each li In ListaResultadosImportacion
                        If li.RequiereConfirmacion Then
                            logError = False
                            logRequiereConfirmacion = True
                        ElseIf li.RequiereJustificacion Then
                            logError = False
                            logRequiereJustificacion = True
                        ElseIf li.RequiereAprobacion Then
                            logError = False
                            logRequiereAprobacion = True
                        End If
                    Next

                    If logRequiereConfirmacion Or logRequiereJustificacion Or logRequiereAprobacion Then
                        ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.TODOS)
                    Else
                        ComplementacionPrecioPromedio = False
                        SeleccionarTodos = False
                        HabilitarComplementacionPrecioPromedio = True
                        CargarContenido(TipoOpcionCargar.ARCHIVO)
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar los resultados.", Me.ToString(), "ConfirmarValoresResultado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = True
        End Try
    End Sub

    Public Sub EjecutarConfirmacion()
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                dcProxy.tblRespuestaValidaciones.Clear()
            End If

            dcProxy.Load(dcProxy.OYDPLUS_CargaMasivarConfirmarQuery(Program.Usuario, Program.UsuarioWindows, Program.Maquina, Confirmaciones, ConfirmacionesUsuario, Justificaciones, JustificacionesUsuario, Aprobaciones, AprobacionesUsuario, Program.HashConexion), AddressOf TerminoEjecutarConfirmacion, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la confirmacion.", Me.ToString(), "EjecutarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = True
            IsBusy = False
        End Try
    End Sub

    Public Sub ConsultarCantidadProcesadas(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.CargaMasivaCantidadProcesadas) Then
                dcProxy.CargaMasivaCantidadProcesadas.Clear()
            End If

            dcProxy.Load(dcProxy.OYDPLUS_CargaMasivaConsultarCantidadProcesadasQuery(Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarCantidadProcesadas, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la cantidad procesadas.", Me.ToString(), "ConsultarCantidadProcesadas", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = True
            IsBusy = False
        End Try
    End Sub

    Public Sub ExportarExcel()
        Try
            IsBusy = True

            Dim strParametrosEnviar As String = String.Format("[@pstrUsuario]={0}|[@pstrUsuarioWindows]={1}|[@pstrMaquina]={2}", Program.Usuario, Program.UsuarioWindows, Program.Maquina)
            Dim strNombreArchivo = String.Format("ImportacionMasivaOrdenes_{0:yyyyMMddHHmmss}", Now)

            If Not IsNothing(mdcProxyUtilidad.GenerarArchivosPlanos) Then
                mdcProxyUtilidad.GenerarArchivosPlanos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.GenerarArchivoPlanoQuery("ImpOrdenesMA", "ImpOrdenesMA", strParametrosEnviar, strNombreArchivo, "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarArchivoPlanoFondos, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la cantidad procesadas.", Me.ToString(), "ConsultarCantidadProcesadas", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = True
            IsBusy = False
        End Try
    End Sub

    Public Sub CalcularDiasPlazo(ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.OrdenOYDPLUS)
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                If Not IsNothing(pobjOrdenSelected.FechaCumplimiento) Then
                    If Not IsNothing(pobjOrdenSelected.FechaOrden) Then
                        pobjOrdenSelected.DiasRepo = DateDiff(DateInterval.Day, pobjOrdenSelected.FechaOrden.Value.Date, pobjOrdenSelected.FechaCumplimiento.Value.Date)
                        pobjOrdenSelected.DiasCumplimiento = DateDiff(DateInterval.Day, pobjOrdenSelected.FechaOrden.Value.Date, pobjOrdenSelected.FechaCumplimiento.Value.Date)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los días del plazo.", Me.ToString(), "CalcularDiasPlazo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub VerificarTasaRegistro_TasaCliente(ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.OrdenOYDPLUS)
        Try
            MensajeTasas = String.Empty
            MostrarMensajeTasas = Visibility.Collapsed

            If logEditarRegistro Or logNuevoRegistro Then
                If Not IsNothing(pobjOrdenSelected) Then
                    If Not IsNothing(pobjOrdenSelected.TasaRegistro) And Not IsNothing(pobjOrdenSelected.TasaCliente) Then
                        If pobjOrdenSelected.TasaRegistro <> 0 And pobjOrdenSelected.TasaCliente <> 0 Then
                            If pobjOrdenSelected.TipoNegocio = TIPONEGOCIO_REPO Or pobjOrdenSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                                If pobjOrdenSelected.TipoOperacion = TIPOOPERACION_COMPRA Then
                                    If pobjOrdenSelected.TasaCliente > pobjOrdenSelected.TasaRegistro Then
                                        MensajeTasas = "La tasa cliente no puede ser mayor a la tasa registro."
                                        MostrarMensajeTasas = Visibility.Visible
                                    End If
                                ElseIf pobjOrdenSelected.TipoOperacion = TIPOOPERACION_VENTA Then
                                    If pobjOrdenSelected.TasaCliente < pobjOrdenSelected.TasaRegistro Then
                                        MensajeTasas = "La tasa cliente no puede ser menor a la tasa registro."
                                        MostrarMensajeTasas = Visibility.Visible
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los días del plazo.", Me.ToString(), "CalcularDiasPlazo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Async Function ConsultarUltimoPrecioEspecieAsync(ByVal pstrEspecie As String,
                                            ByVal pstrTipoOperacion As String,
                                            ByVal pstrTipoNegocio As String,
                                            Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of OyDPLUSOrdenesBolsa.MejorPrecioEspecieOrden)

        If Not IsNothing(dcProxyConsulta.MejorPrecioEspecieOrdens) Then
            dcProxyConsulta.MejorPrecioEspecieOrdens.Clear()
        End If

        Try
            IsBusy = True
            ErrorForma = String.Empty
            If Not String.IsNullOrEmpty(pstrEspecie) And Not String.IsNullOrEmpty(pstrTipoOperacion) Then
                If pstrEspecie <> "(No Seleccionado)" Then
                    objRet = Await dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_ConsultarMejorPrecioEspecie_OrdenQuery(pstrTipoOperacion, pstrEspecie, Program.Usuario, pstrTipoNegocio, Program.HashConexion)).AsTask()
                Else
                    objRet = Nothing
                End If
            Else
                objRet = Nothing
            End If
            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar el mejor precio de la especie.", Me.ToString(), "ConsultarUltimoPrecioEspecieAsync", Program.TituloSistema, Program.Maquina, objRet.Error, Program.RutaServicioLog)
                    IsBusy = False
                Else
                    If objRet.Entities.ToList.Count > 0 Then
                        If Not IsNothing(_OrdenOYDPLUSSelected) Then
                            If Not IsNothing(objRet.Entities.FirstOrDefault.Precio) Then
                                If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                                    _OrdenOYDPLUSSelected.ValorAccion = 0
                                    _OrdenOYDPLUSSelected.ValorAccion = objRet.Entities.FirstOrDefault.Precio
                                Else
                                    _OrdenOYDPLUSSelected.Precio = 0
                                    _OrdenOYDPLUSSelected.Precio = objRet.Entities.FirstOrDefault.Precio
                                End If
                            End If
                        End If
                    End If

                End If
            End If

            Return Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        Finally
            IsBusy = False
        End Try
    End Function

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarResultado(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones))
        Try
            If lo.HasError = False Then

                ListaResultadosImportacion = lo.Entities.ToList

                IsBusy = False
                logRecargarResultados = True
                ReiniciaTimer()

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoConsultarResultado", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoConsultarResultado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoEjecutarConfirmacion(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones))
        Try
            If lo.HasError = False Then
                If lo.Entities.ToList.Where(Function(i) i.Exitoso = False).Count > 0 Then
                    Dim strMensaje As String = lo.Entities.ToList.Where(Function(i) i.Exitoso = False).First.Mensaje
                    mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                Else
                    ConsultarCantidadProcesadas(String.Empty)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoEjecutarConfirmacion", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoEjecutarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarCantidadProcesadas(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.CargaMasivaCantidadProcesadas))
        Try
            If lo.HasError = False Then
                Dim objCantidadProcesadas As OyDPLUSOrdenesBolsa.CargaMasivaCantidadProcesadas = Nothing
                objCantidadProcesadas = lo.Entities.First

                If lo.UserState = "INICIO" Then
                    If objCantidadProcesadas.ProcesoActivo Then
                        logCargarContenido = False

                        TipoNegocio = objCantidadProcesadas.TipoNegocio
                        TipoOperacion = objCantidadProcesadas.TipoOperacion

                        CargarContenido(TipoOpcionCargar.RESULTADO)

                        logCargarContenido = True
                    Else
                        If objCantidadProcesadas.CantidadConfirmacion > 0 Or
                            objCantidadProcesadas.CantidadJustificaciones > 0 Or
                            objCantidadProcesadas.CantidadAprobaciones > 0 Then

                            strTemporalTipoNegocio = objCantidadProcesadas.TipoNegocio
                            strTemporalTipoOperacion = objCantidadProcesadas.TipoOperacion

                            mostrarMensajePregunta(String.Format("Señor usuario, tiene pendiente realizar la confirmación de algunas reglas incumplidas en la importación anterior.{0}{0}Nota: Sí no confirma las ordenes y continua con el proceso de importación el sistema borrara automaticamente las ordenes que no se confirmaron.", vbCrLf), _
                                                   Program.TituloSistema, _
                                                   "INICIOPROCESOCONRESULTADOS", _
                                                   AddressOf TerminoMensajePregunta, True, "¿Desea ver la pestaña de resultados y realizar las confirmaciones requeridas?")
                        Else
                            IsBusy = False
                        End If
                    End If
                Else
                    MensajeCantidadProcesadas = String.Format("Cantidad de total de registros a procesar {1}{0}Cantidad de registros procesados {2}{0}Cantidad de registros pendientes por procesar {3}", vbCrLf, objCantidadProcesadas.CantidadTotalProcesar, objCantidadProcesadas.CantidadProcesados, objCantidadProcesadas.CantidadFaltantes)

                    If objCantidadProcesadas.CantidadFaltantes > 0 Then
                        IsbusyResultados = True
                    Else
                        IsbusyResultados = False
                    End If

                    CargarResultadosImportacion()



                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoEjecutarConfirmacion", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoEjecutarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarCombosOYDCompleto(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    If Not IsNothing(DiccionarioCombosOYDPlus) Then
                        DiccionarioCombosOYDPlus.Clear()
                    End If

                    DiccionarioCombosOYDPlusCompletos = Nothing
                    DiccionarioCombosOYDPlusCompletos = objDiccionario

                    Dim objListaTipoNegocio As New List(Of OYDPLUSUtilidades.CombosReceptor)

                    If DiccionarioCombosOYDPlusCompletos.ContainsKey("TIPONEGOCIOCARGAMASIVA") Then
                        For Each li In DiccionarioCombosOYDPlusCompletos("TIPONEGOCIOCARGAMASIVA")
                            objListaTipoNegocio.Add(li)
                        Next
                    End If

                    ListaTipoNegocio = objListaTipoNegocio

                    ConsultarCantidadProcesadas("INICIO")
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
    ''' <summary>
    ''' Metodo para obtener la información completa de los combos de la aplicación cuando sea necesario.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 10 de Septiembre del 2012
    ''' </summary>
    Public Sub ObtenerInformacionCombosCompletos()
        Try
            If Not IsNothing(DiccionarioCombosOYDPlusCompletos) Then
                Dim objDiccionarioCombosOYDPlus As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
                Dim strNombreCategoria As String = String.Empty

                For Each dic In DiccionarioCombosOYDPlusCompletos
                    strNombreCategoria = dic.Key
                    objDiccionarioCombosOYDPlus.Add(strNombreCategoria, dic.Value)
                Next

                DiccionarioCombosOYDPlus = Nothing
                DiccionarioCombosOYDPlus = objDiccionarioCombosOYDPlus

                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                    ObtenerValoresCombos(True, _OrdenOYDPLUSSelected)
                End If
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Obtener la información de los combos.", Me.ToString, "ObtenerInformacionCombosCompletos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub


    Private Sub TerminoConsultarMejorPrecioEspecie(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.MejorPrecioEspecieOrden))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar el mejor precio de la especie.", Me.ToString(), "TerminoConsultarMejorPrecioEspecie", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            Else
                If lo.Entities.Count > 0 Then
                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                        _OrdenOYDPLUSSelected.Precio = lo.Entities.FirstOrDefault.Precio
                    End If
                End If

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar el mejor precio de la especie.", Me.ToString(), "TerminoConsultarMejorPrecioEspecie", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper

                        Case "PREGUNTARCONFIRMACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.CONFIRMACION, objResultado)
                        Case "PREGUNTARAPROBACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.APROBACION, objResultado)
                        Case "PREGUNTARJUSTIFICACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.JUSTIFICACION, objResultado)
                        Case "INICIOPROCESOCONRESULTADOS"
                            If objResultado.DialogResult Then
                                logCargarContenido = False

                                TipoNegocio = strTemporalTipoNegocio
                                TipoOperacion = strTemporalTipoOperacion

                                CargarContenido(TipoOpcionCargar.RESULTADO)

                                logCargarContenido = True
                            Else
                                IsBusy = False
                                IsbusyResultados = False
                            End If
                    End Select
                End If
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje pregunta.", Me.ToString(), "TerminoMensajePregunta", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoValidarIngresoOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.tblRespuestaValidaciones))
        Try
            If lo.HasError = False Then

                ListaResultadoValidacion = lo.Entities.ToList

                If ListaResultadoValidacion.Count > 0 Then
                    Dim logExitoso As Boolean = False
                    Dim strMensajeExitoso As String = "Se realizo el proceso de carga exitosamente"
                    Dim strMensajeError As String = "El proceso de carga no se realizo"
                    Dim logError As Boolean = False

                    For Each li In ListaResultadoValidacion
                        If li.Exitoso Then
                            logExitoso = True
                            strMensajeExitoso = String.Format("{0}{1} {2}", strMensajeExitoso, vbCrLf, li.Mensaje)
                        ElseIf li.DetieneIngreso Then
                            logError = True
                            logExitoso = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                        Else
                            logError = True
                            logExitoso = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)

                        End If
                    Next

                    If logExitoso = True And logError = False Then
                        strMensajeExitoso = strMensajeExitoso.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("--", String.Format("{0}", vbCrLf))
                        mostrarMensaje(strMensajeExitoso, "Carga Masiva Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        CargarContenido(TipoOpcionCargar.RESULTADO)
                    ElseIf logError Then
                        strMensajeError = strMensajeError.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("--", String.Format("{0}", vbCrLf))

                        mostrarMensaje(strMensajeError, "Carga Masiva Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                End If
            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)

            IsBusy = False
        End Try
    End Sub
    Private Sub TerminoValidarCamposEditablesOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.CamposEditablesOrden))
        Try
            If lo.HasError = False Then
                ListaComboCamposPermitidosEdicion = lo.Entities.ToList


                Dim objDiccionarioCamposEditables As New Dictionary(Of String, Boolean)

                For Each li In ListaComboCamposPermitidosEdicion
                    If Not objDiccionarioCamposEditables.ContainsKey(li.NombreCampo) Then
                        objDiccionarioCamposEditables.Add(li.NombreCampo, li.PermiteEditar)
                    End If
                Next

                DiccionarioEdicionCamposOYDPLUS = objDiccionarioCamposEditables

                If DiccionarioEdicionCamposOYDPLUS("TasaCliente") = False Or DiccionarioEdicionCamposOYDPLUS("PorcentajeComision") = False Or DiccionarioEdicionCamposOYDPLUS("ValorComision") = False Then
                    HabilitarTasaClienteComisionValor = False
                Else
                    HabilitarTasaClienteComisionValor = True
                End If

                HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
            Else
                If logNuevoRegistro Or logEditarRegistro Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar los campos editables.", Me.ToString(), "TerminoValidarCamposEditablesOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                End If

            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar los campos editables.", Me.ToString(), "TerminoValidarCamposEditablesOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoValidarDiasHabiles(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.ValidarFecha))
        Dim objDatos As OyDPLUSOrdenesBolsa.ValidarFecha
        Dim strMsg As String = String.Empty
        Dim strAccion As String = String.Empty

        Try
            If Not lo.HasError Then
                objDatos = lo.Entities.FirstOrDefault
                strAccion = lo.UserState.ToString.ToLower

                If Not objDatos Is Nothing Then

                    If objDatos.EsDiaHabil Then

                        If lo.UserState = MSTR_ACCION_VALIDACION_GUARDADO_ORDEN Then

                        Else

                            '/ Guardar la fecha de cierre del sistema
                            mdtmFechaCierreSistema = objDatos.FechaCierre

                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                                If logEditarRegistro Or logNuevoRegistro Then
                                    If objDatos.FechaInicial < objDatos.FechaCierre Then
                                        mostrarMensaje("La fecha de la orden no puede ser menor a la fecha de cierre del sistema (" & objDatos.FechaCierre.ToLongDateString() & ")." & vbNewLine & "No se puede crear o modificar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                                        If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                            _OrdenOYDPLUSSelected.Dias = 0
                                        End If

                                    End If
                                End If
                            ElseIf objDatos.TipoCalculo.ToLower() = MSTR_ACCION_VALIDACION_FECHA_CIERRE Then
                                If objDatos.FechaInicial < objDatos.FechaCierre Then
                                    mostrarMensaje("La fecha de la orden no puede ser menor a la fecha de cierre del sistema (" & objDatos.FechaCierre.ToLongDateString() & ")." & vbNewLine & "No se puede crear o modificar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    IsBusy = False
                                End If
                            End If

                            If strAccion = MSTR_ACCION_CALCULAR_DIAS Then
                                If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then

                                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                        _OrdenOYDPLUSSelected.Dias = objDatos.NroDias
                                    End If


                                Else

                                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                        _OrdenOYDPLUSSelected.Dias = objDatos.NroDias
                                    End If

                                End If
                            Else
                                If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_TITULO Then

                                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                        _OrdenOYDPLUSSelected.FechaVencimiento = objDatos.FechaFinal
                                    End If

                                Else
                                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                        _OrdenOYDPLUSSelected.FechaVencimiento = objDatos.FechaFinal
                                    End If
                                End If
                            End If
                        End If
                    End If
                Else 'If objDatos.EsDiaHabil Then
                    If logEditarRegistro Or logNuevoRegistro Then

                        If objDatos.TipoCalculo.ToLower() = MSTR_ACCION_VALIDACION_FECHA_CIERRE Then
                            If Now < objDatos.FechaCierre Then
                                mostrarMensaje("La fecha de la orden no puede ser menor a la fecha de cierre del sistema (" & objDatos.FechaCierre.ToLongDateString() & ")." & vbNewLine & "No se puede crear o modificar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                mostrarMensaje(strMsg, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else
                                ' CargarReceptoresUsuarioOYDPLUS("NUEVO", "NUEVO")
                            End If
                        Else
                            'Valida sí la duración es cancelación para que no se muestre el mensaje de error.
                            'sino que se asigne la fecha posterior sugerida por el sistema.
                            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                If _OrdenOYDPLUSSelected.Duracion = DURACION_CANCELACION And _
                                objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN And _
                                lo.UserState <> MSTR_ACCION_VALIDACION_GUARDADO_ORDEN Then
                                    _OrdenOYDPLUSSelected.FechaVigencia = objDatos.FechaHabilMayor
                                Else
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

                                    mostrarMensaje(strMsg, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                            End If
                        End If
                    Else
                        If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                            If IsNothing(objDatos.NroDias) Then
                                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                    If IsNothing(_OrdenOYDPLUSSelected.FechaVigencia) Then

                                        _OrdenOYDPLUSSelected.Dias = 0

                                    Else
                                        ' Se suma uno porque se deben incluir la fecha de la orden y la fecha de vigencia

                                        _OrdenOYDPLUSSelected.Dias = DateDiff(DateInterval.Day, CType(_OrdenOYDPLUSSelected.FechaOrden, Date), CType(_OrdenOYDPLUSSelected.FechaVigencia, Date)) + 1

                                    End If
                                End If
                            Else

                                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                    _OrdenOYDPLUSSelected.Dias = 0
                                End If


                            End If
                        Else

                            If IsNothing(objDatos.NroDias) Then
                                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                    _OrdenOYDPLUSSelected.Dias = 0
                                End If
                            Else
                                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                    _OrdenOYDPLUSSelected.Dias = objDatos.NroDias
                                End If
                            End If
                        End If


                    End If

                    If lo.UserState = MSTR_ACCION_VALIDACION_GUARDADO_ORDEN Then
                        IsBusy = False
                    End If

                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación de los días al vencimiento", Me.ToString(), "TerminoValidarDiasHabiles", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la validación de los días al vencimiento", Me.ToString(), "TerminoValidarDiasHabiles", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub TerminoCargarArchivoOrdenes(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim ViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim logContinuar As Boolean = False
                HabilitarComplementacionPrecioPromedio = False
                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    Dim objTipo As String = String.Empty

                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                        objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")
                    End If

                    For Each li In objListaRespuesta.OrderBy(Function(o) o.Tipo)
                        If objTipo <> li.Tipo And li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" And li.Tipo <> "ESPECIE" And li.Tipo <> "CANTIDAD" Then
                            objTipo = li.Tipo
                            objListaMensajes.Add(String.Format("Hoja {0}", li.Tipo))
                        ElseIf li.Tipo = "REGISTROSIMPORTADOS" Then
                            If li.Columna > 0 Then
                                logContinuar = True
                            End If
                        End If

                        If li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" And li.Tipo <> "ESPECIE" And li.Tipo <> "CANTIDAD" Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Campo {1} - Validación: {2}", li.Fila, li.Campo, li.Mensaje))
                        Else
                            If li.Tipo <> "ESPECIE" And li.Tipo <> "CANTIDAD" Then
                                objListaMensajes.Add(li.Mensaje)
                            End If
                        End If
                    Next

                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo órdenes masivas"
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo órdenes masivas"
                End If

                Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                ViewImportarArchivo.ShowDialog()

                If logContinuar Then
                    VerAtras = Visibility.Visible
                    ValidarCamposEditablesOrden()
                    If ComplementacionPrecioPromedio Then
                        EspecieComplementacionPrecioPromedio = objListaRespuesta.Where(Function(i) i.Tipo = "ESPECIE").FirstOrDefault.Mensaje.ToUpper
                        SumatoriaCantidadOrdenes = CDbl(objListaRespuesta.Where(Function(i) i.Tipo = "CANTIDAD").FirstOrDefault.Mensaje)
                        CargarContenido(TipoOpcionCargar.COMPLEMENTACION_PRECIOPROMEDIO)

                    Else
                        CargarContenido(TipoOpcionCargar.CAMPOSORDENES)

                    End If


                    HabilitarTipoNegocio = False
                    VerGrabar = Visibility.Visible
                Else
                    IsBusy = False
                    VerGrabar = Visibility.Collapsed
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de ordenes.", Me.ToString(), "TerminoCargarArchivoOrdenes", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
                HabilitarComplementacionPrecioPromedio = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de ordenes.", Me.ToString(), "TerminoCargarArchivoOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoGenerarArchivoPlanoFondos(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.Exitoso Then
                        Program.VisorArchivosWeb_DescargarURL(lo.Entities.First.RutaArchivoPlano)

                        IsBusy = False
                    Else
                        mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                Else
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "TerminoGenerarArchivoPlanoFondos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "TerminoGenerarArchivoPlanoFondos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Timer Refrescar pantalla"

    Private _myDispatcherTimerOrdenes As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    Public Sub ReiniciaTimer()
        Try
            If Program.Recarga_Automatica_Activa Then
                If _myDispatcherTimerOrdenes Is Nothing Then
                    _myDispatcherTimerOrdenes = New System.Windows.Threading.DispatcherTimer
                    _myDispatcherTimerOrdenes.Interval = New TimeSpan(0, 0, 8)
                    AddHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
                End If
                _myDispatcherTimerOrdenes.Start()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador()
        Try
            If Not IsNothing(_myDispatcherTimerOrdenes) Then
                _myDispatcherTimerOrdenes.Stop()
                RemoveHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerOrdenes = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Each_Tick(sender As Object, e As EventArgs)
        'Recarga la pantalla cuando se este en un tiempo de espera sin notificaciones y sin refrescar la pantalla.
        If logRecargarResultados Then
            ConsultarCantidadProcesadas(String.Empty)
        End If
    End Sub

#End Region

    Private Sub _OrdenOYDPLUSSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _OrdenOYDPLUSSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "tiponegocio"
                    If Not IsNothing(_OrdenOYDPLUSSelected.TipoNegocio) Then


                        'If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_ACCIONES Then
                        '    _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(_OrdenOYDPLUSSelected.Clasificacion, "CLASIFICACIONACCIONES", "CLASIFICACIONACCIONES", CLASIFICACIONXDEFECTO)
                        'ElseIf _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                        '    _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(_OrdenOYDPLUSSelected.Clasificacion, "CLASIFICACIONRENTAFIJA", "CLASIFICACIONRENTAFIJA", CLASIFICACIONXDEFECTO)
                        'ElseIf _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_REPO Or _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_REPOC Then
                        '    _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(String.Empty, "CLASIFICACIONREPO", "CLASIFICACIONREPO", String.Empty)
                        'ElseIf _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                        '    _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(String.Empty, "CLASIFICACIONSIMULTANEAS", "CLASIFICACIONSIMULTANEAS", String.Empty)
                        'ElseIf _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_TTV Or _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_TTVC Then
                        '    _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(String.Empty, "CLASIFICACIONTTV", "CLASIFICACIONTTV", String.Empty)
                        'ElseIf _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_ADR Then
                        '    _OrdenOYDPLUSSelected.Clasificacion = AsignarValorTopicoCategoria(String.Empty, "CLASIFICACIONADR", "CLASIFICACIONADR", String.Empty)
                        'End If
                    End If

                Case "tipoorden"
                    If Not IsNothing(_OrdenOYDPLUSSelected.TipoOrden) Then
                        If logEditarRegistro Or logNuevoRegistro Then
                            ValidarHabilitarNegocio()
                            BuscarControlValidacion(CargaMasivaCamposOrdenesOYDPLUSView, "cboTipoProducto")
                        End If
                    End If
                Case "cantidad"
                    If logEditarRegistro Or logNuevoRegistro Then
                    End If
                Case "precio"
                    If logEditarRegistro Or logNuevoRegistro Then
                    End If
                    ' también se dispara el calcular valor cuando cambie este dato -- EOMC -- 11/20/2012
                Case "preciomaximominimo"
                    If logEditarRegistro Or logNuevoRegistro Then
                    End If
                    ' también se dispara el calcular valor cuando cambie este dato -- EOMC -- 11/20/2012
                Case "comision"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If logEditarRegistro Or logNuevoRegistro Then
                            VerificarComision(_OrdenOYDPLUSSelected)
                        End If
                        If logCalculoValores Then
                            If DiccionarioEdicionCamposOYDPLUS("ValorComision") Then
                                logCalculoValores = False
                                _OrdenOYDPLUSSelected.ValorComision = 0
                                logCalculoValores = True
                            End If
                            If DiccionarioEdicionCamposOYDPLUS("TasaCliente") Then
                                logCalculoValores = False
                                _OrdenOYDPLUSSelected.TasaCliente = 0
                                logCalculoValores = True
                            End If
                        End If
                    End If
                Case "valorcomision"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If logCalculoValores Then
                            If DiccionarioEdicionCamposOYDPLUS("PorcentajeComision") Then
                                logCalculoValores = False
                                _OrdenOYDPLUSSelected.Comision = 0
                                logCalculoValores = True
                            End If
                            If DiccionarioEdicionCamposOYDPLUS("TasaCliente") Then
                                logCalculoValores = False
                                _OrdenOYDPLUSSelected.TasaCliente = 0
                                logCalculoValores = True
                            End If
                        End If
                    End If
                Case "valorcaptaciongiro"
                    If logEditarRegistro Or logNuevoRegistro Then
                    End If
                Case "castigo"
                    If logEditarRegistro Or logNuevoRegistro Then
                    End If
                Case "tasaregistro"
                    If logEditarRegistro Or logNuevoRegistro Then
                        VerificarTasaRegistro_TasaCliente(_OrdenOYDPLUSSelected)
                    End If
                Case "tasacliente"
                    If logEditarRegistro Or logNuevoRegistro Then
                        VerificarTasaRegistro_TasaCliente(_OrdenOYDPLUSSelected)
                        If logCalculoValores Then
                            If DiccionarioEdicionCamposOYDPLUS("PorcentajeComision") Then
                                logCalculoValores = False
                                _OrdenOYDPLUSSelected.Comision = 0
                                logCalculoValores = True
                            End If
                            If DiccionarioEdicionCamposOYDPLUS("ValorComision") Then
                                logCalculoValores = False
                                _OrdenOYDPLUSSelected.ValorComision = 0
                                logCalculoValores = True
                            End If
                        End If
                    End If
                Case "tasafacial"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If (_OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA) Then
                            _OrdenOYDPLUSSelected.TasaNominal = _OrdenOYDPLUSSelected.TasaFacial
                        End If
                    End If
                Case "valoraccion"
                    If logEditarRegistro Or logNuevoRegistro Then
                    End If
                Case "fechavigencia"
                    If logNuevoRegistro Or logEditarRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.FechaVigencia) Then
                            _OrdenOYDPLUSSelected.HoraVigencia = String.Format("{0:00}:{1:00}:{2:00}",
                                                                               _OrdenOYDPLUSSelected.FechaVigencia.Value.Hour, _
                                                                               _OrdenOYDPLUSSelected.FechaVigencia.Value.Minute, _
                                                                               _OrdenOYDPLUSSelected.FechaVigencia.Value.Second)
                            Me.CalcularDiasOrdenOYDPLUS(MSTR_CALCULAR_DIAS_ORDEN, _OrdenOYDPLUSSelected, -1)
                        End If
                    End If
                Case "dias"
                    If logNuevoRegistro Or logEditarRegistro Then
                    End If
                Case "tipolimite"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoLimite) And _
                            Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Especie) And _
                            Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOperacion) Then
                            If _OrdenOYDPLUSSelected.TipoLimite = TIPOMERCADO_PRECIO_ESPECIE Or _OrdenOYDPLUSSelected.TipoLimite = TIPOMERCADO_PORLOMEJOR_PRECIO_ESPECIE Then
                                ConsultarUltimoPrecioEspecie(_OrdenOYDPLUSSelected.Especie, _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.TipoNegocio)
                            End If
                        End If
                    End If
                Case "fechacumplimiento"
                    If logEditarRegistro Or logNuevoRegistro Then
                        CalcularDiasPlazo(_OrdenOYDPLUSSelected)
                    End If


            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenOYDPLUSSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

End Class