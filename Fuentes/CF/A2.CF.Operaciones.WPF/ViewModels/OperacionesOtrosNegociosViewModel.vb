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
Imports A2MCCoreWPF
Imports A2.OYD.OYDServer.RIA.Web.CFOperaciones
Imports System.Threading.Tasks

Public Class OperacionesOtrosNegociosViewModel
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
                viewFormaOperacionesOtrosNegocios = New FormaOperacionesOtrosNegociosView(Me)

                Dim objDiccionarioHabilitarCampos = New Dictionary(Of String, Boolean)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.TIPOCUMPLIMIENTO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.MERCADO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.TIPOREPO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION.ToString, True)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.ESPECIES.ToString, True)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES.ToString, True)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARMONEDA.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARPAIS.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARTIPO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARCONTRAPARTE.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIO.ToString, False)
                objDiccionarioHabilitarCampos.Add(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIOCOP.ToString, False)

                MostrarContraparte = Visibility.Visible
                MostrarContraparteOTC = Visibility.Collapsed
                MostrarTipoBanRep = Visibility.Collapsed

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
                If logEsModal Then
                    ListaEncabezado = New List(Of CFOperaciones.Operaciones_OtrosNegocios)
                    NuevoRegistro()
                Else
                    Await RecargarPantalla()
                End If
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

#Region "Eventos publicos"

    Public Event TerminoConfigurarNuevoRegistro()
    Public Event TerminoGuardarRegistro(ByVal plogGuardoRegistro As Boolean, ByVal plngIDOrden As Integer, ByVal pstrTipoOrden As String, ByVal pstrTipoOperacion As String, ByVal pstrTipoOrigen As String)

#End Region

#Region "Propiedades"

#Region "Variables"

    Private dcProxy As OperacionesCFDomainContext
    Private dcProxy1 As OperacionesCFDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext

    'CCM20151107
    Private mdtmFechaCierrePortafolio As DateTime? = Nothing

    Public logNuevoRegistro As Boolean = False
    Public logEditarRegistro As Boolean = False
    Public logCancelarRegistro As Boolean = False
    Public logDuplicarRegistro As Boolean = False
    Public logModificarDatosTipoNegocio As Boolean = True
    Public logCalcularValores As Boolean = True
    Public strTipoCalculo As String = String.Empty

    Dim viewFormaOperacionesOtrosNegocios As FormaOperacionesOtrosNegociosView = Nothing

    Dim dtmFechaServidor As DateTime
    Dim logCambiarSelected As Boolean = True
    Dim logCambiarValoresEnSelected As Boolean = True
    Dim logCargoForma As Boolean = False
    Dim objEntidadPorDefecto As Operaciones_EntidadDefecto = Nothing

    Dim intMonedaLocal As Integer = 0
    Dim intPaisLocal As Integer = 0
    Public dblPrecioConsultado As Double = 0
    Dim logPosicionarCantidad As Boolean = True

    'VARIABLES CREADAS PARA MANEJAR EL MODAL
    Public logEsModal As Boolean = False

    Dim cwOperacionesOtrosNegociosView As cwOperacionesOtrosNegociosView
    Public dblCantidadAplazada As Double = 0
    Public dtmFechaCumplimientoAplazada As DateTime? = Nothing
#End Region

#Region "Constantes"
    Private TOOLBARACTIVOPANTALLA As String = "A2Consola_ToolbarActivo"
    Private MDTM_FECHA_CIERRE_SIN_ACTUALIZAR As Date = New Date(1999, 1, 1) 'CCM20151107: Fecha para inicializar fecha de cierre
    Private MINT_LONG_MAX_CODIGO_OYD As Integer = 17

    Public TIPOREGISTRO_MANUAL As String = "M"
    Public TIPOREGISTRO_AUTOMATICO As String = "A"

    Public MERCADO_SECUNDARIO As String = "S"
    Public MERCADO_PRIMARIO As String = "P"
    Public MERCADO_REPO As String = "E"
    Public MERCADO_RENOVACION As String = "R"

    Public TIPOPERMISO_NEGOCIO As String = "TIPONEGOCIO"
    Public TIPOPERMISO_ORIGEN As String = "TIPOORIGEN"

    Public TIPONEGOCIO_RENTAFIJA As String = "C"
    Public TIPONEGOCIO_RENTAVARIABLE As String = "A"
    Public TIPONEGOCIO_REPO As String = "R"
    Public TIPONEGOCIO_DEPOSITOREMUNERADO As String = "D"
    Public TIPONEGOCIO_SIMULTANEA As String = "1"
    Public TIPONEGOCIO_TTV As String = "3"
    Public TIPONEGOCIO_DIVISAS As String = "I"

    Public TIPOORIGEN_INTERNACIONAL As String = "EX"
    Public TIPOORIGEN_MERCADOGLOBAL As String = "MG"
    Public TIPOORIGEN_EUROBONOS As String = "EU"
    Public TIPOORIGEN_OTRASFIRMAS As String = "OF"
    Public TIPOORIGEN_OTC As String = "OTC"
    Public TIPOORIGEN_DIVISAS As String = "DI"

    Public CLASE_ACCIONES As String = "A"
    Public CLASE_RENTAFIJA As String = "C"

    Private TIPOOPERACION_COMPRA As String = "C"
    Private TIPOOPERACION_VENTA As String = "V"
    Private TIPOOPERACION_RECOMPRA As String = "R"
    Private TIPOOPERACION_REVENTA As String = "S"

    Private ESTADO_INGRESADA As String = "I"
    Private ESTADO_ANULADA As String = "A"
    Private ESTADO_PORAPROBAR As String = "PA"

    Private STR_URLMOTORCALCULOS As String = ""
    Private LOG_HACERLOGMOTORCALCULOS As Boolean = False
    Private STR_RUTALOGMOTORCALCULOS As String = ""

    Private Const OPCION_INICIO As String = "INICIO"
    Private Const OPCION_NUEVO As String = "NUEVO"
    Private Const OPCION_EDITAR As String = "EDITAR"
    Private Const OPCION_ANULAR As String = "ANULAR"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    Private Const OPCION_APLAZAR As String = "APLAZAR"
    Private Const OPCION_LIQXDIFE As String = "OPCION_LIQXDIFE"
    Private Const OPCION_TIPONEGOCIO As String = "TIPONEGOCIO"
    Private Const OPCION_TIPOORIGEN As String = "TIPOORIGEN"
    Private Const OPCION_ACTUALIZAR As String = "ACTUALIZAR"
    Private Const OPCION_CLASIFICACIONINVERSION As String = "CLASIFICACIONINVERSION"
    Private Const OPCION_CONTRAPARTE As String = "CONTRAPARTE"
    Private Const OPCION_TIPOREPO As String = "TIPOREPO"
    Private Const OPCION_TIPOBANREP As String = "TIPOBANREPUBLICA"
    Private Const OPCION_SOLOCONSULTA As String = "SOLOCONSULTA"

    Public Const LIMPIARDATOS_TIPONEGOCIO As String = "TIPONEGOCIO"
    Public Const LIMPIARDATOS_NEMOTECNICO As String = "NEMOTECNICO"
    Public Const LIMPIARDATOS_TIPO As String = "TIPO"
    Public Const LIMPIARDATOS_NOMINAL As String = "NOMINAL"

    Private TIPOTASA_VARIABLE As String = "V"
    Private TIPOTASA_FIJA As String = "F"

    Public TIPOREPO_ABIERTO As String = "A"
    Public TIPOREPO_CERRADO As String = "C"

    Private Const HOJA_MOTORCALCULOS As String = "CF.CalculosOperacionesOtrosNegocios"

    Private Const COLOR_DESHABILITADO As String = "#E2E2E2"
    Private Const COLOR_HABILITADO As String = "#FFFFFFFF"

    Public Enum OPCIONES_HABILITARCAMPOS
        HABILITARENCABEZADO
        HABILITARNEGOCIO
        MOSTRARCAMPOSRENTAVARIABLE
        MOSTRARCAMPOSRENTAFIJA
        MOSTRARCAMPOSREPOACCIONES
        MOSTRARCAMPOSREPOCERRADO
        MOSTRARCAMPOSREPOABIERTO
        MOSTRARCAMPOSDEPOSITOREMUNERADO
        MOSTRARCAMPOSSIMULTANEA
        MOSTRARCAMPOSTTV
        MOSTRARCAMPOSTTVACCIONES
        MOSTRARCAMPOSDIVISAS
        CLASIFICACIONINVERSION
        FECHAVENCIMIENTOOPERACION
        TIPOCUMPLIMIENTO
        MERCADO
        TIPOREPO
        ESPECIESHABILITARSELECCION
        ESPECIES
        CARACTERISTICASFACIALESESPECIES
        ESPECIESTASAFIJA
        ESPECIESTASAVARIABLE
        HABILITARMONEDALOCAL
        HABILITARMONEDA
        HABILITARPAIS
        HABILITARTIRREPO
        HABILITARCAMPOSCOMISION
        HABILITARCAMPOSRETENCION
        HABILITARTIPO
        HABILITARCONTRAPARTE
        HABILITARVALORGIROINICIO
        HABILITARVALORGIROINICIOCOP
    End Enum

    Public Enum TIPOCALCULOS_MOTOR
        PRECIO
        TASANEGOCIACION
        VALORGIRO
        PORCENTAJECOMISION
        VALORCOMISION
        VENCIMIENTOOPERACION
    End Enum

    Public Enum TIPOSORIGEN_TIPOMANEJO
        NEGOCIOS_TIPOORIGEN_MANEJACOMISION
        NEGOCIOS_TIPOORIGEN_MANEJAIVA
        NEGOCIOS_TIPOORIGEN_MANEJARETENCION
        NEGOCIOS_TIPOORIGEN_MANEJACONTRAPARTE
    End Enum

#End Region

#Region "Propiedades para el Tipo de registro"

    Private _ListaEncabezado As List(Of CFOperaciones.Operaciones_OtrosNegocios)
    Public Property ListaEncabezado() As List(Of CFOperaciones.Operaciones_OtrosNegocios)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CFOperaciones.Operaciones_OtrosNegocios))
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

    Private _EncabezadoDataForm As CFOperaciones.Operaciones_OtrosNegocios = New CFOperaciones.Operaciones_OtrosNegocios
    Public Property EncabezadoDataForm() As CFOperaciones.Operaciones_OtrosNegocios
        Get
            Return _EncabezadoDataForm
        End Get
        Set(ByVal value As CFOperaciones.Operaciones_OtrosNegocios)
            _EncabezadoDataForm = value
            MyBase.CambioItem("EncabezadoDataForm")
        End Set
    End Property

    Private WithEvents _EncabezadoSeleccionado As CFOperaciones.Operaciones_OtrosNegocios
    Public Property EncabezadoSeleccionado() As CFOperaciones.Operaciones_OtrosNegocios
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CFOperaciones.Operaciones_OtrosNegocios)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If logCambiarValoresEnSelected Then
                    mdtmFechaCierrePortafolio = MDTM_FECHA_CIERRE_SIN_ACTUALIZAR 'CCM20151107

                    If _EncabezadoSeleccionado.ID > 0 Then
                        ConsultarDetalle(_EncabezadoSeleccionado.ID)
                        ObtenerClaseEspecies()
                    End If

                    If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or _
                        _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or _
                        _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                        For Each liOperacion In DiccionarioCombos("NEGOCIOS_TIPOOPERACION")
                            For Each liOperacionEspecifico In DiccionarioCombos("NEGOCIOS_TIPOOPERACION_GENERAL")
                                If liOperacion.Codigo = liOperacionEspecifico.Codigo Then
                                    liOperacion.Descripcion = liOperacionEspecifico.Descripcion
                                End If
                            Next
                        Next
                    ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                        For Each liOperacion In DiccionarioCombos("NEGOCIOS_TIPOOPERACION")
                            For Each liOperacionEspecifico In DiccionarioCombos("NEGOCIOS_TIPOOPERACION_REPO")
                                If liOperacion.Codigo = liOperacionEspecifico.Codigo Then
                                    liOperacion.Descripcion = liOperacionEspecifico.Descripcion
                                End If
                            Next
                        Next
                    ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                        For Each liOperacion In DiccionarioCombos("NEGOCIOS_TIPOOPERACION")
                            For Each liOperacionEspecifico In DiccionarioCombos("NEGOCIOS_TIPOOPERACION_TTV")
                                If liOperacion.Codigo = liOperacionEspecifico.Codigo Then
                                    liOperacion.Descripcion = liOperacionEspecifico.Descripcion
                                End If
                            Next
                        Next
                    ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                        For Each liOperacion In DiccionarioCombos("NEGOCIOS_TIPOOPERACION")
                            For Each liOperacionEspecifico In DiccionarioCombos("NEGOCIOS_TIPOOPERACION_DEPOSITOREMUNERADO")
                                If liOperacion.Codigo = liOperacionEspecifico.Codigo Then
                                    liOperacion.Descripcion = liOperacionEspecifico.Descripcion
                                End If
                            Next
                        Next
                    End If

                    MyBase.CambioItem("DiccionarioCombos")

                    HabilitarCamposRegistro(_EncabezadoSeleccionado)
                End If
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private _EncabezadoSeleccionadoAnterior As CFOperaciones.Operaciones_OtrosNegocios
    Public Property EncabezadoSeleccionadoAnterior() As CFOperaciones.Operaciones_OtrosNegocios
        Get
            Return _EncabezadoSeleccionadoAnterior
        End Get
        Set(ByVal value As CFOperaciones.Operaciones_OtrosNegocios)
            _EncabezadoSeleccionadoAnterior = value
        End Set
    End Property

    Private _EncabezadoSeleccionadoDuplicar As CFOperaciones.Operaciones_OtrosNegocios
    Public Property EncabezadoSeleccionadoDuplicar() As CFOperaciones.Operaciones_OtrosNegocios
        Get
            Return _EncabezadoSeleccionadoDuplicar
        End Get
        Set(ByVal value As CFOperaciones.Operaciones_OtrosNegocios)
            _EncabezadoSeleccionadoDuplicar = value
        End Set
    End Property

    Private _ViewOperacionesOtrosNegocios As OperacionesOtrosNegociosView
    Public Property ViewOperacionesOtrosNegocios() As OperacionesOtrosNegociosView
        Get
            Return _ViewOperacionesOtrosNegocios
        End Get
        Set(ByVal value As OperacionesOtrosNegociosView)
            _ViewOperacionesOtrosNegocios = value
        End Set
    End Property

    Private _BusquedaOperacionesOtrosNegocios As CamposBusquedaOperacionesOtrosNegocios = New CamposBusquedaOperacionesOtrosNegocios
    Public Property BusquedaOperacionesOtrosNegocios() As CamposBusquedaOperacionesOtrosNegocios
        Get
            Return _BusquedaOperacionesOtrosNegocios
        End Get
        Set(ByVal value As CamposBusquedaOperacionesOtrosNegocios)
            _BusquedaOperacionesOtrosNegocios = value
            MyBase.CambioItem("BusquedaOperacionesOtrosNegocios")
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

#Region "Propiedades Receptores"

    Private _ListaReceptores As List(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)
    Public Property ListaReceptores() As List(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)
        Get
            Return _ListaReceptores
        End Get
        Set(ByVal value As List(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios))
            _ListaReceptores = value
            MyBase.CambioItem("ListaReceptores")
        End Set
    End Property

    Private _ReceptorSeleccionado As CFOperaciones.Operaciones_ReceptoresOtrosNegocios
    Public Property ReceptorSeleccionado() As CFOperaciones.Operaciones_ReceptoresOtrosNegocios
        Get
            Return _ReceptorSeleccionado
        End Get
        Set(ByVal value As CFOperaciones.Operaciones_ReceptoresOtrosNegocios)
            _ReceptorSeleccionado = value
            MyBase.CambioItem("ReceptorSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades Tipo Negocio y tipo origen"

    Private _ListaTipoNegocio As List(Of CFOperaciones.Operaciones_TiposNegocio)
    Public Property ListaTipoNegocio() As List(Of CFOperaciones.Operaciones_TiposNegocio)
        Get
            Return _ListaTipoNegocio
        End Get
        Set(ByVal value As List(Of CFOperaciones.Operaciones_TiposNegocio))
            _ListaTipoNegocio = value
            MyBase.CambioItem("ListaTipoNegocio")
        End Set
    End Property

    Private _ListaTipoOrigenPermisos As List(Of CFOperaciones.Operaciones_TiposNegocio)
    Public Property ListaTipoOrigenPermisos() As List(Of CFOperaciones.Operaciones_TiposNegocio)
        Get
            Return _ListaTipoOrigenPermisos
        End Get
        Set(ByVal value As List(Of CFOperaciones.Operaciones_TiposNegocio))
            _ListaTipoOrigenPermisos = value
            MyBase.CambioItem("ListaTipoOrigenPermisos")
        End Set
    End Property

    Private _ListaTipoOrigen As List(Of CFOperaciones.Operaciones_TiposNegocio)
    Public Property ListaTipoOrigen() As List(Of CFOperaciones.Operaciones_TiposNegocio)
        Get
            Return _ListaTipoOrigen
        End Get
        Set(ByVal value As List(Of CFOperaciones.Operaciones_TiposNegocio))
            _ListaTipoOrigen = value
            MyBase.CambioItem("ListaTipoOrigen")
        End Set
    End Property

#End Region

#Region "Propiedades para el Ordenante"

    Private _ListaOrdenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
    Public Property ListaOrdenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
        Get
            Return (_ListaOrdenantes)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorOrdenantes))
            _ListaOrdenantes = value
            MyBase.CambioItem("ListaOrdenantes")
            If Not IsNothing(ListaOrdenantes) Then
                If logNuevoRegistro = False And logEditarRegistro = False Then
                    If ListaOrdenantes.Where(Function(i) i.IdOrdenante = _EncabezadoSeleccionado.IDOrdenante).Count > 0 Then
                        OrdenanteSeleccionado = ListaOrdenantes.Where(Function(i) i.IdOrdenante = _EncabezadoSeleccionado.IDOrdenante).FirstOrDefault
                    End If
                Else
                    If ListaOrdenantes.Count = 1 Then
                        OrdenanteSeleccionado = ListaOrdenantes.FirstOrDefault
                    Else
                        If ListaOrdenantes.Where(Function(i) i.IdOrdenante = _EncabezadoSeleccionado.IDOrdenante).Count > 0 Then
                            OrdenanteSeleccionado = ListaOrdenantes.Where(Function(i) i.IdOrdenante = _EncabezadoSeleccionado.IDOrdenante).FirstOrDefault
                        End If
                    End If
                End If
            End If
        End Set
    End Property

    Private _mobjOrdenanteSeleccionado As OYDUtilidades.BuscadorOrdenantes
    Public Property OrdenanteSeleccionado() As OYDUtilidades.BuscadorOrdenantes
        Get
            Return (_mobjOrdenanteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorOrdenantes)
            _mobjOrdenanteSeleccionado = value
            If Not IsNothing(OrdenanteSeleccionado) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    If Not IsNothing(_EncabezadoSeleccionado) Then
                        _EncabezadoSeleccionado.IDOrdenante = OrdenanteSeleccionado.IdOrdenante
                    End If
                End If
            End If
            MyBase.CambioItem("OrdenanteSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades para las cuentas deposito"

    Private _ListaCuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property ListaCuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return (_ListaCuentasDeposito)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _ListaCuentasDeposito = value
            MyBase.CambioItem("ListaCuentasDeposito")
            If Not IsNothing(ListaCuentasDeposito) Then
                If logNuevoRegistro = False And logEditarRegistro = False Then
                    If ListaCuentasDeposito.Where(Function(i) i.NroCuentaDeposito = _EncabezadoSeleccionado.IDCuentaDeposito).Count > 0 Then
                        CtaDepositoSeleccionada = ListaCuentasDeposito.Where(Function(i) i.NroCuentaDeposito = IIf(IsNothing(_EncabezadoSeleccionado.IDCuentaDeposito), Nothing, 0)).FirstOrDefault
                    End If
                Else
                    If ListaCuentasDeposito.Count = 1 Then
                        CtaDepositoSeleccionada = ListaCuentasDeposito.FirstOrDefault
                    Else
                        If ListaCuentasDeposito.Where(Function(i) i.NroCuentaDeposito = IIf(IsNothing(_EncabezadoSeleccionado.IDCuentaDeposito), Nothing, 0)).Count > 0 Then
                            CtaDepositoSeleccionada = ListaCuentasDeposito.Where(Function(i) i.NroCuentaDeposito = IIf(IsNothing(_EncabezadoSeleccionado.IDCuentaDeposito), Nothing, 0)).FirstOrDefault
                        End If
                    End If

                End If
            Else
                CtaDepositoSeleccionada = Nothing
            End If
        End Set
    End Property

    Private _mobjCtaDepositoSeleccionada As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CtaDepositoSeleccionada() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCtaDepositoSeleccionada)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            _mobjCtaDepositoSeleccionada = value
            If Not IsNothing(CtaDepositoSeleccionada) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    If Not IsNothing(_EncabezadoSeleccionado) Then
                        _EncabezadoSeleccionado.IDCuentaDeposito = CtaDepositoSeleccionada.NroCuentaDeposito
                        _EncabezadoSeleccionado.Deposito = CtaDepositoSeleccionada.Deposito
                    End If
                End If
            Else
                If logEditarRegistro Or logNuevoRegistro Then
                    _EncabezadoSeleccionado.IDCuentaDeposito = 0
                    _EncabezadoSeleccionado.Deposito = String.Empty
                End If
            End If
            MyBase.CambioItem("CtaDepositoSeleccionada")
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

    Private _IsBusyCalculos As String
    Public Property IsBusyCalculos() As String
        Get
            Return _IsBusyCalculos
        End Get
        Set(ByVal value As String)
            _IsBusyCalculos = value
            MyBase.CambioItem("IsBusyCalculos")
        End Set
    End Property

    Private _HabilitaBoton As Boolean = True
    Public Property HabilitaBoton As Boolean
        Get
            Return _HabilitaBoton
        End Get
        Set(ByVal value As Boolean)
            _HabilitaBoton = value
            MyBase.CambioItem("HabilitaBoton")
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
                If Not IsNothing(ComitenteSeleccionado) Then
                    OrdenanteSeleccionado = Nothing
                    CtaDepositoSeleccionada = Nothing
                    consultarOrdenantes(ComitenteSeleccionado.IdComitente)
                    consultarCuentasDeposito(ComitenteSeleccionado.IdComitente)
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad del cliente seleccionado.", Me.ToString, "ComitenteSeleccionado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try

            MyBase.CambioItem("ComitenteSeleccionado")
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

    Private _TipoProductoFiltroCliente As String
    Public Property TipoProductoFiltroCliente() As String
        Get
            Return _TipoProductoFiltroCliente
        End Get
        Set(ByVal value As String)
            _TipoProductoFiltroCliente = value
            If BorrarCliente Then
                BorrarCliente = False
            End If
            BorrarCliente = True
            MyBase.CambioItem("TipoProductoFiltroCliente")
        End Set
    End Property

    Private _ListaComitentesContraparte As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComitentesContraparte() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComitentesContraparte
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComitentesContraparte = value
            MyBase.CambioItem("ListaComitentesContraparte")
        End Set
    End Property

#End Region

#Region "Propiedades de la Especie"

    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionado = value
            Try
                If logCancelarRegistro = False Then
                    SeleccionarEspecie(NemotecnicoSeleccionado)
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad de la especie seleccionada.", Me.ToString, "NemotecnicoSeleccionado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try
            MyBase.CambioItem("NemotecnicoSeleccionado")
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

    Private _ClaseEspecies As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie
    Public Property ClaseEspecies() As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie
        Get
            Return _ClaseEspecies
        End Get
        Set(ByVal value As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie)
            _ClaseEspecies = value
            MyBase.CambioItem("ClaseEspecies")
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

#Region "Propiedades para las cuentas deposito contraparte"

    Private _ListaCuentasDepositoContraparte As List(Of CFOperaciones.Operaciones_EntidadesCuentasDeposito)
    Public Property ListaCuentasDepositoContraparte As List(Of CFOperaciones.Operaciones_EntidadesCuentasDeposito)
        Get
            Return (_ListaCuentasDepositoContraparte)
        End Get
        Set(ByVal value As List(Of CFOperaciones.Operaciones_EntidadesCuentasDeposito))
            _ListaCuentasDepositoContraparte = value
            MyBase.CambioItem("ListaCuentasDepositoContraparte")
            If Not IsNothing(_ListaCuentasDepositoContraparte) Then
                If logNuevoRegistro = False And logEditarRegistro = False Then
                    If Not IsNothing(_EncabezadoSeleccionado.IDCuentaDepositoContraparte) Then
                        If _ListaCuentasDepositoContraparte.Where(Function(i) i.ID = _EncabezadoSeleccionado.IDCuentaDepositoContraparte).Count > 0 Then
                            CtaDepositoSeleccionadaContraparte = _ListaCuentasDepositoContraparte.Where(Function(i) i.ID = _EncabezadoSeleccionado.IDCuentaDepositoContraparte).FirstOrDefault
                        End If
                    End If
                Else
                    If _ListaCuentasDepositoContraparte.Count = 1 Then
                        CtaDepositoSeleccionadaContraparte = _ListaCuentasDepositoContraparte.FirstOrDefault
                    Else
                        If Not IsNothing(_EncabezadoSeleccionado.IDCuentaDepositoContraparte) Then
                            If _ListaCuentasDepositoContraparte.Where(Function(i) i.ID = _EncabezadoSeleccionado.IDCuentaDepositoContraparte).Count > 0 Then
                                CtaDepositoSeleccionadaContraparte = _ListaCuentasDepositoContraparte.Where(Function(i) i.ID = _EncabezadoSeleccionado.IDCuentaDepositoContraparte).FirstOrDefault
                            End If
                        End If
                    End If
                End If
            Else
                CtaDepositoSeleccionadaContraparte = Nothing
            End If
        End Set
    End Property

    Private _mobjCtaDepositoSeleccionadaContraparte As CFOperaciones.Operaciones_EntidadesCuentasDeposito
    Public Property CtaDepositoSeleccionadaContraparte() As CFOperaciones.Operaciones_EntidadesCuentasDeposito
        Get
            Return (_mobjCtaDepositoSeleccionadaContraparte)
        End Get
        Set(ByVal value As CFOperaciones.Operaciones_EntidadesCuentasDeposito)
            _mobjCtaDepositoSeleccionadaContraparte = value
            If Not IsNothing(_mobjCtaDepositoSeleccionadaContraparte) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    If Not IsNothing(_EncabezadoSeleccionado) Then
                        _EncabezadoSeleccionado.IDCuentaDepositoContraparte = _mobjCtaDepositoSeleccionadaContraparte.ID
                        _EncabezadoSeleccionado.DescripcionCuentaDepositoContraparte = _mobjCtaDepositoSeleccionadaContraparte.DescripcionCuenta
                    End If
                End If
            Else
                If logEditarRegistro Or logNuevoRegistro Then
                    _EncabezadoSeleccionado.IDCuentaDepositoContraparte = Nothing
                    _EncabezadoSeleccionado.DescripcionCuentaDepositoContraparte = String.Empty
                End If
            End If
            MyBase.CambioItem("CtaDepositoSeleccionadaContraparte")
        End Set
    End Property

    Private _ListaCuentasDepositoContraparteOTC As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property ListaCuentasDepositoContraparteOTC As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return (_ListaCuentasDepositoContraparteOTC)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _ListaCuentasDepositoContraparteOTC = value
            MyBase.CambioItem("ListaCuentasDepositoContraparteOTC")
            If Not IsNothing(ListaCuentasDepositoContraparteOTC) Then
                If logNuevoRegistro = False And logEditarRegistro = False Then
                    If Not IsNothing(_EncabezadoSeleccionado.NroCuentaDepositoContraparte) Then
                        If ListaCuentasDepositoContraparteOTC.Where(Function(i) i.NroCuentaDeposito = _EncabezadoSeleccionado.NroCuentaDepositoContraparte).Count > 0 Then
                            CtaDepositoSeleccionadaContraparteOTC = ListaCuentasDepositoContraparteOTC.Where(Function(i) i.NroCuentaDeposito = IIf(IsNothing(_EncabezadoSeleccionado.NroCuentaDepositoContraparte), Nothing, 0)).FirstOrDefault
                        End If
                    End If
                Else
                    If ListaCuentasDepositoContraparteOTC.Count = 1 Then
                        CtaDepositoSeleccionadaContraparteOTC = ListaCuentasDepositoContraparteOTC.FirstOrDefault
                    Else
                        If ListaCuentasDepositoContraparteOTC.Where(Function(i) i.NroCuentaDeposito = IIf(IsNothing(_EncabezadoSeleccionado.NroCuentaDepositoContraparte), Nothing, 0)).Count > 0 Then
                            CtaDepositoSeleccionadaContraparteOTC = ListaCuentasDepositoContraparteOTC.Where(Function(i) i.NroCuentaDeposito = IIf(IsNothing(_EncabezadoSeleccionado.NroCuentaDepositoContraparte), Nothing, 0)).FirstOrDefault
                        End If
                    End If

                End If
            Else
                CtaDepositoSeleccionadaContraparteOTC = Nothing
            End If
        End Set
    End Property

    Private _mobjCtaDepositoSeleccionadaContraparteOTC As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CtaDepositoSeleccionadaContraparteOTC() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCtaDepositoSeleccionadaContraparteOTC)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            _mobjCtaDepositoSeleccionadaContraparteOTC = value
            If Not IsNothing(CtaDepositoSeleccionadaContraparteOTC) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    If Not IsNothing(_EncabezadoSeleccionado) Then
                        _EncabezadoSeleccionado.NroCuentaDepositoContraparte = CtaDepositoSeleccionadaContraparteOTC.NroCuentaDeposito
                        _EncabezadoSeleccionado.DepositoContraparte = CtaDepositoSeleccionadaContraparteOTC.Deposito
                        _EncabezadoSeleccionado.DescripcionCuentaDepositoContraparte = CtaDepositoSeleccionadaContraparteOTC.NombreDeposito & " Cuenta: " & CtaDepositoSeleccionadaContraparteOTC.NroCuentaDeposito
                    End If
                End If
            Else
                If logEditarRegistro Or logNuevoRegistro Then
                    _EncabezadoSeleccionado.NroCuentaDepositoContraparte = 0
                    _EncabezadoSeleccionado.DepositoContraparte = String.Empty
                    _EncabezadoSeleccionado.DescripcionCuentaDepositoContraparte = String.Empty
                End If
            End If
            MyBase.CambioItem("CtaDepositoSeleccionadaContraparteOTC")
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


    Private _FormatoCamposDecimalesEspeciales As String = "2"
    Public Property FormatoCamposDecimalesEspeciales() As String
        Get
            Return _FormatoCamposDecimalesEspeciales
        End Get
        Set(ByVal value As String)
            _FormatoCamposDecimalesEspeciales = value
            MyBase.CambioItem("FormatoCamposDecimalesEspeciales")
        End Set
    End Property

    Private _FormatoCamposNumericosEspeciales As String = "2"
    Public Property FormatoCamposNumericosEspeciales() As String
        Get
            Return _FormatoCamposNumericosEspeciales
        End Get
        Set(ByVal value As String)
            _FormatoCamposNumericosEspeciales = value
            MyBase.CambioItem("FormatoCamposNumericosEspeciales")
        End Set
    End Property

    Private _FormatoCamposDecimalesPorcentajeComision As String = "3"
    Public Property FormatoCamposDecimalesPorcentajeComision() As String
        Get
            Return _FormatoCamposDecimalesPorcentajeComision
        End Get
        Set(ByVal value As String)
            _FormatoCamposDecimalesPorcentajeComision = value
            MyBase.CambioItem("FormatoCamposDecimalesPorcentajeComision")
        End Set
    End Property

    Private _FormatoCamposNumericosPorcentajeComision As String = "3"
    Public Property FormatoCamposNumericosPorcentajeComision() As String
        Get
            Return _FormatoCamposNumericosPorcentajeComision
        End Get
        Set(ByVal value As String)
            _FormatoCamposNumericosPorcentajeComision = value
            MyBase.CambioItem("FormatoCamposNumericosPorcentajeComision")
        End Set
    End Property


#End Region

#Region "Propiedades Visibilidad"

    Private _MostrarContraparte As Visibility
    Public Property MostrarContraparte() As Visibility
        Get
            Return _MostrarContraparte
        End Get
        Set(ByVal value As Visibility)
            _MostrarContraparte = value
            MyBase.CambioItem("MostrarContraparte")
        End Set
    End Property

    Private _MostrarContraparteOTC As Visibility
    Public Property MostrarContraparteOTC() As Visibility
        Get
            Return _MostrarContraparteOTC
        End Get
        Set(ByVal value As Visibility)
            _MostrarContraparteOTC = value
            MyBase.CambioItem("MostrarContraparteOTC")
        End Set
    End Property

    Private _MostrarTipoBanRep As Visibility
    Public Property MostrarTipoBanRep() As Visibility
        Get
            Return _MostrarTipoBanRep
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoBanRep = value
            MyBase.CambioItem("MostrarTipoBanRep")
        End Set
    End Property

    Private _MostrarOrdenante As Visibility
    Public Property MostrarOrdenante() As Visibility
        Get
            Return _MostrarOrdenante
        End Get
        Set(ByVal value As Visibility)
            _MostrarOrdenante = value
            MyBase.CambioItem("MostrarOrdenante")
        End Set
    End Property

    Private _MostrarCuentaDeposito As Visibility
    Public Property MostrarCuentaDeposito() As Visibility
        Get
            Return _MostrarCuentaDeposito
        End Get
        Set(ByVal value As Visibility)
            _MostrarCuentaDeposito = value
            MyBase.CambioItem("MostrarCuentaDeposito")
        End Set
    End Property

#End Region

#End Region

#Region "Metodos"

    Public Overrides Async Sub NuevoRegistro()
        Try
            If logCargoForma = False Then
                ViewOperacionesOtrosNegocios.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
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

            TipoProductoFiltroCliente = "TOD"
            objEntidadPorDefecto = Await ConsultarEntidadPorDefecto()
            Await CargarTipoNegocioUsuario(OPCION_NUEVO)

            HabilitaBoton = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub NuevaOperacion()
        Try
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, True)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)

            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)

            Dim objNewOperaciones_OtrosNegocios As New CFOperaciones.Operaciones_OtrosNegocios
            objNewOperaciones_OtrosNegocios.ID = 0
            objNewOperaciones_OtrosNegocios.Referencia = String.Empty
            objNewOperaciones_OtrosNegocios.Estado = ESTADO_INGRESADA
            objNewOperaciones_OtrosNegocios.NombreEstado = "Ingresada"
            objNewOperaciones_OtrosNegocios.NombreTipoNegocio = String.Empty
            objNewOperaciones_OtrosNegocios.TipoOrigen = String.Empty
            objNewOperaciones_OtrosNegocios.NombreTipoOrigen = String.Empty
            objNewOperaciones_OtrosNegocios.IDComitente = String.Empty
            objNewOperaciones_OtrosNegocios.NombreCliente = String.Empty
            objNewOperaciones_OtrosNegocios.NombreCliente = String.Empty
            objNewOperaciones_OtrosNegocios.CodTipoIdentificacionCliente = String.Empty
            objNewOperaciones_OtrosNegocios.TipoIdentificacionCliente = String.Empty
            objNewOperaciones_OtrosNegocios.NumeroDocumentoCliente = String.Empty
            objNewOperaciones_OtrosNegocios.IDOrdenante = String.Empty
            objNewOperaciones_OtrosNegocios.NombreOrdenante = String.Empty
            objNewOperaciones_OtrosNegocios.CodTipoIdentificacionOrdenante = String.Empty
            objNewOperaciones_OtrosNegocios.TipoIdentificacionOrdenante = String.Empty
            objNewOperaciones_OtrosNegocios.NumeroDocumentoOrdenante = String.Empty
            objNewOperaciones_OtrosNegocios.Deposito = String.Empty
            objNewOperaciones_OtrosNegocios.IDCuentaDeposito = 0
            objNewOperaciones_OtrosNegocios.IDContraparte = 0
            objNewOperaciones_OtrosNegocios.NombreContraparte = String.Empty
            objNewOperaciones_OtrosNegocios.CodTipoIdentificacionContraparte = String.Empty
            objNewOperaciones_OtrosNegocios.TipoIdentificacionContraparte = String.Empty
            objNewOperaciones_OtrosNegocios.NumeroDocumentoContraparte = String.Empty
            objNewOperaciones_OtrosNegocios.ClasificacionInversion = String.Empty
            objNewOperaciones_OtrosNegocios.NombreClasificacionInversion = String.Empty
            objNewOperaciones_OtrosNegocios.IDPagador = 0
            objNewOperaciones_OtrosNegocios.NombrePagador = String.Empty
            objNewOperaciones_OtrosNegocios.CodTipoIdentificacionPagador = String.Empty
            objNewOperaciones_OtrosNegocios.TipoIdentificacionPagador = String.Empty
            objNewOperaciones_OtrosNegocios.NumeroDocumentoPagador = String.Empty
            objNewOperaciones_OtrosNegocios.TipoOperacion = String.Empty
            objNewOperaciones_OtrosNegocios.NombreTipoOperacion = String.Empty
            objNewOperaciones_OtrosNegocios.FechaOperacion = dtmFechaServidor
            objNewOperaciones_OtrosNegocios.FechaCumplimiento = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
            objNewOperaciones_OtrosNegocios.FechaVencimientoOperacion = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, dtmFechaServidor.Date))
            objNewOperaciones_OtrosNegocios.Nemotecnico = String.Empty
            objNewOperaciones_OtrosNegocios.ISIN = String.Empty
            objNewOperaciones_OtrosNegocios.FechaEmision = Nothing
            objNewOperaciones_OtrosNegocios.FechaVencimiento = Nothing
            objNewOperaciones_OtrosNegocios.Modalidad = Nothing
            objNewOperaciones_OtrosNegocios.TasaFacial = Nothing
            objNewOperaciones_OtrosNegocios.IndicadorEconomico = Nothing
            objNewOperaciones_OtrosNegocios.PuntosIndicador = Nothing
            objNewOperaciones_OtrosNegocios.TipoCumplimiento = String.Empty
            objNewOperaciones_OtrosNegocios.NombreTipoCumplimiento = String.Empty
            objNewOperaciones_OtrosNegocios.Mercado = String.Empty
            objNewOperaciones_OtrosNegocios.NombreMercado = String.Empty
            objNewOperaciones_OtrosNegocios.Nominal = 0
            objNewOperaciones_OtrosNegocios.IDMoneda = 0
            objNewOperaciones_OtrosNegocios.CodigoMoneda = String.Empty
            objNewOperaciones_OtrosNegocios.NombreMoneda = String.Empty
            objNewOperaciones_OtrosNegocios.TasaCambioConversion = 0
            objNewOperaciones_OtrosNegocios.PrecioSucio = 0
            objNewOperaciones_OtrosNegocios.TasaNegociacionYield = 0
            objNewOperaciones_OtrosNegocios.ValorGiroBruto = 0
            objNewOperaciones_OtrosNegocios.TasaPactadaRepo = 0
            objNewOperaciones_OtrosNegocios.Haircut = 0
            objNewOperaciones_OtrosNegocios.PorcentajeComision = 0
            objNewOperaciones_OtrosNegocios.ValorComision = 0
            objNewOperaciones_OtrosNegocios.IVA = 0
            objNewOperaciones_OtrosNegocios.TipoReteFuente = String.Empty
            objNewOperaciones_OtrosNegocios.NombreTipoReteFuente = 0
            objNewOperaciones_OtrosNegocios.Retencion = 0
            objNewOperaciones_OtrosNegocios.ValorNeto = 0
            objNewOperaciones_OtrosNegocios.ValorRegresoRepo = 0
            objNewOperaciones_OtrosNegocios.Observaciones = String.Empty
            objNewOperaciones_OtrosNegocios.PaisNegociacion = 0
            objNewOperaciones_OtrosNegocios.CodigoPaisNegociacion = String.Empty
            objNewOperaciones_OtrosNegocios.NombrePaisNegociacion = String.Empty
            objNewOperaciones_OtrosNegocios.TipoRepo = String.Empty
            objNewOperaciones_OtrosNegocios.NombreTipoRepo = String.Empty
            objNewOperaciones_OtrosNegocios.UsuarioRegistro = Program.Usuario
            objNewOperaciones_OtrosNegocios.TipoRegistro = TIPOREGISTRO_MANUAL
            objNewOperaciones_OtrosNegocios.NombreTipoRegistro = "Manual"
            objNewOperaciones_OtrosNegocios.FechaRegistro = dtmFechaServidor
            objNewOperaciones_OtrosNegocios.Actualizacion = dtmFechaServidor
            objNewOperaciones_OtrosNegocios.Usuario = Program.Usuario
            objNewOperaciones_OtrosNegocios.UsuarioWindows = Program.UsuarioWindows
            objNewOperaciones_OtrosNegocios.Maquina = Program.Maquina
            objNewOperaciones_OtrosNegocios.TipoBanRep = False
            objNewOperaciones_OtrosNegocios.ValorFlujoIntermedio = 0

            Editando = True

            ObtenerValoresRegistroAnterior(objNewOperaciones_OtrosNegocios, EncabezadoSeleccionado)

            CtaDepositoSeleccionada = Nothing
            If Not IsNothing(ListaCuentasDeposito) Then
                Dim objListaCuenta As New List(Of OYDUtilidades.BuscadorCuentasDeposito)
                ListaCuentasDeposito = Nothing
                ListaCuentasDeposito = objListaCuenta
            End If

            OrdenanteSeleccionado = Nothing
            If Not IsNothing(ListaOrdenantes) Then
                Dim objListaOrdenante As New List(Of OYDUtilidades.BuscadorOrdenantes)
                ListaOrdenantes = Nothing
                ListaOrdenantes = objListaOrdenante
            End If

            ReceptorSeleccionado = Nothing
            If Not IsNothing(ListaReceptores) Then
                Dim objListaReceptores As New List(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)
                ListaReceptores = Nothing
                ListaReceptores = objListaReceptores
            End If

            CtaDepositoSeleccionadaContraparte = Nothing
            If Not IsNothing(ListaCuentasDepositoContraparte) Then
                Dim objListaCuentaContraparte As New List(Of CFOperaciones.Operaciones_EntidadesCuentasDeposito)
                ListaCuentasDepositoContraparte = Nothing
                ListaCuentasDepositoContraparte = objListaCuentaContraparte
            End If

            If BorrarCliente Then
                BorrarCliente = False
            End If

            BorrarCliente = True

            If BorrarEspecie Then
                BorrarEspecie = False
            End If

            BorrarEspecie = True

            BuscarControlValidacion(viewFormaOperacionesOtrosNegocios, "tabItemValoresComisiones")
            'Se posiciona en el primer registro
            BuscarControlValidacion(viewFormaOperacionesOtrosNegocios, "cboTipoNegocio")

            If _ListaTipoNegocio.Count = 1 Then
                EncabezadoSeleccionado.TipoNegocio = _ListaTipoNegocio.First.CodigoTipoNegocio
            End If

            RaiseEvent TerminoConfigurarNuevoRegistro()

            CambiarColorFondoTextoBuscador()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevaOperacion", Program.TituloSistema, Program.Maquina, ex)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)

            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)
            Editando = False
        End Try
    End Sub

    Public Async Sub PreguntarDuplicarRegistro()
        Try
            'JP20190626 Se comenta porque genera error de visualización cuando de presiona el botón aplazar la primera vez que carga la pantalla en modo forma.
            If logCargoForma = False Then
                ViewOperacionesOtrosNegocios.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
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

    Public Async Sub PreguntarAplicarLiqxDife()
        Try
            If logCargoForma = False Then
                ViewOperacionesOtrosNegocios.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
                logCargoForma = True
            End If

            If Not IsNothing(_EncabezadoSeleccionado) Then
                Await validarEstadoRegistro(OPCION_LIQXDIFE)
            Else
                mostrarMensaje("Debe de seleccionar un registro para aplicar liquidación por diferencia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preguntar sí se deseaba aplicar liquidación por diferencia", Me.ToString(), "PreguntarAplicarLiqxDife", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub DuplicarRegistro()
        Try
            EncabezadoSeleccionadoAnterior = Nothing

            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)
            dtmFechaServidor = Await ObtenerFechaHoraServidor()

            'Crea el nuevo registro para duplicar.
            Dim objNewRegistroDuplicar As New CFOperaciones.Operaciones_OtrosNegocios
            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, objNewRegistroDuplicar)

            objNewRegistroDuplicar.ID = 0
            objNewRegistroDuplicar.FechaRegistro = dtmFechaServidor
            objNewRegistroDuplicar.Actualizacion = dtmFechaServidor
            objNewRegistroDuplicar.UsuarioRegistro = Program.Usuario
            objNewRegistroDuplicar.Usuario = Program.Usuario
            objNewRegistroDuplicar.UsuarioWindows = Program.UsuarioWindows
            objNewRegistroDuplicar.Maquina = Program.Maquina
            objNewRegistroDuplicar.Estado = ESTADO_INGRESADA
            objNewRegistroDuplicar.NombreEstado = "Ingresada"
            objNewRegistroDuplicar.Referencia = String.Empty

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = True
            logNuevoRegistro = False

            TipoProductoFiltroCliente = "TOD"

            ObtenerValoresRegistroAnterior(objNewRegistroDuplicar, EncabezadoSeleccionadoDuplicar)

            Await CargarTipoNegocioUsuario(OPCION_DUPLICAR, OPCION_DUPLICAR)

            BuscarControlValidacion(viewFormaOperacionesOtrosNegocios, "tabItemValoresComisiones")

            strTipoCalculo = TIPOCALCULOS_MOTOR.PRECIO.ToString

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro duplicado", Me.ToString(), "DuplicarRegistro", Program.TituloSistema, Program.Maquina, ex)
            Editando = False
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)

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
            If Not IsNothing(BusquedaOperacionesOtrosNegocios) Then
                IsBusy = True
                Editando = False
                MyBase.CambioItem("Editando")
                MyBase.ConfirmarBuscar()
                Await ConsultarEncabezado(False, String.Empty, False, 0, _BusquedaOperacionesOtrosNegocios.ID, _BusquedaOperacionesOtrosNegocios.Referencia, _BusquedaOperacionesOtrosNegocios.TipoRegistro, _BusquedaOperacionesOtrosNegocios.Estado, _BusquedaOperacionesOtrosNegocios.TipoNegocio, _BusquedaOperacionesOtrosNegocios.TipoOrigen, _BusquedaOperacionesOtrosNegocios.TipoOperacion, _BusquedaOperacionesOtrosNegocios.Cliente, _BusquedaOperacionesOtrosNegocios.Especie, _BusquedaOperacionesOtrosNegocios.FechaOperacion, _BusquedaOperacionesOtrosNegocios.FechaCumplimiento)
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
                                               Optional ByVal pstrReferencia As String = "",
                                               Optional ByVal pstrTipoRegistro As String = "",
                                               Optional ByVal pstrEstado As String = "",
                                               Optional ByVal pstrTipoNegocio As String = "",
                                               Optional ByVal pstrTipoOrigen As String = "",
                                               Optional ByVal pstrTipoOperacion As String = "",
                                               Optional ByVal pstrCliente As String = "",
                                               Optional ByVal pstrEspecie As String = "",
                                               Optional ByVal pdtmFechaOperacion As Nullable(Of DateTime) = Nothing,
                                               Optional ByVal pdtmFechaCumplimiento As Nullable(Of DateTime) = Nothing) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_OtrosNegocios)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Operaciones_OtrosNegocios.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosFiltrarSyncQuery(ESTADO_INGRESADA, pstrFiltro, Program.Usuario, Program.HashConexion)).AsTask()
            Else
                objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosConsultarSyncQuery(pintID, pstrReferencia, pstrTipoRegistro, pstrEstado, pstrTipoNegocio, pstrTipoOrigen, pstrTipoOperacion, pstrCliente, pstrEspecie, pdtmFechaOperacion, pdtmFechaCumplimiento, Program.Usuario, Program.HashConexion)).AsTask()
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

    Public Async Function ConsultarDetalle(ByVal pintIDOperacion As System.Nullable(Of Integer)) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)

        Try
            ErrorForma = String.Empty

            dcProxy.Operaciones_ReceptoresOtrosNegocios.Clear()

            objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosReceptoresSyncQuery(pintIDOperacion, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el de talle de la distribución.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el de talle de la distribución.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaReceptores = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el de talle de la distribución.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Async Function ConsultarEntidadPorDefecto() As Task(Of CFOperaciones.Operaciones_EntidadDefecto)

        Dim objResultado As CFOperaciones.Operaciones_EntidadDefecto = Nothing
        Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_EntidadDefecto)

        Try
            ErrorForma = String.Empty

            dcProxy.Operaciones_EntidadDefectos.Clear()

            objRet = Await dcProxy.Load(dcProxy.Operaciones_EntidadDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el de talle de la distribución.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el de talle de la distribución.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    If objRet.Entities.ToList.Count > 0 Then
                        objResultado = objRet.Entities.First
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el de talle de la distribución.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objResultado)
    End Function

    Public Async Function ConsultarReceptoresCliente(ByVal pstrCliente As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)

        Try
            ErrorForma = String.Empty

            dcProxy.Operaciones_ReceptoresOtrosNegocios.Clear()

            objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosReceptoresClienteSyncQuery(pstrCliente, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar los receptores del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar los receptores del cliente.", Me.ToString(), "ConsultarReceptoresCliente", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    Dim objListaReceptor As New List(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)

                    For Each li In objRet.Entities.ToList
                        Dim NewReceptoresOperacion As New CFOperaciones.Operaciones_ReceptoresOtrosNegocios

                        NewReceptoresOperacion.IDLiquidacion = _EncabezadoSeleccionado.ID
                        NewReceptoresOperacion.Actualizacion = Now
                        NewReceptoresOperacion.Usuario = Program.Usuario
                        NewReceptoresOperacion.Lider = li.Lider
                        NewReceptoresOperacion.Porcentaje = li.Porcentaje
                        NewReceptoresOperacion.NombreReceptor = li.NombreReceptor
                        NewReceptoresOperacion.Receptor = li.Receptor

                        objListaReceptor.Add(NewReceptoresOperacion)
                    Next

                    ListaReceptores = objListaReceptor
                    If objListaReceptor.Count > 0 Then
                        ReceptorSeleccionado = objListaReceptor.First
                    Else
                        ReceptorSeleccionado = Nothing
                    End If
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar los receptores del cliente.", Me.ToString(), "ConsultarReceptoresCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Overrides Async Sub ActualizarRegistro()
        Try
            IsBusy = True
            dtmFechaServidor = Await ObtenerFechaHoraServidor()
            IsBusy = True

            If Await CalcularValorRegistro() Then
                Await ActualizarOperaciones_OtrosNegocios(_EncabezadoSeleccionado)
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ActualizarOperaciones_OtrosNegocios(ByVal objRegistroSelected As CFOperaciones.Operaciones_OtrosNegocios) As Task
        Try
            IsBusy = True

            If Not IsNothing(objRegistroSelected) Then

                If ValidarGuardadoRegistro(objRegistroSelected) Then
                    Await CargarTipoNegocioUsuario(OPCION_ACTUALIZAR)
                Else
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "ActualizarOperaciones_OtrosNegocios", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Public Async Function GuardarOperaciones_OtrosNegocios(ByVal objRegistroSelected As CFOperaciones.Operaciones_OtrosNegocios) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_RespuestaValidacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            '---------------------------------------
            '-- CCM20160109: Mover validación del cierre de portafolio del código fuente al sp de validar estado 

            Dim strReceptoresXML As String = String.Empty

            For Each li In _ListaReceptores
                If String.IsNullOrEmpty(strReceptoresXML) Then
                    strReceptoresXML = String.Format("{0}|{1}|{2}", li.Receptor, IIf(li.Lider, 1, 0), li.Porcentaje)
                Else
                    strReceptoresXML = String.Format("{0}**{1}|{2}|{3}", strReceptoresXML, li.Receptor, IIf(li.Lider, 1, 0), li.Porcentaje)
                End If
            Next

            strReceptoresXML = Replace(strReceptoresXML, ",", ".")

            strReceptoresXML = System.Web.HttpUtility.UrlEncode(strReceptoresXML)
            Dim strObservaciones As String = System.Web.HttpUtility.UrlEncode(_EncabezadoSeleccionado.Observaciones)

            dcProxy.Operaciones_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosValidarSyncQuery(_EncabezadoSeleccionado.ID,
                                                                                          _EncabezadoSeleccionado.Referencia,
                                                                                          _EncabezadoSeleccionado.Estado,
                                                                                          _EncabezadoSeleccionado.TipoNegocio,
                                                                                          _EncabezadoSeleccionado.TipoOrigen,
                                                                                          _EncabezadoSeleccionado.IDComitente,
                                                                                          _EncabezadoSeleccionado.IDOrdenante,
                                                                                          _EncabezadoSeleccionado.Deposito,
                                                                                          _EncabezadoSeleccionado.IDCuentaDeposito,
                                                                                          _EncabezadoSeleccionado.IDContraparte,
                                                                                          _EncabezadoSeleccionado.IDCuentaDepositoContraparte,
                                                                                          _EncabezadoSeleccionado.ClasificacionInversion,
                                                                                          _EncabezadoSeleccionado.IDPagador,
                                                                                          _EncabezadoSeleccionado.TipoOperacion,
                                                                                          _EncabezadoSeleccionado.FechaOperacion,
                                                                                          _EncabezadoSeleccionado.FechaCumplimiento,
                                                                                          _EncabezadoSeleccionado.FechaVencimientoOperacion,
                                                                                          _EncabezadoSeleccionado.Nemotecnico,
                                                                                          _EncabezadoSeleccionado.ISIN,
                                                                                          _EncabezadoSeleccionado.FechaEmision,
                                                                                          _EncabezadoSeleccionado.FechaVencimiento,
                                                                                          _EncabezadoSeleccionado.Modalidad,
                                                                                          _EncabezadoSeleccionado.TasaFacial,
                                                                                          _EncabezadoSeleccionado.IndicadorEconomico,
                                                                                          _EncabezadoSeleccionado.PuntosIndicador,
                                                                                          _EncabezadoSeleccionado.TipoCumplimiento,
                                                                                          _EncabezadoSeleccionado.Mercado,
                                                                                          _EncabezadoSeleccionado.Nominal,
                                                                                          _EncabezadoSeleccionado.IDMoneda,
                                                                                          _EncabezadoSeleccionado.TasaCambioConversion,
                                                                                          _EncabezadoSeleccionado.PrecioSucio,
                                                                                          _EncabezadoSeleccionado.TasaNegociacionYield,
                                                                                          _EncabezadoSeleccionado.ValorGiroBruto,
                                                                                          _EncabezadoSeleccionado.TasaPactadaRepo,
                                                                                          _EncabezadoSeleccionado.Haircut,
                                                                                          _EncabezadoSeleccionado.PorcentajeComision,
                                                                                          _EncabezadoSeleccionado.ValorComision,
                                                                                          _EncabezadoSeleccionado.IVA,
                                                                                          _EncabezadoSeleccionado.TipoReteFuente,
                                                                                          _EncabezadoSeleccionado.Retencion,
                                                                                          _EncabezadoSeleccionado.ValorNeto,
                                                                                          _EncabezadoSeleccionado.ValorRegresoRepo,
                                                                                          strObservaciones,
                                                                                          _EncabezadoSeleccionado.PaisNegociacion,
                                                                                          _EncabezadoSeleccionado.TipoRepo,
                                                                                          _EncabezadoSeleccionado.TipoRegistro,
                                                                                          strReceptoresXML,
                                                                                          Program.Usuario,
                                                                                          Program.UsuarioWindows,
                                                                                          Program.Maquina,
                                                                                          _EncabezadoSeleccionado.ValorNetoCop,
                                                                                          _EncabezadoSeleccionado.TipoBanRep,
                                                                                          _EncabezadoSeleccionado.ValorFlujoIntermedio,
                                                                                          _EncabezadoSeleccionado.IDOrdenOrigen,
                                                                                          _EncabezadoSeleccionado.IDComitenteOtro,
                                                                                          _EncabezadoSeleccionado.Tipo,
                                                                                          _EncabezadoSeleccionado.DepositoContraparte,
                                                                                          _EncabezadoSeleccionado.NroCuentaDepositoContraparte,
                                                                                          _EncabezadoSeleccionado.TasaNeta,
                                                                                          _EncabezadoSeleccionado.ValorGiroBrutoCOP,
                                                                                          _EncabezadoSeleccionado.ValorComisionCOP,
                                                                                          _EncabezadoSeleccionado.ValorRegresoRepoMoneda,
                                                                                          Program.HashConexion,
                                                                                          _EncabezadoSeleccionado.logOperacionAplazada,
                                                                                          dblCantidadAplazada,
                                                                                          dtmFechaCumplimientoAplazada)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "GuardarOperaciones_OtrosNegocios", Program.TituloSistema, Program.Maquina, objRet.Error)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro ", Me.ToString(), "GuardarOperaciones_OtrosNegocios", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Overrides Async Sub TerminoSubmitChanges(ByVal so As SubmitOperation)
        Try
            MyBase.TerminoSubmitChanges(so)

            mostrarMensaje(so.UserState.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

            If logEsModal Then
                RaiseEvent TerminoGuardarRegistro(True, _EncabezadoSeleccionado.ID, _EncabezadoSeleccionado.TipoNegocio, _EncabezadoSeleccionado.TipoOperacion, _EncabezadoSeleccionado.TipoOrigen)
            Else
                ObtenerInformacionCombosCompletos()

                Await ConsultarEncabezado(True, String.Empty, True, _EncabezadoSeleccionado.ID)

                If BorrarEspecie = True Then
                    BorrarEspecie = False
                End If

                If BorrarCliente = True Then
                    BorrarCliente = False
                End If

                BorrarCliente = True
                BorrarEspecie = True

                Editando = False
                IsBusy = False

                CambiarColorFondoTextoBuscador()

                HabilitarCamposRegistro(_EncabezadoSeleccionado)

                HabilitaBoton = True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro.", Me.ToString(), "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub EditarRegistro()
        Try
            If logCargoForma = False Then
                ViewOperacionesOtrosNegocios.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
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
                    TipoProductoFiltroCliente = "TOD"

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

    Private Async Sub EditarOperaciones_OtrosNegocios()
        Try
            EncabezadoSeleccionadoAnterior = Nothing

            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)

            logCancelarRegistro = False
            logEditarRegistro = True
            logDuplicarRegistro = False
            logNuevoRegistro = False

            Await CargarTipoNegocioUsuario(OPCION_EDITAR, OPCION_EDITAR)

            BuscarControlValidacion(viewFormaOperacionesOtrosNegocios, "tabItemValoresComisiones")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro.", Me.ToString(), "EditarOperaciones_OtrosNegocios", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Function validarEstadoRegistro(ByVal pstrAccion As String) As Task
        Try
            Dim strMsg As String = String.Empty
            HabilitaBoton = False

            'SE INICIALIZA EL CAMPO
            If Not IsNothing(_EncabezadoSeleccionado) Then
                _EncabezadoSeleccionado.logOperacionAplazada = False
            End If

            If _EncabezadoSeleccionado.Estado <> ESTADO_INGRESADA And pstrAccion <> OPCION_DUPLICAR And pstrAccion <> OPCION_APLAZAR And pstrAccion <> OPCION_LIQXDIFE Then
                MyBase.RetornarValorEdicionNavegacion()
                strMsg = String.Format("No se puede {0} el registro porque se encuentra en estado {1}", IIf(pstrAccion = OPCION_EDITAR, "Editar", "Anular"), IIf(_EncabezadoSeleccionado.Estado = ESTADO_ANULADA, "Anulada", "Pendiente por aprobar"))
                mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.IsBusy = False
                HabilitaBoton = True
            ElseIf pstrAccion = OPCION_LIQXDIFE Then
                mostrarMensajePregunta("¿Está seguro que desea aplicar el estado liquidación por diferencia para la operación seleccionada?",
                                  Program.TituloSistema,
                                  "LIQXDIFE",
                                  AddressOf TerminoMensajePregunta, False)
            ElseIf _EncabezadoSeleccionado.TipoOperacion = TIPOOPERACION_RECOMPRA Or
                _EncabezadoSeleccionado.TipoOperacion = TIPOOPERACION_REVENTA Or
                _EncabezadoSeleccionado.EsFutura Then

                If pstrAccion = OPCION_EDITAR Then
                    MyBase.RetornarValorEdicionNavegacion()
                End If

                If pstrAccion = OPCION_EDITAR Then
                    strMsg = String.Format("No se puede Editar el registro porque es una operación futura.")
                ElseIf pstrAccion = OPCION_ANULAR Then
                    strMsg = String.Format("No se puede Anular el registro porque es una operación futura.")
                ElseIf pstrAccion = OPCION_DUPLICAR Then
                    strMsg = String.Format("No se puede Duplicar el registro porque es una operación futura.")
                ElseIf pstrAccion = OPCION_APLAZAR Then
                    strMsg = String.Format("No se puede Aplazar el registro porque es una operación futura.")
                    End If

                    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Me.IsBusy = False
                    HabilitaBoton = True
                Else
                    Me.IsBusy = True
                If pstrAccion = OPCION_EDITAR Then
                    If Await ValidarEstadoOperaciones_OtrosNegocios(_EncabezadoSeleccionado, "editar", False) Then
                        EditarOperaciones_OtrosNegocios()
                    Else
                        MyBase.RetornarValorEdicionNavegacion()
                        IsBusy = False
                        HabilitaBoton = True
                    End If
                ElseIf pstrAccion = OPCION_DUPLICAR Then
                    mostrarMensajePregunta("¿Está seguro que desea duplicar el registro?",
                                  Program.TituloSistema,
                                  "DUPLICARREGISTRO",
                                  AddressOf TerminoMensajePregunta, False)
                ElseIf pstrAccion = OPCION_ANULAR Then
                    If Await ValidarEstadoOperaciones_OtrosNegocios(_EncabezadoSeleccionado, "anular", False) Then
                        Await CargarTipoNegocioUsuario(OPCION_ANULAR, OPCION_ANULAR)
                    Else
                        IsBusy = False
                    End If
                    HabilitaBoton = True
                ElseIf pstrAccion = OPCION_APLAZAR Then

                    If Await ValidarEstadoOperaciones_OtrosNegocios(_EncabezadoSeleccionado, "aplazar", True) Then

                        cwOperacionesOtrosNegociosView = New cwOperacionesOtrosNegociosView(Me, EncabezadoSeleccionado, FormatoCamposDecimales, DiccionarioHabilitarCampos)
                        Program.Modal_OwnerMainWindowsPrincipal(cwOperacionesOtrosNegociosView)
                        cwOperacionesOtrosNegociosView.ShowDialog()


                        If EncabezadoSeleccionado.logOperacionAplazada Then
                            EncabezadoSeleccionadoAnterior = Nothing

                            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, EncabezadoSeleccionadoAnterior)
                            'dtmFechaServidor = Await ObtenerFechaHoraServidor()

                            'Crea el nuevo registro para duplicar.
                            Dim objNewRegistroDuplicar As New CFOperaciones.Operaciones_OtrosNegocios
                            ObtenerValoresRegistroAnterior(_EncabezadoSeleccionado, objNewRegistroDuplicar)

                            'objNewRegistroDuplicar.ID = 0
                            objNewRegistroDuplicar.FechaRegistro = dtmFechaServidor
                            objNewRegistroDuplicar.Actualizacion = dtmFechaServidor
                            objNewRegistroDuplicar.UsuarioRegistro = Program.Usuario
                            objNewRegistroDuplicar.Usuario = Program.Usuario
                            objNewRegistroDuplicar.UsuarioWindows = Program.UsuarioWindows
                            objNewRegistroDuplicar.Maquina = Program.Maquina
                            objNewRegistroDuplicar.Estado = ESTADO_INGRESADA
                            objNewRegistroDuplicar.NombreEstado = "Ingresada"
                            objNewRegistroDuplicar.Referencia = String.Empty
                            objNewRegistroDuplicar.logOperacionAplazada = True

                            logCancelarRegistro = False
                            logEditarRegistro = True
                            logDuplicarRegistro = True
                            logNuevoRegistro = False

                            TipoProductoFiltroCliente = "TOD"

                            ObtenerValoresRegistroAnterior(objNewRegistroDuplicar, EncabezadoSeleccionadoDuplicar)

                            Await CargarTipoNegocioUsuario(OPCION_APLAZAR, OPCION_APLAZAR)

                            BuscarControlValidacion(viewFormaOperacionesOtrosNegocios, "btnDuplicar")

                            strTipoCalculo = TIPOCALCULOS_MOTOR.PRECIO.ToString
                        End If
                    Else
                        If pstrAccion = OPCION_EDITAR Then
                            MyBase.RetornarValorEdicionNavegacion()
                        End If
                        'IsBusy = False
                    End If
                    IsBusy = False
                    HabilitaBoton = True
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el estado del registro.", Me.ToString(), "validarEstadoRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Function

    Public Overrides Sub CancelarEditarRegistro()
        Try
            If logEsModal Then
                RaiseEvent TerminoGuardarRegistro(False, 0, String.Empty, String.Empty, String.Empty)
                Editando = False
            Else
                logCancelarRegistro = True
                logEditarRegistro = False
                logDuplicarRegistro = False
                logNuevoRegistro = False
                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)

                If BorrarEspecie = True Then
                    BorrarEspecie = False
                End If

                If BorrarCliente = True Then
                    BorrarCliente = False
                End If

                BorrarCliente = True
                BorrarEspecie = True

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

                HabilitaBoton = True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
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

            objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosAnularSyncQuery(_EncabezadoSeleccionado.ID, pstrObservaciones, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

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

    Private Async Sub AplicarLiqxDife(ByVal pstrObservaciones As String)

        Try
            Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_RespuestaValidacion)

            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Operaciones_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosLiqxDifeSyncQuery(_EncabezadoSeleccionado.ID, pstrObservaciones, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al aplicar liquidación por diferencia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al aplicar liquidación por diferencia.", Me.ToString(), "LiqxDife", Program.TituloSistema, Program.Maquina, objRet.Error)
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
                        Dim strMensajeExitoso As String = "El registro aplicó correctamente liquidación por diferencia."
                        Dim strMensajeError As String = "El registro no se pudo actualizar."
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

                            mostrarMensaje(strMensajeExitoso, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al aplicar liquidación por diferencia.", Me.ToString(), "LiqxDife", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            HabilitaBoton = True
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOperaciones"
                    Dim NewReceptoresOperacion As New CFOperaciones.Operaciones_ReceptoresOtrosNegocios

                    NewReceptoresOperacion.IDLiquidacion = _EncabezadoSeleccionado.ID
                    NewReceptoresOperacion.Actualizacion = Now
                    NewReceptoresOperacion.Usuario = Program.Usuario
                    NewReceptoresOperacion.Lider = False
                    NewReceptoresOperacion.Porcentaje = 0
                    NewReceptoresOperacion.NombreReceptor = ""
                    NewReceptoresOperacion.Receptor = String.Empty

                    Dim objListaReceptor As New List(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)
                    If Not IsNothing(_ListaReceptores) Then
                        For Each li In _ListaReceptores
                            objListaReceptor.Add(li)
                        Next
                    End If

                    objListaReceptor.Add(NewReceptoresOperacion)

                    ListaReceptores = objListaReceptor
                    ReceptorSeleccionado = NewReceptoresOperacion

                    MyBase.CambioItem("ListaReceptores")
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo detalle.", Me.ToString(), "NuevoRegistroDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOperaciones"
                    If Not IsNothing(_ListaReceptores) Then
                        If Not _ReceptorSeleccionado Is Nothing Then
                            Dim objListaReceptores As New List(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptorSeleccionado, ListaReceptores)

                            For Each li In _ListaReceptores
                                objListaReceptores.Add(li)
                            Next

                            If objListaReceptores.Contains(_ReceptorSeleccionado) Then
                                objListaReceptores.Remove(_ReceptorSeleccionado)
                            End If

                            ReceptorSeleccionado = Nothing
                            ListaReceptores = objListaReceptores
                            If _ListaReceptores.Count > 0 Then
                                Program.PosicionarItemLista(ReceptorSeleccionado, ListaReceptores, intRegistroPosicionar)
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
                ViewOperacionesOtrosNegocios.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
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
            Dim objBusqueda As New CamposBusquedaOperacionesOtrosNegocios
            objBusqueda.ID = 0
            objBusqueda.Estado = String.Empty
            objBusqueda.Referencia = String.Empty
            objBusqueda.TipoNegocio = String.Empty
            objBusqueda.TipoOperacion = String.Empty
            objBusqueda.TipoOrigen = String.Empty
            objBusqueda.TipoRegistro = String.Empty
            objBusqueda.Cliente = String.Empty
            objBusqueda.Especie = String.Empty
            objBusqueda.FechaOperacion = Nothing
            objBusqueda.FechaCumplimiento = Nothing

            BusquedaOperacionesOtrosNegocios = objBusqueda

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

            objRet = Await dcProxy.Load(dcProxy.Operaciones_ConsultarCombosSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

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
                                If pstrUserState = OPCION_DUPLICAR Then
                                    ObtenerValoresCombos(False, EncabezadoSeleccionadoDuplicar, pstrUserState)
                                ElseIf pstrUserState = OPCION_APLAZAR
                                    ObtenerValoresCombos(False, EncabezadoSeleccionadoDuplicar, pstrUserState)
                                Else
                                    ObtenerValoresCombos(False, _EncabezadoSeleccionado, pstrUserState)
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

    Public Async Function CargarTipoNegocioUsuario(ByVal pstrOpcion As String, Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_TiposNegocio)
            Dim objProxy = inicializarProxyOperacionesOtrosNegocios()
            If logNuevoRegistro Or logEditarRegistro Then
                IsBusy = True
            End If

            If Not IsNothing(pstrUserState) Then
                pstrUserState = pstrUserState.ToUpper
            End If

            ErrorForma = String.Empty

            If Not IsNothing(objProxy.Operaciones_TiposNegocios) Then
                objProxy.Operaciones_TiposNegocios.Clear()
            End If

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                objRet = Await objProxy.Load(objProxy.Operaciones_ConsultarTiposNegocioSyncQuery(Program.CifrarTexto(Program.Aplicacion), Program.CifrarTexto(Program.VersionAplicacion), Program.CifrarTexto(Program.UsuarioWindows), String.Empty, Program.CifrarTexto(Program.Usuario), Program.CifrarTexto(Program.Maquina), Program.HashConexion)).AsTask()
            Else
                objRet = Await objProxy.Load(objProxy.Operaciones_ConsultarTiposNegocioSyncQuery(Program.CifrarTexto(Program.Aplicacion), Program.CifrarTexto(Program.VersionAplicacion), Program.CifrarTexto(Program.DatosUsuario_Usuario), Program.CifrarTexto(Program.DatosUsuario_PasswordUsuario), Program.CifrarTexto(Program.Usuario), Program.CifrarTexto(Program.Maquina), Program.HashConexion)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consultar los tipos de negocio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar los tipos de negocio.", Me.ToString(), "CargarTipoNegocioUsuario", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                    IsBusy = False
                Else
                    Dim logContinuar As Boolean = True
                    Dim objTipoNegocio As New List(Of CFOperaciones.Operaciones_TiposNegocio)
                    Dim objTipoOrigen As New List(Of CFOperaciones.Operaciones_TiposNegocio)
                    Dim objTipoOrigenNuevo As New List(Of CFOperaciones.Operaciones_TiposNegocio)
                    Dim intContador As Integer = 0

                    If objRet.Entities.Where(Function(i) i.Tipo = TIPOPERMISO_NEGOCIO).Count > 0 Then
                        If pstrOpcion = OPCION_EDITAR Or pstrOpcion = OPCION_DUPLICAR Or pstrOpcion = OPCION_ACTUALIZAR Or pstrOpcion = OPCION_ANULAR Or pstrOpcion = OPCION_APLAZAR Then
                            If objRet.Entities.ToList.Where(Function(i) i.Tipo = TIPOPERMISO_NEGOCIO And i.CodigoTipoNegocio = _EncabezadoSeleccionado.TipoNegocio).Count = 0 Then
                                mostrarMensaje("Señor usuario usted no tiene configurado el tipo de negocio del registro seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logContinuar = False
                                IsBusy = False
                            End If
                        End If

                        If logContinuar Then
                            objTipoNegocio = objRet.Entities.Where(Function(i) i.Tipo = TIPOPERMISO_NEGOCIO).ToList
                        End If
                    Else
                        If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_TIPONEGOCIO") Then
                            intContador = 1
                            For Each liCombo In DiccionarioCombosCompleta("NEGOCIOS_TIPONEGOCIO")
                                objTipoNegocio.Add(New CFOperaciones.Operaciones_TiposNegocio With {.ID = intContador,
                                                                                                    .CodigoTipoNegocio = liCombo.Codigo,
                                                                                                    .NombreTipoNegocio = liCombo.Descripcion})
                                intContador += 1
                            Next
                        End If
                    End If

                    If objRet.Entities.Where(Function(i) i.Tipo = TIPOPERMISO_ORIGEN).Count > 0 Then
                        If pstrOpcion = OPCION_EDITAR Or pstrOpcion = OPCION_DUPLICAR Or pstrOpcion = OPCION_ACTUALIZAR Or pstrOpcion = OPCION_ANULAR Or pstrOpcion = OPCION_APLAZAR Then
                            If objRet.Entities.ToList.Where(Function(i) i.Tipo = TIPOPERMISO_ORIGEN And i.CodigoTipoNegocio = _EncabezadoSeleccionado.TipoOrigen).Count = 0 Then
                                mostrarMensaje("Señor usuario usted no tiene configurado el tipo de origen del registro seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logContinuar = False
                                IsBusy = False
                            End If
                        End If

                        If logContinuar Then
                            objTipoOrigen = objRet.Entities.Where(Function(i) i.Tipo = TIPOPERMISO_ORIGEN).ToList
                        End If
                    Else
                        If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_ORIGEN") Then
                            intContador = 1
                            For Each liCombo In DiccionarioCombosCompleta("NEGOCIOS_ORIGEN")
                                objTipoOrigen.Add(New CFOperaciones.Operaciones_TiposNegocio With {.ID = intContador,
                                                                                                    .CodigoTipoNegocio = liCombo.Codigo,
                                                                                                    .NombreTipoNegocio = liCombo.Descripcion})
                                intContador += 1
                            Next
                        End If
                    End If


                    If logContinuar Then
                        If pstrOpcion = OPCION_EDITAR Then
                            Await CargarCombos(False, OPCION_EDITAR)
                        ElseIf pstrOpcion = OPCION_DUPLICAR Then
                            ListaTipoNegocio = objTipoNegocio

                            intContador = 1
                            For Each liCombo In objTipoOrigen
                                If ValidarTipoOrigenXTipoNegocio(EncabezadoSeleccionadoDuplicar.TipoNegocio, liCombo.CodigoTipoNegocio) Then
                                    objTipoOrigenNuevo.Add(New CFOperaciones.Operaciones_TiposNegocio With {.ID = intContador,
                                                                                                            .CodigoTipoNegocio = liCombo.CodigoTipoNegocio,
                                                                                                            .NombreTipoNegocio = liCombo.NombreTipoNegocio})
                                    intContador += 1
                                End If
                            Next

                            ListaTipoOrigen = objTipoOrigenNuevo

                            Await CargarCombos(False, OPCION_DUPLICAR)
                        ElseIf pstrOpcion = OPCION_ACTUALIZAR Then
                            Await GuardarOperaciones_OtrosNegocios(_EncabezadoSeleccionado)
                        ElseIf pstrOpcion = OPCION_NUEVO Then
                            ListaTipoNegocio = objTipoNegocio
                            ListaTipoOrigenPermisos = objTipoOrigen
                            ListaTipoOrigen = Nothing

                            NuevaOperacion()
                        ElseIf pstrOpcion = OPCION_ANULAR Then
                            mostrarMensajePregunta("Comentario para la anulación del registro",
                                       Program.TituloSistema,
                                       "ANULARREGISTRO",
                                       AddressOf TerminoMensajePregunta,
                                       True,
                                       "¿Anular la registro?", False, True, True, False)
                        ElseIf pstrOpcion = OPCION_APLAZAR Then
                            ListaTipoNegocio = objTipoNegocio

                            intContador = 1
                            For Each liCombo In objTipoOrigen
                                If ValidarTipoOrigenXTipoNegocio(EncabezadoSeleccionadoDuplicar.TipoNegocio, liCombo.CodigoTipoNegocio) Then
                                    objTipoOrigenNuevo.Add(New CFOperaciones.Operaciones_TiposNegocio With {.ID = intContador,
                                                                                                            .CodigoTipoNegocio = liCombo.CodigoTipoNegocio,
                                                                                                            .NombreTipoNegocio = liCombo.NombreTipoNegocio})
                                    intContador += 1
                                End If
                            Next

                            ListaTipoOrigen = objTipoOrigenNuevo

                            Await CargarCombos(False, OPCION_APLAZAR)
                        End If
                    End If
                End If
            Else
                IsBusy = False
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar los tipos de negocio.", Me.ToString(), "CargarTipoNegocioUsuario", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Private Function ValidarTipoOrigenXTipoNegocio(ByVal pstrCodigoTipoNegocio As String, ByVal pstrCodigoTipoOrigen As String) As Boolean
        Try
            Dim logEsTipoOrigenValido As Boolean = True

            If DiccionarioCombosCompleta.ContainsKey("TIPONEGOCIOXTIPOORIGEN") Then
                If DiccionarioCombosCompleta("TIPONEGOCIOXTIPOORIGEN").Where(Function(i) i.Codigo = pstrCodigoTipoNegocio).Count > 0 Then
                    If DiccionarioCombosCompleta("TIPONEGOCIOXTIPOORIGEN").Where(Function(i) i.Codigo = pstrCodigoTipoNegocio And i.Descripcion = pstrCodigoTipoOrigen).Count > 0 Then
                        logEsTipoOrigenValido = True
                    Else
                        logEsTipoOrigenValido = False
                    End If
                ElseIf DiccionarioCombosCompleta("TIPONEGOCIOXTIPOORIGEN").Where(Function(i) i.Descripcion = pstrCodigoTipoOrigen).Count > 0 Then
                    logEsTipoOrigenValido = False
                End If
            End If

            Return logEsTipoOrigenValido
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar el tipo de origen.",
                                 Me.ToString(), "ValidarTipoOrigenXTipoNegocio", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información del cliente.",
                                 Me.ToString(), "BuscarCliente", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub BuscarEspecie(ByVal pstrMercado As String, ByVal pstrEspecie As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not String.IsNullOrEmpty(pstrEspecie) Then
                If Not IsNothing(mdcProxyUtilidad.BuscadorEspecies) Then
                    mdcProxyUtilidad.BuscadorEspecies.Clear()
                End If

                mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarNemotecnicoEspecificoQuery(pstrMercado, pstrEspecie, Program.Usuario, Program.HashConexion), AddressOf buscarEspecieCompleted, pstrUserState)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información de la especie.",
                                 Me.ToString(), "BuscarEspecie", Application.Current.ToString(), Program.Maquina, ex)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar la información del cliente.",
                                 Me.ToString(), "SeleccionarCliente", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub VerificarValoresEnCombos()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                'If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoOrden) AndAlso Not ValidarCampoEnDiccionario("TIPOORDEN", _EncabezadoSeleccionado.TipoOrden) Then
                '    _EncabezadoSeleccionado.TipoOrden = String.Empty
                '    _EncabezadoSeleccionado.NombreTipoOrden = String.Empty
                'End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los valores de los combos.",
                                 Me.ToString(), "VerificarValoresEnCombos", Application.Current.ToString(), Program.Maquina, ex)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valor por defecto del combo.",
                                Me.ToString(), "AsignarValorTopicoCategoria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return objRetorno
    End Function

    Public Async Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean, ByVal pobjRegistroSelected As CFOperaciones.Operaciones_OtrosNegocios, Optional ByVal Opcion As String = "")
        Try
            Dim objDiccionario As New Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos))
            Dim objListaCategoria As New List(Of CFOperaciones.Operaciones_Combos)
            Dim objListaCategoria1 As New List(Of CFOperaciones.Operaciones_Combos)
            Dim objListaCategoria2 As New List(Of CFOperaciones.Operaciones_Combos)
            Dim objListaCategoria3 As New List(Of CFOperaciones.Operaciones_Combos)

            If Not IsNothing(DiccionarioCombos) Then
                For Each li In DiccionarioCombos
                    objDiccionario.Add(li.Key, li.Value)
                Next
            End If

            If Not IsNothing(objDiccionario) Then
                If ValoresCompletos Then 'Cuando ValoresCompletos = True
                    Call PrRemoverValoresDic(objDiccionario, {"NEGOCIOS_CLASEINVERSION", "NEGOCIOS_TIPOOPERACION"})

                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    'Valores para el combo de clasificación inversión
                    '************************************************************************************
                    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERC") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERC", "NEGOCIOS_CLASEINVERSION"))
                    'If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR_ENTREGADA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR_ENTREGADA", "NEGOCIOS_CLASEINVERSION"))
                    'If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR_RECIBIDA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR_RECIBIDA", "NEGOCIOS_CLASEINVERSION"))

                    If objListaCategoria.Count > 0 Then objDiccionario.Add("NEGOCIOS_CLASEINVERSION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)
                    '************************************************************************************
                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_GENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_GENERAL", "NEGOCIOS_TIPOOPERACION"))
                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_REPOREGRESO") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_REPOREGRESO", "NEGOCIOS_TIPOOPERACION"))

                    If objListaCategoria.Count > 0 Then objDiccionario.Add("NEGOCIOS_TIPOOPERACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                    If Not IsNothing(objDiccionario) Then

                        If objDiccionario.ContainsKey("MOTORCALCULOS_RUTASERVICIO") Then
                            If objDiccionario("MOTORCALCULOS_RUTASERVICIO").Count > 0 Then
                                STR_URLMOTORCALCULOS = objDiccionario("MOTORCALCULOS_RUTASERVICIO").First.Codigo
                            End If
                        End If

                        If objDiccionario.ContainsKey("MOTORCALCULOS_HACERLOGMOTOR") Then
                            If objDiccionario("MOTORCALCULOS_HACERLOGMOTOR").Count > 0 Then
                                If objDiccionario("MOTORCALCULOS_HACERLOGMOTOR").First.Codigo = "SI" Then
                                    LOG_HACERLOGMOTORCALCULOS = True
                                End If
                            End If
                        End If

                        If objDiccionario.ContainsKey("MOTORCALCULOS_RUTALOGMOTOR") Then
                            If objDiccionario("MOTORCALCULOS_RUTALOGMOTOR").Count > 0 Then
                                STR_RUTALOGMOTORCALCULOS = objDiccionario("MOTORCALCULOS_RUTALOGMOTOR").First.Codigo
                            End If
                        End If

                        If objDiccionario.ContainsKey("NEGOCIOS_PAISLOCAL") Then
                            If objDiccionario("NEGOCIOS_PAISLOCAL").Count > 0 Then
                                intPaisLocal = objDiccionario("NEGOCIOS_PAISLOCAL").First.ID
                            End If
                        End If

                        If objDiccionario.ContainsKey("NEGOCIOS_MONEDALOCAL") Then
                            If objDiccionario("NEGOCIOS_MONEDALOCAL").Count > 0 Then
                                intMonedaLocal = objDiccionario("NEGOCIOS_MONEDALOCAL").First.ID
                            End If
                        End If

                    End If

                    DiccionarioCombos = objDiccionario

                Else ' Cuando ValoresCompletos = False
                    If Not String.IsNullOrEmpty(Opcion) Then
                        Dim OpcionValoresDefecto As String = String.Empty

                        Select Case Opcion.ToUpper
                            Case OPCION_TIPONEGOCIO, OPCION_CLASIFICACIONINVERSION
                                If Opcion.ToUpper = OPCION_TIPONEGOCIO Then
                                    Call PrRemoverValoresDic(objDiccionario, {"NEGOCIOS_TIPOOPERACION"})

                                    If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                                    pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or
                                    pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                                        If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_GENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_GENERAL", "NEGOCIOS_TIPOOPERACION"))
                                    ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Or pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                                        If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_REPOSELECCION") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_REPOSELECCION", "NEGOCIOS_TIPOOPERACION"))
                                    ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                                        If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_TTVSELECCION") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_TTVSELECCION", "NEGOCIOS_TIPOOPERACION"))
                                    ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                                        If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_DEPOSITOREMUNERADO") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_DEPOSITOREMUNERADO", "NEGOCIOS_TIPOOPERACION"))
                                    End If

                                    If objListaCategoria.Count > 0 Then objDiccionario.Add("NEGOCIOS_TIPOOPERACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                                    Dim intContador As Integer = 1
                                    Dim objTipoOrigenNuevo As New List(Of CFOperaciones.Operaciones_TiposNegocio)

                                    For Each liCombo In ListaTipoOrigenPermisos
                                        If ValidarTipoOrigenXTipoNegocio(_EncabezadoSeleccionado.TipoNegocio, liCombo.CodigoTipoNegocio) Then
                                            objTipoOrigenNuevo.Add(New CFOperaciones.Operaciones_TiposNegocio With {.ID = intContador,
                                                                                                                    .CodigoTipoNegocio = liCombo.CodigoTipoNegocio,
                                                                                                                    .NombreTipoNegocio = liCombo.NombreTipoNegocio})
                                            intContador += 1
                                        End If
                                    Next

                                    ListaTipoOrigen = objTipoOrigenNuevo

                                    If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()
                                End If

                                Call PrRemoverValoresDic(objDiccionario, {"NEGOCIOS_CLASEINVERSION"})

                                If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                                    pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERC") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERC", "NEGOCIOS_CLASEINVERSION"))
                                ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Or pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR", "NEGOCIOS_CLASEINVERSION"))

                                    'If pobjRegistroSelected.TipoOperacion = TIPOOPERACION_COMPRA Or pobjRegistroSelected.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                                    '    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR_ENTREGADA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR_RECIBIDA", "NEGOCIOS_CLASEINVERSION"))
                                    'ElseIf pobjRegistroSelected.TipoOperacion = TIPOOPERACION_VENTA Or pobjRegistroSelected.TipoOperacion = TIPOOPERACION_REVENTA Then
                                    '    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR_ENTREGADA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR_ENTREGADA", "NEGOCIOS_CLASEINVERSION"))
                                    'End If
                                End If

                                If objListaCategoria.Count > 0 Then objDiccionario.Add("NEGOCIOS_CLASEINVERSION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                                If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()
                            Case OPCION_EDITAR
                                Call PrRemoverValoresDic(objDiccionario, {"NEGOCIOS_CLASEINVERSION", "NEGOCIOS_TIPOOPERACION"})

                                If EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                                    EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERC") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERC", "NEGOCIOS_CLASEINVERSION"))
                                ElseIf EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_REPO Or EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_TTV Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR", "NEGOCIOS_CLASEINVERSION"))

                                    'If EncabezadoSeleccionadoAnterior.TipoOperacion = TIPOOPERACION_COMPRA Or EncabezadoSeleccionadoAnterior.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                                    '    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR_ENTREGADA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR_RECIBIDA", "NEGOCIOS_CLASEINVERSION"))
                                    'ElseIf EncabezadoSeleccionadoAnterior.TipoOperacion = TIPOOPERACION_VENTA Or EncabezadoSeleccionadoAnterior.TipoOperacion = TIPOOPERACION_REVENTA Then
                                    '    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR_ENTREGADA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR_ENTREGADA", "NEGOCIOS_CLASEINVERSION"))
                                    'End If
                                End If

                                If objListaCategoria.Count > 0 Then objDiccionario.Add("NEGOCIOS_CLASEINVERSION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                                If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                                If EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                                    EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or
                                    EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_GENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_GENERAL", "NEGOCIOS_TIPOOPERACION"))
                                ElseIf EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_REPO Or EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_TTV Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_REPOSELECCION") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_REPOSELECCION", "NEGOCIOS_TIPOOPERACION"))
                                ElseIf EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_TTV Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_TTVSELECCION") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_TTVELECCION", "NEGOCIOS_TIPOOPERACION"))
                                ElseIf EncabezadoSeleccionadoAnterior.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_DEPOSITOREMUNERADO") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_DEPOSITOREMUNERADO", "NEGOCIOS_TIPOOPERACION"))
                                End If

                                If objListaCategoria.Count > 0 Then objDiccionario.Add("NEGOCIOS_TIPOOPERACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                                If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()
                            Case OPCION_DUPLICAR, OPCION_APLAZAR
                                Call PrRemoverValoresDic(objDiccionario, {"NEGOCIOS_CLASEINVERSION", "NEGOCIOS_TIPOOPERACION"})

                                If EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                                    EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERC") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERC", "NEGOCIOS_CLASEINVERSION"))
                                ElseIf EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_REPO Or EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_TTV Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR", "NEGOCIOS_CLASEINVERSION"))

                                    'If EncabezadoSeleccionadoDuplicar.TipoOperacion = TIPOOPERACION_COMPRA Or EncabezadoSeleccionadoDuplicar.TipoOperacion = TIPOOPERACION_RECOMPRA Then
                                    '    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR_ENTREGADA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR_RECIBIDA", "NEGOCIOS_CLASEINVERSION"))
                                    'ElseIf EncabezadoSeleccionadoDuplicar.TipoOperacion = TIPOOPERACION_VENTA Or EncabezadoSeleccionadoDuplicar.TipoOperacion = TIPOOPERACION_REVENTA Then
                                    '    If objDiccionario.ContainsKey("NEGOCIOS_CLASEINVERR_ENTREGADA") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_CLASEINVERR_ENTREGADA", "NEGOCIOS_CLASEINVERSION"))
                                    'End If
                                End If

                                If objListaCategoria.Count > 0 Then objDiccionario.Add("NEGOCIOS_CLASEINVERSION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                                If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()

                                If EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                                    EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or
                                    EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_GENERAL") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_GENERAL", "NEGOCIOS_TIPOOPERACION"))
                                ElseIf EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_REPO Or EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_REPOSELECCION") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_REPOSELECCION", "NEGOCIOS_TIPOOPERACION"))
                                ElseIf EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_TTV Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_TTVSELECCION") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_TTVSELECCION", "NEGOCIOS_TIPOOPERACION"))
                                ElseIf EncabezadoSeleccionadoDuplicar.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                                    If objDiccionario.ContainsKey("NEGOCIOS_TIPOOPERACION_DEPOSITOREMUNERADO") Then objListaCategoria.InsertRange(0, ExtraerListaPorCategoria(objDiccionario, "NEGOCIOS_TIPOOPERACION_DEPOSITOREMUNERADO", "NEGOCIOS_TIPOOPERACION"))
                                End If

                                If objListaCategoria.Count > 0 Then objDiccionario.Add("NEGOCIOS_TIPOOPERACION", objListaCategoria.OrderBy(Function(i) i.Descripcion).ToList)

                                If Not IsNothing(objListaCategoria) Then objListaCategoria.Clear()
                        End Select

                        DiccionarioCombos = objDiccionario

                        If Opcion.ToUpper = OPCION_EDITAR Then
                            'Se llevan los anteriores registro ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.
                            logCambiarValoresEnSelected = False
                            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoAnterior, EncabezadoSeleccionado)
                            logCambiarValoresEnSelected = True

                            Editando = True
                            MyBase.CambioItem("Editando")

                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)

                            consultarOrdenantes(_EncabezadoSeleccionado.IDComitente, OPCION_EDITAR)
                            consultarCuentasDeposito(_EncabezadoSeleccionado.IDComitente, OPCION_EDITAR)
                            consultarCuentasDepositoEntidad(_EncabezadoSeleccionado.IDContraparte, OPCION_EDITAR)
                            'SE REALIZA EL CAMBIO PARA QUE EL MANEJO DE LA CONTRAPARTE SEA POR MEDIO DE LA CONFIGURACIÓN DE LA TABLA CF.tblConfiguracionTipoOrigen
                            If VerificarTipoFuncionalidadTipoOrigen(_EncabezadoSeleccionado.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACONTRAPARTE) Then
                                consultarComitentesContraparte(EncabezadoSeleccionado.IDContraparte, OPCION_EDITAR)
                            End If
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)

                            'Se posiciona en el primer registro
                            BuscarControlValidacion(viewFormaOperacionesOtrosNegocios, "dtmFechaCumplimiento")

                            OpcionValoresDefecto = OPCION_EDITAR

                            CambiarColorFondoTextoBuscador()

                            IsBusy = False

                            ObtenerValoresDefecto(OpcionValoresDefecto, _EncabezadoSeleccionado)
                        ElseIf Opcion.ToUpper = OPCION_DUPLICAR Or Opcion.ToUpper = OPCION_APLAZAR Then
                            'Se llevan los anteriores a la orden ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.
                            logCambiarValoresEnSelected = False
                            ObtenerValoresRegistroAnterior(EncabezadoSeleccionadoDuplicar, EncabezadoSeleccionado)
                            logCambiarValoresEnSelected = True

                            If Opcion.ToUpper = OPCION_DUPLICAR Then
                                Editando = True
                                MyBase.CambioItem("Editando")
                            End If

                            'Se llevan los anteriores a la orden ya que al momendo de actualizar los combos se pierde los valores del seleccionado de los combos.
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, True)

                            If Opcion.ToUpper = OPCION_APLAZAR Then
                                Await consultarOrdenantesAsincronico(_EncabezadoSeleccionado.IDComitente, OPCION_APLAZAR)
                                Await consultarCuentasDepositoAsincronico(_EncabezadoSeleccionado.IDComitente, OPCION_APLAZAR)
                                Await consultarCuentasDepositoEntidadAsincronico(_EncabezadoSeleccionado.IDContraparte, OPCION_APLAZAR)
                            Else
                                consultarOrdenantes(_EncabezadoSeleccionado.IDComitente, OPCION_EDITAR)
                                consultarCuentasDeposito(_EncabezadoSeleccionado.IDComitente, OPCION_EDITAR)
                                consultarCuentasDepositoEntidad(_EncabezadoSeleccionado.IDContraparte, OPCION_EDITAR)
                            End If

                            'SE REALIZA EL CAMBIO PARA QUE EL MANEJO DE LA CONTRAPARTE SEA POR MEDIO DE LA CONFIGURACIÓN DE LA TABLA CF.tblConfiguracionTipoOrigen
                            If VerificarTipoFuncionalidadTipoOrigen(_EncabezadoSeleccionado.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACONTRAPARTE) Then
                                consultarComitentesContraparte(EncabezadoSeleccionado.IDContraparte, OPCION_EDITAR)
                            End If
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)

                            'Se posiciona en el primer registro
                            BuscarControlValidacion(viewFormaOperacionesOtrosNegocios, "")

                            OpcionValoresDefecto = Opcion.ToUpper

                            CambiarColorFondoTextoBuscador()

                            IsBusy = False

                            ObtenerValoresDefecto(OpcionValoresDefecto, _EncabezadoSeleccionado)

                            If Opcion.ToUpper = OPCION_APLAZAR Then
                                Await GuardarOperaciones_OtrosNegocios(EncabezadoSeleccionado)
                            End If
                        Else
                            OpcionValoresDefecto = Opcion.ToUpper

                            ObtenerValoresDefecto(OpcionValoresDefecto, pobjRegistroSelected)
                        End If
                    End If

                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto de los combos.",
                                 Me.ToString(), "ObtenerValoresCombos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Async Sub ObtenerValoresDefecto(ByVal pstrOpcion As String, ByVal objRegistroSelected As CFOperaciones.Operaciones_OtrosNegocios)
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                logCalcularValores = False

                Select Case pstrOpcion.ToUpper
                    Case OPCION_TIPONEGOCIO
                        If ListaTipoOrigen.Count = 1 Then
                            objRegistroSelected.TipoOrigen = ListaTipoOrigen.First.CodigoTipoNegocio
                        End If

                        objRegistroSelected.TipoOperacion = AsignarValorTopicoCategoria("NEGOCIOS_TIPOOPERACION", String.Empty)

                        If objRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                            objRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                            Dim strClasificacionInversionDefecto As String = String.Empty

                            If Not IsNothing(DiccionarioCombosCompleta) Then
                                If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_CLASEINVERC_DEFECTO") Then
                                    If DiccionarioCombosCompleta("NEGOCIOS_CLASEINVERC_DEFECTO").Count > 0 Then
                                        strClasificacionInversionDefecto = DiccionarioCombosCompleta("NEGOCIOS_CLASEINVERC_DEFECTO").First.Codigo
                                    End If
                                End If
                            End If

                            objRegistroSelected.ClasificacionInversion = AsignarValorTopicoCategoria("NEGOCIOS_CLASEINVERSION", strClasificacionInversionDefecto)
                        ElseIf objRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Or objRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or objRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                            objRegistroSelected.ClasificacionInversion = AsignarValorTopicoCategoria("NEGOCIOS_CLASEINVERSION", String.Empty)
                        End If

                        If objRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                            objRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                            objRegistroSelected.TipoCumplimiento = AsignarValorTopicoCategoria("NEGOCIOS_TIPOCUMP", String.Empty)
                            objRegistroSelected.Mercado = AsignarValorTopicoCategoria("NEGOCIOS_MERCADO", String.Empty)
                        End If

                        If objRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                            objRegistroSelected.Tipo = AsignarValorTopicoCategoria("NEGOCIOS_TIPO", CLASE_ACCIONES)
                        ElseIf objRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                            objRegistroSelected.Tipo = AsignarValorTopicoCategoria("NEGOCIOS_TIPO", CLASE_RENTAFIJA)
                        ElseIf objRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                            objRegistroSelected.Tipo = AsignarValorTopicoCategoria("NEGOCIOS_TIPO", CLASE_RENTAFIJA)
                        ElseIf objRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                            objRegistroSelected.Tipo = AsignarValorTopicoCategoria("NEGOCIOS_TIPO", CLASE_ACCIONES)
                        ElseIf objRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                            objRegistroSelected.Tipo = AsignarValorTopicoCategoria("NEGOCIOS_TIPO", CLASE_ACCIONES)
                            objRegistroSelected.TipoRepo = AsignarValorTopicoCategoria("NEGOCIOS_TIPOREPO", TIPOREPO_CERRADO)
                            objRegistroSelected.TipoBanRep = False
                        End If

                        LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_TIPO)
                        ObtenerClaseEspecies()

                        objRegistroSelected.TipoReteFuente = AsignarValorTopicoCategoria("NEGOCIOS_RETEFUENTE", String.Empty)

                        If Not IsNothing(objEntidadPorDefecto) Then
                            objRegistroSelected.IDPagador = objEntidadPorDefecto.IDEntidad
                            objRegistroSelected.NumeroDocumentoPagador = objEntidadPorDefecto.NroDocumento
                            objRegistroSelected.CodTipoIdentificacionPagador = objEntidadPorDefecto.TipoIdentificacion
                            objRegistroSelected.TipoIdentificacionPagador = objEntidadPorDefecto.NombreTipoIdentificacion
                        End If

                        If objRegistroSelected.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                            If DiccionarioCombos.ContainsKey("NEGOCIOS_PAISLOCAL") Then
                                objRegistroSelected.PaisNegociacion = DiccionarioCombos("NEGOCIOS_PAISLOCAL").First.ID
                                objRegistroSelected.NombrePaisNegociacion = DiccionarioCombos("NEGOCIOS_PAISLOCAL").First.Descripcion
                                objRegistroSelected.CodigoPaisNegociacion = DiccionarioCombos("NEGOCIOS_PAISLOCAL").First.Codigo
                            End If

                            If DiccionarioCombos.ContainsKey("NEGOCIOS_MONEDALOCAL") Then
                                logPosicionarCantidad = False
                                objRegistroSelected.IDMoneda = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.ID

                                objRegistroSelected.NombreMoneda = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.Descripcion
                                objRegistroSelected.CodigoMoneda = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.Codigo
                            End If
                        Else
                            If DiccionarioCombos.ContainsKey("NEGOCIOS_PAISDEFECTO") Then
                                objRegistroSelected.PaisNegociacion = DiccionarioCombos("NEGOCIOS_PAISDEFECTO").First.ID
                                objRegistroSelected.NombrePaisNegociacion = DiccionarioCombos("NEGOCIOS_PAISDEFECTO").First.Descripcion
                                objRegistroSelected.CodigoPaisNegociacion = DiccionarioCombos("NEGOCIOS_PAISDEFECTO").First.Codigo
                            End If
                        End If

                        HabilitarCamposRegistro(objRegistroSelected)
                        Await ObtenerTRMMoneda()

                        IsBusy = False
                    Case OPCION_CLASIFICACIONINVERSION
                        If objRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                           objRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or
                           objRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Or
                           objRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or
                           objRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                            objRegistroSelected.ClasificacionInversion = AsignarValorTopicoCategoria("NEGOCIOS_CLASEINVERSION", String.Empty)
                        End If

                    Case OPCION_TIPOORIGEN
                        'SE REALIZA EL CAMBIO PARA QUE EL MANEJO DE LA CONTRAPARTE SEA POR MEDIO DE LA CONFIGURACIÓN DE LA TABLA CF.tblConfiguracionTipoOrigen
                        Dim logManejaContraparteTipoOrigen As Boolean = False

                        If VerificarTipoFuncionalidadTipoOrigen(objRegistroSelected.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACONTRAPARTE) Then
                            logManejaContraparteTipoOrigen = True
                        End If

                        If logManejaContraparteTipoOrigen Then
                            If Not IsNothing(DiccionarioCombos) Then
                                If DiccionarioCombos.ContainsKey("NEGOCIOS_CONTRAPARTEDEFECTOORIGENOTC") Then
                                    If DiccionarioCombos("NEGOCIOS_CONTRAPARTEDEFECTOORIGENOTC").Count > 0 Then
                                        objRegistroSelected.IDContraparte = DiccionarioCombos("NEGOCIOS_CONTRAPARTEDEFECTOORIGENOTC").First.ID
                                        objRegistroSelected.NombreContraparte = DiccionarioCombos("NEGOCIOS_CONTRAPARTEDEFECTOORIGENOTC").First.Descripcion
                                        objRegistroSelected.NumeroDocumentoContraparte = DiccionarioCombos("NEGOCIOS_CONTRAPARTEDEFECTOORIGENOTC").First.Codigo
                                        objRegistroSelected.CodTipoIdentificacionContraparte = "N"
                                        objRegistroSelected.TipoIdentificacionContraparte = "Nit"
                                    End If
                                End If

                                If DiccionarioCombos.ContainsKey("NEGOCIOS_PAGADORDEFECTOORIGENOTC") Then
                                    If DiccionarioCombos("NEGOCIOS_PAGADORDEFECTOORIGENOTC").Count > 0 Then
                                        objRegistroSelected.IDPagador = DiccionarioCombos("NEGOCIOS_PAGADORDEFECTOORIGENOTC").First.ID
                                        objRegistroSelected.NombrePagador = DiccionarioCombos("NEGOCIOS_PAGADORDEFECTOORIGENOTC").First.Descripcion
                                        objRegistroSelected.NumeroDocumentoPagador = DiccionarioCombos("NEGOCIOS_PAGADORDEFECTOORIGENOTC").First.Codigo
                                        objRegistroSelected.CodTipoIdentificacionPagador = "N"
                                        objRegistroSelected.TipoIdentificacionPagador = "Nit"
                                    End If
                                End If
                            End If

                            consultarComitentesContraparte(objRegistroSelected.IDContraparte)
                        Else
                            objRegistroSelected.IDContraparte = Nothing
                            objRegistroSelected.NombreContraparte = String.Empty
                            objRegistroSelected.NumeroDocumentoContraparte = String.Empty
                            objRegistroSelected.CodTipoIdentificacionContraparte = String.Empty
                            objRegistroSelected.TipoIdentificacionContraparte = String.Empty

                            objRegistroSelected.IDPagador = Nothing
                            objRegistroSelected.NombrePagador = String.Empty
                            objRegistroSelected.NumeroDocumentoPagador = String.Empty
                            objRegistroSelected.TipoIdentificacionPagador = String.Empty
                            objRegistroSelected.IDComitenteOtro = String.Empty
                            objRegistroSelected.NroCuentaDepositoContraparte = Nothing
                            objRegistroSelected.DepositoContraparte = String.Empty
                            objRegistroSelected.DescripcionCuentaDepositoContraparte = String.Empty
                        End If

                    Case OPCION_DUPLICAR

                        VerificarValoresEnCombos()
                        HabilitarCamposRegistro(_EncabezadoSeleccionado)

                    Case OPCION_EDITAR

                        VerificarValoresEnCombos()
                        HabilitarCamposRegistro(_EncabezadoSeleccionado)

                    Case OPCION_TIPOREPO

                        If objRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO And objRegistroSelected.Tipo = CLASE_ACCIONES Then
                            objRegistroSelected.TipoRepo = TIPOREPO_CERRADO
                        ElseIf objRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO And objRegistroSelected.Tipo = CLASE_RENTAFIJA Then
                            objRegistroSelected.TipoRepo = TIPOREPO_ABIERTO
                        End If

                    Case OPCION_TIPOBANREP

                        If objRegistroSelected.TipoBanRep Then
                            objRegistroSelected.Tipo = CLASE_RENTAFIJA
                            LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_TIPO)
                            ObtenerClaseEspecies()
                        End If

                End Select

                logCalcularValores = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto.",
                                 Me.ToString(), "ObtenerValoresDefecto", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ValidarHabilitarNegocio(Optional ByVal CambioTipoOperacion As Boolean = False)
        Try
            If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoNegocio) Then
                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)

                If CambioTipoOperacion = False Then
                    ComitenteSeleccionado = Nothing
                    NemotecnicoSeleccionado = Nothing
                    _EncabezadoSeleccionado.Nemotecnico = String.Empty
                    If BorrarCliente Then
                        BorrarCliente = False
                    End If
                    BorrarCliente = True
                    If BorrarEspecie Then
                        BorrarEspecie = False
                    End If
                    BorrarEspecie = True
                End If

                HabilitarCamposRegistro(_EncabezadoSeleccionado)
            Else
                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                ComitenteSeleccionado = Nothing
                NemotecnicoSeleccionado = Nothing
                _EncabezadoSeleccionado.Nemotecnico = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la habilitación del negocio.",
                                Me.ToString(), "ValidarHabilitarNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function SeleccionarEspecie(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies) As Task
        Try
            If Not IsNothing(pobjEspecie) Then
                If logEditarRegistro Or logNuevoRegistro Then
                    Dim logConsutarTRMMoneda As Boolean = False

                    logCalcularValores = False

                    _EncabezadoSeleccionado.ISIN = pobjEspecie.ISIN
                    _EncabezadoSeleccionado.EsAccion = pobjEspecie.EsAccion

                    If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or
                        _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO Or
                        _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or
                        _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                        _EncabezadoSeleccionado.FechaEmision = pobjEspecie.Emision
                        _EncabezadoSeleccionado.FechaVencimiento = pobjEspecie.Vencimiento
                        _EncabezadoSeleccionado.Modalidad = pobjEspecie.CodModalidad
                        _EncabezadoSeleccionado.TipoTasaFija = pobjEspecie.CodTipoTasaFija

                        If _EncabezadoSeleccionado.TipoTasaFija = TIPOTASA_VARIABLE Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)

                            _EncabezadoSeleccionado.TasaFacial = 0

                            If Not IsNothing(pobjEspecie.IdIndicador) Then
                                _EncabezadoSeleccionado.IndicadorEconomico = pobjEspecie.IdIndicador.ToString
                            Else
                                _EncabezadoSeleccionado.IndicadorEconomico = String.Empty
                            End If

                            If Not IsNothing(pobjEspecie.PuntosIndicador) Then
                                If pobjEspecie.PuntosIndicador > 99 Or pobjEspecie.PuntosIndicador < -99 Then
                                    mostrarMensaje("Los puntos del indicador estan en un rango no permitido (-99->99), por favor corrija los valores", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                                _EncabezadoSeleccionado.PuntosIndicador = pobjEspecie.PuntosIndicador
                            Else
                                _EncabezadoSeleccionado.PuntosIndicador = 0
                            End If

                        Else
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)

                            If Not IsNothing(pobjEspecie.TasaFacial) Then
                                _EncabezadoSeleccionado.TasaFacial = pobjEspecie.TasaFacial
                            Else
                                _EncabezadoSeleccionado.TasaFacial = 0
                            End If

                            _EncabezadoSeleccionado.IndicadorEconomico = "0"
                            _EncabezadoSeleccionado.PuntosIndicador = 0
                        End If
                    End If

                    If Not IsNothing(pobjEspecie.IDMoneda) Then
                        If _EncabezadoSeleccionado.IDMoneda <> pobjEspecie.IDMoneda Then
                            _EncabezadoSeleccionado.IDMoneda = pobjEspecie.IDMoneda
                            _EncabezadoSeleccionado.CodigoMoneda = pobjEspecie.CodMoneda
                            _EncabezadoSeleccionado.NombreMoneda = pobjEspecie.NombreMoneda
                            logConsutarTRMMoneda = True

                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
                        End If
                    End If

                    If logEsModal = False Then
                        If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAFIJA _
                        Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO _
                        Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA _
                        Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                            LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                        End If
                    End If

                    Await ObtenerPrecioMercado()
                    If logConsutarTRMMoneda Then
                        Await ObtenerTRMMoneda()
                    End If

                    logCalcularValores = True
                    BorrarEspecie = False
                End If
            Else
                _EncabezadoSeleccionado.Nemotecnico = String.Empty
                _EncabezadoSeleccionado.ISIN = String.Empty
                _EncabezadoSeleccionado.FechaEmision = Nothing
                _EncabezadoSeleccionado.FechaVencimiento = Nothing
                _EncabezadoSeleccionado.Modalidad = Nothing
                _EncabezadoSeleccionado.TasaFacial = Nothing
                _EncabezadoSeleccionado.IndicadorEconomico = Nothing
                _EncabezadoSeleccionado.PuntosIndicador = Nothing

                If BorrarEspecie Then
                    BorrarEspecie = False
                End If
                BorrarEspecie = True

                LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos de la especie.", Me.ToString, "SeleccionarEspecie", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Async Sub SeleccionarCliente(ByVal pobjCliente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjCliente) Then
                If logEditarRegistro Or logNuevoRegistro Then

                    If Not IsNothing(_EncabezadoSeleccionado) Then

                        _EncabezadoSeleccionado.IDComitente = pobjCliente.IdComitente
                        _EncabezadoSeleccionado.NombreCliente = pobjCliente.Nombre
                        _EncabezadoSeleccionado.NumeroDocumentoCliente = pobjCliente.NroDocumento
                        _EncabezadoSeleccionado.CodTipoIdentificacionCliente = pobjCliente.CodTipoIdentificacion
                        _EncabezadoSeleccionado.TipoIdentificacionCliente = pobjCliente.TipoIdentificacion

                        Await ConsultarReceptoresCliente(_EncabezadoSeleccionado.IDComitente)

                        BorrarCliente = False
                    End If
                End If
            Else
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    _EncabezadoSeleccionado.IDComitente = String.Empty
                    _EncabezadoSeleccionado.NombreCliente = String.Empty
                    _EncabezadoSeleccionado.CodTipoIdentificacionCliente = String.Empty
                    _EncabezadoSeleccionado.TipoIdentificacionCliente = String.Empty
                    _EncabezadoSeleccionado.NumeroDocumentoCliente = 0
                    _EncabezadoSeleccionado.IDOrdenante = String.Empty

                    OrdenanteSeleccionado = Nothing
                    CtaDepositoSeleccionada = Nothing

                    ListaOrdenantes = Nothing
                    ListaCuentasDeposito = Nothing

                    BorrarCliente = True

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub HabilitarCamposRegistro(ByVal pobjRegistroSelected As CFOperaciones.Operaciones_OtrosNegocios)
        Try
            If Editando Then
                If Not IsNothing(pobjRegistroSelected) Then
                    If Not String.IsNullOrEmpty(pobjRegistroSelected.TipoNegocio) Then

                        If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then

                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOCUMPLIMIENTO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIES, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDA, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARPAIS, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCONTRAPARTE, True)

                            MostrarTipoBanRep = Visibility.Collapsed
                            MostrarOrdenante = Visibility.Visible
                            MostrarCuentaDeposito = Visibility.Visible

                            If Not String.IsNullOrEmpty(pobjRegistroSelected.TipoOrigen) Then
                                Dim logHabilitarCamposComision As Boolean = False

                                If VerificarTipoFuncionalidadTipoOrigen(pobjRegistroSelected.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACOMISION) Then
                                    logHabilitarCamposComision = True
                                End If

                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, logHabilitarCamposComision)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, False)
                            End If
                        ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOCUMPLIMIENTO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIES, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDA, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARPAIS, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCONTRAPARTE, True)
                            MostrarTipoBanRep = Visibility.Collapsed
                            MostrarOrdenante = Visibility.Visible
                            MostrarCuentaDeposito = Visibility.Visible

                            If pobjRegistroSelected.TipoTasaFija = TIPOTASA_VARIABLE Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, True)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, True)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                            End If

                            If Not String.IsNullOrEmpty(pobjRegistroSelected.TipoOrigen) Then
                                Dim logHabilitarCamposComision As Boolean = False
                                Dim logHabilitarCamposRetencion As Boolean = False

                                If VerificarTipoFuncionalidadTipoOrigen(pobjRegistroSelected.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACOMISION) Then
                                    logHabilitarCamposComision = True
                                End If

                                If VerificarTipoFuncionalidadTipoOrigen(pobjRegistroSelected.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJARETENCION) Then
                                    logHabilitarCamposRetencion = True
                                End If

                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, logHabilitarCamposComision)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION, logHabilitarCamposRetencion)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION, False)
                            End If
                        ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO _
                            Or pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA _
                            Or pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)

                            If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                                If pobjRegistroSelected.Tipo = CLASE_ACCIONES Then
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, True)
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                                Else
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                                    If pobjRegistroSelected.TipoRepo = TIPOREPO_ABIERTO Then
                                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, True)
                                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                                    Else
                                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, True)
                                    End If
                                End If

                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                            ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, True)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                            ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)

                                If pobjRegistroSelected.Tipo = CLASE_ACCIONES Then
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, True)
                                Else
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, True)
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                                End If
                            End If

                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOCUMPLIMIENTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIES, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDA, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARPAIS, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCONTRAPARTE, True)

                            MostrarOrdenante = Visibility.Visible
                            MostrarCuentaDeposito = Visibility.Visible

                            If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA _
                            Or pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, False)
                                MostrarTipoBanRep = Visibility.Collapsed
                            Else
                                If pobjRegistroSelected.Tipo = CLASE_ACCIONES Then
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, False)
                                Else
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, True)
                                End If

                                MostrarTipoBanRep = Visibility.Visible
                            End If

                            If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, False)
                            ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                                If pobjRegistroSelected.TipoBanRep Then
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, False)
                                Else
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, True)
                                End If
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, True)
                            End If

                            If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, True)
                            Else
                                If pobjRegistroSelected.Tipo = CLASE_RENTAFIJA Then
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, True)
                                Else
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, False)
                                End If
                            End If

                            If pobjRegistroSelected.TipoTasaFija = TIPOTASA_VARIABLE Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, True)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, True)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                            End If

                            If pobjRegistroSelected.Tipo = CLASE_ACCIONES Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO, False)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO, True)
                            End If

                            If Not String.IsNullOrEmpty(pobjRegistroSelected.TipoOrigen) Then
                                Dim logHabilitarCamposComision As Boolean = False
                                If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_TIPOORIGEN_MANEJACOMISION") Then
                                    If DiccionarioCombosCompleta("NEGOCIOS_TIPOORIGEN_MANEJACOMISION").Where(Function(i) i.Descripcion = pobjRegistroSelected.TipoOrigen And i.Codigo = "SI").Count > 0 Then
                                        logHabilitarCamposComision = True
                                    End If
                                End If

                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, logHabilitarCamposComision)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, False)
                            End If

                            If pobjRegistroSelected.TipoRepo = TIPOREPO_ABIERTO Then
                                If pobjRegistroSelected.IDMoneda <> intMonedaLocal Then
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIO, True)
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIOCOP, False)
                                Else
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIO, False)
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIOCOP, True)
                                End If
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIO, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIOCOP, True)
                            End If

                        ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOCUMPLIMIENTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARPAIS, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCONTRAPARTE, True)
                            MostrarTipoBanRep = Visibility.Collapsed
                            MostrarOrdenante = Visibility.Visible
                            MostrarCuentaDeposito = Visibility.Visible

                        ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOCUMPLIMIENTO, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDA, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARPAIS, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCONTRAPARTE, False)
                            MostrarTipoBanRep = Visibility.Collapsed
                            MostrarOrdenante = Visibility.Collapsed
                            MostrarCuentaDeposito = Visibility.Collapsed

                            If Not String.IsNullOrEmpty(pobjRegistroSelected.TipoOrigen) Then
                                Dim logHabilitarCamposComision As Boolean = False

                                If VerificarTipoFuncionalidadTipoOrigen(pobjRegistroSelected.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACOMISION) Then
                                    logHabilitarCamposComision = True
                                End If

                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, logHabilitarCamposComision)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, False)
                            End If

                        End If

                        If String.IsNullOrEmpty(pobjRegistroSelected.TipoOperacion) Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                        End If

                        If pobjRegistroSelected.IDMoneda = intMonedaLocal Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL, False)
                        Else
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL, True)
                        End If

                        'SE REALIZA EL CAMBIO PARA QUE EL MANEJO DE LA CONTRAPARTE SEA POR MEDIO DE LA CONFIGURACIÓN DE LA TABLA CF.tblConfiguracionTipoOrigen
                        Dim logManejaContraparteTipoOrigenHabilitar As Boolean = False

                        If VerificarTipoFuncionalidadTipoOrigen(pobjRegistroSelected.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACONTRAPARTE) Then
                            logManejaContraparteTipoOrigenHabilitar = True
                        End If

                        If logManejaContraparteTipoOrigenHabilitar Then
                            MostrarContraparte = Visibility.Collapsed
                            MostrarContraparteOTC = Visibility.Visible
                        Else
                            MostrarContraparte = Visibility.Visible
                            MostrarContraparteOTC = Visibility.Collapsed
                        End If

                    End If
                End If
            Else
                If Not IsNothing(pobjRegistroSelected) Then
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOCUMPLIMIENTO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, True)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIES, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDA, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARPAIS, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCONTRAPARTE, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARVALORGIROINICIOCOP, False)

                    If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, True)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                        MostrarTipoBanRep = Visibility.Collapsed
                        MostrarOrdenante = Visibility.Visible
                        MostrarCuentaDeposito = Visibility.Visible
                    ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, True)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, True)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                        MostrarTipoBanRep = Visibility.Collapsed
                        MostrarOrdenante = Visibility.Visible
                        MostrarCuentaDeposito = Visibility.Visible
                    ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO _
                        Or pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA _
                        Or pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then

                        If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                            If pobjRegistroSelected.Tipo = CLASE_ACCIONES Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, True)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                                If pobjRegistroSelected.TipoRepo = TIPOREPO_ABIERTO Then
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, True)
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                                Else
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, True)
                                End If
                            End If

                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                        ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, True)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                        ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_TTV Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                            If pobjRegistroSelected.Tipo = CLASE_ACCIONES Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, True)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, True)
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                            End If
                        End If
                        MostrarOrdenante = Visibility.Visible
                        MostrarCuentaDeposito = Visibility.Visible

                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)

                        If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                            CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, True)
                        Else
                            If pobjRegistroSelected.Tipo = CLASE_RENTAFIJA Then
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, True)
                            Else
                                CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, False)
                            End If
                        End If

                        If pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_REPO Then
                            MostrarTipoBanRep = Visibility.Visible
                        Else
                            MostrarTipoBanRep = Visibility.Collapsed
                        End If
                    ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, True)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                        MostrarTipoBanRep = Visibility.Collapsed
                        MostrarOrdenante = Visibility.Visible
                        MostrarCuentaDeposito = Visibility.Visible
                    ElseIf pobjRegistroSelected.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDIVISAS, True)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSDEPOSITOREMUNERADO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAVARIABLE, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSRENTAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOABIERTO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOACCIONES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSREPOCERRADO, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSSIMULTANEA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTV, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MOSTRARCAMPOSTTVACCIONES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                        CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                        MostrarTipoBanRep = Visibility.Collapsed
                        MostrarOrdenante = Visibility.Collapsed
                        MostrarCuentaDeposito = Visibility.Collapsed
                    End If

                    'SE REALIZA EL CAMBIO PARA QUE EL MANEJO DE LA CONTRAPARTE SEA POR MEDIO DE LA CONFIGURACIÓN DE LA TABLA CF.tblConfiguracionTipoOrigen
                    Dim logManejaContraparteTipoOrigenHabilitar1 As Boolean = False

                    If VerificarTipoFuncionalidadTipoOrigen(pobjRegistroSelected.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACONTRAPARTE) Then
                        logManejaContraparteTipoOrigenHabilitar1 = True
                    End If

                    If logManejaContraparteTipoOrigenHabilitar1 Then
                        MostrarContraparte = Visibility.Collapsed
                        MostrarContraparteOTC = Visibility.Visible
                    Else
                        MostrarContraparte = Visibility.Visible
                        MostrarContraparteOTC = Visibility.Collapsed
                    End If
                Else
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARENCABEZADO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOCUMPLIMIENTO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESHABILITARSELECCION, True)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIES, True)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES, True)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAFIJA, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIESTASAVARIABLE, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDALOCAL, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARMONEDA, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARPAIS, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIRREPO, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSRETENCION, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCAMPOSCOMISION, False)
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARCONTRAPARTE, False)
                    MostrarContraparte = Visibility.Visible
                    MostrarContraparteOTC = Visibility.Collapsed
                    MostrarTipoBanRep = Visibility.Collapsed
                    CambiarValor_OpcionHabilitarCampos(OPCIONES_HABILITARCAMPOS.HABILITARTIPO, False)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al habilitar los controles dependiendo del tipo de registro.", Me.ToString, "HabilitarCamposRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub consultarOrdenantes(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                If Not IsNothing(mdcProxyUtilidad.BuscadorOrdenantes) Then
                    mdcProxyUtilidad.BuscadorOrdenantes.Clear()
                End If

                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarOrdenantesComitenteQuery(strClienteABuscar, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenantes, pstrUserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los ordenantes del cliente.", Me.ToString, "consultarOrdenantes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Async Function consultarOrdenantesAsincronico(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            If logNuevoRegistro Or logEditarRegistro Then
                Dim objRet As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes)
                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                objRet = Await mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarOrdenantesComitenteQuery(strClienteABuscar, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los valores.", Me.ToString(), "consultarOrdenantesAsincronico", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        ObtenerValoresRespuestaOrdenante(objRet.Entities.ToList, pstrUserState)
                        logResultado = True
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los ordenantes.", Me.ToString(), "consultarOrdenantesAsincronico", Program.TituloSistema, Program.Maquina, ex)
            logResultado = False
        End Try

        IsBusyCalculos = False
        Return (logResultado)
    End Function

    Private Sub consultarCuentasDeposito(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                If Not IsNothing(mdcProxyUtilidad.BuscadorCuentasDepositos) Then
                    mdcProxyUtilidad.BuscadorCuentasDepositos.Clear()
                End If
                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                ListaCuentasDeposito = Nothing
                mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarCuentasDepositoComitenteQuery(strClienteABuscar, True, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDeposito, pstrUserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar las cuentas deposito del cliente.", Me.ToString, "consultarCuentasDeposito", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Async Function consultarCuentasDepositoAsincronico(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            If logNuevoRegistro Or logEditarRegistro Then
                Dim objRet As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito)
                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                objRet = Await mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarCuentasDepositoComitenteQuery(strClienteABuscar, True, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los valores.", Me.ToString(), "consultarCuentasDepositoAsincronico", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        ObtenerValoresRespuestaCuentasDeposito(objRet.Entities.ToList, pstrUserState)
                        logResultado = True
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar las cuentas deposito.", Me.ToString(), "consultarCuentasDepositoAsincronico", Program.TituloSistema, Program.Maquina, ex)
            logResultado = False
        End Try

        IsBusyCalculos = False
        Return (logResultado)
    End Function

    Private Sub consultarCuentasDepositoContraparte(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                If Not IsNothing(mdcProxyUtilidad.BuscadorCuentasDepositos) Then
                    mdcProxyUtilidad.BuscadorCuentasDepositos.Clear()
                End If
                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

                mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarCuentasDepositoComitenteQuery(strClienteABuscar, True, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDepositoContraparte, pstrUserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar las cuentas deposito del cliente.", Me.ToString, "consultarCuentasDeposito", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Function ValidarGuardadoRegistro(ByVal pobjRegistro As CFOperaciones.Operaciones_OtrosNegocios) As Boolean
        Try
            'Valida los campos que son requeridos por el sistema de OYDPLUS.
            Dim logPasaValidacion As Boolean = True
            Dim strMensajeValidacion = String.Empty

            If String.IsNullOrEmpty(pobjRegistro.TipoNegocio) Then
                strMensajeValidacion = String.Format("{0}{1} -   Tipo negocio", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.TipoOrigen) Then
                strMensajeValidacion = String.Format("{0}{1} -   Tipo origen", strMensajeValidacion, vbCrLf)
            End If

            If String.IsNullOrEmpty(pobjRegistro.TipoOperacion) Then
                strMensajeValidacion = String.Format("{0}{1} -   Tipo operación", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.FechaOperacion) Then
                strMensajeValidacion = String.Format("{0}{1} -   Fecha operación", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.FechaCumplimiento) Then
                strMensajeValidacion = String.Format("{0}{1} -   Fecha cumplimiento", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.PaisNegociacion) Or pobjRegistro.PaisNegociacion = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   País negociación", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.IDContraparte) Or pobjRegistro.IDContraparte = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Contraparte", strMensajeValidacion, vbCrLf)
            End If

            'SE REALIZA EL CAMBIO PARA QUE EL MANEJO DE LA CONTRAPARTE SEA POR MEDIO DE LA CONFIGURACIÓN DE LA TABLA CF.tblConfiguracionTipoOrigen
            If VerificarTipoFuncionalidadTipoOrigen(pobjRegistro.TipoOrigen, TIPOSORIGEN_TIPOMANEJO.NEGOCIOS_TIPOORIGEN_MANEJACONTRAPARTE) Then
                If Not String.IsNullOrEmpty(pobjRegistro.IDComitenteOtro) Then
                    If ((IsNothing(pobjRegistro.IDCuentaDepositoContraparte) Or pobjRegistro.IDCuentaDepositoContraparte = 0) And String.IsNullOrEmpty(pobjRegistro.DepositoContraparte)) Then
                        strMensajeValidacion = String.Format("{0}{1} -   Cuenta depósito Contraparte", strMensajeValidacion, vbCrLf)
                    End If
                End If
            End If

            ' CCM2015111: La cuenta de depósito de la contraparte es requerida solamente para los negocios internacionales
            If VerificarOrigenPrincipalSecundario(TIPOORIGEN_INTERNACIONAL, pobjRegistro.TipoOrigen) Then
                If (IsNothing(pobjRegistro.IDCuentaDepositoContraparte) Or pobjRegistro.IDCuentaDepositoContraparte = 0) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Cuenta depósito contraparte", strMensajeValidacion, vbCrLf)
                End If
            End If
            'If pobjRegistro.TipoOrigen.ToUpper.Equals(TIPOORIGEN_INTERNACIONAL.ToUpper) And (IsNothing(pobjRegistro.IDCuentaDepositoContraparte) Or pobjRegistro.IDCuentaDepositoContraparte = 0) Then
            '    strMensajeValidacion = String.Format("{0}{1} -   Cuenta depósito contraparte", strMensajeValidacion, vbCrLf)
            'End If

            ' JDCP20160630: Se elimina codigo para no validar el pagador ya que se quito de la pantalla
            'If IsNothing(pobjRegistro.IDPagador) Or pobjRegistro.IDPagador = 0 Then
            '    strMensajeValidacion = String.Format("{0}{1} -   Pagador", strMensajeValidacion, vbCrLf)
            'End If

            If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION.ToString) Then
                If String.IsNullOrEmpty(pobjRegistro.ClasificacionInversion) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Clasificación inversión", strMensajeValidacion, vbCrLf)
                End If
            End If

            If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION.ToString) Then
                If IsNothing(pobjRegistro.FechaVencimientoOperacion) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Fecha vencimiento operación", strMensajeValidacion, vbCrLf)
                End If
            End If

            If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO.ToString) Then
                If String.IsNullOrEmpty(pobjRegistro.Mercado) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Mercado", strMensajeValidacion, vbCrLf)
                End If
            End If

            If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO.ToString) Then
                If String.IsNullOrEmpty(pobjRegistro.TipoRepo) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Tipo repo", strMensajeValidacion, vbCrLf)
                End If
            End If

            If String.IsNullOrEmpty(pobjRegistro.IDComitente) Then
                strMensajeValidacion = String.Format("{0}{1} -   Cliente", strMensajeValidacion, vbCrLf)
            End If

            If pobjRegistro.TipoNegocio <> TIPONEGOCIO_DIVISAS Then
                If String.IsNullOrEmpty(pobjRegistro.IDOrdenante) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Ordenante", strMensajeValidacion, vbCrLf)
                End If
            End If

            If pobjRegistro.TipoNegocio <> TIPONEGOCIO_DIVISAS Then
                If ((IsNothing(pobjRegistro.IDCuentaDeposito) Or pobjRegistro.IDCuentaDeposito = 0) And String.IsNullOrEmpty(pobjRegistro.Deposito)) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Cuenta depósito", strMensajeValidacion, vbCrLf)
                End If
            End If

            If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.ESPECIES.ToString) Then
                If String.IsNullOrEmpty(pobjRegistro.Nemotecnico) Then
                    strMensajeValidacion = String.Format("{0}{1} -   Especie", strMensajeValidacion, vbCrLf)
                End If
            End If

            If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES.ToString) Then
                If pobjRegistro.EsAccion = False Then
                    If IsNothing(pobjRegistro.FechaEmision) Then
                        strMensajeValidacion = String.Format("{0}{1} -   Emisión", strMensajeValidacion, vbCrLf)
                    End If

                    If IsNothing(pobjRegistro.FechaVencimiento) Then
                        strMensajeValidacion = String.Format("{0}{1} -   Vencimiento", strMensajeValidacion, vbCrLf)
                    End If

                    If String.IsNullOrEmpty(pobjRegistro.Modalidad) Then
                        strMensajeValidacion = String.Format("{0}{1} -   Modalidad", strMensajeValidacion, vbCrLf)
                    End If

                    If pobjRegistro.TipoTasaFija = TIPOTASA_VARIABLE Then
                        If String.IsNullOrEmpty(pobjRegistro.IndicadorEconomico) Then
                            strMensajeValidacion = String.Format("{0}{1} -   Indicador", strMensajeValidacion, vbCrLf)
                        End If

                        If IsNothing(pobjRegistro.PuntosIndicador) Then
                            strMensajeValidacion = String.Format("{0}{1} -   Puntos indicador", strMensajeValidacion, vbCrLf)
                        End If
                    Else
                        If IsNothing(pobjRegistro.TasaFacial) Then
                            strMensajeValidacion = String.Format("{0}{1} -   Tasa facial", strMensajeValidacion, vbCrLf)
                        End If
                    End If
                End If
            End If

            If IsNothing(pobjRegistro.IDMoneda) Or pobjRegistro.IDMoneda = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   Moneda", strMensajeValidacion, vbCrLf)
            End If

            If IsNothing(pobjRegistro.Nominal) Or pobjRegistro.Nominal = 0 Then
                If pobjRegistro.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                    strMensajeValidacion = String.Format("{0}{1} -   Cantidad", strMensajeValidacion, vbCrLf)
                ElseIf pobjRegistro.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                    strMensajeValidacion = String.Format("{0}{1} -   Inversión", strMensajeValidacion, vbCrLf)
                Else
                    strMensajeValidacion = String.Format("{0}{1} -   Nominal", strMensajeValidacion, vbCrLf)
                End If
            End If

            If IsNothing(pobjRegistro.TasaCambioConversion) Or pobjRegistro.TasaCambioConversion = 0 Then
                strMensajeValidacion = String.Format("{0}{1} -   TRM", strMensajeValidacion, vbCrLf)
            End If

            If pobjRegistro.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                pobjRegistro.TipoNegocio = TIPONEGOCIO_RENTAFIJA Or
                pobjRegistro.TipoNegocio = TIPONEGOCIO_REPO Or
                pobjRegistro.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or
                pobjRegistro.TipoNegocio = TIPONEGOCIO_TTV Then
                If IsNothing(pobjRegistro.PrecioSucio) Or pobjRegistro.PrecioSucio = 0 Then
                    If pobjRegistro.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                        strMensajeValidacion = String.Format("{0}{1} -   Precio", strMensajeValidacion, vbCrLf)
                    Else
                        strMensajeValidacion = String.Format("{0}{1} -   Precio sucio", strMensajeValidacion, vbCrLf)
                    End If
                End If
            End If

            'If pobjRegistro.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
            '    If IsNothing(pobjRegistro.TasaNegociacionYield) Or pobjRegistro.TasaNegociacionYield = 0 Then
            '        strMensajeValidacion = String.Format("{0}{1} -   Tasa de retorno (Yield)", strMensajeValidacion, vbCrLf)
            '    End If
            'End If

            If pobjRegistro.TipoNegocio = TIPONEGOCIO_REPO Then
                If pobjRegistro.Tipo = CLASE_ACCIONES Then
                    If IsNothing(pobjRegistro.ValorGiroBrutoCOP) Or pobjRegistro.ValorGiroBrutoCOP = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Valor giro bruto", strMensajeValidacion, vbCrLf)
                    End If
                    If IsNothing(pobjRegistro.ValorNetoCop) Or pobjRegistro.ValorNetoCop = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Valor neto COP", strMensajeValidacion, vbCrLf)
                    End If
                Else
                    If pobjRegistro.TipoRepo = TIPOREPO_ABIERTO Then
                        If IsNothing(pobjRegistro.ValorGiroBruto) Or pobjRegistro.ValorGiroBruto = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} -   Valor giro inicio COP", strMensajeValidacion, vbCrLf)
                        End If
                        If IsNothing(pobjRegistro.ValorNeto) Or pobjRegistro.ValorNeto = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} -   Valor neto moneda", strMensajeValidacion, vbCrLf)
                        End If
                        If IsNothing(pobjRegistro.ValorNetoCop) Or pobjRegistro.ValorNetoCop = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} -   Valor neto COP", strMensajeValidacion, vbCrLf)
                        End If
                    Else
                        If IsNothing(pobjRegistro.ValorGiroBruto) Or pobjRegistro.ValorGiroBruto = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} -   Valor bruto moneda", strMensajeValidacion, vbCrLf)
                        End If
                        If IsNothing(pobjRegistro.ValorGiroBrutoCOP) Or pobjRegistro.ValorGiroBrutoCOP = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} -   Valor bruto COP", strMensajeValidacion, vbCrLf)
                        End If
                        If IsNothing(pobjRegistro.ValorGiroBrutoCOP) Or pobjRegistro.ValorGiroBrutoCOP = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} -   Valor bruto COP", strMensajeValidacion, vbCrLf)
                        End If
                    End If
                End If
            ElseIf pobjRegistro.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                If IsNothing(pobjRegistro.ValorGiroBruto) Or pobjRegistro.ValorGiroBruto = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} -   Valor bruto moneda", strMensajeValidacion, vbCrLf)
                End If
                If IsNothing(pobjRegistro.ValorGiroBrutoCOP) Or pobjRegistro.ValorGiroBrutoCOP = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} -   Valor bruto COP", strMensajeValidacion, vbCrLf)
                End If
                If IsNothing(pobjRegistro.ValorNetoCop) Or pobjRegistro.ValorNetoCop = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} -   Valor neto COP", strMensajeValidacion, vbCrLf)
                End If
            ElseIf pobjRegistro.TipoNegocio = TIPONEGOCIO_TTV Then
                If pobjRegistro.Tipo = CLASE_ACCIONES Then
                    If IsNothing(pobjRegistro.ValorGiroBruto) Or pobjRegistro.ValorGiroBruto = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Valor base", strMensajeValidacion, vbCrLf)
                    End If
                    'If IsNothing(pobjRegistro.ValorGiroBrutoCOP) Or pobjRegistro.ValorGiroBrutoCOP = 0 Then
                    '    strMensajeValidacion = String.Format("{0}{1} -   Valor giro bruto", strMensajeValidacion, vbCrLf)
                    'End If
                    'JP20190809 SE VALIDA EN EL SP CF.uspCalculosFinancieros_LiquidacionesOtrosNegocios_Validar
                    'If IsNothing(pobjRegistro.ValorNetoCop) Or pobjRegistro.ValorNetoCop = 0 Then
                    '    strMensajeValidacion = String.Format("{0}{1} -   Valor giro neto", strMensajeValidacion, vbCrLf)
                    'End If
                Else
                    If IsNothing(pobjRegistro.ValorGiroBruto) Or pobjRegistro.ValorGiroBruto = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Valor base", strMensajeValidacion, vbCrLf)
                    End If
                    'If IsNothing(pobjRegistro.ValorGiroBrutoCOP) Or pobjRegistro.ValorGiroBrutoCOP = 0 Then
                    '    strMensajeValidacion = String.Format("{0}{1} -   Valor giro bruto", strMensajeValidacion, vbCrLf)
                    'End If
                    If IsNothing(pobjRegistro.ValorNetoCop) Or pobjRegistro.ValorNetoCop = 0 Then
                        strMensajeValidacion = String.Format("{0}{1} -   Valor giro neto", strMensajeValidacion, vbCrLf)
                    End If
                End If
            Else
                If IsNothing(pobjRegistro.ValorGiroBruto) Or pobjRegistro.ValorGiroBruto = 0 Then
                    strMensajeValidacion = String.Format("{0}{1} -   Valor de giro", strMensajeValidacion, vbCrLf)
                End If
            End If

            If pobjRegistro.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Or
               pobjRegistro.TipoNegocio = TIPONEGOCIO_REPO Or
               pobjRegistro.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or
               pobjRegistro.TipoNegocio = TIPONEGOCIO_TTV Then
                If IsNothing(pobjRegistro.TasaPactadaRepo) Or pobjRegistro.TasaPactadaRepo <= 0 Then
                    strMensajeValidacion = String.Format("{0}{1} -   Tasa pactada", strMensajeValidacion, vbCrLf)
                End If
            End If

            If pobjRegistro.TipoNegocio = TIPONEGOCIO_REPO Or
                pobjRegistro.TipoNegocio = TIPONEGOCIO_SIMULTANEA Or
                pobjRegistro.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                If IsNothing(pobjRegistro.ValorRegresoRepo) Or pobjRegistro.ValorRegresoRepo <= 0 Then
                    strMensajeValidacion = String.Format("{0}{1} -   Valor regreso", strMensajeValidacion, vbCrLf)
                End If
                'Se retira esta condición debido a que para las ttv el campo ValorRegresoRepo siempre será cero
                'ElseIf pobjRegistro.TipoNegocio = TIPONEGOCIO_TTV And pobjRegistro.Tipo = CLASE_RENTAFIJA Then
                '    If IsNothing(pobjRegistro.ValorRegresoRepo) Or pobjRegistro.ValorRegresoRepo <= 0 Then
                '        strMensajeValidacion = String.Format("{0}{1} -   Valor regreso", strMensajeValidacion, vbCrLf)
                '    End If
            ElseIf pobjRegistro.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Or
                pobjRegistro.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                If IsNothing(pobjRegistro.ValorNeto) Or pobjRegistro.ValorNeto <= 0 Then
                    strMensajeValidacion = String.Format("{0}{1} -   Valor neto", strMensajeValidacion, vbCrLf)
                End If
            End If

            If String.IsNullOrEmpty(strMensajeValidacion) Then
                If Not IsNothing(_ListaReceptores) Then
                    If _ListaReceptores.Count > 0 Then
                        Dim logReceptorVacio As Boolean = False
                        Dim logReceptorRepetido As Boolean = False
                        Dim logPorcentajeDiferente As Boolean = False
                        Dim dblPorcentaje As Double = 0
                        Dim logNoTieneUnLider As Boolean = False
                        Dim logMasDeUnLider As Boolean = False

                        If _ListaReceptores.Where(Function(i) String.IsNullOrEmpty(i.Receptor)).Count > 0 Then
                            logReceptorVacio = True
                        End If

                        If _ListaReceptores.Where(Function(i) i.Lider = True).Count > 1 Then
                            logMasDeUnLider = True
                        End If

                        If _ListaReceptores.Where(Function(i) i.Lider = True).Count = 0 Then
                            logNoTieneUnLider = True
                        End If

                        For Each li In _ListaReceptores
                            If IsNothing(li.Porcentaje) Then
                                dblPorcentaje += 0
                            Else
                                dblPorcentaje += li.Porcentaje
                            End If

                            If logReceptorRepetido = False Then
                                If _ListaReceptores.Where(Function(i) i.Receptor = li.Receptor).Count > 1 Then
                                    logReceptorRepetido = True
                                End If
                            End If
                        Next

                        If dblPorcentaje <> 100 Then
                            logPorcentajeDiferente = True
                        End If

                        If logReceptorVacio Then
                            strMensajeValidacion = String.Format("{0}{1} -   Asesor es requerido en todos los detalles", strMensajeValidacion, vbCrLf)
                        End If
                        If logReceptorRepetido Then
                            strMensajeValidacion = String.Format("{0}{1} -   Asesor repetido", strMensajeValidacion, vbCrLf)
                        End If
                        If logNoTieneUnLider Then
                            strMensajeValidacion = String.Format("{0}{1} -   No tiene al menos un asesor lider", strMensajeValidacion, vbCrLf)
                        End If
                        If logMasDeUnLider Then
                            strMensajeValidacion = String.Format("{0}{1} -   Tiene mas de un lider", strMensajeValidacion, vbCrLf)
                        End If
                        If logPorcentajeDiferente Then
                            strMensajeValidacion = String.Format("{0}{1} -   El porcentaje no suma 100%", strMensajeValidacion, vbCrLf)
                        End If

                        If Not String.IsNullOrEmpty(strMensajeValidacion) Then
                            logPasaValidacion = False
                            strMensajeValidacion = String.Format("Señor usuario, debe de corregir las siguientes inconsistencias en el detalle de la distribución de comisión.{0}{1}", strMensajeValidacion, vbCrLf)
                            mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If

                    Else
                        logPasaValidacion = False
                        strMensajeValidacion = "Señor usuario, debe de ingresar al menos un registro en los detalles de la distribución de comisión."
                        mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    logPasaValidacion = False
                    strMensajeValidacion = "Señor usuario, debe de ingresar al menos un registro en los detalles de la distribución de comisión."
                    mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                If String.IsNullOrEmpty(strMensajeValidacion) Then
                    If pobjRegistro.FechaOperacion.Value.Date > pobjRegistro.FechaCumplimiento.Value.Date Then
                        strMensajeValidacion = String.Format("{0}{1} -   La fecha de operación no puede ser mayor a la fecha de cumplimiento", strMensajeValidacion, vbCrLf)
                    End If

                    If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.FECHAVENCIMIENTOOPERACION.ToString) Then
                        If pobjRegistro.FechaCumplimiento.Value.Date > pobjRegistro.FechaVencimientoOperacion.Value.Date Then
                            strMensajeValidacion = String.Format("{0}{1} -   La fecha de cumplimiento no puede ser mayor a la fecha de cumpimiento futuro", strMensajeValidacion, vbCrLf)
                        End If
                    End If

                    If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.CARACTERISTICASFACIALESESPECIES.ToString) Then
                        If Not IsNothing(pobjRegistro.FechaEmision) And Not IsNothing(pobjRegistro.FechaVencimiento) Then
                            If pobjRegistro.FechaEmision.Value.Date > pobjRegistro.FechaVencimiento.Value.Date Then
                                strMensajeValidacion = String.Format("{0}{1} -   La fecha de emisión no puede ser mayor a la fecha de vencimiento", strMensajeValidacion, vbCrLf)
                            End If
                        End If
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

    Public Async Function CalcularValorRegistro(Optional ByVal plogMostrarMensajeUsuario As Boolean = True) As System.Threading.Tasks.Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoNegocio) And Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoOperacion) Then
                    Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_OtrosNegociosCalculos)

                    Try
                        IsBusyCalculos = True
                        ErrorForma = String.Empty

                        dcProxy.Operaciones_OtrosNegociosCalculos.Clear()

                        objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosCalcularSyncQuery(strTipoCalculo,
                                                                                                       _EncabezadoSeleccionado.TipoNegocio,
                                                                                                       _EncabezadoSeleccionado.TipoOrigen,
                                                                                                       _EncabezadoSeleccionado.IDComitente,
                                                                                                       _EncabezadoSeleccionado.TipoOperacion,
                                                                                                       _EncabezadoSeleccionado.FechaOperacion,
                                                                                                       _EncabezadoSeleccionado.FechaCumplimiento,
                                                                                                       _EncabezadoSeleccionado.FechaVencimientoOperacion,
                                                                                                       _EncabezadoSeleccionado.Nemotecnico,
                                                                                                       _EncabezadoSeleccionado.ISIN,
                                                                                                       _EncabezadoSeleccionado.FechaEmision,
                                                                                                       _EncabezadoSeleccionado.FechaVencimiento,
                                                                                                       _EncabezadoSeleccionado.Modalidad,
                                                                                                       _EncabezadoSeleccionado.TasaFacial,
                                                                                                       _EncabezadoSeleccionado.IndicadorEconomico,
                                                                                                       _EncabezadoSeleccionado.PuntosIndicador,
                                                                                                       _EncabezadoSeleccionado.Nominal,
                                                                                                       _EncabezadoSeleccionado.IDMoneda,
                                                                                                       _EncabezadoSeleccionado.TasaCambioConversion,
                                                                                                       _EncabezadoSeleccionado.PrecioSucio,
                                                                                                       _EncabezadoSeleccionado.TasaNegociacionYield,
                                                                                                       _EncabezadoSeleccionado.ValorGiroBruto,
                                                                                                       _EncabezadoSeleccionado.TasaPactadaRepo,
                                                                                                       _EncabezadoSeleccionado.Haircut,
                                                                                                       _EncabezadoSeleccionado.PorcentajeComision,
                                                                                                       _EncabezadoSeleccionado.ValorComision,
                                                                                                       _EncabezadoSeleccionado.IVA,
                                                                                                       _EncabezadoSeleccionado.TipoReteFuente,
                                                                                                       _EncabezadoSeleccionado.Retencion,
                                                                                                       _EncabezadoSeleccionado.ValorNeto,
                                                                                                       _EncabezadoSeleccionado.ValorRegresoRepo,
                                                                                                       _EncabezadoSeleccionado.PaisNegociacion,
                                                                                                       Program.Usuario,
                                                                                                       Program.UsuarioWindows,
                                                                                                       Program.Maquina,
                                                                                                       _EncabezadoSeleccionado.ValorNetoCop,
                                                                                                       _EncabezadoSeleccionado.TipoBanRep,
                                                                                                       _EncabezadoSeleccionado.ValorFlujoIntermedio,
                                                                                                       _EncabezadoSeleccionado.Tipo,
                                                                                                       _EncabezadoSeleccionado.TipoRepo,
                                                                                                       _EncabezadoSeleccionado.TasaNeta,
                                                                                                       _EncabezadoSeleccionado.ValorGiroBrutoCOP,
                                                                                                       _EncabezadoSeleccionado.ValorComisionCOP,
                                                                                                       _EncabezadoSeleccionado.ValorRegresoRepoMoneda, Program.HashConexion)).AsTask()

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
                                    If objRet.Entities.First.Exitoso Then
                                        Dim objResultadoCalculos = objRet.Entities.First

                                        logCalcularValores = False
                                        _EncabezadoSeleccionado.TipoCumplimiento = objResultadoCalculos.TipoCumplimiento

                                        If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                                            _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                            _EncabezadoSeleccionado.PorcentajeComision = objResultadoCalculos.PorcentajeComision
                                            _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                            _EncabezadoSeleccionado.IVA = objResultadoCalculos.IVA
                                            _EncabezadoSeleccionado.ValorNeto = objResultadoCalculos.ValorNeto
                                            _EncabezadoSeleccionado.ValorNetoCop = objResultadoCalculos.ValorNetoCop
                                            _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP
                                            _EncabezadoSeleccionado.ValorComisionCOP = objResultadoCalculos.ValorComisionCOP

                                        ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                                            _EncabezadoSeleccionado.PrecioSucio = objResultadoCalculos.PrecioSucio
                                            _EncabezadoSeleccionado.TasaNegociacionYield = objResultadoCalculos.TasaNegociacionYield
                                            _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                            _EncabezadoSeleccionado.PorcentajeComision = objResultadoCalculos.PorcentajeComision
                                            _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                            _EncabezadoSeleccionado.IVA = objResultadoCalculos.IVA
                                            _EncabezadoSeleccionado.ValorNeto = objResultadoCalculos.ValorNeto
                                            _EncabezadoSeleccionado.ValorNetoCop = objResultadoCalculos.ValorNetoCop
                                            _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP
                                            _EncabezadoSeleccionado.ValorComisionCOP = objResultadoCalculos.ValorComisionCOP

                                        ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                                            _EncabezadoSeleccionado.FechaVencimientoOperacion = objResultadoCalculos.FechaVencimientoOperacion
                                            _EncabezadoSeleccionado.PrecioSucio = objResultadoCalculos.PrecioSucio
                                            _EncabezadoSeleccionado.TasaNegociacionYield = objResultadoCalculos.TasaNegociacionYield
                                            _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                            _EncabezadoSeleccionado.ValorNeto = objResultadoCalculos.ValorNeto
                                            _EncabezadoSeleccionado.PorcentajeComision = objResultadoCalculos.PorcentajeComision
                                            _EncabezadoSeleccionado.ValorFlujoIntermedio = objResultadoCalculos.ValorFlujoIntermedio
                                            _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                            _EncabezadoSeleccionado.IVA = objResultadoCalculos.IVA
                                            _EncabezadoSeleccionado.ValorNetoCop = objResultadoCalculos.ValorNetoCop
                                            _EncabezadoSeleccionado.ValorRegresoRepo = objResultadoCalculos.ValorRegresoRepo
                                            _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP
                                            _EncabezadoSeleccionado.ValorComisionCOP = objResultadoCalculos.ValorComisionCOP
                                            _EncabezadoSeleccionado.TasaNeta = objResultadoCalculos.TasaNeta
                                        ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                                            If EncabezadoSeleccionado.Tipo = CLASE_RENTAFIJA Then
                                                _EncabezadoSeleccionado.FechaVencimientoOperacion = objResultadoCalculos.FechaVencimientoOperacion
                                                _EncabezadoSeleccionado.PrecioSucio = objResultadoCalculos.PrecioSucio
                                                _EncabezadoSeleccionado.TasaNegociacionYield = objResultadoCalculos.TasaNegociacionYield
                                                _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                                _EncabezadoSeleccionado.ValorNeto = objResultadoCalculos.ValorNeto
                                                _EncabezadoSeleccionado.PorcentajeComision = objResultadoCalculos.PorcentajeComision
                                                _EncabezadoSeleccionado.ValorFlujoIntermedio = objResultadoCalculos.ValorFlujoIntermedio
                                                _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                                _EncabezadoSeleccionado.IVA = objResultadoCalculos.IVA
                                                _EncabezadoSeleccionado.ValorNetoCop = objResultadoCalculos.ValorNetoCop
                                                _EncabezadoSeleccionado.ValorRegresoRepo = objResultadoCalculos.ValorRegresoRepo
                                                _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP
                                                _EncabezadoSeleccionado.ValorComisionCOP = objResultadoCalculos.ValorComisionCOP
                                                _EncabezadoSeleccionado.TasaNeta = objResultadoCalculos.TasaNeta
                                            Else
                                                _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                                _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                                _EncabezadoSeleccionado.ValorComisionCOP = objResultadoCalculos.ValorComisionCOP
                                                _EncabezadoSeleccionado.ValorNeto = objResultadoCalculos.ValorNeto
                                                _EncabezadoSeleccionado.ValorRegresoRepo = objResultadoCalculos.ValorRegresoRepo
                                                _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP
                                                _EncabezadoSeleccionado.ValorNetoCop = objResultadoCalculos.ValorNetoCop
                                                _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                                _EncabezadoSeleccionado.IVA = objResultadoCalculos.IVA
                                                _EncabezadoSeleccionado.TasaNeta = objResultadoCalculos.TasaNeta
                                            End If
                                        ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO Then
                                            If EncabezadoSeleccionado.Tipo = CLASE_RENTAFIJA Then
                                                _EncabezadoSeleccionado.FechaVencimientoOperacion = objResultadoCalculos.FechaVencimientoOperacion
                                                _EncabezadoSeleccionado.PrecioSucio = objResultadoCalculos.PrecioSucio
                                                _EncabezadoSeleccionado.TasaNegociacionYield = objResultadoCalculos.TasaNegociacionYield
                                                _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                                _EncabezadoSeleccionado.ValorNeto = objResultadoCalculos.ValorNeto
                                                _EncabezadoSeleccionado.PorcentajeComision = objResultadoCalculos.PorcentajeComision
                                                _EncabezadoSeleccionado.ValorFlujoIntermedio = objResultadoCalculos.ValorFlujoIntermedio
                                                _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                                _EncabezadoSeleccionado.IVA = objResultadoCalculos.IVA
                                                _EncabezadoSeleccionado.ValorNetoCop = objResultadoCalculos.ValorNetoCop
                                                _EncabezadoSeleccionado.ValorRegresoRepo = objResultadoCalculos.ValorRegresoRepo
                                                _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP
                                                _EncabezadoSeleccionado.ValorComisionCOP = objResultadoCalculos.ValorComisionCOP
                                                _EncabezadoSeleccionado.TasaNeta = objResultadoCalculos.TasaNeta
                                                _EncabezadoSeleccionado.ValorRegresoRepoMoneda = objResultadoCalculos.ValorRegresoRepoMoneda
                                            Else
                                                _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                                _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                                _EncabezadoSeleccionado.ValorComisionCOP = objResultadoCalculos.ValorComisionCOP
                                                _EncabezadoSeleccionado.ValorNeto = objResultadoCalculos.ValorNeto
                                                _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP
                                                _EncabezadoSeleccionado.ValorNetoCop = objResultadoCalculos.ValorNetoCop
                                                _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                                _EncabezadoSeleccionado.IVA = objResultadoCalculos.IVA
                                                _EncabezadoSeleccionado.TasaNeta = objResultadoCalculos.TasaNeta
                                                _EncabezadoSeleccionado.ValorRegresoRepo = objResultadoCalculos.ValorRegresoRepo
                                            End If
                                        ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                                            _EncabezadoSeleccionado.FechaVencimientoOperacion = objResultadoCalculos.FechaVencimientoOperacion
                                            _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                            _EncabezadoSeleccionado.ValorRegresoRepo = objResultadoCalculos.ValorRegresoRepo
                                            _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP

                                        ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                                            _EncabezadoSeleccionado.ValorGiroBruto = objResultadoCalculos.ValorGiroBruto
                                            _EncabezadoSeleccionado.PorcentajeComision = objResultadoCalculos.PorcentajeComision
                                            _EncabezadoSeleccionado.ValorComision = objResultadoCalculos.ValorComision
                                            _EncabezadoSeleccionado.IVA = objResultadoCalculos.IVA
                                            _EncabezadoSeleccionado.ValorNeto = objResultadoCalculos.ValorNeto
                                            _EncabezadoSeleccionado.ValorNetoCop = objResultadoCalculos.ValorNetoCop
                                            _EncabezadoSeleccionado.ValorGiroBrutoCOP = objResultadoCalculos.ValorGiroBrutoCOP
                                            _EncabezadoSeleccionado.ValorComisionCOP = objResultadoCalculos.ValorComisionCOP

                                        End If

                                        logCalcularValores = True
                                        logResultado = True
                                    Else
                                        logResultado = False
                                        If plogMostrarMensajeUsuario Then
                                            mostrarMensaje(objRet.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
            Dim objDiccionarioCombos As New Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos))
            Dim objTipoNegocio As New List(Of CFOperaciones.Operaciones_TiposNegocio)
            Dim objTipoOrigen As New List(Of CFOperaciones.Operaciones_TiposNegocio)
            Dim strNombreCategoria As String = String.Empty
            Dim intContador As Integer = 1

            If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_TIPONEGOCIO") Then
                For Each liCombo In DiccionarioCombosCompleta("NEGOCIOS_TIPONEGOCIO")
                    objTipoNegocio.Add(New CFOperaciones.Operaciones_TiposNegocio With {.ID = intContador,
                                                                                        .CodigoTipoNegocio = liCombo.Codigo,
                                                                                        .NombreTipoNegocio = liCombo.Descripcion})
                    intContador += 1
                Next
            End If

            If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_ORIGEN") Then
                For Each liCombo In DiccionarioCombosCompleta("NEGOCIOS_ORIGEN")
                    objTipoOrigen.Add(New CFOperaciones.Operaciones_TiposNegocio With {.ID = intContador,
                                                                                       .CodigoTipoNegocio = liCombo.Codigo,
                                                                                       .NombreTipoNegocio = liCombo.Descripcion})
                    intContador += 1
                Next
            End If

            For Each dic In DiccionarioCombosCompleta
                strNombreCategoria = dic.Key
                objDiccionarioCombos.Add(strNombreCategoria, dic.Value)
            Next

            DiccionarioCombos = Nothing
            DiccionarioCombos = objDiccionarioCombos

            ListaTipoNegocio = Nothing
            ListaTipoNegocio = objTipoNegocio

            ListaTipoOrigen = Nothing
            ListaTipoOrigen = objTipoOrigen

            ObtenerValoresCombos(True, _EncabezadoSeleccionado)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Obtener la información de los combos.", Me.ToString, "ObtenerInformacionCombosCompletos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub ObtenerValoresRegistroAnterior(ByVal pobjRegistro As CFOperaciones.Operaciones_OtrosNegocios, ByRef pobjRegistroSalvarDatos As CFOperaciones.Operaciones_OtrosNegocios)
        Try
            If Not IsNothing(pobjRegistro) Then
                Dim objNewRegistro As New CFOperaciones.Operaciones_OtrosNegocios

                objNewRegistro.ID = pobjRegistro.ID
                objNewRegistro.Referencia = pobjRegistro.Referencia
                objNewRegistro.Estado = pobjRegistro.Estado
                objNewRegistro.NombreEstado = pobjRegistro.NombreEstado
                objNewRegistro.TipoNegocio = pobjRegistro.TipoNegocio
                objNewRegistro.NombreTipoNegocio = pobjRegistro.NombreTipoNegocio
                objNewRegistro.TipoOrigen = pobjRegistro.TipoOrigen
                objNewRegistro.NombreTipoOrigen = pobjRegistro.NombreTipoOrigen
                objNewRegistro.IDComitente = pobjRegistro.IDComitente
                objNewRegistro.NombreCliente = pobjRegistro.NombreCliente
                objNewRegistro.CodTipoIdentificacionCliente = pobjRegistro.CodTipoIdentificacionCliente
                objNewRegistro.TipoIdentificacionCliente = pobjRegistro.TipoIdentificacionCliente
                objNewRegistro.NumeroDocumentoCliente = pobjRegistro.NumeroDocumentoCliente
                objNewRegistro.IDOrdenante = pobjRegistro.IDOrdenante
                objNewRegistro.NombreOrdenante = pobjRegistro.NombreOrdenante
                objNewRegistro.CodTipoIdentificacionOrdenante = pobjRegistro.CodTipoIdentificacionOrdenante
                objNewRegistro.TipoIdentificacionOrdenante = pobjRegistro.TipoIdentificacionOrdenante
                objNewRegistro.NumeroDocumentoOrdenante = pobjRegistro.NumeroDocumentoOrdenante
                objNewRegistro.DescripcionOrdenante = pobjRegistro.DescripcionOrdenante
                objNewRegistro.Deposito = pobjRegistro.Deposito
                objNewRegistro.IDCuentaDeposito = pobjRegistro.IDCuentaDeposito
                objNewRegistro.DescripcionCuentaDeposito = pobjRegistro.DescripcionCuentaDeposito
                objNewRegistro.IDContraparte = pobjRegistro.IDContraparte
                objNewRegistro.NombreContraparte = pobjRegistro.NombreContraparte
                objNewRegistro.CodTipoIdentificacionContraparte = pobjRegistro.CodTipoIdentificacionContraparte
                objNewRegistro.TipoIdentificacionContraparte = pobjRegistro.TipoIdentificacionContraparte
                objNewRegistro.NumeroDocumentoContraparte = pobjRegistro.NumeroDocumentoContraparte
                objNewRegistro.IDCuentaDepositoContraparte = pobjRegistro.IDCuentaDepositoContraparte
                objNewRegistro.DescripcionCuentaDepositoContraparte = pobjRegistro.DescripcionCuentaDepositoContraparte
                objNewRegistro.ClasificacionInversion = pobjRegistro.ClasificacionInversion
                objNewRegistro.NombreClasificacionInversion = pobjRegistro.NombreClasificacionInversion
                objNewRegistro.IDPagador = pobjRegistro.IDPagador
                objNewRegistro.NombrePagador = pobjRegistro.NombrePagador
                objNewRegistro.CodTipoIdentificacionPagador = pobjRegistro.CodTipoIdentificacionPagador
                objNewRegistro.TipoIdentificacionPagador = pobjRegistro.TipoIdentificacionPagador
                objNewRegistro.NumeroDocumentoPagador = pobjRegistro.NumeroDocumentoPagador
                objNewRegistro.TipoOperacion = pobjRegistro.TipoOperacion
                objNewRegistro.NombreTipoOperacion = pobjRegistro.NombreTipoOperacion
                objNewRegistro.FechaOperacion = pobjRegistro.FechaOperacion
                objNewRegistro.FechaCumplimiento = pobjRegistro.FechaCumplimiento
                objNewRegistro.FechaVencimientoOperacion = pobjRegistro.FechaVencimientoOperacion
                objNewRegistro.Nemotecnico = pobjRegistro.Nemotecnico
                objNewRegistro.ISIN = pobjRegistro.ISIN
                objNewRegistro.FechaEmision = pobjRegistro.FechaEmision
                objNewRegistro.FechaVencimiento = pobjRegistro.FechaVencimiento
                objNewRegistro.Modalidad = pobjRegistro.Modalidad
                objNewRegistro.TasaFacial = pobjRegistro.TasaFacial
                objNewRegistro.IndicadorEconomico = pobjRegistro.IndicadorEconomico
                objNewRegistro.PuntosIndicador = pobjRegistro.PuntosIndicador
                objNewRegistro.TipoTasaFija = pobjRegistro.TipoTasaFija
                objNewRegistro.EsAccion = pobjRegistro.EsAccion
                objNewRegistro.TipoCumplimiento = pobjRegistro.TipoCumplimiento
                objNewRegistro.NombreTipoCumplimiento = pobjRegistro.NombreTipoCumplimiento
                objNewRegistro.Mercado = pobjRegistro.Mercado
                objNewRegistro.NombreMercado = pobjRegistro.NombreMercado
                objNewRegistro.Nominal = pobjRegistro.Nominal
                objNewRegistro.IDMoneda = pobjRegistro.IDMoneda
                objNewRegistro.CodigoMoneda = pobjRegistro.CodigoMoneda
                objNewRegistro.NombreMoneda = pobjRegistro.NombreMoneda
                objNewRegistro.TasaCambioConversion = pobjRegistro.TasaCambioConversion
                objNewRegistro.PrecioSucio = pobjRegistro.PrecioSucio
                objNewRegistro.TasaNegociacionYield = pobjRegistro.TasaNegociacionYield
                objNewRegistro.ValorGiroBruto = pobjRegistro.ValorGiroBruto
                objNewRegistro.TasaPactadaRepo = pobjRegistro.TasaPactadaRepo
                objNewRegistro.Haircut = pobjRegistro.Haircut
                objNewRegistro.PorcentajeComision = pobjRegistro.PorcentajeComision
                objNewRegistro.ValorComision = pobjRegistro.ValorComision
                objNewRegistro.IVA = pobjRegistro.IVA
                objNewRegistro.TipoReteFuente = pobjRegistro.TipoReteFuente
                objNewRegistro.NombreTipoReteFuente = pobjRegistro.NombreTipoReteFuente
                objNewRegistro.Retencion = pobjRegistro.Retencion
                objNewRegistro.ValorNeto = pobjRegistro.ValorNeto
                objNewRegistro.ValorNetoCop = pobjRegistro.ValorNetoCop
                objNewRegistro.ValorRegresoRepo = pobjRegistro.ValorRegresoRepo
                objNewRegistro.Observaciones = pobjRegistro.Observaciones
                objNewRegistro.PaisNegociacion = pobjRegistro.PaisNegociacion
                objNewRegistro.CodigoPaisNegociacion = pobjRegistro.CodigoPaisNegociacion
                objNewRegistro.NombrePaisNegociacion = pobjRegistro.NombrePaisNegociacion
                objNewRegistro.TipoRepo = pobjRegistro.TipoRepo
                objNewRegistro.NombreTipoRepo = pobjRegistro.NombreTipoRepo
                objNewRegistro.UsuarioRegistro = pobjRegistro.UsuarioRegistro
                objNewRegistro.TipoRegistro = pobjRegistro.TipoRegistro
                objNewRegistro.NombreTipoRegistro = pobjRegistro.NombreTipoRegistro
                objNewRegistro.FechaRegistro = pobjRegistro.FechaRegistro
                objNewRegistro.Actualizacion = pobjRegistro.Actualizacion
                objNewRegistro.Usuario = pobjRegistro.Usuario
                objNewRegistro.UsuarioWindows = pobjRegistro.UsuarioWindows
                objNewRegistro.Maquina = pobjRegistro.Maquina
                objNewRegistro.TipoBanRep = pobjRegistro.TipoBanRep
                objNewRegistro.ValorFlujoIntermedio = pobjRegistro.ValorFlujoIntermedio
                objNewRegistro.IDOrdenOrigen = pobjRegistro.IDOrdenOrigen
                objNewRegistro.IDComitenteOtro = pobjRegistro.IDComitenteOtro
                objNewRegistro.Tipo = pobjRegistro.Tipo
                objNewRegistro.DepositoContraparte = pobjRegistro.DepositoContraparte
                objNewRegistro.NroCuentaDepositoContraparte = pobjRegistro.NroCuentaDepositoContraparte
                objNewRegistro.EsFutura = pobjRegistro.EsFutura
                objNewRegistro.TasaNeta = pobjRegistro.TasaNeta
                objNewRegistro.ValorGiroBrutoCOP = pobjRegistro.ValorGiroBrutoCOP
                objNewRegistro.ValorComisionCOP = pobjRegistro.ValorComisionCOP
                objNewRegistro.ValorRegresoRepoMoneda = pobjRegistro.ValorRegresoRepoMoneda
                objNewRegistro.logOperacionAplazada = pobjRegistro.logOperacionAplazada

                pobjRegistroSalvarDatos = objNewRegistro
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", Me.ToString(), "ObtenerValoresRegistroAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub LimpiarDatosTipoNegocio(ByVal pobjRegistroSeleccionado As CFOperaciones.Operaciones_OtrosNegocios, Optional ByVal pstrTipo As String = "")
        Try
            If Not IsNothing(pobjRegistroSeleccionado) Then
                logCalcularValores = False

                If pstrTipo = LIMPIARDATOS_TIPONEGOCIO Then
                    pobjRegistroSeleccionado.TipoOperacion = String.Empty
                    If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.CLASIFICACIONINVERSION.ToString) = False Then
                        pobjRegistroSeleccionado.ClasificacionInversion = String.Empty
                    End If
                    If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.MERCADO.ToString) = False Then
                        pobjRegistroSeleccionado.Mercado = String.Empty
                    End If
                    If DiccionarioHabilitarCampos(OPCIONES_HABILITARCAMPOS.TIPOREPO.ToString) = False Then
                        pobjRegistroSeleccionado.TipoRepo = String.Empty
                    End If

                    ComitenteSeleccionado = Nothing
                    If BorrarCliente Then
                        BorrarCliente = False
                    End If
                    BorrarCliente = True

                    NemotecnicoSeleccionado = Nothing
                    If BorrarEspecie Then
                        BorrarEspecie = False
                    End If
                    BorrarEspecie = True

                    pobjRegistroSeleccionado.Nemotecnico = String.Empty

                    Dim objListaReceptores As New List(Of CFOperaciones.Operaciones_ReceptoresOtrosNegocios)
                    ListaReceptores = Nothing
                    ListaReceptores = objListaReceptores
                    ReceptorSeleccionado = Nothing
                    pobjRegistroSeleccionado.TipoRepo = String.Empty
                End If

                If pstrTipo = LIMPIARDATOS_TIPO Then
                    NemotecnicoSeleccionado = Nothing
                    If BorrarEspecie Then
                        BorrarEspecie = False
                    End If
                    BorrarEspecie = True

                    pobjRegistroSeleccionado.Nemotecnico = String.Empty
                ElseIf pstrTipo = LIMPIARDATOS_NEMOTECNICO Or pstrTipo = LIMPIARDATOS_TIPONEGOCIO Then
                    pobjRegistroSeleccionado.Nominal = 0
                    pobjRegistroSeleccionado.PrecioSucio = 0
                Else
                    Dim logBorrarprecio As Boolean = True

                    If dblPrecioConsultado = pobjRegistroSeleccionado.PrecioSucio And pobjRegistroSeleccionado.Mercado = MERCADO_SECUNDARIO Then
                        logBorrarprecio = False
                    End If

                    If logBorrarprecio Then
                        pobjRegistroSeleccionado.PrecioSucio = 0
                    End If
                End If

                pobjRegistroSeleccionado.TasaNegociacionYield = 0

                pobjRegistroSeleccionado.TasaPactadaRepo = 0
                pobjRegistroSeleccionado.ValorGiroBruto = 0
                pobjRegistroSeleccionado.Haircut = 0
                pobjRegistroSeleccionado.PorcentajeComision = 0
                pobjRegistroSeleccionado.ValorComision = 0
                pobjRegistroSeleccionado.IVA = 0
                pobjRegistroSeleccionado.Retencion = 0
                pobjRegistroSeleccionado.FechaVencimientoOperacion = Nothing
                pobjRegistroSeleccionado.ValorRegresoRepo = 0
                pobjRegistroSeleccionado.ValorNeto = 0
                pobjRegistroSeleccionado.ValorNetoCop = 0
                pobjRegistroSeleccionado.TasaNeta = 0
                pobjRegistroSeleccionado.ValorComisionCOP = 0
                pobjRegistroSeleccionado.ValorGiroBrutoCOP = 0
                pobjRegistroSeleccionado.ValorRegresoRepoMoneda = 0

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

    ''' <summary>
    ''' CCM20151107: Validar que la fecha de cierre del portafolio del comitente seleccionado no sea mayor o igual a la de la operación
    ''' </summary>
    ''' 
    Public Async Function ValidarFechaCierrePortafolio(Optional ByVal plogMostrarAdvertencia As Boolean = True) As Task(Of Boolean)
        Dim logResultadoValidacion As Boolean = False
        Dim dtmFechaOperacion As Date

        Try
            If Not IsNothing(EncabezadoSeleccionado) Then
                If mdtmFechaCierrePortafolio.Equals(MDTM_FECHA_CIERRE_SIN_ACTUALIZAR) Then
                    mdtmFechaCierrePortafolio = Await ObtenerFechaCierrePortafolio(EncabezadoSeleccionado.IDComitente)
                End If

                If Not IsNothing(mdtmFechaCierrePortafolio) And Not IsNothing(EncabezadoSeleccionado.FechaOperacion) Then
                    dtmFechaOperacion = New Date(EncabezadoSeleccionado.FechaOperacion.Value.Year, EncabezadoSeleccionado.FechaOperacion.Value.Month, EncabezadoSeleccionado.FechaOperacion.Value.Day) ' Eliminar la hora
                    If mdtmFechaCierrePortafolio >= dtmFechaOperacion Then
                        If plogMostrarAdvertencia Then
                            A2Utilidades.Mensajes.mostrarMensaje("El portafolio del cliente " & EncabezadoSeleccionado.IDComitente.Trim & "-" & EncabezadoSeleccionado.NombreCliente & " está cerrado para la fecha de la operacion (Fecha de cierre " & Year(mdtmFechaCierrePortafolio) & "/" & Month(mdtmFechaCierrePortafolio) & "/" & Day(mdtmFechaCierrePortafolio) & ", Fecha de la operación " & Year(EncabezadoSeleccionado.FechaOperacion) & "/" & Month(EncabezadoSeleccionado.FechaOperacion) & "/" & Day(EncabezadoSeleccionado.FechaOperacion) & "). ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
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
                If Not IsNothing(_EncabezadoSeleccionado.FechaOperacion) And Not IsNothing(_EncabezadoSeleccionado.IDMoneda) Then
                    Dim objRet As InvokeOperation(Of Double)

                    objRet = Await dcProxy.Operaciones_OtrosNegociosConsultarTRMMonedaSync(_EncabezadoSeleccionado.FechaOperacion, _EncabezadoSeleccionado.IDMoneda, Program.Usuario, Program.HashConexion).AsTask

                    If Not objRet Is Nothing Then
                        If objRet.HasError Then
                            If objRet.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ObtenerTRMMoneda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            End If

                            objRet.MarkErrorAsHandled()
                        Else
                            If Not IsNothing(objRet.Value) Then
                                logCalcularValores = False
                                _EncabezadoSeleccionado.TasaCambioConversion = objRet.Value
                                logCalcularValores = True
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

    Private Sub consultarCuentasDepositoEntidad(ByVal pintIDEntidad As Integer, Optional ByVal pstrUserState As String = "")
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                If Not IsNothing(dcProxy.Operaciones_EntidadesCuentasDepositos) Then
                    dcProxy.Operaciones_EntidadesCuentasDepositos.Clear()
                End If
                ListaCuentasDepositoContraparte = Nothing

                dcProxy.Load(dcProxy.Operaciones_EntidadesCuentasDepositoQuery(False, pintIDEntidad, False, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDepositoEntidad, pstrUserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar las cuentas deposito de la entidad.", Me.ToString, "consultarCuentasDepositoEntidad", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Async Function consultarCuentasDepositoEntidadAsincronico(ByVal pintIDEntidad As Integer, Optional ByVal pstrUserState As String = "") As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Try
            If logNuevoRegistro Or logEditarRegistro Then
                Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_EntidadesCuentasDeposito)

                objRet = Await dcProxy.Load(dcProxy.Operaciones_EntidadesCuentasDepositoQuery(False, pintIDEntidad, False, String.Empty, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al calcular los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "consultarCuentasDepositoEntidadAsincronico", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        ObtenerValoresRespuestaCuentasDepositoEntidad(objRet.Entities.ToList, pstrUserState)
                        logResultado = True
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar las cuentas deposito de la entidad.", Me.ToString, "consultarCuentasDepositoEntidadAsincronico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

        IsBusyCalculos = False
        Return (logResultado)
    End Function

    Public Sub ObtenerClaseEspecies()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                    ClaseEspecies = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
                ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO Then
                    If _EncabezadoSeleccionado.Tipo = CLASE_ACCIONES Then
                        ClaseEspecies = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
                    Else
                        ClaseEspecies = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                    End If
                ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                    ClaseEspecies = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                    If _EncabezadoSeleccionado.Tipo = CLASE_ACCIONES Then
                        ClaseEspecies = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
                    Else
                        ClaseEspecies = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                    End If
                ElseIf _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                    ClaseEspecies = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                Else
                    ClaseEspecies = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el valor de la clase de especie.", Me.ToString(), "ObtenerClaseEspecies", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerMonedaLocal()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If Not IsNothing(_EncabezadoSeleccionado.PaisNegociacion) Then
                    If _EncabezadoSeleccionado.PaisNegociacion = intPaisLocal Then
                        If _EncabezadoSeleccionado.IDMoneda <> intMonedaLocal Then
                            If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_MONEDALOCAL") Then
                                logPosicionarCantidad = False
                                _EncabezadoSeleccionado.IDMoneda = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.ID

                                _EncabezadoSeleccionado.NombreMoneda = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.Descripcion
                                _EncabezadoSeleccionado.CodigoMoneda = DiccionarioCombos("NEGOCIOS_MONEDALOCAL").First.Codigo
                            End If
                        End If
                    Else
                        If _EncabezadoSeleccionado.IDMoneda = intMonedaLocal Then
                            logPosicionarCantidad = False
                            _EncabezadoSeleccionado.IDMoneda = Nothing

                            _EncabezadoSeleccionado.NombreMoneda = String.Empty
                            _EncabezadoSeleccionado.CodigoMoneda = String.Empty
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el valor por defecto de la moneda local.", Me.ToString(), "ObtenerMonedaLocal", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerDecimales()
        Try
            Dim intCantidadDecimales As Integer = 6
            Dim intCantidadDecimalesEspeciales As Integer = 2
            Dim intCantidadDecimalesPorcentajeComision As Integer = 3

            If Not IsNothing(DiccionarioCombosCompleta) Then
                If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_CANTIDADDECIMALES") Then
                    If DiccionarioCombosCompleta("NEGOCIOS_CANTIDADDECIMALES").Count > 0 Then
                        intCantidadDecimales = DiccionarioCombosCompleta("NEGOCIOS_CANTIDADDECIMALES").First.ID
                    End If
                End If
            End If

            If Not IsNothing(DiccionarioCombosCompleta) Then
                If DiccionarioCombosCompleta.ContainsKey("OTROSNEGOCIOS_CANTIDADDECIMALESESPECIALES") Then
                    If DiccionarioCombosCompleta("OTROSNEGOCIOS_CANTIDADDECIMALESESPECIALES").Count > 0 Then
                        intCantidadDecimalesEspeciales = DiccionarioCombosCompleta("OTROSNEGOCIOS_CANTIDADDECIMALESESPECIALES").First.ID
                    End If
                End If
            End If

            If Not IsNothing(DiccionarioCombosCompleta) Then
                If DiccionarioCombosCompleta.ContainsKey("OTROSNEGOCIOS_CANTIDADDECIMALES_PORCOMISION") Then
                    If DiccionarioCombosCompleta("OTROSNEGOCIOS_CANTIDADDECIMALES_PORCOMISION").Count > 0 Then
                        intCantidadDecimalesPorcentajeComision = DiccionarioCombosCompleta("OTROSNEGOCIOS_CANTIDADDECIMALES_PORCOMISION").First.ID
                    End If
                End If
            End If

            FormatoCamposDecimales = String.Format("{0}", intCantidadDecimales)
            FormatoCamposNumericos = String.Format("{0}", intCantidadDecimales)

            FormatoCamposDecimalesEspeciales = String.Format("{0}", intCantidadDecimalesEspeciales)
            FormatoCamposNumericosEspeciales = String.Format("{0}", intCantidadDecimalesEspeciales)

            FormatoCamposDecimalesPorcentajeComision = String.Format("{0}", intCantidadDecimalesPorcentajeComision)
            FormatoCamposNumericosPorcentajeComision = String.Format("{0}", intCantidadDecimalesPorcentajeComision)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la valición del registro.", Me.ToString, "ObtenerDecimales", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Async Function ValidarEstadoOperaciones_OtrosNegocios(ByVal objRegistroSelected As CFOperaciones.Operaciones_OtrosNegocios, ByVal pstrAccion As String, ByVal plogOperacionAplazada As System.Nullable(Of System.Boolean)) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFOperaciones.Operaciones_RespuestaValidacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Operaciones_RespuestaValidacions.Clear()

            objRet = Await dcProxy.Load(dcProxy.Operaciones_OtrosNegociosValidarEstadoSyncQuery(_EncabezadoSeleccionado.ID, pstrAccion, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion, plogOperacionAplazada)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización del registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización del registro.", Me.ToString(), "ValidarEstadoOperaciones_OtrosNegocios", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                    IsBusy = False
                Else
                    Dim objListaResultadoValidacion As List(Of CFOperaciones.Operaciones_RespuestaValidacion) = objRet.Entities.ToList

                    If objListaResultadoValidacion.Count > 0 Then

                        If objListaResultadoValidacion.Where(Function(i) i.DetieneIngreso).Count > 0 Then
                            logResultado = False
                            mostrarMensaje(objListaResultadoValidacion.Where(Function(i) i.DetieneIngreso).First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            logResultado = True
                        End If
                    End If
                End If
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al validar el estado del registro.", Me.ToString(), "ValidarEstadoOperaciones_OtrosNegocios", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Async Function ObtenerPrecioMercado() As Task
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If Not IsNothing(_EncabezadoSeleccionado.FechaOperacion) And
                    Not String.IsNullOrEmpty(_EncabezadoSeleccionado.Nemotecnico) And
                    Not String.IsNullOrEmpty(_EncabezadoSeleccionado.ISIN) Then

                    Dim logConsultarPrecioMercado As Boolean = False

                    If DiccionarioCombosCompleta.ContainsKey("NEGOCIOS_HABILITARCONSULTAPRECIOMERCADO") Then
                        If DiccionarioCombosCompleta("NEGOCIOS_HABILITARCONSULTAPRECIOMERCADO").Where(Function(i) i.Codigo.ToUpper = "SI").Count > 0 Then
                            logConsultarPrecioMercado = True
                        End If
                    End If

                    If logConsultarPrecioMercado Then
                        If _EncabezadoSeleccionado.Mercado = MERCADO_SECUNDARIO Then
                            Dim objRet As InvokeOperation(Of Double)

                            objRet = Await dcProxy.Operaciones_OtrosNegociosConsultarPrecioMercadoSync(_EncabezadoSeleccionado.Nemotecnico, _EncabezadoSeleccionado.ISIN, _EncabezadoSeleccionado.FechaOperacion, Program.Usuario, Program.HashConexion).AsTask

                            If Not objRet Is Nothing Then
                                If objRet.HasError Then
                                    If objRet.Error Is Nothing Then
                                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ObtenerPrecioMercado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                    End If

                                    objRet.MarkErrorAsHandled()
                                    dblPrecioConsultado = 0
                                Else
                                    If Not IsNothing(objRet.Value) Then
                                        If objRet.Value > 0 Then
                                            logCalcularValores = False
                                            dblPrecioConsultado = objRet.Value
                                            _EncabezadoSeleccionado.PrecioSucio = objRet.Value
                                            If _EncabezadoSeleccionado.Nominal > 0 Then
                                                strTipoCalculo = TIPOCALCULOS_MOTOR.PRECIO.ToString
                                                Await CalcularValorRegistro()
                                            End If

                                            logCalcularValores = True
                                        Else
                                            dblPrecioConsultado = 0
                                        End If
                                    Else
                                        dblPrecioConsultado = 0
                                    End If
                                End If
                            End If
                        Else
                            dblPrecioConsultado = 0
                        End If
                    Else
                        dblPrecioConsultado = 0
                    End If
                Else
                    dblPrecioConsultado = 0
                End If
            Else
                dblPrecioConsultado = 0
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ObtenerPrecioMercado. ", Me.ToString(), "ObtenerPrecioMercado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
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

    Public Function VerificarTipoCalculo(ByVal pobjRegistro As CFOperaciones.Operaciones_OtrosNegocios, ByVal pstrTipoCalculoConsolidado As String, ByVal pstrCampoModificado As String) As String
        Dim strRetorno As String = String.Empty

        Try
            If Not IsNothing(pobjRegistro) Then
                Dim logContieneTexto As Boolean = False
                For Each li In pstrTipoCalculoConsolidado.Split("_")
                    If li = pstrCampoModificado Then
                        logContieneTexto = True
                        Exit For
                    End If
                Next

                If logContieneTexto = False Then
                    Dim intCantidadSeparador As Integer = pstrTipoCalculoConsolidado.Split("_").Count
                    If intCantidadSeparador >= 2 Then
                        Dim strTextoTemporal As String = pstrTipoCalculoConsolidado.Substring(0, pstrTipoCalculoConsolidado.IndexOf("_") + 1)
                        pstrTipoCalculoConsolidado = Replace(pstrTipoCalculoConsolidado, strTextoTemporal, String.Empty)

                        If (pstrTipoCalculoConsolidado = TIPOCALCULOS_MOTOR.PRECIO.ToString _
                            Or pstrTipoCalculoConsolidado = TIPOCALCULOS_MOTOR.TASANEGOCIACION.ToString _
                            Or pstrTipoCalculoConsolidado = TIPOCALCULOS_MOTOR.VALORGIRO.ToString) _
                        And (pstrCampoModificado = TIPOCALCULOS_MOTOR.PRECIO.ToString _
                            Or pstrCampoModificado = TIPOCALCULOS_MOTOR.TASANEGOCIACION.ToString _
                            Or pstrCampoModificado = TIPOCALCULOS_MOTOR.VALORGIRO.ToString) Then

                            strRetorno = strTextoTemporal & pstrCampoModificado
                        Else
                            strRetorno = String.Format("{0}_{1}", pstrTipoCalculoConsolidado, pstrCampoModificado)
                        End If

                        If strRetorno = String.Format("{0}_{1}", TIPOCALCULOS_MOTOR.PORCENTAJECOMISION.ToString, TIPOCALCULOS_MOTOR.VALORCOMISION.ToString) Or
                            strRetorno = String.Format("{0}_{1}", TIPOCALCULOS_MOTOR.VALORCOMISION.ToString, TIPOCALCULOS_MOTOR.PORCENTAJECOMISION.ToString) Then
                            strRetorno = String.Format("{0}{1}", strTextoTemporal, pstrCampoModificado)
                        End If
                    Else
                        If String.IsNullOrEmpty(pstrTipoCalculoConsolidado) Then
                            strRetorno = pstrCampoModificado
                        Else
                            If Not String.IsNullOrEmpty(pstrCampoModificado) Then
                                If pstrTipoCalculoConsolidado = TIPOCALCULOS_MOTOR.PRECIO.ToString _
                                    Or pstrTipoCalculoConsolidado = TIPOCALCULOS_MOTOR.TASANEGOCIACION.ToString _
                                    Or pstrTipoCalculoConsolidado = TIPOCALCULOS_MOTOR.VALORGIRO.ToString Then
                                    If pstrCampoModificado = TIPOCALCULOS_MOTOR.PRECIO.ToString _
                                        Or pstrCampoModificado = TIPOCALCULOS_MOTOR.TASANEGOCIACION.ToString _
                                        Or pstrCampoModificado = TIPOCALCULOS_MOTOR.VALORGIRO.ToString Then
                                        strRetorno = pstrCampoModificado
                                    Else
                                        strRetorno = String.Format("{0}_{1}", pstrTipoCalculoConsolidado, pstrCampoModificado)
                                    End If
                                Else
                                    strRetorno = String.Format("{0}_{1}", pstrTipoCalculoConsolidado, pstrCampoModificado)
                                End If
                            Else
                                strRetorno = pstrTipoCalculoConsolidado
                            End If
                        End If
                    End If
                Else
                    strRetorno = pstrTipoCalculoConsolidado
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
        Return strRetorno
    End Function

    Public Sub consultarComitentesContraparte(ByVal pintIDContraparte As Nullable(Of Integer), Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(pintIDContraparte) Then
                If Not IsNothing(mdcProxyUtilidad.ItemCombos) Then
                    mdcProxyUtilidad.ItemCombos.Clear()
                End If

                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosCondicionalQuery("OTROSNEGOCIOS_COMITENTES_PP", "", pintIDContraparte, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosOYDContraparte, pstrUserState)
            Else
                ListaComitentesContraparte = Nothing
                ListaCuentasDepositoContraparteOTC = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los clientes comitentes contraparte PP.", Me.ToString, "consultarComitentesContraparte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Function VerificarTipoFuncionalidadTipoOrigen(ByVal pstrTipoOrigen As String, ByVal pobjTipoValidar As TIPOSORIGEN_TIPOMANEJO) As Boolean
        Dim logTieneFuncionalidad As Boolean = False
        Try
            If Not IsNothing(DiccionarioCombosCompleta) Then
                If DiccionarioCombosCompleta.ContainsKey(pobjTipoValidar.ToString) Then
                    If DiccionarioCombosCompleta(pobjTipoValidar.ToString).Where(Function(i) i.Descripcion = pstrTipoOrigen And i.Codigo = "SI").Count > 0 Then
                        logTieneFuncionalidad = True
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al validar el tipo de funcionalidad.", Me.ToString, "VerificarTipoFuncionalidadTipoOrigen", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Return logTieneFuncionalidad
    End Function

    Public Function VerificarOrigenPrincipalSecundario(ByVal pstrTipoOrigenPrincipal As String, ByVal pstrTipoOrigenSeleccionado As String) As Boolean
        Dim logTieneFuncionalidad As Boolean = False
        If pstrTipoOrigenPrincipal = pstrTipoOrigenSeleccionado Then
            logTieneFuncionalidad = True
        Else
            If Not IsNothing(DiccionarioCombosCompleta) Then
                If DiccionarioCombosCompleta.ContainsKey("HOMOLOGACION_TIPOORIGEN") Then
                    If DiccionarioCombosCompleta("HOMOLOGACION_TIPOORIGEN").Where(Function(i) i.Codigo = pstrTipoOrigenPrincipal And i.Descripcion = pstrTipoOrigenSeleccionado).Count > 0 Then
                        logTieneFuncionalidad = True
                    End If
                End If
            End If
        End If
        Return logTieneFuncionalidad
    End Function

    Public Async Sub AplazarOperacion()
        Try
            'JP20190626 Se comenta porque genera error de visualización cuando de presiona el botón aplazar la primera vez que carga la pantalla en modo forma.
            If logCargoForma = False Then
                ViewOperacionesOtrosNegocios.GridEdicion.Children.Add(viewFormaOperacionesOtrosNegocios)
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
                    TipoProductoFiltroCliente = "TOD"

                    Await validarEstadoRegistro(OPCION_APLAZAR)

                Else
                    'MyBase.RetornarValorEdicionNavegacion()
                    mostrarMensaje("No se ha seleccionado ningun registro para aplazar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
                mostrarMensaje("No se ha seleccionado ningun registro para aplazar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al validar el tipo de funcionalidad.", Me.ToString, "AplazarOperacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub buscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If lo.HasError = False Then
                If lo.Entities.ToList.Count > 0 Then
                    Me.NemotecnicoSeleccionado = lo.Entities.ToList.FirstOrDefault
                Else
                    Me.NemotecnicoSeleccionado = Nothing
                    _EncabezadoSeleccionado.Nemotecnico = String.Empty
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie del registro", Me.ToString(), "buscarEspecieCompleted", Program.TituloSistema, Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie del registro", Me.ToString(), "buscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerOrdenantes(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        Try
            If Not lo.HasError Then
                ObtenerValoresRespuestaOrdenante(lo.Entities.ToList, lo.UserState)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes",
                                                 Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes",
                                                 Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ObtenerValoresRespuestaOrdenante(ByVal pobjListaOrdenante As List(Of OYDUtilidades.BuscadorOrdenantes), ByVal pstrUserState As String)
        Try
            Dim objOrdenante As String = String.Empty
            If (pstrUserState = OPCION_EDITAR Or pstrUserState = OPCION_APLAZAR) Then
                objOrdenante = EncabezadoSeleccionado.IDOrdenante
            End If

            ListaOrdenantes = pobjListaOrdenante

            If (pstrUserState = OPCION_EDITAR Or pstrUserState = OPCION_APLAZAR) Then
                If _ListaOrdenantes.Where(Function(i) i.IdOrdenante = objOrdenante).Count > 0 Then
                    OrdenanteSeleccionado = _ListaOrdenantes.Where(Function(i) i.IdOrdenante = objOrdenante).FirstOrDefault
                End If
            ElseIf logNuevoRegistro Or logEditarRegistro Then
                If Not IsNothing(DiccionarioCombosCompleta) Then
                    If DiccionarioCombosCompleta.ContainsKey("COLOCARORDENANTELIDER") Then
                        If DiccionarioCombosCompleta("COLOCARORDENANTELIDER").Where(Function(i) i.Codigo.ToUpper = "SI").Count > 0 Then
                            If _ListaOrdenantes.Where(Function(i) i.Lider).Count > 0 Then
                                OrdenanteSeleccionado = _ListaOrdenantes.Where(Function(i) i.Lider).First()
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes",
                                                 Me.ToString(), "ObtenerValoresRespuestaOrdenante", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Private Sub ObtenerValoresRespuestaCuentasDeposito(ByVal pobjListaCuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito), ByVal pstrUserState As String)
        Try
            Dim objNroCuenta As Integer? = 0
            Dim strDeposito As String = String.Empty

            If (pstrUserState = OPCION_EDITAR Or pstrUserState = OPCION_APLAZAR) And Not IsNothing(_EncabezadoSeleccionadoAnterior) Then
                strDeposito = _EncabezadoSeleccionadoAnterior.Deposito
                objNroCuenta = _EncabezadoSeleccionadoAnterior.IDCuentaDeposito
            End If

            If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                If pobjListaCuentasDeposito.ToList.Where(Function(I) I.Deposito <> "V").Count > 0 Then
                    ListaCuentasDeposito = pobjListaCuentasDeposito.ToList.Where(Function(I) I.Deposito <> "V").ToList

                    If ListaCuentasDeposito.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = IIf(IsNothing(objNroCuenta), -1, objNroCuenta)).Count > 0 Then
                        CtaDepositoSeleccionada = ListaCuentasDeposito.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = IIf(IsNothing(objNroCuenta), -1, objNroCuenta)).FirstOrDefault
                    Else
                        If ListaCuentasDeposito.Where(Function(i) i.Deposito = "D").Count > 0 Then
                            CtaDepositoSeleccionada = ListaCuentasDeposito.Where(Function(i) i.Deposito = "D").First
                        End If
                    End If
                Else
                    ListaCuentasDeposito = Nothing
                End If
            Else

                ListaCuentasDeposito = pobjListaCuentasDeposito
                If ListaCuentasDeposito.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = IIf(IsNothing(objNroCuenta), -1, objNroCuenta)).Count > 0 Then
                    CtaDepositoSeleccionada = ListaCuentasDeposito.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = IIf(IsNothing(objNroCuenta), -1, objNroCuenta)).FirstOrDefault
                Else
                    CtaDepositoSeleccionada = Nothing
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas depósito",
                                                 Me.ToString(), "ObtenerValoresRespuestaCuentasDeposito", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ObtenerValoresRespuestaCuentasDepositoEntidad(ByVal pobjListaCuentasDepositoEntidad As List(Of CFOperaciones.Operaciones_EntidadesCuentasDeposito), ByVal pstrUserState As String)
        Try
            Dim intIDCuentaDepositoEntidad As Integer? = 0

            If (pstrUserState = OPCION_EDITAR Or pstrUserState = OPCION_APLAZAR) And Not IsNothing(_EncabezadoSeleccionadoAnterior) Then
                If Not IsNothing(_EncabezadoSeleccionadoAnterior.IDCuentaDepositoContraparte) Then
                    intIDCuentaDepositoEntidad = _EncabezadoSeleccionadoAnterior.IDCuentaDepositoContraparte
                End If
            End If

            ListaCuentasDepositoContraparte = pobjListaCuentasDepositoEntidad
            If ListaCuentasDepositoContraparte.Where(Function(i) i.ID = intIDCuentaDepositoEntidad).Count > 0 Then
                CtaDepositoSeleccionadaContraparte = ListaCuentasDepositoContraparte.Where(Function(i) i.ID = intIDCuentaDepositoEntidad).FirstOrDefault
            Else
                If ListaCuentasDepositoContraparte.Count > 1 Then
                    CtaDepositoSeleccionadaContraparte = Nothing
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas depósito",
                                                 Me.ToString(), "ObtenerValoresRespuestaCuentasDepositoEntidad", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Private Sub TerminoTraerCuentasDeposito(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        Try
            If Not lo.HasError Then
                ObtenerValoresRespuestaCuentasDeposito(lo.Entities.ToList, lo.UserState)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito",
                                                 Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito",
                                                 Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerCuentasDepositoContraparte(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        Try
            If Not lo.HasError Then
                Dim objNroCuenta As Integer? = 0
                Dim strDeposito As String = String.Empty

                If lo.UserState = OPCION_EDITAR Then
                    strDeposito = _EncabezadoSeleccionado.DepositoContraparte
                    objNroCuenta = _EncabezadoSeleccionado.NroCuentaDepositoContraparte
                End If

                ListaCuentasDepositoContraparteOTC = Nothing

                If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                    If lo.Entities.ToList.Where(Function(I) I.Deposito <> "V").Count > 0 Then
                        ListaCuentasDepositoContraparteOTC = lo.Entities.ToList.Where(Function(I) I.Deposito <> "V").ToList

                        If ListaCuentasDepositoContraparteOTC.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = IIf(IsNothing(objNroCuenta), -1, objNroCuenta)).Count > 0 Then
                            CtaDepositoSeleccionadaContraparteOTC = ListaCuentasDepositoContraparteOTC.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = IIf(IsNothing(objNroCuenta), -1, objNroCuenta)).FirstOrDefault
                        Else
                            If ListaCuentasDepositoContraparteOTC.Where(Function(i) i.Deposito = "D").Count > 0 Then
                                CtaDepositoSeleccionadaContraparteOTC = ListaCuentasDepositoContraparteOTC.Where(Function(i) i.Deposito = "D").First
                            End If
                        End If
                    Else
                        ListaCuentasDepositoContraparteOTC = Nothing
                    End If
                Else

                    ListaCuentasDepositoContraparteOTC = lo.Entities.ToList
                    If ListaCuentasDepositoContraparteOTC.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = IIf(IsNothing(objNroCuenta), -1, objNroCuenta)).Count > 0 Then
                        CtaDepositoSeleccionadaContraparteOTC = ListaCuentasDepositoContraparteOTC.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = IIf(IsNothing(objNroCuenta), -1, objNroCuenta)).FirstOrDefault
                    Else
                        CtaDepositoSeleccionadaContraparteOTC = Nothing
                    End If

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                                 Me.ToString(), "TerminoTraerCuentasDepositoContraparte", Program.TituloSistema, Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                                 Me.ToString(), "TerminoTraerCuentasDepositoContraparte", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Async Sub TerminoTraerCuentasDepositoEntidad(ByVal lo As LoadOperation(Of CFOperaciones.Operaciones_EntidadesCuentasDeposito))
        Try
            If Not lo.HasError Then
                ObtenerValoresRespuestaCuentasDepositoEntidad(lo.Entities.ToList, lo.UserState)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito de la contraparte",
                                                 Me.ToString(), "TerminoTraerCuentasDepositoEntidad", Program.TituloSistema, Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito de la contraparte",
                                                 Me.ToString(), "TerminoTraerCuentasDepositoEntidad", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    'Private Sub PosicionarControlValidaciones(ByVal plogOrdenOriginal As Boolean, ByVal pobjRegistroSelected As CFOperaciones.Operaciones_OtrosNegocios, ByVal pobjViewOperaciones_OtrosNegocios As OrdenesPLUSView, ByVal pobjViewCruzada As OrdenesCruzadasOYDPLUSView)
    '    Try
    '        'Se busca el control para llevarle el foco al control que se requiere
    '        Dim strMensaje As String = strMensajeValidacion.ToLower

    '        If strMensaje.Contains("- receptor") Then
    '            If plogOrdenOriginal Then BuscarControlValidacion(pobjViewOperaciones_OtrosNegocios, "cboReceptores") Else BuscarControlValidacionCruzada(pobjViewCruzada, "cboReceptores")
    '            Exit Sub
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al terminar de mostrar el mensaje de validación.", Me.ToString, "TerminoValidacionMensajeOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '        IsBusy = False
    '    End Try
    'End Sub

    Private Sub RecargarOrdenDespuesGuardado(ByVal pstrMensaje As String)
        Try
            logCancelarRegistro = False
            logEditarRegistro = False
            logDuplicarRegistro = False
            logNuevoRegistro = False

            'Esto se realiza para habilitar los botones de navegación llamando el metodo TERMINOSUBMITCHANGED
            If dcProxy1.Operaciones_OtrosNegocios.Where(Function(i) i.ID = _EncabezadoSeleccionado.ID).Count = 0 Then
                dcProxy1.Operaciones_OtrosNegocios.Add(_EncabezadoSeleccionado)
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
                                HabilitaBoton = True
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
                        Case "LIQXDIFE"
                            If objResultado.DialogResult Then
                                IsBusy = True
                                AplicarLiqxDife(objResultado.Observaciones)
                            Else
                                HabilitaBoton = True
                                IsBusy = False
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

    Private Sub TerminoTraerCodigosOYDContraparte(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                Dim strComitenteOtro As String = String.Empty

                If lo.UserState = OPCION_EDITAR Then
                    strComitenteOtro = _EncabezadoSeleccionado.IDComitenteOtro
                End If

                ListaComitentesContraparte = lo.Entities.ToList

                If lo.UserState = OPCION_EDITAR Then
                    logCalcularValores = False
                    _EncabezadoSeleccionado.IDComitenteOtro = Nothing

                    If ListaComitentesContraparte.Where(Function(i) i.ID = strComitenteOtro).Count > 0 Then
                        _EncabezadoSeleccionado.IDComitenteOtro = strComitenteOtro
                    End If
                    consultarCuentasDepositoContraparte(_EncabezadoSeleccionado.IDComitenteOtro, OPCION_EDITAR)
                    logCalcularValores = True
                Else
                    ListaCuentasDepositoContraparteOTC = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito de la contraparte", _
                                                 Me.ToString(), "TerminoTraerCuentasDepositoEntidad", Program.TituloSistema, Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito de la contraparte", _
                                                 Me.ToString(), "TerminoTraerCuentasDepositoEntidad", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Funciones Generales"


    Public Sub PrRemoverValoresDic(ByRef pobjDiccionario As Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos)), ByVal pstrArray As String())
        Try
            For i = 0 To pstrArray.Count - 1
                If pobjDiccionario.ContainsKey(pstrArray(i)) Then pobjDiccionario.Remove(pstrArray(i))
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de los combos.", _
                                 Me.ToString(), "PrRemoverValoresDic", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ExtraerListaPorCategoria(pobjDiccionario As Dictionary(Of String, List(Of CFOperaciones.Operaciones_Combos)), pstrTopico As String, pstrCategoria As String) As List(Of CFOperaciones.Operaciones_Combos)
        ExtraerListaPorCategoria = New List(Of CFOperaciones.Operaciones_Combos)
        Try
            If pobjDiccionario.ContainsKey(pstrTopico) Then
                Dim objRetorno = From item In pobjDiccionario(pstrTopico)
                                 Select New CFOperaciones.Operaciones_Combos With {.ID = item.ID, _
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
            Return New List(Of CFOperaciones.Operaciones_Combos)
        End Try
    End Function


#End Region

#Region "Eventos"

    Private Async Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try
            If logCalcularValores Then
                Select Case e.PropertyName.ToLower()
                    Case "tiponegocio"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoNegocio) Then
                            IsBusy = True
                            LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_TIPONEGOCIO)
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
                            ObtenerValoresCombos(False, _EncabezadoSeleccionado, OPCION_TIPONEGOCIO)
                            ObtenerClaseEspecies()
                            If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                                If DiccionarioCombos.ContainsKey("NEGOCIOS_MONEDADEFECTOINTERNACIONAL") Then
                                    logPosicionarCantidad = False
                                    _EncabezadoSeleccionado.IDMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTOINTERNACIONAL").First.ID

                                    _EncabezadoSeleccionado.NombreMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTOINTERNACIONAL").First.Descripcion
                                    _EncabezadoSeleccionado.CodigoMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTOINTERNACIONAL").First.Codigo

                                    Await ObtenerTRMMoneda()
                                    HabilitarCamposRegistro(_EncabezadoSeleccionado)
                                End If
                            End If
                            IsBusy = False
                        End If
                    Case "tipooperacion"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoOperacion) Then
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
                            LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                            ObtenerValoresCombos(False, _EncabezadoSeleccionado, OPCION_CLASIFICACIONINVERSION)
                        End If
                    Case "tipoorigen"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.TipoOrigen) Then
                            ObtenerValoresDefecto(OPCION_TIPOORIGEN, _EncabezadoSeleccionado)
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
                            LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)

                            If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAFIJA _
                                Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO _
                                Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA _
                                Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.Nemotecnico) Then
                                    Await CalcularValorRegistro(False)
                                    logCalcularValores = False
                                End If

                                Dim logTipoOrigenEsOtrasFirmas As Boolean = False

                                If VerificarOrigenPrincipalSecundario(TIPOORIGEN_OTRASFIRMAS, _EncabezadoSeleccionado.TipoOrigen) Then
                                    logTipoOrigenEsOtrasFirmas = True
                                End If

                                If logTipoOrigenEsOtrasFirmas = False Then
                                    If String.IsNullOrEmpty(_EncabezadoSeleccionado.Nemotecnico) Then
                                        logCalcularValores = False
                                    End If
                                    _EncabezadoSeleccionado.Retencion = 0
                                    If String.IsNullOrEmpty(_EncabezadoSeleccionado.Nemotecnico) Then
                                        logCalcularValores = True
                                    End If
                                End If

                                'If _EncabezadoSeleccionado.TipoOrigen <> TIPOORIGEN_OTRASFIRMAS Then
                                '    If String.IsNullOrEmpty(_EncabezadoSeleccionado.Nemotecnico) Then
                                '        logCalcularValores = False
                                '    End If
                                '    _EncabezadoSeleccionado.Retencion = 0
                                '    If String.IsNullOrEmpty(_EncabezadoSeleccionado.Nemotecnico) Then
                                '        logCalcularValores = True
                                '    End If
                                'End If
                                logCalcularValores = True
                            End If

                            If _EncabezadoSeleccionado.TipoNegocio <> TIPONEGOCIO_DEPOSITOREMUNERADO Then
                                Dim logTipoOrigenSeleccionadoInternacional As Boolean = False

                                If VerificarOrigenPrincipalSecundario(TIPOORIGEN_INTERNACIONAL, _EncabezadoSeleccionado.TipoOrigen) Or _EncabezadoSeleccionado.TipoOrigen = TIPOORIGEN_DIVISAS Then
                                    logTipoOrigenSeleccionadoInternacional = True
                                End If

                                If logTipoOrigenSeleccionadoInternacional Then
                                    If DiccionarioCombos.ContainsKey("NEGOCIOS_MONEDADEFECTOINTERNACIONAL") Then
                                        logPosicionarCantidad = False
                                        _EncabezadoSeleccionado.IDMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTOINTERNACIONAL").First.ID

                                        _EncabezadoSeleccionado.NombreMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTOINTERNACIONAL").First.Descripcion
                                        _EncabezadoSeleccionado.CodigoMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTOINTERNACIONAL").First.Codigo

                                        Await ObtenerTRMMoneda()
                                        HabilitarCamposRegistro(_EncabezadoSeleccionado)
                                    End If
                                Else
                                    If DiccionarioCombos.ContainsKey("NEGOCIOS_MONEDADEFECTO") Then
                                        logPosicionarCantidad = False
                                        _EncabezadoSeleccionado.IDMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTO").First.ID

                                        _EncabezadoSeleccionado.NombreMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTO").First.Descripcion
                                        _EncabezadoSeleccionado.CodigoMoneda = DiccionarioCombos("NEGOCIOS_MONEDADEFECTO").First.Codigo
                                    End If
                                End If
                            End If
                        End If
                    Case "paisnegociacion"
                        If Not IsNothing(_EncabezadoSeleccionado.PaisNegociacion) Then
                            ObtenerMonedaLocal()
                        End If
                    Case "idmoneda"
                        If Not IsNothing(_EncabezadoSeleccionado.IDMoneda) Then
                            logCalcularValores = False
                            Await ObtenerTRMMoneda()
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
                            logCalcularValores = True
                            strTipoCalculo = TIPOCALCULOS_MOTOR.PRECIO.ToString

                            If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAFIJA _
                                Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO _
                                Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA _
                                Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.Nemotecnico) Then
                                    Await CalcularValorRegistro()
                                End If
                            Else
                                Await CalcularValorRegistro()
                            End If

                            If logPosicionarCantidad Then
                                If EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAVARIABLE Then
                                    viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalRentaVariable")
                                ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_RENTAFIJA Then
                                    viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalRentaFija")
                                ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO Then
                                    If EncabezadoSeleccionado.Tipo = CLASE_ACCIONES Then
                                        viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalREPOAcciones")
                                    Else
                                        If EncabezadoSeleccionado.TipoRepo = TIPOREPO_ABIERTO Then
                                            viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalRepoAbierto")
                                        Else
                                            viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalRepo")
                                        End If
                                    End If
                                ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                                    viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalSimultanea")
                                ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                                    If EncabezadoSeleccionado.Tipo = CLASE_ACCIONES Then
                                        viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalTTVAcciones")
                                    Else
                                        viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalTTV")
                                    End If
                                ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                                    viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalDepositoRemunerado")
                                ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DIVISAS Then
                                    viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtNominalDivisas")
                                End If

                            Else
                                logPosicionarCantidad = True
                            End If
                        End If
                    Case "fechacumplimiento"
                        If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO _
                            Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA _
                            Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV _
                            Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                            Dim strValorActualTipoCalculo As String = strTipoCalculo
                            strTipoCalculo = TIPOCALCULOS_MOTOR.VENCIMIENTOOPERACION.ToString
                            Await CalcularValorRegistro(False)
                            strTipoCalculo = strValorActualTipoCalculo
                        Else
                            Await CalcularValorRegistro(False)
                        End If
                    Case "fechavencimientooperacion"
                        If _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO _
                            Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA _
                            Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV _
                            Or _EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                            Await CalcularValorRegistro()
                        End If

                        If EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_REPO Then
                            If _EncabezadoSeleccionado.Tipo = CLASE_ACCIONES Then
                                viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtVTasaPactadaREPOAcciones")
                            Else
                                If EncabezadoSeleccionado.TipoRepo = TIPOREPO_ABIERTO Then
                                    viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtVTasaPactadaRepoAbierto")
                                Else
                                    viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtVTasaPactadaRepo")
                                End If
                            End If
                        ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_SIMULTANEA Then
                            viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtVTasaPactadaSimultanea")
                        ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_TTV Then
                            If _EncabezadoSeleccionado.Tipo = CLASE_ACCIONES Then
                                viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtVTasaPactadaTTVAcciones")
                            Else
                                viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtVTasaPactadaTTV")
                            End If
                        ElseIf EncabezadoSeleccionado.TipoNegocio = TIPONEGOCIO_DEPOSITOREMUNERADO Then
                            viewFormaOperacionesOtrosNegocios.BuscarControlValidacion("txtVTasaPactadaDepositoRemunerado")
                        End If
                    Case "fechaoperacion"
                        If Not IsNothing(_EncabezadoSeleccionado.FechaOperacion) Then
                            _EncabezadoSeleccionado.FechaCumplimiento = _EncabezadoSeleccionado.FechaOperacion
                            logCalcularValores = False
                            Await ObtenerTRMMoneda()
                            logCalcularValores = True
                        End If
                    Case "fechaemision"
                        LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                    Case "fechavencimiento"
                        LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                    Case "modalidad"
                        LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                    Case "indicadoreconomico"
                        LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                    Case "tasafacial"
                        LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                    Case "puntosindicador"
                        LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                    Case "idcontraparte"
                        If Not IsNothing(_EncabezadoSeleccionado.IDContraparte) Then
                            consultarCuentasDepositoEntidad(_EncabezadoSeleccionado.IDContraparte, OPCION_EDITAR)
                        End If
                    Case "mercado"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.Mercado) Then
                            Await ObtenerPrecioMercado()
                        End If
                    Case "tipo"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.Tipo) Then
                            LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_TIPO)
                            ObtenerValoresDefecto(OPCION_TIPOREPO, _EncabezadoSeleccionado)
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
                            ObtenerClaseEspecies()
                        End If
                    Case "idcomitenteotro"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.IDComitenteOtro) Then
                            consultarCuentasDepositoContraparte(_EncabezadoSeleccionado.IDComitenteOtro)
                        End If
                    Case "tipobanrep"
                        If Not IsNothing(_EncabezadoSeleccionado.TipoBanRep) Then
                            ObtenerValoresDefecto(OPCION_TIPOBANREP, _EncabezadoSeleccionado)
                            HabilitarCamposRegistro(_EncabezadoSeleccionado)
                        End If
                    Case "tiporepo"
                        LimpiarDatosTipoNegocio(_EncabezadoSeleccionado, LIMPIARDATOS_NEMOTECNICO)
                        HabilitarCamposRegistro(_EncabezadoSeleccionado)
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_EncabezadoSeleccionado_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el mensaje de la notificación.", _
                                Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaOperacionesOtrosNegocios
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

    Private _Referencia As String
    <Display(Name:="Referencia")> _
    Public Property Referencia() As String
        Get
            Return _Referencia
        End Get
        Set(ByVal value As String)
            _Referencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Referencia"))
        End Set
    End Property

    Private _TipoRegistro As String
    <Display(Name:="TipoRegistro")> _
    Public Property TipoRegistro() As String
        Get
            Return _TipoRegistro
        End Get
        Set(ByVal value As String)
            _TipoRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoRegistro"))
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

    Private _TipoNegocio As String
    <Display(Name:="TipoNegocio")> _
    Public Property TipoNegocio() As String
        Get
            Return _TipoNegocio
        End Get
        Set(ByVal value As String)
            _TipoNegocio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoNegocio"))
        End Set
    End Property

    Private _TipoOrigen As String
    <Display(Name:="TipoOrigen")> _
    Public Property TipoOrigen() As String
        Get
            Return _TipoOrigen
        End Get
        Set(ByVal value As String)
            _TipoOrigen = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOrigen"))
        End Set
    End Property

    Private _TipoOperacion As String
    <Display(Name:="TipoOperacion")> _
    Public Property TipoOperacion() As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _Cliente As String
    <Display(Name:="Cliente")> _
    Public Property Cliente() As String
        Get
            Return _Cliente
        End Get
        Set(ByVal value As String)
            _Cliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Cliente"))
        End Set
    End Property

    Private _Especie As String
    <Display(Name:="Especie")> _
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property

    Private _FechaOperacion As Nullable(Of DateTime)
    <Display(Name:="FechaOperacion")> _
    Public Property FechaOperacion() As Nullable(Of DateTime)
        Get
            Return _FechaOperacion
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaOperacion"))
        End Set
    End Property

    Private _FechaCumplimiento As Nullable(Of DateTime)
    <Display(Name:="FechaCumplimiento")> _
    Public Property FechaCumplimiento() As Nullable(Of DateTime)
        Get
            Return _FechaCumplimiento
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaCumplimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaCumplimiento"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class