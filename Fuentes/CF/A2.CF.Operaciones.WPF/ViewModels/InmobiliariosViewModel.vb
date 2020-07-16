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
Imports A2.OyD.OYDServer.RIA.Web.CFOperaciones
Imports System.Threading.Tasks

Public Class InmobiliariosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            dcProxy = inicializarProxyOperacionesOtrosNegocios()
            dcProxy1 = inicializarProxyOperacionesOtrosNegocios()
            mdcProxyUtilidad = inicializarProxyUtilidadesOYD()

            If System.Diagnostics.Debugger.IsAttached Then

            End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de registro que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                IsBusy = True
                viewFormaInmuebles = New FormaInmobiliariosView(Me)

                Dim objDiccionarioHabilitarCampos = New Dictionary(Of String, Boolean)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE.ToString, False)

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

    Private Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String)
        Dim objRetorno As String = String.Empty

        If Not String.IsNullOrEmpty(strProgram) Then
            objRetorno = strProgram
        Else
            objRetorno = strRetornoOpcional
        End If

        Return objRetorno
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

    Private dcProxy As OperacionesCFDomainContext
    Private dcProxy1 As OperacionesCFDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext

    Private mdtmFechaCierrePortafolio As DateTime? = Nothing

    Public logNuevoRegistro As Boolean = False
    Public logEditarRegistro As Boolean = False
    Public logCancelarRegistro As Boolean = False
    Public logDuplicarRegistro As Boolean = False
    Public logCalcularValores As Boolean = True
    Public strTipoCalculo As String = String.Empty

    Dim viewFormaInmuebles As FormaInmobiliariosView = Nothing

    Dim dtmFechaServidor As DateTime
    Dim logCambiarSelected As Boolean = True
    Dim logCambiarValoresEnSelected As Boolean = True
    Dim logCargoForma As Boolean = False

    Dim intMonedaLocal As Integer = 0
    Dim intPaisLocal As Integer = 0
    Public dblPrecioConsultado As Double = 0

    Public viewDetalle As DetalleInmobiliarioView
    Dim intCantidadMaximaDiasEntreFechas As Integer = 60

#End Region

#Region "Constantes"
    Private TOOLBARACTIVOPANTALLA As String = "A2Consola_ToolbarActivo"
    Private MDTM_FECHA_CIERRE_SIN_ACTUALIZAR As Date = New Date(1999, 1, 1) 'CCM20151107: Fecha para inicializar fecha de cierre
    Private MINT_LONG_MAX_CODIGO_OYD As Integer = 17

    Public TIPOINMUEBLE_LOTE As String = "L"
    Public TIPOINMUEBLE_CASA As String = "C"
    Public TIPOINMUEBLE_APARTAMENTO As String = "A"
    Public TIPOINMUEBLE_OFICINAL As String = "O"

    Private ESTADO_INGRESADA As String = "I"
    Private ESTADO_ANULADA As String = "A"

    Private Const OPCION_INICIO As String = "INICIO"
    Private Const OPCION_NUEVO As String = "NUEVO"
    Private Const OPCION_EDITAR As String = "EDITAR"
    Private Const OPCION_ANULAR As String = "ANULAR"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    Private Const OPCION_ACTUALIZAR As String = "ACTUALIZAR"

    Private Const COLOR_DESHABILITADO As String = "#E2E2E2"
    Private Const COLOR_HABILITADO As String = "#FFFFFFFF"

    Private Const TIPODOCUMENTOCONCEPTO_EGRESOS As String = "EGRESOS"
    Private Const TIPODOCUMENTOCONCEPTO_NOTAS As String = "NOTAS"
    Private Const TIPODOCUMENTOCONCEPTO_CAJA As String = "CAJA"

    Private Const TIPOMOVIMIENTOCONCEPTO_BANCOXBALANCE As String = "BBA"
    Private Const TIPOMOVIMIENTOCONCEPTO_BANCOXPYG As String = "BP"

    Private Const FORMAPAGO_CHEQUE As String = "C"

    Public Enum OPCIONES_HABILITARCAMPOS
        HABILITARENCABEZADO
        HABILITARNEGOCIO
        HABILITARTASACAMBIO
        HABILITARDETALLE
    End Enum

    Public Enum OPCIONES_VALIDARCAMPOS
        AREA
        CODIGOCATASTRO
        DIRECCION
        DESCRIPCION
    End Enum

#End Region

#Region "Propiedades para el Tipo de registro"

    Private _ListaEncabezado As List(Of CFOperaciones.Inmobiliarios)
    Public Property ListaEncabezado() As List(Of CFOperaciones.Inmobiliarios)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CFOperaciones.Inmobiliarios))
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

    Private _EncabezadoDataForm As CFOperaciones.Inmobiliarios = New CFOperaciones.Inmobiliarios
    Public Property EncabezadoDataForm() As CFOperaciones.Inmobiliarios
        Get
            Return _EncabezadoDataForm
        End Get
        Set(ByVal value As CFOperaciones.Inmobiliarios)
            _EncabezadoDataForm = value
            MyBase.CambioItem("EncabezadoDataForm")
        End Set
    End Property

    Private WithEvents _EncabezadoSeleccionado As CFOperaciones.Inmobiliarios
    Public Property EncabezadoSeleccionado() As CFOperaciones.Inmobiliarios
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CFOperaciones.Inmobiliarios)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If logCambiarValoresEnSelected Then
                    mdtmFechaCierrePortafolio = MDTM_FECHA_CIERRE_SIN_ACTUALIZAR

                    If _EncabezadoSeleccionado.ID > 0 Then
                        ConsultarDetalle(_EncabezadoSeleccionado.ID)
                    End If
                End If
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private _EncabezadoSeleccionadoAnterior As CFOperaciones.Inmobiliarios
    Public Property EncabezadoSeleccionadoAnterior() As CFOperaciones.Inmobiliarios
        Get
            Return _EncabezadoSeleccionadoAnterior
        End Get
        Set(ByVal value As CFOperaciones.Inmobiliarios)
            _EncabezadoSeleccionadoAnterior = value
        End Set
    End Property

    Private _EncabezadoSeleccionadoDuplicar As CFOperaciones.Inmobiliarios
    Public Property EncabezadoSeleccionadoDuplicar() As CFOperaciones.Inmobiliarios
        Get
            Return _EncabezadoSeleccionadoDuplicar
        End Get
        Set(ByVal value As CFOperaciones.Inmobiliarios)
            _EncabezadoSeleccionadoDuplicar = value
        End Set
    End Property

    Private _ViewInmuebles As InmobiliariosView
    Public Property ViewInmuebles() As InmobiliariosView
        Get
            Return _ViewInmuebles
        End Get
        Set(ByVal value As InmobiliariosView)
            _ViewInmuebles = value
        End Set
    End Property

    Private _BusquedaInmueble As CamposBusquedaInmuebles = New CamposBusquedaInmuebles
    Public Property BusquedaInmueble() As CamposBusquedaInmuebles
        Get
            Return _BusquedaInmueble
        End Get
        Set(ByVal value As CamposBusquedaInmuebles)
            _BusquedaInmueble = value
            MyBase.CambioItem("BusquedaInmueble")
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

    Private _CantidadDecimales As String = "3"
    Public Property CantidadDecimales() As String
        Get
            Return _CantidadDecimales
        End Get
        Set(ByVal value As String)
            _CantidadDecimales = value
            MyBase.CambioItem("CantidadDecimales")
        End Set
    End Property

    Private _MaximoBaseIVA As String = "100"
    Public Property MaximoBaseIVA() As String
        Get
            Return _MaximoBaseIVA
        End Get
        Set(ByVal value As String)
            _MaximoBaseIVA = value
            MyBase.CambioItem("MaximoBaseIVA")
        End Set
    End Property


#End Region

#Region "Propiedades para cargar Información de los Combos"

    Private _DiccionarioCombos As Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos))
    Public Property DiccionarioCombos() As Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos))
        Get
            Return _DiccionarioCombos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos)))
            _DiccionarioCombos = value
            MyBase.CambioItem("DiccionarioCombos")
        End Set
    End Property

    Private _DiccionarioCombosCompleta As Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos))
    Public Property DiccionarioCombosCompleta() As Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos))
        Get
            Return _DiccionarioCombosCompleta
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos)))
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

    Private _IsBusyDetalles As Boolean
    Public Property IsBusyDetalles() As Boolean
        Get
            Return _IsBusyDetalles
        End Get
        Set(ByVal value As Boolean)
            _IsBusyDetalles = value
            MyBase.CambioItem("IsBusyDetalles")
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

#Region "Detalles"

    Private _ListaDetalle As List(Of Inmobiliarios_Detalles)
    Public Property ListaDetalle() As List(Of Inmobiliarios_Detalles)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of Inmobiliarios_Detalles))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
        End Set
    End Property

    Private WithEvents _DetalleSeleccionado As Inmobiliarios_Detalles
    Public Property DetalleSeleccionado() As Inmobiliarios_Detalles
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As Inmobiliarios_Detalles)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    Private _FechaInicialFiltro As Nullable(Of DateTime)
    Public Property FechaInicialFiltro() As Nullable(Of DateTime)
        Get
            Return _FechaInicialFiltro
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaInicialFiltro = value
            ValidarMaximosDiasEntreFechasFiltro()
            MyBase.CambioItem("FechaInicialFiltro")
        End Set
    End Property

    Private _FechaFinalFiltro As Nullable(Of DateTime)
    Public Property FechaFinalFiltro() As Nullable(Of DateTime)
        Get
            Return _FechaFinalFiltro
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaFinalFiltro = value
            ValidarMaximosDiasEntreFechasFiltro()
            MyBase.CambioItem("FechaFinalFiltro")
        End Set
    End Property

    Private _ConceptoFiltro As Nullable(Of Integer)
    Public Property ConceptoFiltro() As Nullable(Of Integer)
        Get
            Return _ConceptoFiltro
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ConceptoFiltro = value
            MyBase.CambioItem("ConceptoFiltro")
        End Set
    End Property

    Private _TipoMovimientoNuevo As String
    Public Property TipoMovimientoNuevo() As String
        Get
            Return _TipoMovimientoNuevo
        End Get
        Set(ByVal value As String)
            _TipoMovimientoNuevo = value
            If _TipoMovimientoNuevo = TIPODOCUMENTOCONCEPTO_NOTAS Then
                MostrarSeleccionConceptoNota = Visibility.Visible
            Else
                MostrarSeleccionConceptoNota = Visibility.Collapsed
            End If
            MyBase.CambioItem("TipoMovimientoNuevo")
        End Set
    End Property

    Private _ConceptoSeleccionadoNuevoRegistro As OYDUtilidades.BuscadorGenerico
    Public Property ConceptoSeleccionadoNuevoRegistro() As OYDUtilidades.BuscadorGenerico
        Get
            Return _ConceptoSeleccionadoNuevoRegistro
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
            _ConceptoSeleccionadoNuevoRegistro = value
            MyBase.CambioItem("ConceptoSeleccionadoNuevoRegistro")
        End Set
    End Property


    Private _IDConceptoNuevoRegistro As Integer
    Public Property IDConceptoNuevoRegistro() As Integer
        Get
            Return _IDConceptoNuevoRegistro
        End Get
        Set(ByVal value As Integer)
            _IDConceptoNuevoRegistro = value
            MyBase.CambioItem("IDConceptoNuevoRegistro")
        End Set
    End Property

    Private _ConceptoNuevoRegistro As String
    Public Property ConceptoNuevoRegistro() As String
        Get
            Return _ConceptoNuevoRegistro
        End Get
        Set(ByVal value As String)
            _ConceptoNuevoRegistro = value
            MyBase.CambioItem("ConceptoNuevoRegistro")
        End Set
    End Property

    Private _MostrarSeleccionConceptoNota As Visibility
    Public Property MostrarSeleccionConceptoNota() As Visibility
        Get
            Return _MostrarSeleccionConceptoNota
        End Get
        Set(ByVal value As Visibility)
            _MostrarSeleccionConceptoNota = value
            MyBase.CambioItem("MostrarSeleccionConceptoNota")
        End Set
    End Property

#End Region

#End Region

#Region "Metodos"

    Public Overrides Async Sub NuevoRegistro()
        Try
            If logCargoForma = False Then
                ViewInmuebles.GridEdicion.Children.Add(viewFormaInmuebles)
                logCargoForma = True
            End If

            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            dtmFechaServidor = Await ObtenerFechaHoraServidor()

            If Not IsNothing(_EncabezadoSeleccionado) Then
                EncabezadoSeleccionadoAnterior = Nothing
                ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)
            End If

            logNuevoRegistro = True
            logEditarRegistro = False
            logDuplicarRegistro = False
            logCancelarRegistro = False

            NuevaOperacion()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub NuevaOperacion()
        Try
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE, True)

            Dim objNewInmobiliarios As New CFOperaciones.Inmobiliarios
            objNewInmobiliarios.ID = 0
            objNewInmobiliarios.Codigo = String.Empty
            objNewInmobiliarios.Estado = ESTADO_INGRESADA
            objNewInmobiliarios.NombreEstado = "Ingresada"
            objNewInmobiliarios.Tipo = String.Empty
            objNewInmobiliarios.Area = String.Empty
            objNewInmobiliarios.CodigoCatastro = String.Empty
            objNewInmobiliarios.Direccion = String.Empty
            objNewInmobiliarios.IDComitente = String.Empty
            objNewInmobiliarios.NombreCliente = String.Empty
            objNewInmobiliarios.CodTipoIdentificacionCliente = String.Empty
            objNewInmobiliarios.TipoIdentificacionCliente = String.Empty
            objNewInmobiliarios.NumeroDocumentoCliente = String.Empty
            objNewInmobiliarios.FechaCompra = dtmFechaServidor
            objNewInmobiliarios.ValorCompra = 0
            objNewInmobiliarios.MonedaNegociacion = 0
            objNewInmobiliarios.CodigoMonedaNegociacion = String.Empty
            objNewInmobiliarios.NombreMonedaNegociacion = String.Empty
            objNewInmobiliarios.MonedaLocal = 0
            objNewInmobiliarios.CodigoMonedaLocal = String.Empty
            objNewInmobiliarios.NombreMonedaLocal = String.Empty
            objNewInmobiliarios.TasaCambio = 0
            objNewInmobiliarios.Descripcion = String.Empty
            objNewInmobiliarios.FechaRegistro = dtmFechaServidor
            objNewInmobiliarios.Actualizacion = dtmFechaServidor
            objNewInmobiliarios.Usuario = Program.Usuario
            objNewInmobiliarios.UsuarioWindows = Program.UsuarioWindows
            objNewInmobiliarios.Maquina = Program.Maquina

            Editando = True

            ObtenerValoresRegistroAnterior(objNewInmobiliarios, EncabezadoSeleccionado)
            ObtenerMonedaLocal()

            CambiarColorFondoTextoBuscador()

            ListaDetalle = Nothing
            DetalleSeleccionado = Nothing

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevaOperacion", Program.TituloSistema, Program.Maquina, ex)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE, False)

            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)
            Editando = False
        End Try
    End Sub

    Public Sub PreguntarDuplicarRegistro()
        Try
            If logCargoForma = False Then
                ViewInmuebles.GridEdicion.Children.Add(viewFormaInmuebles)
                logCargoForma = True
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                validarEstadoRegistro(OPCION_DUPLICAR)
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
            Dim objNewRegistroDuplicar As New CFOperaciones.Inmobiliarios
            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, objNewRegistroDuplicar)

            objNewRegistroDuplicar.ID = 0
            objNewRegistroDuplicar.FechaRegistro = dtmFechaServidor
            objNewRegistroDuplicar.FechaCumplimiento = dtmFechaServidor
            objNewRegistroDuplicar.Actualizacion = dtmFechaServidor
            objNewRegistroDuplicar.Usuario = Program.Usuario
            objNewRegistroDuplicar.UsuarioWindows = Program.UsuarioWindows
            objNewRegistroDuplicar.Maquina = Program.Maquina
            objNewRegistroDuplicar.Estado = ESTADO_INGRESADA
            objNewRegistroDuplicar.NombreEstado = "Ingresada"
            objNewRegistroDuplicar.Codigo = String.Empty
            objNewRegistroDuplicar.FechaUltimaValoracion = Nothing
            objNewRegistroDuplicar.ValorCompraActualizado = 0

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = True
            logNuevoRegistro = False

            ListaDetalle = Nothing
            ListaDetalle = New List(Of Inmobiliarios_Detalles)

            IsBusy = False
            Editando = True

            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE, True)

            ObtenerValoresRegistroAnterior(objNewRegistroDuplicar, EncabezadoSeleccionado)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro duplicado", Me.ToString(), "DuplicarRegistro", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE, False)

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
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE, False)
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
            If Not IsNothing(BusquedaInmueble) Then
                IsBusy = True

                Await ConsultarEncabezado(False, String.Empty, False, 0, _BusquedaInmueble.ID, _BusquedaInmueble.Codigo, _BusquedaInmueble.Tipo, _BusquedaInmueble.Estado, _BusquedaInmueble.FechaCompra, _BusquedaInmueble.IDComitente)
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
                                               Optional ByVal pstrCodigo As String = "",
                                               Optional ByVal pstrTipo As String = "",
                                               Optional ByVal pstrEstado As String = "",
                                               Optional ByVal pdtmFechaCompra As Nullable(Of DateTime) = Nothing,
                                               Optional ByVal pstrComitente As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Inmobiliarios)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Inmobiliarios.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await dcProxy.Load(dcProxy.InmobiliariosFiltrarSyncQuery(pstrFiltro, Program.Usuario, Program.HashConexion)).AsTask()
            Else
                objRet = Await dcProxy.Load(dcProxy.InmobiliariosConsultarSyncQuery(pintID, pstrCodigo, pdtmFechaCompra, pstrComitente, pstrTipo, pstrEstado, Program.Usuario, Program.HashConexion)).AsTask()
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
                        If Not plogFiltrar Or (plogFiltrar And Not pstrFiltro.Equals(String.Empty)) Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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

    Public Async Function ConsultarDetalle(ByVal pintIDOperacion As System.Nullable(Of Integer),
                                           Optional ByVal pdtmFechaInicial As System.Nullable(Of DateTime) = Nothing,
                                           Optional ByVal pdtmFechaFinal As System.Nullable(Of DateTime) = Nothing,
                                           Optional ByVal pintIDConcepto As System.Nullable(Of Integer) = Nothing) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Inmobiliarios_Detalles)

        Try
            IsBusyDetalles = True
            ErrorForma = String.Empty

            dcProxy.Inmobiliarios_Detalles.Clear()

            objRet = Await dcProxy.Load(dcProxy.InmobiliariosDetalles_ConsultarSyncQuery(pintIDOperacion, pdtmFechaInicial, pdtmFechaFinal, pintIDConcepto, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la distribución.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la distribución.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la distribución.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusyDetalles = False

        Return (logResultado)
    End Function

    Public Overrides Async Sub ActualizarRegistro()
        Try
            IsBusy = True
            dtmFechaServidor = Await ObtenerFechaHoraServidor()
            IsBusy = True

            If CalcularValorRegistro() Then
                Await ActualizarInmueble(_EncabezadoSeleccionado)
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ActualizarInmueble(ByVal objRegistroSelected As CFOperaciones.Inmobiliarios) As Task
        Try
            IsBusy = True

            If Not IsNothing(objRegistroSelected) Then

                If Await ValidarFechaCierrePortafolio(_EncabezadoSeleccionado.FechaCompra.Value, "Fecha de compra") Then
                    If ValidarGuardadoRegistro(objRegistroSelected) Then
                        Await GuardarInmueble(_EncabezadoSeleccionado)
                    Else
                        IsBusy = False
                    End If
                Else
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "ActualizarInmueble", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Private Async Function GuardarInmueble(ByVal objRegistroSelected As CFOperaciones.Inmobiliarios) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_RespuestaValidacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            Dim strCodigo As String = System.Web.HttpUtility.UrlEncode(_EncabezadoSeleccionado.Codigo)
            Dim strArea As String = System.Web.HttpUtility.UrlEncode(_EncabezadoSeleccionado.Area)
            Dim strCodigoCatastro As String = System.Web.HttpUtility.UrlEncode(_EncabezadoSeleccionado.CodigoCatastro)
            Dim strDireccion As String = System.Web.HttpUtility.UrlEncode(_EncabezadoSeleccionado.Direccion)
            Dim strDescripcion As String = System.Web.HttpUtility.UrlEncode(_EncabezadoSeleccionado.Descripcion)

            dcProxy.Operaciones_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.InmobiliariosValidarSyncQuery(_EncabezadoSeleccionado.ID,
                                                                              strCodigo,
                                                                              _EncabezadoSeleccionado.Estado,
                                                                              _EncabezadoSeleccionado.Tipo,
                                                                              strArea,
                                                                              strCodigoCatastro,
                                                                              strDireccion,
                                                                              _EncabezadoSeleccionado.IDComitente,
                                                                              _EncabezadoSeleccionado.FechaCompra,
                                                                              _EncabezadoSeleccionado.ValorCompra,
                                                                              _EncabezadoSeleccionado.MonedaNegociacion,
                                                                              _EncabezadoSeleccionado.MonedaLocal,
                                                                              _EncabezadoSeleccionado.TasaCambio,
                                                                              strDescripcion,
                                                                              _EncabezadoSeleccionado.FechaCumplimiento,
                                                                              _EncabezadoSeleccionado.FechaUltimaValoracion,
                                                                              _EncabezadoSeleccionado.ValorCompraActualizado,
                                                                              _EncabezadoSeleccionado.ValorOperacion,
                                                                              Program.Usuario,
                                                                              Program.UsuarioWindows,
                                                                              Program.Maquina, Program.HashConexion)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarInmueble", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                    IsBusy = False
                Else
                    Dim objListaResultadoValidacion As List(Of CFOperaciones.Operaciones_RespuestaValidacion) = objRet.Entities.ToList

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
                                intIDInsertado = li.IDRegistroIdentity
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro ", Me.ToString(), "GuardarInmueble", Application.Current.ToString(), Program.Maquina, ex)
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

            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE, False)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub EditarRegistro()
        Try
            If logCargoForma = False Then
                ViewInmuebles.GridEdicion.Children.Add(viewFormaInmuebles)
                logCargoForma = True
            End If

            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.ID <> 0 Then
                    dtmFechaServidor = Await ObtenerFechaHoraServidor()

                    validarEstadoRegistro(OPCION_EDITAR)
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

    Private Sub EditarInmobiliarios()
        Try
            EncabezadoSeleccionadoAnterior = Nothing

            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logNuevoRegistro = False

            'Se llevan los anteriores registro ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.
            logCambiarValoresEnSelected = False
            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)
            logCambiarValoresEnSelected = True

            Editando = True
            MyBase.CambioItem("Editando")

            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)

            If Not IsNothing(_ListaDetalle) Then
                If _ListaDetalle.count() > 0 Then
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
                Else
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                    If intMonedaLocal = _EncabezadoSeleccionado.MonedaNegociacion Then
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
                    Else
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, True)
                    End If
                End If
            Else
                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                If intMonedaLocal = _EncabezadoSeleccionado.MonedaNegociacion Then
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
                Else
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, True)
                End If
            End If

            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE, True)

            'Se posiciona en el primer registro
            CambiarColorFondoTextoBuscador()

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro.", Me.ToString(), "EditarInmobiliarios", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub validarEstadoRegistro(ByVal pstrAccion As String)
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
                    EditarInmobiliarios()
                ElseIf pstrAccion = OPCION_DUPLICAR Then
                    mostrarMensajePregunta("¿Esta seguro que desea duplicar el registro?", _
                                  Program.TituloSistema, _
                                  "DUPLICARREGISTRO", _
                                  AddressOf TerminoMensajePregunta, False)
                ElseIf pstrAccion = OPCION_ANULAR Then
                    mostrarMensajePregunta("¿Esta seguro que desea anular el registro?", _
                                  Program.TituloSistema, _
                                  "ANULARREGISTRO", _
                                  AddressOf TerminoMensajePregunta, False)
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el estado del registro.", Me.ToString(), "validarEstadoRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            logCancelarRegistro = True
            logEditarRegistro = False
            logDuplicarRegistro = False
            logNuevoRegistro = False
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARDETALLE, False)

            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)

            EncabezadoSeleccionadoAnterior = Nothing

            Editando = False

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

    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.ID <> 0 Then
                    validarEstadoRegistro(OPCION_ANULAR)
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
            Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_RespuestaValidacion)

            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Operaciones_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.InmobiliariosAnularSyncQuery(_EncabezadoSeleccionado.ID, pstrObservaciones, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

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

    Public Sub Detalle_NuevoRegistro(Optional ByVal plogAbrirDetalle As Boolean = True)
        Try
            If _EncabezadoSeleccionado.ID > 0 Then
                If String.IsNullOrEmpty(TipoMovimientoNuevo) Then
                    mostrarMensaje("Por favor seleccione el Tipo movimiento para la creación del nuevo detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If MostrarSeleccionConceptoNota = Visibility.Visible Then
                    If IsNothing(IDConceptoNuevoRegistro) Or IDConceptoNuevoRegistro = 0 Then
                        mostrarMensaje("Por favor seleccione el concepto para la creación del nuevo detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                Dim objVentanaEmergenteTesoreria As New A2OYDTesoreria.TesoreriaEmergenteEncabezado()
                objVentanaEmergenteTesoreria.IDCompania = _EncabezadoSeleccionado.IDCompania
                objVentanaEmergenteTesoreria.NombreCompania = _EncabezadoSeleccionado.NombreCompania

                If TipoMovimientoNuevo = TIPODOCUMENTOCONCEPTO_EGRESOS Or TipoMovimientoNuevo = TIPODOCUMENTOCONCEPTO_CAJA Then
                    objVentanaEmergenteTesoreria.NroDocumentoBeneficiario = _EncabezadoSeleccionado.NumeroDocumentoCliente
                    objVentanaEmergenteTesoreria.TipoIdentificacionBeneficiario = _EncabezadoSeleccionado.CodTipoIdentificacionCliente
                    objVentanaEmergenteTesoreria.NombreBeneficiario = _EncabezadoSeleccionado.NombreCliente

                    objVentanaEmergenteTesoreria.ListaDetalle = New List(Of A2OYDTesoreria.TesoreriaEmergenteDetalle)

                    objVentanaEmergenteTesoreria.ListaDetalle.Add(New A2OYDTesoreria.TesoreriaEmergenteDetalle With {.IDComitente = _EncabezadoSeleccionado.IDComitente,
                                                                                                                       .NombreComitente = _EncabezadoSeleccionado.NombreCliente,
                                                                                                                       .Nit = _EncabezadoSeleccionado.NumeroDocumentoCliente})

                    If TipoMovimientoNuevo = TIPODOCUMENTOCONCEPTO_CAJA Then
                        objVentanaEmergenteTesoreria.ListaCheque = New List(Of A2OYDTesoreria.TesoreriaEmergenteCheque)

                        objVentanaEmergenteTesoreria.ListaCheque.Add(New A2OYDTesoreria.TesoreriaEmergenteCheque)
                    End If
                Else
                    objVentanaEmergenteTesoreria.ListaDetalle = New List(Of A2OYDTesoreria.TesoreriaEmergenteDetalle)

                    objVentanaEmergenteTesoreria.ListaDetalle.Add(New A2OYDTesoreria.TesoreriaEmergenteDetalle With {.IDConcepto = IDConceptoNuevoRegistro,
                                                                                                                       .Detalle = ConceptoNuevoRegistro,
                                                                                                                       .CuentaContable = ConceptoSeleccionadoNuevoRegistro.InfoAdicional03})
                    objVentanaEmergenteTesoreria.ListaDetalle.Add(New A2OYDTesoreria.TesoreriaEmergenteDetalle With {.IDConcepto = IDConceptoNuevoRegistro,
                                                                                                                       .Detalle = ConceptoNuevoRegistro,
                                                                                                                       .CuentaContable = ConceptoSeleccionadoNuevoRegistro.InfoAdicional04})
                End If

                Dim strTipoDocumento As String = String.Empty

                If TipoMovimientoNuevo = TIPODOCUMENTOCONCEPTO_NOTAS Then
                    strTipoDocumento = "N"
                ElseIf TipoMovimientoNuevo = TIPODOCUMENTOCONCEPTO_CAJA Then
                    strTipoDocumento = "RC"
                Else
                    strTipoDocumento = "CE"
                End If

                Dim objViewMovimientosEmergentesTesoreria As New A2OyDTesoreria.TesoreriaVentanaEmergenteView(strTipoDocumento, Nothing, String.Empty, objVentanaEmergenteTesoreria)
                AddHandler objViewMovimientosEmergentesTesoreria.Closed, AddressOf TerminoCrearNuevoRegistroDetalle
                Program.Modal_OwnerMainWindowsPrincipal(objViewMovimientosEmergentesTesoreria)
                objViewMovimientosEmergentesTesoreria.ShowDialog()
            Else
                mostrarMensaje("Para crear detalles es necesario realizar el guardado del encabezado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo detalle.", Me.ToString(), "Detalle_NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCrearNuevoRegistroDetalle(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultadoTesoreria As A2OYDTesoreria.TesoreriaVentanaEmergenteView = CType(sender, A2OYDTesoreria.TesoreriaVentanaEmergenteView)

            If objResultadoTesoreria.DocumentoCreado Then
                Dim NewDetalle As New CFOperaciones.Inmobiliarios_Detalles

                NewDetalle.IDInmobiliario = _EncabezadoSeleccionado.ID
                NewDetalle.Actualizacion = dtmFechaServidor
                NewDetalle.FechaRegistro = dtmFechaServidor
                NewDetalle.FechaMovimiento = dtmFechaServidor
                NewDetalle.Usuario = Program.Usuario
                NewDetalle.Maquina = Program.Maquina
                NewDetalle.UsuarioWindows = Program.UsuarioWindows

                NewDetalle.FechaMovimiento = objResultadoTesoreria.FechaDocumentoActualizado
                NewDetalle.TipoMovimiento = TipoMovimientoNuevo
                NewDetalle.IDDocumento = objResultadoTesoreria.IDDocumentoActualizado
                NewDetalle.NombreConsecutivo = objResultadoTesoreria.NombreConsecutivoActualizado
                NewDetalle.Valor = objResultadoTesoreria.ValorTotalActualizado
                NewDetalle.TipoTesoreria = objResultadoTesoreria.TipoRegistroActualizado

                NewDetalle.BaseIva = 0

                Dim objListaDetalle As New List(Of CFOperaciones.Inmobiliarios_Detalles)
                objListaDetalle = _ListaDetalle

                If IsNothing(objListaDetalle) Then
                    objListaDetalle = New List(Of CFOperaciones.Inmobiliarios_Detalles)
                    NewDetalle.Orden = 1
                Else
                    If objListaDetalle.Count > 1 Then
                        Dim intOrden As Integer = objListaDetalle.Max(Function(i) i.Orden).Value
                        intOrden += 1
                        NewDetalle.Orden = intOrden
                    Else
                        NewDetalle.Orden = 1
                    End If
                End If

                objListaDetalle.Add(NewDetalle)

                ListaDetalle = objListaDetalle
                DetalleSeleccionado = NewDetalle

                MyBase.CambioItem("ListaDetalle")

                Detalle_ActualizarRegistro()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo detalle.", Me.ToString(), "TerminoCrearNuevoRegistroDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub Detalle_EditarRegistro()
        Try
            If Not IsNothing(_DetalleSeleccionado) Then
                If Await Detalle_ValidarEstado("editar", _DetalleSeleccionado.ID) Then
                    Dim objViewMovimientosEmergentesTesoreria As New A2OYDTesoreria.TesoreriaVentanaEmergenteView(_DetalleSeleccionado.TipoTesoreria, _DetalleSeleccionado.IDDocumento, _DetalleSeleccionado.NombreConsecutivo)
                    AddHandler objViewMovimientosEmergentesTesoreria.Closed, AddressOf TerminoCrearEditarRegistroDetalle
                    Program.Modal_OwnerMainWindowsPrincipal(objViewMovimientosEmergentesTesoreria)
                    objViewMovimientosEmergentesTesoreria.ShowDialog()
                Else
                    IsBusy = False
                    IsBusyDetalles = False
                End If
            Else
                mostrarMensaje("Por favor seleccione un detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el detalle.", Me.ToString(), "Detalle_EditarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCrearEditarRegistroDetalle(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultadoTesoreria As A2OYDTesoreria.TesoreriaVentanaEmergenteView = CType(sender, A2OYDTesoreria.TesoreriaVentanaEmergenteView)

            If objResultadoTesoreria.DocumentoCreado Then
                _DetalleSeleccionado.Valor = objResultadoTesoreria.ValorTotalActualizado

                Detalle_ActualizarRegistro()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo detalle.", Me.ToString(), "TerminoCrearNuevoRegistroDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub Detalle_BorrarRegistro()
        Try
            If Not IsNothing(_ListaDetalle) Then
                If Not _DetalleSeleccionado Is Nothing Then
                    If _DetalleSeleccionado.ID > 0 Then
                        If Await Detalle_ValidarEstado("borrar", _DetalleSeleccionado.ID) Then
                            Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_RespuestaValidacion)

                            Try
                                IsBusyDetalles = True
                                ErrorForma = String.Empty

                                dcProxy.Operaciones_RespuestaValidacions.Clear()

                                objRet = Await dcProxy.Load(dcProxy.InmobiliariosDetalles_EliminarSyncQuery(_DetalleSeleccionado.ID, _EncabezadoSeleccionado.ID, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

                                If Not objRet Is Nothing Then
                                    If objRet.HasError Then
                                        If objRet.Error Is Nothing Then
                                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                        Else
                                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarInmueble", Program.TituloSistema, Program.Maquina, objRet.Error)
                                        End If

                                        objRet.MarkErrorAsHandled()
                                        IsBusy = False
                                    Else
                                        Dim objListaResultadoValidacion As List(Of CFOperaciones.Operaciones_RespuestaValidacion) = objRet.Entities.ToList

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
                                                    intIDInsertado = li.IDRegistroIdentity
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

                                                Dim objListaDetalle As New List(Of CFOperaciones.Inmobiliarios_Detalles)
                                                For Each li In _ListaDetalle
                                                    objListaDetalle.Add(li)
                                                Next

                                                If objListaDetalle.Contains(_DetalleSeleccionado) Then
                                                    objListaDetalle.Remove(_DetalleSeleccionado)
                                                End If

                                                DetalleSeleccionado = Nothing
                                                ListaDetalle = objListaDetalle
                                                If ListaDetalle.Count > 0 Then
                                                    DetalleSeleccionado = ListaDetalle.First
                                                End If

                                                mostrarMensaje(strMensajeExitoso, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

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
                            Catch ex As Exception
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la distribución.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
                            End Try

                            IsBusyDetalles = False
                        Else
                            IsBusyDetalles = False
                            IsBusy = False
                        End If
                    Else
                        Dim objListaDetalle As New List(Of CFOperaciones.Inmobiliarios_Detalles)
                        For Each li In _ListaDetalle
                            objListaDetalle.Add(li)
                        Next

                        If objListaDetalle.Contains(_DetalleSeleccionado) Then
                            objListaDetalle.Remove(_DetalleSeleccionado)
                        End If

                        DetalleSeleccionado = Nothing
                        ListaDetalle = objListaDetalle
                        If ListaDetalle.Count > 0 Then
                            DetalleSeleccionado = ListaDetalle.First
                        End If
                    End If
                Else
                    mostrarMensaje("Por favor seleccione un detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.", Me.ToString(), "Detalle_BorrarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub Detalle_ActualizarRegistro()
        Try
            IsBusyDetalles = True
            If IsNothing(_DetalleSeleccionado.FechaMovimiento) Then
                mostrarMensaje("La Fecha de movimiento es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusyDetalles = False
                Exit Sub
            Else
                Dim dtmFechaHabilMovimiento As Nullable(Of DateTime) = Await ObtenerFechaCierrePortafolio(_EncabezadoSeleccionado.IDComitente)

                If Not IsNothing(dtmFechaHabilMovimiento) Then
                    If _DetalleSeleccionado.FechaMovimiento.Value < dtmFechaHabilMovimiento Then
                        mostrarMensaje("La Fecha de movimiento es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusyDetalles = False
                        Exit Sub
                    End If
                End If
            End If

            If IsNothing(_DetalleSeleccionado.Valor) Or _DetalleSeleccionado.Valor = 0 Then
                mostrarMensaje("El Valor debe ser mayor a cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusyDetalles = False
                Exit Sub
            End If

            If String.IsNullOrEmpty(_DetalleSeleccionado.TipoMovimiento) Then
                mostrarMensaje("El tipo movimiento debe ser mayor a cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusyDetalles = False
                Exit Sub
            End If

            Await GuardarDetalle()

            IsBusyDetalles = False

        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el detalle.", Me.ToString(), "Detalle_ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function GuardarDetalle() As Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_RespuestaValidacion)

            Try
                IsBusyDetalles = True
                ErrorForma = String.Empty

                dcProxy.Operaciones_RespuestaValidacions.Clear()

                objRet = Await dcProxy.Load(dcProxy.InmobiliariosDetalles_ActualizarSyncQuery(_DetalleSeleccionado.ID,
                                                                                              _EncabezadoSeleccionado.ID,
                                                                                              _DetalleSeleccionado.FechaMovimiento,
                                                                                              _DetalleSeleccionado.Valor,
                                                                                              _DetalleSeleccionado.BaseIva,
                                                                                              _DetalleSeleccionado.TipoMovimiento,
                                                                                              _DetalleSeleccionado.IDDocumento,
                                                                                              _DetalleSeleccionado.TipoTesoreria,
                                                                                              _DetalleSeleccionado.NombreConsecutivo,
                                                                                              Program.Usuario,
                                                                                              Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarInmueble", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                        IsBusy = False
                    Else
                        Dim objListaResultadoValidacion As List(Of CFOperaciones.Operaciones_RespuestaValidacion) = objRet.Entities.ToList

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
                                    intIDInsertado = li.IDRegistroIdentity
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

                                mostrarMensaje(strMensajeExitoso, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

                                Await ConsultarDetalle(_EncabezadoSeleccionado.ID,
                                                       FechaInicialFiltro,
                                                       FechaFinalFiltro,
                                                       ConceptoFiltro)

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
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al guardar el detalle.", Me.ToString(), "GuardarDetalle", Application.Current.ToString(), Program.Maquina, ex)
            End Try

            logResultado = True
            IsBusyDetalles = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el detalle.", Me.ToString(), "GuardarDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
        Return logResultado
    End Function

    Public Async Function Detalle_ValidarEstado(ByVal pstrAccion As String, ByVal pintIDDetalle As Integer) As Task(Of Boolean)
        Try
            Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_RespuestaValidacion)
            Dim logRetorno As Boolean = True

            Try
                IsBusyDetalles = True
                ErrorForma = String.Empty

                dcProxy.Operaciones_RespuestaValidacions.Clear()

                objRet = Await dcProxy.Load(dcProxy.InmobiliariosDetalles_ValidarEstadoSyncQuery(_DetalleSeleccionado.ID, pintIDDetalle, pstrAccion, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarInmueble", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                        IsBusy = False
                    Else
                        Dim objListaResultadoValidacion As List(Of CFOperaciones.Operaciones_RespuestaValidacion) = objRet.Entities.ToList

                        If objListaResultadoValidacion.Count > 0 Then
                            Dim logDetieneIngreso As Boolean = False
                            Dim strMensajeError As String = String.Empty

                            For Each li In objListaResultadoValidacion.Where(Function(i) i.DetieneIngreso)
                                logDetieneIngreso = True
                                strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                            Next

                            If logDetieneIngreso Then
                                logRetorno = False
                                mostrarMensaje(strMensajeError, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la distribución.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
            End Try

            IsBusyDetalles = False

            Return logRetorno
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.", Me.ToString(), "Detalle_BorrarRegistro", Program.TituloSistema, Program.Maquina, ex)
            Return False
        End Try
    End Function

    Public Overrides Sub CambiarAForma()
        Try
            If logCargoForma = False Then
                ViewInmuebles.GridEdicion.Children.Add(viewFormaInmuebles)
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
            Dim objBusqueda As New CamposBusquedaInmuebles
            objBusqueda.ID = 0
            objBusqueda.Codigo = String.Empty
            objBusqueda.Tipo = String.Empty
            objBusqueda.FechaCompra = Nothing
            objBusqueda.IDComitente = String.Empty

            BusquedaInmueble = objBusqueda

            Editando = True
            MyBase.CambioItem("Editando")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al organizar la nueva busqueda.", Me.ToString(), "OrganizarNuevaBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function CargarCombos(ByVal plogCompletos As Boolean, Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_Combos)

        Try
            If logNuevoRegistro Or logEditarRegistro Then
                IsBusy = True
            End If

            If Not IsNothing(pstrUserState) Then
                pstrUserState = pstrUserState.ToUpper
            End If

            ErrorForma = String.Empty

            dcProxy.Operaciones_Combos.Clear()

            objRet = Await dcProxy.Load(dcProxy.InmobiliariosCombosSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

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
                        Dim objListaNodosCategoria As List(Of CFOperaciones.Operaciones_Combos) = Nothing
                        Dim objDiccionario As New Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos))

                        Dim listaCategorias = From lc In objRet.Entities Select lc.Topico Distinct

                        For Each li In listaCategorias
                            strNombreCategoria = li
                            objListaNodosCategoria = (From ln In objRet.Entities Where ln.Topico = strNombreCategoria).ToList
                            objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                        Next

                        DiccionarioCombosCompleta = Nothing
                        DiccionarioCombos = Nothing

                        DiccionarioCombosCompleta = objDiccionario
                        DiccionarioCombos = objDiccionario

                        If objDiccionario.ContainsKey("NEGOCIOS_MONEDALOCAL") Then
                            If objDiccionario("NEGOCIOS_MONEDALOCAL").Count > 0 Then
                                intMonedaLocal = objDiccionario("NEGOCIOS_MONEDALOCAL").First.ID
                            End If
                        End If

                        If objDiccionario.ContainsKey("MAXIMOENTREFECHASDETALLES") Then
                            If objDiccionario("MAXIMOENTREFECHASDETALLES").Count > 0 Then
                                Try
                                    intCantidadMaximaDiasEntreFechas = CInt(objDiccionario("MAXIMOENTREFECHASDETALLES").First.Codigo)
                                Catch ex As Exception
                                    intCantidadMaximaDiasEntreFechas = 60
                                End Try
                            End If
                        End If

                        TipoMovimientoNuevo = "EGRESOS"

                        ObtenerDecimales()
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

    Public Sub SeleccionarCliente(ByVal pobjCliente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjCliente) Then
                If logEditarRegistro Or logNuevoRegistro Then

                    If Not IsNothing(_EncabezadoSeleccionado) Then

                        _EncabezadoSeleccionado.IDComitente = pobjCliente.IdComitente
                        _EncabezadoSeleccionado.NombreCliente = pobjCliente.Nombre
                        _EncabezadoSeleccionado.NumeroDocumentoCliente = pobjCliente.NroDocumento
                        _EncabezadoSeleccionado.CodTipoIdentificacionCliente = pobjCliente.CodTipoIdentificacion
                        _EncabezadoSeleccionado.TipoIdentificacionCliente = pobjCliente.TipoIdentificacion
                        _EncabezadoSeleccionado.IDCompania = pobjCliente.IDCompania
                        _EncabezadoSeleccionado.NombreCompania = pobjCliente.NombreCompania

                    End If
                End If
            Else
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    _EncabezadoSeleccionado.IDComitente = String.Empty
                    _EncabezadoSeleccionado.NombreCliente = String.Empty
                    _EncabezadoSeleccionado.CodTipoIdentificacionCliente = String.Empty
                    _EncabezadoSeleccionado.TipoIdentificacionCliente = String.Empty
                    _EncabezadoSeleccionado.NumeroDocumentoCliente = 0
                    _EncabezadoSeleccionado.IDCompania = Nothing
                    _EncabezadoSeleccionado.NombreCompania = String.Empty

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Function ValidarGuardadoRegistro(ByVal pobjRegistro As CFOperaciones.Inmobiliarios) As Boolean
        Try
            'Valida los campos que son requeridos por el sistema de OYDPLUS.
            Dim logPasaValidacion As Boolean = True
            Dim strMensajeValidacion = String.Empty

            If String.IsNullOrEmpty(pobjRegistro.Tipo) Then
                strMensajeValidacion = String.Format("{0}{1} -   Tipo inmueble", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.IDComitente) Then
                strMensajeValidacion = String.Format("{0}{1} -   Portafolio", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.FechaCompra) Then
                strMensajeValidacion = String.Format("{0}{1} -   Fecha compra", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.FechaCumplimiento) Then
                strMensajeValidacion = String.Format("{0}{1} -   Fecha cumplimiento", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.ValorCompra) Or pobjRegistro.ValorCompra = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Valor inversión", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.MonedaNegociacion) Or pobjRegistro.MonedaNegociacion = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Moneda negociación", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.TasaCambio) Or pobjRegistro.TasaCambio = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Tasa cambio", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.ValorOperacion) Or pobjRegistro.ValorOperacion = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Valor operación", strMensajeValidacion, vbCrLf)
            End If

            If Not IsNothing(DiccionarioCombos) Then
                If DiccionarioCombos.ContainsKey("CAMPOSREQUERIDOSTIPOS") Then
                    For Each li In DiccionarioCombos("CAMPOSREQUERIDOSTIPOS")
                        If li.Codigo = _EncabezadoSeleccionado.Tipo Then
                            If li.Descripcion = OPCIONES_VALIDARCAMPOS.AREA.ToString Then
                                If String.IsNullOrEmpty(pobjRegistro.Area) Then
                                    strMensajeValidacion = String.Format("{0}{1} -   Área", strMensajeValidacion, vbCrLf)
                                End If
                            ElseIf li.Descripcion = OPCIONES_VALIDARCAMPOS.CODIGOCATASTRO.ToString Then
                                If String.IsNullOrEmpty(pobjRegistro.Area) Then
                                    strMensajeValidacion = String.Format("{0}{1} -   Código catastro", strMensajeValidacion, vbCrLf)
                                End If
                            ElseIf li.Descripcion = OPCIONES_VALIDARCAMPOS.DIRECCION.ToString Then
                                If String.IsNullOrEmpty(pobjRegistro.Area) Then
                                    strMensajeValidacion = String.Format("{0}{1} -   Dirección", strMensajeValidacion, vbCrLf)
                                End If
                            ElseIf li.Descripcion = OPCIONES_VALIDARCAMPOS.DESCRIPCION.ToString Then
                                If String.IsNullOrEmpty(pobjRegistro.Descripcion) Then
                                    strMensajeValidacion = String.Format("{0}{1} -   Descripción", strMensajeValidacion, vbCrLf)
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            If String.IsNullOrEmpty(strMensajeValidacion) Then
                logPasaValidacion = True

                If _EncabezadoSeleccionado.FechaCumplimiento.Value < _EncabezadoSeleccionado.FechaCompra.Value Then
                    logPasaValidacion = False
                    strMensajeValidacion = String.Format("Señor usuario, la fecha de cumplimiento no puede ser menor o igual a la fecha de compra.")
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

    Public Sub BuscarControlValidacion(ByVal pViewOperaciones As FormaOperacionesOtrosNegociosView, ByVal pstrOpcion As String)
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

    Public Function CalcularValorRegistro(Optional ByVal plogMostrarMensajeUsuario As Boolean = True) As Boolean
        Dim logResultado As Boolean = True

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Try
                    IsBusyDetalles = True
                    ErrorForma = String.Empty

                    _EncabezadoSeleccionado.ValorOperacion = _EncabezadoSeleccionado.ValorCompra / IIf(_EncabezadoSeleccionado.TasaCambio > 0, _EncabezadoSeleccionado.TasaCambio, 1)

                Catch ex As Exception
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "CalcularValorRegistro", Application.Current.ToString(), Program.Maquina, ex)
                    logResultado = False
                Finally
                End Try
            Else
                logResultado = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor del registro.", Me.ToString(), "CalcularValorRegistro", Program.TituloSistema, Program.Maquina, ex)
            logResultado = False
        End Try

        IsBusyDetalles = False
        Return (logResultado)
    End Function

    Public Sub ObtenerValoresRegistroAnterior(ByVal pobjRegistro As CFOperaciones.Inmobiliarios, ByRef pobjRegistroSalvarDatos As CFOperaciones.Inmobiliarios)
        Try
            If Not IsNothing(pobjRegistro) Then
                Dim objNewRegistro As New CFOperaciones.Inmobiliarios

                objNewRegistro.ID = pobjRegistro.ID
                objNewRegistro.Codigo = pobjRegistro.Codigo
                objNewRegistro.Estado = pobjRegistro.Estado
                objNewRegistro.NombreEstado = pobjRegistro.NombreEstado
                objNewRegistro.Tipo = pobjRegistro.Tipo
                objNewRegistro.TipoInmueble = pobjRegistro.TipoInmueble
                objNewRegistro.Area = pobjRegistro.Area
                objNewRegistro.CodigoCatastro = pobjRegistro.CodigoCatastro
                objNewRegistro.Direccion = pobjRegistro.Direccion
                objNewRegistro.IDComitente = pobjRegistro.IDComitente
                objNewRegistro.NombreCliente = pobjRegistro.NombreCliente
                objNewRegistro.CodTipoIdentificacionCliente = pobjRegistro.CodTipoIdentificacionCliente
                objNewRegistro.TipoIdentificacionCliente = pobjRegistro.TipoIdentificacionCliente
                objNewRegistro.NumeroDocumentoCliente = pobjRegistro.NumeroDocumentoCliente
                objNewRegistro.FechaCompra = pobjRegistro.FechaCompra
                objNewRegistro.ValorCompra = pobjRegistro.ValorCompra
                objNewRegistro.MonedaNegociacion = pobjRegistro.MonedaNegociacion
                objNewRegistro.CodigoMonedaNegociacion = pobjRegistro.CodigoMonedaNegociacion
                objNewRegistro.NombreMonedaNegociacion = pobjRegistro.NombreMonedaNegociacion
                objNewRegistro.MonedaLocal = pobjRegistro.MonedaLocal
                objNewRegistro.CodigoMonedaLocal = pobjRegistro.CodigoMonedaLocal
                objNewRegistro.NombreMonedaLocal = pobjRegistro.NombreMonedaLocal
                objNewRegistro.TasaCambio = pobjRegistro.TasaCambio
                objNewRegistro.Descripcion = pobjRegistro.Descripcion
                objNewRegistro.FechaCumplimiento = pobjRegistro.FechaCumplimiento
                objNewRegistro.FechaUltimaValoracion = pobjRegistro.FechaUltimaValoracion
                objNewRegistro.ValorCompraActualizado = pobjRegistro.ValorCompraActualizado
                objNewRegistro.ValorOperacion = pobjRegistro.ValorOperacion
                objNewRegistro.FechaRegistro = pobjRegistro.FechaRegistro
                objNewRegistro.Actualizacion = pobjRegistro.Actualizacion
                objNewRegistro.Usuario = pobjRegistro.Usuario
                objNewRegistro.UsuarioWindows = pobjRegistro.UsuarioWindows
                objNewRegistro.Maquina = pobjRegistro.Maquina
                objNewRegistro.IDCompania = pobjRegistro.IDCompania
                objNewRegistro.NombreCompania = pobjRegistro.NombreCompania

                pobjRegistroSalvarDatos = objNewRegistro
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", Me.ToString(), "ObtenerValoresRegistroAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Public Async Function ValidarFechaCierrePortafolio(ByVal pdtmFechaValidacion As DateTime, ByVal pstrDescripcionMensaje As String) As Task(Of Boolean)
        Dim logResultadoValidacion As Boolean = False
        Dim dtmFechaOperacion As Date

        Try
            If Not IsNothing(EncabezadoSeleccionado) Then
                If mdtmFechaCierrePortafolio.Equals(MDTM_FECHA_CIERRE_SIN_ACTUALIZAR) Then
                    mdtmFechaCierrePortafolio = Await ObtenerFechaCierrePortafolio(EncabezadoSeleccionado.IDComitente)
                End If

                If Not IsNothing(mdtmFechaCierrePortafolio) And Not IsNothing(pdtmFechaValidacion) Then
                    dtmFechaOperacion = New Date(pdtmFechaValidacion.Year, pdtmFechaValidacion.Month, pdtmFechaValidacion.Day) ' Eliminar la hora
                    If mdtmFechaCierrePortafolio >= dtmFechaOperacion Then
                        A2Utilidades.Mensajes.mostrarMensaje("El portafolio del cliente " & EncabezadoSeleccionado.IDComitente.Trim & "-" & EncabezadoSeleccionado.NombreCliente & " está cerrado para la " & pstrDescripcionMensaje & " (Fecha de cierre " & Year(mdtmFechaCierrePortafolio) & "/" & Month(mdtmFechaCierrePortafolio) & "/" & Day(mdtmFechaCierrePortafolio) & ", " & pstrDescripcionMensaje & " " & Year(pdtmFechaValidacion) & "/" & Month(pdtmFechaValidacion) & "/" & Day(pdtmFechaValidacion) & "). ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        logResultadoValidacion = True
                    End If
                Else
                    logResultadoValidacion = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la fecha de cierre del portafolio del cliente. ", Me.ToString(), "ObtenerFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return logResultadoValidacion
    End Function

    Public Async Function ObtenerTRMMoneda() As Task
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If Not IsNothing(_EncabezadoSeleccionado.FechaCompra) And Not IsNothing(_EncabezadoSeleccionado.MonedaNegociacion) Then
                    Dim objRet As InvokeOperation(Of Double)

                    objRet = Await dcProxy.Operaciones_OtrosNegociosConsultarTRMMonedaSync(_EncabezadoSeleccionado.FechaCompra, _EncabezadoSeleccionado.MonedaNegociacion, Program.Usuario, Program.HashConexion).AsTask

                    If Not objRet Is Nothing Then
                        If objRet.HasError Then
                            If objRet.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ObtenerTRMMoneda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            End If

                            objRet.MarkErrorAsHandled()
                        Else
                            If Not IsNothing(objRet.Value) Then
                                _EncabezadoSeleccionado.TasaCambio = objRet.Value
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ObtenerTRMMoneda. ", Me.ToString(), "ObtenerTRMMoneda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Sub ObtenerMonedaLocal()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.MonedaLocal <> intMonedaLocal Then
                    If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_MONEDALOCAL") Then
                        _EncabezadoSeleccionado.MonedaLocal = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.ID

                        _EncabezadoSeleccionado.NombreMonedaLocal = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.Descripcion
                        _EncabezadoSeleccionado.CodigoMonedaLocal = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.Codigo
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el valor por defecto de la moneda local.", Me.ToString(), "ObtenerMonedaLocal", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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

    Public Sub ObtenerDecimales()
        Try
            Dim intCantidadDecimales As Integer = 3
            Dim intNroMaximoBaseIVA As Integer = 100

            If Not IsNothing(DiccionarioCombosCompleta) Then
                If DiccionarioCombosCompleta.ContainsKey("INMOBILIARIOS_CANTIDADDECIMALES") Then
                    If DiccionarioCombosCompleta("INMOBILIARIOS_CANTIDADDECIMALES").Count > 0 Then
                        intCantidadDecimales = DiccionarioCombosCompleta("INMOBILIARIOS_CANTIDADDECIMALES").First.Codigo
                    End If
                End If

                If DiccionarioCombosCompleta.ContainsKey("INMOBILIARIOS_MAXIMOBASEIVA") Then
                    If DiccionarioCombosCompleta("INMOBILIARIOS_MAXIMOBASEIVA").Count > 0 Then
                        intNroMaximoBaseIVA = DiccionarioCombosCompleta("INMOBILIARIOS_MAXIMOBASEIVA").First.Codigo
                    End If
                End If
            End If

            CantidadDecimales = String.Format("{0}", intCantidadDecimales)
            MaximoBaseIVA = intNroMaximoBaseIVA.ToString

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la valición del registro.", Me.ToString, "ObtenerDecimales", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
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
            If dcProxy1.Inmobiliarios.Where(Function(i) i.ID = _EncabezadoSeleccionado.ID).Count = 0 Then
                dcProxy1.Inmobiliarios.Add(_EncabezadoSeleccionado)
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

#Region "Eventos"

    Private Async Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try
            If logCalcularValores And Editando Then
                Select Case e.PropertyName.ToLower()
                    Case "monedanegociacion"
                        If Not IsNothing(_EncabezadoSeleccionado.MonedaNegociacion) Then
                            If intMonedaLocal = _EncabezadoSeleccionado.MonedaNegociacion Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, False)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTASACAMBIO, True)
                            End If

                            Await ObtenerTRMMoneda()
                        End If
                    Case "valorcompra"
                        CalcularValorRegistro()
                    Case "tasacambio"
                        CalcularValorRegistro()
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_EncabezadoSeleccionado_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub _DetalleSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleSeleccionado.PropertyChanged
        Try
            If logCalcularValores And Editando Then
                Select Case e.PropertyName.ToLower()
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_DetalleSeleccionado_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Function ValidarMaximosDiasEntreFechasFiltro() As Boolean
        Try
            Dim logRetorno As Boolean = True

            If Not IsNothing(FechaInicialFiltro) And Not IsNothing(FechaFinalFiltro) Then
                Dim intCantidadDiasFechas As Integer = DateDiff(DateInterval.Day, FechaInicialFiltro.Value, FechaFinalFiltro.Value)
                If intCantidadDiasFechas > intCantidadMaximaDiasEntreFechas Then
                    mostrarMensaje("La diferencia entre fechas no puede ser mayor a " & intCantidadMaximaDiasEntreFechas.ToString & ".", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logRetorno = False
                End If
            End If
            Return logRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la validación de la fecha.", Me.ToString, "ValidarMaximosDiasEntreFechasFiltro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Return False
        End Try
    End Function

#End Region

#Region "Timer Refrescar pantalla"

    Private _myDispatcherTimerOrdenes As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    Public Sub ReiniciaTimer()
        Try
            If Program.Recarga_Automatica_Activa Then
                If _myDispatcherTimerOrdenes Is Nothing Then
                    _myDispatcherTimerOrdenes = New System.Windows.Threading.DispatcherTimer
                    _myDispatcherTimerOrdenes.Interval = New TimeSpan(0, 0, Program.Par_lapso_recarga)
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
Public Class CamposBusquedaInmuebles
    Implements INotifyPropertyChanged

    Private _ID As Integer
    <Display(Name:="ID registro")> _
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _Codigo As String
    <Display(Name:="Código")> _
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Private _Tipo As String
    <Display(Name:="Tipo inmueble")> _
    Public Property Tipo() As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo"))
        End Set
    End Property

    Private _Estado As String
    <Display(Name:="Estado")> _
    Public Property Estado() As String
        Get
            Return _Estado
        End Get
        Set(ByVal value As String)
            _Estado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Estado"))
        End Set
    End Property

    Private _FechaCompra As Nullable(Of DateTime)
    <Display(Name:="Fecha compra")> _
    Public Property FechaCompra() As Nullable(Of DateTime)
        Get
            Return _FechaCompra
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaCompra = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaCompra"))
        End Set
    End Property

    Private _IDComitente As String
    <Display(Name:="Portafolio")> _
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