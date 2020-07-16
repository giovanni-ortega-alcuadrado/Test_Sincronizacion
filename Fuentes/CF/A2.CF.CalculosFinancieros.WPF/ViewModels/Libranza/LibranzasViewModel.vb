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
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Threading.Tasks

Public Class LibranzasViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            dcProxy = inicializarProxyCalculosFinancieros()
            dcProxy1 = inicializarProxyCalculosFinancieros()
            mdcProxyUtilidad = inicializarProxyUtilidadesOYD()

            If System.Diagnostics.Debugger.IsAttached Then

            End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de registro que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                IsBusy = True
                viewFormaLibranzas = New FormaLibranzasView(Me)

                Dim objDiccionarioHabilitarCampos = New Dictionary(Of String, Boolean)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS.ToString, False)

                DiccionarioHabilitarCampos = objDiccionarioHabilitarCampos

                CambiarColorFondoTextoBuscador()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "OperacionesOtrosNegociosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function inicializar() As Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                'Consulta los combos de la pantalla.
                Await CargarCombos(True, OPCION_INICIO)

                Await RecargarPantalla()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

    'Private Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String)
    '    Dim objRetorno As String = String.Empty

    '    If Not String.IsNullOrEmpty(strProgram) Then
    '        objRetorno = strProgram
    '    Else
    '        objRetorno = strRetornoOpcional
    '    End If

    '    Return objRetorno
    'End Function

    Private Sub CambiarValor_OpcionHabilitarCampos(ByVal pobjOpcion As OPCIONES_HABILITARCAMPOS, ByVal plogValorNuevo As Boolean)
        Try
            If Not IsNothing(pobjOpcion) Then
                If DiccionarioHabilitarCampos.ContainsKey(pobjOpcion.ToString) Then
                    DiccionarioHabilitarCampos(pobjOpcion.ToString) = plogValorNuevo
                    MyBase.CambioItem("DiccionarioHabilitarCampos")
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar las opciones.", Me.ToString(), "CambiarValor_OpcionHabilitarCampos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

#Region "Variables"

    Private dcProxy As CalculosFinancierosDomainContext
    Private dcProxy1 As CalculosFinancierosDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext

    Public logNuevoRegistro As Boolean = False
    Public logEditarRegistro As Boolean = False
    Public logCancelarRegistro As Boolean = False
    Public logDuplicarRegistro As Boolean = False

    Dim viewFormaLibranzas As FormaLibranzasView = Nothing

    Dim dtmFechaServidor As DateTime
    Dim logCambiarSelected As Boolean = True
    Dim logCambiarValoresEnSelected As Boolean = True
    Dim logCargoForma As Boolean = False
    Dim logRecalcularFlujos As Boolean = False
    Public logCalcularValores As Boolean = True
    Public strTipoCalculo As String

#End Region

#Region "Constantes"
    Private TOOLBARACTIVOPANTALLA As String = "A2Consola_ToolbarActivo"
    Private MINT_LONG_MAX_CODIGO_OYD As Integer = 17

    Public TIPOREGISTRO_MANUAL As String = "M"
    Public TIPOREGISTRO_AUTOMATICO As String = "A"

    Private ESTADO_PENDIENTE As String = "P"
    Private ESTADO_ANULADO As String = "A"
    Private ESTADO_VALORADO As String = "V"

    Private Const OPCION_INICIO As String = "INICIO"
    Private Const OPCION_NUEVO As String = "NUEVO"
    Private Const OPCION_EDITAR As String = "EDITAR"
    Private Const OPCION_ANULAR As String = "ANULAR"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    Private Const OPCION_ACTUALIZAR As String = "ACTUALIZAR"

    Private Const COLOR_DESHABILITADO As String = "#E2E2E2"
    Private Const COLOR_HABILITADO As String = "#FFFFFFFF"

    Public Enum OPCIONES_HABILITARCAMPOS
        HABILITARENCABEZADO
        HABILITARNEGOCIO
        HABILITARFLUJOS
    End Enum

#End Region

#Region "Propiedades para el Tipo de registro"

    Private _ListaEncabezado As List(Of CFCalculosFinancieros.Libranzas)
    Public Property ListaEncabezado() As List(Of CFCalculosFinancieros.Libranzas)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CFCalculosFinancieros.Libranzas))
            _ListaEncabezado = value
            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaged")
            If Not IsNothing(_ListaEncabezado) Then
                If _ListaEncabezado.Count > 0 And logCambiarSelected Then
                    EncabezadoSeleccionado = _ListaEncabezado.FirstOrDefault
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaEncabezadoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezado) Then
                Dim view = New PagedCollectionView(_ListaEncabezado)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _EncabezadoDataForm As CFCalculosFinancieros.Libranzas = New CFCalculosFinancieros.Libranzas
    Public Property EncabezadoDataForm() As CFCalculosFinancieros.Libranzas
        Get
            Return _EncabezadoDataForm
        End Get
        Set(ByVal value As CFCalculosFinancieros.Libranzas)
            _EncabezadoDataForm = value
            MyBase.CambioItem("EncabezadoDataForm")
        End Set
    End Property

    Private WithEvents _EncabezadoSeleccionado As CFCalculosFinancieros.Libranzas
    Public Property EncabezadoSeleccionado() As CFCalculosFinancieros.Libranzas
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CFCalculosFinancieros.Libranzas)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If logCambiarValoresEnSelected Then
                    If _EncabezadoSeleccionado.intID > 0 Then
                        ConsultarDetalle(_EncabezadoSeleccionado.intID)
                    End If

                    MyBase.CambioItem("DiccionarioCombos")

                    HabilitarCamposRegistro(_EncabezadoSeleccionado)
                End If
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private _EncabezadoSeleccionadoAnterior As CFCalculosFinancieros.Libranzas
    Public Property EncabezadoSeleccionadoAnterior() As CFCalculosFinancieros.Libranzas
        Get
            Return _EncabezadoSeleccionadoAnterior
        End Get
        Set(ByVal value As CFCalculosFinancieros.Libranzas)
            _EncabezadoSeleccionadoAnterior = value
        End Set
    End Property

    Private _ViewLibranzas As LibranzasView
    Public Property ViewLibranzas() As LibranzasView
        Get
            Return _ViewLibranzas
        End Get
        Set(ByVal value As LibranzasView)
            _ViewLibranzas = value
        End Set
    End Property

    Private _BusquedaLibranzas As CamposBusquedaLibranzas = New CamposBusquedaLibranzas
    Public Property BusquedaLibranzas() As CamposBusquedaLibranzas
        Get
            Return _BusquedaLibranzas
        End Get
        Set(ByVal value As CamposBusquedaLibranzas)
            _BusquedaLibranzas = value
            MyBase.CambioItem("BusquedaLibranzas")
        End Set
    End Property

    Private _FondoTextoBuscadores As SolidColorBrush
    Public Property FondoTextoBuscadores() As SolidColorBrush
        Get
            Return _FondoTextoBuscadores
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadores = value
            MyBase.CambioItem("FondoTextoBuscadores")
        End Set
    End Property


#End Region

#Region "Propiedades Flujos"

    Private _ListaFlujos As List(Of CFCalculosFinancieros.Libranzas_Flujos)
    Public Property ListaFlujos() As List(Of CFCalculosFinancieros.Libranzas_Flujos)
        Get
            Return _ListaFlujos
        End Get
        Set(ByVal value As List(Of CFCalculosFinancieros.Libranzas_Flujos))
            _ListaFlujos = value
            MyBase.CambioItem("ListaFlujos")
            MyBase.CambioItem("ListaFlujosPaged")
            If Not IsNothing(_ListaFlujos) Then
                If _ListaFlujos.Count > 0 Then
                    FlujoSeleccionado = _ListaFlujos.FirstOrDefault
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaFlujosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaFlujos) Then
                Dim view = New PagedCollectionView(_ListaFlujos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _FlujoSeleccionado As CFCalculosFinancieros.Libranzas_Flujos
    Public Property FlujoSeleccionado() As CFCalculosFinancieros.Libranzas_Flujos
        Get
            Return _FlujoSeleccionado
        End Get
        Set(ByVal value As CFCalculosFinancieros.Libranzas_Flujos)
            _FlujoSeleccionado = value
            MyBase.CambioItem("FlujoSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades para cargar Información de los Combos"

    Private _DiccionarioCombos As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))
    Public Property DiccionarioCombos() As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))
        Get
            Return _DiccionarioCombos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos)))
            _DiccionarioCombos = value
            MyBase.CambioItem("DiccionarioCombos")
        End Set
    End Property

    Private _DiccionarioCombosCompleta As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))
    Public Property DiccionarioCombosCompleta() As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))
        Get
            Return _DiccionarioCombosCompleta
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos)))
            _DiccionarioCombosCompleta = value
            MyBase.CambioItem("DiccionarioCombosCompleta")
        End Set
    End Property

    Private _DiccionarioHabilitarCampos As Dictionary(Of String, Boolean)
    Public Property DiccionarioHabilitarCampos() As Dictionary(Of String, Boolean)
        Get
            Return _DiccionarioHabilitarCampos
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            _DiccionarioHabilitarCampos = value
            MyBase.CambioItem("DiccionarioHabilitarCampos")
        End Set
    End Property

#End Region

#Region "Propiedades para habilitar los controles de la pantalla"

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

    Private _IsBusyCalculos As Boolean
    Public Property IsBusyCalculos() As Boolean
        Get
            Return _IsBusyCalculos
        End Get
        Set(ByVal value As Boolean)
            _IsBusyCalculos = value
            MyBase.CambioItem("IsBusyCalculos")
        End Set
    End Property

#End Region

#Region "Propiedades del cliente"

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionado = value
            Try
                If logCancelarRegistro = False Then
                    SeleccionarCliente(_ComitenteSeleccionado)
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad del cliente seleccionado.", Me.ToString, "ComitenteSeleccionado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try

            MyBase.CambioItem("ComitenteSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades genericas"

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

#End Region

#Region "Propiedades decimales"

    Private _FormatoCamposDecimales As String = "2"
    Public Property FormatoCamposDecimales() As String
        Get
            Return _FormatoCamposDecimales
        End Get
        Set(ByVal value As String)
            _FormatoCamposDecimales = value
            MyBase.CambioItem("FormatoCamposDecimales")
        End Set
    End Property

    Private _FormatoCamposNumericos As String = "2"
    Public Property FormatoCamposNumericos() As String
        Get
            Return _FormatoCamposNumericos
        End Get
        Set(ByVal value As String)
            _FormatoCamposNumericos = value
            MyBase.CambioItem("FormatoCamposNumericos")
        End Set
    End Property


#End Region

#End Region

#Region "Metodos"

    Public Overrides Async Sub NuevoRegistro()
        Try
            If logCargoForma = False Then
                ViewLibranzas.GridEdicion.Children.Add(viewFormaLibranzas)
                logCargoForma = True
            End If

            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            dtmFechaServidor = Await ObtenerFechaHoraServidor()

            If Not IsNothing(_EncabezadoSeleccionado) Then
                EncabezadoSeleccionadoAnterior = Nothing
                ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)
            End If

            logNuevoRegistro = True
            logEditarRegistro = False
            logDuplicarRegistro = False
            logCancelarRegistro = False

            NuevaLibranza()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub NuevaLibranza()
        Try
            Dim objNewLibranzas As New CFCalculosFinancieros.Libranzas
            objNewLibranzas.intID = 0
            objNewLibranzas.strEstado = ESTADO_PENDIENTE
            objNewLibranzas.strDescripcionEstado = "Pendiente"
            objNewLibranzas.dtmFechaRegistro = dtmFechaServidor.Date
            objNewLibranzas.strNroCredito = String.Empty
            objNewLibranzas.lngIDComitente = String.Empty
            objNewLibranzas.strNombreCliente = String.Empty
            objNewLibranzas.strCodTipoIdentificacionCliente = String.Empty
            objNewLibranzas.strTipoIdentificacionCliente = String.Empty
            objNewLibranzas.strNumeroDocumentoCliente = String.Empty
            objNewLibranzas.intIDCompania = Nothing
            objNewLibranzas.strNombreCompania = String.Empty
            objNewLibranzas.intIDEmisor = Nothing
            objNewLibranzas.strNombreEmisor = String.Empty
            objNewLibranzas.strCodTipoIdentificacionEmisor = String.Empty
            objNewLibranzas.strTipoIdentificacionEmisor = String.Empty
            objNewLibranzas.strNumeroDocumentoEmisor = String.Empty
            objNewLibranzas.dtmFechaInicioCredito = dtmFechaServidor.Date
            objNewLibranzas.dtmFechaFinCredito = dtmFechaServidor.Date
            objNewLibranzas.intNroCuotas = 0
            objNewLibranzas.dblValorCuotas = 0
            objNewLibranzas.strPeriodoPago = String.Empty
            objNewLibranzas.strDescripcionPeriodoPago = String.Empty
            objNewLibranzas.dblValorCreditoOriginal = 0
            objNewLibranzas.strTipoPago = String.Empty
            objNewLibranzas.strDescripcionTipoPago = String.Empty
            objNewLibranzas.dblTasaInteresCredito = 0
            objNewLibranzas.dblTasaDescuento = 0
            objNewLibranzas.dblValorOperacion = 0
            objNewLibranzas.dblValorCreditoActual = 0
            objNewLibranzas.strNroDocumentoBeneficiario = String.Empty
            objNewLibranzas.strNombreBeneficiario = String.Empty
            objNewLibranzas.strTipoIdentificacionBeneficiario = String.Empty
            objNewLibranzas.strDescripcionTipoIdentificacionBeneficiario = String.Empty
            objNewLibranzas.intIDPagador = 0
            objNewLibranzas.strNombrePagador = String.Empty
            objNewLibranzas.strCodTipoIdentificacionPagador = String.Empty
            objNewLibranzas.strTipoIdentificacionPagador = String.Empty
            objNewLibranzas.strNumeroDocumentoPagador = String.Empty
            objNewLibranzas.intIDCustodio = 0
            objNewLibranzas.strNombreCustodio = String.Empty
            objNewLibranzas.strCodTipoIdentificacionCustodio = String.Empty
            objNewLibranzas.strTipoIdentificacionCustodio = String.Empty
            objNewLibranzas.strNumeroDocumentoCustodio = String.Empty
            objNewLibranzas.dtmFechaValoracion = Nothing
            objNewLibranzas.dtmFechaRegistroSistema = Now
            objNewLibranzas.strTipoRegistro = TIPOREGISTRO_MANUAL
            objNewLibranzas.strDescripcionTipoRegistro = "Manual"
            objNewLibranzas.dtmActualizacion = Now
            objNewLibranzas.strUsuario = Program.Usuario
            objNewLibranzas.strObservaciones = String.Empty

            Editando = True

            ObtenerValoresRegistroAnterior(objNewLibranzas, EncabezadoSeleccionado)

            FlujoSeleccionado = Nothing
            If Not IsNothing(ListaFlujos) Then
                Dim objListaFlujos As New List(Of CFCalculosFinancieros.Libranzas_Flujos)
                ListaFlujos = Nothing
                ListaFlujos = objListaFlujos
            End If

            'BuscarControlValidacion(viewFormaLibranzas, "tabItemValoresComisiones")
            'Se posiciona en el primer registro
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, False)

            CambiarColorFondoTextoBuscador()
            logRecalcularFlujos = True

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevaLibranza", Program.TituloSistema, Program.Maquina, ex)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, False)

            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)
            Editando = False
        End Try
    End Sub

    Public Async Sub PreguntarDuplicarRegistro()
        Try
            If logCargoForma = False Then
                ViewLibranzas.GridEdicion.Children.Add(viewFormaLibranzas)
                logCargoForma = True
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                Await validarEstadoRegistro(OPCION_DUPLICAR)
            Else
                mostrarMensaje("Debe de seleccionar un registro para duplicar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preguntar sí se deseaba duplicar el registro", Me.ToString(), "PreguntarDuplicarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub DuplicarRegistro()
        Try
            EncabezadoSeleccionadoAnterior = Nothing

            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)
            dtmFechaServidor = Await ObtenerFechaHoraServidor()

            'Crea el nuevo registro para duplicar.
            Dim objNewRegistroDuplicar As New CFCalculosFinancieros.Libranzas
            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, objNewRegistroDuplicar)

            objNewRegistroDuplicar.intID = 0
            objNewRegistroDuplicar.dtmActualizacion = dtmFechaServidor
            objNewRegistroDuplicar.strUsuario = Program.Usuario
            objNewRegistroDuplicar.strTipoRegistro = TIPOREGISTRO_MANUAL
            objNewRegistroDuplicar.strDescripcionTipoRegistro = "Manual"
            objNewRegistroDuplicar.dblValorCuotas = 0
            objNewRegistroDuplicar.dblValorCreditoActual = 0
            objNewRegistroDuplicar.dblValorOperacion = 0
            objNewRegistroDuplicar.dtmFechaFinCredito = dtmFechaServidor.Date

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = True
            logNuevoRegistro = False

            ObtenerValoresRegistroAnterior(objNewRegistroDuplicar, EncabezadoSeleccionado)

            FlujoSeleccionado = Nothing
            If Not IsNothing(ListaFlujos) Then
                Dim objListaFlujos As New List(Of CFCalculosFinancieros.Libranzas_Flujos)
                ListaFlujos = Nothing
                ListaFlujos = objListaFlujos
            End If

            Editando = True
            IsBusy = False
            IsBusyCalculos = False

            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, True)
            CambiarColorFondoTextoBuscador()

            logRecalcularFlujos = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro duplicado", Me.ToString(), "DuplicarRegistro", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, False)

            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)
        End Try
    End Sub

    Public Overrides Async Sub Filtrar()
        Try
            IsBusy = True

            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                Await ConsultarEncabezado(True, TextoFiltroSeguro)
            Else
                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            OrganizarNuevaBusqueda()
            MyBase.Buscar()
            'MyBase.CambioItem("visBuscando")
            'MyBase.CambioItem("visNavegando")
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, False)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación de la busqueda", Me.ToString(), "Buscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
    End Sub

    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(BusquedaLibranzas) Then
                IsBusy = True

                Await ConsultarEncabezado(False, String.Empty, False, 0, _BusquedaLibranzas.ID, _BusquedaLibranzas.Comitente, _BusquedaLibranzas.FechaRegistro, _BusquedaLibranzas.Emisor, _BusquedaLibranzas.Pagador, _BusquedaLibranzas.Custodio, _BusquedaLibranzas.NroCredito, _BusquedaLibranzas.TipoRegistro)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConfirmarBuscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal plogPosicionarRegistro As Boolean = False,
                                               Optional ByVal pintIDRegistroPosicionar As Integer = 0,
                                               Optional ByVal pintIDLibranza As Nullable(Of Integer) = Nothing,
                                               Optional ByVal pstrIDComitente As String = "",
                                               Optional ByVal pdtmFechaRegistro As Nullable(Of DateTime) = Nothing,
                                               Optional ByVal pintIDEmisor As Nullable(Of Integer) = Nothing,
                                               Optional ByVal pintIDPagador As Nullable(Of Integer) = Nothing,
                                               Optional ByVal pintIDCustodio As Nullable(Of Integer) = Nothing,
                                               Optional ByVal pstrNroCredito As String = "",
                                               Optional ByVal pstrTipoRegistro As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Libranzas)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Libranzas.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await dcProxy.Load(dcProxy.Libranzas_FiltrarSyncQuery(ESTADO_PENDIENTE, pstrFiltro, Program.Usuario, Program.HashConexion)).AsTask()
            Else
                objRet = Await dcProxy.Load(dcProxy.Libranzas_ConsultarSyncQuery(pintIDLibranza, pstrIDComitente, pdtmFechaRegistro, pintIDEmisor, pintIDPagador, pintIDCustodio, pstrNroCredito, pstrTipoRegistro, Program.Usuario, Program.HashConexion)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los registros.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If plogPosicionarRegistro Then
                        logCambiarSelected = False
                    End If

                    ListaEncabezado = objRet.Entities.ToList

                    If plogPosicionarRegistro Then
                        logCambiarSelected = True
                    End If

                    If objRet.Entities.Count > 0 Then
                        If Not plogFiltrar Then
                            MyBase.ConfirmarBuscar()
                        End If

                        If plogPosicionarRegistro And Not IsNothing(pintIDRegistroPosicionar) Then
                            If _ListaEncabezado.Where(Function(i) i.intID = pintIDRegistroPosicionar).Count > 0 Then
                                EncabezadoSeleccionado = _ListaEncabezado.Where(Function(i) i.intID = pintIDRegistroPosicionar).First
                            End If
                        End If

                        ReiniciaTimer()
                    Else
                        If Not plogFiltrar Or (plogFiltrar And Not pstrFiltro.Equals(String.Empty)) Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                ListaEncabezado.Clear()
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Async Function ConsultarDetalle(ByVal pintIDLibranza As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Libranzas_Flujos)

        Try
            ErrorForma = String.Empty

            dcProxy.Libranzas_Flujos.Clear()

            objRet = Await dcProxy.Load(dcProxy.Libranzas_FlujosConsultarSyncQuery(pintIDLibranza, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el de talle de la distribución.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de los flujos.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaFlujos = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de los flujos.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Overrides Async Sub ActualizarRegistro()
        Try
            IsBusy = True
            dtmFechaServidor = Await ObtenerFechaHoraServidor()
            IsBusy = True

            Dim logCalcularValorRegistro As Boolean = True

            If logEditarRegistro And logRecalcularFlujos = False Then
                logCalcularValorRegistro = False
            End If

            If logCalcularValorRegistro Then
                If Await CalcularValorRegistro() Then
                    Await ActualizarLibranzas(_EncabezadoSeleccionado)
                Else
                    IsBusy = False
                End If
            Else
                Await ActualizarLibranzas(_EncabezadoSeleccionado)
            End If
            
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ActualizarLibranzas(ByVal objRegistroSelected As CFCalculosFinancieros.Libranzas) As Task
        Try
            IsBusy = True

            If Not IsNothing(objRegistroSelected) Then

                If ValidarGuardadoRegistro(objRegistroSelected) Then
                    Await GuardarLibranzas(objRegistroSelected)
                Else
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "ActualizarLibranzas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Private Async Function GuardarLibranzas(ByVal objRegistroSelected As CFCalculosFinancieros.Libranzas) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            Dim strFlujosXML As String = String.Empty

            If Not IsNothing(_ListaFlujos) Then
                For Each li In _ListaFlujos
                    If li.logModificoFecha Then
                        If String.IsNullOrEmpty(strFlujosXML) Then
                            strFlujosXML = String.Format("{0},{1:yyyy-MM-dd}", li.intID, li.dtmFechaFlujo)
                        Else
                            strFlujosXML = String.Format("{0}|{1},{2:yyyy-MM-dd}", strFlujosXML, li.intID, li.dtmFechaFlujo)
                        End If
                    End If
                Next
            End If

            strFlujosXML = System.Web.HttpUtility.UrlEncode(strFlujosXML)
            Dim strObservaciones As String = System.Web.HttpUtility.UrlEncode(_EncabezadoSeleccionado.strObservaciones)

            dcProxy.Libranzas_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.Libranzas_ValidarSyncQuery(_EncabezadoSeleccionado.intID,
                                                                           _EncabezadoSeleccionado.dtmFechaRegistro,
                                                                           _EncabezadoSeleccionado.strNroCredito,
                                                                           _EncabezadoSeleccionado.lngIDComitente,
                                                                           _EncabezadoSeleccionado.intIDCompania,
                                                                           _EncabezadoSeleccionado.intIDEmisor,
                                                                           _EncabezadoSeleccionado.dtmFechaInicioCredito,
                                                                           _EncabezadoSeleccionado.dtmFechaFinCredito,
                                                                           _EncabezadoSeleccionado.intNroCuotas,
                                                                           _EncabezadoSeleccionado.dblValorCuotas,
                                                                           _EncabezadoSeleccionado.strPeriodoPago,
                                                                           _EncabezadoSeleccionado.dblValorCreditoOriginal,
                                                                           _EncabezadoSeleccionado.strTipoPago,
                                                                           _EncabezadoSeleccionado.dblTasaInteresCredito,
                                                                           _EncabezadoSeleccionado.dblTasaDescuento,
                                                                           _EncabezadoSeleccionado.dblValorOperacion,
                                                                           _EncabezadoSeleccionado.dblValorCreditoActual,
                                                                           _EncabezadoSeleccionado.strNroDocumentoBeneficiario,
                                                                           _EncabezadoSeleccionado.strNombreBeneficiario,
                                                                           _EncabezadoSeleccionado.strTipoIdentificacionBeneficiario,
                                                                           _EncabezadoSeleccionado.intIDPagador,
                                                                           _EncabezadoSeleccionado.intIDCustodio,
                                                                           _EncabezadoSeleccionado.strTipoRegistro,
                                                                           strObservaciones,
                                                                           strFlujosXML,
                                                                           Program.Usuario,
                                                                           logRecalcularFlujos, Program.HashConexion)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarLibranzas", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                    IsBusy = False
                Else
                    Dim objListaResultadoValidacion As List(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion) = objRet.Entities.ToList

                    If objListaResultadoValidacion.Count > 0 Then
                        Dim logExitoso As Boolean = False
                        Dim logContinuaMostrandoMensaje As Boolean = False
                        Dim logRequiereConfirmacion As Boolean = False
                        Dim logRequiereJustificacion As Boolean = False
                        Dim logRequiereAprobacion As Boolean = False
                        Dim logConsultaListaJustificacion As Boolean = False
                        Dim logError As Boolean = False
                        Dim strMensajeExitoso As String = "El registro se actualizó correctamente."
                        Dim strMensajeError As String = "El registro no se pudo actualizar."
                        Dim logEsHtml As Boolean = False
                        Dim strMensajeDetallesHtml As String = String.Empty
                        Dim strMensajeRetornoHtml As String = String.Empty
                        Dim intIDInsertado As Integer = 0

                        For Each li In objListaResultadoValidacion
                            If li.Exitoso Then
                                logExitoso = True
                                logError = False
                                logContinuaMostrandoMensaje = False
                                logRequiereJustificacion = False
                                logRequiereConfirmacion = False
                                logRequiereAprobacion = False
                                strMensajeExitoso = String.Format("{0}{1} {2}", strMensajeExitoso, vbCrLf, li.Mensaje)
                                If Not IsNothing(li.IDRegistroIdentity) Then
                                    intIDInsertado = CInt(li.IDRegistroIdentity)
                                End If
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

                            If _EncabezadoSeleccionado.intID <> intIDInsertado Then
                                _EncabezadoSeleccionado.intID = intIDInsertado
                                RecargarOrdenDespuesGuardado(True, strMensajeExitoso)
                            Else
                                RecargarOrdenDespuesGuardado(False, strMensajeExitoso)
                            End If

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

                            mostrarMensaje(strMensajeError, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, String.Empty, "Reglas incumplidas en los detalles de la operación", logEsHtml, strMensajeDetallesHtml)
                            IsBusy = False
                        End If
                    End If
                End If
            Else
                IsBusy = False
            End If

            logResultado = True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro ", Me.ToString(), "GuardarLibranzas", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Overrides Async Sub TerminoSubmitChanges(ByVal so As SubmitOperation)
        Try
            MyBase.TerminoSubmitChanges(so)

            mostrarMensaje(so.UserState.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

            ObtenerInformacionCombosCompletos()

            Await ConsultarEncabezado(True, String.Empty, True, _EncabezadoSeleccionado.intID)

            Editando = False
            IsBusy = False

            CambiarColorFondoTextoBuscador()

            HabilitarCamposRegistro(_EncabezadoSeleccionado)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub EditarRegistro()
        Try
            If logCargoForma = False Then
                ViewLibranzas.GridEdicion.Children.Add(viewFormaLibranzas)
                logCargoForma = True
            End If

            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.intID <> 0 Then
                    dtmFechaServidor = Await ObtenerFechaHoraServidor()

                    Await validarEstadoRegistro(OPCION_EDITAR)
                Else
                    MyBase.RetornarValorEdicionNavegacion()
                    mostrarMensaje("No se ha seleccionado ningun registro para editar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
                mostrarMensaje("No se ha seleccionado ningun registro para editar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            Me.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar editar el registro.", Me.ToString(), "EditarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub EditarLibranzas()
        Try
            EncabezadoSeleccionadoAnterior = Nothing

            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logNuevoRegistro = False

            Editando = True

            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, True)
            CambiarColorFondoTextoBuscador()

            logRecalcularFlujos = False

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro.", Me.ToString(), "EditarLibranzas", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Function validarEstadoRegistro(ByVal pstrAccion As String) As Task
        Try
            Dim strMsg As String = String.Empty

            If _EncabezadoSeleccionado.strEstado <> ESTADO_PENDIENTE And pstrAccion <> OPCION_DUPLICAR Then
                MyBase.RetornarValorEdicionNavegacion()
                strMsg = String.Format("No se puede {0} el registro porque se encuentra en estado {1}", IIf(pstrAccion = OPCION_EDITAR, "Editar", "Anular"), IIf(_EncabezadoSeleccionado.strEstado = ESTADO_ANULADO, "Anulada", "Pendiente por aprobar"))
                mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.IsBusy = False
            Else
                Me.IsBusy = True
                If pstrAccion = OPCION_EDITAR Then
                    If Await ValidarEstadoLibranzas(_EncabezadoSeleccionado, "editar") Then
                        EditarLibranzas()
                    Else
                        MyBase.RetornarValorEdicionNavegacion()
                        IsBusy = False
                    End If
                ElseIf pstrAccion = OPCION_DUPLICAR Then
                    mostrarMensajePregunta("¿Esta seguro que desea duplicar el registro?", _
                                  Program.TituloSistema, _
                                  "DUPLICARREGISTRO", _
                                  AddressOf TerminoMensajePregunta, False)
                ElseIf pstrAccion = OPCION_ANULAR Then
                    If Await ValidarEstadoLibranzas(_EncabezadoSeleccionado, "anular") Then
                        mostrarMensajePregunta("¿Esta seguro que desea anular el registro?", _
                                  Program.TituloSistema, _
                                  "ANULARREGISTRO", _
                                  AddressOf TerminoMensajePregunta, False)
                    Else
                        IsBusy = False
                    End If
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el estado del registro.", Me.ToString(), "validarEstadoRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Function

    Public Overrides Sub CancelarEditarRegistro()
        Try
            logCancelarRegistro = True
            logEditarRegistro = False
            logDuplicarRegistro = False
            logNuevoRegistro = False
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, False)

            ObtenerInformacionCombosCompletos()

            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)

            EncabezadoSeleccionadoAnterior = Nothing

            Editando = False

            HabilitarCamposRegistro(_EncabezadoSeleccionado)

            CambiarColorFondoTextoBuscador()

            IsBusy = False

            dcProxy.RejectChanges()
            MyBase.CambioItem("Editando")
            MyBase.CancelarEditarRegistro()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
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

    Public Overrides Async Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.intID <> 0 Then
                    Await validarEstadoRegistro(OPCION_ANULAR)
                Else
                    mostrarMensaje("No se ha seleccionado ningun registro para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                mostrarMensaje("No se ha seleccionado ningun registro para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular el registro", Me.ToString(), Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Async Sub AnularRegistro(ByVal pstrObservaciones As String)

        Try
            Dim objRet As LoadOperation(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion)

            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Libranzas_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.Libranzas_AnularSyncQuery(_EncabezadoSeleccionado.intID, pstrObservaciones, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al anular el registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular el registro.", Me.ToString(), "AnularRegistro", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim logExitoso As Boolean = False
                        Dim logContinuaMostrandoMensaje As Boolean = False
                        Dim logRequiereConfirmacion As Boolean = False
                        Dim logRequiereJustificacion As Boolean = False
                        Dim logRequiereAprobacion As Boolean = False
                        Dim logConsultaListaJustificacion As Boolean = False
                        Dim logError As Boolean = False
                        Dim strMensajeExitoso As String = "El registro se anulo correctamente."
                        Dim strMensajeError As String = "El registro no se pudo anular."
                        Dim logEsHtml As Boolean = False
                        Dim strMensajeDetallesHtml As String = String.Empty
                        Dim strMensajeRetornoHtml As String = String.Empty

                        For Each li In objRet.Entities.ToList
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

                            Await ConsultarEncabezado(True, String.Empty)
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

                            mostrarMensaje(strMensajeError, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, String.Empty, "Reglas incumplidas en los detalles de la operación", logEsHtml, strMensajeDetallesHtml)
                            IsBusy = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular el registro.", Me.ToString(), "AnularRegistro", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub CambiarAForma()
        Try
            If logCargoForma = False Then
                ViewLibranzas.GridEdicion.Children.Add(viewFormaLibranzas)
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

    Public Sub OrganizarNuevaBusqueda()
        Try
            Dim objBusqueda As New CamposBusquedaLibranzas
            objBusqueda.ID = Nothing
            objBusqueda.Comitente = String.Empty
            objBusqueda.Custodio = Nothing
            objBusqueda.NombreCustodio = String.Empty
            objBusqueda.Emisor = Nothing
            objBusqueda.NombreEmisor = String.Empty
            objBusqueda.FechaRegistro = Nothing
            objBusqueda.NroCredito = String.Empty
            objBusqueda.TipoRegistro = String.Empty
            objBusqueda.Pagador = Nothing
            objBusqueda.NombrePagador = String.Empty

            BusquedaLibranzas = objBusqueda

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al organizar la nueva busqueda.", Me.ToString(), "OrganizarNuevaBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function CargarCombos(ByVal plogCompletos As Boolean, Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Libranzas_Combos)

        Try
            If logNuevoRegistro Or logEditarRegistro Then
                IsBusy = True
            End If

            If Not IsNothing(pstrUserState) Then
                pstrUserState = pstrUserState.ToUpper
            End If

            ErrorForma = String.Empty

            dcProxy.Libranzas_Combos.Clear()

            objRet = Await dcProxy.Load(dcProxy.Libranzas_ConsultarCombosSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consultar los combos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar los combos.", Me.ToString(), "CargarCombos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.ToList.Count > 0 Then
                        Dim strNombreCategoria As String = String.Empty
                        Dim objListaNodosCategoria As List(Of CFCalculosFinancieros.Libranzas_Combos) = Nothing
                        Dim objDiccionario As New Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))

                        Dim listaCategorias = From lc In objRet.Entities Select lc.Topico Distinct

                        For Each li In listaCategorias
                            strNombreCategoria = li
                            objListaNodosCategoria = (From ln In objRet.Entities Where ln.Topico = strNombreCategoria).ToList
                            objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                        Next

                        If plogCompletos Then
                            DiccionarioCombosCompleta = Nothing

                            DiccionarioCombosCompleta = objDiccionario

                            If pstrUserState = OPCION_INICIO Then
                                ObtenerDecimales()
                            End If

                            ObtenerInformacionCombosCompletos()
                        Else
                            DiccionarioCombos = Nothing
                            DiccionarioCombos = objDiccionario

                            If logNuevoRegistro = False And logEditarRegistro = False Then
                                ObtenerValoresCombos(True, _EncabezadoSeleccionado)
                            Else
                                 ObtenerValoresCombos(False, _EncabezadoSeleccionado, pstrUserState)
                            End If
                        End If

                    End If
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar los combos ", Me.ToString(), "CargarCombos", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Sub BuscarCliente(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not String.IsNullOrEmpty(pstrIdComitente) Then
                If Not IsNothing(mdcProxyUtilidad.BuscadorClientes) Then
                    mdcProxyUtilidad.BuscadorClientes.Clear()
                End If

                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                If Not String.IsNullOrEmpty(strClienteABuscar) Then
                    mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarClienteEspecificoQuery(strClienteABuscar, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrUserState)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información del cliente.", _
                                 Me.ToString(), "BuscarCliente", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub CargarDatosCliente(ByVal pComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pComitente) Then
                If Not IsNothing(_EncabezadoSeleccionado) Then

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información del cliente.", _
                                 Me.ToString(), "SeleccionarCliente", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Function AsignarValorTopicoCategoria(ByVal pstrTopicoDiccionario As String, pstrValorDefecto As String) As String
        Dim objRetorno As String = String.Empty
        Try
            'Valida que el topico exista
            If DiccionarioCombos.ContainsKey(pstrTopicoDiccionario) Then
                'Valida sí el diccionario tiene solo un valor para asignarselo por defecto
                If DiccionarioCombos(pstrTopicoDiccionario).Count = 1 Then
                    objRetorno = DiccionarioCombos(pstrTopicoDiccionario).FirstOrDefault.Codigo
                ElseIf Not String.IsNullOrEmpty(pstrValorDefecto) Then
                    If DiccionarioCombos(pstrTopicoDiccionario).Where(Function(i) i.Codigo = pstrValorDefecto).Count > 0 Then
                        objRetorno = pstrValorDefecto
                    Else
                        objRetorno = String.Empty
                    End If
                Else
                    objRetorno = pstrValorDefecto
                End If
            Else
                objRetorno = pstrValorDefecto
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valor por defecto del combo.", _
                                Me.ToString(), "AsignarValorTopicoCategoria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return objRetorno
    End Function

    Public Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean, ByVal pobjRegistroSelected As CFCalculosFinancieros.Libranzas, Optional ByVal Opcion As String = "")
        Try
            Dim objDiccionario As New Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))
            Dim objListaCategoria As New List(Of CFCalculosFinancieros.Libranzas_Combos)
            Dim objListaCategoria1 As New List(Of CFCalculosFinancieros.Libranzas_Combos)
            Dim objListaCategoria2 As New List(Of CFCalculosFinancieros.Libranzas_Combos)
            Dim objListaCategoria3 As New List(Of CFCalculosFinancieros.Libranzas_Combos)

            If Not IsNothing(DiccionarioCombos) Then
                For Each li In DiccionarioCombos
                    objDiccionario.Add(li.Key, li.Value)
                Next
            End If

            If Not IsNothing(objDiccionario) Then
                DiccionarioCombos = objDiccionario
                If ValoresCompletos Then 'Cuando ValoresCompletos = True

                Else ' Cuando ValoresCompletos = False
                    If Not String.IsNullOrEmpty(Opcion) Then
                        Dim OpcionValoresDefecto As String = String.Empty

                        If Opcion.ToUpper = OPCION_EDITAR Then
                            'Se llevan los anteriores registro ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.
                            logCambiarValoresEnSelected = False
                            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)
                            logCambiarValoresEnSelected = True

                            Editando = True
                            MyBase.CambioItem("Editando")

                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)

                            HabilitarCamposRegistro(_EncabezadoSeleccionado)

                            'Se posiciona en el primer registro
                            'BuscarControlValidacion(viewFormaLibranzas, "dtmFechaCumplimiento")

                            OpcionValoresDefecto = OPCION_EDITAR

                            CambiarColorFondoTextoBuscador()

                            IsBusy = False

                            ObtenerValoresDefecto(OpcionValoresDefecto, _EncabezadoSeleccionado)
                        Else
                            OpcionValoresDefecto = Opcion.ToUpper

                            ObtenerValoresDefecto(OpcionValoresDefecto, pobjRegistroSelected)
                        End If
                    End If

                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto de los combos.", _
                                 Me.ToString(), "ObtenerValoresCombos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ObtenerValoresDefecto(ByVal pstrOpcion As String, ByVal objRegistroSelected As CFCalculosFinancieros.Libranzas)
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                logCalcularValores = False

                Select Case pstrOpcion.ToUpper
                    Case OPCION_DUPLICAR
                        HabilitarCamposRegistro(_EncabezadoSeleccionado)
                    Case OPCION_EDITAR
                        HabilitarCamposRegistro(_EncabezadoSeleccionado)
                End Select

                logCalcularValores = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto.", _
                                 Me.ToString(), "ObtenerValoresDefecto", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Async Sub SeleccionarCliente(ByVal pobjCliente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjCliente) Then
                If logEditarRegistro Or logNuevoRegistro Then

                    If Not IsNothing(_EncabezadoSeleccionado) Then

                        _EncabezadoSeleccionado.lngIDComitente = pobjCliente.IdComitente
                        _EncabezadoSeleccionado.strNombreCliente = pobjCliente.Nombre
                        _EncabezadoSeleccionado.strNumeroDocumentoCliente = pobjCliente.NroDocumento
                        _EncabezadoSeleccionado.strCodTipoIdentificacionCliente = pobjCliente.CodTipoIdentificacion
                        _EncabezadoSeleccionado.strTipoIdentificacionCliente = pobjCliente.TipoIdentificacion

                        'Consulta la compañia asociada al cliente
                        Dim objRet As LoadOperation(Of CFCalculosFinancieros.CompaniaComitente)

                        Try
                            ErrorForma = String.Empty

                            dcProxy.CompaniaComitentes.Clear()

                            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniaAsociadaComitenteSyncQuery(_EncabezadoSeleccionado.lngIDComitente, Program.Usuario, Program.HashConexion)).AsTask()

                            If Not objRet Is Nothing Then
                                If objRet.HasError Then
                                    If objRet.Error Is Nothing Then
                                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar la compañia del comitente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                    Else
                                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la compañia del comitente.", Me.ToString(), "SeleccionarCliente", Program.TituloSistema, Program.Maquina, objRet.Error)
                                    End If

                                    objRet.MarkErrorAsHandled()
                                Else
                                    If objRet.Entities.Count > 0 Then
                                        _EncabezadoSeleccionado.intIDCompania = objRet.Entities.First.intID
                                        _EncabezadoSeleccionado.strNombreCompania = objRet.Entities.First.strNombreCorto
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la compañia del comitente.", Me.ToString(), "SeleccionarCliente", Application.Current.ToString(), Program.Maquina, ex)
                        End Try
                    End If
                End If
            Else
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    _EncabezadoSeleccionado.lngIDComitente = String.Empty
                    _EncabezadoSeleccionado.strNombreCliente = String.Empty
                    _EncabezadoSeleccionado.strNumeroDocumentoCliente = String.Empty
                    _EncabezadoSeleccionado.strCodTipoIdentificacionCliente = String.Empty
                    _EncabezadoSeleccionado.strTipoIdentificacionCliente = String.Empty
                    _EncabezadoSeleccionado.intIDCompania = Nothing
                    _EncabezadoSeleccionado.strNombreCompania = String.Empty
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub HabilitarCamposRegistro(ByVal pobjRegistroSelected As CFCalculosFinancieros.Libranzas)
        Try
            If Editando Then
                If Not IsNothing(pobjRegistroSelected) Then
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, True)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, True)
                End If
            Else
                If Not IsNothing(pobjRegistroSelected) Then
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, False)
                Else
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARFLUJOS, False)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al habilitar los controles dependiendo del tipo de registro.", Me.ToString, "HabilitarCamposRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Function ValidarGuardadoRegistro(ByVal pobjRegistro As CFCalculosFinancieros.Libranzas) As Boolean
        Try
            'Valida los campos que son requeridos por el sistema de OYDPLUS.
            Dim logPasaValidacion As Boolean = True
            Dim strMensajeValidacion = String.Empty

            If String.IsNullOrEmpty(pobjRegistro.strNroCredito) Then
                strMensajeValidacion = String.Format("{0}{1} -   Nro. Crédito", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.lngIDComitente) Then
                strMensajeValidacion = String.Format("{0}{1} -   Comitente", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.intIDEmisor) Or pobjRegistro.intIDEmisor = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Emisor", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.intNroCuotas) Or pobjRegistro.intNroCuotas = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Nro cuotas", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.strPeriodoPago) Then
                strMensajeValidacion = String.Format("{0}{1} -  Periodo pago/cuotas", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.dblValorCreditoOriginal) Or pobjRegistro.dblValorCreditoOriginal = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Valor crédito (Original)", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.strTipoPago) Then
                strMensajeValidacion = String.Format("{0}{1} -  Tipo pago/cuota", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.dblTasaInteresCredito) Or pobjRegistro.dblTasaInteresCredito = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Tasa interés crédito (E.A.)", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.dblTasaDescuento) Or pobjRegistro.dblTasaDescuento = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Tasa descuento (E.A.)", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.intIDPagador) Or pobjRegistro.intIDPagador = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Pagador", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.intIDCustodio) Or pobjRegistro.intIDCustodio = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Custodio", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.strNroDocumentoBeneficiario) Then
                strMensajeValidacion = String.Format("{0}{1} -  Nro documento beneficiario", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.strNombreBeneficiario) Then
                strMensajeValidacion = String.Format("{0}{1} -  Nombre beneficiario", strMensajeValidacion, vbCrLf)
            End If

            'If String.IsNullOrEmpty(pobjRegistro.strTipoIdentificacionBeneficiario) Then
            '    strMensajeValidacion = String.Format("{0}{1} -  Tipo identificación beneficiario", strMensajeValidacion, vbCrLf)
            'End If

            If String.IsNullOrEmpty(strMensajeValidacion) Then
                If Not IsNothing(_ListaFlujos) Then
                    If _ListaFlujos.Count > 0 Then
                        For Each li In _ListaFlujos
                            If li.logModificoFecha Then
                                If _ListaFlujos.Where(Function(i) CDate(i.dtmFechaFlujo) = CDate(li.dtmFechaFlujo)).Count > 1 Then
                                    strMensajeValidacion = "La fecha no puede estar repetida en los flujos."
                                    Exit For
                                End If
                            End If
                        Next

                        If Not String.IsNullOrEmpty(strMensajeValidacion) Then
                            logPasaValidacion = False
                            strMensajeValidacion = String.Format("Señor usuario, debe de corregir las siguientes inconsistencias en el detalle de los flujos.{0}{1}", strMensajeValidacion, vbCrLf)
                            mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If

                If String.IsNullOrEmpty(strMensajeValidacion) Then
                    If pobjRegistro.dtmFechaInicioCredito.Value.Date > pobjRegistro.dtmFechaRegistro.Value.Date Then
                        strMensajeValidacion = String.Format("{0}{1} -   La fecha de inicio del crédito no puede ser mayor a la fecha de registro", strMensajeValidacion, vbCrLf)
                    End If

                    If pobjRegistro.dtmFechaFinCredito.Value.Date < pobjRegistro.dtmFechaRegistro.Value.Date Then
                        strMensajeValidacion = String.Format("{0}{1} -   La fecha de fin del crédito no puede ser menor a la fecha de registro", strMensajeValidacion, vbCrLf)
                    End If

                    If pobjRegistro.dtmFechaInicioCredito.Value.Date > pobjRegistro.dtmFechaFinCredito.Value.Date Then
                        strMensajeValidacion = String.Format("{0}{1} -   La fecha de inicio del crédito no puede ser mayor a la fecha de fin del crédito", strMensajeValidacion, vbCrLf)
                    End If

                    If Not String.IsNullOrEmpty(strMensajeValidacion) Then
                        logPasaValidacion = False
                        strMensajeValidacion = String.Format("Señor usuario, debe de corregir las siguientes inconsistencias en el registro.{0}{1}", strMensajeValidacion, vbCrLf)
                        mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If

            Else
                logPasaValidacion = False
                strMensajeValidacion = String.Format("Señor usuario, los siguientes datos son requeridos para guardar el registro: {0}{1}", strMensajeValidacion, vbCrLf)
                mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            Return logPasaValidacion
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la valición del registro.", Me.ToString, "ValidarGuardadoRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
            Return False
        End Try
    End Function

    Private Function ValidarCampoEnDiccionario(ByVal pstrOpcionDiccionario As String, ByVal pstrValor As String) As Boolean
        Dim logRetorno As Boolean = False
        Try
            If Not IsNothing(_DiccionarioCombos) Then
                If Not IsNothing(_DiccionarioCombos(pstrOpcionDiccionario)) Then
                    If _DiccionarioCombos(pstrOpcionDiccionario).Where(Function(i) i.Codigo = pstrValor).Count > 0 Then
                        logRetorno = True
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la valición de que el valor exista en el diccionario.", Me.ToString, "ValidarCampoEnDiccionario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
        Return logRetorno
    End Function

    'Public Sub BuscarControlValidacion(ByVal pViewOperaciones As FormaOperacionesOtrosNegociosView, ByVal pstrOpcion As String)
    '    Try
    '        If Not IsNothing(pViewOperaciones) Then
    '            If TypeOf pViewOperaciones.FindName(pstrOpcion) Is TabItem Then
    '                CType(pViewOperaciones.FindName(pstrOpcion), TabItem).IsSelected = True
    '            ElseIf TypeOf pViewOperaciones.FindName(pstrOpcion) Is TextBox Then
    '                CType(pViewOperaciones.FindName(pstrOpcion), TextBox).Focus()
    '            ElseIf TypeOf pViewOperaciones.FindName(pstrOpcion) Is ComboBox Then
    '                CType(pViewOperaciones.FindName(pstrOpcion), ComboBox).Focus()
    '           
    '                
    '            ElseIf TypeOf pViewOperaciones.FindName(pstrOpcion) Is A2Utilidades.A2NumericBox Then
    '                CType(pViewOperaciones.FindName(pstrOpcion), A2Utilidades.A2NumericBox).Focus()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control dentro del registro.", Me.ToString, "BuscarControlValidacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '        IsBusy = False
    '    End Try
    'End Sub

    Public Async Function CalcularValorRegistro(Optional ByVal plogMostrarMensajeUsuario As Boolean = True) As System.Threading.Tasks.Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If Not IsNothing(_EncabezadoSeleccionado.dtmFechaRegistro) And _
                    Not IsNothing(_EncabezadoSeleccionado.dtmFechaInicioCredito) And _
                    _EncabezadoSeleccionado.intNroCuotas > 0 And _
                    Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strPeriodoPago) And _
                    _EncabezadoSeleccionado.dblValorCreditoOriginal > 0 And _
                    Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoPago) And _
                    _EncabezadoSeleccionado.dblTasaDescuento > 0 Then

                    Dim objRet As LoadOperation(Of CFCalculosFinancieros.Libranzas_Calculos)

                    Try
                        IsBusyCalculos = True
                        ErrorForma = String.Empty

                        dcProxy.Libranzas_Calculos.Clear()

                        objRet = Await dcProxy.Load(dcProxy.Libranzas_CalcularSyncQuery(strTipoCalculo,
                                                                                        _EncabezadoSeleccionado.dtmFechaRegistro,
                                                                                        _EncabezadoSeleccionado.dtmFechaInicioCredito,
                                                                                        _EncabezadoSeleccionado.dtmFechaFinCredito,
                                                                                        _EncabezadoSeleccionado.intNroCuotas,
                                                                                        _EncabezadoSeleccionado.dblValorCuotas,
                                                                                        _EncabezadoSeleccionado.strPeriodoPago,
                                                                                        _EncabezadoSeleccionado.dblValorCreditoOriginal,
                                                                                        _EncabezadoSeleccionado.strTipoPago,
                                                                                        _EncabezadoSeleccionado.dblTasaInteresCredito,
                                                                                        _EncabezadoSeleccionado.dblTasaDescuento,
                                                                                        _EncabezadoSeleccionado.dblValorOperacion,
                                                                                        _EncabezadoSeleccionado.dblValorCreditoActual, Program.Usuario, Program.HashConexion)).AsTask()

                        logRecalcularFlujos = True

                        If Not objRet Is Nothing Then
                            If objRet.HasError Then
                                If objRet.Error Is Nothing Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al calcular los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                Else
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "CalcularValorRegistro", Program.TituloSistema, Program.Maquina, objRet.Error)
                                End If

                                objRet.MarkErrorAsHandled()
                            Else
                                If objRet.Entities.Count > 0 Then
                                    If objRet.Entities.First.logExitoso Then
                                        Dim objResultadoCalculos = objRet.Entities.First

                                        logCalcularValores = False

                                        _EncabezadoSeleccionado.dtmFechaFinCredito = objResultadoCalculos.dtmFechaFinCredito
                                        _EncabezadoSeleccionado.dblValorCuotas = objResultadoCalculos.dblValorCuotas
                                        _EncabezadoSeleccionado.dblValorOperacion = objResultadoCalculos.dblValorOperacion
                                        _EncabezadoSeleccionado.dblValorCreditoActual = objResultadoCalculos.dblValorCreditoActual

                                        logCalcularValores = True
                                        logResultado = True
                                    Else
                                        logResultado = False
                                        If plogMostrarMensajeUsuario Then
                                            mostrarMensaje(objRet.Entities.First.strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "CalcularValorRegistro", Application.Current.ToString(), Program.Maquina, ex)
                        logResultado = False
                    Finally
                    End Try
                Else
                    logResultado = True
                End If
            Else
                logResultado = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor del registro.", Me.ToString(), "CalcularValorRegistro", Program.TituloSistema, Program.Maquina, ex)
            logResultado = False
        End Try

        IsBusyCalculos = False
        Return (logResultado)
    End Function

    Public Sub ObtenerInformacionCombosCompletos()
        Try
            Dim objDiccionarioCombos As New Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))
            Dim strNombreCategoria As String = String.Empty
            Dim intContador As Integer = 1

            For Each dic In DiccionarioCombosCompleta
                strNombreCategoria = dic.Key
                objDiccionarioCombos.Add(strNombreCategoria, dic.Value)
            Next

            DiccionarioCombos = Nothing
            DiccionarioCombos = objDiccionarioCombos

            ObtenerValoresCombos(True, _EncabezadoSeleccionado)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Obtener la información de los combos.", Me.ToString, "ObtenerInformacionCombosCompletos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub ObtenerValoresRegistroAnterior(ByVal pobjRegistro As CFCalculosFinancieros.Libranzas, ByRef pobjRegistroSalvarDatos As CFCalculosFinancieros.Libranzas)
        Try
            If Not IsNothing(pobjRegistro) Then
                Dim objNewRegistro As New CFCalculosFinancieros.Libranzas

                objNewRegistro.intID = pobjRegistro.intID
                objNewRegistro.dblTasaDescuento = pobjRegistro.dblTasaDescuento
                objNewRegistro.dblTasaInteresCredito = pobjRegistro.dblTasaInteresCredito
                objNewRegistro.dblValorCreditoActual = pobjRegistro.dblValorCreditoActual
                objNewRegistro.dblValorCreditoOriginal = pobjRegistro.dblValorCreditoOriginal
                objNewRegistro.dblValorCuotas = pobjRegistro.dblValorCuotas
                objNewRegistro.dblValorOperacion = pobjRegistro.dblValorOperacion
                objNewRegistro.dtmActualizacion = pobjRegistro.dtmActualizacion
                objNewRegistro.dtmFechaFinCredito = pobjRegistro.dtmFechaFinCredito
                objNewRegistro.dtmFechaInicioCredito = pobjRegistro.dtmFechaInicioCredito
                objNewRegistro.dtmFechaRegistro = pobjRegistro.dtmFechaRegistro
                objNewRegistro.dtmFechaRegistroSistema = pobjRegistro.dtmFechaRegistroSistema
                objNewRegistro.dtmFechaValoracion = pobjRegistro.dtmFechaValoracion
                objNewRegistro.intIDCompania = pobjRegistro.intIDCompania
                objNewRegistro.intIDCustodio = pobjRegistro.intIDCustodio
                objNewRegistro.intIDEmisor = pobjRegistro.intIDEmisor
                objNewRegistro.intIDPagador = pobjRegistro.intIDPagador
                objNewRegistro.intNroCuotas = pobjRegistro.intNroCuotas
                objNewRegistro.lngIDComitente = pobjRegistro.lngIDComitente
                objNewRegistro.strCodTipoIdentificacionCliente = pobjRegistro.strCodTipoIdentificacionCliente
                objNewRegistro.strCodTipoIdentificacionCustodio = pobjRegistro.strCodTipoIdentificacionCustodio
                objNewRegistro.strCodTipoIdentificacionEmisor = pobjRegistro.strCodTipoIdentificacionEmisor
                objNewRegistro.strCodTipoIdentificacionPagador = pobjRegistro.strCodTipoIdentificacionPagador
                objNewRegistro.strDescripcionEstado = pobjRegistro.strDescripcionEstado
                objNewRegistro.strDescripcionPeriodoPago = pobjRegistro.strDescripcionPeriodoPago
                objNewRegistro.strDescripcionTipoIdentificacionBeneficiario = pobjRegistro.strDescripcionTipoIdentificacionBeneficiario
                objNewRegistro.strDescripcionTipoPago = pobjRegistro.strDescripcionTipoPago
                objNewRegistro.strDescripcionTipoRegistro = pobjRegistro.strDescripcionTipoRegistro
                objNewRegistro.strEstado = pobjRegistro.strEstado
                objNewRegistro.strNombreBeneficiario = pobjRegistro.strNombreBeneficiario
                objNewRegistro.strNombreCliente = pobjRegistro.strNombreCliente
                objNewRegistro.strNombreCompania = pobjRegistro.strNombreCompania
                objNewRegistro.strNombreCustodio = pobjRegistro.strNombreCustodio
                objNewRegistro.strNombreEmisor = pobjRegistro.strNombreEmisor
                objNewRegistro.strNombrePagador = pobjRegistro.strNombrePagador
                objNewRegistro.strNroCredito = pobjRegistro.strNroCredito
                objNewRegistro.strNroDocumentoBeneficiario = pobjRegistro.strNroDocumentoBeneficiario
                objNewRegistro.strNumeroDocumentoCliente = pobjRegistro.strNumeroDocumentoCliente
                objNewRegistro.strNumeroDocumentoCustodio = pobjRegistro.strNumeroDocumentoCustodio
                objNewRegistro.strNumeroDocumentoEmisor = pobjRegistro.strNumeroDocumentoEmisor
                objNewRegistro.strNumeroDocumentoPagador = pobjRegistro.strNumeroDocumentoPagador
                objNewRegistro.strObservaciones = pobjRegistro.strObservaciones
                objNewRegistro.strPeriodoPago = pobjRegistro.strPeriodoPago
                objNewRegistro.strTipoIdentificacionBeneficiario = pobjRegistro.strTipoIdentificacionBeneficiario
                objNewRegistro.strTipoIdentificacionCliente = pobjRegistro.strTipoIdentificacionCliente
                objNewRegistro.strTipoIdentificacionCustodio = pobjRegistro.strTipoIdentificacionCustodio
                objNewRegistro.strTipoIdentificacionEmisor = pobjRegistro.strTipoIdentificacionEmisor
                objNewRegistro.strTipoIdentificacionPagador = pobjRegistro.strTipoIdentificacionPagador
                objNewRegistro.strTipoPago = pobjRegistro.strTipoPago
                objNewRegistro.strTipoRegistro = pobjRegistro.strTipoRegistro
                objNewRegistro.strUsuario = pobjRegistro.strUsuario

                pobjRegistroSalvarDatos = objNewRegistro
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", Me.ToString(), "ObtenerValoresRegistroAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub LimpiarDatosTipoNegocio(ByVal pobjRegistroSeleccionado As CFCalculosFinancieros.Libranzas, Optional ByVal pstrTipo As String = "")
        Try
            If Not IsNothing(pobjRegistroSeleccionado) Then
                logCalcularValores = False


                logCalcularValores = True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los datos del tipo de negocio.", Me.ToString(), "LimpiarDatosTipoNegocio", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Async Function RecargarPantalla() As Task
        Try
            If logEditarRegistro = False And logNuevoRegistro = False Then
                Dim IDRegistroPosicionar As Integer = 0

                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IDRegistroPosicionar = _EncabezadoSeleccionado.intID
                End If

                If IDRegistroPosicionar = 0 Then
                    Await ConsultarEncabezado(True, String.Empty)
                Else
                    Await ConsultarEncabezado(True, String.Empty, True, IDRegistroPosicionar)
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar la pantalla de otros negocios.", Me.ToString(), "RecargarPantalla", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Async Function ObtenerFechaHoraServidor() As Task(Of DateTime)
        Dim dtmFechaHoraServidor As DateTime = Now

        Try
            Dim objRet As InvokeOperation(Of DateTime)
            Dim objProxyUtil As UtilidadesCFDomainContext

            objProxyUtil = inicializarProxyUtilidades()

            objRet = Await objProxyUtil.ConsultarFechaServidorSync(Program.Usuario, Program.HashConexion).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ObtenerFechaHoraServidor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If Not IsNothing(objRet.Value) Then
                        dtmFechaHoraServidor = objRet.Value
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ObtenerFechaHoraServidor. ", Me.ToString(), "ObtenerFechaHoraServidor", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return dtmFechaHoraServidor
    End Function

    ''' <summary>
    ''' CCM20151107: Consultar fecha de cierre del portafolio para validar el ingreso de las operaciones
    ''' </summary>
    ''' 
    Public Async Function ObtenerFechaCierrePortafolio(ByVal pstrIdComitente As String) As Task(Of DateTime?)
        Dim dtmFechaCierre As DateTime? = Nothing

        Try
            If String.IsNullOrEmpty(pstrIdComitente) Then
                Return (Nothing)
            End If

            Dim objRet As InvokeOperation(Of DateTime?)
            Dim objProxyUtil As UtilidadesCFDomainContext

            objProxyUtil = inicializarProxyUtilidades()

            objRet = Await objProxyUtil.ConsultarFechaCierrePortafolioSync(pstrIdComitente, Program.Usuario, Program.HashConexion).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar la fecha de cierre del portafolio del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If Not IsNothing(objRet.Value) Then
                        dtmFechaCierre = objRet.Value
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del portafolio. ", Me.ToString(), "ObtenerFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return dtmFechaCierre
    End Function

    Public Sub ObtenerDecimales()
        Try
            Dim intCantidadDecimales As Integer = 6

            If Not IsNothing(DiccionarioCombosCompleta) Then
                If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_CANTIDADDECIMALES") Then
                    If DiccionarioCombosCompleta("NEGOCIOS_CANTIDADDECIMALES").Count > 0 Then
                        intCantidadDecimales = DiccionarioCombosCompleta("NEGOCIOS_CANTIDADDECIMALES").First.ID
                    End If
                End If
            End If

            FormatoCamposDecimales = String.Format("{0}", intCantidadDecimales)
            FormatoCamposNumericos = String.Format("{0}", intCantidadDecimales)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la valición del registro.", Me.ToString, "ObtenerDecimales", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Async Function ValidarEstadoLibranzas(ByVal objRegistroSelected As CFCalculosFinancieros.Libranzas, ByVal pstrAccion As String) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Libranzas_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.Libranzas_ValidarEstadoSyncQuery(objRegistroSelected.intID, pstrAccion, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarLibranzas", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                    IsBusy = False
                Else
                    Dim objListaResultadoValidacion As List(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion) = objRet.Entities.ToList

                    If objListaResultadoValidacion.Count > 0 Then

                        If objListaResultadoValidacion.Where(Function(i) CBool(i.DetieneIngreso)).Count > 0 Then
                            logResultado = False
                            mostrarMensaje(objListaResultadoValidacion.Where(Function(i) CBool(i.DetieneIngreso)).First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            logResultado = True
                        End If
                    End If
                End If
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al validar el estado del registro.", Me.ToString(), "ValidarEstadoLibranzas", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Sub CambiarColorFondoTextoBuscador()
        Try
            Dim colorFondo As Color
            If Editando Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadores = New SolidColorBrush(colorFondo)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el color de fondo del texto.", Me.ToString(), "CambiarColorFondoTextoBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos OYDPlus"

    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.HasError = False Then
                If lo.Entities.ToList.Count > 0 Then
                    Me.ComitenteSeleccionado = lo.Entities.ToList.FirstOrDefault
                Else
                    Me.ComitenteSeleccionado = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente del registro", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitentel registro", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    'Private Sub PosicionarControlValidaciones(ByVal plogOrdenOriginal As Boolean, ByVal pobjRegistroSelected As CFCalculosFinancieros.Libranzas, ByVal pobjViewLibranzas As OrdenesPLUSView, ByVal pobjViewCruzada As OrdenesCruzadasOYDPLUSView)
    '    Try
    '        'Se busca el control para llevarle el foco al control que se requiere
    '        Dim strMensaje As String = strMensajeValidacion.ToLower

    '        If strMensaje.Contains("- receptor") Then
    '            If plogOrdenOriginal Then BuscarControlValidacion(pobjViewLibranzas, "cboReceptores") Else BuscarControlValidacionCruzada(pobjViewCruzada, "cboReceptores")
    '            Exit Sub
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al terminar de mostrar el mensaje de validación.", Me.ToString, "TerminoValidacionMensajeOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '        IsBusy = False
    '    End Try
    'End Sub

    Private Sub RecargarOrdenDespuesGuardado(ByVal plogEsNuevoRegistro As Boolean, ByVal pstrMensaje As String)
        Try
            logCancelarRegistro = False
            logEditarRegistro = False
            logDuplicarRegistro = False
            logNuevoRegistro = False

            'Esto se realiza para habilitar los botones de navegación llamando el metodo TERMINOSUBMITCHANGED
            If plogEsNuevoRegistro Then
                If dcProxy1.Libranzas.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).Count = 0 Then
                    dcProxy1.Libranzas.Add(_EncabezadoSeleccionado)
                End If
            End If

            Program.VerificarCambiosProxyServidor(dcProxy1)
            dcProxy1.SubmitChanges(AddressOf TerminoSubmitChanges, pstrMensaje)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar de nuevo los parametros.", Me.ToString(), "RecargarOrdenDespuesGuardado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper
                        Case "DUPLICARREGISTRO"
                            If objResultado.DialogResult Then
                                IsBusy = True
                                DuplicarRegistro()
                            Else
                                IsBusy = False
                            End If
                        Case "ANULARREGISTRO"
                            If objResultado.DialogResult Then
                                AnularRegistro(objResultado.Observaciones)
                            Else
                                IsBusy = False

                                dcProxy.RejectChanges()
                                MyBase.CambioItem("Editando")
                                MyBase.CancelarEditarRegistro()
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

                    End Select
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje asincronico.", Me.ToString(), "TerminoMensajeResultadoAsincronico", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Funciones Generales"


    Public Sub PrRemoverValoresDic(ByRef pobjDiccionario As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos)), ByVal pstrArray As String())
        Try
            For i = 0 To pstrArray.Count - 1
                If pobjDiccionario.ContainsKey(pstrArray(i)) Then pobjDiccionario.Remove(pstrArray(i))
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.", _
                                 Me.ToString(), "PrRemoverValoresDic", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ExtraerListaPorCategoria(pobjDiccionario As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos)), pstrTopico As String, pstrCategoria As String) As List(Of CFCalculosFinancieros.Libranzas_Combos)
        ExtraerListaPorCategoria = New List(Of CFCalculosFinancieros.Libranzas_Combos)
        Try
            If pobjDiccionario.ContainsKey(pstrTopico) Then
                Dim objRetorno = From item In pobjDiccionario(pstrTopico)
                                 Select New CFCalculosFinancieros.Libranzas_Combos With {.ID = item.ID, _
                                                                            .Codigo = item.Codigo, _
                                                                            .Descripcion = item.Descripcion, _
                                                                            .Topico = pstrCategoria}
                If objRetorno.Count > 0 Then
                    ExtraerListaPorCategoria = objRetorno.ToList()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.", _
                                 Me.ToString(), "ExtraerListaPorCategoria", Application.Current.ToString(), Program.Maquina, ex)
            Return New List(Of CFCalculosFinancieros.Libranzas_Combos)
        End Try
    End Function


#End Region

#Region "Eventos"

    Private Async Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try
            If logCalcularValores Then
                Select Case e.PropertyName.ToLower()
                    Case "dtmfecharegistro"
                        If logCalcularValores Then
                            Await CalcularValorRegistro(False)
                        End If
                    Case "dtmfechainiciocredito"
                        If logCalcularValores Then
                            Await CalcularValorRegistro(False)
                        End If
                    Case "strtipopago"
                        If logCalcularValores Then
                            Await CalcularValorRegistro(False)
                        End If
                    Case "strperiodopago"
                        If logCalcularValores Then
                            Await CalcularValorRegistro(False)
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_EncabezadoSeleccionado_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    Private Sub _FlujoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _FlujoSeleccionado.PropertyChanged
        Try
            If logCalcularValores Then
                Select Case e.PropertyName.ToLower()
                    Case "dtmfechaflujo"
                        If logRecalcularFlujos Then
                            mostrarMensaje("Los flujos seran recalculados de nuevo porque se realizo una modificación de la libranza.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            _FlujoSeleccionado.logModificoFecha = True
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_EncabezadoSeleccionado_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
                    _myDispatcherTimerOrdenes.Interval = New TimeSpan(0, Program.Par_lapso_recarga, 0)
                    AddHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
                End If
                _myDispatcherTimerOrdenes.Start()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

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

    Private Async Sub Each_Tick(sender As Object, e As EventArgs)
        'Recarga la pantalla cuando se este en un tiempo de espera sin notificaciones y sin refrescar la pantalla.
        Await RecargarPantalla()
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaLibranzas
    Implements INotifyPropertyChanged

    Private _ID As Nullable(Of Integer)
    <Display(Name:="ID Libranza")> _
    Public Property ID() As Nullable(Of Integer)
        Get
            Return _ID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _Comitente As String
    <Display(Name:="Comitente")> _
    Public Property Comitente() As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _FechaRegistro As Nullable(Of DateTime)
    <Display(Name:="Fecha registro")> _
    Public Property FechaRegistro() As Nullable(Of DateTime)
        Get
            Return _FechaRegistro
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaRegistro"))
        End Set
    End Property

    Private _Emisor As Nullable(Of Integer)
    <Display(Name:="Emisor")> _
    Public Property Emisor() As Nullable(Of Integer)
        Get
            Return _Emisor
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Emisor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Emisor"))
        End Set
    End Property

    Private _NombreEmisor As String
    <Display(Name:="Emisor")> _
    Public Property NombreEmisor() As String
        Get
            Return _NombreEmisor
        End Get
        Set(ByVal value As String)
            _NombreEmisor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreEmisor"))
        End Set
    End Property

    Private _Pagador As Nullable(Of Integer)
    <Display(Name:="Pagador")> _
    Public Property Pagador() As Nullable(Of Integer)
        Get
            Return _Pagador
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Pagador = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Pagador"))
        End Set
    End Property

    Private _NombrePagador As String
    <Display(Name:="Pagador")> _
    Public Property NombrePagador() As String
        Get
            Return _NombrePagador
        End Get
        Set(ByVal value As String)
            _NombrePagador = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombrePagador"))
        End Set
    End Property

    Private _Custodio As Nullable(Of Integer)
    <Display(Name:="Custodio")> _
    Public Property Custodio() As Nullable(Of Integer)
        Get
            Return _Custodio
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Custodio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Custodio"))
        End Set
    End Property

    Private _NombreCustodio As String
    <Display(Name:="Custodio")> _
    Public Property NombreCustodio() As String
        Get
            Return _NombreCustodio
        End Get
        Set(ByVal value As String)
            _NombreCustodio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCustodio"))
        End Set
    End Property


    Private _NroCredito As String
    <Display(Name:="Nro. Crédito")> _
    Public Property NroCredito() As String
        Get
            Return _NroCredito
        End Get
        Set(ByVal value As String)
            _NroCredito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroCredito"))
        End Set
    End Property

    Private _TipoRegistro As String
    <Display(Name:="Tipo registro")> _
    Public Property TipoRegistro() As String
        Get
            Return _TipoRegistro
        End Get
        Set(ByVal value As String)
            _TipoRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoRegistro"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class