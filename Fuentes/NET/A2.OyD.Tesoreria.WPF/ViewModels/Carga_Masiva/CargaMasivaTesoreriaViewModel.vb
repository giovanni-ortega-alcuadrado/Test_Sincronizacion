Imports Telerik.Windows.Controls
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
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports A2MCCoreWPF
Imports A2.OyD.Tesoreria
Imports System.Threading.Tasks


Public Class CargaMasivaTesoreriaViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            IsBusy = True

            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext
                dcProxyConsulta = New TesoreriaDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext
            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyConsulta = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0) 'DEMC20191017 SE AUMENTA EL TIMEOUT A 60 MINUTOS YA QUE ES PROCESO MASIVO.
            DirectCast(dcProxyConsulta.DomainClient, WebDomainClient(Of TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0) 'DEMC20191017 SE AUMENTA EL TIMEOUT A 60 MINUTOS YA QUE ES PROCESO MASIVO.
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0) 'DEMC20191017 SE AUMENTA EL TIMEOUT A 60 MINUTOS YA QUE ES PROCESO MASIVO.
            mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosEspecificosQuery("CargaMasivaTesoreria", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", Me.ToString(), "CargaMasivaTesoreriaViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Constantes"

    Public Enum TipoOpcionCargar
        ARCHIVO
        CAMPOS
        RESULTADO
    End Enum
    Private Const COLOR_DESHABILITADO As String = "#E2E2E2"
    Private Const COLOR_HABILITADO As String = "#FFFFFFFF"

#End Region

#Region "Variables"
    Private dcProxy As TesoreriaDomainContext
    Private dcProxyConsulta As TesoreriaDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private dcProxyImportaciones As ImportacionesDomainContext
    Dim logConsultarCliente As Boolean = True
    Public viewCargaMasiva As CargaMasivaTesoreriaView
    Dim logRecargarResultados As Boolean = False
    Dim logCargarContenido As Boolean = True
    Dim strTemporalTipoNegocio As String = String.Empty
    Dim strTemporalTipoOperacion As String = String.Empty
    Dim logCalculoValores As Boolean = True
    Dim dtmFechaServidor As DateTime
    Public strCuentaContableClientes As String = String.Empty
    Public strSeparadorFormato As String = String.Empty
    Dim logConsultoCamposHabilitar As Boolean = False
    Dim cuentaContable As String = String.Empty
    Dim TipoCuenta As String = String.Empty
    Dim SplitCuenta As Boolean = False

#End Region

#Region "Propiedades"

    Private _HabilitarSeleccionTipoDocumento As Boolean = True
    Public Property HabilitarSeleccionTipoDocumento() As Boolean
        Get
            Return _HabilitarSeleccionTipoDocumento
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionTipoDocumento = value
            MyBase.CambioItem("HabilitarSeleccionTipoDocumento")
        End Set
    End Property

    Private _FondoTextoBuscadorCliente As SolidColorBrush
    Public Property FondoTextoBuscadorCliente() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorCliente
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorCliente = value
            MyBase.CambioItem("FondoTextoBuscadorCliente")
        End Set
    End Property

    Private _FondoTextoBuscadorClienteD As SolidColorBrush
    Public Property FondoTextoBuscadorClienteD() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorClienteD
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorClienteD = value
            MyBase.CambioItem("FondoTextoBuscadorClienteD")
        End Set
    End Property

    Private _FondoTextoBuscadorCuentaContable As SolidColorBrush
    Public Property FondoTextoBuscadorCuentaContable() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorCuentaContable
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorCuentaContable = value
            MyBase.CambioItem("FondoTextoBuscadorCuentaContable")
        End Set
    End Property

    Private _FondoTextoBuscadorNIT As SolidColorBrush
    Public Property FondoTextoBuscadorNIT() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorNIT
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorNIT = value
            MyBase.CambioItem("FondoTextoBuscadorNIT")
        End Set
    End Property

    Private _FondoTextoBuscadorCentroCostos As SolidColorBrush
    Public Property FondoTextoBuscadorCentroCostos() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorCentroCostos
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorCentroCostos = value
            MyBase.CambioItem("FondoTextoBuscadorCentroCostos")
        End Set
    End Property

    Private _FondoTextoBuscadorBancoConsignacion As SolidColorBrush
    Public Property FondoTextoBuscadorBancoConsignacion() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorBancoConsignacion
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorBancoConsignacion = value
            MyBase.CambioItem("FondoTextoBuscadorBancoConsignacion")
        End Set
    End Property

    Private _FondoTextoBuscadorBanco As SolidColorBrush
    Public Property FondoTextoBuscadorBanco() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorBanco
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorBanco = value
            MyBase.CambioItem("FondoTextoBuscadorBanco")
        End Set
    End Property

    Private _FondoTextoBuscadorConcepto As SolidColorBrush
    Public Property FondoTextoBuscadorConcepto() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorConcepto
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorConcepto = value
            MyBase.CambioItem("FondoTextoBuscadorConcepto")
        End Set
    End Property

    Private _FondoTextoBuscadorFormaEntrega As SolidColorBrush
    Public Property FondoTextoBuscadorFormaEntrega() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorFormaEntrega
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorFormaEntrega = value
            MyBase.CambioItem("FondoTextoBuscadorFormaEntrega")
        End Set
    End Property

    Private _FondoTextoBuscadorTipoIdenBeneficiario As SolidColorBrush
    Public Property FondoTextoBuscadorTipoIdenBeneficiario() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorTipoIdenBeneficiario
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorTipoIdenBeneficiario = value
            MyBase.CambioItem("FondoTextoBuscadorTipoIdenBeneficiario")
        End Set
    End Property

    Private _FondoTextoBuscadorEntidad As SolidColorBrush
    Public Property FondoTextoBuscadorEntidad() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorEntidad
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorEntidad = value
            MyBase.CambioItem("FondoTextoBuscadorEntidad")
        End Set
    End Property

    Private _FondoTextoBuscadorTipoCuenta As SolidColorBrush
    Public Property FondoTextoBuscadorTipoCuenta() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorTipoCuenta
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorTipoCuenta = value
            MyBase.CambioItem("FondoTextoBuscadorTipoCuenta")
        End Set
    End Property

    Private _FondoTextoBuscadorTipoIDTitular As SolidColorBrush
    Public Property FondoTextoBuscadorTipoIDTitular() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorTipoIDTitular
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorTipoIDTitular = value
            MyBase.CambioItem("FondoTextoBuscadorTipoIDTitular")
        End Set
    End Property

    Private _DiccionarioCargaTesoreria As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCargaTesoreria() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCargaTesoreria
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCargaTesoreria = value
            MyBase.CambioItem("DiccionarioCargaTesoreria")
        End Set
    End Property

    Private _ListaCombo As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaCombo() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaCombo
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaCombo = value
            MyBase.CambioItem("ListaCombo")
        End Set
    End Property

    Private _TipoDocumentoSeleccionado As String = String.Empty
    Public Property TipoDocumentoSeleccionado() As String
        Get
            Return _TipoDocumentoSeleccionado
        End Get
        Set(ByVal value As String)
            _TipoDocumentoSeleccionado = value
            LlenarComboConsecutivo(value)
            MyBase.CambioItem("TipoDocumentoSeleccionado")
        End Set
    End Property

    Private _ConsecutivoSeleccionado As String = String.Empty
    Public Property ConsecutivoSeleccionado() As String
        Get
            Return _ConsecutivoSeleccionado
        End Get
        Set(ByVal value As String)
            _ConsecutivoSeleccionado = value
            ObtenerCompaniaConsecutivo(ConsecutivoSeleccionado)
            CargarContenido(TipoOpcionCargar.ARCHIVO)
            MyBase.CambioItem("ConsecutivoSeleccionado")
        End Set
    End Property

    Private _IDCompaniaConsecutivo As Integer
    Public Property IDCompaniaConsecutivo() As Integer
        Get
            Return _IDCompaniaConsecutivo
        End Get
        Set(ByVal value As Integer)
            _IDCompaniaConsecutivo = value
            MyBase.CambioItem("IDCompaniaConsecutivo")
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

    Private _ListaResultadoValidacion As List(Of OyDTesoreria.tblRespuestaValidacionesCargaMasiva)
    Public Property ListaResultadoValidacion() As List(Of OyDTesoreria.tblRespuestaValidacionesCargaMasiva)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of OyDTesoreria.tblRespuestaValidacionesCargaMasiva))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
            MyBase.CambioItem("ListaResultadoValidacionPaged")
        End Set
    End Property

    Public ReadOnly Property ListaResultadoValidacionPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultadoValidacion) Then
                Dim view = New PagedCollectionView(_ListaResultadoValidacion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DiccionarioEdicionCampos As Dictionary(Of String, Boolean)
    Public Property DiccionarioEdicionCampos() As Dictionary(Of String, Boolean)
        Get
            Return _DiccionarioEdicionCampos
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            _DiccionarioEdicionCampos = value
            MyBase.CambioItem("DiccionarioEdicionCampos")
        End Set
    End Property

    Private _ListaComboCamposPermitidosEdicion As List(Of OyDTesoreria.CamposEditablesCargaTesoreria)
    Public Property ListaComboCamposPermitidosEdicion() As List(Of OyDTesoreria.CamposEditablesCargaTesoreria)
        Get
            Return _ListaComboCamposPermitidosEdicion
        End Get
        Set(ByVal value As List(Of OyDTesoreria.CamposEditablesCargaTesoreria))
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

    Private WithEvents _ImportacionEncabezadoTeSelected As New OyDTesoreria.Tesoreri
    Public Property ImportacionEncabezadoTeSelected() As OyDTesoreria.Tesoreri
        Get
            Return _ImportacionEncabezadoTeSelected
        End Get
        Set(ByVal value As OyDTesoreria.Tesoreri)
            _ImportacionEncabezadoTeSelected = value
            MyBase.CambioItem("ImportacionEncabezadoTeSelected")

            Try
                If Not IsNothing(_ImportacionEncabezadoTeSelected) Then
                    MyBase.CambioItem("DiccionarioCombos")
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al realizar las consultas del encabezado de tesorería.", Me.ToString(), "ImportacionEncabezadoTeSelected", Application.Current.ToString(), Program.Maquina, ex)
                IsBusy = False
            End Try
        End Set
    End Property

    Private _ImportacionDetalleTeSelected As New OyDTesoreria.DetalleTesoreri
    Public Property ImportacionDetalleTeSelected() As OyDTesoreria.DetalleTesoreri
        Get
            Return _ImportacionDetalleTeSelected
        End Get
        Set(ByVal value As OyDTesoreria.DetalleTesoreri)
            _ImportacionDetalleTeSelected = value
            MyBase.CambioItem("ImportacionDetalleTeSelected")
        End Set
    End Property

    Private _ImportacionDetalleChequesTeSelected As New OyDTesoreria.Cheque
    Public Property ImportacionDetalleChequesTeSelected() As OyDTesoreria.Cheque
        Get
            Return _ImportacionDetalleChequesTeSelected
        End Get
        Set(ByVal value As OyDTesoreria.Cheque)
            _ImportacionDetalleChequesTeSelected = value
            MyBase.CambioItem("ImportacionDetalleChequesTeSelected")
        End Set
    End Property


    Private _ListaDetalleTeSelected As New List(Of OyDTesoreria.DetalleTesoreri)
    Public Property ListaDetalleTeSelected() As List(Of OyDTesoreria.DetalleTesoreri)
        Get
            Return _ListaDetalleTeSelected
        End Get
        Set(ByVal value As List(Of OyDTesoreria.DetalleTesoreri))
            _ListaDetalleTeSelected = value
            MyBase.CambioItem("ImportacionDetalleTeSelected")
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

    Private _listCuentasbancarias As List(Of OYDUtilidades.ItemCombo)
    Public Property listCuentasbancarias() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listCuentasbancarias
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listCuentasbancarias = value
            MyBase.CambioItem("listCuentasbancarias")
        End Set
    End Property

    Private _HabilitarIDComitente As Boolean = False
    Public Property HabilitarIDComitente() As Boolean
        Get
            Return _HabilitarIDComitente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarIDComitente = value
            MyBase.CambioItem("HabilitarIDComitente")
        End Set
    End Property

    Private _HabilitarCuentaBancaria As Boolean = False
    Public Property HabilitarCuentaBancaria() As Boolean
        Get
            Return _HabilitarCuentaBancaria
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCuentaBancaria = value
            MyBase.CambioItem("HabilitarCuentaBancaria")
        End Set
    End Property

    Private _HabilitarFormaEntrega As Boolean
    Public Property HabilitarFormaEntrega() As Boolean
        Get
            Return _HabilitarFormaEntrega
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFormaEntrega = value
            MyBase.CambioItem("HabilitarFormaEntrega")
        End Set
    End Property

    Private _HabilitarBeneficiario As Boolean
    Public Property HabilitarBeneficiario() As Boolean
        Get
            Return _HabilitarBeneficiario
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBeneficiario = value
            MyBase.CambioItem("HabilitarBeneficiario")
        End Set
    End Property

    Private _HabilitarTipoIdenBeneficiario As Boolean
    Public Property HabilitarTipoIdenBeneficiario() As Boolean
        Get
            Return _HabilitarTipoIdenBeneficiario
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoIdenBeneficiario = value
            MyBase.CambioItem("HabilitarTipoIdenBeneficiario")
        End Set
    End Property

    Private _HabilitarIdentificacionBeneficiario As Boolean
    Public Property HabilitarIdentificacionBeneficiario() As Boolean
        Get
            Return _HabilitarIdentificacionBeneficiario
        End Get
        Set(ByVal value As Boolean)
            _HabilitarIdentificacionBeneficiario = value
            MyBase.CambioItem("HabilitarIdentificacionBeneficiario")
        End Set
    End Property

    Private _HabilitarNombrePersonaRecoge As Boolean
    Public Property HabilitarNombrePersonaRecoge() As Boolean
        Get
            Return _HabilitarNombrePersonaRecoge
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNombrePersonaRecoge = value
            MyBase.CambioItem("HabilitarNombrePersonaRecoge")
        End Set
    End Property

    Private _HabilitarIdentificacionPersonaRecoge As Boolean
    Public Property HabilitarIdentificacionPersonaRecoge() As Boolean
        Get
            Return _HabilitarIdentificacionPersonaRecoge
        End Get
        Set(ByVal value As Boolean)
            _HabilitarIdentificacionPersonaRecoge = value
            MyBase.CambioItem("HabilitarIdentificacionPersonaRecoge")
        End Set
    End Property

    Private _HabilitarOficina As Boolean
    Public Property HabilitarOficina() As Boolean
        Get
            Return _HabilitarOficina
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOficina = value
            MyBase.CambioItem("HabilitarOficina")
        End Set
    End Property

    Private _HabilitarEntidad As Boolean
    Public Property HabilitarEntidad() As Boolean
        Get
            Return _HabilitarEntidad
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEntidad = value
            MyBase.CambioItem("HabilitarEntidad")
        End Set
    End Property

    Private _HabilitarTipoCuenta As Boolean
    Public Property HabilitarTipoCuenta() As Boolean
        Get
            Return _HabilitarTipoCuenta
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoCuenta = value
            MyBase.CambioItem("HabilitarTipoCuenta")
        End Set
    End Property

    Private _HabilitarNroCuenta As Boolean
    Public Property HabilitarNroCuenta() As Boolean
        Get
            Return _HabilitarNroCuenta
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNroCuenta = value
            MyBase.CambioItem("HabilitarNroCuenta")
        End Set
    End Property

    Private _HabilitarTipoIDTitular As Boolean
    Public Property HabilitarTipoIDTitular() As Boolean
        Get
            Return _HabilitarTipoIDTitular
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoIDTitular = value
            MyBase.CambioItem("HabilitarTipoIDTitular")
        End Set
    End Property

    Private _HabilitarIDTitular As Boolean
    Public Property HabilitarIDTitular() As Boolean
        Get
            Return _HabilitarIDTitular
        End Get
        Set(ByVal value As Boolean)
            _HabilitarIDTitular = value
            MyBase.CambioItem("HabilitarIDTitular")
        End Set
    End Property

    Private _HabilitarNombreTitular As Boolean
    Public Property HabilitarNombreTitular() As Boolean
        Get
            Return _HabilitarNombreTitular
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNombreTitular = value
            MyBase.CambioItem("HabilitarNombreTitular")
        End Set
    End Property

    Private _HabilitarCodCartera As Boolean
    Public Property HabilitarCodCartera() As Boolean
        Get
            Return _HabilitarCodCartera
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodCartera = value
            MyBase.CambioItem("HabilitarCodCartera")
        End Set
    End Property

    Private _HabilitarOficinaCuentaInversion As Boolean
    Public Property HabilitarOficinaCuentaInversion() As Boolean
        Get
            Return _HabilitarOficinaCuentaInversion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOficinaCuentaInversion = value
            MyBase.CambioItem("HabilitarOficinaCuentaInversion")
        End Set
    End Property

    Private _HabilitarNombreCarteraColectiva As Boolean
    Public Property HabilitarNombreCarteraColectiva() As Boolean
        Get
            Return _HabilitarNombreCarteraColectiva
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNombreCarteraColectiva = value
            MyBase.CambioItem("HabilitarNombreCarteraColectiva")
        End Set
    End Property

    Private _HabilitarTipoCheque As Boolean
    Public Property HabilitarTipoCheque() As Boolean
        Get
            Return _HabilitarTipoCheque
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoCheque = value
            MyBase.CambioItem("HabilitarTipoCheque")
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub CambiarColorFondoTextoBuscador()
        Try
            Dim colorFondo As Color

            '----------------------------------------------------------
            If ValidarRetornoHabilitarCampos("Nombre") _
                And ValidarRetornoHabilitarCampos("TipoIdentificacion") _
                And ValidarRetornoHabilitarCampos("NroDocumento") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
                HabilitarIDComitente = True
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
                HabilitarIDComitente = False
            End If
            FondoTextoBuscadorCliente = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If ValidarRetornoHabilitarCampos("IdComitente") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If

            FondoTextoBuscadorClienteD = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If ValidarRetornoHabilitarCampos("CuentaContable") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorCuentaContable = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If ValidarRetornoHabilitarCampos("Nit") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorNIT = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If ValidarRetornoHabilitarCampos("CentroCostos") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorCentroCostos = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If ValidarRetornoHabilitarCampos("IdBanco") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorBanco = New SolidColorBrush(colorFondo)
            FondoTextoBuscadorBancoConsignacion = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If ValidarRetornoHabilitarCampos("IdConcepto") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorConcepto = New SolidColorBrush(colorFondo)

            HabilitarCuentaBancaria = False

            colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            FondoTextoBuscadorFormaEntrega = New SolidColorBrush(colorFondo)
            FondoTextoBuscadorTipoIdenBeneficiario = New SolidColorBrush(colorFondo)
            FondoTextoBuscadorEntidad = New SolidColorBrush(colorFondo)
            FondoTextoBuscadorTipoCuenta = New SolidColorBrush(colorFondo)
            FondoTextoBuscadorTipoIDTitular = New SolidColorBrush(colorFondo)
            
            HabilitarFormaEntrega = False
            HabilitarBeneficiario = False
            HabilitarTipoIdenBeneficiario = False
            HabilitarIdentificacionBeneficiario = False
            HabilitarNombrePersonaRecoge = False
            HabilitarIdentificacionPersonaRecoge = False
            HabilitarOficina = False
            HabilitarEntidad = False
            HabilitarTipoCuenta = False
            HabilitarNroCuenta = False
            HabilitarTipoIDTitular = False
            HabilitarIDTitular = False
            HabilitarNombreTitular = False
            HabilitarCodCartera = False
            HabilitarOficinaCuentaInversion = False
            HabilitarNombreCarteraColectiva = False
            HabilitarTipoCheque = False
            HabilitarCuentaBancaria = False

            MyBase.CambioItem("DiccionarioEdicionCampos")

            If TipoDocumentoSeleccionado.ToUpper = "EGRESOS" Then
                If ValidarRetornoHabilitarCampos("FormaPago") Then
                    If _ImportacionEncabezadoTeSelected.FormaPagoCE = "C" Then
                        If ValidarRetornoHabilitarCampos("TipoCheque") Then
                            HabilitarTipoCheque = True
                        End If
                    ElseIf _ImportacionEncabezadoTeSelected.FormaPagoCE = "T" Then
                        If ValidarRetornoHabilitarCampos("CuentaBancaria") Then
                            If DiccionarioEdicionCampos.ContainsKey("IdComitente") Then
                                HabilitarCuentaBancaria = False
                                mostrarMensaje("No se puede seleccionar esta forma de pago. Como en la importación se envió el campo IDComitente también es necesario enviar la cuenta bancaria. Por favor modifique la importación para incluir el campo CuentaBancaria para cada comitente o modifique la importación para no enviar el campo IDComitente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else
                                HabilitarCuentaBancaria = True
                            End If
                        End If
                    ElseIf _ImportacionEncabezadoTeSelected.FormaPagoCE = "B" Then
                        colorFondo = Program.colorFromHex(COLOR_HABILITADO)

                        If ValidarRetornoHabilitarCampos("FormaEntrega") Then
                            HabilitarFormaEntrega = True
                            FondoTextoBuscadorFormaEntrega = New SolidColorBrush(colorFondo)
                        End If
                        If ValidarRetornoHabilitarCampos("Beneficiario") Then
                            HabilitarBeneficiario = True
                        End If
                        If ValidarRetornoHabilitarCampos("TipoIdentBeneficiario") Then
                            HabilitarTipoIdenBeneficiario = True
                            FondoTextoBuscadorTipoIdenBeneficiario = New SolidColorBrush(colorFondo)
                        End If
                        If ValidarRetornoHabilitarCampos("IdentificacionBenficiciario") Then
                            HabilitarIdentificacionBeneficiario = True
                        End If
                        If ValidarRetornoHabilitarCampos("NombrePersonaRecoge") Then
                            HabilitarNombrePersonaRecoge = True
                        End If
                        If ValidarRetornoHabilitarCampos("IdentificacionPerRecoge") Then
                            HabilitarIdentificacionPersonaRecoge = True
                        End If
                        If ValidarRetornoHabilitarCampos("OficinaEntrega") Then
                            HabilitarOficina = True
                        End If
                    ElseIf _ImportacionEncabezadoTeSelected.FormaPagoCE = "N" Then
                        colorFondo = Program.colorFromHex(COLOR_HABILITADO)

                        If ValidarRetornoHabilitarCampos("BancoDestino") Then
                            HabilitarEntidad = True
                        End If
                        If ValidarRetornoHabilitarCampos("TipoCuenta") Then
                            HabilitarTipoCuenta = True
                            FondoTextoBuscadorTipoCuenta = New SolidColorBrush(colorFondo)
                        End If
                        If ValidarRetornoHabilitarCampos("CuentaDestino") Then
                            HabilitarNroCuenta = True
                        End If
                        If ValidarRetornoHabilitarCampos("TipoIdTitular") Then
                            HabilitarTipoIDTitular = True
                            FondoTextoBuscadorTipoIDTitular = New SolidColorBrush(colorFondo)
                        End If
                        If ValidarRetornoHabilitarCampos("IdentificacionTitular") Then
                            HabilitarIDTitular = True
                        End If
                        If ValidarRetornoHabilitarCampos("Titular") Then
                            HabilitarNombreTitular = True
                        End If
                    ElseIf _ImportacionEncabezadoTeSelected.FormaPagoCE = "L" Then
                        colorFondo = Program.colorFromHex(COLOR_HABILITADO)

                        If ValidarRetornoHabilitarCampos("BancoDestino") Then
                            HabilitarEntidad = True
                            FondoTextoBuscadorEntidad = New SolidColorBrush(colorFondo)
                        End If
                        If ValidarRetornoHabilitarCampos("TipoCuenta") Then
                            HabilitarTipoCuenta = True
                            FondoTextoBuscadorTipoCuenta = New SolidColorBrush(colorFondo)
                        End If
                        If ValidarRetornoHabilitarCampos("CuentaDestino") Then
                            HabilitarNroCuenta = True
                        End If
                        If ValidarRetornoHabilitarCampos("TipoIdTitular") Then
                            HabilitarTipoIDTitular = True
                            FondoTextoBuscadorTipoIDTitular = New SolidColorBrush(colorFondo)
                        End If
                        If ValidarRetornoHabilitarCampos("IdentificacionTitular") Then
                            HabilitarIDTitular = True
                        End If
                        If ValidarRetornoHabilitarCampos("Titular") Then
                            HabilitarNombreTitular = True
                        End If
                        If ValidarRetornoHabilitarCampos("CodigoCartera") Then
                            HabilitarCodCartera = True
                        End If
                        If ValidarRetornoHabilitarCampos("OficinaCuentaInversion") Then
                            HabilitarOficinaCuentaInversion = True
                        End If
                        If ValidarRetornoHabilitarCampos("NombreCarteraColectiva") Then
                            HabilitarNombreCarteraColectiva = True
                        End If
                    End If
                Else
                    If ValidarRetornoHabilitarCampos("TipoCheque") Then
                        HabilitarTipoCheque = True
                    End If
                    If ValidarRetornoHabilitarCampos("CuentaBancaria") Then
                        HabilitarCuentaBancaria = True
                    End If
                    If ValidarRetornoHabilitarCampos("FormaEntrega") Then
                        HabilitarFormaEntrega = True
                        FondoTextoBuscadorFormaEntrega = New SolidColorBrush(colorFondo)
                    End If
                    If ValidarRetornoHabilitarCampos("Beneficiario") Then
                        HabilitarBeneficiario = True
                    End If
                    If ValidarRetornoHabilitarCampos("TipoIdentBeneficiario") Then
                        HabilitarTipoIdenBeneficiario = True
                        FondoTextoBuscadorTipoIdenBeneficiario = New SolidColorBrush(colorFondo)
                    End If
                    If ValidarRetornoHabilitarCampos("IdentificacionBenficiciario") Then
                        HabilitarIdentificacionBeneficiario = True
                    End If
                    If ValidarRetornoHabilitarCampos("NombrePersonaRecoge") Then
                        HabilitarNombrePersonaRecoge = True
                    End If
                    If ValidarRetornoHabilitarCampos("IdentificacionPerRecoge") Then
                        HabilitarIdentificacionPersonaRecoge = True
                    End If
                    If ValidarRetornoHabilitarCampos("OficinaEntrega") Then
                        HabilitarOficina = True
                    End If
                    If ValidarRetornoHabilitarCampos("BancoDestino") Then
                        HabilitarEntidad = True
                    End If
                    If ValidarRetornoHabilitarCampos("TipoCuenta") Then
                        HabilitarTipoCuenta = True
                        FondoTextoBuscadorTipoCuenta = New SolidColorBrush(colorFondo)
                    End If
                    If ValidarRetornoHabilitarCampos("CuentaDestino") Then
                        HabilitarNroCuenta = True
                    End If
                    If ValidarRetornoHabilitarCampos("TipoIdTitular") Then
                        HabilitarTipoIDTitular = True
                        FondoTextoBuscadorTipoIDTitular = New SolidColorBrush(colorFondo)
                    End If
                    If ValidarRetornoHabilitarCampos("IdentificacionTitular") Then
                        HabilitarIDTitular = True
                    End If
                    If ValidarRetornoHabilitarCampos("Titular") Then
                        HabilitarNombreTitular = True
                    End If
                    If ValidarRetornoHabilitarCampos("CodigoCartera") Then
                        HabilitarCodCartera = True
                    End If
                    If ValidarRetornoHabilitarCampos("OficinaCuentaInversion") Then
                        HabilitarOficinaCuentaInversion = True
                    End If
                    If ValidarRetornoHabilitarCampos("NombreCarteraColectiva") Then
                        HabilitarNombreCarteraColectiva = True
                    End If
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el color de fondo del texto.", Me.ToString(), "CambiarColorFondoTextoBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValidarRetornoHabilitarCampos(ByVal pstrNombreCampo As String) As Boolean
        Dim objRetorno As Boolean = False

        If DiccionarioEdicionCampos.ContainsKey(pstrNombreCampo) Then
            objRetorno = DiccionarioEdicionCampos(pstrNombreCampo)
        End If

        Return objRetorno
    End Function

    Private Sub LlenarComboConsecutivo(ByVal pstrNombreConsecutivo As String)
        Try
            Dim objListaCOmbo As New List(Of OYDUtilidades.ItemCombo)
            If Not IsNothing(pstrNombreConsecutivo) Then
                If pstrNombreConsecutivo.ToUpper = "CAJA" Then
                    If DiccionarioCargaTesoreria.ContainsKey("CONSECUTIVOCAJA") Then
                        objListaCOmbo = DiccionarioCargaTesoreria("CONSECUTIVOCAJA")
                    End If
                End If
                If pstrNombreConsecutivo.ToUpper = "EGRESOS" Then
                    If DiccionarioCargaTesoreria.ContainsKey("CONSECUTIVOEGRESOS") Then
                        objListaCOmbo = DiccionarioCargaTesoreria("CONSECUTIVOEGRESOS")
                    End If
                End If
                If pstrNombreConsecutivo.ToUpper = "NOTAS" Then
                    If DiccionarioCargaTesoreria.ContainsKey("CONSECUTIVONOTAS") Then
                        objListaCOmbo = DiccionarioCargaTesoreria("CONSECUTIVONOTAS")
                    End If
                End If
                If objListaCOmbo.Count > 0 Then
                    ListaCombo = objListaCOmbo.OrderBy(Function(i) i.Descripcion).ToList
                Else
                    ListaCombo = objListaCOmbo
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos consecutivos", _
                     Me.ToString(), "LlenarComboConsecutivo", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TerminoCargarCombosEspecificos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        'se definen de tipo observable los diccionarios y los recursos List
        Dim objListaCombos As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim dicListaCombos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)) = Nothing
        Dim strNombreCategoria As String = String.Empty

        Try
            If Not lo.HasError Then
                'Obtiene los valores del UserState
                'Convierte los datos recibidos en un diccionario donde el nombre de la categoría es la clave
                objListaCombos = New List(Of OYDUtilidades.ItemCombo)(lo.Entities)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    dicListaCombos = New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        objListaNodosCategoria = New List(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))

                        dicListaCombos.Add(NombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCargaTesoreria = dicListaCombos

                    If DiccionarioCargaTesoreria.ContainsKey("CUENTACONTABLECLIENTE") Then
                        strCuentaContableClientes = DiccionarioCargaTesoreria("CUENTACONTABLECLIENTE").First.ID
                    End If

                    If DiccionarioCargaTesoreria.ContainsKey("CARGAMASIVATESORERIA_TIPOSEPARADORCSV") Then
                        strFormapagoImportacion = DiccionarioCargaTesoreria("CARGAMASIVATESORERIA_TIPOSEPARADORCSV").First.ID
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Function ValidarIngresoCamposEditables() As Boolean
        Try
            Dim logRespuesta As Boolean = False
            Dim strMensajeValidacion = String.Empty

            If Not IsNothing(DiccionarioEdicionCampos) Then

                If TipoDocumentoSeleccionado.ToUpper = "EGRESOS" Then
                    For Each li In DiccionarioEdicionCampos()
                        If li.Key = "FechaDocumento" And li.Value = True Then
                            If IsNothing(ImportacionEncabezadoTeSelected.Documento) Then
                                strMensajeValidacion = String.Format("{0}{1} - Fecha Documento", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "IdBanco" And li.Value = True Then
                            If IsNothing(ImportacionEncabezadoTeSelected.IDBanco) Then
                                strMensajeValidacion = String.Format("{0}{1} - Banco", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "Nombre" And li.Value = True Then
                            If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.Nombre) Then
                                strMensajeValidacion = String.Format("{0}{1} - Recibí de", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "TipoIdentificacion" And li.Value = True Then
                            If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.TipoIdentificacion) Then
                                strMensajeValidacion = String.Format("{0}{1} - Tipo identificación", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "NroDocumento" And li.Value = True Then
                            If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.NroDocumento) Then
                                strMensajeValidacion = String.Format("{0}{1} - Número de identificación", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "FormaPago" And li.Value = True Then
                            If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.FormaPagoCE) Then
                                strMensajeValidacion = String.Format("{0}{1} - Forma pago", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "Valor" And li.Value = True Then
                            If IsNothing(ImportacionDetalleTeSelected.Valor) Or ImportacionDetalleTeSelected.Valor = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} - Valor", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "NroCheque" And li.Value = True Then
                            If ValidarRetornoHabilitarCampos("FormaPago") Then
                                If IsNothing(ImportacionEncabezadoTeSelected.NumCheque) Or ImportacionEncabezadoTeSelected.NumCheque = 0 Then
                                    strMensajeValidacion = String.Format("{0}{1} - Nro cheque", strMensajeValidacion, vbCrLf)
                                    logRespuesta = True
                                End If
                            End If
                        ElseIf li.Key = "TipoCheque" Then
                            If HabilitarTipoCheque Then
                                If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.Tipocheque) Then
                                    strMensajeValidacion = String.Format("{0}{1} - Tipo cheque", strMensajeValidacion, vbCrLf)
                                    logRespuesta = True
                                End If
                            End If
                        ElseIf li.Key = "CuentaBancaria" Then
                            If HabilitarCuentaBancaria Then
                                If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.CuentaCliente) Then
                                    strMensajeValidacion = String.Format("{0}{1} - Cuenta bancaria", strMensajeValidacion, vbCrLf)
                                    logRespuesta = True
                                End If
                            End If
                        End If
                    Next
                ElseIf TipoDocumentoSeleccionado.ToUpper = "CAJA" Then
                    For Each li In DiccionarioEdicionCampos()
                        If li.Key = "FechaDocumento" And li.Value = True Then
                            If IsNothing(ImportacionEncabezadoTeSelected.Documento) Then
                                strMensajeValidacion = String.Format("{0}{1} - Fecha Documento", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "Nombre" And li.Value = True Then
                            If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.Nombre) Then
                                strMensajeValidacion = String.Format("{0}{1} - Recibí de", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "TipoIdentificacion" And li.Value = True Then
                            If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.TipoIdentificacion) Then
                                strMensajeValidacion = String.Format("{0}{1} - Tipo identificación", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "NroDocumento" And li.Value = True Then
                            If String.IsNullOrEmpty(ImportacionEncabezadoTeSelected.NroDocumento) Then
                                strMensajeValidacion = String.Format("{0}{1} - Número de identificación", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "Valor" And li.Value = True Then
                            If IsNothing(ImportacionDetalleTeSelected.Valor) Or ImportacionDetalleTeSelected.Valor = 0 Then
                                strMensajeValidacion = String.Format("{0}{1} - Valor", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        ElseIf li.Key = "FormaPago" And li.Value = True Then
                            If String.IsNullOrEmpty(ImportacionDetalleChequesTeSelected.FormaPagoRC) Then
                                strMensajeValidacion = String.Format("{0}{1} - Forma pago", strMensajeValidacion, vbCrLf)
                                logRespuesta = True
                            End If
                        End If
                    Next
                End If
            End If

            If logRespuesta Then
                strMensajeValidacion = String.Format("Es necesario completar todos los campos :{0}{1}", vbCrLf, strMensajeValidacion)
                mostrarMensaje(strMensajeValidacion, "Carga Masivas Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'Return False
                logRespuesta = False
            Else
                'Return True
                logRespuesta = True
            End If

            Return logRespuesta
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al validar los campos", Me.ToString, "ValidarCamposEditables", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Return False
        End Try
    End Function

    '    '    ''' <summary>
    '    '    ''' Valida los campos editables de la Libranza.
    '    '    ''' Desarrollado por: Jeison Ramirez Pino.
    '    '    ''' </summary>
    Public Sub ValidarCamposEditables(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.CamposEditablesCargaTesorerias) Then
                dcProxyConsulta.CamposEditablesCargaTesorerias.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.CargaMasivaTesoreria_ValidarHabilitarCamposQuery(_TipoDocumentoSeleccionado, _ConsecutivoSeleccionado, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoValidarCamposEditables, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los campos editables", Me.ToString(), "ValidarCamposEditables", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub VolverAtras()
        Try
            VerAtras = Visibility.Collapsed
            VerGrabar = Visibility.Collapsed
            ImportacionEncabezadoTeSelected = Nothing
            ImportacionDetalleTeSelected = Nothing
            ImportacionDetalleChequesTeSelected = Nothing
            CargarContenido(TipoOpcionCargar.ARCHIVO)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema.", Me.ToString(), "VolverAtras", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Sub GrabarCarga()
        Try
            If ValidarIngresoCamposEditables() Then
                IsBusy = True

                If Not IsNothing(dcProxy.tblRespuestaValidacionesCargaMasivas) Then
                    dcProxy.tblRespuestaValidacionesCargaMasivas.Clear()
                End If

                If IsNothing(TipoDocumentoSeleccionado) Then
                    mostrarMensaje("Debe de seleccionar el campo Tipo Tesorería.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If

                If IsNothing(ConsecutivoSeleccionado) Then
                    mostrarMensaje("Debe de seleccionar el consecutivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If

                If TipoDocumentoSeleccionado.ToUpper = "NOTAS" Then
                    ImportacionEncabezadoTeSelected.Documento = Now.Date
                End If

                Dim intIDBanco As Nullable(Of Integer) = Nothing

                If TipoDocumentoSeleccionado.ToUpper = "CAJA" Then
                    intIDBanco = ImportacionDetalleChequesTeSelected.BancoConsignacion
                ElseIf TipoDocumentoSeleccionado.ToUpper = "EGRESOS" Then
                    intIDBanco = ImportacionEncabezadoTeSelected.IDBanco
                Else
                    intIDBanco = Nothing
                End If

                dcProxy.Load(dcProxy.CargaMasivaValidarTesoreriaManualQuery(TipoDocumentoSeleccionado,
                                                                            ConsecutivoSeleccionado,
                                                                            ImportacionEncabezadoTeSelected.Documento,
                                                                            intIDBanco,
                                                                            ImportacionEncabezadoTeSelected.TipoIdentificacion,
                                                                            ImportacionEncabezadoTeSelected.NroDocumento,
                                                                            ImportacionEncabezadoTeSelected.Nombre,
                                                                            ImportacionEncabezadoTeSelected.FormaPagoCE,
                                                                            True,
                                                                            ImportacionEncabezadoTeSelected.Tipocheque,
                                                                            ImportacionEncabezadoTeSelected.CuentaCliente,
                                                                            ImportacionDetalleTeSelected.IDComitente,
                                                                            ImportacionDetalleTeSelected.IDConcepto,
                                                                            ImportacionDetalleTeSelected.Detalle,
                                                                            ImportacionDetalleTeSelected.IDCuentaContable,
                                                                            ImportacionDetalleTeSelected.NIT,
                                                                            ImportacionDetalleTeSelected.CentroCosto,
                                                                            ImportacionDetalleTeSelected.Valor,
                                                                            0,
                                                                            0,
                                                                            ImportacionDetalleChequesTeSelected.BancoGirador,
                                                                            ImportacionDetalleChequesTeSelected.NumCheque,
                                                                            ImportacionDetalleChequesTeSelected.Valor,
                                                                            ImportacionDetalleChequesTeSelected.Consignacion,
                                                                            ImportacionDetalleChequesTeSelected.FormaPagoRC,
                                                                            ImportacionDetalleChequesTeSelected.Comentario,
                                                                            ImportacionDetalleChequesTeSelected.IdProducto,
                                                                            ImportacionDetalleTeSelected.BancoDestino,
                                                                            ImportacionDetalleTeSelected.NombreBancoDestino,
                                                                            ImportacionDetalleTeSelected.CuentaDestino,
                                                                            ImportacionDetalleTeSelected.TipoCuenta,
                                                                            ImportacionDetalleTeSelected.DescripcionTipoCuenta,
                                                                            ImportacionDetalleTeSelected.IdentificacionTitular,
                                                                            ImportacionDetalleTeSelected.Titular,
                                                                            ImportacionDetalleTeSelected.TipoIdTitular,
                                                                            ImportacionDetalleTeSelected.DescripcionTipoIdTitular,
                                                                            ImportacionDetalleTeSelected.FormaEntrega,
                                                                            ImportacionDetalleTeSelected.DescripcionFormaEntrega,
                                                                            ImportacionDetalleTeSelected.Beneficiario,
                                                                            ImportacionDetalleTeSelected.TipoIdentBeneficiario,
                                                                            ImportacionDetalleTeSelected.DescripcionTipoIdentBeneficiario,
                                                                            ImportacionDetalleTeSelected.IdentificacionBenficiciario,
                                                                            ImportacionDetalleTeSelected.NombrePersonaRecoge,
                                                                            ImportacionDetalleTeSelected.IdentificacionPerRecoge,
                                                                            ImportacionDetalleTeSelected.OficinaEntrega,
                                                                            ImportacionDetalleTeSelected.OficinaCuentaInversion,
                                                                            ImportacionDetalleTeSelected.NombreCarteraColectiva,
                                                                            ImportacionDetalleTeSelected.NombreAsesor,
                                                                            Program.Usuario,
                                                                            Program.UsuarioWindows,
                                                                            Program.Maquina, Program.HashConexion), AddressOf TerminoValidarIngreso, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de validar campos habilitados", Me.ToString(), "GrabarCarga", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try


            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim objNewTesoreriSelected As New OyDTesoreria.Tesoreri
            Dim objNewDetalleChequesTeSelected As New OyDTesoreria.Cheque

            objNewTesoreriSelected.Documento = Now.Date
            objNewDetalleChequesTeSelected.Consignacion = Now.Date
            objNewDetalleChequesTeSelected.Valor = 0


            Dim objNewDetalleTesoreriSelected As New OyDTesoreria.DetalleTesoreri
            objNewDetalleTesoreriSelected.Valor = 0

            If TipoDocumentoSeleccionado.ToUpper = "CAJA" Then

            ElseIf TipoDocumentoSeleccionado.ToUpper = "EGRESOS" Then

            Else

            End If


            ImportacionEncabezadoTeSelected = objNewTesoreriSelected
            ImportacionDetalleTeSelected = objNewDetalleTesoreriSelected
            ImportacionDetalleChequesTeSelected = objNewDetalleChequesTeSelected

            listCuentasbancarias = Nothing
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarResultadosImportacion()
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.tblRespuestaValidacionesCargaMasivas) Then
                dcProxyConsulta.tblRespuestaValidacionesCargaMasivas.Clear()
            End If
            ListaResultadoValidacion = Nothing
            dcProxyConsulta.Load(dcProxyConsulta.CargaMasivaTesoreriaConsultarResultadosQuery(TipoDocumentoSeleccionado, ConsecutivoSeleccionado, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarResultado, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los campos editables de la orden.", Me.ToString(), "ValidarCamposEditablesOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub CargarContenido(ByVal pobjTipoOpcionCargar As TipoOpcionCargar)
        Try
            If Not IsNothing(viewCargaMasiva.gridContenido.Children) Then
                viewCargaMasiva.gridContenido.Children.Clear()
            End If

            logRecargarResultados = False
            logConsultoCamposHabilitar = False
            pararTemporizador()

            If pobjTipoOpcionCargar = TipoOpcionCargar.ARCHIVO Then

                HabilitarSeleccionTipoDocumento = True

                If Not String.IsNullOrEmpty(TipoDocumentoSeleccionado) And Not String.IsNullOrEmpty(ConsecutivoSeleccionado) Then
                    Dim objContenidoCargar As CargaMasivaTeImportarArchivoView = New CargaMasivaTeImportarArchivoView(Me)
                    NombreArchivo = String.Empty
                    VerGrabar = Visibility.Collapsed
                    viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)
                End If

            ElseIf pobjTipoOpcionCargar = TipoOpcionCargar.CAMPOS Then

                NuevoRegistro()
                HabilitarSeleccionTipoDocumento = False

                If TipoDocumentoSeleccionado.ToUpper = "CAJA" Then
                    Dim objContenidoCargar As CargaMasivaTeImportar_RecibosView = New CargaMasivaTeImportar_RecibosView(Me)
                    viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)

                ElseIf TipoDocumentoSeleccionado.ToUpper = "EGRESOS" Then
                    Dim objContenidoCargar As CargaMasivaTeImportar_EgresosView = New CargaMasivaTeImportar_EgresosView(Me)
                    viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)
                Else
                    Dim objContenidoCargar As CargaMasivaTeImportar_NotasView = New CargaMasivaTeImportar_NotasView(Me)
                    viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)
                End If


                ValidarCamposEditables()

            ElseIf pobjTipoOpcionCargar = TipoOpcionCargar.RESULTADO Then

                VerAtras = Visibility.Collapsed
                VerGrabar = Visibility.Collapsed

                Dim objContenidoCargar As CargaMasivaTeImportarResultadoView = New CargaMasivaTeImportarResultadoView(Me)
                logRecargarResultados = True
                viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)

            End If



        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar el contenido.", Me.ToString(), "CargarContenido", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ObtenerCompaniaConsecutivo(ByVal pstrNombreConsecutivo As String)
        Try
            If Not IsNothing(DiccionarioCargaTesoreria) Then
                If Not String.IsNullOrEmpty(pstrNombreConsecutivo) And Not String.IsNullOrEmpty(TipoDocumentoSeleccionado) Then
                    If TipoDocumentoSeleccionado.ToUpper = "EGRESOS" Then
                        If DiccionarioCargaTesoreria("CONSECUTIVOEGRESOS").Where(Function(i) i.ID = pstrNombreConsecutivo).Count > 0 Then
                            IDCompaniaConsecutivo = DiccionarioCargaTesoreria("CONSECUTIVOEGRESOS").Where(Function(i) i.ID = pstrNombreConsecutivo).First.intID
                        End If
                    ElseIf TipoDocumentoSeleccionado.ToUpper = "CAJA" Then
                        If DiccionarioCargaTesoreria("CONSECUTIVOCAJA").Where(Function(i) i.ID = pstrNombreConsecutivo).Count > 0 Then
                            IDCompaniaConsecutivo = DiccionarioCargaTesoreria("CONSECUTIVOCAJA").Where(Function(i) i.ID = pstrNombreConsecutivo).First.intID
                        End If
                    Else
                        If DiccionarioCargaTesoreria("CONSECUTIVONOTAS").Where(Function(i) i.ID = pstrNombreConsecutivo).Count > 0 Then
                            IDCompaniaConsecutivo = DiccionarioCargaTesoreria("CONSECUTIVONOTAS").Where(Function(i) i.ID = pstrNombreConsecutivo).First.intID
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar el ID de la compañia.", Me.ToString(), "ObtenerCompaniaConsecutivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

            dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionTesoreriaMasivaQuery(strFormapagoImportacion, NombreArchivo, "ImpTesoreria", TipoDocumentoSeleccionado, ConsecutivoSeleccionado, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar el archivo.", Me.ToString(), "ImportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ConfirmarValoresResultado()
        Try
            logRecargarResultados = False

            If Not IsNothing(ListaResultadoValidacion) Then
                If ListaResultadoValidacion.Count > 0 Then
                    CargarContenido(TipoOpcionCargar.ARCHIVO)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar los resultados.", Me.ToString(), "ConfirmarValoresResultado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = False
        End Try
    End Sub

    Public Sub ConsultarCantidadProcesadas(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.CargaMasivaCantidadProcesadas) Then
                dcProxy.CargaMasivaCantidadProcesadas.Clear()
            End If

            dcProxy.Load(dcProxy.CargaMasivaTesoreria_ConsultarCantidadProcesadosQuery(TipoDocumentoSeleccionado, ConsecutivoSeleccionado, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarCantidadProcesadas, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la cantidad procesadas.", Me.ToString(), "ConsultarCantidadProcesadas", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = False
            IsBusy = False
        End Try
    End Sub

    Public Sub ExportarExcel()
        Try
            If Not String.IsNullOrEmpty(TipoDocumentoSeleccionado) And Not String.IsNullOrEmpty(ConsecutivoSeleccionado) Then
                IsBusy = True
                Dim strParametrosEnviar As String = String.Format("[@pstrTipoDocumento]={0}|[@pstrNombreConsecutivo]={1}|[@pstrUsuario]={2}|[@pstrUsuarioWindows]={3}|[@pstrMaquina]={4}", TipoDocumentoSeleccionado, ConsecutivoSeleccionado, Program.Usuario, Program.UsuarioWindows, Program.Maquina)
                Dim strNombreArchivo = String.Format("ImportacionMasivaTesoreria_{0:yyyyMMddHHmmss}", Now)

                If Not IsNothing(mdcProxyUtilidad.GenerarArchivosPlanos) Then
                    mdcProxyUtilidad.GenerarArchivosPlanos.Clear()
                End If

                mdcProxyUtilidad.Load(mdcProxyUtilidad.GenerarArchivoPlanoQuery("ImpTesoreriaMA", "ImpTesoreriaMA", strParametrosEnviar, strNombreArchivo, "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarArchivoPlanoFondos, String.Empty)
            Else
                mostrarMensaje("Por favor seleccione los campos Tipo Tesorería y Consecutivo tesorería.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la cantidad procesadas.", Me.ToString(), "ConsultarCantidadProcesadas", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = False
            IsBusy = False
        End Try
    End Sub

    Public Sub ConsultarCuentasContablesCliente(ByVal pstrIDComitente As String)
        Try
            mdcProxyUtilidad.ItemCombos.Clear()
            mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosEspecificos_ConUsuarioQuery("Tesoreria_CuentasBancarias", pstrIDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoatraerCuentasbancarias, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las cuentas bancarias.", Me.ToString(), "ConsultarCuentasContablesCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _ImportacionEncabezadoTeSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ImportacionEncabezadoTeSelected.PropertyChanged
        Try
            If e.PropertyName = "FormaPagoCE" Then
                If logConsultoCamposHabilitar Then
                    CambiarColorFondoTextoBuscador()
                End If
            End If
            If e.PropertyName = "CuentaCliente" Then
                If SplitCuenta = False Then

                    If Not String.IsNullOrEmpty(_ImportacionEncabezadoTeSelected.CuentaCliente) Then
                        If _ImportacionEncabezadoTeSelected.CuentaCliente.Contains("|") = True Then
                            Dim Parte = Split(_ImportacionEncabezadoTeSelected.CuentaCliente, "|")
                            cuentaContable = Parte.First
                            TipoCuenta = Parte.Last
                        End If
                    End If
                    If Not String.IsNullOrEmpty(cuentaContable) And Not String.IsNullOrEmpty(TipoCuenta) Then
                        SplitCuenta = True
                        _ImportacionEncabezadoTeSelected.CuentaCliente = cuentaContable

                    End If
                Else
                    If Not String.IsNullOrEmpty(TipoCuenta) Then
                        _ImportacionEncabezadoTeSelected.TipoCuenta = TipoCuenta
                        SplitCuenta = False
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_TesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarResultado(ByVal lo As LoadOperation(Of OyDTesoreria.tblRespuestaValidacionesCargaMasiva))
        Try
            If lo.HasError = False Then
                ListaResultadoValidacion = lo.Entities.ToList

                IsBusy = False

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

    Private Sub TerminoConsultarCantidadProcesadas(ByVal lo As LoadOperation(Of OyDTesoreria.CargaMasivaCantidadProcesadas))
        Try
            If lo.HasError = False Then
                Dim objCantidadProcesadas As OyDTesoreria.CargaMasivaCantidadProcesadas = Nothing
                objCantidadProcesadas = lo.Entities.First

                MensajeCantidadProcesadas = String.Format("Cantidad de total de registros a procesar {1}{0}Cantidad de registros procesados {2}{0}Cantidad de registros pendientes por procesar {3}", vbCrLf, objCantidadProcesadas.CantidadTotalProcesar, objCantidadProcesadas.CantidadProcesados, objCantidadProcesadas.CantidadFaltantes)
                If objCantidadProcesadas.CantidadFaltantes > 0 Then
                    IsbusyResultados = True
                Else
                    IsbusyResultados = False
                End If

                If objCantidadProcesadas.ProcesoActivo Then
                    IsbusyResultados = True
                Else
                    IsbusyResultados = False
                End If

                CargarResultadosImportacion()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoEjecutarConfirmacion", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoEjecutarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper

                        Case "INICIOPROCESOCONRESULTADOS"
                            If objResultado.DialogResult Then
                                logCargarContenido = False


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

    Private Sub TerminoMensajeResultadoAsincronico(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado = CType(sender, A2Utilidades.wcpMensajes)
            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper
                        Case "RESULTADOIMPORTACION"
                            ConsultarCantidadProcesadas(String.Empty)
                    End Select
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje asincronico.", Me.ToString(), "TerminoMensajeResultadoAsincronico", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoValidarIngreso(ByVal lo As LoadOperation(Of OyDTesoreria.tblRespuestaValidacionesCargaMasiva))
        Try
            If lo.HasError = False Then

                ListaResultadoValidacion = lo.Entities.ToList

                If ListaResultadoValidacion.Count > 0 Then
                    Dim logExitoso As Boolean = False
                    Dim strMensajeExitoso As String = "Se realizó el proceso de carga exitosamente"
                    Dim strMensajeError As String = "El proceso de carga no se realizó"
                    Dim logError As Boolean = False
                    Dim logAdvertencia As Boolean = False 'DEMC20190926 S-42805

                    For Each li In ListaResultadoValidacion
                        If li.Exitoso Then
                            logExitoso = True
                            strMensajeExitoso = String.Format("{0}{1} {2}", strMensajeExitoso, vbCrLf, li.Mensaje)
                        ElseIf li.DetieneIngreso = 1 Then 'DEMC20190926 S-42805
                            logError = True
                            logExitoso = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, "Fila " & li.Fila & ": " & li.Mensaje)
                        ElseIf li.DetieneIngreso = 2 Then
                            logExitoso = True
                            logError = False
                            logAdvertencia = True
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, "Fila " & li.Fila & ": " & li.Mensaje)
                        Else
                            logError = True
                            logExitoso = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, "Fila " & li.Fila & ": " & li.Mensaje)

                        End If
                    Next

                    If logExitoso = True And logError = False Then
                        If TipoDocumentoSeleccionado.ToUpper <> "NOTAS" Then
                            strMensajeExitoso = strMensajeExitoso.Replace("++", String.Format("{0}      -> ", vbCrLf))
                            strMensajeExitoso = strMensajeExitoso.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                            strMensajeExitoso = strMensajeExitoso.Replace("|", String.Format("{0}   -> ", vbCrLf))
                            strMensajeExitoso = strMensajeExitoso.Replace("--", String.Format("{0}", vbCrLf))
                            'DEMC20190926 S-42805 INICIO
                            If logAdvertencia = True And logError = False Then
                                If Not String.IsNullOrEmpty(strMensajeError) Then
                                    Dim objListaMensajes As New List(Of String)
                                    Dim ViewImportarArchivo As New ResultadoGenericoImportaciones()

                                    objListaMensajes.Add(strMensajeError)

                                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                                    ViewImportarArchivo.IsBusy = False
                                    ViewImportarArchivo.Title = "Registros no procesados"
                                    ViewImportarArchivo.Show()
                                End If
                            End If
                            mostrarMensajeResultadoAsincronico(strMensajeExitoso, "Carga Masiva ", AddressOf TerminoMensajeResultadoAsincronico, "RESULTADOIMPORTACION", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        End If

                        CargarContenido(TipoOpcionCargar.RESULTADO)

                        'DEMC20190926 S-42805 FIN
                    ElseIf logError Then
                        Dim objListaMensajes As New List(Of String)
                        Dim ViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()

                        'For Each li In ListaResultadoValidacion.Where(Function(i) i.DetieneIngreso = True).ToList
                        '    objListaMensajes.Add(li.Mensaje)
                        'Next

                        objListaMensajes.Add(strMensajeError)

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                        ViewImportarArchivo.Title = "Complementación importación"
                        Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                        ViewImportarArchivo.ShowDialog()
                        IsBusy = False
                    End If
                End If
            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngreso", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngreso", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)

            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoValidarCamposEditables(ByVal lo As LoadOperation(Of OyDTesoreria.CamposEditablesCargaTesoreria))
        Try
            If lo.HasError = False Then
                ListaComboCamposPermitidosEdicion = lo.Entities.ToList

                Dim objDiccionarioCamposEditables As New Dictionary(Of String, Boolean)

                For Each li In ListaComboCamposPermitidosEdicion
                    If Not objDiccionarioCamposEditables.ContainsKey(li.NombreCampo) Then
                        objDiccionarioCamposEditables.Add(li.NombreCampo, li.PermiteEditar)
                    End If
                Next

                DiccionarioEdicionCampos = objDiccionarioCamposEditables

                CambiarColorFondoTextoBuscador()
                logConsultoCamposHabilitar = True
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar los campos editables.", Me.ToString(), "TerminoValidarCamposEditables", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar los campos editables.", Me.ToString(), "TerminoValidarCamposEditables", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCargarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim ViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim logContinuar As Boolean = False

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    Dim objTipo As String = String.Empty

                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                        objListaMensajes.Add("El archivo generó algunas inconsistencias al intentar subirlo:")
                    End If

                    For Each li In objListaRespuesta.OrderBy(Function(o) o.Tipo)
                        If objTipo <> li.Tipo And li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                            objTipo = li.Tipo
                        ElseIf li.Tipo = "REGISTROSIMPORTADOS" Then
                            If li.Columna > 0 Then
                                logContinuar = True
                            End If
                        End If

                        If li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                            objListaMensajes.Add(String.Format("{0}: {1} - Campo {2} - Validación: {3}", IIf(TipoDocumentoSeleccionado.ToUpper = "NOTAS", "Secuencia", "Fila"), li.Fila, li.Campo, li.Mensaje))
                        Else
                            objListaMensajes.Add(li.Mensaje)
                        End If
                    Next

                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo"
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo"
                End If

                Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                ViewImportarArchivo.ShowDialog()

                If logContinuar Then
                    If TipoDocumentoSeleccionado.ToUpper = "NOTAS" Then
                        NuevoRegistro()
                        GrabarCarga()
                    Else
                        VerAtras = Visibility.Visible
                        CargarContenido(TipoOpcionCargar.CAMPOS)
                        VerGrabar = Visibility.Visible
                    End If
                Else
                    VerGrabar = Visibility.Collapsed
                End If
                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir el archivo de carga de tesorería.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir el archivo de carga de tesorería.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub TerminoatraerCuentasbancarias(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                Dim objListaCuentasBancarias As New List(Of OYDUtilidades.ItemCombo)
                If mdcProxyUtilidad.ItemCombos.Count > 0 Then


                    If Not listCuentasbancarias Is Nothing Then
                        listCuentasbancarias.Clear()
                    End If

                    For Each li In mdcProxyUtilidad.ItemCombos.Where(Function(y) y.Categoria = "CuentasBancarias").ToList
                        objListaCuentasBancarias.Add(li)
                    Next
                End If
                listCuentasbancarias = objListaCuentasBancarias
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las cuentas bancarias.", _
                                     Me.ToString(), "TerminoatraerCuentasbancarias", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar las cuentas bancarias.", _
                                       Me.ToString(), "TerminoatraerCuentasbancarias", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Timer Refrescar pantalla"

    Private _myDispatcherTimerTesoreria As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    Public Sub ReiniciaTimer()
        Try
            If _myDispatcherTimerTesoreria Is Nothing Then
                _myDispatcherTimerTesoreria = New System.Windows.Threading.DispatcherTimer
                _myDispatcherTimerTesoreria.Interval = New TimeSpan(0, 0, 15)
                AddHandler _myDispatcherTimerTesoreria.Tick, AddressOf Me.Each_Tick
            End If
            _myDispatcherTimerTesoreria.Start()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrió un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador()
        Try
            If Not IsNothing(_myDispatcherTimerTesoreria) Then
                _myDispatcherTimerTesoreria.Stop()
                RemoveHandler _myDispatcherTimerTesoreria.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerTesoreria = Nothing
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

End Class