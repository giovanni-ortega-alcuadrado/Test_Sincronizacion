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
Imports A2MCCoreWPF
Imports OpenRiaServices.DomainServices.Client

Public Class OrdenesOYDPLUSOFViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesOFDomainContext()
                dcProxy1 = New OYDPLUSOrdenesOFDomainContext()
                dcProxyConsulta = New OYDPLUSOrdenesOFDomainContext()
                dcProxyPlantilla = New OYDPLUSOrdenesOFDomainContext()

                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
                mdcProxyUtilidad03 = New OYDPLUSUtilidadesDomainContext()

            Else
                dcProxy = New OYDPLUSOrdenesOFDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OYDPLUSOrdenesOFDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyConsulta = New OYDPLUSOrdenesOFDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyPlantilla = New OYDPLUSOrdenesOFDomainContext(New System.Uri(Program.RutaServicioNegocio))

                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad03 = New OYDPLUSUtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesOFDomainContext.IOYDPLUSOrdenesOFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxy1.DomainClient, WebDomainClient(Of OYDPLUSOrdenesOFDomainContext.IOYDPLUSOrdenesOFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad01.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad02.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

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

                'Lista que se utiliza para filtrar las ordenes que se encuentren aprobadas y pendientes por aprobar
                ListaDatos = New List(Of String)
                ListaDatos.Add(VISTA_APROBADAS)
                ListaDatos.Add(VISTA_PENDIENTESAPROBAR)

                'Se calcula la fecha de cierre.
                mdtmFechaCierreSistema = DateAdd(DateInterval.Year, -5, Now()).Date

                'Consulta el tipo de negocio completos.
                CargarTipoNegocioReceptor("INICIO", String.Empty, _Modulo)

                'Consultar los precios del mercado para activar el ticker.
                CargarMensajeDinamicoOYDPLUS("PRECIOSMERCADO", String.Empty, String.Empty, String.Empty)

                viewFormaOrdenes = New FormaOrdenesOFView(Me)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "OrdenesOYDPLUSViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
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

#Region "Propiedades Ordenes OYDPLUS"

#Region "Propiedades Vistas Datos"

    Private _ListaDatos As List(Of String)
    Public Property ListaDatos() As List(Of String)
        Get
            Return _ListaDatos
        End Get
        Set(ByVal value As List(Of String))
            _ListaDatos = value
            MyBase.CambioItem("ListaDatos")
        End Set
    End Property

    Private _VistaSeleccionada As String
    Public Property VistaSeleccionada() As String
        Get
            Return _VistaSeleccionada
        End Get
        Set(ByVal value As String)
            _VistaSeleccionada = value
            NombreDescripcion = "Cliente"

            If Not String.IsNullOrEmpty(VistaSeleccionada) And logRefrescarconsultaCambioTab Then
                Dim strEstadoFiltro As String = String.Empty

                If VistaSeleccionada = VISTA_PENDIENTESAPROBAR Then
                    strEstadoFiltro = "D"
                Else
                    strEstadoFiltro = "P"
                End If

                FiltrarRegistrosOYDPLUS(strEstadoFiltro, String.Empty, "CAMBIO TAB")
            End If
            MyBase.CambioItem("VistaSeleccionada")
        End Set
    End Property

    Private _DiccionarioBotonesOrdenes As Dictionary(Of String, BotonMenu)
    Public Property DiccionarioBotonesOrdenes() As Dictionary(Of String, BotonMenu)
        Get
            Return _DiccionarioBotonesOrdenes
        End Get
        Set(ByVal value As Dictionary(Of String, BotonMenu))
            _DiccionarioBotonesOrdenes = value
            MyBase.CambioItem("DiccionarioBotonesOrdenes")
        End Set
    End Property


#End Region

#Region "Variables"

    Private dcProxy As OyDPLUSordenesofDomainContext
    Private dcProxy1 As OyDPLUSordenesofDomainContext
    Private dcProxyConsulta As OyDPLUSordenesofDomainContext
    Private dcProxyPlantilla As OyDPLUSordenesofDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Private mdcProxyUtilidad03 As OyDPLUSutilidadesDomainContext

    Dim logConsultarCliente As Boolean = True

    Public logNuevoRegistro As Boolean = False
    Public logEditarRegistro As Boolean = False
    Dim logCancelarRegistro As Boolean = False
    Dim logDuplicarRegistro As Boolean = False
    Dim logPlantillaRegistro As Boolean = False
    Public logModificarDatosTipoNegocio As Boolean = True

    Dim logRealizarConsultaPropiedades As Boolean = False
    Dim strMensajeValidacion As String = String.Empty
    Public mdtmFechaCierreSistema As Date
    Dim cantidadTotalConfirmacion As Integer = 0
    Dim cantidadTotalJustificacion As Integer = 0
    Dim CantidadTotalAprobaciones As Integer = 0
    Dim logCalcularValores As Boolean = False
    Dim logCambiarConsultaSAE As Boolean = True
    Dim logCambiarConsultaPortafolio As Boolean = True
    Dim logCambiarConsultaOperaciones As Boolean = True
    Dim logCambiarConsultaSaldo As Boolean = True
    Dim intIDOrdenTimer As Integer = 0
    Dim logRefrescarconsultaCambioTab As Boolean = True
    Dim logCambiarDetallesOrden As Boolean = True
    Dim logCambiarSelected As Boolean = True
    Dim logMostrarValorPorDefecto As Boolean = True

    Private _mlogEsOrdenOYDPLUS As Boolean = False
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de OYD PLUS
    ''' </summary>
    Public ReadOnly Property EsOrdenOYDPLUS As Boolean
        Get
            Return (_mlogEsOrdenOYDPLUS)
        End Get
    End Property

    Dim strNombrePlantilla As String = String.Empty
    Dim viewNombrePlantilla As NombrePlantillaOYDPLUSView

    Dim intCantidadMaximaDetalles As Integer = 30
    Dim dblValorIva As Double = 0
    Dim dblValorBase As Double = 0
    Dim dblValorBaseRepo As Double = 0

    Dim viewFormaOrdenes As FormaOrdenesOFView = Nothing
    Dim logCargoForma As Boolean = False
    Public dtmFechaServidor As DateTime
    Dim logHabilitarCondicionesTipoProductoCuentaPropia As Boolean = False
    Dim strTiposProductoPosicionPropia As String = ""

#End Region

#Region "Constantes"

    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17
    Private MINT_LONG_MAX_NEMOTECNICO As Byte = 15
    Public MSTR_CALCULAR_DIAS_ORDEN As String = "vencimiento_orden"
    Public MSTR_CALCULAR_DIAS_PLAZO As String = "vencimiento_orden_plazo"
    Friend Const MSTR_CALCULAR_DIAS_TITULO As String = "vencimiento_titulo"
    Private Const MSTR_ACCION_CALCULAR_DIAS As String = "dias"
    Private Const MSTR_ACCION_VALIDACION_GUARDADO_ORDEN As String = "guardado_orden"
    Private Const MSTR_ACCION_VALIDACION_GUARDADO_ORDEN_FECHACUMPLIMIENTO As String = "guardado_orden_fechacumplimiento"

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

    Private TIPONEGOCIO_ACCIONES As String = ""
    Private TIPONEGOCIO_REPO As String = ""
    Private TIPONEGOCIO_SIMULTANEA As String = ""
    Private TIPONEGOCIO_RENTAFIJA As String = ""
    Private TIPONEGOCIO_TTV As String = ""

    'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añaden los tipos de negocio para otras firmas acciones y renta fija
    Private TIPONEGOCIO_ACCIONESOF As String = ""
    Private TIPONEGOCIO_RENTAFIJAOF As String = ""

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

    Public MSTR_MODULO_OYD_ORDENES As String = "O"

    Private VELOCIDAD_TEXTO As String = ""

    Private STR_URLMOTORCALCULOS As String = ""
    Private LOG_HACERLOGMOTORCALCULOS As Boolean = False
    Private STR_RUTALOGMOTORCALCULOS As String = ""

    Private Const VISTA_APROBADAS As String = "Aprobadas"
    Private Const VISTA_PENDIENTESAPROBAR As String = "Pendientes aprobar"

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

    Private Enum TIPOMENSAJEUSUARIO
        CONFIRMACION
        JUSTIFICACION
        APROBACION
        TODOS
    End Enum

    Private Const HOJA_MOTORCALCULOS As String = "OyDPlus.CalculosOrdenesBolsa"

    Private Enum SALIDAS_CAMPOS_ACCIONESVENTA
        SALIDA_ACCIONESVENTA_COMISION
        SALIDA_ACCIONESVENTA_VALORCOMISION
        SALIDA_ACCIONESVENTA_IVACOMISION
        SALIDA_ACCIONESVENTA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_ACCIONESCOMPRA
        SALIDA_ACCIONESCOMPRA_COMISION
        SALIDA_ACCIONESCOMPRA_VALORCOMISION
        SALIDA_ACCIONESCOMPRA_IVACOMISION
        SALIDA_ACCIONESCOMPRA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_RENTAFIJAVENTA
        SALIDA_RENTAFIJAVENTA_VALORGIRO
        SALIDA_RENTAFIJAVENTA_TASAMERCADOEA
        SALIDA_RENTAFIJAVENTA_TASACLIENTEEA
        SALIDA_RENTAFIJAVENTA_VALORFURUROMERCADO
        SALIDA_RENTAFIJAVENTA_VALORFUTUROCLIENTE
        SALIDA_RENTAFIJAVENTA_COMISION
        SALIDA_RENTAFIJAVENTA_VALORCOMISION
        SALIDA_RENTAFIJAVENTA_IVACOMISION
        SALIDA_RENTAFIJAVENTA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_RENTAFIJACOMPRA
        SALIDA_RENTAFIJACOMPRA_VALORGIRO
        SALIDA_RENTAFIJACOMPRA_TASAMERCADOEA
        SALIDA_RENTAFIJACOMPRA_TASACLIENTEEA
        SALIDA_RENTAFIJACOMPRA_VALORFUTUROMERCADO
        SALIDA_RENTAFIJACOMPRA_VALORFUTUROCLIENTE
        SALIDA_RENTAFIJACOMPRA_COMISION
        SALIDA_RENTAFIJACOMPRA_VALORCOMISION
        SALIDA_RENTAFIJACOMPRA_IVACOMISION
        SALIDA_RENTAFIJACOMPRA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_REPOVENTA
        SALIDA_REPOVENTA_NROACCIONES
        SALIDA_REPOVENTA_PRECIOCONGARANTIA
        SALIDA_REPOVENTA_TASAMERCADOEA
        SALIDA_REPOVENTA_TASACLIENTEEA
        SALIDA_REPOVENTA_VALORCAPTACION
        SALIDA_REPOVENTA_VALORFUTUROMERCADO
        SALIDA_REPOVENTA_VALORFUTUROCLIENTE
        SALIDA_REPOVENTA_COMISION
        SALIDA_REPOVENTA_VALORCOMISION
        SALIDA_REPOVENTA_IVACOMISION
        SALIDA_REPOVENTA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_REPOCOMPRA
        SALIDA_REPOCOMPRA_TASAMERCADOEA
        SALIDA_REPOCOMPRA_TASACLIENTEEA
        SALIDA_REPOCOMPRA_VALORFUTUROMERCADO
        SALIDA_REPOCOMPRA_VALORFUTUROCLIENTE
        SALIDA_REPOCOMPRA_COMISION
        SALIDA_REPOCOMPRA_VALORCOMISION
        SALIDA_REPOCOMPRA_IVACOMISION
        SALIDA_REPOCOMPRA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_SIMULTANEAVENTA
        SALIDA_SIMULTANEAVENTA_VALORGIRO
        SALIDA_SIMULTANEAVENTA_TASAMERCADOEA
        SALIDA_SIMULTANEAVENTA_TASACLIENTEEA
        SALIDA_SIMULTANEAVENTA_VALORFUTUROMERCADO
        SALIDA_SIMULTANEAVENTA_VALORFUTUROCLIENTE
        SALIDA_SIMULTANEAVENTA_COMISION
        SALIDA_SIMULTANEAVENTA_VALORCOMISION
        SALIDA_SIMULTANEAVENTA_IVACOMISION
        SALIDA_SIMULTANEAVENTA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_SIMULTANEACOMPRA
        SALIDA_SIMULTANEACOMPRA_VALORGIRO
        SALIDA_SIMULTANEACOMPRA_TASAMERCADOEA
        SALIDA_SIMULTANEACOMPRA_TASACLIENTEEA
        SALIDA_SIMULTANEACOMPRA_VALORFUTUROMERCADO
        SALIDA_SIMULTANEACOMPRA_VALORFUTUROCLIENTE
        SALIDA_SIMULTANEACOMPRA_COMISION
        SALIDA_SIMULTANEACOMPRA_VALORCOMISION
        SALIDA_SIMULTANEACOMPRA_IVACOMISION
        SALIDA_SIMULTANEACOMPRA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_TTVVENTA
        SALIDA_TTVVENTA_VALORGIRO
        SALIDA_TTVVENTA_TASAMERCADOEA
        SALIDA_TTVVENTA_TASACLIENTEEA
        SALIDA_TTVVENTA_VALORFUTUROMERCADO
        SALIDA_TTVVENTA_VALORFUTUROCLIENTE
        SALIDA_TTVVENTA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_TTVCOMPRA
        SALIDA_TTVCOMPRA_VALORGIRO
        SALIDA_TTVCOMPRA_TASAMERCADOEA
        SALIDA_TTVCOMPRA_TASACLIENTEEA
        SALIDA_TTVCOMPRA_VALORFUTUROMERCADO
        SALIDA_TTVCOMPRA_VALORFUTUROCLIENTE
        SALIDA_TTVCOMPRA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_ACCIONESOFVENTA
        SALIDA_ACCIONESOFVENTA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_ACCIONESOFCOMPRA
        SALIDA_ACCIONESOFCOMPRA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_RENTAFIJAOFVENTA
        SALIDA_RENTAFIJAOFVENTA_VALORORDEN
    End Enum

    Private Enum SALIDAS_CAMPOS_RENTAFIJAOFCOMPRA
        SALIDA_RENTAFIJAOFCOMPRA_VALORORDEN
    End Enum

    Private Enum ENTRADAS_MOTORCALCULOS
        STR_ENCABEZADO_TIPOORDEN
        STR_ENCABEZADO_TIPONEGOCIO
        STR_ENCABEZADO_TIPOPRODUCTO
        STR_ENCABEZADO_TIPOOPERACION
        STR_ENCABEZADO_ESPECIE
        STR_ENCABEZADO_MODALIDAD
        STR_ENCABEZADO_INDICADOR
        DBL_ENCABEZADO_PUNTOSINDICADOR
        STR_ENCABEZADO_ENPESOS
        DBL_ENCABEZADO_CANTIDAD
        DBL_ENCABEZADO_PRECIO
        DBL_ENCABEZADO_PRECIOMAXIMONIMINO
        DBL_ENCABEZADO_VALORCAPTACION
        DBL_ENCABEZADO_VALORFUTUROREPO
        DBL_ENCABEZADO_TASAREGISTRO
        DBL_ENCABEZADO_TASACLIENTE
        DBL_ENCABEZADO_TASANOMINAL
        DBL_ENCABEZADO_CASTIGO
        DBL_ENCABEZADO_VALORACCION
        DBL_ENCABEZADO_COMISION
        DBL_ENCABEZADO_VALORCOMISION
        DBL_ENCABEZADO_VALORORDEN
        INT_ENCABEZADO_DIASREPO
        DBL_ENCABEZADO_IVACOMISION
        DBL_VALOR_IVA
        INT_ENCABEZADO_DIASVIGENCIA
        DBL_VALOR_BASE
        DBL_VALOR_BASE_REPO
    End Enum

#End Region

#Region "Propiedades de la busqueda"

    Public Property cb As CamposBusquedaOrdenOYDPLUS

#End Region

#Region "Propiedades para el Tipo de Orden"

    Private _mlogEsOrdenRENTAFIJAOYDPLUS As Boolean = False
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de renta variable (true) o no (false)
    ''' </summary>
    Public ReadOnly Property EsOrdenRENTAFIJAOYDPLUS As Boolean
        Get
            Return (_mlogEsOrdenRENTAFIJAOYDPLUS)
        End Get
    End Property

    Private _ListaOrdenOYDPLUS As List(Of OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
    Public Property ListaOrdenOYDPLUS() As List(Of OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Get
            Return _ListaOrdenOYDPLUS
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesOF.OrdenOYDPLUSOF))
            _ListaOrdenOYDPLUS = value
            MyBase.CambioItem("ListaOrdenOYDPLUS")
            MyBase.CambioItem("ListaOrdenesOYDPLUSPaged")
            If Not IsNothing(_ListaOrdenOYDPLUS) Then
                If _ListaOrdenOYDPLUS.Count > 0 And logCambiarSelected Then
                    OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.FirstOrDefault
                End If
            End If
        End Set
    End Property

    Private _strAgruparPorColumna As String
    Public Property AgruparPorColumna() As String
        Get
            Return _strAgruparPorColumna
        End Get
        Set(ByVal value As String)
            _strAgruparPorColumna = value
            ListaOrdenesOYDPLUSPaged.GroupDescriptions.Clear()
            ListaOrdenesOYDPLUSPaged.GroupDescriptions.Add(New PropertyGroupDescription(value))
            MyBase.CambioItem("AgruparPorColumna")
        End Set
    End Property

    Public ReadOnly Property ListaOrdenesOYDPLUSPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOrdenOYDPLUS) Then
                Dim view = New PagedCollectionView(_ListaOrdenOYDPLUS)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _OrdenDataForm As OyDPLUSOrdenesOF.OrdenOYDPLUSOF = New OyDPLUSOrdenesOF.OrdenOYDPLUSOF
    Public Property OrdenDataForm() As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
        Get
            Return _OrdenDataForm
        End Get
        Set(ByVal value As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
            _OrdenDataForm = value
            MyBase.CambioItem("OrdenDataForm")
        End Set
    End Property

    Private WithEvents _OrdenOYDPLUSSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
    Public Property OrdenOYDPLUSSelected() As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
        Get
            Return _OrdenOYDPLUSSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
            _OrdenOYDPLUSSelected = value
            MyBase.CambioItem("OrdenOYDPLUSSelected")
            BuscarControlValidacion(ViewOrdenesOYDPLUS, "tabItemValoresComisiones")
            Try
                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                    If (logNuevoRegistro = False Or logEditarRegistro = False) And logCambiarDetallesOrden Then
                        HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)

                        CalcularDiasPlazo(_OrdenOYDPLUSSelected)
                        'Obtiene los receptores de la orden
                        If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Clase) Then
                            'Buscar el cliente de la orden.
                            'BuscarClienteOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, String.Empty)

                            'Buscar los receptores de la orden.
                            consultarReceptoresOrden(_OrdenOYDPLUSSelected.Clase, _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.NroOrden, _OrdenOYDPLUSSelected.Version, _OrdenOYDPLUSSelected.EstadoOrden)

                            'Buscar los beneficiarios de la orden.
                            consultarBeneficiariosOrden()

                            'Buscar las liquidaciones de la orden.
                            consultarLiquidacionesOrden()

                            'Obtiene el numero de días de la orden.
                            Me.CalcularDiasOrdenOYDPLUS(MSTR_CALCULAR_DIAS_ORDEN, _OrdenOYDPLUSSelected, -1)
                        Else
                            IsBusy = False
                        End If
                    End If
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al realizar las consultas de la orden.",
                                Me.ToString(), "OrdenOYDPLUSSelected", Application.Current.ToString(), Program.Maquina, ex)
                IsBusy = False
            End Try
        End Set
    End Property

    Public Property OrdenAnterior As Object

    Private _OrdenAnteriorOYDPLUS As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
    Public Property OrdenAnteriorOYDPLUS() As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
        Get
            Return _OrdenAnteriorOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
            _OrdenAnteriorOYDPLUS = value
        End Set
    End Property

    Private _OrdenDuplicarOYDPLUS As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
    Public Property OrdenDuplicarOYDPLUS() As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
        Get
            Return _OrdenDuplicarOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
            _OrdenDuplicarOYDPLUS = value
        End Set
    End Property

    Private _OrdenPlantillaOYDPLUS As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
    Public Property OrdenPlantillaOYDPLUS() As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
        Get
            Return _OrdenPlantillaOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
            _OrdenPlantillaOYDPLUS = value
        End Set
    End Property

    Private _ListaDistribucionComisionSalvar As List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
    Public Property ListaDistribucionComisionSalvar() As List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
        Get
            Return _ListaDistribucionComisionSalvar
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesOF.ReceptoresOrden))
            _ListaDistribucionComisionSalvar = value
        End Set
    End Property

    Private _ListaReceptoresSalvar As List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
    Public Property ListaReceptoresSalvar() As List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
        Get
            Return _ListaReceptoresSalvar
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesOF.ReceptoresOrden))
            _ListaReceptoresSalvar = value
        End Set
    End Property

    Private _ViewOrdenesOYDPLUS As OrdenesPLUSOFView
    Public Property ViewOrdenesOYDPLUS() As OrdenesPLUSOFView
        Get
            Return _ViewOrdenesOYDPLUS
        End Get
        Set(ByVal value As OrdenesPLUSOFView)
            _ViewOrdenesOYDPLUS = value
        End Set
    End Property

    Private _BusquedaOrdenOyDPlus As CamposBusquedaOrdenOYDPLUS = New CamposBusquedaOrdenOYDPLUS
    Public Property BusquedaOrdenOyDPlus() As CamposBusquedaOrdenOYDPLUS
        Get
            Return _BusquedaOrdenOyDPlus
        End Get
        Set(ByVal value As CamposBusquedaOrdenOYDPLUS)
            _BusquedaOrdenOyDPlus = value
            MyBase.CambioItem("BusquedaOrdenOyDPlus")
        End Set
    End Property

#End Region

#Region "Propiedades Tipo Negocio"

    Private _ListaTipoNegocioCOMPLETOS As List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor)
    Public Property ListaTipoNegocioCOMPLETOS() As List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor)
        Get
            Return _ListaTipoNegocioCOMPLETOS
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor))
            _ListaTipoNegocioCOMPLETOS = value
            MyBase.CambioItem("ListaTipoNegocioCOMPLETOS")
        End Set
    End Property

    Private _ListaTipoNegocio As List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor)
    Public Property ListaTipoNegocio() As List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor)
        Get
            Return _ListaTipoNegocio
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor))
            _ListaTipoNegocio = value
            MyBase.CambioItem("ListaTipoNegocio")
        End Set
    End Property

    Private _TipoNegocioSelected As OYDPLUSUtilidades.tblTipoNegocioReceptor
    Public Property TipoNegocioSelected() As OYDPLUSUtilidades.tblTipoNegocioReceptor
        Get
            Return _TipoNegocioSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblTipoNegocioReceptor)
            _TipoNegocioSelected = value
            If Not IsNothing(TipoNegocioSelected) Then
                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                    If logNuevoRegistro Or logEditarRegistro Then
                        If logModificarDatosTipoNegocio Then
                            _OrdenOYDPLUSSelected.Comision = TipoNegocioSelected.PorcentajeComision
                            _OrdenOYDPLUSSelected.ValorComision = TipoNegocioSelected.ValorComision
                        End If
                    End If
                End If
            End If

            MyBase.CambioItem("TipoNegocioSelected")
        End Set
    End Property


#End Region

#Region "Propiedades para el Ordenante"

    Private _ListaOrdenantesOYDPLUS As List(Of OYDUtilidades.BuscadorOrdenantes)
    Public Property ListaOrdenantesOYDPLUS As List(Of OYDUtilidades.BuscadorOrdenantes)
        Get
            Return (_ListaOrdenantesOYDPLUS)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorOrdenantes))
            _ListaOrdenantesOYDPLUS = value
            MyBase.CambioItem("ListaOrdenantesOYDPLUS")
            If Not IsNothing(ListaOrdenantesOYDPLUS) Then
                If logNuevoRegistro = False And logEditarRegistro = False Then
                    If ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = _OrdenOYDPLUSSelected.IDOrdenante).Count > 0 Then
                        OrdenanteSeleccionadoOYDPLUS = ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = _OrdenOYDPLUSSelected.IDOrdenante).FirstOrDefault
                    End If
                Else
                    If ListaOrdenantesOYDPLUS.Count = 1 Then
                        OrdenanteSeleccionadoOYDPLUS = ListaOrdenantesOYDPLUS.FirstOrDefault
                    Else
                        If ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = _OrdenOYDPLUSSelected.IDOrdenante).Count > 0 Then
                            OrdenanteSeleccionadoOYDPLUS = ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = _OrdenOYDPLUSSelected.IDOrdenante).FirstOrDefault
                        End If
                    End If
                End If
            End If
        End Set
    End Property

    Private _mobjOrdenanteSeleccionadoOYDPLUS As OYDUtilidades.BuscadorOrdenantes
    Public Property OrdenanteSeleccionadoOYDPLUS() As OYDUtilidades.BuscadorOrdenantes
        Get
            Return (_mobjOrdenanteSeleccionadoOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorOrdenantes)
            _mobjOrdenanteSeleccionadoOYDPLUS = value
            If Not IsNothing(OrdenanteSeleccionadoOYDPLUS) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                        _OrdenOYDPLUSSelected.IDOrdenante = OrdenanteSeleccionadoOYDPLUS.IdOrdenante
                    End If
                End If
            End If
            MyBase.CambioItem("OrdenanteSeleccionadoOYDPLUS")
        End Set
    End Property

#End Region

#Region "Propiedades para las cuentas deposito"

    Private _ListaCuentasDepositoOYDPLUS As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property ListaCuentasDepositoOYDPLUS As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return (_ListaCuentasDepositoOYDPLUS)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _ListaCuentasDepositoOYDPLUS = value
            MyBase.CambioItem("ListaCuentasDepositoOYDPLUS")
            If Not IsNothing(ListaCuentasDepositoOYDPLUS) Then
                If logNuevoRegistro = False And logEditarRegistro = False Then
                    If ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = _OrdenOYDPLUSSelected.CuentaDeposito).Count > 0 Then
                        CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = _OrdenOYDPLUSSelected.CuentaDeposito).FirstOrDefault
                    End If
                Else
                    If ListaCuentasDepositoOYDPLUS.Count = 1 Then
                        CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.FirstOrDefault
                    Else
                        If ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = _OrdenOYDPLUSSelected.CuentaDeposito).Count > 0 Then
                            CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = _OrdenOYDPLUSSelected.CuentaDeposito).FirstOrDefault
                        End If
                    End If
                End If
            Else
                CtaDepositoSeleccionadaOYDPLUS = Nothing
            End If
        End Set
    End Property

    Private _mobjCtaDepositoSeleccionadaOYDPLUS As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CtaDepositoSeleccionadaOYDPLUS() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCtaDepositoSeleccionadaOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            _mobjCtaDepositoSeleccionadaOYDPLUS = value
            If Not IsNothing(CtaDepositoSeleccionadaOYDPLUS) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                        _OrdenOYDPLUSSelected.CuentaDeposito = CtaDepositoSeleccionadaOYDPLUS.NroCuentaDeposito
                        _OrdenOYDPLUSSelected.UBICACIONTITULO = CtaDepositoSeleccionadaOYDPLUS.Deposito
                    End If
                End If
            Else
                If logEditarRegistro Or logNuevoRegistro Then
                    _OrdenOYDPLUSSelected.CuentaDeposito = 0
                    _OrdenOYDPLUSSelected.UBICACIONTITULO = String.Empty
                End If
            End If
            MyBase.CambioItem("CtaDepositoSeleccionadaOYDPLUS")
        End Set
    End Property


#End Region

#Region "Propiedades para el receptor de la orden"

    '******************************************************** ReceptoresOrdenes 
    Private _ListaReceptoresOrdenes As List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
    Public Property ListaReceptoresOrdenes() As List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
        Get
            Return _ListaReceptoresOrdenes
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesOF.ReceptoresOrden))
            _ListaReceptoresOrdenes = value
            MyBase.CambioItem("ListaReceptoresOrdenes")
            MyBase.CambioItem("ListaReceptoresOrdenesPaged")
            If Not IsNothing(_ListaReceptoresOrdenes) Then
                If _ListaReceptoresOrdenes.Count > 0 Then
                    ReceptoresOrdenSelected = _ListaReceptoresOrdenes.FirstOrDefault
                End If
            End If
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

    Private _ReceptoresOrdenSelected As OyDPLUSOrdenesOF.ReceptoresOrden
    Public Property ReceptoresOrdenSelected() As OyDPLUSOrdenesOF.ReceptoresOrden
        Get
            Return _ReceptoresOrdenSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesOF.ReceptoresOrden)
            If Not IsNothing(value) Then
                _ReceptoresOrdenSelected = value
                MyBase.CambioItem("ReceptoresOrdenSelected")
            End If
        End Set
    End Property

#End Region

#Region "Propiedades para los beneficiarios de la orden"

    '******************************************************** BeneficiariosOrdenes 
    Private _ListaBeneficiariosOrdenes As List(Of OyDPLUSOrdenesOF.BeneficiariosOrden)
    Public Property ListaBeneficiariosOrdenes() As List(Of OyDPLUSOrdenesOF.BeneficiariosOrden)
        Get
            Return _ListaBeneficiariosOrdenes
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesOF.BeneficiariosOrden))
            _ListaBeneficiariosOrdenes = value
            MyBase.CambioItem("ListaBeneficiariosOrdenes")
            MyBase.CambioItem("ListaBeneficiariosOrdenesPaged")
            If Not IsNothing(_ListaBeneficiariosOrdenes) Then
                If _ListaBeneficiariosOrdenes.Count > 0 Then
                    BeneficiariosOrdenSelected = _ListaBeneficiariosOrdenes.FirstOrDefault
                End If
            End If
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

    Private _BeneficiariosOrdenSelected As OyDPLUSOrdenesOF.BeneficiariosOrden
    Public Property BeneficiariosOrdenSelected() As OyDPLUSOrdenesOF.BeneficiariosOrden
        Get
            Return _BeneficiariosOrdenSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesOF.BeneficiariosOrden)
            _BeneficiariosOrdenSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("BeneficiariosOrdenSelected")
            End If
        End Set
    End Property

#End Region

#Region "Propiedades para las liquidaciones problables"

    Private _ListaLiqAsociadasOrdenes As List(Of OyDPLUSOrdenesOF.LiquidacionesOrden)
    Public Property ListaLiqAsociadasOrdenes() As List(Of OyDPLUSOrdenesOF.LiquidacionesOrden)
        Get
            Return _ListaLiqAsociadasOrdenes
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesOF.LiquidacionesOrden))
            _ListaLiqAsociadasOrdenes = value
            MyBase.CambioItem("ListaLiqAsociadasOrdenes")
        End Set
    End Property


#End Region

#Region "Propiedades para cargar Información de los Combos"

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

    Private _DiccionarioCombosOYDPlusCompleta As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlusCompleta() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlusCompleta
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlusCompleta = value
            MyBase.CambioItem("DiccionarioCombosOYDPlusCompleta")
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

    Private _ListaReceptoresUsuario As List(Of OYDPLUSUtilidades.tblReceptoresUsuario)
    Public Property ListaReceptoresUsuario() As List(Of OYDPLUSUtilidades.tblReceptoresUsuario)
        Get
            Return _ListaReceptoresUsuario
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblReceptoresUsuario))
            _ListaReceptoresUsuario = value
            MyBase.CambioItem("ListaReceptoresUsuario")
        End Set
    End Property

    Private _ConfiguracionReceptor As OYDPLUSUtilidades.tblConfiguracionesAdicionalesReceptor
    Public Property ConfiguracionReceptor() As OYDPLUSUtilidades.tblConfiguracionesAdicionalesReceptor
        Get
            Return _ConfiguracionReceptor
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblConfiguracionesAdicionalesReceptor)
            _ConfiguracionReceptor = value
            MyBase.CambioItem("ConfiguracionReceptor")
        End Set
    End Property

    Private _ListaParametrosReceptor As List(Of OYDPLUSUtilidades.tblParametrosReceptor)
    Public Property ListaParametrosReceptor() As List(Of OYDPLUSUtilidades.tblParametrosReceptor)
        Get
            Return _ListaParametrosReceptor
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblParametrosReceptor))
            _ListaParametrosReceptor = value
            MyBase.CambioItem("ListaParametrosReceptor")
        End Set
    End Property

    Private _ListaEspeciesTipoNegocio As List(Of OYDPLUSUtilidades.tblEspeciesXTipoNegocio)
    Public Property ListaEspeciesTipoNegocio() As List(Of OYDPLUSUtilidades.tblEspeciesXTipoNegocio)
        Get
            Return _ListaEspeciesTipoNegocio
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblEspeciesXTipoNegocio))
            _ListaEspeciesTipoNegocio = value
            MyBase.CambioItem("ListaEspeciesTipoNegocio")
        End Set
    End Property

#End Region

#Region "Propiedades para la parte de los mensajes TICKER"

    Private _TituloMensaje As String
    Public Property TituloMensaje() As String
        Get
            Return _TituloMensaje
        End Get
        Set(ByVal value As String)
            _TituloMensaje = value
            MyBase.CambioItem("TituloMensaje")
        End Set
    End Property

    Private _ListaMensajes As List(Of OYDPLUSUtilidades.tblMensajes)
    Public Property ListaMensajes() As List(Of OYDPLUSUtilidades.tblMensajes)
        Get
            Return _ListaMensajes
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblMensajes))
            _ListaMensajes = value
            MyBase.CambioItem("ListaMensajes")
        End Set
    End Property

    Private _VelocidadMensaje As String
    Public Property VelocidadMensaje() As String
        Get
            Return _VelocidadMensaje
        End Get
        Set(ByVal value As String)
            _VelocidadMensaje = value
            MyBase.CambioItem("VelocidadMensaje")
        End Set
    End Property


#End Region

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


    Private _TextoRomperCruzada As String = "* La orden cruzada " & vbCrLf & "ya no tiene " & vbCrLf & "cruce activo."
    Public Property TextoRomperCruzada() As String
        Get
            Return _TextoRomperCruzada
        End Get
        Set(ByVal value As String)
            _TextoRomperCruzada = value
            MyBase.CambioItem("TextoRomperCruzada")
        End Set
    End Property

    Private _TextoCruzadaReceptor As String = "Orden cruzada con " & vbCrLf & "otro receptor"
    Public Property TextoCruzadaReceptor() As String
        Get
            Return _TextoCruzadaReceptor
        End Get
        Set(ByVal value As String)
            _TextoCruzadaReceptor = value
            MyBase.CambioItem("TextoCruzadaReceptor")
        End Set
    End Property

    Private _TextoCruzadaCliente As String = "Orden cruzada con" & vbCrLf & "uno de mis clientes"
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

#Region "Propiedades para habilitar los controles de la pantalla"

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

    Private _HabilitarOpcionesCruzada As Boolean = False
    Public Property HabilitarOpcionesCruzada() As Boolean
        Get
            Return _HabilitarOpcionesCruzada
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOpcionesCruzada = value
            MyBase.CambioItem("HabilitarOpcionesCruzada")
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

    Private _MostrarBeneficiarios As Visibility = Visibility.Collapsed
    Public Property MostrarBeneficiarios() As Visibility
        Get
            Return _MostrarBeneficiarios
        End Get
        Set(ByVal value As Visibility)
            _MostrarBeneficiarios = value
            MyBase.CambioItem("MostrarBeneficiarios")
        End Set
    End Property

    Private _PermitirGuardar As Boolean
    Public Property PermitirGuardar() As Boolean
        Get
            Return _PermitirGuardar
        End Get
        Set(ByVal value As Boolean)
            _PermitirGuardar = value
            MyBase.CambioItem("PermitirGuardar")
        End Set
    End Property

    Private _MostrarNoEdicion As Visibility = Visibility.Visible
    Public Property MostrarNoEdicion() As Visibility
        Get
            Return _MostrarNoEdicion
        End Get
        Set(ByVal value As Visibility)
            _MostrarNoEdicion = value
            MyBase.CambioItem("MostrarNoEdicion")
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

    Private _MostrarCruzadaCon As Visibility = Visibility.Collapsed
    Public Property MostrarCruzadaCon() As Visibility
        Get
            Return _MostrarCruzadaCon
        End Get
        Set(ByVal value As Visibility)
            _MostrarCruzadaCon = value
            MyBase.CambioItem("MostrarCruzadaCon")
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

    Private _HabilitarComboISIN As Boolean = False
    Public Property HabilitarComboISIN() As Boolean
        Get
            Return _HabilitarComboISIN
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComboISIN = value
            MyBase.CambioItem("HabilitarComboISIN")
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

    Private _MostrarTipoNegocioRepoVenta As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioRepoVenta() As Visibility
        Get
            Return _MostrarTipoNegocioRepoVenta
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioRepoVenta = value
            MyBase.CambioItem("MostrarTipoNegocioRepoVenta")
        End Set
    End Property

    Private _MostrarTipoNegocioRepoCompra As Visibility = Visibility.Collapsed
    Public Property MostrarTipoNegocioRepoCompra() As Visibility
        Get
            Return _MostrarTipoNegocioRepoCompra
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoNegocioRepoCompra = value
            MyBase.CambioItem("MostrarTipoNegocioRepoCompra")
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

    Private _TextoCruces As String
    Public Property TextoCruces() As String
        Get
            Return _TextoCruces
        End Get
        Set(ByVal value As String)
            _TextoCruces = value
            MyBase.CambioItem("TextoCruces")
        End Set
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

#End Region

#Region "Propiedades para la Validacion de la Orden"

    Private _ListaResultadoValidacion As List(Of OyDPLUSOrdenesOF.tblRespuestaValidaciones)
    Public Property ListaResultadoValidacion() As List(Of OyDPLUSOrdenesOF.tblRespuestaValidaciones)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesOF.tblRespuestaValidaciones))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
        End Set
    End Property

    Private _CantidadValidacionesUsuario As Integer
    Public Property CantidadValidacionesUsuario() As Integer
        Get
            Return _CantidadValidacionesUsuario
        End Get
        Set(ByVal value As Integer)
            _CantidadValidacionesUsuario = value
            MyBase.CambioItem("CantidadValidacionesUsuario")
        End Set
    End Property


#End Region

#Region "Propiedades del cliente"

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
                If Not IsNothing(ComitenteSeleccionadoOYDPLUS) Then
                    OrdenanteSeleccionadoOYDPLUS = Nothing
                    CtaDepositoSeleccionadaOYDPLUS = Nothing
                    consultarOrdenantesOYDPLUS(ComitenteSeleccionadoOYDPLUS.IdComitente)
                    consultarCuentasDepositoOYDPLUS(ComitenteSeleccionadoOYDPLUS.IdComitente)
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

#End Region

#Region "Propiedades de la Especie"

    Private _NemotecnicoSeleccionadoOYDPLUS As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionadoOYDPLUS As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionadoOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionadoOYDPLUS = value
            Try
                If logCancelarRegistro = False Then
                    SeleccionarEspecieOYDPLUS(NemotecnicoSeleccionadoOYDPLUS)
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad de la especie seleccionada.", Me.ToString, "NemotecnicoSeleccionadoOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try
            MyBase.CambioItem("NemotecnicoSeleccionadoOYDPLUS")
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


#End Region

#Region "Propiedades genericas"
    ''' <summary>
    ''' Propiedad que permite manejar las opciones del modulo del combo cboModulo
    ''' </summary>
    ''' <remarks>Por: Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _Modulo As String = "ORDENESOF"
    <Display(Name:="Módulo")>
    Public Property Modulo() As String
        Get
            Return _Modulo
        End Get
        Set(ByVal value As String)
            _Modulo = value
            MyBase.CambioItem("Modulo")
        End Set
    End Property

    Private _BolsaValores As String
    Public Property BolsaValores() As String
        Get
            Return _BolsaValores
        End Get
        Set(ByVal value As String)
            _BolsaValores = value
            MyBase.CambioItem("BolsaValores")
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

    Private _CantidadConfirmaciones As Integer = 0
    Public Property CantidadConfirmaciones() As Integer
        Get
            Return _CantidadConfirmaciones
        End Get
        Set(ByVal value As Integer)
            _CantidadConfirmaciones = value
            If CantidadConfirmaciones > 0 Then
                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
                    ValidarOrdenOriginaloCruzada()
                End If
            End If
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
                    ValidarOrdenOriginaloCruzada()
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
                    ValidarOrdenOriginaloCruzada()
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

    Private _IsBusyPlantilla As Boolean = False
    Public Property IsBusyPlantilla() As Boolean
        Get
            Return _IsBusyPlantilla
        End Get
        Set(ByVal value As Boolean)
            _IsBusyPlantilla = value
            MyBase.CambioItem("IsBusyPlantilla")
        End Set
    End Property

    Private _HabilitarGenerarPlantillas As Boolean
    Public Property HabilitarGenerarPlantillas() As Boolean
        Get
            Return _HabilitarGenerarPlantillas
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGenerarPlantillas = value
            MyBase.CambioItem("HabilitarGenerarPlantillas")
        End Set
    End Property

    Private _HabilitarAbrirPlantillas As Boolean
    Public Property HabilitarAbrirPlantillas() As Boolean
        Get
            Return _HabilitarAbrirPlantillas
        End Get
        Set(ByVal value As Boolean)
            _HabilitarAbrirPlantillas = value
            MyBase.CambioItem("HabilitarAbrirPlantillas")
        End Set
    End Property

    Private _HabilitarDuplicar As Boolean
    Public Property HabilitarDuplicar() As String
        Get
            Return _HabilitarDuplicar
        End Get
        Set(ByVal value As String)
            _HabilitarDuplicar = value
            MyBase.CambioItem("HabilitarDuplicar")
        End Set
    End Property

#End Region

#Region "Propiedades Plantillas"

    Private _FiltroPlantillas As String = String.Empty
    Public Property FiltroPlantillas() As String
        Get
            Return _FiltroPlantillas
        End Get
        Set(ByVal value As String)
            _FiltroPlantillas = value
            MyBase.CambioItem("FiltroPlantillas")
        End Set
    End Property


    Private _ListaPlantillas As List(Of OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
    Public Property ListaPlantillas() As List(Of OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Get
            Return _ListaPlantillas
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesOF.OrdenOYDPLUSOF))
            _ListaPlantillas = value
            MyBase.CambioItem("ListaPlantillas")
            MyBase.CambioItem("ListaPlantillasPaged")
        End Set
    End Property

    Public ReadOnly Property ListaPlantillasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPlantillas) Then
                Dim view = New PagedCollectionView(_ListaPlantillas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _PlantillaSeleccionada As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
    Public Property PlantillaSeleccionada() As OyDPLUSOrdenesOF.OrdenOYDPLUSOF
        Get
            Return _PlantillaSeleccionada
        End Get
        Set(ByVal value As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
            _PlantillaSeleccionada = value
            MyBase.CambioItem("PlantillaSeleccionada")
        End Set
    End Property


#End Region

#Region "Propiedades de seleccion de los controles"

    Private _OrdenSAEAccionesSelected As OYDPLUSUtilidades.tblOrdenesSAEAcciones
    Public Property OrdenSAEAccionesSelected() As OYDPLUSUtilidades.tblOrdenesSAEAcciones
        Get
            Return _OrdenSAEAccionesSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOrdenesSAEAcciones)
            _OrdenSAEAccionesSelected = value
            'Obtiene la información de la orden de SAE.
            ObtenerInformacionOrdenSAEAcciones(OrdenSAEAccionesSelected, _OrdenOYDPLUSSelected, True)
            MyBase.CambioItem("OrdenSAEAccionesSelected")
        End Set
    End Property

    Private _OrdenSAERentaFijaSelected As OYDPLUSUtilidades.tblOrdenesSAERentaFija
    Public Property OrdenSAERentaFijaSelected() As OYDPLUSUtilidades.tblOrdenesSAERentaFija
        Get
            Return _OrdenSAERentaFijaSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOrdenesSAERentaFija)
            _OrdenSAERentaFijaSelected = value
            'Obtiene la información de la orden de SAE.
            ObtenerInformacionOrdenSAERentaFija(OrdenSAERentaFijaSelected, _OrdenOYDPLUSSelected, True)
            MyBase.CambioItem("OrdenSAERentaFijaSelected")
        End Set
    End Property

    Private _OrdenSAESeleccionada As Boolean
    Public Property OrdenSAESeleccionada() As Boolean
        Get
            Return _OrdenSAESeleccionada
        End Get
        Set(ByVal value As Boolean)
            _OrdenSAESeleccionada = value
            If OrdenSAESeleccionada Then
                'Obtiene la información de la orden de SAE.
                If _OrdenOYDPLUSSelected.Clase = TIPONEGOCIO_ACCIONES Or _OrdenOYDPLUSSelected.Clase = TIPONEGOCIO_ACCIONESOF Then
                    ObtenerInformacionOrdenSAEAcciones(OrdenSAEAccionesSelected, _OrdenOYDPLUSSelected, True)
                Else
                    ObtenerInformacionOrdenSAERentaFija(OrdenSAERentaFijaSelected, _OrdenOYDPLUSSelected, True)
                End If
            End If
            MyBase.CambioItem("OrdenSAESeleccionada")
        End Set
    End Property

    Private _PortafolioClienteSelected As OYDPLUSUtilidades.tblPortafolioCliente
    Public Property PortafolioClienteSelected() As OYDPLUSUtilidades.tblPortafolioCliente
        Get
            Return _PortafolioClienteSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblPortafolioCliente)
            _PortafolioClienteSelected = value
            MyBase.CambioItem("PortafolioClienteSelected")
        End Set
    End Property

    Private _PortafolioSeleccionada As Boolean
    Public Property PortafolioSeleccionada() As Boolean
        Get
            Return _PortafolioSeleccionada
        End Get
        Set(ByVal value As Boolean)
            _PortafolioSeleccionada = value
            If PortafolioSeleccionada Then
                'Obtiene la información del portafolio del cliente.
                ObtenerInformacionPortafolioCliente(PortafolioClienteSelected, _OrdenOYDPLUSSelected, True)
            Else
                _OrdenOYDPLUSSelected.Custodia = 0
                _OrdenOYDPLUSSelected.CustodiaSecuencia = 0
            End If
            MyBase.CambioItem("PortafolioSeleccionada")
        End Set
    End Property

    Private _OperacionesXCumplirSelected As OYDPLUSUtilidades.tblOperacionesCumplir
    Public Property OperacionesXCumplirSelected() As OYDPLUSUtilidades.tblOperacionesCumplir
        Get
            Return _OperacionesXCumplirSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOperacionesCumplir)
            _OperacionesXCumplirSelected = value
            MyBase.CambioItem("OperacionesXCumplirSelected")
        End Set
    End Property

    Private _OperacionSeleccionada As Boolean
    Public Property OperacionSeleccionada() As Boolean
        Get
            Return _OperacionSeleccionada
        End Get
        Set(ByVal value As Boolean)
            _OperacionSeleccionada = value
            If OperacionSeleccionada Then
                'Obtiene la información de las operaciones x cumplir.
                ObtenerInformacionOperacionesXCumplir(OperacionesXCumplirSelected, _OrdenOYDPLUSSelected, True)
            End If
            MyBase.CambioItem("OperacionSeleccionada")
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

    Private _ConsultarPortafolio As Boolean = False
    Public Property ConsultarPortafolio() As Boolean
        Get
            Return _ConsultarPortafolio
        End Get
        Set(ByVal value As Boolean)
            _ConsultarPortafolio = value
            MyBase.CambioItem("ConsultarPortafolio")
        End Set
    End Property

    Private _ConsultarOperaciones As Boolean = False
    Public Property ConsultarOperaciones() As Boolean
        Get
            Return _ConsultarOperaciones
        End Get
        Set(ByVal value As Boolean)
            _ConsultarOperaciones = value
            MyBase.CambioItem("ConsultarOperaciones")
        End Set
    End Property

    Private _ConsultarSaldo As Boolean = False
    Public Property ConsultarSaldo() As Boolean
        Get
            Return _ConsultarSaldo
        End Get
        Set(ByVal value As Boolean)
            _ConsultarSaldo = value
            MyBase.CambioItem("ConsultarSaldo")
        End Set
    End Property

    Private _ClienteBuscar As String
    Public Property ClienteBuscar() As String
        Get
            Return _ClienteBuscar
        End Get
        Set(ByVal value As String)
            _ClienteBuscar = value
            MyBase.CambioItem("ClienteBuscar")
        End Set
    End Property

    Private _EspecieBuscar As String
    Public Property EspecieBuscar() As String
        Get
            Return _EspecieBuscar
        End Get
        Set(ByVal value As String)
            _EspecieBuscar = value
            MyBase.CambioItem("EspecieBuscar")
        End Set
    End Property

    Private _CodigoOYDControles As String
    Public Property CodigoOYDControles() As String
        Get
            Return _CodigoOYDControles
        End Get
        Set(ByVal value As String)
            _CodigoOYDControles = value
            MyBase.CambioItem("CodigoOYDControles")
        End Set
    End Property

    Private _TipoNegocioControles As String
    Public Property TipoNegocioControles() As String
        Get
            Return _TipoNegocioControles
        End Get
        Set(ByVal value As String)
            _TipoNegocioControles = value
            MyBase.CambioItem("TipoNegocioControles")
        End Set
    End Property

    Private _TipoOperacionControles As String
    Public Property TipoOperacionControles() As String
        Get
            Return _TipoOperacionControles
        End Get
        Set(ByVal value As String)
            _TipoOperacionControles = value
            MyBase.CambioItem("TipoOperacionControles")
        End Set
    End Property

    Private _EspecieControles As String
    Public Property EspecieControles() As String
        Get
            Return _EspecieControles
        End Get
        Set(ByVal value As String)
            _EspecieControles = value
            MyBase.CambioItem("EspecieControles")
        End Set
    End Property


#End Region

#End Region

#Region "Metodos OYDPlus"

    Public Overrides Async Sub NuevoRegistro()
        Try
            If logCargoForma = False Then
                ViewOrdenesOYDPLUS.GridEdicion.Children.Add(viewFormaOrdenes)
                logCargoForma = True
            End If

            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
            mdtmFechaCierreSistema = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaCierreSistema(MSTR_MODULO_OYD_ORDENES, Program.Usuario)

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                OrdenAnteriorOYDPLUS = Nothing
                ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)
            End If

            logNuevoRegistro = True
            logEditarRegistro = False
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logCancelarRegistro = False

            CargarReceptoresUsuarioOYDPLUS("NUEVO", "NUEVO")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Crea el objeto nuevo de la orden despues de realizar la carga de los receptores y combos del usuario.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub NuevaOrden()
        Try
            HabilitarEncabezado = True
            HabilitarOpcionesCruzada = True
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

            Dim objNewOrdenOYDPLUS As New OyDPLUSOrdenesOF.OrdenOYDPLUSOF
            objNewOrdenOYDPLUS.IDNroOrden = 0
            objNewOrdenOYDPLUS.NroOrden = 0
            objNewOrdenOYDPLUS.Receptor = String.Empty
            objNewOrdenOYDPLUS.TipoOrden = String.Empty
            objNewOrdenOYDPLUS.NombreTipoOrden = String.Empty
            objNewOrdenOYDPLUS.TipoNegocio = String.Empty
            objNewOrdenOYDPLUS.NombreTipoNegocio = String.Empty
            objNewOrdenOYDPLUS.TipoProducto = String.Empty
            objNewOrdenOYDPLUS.NombreTipoProducto = String.Empty
            objNewOrdenOYDPLUS.TipoOperacion = String.Empty
            objNewOrdenOYDPLUS.NombreTipoOperacion = String.Empty
            objNewOrdenOYDPLUS.Clase = String.Empty
            objNewOrdenOYDPLUS.FechaOrden = dtmFechaServidor
            objNewOrdenOYDPLUS.Clasificacion = String.Empty
            objNewOrdenOYDPLUS.TipoLimite = String.Empty
            objNewOrdenOYDPLUS.Duracion = String.Empty
            objNewOrdenOYDPLUS.FechaVigencia = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
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
            objNewOrdenOYDPLUS.UBICACIONTITULO = String.Empty
            objNewOrdenOYDPLUS.CuentaDeposito = 0
            objNewOrdenOYDPLUS.DescripcionCta = String.Empty
            objNewOrdenOYDPLUS.UsuarioOperador = String.Empty
            objNewOrdenOYDPLUS.CanalRecepcion = String.Empty
            objNewOrdenOYDPLUS.MedioVerificable = String.Empty
            objNewOrdenOYDPLUS.FechaRecepcion = dtmFechaServidor
            objNewOrdenOYDPLUS.NroExtensionToma = String.Empty
            objNewOrdenOYDPLUS.Especie = "(No Seleccionado)"
            objNewOrdenOYDPLUS.ISIN = "(No Seleccionado)"
            objNewOrdenOYDPLUS.FechaEmision = Nothing
            objNewOrdenOYDPLUS.FechaVencimiento = Nothing
            objNewOrdenOYDPLUS.Estandarizada = False
            objNewOrdenOYDPLUS.FechaCumplimiento = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
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
            objNewOrdenOYDPLUS.FechaActualizacion = dtmFechaServidor
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

            'objOrden.UsuarioOperador=ConfiguracionReceptor.
            If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
                If DiccionarioCombosOYDPlusCompleta.ContainsKey("O_ESTADOS_ORDEN") Then
                    If DiccionarioCombosOYDPlusCompleta.Item("O_ESTADOS_ORDEN").Count > 0 Then
                        objNewOrdenOYDPLUS.EstadoOrden = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Retorno
                        objNewOrdenOYDPLUS.NombreEstadoOrden = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Descripcion
                    Else
                        objNewOrdenOYDPLUS.EstadoOrden = "P"
                        objNewOrdenOYDPLUS.NombreEstadoOrden = "Pendiente"
                    End If
                Else
                    objNewOrdenOYDPLUS.EstadoOrden = "P"
                    objNewOrdenOYDPLUS.NombreEstadoOrden = "Pendiente"
                End If

                If DiccionarioCombosOYDPlusCompleta.ContainsKey("O_ESTADOS_ORDEN_LEO") Then
                    If DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Count > 0 Then
                        objNewOrdenOYDPLUS.EstadoLEO = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Retorno
                        objNewOrdenOYDPLUS.NombreEstadoLEO = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Descripcion
                    Else
                        objNewOrdenOYDPLUS.EstadoLEO = "R"
                        objNewOrdenOYDPLUS.NombreEstadoLEO = "Recibida"
                    End If
                Else
                    objNewOrdenOYDPLUS.EstadoLEO = "R"
                    objNewOrdenOYDPLUS.NombreEstadoLEO = "Recibida"
                End If

            End If

            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                If DiccionarioCombosOYDPlus("BOLSA").Count > 0 Then
                    ' EOMC -- 11/20/2012
                    ' Retorno identifica la bolsa, se estaba asignando el id de la tabla de combosReceptor
                    objNewOrdenOYDPLUS.Bolsa = DiccionarioCombosOYDPlus("BOLSA").FirstOrDefault.Retorno
                    objNewOrdenOYDPLUS.NombreBolsa = DiccionarioCombosOYDPlus("BOLSA").FirstOrDefault.Descripcion
                End If
            End If

            MostrarCruzadaCon = Visibility.Collapsed
            MostrarOrdenesSAE = Visibility.Collapsed
            MostrarCamposCompra = Visibility.Collapsed
            MostrarCamposVenta = Visibility.Collapsed

            Confirmaciones = String.Empty

            'Limpia la lista de combos cuando es una nueva orden
            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                DiccionarioCombosOYDPlus.Clear()
            End If

            If Not IsNothing(ListaTipoNegocio) Then
                ListaTipoNegocio.Clear()
            End If

            logCalcularValores = False

            ObtenerValoresOrdenAnterior(objNewOrdenOYDPLUS, OrdenOYDPLUSSelected)

            logCalcularValores = True

            OrdenanteSeleccionadoOYDPLUS = Nothing
            If Not IsNothing(ListaCuentasDepositoOYDPLUS) Then
                Dim objListaCuenta As New List(Of OYDUtilidades.BuscadorCuentasDeposito)
                ListaCuentasDepositoOYDPLUS = Nothing
                ListaCuentasDepositoOYDPLUS = objListaCuenta
            End If

            CtaDepositoSeleccionadaOYDPLUS = Nothing
            If Not IsNothing(ListaReceptoresOrdenes) Then
                Dim objListaRep As New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
                ListaReceptoresOrdenes = Nothing
                ReceptoresOrdenSelected = Nothing
                ListaReceptoresOrdenes = objListaRep
            End If

            BeneficiariosOrdenSelected = Nothing
            If Not IsNothing(ListaBeneficiariosOrdenes) Then
                Dim objListaBen As New List(Of OyDPLUSOrdenesOF.BeneficiariosOrden)
                ListaBeneficiariosOrdenes = Nothing
                BeneficiariosOrdenSelected = Nothing
                ListaBeneficiariosOrdenes = objListaBen
            End If

            If Not IsNothing(ListaLiqAsociadasOrdenes) Then
                Dim objListaLiq As New List(Of OyDPLUSOrdenesOF.LiquidacionesOrden)
                ListaLiqAsociadasOrdenes = Nothing
                ListaLiqAsociadasOrdenes = objListaLiq
            End If

            Editando = True

            logCalcularValores = True

            'Valida si tiene un receptor por defecto.
            If Not IsNothing(ListaReceptoresUsuario) Then
                If ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).Count > 0 Then
                    _OrdenOYDPLUSSelected.Receptor = ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).First.CodigoReceptor
                ElseIf ListaReceptoresUsuario.Count = 1 Then
                    _OrdenOYDPLUSSelected.Receptor = ListaReceptoresUsuario.FirstOrDefault.CodigoReceptor
                Else
                    IsBusy = False
                End If
            Else
                IsBusy = False
            End If

            If BorrarCliente Then
                BorrarCliente = False
            End If

            BorrarCliente = True

            If BorrarEspecie Then
                BorrarEspecie = False
            End If

            BorrarEspecie = True

            BuscarControlValidacion(ViewOrdenesOYDPLUS, "tabItemValoresComisiones")
            'Se posiciona en el primer registro
            BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboReceptores")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevaOrden", Program.TituloSistema, Program.Maquina, ex)
            HabilitarEncabezado = False
            HabilitarOpcionesCruzada = False
            HabilitarNegocio = False
            HabilitarDuracion = False
            MostrarNegocio = Visibility.Visible
            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)
            Editando = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para preguntar sí se desea duplicar la orden que este seleccionada.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 22 de marzo del 2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PreguntarDuplicarOrden()
        Try
            If logCargoForma = False Then
                ViewOrdenesOYDPLUS.GridEdicion.Children.Add(viewFormaOrdenes)
                logCargoForma = True
            End If

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                mostrarMensajePregunta("¿Esta seguro que desea duplicar la orden?",
                                   Program.TituloSistema,
                                   "DUPLICARORDEN",
                                   AddressOf TerminoMensajePregunta, False)
            Else
                mostrarMensaje("Debe de seleccionar una orden para duplicar la orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preguntar sí se deseaba duplicar la orden", Me.ToString(), "PreguntarDuplicarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PreguntarGenerarPlantillaOrden()
        Try
            If logCargoForma = False Then
                ViewOrdenesOYDPLUS.GridEdicion.Children.Add(viewFormaOrdenes)
                logCargoForma = True
            End If

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                Dim objNombrePlatilla As New NombrePlantillaOYDPLUSView(Me)
                Program.Modal_OwnerMainWindowsPrincipal(objNombrePlatilla)
                objNombrePlatilla.ShowDialog()
            Else
                mostrarMensaje("Debe de seleccionar una orden para generar la plantilla.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preguntar sí se deseaba generar la plantilla de la orden", Me.ToString(), "PreguntarGenerarPlantillaOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PreguntarAbrirConsultaPlantillasOrden()
        Try
            Dim objPlantillas As New ConsultaPlantillasView(Me)
            Program.Modal_OwnerMainWindowsPrincipal(objPlantillas)
            objPlantillas.ShowDialog()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preguntar sí se deseaba consultar la plantilla de la orden", Me.ToString(), "PreguntarAbrirConsultaPlantillasOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ValidarExistenciaNombrePlantillaOrden(ByVal pstrNombrePlantilla As String, ByVal pviewNombrePlantillas As NombrePlantillaOYDPLUSView)
        Try
            viewNombrePlantilla = Nothing
            viewNombrePlantilla = pviewNombrePlantillas

            IsBusyPlantilla = True

            dcProxyPlantilla.OYDPLUS_OrdenesPlantillasVerificarNombre(pstrNombrePlantilla, Modulo, Program.Usuario, Program.HashConexion, AddressOf TerminoVerificarNombrePlantilla, pstrNombrePlantilla)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preguntar sí se deseaba duplicar la orden", Me.ToString(), "ValidarExistenciaNombrePlantillaOrden", Program.TituloSistema, Program.Maquina, ex)
            IsBusyPlantilla = False
        End Try
    End Sub

    Public Sub ConsultarPlantillasOrden()
        Try
            IsBusyPlantilla = True

            If IsNothing(FiltroPlantillas) Then
                FiltroPlantillas = String.Empty
            End If

            If Not IsNothing(dcProxyPlantilla.OrdenOYDPLUSOFs) Then
                dcProxyPlantilla.OrdenOYDPLUSOFs.Clear()
            End If
            dcProxyPlantilla.Load(dcProxyPlantilla.OYDPLUS_OrdenesPlantillasConsultarQuery(_FiltroPlantillas, Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPlantillas, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los parametros del receptor.",
                                 Me.ToString(), "ConsultarPlantillasOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyPlantilla = False
        End Try
    End Sub

    Public Sub EliminarPlantillasOrden()
        Try
            Dim strPlantillasEliminar As String = String.Empty

            For Each li In _ListaPlantillas
                If li.Seleccionada Then
                    If String.IsNullOrEmpty(strPlantillasEliminar) Then
                        strPlantillasEliminar = li.IDNroOrden.ToString
                    Else
                        strPlantillasEliminar = String.Format("{0}|{1}", strPlantillasEliminar, li.IDNroOrden)
                    End If
                End If
            Next

            If Not String.IsNullOrEmpty(strPlantillasEliminar) Then
                IsBusyPlantilla = True
                If Not IsNothing(dcProxyPlantilla.tblRespuestaValidaciones) Then
                    dcProxyPlantilla.tblRespuestaValidaciones.Clear()
                End If
                dcProxyPlantilla.Load(dcProxyPlantilla.OYDPLUS_OrdenesPlantillasEliminarQuery(strPlantillasEliminar, Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoEliminarPlantillas, String.Empty)
            Else
                mostrarMensaje("No se ha seleccionado ninguna plantilla para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los parametros del receptor.",
                                 Me.ToString(), "EliminarPlantillasOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyPlantilla = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para duplicar la orden.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 22 de marzo del 2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Sub DuplicarOrden()
        Try
            OrdenAnteriorOYDPLUS = Nothing

            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)
            dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
            mdtmFechaCierreSistema = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaCierreSistema(MSTR_MODULO_OYD_ORDENES, Program.Usuario)

            'Crea la nueva orden para duplicar.
            Dim objNewOrdenDuplicar As New OyDPLUSOrdenesOF.OrdenOYDPLUSOF
            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, objNewOrdenDuplicar)

            objNewOrdenDuplicar.IDNroOrden = 0
            objNewOrdenDuplicar.NroOrden = 0
            objNewOrdenDuplicar.FechaOrden = dtmFechaServidor
            objNewOrdenDuplicar.FechaActualizacion = dtmFechaServidor
            objNewOrdenDuplicar.UsuarioOperador = String.Empty
            objNewOrdenDuplicar.CanalRecepcion = String.Empty
            objNewOrdenDuplicar.MedioVerificable = String.Empty
            objNewOrdenDuplicar.FechaRecepcion = dtmFechaServidor
            objNewOrdenDuplicar.NroExtensionToma = String.Empty
            objNewOrdenDuplicar.EstadoLEO = String.Empty
            objNewOrdenDuplicar.NombreEstadoLEO = String.Empty
            objNewOrdenDuplicar.EstadoOrden = String.Empty
            objNewOrdenDuplicar.NombreEstadoOrden = String.Empty
            objNewOrdenDuplicar.OrdenCruzada = False
            objNewOrdenDuplicar.OrdenCruzadaCliente = False
            objNewOrdenDuplicar.OrdenCruzadaReceptor = False
            objNewOrdenDuplicar.IDNroOrdenOriginal = Nothing
            objNewOrdenDuplicar.IDOrdenOriginal = Nothing
            objNewOrdenDuplicar.NroOrdenSAE = Nothing
            objNewOrdenDuplicar.EstadoSAE = Nothing
            objNewOrdenDuplicar.NombreEstadoSAE = Nothing
            objNewOrdenDuplicar.Notas = String.Empty
            objNewOrdenDuplicar.Usuario = Program.Usuario

            'objOrden.UsuarioOperador=ConfiguracionReceptor.
            If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
                If DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Count > 0 Then
                    objNewOrdenDuplicar.EstadoOrden = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Retorno
                    objNewOrdenDuplicar.NombreEstadoOrden = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Descripcion
                End If

                If DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Count > 0 Then
                    objNewOrdenDuplicar.EstadoLEO = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Retorno
                    objNewOrdenDuplicar.NombreEstadoLEO = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Descripcion
                End If
            End If

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = True
            logNuevoRegistro = False
            logCambiarDetallesOrden = True
            HabilitarUsuarioOperador = True

            ObtenerValoresOrdenAnterior(objNewOrdenDuplicar, OrdenDuplicarOYDPLUS)

            Dim objNuevaListaDistribucion As New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
            For Each li In ListaReceptoresOrdenes
                objNuevaListaDistribucion.Add(li)
            Next
            ListaDistribucionComisionSalvar = objNuevaListaDistribucion
            CargarTipoNegocioReceptor(OPCION_DUPLICAR, _OrdenOYDPLUSSelected.Receptor, _Modulo, OPCION_DUPLICAR)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro duplicado", Me.ToString(), "DuplicarOrden", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            HabilitarEncabezado = False
            HabilitarOpcionesCruzada = False
            HabilitarNegocio = False
            HabilitarDuracion = False
            MostrarNegocio = Visibility.Visible
            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)
        End Try
    End Sub

    Public Async Sub PlantillaOrden()
        Try
            OrdenAnteriorOYDPLUS = Nothing

            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)
            dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
            mdtmFechaCierreSistema = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaCierreSistema(MSTR_MODULO_OYD_ORDENES, Program.Usuario)

            'Crea la nueva orden para duplicar.
            Dim objNewOrdenPlantilla As New OyDPLUSOrdenesOF.OrdenOYDPLUSOF
            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, objNewOrdenPlantilla)

            objNewOrdenPlantilla.IDNroOrden = 0
            objNewOrdenPlantilla.NroOrden = 0
            objNewOrdenPlantilla.FechaOrden = dtmFechaServidor
            objNewOrdenPlantilla.FechaActualizacion = dtmFechaServidor
            objNewOrdenPlantilla.EstadoLEO = String.Empty
            objNewOrdenPlantilla.NombreEstadoLEO = String.Empty
            objNewOrdenPlantilla.EstadoOrden = String.Empty
            objNewOrdenPlantilla.NombreEstadoOrden = String.Empty
            objNewOrdenPlantilla.IDNroOrdenOriginal = Nothing
            objNewOrdenPlantilla.IDOrdenOriginal = Nothing
            objNewOrdenPlantilla.NroOrdenSAE = Nothing
            objNewOrdenPlantilla.EstadoSAE = Nothing
            objNewOrdenPlantilla.NombreEstadoSAE = Nothing
            objNewOrdenPlantilla.Usuario = Program.Usuario

            'objOrden.UsuarioOperador=ConfiguracionReceptor.
            If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
                If DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Count > 0 Then
                    objNewOrdenPlantilla.EstadoOrden = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Retorno
                    objNewOrdenPlantilla.NombreEstadoOrden = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Descripcion
                End If

                If DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Count > 0 Then
                    objNewOrdenPlantilla.EstadoLEO = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Retorno
                    objNewOrdenPlantilla.NombreEstadoLEO = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Descripcion
                End If
            End If

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logPlantillaRegistro = True
            logNuevoRegistro = False
            logPlantillaRegistro = True
            logCambiarDetallesOrden = True
            HabilitarUsuarioOperador = True

            ObtenerValoresOrdenAnterior(objNewOrdenPlantilla, OrdenPlantillaOYDPLUS)

            CargarTipoNegocioReceptor(OPCION_PLANTILLA, _OrdenOYDPLUSSelected.Receptor, _Modulo, OPCION_PLANTILLA)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro plantilla", Me.ToString(), "PlantillaOrden", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            HabilitarEncabezado = False
            HabilitarOpcionesCruzada = False
            HabilitarNegocio = False
            HabilitarDuracion = False
            MostrarNegocio = Visibility.Visible
            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)
        End Try
    End Sub

    Public Async Sub CrearOrdenAPartirPlantilla(ByVal viewPlantilla As ConsultaPlantillasView)
        Try
            If logCargoForma = False Then
                ViewOrdenesOYDPLUS.GridEdicion.Children.Add(viewFormaOrdenes)
                logCargoForma = True
            End If

            Dim intCantidadSeleccionada As Integer = 0

            dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
            mdtmFechaCierreSistema = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaCierreSistema(MSTR_MODULO_OYD_ORDENES, Program.Usuario)

            For Each li In _ListaPlantillas
                If li.Seleccionada Then
                    intCantidadSeleccionada += 1
                    _PlantillaSeleccionada = li
                End If
            Next

            If intCantidadSeleccionada = 0 Then
                mostrarMensaje("Debe de seleccionar una plantilla.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If intCantidadSeleccionada > 1 Then
                mostrarMensaje("Debe de seleccionar solo una plantilla.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            viewPlantilla.DialogResult = True
            IsBusy = True

            OrdenAnteriorOYDPLUS = Nothing

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)
            End If

            'Crea la nueva orden para duplicar.
            Dim objNewOrdenPlantilla As New OyDPLUSOrdenesOF.OrdenOYDPLUSOF
            ObtenerValoresOrdenAnterior(_PlantillaSeleccionada, objNewOrdenPlantilla)

            objNewOrdenPlantilla.IDNroOrden = 0
            objNewOrdenPlantilla.NroOrden = 0
            objNewOrdenPlantilla.FechaOrden = dtmFechaServidor
            objNewOrdenPlantilla.FechaActualizacion = dtmFechaServidor
            objNewOrdenPlantilla.EstadoLEO = String.Empty
            objNewOrdenPlantilla.NombreEstadoLEO = String.Empty
            objNewOrdenPlantilla.EstadoOrden = String.Empty
            objNewOrdenPlantilla.NombreEstadoOrden = String.Empty
            objNewOrdenPlantilla.IDNroOrdenOriginal = Nothing
            objNewOrdenPlantilla.IDOrdenOriginal = Nothing
            objNewOrdenPlantilla.NroOrdenSAE = Nothing
            objNewOrdenPlantilla.EstadoSAE = Nothing
            objNewOrdenPlantilla.NombreEstadoSAE = Nothing
            objNewOrdenPlantilla.Usuario = Program.Usuario

            'objOrden.UsuarioOperador=ConfiguracionReceptor.
            If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
                If DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Count > 0 Then
                    objNewOrdenPlantilla.EstadoOrden = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Retorno
                    objNewOrdenPlantilla.NombreEstadoOrden = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN").Where(Function(i) i.Retorno = ESTADOORDEN_PENDIENTE).FirstOrDefault.Descripcion
                End If

                If DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Count > 0 Then
                    objNewOrdenPlantilla.EstadoLEO = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Retorno
                    objNewOrdenPlantilla.NombreEstadoLEO = DiccionarioCombosOYDPlusCompleta("O_ESTADOS_ORDEN_LEO").Where(Function(i) i.Retorno = ESTADOORDENLEO_RECIBIDA).FirstOrDefault.Descripcion
                End If
            End If

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logNuevoRegistro = False
            logPlantillaRegistro = True
            logCambiarDetallesOrden = True
            HabilitarUsuarioOperador = True

            ObtenerValoresOrdenAnterior(objNewOrdenPlantilla, OrdenPlantillaOYDPLUS)

            Dim objNuevaListaDistribucion As New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
            If Not IsNothing(ListaReceptoresOrdenes) Then
                For Each li In ListaReceptoresOrdenes
                    objNuevaListaDistribucion.Add(li)
                Next
            End If


            CargarTipoNegocioReceptor(OPCION_CREARORDENPLANTILLA, _OrdenPlantillaOYDPLUS.Receptor, _Modulo, OPCION_CREARORDENPLANTILLA)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización al crear la plantilla", Me.ToString(), "CrearOrdenAPartirPlantilla", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            HabilitarEncabezado = False
            HabilitarOpcionesCruzada = False
            HabilitarNegocio = False
            HabilitarDuracion = False
            MostrarNegocio = Visibility.Visible
            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.OrdenOYDPLUSOFs) Then
                dcProxyConsulta.OrdenOYDPLUSOFs.Clear()
            End If

            'Se sabe si se utiliza en las ordenes por aprobar o en las ordenes aprobadas.
            Dim strEstado As String = String.Empty

            Select Case VistaSeleccionada
                Case VISTA_APROBADAS
                    strEstado = "P"
                Case VISTA_PENDIENTESAPROBAR
                    strEstado = "D"
            End Select

            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                'Descripción: Agrega el filtro de modulo en la consulta - Por: Giovanny Velez Bolivar - 19/05/2014
                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenQuery(strEstado, TextoFiltroSeguro, _Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
            Else
                'Descripción: Agrega el filtro de modulo en la consulta - Por: Giovanny Velez Bolivar - 19/05/2014
                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenQuery(strEstado, String.Empty, _Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
                'If VistaSeleccionada <> VISTA_PENDIENTESCRUZAR Then
                '    dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenQuery(strEstado, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
                'Else
                '    FiltrarOrdenesCruzadasOYDPLUS("FILTRAR")
                'End If
            End If

            logCalcularValores = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Filtra los registros de OYDPLUS, cuando el filtro se encuentra vacio se llama el metodo
    ''' de Filtrar()
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <param name="pstrEstado"></param>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="pstrOpcion"></param>
    ''' <param name="pstrUserState"></param>
    ''' <remarks></remarks>
    Public Sub FiltrarRegistrosOYDPLUS(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrOpcion As String, Optional ByVal pstrUserState As String = "")
        Try
            If String.IsNullOrEmpty(pstrOpcion) Then
                Filtrar()
            Else
                IsBusy = True

                If Not IsNothing(dcProxyConsulta.OrdenOYDPLUSOFs) Then
                    dcProxyConsulta.OrdenOYDPLUSOFs.Clear()
                End If
                'Descripción: Agrega el filtro de modulo en la consulta - Por: Giovanny Velez Bolivar - 19/05/2014
                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenQuery(pstrEstado, pstrFiltro, _Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, pstrUserState)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar los registros.",
                     Me.ToString(), "FiltrarRegistrosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            OrganizarNuevaBusqueda()
            MyBase.Buscar()
            'MyBase.CambioItem("visBuscando")
            'MyBase.CambioItem("visNavegando")
            HabilitarEncabezado = False
            HabilitarOpcionesCruzada = False
            logCalcularValores = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación de la busqueda", Me.ToString(), "Buscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(BusquedaOrdenOyDPlus) Then
                IsBusy = True
                If Not IsNothing(dcProxyConsulta.OrdenOYDPLUSOFs) Then
                    dcProxyConsulta.OrdenOYDPLUSOFs.Clear()
                End If

                Dim strEstado As String = String.Empty

                Select Case VistaSeleccionada
                    Case VISTA_APROBADAS
                        strEstado = "P"
                    Case VISTA_PENDIENTESAPROBAR
                        strEstado = "D"
                End Select

                'Descripción: Permite validar el modulo de acuerdo al tipo de negocio - Por: Giovanny Velez Bolivar - 19/05/2014
                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_ConsultarOrdenQuery(Modulo, strEstado, BusquedaOrdenOyDPlus.NroOrden, BusquedaOrdenOyDPlus.Version, BusquedaOrdenOyDPlus.EstadoOrden, BusquedaOrdenOyDPlus.Receptor, BusquedaOrdenOyDPlus.TipoOrden,
                                                                 BusquedaOrdenOyDPlus.TipoNegocio, BusquedaOrdenOyDPlus.TipoOperacion, BusquedaOrdenOyDPlus.TipoProducto, String.Empty, Program.Usuario, Program.HashConexion),
                                                                 AddressOf TerminoTraerOrdenes, "BUSQUEDA")

                MyBase.ConfirmarBuscar()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConfirmarBuscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub ActualizarRegistro()
        Try
            If logPlantillaRegistro Then
                GuardarOrdenOYDPLUS(_OrdenOYDPLUSSelected)
            Else
                IsBusy = True
                dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
                mdtmFechaCierreSistema = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaCierreSistema(MSTR_MODULO_OYD_ORDENES, Program.Usuario)

                If ValidarFechaCierreSistema(_OrdenOYDPLUSSelected, "actualizar") Then
                    If logCalcularValores Then
                        Await CalcularValorOrden(_OrdenOYDPLUSSelected)
                    End If
                    LimpiarVariablesConfirmadas()
                    CalcularDiasOrdenOYDPLUS(MSTR_CALCULAR_DIAS_ORDEN, _OrdenOYDPLUSSelected, -1, True)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden.", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida sí el popup de ordenes cruzadas se encuentra activo.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 18 de marzo del 2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ValidarOrdenOriginaloCruzada()
        Try
            ActualizarOrdenOYDPLUS(_OrdenOYDPLUSSelected)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar sí es una orden original o cruzada.", Me.ToString(), "ValidarOrdenOriginaloCruzada", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cuando se pasan todas las validaciones, sí se requeria confirmación, justificación y aprobación
    ''' se llama este metodo y se realiza el envio de la orden a la base de datos con todos los parametros de la orden.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ActualizarOrdenOYDPLUS(ByVal objOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            IsBusy = True

            If Not IsNothing(objOrdenSelected) Then

                If ValidarGuardadoOrden(objOrdenSelected) Then
                    GuardarOrdenOYDPLUS(objOrdenSelected)
                Else
                    IsBusy = False
                    PosicionarControlValidaciones(True, objOrdenSelected, ViewOrdenesOYDPLUS)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden.", Me.ToString(), "ActualizarOrdenOYDPLUS", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ValidarMensajesMostrarUsuario(ByVal pobjTipoMensaje As TIPOMENSAJEUSUARIO, Optional ByVal pobjResultaUsuario As A2Utilidades.wppMensajePregunta = Nothing)
        Try
            If Not IsNothing(_ListaResultadoValidacion) Then
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
                    If ListaResultadoValidacion.Where(Function(i) i.RequiereConfirmacion = True).Count > 0 Then
                        cantidadTotalConfirmacion = 1
                    End If
                    cantidadTotalJustificacion = ListaResultadoValidacion.Where(Function(i) i.RequiereJustificacion = True).Count
                    CantidadTotalAprobaciones = ListaResultadoValidacion.Where(Function(i) i.RequiereAprobacion = True).Count
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

                    If ListaResultadoValidacion.Where(Function(i) i.RequiereConfirmacion = True).Count > 0 Then
                        cantidadTotalConfirmacion = 1
                    End If

                    For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereConfirmacion = True).ToList
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

                    For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereJustificacion = True).ToList
                        If Not Justificaciones.Contains(li.Confirmacion) Then
                            ListaJustificacion.Clear()

                            If Not String.IsNullOrEmpty(li.CausasJustificacion) Then
                                For Each item In li.CausasJustificacion.Split("|")
                                    ListaJustificacion.Add(New A2Utilidades.CausasJustificacionMensajePregunta With {.ID = item,
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


                    For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereAprobacion = True).ToList
                        If Not Aprobaciones.Contains(li.Confirmacion) Then
                            ListaJustificacion.Clear()

                            If Not String.IsNullOrEmpty(li.CausasJustificacion) Then
                                For Each item In li.CausasJustificacion.Split("|")
                                    ListaJustificacion.Add(New A2Utilidades.CausasJustificacionMensajePregunta With {.ID = item,
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

    Private Sub GuardarOrdenOYDPLUS(ByVal objOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            Dim strReceptores As String
            Dim lstReceptores = New List(Of String)
            Dim lstReceptoresRep = New List(Of Object)

            IsBusy = True

            strReceptores = "<receptores>"
            For Each obj In _ListaReceptoresOrdenes
                If Not lstReceptores.Contains(obj.IDReceptor) And obj.Porcentaje > 0 Then
                    lstReceptores.Add(obj.IDReceptor)
                    strReceptores &= "<receptor Id=""" & obj.IDReceptor & """ Lider=""" & IIf(obj.Lider, "1", "0") & """ Porcentaje=""" & (obj.Porcentaje).ToString() & """ />"
                Else
                    lstReceptoresRep.Add(obj)
                End If
            Next
            strReceptores &= "</receptores>"

            objOrdenSelected.ReceptoresXML = strReceptores

            If Not String.IsNullOrEmpty(objOrdenSelected.ReferenciaBolsa) And objOrdenSelected.Clase = CLASE_ACCIONES Then
                objOrdenSelected.LiqAsociadasXML = "<liqprobables><liqprobable Id=""" & objOrdenSelected.ReferenciaBolsa.ToString() & """ Parcial=""-1"" FechaLiq=""" & objOrdenSelected.FechaReferenciaBolsa.Value.ToString("yyyy-MM-dd hh:mm:ss") & """ /></liqprobables>"
            Else
                objOrdenSelected.LiqAsociadasXML = "<liqprobables><liqprobable Id=""0"" Parcial=""-1"" FechaLiq=""" & dtmFechaServidor.ToString("yyyy-MM-dd hh:mm:ss") & """ /></liqprobables>"
            End If

            objOrdenSelected.InstruccionesOrdenesXML = "<instrucciones></instrucciones>"
            objOrdenSelected.PagosOrdenesXML = String.Empty
            objOrdenSelected.ComisionesOrdenesXML = String.Empty

            Dim strReceptoresOrdenesCruzada As String

            strReceptoresOrdenesCruzada = "<receptores>"

            strReceptoresOrdenesCruzada &= "</receptores>"

            objOrdenSelected.ReceptoresCruzadasXML = strReceptoresOrdenesCruzada

            Dim strXMLReceptores As String = System.Web.HttpUtility.HtmlEncode(objOrdenSelected.ReceptoresXML)
            Dim strXMLLiquidaciones As String = System.Web.HttpUtility.HtmlEncode(objOrdenSelected.LiqAsociadasXML)
            Dim strXMLPagosOrdenes As String = System.Web.HttpUtility.HtmlEncode(objOrdenSelected.PagosOrdenesXML)
            Dim strXMLInstrucciones As String = System.Web.HttpUtility.HtmlEncode(objOrdenSelected.InstruccionesOrdenesXML)
            Dim strXMLComisionesOrdenes As String = System.Web.HttpUtility.HtmlEncode(objOrdenSelected.ComisionesOrdenesXML)
            Dim strXMLReceptoresOrdenesCruzadas As String = System.Web.HttpUtility.HtmlEncode(objOrdenSelected.ReceptoresCruzadasXML)

            If logNuevoRegistro Then
                objOrdenSelected.FechaOrden = dtmFechaServidor
            End If

            If String.IsNullOrEmpty(objOrdenSelected.Bolsa) Then
                If Not IsNothing(DiccionarioCombosOYDPlus) Then
                    If DiccionarioCombosOYDPlus("BOLSA").Count > 0 Then
                        ' EOMC -- 11/20/2012
                        ' Retorno identifica la bolsa, se estaba asignando el id de la tabla de combosReceptor
                        objOrdenSelected.Bolsa = DiccionarioCombosOYDPlus("BOLSA").FirstOrDefault.Retorno
                        objOrdenSelected.NombreBolsa = DiccionarioCombosOYDPlus("BOLSA").FirstOrDefault.Descripcion
                    End If
                End If
            End If

            If logPlantillaRegistro = False Then
                strNombrePlantilla = String.Empty
            End If

            If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                dcProxy.tblRespuestaValidaciones.Clear()
            End If

            dcProxy.Load(dcProxy.OYDPLUS_ValidarIngresoOrdenQuery(objOrdenSelected.IDNroOrden, objOrdenSelected.NroOrden, objOrdenSelected.Bolsa, objOrdenSelected.Receptor,
                                                                 objOrdenSelected.TipoOrden, objOrdenSelected.TipoNegocio, objOrdenSelected.TipoProducto,
                                                                 objOrdenSelected.TipoOperacion, objOrdenSelected.Clase, objOrdenSelected.FechaOrden, objOrdenSelected.EstadoOrden, objOrdenSelected.EstadoLEO,
                                                                 objOrdenSelected.Clasificacion, objOrdenSelected.TipoLimite, objOrdenSelected.Duracion, objOrdenSelected.FechaVigencia, objOrdenSelected.HoraVigencia, objOrdenSelected.Dias,
                                                                 objOrdenSelected.CondicionesNegociacion, objOrdenSelected.FormaPago, objOrdenSelected.TipoInversion,
                                                                 objOrdenSelected.Ejecucion, objOrdenSelected.Mercado, objOrdenSelected.IDComitente, objOrdenSelected.IDOrdenante,
                                                                 objOrdenSelected.UBICACIONTITULO, objOrdenSelected.CuentaDeposito, objOrdenSelected.UsuarioOperador,
                                                                 objOrdenSelected.CanalRecepcion, objOrdenSelected.MedioVerificable, objOrdenSelected.FechaRecepcion, objOrdenSelected.NroExtensionToma,
                                                                 objOrdenSelected.Especie, objOrdenSelected.ISIN, objOrdenSelected.FechaEmision, objOrdenSelected.FechaVencimiento, objOrdenSelected.Estandarizada,
                                                                 objOrdenSelected.FechaCumplimiento, objOrdenSelected.TasaFacial, objOrdenSelected.Modalidad, objOrdenSelected.Indicador, objOrdenSelected.PuntosIndicador,
                                                                 objOrdenSelected.EnPesos, objOrdenSelected.Cantidad, objOrdenSelected.Precio, objOrdenSelected.PrecioMaximoMinimo, objOrdenSelected.ValorCaptacionGiro,
                                                                 objOrdenSelected.ValorFuturoRepo, objOrdenSelected.TasaRegistro, objOrdenSelected.TasaCliente, objOrdenSelected.TasaNominal,
                                                                 objOrdenSelected.Castigo, objOrdenSelected.ValorAccion, objOrdenSelected.Comision, objOrdenSelected.ValorComision, objOrdenSelected.ValorOrden, objOrdenSelected.DiasRepo,
                                                                 objOrdenSelected.ProductoValores, objOrdenSelected.CostosAdicionales, objOrdenSelected.Instrucciones, objOrdenSelected.Notas, strXMLReceptores, strXMLLiquidaciones,
                                                                 strXMLPagosOrdenes, strXMLInstrucciones, strXMLComisionesOrdenes, Confirmaciones, ConfirmacionesUsuario, Justificaciones, JustificacionesUsuario, Aprobaciones, AprobacionesUsuario,
                                                                 objOrdenSelected.Custodia, objOrdenSelected.CustodiaSecuencia, objOrdenSelected.DiasCumplimiento,
                                                                 objOrdenSelected.RuedaNegocio, objOrdenSelected.PrecioLimpio, objOrdenSelected.EstadoTitulo, objOrdenSelected.IvaComision, objOrdenSelected.ValorFuturoCliente,
                                                                 strXMLReceptoresOrdenesCruzadas, objOrdenSelected.OrdenCruzada, objOrdenSelected.OrdenCruzadaCliente, objOrdenSelected.OrdenCruzadaReceptor, objOrdenSelected.IDOrdenOriginal,
                                                                 logPlantillaRegistro, strNombrePlantilla, objOrdenSelected.BrokenTrader, objOrdenSelected.Entidad, objOrdenSelected.Estrategia, Program.Maquina, Program.Usuario, Program.UsuarioWindows, Program.HashConexion),
                                                                 AddressOf TerminoValidarIngresoOrden, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de guardar la orden.", Me.ToString(), "GuardarOrdenOYDPLUS", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

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

    ''' <summary>
    ''' Se esta utilizando en la aplicación para que el menu de los controles se ponga en estado de navegación.
    ''' En este se recarga la pantalla de Ordenes.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <param name="so"></param>
    ''' <remarks></remarks>
    Overrides Sub TerminoSubmitChanges(ByVal so As SubmitOperation)
        Try
            MyBase.TerminoSubmitChanges(so)

            mostrarMensaje(so.UserState.ToString, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Exito)

            ObtenerInformacionCombosCompletos()
            'Consultar los precios del mercado para activar el ticker.
            CargarMensajeDinamicoOYDPLUS("PRECIOSMERCADO", String.Empty, String.Empty, String.Empty)

            intIDOrdenTimer = _OrdenOYDPLUSSelected.IDNroOrden
            logRefrescarconsultaCambioTab = False

            If CantidadAprobaciones > 0 Then
                VistaSeleccionada = VISTA_PENDIENTESAPROBAR
                FiltrarRegistrosOYDPLUS("D", String.Empty, "TERMINOGUARDAR", "TERMINOGUARDARPENDIENTES")
                'VistaSeleccionada = VISTA_PENDIENTESAPROBAR
            Else
                VistaSeleccionada = VISTA_APROBADAS
                If intIDOrdenTimer = 0 Then
                    FiltrarRegistrosOYDPLUS("P", String.Empty, "TERMINOGUARDAR", "TERMINOGUARDARNUEVO")
                Else
                    FiltrarRegistrosOYDPLUS("P", String.Empty, "TERMINOGUARDAR", "TERMINOGUARDAREDICION")
                End If
                'VistaSeleccionada = VISTA_APROBADAS

            End If

            If BorrarEspecie = True Then
                BorrarEspecie = False
            End If

            If BorrarCliente = True Then
                BorrarCliente = False
            End If

            BorrarCliente = True
            BorrarEspecie = True

            OrdenAnterior = Nothing
            OrdenAnteriorOYDPLUS = Nothing
            OrdenDuplicarOYDPLUS = Nothing

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden.", Me.ToString(), "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub EditarRegistro()
        Try
            If logCargoForma = False Then
                ViewOrdenesOYDPLUS.GridEdicion.Children.Add(viewFormaOrdenes)
                logCargoForma = True
            End If

            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If _OrdenOYDPLUSSelected.IDNroOrden <> 0 Then
                    IsBusy = True

                    dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
                    mdtmFechaCierreSistema = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaCierreSistema(MSTR_MODULO_OYD_ORDENES, Program.Usuario)

                    MostrarNegocio = Visibility.Visible
                    MostrarControles = Visibility.Visible
                    MostrarNoEdicion = Visibility.Collapsed
                    validarEstadoOrden("EDITARORDENOYDPLUS")
                    logCalcularValores = True
                Else
                    MyBase.RetornarValorEdicionNavegacion()
                    mostrarMensaje("No se ha seleccionado ninguna orden para editar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Me.IsBusy = False
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
                mostrarMensaje("No se ha seleccionado ninguna orden para editar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.IsBusy = False
            End If
        Catch ex As Exception
            Me.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar editar la orden.", Me.ToString(), "EditarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para editar la orden de OYDPLUS.
    ''' Despues de realizar las validaciones necesarias que permitan la edición de la orden de oydplus.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    Private Sub EditarOrdenOYDPLUS()
        Try
            OrdenAnteriorOYDPLUS = Nothing

            If _OrdenOYDPLUSSelected.FechaVigencia <= dtmFechaServidor Then
                CalcularFechaVigenciaOrden(_OrdenOYDPLUSSelected)
            End If

            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logNuevoRegistro = False
            logCambiarDetallesOrden = True

            HabilitarUsuarioOperador = Not (_OrdenOYDPLUSSelected.EstadoLEO = ESTADOORDENLEO_LANZADA)

            ObtenerReceptorLiderOrdenOYDPLUS()

            CargarCombosOYDPLUS(OPCION_EDITAR, _OrdenOYDPLUSSelected.Receptor, OPCION_EDITAR)

            BuscarControlValidacion(ViewOrdenesOYDPLUS, "tabItemValoresComisiones")

            Dim Mensaje As String = String.Format("Se modifica la orden nro {0}", _OrdenOYDPLUSSelected.IDNroOrden)

#If HAY_NOTIFICACIONES = 1 Then
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.Modificada, Mensaje, _OrdenOYDPLUSSelected.IDNroOrden, String.Empty)
#End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar la orden.", Me.ToString(), "EditarOrdenOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para validar el estado actual de la orden y realiza una consulta en base de datos para validar 
    ''' que la orden no se encuentre complementada.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    Private Sub validarEstadoOrden(ByVal pstrAccion As String)
        Try
            Dim strMsg As String = String.Empty

            If _OrdenOYDPLUSSelected.Modificable = False Then
                If pstrAccion = "EDITARORDENOYDPLUS" Then
                    strMsg = "Solamente las órdenes en estado pendiente pueden ser modificadas"
                Else
                    strMsg = "Solamente las órdenes en estado pendiente pueden ser anuladas"
                End If

                MyBase.RetornarValorEdicionNavegacion()
                mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'ElseIf _OrdenOYDPLUSSelected.EstadoLEO <> ESTADOORDENLEO_RECIBIDA And pstrAccion = OPCION_EDITAR Then
                '    strMsg = "Solamente las órdenes en estado LEO recibida pueden ser modificadas"

                '    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.IsBusy = False
            Else
                Select Case _OrdenOYDPLUSSelected.TipoNegocio
                    Case TIPONEGOCIO_SIMULTANEA 'SIMULTANEA
                        If _OrdenOYDPLUSSelected.Clasificacion = RF_Simulatena_Regreso Then
                            strMsg = "La Orden No. " + _OrdenOYDPLUSSelected.NroOrden.ToString + " es una Simultanea de Regreso no puede ser modificada. Modifique la Orden No. " + (_OrdenOYDPLUSSelected.NroOrden - 1).ToString + " que es la correspondiente Simúltanea de Salida."
                        End If
                    Case TIPONEGOCIO_REPO 'REPO
                        If _OrdenOYDPLUSSelected.Clasificacion = RF_REPO And (_OrdenOYDPLUSSelected.TipoOperacion = TIPONEGOCIO_REPO Or _OrdenOYDPLUSSelected.TipoOperacion = TIPONEGOCIO_SIMULTANEA) Then
                            strMsg = "La Orden No. " + _OrdenOYDPLUSSelected.NroOrden.ToString + " es una REPO de Regreso no puede ser modificada. Modifique la Orden No. " + (_OrdenOYDPLUSSelected.NroOrden - 1).ToString + " que es la correspondiente REPO de Salida."
                        End If
                    Case TIPONEGOCIO_TTV 'TTV
                        If _OrdenOYDPLUSSelected.Clasificacion = RF_TTV_Regreso Then
                            strMsg = "La Orden No. " + _OrdenOYDPLUSSelected.NroOrden.ToString + " es una TTV de Regreso no puede ser modificada. Modifique la Orden No. " + (_OrdenOYDPLUSSelected.NroOrden - 1).ToString + " que es la correspondiente TTV de Salida."
                        End If
                End Select

                'Se quita la validación porque las ordenes si se pueden modificar pero el usuario tiene que realizar la modificación de la fecha de vigencia.
                'If _OrdenOYDPLUSSelected.FechaVigencia < dtmFechaServidor Then
                '    strMsg = "La fecha de vigencia de la Orden No. " + _OrdenOYDPLUSSelected.NroOrden.ToString + " es menor a la fecha actual."
                'End If

                If strMsg.Equals(String.Empty) Then
                    Dim strAccionMensaje As String = IIf(pstrAccion = "EDITARORDENOYDPLUS", "Editar", "Anular")
                    If ValidarFechaCierreSistema(_OrdenOYDPLUSSelected, strAccionMensaje) Then
                        Me.IsBusy = True
                        validarOrdenModificable(pstrAccion)
                    Else
                        MyBase.RetornarValorEdicionNavegacion()
                        Me.IsBusy = False
                    End If
                Else
                    MyBase.RetornarValorEdicionNavegacion()
                    IsBusy = False
                    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            Me.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el estado de la orden.", Me.ToString(), "validarEstadoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Valida el estado de la orden en el servidor
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <param name="pstrAccion">Indica si se valida la edición o anulación de la orden</param>
    Private Sub validarOrdenModificable(ByVal pstrAccion As String)
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not IsNothing(dcProxy.OrdenModificables) Then
                    dcProxy.OrdenModificables.Clear()
                End If

                dcProxy.Load(dcProxy.verificarOrdenModificableQuery(_OrdenOYDPLUSSelected.Clase, _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.NroOrden, _OrdenOYDPLUSSelected.Version, MSTR_MODULO_OYD_ORDENES, pstrAccion, Program.Usuario, _OrdenOYDPLUSSelected.TipoNegocio, Program.HashConexion), AddressOf TerminoVerificarOrdenModificable, pstrAccion)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la validación del estado de la orden", Me.ToString(), "ValidarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                IsBusy = True
                dcProxy1.OYDPLUS_CancelarOrdenOYDPLUS(_OrdenOYDPLUSSelected.NroOrden, "ORDENES", Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCancelarEditarRegistro(ByVal lo As InvokeOperation(Of Integer))
        Try
            logCancelarRegistro = True
            logEditarRegistro = False
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logNuevoRegistro = False
            HabilitarOpcionesCruzada = False
            HabilitarEncabezado = False
            HabilitarNegocio = False
            HabilitarEjecucion = False
            HabilitarDuracion = False
            HabilitarFechaVigencia = False
            HabilitarHoraVigencia = False
            HabilitarUsuarioOperador = False

            MostrarControles = Visibility.Collapsed
            MostrarControlMensajes = Visibility.Collapsed
            MostrarCamposAcciones = Visibility.Collapsed
            MostrarCamposRentaFija = Visibility.Collapsed
            MostrarCamposCuentaPropia = Visibility.Collapsed
            MostrarNoEdicion = Visibility.Visible
            MostrarCamposFaciales = Visibility.Collapsed
            MostrarAdicionalesEspecie = Visibility.Collapsed

            logCambiarDetallesOrden = True

            If BorrarEspecie = True Then
                BorrarEspecie = False
            End If

            If BorrarCliente = True Then
                BorrarCliente = False
            End If

            BorrarCliente = True
            BorrarEspecie = True

            ObtenerInformacionCombosCompletos()

            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)

            ObtenerValoresOrdenEnLista(_OrdenOYDPLUSSelected)

            OrdenAnteriorOYDPLUS = Nothing
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                MostrarNegocio = Visibility.Visible
            Else
                MostrarNegocio = Visibility.Collapsed
            End If

            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)

            logCalcularValores = False

            'Consultar los precios del mercado para activar el ticker.
            CargarMensajeDinamicoOYDPLUS("PRECIOSMERCADO", String.Empty, String.Empty, String.Empty)
            IsBusy = False

            Editando = False
            dcProxy.RejectChanges()
            MyBase.CambioItem("Editando")
            MyBase.CancelarEditarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "TerminoCancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCancelarAnularRegistro(ByVal lo As InvokeOperation(Of Integer))
        Try
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la anulación del registro",
                     Me.ToString(), "TerminoCancelarAnularRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Async Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If _OrdenOYDPLUSSelected.IDNroOrden <> 0 Then
                    IsBusy = True

                    dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
                    mdtmFechaCierreSistema = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaCierreSistema(MSTR_MODULO_OYD_ORDENES, Program.Usuario)

                    validarEstadoOrden("ANULARORDENOYDPLUS")
                Else
                    mostrarMensaje("No se ha seleccionado ninguna orden para eliminar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                mostrarMensaje("No se ha seleccionado ninguna orden para eliminar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular la orden", Me.ToString(), "BorrarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOrden"
                    Dim NewReceptoresOrden As New OyDPLUSOrdenesOF.ReceptoresOrden

                    NewReceptoresOrden.ClaseOrden = _OrdenOYDPLUSSelected.Clase
                    NewReceptoresOrden.TipoOrden = _OrdenOYDPLUSSelected.TipoOperacion
                    NewReceptoresOrden.NroOrden = _OrdenOYDPLUSSelected.NroOrden
                    NewReceptoresOrden.Version = _OrdenOYDPLUSSelected.Version
                    NewReceptoresOrden.IDComisionista = 0
                    NewReceptoresOrden.IDSucComisionista = 0
                    NewReceptoresOrden.FechaActualizacion = dtmFechaServidor
                    NewReceptoresOrden.Usuario = Program.Usuario
                    NewReceptoresOrden.Lider = False
                    NewReceptoresOrden.Porcentaje = 0
                    NewReceptoresOrden.Nombre = ""
                    NewReceptoresOrden.IDReceptor = String.Empty

                    Dim objListaReceptorOrden As New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
                    objListaReceptorOrden = _ListaReceptoresOrdenes

                    If IsNothing(objListaReceptorOrden) Then
                        objListaReceptorOrden = New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
                    End If

                    objListaReceptorOrden.Add(NewReceptoresOrden)

                    ListaReceptoresOrdenes = objListaReceptorOrden
                    ReceptoresOrdenSelected = NewReceptoresOrden

                    MyBase.CambioItem("ListaReceptoresOrdenes")
                    MyBase.CambioItem("ListaReceptoresOrdenesPaged")
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo detalle.", Me.ToString(), "NuevoRegistroDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOrden"
                    If Not IsNothing(_ListaReceptoresOrdenes) Then
                        If Not _ReceptoresOrdenSelected Is Nothing Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresOrdenSelected, ListaReceptoresOrdenes)
                            Dim objListaReceptoresOrden As New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
                            For Each li In _ListaReceptoresOrdenes
                                objListaReceptoresOrden.Add(li)
                            Next

                            If objListaReceptoresOrden.Contains(_ReceptoresOrdenSelected) Then
                                objListaReceptoresOrden.Remove(_ReceptoresOrdenSelected)
                            End If

                            ReceptoresOrdenSelected = Nothing
                            ListaReceptoresOrdenes = objListaReceptoresOrden
                            Program.PosicionarItemLista(ReceptoresOrdenSelected, ListaReceptoresOrdenes, intRegistroPosicionar)
                        End If
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.", Me.ToString(), "BorrarRegistroDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CambiarAForma()
        Try
            If logCargoForma = False Then
                ViewOrdenesOYDPLUS.GridEdicion.Children.Add(viewFormaOrdenes)
                logCargoForma = True
            End If
            MyBase.CambiarAForma()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ver el detalle del registro.", Me.ToString(), "CambiarAForma", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CambiarALista()
        Try
            MyBase.CambiarALista()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ver los registros.", Me.ToString(), "CambiarALista", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Este metodo limpia las propiedades de busqueda que hallan sido ingresado anteriormente.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OrganizarNuevaBusqueda()
        Try
            Dim objBusqueda As New CamposBusquedaOrdenOYDPLUS
            objBusqueda.NroOrden = 0
            objBusqueda.Receptor = String.Empty
            objBusqueda.TipoNegocio = String.Empty
            objBusqueda.TipoOperacion = String.Empty
            objBusqueda.TipoOrden = String.Empty
            objBusqueda.TipoProducto = String.Empty

            BusquedaOrdenOyDPlus = objBusqueda

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al organizar la nueva busqueda.", Me.ToString(), "OrganizarNuevaBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    '''Metodo para cargar los combos especificos para la pantalla de OYDPLUS.
    ''' Se realiza la consulta en base de datos dependiendo 
    ''' </summary>
    Public Sub CargarCombosOYDPLUS(ByVal pstrOpcion As String, ByVal pstrIDReceptor As String, Optional ByVal pstrUserState As String = "")
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                IsBusy = True
            End If

            If Not IsNothing(mdcProxyUtilidad03.CombosReceptors) Then
                mdcProxyUtilidad03.CombosReceptors.Clear()
            End If

            If String.IsNullOrEmpty(pstrOpcion) Then
                mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarCombosReceptorQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYDCOMPLETOS, pstrUserState)
            Else
                If pstrOpcion.ToUpper = "INICIO" Then
                    mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarCombosReceptorQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYDCOMPLETOS, pstrUserState)
                Else
                    mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarCombosReceptorQuery(pstrIDReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYD, pstrUserState)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.",
                                 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodod para cargar los tipos de negocio a los cuales tiene derecho el receptor.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 02 de Octubre del 2012
    ''' </summary>
    Public Sub CargarTipoNegocioReceptor(ByVal pstrOpcion As String, ByVal pstrReceptor As String, ByVal pstrModulo As String, Optional ByVal pstrUserState As String = "")
        Try
            If logEditarRegistro Or logNuevoRegistro Then
                IsBusy = True
            End If

            If Not IsNothing(mdcProxyUtilidad03.tblTipoNegocioReceptors) Then
                mdcProxyUtilidad03.tblTipoNegocioReceptors.Clear()
            End If

            If String.IsNullOrEmpty(pstrOpcion) Then
                mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_TipoNegocioReceptorQuery(String.Empty, pstrModulo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTipoNegocioOYDCOMPLETOS, pstrUserState)
            Else
                If pstrOpcion.ToUpper = "INICIO" Then
                    mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_TipoNegocioReceptorQuery(String.Empty, pstrModulo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTipoNegocioOYDCOMPLETOS, pstrUserState)
                Else
                    mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_TipoNegocioReceptorQuery(pstrReceptor, pstrModulo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTipoNegocioOYD, pstrUserState)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.",
                                 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    '''Metodo para cargar la configuración del receptor de la tabla de tblConfiguracionesAdicionalesReceptor OYDPLUS.
    '''Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub CargarConfiguracionReceptorOYDPLUS(ByVal pstrReceptor As String, ByVal pUserState As String)
        Try
            If logNuevoRegistro Then
                IsBusy = True
            End If
            If Not IsNothing(mdcProxyUtilidad03.tblConfiguracionesAdicionalesReceptors) Then
                mdcProxyUtilidad03.tblConfiguracionesAdicionalesReceptors.Clear()
            End If
            mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarConfiguracionReceptorQuery(pstrReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarConfiguracionReceptor, pUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar la configuración del receptor.",
                                 Me.ToString(), "CargarConfiguracionReceptor", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    '''Metodo para cargar los parametros por defecto que tiene el receptor en la tabla de tblParametrosReceptor OYDPLUS.
    '''Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub CargarParametrosReceptorOYDPLUS(ByVal pstrReceptor As String, ByVal pUserState As String)
        Try
            If logNuevoRegistro Then
                IsBusy = True
            End If
            If Not IsNothing(mdcProxyUtilidad03.tblParametrosReceptors) Then
                mdcProxyUtilidad03.tblParametrosReceptors.Clear()
            End If
            mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarParametrosReceptorQuery(pstrReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarParametrosReceptor, pUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los parametros del receptor.",
                                 Me.ToString(), "CargarParametrosReceptor", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    '''Metodo para cargar los mensajes para la pantalla
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub CargarMensajeDinamicoOYDPLUS(ByVal pstrOpcion As String, ByVal pstrIDReceptor As String, ByVal pstrIDCliente As String, ByVal pstrIDEspecie As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(mdcProxyUtilidad03.tblMensajes) Then
                mdcProxyUtilidad03.tblMensajes.Clear()
            End If
            mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarMensajePantallaQuery(pstrOpcion, pstrIDReceptor, pstrIDCliente, pstrIDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarMensaje, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar el mensaje dinamico.",
                                 Me.ToString(), "CargarMensajeDinamico", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para limpiar valores del control.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub LimpiarControlesOYDPLUS(ByVal pstrOpcion As String)
        Try
            Select Case pstrOpcion.ToUpper
                Case OPCION_RECEPTOR
                    _OrdenOYDPLUSSelected.TipoOrden = Nothing
                    _OrdenOYDPLUSSelected.TipoNegocio = Nothing
                    _OrdenOYDPLUSSelected.TipoProducto = Nothing
                    _OrdenOYDPLUSSelected.TipoOperacion = Nothing
                    _OrdenOYDPLUSSelected.TipoLimite = Nothing
                    _OrdenOYDPLUSSelected.Duracion = Nothing
                    _OrdenOYDPLUSSelected.CondicionesNegociacion = Nothing
                    _OrdenOYDPLUSSelected.FormaPago = Nothing
                    _OrdenOYDPLUSSelected.TipoInversion = Nothing
                    _OrdenOYDPLUSSelected.Ejecucion = Nothing
                    _OrdenOYDPLUSSelected.Mercado = Nothing
                    _OrdenOYDPLUSSelected.Modalidad = Nothing
                    _OrdenOYDPLUSSelected.CanalRecepcion = Nothing
                    _OrdenOYDPLUSSelected.UsuarioOperador = Nothing
                    _OrdenOYDPLUSSelected.MedioVerificable = Nothing
                    _OrdenOYDPLUSSelected.ProductoValores = Nothing

                    Me.ComitenteSeleccionadoOYDPLUS = Nothing

                    If Not IsNothing(Me.ListaOrdenantesOYDPLUS) Then
                        Me.ListaOrdenantesOYDPLUS.Clear()
                    End If

                    Me.OrdenanteSeleccionadoOYDPLUS = Nothing

                    If Not IsNothing(Me.ListaCuentasDepositoOYDPLUS) Then
                        Me.ListaCuentasDepositoOYDPLUS.Clear()
                    End If

                    Me.CtaDepositoSeleccionadaOYDPLUS = Nothing
                    DiccionarioCombosOYDPlus = Nothing
                Case OPCION_TIPONEGOCIO
                    _OrdenOYDPLUSSelected.TipoOperacion = Nothing
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al limpiar los controles.",
                                 Me.ToString(), "LimpiarControles", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Cargar los receptores activos del usuario logueado.
    ''' Cuando el parametro opción se encuentra vacio carga todos los receptores del usuario.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub CargarReceptoresUsuarioOYDPLUS(ByVal pstrOpcion As String, Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad03.tblReceptoresUsuarios) Then
                mdcProxyUtilidad03.tblReceptoresUsuarios.Clear()
            End If
            If String.IsNullOrEmpty(pstrOpcion) Then
                mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarReceptoresUsuarioQuery(False, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptoresUsuarioCOMPLETOS, pstrUserState)
            Else
                If pstrOpcion.ToUpper = "INICIO" Then
                    mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarReceptoresUsuarioQuery(False, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptoresUsuarioCOMPLETOS, pstrUserState)
                Else
                    mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarReceptoresUsuarioQuery(True, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptoresUsuario, pstrUserState)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar los receptores del usuario.",
                                 Me.ToString(), "CargarReceptoresUsuario", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Cargar la información del cliente de la orden.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub BuscarClienteOYDPLUS(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not String.IsNullOrEmpty(pstrIdComitente) Then
                If Not IsNothing(mdcProxyUtilidad01.BuscadorClientes) Then
                    mdcProxyUtilidad01.BuscadorClientes.Clear()
                End If

                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                If Not String.IsNullOrEmpty(strClienteABuscar) Then
                    mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(strClienteABuscar, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrUserState)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información del cliente.",
                                 Me.ToString(), "SeleccionarCliente", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub BuscarEspecieOYDPLUS(ByVal pstrMercado As String, ByVal pstrEspecie As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not String.IsNullOrEmpty(pstrEspecie) Then
                If Not IsNothing(mdcProxyUtilidad01.BuscadorEspecies) Then
                    mdcProxyUtilidad01.BuscadorEspecies.Clear()
                End If

                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery(pstrMercado, pstrEspecie, Program.Usuario, Program.HashConexion), AddressOf buscarEspecieCompleted, pstrUserState)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información de la especie.",
                                 Me.ToString(), "BuscarEspecieOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Seleccionar información del cliente seleccionado
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub CargarDatosClienteOYDPLUS(ByVal pComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pComitente) Then
                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                    _OrdenOYDPLUSSelected.CategoriaCliente = pComitente.Categoria
                    _OrdenOYDPLUSSelected.NroDocumento = pComitente.NroDocumento
                    _OrdenOYDPLUSSelected.TipoProducto = pComitente.CodTipoProducto
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información del cliente.",
                                 Me.ToString(), "SeleccionarCliente", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para tomar los valores por defecto de los controles.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub ObtenerValoresDefectoOYDPLUS(ByVal pstrOpcion As String, ByVal objOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                Select Case pstrOpcion.ToUpper
                    Case OPCION_RECEPTOR
                        If ListaReceptoresUsuario.Count > 1 Then
                            If ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).Count > 0 Then
                                objOrdenSelected.Receptor = ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).FirstOrDefault.CodigoReceptor
                            End If
                        ElseIf ListaReceptoresUsuario.Count = 1 Then
                            objOrdenSelected.Receptor = ListaReceptoresUsuario.FirstOrDefault.CodigoReceptor
                        End If

                        If String.IsNullOrEmpty(objOrdenSelected.Receptor) Then
                            IsBusy = False
                        End If

                    Case OPCION_COMBOSRECEPTOR
                        logRealizarConsultaPropiedades = False
                        If Not IsNothing(ListaParametrosReceptor) And Not IsNothing(ConfiguracionReceptor) Then
                            If String.IsNullOrEmpty(objOrdenSelected.TipoOrden) Then
                                'Obtiene el valor por defecto del tipo de Orden
                                objOrdenSelected.TipoOrden = ConfiguracionReceptor.TipoOrdenDefecto
                            End If

                            objOrdenSelected.TipoProducto = AsignarValorTopicoCategoria(objOrdenSelected.TipoProducto, "TIPOPRODUCTO", "TIPOPRODUCTO", String.Empty)

                            If ValidarTipoProductoPosicionPropia(objOrdenSelected) Then
                                LlevarRecetorOrdenDistribucionComision(objOrdenSelected, True)
                            End If

                            objOrdenSelected.TipoOperacion = AsignarValorTopicoCategoria(objOrdenSelected.TipoOperacion, "TIPOOPERACION", "TIPOOPERACION", String.Empty)
                            'objOrdenSelected.TipoNegocio=AsignarValorTopicoCategoria(objOrdenSelected.TipoNegocio, "TIPONEGOCIO", "TIPOPRODUCTO", String.Empty)
                            objOrdenSelected.TipoLimite = AsignarValorTopicoCategoria(objOrdenSelected.TipoLimite, "TIPOLIMITE", "TIPOLIMITE", TIPOLIMITEXDEFECTO)
                            objOrdenSelected.CondicionesNegociacion = AsignarValorTopicoCategoria(objOrdenSelected.CondicionesNegociacion, "CONDICNEGOCIACION", "CONDICNEGOCIACION", CONDNEGOCIACIONXDEFECTO)
                            objOrdenSelected.FormaPago = AsignarValorTopicoCategoria(objOrdenSelected.FormaPago, "FORMAPAGO", "FORMAPAGO", String.Empty)
                            objOrdenSelected.TipoInversion = AsignarValorTopicoCategoria(objOrdenSelected.TipoInversion, "TIPOINVERSION", "TIPOINVERSION", String.Empty)
                            If String.IsNullOrEmpty(objOrdenSelected.TipoInversion) Then
                                If objOrdenSelected.TipoProducto = TIPOPRODUCTO_CUENTAPROPIA Then
                                    objOrdenSelected.TipoInversion = TIPOINVERSIONXDEFECTO        ' EOMC -- Dato por defecto si hay más de un item en el combo -- 11/20/2012
                                End If
                            End If

                            objOrdenSelected.Ejecucion = AsignarValorTopicoCategoria(objOrdenSelected.Ejecucion, "EJECUCION", "EJECUCION", String.Empty)
                            If String.IsNullOrEmpty(objOrdenSelected.Ejecucion) Then
                                If objOrdenSelected.TipoNegocio = TIPONEGOCIO_ACCIONES Then
                                    objOrdenSelected.Ejecucion = EJECUCIONXDEFECTO
                                End If
                            End If

                            objOrdenSelected.Duracion = AsignarValorTopicoCategoria(objOrdenSelected.Duracion, "DURACION", "DURACION", String.Empty)
                            If String.IsNullOrEmpty(objOrdenSelected.Duracion) Then
                                If objOrdenSelected.TipoNegocio = TIPONEGOCIO_ACCIONES Then
                                    objOrdenSelected.Duracion = DURACIONXDEFECTO     ' EOMC -- Dato por defecto si hay más de un item en el combo -- 11/20/2012
                                End If
                            End If

                            objOrdenSelected.CanalRecepcion = AsignarValorTopicoCategoria(objOrdenSelected.CanalRecepcion, "CANALLEO", "CANALLEO", String.Empty)
                            objOrdenSelected.UsuarioOperador = AsignarValorTopicoCategoria(objOrdenSelected.UsuarioOperador, "USUARIO_OPERADOR", "USUARIO_OPERADOR", String.Empty)
                            objOrdenSelected.MedioVerificable = AsignarValorTopicoCategoria(objOrdenSelected.MedioVerificable, "MEDIOVERLEO", "MEDIOVERLEO", String.Empty)

                            If IsNothing(objOrdenSelected.ProductoValores) Then
                                If DiccionarioCombosOYDPlus.ContainsKey("PRODUCTO_VALORES") Then
                                    'Valida sí el diccionario tiene solo un valor para asignarselo por defecto
                                    If DiccionarioCombosOYDPlus("PRODUCTO_VALORES").Count = 1 Then
                                        objOrdenSelected.ProductoValores = DiccionarioCombosOYDPlus("PRODUCTO_VALORES").FirstOrDefault.Retorno
                                    Else
                                        If Not IsNothing(ListaParametrosReceptor) Then
                                            'Valida que tenga configurado para el topico un valor por defecto.
                                            If ListaParametrosReceptor.Where(Function(i) i.Prioridad = 0 And i.Topico = "PRODUCTO_VALORES").Count > 0 Then
                                                objOrdenSelected.ProductoValores = ListaParametrosReceptor.Where(Function(i) i.Prioridad = 0 And i.Topico = "PRODUCTO_VALORES").First.Valor
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If objOrdenSelected.TipoNegocio = TIPONEGOCIO_ACCIONES Then
                                objOrdenSelected.Clasificacion = AsignarValorTopicoCategoria(objOrdenSelected.Clasificacion, "CLASIFICACIONACCIONES", "CLASIFICACIONACCIONES", CLASIFICACIONXDEFECTO)
                            ElseIf objOrdenSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                                objOrdenSelected.Clasificacion = AsignarValorTopicoCategoria(objOrdenSelected.Clasificacion, "CLASIFICACIONRENTAFIJA", "CLASIFICACIONRENTAFIJA", CLASIFICACIONXDEFECTO)
                            End If

                            'Obtiene el valor por defecto de la extersión
                            objOrdenSelected.NroExtensionToma = ConfiguracionReceptor.ExtensionDefecto

                            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                                If DiccionarioCombosOYDPlus.ContainsKey("BOLSA") Then
                                    If DiccionarioCombosOYDPlus("BOLSA").Count > 0 Then
                                        ' EOMC -- 11/20/2012
                                        ' Retorno identifica la bolsa, se estaba asignando el id de la tabla de combosReceptor
                                        objOrdenSelected.Bolsa = DiccionarioCombosOYDPlus("BOLSA").FirstOrDefault.Retorno
                                        objOrdenSelected.NombreBolsa = DiccionarioCombosOYDPlus("BOLSA").FirstOrDefault.Descripcion
                                    End If
                                End If
                            End If

                        End If

                        IsBusy = False
                    Case OPCION_TIPONEGOCIO
                        logRealizarConsultaPropiedades = False

                        If objOrdenSelected.TipoNegocio = TIPONEGOCIO_ACCIONES Then
                            objOrdenSelected.Clasificacion = AsignarValorTopicoCategoria(objOrdenSelected.Clasificacion, "CLASIFICACIONACCIONES", "CLASIFICACIONACCIONES", CLASIFICACIONXDEFECTO)
                        ElseIf objOrdenSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                            objOrdenSelected.Clasificacion = AsignarValorTopicoCategoria(objOrdenSelected.Clasificacion, "CLASIFICACIONRENTAFIJA", "CLASIFICACIONRENTAFIJA", CLASIFICACIONXDEFECTO)
                        Else
                            objOrdenSelected.Clasificacion = AsignarValorTopicoCategoria(objOrdenSelected.Clasificacion, "CLASIFICACION", "CLASIFICACION", CLASIFICACIONXDEFECTO)
                        End If


                        objOrdenSelected.Mercado = AsignarValorTopicoCategoria(objOrdenSelected.Mercado, "MERCADO", "MERCADO", MERCADOXDEFECTO)

                        If objOrdenSelected.TipoNegocio = TIPONEGOCIO_ACCIONES Then
                            If String.IsNullOrEmpty(objOrdenSelected.Duracion) Then
                                objOrdenSelected.Duracion = AsignarValorTopicoCategoria(objOrdenSelected.Duracion, "DURACION", "DURACION", DURACIONXDEFECTO)
                            End If
                        End If

                        If objOrdenSelected.TipoNegocio = TIPONEGOCIO_ACCIONES Then
                            If String.IsNullOrEmpty(objOrdenSelected.Ejecucion) Then
                                objOrdenSelected.Ejecucion = AsignarValorTopicoCategoria(objOrdenSelected.Ejecucion, "EJECUCION", "EJECUCION", EJECUCIONXDEFECTO)
                            End If
                        End If

                        IsBusy = False
                    Case OPCION_DUPLICAR
                        If Not IsNothing(ListaParametrosReceptor) And Not IsNothing(ConfiguracionReceptor) Then
                            If Not IsNothing(_OrdenDuplicarOYDPLUS) Then
                                _OrdenDuplicarOYDPLUS.CanalRecepcion = Nothing
                                _OrdenDuplicarOYDPLUS.UsuarioOperador = Nothing
                                _OrdenDuplicarOYDPLUS.MedioVerificable = Nothing

                                _OrdenDuplicarOYDPLUS.CanalRecepcion = AsignarValorTopicoCategoria(_OrdenDuplicarOYDPLUS.CanalRecepcion, "CANALLEO", "CANALLEO", String.Empty)
                                _OrdenDuplicarOYDPLUS.UsuarioOperador = AsignarValorTopicoCategoria(_OrdenDuplicarOYDPLUS.UsuarioOperador, "USUARIO_OPERADOR", "USUARIO_OPERADOR", String.Empty)
                                _OrdenDuplicarOYDPLUS.MedioVerificable = AsignarValorTopicoCategoria(_OrdenDuplicarOYDPLUS.MedioVerificable, "MEDIOVERLEO", "MEDIOVERLEO", String.Empty)

                                'Obtiene el valor por defecto de la extersión
                                _OrdenDuplicarOYDPLUS.NroExtensionToma = ConfiguracionReceptor.ExtensionDefecto

                                logCambiarDetallesOrden = False
                                ObtenerValoresOrdenAnterior(OrdenDuplicarOYDPLUS, OrdenOYDPLUSSelected)
                                logCambiarDetallesOrden = True

                                logModificarDatosTipoNegocio = False
                                If Not IsNothing(ListaTipoNegocio) Then
                                    If ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = _OrdenOYDPLUSSelected.TipoNegocio).Count > 0 Then
                                        TipoNegocioSelected = ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = _OrdenOYDPLUSSelected.TipoNegocio).FirstOrDefault
                                    End If
                                End If
                                logModificarDatosTipoNegocio = True

                                consultarOrdenantesOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                                consultarCuentasDepositoOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                                HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                                logCalcularValores = False
                                CalcularFechaVigenciaOrden(_OrdenOYDPLUSSelected)
                                logCalcularValores = True

                                Dim objNuevaListaDistribucion As New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
                                For Each li In ListaDistribucionComisionSalvar
                                    objNuevaListaDistribucion.Add(li)
                                Next

                                ListaReceptoresOrdenes = objNuevaListaDistribucion

                                BeneficiariosOrdenSelected = Nothing
                                If Not IsNothing(ListaBeneficiariosOrdenes) Then
                                    Dim objListaBen As New List(Of OyDPLUSOrdenesOF.BeneficiariosOrden)
                                    ListaBeneficiariosOrdenes = Nothing
                                    BeneficiariosOrdenSelected = Nothing
                                    ListaBeneficiariosOrdenes = objListaBen
                                End If

                                If Not IsNothing(ListaLiqAsociadasOrdenes) Then
                                    Dim objListaLiq As New List(Of OyDPLUSOrdenesOF.LiquidacionesOrden)
                                    ListaLiqAsociadasOrdenes = Nothing
                                    ListaLiqAsociadasOrdenes = objListaLiq
                                End If

                                MostrarNegocio = Visibility.Visible
                                MostrarControles = Visibility.Visible

                                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                    If _OrdenOYDPLUSSelected.TipoOperacion = TIPOOPERACION_COMPRA Or _OrdenOYDPLUSSelected.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                                        MostrarCamposCompra = Visibility.Visible
                                        MostrarCamposVenta = Visibility.Collapsed
                                    Else
                                        MostrarCamposCompra = Visibility.Collapsed
                                        MostrarCamposVenta = Visibility.Visible
                                    End If
                                End If

                                HabilitarConsultaControles(_OrdenOYDPLUSSelected)

                            End If
                        End If

                        logPlantillaRegistro = False
                        strNombrePlantilla = String.Empty

                        VerificarValoresEnCombos()


                    Case OPCION_PLANTILLA, OPCION_CREARORDENPLANTILLA
                        If Not IsNothing(_OrdenPlantillaOYDPLUS) Then
                            logCambiarDetallesOrden = False
                            ObtenerValoresOrdenAnterior(OrdenPlantillaOYDPLUS, OrdenOYDPLUSSelected)
                            logCambiarDetallesOrden = True

                            logModificarDatosTipoNegocio = False
                            If Not IsNothing(ListaTipoNegocio) Then
                                If ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = _OrdenOYDPLUSSelected.TipoNegocio).Count > 0 Then
                                    TipoNegocioSelected = ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = _OrdenOYDPLUSSelected.TipoNegocio).FirstOrDefault
                                End If
                            End If
                            logModificarDatosTipoNegocio = False

                            consultarOrdenantesOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                            consultarCuentasDepositoOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                            logCalcularValores = False
                            CalcularFechaVigenciaOrden(_OrdenOYDPLUSSelected)
                            logCalcularValores = True

                            Dim objNuevaListaDistribucion As New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
                            objNuevaListaDistribucion.Add(New OyDPLUSOrdenesOF.ReceptoresOrden With {.ClaseOrden = _OrdenOYDPLUSSelected.Clase,
                                                                                               .FechaActualizacion = dtmFechaServidor,
                                                                                               .IDComisionista = 0,
                                                                                               .IDReceptor = _OrdenOYDPLUSSelected.Receptor,
                                                                                               .IDReceptorOrden = 0,
                                                                                               .IDSucComisionista = 0,
                                                                                               .Lider = True,
                                                                                               .Nombre = _OrdenOYDPLUSSelected.NombreReceptor,
                                                                                               .NroOrden = _OrdenOYDPLUSSelected.NroOrden,
                                                                                               .Porcentaje = 100,
                                                                                               .TipoOrden = _OrdenOYDPLUSSelected.TipoOrden,
                                                                                               .Usuario = Program.Usuario,
                                                                                               .Version = _OrdenOYDPLUSSelected.Version})

                            ListaReceptoresOrdenes = objNuevaListaDistribucion

                            BeneficiariosOrdenSelected = Nothing
                            If Not IsNothing(ListaBeneficiariosOrdenes) Then
                                Dim objListaBen As New List(Of OyDPLUSOrdenesOF.BeneficiariosOrden)
                                ListaBeneficiariosOrdenes = Nothing
                                BeneficiariosOrdenSelected = Nothing
                                ListaBeneficiariosOrdenes = objListaBen
                            End If

                            If Not IsNothing(ListaLiqAsociadasOrdenes) Then
                                Dim objListaLiq As New List(Of OyDPLUSOrdenesOF.LiquidacionesOrden)
                                ListaLiqAsociadasOrdenes = Nothing
                                ListaLiqAsociadasOrdenes = objListaLiq
                            End If
                        End If

                        If pstrOpcion.ToUpper = OPCION_CREARORDENPLANTILLA Then
                            logPlantillaRegistro = False
                            strNombrePlantilla = String.Empty

                            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                If _OrdenOYDPLUSSelected.TipoOperacion = TIPOOPERACION_COMPRA Or _OrdenOYDPLUSSelected.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                                    MostrarCamposCompra = Visibility.Visible
                                    MostrarCamposVenta = Visibility.Collapsed
                                Else
                                    MostrarCamposCompra = Visibility.Collapsed
                                    MostrarCamposVenta = Visibility.Visible
                                End If
                            End If

                            HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                        End If

                        VerificarValoresEnCombos()
                    Case OPCION_EDITAR
                        VerificarValoresEnCombos()

                        If Not IsNothing(_OrdenOYDPLUSSelected) Then
                            If _OrdenOYDPLUSSelected.TipoOperacion = TIPOOPERACION_COMPRA Or _OrdenOYDPLUSSelected.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                                MostrarCamposCompra = Visibility.Visible
                                MostrarCamposVenta = Visibility.Collapsed
                            Else
                                MostrarCamposCompra = Visibility.Collapsed
                                MostrarCamposVenta = Visibility.Visible
                            End If

                            HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                        End If
                End Select


                logCalcularValores = True
                logRealizarConsultaPropiedades = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto.",
                                 Me.ToString(), "ObtenerValoresDefecto", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub VerificarValoresEnCombos()
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOrden) AndAlso Not ValidarCampoEnDiccionario("TIPOORDEN", _OrdenOYDPLUSSelected.TipoOrden) Then
                    _OrdenOYDPLUSSelected.TipoOrden = String.Empty
                    _OrdenOYDPLUSSelected.NombreTipoOrden = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoProducto) AndAlso Not ValidarCampoEnDiccionario("TIPOPRODUCTO", _OrdenOYDPLUSSelected.TipoProducto) Then
                    _OrdenOYDPLUSSelected.NombreTipoProducto = String.Empty
                    _OrdenOYDPLUSSelected.TipoProducto = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOperacion) AndAlso Not ValidarCampoEnDiccionario("TIPOOPERACION", _OrdenOYDPLUSSelected.TipoOperacion) Then
                    _OrdenOYDPLUSSelected.TipoOperacion = String.Empty
                    _OrdenOYDPLUSSelected.NombreTipoOperacion = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Clasificacion) AndAlso Not ValidarCampoEnDiccionario("CLASIFICACION", _OrdenOYDPLUSSelected.Clasificacion) Then
                    _OrdenOYDPLUSSelected.Clasificacion = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Mercado) AndAlso Not ValidarCampoEnDiccionario("MERCADO", _OrdenOYDPLUSSelected.Mercado) Then
                    _OrdenOYDPLUSSelected.Mercado = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoLimite) AndAlso Not ValidarCampoEnDiccionario("TIPOLIMITE", _OrdenOYDPLUSSelected.TipoLimite) Then
                    _OrdenOYDPLUSSelected.TipoLimite = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.CondicionesNegociacion) AndAlso Not ValidarCampoEnDiccionario("CONDICNEGOCIACION", _OrdenOYDPLUSSelected.CondicionesNegociacion) Then
                    _OrdenOYDPLUSSelected.CondicionesNegociacion = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.FormaPago) AndAlso Not ValidarCampoEnDiccionario("FORMAPAGO", _OrdenOYDPLUSSelected.FormaPago) Then
                    _OrdenOYDPLUSSelected.FormaPago = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoInversion) AndAlso Not ValidarCampoEnDiccionario("TIPOINVERSION", _OrdenOYDPLUSSelected.TipoInversion) Then
                    _OrdenOYDPLUSSelected.TipoInversion = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Ejecucion) AndAlso Not ValidarCampoEnDiccionario("EJECUCION", _OrdenOYDPLUSSelected.Ejecucion) Then
                    _OrdenOYDPLUSSelected.Ejecucion = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Duracion) AndAlso Not ValidarCampoEnDiccionario("DURACION", _OrdenOYDPLUSSelected.Duracion) Then
                    _OrdenOYDPLUSSelected.Duracion = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.CanalRecepcion) AndAlso Not ValidarCampoEnDiccionario("CANALLEO", _OrdenOYDPLUSSelected.CanalRecepcion) Then
                    _OrdenOYDPLUSSelected.CanalRecepcion = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.UsuarioOperador) AndAlso Not ValidarCampoEnDiccionario("USUARIO_OPERADOR", _OrdenOYDPLUSSelected.UsuarioOperador) Then
                    _OrdenOYDPLUSSelected.UsuarioOperador = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.MedioVerificable) AndAlso Not ValidarCampoEnDiccionario("MEDIOVERLEO", _OrdenOYDPLUSSelected.MedioVerificable) Then
                    _OrdenOYDPLUSSelected.MedioVerificable = String.Empty
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los valores de los combos.",
                                 Me.ToString(), "VerificarValoresEnCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Asignar el valor x defecto de una variable
    ''' Desarrollo Juan David Correa.
    ''' 
    ''' </summary>
    ''' <param name="pobjValor"></param>
    ''' <param name="pstrTopicoDiccionario"></param>
    ''' <param name="pstrTopicoParametros"></param>
    ''' <param name="pstrValorDefecto"></param>
    ''' <remarks></remarks>
    Private Function AsignarValorTopicoCategoria(ByVal pobjValor As String, ByVal pstrTopicoDiccionario As String, ByVal pstrTopicoParametros As String, pstrValorDefecto As String) As String
        Dim objRetorno As String = String.Empty
        Try
            'Valida que la opción no se encuentre llena para no sobreescribirla
            If String.IsNullOrEmpty(pobjValor) Then
                'Valida que el topico exista
                If DiccionarioCombosOYDPlus.ContainsKey(pstrTopicoDiccionario) Then
                    'Valida sí el diccionario tiene solo un valor para asignarselo por defecto
                    If DiccionarioCombosOYDPlus(pstrTopicoDiccionario).Count = 1 Then
                        objRetorno = DiccionarioCombosOYDPlus(pstrTopicoDiccionario).FirstOrDefault.Retorno
                    Else
                        If Not IsNothing(ListaParametrosReceptor) Then
                            'Valida que tenga configurado para el topico un valor por defecto.
                            If ListaParametrosReceptor.Where(Function(i) i.Prioridad = 0 And i.Topico = pstrTopicoParametros).Count > 0 Then
                                Dim objValorParametros As String = ListaParametrosReceptor.Where(Function(i) i.Prioridad = 0 And i.Topico = pstrTopicoParametros).First.Valor
                                If DiccionarioCombosOYDPlus(pstrTopicoDiccionario).Where(Function(i) i.Retorno = objValorParametros).Count > 0 Then
                                    objRetorno = objValorParametros
                                Else
                                    objRetorno = String.Empty
                                End If
                            Else
                                objRetorno = pstrValorDefecto
                            End If
                        Else
                            objRetorno = pstrValorDefecto
                        End If
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

    ''' <summary>
    ''' Metodo para obtener los valores de los combos.
    ''' Cuando el parametro esta en true es para obtener los combos por completo.
    ''' Solo se necesita para que cuando este en modo vista.
    ''' Cuando se encuentre en modo edición se cargan los combos dependiendo de las caracteristicas que tenga el receptor habilitadas
    ''' Fecha 23 de agosto del 2012
    ''' </summary>
    ''' <param name="ValoresCompletos"></param>
    ''' <remarks></remarks>
    Public Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, Optional ByVal Opcion As String = "")
        Try
            Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
            Dim objListaCategoria As New List(Of OYDPLUSUtilidades.CombosReceptor)
            Dim objListaCategoria1 As New List(Of OYDPLUSUtilidades.CombosReceptor)
            Dim objListaCategoria2 As New List(Of OYDPLUSUtilidades.CombosReceptor)
            Dim objListaCategoria3 As New List(Of OYDPLUSUtilidades.CombosReceptor)

            For Each li In DiccionarioCombosOYDPlus
                objDiccionario.Add(li.Key, li.Value)
            Next

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
                    If objDiccionario.ContainsKey("CLASIFICACIONACCIONES") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONACCIONES", "CLASIFICACION"))
                    If objDiccionario.ContainsKey("CLASIFICACIONRENTAFIJA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONRENTAFIJA", "CLASIFICACION"))
                    If objDiccionario.ContainsKey("CLASIFICACIONREPO") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONREPO", "CLASIFICACION"))
                    If objDiccionario.ContainsKey("CLASIFICACIONSIMULTANEAS") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONSIMULTANEAS", "CLASIFICACION"))
                    If objDiccionario.ContainsKey("CLASIFICACIONTTV") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONTTV", "CLASIFICACION"))

                    If objListaCategoria.Count > 0 Then objDiccionario.Add("CLASIFICACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                    '************************************************************************************
                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    'Valores para el tipo de Operación
                    '************************************************************************************
                    If objDiccionario.ContainsKey("TIPOOPERACIONGENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOOPERACIONGENERAL", "TIPOOPERACION"))
                    If objDiccionario.ContainsKey("TIPOOPERACIONACTIVAPASIVA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOOPERACIONACTIVAPASIVA", "TIPOOPERACION"))

                    If objListaCategoria.Count > 0 Then objDiccionario.Add("TIPOOPERACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                    '************************************************************************************
                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    'Valores para el combo de duración
                    '************************************************************************************
                    If objDiccionario.ContainsKey("DURACIONGENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "DURACIONGENERAL", "DURACION"))

                    If objListaCategoria.Count > 0 Then objDiccionario.Add("DURACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

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

                        If objDiccionario.ContainsKey("FECHAACTUAL_SERVIDOR") Then
                            If objDiccionario("FECHAACTUAL_SERVIDOR").Count > 0 Then
                                Try
                                    dtmFechaServidor = DateTime.ParseExact(objDiccionario("FECHAACTUAL_SERVIDOR").First.Retorno, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)
                                Catch ex As Exception
                                    dtmFechaServidor = Now
                                End Try
                            End If
                        End If

                        If objDiccionario.ContainsKey("HABILITARFUNCIONALIDADPOSICIONPROPIA_ORDENOYDPLUS") Then
                            If objDiccionario("HABILITARFUNCIONALIDADPOSICIONPROPIA_ORDENOYDPLUS").Count > 0 Then
                                If objDiccionario("HABILITARFUNCIONALIDADPOSICIONPROPIA_ORDENOYDPLUS").First.Retorno = "SI" Then
                                    logHabilitarCondicionesTipoProductoCuentaPropia = True
                                Else
                                    logHabilitarCondicionesTipoProductoCuentaPropia = False
                                End If
                            End If
                        End If

                        If objDiccionario.ContainsKey("TIPOPRODUCTOPOSICIONPROPIA") Then
                            If objDiccionario("TIPOPRODUCTOPOSICIONPROPIA").Count > 0 Then
                                strTiposProductoPosicionPropia = objDiccionario("TIPOPRODUCTOPOSICIONPROPIA").First.Retorno
                            End If
                        End If
                    End If

                    DiccionarioCombosOYDPlus = objDiccionario

                Else ' Cuando ValoresCompletos = False
                    If Not String.IsNullOrEmpty(Opcion) Then
                        Dim OpcionValoresDefecto As String = String.Empty


                        Select Case Opcion.ToUpper
                            Case OPCION_RECEPTOR
                                If objDiccionario.ContainsKey("TIPOORDEN") Then Call PrRemoverValoresDic(objDiccionario, {"TIPOORDEN"})
                                If Not IsNothing(objDiccionario("TIPOORDENGENERAL")) Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOORDENGENERAL", "TIPOORDEN"))
                                If objListaCategoria.Count > 0 Then objDiccionario.Add("TIPOORDEN", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)
                                OpcionValoresDefecto = OPCION_COMBOSRECEPTOR

                            Case OPCION_TIPOOPERACION
                                Call PrRemoverValoresDic(objDiccionario, {"TIPOOPERACION"})
                                If objDiccionario.ContainsKey("TIPOOPERACIONGENERAL") Then objListaCategoria1.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOOPERACIONGENERAL", "TIPOOPERACION"))
                                objDiccionario.Add("TIPOOPERACION", objListaCategoria1.OrderBy(Function(i) i.Descripcion).ToList)

                            Case OPCION_EDITAR, OPCION_DUPLICAR, OPCION_TIPONEGOCIO, OPCION_PLANTILLA, OPCION_CREARORDENPLANTILLA
                                Dim strValorCampo As String = String.Empty

                                If Opcion.ToUpper.ToString.Equals(OPCION_TIPONEGOCIO) Then
                                    PrRemoverValoresDic(objDiccionario, {"CLASIFICACION", "DURACION", "MERCADO", "TIPOOPERACION"})
                                Else
                                    PrRemoverValoresDic(objDiccionario, {"CLASIFICACION", "TIPOOPERACION", "TIPOORDEN", "DURACION", "MERCADO"})
                                    If objDiccionario.ContainsKey("TIPOORDENGENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOORDENGENERAL", "TIPOORDEN"))
                                    If objListaCategoria.Count > 0 Then objDiccionario.Add("TIPOORDEN", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)
                                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()
                                End If

                                If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) Then
                                    Select Case pobjOrdenSelected.TipoNegocio.ToUpper

                                        'Santiago Alexander Vergara Orrego - Mayo 15/2014 - lógica para llenar los combos de los tipos de negocio otras firmas acciones y renta fija
                                        Case TIPONEGOCIO_ACCIONES, TIPONEGOCIO_RENTAFIJA, TIPONEGOCIO_ACCIONESOF, TIPONEGOCIO_RENTAFIJAOF

                                            If objDiccionario.ContainsKey("TIPOOPERACIONGENERAL") Then objListaCategoria1.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOOPERACIONGENERAL", "TIPOOPERACION")) 'a y c

                                            If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONES Then
                                                If objDiccionario.ContainsKey("CLASIFICACIONACCIONES") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONACCIONES", "CLASIFICACION")) 'a
                                                If objDiccionario.ContainsKey("MERCADO_GENERAL") Then objListaCategoria2.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "MERCADO_GENERAL", "MERCADO").Where(Function(i) i.Retorno = "S" Or i.Retorno = "P")) 'a
                                            Else
                                                If objDiccionario.ContainsKey("CLASIFICACIONRENTAFIJA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONRENTAFIJA", "CLASIFICACION")) 'c
                                                If objDiccionario.ContainsKey("MERCADO_GENERAL") Then objListaCategoria2.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "MERCADO_GENERAL", "MERCADO").Where(Function(i) i.Retorno = "S" Or i.Retorno = "P" Or i.Retorno = "R")) 'c
                                            End If

                                        Case TIPONEGOCIO_REPO
                                            If objDiccionario.ContainsKey("CLASIFICACIONREPO") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONREPO", "CLASIFICACION"))
                                            If objDiccionario.ContainsKey("TIPOOPERACIONACTIVAPASIVA") Then objListaCategoria1.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOOPERACIONACTIVAPASIVA", "TIPOOPERACION"))
                                            If objDiccionario.ContainsKey("MERCADO_GENERAL") Then objListaCategoria2.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "MERCADO_GENERAL", "MERCADO").Where(Function(i) i.Retorno = "E"))
                                        Case TIPONEGOCIO_SIMULTANEA, TIPONEGOCIO_TTV

                                            If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_SIMULTANEA Then
                                                If objDiccionario.ContainsKey("CLASIFICACIONSIMULTANEAS") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONSIMULTANEAS", "CLASIFICACION"))
                                            Else
                                                If objDiccionario.ContainsKey("CLASIFICACIONTTV") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "CLASIFICACIONTTV", "CLASIFICACION"))
                                            End If

                                            If objDiccionario.ContainsKey("TIPOOPERACIONACTIVAPASIVA") Then objListaCategoria1.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "TIPOOPERACIONACTIVAPASIVA", "TIPOOPERACION"))
                                            If objDiccionario.ContainsKey("MERCADO_GENERAL") Then objListaCategoria2.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "MERCADO_GENERAL", "MERCADO").Where(Function(i) i.Retorno = "S"))

                                    End Select

                                    objDiccionario.Add("CLASIFICACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)
                                    objDiccionario.Add("TIPOOPERACION", objListaCategoria1.OrderBy(Function(i) i.Descripcion).ToList)
                                    objDiccionario.Add("MERCADO", objListaCategoria2.OrderBy(Function(i) i.Descripcion).ToList)

                                    'Valores para el combo de duración
                                    '************************************************************************************
                                    If objDiccionario.ContainsKey("DURACIONGENERAL") Then
                                        Dim obj = ExtraerListaPorCategoria(objDiccionario, "DURACIONGENERAL", "DURACION")
                                        If pobjOrdenSelected.TipoNegocio.ToUpper <> TIPONEGOCIO_ACCIONES Then
                                            objListaCategoria3 = obj.Where(Function(w) w.Retorno.ToLower.Equals("c") Or w.Retorno.ToLower.Equals("f")).ToList
                                        Else
                                            objListaCategoria3 = obj
                                        End If

                                        objDiccionario.Add("DURACION", objListaCategoria3.OrderBy(Function(i) i.Descripcion).ToList)
                                    End If

                                    '************************************************************************************

                                    If Opcion.ToUpper.ToString.Equals(OPCION_TIPONEGOCIO) Then OpcionValoresDefecto = OPCION_TIPONEGOCIO

                                End If

                        End Select

                        DiccionarioCombosOYDPlus = objDiccionario

                        If Opcion.ToUpper = OPCION_EDITAR Then
                            'Se llevan los anteriores a la orden ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.

                            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)

                            Editando = True
                            MyBase.CambioItem("Editando")

                            'Se posiciona en el primer registro
                            BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboClasificacion")

                            HabilitarEncabezado = False
                            HabilitarOpcionesCruzada = False
                            HabilitarNegocio = True
                            Confirmaciones = String.Empty
                            Justificaciones = String.Empty
                            Aprobaciones = String.Empty
                            CantidadConfirmaciones = 0
                            CantidadAprobaciones = 0
                            CantidadJustificaciones = 0
                            MostrarControlMensajes = Visibility.Visible

                            logModificarDatosTipoNegocio = False
                            If Not IsNothing(ListaTipoNegocio) Then
                                If ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = pobjOrdenSelected.TipoNegocio).Count > 0 Then
                                    TipoNegocioSelected = ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = pobjOrdenSelected.TipoNegocio).FirstOrDefault
                                End If
                            End If
                            logModificarDatosTipoNegocio = True

                            consultarOrdenantesOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                            consultarCuentasDepositoOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)

                            OpcionValoresDefecto = OPCION_EDITAR

                            IsBusy = False

                        ElseIf Opcion.ToUpper = OPCION_DUPLICAR Or Opcion.ToUpper = OPCION_PLANTILLA Or Opcion.ToUpper = OPCION_CREARORDENPLANTILLA Then
                            'Se llevan los anteriores a la orden ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.
                            logCambiarDetallesOrden = False

                            HabilitarEncabezado = True
                            HabilitarOpcionesCruzada = True
                            HabilitarNegocio = True
                            Confirmaciones = String.Empty
                            Justificaciones = String.Empty
                            Aprobaciones = String.Empty
                            CantidadConfirmaciones = 0
                            CantidadAprobaciones = 0
                            CantidadJustificaciones = 0

                            Editando = True
                            MyBase.CambioItem("Editando")

                            'Se posiciona en el primer registro
                            BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboClasificacion")

                            MostrarControlMensajes = Visibility.Visible

                            logCambiarDetallesOrden = True

                            OpcionValoresDefecto = Opcion.ToUpper

                            IsBusy = False
                        End If

                        ObtenerValoresDefectoOYDPLUS(OpcionValoresDefecto, pobjOrdenSelected)

                    End If

                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto de los combos.",
                                 Me.ToString(), "ObtenerValoresCombos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para validar si se debe de habilitar la negociación de la pantalla de ordenes.
    ''' </summary>
    Public Sub ValidarHabilitarNegocio(Optional ByVal CambioTipoOperacion As Boolean = False)
        Try
            If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOrden) And
                Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoNegocio) And
                Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOperacion) And
                Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoProducto) Then
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la habilitación del negocio.",
                                Me.ToString(), "ValidarHabilitarNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Seleccionar especie elegida en el buscador.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    Public Sub SeleccionarEspecieOYDPLUS(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    _OrdenOYDPLUSSelected.Especie = pobjEspecie.Nemotecnico

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

                    BorrarEspecie = False

                    If _OrdenOYDPLUSSelected.TipoOperacion = TIPOOPERACION_COMPRA Then
                        'Consultar los precios del mercado para activar el ticker.
                        CargarMensajeDinamicoOYDPLUS("PRECIOSVENTAESPECIE", String.Empty, String.Empty, _OrdenOYDPLUSSelected.Especie)
                    Else
                        'Consultar los precios del mercado para activar el ticker.
                        CargarMensajeDinamicoOYDPLUS("PRECIOSCOMPRAESPECIE", String.Empty, String.Empty, _OrdenOYDPLUSSelected.Especie)
                    End If
                End If
            Else
                _OrdenOYDPLUSSelected.Especie = "(No Seleccionado)"
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
    ''' Seleccionar el cliente elegido en el buscador.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    ''' <param name="pobjCliente"></param>
    ''' <remarks></remarks>
    Public Sub SeleccionarClienteOYDPLUS(ByVal pobjCliente As OYDUtilidades.BuscadorClientes, ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, Optional ByVal pstrOpcion As String = "")
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
                        If logNuevoRegistro Or logEditarRegistro Then
                            If ValidarTipoProductoPosicionPropia(pobjOrdenSelected) = False Then
                                consultarReceptoresComitente(pobjOrdenSelected.Clase, pobjOrdenSelected.TipoOperacion, pobjOrdenSelected.IDComitente, String.Empty)
                            End If
                        End If

                        BorrarCliente = False
                        'Consultar los precios del mercado para activar el ticker.
                        CargarMensajeDinamicoOYDPLUS("DOCUMENTOSCLIENTE", String.Empty, pobjOrdenSelected.IDComitente, String.Empty)

                    End If
                End If
            Else
                If Not IsNothing(pobjOrdenSelected) Then
                    pobjOrdenSelected.IDComitente = "-9999999999"
                    pobjOrdenSelected.NombreCliente = "(No Seleccionado)"
                    pobjOrdenSelected.NroDocumento = 0
                    pobjOrdenSelected.CategoriaCliente = "(No Seleccionado)"
                    pobjOrdenSelected.CuentaDeposito = 0
                    pobjOrdenSelected.UBICACIONTITULO = String.Empty
                    pobjOrdenSelected.IDOrdenante = String.Empty

                    OrdenanteSeleccionadoOYDPLUS = Nothing
                    CtaDepositoSeleccionadaOYDPLUS = Nothing

                    ListaOrdenantesOYDPLUS = Nothing
                    ListaCuentasDepositoOYDPLUS = Nothing

                    BorrarCliente = True

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarClienteOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Modificado por Juan David Correa
    '''Obtiene el receptor de la orden siempre y cuando el receptor de la orden este vacio
    '''Fecha 23 de agosto del 2012
    '''Cargar el receptor de la orden si no lo tiene ingresado.
    ''' </summary>
    Public Sub ObtenerReceptorLiderOrdenOYDPLUS()
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not IsNothing(_ListaReceptoresOrdenes) Then
                    If String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Receptor) Then
                        If ListaReceptoresOrdenes.Where(Function(i) i.Lider = True).Count > 0 Then
                            If _OrdenOYDPLUSSelected.Receptor <> _ListaReceptoresOrdenes.Where(Function(i) i.Lider = True).FirstOrDefault.IDReceptor Then
                                _OrdenOYDPLUSSelected.Receptor = _ListaReceptoresOrdenes.Where(Function(i) i.Lider = True).FirstOrDefault.IDReceptor
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener el receptor lider de la orden.", Me.ToString, "ObtenerReceptorLiderOrdenOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para habilitar o deshabilitar los campos dependiendo del tipo de orden que sea.
    ''' Desarrollado por Juan david Correa
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub HabilitarCamposOYDPLUS(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                'Llevar el texto al campo precio dependiendo de lo seleccionado en la Orden.
                If Not IsNothing(pobjOrdenSelected.TipoNegocio) Then

                    'Mostrar Consulta de ISINES PARA TIPOS NEGOCIOS RENTA FIJA : JDOL 20150602
                    If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJAOF Then
                        HabilitarComboISIN = True
                    Else
                        HabilitarComboISIN = False
                    End If

                    If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then
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
                        MostrarTipoNegocioRepoCompra = Visibility.Collapsed
                        MostrarTipoNegocioRepoVenta = Visibility.Collapsed
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarCamposFaciales = Visibility.Collapsed
                        MostrarAdicionalesEspecie = Visibility.Collapsed
                        MostrarTipoNegocioRentaFijaOF = Visibility.Collapsed

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
                        MostrarTipoNegocioRepoCompra = Visibility.Collapsed
                        MostrarTipoNegocioRepoVenta = Visibility.Collapsed
                        MostrarTipoNegocioSimultanea = Visibility.Collapsed
                        MostrarTipoNegocioTTV = Visibility.Collapsed
                        MostrarTipoNegocioAccionesOF = Visibility.Collapsed
                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCamposFaciales = Visibility.Visible
                        End If

                        TextoCantidad = "Nominal"
                        TextoPrecio = "Precio"
                        TextoTasaNominal = "Tasa efectiva"
                        TextoComision = "% comisión pactada"

                    End If

                    'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añade la lógica para mostrar los tabs depediendo del tipo de negocio' 
                    If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJAOF Then
                        MostrarBeneficiarios = Visibility.Visible
                    Else
                        MostrarBeneficiarios = Visibility.Collapsed
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
                        'If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_REPO Or _
                        '    pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONES Then
                        '    TextoValorCaptacionGiro = "Valor compra"
                        'Else
                        '    TextoValorCaptacionGiro = "Valor giro"
                        'End If
                        'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Cambio de label para los campos de acciones de otras firmas 
                        If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then
                            TextoPrecioMaximoMinimo = "Precio inferior"
                            'Else
                            '    TextoPrecioMaximoMinimo = "Precio máximo"
                        End If
                        'Else
                        '    TextoValorCaptacionGiro = "Valor giro"
                    End If

                    TextoSaldoPortafolio = "Saldo"
                ElseIf pobjOrdenSelected.TipoOperacion = TIPOOPERACION_VENTA Or pobjOrdenSelected.TipoOperacion = TIPOOPERACION_REVENTA Then
                    If logEditarRegistro = True Or logNuevoRegistro = True Then
                        MostrarCamposVenta = Visibility.Visible
                    End If
                    MostrarCamposCompra = Visibility.Collapsed

                    If Not IsNothing(pobjOrdenSelected.TipoNegocio) Then
                        'If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_REPO Or _
                        '    pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONES Then
                        '    TextoValorCaptacionGiro = "Valor venta"
                        'Else
                        '    TextoValorCaptacionGiro = "Valor giro"
                        'End If
                        'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Cambio de label para los campos de acciones de otras firmas 
                        If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then
                            TextoPrecioMaximoMinimo = "Precio inferior"
                            'Else
                            '    TextoPrecioMaximoMinimo = "Precio mínimo"
                        End If
                        'Else
                        '    TextoValorCaptacionGiro = "Valor giro"
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

                If Not IsNothing(pobjOrdenSelected.OrdenCruzadaReceptor) Then
                    If pobjOrdenSelected.OrdenCruzadaReceptor Then
                        If logEditarRegistro = True Or logNuevoRegistro = True Then
                            MostrarCruzadaCon = Visibility.Visible
                        End If
                    Else
                        MostrarCruzadaCon = Visibility.Collapsed
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
                        'If MostrarOrdenesSAE = Visibility.Collapsed Then
                        '    BuscarControlValidacion(ViewOrdenesOYDPLUS, "TabSaldoCliente")
                        'Else
                        '    BuscarControlValidacion(ViewOrdenesOYDPLUS, "TabOrdenSAE")
                        'End If
                        MostrarCamposCuentaPropia = Visibility.Collapsed
                    End If
                    If pobjOrdenSelected.TipoOrden = TIPOORDEN_DIRECTA Then
                        BuscarControlValidacion(ViewOrdenesOYDPLUS, "TabOrdenSAE")
                    Else
                        BuscarControlValidacion(ViewOrdenesOYDPLUS, "TabSaldoCliente")
                    End If
                End If

                'Validaciones para mostrar la información de las ordenes cruzadas.
                If pobjOrdenSelected.OrdenCruzadaReceptor Or pobjOrdenSelected.OrdenCruzadaCliente Or pobjOrdenSelected.OrdenCruzada Then
                    MostrarDescripcionOrdenCruzada = Visibility.Visible

                    If pobjOrdenSelected.OrdenCruzada Then
                        TextoCruces = "con órden nro. " & pobjOrdenSelected.IDNroOrdenOriginal
                    Else
                        TextoCruces = "pendiente por cruces"
                    End If
                Else
                    MostrarDescripcionOrdenCruzada = Visibility.Collapsed
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al habilitar los controles dependiendo del tipo de orden.", Me.ToString, "HabilitarCamposOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para habilitar las consultas en los controles.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 22 de octubre del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub HabilitarConsultaControles(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            If logEditarRegistro Or logNuevoRegistro Then
                If Not IsNothing(pobjOrdenSelected) Then
                    TipoNegocioControles = pobjOrdenSelected.TipoNegocio
                    TipoOperacionControles = pobjOrdenSelected.TipoOperacion
                    CodigoOYDControles = pobjOrdenSelected.IDComitente
                    EspecieControles = pobjOrdenSelected.Especie

                    If logCambiarConsultaSAE Then
                        If pobjOrdenSelected.TipoOrden = TIPOORDEN_DIRECTA Then
                            If ConsultarOrdenesSAE Then
                                ConsultarOrdenesSAE = False
                            End If
                            ConsultarOrdenesSAE = True
                        Else
                            ConsultarOrdenesSAE = False
                        End If
                    End If

                    If logCambiarConsultaPortafolio Then
                        If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) And
                        pobjOrdenSelected.TipoOperacion = TIPOOPERACION_VENTA And
                       (Not String.IsNullOrEmpty(pobjOrdenSelected.IDComitente) Or
                        Not String.IsNullOrEmpty(pobjOrdenSelected.Especie)) Then
                            If ConsultarPortafolio Then
                                ConsultarPortafolio = False
                            End If
                            ConsultarPortafolio = True
                        Else
                            ConsultarPortafolio = False
                        End If
                    End If

                    If logCambiarConsultaSaldo Then
                        If pobjOrdenSelected.TipoOperacion = TIPOOPERACION_COMPRA And
                        Not String.IsNullOrEmpty(pobjOrdenSelected.IDComitente) Then
                            If ConsultarSaldo Then
                                ConsultarSaldo = False
                            End If
                            ConsultarSaldo = True
                        Else
                            ConsultarSaldo = False
                        End If
                    End If

                    If logCambiarConsultaOperaciones Then
                        If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) And
                       (Not String.IsNullOrEmpty(pobjOrdenSelected.IDComitente) Or
                        Not String.IsNullOrEmpty(pobjOrdenSelected.Especie)) Then
                            If ConsultarOperaciones Then
                                ConsultarOperaciones = False
                            End If
                            ConsultarOperaciones = True
                        Else
                            ConsultarOperaciones = False
                        End If
                    End If

                Else
                    ConsultarOrdenesSAE = False
                    ConsultarSaldo = False
                    ConsultarPortafolio = False
                    ConsultarOperaciones = False
                End If
            Else
                ConsultarOrdenesSAE = False
                ConsultarSaldo = False
                ConsultarPortafolio = False
                ConsultarOperaciones = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al habilitar la busqueda en los controles dependiendo del tipo de orden.", Me.ToString, "HabilitarConsultaControles", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Desarrollado por Juan David Correa.
    ''' Consultar los ordenantes del comitente asociado a la orden de OYDPLUS
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    Private Sub consultarOrdenantesOYDPLUS(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                If Not IsNothing(mdcProxyUtilidad01.BuscadorOrdenantes) Then
                    mdcProxyUtilidad01.BuscadorOrdenantes.Clear()
                End If

                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarOrdenantesComitenteQuery(strClienteABuscar, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenantes, pstrUserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los ordenantes del cliente.", Me.ToString, "consultarOrdenantesOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Desarrollado por Juan David Correa.
    ''' Consultar las cuentas depósito del comitente asociado a la orden de OYDPLUS
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    Private Sub consultarCuentasDepositoOYDPLUS(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                If Not IsNothing(mdcProxyUtilidad02.BuscadorCuentasDepositos) Then
                    mdcProxyUtilidad02.BuscadorCuentasDepositos.Clear()
                End If

                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarCuentasDepositoComitenteQuery(strClienteABuscar, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDeposito, pstrUserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar las cuentas deposito del cliente.", Me.ToString, "consultarCuentasDepositoOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Desarrollado por Juan David Correa.
    ''' Se realizan las validaciones para el guardado de la orden de oydplus.
    ''' Fecha 27 de agosto del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Function ValidarGuardadoOrden(ByVal pobjOrden As OyDPLUSOrdenesOF.OrdenOYDPLUSOF) As Boolean
        Try
            'Valida los campos que son requeridos por el sistema de OYDPLUS.
            Dim logTieneError As Boolean = False
            strMensajeValidacion = String.Empty

            If Not IsNothing(pobjOrden) Then
                'Valida el campo de Receptor
                If logNuevoRegistro Then
                    If IsNothing(pobjOrden.Receptor) Or String.IsNullOrEmpty(pobjOrden.Receptor) Then
                        strMensajeValidacion = String.Format("{0}{1} - Receptor", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If

                'Valida el campo de Tipo de Orden
                If IsNothing(pobjOrden.TipoOrden) Or String.IsNullOrEmpty(pobjOrden.TipoOrden) Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo orden", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Tipo de producto
                If IsNothing(pobjOrden.TipoProducto) Or String.IsNullOrEmpty(pobjOrden.TipoProducto) Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo producto", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Tipo de negocio
                If IsNothing(pobjOrden.TipoNegocio) Or String.IsNullOrEmpty(pobjOrden.TipoNegocio) Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo negocio", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Tipo de operación
                If IsNothing(pobjOrden.TipoOperacion) Or String.IsNullOrEmpty(pobjOrden.TipoOperacion) Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo operación", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                'Valida el campo de Clasificación
                If IsNothing(pobjOrden.Clasificacion) Or String.IsNullOrEmpty(pobjOrden.Clasificacion) Then
                    strMensajeValidacion = String.Format("{0}{1} - Clasificación", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de tipo limite
                If IsNothing(pobjOrden.TipoLimite) Or String.IsNullOrEmpty(pobjOrden.TipoLimite) Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo limite", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de las condiciones de la negociación
                If IsNothing(pobjOrden.CondicionesNegociacion) Or String.IsNullOrEmpty(pobjOrden.CondicionesNegociacion) Then
                    strMensajeValidacion = String.Format("{0}{1} - Condiciones negociación", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de la forma de pago
                If IsNothing(pobjOrden.FormaPago) Or String.IsNullOrEmpty(pobjOrden.FormaPago) Then
                    strMensajeValidacion = String.Format("{0}{1} - Forma de pago", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de la especie
                If IsNothing(pobjOrden.Especie) Or String.IsNullOrEmpty(pobjOrden.Especie) Or pobjOrden.Especie = "(No Seleccionado)" Then
                    strMensajeValidacion = String.Format("{0}{1} - Especie", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo del cliente
                If IsNothing(pobjOrden.IDComitente) Or String.IsNullOrEmpty(pobjOrden.IDComitente) Or pobjOrden.IDComitente = "(No Seleccionado)" Then
                    strMensajeValidacion = String.Format("{0}{1} - Cliente", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo del ordenante
                If IsNothing(pobjOrden.IDOrdenante) Or String.IsNullOrEmpty(pobjOrden.IDOrdenante) Then
                    strMensajeValidacion = String.Format("{0}{1} - Ordenante", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo del cuenta deposito
                If IsNothing(pobjOrden.CuentaDeposito) Or pobjOrden.CuentaDeposito = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Cuenta deposito", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de las duración
                If pobjOrden.TipoNegocio <> TIPONEGOCIO_ACCIONESOF And pobjOrden.TipoNegocio <> TIPONEGOCIO_RENTAFIJAOF Then
                    If IsNothing(pobjOrden.Duracion) Or String.IsNullOrEmpty(pobjOrden.Duracion) Then
                        strMensajeValidacion = String.Format("{0}{1} - Duración", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If

                'Valida el campo del tipo de inversión
                If pobjOrden.TipoProducto = TIPOPRODUCTO_CUENTAPROPIA Then
                    If IsNothing(pobjOrden.TipoInversion) Or String.IsNullOrEmpty(pobjOrden.TipoInversion) Then
                        strMensajeValidacion = String.Format("{0}{1} - Tipo inversión", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If

                'Valida el campos dependiendo de la clase de la orden Acciones o Renta Fija
                If Not String.IsNullOrEmpty(pobjOrden.TipoNegocio) And Not String.IsNullOrEmpty(pobjOrden.TipoOperacion) Then
                    If pobjOrden.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then
                        'Valida el campo de la cantidad
                        If IsNothing(pobjOrden.Cantidad) Or pobjOrden.Cantidad = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Cantidad", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                        'Valida el campo precio
                        If (IsNothing(pobjOrden.Precio) Or pobjOrden.Precio = 0) Then
                            strMensajeValidacion = String.Format("{0}{1} - Precio", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    ElseIf pobjOrden.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJAOF Then

                        'Valida el campo de la cantidad
                        If IsNothing(pobjOrden.Cantidad) Or pobjOrden.Cantidad = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Nominal", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If

                        'Valida el campo precio
                        If IsNothing(pobjOrden.Precio) Or pobjOrden.Precio = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Precio", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If

                        'Valida el campo tasa de comprador vendedor
                        If IsNothing(pobjOrden.TasaNominal) Or pobjOrden.TasaNominal = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Tasa efectiva", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    End If
                End If
            Else
                mostrarMensaje("Señor Usuario, la orden tiene que tener un dato como minimo.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If logTieneError Then

                strMensajeValidacion = String.Format("Señor usuario los siguientes campos son requeridos:{0}", strMensajeValidacion)
                mostrarMensaje(strMensajeValidacion, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else
                'Valida la fecha de vigencia no sea mayor al parametro de diascancelación
                'Valida la fecha de vigencia no sea mayor al parametro de diascancelación
                Dim diasCancelacion As Integer = ObtenerDiasCancelacion()

                If pobjOrden.FechaVigencia.Value.Date > DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, diasCancelacion, dtmFechaServidor.Date)) Then
                    mostrarMensaje(String.Format("La fecha de vigencia no puede ser una fecha mayor a ({0:dd/MM/yyyy}) ya que los días maximos de cancelación de la orden son ({1}).", DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, diasCancelacion, dtmFechaServidor.Date)), diasCancelacion), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                End If

                'Valida la distribución de comisiones
                Dim objListaReceptores As List(Of OyDPLUSOrdenesOF.ReceptoresOrden)

                objListaReceptores = _ListaReceptoresOrdenes

                If Not IsNothing(objListaReceptores) Then
                    Dim sumarPorcentaje As Double = 0
                    Dim intNumeroLideres As Integer = 0
                    Dim logReceptorVacion As Boolean = False
                    For Each li In objListaReceptores
                        If li.Lider Then
                            intNumeroLideres += 1
                        End If
                        If String.IsNullOrEmpty(li.IDReceptor) Then
                            logReceptorVacion = True
                        End If
                        sumarPorcentaje += li.Porcentaje
                    Next

                    If logReceptorVacion Then
                        strMensajeValidacion = "Hay algunos registros de los receptores de la distrubución de comisiones que se encuentran vacios."
                        logTieneError = True
                    End If

                    If intNumeroLideres = 0 Then
                        strMensajeValidacion = "Tiene que haber como minimo un receptor lider en la distribución de comisiones"
                        logTieneError = True
                    ElseIf intNumeroLideres > 1 Then
                        strMensajeValidacion = "Solo puede haber un receptor lider en la distribución de comisiones"
                        logTieneError = True
                    Else
                        If sumarPorcentaje <> 100 Then
                            strMensajeValidacion = "El porcentaje en la distribución de comisiones debe de sumar 100%"
                            logTieneError = True
                        End If
                    End If
                Else
                    strMensajeValidacion = "Debe de ingresar la distribución de comisiones"
                    IsBusy = False
                    logTieneError = True
                End If

                If logTieneError Then
                    mostrarMensaje(strMensajeValidacion, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                End If

                If logTieneError Then
                    mostrarMensaje(strMensajeValidacion, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                Else
                    'Valida el valor de la orden que no sea cero.
                    If IsNothing(pobjOrden.ValorOrden) And pobjOrden.ValorOrden = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - El valor de la orden no puede estar en cero, ingrese todos los datos para que sea calculado el valor de la orden.", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If

                If logTieneError Then
                    mostrarMensaje(strMensajeValidacion, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                Else
                    'Valida los valores negativos.
                    If pobjOrden.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then
                        'Valida el campo de la cantidad
                        If pobjOrden.Cantidad < 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - La cantidad no puede ser negativa.", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    ElseIf pobjOrden.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJAOF Then

                        'Valida el campo de la cantidad
                        If pobjOrden.Cantidad < 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - El valor Nominal no puede ser negativo.", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                        'Valida el campo de la tasa nominal
                        If pobjOrden.TasaNominal < 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - La Tasa Nominal no puede ser negativo.", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If

                        'Valida el campo tasa de comprador vendedor
                        If pobjOrden.PrecioMaximoMinimo > 0 Then

                            If pobjOrden.Precio > pobjOrden.PrecioMaximoMinimo Then
                                strMensajeValidacion = String.Format("{0}{1} -El precio no puede ser menor al precio inferior.", strMensajeValidacion, vbCrLf)
                                logTieneError = True
                            End If
                        End If
                    End If
                End If

                If logTieneError Then
                    mostrarMensaje(strMensajeValidacion, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                Else
                    'Valida los precios.
                    If pobjOrden.TipoNegocio = TIPONEGOCIO_ACCIONES Or
                        pobjOrden.TipoNegocio = TIPONEGOCIO_TTV Then
                        If pobjOrden.TipoOperacion = TIPOOPERACION_COMPRA Then
                            If pobjOrden.PrecioMaximoMinimo > 0 Then
                                If pobjOrden.Precio > pobjOrden.PrecioMaximoMinimo Then
                                    strMensajeValidacion = String.Format("{0}{1} - Precio no puede ser mayor que el precio maximo", strMensajeValidacion, vbCrLf)
                                    logTieneError = True
                                End If
                            End If
                        Else
                            If pobjOrden.PrecioMaximoMinimo > 0 Then
                                If pobjOrden.Precio < pobjOrden.PrecioMaximoMinimo Then
                                    strMensajeValidacion = String.Format("{0}{1} - Precio no puede ser menor que el precio minimo", strMensajeValidacion, vbCrLf)
                                    logTieneError = True
                                End If
                            End If
                        End If
                    End If

                    If pobjOrden.FechaVigencia <= dtmFechaServidor And pobjOrden.TipoNegocio <> TIPONEGOCIO_ACCIONESOF And pobjOrden.TipoNegocio <> TIPONEGOCIO_RENTAFIJAOF Then
                        'Santiago Alexander Vergara Orrego - Mayo 27/2014 - Se añade la condición para que sólo se valide la fecha de vigencia cuando el tipo de negocio no sea de otras firmas
                        strMensajeValidacion = String.Format("{0}{1} - La fecha de vigencia de la orden no puede ser menor a la fecha actual.", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If

                    If Not IsNothing(_ListaReceptoresOrdenes) Then
                        If _ListaReceptoresOrdenes.Count > intCantidadMaximaDetalles Then
                            strMensajeValidacion = String.Format("{0}{1} - Señor Usuario, la cantidad máxima de registros permitida por detalle es ({2}), por favor valide el detalle de receptores orden.", strMensajeValidacion, vbCrLf, intCantidadMaximaDetalles)
                            logTieneError = True
                        End If
                    End If

                    If logTieneError Then
                        mostrarMensaje(strMensajeValidacion, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return False
                    Else
                        strMensajeValidacion = String.Empty
                        Return True
                    End If

                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la valición de la orden.", Me.ToString, "ValidarGuardadoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
            Return False
        End Try
    End Function

    Private Function ValidarCampoEnDiccionario(ByVal pstrOpcionDiccionario As String, ByVal pstrValor As String) As Boolean
        Dim logRetorno As Boolean = False
        Try
            If Not IsNothing(_DiccionarioCombosOYDPlus) Then
                If _DiccionarioCombosOYDPlus.ContainsKey(pstrOpcionDiccionario) Then
                    If Not IsNothing(_DiccionarioCombosOYDPlus(pstrOpcionDiccionario)) Then
                        If _DiccionarioCombosOYDPlus(pstrOpcionDiccionario).Where(Function(i) i.Retorno = pstrValor).Count > 0 Then
                            logRetorno = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la valición de que el valor exista en el diccionario.", Me.ToString, "ValidarCampoEnDiccionario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
        Return logRetorno
    End Function

    ''' <summary>
    ''' Desarrollado por Juan David Correa
    ''' Se crea un metodo para ubicarse en la opción o campo que se encuentra vacio.
    ''' Fecha 27 de agosto del 2012
    ''' </summary>
    ''' <param name="pViewOrdenes"></param>
    ''' <param name="pstrOpcion"></param>
    ''' <remarks></remarks>
    Public Sub BuscarControlValidacion(ByVal pViewOrdenes As OrdenesPLUSOFView, ByVal pstrOpcion As String)
        Try
            'If Not IsNothing(pViewOrdenes) Then
            '    If TypeOf pViewOrdenes.df.FindName(pstrOpcion) Is TabItem Then
            '        CType(pViewOrdenes.df.FindName(pstrOpcion), TabItem).IsSelected = True
            '    ElseIf TypeOf pViewOrdenes.df.FindName(pstrOpcion) Is TextBox Then
            '        CType(pViewOrdenes.df.FindName(pstrOpcion), TextBox).Focus()
            '    ElseIf TypeOf pViewOrdenes.df.FindName(pstrOpcion) Is ComboBox Then
            '        CType(pViewOrdenes.df.FindName(pstrOpcion), ComboBox).Focus()
            '    ElseIf TypeOf pViewOrdenes.df.FindName(pstrOpcion) Is C1.Silverlight.C1ComboBox Then
            '        CType(pViewOrdenes.df.FindName(pstrOpcion), C1.Silverlight.C1ComboBox).Focus()
            '    ElseIf TypeOf pViewOrdenes.df.FindName(pstrOpcion) Is A2Utilidades.A2NumericBox Then
            '        CType(pViewOrdenes.df.FindName(pstrOpcion), A2Utilidades.A2NumericBox).Focus()
            '    End If
            'End If
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
    ''' Metodo para consultar los receptores de la orden
    ''' </summary>
    ''' <param name="pstrClaseOrden"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pintNroOrden"></param>
    ''' <param name="pintVersion"></param>
    ''' <param name="pstrUserState"></param>
    ''' <remarks></remarks>
    Private Sub consultarReceptoresOrden(ByVal pstrClaseOrden As String, ByVal pstrTipo As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrEstado As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(dcProxy.ReceptoresOrdens) Then
                dcProxy.ReceptoresOrdens.Clear()
            End If
            dcProxy.Load(dcProxy.Traer_ReceptoresOrdenes_OrdenQuery(pstrClaseOrden, pstrTipo, pintNroOrden, pintVersion, Program.Usuario, pstrEstado, _OrdenOYDPLUSSelected.TipoNegocio, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar los receptores de la orden.", Me.ToString, "consultarReceptoresOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para consultar los receptores del comitente seleccionado.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 28 de agosto del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub consultarReceptoresComitente(ByVal pstrClaseOrden As String, ByVal pstrTipo As String, ByVal pstrComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(dcProxy.ReceptoresOrdens) Then
                dcProxy.ReceptoresOrdens.Clear()
            End If

            Dim strClienteABuscar = Right(Space(17) & pstrComitente, MINT_LONG_MAX_CODIGO_OYD)

            dcProxy.Load(dcProxy.Traer_ReceptoresOrdenes_ClienteQuery(pstrClaseOrden, pstrTipo, strClienteABuscar, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar los receptores del comitente.", Me.ToString, "consultarReceptoresComitente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Carcula la fecha de vigencia de la orden dependiendo de la duración.
    ''' Desarrollado por Juan David Correa
    ''' Fecha Agosto 29 del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CalcularFechaVigenciaOrden(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                If Not IsNothing(pobjOrdenSelected.Duracion) Then
                    If pobjOrdenSelected.TipoNegocio = TIPONEGOCIO_ACCIONESOF Or pobjOrdenSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJAOF Then
                        pobjOrdenSelected.Duracion = Nothing
                        HabilitarFechaVigencia = True
                        HabilitarHoraVigencia = False
                        'pobjOrdenSelected.Dias = 1
                        'pobjOrdenSelected.FechaVigencia = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
                    ElseIf pobjOrdenSelected.Duracion.ToUpper = DURACION_DIA Or
                pobjOrdenSelected.Duracion.ToUpper = DURACION_INMEDIATA Or
                pobjOrdenSelected.Duracion.ToUpper = DURACION_SESSION Then
                        HabilitarFechaVigencia = False
                        HabilitarHoraVigencia = False
                        pobjOrdenSelected.Dias = 1
                        pobjOrdenSelected.FechaVigencia = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
                    ElseIf pobjOrdenSelected.Duracion.ToUpper = DURACION_HASTAHORA Then
                        HabilitarFechaVigencia = False
                        HabilitarHoraVigencia = True
                        pobjOrdenSelected.Dias = 1
                        pobjOrdenSelected.FechaVigencia = DateAdd(DateInterval.Hour, 1, dtmFechaServidor)
                    ElseIf pobjOrdenSelected.Duracion.ToUpper = DURACION_CANCELACION Then
                        'Obtener los dias de cancelación de la Orden.
                        Dim DiasCancelacion As Integer = 0
                        DiasCancelacion = ObtenerDiasCancelacion()

                        HabilitarFechaVigencia = False
                        HabilitarHoraVigencia = False
                        pobjOrdenSelected.Dias = DiasCancelacion
                        pobjOrdenSelected.FechaVigencia = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, DiasCancelacion, dtmFechaServidor.Date))
                    ElseIf pobjOrdenSelected.Duracion.ToUpper = DURACION_FECHA Then
                        HabilitarFechaVigencia = True
                        HabilitarHoraVigencia = False
                        pobjOrdenSelected.Dias = 1
                        pobjOrdenSelected.FechaVigencia = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
                    End If
                Else
                    If pobjOrdenSelected.TipoNegocio = TIPONEGOCIO_ACCIONESOF Or pobjOrdenSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJAOF Then
                        HabilitarFechaVigencia = True
                        HabilitarHoraVigencia = False
                        'pobjOrdenSelected.Dias = 1
                        'pobjOrdenSelected.FechaVigencia = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al calcular la fecha de vigencia de la orden.", Me.ToString, "CalcularFechaVigenciaOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Function ObtenerDiasCancelacion() As Integer
        Try
            Dim Dias As Integer = 0

            If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
                If DiccionarioCombosOYDPlusCompleta("DIASCANCELACION").Count > 0 Then
                    Try
                        Dias = Convert.ToInt32(DiccionarioCombosOYDPlusCompleta("DIASCANCELACION").FirstOrDefault.Retorno)
                    Catch ex As Exception
                        Dias = 30
                    End Try
                End If
            Else
                Dias = 30
            End If

            Return Dias
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los días de vigencia", Me.ToString(), "ObtenerDiasCancelacion", Program.TituloSistema, Program.Maquina, ex)
            Return 30
        End Try
    End Function

    ''' <summary>
    ''' Metodo para calcular los dias de vigencia de la orden.
    ''' Desarrollado por Juan David Correa
    ''' Fecha Agosto 29 del 2012
    ''' </summary>
    ''' <param name="pstrTipoCalculo"></param>
    ''' <param name="pintDias"></param>
    ''' <param name="plogGuardarOrden"></param>
    ''' <remarks></remarks>
    Public Sub CalcularDiasOrdenOYDPLUS(ByVal pstrTipoCalculo As String, ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, Optional ByVal pintDias As Integer = -1, Optional ByVal plogGuardarOrden As Boolean = False, Optional ByVal plogNuevoRegistro As Boolean = False)
        Try
            If (logNuevoRegistro Or logEditarRegistro) Then
                If IsNothing(pobjOrdenSelected) Then
                    Exit Sub
                ElseIf pobjOrdenSelected.FechaVigencia <= dtmFechaServidor And pobjOrdenSelected.TipoNegocio <> TIPONEGOCIO_ACCIONESOF And pobjOrdenSelected.TipoNegocio <> TIPONEGOCIO_RENTAFIJAOF Then
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
            Dim FechaInicial As Date = dtmFechaServidor.Date
            Dim FechaFinal As Date = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))

            If pintDias <= 0 Then
                ' Calcular los días al vencimiento de la orden a partir de la fecha de elaboración y vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                    FechaInicial = pobjOrdenSelected.FechaEmision.Value.Date
                    FechaFinal = pobjOrdenSelected.FechaVencimiento.Value.Date

                    dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, FechaInicial, FechaFinal, -1, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_CALCULAR_DIAS)
                Else
                    If plogNuevoRegistro Then
                        FechaInicial = dtmFechaServidor
                        FechaFinal = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
                    Else
                        If Not IsNothing(pobjOrdenSelected) Then
                            FechaInicial = pobjOrdenSelected.FechaOrden
                            FechaFinal = pobjOrdenSelected.FechaVigencia.Value.Date
                        Else
                            FechaInicial = dtmFechaServidor
                            FechaFinal = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
                        End If
                    End If

                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, FechaInicial, FechaFinal, -1, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_VALIDACION_GUARDADO_ORDEN)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, FechaInicial, FechaFinal, -1, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_CALCULAR_DIAS)
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
    ''' Metodo para validar el valor de la orden, sí es un repo se calcula el valor del repo.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 02 de Octubre del 2012
    ''' </summary>
    Public Async Function CalcularValorOrden(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, Optional ByVal pobjTipoModificacion As String = "") As System.Threading.Tasks.Task
        Try
            If Not IsNothing(pobjOrdenSelected) Then

                If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) And Not String.IsNullOrEmpty(pobjOrdenSelected.TipoOperacion) Then
                    logCalcularValores = False

                    If Await ObtenerCalculosMotor(pobjOrdenSelected) = False Then

                    End If

                    logCalcularValores = True
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor de la orden.", Me.ToString(), "CalcularValorOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try

        If logCalcularValores = False Then
            logCalcularValores = True
        End If
    End Function

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

    Private Async Function ObtenerCalculosMotor(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF) As System.Threading.Tasks.Task(Of Boolean)
        Dim logLlamadoExitoso As Boolean = False
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) And Not String.IsNullOrEmpty(pobjOrdenSelected.TipoOperacion) Then
                    Dim objCalculos As New clsMotorCliente(STR_URLMOTORCALCULOS)
                    Dim objListaParametros As New Dictionary(Of String, String)
                    objListaParametros.Add("VERSION", Await objCalculos.ObtenerVersionTaskAsync(HOJA_MOTORCALCULOS))
                    If LOG_HACERLOGMOTORCALCULOS Then
                        objListaParametros.Add("RUTADEBUG", STR_RUTALOGMOTORCALCULOS)
                    End If

                    Dim objListaValoresEntrada As Dictionary(Of String, String) = ArmarEntradaMotorCalculos(pobjOrdenSelected)
                    Dim objListaValoresSalida As List(Of String) = ArmarSalidasMotorCalculos(pobjOrdenSelected)


                    If Not IsNothing(objListaValoresEntrada) And Not IsNothing(objListaValoresSalida) Then
                        Dim objDiccionarioRespuesta As Dictionary(Of String, String) = Await objCalculos.ProcesarTaskAsync(HOJA_MOTORCALCULOS, objListaValoresEntrada, objListaValoresSalida, objListaParametros)

                        If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) And Not String.IsNullOrEmpty(pobjOrdenSelected.TipoOperacion) Then
                            If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then 'ACCIONES OF
                                If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                                    pobjOrdenSelected.ValorOrden = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_ACCIONESOFVENTA.SALIDA_ACCIONESOFVENTA_VALORORDEN.ToString, 2)

                                Else 'COMPRA

                                    pobjOrdenSelected.ValorOrden = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_ACCIONESOFCOMPRA.SALIDA_ACCIONESOFCOMPRA_VALORORDEN.ToString, 2)

                                End If
                            ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJAOF Then 'RENTA FIJA OF
                                If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                                    pobjOrdenSelected.ValorOrden = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_RENTAFIJAOFVENTA.SALIDA_RENTAFIJAOFVENTA_VALORORDEN.ToString, 2)

                                Else 'COMPRA

                                    pobjOrdenSelected.ValorOrden = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_RENTAFIJAOFCOMPRA.SALIDA_RENTAFIJAOFCOMPRA_VALORORDEN.ToString, 2)

                                End If
                            End If

                            logLlamadoExitoso = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor en el motor de calculos.", Me.ToString(), "ObtenerCalculosMotor", Program.TituloSistema, Program.Maquina, ex)
        End Try

        Return logLlamadoExitoso
    End Function

    Private Function ArmarEntradaMotorCalculos(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF) As Dictionary(Of String, String)
        Try
            Dim strValorRetorno As New Dictionary(Of String, String)

            If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) And Not String.IsNullOrEmpty(pobjOrdenSelected.TipoOperacion) Then
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.STR_ENCABEZADO_TIPOORDEN.ToString, VerificarValorEntrada(pobjOrdenSelected.TipoOrden))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.STR_ENCABEZADO_TIPONEGOCIO.ToString, VerificarValorEntrada(pobjOrdenSelected.TipoNegocio))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.STR_ENCABEZADO_TIPOPRODUCTO.ToString, VerificarValorEntrada(pobjOrdenSelected.TipoProducto))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.STR_ENCABEZADO_TIPOOPERACION.ToString, VerificarValorEntrada(pobjOrdenSelected.TipoOperacion))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.STR_ENCABEZADO_ESPECIE.ToString, VerificarValorEntrada(pobjOrdenSelected.Especie))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.STR_ENCABEZADO_MODALIDAD.ToString, VerificarValorEntrada(pobjOrdenSelected.Modalidad))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.STR_ENCABEZADO_INDICADOR.ToString, VerificarValorEntrada(pobjOrdenSelected.Indicador))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_PUNTOSINDICADOR.ToString, VerificarValorEntrada(pobjOrdenSelected.PuntosIndicador, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.STR_ENCABEZADO_ENPESOS.ToString, VerificarValorEntrada(pobjOrdenSelected.EnPesos))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_CANTIDAD.ToString, VerificarValorEntrada(pobjOrdenSelected.Cantidad, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_PRECIO.ToString, VerificarValorEntrada(pobjOrdenSelected.Precio, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_PRECIOMAXIMONIMINO.ToString, VerificarValorEntrada(pobjOrdenSelected.PrecioMaximoMinimo, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_VALORCAPTACION.ToString, VerificarValorEntrada(pobjOrdenSelected.ValorCaptacionGiro, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_VALORFUTUROREPO.ToString, VerificarValorEntrada(pobjOrdenSelected.ValorFuturoRepo, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_TASAREGISTRO.ToString, VerificarValorEntrada(pobjOrdenSelected.TasaRegistro, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_TASACLIENTE.ToString, VerificarValorEntrada(pobjOrdenSelected.TasaCliente, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_TASANOMINAL.ToString, VerificarValorEntrada(pobjOrdenSelected.TasaNominal, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_CASTIGO.ToString, VerificarValorEntrada(pobjOrdenSelected.Castigo, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_VALORACCION.ToString, VerificarValorEntrada(pobjOrdenSelected.ValorAccion, 4, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_COMISION.ToString, VerificarValorEntrada(pobjOrdenSelected.Comision, 3, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_VALORCOMISION.ToString, VerificarValorEntrada(pobjOrdenSelected.ValorComision, 3, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_VALORORDEN.ToString, VerificarValorEntrada(pobjOrdenSelected.ValorOrden, 3, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.INT_ENCABEZADO_DIASREPO.ToString, VerificarValorEntrada(pobjOrdenSelected.DiasRepo, "0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_ENCABEZADO_IVACOMISION.ToString, VerificarValorEntrada(pobjOrdenSelected.IvaComision, 3, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_VALOR_IVA.ToString, VerificarValorEntrada(dblValorIva, 0, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.INT_ENCABEZADO_DIASVIGENCIA.ToString, VerificarValorEntrada(pobjOrdenSelected.Dias, 0, "0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_VALOR_BASE.ToString, VerificarValorEntrada(dblValorBase, 0, "0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS.DBL_VALOR_BASE_REPO.ToString, VerificarValorEntrada(dblValorBaseRepo, 0, "0"))
                Return strValorRetorno
            Else
                Return Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para construir los parametros de entrada del procedimiento.", Me.ToString(), "ArmarEntradaMotorCalculos", Program.TituloSistema, Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

    Private Function ArmarSalidasMotorCalculos(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF) As List(Of String)
        Try
            Dim objListaRetorno As New List(Of String)

            If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) And Not String.IsNullOrEmpty(pobjOrdenSelected.TipoOperacion) Then
                If pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONES Then 'ACCIONES

                    If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESVENTA.SALIDA_ACCIONESVENTA_COMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESVENTA.SALIDA_ACCIONESVENTA_VALORCOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESVENTA.SALIDA_ACCIONESVENTA_IVACOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESVENTA.SALIDA_ACCIONESVENTA_VALORORDEN.ToString)

                    Else 'COMPRA

                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESCOMPRA.SALIDA_ACCIONESCOMPRA_COMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESCOMPRA.SALIDA_ACCIONESCOMPRA_VALORCOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESCOMPRA.SALIDA_ACCIONESCOMPRA_IVACOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESCOMPRA.SALIDA_ACCIONESCOMPRA_VALORORDEN.ToString)

                    End If


                ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJA Then 'RENTA FIJA

                    If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_COMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_IVACOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_TASACLIENTEEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_TASAMERCADOEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_VALORCOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_VALORFURUROMERCADO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_VALORFUTUROCLIENTE.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_VALORGIRO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAVENTA.SALIDA_RENTAFIJAVENTA_VALORORDEN.ToString)

                    Else 'COMPRA

                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_COMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_IVACOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_TASACLIENTEEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_TASAMERCADOEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_VALORCOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_VALORFUTUROCLIENTE.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_VALORFUTUROMERCADO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_VALORGIRO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJACOMPRA.SALIDA_RENTAFIJACOMPRA_VALORORDEN.ToString)

                    End If

                ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_REPO Then 'REPO
                    If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_COMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_IVACOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_NROACCIONES.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_PRECIOCONGARANTIA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_TASACLIENTEEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_TASAMERCADOEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_VALORCAPTACION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_VALORCOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_VALORFUTUROCLIENTE.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_VALORFUTUROMERCADO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOVENTA.SALIDA_REPOVENTA_VALORORDEN.ToString)

                    Else 'COMPRA

                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOCOMPRA.SALIDA_REPOCOMPRA_COMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOCOMPRA.SALIDA_REPOCOMPRA_IVACOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOCOMPRA.SALIDA_REPOCOMPRA_TASACLIENTEEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOCOMPRA.SALIDA_REPOCOMPRA_TASAMERCADOEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOCOMPRA.SALIDA_REPOCOMPRA_VALORCOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOCOMPRA.SALIDA_REPOCOMPRA_VALORFUTUROCLIENTE.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOCOMPRA.SALIDA_REPOCOMPRA_VALORFUTUROMERCADO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_REPOCOMPRA.SALIDA_REPOCOMPRA_VALORORDEN.ToString)

                    End If
                ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_SIMULTANEA Then 'SIMULTANEA

                    If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_COMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_IVACOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_TASACLIENTEEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_TASAMERCADOEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_VALORCOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_VALORFUTUROCLIENTE.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_VALORFUTUROMERCADO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_VALORGIRO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEAVENTA.SALIDA_SIMULTANEAVENTA_VALORORDEN.ToString)

                    Else 'COMPRA

                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_COMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_IVACOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_TASACLIENTEEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_TASAMERCADOEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_VALORCOMISION.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_VALORFUTUROCLIENTE.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_VALORFUTUROMERCADO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_VALORGIRO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_SIMULTANEACOMPRA.SALIDA_SIMULTANEACOMPRA_VALORORDEN.ToString)

                    End If

                ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_TTV Then 'TTV

                    If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVVENTA.SALIDA_TTVVENTA_TASACLIENTEEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVVENTA.SALIDA_TTVVENTA_TASAMERCADOEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVVENTA.SALIDA_TTVVENTA_VALORFUTUROCLIENTE.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVVENTA.SALIDA_TTVVENTA_VALORFUTUROMERCADO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVVENTA.SALIDA_TTVVENTA_VALORGIRO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVVENTA.SALIDA_TTVVENTA_VALORORDEN.ToString)

                    Else 'COMPRA

                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVCOMPRA.SALIDA_TTVCOMPRA_TASACLIENTEEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVCOMPRA.SALIDA_TTVCOMPRA_TASAMERCADOEA.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVCOMPRA.SALIDA_TTVCOMPRA_VALORFUTUROCLIENTE.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVCOMPRA.SALIDA_TTVCOMPRA_VALORFUTUROMERCADO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVCOMPRA.SALIDA_TTVCOMPRA_VALORGIRO.ToString)
                        objListaRetorno.Add(SALIDAS_CAMPOS_TTVCOMPRA.SALIDA_TTVCOMPRA_VALORORDEN.ToString)

                    End If

                ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_ACCIONESOF Then 'ACCIONES OF

                    If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESOFVENTA.SALIDA_ACCIONESOFVENTA_VALORORDEN.ToString)

                    Else 'COMPRA

                        objListaRetorno.Add(SALIDAS_CAMPOS_ACCIONESOFCOMPRA.SALIDA_ACCIONESOFCOMPRA_VALORORDEN.ToString)

                    End If

                ElseIf pobjOrdenSelected.TipoNegocio.ToUpper = TIPONEGOCIO_RENTAFIJAOF Then 'RENTA FIJA OF

                    If pobjOrdenSelected.TipoOperacion.ToUpper = TIPOOPERACION_VENTA Then 'VENTA

                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAOFVENTA.SALIDA_RENTAFIJAOFVENTA_VALORORDEN.ToString)

                    Else 'COMPRA

                        objListaRetorno.Add(SALIDAS_CAMPOS_RENTAFIJAOFCOMPRA.SALIDA_RENTAFIJAOFCOMPRA_VALORORDEN.ToString)

                    End If

                End If

                Return objListaRetorno
            Else
                Return Nothing
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener las salidas del motor.", Me.ToString(), "ArmarSalidasMotorCalculos", Program.TituloSistema, Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

    Private Function VerificarValorEntrada(ByVal pobjValor As Object, Optional ByVal pintNroDecimales As Integer = 0, Optional ByVal pstrValorDefecto As String = "") As String
        Dim objRetorno As String = String.Empty
        Try
            If TypeOf pobjValor Is Double Or TypeOf pobjValor Is Double? Then
                If IsNothing(pobjValor) Then
                    objRetorno = "0.0"
                Else
                    objRetorno = Math.Round(CDbl(pobjValor), pintNroDecimales).ToString().Replace(",", ".")
                End If
            ElseIf TypeOf pobjValor Is Integer Or TypeOf pobjValor Is Integer? Then
                If IsNothing(pobjValor) Then
                    objRetorno = "0"
                Else
                    objRetorno = CInt(pobjValor).ToString
                End If
            ElseIf TypeOf pobjValor Is Boolean Or TypeOf pobjValor Is Boolean? Then
                If IsNothing(pobjValor) Then
                    objRetorno = "0"
                Else
                    objRetorno = IIf(CBool(pobjValor), 1, 0).ToString
                End If
            ElseIf TypeOf pobjValor Is Date Or TypeOf pobjValor Is Date? Or TypeOf pobjValor Is DateTime Or TypeOf pobjValor Is DateTime? Then
                If IsNothing(pobjValor) Then
                    objRetorno = "NULL"
                Else
                    objRetorno = String.Format("'{0:yyyy/MM/dd hh:mm:ss}'", pobjValor)
                End If
            Else
                If IsNothing(pobjValor) Then
                    If Not String.IsNullOrEmpty(pstrValorDefecto) Then
                        objRetorno = pstrValorDefecto
                    Else
                        objRetorno = ""
                    End If
                Else
                    objRetorno = pobjValor
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar el valor de entrada.", Me.ToString(), "VerificarValorEntrada", Program.TituloSistema, Program.Maquina, ex)
        End Try
        Return objRetorno

    End Function

    Private Function RetornarValorEntrada(ByVal pstrValorRetorno As String, ByVal pintOpcion As ENTRADAS_MOTORCALCULOS, ByVal pobjValor As Object, Optional ByVal pintNroDecimales As Integer = 0) As String
        Try
            Dim strValor As String = VerificarValorEntrada(pobjValor, pintNroDecimales)
            If String.IsNullOrEmpty(pstrValorRetorno) Then
                Return String.Format("{0}={1}", pintOpcion.ToString, strValor)
            Else
                Return String.Format("{0}&{1}={2}", pstrValorRetorno, pintOpcion.ToString, strValor)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retornar el valor de entrada.", Me.ToString(), "RetornarValorEntrada", Program.TituloSistema, Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

    Private Function RetornarValorSalida(ByVal pobjListaDiccionario As Dictionary(Of String, String), ByVal pstrOpcion As String, ByVal pintNroDecimales As Integer) As Double
        Dim dblValorRetorno As Double = 0
        Try
            Dim strCampo As String = String.Empty
            Dim strValor As String = String.Empty

            If Not IsNothing(pobjListaDiccionario) Then
                If pobjListaDiccionario.ContainsKey(pstrOpcion) Then
                    strValor = pobjListaDiccionario(pstrOpcion)
                    Try
                        dblValorRetorno = Math.Round(CDbl(strValor), pintNroDecimales)
                    Catch ex As Exception
                        dblValorRetorno = 0
                    End Try
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retornar el valor de salida.", Me.ToString(), "RetornarValorSalida", Program.TituloSistema, Program.Maquina, ex)
        End Try

        Return dblValorRetorno
    End Function

    ''' <summary>
    ''' Metodo para obtener la información completa de los combos de la aplicación cuando sea necesario.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 10 de Septiembre del 2012
    ''' </summary>
    Public Sub ObtenerInformacionCombosCompletos()
        Try
            Dim objDiccionarioCombosOYDPlus As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
            Dim objTipoNegocioReceptor As New List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor)
            Dim strNombreCategoria As String = String.Empty

            objTipoNegocioReceptor = ListaTipoNegocioCOMPLETOS.OrderBy(Function(i) i.ID).ToList

            For Each dic In DiccionarioCombosOYDPlusCompleta
                strNombreCategoria = dic.Key
                objDiccionarioCombosOYDPlus.Add(strNombreCategoria, dic.Value)
            Next

            DiccionarioCombosOYDPlus = Nothing
            DiccionarioCombosOYDPlus = objDiccionarioCombosOYDPlus

            ListaReceptoresUsuario = Nothing
            ListaReceptoresUsuario = ListaReceptoresCompleta.OrderBy(Function(i) i.ID).ToList

            ListaTipoNegocio = Nothing
            ListaTipoNegocio = objTipoNegocioReceptor

            ObtenerValoresCombos(True, _OrdenOYDPLUSSelected)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Obtener la información de los combos.", Me.ToString, "ObtenerInformacionCombosCompletos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Desarrollado por Juan David Correa.
    ''' Obtener el registro anterior de la orden.
    ''' Fecha: Febrero 25 del 2013
    ''' </summary>
    ''' <param name="pobjOrden"></param>
    ''' <param name="pobjOrdenSalvarDatos"></param>
    ''' <remarks></remarks>
    Public Sub ObtenerValoresOrdenAnterior(ByVal pobjOrden As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, ByRef pobjOrdenSalvarDatos As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            If Not IsNothing(pobjOrden) Then
                Dim objNewOrdenOYD As New OyDPLUSOrdenesOF.OrdenOYDPLUSOF

                objNewOrdenOYD.IDNroOrden = pobjOrden.IDNroOrden
                objNewOrdenOYD.NroOrden = pobjOrden.NroOrden
                objNewOrdenOYD.Version = pobjOrden.Version
                objNewOrdenOYD.Bolsa = pobjOrden.Bolsa
                objNewOrdenOYD.NombreBolsa = pobjOrden.NombreBolsa
                objNewOrdenOYD.Receptor = pobjOrden.Receptor
                objNewOrdenOYD.NombreReceptor = pobjOrden.NombreReceptor
                objNewOrdenOYD.TipoOrden = pobjOrden.TipoOrden
                objNewOrdenOYD.NombreTipoOrden = pobjOrden.NombreTipoOrden
                objNewOrdenOYD.TipoNegocio = pobjOrden.TipoNegocio
                objNewOrdenOYD.NombreTipoNegocio = pobjOrden.NombreTipoNegocio
                objNewOrdenOYD.TipoProducto = pobjOrden.TipoProducto
                objNewOrdenOYD.NombreTipoProducto = pobjOrden.NombreTipoProducto
                objNewOrdenOYD.TipoOperacion = pobjOrden.TipoOperacion
                objNewOrdenOYD.NombreTipoOperacion = pobjOrden.NombreTipoOperacion
                objNewOrdenOYD.Clase = pobjOrden.Clase
                objNewOrdenOYD.FechaOrden = pobjOrden.FechaOrden
                objNewOrdenOYD.EstadoOrden = pobjOrden.EstadoOrden
                objNewOrdenOYD.NombreEstadoOrden = pobjOrden.NombreEstadoOrden
                objNewOrdenOYD.EstadoLEO = pobjOrden.EstadoLEO
                objNewOrdenOYD.NombreEstadoLEO = pobjOrden.NombreEstadoLEO
                objNewOrdenOYD.NroOrdenSAE = pobjOrden.NroOrdenSAE
                objNewOrdenOYD.EstadoSAE = pobjOrden.EstadoSAE
                objNewOrdenOYD.NombreEstadoSAE = pobjOrden.NombreEstadoSAE
                objNewOrdenOYD.Clasificacion = pobjOrden.Clasificacion
                objNewOrdenOYD.TipoLimite = pobjOrden.TipoLimite
                objNewOrdenOYD.Duracion = pobjOrden.Duracion
                objNewOrdenOYD.FechaVigencia = pobjOrden.FechaVigencia
                objNewOrdenOYD.HoraVigencia = pobjOrden.HoraVigencia
                objNewOrdenOYD.Dias = pobjOrden.Dias
                objNewOrdenOYD.CondicionesNegociacion = pobjOrden.CondicionesNegociacion
                objNewOrdenOYD.FormaPago = pobjOrden.FormaPago
                objNewOrdenOYD.TipoInversion = pobjOrden.TipoInversion
                objNewOrdenOYD.Ejecucion = pobjOrden.Ejecucion
                objNewOrdenOYD.Mercado = pobjOrden.Mercado
                objNewOrdenOYD.IDComitente = pobjOrden.IDComitente
                objNewOrdenOYD.NombreCliente = pobjOrden.NombreCliente
                objNewOrdenOYD.NroDocumento = pobjOrden.NroDocumento
                objNewOrdenOYD.CategoriaCliente = pobjOrden.CategoriaCliente
                objNewOrdenOYD.IDOrdenante = pobjOrden.IDOrdenante
                objNewOrdenOYD.NombreOrdenante = pobjOrden.NombreOrdenante
                objNewOrdenOYD.UBICACIONTITULO = pobjOrden.UBICACIONTITULO
                objNewOrdenOYD.CuentaDeposito = pobjOrden.CuentaDeposito
                objNewOrdenOYD.DescripcionCta = pobjOrden.DescripcionCta
                objNewOrdenOYD.UsuarioOperador = pobjOrden.UsuarioOperador
                objNewOrdenOYD.CanalRecepcion = pobjOrden.CanalRecepcion
                objNewOrdenOYD.MedioVerificable = pobjOrden.MedioVerificable
                objNewOrdenOYD.FechaRecepcion = pobjOrden.FechaRecepcion
                objNewOrdenOYD.NroExtensionToma = pobjOrden.NroExtensionToma
                objNewOrdenOYD.Especie = pobjOrden.Especie
                objNewOrdenOYD.ISIN = pobjOrden.ISIN
                objNewOrdenOYD.FechaEmision = pobjOrden.FechaEmision
                objNewOrdenOYD.FechaVencimiento = pobjOrden.FechaVencimiento
                objNewOrdenOYD.Estandarizada = pobjOrden.Estandarizada
                objNewOrdenOYD.FechaCumplimiento = pobjOrden.FechaCumplimiento
                objNewOrdenOYD.TasaFacial = pobjOrden.TasaFacial
                objNewOrdenOYD.Modalidad = pobjOrden.Modalidad
                objNewOrdenOYD.NombreModalidad = pobjOrden.NombreModalidad
                objNewOrdenOYD.Indicador = pobjOrden.Indicador
                objNewOrdenOYD.PuntosIndicador = pobjOrden.PuntosIndicador
                objNewOrdenOYD.EnPesos = pobjOrden.EnPesos
                objNewOrdenOYD.Cantidad = pobjOrden.Cantidad
                objNewOrdenOYD.Precio = pobjOrden.Precio
                objNewOrdenOYD.PrecioMaximoMinimo = pobjOrden.PrecioMaximoMinimo
                objNewOrdenOYD.ValorCaptacionGiro = pobjOrden.ValorCaptacionGiro
                objNewOrdenOYD.ValorFuturoRepo = pobjOrden.ValorFuturoRepo
                objNewOrdenOYD.TasaRegistro = pobjOrden.TasaRegistro
                objNewOrdenOYD.TasaCliente = pobjOrden.TasaCliente
                objNewOrdenOYD.TasaNominal = pobjOrden.TasaNominal
                objNewOrdenOYD.Castigo = pobjOrden.Castigo
                objNewOrdenOYD.ValorAccion = pobjOrden.ValorAccion
                objNewOrdenOYD.Comision = pobjOrden.Comision
                objNewOrdenOYD.ValorComision = pobjOrden.ValorComision
                objNewOrdenOYD.ValorOrden = pobjOrden.ValorOrden
                objNewOrdenOYD.DiasRepo = pobjOrden.DiasRepo
                objNewOrdenOYD.ProductoValores = pobjOrden.ProductoValores
                objNewOrdenOYD.CostosAdicionales = pobjOrden.CostosAdicionales
                objNewOrdenOYD.Instrucciones = pobjOrden.Instrucciones
                objNewOrdenOYD.Notas = pobjOrden.Notas
                objNewOrdenOYD.Custodia = pobjOrden.Custodia
                objNewOrdenOYD.CustodiaSecuencia = pobjOrden.CustodiaSecuencia
                objNewOrdenOYD.DiasCumplimiento = pobjOrden.DiasCumplimiento
                objNewOrdenOYD.RuedaNegocio = pobjOrden.RuedaNegocio
                objNewOrdenOYD.PrecioLimpio = pobjOrden.PrecioLimpio
                objNewOrdenOYD.EstadoTitulo = pobjOrden.EstadoTitulo
                objNewOrdenOYD.ReferenciaBolsa = pobjOrden.ReferenciaBolsa
                objNewOrdenOYD.FechaReferenciaBolsa = pobjOrden.FechaReferenciaBolsa
                objNewOrdenOYD.ComisionesOrdenesXML = pobjOrden.ComisionesOrdenesXML
                objNewOrdenOYD.InstruccionesOrdenesXML = pobjOrden.InstruccionesOrdenesXML
                objNewOrdenOYD.LiqAsociadasXML = pobjOrden.LiqAsociadasXML
                objNewOrdenOYD.PagosOrdenesXML = pobjOrden.PagosOrdenesXML
                objNewOrdenOYD.ReceptoresCruzadasXML = pobjOrden.ReceptoresCruzadasXML
                objNewOrdenOYD.ReceptoresXML = pobjOrden.ReceptoresXML
                objNewOrdenOYD.OrdenCruzada = pobjOrden.OrdenCruzada
                objNewOrdenOYD.OrdenCruzadaCliente = pobjOrden.OrdenCruzadaCliente
                objNewOrdenOYD.OrdenCruzadaReceptor = pobjOrden.OrdenCruzadaReceptor
                objNewOrdenOYD.IDOrdenOriginal = pobjOrden.IDOrdenOriginal
                objNewOrdenOYD.IDNroOrdenOriginal = pobjOrden.IDNroOrdenOriginal
                objNewOrdenOYD.NombreReceptorParaCruzada = pobjOrden.NombreReceptorParaCruzada
                objNewOrdenOYD.Usuario = pobjOrden.Usuario
                objNewOrdenOYD.Modificable = pobjOrden.Modificable
                objNewOrdenOYD.FechaActualizacion = pobjOrden.FechaActualizacion
                objNewOrdenOYD.ValorFuturoCliente = pobjOrden.ValorFuturoCliente
                objNewOrdenOYD.IvaComision = pobjOrden.IvaComision
                objNewOrdenOYD.BrokenTrader = pobjOrden.BrokenTrader
                objNewOrdenOYD.Entidad = pobjOrden.Entidad
                objNewOrdenOYD.Estrategia = pobjOrden.Estrategia

                pobjOrdenSalvarDatos = objNewOrdenOYD
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", Me.ToString(), "ObtenerValoresOrdenAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Desarrollado por Juan David Correa.
    ''' Llevar los datos de la orden que se estaba editando a la orden de la lista.
    ''' Esto soluciona el error cuando se edita una orden y luego se da cancelar, como los combos se vuelven a cargar 
    ''' </summary>
    ''' <param name="pobjOrden"></param>
    ''' <remarks></remarks>
    Public Sub ObtenerValoresOrdenEnLista(ByVal pobjOrden As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            If Not IsNothing(pobjOrden) Then
                If ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).Count > 0 Then
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.Receptor = pobjOrden.Receptor
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.TipoOrden = pobjOrden.TipoOrden
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.TipoNegocio = pobjOrden.TipoNegocio
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.TipoProducto = pobjOrden.TipoProducto
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.TipoOperacion = pobjOrden.TipoOperacion
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.Clasificacion = pobjOrden.Clasificacion
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.TipoLimite = pobjOrden.TipoLimite
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.Duracion = pobjOrden.Duracion
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.CondicionesNegociacion = pobjOrden.CondicionesNegociacion
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.FormaPago = pobjOrden.FormaPago
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.TipoInversion = pobjOrden.TipoInversion
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.Ejecucion = pobjOrden.Ejecucion
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.Mercado = pobjOrden.Mercado
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.UsuarioOperador = pobjOrden.UsuarioOperador
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.CanalRecepcion = pobjOrden.CanalRecepcion
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.MedioVerificable = pobjOrden.MedioVerificable
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.Modalidad = pobjOrden.Modalidad
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.Indicador = pobjOrden.Indicador
                    ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjOrden.IDNroOrden).FirstOrDefault.ProductoValores = pobjOrden.ProductoValores
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", Me.ToString(), "ObtenerValoresOrdenAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ReemplazarValorEnLista()

    End Sub

    ''' <summary>
    ''' Metodo para obtener la información de la orden de SAE.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 20 de septiembre del 2012
    ''' </summary>
    Public Sub ObtenerInformacionOrdenSAEAcciones(ByVal objOrdenSAE As OYDPLUSUtilidades.tblOrdenesSAEAcciones, ByVal objOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, ByVal logBorrarEspecie As Boolean)
        Try
            If Not IsNothing(objOrdenSAE) Then
                If objOrdenSAE.Seleccionada Then

                    logCambiarConsultaSAE = False

                    If logBorrarEspecie Then
                        objOrdenSelected.Especie = objOrdenSAE.Especie
                        BorrarEspecie = False
                    End If

                    objOrdenSelected.Cantidad = objOrdenSAE.Cantidad
                    objOrdenSelected.Precio = objOrdenSAE.Precio
                    objOrdenSelected.ReferenciaBolsa = objOrdenSAE.NroLiquidacion
                    objOrdenSelected.FechaReferenciaBolsa = objOrdenSAE.FechaReferencia
                    objOrdenSelected.Ejecucion = objOrdenSAE.Ejecucion
                    objOrdenSelected.Duracion = objOrdenSAE.Duracion

                    logCambiarConsultaSAE = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información de la orden de SAE.", Me.ToString(), "ObtenerInformacionOrdenSAEAcciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ObtenerInformacionOrdenSAERentaFija(ByVal objOrdenSAE As OYDPLUSUtilidades.tblOrdenesSAERentaFija, ByVal objOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, ByVal logBorrarEspecie As Boolean)
        Try
            If Not IsNothing(objOrdenSAE) Then
                If objOrdenSAE.Seleccionada Then

                    logCambiarConsultaSAE = False

                    If logBorrarEspecie Then
                        objOrdenSelected.Especie = objOrdenSAE.Especie
                        objOrdenSelected.FechaEmision = objOrdenSAE.FechaEmision
                        objOrdenSelected.FechaVencimiento = objOrdenSAE.FechaVencimiento
                        objOrdenSelected.Indicador = objOrdenSAE.Indicador
                        objOrdenSelected.PuntosIndicador = objOrdenSAE.PuntosIndicador
                        objOrdenSelected.ISIN = objOrdenSAE.ISIN
                        BorrarEspecie = False
                    End If

                    objOrdenSelected.Precio = objOrdenSAE.Precio
                    objOrdenSelected.Cantidad = objOrdenSAE.Cantidad
                    objOrdenSelected.ReferenciaBolsa = objOrdenSAE.NroLiquidacion
                    objOrdenSelected.FechaReferenciaBolsa = objOrdenSAE.FechaReferencia
                    objOrdenSelected.Clasificacion = objOrdenSAE.Clasificacion
                    objOrdenSelected.CondicionesNegociacion = objOrdenSAE.CondicionesNegociacion
                    objOrdenSelected.DiasCumplimiento = objOrdenSAE.DiasCumplimiento
                    objOrdenSelected.FechaCumplimiento = objOrdenSAE.FechaCumplimiento

                    objOrdenSelected.Mercado = objOrdenSAE.Mercado
                    objOrdenSelected.EstadoTitulo = objOrdenSAE.EstadoTitulo
                    objOrdenSelected.PrecioLimpio = objOrdenSAE.PrecioLimpio
                    objOrdenSelected.RuedaNegocio = objOrdenSAE.RuedaNegocio

                    logCambiarConsultaSAE = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información de la orden de SAE.", Me.ToString(), "ObtenerInformacionOrdenSAERentaFija", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para obtener la información del portafolio del cliente.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 20 de septiembre del 2012
    ''' </summary>
    Public Sub ObtenerInformacionPortafolioCliente(ByVal objPortafolioCliente As OYDPLUSUtilidades.tblPortafolioCliente, ByVal objOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, ByVal logBorarEspecie As Boolean)
        Try
            If Not IsNothing(objPortafolioCliente) Then
                If objPortafolioCliente.Seleccionada Then

                    logCambiarConsultaPortafolio = False

                    If logBorarEspecie Then
                        If Not IsNothing(objPortafolioCliente.Especie) Then
                            EspecieBuscar = String.Empty
                            EspecieBuscar = objPortafolioCliente.Especie
                            BorrarEspecie = False
                        End If

                        If Not IsNothing(objPortafolioCliente.Especie) Then
                            objOrdenSelected.Especie = objPortafolioCliente.Especie
                            EspecieControles = objPortafolioCliente.Especie

                            If Not IsNothing(objPortafolioCliente.FechaEmision) Then
                                objOrdenSelected.FechaEmision = objPortafolioCliente.FechaEmision
                                objOrdenSelected.FechaVencimiento = objPortafolioCliente.FechaVencimiento
                                objOrdenSelected.Modalidad = objPortafolioCliente.Modalidad
                                objOrdenSelected.Indicador = objPortafolioCliente.Indicador
                                objOrdenSelected.PuntosIndicador = objPortafolioCliente.PuntosIndicador
                            End If
                        End If
                    End If

                    objOrdenSelected.Cantidad = objPortafolioCliente.Cantidad
                    'objOrdenSelected.Precio = objPortafolioCliente.PrecioCompra
                    objOrdenSelected.Custodia = objPortafolioCliente.Custodia
                    objOrdenSelected.CustodiaSecuencia = objPortafolioCliente.CustodiaSecuencia

                    If Not String.IsNullOrEmpty(objPortafolioCliente.CentralDeposito) And Not String.IsNullOrEmpty(objPortafolioCliente.CuentaDeposito) Then
                        If Not IsNothing(_ListaCuentasDepositoOYDPLUS) Then
                            For Each li In _ListaCuentasDepositoOYDPLUS
                                If li.Deposito = objPortafolioCliente.CentralDeposito.ToString And li.NroCuentaDeposito = objPortafolioCliente.CuentaDeposito Then
                                    CtaDepositoSeleccionadaOYDPLUS = li
                                End If
                            Next
                        End If
                    End If

                    logCambiarConsultaPortafolio = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información del portafolio del cliente.", Me.ToString(), "ObtenerInformacionPortafolioCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para obtener la información de las operaciones x cumplir.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 20 de septiembre del 2012
    ''' </summary>
    Public Sub ObtenerInformacionOperacionesXCumplir(ByVal objOperacionXCumplir As OYDPLUSUtilidades.tblOperacionesCumplir, ByVal objOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, ByVal logBorrarEspecie As Boolean)
        Try
            If Not IsNothing(objOperacionXCumplir) Then
                If objOperacionXCumplir.Seleccionada Then

                    logCambiarConsultaOperaciones = False

                    If logBorrarEspecie Then
                        If Not IsNothing(objOperacionXCumplir.Especie) Then
                            EspecieBuscar = String.Empty
                            EspecieBuscar = objOperacionXCumplir.Especie
                            BorrarEspecie = False
                        End If

                        If Not IsNothing(objOperacionXCumplir.Especie) Then
                            objOrdenSelected.Especie = objOperacionXCumplir.Especie
                            If Not IsNothing(objOperacionXCumplir.FechaEmision) Then
                                objOrdenSelected.FechaEmision = objOperacionXCumplir.FechaEmision
                                objOrdenSelected.FechaVencimiento = objOperacionXCumplir.FechaVencimiento
                                objOrdenSelected.Modalidad = objOperacionXCumplir.Modalidad
                                objOrdenSelected.Indicador = objOperacionXCumplir.Indicador
                                objOrdenSelected.PuntosIndicador = objOperacionXCumplir.PuntosIndicador
                            End If
                        End If
                    End If

                    objOrdenSelected.Cantidad = objOperacionXCumplir.Cantidad
                    objOrdenSelected.Precio = objOperacionXCumplir.PrecioLiquidacion

                    logCambiarConsultaOperaciones = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información de las operaciones x cumplir.", Me.ToString(), "ObtenerInformacionOperacionesXCumplir", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar los beneficiarios de la cuenta depósito asignada a la orden.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 21 de septiembre del 2012.
    ''' </summary>
    Private Sub consultarBeneficiariosOrden()
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not IsNothing(dcProxy.BeneficiariosOrdens) Then
                    dcProxy.BeneficiariosOrdens.Clear()
                End If

                dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenes_OrdenQuery(_OrdenOYDPLUSSelected.Clase, _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.NroOrden, _OrdenOYDPLUSSelected.Version, Program.Usuario, _OrdenOYDPLUSSelected.TipoNegocio, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "")
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los beneficiarios de la orden.", Me.ToString(), "consultarBeneficiariosOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' Consultar las liquidaciones asociadas a la orden
    ''' Desarrollado por: Juan David Correa.
    ''' Fecha 21 de septiembre del 2012
    ''' </summary>
    Private Sub consultarLiquidacionesOrden()
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not IsNothing(dcProxy.LiquidacionesOrdens) Then
                    dcProxy.LiquidacionesOrdens.Clear()
                End If
                dcProxy.Load(dcProxy.ConsultarLiquidacionesAsociadasOrdenQuery(_OrdenOYDPLUSSelected.Clase, _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.NroOrden, Program.Usuario, _OrdenOYDPLUSSelected.TipoNegocio, Program.HashConexion), AddressOf TerminoConsultarLiquidacionesOrden, "consultarLiquidaciones")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener las liquidaciones de la orden.", Me.ToString(), "consultarLiquidacionesOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub LimpiarDatosTipoNegocio()
        Try
            ComitenteSeleccionadoOYDPLUS = Nothing
            If Not IsNothing(ListaOrdenantesOYDPLUS) Then
                ListaOrdenantesOYDPLUS.Clear()
            End If

            NemotecnicoSeleccionadoOYDPLUS = Nothing


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los datos del tipo de negocio.", Me.ToString(), "LimpiarDatosTipoNegocio", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Actualizar elemento buscado con los datos recibidos como parámetro
    ''' </summary>
    ''' <param name="pstrTipoItem">Tipo de objeto que se recibe</param>
    ''' <param name="pobjItem">Item enviado como parámetro</param>
    Public Sub actualizarItemOrden(ByVal pstrTipoItem As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrTipoItem.ToLower
                    Case "idreceptor"
                        Me.ReceptoresOrdenSelected.IDReceptor = pobjItem.IdItem
                        Me.ReceptoresOrdenSelected.Nombre = pobjItem.Nombre
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el item de la orden.", Me.ToString(), "actualizarItemOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ConsultarUltimoPrecioEspecie(ByVal pstrEspecie As String, ByVal pstrTipoOperacion As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not String.IsNullOrEmpty(pstrEspecie) And Not String.IsNullOrEmpty(pstrTipoOperacion) Then
                If pstrEspecie <> "(No Seleccionado)" Then
                    If Not IsNothing(dcProxy1.MejorPrecioEspecieOrdens) Then
                        dcProxy1.MejorPrecioEspecieOrdens.Clear()
                    End If

                    dcProxy1.Load(dcProxy1.OYDPLUS_ConsultarMejorPrecioEspecie_OrdenQuery(pstrTipoOperacion, pstrEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarMejorPrecioEspecie, pstrUserState)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el ultimo precio de la especie.", Me.ToString(), "ConsultarUltimoPrecioEspecie", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub RecargarPantallaOrdenes()
        Try
            If logEditarRegistro = False And logNuevoRegistro = False Then
                If Not IsNothing(ListaOrdenOYDPLUS) Then
                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                        If ListaOrdenOYDPLUS.Count > 0 Then
                            If ListaOrdenOYDPLUS.First.IDNroOrden = _OrdenOYDPLUSSelected.IDNroOrden Then
                                Dim vista As String = VistaSeleccionada
                                VistaSeleccionada = String.Empty
                                VistaSeleccionada = vista
                            Else
                                intIDOrdenTimer = _OrdenOYDPLUSSelected.IDNroOrden

                                If VistaSeleccionada = VISTA_PENDIENTESAPROBAR Then
                                    FiltrarRegistrosOYDPLUS("D", String.Empty, "TIMERSELECTED", "TIMERSELECTED")
                                Else
                                    FiltrarRegistrosOYDPLUS("P", String.Empty, "TIMERSELECTED", "TIMERSELECTED")
                                End If
                            End If
                        Else
                            Dim vista As String = VistaSeleccionada
                            VistaSeleccionada = String.Empty
                            VistaSeleccionada = vista
                        End If
                    Else
                        Dim vista As String = VistaSeleccionada
                        VistaSeleccionada = String.Empty
                        VistaSeleccionada = vista
                    End If
                Else
                    Dim vista As String = VistaSeleccionada
                    VistaSeleccionada = String.Empty
                    VistaSeleccionada = vista
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar la pantalla de Ordenes.", Me.ToString(), "RecargarPantallaOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub CalcularDiasPlazo(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF)
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                If Not IsNothing(pobjOrdenSelected.FechaCumplimiento) Then
                    If Not IsNothing(pobjOrdenSelected.FechaOrden) Then
                        pobjOrdenSelected.DiasRepo = DateDiff(DateInterval.Day, pobjOrdenSelected.FechaOrden.Value.Date, pobjOrdenSelected.FechaCumplimiento.Value.Date)
                        pobjOrdenSelected.DiasCumplimiento = DateDiff(DateInterval.Day, pobjOrdenSelected.FechaOrden.Value.Date, pobjOrdenSelected.FechaCumplimiento.Value.Date)
                        CalcularDiasOrdenOYDPLUS(MSTR_CALCULAR_DIAS_PLAZO, pobjOrdenSelected, -1)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los días del plazo.", Me.ToString(), "CalcularDiasPlazo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Function ValidarFechaCierreSistema(ByVal pobjOrdenOYDPLUS As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, ByVal pstrAccion As String)
        Dim logRetorno As Boolean = True
        Try
            If Not IsNothing(pobjOrdenOYDPLUS) Then
                If Not IsNothing(pobjOrdenOYDPLUS.FechaVigencia) Then
                    If pobjOrdenOYDPLUS.FechaVigencia.Value.Date <= mdtmFechaCierreSistema Then
                        mostrarMensaje("La fecha de vigencia no puede ser menor o igual a la fecha de cierre del sistema (" & mdtmFechaCierreSistema.ToLongDateString() & ")." & vbNewLine & "No se puede " & pstrAccion & ".", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        logRetorno = False
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario,
                                                         "Se presentó un problema al validar la fecha de cierre.",
                                                         Me.ToString(), "ValidarFechaCierreSistema", Program.TituloSistema,
                                                         Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
            logRetorno = False
        End Try
        Return logRetorno
    End Function

    Private Function ValidarTipoProductoPosicionPropia(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF) As Boolean
        Dim logRetorno As Boolean = False

        Try
            If logHabilitarCondicionesTipoProductoCuentaPropia Then
                If Not IsNothing(pobjOrdenSelected) Then
                    If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoProducto) Then
                        If Not IsNothing(strTiposProductoPosicionPropia) Then
                            Dim objRegistros = strTiposProductoPosicionPropia.Split(",")
                            For Each objRegistro In objRegistros
                                If objRegistro = pobjOrdenSelected.TipoProducto Then
                                    logRetorno = True
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
        Return logRetorno
    End Function

    Private Function LlevarRecetorOrdenDistribucionComision(ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, ByVal plogOrdenCruzada As Boolean) As Boolean
        Dim logRetorno As Boolean = False

        Try
            Dim objListaDistribucionComision As New List(Of OyDPLUSOrdenesOF.ReceptoresOrden)
            Dim NewReceptoresOrden As New OyDPLUSOrdenesOF.ReceptoresOrden

            NewReceptoresOrden.ClaseOrden = pobjOrdenSelected.Clase
            NewReceptoresOrden.TipoOrden = pobjOrdenSelected.TipoOperacion
            NewReceptoresOrden.NroOrden = pobjOrdenSelected.NroOrden
            NewReceptoresOrden.Version = pobjOrdenSelected.Version
            NewReceptoresOrden.IDComisionista = 0
            NewReceptoresOrden.IDSucComisionista = 0
            NewReceptoresOrden.FechaActualizacion = dtmFechaServidor
            NewReceptoresOrden.Usuario = Program.Usuario
            NewReceptoresOrden.Lider = True
            NewReceptoresOrden.Porcentaje = 100
            NewReceptoresOrden.Nombre = pobjOrdenSelected.NombreReceptor
            NewReceptoresOrden.IDReceptor = pobjOrdenSelected.Receptor

            objListaDistribucionComision.Add(NewReceptoresOrden)

            ReceptoresOrdenSelected = Nothing
            ListaReceptoresOrdenes = objListaDistribucionComision
            If ListaReceptoresOrdenes.Count > 0 Then
                ReceptoresOrdenSelected = ListaReceptoresOrdenes.First
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
        Return logRetorno
    End Function

#End Region

#Region "Resultados Asincrónicos OYDPlus"

    Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.OrdenOYDPLUSOF))
        Try
            If Not lo.HasError Then

                If lo.UserState = "TERMINOGUARDAREDICION" Or lo.UserState = "TIMERSELECTED" Or lo.UserState = "TERMINOGUARDARPENDIENTES" Then
                    logCambiarSelected = False
                Else
                    logCambiarSelected = True
                End If

                ListaOrdenOYDPLUS = dcProxyConsulta.OrdenOYDPLUSOFs.ToList

                If lo.UserState = "BUSQUEDA" Then
                    If _ListaOrdenOYDPLUS.Count = 0 Then
                        mostrarMensaje("No se encontraron registros que cumplan con las condiciones de busqueda.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                    IsBusy = False
                    'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                    logRefrescarconsultaCambioTab = True
                ElseIf lo.UserState = "FILTRAR" Then
                    If _ListaOrdenOYDPLUS.Count = 0 Then
                        mostrarMensaje("No se encontraron registros que cumplan con las condiciones de indicadas.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                    IsBusy = False
                    'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                    logRefrescarconsultaCambioTab = True
                ElseIf lo.UserState = "TERMINOGUARDAREDICION" Then
                    If _ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = intIDOrdenTimer).Count > 0 Then
                        OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = intIDOrdenTimer).First
                    Else
                        OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.FirstOrDefault
                    End If

                    logCambiarSelected = True
                    IsBusy = False
                    'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                    logRefrescarconsultaCambioTab = True

                ElseIf lo.UserState = "TERMINOGUARDARNUEVO" Then
                    IsBusy = False
                    'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                    logRefrescarconsultaCambioTab = True

                ElseIf lo.UserState = "TERMINOGUARDARPENDIENTES" Then
                    If _ListaOrdenOYDPLUS.Count = 0 Then
                        'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                        logRefrescarconsultaCambioTab = False
                        VistaSeleccionada = VISTA_PENDIENTESAPROBAR
                        FiltrarRegistrosOYDPLUS("P", "GUARDADO ORDEN", "TERMINOGUARDAR", "TERMINOGUARDAR")
                    Else
                        If intIDOrdenTimer <> 0 Then
                            If _ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = intIDOrdenTimer).Count > 0 Then
                                OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = intIDOrdenTimer).First
                            Else
                                OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.FirstOrDefault
                            End If
                        Else
                            OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.FirstOrDefault
                        End If
                        IsBusy = False
                        'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                        logRefrescarconsultaCambioTab = True
                        logCambiarSelected = True
                    End If

                ElseIf lo.UserState = "TIMERSELECTED" Then
                    If _ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = intIDOrdenTimer).Count > 0 Then
                        OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = intIDOrdenTimer).First
                    End If
                    IsBusy = False
                    logCambiarSelected = True
                    'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                    logRefrescarconsultaCambioTab = True
                Else
                    IsBusy = False
                    'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                    logRefrescarconsultaCambioTab = True
                End If

                'Reinicia el TIMER para no realizar llamados a la base de datos.
                ReiniciaTimer()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la lista de Ordenes", Me.ToString(), "TerminoTraerOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
                ''lo.MarkErrorAsHandled()   '????
                IsBusy = False
                'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                logRefrescarconsultaCambioTab = True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes", Me.ToString(), "TerminoTraerOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
            'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
            logRefrescarconsultaCambioTab = True
        End Try

    End Sub

    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.HasError = False Then
                If lo.Entities.ToList.Count > 0 Then
                    Me.ComitenteSeleccionadoOYDPLUS = lo.Entities.ToList.FirstOrDefault
                Else
                    Me.ComitenteSeleccionadoOYDPLUS = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If lo.HasError = False Then
                If lo.Entities.ToList.Count > 0 Then
                    Me.NemotecnicoSeleccionadoOYDPLUS = lo.Entities.ToList.FirstOrDefault
                Else
                    Me.NemotecnicoSeleccionadoOYDPLUS = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie de la orden", Me.ToString(), "buscarEspecieCompleted", Program.TituloSistema, Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie de la orden", Me.ToString(), "buscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerOrdenantes(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        Try
            If Not lo.HasError Then
                Dim objOrdenante As String = String.Empty
                If lo.UserState = OPCION_EDITAR Then
                    objOrdenante = OrdenOYDPLUSSelected.IDOrdenante
                End If

                ListaOrdenantesOYDPLUS = lo.Entities.ToList

                If lo.UserState = OPCION_EDITAR Then
                    If _ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = objOrdenante).Count > 0 Then
                        OrdenanteSeleccionadoOYDPLUS = _ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = objOrdenante).FirstOrDefault
                    End If
                ElseIf logNuevoRegistro Or logEditarRegistro Then
                    If Not IsNothing(DiccionarioCombosOYDPlus) Then
                        If DiccionarioCombosOYDPlus.ContainsKey("COLOCARORDENANTELIDER") Then
                            If DiccionarioCombosOYDPlus("COLOCARORDENANTELIDER").Where(Function(i) i.Retorno.ToUpper = "SI").Count > 0 Then
                                If _ListaOrdenantesOYDPLUS.Where(Function(i) i.Lider).Count > 0 Then
                                    OrdenanteSeleccionadoOYDPLUS = _ListaOrdenantesOYDPLUS.Where(Function(i) i.Lider).First()
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes",
                                                 Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes",
                                                 Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, ex)
        End Try


    End Sub

    Private Sub TerminoTraerCuentasDeposito(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        Try
            If Not lo.HasError Then
                Dim objNroCuenta As Integer = 0
                Dim strDeposito As String = String.Empty

                If lo.UserState = OPCION_EDITAR And Not IsNothing(_OrdenAnteriorOYDPLUS) Then
                    strDeposito = _OrdenAnteriorOYDPLUS.UBICACIONTITULO
                    objNroCuenta = _OrdenAnteriorOYDPLUS.CuentaDeposito
                End If

                If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_ACCIONESOF Then
                    If lo.Entities.ToList.Where(Function(I) I.Deposito <> "D").Count > 0 Then
                        ListaCuentasDepositoOYDPLUS = lo.Entities.ToList.Where(Function(I) I.Deposito <> "D").ToList
                        If ListaCuentasDepositoOYDPLUS.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = objNroCuenta).Count > 0 Then
                            CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = objNroCuenta).FirstOrDefault
                        Else
                            If ListaCuentasDepositoOYDPLUS.Where(Function(i) i.Deposito = "D").Count > 0 Then
                                CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.Where(Function(i) i.Deposito = "D").First
                            End If
                        End If
                    Else
                        ListaCuentasDepositoOYDPLUS = Nothing
                    End If
                Else
                    ListaCuentasDepositoOYDPLUS = lo.Entities.ToList
                    If ListaCuentasDepositoOYDPLUS.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = objNroCuenta).Count > 0 Then
                        CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = objNroCuenta).FirstOrDefault
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito",
                                                 Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito",
                                                 Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerReceptoresOrdenes(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.ReceptoresOrden))
        Try
            If Not lo.HasError Then
                ListaReceptoresOrdenes = lo.Entities.ToList()

                ObtenerReceptorLiderOrdenOYDPLUS()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenes",
                                                 Me.ToString(), "TerminoTraerReceptoresOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenes",
                                                Me.ToString(), "TerminoTraerReceptoresOrdenes", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoConsultarCombosOYDCOMPLETOS(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCombosOYDPlusCompleta = Nothing

                    DiccionarioCombosOYDPlusCompleta = objDiccionarioCompleto

                    ObtenerInformacionCombosCompletos()

                    VistaSeleccionada = VISTA_APROBADAS
                Else
                    mostrarMensaje("Señor Usuario, usted no tiene combos configurados.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYDCOMPLETOS", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYDCOMPLETOS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarCombosOYD(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
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

                    DiccionarioCombosOYDPlus = Nothing
                    DiccionarioCombosOYDPlus = objDiccionario

                    If logNuevoRegistro = False And logEditarRegistro = False Then
                        ObtenerValoresCombos(True, _OrdenOYDPLUSSelected)
                    Else
                        If Not DiccionarioCombosOYDPlus.ContainsKey("TIPOPRODUCTO") Then
                            mostrarMensaje(String.Format("Señor Usuario, el receptor no tiene configurado tipos de producto.", vbCrLf), "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            HabilitarNegocio = False
                            HabilitarDuracion = False
                            MostrarNegocio = Visibility.Collapsed
                            MostrarCamposAcciones = Visibility.Collapsed
                            HabilitarEjecucion = False
                            MostrarCamposRentaFija = Visibility.Collapsed
                            MostrarAdicionalesEspecie = Visibility.Collapsed
                            MostrarCamposCompra = Visibility.Collapsed
                            MostrarCamposCuentaPropia = Visibility.Collapsed
                            MostrarCamposFaciales = Visibility.Collapsed
                            MostrarCamposVenta = Visibility.Collapsed
                            MostrarControles = Visibility.Collapsed
                            IsBusy = False
                        Else
                            If Not IsNothing(lo.UserState) Then
                                If lo.UserState.ToString = OPCION_DUPLICAR Then
                                    ObtenerValoresCombos(False, _OrdenDuplicarOYDPLUS, lo.UserState.ToString.ToUpper)
                                ElseIf lo.UserState.ToString = OPCION_PLANTILLA Or lo.UserState.ToString = OPCION_CREARORDENPLANTILLA Then
                                    ObtenerValoresCombos(False, _OrdenPlantillaOYDPLUS, lo.UserState.ToString.ToUpper)
                                Else
                                    ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, lo.UserState.ToString.ToUpper)
                                End If
                            Else
                                ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_RECEPTOR)
                            End If
                        End If
                    End If

                Else
                    mostrarMensaje("Señor Usuario, usted no tiene receptores asociados.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
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

    Private Sub TerminoConsultarTipoNegocioOYDCOMPLETOS(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblTipoNegocioReceptor))
        Try
            If lo.HasError = False Then
                ListaTipoNegocioCOMPLETOS = lo.Entities.ToList

                'Consultar Receptores Usuario
                CargarReceptoresUsuarioOYDPLUS("INICIO")
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los tipos de negocio del receptor.", Me.ToString(), "TerminoConsultarTipoNegocioOYDCOMPLETOS", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los tipos de negocio del receptor.", Me.ToString(), "TerminoConsultarTipoNegocioOYDCOMPLETOS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarTipoNegocioOYD(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblTipoNegocioReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaTipoNegocio = lo.Entities.ToList

                    If lo.UserState.ToString = OPCION_DUPLICAR Then
                        CargarConfiguracionReceptorOYDPLUS(_OrdenDuplicarOYDPLUS.Receptor, lo.UserState)
                    ElseIf lo.UserState.ToString = OPCION_CREARORDENPLANTILLA Then
                        CargarConfiguracionReceptorOYDPLUS(_OrdenPlantillaOYDPLUS.Receptor, lo.UserState)
                    Else
                        CargarConfiguracionReceptorOYDPLUS(_OrdenOYDPLUSSelected.Receptor, lo.UserState)
                    End If
                Else
                    mostrarMensaje("Señor Usuario, el receptor seleccionado no tiene tipos de negocio configurados.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    HabilitarNegocio = False
                    HabilitarDuracion = False
                    MostrarNegocio = Visibility.Collapsed
                    MostrarCamposAcciones = Visibility.Collapsed
                    HabilitarEjecucion = False
                    MostrarCamposRentaFija = Visibility.Collapsed
                    MostrarAdicionalesEspecie = Visibility.Collapsed
                    MostrarCamposCompra = Visibility.Collapsed
                    MostrarCamposCuentaPropia = Visibility.Collapsed
                    MostrarCamposFaciales = Visibility.Collapsed
                    MostrarCamposVenta = Visibility.Collapsed
                    MostrarControles = Visibility.Collapsed
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los tipos de negocio del receptor.", Me.ToString(), "TerminoConsultarTipoNegocioOYD", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los tipos de negocio del receptor.", Me.ToString(), "TerminoConsultarTipoNegocioOYD", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarConfiguracionReceptor(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblConfiguracionesAdicionalesReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ConfiguracionReceptor = lo.Entities.FirstOrDefault

                    'Consulta los parametros del receptor.
                    If lo.UserState.ToString = OPCION_DUPLICAR Then
                        CargarParametrosReceptorOYDPLUS(_OrdenDuplicarOYDPLUS.Receptor, lo.UserState)
                    ElseIf lo.UserState.ToString = OPCION_CREARORDENPLANTILLA Then
                        CargarParametrosReceptorOYDPLUS(_OrdenPlantillaOYDPLUS.Receptor, lo.UserState)
                    Else
                        CargarParametrosReceptorOYDPLUS(_OrdenOYDPLUSSelected.Receptor, lo.UserState)
                    End If
                Else
                    ConfiguracionReceptor = Nothing
                    mostrarMensaje(String.Format("Señor Usuario, el receptor no tiene configurado datos en el maestro de configuraciones adicionales receptor.", vbCrLf), "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    HabilitarNegocio = False
                    HabilitarDuracion = False
                    MostrarNegocio = Visibility.Collapsed
                    MostrarCamposAcciones = Visibility.Collapsed
                    HabilitarEjecucion = False
                    MostrarCamposRentaFija = Visibility.Collapsed
                    MostrarAdicionalesEspecie = Visibility.Collapsed
                    MostrarCamposCompra = Visibility.Collapsed
                    MostrarCamposCuentaPropia = Visibility.Collapsed
                    MostrarCamposFaciales = Visibility.Collapsed
                    MostrarCamposVenta = Visibility.Collapsed
                    MostrarControles = Visibility.Collapsed
                    IsBusy = False
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la configuración del receptor.", Me.ToString(), "TerminoConsultarConfiguracionReceptor", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los parametros del receptor.", Me.ToString(), "TerminoConsultarConfiguracionReceptor", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarParametrosReceptor(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblParametrosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaParametrosReceptor = lo.Entities.ToList
                Else
                    ListaParametrosReceptor = Nothing
                End If

                If lo.UserState.ToString = OPCION_DUPLICAR Then
                    CargarCombosOYDPLUS(lo.UserState, _OrdenDuplicarOYDPLUS.Receptor, lo.UserState)
                ElseIf lo.UserState.ToString = OPCION_CREARORDENPLANTILLA Then
                    CargarCombosOYDPLUS(lo.UserState, _OrdenPlantillaOYDPLUS.Receptor, lo.UserState)
                Else
                    CargarCombosOYDPLUS(lo.UserState, _OrdenOYDPLUSSelected.Receptor, lo.UserState)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los parametros del receptor.", Me.ToString(), "TerminoConsultarParametrosReceptor", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los parametros del receptor.", Me.ToString(), "TerminoConsultarParametrosReceptor", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCargarMensaje(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblMensajes))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    TituloMensaje = lo.Entities.FirstOrDefault.Titulo
                    ListaMensajes = lo.Entities.ToList
                Else
                    TituloMensaje = String.Empty
                    ListaMensajes = lo.Entities.ToList
                End If
                VelocidadMensaje = VELOCIDAD_TEXTO
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el mensaje dinamico.", Me.ToString(), "TerminoCargarMensaje", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                TituloMensaje = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el mensaje dinamico.", Me.ToString(), "TerminoCargarMensaje", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoConsultarReceptoresUsuarioCOMPLETOS(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
        Try
            If lo.HasError = False Then
                ListaReceptoresCompleta = lo.Entities.ToList

                'Se adiciona la condición cuando para que no inicie las variables si es una orden de OYDPLUS
                CargarCombosOYDPLUS("INICIO", String.Empty, String.Empty)
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuarioCOMPLETOS", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuarioCOMPLETOS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarTodosReceptoresUsuario(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaReceptoresUsuario = lo.Entities.ToList
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoConsultarReceptoresUsuario(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaReceptoresUsuario = lo.Entities.ToList
                    If lo.UserState <> "INICIO" Then
                        If logEditarRegistro Or logNuevoRegistro Then
                            If lo.UserState = "NUEVO" Then
                                NuevaOrden()
                            Else
                                ObtenerValoresDefectoOYDPLUS(OPCION_RECEPTOR, _OrdenOYDPLUSSelected)
                            End If
                        End If
                    End If
                Else
                    If logNuevoRegistro Or logEditarRegistro Then
                        mostrarMensaje("No hay ningun receptor configurado para el usuario.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logNuevoRegistro = False
                        Editando = False
                        HabilitarNegocio = False
                        HabilitarDuracion = False
                        IsBusy = False
                    End If
                End If
            Else
                If logNuevoRegistro Or logEditarRegistro Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                    HabilitarNegocio = False
                    HabilitarDuracion = False
                End If
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub PosicionarControlValidaciones(ByVal plogOrdenOriginal As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesOF.OrdenOYDPLUSOF, ByVal pobjViewOrdenOYDPLUS As OrdenesPLUSOFView)
        Try
            'Se busca el control para llevarle el foco al control que se requiere
            Dim strMensaje As String = strMensajeValidacion.ToLower

            If strMensaje.Contains("- receptor") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboReceptores")
                Exit Sub
            End If
            If strMensaje.Contains("- tipo orden") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboTipoOrden")
                Exit Sub
            End If
            If strMensaje.Contains("- tipo negocio") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboTipoNegocio")
                Exit Sub
            End If
            If strMensaje.Contains("- tipo producto") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboTipoProducto")
                Exit Sub
            End If
            If strMensaje.Contains("- tipo operación") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboTipoOperacion")
                Exit Sub
            End If
            If strMensaje.Contains("- clasificación") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboClasificacion")
                Exit Sub
            End If
            If strMensaje.Contains("- tipo limite") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboTipoLimite")
                Exit Sub
            End If
            If strMensaje.Contains("- condiciones negociación") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboCondicionesNegociacion")
                Exit Sub
            End If
            If strMensaje.Contains("- forma de pago") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboFormaPago")
                Exit Sub
            End If
            If strMensaje.Contains("- tipo inversión") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboTipoInversion")
                Exit Sub
            End If
            If strMensaje.Contains("- ejecución") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboEjecucion")
                Exit Sub
            End If
            If strMensaje.Contains("- duración") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboDuracion")
                Exit Sub
            End If
            If strMensaje.Contains("- especie") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "txtEspecie")
                Exit Sub
            End If
            If strMensaje.Contains("- cliente") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "txtCliente")
                Exit Sub
            End If
            If strMensaje.Contains("- ordenante") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboOrdenante")
                Exit Sub
            End If
            If strMensaje.Contains("- cuenta depósito") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboCuentaDeposito")
                Exit Sub
            End If
            If strMensaje.Contains("- cantidad") Or
                strMensaje.Contains("- precio") Or
                strMensaje.Contains("- la cantidad") Or
                strMensaje.Contains("- el precio") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "tabItemValoresComisiones")
                Exit Sub
            End If
            If strMensaje.Contains("- fecha recepción") Or
                strMensaje.Contains("- canal recepción") Or
                strMensaje.Contains("- usuario operador") Or
                strMensaje.Contains("- medio verificable") Or
                strMensaje.Contains(" - el valor") Or
                strMensaje.Contains(" - la tasa") Or
                strMensaje.Contains(" - el nominal") Or
                strMensaje.Contains(" - los Dias") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "tabItemDatosLEO")
                Exit Sub
            End If
            If strMensaje.Contains("distribución de comisiones") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "tabItemDistribucionComisiones")
                Exit Sub
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al terminar de mostrar el mensaje de validación.", Me.ToString, "TerminoValidacionMensajeOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoValidarIngresoOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.tblRespuestaValidaciones))
        Try
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
                    Dim logEsHtml As Boolean = False
                    Dim strMensajeDetallesHtml As String = String.Empty
                    Dim strMensajeRetornoHtml As String = String.Empty

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
                            strMensajeRetornoHtml = String.Format("{0}{1}", strMensajeRetornoHtml, li.DetalleRegla)
                        Else
                            logError = True
                            logExitoso = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                            strMensajeRetornoHtml = String.Format("{0}{1}", strMensajeRetornoHtml, li.DetalleRegla)
                        End If

                    Next

                    If logExitoso And
                        logContinuaMostrandoMensaje = False And
                        logRequiereConfirmacion = False And
                        logRequiereJustificacion = False And
                        logRequiereAprobacion = False And
                        logError = False Then

                        strMensajeExitoso = strMensajeExitoso.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("--", String.Format("{0}", vbCrLf))

                        'Valida sí es una orden cruzada.
                        RecargarOrdenDespuesGuardado(strMensajeExitoso)

                    ElseIf logError Then
                        If Not String.IsNullOrEmpty(strMensajeRetornoHtml) Then
                            logEsHtml = True
                            strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", strMensajeRetornoHtml)
                        Else
                            logEsHtml = False
                            strMensajeDetallesHtml = String.Empty
                        End If

                        strMensajeError = strMensajeError.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("--", String.Format("{0}", vbCrLf))

                        mostrarMensaje(strMensajeError, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia, String.Empty, "Reglas incumplidas en los detalles de las ordenes", logEsHtml, strMensajeDetallesHtml)
                        IsBusy = False
                    Else
                        ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.TODOS)
                    End If
                End If
            Else
                IsBusy = False

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub RecargarOrdenDespuesGuardado(ByVal pstrMensaje As String)
        Try
            logCancelarRegistro = False
            logEditarRegistro = False
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logNuevoRegistro = False
            HabilitarOpcionesCruzada = False
            HabilitarEncabezado = False
            HabilitarNegocio = False
            HabilitarEjecucion = False
            HabilitarDuracion = False
            HabilitarFechaVigencia = False
            HabilitarHoraVigencia = False
            HabilitarUsuarioOperador = False

            MostrarControles = Visibility.Collapsed
            MostrarControlMensajes = Visibility.Collapsed
            MostrarCamposAcciones = Visibility.Collapsed
            MostrarCamposRentaFija = Visibility.Collapsed
            MostrarCamposCuentaPropia = Visibility.Collapsed
            MostrarNoEdicion = Visibility.Visible
            MostrarCamposFaciales = Visibility.Collapsed
            MostrarAdicionalesEspecie = Visibility.Collapsed

            'Esto se realiza para habilitar los botones de navegación llamando el metodo TERMINOSUBMITCHANGED
            If dcProxy1.OrdenOYDPLUSOFs.Contains(_OrdenOYDPLUSSelected) Then
                dcProxy1.OrdenOYDPLUSOFs.Add(_OrdenOYDPLUSSelected)
            End If

            'Descripción: Permite establecer el modulo de acuerdo al tipo de negocio - Por: Giovanny Velez Bolivar - 19/05/2014
            Program.VerificarCambiosProxyServidor(dcProxy1)
            dcProxy1.SubmitChanges(AddressOf TerminoSubmitChanges, pstrMensaje)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar de nuevo los parametros.", Me.ToString(), "RecargarOrdenDespuesGuardado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoValidarDiasHabiles(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.ValidarFecha))
        Dim objDatos As OyDPLUSOrdenesOF.ValidarFecha
        Dim strMsg As String = String.Empty
        Dim strAccion As String = String.Empty

        Try
            If Not lo.HasError Then
                objDatos = lo.Entities.FirstOrDefault
                strAccion = lo.UserState.ToString.ToLower

                If Not objDatos Is Nothing Then

                    If objDatos.EsDiaHabil Then

                        If lo.UserState = MSTR_ACCION_VALIDACION_GUARDADO_ORDEN Then
                            dcProxy.ValidarFechas.Clear()
                            dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(MSTR_CALCULAR_DIAS_ORDEN, _OrdenOYDPLUSSelected.FechaOrden, _OrdenOYDPLUSSelected.FechaCumplimiento.Value.Date, -1, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarDiasHabiles, MSTR_ACCION_VALIDACION_GUARDADO_ORDEN_FECHACUMPLIMIENTO)
                        ElseIf lo.UserState = MSTR_ACCION_VALIDACION_GUARDADO_ORDEN_FECHACUMPLIMIENTO Then
                            ValidarOrdenOriginaloCruzada()
                        Else
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
                    Else 'If objDatos.EsDiaHabil Then
                        If logEditarRegistro Or logNuevoRegistro Then

                            'Valida sí la duración es cancelación para que no se muestre el mensaje de error.
                            'sino que se asigne la fecha posterior sugerida por el sistema.
                            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                                If _OrdenOYDPLUSSelected.Duracion = DURACION_CANCELACION And
                                objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN And
                                lo.UserState <> MSTR_ACCION_VALIDACION_GUARDADO_ORDEN And
                                    lo.UserState <> MSTR_ACCION_VALIDACION_GUARDADO_ORDEN_FECHACUMPLIMIENTO And
                                    lo.UserState <> MSTR_CALCULAR_DIAS_PLAZO Then
                                    _OrdenOYDPLUSSelected.FechaVigencia = objDatos.FechaHabilMayor
                                Else
                                    If Not objDatos.FechaHabilMayor Is Nothing Then
                                        strMsg = "La fecha hábil siguiente es " & FormatDateTime(objDatos.FechaHabilMayor, Microsoft.VisualBasic.DateFormat.LongDate) & "."
                                    End If
                                    If Not objDatos.FechaHabilMenor Is Nothing Then
                                        strMsg &= "La fecha hábil anterior es " & FormatDateTime(objDatos.FechaHabilMenor, Microsoft.VisualBasic.DateFormat.LongDate) & "."
                                    End If

                                    If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                                        If lo.UserState = MSTR_ACCION_VALIDACION_GUARDADO_ORDEN_FECHACUMPLIMIENTO Or
                                            lo.UserState = MSTR_CALCULAR_DIAS_PLAZO Then
                                            strMsg = "La fecha de cumplimiento no es un día hábil. " & strMsg & "." & vbNewLine & vbNewLine &
                                                    "Debe seleccionar un día hábil."
                                        Else
                                            strMsg = "La fecha de vigencia no es un día hábil. " & strMsg & "." & vbNewLine & vbNewLine &
                                                "Debe seleccionar un día hábil."
                                        End If
                                    Else
                                        strMsg = "La fecha de vencimiento no es un día hábil. " & strMsg & "." & vbNewLine & vbNewLine &
                                                "Debe seleccionar un día hábil."
                                    End If

                                    mostrarMensaje(strMsg, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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

                        If lo.UserState = MSTR_ACCION_VALIDACION_GUARDADO_ORDEN Or lo.UserState = MSTR_ACCION_VALIDACION_GUARDADO_ORDEN_FECHACUMPLIMIENTO Then
                            IsBusy = False
                        End If

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

    Private Sub TerminoVerificarOrdenModificable(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.OrdenModificable))
        Dim objDatos As OyDPLUSOrdenesOF.OrdenModificable
        Dim plogEditar As Boolean = False
        Dim strAccion As String = String.Empty
        Dim strMsg As String = String.Empty

        Try
            If Not lo.HasError Then
                objDatos = lo.Entities.FirstOrDefault
                strAccion = lo.UserState.ToString.ToUpper

                If Not objDatos Is Nothing Then
                    If objDatos.Modificable = False Then
                        strMsg = objDatos.Mensaje
                        If strAccion = "ANULARORDENOYDPLUS" Then
                            strMsg &= vbNewLine & "La orden no puede ser anulada."
                        End If
                        mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        If objDatos.UltimaModificacion.Equals(_OrdenOYDPLUSSelected.FechaActualizacion) Then
                            plogEditar = True
                        Else
                            plogEditar = False
                        End If
                    End If
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación del estado de la orden", Me.ToString(), "TerminoVerificarOrdenModificable", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
            End If

            If plogEditar And strAccion = "EDITARORDENOYDPLUS" Then
                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                    'La orden se puede editar.
                    EditarOrdenOYDPLUS()
                Else
                    MyBase.RetornarValorEdicionNavegacion()
                    IsBusy = False
                End If
            ElseIf plogEditar And strAccion = "ANULARORDENOYDPLUS" Then
                mostrarMensajePregunta("Comentario para la anulación de la orden",
                                       Program.TituloSistema,
                                       "ANULARORDENOYDPLUS",
                                       AddressOf TerminoMensajePregunta,
                                       True,
                                       "¿Anular la orden?", False, True, True, False)

            Else
                IsBusy = False
                '    mostrarMensaje("La orden fue modificada después que se realizó la consulta para desplegarla en pantalla. Debe actualizarla para poder modificarla.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la validación del estado de la orden", Me.ToString(), "TerminoVerificarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando finaliza la anulación de la orden en el servidor
    ''' </summary>
    Private Sub TerminoAnularOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.OrdenSAE))
        Dim objOrdenSAE As OyDPLUSOrdenesOF.OrdenSAE
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se anuló la orden porque se presentó un problema durante el proceso.", Me.ToString(), "TerminoAnularOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                'lo.MarkErrorAsHandled()
            Else
                objOrdenSAE = lo.Entities.FirstOrDefault
                If objOrdenSAE.TipoMensaje > 1 Then
                    mostrarMensaje(objOrdenSAE.Msg, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    mostrarMensaje("La orden fue anulada correctamente.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    logRefrescarconsultaCambioTab = False
                    VistaSeleccionada = VISTA_APROBADAS
                    FiltrarRegistrosOYDPLUS("P", String.Empty, "TERMINOANULAR", "TERMINOANULAR")
                    logRefrescarconsultaCambioTab = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la anulación de la orden.", Me.ToString(), "TerminoAnularOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerBeneficiariosOrdenes(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.BeneficiariosOrden))
        If Not lo.HasError Then
            ListaBeneficiariosOrdenes = lo.Entities.ToList
        Else
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenes",
                                             Me.ToString(), "TerminoTraerBeneficiariosOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)

        End If
    End Sub

    Private Sub TerminoConsultarLiquidacionesOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.LiquidacionesOrden))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se cargaron las liquidaciones de la orden porque se presentó un problema durante el proceso.", Me.ToString(), "TerminoConsultarLiquidacionesOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            Else
                ListaLiqAsociadasOrdenes = lo.Entities.ToList
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir las liquidaciones de la orden", Me.ToString(), "TerminoConsultarLiquidacionesOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarMejorPrecioEspecie(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.MejorPrecioEspecieOrden))
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
                        Case "DUPLICARORDEN"
                            If objResultado.DialogResult Then
                                IsBusy = True
                                DuplicarOrden()
                            End If
                        Case "PREGUNTARCONFIRMACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.CONFIRMACION, objResultado)
                        Case "PREGUNTARAPROBACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.APROBACION, objResultado)
                        Case "PREGUNTARJUSTIFICACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.JUSTIFICACION, objResultado)
                        Case "ANULARORDENOYDPLUS"
                            If objResultado.DialogResult Then
                                IsBusy = True
                                If Not IsNothing(dcProxy.OrdenSAEs) Then
                                    dcProxy.OrdenSAEs.Clear()
                                End If

                                dcProxy.Load(dcProxy.OYDPLUS_AnularOrdenOYDPLUSQuery(_OrdenOYDPLUSSelected.Clase, _OrdenOYDPLUSSelected.TipoOperacion, _OrdenOYDPLUSSelected.NroOrden, _OrdenOYDPLUSSelected.Version, "", objResultado.Observaciones, Program.Usuario, Program.HashConexion), AddressOf TerminoAnularOrden, "ANULAR")
                            Else
                                IsBusy = True
                                dcProxy1.OYDPLUS_CancelarOrdenOYDPLUS(_OrdenOYDPLUSSelected.NroOrden, "ORDENES", Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarAnularRegistro, String.Empty)
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

    Private Sub TerminoVerificarNombrePlantilla(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verficar el nombre de la plantilla.", Me.ToString(), "TerminoVerificarNombrePlantilla", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            Else
                If lo.Value Then
                    strNombrePlantilla = lo.UserState.ToString
                    viewNombrePlantilla.DialogResult = True
                    PlantillaOrden()
                Else
                    mostrarMensaje("El nombre de la plantilla ya existe, por favor cambie el nombre.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verficar el nombre de la plantilla.", Me.ToString(), "TerminoVerificarNombrePlantilla", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusyPlantilla = False
    End Sub

    Private Sub TerminoConsultarPlantillas(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.OrdenOYDPLUSOF))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar las plantillas.", Me.ToString(), "TerminoConsultarPlantillas", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            Else
                ListaPlantillas = lo.Entities.ToList
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar las plantillas.", Me.ToString(), "TerminoConsultarPlantillas", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusyPlantilla = False
    End Sub

    Private Sub TerminoEliminarPlantillas(ByVal lo As LoadOperation(Of OyDPLUSOrdenesOF.tblRespuestaValidaciones))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar las plantillas.", Me.ToString(), "TerminoConsultarPlantillas", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            Else
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.First.Exitoso Then
                        mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        ConsultarPlantillasOrden()
                    Else
                        mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar las plantillas.", Me.ToString(), "TerminoConsultarPlantillas", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusyPlantilla = False
    End Sub

#End Region

#Region "Funciones Generales"


    Public Sub PrRemoverValoresDic(ByRef pobjDiccionario As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)), ByVal pstrArray As String())
        Try
            For i = 0 To pstrArray.Count - 1
                If pobjDiccionario.ContainsKey(pstrArray(i)) Then pobjDiccionario.Remove(pstrArray(i))
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.",
                                 Me.ToString(), "PrRemoverValoresDic", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ExtraerListaPorCategoria(pobjDiccionario As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)), pstrTopico As String, pstrCategoria As String) As List(Of OYDPLUSUtilidades.CombosReceptor)
        ExtraerListaPorCategoria = New List(Of OYDPLUSUtilidades.CombosReceptor)
        Try
            If pobjDiccionario.ContainsKey(pstrTopico) Then
                Dim objRetorno = From item In pobjDiccionario(pstrTopico)
                                 Select New OYDPLUSUtilidades.CombosReceptor With {.ID = item.ID,
                                                                            .Retorno = item.Retorno,
                                                                            .Descripcion = item.Descripcion,
                                                                            .Categoria = pstrCategoria}
                If objRetorno.Count > 0 Then
                    ExtraerListaPorCategoria = objRetorno.ToList()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.",
                                 Me.ToString(), "ExtraerListaPorCategoria", Application.Current.ToString(), Program.Maquina, ex)
            Return New List(Of OYDPLUSUtilidades.CombosReceptor)
        End Try
    End Function


#End Region

#Region "Eventos"

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Desarrollado por Juan David Correa
    ''' Fecha 14 de Agosto del 2012
    ''' </summary>
    Private Sub _OrdenOYDPLUSSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenOYDPLUSSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "receptor"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Receptor) Then
                            LimpiarControlesOYDPLUS(OPCION_RECEPTOR)
                            MostrarControlMensajes = Visibility.Visible
                            MostrarControles = Visibility.Visible
                            MostrarNoEdicion = Visibility.Collapsed
                            CargarTipoNegocioReceptor(OPCION_RECEPTOR, _OrdenOYDPLUSSelected.Receptor, _Modulo, OPCION_RECEPTOR)
                            CargarMensajeDinamicoOYDPLUS("CERTIFICACIONES", _OrdenOYDPLUSSelected.Receptor, String.Empty, String.Empty, String.Empty)
                        Else
                            MostrarControlMensajes = Visibility.Collapsed
                            MostrarNoEdicion = Visibility.Visible
                            MostrarControles = Visibility.Collapsed
                            ListaMensajes = Nothing
                            ConfiguracionReceptor = Nothing
                            LimpiarControlesOYDPLUS(OPCION_RECEPTOR)
                        End If
                        BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboTipoOrden")
                    End If
                Case "tipoorden"
                    If Not IsNothing(_OrdenOYDPLUSSelected.TipoOrden) Then
                        If logEditarRegistro Or logNuevoRegistro Then
                            ValidarHabilitarNegocio()
                            BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboTipoProducto")

                            If _OrdenOYDPLUSSelected.TipoOrden = TIPOORDEN_DIRECTA Then
                                If ConsultarOrdenesSAE Then
                                    ConsultarOrdenesSAE = False
                                End If
                                ConsultarOrdenesSAE = True
                            Else
                                ConsultarOrdenesSAE = False
                            End If
                        End If
                    End If
                Case "tiponegocio"
                    If Not IsNothing(_OrdenOYDPLUSSelected.TipoNegocio) Then
                        IsBusy = True
                        If Not IsNothing(ListaTipoNegocio) Then
                            If ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = _OrdenOYDPLUSSelected.TipoNegocio).Count > 0 Then
                                TipoNegocioSelected = ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = _OrdenOYDPLUSSelected.TipoNegocio).FirstOrDefault
                            End If
                        End If
                        'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se deshabilitan las opciones de orden cruzada cuando esta es de otras firmas
                        If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJAOF Or _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_ACCIONESOF Then
                            _OrdenOYDPLUSSelected.OrdenCruzadaCliente = False
                            _OrdenOYDPLUSSelected.OrdenCruzadaReceptor = False
                            HabilitarOpcionesCruzada = False
                        Else
                            HabilitarOpcionesCruzada = True
                        End If
                        LimpiarControlesOYDPLUS(OPCION_TIPONEGOCIO)
                        If logEditarRegistro Or logNuevoRegistro Then
                            ValidarHabilitarNegocio()
                            If logRealizarConsultaPropiedades Then
                                ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_TIPONEGOCIO)
                            Else
                                IsBusy = False
                            End If
                        End If
                        If logEditarRegistro Or logNuevoRegistro Then
                            BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboTipoOperacion")
                        End If

                        HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                        CalcularFechaVigenciaOrden(_OrdenOYDPLUSSelected)
                    End If
                Case "tipoproducto"
                    If Not IsNothing(_OrdenOYDPLUSSelected.TipoProducto) Then
                        If logNuevoRegistro Or logEditarRegistro Then
                            ValidarHabilitarNegocio()
                            If ValidarTipoProductoPosicionPropia(_OrdenOYDPLUSSelected) Then
                                LlevarRecetorOrdenDistribucionComision(_OrdenOYDPLUSSelected, False)
                            End If
                        End If
                        BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboTipoNegocio")
                    End If
                Case "tipooperacion"
                    If Not IsNothing(_OrdenOYDPLUSSelected.TipoOperacion) Then
                        If logNuevoRegistro Or logEditarRegistro Then
                            ValidarHabilitarNegocio(True)

                            If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoLimite) And
                            Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Especie) And
                            Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOperacion) Then
                                If _OrdenOYDPLUSSelected.TipoLimite = TIPOMERCADO_PRECIO_ESPECIE Or _OrdenOYDPLUSSelected.TipoLimite = TIPOMERCADO_PORLOMEJOR_PRECIO_ESPECIE Then
                                    ConsultarUltimoPrecioEspecie(_OrdenOYDPLUSSelected.Especie, _OrdenOYDPLUSSelected.TipoOperacion)
                                End If
                            End If
                        End If
                        If logEditarRegistro Or logNuevoRegistro Then
                            If logCalcularValores Then
                                CalcularValorOrden(_OrdenOYDPLUSSelected)
                            End If
                        End If

                        HabilitarConsultaControles(_OrdenOYDPLUSSelected)

                        BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboClasificacion")

                    End If
                Case "idcomitente"
                    If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.IDComitente) Then
                        HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                    End If
                Case "especie"
                    If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Especie) Then
                        HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                    End If
                    'Case "operacioncruzada"
                Case "duracion"
                    If logNuevoRegistro Or logEditarRegistro Then
                        If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Duracion) Then
                            CalcularFechaVigenciaOrden(_OrdenOYDPLUSSelected)
                        End If
                    End If
                    'Case "fechaorden"
                    'If logNuevoRegistro Or logEditarRegistro Then
                    '    Me.CalcularDiasOrdenOYDPLUS(MSTR_CALCULAR_DIAS_ORDEN, -1)
                    'End If
                Case "fechavigencia"
                    If logNuevoRegistro Or logEditarRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.FechaVigencia) Then
                            _OrdenOYDPLUSSelected.HoraVigencia = String.Format("{0:00}:{1:00}:{2:00}",
                                                                               _OrdenOYDPLUSSelected.FechaVigencia.Value.Hour,
                                                                               _OrdenOYDPLUSSelected.FechaVigencia.Value.Minute,
                                                                               _OrdenOYDPLUSSelected.FechaVigencia.Value.Second)
                            Me.CalcularDiasOrdenOYDPLUS(MSTR_CALCULAR_DIAS_ORDEN, _OrdenOYDPLUSSelected, -1)

                            If logCalcularValores Then
                                If Not IsNothing(_OrdenOYDPLUSSelected.FechaVigencia) Then
                                    If ValidarFechaCierreSistema(_OrdenOYDPLUSSelected, "seleccionar esta fecha de vigencia") Then

                                    End If
                                End If
                            End If
                        End If
                    End If
                Case "dias"
                    If logNuevoRegistro Or logEditarRegistro Then
                        If _OrdenOYDPLUSSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                            _OrdenOYDPLUSSelected.DiasRepo = _OrdenOYDPLUSSelected.Dias
                        End If
                        If logCalcularValores Then
                            CalcularValorOrden(_OrdenOYDPLUSSelected)
                        End If
                    End If
                Case "enpesos"
                    HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                Case "fechacumplimiento"
                    If logEditarRegistro Or logNuevoRegistro Then
                        'CAMBIO CALCULOS SOLICITADO POR CASA DE BOLSA
                        'JUAN DAVID CORREA MARZO 2015
                        CalcularDiasPlazo(_OrdenOYDPLUSSelected)
                    End If
                Case "diasrepo"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If logCalcularValores Then
                            'CAMBIO CALCULOS SOLICITADO POR CASA DE BOLSA
                            'JUAN DAVID CORREA MARZO 2015
                            CalcularValorOrden(_OrdenOYDPLUSSelected)
                        End If
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenOYDPLUSSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

#End Region

#Region "Notificaciones"

    Private Const TIPOMENSAJE_NOTIFICACIONOPERACIONES = "SAE_NOTIFICACIONOPERACIONES"

    'Private Const TOPICOORDENES = "ORDENES"
    'Private Const TOPICOAUTORIZACIONES = "AUTORIZACIONES"
    'Private Const TOPICOSETEADOR = "SETEADOR"
    'Private Const TOPICOBUSINTEGRACION = "BUS"
    Dim NroOrdenEditar As Integer = 0


    Public Overrides Sub LlegoNotificacion(pobjInfoNotificacion As A2.Notificaciones.Cliente.clsNotificacion)
        Try

            Dim objNotificacion As clsSAENotificacionOperacion

            If Not String.IsNullOrEmpty(pobjInfoNotificacion.strTipoMensaje) Then

                If pobjInfoNotificacion.strTipoMensaje.ToUpper = TIPOMENSAJE_NOTIFICACIONOPERACIONES Then

                    If Not IsNothing(pobjInfoNotificacion.strInfoMensaje) Then

                        objNotificacion = New clsSAENotificacionOperacion

                        objNotificacion = clsSAENotificacionOperacion.Deserialize(pobjInfoNotificacion.strInfoMensaje)

                        If objNotificacion.strCodigoReceptor = _OrdenOYDPLUSSelected.Receptor And objNotificacion.strClaseOrden = _OrdenOYDPLUSSelected.Clase _
                            And objNotificacion.strTipoOperacion = _OrdenOYDPLUSSelected.TipoOperacion And _OrdenOYDPLUSSelected.TipoOrden = TIPOORDEN_DIRECTA Then

                            If ConsultarOrdenesSAE Then
                                ConsultarOrdenesSAE = False
                            End If

                            ConsultarOrdenesSAE = True

                        End If

                    End If

                End If

                'strTopico = pobjInfoNotificacion.strTopicos.ToUpper

                'mostrarMensaje(pobjInfoNotificacion.strMensajeConsola, "Notificaciones Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                'Select Case strTopico
                '    Case TOPICOSETEADOR
                '        If Not IsNothing(pobjInfoNotificacion.strInfoMensaje.Split("|")) Then
                '            arrResultado = pobjInfoNotificacion.strInfoMensaje.Split("|")
                '            strAccionMensaje = arrResultado(0)
                '            strUsuarioMensaje = arrResultado(1)

                '            If strUsuarioMensaje = Program.Usuario Then
                '                If strAccionMensaje = MensajeNotificacion.AccionEjecutada.Rechazada Then
                '                    Dim strMensajeMostrarUsuario As String = String.Empty

                '                    strMensajeMostrarUsuario = String.Format("Señor Usuario, la Orden nro {0} fue rechazada en la pantalla del Seteador. Las causas de rechazo fueron las siguientes: ", pobjInfoNotificacion.intConsecutivo)

                '                    If logEditarRegistro = False And logNuevoRegistro = False Then
                '                        Dim strCausas As String = String.Empty
                '                        Dim strObservacionUsuario As String = String.Empty
                '                        Dim arrMensajeConsola() As String
                '                        ReDim arrMensajeConsola(2)

                '                        If Not IsNothing(ListaOrdenOYDPLUS) Then
                '                            Dim NroOrden As Integer = 0
                '                            If ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjInfoNotificacion.intConsecutivo).Count > 0 Then
                '                                NroOrdenEditar = ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjInfoNotificacion.intConsecutivo).FirstOrDefault.NroOrden
                '                            End If
                '                        End If

                '                        If NroOrdenEditar <> 0 Then
                '                            If Not IsNothing(pobjInfoNotificacion.strMensajeConsola.Split("*")) Then
                '                                strCausas = arrMensajeConsola(0)
                '                                strObservacionUsuario = arrMensajeConsola(1)

                '                                For Each li In strCausas.Split("|")
                '                                    strMensajeMostrarUsuario = String.Format("{0}{1}{2}", strMensajeMostrarUsuario, vbCrLf, li)
                '                                Next

                '                                strMensajeMostrarUsuario = String.Format("{0}{1}Observaciones: {2}", strMensajeMostrarUsuario, vbCrLf, strObservacionUsuario)

                '                                strMensajeMostrarUsuario = String.Format("{0}{1}¿Desea Editar la Orden?", strMensajeMostrarUsuario, vbCrLf)

                '                                mostrarMensajePregunta(strMensajeMostrarUsuario, _
                '                                                       Program.TituloSistema, _
                '                                                       "CONFIRMACIONSETEADOR", _
                '                                                       AddressOf TerminoPreguntarConfirmacionSeteador)

                '                            End If
                '                        End If
                '                    Else
                '                        mostrarMensaje(strMensajeMostrarUsuario, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '                    End If
                '                ElseIf strAccionMensaje = MensajeNotificacion.AccionEjecutada.EnviadaSAE Then
                '                    If Not IsNothing(ListaOrdenOYDPLUS) Then
                '                        Dim NroOrden As Integer = 0
                '                        If ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjInfoNotificacion.intConsecutivo).Count > 0 Then
                '                            NroOrdenEditar = ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = pobjInfoNotificacion.intConsecutivo).FirstOrDefault.NroOrden
                '                        End If
                '                    End If

                '                    If NroOrdenEditar <> 0 Then
                '                        mostrarMensaje(pobjInfoNotificacion.strMensajeConsola, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                '                    End If
                '                End If
                '            End If
                '        End If


                '    Case TOPICOBUSINTEGRACION
                '        If Not IsNothing(pobjInfoNotificacion.strInfoMensaje.Split("|")) Then
                '            arrResultado = pobjInfoNotificacion.strInfoMensaje.Split("|")
                '            strAccionMensaje = arrResultado(0)
                '            strUsuarioMensaje = arrResultado(1)

                '            If strUsuarioMensaje = Program.Usuario Then
                '                mostrarMensaje(pobjInfoNotificacion.strMensajeConsola, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                '            End If
                '        End If
                '    Case TOPICOAUTORIZACIONES
                '        If logEditarRegistro = False And logNuevoRegistro = False Then
                '            If Not IsNothing(pobjInfoNotificacion.strInfoMensaje.Split("|")) Then
                '                arrResultado = pobjInfoNotificacion.strInfoMensaje.Split("|")
                '                strAccionMensaje = arrResultado(0)
                '                strUsuarioMensaje = arrResultado(1)

                '                If strUsuarioMensaje = Program.Usuario Then
                '                    If VistaSeleccionada = VISTA_APROBADAS Then
                '                        FiltrarRegistrosOYDPLUS("P", String.Empty, "CAMBIOBUS")
                '                    Else
                '                        VistaSeleccionada = VISTA_APROBADAS
                '                    End If
                '                End If
                '            End If
                '        End If
                'End Select

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el mensaje de la notificación.",
                                Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoPreguntarConfirmacionSeteador(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta
            objResultado = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                If logEditarRegistro = False And logNuevoRegistro = False Then
                    ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)

                    If Not IsNothing(ListaOrdenOYDPLUS) Then
                        If ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = NroOrdenEditar).Count > 0 Then
                            OrdenOYDPLUSSelected = ListaOrdenOYDPLUS.Where(Function(i) i.IDNroOrden = NroOrdenEditar).FirstOrDefault
                            EditarRegistro()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
            CantidadConfirmaciones = 0
        End Try
    End Sub

    'Public Sub EnviarMensajeCliente(ByVal pNotificacion As MensajeNotificacion.AccionEjecutada, ByVal pMensajeNotificacion As String, ByVal pConsecutivo As Integer, ByVal pIDConexion As String)
    '    Try
    '        Dim objNC As clsNotificacionCliente = New clsNotificacionCliente

    '        objNC.objInfoNotificacion = New clsNotificacion With {.strTopico = TOPICOORDENES,
    '                                                              .strInfoMensaje = pNotificacion & "|" & Program.Usuario,
    '                                                              .strMensajeConsola = pMensajeNotificacion,
    '                                                              .dtmFechaEnvio = dtmFechaServidor,
    '                                                              .intConsecutivo = pConsecutivo,
    '                                                              .strIdConexion = pIDConexion}

    '        'EnviarNotificacion(objNC)
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al enviar el mensaje de notificación.", _
    '                            Me.ToString(), "EnviarMensajeCliente", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    '#End If


#End Region

#Region "Timer Refrescar pantalla"

    Private _myDispatcherTimerOrdenes As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    Public Sub ReiniciaTimer()
        Try
            If Program.Recarga_Automatica_Activa Then
                If _myDispatcherTimerOrdenes Is Nothing Then
                    _myDispatcherTimerOrdenes = New System.Windows.Threading.DispatcherTimer
                    _myDispatcherTimerOrdenes.Interval = New TimeSpan(0, Program.Par_lapso_recarga_OYDPLUS, 0)
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
        RecargarPantallaOrdenes()
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaOrdenOYDPLUS
    Implements INotifyPropertyChanged

    Private _NroOrden As Integer
    <Display(Name:="Nro Orden")>
    Public Property NroOrden() As Integer
        Get
            Return _NroOrden
        End Get
        Set(ByVal value As Integer)
            _NroOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroOrden"))
        End Set
    End Property

    Private _Version As Integer
    <Display(Name:="Versión")>
    Public Property Version() As Integer
        Get
            Return _Version
        End Get
        Set(ByVal value As Integer)
            _Version = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Version"))
        End Set
    End Property

    Private _EstadoOrden As String
    <Display(Name:="Estado orden")>
    Public Property EstadoOrden() As String
        Get
            Return _EstadoOrden
        End Get
        Set(ByVal value As String)
            _EstadoOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EstadoOrden"))
        End Set
    End Property

    Private _Receptor As String
    <Display(Name:="Receptor")>
    Public Property Receptor() As String
        Get
            Return _Receptor
        End Get
        Set(ByVal value As String)
            _Receptor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Receptor"))
        End Set
    End Property

    Private _TipoOrden As String
    <Display(Name:="Tipo Orden")>
    Public Property TipoOrden() As String
        Get
            Return _TipoOrden
        End Get
        Set(ByVal value As String)
            _TipoOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOrden"))
        End Set
    End Property

    Private _TipoNegocio As String
    <Display(Name:="Tipo Negocio")>
    Public Property TipoNegocio() As String
        Get
            Return _TipoNegocio
        End Get
        Set(ByVal value As String)
            _TipoNegocio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoNegocio"))
        End Set
    End Property

    Private _TipoOperacion As String
    <Display(Name:="Tipo Operación")>
    Public Property TipoOperacion() As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _TipoProducto As String
    <Display(Name:="Tipo Producto")>
    Public Property TipoProducto() As String
        Get
            Return _TipoProducto
        End Get
        Set(ByVal value As String)
            _TipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoProducto"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
