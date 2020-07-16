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
Imports A2.OYD.OYDServer.RIA.Web
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Threading.Tasks

Public Class AjustesManualesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            dcProxy = inicializarProxyCalculosFinancieros()
            mdcProxyUtilidad = inicializarProxyUtilidadesOYD()

            If System.Diagnostics.Debugger.IsAttached Then

            End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de registro que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                IsBusy = True
                viewFormaOperacionesOtrosNegocios = New FormaAjustesManualesView(Me)

                Dim objDiccionarioHabilitarCampos = New Dictionary(Of String, Boolean)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO.ToString, False)

                DiccionarioHabilitarCampos = objDiccionarioHabilitarCampos

                CambiarColorFondoTextoBuscador()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "AjustesManualesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
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
    Private mdcProxyUtilidad As UtilidadesDomainContext

    Private mdtmFechaCierrePortafolio As DateTime? = Nothing

    Public logNuevoRegistro As Boolean = False
    Public logEditarRegistro As Boolean = False
    Public logCancelarRegistro As Boolean = False
    Public logDuplicarRegistro As Boolean = False
    Public logModificarDatosTipoNegocio As Boolean = True
    Public logCalcularValores As Boolean = True
    Public strTipoCalculo As String = String.Empty

    Dim viewFormaOperacionesOtrosNegocios As FormaAjustesManualesView = Nothing

    Dim dtmFechaServidor As DateTime
    Dim logCambiarSelected As Boolean = True
    Dim logCambiarValoresEnSelected As Boolean = True
    Dim logCargoForma As Boolean = False
    Dim strNormaContableDefecto As String = String.Empty

#End Region

#Region "Constantes"
    Private TOOLBARACTIVOPANTALLA As String = "A2Consola_ToolbarActivo"
    Private MDTM_FECHA_CIERRE_SIN_ACTUALIZAR As Date = New Date(1999, 1, 1) 'CCM20151107: Fecha para inicializar fecha de cierre
    Private MINT_LONG_MAX_CODIGO_OYD As Integer = 17

    Public TIPOCOMPROBANTE_MANUAL As String = "M"
    Public TIPOCOMPROBANTE_BASADOENEVENTO As String = "E"

    Public TIPOORIGEN_LIQUIDACION As String = "L"
    Public TIPOORIGEN_LIQUIDACIONOTROSNEGOCIOS As String = "O"
    Public TIPOORIGEN_CUSTODIAS As String = "C"

    Public LIMPIARNEGOCIO_TIPOCOMPROBANTE As String = "C"
    Public LIMPIARNEGOCIO_TIPOORIGEN As String = "O"

    Private ESTADO_INGRESADA As String = "I"
    Private ESTADO_ANULADA As String = "A"
    Private ESTADO_PORAPROBAR As String = "PA"

    Private NATURALEZA_DB As String = "DB"
    Private NATURALEZA_CR As String = "CR"

    Private Const OPCION_INICIO As String = "INICIO"
    Private Const OPCION_NUEVO As String = "NUEVO"
    Private Const OPCION_EDITAR As String = "EDITAR"
    Private Const OPCION_ANULAR As String = "ANULAR"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    Private Const OPCION_TIPONEGOCIO As String = "TIPONEGOCIO"
    Private Const OPCION_TIPOORIGEN As String = "TIPOORIGEN"
    Private Const OPCION_ACTUALIZAR As String = "ACTUALIZAR"
    Private Const OPCION_CLASIFICACIONINVERSION As String = "CLASIFICACIONINVERSION"
    Private Const OPCION_CONTRAPARTE As String = "CONTRAPARTE"
    Private Const OPCION_TIPOREPO As String = "TIPOREPO"
    Private Const OPCION_TIPOBANREP As String = "TIPOBANREPUBLICA"
    Private Const OPCION_SOLOCONSULTA As String = "SOLOCONSULTA"

    Private Const COLOR_DESHABILITADO As String = "#E2E2E2"
    Private Const COLOR_HABILITADO As String = "#FFFFFFFF"

    Public Enum OPCIONES_HABILITARCAMPOS
        HABILITARENCABEZADO
        HABILITARNEGOCIO
        HABILITAREVENTO
    End Enum

#End Region

#Region "Propiedades para el Tipo de registro"

    Private _ListaEncabezado As List(Of CFCalculosFinancieros.AjustesContablesManual)
    Public Property ListaEncabezado() As List(Of CFCalculosFinancieros.AjustesContablesManual)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CFCalculosFinancieros.AjustesContablesManual))
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

    Private _EncabezadoDataForm As CFCalculosFinancieros.AjustesContablesManual = New CFCalculosFinancieros.AjustesContablesManual
    Public Property EncabezadoDataForm() As CFCalculosFinancieros.AjustesContablesManual
        Get
            Return _EncabezadoDataForm
        End Get
        Set(ByVal value As CFCalculosFinancieros.AjustesContablesManual)
            _EncabezadoDataForm = value
            MyBase.CambioItem("EncabezadoDataForm")
        End Set
    End Property

    Private WithEvents _EncabezadoSeleccionado As CFCalculosFinancieros.AjustesContablesManual
    Public Property EncabezadoSeleccionado() As CFCalculosFinancieros.AjustesContablesManual
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CFCalculosFinancieros.AjustesContablesManual)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If logCambiarValoresEnSelected Then
                    mdtmFechaCierrePortafolio = MDTM_FECHA_CIERRE_SIN_ACTUALIZAR

                    If _EncabezadoSeleccionado.ID > 0 Then
                        ConsultarDetalle(_EncabezadoSeleccionado.ID)
                    End If

                    HabilitarCamposRegistro(_EncabezadoSeleccionado)
                End If
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private _EncabezadoSeleccionadoAnterior As CFCalculosFinancieros.AjustesContablesManual
    Public Property EncabezadoSeleccionadoAnterior() As CFCalculosFinancieros.AjustesContablesManual
        Get
            Return _EncabezadoSeleccionadoAnterior
        End Get
        Set(ByVal value As CFCalculosFinancieros.AjustesContablesManual)
            _EncabezadoSeleccionadoAnterior = value
        End Set
    End Property

    Private _EncabezadoSeleccionadoDuplicar As CFCalculosFinancieros.AjustesContablesManual
    Public Property EncabezadoSeleccionadoDuplicar() As CFCalculosFinancieros.AjustesContablesManual
        Get
            Return _EncabezadoSeleccionadoDuplicar
        End Get
        Set(ByVal value As CFCalculosFinancieros.AjustesContablesManual)
            _EncabezadoSeleccionadoDuplicar = value
        End Set
    End Property

    Private _ViewAjustesManuales As AjustesManualesView
    Public Property ViewAjustesManuales() As AjustesManualesView
        Get
            Return _ViewAjustesManuales
        End Get
        Set(ByVal value As AjustesManualesView)
            _ViewAjustesManuales = value
        End Set
    End Property

    Private _BusquedaAjustesManuales As CamposBusquedaAjustesManuales = New CamposBusquedaAjustesManuales
    Public Property BusquedaAjustesManuales() As CamposBusquedaAjustesManuales
        Get
            Return _BusquedaAjustesManuales
        End Get
        Set(ByVal value As CamposBusquedaAjustesManuales)
            _BusquedaAjustesManuales = value
            MyBase.CambioItem("BusquedaAjustesManuales")
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

    Private _FondoTextoBuscadoresEventoContable As SolidColorBrush
    Public Property FondoTextoBuscadoresEventoContable() As SolidColorBrush
        Get
            Return _FondoTextoBuscadoresEventoContable
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadoresEventoContable = value
            MyBase.CambioItem("_FondoTextoBuscadoresEventoContable")
        End Set
    End Property

#End Region

#Region "Propiedades Receptores"

    Private _ListaDetalle As List(Of CFCalculosFinancieros.AjustesContablesManual_Detalles)
    Public Property ListaDetalle() As List(Of CFCalculosFinancieros.AjustesContablesManual_Detalles)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of CFCalculosFinancieros.AjustesContablesManual_Detalles))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
        End Set
    End Property

    Private _DetalleSeleccionado As CFCalculosFinancieros.AjustesContablesManual_Detalles
    Public Property DetalleSeleccionado As CFCalculosFinancieros.AjustesContablesManual_Detalles
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As CFCalculosFinancieros.AjustesContablesManual_Detalles)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades para cargar Información de los Combos"

    Private _DiccionarioCombos As Dictionary(Of String, List(Of CFCalculosFinancieros.AjustesContablesManual_Combos))
    Public Property DiccionarioCombos() As Dictionary(Of String, List(Of CFCalculosFinancieros.AjustesContablesManual_Combos))
        Get
            Return _DiccionarioCombos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFCalculosFinancieros.AjustesContablesManual_Combos)))
            _DiccionarioCombos = value
            MyBase.CambioItem("DiccionarioCombos")
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

    Private _MostrarCamposLiquidacion As Visibility = Visibility.Collapsed
    Public Property MostrarCamposLiquidacion() As Visibility
        Get
            Return _MostrarCamposLiquidacion
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposLiquidacion = value
            MyBase.CambioItem("MostrarCamposLiquidacion")
        End Set
    End Property

    Private _MostrarCamposLiquidacionOtrosNegocios As Visibility = Visibility.Collapsed
    Public Property MostrarCamposLiquidacionOtrosNegocios() As Visibility
        Get
            Return _MostrarCamposLiquidacionOtrosNegocios
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposLiquidacionOtrosNegocios = value
            MyBase.CambioItem("MostrarCamposLiquidacionOtrosNegocios")
        End Set
    End Property

    Private _MostrarCamposCustodia As Visibility = Visibility.Collapsed
    Public Property MostrarCamposCustodia() As Visibility
        Get
            Return _MostrarCamposCustodia
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposCustodia = value
            MyBase.CambioItem("MostrarCamposCustodia")
        End Set
    End Property

    Private _MostrarCuentaManual As Visibility = Visibility.Collapsed
    Public Property MostrarCuentaManual() As Visibility
        Get
            Return _MostrarCuentaManual
        End Get
        Set(ByVal value As Visibility)
            _MostrarCuentaManual = value
            MyBase.CambioItem("MostrarCuentaManual")
        End Set
    End Property

    Private _MostrarCuentaEvento As Visibility = Visibility.Collapsed
    Public Property MostrarCuentaEvento() As Visibility
        Get
            Return _MostrarCuentaEvento
        End Get
        Set(ByVal value As Visibility)
            _MostrarCuentaEvento = value
            MyBase.CambioItem("MostrarCuentaEvento")
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
                mdtmFechaCierrePortafolio = MDTM_FECHA_CIERRE_SIN_ACTUALIZAR

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

#End Region

#Region "Metodos"

    Public Overrides Async Sub NuevoRegistro()
        Try
            If logCargoForma = False Then
                ViewAjustesManuales.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
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

            NuevoAjuste()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub NuevoAjuste()
        Try
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)

            Dim objNewAjustesContablesManual As New CFCalculosFinancieros.AjustesContablesManual
            objNewAjustesContablesManual.ID = 0
            objNewAjustesContablesManual.Estado = ESTADO_INGRESADA
            objNewAjustesContablesManual.DescripcionEstado = "Ingresada"
            objNewAjustesContablesManual.TipoComprobante = TIPOCOMPROBANTE_BASADOENEVENTO
            objNewAjustesContablesManual.DescripcionTipoComprobante = "Basado en evento"
            objNewAjustesContablesManual.UsuarioRegistro = Program.Usuario
            objNewAjustesContablesManual.FechaRegistro = dtmFechaServidor
            objNewAjustesContablesManual.Actualizacion = dtmFechaServidor
            objNewAjustesContablesManual.FechaAplicacion = dtmFechaServidor
            objNewAjustesContablesManual.NormaContable = String.Empty
            objNewAjustesContablesManual.DescripcionNormaContable = String.Empty

            If Not IsNothing(DiccionarioCombos) Then
                If DiccionarioCombos.ContainsKey("NORMACONTABLE_DEFECTO") Then
                    If DiccionarioCombos("NORMACONTABLE_DEFECTO").Where(Function(i) i.Codigo = strNormaContableDefecto).Count > 0 Then
                        objNewAjustesContablesManual.NormaContable = DiccionarioCombos("NORMACONTABLE_DEFECTO").Where(Function(i) i.Codigo = strNormaContableDefecto).First.Codigo
                        objNewAjustesContablesManual.DescripcionNormaContable = DiccionarioCombos("NORMACONTABLE_DEFECTO").Where(Function(i) i.Codigo = strNormaContableDefecto).First.Descripcion
                    End If
                End If
            End If

            objNewAjustesContablesManual.IDComitente = String.Empty
            objNewAjustesContablesManual.NombreCliente = String.Empty
            objNewAjustesContablesManual.TipoIdentificacionCliente = String.Empty
            objNewAjustesContablesManual.DescripcionTipoIdentificacionCliente = String.Empty
            objNewAjustesContablesManual.NroDocumentoCliente = String.Empty
            objNewAjustesContablesManual.IDMoneda = Nothing
            objNewAjustesContablesManual.CodMoneda = Nothing
            objNewAjustesContablesManual.TipoAjuste = Nothing
            objNewAjustesContablesManual.IDEventoContable = Nothing
            objNewAjustesContablesManual.CodEventoContable = Nothing
            objNewAjustesContablesManual.EventoContable = Nothing
            objNewAjustesContablesManual.ClaseContable = Nothing
            objNewAjustesContablesManual.DescripcionClaseContable = Nothing
            objNewAjustesContablesManual.TipoInversion = Nothing
            objNewAjustesContablesManual.DescripcionTipoInversion = Nothing
            objNewAjustesContablesManual.TipoOrigen = Nothing
            objNewAjustesContablesManual.Origen_Numero = Nothing
            objNewAjustesContablesManual.Origen_Secuencia = Nothing
            objNewAjustesContablesManual.Origen_Tipo = Nothing
            objNewAjustesContablesManual.Origen_Clase = Nothing
            objNewAjustesContablesManual.Origen_Fecha = Nothing

            objNewAjustesContablesManual.Actualizacion = dtmFechaServidor
            objNewAjustesContablesManual.Usuario = Program.Usuario
            objNewAjustesContablesManual.UsuarioWindows = Program.UsuarioWindows
            objNewAjustesContablesManual.Maquina = Program.Maquina

            Editando = True

            ObtenerValoresRegistroAnterior(objNewAjustesContablesManual, EncabezadoSeleccionado)

            DetalleSeleccionado = Nothing
            If Not IsNothing(ListaDetalle) Then
                Dim objListaAjustes As New List(Of CFCalculosFinancieros.AjustesContablesManual_Detalles)
                ListaDetalle = Nothing
                ListaDetalle = objListaAjustes
            End If

            CambiarColorFondoTextoBuscador()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoAjuste", Program.TituloSistema, Program.Maquina, ex)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, False)

            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)
            Editando = False
        End Try
    End Sub

    Public Async Sub PreguntarDuplicarRegistro()
        Try
            If logCargoForma = False Then
                ViewAjustesManuales.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
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
            Dim objNewRegistroDuplicar As New CFCalculosFinancieros.AjustesContablesManual
            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, objNewRegistroDuplicar)

            objNewRegistroDuplicar.ID = 0
            objNewRegistroDuplicar.FechaRegistro = dtmFechaServidor
            objNewRegistroDuplicar.Actualizacion = dtmFechaServidor
            objNewRegistroDuplicar.UsuarioRegistro = Program.Usuario
            objNewRegistroDuplicar.Usuario = Program.Usuario
            objNewRegistroDuplicar.UsuarioWindows = Program.UsuarioWindows
            objNewRegistroDuplicar.Maquina = Program.Maquina
            objNewRegistroDuplicar.Estado = ESTADO_INGRESADA
            objNewRegistroDuplicar.DescripcionEstado = "Ingresada"

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = True
            logNuevoRegistro = False

            ObtenerValoresRegistroAnterior(objNewRegistroDuplicar, EncabezadoSeleccionadoDuplicar)

            Editando = True
            IsBusy = False

            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoDuplicar, EncabezadoSeleccionado)
            HabilitarCamposRegistro(_EncabezadoSeleccionado)
            CambiarColorFondoTextoBuscador()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro duplicado", Me.ToString(), "DuplicarRegistro", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, False)

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
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, False)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación de la busqueda", Me.ToString(), "Buscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Editando = False
        MyBase.CambioItem("Editando")
        MyBase.CancelarBuscar()
    End Sub

    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(BusquedaAjustesManuales) Then
                IsBusy = True
                Editando = False
                MyBase.CambioItem("Editando")
                MyBase.ConfirmarBuscar()
                Await ConsultarEncabezado(False, String.Empty, False, 0, BusquedaAjustesManuales.ID, BusquedaAjustesManuales.TipoComprobante, BusquedaAjustesManuales.UsuarioRegistro, BusquedaAjustesManuales.FechaRegistro, BusquedaAjustesManuales.IDComitente)
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
                                               Optional ByVal pintID As System.Nullable(Of Integer) = 0,
                                               Optional ByVal pstrTipoComprobante As String = "",
                                               Optional ByVal pstrUsuarioRegistro As String = "",
                                               Optional ByVal pdtmFechaRegistro As System.Nullable(Of DateTime) = Nothing,
                                               Optional ByVal plngIDComitente As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.AjustesContablesManual)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.AjustesContablesManuals.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await dcProxy.Load(dcProxy.AjustesContablesManual_FiltrarSyncQuery(pstrFiltro, Program.Usuario, Program.HashConexion)).AsTask()
            Else
                objRet = Await dcProxy.Load(dcProxy.AjustesContablesManual_ConsultarSyncQuery(pintID, pstrTipoComprobante, pstrUsuarioRegistro, pdtmFechaRegistro, plngIDComitente, Program.Usuario, Program.HashConexion)).AsTask()
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
                            If _ListaEncabezado.Where(Function(i) i.ID = pintIDRegistroPosicionar).Count > 0 Then
                                EncabezadoSeleccionado = _ListaEncabezado.Where(Function(i) i.ID = pintIDRegistroPosicionar).First
                            End If
                        End If

                        ReiniciaTimer()
                    Else
                        If objRet.Entities.Count > 0 Then
                            If Not pstrFiltro Then
                                MyBase.ConfirmarBuscar()
                            End If
                        Else
                            If Not plogFiltrar Or (plogFiltrar And Not pstrFiltro.Equals(String.Empty)) Then
                                A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If
            Else
                ListaEncabezado.Clear()
            End If

            Editando = False
            MyBase.CambioItem("Editando")
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

    Public Async Function ConsultarDetalle(ByVal pintID As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.AjustesContablesManual_Detalles)

        Try
            ErrorForma = String.Empty

            dcProxy.AjustesContablesManual_Detalles.Clear()

            objRet = Await dcProxy.Load(dcProxy.AjustesContablesManual_Detalles_ConsultarSyncQuery(pintID, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el de talle de la distribución.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el de talle de la distribución.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el de talle de la distribución.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Overrides Async Sub ActualizarRegistro()
        Try
            IsBusy = True
            dtmFechaServidor = Await ObtenerFechaHoraServidor()
            IsBusy = True

            Await ActualizarAjustesContablesManual(_EncabezadoSeleccionado)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ActualizarAjustesContablesManual(ByVal objRegistroSelected As CFCalculosFinancieros.AjustesContablesManual) As Task
        Try
            IsBusy = True

            If Not IsNothing(objRegistroSelected) Then

                If ValidarGuardadoRegistro(objRegistroSelected) Then
                    Await GuardarAjustesContablesManual(objRegistroSelected)
                Else
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "ActualizarAjustesContablesManual", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Private Async Function GuardarAjustesContablesManual(ByVal objRegistroSelected As CFCalculosFinancieros.AjustesContablesManual) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.AjustesContablesManual_RespuestaValidacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            Dim strDetalleXML As String = String.Empty

            For Each li In _ListaDetalle
                If String.IsNullOrEmpty(strDetalleXML) Then
                    strDetalleXML = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", li.ID, li.IDConceptoContable, li.ConceptoContable, li.TipoMovimiento, li.Tercero, li.CentroCostos, li.Naturaleza, li.CuentaContable, li.Valor, li.IDMvtoContabilidad)
                Else
                    strDetalleXML = String.Format("{0}**{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", strDetalleXML, li.ID, li.IDConceptoContable, li.ConceptoContable, li.TipoMovimiento, li.Tercero, li.CentroCostos, li.Naturaleza, li.CuentaContable, li.Valor, li.IDMvtoContabilidad)
                End If
            Next

            strDetalleXML = Replace(strDetalleXML, ",", ".")

            strDetalleXML = System.Web.HttpUtility.UrlEncode(strDetalleXML)
            Dim strObservaciones As String = System.Web.HttpUtility.UrlEncode(_EncabezadoSeleccionado.Observaciones)

            dcProxy.AjustesContablesManual_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.AjustesContablesManual_ValidarSyncQuery(objRegistroSelected.ID,
                                                                                        objRegistroSelected.TipoComprobante,
                                                                                        objRegistroSelected.UsuarioRegistro,
                                                                                        objRegistroSelected.FechaRegistro,
                                                                                        objRegistroSelected.NormaContable,
                                                                                        objRegistroSelected.IDComitente,
                                                                                        objRegistroSelected.FechaAplicacion,
                                                                                        objRegistroSelected.IDMoneda,
                                                                                        objRegistroSelected.CodMoneda,
                                                                                        objRegistroSelected.TipoAjuste,
                                                                                        objRegistroSelected.IDEventoContable,
                                                                                        objRegistroSelected.CodEventoContable,
                                                                                        objRegistroSelected.ClaseContable,
                                                                                        objRegistroSelected.TipoInversion,
                                                                                        objRegistroSelected.TipoOrigen,
                                                                                        objRegistroSelected.Origen_Numero,
                                                                                        objRegistroSelected.Origen_Secuencia,
                                                                                        objRegistroSelected.Origen_Tipo,
                                                                                        objRegistroSelected.Origen_Clase,
                                                                                        objRegistroSelected.Origen_Fecha,
                                                                                        Program.Usuario,
                                                                                        Program.UsuarioWindows,
                                                                                        Program.Maquina,
                                                                                        strDetalleXML,
                                                                                        objRegistroSelected.Observaciones, Program.HashConexion)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarAjustesContablesManual", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                    IsBusy = False
                Else
                    Dim objListaResultadoValidacion As List(Of CFCalculosFinancieros.AjustesContablesManual_RespuestaValidacion) = objRet.Entities.ToList

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
                                intIDInsertado = CInt(li.IDRegistroIdentity)
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

                            _EncabezadoSeleccionado.ID = intIDInsertado
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro ", Me.ToString(), "GuardarAjustesContablesManual", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Overrides Async Sub TerminoSubmitChanges(ByVal so As SubmitOperation)
        Try
            MyBase.TerminoSubmitChanges(so)

            mostrarMensaje(so.UserState.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

            Await ConsultarEncabezado(True, String.Empty, True, _EncabezadoSeleccionado.ID)

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
                ViewAjustesManuales.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
                logCargoForma = True
            End If

            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.ID <> 0 Then
                    IsBusy = True
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

    Private Sub EditarAjustesContablesManual()
        Try
            EncabezadoSeleccionadoAnterior = Nothing

            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logNuevoRegistro = False

            Editando = True

            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)

            HabilitarCamposRegistro(_EncabezadoSeleccionado)

            IsBusy = False
            CambiarColorFondoTextoBuscador()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro.", Me.ToString(), "EditarAjustesContablesManual", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Function validarEstadoRegistro(ByVal pstrAccion As String) As Task
        Try
            Dim strMsg As String = String.Empty

            If _EncabezadoSeleccionado.Estado <> ESTADO_INGRESADA And pstrAccion <> OPCION_DUPLICAR Then
                MyBase.RetornarValorEdicionNavegacion()
                strMsg = String.Format("No se puede {0} el registro porque se encuentra en estado {1}", IIf(pstrAccion = OPCION_EDITAR, "Editar", "Anular"), IIf(_EncabezadoSeleccionado.Estado = ESTADO_ANULADA, "Anulada", "Pendiente por aprobar"))
                mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.IsBusy = False
            Else
                Me.IsBusy = True
                If pstrAccion = OPCION_EDITAR Then
                    If Await ValidarEstadoAjustesContablesManual(_EncabezadoSeleccionado, "editar") Then
                        EditarAjustesContablesManual()
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
                    If Await ValidarEstadoAjustesContablesManual(_EncabezadoSeleccionado, "anular") Then
                        mostrarMensajePregunta("¿Esta seguro que desea anular el registro?", _
                                  Program.TituloSistema, _
                                  "ANULARREGISTRO", _
                                  AddressOf TerminoMensajePregunta, False, String.Empty, False, True, True, False)
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
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, False)

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

    Public Overrides Async Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.ID <> 0 Then
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
            Dim objRet As LoadOperation(Of CFCalculosFinancieros.AjustesContablesManual_RespuestaValidacion)

            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.AjustesContablesManual_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.AjustesContablesManual_AnularSyncQuery(_EncabezadoSeleccionado.ID, pstrObservaciones, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

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

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmMovimientos"
                    Dim objListaDetalle As New List(Of CFCalculosFinancieros.AjustesContablesManual_Detalles)
                    Dim NewReceptoresOperacion As New CFCalculosFinancieros.AjustesContablesManual_Detalles

                    If Not IsNothing(_ListaDetalle) Then
                        For Each li In _ListaDetalle
                            objListaDetalle.Add(li)
                        Next
                    End If

                    NewReceptoresOperacion.ID = -(objListaDetalle.Count)
                    NewReceptoresOperacion.IDAjusteContable = _EncabezadoSeleccionado.ID
                    NewReceptoresOperacion.CentroCostos = String.Empty
                    NewReceptoresOperacion.ConceptoContable = String.Empty
                    NewReceptoresOperacion.CuentaContable = String.Empty
                    NewReceptoresOperacion.DescripcionConceptoContable = String.Empty
                    NewReceptoresOperacion.DescripcionNaturaleza = String.Empty
                    NewReceptoresOperacion.DescripcionTipoMovimiento = String.Empty
                    NewReceptoresOperacion.Maquina = Program.Maquina
                    NewReceptoresOperacion.Naturaleza = String.Empty
                    NewReceptoresOperacion.Tercero = String.Empty
                    NewReceptoresOperacion.TipoMovimiento = String.Empty
                    NewReceptoresOperacion.Valor = 0
                    NewReceptoresOperacion.Usuario = Program.Usuario
                    NewReceptoresOperacion.UsuarioWindows = Program.Usuario

                    objListaDetalle.Add(NewReceptoresOperacion)

                    ListaDetalle = objListaDetalle
                    DetalleSeleccionado = NewReceptoresOperacion

                    MyBase.CambioItem("ListaDetalle")
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo detalle.", Me.ToString(), "NuevoRegistroDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmMovimientos"
                    If Not IsNothing(_ListaDetalle) Then
                        If Not _DetalleSeleccionado Is Nothing Then
                            Dim objListaDetalles As New List(Of CFCalculosFinancieros.AjustesContablesManual_Detalles)
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetalleSeleccionado, ListaDetalle)

                            For Each li In _ListaDetalle
                                objListaDetalles.Add(li)
                            Next

                            If objListaDetalles.Contains(_DetalleSeleccionado) Then
                                objListaDetalles.Remove(_DetalleSeleccionado)
                            End If

                            DetalleSeleccionado = Nothing
                            ListaDetalle = objListaDetalles
                            If ListaDetalle.Count > 0 Then
                                Program.PosicionarItemLista(DetalleSeleccionado, ListaDetalle, intRegistroPosicionar)
                            End If
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
                ViewAjustesManuales.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
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
            Dim objBusqueda As New CamposBusquedaAjustesManuales
            objBusqueda.ID = 0
            objBusqueda.TipoComprobante = String.Empty
            objBusqueda.UsuarioRegistro = String.Empty
            objBusqueda.IDComitente = String.Empty
            objBusqueda.FechaRegistro = Nothing

            BusquedaAjustesManuales = objBusqueda

            Editando = True
            MyBase.CambioItem("Editando")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al organizar la nueva busqueda.", Me.ToString(), "OrganizarNuevaBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function CargarCombos(ByVal plogCompletos As Boolean, Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.AjustesContablesManual_Combos)

        Try
            If logNuevoRegistro Or logEditarRegistro Then
                IsBusy = True
            End If

            If Not IsNothing(pstrUserState) Then
                pstrUserState = pstrUserState.ToUpper
            End If

            ErrorForma = String.Empty

            dcProxy.AjustesContablesManual_Combos.Clear()

            objRet = Await dcProxy.Load(dcProxy.AjustesContablesManual_ConsultarCombosSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

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
                        Dim objListaNodosCategoria As List(Of CFCalculosFinancieros.AjustesContablesManual_Combos) = Nothing
                        Dim objDiccionario As New Dictionary(Of String, List(Of CFCalculosFinancieros.AjustesContablesManual_Combos))

                        Dim listaCategorias = From lc In objRet.Entities Select lc.Topico Distinct

                        For Each li In listaCategorias
                            strNombreCategoria = li
                            objListaNodosCategoria = (From ln In objRet.Entities Where ln.Topico = strNombreCategoria).ToList
                            objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                        Next

                        DiccionarioCombos = Nothing
                        DiccionarioCombos = objDiccionario

                        If DiccionarioCombos.ContainsKey("NORMACONTABLE_DEFECTO") Then
                            Dim strCodigoParametro As String = CStr(DiccionarioCombos("NORMACONTABLE_DEFECTO").First.Codigo)
                            If Not String.IsNullOrEmpty(strCodigoParametro) Then
                                If Left(DiccionarioCombos("NORMACONTABLE_DEFECTO").First.Codigo, 1) <> "0" Then
                                    strNormaContableDefecto = "0" & DiccionarioCombos("NORMACONTABLE_DEFECTO").First.Codigo
                                Else
                                    strNormaContableDefecto = DiccionarioCombos("NORMACONTABLE_DEFECTO").First.Codigo
                                End If
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

    Public Async Sub SeleccionarCliente(ByVal pobjCliente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjCliente) Then
                If logEditarRegistro Or logNuevoRegistro Then

                    If Not IsNothing(_EncabezadoSeleccionado) Then

                        _EncabezadoSeleccionado.IDComitente = pobjCliente.IdComitente
                        _EncabezadoSeleccionado.NombreCliente = pobjCliente.Nombre
                        _EncabezadoSeleccionado.NroDocumentoCliente = pobjCliente.NroDocumento
                        _EncabezadoSeleccionado.TipoIdentificacionCliente = pobjCliente.CodTipoIdentificacion
                        _EncabezadoSeleccionado.DescripcionTipoIdentificacionCliente = pobjCliente.TipoIdentificacion
                    End If
                End If
            Else
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    _EncabezadoSeleccionado.IDComitente = String.Empty
                    _EncabezadoSeleccionado.NombreCliente = String.Empty
                    _EncabezadoSeleccionado.NroDocumentoCliente = String.Empty
                    _EncabezadoSeleccionado.TipoIdentificacionCliente = String.Empty
                    _EncabezadoSeleccionado.DescripcionTipoIdentificacionCliente = String.Empty

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub HabilitarCamposRegistro(ByVal pobjRegistroSelected As CFCalculosFinancieros.AjustesContablesManual)
        Try
            If Editando Then
                If Not IsNothing(pobjRegistroSelected) Then
                    If pobjRegistroSelected.TipoComprobante = TIPOCOMPROBANTE_BASADOENEVENTO Then

                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, True)

                        MostrarCuentaEvento = Visibility.Visible
                        MostrarCuentaManual = Visibility.Collapsed

                        If pobjRegistroSelected.TipoOrigen = TIPOORIGEN_LIQUIDACION Then
                            MostrarCamposLiquidacion = Visibility.Visible
                            MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                            MostrarCamposCustodia = Visibility.Collapsed
                        ElseIf pobjRegistroSelected.TipoOrigen = TIPOORIGEN_LIQUIDACIONOTROSNEGOCIOS Then
                            MostrarCamposLiquidacion = Visibility.Collapsed
                            MostrarCamposLiquidacionOtrosNegocios = Visibility.Visible
                            MostrarCamposCustodia = Visibility.Collapsed
                        ElseIf pobjRegistroSelected.TipoOrigen = TIPOORIGEN_CUSTODIAS Then
                            MostrarCamposLiquidacion = Visibility.Collapsed
                            MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                            MostrarCamposCustodia = Visibility.Visible
                        Else
                            MostrarCamposLiquidacion = Visibility.Collapsed
                            MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                            MostrarCamposCustodia = Visibility.Collapsed
                        End If
                    ElseIf pobjRegistroSelected.TipoComprobante = TIPOCOMPROBANTE_MANUAL Then
                        MostrarCuentaEvento = Visibility.Collapsed
                        MostrarCuentaManual = Visibility.Visible
                        MostrarCamposLiquidacion = Visibility.Collapsed
                        MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                        MostrarCamposCustodia = Visibility.Visible

                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, False)
                    Else
                        MostrarCuentaEvento = Visibility.Collapsed
                        MostrarCuentaManual = Visibility.Collapsed
                        MostrarCamposLiquidacion = Visibility.Collapsed
                        MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                        MostrarCamposCustodia = Visibility.Visible

                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, False)
                    End If
                End If
            Else
                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITAREVENTO, False)

                If Not IsNothing(pobjRegistroSelected) Then
                    If pobjRegistroSelected.TipoComprobante = TIPOCOMPROBANTE_BASADOENEVENTO Then
                        MostrarCuentaEvento = Visibility.Visible
                        MostrarCuentaManual = Visibility.Collapsed

                        If pobjRegistroSelected.TipoOrigen = TIPOORIGEN_LIQUIDACION Then
                            MostrarCamposLiquidacion = Visibility.Visible
                            MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                            MostrarCamposCustodia = Visibility.Collapsed
                        ElseIf pobjRegistroSelected.TipoOrigen = TIPOORIGEN_LIQUIDACIONOTROSNEGOCIOS Then
                            MostrarCamposLiquidacion = Visibility.Collapsed
                            MostrarCamposLiquidacionOtrosNegocios = Visibility.Visible
                            MostrarCamposCustodia = Visibility.Collapsed
                        ElseIf pobjRegistroSelected.TipoOrigen = TIPOORIGEN_CUSTODIAS Then
                            MostrarCamposLiquidacion = Visibility.Collapsed
                            MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                            MostrarCamposCustodia = Visibility.Visible
                        Else
                            MostrarCamposLiquidacion = Visibility.Collapsed
                            MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                            MostrarCamposCustodia = Visibility.Collapsed
                        End If
                    ElseIf pobjRegistroSelected.TipoComprobante = TIPOCOMPROBANTE_MANUAL Then
                        MostrarCuentaEvento = Visibility.Collapsed
                        MostrarCuentaManual = Visibility.Visible
                    Else
                        MostrarCuentaEvento = Visibility.Collapsed
                        MostrarCuentaManual = Visibility.Collapsed
                    End If
                Else
                    MostrarCuentaEvento = Visibility.Collapsed
                    MostrarCuentaManual = Visibility.Collapsed
                    MostrarCamposLiquidacion = Visibility.Collapsed
                    MostrarCamposLiquidacionOtrosNegocios = Visibility.Collapsed
                    MostrarCamposCustodia = Visibility.Collapsed
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al habilitar los controles dependiendo del tipo de registro.", Me.ToString, "HabilitarCamposRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Function ValidarGuardadoRegistro(ByVal pobjRegistro As CFCalculosFinancieros.AjustesContablesManual) As Boolean
        Try
            'Valida los campos que son requeridos por el sistema de OYDPLUS.
            Dim logPasaValidacion As Boolean = True
            Dim strMensajeValidacion = String.Empty

            If String.IsNullOrEmpty(pobjRegistro.TipoComprobante) Then
                strMensajeValidacion = String.Format("{0}{1} -   Tipo comprobante", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.NormaContable) Then
                strMensajeValidacion = String.Format("{0}{1} -   Norma contable", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.IDComitente) Then
                strMensajeValidacion = String.Format("{0}{1} -   Código OyD", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.FechaAplicacion) Then
                strMensajeValidacion = String.Format("{0}{1} -   Fecha aplicación", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.IDMoneda) Or pobjRegistro.IDMoneda = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Moneda", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.TipoAjuste) Then
                strMensajeValidacion = String.Format("{0}{1} -   Tipo ajuste", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.Observaciones) Then
                strMensajeValidacion = String.Format("{0}{1} -   Detalle", strMensajeValidacion, vbCrLf)
            End If

            If pobjRegistro.TipoComprobante = TIPOCOMPROBANTE_BASADOENEVENTO Then
                If IsNothing(pobjRegistro.IDEventoContable) Or pobjRegistro.IDEventoContable = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} -   Evento", strMensajeValidacion, vbCrLf)
                End If
                If String.IsNullOrEmpty(pobjRegistro.ClaseContable) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Clase contable", strMensajeValidacion, vbCrLf)
                End If
                'If String.IsNullOrEmpty(pobjRegistro.TipoInversion) Then
                '    strMensajeValidacion = String.Format("{0}{1} -   Tipo inversión", strMensajeValidacion, vbCrLf)
                'End If
                If String.IsNullOrEmpty(pobjRegistro.TipoOrigen) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Tipo origen", strMensajeValidacion, vbCrLf)
                ElseIf pobjRegistro.TipoOrigen = TIPOORIGEN_LIQUIDACION Then
                    If IsNothing(pobjRegistro.Origen_Numero) Or pobjRegistro.Origen_Numero = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Número", strMensajeValidacion, vbCrLf)
                    End If
                    If IsNothing(pobjRegistro.Origen_Secuencia) Or pobjRegistro.Origen_Secuencia = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Parcial", strMensajeValidacion, vbCrLf)
                    End If
                    If String.IsNullOrEmpty(pobjRegistro.Origen_Tipo) Then
                        strMensajeValidacion = String.Format("{0}{1} -   Tipo", strMensajeValidacion, vbCrLf)
                    End If
                    If String.IsNullOrEmpty(pobjRegistro.Origen_Clase) Then
                        strMensajeValidacion = String.Format("{0}{1} -   Clase", strMensajeValidacion, vbCrLf)
                    End If
                    If IsNothing(pobjRegistro.Origen_Fecha) Then
                        strMensajeValidacion = String.Format("{0}{1} -   Fecha", strMensajeValidacion, vbCrLf)
                    End If
                ElseIf pobjRegistro.TipoOrigen = TIPOORIGEN_LIQUIDACIONOTROSNEGOCIOS Then
                    If IsNothing(pobjRegistro.Origen_Numero) Or pobjRegistro.Origen_Numero = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Número", strMensajeValidacion, vbCrLf)
                    End If
                ElseIf pobjRegistro.TipoOrigen = TIPOORIGEN_CUSTODIAS Then
                    If IsNothing(pobjRegistro.Origen_Numero) Or pobjRegistro.Origen_Numero = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Número", strMensajeValidacion, vbCrLf)
                    End If
                    If IsNothing(pobjRegistro.Origen_Secuencia) Or pobjRegistro.Origen_Secuencia = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Secuencia", strMensajeValidacion, vbCrLf)
                    End If
                End If
            End If

            If String.IsNullOrEmpty(strMensajeValidacion) Then
                If Not IsNothing(_ListaDetalle) Then
                    If _ListaDetalle.Count > 0 Then
                        Dim logConceptoVacio As Boolean = False
                        Dim logNaturaleza As Boolean = False
                        Dim logCuentaContable As Boolean = False
                        Dim logCuentaContableManualInvalida As Boolean = False
                        Dim logValor As Boolean = False
                        Dim dblSumaCreditos As Double = 0
                        Dim dblSumaDebitos As Double = 0

                        If _ListaDetalle.Where(Function(i) String.IsNullOrEmpty(i.ConceptoContable)).Count > 0 Then
                            logConceptoVacio = True
                        End If

                        If _ListaDetalle.Where(Function(i) String.IsNullOrEmpty(i.Naturaleza)).Count > 0 Then
                            logNaturaleza = True
                        End If

                        If _ListaDetalle.Where(Function(i) String.IsNullOrEmpty(i.CuentaContable)).Count > 0 Then
                            logCuentaContable = True
                        End If

                        If logCuentaContable = False Then
                            If pobjRegistro.TipoComprobante = TIPOCOMPROBANTE_MANUAL Then
                                If _ListaDetalle.Where(Function(i) Not String.IsNullOrEmpty(i.CuentaContable) And Len(i.CuentaContable) < 4).Count > 0 Then
                                    logCuentaContableManualInvalida = True
                                End If
                            End If
                        End If
                        
                        If _ListaDetalle.Where(Function(i) CDbl(i.Valor) = 0).Count > 0 Then
                            logValor = True
                        End If

                        If logConceptoVacio Then
                            strMensajeValidacion = String.Format("{0}{1} -   El Concepto contable es requerido en todos los detalles", strMensajeValidacion, vbCrLf)
                        End If
                        If logNaturaleza Then
                            strMensajeValidacion = String.Format("{0}{1} -   La naturaleza es repetido en todos los detalles", strMensajeValidacion, vbCrLf)
                        End If
                        If logCuentaContable Then
                            strMensajeValidacion = String.Format("{0}{1} -   La cuenta contable es requerido en todos los detalles", strMensajeValidacion, vbCrLf)
                        End If
                        If logCuentaContableManualInvalida Then
                            strMensajeValidacion = String.Format("{0}{1} -   La cuenta contable tiene que tener una longitud entre 4 y 16 caracteres en todos los detalles", strMensajeValidacion, vbCrLf)
                        End If
                        If logValor Then
                            strMensajeValidacion = String.Format("{0}{1} -   El valor es requerido en todos los detalles", strMensajeValidacion, vbCrLf)
                        End If

                        For Each li In _ListaDetalle
                            If li.Naturaleza = NATURALEZA_DB Then
                                dblSumaDebitos = CDbl(dblSumaDebitos + li.Valor)
                            ElseIf li.Naturaleza = NATURALEZA_CR Then
                                dblSumaCreditos = CDbl(dblSumaCreditos + li.Valor)
                            End If
                        Next

                        If dblSumaDebitos <> dblSumaCreditos Then
                            strMensajeValidacion = String.Format("{0}{1} -   El valor débitos debe ser igual al valor créditos.", strMensajeValidacion, vbCrLf)
                        End If

                        If Not String.IsNullOrEmpty(strMensajeValidacion) Then
                            logPasaValidacion = False
                            strMensajeValidacion = String.Format("Señor usuario, debe de corregir las siguientes inconsistencias en el detalle.{0}{1}", strMensajeValidacion, vbCrLf)
                            mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        logPasaValidacion = False
                        strMensajeValidacion = "Señor usuario, debe de ingresar al menos un registro en los detalles."
                        mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    logPasaValidacion = False
                    strMensajeValidacion = "Señor usuario, debe de ingresar al menos un registro en los detalles."
                    mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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

    Public Sub BuscarControlValidacion(ByVal pViewOperaciones As FormaAjustesManualesView, ByVal pstrOpcion As String)
        Try
            If Not IsNothing(pViewOperaciones) Then
                If TypeOf pViewOperaciones.FindName(pstrOpcion) Is TabItem Then
                    CType(pViewOperaciones.FindName(pstrOpcion), TabItem).IsSelected = True
                ElseIf TypeOf pViewOperaciones.FindName(pstrOpcion) Is TextBox Then
                    CType(pViewOperaciones.FindName(pstrOpcion), TextBox).Focus()
                ElseIf TypeOf pViewOperaciones.FindName(pstrOpcion) Is ComboBox Then
                    CType(pViewOperaciones.FindName(pstrOpcion), ComboBox).Focus()


                ElseIf TypeOf pViewOperaciones.FindName(pstrOpcion) Is A2Utilidades.A2NumericBox Then
                    CType(pViewOperaciones.FindName(pstrOpcion), A2Utilidades.A2NumericBox).Focus()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control dentro del registro.", Me.ToString, "BuscarControlValidacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub ObtenerValoresRegistroAnterior(ByVal pobjRegistro As CFCalculosFinancieros.AjustesContablesManual, ByRef pobjRegistroSalvarDatos As CFCalculosFinancieros.AjustesContablesManual)
        Try
            If Not IsNothing(pobjRegistro) Then
                Dim objNewRegistro As New CFCalculosFinancieros.AjustesContablesManual

                objNewRegistro.ID = pobjRegistro.ID
                objNewRegistro.Estado = pobjRegistro.Estado
                objNewRegistro.DescripcionEstado = pobjRegistro.DescripcionEstado
                objNewRegistro.TipoComprobante = pobjRegistro.TipoComprobante
                objNewRegistro.DescripcionTipoComprobante = pobjRegistro.DescripcionTipoComprobante
                objNewRegistro.UsuarioRegistro = pobjRegistro.UsuarioRegistro
                objNewRegistro.FechaRegistro = pobjRegistro.FechaRegistro
                objNewRegistro.NormaContable = pobjRegistro.NormaContable
                objNewRegistro.DescripcionNormaContable = pobjRegistro.DescripcionNormaContable
                objNewRegistro.IDComitente = pobjRegistro.IDComitente
                objNewRegistro.NombreCliente = pobjRegistro.NombreCliente
                objNewRegistro.TipoIdentificacionCliente = pobjRegistro.TipoIdentificacionCliente
                objNewRegistro.DescripcionTipoIdentificacionCliente = pobjRegistro.DescripcionTipoIdentificacionCliente
                objNewRegistro.NroDocumentoCliente = pobjRegistro.NroDocumentoCliente
                objNewRegistro.FechaAplicacion = pobjRegistro.FechaAplicacion
                objNewRegistro.IDMoneda = pobjRegistro.IDMoneda
                objNewRegistro.CodMoneda = pobjRegistro.CodMoneda
                objNewRegistro.DescripcionMoneda = pobjRegistro.DescripcionMoneda
                objNewRegistro.TipoAjuste = pobjRegistro.TipoAjuste
                objNewRegistro.DescripcionTipoAjuste = pobjRegistro.DescripcionTipoAjuste
                objNewRegistro.IDEventoContable = pobjRegistro.IDEventoContable
                objNewRegistro.CodEventoContable = pobjRegistro.CodEventoContable
                objNewRegistro.EventoContable = pobjRegistro.EventoContable
                objNewRegistro.ClaseContable = pobjRegistro.ClaseContable
                objNewRegistro.DescripcionClaseContable = pobjRegistro.DescripcionClaseContable
                objNewRegistro.TipoInversion = pobjRegistro.TipoInversion
                objNewRegistro.DescripcionTipoInversion = pobjRegistro.DescripcionTipoInversion
                objNewRegistro.TipoOrigen = pobjRegistro.TipoOrigen
                objNewRegistro.Origen_Numero = pobjRegistro.Origen_Numero
                objNewRegistro.Origen_Secuencia = pobjRegistro.Origen_Secuencia
                objNewRegistro.Origen_Tipo = pobjRegistro.Origen_Tipo
                objNewRegistro.Origen_Clase = pobjRegistro.Origen_Clase
                objNewRegistro.Origen_Fecha = pobjRegistro.Origen_Fecha
                objNewRegistro.Observaciones = pobjRegistro.Observaciones
                objNewRegistro.Actualizacion = pobjRegistro.Actualizacion
                objNewRegistro.Usuario = pobjRegistro.Usuario
                objNewRegistro.UsuarioWindows = pobjRegistro.UsuarioWindows
                objNewRegistro.Maquina = pobjRegistro.Maquina
                objNewRegistro.ValorMovimiento = pobjRegistro.ValorMovimiento

                pobjRegistroSalvarDatos = objNewRegistro
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", Me.ToString(), "ObtenerValoresRegistroAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub LimpiarDatosTipoNegocio(ByVal pobjRegistroSeleccionado As CFCalculosFinancieros.AjustesContablesManual, Optional ByVal pstrTipo As String = "")
        Try
            If Not IsNothing(pobjRegistroSeleccionado) Then
                logCalcularValores = False

                If pstrTipo = LIMPIARNEGOCIO_TIPOCOMPROBANTE Then
                    pobjRegistroSeleccionado.IDEventoContable = Nothing
                    pobjRegistroSeleccionado.CodEventoContable = String.Empty
                    pobjRegistroSeleccionado.EventoContable = String.Empty
                    pobjRegistroSeleccionado.ClaseContable = String.Empty
                    pobjRegistroSeleccionado.TipoInversion = String.Empty
                    pobjRegistroSeleccionado.TipoOrigen = String.Empty
                    pobjRegistroSeleccionado.Origen_Numero = Nothing
                    pobjRegistroSeleccionado.Origen_Secuencia = Nothing
                    pobjRegistroSeleccionado.Origen_Tipo = Nothing
                    pobjRegistroSeleccionado.Origen_Clase = Nothing
                    pobjRegistroSeleccionado.Origen_Fecha = Nothing

                    If Not IsNothing(_ListaDetalle) Then
                        For Each li In _ListaDetalle
                            li.ConceptoContable = String.Empty
                            li.CuentaContable = String.Empty
                        Next
                    End If
                ElseIf pstrTipo = LIMPIARNEGOCIO_TIPOORIGEN Then
                    pobjRegistroSeleccionado.Origen_Numero = Nothing
                    pobjRegistroSeleccionado.Origen_Secuencia = Nothing
                    pobjRegistroSeleccionado.Origen_Tipo = Nothing
                    pobjRegistroSeleccionado.Origen_Clase = Nothing
                    pobjRegistroSeleccionado.Origen_Fecha = Nothing

                    If Not IsNothing(_ListaDetalle) Then
                        For Each li In _ListaDetalle
                            li.ConceptoContable = String.Empty
                            li.CuentaContable = String.Empty
                        Next
                    End If
                End If

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
                    IDRegistroPosicionar = _EncabezadoSeleccionado.ID
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

    Private Async Function ValidarEstadoAjustesContablesManual(ByVal objRegistroSelected As CFCalculosFinancieros.AjustesContablesManual, ByVal pstrAccion As String) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.AjustesContablesManual_RespuestaValidacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.AjustesContablesManual_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.AjustesContablesManual_ValidarEstadoSyncQuery(_EncabezadoSeleccionado.ID, pstrAccion, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarAjustesContablesManual", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                    IsBusy = False
                Else
                    Dim objListaResultadoValidacion As List(Of CFCalculosFinancieros.AjustesContablesManual_RespuestaValidacion) = objRet.Entities.ToList

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al validar el estado del registro.", Me.ToString(), "ValidarEstadoAjustesContablesManual", Application.Current.ToString(), Program.Maquina, ex)
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

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.TipoComprobante = TIPOCOMPROBANTE_BASADOENEVENTO Then
                    colorFondo = Program.colorFromHex(COLOR_HABILITADO)
                Else
                    colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
                End If
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            
            FondoTextoBuscadoresEventoContable = New SolidColorBrush(colorFondo)
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

    Private Sub RecargarOrdenDespuesGuardado(ByVal pstrMensaje As String)
        Try
            logCancelarRegistro = False
            logEditarRegistro = False
            logDuplicarRegistro = False
            logNuevoRegistro = False

            'Esto se realiza para habilitar los botones de navegación llamando el metodo TERMINOSUBMITCHANGED
            If dcProxy.AjustesContablesManuals.Where(Function(i) i.ID = _EncabezadoSeleccionado.ID).Count = 0 Then
                dcProxy.AjustesContablesManuals.Add(_EncabezadoSeleccionado)
            End If

            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, pstrMensaje)
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

#Region "Eventos"

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try
            If logCalcularValores Then
                Select Case e.PropertyName.ToLower()
                    Case "tipocomprobante"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoComprobante) Then
                            LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARNEGOCIO_TIPOCOMPROBANTE)
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
                            CambiarColorFondoTextoBuscador()
                        End If
                    Case "tipoorigen"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoOrigen) Then
                            LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARNEGOCIO_TIPOORIGEN)
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
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
Public Class CamposBusquedaAjustesManuales
    Implements INotifyPropertyChanged

    Private _ID As Integer
    <Display(Name:="ID Operación")> _
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _TipoComprobante As String
    <Display(Name:="Tipo comprobante")> _
    Public Property TipoComprobante() As String
        Get
            Return _TipoComprobante
        End Get
        Set(ByVal value As String)
            _TipoComprobante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoComprobante"))
        End Set
    End Property

    Private _UsuarioRegistro As String
    <Display(Name:="Usuario registro")> _
    Public Property UsuarioRegistro() As String
        Get
            Return _UsuarioRegistro
        End Get
        Set(ByVal value As String)
            _UsuarioRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("UsuarioRegistro"))
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

    Private _IDComitente As String
    <Display(Name:="Código OyD")> _
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
