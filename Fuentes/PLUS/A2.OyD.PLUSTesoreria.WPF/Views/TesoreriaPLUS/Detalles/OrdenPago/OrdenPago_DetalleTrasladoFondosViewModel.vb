Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSTesoreria
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports A2.Notificaciones.Cliente
Imports A2OYDPLUSUtilidades
Imports OpenRiaServices.DomainServices.Client

Public Class OrdenPago_DetalleTrasladoFondosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"
    Private dcProxy As OYDPLUSTesoreriaDomainContext
    Private dcProxyUtilidades As UtilidadesDomainContext
    Private dcProxyUtilidadesPLUS As OYDPLUSUtilidadesDomainContext

    Public logCambiarPropiedades As Boolean = False

    Private TIPOCONCEPTOCONCOBRO As String = String.Empty
    Private logEsFondosOYD As Boolean = False
    Private strTipoGMF_TesoreriaFondosOYD As String = String.Empty
    Private logRegistroExistente As Boolean = False
    Private logEsFondosUnity As Boolean = False
    Private strMensajeValidacion As String = String.Empty
    Public viewRegistro As OrdenPago_DetalleTrasladoFondosView = Nothing
    Public EsNuevoRegistro As Boolean = False
    Public EsEdicionRegistro As Boolean = False
    Private strConceptoDefecto_Fondos As String = String.Empty
    Private intDiasAplicacionFondosApertura As Integer = 0
    Private intDiasAplicacionFondosAdicion As Integer = 0
    Public HabilitarValor_Inicio As Boolean = False
    Private ConceptoDefectoFondos_Retiro As Nullable(Of Integer) = Nothing
    Private ConceptoDescripcionDefectoFondos_Retiro As String = String.Empty
    Private ConceptoDefectoFondos_Cancelacion As Nullable(Of Integer) = Nothing
    Private ConceptoDescripcionDefectoFondos_Cancelacion As String = String.Empty

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            IsBusy = True
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSTesoreriaDomainContext()
                dcProxyUtilidades = New UtilidadesDomainContext()
                dcProxyUtilidadesPLUS = New OYDPLUSUtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                dcProxyUtilidadesPLUS = New OYDPLUSUtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYDPLUS)))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OYDPLUSTesoreriaDomainContext.IOYDPLUSTesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

        Catch ex As Exception

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "OrdenPago_DetalleTrasladoFondosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

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

    Private _DiccionarioCombosOYDPlusCompleto As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlusCompleto() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlusCompleto
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlusCompleto = value
            MyBase.CambioItem("DiccionarioCombosOYDPlusCompleto")
        End Set
    End Property

    Private _ListaTipoGMF As List(Of OYDPLUSUtilidades.CombosReceptor)
    Public Property ListaTipoGMF() As List(Of OYDPLUSUtilidades.CombosReceptor)
        Get
            Return _ListaTipoGMF
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.CombosReceptor))
            _ListaTipoGMF = value
            MyBase.CambioItem("ListaTipoGMF")
        End Set
    End Property

    Private _IDEncabezado As Integer
    Public Property IDEncabezado() As Integer
        Get
            Return _IDEncabezado
        End Get
        Set(ByVal value As Integer)
            _IDEncabezado = value
            MyBase.CambioItem("IDEncabezado")
        End Set
    End Property

    Private _IDDetalle As Integer
    Public Property IDDetalle() As Integer
        Get
            Return _IDDetalle
        End Get
        Set(ByVal value As Integer)
            _IDDetalle = value
            MyBase.CambioItem("IDDetalle")
        End Set
    End Property

    Private _ConsultarValoresBaseDatos As Boolean
    Public Property ConsultarValoresBaseDatos() As Boolean
        Get
            Return _ConsultarValoresBaseDatos
        End Get
        Set(ByVal value As Boolean)
            _ConsultarValoresBaseDatos = value
            MyBase.CambioItem("ConsultarValoresBaseDatos")
        End Set
    End Property

    Private _MostrarInformacionFondos As Visibility
    Public Property MostrarInformacionFondos() As Visibility
        Get
            Return _MostrarInformacionFondos
        End Get
        Set(ByVal value As Visibility)
            _MostrarInformacionFondos = value
            MyBase.CambioItem("MostrarInformacionFondos")
        End Set
    End Property

    Private _FechaOrden As DateTime
    Public Property FechaOrden() As DateTime
        Get
            Return _FechaOrden
        End Get
        Set(ByVal value As DateTime)
            _FechaOrden = value
            MyBase.CambioItem("FechaOrden")
        End Set
    End Property

    Private _CarteraColectivaFondos As String
    Public Property CarteraColectivaFondos() As String
        Get
            Return _CarteraColectivaFondos
        End Get
        Set(ByVal value As String)
            _CarteraColectivaFondos = value
            MyBase.CambioItem("CarteraColectivaFondos")
        End Set
    End Property

    Private _NroEncargoFondos As String
    Public Property NroEncargoFondos() As String
        Get
            Return _NroEncargoFondos
        End Get
        Set(ByVal value As String)
            _NroEncargoFondos = value
            MyBase.CambioItem("NroEncargoFondos")
        End Set
    End Property

    Private _TipoRetiroFondos As String
    Public Property TipoRetiroFondos() As String
        Get
            Return _TipoRetiroFondos
        End Get
        Set(ByVal value As String)
            _TipoRetiroFondos = value
            MyBase.CambioItem("TipoRetiroFondos")
        End Set
    End Property

    Private _Receptor As String
    Public Property Receptor() As String
        Get
            Return _Receptor
        End Get
        Set(ByVal value As String)
            _Receptor = value
            MyBase.CambioItem("Receptor")
        End Set
    End Property

    Private _IDComitente As String
    Public Property IDComitente() As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            MyBase.CambioItem("IDComitente")
        End Set
    End Property

    Private _NroDocumentoComitente As String
    Public Property NroDocumentoComitente() As String
        Get
            Return _NroDocumentoComitente
        End Get
        Set(ByVal value As String)
            _NroDocumentoComitente = value
            MyBase.CambioItem("NroDocumentoComitente")
        End Set
    End Property

    Private _TipoDocumentoComitente As String
    Public Property TipoDocumentoComitente() As String
        Get
            Return _TipoDocumentoComitente
        End Get
        Set(ByVal value As String)
            _TipoDocumentoComitente = value
            MyBase.CambioItem("TipoDocumentoComitente")
        End Set
    End Property

    Private _DescripcionTipoDocumentoComitente As String
    Public Property DescripcionTipoDocumentoComitente() As String
        Get
            Return _DescripcionTipoDocumentoComitente
        End Get
        Set(ByVal value As String)
            _DescripcionTipoDocumentoComitente = value
            MyBase.CambioItem("DescripcionTipoDocumentoComitente")
        End Set
    End Property

    Private _NombreComitente As String
    Public Property NombreComitente() As String
        Get
            Return _NombreComitente
        End Get
        Set(ByVal value As String)
            _NombreComitente = value
            MyBase.CambioItem("NombreComitente")
        End Set
    End Property

    Private _TipoProducto As String
    Public Property TipoProducto() As String
        Get
            Return _TipoProducto
        End Get
        Set(ByVal value As String)
            _TipoProducto = value
            MyBase.CambioItem("TipoProducto")
        End Set
    End Property

    Private _ValorNetoOrden As Double
    Public Property ValorNetoOrden() As Double
        Get
            Return _ValorNetoOrden
        End Get
        Set(ByVal value As Double)
            _ValorNetoOrden = value
            MyBase.CambioItem("ValorNetoOrden")
        End Set
    End Property

    Private _TieneOrdenEnCero As Boolean
    Public Property TieneOrdenEnCero() As Boolean
        Get
            Return _TieneOrdenEnCero
        End Get
        Set(ByVal value As Boolean)
            _TieneOrdenEnCero = value
            MyBase.CambioItem("TieneOrdenEnCero")
        End Set
    End Property

    Private _ValorOriginalDetalle As Double
    Public Property ValorOriginalDetalle() As Double
        Get
            Return _ValorOriginalDetalle
        End Get
        Set(ByVal value As Double)
            _ValorOriginalDetalle = value
            MyBase.CambioItem("ValorOriginalDetalle")
        End Set
    End Property

    Private _ValorEdicionDetalle As Double
    Public Property ValorEdicionDetalle() As Double
        Get
            Return _ValorEdicionDetalle
        End Get
        Set(ByVal value As Double)
            _ValorEdicionDetalle = value
            MyBase.CambioItem("ValorEdicionDetalle")
        End Set
    End Property

    Private _EsTercero As Boolean
    Public Property EsTercero() As Boolean
        Get
            Return _EsTercero
        End Get
        Set(ByVal value As Boolean)
            _EsTercero = value
            MyBase.CambioItem("EsTercero")
        End Set
    End Property

    Private _HabilitarTipoCliente As String
    Public Property HabilitarTipoCliente() As String
        Get
            Return _HabilitarTipoCliente
        End Get
        Set(ByVal value As String)
            _HabilitarTipoCliente = value
            MyBase.CambioItem("HabilitarTipoCliente")
        End Set
    End Property

    Private _TipoCliente As String
    Public Property TipoCliente() As String
        Get
            Return _TipoCliente
        End Get
        Set(ByVal value As String)
            _TipoCliente = value
            If logCambiarPropiedades Then
                If Not IsNothing(_TipoCliente) Then
                    CodigoOYDDetalle = String.Empty
                    TipoIdentificacion = String.Empty
                    NroDocumento = String.Empty
                    Nombre = String.Empty

                    CargarConceptosPorDefecto()
                    VerificarCobro_GMF()
                End If
            End If
            MyBase.CambioItem("TipoCliente")
        End Set
    End Property

    Private _DescripcionTipoCliente As String
    Public Property DescripcionTipoCliente() As String
        Get
            Return _DescripcionTipoCliente
        End Get
        Set(ByVal value As String)
            _DescripcionTipoCliente = value
            MyBase.CambioItem("_DescripcionTipoCliente")
        End Set
    End Property

    Private _HabilitarCodigoOYDDetalle As Boolean
    Public Property HabilitarCodigoOYDDetalle() As Boolean
        Get
            Return _HabilitarCodigoOYDDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodigoOYDDetalle = value
            MyBase.CambioItem("HabilitarCodigoOYDDetalle")
        End Set
    End Property

    Private _CodigoOYDDetalle As String
    Public Property CodigoOYDDetalle() As String
        Get
            Return _CodigoOYDDetalle
        End Get
        Set(ByVal value As String)
            _CodigoOYDDetalle = value
            If logCambiarPropiedades Then
                If logEsFondosOYD Then
                    logCambiarPropiedades = False
                    TipoAccionFondosOYD = Nothing
                    CarteraColectivaOYD = Nothing
                    EncargoCarteraOYD = Nothing
                    DescripcionEncargoFondosOYD = Nothing
                    logCambiarPropiedades = True
                    ConsultarCarterasColectivasClientes(True, "CODIGOOYDDETALLE")
                Else
                    logCambiarPropiedades = False
                    TipoAccionFondosExterno = Nothing
                    CarteraColectivaExterno = Nothing
                    EncargoCarteraExterno = Nothing
                    logCambiarPropiedades = True
                    ConsultarCarterasColectivasClientes(False, "CODIGOOYDDETALLE")
                End If
            End If
            MyBase.CambioItem("CodigoOYDDetalle")
        End Set
    End Property

    Private _TipoIdentificacion As String
    Public Property TipoIdentificacion() As String
        Get
            Return _TipoIdentificacion
        End Get
        Set(ByVal value As String)
            _TipoIdentificacion = value
            MyBase.CambioItem("TipoIdentificacion")
        End Set
    End Property

    Private _DescripcionTipoIdentificacion As String
    Public Property DescripcionTipoIdentificacion() As String
        Get
            Return _DescripcionTipoIdentificacion
        End Get
        Set(ByVal value As String)
            _DescripcionTipoIdentificacion = value
            MyBase.CambioItem("DescripcionTipoIdentificacion")
        End Set
    End Property

    Private _NroDocumento As String
    Public Property NroDocumento() As String
        Get
            Return _NroDocumento
        End Get
        Set(ByVal value As String)
            _NroDocumento = value
            MyBase.CambioItem("NroDocumento")
        End Set
    End Property

    Private _Nombre As String
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            MyBase.CambioItem("Nombre")
        End Set
    End Property

    Private _ListaCarterasColectivasCompleta As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
    Public Property ListaCarterasColectivasCompleta() As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
        Get
            Return _ListaCarterasColectivasCompleta
        End Get
        Set(ByVal value As List(Of OyDPLUSTesoreria.CarterasColectivasClientes))
            _ListaCarterasColectivasCompleta = value
            MyBase.CambioItem("ListaCarterasColectivasCompleta")
        End Set
    End Property

    Private _ListaCarterasColectivasOYD As List(Of CarterasColectivas)
    Public Property ListaCarterasColectivasOYD() As List(Of CarterasColectivas)
        Get
            Return _ListaCarterasColectivasOYD
        End Get
        Set(ByVal value As List(Of CarterasColectivas))
            _ListaCarterasColectivasOYD = value
            MyBase.CambioItem("ListaCarterasColectivasOYD")
        End Set
    End Property

    Private _ListaCarterasColectivasExterno As List(Of CarterasColectivas)
    Public Property ListaCarterasColectivasExterno() As List(Of CarterasColectivas)
        Get
            Return _ListaCarterasColectivasExterno
        End Get
        Set(ByVal value As List(Of CarterasColectivas))
            _ListaCarterasColectivasExterno = value
            MyBase.CambioItem("ListaCarterasColectivasExterno")
        End Set
    End Property

    Private _ListaEncargosCarteraColectivaOYD As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
    Public Property ListaEncargosCarteraColectivaOYD() As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
        Get
            Return _ListaEncargosCarteraColectivaOYD
        End Get
        Set(ByVal value As List(Of OyDPLUSTesoreria.CarterasColectivasClientes))
            _ListaEncargosCarteraColectivaOYD = value
            MyBase.CambioItem("ListaEncargosCarteraColectivaOYD")
        End Set
    End Property

    Private _ListaEncargosCarteraColectivaExterno As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
    Public Property ListaEncargosCarteraColectivaExterno() As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
        Get
            Return _ListaEncargosCarteraColectivaExterno
        End Get
        Set(ByVal value As List(Of OyDPLUSTesoreria.CarterasColectivasClientes))
            _ListaEncargosCarteraColectivaExterno = value
            MyBase.CambioItem("ListaEncargosCarteraColectivaExterno")
        End Set
    End Property

    Private _TipoAccionRetorno As String
    Public Property TipoAccionRetorno() As String
        Get
            Return _TipoAccionRetorno
        End Get
        Set(ByVal value As String)
            _TipoAccionRetorno = value
            MyBase.CambioItem("TipoAccionRetorno")
        End Set
    End Property

    Private _CarteraColectivaRetorno As String
    Public Property CarteraColectivaRetorno() As String
        Get
            Return _CarteraColectivaRetorno
        End Get
        Set(ByVal value As String)
            _CarteraColectivaRetorno = value
            MyBase.CambioItem("CarteraColectivaRetorno")
        End Set
    End Property

    Private _DescripcionCarteraColectivaRetorno As String
    Public Property DescripcionCarteraColectivaRetorno() As String
        Get
            Return _DescripcionCarteraColectivaRetorno
        End Get
        Set(ByVal value As String)
            _DescripcionCarteraColectivaRetorno = value
            MyBase.CambioItem("DescripcionCarteraColectivaRetorno")
        End Set
    End Property

    Private _EncargoCarteraRetorno As String
    Public Property EncargoCarteraRetorno() As String
        Get
            Return _EncargoCarteraRetorno
        End Get
        Set(ByVal value As String)
            _EncargoCarteraRetorno = value
            MyBase.CambioItem("EncargoCarteraRetorno")
        End Set
    End Property

    Private _DescripcionEncargoRetorno As String
    Public Property DescripcionEncargoRetorno() As String
        Get
            Return _DescripcionEncargoRetorno
        End Get
        Set(ByVal value As String)
            _DescripcionEncargoRetorno = value
            MyBase.CambioItem("DescripcionEncargoRetorno")
        End Set
    End Property

    Private _ListaTiposAccionOYD As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTiposAccionOYD() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTiposAccionOYD
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTiposAccionOYD = value
            MyBase.CambioItem("ListaTiposAccionOYD")
        End Set
    End Property

    Private _MostrarControlesFondosOYD As Visibility
    Public Property MostrarControlesFondosOYD() As Visibility
        Get
            Return _MostrarControlesFondosOYD
        End Get
        Set(ByVal value As Visibility)
            _MostrarControlesFondosOYD = value
            MyBase.CambioItem("MostrarControlesFondosOYD")
        End Set
    End Property

    Private _HabilitarControlesFondosOYD As String
    Public Property HabilitarControlesFondosOYD() As String
        Get
            Return _HabilitarControlesFondosOYD
        End Get
        Set(ByVal value As String)
            _HabilitarControlesFondosOYD = value
            MyBase.CambioItem("HabilitarControlesFondosOYD")
        End Set
    End Property

    Private _CarteraColectivaOYD As String
    Public Property CarteraColectivaOYD() As String
        Get
            Return _CarteraColectivaOYD
        End Get
        Set(ByVal value As String)
            _CarteraColectivaOYD = value
            If logCambiarPropiedades Then
                If Not String.IsNullOrEmpty(_CarteraColectivaOYD) Then
                    ConsultarCarterasColectivasClientes(False, "CARTERACOLECTIVAOYD")
                    VerificarRestriccionesTipoCartera("CARTERACOLECTIVAOYD")
                    consultarCombosEspecificosFondo(_CarteraColectivaOYD, "CARTERACOLECTIVAOYD")
                Else
                    LimpiarDatosCarteraOYD(False)
                End If
            End If
            MyBase.CambioItem("CarteraColectivaOYD")
        End Set
    End Property

    Private _TipoAccionFondosOYD As String
    Public Property TipoAccionFondosOYD() As String
        Get
            Return _TipoAccionFondosOYD
        End Get
        Set(ByVal value As String)
            _TipoAccionFondosOYD = value
            If logCambiarPropiedades Then
                If _TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
                    EncargoCarteraOYD = Nothing
                Else
                    DescripcionEncargoFondosOYD = Nothing
                End If

                CalcularFechaAplicacionOYD()
                HabilitarControles()
            End If
            MyBase.CambioItem("TipoAccionFondosOYD")
        End Set
    End Property

    Private _EncargoCarteraOYD As String
    Public Property EncargoCarteraOYD() As String
        Get
            Return _EncargoCarteraOYD
        End Get
        Set(ByVal value As String)
            _EncargoCarteraOYD = value
            If logCambiarPropiedades Then
                VerificarRestriccionesTipoCartera("ENCARGOCARTERAOYD")
            End If
            MyBase.CambioItem("EncargoCarteraOYD")
        End Set
    End Property

    Private _DescripcionEncargoFondosOYD As String
    Public Property DescripcionEncargoFondosOYD() As String
        Get
            Return _DescripcionEncargoFondosOYD
        End Get
        Set(ByVal value As String)
            _DescripcionEncargoFondosOYD = value
            MyBase.CambioItem("DescripcionEncargoFondosOYD")
        End Set
    End Property

    Private _MostrarNroEncargoOYD As Visibility
    Public Property MostrarNroEncargoOYD() As Visibility
        Get
            Return _MostrarNroEncargoOYD
        End Get
        Set(ByVal value As Visibility)
            _MostrarNroEncargoOYD = value
            MyBase.CambioItem("MostrarNroEncargoOYD")
        End Set
    End Property

    Private _MostrarDescripcionEncargoOYD As Visibility
    Public Property MostrarDescripcionEncargoOYD() As Visibility
        Get
            Return _MostrarDescripcionEncargoOYD
        End Get
        Set(ByVal value As Visibility)
            _MostrarDescripcionEncargoOYD = value
            MyBase.CambioItem("MostrarDescripcionEncargoOYD")
        End Set
    End Property

    Private _MostrarControlesFondosExterno As Visibility
    Public Property MostrarControlesFondosExterno() As Visibility
        Get
            Return _MostrarControlesFondosExterno
        End Get
        Set(ByVal value As Visibility)
            _MostrarControlesFondosExterno = value
            MyBase.CambioItem("MostrarControlesFondosExterno")
        End Set
    End Property

    Private _HabilitarControlesFondosExterno As String
    Public Property HabilitarControlesFondosExterno() As String
        Get
            Return _HabilitarControlesFondosExterno
        End Get
        Set(ByVal value As String)
            _HabilitarControlesFondosExterno = value
            MyBase.CambioItem("HabilitarControlesFondosExterno")
        End Set
    End Property

    Private _TipoAccionFondosExterno As String
    Public Property TipoAccionFondosExterno() As String
        Get
            Return _TipoAccionFondosExterno
        End Get
        Set(ByVal value As String)
            _TipoAccionFondosExterno = value
            If logCambiarPropiedades Then
                If Not IsNothing(_TipoAccionFondosExterno) Then
                    CargarCarteraColectivaExterno()
                    CalcularFechaAplicacionExterno()
                    HabilitarControles()
                End If
            End If
            MyBase.CambioItem("TipoAccionFondosExterno")
        End Set
    End Property

    Private _CarteraColectivaExterno As String
    Public Property CarteraColectivaExterno() As String
        Get
            Return _CarteraColectivaExterno
        End Get
        Set(ByVal value As String)
            _CarteraColectivaExterno = value
            If logCambiarPropiedades Then
                If Not String.IsNullOrEmpty(_CarteraColectivaExterno) Then
                    If _TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_ADICION Then
                        ConsultarCarterasColectivasClientes(False, "CARTERACOLECTIVAEXTERNO")
                    Else
                        EncargoCarteraExterno = Nothing
                    End If

                    VerificarRestriccionesTipoCartera("CARTERACOLECTIVAFONDOSEXTERNO")
                End If
            End If
            MyBase.CambioItem("CarteraColectivaExterno")
        End Set
    End Property

    Private _HabilitarEncargoFondosExterno As Boolean
    Public Property HabilitarEncargoFondosExterno() As Boolean
        Get
            Return _HabilitarEncargoFondosExterno
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncargoFondosExterno = value
            MyBase.CambioItem("HabilitarEncargoFondosExterno")
        End Set
    End Property

    Private _EncargoCarteraExterno As String
    Public Property EncargoCarteraExterno() As String
        Get
            Return _EncargoCarteraExterno
        End Get
        Set(ByVal value As String)
            _EncargoCarteraExterno = value
            If logCambiarPropiedades Then
                VerificarRestriccionesTipoCartera("ENCARGOCARTERAEXTERNO")
            End If
            MyBase.CambioItem("EncargoCarteraExterno")
        End Set
    End Property

    Private _FechaAplicacion As DateTime
    Public Property FechaAplicacion() As DateTime
        Get
            Return _FechaAplicacion
        End Get
        Set(ByVal value As DateTime)
            _FechaAplicacion = value
            MyBase.CambioItem("FechaAplicacion")
        End Set
    End Property

    Private _HabilitarConcepto As Boolean
    Public Property HabilitarConcepto() As Boolean
        Get
            Return _HabilitarConcepto
        End Get
        Set(ByVal value As Boolean)
            _HabilitarConcepto = value
            MyBase.CambioItem("HabilitarConcepto")
        End Set
    End Property

    Private _IDConcepto As Nullable(Of Integer)
    Public Property IDConcepto() As Nullable(Of Integer)
        Get
            Return _IDConcepto
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDConcepto = value
            DescripcionConcepto = ConcatenarConcepto(_IDConcepto)
            If logCambiarPropiedades Then
                VerificarCobro_GMF()
            End If
            MyBase.CambioItem("IDConcepto")
        End Set
    End Property

    Private _DescripcionConcepto As String
    Public Property DescripcionConcepto() As String
        Get
            Return _DescripcionConcepto
        End Get
        Set(ByVal value As String)
            _DescripcionConcepto = value
            MyBase.CambioItem("DescripcionConcepto")
        End Set
    End Property

    Private _HabilitarDetalleConcepto As Boolean
    Public Property HabilitarDetalleConcepto() As Boolean
        Get
            Return _HabilitarDetalleConcepto
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDetalleConcepto = value
            MyBase.CambioItem("HabilitarDetalleConcepto")
        End Set
    End Property

    Private _DetalleConcepto As String
    Public Property DetalleConcepto() As String
        Get
            Return _DetalleConcepto
        End Get
        Set(ByVal value As String)
            _DetalleConcepto = value
            MyBase.CambioItem("DetalleConcepto")
        End Set
    End Property

    Private _HabilitarTipoGMF As Boolean = False
    Public Property HabilitarTipoGMF() As Boolean
        Get
            Return _HabilitarTipoGMF
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoGMF = value
            MyBase.CambioItem("HabilitarTipoGMF")
        End Set
    End Property

    Private _TipoGMF As String
    Public Property TipoGMF() As String
        Get
            Return _TipoGMF
        End Get
        Set(ByVal value As String)
            _TipoGMF = value
            If logCambiarPropiedades Then
                CalcularValorRegistro()
            End If
            MyBase.CambioItem("TipoGMF")
        End Set
    End Property

    Private _DescripcionTipoGMF As String
    Public Property DescripcionTipoGMF() As String
        Get
            Return _DescripcionTipoGMF
        End Get
        Set(ByVal value As String)
            _DescripcionTipoGMF = value
            MyBase.CambioItem("DescripcionTipoGMF")
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

    Private _ValorGenerar As Double
    Public Property ValorGenerar() As Double
        Get
            Return _ValorGenerar
        End Get
        Set(ByVal value As Double)
            _ValorGenerar = value
            CalcularValorRegistro()
            MyBase.CambioItem("ValorGenerar")
        End Set
    End Property

    Private _ValorGMF As Double
    Public Property ValorGMF() As Double
        Get
            Return _ValorGMF
        End Get
        Set(ByVal value As Double)
            _ValorGMF = value
            MyBase.CambioItem("ValorGMF")
        End Set
    End Property

    Private _ValorNeto As Double
    Public Property ValorNeto() As Double
        Get
            Return _ValorNeto
        End Get
        Set(ByVal value As Double)
            _ValorNeto = value
            MyBase.CambioItem("ValorNeto")
        End Set
    End Property

    Private _MostrarConsultaSaldo As Visibility
    Public Property MostrarConsultaSaldo() As Visibility
        Get
            Return _MostrarConsultaSaldo
        End Get
        Set(ByVal value As Visibility)
            _MostrarConsultaSaldo = value
            MyBase.CambioItem("MostrarConsultaSaldo")
        End Set
    End Property

    Private _MostrarConsultaLiquidaciones As Visibility
    Public Property MostrarConsultaLiquidaciones() As Visibility
        Get
            Return _MostrarConsultaLiquidaciones
        End Get
        Set(ByVal value As Visibility)
            _MostrarConsultaLiquidaciones = value
            MyBase.CambioItem("MostrarConsultaLiquidaciones")
        End Set
    End Property

    Private _HabilitarGuardarYSalir As Boolean = True
    Public Property HabilitarGuardarYSalir() As Boolean
        Get
            Return _HabilitarGuardarYSalir
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGuardarYSalir = value
            MyBase.CambioItem("HabilitarGuardarYSalir")
        End Set
    End Property

    Private _HabilitarGuardarContinuar As Boolean = True
    Public Property HabilitarGuardarContinuar() As Boolean
        Get
            Return _HabilitarGuardarContinuar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGuardarContinuar = value
            MyBase.CambioItem("HabilitarGuardarContinuar")
        End Set
    End Property

    Private _LiquidacionesSeleccionadas As String
    Public Property LiquidacionesSeleccionadas() As String
        Get
            Return _LiquidacionesSeleccionadas
        End Get
        Set(ByVal value As String)
            _LiquidacionesSeleccionadas = value
        End Set
    End Property

    Private _SaldoConsultado As Double
    Public Property SaldoConsultado() As Double
        Get
            Return _SaldoConsultado
        End Get
        Set(ByVal value As Double)
            _SaldoConsultado = value
        End Set
    End Property

    ''' <summary>
    ''' JAPC20200424-CC20200364:Tesorero puede editar GMF
    ''' </summary>
    Private _HabilitarTipoGMFTesorero As Boolean = False
    Public Property HabilitarTipoGMFTesorero() As Boolean
        Get
            Return _HabilitarTipoGMFTesorero
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoGMFTesorero = value
            MyBase.CambioItem("HabilitarTipoGMF")
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub PrepararNuevoRegistro()
        Try
            TipoCliente = Nothing
            CodigoOYDDetalle = Nothing
            TipoIdentificacion = Nothing
            NroDocumento = Nothing
            Nombre = Nothing
            TipoAccionFondosExterno = Nothing
            TipoAccionFondosOYD = Nothing
            CarteraColectivaExterno = Nothing
            CarteraColectivaOYD = Nothing
            EncargoCarteraExterno = Nothing
            EncargoCarteraOYD = Nothing
            TipoAccionRetorno = Nothing
            CarteraColectivaRetorno = Nothing
            EncargoCarteraRetorno = Nothing

            IDConcepto = Nothing
            DescripcionConcepto = Nothing
            DetalleConcepto = Nothing
            TipoGMF = Nothing
            DescripcionTipoGMF = Nothing
            ValorGenerar = 0
            ValorGMF = 0
            ValorNeto = 0

            If TipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
                If logEsFondosOYD Then
                    CargarConceptosPorDefecto()
                Else
                    If Not String.IsNullOrEmpty(strConceptoDefecto_Fondos) Then
                        DetalleConcepto = strConceptoDefecto_Fondos
                    End If
                End If
            Else
                DetalleConcepto = String.Empty
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar el nuevo registro.",
                                 Me.ToString(), "PrepararNuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub PrepararEdicionRegistro()
        Try
            If logEsFondosOYD Then
                CarteraColectivaRetorno = CarteraColectivaOYD
                TipoAccionRetorno = TipoAccionFondosOYD
                EncargoCarteraRetorno = EncargoCarteraOYD
                If ListaCarterasColectivasOYD.Where(Function(i) i.ID = CarteraColectivaOYD).Count > 0 Then
                    DescripcionCarteraColectivaRetorno = ListaCarterasColectivasOYD.Where(Function(i) i.ID = CarteraColectivaOYD).First.Descripcion
                End If
                DescripcionEncargoRetorno = DescripcionEncargoFondosOYD
            Else
                TipoAccionRetorno = TipoAccionFondosExterno
                CarteraColectivaRetorno = CarteraColectivaExterno
                EncargoCarteraRetorno = EncargoCarteraExterno
                If ListaCarterasColectivasExterno.Where(Function(i) i.ID = CarteraColectivaExterno).Count > 0 Then
                    DescripcionCarteraColectivaRetorno = ListaCarterasColectivasExterno.Where(Function(i) i.ID = CarteraColectivaExterno).First.Descripcion
                End If
                DescripcionEncargoRetorno = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar el nuevo registro.",
                                 Me.ToString(), "PrepararEdicionRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Function ConcatenarConcepto(ByVal intIDConcepto As Nullable(Of Integer)) As String
        Try
            Dim strResultado As String = String.Empty

            If Not IsNothing(DiccionarioCombosOYDPlusCompleto) And Not IsNothing(intIDConcepto) Then
                If DiccionarioCombosOYDPlusCompleto.ContainsKey("CONCEPTOS") Then
                    If DiccionarioCombosOYDPlusCompleto("CONCEPTOS").Where(Function(i) i.Retorno = intIDConcepto.ToString).Count > 0 Then
                        strResultado = String.Format("{0} - {1}",
                                                     DiccionarioCombosOYDPlusCompleto("CONCEPTOS").Where(Function(i) i.Retorno = intIDConcepto.ToString).First.Retorno,
                                                     DiccionarioCombosOYDPlusCompleto("CONCEPTOS").Where(Function(i) i.Retorno = intIDConcepto.ToString).First.Descripcion
                                                     )
                    Else
                        strResultado = String.Empty
                    End If
                Else
                    strResultado = String.Empty
                End If
            Else
                strResultado = String.Empty
            End If

            Return strResultado
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concatenar los detalles.",
                                Me.ToString(), "ConcatenarConcepto", Application.Current.ToString(), Program.Maquina, ex)
            Return String.Empty
        End Try
    End Function

    Public Function ConcatenarDetalle(ByVal strDescripcionConcepto As String, ByVal pstrDetalle As String, Optional ByVal pintNroEncargo As String = "", Optional ByVal pstrFormaPago As String = "") As String
        Try
            Dim strResultado As String = String.Empty

            If (Not String.IsNullOrEmpty(pintNroEncargo)) And logEsFondosOYD And (TipoProducto = GSTR_FONDOS_TIPOPRODUCTO Or pstrFormaPago = "CC") Then
                strResultado = String.Format("{0} ENCARGO NRO {1}-({2})", strDescripcionConcepto, pintNroEncargo, pstrDetalle)
            ElseIf Not String.IsNullOrEmpty(strDescripcionConcepto) And Not String.IsNullOrEmpty(pstrDetalle) Then
                If SaldoConsultado > 0 Then
                    strResultado = String.Format("{0}-{1}", strDescripcionConcepto, pstrDetalle)

                Else
                    If Not String.IsNullOrEmpty(LiquidacionesSeleccionadas) Then
                        strResultado = String.Format("{0}-[{1}]-({2})", strDescripcionConcepto, LiquidacionesSeleccionadas, pstrDetalle)
                    Else
                        strResultado = String.Format("{0}-({1})", strDescripcionConcepto, pstrDetalle)
                    End If
                End If
            Else
                If Not String.IsNullOrEmpty(strDescripcionConcepto) Then

                    If SaldoConsultado > 0 Then
                        strResultado = String.Format("{0}", strDescripcionConcepto)
                    Else
                        If Not String.IsNullOrEmpty(LiquidacionesSeleccionadas) Then
                            strResultado = String.Format("{0}-[{1}]", strDescripcionConcepto, LiquidacionesSeleccionadas)
                        Else
                            strResultado = String.Format("{0}", strDescripcionConcepto)
                        End If
                    End If
                Else
                    If SaldoConsultado > 0 Then
                        strResultado = String.Format("{0}", pstrDetalle)
                    Else
                        If Not String.IsNullOrEmpty(LiquidacionesSeleccionadas) Then
                            strResultado = String.Format("{0}-[{1}]", pstrDetalle, LiquidacionesSeleccionadas)
                        Else
                            strResultado = String.Format("{0}", pstrDetalle)
                        End If
                    End If
                End If
            End If

            Return strResultado
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concatenar los detalles.",
                                Me.ToString(), "ConcatenarDetalle", Application.Current.ToString(), Program.Maquina, ex)
            Return String.Empty
        End Try
    End Function

    Public Sub CargarCombosOYDPLUS(ByVal pstrUserState As String)

        Try
            IsBusy = True

            If Not IsNothing(dcProxyUtilidadesPLUS.CombosReceptors) Then
                dcProxyUtilidadesPLUS.CombosReceptors.Clear()
            End If

            dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarCombosReceptorQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYDCompleta, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.",
                                 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Async Function ConsultarDatosRegistro() As Task(Of Boolean)
        Try
            Dim objRespuesta As InvokeResult(Of List(Of TesoreriaOrdenesDetalle)) = Nothing
            objRespuesta = Await dcProxy.OrdenPago_ConsultarDetalleAsync(_IDEncabezado, _IDDetalle, Program.Usuario, Program.UsuarioWindows, Program.HashConexion)
            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    If objRespuesta.Value.Count > 0 Then
                        logCambiarPropiedades = False

                        CarteraColectivaFondos = objRespuesta.Value.First.strCarteraColectivaFondosEncabezado
                        NroEncargoFondos = objRespuesta.Value.First.intNroEncargoFondosEncabezado
                        TipoProducto = objRespuesta.Value.First.strTipoProductoEncabezado
                        TipoRetiroFondos = objRespuesta.Value.First.strTipoRetiroFondosEncabezado

                        TipoCliente = objRespuesta.Value.First.strIDTipoCliente
                        CodigoOYDDetalle = objRespuesta.Value.First.strCodigoOyD
                        TipoIdentificacion = objRespuesta.Value.First.strTipoDocumento
                        NroDocumento = objRespuesta.Value.First.strNroDocumento
                        Nombre = objRespuesta.Value.First.strNombre

                        TipoAccionRetorno = objRespuesta.Value.First.strTipoAccionFondos
                        CarteraColectivaRetorno = objRespuesta.Value.First.strNombreCarteraColectiva
                        EncargoCarteraRetorno = objRespuesta.Value.First.lngNroEncargo
                        DescripcionEncargoRetorno = objRespuesta.Value.First.strDescripcionEncargoFondos
                        FechaAplicacion = objRespuesta.Value.First.dtmFechaAplicacion

                        IDConcepto = objRespuesta.Value.First.lngIDConcepto

                        If IsNothing(IDConcepto) Or IDConcepto = 0 Then
                            If objRespuesta.Value.First.strDetalleConcepto.Contains("(") Then
                                DetalleConcepto = RetornarValorDetalle(objRespuesta.Value.First.strDetalleConcepto, "(", ")")
                            Else
                                DetalleConcepto = objRespuesta.Value.First.strDetalleConcepto
                            End If
                        Else
                            DetalleConcepto = RetornarValorDetalle(objRespuesta.Value.First.strDetalleConcepto, "(", ")")
                        End If

                        If String.IsNullOrEmpty(objRespuesta.Value.First.strTipoCobroGMF) Then
                            TipoGMF = GSTR_GMF_NOAPLICA
                        Else
                            TipoGMF = objRespuesta.Value.First.strTipoCobroGMF
                        End If

                        If objRespuesta.Value.First.strTipoCobroGMF = GSTR_GMF_ENCIMA Then
                            ValorGenerar = objRespuesta.Value.First.curValor
                            ValorGMF = objRespuesta.Value.First.curValorGMF
                            ValorNeto = objRespuesta.Value.First.curValorNeto
                        ElseIf objRespuesta.Value.First.strTipoCobroGMF = GSTR_GMF_DEBAJO Then
                            ValorGenerar = objRespuesta.Value.First.curValorNeto
                            ValorGMF = objRespuesta.Value.First.curValorGMF
                            ValorNeto = objRespuesta.Value.First.curValor
                        Else
                            ValorGenerar = objRespuesta.Value.First.curValor
                            ValorGMF = 0
                            ValorNeto = objRespuesta.Value.First.curValor
                        End If
                        logCambiarPropiedades = True
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Ocurrio un error al momento de ejecutar la acción.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Ocurrio un error al momento de ejecutar la acción.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Ocurrio un error al momento de ejecutar la acción.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
            End If

            Return True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la información de base de datos.",
                                 Me.ToString(), "ConsultarDatosRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Función que retorna el valor de un texto que este contenido dentro de los dos caracteres enviados como parametros.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 12 de marzo del 2013
    ''' </summary>
    ''' <param name="pstrDetalle"></param>
    ''' <param name="pstrCaracterInicial"></param>
    ''' <param name="pstrCaracterFinal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RetornarValorDetalle(ByVal pstrDetalle As String, ByVal pstrCaracterInicial As String, ByVal pstrCaracterFinal As String) As String
        Try
            Dim strResultado As String = String.Empty
            Dim strExpresionBusqueda As String = String.Format("\{0}\S*\s*\S*\{1}", pstrCaracterInicial, pstrCaracterFinal)
            Dim regexp As New Regex(strExpresionBusqueda)

            pstrDetalle = pstrDetalle.Replace(" ", "*_*")

            Dim m = regexp.Match(pstrDetalle)

            strResultado = m.Groups(0).Value
            strResultado = strResultado.Replace(pstrCaracterInicial, String.Empty)
            strResultado = strResultado.Replace(pstrCaracterFinal, String.Empty)
            strResultado = strResultado.Replace("*_*", " ")

            Return strResultado
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retornar el valor del detalle.",
                                Me.ToString(), "RetornarValorDetalle", Application.Current.ToString(), Program.Maquina, ex)
            Return String.Empty
        End Try
    End Function

    Public Sub ObtenerLiquidacionesSeleccionadas(ByVal pobjValores As A2OYDPLUSUtilidades.RetornoValoresLiquidacion)
        Try
            Dim dblValorLiquidacionesSugerido As Double = 0

            If Not IsNothing(pobjValores) Then
                If Not String.IsNullOrEmpty(pobjValores.strLiquidaciones) Then
                    dblValorLiquidacionesSugerido = pobjValores.dblValorLiquidacionesSeleccionadas
                    LiquidacionesSeleccionadas = pobjValores.strLiquidaciones

                    LiquidacionesSeleccionadas = String.Format("Liquidaciones: {0} - Valor total:{1:C2}", LiquidacionesSeleccionadas, dblValorLiquidacionesSugerido)

                    ValorGenerar = dblValorLiquidacionesSugerido

                    'Sí se consulta las liquidaciones la variable de la consulta del saldo queda en cero
                    SaldoConsultado = 0
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener las liquidaciones seleccionas.",
                                 Me.ToString(), "ObtenerLiquidacionesSeleccionadas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub VerificarCobro_GMF(Optional ByVal plogSoloHabilitarTipo As Boolean = False)
        Try
            Dim logDebeCobrarGMF As Boolean = False

            'VERIFICA EL TIPO DE CLIENTE
            If _TipoCliente = GSTR_CLIENTE Then
                logDebeCobrarGMF = False
                'JAPC20200627_C-20200433-01_Se limpiar valor GMF cuando cambia el tipo de cliente de tercero a cliente
                ValorGMF = 0
            Else
                logDebeCobrarGMF = True
            End If

            If logDebeCobrarGMF = False Then
                If Not String.IsNullOrEmpty(TIPOCONCEPTOCONCOBRO) Then
                    Dim conceptosCobroGMF = Split(TIPOCONCEPTOCONCOBRO, "|")
                    For Each concepto In conceptosCobroGMF
                        If Not String.IsNullOrEmpty(concepto) Then
                            If _IDConcepto = Integer.Parse(concepto) Then
                                logDebeCobrarGMF = True
                            End If
                        End If
                    Next
                End If
            End If

            If logDebeCobrarGMF Then
                If logEsFondosOYD And Not String.IsNullOrEmpty(strTipoGMF_TesoreriaFondosOYD) Then
                    'JAPC20200627_C-20200433-01_Se habilita GMF cuando se debe cobrar 
                    HabilitarTipoGMF = True
                    If plogSoloHabilitarTipo = False Then
                        TipoGMF = strTipoGMF_TesoreriaFondosOYD
                    End If

                Else
                    HabilitarTipoGMF = True
                    If plogSoloHabilitarTipo = False Then
                        If Not IsNothing(DiccionarioCombosOYDPlus) Then
                            If DiccionarioCombosOYDPlus.ContainsKey("TIPOGMF") Then
                                If DiccionarioCombosOYDPlus("TIPOGMF").Count = 1 Then
                                    TipoGMF = DiccionarioCombosOYDPlus("TIPOGMF").FirstOrDefault.Retorno
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                HabilitarTipoGMF = False
                If plogSoloHabilitarTipo = False Then
                    TipoGMF = Nothing
                End If
            End If

            'JAPC20200424-CC20200364:Tesorero puede editar GMF
            If HabilitarTipoGMFTesorero Then
                HabilitarTipoGMF = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar el cobro de GMF.", Me.ToString(), "VerificarCobro_Calculo_GMF", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Sub CargarConceptosPorDefecto()
        Try
            If logEsFondosOYD Then
                If TipoRetiroFondos = GSTR_FONDOS_TIPOACCION_CANCELACION Then
                    If Not IsNothing(ConceptoDefectoFondos_Cancelacion) Then
                        IDConcepto = ConceptoDefectoFondos_Cancelacion
                        DescripcionConcepto = ConceptoDescripcionDefectoFondos_Cancelacion
                    Else
                        IDConcepto = Nothing
                        DescripcionConcepto = String.Empty
                    End If
                Else
                    If Not IsNothing(ConceptoDefectoFondos_Retiro) Then
                        IDConcepto = ConceptoDefectoFondos_Retiro
                        DescripcionConcepto = ConceptoDescripcionDefectoFondos_Retiro
                    Else
                        IDConcepto = Nothing
                        DescripcionConcepto = String.Empty
                    End If
                End If
            Else
                If DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "NOTAS").Count = 1 _
                And DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "TODOS").Count = 0 Then 'Cuando solo hay 1 concepto de Nota registrado al receptor
                    IDConcepto = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "NOTAS").FirstOrDefault.IntId
                    DescripcionConcepto = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "NOTAS").FirstOrDefault.Descripcion
                ElseIf DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "TODOS").Count = 1 _
                    And DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "NOTAS").Count = 0 Then 'Cuando solo hay 1 concepto de Nota TODOS registrado al receptor
                    IDConcepto = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "TODOS").FirstOrDefault.IntId
                    DescripcionConcepto = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "TODOS").FirstOrDefault.Descripcion
                Else
                    IDConcepto = Nothing
                    DescripcionConcepto = String.Empty
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el concepto por defecto",
                                                 Me.ToString(), "CargarConceptosPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarSaldos(ByVal pstrOpcion As String)
        Try
            If Not IsNothing(IDComitente) Then
                IsBusy = True
                If Not IsNothing(dcProxyUtilidadesPLUS.tblSaldosClientes) Then
                    dcProxyUtilidadesPLUS.tblSaldosClientes.Clear()
                End If
                dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarSaldoClienteQuery(IDComitente, Program.Usuario, TipoProducto, CarteraColectivaFondos, NroEncargoFondos, Program.HashConexion), AddressOf TerminoConsultarSaldoCliente, pstrOpcion)
            Else
                ValorGenerar = 0
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el saldo del cliente.",
                                 Me.ToString(), "ConsultarSaldoCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub HabilitarControles()
        Try
            If ConsultarValoresBaseDatos Then
                MostrarConsultaSaldo = Visibility.Collapsed
                MostrarConsultaLiquidaciones = Visibility.Collapsed
                HabilitarTipoCliente = False
                HabilitarCodigoOYDDetalle = False
                HabilitarControlesFondosExterno = False
                HabilitarControlesFondosOYD = False
                HabilitarValor = False
                HabilitarEncargoFondosExterno = False

                If logEsFondosOYD Then
                    MostrarControlesFondosOYD = Visibility.Visible
                    MostrarControlesFondosExterno = Visibility.Collapsed

                    If _TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
                        MostrarNroEncargoOYD = Visibility.Collapsed
                        MostrarDescripcionEncargoOYD = Visibility.Visible
                    Else
                        MostrarNroEncargoOYD = Visibility.Visible
                        MostrarDescripcionEncargoOYD = Visibility.Collapsed
                    End If
                Else
                    MostrarControlesFondosOYD = Visibility.Collapsed
                    MostrarControlesFondosExterno = Visibility.Visible
                    MostrarNroEncargoOYD = Visibility.Collapsed
                    MostrarDescripcionEncargoOYD = Visibility.Collapsed
                End If

                If logEsFondosOYD And TipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
                    HabilitarConcepto = False
                Else
                    HabilitarConcepto = True
                End If
                HabilitarDetalleConcepto = True
                HabilitarTipoGMF = True
            Else
                MostrarConsultaSaldo = Visibility.Visible
                MostrarConsultaLiquidaciones = Visibility.Visible
                HabilitarTipoCliente = True
                HabilitarCodigoOYDDetalle = True

                If logEsFondosOYD Then
                    HabilitarControlesFondosOYD = True
                    HabilitarControlesFondosExterno = False
                    HabilitarEncargoFondosExterno = False
                    MostrarControlesFondosOYD = Visibility.Visible
                    MostrarControlesFondosExterno = Visibility.Collapsed

                    If _TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
                        HabilitarEncargoFondosExterno = False
                        MostrarNroEncargoOYD = Visibility.Collapsed
                        MostrarDescripcionEncargoOYD = Visibility.Visible
                    Else
                        HabilitarEncargoFondosExterno = True
                        MostrarNroEncargoOYD = Visibility.Visible
                        MostrarDescripcionEncargoOYD = Visibility.Collapsed
                    End If
                Else
                    HabilitarControlesFondosOYD = False
                    HabilitarControlesFondosExterno = True
                    MostrarControlesFondosOYD = Visibility.Collapsed
                    MostrarControlesFondosExterno = Visibility.Visible
                    MostrarNroEncargoOYD = Visibility.Collapsed
                    MostrarDescripcionEncargoOYD = Visibility.Collapsed

                    If _TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
                        HabilitarEncargoFondosExterno = False
                    Else
                        HabilitarEncargoFondosExterno = True
                    End If
                End If

                If logEsFondosOYD And TipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
                    HabilitarConcepto = False
                Else
                    If logEsFondosUnity Then
                        HabilitarConcepto = True
                    Else
                        HabilitarConcepto = False
                    End If
                End If

                HabilitarDetalleConcepto = True

                VerificarCobro_GMF(True)

                If HabilitarValor_Inicio Then
                    HabilitarValor = True
                Else
                    HabilitarValor = False
                End If
            End If

            If TipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
                MostrarInformacionFondos = Visibility.Visible
            Else
                MostrarInformacionFondos = Visibility.Collapsed
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los controles.",
                                 Me.ToString(), "VerificarHabilitarControles", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CalcularValorRegistro()
        Try
            If Not String.IsNullOrEmpty(_TipoGMF) Then
                If _TipoGMF = GSTR_GMF_ENCIMA Then
                    ValorGMF = dblGMF_E * _ValorGenerar
                    ValorNeto = _ValorGenerar + ValorGMF
                    If dblGMF_E = 0 Then
                        MostrarMensajeGMF(GSTR_GMF_ENCIMA)
                    End If
                ElseIf _TipoGMF = GSTR_GMF_DEBAJO Then
                    ValorGMF = dblGMF_D * _ValorGenerar
                    ValorNeto = _ValorGenerar - ValorGMF
                    If dblGMF_D = 0 Then
                        MostrarMensajeGMF(GSTR_GMF_DEBAJO)
                    End If
                ElseIf _TipoGMF = GSTR_GMF_NOAPLICA Then
                    ValorGMF = 0
                    ValorNeto = _ValorGenerar
                End If
            Else
                ValorNeto = _ValorGenerar
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular el valor del registro.",
                                 Me.ToString(), "CalcularValorRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub MostrarMensajeGMF(pstrTipoGMF As String)
        If pstrTipoGMF = GSTR_GMF_ENCIMA Then
            A2Utilidades.Mensajes.mostrarMensaje("No se puede calcular el valor del GMF por encima ya que no esta configurado correctamente, o este tiene un valor igual a CERO", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        ElseIf pstrTipoGMF = GSTR_GMF_DEBAJO Then
            A2Utilidades.Mensajes.mostrarMensaje("No se puede calcular el valor del GMF por debajo ya que no esta configurado correctamente, o este tiene un valor igual a CERO", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
    End Sub

    Public Function ValidarRegistro() As Boolean
        Try
            Dim logTieneError As Boolean
            strMensajeValidacion = String.Empty

            If IsNothing(_TipoCliente) Then
                strMensajeValidacion = String.Format("{0}{1} - Tipo Cliente: Si es Tercero o No lo es.", strMensajeValidacion, vbCrLf)
                logTieneError = True
            End If
            If IsNothing(Nombre) Or String.IsNullOrEmpty(Nombre) Then
                strMensajeValidacion = String.Format("{0}{1} - Nombre Beneficiario.", strMensajeValidacion, vbCrLf)
                logTieneError = True
            Else
                Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(Nombre, clsExpresiones.TipoExpresion.Caracteres2)
                If Not IsNothing(objValidacionExpresion) Then
                    If objValidacionExpresion.TextoValido = False Then
                        strMensajeValidacion = String.Format("{0}{1} - Nombre Beneficiario: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            End If
            If IsNothing(TipoIdentificacion) Or String.IsNullOrEmpty(TipoIdentificacion) Then
                strMensajeValidacion = String.Format("{0}{1} - Tipo Documento Beneficiario.", strMensajeValidacion, vbCrLf)
                logTieneError = True
            End If
            If IsNothing(NroDocumento) Or String.IsNullOrEmpty(NroDocumento) Then
                strMensajeValidacion = String.Format("{0}{1} - Nro Documento Beneficiario.", strMensajeValidacion, vbCrLf)
                logTieneError = True
            Else
                Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(NroDocumento, clsExpresiones.TipoExpresion.Caracteres)
                If Not IsNothing(objValidacionExpresion) Then
                    If objValidacionExpresion.TextoValido = False Then
                        strMensajeValidacion = String.Format("{0}{1} - Nro Documento Beneficiario: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            End If
            If (IsNothing(IDConcepto) Or IDConcepto <= 0) And
                (IsNothing(DescripcionConcepto) Or String.IsNullOrEmpty(DescripcionConcepto)) Then
                strMensajeValidacion = String.Format("{0}{1} - Seleccionar un concepto de la lista ó ingrese detalle concepto.", strMensajeValidacion, vbCrLf)
                logTieneError = True
            End If
            If Not String.IsNullOrEmpty(DescripcionConcepto) Then
                Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(DescripcionConcepto, clsExpresiones.TipoExpresion.Caracteres)
                If Not IsNothing(objValidacionExpresion) Then
                    If objValidacionExpresion.TextoValido = False Then
                        strMensajeValidacion = String.Format("{0}{1} - Descripcion Concepto: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            End If

            If logEsFondosOYD Then
                If String.IsNullOrEmpty(CarteraColectivaOYD) Then
                    strMensajeValidacion = String.Format("{0}{1} - Seleccionar una Cartera colectiva.", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                If String.IsNullOrEmpty(TipoAccionFondosOYD) Then
                    strMensajeValidacion = String.Format("{0}{1} - Seleccionar una Tipo acción.", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                If TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_ADICION Then
                    If String.IsNullOrEmpty(EncargoCarteraOYD) Then
                        strMensajeValidacion = String.Format("{0}{1} - Número Encargo.", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            Else
                If String.IsNullOrEmpty(TipoAccionFondosExterno) Then
                    strMensajeValidacion = String.Format("{0}{1} - Seleccionar una Tipo acción.", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                If String.IsNullOrEmpty(CarteraColectivaExterno) Then
                    strMensajeValidacion = String.Format("{0}{1} - Seleccionar una Cartera colectiva.", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                If TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_ADICION Then
                    If String.IsNullOrEmpty(EncargoCarteraExterno) Then
                        strMensajeValidacion = String.Format("{0}{1} - Número Encargo.", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            End If

            If logEsFondosOYD Then
                If Not String.IsNullOrEmpty(CarteraColectivaOYD) And TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_ADICION And Not String.IsNullOrEmpty(EncargoCarteraOYD) Then
                    If CarteraColectivaFondos = CarteraColectivaOYD And NroEncargoFondos = EncargoCarteraOYD Then
                        strMensajeValidacion = String.Format("{0}{1} - Número Encargo y el nombre de la cartera no pueden ser igual al del encabezado", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            Else
                If Not String.IsNullOrEmpty(CarteraColectivaExterno) And TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_ADICION And Not String.IsNullOrEmpty(EncargoCarteraExterno) Then
                    If CarteraColectivaFondos = CarteraColectivaExterno And NroEncargoFondos = EncargoCarteraExterno Then
                        strMensajeValidacion = String.Format("{0}{1} - Número Encargo y el nombre de la cartera no pueden ser igual al del encabezado", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            End If

            If HabilitarTipoGMF Then
                If String.IsNullOrEmpty(TipoGMF) Then
                    strMensajeValidacion = String.Format("{0}{1} - Tipo GMF.", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
            End If

            If TipoProducto = GSTR_FONDOS_TIPOPRODUCTO And TipoRetiroFondos = GSTR_FONDOS_TIPOACCION_CANCELACION Then
                Dim logValidarValorEnCero As Boolean = True

                If logEsFondosOYD Then
                    If ValorGenerar > 0 Or ValorNeto > 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Cuando la orden es de Fondos y es una cancelación no puede existir detalles con valor.", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                        logValidarValorEnCero = False
                    End If
                End If

                If logValidarValorEnCero Then
                    If TieneOrdenEnCero Then
                        If (IsNothing(ValorGenerar) Or ValorGenerar <= 0) Or
                            (IsNothing(ValorNeto) Or ValorNeto <= 0) Then
                            strMensajeValidacion = String.Format("{0}{1} - Cuando la orden es de Fondos y es una cancelación solo puede existir 1 detalle con el valor en cero.", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    End If
                End If
            ElseIf HabilitarValor Then
                If IsNothing(ValorGenerar) Or ValorGenerar <= 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Valor Generar.", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If

                If IsNothing(ValorNeto) Or ValorNeto <= 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Valor Neto.", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
            End If

            If logTieneError Then
                mostrarMensaje("Para guardar el Registro es necesario completar los siguientes datos con sus valores correspondientes:" & vbCrLf & strMensajeValidacion, "Ordenes de Pago - Carteras Colectivas.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else
                strMensajeValidacion = String.Empty
                Return True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Validar Campos de Cheque",
                                 Me.ToString(), "ValidarCamposDiligenciadosCheque", Application.Current.ToString(), Program.Maquina, ex)

            IsBusy = False
            Return False
        End Try
    End Function

    Public Async Function GuardarRegistro() As Task(Of Boolean)
        Try
            IsBusy = True

            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                DescripcionTipoCliente = ObtenerDescripcionEnDiccionario("TIPOCLIENTE", _TipoCliente)
                DescripcionTipoIdentificacion = ObtenerDescripcionEnDiccionario("TIPOID", _TipoIdentificacion)

                If String.IsNullOrEmpty(TipoGMF) Then
                    DescripcionTipoGMF = String.Empty
                Else
                    If ConsultarValoresBaseDatos Then
                        DescripcionTipoGMF = ObtenerDescripcionEnDiccionario("TIPOGMF_TESORERO", _TipoGMF)
                    Else
                        DescripcionTipoGMF = ObtenerDescripcionEnDiccionario("TIPOGMF", _TipoGMF)
                    End If
                End If
            End If

            If _TipoCliente = Program.Tipo_Cliente_Cliente Then
                EsTercero = False
            Else
                EsTercero = True
            End If

            If ValidarRegistro() Then

                If logEsFondosOYD Then
                    CarteraColectivaRetorno = CarteraColectivaOYD
                    TipoAccionRetorno = TipoAccionFondosOYD
                    EncargoCarteraRetorno = EncargoCarteraOYD
                    If ListaCarterasColectivasOYD.Where(Function(i) i.ID = CarteraColectivaOYD).Count > 0 Then
                        DescripcionCarteraColectivaRetorno = ListaCarterasColectivasOYD.Where(Function(i) i.ID = CarteraColectivaOYD).First.Descripcion
                    End If
                    DescripcionEncargoRetorno = DescripcionEncargoFondosOYD
                Else
                    TipoAccionRetorno = TipoAccionFondosExterno
                    CarteraColectivaRetorno = CarteraColectivaExterno
                    EncargoCarteraRetorno = EncargoCarteraExterno
                    If ListaCarterasColectivasExterno.Where(Function(i) i.ID = CarteraColectivaExterno).Count > 0 Then
                        DescripcionCarteraColectivaRetorno = ListaCarterasColectivasExterno.Where(Function(i) i.ID = CarteraColectivaExterno).First.Descripcion
                    End If
                    DescripcionEncargoRetorno = String.Empty
                End If

                If ConsultarValoresBaseDatos Then
                    Dim strDetalleConcepto = ConcatenarDetalle(DescripcionConcepto, DetalleConcepto, NroEncargoFondos)
                    Dim strTipoGMF As String = String.Empty
                    If _TipoGMF = GSTR_GMF_DEBAJO Or _TipoGMF = GSTR_GMF_ENCIMA Then
                        strTipoGMF = _TipoGMF
                    End If

                    Dim objRespuesta As InvokeResult(Of List(Of tblRespuestaValidaciones)) = Nothing
                    'JAPC20200627_C-20200433-01: Se ajusta guardar detalle tesorero con iff cuando el GMF es por debajo el valor bruto sera el neto y el neto el bruto si es por encima sera lo contrario bruto sera bruto y neto neto esto a acorde a como se guarda en ordenes de pago y evitar errores en extractos
                    objRespuesta = Await dcProxy.OrdenPago_GuardarDetalle_TesoreroAsync(_IDEncabezado, _IDDetalle, "TRASLADOFONDOS", Nothing, Nothing, strDetalleConcepto, strTipoGMF, IIf(strTipoGMF = "D", _ValorNeto, _ValorGenerar), _ValorGMF, IIf(strTipoGMF = "D", _ValorGenerar, _ValorNeto), Program.Usuario, Program.UsuarioWindows, Program.HashConexion, _IDConcepto)
                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            If objRespuesta.Value.Count > 0 Then
                                If objRespuesta.Value.First.Exitoso Then
                                    IsBusy = False
                                    Return True
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje(objRespuesta.Value.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    IsBusy = False
                                    Return False
                                End If
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("Ocurrio un error al momento de ejecutar la acción.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Ocurrio un error al momento de ejecutar la acción.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Ocurrio un error al momento de ejecutar la acción.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If
                End If

                IsBusy = False
                Return True
            Else
                IsBusy = False
                Return False
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar guardar el registro.",
                                 Me.ToString(), "GuardarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Public Sub CancelarEdicionRegistro()
        Try
            If EsNuevoRegistro = False Then
                If IDDetalle > 0 Then
                    dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(IDDetalle, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar cancelar la edición del registro.",
                                 Me.ToString(), "CancelarEdicionRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ObtenerDescripcionEnDiccionario(ByVal pstrTopico As String, ByVal pstrIDItem As String) As String
        Dim strRetorno As String = String.Empty
        If DiccionarioCombosOYDPlus.ContainsKey(pstrTopico) Then
            If DiccionarioCombosOYDPlus(pstrTopico).Where(Function(i) i.Retorno = pstrIDItem).Count > 0 Then
                strRetorno = DiccionarioCombosOYDPlus(pstrTopico).Where(Function(i) i.Retorno = pstrIDItem).First.Descripcion
            End If
        End If
        Return strRetorno
    End Function

    Public Sub ConsultarCarterasColectivasClientes(ByVal plogConsultarTodasCarteras As Boolean, ByVal pstrUserState As String)
        Try
            If Not String.IsNullOrEmpty(_CodigoOYDDetalle) Then
                dcProxy.CarterasColectivasClientes.Clear()
                dcProxy.Load(dcProxy.ConsultarCarterasColectivasClienteQuery(_CodigoOYDDetalle, plogConsultarTodasCarteras, Program.Usuario, Program.HashConexion, "DETALLE_CARTERAS"), AddressOf TerminoConsultarCarterasColectivasClientes, pstrUserState)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar cancelar la edición del registro.",
                                 Me.ToString(), "CancelarEdicionRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub VerificarRestriccionesTipoCartera(ByVal pstrUserState As String)
        Try
            If Not String.IsNullOrEmpty(_CarteraColectivaExterno) _
                And Not String.IsNullOrEmpty(_EncargoCarteraExterno) _
                And Not String.IsNullOrEmpty(_TipoAccionFondosExterno) Then
                dcProxy.VerificarRestriccionesTipoCartera(_TipoAccionFondosExterno,
                                                          IDComitente,
                                                          CarteraColectivaExterno,
                                                          EncargoCarteraExterno,
                                                          Program.Usuario,
                                                          FechaAplicacion, Program.HashConexion,
                                                          AddressOf TerminoVerificarRestriccionesTipoCartera,
                                                          pstrUserState)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la existencia de los tipos de cartera.", Me.ToString(), "VerificarRestriccionesTipoCartera", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub consultarCombosEspecificosFondo(ByVal pstrCarteraColectiva As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(dcProxyUtilidades.BuscadorOrdenantes) Then
                dcProxyUtilidades.BuscadorOrdenantes.Clear()
            End If

            If Not IsNothing(dcProxyUtilidades.ItemCombos) Then
                dcProxyUtilidades.ItemCombos.Clear()
            End If

            dcProxyUtilidades.Load(dcProxyUtilidades.cargarCombosCondicionalQuery("COMBOS_DEPENDIENTES_CARTERA", pstrCarteraColectiva, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCombosEspecificosCartera, pstrUserState)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los combos especificos.", Me.ToString(), "consultarCombosEspecificosFondo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LimpiarDatosCarteraOYD(ByVal plogLimpiarListaCartera As Boolean)
        Try
            If plogLimpiarListaCartera Then
                ListaCarterasColectivasCompleta = Nothing
                ListaCarterasColectivasOYD = Nothing
                CarteraColectivaOYD = Nothing
            End If

            ListaEncargosCarteraColectivaOYD = Nothing
            ListaTiposAccionOYD = Nothing
            TipoAccionFondosOYD = Nothing
            EncargoCarteraOYD = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar las carteras colectivas.", Me.ToString(), "TerminoConsultarCarterasColectivasClientes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CargarCarteraColectivaExterno()
        Try
            Dim objListaNueva As New List(Of CarterasColectivas)

            If _TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
                If Not IsNothing(DiccionarioCombosOYDPlus) Then
                    If DiccionarioCombosOYDPlus.ContainsKey("CARTERACOLECTIVAS") Then
                        For Each li In DiccionarioCombosOYDPlus("CARTERACOLECTIVAS")
                            objListaNueva.Add(New CarterasColectivas With {.ID = li.Retorno, .Descripcion = li.Descripcion})
                        Next
                    End If
                End If

                ListaCarterasColectivasExterno = objListaNueva

                If ConsultarValoresBaseDatos = False Then
                    If ListaCarterasColectivasExterno.Count = 1 Then
                        CarteraColectivaExterno = ListaCarterasColectivasExterno.First.ID
                    End If
                End If
            Else
                If Not IsNothing(_ListaCarterasColectivasCompleta) Then
                    For Each li In _ListaCarterasColectivasCompleta
                        If objListaNueva.Where(Function(i) i.ID = li.CarteraColectiva).Count = 0 Then
                            objListaNueva.Add(New CarterasColectivas With {.ID = li.CarteraColectiva, .Descripcion = li.NombreCarteraColectiva})
                        End If
                    Next
                    ListaCarterasColectivasExterno = objListaNueva

                    If ConsultarValoresBaseDatos = False Then
                        If ListaCarterasColectivasExterno.Count = 1 Then
                            CarteraColectivaExterno = ListaCarterasColectivasExterno.First.ID
                        End If
                    End If
                End If
            End If

            ListaEncargosCarteraColectivaExterno = Nothing
            EncargoCarteraExterno = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar las carteras colectivas.", Me.ToString(), "CargarCarteraColectivaExterno", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CalcularFechaAplicacionExterno()
        Try
            If ConsultarValoresBaseDatos = False Then
                If _TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_ADICION Then
                    FechaAplicacion = DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, FechaOrden)
                ElseIf _TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
                    FechaAplicacion = DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, FechaOrden)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al calcular la fecha de aplicación externo.", Me.ToString(), "CalcularFechaAplicacionExterno", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CalcularFechaAplicacionOYD()
        Try
            If ConsultarValoresBaseDatos = False Then
                If _TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_ADICION Then
                    FechaAplicacion = DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, FechaOrden)
                ElseIf _TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
                    FechaAplicacion = DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, FechaOrden)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al calcular la fecha de aplicación.", Me.ToString(), "CalcularFechaAplicacionOYD", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarCombosOYDCompleta(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCombosOYDPlusCompleto = Nothing
                    DiccionarioCombosOYDPlusCompleto = objDiccionarioCompleto

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("OYDPLUS_CONCEPTOSCONFIGURADOSCOBROGMF") Then
                        If DiccionarioCombosOYDPlusCompleto("OYDPLUS_CONCEPTOSCONFIGURADOSCOBROGMF").Count > 0 Then
                            TIPOCONCEPTOCONCOBRO = DiccionarioCombosOYDPlusCompleto("OYDPLUS_CONCEPTOSCONFIGURADOSCOBROGMF").First.Retorno
                        End If
                    End If

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("CF_UTILIZAPASIVA_A2") Then
                        If DiccionarioCombosOYDPlusCompleto("CF_UTILIZAPASIVA_A2").Count > 0 Then
                            If DiccionarioCombosOYDPlusCompleto("CF_UTILIZAPASIVA_A2").First.Retorno = "SI" Then
                                logEsFondosOYD = True
                            Else
                                logEsFondosOYD = False
                            End If
                        End If
                    End If

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("TIPOGMF_FONDOSOYD") Then
                        If DiccionarioCombosOYDPlusCompleto("TIPOGMF_FONDOSOYD").Count > 0 Then
                            strTipoGMF_TesoreriaFondosOYD = DiccionarioCombosOYDPlusCompleto("TIPOGMF_FONDOSOYD").First.Retorno
                        End If
                    End If

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("A2_UTILIZAUNITY") Then
                        If DiccionarioCombosOYDPlusCompleto("A2_UTILIZAUNITY").Count > 0 Then
                            If DiccionarioCombosOYDPlusCompleto("A2_UTILIZAUNITY").First.Retorno = "SI" Then
                                logEsFondosUnity = True
                            Else
                                logEsFondosUnity = False
                            End If
                        End If
                    End If

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("CONCEPTODEFECTO_ORDENGIRO_FONDOS") Then
                        If DiccionarioCombosOYDPlusCompleto("CONCEPTODEFECTO_ORDENGIRO_FONDOS").Count > 0 Then
                            strConceptoDefecto_Fondos = DiccionarioCombosOYDPlusCompleto("CONCEPTODEFECTO_ORDENGIRO_FONDOS").First.Retorno
                        End If
                    End If

                    If ConsultarValoresBaseDatos Then
                        If DiccionarioCombosOYDPlusCompleto.ContainsKey("TIPOGMF_TESORERO") Then
                            ListaTipoGMF = DiccionarioCombosOYDPlusCompleto("TIPOGMF_TESORERO")
                        End If
                    Else
                        If DiccionarioCombosOYDPlusCompleto.ContainsKey("TIPOGMF") Then
                            ListaTipoGMF = DiccionarioCombosOYDPlusCompleto("TIPOGMF")
                        End If
                    End If

                    If Not IsNothing(dcProxyUtilidadesPLUS.CombosReceptors) Then
                        dcProxyUtilidadesPLUS.CombosReceptors.Clear()
                    End If

                    dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarCombosReceptorQuery(Receptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYDReceptor, lo.UserState)
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

    Private Sub TerminoConsultarCombosOYDReceptor(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCombosOYDPlus = Nothing
                    DiccionarioCombosOYDPlus = objDiccionarioCompleto

                    consultarCombosEspecificosFondo(CarteraColectivaFondos, "INICIO")
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

    Private Sub TerminoTraerCombosEspecificosCartera(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If lo.UserState = "INICIO" Then

                    If lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_RETIRO").Count > 0 Then
                        ConceptoDefectoFondos_Retiro = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_RETIRO").First.intID
                        ConceptoDescripcionDefectoFondos_Retiro = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_RETIRO").First.Descripcion
                    Else
                        ConceptoDefectoFondos_Retiro = Nothing
                        ConceptoDescripcionDefectoFondos_Retiro = Nothing
                    End If

                    If lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_CANCELACION").Count > 0 Then
                        ConceptoDefectoFondos_Cancelacion = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_CANCELACION").First.intID
                        ConceptoDescripcionDefectoFondos_Cancelacion = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_CANCELACION").First.Descripcion
                    Else
                        ConceptoDefectoFondos_Cancelacion = Nothing
                        ConceptoDescripcionDefectoFondos_Cancelacion = Nothing
                    End If

                    If EsNuevoRegistro = False Then
                        DescripcionConcepto = ConcatenarConcepto(_IDConcepto)

                        If logEsFondosOYD Then
                            ConsultarCarterasColectivasClientes(True, "INICIO")
                        Else
                            ConsultarCarterasColectivasClientes(False, "INICIO")
                        End If
                    Else
                        PrepararNuevoRegistro()
                        IsBusy = False
                        HabilitarControles()
                    End If
                Else
                    Dim objListaTipoAccionFondos As New List(Of OYDUtilidades.ItemCombo)

                    If TipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
                        If lo.Entities.ToList.Where(Function(i) i.Categoria = "TIPOACCIONFONDOS").Count > 0 Then
                            objListaTipoAccionFondos = lo.Entities.ToList.Where(Function(i) i.Categoria = "TIPOACCIONFONDOS").ToList
                        End If
                    Else
                        If lo.Entities.ToList.Where(Function(i) i.Categoria = "TIPOACCIONFONDOS").Count > 0 Then
                            objListaTipoAccionFondos = lo.Entities.ToList.Where(Function(i) i.Categoria = "TIPOACCIONFONDOS").ToList
                        End If
                    End If

                    If lo.Entities.ToList.Where(Function(i) i.Categoria = "ADICIONES_DIASAPLICACION").Count > 0 Then
                        intDiasAplicacionFondosAdicion = CInt(lo.Entities.ToList.Where(Function(i) i.Categoria = "ADICIONES_DIASAPLICACION").First.ID)
                    End If
                    If lo.Entities.ToList.Where(Function(i) i.Categoria = "APERTURAS_DIASAPLICACION").Count > 0 Then
                        intDiasAplicacionFondosApertura = CInt(lo.Entities.ToList.Where(Function(i) i.Categoria = "APERTURAS_DIASAPLICACION").First.ID)
                    End If

                    ListaTiposAccionOYD = objListaTipoAccionFondos

                    If lo.UserState = "INICIOOYD" Then
                        logCambiarPropiedades = False
                        TipoAccionFondosOYD = TipoAccionRetorno
                        logCambiarPropiedades = True
                        ConsultarCarterasColectivasClientes(False, "INICIOOYD")
                    Else
                        If ListaTiposAccionOYD.Count = 1 Then
                            TipoAccionFondosOYD = ListaTiposAccionOYD.First.ID
                        End If
                    End If
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos de la cartera",
                                                 Me.ToString(), "TerminoTraerCombosEspecificosCartera", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos de la cartera",
                                                 Me.ToString(), "TerminoTraerCombosEspecificosCartera", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarSaldoCliente(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblSaldosCliente))
        Try
            If lo.HasError = False Then
                Dim objListaSaldoCliente = lo.Entities.ToList

                If logRegistroExistente Then
                    Dim dblValorDecimal As Decimal = objListaSaldoCliente.FirstOrDefault.Valor - ValorNetoOrden - ValorOriginalDetalle
                    SaldoConsultado = Decimal.Round(dblValorDecimal, 2)

                    If SaldoConsultado <= 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("No hay saldo disponible", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        ValorGenerar = SaldoConsultado
                        If TipoGMF = GSTR_GMF_ENCIMA Then
                            ValorGenerar = ValorGenerar - ValorGMF
                        End If
                    End If

                Else
                    Dim dblValorDecimal2 As Decimal = objListaSaldoCliente.FirstOrDefault.Valor - ValorEdicionDetalle + ValorOriginalDetalle
                    SaldoConsultado = Decimal.Round(dblValorDecimal2, 2)
                    If SaldoConsultado <= 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("No hay saldo disponible", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        ValorGenerar = SaldoConsultado
                        If TipoGMF = GSTR_GMF_ENCIMA Then
                            ValorGenerar = ValorGenerar - ValorGMF
                        End If
                    End If
                End If
                'Sí se consulta saldo la variable de las liquidaciones queda vacia
                If SaldoConsultado > 0 Then
                    LiquidacionesSeleccionadas = String.Empty
                End If

                IsBusy = False
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el saldo del cliente.", Me.ToString(), "TerminoConsultarSaldoCliente", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el saldo del cliente.", Me.ToString(), "TerminoConsultarSaldoCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub TerminoCancelarEditarRegistro(ByVal lo As InvokeOperation(Of Integer))
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cancelar la edición del registro.", Me.ToString(), "TerminoCancelarEditarRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoConsultarCarterasColectivasClientes(ByVal lo As LoadOperation(Of OyDPLUSTesoreria.CarterasColectivasClientes))
        Try
            If lo.UserState = "INICIO" Then
                ListaCarterasColectivasCompleta = lo.Entities.ToList

                If logEsFondosOYD Then
                    Dim objListaNueva As New List(Of CarterasColectivas)
                    For Each li In _ListaCarterasColectivasCompleta
                        If objListaNueva.Where(Function(i) i.ID = li.CarteraColectiva).Count = 0 Then
                            objListaNueva.Add(New CarterasColectivas With {.ID = li.CarteraColectiva, .Descripcion = li.NombreCarteraColectiva})
                        End If
                    Next

                    ListaCarterasColectivasOYD = objListaNueva
                    logCambiarPropiedades = False
                    CarteraColectivaOYD = CarteraColectivaRetorno
                    consultarCombosEspecificosFondo(_CarteraColectivaOYD, "INICIOOYD")
                    logCambiarPropiedades = True
                Else
                    logCambiarPropiedades = False
                    TipoAccionFondosExterno = TipoAccionRetorno
                    CargarCarteraColectivaExterno()
                    CalcularFechaAplicacionExterno()
                    CarteraColectivaExterno = CarteraColectivaRetorno
                    If _TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_ADICION Then
                        ConsultarCarterasColectivasClientes(False, "INICIOEXTERNO")
                    Else
                        HabilitarControles()
                        IsBusy = False
                    End If
                    logCambiarPropiedades = True
                End If
            ElseIf lo.UserState = "INICIOEXTERNO" Then
                Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
                For Each li In lo.Entities.ToList
                    If li.CarteraColectiva = CarteraColectivaExterno And Not String.IsNullOrEmpty(li.NroEncargo) Then
                        If objNuevaListaEncargos.Where(Function(i) i.NroEncargo = li.NroEncargo).Count = 0 Then
                            objNuevaListaEncargos.Add(New OyDPLUSTesoreria.CarterasColectivasClientes With {.CarteraColectiva = li.CarteraColectiva,
                                                      .CodigoOYD = li.CodigoOYD,
                                                      .ID = li.ID,
                                                      .NroEncargo = li.NroEncargo,
                                                      .Saldo = li.Saldo,
                                                      .dtmFechaCierre = li.dtmFechaCierre})
                        End If
                    End If
                Next

                ListaEncargosCarteraColectivaExterno = objNuevaListaEncargos

                EncargoCarteraExterno = EncargoCarteraRetorno
                HabilitarControles()

                IsBusy = False
            ElseIf lo.UserState = "INICIOOYD" Then
                Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
                For Each li In lo.Entities.ToList
                    If li.CarteraColectiva = CarteraColectivaOYD And Not String.IsNullOrEmpty(li.NroEncargo) Then
                        If objNuevaListaEncargos.Where(Function(i) i.NroEncargo = li.NroEncargo).Count = 0 Then
                            objNuevaListaEncargos.Add(New OyDPLUSTesoreria.CarterasColectivasClientes With {.CarteraColectiva = li.CarteraColectiva,
                                                      .CodigoOYD = li.CodigoOYD,
                                                      .ID = li.ID,
                                                      .NroEncargo = li.NroEncargo,
                                                      .Saldo = li.Saldo,
                                                      .dtmFechaCierre = li.dtmFechaCierre})
                        End If
                    End If
                Next

                ListaEncargosCarteraColectivaOYD = objNuevaListaEncargos

                logCambiarPropiedades = False
                If _TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_ADICION Then
                    EncargoCarteraOYD = EncargoCarteraRetorno
                    DescripcionEncargoFondosOYD = Nothing
                Else
                    EncargoCarteraOYD = Nothing
                    DescripcionEncargoFondosOYD = DescripcionEncargoRetorno
                End If
                logCambiarPropiedades = True
                HabilitarControles()
                IsBusy = False

            ElseIf lo.UserState = "CODIGOOYDDETALLE" Then
                ListaCarterasColectivasCompleta = lo.Entities.ToList

                If logEsFondosOYD Then
                    Dim objListaNueva As New List(Of CarterasColectivas)
                    For Each li In _ListaCarterasColectivasCompleta
                        If objListaNueva.Where(Function(i) i.ID = li.CarteraColectiva).Count = 0 Then
                            objListaNueva.Add(New CarterasColectivas With {.ID = li.CarteraColectiva, .Descripcion = li.NombreCarteraColectiva})
                        End If
                    Next

                    ListaCarterasColectivasOYD = objListaNueva

                    If ListaCarterasColectivasOYD.Count = 1 Then
                        CarteraColectivaOYD = ListaCarterasColectivasOYD.First.ID
                    End If
                End If

                LimpiarDatosCarteraOYD(False)
            ElseIf lo.UserState = "CARTERACOLECTIVAOYD" Then
                Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
                For Each li In lo.Entities.ToList
                    If li.CarteraColectiva = CarteraColectivaOYD And Not String.IsNullOrEmpty(li.NroEncargo) Then
                        If objNuevaListaEncargos.Where(Function(i) i.NroEncargo = li.NroEncargo).Count = 0 Then
                            objNuevaListaEncargos.Add(New OyDPLUSTesoreria.CarterasColectivasClientes With {.CarteraColectiva = li.CarteraColectiva,
                                                      .CodigoOYD = li.CodigoOYD,
                                                      .ID = li.ID,
                                                      .NroEncargo = li.NroEncargo,
                                                      .Saldo = li.Saldo,
                                                      .dtmFechaCierre = li.dtmFechaCierre})
                        End If
                    End If
                Next

                ListaEncargosCarteraColectivaOYD = objNuevaListaEncargos

                If TipoAccionFondosOYD = GSTR_FONDOS_TIPOACCION_ADICION Then
                    If ListaEncargosCarteraColectivaOYD.Count = 1 Then
                        EncargoCarteraOYD = ListaEncargosCarteraColectivaOYD.First.NroEncargo
                    End If
                End If

            ElseIf lo.UserState = "CARTERACOLECTIVAEXTERNO" Then
                Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
                For Each li In lo.Entities.ToList
                    If li.CarteraColectiva = CarteraColectivaExterno And Not String.IsNullOrEmpty(li.NroEncargo) Then
                        If objNuevaListaEncargos.Where(Function(i) i.NroEncargo = li.NroEncargo).Count = 0 Then
                            objNuevaListaEncargos.Add(New OyDPLUSTesoreria.CarterasColectivasClientes With {.CarteraColectiva = li.CarteraColectiva,
                                                      .CodigoOYD = li.CodigoOYD,
                                                      .ID = li.ID,
                                                      .NroEncargo = li.NroEncargo,
                                                      .Saldo = li.Saldo,
                                                      .dtmFechaCierre = li.dtmFechaCierre})
                        End If
                    End If
                Next

                ListaEncargosCarteraColectivaExterno = objNuevaListaEncargos

                If TipoAccionFondosExterno = GSTR_FONDOS_TIPOACCION_ADICION Then
                    If ListaEncargosCarteraColectivaExterno.Count = 1 Then
                        EncargoCarteraExterno = ListaEncargosCarteraColectivaExterno.First.NroEncargo
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar las carteras colectivas.", Me.ToString(), "TerminoConsultarCarterasColectivasClientes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoVerificarRestriccionesTipoCartera(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                If Not String.IsNullOrEmpty(lo.Value) Then
                    mostrarMensaje(lo.Value, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las restriccione del tipo de cartera",
                                                 Me.ToString(), "TerminoVerificarRestriccionesTipoCartera", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las restriccione del tipo de cartera",
                                                 Me.ToString(), "TerminoVerificarRestriccionesTipoCartera", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class
