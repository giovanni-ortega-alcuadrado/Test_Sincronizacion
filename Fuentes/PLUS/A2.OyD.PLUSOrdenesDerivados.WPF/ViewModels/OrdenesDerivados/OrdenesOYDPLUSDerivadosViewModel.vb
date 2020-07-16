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

Public Class OrdenesOYDPLUSDerivadosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesDerivadosDomainContext()
                dcProxy1 = New OYDPLUSOrdenesDerivadosDomainContext()
                dcProxyConsulta = New OYDPLUSOrdenesDerivadosDomainContext()
                dcProxyPlantilla = New OYDPLUSOrdenesDerivadosDomainContext()

                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
                mdcProxyUtilidad03 = New OYDPLUSUtilidadesDomainContext()

            Else
                dcProxy = New OYDPLUSOrdenesDerivadosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OYDPLUSOrdenesDerivadosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyConsulta = New OYDPLUSOrdenesDerivadosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyPlantilla = New OYDPLUSOrdenesDerivadosDomainContext(New System.Uri(Program.RutaServicioNegocio))

                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad03 = New OYDPLUSUtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesDerivadosDomainContext.IOYDPLUSOrdenesDerivadosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxy1.DomainClient, WebDomainClient(Of OYDPLUSOrdenesDerivadosDomainContext.IOYDPLUSOrdenesDerivadosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxyConsulta.DomainClient, WebDomainClient(Of OYDPLUSOrdenesDerivadosDomainContext.IOYDPLUSOrdenesDerivadosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxyPlantilla.DomainClient, WebDomainClient(Of OYDPLUSOrdenesDerivadosDomainContext.IOYDPLUSOrdenesDerivadosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad01.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad02.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad03.DomainClient, WebDomainClient(Of OYDPLUSUtilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

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

                viewFormaOrdenes = New FormaOrdenesView(Me)

                CargarCombosSistemaExternoOYDPLUS("INICIO")
            End If

        Catch ex As Exception
            IsBusy = False
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

    Private dcProxy As OYDPLUSOrdenesDerivadosDomainContext
    Private dcProxy1 As OYDPLUSOrdenesDerivadosDomainContext
    Private dcProxyConsulta As OYDPLUSOrdenesDerivadosDomainContext
    Private dcProxyPlantilla As OYDPLUSOrdenesDerivadosDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Private mdcProxyUtilidad03 As OYDPLUSUtilidadesDomainContext

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
    Dim logCambiarDetallesOrden As Boolean = True

    Dim strNombrePlantilla As String = String.Empty
    Dim viewNombrePlantilla As NombrePlantillaOYDPLUSView
    Private STR_URLMOTORCALCULOS As String = ""

    Private objNotificacionGrabacion As A2OYDPLUSUtilidades.clsNotificacionesOYDPLUS

    Dim viewFormaOrdenes As FormaOrdenesView = Nothing
    Dim logCargoForma As Boolean = False
    Dim dtmFechaServidor As DateTime

#End Region

#Region "Constantes"
    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17

    Private Const VISTA_APROBADAS As String = "Aprobadas"
    Private Const VISTA_PENDIENTESAPROBAR As String = "Pendientes aprobar"

    Private Const OPCION_NUEVO As String = "NUEVO"
    Private Const OPCION_EDITAR As String = "EDITAR"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    Private Const OPCION_TIPONEGOCIO As String = "TIPONEGOCIO"
    Private Const OPCION_RECEPTOR As String = "RECEPTOR"
    Private Const OPCION_SUBCUENTA As String = "SUBCUENTA"
    Private Const OPCION_TIPOOPCION As String = "TIPOOPCION"
    Private Const OPCION_INSTRUMENTO As String = "INSTRUMENTO"
    Private Const OPCION_VENCIMIENTOINICIAL As String = "VENCIMIENTOINICIAL"
    Private Const OPCION_VENCIMIENTOFINAL As String = "VENCIMIENTOFINAL"
    Private Const OPCION_CONTRAPARTE As String = "CONTRAPARTE"
    Private Const OPCION_MEDIOVERIFICABLE As String = "MEDIOVERIFICABLE"
    Private Const OPCION_TIPOEJECUCION As String = "TIPOEJECUCION"
    Private Const OPCION_TIPOPRECIO As String = "TIPOPRECIO"
    Private Const OPCION_TIPOFINALIDAD As String = "TIPOFINALIDAD"
    Private Const OPCION_PLANTILLA As String = "PLANTILLA"
    Private Const OPCION_CREARORDENPLANTILLA As String = "CREARORDENPLANTILLA"
    Private Const OPCION_COMBOSRECEPTOR As String = "COMBOSRECEPTOR"

    Private Const SISTEMAS_ORIGEN As String = "OYD"
    Private Const SISTEMA_DESTINO As String = "DERIVADOS"
    Private Const SISTEMA_OPCION_COMBOS As String = "COMBOS"
    Private Const SISTEMA_OPCION_COMBOSRECEPTOR As String = "COMBOSRECEPTOR"
    Private Const SISTEMA_OPCION_ELIMINAR As String = "ELIMINAR"
    Private Const SISTEMA_OPCION_ACTUALIZAR As String = "ACTUALIZAR"

    Private Const TIPONEGOCIO_DERIVADOS As String = "DE"
    Private Const CANTIDADMINIMA As String = "CM"


    Private Enum TIPOMENSAJEUSUARIO
        CONFIRMACION
        JUSTIFICACION
        APROBACION
        TODOS
    End Enum

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

    Private _ListaOrdenOYDPLUS As List(Of OyDPLUSOrdenesDerivados.OrdenDerivados)
    Public Property ListaOrdenOYDPLUS() As List(Of OyDPLUSOrdenesDerivados.OrdenDerivados)
        Get
            Return _ListaOrdenOYDPLUS
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDerivados.OrdenDerivados))
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

    Private _OrdenDataForm As OyDPLUSOrdenesDerivados.OrdenDerivados = New OyDPLUSOrdenesDerivados.OrdenDerivados
    Public Property OrdenDataForm() As OyDPLUSOrdenesDerivados.OrdenDerivados
        Get
            Return _OrdenDataForm
        End Get
        Set(ByVal value As OyDPLUSOrdenesDerivados.OrdenDerivados)
            _OrdenDataForm = value
            MyBase.CambioItem("OrdenDataForm")
        End Set
    End Property

    Private WithEvents _OrdenOYDPLUSSelected As OyDPLUSOrdenesDerivados.OrdenDerivados
    Public Property OrdenOYDPLUSSelected() As OyDPLUSOrdenesDerivados.OrdenDerivados
        Get
            Return _OrdenOYDPLUSSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesDerivados.OrdenDerivados)
            _OrdenOYDPLUSSelected = value
            MyBase.CambioItem("OrdenOYDPLUSSelected")
            BuscarControlValidacion(ViewOrdenesOYDPLUS, "tabItemValoresComisiones")
            Try
                If Not IsNothing(_OrdenOYDPLUSSelected) Then
                    If _OrdenOYDPLUSSelected.TipoComision = 1 Then
                        MostrarComision = True
                    Else
                        MostrarComision = False
                    End If

                    If logCambiarDetallesOrden Then
                        CargarReceptoresOrden(_OrdenOYDPLUSSelected)
                    End If

                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al realizar las consultas de la orden.", _
                                Me.ToString(), "OrdenOYDPLUSSelected", Application.Current.ToString(), Program.Maquina, ex)
                IsBusy = False
            End Try
        End Set
    End Property

    Private _OrdenAnteriorOYDPLUS As OyDPLUSOrdenesDerivados.OrdenDerivados
    Public Property OrdenAnteriorOYDPLUS() As OyDPLUSOrdenesDerivados.OrdenDerivados
        Get
            Return _OrdenAnteriorOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesDerivados.OrdenDerivados)
            _OrdenAnteriorOYDPLUS = value
        End Set
    End Property

    Private _OrdenDuplicarOYDPLUS As OyDPLUSOrdenesDerivados.OrdenDerivados
    Public Property OrdenDuplicarOYDPLUS() As OyDPLUSOrdenesDerivados.OrdenDerivados
        Get
            Return _OrdenDuplicarOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesDerivados.OrdenDerivados)
            _OrdenDuplicarOYDPLUS = value
        End Set
    End Property

    Private _OrdenPlantillaOYDPLUS As OyDPLUSOrdenesDerivados.OrdenDerivados
    Public Property OrdenPlantillaOYDPLUS() As OyDPLUSOrdenesDerivados.OrdenDerivados
        Get
            Return _OrdenPlantillaOYDPLUS
        End Get
        Set(ByVal value As OyDPLUSOrdenesDerivados.OrdenDerivados)
            _OrdenPlantillaOYDPLUS = value
        End Set
    End Property

    Private _ViewOrdenesOYDPLUS As OrdenesPLUSDerivadosView
    Public Property ViewOrdenesOYDPLUS() As OrdenesPLUSDerivadosView
        Get
            Return _ViewOrdenesOYDPLUS
        End Get
        Set(ByVal value As OrdenesPLUSDerivadosView)
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

#End Region

#Region "Propiedades para cargar Información de los Combos"

    Private _ListaCombosSistemaExternoCompletos As List(Of OYDPLUSUtilidades.ItemCombosSistemaExterno)
    Public Property ListaCombosSistemaExternoCompletos() As List(Of OYDPLUSUtilidades.ItemCombosSistemaExterno)
        Get
            Return _ListaCombosSistemaExternoCompletos
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.ItemCombosSistemaExterno))
            _ListaCombosSistemaExternoCompletos = value
            MyBase.CambioItem("ListaCombosSistemaExternoCompletos")
        End Set
    End Property

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

    Private _ListaReceptoresUsuarioDerivados As List(Of OyDPLUSOrdenesDerivados.ReceptoresBusqueda)
    Public Property ListaReceptoresUsuarioDerivados() As List(Of OyDPLUSOrdenesDerivados.ReceptoresBusqueda)
        Get
            Return _ListaReceptoresUsuarioDerivados
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDerivados.ReceptoresBusqueda))
            _ListaReceptoresUsuarioDerivados = value
            MyBase.CambioItem("ListaReceptoresUsuarioDerivados")
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

    Private _ListaComboCuentasCCCR As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboCuentasCCCR() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboCuentasCCCR
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboCuentasCCCR = value
            MyBase.CambioItem("ListaComboCuentasCCCR")
        End Set
    End Property

    Private _ListaComboSubCuentasCCCR As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboSubCuentasCCCR() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboSubCuentasCCCR
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboSubCuentasCCCR = value
            MyBase.CambioItem("ListaComboSubCuentasCCCR")
        End Set
    End Property

    Private _ListaComboTipoOpcion As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboTipoOpcion() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboTipoOpcion
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboTipoOpcion = value
            MyBase.CambioItem("ListaComboTipoOpcion")
        End Set
    End Property

    Private _ListaComboInstrumento As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboInstrumento() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboInstrumento
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboInstrumento = value
            MyBase.CambioItem("ListaComboInstrumento")
        End Set
    End Property

    Private _ListaComboVenimientoInicial As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboVenimientoInicial() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboVenimientoInicial
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboVenimientoInicial = value
            MyBase.CambioItem("ListaComboVenimientoInicial")
        End Set
    End Property

    Private _ListaComboVenimientoFinal As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboVenimientoFinal() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboVenimientoFinal
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboVenimientoFinal = value
            MyBase.CambioItem("ListaComboVenimientoFinal")
        End Set
    End Property

    Private _ListaComboContraparte As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboContraparte() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboContraparte
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboContraparte = value
            MyBase.CambioItem("ListaComboContraparte")
        End Set
    End Property

    Private _ListaComboCanalesActivos As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboCanalesActivos() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboCanalesActivos
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboCanalesActivos = value
            MyBase.CambioItem("ListaComboCanalesActivos")
        End Set
    End Property

    Private _ListaComboMedioVerificable As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboMedioVerificable() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboMedioVerificable
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboMedioVerificable = value
            MyBase.CambioItem("ListaComboMedioVerificable")
        End Set
    End Property

    Private _ListaComboTipoEjecucion As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboTipoEjecucion() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboTipoEjecucion
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboTipoEjecucion = value
            MyBase.CambioItem("ListaComboTipoEjecucion")
        End Set
    End Property

    Private _ListaComboInstrumentoSeguimiento As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboInstrumentoSeguimiento() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboInstrumentoSeguimiento
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboInstrumentoSeguimiento = value
            MyBase.CambioItem("ListaComboInstrumentoSeguimiento")
        End Set
    End Property

    Private _ListaComboTipoFinalidad As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboTipoFinalidad() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboTipoFinalidad
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboTipoFinalidad = value
            MyBase.CambioItem("ListaComboTipoFinalidad")
        End Set
    End Property

    Private _ListaComboUbicacionPosicion As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboUbicacionPosicion() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboUbicacionPosicion
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboUbicacionPosicion = value
            MyBase.CambioItem("ListaComboUbicacionPosicion")
        End Set
    End Property

    Private _ListaComboFirmasGiveOut As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboFirmasGiveOut() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboFirmasGiveOut
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboFirmasGiveOut = value
            MyBase.CambioItem("ListaComboFirmasGiveOut")
        End Set
    End Property

    Private _ListaComboCamposEditables As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaComboCamposEditables() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaComboCamposEditables
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaComboCamposEditables = value
            MyBase.CambioItem("ListaComboCamposEditables")
        End Set
    End Property

    Private _ListaComboCamposPermitidosEdicion As List(Of OyDPLUSOrdenesDerivados.CamposEditablesOrden)
    Public Property ListaComboCamposPermitidosEdicion() As List(Of OyDPLUSOrdenesDerivados.CamposEditablesOrden)
        Get
            Return _ListaComboCamposPermitidosEdicion
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDerivados.CamposEditablesOrden))
            _ListaComboCamposPermitidosEdicion = value
            MyBase.CambioItem("ListaComboCamposPermitidosEdicion")
        End Set
    End Property

#End Region

#Region "Propiedades para la Validacion de la Orden"

    Private _ListaResultadoValidacion As List(Of OyDPLUSOrdenesDerivados.tblRespuestaValidaciones)
    Public Property ListaResultadoValidacion() As List(Of OyDPLUSOrdenesDerivados.tblRespuestaValidaciones)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDerivados.tblRespuestaValidaciones))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
        End Set
    End Property

#End Region

#Region "Propiedades para el receptor de la orden"

    Private _ListaReceptoresOrdenes As List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
    Public Property ListaReceptoresOrdenes() As List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
        Get
            Return _ListaReceptoresOrdenes
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden))
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

    Private _ReceptoresOrdenSelected As OyDPLUSOrdenesDerivados.ReceptoresOrden
    Public Property ReceptoresOrdenSelected() As OyDPLUSOrdenesDerivados.ReceptoresOrden
        Get
            Return _ReceptoresOrdenSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesDerivados.ReceptoresOrden)
            If Not IsNothing(value) Then
                _ReceptoresOrdenSelected = value
                MyBase.CambioItem("ReceptoresOrdenSelected")
            End If
        End Set
    End Property

    Private _ListaReceptoresSalvar As List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
    Public Property ListaReceptoresSalvar() As List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
        Get
            Return _ListaReceptoresSalvar
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden))
            _ListaReceptoresSalvar = value
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

    Private _Modulo As String = "DERIVADOS"
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
    Public Property HabilitarDuplicar() As String
        Get
            Return _HabilitarDuplicar
        End Get
        Set(ByVal value As String)
            _HabilitarDuplicar = value
            MyBase.CambioItem("HabilitarDuplicar")
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

    Private _MostrarComision As Boolean = False
    Public Property MostrarComision() As Boolean
        Get
            Return _MostrarComision
        End Get
        Set(ByVal value As Boolean)
            _MostrarComision = value
            MyBase.CambioItem("MostrarComision")
        End Set
    End Property

    Private _HabilitarCondicionesNegociacion As Boolean = False
    Public Property HabilitarCondicionesNegociacion() As Boolean
        Get
            Return _HabilitarCondicionesNegociacion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCondicionesNegociacion = value
            MyBase.CambioItem("HabilitarCondicionesNegociacion")
        End Set
    End Property

    Private _HabilitarCantidadMinima As Boolean = False
    Public Property HabilitarCantidadMinima() As Boolean
        Get
            Return _HabilitarCantidadMinima
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCantidadMinima = value
            MyBase.CambioItem("HabilitarCantidadMinima")
        End Set
    End Property

    Private _HabilitarPrecioSpot As Boolean
    Public Property HabilitarPrecioSpot() As Boolean
        Get
            Return _HabilitarPrecioSpot
        End Get
        Set(ByVal value As Boolean)
            _HabilitarPrecioSpot = value
            MyBase.CambioItem("HabilitarPrecioSpot")
        End Set
    End Property

    Private _HabilitarGiveOut As Boolean = False
    Public Property HabilitarGiveOut() As Boolean
        Get
            Return _HabilitarGiveOut
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGiveOut = value
            MyBase.CambioItem("HabilitarGiveOut")
        End Set
    End Property

    Private _HabilitarDatosAdicionalesFinalidad As Boolean = False
    Public Property HabilitarDatosAdicionalesFinalidad() As Boolean
        Get
            Return _HabilitarDatosAdicionalesFinalidad
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDatosAdicionalesFinalidad = value
            MyBase.CambioItem("HabilitarDatosAdicionalesFinalidad")
        End Set
    End Property

    Private _HabilitarDatosAdicionalesTipoFinalidad As Boolean = False
    Public Property HabilitarDatosAdicionalesTipoFinalidad() As Boolean
        Get
            Return _HabilitarDatosAdicionalesTipoFinalidad
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDatosAdicionalesTipoFinalidad = value
            MyBase.CambioItem("HabilitarDatosAdicionalesTipoFinalidad")
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

    Private _ListaPlantillas As List(Of OyDPLUSOrdenesDerivados.OrdenDerivados)
    Public Property ListaPlantillas() As List(Of OyDPLUSOrdenesDerivados.OrdenDerivados)
        Get
            Return _ListaPlantillas
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDerivados.OrdenDerivados))
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

    Private _PlantillaSeleccionada As OyDPLUSOrdenesDerivados.OrdenDerivados
    Public Property PlantillaSeleccionada() As OyDPLUSOrdenesDerivados.OrdenDerivados
        Get
            Return _PlantillaSeleccionada
        End Get
        Set(ByVal value As OyDPLUSOrdenesDerivados.OrdenDerivados)
            _PlantillaSeleccionada = value
            MyBase.CambioItem("PlantillaSeleccionada")
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

            ValidarPermisosUsuarioDerivados(OPCION_NUEVO)

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
            Dim objNewOrdenOYDPLUS As New OyDPLUSOrdenesDerivados.OrdenDerivados
            objNewOrdenOYDPLUS.ID = 0
            objNewOrdenOYDPLUS.IDPreorden = 0
            objNewOrdenOYDPLUS.NroOrden = 0
            objNewOrdenOYDPLUS.strEstado = String.Empty
            objNewOrdenOYDPLUS.FechaGenerarOrden = String.Empty
            objNewOrdenOYDPLUS.FechaPreorden = dtmFechaServidor
            objNewOrdenOYDPLUS.FechaOrden = Nothing
            objNewOrdenOYDPLUS.Receptor = String.Empty
            objNewOrdenOYDPLUS.NombreReceptor = String.Empty
            objNewOrdenOYDPLUS.NroDocumento = String.Empty
            objNewOrdenOYDPLUS.CodigoOYD = String.Empty
            objNewOrdenOYDPLUS.IDCuenta = 0
            objNewOrdenOYDPLUS.IDSubCuenta = 0
            objNewOrdenOYDPLUS.TipoOperacion = 0
            objNewOrdenOYDPLUS.TipoInstruccion = 0
            objNewOrdenOYDPLUS.TipoRegularOSpread = 0
            objNewOrdenOYDPLUS.Instrumento = 0
            objNewOrdenOYDPLUS.VencimientoInicial = 0
            objNewOrdenOYDPLUS.VencimientoFinal = 0
            objNewOrdenOYDPLUS.Precio = 0
            objNewOrdenOYDPLUS.Cantidad = 0
            objNewOrdenOYDPLUS.Prima = 0
            objNewOrdenOYDPLUS.Comision = 0
            objNewOrdenOYDPLUS.ComisionPorPorcentaje = 0
            objNewOrdenOYDPLUS.FacturarComision = 0
            objNewOrdenOYDPLUS.Estado = 0
            objNewOrdenOYDPLUS.RegistroEnBolsa = False
            objNewOrdenOYDPLUS.TipoRegistro = 0
            objNewOrdenOYDPLUS.Contraparte = 0
            objNewOrdenOYDPLUS.Canal = 0
            objNewOrdenOYDPLUS.MedioVerificable = 0
            objNewOrdenOYDPLUS.FechaHoraToma = dtmFechaServidor
            objNewOrdenOYDPLUS.DetalleMedioVerificable = String.Empty
            objNewOrdenOYDPLUS.Naturaleza = 0
            objNewOrdenOYDPLUS.TipoEjecucion = 0
            objNewOrdenOYDPLUS.Duracion = 0
            objNewOrdenOYDPLUS.PrecioStop = 0
            objNewOrdenOYDPLUS.CantidadVisible = 0
            objNewOrdenOYDPLUS.OtroInstrumento = 0
            objNewOrdenOYDPLUS.TipoPrecio = 0
            objNewOrdenOYDPLUS.Instrucciones = String.Empty
            objNewOrdenOYDPLUS.Finalidad = 0
            objNewOrdenOYDPLUS.TipoCobertura = 0
            objNewOrdenOYDPLUS.UbicacionPosicion = 0
            objNewOrdenOYDPLUS.DescripcionPosicion = 0
            objNewOrdenOYDPLUS.MontoCubrir = 0
            objNewOrdenOYDPLUS.Moneda = 0
            objNewOrdenOYDPLUS.GiveOut = False
            objNewOrdenOYDPLUS.FirmaGiveOut = 0
            objNewOrdenOYDPLUS.ReferenciaGiveOut = String.Empty
            objNewOrdenOYDPLUS.Comentarios = String.Empty
            objNewOrdenOYDPLUS.DiasAvisoCumplimiento = 0
            objNewOrdenOYDPLUS.PrecioSpot = 0
            objNewOrdenOYDPLUS.CantidadMinima = 0
            objNewOrdenOYDPLUS.DetalleReceptores = String.Empty
            objNewOrdenOYDPLUS.Usuario = Program.Usuario
            objNewOrdenOYDPLUS.UsuarioCreacion = Program.UsuarioWindows
            objNewOrdenOYDPLUS.UsuarioWindows = Program.UsuarioWindows
            objNewOrdenOYDPLUS.Actualizacion = dtmFechaServidor

            If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "ESTADOORDEN" And i.Retorno = "PENDIENTE").Count > 0 Then
                objNewOrdenOYDPLUS.Estado = _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "ESTADOORDEN" And i.Retorno = "PENDIENTE").First.ID
            End If

            Confirmaciones = String.Empty

            'Limpia la lista de combos cuando es una nueva orden
            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                DiccionarioCombosOYDPlus.Clear()
            End If

            logCalcularValores = False

            ObtenerValoresOrdenAnterior(objNewOrdenOYDPLUS, OrdenOYDPLUSSelected)

            logCalcularValores = True

            Editando = True

            HabilitarEncabezado = True
            HabilitarNegocio = False

            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_NUEVO)

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
            If logCargoForma = False Then
                ViewOrdenesOYDPLUS.GridEdicion.Children.Add(viewFormaOrdenes)
                logCargoForma = True
            End If

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

            If Not IsNothing(dcProxyPlantilla.OrdenDerivados) Then
                dcProxyPlantilla.OrdenDerivados.Clear()
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
            Dim objNewOrdenDuplicar As New OyDPLUSOrdenesDerivados.OrdenDerivados
            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, objNewOrdenDuplicar)

            objNewOrdenDuplicar.ID = 0
            objNewOrdenDuplicar.IDPreorden = 0
            objNewOrdenDuplicar.IDPreorden = 0
            objNewOrdenDuplicar.FechaPreorden = dtmFechaServidor
            objNewOrdenDuplicar.Actualizacion = dtmFechaServidor
            objNewOrdenDuplicar.strEstado = String.Empty
            objNewOrdenDuplicar.Usuario = Program.Usuario
            objNewOrdenDuplicar.UsuarioWindows = Program.UsuarioWindows

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = True
            logNuevoRegistro = False

            ObtenerValoresOrdenAnterior(objNewOrdenDuplicar, OrdenDuplicarOYDPLUS)

            Dim objNuevaListaDistribucion As New List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
            For Each li In ListaReceptoresOrdenes
                objNuevaListaDistribucion.Add(li)
            Next
            ListaReceptoresSalvar = objNuevaListaDistribucion

            CargarReceptoresUsuarioOYDPLUS(OPCION_DUPLICAR, OPCION_DUPLICAR)

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
            Dim objNewOrdenPlantilla As New OyDPLUSOrdenesDerivados.OrdenDerivados
            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, objNewOrdenPlantilla)

            objNewOrdenPlantilla.ID = 0
            objNewOrdenPlantilla.IDPreorden = 0
            objNewOrdenPlantilla.IDPreorden = 0
            objNewOrdenPlantilla.FechaPreorden = dtmFechaServidor
            objNewOrdenPlantilla.Actualizacion = dtmFechaServidor
            objNewOrdenPlantilla.strEstado = String.Empty
            objNewOrdenPlantilla.Usuario = Program.Usuario
            objNewOrdenPlantilla.UsuarioWindows = Program.UsuarioWindows

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
            If logCargoForma = False Then
                ViewOrdenesOYDPLUS.GridEdicion.Children.Add(viewFormaOrdenes)
                logCargoForma = True
            End If

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
            Dim objNewOrdenPlantilla As New OyDPLUSOrdenesDerivados.OrdenDerivados
            ObtenerValoresOrdenAnterior(_PlantillaSeleccionada, objNewOrdenPlantilla)

            objNewOrdenPlantilla.ID = 0
            objNewOrdenPlantilla.IDPreorden = 0
            objNewOrdenPlantilla.IDPreorden = 0
            objNewOrdenPlantilla.FechaPreorden = dtmFechaServidor
            objNewOrdenPlantilla.Actualizacion = dtmFechaServidor
            objNewOrdenPlantilla.strEstado = String.Empty
            objNewOrdenPlantilla.Usuario = Program.Usuario
            objNewOrdenPlantilla.UsuarioWindows = Program.UsuarioWindows

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

            If Not IsNothing(dcProxyConsulta.OrdenDerivados) Then
                dcProxyConsulta.OrdenDerivados.Clear()
            End If

            If strEstado = "D" Then
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenPendienteQuery(strEstado, TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
                Else
                    dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenPendienteQuery(strEstado, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
                End If
            Else
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenQuery(TextoFiltroSeguro, Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
                Else
                    dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenQuery(String.Empty, Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoTraerOrdenes, "FILTRAR")
                End If
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

                If Not IsNothing(dcProxyConsulta.OrdenDerivados) Then
                    dcProxyConsulta.OrdenDerivados.Clear()
                End If

                If pstrEstado = "D" Then
                    dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenPendienteQuery(pstrEstado, pstrFiltro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, pstrUserState)
                Else
                    dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_FiltrarOrdenQuery(pstrFiltro, Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoTraerOrdenes, pstrUserState)
                End If
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
                If Not IsNothing(dcProxyConsulta.OrdenDerivados) Then
                    dcProxyConsulta.OrdenDerivados.Clear()
                End If

                logRefrescarconsultaCambioTab = False
                VistaSeleccionada = VISTA_APROBADAS
                logRefrescarconsultaCambioTab = True

                dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_ConsultarOrdenQuery(BusquedaOrdenOyDPlus.NroOrden, BusquedaOrdenOyDPlus.Estado, BusquedaOrdenOyDPlus.TipoOperacion, BusquedaOrdenOyDPlus.FechaInicial, BusquedaOrdenOyDPlus.FechaFinal, Program.UsuarioWindows, Program.HashConexion), _
                                     AddressOf TerminoTraerOrdenes, "BUSQUEDA")

                MyBase.ConfirmarBuscar()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConfirmarBuscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                If _OrdenOYDPLUSSelected.TipoComision = 1 Then
                    _OrdenOYDPLUSSelected.ComisionPorPorcentaje = False
                    _OrdenOYDPLUSSelected.PorcentajeComision = 0
                Else
                    _OrdenOYDPLUSSelected.ComisionPorPorcentaje = True
                    _OrdenOYDPLUSSelected.Comision = 0
                End If
                If _OrdenOYDPLUSSelected.FacturarComision = 1 Then
                    _OrdenOYDPLUSSelected.logFacturarComision = True
                Else
                    _OrdenOYDPLUSSelected.logFacturarComision = False
                End If
            End If

            ActualizarOrdenOYDPLUS(_OrdenOYDPLUSSelected)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden.", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cuando se pasan todas las validaciones, sí se requeria confirmación, justificación y aprobación
    ''' se llama este metodo y se realiza el envio de la orden a la base de datos con todos los parametros de la orden.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ActualizarOrdenOYDPLUS(ByVal objOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados)
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
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de validar el mensaje para mostrar al usuario.", Me.ToString(), "ValidarMensajesMostrarUsuario", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub GuardarOrdenOYDPLUS(ByVal objOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados)
        Try
            Dim dblValorComision As Double = 0

            If _OrdenOYDPLUSSelected.TipoComision = 1 Then
                dblValorComision = _OrdenOYDPLUSSelected.Comision
            Else
                dblValorComision = _OrdenOYDPLUSSelected.PorcentajeComision
            End If

            IsBusy = True

            If logPlantillaRegistro = False Then
                strNombrePlantilla = String.Empty
            End If

            If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                dcProxy.tblRespuestaValidaciones.Clear()
            End If

            _OrdenOYDPLUSSelected.DetalleReceptores = String.Empty

            If Not IsNothing(_ListaReceptoresOrdenes) Then
                For Each li In _ListaReceptoresOrdenes
                    If String.IsNullOrEmpty(_OrdenOYDPLUSSelected.DetalleReceptores) Then
                        _OrdenOYDPLUSSelected.DetalleReceptores = String.Format("{0},{1},{2}", li.IDComercial, li.IDReceptor, li.Porcentaje)
                    Else
                        _OrdenOYDPLUSSelected.DetalleReceptores = String.Format("{0}|{1},{2},{3}", _OrdenOYDPLUSSelected.DetalleReceptores, li.IDComercial, li.IDReceptor, li.Porcentaje)
                    End If
                Next
            End If

            dcProxy.Load(dcProxy.OYDPLUS_ValidarIngresoOrdenQuery(objOrdenSelected.ID, objOrdenSelected.IDOrden, objOrdenSelected.IDPreorden, objOrdenSelected.NroOrden,
                                                                  objOrdenSelected.FechaGenerarOrden, objOrdenSelected.FechaPreorden, objOrdenSelected.FechaOrden, objOrdenSelected.FechaVigenciaOrden,
                                                                  objOrdenSelected.Receptor, objOrdenSelected.NroDocumento, objOrdenSelected.CodigoOYD, objOrdenSelected.IDCuenta, objOrdenSelected.IDSubCuenta,
                                                                  objOrdenSelected.TipoOperacion, objOrdenSelected.TipoInstruccion, objOrdenSelected.TipoRegularOSpread, objOrdenSelected.Instrumento,
                                                                  objOrdenSelected.VencimientoInicial, objOrdenSelected.VencimientoFinal, objOrdenSelected.Precio, objOrdenSelected.Cantidad, objOrdenSelected.Prima, objOrdenSelected.TipoComision,
                                                                  dblValorComision, objOrdenSelected.FacturarComision, objOrdenSelected.Estado, objOrdenSelected.SubEstado, objOrdenSelected.RegistroEnBolsa, objOrdenSelected.TipoRegistro,
                                                                  objOrdenSelected.Contraparte, objOrdenSelected.Canal, objOrdenSelected.MedioVerificable, objOrdenSelected.FechaHoraToma, objOrdenSelected.DetalleMedioVerificable,
                                                                  objOrdenSelected.Naturaleza, objOrdenSelected.TipoEjecucion, objOrdenSelected.Duracion, objOrdenSelected.PrecioStop, objOrdenSelected.CantidadVisible, objOrdenSelected.OtroInstrumento,
                                                                  objOrdenSelected.TipoPrecio, objOrdenSelected.Instrumento, objOrdenSelected.Finalidad, objOrdenSelected.TipoCobertura, objOrdenSelected.UbicacionPosicion,
                                                                  objOrdenSelected.DescripcionPosicion, objOrdenSelected.MontoCubrir, objOrdenSelected.Moneda, objOrdenSelected.GiveOut, objOrdenSelected.FirmaGiveOut,
                                                                  objOrdenSelected.ReferenciaGiveOut, objOrdenSelected.Comentarios, objOrdenSelected.DiasAvisoCumplimiento, objOrdenSelected.PrecioSpot, objOrdenSelected.CantidadMinima,
                                                                  objOrdenSelected.DetalleReceptores, Confirmaciones, ConfirmacionesUsuario, Justificaciones, JustificacionesUsuario, Aprobaciones, AprobacionesUsuario,
                                                                  Program.Maquina, Program.Usuario, Program.UsuarioWindows, logPlantillaRegistro, strNombrePlantilla, Modulo, objOrdenSelected.TipoNegocio, Program.HashConexion), AddressOf TerminoValidarIngresoOrden, String.Empty)
        Catch ex As Exception
            IsBusy = False
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
            IsBusy = False
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

            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
            ValidarHabilitarNegocio(_OrdenOYDPLUSSelected)

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

            CargarReceptoresUsuarioOYDPLUS(OPCION_EDITAR, OPCION_EDITAR)

            BuscarControlValidacion(ViewOrdenesOYDPLUS, "tabItemValoresComisiones")
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
            If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.strEstado) Then
                If _OrdenOYDPLUSSelected.strEstado <> "R" Then
                    strMsg = "Solamente las pre-ordenes en estado Rechazado se pueden "
                    If pstrAccion = OPCION_EDITAR Then
                        strMsg = strMsg & "editar."
                    Else
                        strMsg = strMsg & "anular."
                    End If
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
                    ValidarPermisosUsuarioDerivados(pstrAccion)
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
                IsBusy = False
                mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el estado de la orden.", Me.ToString(), "validarEstadoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            If Not IsNothing(_OrdenOYDPLUSSelected) Then
                IsBusy = True
                dcProxy1.OYDPLUS_CancelarOrdenOYDPLUS(_OrdenOYDPLUSSelected.ID, "ORDENES", Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
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
            IsBusy = False
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

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOrden"
                    Dim NewReceptoresOrden As New OyDPLUSOrdenesDerivados.ReceptoresOrden

                    NewReceptoresOrden.FechaActualizacion = dtmFechaServidor
                    NewReceptoresOrden.Usuario = Program.Usuario
                    NewReceptoresOrden.Porcentaje = 0
                    NewReceptoresOrden.Nombre = ""
                    NewReceptoresOrden.IDReceptor = String.Empty

                    Dim objListaReceptorOrden As New List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
                    objListaReceptorOrden = _ListaReceptoresOrdenes

                    If IsNothing(objListaReceptorOrden) Then
                        objListaReceptorOrden = New List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
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
                            Dim objListaReceptoresOrden As New List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
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
            Dim objBusqueda As New CamposBusquedaOrdenDerivados
            objBusqueda.Estado = Nothing
            objBusqueda.NroOrden = 0
            objBusqueda.TipoOperacion = Nothing
            objBusqueda.FechaInicial = dtmFechaServidor
            objBusqueda.FechaFinal = dtmFechaServidor

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
                    If Not IsNothing(pstrOpcion) Then
                        If pstrOpcion = OPCION_DUPLICAR Then
                            ObtenerValoresCombos(False, _OrdenDuplicarOYDPLUS, pstrOpcion.ToUpper)
                        ElseIf pstrOpcion = OPCION_EDITAR Then
                            ObtenerValoresCombos(False, _OrdenAnteriorOYDPLUS, pstrOpcion.ToUpper)
                        ElseIf pstrOpcion = OPCION_PLANTILLA Or pstrOpcion = OPCION_CREARORDENPLANTILLA Then
                            ObtenerValoresCombos(False, _OrdenPlantillaOYDPLUS, pstrOpcion.ToUpper)
                        Else
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, pstrOpcion.ToUpper)
                        End If
                    Else
                        ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_RECEPTOR)
                    End If
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

            mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarCombosOtroSistemaQuery("COMBOSDERIVADOS", SISTEMAS_ORIGEN, SISTEMA_DESTINO, SISTEMA_OPCION_COMBOS, String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosSistemaExterno, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos del sistema externo.", _
                                Me.ToString(), "CargarCombosSistemaExternoOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub CargarCombosSistemaExternoReceptorOYDPLUS(ByVal pstrNroDocumento As String, ByVal pstrCodigoOYD As String, ByVal pstrReceptor As String, Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            Dim strInformacionAdicional As String = String.Format("<Usuario>{0}</Usuario><NroDocumento>{1}</NroDocumento><CodigoOYD>{2}</CodigoOYD><Receptor>{3}</Receptor>",
                                                                  Program.UsuarioWindows,
                                                                  pstrNroDocumento,
                                                                  pstrCodigoOYD,
                                                                  pstrReceptor)
            strInformacionAdicional = System.Web.HttpUtility.UrlEncode(strInformacionAdicional)

            If Not IsNothing(mdcProxyUtilidad03.ItemCombosSistemaExternos) Then
                mdcProxyUtilidad03.ItemCombosSistemaExternos.Clear()
            End If

            mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarCombosOtroSistemaQuery("COMBOSDERIVADOS", SISTEMAS_ORIGEN, SISTEMA_DESTINO, SISTEMA_OPCION_COMBOSRECEPTOR, String.Empty, strInformacionAdicional, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosSistemaExterno, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos del sistema externo.", _
                                Me.ToString(), "CargarCombosSistemaExternoReceptorOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
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
                                 Me.ToString(), "CargarTipoNegocioReceptor", Application.Current.ToString(), Program.Maquina, ex)
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
                    Me.ComitenteSeleccionadoOYDPLUS = Nothing
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al limpiar los controles.", _
                                 Me.ToString(), "LimpiarControles", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Valida los permisos del usuario para crear ordenes de derivados.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub ValidarPermisosUsuarioDerivados(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.tblRespuestaValidaciones) Then
                dcProxyConsulta.tblRespuestaValidaciones.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_ValidarPermisosUsuarioQuery(Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoValidarPermisosUsuario, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los permisos del usuario en derivados.", _
                                 Me.ToString(), "ValidarPermisosUsuarioDerivados", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Valida sí el estado de la orden es valido para editar.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub ValidarEdicionOrdenDerivados(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.tblRespuestaValidaciones) Then
                dcProxyConsulta.tblRespuestaValidaciones.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_ValidarEdicionOrdenQuery(_OrdenOYDPLUSSelected.ID, _OrdenOYDPLUSSelected.SubEstado, Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoValidarEdicionOrden, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar la edición de la orden de derivados.", _
                                 Me.ToString(), "ValidarEdicionOrdenDerivados", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Valida los campos editables de la orden.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub ValidarCamposEditablesOrden(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.CamposEditablesOrdens) Then
                dcProxyConsulta.CamposEditablesOrdens.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_ValidarCamposEditablesOrdenQuery(Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoValidarCamposEditablesOrden, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los campos editables de la orden.", _
                                 Me.ToString(), "ValidarCamposEditablesOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub


    ''' <summary>
    ''' Cargar los receptores activos del usuario logueado.
    ''' Cuando el parametro opción se encuentra vacio carga todos los receptores del usuario.
    ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub CargarReceptoresUsuarioDerivados(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.ReceptoresBusquedas) Then
                dcProxyConsulta.ReceptoresBusquedas.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.OYDPLUS_ConsultarReceptoresUsuarioDerivadosQuery(Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoConsultarReceptoresUsuarioDerivados, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar los receptores del usuario.", _
                                 Me.ToString(), "CargarReceptoresUsuario", Application.Current.ToString(), Program.Maquina, ex)
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
    ''' Cargar los receptores de la orden.
    ''' ''' Desarrollado por: Juan David Correa
    ''' </summary>
    Public Sub CargarReceptoresOrden(ByVal pobjOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados, Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(dcProxy1.ReceptoresOrdens) Then
                dcProxy1.ReceptoresOrdens.Clear()
            End If

            If pobjOrdenSelected.strEstado = "R" Or pobjOrdenSelected.strEstado = "D" Then
                dcProxy1.Load(dcProxy1.OYDPLUS_ConsultarReceptoresOrdenPendienteQuery(pobjOrdenSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptoresOrden, pstrUserState)
            Else
                dcProxy1.Load(dcProxy1.OYDPLUS_ConsultarReceptoresOrdenQuery(pobjOrdenSelected.IDPreorden, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptoresOrden, pstrUserState)
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
                    _OrdenOYDPLUSSelected.NroDocumento = pComitente.NroDocumento
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
    Public Sub ObtenerValoresDefectoOYDPLUS(ByVal pstrOpcion As String, ByVal objOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados)
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

                        ObtenerValorPorDefectorEnLista(_ListaComboCuentasCCCR, objOrdenSelected.IDCuenta)
                        ObtenerValorPorDefectorEnLista(_ListaComboCanalesActivos, objOrdenSelected.Canal)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "FECHASGENERAR", objOrdenSelected.FechaGenerarOrden, False)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "TIPOOPERACION", objOrdenSelected.TipoOperacion)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "TIPOINSTRUMENTO", objOrdenSelected.TipoInstruccion)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "TIPOCOMISION", objOrdenSelected.TipoComision)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "FACTURARCOMISION", objOrdenSelected.FacturarComision)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "TIPOREGISTRO", objOrdenSelected.TipoRegistro)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "NATURALEZA", objOrdenSelected.Naturaleza)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "DURACION", objOrdenSelected.Duracion)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "FINALIDAD", objOrdenSelected.Finalidad)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "MONEDA", objOrdenSelected.Moneda)
                        ObtenerValorPorDefectorEnDiccionario(DiccionarioCombosOYDPlus, "TIPOPRECIOINSTRUMENTO", objOrdenSelected.TipoPrecio)

                        IsBusy = False
                    Case OPCION_DUPLICAR
                        If Not IsNothing(_OrdenDuplicarOYDPLUS) Then
                            logCambiarDetallesOrden = False
                            ObtenerValoresOrdenAnterior(OrdenDuplicarOYDPLUS, OrdenOYDPLUSSelected)

                            Dim objNuevaListaDistribucion As New List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)
                            For Each li In ListaReceptoresSalvar
                                objNuevaListaDistribucion.Add(li)
                            Next

                            ListaReceptoresOrdenes = objNuevaListaDistribucion
                            logCambiarDetallesOrden = True

                            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                            ValidarHabilitarNegocio(_OrdenOYDPLUSSelected)

                        End If

                        logPlantillaRegistro = False
                        strNombrePlantilla = String.Empty

                        VerificarValoresEnCombos()
                    Case OPCION_PLANTILLA, OPCION_CREARORDENPLANTILLA
                        If Not IsNothing(_OrdenPlantillaOYDPLUS) Then
                            ObtenerValoresOrdenAnterior(OrdenPlantillaOYDPLUS, OrdenOYDPLUSSelected)

                            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                        End If

                        If pstrOpcion.ToUpper = OPCION_CREARORDENPLANTILLA Then
                            logPlantillaRegistro = False
                            strNombrePlantilla = String.Empty
                        End If

                        VerificarValoresEnCombos()
                    Case OPCION_EDITAR
                        VerificarValoresEnCombos()
                    Case OPCION_SUBCUENTA
                        ObtenerValorPorDefectorEnLista(_ListaComboSubCuentasCCCR, objOrdenSelected.IDSubCuenta)
                    Case OPCION_TIPOOPCION
                        ObtenerValorPorDefectorEnLista(_ListaComboTipoOpcion, objOrdenSelected.TipoRegularOSpread)
                    Case OPCION_INSTRUMENTO
                        ObtenerValorPorDefectorEnLista(_ListaComboInstrumento, objOrdenSelected.Instrumento)
                    Case OPCION_VENCIMIENTOINICIAL
                        ObtenerValorPorDefectorEnLista(_ListaComboVenimientoInicial, objOrdenSelected.VencimientoInicial)
                    Case OPCION_VENCIMIENTOFINAL
                        ObtenerValorPorDefectorEnLista(_ListaComboVenimientoFinal, objOrdenSelected.VencimientoFinal)
                        ObtenerValorPorDefectorEnLista(_ListaComboInstrumentoSeguimiento, objOrdenSelected.OtroInstrumento)
                    Case OPCION_CONTRAPARTE
                        ObtenerValorPorDefectorEnLista(_ListaComboContraparte, objOrdenSelected.Contraparte)
                    Case OPCION_MEDIOVERIFICABLE
                        ObtenerValorPorDefectorEnLista(_ListaComboMedioVerificable, objOrdenSelected.MedioVerificable)
                    Case OPCION_TIPOEJECUCION
                        ObtenerValorPorDefectorEnLista(_ListaComboTipoEjecucion, objOrdenSelected.TipoEjecucion)
                    Case OPCION_TIPOFINALIDAD
                        ObtenerValorPorDefectorEnLista(_ListaComboTipoFinalidad, objOrdenSelected.TipoCobertura)
                        ObtenerValorPorDefectorEnLista(_ListaComboUbicacionPosicion, objOrdenSelected.UbicacionPosicion)
                    Case OPCION_NUEVO
                        IsBusy = False
                    Case OPCION_EDITAR
                        IsBusy = False
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

            End If
        Catch ex As Exception
            IsBusy = False
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
            IsBusy = False
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
    Public Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados, Optional ByVal Opcion As String = "")
        Try
            If Not IsNothing(_ListaCombosSistemaExternoCompletos) Then

                If IsNothing(_DiccionarioCombosOYDPlus) Then
                    DiccionarioCombosOYDPlus = New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
                End If

                If ValoresCompletos Then 'Cuando ValoresCompletos = True

                    If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
                        If DiccionarioCombosOYDPlusCompleta.ContainsKey("MOTORCALCULOS_RUTASERVICIO") Then
                            If DiccionarioCombosOYDPlusCompleta("MOTORCALCULOS_RUTASERVICIO").Count > 0 Then
                                STR_URLMOTORCALCULOS = DiccionarioCombosOYDPlusCompleta("MOTORCALCULOS_RUTASERVICIO").First.Retorno
                            End If
                        End If

                        If DiccionarioCombosOYDPlusCompleta.ContainsKey("FECHAACTUAL_SERVIDOR") Then
                            If DiccionarioCombosOYDPlusCompleta("FECHAACTUAL_SERVIDOR").Count > 0 Then
                                Try
                                    dtmFechaServidor = DateTime.ParseExact(DiccionarioCombosOYDPlusCompleta("FECHAACTUAL_SERVIDOR").First.Retorno, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)
                                Catch ex As Exception
                                    dtmFechaServidor = Now
                                End Try
                            End If
                        End If
                    End If

                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    If Not IsNothing(ListaCombosSistemaExternoCompletos) Then
                        Dim objListaCategorias = _ListaCombosSistemaExternoCompletos.Select(Function(i) i.Categoria).Distinct()

                        For Each liCategoria In objListaCategorias
                            If liCategoria = "ESTADOORDEN" Or liCategoria = "ESTADOPREORDEN" Or liCategoria = "FECHASGENERAR" Or liCategoria = "TIPOOPERACION" Or liCategoria = "TIPOINSTRUMENTO" Or _
                                liCategoria = "TIPOCOMISION" Or liCategoria = "FACTURARCOMISION" Or liCategoria = "ESTADOORDEN" Or _
                                liCategoria = "TIPOREGISTRO" Or liCategoria = "NATURALEZA" Or liCategoria = "DURACION" Or liCategoria = "FINALIDAD" Or _
                                liCategoria = "MONEDA" Or liCategoria = "TIPOPRECIOINSTRUMENTO" Then
                                PrRemoverValoresDic(objDiccionario, {liCategoria})
                                objDiccionario.Add(liCategoria, ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria))
                            ElseIf liCategoria = "CUENTASCRCC" Then
                                ListaComboCuentasCCCR = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "CANALESACTIVOS" Then
                                ListaComboCanalesActivos = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "FIRMASGIVEUP" Then
                                ListaComboFirmasGiveOut = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "SUBCUENTACRCC" Then
                                ListaComboSubCuentasCCCR = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "TIPOOPCION" Then
                                ListaComboTipoOpcion = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "PRODUCTOS" Then
                                ListaComboInstrumento = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "VENCIMIENTOPRODUCTOS" Then
                                ListaComboVenimientoInicial = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "VENCIMIENTOFINALPRODUCTOS" Then
                                ListaComboVenimientoFinal = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "CONTRAPARTE" Then
                                ListaComboContraparte = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "MEDIOVERIFICABLE" Then
                                ListaComboMedioVerificable = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "TIPOEJECUCION" Then
                                ListaComboTipoEjecucion = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "INSTRUMENTOSEGUIMIENTO" Then
                                ListaComboInstrumentoSeguimiento = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            ElseIf liCategoria = "CAMPOEDITABLES" Then
                                ListaComboCamposEditables = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, liCategoria, liCategoria)
                            End If

                        Next

                        Dim objDiccionarioCamposEditables As New Dictionary(Of String, Boolean)

                        For Each li In ListaComboCamposEditables
                            If Not objDiccionarioCamposEditables.ContainsKey(li.Retorno) Then
                                objDiccionarioCamposEditables.Add(li.Retorno, False)
                            End If
                        Next

                        DiccionarioEdicionCamposOYDPLUS = objDiccionarioCamposEditables

                    End If

                    DiccionarioCombosOYDPlus = objDiccionario
                Else ' Cuando ValoresCompletos = False
                    If Not String.IsNullOrEmpty(Opcion) Then
                        Dim OpcionValoresDefecto As String = String.Empty

                        Select Case Opcion.ToUpper
                            Case OPCION_NUEVO
                                PrRemoverValoresDic(DiccionarioCombosOYDPlus, {"ESTADOORDEN", "FECHASGENERAR", "TIPOOPERACION", "TIPOINSTRUMENTO", "TIPOCOMISION",
                                                                               "FACTURARCOMISION", "TIPOREGISTRO", "NATURALEZA",
                                                                               "DURACION", "FINALIDAD", "MONEDA", "TIPOPRECIOINSTRUMENTO"})

                                OpcionValoresDefecto = OPCION_RECEPTOR
                            Case OPCION_EDITAR, OPCION_DUPLICAR
                                ListaComboMedioVerificable = Nothing
                                ListaComboCuentasCCCR = Nothing
                                ListaComboCanalesActivos = Nothing
                                ListaComboFirmasGiveOut = Nothing
                                ListaComboSubCuentasCCCR = Nothing
                                ListaComboInstrumento = Nothing
                                ListaComboVenimientoInicial = Nothing
                                ListaComboVenimientoFinal = Nothing
                                ListaComboTipoOpcion = Nothing
                                ListaComboInstrumentoSeguimiento = Nothing
                                ListaComboContraparte = Nothing
                                ListaComboTipoEjecucion = Nothing
                                ListaComboTipoFinalidad = Nothing
                                ListaComboUbicacionPosicion = Nothing

                                ListaComboCuentasCCCR = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExterno, "CUENTASCRCC", "CUENTASCRCC")
                                ListaComboCanalesActivos = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExterno, "CANALESACTIVOS", "CANALESACTIVOS")
                                ListaComboFirmasGiveOut = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExterno, "FIRMASGIVEUP", "FIRMASGIVEUP")
                                ListaComboSubCuentasCCCR = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExterno, "SUBCUENTACRCC", "SUBCUENTACRCC", pobjOrdenSelected.IDCuenta)
                                ListaComboTipoOpcion = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOOPCION", "TIPOOPCION", pobjOrdenSelected.TipoInstruccion)

                                If Not IsNothing(pobjOrdenSelected.TipoRegularOSpread) Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "PRODUCTOSESPECIALES" And i.IDDependencia1 = pobjOrdenSelected.TipoRegularOSpread).Count > 0 Then
                                        ListaComboInstrumento = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "PRODUCTOSESPECIALES", "PRODUCTOSTIPOPRODUCTO", pobjOrdenSelected.TipoRegularOSpread)
                                    Else
                                        ListaComboInstrumento = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "PRODUCTOS", "PRODUCTOSTIPOPRODUCTO", pobjOrdenSelected.TipoRegularOSpread)
                                    End If
                                End If

                                Dim objListaVencimientoInicial As New List(Of OYDPLUSUtilidades.CombosReceptor)

                                If Not IsNothing(pobjOrdenSelected.Instrumento) Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "VENCIMIENTOPRODUCTOS").Count > 0 Then
                                        For Each li In _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "VENCIMIENTOPRODUCTOS")
                                            If li.IDDependencia1 = pobjOrdenSelected.Instrumento And _
                                                String.Format("{0:yyyy-MM-dd}", dtmFechaServidor) <= li.Retorno Then
                                                objListaVencimientoInicial.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                                          .Retorno = li.Retorno,
                                                                                                                          .Categoria = li.Categoria,
                                                                                                                          .Descripcion = li.Descripcion,
                                                                                                                          .Prioridad = li.Prioridad})
                                            End If
                                        Next
                                    End If
                                End If

                                ListaComboVenimientoInicial = objListaVencimientoInicial

                                ListaComboVenimientoFinal = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "VENCIMIENTOFINALPRODUCTOS", "VENCIMIENTOFINALPRODUCTOS", pobjOrdenSelected.TipoRegularOSpread, pobjOrdenSelected.VencimientoInicial)

                                Dim objListaSeguimiento As New List(Of OYDPLUSUtilidades.CombosReceptor)
                                Dim strFechaVencimiento As String = String.Empty

                                If Not IsNothing(pobjOrdenSelected.VencimientoInicial) Then
                                    If _ListaComboVenimientoInicial.Where(Function(i) i.ID = pobjOrdenSelected.VencimientoInicial).Count > 0 Then
                                        strFechaVencimiento = _ListaComboVenimientoInicial.Where(Function(i) i.ID = pobjOrdenSelected.VencimientoInicial).First.Retorno
                                    End If
                                End If


                                If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "INSTRUMENTOSEGUIMIENTO").Count > 0 Then
                                    For Each li In _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "INSTRUMENTOSEGUIMIENTO")
                                        If li.IDDependencia1 <> pobjOrdenSelected.VencimientoInicial And _
                                            li.Retorno >= strFechaVencimiento Then
                                            objListaSeguimiento.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                               .Retorno = li.Retorno,
                                                                                                               .Categoria = li.Categoria,
                                                                                                               .Descripcion = li.Descripcion,
                                                                                                               .Prioridad = li.Prioridad})
                                        End If
                                    Next
                                End If

                                ListaComboInstrumentoSeguimiento = objListaSeguimiento

                                If Not IsNothing(pobjOrdenSelected.TipoRegistro) Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_CONTRAPARTEDEFECTO" And i.IDDependencia1 = pobjOrdenSelected.TipoRegistro).Count > 0 Then
                                        Dim IDDependencia1 As Nullable(Of Integer) = _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_CONTRAPARTEDEFECTO" And i.IDDependencia1 = pobjOrdenSelected.TipoRegistro).First.ID
                                        Dim objListaContraparte As New List(Of OYDPLUSUtilidades.CombosReceptor)
                                        For Each li In _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_CONTRAPARTEDEFECTO")
                                            If li.ID = IDDependencia1 Then
                                                objListaContraparte.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                                   .Retorno = li.Retorno,
                                                                                                                   .Categoria = li.Categoria,
                                                                                                                   .Descripcion = li.Descripcion,
                                                                                                                   .Prioridad = li.Prioridad})
                                            End If
                                        Next

                                        ListaComboContraparte = objListaContraparte
                                    Else
                                        ListaComboContraparte = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "CONTRAPARTE", "CONTRAPARTE")
                                    End If
                                End If

                                If Not IsNothing(pobjOrdenSelected.Canal) Then
                                    ListaComboMedioVerificable = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "MEDIOVERIFICABLE", "MEDIOVERIFICABLE", pobjOrdenSelected.Canal)
                                End If

                                If Not IsNothing(pobjOrdenSelected.Naturaleza) Then
                                    ListaComboTipoEjecucion = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOEJECUCION", "TIPOEJECUCION", pobjOrdenSelected.Naturaleza)
                                End If

                                If Not IsNothing(pobjOrdenSelected.Finalidad) Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "FINALIDAD_DATOSADICIONALES" And i.ID = pobjOrdenSelected.Finalidad).Count > 0 Then
                                        ListaComboTipoFinalidad = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOFINALIDAD", "TIPOFINALIDAD", pobjOrdenSelected.Finalidad)
                                        ListaComboUbicacionPosicion = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "UBICACIONPOSICION", "UBICACIONPOSICION", pobjOrdenSelected.Finalidad)
                                    End If
                                End If

                                OpcionValoresDefecto = OPCION_EDITAR
                            Case OPCION_RECEPTOR
                                ListaComboMedioVerificable = Nothing
                                ListaComboCuentasCCCR = Nothing
                                ListaComboCanalesActivos = Nothing
                                ListaComboFirmasGiveOut = Nothing

                                ListaComboCuentasCCCR = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExterno, "CUENTASCRCC", "CUENTASCRCC")
                                ListaComboCanalesActivos = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExterno, "CANALESACTIVOS", "CANALESACTIVOS")
                                ListaComboFirmasGiveOut = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExterno, "FIRMASGIVEUP", "FIRMASGIVEUP")

                                OpcionValoresDefecto = OPCION_COMBOSRECEPTOR
                            Case OPCION_SUBCUENTA
                                ListaComboSubCuentasCCCR = Nothing
                                If Not IsNothing(pobjOrdenSelected.IDCuenta) Then
                                    ListaComboSubCuentasCCCR = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExterno, "SUBCUENTACRCC", "SUBCUENTACRCC", pobjOrdenSelected.IDCuenta)
                                End If

                                OpcionValoresDefecto = OPCION_SUBCUENTA
                            Case OPCION_TIPOOPCION
                                ListaComboInstrumento = Nothing
                                ListaComboVenimientoInicial = Nothing
                                ListaComboVenimientoFinal = Nothing
                                ListaComboTipoOpcion = Nothing

                                If Not IsNothing(pobjOrdenSelected.TipoInstruccion) Then
                                    ListaComboTipoOpcion = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOOPCION", "TIPOOPCION", pobjOrdenSelected.TipoInstruccion)
                                End If

                                OpcionValoresDefecto = OPCION_TIPOOPCION
                            Case OPCION_INSTRUMENTO
                                ListaComboVenimientoInicial = Nothing
                                ListaComboVenimientoFinal = Nothing
                                ListaComboInstrumento = Nothing

                                If Not IsNothing(pobjOrdenSelected.TipoRegularOSpread) Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "PRODUCTOSESPECIALES" And i.IDDependencia1 = pobjOrdenSelected.TipoRegularOSpread).Count > 0 Then
                                        ListaComboInstrumento = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "PRODUCTOSESPECIALES", "PRODUCTOSTIPOPRODUCTO", pobjOrdenSelected.TipoRegularOSpread)
                                    Else
                                        ListaComboInstrumento = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "PRODUCTOS", "PRODUCTOSTIPOPRODUCTO", pobjOrdenSelected.TipoRegularOSpread)
                                    End If
                                End If

                                OpcionValoresDefecto = OPCION_INSTRUMENTO
                            Case OPCION_VENCIMIENTOINICIAL
                                ListaComboVenimientoFinal = Nothing
                                ListaComboVenimientoInicial = Nothing

                                Dim objListaVencimientoInicial As New List(Of OYDPLUSUtilidades.CombosReceptor)

                                If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "VENCIMIENTOPRODUCTOS").Count > 0 Then
                                    For Each li In _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "VENCIMIENTOPRODUCTOS")
                                        If li.IDDependencia1 = pobjOrdenSelected.Instrumento And _
                                            String.Format("{0:yyyy-MM-dd}", dtmFechaServidor) <= li.Retorno Then
                                            objListaVencimientoInicial.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                                      .Retorno = li.Retorno,
                                                                                                                      .Categoria = li.Categoria,
                                                                                                                      .Descripcion = li.Descripcion,
                                                                                                                      .Prioridad = li.Prioridad})
                                        End If
                                    Next
                                End If

                                ListaComboVenimientoInicial = objListaVencimientoInicial

                                OpcionValoresDefecto = OPCION_VENCIMIENTOINICIAL
                            Case OPCION_VENCIMIENTOFINAL
                                ListaComboVenimientoFinal = Nothing
                                ListaComboInstrumentoSeguimiento = Nothing
                                ListaComboVenimientoFinal = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "VENCIMIENTOFINALPRODUCTOS", "VENCIMIENTOFINALPRODUCTOS", pobjOrdenSelected.TipoRegularOSpread, pobjOrdenSelected.VencimientoInicial)

                                Dim objListaSeguimiento As New List(Of OYDPLUSUtilidades.CombosReceptor)
                                Dim strFechaVencimiento As String = String.Empty

                                If Not IsNothing(pobjOrdenSelected.VencimientoInicial) Then
                                    If _ListaComboVenimientoInicial.Where(Function(i) i.ID = pobjOrdenSelected.VencimientoInicial).Count > 0 Then
                                        strFechaVencimiento = _ListaComboVenimientoInicial.Where(Function(i) i.ID = pobjOrdenSelected.VencimientoInicial).First.Retorno
                                    End If
                                End If

                                If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "INSTRUMENTOSEGUIMIENTO").Count > 0 Then
                                    For Each li In _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "INSTRUMENTOSEGUIMIENTO")
                                        If li.IDDependencia1 <> pobjOrdenSelected.VencimientoInicial And _
                                            li.Retorno >= strFechaVencimiento Then
                                            objListaSeguimiento.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                               .Retorno = li.Retorno,
                                                                                                               .Categoria = li.Categoria,
                                                                                                               .Descripcion = li.Descripcion,
                                                                                                               .Prioridad = li.Prioridad})
                                        End If
                                    Next
                                End If

                                ListaComboInstrumentoSeguimiento = objListaSeguimiento

                                OpcionValoresDefecto = OPCION_VENCIMIENTOFINAL
                            Case OPCION_CONTRAPARTE

                                ListaComboContraparte = Nothing

                                If Not IsNothing(pobjOrdenSelected.TipoRegistro) Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_CONTRAPARTEDEFECTO" And i.IDDependencia1 = pobjOrdenSelected.TipoRegistro).Count > 0 Then
                                        Dim IDDependencia1 As Nullable(Of Integer) = _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_CONTRAPARTEDEFECTO" And i.IDDependencia1 = pobjOrdenSelected.TipoRegistro).First.ID
                                        Dim objListaContraparte As New List(Of OYDPLUSUtilidades.CombosReceptor)
                                        For Each li In _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_CONTRAPARTEDEFECTO")
                                            If li.ID = IDDependencia1 Then
                                                objListaContraparte.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID,
                                                                                                                   .Retorno = li.Retorno,
                                                                                                                   .Categoria = li.Categoria,
                                                                                                                   .Descripcion = li.Descripcion,
                                                                                                                   .Prioridad = li.Prioridad})
                                            End If
                                        Next

                                        ListaComboContraparte = objListaContraparte
                                    Else
                                        ListaComboContraparte = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "CONTRAPARTE", "CONTRAPARTE")
                                    End If
                                End If

                                If logNuevoRegistro Then
                                    If Not IsNothing(pobjOrdenSelected.TipoRegistro) Then
                                        If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_REQUIERECONDICIONES" And i.ID = pobjOrdenSelected.TipoRegistro).Count > 0 Then
                                            HabilitarCondicionesNegociacion = True
                                        Else
                                            HabilitarCondicionesNegociacion = False
                                            pobjOrdenSelected.Naturaleza = Nothing
                                            pobjOrdenSelected.TipoEjecucion = Nothing
                                            pobjOrdenSelected.Duracion = Nothing
                                            pobjOrdenSelected.PrecioStop = 0
                                            pobjOrdenSelected.CantidadVisible = 0
                                            pobjOrdenSelected.OtroInstrumento = Nothing
                                            pobjOrdenSelected.TipoPrecio = Nothing
                                        End If
                                    End If

                                    If Not IsNothing(pobjOrdenSelected.TipoRegistro) Then
                                        If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_REQUIEREGIVEOUT" And i.ID = pobjOrdenSelected.TipoRegistro).Count > 0 Then
                                            If pobjOrdenSelected.GiveOut Then
                                                HabilitarGiveOut = True
                                            Else
                                                HabilitarGiveOut = False
                                            End If
                                        Else
                                            HabilitarGiveOut = False
                                            pobjOrdenSelected.GiveOut = False
                                            pobjOrdenSelected.FirmaGiveOut = Nothing
                                            pobjOrdenSelected.ReferenciaGiveOut = String.Empty
                                        End If
                                    End If

                                Else
                                    If Not IsNothing(pobjOrdenSelected.TipoRegistro) Then
                                        If DiccionarioEdicionCamposOYDPLUS.ContainsKey("bitEsGiveOut") Then
                                            If DiccionarioEdicionCamposOYDPLUS("bitEsGiveOut") Then
                                                If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_REQUIEREGIVEOUT" And i.ID = pobjOrdenSelected.TipoRegistro).Count > 0 Then
                                                    If pobjOrdenSelected.GiveOut Then
                                                        HabilitarGiveOut = True
                                                    Else
                                                        HabilitarGiveOut = False
                                                    End If
                                                Else
                                                    HabilitarGiveOut = False
                                                    pobjOrdenSelected.GiveOut = False
                                                    pobjOrdenSelected.FirmaGiveOut = Nothing
                                                    pobjOrdenSelected.ReferenciaGiveOut = String.Empty
                                                End If
                                            Else
                                                HabilitarGiveOut = False
                                                pobjOrdenSelected.GiveOut = False
                                                pobjOrdenSelected.FirmaGiveOut = Nothing
                                                pobjOrdenSelected.ReferenciaGiveOut = String.Empty
                                            End If
                                        End If
                                    End If
                                End If

                                OpcionValoresDefecto = OPCION_CONTRAPARTE
                            Case OPCION_MEDIOVERIFICABLE
                                ListaComboMedioVerificable = Nothing
                                If Not IsNothing(pobjOrdenSelected.Canal) Then
                                    ListaComboMedioVerificable = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "MEDIOVERIFICABLE", "MEDIOVERIFICABLE", pobjOrdenSelected.Canal)
                                End If

                                OpcionValoresDefecto = OPCION_MEDIOVERIFICABLE
                            Case OPCION_TIPOEJECUCION
                                ListaComboTipoEjecucion = Nothing
                                If Not IsNothing(pobjOrdenSelected.Naturaleza) Then
                                    ListaComboTipoEjecucion = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOEJECUCION", "TIPOEJECUCION", pobjOrdenSelected.Naturaleza)
                                End If

                                OpcionValoresDefecto = OPCION_TIPOEJECUCION
                            Case OPCION_TIPOFINALIDAD
                                ListaComboTipoFinalidad = Nothing
                                ListaComboUbicacionPosicion = Nothing

                                If Not IsNothing(pobjOrdenSelected.Finalidad) Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "FINALIDAD_DATOSADICIONALES" And i.ID = pobjOrdenSelected.Finalidad).Count > 0 Then
                                        If logNuevoRegistro Then
                                            HabilitarDatosAdicionalesFinalidad = True
                                        Else
                                            HabilitarDatosAdicionalesFinalidad = False
                                        End If

                                        ListaComboTipoFinalidad = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOFINALIDAD", "TIPOFINALIDAD", pobjOrdenSelected.Finalidad)
                                        ListaComboUbicacionPosicion = ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "UBICACIONPOSICION", "UBICACIONPOSICION", pobjOrdenSelected.Finalidad)
                                    Else
                                        HabilitarDatosAdicionalesFinalidad = False
                                        pobjOrdenSelected.TipoCobertura = Nothing
                                        pobjOrdenSelected.UbicacionPosicion = Nothing
                                        pobjOrdenSelected.DescripcionPosicion = String.Empty
                                        pobjOrdenSelected.MontoCubrir = 0
                                        pobjOrdenSelected.Moneda = Nothing
                                    End If
                                End If


                                OpcionValoresDefecto = OPCION_TIPOFINALIDAD
                        End Select

                        If Not DiccionarioCombosOYDPlus.ContainsKey("ESTADOORDEN") Then DiccionarioCombosOYDPlus.Add("ESTADOORDEN", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "ESTADOORDEN", "ESTADOORDEN"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("ESTADOPREORDEN") Then DiccionarioCombosOYDPlus.Add("ESTADOPREORDEN", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "ESTADOPREORDEN", "ESTADOPREORDEN"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("FECHASGENERAR") Then DiccionarioCombosOYDPlus.Add("FECHASGENERAR", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "FECHASGENERAR", "FECHASGENERAR"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("TIPOOPERACION") Then DiccionarioCombosOYDPlus.Add("TIPOOPERACION", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOOPERACION", "TIPOOPERACION"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("TIPOINSTRUMENTO") Then DiccionarioCombosOYDPlus.Add("TIPOINSTRUMENTO", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOINSTRUMENTO", "TIPOINSTRUMENTO"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("TIPOCOMISION") Then DiccionarioCombosOYDPlus.Add("TIPOCOMISION", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOCOMISION", "TIPOCOMISION"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("FACTURARCOMISION") Then DiccionarioCombosOYDPlus.Add("FACTURARCOMISION", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "FACTURARCOMISION", "FACTURARCOMISION"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("TIPOREGISTRO") Then DiccionarioCombosOYDPlus.Add("TIPOREGISTRO", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOREGISTRO", "TIPOREGISTRO"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("NATURALEZA") Then DiccionarioCombosOYDPlus.Add("NATURALEZA", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "NATURALEZA", "NATURALEZA"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("DURACION") Then DiccionarioCombosOYDPlus.Add("DURACION", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "DURACION", "DURACION"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("FINALIDAD") Then DiccionarioCombosOYDPlus.Add("FINALIDAD", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "FINALIDAD", "FINALIDAD"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("MONEDA") Then DiccionarioCombosOYDPlus.Add("MONEDA", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "MONEDA", "MONEDA"))
                        If Not DiccionarioCombosOYDPlus.ContainsKey("TIPOPRECIOINSTRUMENTO") Then DiccionarioCombosOYDPlus.Add("TIPOPRECIOINSTRUMENTO", ExtraerListaPorCategoriaExterno(_ListaCombosSistemaExternoCompletos, "TIPOPRECIOINSTRUMENTO", "TIPOPRECIOINSTRUMENTO"))

                        MyBase.CambioItem("DiccionarioCombosOYDPlus")

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

                            HabilitarCamposOYDPLUS(_OrdenOYDPLUSSelected)
                            ValidarHabilitarNegocio(_OrdenOYDPLUSSelected)

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
    Public Sub ValidarHabilitarNegocio(ByVal pobjOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados)
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                If Not String.IsNullOrEmpty(pobjOrdenSelected.CodigoOYD) And pobjOrdenSelected.CodigoOYD <> "-9999999999" And _
                       Not String.IsNullOrEmpty(pobjOrdenSelected.Receptor) Then

                    If logNuevoRegistro Or logDuplicarRegistro Or pobjOrdenSelected.strEstado = "R" Then
                        HabilitarNegocio = True

                        For Each li In ListaComboCamposEditables
                            If DiccionarioEdicionCamposOYDPLUS.ContainsKey(li.Retorno) Then
                                DiccionarioEdicionCamposOYDPLUS(li.Retorno) = True
                            End If
                        Next
                    Else
                        HabilitarNegocio = False

                        For Each li In ListaComboCamposPermitidosEdicion
                            If DiccionarioEdicionCamposOYDPLUS.ContainsKey(li.NombreCampo) Then
                                DiccionarioEdicionCamposOYDPLUS(li.NombreCampo) = li.PermiteEditar
                            End If
                        Next
                    End If
                Else
                    HabilitarNegocio = False

                    For Each li In ListaComboCamposEditables
                        If DiccionarioEdicionCamposOYDPLUS.ContainsKey(li.Retorno) Then
                            DiccionarioEdicionCamposOYDPLUS(li.Retorno) = False
                        End If
                    Next
                End If
            Else
                HabilitarNegocio = False

                For Each li In ListaComboCamposEditables
                    If DiccionarioEdicionCamposOYDPLUS.ContainsKey(li.Retorno) Then
                        DiccionarioEdicionCamposOYDPLUS(li.Retorno) = False
                    End If
                Next
            End If

            MyBase.CambioItem("DiccionarioEdicionCamposOYDPLUS")
        Catch ex As Exception
            IsBusy = False
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
    Public Sub SeleccionarClienteOYDPLUS(ByVal pobjCliente As OYDUtilidades.BuscadorClientes, ByVal pobjOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados, Optional ByVal pstrOpcion As String = "")
        Try
            If Not IsNothing(pobjCliente) Then
                If logEditarRegistro Or logNuevoRegistro Then

                    If Not IsNothing(pobjOrdenSelected) Then

                        pobjOrdenSelected.CodigoOYD = pobjCliente.IdComitente
                        pobjOrdenSelected.NombreCliente = pobjCliente.Nombre
                        pobjOrdenSelected.NroDocumento = pobjCliente.NroDocumento

                        BorrarCliente = False
                    End If
                End If
            Else
                If Not IsNothing(pobjOrdenSelected) Then
                    pobjOrdenSelected.CodigoOYD = "-9999999999"
                    pobjOrdenSelected.NombreCliente = "(No Seleccionado)"
                    pobjOrdenSelected.NroDocumento = 0

                    BorrarCliente = True

                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarClienteOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para habilitar o deshabilitar los campos dependiendo del tipo de orden que sea.
    ''' Desarrollado por Juan david Correa
    ''' Fecha 24 de agosto del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub HabilitarCamposOYDPLUS(ByVal pobjOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados)
        Try
            If Not IsNothing(pobjOrdenSelected) Then
                If logNuevoRegistro = False And logEditarRegistro = False Then
                    HabilitarEncabezado = False
                    HabilitarNegocio = False
                    HabilitarCantidadMinima = False
                    HabilitarCondicionesNegociacion = False
                    If pobjOrdenSelected.GiveOut Then
                        HabilitarGiveOut = True
                    Else
                        HabilitarGiveOut = False
                    End If
                    HabilitarDatosAdicionalesFinalidad = False
                    HabilitarPrecioSpot = False

                    For Each li In ListaComboCamposEditables
                        If DiccionarioEdicionCamposOYDPLUS.ContainsKey(li.Retorno) Then
                            DiccionarioEdicionCamposOYDPLUS(li.Retorno) = False
                        End If
                    Next
                Else
                    If logNuevoRegistro Or logDuplicarRegistro Or pobjOrdenSelected.strEstado = "R" Then
                        HabilitarEncabezado = True
                    Else
                        HabilitarEncabezado = False
                        HabilitarNegocio = False

                        For Each li In ListaComboCamposPermitidosEdicion
                            If DiccionarioEdicionCamposOYDPLUS.ContainsKey(li.NombreCampo) Then
                                DiccionarioEdicionCamposOYDPLUS(li.NombreCampo) = li.PermiteEditar
                            End If
                        Next
                    End If

                    If Not IsNothing(_ListaCombosSistemaExternoCompletos) Then
                        If logNuevoRegistro Or logDuplicarRegistro Or pobjOrdenSelected.strEstado = "R" Then
                            If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOEJECUCION" And i.ID = pobjOrdenSelected.TipoEjecucion).Count > 0 Then
                                If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOEJECUCION" And i.ID = pobjOrdenSelected.TipoEjecucion).First.Retorno = String.Format("{0}-{1}", pobjOrdenSelected.Naturaleza, CANTIDADMINIMA) Then
                                    HabilitarCantidadMinima = True
                                Else
                                    HabilitarCantidadMinima = False
                                End If
                            End If
                        Else
                            HabilitarCantidadMinima = False
                        End If

                        If logNuevoRegistro Or logDuplicarRegistro Or pobjOrdenSelected.strEstado = "R" Then
                            If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_REQUIERECONDICIONES" And i.ID = pobjOrdenSelected.TipoRegistro).Count > 0 Then
                                HabilitarCondicionesNegociacion = True
                            Else
                                HabilitarCondicionesNegociacion = False
                            End If
                        Else
                            HabilitarCondicionesNegociacion = False
                        End If

                        If logNuevoRegistro Or logDuplicarRegistro Or pobjOrdenSelected.strEstado = "R" Then
                            If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_REQUIEREGIVEOUT" And i.ID = pobjOrdenSelected.TipoRegistro).Count > 0 Then
                                If pobjOrdenSelected.GiveOut Then
                                    HabilitarGiveOut = True
                                Else
                                    HabilitarGiveOut = False
                                End If
                            Else
                                HabilitarGiveOut = False
                            End If
                        Else
                            If DiccionarioEdicionCamposOYDPLUS.ContainsKey("bitEsGiveOut") Then
                                If DiccionarioEdicionCamposOYDPLUS("bitEsGiveOut") Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_REQUIEREGIVEOUT" And i.ID = pobjOrdenSelected.TipoRegistro).Count > 0 Then
                                        If pobjOrdenSelected.GiveOut Then
                                            HabilitarGiveOut = True
                                        Else
                                            HabilitarGiveOut = False
                                        End If
                                    Else
                                        HabilitarGiveOut = False
                                    End If
                                Else
                                    HabilitarGiveOut = False
                                End If
                            End If
                        End If

                        If logNuevoRegistro Or logDuplicarRegistro Or pobjOrdenSelected.strEstado = "R" Then
                            If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "FINALIDAD_DATOSADICIONALES" And i.ID = pobjOrdenSelected.Finalidad).Count > 0 Then
                                HabilitarDatosAdicionalesFinalidad = True
                            Else
                                HabilitarDatosAdicionalesFinalidad = False
                            End If
                        Else
                            HabilitarDatosAdicionalesFinalidad = False
                        End If

                        If logNuevoRegistro Or logDuplicarRegistro Or pobjOrdenSelected.strEstado = "R" Then
                            If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "HABILITAR_PRECIOSPOT" And i.Retorno = "1").Count > 0 Then
                                HabilitarPrecioSpot = True
                            Else
                                HabilitarPrecioSpot = False
                            End If
                        Else
                            If DiccionarioEdicionCamposOYDPLUS.ContainsKey("numPrecioSpot") Then
                                If DiccionarioEdicionCamposOYDPLUS("numPrecioSpot") Then
                                    If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "HABILITAR_PRECIOSPOT" And i.Retorno = "1").Count > 0 Then
                                        HabilitarPrecioSpot = True
                                    Else
                                        HabilitarPrecioSpot = False
                                    End If
                                Else
                                    HabilitarPrecioSpot = False
                                End If
                            End If
                        End If
                    Else
                        HabilitarCantidadMinima = False
                        HabilitarCondicionesNegociacion = False
                        HabilitarGiveOut = False
                        HabilitarDatosAdicionalesFinalidad = False
                        HabilitarPrecioSpot = False

                        For Each li In ListaComboCamposEditables
                            If DiccionarioEdicionCamposOYDPLUS.ContainsKey(li.Retorno) Then
                                DiccionarioEdicionCamposOYDPLUS(li.Retorno) = False
                            End If
                        Next
                    End If
                End If

                If pobjOrdenSelected.TipoComision = 1 Then
                    MostrarComision = True
                Else
                    MostrarComision = False
                End If
            End If

            MyBase.CambioItem("DiccionarioEdicionCamposOYDPLUS")

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
    Public Sub ValidarConsultaCombosCliente(ByVal pobjOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados, Optional ByVal pstrUserState As String = "")
        Try
            If logEditarRegistro Or logNuevoRegistro Then
                If Not IsNothing(pobjOrdenSelected) Then
                    If Not String.IsNullOrEmpty(pobjOrdenSelected.NroDocumento) And pobjOrdenSelected.NroDocumento <> "0" And _
                        Not String.IsNullOrEmpty(pobjOrdenSelected.CodigoOYD) And pobjOrdenSelected.CodigoOYD <> "-9999999999" And _
                        Not String.IsNullOrEmpty(pobjOrdenSelected.Receptor) Then
                        CargarCombosSistemaExternoReceptorOYDPLUS(pobjOrdenSelected.NroDocumento, pobjOrdenSelected.CodigoOYD, pobjOrdenSelected.Receptor, pstrUserState)
                    Else
                        IsBusy = False
                    End If
                Else
                    IsBusy = False
                End If
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al habilitar la busqueda en los controles dependiendo del tipo de orden.", Me.ToString, "HabilitarConsultaControles", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Desarrollado por Juan David Correa.
    ''' Se realizan las validaciones para el guardado de la orden de oydplus.
    ''' Fecha 27 de agosto del 2012
    ''' </summary>
    ''' <remarks></remarks>
    Public Function ValidarGuardadoOrden(ByVal pobjOrden As OyDPLUSOrdenesDerivados.OrdenDerivados) As Boolean
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
                    If IsNothing(pobjOrden.CodigoOYD) Or String.IsNullOrEmpty(pobjOrden.CodigoOYD) Then
                        strMensajeValidacion = String.Format("{0}{1} - Cuenta", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
                'Valida el campo de Tipo de negocio
                If IsNothing(pobjOrden.TipoNegocio) Or String.IsNullOrEmpty(pobjOrden.TipoNegocio) Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo negocio", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo fecha generar orden
                If IsNothing(pobjOrden.FechaVigenciaOrden) Or String.IsNullOrEmpty(pobjOrden.FechaVigenciaOrden) Then
                    strMensajeValidacion = String.Format("{0}{1} - Fecha generar orden", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Cuenta CCCR
                If IsNothing(pobjOrden.IDCuenta) Or pobjOrden.IDCuenta = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Cuenta CCCR", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Operación
                If IsNothing(pobjOrden.TipoOperacion) Or pobjOrden.TipoOperacion = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Operación", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Tipo instrumento
                If IsNothing(pobjOrden.TipoInstruccion) Or pobjOrden.TipoInstruccion = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo instrumento", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Tipo
                If IsNothing(pobjOrden.TipoRegularOSpread) Or pobjOrden.TipoRegularOSpread = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo regular o spread", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Instrumento
                If IsNothing(pobjOrden.Instrumento) Or pobjOrden.Instrumento = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Instrumento", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Vencimiento inicial
                If IsNothing(pobjOrden.VencimientoInicial) Or pobjOrden.VencimientoInicial = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Vencimiento inicial", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de precio
                If IsNothing(pobjOrden.Precio) Or pobjOrden.Precio = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Precio", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Cantidad contratos
                If IsNothing(pobjOrden.Cantidad) Or pobjOrden.Cantidad = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Cantidad contratos", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Tipo de comisión
                If IsNothing(pobjOrden.TipoComision) Or pobjOrden.TipoComision = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo comisión", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                Else
                    If pobjOrden.ComisionPorPorcentaje Then
                        If IsNothing(pobjOrden.PorcentajeComision) Or pobjOrden.PorcentajeComision = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Comisión", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    Else
                        If IsNothing(pobjOrden.Comision) Or pobjOrden.Comision = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Comisión", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    End If
                End If
                'Valida el campo de Facturar comisión
                If IsNothing(pobjOrden.FacturarComision) Or pobjOrden.FacturarComision = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Facturar comisión", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Facturar comisión
                If IsNothing(pobjOrden.TipoRegistro) Or pobjOrden.TipoRegistro = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo registro", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Facturar comisión
                If IsNothing(pobjOrden.Contraparte) Or pobjOrden.Contraparte = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Contraparte", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Facturar comisión
                If IsNothing(pobjOrden.Canal) Or pobjOrden.Canal = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Canal", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Medio verificable
                If IsNothing(pobjOrden.MedioVerificable) Or pobjOrden.MedioVerificable = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Medio verificable", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                'Valida el campo de Hora Toma
                If IsNothing(pobjOrden.FechaHoraToma) Then
                    strMensajeValidacion = String.Format("{0}{1} - Hora toma", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
                If HabilitarCondicionesNegociacion Then
                    'Valida el campo de Naturaleza
                    If IsNothing(pobjOrden.Naturaleza) Or pobjOrden.Naturaleza = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Naturaleza", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                    'Valida el campo de Tipo ejecución
                    If IsNothing(pobjOrden.TipoEjecucion) Or pobjOrden.TipoEjecucion = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Tipo ejecución", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                    'Valida el campo de Tipo ejecución
                    If IsNothing(pobjOrden.TipoEjecucion) Or pobjOrden.TipoEjecucion = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Tipo ejecución", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                    If HabilitarCantidadMinima Then
                        'Valida el campo de Cantidad minima
                        If IsNothing(pobjOrden.CantidadMinima) Or pobjOrden.CantidadMinima = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Cantidad minima", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    End If
                    'Valida el campo de Duracion
                    If IsNothing(pobjOrden.Duracion) Or pobjOrden.Duracion = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Duración", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If

                'Valida el campo de Finalidad
                If IsNothing(pobjOrden.Finalidad) Or pobjOrden.Finalidad = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Finalidad", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                If HabilitarGiveOut Then
                    If pobjOrden.GiveOut Then
                        'Valida el campo de Firma Give OUT
                        If IsNothing(pobjOrden.FirmaGiveOut) Or pobjOrden.FirmaGiveOut = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Firma Give Out", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    End If
                End If

                If logTieneError Then
                    strMensajeValidacion = String.Format("Señor usuario los siguientes campos son requeridos:{0}", strMensajeValidacion)
                    mostrarMensaje(strMensajeValidacion, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                Else
                    'Valida la distribución de comisiones
                    Dim objListaReceptores As List(Of OyDPLUSOrdenesDerivados.ReceptoresOrden)

                    objListaReceptores = _ListaReceptoresOrdenes

                    If Not IsNothing(objListaReceptores) Then
                        Dim sumarPorcentaje As Double = 0
                        Dim logReceptorVacion As Boolean = False
                        For Each li In objListaReceptores
                            If String.IsNullOrEmpty(li.IDReceptor) Then
                                logReceptorVacion = True
                            End If
                            sumarPorcentaje += li.Porcentaje
                        Next

                        If logReceptorVacion Then
                            strMensajeValidacion = "Hay algunos registros de los receptores de la distrubución de comisiones que se encuentran vacios."
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
                    Else
                        Return True
                    End If
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
            IsBusy = False
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
    Public Sub BuscarControlValidacion(ByVal pViewOrdenes As OrdenesPLUSDerivadosView, ByVal pstrOpcion As String)
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
            Dim objTipoNegocioReceptor As New List(Of OYDPLUSUtilidades.tblTipoNegocioReceptor)

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
    Public Sub ObtenerValoresOrdenAnterior(ByVal pobjOrden As OyDPLUSOrdenesDerivados.OrdenDerivados, ByRef pobjOrdenSalvarDatos As OyDPLUSOrdenesDerivados.OrdenDerivados)
        Try
            If Not IsNothing(pobjOrden) Then
                Dim objNewOrdenOYD As New OyDPLUSOrdenesDerivados.OrdenDerivados

                objNewOrdenOYD.ID = pobjOrden.ID
                objNewOrdenOYD.IDPreorden = pobjOrden.IDPreorden
                objNewOrdenOYD.NroOrden = pobjOrden.NroOrden
                objNewOrdenOYD.FechaGenerarOrden = pobjOrden.FechaGenerarOrden
                objNewOrdenOYD.FechaPreorden = pobjOrden.FechaPreorden
                objNewOrdenOYD.FechaOrden = pobjOrden.FechaOrden
                objNewOrdenOYD.FechaVigenciaOrden = pobjOrden.FechaVigenciaOrden
                objNewOrdenOYD.TipoNegocio = pobjOrden.TipoNegocio
                objNewOrdenOYD.strEstado = pobjOrden.strEstado
                objNewOrdenOYD.NombreEstado = pobjOrden.NombreEstado
                objNewOrdenOYD.Receptor = pobjOrden.Receptor
                objNewOrdenOYD.NombreReceptor = pobjOrden.NombreReceptor
                objNewOrdenOYD.NroDocumento = pobjOrden.NroDocumento
                objNewOrdenOYD.IdCuentaMultiproducto = pobjOrden.IdCuentaMultiproducto
                objNewOrdenOYD.CodigoOYD = pobjOrden.CodigoOYD
                objNewOrdenOYD.NombreCliente = pobjOrden.NombreCliente
                objNewOrdenOYD.IDCuenta = pobjOrden.IDCuenta
                objNewOrdenOYD.IDSubCuenta = pobjOrden.IDSubCuenta
                objNewOrdenOYD.TipoOperacion = pobjOrden.TipoOperacion
                objNewOrdenOYD.TipoInstruccion = pobjOrden.TipoInstruccion
                objNewOrdenOYD.TipoRegularOSpread = pobjOrden.TipoRegularOSpread
                objNewOrdenOYD.Instrumento = pobjOrden.Instrumento
                objNewOrdenOYD.VencimientoInicial = pobjOrden.VencimientoInicial
                objNewOrdenOYD.VencimientoFinal = pobjOrden.VencimientoFinal
                objNewOrdenOYD.Precio = pobjOrden.Precio
                objNewOrdenOYD.Cantidad = pobjOrden.Cantidad
                objNewOrdenOYD.CantidadMinima = pobjOrden.CantidadMinima
                objNewOrdenOYD.Prima = pobjOrden.Prima
                objNewOrdenOYD.ComisionPorPorcentaje = pobjOrden.ComisionPorPorcentaje
                objNewOrdenOYD.TipoComision = pobjOrden.TipoComision
                objNewOrdenOYD.Comision = pobjOrden.Comision
                objNewOrdenOYD.PorcentajeComision = pobjOrden.PorcentajeComision
                objNewOrdenOYD.FacturarComision = pobjOrden.FacturarComision
                objNewOrdenOYD.logFacturarComision = pobjOrden.logFacturarComision
                objNewOrdenOYD.Estado = pobjOrden.Estado
                objNewOrdenOYD.NombreEstadoDestino = pobjOrden.NombreEstadoDestino
                objNewOrdenOYD.SubEstado = pobjOrden.SubEstado
                objNewOrdenOYD.NombreSubEstadoDestino = pobjOrden.NombreSubEstadoDestino
                objNewOrdenOYD.RegistroEnBolsa = pobjOrden.RegistroEnBolsa
                objNewOrdenOYD.TipoRegistro = pobjOrden.TipoRegistro
                objNewOrdenOYD.Contraparte = pobjOrden.Contraparte
                objNewOrdenOYD.Canal = pobjOrden.Canal
                objNewOrdenOYD.MedioVerificable = pobjOrden.MedioVerificable
                objNewOrdenOYD.FechaHoraToma = pobjOrden.FechaHoraToma
                objNewOrdenOYD.DetalleMedioVerificable = pobjOrden.DetalleMedioVerificable
                objNewOrdenOYD.PrecioSpot = pobjOrden.PrecioSpot
                objNewOrdenOYD.Naturaleza = pobjOrden.Naturaleza
                objNewOrdenOYD.TipoEjecucion = pobjOrden.TipoEjecucion
                objNewOrdenOYD.Duracion = pobjOrden.Duracion
                objNewOrdenOYD.PrecioStop = pobjOrden.PrecioStop
                objNewOrdenOYD.CantidadVisible = pobjOrden.CantidadVisible
                objNewOrdenOYD.OtroInstrumento = pobjOrden.OtroInstrumento
                objNewOrdenOYD.TipoPrecio = pobjOrden.TipoPrecio
                objNewOrdenOYD.Instrucciones = pobjOrden.Instrucciones
                objNewOrdenOYD.Finalidad = pobjOrden.Finalidad
                objNewOrdenOYD.TipoCobertura = pobjOrden.TipoCobertura
                objNewOrdenOYD.UbicacionPosicion = pobjOrden.UbicacionPosicion
                objNewOrdenOYD.DescripcionPosicion = pobjOrden.DescripcionPosicion
                objNewOrdenOYD.MontoCubrir = pobjOrden.MontoCubrir
                objNewOrdenOYD.Moneda = pobjOrden.Moneda
                objNewOrdenOYD.GiveOut = pobjOrden.GiveOut
                objNewOrdenOYD.FirmaGiveOut = pobjOrden.FirmaGiveOut
                objNewOrdenOYD.ReferenciaGiveOut = pobjOrden.ReferenciaGiveOut
                objNewOrdenOYD.Comentarios = pobjOrden.Comentarios
                objNewOrdenOYD.DiasAvisoCumplimiento = pobjOrden.DiasAvisoCumplimiento
                objNewOrdenOYD.DetalleReceptores = pobjOrden.DetalleReceptores
                objNewOrdenOYD.Usuario = pobjOrden.Usuario
                objNewOrdenOYD.UsuarioWindows = pobjOrden.UsuarioWindows
                objNewOrdenOYD.Actualizacion = pobjOrden.Actualizacion

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
    Public Sub ObtenerValoresOrdenEnLista(ByVal pobjOrden As OyDPLUSOrdenesDerivados.OrdenDerivados)
        Try
            If Not IsNothing(pobjOrden) Then
                If ListaOrdenOYDPLUS.Where(Function(i) i.ID = pobjOrden.ID).Count > 0 Then
                    ListaOrdenOYDPLUS.Where(Function(i) i.ID = pobjOrden.ID).FirstOrDefault.Receptor = pobjOrden.Receptor
                    ListaOrdenOYDPLUS.Where(Function(i) i.ID = pobjOrden.ID).FirstOrDefault.TipoNegocio = pobjOrden.TipoNegocio
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
            IsBusy = False
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
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al mostrar la notificación.", Me.ToString(), "MostrarNotificacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Actualizar elemento buscado con los datos recibidos como parámetro
    ''' </summary>
    ''' <param name="pstrTipoItem">Tipo de objeto que se recibe</param>
    ''' <param name="pobjItem">Item enviado como parámetro</param>
    Public Sub actualizarItemReceptor(ByVal pobjItem As OyDPLUSOrdenesDerivados.ReceptoresBusqueda)
        Try
            If Not IsNothing(pobjItem) Then
                Me.ReceptoresOrdenSelected.IDComercial = pobjItem.IDComercial
                Me.ReceptoresOrdenSelected.IDReceptor = pobjItem.CodigoComercial
                Me.ReceptoresOrdenSelected.Nombre = pobjItem.Nombre
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el item de la orden.", Me.ToString(), "actualizarItemOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos OYDPlus"

    Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.OrdenDerivados))
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
                                strEstado = _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.strEstado
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

                ListaOrdenOYDPLUS = dcProxyConsulta.OrdenDerivados.ToList

                If Not String.IsNullOrEmpty(strNotificacion) Then
                    If _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).Count > 0 Then
                        _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.NroOrden = intNroOrden
                        _ListaOrdenOYDPLUS.Where(Function(i) i.ID = intIDOrdenTimer).First.strEstado = strEstado
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
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarCombosOYDCOMPLETOS(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        Dim objListaNodosCategoria As New List(Of OYDPLUSUtilidades.CombosReceptor)

                        For Each liRegistro In lo.Entities.Where(Function(i) i.Categoria = li)
                            objListaNodosCategoria.Add(liRegistro)
                        Next

                        objDiccionarioCompleto.Add(li, objListaNodosCategoria)
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
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        Dim objListaNodosCategoria As New List(Of OYDPLUSUtilidades.CombosReceptor)

                        For Each liRegistro In lo.Entities.Where(Function(i) i.Categoria = li)
                            objListaNodosCategoria.Add(liRegistro)
                        Next

                        objDiccionario.Add(li, objListaNodosCategoria)
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
                If lo.UserState = "INICIO" Then
                    ListaCombosSistemaExternoCompletos = lo.Entities.ToList

                    CargarTipoNegocioReceptor("INICIO", String.Empty, _Modulo)
                Else
                    ListaCombosSistemaExterno = lo.Entities.ToList

                    If Not IsNothing(_OrdenOYDPLUSSelected) Then
                        If lo.UserState = OPCION_EDITAR Then
                            CargarCombosOYDPLUS(OPCION_EDITAR, _OrdenAnteriorOYDPLUS.Receptor, OPCION_EDITAR)
                        ElseIf lo.UserState = OPCION_DUPLICAR Then
                            CargarCombosOYDPLUS(OPCION_DUPLICAR, _OrdenAnteriorOYDPLUS.Receptor, OPCION_DUPLICAR)
                        Else
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_RECEPTOR)
                        End If
                    End If
                End If
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
                    mostrarMensaje("Señor Usuario, el receptor seleccionado no tiene tipos de negocio configurados para el modulo.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoValidarPermisosUsuario(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.tblRespuestaValidaciones))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strMensajeError As String = String.Empty

                    For Each li In lo.Entities.ToList
                        If li.Exitoso = False Then
                            If String.IsNullOrEmpty(strMensajeError) Then
                                strMensajeError = li.Mensaje
                            Else
                                strMensajeError = String.Format("{0}{1}{2}", strMensajeError, vbCrLf, li.Mensaje)
                            End If
                        End If
                    Next

                    If Not String.IsNullOrEmpty(strMensajeError) Then
                        MyBase.RetornarValorEdicionNavegacion()
                        A2Utilidades.Mensajes.mostrarMensaje(strMensajeError, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    Else
                        If lo.UserState = OPCION_NUEVO Then
                            CargarReceptoresUsuarioDerivados(OPCION_NUEVO)
                        Else
                            If _OrdenOYDPLUSSelected.strEstado = "R" Then
                                ValidarCamposEditablesOrden(OPCION_EDITAR)
                            Else
                                ValidarEdicionOrdenDerivados(OPCION_EDITAR)
                            End If
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se pudo validar los datos del usuario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            Else
                If logNuevoRegistro Or logEditarRegistro Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                End If
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoValidarEdicionOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.tblRespuestaValidaciones))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strMensajeError As String = String.Empty

                    For Each li In lo.Entities.ToList
                        If li.Exitoso = False Then
                            If String.IsNullOrEmpty(strMensajeError) Then
                                strMensajeError = li.Mensaje
                            Else
                                strMensajeError = String.Format("{0}{1}{2}", strMensajeError, vbCrLf, li.Mensaje)
                            End If
                        End If
                    Next

                    If Not String.IsNullOrEmpty(strMensajeError) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMensajeError, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    Else
                        ValidarCamposEditablesOrden(OPCION_EDITAR)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se pudo validar el estado de la orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            Else
                If logNuevoRegistro Or logEditarRegistro Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar el estado de la orden.", Me.ToString(), "TerminoValidarEdicionOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                End If
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar el estado de la orden.", Me.ToString(), "TerminoValidarEdicionOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoValidarCamposEditablesOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.CamposEditablesOrden))
        Try
            If lo.HasError = False Then
                ListaComboCamposPermitidosEdicion = lo.Entities.ToList

                EditarOrdenOYDPLUS()
            Else
                If logNuevoRegistro Or logEditarRegistro Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar los campos editables.", Me.ToString(), "TerminoValidarCamposEditablesOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                End If
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar los campos editables.", Me.ToString(), "TerminoValidarCamposEditablesOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarReceptoresUsuarioDerivados(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.ReceptoresBusqueda))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaReceptoresUsuarioDerivados = lo.Entities.ToList
                    If lo.UserState = OPCION_NUEVO Then
                        If Not IsNothing(_OrdenOYDPLUSSelected) Then
                            OrdenAnteriorOYDPLUS = Nothing
                            ObtenerValoresOrdenAnterior(_OrdenOYDPLUSSelected, OrdenAnteriorOYDPLUS)
                        End If

                        logNuevoRegistro = True
                        logEditarRegistro = False
                        logDuplicarRegistro = False
                        logPlantillaRegistro = False
                        logCancelarRegistro = False

                        CargarReceptoresUsuarioOYDPLUS(OPCION_NUEVO, OPCION_NUEVO)
                    End If
                Else
                    mostrarMensaje("No hay ningun comercial configurado para el usuario en el sistema de Derivados.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
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

    Private Sub TerminoConsultarReceptoresUsuario(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.UserState <> "INICIO" Then
                        If logEditarRegistro Or logNuevoRegistro Then
                            If lo.UserState = OPCION_NUEVO Then
                                Dim objListaReceptoresOYD As New List(Of OYDPLUSUtilidades.tblReceptoresUsuario)

                                For Each li In lo.Entities.ToList
                                    If _ListaReceptoresUsuarioDerivados.Where(Function(i) i.CodigoComercial = li.CodigoReceptor).Count > 0 Then
                                        objListaReceptoresOYD.Add(li)
                                    End If
                                Next

                                ListaReceptoresUsuario = objListaReceptoresOYD
                                NuevaOrden()
                            ElseIf lo.UserState = OPCION_EDITAR Then
                                ListaReceptoresUsuario = lo.Entities.ToList
                                CargarCombosSistemaExternoReceptorOYDPLUS(_OrdenAnteriorOYDPLUS.NroDocumento, _OrdenAnteriorOYDPLUS.CodigoOYD, _OrdenAnteriorOYDPLUS.Receptor, OPCION_EDITAR)
                            ElseIf lo.UserState = OPCION_DUPLICAR Then
                                ListaReceptoresUsuario = lo.Entities.ToList
                                CargarCombosSistemaExternoReceptorOYDPLUS(_OrdenAnteriorOYDPLUS.NroDocumento, _OrdenAnteriorOYDPLUS.CodigoOYD, _OrdenAnteriorOYDPLUS.Receptor, OPCION_DUPLICAR)
                            Else
                                ListaReceptoresUsuario = lo.Entities.ToList
                                ObtenerValoresDefectoOYDPLUS(OPCION_RECEPTOR, _OrdenOYDPLUSSelected)
                            End If
                        End If
                    Else
                        ListaReceptoresUsuario = lo.Entities.ToList
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

    Private Sub TerminoConsultarReceptoresOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.ReceptoresOrden))
        Try
            If lo.HasError = False Then
                ListaReceptoresOrdenes = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores de la orden.", Me.ToString(), "TerminoConsultarReceptoresOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores de la orden.", Me.ToString(), "TerminoConsultarReceptoresOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub


    Private Sub PosicionarControlValidaciones(ByVal plogOrdenOriginal As Boolean, ByVal pobjOrdenSelected As OyDPLUSOrdenesDerivados.OrdenDerivados, ByVal pobjViewOrdenOYDPLUS As OrdenesPLUSDerivadosView)
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

    Private Sub TerminoValidarIngresoOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.tblRespuestaValidaciones))
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
            If dcProxy1.OrdenDerivados.Contains(_OrdenOYDPLUSSelected) Then
                dcProxy1.OrdenDerivados.Add(_OrdenOYDPLUSSelected)
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
    Private Sub TerminoAnularOrden(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.tblRespuestaValidaciones))
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

                                dcProxy.Load(dcProxy.OYDPLUSDerivados_AnularOrdenQuery(_OrdenOYDPLUSSelected.ID, _OrdenOYDPLUSSelected.NroOrden, objResultado.Observaciones, Program.Usuario, Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoAnularOrden, "ANULAR")
                            Else
                                IsBusy = True
                                dcProxy1.OYDPLUS_CancelarOrdenOYDPLUS(_OrdenOYDPLUSSelected.ID, "ORDENES", Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarAnularRegistro, String.Empty)
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

    Private Sub TerminoConsultarPlantillas(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.OrdenDerivados))
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

    Private Sub TerminoEliminarPlantillas(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.tblRespuestaValidaciones))
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
                If pobjDiccionario.ContainsKey(pstrArray(i)) Then
                    pobjDiccionario(pstrArray(i)) = Nothing
                End If
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.", _
                                 Me.ToString(), "PrRemoverValoresDic", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Function ExtraerListaPorCategoria(pobjDiccionario As Dictionary(Of String, ObservableCollection(Of OYDPLUSUtilidades.CombosReceptor)), pstrTopico As String, pstrCategoria As String) As ObservableCollection(Of OYDPLUSUtilidades.CombosReceptor)
        ExtraerListaPorCategoria = New ObservableCollection(Of OYDPLUSUtilidades.CombosReceptor)
        Try
            If pobjDiccionario.ContainsKey(pstrTopico) Then
                Dim objRetorno = From item In pobjDiccionario(pstrTopico)
                                 Select New OYDPLUSUtilidades.CombosReceptor With {.ID = item.ID, _
                                                                                   .Retorno = item.Retorno, _
                                                                                   .Descripcion = item.Descripcion, _
                                                                                   .Categoria = pstrCategoria, _
                                                                                   .Prioridad = item.Prioridad}
                If objRetorno.Count > 0 Then
                    ExtraerListaPorCategoria = objRetorno
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.", _
                                 Me.ToString(), "ExtraerListaPorCategoria", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return New ObservableCollection(Of OYDPLUSUtilidades.CombosReceptor)
        End Try
    End Function

    Private Function ExtraerListaPorCategoriaExterno(pobjDiccionario As List(Of OYDPLUSUtilidades.ItemCombosSistemaExterno), pstrTopico As String, pstrCategoria As String, Optional pintIDDependiencia1 As Nullable(Of Integer) = Nothing, Optional pintIDDependencia2 As Nullable(Of Integer) = Nothing) As List(Of OYDPLUSUtilidades.CombosReceptor)
        ExtraerListaPorCategoriaExterno = New List(Of OYDPLUSUtilidades.CombosReceptor)
        Try
            If Not IsNothing(pobjDiccionario) Then
                If pobjDiccionario.Where(Function(i) i.Categoria = pstrTopico).Count > 0 Then
                    Dim objRetorno As New List(Of OYDPLUSUtilidades.CombosReceptor)
                    For Each li In pobjDiccionario.Where(Function(i) i.Categoria = pstrTopico)
                        If IsNothing(pintIDDependiencia1) And IsNothing(pintIDDependencia2) Then
                            objRetorno.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID, _
                                                                                      .Retorno = li.Retorno, _
                                                                                      .Descripcion = li.Descripcion, _
                                                                                      .Categoria = pstrCategoria, _
                                                                                      .Prioridad = li.Prioridad})
                        ElseIf Not IsNothing(pintIDDependiencia1) And Not IsNothing(pintIDDependencia2) Then
                            If li.IDDependencia1 = pintIDDependiencia1 And li.IDDependencia2 = pintIDDependencia2 Then
                                objRetorno.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID, _
                                                                                          .Retorno = li.Retorno, _
                                                                                          .Descripcion = li.Descripcion, _
                                                                                          .Categoria = pstrCategoria, _
                                                                                          .Prioridad = li.Prioridad})
                            End If
                        ElseIf Not IsNothing(pintIDDependiencia1) And IsNothing(pintIDDependencia2) Then
                            If li.IDDependencia1 = pintIDDependiencia1 Then
                                objRetorno.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID, _
                                                                                          .Retorno = li.Retorno, _
                                                                                          .Descripcion = li.Descripcion, _
                                                                                          .Categoria = pstrCategoria, _
                                                                                          .Prioridad = li.Prioridad})
                            End If
                        ElseIf IsNothing(pintIDDependiencia1) And Not IsNothing(pintIDDependencia2) Then
                            If li.IDDependencia2 = pintIDDependencia2 Then
                                objRetorno.Add(New OYDPLUSUtilidades.CombosReceptor With {.ID = li.ID, _
                                                                                          .Retorno = li.Retorno, _
                                                                                          .Descripcion = li.Descripcion, _
                                                                                          .Categoria = pstrCategoria, _
                                                                                          .Prioridad = li.Prioridad})
                            End If
                        End If

                    Next

                    If objRetorno.Count > 0 Then
                        ExtraerListaPorCategoriaExterno = objRetorno
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.", _
                                 Me.ToString(), "ExtraerListaPorCategoria", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return New List(Of OYDPLUSUtilidades.CombosReceptor)
        End Try
    End Function

    Private Sub ObtenerValorPorDefectorEnDiccionario(ByVal pobjDiccionario As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)), ByVal pstrTopico As String, ByRef pobjValorActualizar As Object, Optional ByVal plogObtenerID As Boolean = True)
        Try
            If Not IsNothing(pobjDiccionario) Then
                If pobjDiccionario.ContainsKey(pstrTopico) Then
                    If Not IsNothing(pobjDiccionario(pstrTopico)) Then
                        If pobjDiccionario(pstrTopico).Count = 1 Then
                            If plogObtenerID Then
                                pobjValorActualizar = pobjDiccionario(pstrTopico).First.ID
                            Else
                                pobjValorActualizar = pobjDiccionario(pstrTopico).First.Retorno
                            End If
                        ElseIf pobjDiccionario(pstrTopico).Where(Function(i) i.Prioridad = 0).Count > 0 Then
                            If plogObtenerID Then
                                pobjValorActualizar = pobjDiccionario(pstrTopico).Where(Function(i) i.Prioridad = 0).First.ID
                            Else
                                pobjValorActualizar = pobjDiccionario(pstrTopico).Where(Function(i) i.Prioridad = 0).First.Retorno
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener el valor por defecto.", _
                                 Me.ToString(), "ObtenerValorPorDefectorEnDiccionario", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub ObtenerValorPorDefectorEnLista(ByVal pstrListaCombo As List(Of OYDPLUSUtilidades.CombosReceptor), ByRef pobjValorActualizar As Object, Optional ByVal plogObtenerID As Boolean = True)
        Try
            If Not IsNothing(pstrListaCombo) Then
                If pstrListaCombo.Count = 1 Then
                    If plogObtenerID Then
                        pobjValorActualizar = pstrListaCombo.First.ID
                    Else
                        pobjValorActualizar = pstrListaCombo.First.Retorno
                    End If
                ElseIf pstrListaCombo.Where(Function(i) i.Prioridad = 0).Count > 0 Then
                    If plogObtenerID Then
                        pobjValorActualizar = pstrListaCombo.Where(Function(i) i.Prioridad = 0).First.ID
                    Else
                        pobjValorActualizar = pstrListaCombo.Where(Function(i) i.Prioridad = 0).First.Retorno
                    End If

                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener el valor por defecto.", _
                                 Me.ToString(), "ObtenerValorPorDefectorEnDiccionario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


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
                        ComitenteSeleccionadoOYDPLUS = Nothing
                        If BorrarCliente Then
                            BorrarCliente = False
                        End If
                        BorrarCliente = True

                        ValidarHabilitarNegocio(_OrdenOYDPLUSSelected)

                        If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.Receptor) Then
                            LimpiarControlesOYDPLUS(OPCION_RECEPTOR)
                            CargarTipoNegocioReceptor(OPCION_RECEPTOR, _OrdenOYDPLUSSelected.Receptor, _Modulo, OPCION_RECEPTOR)
                        Else
                            ConfiguracionReceptor = Nothing
                            LimpiarControlesOYDPLUS(OPCION_RECEPTOR)
                        End If
                    End If
                Case "codigooyd"
                    If logEditarRegistro Or logNuevoRegistro Then
                        ValidarHabilitarNegocio(_OrdenOYDPLUSSelected)
                    End If
                Case "nrodocumento"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not String.IsNullOrEmpty(_OrdenOYDPLUSSelected.NroDocumento) Then
                            ValidarConsultaCombosCliente(_OrdenOYDPLUSSelected, "COMBOSRECEPTOR")
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
                            ValidarHabilitarNegocio(_OrdenOYDPLUSSelected)
                            If logRealizarConsultaPropiedades Then
                                ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_TIPONEGOCIO)
                            Else
                                IsBusy = False
                            End If
                        End If
                    End If
                Case "idcuenta"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.IDCuenta) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_SUBCUENTA)
                        End If
                    End If
                Case "tipoinstruccion"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.TipoInstruccion) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_TIPOOPCION)
                        End If
                    End If
                Case "tiporegularospread"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.TipoRegularOSpread) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_INSTRUMENTO)
                        End If
                    End If
                Case "instrumento"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.Instrumento) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_VENCIMIENTOINICIAL)
                        End If
                    End If
                Case "vencimientoinicial"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.VencimientoInicial) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_VENCIMIENTOFINAL)
                        End If
                    End If
                Case "tipocomision"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If _OrdenOYDPLUSSelected.TipoComision = 1 Then 'comisión en valor
                            MostrarComision = True
                        Else
                            MostrarComision = False
                        End If
                    End If
                Case "tiporegistro"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.TipoRegistro) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_CONTRAPARTE)
                        End If
                    End If
                Case "canal"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.Canal) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_MEDIOVERIFICABLE)
                        End If
                    End If
                Case "naturaleza"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.Naturaleza) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_TIPOEJECUCION)
                        End If
                    End If
                Case "tipoejecucion"
                    If logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.TipoEjecucion) Then
                            If Not IsNothing(_ListaComboTipoEjecucion) Then
                                If _ListaComboTipoEjecucion.Where(Function(i) i.ID = _OrdenOYDPLUSSelected.TipoEjecucion).Count > 0 Then
                                    If _ListaComboTipoEjecucion.Where(Function(i) i.ID = _OrdenOYDPLUSSelected.TipoEjecucion).First.Retorno = String.Format("{0}-{1}", _OrdenOYDPLUSSelected.Naturaleza, CANTIDADMINIMA) Then
                                        HabilitarCantidadMinima = True
                                    Else
                                        HabilitarCantidadMinima = False
                                        _OrdenOYDPLUSSelected.CantidadMinima = 0
                                    End If
                                End If
                            End If
                        End If
                    End If
                Case "otroinstrumento"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.OtroInstrumento) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_TIPOPRECIO)
                        End If
                    End If
                Case "finalidad"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.Finalidad) Then
                            ObtenerValoresCombos(False, _OrdenOYDPLUSSelected, OPCION_TIPOFINALIDAD)
                        End If
                    End If
                Case "tipocobertura"
                    If logEditarRegistro Or logNuevoRegistro Then
                        If Not IsNothing(_OrdenOYDPLUSSelected.TipoCobertura) Then
                            If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOFINALIDAD_DATOSADICIONALES" And i.ID = _OrdenOYDPLUSSelected.TipoCobertura).Count > 0 Then
                                HabilitarDatosAdicionalesTipoFinalidad = True
                            Else
                                HabilitarDatosAdicionalesTipoFinalidad = False
                                _OrdenOYDPLUSSelected.MontoCubrir = 0
                                _OrdenOYDPLUSSelected.Moneda = Nothing
                            End If
                        End If
                    End If
                Case "giveout"
                    If logNuevoRegistro Then
                        If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_REQUIEREGIVEOUT" And i.ID = _OrdenOYDPLUSSelected.TipoRegistro).Count > 0 Then
                            If _OrdenOYDPLUSSelected.GiveOut Then
                                HabilitarGiveOut = True
                            Else
                                HabilitarGiveOut = False
                            End If
                        Else
                            HabilitarGiveOut = False
                        End If
                    Else
                        If DiccionarioEdicionCamposOYDPLUS.ContainsKey("bitEsGiveOut") Then
                            If DiccionarioEdicionCamposOYDPLUS("bitEsGiveOut") Then
                                If _ListaCombosSistemaExternoCompletos.Where(Function(i) i.Categoria = "TIPOREGISTRO_REQUIEREGIVEOUT" And i.ID = _OrdenOYDPLUSSelected.TipoRegistro).Count > 0 Then
                                    If _OrdenOYDPLUSSelected.GiveOut Then
                                        HabilitarGiveOut = True
                                    Else
                                        HabilitarGiveOut = False
                                    End If
                                Else
                                    HabilitarGiveOut = False
                                End If
                            Else
                                HabilitarGiveOut = False
                            End If
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

    Private Const TOPICOMENSAJE_RESPUESTADERIVADOS = "OYDPLUS_DERIVADOS_RESPUESTA"
    Private Const TOPICOMENSAJE_AUTORIZACIONES = "OYDPLUS_AUTORIZACIONES"

    Public Overrides Sub LlegoNotificacion(pobjInfoNotificacion As A2.Notificaciones.Cliente.clsNotificacion)
        Try
            If Not IsNothing(pobjInfoNotificacion) Then
                If pobjInfoNotificacion.strTopicos = TOPICOMENSAJE_RESPUESTADERIVADOS Or pobjInfoNotificacion.strTopicos = TOPICOMENSAJE_AUTORIZACIONES Then
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
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.strEstado = String.Empty
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.Notificacion = String.Empty
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NotificacionDescripcion = String.Empty

                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NroOrden = objNotificacion.NroRegistro
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.strEstado = objNotificacion.Estado
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.Notificacion = objNotificacion.EstadoMostrar
                            _ListaOrdenOYDPLUS.Where(Function(i) i.ID = objNotificacion.IDRegistro).First.NotificacionDescripcion = objNotificacion.ListaDescripciones
                        Else
                            objNotificacionGrabacion = objNotificacion
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el mensaje de la notificación.", _
                                Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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
            IsBusy = False
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
            IsBusy = False
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
            IsBusy = False
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

    Private _NroOrden As Nullable(Of Integer)
    <Display(Name:="Nro orden")> _
    Public Property NroOrden() As Nullable(Of Integer)
        Get
            Return _NroOrden
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _NroOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroOrden"))
        End Set
    End Property

    Private _Estado As Nullable(Of Integer)
    <Display(Name:="Estado")> _
    Public Property Estado() As Nullable(Of Integer)
        Get
            Return _Estado
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Estado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Estado"))
        End Set
    End Property

    Private _TipoOperacion As Nullable(Of Integer)
    <Display(Name:="Tipo operación")> _
    Public Property TipoOperacion() As Nullable(Of Integer)
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _FechaInicial As Nullable(Of DateTime)
    Public Property FechaInicial() As Nullable(Of DateTime)
        Get
            Return _FechaInicial
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaInicial"))
        End Set
    End Property

    Private _FechaFinal As Nullable(Of DateTime)
    Public Property FechaFinal() As Nullable(Of DateTime)
        Get
            Return _FechaFinal
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaFinal"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
