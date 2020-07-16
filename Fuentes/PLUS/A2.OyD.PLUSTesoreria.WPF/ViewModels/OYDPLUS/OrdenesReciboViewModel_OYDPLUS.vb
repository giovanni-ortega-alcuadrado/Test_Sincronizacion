Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: TesoreriaViewModel.vb
'Generado el : 07/08/2011 15:52:29
'Propiedad de Alcuadrado S.A. 2010

#Region "Bitacora de Modificaciones"
'Modificado Por : Juan David Correa
'Fecha          : 16/Agosto 4:15 pm
'Descripción    : Se agrego el metodo ValidarEdicion y el metodo asincronico TerminoValidarEdicion.

'Modificaciones
'Modificado Por : Juan David Correa
'Fecha          : 17/Agosto 8:20 am
'Descripción    : Se agrego la propiedad Read para controlar la edición del grid y habilitar los scroll.

'Modificado Por : Juan David Correa
'Fecha          : 17/Agosto 9:50 am
'Descripción    : Se modifico el evento de VesionRegistro al codigo se le asigna el ID del documento 
'                 codigo = TesoreriSelected.IDDocumento.

'Modificado Por : Juan David Correa
'Fecha          : 17/Agosto 9:50 am
'Descripción    : Se modifico la propiedad cmbNombreConsecutivoHabilitado para que cuando se le de clic al boton
'                 nuevo la propiedad este en true y cuando se le de editar cambie a false

'Modificado Por : Juan David Correa
'Fecha          : 18/Agosto 8:50 am
'Descripción    : Se agrego la propiedad contador para controlar la vista del detalle cuando se crea un nuevo registro

'Modificado Por : Juan Carlos Soto Cruz
'Fecha          : 18/Agosto 10:24 a.m
'Descripción    : 

'Modificado Por : Juan Carlos Soto Cruz
'Fecha          : 04 Septiembre/2011
'Descripción    : Se

#End Region

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSTesoreria
'Imports Microsoft.VisualBasic.ac
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports Microsoft.VisualBasic
Imports OpenRiaServices.DomainServices.Client

Public Class OrdenesReciboViewModel_OYDPLUS
	Inherits A2ControlMenu.A2ViewModel

	Public Event LanzarPopupCheques(ByVal sender As Object, ByVal e As System.EventArgs, ByVal pstrOpcion As String)
	Public Event LanzarPopupTransferencias(ByVal sender As Object, ByVal e As System.EventArgs, ByVal pstrOpcion As String)
	Public Event LanzarPopupConsignacion(ByVal sender As Object, ByVal e As System.EventArgs, ByVal pstrOpcion As String)
	Public Event LanzarPopupCargarPagoA(ByVal sender As Object, ByVal e As System.EventArgs, ByVal pstrOpcion As String)
	Public Event LanzarPopupCargarPagoAFondos(ByVal sender As Object, ByVal e As System.EventArgs, ByVal pstrOpcion As String)
	Dim objTipoCliente As Object

#Region "Declaraciones"
	Public logXTesorero As Boolean = False
	Public IdEncabezadoXTesorero As Integer = -1
	Public objViewOrdenReciboPopPup As OrdenReciboConsultaView
	Public objViewModelTesorero As TesoreroViewModel_OYDPLUS

	Dim intIDOrdenTimer As Integer
	Dim num As Integer = 0
	Public strOpcionActualizar As String = String.Empty

	Public logHayEncabezado As Boolean = False
	Dim ValorTotalOrden As Decimal = 0
	Public objWppImportacion As ImportacionCheque

	'------'------'------'------'------'------'------
	Public objWppChequeRC As wppFrmCheque_RC
	Public objWppCargarPagosA As wppFrmCargarPagosA_RC
	Public objWppCargarPagosAFondos As wppFrmCargarPagosAFondos_RC
	Public objWppConsignacion As wppFrmConsignaciones_RC
	Public objWppTransferencia_RC As wppFrmTransferencia_RC

	Public logEsTercero As Boolean
	Public TesoreriaOrdenesPlusAnterior As New TesoreriaOrdenesEncabezado

	Dim strRegistrosDetalle As String = ""
	Dim cantidadTotalConfirmacion As Integer = 0
	Dim cantidadTotalJustificacion As Integer = 0
	Dim CantidadTotalAprobaciones As Integer = 0

	Dim cantidadTotalConfirmacionListasRestrictivas As Integer = 0
	Dim cantidadTotalJustificacionListasRestrictivas As Integer = 0
	Dim CantidadTotalAprobacionesListasRestrictivas As Integer = 0
	Dim strMensajeValidacionListasRestrictiva As String = String.Empty

	Dim strMensajeValidacion As String = String.Empty
	Dim mdtmFechaCierreSistema As DateTime = Now.Date

	Public dcProxy As OYDPLUSTesoreriaDomainContext
	Public dcProxyUtilidades As UtilidadesDomainContext
	Public dcProxyUtilidadesPLUS As OYDPLUSUtilidadesDomainContext
	Public dcProxyImportaciones As ImportacionesDomainContext
	Public dcProxy2 As OYDPLUSTesoreriaDomainContext

	Dim A2Util As A2UtilsViewModel
	Dim DicCamposTab As New Dictionary(Of String, Integer)
	Dim fechaCierre As DateTime
	Dim Filtro As Integer = 0
	Public TIPODOCUMENTOSIMPORTACION As String
	Public TIPOCUENTAOYDPLUS As String

	Dim logNuevoRegistroCheque As Boolean
	Public logNuevaConsignacion As Boolean = False
	Public logEsTerceroTransferencia As Boolean
	Public logEsCuentaRegistrada As Boolean
	Public logEsTerceroCartera As Boolean
	Public logEsTerceroInternos As Boolean

	Dim logCancelarRegistro As Boolean
	Public logNuevoRegistro As Boolean
	Public logEditarRegistro As Boolean
	Public logDuplicarRegistro As Boolean
	Dim logBuscar As Boolean
	Public logFiltrar As Boolean
	Dim logRealizarConsultaPropiedades As Boolean = False

	''***INSTRUCCIONES***
	Dim logDireccionInscrita_Instrucciones As Boolean = False
	Dim logEsTercero_Instrucciones As Boolean = False
	Dim logEsCtaInscrita_Instrucciones As Boolean = False
	Dim logGrabar As Boolean

	'***EDICION DETALLE***
	Public logEditarChequeRC As Boolean = False
	Public logEditarTransferenciaRC As Boolean = False
	Public logEditarConsignacion As Boolean = False
	Public logEditarCargarPagosA As Boolean = False
	Public logEditarCargarPagosAFondos As Boolean = False

	'VARIABLE PARA SER UTILIZADA CUANDO SE GRABE LA ORDEN
	Dim intIDOrdenTesoreria As Integer = 0
	Dim intIDORdenTesoreriaEditar As Integer = 0
	Dim logCambiarPropiedadesPOPPUP As Boolean = True
	Dim logRecargarPantalla As Boolean = True
	Dim logRefrescarDetalles As Boolean = True
	Dim logGuardandoOrden As Boolean = False
	Dim logConsultarCarterasColectivas As Boolean = False

	Dim intCantidadMaximaDetalles As Integer = 1000
	Dim logHabilitarFuncionalidadFondos As Boolean = True
	Dim dtmFechaServidor As DateTime

	Dim strConceptoDefecto_Fondos As String = String.Empty
	Public logEsFondosOYD As Boolean = False
	Dim intDiasAplicacionFondosApertura As Integer = 0
	Dim intDiasAplicacionFondosAdicion As Integer = 0
	Dim logCaracterInvalido As Boolean ' para validacion de caracteres invalidos

	Dim ConceptoDefectoFondos_Adicion As Nullable(Of Integer) = Nothing
	Dim ConceptoDescripcionDefectoFondos_Adicion As String = String.Empty
	Dim ConceptoDefectoFondos_Apertura As Nullable(Of Integer) = Nothing
	Dim ConceptoDescripcionDefectoFondos_Apertura As String = String.Empty

	Dim dtmFechaMenorPermitidaIngreso As Nullable(Of DateTime) = Now.Date
	Dim logValidarCamposObligatoriosTransferenciaConsignacion As Boolean = False
	Public logEsFondosUnity As Boolean = False
	Dim logRecargar As Boolean = False
	Private intCantidadDocumentosSubir As Integer = 0
	Private intCantidadDocumentosSubidos As Integer = 0

#End Region

#Region "Constantes"

	''' <summary>
	''' CE : Comprobantes de Egreso
	''' RC : Recibos de Caja
	''' N : Notas Contables
	''' </summary>
	Public Enum ClasesTesoreria
		CE '// Comprobantes de Egreso
		RC '// Bancos
		N '// Notas Contables
		PlusCE ' //Comprobantes de Egreso OyD Plus
	End Enum
	Public Enum TabFormasPago
		Cheque
		Transferencia
		CarteraColectiva
		Internos
		Instrucciones
	End Enum
	Enum OpcionesInstrucciones
		ClienteRecoge
		ClientePresente
		LlevarDireccion
		RecogeTercero
		ConsignarCuenta
	End Enum

	Private Const COLOR_DESHABILITADO As String = "#E2E2E2"
	Private Const COLOR_HABILITADO As String = "#FFFFFFFF"

#End Region

#Region "Inicializacion"
	Public Sub New()
		Try
			If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
				dcProxy = New OYDPLUSTesoreriaDomainContext()
				dcProxyUtilidades = New UtilidadesDomainContext()
				dcProxyUtilidadesPLUS = New OYDPLUSUtilidadesDomainContext()
				dcProxyImportaciones = New ImportacionesDomainContext
				dcProxy2 = New OYDPLUSTesoreriaDomainContext()
			Else
				dcProxy = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
				dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
				dcProxyUtilidadesPLUS = New OYDPLUSUtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYDPLUS)))
				dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
				dcProxy2 = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
			End If

			'SE LLENA EL DICCIONARIO PARA EL CONTROL DE HABILITACIÓN DE LOS TAB Y CADA UNO SE PONE VISIBLE DEPENDIENDO
			'DEL CAMBIO EN PANTALLA
			DiccionarioTabPantalla = New Dictionary(Of String, clsTabHabilitar)
			DiccionarioTabPantalla.Add(GSTR_TABORDENESRECAUDO_CHEQUE_CODIGO, New clsTabHabilitar With {.Codigo = GSTR_TABORDENESRECAUDO_CHEQUE_CODIGOINTERNO, .Descripcion = GSTR_TABORDENESRECAUDO_CHEQUE_DESCRIPCION, .TabVisible = Visibility.Collapsed})
			DiccionarioTabPantalla.Add(GSTR_TABORDENESRECAUDO_TRANSFERENCIA_CODIGO, New clsTabHabilitar With {.Codigo = GSTR_TABORDENESRECAUDO_TRANSFERENCIA_CODIGOINTERNO, .Descripcion = GSTR_TABORDENESRECAUDO_TRANSFERENCIA_DESCRIPCION, .TabVisible = Visibility.Collapsed})
			DiccionarioTabPantalla.Add(GSTR_TABORDENESRECAUDO_CONSIGNACION_CODIGO, New clsTabHabilitar With {.Codigo = GSTR_TABORDENESRECAUDO_CONSIGNACION_CODIGOINTERNO, .Descripcion = GSTR_TABORDENESRECAUDO_CONSIGNACION_DESCRIPCION, .TabVisible = Visibility.Collapsed})

			DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OYDPLUSTesoreriaDomainContext.IOYDPLUSTesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
			'If Not Program.IsDesignMode() Then
			TIPODOCUMENTOSIMPORTACION = Program.TipoDocumentoOyDPlus
			TIPOCUENTAOYDPLUS = Program.TipoCuentaOyDPlus
			GSTR_CLIENTE = Program.Tipo_Cliente_Cliente
			GSTR_TERCERO = Program.Tipo_Cliente_Tercero
			GSTR_CUENTA_NO_REGISTRADA = Program.Tipo_CUENTA_NO_REGISTRADA
			GSTR_CUENTA_REGISTRADA = Program.Tipo_CUENTA_REGISTRADA
			IsBusy = True
			TabItemActual = "Cheques"
			Usuario = Program.Usuario
			CargarReceptoresUsuarioOYDPLUS("INICIO")
			CargarParametrosReceptorOYDPLUS(String.Empty, "INICIO")
			CargarCombosOYDPLUS(String.Empty, "INICIO")
			DefineCommands()
			HabilitarEntregar = False
			FechaOrden = Now

			Dim objListaDatos As New List(Of String)

			objListaDatos = New List(Of String)
			objListaDatos.Add(GSTR_PENDIENTES)
			objListaDatos.Add(GSTR_RECHAZADAS)
			objListaDatos.Add(GSTR_POR_APROBAR)
			objListaDatos.Add(GSTR_ANULADAS)

			ListaDatos = objListaDatos
		Catch ex As Exception

			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
								 Me.ToString(), "TesoreriaViewModelOyDPlus.New", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Sub Inicializar()
		Try
			VistaSeleccionada = ListaDatos.First

			HabilitarInstrucciones = False
			VerDuplicar = Visibility.Visible
			ConsultarCarterasColectivasFondos(String.Empty, True, "TODASLASCARTERAS")
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
								 Me.ToString(), "TesoreriaViewModelOyDPlus.Inicializar", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub
#End Region

#Region "OyD PLUS"

#Region "Propiedades Command"
	Public Property ImportarCheque As ICommand
	Public Property ImportarTransferencia As ICommand

	Public Property NuevoCheque As ICommand
	Public Property EditarWppCheque As ICommand
	Public Property BorrarCheque As ICommand
	Public Property CancelarGrabacion As ICommand
	Public Property GuardarCheque As ICommand
	Public Property GuardarContinuar As ICommand

	Public Property NuevaTransferencia As ICommand
	Public Property EditarWppTransferencia As ICommand
	Public Property BorrarTransferencia As ICommand
	Public Property CancelarTransferencia As ICommand
	Public Property GuardarSalirTransferencia As ICommand
	Public Property GuardarContinuarTransferencia As ICommand

	Public Property NuevaConsignacion As ICommand
	Public Property EditarWppConsignacion As ICommand
	Public Property BorrarConsignacion As ICommand
	Public Property CancelarConsignacion As ICommand
	Public Property GuardarSalirConsignacion As ICommand
	Public Property GuardarContinuarConsignacion As ICommand
	Public Property GrabarChequeConsignacion As ICommand

	Public Property NuevoCargarPagoA As ICommand
	Public Property EditarCargarPagoA As ICommand
	Public Property BorrarCargarPagoA As ICommand
	Public Property CancelarCargarPagosAGrabacion As ICommand
	Public Property GuardarCargarPagosASalir As ICommand
	Public Property GuardarCargarPagosAContinuar As ICommand

	Public Property NuevoCargarPagoAFondos As ICommand
	Public Property EditarCargarPagoAFondos As ICommand
	Public Property BorrarCargarPagoAFondos As ICommand
	Public Property CancelarCargarPagosAFondosGrabacion As ICommand
	Public Property GuardarCargarPagosAFondosSalir As ICommand
	Public Property GuardarCargarPagosAFondosContinuar As ICommand

#End Region

#Region "PROPIEDADES OYD PLUS"
	Private _DiccionarioTabPantalla As Dictionary(Of String, clsTabHabilitar)
	Public Property DiccionarioTabPantalla() As Dictionary(Of String, clsTabHabilitar)
		Get
			Return _DiccionarioTabPantalla
		End Get
		Set(ByVal value As Dictionary(Of String, clsTabHabilitar))
			_DiccionarioTabPantalla = value
			MyBase.CambioItem("DiccionarioTabPantalla")
		End Set
	End Property

	'JDOL 20170725 Ajuste para Buscador Concepto Receptor desde el Tesorero
	Private _strCodigoReceptorBuscadorConcepto As String = ""
	Public Property strCodigoReceptorBuscadorConcepto() As String
		Get
			Return _strCodigoReceptorBuscadorConcepto
		End Get
		Set(ByVal value As String)
			_strCodigoReceptorBuscadorConcepto = value
			MyBase.CambioItem("strCodigoReceptorBuscadorConcepto")
		End Set
	End Property

	Private _VerEditarXTesorero As Visibility
	Public Property VerEditarXTesorero() As Visibility
		Get
			Return _VerEditarXTesorero
		End Get
		Set(ByVal value As Visibility)
			_VerEditarXTesorero = value
			MyBase.CambioItem("VerEditarXTesorero")
		End Set
	End Property
	Private _VerGrabarXTesorero As Visibility = Visibility.Collapsed
	Public Property VerGrabarXTesorero() As Visibility
		Get
			Return _VerGrabarXTesorero
		End Get
		Set(ByVal value As Visibility)
			_VerGrabarXTesorero = value
			MyBase.CambioItem("VerGrabarXTesorero")
		End Set
	End Property
	Private _VerCancelarXTesorero As Visibility = Visibility.Collapsed
	Public Property VerCancelarXTesorero() As Visibility
		Get
			Return _VerCancelarXTesorero
		End Get
		Set(ByVal value As Visibility)
			_VerCancelarXTesorero = value
			MyBase.CambioItem("VerCancelarXTesorero")
		End Set
	End Property

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
			If logRecargarPantalla Then
				If Not String.IsNullOrEmpty(VistaSeleccionada) Then
					TraerOrdenes("", VistaSeleccionada)
				End If
			End If
			MyBase.CambioItem("VistaSeleccionada")
		End Set
	End Property

	Private _DescripcionComboConcepto As String
	Public Property DescripcionComboConcepto() As String
		Get
			Return _DescripcionComboConcepto
		End Get
		Set(ByVal value As String)
			_DescripcionComboConcepto = value
			MyBase.CambioItem("DescripcionComboConcepto")
		End Set
	End Property

	Private _DescripcionComboConceptoFondos As String
	Public Property DescripcionComboConceptoFondos() As String
		Get
			Return _DescripcionComboConceptoFondos
		End Get
		Set(ByVal value As String)
			_DescripcionComboConceptoFondos = value
			MyBase.CambioItem("DescripcionComboConceptoFondos")
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

	Private _DescripcionConceptoFondos As String
	Public Property DescripcionConceptoFondos() As String
		Get
			Return _DescripcionConceptoFondos
		End Get
		Set(ByVal value As String)
			_DescripcionConceptoFondos = value
			MyBase.CambioItem("DescripcionConceptoFondos")
		End Set
	End Property

	Private _IdConcepto As Nullable(Of Integer)
	Public Property IdConcepto() As Nullable(Of Integer)
		Get
			Return _IdConcepto
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdConcepto = value
			If Not IsNothing(DiccionarioCombosOYDPlus) And Not IsNothing(_IdConcepto) Then
				If DiccionarioCombosOYDPlus.ContainsKey("CONCEPTOS") Then
					If DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.IntId = _IdConcepto).Count > 0 Then
						DescripcionComboConcepto = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.IntId = _IdConcepto).First.Descripcion
					Else
						DescripcionComboConcepto = String.Empty
					End If
				Else
					DescripcionComboConcepto = String.Empty
				End If
			Else
				DescripcionComboConcepto = String.Empty
			End If
			MyBase.CambioItem("IdConcepto")
		End Set
	End Property

	Private _IdConceptoFondos As Nullable(Of Integer)
	Public Property IdConceptoFondos() As Nullable(Of Integer)
		Get
			Return _IdConceptoFondos
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdConceptoFondos = value
			If Not IsNothing(DiccionarioCombosOYDPlus) And Not IsNothing(_IdConceptoFondos) Then
				If DiccionarioCombosOYDPlus.ContainsKey("CONCEPTOS") Then
					If DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.IntId = _IdConceptoFondos).Count > 0 Then
						DescripcionComboConceptoFondos = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.IntId = _IdConceptoFondos).First.Descripcion
					Else
						DescripcionComboConceptoFondos = String.Empty
					End If
				Else
					DescripcionComboConceptoFondos = String.Empty
				End If
			Else
				DescripcionComboConceptoFondos = String.Empty
			End If
			MyBase.CambioItem("IdConceptoFondos")
		End Set
	End Property

	Private _VerFormaChequeConsignacionGrid As Visibility = Visibility.Collapsed
	Public Property VerFormaChequeConsignacionGrid As Visibility
		Get
			Return _VerFormaChequeConsignacionGrid
		End Get
		Set(ByVal value As Visibility)
			_VerFormaChequeConsignacionGrid = value
			MyBase.CambioItem("VerFormaChequeConsignacionGrid")
		End Set
	End Property

	Private _VerFormaChequeConsignacion As Visibility = Visibility.Collapsed
	Public Property VerFormaChequeConsignacion As Visibility
		Get
			Return _VerFormaChequeConsignacion
		End Get
		Set(ByVal value As Visibility)
			_VerFormaChequeConsignacion = value
			MyBase.CambioItem("VerFormaChequeConsignacion")
		End Set
	End Property

	Private _VerFormaChequeEfectivoConsignacion As Visibility = Visibility.Collapsed
	Public Property VerFormaChequeEfectivoConsignacion() As Visibility
		Get
			Return _VerFormaChequeEfectivoConsignacion
		End Get
		Set(ByVal value As Visibility)
			_VerFormaChequeEfectivoConsignacion = value
			MyBase.CambioItem("VerFormaChequeEfectivoConsignacion")
		End Set
	End Property

	Private _VerCargarPagosA As Visibility = Visibility.Collapsed
	Public Property VerCargarPagosA As Visibility
		Get
			Return _VerCargarPagosA
		End Get
		Set(ByVal value As Visibility)
			_VerCargarPagosA = value
			MyBase.CambioItem("VerCargarPagosA")
		End Set
	End Property

	Private _VerCargarPagosAFondos As Visibility = Visibility.Collapsed
	Public Property VerCargarPagosAFondos As Visibility
		Get
			Return _VerCargarPagosAFondos
		End Get
		Set(ByVal value As Visibility)
			_VerCargarPagosAFondos = value
			MyBase.CambioItem("VerCargarPagosAFondos")
		End Set
	End Property

	Private _VerDatosFondos As Visibility = Visibility.Collapsed
	Public Property VerDatosFondos As Visibility
		Get
			Return _VerDatosFondos
		End Get
		Set(ByVal value As Visibility)
			_VerDatosFondos = value
			If _VerDatosFondos = Visibility.Visible Then
				If _HabilitarCamposFondosOYD Then
					MostrarFechaOrden = True
				Else
					MostrarFechaOrden = False
				End If
			Else
				MostrarFechaOrden = False
			End If
			MyBase.CambioItem("VerDatosFondos")
		End Set
	End Property

	Private _HabilitarCarteraColectiva As Boolean = False
	Public Property HabilitarCarteraColectiva() As Boolean
		Get
			Return _HabilitarCarteraColectiva
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCarteraColectiva = value
			MyBase.CambioItem("HabilitarCarteraColectiva")
		End Set
	End Property

	Private _Descripcionformaconsignacion As String
	Public Property Descripcionformaconsignacion() As String
		Get
			Return _Descripcionformaconsignacion
		End Get
		Set(ByVal value As String)
			_Descripcionformaconsignacion = value
			MyBase.CambioItem("Descripcionformaconsignacion")
		End Set
	End Property

	Private _strNombreEntregado As String
	Public Property strNombreEntregado() As String
		Get
			Return _strNombreEntregado
		End Get
		Set(ByVal value As String)
			_strNombreEntregado = value
			HabilitarSubirArchivo()

			If Not String.IsNullOrEmpty(_strNombreEntregado) Then
				If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
					TesoreriaOrdenesPlusRC_Selected.strNombre = _strNombreEntregado
				End If
			End If

			MyBase.CambioItem("strNombreEntregado")
		End Set
	End Property

	Private _strNroDocumentoEntregado As String
	Public Property strNroDocumentoEntregado() As String
		Get
			Return _strNroDocumentoEntregado
		End Get
		Set(ByVal value As String)
			_strNroDocumentoEntregado = value
			HabilitarSubirArchivo()

			If Not String.IsNullOrEmpty(_strNroDocumentoEntregado) Then
				If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
					TesoreriaOrdenesPlusRC_Selected.strNroDocumento = _strNroDocumentoEntregado
				End If
			End If
			MyBase.CambioItem("strNroDocumentoEntregado")
		End Set
	End Property

	Private _DescripcionTipoIdentificacionEntregado As String
	Public Property DescripcionTipoIdentificacionEntregado() As String
		Get
			Return _DescripcionTipoIdentificacionEntregado
		End Get
		Set(ByVal value As String)
			_DescripcionTipoIdentificacionEntregado = value
			MyBase.CambioItem("DescripcionTipoIdentificacionEntregado")
		End Set
	End Property

	Private _strTipoIdentificacionEntregado As String
	Public Property strTipoIdentificacionEntregado() As String
		Get
			Return _strTipoIdentificacionEntregado
		End Get
		Set(ByVal value As String)
			_strTipoIdentificacionEntregado = value
			HabilitarSubirArchivo()

			If Not String.IsNullOrEmpty(_strTipoIdentificacionEntregado) Then
				If Not IsNothing(DiccionarioCombosOYDPlus) Then
					If DiccionarioCombosOYDPlus.ContainsKey("TIPOID") Then
						For Each li In DiccionarioCombosOYDPlus("TIPOID").Where(Function(i) i.Retorno = _strTipoIdentificacionEntregado)
							DescripcionTipoIdentificacionEntregado = li.Descripcion
						Next
					End If
				End If
				If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
					TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion = _strTipoIdentificacionEntregado
					TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion = DescripcionTipoIdentificacionEntregado
				End If
			End If
			MyBase.CambioItem("strTipoIdentificacionEntregado")
		End Set
	End Property

	Private _strTipoPagoConsignacion As String
	Public Property strTipoPagoConsignacion() As String
		Get
			Return _strTipoPagoConsignacion
		End Get
		Set(ByVal value As String)
			_strTipoPagoConsignacion = value
			If Not String.IsNullOrEmpty(_strTipoPagoConsignacion) Then
				If _strTipoPagoConsignacion = GSTR_CHEQUE Then
					VerFormaChequeConsignacion = Visibility.Visible
					Descripcionformaconsignacion = "Cheque"
					VerFormaChequeEfectivoConsignacion = Visibility.Collapsed
					If logNuevaConsignacion = True Then
						VerFormaChequeConsignacionGrid = Visibility.Visible
					End If
				ElseIf _strTipoPagoConsignacion = GSTR_EFECTIVO Then
					VerFormaChequeConsignacion = Visibility.Collapsed
					Descripcionformaconsignacion = "Efectivo"
					VerFormaChequeConsignacionGrid = Visibility.Collapsed
					VerFormaChequeEfectivoConsignacion = Visibility.Collapsed
				ElseIf _strTipoPagoConsignacion = GSTR_CONSIGNACION_AMBAS Then
					VerFormaChequeConsignacion = Visibility.Visible
					VerFormaChequeEfectivoConsignacion = Visibility.Visible
					VerFormaChequeConsignacionGrid = Visibility.Visible
					Descripcionformaconsignacion = "Ambas"
				End If
			Else
				VerFormaChequeConsignacion = Visibility.Collapsed
			End If

			MyBase.CambioItem("strTipoPagoConsignacion")
		End Set
	End Property

	Private _lngNroCheque As Double
	Public Property lngNroCheque() As Double
		Get
			Return _lngNroCheque
		End Get
		Set(ByVal value As Double)
			_lngNroCheque = value
			MyBase.CambioItem("lngNroCheque")
		End Set
	End Property

	Private _lngNroChequeConsignacion As Double
	Public Property lngNroChequeConsignacion() As Double
		Get
			Return _lngNroChequeConsignacion
		End Get
		Set(ByVal value As Double)
			_lngNroChequeConsignacion = value
			MyBase.CambioItem("lngNroChequeConsignacion")
		End Set
	End Property

	Private _lngNroReferencia As Double
	Public Property lngNroReferencia() As Double
		Get
			Return _lngNroReferencia
		End Get
		Set(ByVal value As Double)
			_lngNroReferencia = value
			MyBase.CambioItem("lngNroReferencia")
		End Set
	End Property

	Private _DireccionRegistrada As Integer
	Public Property DireccionRegistrada() As Integer
		Get
			Return _DireccionRegistrada
		End Get
		Set(ByVal value As Integer)
			_DireccionRegistrada = value
			MyBase.CambioItem("DireccionRegistrada")
		End Set
	End Property

	Private _strDireccionCheque As String
	Public Property strDireccionCheque() As String
		Get
			Return _strDireccionCheque
		End Get
		Set(ByVal value As String)
			_strDireccionCheque = value
			MyBase.CambioItem("strDireccionCheque")
		End Set
	End Property

	Private _strTelefono As String
	Public Property strTelefono() As String
		Get
			Return _strTelefono
		End Get
		Set(ByVal value As String)
			_strTelefono = value
			MyBase.CambioItem("strTelefono")
		End Set
	End Property

	Private _strCiudad As String
	Public Property strCiudad() As String
		Get
			Return _strCiudad
		End Get
		Set(ByVal value As String)
			_strCiudad = value
			MyBase.CambioItem("strCiudad")
		End Set
	End Property

	Private _strSector As String
	Public Property strSector() As String
		Get
			Return _strSector
		End Get
		Set(ByVal value As String)
			_strSector = value
			MyBase.CambioItem("strSector")
		End Set
	End Property

#Region "Control Documentos Detalles Recibos"

	Private _mstrArchivoCheque As String
	Public Property mstrArchivoCheque() As String
		Get
			Return _mstrArchivoCheque
		End Get
		Set(ByVal value As String)
			_mstrArchivoCheque = value
			MyBase.CambioItem("mstrArchivoCheque")
		End Set
	End Property

	Private _strUsuario As String = Program.Usuario
	Public Property strUsuario() As String
		Get
			Return _strUsuario
		End Get
		Set(ByVal value As String)
			_strUsuario = value
			MyBase.CambioItem("strUsuario")
		End Set
	End Property

	Private _mstrRutaCheque As String
	Public Property mstrRutaCheque() As String
		Get
			Return _mstrRutaCheque
		End Get
		Set(ByVal value As String)
			_mstrRutaCheque = value
			MyBase.CambioItem("mstrRutaCheque")
		End Set
	End Property

	Private _mabytArchivoCheque As Byte()
	Public Property mabytArchivoCheque As Byte()
		Get
			Return _mabytArchivoCheque
		End Get
		Set(ByVal value As Byte())
			_mabytArchivoCheque = value
			MyBase.CambioItem("mabytArchivoCheque")
		End Set
	End Property

	Private _mobjCtlSubirArchivoCheque As A2DocumentosWPF.A2SubirDocumento
	Public Property mobjCtlSubirArchivoCheque() As A2DocumentosWPF.A2SubirDocumento
		Get
			Return _mobjCtlSubirArchivoCheque
		End Get
		Set(ByVal value As A2DocumentosWPF.A2SubirDocumento)
			_mobjCtlSubirArchivoCheque = value
			MyBase.CambioItem("mobjCtlSubirArchivoCheque")
		End Set
	End Property

	'------------------Transferencias-------------------------------
	Private _mstrArchivoTransferencia As String
	Public Property mstrArchivoTransferencia() As String
		Get
			Return _mstrArchivoTransferencia
		End Get
		Set(ByVal value As String)
			_mstrArchivoTransferencia = value
			MyBase.CambioItem("mstrArchivoTransferencia")
		End Set
	End Property

	Private _mstrRutaTransferencia As String
	Public Property mstrRutaTransferencia() As String
		Get
			Return _mstrRutaTransferencia
		End Get
		Set(ByVal value As String)
			_mstrRutaTransferencia = value
			MyBase.CambioItem("mstrRutaTransferencia")
		End Set
	End Property
	Private _mabytArchivoTransferencia As Byte()
	Public Property mabytArchivoTransferencia As Byte()
		Get
			Return _mabytArchivoTransferencia
		End Get
		Set(ByVal value As Byte())
			_mabytArchivoTransferencia = value
			MyBase.CambioItem("mabytArchivoTransferencia")
		End Set
	End Property

	Private _mobjCtlSubirArchivoTransferencia As A2DocumentosWPF.A2SubirDocumento
	Public Property mobjCtlSubirArchivoTransferencia() As A2DocumentosWPF.A2SubirDocumento
		Get
			Return _mobjCtlSubirArchivoTransferencia
		End Get
		Set(ByVal value As A2DocumentosWPF.A2SubirDocumento)
			_mobjCtlSubirArchivoTransferencia = value
			MyBase.CambioItem("mobjCtlSubirArchivoTransferencia")
		End Set
	End Property
	'---------------Consignaciones---------------
	Private _mstrArchivoConsignacion As String
	Public Property mstrArchivoConsignacion() As String
		Get
			Return _mstrArchivoConsignacion
		End Get
		Set(ByVal value As String)
			_mstrArchivoConsignacion = value
			MyBase.CambioItem("mstrArchivoConsignacion")
		End Set
	End Property

	Private _mstrRutaConsignacion As String
	Public Property mstrRutaConsignacion() As String
		Get
			Return _mstrRutaConsignacion
		End Get
		Set(ByVal value As String)
			_mstrRutaConsignacion = value
			MyBase.CambioItem("mstrRutaConsignacion")
		End Set
	End Property

	Private _mabytArchivoConsignacion As Byte()
	Public Property mabytArchivoConsignacion As Byte()
		Get
			Return _mabytArchivoConsignacion
		End Get
		Set(ByVal value As Byte())
			_mabytArchivoConsignacion = value
			MyBase.CambioItem("mabytArchivoConsignacion")
		End Set
	End Property

	Private _mobjCtlSubirArchivoConsignacion As A2DocumentosWPF.A2SubirDocumento
	Public Property mobjCtlSubirArchivoConsignacion() As A2DocumentosWPF.A2SubirDocumento
		Get
			Return _mobjCtlSubirArchivoConsignacion
		End Get
		Set(ByVal value As A2DocumentosWPF.A2SubirDocumento)
			_mobjCtlSubirArchivoConsignacion = value
			MyBase.CambioItem("mobjCtlSubirArchivoConsignacion")
		End Set
	End Property

#End Region

	Private _HabilitarEdicionEncabezado As Boolean
	Public Property HabilitarEdicionEncabezado() As Boolean
		Get
			Return _HabilitarEdicionEncabezado
		End Get
		Set(ByVal value As Boolean)
			_HabilitarEdicionEncabezado = value
			MyBase.CambioItem("HabilitarEdicionEncabezado")
		End Set
	End Property

#Region "VALORES TOTALES PARA NOTA BLOQUEO"
	Public _ValorTotalNota_Cheque As Decimal
	Public Property ValorTotalNota_Cheque() As Decimal
		Get
			Return _ValorTotalNota_Cheque
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalNota_Cheque = value
			MyBase.CambioItem("ValorTotalNota_Cheque")
		End Set
	End Property

	Public _ValorTotalNota_Transferencia As Decimal
	Public Property ValorTotalNota_Transferencia() As Decimal
		Get
			Return _ValorTotalNota_Transferencia
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalNota_Transferencia = value
			MyBase.CambioItem("ValorTotalNota_Transferencia")
		End Set
	End Property

	Public _ValorTotalNota_Carteras As Decimal
	Public Property ValorTotalNota_Carteras() As Decimal
		Get
			Return _ValorTotalNota_Carteras
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalNota_Carteras = value
			MyBase.CambioItem("ValorTotalNota_Carteras")
		End Set
	End Property

	Public _ValorTotalNota_Internos As Decimal
	Public Property ValorTotalNota_Internos() As Decimal
		Get
			Return _ValorTotalNota_Internos
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalNota_Internos = value
			MyBase.CambioItem("ValorTotalNota_Internos")
		End Set
	End Property
#End Region

	Private _cargaAutomaticaActiva As Boolean
	''' <summary>
	''' Activa o desactiva la recarga de ordenes mediante temporizador
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property cargaAutomaticaActiva() As Boolean
		Get
			Return _cargaAutomaticaActiva
		End Get
		Set(ByVal value As Boolean)
			_cargaAutomaticaActiva = value
		End Set
	End Property

	Private _DescripcionBancoTransferencia As String
	Public Property DescripcionBancoTransferencia() As String
		Get
			Return _DescripcionBancoTransferencia
		End Get
		Set(ByVal value As String)
			_DescripcionBancoTransferencia = value
			MyBase.CambioItem("DescripcionBancoTransferencia")
		End Set
	End Property

	Private _IdBancoTransferencia As Integer
	Public Property IdBancoTransferencia() As Integer
		Get
			Return _IdBancoTransferencia
		End Get
		Set(ByVal value As Integer)
			_IdBancoTransferencia = value
			MyBase.CambioItem("IdBancoTransferencia")
		End Set
	End Property

	'DEMC20180612 INICIO
	'Private _IdBanco As Integer
	Private _IdBanco As Nullable(Of Integer)
	'Public Property IdBanco() As Integer
	Public Property IdBanco() As Nullable(Of Integer)
		Get
			Return _IdBanco
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdBanco = value
			MyBase.CambioItem("IdBanco")
		End Set
	End Property
	'DEMC20180612 FIN

	Private _ListaValidacionesListaRestrictiva As List(Of tblRespuestaValidacionesTesoreria)
	Public Property ListaValidacionesListaRestrictiva() As List(Of tblRespuestaValidacionesTesoreria)
		Get
			Return _ListaValidacionesListaRestrictiva
		End Get
		Set(ByVal value As List(Of tblRespuestaValidacionesTesoreria))
			_ListaValidacionesListaRestrictiva = value
			MyBase.CambioItem("ListaValidacionesListaRestrictiva")
		End Set
	End Property

	Private _VerItemActual As Visibility
	Public Property VerItemActual() As Visibility
		Get
			Return _VerItemActual
		End Get
		Set(ByVal value As Visibility)
			_VerItemActual = value
			MyBase.CambioItem("VerItemActual")
		End Set
	End Property

	Private _TabItemActual As String
	Public Property TabItemActual() As String
		Get
			Return _TabItemActual
		End Get
		Set(ByVal value As String)
			_TabItemActual = value
			MyBase.CambioItem("TabItemActual")
		End Set
	End Property

	Private _OrdenesReciboPLUSView As OrdenesReciboPLUSView
	Public Property OrdenesReciboPLUSView() As OrdenesReciboPLUSView
		Get
			Return _OrdenesReciboPLUSView
		End Get
		Set(ByVal value As OrdenesReciboPLUSView)
			_OrdenesReciboPLUSView = value


			MyBase.CambioItem("OrdenesReciboPLUSView")
		End Set
	End Property

	Private _BorrarCliente As Boolean
	Public Property BorrarCliente() As Boolean
		Get
			Return _BorrarCliente
		End Get
		Set(ByVal value As Boolean)
			_BorrarCliente = value
			MyBase.CambioItem("BorrarCliente")
		End Set
	End Property

	Private _ValorTotalGenerarActual As Decimal = 0
	Public Property ValorTotalGenerarActual() As Decimal
		Get
			Return _ValorTotalGenerarActual
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalGenerarActual = value
			MyBase.CambioItem("ValorTotalGenerarActual")
		End Set
	End Property

	Private _ValorCargarPagoA As Decimal
	Public Property ValorCargarPagoA() As Decimal
		Get
			Return _ValorCargarPagoA
		End Get
		Set(ByVal value As Decimal)
			_ValorCargarPagoA = value
			MyBase.CambioItem("ValorCargarPagoA")
		End Set
	End Property

	Private _ValorCargarPagoAFondos As Decimal
	Public Property ValorCargarPagoAFondos() As Decimal
		Get
			Return _ValorCargarPagoAFondos
		End Get
		Set(ByVal value As Decimal)
			_ValorCargarPagoAFondos = value
			MyBase.CambioItem("ValorCargarPagoAFondos")
		End Set
	End Property

	Private _ValorTotalSumatoria As Decimal
	Public Property ValorTotalSumatoria() As Decimal
		Get
			Return _ValorTotalSumatoria
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalSumatoria = value
			MyBase.CambioItem("ValorTotalSumatoria")
		End Set
	End Property

	Private _ValorTotalGenerarTransferencia As Decimal = 0
	Public Property ValorTotalGenerarTransferencia() As Decimal
		Get
			Return _ValorTotalGenerarTransferencia
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalGenerarTransferencia = value
			MyBase.CambioItem("ValorTotalGenerarTransferencia")
		End Set
	End Property

	Private _ValorTotalGenerarConsignacion As Decimal = 0
	Public Property ValorTotalGenerarConsignacion() As Decimal
		Get
			Return _ValorTotalGenerarConsignacion
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalGenerarConsignacion = value
			MyBase.CambioItem("ValorTotalGenerarConsignacion")
		End Set
	End Property


#Region "Propiedades ValidarIngreso"

	Private _CantidadAprobaciones As Integer = 0
	Public Property CantidadAprobaciones() As Integer
		Get
			Return _CantidadAprobaciones
		End Get
		Set(ByVal value As Integer)
			_CantidadAprobaciones = value
			If CantidadAprobaciones > 0 Then
				If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
					ValidarIngresoOrdenTesoreria()
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
			MyBase.CambioItem("Confirmaciones")
		End Set
	End Property

	Private _ConfirmacionesUsuario As String = String.Empty
	Public Property ConfirmacionesUsuario() As String
		Get
			Return _ConfirmacionesUsuario
		End Get
		Set(ByVal value As String)
			_ConfirmacionesUsuario = value
			MyBase.CambioItem("ConfirmacionesUsuario")
		End Set
	End Property

	Private _ConfirmacionesListasRestrictivas As String = String.Empty
	Public Property ConfirmacionesListasRestrictivas() As String
		Get
			Return _ConfirmacionesListasRestrictivas
		End Get
		Set(ByVal value As String)
			_ConfirmacionesListasRestrictivas = value
		End Set
	End Property

	Private _Justificaciones As String = String.Empty
	Public Property Justificaciones() As String
		Get
			Return _Justificaciones
		End Get
		Set(ByVal value As String)
			_Justificaciones = value
			MyBase.CambioItem("Justificaciones")
		End Set
	End Property
	Private _JustificacionesListasRestrictivas As String = String.Empty
	Public Property JustificacionesListasRestrictivas() As String
		Get
			Return _JustificacionesListasRestrictivas
		End Get
		Set(ByVal value As String)
			_JustificacionesListasRestrictivas = value
			MyBase.CambioItem("JustificacionesListasRestrictivas")
		End Set
	End Property

	Private _JustificacionesUsuario As String = String.Empty
	Public Property JustificacionesUsuario() As String
		Get
			Return _JustificacionesUsuario
		End Get
		Set(ByVal value As String)
			_JustificacionesUsuario = value
			MyBase.CambioItem("JustificacionesUsuario")
		End Set
	End Property
	Private _JustificacionesUsuarioListasRestrictivas As String = String.Empty
	Public Property JustificacionesUsuarioListasRestrictivas() As String
		Get
			Return _JustificacionesUsuarioListasRestrictivas
		End Get
		Set(ByVal value As String)
			_JustificacionesUsuarioListasRestrictivas = value
		End Set
	End Property

	Private _Aprobaciones As String
	Public Property Aprobaciones() As String
		Get
			Return _Aprobaciones
		End Get
		Set(ByVal value As String)
			_Aprobaciones = value
			MyBase.CambioItem("Aprobaciones")
		End Set
	End Property
	Private _AprobacionesListasRestrictivas As String
	Public Property AprobacionesListasRestrictivas() As String
		Get
			Return _AprobacionesListasRestrictivas
		End Get
		Set(ByVal value As String)
			_AprobacionesListasRestrictivas = value
		End Set
	End Property

	Private _AprobacionesUsuarioListasRestrictivas As String
	Public Property AprobacionesUsuarioListasRestrictivas() As String
		Get
			Return _AprobacionesUsuarioListasRestrictivas
		End Get
		Set(ByVal value As String)
			_AprobacionesUsuarioListasRestrictivas = value
		End Set
	End Property
	Private _AprobacionesUsuario As String
	Public Property AprobacionesUsuario() As String
		Get
			Return _AprobacionesUsuario
		End Get
		Set(ByVal value As String)
			_AprobacionesUsuario = value
			MyBase.CambioItem("AprobacionesUsuario")
		End Set
	End Property

#End Region

	Private _ValorTotalOrdenRecibo As Decimal
	Public Property ValorTotalOrdenRecibo() As Decimal
		Get
			Return _ValorTotalOrdenRecibo
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalOrdenRecibo = value
			MyBase.CambioItem("ValorTotalOrdenRecibo")
		End Set
	End Property

	Private _TipoIdClienteDescripcion As String
	Public Property TipoIdClienteDescripcion() As String
		Get
			Return _TipoIdClienteDescripcion
		End Get
		Set(ByVal value As String)
			_TipoIdClienteDescripcion = value
			MyBase.CambioItem("TipoIdClienteDescripcion")
		End Set
	End Property

	Private _strCodigoOyDCliente As String
	Public Property strCodigoOyDCliente() As String
		Get
			Return _strCodigoOyDCliente
		End Get
		Set(ByVal value As String)
			_strCodigoOyDCliente = value
			MyBase.CambioItem("strCodigoOyDCliente")
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

	Private _ConsultarSaldo As Boolean
	Public Property ConsultarSaldo() As Boolean
		Get
			Return _ConsultarSaldo
		End Get
		Set(ByVal value As Boolean)
			_ConsultarSaldo = value
			MyBase.CambioItem("ConsultarSaldo")
		End Set
	End Property

	Private _cb As CamposBusquedaTesoreriaOyDPLUS = New CamposBusquedaTesoreriaOyDPLUS
	Public Property cb() As CamposBusquedaTesoreriaOyDPLUS
		Get
			Return _cb
		End Get
		Set(ByVal value As CamposBusquedaTesoreriaOyDPLUS)
			_cb = value
			MyBase.CambioItem("cb")
		End Set
	End Property

	Private _HabilitarImportacion As Boolean = False
	Public Property HabilitarImportacion() As Boolean
		Get
			Return _HabilitarImportacion
		End Get
		Set(ByVal value As Boolean)
			_HabilitarImportacion = value
			MyBase.CambioItem("HabilitarImportacion")
		End Set
	End Property

	Private _HabilitarCuentasRegistradas As Boolean = False
	Public Property HabilitarCuentasRegistradas() As Boolean
		Get
			Return _HabilitarCuentasRegistradas
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCuentasRegistradas = value
			MyBase.CambioItem("HabilitarCuentasRegistradas")
		End Set
	End Property

	Private _ValorGenerarTransferencia As Decimal
	Public Property ValorGenerarTransferencia() As Decimal
		Get
			Return _ValorGenerarTransferencia
		End Get
		Set(ByVal value As Decimal)
			_ValorGenerarTransferencia = value
			If Not IsNothing(_ValorGenerarTransferencia) Then
				If logCambiarPropiedadesPOPPUP Then

				End If
			End If
			MyBase.CambioItem("ValorGenerarTransferencia")
		End Set
	End Property

	Private _DescripcionConceptoTransferencia As String
	Public Property DescripcionConceptoTransferencia() As String
		Get
			Return _DescripcionConceptoTransferencia
		End Get
		Set(ByVal value As String)
			_DescripcionConceptoTransferencia = value
			MyBase.CambioItem("DescripcionConceptoTransferencia")
		End Set
	End Property

	Private _HabilitarCampoGMFTransferencia As Boolean
	Public Property HabilitarCampoGMFTransferencia() As Boolean
		Get
			Return _HabilitarCampoGMFTransferencia
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCampoGMFTransferencia = value
			MyBase.CambioItem("HabilitarCampoGMFTransferencia")
		End Set
	End Property

	Private _DescripcionTipoIdentificacionTitularTransferencia As String
	Public Property DescripcionTipoIdentificacionTitularTransferencia() As String
		Get
			Return _DescripcionTipoIdentificacionTitularTransferencia
		End Get
		Set(ByVal value As String)
			_DescripcionTipoIdentificacionTitularTransferencia = value
			MyBase.CambioItem("DescripcionTipoIdentificacionTitularTransferencia")
		End Set
	End Property

	Private _strTipoIdentificacionTitularWpp As String
	Public Property strTipoIdentificacionTitularWpp() As String
		Get
			Return _strTipoIdentificacionTitularWpp
		End Get
		Set(ByVal value As String)
			_strTipoIdentificacionTitularWpp = value
			If Not IsNothing(DiccionarioCombosOYDPlus) Then
				If DiccionarioCombosOYDPlus.ContainsKey("TIPOID") Then
					For Each li In DiccionarioCombosOYDPlus("TIPOID").Where(Function(i) i.Retorno = _strTipoIdentificacionTitularWpp)
						DescripcionTipoIdentificacionTitularTransferencia = li.Descripcion
					Next
				End If
			End If
			MyBase.CambioItem("strTipoIdentificacionTitularWpp")
		End Set
	End Property

	Private _strNombreTitularWpp As String
	Public Property strNombreTitularWpp() As String
		Get
			Return _strNombreTitularWpp
		End Get
		Set(ByVal value As String)
			_strNombreTitularWpp = value
			MyBase.CambioItem("strNombreTitularWpp")
		End Set
	End Property

	Private _strNroDocumentoTitularWpp As String
	Public Property strNroDocumentoTitularWpp() As String
		Get
			Return _strNroDocumentoTitularWpp
		End Get
		Set(ByVal value As String)
			_strNroDocumentoTitularWpp = value
			If Not String.IsNullOrEmpty(_strNroDocumentoTitularWpp) Then
				If logCambiarPropiedadesPOPPUP Then
					If Not String.IsNullOrEmpty(LTrim(RTrim(_strNroDocumentoTitularWpp))) And Not String.IsNullOrEmpty(strTipoIdentificacionTitularWpp) Then
						If TesoreriaOrdenesPlusRC_Selected.strNroDocumento <> LTrim(RTrim(_strNroDocumentoTitularWpp)) Or
							TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion <> strTipoIdentificacionTitularWpp Then
							HabilitarCampoGMFTransferencia = True
							logEsTerceroTransferencia = True

						Else
							logEsTerceroTransferencia = False
							HabilitarCampoGMFTransferencia = False
							HabilitarCampoGMFTransferencia = False
						End If
					End If
				End If
			End If
			MyBase.CambioItem("strNroDocumentoTitularWpp")
		End Set
	End Property

	Private _BancoChequewpp As String
	Public Property BancoChequewpp() As String
		Get
			Return _BancoChequewpp
		End Get
		Set(ByVal value As String)
			_BancoChequewpp = value
			MyBase.CambioItem("BancoChequewpp")
		End Set
	End Property

	Private _BancoChequeConsignacionwpp As String
	Public Property BancoChequeConsignacionwpp() As String
		Get
			Return _BancoChequeConsignacionwpp
		End Get
		Set(ByVal value As String)
			_BancoChequeConsignacionwpp = value
			MyBase.CambioItem("BancoChequeConsignacionwpp")
		End Set
	End Property

	Private _BancoTransferenciaDescripcionDestinowpp As String
	Public Property BancoTransferenciaDescripcionDestinowpp() As String
		Get
			Return _BancoTransferenciaDescripcionDestinowpp
		End Get
		Set(ByVal value As String)
			_BancoTransferenciaDescripcionDestinowpp = value
			MyBase.CambioItem("BancoTransferenciaDescripcionDestinowpp")
		End Set
	End Property

	Private _lngCodigoBancoDestinoTransferenciaWpp As Nullable(Of Integer)
	Public Property lngCodigoBancoDestinoTransferenciaWpp() As Nullable(Of Integer)
		Get
			Return _lngCodigoBancoDestinoTransferenciaWpp
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_lngCodigoBancoDestinoTransferenciaWpp = value
			MyBase.CambioItem("lngCodigoBancoDestinoTransferenciaWpp")
		End Set
	End Property

	Private _lngCodigoBancoDestinoConsignacionWpp As Nullable(Of Integer)
	Public Property lngCodigoBancoDestinoConsignacionWpp() As Nullable(Of Integer)
		Get
			Return _lngCodigoBancoDestinoConsignacionWpp
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_lngCodigoBancoDestinoConsignacionWpp = value
			MyBase.CambioItem("lngCodigoBancoDestinoConsignacionWpp")
		End Set
	End Property

	'DEMC20180612 INICIO
	'Private _lngCodigoBancoWpp As Integer
	Private _lngCodigoBancoWpp As Nullable(Of Integer)
	'Public Property lngCodigoBancoWpp() As Integer
	Public Property lngCodigoBancoWpp() As Nullable(Of Integer)
		Get
			Return _lngCodigoBancoWpp
		End Get
		'Set(ByVal value As Integer)
		Set(ByVal value As Nullable(Of Integer))
			_lngCodigoBancoWpp = value
			MyBase.CambioItem("lngCodigoBancoWpp")
		End Set
	End Property
	'DEMC20180612 FIN

	'DEMC20180612 INICIO
	'Private _lngCodigoBancoConsignacionWpp As Integer
	Private _lngCodigoBancoConsignacionWpp As Nullable(Of Integer)
	'Public Property lngCodigoBancoConsignacionWpp() As Integer
	Public Property lngCodigoBancoConsignacionWpp() As Nullable(Of Integer)
		Get
			Return _lngCodigoBancoConsignacionWpp
		End Get
		'Set(ByVal value As Integer)
		Set(ByVal value As Nullable(Of Integer))
			_lngCodigoBancoConsignacionWpp = value
			MyBase.CambioItem("lngCodigoBancoConsignacionWpp")
		End Set
	End Property
	'DEMC20180612 FIN

	Private _strDescripcionBancoConsignacion As String
	Public Property strDescripcionBancoConsignacion() As String
		Get
			Return _strDescripcionBancoConsignacion
		End Get
		Set(ByVal value As String)
			_strDescripcionBancoConsignacion = value
			MyBase.CambioItem("strDescripcionBancoConsignacion")
		End Set
	End Property

	Private _strNroCuentaWpp As String
	Public Property strNroCuentaWpp() As String
		Get
			Return _strNroCuentaWpp
		End Get
		Set(ByVal value As String)
			_strNroCuentaWpp = value
			MyBase.CambioItem("strNroCuentaWpp")
		End Set
	End Property

	Private _DescripcionTipoCuentaDestinoTransferencia As String
	Public Property DescripcionTipoCuentaDestinoTransferencia() As String
		Get
			Return _DescripcionTipoCuentaDestinoTransferencia
		End Get
		Set(ByVal value As String)
			_DescripcionTipoCuentaDestinoTransferencia = value
			MyBase.CambioItem("DescripcionTipoCuentaDestinoTransferencia")
		End Set
	End Property

	Private _DescripcionTipoCuentaTransferencia As String
	Public Property DescripcionTipoCuentaTransferencia() As String
		Get
			Return _DescripcionTipoCuentaTransferencia
		End Get
		Set(ByVal value As String)
			_DescripcionTipoCuentaTransferencia = value
			MyBase.CambioItem("DescripcionTipoCuentaTransferencia")
		End Set
	End Property

	Private _strValorTipoCuentaWpp As String
	Public Property strValorTipoCuentaWpp() As String
		Get
			Return _strValorTipoCuentaWpp
		End Get
		Set(ByVal value As String)
			_strValorTipoCuentaWpp = value
			If Not IsNothing(DiccionarioCombosOYDPlus) Then
				If DiccionarioCombosOYDPlus.ContainsKey("TIPOCUENTABANCARIA") Then
					For Each li In DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Retorno = _strValorTipoCuentaWpp)
						DescripcionTipoCuentaTransferencia = li.Descripcion
					Next
				End If
			End If
			MyBase.CambioItem("strValorTipoCuentaWpp")
		End Set
	End Property

	Private _ListaCuentasClientes As List(Of TempCuentasClientes)
	Public Property ListaCuentasClientes() As List(Of TempCuentasClientes)
		Get
			Return _ListaCuentasClientes
		End Get
		Set(ByVal value As List(Of TempCuentasClientes))

			_ListaCuentasClientes = value
			MyBase.CambioItem("ListaCuentasClientes")
		End Set
	End Property

	Private _ListaDireccionesClientes As List(Of TempDireccionesClientes)
	Public Property ListaDireccionesClientes() As List(Of TempDireccionesClientes)
		Get
			Return _ListaDireccionesClientes
		End Get
		Set(ByVal value As List(Of TempDireccionesClientes))

			_ListaDireccionesClientes = value
			MyBase.CambioItem("ListaCuentasClientes")
		End Set
	End Property

	Private _HabilitarEnEdicion As Boolean
	Public Property HabilitarEnEdicion() As Boolean
		Get
			Return _HabilitarEnEdicion
		End Get
		Set(ByVal value As Boolean)
			_HabilitarEnEdicion = value
			MyBase.CambioItem("HabilitarEnEdicion")
		End Set
	End Property

	Private _HabilitarReceptor As Boolean = False
	Public Property HabilitarReceptor() As Boolean
		Get
			Return _HabilitarReceptor
		End Get
		Set(ByVal value As Boolean)
			_HabilitarReceptor = value
			MyBase.CambioItem("HabilitarReceptor")
		End Set
	End Property

	Private _strValorTipoDoc As String
	Public Property strValorTipoDoc As String
		Get
			Return _strValorTipoDoc
		End Get
		Set(value As String)
			_strValorTipoDoc = value
			MyBase.CambioItem("strValorTipoDoc")
		End Set
	End Property

	Private _strValorTipoDocTransferencia As String
	Public Property strValorTipoDocTransferencia As String
		Get
			Return _strValorTipoDocTransferencia
		End Get
		Set(value As String)
			_strValorTipoDocTransferencia = value
			MyBase.CambioItem("strValorTipoDocTransferencia")
		End Set
	End Property

	Private _IDCuentaInscrita As String
	Public Property IDCuentaInscrita() As String
		Get
			Return _IDCuentaInscrita
		End Get
		Set(ByVal value As String)
			_IDCuentaInscrita = value
			MyBase.CambioItem("IDCuentaInscrita")
		End Set
	End Property

	Private _HabilitarComboCuentasNroCuenta As Boolean
	Public Property HabilitarComboCuentasNroCuenta() As Boolean
		Get
			Return _HabilitarComboCuentasNroCuenta
		End Get
		Set(ByVal value As Boolean)
			_HabilitarComboCuentasNroCuenta = value
			MyBase.CambioItem("HabilitarComboCuentasNroCuenta")
		End Set
	End Property

	Private _HabilitarComboCuentasNroDocumentoTitular As Boolean
	Public Property HabilitarComboCuentasNroDocumentoTitular() As Boolean
		Get
			Return _HabilitarComboCuentasNroDocumentoTitular
		End Get
		Set(ByVal value As Boolean)
			_HabilitarComboCuentasNroDocumentoTitular = value
			MyBase.CambioItem("HabilitarComboCuentasNroDocumentoTitular")
		End Set
	End Property

	Private _HabilitarComboCuentasNombreTitular As Boolean
	Public Property HabilitarComboCuentasNombreTitular() As Boolean
		Get
			Return _HabilitarComboCuentasNombreTitular
		End Get
		Set(ByVal value As Boolean)
			_HabilitarComboCuentasNombreTitular = value
			MyBase.CambioItem("HabilitarComboCuentasNombreTitular")
		End Set
	End Property

	Private _HabilitarComboCuentasCliente As Boolean
	Public Property HabilitarComboCuentasCliente() As Boolean
		Get
			Return _HabilitarComboCuentasCliente
		End Get
		Set(ByVal value As Boolean)
			_HabilitarComboCuentasCliente = value
			MyBase.CambioItem("HabilitarComboCuentasCliente")
		End Set
	End Property

	Private _strCtaRegistrada As String
	Public Property strCtaRegistrada() As String
		Get
			Return _strCtaRegistrada
		End Get
		Set(ByVal value As String)
			_strCtaRegistrada = value
			MyBase.CambioItem("strCtaRegistrada")
		End Set
	End Property

	Private _CuentaRegistrada As Integer
	Public Property CuentaRegistrada() As Integer
		Get
			Return _CuentaRegistrada
		End Get
		Set(ByVal value As Integer)
			_CuentaRegistrada = value
			If _CuentaRegistrada > 0 Then
				If Not IsNothing(ListaCuentasClientes) Then
					For Each li In ListaCuentasClientes.Where(Function(i) i.ID = _CuentaRegistrada)
						CargarInformacionCuentaRegistrada(li.strCuenta,
														  li.strTipoCuenta,
														  li.strValorTipoCuenta,
														  li.lngIDBanco,
														  li.strNombreTitular,
														  li.strNombreBanco,
														  li.strTipoDocumento,
														  li.strValorTipoDocumento,
														  li.strNroDocumento,
														  li.logDividendos,
														  li.logOperacion,
														  li.logTransferir,
														  li.logUnicoTitular)
					Next
				End If
			End If
			MyBase.CambioItem("CuentaRegistrada")
		End Set
	End Property

	Private _strValorTipoCuentaDestinoWpp As String
	Public Property strValorTipoCuentaDestinoWpp() As String
		Get
			Return _strValorTipoCuentaDestinoWpp
		End Get
		Set(ByVal value As String)
			_strValorTipoCuentaDestinoWpp = value
			If Not IsNothing(DiccionarioCombosOYDPlus) Then
				If DiccionarioCombosOYDPlus.ContainsKey("TIPOCUENTABANCARIA") Then
					For Each li In DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Retorno = _strValorTipoCuentaDestinoWpp)
						DescripcionTipoCuentaDestinoTransferencia = li.Descripcion
					Next
				End If
			End If
			MyBase.CambioItem("strValorTipoCuentaDestinoWpp")
		End Set
	End Property

	Private _strNroCuentaDestinoWpp As String
	Public Property strNroCuentaDestinoWpp() As String
		Get
			Return _strNroCuentaDestinoWpp
		End Get
		Set(ByVal value As String)
			_strNroCuentaDestinoWpp = value
			MyBase.CambioItem("strNroCuentaDestinoWpp")
		End Set
	End Property

	Private _IdTipoCuentaRegistrada As String
	Public Property IdTipoCuentaRegistrada() As String
		Get
			Return _IdTipoCuentaRegistrada
		End Get
		Set(ByVal value As String)
			_IdTipoCuentaRegistrada = value
			If Not String.IsNullOrEmpty(_IdTipoCuentaRegistrada) Then
				If logCambiarPropiedadesPOPPUP Then

					If _IdTipoCuentaRegistrada = GSTR_CUENTA_REGISTRADA Then
						HabilitarComboCuentasCliente = False
						HabilitarComboCuentasNombreTitular = False
						HabilitarComboCuentasNroCuenta = False
						HabilitarComboCuentasNroDocumentoTitular = False
						VerComboCuentasRegistradas = Visibility.Visible
						HabilitarCuentasRegistradas = True

						If Not IsNothing(ListaCuentasClientes) Then
							If ListaCuentasClientes.Count = 1 Then
								CuentaRegistrada = ListaCuentasClientes.First.ID
							End If
						End If

						logEsCuentaRegistrada = True
						LimpiarCuenta = Nothing

					Else
						HabilitarCuentasRegistradas = False
						HabilitarComboCuentasCliente = True
						HabilitarComboCuentasNombreTitular = True
						HabilitarComboCuentasNroCuenta = True
						HabilitarComboCuentasNroDocumentoTitular = True


						strValorTipoCuentaWpp = String.Empty
						logEsCuentaRegistrada = False

						CuentaRegistrada = Nothing
						strNombreTitularWpp = String.Empty
						strTipoIdentificacionTitularWpp = Nothing
						strNroDocumentoTitularWpp = String.Empty
						strNroCuentaWpp = String.Empty
						strValorTipoCuentaWpp = Nothing
						lngCodigoBancoWpp = Nothing
						DescripcionBancoTransferencia = String.Empty
						HabilitarComboCuentasCliente = True
						VerComboCuentasRegistradas = Visibility.Collapsed
					End If

				End If
			End If

			MyBase.CambioItem("IdTipoCuentaRegistrada")
		End Set
	End Property

	Private _VerComboCuentasRegistradas As Visibility = Visibility.Collapsed
	Public Property VerComboCuentasRegistradas() As Visibility
		Get
			Return _VerComboCuentasRegistradas
		End Get
		Set(ByVal value As Visibility)
			_VerComboCuentasRegistradas = value
			MyBase.CambioItem("VerComboCuentasRegistradas")
		End Set
	End Property

	Private _LimpiarCuenta As String
	Public Property LimpiarCuenta() As String
		Get
			Return _LimpiarCuenta
		End Get
		Set(ByVal value As String)
			_LimpiarCuenta = value
			MyBase.CambioItem("LimpiarCuenta")
		End Set
	End Property

	Private _HabilitarCampoTransferencia As Boolean
	Public Property HabilitarCampoTransferencia() As Boolean
		Get
			Return _HabilitarCampoTransferencia
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCampoTransferencia = value
			MyBase.CambioItem("HabilitarCampoTransferencia")
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
					ValidarIngresoOrdenTesoreria()
				End If
			End If
		End Set
	End Property

	Private _ListaResultadoValidacion As List(Of OyDPLUSTesoreria.tblRespuestaValidaciones)
	Public Property ListaResultadoValidacion() As List(Of OyDPLUSTesoreria.tblRespuestaValidaciones)
		Get
			Return _ListaResultadoValidacion
		End Get
		Set(ByVal value As List(Of OyDPLUSTesoreria.tblRespuestaValidaciones))
			_ListaResultadoValidacion = value
			MyBase.CambioItem("ListaResultadoValidacion")
		End Set
	End Property

	Private _HabilitarEncabezado As Boolean
	Public Property HabilitarEncabezado() As Boolean
		Get
			Return _HabilitarEncabezado
		End Get
		Set(ByVal value As Boolean)
			_HabilitarEncabezado = value
			MyBase.CambioItem("HabilitarEncabezado")
		End Set
	End Property

	Private _HabilitarEntregar As Boolean
	Public Property HabilitarEntregar() As Boolean
		Get
			Return _HabilitarEntregar
		End Get
		Set(ByVal value As Boolean)
			_HabilitarEntregar = value
			MyBase.CambioItem("HabilitarEntregar")
		End Set
	End Property

	Private _HabilitarEntregarA As Boolean
	Public Property HabilitarEntregarA() As Boolean
		Get
			Return _HabilitarEntregarA
		End Get
		Set(ByVal value As Boolean)
			_HabilitarEntregarA = value
			MyBase.CambioItem("HabilitarEntregarA")
		End Set
	End Property

	Private _VerDuplicar As Visibility = Visibility.Visible
	Public Property VerDuplicar() As Visibility
		Get
			Return _VerDuplicar
		End Get
		Set(ByVal value As Visibility)
			_VerDuplicar = value
			MyBase.CambioItem("VerDuplicar")
		End Set
	End Property

	Private _HabilitarInstrucciones As Boolean = False
	Public Property HabilitarInstrucciones() As Boolean
		Get
			Return _HabilitarInstrucciones
		End Get
		Set(ByVal value As Boolean)
			_HabilitarInstrucciones = value

			MyBase.CambioItem("HabilitarInstrucciones")
		End Set
	End Property

	Private _LimpiarBuscadorCliente As String
	Public Property LimpiarBuscadorCliente() As String
		Get
			Return _LimpiarBuscadorCliente
		End Get
		Set(ByVal value As String)
			_LimpiarBuscadorCliente = value
			MyBase.CambioItem("LimpiarBuscadorCliente")
		End Set
	End Property

	Private _HabilitarCamposValidacion As Boolean
	Public Property HabilitarCamposValidacion() As Boolean
		Get
			Return _HabilitarCamposValidacion
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCamposValidacion = value
			MyBase.CambioItem("HabilitarCamposValidacion")
		End Set
	End Property

	Private _HabilitarCamposPopup As Boolean
	Public Property HabilitarCamposPopup() As Boolean
		Get
			Return _HabilitarCamposPopup
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCamposPopup = value
			MyBase.CambioItem("HabilitarCamposPopup")
		End Set
	End Property

	Private _HabilitarCampoBeneficiario As Boolean
	Public Property HabilitarCampoBeneficiario() As Boolean
		Get
			Return _HabilitarCampoBeneficiario
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCampoBeneficiario = value
			MyBase.CambioItem("HabilitarCampoBeneficiario")
		End Set
	End Property

	Private _HabilitarCampoNroDocumento As Boolean = True
	Public Property HabilitarCampoNroDocumento() As Boolean
		Get
			Return _HabilitarCampoNroDocumento
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCampoNroDocumento = value
			MyBase.CambioItem("HabilitarCampoNroDocumento")
		End Set
	End Property

	Private _DescripcionTipoIdentificacionCheque As String
	Public Property DescripcionTipoIdentificacionCheque() As String
		Get
			Return _DescripcionTipoIdentificacionCheque
		End Get
		Set(ByVal value As String)
			_DescripcionTipoIdentificacionCheque = value
			MyBase.CambioItem("DescripcionTipoIdentificacionCheque")
		End Set
	End Property

	Private _strTipoIdentificacionWpp As String
	Public Property strTipoIdentificacionWpp() As String
		Get
			Return _strTipoIdentificacionWpp
		End Get
		Set(ByVal value As String)
			_strTipoIdentificacionWpp = value
			If Not IsNothing(DiccionarioCombosOYDPlus) Then
				If DiccionarioCombosOYDPlus.ContainsKey("TIPOID") Then
					For Each li In DiccionarioCombosOYDPlus("TIPOID").Where(Function(i) i.Retorno = _strTipoIdentificacionWpp)
						DescripcionTipoIdentificacionCheque = li.Descripcion
					Next
				End If
			End If
			MyBase.CambioItem("strTipoIdentificacionWpp")
		End Set
	End Property

	Private _NroDocumento As Integer
	Public Property NroDocumento() As Integer
		Get
			Return _NroDocumento
		End Get
		Set(ByVal value As Integer)
			_NroDocumento = value
			MyBase.CambioItem("NroDocumento")
		End Set
	End Property

	Private _strBeneficiarioWpp As String
	Public Property strBeneficiarioWpp() As String
		Get
			Return _strBeneficiarioWpp
		End Get
		Set(ByVal value As String)
			_strBeneficiarioWpp = value
			MyBase.CambioItem("strBeneficiarioWpp")
		End Set
	End Property

	Private _strNroDocumentoWpp As String
	Public Property strNroDocumentoWpp() As String
		Get
			Return _strNroDocumentoWpp
		End Get
		Set(ByVal value As String)
			_strNroDocumentoWpp = value
			MyBase.CambioItem("strNroDocumentoWpp")
		End Set
	End Property

	Private _limpiar As String
	Public Property limpiar() As String
		Get
			Return _limpiar
		End Get
		Set(ByVal value As String)
			_limpiar = value
			MyBase.CambioItem("limpiar")
		End Set
	End Property

	Private _ExpresionRegularNroDocumento As String = "black"
	Public Property ExpresionRegularNroDocumento() As String
		Get
			Return _ExpresionRegularNroDocumento
		End Get
		Set(ByVal value As String)
			_ExpresionRegularNroDocumento = value
			MyBase.CambioItem("ExpresionRegularNroDocumento")
		End Set
	End Property

	Private _ExpresionRegularNombreBeneficiario As String = "black"
	Public Property ExpresionRegularNombreBeneficiario() As String
		Get
			Return _ExpresionRegularNombreBeneficiario
		End Get
		Set(ByVal value As String)
			_ExpresionRegularNombreBeneficiario = value
			MyBase.CambioItem("ExpresionRegularNombreBeneficiario")
		End Set
	End Property

	Private _HabilitarFecha As Boolean
	Public Property HabilitarFecha() As Boolean
		Get
			Return _HabilitarFecha
		End Get
		Set(ByVal value As Boolean)
			_HabilitarFecha = value
			MyBase.CambioItem("HabilitarFecha")
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

	Private _HabilitarBuscadorCliente As Boolean = False
	Public Property HabilitarBuscadorCliente() As Boolean
		Get
			Return _HabilitarBuscadorCliente
		End Get
		Set(ByVal value As Boolean)
			_HabilitarBuscadorCliente = value
			MyBase.CambioItem("HabilitarBuscadorCliente")
		End Set
	End Property

	Private _HablitarDatosTerceros As Boolean
	Public Property HablitarDatosTerceros() As Boolean
		Get
			Return _HablitarDatosTerceros
		End Get
		Set(ByVal value As Boolean)
			_HablitarDatosTerceros = value
			MyBase.CambioItem("HablitarDatosTerceros")
		End Set
	End Property

	Private _HabilitarTipoProducto As Boolean = False
	Public Property HabilitarTipoProducto() As Boolean
		Get
			Return _HabilitarTipoProducto
		End Get
		Set(ByVal value As Boolean)
			_HabilitarTipoProducto = value
			MyBase.CambioItem("HabilitarTipoProducto")
		End Set
	End Property

	Private _TextoComboTipoProducto As String
	Public Property TextoComboTipoProducto() As String
		Get
			Return _TextoComboTipoProducto
		End Get
		Set(ByVal value As String)
			_TextoComboTipoProducto = value
			MyBase.CambioItem("TextoComboTipoProducto")
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

	Private _DiccionarioCombosOYDPlusTodosInicio As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
	Public Property DiccionarioCombosOYDPlusTodosInicio() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
		Get
			Return _DiccionarioCombosOYDPlusTodosInicio
		End Get
		Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
			_DiccionarioCombosOYDPlusTodosInicio = value
			MyBase.CambioItem("DiccionarioCombosOYDPlusTodosInicio")
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

	Private _ListaFormaPagoConsignacion As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
	Public Property ListaFormaPagoConsignacion() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
		Get
			Return _ListaFormaPagoConsignacion
		End Get
		Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
			_ListaFormaPagoConsignacion = value
			MyBase.CambioItem("ListaFormaPagoConsignacion")
		End Set
	End Property

	Private _FechaOrden As DateTime
	Public Property FechaOrden() As DateTime
		Get
			Return _FechaOrden
		End Get
		Set(ByVal value As DateTime)
			_FechaOrden = value
			If Editando And logNuevoRegistro Then
				Validarfecha()
			End If
			MyBase.CambioItem("FechaOrden")
		End Set
	End Property

	Private _FechaAplicacion As Nullable(Of DateTime)
	Public Property FechaAplicacion() As Nullable(Of DateTime)
		Get
			Return _FechaAplicacion
		End Get
		Set(ByVal value As Nullable(Of DateTime))
			_FechaAplicacion = value
			If Editando And logNuevoRegistro Then
				Validarfechaaplicacion()
			End If
			MyBase.CambioItem("FechaAplicacion")
		End Set
	End Property

	Private _Usuario As String
	Public Property Usuario() As String
		Get
			Return _Usuario
		End Get
		Set(ByVal value As String)
			_Usuario = value
			MyBase.CambioItem("Usuario")
		End Set
	End Property

	Private _ComitenteSeleccionado As New OYDUtilidades.BuscadorClientes
	Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
		Get
			Return (_ComitenteSeleccionado)
		End Get
		Set(ByVal value As OYDUtilidades.BuscadorClientes)
			Dim logIgual As Boolean = False
			_ComitenteSeleccionado = value
			'Valida sí se debe de habilitar el archivo

			If Not IsNothing(_ComitenteSeleccionado) Then
				HabilitarSubirArchivo()
				TesoreriaOrdenesPlusRC_Selected.strNombre = _ComitenteSeleccionado.Nombre
				TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion = _ComitenteSeleccionado.TipoIdentificacion
				TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion = _ComitenteSeleccionado.CodTipoIdentificacion
				TesoreriaOrdenesPlusRC_Selected.strNroDocumento = _ComitenteSeleccionado.NroDocumento
				TesoreriaOrdenesPlusRC_Selected.strIDComitente = _ComitenteSeleccionado.CodigoOYD
				TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = Nothing 'DEMC20180615 Y JAP20180615
				If IDTipoClienteEntregado = GSTR_CLIENTE Then
					strNombreEntregado = _ComitenteSeleccionado.Nombre
					strTipoIdentificacionEntregado = _ComitenteSeleccionado.CodTipoIdentificacion
					strNroDocumentoEntregado = _ComitenteSeleccionado.NroDocumento
				End If
				CodigoOYDControles = TesoreriaOrdenesPlusRC_Selected.strIDComitente
				strCodigoOyDCliente = TesoreriaOrdenesPlusRC_Selected.strIDComitente
				ConsultarSaldo = False
				ConsultarSaldo = True
				logHayEncabezado = True

				If Not IsNothing(dcProxy.TempCuentasClientes) Then
					dcProxy.TempCuentasClientes.Clear()
				End If
				If Not IsNothing(dcProxy.TempDireccionesClientes) Then
					dcProxy.TempDireccionesClientes.Clear()
				End If

				If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
					HabilitarEnEdicion = True
					If Not IsNothing(dcProxy.TempDireccionesClientes) Then
						dcProxy.TempDireccionesClientes.Clear()
					End If
					If Not IsNothing(dcProxy.TempCuentasClientes) Then
						dcProxy.TempCuentasClientes.Clear()
					End If
					dcProxy.Load(dcProxy.OyDPlusListarDireccionesClientesQuery(TesoreriaOrdenesPlusRC_Selected.strIDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDireccionesCliente, "")
					dcProxy.Load(dcProxy.OyDPlusListarCuentasClientesQuery(TesoreriaOrdenesPlusRC_Selected.strIDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCliente, "")

					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						If logEsFondosOYD Then
							ConsultarCarterasColectivasFondos(_TesoreriaOrdenesPlusRC_Selected.strIDComitente, True, "CARTERASCLIENTEENCABEZADO")
						Else
							ConsultarCarterasColectivasFondos(_TesoreriaOrdenesPlusRC_Selected.strIDComitente, False, "CARTERASCLIENTEENCABEZADO")
						End If
					Else
						If Editando Then
							ListaCarterasColectivasClienteCompleta = Nothing
							ListaCarterasColectivas = Nothing
							CarteraColectivaFondos = String.Empty
							_TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos = String.Empty
							_TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = String.Empty
							_TesoreriaOrdenesPlusRC_Selected.strDescripcionTipoRetiroFondos = String.Empty
						End If
					End If

					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD Then
						TipoCuentasBancarias = "cuentasbancarias_tesorero"
					ElseIf _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						TipoCuentasBancarias = "cuentasbancariascartera"
					Else
						TipoCuentasBancarias = "cuentasbancariasPLUS"
					End If

				End If
				VerInstrucciones = Visibility.Visible
			Else
				strCodigoOyDCliente = Nothing

				TesoreriaOrdenesPlusRC_Selected.strNombre = String.Empty
				TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion = String.Empty
				TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion = String.Empty
				TesoreriaOrdenesPlusRC_Selected.strNroDocumento = String.Empty
				TesoreriaOrdenesPlusRC_Selected.strIDComitente = String.Empty
				strNombreEntregado = String.Empty
				strTipoIdentificacionEntregado = String.Empty
				strNroDocumentoEntregado = String.Empty
				CodigoOYDControles = String.Empty
				strCodigoOyDCliente = String.Empty
				logHayEncabezado = False
				HabilitarEnEdicion = False
				If Not IsNothing(_IDTipoClienteEntregado) Then
					If (_IDTipoClienteEntregado = GSTR_TERCERO) And logNuevoRegistro Or logEditarRegistro Then
						HabilitarEnEdicion = True
					End If
					'Else
					'HabilitarEnEdicion = False
				End If

				ListaDireccionesClientes = Nothing
				ListaCuentasClientes = Nothing
				ListaCarterasColectivasClienteCompleta = Nothing
				ListaCarterasColectivas = Nothing
				CarteraColectivaFondos = String.Empty
				_TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos = String.Empty
				_TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = String.Empty
				_TesoreriaOrdenesPlusRC_Selected.strDescripcionTipoRetiroFondos = String.Empty
				VerInstrucciones = Visibility.Collapsed
			End If

			MyBase.CambioItem("ComitenteSeleccionado")

		End Set
	End Property

	Private _ListaTesoreriaOrdenesPlusRC As List(Of TesoreriaOrdenesEncabezado)
	Public Property ListaTesoreriaOrdenesPlusRC() As List(Of TesoreriaOrdenesEncabezado)
		Get
			Return _ListaTesoreriaOrdenesPlusRC
		End Get
		Set(ByVal value As List(Of TesoreriaOrdenesEncabezado))
			_ListaTesoreriaOrdenesPlusRC = value
			MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC")
			MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Paged")
			'If Not IsNothing(_ListaTesoreriaOrdenesPlusRC) Then
			'    TesoreriaOrdenesPlusRC_Selected = _ListaTesoreriaOrdenesPlusRC.FirstOrDefault
			'End If
		End Set
	End Property

	Public ReadOnly Property ListaTesoreriaOrdenesPlusRC_Paged() As PagedCollectionView
		Get
			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC) Then
				Dim view = New PagedCollectionView(_ListaTesoreriaOrdenesPlusRC)
				Return view
			Else
				Return Nothing
			End If
		End Get
	End Property

	Private WithEvents _TesoreriaOrdenesPlusRC_Selected As TesoreriaOrdenesEncabezado
	Public Property TesoreriaOrdenesPlusRC_Selected() As TesoreriaOrdenesEncabezado
		Get
			Return _TesoreriaOrdenesPlusRC_Selected
		End Get
		Set(ByVal value As TesoreriaOrdenesEncabezado)
			_TesoreriaOrdenesPlusRC_Selected = value

			If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) Then
				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					If logEsFondosOYD Then
						HabilitarConceptoDetalles = False
					Else
						HabilitarConceptoDetalles = True
					End If
				End If

				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					VerCargarPagosAFondos = Visibility.Visible
					VerCargarPagosA = Visibility.Collapsed
				Else
					VerCargarPagosAFondos = Visibility.Collapsed
					VerCargarPagosA = Visibility.Visible
				End If

				If Not IsNothing(dcProxy.TempDireccionesClientes) Then
					dcProxy.TempDireccionesClientes.Clear()
				End If
				If Not IsNothing(dcProxy.TempCuentasClientes) Then
					dcProxy.TempCuentasClientes.Clear()
				End If
				dcProxy.Load(dcProxy.OyDPlusListarDireccionesClientesQuery(TesoreriaOrdenesPlusRC_Selected.strIDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDireccionesCliente, "")
				dcProxy.Load(dcProxy.OyDPlusListarCuentasClientesQuery(TesoreriaOrdenesPlusRC_Selected.strIDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCliente, "")
			End If

			If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) Then
				VerificarHabilitarTabsOrdenRecaudo(_TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto, _TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos)
			Else
				VerificarHabilitarTabsOrdenRecaudo("C", String.Empty)
			End If

			If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) And logRefrescarDetalles Then
				FechaOrden = _TesoreriaOrdenesPlusRC_Selected.dtmDocumento
				FechaAplicacion = _TesoreriaOrdenesPlusRC_Selected.dtmFechaAplicacion
				dtmFechaMenorPermitidaIngreso = _TesoreriaOrdenesPlusRC_Selected.dtmFechaCierreFondo

				CodigoOYDControles = _TesoreriaOrdenesPlusRC_Selected.strIDComitente
				TipoAccionFondos = _TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos
				CarteraColectivaFondos = _TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos

				If _TesoreriaOrdenesPlusRC_Selected.lngID > 0 Then
					LlamarDetalle()
					ConsultarCarterasColectivasFondos(_TesoreriaOrdenesPlusRC_Selected.strIDComitente, False, "SOLOCARGARLISTACOMPLETA")
					Instrucciones()
				End If

				If logEditarRegistro = False And logNuevoRegistro = False Then
					HabilitarEntregar = False
				End If

				If _TesoreriaOrdenesPlusRC_Selected.strTipoCliente = GSTR_TERCERO Then
					HablitarDatosTerceros = True
				Else
					HablitarDatosTerceros = False
				End If

				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					VerDatosFondos = Visibility.Visible
				Else
					VerDatosFondos = Visibility.Collapsed
				End If

				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD Then
					TipoCuentasBancarias = "cuentasbancarias_tesorero"
				ElseIf _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					TipoCuentasBancarias = "cuentasbancariascartera"
				Else
					TipoCuentasBancarias = "cuentasbancariasPLUS"
				End If
				If logXTesorero = False Then
					strCodigoReceptorBuscadorConcepto = TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor
				Else
					strCodigoReceptorBuscadorConcepto = String.Empty
				End If

			End If
			MyBase.CambioItem("TesoreriaOrdenesPlusRC_Selected")
		End Set
	End Property

	Private _TesoreriaOrdenesPlusRC_DataForm As New TesoreriaOrdenesEncabezado
	Public Property TesoreriaOrdenesPlusRC_DataForm() As TesoreriaOrdenesEncabezado
		Get
			Return _TesoreriaOrdenesPlusRC_DataForm
		End Get
		Set(ByVal value As TesoreriaOrdenesEncabezado)
			_TesoreriaOrdenesPlusRC_DataForm = value
			MyBase.CambioItem("TesoreriaOrdenesPlusRC_DataForm")
		End Set
	End Property

	'---PROPIEDADES PARA CHEQUES -----
	Private _ListaTesoreriaOrdenesPlusRC_Detalle_Cheques As New List(Of TesoreriaOyDPlusChequesRecibo)
	Public Property ListaTesoreriaOrdenesPlusRC_Detalle_Cheques() As List(Of TesoreriaOyDPlusChequesRecibo)
		Get
			Return _ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
		End Get
		Set(ByVal value As List(Of TesoreriaOyDPlusChequesRecibo))
			_ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = value
			MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_Cheques")
			MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_Cheques_Paged")
			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
				TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected = _ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.FirstOrDefault
			End If
		End Set
	End Property

	Public ReadOnly Property ListaTesoreriaOrdenesPlusRC_Detalle_Cheques_Paged() As PagedCollectionView
		Get
			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
				Dim view = New PagedCollectionView(_ListaTesoreriaOrdenesPlusRC_Detalle_Cheques)
				Return view
			Else
				Return Nothing
			End If
		End Get
	End Property

	Private WithEvents _TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected As TesoreriaOyDPlusChequesRecibo
	Public Property TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected() As TesoreriaOyDPlusChequesRecibo
		Get
			Return _TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected
		End Get
		Set(ByVal value As TesoreriaOyDPlusChequesRecibo)
			_TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected = value
			MyBase.CambioItem("TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected")
		End Set
	End Property

	'---PROPIEDADES PARA TRANSFERENCIAS -----
	Private _ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias As List(Of TesoreriaOyDPlusTransferenciasRecibo)
	Public Property ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias() As List(Of TesoreriaOyDPlusTransferenciasRecibo)
		Get
			Return _ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
		End Get
		Set(ByVal value As List(Of TesoreriaOyDPlusTransferenciasRecibo))
			_ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = value
			If Not IsNothing(value) Then
				TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected = _ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.FirstOrDefault
			End If
			MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias")
			MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias_Paged")
		End Set
	End Property

	Public ReadOnly Property ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias_Paged() As PagedCollectionView
		Get
			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
				Dim view = New PagedCollectionView(_ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias)
				Return view
			Else
				Return Nothing
			End If
		End Get
	End Property

	Private WithEvents _TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected As TesoreriaOyDPlusTransferenciasRecibo
	Public Property TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected() As TesoreriaOyDPlusTransferenciasRecibo
		Get
			Return _TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected
		End Get
		Set(ByVal value As TesoreriaOyDPlusTransferenciasRecibo)
			_TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected = value

			MyBase.CambioItem("TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected")
		End Set
	End Property


	'---------------PROPIEDADES PARA CARGA DE PAGOS A: ----------------
	Private _ListatesoreriaordenesplusRC_Detalle_CargarPagosA As List(Of TesoreriaOyDPlusCargosPagosARecibo)
	Public Property ListatesoreriaordenesplusRC_Detalle_CargarPagosA() As List(Of TesoreriaOyDPlusCargosPagosARecibo)
		Get
			Return _ListatesoreriaordenesplusRC_Detalle_CargarPagosA
		End Get
		Set(ByVal value As List(Of TesoreriaOyDPlusCargosPagosARecibo))
			_ListatesoreriaordenesplusRC_Detalle_CargarPagosA = value
			If Not IsNothing(value) Then
				TesoreriaordenesplusRC_Detalle_CargarPagosA_selected = _ListatesoreriaordenesplusRC_Detalle_CargarPagosA.FirstOrDefault
			End If
			MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_CargarPagosA")
			MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_CargarPagosA_paged")
		End Set
	End Property

	Public ReadOnly Property ListatesoreriaordenesplusRC_Detalle_CargarPagosA_paged() As PagedCollectionView
		Get
			If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
				Dim view = New PagedCollectionView(_ListatesoreriaordenesplusRC_Detalle_CargarPagosA)
				Return view
			Else
				Return Nothing
			End If
		End Get
	End Property

	Private WithEvents _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected As TesoreriaOyDPlusCargosPagosARecibo
	Public Property TesoreriaordenesplusRC_Detalle_CargarPagosA_selected() As TesoreriaOyDPlusCargosPagosARecibo
		Get
			Return _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected
		End Get
		Set(ByVal value As TesoreriaOyDPlusCargosPagosARecibo)
			_TesoreriaordenesplusRC_Detalle_CargarPagosA_selected = value
			MyBase.CambioItem("TesoreriaordenesplusRC_Detalle_CargarPagosA_selected")
		End Set
	End Property

	'---------------PROPIEDADES PARA CARGA DE PAGOS A FONDOS: ----------------
	Private _ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos As List(Of TesoreriaOyDPlusCargosPagosAFondosRecibo)
	Public Property ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos() As List(Of TesoreriaOyDPlusCargosPagosAFondosRecibo)
		Get
			Return _ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
		End Get
		Set(ByVal value As List(Of TesoreriaOyDPlusCargosPagosAFondosRecibo))
			_ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = value
			If Not IsNothing(value) Then
				TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected = _ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.FirstOrDefault
			End If
			MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos")
			MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos_paged")
		End Set
	End Property

	Public ReadOnly Property ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos_paged() As PagedCollectionView
		Get
			If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
				Dim view = New PagedCollectionView(_ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos)
				Return view
			Else
				Return Nothing
			End If
		End Get
	End Property

	Private WithEvents _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected As TesoreriaOyDPlusCargosPagosAFondosRecibo
	Public Property TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected() As TesoreriaOyDPlusCargosPagosAFondosRecibo
		Get
			Return _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected
		End Get
		Set(ByVal value As TesoreriaOyDPlusCargosPagosAFondosRecibo)
			_TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected = value
			MyBase.CambioItem("TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected")
		End Set
	End Property

	'---------------PROPIEDADES PARA CONSIGNACIONES: ----------------
	Private _ListatesoreriaordenesplusRC_Detalle_Consignaciones As List(Of TesoreriaOyDPlusConsignacionesRecibo)
	Public Property ListatesoreriaordenesplusRC_Detalle_Consignaciones() As List(Of TesoreriaOyDPlusConsignacionesRecibo)
		Get
			Return _ListatesoreriaordenesplusRC_Detalle_Consignaciones
		End Get
		Set(ByVal value As List(Of TesoreriaOyDPlusConsignacionesRecibo))
			_ListatesoreriaordenesplusRC_Detalle_Consignaciones = value
			If Not IsNothing(value) Then
				TesoreriaordenesplusRC_Detalle_Consignaciones_selected = _ListatesoreriaordenesplusRC_Detalle_Consignaciones.FirstOrDefault
			End If
			MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_Consignaciones")
			MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_Consignaciones_paged")
		End Set
	End Property

	Public ReadOnly Property ListatesoreriaordenesplusRC_Detalle_Consignaciones_paged() As PagedCollectionView
		Get
			If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
				Dim view = New PagedCollectionView(_ListatesoreriaordenesplusRC_Detalle_Consignaciones)
				Return view
			Else
				Return Nothing
			End If
		End Get
	End Property

	Private WithEvents _TesoreriaordenesplusRC_Detalle_Consignaciones_selected As TesoreriaOyDPlusConsignacionesRecibo
	Public Property TesoreriaordenesplusRC_Detalle_Consignaciones_selected() As TesoreriaOyDPlusConsignacionesRecibo
		Get
			Return _TesoreriaordenesplusRC_Detalle_Consignaciones_selected
		End Get
		Set(ByVal value As TesoreriaOyDPlusConsignacionesRecibo)
			_TesoreriaordenesplusRC_Detalle_Consignaciones_selected = value

			MyBase.CambioItem("TesoreriaordenesplusRC_Detalle_Consignaciones_selected")
		End Set
	End Property

	Private _bolIsBusyReceptor As Boolean
	Public Property IsBusyReceptor() As Boolean
		Get
			Return _bolIsBusyReceptor
		End Get
		Set(ByVal value As Boolean)
			_bolIsBusyReceptor = value
			CambioItem("IsBusyReceptor")
		End Set
	End Property

	Private _HabilitarTipoCruce As Boolean
	Public Property HabilitarTipoCruce() As Boolean
		Get
			Return _HabilitarTipoCruce
		End Get
		Set(ByVal value As Boolean)
			_HabilitarTipoCruce = value
			MyBase.CambioItem("HabilitarTipoCruce")
		End Set
	End Property

	Private _strTipoCheque As String
	Public Property strTipoCheque() As String
		Get
			Return _strTipoCheque
		End Get
		Set(ByVal value As String)
			_strTipoCheque = value
			MyBase.CambioItem("strTipoCheque")
		End Set
	End Property

	Private _TipoChequeDescripcion As String
	Public Property TipoChequeDescripcion() As String
		Get
			Return _TipoChequeDescripcion
		End Get
		Set(ByVal value As String)
			_TipoChequeDescripcion = value
			MyBase.CambioItem("TipoChequeDescripcion")
		End Set
	End Property

	Private _DescripcionTipoCruce As String
	Public Property DescripcionTipoCruce() As String
		Get
			Return _DescripcionTipoCruce
		End Get
		Set(ByVal value As String)
			_DescripcionTipoCruce = value
			MyBase.CambioItem("DescripcionTipoCruce")
		End Set
	End Property
	'Private _IDCruce As OYDPLUSUtilidades.CombosReceptor
	'Public Property IDCruce() As OYDPLUSUtilidades.CombosReceptor
	'    Get
	'        Return _IDCruce
	'    End Get
	'    Set(ByVal value As OYDPLUSUtilidades.CombosReceptor)
	'        _IDCruce = value

	'        MyBase.CambioItem("IDCruce")
	'    End Set
	'End Propert

	Private _VerDatosCliente As Visibility = Visibility.Collapsed
	Public Property VerDatosCliente() As Visibility
		Get
			Return _VerDatosCliente
		End Get
		Set(ByVal value As Visibility)
			_VerDatosCliente = value
			MyBase.CambioItem("VerDatosCliente")
		End Set
	End Property

	Private _ValorTotalGenerarCheque As Decimal
	Public Property ValorTotalGenerarCheque() As Decimal
		Get
			Return _ValorTotalGenerarCheque
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalGenerarCheque = value
			MyBase.CambioItem("ValorTotalGenerarCheque")
		End Set
	End Property

	Private _ValorTotalGenerarOrden As Decimal
	Public Property ValorTotalGenerarOrden() As Decimal
		Get
			Return _ValorTotalGenerarOrden
		End Get
		Set(ByVal value As Decimal)
			_ValorTotalGenerarOrden = value
			MyBase.CambioItem("ValorTotalGenerarOrden")
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
					ValidarIngresoOrdenTesoreria()
				End If
			End If
		End Set
	End Property
	Private _strNombreCodigoOyD As String
	Public Property strNombreCodigoOyD() As String
		Get
			Return _strNombreCodigoOyD
		End Get
		Set(ByVal value As String)
			_strNombreCodigoOyD = value
			MyBase.CambioItem("strNombreCodigoOyD")
		End Set
	End Property
	Private _strNombreCodigoOyDFondos As String
	Public Property strNombreCodigoOyDFondos() As String
		Get
			Return _strNombreCodigoOyDFondos
		End Get
		Set(ByVal value As String)
			_strNombreCodigoOyD = value
			MyBase.CambioItem("strNombreCodigoOyDFondos")
		End Set
	End Property



	Private _strCodigoOyDCargarPagosA As String
	Public Property strCodigoOyDCargarPagosA() As String
		Get
			Return _strCodigoOyDCargarPagosA
		End Get
		Set(ByVal value As String)
			_strCodigoOyDCargarPagosA = value
			MyBase.CambioItem("strCodigoOyDCargarPagosA")
		End Set
	End Property

	Private _strCodigoOyDCargarPagosAFondos As String
	Public Property strCodigoOyDCargarPagosAFondos() As String
		Get
			Return _strCodigoOyDCargarPagosAFondos
		End Get
		Set(ByVal value As String)
			_strCodigoOyDCargarPagosAFondos = value
			If logCambiarPropiedadesPOPPUP Then
				If _TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
					ConsultarCarterasColectivasFondos(_strCodigoOyDCargarPagosAFondos, False, "ENCARGOS_CARTERACOLECTIVA")
				End If
			End If
			MyBase.CambioItem("strCodigoOyDCargarPagosAFondos")
		End Set
	End Property

	Public _ValorGenerar As Decimal
	Public Property ValorGenerar() As Decimal
		Get
			Return _ValorGenerar
		End Get
		Set(ByVal value As Decimal)
			_ValorGenerar = value
			If Not IsNothing(_ValorGenerar) Then
				If logCambiarPropiedadesPOPPUP Then

				End If
			End If
			MyBase.CambioItem("ValorGenerar")
		End Set
	End Property

	Private _HabilitarEncabezadoConsignacion As Boolean
	Public Property HabilitarEncabezadoConsignacion() As Boolean
		Get
			Return _HabilitarEncabezadoConsignacion
		End Get
		Set(ByVal value As Boolean)
			_HabilitarEncabezadoConsignacion = value
			MyBase.CambioItem("HabilitarEncabezadoConsignacion")
		End Set
	End Property

	Private _HabilitarEncabezadoConsignacionReferencia As Boolean
	Public Property HabilitarEncabezadoConsignacionReferencia() As Boolean
		Get
			Return _HabilitarEncabezadoConsignacionReferencia
		End Get
		Set(ByVal value As Boolean)
			_HabilitarEncabezadoConsignacionReferencia = value
			MyBase.CambioItem("HabilitarEncabezadoConsignacionReferencia")
		End Set
	End Property
	Private _strDescripcionCuentaConsignacion As String
	Public Property strDescripcionCuentaConsignacion() As String
		Get
			Return _strDescripcionCuentaConsignacion
		End Get
		Set(ByVal value As String)
			_strDescripcionCuentaConsignacion = value
			MyBase.CambioItem("strDescripcionCuentaConsignacion")
		End Set
	End Property

	Private _strCuentaConsignacion As String
	Public Property strCuentaConsignacion() As String
		Get
			Return _strCuentaConsignacion
		End Get
		Set(ByVal value As String)
			_strCuentaConsignacion = value
			MyBase.CambioItem("strCuentaConsignacion")
		End Set
	End Property

	Private _strTipoCuentaConsignacion As String
	Public Property strTipoCuentaConsignacion() As String
		Get
			Return _strTipoCuentaConsignacion
		End Get
		Set(ByVal value As String)
			_strTipoCuentaConsignacion = value
			MyBase.CambioItem("strTipoCuentaConsignacion")
		End Set
	End Property


	Public _ValorGenerarConsignacion As Decimal
	Public Property ValorGenerarConsignacion() As Decimal
		Get
			Return _ValorGenerarConsignacion
		End Get
		Set(ByVal value As Decimal)
			_ValorGenerarConsignacion = value
			If Not IsNothing(_ValorGenerarConsignacion) Then
				If logCambiarPropiedadesPOPPUP Then

				End If
			End If
			MyBase.CambioItem("ValorGenerarConsignacion")
		End Set
	End Property

	Private _ValorGenerarEfectivoConsignacion As Decimal
	Public Property ValorGenerarEfectivoConsignacion() As Decimal
		Get
			Return _ValorGenerarEfectivoConsignacion
		End Get
		Set(ByVal value As Decimal)
			_ValorGenerarEfectivoConsignacion = value
			If Not IsNothing(_ValorGenerarEfectivoConsignacion) Then
				If logCambiarPropiedadesPOPPUP Then

				End If
			End If
			MyBase.CambioItem("ValorGenerarEfectivoConsignacion")
		End Set
	End Property


	Private _IsBusyDetalles As Boolean = False
	Public Property IsBusyDetalles() As Boolean
		Get
			Return _IsBusyDetalles
		End Get
		Set(ByVal value As Boolean)
			_IsBusyDetalles = value
			MyBase.CambioItem("IsBusyDetalles")
		End Set
	End Property

	Private _DetalleBloqueo As String
	Public Property DetalleBloqueo() As String
		Get
			Return _DetalleBloqueo
		End Get
		Set(ByVal value As String)
			_DetalleBloqueo = value
			MyBase.CambioItem("DetalleBloqueo")
		End Set
	End Property

	Private _strNaturaleza As String
	Public Property strNaturaleza() As String
		Get
			Return _strNaturaleza
		End Get
		Set(ByVal value As String)
			_strNaturaleza = value
			MyBase.CambioItem("strNaturaleza")
		End Set
	End Property

	Private _ValorBloqueado As Decimal
	Public Property ValorBloqueado() As Decimal
		Get
			Return _ValorBloqueado
		End Get
		Set(ByVal value As Decimal)
			_ValorBloqueado = value
			MyBase.CambioItem("ValorBloqueado")
		End Set
	End Property

	Private _strTipoBloqueo As String
	Public Property strTipoBloqueo() As String
		Get
			Return _strTipoBloqueo
		End Get
		Set(ByVal value As String)
			_strTipoBloqueo = value
			MyBase.CambioItem("strTipoBloqueo")
		End Set
	End Property


	Private _ValorDisponibleCargarPago As Decimal
	Public Property ValorDisponibleCargarPago() As Decimal
		Get
			Return _ValorDisponibleCargarPago
		End Get
		Set(ByVal value As Decimal)
			_ValorDisponibleCargarPago = value
			MyBase.CambioItem("ValorDisponibleCargarPago")
		End Set
	End Property

	Private _ValorDisponibleCargarPagoFondos As Decimal
	Public Property ValorDisponibleCargarPagoFondos() As Decimal
		Get
			Return _ValorDisponibleCargarPagoFondos
		End Get
		Set(ByVal value As Decimal)
			_ValorDisponibleCargarPagoFondos = value
			MyBase.CambioItem("ValorDisponibleCargarPagoFondos")
		End Set
	End Property


	Private _strOpcionConsutaSaldo As String
	Public Property strOpcionConsutaSaldo() As String
		Get
			Return _strOpcionConsutaSaldo
		End Get
		Set(ByVal value As String)
			_strOpcionConsutaSaldo = value
		End Set
	End Property

	Private _listaSaldoCliente As List(Of OYDPLUSUtilidades.tblSaldosCliente)
	Public Property ListaSaldoCliente() As List(Of OYDPLUSUtilidades.tblSaldosCliente)
		Get
			Return _listaSaldoCliente
		End Get
		Set(ByVal value As List(Of OYDPLUSUtilidades.tblSaldosCliente))
			_listaSaldoCliente = value
		End Set
	End Property

	Private _dblSaldoConsultado As Double
	Public Property SaldoConsultado() As Double
		Get
			Return _dblSaldoConsultado
		End Get
		Set(ByVal value As Double)
			_dblSaldoConsultado = value
		End Set
	End Property


#Region "Ancho y Altos"
	Private _ActualAltoPantalla As Double = 500
	Public Property ActualAltoPantalla() As Double
		Get
			Return _ActualAltoPantalla
		End Get
		Set(ByVal value As Double)
			_ActualAltoPantalla = value
			MyBase.CambioItem("ActualAltoPantalla")
		End Set
	End Property

	Private _AnchoMensaje As String
	Public Property AnchoMensaje() As String
		Get
			Return _AnchoMensaje
		End Get
		Set(ByVal value As String)
			_AnchoMensaje = value
			MyBase.CambioItem("AnchoMensaje")
		End Set
	End Property

	Private _ActualAnchoPantalla As Double
	Public Property ActualAnchoPantalla() As Double
		Get
			Return _ActualAnchoPantalla
		End Get
		Set(ByVal value As Double)
			_ActualAnchoPantalla = value
			If Not IsNothing(value) Then
				AnchoMensaje = value.ToString
			End If
			MyBase.CambioItem("ActualAnchoPantalla")
		End Set
	End Property

	Private _MostrarItemCheque As Visibility = Visibility.Collapsed
	Public Property MostrarItemCheque() As Visibility
		Get
			Return _MostrarItemCheque
		End Get
		Set(ByVal value As Visibility)
			_MostrarItemCheque = value
			MyBase.CambioItem("MostrarItemCheque")
		End Set
	End Property
#End Region

	'Propiedad para almacenar el valor total de las liquidaciones y concatenarlo en los detalles de tesoreria.
	Private _liquidacionesSelecciondas As String
	Public Property liquidacionesSelecciondas() As String
		Get
			Return _liquidacionesSelecciondas
		End Get
		Set(ByVal value As String)
			_liquidacionesSelecciondas = value
		End Set
	End Property


	'---PROPIEDADES PARA CHEQUES CONSIGNACIONES-----
	Private _ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones As New List(Of ChequesConsignacionesOyDPlus)
	Public Property ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones() As List(Of ChequesConsignacionesOyDPlus)
		Get
			Return _ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones
		End Get
		Set(ByVal value As List(Of ChequesConsignacionesOyDPlus))
			_ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones = value
			If Not IsNothing(value) Then
				TesoreriaordenesplusRC_Detalle_ChequesConsignaciones_selected = _ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones.FirstOrDefault
			End If
			MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones")
			MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_ChequesConsignaciones_paged")
		End Set
	End Property

	Public ReadOnly Property ListatesoreriaordenesplusRC_Detalle_ChequesConsignaciones_paged() As PagedCollectionView
		Get
			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
				Dim view = New PagedCollectionView(_ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones)
				Return view
			Else
				Return Nothing
			End If
		End Get
	End Property

	Private WithEvents _TesoreriaordenesplusRC_Detalle_ChequesConsignaciones_selected As ChequesConsignacionesOyDPlus
	Public Property TesoreriaordenesplusRC_Detalle_ChequesConsignaciones_selected() As ChequesConsignacionesOyDPlus
		Get
			Return _TesoreriaordenesplusRC_Detalle_ChequesConsignaciones_selected
		End Get
		Set(ByVal value As ChequesConsignacionesOyDPlus)
			_TesoreriaordenesplusRC_Detalle_ChequesConsignaciones_selected = value

			MyBase.CambioItem("TesoreriaordenesplusRC_Detalle_ChequesConsignaciones_selected")
		End Set
	End Property

	Private _ViewImportarArchivo As ImportarArchivoRecibos
	Public Property ViewImportarArchivo() As ImportarArchivoRecibos
		Get
			Return _ViewImportarArchivo
		End Get
		Set(ByVal value As ImportarArchivoRecibos)
			_ViewImportarArchivo = value
		End Set
	End Property

	Private _ListaBancos As List(Of OyDPLUSTesoreria.Banco)
	Public Property ListaBancos() As List(Of OyDPLUSTesoreria.Banco)
		Get
			Return _ListaBancos
		End Get
		Set(ByVal value As List(Of OyDPLUSTesoreria.Banco))
			_ListaBancos = value
		End Set
	End Property

	Private _ListaBancosNacionales As List(Of BancosNacionale)
	Public Property ListaBancosNacionales() As List(Of BancosNacionale)
		Get
			Return _ListaBancosNacionales
		End Get
		Set(ByVal value As List(Of BancosNacionale))
			_ListaBancosNacionales = value
		End Set
	End Property

	Private _ListaArchivosCheque As List(Of InformacionArchivos)
	Public Property ListaArchivosCheque() As List(Of InformacionArchivos)
		Get
			Return _ListaArchivosCheque
		End Get
		Set(ByVal value As List(Of InformacionArchivos))
			_ListaArchivosCheque = value
		End Set
	End Property

	Private _ListaArchivoTransferencia As List(Of InformacionArchivos)
	Public Property ListaArchivoTransferencia() As List(Of InformacionArchivos)
		Get
			Return _ListaArchivoTransferencia
		End Get
		Set(ByVal value As List(Of InformacionArchivos))
			_ListaArchivoTransferencia = value
		End Set
	End Property

	Private _ListaArchivoConsignacion As List(Of InformacionArchivos)
	Public Property ListaArchivoConsignacion() As List(Of InformacionArchivos)
		Get
			Return _ListaArchivoConsignacion
		End Get
		Set(ByVal value As List(Of InformacionArchivos))
			_ListaArchivoConsignacion = value
		End Set
	End Property

	'Propiedades para el manejo de carteras colectivas fondos.
	'***************************************************************************
	Private _ListaCarterasColectivas As List(Of CarterasColectivasClientes)
	Public Property ListaCarterasColectivas() As List(Of CarterasColectivasClientes)
		Get
			Return _ListaCarterasColectivas
		End Get
		Set(ByVal value As List(Of CarterasColectivasClientes))
			_ListaCarterasColectivas = value
			If Not IsNothing(_ListaCarterasColectivas) Then
				If _ListaCarterasColectivas.Where(Function(i) i.CarteraColectiva = _TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos).Count > 0 Then
					CarteraColectivaFondos = _ListaCarterasColectivas.Where(Function(i) i.CarteraColectiva = _TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos).First.CarteraColectiva
				End If
			End If
			MyBase.CambioItem("ListaCarterasColectivas")
		End Set
	End Property

	Private _ListaTiposAccionFondos As List(Of OYDUtilidades.ItemCombo)
	Public Property ListaTiposAccionFondos() As List(Of OYDUtilidades.ItemCombo)
		Get
			Return _ListaTiposAccionFondos
		End Get
		Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
			_ListaTiposAccionFondos = value
			MyBase.CambioItem("ListaTiposAccionFondos")
		End Set
	End Property

	Private _TipoAccionFondos As String
	Public Property TipoAccionFondos() As String
		Get
			Return _TipoAccionFondos
		End Get
		Set(ByVal value As String)
			_TipoAccionFondos = value

			If Not IsNothing(DiccionarioCombosOYDPlus) Then
				If DiccionarioCombosOYDPlus.ContainsKey("TIPOACCIONFONDOS") Then
					If DiccionarioCombosOYDPlus("TIPOACCIONFONDOS").Where(Function(i) i.Retorno = TipoAccionFondos).Count > 0 Then
						DescripcionTipoAccion = DiccionarioCombosOYDPlus("TIPOACCIONFONDOS").Where(Function(i) i.Retorno = TipoAccionFondos).First.Descripcion
					End If
				End If
			End If

			If Not IsNothing(_TipoAccionFondos) And logNuevoRegistro Then
				If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) Then
					_TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = _TipoAccionFondos
				End If
			End If

			If Not IsNothing(_TipoAccionFondos) Then
				If logCambiarPropiedadesPOPPUP Then
					If logEsFondosOYD = False Then
						Dim objListaNuevaCarteras As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)

						If _TipoAccionFondos = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
							HabilitarNroEncargo = False

							If Not IsNothing(_ListaTodasLasCarterasColectivas) Then
								For Each li In _ListaTodasLasCarterasColectivas
									If objListaNuevaCarteras.Where(Function(i) i.CarteraColectiva = li.CarteraColectiva).Count = 0 Then
										objListaNuevaCarteras.Add(New OyDPLUSTesoreria.CarterasColectivasClientes With {.ID = li.ID,
																													.CarteraColectiva = li.CarteraColectiva,
																													.NombreCarteraColectiva = li.NombreCarteraColectiva,
																													.CodigoOYD = String.Empty,
																														.NroEncargo = String.Empty,
																													.Saldo = 0,
																													.SaldoDisponible = 0,
																														.dtmFechaCierre = li.dtmFechaCierre})
									End If
								Next
							End If
						Else
							HabilitarNroEncargo = True

							If Not IsNothing(_ListaCarterasColectivasClienteCompleta) Then
								For Each li In _ListaCarterasColectivasClienteCompleta
									If objListaNuevaCarteras.Where(Function(i) i.CarteraColectiva = li.CarteraColectiva).Count = 0 Then
										objListaNuevaCarteras.Add(New OyDPLUSTesoreria.CarterasColectivasClientes With {.ID = li.ID,
																													.CarteraColectiva = li.CarteraColectiva,
																													.NombreCarteraColectiva = li.NombreCarteraColectiva,
																													.CodigoOYD = li.CodigoOYD,
																													.NroEncargo = li.NroEncargo,
																													.Saldo = li.Saldo,
																													.SaldoDisponible = li.SaldoDisponible,
																														.dtmFechaCierre = li.dtmFechaCierre})
									End If
								Next
							End If
						End If

						ListaCarterasColectivas = objListaNuevaCarteras
						'JFSB 20171026 Se agrega validacion si la lista tiene datos
						If ListaCarterasColectivas.Count > 0 Then
							CarteraColectivaFondos = ListaCarterasColectivas.FirstOrDefault.CarteraColectiva
						End If
					End If

					If _TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
						HabilitarNroEncargo = True
						If Editando Then
							FechaAplicacion = DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, FechaOrden)
						End If
					ElseIf _TipoAccionFondos = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
						HabilitarNroEncargo = False
						If Editando Then
							FechaAplicacion = DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, FechaOrden)
						End If
					End If
				End If
			End If
			MyBase.CambioItem("TipoAccionFondos")
		End Set
	End Property

	Private _DescripcionTipoAccion As String
	Public Property DescripcionTipoAccion() As String
		Get
			Return _DescripcionTipoAccion
		End Get
		Set(ByVal value As String)
			_DescripcionTipoAccion = value
			MyBase.CambioItem("DescripcionTipoAccion")
		End Set
	End Property

	Private _TipoCuentasBancarias As String = String.Empty
	Public Property TipoCuentasBancarias() As String
		Get
			Return _TipoCuentasBancarias
		End Get
		Set(ByVal value As String)
			_TipoCuentasBancarias = value
			MyBase.CambioItem("TipoCuentasBancarias")
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

			If Not IsNothing(_CarteraColectivaFondos) And logNuevoRegistro Then
				If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) Then
					If logEsFondosOYD Then
						If Not IsNothing(_ListaCarterasColectivas) Then
							If _ListaCarterasColectivas.Where(Function(i) i.CarteraColectiva = _CarteraColectivaFondos).Count > 0 Then
								dtmFechaMenorPermitidaIngreso = _ListaCarterasColectivas.Where(Function(i) i.CarteraColectiva = _CarteraColectivaFondos).First.dtmFechaCierre
							End If
						End If
					End If
					_TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos = _CarteraColectivaFondos
				End If
			End If

			If Not String.IsNullOrEmpty(_CarteraColectivaFondos) Then
				If logEsFondosOYD = False Then
					If _TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
						If Not IsNothing(_ListaCarterasColectivasClienteCompleta) Then
							Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
							For Each li In _ListaCarterasColectivasClienteCompleta
								If li.CarteraColectiva = _CarteraColectivaFondos And Not String.IsNullOrEmpty(li.NroEncargo) Then
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

							ListaEncargosCarteraColectiva = objNuevaListaEncargos
						Else
							ListaEncargosCarteraColectiva = Nothing
						End If
					Else
						ListaEncargosCarteraColectiva = Nothing
					End If
				End If

				DescripcionCarteraColectivaFondos = String.Empty
				If Not IsNothing(DiccionarioCombosOYDPlus) Then
					If DiccionarioCombosOYDPlus.ContainsKey("CARTERACOLECTIVAS") Then
						If DiccionarioCombosOYDPlus("CARTERACOLECTIVAS").Where(Function(i) i.Retorno = _CarteraColectivaFondos).Count > 0 Then
							DescripcionCarteraColectivaFondos = DiccionarioCombosOYDPlus("CARTERACOLECTIVAS").Where(Function(i) i.Retorno = _CarteraColectivaFondos).First.Descripcion
						End If
					End If
				End If
			Else
				'ListaEncargosCarteraColectiva = Nothing
				'TipoAccionFondos = Nothing 'DEMC20180615 Y JAP20180615
			End If
		End Set
	End Property

	Private _DescripcionCarteraColectivaFondos As String
	Public Property DescripcionCarteraColectivaFondos() As String
		Get
			Return _DescripcionCarteraColectivaFondos
		End Get
		Set(ByVal value As String)
			_DescripcionCarteraColectivaFondos = value
			MyBase.CambioItem("DescripcionCarteraColectivaFondos")
		End Set
	End Property

	Private _HabilitarCategoriaFondos As Boolean
	Public Property HabilitarCategoriaFondos() As Boolean
		Get
			Return _HabilitarCategoriaFondos
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCategoriaFondos = value
			MyBase.CambioItem("HabilitarCategoriaFondos")
		End Set
	End Property

	Private _NroEncargoFondos As String
	Public Property NroEncargoFondos() As String
		Get
			Return _NroEncargoFondos
		End Get
		Set(ByVal value As String)
			_NroEncargoFondos = value
			If logCambiarPropiedadesPOPPUP Then
				If Editando Then
					If Not IsNothing(_NroEncargoFondos) Then 'C-20190384  JAPC20200219:se ajusta invocacion validacion restricciones cartera cuando nro encargo no este vacio
                        If _NroEncargoFondos <> String.Empty Then
                            VerificarRestriccionesTipoCartera("DETALLEOORDENGIRO")
                        End If
                    End If
                End If
			End If
			MyBase.CambioItem("NroEncargoFondos")
		End Set
	End Property

	Private _DescripcionEncargoFondos As String
	Public Property DescripcionEncargoFondos() As String
		Get
			Return _DescripcionEncargoFondos
		End Get
		Set(ByVal value As String)
			_DescripcionEncargoFondos = value
			MyBase.CambioItem("DescripcionEncargoFondos")
		End Set
	End Property

	Private _HabilitarNroEncargo As Boolean
	Public Property HabilitarNroEncargo() As Boolean
		Get
			Return _HabilitarNroEncargo
		End Get
		Set(ByVal value As Boolean)
			_HabilitarNroEncargo = value
			If logCambiarPropiedadesPOPPUP Then
				If _HabilitarNroEncargo = False Then
					NroEncargoFondos = String.Empty
				End If
			End If
			MyBase.CambioItem("HabilitarNroEncargo")
		End Set
	End Property

	Private _ListaTodasLasCarterasColectivas As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
	Public Property ListaTodasLasCarterasColectivas() As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
		Get
			Return _ListaTodasLasCarterasColectivas
		End Get
		Set(ByVal value As List(Of OyDPLUSTesoreria.CarterasColectivasClientes))
			_ListaTodasLasCarterasColectivas = value
			MyBase.CambioItem("ListaTodasLasCarterasColectivas")
		End Set
	End Property

	Private _ListaCarterasColectivasClienteCompleta As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
	Public Property ListaCarterasColectivasClienteCompleta() As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
		Get
			Return _ListaCarterasColectivasClienteCompleta
		End Get
		Set(ByVal value As List(Of OyDPLUSTesoreria.CarterasColectivasClientes))
			_ListaCarterasColectivasClienteCompleta = value
			MyBase.CambioItem("ListaCarterasColectivasClienteCompleta")
		End Set
	End Property

	Private _ListaEncargosCarteraColectiva As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
	Public Property ListaEncargosCarteraColectiva() As List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
		Get
			Return _ListaEncargosCarteraColectiva
		End Get
		Set(ByVal value As List(Of OyDPLUSTesoreria.CarterasColectivasClientes))
			_ListaEncargosCarteraColectiva = value
			MyBase.CambioItem("ListaEncargosCarteraColectiva")
		End Set
	End Property

	Private _MostrarCarteraColectivasFondos As Visibility = Visibility.Collapsed
	Public Property MostrarCarteraColectivasFondos() As Visibility
		Get
			Return _MostrarCarteraColectivasFondos
		End Get
		Set(ByVal value As Visibility)
			_MostrarCarteraColectivasFondos = value
			MyBase.CambioItem("MostrarCarteraColectivasFondos")
		End Set
	End Property

	'***************************************************************************
	Private _HabilitarCamposFondosOYD As Boolean = False
	Public Property HabilitarCamposFondosOYD() As Boolean
		Get
			Return _HabilitarCamposFondosOYD
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCamposFondosOYD = value
			MyBase.CambioItem("HabilitarCamposFondosOYD")
		End Set
	End Property

	Private _AgrupamientoBanco As String
	Public Property AgrupamientoBanco() As String
		Get
			Return _AgrupamientoBanco
		End Get
		Set(ByVal value As String)
			_AgrupamientoBanco = value
			MyBase.CambioItem("AgrupamientoBanco")
		End Set
	End Property

	Private _TextoArchivoSeleccionadoDetalle As String = "Seleccione el archivo ..."
	Public Property TextoArchivoSeleccionadoDetalle() As String
		Get
			Return _TextoArchivoSeleccionadoDetalle
		End Get
		Set(ByVal value As String)
			_TextoArchivoSeleccionadoDetalle = value
			MyBase.CambioItem("TextoArchivoSeleccionadoDetalle")
		End Set
	End Property

	Private _LlevarSaldoDisponibleCargarPagosA As Boolean
	Public Property LlevarSaldoDisponibleCargarPagosA() As Boolean
		Get
			Return _LlevarSaldoDisponibleCargarPagosA
		End Get
		Set(ByVal value As Boolean)
			_LlevarSaldoDisponibleCargarPagosA = value
			If _LlevarSaldoDisponibleCargarPagosA Then
				ValorCargarPagoA = ValorDisponibleCargarPago
			End If
			MyBase.CambioItem("LlevarSaldoDisponibleCargarPagosA")
		End Set
	End Property

	Private _LlevarSaldoDisponibleCargarPagosAFondos As Boolean
	Public Property LlevarSaldoDisponibleCargarPagosAFondos() As Boolean
		Get
			Return _LlevarSaldoDisponibleCargarPagosAFondos
		End Get
		Set(ByVal value As Boolean)
			_LlevarSaldoDisponibleCargarPagosAFondos = value
			If _LlevarSaldoDisponibleCargarPagosAFondos Then
				ValorCargarPagoAFondos = ValorDisponibleCargarPagoFondos
			End If
			MyBase.CambioItem("LlevarSaldoDisponibleCargarPagosAFondos")
		End Set
	End Property

	Private _ListaClientesEncabezado As List(Of OYDUtilidades.BuscadorClientes)
	Public Property ListaClientesEncabezado() As List(Of OYDUtilidades.BuscadorClientes)
		Get
			Return _ListaClientesEncabezado
		End Get
		Set(ByVal value As List(Of OYDUtilidades.BuscadorClientes))
			_ListaClientesEncabezado = value
		End Set
	End Property

	Private _MostrarControlMenuGuardar As Boolean
	Public Property MostrarControlMenuGuardar() As Boolean
		Get
			Return _MostrarControlMenuGuardar
		End Get
		Set(ByVal value As Boolean)
			_MostrarControlMenuGuardar = value
			MyBase.CambioItem("MostrarControlMenuGuardar")
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

	Private _FondoTextoBuscadoresHabilitado As SolidColorBrush
	Public Property FondoTextoBuscadoresHabilitado() As SolidColorBrush
		Get
			Return _FondoTextoBuscadoresHabilitado
		End Get
		Set(ByVal value As SolidColorBrush)
			_FondoTextoBuscadoresHabilitado = value
			MyBase.CambioItem("FondoTextoBuscadoresHabilitado")
		End Set
	End Property

	Private _MostrarFechaOrden As Boolean = False
	Public Property MostrarFechaOrden() As Boolean
		Get
			Return _MostrarFechaOrden
		End Get
		Set(ByVal value As Boolean)
			_MostrarFechaOrden = value
			MyBase.CambioItem("MostrarFechaOrden")
		End Set
	End Property

	Private _HabilitarConceptoDetalles As Boolean
	Public Property HabilitarConceptoDetalles() As Boolean
		Get
			Return _HabilitarConceptoDetalles
		End Get
		Set(ByVal value As Boolean)
			_HabilitarConceptoDetalles = value
			MyBase.CambioItem("HabilitarConceptoDetalles")
		End Set
	End Property

	Private _TituloPestanaCargarPagosA As String = "Cargar Pago A"
	Public Property TituloPestanaCargarPagosA() As String
		Get
			Return _TituloPestanaCargarPagosA
		End Get
		Set(ByVal value As String)
			_TituloPestanaCargarPagosA = value
			MyBase.CambioItem("TituloPestanaCargarPagosA")
		End Set
	End Property

	Private _TituloPestanaCargarPagosAFondos As String = "Cargar Pago A Fondos"
	Public Property TituloPestanaCargarPagosAFondos() As String
		Get
			Return _TituloPestanaCargarPagosAFondos
		End Get
		Set(ByVal value As String)
			_TituloPestanaCargarPagosAFondos = value
			MyBase.CambioItem("TituloPestanaCargarPagosAFondos")
		End Set
	End Property

	Private _MostrarGuardarContinuarPagosFondos As Visibility = Visibility.Collapsed
	Public Property MostrarGuardarContinuarPagosFondos() As Visibility
		Get
			Return _MostrarGuardarContinuarPagosFondos
		End Get
		Set(ByVal value As Visibility)
			_MostrarGuardarContinuarPagosFondos = value
			MyBase.CambioItem("MostrarGuardarContinuarPagosFondos")
		End Set
	End Property


#End Region

#Region "Instrucciones"

	Private _IDTipoCliente As String
	Public Property IDTipoCliente() As String
		Get
			Return _IDTipoCliente
		End Get
		Set(ByVal value As String)
			_IDTipoCliente = value
			MyBase.CambioItem("IDTipoCliente")
		End Set
	End Property

	Private _IDTipoClienteCargarPagosA As String
	Public Property IDTipoClienteCargarPagosA() As String
		Get
			Return _IDTipoClienteCargarPagosA
		End Get
		Set(ByVal value As String)
			_IDTipoClienteCargarPagosA = value
			If value = GSTR_CLIENTE Then
				TipoIdClienteDescripcion = "Cliente"
				If String.IsNullOrEmpty(TesoreriaOrdenesPlusRC_Selected.strIDComitente) Then
					mostrarMensaje("No existe Código OyD en el Encabezado, Elija un código OyD", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					_IDTipoClienteCargarPagosA = GSTR_TERCERO
				End If

				If logCambiarPropiedadesPOPPUP Then
					If Not IsNothing(_ListaClientesEncabezado) Then
						If _ListaClientesEncabezado.Count = 1 Then
							strCodigoOyDCargarPagosA = _ListaClientesEncabezado.First.CodigoOYD
							strNombreCodigoOyD = _ListaClientesEncabezado.First.Nombre
						End If
					End If
				End If

			ElseIf _IDTipoClienteCargarPagosA = GSTR_TERCERO Then
				TipoIdClienteDescripcion = "Otro cliente"
			End If
			MyBase.CambioItem("IDTipoClienteCargarPagosA")
		End Set
	End Property

	Private _IDTipoClienteCargarPagosAFondos As String
	Public Property IDTipoClienteCargarPagosAFondos() As String
		Get
			Return _IDTipoClienteCargarPagosAFondos
		End Get
		Set(ByVal value As String)
			_IDTipoClienteCargarPagosAFondos = value
			'IDTipoClienteEntregado = _IDTipoClienteCargarPagosAFondos
			If value = GSTR_CLIENTE Then
				TipoIdClienteDescripcion = "Cliente"

				If logCambiarPropiedadesPOPPUP Then
					If Not IsNothing(_ListaClientesEncabezado) Then
						If _ListaClientesEncabezado.Count = 1 Then
							strCodigoOyDCargarPagosAFondos = _ListaClientesEncabezado.First.CodigoOYD
							strNombreCodigoOyDFondos = _ListaClientesEncabezado.First.Nombre
						End If
					End If
				End If
			Else
				TipoIdClienteDescripcion = "Otro cliente"
			End If

			MyBase.CambioItem("IDTipoClienteCargarPagosAFondos")
		End Set
	End Property

	Private _IDTipoClienteEntregado As String
	Public Property IDTipoClienteEntregado() As String
		Get
			Return _IDTipoClienteEntregado
		End Get
		Set(ByVal value As String)
			_IDTipoClienteEntregado = value
			If Not String.IsNullOrEmpty(_IDTipoClienteEntregado) Then

				If _IDTipoClienteEntregado = GSTR_CLIENTE Then
					VerDatosCliente = Visibility.Visible
					HablitarDatosTerceros = False

					Dim logContieneDetalles As Boolean = False

					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
						If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
							logContieneDetalles = True
						End If
					End If
					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
						If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
							logContieneDetalles = True
						End If
					End If
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
						If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > 0 Then
							logContieneDetalles = True
						End If
					End If
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
						If _ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
							logContieneDetalles = True
						End If
					End If
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
						If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
							logContieneDetalles = True
						End If
					End If

					If logContieneDetalles Then
						HabilitarBuscadorCliente = False
					Else
						If Editando Then
							HabilitarBuscadorCliente = True
						Else
							HabilitarBuscadorCliente = False
						End If
					End If

					If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
						If String.IsNullOrEmpty(TesoreriaOrdenesPlusRC_Selected.strIDComitente) Then
							'mostrarMensaje("No se ha seleccionado ningún clientes, por favor seleccione un cliente o ingrese los datos de un tercero", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
							HabilitarEntregar = False
						Else
							If Not IsNothing(ComitenteSeleccionado) Then
								strNombreEntregado = ComitenteSeleccionado.Nombre
								strNroDocumentoEntregado = ComitenteSeleccionado.NroDocumento
								strTipoIdentificacionEntregado = ComitenteSeleccionado.CodTipoIdentificacion
								HabilitarEntregar = False
							Else
								strNombreEntregado = TesoreriaOrdenesPlusRC_Selected.strNombre_Instrucciones
								strNroDocumentoEntregado = TesoreriaOrdenesPlusRC_Selected.strNroDocumento_Instrucciones
								strTipoIdentificacionEntregado = TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion_Instrucciones
							End If
						End If
					Else
						If Not IsNothing(ComitenteSeleccionado) Then
							strNombreEntregado = ComitenteSeleccionado.Nombre
							strNroDocumentoEntregado = ComitenteSeleccionado.NroDocumento
							strTipoIdentificacionEntregado = ComitenteSeleccionado.CodTipoIdentificacion
							HabilitarEntregar = False
						Else
							strNombreEntregado = TesoreriaOrdenesPlusRC_Selected.strNombre_Instrucciones
							strNroDocumentoEntregado = TesoreriaOrdenesPlusRC_Selected.strNroDocumento_Instrucciones
							strTipoIdentificacionEntregado = TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion_Instrucciones
						End If
					End If
				ElseIf _IDTipoClienteEntregado = GSTR_TERCERO Then
					If TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto <> GSTR_FONDOS_TIPOPRODUCTO Then
						If BorrarCliente Then
							BorrarCliente = False
						End If

						HablitarDatosTerceros = True
						VerDatosCliente = Visibility.Collapsed
						If logNuevoRegistro Or logEditarRegistro Then
							HabilitarEnEdicion = True
							HabilitarEntregar = True
						End If
						ListaDireccionesClientes = Nothing
						BorrarCliente = True
						strCodigoOyDCliente = String.Empty
						strNombreEntregado = String.Empty
						strNroDocumentoEntregado = String.Empty
						strTipoIdentificacionEntregado = String.Empty
						TesoreriaOrdenesPlusRC_Selected.strNroDocumento = String.Empty
						TesoreriaOrdenesPlusRC_Selected.strNombre = String.Empty
						TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion = String.Empty
						TesoreriaOrdenesPlusRC_Selected.strIDComitente = String.Empty
						ComitenteSeleccionado = Nothing
						HabilitarBuscadorCliente = False
					End If

				End If
			End If

			MyBase.CambioItem("IDTipoClienteEntregado")
		End Set
	End Property

	Private _VerOtraDireccion As Visibility = Visibility.Collapsed
	Public Property VerOtraDireccion() As Visibility
		Get
			Return _VerOtraDireccion
		End Get
		Set(ByVal value As Visibility)
			_VerOtraDireccion = value
			MyBase.CambioItem("VerOtraDireccion")
		End Set
	End Property

	Private _CheckOtraDireccion As Boolean
	Public Property CheckOtraDireccion() As Boolean
		Get
			Return _CheckOtraDireccion
		End Get
		Set(ByVal value As Boolean)
			_CheckOtraDireccion = value
			If _CheckOtraDireccion Then
				VerOtraDireccion = Visibility.Visible
				CheckDireccionCliente = False
				CheckClientetraeCheque = False
			Else
				VerOtraDireccion = Visibility.Collapsed
				strDireccionCheque = String.Empty
				strTelefono = String.Empty
				strCiudad = String.Empty
				strSector = String.Empty
			End If
			MyBase.CambioItem("CheckOtraDireccion")
		End Set
	End Property

	Private _CheckDireccionCliente As Boolean
	Public Property CheckDireccionCliente() As Boolean
		Get
			Return _CheckDireccionCliente
		End Get
		Set(ByVal value As Boolean)
			_CheckDireccionCliente = value
			If _CheckDireccionCliente Then
				CheckOtraDireccion = False
				CheckClientetraeCheque = False
				VerDireccionesRegistradas = Visibility.Visible
				If Not IsNothing(ListaDireccionesClientes) Then
					If ListaDireccionesClientes.Count > 0 Then
						If ListaDireccionesClientes.Count = 1 Then
							DireccionRegistrada = ListaDireccionesClientes.First.ID
						End If
						HabilitarDireccionesRegistradas = True
					Else
						HabilitarDireccionesRegistradas = False
						mostrarMensaje("¡¡¡No hay direcciones registradas!!!", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					End If
				Else
					HabilitarDireccionesRegistradas = False
					mostrarMensaje("¡¡¡No hay direcciones registradas!!!", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

			Else
				VerDireccionesRegistradas = Visibility.Collapsed
				DireccionRegistrada = 0
			End If

			MyBase.CambioItem("CheckDireccionCliente")
		End Set
	End Property

	Private _CheckClientetraeCheque As Boolean = True
	Public Property CheckClientetraeCheque() As Boolean
		Get
			Return _CheckClientetraeCheque
		End Get
		Set(ByVal value As Boolean)
			_CheckClientetraeCheque = value
			If _CheckClientetraeCheque Then
				CheckOtraDireccion = False
				CheckDireccionCliente = False
			End If

			MyBase.CambioItem("CheckClientetraeCheque")
		End Set
	End Property

	Private _HabilitarDireccionesRegistradas As Boolean
	Public Property HabilitarDireccionesRegistradas() As Boolean
		Get
			Return _HabilitarDireccionesRegistradas
		End Get
		Set(ByVal value As Boolean)
			_HabilitarDireccionesRegistradas = value
			MyBase.CambioItem("HabilitarDireccionesRegistradas")
		End Set
	End Property

	Private _strTipoIdentificacionTitularInstruccionesDESCRIPCION As String
	Public Property strTipoIdentificacionTitularInstruccionesDESCRIPCION() As String
		Get
			Return _strTipoIdentificacionTitularInstruccionesDESCRIPCION
		End Get
		Set(ByVal value As String)
			_strTipoIdentificacionTitularInstruccionesDESCRIPCION = value
			MyBase.CambioItem("strTipoIdentificacionTitularInstruccionesDESCRIPCION")
		End Set
	End Property

	Private _DescripcionTipoIdentificacionInstrucciones As String
	Public Property DescripcionTipoIdentificacionInstrucciones() As String
		Get
			Return _DescripcionTipoIdentificacionInstrucciones
		End Get
		Set(ByVal value As String)
			_DescripcionTipoIdentificacionInstrucciones = value
			MyBase.CambioItem("DescripcionTipoIdentificacionInstrucciones")
		End Set
	End Property

	Private _strTipoIdentificacionInstrucciones As String
	Public Property strTipoIdentificacionInstrucciones() As String
		Get
			Return _strTipoIdentificacionInstrucciones
		End Get
		Set(ByVal value As String)
			_strTipoIdentificacionInstrucciones = value
			If Not String.IsNullOrEmpty(_strTipoIdentificacionInstrucciones) Then
				If Not IsNothing(DiccionarioCombosOYDPlus) Then
					If DiccionarioCombosOYDPlus.ContainsKey("TIPOID") Then
						For Each li In DiccionarioCombosOYDPlus("TIPOID").Where(Function(i) i.Retorno = _strTipoIdentificacionInstrucciones)
							DescripcionTipoIdentificacionInstrucciones = li.Descripcion
						Next
					End If
				End If
			End If
			MyBase.CambioItem("strTipoIdentificacionInstrucciones")
		End Set
	End Property

	Private _strSectorInstrucciones As String
	Public Property strSectorInstrucciones() As String
		Get
			Return _strSectorInstrucciones
		End Get
		Set(ByVal value As String)
			_strSectorInstrucciones = value
			MyBase.CambioItem("strSectorInstrucciones")
		End Set
	End Property


	Private _strTipoCuentaInstrucciones As String
	Public Property strTipoCuentaInstrucciones() As String
		Get
			Return _strTipoCuentaInstrucciones
		End Get
		Set(ByVal value As String)
			_strTipoCuentaInstrucciones = value
			MyBase.CambioItem("strTipoCuentaInstrucciones")
		End Set
	End Property

	Private _strNroDocumentoInstrucciones As String
	Public Property strNroDocumentoInstrucciones() As String
		Get
			Return _strNroDocumentoInstrucciones
		End Get
		Set(ByVal value As String)
			_strNroDocumentoInstrucciones = value
			If (ClientePresente = True Or ClienteRecoge = True) And String.IsNullOrEmpty(_strNroDocumentoInstrucciones) Then
				_strNroDocumentoInstrucciones = TesoreriaOrdenesPlusRC_Selected.strNroDocumento_Instrucciones
			End If
			MyBase.CambioItem("strNroDocumentoInstrucciones")
		End Set
	End Property

	Private _strNombreInstrucciones As String
	Public Property strNombreInstrucciones() As String
		Get
			Return _strNombreInstrucciones
		End Get
		Set(ByVal value As String)
			_strNombreInstrucciones = value
			If (ClientePresente = True Or ClienteRecoge = True) And String.IsNullOrEmpty(_strNombreInstrucciones) Then
				_strNombreInstrucciones = TesoreriaOrdenesPlusRC_Selected.strNombre_Instrucciones
			End If
			MyBase.CambioItem("strNombreInstrucciones")
		End Set
	End Property

	Private _strCodigoBancoInstrucciones As String
	Public Property strCodigoBancoInstrucciones() As String
		Get
			Return _strCodigoBancoInstrucciones
		End Get
		Set(ByVal value As String)
			_strCodigoBancoInstrucciones = value
			'If ConsignarCuentaChecked = True And String.IsNullOrEmpty(_strCodigoBancoInstrucciones) Then
			'    _strCodigoBancoInstrucciones = TesoreriaOrdenesPlusRC_Selected.lngIDBanco.ToString
			'End If
			MyBase.CambioItem("strCodigoBancoInstrucciones")
		End Set
	End Property

	Private _strCuentaInstrucciones As String
	Public Property strCuentaInstrucciones() As String
		Get
			Return _strCuentaInstrucciones
		End Get
		Set(ByVal value As String)
			_strCuentaInstrucciones = value
			MyBase.CambioItem("strCuentaInstrucciones")
		End Set
	End Property

	Private _VerCamposOpcionalesLlevarDireccion As Visibility = Visibility.Collapsed
	Public Property VerCamposOpcionalesLlevarDireccion() As Visibility
		Get
			Return _VerCamposOpcionalesLlevarDireccion
		End Get
		Set(ByVal value As Visibility)
			_VerCamposOpcionalesLlevarDireccion = value
			MyBase.CambioItem("VerCamposOpcionalesLlevarDireccion")
		End Set
	End Property

	Private _VerCuentasRegistradas As Visibility = Visibility.Collapsed
	Public Property VerCuentasRegistradas() As Visibility
		Get
			Return _VerCuentasRegistradas
		End Get
		Set(ByVal value As Visibility)
			_VerCuentasRegistradas = value
			MyBase.CambioItem("VerCuentasRegistradas")
		End Set
	End Property

	Private _VerConsignarCuenta As Visibility = Visibility.Collapsed
	Public Property VerConsignarCuenta() As Visibility
		Get
			Return _VerConsignarCuenta
		End Get
		Set(ByVal value As Visibility)
			_VerConsignarCuenta = value
			MyBase.CambioItem("VerConsignarCuenta")
		End Set
	End Property

	Private _VerDireccionesRegistradas As Visibility = Visibility.Collapsed
	Public Property VerDireccionesRegistradas() As Visibility
		Get
			Return _VerDireccionesRegistradas
		End Get
		Set(ByVal value As Visibility)
			_VerDireccionesRegistradas = value
			MyBase.CambioItem("VerDireccionesRegistradas")
		End Set
	End Property

	Private _VerLlevarDireccion As Visibility = Visibility.Collapsed
	Public Property VerLlevarDireccion() As Visibility
		Get
			Return _VerLlevarDireccion
		End Get
		Set(ByVal value As Visibility)
			_VerLlevarDireccion = value
			MyBase.CambioItem("VerLlevarDireccion")
		End Set
	End Property

	Private _VerInstrucciones As Visibility = Visibility.Visible
	Public Property VerInstrucciones() As Visibility
		Get
			Return _VerInstrucciones
		End Get
		Set(ByVal value As Visibility)
			_VerInstrucciones = value
			MyBase.CambioItem("VerInstrucciones")
		End Set
	End Property

	Private _VerTipoCliente As Visibility = Visibility.Collapsed
	Public Property VerTipoCliente() As Visibility
		Get
			Return _VerTipoCliente
		End Get
		Set(ByVal value As Visibility)
			_VerTipoCliente = value
			MyBase.CambioItem("VerTipoCliente")
		End Set
	End Property

	Private _HabilitarDocumento As Boolean
	Public Property HabilitarDocumento() As Boolean
		Get
			Return _HabilitarDocumento
		End Get
		Set(ByVal value As Boolean)
			_HabilitarDocumento = value
			MyBase.CambioItem("HabilitarDocumento")
		End Set
	End Property

	Private _ClienteRecoge As Boolean
	Public Property ClienteRecoge() As Boolean
		Get
			Return _ClienteRecoge
		End Get
		Set(ByVal value As Boolean)
			_ClienteRecoge = value
			If _ClienteRecoge Then

				VerLlevarDireccion = Visibility.Collapsed
				HabilitarDocumento = False
				VerCuentasRegistradas = Visibility.Collapsed
				strNroDocumentoInstrucciones = TesoreriaOrdenesPlusRC_Selected.strNroDocumento
				strNombreInstrucciones = TesoreriaOrdenesPlusRC_Selected.strNombre
				strTipoIdentificacionInstrucciones = TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion
				VerCamposOpcionalesLlevarDireccion = Visibility.Collapsed
				VerConsignarCuenta = Visibility.Collapsed
				'DeshabilitarOpciones(OpcionesInstrucciones.ClienteRecoge)
			End If
			MyBase.CambioItem("ClienteRecoge")
		End Set
	End Property

	Private _ClientePresente As Boolean
	Public Property ClientePresente() As Boolean
		Get
			Return _ClientePresente
		End Get
		Set(ByVal value As Boolean)
			_ClientePresente = value
			If _ClientePresente Then

				VerLlevarDireccion = Visibility.Collapsed
				HabilitarDocumento = False
				VerCuentasRegistradas = Visibility.Collapsed
				strNroDocumentoInstrucciones = TesoreriaOrdenesPlusRC_Selected.strNroDocumento
				strNombreInstrucciones = TesoreriaOrdenesPlusRC_Selected.strNombre
				strTipoIdentificacionInstrucciones = TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion
				VerCamposOpcionalesLlevarDireccion = Visibility.Collapsed
				VerConsignarCuenta = Visibility.Collapsed

				'DeshabilitarOpciones(OpcionesInstrucciones.ClientePresente)
			End If
			MyBase.CambioItem("ClientePresente")
		End Set
	End Property

	Private _RecogeTercero As Boolean
	Public Property RecogeTercero() As Boolean
		Get
			Return _RecogeTercero
		End Get
		Set(ByVal value As Boolean)
			_RecogeTercero = value
			If _RecogeTercero Then

				logEsTercero_Instrucciones = True

				If logNuevoRegistro Or logEditarRegistro Then
					strNombreInstrucciones = String.Empty
					strNroDocumentoInstrucciones = String.Empty
					strTipoIdentificacionInstrucciones = Nothing
					strTipoCuentaInstrucciones = Nothing
					strCodigoBancoInstrucciones = Nothing
				End If

				HabilitarDocumento = True
				VerCuentasRegistradas = Visibility.Collapsed
				VerCamposOpcionalesLlevarDireccion = Visibility.Collapsed
				'DeshabilitarOpciones(OpcionesInstrucciones.RecogeTercero)
			End If
			MyBase.CambioItem("RecogeTercero")
		End Set
	End Property


#End Region

#Region "Resultados Asincrónicos"

	Public Sub TerminoPreguntarDuplicarOrden(ByVal sender As Object, ByVal e As EventArgs)
		Try
			IsBusy = True
			IsBusyDetalles = True
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
			Dim intIDRegistro As Integer

			If objResultado.DialogResult Then
				If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
					Dim objListaDetalleCheques As New List(Of TesoreriaOyDPlusChequesRecibo)
					Dim objListaDetalleTransferencias As New List(Of TesoreriaOyDPlusTransferenciasRecibo)
					Dim objListaDetalleConsignaciones As New List(Of TesoreriaOyDPlusConsignacionesRecibo)
					Dim objListaDetalleCargarPagosA As New List(Of TesoreriaOyDPlusCargosPagosARecibo)
					Dim objListaDetalleCargarPagosAFondos As New List(Of TesoreriaOyDPlusCargosPagosAFondosRecibo)

					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
						For Each Cheque In ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
							intIDRegistro = intIDRegistro - 1
							Cheque.strConsecutivo = String.Empty
							Cheque.strFormaPago = GSTR_CHEQUE
							Cheque.lngID = intIDRegistro
							Cheque.strEstado = GSTR_PENDIENTE_Plus_Detalle
							Cheque.lngIDTesoreriaEncabezado = Nothing
							Cheque.logEsProcesada = True
							objListaDetalleCheques.Add(Cheque)
						Next
					Else
						objListaDetalleCheques = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
					End If


					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
						For Each Transferencia In ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
							intIDRegistro = intIDRegistro - 1
							Transferencia.strConsecutivo = String.Empty
							Transferencia.strFormaPago = GSTR_TRANSFERENCIA
							Transferencia.lngID = intIDRegistro
							Transferencia.lngIDTesoreriaEncabezado = Nothing
							Transferencia.strTipo = GSTR_ORDENRECIBO
							Transferencia.strEstado = GSTR_PENDIENTE_Plus_Detalle
							Transferencia.logEsProcesada = True
							objListaDetalleTransferencias.Add(Transferencia)
						Next
					Else
						objListaDetalleTransferencias = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
					End If

					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
						For Each Consignaciones In ListatesoreriaordenesplusRC_Detalle_Consignaciones
							intIDRegistro = intIDRegistro - 1
							Consignaciones.strConsecutivo = String.Empty
							Consignaciones.strEstado = GSTR_PENDIENTE_Plus_Detalle
							Consignaciones.strFormaPago = GSTR_CONSIGNACIONES
							Consignaciones.lngID = intIDRegistro
							Consignaciones.lngIDTesoreriaEncabezado = Nothing
							Consignaciones.logEsProcesada = True
							objListaDetalleConsignaciones.Add(Consignaciones)
						Next
					Else
						objListaDetalleConsignaciones = ListatesoreriaordenesplusRC_Detalle_Consignaciones
					End If

					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
						For Each CargarPagosA In ListatesoreriaordenesplusRC_Detalle_CargarPagosA
							CargarPagosA.strConsecutivo = String.Empty
							'CargarPagosA.strEstado = GSTR_PENDIENTE_Plus_Detalle
							CargarPagosA.strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOA
							CargarPagosA.lngID = Nothing
							CargarPagosA.lngIDTesoreriaEncabezado = Nothing
							CargarPagosA.logEsProcesada = True
							objListaDetalleCargarPagosA.Add(CargarPagosA)
						Next
					Else
						objListaDetalleCargarPagosA = ListatesoreriaordenesplusRC_Detalle_CargarPagosA
					End If

					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
						For Each CargarPagosAFondos In ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
							CargarPagosAFondos.strConsecutivo = String.Empty
							'CargarPagosA.strEstado = GSTR_PENDIENTE_Plus_Detalle
							CargarPagosAFondos.strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOAFONDOS
							CargarPagosAFondos.lngID = Nothing
							CargarPagosAFondos.lngIDTesoreriaEncabezado = Nothing
							CargarPagosAFondos.logEsProcesada = True
							objListaDetalleCargarPagosAFondos.Add(CargarPagosAFondos)
						Next
					Else
						objListaDetalleCargarPagosAFondos = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
					End If


					ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = Nothing
					ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = Nothing
					ListatesoreriaordenesplusRC_Detalle_Consignaciones = Nothing
					ListatesoreriaordenesplusRC_Detalle_CargarPagosA = Nothing
					ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = Nothing


					dcProxy.TesoreriaOyDPlusChequesRecibos.Clear()
					dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Clear()
					dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Clear()
					dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Clear()
					dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Clear()

					ConfigurarOrdenDuplicada(TesoreriaOrdenesPlusRC_Selected, objListaDetalleCheques, objListaDetalleTransferencias, objListaDetalleConsignaciones, objListaDetalleCargarPagosA, objListaDetalleCargarPagosAFondos)

					If visNavegando = "Collapsed" Then
						MyBase.CambiarFormulario_Forma_Manual()
					End If
				End If
			Else
				IsBusy = False
				IsBusyDetalles = False
			End If


		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al duplicar la orden de tesoreria",
															 Me.ToString(), "TerminoPreguntarDuplicarOrden", Application.Current.ToString(), Program.Maquina, ex)

		End Try

	End Sub

	Public Sub TerminoValidarEstadoTesoreria(ByVal lo As LoadOperation(Of tblFechasHabiles))
		Try
			IsBusy = False
			If lo.HasError = False Then
				If lo.UserState = "ValidarFechaCierre" Then
					If dcProxy.tblFechasHabiles.Count > 0 Then
						If dcProxy.tblFechasHabiles.FirstOrDefault.logEsHabil = False Then
							A2Utilidades.Mensajes.mostrarMensaje("No se puede guardar la orden de tesoreria por que la fecha que ingresó no es una fecha hábil, o esta es menor a la del sistema corrija la fecha para guardar la orden de tesoreria ", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						Else
							If Not IsNothing(dcProxy.tblFechasHabiles.FirstOrDefault.dtmMayorFechaHabil) Then
								FechaOrden = dcProxy.tblFechasHabiles.FirstOrDefault.dtmMayorFechaHabil
							End If

							consultarCombosEspecificosFondo(_TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos, "ORDENRECIBOFONDOS")
						End If
					End If
				ElseIf lo.UserState = "validar_fecha_guardado" Then
					If dcProxy.tblFechasHabiles.Count > 0 Then
						If dcProxy.tblFechasHabiles.FirstOrDefault.logEsHabil Then
							If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD Then
								dcProxy.tblFechasHabiles.Clear()
								dcProxy.Load(dcProxy.CalcularDiaHabilQuery(_TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto, _TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos, FechaAplicacion, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEstadoTesoreria, "validar_fechaaplicacion_guardado")
							Else
								ValidarIngresoOrdenTesoreria()
							End If
						Else
							A2Utilidades.Mensajes.mostrarMensaje("No se puede guardar la orden de tesoreria por que la fecha que ingresó no es una fecha hábil, o esta es menor a la del sistema corrija la fecha para guardar la orden de tesoreria ", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						End If
					End If
				ElseIf lo.UserState = "validar_fechaaplicacion_guardado" Then
					If dcProxy.tblFechasHabiles.Count > 0 Then
						If dcProxy.tblFechasHabiles.FirstOrDefault.logEsHabil Then
							ValidarIngresoOrdenTesoreria()
						Else
							A2Utilidades.Mensajes.mostrarMensaje("No se puede guardar la orden de tesoreria por que la fecha de aplicación que ingresó no es una fecha hábil.", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						End If

					End If
				End If

			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar dia habil",
												 Me.ToString(), "TerminoValidarEstadoTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
				IsBusy = False
				IsBusyDetalles = False
			End If

		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar dia habil",
															 Me.ToString(), "TerminoValidarEstadoTesoreria", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Sub TerminoCancelarEditarRegistro(ByVal lo As InvokeOperation(Of Integer))
		Try

		Catch ex As Exception

		End Try
	End Sub

	Private Sub TerminoGuardarExitosamente(ByVal sender As Object, ByVal e As EventArgs)
		Try

			IsBusy = True
			logDuplicarRegistro = False
			logCancelarRegistro = False
			logEditarRegistro = False
			logNuevoRegistro = False
			HabilitarEncabezado = False
			HabilitarNegocio = False
			If String.IsNullOrEmpty(TesoreriaOrdenesPlusRC_Selected.ValorEstado) Then
				TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTE_Plus
			End If
			Program.VerificarCambiosProxyServidor(dcProxy)
			dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, String.Empty)

			VerInstrucciones = Visibility.Visible
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al Guardar Orden.", Me.ToString(), "TerminoGuardarExitosamente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try

	End Sub

	Private Sub TerminoPreguntarAprobacion(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				If String.IsNullOrEmpty(Aprobaciones) Then
					Aprobaciones = objResultado.CodConfirmacion
					AprobacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'**'{3}'", objResultado.NombreRegla, objResultado.CodRegla, objResultado.MensajeRegla, String.Format("{0}++{1}", objResultado.Observaciones, objResultado.TextoConfirmacion.Replace("|", "++")))
				Else
					Aprobaciones = String.Format("{0}|{1}", Aprobaciones, objResultado.CodConfirmacion)
					AprobacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'", AprobacionesUsuario, objResultado.NombreRegla, objResultado.CodRegla, objResultado.MensajeRegla, String.Format("{0}++{1}", objResultado.Observaciones, objResultado.TextoConfirmacion.Replace("|", "++")))
				End If

				Dim ListaJustificacion As New List(Of A2Utilidades.CausasJustificacionMensajePregunta)
				Dim logEsHtml As Boolean = False
				Dim strMensajeDetallesHtml As String = String.Empty
				Dim strMensajeRetornoHtml As String = String.Empty

				For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereAprobacion = True).ToList
					If Not IsNothing(li.Confirmacion) Then
						If Not Aprobaciones.Contains(li.Confirmacion) Then

							mostrarMensajePregunta(li.Mensaje,
												   Program.TituloSistema,
												   "PREGUNTARAPROBACION",
												   AddressOf TerminoPreguntarAprobacion,
												   True,
												   "¿Desea continuar?",
												   IIf(ListaJustificacion.Count > 0, True, False),
												   True,
												   IIf(ListaJustificacion.Count > 0, False, True),
												   IIf(ListaJustificacion.Count = 0, False, True),
												   li.Confirmacion,
												   li.Regla,
												   li.NombreRegla,
												   IIf(ListaJustificacion.Count > 0, ListaJustificacion, Nothing),
												   "Reglas incumplidas en los detalles de las ordenes de recaudo",
												   logEsHtml,
												   strMensajeDetallesHtml)
							Exit For
						End If
					End If
				Next
				TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTEAPROBACION_Plus
				CantidadAprobaciones = CantidadAprobaciones + 1
				IsBusy = True
			Else
				IsBusy = False
				LimpiarVariablesConfirmadas()
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la aprobación.", Me.ToString(), "TerminoPreguntarAprobacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadAprobaciones = 0
		End Try
	End Sub

	Sub TerminoTraerDireccionesCliente(lo As LoadOperation(Of OyDPLUSTesoreria.TempDireccionesClientes))
		Try
			If lo.HasError = False Then
				If dcProxy.TempDireccionesClientes.Count > 0 Then
					ListaDireccionesClientes = dcProxy.TempDireccionesClientes.ToList
				Else
					ListaDireccionesClientes = Nothing
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Direcciones Clientes",
												 Me.ToString(), "TerminoTraerDireccionesCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
			End If
			IsBusy = False
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Direcciones Clientes",
															 Me.ToString(), "TerminoTraerDireccionesCliente", Application.Current.ToString(), Program.Maquina, ex)
		End Try


	End Sub

	Sub TerminoTraerCuentasCliente(lo As LoadOperation(Of OyDPLUSTesoreria.TempCuentasClientes))
		Try
			If lo.HasError = False Then
				If dcProxy.TempCuentasClientes.Count > 0 Then
					ListaCuentasClientes = dcProxy.TempCuentasClientes.ToList
				Else
					ListaCuentasClientes = Nothing
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Cuentas Clientes",
												 Me.ToString(), "TerminoTraerCuentasCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
			End If
			IsBusy = False
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Cuentas Clientes",
															 Me.ToString(), "TerminoTraerCuentasCliente", Application.Current.ToString(), Program.Maquina, ex)
		End Try


	End Sub

	Private Sub TerminoConsultarReceptoresUsuario(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
		Try
			If lo.HasError = False Then
				If lo.Entities.Count > 0 Then
					ListaReceptoresUsuario = lo.Entities.ToList
					If lo.UserState <> "INICIO" And lo.UserState <> "EDITAR" Then
						If logEditarRegistro Or logNuevoRegistro Then
							ObtenerValoresDefectoOYDPLUS("RECEPTOR")
						End If
					ElseIf lo.UserState = "EDITAR" Then
						CargarCombosOYDPLUS(TesoreriaOrdenesPlusAnterior.strCodigoReceptor, lo.UserState)
					End If
				Else
					If logNuevoRegistro Or logEditarRegistro Then
						CancelarEditarRegistro()
						mostrarMensaje("No hay ningun receptor configurado para el Usuario Actual Logueado.", "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						Editando = False
						'HabilitarNegocio = False
						IsBusy = False
					End If
					'If lo.UserState = "INICIO" Then
					'    A2Utilidades.Mensajes.mostrarMensaje("No hay ningun receptor configurado para el usuario.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					'    Editando = False
					'    'HabilitarNegocio = False
					'    IsBusy = False
					'End If
				End If
			Else
				If logNuevoRegistro Or logEditarRegistro Then
					A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
					'HabilitarNegocio = False
				End If
				IsBusy = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub TerminoValidarEstadoTesoreriaDetalle(ByVal lo As LoadOperation(Of TempValidarEstadoOrden))
		Try
			If Not lo.HasError Then
				If lo.Entities.Count > 0 Then
					For Each li In lo.Entities
						If li.logExitoso Then
							Select Case lo.UserState
								Case GSTR_CHEQUE
									If ObjetoEditarCheque() Then
										Program.Modal_OwnerMainWindowsPrincipal(objWppChequeRC)
										objWppChequeRC.ShowDialog()
									Else
										mostrarMensaje("No se puede editar verifique que el registro tenga un estado válido en el que se pueda Editar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

									End If

								Case GSTR_TRANSFERENCIA
									If ObjetoEditarTransferencia() Then
										Program.Modal_OwnerMainWindowsPrincipal(objWppTransferencia_RC)
										objWppTransferencia_RC.ShowDialog()
										If TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.logEsCuentaRegistrada = True Then
											If CuentaRegistrada = 0 Then
												mostrarMensaje("Señor usuario, la cuenta que se había registrado para el cliente ya no está asociada. Por favor revise la configuración.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
											End If
										End If
									Else
										mostrarMensaje("No se puede editar verifique que el registro tenga un estado válido en el que se pueda Editar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

									End If
								Case GSTR_CONSIGNACIONES
									If ObjetoEditarConsignacion() Then
										Program.Modal_OwnerMainWindowsPrincipal(objWppConsignacion)
										objWppConsignacion.ShowDialog()
									Else
										mostrarMensaje("No se puede editar verifique que el registro tenga un estado válido en el que se pueda Editar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

									End If
								Case GSTR_ORDENRECIBO_CARGOPAGOA
									If ObjetoEditarCargarPagosA() Then
										Program.Modal_OwnerMainWindowsPrincipal(objWppCargarPagosA)
										objWppCargarPagosA.ShowDialog()
									Else
										mostrarMensaje("No se puede editar verifique que el registro tenga un estado válido en el que se pueda Editar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

									End If
								Case GSTR_ORDENRECIBO_CARGOPAGOAFONDOS
									If ObjetoEditarCargarPagosFondos() Then
										Program.Modal_OwnerMainWindowsPrincipal(objWppCargarPagosAFondos)
										objWppCargarPagosAFondos.ShowDialog()
									Else
										mostrarMensaje("No se puede editar verifique que el registro tenga un estado válido en el que se pueda Editar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
									End If

							End Select

						Else
							mostrarMensaje(li.strMensajeValidacion, Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						End If
					Next


				End If

			Else
				A2Utilidades.Mensajes.mostrarMensaje("La Orden de Tesoreria no se puede modificar.", "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al Validar el estado del detalle de la Orden de Tesoreria.", Me.ToString(), "TerminoValidarEstadoTesoreria", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub TerminoValidarEstadoTesoreria(ByVal lo As LoadOperation(Of TempValidarEstadoOrden))
		Try

			If Not lo.HasError Then
				If lo.Entities.Count > 0 Then
					For Each li In lo.Entities
						If li.logExitoso Then
							IsBusy = False
							ObtenerValoresOrdenAnterior(TesoreriaOrdenesPlusRC_Selected, TesoreriaOrdenesPlusAnterior)

							HabilitarEdicionEncabezado = False
							CargarReceptoresUsuarioOYDPLUS("EDITAR")
							CargarParametrosReceptorOYDPLUS(TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor, "EDITAR")
							HabilitarEncabezado = False
							HabilitarEnEdicion = True
							HabilitarFecha = True
							HabilitarInstrucciones = True
							HabilitarTipoProducto = False
							If HabilitarBuscadorCliente Then
								HabilitarBuscadorCliente = False
							End If
							VerInstrucciones = Visibility.Visible

							HabilitarReceptor = False
							logHayEncabezado = True
							logCancelarRegistro = False
							logEditarRegistro = True
							logNuevoRegistro = False
							logDuplicarRegistro = False
							logBuscar = False
							If logNuevoRegistro Or logEditarRegistro Then
								HabilitarEntregar = True
							End If
							VerInstrucciones = Visibility.Visible
							If logXTesorero Then
								If Not IsNothing(objViewModelTesorero) Then
									If objViewModelTesorero.strEstado = GSTR_PENDIENTE_Plus Then
										Editando = True
									Else
										mostrarMensaje("¡Solo se pueden editar registros con estado pendiente!", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
										Editando = False
									End If
								End If
							Else
								Editando = True
							End If
							Instrucciones()
							HabilitarEntregarA = False
							HabilitarBuscadorCliente = False
							HabilitarEncabezado = False
							HabilitarReceptor = False
							HabilitarTipoProducto = False

							'Limpia las variables para subir los archivos.
							ListaArchivoConsignacion = Nothing
							ListaArchivosCheque = Nothing
							ListaArchivoTransferencia = Nothing

							HabilitarSubirArchivo()
							MyBase.CambioItem("Editando")
							If logXTesorero Then
								VerEditarXTesorero = Visibility.Collapsed
								VerGrabarXTesorero = Visibility.Visible
								VerCancelarXTesorero = Visibility.Visible
							End If
							Habilitar_Encabezado()

							If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
								ConsultarCarterasColectivasFondos(_TesoreriaOrdenesPlusRC_Selected.strIDComitente, False)
							End If

						Else
							MyBase.RetornarValorEdicionNavegacion()
							IsBusy = False
							Editando = False
							mostrarMensaje(li.strMensajeValidacion, Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						End If
					Next


				End If

			Else
				MyBase.RetornarValorEdicionNavegacion()
				A2Utilidades.Mensajes.mostrarMensaje("La Orden de Tesoreria no se puede modificar.", "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				IsBusy = False
				IsBusyDetalles = False
			End If


		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al Validar el estado de la Orden de Tesoreria.", Me.ToString(), "TerminoValidarEstadoTesoreria", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
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
				'CargarCombosOYDPLUS(TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor, "RECEPTOR")
				If logNuevoRegistro Or logEditarRegistro Then
					ObtenerValoresCombos(False)

					If lo.UserState = "EDITAR" Then

						LlamarDetalle()
						Instrucciones()
						ObtenerValoresOrdenAnterior(TesoreriaOrdenesPlusAnterior, TesoreriaOrdenesPlusRC_Selected)
					End If
				Else
					ObtenerValoresCombos(True)
				End If

			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los parametros del receptor.", Me.ToString(), "TerminoConsultarParametrosReceptor", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
				IsBusy = False
				IsBusyDetalles = False
			End If
			IsBusy = False
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los parametros del receptor.", Me.ToString(), "TerminoConsultarParametrosReceptor", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub TerminoConsultarCombosOYD(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
		Try
			If lo.HasError = False Then
				If lo.Entities.Count > 0 Then
					IsBusyReceptor = False
					Dim strNombreCategoria As String = String.Empty
					Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
					Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
					Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
					Dim objDiccionarioCompletoTodosInicio As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

					Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

					For Each li In listaCategorias
						strNombreCategoria = li
						objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
						objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
						objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
						objDiccionarioCompletoTodosInicio.Add(strNombreCategoria, objListaNodosCategoria)
					Next

					If Not IsNothing(DiccionarioCombosOYDPlus) Then
						DiccionarioCombosOYDPlus.Clear()
					End If

					DiccionarioCombosOYDPlus = Nothing
					DiccionarioCombosOYDPlusCompleta = Nothing

					DiccionarioCombosOYDPlusCompleta = objDiccionarioCompleto
					DiccionarioCombosOYDPlus = objDiccionario

					If lo.UserState = "INICIO" Then
						DiccionarioCombosOYDPlusTodosInicio = objDiccionarioCompletoTodosInicio

						If DiccionarioCombosOYDPlusTodosInicio.ContainsKey("CONCEPTODEFECTO_ORDENRECIBO_FONDOS") Then
							If DiccionarioCombosOYDPlusTodosInicio("CONCEPTODEFECTO_ORDENRECIBO_FONDOS").Count > 0 Then
								strConceptoDefecto_Fondos = DiccionarioCombosOYDPlusTodosInicio("CONCEPTODEFECTO_ORDENRECIBO_FONDOS").First.Descripcion
							End If
						End If
					End If

					If lo.UserState = "CANCELARREGISTRO" Then
						If Not IsNothing(TesoreriaOrdenesPlusAnterior) Then
							'TesoreriaOrdenesPlusRC_Selected = TesoreriaOrdenesPlusAnterior
						End If
					Else
						If logNuevoRegistro Or logEditarRegistro Then
							'HabilitarNegocio = False
							If Not IsNothing(TesoreriaOrdenesPlusAnterior) Then
								CargarParametrosReceptorOYDPLUS(TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor, lo.UserState)
							End If
						Else
							ObtenerValoresCombos(True)
						End If
					End If

				Else
					A2Utilidades.Mensajes.mostrarMensaje("Señor Usuario, usted no tiene receptores asociados.", "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					IsBusyReceptor = False
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
				IsBusyReceptor = False
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusyReceptor = False
			IsBusy = False
			IsBusyDetalles = False
		End Try
		IsBusyReceptor = False
	End Sub

	Sub TerminoTraerTesoreriaOrdenes(ByVal lo As LoadOperation(Of TesoreriaOrdenesEncabezado))
		Try
			IsBusy = True
			Dim objlista = New List(Of TesoreriaOrdenesEncabezado)

			If Not lo.HasError Then
				If logXTesorero = False Then
					If dcProxy.TesoreriaOrdenesEncabezados.Count > 0 Then
						If lo.UserState = "TERMINOGUARDAREDITAR" Or lo.UserState = "TERMINOGUARDARNUEVO" Or lo.UserState = "REFRESCARPANTALLA" Then
							logRefrescarDetalles = False
						Else
							logRefrescarDetalles = True
						End If

						ListaTesoreriaOrdenesPlusRC = dcProxy.TesoreriaOrdenesEncabezados.ToList

						If lo.UserState = "TERMINOGUARDAREDITAR" Then
							If ListaTesoreriaOrdenesPlusRC.Where(Function(i) i.lngID = intIDOrdenTesoreria).Count > 0 Then
								logRefrescarDetalles = True
								TesoreriaOrdenesPlusRC_Selected = ListaTesoreriaOrdenesPlusRC.Where(Function(i) i.lngID = intIDOrdenTesoreria).First
								ConsultarSaldo = False
								ConsultarSaldo = True
							End If
						ElseIf lo.UserState = "TERMINOGUARDARNUEVO" Then
							logRefrescarDetalles = True
							TesoreriaOrdenesPlusRC_Selected = ListaTesoreriaOrdenesPlusRC.FirstOrDefault
							ConsultarSaldo = False
							ConsultarSaldo = True

						ElseIf lo.UserState = "REFRESCARPANTALLA" Then
							If ListaTesoreriaOrdenesPlusRC.Where(Function(i) i.lngID = intIDOrdenTimer).Count > 0 Then
								logRefrescarDetalles = True
								TesoreriaOrdenesPlusRC_Selected = ListaTesoreriaOrdenesPlusRC.Where(Function(i) i.lngID = intIDOrdenTimer).First
								ConsultarSaldo = False
								ConsultarSaldo = True
							End If
						End If
						IsBusy = False
						ReiniciaTimer()
					Else
						IsBusy = False
						logRefrescarDetalles = True
						ListaTesoreriaOrdenesPlusRC = Nothing
						'mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					End If
				End If
			Else

				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Plus",
												 Me.ToString(), "TerminoTraerTesoreriaOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
				IsBusy = False
				IsBusyDetalles = False
			End If

		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			logRefrescarDetalles = True
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Plus",
															 Me.ToString(), "TerminoTraerTesoreriaOrdenes", Application.Current.ToString(), Program.Maquina, ex)
		End Try

		logRecargarPantalla = True
	End Sub

	Sub TerminoTraerTesoreriaOrdenesConsultar(ByVal lo As LoadOperation(Of TesoreriaOrdenesEncabezado))
		Try
			logBuscar = False
			If Not lo.HasError Then
				If dcProxy.TesoreriaOrdenesEncabezados.Count > 0 Then
					ListaTesoreriaOrdenesPlusRC = dcProxy.TesoreriaOrdenesEncabezados.ToList
					If logXTesorero Then
						TesoreriaOrdenesPlusRC_Selected = ListaTesoreriaOrdenesPlusRC.FirstOrDefault
						MyBase.CambiarFormulario_Forma_Manual()
					End If
				Else
					If logXTesorero = False Then
						ListaTesoreriaOrdenesPlusRC = Nothing
						TesoreriaOrdenesPlusRC_Selected = Nothing
						mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					Else
						ListaTesoreriaOrdenesPlusRC = Nothing
						TesoreriaOrdenesPlusRC_Selected = Nothing
						mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					End If
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Tesoreria Ordenes Plus",
												 Me.ToString(), "TerminoTraerTesoreriaOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
				lo.MarkErrorAsHandled()   '????
				IsBusy = False
				IsBusyDetalles = False
			End If
			IsBusy = False
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Tesoreria Ordenes Plus",
															 Me.ToString(), "TerminoTraerTesoreriaOrdenes", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Sub TerminoTraerTesoreriaOrdenesDetalleCargarPagosA(ByVal lo As LoadOperation(Of TesoreriaOyDPlusCargosPagosARecibo))
		Try
			If Not lo.HasError Then
				If dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Count > 0 Then
					ListatesoreriaordenesplusRC_Detalle_CargarPagosA = dcProxy.TesoreriaOyDPlusCargosPagosARecibos.ToList
					If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
						If dcProxy.TesoreriaOyDPlusCargosPagosARecibos.First.lngIDTesoreriaEncabezado <> TesoreriaOrdenesPlusRC_Selected.lngID Then
							ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Clear()
						End If
					Else
						ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Clear()
					End If

					If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
						TesoreriaordenesplusRC_Detalle_CargarPagosA_selected = ListatesoreriaordenesplusRC_Detalle_CargarPagosA.First
					Else
						TesoreriaordenesplusRC_Detalle_CargarPagosA_selected = Nothing
					End If

				Else
					'A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

				If lo.UserState = "CAMBIOSELECTED" Then
					dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_ConsignacionesReciboQuery(TesoreriaOrdenesPlusRC_Selected.lngID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleConsignaciones, lo.UserState)

				End If

				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					VerCargarPagosAFondos = Visibility.Visible
					VerCargarPagosA = Visibility.Collapsed
				Else
					VerCargarPagosAFondos = Visibility.Collapsed
					VerCargarPagosA = Visibility.Visible
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
					If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD Then
							TipoCuentasBancarias = "cuentasbancarias_tesorero"
						ElseIf _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							TipoCuentasBancarias = "cuentasbancariascartera"
						Else
							TipoCuentasBancarias = "cuentasbancariasPLUS"
						End If

					End If
				End If

				IsBusy = False
				IsBusyDetalles = False
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
												 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, lo.Error)
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
															 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Sub TerminoTraerTesoreriaOrdenesDetalleCargarPagosAFondos(ByVal lo As LoadOperation(Of TesoreriaOyDPlusCargosPagosAFondosRecibo))
		Try
			If Not lo.HasError Then
				If dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Count > 0 Then
					ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.ToList
					If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
						If dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.First.lngIDTesoreriaEncabezado <> TesoreriaOrdenesPlusRC_Selected.lngID Then
							ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Clear()
						End If
					Else
						ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Clear()
					End If

					If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
						TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.First
					Else
						TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected = Nothing
					End If


				Else
					'A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

				If lo.UserState = "CAMBIOSELECTED" Then
					dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_ConsignacionesReciboQuery(TesoreriaOrdenesPlusRC_Selected.lngID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleConsignaciones, lo.UserState)
				End If

				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					VerCargarPagosAFondos = Visibility.Visible
					VerCargarPagosA = Visibility.Collapsed
				Else
					VerCargarPagosAFondos = Visibility.Collapsed
					VerCargarPagosA = Visibility.Visible
				End If

				IsBusy = False
				IsBusyDetalles = False
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
												 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, lo.Error)
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
															 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Sub TerminoTraerTesoreriaOrdenesDetalleConsignaciones(ByVal lo As LoadOperation(Of TesoreriaOyDPlusConsignacionesRecibo))
		Try
			If Not lo.HasError Then
				If dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Count > 0 Then
					ListatesoreriaordenesplusRC_Detalle_Consignaciones = dcProxy.TesoreriaOyDPlusConsignacionesRecibos.ToList
					If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
						If dcProxy.TesoreriaOyDPlusConsignacionesRecibos.First.lngIDTesoreriaEncabezado <> TesoreriaOrdenesPlusRC_Selected.lngID Then
							ListatesoreriaordenesplusRC_Detalle_Consignaciones.Clear()
						End If
					Else
						ListatesoreriaordenesplusRC_Detalle_Consignaciones.Clear()
					End If

					If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > 0 Then
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected = ListatesoreriaordenesplusRC_Detalle_Consignaciones.First
					Else
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected = Nothing

					End If


				Else
					'A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

				If lo.UserState = "CAMBIOSELECTED" Then
					CalcularTotales(GSTR_CONSIGNACIONES)
					Habilitar_Encabezado()
				End If

				If logGuardandoOrden And logXTesorero = False Then
					RecorrerDetallesDocumento()
				Else
					IsBusy = False
					IsBusyDetalles = False
				End If

				logGuardandoOrden = False

				OrganizarDetallesRechazados()
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Consignaciones",
												 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleConsignaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus consignaciones",
															 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleConsignaciones", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Sub TerminoTraerTesoreriaOrdenesDetalleCheques(ByVal lo As LoadOperation(Of TesoreriaOyDPlusChequesRecibo))
		Try
			If Not lo.HasError Then
				If dcProxy.TesoreriaOyDPlusChequesRecibos.Count > 0 Then
					ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = dcProxy.TesoreriaOyDPlusChequesRecibos.ToList
					If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
						If dcProxy.TesoreriaOyDPlusChequesRecibos.First.lngIDTesoreriaEncabezado <> TesoreriaOrdenesPlusRC_Selected.lngID Then
							ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Clear()
						End If
					Else
						ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Clear()
					End If


					If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.First
					Else
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected = Nothing
					End If


				Else
					'A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

				If lo.UserState = "CAMBIOSELECTED" Then
					If TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_CargosPagosAFondos_ReciboQuery(TesoreriaOrdenesPlusRC_Selected.lngID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleCargarPagosAFondos, lo.UserState)
					Else
						dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_CargosPagosA_ReciboQuery(TesoreriaOrdenesPlusRC_Selected.lngID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleCargarPagosA, lo.UserState)
					End If

					CalcularTotales(GSTR_CHEQUE)
				End If

				IsBusy = False
				IsBusyDetalles = False
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
												 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, lo.Error)
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
															 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Sub TerminoTraerTesoreriaOrdenesDetalleTransferencias(ByVal lo As LoadOperation(Of TesoreriaOyDPlusTransferenciasRecibo))
		Try
			If Not lo.HasError Then
				If dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Count > 0 Then
					ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = dcProxy.TesoreriaOyDPlusTransferenciasRecibos.ToList
					If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
						If dcProxy.TesoreriaOyDPlusTransferenciasRecibos.First.lngIDTesoreriaEncabezado <> TesoreriaOrdenesPlusRC_Selected.lngID Then
							ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Clear()
						End If
					Else
						ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Clear()
					End If

					If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.First
					Else
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected = Nothing
					End If

				Else
					'A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

				If lo.UserState = "CAMBIOSELECTED" Then
					dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_Cheques_ReciboQuery(TesoreriaOrdenesPlusRC_Selected.lngID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleCheques, lo.UserState)
				End If

				CalcularTotales(GSTR_TRANSFERENCIA)
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Transferencias",
												 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleTransferencias", Application.Current.ToString(), Program.Maquina, lo.Error)
				lo.MarkErrorAsHandled()   '????
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesorería  Detalle Plus Transferencias",
															 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleTransferencias", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Private Sub TerminoEliminarEncabezado(lo As InvokeOperation(Of Integer))
		Try

			If Not lo.HasError Then
				If lo.Value > 0 Then

					ListaTesoreriaOrdenesPlusRC.Remove(TesoreriaOrdenesPlusRC_Selected)
					ListaTesoreriaOrdenesPlusRC = ListaTesoreriaOrdenesPlusRC
					MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC")
					TraerOrdenes("", VistaSeleccionada)
					IsBusy = False
					A2Utilidades.Mensajes.mostrarMensaje("El registro se anuló correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Personalizado)
				Else
					IsBusy = False
					A2Utilidades.Mensajes.mostrarMensaje("El registro no se pudo eliminar, verifique que el estado de la orden de tesorería y los detalles no estén en un estado donde no se permita eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Eliminar registro",
											Me.ToString(), "TerminoEliminarEncabezado", Application.Current.ToString(), Program.Maquina, lo.Error)
				IsBusyDetalles = False
			End If
			IsBusy = False
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Eliminación de la lista de Ordenes Tesorería Plus ",
															 Me.ToString(), "TerminoEliminarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
		End Try

	End Sub

	Private Sub TerminoEliminarDetalle(lo As InvokeOperation(Of Integer))
		Try

			If Not lo.HasError Then
				If lo.Value > 0 Then
					Select Case lo.UserState.ToString()
						Case Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarCheque.ToString
							If dcProxy.TesoreriaOyDPlusChequesRecibos.Where(Function(i) i.lngID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID).Count > 0 Then
								dcProxy.TesoreriaOyDPlusChequesRecibos.Remove(dcProxy.TesoreriaOyDPlusChequesRecibos.Where(Function(i) i.lngID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID).First)
							End If

							ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Remove(TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected)
							MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_Cheques")
							If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
								TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.FirstOrDefault
							Else
								TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected = Nothing
							End If

							CalcularTotales(GSTR_CHEQUE)
						Case Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarTransferencia.ToString

							If dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Where(Function(i) i.lngID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID).Count > 0 Then
								dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Remove(dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Where(Function(i) i.lngID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID).First)
							End If

							ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Remove(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected)
							MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias")
							If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
								TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.FirstOrDefault
							Else
								TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected = Nothing
							End If

							'A2Utilidades.Mensajes.mostrarMensaje("El registro se Eliminó correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Personalizado)
							CalcularTotales(GSTR_TRANSFERENCIA)
						Case Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarConsignaciones.ToString
							If dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Where(Function(i) i.lngID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID).Count > 0 Then
								dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Remove(dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Where(Function(i) i.lngID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID).First)
							End If

							ListatesoreriaordenesplusRC_Detalle_Consignaciones.Remove(TesoreriaordenesplusRC_Detalle_Consignaciones_selected)
							MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_Consignaciones")
							If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > 0 Then
								TesoreriaordenesplusRC_Detalle_Consignaciones_selected = ListatesoreriaordenesplusRC_Detalle_Consignaciones.FirstOrDefault
							Else
								TesoreriaordenesplusRC_Detalle_Consignaciones_selected = Nothing
							End If

							'A2Utilidades.Mensajes.mostrarMensaje("El registro se Eliminó correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Personalizado)
							CalcularTotales(GSTR_CONSIGNACIONES)
						Case Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarCargarPagosA.ToString
							If dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Where(Function(i) i.lngID = TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngID).Count > 0 Then
								dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Remove(dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Where(Function(i) i.lngID = TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngID).First)
							End If

							ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Remove(TesoreriaordenesplusRC_Detalle_CargarPagosA_selected)
							MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_CargarPagosA")
							If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
								TesoreriaordenesplusRC_Detalle_CargarPagosA_selected = ListatesoreriaordenesplusRC_Detalle_CargarPagosA.FirstOrDefault
							Else
								TesoreriaordenesplusRC_Detalle_CargarPagosA_selected = Nothing
							End If

							CalcularValorDisponibleCargar()
							'A2Utilidades.Mensajes.mostrarMensaje("El registro se Eliminó correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Personalizado)
							CalcularTotales("")
						Case Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarCargarPagosAFondos.ToString
							If dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Where(Function(i) i.lngID = TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID).Count > 0 Then
								dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Remove(dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Where(Function(i) i.lngID = TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID).First)
							End If

							ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Remove(TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected)
							MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos")
							If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
								TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.FirstOrDefault
							Else
								TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected = Nothing
							End If

							CalcularValorDisponibleCargar()
							'A2Utilidades.Mensajes.mostrarMensaje("El registro se Eliminó correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Personalizado)
							CalcularTotales("")
					End Select


					CalcularValorDisponibleCargar()
					Habilitar_Encabezado()
				Else
					A2Utilidades.Mensajes.mostrarMensaje("El registro no se puede eliminar, verifique que este en un estado en que se pueda eliminar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Eliminar registro del Detalle",
											Me.ToString(), "TerminoEliminarDetalle", Application.Current.ToString(), Program.Maquina, lo.Error)
				IsBusyDetalles = False
			End If

			'lo.MarkErrorAsHandled()   '????

			IsBusy = False
		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Eliminación de la lista de Ordenes Tesorería  Detalle Plus ",
															 Me.ToString(), "TerminoEliminarDetalle", Application.Current.ToString(), Program.Maquina, ex)
		End Try

	End Sub

	Private Sub TerminoPreguntarConfirmacionConsignaciones(ByVal sender As Object, ByVal e As EventArgs)
		Try
			IsBusy = True
			Dim logCheque As Boolean = False
			Dim logTransferencia As Boolean = False
			Dim objListaTesoreriaOrdenesPlusRC_Detalle_Consignaciones = New List(Of TesoreriaOyDPlusConsignacionesRecibo)



			If IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
				logCheque = True
			ElseIf ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count <= 0 Then
				logCheque = True
			End If

			If IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
				logTransferencia = True
			ElseIf ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count <= 0 Then
				logTransferencia = True
			End If

			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count <> 0 And Not TesoreriaordenesplusRC_Detalle_Consignaciones_selected Is Nothing Then
					If TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID <= 0 Or TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID Is Nothing Then

						'Elimina el detalle del recibo siempre y cuando exista.
						If Not IsNothing(_ListaArchivoConsignacion) Then
							If _ListaArchivoConsignacion.Where(Function(i) i.ID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle).Count > 0 Then
								_ListaArchivoConsignacion.Remove(_ListaArchivoConsignacion.Where(Function(i) i.ID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle).First)
								Dim intIDContador As Integer = 1
								For Each li In _ListaArchivoConsignacion
									li.ID = intIDContador
									intIDContador += 1
								Next
							End If
						End If

						ListatesoreriaordenesplusRC_Detalle_Consignaciones.Remove(TesoreriaordenesplusRC_Detalle_Consignaciones_selected)
						objListaTesoreriaOrdenesPlusRC_Detalle_Consignaciones = ListatesoreriaordenesplusRC_Detalle_Consignaciones
						ListatesoreriaordenesplusRC_Detalle_Consignaciones = Nothing
						ListatesoreriaordenesplusRC_Detalle_Consignaciones = objListaTesoreriaOrdenesPlusRC_Detalle_Consignaciones

						If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > 0 Then
							TesoreriaordenesplusRC_Detalle_Consignaciones_selected = ListatesoreriaordenesplusRC_Detalle_Consignaciones.FirstOrDefault
						End If

						IsBusy = False
						Habilitar_Encabezado()
						CalcularTotales(GSTR_CONSIGNACIONES)
					Else
						dcProxy.TesoreriaOrdenesDetalle_ValidarEstado(TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDTesoreriaEncabezado,
																				 TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID,
																				 GSTR_CONSIGNACIONES,
																		 Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarDetalle, Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarConsignaciones.ToString)
					End If

				End If


			Else
				IsBusy = False
			End If


		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacionConsignaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadConfirmaciones = 0
		End Try
	End Sub

	Private Sub TerminoPreguntarConfirmacionTransferencias(ByVal sender As Object, ByVal e As EventArgs)
		Try
			IsBusy = True
			Dim logCheque As Boolean = False
			Dim logConsignaciones As Boolean = False
			Dim objListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = New List(Of TesoreriaOyDPlusTransferenciasRecibo)


			If IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
				logCheque = True
			ElseIf ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count <= 0 Then
				logCheque = True
			End If
			If IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
				logConsignaciones = True
			ElseIf ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count <= 0 Then
				logConsignaciones = True
			End If

			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count <> 0 And Not TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected Is Nothing Then
					If TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID <= 0 Or TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID Is Nothing Then

						'Elimina el detalle del recibo siempre y cuando exista.
						If Not IsNothing(_ListaArchivoTransferencia) Then
							If _ListaArchivoTransferencia.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle).Count > 0 Then
								_ListaArchivoTransferencia.Remove(_ListaArchivoTransferencia.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle).First)
								Dim intIDContador As Integer = 1
								For Each li In _ListaArchivoTransferencia
									li.ID = intIDContador
									intIDContador += 1
								Next
							End If
						End If

						ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Remove(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected)
						objListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
						ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = Nothing
						ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = objListaTesoreriaOrdenesPlusRC_Detalle_Transferencias

						If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
							TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.FirstOrDefault
						End If

						IsBusy = False
						Habilitar_Encabezado()
						CalcularTotales(GSTR_TRANSFERENCIA)
					Else
						dcProxy.TesoreriaOrdenesDetalle_ValidarEstado(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDTesoreriaEncabezado,
																				 TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID,
																				 GSTR_TRANSFERENCIA,
																		 Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarDetalle, Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarTransferencia.ToString)

					End If

				End If


			Else
				IsBusy = False
			End If


		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacionTransferencias", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadConfirmaciones = 0
		End Try
	End Sub

	Private Sub TerminoPreguntarConfirmacionCargarPagosA(ByVal sender As Object, ByVal e As EventArgs)
		Try
			IsBusy = True

			Dim objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosA = New List(Of TesoreriaOyDPlusCargosPagosARecibo)



			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count <> 0 And Not ListatesoreriaordenesplusRC_Detalle_CargarPagosA Is Nothing Then
					If TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngID = 0 Or TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngID Is Nothing Then

						ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Remove(TesoreriaordenesplusRC_Detalle_CargarPagosA_selected)
						objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosA = ListatesoreriaordenesplusRC_Detalle_CargarPagosA
						ListatesoreriaordenesplusRC_Detalle_CargarPagosA = Nothing
						ListatesoreriaordenesplusRC_Detalle_CargarPagosA = objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosA

						If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
							_TesoreriaordenesplusRC_Detalle_CargarPagosA_selected = ListatesoreriaordenesplusRC_Detalle_CargarPagosA.FirstOrDefault
						End If

						CalcularValorDisponibleCargar()
						IsBusy = False
						Habilitar_Encabezado()
					Else
						dcProxy.TesoreriaOrdenesDetalle_ValidarEstado(TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngIDTesoreriaEncabezado,
																			 TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngID,
																			 GSTR_ORDENRECIBO_CARGOPAGOA,
																	 Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarDetalle, Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarCargarPagosA.ToString)
					End If

				End If


			Else
				IsBusy = False
			End If


		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacionCargarPagosA", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadConfirmaciones = 0
		End Try
	End Sub

	Private Sub TerminoPreguntarConfirmacionCargarPagosAFondos(ByVal sender As Object, ByVal e As EventArgs)
		Try
			IsBusy = True

			Dim objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosAFondos = New List(Of TesoreriaOyDPlusCargosPagosAFondosRecibo)

			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count <> 0 And Not ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos Is Nothing Then
					If TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID = 0 Or TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID Is Nothing Then

						ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Remove(TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected)
						objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosAFondos = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
						ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = Nothing
						ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosAFondos

						If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
							TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.FirstOrDefault
						End If

						CalcularTotales("")
						CalcularValorDisponibleCargar()
						IsBusy = False
						Habilitar_Encabezado()
					Else
						dcProxy.TesoreriaOrdenesDetalle_ValidarEstado(TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngIDTesoreriaEncabezado,
																TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID,
																GSTR_ORDENRECIBO_CARGOPAGOAFONDOS,
																Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarDetalle, Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarCargarPagosAFondos.ToString)
					End If
				End If
			Else
				IsBusy = False
			End If


		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacionCargarPagosA", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadConfirmaciones = 0
		End Try
	End Sub

	Private Sub TerminoPreguntarConfirmacionCheque(ByVal sender As Object, ByVal e As EventArgs)
		Try
			IsBusy = True
			Dim logTransferencia As Boolean = False
			Dim logConsignacion As Boolean = False
			Dim objListaTesoreriaOrdenesPlusRC_Detalle_Cheques = New List(Of TesoreriaOyDPlusChequesRecibo)


			If IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
				logTransferencia = True
			ElseIf ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count <= 0 Then
				logTransferencia = True
			End If


			If IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
				logConsignacion = True
			ElseIf ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count <= 0 Then
				logConsignacion = True
			End If

			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count <> 0 And Not TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected Is Nothing Then
					If TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID <= 0 Or TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID Is Nothing Then
						'Elimina el detalle del recibo siempre y cuando exista.
						If Not IsNothing(_ListaArchivosCheque) Then
							If _ListaArchivosCheque.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle).Count > 0 Then
								_ListaArchivosCheque.Remove(_ListaArchivosCheque.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle).First)
								Dim intIDContador As Integer = 1
								For Each li In _ListaArchivosCheque
									li.ID = intIDContador
									intIDContador += 1
								Next
							End If
						End If

						ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Remove(TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected)
						objListaTesoreriaOrdenesPlusRC_Detalle_Cheques = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
						ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = Nothing
						ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = objListaTesoreriaOrdenesPlusRC_Detalle_Cheques

						If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
							TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.FirstOrDefault
						End If

						IsBusy = False
						Habilitar_Encabezado()
						CalcularTotales(GSTR_CHEQUE)
					Else
						dcProxy.TesoreriaOrdenesDetalle_ValidarEstado(TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDTesoreriaEncabezado,
																				 TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID,
																				 GSTR_CHEQUE,
																		 Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarDetalle, Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarCheque.ToString)
					End If

				End If


			Else
				IsBusy = False
			End If


		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadConfirmaciones = 0
		End Try
	End Sub

	Private Sub TerminoPreguntarConfirmacion(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				CantidadConfirmaciones = CantidadConfirmaciones + 1
				TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTE_Plus
			Else
				IsBusy = False
				LimpiarVariablesConfirmadas()
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadConfirmaciones = 0
		End Try
	End Sub

	Private Sub TerminoPreguntarJustificacion(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				If String.IsNullOrEmpty(Justificaciones) Then
					Justificaciones = objResultado.CodConfirmacion
					JustificacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'**'{3}'", objResultado.NombreRegla, objResultado.CodRegla, objResultado.MensajeRegla, String.Format("{0}++{1}", objResultado.Observaciones, objResultado.TextoConfirmacion.Replace("|", "++")))
				Else
					Justificaciones = String.Format("{0}|{1}", Justificaciones, objResultado.CodConfirmacion)
					JustificacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'", JustificacionesUsuario, objResultado.NombreRegla, objResultado.CodRegla, objResultado.MensajeRegla, String.Format("{0}++{1}", objResultado.Observaciones, objResultado.TextoConfirmacion.Replace("|", "++")))
				End If

				Dim ListaJustificacion As New List(Of A2Utilidades.CausasJustificacionMensajePregunta)
				Dim logEsHtml As Boolean = False
				Dim strMensajeDetallesHtml As String = String.Empty
				Dim strMensajeRetornoHtml As String = String.Empty

				For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereJustificacion = True).ToList
					If Not Justificaciones.Contains(li.Confirmacion) Then

						If Not String.IsNullOrEmpty(li.DetalleRegla) Then
							logEsHtml = True
							strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", li.DetalleRegla)
						Else
							logEsHtml = False
							strMensajeDetallesHtml = String.Empty
						End If

						mostrarMensajePregunta(li.Mensaje,
											   Program.TituloSistema,
											   "PREGUNTARJUSTIFICACION",
											   AddressOf TerminoPreguntarJustificacion,
											   True,
											   "¿Desea continuar?",
											   IIf(ListaJustificacion.Count > 0, True, False),
											   True,
											   IIf(ListaJustificacion.Count > 0, False, True),
											   IIf(ListaJustificacion.Count = 0, False, True),
											   li.Confirmacion,
											   String.Empty,
											   String.Empty,
											   IIf(ListaJustificacion.Count > 0, ListaJustificacion, Nothing),
											   "Reglas incumplidas en los detalles de las ordenes de recaudo",
											   logEsHtml,
											   strMensajeDetallesHtml)
						Exit For
					End If
				Next

				CantidadJustificaciones = CantidadJustificaciones + 1
				TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTE_Plus
			Else
				IsBusy = False
				LimpiarVariablesConfirmadas()
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarJustificacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadJustificaciones = 0
		End Try
	End Sub

	Private Sub TerminoCargarArchivoRecibos(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
		Try
			If lo.HasError = False Then
				Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
				Dim objListaMensajes As New List(Of String)

				objListaRespuesta = lo.Entities.ToList

				If objListaRespuesta.Count > 0 Then
					If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
						Dim objTipo As String = String.Empty
						objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")
						For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False).OrderBy(Function(o) o.Tipo)
							If objTipo <> li.Tipo Then
								objTipo = li.Tipo
								objListaMensajes.Add(String.Format("Hoja {0}", li.Tipo))
							End If

							objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
						Next

						ViewImportarArchivo.ListaMensajes = objListaMensajes
						ViewImportarArchivo.IsBusy = False
					Else
						Dim objProceso As Double = 0

						For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
							objListaMensajes.Add(li.Mensaje)
							objProceso = li.NroProceso
						Next

						If objProceso <> 0 Then

							ViewImportarArchivo.ListaMensajes = objListaMensajes

							If Not IsNothing(dcProxyImportaciones.InformacionArchivoRecibos) Then
								dcProxyImportaciones.InformacionArchivoRecibos.Clear()
							End If

							dcProxyImportaciones.Load(dcProxyImportaciones.ObtenerValoresArchivoImportacionQuery(objProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoObtenerArchivoRecibos, String.Empty)
						Else
							ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
							ViewImportarArchivo.IsBusy = False
						End If

					End If
				Else
					ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
					ViewImportarArchivo.IsBusy = False
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de recibos.", Me.ToString(), "TerminoCargarArchivoRecibos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
				ViewImportarArchivo.IsBusy = False
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de recibos.", Me.ToString(), "TerminoCargarArchivoRecibos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			ViewImportarArchivo.IsBusy = False
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub TerminoObtenerArchivoRecibos(ByVal lo As LoadOperation(Of OyDImportaciones.InformacionArchivoRecibos))
		Try
			If lo.HasError = False Then
				If lo.Entities.Count > 0 Then
					SubirInformacionArchivoRecibo(lo.Entities.ToList)
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la información el archivo de carga de recibos.", Me.ToString(), "TerminoObtenerArchivoRecibos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
				ViewImportarArchivo.IsBusy = False
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la información el archivo de carga de recibos.", Me.ToString(), "TerminoObtenerArchivoRecibos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			ViewImportarArchivo.IsBusy = False
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub TerminoValidarIngresoOrden(ByVal lo As LoadOperation(Of OyDPLUSTesoreria.tblRespuestaValidaciones))
		Try

			If lo.HasError = False Then
				ListaResultadoValidacion = lo.Entities.ToList

				IsBusy = False
				If ListaResultadoValidacion.Count > 0 Then
					Dim logExitoso As Boolean = False
					Dim logRequiereConfirmacion As Boolean = False
					Dim logRequiereJustificacion As Boolean = False
					Dim logRequiereAprobacion As Boolean = False
					Dim logConsultaListaJustificacion As Boolean = False
					Dim logError As Boolean = False
					Dim strMensajeExitoso As String = String.Empty
					Dim strMensajeError As String = String.Empty
					Dim logEsHtml As Boolean = False
					Dim strMensajeDetallesHtml As String = String.Empty
					Dim strMensajeRetornoHtml As String = String.Empty
					Dim intIDRegistroInsertado As Integer = 0

					For Each li In ListaResultadoValidacion
						If li.Exitoso Then
							logExitoso = True
							logError = False
							logRequiereJustificacion = False
							logRequiereConfirmacion = False
							logRequiereAprobacion = False
							strMensajeExitoso = strMensajeExitoso & vbCrLf & li.Mensaje
							intIDRegistroInsertado = li.IDOrdenIdentity
						ElseIf li.RequiereConfirmacion Then
							logExitoso = False
							logError = False
							logRequiereConfirmacion = True
						ElseIf li.RequiereJustificacion Then
							logExitoso = False
							logError = False
							logRequiereJustificacion = True
						ElseIf li.RequiereAprobacion Then
							logExitoso = False
							logError = False
							logRequiereAprobacion = True
						ElseIf li.DetieneIngreso Then
							logError = True
							logExitoso = False
							logRequiereJustificacion = False
							logRequiereConfirmacion = False
							logRequiereAprobacion = False
							strMensajeError = String.Format("{0}{1}{2}", strMensajeError, vbCrLf, li.Mensaje)
							strMensajeRetornoHtml = String.Format("{0}{1}", strMensajeRetornoHtml, li.DetalleRegla)
						Else
							logError = True
							logExitoso = False
							logRequiereJustificacion = False
							logRequiereConfirmacion = False
							logRequiereAprobacion = False
							strMensajeError = String.Format("{0}{1}{2}", strMensajeError, vbCrLf, li.Mensaje)
							strMensajeRetornoHtml = String.Format("{0}{1}", strMensajeRetornoHtml, li.DetalleRegla)
						End If

					Next

					If logExitoso And
						logRequiereConfirmacion = False And
						logRequiereJustificacion = False And
						logRequiereAprobacion = False And
						logError = False Then

						ActualizarOrdenTesoreria(intIDRegistroInsertado)

					ElseIf logError Then
						If Not String.IsNullOrEmpty(strMensajeRetornoHtml) Then
							logEsHtml = True
							strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", strMensajeRetornoHtml)
						Else
							logEsHtml = False
							strMensajeDetallesHtml = String.Empty
						End If

						strMensajeError = String.Format("La orden no fue posible actualizarla: {0}{1}", vbCrLf, strMensajeError)
						strMensajeError = Replace(strMensajeError, "-", String.Format("{0}", vbCrLf))
						mostrarMensaje(strMensajeError, "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia, String.Empty, "Reglas incumplidas en los detalles de las ordenes de recaudo", logEsHtml, strMensajeDetallesHtml)
						IsBusy = False
					Else
						If logRequiereConfirmacion Then
							CantidadConfirmaciones = 0
						End If

						If logRequiereJustificacion Then
							CantidadJustificaciones = 0
							cantidadTotalJustificacion = ListaResultadoValidacion.Where(Function(i) i.RequiereJustificacion = True).Count
						End If

						If logRequiereAprobacion Then
							CantidadAprobaciones = 0
							CantidadTotalAprobaciones = ListaResultadoValidacion.Where(Function(i) i.RequiereAprobacion = True).Count
						End If

						If logRequiereConfirmacion Then
							Dim MensajeConfirmacion As String = String.Empty

							If ListaResultadoValidacion.Where(Function(i) i.RequiereConfirmacion = True).Count > 0 Then
								cantidadTotalConfirmacion = 1
							End If

							strMensajeRetornoHtml = String.Empty

							For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereConfirmacion = True).ToList
								If String.IsNullOrEmpty(Confirmaciones) Then
									Confirmaciones = String.Format("'{0}'", li.Confirmacion)
									ConfirmacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'", li.NombreRegla, li.Regla, li.Mensaje)
									MensajeConfirmacion = String.Format("{0}", li.Mensaje)
								Else
									Confirmaciones = String.Format("{0}|'{1}'", Confirmaciones, li.Confirmacion)
									ConfirmacionesUsuario = String.Format("'{0}'|'{1}'**'{2}'**'{3}'", ConfirmacionesUsuario, li.NombreRegla, li.Regla, li.Mensaje)
									MensajeConfirmacion = String.Format("{0}{2}{1}", MensajeConfirmacion, vbCrLf, li.Mensaje)
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

							mostrarMensajePregunta(MensajeConfirmacion,
												   Program.TituloSistema,
												   "PREGUNTARCONFIRMACION",
												   AddressOf TerminoPreguntarConfirmacion,
												   True,
												   "¿Desea continuar?",
												   False,
												   False,
												   True,
												   True,
												   Confirmaciones,
												   String.Empty,
												   String.Empty,
												   Nothing,
												   "Reglas incumplidas en los detalles de las ordenes de recaudo",
												   logEsHtml,
												   strMensajeDetallesHtml)
						End If

						If logRequiereJustificacion Then
							Dim ListaJustificacion As New List(Of A2Utilidades.CausasJustificacionMensajePregunta)

							For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereJustificacion = True).ToList
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

								mostrarMensajePregunta(li.Mensaje,
														Program.TituloSistema,
														"PREGUNTARJUSTIFICACION",
														AddressOf TerminoPreguntarJustificacion,
														True,
														"¿Desea continuar?",
														IIf(ListaJustificacion.Count > 0, True, False),
														True,
														IIf(ListaJustificacion.Count > 0, False, True),
														IIf(ListaJustificacion.Count = 0, False, True),
														li.Confirmacion,
														String.Empty,
														String.Empty,
														IIf(ListaJustificacion.Count > 0, ListaJustificacion, Nothing),
														"Reglas incumplidas en los detalles de las ordenes de recaudo",
														logEsHtml,
														strMensajeDetallesHtml)
								Exit For
							Next
						End If

						If logRequiereAprobacion Then
							Dim ListaJustificacion As New List(Of A2Utilidades.CausasJustificacionMensajePregunta)

							For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereAprobacion = True).ToList
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

								mostrarMensajePregunta(li.Mensaje,
													   Program.TituloSistema,
													   "PREGUNTARAPROBACION",
													   AddressOf TerminoPreguntarAprobacion,
													   True,
													   "¿Desea continuar?",
													   IIf(ListaJustificacion.Count > 0, True, False),
													   True,
													   IIf(ListaJustificacion.Count > 0, False, True),
													   IIf(ListaJustificacion.Count = 0, False, True),
													   li.Confirmacion,
													   li.Regla,
													   li.NombreRegla,
													   IIf(ListaJustificacion.Count > 0, ListaJustificacion, Nothing),
													   "Reglas incumplidas en los detalles de las ordenes de recaudo",
													   logEsHtml,
													   strMensajeDetallesHtml)

								Exit For
							Next
						End If
					End If
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
				IsBusy = False
				IsBusyDetalles = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub TerminoObtenerRespuestaPregunta(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then
				If String.IsNullOrEmpty(Justificaciones) Then
					Justificaciones = objResultado.CodConfirmacion
					JustificacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'**'{3}'", objResultado.NombreRegla, objResultado.CodRegla, objResultado.MensajeRegla, String.Format("{0}++{1}", objResultado.Observaciones, objResultado.TextoConfirmacion.Replace("|", "++")))
				Else
					Justificaciones = String.Format("{0}|{1}", Justificaciones, objResultado.CodConfirmacion)
					JustificacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'", JustificacionesUsuario, objResultado.NombreRegla, objResultado.CodRegla, objResultado.MensajeRegla, String.Format("{0}++{1}", objResultado.Observaciones, objResultado.TextoConfirmacion.Replace("|", "++")))
				End If

				Dim ListaJustificacion As New List(Of A2Utilidades.CausasJustificacionMensajePregunta)
				For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereJustificacion = True).ToList
					If Not Justificaciones.Contains(li.Confirmacion) Then

						mostrarMensajePregunta(li.Mensaje,
											   Program.TituloSistema,
											   "PREGUNTARJUSTIFICACION",
											   AddressOf TerminoPreguntarJustificacion,
											   True,
											   "¿Desea continuar?",
											   IIf(ListaJustificacion.Count > 0, True, False),
											   True,
											   IIf(ListaJustificacion.Count > 0, False, True),
											   IIf(ListaJustificacion.Count = 0, False, True),
											   li.Confirmacion,
											   String.Empty,
											   String.Empty,
											   IIf(ListaJustificacion.Count > 0, ListaJustificacion, Nothing))
						Exit For
					End If
				Next

				CantidadJustificaciones = CantidadJustificaciones + 1
				TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTE_Plus
			Else
				IsBusy = False
				LimpiarVariablesConfirmadas()
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarJustificacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			CantidadJustificaciones = 0
		End Try
	End Sub

	Private Sub TerminoTraerCombosEspecificosCartera(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
		Try
			If Not lo.HasError Then
				Dim objListaTipoAccionFondos As New List(Of OYDUtilidades.ItemCombo)

				If lo.Entities.ToList.Where(Function(i) i.Categoria = "TIPOACCIONFONDOS").Count > 0 Then
					objListaTipoAccionFondos = lo.Entities.ToList.Where(Function(i) i.Categoria = "TIPOACCIONFONDOS").ToList
				End If

				If lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_ADICION").Count > 0 Then
					ConceptoDefectoFondos_Adicion = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_ADICION").First.intID
					ConceptoDescripcionDefectoFondos_Adicion = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_ADICION").First.Descripcion
				Else
					ConceptoDefectoFondos_Adicion = Nothing
					ConceptoDescripcionDefectoFondos_Adicion = Nothing
				End If

				If lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_APERTURA").Count > 0 Then
					ConceptoDefectoFondos_Apertura = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_APERTURA").First.intID
					ConceptoDescripcionDefectoFondos_Apertura = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_APERTURA").First.Descripcion
				Else
					ConceptoDefectoFondos_Apertura = Nothing
					ConceptoDescripcionDefectoFondos_Apertura = Nothing
				End If

				ListaTiposAccionFondos = objListaTipoAccionFondos

				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					If lo.Entities.ToList.Where(Function(i) i.Categoria = "ADICIONES_DIASAPLICACION").Count > 0 Then
						intDiasAplicacionFondosAdicion = CInt(lo.Entities.ToList.Where(Function(i) i.Categoria = "ADICIONES_DIASAPLICACION").First.ID)
					End If
					If lo.Entities.ToList.Where(Function(i) i.Categoria = "APERTURAS_DIASAPLICACION").Count > 0 Then
						intDiasAplicacionFondosApertura = CInt(lo.Entities.ToList.Where(Function(i) i.Categoria = "APERTURAS_DIASAPLICACION").First.ID)
					End If
				End If

				If TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
					If FechaAplicacion.Value.Date < DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, FechaOrden.Date) Then
						mostrarMensaje(String.Format("La fecha de aplicación no puede ser menor a la fecha de orden + los días de aplicación {0:yyyy-MM-dd}", DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, FechaOrden.Date)), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					End If
				ElseIf TipoAccionFondos = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
					If FechaAplicacion.Value.Date < DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, FechaOrden.Date) Then
						mostrarMensaje(String.Format("La fecha de aplicación no puede ser menor a la fecha de orden + los días de aplicación {0:yyyy-MM-dd}", DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, FechaOrden.Date)), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					End If
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos de la cartera",
												 Me.ToString(), "TerminoTraerCombosEspecificosCartera", Program.TituloSistema, Program.Maquina, lo.Error)
				'lo.MarkErrorAsHandled()   '????
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos de la cartera",
												 Me.ToString(), "TerminoTraerCombosEspecificosCartera", Program.TituloSistema, Program.Maquina, ex)
		End Try


	End Sub

#End Region

#Region "Metodos"

	Public Sub CargarInformacionCuentaRegistrada(ByVal pstrCuenta As String,
												 ByVal pstrTipoCuenta As String,
												 ByVal pstrCodigoTipoCuenta As String,
												 ByVal pintIDBanco As Integer,
												 ByVal pstrNombreTitular As String,
												 ByVal pstrNombreBanco As String,
												 ByVal pstrTipoDocumento As String,
												 ByVal pstrCodigoTipoDocumento As String,
												 ByVal pstrNroDocumento As String,
												 ByVal plogDividendos As Boolean,
												 ByVal plogOperacion As Boolean,
												 ByVal plogTransferir As Boolean,
												 ByVal plogUnicoTitular As Boolean)
		Try
			strNroCuentaWpp = pstrCuenta
			DescripcionTipoCuentaTransferencia = pstrTipoCuenta
			strValorTipoCuentaWpp = pstrCodigoTipoCuenta
			lngCodigoBancoWpp = pintIDBanco
			DescripcionBancoTransferencia = pintIDBanco.ToString & " - " & pstrNombreBanco

			HabilitarComboCuentasNombreTitular = False
			HabilitarComboCuentasCliente = False
			HabilitarComboCuentasNroDocumentoTitular = False
			HabilitarComboCuentasNroCuenta = False
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar la cuenta registrada.",
												 Me.ToString(), "CargarInformacionCuentaRegistrada", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	Public Sub CerrarPopup()
		Try
			If Not IsNothing(objViewOrdenReciboPopPup) Then
				objViewOrdenReciboPopPup.Close()
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la pantalla", Me.ToString(), "CerrarPopup",
														 Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub configurarDocumentos(obj As A2DocumentosWPF.A2SubirDocumento, lngIDDetalle As Integer, strOpcionDetalle As String)
		Try
			obj.URLServicioDocumentos = GSTR_SERVICIO_DOCUMENTOS
			obj.Aplicacion = Program.Aplicacion
			obj.Version = Program.VersionAplicacion
			obj.SoloCapturarRutaArchivo = "True"
			obj.AnchoNombreArchivo = "200"
			obj.Tema = "OrdenesRecibo"

			If strOpcionDetalle = GSTR_CHEQUE Then
				obj.Modulo = "OR\CH"
				obj.Subtema = "Cheque"
			ElseIf strOpcionDetalle = GSTR_TRANSFERENCIA Then
				obj.Modulo = "OR\TR"
				obj.Subtema = "Transferencia"
			ElseIf strOpcionDetalle = GSTR_CONSIGNACIONES Then
				obj.Modulo = "OR\CO"
				obj.Subtema = "Consignacion"
			Else
				obj.Subtema = ""
			End If

			obj.ClaveUnica = lngIDDetalle
			obj.TagsBusqueda = "Documentos Escaneados, orden recaudo"
			obj.Titulo = "Documentos escaneados de orden de recaudo"
			obj.Descripcion = "Servicio documentos Ordenes de recaudo OyD Plus"
			obj.UsuarioActivo = Program.Usuario
			obj.FiltroArchivos = "Todos los archivos (*.*)|*.*|Texto (*.txt)|*.txt|Excel 2010 (*.xlsx)|*.xlsx|Word 2010 (*.docx)|*.docx|Excel 2003 (.xls)|*.xls|Word 2003 (*.doc)|*.doc"
			obj.MostrarLog = "False"
			obj.TituloSistema = "Ordenes de Recaudo"
			obj.TextoBotonSubirArchivo = "Cargar archivo"

			Dim logMensaje As Boolean = False
			If Not String.IsNullOrEmpty(Program.MostrarMensajeLog) Then
				If Program.MostrarMensajeLog = "1" Then
					logMensaje = True
				End If
			End If

			obj.MostrarMensajeFinalizacion = logMensaje
			obj.MostrarNombreArchivo = False
			obj.MostrarLog = logMensaje

			TextoCampoSubirArchivoDetalle(True)
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al configurar la subida de archivo.", Me.ToString(), "configurarDocumentos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub RecorrerDetallesDocumento()
		Try
			intCantidadDocumentosSubir = 0
			intCantidadDocumentosSubidos = 0

			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) And Not IsNothing(_ListaArchivosCheque) Then
				intCantidadDocumentosSubir = intCantidadDocumentosSubir + _ListaArchivosCheque.Count
			End If
			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) And Not IsNothing(_ListaArchivoTransferencia) Then
				intCantidadDocumentosSubir = intCantidadDocumentosSubir + _ListaArchivoTransferencia.Count
			End If
			If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_Consignaciones) And Not IsNothing(_ListaArchivoConsignacion) Then
				intCantidadDocumentosSubir = intCantidadDocumentosSubir + _ListaArchivoConsignacion.Count
			End If

			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) And Not IsNothing(_ListaArchivosCheque) Then
				For Each itemDetalle In _ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
					If Not IsNothing(itemDetalle.IDTempNuevoRegistro) Then
						If _ListaArchivosCheque.Where(Function(i) i.ID = itemDetalle.IDTempNuevoRegistro And i.IDDetalle = 0).Count > 0 Then
							_ListaArchivosCheque.Where(Function(i) i.ID = itemDetalle.IDTempNuevoRegistro And i.IDDetalle = 0).First.IDDetalle = itemDetalle.lngIDDetalle
						End If
					Else
						If _ListaArchivosCheque.Where(Function(i) i.ID = itemDetalle.lngIDDetalle And i.IDDetalle = 0).Count > 0 Then
							_ListaArchivosCheque.Where(Function(i) i.ID = itemDetalle.lngIDDetalle And i.IDDetalle = 0).First.IDDetalle = itemDetalle.lngIDDetalle
						End If
					End If
				Next

				For Each li In _ListaArchivosCheque
					Dim objWppChequeRC As New wppFrmCheque_RC(Me)
					objWppChequeRC.ctlSubirArchivo.inicializarControl()
					AddHandler objWppChequeRC.ctlSubirArchivo.finalizoTransmisionArchivo, AddressOf TerminoTransmisionArchivo
					AddHandler objWppChequeRC.ctlSubirArchivo.errorTransmisionArchivo, AddressOf TerminoTransmisionArchivoError
					configurarDocumentos(objWppChequeRC.ctlSubirArchivo, li.IDDetalle, GSTR_CHEQUE)
					objWppChequeRC.ctlSubirArchivo.ClaveUnica = li.IDDetalle
					If Not String.IsNullOrEmpty(li.RutaArchivo) Then
						objWppChequeRC.ctlSubirArchivo.subirArchivo(li.NombreArchivo, li.RutaArchivo)
					Else
						objWppChequeRC.ctlSubirArchivo.subirArchivo(li.NombreArchivo, li.ByteArchivo)
					End If
				Next
			End If

			If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) And Not IsNothing(_ListaArchivoTransferencia) Then
				For Each itemDetalle In _ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
					If Not IsNothing(itemDetalle.IDTempNuevoRegistro) Then
						If _ListaArchivoTransferencia.Where(Function(i) i.ID = itemDetalle.IDTempNuevoRegistro And i.IDDetalle = 0).Count > 0 Then
							_ListaArchivoTransferencia.Where(Function(i) i.ID = itemDetalle.IDTempNuevoRegistro And i.IDDetalle = 0).First.IDDetalle = itemDetalle.lngIDDetalle
						End If
					Else
						If _ListaArchivoTransferencia.Where(Function(i) i.ID = itemDetalle.lngIDDetalle And i.IDDetalle = 0).Count > 0 Then
							_ListaArchivoTransferencia.Where(Function(i) i.ID = itemDetalle.lngIDDetalle And i.IDDetalle = 0).First.IDDetalle = itemDetalle.lngIDDetalle
						End If
					End If
				Next

				For Each li In _ListaArchivoTransferencia
					Dim objWppTransferencia_RC As New wppFrmTransferencia_RC(Me)
					objWppTransferencia_RC.ctlSubirArchivo.inicializarControl()
					AddHandler objWppTransferencia_RC.ctlSubirArchivo.finalizoTransmisionArchivo, AddressOf TerminoTransmisionArchivo
					AddHandler objWppTransferencia_RC.ctlSubirArchivo.errorTransmisionArchivo, AddressOf TerminoTransmisionArchivoError
					configurarDocumentos(objWppTransferencia_RC.ctlSubirArchivo, li.IDDetalle, GSTR_TRANSFERENCIA)
					If Not String.IsNullOrEmpty(li.NombreArchivo) Then
						objWppTransferencia_RC.ctlSubirArchivo.ClaveUnica = li.IDDetalle
						If Not String.IsNullOrEmpty(li.RutaArchivo) Then
							objWppTransferencia_RC.ctlSubirArchivo.subirArchivo(li.NombreArchivo, li.RutaArchivo)
						Else
							objWppTransferencia_RC.ctlSubirArchivo.subirArchivo(li.NombreArchivo, li.ByteArchivo)
						End If
					End If
				Next
			End If

			If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_Consignaciones) And Not IsNothing(_ListaArchivoConsignacion) Then
				For Each itemDetalle In _ListatesoreriaordenesplusRC_Detalle_Consignaciones
					If Not IsNothing(itemDetalle.IDTempNuevoRegistro) Then
						If _ListaArchivoConsignacion.Where(Function(i) i.ID = itemDetalle.IDTempNuevoRegistro And i.IDDetalle = 0).Count > 0 Then
							_ListaArchivoConsignacion.Where(Function(i) i.ID = itemDetalle.IDTempNuevoRegistro And i.IDDetalle = 0).First.IDDetalle = itemDetalle.lngIDDetalle
						End If
					Else
						If _ListaArchivoConsignacion.Where(Function(i) i.ID = itemDetalle.lngIDDetalle And i.IDDetalle = 0).Count > 0 Then
							_ListaArchivoConsignacion.Where(Function(i) i.ID = itemDetalle.lngIDDetalle And i.IDDetalle = 0).First.IDDetalle = itemDetalle.lngIDDetalle
						End If
					End If
				Next

				For Each li In _ListaArchivoConsignacion
					Dim objWppConsignacion As New wppFrmConsignaciones_RC(Me)
					objWppConsignacion.ctlSubirArchivo.inicializarControl()
					AddHandler objWppConsignacion.ctlSubirArchivo.finalizoTransmisionArchivo, AddressOf TerminoTransmisionArchivo
					AddHandler objWppConsignacion.ctlSubirArchivo.errorTransmisionArchivo, AddressOf TerminoTransmisionArchivoError
					configurarDocumentos(objWppConsignacion.ctlSubirArchivo, li.IDDetalle, GSTR_CONSIGNACIONES)
					If Not String.IsNullOrEmpty(li.NombreArchivo) Then
						objWppConsignacion.ctlSubirArchivo.ClaveUnica = li.IDDetalle
						If Not String.IsNullOrEmpty(li.RutaArchivo) Then
							objWppConsignacion.ctlSubirArchivo.subirArchivo(li.NombreArchivo, li.RutaArchivo)
						Else
							objWppConsignacion.ctlSubirArchivo.subirArchivo(li.NombreArchivo, li.ByteArchivo)
						End If
					End If
				Next
			End If

			If intCantidadDocumentosSubir = 0 Then
				IsBusy = False
				IsBusyDetalles = False
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recorrer los detalles para subir los documentos.", Me.ToString(), "RecorrerDetallesDocumento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub TerminoTransmisionArchivo(pstrArchivo As String, pstrRuta As String)
		Try
			intCantidadDocumentosSubidos = intCantidadDocumentosSubidos + 1
			If intCantidadDocumentosSubidos >= intCantidadDocumentosSubir Then
				logRecargar = True
				If logRecargar Then
					'Thread.Sleep(9000)
					RecargarPantalla()
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al finalizar el guardado de la orden.", Me.ToString(), "TerminoTransmisionArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub TerminoTransmisionArchivoError(pstrArchivo As String, pstrRuta As String, pobjError As Exception)
		Try
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al subir el documento adjunto.", Me.ToString(), "TerminoTransmisionArchivoError", Program.TituloSistema, Program.Maquina, pobjError, Program.RutaServicioLog)
			logRecargar = True
			If logRecargar Then
				'Thread.Sleep(9000)
				RecargarPantalla()
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al finalizar el guardado de la orden.", Me.ToString(), "TerminoTransmisionArchivoError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Public Async Sub ConfigurarOrdenDuplicada(pOrdenTesoreriaEncabezado As TesoreriaOrdenesEncabezado, pListaDetallesCheques As List(Of TesoreriaOyDPlusChequesRecibo),
										pListaDetallesTransferencias As List(Of TesoreriaOyDPlusTransferenciasRecibo), pListaDetalleConsignaciones As List(Of TesoreriaOyDPlusConsignacionesRecibo),
										pListaDetallesCargarPagosA As List(Of TesoreriaOyDPlusCargosPagosARecibo), pListaDetallesCargarPagosAFondos As List(Of TesoreriaOyDPlusCargosPagosAFondosRecibo))
		Try
			logDuplicarRegistro = True
			Editando = True

			dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()

			Dim xDefecto As New TesoreriaOrdenesEncabezado

			xDefecto.strNombre = pOrdenTesoreriaEncabezado.strNombre
			xDefecto.lngNroDocumento = pOrdenTesoreriaEncabezado.lngNroDocumento
			xDefecto.lngIDDocumento = pOrdenTesoreriaEncabezado.lngIDDocumento
			xDefecto.strIDComitente = pOrdenTesoreriaEncabezado.strIDComitente
			xDefecto.strEstado = pOrdenTesoreriaEncabezado.strEstado
			xDefecto.strNroDocumento = pOrdenTesoreriaEncabezado.strNroDocumento
			xDefecto.curValor = pOrdenTesoreriaEncabezado.curValor
			xDefecto.strCuentaCliente = pOrdenTesoreriaEncabezado.strCuentaCliente
			xDefecto.strTipoIdentificacion = pOrdenTesoreriaEncabezado.strTipoIdentificacion
			xDefecto.strTipoProducto = pOrdenTesoreriaEncabezado.strTipoProducto
			xDefecto.strCodigoReceptor = pOrdenTesoreriaEncabezado.strCodigoReceptor
			xDefecto.ValorTipo = GSTR_ORDENRECIBO
			If logEsFondosOYD And pOrdenTesoreriaEncabezado.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
				xDefecto.dtmDocumento = pOrdenTesoreriaEncabezado.dtmDocumento
				xDefecto.dtmFechaAplicacion = pOrdenTesoreriaEncabezado.dtmFechaAplicacion
				xDefecto.dtmFechaCierreFondo = pOrdenTesoreriaEncabezado.dtmFechaCierreFondo
			Else
				xDefecto.dtmDocumento = dtmFechaServidor.Date
				xDefecto.dtmFechaAplicacion = dtmFechaServidor.Date
			End If

			xDefecto.logClientePresente = pOrdenTesoreriaEncabezado.logClientePresente
			xDefecto.logClienteRecoge = pOrdenTesoreriaEncabezado.logClienteRecoge
			xDefecto.logConsignarCta = pOrdenTesoreriaEncabezado.logConsignarCta
			xDefecto.logDireccionInscrita_Instrucciones = pOrdenTesoreriaEncabezado.logDireccionInscrita_Instrucciones
			xDefecto.logEsCtaInscrita_Instrucciones = pOrdenTesoreriaEncabezado.logEsCtaInscrita_Instrucciones
			xDefecto.logEsTercero_Instrucciones = pOrdenTesoreriaEncabezado.logEsTercero_Instrucciones
			xDefecto.logllevarDireccion = pOrdenTesoreriaEncabezado.logllevarDireccion
			xDefecto.logRecogeTercero = pOrdenTesoreriaEncabezado.logRecogeTercero
			xDefecto.strCiudad_Instrucciones = pOrdenTesoreriaEncabezado.strCiudad_Instrucciones
			xDefecto.strNroDocumento_Instrucciones = pOrdenTesoreriaEncabezado.strNroDocumento_Instrucciones
			xDefecto.strTipoIdentificacion_Instrucciones = pOrdenTesoreriaEncabezado.strTipoIdentificacion_Instrucciones
			xDefecto.ValorTipoCta_Instrucciones = pOrdenTesoreriaEncabezado.ValorTipoCta_Instrucciones
			xDefecto.ValorTipoIdentificacion = pOrdenTesoreriaEncabezado.ValorTipoIdentificacion
			xDefecto.ValorTipoProducto = pOrdenTesoreriaEncabezado.ValorTipoProducto
			xDefecto.strTipoRetiroFondos = pOrdenTesoreriaEncabezado.strTipoRetiroFondos
			xDefecto.strCarteraColectivaFondos = pOrdenTesoreriaEncabezado.strCarteraColectivaFondos
			xDefecto.ValorEstado = String.Empty
			xDefecto.strEstado = String.Empty

			ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = pListaDetallesCheques
			ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = pListaDetallesTransferencias
			ListatesoreriaordenesplusRC_Detalle_CargarPagosA = pListaDetallesCargarPagosA
			ListatesoreriaordenesplusRC_Detalle_Consignaciones = pListaDetalleConsignaciones
			ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = pListaDetallesCargarPagosAFondos

			ConsultarSaldo = False
			ConsultarSaldo = True


			HabilitarEnEdicion = True
			HabilitarFecha = True

			logCambiarPropiedadesPOPPUP = False
			TesoreriaOrdenesPlusRC_Selected = xDefecto
			logCambiarPropiedadesPOPPUP = True
			logCaracterInvalido = True

			VerDuplicar = Visibility.Collapsed
			CalcularTotales(GSTR_CHEQUE)
			CalcularTotales(GSTR_TRANSFERENCIA)
			CalcularTotales(GSTR_CONSIGNACIONES)

			If TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
				If logEsFondosOYD Then
					HabilitarConceptoDetalles = False
				Else
					HabilitarConceptoDetalles = True
				End If
			End If

			IsBusy = False
			IsBusyDetalles = False
			CambiarColorFondoTextoBuscador()
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar configurar el duplicar datos de la orden", Me.ToString(), "ConfigurarOrdenDuplicada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub Duplicar()
		Try
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC) Then
				If ListaTesoreriaOrdenesPlusRC.Count > 0 Then
					If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
						mostrarMensajePregunta("¿Está Seguro que desea duplicar la orden?",
										   Program.TituloSistema,
										   "DUPLICARORDEN",
										   AddressOf TerminoPreguntarDuplicarOrden, False)
					Else
						mostrarMensaje("No es posible duplicar una orden de recaudo, verifique que tenga selecionado un registro.", "Duplicar Orden", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					End If
				Else
					mostrarMensaje("No es posible duplicar una orden de Recaudo, verifique que minimo exista una orden de Recaudo previamente creada", "Duplicar Orden", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If
			Else
				mostrarMensaje("No es posible duplicar una orden de Recaudo, verifique que minimo exista una orden de Recaudo previamente creada", "Duplicar Orden", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la duplicación de la orden", Me.ToString(), "duplicarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
		End Try
	End Sub

	Private Sub validarDuplicarOrden(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

			If objResultado.DialogResult Then

			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el duplicar una orden", Me.ToString(), "validarDuplicarOrden", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub TraerOrdenes(Optional ByVal pstrOpcion As String = "", Optional pstrEstado As String = "")
		Try
			IsBusy = True
			If Not IsNothing(dcProxy.TesoreriaOrdenesEncabezados) Then
				dcProxy.TesoreriaOrdenesEncabezados.Clear()
			End If
			If logXTesorero = False Then
				If pstrEstado = GSTR_ANULADAS Then
					dcProxy.Load(dcProxy.TesoreriaOrdenesListarQuery(GSTR_ORDENRECIBO, GSTR_ANULADA_Plus, "", "", 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenes, pstrOpcion)
				ElseIf pstrEstado = GSTR_PENDIENTES Then
					dcProxy.Load(dcProxy.TesoreriaOrdenesListarQuery(GSTR_ORDENRECIBO, GSTR_PENDIENTE_Plus, "", "", 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenes, pstrOpcion)
				ElseIf pstrEstado = GSTR_RECHAZADAS Then
					dcProxy.Load(dcProxy.TesoreriaOrdenesListarQuery(GSTR_ORDENRECIBO, GSTR_RECHAZADAS_Plus, "", "", 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenes, pstrOpcion)
				ElseIf pstrEstado = GSTR_POR_APROBAR Then
					dcProxy.Load(dcProxy.TesoreriaOrdenesListarQuery(GSTR_ORDENRECIBO, GSTR_PENDIENTEAPROBACION_Plus, "", "", 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenes, pstrOpcion)
				End If
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las ordenes.", Me.ToString(), "TraerOrdenes", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			logGuardandoOrden = False
		End Try
	End Sub

	Public Sub PorDefectoInstrucciones()
		Try
			strNombreInstrucciones = String.Empty
			strNroDocumentoInstrucciones = String.Empty
			strTipoIdentificacionInstrucciones = String.Empty
			ClientePresente = False
			ClienteRecoge = False
			strCodigoBancoInstrucciones = String.Empty
			RecogeTercero = False

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al colocar el por defecto de las instrucciones.", Me.ToString(), "PorDefectoInstrucciones", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub Habilitar_Encabezado()
		Try
			If Editando Then

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
						HabilitarEncabezado = False
						HabilitarReceptor = False
						HabilitarTipoProducto = False
						HabilitarEntregarA = False
						HabilitarBuscadorCliente = False

						'JABG20180412
						'Habilitar cartera colectiva y encargo 
						HabilitarCarteraColectiva = False
						HabilitarFecha = False

					Else
						HabilitarEncabezado = True
						HabilitarReceptor = True
						HabilitarTipoProducto = True
						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							HabilitarEntregarA = False
							IDTipoClienteEntregado = GSTR_CLIENTE
						Else
							HabilitarEntregarA = True
						End If

						HabilitarBuscadorCliente = True

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarCarteraColectiva = True
						HabilitarFecha = True

						PorDefectoInstrucciones()

					End If
				End If

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
						HabilitarEncabezado = False
						HabilitarReceptor = False
						HabilitarTipoProducto = False
						HabilitarEntregarA = False
						HabilitarBuscadorCliente = False
						HabilitarCarteraColectiva = False

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarFecha = False

					Else
						HabilitarEncabezado = True
						HabilitarReceptor = True
						HabilitarTipoProducto = True
						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							HabilitarEntregarA = False
							IDTipoClienteEntregado = GSTR_CLIENTE
						Else
							HabilitarEntregarA = True
						End If
						HabilitarBuscadorCliente = True
						HabilitarCarteraColectiva = True

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarFecha = True

					End If
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > 0 Then
						HabilitarEncabezado = False
						HabilitarReceptor = False
						HabilitarTipoProducto = False
						HabilitarEntregarA = False
						HabilitarBuscadorCliente = False

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarCarteraColectiva = False
						HabilitarFecha = False

					Else
						HabilitarEncabezado = True
						HabilitarReceptor = True
						HabilitarTipoProducto = True
						HabilitarBuscadorCliente = True
						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							HabilitarEntregarA = False
							IDTipoClienteEntregado = GSTR_CLIENTE
						Else
							HabilitarEntregarA = True
						End If

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarCarteraColectiva = True
						HabilitarFecha = True

					End If
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
					If _ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
						HabilitarEncabezado = False
						HabilitarReceptor = False
						HabilitarTipoProducto = False
						HabilitarEntregarA = False
						HabilitarBuscadorCliente = False

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarCarteraColectiva = False
						HabilitarFecha = False

					Else
						HabilitarEncabezado = True
						HabilitarReceptor = True
						HabilitarTipoProducto = True
						HabilitarBuscadorCliente = True
						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							HabilitarEntregarA = False
							IDTipoClienteEntregado = GSTR_CLIENTE
						Else
							HabilitarEntregarA = True
						End If

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarCarteraColectiva = True
						HabilitarFecha = True

					End If

				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
					If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
						HabilitarEncabezado = False
						HabilitarReceptor = False
						HabilitarTipoProducto = False
						HabilitarEntregarA = False
						HabilitarBuscadorCliente = False
						HabilitarCarteraColectiva = False

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarFecha = False

					Else
						HabilitarEncabezado = True
						HabilitarReceptor = True
						HabilitarTipoProducto = True
						HabilitarBuscadorCliente = True
						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							HabilitarEntregarA = False
							IDTipoClienteEntregado = GSTR_CLIENTE
						Else
							HabilitarEntregarA = True
						End If
						HabilitarCarteraColectiva = True

						'JABG20180412
						'Habilitar cartera colectiva y encargo
						HabilitarFecha = True

					End If
				End If
			Else
				HabilitarEncabezado = False
				HabilitarReceptor = False
				HabilitarTipoProducto = False
				HabilitarEntregarA = False
				HabilitarBuscadorCliente = False
				HabilitarCarteraColectiva = False
				'JABG20180412
				'Habilitar cartera colectiva y encargo
				HabilitarFecha = False

			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar el encabezado.", Me.ToString(), "Habilitar_Encabezado", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub MostrarMensajeGMF(pstrTipoGMF As String)
		Try
			If pstrTipoGMF = GSTR_GMF_ENCIMA Then
				A2Utilidades.Mensajes.mostrarMensaje("No se puede calcular el valor del GMF por encima ya que no esta configurado correctamente, o este tiene un valor igual a CERO", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			ElseIf pstrTipoGMF = GSTR_GMF_DEBAJO Then
				A2Utilidades.Mensajes.mostrarMensaje("No se puede calcular el valor del GMF por debajo ya que no esta configurado correctamente, o este tiene un valor igual a CERO", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al mostrar el mensaje GMF.", Me.ToString(), "MostrarMensajeGMF", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub BuscarControlValidacion(ByVal pViewOrdenesTesoreria As OrdenesReciboPLUSView, ByVal pstrOpcion As String)
		Try
			If Not IsNothing(pViewOrdenesTesoreria) Then

				If TypeOf pViewOrdenesTesoreria.stackpEdicion.FindName(pstrOpcion) Is TabItem Then
					CType(pViewOrdenesTesoreria.stackpEdicion.FindName(pstrOpcion), TabItem).IsSelected = True
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control dentro de la orden.", Me.ToString, "BuscarControlValidacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub OrganizarNuevaBusqueda()
		Try
			Dim objBusqueda As New CamposBusquedaTesoreriaOyDPLUS
			objBusqueda.lngID = 0
			objBusqueda.strIDComitente = String.Empty
			objBusqueda.strCodigoReceptor = String.Empty
			objBusqueda.strEstado = String.Empty
			objBusqueda.strNroDocumento = String.Empty

			cb = objBusqueda

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al organizar la nueva busqueda.", Me.ToString(), "OrganizarNuevaBusqueda", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Sub Filtrar()
		Try
			IsBusy = True
			If Not IsNothing(dcProxy.TesoreriaOrdenesEncabezados) Then
				dcProxy.TesoreriaOrdenesEncabezados.Clear()
			End If

			If FiltroVM.Length > 0 Then
				Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(FiltroVM, clsExpresiones.TipoExpresion.LetrasNumeros)
				Dim logConsultar As Boolean = True
				If Not IsNothing(objValidacion) Then
					If objValidacion.TextoValido = False Then
						logConsultar = False
					End If
				End If

				If logConsultar Then
					logFiltrar = True
					dcProxy.Load(dcProxy.TesoreriaOrdenesConsultarQuery(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, FiltroVM, Program.Usuario, "", GSTR_ORDENRECIBO, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesConsultar, "FILTRAR")
				Else
					IsBusy = False
					FiltroVM = String.Empty
					mostrarMensaje("¡La opción filtrar no se puede realizar, el filtro que ingreso posee caracteres NO válidos!", "Filtrar", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

				End If
			Else
				TraerOrdenes("", VistaSeleccionada)
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Sub Buscar()
		Try
			OrganizarNuevaBusqueda()
			MyBase.Buscar()
			MyBase.CambioItem("visBuscando")
			MyBase.CambioItem("visNavegando")
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación de busqueda", Me.ToString(), "Buscar", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Public Overrides Sub ConfirmarBuscar()
		Try
			If cb.lngID <> 0 Or Not String.IsNullOrEmpty(cb.strNroDocumento) Or
					Not String.IsNullOrEmpty(cb.strEstado) Or Not String.IsNullOrEmpty(cb.strIDComitente) Or
					Not String.IsNullOrEmpty(cb.strCodigoReceptor) Or Not String.IsNullOrEmpty(cb.strTipoProducto) Then
				IsBusy = True
				ErrorForma = ""
				logBuscar = True
				TesoreriaOrdenesPlusAnterior = Nothing
				If Not IsNothing(dcProxy.TesoreriaOrdenesEncabezados) Then
					dcProxy.TesoreriaOrdenesEncabezados.Clear()
				End If
				dcProxy.Load(dcProxy.TesoreriaOrdenesConsultarQuery(cb.lngID, cb.strNroDocumento, cb.strEstado, cb.strIDComitente, cb.strCodigoReceptor, cb.strTipoProducto, Nothing, Program.Usuario, "CON", GSTR_ORDENRECIBO, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesConsultar, "")
				MyBase.ConfirmarBuscar()
				cb = New CamposBusquedaTesoreriaOyDPLUS
			Else
				mostrarMensaje("Para Realizar la Búsqueda ingrese los Datos Correspondientes", "Confirmar Buscar", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar los registros.", Me.ToString(), "ConfirmarBuscar", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Public Sub actualizarComitenteOrden(objComitente As OYDUtilidades.BuscadorClientes)
		Try
			ComitenteSeleccionado = objComitente
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del Cliente",
														 Me.ToString(), "actualizarComitenteOrden", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Async Sub NuevoRegistro()
		Try
			If dcProxy.IsLoading Then
                MyBase.CambiarALista()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
			End If

			IsBusy = True
			dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()
			FechaOrden = dtmFechaServidor
			FechaAplicacion = dtmFechaServidor

			VerDuplicar = Visibility.Collapsed
			VerCargarPagosA = Visibility.Collapsed
			VerCargarPagosAFondos = Visibility.Collapsed
			VerDatosFondos = Visibility.Collapsed
			HabilitarCategoriaFondos = False
			HabilitarInstrucciones = False
			HabilitarEnEdicion = True
			HabilitarFecha = True
			HabilitarEncabezado = True
			HabilitarEdicionEncabezado = True
			HabilitarImportacion = False
			CodigoOYDControles = Nothing
			strCodigoOyDCliente = Nothing
			'ConsultarSaldo = True
			logNuevoRegistro = True
			logEditarRegistro = False
			logDuplicarRegistro = False
			logDuplicarRegistro = False
			logBuscar = False
			HabilitarReceptor = True
			HabilitarBuscadorCliente = False
			If BorrarCliente Then
				BorrarCliente = False
			End If
			logEditarCargarPagosA = False
			logEditarChequeRC = False
			logEditarConsignacion = False
			logEditarTransferenciaRC = False
			ListaArchivoConsignacion = Nothing
			ListaArchivosCheque = Nothing
			ListaArchivoTransferencia = Nothing
			CuentaRegistrada = Nothing
			BorrarCliente = True
			Dim xDefecto As New TesoreriaOrdenesEncabezado

			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
				TesoreriaOrdenesPlusAnterior = TesoreriaOrdenesPlusRC_Selected
				TesoreriaOrdenesPlusAnterior.ValorTipoIdentificacion = TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion
			End If

			If Not IsNothing(dcProxy.TesoreriaOyDPlusChequesRecibos) Then
				dcProxy.TesoreriaOyDPlusChequesRecibos.Clear()
			End If
			If Not IsNothing(dcProxy.TesoreriaOyDPlusTransferenciasRecibos) Then
				dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Clear()
			End If
			If Not IsNothing(dcProxy.TesoreriaOyDPlusCargosPagosARecibos) Then
				dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Clear()
			End If
			If Not IsNothing(dcProxy.TesoreriaOyDPlusConsignacionesRecibos) Then
				dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Clear()
			End If
			If Not IsNothing(dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos) Then
				dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Clear()
			End If


			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
				ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = Nothing
			End If
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
				ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = Nothing
			End If
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
				ListatesoreriaordenesplusRC_Detalle_CargarPagosA = Nothing
			End If
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
				ListatesoreriaordenesplusRC_Detalle_Consignaciones = Nothing
			End If
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
				ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones = Nothing
			End If
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
				ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = Nothing
			End If

			ValorTotalGenerarActual = 0
			ValorTotalGenerarOrden = 0
			ValorTotalGenerarCheque = 0
			ValorTotalGenerarConsignacion = 0
			ValorTotalGenerarTransferencia = 0
			xDefecto.strNombre = String.Empty
			xDefecto.lngNroDocumento = 0
			xDefecto.strIDComitente = Nothing
			xDefecto.strEstado = String.Empty
			xDefecto.strNroDocumento = String.Empty
			xDefecto.curValor = 0
			xDefecto.strCuentaCliente = String.Empty
			xDefecto.strTipoIdentificacion = String.Empty
			xDefecto.strTipoProducto = String.Empty
			xDefecto.strCodigoReceptor = String.Empty
			xDefecto.strNombreConsecutivo = String.Empty
			xDefecto.dtmActualizacion = dtmFechaServidor.Date
			xDefecto.dtmDocumento = dtmFechaServidor.Date
			xDefecto.strTipo = GSTR_ORDENRECIBO
			xDefecto.strUsuario = Program.Usuario
			xDefecto.strUsuarioWindows = Program.UsuarioWindows

			strNombreEntregado = String.Empty
			strNroDocumentoEntregado = String.Empty
			strTipoIdentificacionEntregado = Nothing
			IDTipoClienteEntregado = Nothing

			TesoreriaOrdenesPlusRC_Selected = xDefecto
			TesoreriaOrdenesPlusRC_Selected.strNombre = xDefecto.strNombre
			TesoreriaOrdenesPlusRC_Selected.lngNroDocumento = xDefecto.lngNroDocumento
			TesoreriaOrdenesPlusRC_Selected.lngIDDocumento = xDefecto.lngIDDocumento
			TesoreriaOrdenesPlusRC_Selected.strIDComitente = xDefecto.strIDComitente

			TesoreriaOrdenesPlusRC_Selected.strEstado = xDefecto.strEstado
			TesoreriaOrdenesPlusRC_Selected.strNroDocumento = xDefecto.strNroDocumento
			TesoreriaOrdenesPlusRC_Selected.curValor = xDefecto.curValor
			TesoreriaOrdenesPlusRC_Selected.strCuentaCliente = xDefecto.strCuentaCliente
			TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion = xDefecto.strTipoIdentificacion
			TesoreriaOrdenesPlusRC_Selected.strTipoProducto = xDefecto.strTipoProducto
			TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor = xDefecto.strCodigoReceptor
			TesoreriaOrdenesPlusRC_Selected.ValorTipo = GSTR_ORDENRECIBO
			TesoreriaOrdenesPlusRC_Selected.strUsuario = xDefecto.strUsuario
			TesoreriaOrdenesPlusRC_Selected.strUsuarioWindows = xDefecto.strUsuarioWindows

			strNombreInstrucciones = String.Empty
			strNroDocumentoInstrucciones = String.Empty
			strTipoIdentificacionInstrucciones = String.Empty
			ClientePresente = False
			ClienteRecoge = False
			strCodigoBancoInstrucciones = String.Empty
			CarteraColectivaFondos = Nothing

			RecogeTercero = False

			CalcularTotales(GSTR_CHEQUE)
			CalcularTotales(GSTR_TRANSFERENCIA)
			If ConsultarSaldo Then
				ConsultarSaldo = False
			End If
			ConsultarSaldo = True
			Editando = True
			HabilitarEntregarA = False
			VerificarPrimerTabHabilitado()
			MyBase.CambioItem("Editando")
			CargarReceptoresUsuarioOYDPLUS("")
			CambiarColorFondoTextoBuscador()

			'BotonesOrdenes = True 'JRP se cambia el valor a la variable para que muestre los botones de las operaciones de Ordenes
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
														 Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
			IsBusyDetalles = False
			IsBusy = False
		End Try
	End Sub

	Public Sub GuardarInstrucciones()
		Try
			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
				Dim intPosicion As Integer = 0
				TesoreriaOrdenesPlusRC_Selected.strTipoCliente = IDTipoClienteEntregado
				TesoreriaOrdenesPlusRC_Selected.strNombre_Instrucciones = strNombreEntregado
				TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion_Instrucciones = strTipoIdentificacionEntregado
				TesoreriaOrdenesPlusRC_Selected.strNroDocumento_Instrucciones = strNroDocumentoEntregado

			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema guardando Instrucciones", Me.ToString(), "GuardarInstrucciones", Program.TituloSistema, Program.Maquina, ex)
			IsBusyDetalles = False
			IsBusy = False
		End Try
	End Sub

	Public Sub ValidarListarRestrictivas(pstrRegistros As String)
		Try
			IsBusy = True
			If Not IsNothing(dcProxy.tblRespuestaValidacionesTesorerias) Then
				dcProxy.tblRespuestaValidacionesTesorerias.Clear()
			End If
			'dcProxy.Load(dcProxy.ValidarListaRestrictivaQuery(pstrRegistros, Program.Usuario, 0), AddressOf TerminoValidarListasRestrictivas, "")
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Validar Listas Restrictivas", Me.ToString(), "ValidarListarRestrictivas", Program.TituloSistema, Program.Maquina, ex)
			IsBusyDetalles = False
			IsBusy = False
		End Try
	End Sub

	Public Sub Validarfecha()
		Try
			'dcProxy.tblFechasHabiles.Clear()
			'dcProxy.Load(dcProxy.CalcularDiaHabilQuery(FechaOrden.ToShortDateString, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEstadoTesoreria, "Validarfecha")

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar fechas.", Me.ToString(), "Validarfecha", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub ObtenerProximaFechaHabilFondo(ByVal pdtmFechaCierre As Nullable(Of DateTime))
		Try
			Dim pdtmFechaConsulta As DateTime = Nothing

			If Not IsNothing(pdtmFechaCierre) Then
				pdtmFechaConsulta = pdtmFechaCierre.Value.AddDays(1)
				FechaOrden = pdtmFechaConsulta
			Else
				pdtmFechaConsulta = FechaOrden
			End If

			dcProxy.tblFechasHabiles.Clear()
			dcProxy.Load(dcProxy.CalcularDiaHabilQuery(_TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto, _TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos, pdtmFechaConsulta.ToShortDateString, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEstadoTesoreria, "ValidarFechaCierre")
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar fechas.", Me.ToString(), "Validarfecha", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	Public Sub Validarfechaaplicacion()
		Try
			If logNuevoRegistro Then
				If logEsFondosOYD Then
					If Not IsNothing(FechaAplicacion) And Not IsNothing(FechaOrden) Then
						If TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
							If FechaAplicacion.Value.Date < FechaOrden.Date Then
								mostrarMensaje("La fecha de aplicación no puede ser menor a la fecha de orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
							ElseIf Not IsNothing(dtmFechaMenorPermitidaIngreso) Then
								If FechaAplicacion.Value.Date < DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, dtmFechaMenorPermitidaIngreso.Value.Date) Then
									mostrarMensaje(String.Format("La fecha de aplicación no puede ser menor a la fecha de cierre + los días de aplicación {0:yyyy-MM-dd}", DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, dtmFechaMenorPermitidaIngreso.Value.Date)), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
								End If
							End If
						ElseIf TipoAccionFondos = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
							If FechaAplicacion.Value.Date < FechaOrden.Date Then
								mostrarMensaje("La fecha de aplicación no puede ser menor a la fecha de orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
							ElseIf Not IsNothing(dtmFechaMenorPermitidaIngreso) Then
								If FechaAplicacion.Value.Date < DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, dtmFechaMenorPermitidaIngreso.Value.Date) Then
									mostrarMensaje(String.Format("La fecha de aplicación no puede ser menor a la fecha de cierre + los días de aplicación {0:yyyy-MM-dd}", DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, dtmFechaMenorPermitidaIngreso.Value.Date)), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
								End If
							End If
						End If
					End If
				End If
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar fechas.", Me.ToString(), "Validarfechaaplicacion", Program.TituloSistema, Program.Maquina, ex)

		End Try
	End Sub

	Private Sub TerminoatraerCuentasbancarias(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
		Try
			If Not lo.HasError Then
				If dcProxyUtilidades.ItemCombos.Count > 0 Then
					'listCuentasbancarias = dcProxyUtilidades1.ItemCombos.Where(Function(y) y.Categoria = "CuentasBancarias").ToList
				End If
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las cuentas bancarias.",
									 Me.ToString(), "TerminoatraerCuentasbancarias", Application.Current.ToString(), Program.Maquina, lo.Error)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar las cuentas bancarias.",
									   Me.ToString(), "TerminoatraerCuentasbancarias", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Async Sub ActualizarRegistro()
		Try
			CalcularTotales(String.Empty)
			_TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = TipoAccionFondos
			_TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos = CarteraColectivaFondos

			If ValidarGuardadoOrden(_TesoreriaOrdenesPlusRC_Selected) Then
				IsBusy = True
				dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()

				logGuardandoOrden = False
				LimpiarVariablesConfirmadas()
				dcProxy.tblFechasHabiles.Clear()
				dcProxy.Load(dcProxy.CalcularDiaHabilQuery(_TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto, _TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos, FechaOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEstadoTesoreria, "validar_fecha_guardado")

			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden.", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub ValidarIngresoOrdenTesoreria()
		Try
			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
				IsBusy = True

				Dim HayCargoPagosA As Boolean = False
				Dim HayCargoPagosAFondos As Boolean = False
				Dim HayCheques As Boolean = False
				Dim HayTransferencias As Boolean = False
				Dim HayConsignacionesCheques As Boolean = False
				Dim HayConsignacionesEfectivo As Boolean = False
				Dim strDetalleCheques As String = String.Empty
				Dim strDetalleTransferencias As String = String.Empty
				Dim strDetalleConsignacion As String = String.Empty
				Dim strDetalleCargarPagosA As String = String.Empty
				Dim strDetalleCargarPagosAFondos As String = String.Empty


				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
					For Each li In ListatesoreriaordenesplusRC_Detalle_CargarPagosA
						If String.IsNullOrEmpty(strDetalleCargarPagosA) Then
							strDetalleCargarPagosA = String.Format("'{0}'**'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'",
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.strFormaPago, li.strTipo,
															  li.strCodigoOyD, li.curValor, li.curValorTotal, li.ValorTipoCliente,
															  li.strIDTipoCliente, li.strDetalleConcepto, li.lngIDConcepto, li.logEsProcesada)
						Else
							strDetalleCargarPagosA = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'",
															  strDetalleCargarPagosA,
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.strFormaPago, li.strTipo,
															  li.strCodigoOyD, li.curValor, li.curValorTotal, li.ValorTipoCliente,
															  li.strIDTipoCliente, li.strDetalleConcepto, li.lngIDConcepto, li.logEsProcesada)
						End If
					Next

					If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 And ValorDisponibleCargarPago = 0 Then
						HayCargoPagosA = True
					Else
						HayCargoPagosA = False
					End If
				Else
					HayCargoPagosA = False
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
					For Each li In ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
						If String.IsNullOrEmpty(strDetalleCargarPagosAFondos) Then
							strDetalleCargarPagosAFondos = String.Format("'{0}'**'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'**'{14}'**'{15}'**'{16}'**'{17}'",
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.strFormaPago, li.strTipo,
															  li.strCodigoOyD, li.curValor, li.curValorTotal, li.ValorTipoCliente,
															  li.strIDTipoCliente, li.strDetalleConcepto, li.lngIDConcepto,
															  li.strTipoAccionFondos, li.strDescripcionTipoAccionFondos, li.strCarteraColectivaFondos, li.intNroEncargoFondos, li.DescripcionEncargoFondos, li.logEsProcesada)
						Else
							strDetalleCargarPagosAFondos = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'**'{14}'**'{15}'**'{16}'**'{17}'**'{18}'",
															  strDetalleCargarPagosAFondos,
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.strFormaPago, li.strTipo,
															  li.strCodigoOyD, li.curValor, li.curValorTotal, li.ValorTipoCliente,
															  li.strIDTipoCliente, li.strDetalleConcepto, li.lngIDConcepto,
															  li.strTipoAccionFondos, li.strDescripcionTipoAccionFondos, li.strCarteraColectivaFondos, li.intNroEncargoFondos, li.DescripcionEncargoFondos, li.logEsProcesada)
						End If
					Next

					CalcularValorDisponibleCargar()

					If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 And ValorDisponibleCargarPagoFondos = 0 Then
						HayCargoPagosAFondos = True
					Else
						HayCargoPagosAFondos = False
					End If
				Else
					HayCargoPagosAFondos = False
				End If

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					For Each li In ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
						If String.IsNullOrEmpty(strDetalleCheques) Then
							strDetalleCheques = String.Format("'{0}'**'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'**'{14}'**'{15}'**'{16}'**'{17}'**'{18}'**'{19}'**'{20}'",
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.lngIDBanco, li.lngNroCheque,
															  li.logClienteTrae, li.logDireccionCliente, li.logOtraDireccion, li.strFormaPago,
															  li.strDireccionCheque, li.strCiudad, li.strTelefono, li.strTipo, li.curValor,
															  li.strClienteTrae, li.strDireccionCliente, li.strOtraDireccion, li.strDescripcionBanco,
															  li.strEstado, li.strSector, li.logEsProcesada)
						Else
							strDetalleCheques = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'**'{14}'**'{15}'**'{16}'**'{17}'**'{18}'**'{19}'**'{20}'**'{21}'",
															  strDetalleCheques,
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.lngIDBanco, li.lngNroCheque,
															  li.logClienteTrae, li.logDireccionCliente, li.logOtraDireccion, li.strFormaPago,
															  li.strDireccionCheque, li.strCiudad, li.strTelefono, li.strTipo, li.curValor,
															  li.strClienteTrae, li.strDireccionCliente, li.strOtraDireccion, li.strDescripcionBanco,
															  li.strEstado, li.strSector, li.logEsProcesada)
						End If
					Next

					If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
						HayCheques = True
					Else
						HayCheques = False
					End If
				Else
					HayCheques = False
				End If

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					For Each li In ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
						If String.IsNullOrEmpty(strDetalleTransferencias) Then
							strDetalleTransferencias = String.Format("'{0}'**'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'**'{14}'**'{15}'**'{16}'**'{17}'**'{18}'",
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.curValor, li.lngIDBanco,
															  li.lngIdBancoDestino, li.strTipo, li.strFormaPago, li.strCuentaOrigen,
															  li.strCuentadestino, li.strTipoCuentadestino, li.strTipoCuentaOrigen,
															  li.ValorTipoCuentaOrigen, li.ValorTipoCuentaDestino, li.strEstado,
															  li.strDescripcionBanco, li.strDescripcionBancoDestino, li.logEsCuentaRegistrada, li.logEsProcesada)
						Else
							strDetalleTransferencias = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'**'{14}'**'{15}'**'{16}'**'{17}'**'{18}'**'{19}'",
															  strDetalleTransferencias,
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.curValor, li.lngIDBanco,
															  li.lngIdBancoDestino, li.strTipo, li.strFormaPago, li.strCuentaOrigen,
															  li.strCuentadestino, li.strTipoCuentadestino, li.strTipoCuentaOrigen,
															  li.ValorTipoCuentaOrigen, li.ValorTipoCuentaDestino, li.strEstado,
															  li.strDescripcionBanco, li.strDescripcionBancoDestino, li.logEsCuentaRegistrada, li.logEsProcesada)
						End If
					Next

					If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
						HayTransferencias = True
					Else
						HayTransferencias = False
					End If
				Else
					HayTransferencias = False
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					For Each li In ListatesoreriaordenesplusRC_Detalle_Consignaciones
						If String.IsNullOrEmpty(strDetalleConsignacion) Then
							strDetalleConsignacion = String.Format("'{0}'**'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'**'{14}'**'{15}'**'{16}'**'{17}'",
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.curValor, li.strTipo,
															  li.strFormaPago, li.ValorFormaConsignacion, li.strFormaConsignacion, li.lngNroReferencia,
															  li.lngIdBancoDestino, li.strCuentadestino, li.ValorTipoCuentaDestino,
															  li.lngNroCheque, li.lngIDBanco, li.strDescripcionBanco,
															  li.strDescripcionCuentaConsignacion, li.strEstado, li.logEsProcesada)
						Else
							strDetalleConsignacion = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'**'{5}'**'{6}'**'{7}'**'{8}'**'{9}'**'{10}'**'{11}'**'{12}'**'{13}'**'{14}'**'{15}'**'{16}'**'{17}'**'{18}'",
															  strDetalleConsignacion,
															  li.lngID, li.lngIDDetalle, li.strConsecutivo, li.curValor, li.strTipo,
															  li.strFormaPago, li.ValorFormaConsignacion, li.strFormaConsignacion, li.lngNroReferencia,
															  li.lngIdBancoDestino, li.strCuentadestino, li.ValorTipoCuentaDestino,
															  li.lngNroCheque, li.lngIDBanco, li.strDescripcionBanco,
															  li.strDescripcionCuentaConsignacion, li.strEstado, li.logEsProcesada)
						End If
					Next

					If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Where(Function(i) i.ValorFormaConsignacion = GSTR_EFECTIVO).Count > 0 Then
						HayConsignacionesEfectivo = True
					Else
						HayConsignacionesEfectivo = False
					End If

					If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Where(Function(i) i.ValorFormaConsignacion = GSTR_CHEQUE).Count > 0 Then
						HayConsignacionesCheques = True
					Else
						HayConsignacionesCheques = False
					End If
				Else
					HayConsignacionesCheques = False
					HayConsignacionesEfectivo = False
				End If


				If ValidarIngresoDetallesCanje(String.Empty, False) = False Then
					Dim strMensajeValidacionChequeCanje As String
					strMensajeValidacionChequeCanje = "Cuando esta habilitado el control de canje de cheques, solo puede ser 1 sola forma de pago en los detalles de la orden."
					mostrarMensaje("No se puede grabar la orden de recaudo por alguna de las siguientes razones: " & vbCrLf & strMensajeValidacionChequeCanje, "Ordenes de recaudo.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					IsBusy = False
				ElseIf (HayCheques = False And
					   HayTransferencias = False And
					   HayConsignacionesCheques = False And
					   HayConsignacionesEfectivo = False) Or (HayCargoPagosA = False And HayCargoPagosAFondos = False) Then
					IsBusy = False
					Dim strMensajeValidacionGrabacion = String.Empty
					strMensajeValidacionGrabacion = String.Format("{0}{1} 1.Debe existir mínimo un registro en la orden ya sea cheque, transferencia o consignación.", strMensajeValidacionGrabacion, vbCrLf)
					strMensajeValidacionGrabacion = String.Format("{0}{1} 2.El valor total de la orden de recaudo debe estar distribuida a un código OYD o varios.", strMensajeValidacionGrabacion, vbCrLf)
					mostrarMensaje("No se puede grabar la orden de recaudo por alguna de las siguientes razones:" & vbCrLf & strMensajeValidacionGrabacion, "Ordenes de Recaudo.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Else
					TesoreriaOrdenesPlusRC_Selected.curValorNota = ValorTotalGenerarOrden
					TesoreriaOrdenesPlusRC_Selected.curValor = ValorTotalGenerarOrden

					If TesoreriaOrdenesPlusRC_Selected.curValor > 0 Then
						TesoreriaOrdenesPlusRC_Selected.curValorNota = ValorTotalGenerarOrden
						TesoreriaOrdenesPlusRC_Selected.curValor = ValorTotalGenerarOrden

						If TesoreriaOrdenesPlusRC_Selected.curValorNota > 0 And TesoreriaOrdenesPlusRC_Selected.curValor > 0 Then
							GuardarInstrucciones()

							If String.IsNullOrEmpty(TesoreriaOrdenesPlusRC_Selected.ValorEstado) Then
								TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTE_Plus
							End If
							If IsNothing(TesoreriaOrdenesPlusRC_Selected.lngIDBanco) Then
								TesoreriaOrdenesPlusRC_Selected.lngIDBanco = 0
							End If

							If logNuevoRegistro Or logDuplicarRegistro Then
								strOpcionActualizar = "ING"
							Else
								strOpcionActualizar = "MOD"
							End If

							If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
								dcProxy.tblRespuestaValidaciones.Clear()
							End If

							Dim EsFondoOYD As Boolean = False

							If TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
								If logEsFondosOYD Then
									EsFondoOYD = True
								End If
							End If

							dcProxy.Load(dcProxy.OYDPLUS_ValidarOrdenesReciboQuery(GSTR_ORDENRECIBO, TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor, TesoreriaOrdenesPlusRC_Selected.strNombreConsecutivo, TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto,
																		 TesoreriaOrdenesPlusRC_Selected.strIDComitente, TesoreriaOrdenesPlusRC_Selected.lngIDDocumento, TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion,
																		 Nothing, TesoreriaOrdenesPlusRC_Selected.strNroDocumento, TesoreriaOrdenesPlusRC_Selected.strNombre, TesoreriaOrdenesPlusRC_Selected.lngIDBanco, TesoreriaOrdenesPlusRC_Selected.curValor, strOpcionActualizar,
																		  TesoreriaOrdenesPlusRC_Selected.lngID, GSTR_PENDIENTE_Plus, FechaOrden, TesoreriaOrdenesPlusRC_Selected.strTipoCliente, TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion_Instrucciones, TesoreriaOrdenesPlusRC_Selected.strNroDocumento_Instrucciones, TesoreriaOrdenesPlusRC_Selected.strNombre_Instrucciones,
																		  Confirmaciones, ConfirmacionesUsuario, Justificaciones, JustificacionesUsuario, Aprobaciones, AprobacionesUsuario,
																		  strDetalleCheques, strDetalleTransferencias, strDetalleConsignacion, strDetalleCargarPagosA, strDetalleCargarPagosAFondos,
																		  Program.Usuario, TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos, TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos, Program.UsuarioWindows, TesoreriaOrdenesPlusRC_Selected.strObservaciones,
																		  EsFondoOYD, FechaAplicacion, Program.HashConexion), AddressOf TerminoValidarIngresoOrden, String.Empty)
						Else
							IsBusy = False
							mostrarMensaje("El Valor total de toda la Orden de Recaudo debe ser mayor que Cero", "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						End If
					Else
						IsBusy = False
						mostrarMensaje("Para realizar el proceso de Grabado por favor ingrese todos los campos correspondientes", "Validar Grabar", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					End If
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el ingreso de la orden de recaudo.", Me.ToString(), "ValidarIngresoOrdenTesoreria", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub ActualizarOrdenTesoreria(ByVal pintIDOrdenInsertada As Integer)
		Try
			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then

				Dim logpermitirCheques As Boolean = True
				Dim logpermitirTransferencias As Boolean = True
				Dim logpermitirCarteras As Boolean = True
				Dim logpermitirInternos As Boolean = True
				Dim logpermitirBloqueo As Boolean = True

				logGrabar = True
				IsBusy = True
				ValorTotalOrden = 0
				Dim origen = "uptate"

				'TesoreriaOrdenesPlusRC_Selected.lngID = pintIDOrdenInsertada

				GuardarInstrucciones()

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
						For Each li In ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
							li.lngIDTesoreriaEncabezado = pintIDOrdenInsertada
							If Not dcProxy.TesoreriaOyDPlusChequesRecibos.Contains(li) Then
								dcProxy.TesoreriaOyDPlusChequesRecibos.Add(li)
							End If
						Next
					End If
				End If
				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
						For Each li In ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
							li.lngIDTesoreriaEncabezado = pintIDOrdenInsertada
							If Not dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Contains(li) Then
								dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Add(li)
							End If
						Next
					End If
				End If
				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > 0 Then
						For Each li In ListatesoreriaordenesplusRC_Detalle_Consignaciones
							li.lngIDTesoreriaEncabezado = pintIDOrdenInsertada
							If Not dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Contains(li) Then
								dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Add(li)
							End If
						Next
					End If
				End If
				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
					If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
						For Each li In ListatesoreriaordenesplusRC_Detalle_CargarPagosA
							li.lngIDTesoreriaEncabezado = pintIDOrdenInsertada
							If Not dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Contains(li) Then
								dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Add(li)
							End If
						Next
					End If
				End If
				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
					If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
						For Each li In ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
							li.lngIDTesoreriaEncabezado = pintIDOrdenInsertada
							If Not dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Contains(li) Then
								dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Add(li)
							End If
						Next
					End If
				End If

				If dcProxy.TesoreriaOrdenesEncabezados.Where(Function(i) i.lngID = TesoreriaOrdenesPlusRC_Selected.lngID).Count = 0 Then
					origen = "insert"
					dcProxy.TesoreriaOrdenesEncabezados.Add(TesoreriaOrdenesPlusRC_Selected)
				End If

				If logNuevoRegistro Or logDuplicarRegistro Then
					strOpcionActualizar = "ING"
				Else
					strOpcionActualizar = "MOD"
				End If

				If String.IsNullOrEmpty(TesoreriaOrdenesPlusRC_Selected.ValorEstado) Then
					TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTE_Plus
				End If
				If IsNothing(TesoreriaOrdenesPlusRC_Selected.lngIDBanco) Then
					TesoreriaOrdenesPlusRC_Selected.lngIDBanco = 0
				End If

				If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
					dcProxy.tblRespuestaValidaciones.Clear()
				End If

				IsBusy = False
				Program.VerificarCambiosProxyServidor(dcProxy)
				dcProxy2.SubmitChanges(AddressOf TerminoSubmitChanges, "")

			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Actualizar Registro",
								 Me.ToString(), "ActualizarOrdenTesoreria", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Sub TerminoSubmitChanges(So As SubmitOperation)
		Try
			If So.HasError Then
				IsBusy = False

				Dim strMsg As String = String.Empty
				'TODO: Pendiente garantizar que Userstate no venga vacío
				'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
				'                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
				If So.EntitiesInError.Count > 0 Then
					For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
						strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
					Next

					A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					So.MarkErrorAsHandled()
				Else
					A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					So.MarkErrorAsHandled()
				End If
				logGuardandoOrden = False
			Else
				Editando = False
				MyBase.CambioItem("Editando")

				logNuevoRegistro = False
				logEditarRegistro = False
				logDuplicarRegistro = False
				logCancelarRegistro = False
				logGuardandoOrden = True
				IsBusy = False

				mostrarMensajeResultadoAsincronico("¡La orden de recibo se guardó correctamente!", "Ordenes Tesoreria", AddressOf TerminoMensajeResultadoAsincronico, "TERMINOGUARDAR", A2Utilidades.wppMensajes.TiposMensaje.Exito)

				' Se comenta ya que no puede consultar los detalles de los documentos sin haber consultado inicialmente los detalles y sus ID's
				' Julian Rincón (Alcuadrado S.A)
				'If logXTesorero = False Then
				'    RecorrerDetallesDocumento()
				'End If

			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la actualización de Datos.", Me.ToString, "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			Editando = False
			MyBase.CambioItem("Editando")
			IsBusy = False
			logGuardandoOrden = False
		End Try
	End Sub

	Private Sub TerminoMensajeResultadoAsincronico(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim objResultado = CType(sender, A2Utilidades.wcpMensajes)

			If Not IsNothing(objResultado) Then
				If Not IsNothing(objResultado.CodigoLlamado) Then
					Select Case objResultado.CodigoLlamado.ToUpper
						Case "TERMINOGUARDAR"
							HabilitarReceptor = False
							HabilitarTipoProducto = False
							HabilitarBuscadorCliente = False
							HabilitarEntregar = False
							HabilitarEnEdicion = False
							HabilitarFecha = False
							HabilitarImportacion = False
							HabilitarCategoriaFondos = False
							If logXTesorero = True Then
								VerGrabarXTesorero = Visibility.Collapsed
								VerCancelarXTesorero = Visibility.Collapsed
								VerEditarXTesorero = Visibility.Visible
								If Not IsNothing(TesoreriaOrdenesPlusRC_Selected.lngID) Then
									dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(TesoreriaOrdenesPlusRC_Selected.lngID, GSTR_OT, Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
								End If
							Else

								'TesoreriaOrdenesPlusRC_Selected = Nothing
								If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
									ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Clear()
								End If

								If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
									ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Clear()
								End If

								If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
									ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Clear()
								End If

								If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
									ListatesoreriaordenesplusRC_Detalle_Consignaciones.Clear()
								End If
								If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
									ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones.Clear()
								End If
								HabilitarEntregarA = False
								HabilitarEntregar = False

								If Not IsNothing(dcProxy.TesoreriaOyDPlusChequesRecibos) Then
									dcProxy.TesoreriaOyDPlusChequesRecibos.Clear()
								End If
								If Not IsNothing(dcProxy.TesoreriaOyDPlusTransferenciasRecibos) Then
									dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Clear()
								End If
								If Not IsNothing(dcProxy.TesoreriaOyDPlusCargosPagosARecibos) Then
									dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Clear()
								End If
								If Not IsNothing(dcProxy.TesoreriaOyDPlusConsignacionesRecibos) Then
									dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Clear()
								End If
								If Not IsNothing(dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos) Then
									dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Clear()
								End If

								'Modificado por Juan David Correa.
								'Se obtiene el id del selected al momento de guardar la orden.
								'******************************************************************************************************************
								Dim strEstadoGuardar As String = String.Empty

								If TesoreriaOrdenesPlusRC_Selected.lngID = 0 Then
									intIDOrdenTesoreria = 0
									strEstadoGuardar = "TERMINOGUARDARNUEVO"
								Else
									intIDOrdenTesoreria = TesoreriaOrdenesPlusRC_Selected.lngID
									strEstadoGuardar = "TERMINOGUARDAREDITAR"
								End If

								If Not IsNothing(TesoreriaOrdenesPlusRC_Selected.lngID) Then
									dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(TesoreriaOrdenesPlusRC_Selected.lngID, GSTR_OT, Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
								End If

								If Not IsNothing(ListaTesoreriaOrdenesPlusRC) Then
									ListaTesoreriaOrdenesPlusRC.Clear()
									ListaTesoreriaOrdenesPlusRC = Nothing
								End If
								dcProxy.TesoreriaOrdenesEncabezados.Clear()

								logRecargarPantalla = False
								MyBase.QuitarFiltroDespuesGuardar()

								If CantidadAprobaciones > 0 Then
									VistaSeleccionada = GSTR_POR_APROBAR
									TraerOrdenes(strEstadoGuardar, GSTR_POR_APROBAR)
								Else
									VistaSeleccionada = GSTR_PENDIENTES
									TraerOrdenes(strEstadoGuardar, GSTR_PENDIENTES)
								End If

								'dcProxy.Load(dcProxy.TesoreriaOrdenesListarQuery("", "", "", "", 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenes, strEstadoGuardar);
								'******************************************************************************************************************
							End If

							Editando = False
							MyBase.CambioItem("Editando")
							VerDuplicar = Visibility.Visible
							logDuplicarRegistro = False
							CambiarColorFondoTextoBuscador()

							'Se realiza consulta de documentos despues de consultar los detalles y sus ID'S
							' Julian Rincón (Alcuadrado S.A)
							'If logXTesorero = False Then
							'    RecorrerDetallesDocumento()
							'End If
					End Select
				End If
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje asincronico.", Me.ToString(), "TerminoMensajeResultadoAsincronico", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	Public Function ValidarGuardadoOrden(ByVal pobjOrden As OyDPLUSTesoreria.TesoreriaOrdenesEncabezado) As Boolean
		Try
			'Valida los campos que son requeridos por el sistema de OYDPLUS.
			Dim logTieneError As Boolean = False
			strMensajeValidacion = String.Empty

			If Not IsNothing(pobjOrden) Then
				'If logCaracterInvalido = False Then
				Dim objvalidacion = clsExpresiones.ValidarCaracteresEnCadena(_TesoreriaOrdenesPlusRC_Selected.strNombre, clsExpresiones.TipoExpresion.Caracteres2)


				If Not objvalidacion.TextoValido Then

					strMensajeValidacion = String.Format("{0}{1} - Nombre: Verifique que el nombre tenga caracteres válidos", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
				'Valida el campo de Receptor
				If logNuevoRegistro Then
					If IsNothing(pobjOrden.strCodigoReceptor) Or String.IsNullOrEmpty(pobjOrden.strCodigoReceptor) Then
						strMensajeValidacion = String.Format("{0}{1} - Receptor", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If
				End If

				'Valida el campo de Tipo de producto
				If IsNothing(pobjOrden.ValorTipoProducto) Or String.IsNullOrEmpty(pobjOrden.ValorTipoProducto) Then
					strMensajeValidacion = String.Format("{0}{1} - Tipo producto", strMensajeValidacion, vbCrLf)
					logTieneError = True
				ElseIf pobjOrden.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					If String.IsNullOrEmpty(TipoAccionFondos) Then
						strMensajeValidacion = String.Format("{0}{1} - Tipo acción", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If
					If String.IsNullOrEmpty(CarteraColectivaFondos) Then
						strMensajeValidacion = String.Format("{0}{1} - Cartera colectiva", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If
				End If

				'Valida el campo fecha de recepción
				If IsNothing(FechaOrden) Then
					strMensajeValidacion = String.Format("{0}{1} - Fecha Orden", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If

				'Valida el tipo de cliente
				If String.IsNullOrEmpty(_IDTipoClienteEntregado) Then
					strMensajeValidacion = String.Format("{0}{1} - Tipo de cliente", strMensajeValidacion, vbCrLf)
					logTieneError = True
				Else
					If _IDTipoClienteEntregado = GSTR_CLIENTE Then
						If String.IsNullOrEmpty(pobjOrden.strIDComitente) Then
							strMensajeValidacion = String.Format("{0}{1} - Codigo OYD", strMensajeValidacion, vbCrLf)
							logTieneError = True
						End If
					End If

					If String.IsNullOrEmpty(pobjOrden.strNombre) Then
						strMensajeValidacion = String.Format("{0}{1} - Nombre", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If

					If String.IsNullOrEmpty(pobjOrden.strNroDocumento) Then
						strMensajeValidacion = String.Format("{0}{1} - Nro de documento", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If

					If String.IsNullOrEmpty(pobjOrden.strTipoIdentificacion) Then
						strMensajeValidacion = String.Format("{0}{1} - Tipo de identificación", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If
				End If
			Else
				mostrarMensaje("Señor Usuario, la orden tiene que tener un dato como minimo.", "Ordenes Tesoreria ", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Return False
			End If

			If logTieneError Then
				strMensajeValidacion = String.Format("Por favor corrija las siguientes inconsistencias antes de guardar:{0}{1}", vbCrLf, strMensajeValidacion)
				mostrarMensaje(strMensajeValidacion, "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Return False
			Else
				'Valida la cantidad maxima de detalles
				If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					If _ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > intCantidadMaximaDetalles Then
						mostrarMensaje(String.Format("Señor Usuario, la cantidad máxima de registros permitida por detalle es ({0}), por favor valide el detalle de cheques.", intCantidadMaximaDetalles), "Ordenes Tesoreria ", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						Return False
					End If
				End If

				If Not IsNothing(_ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					If _ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > intCantidadMaximaDetalles Then
						mostrarMensaje(String.Format("Señor Usuario, la cantidad máxima de registros permitida por detalle es ({0}), por favor valide el detalle de transferencias.", intCantidadMaximaDetalles), "Ordenes Tesoreria ", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						Return False
					End If
				End If

				If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					If _ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > intCantidadMaximaDetalles Then
						mostrarMensaje(String.Format("Señor Usuario, la cantidad máxima de registros permitida por detalle es ({0}), por favor valide el detalle de consignaciones.", intCantidadMaximaDetalles), "Ordenes Tesoreria ", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						Return False
					End If
				End If

				If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
					If _ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > intCantidadMaximaDetalles Then
						mostrarMensaje(String.Format("Señor Usuario, la cantidad máxima de registros permitida por detalle es ({0}), por favor valide el detalle de cargar pagos a.", intCantidadMaximaDetalles), "Ordenes Tesoreria ", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						Return False
					End If
				End If

				If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
					If _ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > intCantidadMaximaDetalles Then
						mostrarMensaje(String.Format("Señor Usuario, la cantidad máxima de registros permitida por detalle es ({0}), por favor valide el detalle de cargar pagos a fondos.", intCantidadMaximaDetalles), "Ordenes Tesoreria ", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						Return False
					End If
				End If

				If logEsFondosOYD Then
					If Not IsNothing(FechaAplicacion) Then
						If TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
							If FechaAplicacion.Value.Date < FechaOrden.Date Then
								mostrarMensaje("La fecha de aplicación no puede ser menor a la fecha de orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
								Return False
							ElseIf Not IsNothing(dtmFechaMenorPermitidaIngreso) Then
								If FechaAplicacion.Value.Date < DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, dtmFechaMenorPermitidaIngreso.Value.Date) Then
									mostrarMensaje(String.Format("La fecha de aplicación no puede ser menor a la fecha de cierre + los días de aplicación {0:yyyy-MM-dd}", DateAdd(DateInterval.Day, intDiasAplicacionFondosAdicion, dtmFechaMenorPermitidaIngreso.Value.Date)), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
									Return False
								End If
							End If

							If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
								For Each li In _ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
									If String.IsNullOrEmpty(li.intNroEncargoFondos) Then
										mostrarMensaje("Para todos los detalles de Cargar Pago A Fondos es necesario indicar el Nro del Encargo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
										Return False
									End If
								Next
							End If
						ElseIf TipoAccionFondos = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
							If FechaAplicacion.Value.Date < FechaOrden.Date Then
								mostrarMensaje("La fecha de aplicación no puede ser menor a la fecha de orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
								Return False
							ElseIf Not IsNothing(dtmFechaMenorPermitidaIngreso) Then
								If FechaAplicacion.Value.Date < DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, dtmFechaMenorPermitidaIngreso.Value.Date) Then
									mostrarMensaje(String.Format("La fecha de aplicación no puede ser menor a la fecha de cierre + los días de aplicación {0:yyyy-MM-dd}", DateAdd(DateInterval.Day, intDiasAplicacionFondosApertura, dtmFechaMenorPermitidaIngreso.Value.Date)), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
									Return False
								End If
							End If

							If Not IsNothing(_ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
								For Each li In _ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
									If Not String.IsNullOrEmpty(li.intNroEncargoFondos) Then
										li.strTipoAccionFondos = GSTR_FONDOS_TIPOACCION_CONSTITUCION
										li.intNroEncargoFondos = Nothing
									End If
								Next
							End If
						End If
					End If
				End If

				strMensajeValidacion = String.Empty
				Return True
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la valición de la orden.", Me.ToString, "ValidarGuardadoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Sub ValidarEstadoOrdenServidor(plngIDOrden As Integer, pstrEstadoOrdenCliente As String)
		Try
			dcProxy.TempValidarEstadoOrdens.Clear()
			dcProxy.Load(dcProxy.ValidarEstadoOrdenTesoreriaEncabezadoQuery(TesoreriaOrdenesPlusRC_Selected.lngID.ToString(), TesoreriaOrdenesPlusRC_Selected.ValorEstado, Program.Usuario, "E", Program.HashConexion), AddressOf TerminoValidarEstadoTesoreria, "")

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al Validar Estado de la Orden de Tesoreria.", Me.ToString, "ValidarEstadoOrdenServidor", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub ValidarEstadoOrdenDetalleServidor(plngIDOrden As Integer, pstrEstadoOrdenCliente As String, pstrFormaPago As String)
		Try
			dcProxy.TempValidarEstadoOrdens.Clear()
			dcProxy.Load(dcProxy.ValidarEstadoOrdenTesoreriaEncabezadoQuery(plngIDOrden.ToString(), pstrEstadoOrdenCliente.Substring(0, 1), Program.Usuario, "D", Program.HashConexion), AddressOf TerminoValidarEstadoTesoreriaDetalle, pstrFormaPago)

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al Validar Estado de la Orden de Tesoreria.", Me.ToString, "ValidarEstadoOrdenDetalleServidor", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Async Sub EditarRegistro()
		Try
			If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
			End If

			VerDuplicar = Visibility.Collapsed
			logBuscar = False
			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
				IsBusy = True

				dtmFechaServidor = Await A2OYDPLUSUtilidades.FuncionesCompartidas.obtenerFechaServidor()

				If (TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTE_Plus _
					And TesoreriaOrdenesPlusRC_Selected.dtmDocumento >= dtmFechaServidor.Date) _
				Or (TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_PENDIENTE_Plus _
					And TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO) _
				Or (TesoreriaOrdenesPlusRC_Selected.ValorEstado = GSTR_FUTURA_Plus _
					And TesoreriaOrdenesPlusRC_Selected.dtmDocumento = dtmFechaServidor.Date) Then
					If TesoreriaOrdenesPlusRC_Selected.lngID > 0 Then

						If TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							If logEsFondosOYD Then
								HabilitarConceptoDetalles = False
							Else
								HabilitarConceptoDetalles = True
							End If
						End If

						FechaOrden = dtmFechaServidor
						'Valida el estado de la Orden de Tesoreria en el Servidor que No haya sido actualizada
						ValidarEstadoOrdenServidor(TesoreriaOrdenesPlusRC_Selected.lngID, TesoreriaOrdenesPlusRC_Selected.ValorEstado)
					End If
				Else
					IsBusy = False
					Dim strMensaje = ""

					mostrarMensaje("La Orden de recaudo no se puede editar por alguna de las siguientes razones: " & vbCrLf & vbCrLf &
								   "-El estado de la orden es diferente a un estado pendiente." & vbCrLf &
								   "-El estado de la orden es futura pero la fecha es diferente a la actual." & vbCrLf &
								   "-La orden es de tipo producto fondos y tiene fecha menor a la fecha actual.", "Editar Registro.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					logEditarRegistro = False
					logNuevoRegistro = False
					logDuplicarRegistro = False
					VerDuplicar = Visibility.Visible
					MyBase.RetornarValorEdicionNavegacion()
					Editando = False
					MyBase.CambioItem("Editando")

				End If

			Else
				MyBase.RetornarValorEdicionNavegacion()
				IsBusy = False
				mostrarMensaje("No es posible Editar asegurese que existan registros en la lista", "Editar Registro", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				logEditarRegistro = False
				logNuevoRegistro = False
				Editando = False
				logDuplicarRegistro = False
				MyBase.CambioItem("Editando")
			End If

			ListaArchivoConsignacion = Nothing
			ListaArchivosCheque = Nothing
			ListaArchivoTransferencia = Nothing

			HabilitarCategoriaFondos = False
			CambiarColorFondoTextoBuscador()

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro",
								 Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Public Sub Instrucciones()
		Try
			If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) Then
				IDTipoClienteEntregado = _TesoreriaOrdenesPlusRC_Selected.strTipoCliente
				strNombreEntregado = _TesoreriaOrdenesPlusRC_Selected.strNombre_Instrucciones
				strNroDocumentoEntregado = _TesoreriaOrdenesPlusRC_Selected.strNroDocumento_Instrucciones
				strTipoIdentificacionEntregado = _TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion_Instrucciones
				'LimpiarInscripcion()
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar las instrucciones.",
								 Me.ToString(), "Instrucciones", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Public Sub LimpiarInscripcion()
		Try
			logEsCtaInscrita_Instrucciones = False
			logDireccionInscrita_Instrucciones = False
			RecogeTercero = False
			VerCuentasRegistradas = Visibility.Collapsed
			VerLlevarDireccion = Visibility.Collapsed
			ClientePresente = False
			logEsTercero_Instrucciones = False

			strTipoIdentificacionInstrucciones = String.Empty
			strNroDocumentoInstrucciones = String.Empty
			strNombreInstrucciones = String.Empty
			strSectorInstrucciones = String.Empty

			strCodigoBancoInstrucciones = String.Empty
			strCuentaInstrucciones = String.Empty
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar las instrucciones.",
								 Me.ToString(), "LimpiarInscripcion", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Sub CambiarAForma()
		Try
			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
				'TesoreriaOrdenesPlusAnterior = TesoreriaOrdenesPlusRC_Selected
				ConsultarSaldo = True
				HabilitarEnEdicion = False
				HabilitarFecha = False
				HabilitarEncabezado = False
				VerInstrucciones = Visibility.Visible
				CalcularTotales(GSTR_CHEQUE)
				VerificarPrimerTabHabilitado()
			End If

			MyBase.CambiarAForma()

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar a la forma",
								 Me.ToString(), "CambiarAForma", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Sub CancelarEditarRegistro()
		Try
			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
				dcProxy.RejectChanges()
				VerDuplicar = Visibility.Visible
				logEditarCargarPagosA = False
				logEditarChequeRC = False
				logEditarConsignacion = False
				logEditarTransferenciaRC = False
				HabilitarEntregarA = False
				HabilitarBuscadorCliente = False
				HabilitarEntregar = False
				HabilitarCategoriaFondos = False

				logEditarRegistro = False
				logDuplicarRegistro = False

				logBuscar = False
				HabilitarTipoProducto = False
				logNuevoRegistro = False
				logCancelarRegistro = True
				HabilitarImportacion = False
				HabilitarReceptor = False
				HabilitarEnEdicion = False
				HabilitarFecha = False
				HabilitarInstrucciones = False
				HabilitarEncabezado = False
				ErrorForma = ""
				dcProxy.TesoreriaOyDPlusChequesRecibos.Clear()
				dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Clear()
				dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Clear()
				dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Clear()
				dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Clear()

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = Nothing
				End If
				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = Nothing
				End If
				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
					ListatesoreriaordenesplusRC_Detalle_CargarPagosA = Nothing
				End If
				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					ListatesoreriaordenesplusRC_Detalle_Consignaciones = Nothing
				End If
				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
					ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones = Nothing
				End If
				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
					ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = Nothing
				End If

				If Not IsNothing(TesoreriaOrdenesPlusAnterior) Then
					If TesoreriaOrdenesPlusAnterior.lngID > 0 Then
						'TesoreriaOrdenesPlusRC_Selected = TesoreriaOrdenesPlusAnterior
						If TesoreriaOrdenesPlusRC_Selected.EntityState = EntityState.Detached Then
							TesoreriaOrdenesPlusRC_Selected = TesoreriaOrdenesPlusAnterior

						End If

						FechaOrden = TesoreriaOrdenesPlusAnterior.dtmDocumento
						FechaAplicacion = TesoreriaOrdenesPlusAnterior.dtmFechaAplicacion

						If ConsultarSaldo Then
							ConsultarSaldo = False
						End If
						ConsultarSaldo = True
						TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = TesoreriaOrdenesPlusAnterior.ValorTipoProducto
						TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor = TesoreriaOrdenesPlusAnterior.strCodigoReceptor
						TesoreriaOrdenesPlusRC_Selected.strEstado = TesoreriaOrdenesPlusAnterior.strEstado
						TesoreriaOrdenesPlusRC_Selected.strNroDocumento = TesoreriaOrdenesPlusAnterior.strNroDocumento
						TesoreriaOrdenesPlusRC_Selected.curValor = TesoreriaOrdenesPlusAnterior.curValor
						TesoreriaOrdenesPlusRC_Selected.strCuentaCliente = TesoreriaOrdenesPlusAnterior.strCuentaCliente
						TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion = TesoreriaOrdenesPlusAnterior.strTipoIdentificacion

						TesoreriaOrdenesPlusRC_Selected.strEstado = TesoreriaOrdenesPlusAnterior.strEstado
						TesoreriaOrdenesPlusRC_Selected.strNombre = TesoreriaOrdenesPlusAnterior.strNombre
						TesoreriaOrdenesPlusRC_Selected.lngNroDocumento = TesoreriaOrdenesPlusAnterior.lngNroDocumento
						TesoreriaOrdenesPlusRC_Selected.lngIDDocumento = TesoreriaOrdenesPlusAnterior.lngIDDocumento
						TesoreriaOrdenesPlusRC_Selected.strIDComitente = TesoreriaOrdenesPlusAnterior.strIDComitente
						CodigoOYDControles = TesoreriaOrdenesPlusAnterior.strIDComitente


						TesoreriaOrdenesPlusRC_Selected.logClientePresente = TesoreriaOrdenesPlusAnterior.logClientePresente
						TesoreriaOrdenesPlusRC_Selected.logClienteRecoge = TesoreriaOrdenesPlusAnterior.logClienteRecoge
						TesoreriaOrdenesPlusRC_Selected.logRecogeTercero = TesoreriaOrdenesPlusAnterior.logRecogeTercero
						TesoreriaOrdenesPlusRC_Selected.logConsignarCta = TesoreriaOrdenesPlusAnterior.logConsignarCta
						TesoreriaOrdenesPlusRC_Selected.logllevarDireccion = TesoreriaOrdenesPlusAnterior.logllevarDireccion
						TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion_Instrucciones = TesoreriaOrdenesPlusAnterior.strTipoIdentificacion_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.strNroDocumento_Instrucciones = TesoreriaOrdenesPlusAnterior.strNroDocumento_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.strNombre_Instrucciones = TesoreriaOrdenesPlusAnterior.strNombre_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.logDireccionInscrita_Instrucciones = TesoreriaOrdenesPlusAnterior.logDireccionInscrita_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.strCiudad_Instrucciones = TesoreriaOrdenesPlusAnterior.strCiudad_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.strSector_Instrucciones = TesoreriaOrdenesPlusAnterior.strSector_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.logEsTercero_Instrucciones = TesoreriaOrdenesPlusAnterior.logEsTercero_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.logEsCtaInscrita_Instrucciones = TesoreriaOrdenesPlusAnterior.logEsCtaInscrita_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.strTipoCta_Instrucciones = TesoreriaOrdenesPlusAnterior.strTipoCta_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.strCuenta_Instrucciones = TesoreriaOrdenesPlusAnterior.strCuenta_Instrucciones
						TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = TesoreriaOrdenesPlusAnterior.strTipoRetiroFondos
						TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos = TesoreriaOrdenesPlusAnterior.strCarteraColectivaFondos
						LlamarDetalle()


						VerCargarPagosA = Visibility.Collapsed
						VerCargarPagosAFondos = Visibility.Collapsed
						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							VerDatosFondos = Visibility.Visible
						Else
							VerDatosFondos = Visibility.Collapsed
						End If
						Instrucciones()
						dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(TesoreriaOrdenesPlusRC_Selected.lngID, GSTR_OT, Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
						Editando = False
						If logXTesorero Then
							VerGrabarXTesorero = Visibility.Collapsed
							VerCancelarXTesorero = Visibility.Collapsed
							VerEditarXTesorero = Visibility.Visible
						End If
						'If ValorDisponibleCargarPago <> 0 Then
						'    Editando = True

						'    HabilitarEnEdicion = True
						'    mostrarMensaje("La orden de recibo siempre debe tener pago(s) distribuido(s) a un código OyD o a varios, por favor ingresar un registro en Cargar Pagos A.", "Cancelando Edición", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						'Else
						'    
						'End If
					Else
						Dim xDefecto As New TesoreriaOrdenesEncabezado
						xDefecto.strNombre = String.Empty
						xDefecto.lngNroDocumento = 0
						xDefecto.strIDComitente = Nothing
						xDefecto.strEstado = String.Empty
						xDefecto.strNroDocumento = String.Empty
						xDefecto.curValor = 0
						xDefecto.strCuentaCliente = String.Empty
						xDefecto.strTipoIdentificacion = String.Empty
						xDefecto.strTipoProducto = String.Empty
						xDefecto.strCodigoReceptor = String.Empty
						xDefecto.strNombreConsecutivo = String.Empty
						xDefecto.dtmActualizacion = dtmFechaServidor.Date
						xDefecto.dtmDocumento = dtmFechaServidor.Date
						xDefecto.strTipo = GSTR_ORDENRECIBO
						xDefecto.strUsuario = Program.Usuario

						strNombreEntregado = String.Empty
						strNroDocumentoEntregado = String.Empty
						strTipoIdentificacionEntregado = Nothing
						IDTipoClienteEntregado = Nothing

						TesoreriaOrdenesPlusRC_Selected = xDefecto
						TesoreriaOrdenesPlusRC_Selected.strNombre = xDefecto.strNombre
						TesoreriaOrdenesPlusRC_Selected.lngNroDocumento = xDefecto.lngNroDocumento
						TesoreriaOrdenesPlusRC_Selected.lngIDDocumento = xDefecto.lngIDDocumento
						TesoreriaOrdenesPlusRC_Selected.strIDComitente = xDefecto.strIDComitente

						TesoreriaOrdenesPlusRC_Selected.strEstado = xDefecto.strEstado
						TesoreriaOrdenesPlusRC_Selected.strNroDocumento = xDefecto.strNroDocumento
						TesoreriaOrdenesPlusRC_Selected.curValor = xDefecto.curValor
						TesoreriaOrdenesPlusRC_Selected.strCuentaCliente = xDefecto.strCuentaCliente
						TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion = xDefecto.strTipoIdentificacion
						TesoreriaOrdenesPlusRC_Selected.strTipoProducto = xDefecto.strTipoProducto
						TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor = xDefecto.strCodigoReceptor
						TesoreriaOrdenesPlusRC_Selected.ValorTipo = GSTR_ORDENRECIBO
						If BorrarCliente Then
							BorrarCliente = False
						End If
						BorrarCliente = True
						Editando = False
					End If
				End If
			Else
				Editando = False
			End If


			If logCancelarRegistro Then
				HabilitarDocumento = False
			End If
			CambiarColorFondoTextoBuscador()

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
					 Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Sub BorrarRegistro()
		Try
			If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
			End If

			If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) Then
				mostrarMensajePregunta("¿Está Seguro que desea eliminar el Registro?",
									   Program.TituloSistema,
									   "BORRARREGISTRO",
									   AddressOf TerminoPreguntarBorrarRegistro, False)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
								 Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub TerminoPreguntarBorrarRegistro(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
			If objResultado.DialogResult Then
				IsBusy = True
				dcProxy.TesoreriaOrdenesEncabezado_Elminar(_TesoreriaOrdenesPlusRC_Selected.lngID, Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarEncabezado, "")
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarBorrarRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Overrides Sub QuitarFiltro()
		Try
			logFiltrar = False
			logRecargarPantalla = False
			VistaSeleccionada = GSTR_PENDIENTES
			logRecargarPantalla = True
			MyBase.QuitarFiltro()
			Filtro = 0
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al quitar el filtro.", Me.ToString(), "QuitarFiltro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub LlamarDetalle()
		Try
			IsBusyDetalles = True

			dcProxy.TesoreriaOyDPlusChequesRecibos.Clear()
			dcProxy.TesoreriaOyDPlusTransferenciasRecibos.Clear()
			dcProxy.TesoreriaOyDPlusCarterasColectivas.Clear()
			dcProxy.TesoreriaOyDPlusCargosPagosARecibos.Clear()
			dcProxy.TesoreriaOyDPlusConsignacionesRecibos.Clear()
			dcProxy.TesoreriaOyDPlusCargosPagosAFondosRecibos.Clear()

			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
				ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = Nothing
			End If
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
				ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = Nothing
			End If
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
				ListatesoreriaordenesplusRC_Detalle_CargarPagosA = Nothing
			End If
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
				ListatesoreriaordenesplusRC_Detalle_Consignaciones = Nothing
			End If
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
				ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones = Nothing
			End If
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
				ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = Nothing
			End If

			If logRecargar Then
				Thread.Sleep(3000)
				logRecargar = False
			End If

			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
				If TesoreriaOrdenesPlusRC_Selected.lngID > 0 Then
					dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_TransferenciasReciboQuery(TesoreriaOrdenesPlusRC_Selected.lngID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleTransferencias, "CAMBIOSELECTED")
				Else

				End If
			Else
				IsBusyDetalles = False
			End If

		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras se consultaba el Detalle de la Orden ", Me.ToString(), "LlamarDetalle", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	Public Sub ValidarEncabezado()
		Try

			If TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor <> String.Empty And TesoreriaOrdenesPlusRC_Selected.strNombre <> String.Empty _
				 And TesoreriaOrdenesPlusRC_Selected.strNroDocumento <> String.Empty Then

				logHayEncabezado = True
				HabilitarEnEdicion = True
				'HabilitarFecha = True 'JABG20180412
			Else
				'HabilitarFecha = False 'JABG20180412
				logHayEncabezado = False
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras al Validar datos de Encabezado", Me.ToString(), "ValidarEncabezado", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub
	Sub CargarConceptosPorDefecto()
		Try
			If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD Then
				If _TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
					If Not IsNothing(ConceptoDefectoFondos_Adicion) Then
						IdConceptoFondos = ConceptoDefectoFondos_Adicion
						DescripcionComboConceptoFondos = ConceptoDescripcionDefectoFondos_Adicion
					End If
				ElseIf _TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
					If Not IsNothing(ConceptoDefectoFondos_Apertura) Then
						IdConceptoFondos = ConceptoDefectoFondos_Apertura
						DescripcionComboConceptoFondos = ConceptoDescripcionDefectoFondos_Apertura
					End If
				Else
					IdConceptoFondos = Nothing
				End If
			Else
				If Not IsNothing(DiccionarioCombosOYDPlus) Then
					If DiccionarioCombosOYDPlus.ContainsKey("CONCEPTOS") Then
						If DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "CAJA").Count = 1 _
							And DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "TODOS").Count = 0 Then
							If TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
								IdConceptoFondos = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "CAJA").FirstOrDefault.IntId
							Else
								IdConcepto = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "CAJA").FirstOrDefault.IntId
							End If
						ElseIf DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "CAJA").Count = 0 _
							And DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "TODOS").Count = 1 Then
							If TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
								IdConceptoFondos = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "TODOS").FirstOrDefault.IntId
							Else
								IdConcepto = DiccionarioCombosOYDPlus("CONCEPTOS").Where(Function(i) i.Retorno.ToUpper = "TODOS").FirstOrDefault.IntId
							End If
						Else
							IdConcepto = Nothing
							IdConceptoFondos = Nothing
						End If
					End If
				End If
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el concepto por defecto",
												 Me.ToString(), "CargarConceptosPorDefecto", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	Sub ValoresXdefectoCargarPagosAWpp()
		Try
			strCodigoOyDCargarPagosA = String.Empty
			strNombreCodigoOyD = String.Empty
			IDTipoClienteCargarPagosA = String.Empty
			TipoIdClienteDescripcion = String.Empty
			DescripcionComboConcepto = String.Empty
			DescripcionConcepto = String.Empty
			IdConcepto = 0
			ValorCargarPagoA = 0
			IDTipoClienteCargarPagosA = String.Empty
			NroEncargoFondos = Nothing
			LlevarSaldoDisponibleCargarPagosA = False
			CargarConceptosPorDefecto()
			ConsultarComitentesClienteEncabezado(TesoreriaOrdenesPlusRC_Selected.strIDComitente)
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras al Validar datos de Encabezado", Me.ToString(), "ValoresXdefectoCargarPagosAWpp", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub
	Sub ValoresXdefectoCargarPagosAFondosWpp()
		Try
			If _TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
				If logEsFondosOYD = False Then
					If Not IsNothing(_ListaCarterasColectivasClienteCompleta) Then
						Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
						For Each li In _ListaCarterasColectivasClienteCompleta
							If li.CarteraColectiva = _CarteraColectivaFondos Then
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

						ListaEncargosCarteraColectiva = objNuevaListaEncargos
					Else
						ListaEncargosCarteraColectiva = Nothing
					End If
				End If
			Else
				ListaEncargosCarteraColectiva = Nothing
			End If

			IDTipoClienteCargarPagosAFondos = Nothing

			If DiccionarioCombosOYDPlus.ContainsKey("TIPOCLIENTECARGARPAGOSFONDOS") Then
				If DiccionarioCombosOYDPlus("TIPOCLIENTECARGARPAGOSFONDOS").Count = 1 Then
					IDTipoClienteCargarPagosAFondos = DiccionarioCombosOYDPlus("TIPOCLIENTECARGARPAGOSFONDOS").First.Retorno
				End If
			End If

			strCodigoOyDCargarPagosAFondos = String.Empty

			DescripcionComboConceptoFondos = String.Empty
			strNombreCodigoOyDFondos = String.Empty
			IdConceptoFondos = 0
			ValorCargarPagoAFondos = 0
			NroEncargoFondos = Nothing
			LlevarSaldoDisponibleCargarPagosAFondos = False
			DescripcionEncargoFondos = String.Empty
			DescripcionConceptoFondos = String.Empty

			CargarConceptosPorDefecto()

			If Not String.IsNullOrEmpty(strConceptoDefecto_Fondos) Then
				DescripcionConceptoFondos = strConceptoDefecto_Fondos
			End If

			ConsultarComitentesClienteEncabezado(TesoreriaOrdenesPlusRC_Selected.strIDComitente)

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras al Validar datos de Encabezado", Me.ToString(), "ValoresXdefectoCargarPagosAWpp", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Sub ValoresXdefectoWpp()
		Try
			HabilitarEnEdicion = True
			ExpresionRegularNroDocumento = "black"
			IdBanco = 0
			lngNroCheque = 0
			CheckClientetraeCheque = True
			CheckDireccionCliente = False
			CheckOtraDireccion = False
			strDireccionCheque = String.Empty
			strCiudad = String.Empty
			strTelefono = String.Empty
			ValorGenerar = 0
			ValorGenerar = Nothing
			ValorGenerar = Nothing

			BancoChequewpp = String.Empty
			strSector = String.Empty
			HabilitarCamposPopup = False
			HabilitarCampoBeneficiario = False
			HabilitarCampoNroDocumento = False
			'CargarCombosOYDPLUS(TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor, "ValoresXdefectoWpp")
			liquidacionesSelecciondas = String.Empty
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras se establecian los Valores por defectos", Me.ToString(), "ValoresXdefectoWpp", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Sub ValoresXdefectoWppConsignacion()
		Try
			HabilitarEnEdicion = True
			lngCodigoBancoWpp = 0
			lngNroChequeConsignacion = 0
			lngNroReferencia = 0
			strCuentaConsignacion = String.Empty
			strTipoPagoConsignacion = String.Empty
			lngCodigoBancoConsignacionWpp = 0
			lngCodigoBancoDestinoConsignacionWpp = Nothing
			ValorGenerarConsignacion = 0
			strDescripcionBancoConsignacion = String.Empty
			HabilitarEncabezadoConsignacion = True
			HabilitarEncabezadoConsignacionReferencia = True
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
				ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones.Clear()
			End If
			strDescripcionCuentaConsignacion = String.Empty
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras se establecian los Valores por defecto Transferencias", Me.ToString(), "ValoresXdefectoWppTransferencia", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Sub ValoresXdefectoWppTransferencia()
		Try
			HabilitarEnEdicion = True
			lngCodigoBancoWpp = 0
			lngCodigoBancoDestinoTransferenciaWpp = Nothing
			strNroCuentaWpp = String.Empty
			strNroCuentaDestinoWpp = String.Empty
			DescripcionTipoCuentaDestinoTransferencia = String.Empty
			DescripcionTipoCuentaTransferencia = String.Empty
			strValorTipoCuentaWpp = String.Empty
			strValorTipoCuentaDestinoWpp = String.Empty
			ExpresionRegularNroDocumento = "black"
			strValorTipoCuentaWpp = String.Empty
			strNroCuentaWpp = String.Empty
			HabilitarComboCuentasCliente = False
			VerComboCuentasRegistradas = Visibility.Collapsed
			strNroDocumentoTitularWpp = String.Empty
			DescripcionConceptoTransferencia = String.Empty
			ValorGenerarTransferencia = Nothing
			lngCodigoBancoWpp = Nothing
			IdTipoCuentaRegistrada = Nothing
			strNombreTitularWpp = String.Empty
			strValorTipoCuentaWpp = Nothing
			strTipoIdentificacionTitularWpp = Nothing
			CuentaRegistrada = Nothing

			BancoTransferenciaDescripcionDestinowpp = String.Empty
			DescripcionBancoTransferencia = String.Empty
			'CargarCombosOYDPLUS(TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor, "ValoresXdefectoWppTransferencia")
			liquidacionesSelecciondas = String.Empty
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras se establecian los Valores por defecto Transferencias", Me.ToString(), "ValoresXdefectoWppTransferencia", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Sub CancelarTransferenciawpp()
		Try
			logEditarTransferenciaRC = False
			ValoresXdefectoWppTransferencia()
			objWppTransferencia_RC.Close()
			If Not IsNothing(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected) Then
				If TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID > 0 Then
					dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
				End If
			End If

			BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemTransferencia")
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar Transferencia", Me.ToString(), "CancelarTransferenciawpp", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Sub CancelarConsignacionwpp()
		Try
			logEditarConsignacion = False
			ValoresXdefectoWppConsignacion()
			objWppConsignacion.Close()
			If Not IsNothing(TesoreriaordenesplusRC_Detalle_Consignaciones_selected) Then
				If TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID > 0 Then
					dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
				End If
			End If

			BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemConsignacion")
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar Consignacion", Me.ToString(), "CancelarConsignacionwpp", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Sub CancelarChequewpp()
		Try
			logEditarChequeRC = False
			ValoresXdefectoWpp()
			objWppChequeRC.Close()
			If Not IsNothing(TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected) Then
				If TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID > 0 Then
					dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
				End If
			End If

			BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemCheque")
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar Cheque", Me.ToString(), "CancelarChequewpp", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Private Sub DefineCommands()

		Try
			'''''''''''''''''''''''''''''''''''''''''CHEQUES'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			NuevoCheque = New DelegateCommandGenerico(Of String)(AddressOf AbrirPopup, AddressOf PuedeEjecutar)
			EditarWppCheque = New DelegateCommandGenerico(Of String)(AddressOf EditarWppSubCheque, AddressOf PuedeEjecutar)
			GuardarCheque = New DelegateCommandGenerico(Of String)(AddressOf GuardarChequewpp, AddressOf PuedeEjecutar)
			GuardarContinuar = New DelegateCommandGenerico(Of String)(AddressOf GuardarChequeContinuarwpp, AddressOf PuedeEjecutar)
			BorrarCheque = New DelegateCommandGenerico(Of String)(AddressOf BorrarChequewpp, AddressOf PuedeEjecutar)
			CancelarGrabacion = New DelegateCommandGenerico(Of String)(AddressOf CancelarChequewpp, AddressOf PuedeEjecutar)
			''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

			'''''''''''''''''''''''''''''''''''''''''Transferencias'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			NuevaTransferencia = New DelegateCommandGenerico(Of String)(AddressOf AbrirPopupTransferencia, AddressOf PuedeEjecutar)
			EditarWppTransferencia = New DelegateCommandGenerico(Of String)(AddressOf EditarWppSubTransferencia, AddressOf PuedeEjecutar)
			GuardarSalirTransferencia = New DelegateCommandGenerico(Of String)(AddressOf GuardarTransferencia, AddressOf PuedeEjecutar)
			GuardarContinuarTransferencia = New DelegateCommandGenerico(Of String)(AddressOf GuardarTransferenciaContinuar, AddressOf PuedeEjecutar)
			BorrarTransferencia = New DelegateCommandGenerico(Of String)(AddressOf BorrarTransferencias, AddressOf PuedeEjecutar)
			CancelarTransferencia = New DelegateCommandGenerico(Of String)(AddressOf CancelarGrabacionTransferencia, AddressOf PuedeEjecutar)
			''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

			'''''''''''''''''''''''''''''''''''''''''''''CONSIGNACION''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			NuevaConsignacion = New DelegateCommandGenerico(Of String)(AddressOf AbrirPopupConsignacion, AddressOf PuedeEjecutar)
			EditarWppConsignacion = New DelegateCommandGenerico(Of String)(AddressOf EditarConsignacion, AddressOf PuedeEjecutar)
			GuardarSalirConsignacion = New DelegateCommandGenerico(Of String)(AddressOf GuardarConsignacion, AddressOf PuedeEjecutar)
			GuardarContinuarConsignacion = New DelegateCommandGenerico(Of String)(AddressOf GuardarConsignacionContinuar, AddressOf PuedeEjecutar)
			BorrarConsignacion = New DelegateCommandGenerico(Of String)(AddressOf BorrarConsignacionwpp, AddressOf PuedeEjecutar)
			GrabarChequeConsignacion = New DelegateCommandGenerico(Of String)(AddressOf GuardarChequeConsignacion, AddressOf PuedeEjecutar)
			CancelarConsignacion = New DelegateCommandGenerico(Of String)(AddressOf CancelarConsignacionwpp, AddressOf PuedeEjecutar)
			'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

			'''''''''''''''''''''''''''''''''''''''''''''CARGAR PAGOS A''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			NuevoCargarPagoA = New DelegateCommandGenerico(Of String)(AddressOf AbrirPopupCargarPagoA, AddressOf PuedeEjecutar)
			EditarCargarPagoA = New DelegateCommandGenerico(Of String)(AddressOf EditarCargarPagoA_wpp, AddressOf PuedeEjecutar)
			BorrarCargarPagoA = New DelegateCommandGenerico(Of String)(AddressOf BorrarCargarPagoA_wpp, AddressOf PuedeEjecutar)
			GuardarCargarPagosAContinuar = New DelegateCommandGenerico(Of String)(AddressOf WppGuardarCargarPagosAContinuar, AddressOf PuedeEjecutar)
			GuardarCargarPagosASalir = New DelegateCommandGenerico(Of String)(AddressOf WppGuardarCargarPagosASalir, AddressOf PuedeEjecutar)
			CancelarCargarPagosAGrabacion = New DelegateCommandGenerico(Of String)(AddressOf CancelarCargarPagosA, AddressOf PuedeEjecutar)
			''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

			'''''''''''''''''''''''''''''''''''''''''''''CARGAR PAGOS A FONDOS''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			NuevoCargarPagoAFondos = New DelegateCommandGenerico(Of String)(AddressOf AbrirPopupCargarPagoAFondos, AddressOf PuedeEjecutar)
			EditarCargarPagoAFondos = New DelegateCommandGenerico(Of String)(AddressOf EditarCargarPagoAFondos_wpp, AddressOf PuedeEjecutar)
			BorrarCargarPagoAFondos = New DelegateCommandGenerico(Of String)(AddressOf BorrarCargarPagoAFondos_wpp, AddressOf PuedeEjecutar)
			GuardarCargarPagosAFondosContinuar = New DelegateCommandGenerico(Of String)(AddressOf WppGuardarCargarPagosAFondosContinuar, AddressOf PuedeEjecutar)
			GuardarCargarPagosAFondosSalir = New DelegateCommandGenerico(Of String)(AddressOf WppGuardarCargarPagosAFondosSalir, AddressOf PuedeEjecutar)
			CancelarCargarPagosAFondosGrabacion = New DelegateCommandGenerico(Of String)(AddressOf CancelarCargarPagosAFondos, AddressOf PuedeEjecutar)
			''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Carga de Comandos",
								 Me.ToString(), "DefineCommands", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Public Sub CalcularValorDisponibleCargar()
		Try
			Dim Valor_Restar As Decimal = 0
			If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) Then
				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
						For Each i In ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
							Valor_Restar = Valor_Restar + i.curValor
						Next
						ValorDisponibleCargarPagoFondos = ValorTotalGenerarOrden - Valor_Restar
					Else
						ValorDisponibleCargarPagoFondos = ValorTotalGenerarOrden
					End If
				Else
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
						For Each i In ListatesoreriaordenesplusRC_Detalle_CargarPagosA
							Valor_Restar = Valor_Restar + i.curValor
						Next
						ValorDisponibleCargarPago = ValorTotalGenerarOrden - Valor_Restar
					Else
						ValorDisponibleCargarPago = ValorTotalGenerarOrden
					End If
				End If
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema calcular el valor disponible.",
								 Me.ToString(), "CalcularValorDisponibleCargar", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Sub GuardarChequeContinuarwpp()
		GuardarDetalleCheque(False)
	End Sub

	Sub GuardarChequewpp()
		GuardarDetalleCheque(True)
	End Sub

	Sub CancelarCargarPagosA()
		Try
			logEditarCargarPagosA = False
			'ValoresXdefectoWpp()
			objWppCargarPagosA.Close()

			BuscarControlValidacion(OrdenesReciboPLUSView, "TabItemCargarPagoA")
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar Cargar pago a", Me.ToString(), "CancelarCargarPagosA", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Sub CancelarCargarPagosAFondos()
		Try
			logEditarCargarPagosAFondos = False
			'ValoresXdefectoWpp()
			objWppCargarPagosAFondos.Close()

			BuscarControlValidacion(OrdenesReciboPLUSView, "TabItemCargarPagoAFondos")
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar Cargar pago a", Me.ToString(), "CancelarCargarPagosAFondos", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub WppGuardarCargarPagosASalir()
		GuardarCargarPagosA(True)
	End Sub

	Public Sub WppGuardarCargarPagosAContinuar()
		GuardarCargarPagosA(False)
	End Sub

	Public Sub GuardarCargarPagosA(plogSalir As Boolean)
		Try
			Dim objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosA = New List(Of TesoreriaOyDPlusCargosPagosARecibo)

			If ValidarCamposDiligenciadosCargarPagosA() Then
				If logEditarCargarPagosA Then
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngIDConcepto = IdConcepto
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strDetalleConcepto = ConcatenarDetalle(DescripcionComboConcepto, DescripcionConcepto)
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOA
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strTipo = GSTR_ORDENRECIBO
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strCodigoOyD = strCodigoOyDCargarPagosA
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.curValor = ValorCargarPagoA
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.curValorTotal = ValorTotalGenerarOrden
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.ValorTipoCliente = IDTipoClienteCargarPagosA
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strIDTipoCliente = TipoIdClienteDescripcion
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.Nombre = strNombreCodigoOyD
					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.logEsProcesada = True

					ValoresXdefectoCargarPagosAWpp()
					CalcularTotales("")
					CalcularValorDisponibleCargar()
					BuscarControlValidacion(OrdenesReciboPLUSView, "TabItemCargarPagoA")
				Else
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
						objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosA = ListatesoreriaordenesplusRC_Detalle_CargarPagosA
					End If

					ListatesoreriaordenesplusRC_Detalle_CargarPagosA = Nothing

					num = objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosA.Count + 1


					objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosA.Add(New TesoreriaOyDPlusCargosPagosARecibo With
																			{.lngIDDetalle = num,
																			 .strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOA,
																			 .strTipo = GSTR_ORDENRECIBO,
																			 .strCodigoOyD = strCodigoOyDCargarPagosA,
																			 .curValor = ValorCargarPagoA,
																			 .curValorTotal = ValorTotalGenerarOrden,
																			 .ValorTipoCliente = IDTipoClienteCargarPagosA,
																			 .strIDTipoCliente = TipoIdClienteDescripcion,
																			 .strDetalleConcepto = ConcatenarDetalle(DescripcionComboConcepto, DescripcionConcepto),
																			 .lngIDConcepto = IdConcepto,
																			 .Nombre = strNombreCodigoOyD,
																			 .logEsProcesada = True})

					ListatesoreriaordenesplusRC_Detalle_CargarPagosA = objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosA


					If (TesoreriaOrdenesPlusRC_Selected.lngID <> 0) Then
						ListatesoreriaordenesplusRC_Detalle_CargarPagosA.LastOrDefault.lngIDTesoreriaEncabezado = TesoreriaOrdenesPlusRC_Selected.lngID
					End If

					TesoreriaordenesplusRC_Detalle_CargarPagosA_selected = ListatesoreriaordenesplusRC_Detalle_CargarPagosA.LastOrDefault

					ValoresXdefectoCargarPagosAWpp()

					CalcularTotales("")
					CalcularValorDisponibleCargar()
					BuscarControlValidacion(OrdenesReciboPLUSView, "TabItemCargarPagoA")
				End If

				If plogSalir Then
					objWppCargarPagosA.Close()
				Else
					logEditarCargarPagosA = False
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar cargar pagos A.", Me.ToString(), "GuardarCargarPagosA", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub WppGuardarCargarPagosAFondosSalir()
		GuardarCargarPagosAFondos(True)
	End Sub

	Public Sub WppGuardarCargarPagosAFondosContinuar()
		GuardarCargarPagosAFondos(False)
	End Sub

	Public Sub GuardarCargarPagosAFondos(plogSalir As Boolean)
		Try
			Dim objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosAFondos = New List(Of TesoreriaOyDPlusCargosPagosAFondosRecibo)

			If ValidarCamposDiligenciadosCargarPagosAFondos() Then
				Dim strDescripcionTipoAccion As String = String.Empty
				If Not IsNothing(DiccionarioCombosOYDPlus) Then
					If DiccionarioCombosOYDPlus.ContainsKey("TIPOACCIONFONDOS") Then
						If DiccionarioCombosOYDPlus("TIPOACCIONFONDOS").Where(Function(i) i.Retorno = TipoAccionFondos).Count > 0 Then
							strDescripcionTipoAccion = DiccionarioCombosOYDPlus("TIPOACCIONFONDOS").Where(Function(i) i.Retorno = TipoAccionFondos).First.Descripcion
						End If
					End If
				End If

				If logEditarCargarPagosAFondos Then
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngIDConcepto = IdConceptoFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strDetalleConcepto = ConcatenarDetalle(DescripcionComboConceptoFondos, DescripcionConceptoFondos, NroEncargoFondos)
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOAFONDOS
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strTipo = GSTR_ORDENRECIBO
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strCodigoOyD = strCodigoOyDCargarPagosAFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.curValor = ValorCargarPagoAFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.curValorTotal = ValorTotalGenerarOrden
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.ValorTipoCliente = IDTipoClienteCargarPagosAFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strIDTipoCliente = TipoIdClienteDescripcion
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strTipoAccionFondos = TipoAccionFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strDescripcionTipoAccionFondos = strDescripcionTipoAccion
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strCarteraColectivaFondos = CarteraColectivaFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.intNroEncargoFondos = NroEncargoFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.Nombre = strNombreCodigoOyDFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.DescripcionEncargoFondos = DescripcionEncargoFondos
					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.logEsProcesada = True

					'ValoresXdefectoCargarPagosAFondosWpp()
					CalcularTotales("")
					CalcularValorDisponibleCargar()
					Habilitar_Encabezado()
					BuscarControlValidacion(OrdenesReciboPLUSView, "TabItemCargarPagoAFondos")
				Else
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
						objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosAFondos = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
					End If

					ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = Nothing

					num = objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosAFondos.Count + 1


					objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosAFondos.Add(New TesoreriaOyDPlusCargosPagosAFondosRecibo With
																			{.lngIDDetalle = num,
																			 .strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOAFONDOS,
																			 .strTipo = GSTR_ORDENRECIBO,
																			 .strCodigoOyD = strCodigoOyDCargarPagosAFondos,
																			 .curValor = ValorCargarPagoAFondos,
																			 .curValorTotal = ValorTotalGenerarOrden,
																			 .ValorTipoCliente = IDTipoClienteCargarPagosAFondos,
																			 .strIDTipoCliente = TipoIdClienteDescripcion,
																			 .strDetalleConcepto = ConcatenarDetalle(DescripcionComboConceptoFondos, DescripcionConceptoFondos, NroEncargoFondos),
																			 .lngIDConcepto = IdConceptoFondos,
																			 .strTipoAccionFondos = TipoAccionFondos,
																			 .strDescripcionTipoAccionFondos = strDescripcionTipoAccion,
																			 .strCarteraColectivaFondos = CarteraColectivaFondos,
																			 .intNroEncargoFondos = NroEncargoFondos,
																			 .Nombre = strNombreCodigoOyDFondos,
																			 .DescripcionEncargoFondos = DescripcionEncargoFondos,
																			 .logEsProcesada = True})

					ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = objListaTesoreriaOrdenesPlusRC_Detalle_CargarPagosAFondos


					If (TesoreriaOrdenesPlusRC_Selected.lngID <> 0) Then
						ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.LastOrDefault.lngIDTesoreriaEncabezado = TesoreriaOrdenesPlusRC_Selected.lngID
					End If

					TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.LastOrDefault

					'ValoresXdefectoCargarPagosAFondosWpp()

					CalcularTotales("")
					CalcularValorDisponibleCargar()
					Habilitar_Encabezado()
					BuscarControlValidacion(OrdenesReciboPLUSView, "TabItemCargarPagoAFondos")
				End If

				If plogSalir Then
					objWppCargarPagosAFondos.Close()
				Else
					logEditarCargarPagosAFondos = False
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar cargar pagos A.", Me.ToString(), "GuardarCargarPagosA", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub GuardarDetalleCheque(ByVal plogSalir As Boolean)
		Try
			If ValidarCamposDiligenciadosCheque() Then

				Dim objListaTesoreriaOrdenesPlusRC_Detalle_Cheques = New List(Of TesoreriaOyDPlusChequesRecibo)
				Dim strClienteTrae As String = String.Empty
				Dim strDireccionCliente As String = String.Empty
				Dim strOtraDireccion As String = String.Empty


				If CheckClientetraeCheque Then
					strClienteTrae = "SI"
				ElseIf CheckDireccionCliente Then
					strDireccionCliente = "SI"
				ElseIf CheckOtraDireccion Then
					strOtraDireccion = "SI"
				End If

				If Not String.IsNullOrEmpty(mstrArchivoCheque) Then
					If IsNothing(ListaArchivosCheque) Then
						ListaArchivosCheque = New List(Of InformacionArchivos)
					End If
				End If

				If logEditarChequeRC Then
					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strTipo = GSTR_ORDENRECIBO
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strFormaPago = GSTR_CHEQUE
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDBanco = IdBanco
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngNroCheque = lngNroCheque
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.logClienteTrae = CheckClientetraeCheque
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.logDireccionCliente = CheckDireccionCliente
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.logOtraDireccion = CheckOtraDireccion
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strFormaPago = GSTR_CHEQUE
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strDireccionCheque = strDireccionCheque
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strCiudad = strCiudad
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strTelefono = strTelefono
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strTipo = GSTR_ORDENRECIBO
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.curValor = ValorGenerar
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strClienteTrae = strClienteTrae
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strDireccionCliente = strDireccionCliente
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strOtraDireccion = strOtraDireccion
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strDescripcionBanco = BancoChequewpp
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strSector = strSector
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strEstado = GSTR_PENDIENTE_Plus_Detalle
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.UsuarioWindows = Program.UsuarioWindows
						TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.logEsProcesada = True
						CalcularTotales(GSTR_CHEQUE)
						ValoresXdefectoWpp()
						BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemCheque")
						Habilitar_Encabezado()
						CalcularValorDisponibleCargar()

						If Not String.IsNullOrEmpty(mstrArchivoCheque) Then
							If ListaArchivosCheque.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle).Count > 0 Then
								ListaArchivosCheque.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle).First.NombreArchivo = mstrArchivoCheque
								ListaArchivosCheque.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle).First.RutaArchivo = mstrRutaCheque
								ListaArchivosCheque.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle).First.ByteArchivo = mabytArchivoCheque
							Else
								ListaArchivosCheque.Add(New InformacionArchivos With {.ID = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle,
														.IDDetalle = IIf(TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle < 0, 0, TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDDetalle),
														.NombreArchivo = mstrArchivoCheque,
														.RutaArchivo = mstrRutaCheque,
														.ByteArchivo = mabytArchivoCheque})

							End If
						End If
					End If
				Else
					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
						'ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Remove(TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected)
						objListaTesoreriaOrdenesPlusRC_Detalle_Cheques = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
					End If

					If CheckDireccionCliente Then
						If Not IsNothing(ListaDireccionesClientes) Then
							For Each li In ListaDireccionesClientes.Where(Function(i) i.ID = DireccionRegistrada)
								strDireccionCheque = li.strDireccion
								strTelefono = li.strTelefono
								strCiudad = li.strCiudad
							Next
						End If

					End If

					ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = Nothing

					num = (objListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count + 1) * -1

					objListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Add(New TesoreriaOyDPlusChequesRecibo With
																			{.lngID = num,
																			.lngIDDetalle = num,
																			 .lngIDBanco = IdBanco,
																			 .lngNroCheque = lngNroCheque,
																			 .logClienteTrae = CheckClientetraeCheque,
																			 .logDireccionCliente = CheckDireccionCliente,
																			 .logOtraDireccion = CheckOtraDireccion,
																			 .strFormaPago = GSTR_CHEQUE,
																			 .strDireccionCheque = strDireccionCheque,
																			 .strCiudad = strCiudad,
																			 .strTelefono = strTelefono,
																			 .strTipo = GSTR_ORDENRECIBO,
																			 .curValor = ValorGenerar,
																			 .strClienteTrae = strClienteTrae,
																			 .strDireccionCliente = strDireccionCliente,
																			 .strOtraDireccion = strOtraDireccion,
																			 .strDescripcionBanco = BancoChequewpp,
																			 .strEstado = GSTR_PENDIENTE_Plus_Detalle,
																			 .strSector = strSector,
																			 .UsuarioWindows = Program.UsuarioWindows,
																			 .logEsProcesada = True})

					ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = objListaTesoreriaOrdenesPlusRC_Detalle_Cheques

					If (TesoreriaOrdenesPlusRC_Selected.lngID <> 0) Then
						For Each li In ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
							If li.lngIDTesoreriaEncabezado = 0 Or IsNothing(li.lngIDTesoreriaEncabezado) Then
								li.lngIDTesoreriaEncabezado = TesoreriaOrdenesPlusRC_Selected.lngID
							End If
						Next
					End If

					TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.LastOrDefault
					CalcularTotales(GSTR_CHEQUE)
					ValoresXdefectoWpp()
					BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemCheque")
					Habilitar_Encabezado()

					CalcularValorDisponibleCargar()

					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						VerCargarPagosAFondos = Visibility.Visible
						VerCargarPagosA = Visibility.Collapsed
					Else
						VerCargarPagosAFondos = Visibility.Collapsed
						VerCargarPagosA = Visibility.Visible
					End If

					If Not String.IsNullOrEmpty(mstrArchivoCheque) Then
						If ListaArchivosCheque.Where(Function(i) i.ID = num And i.IDDetalle = 0).Count > 0 Then
							ListaArchivosCheque.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.NombreArchivo = mstrArchivoCheque
							ListaArchivosCheque.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.RutaArchivo = mstrRutaCheque
							ListaArchivosCheque.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.ByteArchivo = mabytArchivoCheque
						Else
							ListaArchivosCheque.Add(New InformacionArchivos With {.ID = num,
													.IDDetalle = 0,
													.NombreArchivo = mstrArchivoCheque,
													.RutaArchivo = mstrRutaCheque,
													.ByteArchivo = mabytArchivoCheque})

						End If
					End If
				End If

				If plogSalir Then
					objWppChequeRC.Close()
				Else
					logEditarChequeRC = False
				End If
				objWppChequeRC.ctlSubirArchivo.inicializarControl()
				configurarDocumentos(objWppChequeRC.ctlSubirArchivo, 0, GSTR_CHEQUE)
				mstrArchivoCheque = String.Empty
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Guardar Cheque", Me.ToString(), "GuardarChequeContinuarwpp", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub GuardarDetalleTransferencia(ByVal plogCerrar As Boolean)
		Try
			If ValidarCamposDiligenciadosTransferencia() Then

				Dim objListaTesoreriaOrdenesPlusRC_Detalle_Transferencia = New List(Of TesoreriaOyDPlusTransferenciasRecibo)
				Dim logEsCuentaRegistrada As Boolean = False

				If IdTipoCuentaRegistrada = GSTR_CUENTA_REGISTRADA Then
					logEsCuentaRegistrada = True
				End If

				If Not String.IsNullOrEmpty(mstrArchivoTransferencia) Then
					If IsNothing(ListaArchivoTransferencia) Then
						ListaArchivoTransferencia = New List(Of InformacionArchivos)
					End If
				End If

				If logEditarTransferenciaRC Then
					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strTipo = GSTR_ORDENRECIBO
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strFormaPago = GSTR_TRANSFERENCIA
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strCuentadestino = strNroCuentaDestinoWpp
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.ValorTipoCuentaDestino = strValorTipoCuentaDestinoWpp
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.curValor = ValorGenerarTransferencia
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDBanco = lngCodigoBancoWpp
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIdBancoDestino = lngCodigoBancoDestinoTransferenciaWpp
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strCuentaOrigen = strNroCuentaWpp
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strTipoCuentadestino = DescripcionTipoCuentaDestinoTransferencia
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strTipoCuentaOrigen = DescripcionTipoCuentaTransferencia
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.ValorTipoCuentaOrigen = strValorTipoCuentaWpp
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strEstado = GSTR_PENDIENTE_Plus_Detalle
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strDescripcionBanco = DescripcionBancoTransferencia
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strDescripcionBancoDestino = BancoTransferenciaDescripcionDestinowpp
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.logEsCuentaRegistrada = logEsCuentaRegistrada
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strEstado = GSTR_PENDIENTE_Plus_Detalle
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.UsuarioWindows = Program.UsuarioWindows
						TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.logEsProcesada = True
						CalcularTotales(GSTR_TRANSFERENCIA)
						ValoresXdefectoWppTransferencia()
						BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemTransferencia")
						Habilitar_Encabezado()
						CalcularValorDisponibleCargar()

						If Not String.IsNullOrEmpty(mstrArchivoTransferencia) Then
							If ListaArchivoTransferencia.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle).Count > 0 Then
								ListaArchivoTransferencia.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle).First.NombreArchivo = mstrArchivoTransferencia
								ListaArchivoTransferencia.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle).First.RutaArchivo = mstrRutaTransferencia
								ListaArchivoTransferencia.Where(Function(i) i.ID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle).First.ByteArchivo = mabytArchivoTransferencia
							Else
								ListaArchivoTransferencia.Add(New InformacionArchivos With {.ID = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle,
																					  .IDDetalle = IIf(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle < 0, 0, TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDDetalle),
																					  .NombreArchivo = mstrArchivoTransferencia,
																					  .RutaArchivo = mstrRutaTransferencia,
																					  .ByteArchivo = mabytArchivoTransferencia})

							End If
						End If
					End If
				Else

					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
						objListaTesoreriaOrdenesPlusRC_Detalle_Transferencia = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
					End If

					ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = Nothing

					num = (objListaTesoreriaOrdenesPlusRC_Detalle_Transferencia.Count + 1) * -1

					objListaTesoreriaOrdenesPlusRC_Detalle_Transferencia.Add(New TesoreriaOyDPlusTransferenciasRecibo With
																	   {.curValor = ValorGenerarTransferencia,
																		.lngIDBanco = lngCodigoBancoWpp,
																		.lngIdBancoDestino = lngCodigoBancoDestinoTransferenciaWpp,
																		.strTipo = GSTR_ORDENRECIBO,
																		.strFormaPago = GSTR_TRANSFERENCIA,
																		.strCuentaOrigen = strNroCuentaWpp,
																		.strCuentadestino = strNroCuentaDestinoWpp,
																		.strTipoCuentadestino = DescripcionTipoCuentaDestinoTransferencia,
																		.strTipoCuentaOrigen = DescripcionTipoCuentaTransferencia,
																		.ValorTipoCuentaOrigen = strValorTipoCuentaWpp,
																		.ValorTipoCuentaDestino = strValorTipoCuentaDestinoWpp,
																		.strEstado = GSTR_PENDIENTE_Plus_Detalle,
																		.strDescripcionBanco = DescripcionBancoTransferencia,
																		.strDescripcionBancoDestino = BancoTransferenciaDescripcionDestinowpp,
																		.logEsCuentaRegistrada = logEsCuentaRegistrada,
																		.lngIDDetalle = num,
																		.lngID = num,
																		.UsuarioWindows = Program.UsuarioWindows,
																		.logEsProcesada = True})

					ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = objListaTesoreriaOrdenesPlusRC_Detalle_Transferencia


					If (TesoreriaOrdenesPlusRC_Selected.lngID <> 0) Then
						For Each li In ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
							If li.lngIDTesoreriaEncabezado = 0 Or IsNothing(li.lngIDTesoreriaEncabezado) Then
								li.lngIDTesoreriaEncabezado = TesoreriaOrdenesPlusRC_Selected.lngID
							End If
						Next
					End If

					TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.LastOrDefault
					CalcularTotales(GSTR_TRANSFERENCIA)
					ValoresXdefectoWppTransferencia()
					BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemTransferencia")
					Habilitar_Encabezado()
					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						VerCargarPagosAFondos = Visibility.Visible
						VerCargarPagosA = Visibility.Collapsed
					Else
						VerCargarPagosAFondos = Visibility.Collapsed
						VerCargarPagosA = Visibility.Visible
					End If

					CalcularValorDisponibleCargar()

					If Not String.IsNullOrEmpty(mstrArchivoTransferencia) Then
						If ListaArchivoTransferencia.Where(Function(i) i.ID = num And i.IDDetalle = 0).Count > 0 Then
							ListaArchivoTransferencia.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.NombreArchivo = mstrArchivoTransferencia
							ListaArchivoTransferencia.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.RutaArchivo = mstrRutaTransferencia
							ListaArchivoTransferencia.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.ByteArchivo = mabytArchivoTransferencia
						Else
							ListaArchivoTransferencia.Add(New InformacionArchivos With {.ID = num,
																				  .IDDetalle = 0,
																				  .NombreArchivo = mstrArchivoTransferencia,
																				  .RutaArchivo = mstrRutaTransferencia,
																				  .ByteArchivo = mabytArchivoTransferencia})

						End If
					End If
				End If



				If plogCerrar Then
					objWppTransferencia_RC.Close()
				Else
					logEditarTransferenciaRC = False
				End If
				objWppTransferencia_RC.ctlSubirArchivo.inicializarControl()
				configurarDocumentos(objWppTransferencia_RC.ctlSubirArchivo, 0, GSTR_TRANSFERENCIA)
				mstrArchivoTransferencia = String.Empty
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el guardado del registro Transferencia",
														 Me.ToString(), "GuardarTransferencia", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub CalcularTotales(strFormaPago As String)
		Try
			If strFormaPago = GSTR_CHEQUE Or String.IsNullOrEmpty(strFormaPago) Then
				ValorTotalGenerarCheque = 0

				If TabItemActual = "Cheques" Then
					ValorTotalGenerarActual = 0
				End If

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					For Each x In ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
						ValorTotalGenerarCheque = ValorTotalGenerarCheque + x.curValor
					Next
				End If

				If TabItemActual = "Cheques" Then
					ValorTotalGenerarActual = ValorTotalGenerarCheque
				End If
			End If

			If strFormaPago = GSTR_TRANSFERENCIA Or String.IsNullOrEmpty(strFormaPago) Then
				ValorTotalGenerarTransferencia = 0

				If TabItemActual = "Transferencias" Then
					ValorTotalGenerarActual = 0
				End If

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					For Each x In ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
						ValorTotalGenerarTransferencia = ValorTotalGenerarTransferencia + x.curValor
					Next
				End If

				If TabItemActual = "Transferencias" Then
					ValorTotalGenerarActual = ValorTotalGenerarTransferencia
				End If
			End If

			If strFormaPago = GSTR_CONSIGNACIONES Or String.IsNullOrEmpty(strFormaPago) Then
				ValorTotalGenerarConsignacion = 0

				If TabItemActual = "Consignaciones" Then
					ValorTotalGenerarActual = 0
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					For Each x In ListatesoreriaordenesplusRC_Detalle_Consignaciones
						ValorTotalGenerarConsignacion = ValorTotalGenerarConsignacion + x.curValor
					Next
				End If

				If TabItemActual = "Consignaciones" Then
					ValorTotalGenerarActual = ValorTotalGenerarConsignacion
				End If
			End If

			ValorTotalGenerarOrden = 0
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
				For Each x In ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
					ValorTotalGenerarOrden = ValorTotalGenerarOrden + x.curValor
				Next
			End If
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
				For Each x In ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
					ValorTotalGenerarOrden = ValorTotalGenerarOrden + x.curValor
				Next
			End If
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
				For Each x In ListatesoreriaordenesplusRC_Detalle_Consignaciones
					ValorTotalGenerarOrden = ValorTotalGenerarOrden + x.curValor
				Next
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Calcular Totales", Me.ToString(), "CalcularTotales", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub RecargarPantalla()
		Try
			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
				intIDOrdenTimer = TesoreriaOrdenesPlusRC_Selected.lngID
				TraerOrdenes("REFRESCARPANTALLA", VistaSeleccionada)
			Else
				intIDOrdenTimer = 0
				TraerOrdenes("REFRESCARPANTALLA", VistaSeleccionada)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar la pantalla de Ordenes de Tesoreria.", Me.ToString(), "RecargarPantalla", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Function ValidarCamposDiligenciadosCheque() As Boolean
		Try
			Dim logTieneError As Boolean = False
			strMensajeValidacion = String.Empty
			'SE QUITA VALIDACIÓN
			'INDICACIÓN DE LUIS RIVERA
			'If IsNothing(lngNroCheque) Or lngNroCheque <= 0 Then
			'    strMensajeValidacion = String.Format("{0}{1} - Nro. Cheque debe ser mayor que 0.", strMensajeValidacion, vbCrLf)
			'    logTieneError = True
			'End If
			If IsNothing(ValorGenerar) Or ValorGenerar <= 0 Then
				strMensajeValidacion = String.Format("{0}{1} - Valor Generar.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			'SE QUITA VALIDACIÓN
			'INDICACIÓN DE LUIS RIVERA
			'If IsNothing(BancoChequewpp) Or String.IsNullOrEmpty(BancoChequewpp) Then
			'    strMensajeValidacion = String.Format("{0}{1} - Banco.", strMensajeValidacion, vbCrLf)
			'    logTieneError = True
			'End If
			If CheckClientetraeCheque = False And CheckDireccionCliente = False And CheckOtraDireccion = False Then
				strMensajeValidacion = String.Format("{0}{1} - Seleccionar minimo una opción para recibir el Cheque .", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If
			If CheckDireccionCliente Then
				If DireccionRegistrada <= 0 And HabilitarDireccionesRegistradas = True Then
					strMensajeValidacion = String.Format("{0}{1} - Seleccionar dirección registrada del combo.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				ElseIf DireccionRegistrada <= 0 And HabilitarDireccionesRegistradas = False Then
					strMensajeValidacion = String.Format("{0}{1} - No hay direcciones registradas, elija otro medio para recibir el cheque.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
			End If
			If CheckOtraDireccion Then
				If String.IsNullOrEmpty(strDireccionCheque) Then
					strMensajeValidacion = String.Format("{0}{1} - Dirección.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				Else
					Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(strDireccionCheque, clsExpresiones.TipoExpresion.Caracteres)
					If Not IsNothing(objValidacionExpresion) Then
						If objValidacionExpresion.TextoValido = False Then
							strMensajeValidacion = String.Format("{0}{1} - Dirección: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
							logTieneError = True
						End If
					End If
				End If
				If String.IsNullOrEmpty(strTelefono) Then
					strMensajeValidacion = String.Format("{0}{1} - Telofono.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				Else
					Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(strTelefono, clsExpresiones.TipoExpresion.Caracteres)
					If Not IsNothing(objValidacionExpresion) Then
						If objValidacionExpresion.TextoValido = False Then
							strMensajeValidacion = String.Format("{0}{1} - Telofono: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
							logTieneError = True
						End If
					End If
				End If
				If String.IsNullOrEmpty(strCiudad) Then
					strMensajeValidacion = String.Format("{0}{1} - Ciudad.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				Else
					Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(strCiudad, clsExpresiones.TipoExpresion.Caracteres)
					If Not IsNothing(objValidacionExpresion) Then
						If objValidacionExpresion.TextoValido = False Then
							strMensajeValidacion = String.Format("{0}{1} - Ciudad: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
							logTieneError = True
						End If
					End If
				End If

			End If

			If Not String.IsNullOrEmpty(strSector) Then
				Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(strSector, clsExpresiones.TipoExpresion.Caracteres)
				If Not IsNothing(objValidacionExpresion) Then
					If objValidacionExpresion.TextoValido = False Then
						strMensajeValidacion = String.Format("{0}{1} - Sector: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If
				End If
			End If

			If logTieneError Then
				mostrarMensaje("Para guardar el Registro es necesario completar los siguientes datos con sus valores correspondientes:" & vbCrLf & strMensajeValidacion, "Ordenes de Recaudo - Cheque.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Return False
			Else
				If ValidarIngresoDetallesCanje("CHEQUES") Then
					strMensajeValidacion = String.Empty
					Return True
				Else
					Return False
				End If
			End If


		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Validar Campos de Cheque",
								 Me.ToString(), "ValidarCamposDiligenciadosCheque", Application.Current.ToString(), Program.Maquina, ex)

			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ValidarCamposDiligenciadosTransferencia() As Boolean
		Try
			Dim logTieneError As Boolean
			strMensajeValidacion = String.Empty

			'SE QUITA VALIDACIÓN
			'INDICACIÓN DE LUIS RIVERA
			'If IsNothing(strNroCuentaWpp) Or String.IsNullOrEmpty(strNroCuentaWpp) Then
			'    strMensajeValidacion = String.Format("{0}{1} - Nro Cuenta origen.", strMensajeValidacion, vbCrLf)
			'    logTieneError = True
			'Else
			'    Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(strNroCuentaWpp, clsExpresiones.TipoExpresion.Caracteres)
			'    If Not IsNothing(objValidacionExpresion) Then
			'        If objValidacionExpresion.TextoValido = False Then
			'            strMensajeValidacion = String.Format("{0}{1} - Nro Cuenta origen: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
			'            logTieneError = True
			'        End If
			'    End If
			'End If
			'If String.IsNullOrEmpty(strValorTipoCuentaWpp) Then
			'    strMensajeValidacion = String.Format("{0}{1} - Seleccione un tipo de cuenta origen", strMensajeValidacion, vbCrLf)
			'    logTieneError = True
			'End If
			'If lngCodigoBancoWpp <= 0 Then
			'    strMensajeValidacion = String.Format("{0}{1} - el Código del Banco origen debe ser mayor a cero", strMensajeValidacion, vbCrLf)
			'    logTieneError = True
			'End If

			If logValidarCamposObligatoriosTransferenciaConsignacion Then
				If IsNothing(strNroCuentaDestinoWpp) Or String.IsNullOrEmpty(strNroCuentaDestinoWpp) Then
					strMensajeValidacion = String.Format("{0}{1} - Nro Cuenta destino.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				Else
					Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(strNroCuentaDestinoWpp, clsExpresiones.TipoExpresion.Caracteres)
					If Not IsNothing(objValidacionExpresion) Then
						If objValidacionExpresion.TextoValido = False Then
							strMensajeValidacion = String.Format("{0}{1} - Nro Cuenta destino: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
							logTieneError = True
						End If
					End If
				End If
				If String.IsNullOrEmpty(strValorTipoCuentaDestinoWpp) Then
					strMensajeValidacion = String.Format("{0}{1} - Seleccione un tipo de cuenta destino", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
				If lngCodigoBancoDestinoTransferenciaWpp <= 0 Then
					strMensajeValidacion = String.Format("{0}{1} - el Código del Banco destino debe ser mayor a cero", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
			End If

			If IsNothing(ValorGenerarTransferencia) Or ValorGenerarTransferencia <= 0 Then
				strMensajeValidacion = String.Format("{0}{1} - Valor Generar Transferencia.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If logTieneError Then
				mostrarMensaje("Para guardar el Registro es necesario completar los siguientes datos con sus valores correspondientes:" & vbCrLf & strMensajeValidacion, "Ordenes de Recaudo - Transferencia.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Return False
			Else
				If ValidarIngresoDetallesCanje("TRANSFERENCIAS") Then
					strMensajeValidacion = String.Empty
					Return True
				Else
					Return False
				End If
			End If


		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Validar Campos de Transferencia",
								 Me.ToString(), "ValidarCamposDiligenciadosTransferencia", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ValidarCamposDiligenciadosConsignacion() As Boolean
		Try
			Dim logTieneError As Boolean
			strMensajeValidacion = String.Empty

			'SE QUITA VALIDACIÓN
			'INDICACIÓN DE LUIS RIVERA
			'If IsNothing(lngNroReferencia) Or lngNroReferencia <= 0 Then
			'    strMensajeValidacion = String.Format("{0}{1} - Nro Referencia.", strMensajeValidacion, vbCrLf)
			'    logTieneError = True
			'End If

			If logValidarCamposObligatoriosTransferenciaConsignacion Then
				If String.IsNullOrEmpty(strCuentaConsignacion) Then
					strMensajeValidacion = String.Format("{0}{1} - Nro. Cuenta consignación.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
			End If

			If String.IsNullOrEmpty(strTipoPagoConsignacion) Then
				strMensajeValidacion = String.Format("{0}{1} - Forma de consignación", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If (IsNothing(ValorGenerarConsignacion) Or ValorGenerarConsignacion <= 0) Then
				If IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
					strMensajeValidacion = String.Format("{0}{1} - valor", strMensajeValidacion, vbCrLf)
					logTieneError = True
				ElseIf ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones.Count < 0 Then
					strMensajeValidacion = String.Format("{0}{1} - valor", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
			End If

			If strTipoPagoConsignacion = GSTR_CHEQUE Or strTipoPagoConsignacion = GSTR_CONSIGNACION_AMBAS Then
				If logEditarConsignacion = False Then
					If IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
						strMensajeValidacion = String.Format("{0}{1} - si la forma de consignación es CHEQUE, debe existir minimo 1 registro de CHEQUE", strMensajeValidacion, vbCrLf)
						logTieneError = True
					ElseIf ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones.Count <= 0 Then
						strMensajeValidacion = String.Format("{0}{1} - si la forma de consignación es CHEQUE, debe existir minimo 1 registro de CHEQUE", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If
				End If

			End If

			If strTipoPagoConsignacion = GSTR_CONSIGNACION_AMBAS And ValorGenerarEfectivoConsignacion = 0 Then
				strMensajeValidacion = String.Format("{0}{1} - si la forma de consignación es AMBAS, debe de ingresar el valor en efectivo de la consignación", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If logTieneError Then
				mostrarMensaje("Para guardar el Registro es necesario completar los siguientes datos con sus valores correspondientes:" & vbCrLf & strMensajeValidacion, "Ordenes de Recaudo - Transferencia.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Return False
			Else
				Dim strDescripcionValidacionCanje As String = String.Empty

				If strTipoPagoConsignacion = "C" Then
					strDescripcionValidacionCanje = "CONSIGNACION_CHEQUES"
				ElseIf strTipoPagoConsignacion = "E" Then
					strDescripcionValidacionCanje = "CONSIGNACION_EFECTIVO"
				End If

				If ValidarIngresoDetallesCanje(strDescripcionValidacionCanje) Then
					strMensajeValidacion = String.Empty
					Return True
				Else
					Return False
				End If
			End If


		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Validar Campos de Transferencia",
								 Me.ToString(), "ValidarCamposDiligenciadosTransferencia", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ValidarCamposDiligenciadosCargarPagosA() As Boolean
		Try
			Dim logTieneError As Boolean
			strMensajeValidacion = String.Empty

			If IsNothing(ValorCargarPagoA) Or ValorCargarPagoA <= 0 Then
				strMensajeValidacion = String.Format("{0}{1} - Valor cargar pago.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If IsNothing(strCodigoOyDCargarPagosA) Or String.IsNullOrEmpty(strCodigoOyDCargarPagosA) Then
				strMensajeValidacion = String.Format("{0}{1} - Código OyD a cargar pago.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If ValorCargarPagoA > ValorDisponibleCargarPago Then
				strMensajeValidacion = String.Format("{0}{1} - El valor Ingresado no puede ser mayor al disponible.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If Not String.IsNullOrEmpty(DescripcionConcepto) Then
				Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(DescripcionConcepto, clsExpresiones.TipoExpresion.Caracteres)
				If Not IsNothing(objValidacionExpresion) Then
					If objValidacionExpresion.TextoValido = False Then
						strMensajeValidacion = String.Format("{0}{1} - En la descripción hay caracteres invalidos", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If
				End If
			End If

			If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
				If IsNothing(NroEncargoFondos) Then
					strMensajeValidacion = String.Format("{0}{1} - Nro encargo", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
			End If

			If logTieneError Then
				mostrarMensaje("Para guardar el Registro es necesario completar los siguientes datos con sus valores correspondientes:" & vbCrLf & strMensajeValidacion, "Ordenes de Recaudo - Transferencia.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Return False
			Else
				strMensajeValidacion = String.Empty
				Return True
			End If
		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Validar Campos de Transferencia",
								 Me.ToString(), "ValidarCamposDiligenciadosTransferencia", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ValidarCamposDiligenciadosCargarPagosAFondos() As Boolean
		Try
			Dim logTieneError As Boolean
			strMensajeValidacion = String.Empty

			If IsNothing(ValorCargarPagoAFondos) Or ValorCargarPagoAFondos <= 0 Then
				strMensajeValidacion = String.Format("{0}{1} - Valor cargar pago fondos.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If IsNothing(strCodigoOyDCargarPagosAFondos) Or String.IsNullOrEmpty(strCodigoOyDCargarPagosAFondos) Then
				strMensajeValidacion = String.Format("{0}{1} - Código OyD a cargar pago fondos.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If ValorCargarPagoAFondos > ValorDisponibleCargarPagoFondos Then
				strMensajeValidacion = String.Format("{0}{1} - El valor Ingresado no puede ser mayor al disponible.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If Not String.IsNullOrEmpty(DescripcionConcepto) Then
				Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(DescripcionConcepto, clsExpresiones.TipoExpresion.Caracteres)
				If Not IsNothing(objValidacionExpresion) Then
					If objValidacionExpresion.TextoValido = False Then
						strMensajeValidacion = String.Format("{0}{1} - En la descripción hay caracteres invalidos", strMensajeValidacion, vbCrLf)
						logTieneError = True
					End If
				End If
			End If

			If String.IsNullOrEmpty(TipoAccionFondos) Then
				strMensajeValidacion = String.Format("{0}{1} - Tipo acción", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
				If IsNothing(NroEncargoFondos) Then
					strMensajeValidacion = String.Format("{0}{1} - Nro encargo", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
			End If

			If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD Then
				If IsNothing(IdConceptoFondos) Or IdConceptoFondos = 0 Then
					strMensajeValidacion = String.Format("{0}{1} - Concepto.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
			End If

			If logTieneError Then
				mostrarMensaje("Para guardar el Registro es necesario completar los siguientes datos con sus valores correspondientes:" & vbCrLf & strMensajeValidacion, "Ordenes de Recaudo - Transferencia.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Return False
			Else
				strMensajeValidacion = String.Empty
				Return True
			End If
		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Validar Campos de Transferencia",
								 Me.ToString(), "ValidarCamposDiligenciadosTransferencia", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ValidarCamposDiligenciadosChequeConsignacion() As Boolean
		Try
			Dim logTieneError As Boolean
			strMensajeValidacion = String.Empty

			'SE QUITA VALIDACIÓN
			'INDICACIÓN DE LUIS RIVERA
			'If IsNothing(lngNroChequeConsignacion) Or lngNroChequeConsignacion <= 0 Then
			'    strMensajeValidacion = String.Format("{0}{1} - Nro. Cheque", strMensajeValidacion, vbCrLf)
			'    logTieneError = True
			'End If
			If logValidarCamposObligatoriosTransferenciaConsignacion Then
				If IsNothing(lngCodigoBancoConsignacionWpp) Or lngCodigoBancoConsignacionWpp <= 0 Then
					strMensajeValidacion = String.Format("{0}{1} - Código de Banco.", strMensajeValidacion, vbCrLf)
					logTieneError = True
				End If
			End If

			If IsNothing(ValorGenerarConsignacion) Or ValorGenerarConsignacion <= 0 Then
				strMensajeValidacion = String.Format("{0}{1} - El valor Ingresado no puede menor o igual que cero.", strMensajeValidacion, vbCrLf)
				logTieneError = True
			End If

			If logTieneError Then
				mostrarMensaje("Para guardar el Registro es necesario completar los siguientes datos con sus valores correspondientes:" & vbCrLf & strMensajeValidacion, "Ordenes de Recaudo - Transferencia.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Return False
			Else
				strMensajeValidacion = String.Empty
				Return True
			End If


		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Validar Campos de Cheque Consignación",
								 Me.ToString(), "ValidarCamposDiligenciadosChequeConsignacion", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Private Function PuedeEjecutar(ByVal s As String) As Boolean
		Return True
	End Function

	'********************    EDICION DE DETALLE   *******************************************

	Public Sub EditarCargarPagoA_wpp()
		Try
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
				If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
					AbrirPopupCargarPagoA(GSTR_EDITARDETALLE_Plus)
				End If
			Else
				mostrarMensaje("Para Realizar el Proceso de Edición debe existir 1 Registro en Detalle Cargar Pagos a", Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Editar el registro",
								 Me.ToString(), "EditarCargarPagoA_wpp", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub EditarCargarPagoAFondos_wpp()
		Try
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
				If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
					AbrirPopupCargarPagoAFondos(GSTR_EDITARDETALLE_Plus)
				End If
			Else
				mostrarMensaje("Para Realizar el Proceso de Edición debe existir 1 Registro en Detalle Cargar Pagos a fondos", Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Editar el registro",
								 Me.ToString(), "EditarCargarPagoAFondos_wpp", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub EditarWppSubCheque()
		Try
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then

				If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
					AbrirPopup(GSTR_EDITARDETALLE_Plus)
				End If
			Else
				mostrarMensaje("Para Realizar el Proceso de Edición debe existir 1 Registro en Detalle Cheque", Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Editar el registro",
								 Me.ToString(), "EditarChequewpp", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub EditarWppSubTransferencia()
		Try
			If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then

				If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
					AbrirPopupTransferencia(GSTR_EDITARDETALLE_Plus)
				End If
			Else
				mostrarMensaje("Para Realizar el Proceso de Edición debe existir 1 Registro en Detalle Transferencia", Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)


			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Editar el registro",
								 Me.ToString(), "EditarWppSubTransferencia", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub EditarConsignacion()
		Try
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then

				If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > 0 Then

					'Se llena el combo de forma de pago de consignación.
					If Not IsNothing(_DiccionarioCombosOYDPlusCompleta) Then
						If _DiccionarioCombosOYDPlusCompleta.ContainsKey("FORMACONSIGNACION") Then
							Dim objLista As New List(Of OYDPLUSUtilidades.CombosReceptor)
							For Each li In _DiccionarioCombosOYDPlusCompleta("FORMACONSIGNACION")
								If li.Retorno <> GSTR_CONSIGNACION_AMBAS Then
									objLista.Add(li)
								End If
							Next

							If Not IsNothing(ListaFormaPagoConsignacion) Then
								ListaFormaPagoConsignacion.Clear()
							Else
								ListaFormaPagoConsignacion = New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
							End If

							ListaFormaPagoConsignacion.Add("FORMACONSIGNACION", objLista)
						End If
					End If

					VerFormaChequeEfectivoConsignacion = Visibility.Collapsed

					AbrirPopupConsignacion(GSTR_EDITARDETALLE_Plus)
				End If
			Else
				mostrarMensaje("Para Realizar el Proceso de Edición debe existir 1 Registro en Detalle Consignación", Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)


			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Editar el registro",
								 Me.ToString(), "EditarWppSubConsignacion", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Function ObjetoEditarCheque() As Boolean
		Try
			If TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strEstado.Substring(0, 1) <> GSTR_CUMPLIDA_Plus And TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strEstado.Substring(0, 1) <> GSTR_ANULADA_Plus Then
				logCambiarPropiedadesPOPPUP = False
				logEditarChequeRC = True
				If Not IsNothing(TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID) Then
					configurarDocumentos(objWppChequeRC.ctlSubirArchivo, TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngID, GSTR_CHEQUE)
				End If


				lngNroCheque = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngNroCheque
				ValorGenerar = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.curValor
				IdBanco = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.lngIDBanco
				BancoChequewpp = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strDescripcionBanco
				CheckClientetraeCheque = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.logClienteTrae
				CheckDireccionCliente = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.logDireccionCliente
				CheckOtraDireccion = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.logOtraDireccion
				If Not IsNothing(ListaDireccionesClientes) Then
					For Each li In ListaDireccionesClientes.Where(Function(i) i.strCiudad = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strCiudad And i.strDireccion = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strDireccionCheque And i.strTelefono = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strTelefono)
						DireccionRegistrada = li.ID
					Next

				End If
				strDireccionCheque = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strDireccionCheque
				strTelefono = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strTelefono
				strCiudad = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strCiudad
				strSector = TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected.strSector

				TextoCampoSubirArchivoDetalle(GSTR_CHEQUE, TesoreriaOrdenesPlusRC_Detalle_Cheques_Selected)

				logCambiarPropiedadesPOPPUP = True
				Return True
			Else
				Return False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el cheque.",
								 Me.ToString(), "ObjetoEditarCheque", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ObjetoEditarCargarPagosA() As Boolean
		Try
			If Not IsNothing(_TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngIDConcepto) Or
					_TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngIDConcepto = 0 Then
				If _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strDetalleConcepto.Contains("(") Then
					DescripcionConcepto = RetornarValorDetalle(_TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strDetalleConcepto, "(", ")")
				Else
					DescripcionConcepto = _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strDetalleConcepto
				End If
			ElseIf Not IsNothing(_TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strDetalleConcepto) Then
				DescripcionConcepto = RetornarValorDetalle(_TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strDetalleConcepto, "(", ")")
			End If

			If Not IsNothing(_TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngIDConcepto) Then
				IdConcepto = _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.lngIDConcepto
			End If
			logCambiarPropiedadesPOPPUP = False
			logEditarCargarPagosA = True

			logConsultarCarterasColectivas = False
			strCodigoOyDCargarPagosA = _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.strCodigoOyD
			strNombreCodigoOyD = _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.Nombre
			logConsultarCarterasColectivas = True
			IDTipoClienteCargarPagosA = _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.ValorTipoCliente
			strCodigoOyDCliente = _TesoreriaOrdenesPlusRC_Selected.strIDComitente
			ValorCargarPagoA = _TesoreriaordenesplusRC_Detalle_CargarPagosA_selected.curValor

			CalcularTotales(String.Empty)
			CalcularValorDisponibleCargar()

			Dim Valor_Restar As Decimal = 0
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
				For Each i In ListatesoreriaordenesplusRC_Detalle_CargarPagosA
					Valor_Restar = Valor_Restar + i.curValor
				Next
				ValorDisponibleCargarPago = ValorTotalGenerarOrden - Valor_Restar
			Else
				ValorDisponibleCargarPago = ValorTotalGenerarOrden
			End If
			ValorDisponibleCargarPago = ValorDisponibleCargarPago + ValorCargarPagoA

			LlevarSaldoDisponibleCargarPagosA = False
			logCambiarPropiedadesPOPPUP = True

			ConsultarComitentesClienteEncabezado(TesoreriaOrdenesPlusRC_Selected.strIDComitente)
			Return True
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar cargar pagos A.",
								 Me.ToString(), "ObjetoEditarCargarPagosA", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ObjetoEditarCargarPagosFondos() As Boolean
		Try
			If _TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
				If logEsFondosOYD = False Then
					If Not IsNothing(_ListaCarterasColectivasClienteCompleta) Then
						Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
						For Each li In _ListaCarterasColectivasClienteCompleta
							If li.CarteraColectiva = _CarteraColectivaFondos Then
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

						ListaEncargosCarteraColectiva = objNuevaListaEncargos
					Else
						ListaEncargosCarteraColectiva = Nothing
					End If
				Else
					If _TipoAccionFondos = GSTR_FONDOS_TIPOACCION_ADICION Then
						ConsultarCarterasColectivasFondos(_TesoreriaOrdenesPlusRC_Selected.strIDComitente, False, "ENCARGOS_CARTERACOLECTIVA_EDICION")
					Else
						Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
						ListaEncargosCarteraColectiva = objNuevaListaEncargos
					End If
				End If
			Else
				ListaEncargosCarteraColectiva = Nothing
			End If


			If IsNothing(_TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngIDConcepto) Or
					 _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngIDConcepto = 0 Then

				'JABG20180416
				'Se realiza cambio para evaluar si el detalle concepto no esta vacio
				If Not IsNothing(_TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strDetalleConcepto) Then

					If _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strDetalleConcepto.Contains("(") Then
						DescripcionConceptoFondos = RetornarValorDetalle(_TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strDetalleConcepto, "(", ")")
					Else
						DescripcionConceptoFondos = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strDetalleConcepto
					End If

				End If

			Else
				DescripcionConceptoFondos = RetornarValorDetalle(_TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strDetalleConcepto, "(", ")")
			End If

			If Not IsNothing(_TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngIDConcepto) Then
				IdConceptoFondos = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngIDConcepto
			End If

			logCambiarPropiedadesPOPPUP = False
			logEditarCargarPagosAFondos = True

			strCodigoOyDCargarPagosAFondos = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strCodigoOyD
			strNombreCodigoOyD = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.Nombre
			IDTipoClienteCargarPagosAFondos = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.ValorTipoCliente
			ValorCargarPagoAFondos = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.curValor
			'TipoAccionFondos = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.strTipoAccionFondos

			CalcularTotales(String.Empty)
			CalcularValorDisponibleCargar()

			Dim Valor_Restar As Decimal = 0
			If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
				For Each i In ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
					Valor_Restar = Valor_Restar + i.curValor
				Next
				ValorDisponibleCargarPagoFondos = ValorTotalGenerarOrden - Valor_Restar
			Else
				ValorDisponibleCargarPagoFondos = ValorTotalGenerarOrden
			End If
			ValorDisponibleCargarPagoFondos = ValorDisponibleCargarPagoFondos + ValorCargarPagoAFondos

			If logEsFondosOYD = False Then
				NroEncargoFondos = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.intNroEncargoFondos
			End If

			LlevarSaldoDisponibleCargarPagosAFondos = False
			logCambiarPropiedadesPOPPUP = True
			ConsultarComitentesClienteEncabezado(TesoreriaOrdenesPlusRC_Selected.strIDComitente)

			Return True
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar cargar pagos A.",
								 Me.ToString(), "ObjetoEditarCargarPagosFondos", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ObjetoEditarTransferencia() As Boolean
		Try
			If TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strEstado.Substring(0, 1) <> GSTR_CUMPLIDA_Plus And TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strEstado.Substring(0, 1) <> GSTR_ANULADA_Plus Then
				logEditarTransferenciaRC = True

				'logCambiarPropiedadesPOPPUP = False DEMC20190426
				logCambiarPropiedadesPOPPUP = True
				If Not IsNothing(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID) Then
					configurarDocumentos(objWppTransferencia_RC.ctlSubirArchivo, TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID, GSTR_TRANSFERENCIA)
				End If

				If TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.logEsCuentaRegistrada = True Then
					IdTipoCuentaRegistrada = GSTR_CUENTA_REGISTRADA
					VerComboCuentasRegistradas = Visibility.Visible
					HabilitarCuentasRegistradas = True
				Else
					IdTipoCuentaRegistrada = GSTR_CUENTA_NO_REGISTRADA
					VerComboCuentasRegistradas = Visibility.Collapsed
					HabilitarCuentasRegistradas = False
				End If
				CuentaRegistrada = 0

				If Not IsNothing(ListaCuentasClientes) Then
					For Each li In ListaCuentasClientes.Where(Function(i) i.strCuenta = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strCuentaOrigen And i.lngIDBanco = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDBanco)
						CuentaRegistrada = li.ID
					Next
				End If

				strNroCuentaWpp = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strCuentaOrigen
				strValorTipoCuentaWpp = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.ValorTipoCuentaOrigen
				strNroCuentaDestinoWpp = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strCuentadestino
				strValorTipoCuentaDestinoWpp = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.ValorTipoCuentaDestino
				BancoTransferenciaDescripcionDestinowpp = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strDescripcionBancoDestino
				DescripcionBancoTransferencia = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strDescripcionBanco
				lngCodigoBancoDestinoTransferenciaWpp = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIdBancoDestino
				lngCodigoBancoWpp = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDBanco
				ValorGenerarTransferencia = TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.curValor

				TextoCampoSubirArchivoDetalle(GSTR_TRANSFERENCIA, Nothing, TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected)

				logCambiarPropiedadesPOPPUP = True

				Return True
			Else
				Return False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar transferencias.",
								 Me.ToString(), "ObjetoEditarTransferencia", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	Public Function ObjetoEditarConsignacion() As Boolean
		Try
			If TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strEstado.Substring(0, 1) <> GSTR_CUMPLIDA_Plus And TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strEstado.Substring(0, 1) <> GSTR_ANULADA_Plus Then
				logEditarConsignacion = True
				logCambiarPropiedadesPOPPUP = False
				If Not IsNothing(TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID) Then

					configurarDocumentos(objWppConsignacion.ctlSubirArchivo, TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngID, GSTR_CONSIGNACIONES)
				End If

				lngNroReferencia = IIf(IsNothing(TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngNroReferencia), 0, TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngNroReferencia)
				strDescripcionCuentaConsignacion = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strDescripcionCuentaConsignacion
				strCuentaConsignacion = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strCuentadestino
				strTipoPagoConsignacion = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.ValorFormaConsignacion
				VerFormaChequeConsignacionGrid = Visibility.Collapsed
				strDescripcionCuentaConsignacion = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strDescripcionCuentaConsignacion
				If Not IsNothing(TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngNroCheque) Then
					lngNroChequeConsignacion = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngNroCheque
					If lngNroChequeConsignacion > 0 Then
						VerFormaChequeConsignacion = Visibility.Visible
					Else
						VerFormaChequeConsignacion = Visibility.Collapsed
					End If

					lngCodigoBancoConsignacionWpp = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDBanco

				End If
				strDescripcionBancoConsignacion = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strDescripcionBanco
				ValorGenerarConsignacion = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.curValor
				lngCodigoBancoDestinoConsignacionWpp = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIdBancoDestino

				If logEditarConsignacion Then
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
						If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Where(Function(i) i.lngNroReferencia = IIf(IsNothing(TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngNroReferencia), 0, TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngNroReferencia) And i.ValorFormaConsignacion = GSTR_CHEQUE).Count > 1 Then
							HabilitarEncabezadoConsignacionReferencia = False
							HabilitarEncabezadoConsignacion = False
						Else
							HabilitarEncabezadoConsignacionReferencia = True
							HabilitarEncabezadoConsignacion = True
						End If
					End If
				End If

				TextoCampoSubirArchivoDetalle(GSTR_CONSIGNACIONES, Nothing, Nothing, TesoreriaordenesplusRC_Detalle_Consignaciones_selected)

				logCambiarPropiedadesPOPPUP = True
				Return True
			Else
				Return False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar consignación.",
								 Me.ToString(), "ObjetoEditarConsignacion", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return False
		End Try
	End Function

	'********************TERMINA EDICION DE DETALLE*******************************************

	Public Sub BorrarCargarPagoA_wpp()
		Try
			If Not ListatesoreriaordenesplusRC_Detalle_CargarPagosA Is Nothing Then
				If ListatesoreriaordenesplusRC_Detalle_CargarPagosA.Count > 0 Then
					mostrarMensajePregunta("¿Está Seguro que desea eliminar el Registro?",
										   Program.TituloSistema,
										   "ELIMINARCARGARPAGOSA",
										   AddressOf TerminoPreguntarConfirmacionCargarPagosA, False)
				Else
					mostrarMensaje("No hay registros a Eliminar", "Ordenes de recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

			Else
				IsBusy = False
				mostrarMensaje("No hay registros a Eliminar", "Ordenes de recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
								 Me.ToString(), "BorrarChequewpp", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub BorrarCargarPagoAFondos_wpp()
		Try
			If Not ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos Is Nothing Then
				If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
					mostrarMensajePregunta("¿Está Seguro que desea eliminar el Registro?",
										   Program.TituloSistema,
										   "ELIMINARCARGARPAGOSAFONDOS",
										   AddressOf TerminoPreguntarConfirmacionCargarPagosAFondos, False)
				Else
					mostrarMensaje("No hay registros a Eliminar", "Ordenes de recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

			Else
				IsBusy = False
				mostrarMensaje("No hay registros a Eliminar", "Ordenes de recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
								 Me.ToString(), "BorrarChequewpp", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub BorrarChequewpp()
		Try

			If Not ListaTesoreriaOrdenesPlusRC_Detalle_Cheques Is Nothing Then
				If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
					mostrarMensajePregunta("¿Está Seguro que desea eliminar el Registro?",
										   Program.TituloSistema,
										   "ELIMINARCHEQUE",
										   AddressOf TerminoPreguntarConfirmacionCheque, False)
				Else
					mostrarMensaje("No hay registros a Eliminar", "Ordenes de Recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

			Else
				IsBusy = False
				mostrarMensaje("No hay registros a Eliminar", "Ordenes de Recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
								 Me.ToString(), "BorrarChequewpp", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub BorrarTransferencias()
		Try
			If Not ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias Is Nothing Then
				If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
					mostrarMensajePregunta("¿Está Seguro que desea eliminar el Registro?",
										   Program.TituloSistema,
										   "ELIMINARTRANSFERENCIA",
										   AddressOf TerminoPreguntarConfirmacionTransferencias, False)
				Else
					mostrarMensaje("No hay registros a Eliminar", "Ordenes de Recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

			Else
				IsBusy = False
				mostrarMensaje("No hay registros a Eliminar", "Ordenes de Recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
								 Me.ToString(), "BorrarChequewpp", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
		'dcProxy.TesoreriaOrdenesDetalle_Elminar(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDTesoreriaEncabezado,
		'                                                     TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID,
		'                                                     TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strFormaPago,
		'                                                     Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarDetalle, Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarTransferencia.ToString)

	End Sub

	Sub GuardarChequeConsignacion()
		Try

			If ValidarCamposDiligenciadosChequeConsignacion() Then

				Dim objListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignacion = New List(Of ChequesConsignacionesOyDPlus)

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
					objListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignacion = ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones
				End If

				ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones = Nothing

				num = objListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignacion.Count + 1
				objListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignacion.Add(New ChequesConsignacionesOyDPlus With
																	  {.lngIDBanco = lngCodigoBancoConsignacionWpp,
																	   .strDescripcionBanco = strDescripcionBancoConsignacion,
																	   .lngNroCheque = lngNroChequeConsignacion,
																	   .Valor = ValorGenerarConsignacion,
																	   .strCuentaConsignacion = strCuentaConsignacion,
																	   .strTipoCuenta =
																	   .scaneados = ""})
				ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones = objListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignacion


				HabilitarEncabezadoConsignacion = False
				HabilitarEncabezadoConsignacionReferencia = False
				TesoreriaordenesplusRC_Detalle_ChequesConsignaciones_selected = ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones.LastOrDefault
				lngNroChequeConsignacion = 0
				lngCodigoBancoConsignacionWpp = 0

				ValorGenerarConsignacion = 0
				strDescripcionBancoConsignacion = String.Empty
				strDescripcionBancoConsignacion = String.Empty

				'Recarga la lista de forma de pago para quitar la opción de efectivo.
				If Not IsNothing(_DiccionarioCombosOYDPlusCompleta) Then
					If _DiccionarioCombosOYDPlusCompleta.ContainsKey("FORMACONSIGNACION") Then
						Dim objLista As New List(Of OYDPLUSUtilidades.CombosReceptor)
						For Each li In _DiccionarioCombosOYDPlusCompleta("FORMACONSIGNACION")
							If li.Retorno <> GSTR_EFECTIVO Then
								objLista.Add(li)
							End If
						Next

						Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
						objDiccionario.Add("FORMACONSIGNACION", objLista)

						Dim strValorFormaPago As String = _strTipoPagoConsignacion
						ListaFormaPagoConsignacion = objDiccionario
						strTipoPagoConsignacion = strValorFormaPago
					End If
				End If
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Grabar el Cheque para la Consignación.",
								 Me.ToString(), "GuardarChequeConsignacion", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub BorrarConsignacionwpp()
		Try
			If Not ListatesoreriaordenesplusRC_Detalle_Consignaciones Is Nothing Then
				If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Count > 0 Then
					mostrarMensajePregunta("¿Está Seguro que desea eliminar el Registro?",
										   Program.TituloSistema,
										   "ELIMINARCONSIGNACION",
										   AddressOf TerminoPreguntarConfirmacionConsignaciones, False)
				Else
					mostrarMensaje("No hay registros a Eliminar", "Ordenes de Recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				End If

			Else
				IsBusy = False
				mostrarMensaje("No hay registros a Eliminar", "Ordenes de Recaudo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If

		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
								 Me.ToString(), "BorrarConsignacion", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
		'dcProxy.TesoreriaOrdenesDetalle_Elminar(TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngIDTesoreriaEncabezado,
		'                                                     TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID,
		'                                                     TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.strFormaPago,
		'                                                     Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarDetalle, Program.TipoBorradoOrdenesTesoreriaOyDPlus.BorrarTransferencia.ToString)

	End Sub

	Public Sub AbrirPopupCargarPagoA(pstrOpcion As String)
		Try
			If IsNothing(pstrOpcion) Then
				logCambiarPropiedadesPOPPUP = True
				logConsultarCarterasColectivas = True

				RaiseEvent LanzarPopupCargarPagoA(Me, New System.EventArgs, GSTR_NUEVODETALLE_Plus)
			Else
				RaiseEvent LanzarPopupCargarPagoA(Me, New System.EventArgs, pstrOpcion)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al abrir Popup", Me.ToString(), "AbrirPopup", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Public Sub AbrirPopupCargarPagoAFondos(pstrOpcion As String)
		Try
			Dim logAbrirPopup As Boolean = True

			If logEsFondosOYD And _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And GSTR_CONTROL_CANJE_CHEQUE = "SI" Then
				MostrarGuardarContinuarPagosFondos = Visibility.Collapsed
			Else
				MostrarGuardarContinuarPagosFondos = Visibility.Visible
			End If

			If IsNothing(pstrOpcion) Then
				If logEsFondosOYD And _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And GSTR_CONTROL_CANJE_CHEQUE = "SI" Then
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
						If ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos.Count > 0 Then
							logAbrirPopup = False
							mostrarMensaje("Cuando esta habilitado el control de canje de cheques, solo puede existir 1 registro para distribución de los pagos.", "Ordenes de recaudo.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						End If
					End If
				End If

				If logAbrirPopup Then
					logCambiarPropiedadesPOPPUP = True
					logConsultarCarterasColectivas = True

					RaiseEvent LanzarPopupCargarPagoAFondos(Me, New System.EventArgs, GSTR_NUEVODETALLE_Plus)
				End If
			Else
				RaiseEvent LanzarPopupCargarPagoAFondos(Me, New System.EventArgs, pstrOpcion)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al abrir Popup", Me.ToString(), "AbrirPopup", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Public Sub AbrirPopup(pstrOpcion As String)
		Try
			If IsNothing(pstrOpcion) Then
				If ValidarIngresoDetallesCanje("CHEQUES") Then
					logCambiarPropiedadesPOPPUP = True

					RaiseEvent LanzarPopupCheques(Me, New System.EventArgs, GSTR_NUEVODETALLE_Plus)
				End If
			Else
				RaiseEvent LanzarPopupCheques(Me, New System.EventArgs, pstrOpcion)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al abrir Popup", Me.ToString(), "AbrirPopup", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Public Sub AbrirPopupTransferencia(pstrOpcion As String)
		Try
			If IsNothing(pstrOpcion) Then
				If ValidarIngresoDetallesCanje("TRANSFERENCIAS") Then
					logCambiarPropiedadesPOPPUP = True

					RaiseEvent LanzarPopupTransferencias(Me, New System.EventArgs, GSTR_NUEVODETALLE_Plus)
				End If
			Else
				RaiseEvent LanzarPopupTransferencias(Me, New System.EventArgs, pstrOpcion)
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al abrir Popup Transferencia", Me.ToString(), "AbrirPopupTransferencia", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub AbrirPopupConsignacion(pstrOpcion As String)
		Try
			If ValidarIngresoDetallesCanje(String.Empty) Then
				'Se llena el combo de forma de pago de consignación.
				If Not IsNothing(_DiccionarioCombosOYDPlusCompleta) Then
					If _DiccionarioCombosOYDPlusCompleta.ContainsKey("FORMACONSIGNACION") Then
						Dim objLista As New List(Of OYDPLUSUtilidades.CombosReceptor)
						For Each li In _DiccionarioCombosOYDPlusCompleta("FORMACONSIGNACION")
							objLista.Add(li)
						Next

						If Not IsNothing(ListaFormaPagoConsignacion) Then
							ListaFormaPagoConsignacion.Clear()
						Else
							ListaFormaPagoConsignacion = New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
						End If

						ListaFormaPagoConsignacion.Add("FORMACONSIGNACION", objLista)
					End If
				End If

				VerFormaChequeConsignacionGrid = Visibility.Collapsed
				VerFormaChequeEfectivoConsignacion = Visibility.Collapsed
				VerFormaChequeConsignacion = Visibility.Collapsed

				If IsNothing(pstrOpcion) Then
					logCambiarPropiedadesPOPPUP = True

					RaiseEvent LanzarPopupConsignacion(Me, New System.EventArgs, GSTR_NUEVODETALLE_Plus)
				Else
					RaiseEvent LanzarPopupConsignacion(Me, New System.EventArgs, pstrOpcion)
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al abrir Popup Consignación", Me.ToString(), "AbrirPopupConsignacion", Program.TituloSistema, Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub LimpiarControlesOYDPLUS(ByVal pstrOpcion As String)
		Try
			Select Case pstrOpcion.ToUpper
				Case "RECEPTOR"

					LimpiarBuscadorCliente = String.Empty
			End Select
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al limpiar los controles.",
								 Me.ToString(), "LimpiarControles", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	Private Sub TesoreriaOrdenesPlusRC_Selected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _TesoreriaOrdenesPlusRC_Selected.PropertyChanged
		Try
			If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then

				If e.PropertyName = "strCodigoReceptor" Then
					IsBusyReceptor = True
					If logEditarRegistro = False Then
						HabilitarTipoProducto = True
					End If

					If HabilitarBuscadorCliente Then
						HabilitarBuscadorCliente = False
					End If
					IsBusyReceptor = False
					If Not TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor Is Nothing Then
						If BorrarCliente = True Then
							BorrarCliente = False
						End If
						BorrarCliente = True

						If ConsultarSaldo Then
							ConsultarSaldo = False
						End If
						ConsultarSaldo = True
						HabilitarEnEdicion = False
						CargarCombosOYDPLUS(TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor, String.Empty)
						If logXTesorero = False Then
							strCodigoReceptorBuscadorConcepto = TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor
						Else
							strCodigoReceptorBuscadorConcepto = String.Empty
						End If
					End If
				End If
				If e.PropertyName = "ValorTipoProducto" Then
					If BorrarCliente Then
						BorrarCliente = False
					End If
					BorrarCliente = True

					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						If logEsFondosOYD Then
							HabilitarConceptoDetalles = False
						Else
							HabilitarConceptoDetalles = True
						End If
					End If

					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						VerCargarPagosAFondos = Visibility.Visible
						VerCargarPagosA = Visibility.Collapsed
					Else
						VerCargarPagosAFondos = Visibility.Collapsed
						VerCargarPagosA = Visibility.Visible
					End If

					If logEditarRegistro = False Then
						TesoreriaOrdenesPlusRC_Selected.strNombre = String.Empty
						TesoreriaOrdenesPlusRC_Selected.strTipoIdentificacion = String.Empty
						TesoreriaOrdenesPlusRC_Selected.ValorTipoIdentificacion = String.Empty
						TesoreriaOrdenesPlusRC_Selected.strNroDocumento = String.Empty
						TesoreriaOrdenesPlusRC_Selected.strIDComitente = String.Empty
						CodigoOYDControles = String.Empty

						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							IDTipoClienteEntregado = GSTR_CLIENTE
							HabilitarEntregarA = False

							HabilitarBuscadorCliente = True
							HabilitarCategoriaFondos = True
							If logEsFondosOYD = False Then
								dtmFechaMenorPermitidaIngreso = dtmFechaServidor.Date
							End If
						Else
							HabilitarEntregarA = True
							If IDTipoClienteEntregado = GSTR_CLIENTE Then
								HabilitarBuscadorCliente = True
							Else
								HabilitarBuscadorCliente = False
							End If
							HabilitarCategoriaFondos = False
							dtmFechaMenorPermitidaIngreso = dtmFechaServidor.Date
						End If

						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD Then
							TipoCuentasBancarias = "cuentasbancarias_tesorero"
						ElseIf _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							TipoCuentasBancarias = "cuentasbancariascartera"
						Else
							TipoCuentasBancarias = "cuentasbancariasPLUS"
						End If

						If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
							VerDatosFondos = Visibility.Visible
							HabilitarCarteraColectiva = True
						Else
							VerDatosFondos = Visibility.Collapsed
							HabilitarCarteraColectiva = False
						End If

						If Editando Then
							CarteraColectivaFondos = String.Empty
							_TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos = String.Empty
							_TesoreriaOrdenesPlusRC_Selected.strTipoRetiroFondos = String.Empty
							_TesoreriaOrdenesPlusRC_Selected.strDescripcionTipoRetiroFondos = String.Empty
						End If
					Else
						If Not String.IsNullOrEmpty(TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto) Then
							HabilitarBuscadorCliente = True
						End If
					End If
					VerificarHabilitarTabsOrdenRecaudo(TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto, TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos)
				End If
				If e.PropertyName = "strTipoRetiroFondos" Or e.PropertyName = "strCarteraColectivaFondos" Then
					If e.PropertyName = "strCarteraColectivaFondos" Then
						If logNuevoRegistro And _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD Then
							ObtenerProximaFechaHabilFondo(dtmFechaMenorPermitidaIngreso)
						End If
						VerificarHabilitarTabsOrdenRecaudo(TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto, TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos)
					End If
				End If
				If e.PropertyName = "strNombre" Then
					Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(_TesoreriaOrdenesPlusRC_Selected.strNombre, clsExpresiones.TipoExpresion.Caracteres2)
					logCaracterInvalido = True
					If Not IsNothing(objValidacion) Then
						If objValidacion.TextoValido = False Then
							logCaracterInvalido = False
						Else
							logCaracterInvalido = True
						End If
					End If

					If logCaracterInvalido = False Then
						mostrarMensaje("El nombre contiene caracteres inválidos, por favor corregirlos para continuar", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					End If
				End If

			End If



		Catch ex As Exception
			IsBusyReceptor = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
								 Me.ToString(), "_TesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub CargarReceptoresUsuarioOYDPLUS(ByVal pstrUserState As String)
		Try
			IsBusy = True
			If Not IsNothing(dcProxyUtilidadesPLUS.tblReceptoresUsuarios) Then
				dcProxyUtilidadesPLUS.tblReceptoresUsuarios.Clear()
			End If
			If pstrUserState = "INICIO" Then
				dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarReceptoresUsuarioQuery(False, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptoresUsuario, pstrUserState)
			Else
				dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarReceptoresUsuarioQuery(True, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptoresUsuario, pstrUserState)
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó cargar los receptores del usuario.",
								 Me.ToString(), "CargarReceptoresUsuario", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub CargarParametrosReceptorOYDPLUS(ByVal pstrCodigoReceptor As String, ByVal pUserState As String)
		Try
			If logNuevoRegistro Then
				IsBusy = True
			End If
			If Not IsNothing(dcProxyUtilidadesPLUS.tblParametrosReceptors) Then
				dcProxyUtilidadesPLUS.tblParametrosReceptors.Clear()
			End If
			If Not String.IsNullOrEmpty(pstrCodigoReceptor) Then
				dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarParametrosReceptorQuery(pstrCodigoReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarParametrosReceptor, pUserState)
			Else
				dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarParametrosReceptorQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarParametrosReceptor, pUserState)
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los parametros del receptor.",
								 Me.ToString(), "CargarParametrosReceptor", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub ObtenerValoresDefectoOYDPLUS(ByVal pstrOpcion As String)
		Try
			If logNuevoRegistro Or logEditarRegistro Then
				Select Case pstrOpcion.ToUpper
					Case "RECEPTOR"
						If ListaReceptoresUsuario.Count > 1 Then
							If ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).Count > 0 Then
								TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor = ListaReceptoresUsuario.Where(Function(i) i.Prioridad = 0).FirstOrDefault.CodigoReceptor
							End If
						ElseIf ListaReceptoresUsuario.Count = 1 Then
							TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor = ListaReceptoresUsuario.FirstOrDefault.CodigoReceptor
						End If

						If String.IsNullOrEmpty(TesoreriaOrdenesPlusRC_Selected.strCodigoReceptor) Then
							IsBusyReceptor = False
						End If
					Case "COMBOSRECEPTOR"
						If Not IsNothing(ListaParametrosReceptor) Then
							logRealizarConsultaPropiedades = False

							'Obtiene los valores por defecto del receptor de la tabla de Parametros receptor.
							If ListaParametrosReceptor.Where(Function(i) i.Prioridad = 0).Count > 0 Then
								For Each li In ListaParametrosReceptor.Where(Function(i) i.Prioridad = 0)
									Select Case li.Topico.ToUpper
										Case "TIPOPRODUCTO"
											If logNuevoRegistro Then
												_TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = li.Valor
											End If
									End Select
								Next
							End If
							logRealizarConsultaPropiedades = True
						End If

						IsBusyReceptor = False
				End Select

				IsBusy = False
			Else
				If Not IsNothing(DiccionarioCombosOYDPlus) Then
					If DiccionarioCombosOYDPlus.ContainsKey("ESTADOS_PLUS") Then
						Dim objLista = New List(Of OYDPLUSUtilidades.CombosReceptor)

						For Each li In DiccionarioCombosOYDPlus("ESTADOS_PLUS").Where(Function(i) i.Retorno = "A" Or i.Retorno = "P" Or i.Retorno = "C")
							objLista.Add(New OYDPLUSUtilidades.CombosReceptor With {.Retorno = li.Retorno,
																			 .Descripcion = li.Descripcion,
																			 .ID = li.ID,
																			 .Categoria = li.Categoria})

						Next
						DiccionarioCombosOYDPlus("ESTADOS_PLUS") = Nothing
						DiccionarioCombosOYDPlus("ESTADOS_PLUS") = objLista
						MyBase.CambioItem("DiccionarioCombosOYDPlus")
					End If
				End If
			End If

			If Not IsNothing(DiccionarioCombosOYDPlus) Then
				If DiccionarioCombosOYDPlus.ContainsKey("SERVICIO_DOCUMENTOS") Then
					If DiccionarioCombosOYDPlus("SERVICIO_DOCUMENTOS").Count = 1 Then
						GSTR_SERVICIO_DOCUMENTOS = DiccionarioCombosOYDPlus("SERVICIO_DOCUMENTOS").FirstOrDefault.Descripcion
					End If
				End If
				If DiccionarioCombosOYDPlus.ContainsKey("CONTROL_CANJE_CHEQUES") Then
					If DiccionarioCombosOYDPlus("CONTROL_CANJE_CHEQUES").Count = 1 Then
						GSTR_CONTROL_CANJE_CHEQUE = DiccionarioCombosOYDPlus("CONTROL_CANJE_CHEQUES").FirstOrDefault.Descripcion
					End If
				End If
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto.",
								 Me.ToString(), "ObtenerValoresDefecto", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			IsBusyReceptor = False
		End Try
	End Sub

	Public Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean)
		Try
			Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
			Dim objListaCategoria As New List(Of OYDPLUSUtilidades.CombosReceptor)
			Dim objListaCategoria1 As New List(Of OYDPLUSUtilidades.CombosReceptor)

			If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
				For Each li In DiccionarioCombosOYDPlusCompleta
					objDiccionario.Add(li.Key, li.Value)
				Next
			End If

			If ValoresCompletos Then

				'************************************************************************************
				If Not IsNothing(objListaCategoria) Then
					objListaCategoria.Clear()
				End If

				If Not IsNothing(objDiccionario) Then
					If objDiccionario.ContainsKey("CANTIDADMAXDETALLETESORERIA") Then
						If objDiccionario("CANTIDADMAXDETALLETESORERIA").Count > 0 Then
							intCantidadMaximaDetalles = CInt(objDiccionario("CANTIDADMAXDETALLETESORERIA").First.Retorno)
						End If
					End If

					If objDiccionario.ContainsKey("HABILITARFONDOS") Then
						If objDiccionario("HABILITARFONDOS").Count > 0 Then
							If objDiccionario("HABILITARFONDOS").First.Retorno = "1" Then
								logHabilitarFuncionalidadFondos = True
							Else
								logHabilitarFuncionalidadFondos = False
							End If
						End If
					End If

					If objDiccionario.ContainsKey("TIPOACCIONFONDOS") Then
						If objDiccionario("TIPOACCIONFONDOS").Where(Function(i) i.Retorno = TipoAccionFondos).Count > 0 Then
							DescripcionTipoAccion = objDiccionario("TIPOACCIONFONDOS").Where(Function(i) i.Retorno = TipoAccionFondos).First.Descripcion
						End If
					End If

					If objDiccionario.ContainsKey("FECHAACTUAL_SERVIDOR") Then
						If objDiccionario("FECHAACTUAL_SERVIDOR").Count > 0 Then
							Try
								dtmFechaServidor = DateTime.ParseExact(objDiccionario("FECHAACTUAL_SERVIDOR").First.Retorno, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)
							Catch ex As Exception
								dtmFechaServidor = Now
							End Try

							FechaOrden = dtmFechaServidor
						End If
					End If

					If objDiccionario.ContainsKey("CF_UTILIZAPASIVA_A2") Then
						If objDiccionario("CF_UTILIZAPASIVA_A2").Count > 0 Then
							If objDiccionario("CF_UTILIZAPASIVA_A2").First.Retorno = "SI" Then
								logEsFondosOYD = True
								HabilitarCamposFondosOYD = True
							Else
								logEsFondosOYD = False
								HabilitarCamposFondosOYD = False
							End If
						End If
					End If

					If objDiccionario.ContainsKey("OYDPLUS_ORDENRECAUDO_DATOSOBLIGATORIOS") Then
						If objDiccionario("OYDPLUS_ORDENRECAUDO_DATOSOBLIGATORIOS").Count > 0 Then
							If objDiccionario("OYDPLUS_ORDENRECAUDO_DATOSOBLIGATORIOS").First.Retorno = "SI" Then
								logValidarCamposObligatoriosTransferenciaConsignacion = True
							Else
								logValidarCamposObligatoriosTransferenciaConsignacion = False
							End If
						End If
					End If

					If objDiccionario.ContainsKey("OYDPLUS_TITULORECAUDO_CARGARPAGOS") Then
						TituloPestanaCargarPagosA = objDiccionario("OYDPLUS_TITULORECAUDO_CARGARPAGOS").First.Retorno
					End If

					If objDiccionario.ContainsKey("OYDPLUS_TITULORECAUDO_CARGARPAGOSFONDOS") Then
						TituloPestanaCargarPagosAFondos = objDiccionario("OYDPLUS_TITULORECAUDO_CARGARPAGOSFONDOS").First.Retorno
					End If

					If objDiccionario.ContainsKey("A2_UTILIZAUNITY") Then
						If objDiccionario("A2_UTILIZAUNITY").Count > 0 Then
							If objDiccionario("A2_UTILIZAUNITY").First.Retorno = "SI" Then
								logEsFondosUnity = True
							Else
								logEsFondosUnity = False
							End If
						End If
					End If

				End If
			Else
				If Not IsNothing(objListaCategoria) Then
					objListaCategoria.Clear()
				End If

			End If

			DiccionarioCombosOYDPlus = objDiccionario

			ObtenerValoresDefectoOYDPLUS("COMBOSRECEPTOR")


		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de la clasificación.",
								 Me.ToString(), "ObtenerValoresCombos", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub CargarConfiguracionReceptorOYDPLUS(ByVal pUserState As String)
		Try
			If logNuevoRegistro Then
				IsBusy = True
			End If
			If Not IsNothing(dcProxyUtilidadesPLUS.tblConfiguracionesAdicionalesReceptors) Then
				dcProxyUtilidadesPLUS.tblConfiguracionesAdicionalesReceptors.Clear()
			End If
			CargarParametrosReceptorOYDPLUS(String.Empty, String.Empty)

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar la configuración del receptor.",
								 Me.ToString(), "CargarConfiguracionReceptor", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub CargarCombosOYDPLUS(ByVal pstrIDReceptor As String, ByVal pstrUserState As String)

		Try
			If logNuevoRegistro Or logEditarRegistro Then
				IsBusyReceptor = True
			End If

			If Not IsNothing(dcProxyUtilidadesPLUS.CombosReceptors) Then
				dcProxyUtilidadesPLUS.CombosReceptors.Clear()
			End If
			dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarCombosReceptorQuery(pstrIDReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYD, pstrUserState)
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.",
								 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
			IsBusyReceptor = False
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub GuardarTransferencia()
		GuardarDetalleTransferencia(True)
	End Sub

	Private Sub GuardarTransferenciaContinuar()
		Try
			GuardarDetalleTransferencia(False)
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "se presentó un problema en el guardado del registro transferencia",
														 Me.ToString(), "GuardarTransferenciaContinuar", Application.Current.ToString(), Program.Maquina, ex)
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub GuardarConsignacion()
		Try
			GuardarDetalleConsignacion(True)
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "se presentó un problema en el guardado del registro Consignacion",
														 Me.ToString(), "GuardarConsignacion", Application.Current.ToString(), Program.Maquina, ex)
			IsBusyDetalles = False
		End Try
	End Sub

	Private Sub GuardarConsignacionContinuar()
		Try
			GuardarDetalleConsignacion(False)
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "se presentó un problema en el guardado del registro Consignacion",
														 Me.ToString(), "GuardarConsignacionContinuar", Application.Current.ToString(), Program.Maquina, ex)
			IsBusyDetalles = False
		End Try
	End Sub

	Sub GuardarDetalleConsignacion(ByVal plogCerrar As Boolean)
		Try
			HabilitarEncabezadoConsignacion = True
			HabilitarEncabezadoConsignacionReferencia = True

			If ValidarCamposDiligenciadosConsignacion() Then

				Dim objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion = New List(Of TesoreriaOyDPlusConsignacionesRecibo)

				If Not String.IsNullOrEmpty(mstrArchivoConsignacion) Then
					If IsNothing(ListaArchivoConsignacion) Then
						ListaArchivoConsignacion = New List(Of InformacionArchivos)
					End If
				End If

				If logEditarConsignacion Then
					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then

						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.curValor = ValorGenerarConsignacion
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strTipo = GSTR_ORDENRECIBO
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strFormaPago = GSTR_CONSIGNACIONES
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.ValorFormaConsignacion = strTipoPagoConsignacion
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strFormaConsignacion = Descripcionformaconsignacion
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngNroReferencia = lngNroReferencia
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIdBancoDestino = lngCodigoBancoDestinoConsignacionWpp
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strCuentadestino = strCuentaConsignacion
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.ValorTipoCuentaDestino = strTipoCuentaConsignacion
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngNroCheque = lngNroChequeConsignacion
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDBanco = lngCodigoBancoConsignacionWpp
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strDescripcionBanco = strDescripcionBancoConsignacion
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strDescripcionCuentaConsignacion = strDescripcionCuentaConsignacion
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.strEstado = GSTR_PENDIENTE_Plus_Detalle
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.UsuarioWindows = Program.UsuarioWindows
						TesoreriaordenesplusRC_Detalle_Consignaciones_selected.logEsProcesada = True

						CalcularTotales(GSTR_CONSIGNACIONES)
						ValoresXdefectoWppConsignacion()
						BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemConsignacion")
						Habilitar_Encabezado()
						CalcularValorDisponibleCargar()

						If Not String.IsNullOrEmpty(mstrArchivoConsignacion) Then
							If ListaArchivoConsignacion.Where(Function(i) i.ID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle).Count > 0 Then
								ListaArchivoConsignacion.Where(Function(i) i.ID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle).First.NombreArchivo = mstrArchivoConsignacion
								ListaArchivoConsignacion.Where(Function(i) i.ID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle).First.RutaArchivo = mstrRutaConsignacion
								ListaArchivoConsignacion.Where(Function(i) i.ID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle).First.ByteArchivo = mabytArchivoConsignacion
							Else
								ListaArchivoConsignacion.Add(New InformacionArchivos With {.ID = TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle,
																						  .IDDetalle = IIf(TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle < 0, 0, TesoreriaordenesplusRC_Detalle_Consignaciones_selected.lngIDDetalle),
																						  .NombreArchivo = mstrArchivoConsignacion,
																						  .RutaArchivo = mstrRutaConsignacion,
																						  .ByteArchivo = mabytArchivoConsignacion})

							End If
						End If

					End If
				Else

					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
						objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion = ListatesoreriaordenesplusRC_Detalle_Consignaciones
					End If

					ListatesoreriaordenesplusRC_Detalle_Consignaciones = Nothing

					num = (objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion.Count + 1) * -1

					If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones) Then
						If ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones.Count > 0 Then
							Dim objTipoPagoConsignacion As String = String.Empty

							If _strTipoPagoConsignacion = GSTR_CONSIGNACION_AMBAS Then
								objTipoPagoConsignacion = GSTR_CHEQUE
							Else
								objTipoPagoConsignacion = _strTipoPagoConsignacion
							End If
							Descripcionformaconsignacion = "Cheque"

							For Each li In ListaTesoreriaOrdenesPlusRC_Detalle_ChequesConsignaciones
								objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion.Add(New TesoreriaOyDPlusConsignacionesRecibo With
																	  {.curValor = li.Valor,
																	   .strTipo = GSTR_ORDENRECIBO,
																	   .strFormaPago = GSTR_CONSIGNACIONES,
																	   .ValorFormaConsignacion = objTipoPagoConsignacion,
																	   .strFormaConsignacion = Descripcionformaconsignacion,
																	   .lngNroReferencia = lngNroReferencia,
																	   .lngIdBancoDestino = lngCodigoBancoDestinoConsignacionWpp,
																	   .strCuentadestino = strCuentaConsignacion,
																	   .ValorTipoCuentaDestino = strTipoCuentaConsignacion,
																	   .lngNroCheque = li.lngNroCheque,
																	   .lngIDBanco = li.lngIDBanco,
																	   .strDescripcionBanco = li.strDescripcionBanco,
																	   .strDescripcionCuentaConsignacion = strDescripcionCuentaConsignacion,
																	   .strEstado = GSTR_PENDIENTE_Plus_Detalle,
																	   .lngIDDetalle = num,
																	   .lngID = num,
																	   .UsuarioWindows = Program.UsuarioWindows,
																	   .logEsProcesada = True})

								If Not String.IsNullOrEmpty(mstrArchivoConsignacion) Then
									If ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).Count > 0 Then
										ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.NombreArchivo = mstrArchivoConsignacion
										ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.RutaArchivo = mstrRutaConsignacion
										ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.ByteArchivo = mabytArchivoConsignacion
									Else
										ListaArchivoConsignacion.Add(New InformacionArchivos With {.ID = num,
																					  .IDDetalle = 0,
																					  .NombreArchivo = mstrArchivoConsignacion,
																					  .RutaArchivo = mstrRutaConsignacion,
																					  .ByteArchivo = mabytArchivoConsignacion})

									End If
								End If

								num = (objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion.Count + 1) * -1

							Next
						Else
							objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion.Add(New TesoreriaOyDPlusConsignacionesRecibo With
																		   {.curValor = ValorGenerarConsignacion,
																			.strTipo = GSTR_ORDENRECIBO,
																			.strFormaPago = GSTR_CONSIGNACIONES,
																			.ValorFormaConsignacion = strTipoPagoConsignacion,
																			.lngNroReferencia = lngNroReferencia,
																			.lngIdBancoDestino = lngCodigoBancoDestinoConsignacionWpp,
																			.strCuentadestino = strCuentaConsignacion,
																			.strDescripcionCuentaConsignacion = strDescripcionCuentaConsignacion,
																			 .strFormaConsignacion = Descripcionformaconsignacion,
																			.ValorTipoCuentaDestino = strTipoCuentaConsignacion,
																			.strEstado = GSTR_PENDIENTE_Plus_Detalle,
																			.lngIDDetalle = num,
																			.lngID = num,
																			.logEsProcesada = True})

							If Not String.IsNullOrEmpty(mstrArchivoConsignacion) Then
								If ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).Count > 0 Then
									ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.NombreArchivo = mstrArchivoConsignacion
									ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.RutaArchivo = mstrRutaConsignacion
									ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.ByteArchivo = mabytArchivoConsignacion
								Else
									ListaArchivoConsignacion.Add(New InformacionArchivos With {.ID = num,
																							  .IDDetalle = 0,
																							  .NombreArchivo = mstrArchivoConsignacion,
																							  .RutaArchivo = mstrRutaConsignacion,
																							  .ByteArchivo = mabytArchivoConsignacion})

								End If
							End If

						End If

						If ValorGenerarEfectivoConsignacion > 0 Then
							num = (objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion.Count + 1) * -1

							objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion.Add(New TesoreriaOyDPlusConsignacionesRecibo With
																		  {.curValor = ValorGenerarEfectivoConsignacion,
																		   .strTipo = GSTR_ORDENRECIBO,
																		   .strFormaPago = GSTR_CONSIGNACIONES,
																		   .ValorFormaConsignacion = GSTR_EFECTIVO,
																		   .lngNroReferencia = lngNroReferencia,
																			.strFormaConsignacion = "Efectivo",
																		   .lngIdBancoDestino = lngCodigoBancoDestinoConsignacionWpp,
																		   .strCuentadestino = strCuentaConsignacion,
																		   .strDescripcionCuentaConsignacion = strDescripcionCuentaConsignacion,
																		   .ValorTipoCuentaDestino = strTipoCuentaConsignacion,
																		   .strEstado = GSTR_PENDIENTE_Plus_Detalle,
																		   .lngIDDetalle = num,
																		   .logEsProcesada = True})

							If Not String.IsNullOrEmpty(mstrArchivoConsignacion) Then
								If ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).Count > 0 Then
									ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.NombreArchivo = mstrArchivoConsignacion
									ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.RutaArchivo = mstrRutaConsignacion
									ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.ByteArchivo = mabytArchivoConsignacion
								Else
									ListaArchivoConsignacion.Add(New InformacionArchivos With {.ID = num,
																							  .IDDetalle = 0,
																							  .NombreArchivo = mstrArchivoConsignacion,
																							  .RutaArchivo = mstrRutaConsignacion,
																							  .ByteArchivo = mabytArchivoConsignacion})

								End If
							End If
						End If

					Else
						objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion.Add(New TesoreriaOyDPlusConsignacionesRecibo With
																		   {.curValor = ValorGenerarConsignacion,
																			.strTipo = GSTR_ORDENRECIBO,
																			.strFormaPago = GSTR_CONSIGNACIONES,
																			.ValorFormaConsignacion = strTipoPagoConsignacion,
																			.lngNroReferencia = lngNroReferencia,
																			 .strFormaConsignacion = Descripcionformaconsignacion,
																			.lngIdBancoDestino = lngCodigoBancoDestinoConsignacionWpp,
																			.strCuentadestino = strCuentaConsignacion,
																			.strDescripcionCuentaConsignacion = strDescripcionCuentaConsignacion,
																			.ValorTipoCuentaDestino = strTipoCuentaConsignacion,
																			.strEstado = GSTR_PENDIENTE_Plus_Detalle,
																			.lngIDDetalle = num,
																			.logEsProcesada = True})

						If Not String.IsNullOrEmpty(mstrArchivoConsignacion) Then
							If ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).Count > 0 Then
								ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.NombreArchivo = mstrArchivoConsignacion
								ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.RutaArchivo = mstrRutaConsignacion
								ListaArchivoConsignacion.Where(Function(i) i.ID = num And i.IDDetalle = 0).First.ByteArchivo = mabytArchivoConsignacion
							Else
								ListaArchivoConsignacion.Add(New InformacionArchivos With {.ID = num,
																						  .IDDetalle = 0,
																						  .NombreArchivo = mstrArchivoConsignacion,
																						  .RutaArchivo = mstrRutaConsignacion,
																						  .ByteArchivo = mabytArchivoConsignacion})

							End If
						End If
					End If

					ListatesoreriaordenesplusRC_Detalle_Consignaciones = objListaTesoreriaOrdenesPlusRC_Detalle_Consignacion

					If (TesoreriaOrdenesPlusRC_Selected.lngID <> 0) Then
						For Each li In ListatesoreriaordenesplusRC_Detalle_Consignaciones
							If li.lngIDTesoreriaEncabezado = 0 Or IsNothing(li.lngIDTesoreriaEncabezado) Then
								li.lngIDTesoreriaEncabezado = TesoreriaOrdenesPlusRC_Selected.lngID
							End If
						Next
					End If

					TesoreriaordenesplusRC_Detalle_Consignaciones_selected = ListatesoreriaordenesplusRC_Detalle_Consignaciones.LastOrDefault
					CalcularTotales(GSTR_CONSIGNACIONES)
					ValoresXdefectoWppConsignacion()
					BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemConsignacion")
					Habilitar_Encabezado()
					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						VerCargarPagosAFondos = Visibility.Visible
						VerCargarPagosA = Visibility.Collapsed
					Else
						VerCargarPagosAFondos = Visibility.Collapsed
						VerCargarPagosA = Visibility.Visible
					End If

					CalcularValorDisponibleCargar()
				End If

				If plogCerrar Then
					objWppConsignacion.Close()
				Else
					logEditarConsignacion = False
					logNuevaConsignacion = True
					VerFormaChequeConsignacionGrid = Visibility.Collapsed
					VerFormaChequeEfectivoConsignacion = Visibility.Collapsed
					VerFormaChequeConsignacion = Visibility.Collapsed

					'Recarga la lista de forma de pago para quitar la opción de efectivo.
					If Not IsNothing(_DiccionarioCombosOYDPlusCompleta) Then
						If _DiccionarioCombosOYDPlusCompleta.ContainsKey("FORMACONSIGNACION") Then
							Dim objLista As New List(Of OYDPLUSUtilidades.CombosReceptor)
							For Each li In _DiccionarioCombosOYDPlusCompleta("FORMACONSIGNACION")
								objLista.Add(li)
							Next

							Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
							objDiccionario.Add("FORMACONSIGNACION", objLista)
							ListaFormaPagoConsignacion = objDiccionario
						End If
					End If
				End If

				objWppConsignacion.ctlSubirArchivo.inicializarControl()
				configurarDocumentos(objWppConsignacion.ctlSubirArchivo, 0, GSTR_CONSIGNACIONES)
				mstrArchivoConsignacion = String.Empty
			End If

		Catch ex As Exception
			IsBusy = False
			IsBusyDetalles = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el guardado del registro Consignacion",
														 Me.ToString(), "GuardarDetalleConsignacion", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Private Sub CancelarGrabacionTransferencia()
		Try
			logEditarTransferenciaRC = False
			ValoresXdefectoWppTransferencia()
			objWppTransferencia_RC.Close()
			BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemTransferencia")
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cancelar Grabación Transferencia", Me.ToString(), "CancelarGrabacionTransferencia", Program.TituloSistema, Program.Maquina, ex)
			IsBusyDetalles = False
		End Try

	End Sub

	Private Sub LimpiarVariablesConfirmadas()
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
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub ObtenerValoresOrdenAnterior(ByVal pobjOrden As OyDPLUSTesoreria.TesoreriaOrdenesEncabezado, ByRef pobjOrdenSalvarDatos As OyDPLUSTesoreria.TesoreriaOrdenesEncabezado)
		Try
			If Not IsNothing(pobjOrden) Then
				Dim objNewOrdenOYD As New OyDPLUSTesoreria.TesoreriaOrdenesEncabezado

				objNewOrdenOYD.lngID = pobjOrden.lngID
				objNewOrdenOYD.strNombre = pobjOrden.strNombre
				objNewOrdenOYD.strTipoIdentificacion = pobjOrden.strTipoIdentificacion
				objNewOrdenOYD.ValorTipoIdentificacion = pobjOrden.ValorTipoIdentificacion
				objNewOrdenOYD.ValorTipoCta_Instrucciones = pobjOrden.ValorTipoCta_Instrucciones
				objNewOrdenOYD.ValorTipoIdentificacion_Instrucciones = pobjOrden.ValorTipoIdentificacion_Instrucciones
				objNewOrdenOYD.ValorTipoProducto = pobjOrden.ValorTipoProducto
				objNewOrdenOYD.ValorTipo = pobjOrden.ValorTipo
				objNewOrdenOYD.ValorEstado = pobjOrden.ValorEstado
				objNewOrdenOYD.strTipoProducto = pobjOrden.strTipoProducto
				objNewOrdenOYD.strTipo = pobjOrden.strTipo
				objNewOrdenOYD.dtmDocumento = pobjOrden.dtmDocumento
				objNewOrdenOYD.strNroDocumento = pobjOrden.strNroDocumento
				objNewOrdenOYD.lngIDDocumento = pobjOrden.lngIDDocumento
				objNewOrdenOYD.lngNroDocumento = pobjOrden.lngNroDocumento
				objNewOrdenOYD.strCodigoReceptor = pobjOrden.strCodigoReceptor
				objNewOrdenOYD.curValor = pobjOrden.curValor
				objNewOrdenOYD.strEstado = pobjOrden.strEstado
				objNewOrdenOYD.strIDComitente = pobjOrden.strIDComitente
				objNewOrdenOYD.strTipoCliente = pobjOrden.strTipoCliente

				CodigoOYDControles = pobjOrden.strIDComitente

				If ConsultarSaldo = True Then
					ConsultarSaldo = False
				End If
				ConsultarSaldo = True

				objNewOrdenOYD.logClientePresente = pobjOrden.logClientePresente
				objNewOrdenOYD.logClienteRecoge = pobjOrden.logClienteRecoge
				objNewOrdenOYD.logRecogeTercero = pobjOrden.logRecogeTercero
				objNewOrdenOYD.logConsignarCta = pobjOrden.logConsignarCta
				objNewOrdenOYD.logllevarDireccion = pobjOrden.logllevarDireccion
				objNewOrdenOYD.strTipoIdentificacion_Instrucciones = pobjOrden.strTipoIdentificacion_Instrucciones
				objNewOrdenOYD.strNroDocumento_Instrucciones = pobjOrden.strNroDocumento_Instrucciones
				objNewOrdenOYD.strNombre_Instrucciones = pobjOrden.strNombre_Instrucciones
				objNewOrdenOYD.logDireccionInscrita_Instrucciones = pobjOrden.logDireccionInscrita_Instrucciones
				objNewOrdenOYD.strCiudad_Instrucciones = pobjOrden.strCiudad_Instrucciones
				objNewOrdenOYD.strSector_Instrucciones = pobjOrden.strSector_Instrucciones
				objNewOrdenOYD.logEsTercero_Instrucciones = pobjOrden.logEsTercero_Instrucciones
				objNewOrdenOYD.logEsCtaInscrita_Instrucciones = pobjOrden.logEsCtaInscrita_Instrucciones
				objNewOrdenOYD.strTipoCta_Instrucciones = pobjOrden.strTipoCta_Instrucciones
				objNewOrdenOYD.strCuenta_Instrucciones = pobjOrden.strCuenta_Instrucciones

				objNewOrdenOYD.strTipoRetiroFondos = pobjOrden.strTipoRetiroFondos
				objNewOrdenOYD.strCarteraColectivaFondos = pobjOrden.strCarteraColectivaFondos

				pobjOrdenSalvarDatos = objNewOrdenOYD
				objNewOrdenOYD.strObservaciones = pobjOrden.strObservaciones
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", Me.ToString(), "ObtenerValoresOrdenAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	''' <summary>
	''' Consulta el valor real del saldo actual para el cliente
	''' </summary>
	''' <param name="pstrOpcion">Parámetro para identificar desde cuál control se está lanzando la consulta para asiganr la propiedad correspondiente</param>
	''' <remarks>EOMC -- 02-26-2013</remarks>

	''' <summary>
	''' Limpia el saldo según la opción en el parámetro
	''' </summary>
	''' <param name="pstrOpcion"></param>
	''' <remarks>EOMC -- 02-26-2013</remarks>
	Private Sub LimpiarSaldo(pstrOpcion As String)
		Try
			Select Case pstrOpcion
				Case "Cheque"
					ValorGenerar = 0
				Case "Transferencia"
					ValorGenerarTransferencia = 0
			End Select
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el saldo.", Me.ToString(), "LimpiarSaldo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	''' <summary>
	''' Metodo que obtiene el valor de las liquidaciones seleccionadas y sugiere el valor a el detalle seleccionado.
	''' Desarrollado por Juan David Correa
	''' Fecha 12 de marzo del 2013
	''' </summary>
	''' <param name="pstrOpcion"></param>
	''' <param name="pstrCliente"></param>
	''' <param name="pobjValores"></param>
	''' <remarks></remarks>
	Public Sub ObtenerLiquidacionesSeleccionadas(ByVal pstrOpcion As String, ByVal pstrCliente As String, ByVal pobjValores As A2OYDPLUSUtilidades.RetornoValoresLiquidacion)
		Try
			Dim dblValorLiquidacionesSugerido As Double = 0

			If Not IsNothing(pobjValores) Then
				If Not String.IsNullOrEmpty(pobjValores.strLiquidaciones) Then
					dblValorLiquidacionesSugerido = pobjValores.dblValorLiquidacionesSeleccionadas
					liquidacionesSelecciondas = pobjValores.strLiquidaciones

					liquidacionesSelecciondas = String.Format("Liquidaciones: {0} - Valor total:{1:C2}", liquidacionesSelecciondas, dblValorLiquidacionesSugerido)

					Select Case pstrOpcion.ToUpper
						Case "CHEQUE"
							ValorGenerar = dblValorLiquidacionesSugerido
						Case "TRANSFERENCIA"
							ValorGenerarTransferencia = dblValorLiquidacionesSugerido
					End Select

				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener las liquidaciones seleccionas.",
								 Me.ToString(), "ObtenerLiquidacionesSeleccionadas", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	''' <summary>
	''' Función que concatena el detalle del concepto de ordenes de tesoreria y retorna formateado para ser almacenado en la base de datos.
	''' Desarrollado por Juan David Correa
	''' Fecha 12 de marzo del 2013
	''' </summary>
	''' <param name="strDescripcionConcepto"></param>
	''' <param name="pstrDetalle"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function ConcatenarDetalle(ByVal strDescripcionConcepto As String, ByVal pstrDetalle As String, Optional ByVal pintNroEncargo As String = "") As String
		Try
			Dim strResultado As String = String.Empty

			If Not String.IsNullOrEmpty(liquidacionesSelecciondas) Then
				strResultado = String.Format("{0}-[{1}]-({2})", strDescripcionConcepto, liquidacionesSelecciondas, pstrDetalle)
			ElseIf Not (String.IsNullOrEmpty(strDescripcionConcepto)) And (Not String.IsNullOrEmpty(pintNroEncargo)) And logEsFondosOYD And _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
				strResultado = String.Format("{0} ENCARGO NRO {1}-({2})", strDescripcionConcepto, pintNroEncargo, pstrDetalle)
			ElseIf strDescripcionConcepto <> "" And pstrDetalle <> "" Then
				strResultado = String.Format("{0}-({1})", strDescripcionConcepto, pstrDetalle)
			ElseIf strDescripcionConcepto <> "" Then
				strResultado = String.Format("{0}", strDescripcionConcepto)
			ElseIf strDescripcionConcepto = "" And pstrDetalle <> "" Then
				strResultado = String.Format("{0}", pstrDetalle)
			End If

			Return strResultado
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concatenar los detalles.",
								Me.ToString(), "ConcatenarDetalle", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
			Return String.Empty
		End Try
	End Function

	Public Function ConcatenarConcepto(ByVal intIDConcepto As Integer) As String
		Try
			Dim strResultado As String = String.Empty

			If Not IsNothing(DiccionarioCombosOYDPlusTodosInicio) And Not IsNothing(intIDConcepto) Then
				If DiccionarioCombosOYDPlusTodosInicio.ContainsKey("CONCEPTOS") Then
					If DiccionarioCombosOYDPlusTodosInicio("CONCEPTOS").Where(Function(i) i.Retorno = intIDConcepto.ToString).Count > 0 Then
						strResultado = String.Format("{0} - {1}",
																	   DiccionarioCombosOYDPlusTodosInicio("CONCEPTOS").Where(Function(i) i.Retorno = intIDConcepto.ToString).First.Retorno,
																	   DiccionarioCombosOYDPlusTodosInicio("CONCEPTOS").Where(Function(i) i.Retorno = intIDConcepto.ToString).First.Descripcion
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
	Private Function RetornarValorDetalle(ByVal pstrDetalle As String, ByVal pstrCaracterInicial As String, ByVal pstrCaracterFinal As String) As String
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
			IsBusy = False
			IsBusyDetalles = False
			Return String.Empty
		End Try
	End Function

	Public Sub CargarArchivoRecibos(ByVal pstrNombreArchivo As String)
		Try
			ViewImportarArchivo.IsBusy = True
			If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
				dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
			End If

			'JABG20180413
			'Se adiciona el codigo OyD del encabezado y el tipo producto de la orden de recaudos para validar que coincida con los codigos OyD del archivo. Aplica solo para fondos
			dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionRecibosOyDPlusQuery(pstrNombreArchivo, "TesoreriaReciboPlus", Program.Usuario, Program.HashConexion, strCodigoOyDCliente, _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto), AddressOf TerminoCargarArchivoRecibos, String.Empty)

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.",
							   Me.ToString(), "CargarArchivoRecibos", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub SubirInformacionArchivoRecibo(ByVal objResultadoArchivo As List(Of OyDImportaciones.InformacionArchivoRecibos))
		Dim objRespaldoListaCheques As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusChequesRecibo)
		Dim objRespaldoListaTransferencias As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusTransferenciasRecibo)
		Dim objRespaldoListaConsignacion As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusConsignacionesRecibo)
		Dim objRespaldoCargarPagosA As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusCargosPagosARecibo)
		'JFSB 20180326 Se agrega objeto para la pestaña de cargar pagos a fondos
		Dim objRespaldoCargarPagosAFondos As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusCargosPagosAFondosRecibo)

		'JFSB 20180326 Se agrega validación para la pestaña de cargar pagos a fondos
		If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
			objRespaldoCargarPagosAFondos = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
		End If
		If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
			objRespaldoListaCheques = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
		End If
		If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
			objRespaldoListaTransferencias = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
		End If
		If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
			objRespaldoListaConsignacion = ListatesoreriaordenesplusRC_Detalle_Consignaciones
		End If
		If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
			objRespaldoCargarPagosA = ListatesoreriaordenesplusRC_Detalle_CargarPagosA
		End If

		Try
			Dim dblValorTotalDetalles As Double = 0

			'Recorre los detalles para insertar en el grid.

			'Recorre los detalles de cheques
			If objResultadoArchivo.Where(Function(i) i.CodigoDetalle = "CH").Count > 0 Then
				Dim objListaCheques As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusChequesRecibo)
				Dim intIDCheque As Integer = 0

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					objListaCheques = ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
				End If

				If objListaCheques.Count > 0 Then
					intIDCheque = objListaCheques.Last.lngIDDetalle + 1
				End If

				For Each li In objResultadoArchivo.Where(Function(i) i.CodigoDetalle = "CH").ToList
					Dim objCheque As New OyDPLUSTesoreria.TesoreriaOyDPlusChequesRecibo

					objCheque.lngIDDetalle = intIDCheque
					objCheque.strTipo = GSTR_ORDENRECIBO
					objCheque.strFormaPago = GSTR_CHEQUE
					objCheque.lngIDBanco = li.BancoOrigen
					objCheque.strDescripcionBanco = li.NombreBancoOrigen
					objCheque.lngNroCheque = li.NroCheque
					objCheque.logDireccionCliente = False

					If li.Mensajeria.ToLower = GSTR_CLIENTETRAECHEQUE.ToLower Then
						objCheque.logClienteTrae = True
						objCheque.logOtraDireccion = False
						objCheque.strDireccionCheque = String.Empty
						objCheque.strCiudad = String.Empty
						objCheque.strTelefono = String.Empty
						objCheque.strClienteTrae = "Si"
						objCheque.strDireccionCliente = String.Empty
						objCheque.strOtraDireccion = String.Empty
					Else
						objCheque.logClienteTrae = False
						objCheque.logOtraDireccion = True
						objCheque.strDireccionCheque = li.Direccion
						objCheque.strCiudad = li.Ciudad
						objCheque.strTelefono = li.Telefono
						objCheque.strClienteTrae = String.Empty
						objCheque.strDireccionCliente = String.Empty
						objCheque.strOtraDireccion = "Si"
					End If

					objCheque.strFormaPago = GSTR_CHEQUE
					objCheque.strTipo = GSTR_ORDENRECIBO
					objCheque.curValor = li.ValorCheque
					objCheque.strSector = li.Sector
					objCheque.strEstado = GSTR_PENDIENTE_Plus_Detalle
					objCheque.logEsProcesada = True

					dblValorTotalDetalles = dblValorTotalDetalles + objCheque.curValor

					objListaCheques.Add(objCheque)

					intIDCheque += 1
				Next

				ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = objListaCheques

				CalcularTotales(GSTR_CHEQUE)
			End If

			'Recorre los detalles de transferencias
			If objResultadoArchivo.Where(Function(i) i.CodigoDetalle = "TR").Count > 0 Then
				Dim objListaTransferencias As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusTransferenciasRecibo)
				Dim intIDTransferencia As Integer = 0

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					objListaTransferencias = ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
				End If

				If objListaTransferencias.Count > 0 Then
					intIDTransferencia = objListaTransferencias.Last.lngIDDetalle + 1
				End If

				For Each li In objResultadoArchivo.Where(Function(i) i.CodigoDetalle = "TR").ToList
					Dim objTransferencia As New OyDPLUSTesoreria.TesoreriaOyDPlusTransferenciasRecibo

					objTransferencia.lngIDDetalle = intIDTransferencia
					objTransferencia.strTipo = GSTR_ORDENRECIBO
					objTransferencia.strFormaPago = GSTR_TRANSFERENCIA

					If Not IsNothing(DiccionarioCombosOYDPlus) Then
						If DiccionarioCombosOYDPlus.ContainsKey("TIPOCUENTABANCARIA") Then
							If DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Descripcion.ToLower.Contains(li.TipoCuentaDestino.ToLower)).Count > 0 Then
								objTransferencia.ValorTipoCuentaDestino = DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Descripcion.ToLower.Contains(li.TipoCuentaDestino.ToLower)).First.Retorno
								objTransferencia.strTipoCuentadestino = DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Descripcion.ToLower.Contains(li.TipoCuentaDestino.ToLower)).First.Descripcion
							End If

							If DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Descripcion.ToLower.Contains(li.TipoCuentaDestino.ToLower)).Count > 0 Then
								objTransferencia.ValorTipoCuentaOrigen = DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Descripcion.ToLower.Contains(li.TipoCuentaOrigen.ToLower)).First.Retorno
								objTransferencia.strTipoCuentaOrigen = DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Descripcion.ToLower.Contains(li.TipoCuentaOrigen.ToLower)).First.Descripcion
							End If
						End If
					End If

					objTransferencia.curValor = li.Valor
					objTransferencia.lngIDBanco = li.BancoOrigen
					objTransferencia.strDescripcionBanco = li.NombreBancoOrigen
					objTransferencia.lngIdBancoDestino = li.BancoDestino
					objTransferencia.strDescripcionBancoDestino = li.NombreBancoDestino
					objTransferencia.strCuentadestino = li.CuentaDestino
					objTransferencia.strCuentaOrigen = li.CuentaOrigen
					objTransferencia.strEstado = GSTR_PENDIENTE_Plus_Detalle
					objTransferencia.logEsCuentaRegistrada = False
					objTransferencia.strEstado = GSTR_PENDIENTE_Plus_Detalle
					objTransferencia.logEsProcesada = True

					objListaTransferencias.Add(objTransferencia)

					intIDTransferencia += 1
				Next

				ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = objListaTransferencias
				CalcularTotales(GSTR_TRANSFERENCIA)

			End If

			If objResultadoArchivo.Where(Function(i) i.CodigoDetalle = "CO").Count > 0 Then
				Dim objListaConsignacion As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusConsignacionesRecibo)
				Dim intIDConsignacion As Integer = 0

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					objListaConsignacion = ListatesoreriaordenesplusRC_Detalle_Consignaciones
				End If

				If objListaConsignacion.Count > 0 Then
					intIDConsignacion = objListaConsignacion.Last.lngIDDetalle + 1
				End If

				For Each li In objResultadoArchivo.Where(Function(i) i.CodigoDetalle = "CO").ToList
					Dim objConsignacion As New OyDPLUSTesoreria.TesoreriaOyDPlusConsignacionesRecibo

					objConsignacion.lngIDDetalle = intIDConsignacion
					objConsignacion.curValor = li.Valor
					objConsignacion.strTipo = GSTR_ORDENRECIBO
					objConsignacion.strFormaPago = GSTR_CONSIGNACIONES

					If li.FormaConsignacion.ToLower = GSTR_DESCRIPCION_EFECTIVO.ToLower Then
						objConsignacion.ValorFormaConsignacion = GSTR_EFECTIVO
						objConsignacion.strFormaConsignacion = GSTR_DESCRIPCION_EFECTIVO
						objConsignacion.lngNroCheque = Nothing
						objConsignacion.lngIDBanco = Nothing
						objConsignacion.strDescripcionBanco = String.Empty
					Else
						objConsignacion.ValorFormaConsignacion = GSTR_CHEQUE
						objConsignacion.strFormaConsignacion = GSTR_DESCRIPCION_CHEQUE
						objConsignacion.lngNroCheque = li.NroCheque
						objConsignacion.lngIDBanco = li.BancoOrigen
						objConsignacion.strDescripcionBanco = li.NombreBancoOrigen
					End If

					objConsignacion.lngNroReferencia = li.NroReferencia
					objConsignacion.lngIdBancoDestino = li.BancoDestino
                    'DEMC20190604 Se asigna el codigo de la cuenta bancaria matriculada en bancos a la variable de la cuenta bancaria,ya que por el ingreso manual se encuentra de tal manera.
                    'objConsignacion.strCuentadestino = li.CuentaDestino
                    If Not IsNothing(li.BancoDestino) Then
                        objConsignacion.strCuentadestino = li.BancoDestino
                    Else
                        objConsignacion.strCuentadestino = ""
                    End If

                    'objConsignacion.strDescripcionCuentaConsignacion = String.Format("{0} - {1}", objConsignacion.strCuentadestino, objConsignacion.lngIdBancoDestino)
                    If Not IsNothing(objConsignacion.lngIdBancoDestino) Then 'Se arma la descripcion de la cuenta bancaria tal como esta por el ingreso manual.
                        objConsignacion.strDescripcionCuentaConsignacion = String.Format("{0} - {1}", objConsignacion.lngIdBancoDestino, li.NombreBancoDestino)
                    Else
                        objConsignacion.strDescripcionCuentaConsignacion = ""
                    End If
                    'DEMC20190604

                    If Not IsNothing(DiccionarioCombosOYDPlus) Then
						If DiccionarioCombosOYDPlus.ContainsKey("TIPOCUENTABANCARIA") Then
							If DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Descripcion.ToLower.Contains(li.TipoCuentaDestino.ToLower)).Count > 0 Then
								objConsignacion.ValorTipoCuentaDestino = DiccionarioCombosOYDPlus("TIPOCUENTABANCARIA").Where(Function(i) i.Descripcion.ToLower.Contains(li.TipoCuentaDestino.ToLower)).FirstOrDefault.Retorno
							End If
						End If
					End If

					objConsignacion.strEstado = GSTR_PENDIENTE_Plus_Detalle
					objConsignacion.logEsProcesada = True

					objListaConsignacion.Add(objConsignacion)

					intIDConsignacion += 1
				Next

				ListatesoreriaordenesplusRC_Detalle_Consignaciones = objListaConsignacion
				CalcularTotales(GSTR_CONSIGNACIONES)

			End If

			If objResultadoArchivo.Where(Function(i) i.CodigoDetalle = "PC").Count > 0 Then
				Dim objCargarPagosA As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusCargosPagosARecibo)

				'JFSB 20180326 Se agrega objeto para la pestaña de cargar pagos a fondos
				Dim objCargarPagosAFondos As New List(Of OyDPLUSTesoreria.TesoreriaOyDPlusCargosPagosAFondosRecibo)

				Dim intIDCargarPagosA As Integer = 0

				'JFSB 20180326 Se agrega validación para la pestaña de cargar pagos a fondos
				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then

					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
						objCargarPagosAFondos = ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
					End If

					If objCargarPagosAFondos.Count > 0 Then
						intIDCargarPagosA = objCargarPagosAFondos.Last.lngIDDetalle + 1
					End If
				Else

					If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
						objCargarPagosA = ListatesoreriaordenesplusRC_Detalle_CargarPagosA
					End If

					If objCargarPagosA.Count > 0 Then
						intIDCargarPagosA = objCargarPagosA.Last.lngIDDetalle + 1
					End If

				End If

				For Each li In objResultadoArchivo.Where(Function(i) i.CodigoDetalle = "PC").ToList

					'JFSB 20180326 Se agrega asociación de registros para la pestaña de cargar pagos a fondos
					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						Dim objPagosAFondos = New TesoreriaOyDPlusCargosPagosAFondosRecibo

						objPagosAFondos.lngIDDetalle = intIDCargarPagosA
						objPagosAFondos.strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOAFONDOS
						objPagosAFondos.strTipo = GSTR_ORDENRECIBO
						objPagosAFondos.strCodigoOyD = li.CodigoOYD
						objPagosAFondos.curValor = li.Valor
						objPagosAFondos.curValorTotal = dblValorTotalDetalles


						'JABG2018
						'Cuando es tipo producto fondos en el Tab de cargar pagos a fondos no se pueden incluir terceros  

						'If LTrim(RTrim(li.CodigoOYD)) = LTrim(RTrim(strCodigoOyDCliente)) Then
						objPagosAFondos.ValorTipoCliente = GSTR_CLIENTE
						objPagosAFondos.strIDTipoCliente = "Cliente"
						'Else
						'    objPagosAFondos.ValorTipoCliente = GSTR_TERCERO
						'    objPagosAFondos.strIDTipoCliente = "Tercero"
						'End If

						'JABG20180413
						'Se realiza cambio para adicionar el tipo de accion y el encargo porque esta informacion es necesaria para complemetar la orden en el tesorero
						Dim strDescripcionTipoAccion As String = String.Empty
						If Not IsNothing(DiccionarioCombosOYDPlus) Then
							If DiccionarioCombosOYDPlus.ContainsKey("TIPOACCIONFONDOS") Then
								If DiccionarioCombosOYDPlus("TIPOACCIONFONDOS").Where(Function(i) i.Retorno = TipoAccionFondos).Count > 0 Then
									strDescripcionTipoAccion = DiccionarioCombosOYDPlus("TIPOACCIONFONDOS").Where(Function(i) i.Retorno = TipoAccionFondos).First.Descripcion
								End If
							End If
						End If

						objPagosAFondos.strDescripcionTipoAccionFondos = strDescripcionTipoAccion
						objPagosAFondos.strTipoAccionFondos = TipoAccionFondos
						objPagosAFondos.strCarteraColectivaFondos = CarteraColectivaFondos

						objPagosAFondos.logEsProcesada = True

						objCargarPagosAFondos.Add(objPagosAFondos)
					Else
						Dim objPagosA = New TesoreriaOyDPlusCargosPagosARecibo

						objPagosA.lngIDDetalle = intIDCargarPagosA
						objPagosA.strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOA
						objPagosA.strTipo = GSTR_ORDENRECIBO
						objPagosA.strCodigoOyD = li.CodigoOYD
						objPagosA.curValor = li.Valor
						objPagosA.curValorTotal = dblValorTotalDetalles
						If LTrim(RTrim(li.CodigoOYD)) = LTrim(RTrim(strCodigoOyDCliente)) Then
							objPagosA.ValorTipoCliente = GSTR_CLIENTE
							objPagosA.strIDTipoCliente = "Cliente"
						Else
							objPagosA.ValorTipoCliente = GSTR_TERCERO
							objPagosA.strIDTipoCliente = "Tercero"
						End If

						objPagosA.logEsProcesada = True

						objCargarPagosA.Add(objPagosA)
					End If

					intIDCargarPagosA += 1
				Next

				'JFSB 20180326 Se agrega asociación de registros para la pestaña de cargar pagos a fondos
				If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
					ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos = objCargarPagosAFondos
				Else
					ListatesoreriaordenesplusRC_Detalle_CargarPagosA = objCargarPagosA
				End If
				CalcularValorDisponibleCargar()

			End If

			ViewImportarArchivo.IsBusy = False

			If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
				VerCargarPagosAFondos = Visibility.Visible
				VerCargarPagosA = Visibility.Collapsed
			Else
				VerCargarPagosAFondos = Visibility.Collapsed
				VerCargarPagosA = Visibility.Visible
			End If


		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al subir la información del archivo.",
							 Me.ToString(), "SubirInformacionArchivoRecibo", Application.Current.ToString(), Program.Maquina, ex)

			ViewImportarArchivo.IsBusy = False
			ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = objRespaldoListaCheques
			ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = objRespaldoListaTransferencias
			ListatesoreriaordenesplusRC_Detalle_Consignaciones = objRespaldoListaConsignacion
			ListatesoreriaordenesplusRC_Detalle_CargarPagosA = objRespaldoCargarPagosA
			IsBusy = False
			IsBusyDetalles = False
			VerCargarPagosA = Visibility.Collapsed
			VerCargarPagosAFondos = Visibility.Collapsed
		End Try
	End Sub

	Public Sub HabilitarSubirArchivo()
		Try
			If logEditarRegistro Or logNuevoRegistro Then
				If Not String.IsNullOrEmpty(IDTipoClienteEntregado) And Not IsNothing(IDTipoClienteEntregado) Then
					If IDTipoClienteEntregado = GSTR_TERCERO Then
						If Not String.IsNullOrEmpty(strNombreEntregado) And Not String.IsNullOrEmpty(strNroDocumentoEntregado) And Not String.IsNullOrEmpty(strTipoIdentificacionEntregado) Then
							HabilitarImportacion = True
						Else
							HabilitarImportacion = False
						End If
					Else
						If Not IsNothing(ComitenteSeleccionado) Then
							If Not String.IsNullOrEmpty(ComitenteSeleccionado.CodigoOYD) Then
								HabilitarImportacion = True
							Else
								HabilitarImportacion = False
							End If
						Else
							HabilitarImportacion = False
						End If
					End If
				Else
					HabilitarImportacion = False
				End If
			Else
				HabilitarImportacion = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar el archivo.",
							   Me.ToString(), "HabilitarSubirArchivo", Application.Current.ToString(), Program.Maquina, ex)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub ConsultarCarterasColectivasFondos(ByVal pstrCodigoOYD As String, ByVal plogConsultarTodasLasCarteras As Boolean, Optional ByVal pstrUserState As String = "")
		Try
			If plogConsultarTodasLasCarteras Or pstrUserState = "SOLOCARGARLISTACOMPLETA" Then
				If Not IsNothing(dcProxy.CarterasColectivasClientes) Then
					dcProxy.CarterasColectivasClientes.Clear()
				End If
				dcProxy.Load(dcProxy.ConsultarCarterasColectivasClienteQuery(pstrCodigoOYD, plogConsultarTodasLasCarteras, Program.Usuario, Program.HashConexion, String.Empty), AddressOf TerminoConsultarCarterasColectivasClientes, pstrUserState)
			Else
				If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) Then
					If _TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
						MostrarCarteraColectivasFondos = Visibility.Visible
						If Not IsNothing(dcProxy.CarterasColectivasClientes) Then
							dcProxy.CarterasColectivasClientes.Clear()
						End If
						dcProxy.Load(dcProxy.ConsultarCarterasColectivasClienteQuery(pstrCodigoOYD, plogConsultarTodasLasCarteras, Program.Usuario, Program.HashConexion, String.Empty), AddressOf TerminoConsultarCarterasColectivasClientes, pstrUserState)
					Else
						MostrarCarteraColectivasFondos = Visibility.Collapsed
						ListaCarterasColectivasClienteCompleta = Nothing
						ListaEncargosCarteraColectiva = Nothing
					End If
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar sí se habilitaban los campos de fondos.",
							   Me.ToString(), "VerificarHabilitarFondos", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Private Sub TerminoConsultarCarterasColectivasClientes(ByVal lo As LoadOperation(Of OyDPLUSTesoreria.CarterasColectivasClientes))
		Try
			If lo.HasError = False Then
				If lo.UserState = "TODASLASCARTERAS" Then
					ListaTodasLasCarterasColectivas = lo.Entities.ToList
				ElseIf lo.UserState = "SOLOCARGARLISTACOMPLETA" Then
					ListaCarterasColectivasClienteCompleta = lo.Entities.ToList
				Else
					ListaCarterasColectivasClienteCompleta = lo.Entities.ToList

					Dim objListaNuevaCarteras As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)

					If TipoAccionFondos = GSTR_FONDOS_TIPOACCION_CONSTITUCION Then
						HabilitarNroEncargo = False

						If Not IsNothing(_ListaTodasLasCarterasColectivas) Then
							For Each li In _ListaTodasLasCarterasColectivas
								If objListaNuevaCarteras.Where(Function(i) i.CarteraColectiva = li.CarteraColectiva).Count = 0 Then
									objListaNuevaCarteras.Add(New OyDPLUSTesoreria.CarterasColectivasClientes With {.ID = li.ID,
																												.CarteraColectiva = li.CarteraColectiva,
																												.NombreCarteraColectiva = li.NombreCarteraColectiva,
																												.CodigoOYD = String.Empty,
																												.NroEncargo = String.Empty,
																												.Saldo = 0,
																												.SaldoDisponible = 0,
																													.dtmFechaCierre = li.dtmFechaCierre})
								End If
							Next
						End If
					Else
						HabilitarNroEncargo = True

						If Not IsNothing(_ListaCarterasColectivasClienteCompleta) Then
							For Each li In _ListaCarterasColectivasClienteCompleta
								If objListaNuevaCarteras.Where(Function(i) i.CarteraColectiva = li.CarteraColectiva).Count = 0 Then
									objListaNuevaCarteras.Add(New OyDPLUSTesoreria.CarterasColectivasClientes With {.ID = li.ID,
																												.CarteraColectiva = li.CarteraColectiva,
																												.NombreCarteraColectiva = li.NombreCarteraColectiva,
																												.CodigoOYD = li.CodigoOYD,
																												.NroEncargo = li.NroEncargo,
																												.Saldo = li.Saldo,
																												.SaldoDisponible = li.SaldoDisponible,
																													.dtmFechaCierre = li.dtmFechaCierre})
								End If
							Next
						End If
					End If

					If lo.UserState = "CARTERASCLIENTEENCABEZADO" Then
						ListaCarterasColectivas = objListaNuevaCarteras
						If Not IsNothing(TesoreriaOrdenesPlusRC_Selected) Then
							If Not String.IsNullOrEmpty(TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos) Then
								If Not IsNothing(ListaCarterasColectivas) Then
									If ListaCarterasColectivas.Count > 0 Then
										If ListaCarterasColectivas.Where(Function(i) i.CarteraColectiva = TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos).Count > 0 Then
											CarteraColectivaFondos = ListaCarterasColectivas.Where(Function(i) i.CarteraColectiva = TesoreriaOrdenesPlusRC_Selected.strCarteraColectivaFondos).FirstOrDefault.CarteraColectiva
										End If
									End If
								End If
							End If
						End If
					ElseIf lo.UserState = "ENCARGOS_CARTERACOLECTIVA" Or lo.UserState = "ENCARGOS_CARTERACOLECTIVA_EDICION" Then
						Dim objNuevaListaEncargos As New List(Of OyDPLUSTesoreria.CarterasColectivasClientes)
						For Each li In _ListaCarterasColectivasClienteCompleta
							If li.CarteraColectiva = _CarteraColectivaFondos And Not String.IsNullOrEmpty(li.NroEncargo) Then
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

						ListaEncargosCarteraColectiva = objNuevaListaEncargos

						If lo.UserState = "ENCARGOS_CARTERACOLECTIVA_EDICION" Then
							NroEncargoFondos = _TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.intNroEncargoFondos
						End If
					End If

				End If

			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar sí se habilitaban los campos de fondos.",
							   Me.ToString(), "TerminoConsultarCarterasColectivasClientes", Application.Current.ToString(), Program.Maquina, lo.Error)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las carteras colectivas del cliente.",
							   Me.ToString(), "TerminoConsultarCarterasColectivasClientes", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub
	Public Sub OrganizarDetallesRechazados()
		Try
			If logEditarRegistro Or logNuevoRegistro Then
				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					For Each x In ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
						If Not String.IsNullOrEmpty(x.strEstado) Then
							If x.strEstado.ToUpper = "RECHAZADO" Then
								x.UsuarioWindows = Program.UsuarioWindows
								x.strFormaPago = "C"
							End If
						End If
					Next
				End If

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					For Each x In ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
						If Not String.IsNullOrEmpty(x.strEstado) Then
							If x.strEstado.ToUpper = "RECHAZADO" Then
								x.strFormaPago = "T"
								x.UsuarioWindows = Program.UsuarioWindows
							End If
						End If
					Next
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					For Each x In ListatesoreriaordenesplusRC_Detalle_Consignaciones
						If Not String.IsNullOrEmpty(x.strEstado) Then
							If x.strEstado.ToUpper = "RECHAZADO" Then
								x.strFormaPago = "CG"
								x.UsuarioWindows = Program.UsuarioWindows
							End If
						End If
					Next
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosA) Then
					For Each x In ListatesoreriaordenesplusRC_Detalle_CargarPagosA
						x.strFormaPago = "CP"
						x.UsuarioWindows = Program.UsuarioWindows
					Next
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos) Then
					For Each x In ListatesoreriaordenesplusRC_Detalle_CargarPagosAFondos
						x.strFormaPago = "CPF"
						x.UsuarioWindows = Program.UsuarioWindows
					Next
				End If

			End If

		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Calcular Totales", Me.ToString(), "CalcularTotales", Program.TituloSistema, Program.Maquina, ex)
		End Try

	End Sub

	Public Sub VerificarRestriccionesTipoCartera(Optional ByVal pstrUserState As String = "")
		Try
			If Not IsNothing(_TesoreriaOrdenesPlusRC_Selected) And logNuevoRegistro Then
				If Not String.IsNullOrEmpty(CarteraColectivaFondos) _
					And Not String.IsNullOrEmpty(TipoAccionFondos) Then
					dcProxy.VerificarRestriccionesTipoCartera(TipoAccionFondos,
															  _TesoreriaOrdenesPlusRC_Selected.strIDComitente,
															  CarteraColectivaFondos,
															  NroEncargoFondos, 'C-20190384  JAPC20200219:se ajusta envio nro encargo
															  Program.Usuario,
															  FechaAplicacion, Program.HashConexion,
															  AddressOf TerminoVerificarRestriccionesTipoCartera,
															  pstrUserState)
				End If
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la existencia de los tipos de cartera.", Me.ToString(), "VerificarRestriccionesTipoCartera", Program.TituloSistema, Program.Maquina, ex)
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

	Public Sub TextoCampoSubirArchivoDetalle(ByVal plogNuevo As Boolean, Optional ByVal pstrNombreArchivo As String = "")
		Try
			If plogNuevo Then
				TextoArchivoSeleccionadoDetalle = "Seleccione el archivo ..."
			Else
				TextoArchivoSeleccionadoDetalle = pstrNombreArchivo
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el texto del control de subir archivo detalle",
												 Me.ToString(), "TextoCampoSubirArchivoDetalle", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	Public Sub TextoCampoSubirArchivoDetalle(ByVal pstrFormaPago As String, Optional ByVal pobjDetalleCheque As TesoreriaOyDPlusChequesRecibo = Nothing, Optional ByVal pobjDetalleTransferencia As TesoreriaOyDPlusTransferenciasRecibo = Nothing, Optional ByVal pobjDetalleConsignacion As TesoreriaOyDPlusConsignacionesRecibo = Nothing)
		Try
			Dim logRegistroEncontrado As Boolean = False
			Dim objNombreArchivo As String() = Nothing

			If pstrFormaPago = GSTR_CHEQUE Then
				If Not IsNothing(pobjDetalleCheque) Then
					If Not IsNothing(_ListaArchivosCheque) Then
						For Each li In _ListaArchivosCheque
							If li.ID = pobjDetalleCheque.lngIDDetalle Then
								TextoArchivoSeleccionadoDetalle = li.NombreArchivo
								logRegistroEncontrado = True
								Exit For
							End If
						Next
					End If

					If logRegistroEncontrado = False Then
						If Not String.IsNullOrEmpty(pobjDetalleCheque.strUrlArchivo) Then
							If pobjDetalleCheque.strUrlArchivo.Contains("/") Then
								objNombreArchivo = pobjDetalleCheque.strUrlArchivo.Split("/")
								If Not IsNothing(objNombreArchivo) Then
									TextoArchivoSeleccionadoDetalle = objNombreArchivo.Last()
								End If
							End If
						End If
					End If
				End If
			ElseIf pstrFormaPago = GSTR_TRANSFERENCIA Then
				If Not IsNothing(pobjDetalleTransferencia) Then
					If Not IsNothing(_ListaArchivoTransferencia) Then
						For Each li In _ListaArchivoTransferencia
							If li.ID = pobjDetalleTransferencia.lngIDDetalle Then
								TextoArchivoSeleccionadoDetalle = li.NombreArchivo
								logRegistroEncontrado = True
								Exit For
							End If
						Next
					End If

					If logRegistroEncontrado = False Then
						If Not String.IsNullOrEmpty(pobjDetalleTransferencia.strUrlArchivo) Then
							If pobjDetalleTransferencia.strUrlArchivo.Contains("/") Then
								objNombreArchivo = pobjDetalleTransferencia.strUrlArchivo.Split("/")
								If Not IsNothing(objNombreArchivo) Then
									TextoArchivoSeleccionadoDetalle = objNombreArchivo.Last()
								End If
							End If
						End If
					End If
				End If
			ElseIf pstrFormaPago = GSTR_CONSIGNACIONES Then
				If Not IsNothing(pobjDetalleConsignacion) Then
					If Not IsNothing(_ListaArchivoConsignacion) Then
						For Each li In _ListaArchivoConsignacion
							If li.ID = pobjDetalleConsignacion.lngIDDetalle Then
								TextoArchivoSeleccionadoDetalle = li.NombreArchivo
								logRegistroEncontrado = True
								Exit For
							End If
						Next
					End If

					If logRegistroEncontrado = False Then
						If Not String.IsNullOrEmpty(pobjDetalleConsignacion.strUrlArchivo) Then
							If pobjDetalleConsignacion.strUrlArchivo.Contains("/") Then
								objNombreArchivo = pobjDetalleConsignacion.strUrlArchivo.Split("/")
								If Not IsNothing(objNombreArchivo) Then
									TextoArchivoSeleccionadoDetalle = objNombreArchivo.Last()
								End If
							End If
						End If
					End If
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el texto del control de subir archivo detalle",
												 Me.ToString(), "TextoCampoSubirArchivoDetalle", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Metodo para consultar los combos permitidos para la cartera o fondo
	''' </summary>
	''' <param name="pstrCarteraColectiva"></param>
	''' <param name="pstrUserState"></param>
	''' <remarks></remarks>
	Private Sub consultarCombosEspecificosFondo(ByVal pstrCarteraColectiva As String, Optional ByVal pstrUserState As String = "")
		Try
			If logNuevoRegistro Then
				If Not IsNothing(dcProxyUtilidades.BuscadorOrdenantes) Then
					dcProxyUtilidades.BuscadorOrdenantes.Clear()
				End If

				If Not IsNothing(dcProxyUtilidades.ItemCombos) Then
					dcProxyUtilidades.ItemCombos.Clear()
				End If

				dcProxyUtilidades.Load(dcProxyUtilidades.cargarCombosCondicionalQuery("COMBOS_DEPENDIENTES_CARTERA", pstrCarteraColectiva, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCombosEspecificosCartera, pstrUserState)
			Else
				Dim objListaComboTipoAccion As New List(Of OYDUtilidades.ItemCombo)
				If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
					If DiccionarioCombosOYDPlusCompleta.ContainsKey("TIPOACCIONFONDOS") Then
						For Each li In DiccionarioCombosOYDPlusCompleta("TIPOACCIONFONDOS")
							objListaComboTipoAccion.Add(New OYDUtilidades.ItemCombo With {.Categoria = li.Categoria,
																						  .Descripcion = li.Descripcion,
																						  .ID = li.Retorno,
																						  .intID = li.ID,
																						  .Retorno = li.Retorno})
						Next
					End If
				End If

				ListaTiposAccionFondos = objListaComboTipoAccion
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los combos especificos del fondo.", Me.ToString, "consultarCombosEspecificosFondo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
		End Try
	End Sub

	Public Sub ConsultarComitentesClienteEncabezado(ByVal pstrIDComitente As String)
		Try
			dcProxyUtilidades.BuscadorClientes.Clear()
			dcProxyUtilidades.Load(dcProxyUtilidades.buscarClientesQuery(String.Empty, "T", "C", "id*" & pstrIDComitente, Program.Usuario, False, Nothing, Program.HashConexion), AddressOf TerminoConsultarClientesRelacionadosEncabezado, String.Empty)
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los comitentes del cliente seleccionado en el encabezado.", Me.ToString, "ConsultarComitentesClienteEncabezado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
		End Try
	End Sub

	Public Sub TerminoConsultarClientesRelacionadosEncabezado(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
		Try
			If lo.HasError = False Then
				ListaClientesEncabezado = lo.Entities.ToList
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los comitentes del cliente seleccionado en el encabezado.", Me.ToString, "TerminoConsultarClientesRelacionadosEncabezado", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
				IsBusy = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los comitentes del cliente seleccionado en el encabezado.", Me.ToString, "TerminoConsultarClientesRelacionadosEncabezado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
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

			colorFondo = Program.colorFromHex(COLOR_HABILITADO)

			FondoTextoBuscadoresHabilitado = New SolidColorBrush(colorFondo)
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el color de fondo del texto.", Me.ToString(), "CambiarColorFondoTextoBuscador", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Function ValidarIngresoDetallesCanje(ByVal pstrOpcion As String, Optional ByVal plogMostrarMensaje As Boolean = True) As Boolean
		Try
			Dim logRetorno As Boolean = True
			Dim HayCheques As Boolean = False
			Dim HayTransferencias As Boolean = False
			Dim HayConsignacionesCheques As Boolean = False
			Dim HayConsignacionesEfectivo As Boolean = False

			If GSTR_CONTROL_CANJE_CHEQUE = "SI" Then
				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Cheques) Then
					If ListaTesoreriaOrdenesPlusRC_Detalle_Cheques.Count > 0 Then
						HayCheques = True
					Else
						If pstrOpcion = "CHEQUES" Then
							HayCheques = True
						Else
							HayCheques = False
						End If
					End If
				Else
					If pstrOpcion = "CHEQUES" Then
						HayCheques = True
					Else
						HayCheques = False
					End If
				End If

				If Not IsNothing(ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias) Then
					If ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias.Count > 0 Then
						HayTransferencias = True
					Else
						If pstrOpcion = "TRANSFERENCIAS" Then
							HayTransferencias = True
						Else
							HayTransferencias = False
						End If
					End If
				Else
					If pstrOpcion = "TRANSFERENCIAS" Then
						HayTransferencias = True
					Else
						HayTransferencias = False
					End If
				End If

				If Not IsNothing(ListatesoreriaordenesplusRC_Detalle_Consignaciones) Then
					If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Where(Function(i) i.ValorFormaConsignacion = GSTR_EFECTIVO).Count > 0 Then
						HayConsignacionesEfectivo = True
					Else
						If pstrOpcion = "CONSIGNACION_EFECTIVO" Then
							HayConsignacionesEfectivo = True
						Else
							HayConsignacionesEfectivo = False
						End If
					End If

					If ListatesoreriaordenesplusRC_Detalle_Consignaciones.Where(Function(i) i.ValorFormaConsignacion = GSTR_CHEQUE).Count > 0 Then
						HayConsignacionesCheques = True
					Else
						If pstrOpcion = "CONSIGNACION_CHEQUES" Then
							HayConsignacionesCheques = True
						Else
							HayConsignacionesCheques = False
						End If
					End If
				Else
					If pstrOpcion = "CONSIGNACION_CHEQUES" Then
						HayConsignacionesCheques = True
					Else
						HayConsignacionesCheques = False
					End If
					If pstrOpcion = "CONSIGNACION_EFECTIVO" Then
						HayConsignacionesEfectivo = True
					Else
						HayConsignacionesEfectivo = False
					End If
				End If

				If (TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto <> GSTR_FONDOS_TIPOPRODUCTO Or
					TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO And logEsFondosOYD) And
				(((HayCheques Or HayConsignacionesCheques) And HayTransferencias) Or
				 ((HayCheques Or HayConsignacionesCheques) And HayConsignacionesEfectivo)) Then
					'Or
					'(HayTransferencias And HayConsignacionesEfectivo)

					mostrarMensaje("Cuando esta habilitado el control de canje de cheques, solo puede ser 1 sola forma de pago en los detalles de la orden.", "Ordenes de recaudo.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					logRetorno = False
					IsBusy = False
				End If
			End If

			Return logRetorno
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el parametro de cheques en canje.", Me.ToString(), "ValidarIngresoDetallesCanje", Application.Current.ToString(), Program.Maquina, ex)
			Return False
		End Try
	End Function

	Public Sub VerificarHabilitarTabsOrdenRecaudo(ByVal pstrTipoProducto As String, ByVal pstrCarteraColectiva As String, Optional ByVal pstrUserState As String = "")
		Try
			dcProxy.tbl_TabHabilitars.Clear()
			dcProxy.Load(dcProxy.OrdenRecaudo_ValidarTabHabilitarQuery(pstrTipoProducto, pstrCarteraColectiva, Program.Maquina, Program.Usuario, Program.UsuarioWindows, Program.HashConexion), AddressOf TerminoVerificarTabControlOrdenRecaudo, pstrUserState)
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta para obtener los tabs que se deben de habilitar.", Me.ToString, "TerminoVerificarTabControlOrdenRecaudo", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	Private Sub TerminoVerificarTabControlOrdenRecaudo(ByVal lo As LoadOperation(Of OyDPLUSTesoreria.tbl_TabHabilitar))
		Try
			If lo.HasError = False Then
				If lo.Entities.Count > 0 Then
					'TAB CHEQUE
					If lo.Entities.Where(Function(i) i.strTabHabilitar = GSTR_TABORDENESRECAUDO_CHEQUE_CODIGOINTERNO).Count > 0 Then
						DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_CHEQUE_CODIGO).TabVisible = Visibility.Visible
					Else
						DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_CHEQUE_CODIGO).TabVisible = Visibility.Collapsed
					End If
					'TAB TRANSFERENCIA
					If lo.Entities.Where(Function(i) i.strTabHabilitar = GSTR_TABORDENESRECAUDO_TRANSFERENCIA_CODIGOINTERNO).Count > 0 Then
						DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_TRANSFERENCIA_CODIGO).TabVisible = Visibility.Visible
					Else
						DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_TRANSFERENCIA_CODIGO).TabVisible = Visibility.Collapsed
					End If
					'TAB CONSIGNACION
					If lo.Entities.Where(Function(i) i.strTabHabilitar = GSTR_TABORDENESRECAUDO_CONSIGNACION_CODIGOINTERNO).Count > 0 Then
						DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_CONSIGNACION_CODIGO).TabVisible = Visibility.Visible
					Else
						DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_CONSIGNACION_CODIGO).TabVisible = Visibility.Collapsed
					End If
				End If

				VerificarPrimerTabHabilitado()
			Else
				A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los tab de ordenes de recaudo.",
						   Me.ToString(), "TerminoVerificarTabControlOrdenRecaudo", Application.Current.ToString(), Program.Maquina, lo.Error)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los tab de ordenes de pago.",
							   Me.ToString(), "TerminoVerificarTabControlOrdenRecaudo", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Sub VerificarPrimerTabHabilitado()
		Try
			If DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_CHEQUE_CODIGO).TabVisible = Visibility.Visible Then
				BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemCheque")
			ElseIf DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_TRANSFERENCIA_CODIGO).TabVisible = Visibility.Visible Then
				BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemTransferencia")
			ElseIf DiccionarioTabPantalla(GSTR_TABORDENESRECAUDO_CONSIGNACION_CODIGO).TabVisible = Visibility.Visible Then
				BuscarControlValidacion(OrdenesReciboPLUSView, "tabItemConsignacion")
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta para obtener los tabs que se deben de habilitar.", Me.ToString, "VerificarHabilitarTabsOrdenPago", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

#End Region

#Region "Temporizador"

	Private _myDispatcherTimerTesoreria As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

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
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al parar el temporizador.", Me.ToString(), "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try
	End Sub

	Public Sub ReiniciaTimer()
		Try
			If Program.Recarga_Automatica_Activa Then
				If _myDispatcherTimerTesoreria Is Nothing Then
					_myDispatcherTimerTesoreria = New System.Windows.Threading.DispatcherTimer
					_myDispatcherTimerTesoreria.Interval = New TimeSpan(0, 0, 0, 0, Program.Par_lapso_recarga)
					AddHandler _myDispatcherTimerTesoreria.Tick, AddressOf Me.Each_Tick
				End If
				_myDispatcherTimerTesoreria.Start()
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar el temporizador.", Me.ToString(), "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
			IsBusy = False
			IsBusyDetalles = False
		End Try

	End Sub

	''' <summary>
	''' recarga de ordenes cada que se cumple el tiempo del temporizador
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub Each_Tick(sender As Object, e As EventArgs)
		If logEditarRegistro = False And logNuevoRegistro = False And logBuscar = False And logFiltrar = False And logDuplicarRegistro = False Then
			RecargarPantalla()
		End If
	End Sub

#End Region

#End Region

#Region "Notificaciones"

	Private Const TOPICOMENSAJE_RESPUESTATESORERO = "OYDPLUS_TESORERO_RECIBO"

	''' <summary>
	''' Método que recibe una notificaión
	''' </summary>
	''' <param name="pobjInfoNotificacion"></param>
	''' <remarks>Juan David Osorio Diciembre 2014</remarks>
	Public Overrides Sub LlegoNotificacion(pobjInfoNotificacion As A2.Notificaciones.Cliente.clsNotificacion)
		Try

			' Dim objNotificacion As clsNotificacionesOYDPLUS

			If Not String.IsNullOrEmpty(pobjInfoNotificacion.strTipoMensaje) Then

				If pobjInfoNotificacion.strTipoMensaje.ToUpper = TOPICOMENSAJE_RESPUESTATESORERO Then

					If Not String.IsNullOrEmpty(pobjInfoNotificacion.strInfoMensaje) Then
						If Editando = False Then
							If Not IsNothing(_ListaTesoreriaOrdenesPlusRC) Then
								If _ListaTesoreriaOrdenesPlusRC.Where(Function(i) i.lngID = CInt(pobjInfoNotificacion.strInfoMensaje)).Count > 0 Then
									IsBusy = True
									TraerOrdenes("TERMINOGUARDARNUEVO", VistaSeleccionada)
								End If
							End If
						End If
					End If
				End If
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el mensaje de la notificación.",
								Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaOrdenesReciboPLUSView
	Implements INotifyPropertyChanged


	Private _ComitenteSeleccionadoBusqueda As New OYDUtilidades.BuscadorClientes
	Public Property ComitenteSeleccionadoBusqueda As OYDUtilidades.BuscadorClientes
		Get
			Return (_ComitenteSeleccionadoBusqueda)
		End Get
		Set(ByVal value As OYDUtilidades.BuscadorClientes)

			_ComitenteSeleccionadoBusqueda = value
			If Not IsNothing(_ComitenteSeleccionadoBusqueda) Then
				strIDComitente = _ComitenteSeleccionadoBusqueda.CodigoOYD
				strNroDocumento = _ComitenteSeleccionadoBusqueda.NroDocumento

			End If

			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ComitenteSeleccionadoBusqueda"))
		End Set

	End Property



	Private _lngID As Integer
	<Display(Name:="ID Orden Tesoreria", Description:="ID Orden Tesoreria")>
	Property lngID As Integer
		Get
			Return _lngID
		End Get
		Set(ByVal value As Integer)
			_lngID = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngID"))
		End Set
	End Property

	Private _strNroDocumento As String
	<Display(Name:="Nro Documento", Description:="Nro Documento")>
	Property strNroDocumento As String
		Get
			Return _strNroDocumento
		End Get
		Set(ByVal value As String)
			_strNroDocumento = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNroDocumento"))
		End Set
	End Property

	Private _strEstado As String
	<Display(Name:="Estado", Description:="Estado")>
	Property strEstado As String
		Get
			Return _strEstado
		End Get
		Set(ByVal value As String)
			_strEstado = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEstado"))
		End Set
	End Property

	Private _strIDComitente As String
	<Display(Name:="ID Comitente", Description:="ID Comitente")>
	Property strIDComitente As String
		Get
			Return _strIDComitente
		End Get
		Set(ByVal value As String)
			_strIDComitente = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDComitente"))
		End Set
	End Property

	Private _strCodigoReceptor As String
	<Display(Name:="Código Receptor", Description:="Código Receptor")>
	Property strCodigoReceptor As String
		Get
			Return _strCodigoReceptor
		End Get
		Set(ByVal value As String)
			_strCodigoReceptor = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCodigoReceptor"))
		End Set
	End Property

	Private _strTipoProducto As String
	<Display(Name:="Tipo Producto", Description:="Tipo Producto")>
	Property strTipoProducto As String
		Get
			Return _strTipoProducto
		End Get
		Set(ByVal value As String)
			_strTipoProducto = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoProducto"))
		End Set
	End Property


	Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

	Public Sub New()

	End Sub
End Class

Public Class ChequesConsignacionesOyDPlus
	Implements INotifyPropertyChanged

	Private _lngIDBanco As Integer
	Private _lngNroCheque As Double
	Private _Valor As Decimal
	Private _scaneados As String
	Private _strDescripcionBanco As String
	Private _strCuentaConsignacion As String
	Private _strTipoCuenta As String



	Public Property lngIDBanco() As Integer
		Get
			Return _lngIDBanco
		End Get
		Set(ByVal value As Integer)
			_lngIDBanco = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDBanco"))
		End Set
	End Property
	Public Property lngNroCheque() As Double
		Get
			Return _lngNroCheque
		End Get
		Set(ByVal value As Double)
			_lngNroCheque = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngNroCheque"))
		End Set
	End Property
	Public Property Valor() As Decimal
		Get
			Return _Valor
		End Get
		Set(ByVal value As Decimal)
			_Valor = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Valor"))
		End Set
	End Property
	Public Property scaneados() As String
		Get
			Return _scaneados
		End Get
		Set(ByVal value As String)
			_scaneados = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("scaneados"))
		End Set
	End Property
	Public Property strDescripcionBanco() As String
		Get
			Return _strDescripcionBanco
		End Get
		Set(ByVal value As String)
			_strDescripcionBanco = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionBanco"))
		End Set
	End Property
	Public Property strCuentaConsignacion() As String
		Get
			Return _strCuentaConsignacion
		End Get
		Set(ByVal value As String)
			_strCuentaConsignacion = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCuentaConsignacion"))
		End Set
	End Property
	Public Property strTipoCuenta() As String
		Get
			Return _strTipoCuenta
		End Get
		Set(ByVal value As String)
			_strTipoCuenta = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoCuenta"))
		End Set
	End Property



	Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

Public Class InformacionArchivos
	Public Property ID As Integer
	Public Property IDDetalle As Integer
	Public Property NombreArchivo As String
	Public Property RutaArchivo As String
	Public Property ByteArchivo As Byte()
End Class

Public Class CarterasColectivas
	Public Property ID As String
	Public Property Descripcion As String
End Class