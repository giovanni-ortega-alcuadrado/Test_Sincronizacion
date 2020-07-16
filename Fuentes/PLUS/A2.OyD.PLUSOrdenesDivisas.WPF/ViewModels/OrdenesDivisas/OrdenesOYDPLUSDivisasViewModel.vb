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
Imports A2.Notificaciones.Cliente
Imports System.Windows.Messaging
Imports OpenRiaServices.DomainServices.Client

Public Class OrdenesOYDPLUSDivisasViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSordenesdivisasDomainContext()
                dcProxy1 = New OyDPLUSordenesdivisasDomainContext()
                dcProxyConsulta = New OyDPLUSordenesdivisasDomainContext()
                dcProxyPlantilla = New OyDPLUSordenesdivisasDomainContext()

                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
                mdcProxyUtilidad03 = New OyDPLUSutilidadesDomainContext()

            Else
                dcProxy = New OyDPLUSordenesdivisasDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSordenesdivisasDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyConsulta = New OyDPLUSordenesdivisasDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyPlantilla = New OyDPLUSordenesdivisasDomainContext(New System.Uri(Program.RutaServicioNegocio))

                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad03 = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OyDPLUSordenesdivisasDomainContext.IOYDPLUSOrdenesDivisasDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxy1.DomainClient, WebDomainClient(Of OyDPLUSordenesdivisasDomainContext.IOYDPLUSOrdenesDivisasDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad01.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad02.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad03.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

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

                'Adicionar el topico para ordenes
                'ListaTopicos = New List(Of String)

                'ListaTopicos.Add(TOPICOORDENES)
                'ListaTopicos.Add(TOPICOAUTORIZACIONES)
                'ListaTopicos.Add(TOPICOSETEADOR)
                'ListaTopicos.Add(TOPICOBUSINTEGRACION)

                CargarCombosSistemaExternoOYDPLUS()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
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

    Private dcProxy As OyDPLUSordenesdivisasDomainContext
    Private dcProxy1 As OyDPLUSordenesdivisasDomainContext
    Private dcProxyConsulta As OyDPLUSordenesdivisasDomainContext
    Private dcProxyPlantilla As OyDPLUSordenesdivisasDomainContext
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
    Dim mdtmFechaCierreSistema As Date
    Dim cantidadTotalConfirmacion As Integer = 0
    Dim cantidadTotalJustificacion As Integer = 0
    Dim CantidadTotalAprobaciones As Integer = 0
    Dim logCalcularValores As Boolean = False
    Dim intIDOrdenTimer As Integer = 0
    Dim logRefrescarconsultaCambioTab As Boolean = True
    Dim logCambiarSelected As Boolean = True

    Dim strNombrePlantilla As String = String.Empty
    Dim viewNombrePlantilla As NombrePlantillaOYDPLUSView
    Private STR_URLMOTORCALCULOS As String = ""
    Private LOG_HACERLOGMOTORCALCULOS As Boolean = False
    Private STR_RUTALOGMOTORCALCULOS As String = ""

    Private objNotificacionGrabacion As A2OYDPLUSUtilidades.clsNotificacionesOYDPLUS

    Dim dblTRMDia As Double = 0
    Dim dblIva As Double = 0
    Dim dblIvaOtroscobros As Double = 0
    Dim dblTCPIva As Double = 0
    Dim STR_MONEDADOLAR As String = "USD"
    Dim dtmFechaServidor As DateTime

#End Region

#Region "Constantes"
    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17

    Private Const VISTA_APROBADAS As String = "Aprobadas"
    Private Const VISTA_PENDIENTESAPROBAR As String = "Pendientes aprobar"

    Private Const OPCION_EDITAR As String = "EDITAR"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    Private Const OPCION_TIPONEGOCIO As String = "TIPONEGOCIO"
    Private Const OPCION_RECEPTOR As String = "RECEPTOR"
    Private Const OPCION_PLANTILLA As String = "PLANTILLA"
    Private Const OPCION_CREARORDENPLANTILLA As String = "CREARORDENPLANTILLA"
    Private Const OPCION_COMBOSRECEPTOR As String = "COMBOSRECEPTOR"

    Private Const SISTEMAS_ORIGEN As String = "OYD"
    Private Const SISTEMA_DESTINO As String = "MERCAMSOFT"
    Private Const SISTEMA_OPCION_COMBOS As String = "COMBOS"
    Private Const SISTEMA_OPCION_ELIMINAR As String = "ELIMINAR"
    Private Const SISTEMA_OPCION_ACTUALIZAR As String = "ACTUALIZAR"

    Private Enum TIPOMENSAJEUSUARIO
        CONFIRMACION
        JUSTIFICACION
        APROBACION
        TODOS
    End Enum

    Private Const HOJA_MOTORCALCULOS As String = "OyDPlus.CalculosOrdenesDivisas"

    Private Enum SALIDAS_CAMPOS_ENCABEZADO_DIVISAS
        DBL_SALIDAENCABEZADO_CANTIDADUSD
        DBL_SALIDAENCABEZADO_TASANETA
        DBL_SALIDAENCABEZADO_CANTIDADBRUTA
        DBL_SALIDAENCABEZADO_CANTIDADNETA
        DBL_SALIDAENCABEZADO_IVAOTROSCOBROS
        DBL_SALIDAENCABEZADO_VALORNETO
    End Enum

    Private Enum SALIDAS_CAMPOS_DETALLE_DIVISAS
        DBL_SALIDADETALLE_VALORUSD
        DBL_SALIDADETALLE_IVA
        DBL_SALIDADETALLE_VALORNETO
    End Enum

    Private Enum ENTRADAS_MOTORCALCULOS_ENCABEZADO
        STR_ENTRADA_TIPOOPERACION
        STR_ENTRADA_CONCEPTOGIRO
        STR_ENTRADA_FECHACUMPLIMIENTO
        INT_ENTRADA_CUMPLIMIENTO
        STR_ENTRADA_MONEDA
        DBL_ENTRADA_CANTIDAD
        DBL_ENTRADA_TASADOLAR
        DBL_ENTRADA_CANTIDADUSD
        DBL_ENTRADA_TASADECESIONMESA
        DBL_ENTRADA_TASABRUTA
        DBL_ENTRADA_TASACLIENTE
        DBL_ENTRADA_CANTIDADBRUTA
        DBL_ENTRADA_CANTIDADNETA
        DBL_ENTRADA_OTROSCOBROS
        DBL_ENTRADA_IVAOTROSCOBROS
        DBL_ENTRADA_TOTALNETO
        DBL_ENTRADA_PORCENTAJEIVA
        DBL_ENTRADA_PORCENTAJEIVAOTROSCOBROS
        DBL_ENTRADA_TCPIVA
        DBL_ENTRADA_SPREAD
        DBL_ENTRADA_PAPELETA
        DBL_ENTRADA_TRM
    End Enum

    Private Enum ENTRADAS_MOTORCALCULOS_DETALLE
        STR_ENTRADADETALLE_MONEDA
        STR_ENTRADADETALLE_CONCEPTO
        DBL_ENTRADADETALLE_VALOR
        DBL_ENTRADADETALLE_VALORUSD
        DBL_ENTRADADETALLE_IVA
        DBL_ENTRADADETALLE_VALORNETO
        DBL_ENTRADADETALLE_PORCENTAJEIVAOTROSCOBROS
        DBL_ENTRADADETALLE_TRM
        DBL_ENTRADADETALLE_TASADOLAR
    End Enum

#End Region

#Region "Propiedades para el Detalle"

    '******************************************************** ReceptoresOrdenes 
    Private _ListaDetallePreorden As List(Of OyDPLUSOrdenesDivisas.OrdenDivisasDetalle)
    Public Property ListaDetallePreorden() As List(Of OyDPLUSOrdenesDivisas.OrdenDivisasDetalle)
        Get
            Return _ListaDetallePreorden
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDivisas.OrdenDivisasDetalle))
            _ListaDetallePreorden = value
            MyBase.CambioItem("ListaDetallePreorden")
            MyBase.CambioItem("ListaDetallePreordenPaged")
            If Not IsNothing(_ListaDetallePreorden) Then
                If _ListaDetallePreorden.Count > 0 Then
                    DetallePreordenSelected = _ListaDetallePreorden.FirstOrDefault
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaDetallePreordenPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetallePreorden) Then
                Dim view = New PagedCollectionView(_ListaDetallePreorden)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _DetallePreordenSelected As OyDPLUSOrdenesDivisas.OrdenDivisasDetalle
    Public Property DetallePreordenSelected() As OyDPLUSOrdenesDivisas.OrdenDivisasDetalle
        Get
            Return _DetallePreordenSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesDivisas.OrdenDivisasDetalle)
            If Not IsNothing(value) Then
                _DetallePreordenSelected = value
                MyBase.CambioItem("DetallePreordenSelected")
            End If
        End Set
    End Property

#End Region

#Region "Propiedades de la busqueda"

    Public Property cb As CamposBusquedaOrdenDerivados

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

                        End If
                    End If
                End If
            End If

            MyBase.CambioItem("TipoNegocioSelected")
        End Set
    End Property


#End Region

#Region "Propiedades para el Tipo de Orden"

    Private _ListaOrdenOYDPLUS As List(Of OyDPLUSOrdenesDivisas.OrdenDivisas)
    Public Property ListaOrdenOYDPLUS() As List(Of OyDPLUSOrdenesDivisas.OrdenDivisas)
        Get
            Return _ListaOrdenOYDPLUS
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDivisas.OrdenDivisas))
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

    Private _OrdenDataForm As OyDPLUSOrdenesDivisas.OrdenDivisas = New OyDPLUSOrdenesDivisas.OrdenDivisas
    Public Property OrdenDataForm() As OyDPLUSOrdenesDivisas.OrdenDivisas
        Get
            Return _OrdenDataForm
        End Get
        Set(ByVal value As OyDPLUSOrdenesDivisas.OrdenDivisas)
            _OrdenDataForm = value
            MyBase.CambioItem("OrdenDataForm")
        End Set
    End Property

    Private WithEvents _OrdenOYDPLUSSelected As OyDPLUSOrdenesDivisas.OrdenDivisas
    Public Property OrdenOYDPLUSSelected() As OyDPLUSOrdenesDivisas.OrdenDivisas
        Get
            Return _OrdenOYDPLUSSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesDivisas.OrdenDivisas)
            _OrdenOYDPLUSSelected = value
            MyBase.CambioItem("OrdenOYDPLUSSelected")
            BuscarControlValidacion(ViewOrdenesOYDPLUS, "tabItemValoresComisiones")
            Try
                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                    If (logNuevoRegistro = False Or logEditarRegistro = False) Then
                        HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                    End If

                    ConsultarDetallePreOrden()
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al realizar las consultas de la orden.", _
                                Me.ToString(), "OrdenOYDPLUSSelected", Application.Current.ToString(), Program.Maquina, ex)
                IsBusy = False
            End Try
        End Set
    End Property

    Private _OrdenAnteriorOYDPLUS As OyDPLUSOrdenesDivisas.OrdenDivisas
    Public Property OrdenAnteriorOYDPLUS() As OyDPLUSOrdenesDivisas.OrdenDivisas
        Get
            Return _OrdenAnteriorOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesDivisas.OrdenDivisas)
            _OrdenAnteriorOYDPLUS = value
        End Set
    End Property

    Private _OrdenDuplicarOYDPLUS As OyDPLUSOrdenesDivisas.OrdenDivisas
    Public Property OrdenDuplicarOYDPLUS() As OyDPLUSOrdenesDivisas.OrdenDivisas
        Get
            Return _OrdenDuplicarOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesDivisas.OrdenDivisas)
            _OrdenDuplicarOYDPLUS = value
        End Set
    End Property

    Private _OrdenPlantillaOYDPLUS As OyDPLUSOrdenesDivisas.OrdenDivisas
    Public Property OrdenPlantillaOYDPLUS() As OyDPLUSOrdenesDivisas.OrdenDivisas
        Get
            Return _OrdenPlantillaOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesDivisas.OrdenDivisas)
            _OrdenPlantillaOYDPLUS = value
        End Set
    End Property

    Private _ViewOrdenesOYDPLUS As OrdenesPLUSDivisasView
    Public Property ViewOrdenesOYDPLUS() As OrdenesPLUSDivisasView
        Get
            Return _ViewOrdenesOYDPLUS
        End Get
        Set(ByVal value As OrdenesPLUSDivisasView)
            _ViewOrdenesOYDPLUS = value
        End Set
    End Property

    Private _BusquedaOrdenOyDPlus As CamposBusquedaOrdenDerivados = New CamposBusquedaOrdenDerivados
    Public Property BusquedaOrdenOyDPlus() As CamposBusquedaOrdenDerivados
        Get
            Return _BusquedaOrdenOyDPlus
        End Get
        Set(ByVal value As CamposBusquedaOrdenDerivados)
            _BusquedaOrdenOyDPlus = value
            MyBase.CambioItem("BusquedaOrdenOyDPlus")
        End Set
    End Property

    Private _TituloMonto As String = "Monto"
    Public Property TituloMonto() As String
        Get
            Return _TituloMonto
        End Get
        Set(ByVal value As String)
            _TituloMonto = value
            MyBase.CambioItem("TituloMonto")
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

#Region "Propiedades para cargar Información de los Combos"

    Private _ListaCombosSistemaExterno As List(Of OYDPLUSUtilidades.ItemCombosSistemaExterno)
    Public Property ListaCombosSistemaExterno() As List(Of OYDPLUSUtilidades.ItemCombosSistemaExterno)
        Get
            Return _ListaCombosSistemaExterno
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.ItemCombosSistemaExterno))
            _ListaCombosSistemaExterno = value
            MyBase.CambioItem("ListaCombosSistemaExterno")
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

#Region "Propiedades para la Validacion de la Orden"

    Private _ListaResultadoValidacion As List(Of OyDPLUSOrdenesDivisas.tblRespuestaValidaciones)
    Public Property ListaResultadoValidacion() As List(Of OyDPLUSOrdenesDivisas.tblRespuestaValidaciones)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDivisas.tblRespuestaValidaciones))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
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
                    consultarOrdenantesOYDPLUS(ComitenteSeleccionadoOYDPLUS.IdComitente)
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

#Region "Propiedades genericas"

    Private _Modulo As String = "MERCAMSOFT"
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

    Private _CantidadConfirmaciones As Integer = 0
    Public Property CantidadConfirmaciones() As Integer
        Get
            Return _CantidadConfirmaciones
        End Get
        Set(ByVal value As Integer)
            _CantidadConfirmaciones = value
            If CantidadConfirmaciones > 0 Then
                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
                    GuardarOrdenOYDPLUS(_OrdenOYDPLUSSelected)
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
                    GuardarOrdenOYDPLUS(_OrdenOYDPLUSSelected)
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
                    GuardarOrdenOYDPLUS(_OrdenOYDPLUSSelected)
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
    Public Property HabilitarDuplicar() As Boolean
        Get
            Return _HabilitarDuplicar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDuplicar = value
            MyBase.CambioItem("HabilitarDuplicar")
        End Set
    End Property

    Private _HabilitarCamposDolar As Boolean = False
    Public Property HabilitarCamposDolar() As Boolean
        Get
            Return _HabilitarCamposDolar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposDolar = value
            MyBase.CambioItem("HabilitarCamposDolar")
        End Set
    End Property

    Private _HabilitarCamposNoDolar As Boolean = False
    Public Property HabilitarCamposNoDolar() As Boolean
        Get
            Return _HabilitarCamposNoDolar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposNoDolar = value
            MyBase.CambioItem("HabilitarCamposNoDolar")
        End Set
    End Property

    Private _MostrarCamposConversionDolar As Boolean = False
    Public Property MostrarCamposConversionDolar() As Boolean
        Get
            Return _MostrarCamposConversionDolar
        End Get
        Set(ByVal value As Boolean)
            _MostrarCamposConversionDolar = value
            MyBase.CambioItem("MostrarCamposConversionDolar")
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

    Private _ListaPlantillas As List(Of OyDPLUSOrdenesDivisas.OrdenDivisas)
    Public Property ListaPlantillas() As List(Of OyDPLUSOrdenesDivisas.OrdenDivisas)
        Get
            Return _ListaPlantillas
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDivisas.OrdenDivisas))
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

    Private _PlantillaSeleccionada As OyDPLUSOrdenesDivisas.OrdenDivisas
    Public Property PlantillaSeleccionada() As OyDPLUSOrdenesDivisas.OrdenDivisas
        Get
            Return _PlantillaSeleccionada
        End Get
        Set(ByVal value As OyDPLUSOrdenesDivisas.OrdenDivisas)
            _PlantillaSeleccionada = value
            MyBase.CambioItem("PlantillaSeleccionada")
        End Set
    End Property


#End Region

#End Region

#Region "Metodos OYDPlus"

    Public Overrides Async Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(dblTRMDia) Or dblTRMDia = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se pueden realizar esta acción porque la TRM del día no ha sido cargada en el sistema destino.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'If IsNothing(dblIvaOtroscobros) Or dblIvaOtroscobros = 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("No se pueden realizar esta acción porque iva otros costos no contien valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            IsBusy = True
            dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                OrdenAnteriorOYDPLUS = Nothing
                ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)
            End If

            logNuevoRegistro = True
            logEditarRegistro = False
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logCancelarRegistro = False

            NuevaOrden()
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
            Dim objNewOrdenOYDPLUS As New OyDPLUSOrdenesDivisas.OrdenDivisas
            objNewOrdenOYDPLUS.NroOrden = 0
            objNewOrdenOYDPLUS.Receptor = String.Empty
            objNewOrdenOYDPLUS.TipoNegocio = String.Empty
            objNewOrdenOYDPLUS.NombreTipoNegocio = String.Empty
            objNewOrdenOYDPLUS.TipoProducto = String.Empty
            objNewOrdenOYDPLUS.NombreTipoProducto = String.Empty
            objNewOrdenOYDPLUS.TipoOperacion = String.Empty
            'objNewOrdenOYDPLUS.NombreTipoOperacion = String.Empty
            objNewOrdenOYDPLUS.FechaOrden = dtmFechaServidor
            objNewOrdenOYDPLUS.IDComitente = "-9999999999"
            objNewOrdenOYDPLUS.NombreCliente = "(No Seleccionado)"
            objNewOrdenOYDPLUS.NroDocumento = String.Empty
            objNewOrdenOYDPLUS.CategoriaCliente = String.Empty
            objNewOrdenOYDPLUS.IDOrdenante = String.Empty
            objNewOrdenOYDPLUS.NombreOrdenante = String.Empty
            objNewOrdenOYDPLUS.Cumplimiento = 0
            objNewOrdenOYDPLUS.Monto = 0
            objNewOrdenOYDPLUS.TasaDeCesionMesa = 0
            objNewOrdenOYDPLUS.TasaCliente = 0
            objNewOrdenOYDPLUS.ComisionComercialVIASpread = 0
            objNewOrdenOYDPLUS.ComisionComercialVIAPapeleta = 0
            objNewOrdenOYDPLUS.OtrosCostos = 0
            objNewOrdenOYDPLUS.ValorNeto = 0
            objNewOrdenOYDPLUS.TasaBruta = 0
            objNewOrdenOYDPLUS.CantidadBruta = 0
            objNewOrdenOYDPLUS.CantidadNeta = 0
            objNewOrdenOYDPLUS.TasaDolar = 0
            objNewOrdenOYDPLUS.CantidadUSD = 0
            objNewOrdenOYDPLUS.IvaOtrosCostos = dblIvaOtroscobros
            objNewOrdenOYDPLUS.Iva = dblIva
            objNewOrdenOYDPLUS.TCPIva = dblTCPIva
            objNewOrdenOYDPLUS.TRM = dblTRMDia

            objNewOrdenOYDPLUS.FechaActualizacion = dtmFechaServidor
            objNewOrdenOYDPLUS.EstadoOrden = "P"
            objNewOrdenOYDPLUS.NombreEstadoOrden = "Orden ingresada"
            objNewOrdenOYDPLUS.Usuario = Program.Usuario

            Confirmaciones = String.Empty

            'Limpia la lista de combos cuando es una nueva orden
            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                DiccionarioCombosOYDPlus.Clear()
            End If

            logCalcularValores = False

            ObtenerValoresOrdenAnterior(objNewOrdenOYDPLUS, OrdenOYDPLUSSelected)

            logCalcularValores = True

            OrdenanteSeleccionadoOYDPLUS = Nothing

            Editando = True

            'Valida si tiene un receptor por defecto.
            If Not IsNothing(ListaReceptoresUsuario) Then
                If ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).Count > 0 Then
                    _OrdenOYDPLUSSelected.Receptor = ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).First.CodigoReceptor
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

            'Se posiciona en el primer registro
            BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboReceptores")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevaOrden", Program.TituloSistema, Program.Maquina, ex)
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
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                mostrarMensajePregunta("¿Esta seguro que desea duplicar la orden?", _
                                   Program.TituloSistema, _
                                   "DUPLICARORDEN", _
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

            If Not IsNothing(dcProxyPlantilla.OrdenDivisas) Then
                dcProxyPlantilla.OrdenDivisas.Clear()
            End If
            dcProxyPlantilla.Load(dcProxyPlantilla.OYDPLUS_OrdenesPlantillasConsultarQuery(_FiltroPlantillas, Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPlantillas, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los parametros del receptor.", _
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
                        strPlantillasEliminar = li.ID.ToString
                    Else
                        strPlantillasEliminar = String.Format("{0}|{1}", strPlantillasEliminar, li.ID)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los parametros del receptor.", _
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

            'Crea la nueva orden para duplicar.
            Dim objNewOrdenDuplicar As New OyDPLUSOrdenesDivisas.OrdenDivisas
            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, objNewOrdenDuplicar)

            objNewOrdenDuplicar.ID = 0
            objNewOrdenDuplicar.NroOrden = 0
            objNewOrdenDuplicar.FechaOrden = dtmFechaServidor
            objNewOrdenDuplicar.FechaActualizacion = dtmFechaServidor
            objNewOrdenDuplicar.EstadoOrden = String.Empty
            objNewOrdenDuplicar.NombreEstadoOrden = String.Empty
            objNewOrdenDuplicar.Usuario = Program.Usuario

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = True
            logNuevoRegistro = False

            ObtenerValoresOrdenAnterior(objNewOrdenDuplicar, OrdenDuplicarOYDPLUS)

            CargarTipoNegocioReceptor(OPCION_DUPLICAR, _OrdenOYDPLUSSelected.Receptor, _Modulo, OPCION_DUPLICAR)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro duplicado", Me.ToString(), "DuplicarOrden", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)
        End Try
    End Sub

    Public Async Sub PlantillaOrden()
        Try
            OrdenAnteriorOYDPLUS = Nothing

            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)
            dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()

            'Crea la nueva orden para duplicar.
            Dim objNewOrdenPlantilla As New OyDPLUSOrdenesDivisas.OrdenDivisas
            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, objNewOrdenPlantilla)

            objNewOrdenPlantilla.ID = 0
            objNewOrdenPlantilla.NroOrden = 0
            objNewOrdenPlantilla.FechaOrden = dtmFechaServidor
            objNewOrdenPlantilla.FechaActualizacion = dtmFechaServidor
            objNewOrdenPlantilla.EstadoOrden = String.Empty
            objNewOrdenPlantilla.NombreEstadoOrden = String.Empty
            objNewOrdenPlantilla.Usuario = Program.Usuario

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logPlantillaRegistro = True
            logNuevoRegistro = False
            logPlantillaRegistro = True

            ObtenerValoresOrdenAnterior(objNewOrdenPlantilla, OrdenPlantillaOYDPLUS)

            CargarTipoNegocioReceptor(OPCION_PLANTILLA, _OrdenOYDPLUSSelected.Receptor, _Modulo, OPCION_PLANTILLA)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro plantilla", Me.ToString(), "PlantillaOrden", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)
        End Try
    End Sub

    Public Sub CrearOrdenAPartirPlantilla(ByVal viewPlantilla As ConsultaPlantillasView)
        Try
            Dim intCantidadSeleccionada As Integer = 0

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
            Dim objNewOrdenPlantilla As New OyDPLUSOrdenesDivisas.OrdenDivisas
            ObtenerValoresOrdenAnterior(_PlantillaSeleccionada, objNewOrdenPlantilla)

            objNewOrdenPlantilla.ID = 0
            objNewOrdenPlantilla.NroOrden = 0
            objNewOrdenPlantilla.FechaOrden = dtmFechaServidor
            objNewOrdenPlantilla.FechaActualizacion = dtmFechaServidor
            objNewOrdenPlantilla.EstadoOrden = String.Empty
            objNewOrdenPlantilla.NombreEstadoOrden = String.Empty
            objNewOrdenPlantilla.Usuario = Program.Usuario

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logNuevoRegistro = False
            logPlantillaRegistro = True

            ObtenerValoresOrdenAnterior(objNewOrdenPlantilla, OrdenPlantillaOYDPLUS)

            CargarTipoNegocioReceptor(OPCION_CREARORDENPLANTILLA, _OrdenPlantillaOYDPLUS.Receptor, _Modulo, OPCION_CREARORDENPLANTILLA)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización al crear la plantilla", Me.ToString(), "CrearOrdenAPartirPlantilla", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            'Se sabe si se utiliza en las ordenes por aprobar o en las ordenes aprobadas.
            Dim strEstado As String = String.Empty

            Select Case VistaSeleccionada
                Case VISTA_APROBADAS
                    strEstado = "P"
                Case VISTA_PENDIENTESAPROBAR
                    strEstado = "D"
            End Select

            If Not IsNothing(dcProxyConsulta.OrdenDivisas) Then
                dcProxyConsulta.OrdenDivisas.Clear()
            End If

            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                'Descripción: Agrega el filtro de modulo en la consulta - Por: Giovanny Velez Bolivar - 19/05/2014
                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUSDivisas_FiltrarOrdenQuery(strEstado, TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
            Else
                'Descripción: Agrega el filtro de modulo en la consulta - Por: Giovanny Velez Bolivar - 19/05/2014
                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUSDivisas_FiltrarOrdenQuery(strEstado, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
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

                If Not IsNothing(dcProxyConsulta.OrdenDivisas) Then
                    dcProxyConsulta.OrdenDivisas.Clear()
                End If
                'Descripción: Agrega el filtro de modulo en la consulta - Por: Giovanny Velez Bolivar - 19/05/2014
                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUSDivisas_FiltrarOrdenQuery(pstrEstado, pstrFiltro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, pstrUserState)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar los registros.", _
                     Me.ToString(), "FiltrarRegistrosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            OrganizarNuevaBusqueda()
            MyBase.Buscar()
            'MyBase.CambioItem("visBuscando")
            'MyBase.CambioItem("visNavegando")
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
                If Not IsNothing(dcProxyConsulta.OrdenDivisas) Then
                    dcProxyConsulta.OrdenDivisas.Clear()
                End If

                logRefrescarconsultaCambioTab = False

                If BusquedaOrdenOyDPlus.EstadoOrden = "D" Then
                    VistaSeleccionada = VISTA_PENDIENTESAPROBAR
                Else
                    VistaSeleccionada = VISTA_APROBADAS
                End If

                logRefrescarconsultaCambioTab = True

                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUSDivisas_ConsultarOrdenQuery(BusquedaOrdenOyDPlus.EstadoOrden, BusquedaOrdenOyDPlus.NroOrden, BusquedaOrdenOyDPlus.TipoProducto, BusquedaOrdenOyDPlus.IDComitente, Program.Usuario, Program.HashConexion), _
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
            IsBusy = True

            Await CalcularTotalesOrden()
            ConsultarFechaCumplimientoPreOrden("GUARDAR")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden.", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarGuardadoOrden()
        Try
            If _OrdenOYDPLUSSelected.TipoOperacion = "C" Then
                _OrdenOYDPLUSSelected.ValorNeto = (_OrdenOYDPLUSSelected.Monto * _OrdenOYDPLUSSelected.TasaCliente) + _OrdenOYDPLUSSelected.ComisionComercialVIAPapeleta + _OrdenOYDPLUSSelected.ComisionComercialVIASpread + _OrdenOYDPLUSSelected.OtrosCostos
            Else
                _OrdenOYDPLUSSelected.ValorNeto = (_OrdenOYDPLUSSelected.Monto * _OrdenOYDPLUSSelected.TasaCliente) - (_OrdenOYDPLUSSelected.ComisionComercialVIAPapeleta + _OrdenOYDPLUSSelected.ComisionComercialVIASpread + _OrdenOYDPLUSSelected.OtrosCostos)
            End If

            ActualizarOrdenOYDPLUS(_OrdenOYDPLUSSelected)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden.", Me.ToString(), "ContinuarGuardadoOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cuando se pasan todas las validaciones, sí se requeria confirmación, justificación y aprobación
    ''' se llama este metodo y se realiza el envio de la orden a la base de datos con todos los parametros de la orden.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ActualizarOrdenOYDPLUS(ByVal objOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas)
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


                    For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereAprobacion = True).ToList
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

    Private Sub GuardarOrdenOYDPLUS(ByVal objOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas)
        Try
            Dim strDetallePreorden As String = String.Empty

            IsBusy = True

            If logNuevoRegistro Then
                objOrdenSelected.FechaOrden = dtmFechaServidor
            End If

            If logPlantillaRegistro = False Then
                strNombrePlantilla = String.Empty
            End If

            strDetallePreorden = "<OtrosCobros>"
            For Each obj In _ListaDetallePreorden
                strDetallePreorden &= String.Format("<Cobro><Concepto>{0}</Concepto><Valor>{1}</Valor><Iva>{2}</Iva><ValorNeto>{3}</ValorNeto><IvaOtrosCostos>{4}</IvaOtrosCostos><ValorUSD>{5}</ValorUSD></Cobro>", obj.Concepto, obj.Valor, obj.Iva, obj.ValorNeto, dblIvaOtroscobros, obj.ValorUSD)
            Next
            strDetallePreorden &= "</OtrosCobros>"

            strDetallePreorden = System.Web.HttpUtility.UrlEncode(strDetallePreorden)

            If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                dcProxy.tblRespuestaValidaciones.Clear()
            End If

            dcProxy.Load(dcProxy.OYDPLUSDivisas_ValidarIngresoOrdenQuery(objOrdenSelected.ID, objOrdenSelected.NroOrden, objOrdenSelected.Receptor,
                                                                         objOrdenSelected.FechaOrden, objOrdenSelected.TipoOperacion, objOrdenSelected.EstadoOrden,
                                                                         objOrdenSelected.TipoNegocio, objOrdenSelected.TipoProducto, objOrdenSelected.IDComitente,
                                                                         objOrdenSelected.IDOrdenante, objOrdenSelected.Cumplimiento, objOrdenSelected.FechaCumplimiento, objOrdenSelected.ConceptoGiro,
                                                                         objOrdenSelected.Moneda, objOrdenSelected.Monto, objOrdenSelected.TasaDeCesionMesa, objOrdenSelected.TasaCliente,
                                                                         objOrdenSelected.ComisionComercialVIASpread, objOrdenSelected.ComisionComercialVIAPapeleta, objOrdenSelected.Operador, objOrdenSelected.OtrosCostos, objOrdenSelected.ValorNeto,
                                                                         objOrdenSelected.TasaBruta, objOrdenSelected.CantidadBruta, objOrdenSelected.CantidadNeta, objOrdenSelected.TasaDolar, objOrdenSelected.CantidadUSD,
                                                                         objOrdenSelected.IvaOtrosCostos, dblIva, dblTCPIva, dblTRMDia,
                                                                         Program.Usuario, Program.UsuarioWindows, Program.Maquina, Confirmaciones, ConfirmacionesUsuario, Justificaciones, JustificacionesUsuario,
                                                                         Aprobaciones, AprobacionesUsuario, logPlantillaRegistro, strNombrePlantilla, Modulo, strDetallePreorden, Program.HashConexion), AddressOf TerminoValidarIngresoOrden, String.Empty)
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

            If BorrarCliente = True Then
                BorrarCliente = False
            End If

            BorrarCliente = True

            OrdenAnteriorOYDPLUS = Nothing
            OrdenDuplicarOYDPLUS = Nothing

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden.", Me.ToString(), "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub EditarRegistro()
        Try
            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(dblTRMDia) Or dblTRMDia = 0 Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("No se pueden realizar esta acción porque la TRM del día no ha sido cargada en el sistema destino.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If _OrdenOYDPLUSSelected.ID <> 0 Then
                    dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()

                    validarEstadoOrden(OPCION_EDITAR)
                    logCalcularValores = True
                Else
                    MyBase.RetornarValorEdicionNavegacion()
                    mostrarMensaje("No se ha seleccionado ninguna orden para editar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
                mostrarMensaje("No se ha seleccionado ninguna orden para editar.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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

            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logPlantillaRegistro = False
            logNuevoRegistro = False

            If _OrdenOYDPLUSSelected.Moneda = STR_MONEDADOLAR Then
                HabilitarCamposDolar = True
                HabilitarCamposNoDolar = False
            Else
                HabilitarCamposDolar = False
                HabilitarCamposNoDolar = True
            End If

            CargarCombosOYDPLUS(OPCION_EDITAR, _OrdenOYDPLUSSelected.Receptor, OPCION_EDITAR)

            BuscarControlValidacion(ViewOrdenesOYDPLUS, "tabItemValoresComisiones")

            Dim Mensaje As String = String.Format("Se modifica la orden nro {0}", _OrdenOYDPLUSSelected.ID)

#If HAY_NOTIFICACIONES = 1 Then
            EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.Modificada, Mensaje, _OrdenOYDPLUSSelected.ID, String.Empty)
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
            If _OrdenOYDPLUSSelected.EstadoOrden <> "R" Then
                strMsg = "Solamente las pre-ordenes en estado Rechazado se pueden "
                If pstrAccion = OPCION_EDITAR Then
                    strMsg = strMsg & "editar."
                Else
                    strMsg = strMsg & "anular."
                End If
            End If

            If strMsg.Equals(String.Empty) Then
                If pstrAccion = "ANULARORDENOYDPLUS" Then
                    mostrarMensajePregunta("Comentario para la anulación de la pre-orden", _
                                           Program.TituloSistema, _
                                           "ANULARORDENOYDPLUS", _
                                           AddressOf TerminoMensajePregunta, _
                                           True, _
                                           "¿Anular la pre-orden?", False, True, True, False)
                Else
                    EditarOrdenOYDPLUS()
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
                IsBusy = False
                mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el estado de la orden.", Me.ToString(), "validarEstadoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
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
            HabilitarCamposDolar = False
            HabilitarCamposNoDolar = False

            If BorrarCliente = True Then
                BorrarCliente = False
            End If

            BorrarCliente = True

            ObtenerInformacionCombosCompletos()

            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)

            ObtenerValoresOrdenEnLista(_OrdenOYDPLUSSelected)

            OrdenAnteriorOYDPLUS = Nothing
            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)

            logCalcularValores = False

            IsBusy = False

            Editando = False
            dcProxy.RejectChanges()
            MyBase.CambioItem("Editando")
            MyBase.CancelarEditarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "TerminoCancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCancelarAnularRegistro(ByVal lo As InvokeOperation(Of Integer))
        Try
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la anulación del registro", _
                     Me.ToString(), "TerminoCancelarAnularRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(dblTRMDia) Or dblTRMDia = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se pueden realizar esta acción porque la TRM del día no ha sido cargada en el sistema destino.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If _OrdenOYDPLUSSelected.ID <> 0 Then
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

    Public Overrides Sub CambiarAForma()
        Try
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

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmOtrosCostos"
                    Dim newDetalle As New OyDPLUSOrdenesDivisas.OrdenDivisasDetalle

                    newDetalle.IDPreorden = _OrdenOYDPLUSSelected.ID
                    newDetalle.Valor = 0
                    newDetalle.Iva = 0
                    newDetalle.ValorNeto = 0
                    newDetalle.IvaOtrosCostos = dblIvaOtroscobros
                    newDetalle.Usuario = Program.Usuario
                    newDetalle.UsuarioWindows = Program.UsuarioWindows
                    newDetalle.FechaActualizacion = dtmFechaServidor

                    Dim objListaDetallePreOrden As New List(Of OyDPLUSOrdenesDivisas.OrdenDivisasDetalle)
                    objListaDetallePreOrden = _ListaDetallePreorden

                    If IsNothing(objListaDetallePreOrden) Then
                        objListaDetallePreOrden = New List(Of OyDPLUSOrdenesDivisas.OrdenDivisasDetalle)
                    End If

                    objListaDetallePreOrden.Add(newDetalle)

                    ListaDetallePreorden = objListaDetallePreOrden
                    DetallePreordenSelected = newDetalle

                    MyBase.CambioItem("ListaDetallePreorden")
                    MyBase.CambioItem("ListaDetallePreordenPaged")
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo detalle.", Me.ToString(), "NuevoRegistroDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmOtrosCostos"
                    If Not IsNothing(_ListaDetallePreorden) Then
                        If Not _DetallePreordenSelected Is Nothing Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetallePreordenSelected, ListaDetallePreorden)
                            Dim objListaDetallePreOrden As New List(Of OyDPLUSOrdenesDivisas.OrdenDivisasDetalle)
                            For Each li In _ListaDetallePreorden
                                objListaDetallePreOrden.Add(li)
                            Next

                            If objListaDetallePreOrden.Contains(_DetallePreordenSelected) Then
                                objListaDetallePreOrden.Remove(_DetallePreordenSelected)
                            End If

                            DetallePreordenSelected = Nothing
                            ListaDetallePreorden = objListaDetallePreOrden
                            Program.PosicionarItemLista(DetallePreordenSelected, ListaDetallePreorden, intRegistroPosicionar)
                        End If
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.", Me.ToString(), "BorrarRegistroDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Este metodo limpia las propiedades de busqueda que hallan sido ingresado anteriormente.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OrganizarNuevaBusqueda()
        Try
            Dim objBusqueda As New CamposBusquedaOrdenDerivados
            objBusqueda.NroOrden = 0
            objBusqueda.TipoOperacion = String.Empty
            objBusqueda.TipoProducto = String.Empty
            objBusqueda.IDComitente = String.Empty
            objBusqueda.EstadoOrden = "P"

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.", _
                                 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub CargarCombosSistemaExternoOYDPLUS(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True

            If Not IsNothing(mdcProxyUtilidad03.ItemCombosSistemaExternos) Then
                mdcProxyUtilidad03.ItemCombosSistemaExternos.Clear()
            End If

            mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarCombosOtroSistemaQuery("COMBOS", SISTEMAS_ORIGEN, SISTEMA_DESTINO, SISTEMA_OPCION_COMBOS, String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosSistemaExterno, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos del sistema externo.", _
                                Me.ToString(), "CargarCombosSistemaExternoOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.", _
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar la configuración del receptor.", _
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los parametros del receptor.", _
                                 Me.ToString(), "CargarParametrosReceptor", Application.Current.ToString(), Program.Maquina, ex)
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
                    _OrdenOYDPLUSSelected.TipoNegocio = Nothing
                    _OrdenOYDPLUSSelected.TipoProducto = Nothing
                    _OrdenOYDPLUSSelected.TipoOperacion = Nothing

                    Me.ComitenteSeleccionadoOYDPLUS = Nothing

                    If Not IsNothing(Me.ListaOrdenantesOYDPLUS) Then
                        Me.ListaOrdenantesOYDPLUS.Clear()
                    End If

                    Me.OrdenanteSeleccionadoOYDPLUS = Nothing
                    DiccionarioCombosOYDPlus = Nothing
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al limpiar los controles.", _
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar los receptores del usuario.", _
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información del cliente.", _
                                 Me.ToString(), "SeleccionarCliente", Application.Current.ToString(), Program.Maquina, ex)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información del cliente.", _
                                 Me.ToString(), "SeleccionarCliente", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para tomar los valores por defecto de los controles.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub ObtenerValoresDefectoOYDPLUS(ByVal pstrOpcion As String, ByVal objOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas)
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                Select Case pstrOpcion.ToUpper
                    Case OPCION_RECEPTOR
                        If ListaReceptoresUsuario.Count > 1 Then
                            If ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).Count > 0 Then
                                objOrdenSelected.Receptor = ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).FirstOrDefault.CodigoReceptor
                            End If
                        End If

                        If String.IsNullOrEmpty(objOrdenSelected.Receptor) Then
                            IsBusy = False
                        End If

                    Case OPCION_COMBOSRECEPTOR
                        logRealizarConsultaPropiedades = False
                        If Not IsNothing(ListaParametrosReceptor) And Not IsNothing(ConfiguracionReceptor) Then
                            objOrdenSelected.TipoProducto = AsignarValorTopicoCategoria(objOrdenSelected.TipoProducto, "TIPOPRODUCTO", "TIPOPRODUCTO", String.Empty)

                            If Not IsNothing(_ListaTipoNegocio) Then
                                If _ListaTipoNegocio.Count > 0 Then
                                    objOrdenSelected.TipoNegocio = _ListaTipoNegocio.First.CodigoTipoNegocio
                                Else
                                    If _ListaTipoNegocio.Where(Function(i) i.Prioridad = 0).Count > 0 Then
                                        objOrdenSelected.TipoNegocio = _ListaTipoNegocio.Where(Function(i) i.Prioridad = 0).First.CodigoTipoNegocio
                                    End If
                                End If
                            End If
                        End If

                        IsBusy = False
                    Case OPCION_DUPLICAR
                        If Not IsNothing(ListaParametrosReceptor) And Not IsNothing(ConfiguracionReceptor) Then
                            If Not IsNothing(_OrdenDuplicarOYDPLUS) Then
                                ObtenerValoresOrdenAnterior(OrdenDuplicarOYDPLUS, OrdenOYDPLUSSelected)

                                logModificarDatosTipoNegocio = False
                                logModificarDatosTipoNegocio = True

                                consultarOrdenantesOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                                HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)

                            End If

                            HabilitarConsultaControles(_OrdenOYDPLUSSelected)

                        End If

                        logPlantillaRegistro = False
                        strNombrePlantilla = String.Empty

                        VerificarValoresEnCombos()
                    Case OPCION_PLANTILLA, OPCION_CREARORDENPLANTILLA
                        If Not IsNothing(_OrdenPlantillaOYDPLUS) Then
                            ObtenerValoresOrdenAnterior(OrdenPlantillaOYDPLUS, OrdenOYDPLUSSelected)

                            consultarOrdenantesOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                        End If

                        If pstrOpcion.ToUpper = OPCION_CREARORDENPLANTILLA Then
                            logPlantillaRegistro = False
                            strNombrePlantilla = String.Empty

                            HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                        End If

                        VerificarValoresEnCombos()
                    Case OPCION_EDITAR
                        VerificarValoresEnCombos()

                        If Not IsNothing(_OrdenOYDPLUSSelected) Then
                            HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                        End If
                End Select


                logCalcularValores = True
                logRealizarConsultaPropiedades = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto.", _
                                 Me.ToString(), "ObtenerValoresDefecto", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub VerificarValoresEnCombos()
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoProducto) AndAlso Not ValidarCampoEnDiccionario("TIPOPRODUCTO", _OrdenOYDPLUSSelected.TipoProducto) Then
                    _OrdenOYDPLUSSelected.NombreTipoProducto = String.Empty
                    _OrdenOYDPLUSSelected.TipoProducto = String.Empty
                End If
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOperacion) AndAlso Not ValidarCampoEnDiccionario("TIPOTRANSACCIONES", _OrdenOYDPLUSSelected.TipoOperacion) Then
                    _OrdenOYDPLUSSelected.TipoOperacion = String.Empty

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los valores de los combos.", _
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
                                objRetorno = String.Empty
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valor por defecto del combo.", _
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
    Public Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas, Optional ByVal Opcion As String = "")
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

                If Not IsNothing(_ListaCombosSistemaExterno) Then
                    If _ListaCombosSistemaExterno.Where(Function(i) i.Categoria = "CONCEPTOSGIRO").Count > 0 Then
                        Dim objNuevaListaConceptosGiro As New List(Of OYDPLUSUtilidades.CombosReceptor)
                        For Each li In _ListaCombosSistemaExterno.OrderBy(Function(i) i.Prioridad)
                            If li.Categoria = "CONCEPTOSGIRO" Then
                                objNuevaListaConceptosGiro.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                      .Retorno = li.Retorno,
                                                                                                      .Categoria = li.Categoria,
                                                                                                      .Descripcion = li.Descripcion})
                            End If
                        Next

                        objDiccionario.Add("CONCEPTOSGIRO", objNuevaListaConceptosGiro)
                    End If

                    If _ListaCombosSistemaExterno.Where(Function(i) i.Categoria = "TIPOTRANSACCIONES").Count > 0 Then
                        Dim objNuevaListaTipoOperacion As New List(Of OYDPLUSUtilidades.CombosReceptor)
                        For Each li In _ListaCombosSistemaExterno.OrderBy(Function(i) i.Prioridad)
                            If li.Categoria = "TIPOTRANSACCIONES" Then
                                objNuevaListaTipoOperacion.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                      .Retorno = li.Retorno,
                                                                                                      .Categoria = li.Categoria,
                                                                                                      .Descripcion = li.Descripcion})
                            End If
                        Next

                        objDiccionario.Add("TIPOTRANSACCIONES", objNuevaListaTipoOperacion)
                    End If

                    If _ListaCombosSistemaExterno.Where(Function(i) i.Categoria = "CONCEPTOSOTROSCOBROS").Count > 0 Then
                        Dim objNuevaListaTipoOperacion As New List(Of OYDPLUSUtilidades.CombosReceptor)
                        For Each li In _ListaCombosSistemaExterno.OrderBy(Function(i) i.Prioridad)
                            If li.Categoria = "CONCEPTOSOTROSCOBROS" Then
                                objNuevaListaTipoOperacion.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                      .Retorno = li.Retorno,
                                                                                                      .Categoria = li.Categoria,
                                                                                                      .Descripcion = li.Descripcion})
                            End If
                        Next

                        objDiccionario.Add("CONCEPTOSOTROSCOBROS", objNuevaListaTipoOperacion)
                    End If

                    If _ListaCombosSistemaExterno.Where(Function(i) i.Categoria = "OPERADORES").Count > 0 Then
                        Dim objNuevaListaTipoOperacion As New List(Of OYDPLUSUtilidades.CombosReceptor)
                        For Each li In _ListaCombosSistemaExterno.OrderBy(Function(i) i.Prioridad)
                            If li.Categoria = "OPERADORES" Then
                                objNuevaListaTipoOperacion.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                      .Retorno = li.Retorno,
                                                                                                      .Categoria = li.Categoria,
                                                                                                      .Descripcion = li.Descripcion})
                            End If
                        Next

                        objDiccionario.Add("OPERADORES", objNuevaListaTipoOperacion)
                    End If

                End If

                If ValoresCompletos Then 'Cuando ValoresCompletos = True

                    If _ListaCombosSistemaExterno.Where(Function(i) i.Categoria = "TRMDIA").Count > 0 Then
                        For Each li In _ListaCombosSistemaExterno
                            If li.Categoria = "TRMDIA" Then
                                Try
                                    dblTRMDia = CDbl(li.Retorno)
                                Catch ex As Exception
                                    dblTRMDia = 0
                                End Try
                                Exit For
                            End If
                        Next
                    End If

                    If _ListaCombosSistemaExterno.Where(Function(i) i.Categoria = "IVA").Count > 0 Then
                        For Each li In _ListaCombosSistemaExterno
                            If li.Categoria = "IVA" Then
                                Try
                                    dblIva = CDbl(li.Retorno)
                                Catch ex As Exception
                                    dblIva = 0
                                End Try
                                Exit For
                            End If
                        Next
                    End If

                    If _ListaCombosSistemaExterno.Where(Function(i) i.Categoria = "IVAOTROSCOBROS").Count > 0 Then
                        For Each li In _ListaCombosSistemaExterno
                            If li.Categoria = "IVAOTROSCOBROS" Then
                                Try
                                    dblIvaOtroscobros = CDbl(li.Retorno)
                                Catch ex As Exception
                                    dblIvaOtroscobros = 0
                                End Try
                                Exit For
                            End If
                        Next
                    End If

                    If _ListaCombosSistemaExterno.Where(Function(i) i.Categoria = "TPCIVA").Count > 0 Then
                        For Each li In _ListaCombosSistemaExterno
                            If li.Categoria = "TPCIVA" Then
                                Try
                                    dblTCPIva = CDbl(li.Retorno)
                                Catch ex As Exception
                                    dblTCPIva = 0
                                End Try
                                Exit For
                            End If
                        Next
                    End If

                    If Not IsNothing(objDiccionario) Then
                        If objDiccionario.ContainsKey("MOTORCALCULOS_RUTASERVICIO") Then
                            If objDiccionario("MOTORCALCULOS_RUTASERVICIO").Count > 0 Then
                                STR_URLMOTORCALCULOS = objDiccionario("MOTORCALCULOS_RUTASERVICIO").First.Retorno
                            End If
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

                    If Not IsNothing(objDiccionario) Then
                        If objDiccionario.ContainsKey("CODIGO_MONEDA_DOLAR") Then
                            If objDiccionario("CODIGO_MONEDA_DOLAR").Count > 0 Then
                                STR_MONEDADOLAR = objDiccionario("CODIGO_MONEDA_DOLAR").First.Retorno
                            End If
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

                    DiccionarioCombosOYDPlus = objDiccionario
                Else ' Cuando ValoresCompletos = False
                    If Not String.IsNullOrEmpty(Opcion) Then
                        Dim OpcionValoresDefecto As String = String.Empty


                        Select Case Opcion.ToUpper
                            Case OPCION_RECEPTOR
                                OpcionValoresDefecto = OPCION_COMBOSRECEPTOR
                        End Select

                        DiccionarioCombosOYDPlus = objDiccionario

                        If Opcion.ToUpper = OPCION_EDITAR Then
                            'Se llevan los anteriores a la orden ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.

                            ObtenerValoresOrdenAnterior(OrdenAnteriorOYDPLUS, OrdenOYDPLUSSelected)

                            Editando = True
                            MyBase.CambioItem("Editando")

                            'Se posiciona en el primer registro
                            BuscarControlValidacion(ViewOrdenesOYDPLUS, "cboClasificacion")

                            Confirmaciones = String.Empty
                            Justificaciones = String.Empty
                            Aprobaciones = String.Empty
                            CantidadConfirmaciones = 0
                            CantidadAprobaciones = 0
                            CantidadJustificaciones = 0

                            logModificarDatosTipoNegocio = False
                            logModificarDatosTipoNegocio = True

                            consultarOrdenantesOYDPLUS(_OrdenOYDPLUSSelected.IDComitente, OPCION_EDITAR)
                            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)

                            OpcionValoresDefecto = OPCION_EDITAR

                            IsBusy = False

                        ElseIf Opcion.ToUpper = OPCION_DUPLICAR Or Opcion.ToUpper = OPCION_PLANTILLA Or Opcion.ToUpper = OPCION_CREARORDENPLANTILLA Then
                            'Se llevan los anteriores a la orden ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.
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

                            OpcionValoresDefecto = Opcion.ToUpper

                            IsBusy = False
                        End If

                        ObtenerValoresDefectoOYDPLUS(OpcionValoresDefecto, pobjOrdenSelected)
                    End If

                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto de los combos.", _
                                 Me.ToString(), "ObtenerValoresCombos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para validar si se debe de habilitar la negociación de la pantalla de ordenes.
    ''' </summary>
    Public Sub ValidarHabilitarNegocio(Optional ByVal CambioTipoOperacion As Boolean = False)
        Try
            If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoNegocio) And _
                Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoOperacion) And _
                Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.TipoProducto) Then

                If CambioTipoOperacion = False Then
                    ComitenteSeleccionadoOYDPLUS = Nothing
                    If BorrarCliente Then
                        BorrarCliente = False
                    End If
                    BorrarCliente = True
                End If

                HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
            Else
                ComitenteSeleccionadoOYDPLUS = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la habilitación del negocio.", _
                                Me.ToString(), "ValidarHabilitarNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Seleccionar el cliente elegido en el buscador.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    ''' <param name="pobjCliente"></param>
    ''' <remarks></remarks>
    Public Sub SeleccionarClienteOYDPLUS(ByVal pobjCliente As OYDUtilidades.BuscadorClientes, ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas, Optional ByVal pstrOpcion As String = "")
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

                        BorrarCliente = False
                    End If
                End If
            Else
                If Not IsNothing(pobjOrdenSelected) Then
                    pobjOrdenSelected.IDComitente = "-9999999999"
                    pobjOrdenSelected.NombreCliente = "(No Seleccionado)"
                    pobjOrdenSelected.NroDocumento = 0
                    pobjOrdenSelected.CategoriaCliente = "(No Seleccionado)"
                    pobjOrdenSelected.IDOrdenante = String.Empty

                    OrdenanteSeleccionadoOYDPLUS = Nothing

                    ListaOrdenantesOYDPLUS = Nothing

                    BorrarCliente = True

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarClienteOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para habilitar o deshabilitar los campos dependiendo del tipo de orden que sea.
    ''' Desarrollado por Juan david Correa
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub HabilitarCamposOYDPLUS(ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas)
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                If Not IsNothing(pobjOrdenSelected.Moneda) Then
                    If pobjOrdenSelected.Moneda = STR_MONEDADOLAR Then
                        TituloMonto = "Cantidad"
                        MostrarCamposConversionDolar = False
                    Else
                        TituloMonto = "Cantidad " & pobjOrdenSelected.Moneda
                        MostrarCamposConversionDolar = True
                    End If
                Else
                    TituloMonto = "Cantidad"
                    MostrarCamposConversionDolar = False
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
    Public Sub HabilitarConsultaControles(ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas)
        Try
            If logEditarRegistro Or logNuevoRegistro Then

            Else

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
    ''' Se realizan las validaciones para el guardado de la orden de oydplus.
    ''' Fecha 27 de agosto del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Function ValidarGuardadoOrden(ByVal pobjOrden As OyDPLUSOrdenesDivisas.OrdenDivisas) As Boolean
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
                'Valida el campo de Moneda
                If String.IsNullOrEmpty(pobjOrden.Moneda) Then
                    strMensajeValidacion = String.Format("{0}{1} - Moneda", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo del cliente
                If IsNothing(pobjOrden.IDComitente) Or String.IsNullOrEmpty(pobjOrden.IDComitente) Or pobjOrden.IDComitente = "(No Seleccionado)" Or pobjOrden.IDComitente = "-9999999999" Then
                    strMensajeValidacion = String.Format("{0}{1} - Cliente", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo del ordenante
                If IsNothing(pobjOrden.IDOrdenante) Or String.IsNullOrEmpty(pobjOrden.IDOrdenante) Then
                    strMensajeValidacion = String.Format("{0}{1} - Ordenante", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de fecha
                If IsNothing(pobjOrden.FechaOrden) Then
                    strMensajeValidacion = String.Format("{0}{1} - Fecha orden", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Cumplimiento
                If IsNothing(pobjOrden.Cumplimiento) Then
                    strMensajeValidacion = String.Format("{0}{1} - Cumplimiento", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de fecha cumplimiento
                If IsNothing(pobjOrden.FechaCumplimiento) Then
                    strMensajeValidacion = String.Format("{0}{1} - Fecha cumplimiento", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Operador
                If String.IsNullOrEmpty(pobjOrden.Operador) Then
                    strMensajeValidacion = String.Format("{0}{1} - Operador", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                If HabilitarCamposNoDolar Then
                    If IsNothing(pobjOrden.Monto) Or pobjOrden.Monto = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Cantidad", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                    If IsNothing(pobjOrden.TasaDolar) Or pobjOrden.TasaDolar = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Tasa dolar", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                Else
                    If IsNothing(pobjOrden.CantidadUSD) Or pobjOrden.CantidadUSD = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Cantidad USD", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
                
                'Valida el campo de Tasa de cesión mesa
                If IsNothing(pobjOrden.TasaDeCesionMesa) Or pobjOrden.TasaDeCesionMesa = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Tasa de cesion mesa", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Tasa cliente
                If IsNothing(pobjOrden.TasaBruta) Or pobjOrden.TasaBruta = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Tasa Bruta/Tasa negociación", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                'Valida el campo de Tasa cliente
                If IsNothing(_ListaDetallePreorden) Then
                    For Each li In _ListaDetallePreorden
                        If String.IsNullOrEmpty(li.Concepto) Then
                            strMensajeValidacion = String.Format("{0}{1} - Debe de ingresar el Concepto en el detalle de Otros costos", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                            Exit For
                        End If
                        If HabilitarCamposNoDolar Then
                            If IsNothing(li.Valor) Or li.Valor = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} - Debe de ingresar el Valor en el detalle de Otros costos", strMensajeValidacion, vbCrLf)
                                logTieneError = True
                                Exit For
                            End If
                        Else
                            If IsNothing(li.ValorUSD) Or li.ValorUSD = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} - Debe de ingresar el Valor en el detalle de Otros costos", strMensajeValidacion, vbCrLf)
                                logTieneError = True
                                Exit For
                            End If
                        End If
                    Next
                End If

                If logTieneError Then
                    strMensajeValidacion = String.Format("Señor usuario los siguientes campos son requeridos:{0}", strMensajeValidacion)
                    mostrarMensaje(strMensajeValidacion, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                Else
                    strMensajeValidacion = String.Empty
                    Return True
                End If
            Else
                mostrarMensaje("Señor Usuario, la orden tiene que tener un dato como minimo.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
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
    Public Sub BuscarControlValidacion(ByVal pViewOrdenes As OrdenesPLUSDivisasView, ByVal pstrOpcion As String)
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
    ''' Metodo para obtener la información completa de los combos de la aplicación cuando sea necesario.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 10 de Septiembre del 2012
    ''' </summary>
    Public Sub ObtenerInformacionCombosCompletos()
        Try
            Dim objDiccionarioCombosOYDPlus As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
            Dim objTipoNegocioReceptor As New List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor)
            Dim strNombreCategoria As String = String.Empty

            For Each dic In DiccionarioCombosOYDPlusCompleta
                strNombreCategoria = dic.Key
                objDiccionarioCombosOYDPlus.Add(strNombreCategoria, dic.Value)
            Next

            DiccionarioCombosOYDPlus = Nothing
            DiccionarioCombosOYDPlus = objDiccionarioCombosOYDPlus

            ListaReceptoresUsuario = Nothing
            ListaReceptoresUsuario = ListaReceptoresCompleta.OrderBy(Function(i) i.ID).ToList

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
    Public Sub ObtenerValoresOrdenAnterior(ByVal pobjOrden As OyDPLUSOrdenesDivisas.OrdenDivisas, ByRef pobjOrdenSalvarDatos As OyDPLUSOrdenesDivisas.OrdenDivisas)
        Try
            If Not IsNothing(pobjOrden) Then
                Dim objNewOrdenOYD As New OyDPLUSOrdenesDivisas.OrdenDivisas

                objNewOrdenOYD.ID = pobjOrden.ID
                objNewOrdenOYD.NroOrden = pobjOrden.NroOrden
                objNewOrdenOYD.Receptor = pobjOrden.Receptor
                objNewOrdenOYD.NombreReceptor = pobjOrden.NombreReceptor
                objNewOrdenOYD.TipoNegocio = pobjOrden.TipoNegocio
                objNewOrdenOYD.NombreTipoNegocio = pobjOrden.NombreTipoNegocio
                objNewOrdenOYD.TipoProducto = pobjOrden.TipoProducto
                objNewOrdenOYD.NombreTipoProducto = pobjOrden.NombreTipoProducto
                objNewOrdenOYD.TipoOperacion = pobjOrden.TipoOperacion
                objNewOrdenOYD.FechaOrden = pobjOrden.FechaOrden
                objNewOrdenOYD.EstadoOrden = pobjOrden.EstadoOrden
                objNewOrdenOYD.NombreEstadoOrden = pobjOrden.NombreEstadoOrden
                objNewOrdenOYD.IDComitente = pobjOrden.IDComitente
                objNewOrdenOYD.NombreCliente = pobjOrden.NombreCliente
                objNewOrdenOYD.NroDocumento = pobjOrden.NroDocumento
                objNewOrdenOYD.CategoriaCliente = pobjOrden.CategoriaCliente
                objNewOrdenOYD.IDOrdenante = pobjOrden.IDOrdenante
                objNewOrdenOYD.NombreOrdenante = pobjOrden.NombreOrdenante
                objNewOrdenOYD.ConceptoGiro = pobjOrden.ConceptoGiro
                objNewOrdenOYD.Cumplimiento = pobjOrden.Cumplimiento
                objNewOrdenOYD.FechaCumplimiento = pobjOrden.FechaCumplimiento
                objNewOrdenOYD.Operador = pobjOrden.Operador
                objNewOrdenOYD.Moneda = pobjOrden.Moneda
                objNewOrdenOYD.NombreMoneda = pobjOrden.NombreMoneda
                objNewOrdenOYD.Notificacion = pobjOrden.Notificacion
                objNewOrdenOYD.Monto = pobjOrden.Monto
                objNewOrdenOYD.TasaDolar = pobjOrden.TasaDolar
                objNewOrdenOYD.CantidadUSD = pobjOrden.CantidadUSD
                objNewOrdenOYD.TasaBruta = pobjOrden.TasaBruta
                objNewOrdenOYD.TasaCliente = pobjOrden.TasaCliente
                objNewOrdenOYD.TasaDeCesionMesa = pobjOrden.TasaDeCesionMesa
                objNewOrdenOYD.CantidadBruta = pobjOrden.CantidadBruta
                objNewOrdenOYD.CantidadNeta = pobjOrden.CantidadNeta
                objNewOrdenOYD.IvaOtrosCostos = pobjOrden.IvaOtrosCostos
                objNewOrdenOYD.Usuario = pobjOrden.Usuario
                objNewOrdenOYD.FechaActualizacion = pobjOrden.FechaActualizacion
                objNewOrdenOYD.Iva = pobjOrden.Iva
                objNewOrdenOYD.TCPIva = pobjOrden.TCPIva
                objNewOrdenOYD.ComisionComercialVIAPapeleta = pobjOrden.ComisionComercialVIAPapeleta
                objNewOrdenOYD.ComisionComercialVIASpread = pobjOrden.ComisionComercialVIASpread
                objNewOrdenOYD.OtrosCostos = pobjOrden.OtrosCostos
                objNewOrdenOYD.ValorNeto = pobjOrden.ValorNeto

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
    Public Sub ObtenerValoresOrdenEnLista(ByVal pobjOrden As OyDPLUSOrdenesDivisas.OrdenDivisas)
        Try
            If Not IsNothing(pobjOrden) Then
                If ListaOrdenOYDPLUS.Where(Function(i) i.ID = pobjOrden.ID).Count > 0 Then
                    ListaOrdenOYDPLUS.Where(Function(i) i.ID = pobjOrden.ID).FirstOrDefault.Receptor = pobjOrden.Receptor
                    ListaOrdenOYDPLUS.Where(Function(i) i.ID = pobjOrden.ID).FirstOrDefault.TipoNegocio = pobjOrden.TipoNegocio
                    ListaOrdenOYDPLUS.Where(Function(i) i.ID = pobjOrden.ID).FirstOrDefault.TipoProducto = pobjOrden.TipoProducto
                    ListaOrdenOYDPLUS.Where(Function(i) i.ID = pobjOrden.ID).FirstOrDefault.TipoOperacion = pobjOrden.TipoOperacion
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", Me.ToString(), "ObtenerValoresOrdenAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub LimpiarDatosTipoNegocio()
        Try
            ComitenteSeleccionadoOYDPLUS = Nothing
            If Not IsNothing(ListaOrdenantesOYDPLUS) Then
                ListaOrdenantesOYDPLUS.Clear()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los datos del tipo de negocio.", Me.ToString(), "LimpiarDatosTipoNegocio", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub RecargarPantallaOrdenes()
        Try
            If logEditarRegistro = False And logNuevoRegistro = False Then
                If Not IsNothing(ListaOrdenOYDPLUS) Then
                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                        If ListaOrdenOYDPLUS.Count > 0 Then
                            If ListaOrdenOYDPLUS.First.ID = _OrdenOYDPLUSSelected.ID Then
                                Dim vista As String = VistaSeleccionada
                                VistaSeleccionada = String.Empty
                                VistaSeleccionada = vista
                            Else
                                intIDOrdenTimer = _OrdenOYDPLUSSelected.ID

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

    Public Sub MostrarNotificacion()
        Try
            If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Notificacion) Then
                If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.NotificacionDescripcion) Then
                    Dim objListaDescripciones As New A2OYDPLUSUtilidades.ListaNotificacionesView(_OrdenOYDPLUSSelected.NotificacionDescripcion)
                    Program.Modal_OwnerMainWindowsPrincipal(objListaDescripciones)
                    objListaDescripciones.ShowDialog()
                End If

                _OrdenOYDPLUSSelected.Notificacion = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al mostrar la notificación.", Me.ToString(), "MostrarNotificacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ConsultarFechaCumplimientoPreOrden(Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not IsNothing(dcProxy1.tblFechaCumplimientos) Then
                    dcProxy1.tblFechaCumplimientos.Clear()
                End If

                dcProxy1.Load(dcProxy1.OYDPLUS_ConsultarFechaCumplimientoQuery(_OrdenOYDPLUSSelected.FechaOrden, _OrdenOYDPLUSSelected.Cumplimiento, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFechaCumplimiento, pstrUserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cumplimiento de la orden.", _
                                 Me.ToString(), "ConsultarFechaCumplimientoPreOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ConsultarDetallePreOrden(Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not IsNothing(dcProxyConsulta.OrdenDivisasDetalles) Then
                    dcProxyConsulta.OrdenDivisasDetalles.Clear()
                End If

                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUSDivisas_ConsultarDetalleOrdenQuery(_OrdenOYDPLUSSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetallePreOrden, pstrUserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los detalles de la preorden.", _
                                 Me.ToString(), "ConsultarDetallePreOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Async Function CalcularTotalesOrden() As System.Threading.Tasks.Task
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If Not IsNothing(_ListaDetallePreorden) Then
                    For Each liDetalle In _ListaDetallePreorden
                        Await CalcularValorOrden(True, _OrdenOYDPLUSSelected, liDetalle)
                    Next
                End If

                Await CalcularValorOrden(False, _OrdenOYDPLUSSelected, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular los totales de la orden.", Me.ToString(), "CalcularTotalesOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Metodo para validar el valor de la orden, sí es un repo se calcula el valor del repo.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 02 de Octubre del 2012
    ''' CAMBIO CALCULOS SOLICITADO POR CASA DE BOLSA
    ''' JUAN DAVID CORREA MARZO 2015
    ''' </summary>
    Public Async Function CalcularValorOrden(ByVal plogDetalle As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas, ByVal pobjOrdenDetalleSelected As OyDPLUSOrdenesDivisas.OrdenDivisasDetalle) As System.Threading.Tasks.Task
        Try
            If Not IsNothing(pobjOrdenSelected) Then

                If Not String.IsNullOrEmpty(pobjOrdenSelected.TipoNegocio) And Not String.IsNullOrEmpty(pobjOrdenSelected.TipoOperacion) Then
                    If Not IsNothing(_ListaDetallePreorden) Then
                        Dim dblValorOtrosCobros As Double = 0

                        For Each li In _ListaDetallePreorden
                            If HabilitarCamposDolar = False Then
                                dblValorOtrosCobros += li.Valor
                            Else
                                dblValorOtrosCobros += li.ValorUSD
                            End If
                        Next

                        pobjOrdenSelected.OtrosCostos = dblValorOtrosCobros
                    End If

                    If Await ObtenerCalculosMotor(plogDetalle, pobjOrdenSelected, pobjOrdenDetalleSelected) Then

                    End If
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor de la orden.", Me.ToString(), "CalcularValorOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Private Async Function ObtenerCalculosMotor(ByVal plogDetalle As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas, ByVal pobjOrdenDetalleSelected As OyDPLUSOrdenesDivisas.OrdenDivisasDetalle) As System.Threading.Tasks.Task(Of Boolean)
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

                    Dim objListaValoresEntrada As Dictionary(Of String, String) = ArmarEntradaMotorCalculos(plogDetalle, pobjOrdenSelected, pobjOrdenDetalleSelected)
                    Dim objListaValoresSalida As List(Of String) = ArmarSalidasMotorCalculos(plogDetalle, pobjOrdenSelected, pobjOrdenDetalleSelected)


                    If Not IsNothing(objListaValoresEntrada) And Not IsNothing(objListaValoresSalida) Then
                        Dim objDiccionarioRespuesta As Dictionary(Of String, String) = Await objCalculos.ProcesarTaskAsync(HOJA_MOTORCALCULOS, objListaValoresEntrada, objListaValoresSalida, objListaParametros)

                        If plogDetalle = False Then
                            If HabilitarCamposDolar = False Then
                                pobjOrdenSelected.CantidadUSD = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_CANTIDADUSD.ToString, 2)
                            End If

                            pobjOrdenSelected.TasaCliente = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_TASANETA.ToString, 2)
                            pobjOrdenSelected.CantidadBruta = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_CANTIDADBRUTA.ToString, 2)
                            pobjOrdenSelected.CantidadNeta = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_CANTIDADNETA.ToString, 2)
                            pobjOrdenSelected.IvaOtrosCostos = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_IVAOTROSCOBROS.ToString, 2)
                            pobjOrdenSelected.ValorNeto = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_VALORNETO.ToString, 2)
                        Else
                            pobjOrdenDetalleSelected.ValorUSD = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_DETALLE_DIVISAS.DBL_SALIDADETALLE_VALORUSD.ToString, 2)
                            pobjOrdenDetalleSelected.Iva = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_DETALLE_DIVISAS.DBL_SALIDADETALLE_IVA.ToString, 2)
                            pobjOrdenDetalleSelected.ValorNeto = RetornarValorSalida(objDiccionarioRespuesta, SALIDAS_CAMPOS_DETALLE_DIVISAS.DBL_SALIDADETALLE_VALORNETO.ToString, 2)
                        End If

                        logLlamadoExitoso = True
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor en el motor de calculos.", Me.ToString(), "ObtenerCalculosMotor", Program.TituloSistema, Program.Maquina, ex)
        End Try

        Return logLlamadoExitoso
    End Function

    Private Function ArmarEntradaMotorCalculos(ByVal plogDetalle As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas, ByVal pobjOrdenDetalleSelected As OyDPLUSOrdenesDivisas.OrdenDivisasDetalle) As Dictionary(Of String, String)
        Try
            Dim strValorRetorno As New Dictionary(Of String, String)

            If plogDetalle = False Then
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.STR_ENTRADA_TIPOOPERACION.ToString, VerificarValorEntrada(pobjOrdenSelected.TipoOperacion))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.STR_ENTRADA_CONCEPTOGIRO.ToString, VerificarValorEntrada(pobjOrdenSelected.ConceptoGiro))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.STR_ENTRADA_FECHACUMPLIMIENTO.ToString, VerificarValorEntrada(pobjOrdenSelected.FechaCumplimiento))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.INT_ENTRADA_CUMPLIMIENTO.ToString, VerificarValorEntrada(pobjOrdenSelected.Cumplimiento))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.STR_ENTRADA_MONEDA.ToString, VerificarValorEntrada(pobjOrdenSelected.Moneda))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_CANTIDAD.ToString, VerificarValorEntrada(pobjOrdenSelected.Monto, 0, "0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_TASADOLAR.ToString, VerificarValorEntrada(pobjOrdenSelected.TasaDolar, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_CANTIDADUSD.ToString, VerificarValorEntrada(pobjOrdenSelected.CantidadUSD, 0, "0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_TASADECESIONMESA.ToString, VerificarValorEntrada(pobjOrdenSelected.TasaDeCesionMesa, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_TASABRUTA.ToString, VerificarValorEntrada(pobjOrdenSelected.TasaBruta, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_TASACLIENTE.ToString, VerificarValorEntrada(pobjOrdenSelected.TasaCliente, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_CANTIDADBRUTA.ToString, VerificarValorEntrada(pobjOrdenSelected.CantidadBruta, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_CANTIDADNETA.ToString, VerificarValorEntrada(pobjOrdenSelected.CantidadNeta, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_OTROSCOBROS.ToString, VerificarValorEntrada(pobjOrdenSelected.OtrosCostos, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_IVAOTROSCOBROS.ToString, VerificarValorEntrada(pobjOrdenSelected.IvaOtrosCostos, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_TOTALNETO.ToString, VerificarValorEntrada(pobjOrdenSelected.ValorNeto, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_PORCENTAJEIVA.ToString, VerificarValorEntrada(dblIva, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_PORCENTAJEIVAOTROSCOBROS.ToString, VerificarValorEntrada(dblIvaOtroscobros, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_TCPIVA.ToString, VerificarValorEntrada(dblTCPIva, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_SPREAD.ToString, VerificarValorEntrada(pobjOrdenSelected.ComisionComercialVIASpread, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_PAPELETA.ToString, VerificarValorEntrada(pobjOrdenSelected.ComisionComercialVIAPapeleta, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_ENCABEZADO.DBL_ENTRADA_TRM.ToString, VerificarValorEntrada(dblTRMDia, 2, "0.0"))
            Else
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.STR_ENTRADADETALLE_MONEDA.ToString, VerificarValorEntrada(pobjOrdenSelected.Moneda))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.STR_ENTRADADETALLE_CONCEPTO.ToString, VerificarValorEntrada(pobjOrdenDetalleSelected.Concepto))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.DBL_ENTRADADETALLE_VALOR.ToString, VerificarValorEntrada(pobjOrdenDetalleSelected.Valor, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.DBL_ENTRADADETALLE_VALORUSD.ToString, VerificarValorEntrada(pobjOrdenDetalleSelected.ValorUSD, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.DBL_ENTRADADETALLE_IVA.ToString, VerificarValorEntrada(pobjOrdenDetalleSelected.Iva, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.DBL_ENTRADADETALLE_VALORNETO.ToString, VerificarValorEntrada(pobjOrdenDetalleSelected.ValorNeto, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.DBL_ENTRADADETALLE_PORCENTAJEIVAOTROSCOBROS.ToString, VerificarValorEntrada(dblIvaOtroscobros, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.DBL_ENTRADADETALLE_TRM.ToString, VerificarValorEntrada(dblTRMDia, 2, "0.0"))
                strValorRetorno.Add(ENTRADAS_MOTORCALCULOS_DETALLE.DBL_ENTRADADETALLE_TASADOLAR.ToString, VerificarValorEntrada(pobjOrdenSelected.TasaDolar, 2, "0.0"))
            End If

            Return strValorRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para construir los parametros de entrada del procedimiento.", Me.ToString(), "ArmarEntradaMotorCalculos", Program.TituloSistema, Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

    Private Function ArmarSalidasMotorCalculos(ByVal plogDetalle As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas, ByVal pobjOrdenDetalleSelected As OyDPLUSOrdenesDivisas.OrdenDivisasDetalle) As List(Of String)
        Try
            Dim objListaRetorno As New List(Of String)

            If plogDetalle = False Then
                objListaRetorno.Add(SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_CANTIDADUSD.ToString)
                objListaRetorno.Add(SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_TASANETA.ToString)
                objListaRetorno.Add(SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_CANTIDADBRUTA.ToString)
                objListaRetorno.Add(SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_CANTIDADNETA.ToString)
                objListaRetorno.Add(SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_IVAOTROSCOBROS.ToString)
                objListaRetorno.Add(SALIDAS_CAMPOS_ENCABEZADO_DIVISAS.DBL_SALIDAENCABEZADO_VALORNETO.ToString)
            Else
                objListaRetorno.Add(SALIDAS_CAMPOS_DETALLE_DIVISAS.DBL_SALIDADETALLE_VALORUSD.ToString)
                objListaRetorno.Add(SALIDAS_CAMPOS_DETALLE_DIVISAS.DBL_SALIDADETALLE_IVA.ToString)
                objListaRetorno.Add(SALIDAS_CAMPOS_DETALLE_DIVISAS.DBL_SALIDADETALLE_VALORNETO.ToString)
            End If

            Return objListaRetorno
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

    Private Function RetornarValorEntrada(ByVal pstrValorRetorno As String, ByVal pintOpcion As ENTRADAS_MOTORCALCULOS_ENCABEZADO, ByVal pobjValor As Object, Optional ByVal pintNroDecimales As Integer = 0) As String
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

    Private Function RetornarValorEntrada(ByVal pstrValorRetorno As String, ByVal pintOpcion As ENTRADAS_MOTORCALCULOS_DETALLE, ByVal pobjValor As Object, Optional ByVal pintNroDecimales As Integer = 0) As String
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


#End Region

#Region "Resultados Asincrónicos OYDPlus"

    Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDivisas.OrdenDivisas))
        Try
            If Not lo.HasError Then

                If lo.UserState = "TERMINOGUARDAREDICION" Or lo.UserState = "TIMERSELECTED" Or lo.UserState = "TERMINOGUARDARPENDIENTES" Then
                    logCambiarSelected = False
                Else
                    logCambiarSelected = True
                End If

                Dim intNroOrden As String = String.Empty
                Dim strEstado As String = String.Empty
                Dim strDescripcionEstado As String = String.Empty
                Dim strNotificacion As String = String.Empty
                Dim strNotificacionDescripcion As String = String.Empty

                If lo.UserState = "TERMINOGUARDAREDICION" Or lo.UserState = "TERMINOGUARDARNUEVO" Then
                    If Not IsNothing(_ListaOrdenOYDPLUS) Then
                        If _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).Count > 0 Then
                            If Not String.IsNullOrEmpty(_ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.Notificacion) Then
                                intNroOrden = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.NroOrden
                                strEstado = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.EstadoOrden
                                strDescripcionEstado = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.NombreEstadoOrden
                                strNotificacion = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.Notificacion
                                strNotificacionDescripcion = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.NotificacionDescripcion
                            End If
                        End If
                    End If

                    If String.IsNullOrEmpty(strNotificacion) And Not IsNothing(objNotificacionGrabacion) Then
                        intNroOrden = objNotificacionGrabacion.NroRegistro
                        strEstado = objNotificacionGrabacion.Estado
                        strDescripcionEstado = objNotificacionGrabacion.DescripcionEstado
                        strNotificacion = objNotificacionGrabacion.EstadoMostrar
                        strNotificacionDescripcion = objNotificacionGrabacion.ListaDescripciones
                    End If
                End If

                ListaOrdenOYDPLUS = dcProxyConsulta.OrdenDivisas.ToList

                If Not String.IsNullOrEmpty(strNotificacion) Then
                    If _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).Count > 0 Then
                        _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.NroOrden = intNroOrden
                        _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.EstadoOrden = strEstado
                        _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.NombreEstadoOrden = strDescripcionEstado
                        _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.Notificacion = strNotificacion
                        _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.NotificacionDescripcion = strNotificacionDescripcion
                    End If
                End If

                If lo.UserState = "BUSQUEDA" Then
                    If _ListaOrdenOYDPLUS.Count = 0 Then
                        mostrarMensaje("No se encontraron registros que cumplan con las condiciones de busqueda.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                    IsBusy = False
                    'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                    logRefrescarconsultaCambioTab = True
                ElseIf lo.UserState = "FILTRAR" Then
                    If _ListaOrdenOYDPLUS.Count = 0 And Not String.IsNullOrEmpty(FiltroVM) Then
                        mostrarMensaje("No se encontraron registros que cumplan con las condiciones de indicadas.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                    IsBusy = False
                    'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                    logRefrescarconsultaCambioTab = True
                ElseIf lo.UserState = "TERMINOGUARDAREDICION" Then
                    If _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).Count > 0 Then
                        OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First
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
                            If _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).Count > 0 Then
                                OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First
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
                    If _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).Count > 0 Then
                        OrdenOYDPLUSSelected = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First
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
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes", _
                                                 Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes", _
                                                 Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, ex)
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

    Private Sub TerminoConsultarCombosSistemaExterno(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.ItemCombosSistemaExterno))
        Try
            If lo.HasError = False Then
                ListaCombosSistemaExterno = lo.Entities.ToList

                'Consulta el tipo de negocio completos.
                CargarTipoNegocioReceptor("INICIO", String.Empty, _Modulo)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los combos del sistema externo.", Me.ToString(), "TerminoConsultarCombosSistemaExterno", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los combos del sistema externo.", Me.ToString(), "TerminoConsultarCombosSistemaExterno", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
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
                        IsBusy = False
                    End If
                End If
            Else
                If logNuevoRegistro Or logEditarRegistro Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                End If
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub PosicionarControlValidaciones(ByVal plogOrdenOriginal As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesDivisas.OrdenDivisas, ByVal pobjViewOrdenOYDPLUS As OrdenesPLUSDivisasView)
        Try
            'Se busca el control para llevarle el foco al control que se requiere
            Dim strMensaje As String = strMensajeValidacion.ToLower

            If strMensaje.Contains("- receptor") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboReceptores")
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
            If strMensaje.Contains("- cliente") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "txtCliente")
                Exit Sub
            End If
            If strMensaje.Contains("- ordenante") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "txtOrdenante")
                Exit Sub
            End If
            If strMensaje.Contains("- concepto giro") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboConceptoGiro")
                Exit Sub
            End If
            If strMensaje.Contains("- moneda") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboMoneda")
                Exit Sub
            End If
            If strMensaje.Contains("- monto") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "txtMonto")
                Exit Sub
            End If
            If strMensaje.Contains("- tasa de cesion mesa") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "cboTasaDeCesion")
                Exit Sub
            End If
            If strMensaje.Contains("- tasa cliente") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "txtTasaCliente")
                Exit Sub
            End If
            If strMensaje.Contains("- comision via spread") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "txtComisionVIASpread")
                Exit Sub
            End If
            If strMensaje.Contains("- comision via papeleta") Then
                BuscarControlValidacion(pobjViewOrdenOYDPLUS, "txtComisionVIAPapeleta")
                Exit Sub
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al terminar de mostrar el mensaje de validación.", Me.ToString, "TerminoValidacionMensajeOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoValidarIngresoOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDivisas.tblRespuestaValidaciones))
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
                    Dim intIDOrdenAlmacenada As Integer = 0

                    For Each li In ListaResultadoValidacion
                        If li.Exitoso Then
                            logExitoso = True
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logRequiereAprobacion = False
                            strMensajeExitoso = String.Format("{0}{1} {2}", strMensajeExitoso, vbCrLf, li.Mensaje)
                            intIDOrdenAlmacenada = li.IDOrdenIdentity
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

                    If logExitoso And _
                        logContinuaMostrandoMensaje = False And _
                        logRequiereConfirmacion = False And _
                        logRequiereJustificacion = False And _
                        logRequiereAprobacion = False And _
                        logError = False Then

                        strMensajeExitoso = strMensajeExitoso.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("--", String.Format("{0}", vbCrLf))

                        intIDOrdenTimer = intIDOrdenAlmacenada

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

            'Esto se realiza para habilitar los botones de navegación llamando el metodo TERMINOSUBMITCHANGED
            If dcProxy1.OrdenDivisas.Contains(_OrdenOYDPLUSSelected) Then
                dcProxy1.OrdenDivisas.Add(_OrdenOYDPLUSSelected)
            End If

            'Descripción: Permite establecer el modulo de acuerdo al tipo de negocio - Por: Giovanny Velez Bolivar - 19/05/2014
            Program.VerificarCambiosProxyServidor(dcProxy1)
            dcProxy1.SubmitChanges(AddressOf TerminoSubmitChanges, pstrMensaje)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar de nuevo los parametros.", Me.ToString(), "RecargarOrdenDespuesGuardado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    ''' <summary>
    ''' Se ejecuta cuando finaliza la anulación de la orden en el servidor
    ''' </summary>
    Private Sub TerminoAnularOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDivisas.tblRespuestaValidaciones))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se anuló la orden porque se presentó un problema durante el proceso.", Me.ToString(), "TerminoAnularOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                'lo.MarkErrorAsHandled()
            Else
                If lo.Entities.Count > 0 Then
                    Dim strMensaje As String = String.Empty

                    If lo.Entities.Where(Function(i) i.DetieneIngreso).Count > 0 Then
                        strMensaje = lo.Entities.Where(Function(i) i.DetieneIngreso).First.Mensaje
                        mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        strMensaje = lo.Entities.First.Mensaje
                        mostrarMensaje(strMensaje, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        logRefrescarconsultaCambioTab = False
                        VistaSeleccionada = VISTA_APROBADAS
                        FiltrarRegistrosOYDPLUS("P", String.Empty, "TERMINOANULAR", "TERMINOANULAR")
                        logRefrescarconsultaCambioTab = True
                    End If
                Else
                    mostrarMensaje("La orden fue anulada correctamente.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    RecargarPantallaOrdenes()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la anulación de la orden.", Me.ToString(), "TerminoAnularOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
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
                                If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                                    dcProxy.tblRespuestaValidaciones.Clear()
                                End If

                                dcProxy.Load(dcProxy.OYDPLUSDivisas_AnularOrdenQuery(_OrdenOYDPLUSSelected.ID, objResultado.Observaciones, Program.Usuario, Program.HashConexion), AddressOf TerminoAnularOrden, "ANULAR")
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

    Private Sub TerminoConsultarPlantillas(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDivisas.OrdenDivisas))
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

    Private Sub TerminoEliminarPlantillas(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDivisas.tblRespuestaValidaciones))
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

    Private Sub TerminoTraerFechaCumplimiento(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDivisas.tblFechaCumplimiento))
        Try
            If Not lo.HasError Then
                If Not IsNothing(lo.Entities) Then
                    If lo.Entities.Count > 0 Then
                        _OrdenOYDPLUSSelected.FechaCumplimiento = lo.Entities.First.FechaCumplimiento
                    End If
                End If

                If lo.UserState = "GUARDAR" Then
                    ContinuarGuardadoOrden()
                Else
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la fecha de cumplimiento.", Me.ToString(), "TerminoTraerFechaCumplimiento", Application.Current.ToString(), Program.Maquina, lo.Error)
                ''lo.MarkErrorAsHandled()   '????
                IsBusy = False
                'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
                logRefrescarconsultaCambioTab = True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la fecha de cumplimiento.", Me.ToString(), "TerminoTraerFechaCumplimiento", Application.Current.ToString(), Program.Maquina, lo.Error)
            'Esta variable indica si se debe de refrescar la pantalla cuando se cambie de tab
            logRefrescarconsultaCambioTab = True
        End Try

    End Sub

    Private Sub TerminoTraerDetallePreOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDivisas.OrdenDivisasDetalle))
        Try
            If Not lo.HasError Then
                ListaDetallePreorden = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de los detalles de la PreOrden.", Me.ToString(), "TerminoTraerFechaCumplimiento", Application.Current.ToString(), Program.Maquina, lo.Error)
                ''lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de los detalles de la PreOrden.", Me.ToString(), "TerminoTraerFechaCumplimiento", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try

    End Sub

#End Region

#Region "Funciones Generales"


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

#Region "Eventos"

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Desarrollado por Juan David Correa
    ''' Fecha 14 de Agosto del 2012
    ''' </summary>
    Private Async Sub _OrdenOYDPLUSSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenOYDPLUSSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "receptor"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Receptor) Then
                            LimpiarControlesOYDPLUS(OPCION_RECEPTOR)
                            CargarTipoNegocioReceptor(OPCION_RECEPTOR, _OrdenOYDPLUSSelected.Receptor, _Modulo, OPCION_RECEPTOR)
                        Else
                            ConfiguracionReceptor = Nothing
                            LimpiarControlesOYDPLUS(OPCION_RECEPTOR)
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
                        LimpiarControlesOYDPLUS(OPCION_TIPONEGOCIO)
                        If logEditarRegistro Or logNuevoRegistro Then
                            ValidarHabilitarNegocio()
                            If logRealizarConsultaPropiedades Then
                                ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_TIPONEGOCIO)
                            Else
                                IsBusy = False
                            End If
                        End If

                        HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                    End If
                Case "tipoproducto"
                    If Not IsNothing(_OrdenOYDPLUSSelected.TipoProducto) Then
                        If logNuevoRegistro Or logEditarRegistro Then
                            ValidarHabilitarNegocio()
                        End If
                    End If
                Case "idcomitente"
                    If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.IDComitente) Then
                        HabilitarConsultaControles(_OrdenOYDPLUSSelected)
                    End If
                Case "fechaorden"
                    If logNuevoRegistro Or logEditarRegistro Then
                        ConsultarFechaCumplimientoPreOrden()
                    End If
                Case "cumplimiento"
                    If logNuevoRegistro Or logEditarRegistro Then
                        ConsultarFechaCumplimientoPreOrden()
                    End If
                Case "moneda"
                    If logNuevoRegistro Or logEditarRegistro Then
                        If logCalcularValores Then
                            If _OrdenOYDPLUSSelected.Moneda = STR_MONEDADOLAR Then
                                HabilitarCamposDolar = True
                                HabilitarCamposNoDolar = False
                                MostrarCamposConversionDolar = False
                            Else
                                HabilitarCamposDolar = False
                                HabilitarCamposNoDolar = True
                                MostrarCamposConversionDolar = True
                            End If

                            logCalcularValores = False
                            _OrdenOYDPLUSSelected.CantidadUSD = 0
                            _OrdenOYDPLUSSelected.Monto = 0
                            _OrdenOYDPLUSSelected.TasaDolar = 0
                            logCalcularValores = True

                            If Not IsNothing(_ListaDetallePreorden) Then
                                For Each liDetalle In _ListaDetallePreorden
                                    Await CalcularValorOrden(True, _OrdenOYDPLUSSelected, liDetalle)
                                Next
                            End If

                            Await CalcularValorOrden(False, _OrdenOYDPLUSSelected, Nothing)
                        End If
                    End If

            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenOYDPLUSSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    Private Sub _DetalleOrdenOYDPLUSSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetallePreordenSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()

            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenOYDPLUSSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

#End Region

#Region "Notificaciones"

    Private Const TOPICOMENSAJE_RESPUESTADIVISAS = "OYDPLUS_DIVISAS_RESPUESTA"
    Private Const TOPICOMENSAJE_AUTORIZACIONES = "OYDPLUS_AUTORIZACIONES"

    Public Overrides Sub LlegoNotificacion(pobjInfoNotificacion As A2.Notificaciones.Cliente.clsNotificacion)
        Try
            If Not IsNothing(pobjInfoNotificacion) Then
                If pobjInfoNotificacion.strTopicos = TOPICOMENSAJE_RESPUESTADIVISAS Or pobjInfoNotificacion.strTopicos = TOPICOMENSAJE_AUTORIZACIONES Then
                    Dim objNotificacion As New A2OYDPLUSUtilidades.clsNotificacionesOYDPLUS
                    Dim logCambiarInformacion As Boolean = False

                    objNotificacion = A2OYDPLUSUtilidades.clsNotificacionesOYDPLUS.Deserialize(pobjInfoNotificacion.strInfoMensaje)
                    objNotificacionGrabacion = Nothing

                    If Not IsNothing(objNotificacion) Then
                        If Editando = False Then
                            logCambiarInformacion = True
                        ElseIf Not IsNothing(_OrdenOYDPLUSSelected) Then
                            If Editando And _OrdenOYDPLUSSelected.ID = objNotificacion.IDRegistro Then
                                logCambiarInformacion = True
                            End If
                        End If
                    End If

                    If logCambiarInformacion Then

                        If _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).Count > 0 Then
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NroOrden = 0
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.EstadoOrden = String.Empty
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NombreEstadoOrden = String.Empty
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.Notificacion = String.Empty
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NotificacionDescripcion = String.Empty

                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NroOrden = objNotificacion.NroRegistro
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.EstadoOrden = objNotificacion.Estado
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NombreEstadoOrden = objNotificacion.DescripcionEstado
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.Notificacion = objNotificacion.EstadoMostrar
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NotificacionDescripcion = objNotificacion.ListaDescripciones
                        Else
                            objNotificacionGrabacion = objNotificacion
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el mensaje de la notificación.", _
                                Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Private Sub TerminoPreguntarConfirmacionSeteador(ByVal sender As Object, ByVal e As EventArgs)
    '    Try
    '        Dim objResultado As A2Utilidades.wppMensajePregunta
    '        objResultado = CType(sender, A2Utilidades.wppMensajePregunta)

    '        If objResultado.DialogResult Then
    '            If logEditarRegistro = False And logNuevoRegistro = False Then
    '                ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)

    '                If Not IsNothing(ListaOrdenOYDPLUS) Then
    '                    If ListaOrdenOYDPLUS.Where(Function(i) i.ID = NroOrdenEditar).Count > 0 Then
    '                        OrdenOYDPLUSSelected = ListaOrdenOYDPLUS.Where(Function(i) i.ID = NroOrdenEditar).FirstOrDefault
    '                        EditarRegistro()
    '                    End If
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '        IsBusy = False
    '        CantidadConfirmaciones = 0
    '    End Try
    'End Sub

    Public Sub EnviarMensajeCliente(ByVal pNotificacion As MensajeNotificacion.AccionEjecutada, ByVal pstrTopico As String, ByVal pMensajeNotificacion As String, ByVal pConsecutivo As Integer, ByVal pIDConexion As String)
        Try
            Dim objNC As clsNotificacionCliente = New clsNotificacionCliente

            objNC.objInfoNotificacion = New clsNotificacion With {.strTopicos = pstrTopico,
                                                                  .strInfoMensaje = pNotificacion & "|" & Program.Usuario,
                                                                  .strMensajeConsola = pMensajeNotificacion,
                                                                  .dtmFechaEnvio = dtmFechaServidor,
                                                                  .intConsecutivo = pConsecutivo,
                                                                  .strIdConexion = pIDConexion}
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al enviar el mensaje de notificación.", _
                                Me.ToString(), "EnviarMensajeCliente", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaOrdenDerivados
    Implements INotifyPropertyChanged

    Private _NroOrden As Integer
    <Display(Name:="Nro Orden")> _
    Public Property NroOrden() As Integer
        Get
            Return _NroOrden
        End Get
        Set(ByVal value As Integer)
            _NroOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroOrden"))
        End Set
    End Property

    Private _EstadoOrden As String
    <Display(Name:="Estado orden")> _
    Public Property EstadoOrden() As String
        Get
            Return _EstadoOrden
        End Get
        Set(ByVal value As String)
            _EstadoOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EstadoOrden"))
        End Set
    End Property

    Private _TipoOperacion As String
    <Display(Name:="Tipo Operación")> _
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
    <Display(Name:="Tipo Producto")> _
    Public Property TipoProducto() As String
        Get
            Return _TipoProducto
        End Get
        Set(ByVal value As String)
            _TipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoProducto"))
        End Set
    End Property

    Private _IDComitente As String
    <Display(Name:="Comitente")> _
    Public Property IDComitente() As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class