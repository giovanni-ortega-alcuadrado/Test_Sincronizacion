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
Imports Microsoft.VisualBasic.CompilerServices
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Text
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Public Class TesoreroViewModel_OYDPLUS
    Inherits A2ControlMenu.A2ViewModel

    Private Const STR_RUTAREPORTECARTA As String = "[SERVIDOR]/Pages/ReportViewer.aspx?[REPORTE]&pstrTexto=[TEXTO]'&rs:Command=Render&rs:Format=PDF&rc:Parameters=false&rs:ParameterLanguage=[CULTURA]"
    Private Enum PARAMETROSRUTA
        SERVIDOR
        REPORTE
        TEXTO
        CULTURA
    End Enum

#Region "Declaraciones"
    Dim logAcumularArchivo = False
    Dim logGeneroDocumentos = False
    Public objOrdenTesoreria As OrdenGiroConsultaView
    Public objOrdenRecibo As OrdenReciboConsultaView
    Public objTesorero As TesoreroView
    Public logListaFiltrada_OrdenGiro As Boolean = False
    Public logListaFiltrada_OrdenRecibo As Boolean = False
    Public strFormaPagoCancelacion As String = String.Empty
    Dim num As Integer = 0
    Dim logNoDesbloquear As Boolean = False
    'Public Property cb As New CamposBusquedaTesoreri
    Public logHayEncabezado As Boolean = False

    Public objWppDetallesRecibo As wppfrmDetallesRecibo
    Public logEsTercero As Boolean
    Public logHayCheque = False
    Private TesoreriaOrdenesPlusPorDefecto As TesoreriaOrdenesEncabezado
    Private TesoreriaOrdenesPlusAnterior As New TesoreriaOrdenesEncabezado
    Private TesoreriaOrdenesPlusDetalle_ChequesPorDefecto As TesoreriaOyDPlusCheques
    Private TesoreriaOrdenesPlusDetalle_ChequesAnterior As TesoreriaOyDPlusCheques
    Private TesoreriaOrdenesPlusDetalle_TransferenciasAnterior As TesoreriaOyDPlusTransferencia
    Private TesoreriaOrdenesPlusDetalle_CarterasColectivasAnterior As TesoreriaOyDPlusCarterasColectivas
    Private TesoreriaOrdenesPlusDetalle_InternosAnterior As TesoreriaOyDPlusInternos

    Public objListaRecibo = New List(Of OYDPLUSUtilidades.CombosReceptor)
    Public objListaGiro = New List(Of OYDPLUSUtilidades.CombosReceptor)
    Public objListaFondos = New List(Of OYDPLUSUtilidades.CombosReceptor)

    Dim strMensajeValidacion As String = String.Empty
    Dim mdtmFechaCierreSistema As DateTime = Now.Date
    Dim cantidadTotalConfirmacion As Integer = 0
    Dim cantidadTotalJustificacion As Integer = 0
    Public strReceptor As String

    Dim dcProxy As OYDPLUSTesoreriaDomainContext
    Dim dcProxy1 As OYDPLUSTesoreriaDomainContext
    Dim dcProxy2 As OYDPLUSTesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim objProxyPLUS As OYDPLUSUtilidadesDomainContext
    Dim A2Util As A2UtilsViewModel
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim mensaje As String
    Dim MakeAndCheck As Integer = 0
    Dim Make As Boolean
    Dim esVersion As Boolean = False
    Dim codigo As Integer
    Dim fechaCierre As DateTime
    Dim moduloTesoreria As String
    Dim sw As Integer
    Dim Filtro As Integer = 0
    Dim estadoMC As String
    Public TIPODOCUMENTOSIMPORTACION As String
    Public TIPOCUENTAOYDPLUS As String
    Dim logNuevoRegistroCheque As Boolean
    Dim logEditarRegistroCheque As Boolean

    Public logEsTerceroTransferencia As Boolean
    Public logEsCuentaRegistrada As Boolean
    Public logEsTerceroCartera As Boolean
    Public logEsTerceroInternos As Boolean

    Dim logCancelarRegistro As Boolean
    Dim logNuevoRegistro As Boolean
    Dim logEditarRegistro As Boolean
    Dim logRealizarConsultaPropiedades As Boolean = False
    Dim dtmFechaServidor As DateTime
    Dim STRPARAMETRO_FINONSET As String = String.Empty
    Dim objVMOrdenGiro As New TesoreriaViewModel_OYDPLUS
    Public logEsFondosOYD As Boolean = False
    Dim logValidarCamposObligatorios As Boolean = False
    Dim SenderAux As Object
    Public logEsFondosUnity As Boolean = False
    Private logRealizarConsultaSeleccionarTodos As Boolean = True

    Dim plngIdEncabezadoAutogestion As Integer
    Dim plogEditarOrdenAutogestion As Boolean
    Dim plogPendientePorAprobarAutogestion As Boolean
    Dim plogFuncionalidadAutogestionDocumentos As Boolean

#End Region

#Region "Constantes"
    Private Const STR_TOPICO_NOTIFICACION_TESORERO As String = "TESORERO"
    Private Const STR_TOPICO_NOTIFICACION_ORDENESTESORERIA As String = "TESORERIA_OYD_PLUS"
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
    ''' <summary>
    ''' Constante para multiplicar valores con decimales para enviar como string
    ''' se multiplica por valor grande y se redondea a cero, eliminando decimales;
    ''' pero se debe volver a dividir en base de datos
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INT_VALOR_MULTIPLICAR_ELIMINAR_DECIMAL As Integer = 100000

    Private str_NombreHojaExcel As String = "Operaciones"
    Private Const STR_TIPOEXPORTACION As String = "EXCELVIEJO"
    Private Const STR_NOMBREPROCESO As String = "GENERARINFOFONDOS"
    Private Const STR_PARAMETROSGENERACION As String = "[TIPO]=[[TIPO]]|[IDGENERAR]=[[IDGENERAR]]|[FORMAPAGO]=[[FORMAPAGO]]|[CONSECUTIVO]=[[CONSECUTIVO]]|[BANCO]=[[BANCO]]|[BANCOFONDO]=[[BANCOFONDO]]|[USUARIO]=[[USUARIO]]"

    Private Enum ParametrosGeneracion
        TIPO
        IDGENERAR
        FORMAPAGO
        CONSECUTIVO
        BANCO
        BANCOFONDO
        CUENTAFONDO
        USUARIO
    End Enum

    Private Const COLOR_DESHABILITADO As String = "#E2E2E2"
    Private Const COLOR_HABILITADO As String = "#FFFFFFFF"

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            'ListaTopicos = New List(Of String)
            'ListaTopicos.Add(STR_TOPICO_NOTIFICACION_TESORERO)
            'ListaTopicos.Add(STR_TOPICO_NOTIFICACION_ORDENESTESORERIA)

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSTesoreriaDomainContext()
                dcProxy1 = New OYDPLUSTesoreriaDomainContext()
                dcProxy2 = New OYDPLUSTesoreriaDomainContext()
                objProxy = New UtilidadesDomainContext()
                objProxyPLUS = New OYDPLUSUtilidadesDomainContext()

            Else
                dcProxy = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy1 = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy2 = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                objProxyPLUS = New OYDPLUSUtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYDPLUS)))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OYDPLUSTesoreriaDomainContext.IOYDPLUSTesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If Not Program.IsDesignMode() Then
                IsBusy = True
                Usuario = Program.Usuario
                LimpiarCampos()
                ConsultarSaldoBanco = False
                ItemReceptor = GSTR_ID_TODOS
                NombreReceptor = GSTR_TODOS
                dcProxy2.Load(dcProxy2.SucursalesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSucursales, "")
                CambiarColorFondoTextoBuscador()
                If Not IsNothing(_Fecha) Then
                    FechaConvertida = _Fecha.ToString("yyyy-MM-dd")
                End If
            End If
        Catch ex As Exception

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "TesoreriaViewModelOyDPlus.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "OyD PLUS"

#Region "PROPIEDADES OYD PLUS"
    Private _VerInfoFondos As Visibility = Visibility.Collapsed
    Public Property VerInfoFondos() As Visibility
        Get
            Return _VerInfoFondos
        End Get
        Set(ByVal value As Visibility)
            _VerInfoFondos = value
            MyBase.CambioItem("VerInfoFondos")
        End Set
    End Property

    Private _HabilitarFormaPago As Boolean
    Public Property HabilitarFormaPago() As Boolean
        Get
            Return _HabilitarFormaPago
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFormaPago = value
            MyBase.CambioItem("HabilitarFormaPago")
        End Set
    End Property

    Private _VerBanco As Visibility = Visibility.Visible
    Public Property VerBanco() As Visibility
        Get
            Return _VerBanco
        End Get
        Set(ByVal value As Visibility)
            _VerBanco = value
            MyBase.CambioItem("VerBanco")
        End Set
    End Property

    Private _VerBancoFondos As Visibility = Visibility.Collapsed
    Public Property VerBancoFondos() As Visibility
        Get
            Return _VerBancoFondos
        End Get
        Set(ByVal value As Visibility)
            _VerBancoFondos = value
            MyBase.CambioItem("VerBancoFondos")
        End Set
    End Property
    Private _VerBancoFondosDestino As Visibility = Visibility.Collapsed
    Public Property VerBancoFondosDestino() As Visibility
        Get
            Return _VerBancoFondosDestino
        End Get
        Set(ByVal value As Visibility)
            _VerBancoFondosDestino = value
            MyBase.CambioItem("VerBancoFondosDestino")
        End Set
    End Property
    Private _VerBancoFondosOYD As Visibility = Visibility.Collapsed
    Public Property VerBancoFondosOYD() As Visibility
        Get
            Return _VerBancoFondosOYD
        End Get
        Set(ByVal value As Visibility)
            _VerBancoFondosOYD = value
            MyBase.CambioItem("VerBancoFondosOYD")
        End Set
    End Property

    Private _VerControlesGiro As Visibility = Visibility.Visible
    Public Property VerControlesGiro() As Visibility
        Get
            Return _VerControlesGiro
        End Get
        Set(ByVal value As Visibility)
            _VerControlesGiro = value
            MyBase.CambioItem("VerControlesGiro")
        End Set
    End Property
    Private _VerBotonAgregarArchivo As Visibility = Visibility.Collapsed
    Public Property VerBotonAgregarArchivo() As Visibility
        Get
            Return _VerBotonAgregarArchivo
        End Get
        Set(ByVal value As Visibility)
            _VerBotonAgregarArchivo = value
            MyBase.CambioItem("VerBotonAgregarArchivo")
        End Set
    End Property
    Private _strTipoNegocio As String
    Public Property strTipoNegocio() As String
        Get
            Return _strTipoNegocio
        End Get
        Set(ByVal value As String)
            _strTipoNegocio = value
            LimpiarCampos()
            If _strTipoNegocio.ToUpper = GSTR_ORDENRECIBO.ToUpper Then
                HabilitarFormaPago = False
                VerControlesGiro = Visibility.Collapsed
                VerBanco = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerBotonAgregarArchivo = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed
                VerTabRecibos = Visibility.Visible
                HabilitarFiltrosOrdenesRecibo = True
                HabilitarFiltrosOrdenesGiro = False
                HabilitarFiltrosOrdenesFondos = False
                HabilitarFiltrosCarteraColectiva = False
                If DiccionarioCombosOYDPlus.ContainsKey("CONSECUTIVORC_Plus") Then
                    If DiccionarioCombosOYDPlus("CONSECUTIVORC_Plus").Count <= 0 Then
                        mostrarMensaje("No hay consecutivos de recibo configurados para el usuario " + Program.Usuario + ".", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    mostrarMensaje("No hay consecutivos de recibo configurados para el usuario " + Program.Usuario + ".", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                DiccionarioCombosOYDPlus("TIPOPAGOPLUS") = Nothing
                DiccionarioCombosOYDPlus("TIPOPAGOPLUS") = objListaRecibo
                MyBase.CambioItem("DiccionarioCombosOYDPlus")
                strTipoPagoPlus = GSTR_TODOS_RETORNO
                VerInfoFondos = Visibility.Collapsed
            ElseIf _strTipoNegocio.ToUpper = GSTR_ORDENGIRO.ToUpper Then

                DiccionarioCombosOYDPlus("TIPOPAGOPLUS") = Nothing
                DiccionarioCombosOYDPlus("TIPOPAGOPLUS") = objListaGiro
                MyBase.CambioItem("DiccionarioCombosOYDPlus")
                strTipoPagoPlus = String.Empty
                HabilitarFormaPago = True
                VerControlesGiro = Visibility.Collapsed
                VerBanco = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerBotonAgregarArchivo = Visibility.Collapsed
                VerControlesGiro = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabCheque = Visibility.Visible
                VerTabChequeFondo = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed
                strTextoGenerarDocumento = "Generar Documentos"
                VerInfoFondos = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                HabilitarFiltrosOrdenesRecibo = False
                HabilitarFiltrosOrdenesGiro = True
                HabilitarFiltrosOrdenesFondos = False
                HabilitarFiltrosCarteraColectiva = False
            ElseIf _strTipoNegocio.ToUpper = GSTR_ORDENFONDOS.ToUpper Then
                VerInfoFondos = Visibility.Visible
                DiccionarioCombosOYDPlus("TIPOPAGOPLUS") = Nothing
                DiccionarioCombosOYDPlus("TIPOPAGOPLUS") = objListaFondos
                MyBase.CambioItem("DiccionarioCombosOYDPlus")
                strTipoPagoPlus = String.Empty
                HabilitarFormaPago = True
                VerControlesGiro = Visibility.Visible
                VerBancoFondos = Visibility.Visible
                If logEsFondosUnity Then
                    VerBotonAgregarArchivo = Visibility.Collapsed
                Else
                    VerBotonAgregarArchivo = Visibility.Visible
                End If
                VerControlesGiro = Visibility.Visible
                VerTabBloqueos = Visibility.Visible
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Visible
                VerTabInternos = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                HabilitarFiltrosOrdenesRecibo = False
                HabilitarFiltrosOrdenesGiro = False
                HabilitarFiltrosOrdenesFondos = True
                HabilitarFiltrosCarteraColectiva = False
                If logEsFondosUnity Then
                    strTextoGenerarDocumento = "Enviar documentos a fondos"
                Else
                    strTextoGenerarDocumento = "Generar documento y archivo plano"
                End If
                If _strTipoPagoPlus = GSTR_OYD And logEsFondosOYD = False Then
                    If DiccionarioCombosOYDPlus.ContainsKey("CTA_FIRMA_OYD_FONDOS") Then
                        IdBanco = DiccionarioCombosOYDPlus("CTA_FIRMA_OYD_FONDOS").FirstOrDefault.Retorno
                        DescripcionBanco = DiccionarioCombosOYDPlus("CTA_FIRMA_OYD_FONDOS").FirstOrDefault.Descripcion
                    End If
                End If
            Else
                VerInfoFondos = Visibility.Collapsed
                DiccionarioCombosOYDPlus("TIPOPAGOPLUS") = Nothing
                MyBase.CambioItem("DiccionarioCombosOYDPlus")
                strTipoPagoPlus = String.Empty
                HabilitarFormaPago = False
                VerControlesGiro = Visibility.Collapsed
                VerBanco = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerControlesGiro = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                HabilitarFiltrosOrdenesRecibo = False
                HabilitarFiltrosOrdenesGiro = False
                HabilitarFiltrosOrdenesFondos = False
                HabilitarFiltrosCarteraColectiva = False
            End If

            MyBase.CambioItem("strTipoNegocio")
        End Set
    End Property

    Private _ConsultarSaldoBanco As Boolean
    Public Property ConsultarSaldoBanco() As Boolean
        Get
            Return _ConsultarSaldoBanco
        End Get
        Set(ByVal value As Boolean)
            _ConsultarSaldoBanco = value
            MyBase.CambioItem("ConsultarSaldoBanco")
        End Set
    End Property

    Private _ConsultarSaldoBanco_FondosOYD As Boolean
    Public Property ConsultarSaldoBanco_FondosOYD() As Boolean
        Get
            Return _ConsultarSaldoBanco_FondosOYD
        End Get
        Set(ByVal value As Boolean)
            _ConsultarSaldoBanco_FondosOYD = value
            MyBase.CambioItem("ConsultarSaldoBanco_FondosOYD")
        End Set
    End Property

    Private _TotalGenerar As Double
    Public Property TotalGenerar() As Double
        Get
            Return _TotalGenerar
        End Get
        Set(ByVal value As Double)
            _TotalGenerar = value
            MyBase.CambioItem("TotalGenerar")
        End Set
    End Property

    Private _SeleccionarTodos As Boolean
    Public Property SeleccionarTodos() As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value
            If logRealizarConsultaSeleccionarTodos Then
                Try
                    Dim IdsConcatenados As String = String.Empty
                    Dim logpermitir As Boolean = True
                    If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                        If Not IsNothing(_SeleccionarTodos) Then
                            If Not IsNothing(ListaResultadosDocumentosRecibo) Then
                                If _SeleccionarTodos Then
                                    For Each li In ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = False)
                                        If logpermitir Then
                                            IdsConcatenados = String.Format("%{0}%", li.lngIDEncabezado)
                                            logpermitir = False
                                        Else
                                            IdsConcatenados = String.Format("{0}|%{1}%", IdsConcatenados, li.lngIDEncabezado)
                                        End If
                                    Next

                                    dcProxy.TempValidarEstadoOrdens.Clear()
                                    IsBusy = True
                                    If strEstado = GSTR_PENDIENTE_Plus Then
                                        dcProxy.Load(dcProxy.ValidarEstadoOrdenTesoreriaEncabezadoQuery(IdsConcatenados, String.Empty, Program.Usuario, "TE", Program.HashConexion), AddressOf TerminoBloqueoTesoreroTodos, "")
                                    End If
                                Else
                                    For Each li In ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True)
                                        If _SeleccionarTodos = False Then
                                            IsBusy = True
                                            If strEstado = GSTR_PENDIENTE_Plus Then
                                                dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(li.lngIDEncabezado, GSTR_OT, Program.Usuario, Program.HashConexion, AddressOf TerminoDesbloqueoTesoreroTodos, li.lngIDEncabezado.ToString)
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Else
                        If Not IsNothing(_SeleccionarTodos) Then
                            If Not IsNothing(_ListaResultadosDocumentos) Then
                                If ListaResultadosDocumentos.Count > 0 Then
                                    If _SeleccionarTodos Then
                                        If logListaFiltrada_OrdenGiro = True Then
                                            For Each li In objTesorero.ListaFiltrada.Where(Function(i) i.Generar = False)
                                                If logpermitir Then
                                                    IdsConcatenados = String.Format("%{0}%", li.lngID)
                                                    logpermitir = False
                                                Else
                                                    IdsConcatenados = String.Format("{0}|%{1}%", IdsConcatenados, li.lngID)
                                                End If
                                            Next
                                        Else
                                            For Each li In ListaResultadosDocumentos.Where(Function(i) i.Generar = False)
                                                If logpermitir Then
                                                    IdsConcatenados = String.Format("%{0}%", li.lngID)
                                                    logpermitir = False
                                                Else
                                                    IdsConcatenados = String.Format("{0}|%{1}%", IdsConcatenados, li.lngID)
                                                End If
                                            Next
                                        End If

                                        If strEstado = GSTR_PENDIENTE_Plus Then
                                            dcProxy.TempValidarEstadoOrdens.Clear()
                                            IsBusy = True
                                            dcProxy.Load(dcProxy.ValidarEstadoOrdenTesoreriaEncabezadoQuery(IdsConcatenados, String.Empty, Program.Usuario, "T", Program.HashConexion),
                                                         AddressOf TerminoBloqueoTesoreroTodos, "")
                                        Else
                                            For Each li In ListaResultadosDocumentos.Where(Function(i) i.Generar = False)
                                                li.Generar = True
                                            Next
                                        End If
                                    Else
                                        For Each li In ListaResultadosDocumentos.Where(Function(i) i.Generar = True)
                                            If strEstado = GSTR_PENDIENTE_Plus Then
                                                IsBusy = True
                                                dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(li.lngID, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf TerminoDesbloqueoTesoreroTodos, li.lngID.ToString)
                                            Else
                                                li.Generar = False
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        End If
                    End If

                    CalcularValorTotal()
                Catch ex As Exception
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todos los documentos.",
                                                                 Me.ToString(), "SeleccionarTodos", Application.Current.ToString(), Program.Maquina, ex)
                End Try
            End If
            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property

    Private _GenerarDctoTransferencia As Boolean
    Public Property GenerarDctoTransferencia() As Boolean
        Get
            Return _GenerarDctoTransferencia
        End Get
        Set(ByVal value As Boolean)
            _GenerarDctoTransferencia = value
            MyBase.CambioItem("GenerarDctoTransferencia")
        End Set
    End Property

    Private _GenerarDctoCarterasColectivas As Boolean
    Public Property GenerarDctoCarterasColectivas() As Boolean
        Get
            Return _GenerarDctoCarterasColectivas
        End Get
        Set(ByVal value As Boolean)
            _GenerarDctoCarterasColectivas = value
            MyBase.CambioItem("GenerarDctoCarterasColectivas")
        End Set
    End Property

    Private _GenerarDctoInternos As Boolean
    Public Property GenerarDctoInternos() As Boolean
        Get
            Return _GenerarDctoInternos
        End Get
        Set(ByVal value As Boolean)
            _GenerarDctoInternos = value
            MyBase.CambioItem("GenerarDctoInternos")
        End Set
    End Property

    Private _strConsecutivoNOTAS_PLUS_Origen As String
    Public Property strConsecutivoNOTAS_PLUS_Origen() As String
        Get
            Return _strConsecutivoNOTAS_PLUS_Origen
        End Get
        Set(ByVal value As String)
            _strConsecutivoNOTAS_PLUS_Origen = value
            MyBase.CambioItem("strConsecutivoNOTAS_PLUS_Origen")
        End Set
    End Property

    Private _strConsecutivoNOTAS_PLUS_Destino As String
    Public Property strConsecutivoNOTAS_PLUS_Destino() As String
        Get
            Return _strConsecutivoNOTAS_PLUS_Destino
        End Get
        Set(ByVal value As String)
            _strConsecutivoNOTAS_PLUS_Destino = value
            MyBase.CambioItem("strConsecutivoNOTAS_PLUS_Destino")
        End Set
    End Property

    Private _strConsecutivoCE_PLUS_Origen As String
    Public Property strConsecutivoCE_PLUS_Origen() As String
        Get
            Return _strConsecutivoCE_PLUS_Origen
        End Get
        Set(ByVal value As String)
            _strConsecutivoCE_PLUS_Origen = value
            MyBase.CambioItem("strConsecutivoCE_PLUS_Origen")
        End Set
    End Property

    Private _strConsecutivoCE_PLUS_Destino As String
    Public Property strConsecutivoCE_PLUS_Destino() As String
        Get
            Return _strConsecutivoCE_PLUS_Destino
        End Get
        Set(ByVal value As String)
            _strConsecutivoCE_PLUS_Destino = value
            MyBase.CambioItem("strConsecutivoCE_PLUS_Destino")
        End Set
    End Property

    Private _IdConcepto As Integer
    Public Property IdConcepto() As Integer
        Get
            Return _IdConcepto
        End Get
        Set(ByVal value As Integer)
            _IdConcepto = value
            MyBase.CambioItem("IdConcepto")
        End Set
    End Property

    Private _strEstado As String
    Public Property strEstado() As String
        Get
            Return _strEstado
        End Get
        Set(ByVal value As String)
            _strEstado = value
            LimpiarDetalle()
            MyBase.CambioItem("strEstado")
        End Set
    End Property
    Private _Fecha As DateTime = Now
    Public Property Fecha() As DateTime
        Get
            Return _Fecha
        End Get
        Set(ByVal value As DateTime)
            _Fecha = value
            If Not IsNothing(_Fecha) Then
                FechaConvertida = _Fecha.ToString("yyyy-MM-dd")
            End If
            CarteraColectiva = String.Empty
            DescripcionCarteraColectiva = String.Empty
            MyBase.CambioItem("Fecha")
        End Set
    End Property

    Private _FechaConvertida As String
    Public Property FechaConvertida() As String
        Get
            Return _FechaConvertida
        End Get
        Set(ByVal value As String)
            _FechaConvertida = value
            MyBase.CambioItem("FechaConvertida")
        End Set
    End Property

    Private _DescripcionBanco As String
    Public Property DescripcionBanco() As String
        Get
            Return _DescripcionBanco
        End Get
        Set(ByVal value As String)
            _DescripcionBanco = value
            MyBase.CambioItem("DescripcionBanco")
        End Set
    End Property

    Private _DescripcionBanco_FondosOYD As String
    Public Property DescripcionBanco_FondosOYD() As String
        Get
            Return _DescripcionBanco_FondosOYD
        End Get
        Set(ByVal value As String)
            _DescripcionBanco_FondosOYD = value
            MyBase.CambioItem("DescripcionBanco_FondosOYD")
        End Set
    End Property

    Private _IdBanco As Nullable(Of Integer) = Nothing
    Public Property IdBanco() As Nullable(Of Integer)
        Get
            Return _IdBanco
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdBanco = value
            MyBase.CambioItem("IdBanco")
        End Set
    End Property


    Private _IdBancoFondo As Nullable(Of Integer) = Nothing
    Public Property IdBancoFondo() As Nullable(Of Integer)
        Get
            Return _IdBancoFondo
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdBancoFondo = value
            MyBase.CambioItem("IdBancoFondo")
        End Set
    End Property

    Private _IdBancoFondoDestino As Nullable(Of Integer) = Nothing
    Public Property IdBancoFondoDestino() As Nullable(Of Integer)
        Get
            Return _IdBancoFondoDestino
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdBancoFondoDestino = value
            MyBase.CambioItem("IdBancoFondoDestino")
        End Set
    End Property

    Private _IdBanco_FondosOYD As Nullable(Of Integer) = Nothing
    Public Property IdBanco_FondosOYD() As Nullable(Of Integer)
        Get
            Return _IdBanco_FondosOYD
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdBanco_FondosOYD = value
            MyBase.CambioItem("IdBanco_FondosOYD")
        End Set
    End Property

    Private _DescripcionBancoFondo As String
    Public Property DescripcionBancoFondo() As String
        Get
            Return _DescripcionBancoFondo
        End Get
        Set(ByVal value As String)
            _DescripcionBancoFondo = value
            MyBase.CambioItem("DescripcionBancoFondo")
        End Set
    End Property
    Private _DescripcionBancoFondoDestino As String
    Public Property DescripcionBancoFondoDestino() As String
        Get
            Return _DescripcionBancoFondoDestino
        End Get
        Set(ByVal value As String)
            _DescripcionBancoFondoDestino = value
            MyBase.CambioItem("DescripcionBancoFondoDestino")
        End Set
    End Property
    Private _HabilitarGenerarDctos As Boolean
    Public Property HabilitarGenerarDctos() As Boolean
        Get
            Return _HabilitarGenerarDctos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGenerarDctos = value
            If _HabilitarGenerarDctos Then
                HabilitarGenerarDctosReadOnly = False
            Else
                HabilitarGenerarDctosReadOnly = True
            End If
            MyBase.CambioItem("HabilitarGenerarDctos")
        End Set
    End Property
    Private _HabilitarGenerarDctosReadOnly As Boolean
    Public Property HabilitarGenerarDctosReadOnly() As Boolean
        Get
            Return _HabilitarGenerarDctosReadOnly
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGenerarDctosReadOnly = value
            MyBase.CambioItem("HabilitarGenerarDctosReadOnly")
        End Set
    End Property

    Private _HabilitarGeneracionDctos As Boolean = False
    Public Property HabilitarGeneracionDctos() As Boolean
        Get
            Return _HabilitarGeneracionDctos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGeneracionDctos = value
            MyBase.CambioItem("HabilitarGeneracionDctos")
        End Set
    End Property
    Private _strTextoGenerarDocumento As String = "Generar Documentos"
    Public Property strTextoGenerarDocumento() As String
        Get
            Return _strTextoGenerarDocumento
        End Get
        Set(ByVal value As String)
            _strTextoGenerarDocumento = value
            MyBase.CambioItem("strTextoGenerarDocumento")
        End Set
    End Property


    Private _strTipoPagoPlus As String
    Public Property strTipoPagoPlus() As String
        Get
            Return _strTipoPagoPlus
        End Get
        Set(ByVal value As String)
            _strTipoPagoPlus = value
            LimpiarDetalle()
            OcultarCamposConsecutivosBancos()

            If _strTipoNegocio.ToUpper = GSTR_ORDENFONDOS.ToUpper Then
                If _strTipoPagoPlus = GSTR_OPERACIONES_ESPECIALES Then
                    strTextoGenerarDocumento = "Generar Documentos"

                Else
                    If _strTipoPagoPlus = GSTR_OYD Then
                        If DiccionarioCombosOYDPlus.ContainsKey("CTA_FIRMA_OYD_FONDOS") Then
                            IdBanco = DiccionarioCombosOYDPlus("CTA_FIRMA_OYD_FONDOS").FirstOrDefault.Retorno
                            DescripcionBanco = DiccionarioCombosOYDPlus("CTA_FIRMA_OYD_FONDOS").FirstOrDefault.Descripcion
                        End If
                    End If
                    strTextoGenerarDocumento = "Generar documento y archivo plano"
                    If logEsFondosUnity Then
                        strTextoGenerarDocumento = "Enviar documentos a fondos"
                    Else
                        strTextoGenerarDocumento = "Generar documento y archivo plano"
                    End If
                End If
            Else
                If _strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS Then
                    If logEsFondosUnity Then
                        strTextoGenerarDocumento = "Enviar documentos a fondos"
                    Else
                        strTextoGenerarDocumento = "Generar documento y archivo plano"
                    End If
                    VerInfoFondos = Visibility.Visible
                    HabilitarFiltrosCarteraColectiva = True
                Else
                    strTextoGenerarDocumento = "Generar Documentos"
                    VerInfoFondos = Visibility.Collapsed
                    HabilitarFiltrosCarteraColectiva = False
                End If
            End If
            If _strTipoPagoPlus = GSTR_TRASLADOFONDOS And strEstado = GSTR_PENDIENTE_Plus Then
                VerBancoFondosDestino = Visibility.Visible
                VerBanco = Visibility.Collapsed
            Else
                VerBancoFondosDestino = Visibility.Collapsed
            End If


            MyBase.CambioItem("strTipoPagoPlus")
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

    Private _ItemReceptor As String
    Public Property ItemReceptor() As String
        Get
            Return _ItemReceptor
        End Get
        Set(ByVal value As String)
            _ItemReceptor = value
            LimpiarDetalle()
            MyBase.CambioItem("ItemReceptor")
        End Set
    End Property

    Private _NombreReceptor As String
    Public Property NombreReceptor() As String
        Get
            Return _NombreReceptor
        End Get
        Set(ByVal value As String)
            _NombreReceptor = value
            MyBase.CambioItem("NombreReceptor")
        End Set
    End Property

    Private _ItemSucursales As Sucursale
    Public Property ItemSucursales() As Sucursale
        Get
            Return _ItemSucursales
        End Get
        Set(ByVal value As Sucursale)
            _ItemSucursales = value
            MyBase.CambioItem("ItemSucursales")
        End Set
    End Property

    Private _ListaSucursale As List(Of Sucursale)
    Public Property ListaSucursales() As List(Of Sucursale)
        Get
            Return _ListaSucursale
        End Get
        Set(ByVal value As List(Of Sucursale))
            _ListaSucursale = value
            MyBase.CambioItem("ListaSucursales")
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

    Private _ListaConsecutivosNotasOrigen As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivosNotasOrigen() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivosNotasOrigen
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivosNotasOrigen = value
            MyBase.CambioItem("ListaConsecutivosNotasOrigen")
        End Set
    End Property

    Private _ListaConsecutivosEgresosOrigen As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivosEgresosOrigen() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivosEgresosOrigen
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivosEgresosOrigen = value
            MyBase.CambioItem("ListaConsecutivosEgresosOrigen")
        End Set
    End Property

    Private _ListaConsecutivosRecibosOrigen As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivosRecibosOrigen() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivosRecibosOrigen
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivosRecibosOrigen = value
            MyBase.CambioItem("ListaConsecutivosRecibosOrigen")
        End Set
    End Property

    Private _ListaConsecutivosNotasDestino As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivosNotasDestino() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivosNotasDestino
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivosNotasDestino = value
            MyBase.CambioItem("ListaConsecutivosNotasDestino")
        End Set
    End Property

    Private _ListaConsecutivosEgresosDestino As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivosEgresosDestino() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivosEgresosDestino
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivosEgresosDestino = value
            MyBase.CambioItem("ListaConsecutivosEgresosDestino")
        End Set
    End Property

    Private _ListaConsecutivosRecibosDestino As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivosRecibosDestino() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivosRecibosDestino
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivosRecibosDestino = value
            MyBase.CambioItem("ListaConsecutivosRecibosDestino")
        End Set
    End Property

    Private _FechaOrden As DateTime = DateTime.Now
    Public Property FechaOrden() As DateTime
        Get
            Return _FechaOrden
        End Get
        Set(ByVal value As DateTime)
            _FechaOrden = value
            MyBase.CambioItem("FechaOrden")
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

    Private _strEsTercero As String
    Public Property strEsTercero As String
        Get
            Return _strEsTercero
        End Get
        Set(ByVal value As String)
            _strEsTercero = value
            MyBase.CambioItem("strEsTercero")
        End Set
    End Property

    Private _strEsCuentaRegistrada As String
    Public Property strEsCuentaRegistrada As String
        Get
            Return _strEsCuentaRegistrada
        End Get
        Set(ByVal value As String)
            _strEsCuentaRegistrada = value
            MyBase.CambioItem("strEsCuentaRegistrada")
        End Set
    End Property

    Private _IDGMF_Transferencia As String
    Public Property IDGMF_Transferencia() As String
        Get
            Return _IDGMF_Transferencia
        End Get
        Set(ByVal value As String)
            _IDGMF_Transferencia = value
            MyBase.CambioItem("IDGMF_Transferencia")
        End Set
    End Property

    Private _IDGMF As String
    Public Property IDGMF() As String
        Get
            Return _IDGMF
        End Get
        Set(ByVal value As String)
            _IDGMF = value
            MyBase.CambioItem("IDGMF")
        End Set
    End Property
    '----------------------------------------------------------------------------------------
    Private WithEvents _SelectedDocumentosReciboDetallesCargarPagosA As TesoreroDetalleRecibo
    Public Property SelectedDocumentosReciboDetallesCargarPagosA As TesoreroDetalleRecibo
        Get
            Return _SelectedDocumentosReciboDetallesCargarPagosA
        End Get
        Set(ByVal value As TesoreroDetalleRecibo)
            _SelectedDocumentosReciboDetallesCargarPagosA = value
            MyBase.CambioItem("SelectedDocumentosReciboDetallesCargarPagosA")
        End Set
    End Property

    Private _ListaResultadosDocumentosReciboDetallesCargarPagosA As List(Of TesoreroDetalleRecibo)
    Public Property ListaResultadosDocumentosReciboDetallesCargarPagosA() As List(Of TesoreroDetalleRecibo)
        Get
            Return _ListaResultadosDocumentosReciboDetallesCargarPagosA
        End Get
        Set(ByVal value As List(Of TesoreroDetalleRecibo))
            _ListaResultadosDocumentosReciboDetallesCargarPagosA = value
            If Not IsNothing(_ListaResultadosDocumentosReciboDetallesCargarPagosA) Then
                SelectedDocumentosReciboDetallesCargarPagosA = _ListaResultadosDocumentosReciboDetallesCargarPagosA.FirstOrDefault
            End If

            MyBase.CambioItem("ListaResultadosDocumentosReciboDetallesCargarPagosA")
            MyBase.CambioItem("ListaResultadosDocumentosReciboDetallesCargarPagosA_Paged")
        End Set
    End Property

    Public ReadOnly Property ListaResultadosDocumentosReciboDetallesCargarPagosA_Paged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultadosDocumentosReciboDetallesCargarPagosA) Then
                Dim view = New PagedCollectionView(_ListaResultadosDocumentosReciboDetallesCargarPagosA)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    '---------------------------------------------------------------------------------------
    Private WithEvents _SelectedDocumentosReciboDetalles As TesoreroDetalleRecibo
    Public Property SelectedDocumentosReciboDetalles As TesoreroDetalleRecibo
        Get
            Return _SelectedDocumentosReciboDetalles
        End Get
        Set(ByVal value As TesoreroDetalleRecibo)
            _SelectedDocumentosReciboDetalles = value
            MyBase.CambioItem("SelectedDocumentosReciboDetalles")
        End Set
    End Property

    Private _ListaResultadosDocumentosReciboDetalles As List(Of TesoreroDetalleRecibo)
    Public Property ListaResultadosDocumentosReciboDetalles() As List(Of TesoreroDetalleRecibo)
        Get
            Return _ListaResultadosDocumentosReciboDetalles
        End Get
        Set(ByVal value As List(Of TesoreroDetalleRecibo))
            _ListaResultadosDocumentosReciboDetalles = value
            If Not IsNothing(_ListaResultadosDocumentosReciboDetalles) Then
                SelectedDocumentosReciboDetalles = _ListaResultadosDocumentosReciboDetalles.FirstOrDefault
            End If

            MyBase.CambioItem("ListaResultadosDocumentosReciboDetalles")
            MyBase.CambioItem("ListaResultadosDocumentosReciboDetalles_Paged")
        End Set
    End Property

    Public ReadOnly Property ListaResultadosDocumentosReciboDetalles_Paged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultadosDocumentosReciboDetalles) Then
                Dim view = New PagedCollectionView(_ListaResultadosDocumentosReciboDetalles)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    '-----------------------------------------------------------------------------------------------------------------------------
    Private _VerOrden As Boolean = True
    Public Property VerOrden() As Boolean
        Get
            Return _VerOrden
        End Get
        Set(ByVal value As Boolean)
            _VerOrden = value
            MyBase.CambioItem("VerOrden")
        End Set
    End Property


    Private WithEvents _SelectedDocumentosRecibo As tblTesorero_Registros_RC
    Public Property SelectedDocumentosRecibo As tblTesorero_Registros_RC
        Get
            Return _SelectedDocumentosRecibo
        End Get
        Set(ByVal value As tblTesorero_Registros_RC)
            _SelectedDocumentosRecibo = value
            MyBase.CambioItem("SelectedDocumentosRecibo")
        End Set
    End Property

    Private _ListaResultadosDocumentosRecibo As List(Of tblTesorero_Registros_RC)
    Public Property ListaResultadosDocumentosRecibo() As List(Of tblTesorero_Registros_RC)
        Get
            Return _ListaResultadosDocumentosRecibo
        End Get
        Set(ByVal value As List(Of tblTesorero_Registros_RC))
            _ListaResultadosDocumentosRecibo = value
            If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                SelectedDocumentosRecibo = _ListaResultadosDocumentosRecibo.FirstOrDefault
            End If

            MyBase.CambioItem("ListaResultadosDocumentosRecibo")
            MyBase.CambioItem("ListaResultadosDocumentosRecibo_Paged")
        End Set
    End Property

    Public ReadOnly Property ListaResultadosDocumentosRecibo_Paged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                Dim view = New PagedCollectionView(_ListaResultadosDocumentosRecibo)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _SelectedDocumentos As tblTesorero_Registros_CE
    Public Property SelectedDocumentos As tblTesorero_Registros_CE
        Get
            Return _SelectedDocumentos
        End Get
        Set(ByVal value As tblTesorero_Registros_CE)
            _SelectedDocumentos = value
            MyBase.CambioItem("SelectedDocumentos")
        End Set
    End Property

    Private _ListaResultadosDocumentos As List(Of tblTesorero_Registros_CE)
    Public Property ListaResultadosDocumentos() As List(Of tblTesorero_Registros_CE)
        Get
            Return _ListaResultadosDocumentos
        End Get
        Set(ByVal value As List(Of tblTesorero_Registros_CE))
            _ListaResultadosDocumentos = value
            If Not IsNothing(_ListaResultadosDocumentos) Then
                SelectedDocumentos = _ListaResultadosDocumentos.FirstOrDefault
            End If

            MyBase.CambioItem("ListaResultadosDocumentos")
            MyBase.CambioItem("ListaResultadosDocumentos_Paged")
        End Set
    End Property

    Public ReadOnly Property ListaResultadosDocumentos_Paged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultadosDocumentos) Then
                Dim view = New PagedCollectionView(_ListaResultadosDocumentos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _strSUCURSAL_PLUS As String
    Public Property strSUCURSAL_PLUS() As String
        Get
            Return _strSUCURSAL_PLUS
        End Get
        Set(ByVal value As String)
            _strSUCURSAL_PLUS = value
            LimpiarDetalle()
            MyBase.CambioItem("strSUCURSAL_PLUS")
        End Set
    End Property

    Private _VerConsecutivosNOTA As Visibility = Visibility.Collapsed
    Public Property VerConsecutivosNOTA() As Visibility
        Get
            Return _VerConsecutivosNOTA
        End Get
        Set(ByVal value As Visibility)
            _VerConsecutivosNOTA = value
            MyBase.CambioItem("VerConsecutivosNOTA")
        End Set
    End Property

    Private _VerConsecutivosNOTADestino As Visibility = Visibility.Collapsed
    Public Property VerConsecutivosNOTADestino() As Visibility
        Get
            Return _VerConsecutivosNOTADestino
        End Get
        Set(ByVal value As Visibility)
            _VerConsecutivosNOTADestino = value
            MyBase.CambioItem("VerConsecutivosNOTADestino")
        End Set
    End Property

    Private _strConsecutivoRC_PLUS_Origen As String
    Public Property strConsecutivoRC_PLUS_Origen() As String
        Get
            Return _strConsecutivoRC_PLUS_Origen
        End Get
        Set(ByVal value As String)
            _strConsecutivoRC_PLUS_Origen = value
            MyBase.CambioItem("strConsecutivoRC_PLUS_Origen")
        End Set
    End Property

    Private _strConsecutivoRC_PLUS_Destino As String = String.Empty
    Public Property strConsecutivoRC_PLUS_Destino() As String
        Get
            Return _strConsecutivoRC_PLUS_Destino
        End Get
        Set(ByVal value As String)
            _strConsecutivoRC_PLUS_Destino = value
            MyBase.CambioItem("strConsecutivoRC_PLUS_Destino")
        End Set
    End Property

    Private _VerConsecutivosRecibo As Visibility = Visibility.Collapsed
    Public Property VerConsecutivosRecibo() As Visibility
        Get
            Return _VerConsecutivosRecibo
        End Get
        Set(ByVal value As Visibility)
            _VerConsecutivosRecibo = value
            MyBase.CambioItem("VerConsecutivosRecibo")
        End Set
    End Property

    Private _VerConsecutivosReciboDestino As Visibility = Visibility.Collapsed
    Public Property VerConsecutivosReciboDestino() As Visibility
        Get
            Return _VerConsecutivosReciboDestino
        End Get
        Set(ByVal value As Visibility)
            _VerConsecutivosReciboDestino = value
            MyBase.CambioItem("VerConsecutivosReciboDestino")
        End Set
    End Property

    Private _VerConsecutivosEgreso As Visibility = Visibility.Collapsed
    Public Property VerConsecutivosEgreso() As Visibility
        Get
            Return _VerConsecutivosEgreso
        End Get
        Set(ByVal value As Visibility)
            _VerConsecutivosEgreso = value
            MyBase.CambioItem("VerConsecutivosEgreso")
        End Set
    End Property

    Private _VerConsecutivosEgresoDestino As Visibility = Visibility.Collapsed
    Public Property VerConsecutivosEgresoDestino() As Visibility
        Get
            Return _VerConsecutivosEgresoDestino
        End Get
        Set(ByVal value As Visibility)
            _VerConsecutivosEgresoDestino = value
            MyBase.CambioItem("VerConsecutivosEgresoDestino")
        End Set
    End Property

    Private _VerTabCheque As Visibility = Visibility.Visible
    Public Property VerTabCheque() As Visibility
        Get
            Return _VerTabCheque
        End Get
        Set(ByVal value As Visibility)
            _VerTabCheque = value
            MyBase.CambioItem("VerTabCheque")
        End Set
    End Property

    Private _VerTabChequeFondo As Visibility = Visibility.Collapsed
    Public Property VerTabChequeFondo() As Visibility
        Get
            Return _VerTabChequeFondo
        End Get
        Set(ByVal value As Visibility)
            _VerTabChequeFondo = value
            MyBase.CambioItem("VerTabChequeFondo")
        End Set
    End Property

    Private _VerTabTransferencia As Visibility = Visibility.Collapsed
    Public Property VerTabTransferencia() As Visibility
        Get
            Return _VerTabTransferencia
        End Get
        Set(ByVal value As Visibility)
            _VerTabTransferencia = value
            MyBase.CambioItem("VerTabTransferencia")
        End Set
    End Property

    Private _VerTabTransferenciaFondo As Visibility = Visibility.Collapsed
    Public Property VerTabTransferenciaFondo() As Visibility
        Get
            Return _VerTabTransferenciaFondo
        End Get
        Set(ByVal value As Visibility)
            _VerTabTransferenciaFondo = value
            MyBase.CambioItem("VerTabTransferenciaFondo")
        End Set
    End Property

    Private _VerTabTrasladoFondos As Visibility = Visibility.Collapsed
    Public Property VerTabTrasladoFondos() As Visibility
        Get
            Return _VerTabTrasladoFondos
        End Get
        Set(ByVal value As Visibility)
            _VerTabTrasladoFondos = value
            MyBase.CambioItem("VerTabTrasladoFondos")
        End Set
    End Property

    Private _VerTabCarterasColectivas As Visibility = Visibility.Collapsed
    Public Property VerTabCarterasColectivas() As Visibility
        Get
            Return _VerTabCarterasColectivas
        End Get
        Set(ByVal value As Visibility)
            _VerTabCarterasColectivas = value
            MyBase.CambioItem("VerTabCarterasColectivas")
        End Set
    End Property
    Private _VerTabInternos As Visibility = Visibility.Collapsed
    Public Property VerTabInternos() As Visibility
        Get
            Return _VerTabInternos
        End Get
        Set(ByVal value As Visibility)
            _VerTabInternos = value
            MyBase.CambioItem("VerTabInternos")
        End Set
    End Property
    Private _VerTabBloqueos As Visibility = Visibility.Collapsed
    Public Property VerTabBloqueos() As Visibility
        Get
            Return _VerTabBloqueos
        End Get
        Set(ByVal value As Visibility)
            _VerTabBloqueos = value
            MyBase.CambioItem("VerTabBloqueos")
        End Set
    End Property
    Private _VerTabOYD As Visibility = Visibility.Collapsed
    Public Property VerTabOYD() As Visibility
        Get
            Return _VerTabOYD
        End Get
        Set(ByVal value As Visibility)
            _VerTabOYD = value
            MyBase.CambioItem("VerTabOYD")
        End Set
    End Property
    Private _VerTabOperacionesEspeciales As Visibility = Visibility.Collapsed
    Public Property VerTabOperacionesEspeciales() As Visibility
        Get
            Return _VerTabOperacionesEspeciales
        End Get
        Set(ByVal value As Visibility)
            _VerTabOperacionesEspeciales = value
            MyBase.CambioItem("VerTabOperacionesEspeciales")
        End Set
    End Property
    Private _VerTabAdicionConstitucion As Visibility = Visibility.Collapsed
    Public Property VerTabAdicionConstitucion() As Visibility
        Get
            Return _VerTabAdicionConstitucion
        End Get
        Set(ByVal value As Visibility)
            _VerTabAdicionConstitucion = value
            MyBase.CambioItem("VerTabAdicionConstitucion")
        End Set
    End Property
    Private _VerTabRecibos As Visibility = Visibility.Collapsed
    Public Property VerTabRecibos() As Visibility
        Get
            Return _VerTabRecibos
        End Get
        Set(ByVal value As Visibility)
            _VerTabRecibos = value
            MyBase.CambioItem("VerTabRecibos")
        End Set
    End Property
    Private _ViewTesoreroOYDPLUS As TesoreroView
    Public Property ViewTesoreroOYDPLUS() As TesoreroView
        Get
            Return _ViewTesoreroOYDPLUS
        End Get
        Set(ByVal value As TesoreroView)
            _ViewTesoreroOYDPLUS = value
        End Set
    End Property

    Private _strIdDetalle As String
    Public Property strIdDetalle() As String
        Get
            Return _strIdDetalle
        End Get
        Set(ByVal value As String)
            _strIdDetalle = value
            MyBase.CambioItem("strIdDetalle")
        End Set
    End Property

    Private _ListaTesoreriaOrdenesPlusRC_Detalle_Cheques As New List(Of TesoreriaOyDPlusChequesRecibo)
    Public Property ListaTesoreriaOrdenesPlusRC_Detalle_Cheques() As List(Of TesoreriaOyDPlusChequesRecibo)
        Get
            Return _ListaTesoreriaOrdenesPlusRC_Detalle_Cheques
        End Get
        Set(ByVal value As List(Of TesoreriaOyDPlusChequesRecibo))
            _ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = value
            MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_Cheques")
        End Set
    End Property

    Private _ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias As List(Of TesoreriaOyDPlusTransferenciasRecibo)
    Public Property ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias() As List(Of TesoreriaOyDPlusTransferenciasRecibo)
        Get
            Return _ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias
        End Get
        Set(ByVal value As List(Of TesoreriaOyDPlusTransferenciasRecibo))
            _ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = value
            MyBase.CambioItem("ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias")
        End Set
    End Property

    Private _ListatesoreriaordenesplusRC_Detalle_CargarPagosA As List(Of TesoreriaOyDPlusCargosPagosARecibo)
    Public Property ListatesoreriaordenesplusRC_Detalle_CargarPagosA() As List(Of TesoreriaOyDPlusCargosPagosARecibo)
        Get
            Return _ListatesoreriaordenesplusRC_Detalle_CargarPagosA
        End Get
        Set(ByVal value As List(Of TesoreriaOyDPlusCargosPagosARecibo))
            _ListatesoreriaordenesplusRC_Detalle_CargarPagosA = value
            MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_CargarPagosA")
        End Set
    End Property

    Private _ListatesoreriaordenesplusRC_Detalle_Consignaciones As List(Of TesoreriaOyDPlusConsignacionesRecibo)
    Public Property ListatesoreriaordenesplusRC_Detalle_Consignaciones() As List(Of TesoreriaOyDPlusConsignacionesRecibo)
        Get
            Return _ListatesoreriaordenesplusRC_Detalle_Consignaciones
        End Get
        Set(ByVal value As List(Of TesoreriaOyDPlusConsignacionesRecibo))
            _ListatesoreriaordenesplusRC_Detalle_Consignaciones = value
            MyBase.CambioItem("ListatesoreriaordenesplusRC_Detalle_Consignaciones")
        End Set
    End Property

    Private _CarteraColectiva As String
    Public Property CarteraColectiva() As String
        Get
            Return _CarteraColectiva
        End Get
        Set(ByVal value As String)
            _CarteraColectiva = value
            CarteraColectivaDestino = _CarteraColectiva
            MyBase.CambioItem("CarteraColectiva")
        End Set
    End Property

    Private _DescripcionCarteraColectiva As String
    Public Property DescripcionCarteraColectiva() As String
        Get
            Return _DescripcionCarteraColectiva
        End Get
        Set(ByVal value As String)
            _DescripcionCarteraColectiva = value
            MyBase.CambioItem("DescripcionCarteraColectiva")
        End Set
    End Property


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

    Private _HabilitarFiltrosOrdenesGiro As Boolean = False
    Public Property HabilitarFiltrosOrdenesGiro() As Boolean
        Get
            Return _HabilitarFiltrosOrdenesGiro
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFiltrosOrdenesGiro = value
            MyBase.CambioItem("HabilitarFiltrosOrdenesGiro")
        End Set
    End Property

    Private _HabilitarFiltrosOrdenesFondos As Boolean = False
    Public Property HabilitarFiltrosOrdenesFondos() As Boolean
        Get
            Return _HabilitarFiltrosOrdenesFondos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFiltrosOrdenesFondos = value
            MyBase.CambioItem("HabilitarFiltrosOrdenesFondos")
        End Set
    End Property

    Private _HabilitarFiltrosOrdenesRecibo As Boolean = False
    Public Property HabilitarFiltrosOrdenesRecibo() As Boolean
        Get
            Return _HabilitarFiltrosOrdenesRecibo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFiltrosOrdenesRecibo = value
            MyBase.CambioItem("HabilitarFiltrosOrdenesRecibo")
        End Set
    End Property

    Private _HabilitarFiltrosCarteraColectiva As Boolean = False
    Public Property HabilitarFiltrosCarteraColectiva() As Boolean
        Get
            Return _HabilitarFiltrosCarteraColectiva
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFiltrosCarteraColectiva = value
            MyBase.CambioItem("HabilitarFiltrosCarteraColectiva")
        End Set
    End Property

    Private _TituloConsecutivoOrigen As String = "Consecutivo"
    Public Property TituloConsecutivoOrigen() As String
        Get
            Return _TituloConsecutivoOrigen
        End Get
        Set(ByVal value As String)
            _TituloConsecutivoOrigen = value
            MyBase.CambioItem("TituloConsecutivoOrigen")
        End Set
    End Property

    Private _TituloBancoOrigen As String = "Banco"
    Public Property TituloBancoOrigen() As String
        Get
            Return _TituloBancoOrigen
        End Get
        Set(ByVal value As String)
            _TituloBancoOrigen = value
            MyBase.CambioItem("TituloBancoOrigen")
        End Set
    End Property

    Private _TituloSaldoBancoOrigen As String = "Saldo Banco"
    Public Property TituloSaldoBancoOrigen() As String
        Get
            Return _TituloSaldoBancoOrigen
        End Get
        Set(ByVal value As String)
            _TituloSaldoBancoOrigen = value
            MyBase.CambioItem("TituloSaldoBancoOrigen")
        End Set
    End Property

    Private _TituloConsecutivoDestino As String = "Consecutivo"
    Public Property TituloConsecutivoDestino() As String
        Get
            Return _TituloConsecutivoDestino
        End Get
        Set(ByVal value As String)
            _TituloConsecutivoDestino = value
            MyBase.CambioItem("TituloConsecutivoDestino")
        End Set
    End Property

    Private _TituloBancoDestino As String = "Banco fondo"
    Public Property TituloBancoDestino() As String
        Get
            Return _TituloBancoDestino
        End Get
        Set(ByVal value As String)
            _TituloBancoDestino = value
            MyBase.CambioItem("TituloBancoDestino")
        End Set
    End Property

    Private _TituloSaldoBancoDestino As String = "Saldo Banco fondo"
    Public Property TituloSaldoBancoDestino() As String
        Get
            Return _TituloSaldoBancoDestino
        End Get
        Set(ByVal value As String)
            _TituloSaldoBancoDestino = value
            MyBase.CambioItem("TituloSaldoBancoDestino")
        End Set
    End Property

    Private _AgrupamientoFirmaOrigen As String
    Public Property AgrupamientoFirmaOrigen() As String
        Get
            Return _AgrupamientoFirmaOrigen
        End Get
        Set(ByVal value As String)
            _AgrupamientoFirmaOrigen = value
            MyBase.CambioItem("AgrupamientoFirmaOrigen")
        End Set
    End Property

    Private _AgrupamientoFirmaDestino As String
    Public Property AgrupamientoFirmaDestino() As String
        Get
            Return _AgrupamientoFirmaDestino
        End Get
        Set(ByVal value As String)
            _AgrupamientoFirmaDestino = value
            MyBase.CambioItem("AgrupamientoFirmaDestino")
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

    Private _FondoTextoBuscadoresDesHabilitado As SolidColorBrush
    Public Property FondoTextoBuscadoresDesHabilitado() As SolidColorBrush
        Get
            Return _FondoTextoBuscadoresDesHabilitado
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadoresDesHabilitado = value
            MyBase.CambioItem("FondoTextoBuscadoresDesHabilitado")
        End Set
    End Property

    Private _CarteraColectivaDestino As String
    Public Property CarteraColectivaDestino() As String
        Get
            Return _CarteraColectivaDestino
        End Get
        Set(ByVal value As String)
            _CarteraColectivaDestino = value
            MyBase.CambioItem("CarteraColectivaDestino")
        End Set
    End Property

#End Region

#Region "Resultados Asincrónicos"

    Public Sub TerminoDesbloqueoTesoreroTodos(lo As InvokeOperation(Of Integer))
        Try
            If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                    For Each x In _ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = Integer.Parse(lo.UserState))
                        x.Generar = False
                    Next
                End If
            Else
                If Not IsNothing(_ListaResultadosDocumentos) Then
                    For Each x In _ListaResultadosDocumentos.Where(Function(i) i.lngID = Integer.Parse(lo.UserState))
                        x.Generar = False
                    Next
                End If
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
        End Try
    End Sub

    Public Sub TerminoDesbloqueoTesorero()
        Try
            If strTipoNegocio = GSTR_ORDENRECIBO Then
                ValidarExistenciaChequeOrdenesRecibo()
            ElseIf logEsFondosOYD And strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION) Then
                ValidarExistenciaChequeOrdenesRecibo()
            ElseIf strTipoNegocio = GSTR_ORDENFONDOS And logEsFondosOYD And strTipoPagoPlus = GSTR_TRASLADOFONDOS Then
                VerificarCarteraTraslados()
            Else
                IsBusy = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub TerminoDesbloqueoTesoreroEdicionDetalle(ByVal lo As InvokeOperation(Of Integer))
        Try
            Dim lngidEncabezado As Integer = CInt(lo.UserState.ToString().Split("-")(0))
            Dim plngIdDetalle As Integer = CInt(lo.UserState.ToString().Split("-")(1))
            Dim pstrEstado As String = lo.UserState.ToString().Split("-")(2)
            Dim strDatosDetalle As String = String.Format("{0}-{1}", lngidEncabezado, plngIdDetalle)

            dcProxy.TempValidarEstadoOrdens.Clear()
            dcProxy.Load(dcProxy.ValidarEstadoOrdenTesoreriaEncabezadoQuery(plngIdDetalle, pstrEstado, Program.Usuario, "D", Program.HashConexion), AddressOf TerminoBloqueoTesoreroEdicionDetalle, strDatosDetalle)
        Catch ex As Exception

        End Try
    End Sub

    Sub TerminoConsultarDocumentosDetalleRecibo(lo As LoadOperation(Of TesoreroDetalleRecibo))
        Try
            If lo.HasError = False Then
                If dcProxy.TesoreroDetalleRecibos.Count > 0 Then
                    ListaResultadosDocumentosReciboDetalles = dcProxy.TesoreroDetalleRecibos.ToList

                    HabilitarGenerarDctos = True
                    HabilitarGeneracionDctos = True
                    Dim objlistaCargarPagosA = New List(Of TesoreroDetalleRecibo)
                    Dim objlistaDetallesRecibos = New List(Of TesoreroDetalleRecibo)

                    For Each li In ListaResultadosDocumentosReciboDetalles
                        If li.strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOA Or li.strFormaPago = GSTR_ORDENRECIBO_CARGOPAGOAFONDOS Or
                            li.ValorFormaPago = GSTR_ORDENRECIBO_CARGOPAGOA Or li.ValorFormaPago = GSTR_ORDENRECIBO_CARGOPAGOAFONDOS Then
                            ListaResultadosDocumentosReciboDetallesCargarPagosA = Nothing
                            objlistaCargarPagosA.Add(li)
                        Else
                            objlistaDetallesRecibos.Add(li)
                        End If

                    Next
                    ListaResultadosDocumentosReciboDetalles = Nothing
                    ListaResultadosDocumentosReciboDetalles = objlistaDetallesRecibos
                    ListaResultadosDocumentosReciboDetallesCargarPagosA = objlistaCargarPagosA

                    IsBusy = False

                    Dim logVerDatosFondos As Boolean = False

                    If lo.UserState = "FONDOS" Then
                        logVerDatosFondos = True
                    End If

                    Dim popupDetallesRecibos As New wppfrmDetallesRecibo(Me, logVerDatosFondos)
                    objWppDetallesRecibo = popupDetallesRecibos
                    Program.Modal_OwnerMainWindowsPrincipal(popupDetallesRecibos)
                    popupDetallesRecibos.ShowDialog()

                    ReiniciaTimer()
                Else
                    'A2Utilidades.Mensajes.mostrarMensaje("No se encontrarón Documentos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ListaResultadosDocumentosRecibo = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Documentos",
                                                 Me.ToString(), "TerminoConsultarDocumentosDetalleRecibo", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Documentos",
                                                             Me.ToString(), "TerminoConsultarDocumentosDetalleRecibo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub TerminoConsultarDocumentos(lo As LoadOperation(Of tblTesorero_Registros_CE))
        Try
            If lo.HasError = False Then
                Dim objListaDocumentos As List(Of tblTesorero_Registros_CE) = lo.Entities.ToList

                If lo.Entities.Count > 0 Then
                    HabilitarGenerarDctos = True
                    HabilitarGeneracionDctos = True

                    ListaResultadosDocumentos = Nothing
                    ListaResultadosDocumentos = objListaDocumentos

                    If ListaResultadosDocumentos.Where(Function(i) i.strValorEstado = GSTR_CUMPLIDA_Plus).Count > 0 Then
                        HabilitarGenerarDctos = False
                        VerBancoFondosDestino = Visibility.Collapsed
                        VerBancoFondos = Visibility.Collapsed
                        VerBanco = Visibility.Collapsed
                        If logEsFondosUnity Then
                            HabilitarGenerarDctos = False
                        End If
                        VerBotonAgregarArchivo = Visibility.Collapsed
                    ElseIf ListaResultadosDocumentos.Where(Function(i) i.strValorEstado = GSTR_PENDIENTE_Plus Or i.strValorEstado = GSTR_RESPUESTAADMINISTRADOR_Plus).Count > 0 Then
                        HabilitarGenerarDctos = True
                    End If

                    If logEsFondosUnity And strTipoPagoPlus = GSTR_ORDENFONDOS_CANCELACION Then
                        If Not IsNothing(ListaResultadosDocumentos) Then
                            If ListaResultadosDocumentos.Count > 0 Then
                                VerTabCheque = Visibility.Collapsed
                                VerTabChequeFondo = Visibility.Visible
                                If ListaResultadosDocumentos.Where(Function(x) x.strFormaPago = GSTR_OYD).Count > 0 Then
                                    VerConsecutivosEgreso = Visibility.Collapsed
                                    VerConsecutivosNOTA = Visibility.Visible
                                    VerBancoFondosOYD = Visibility.Collapsed
                                    VerBancoFondos = Visibility.Visible
                                    VerConsecutivosNOTADestino = Visibility.Collapsed
                                    VerConsecutivosReciboDestino = Visibility.Collapsed
                                    TituloConsecutivoOrigen = "Consecutivo"
                                    TituloBancoOrigen = "Banco"
                                    TituloSaldoBancoOrigen = "Saldo banco"
                                    AgrupamientoFirmaOrigen = "firma"
                                    VerBanco = Visibility.Visible
                                    If DiccionarioCombosOYDPlus.ContainsKey("CTA_FIRMA_OYD_FONDOS") Then
                                        IdBanco = DiccionarioCombosOYDPlus("CTA_FIRMA_OYD_FONDOS").FirstOrDefault.Retorno
                                        DescripcionBanco = DiccionarioCombosOYDPlus("CTA_FIRMA_OYD_FONDOS").FirstOrDefault.Descripcion
                                    End If
                                Else 'Es una cancelacion por otra forma de pago
                                    VerConsecutivosEgreso = Visibility.Collapsed
                                    VerConsecutivosNOTA = Visibility.Collapsed
                                    VerBancoFondosOYD = Visibility.Collapsed
                                    VerBancoFondos = Visibility.Visible
                                    VerConsecutivosNOTADestino = Visibility.Collapsed
                                    VerConsecutivosReciboDestino = Visibility.Collapsed
                                    TituloConsecutivoOrigen = "Consecutivo"
                                    TituloBancoOrigen = "Banco"
                                    TituloSaldoBancoOrigen = "Saldo banco"
                                    AgrupamientoFirmaOrigen = "firma"
                                    VerBanco = Visibility.Collapsed
                                End If
                            End If
                        End If
                    End If

                    ReiniciaTimer()
                Else
                    HabilitarGenerarDctos = False
                    HabilitarGeneracionDctos = False
                    ListaResultadosDocumentos = Nothing
                End If

                ConsultarConsecutivos(CarteraColectiva)

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Documentos",
                                                 Me.ToString(), "TerminoConsultarDocumentos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Documentos",
                                                             Me.ToString(), "TerminoConsultarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub TerminoConsultarDocumentosRecibo(lo As LoadOperation(Of tblTesorero_Registros_RC))
        Try
            If lo.HasError = False Then
                Dim objListaDocumentos As List(Of tblTesorero_Registros_RC) = lo.Entities.ToList

                If lo.Entities.Count > 0 Then
                    HabilitarGenerarDctos = True
                    HabilitarGeneracionDctos = True
                    ListaResultadosDocumentosRecibo = Nothing
                    ListaResultadosDocumentosRecibo = objListaDocumentos

                    If ListaResultadosDocumentosRecibo.Where(Function(i) i.strValorEstado = GSTR_CUMPLIDA_Plus).Count > 0 Then
                        HabilitarGenerarDctos = False
                    ElseIf ListaResultadosDocumentosRecibo.Where(Function(i) i.strValorEstado = GSTR_PENDIENTE_Plus Or i.strValorEstado = GSTR_RESPUESTAADMINISTRADOR_Plus).Count > 0 Then
                        HabilitarGenerarDctos = True
                    End If

                    ReiniciaTimer()
                Else
                    'A2Utilidades.Mensajes.mostrarMensaje("No se encontrarón Documentos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ListaResultadosDocumentosRecibo = Nothing
                End If

                ConsultarConsecutivos(CarteraColectiva)

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Documentos",
                                                 Me.ToString(), "TerminoConsultarDocumentos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Documentos",
                                                             Me.ToString(), "TerminoConsultarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub TerminoTraerSucursales(lo As LoadOperation(Of Sucursale))
        Try
            If lo.HasError = False Then
                If dcProxy2.Sucursales.Count > 0 Then
                    ListaSucursales = dcProxy2.Sucursales.ToList
                    CargarCombosOYDPLUS(String.Empty, String.Empty)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró Sucursales", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Cuentas Clientes",
                                                 Me.ToString(), "TerminoTraerSucursales", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Cuentas Clientes",
                                                             Me.ToString(), "TerminoTraerSucursales", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarParametrosReceptor(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblParametrosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaParametrosReceptor = lo.Entities.ToList
                Else
                    ListaParametrosReceptor = Nothing
                    If logNuevoRegistro Then
                        If Not IsNothing(ConfiguracionReceptor) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Señor Usuario, el receptor no tiene configurado ningun parametro maestro de parametros receptor.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Señor Usuario, el receptor no tiene configurado ningun parametro maestro de parametros receptor.{0}El receptor no tiene configurado datos en el maestro de configuraciones adicionales receptor.", vbCrLf), "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If

                If logNuevoRegistro Or logEditarRegistro Then
                    ObtenerValoresCombos(False)
                Else
                    ObtenerValoresCombos(True)
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

    Private Sub TerminoConsultarCombosOYD(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then

                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                        objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    If Not IsNothing(DiccionarioCombosOYDPlus) Then
                        DiccionarioCombosOYDPlus.Clear()
                    End If

                    DiccionarioCombosOYDPlus = Nothing
                    DiccionarioCombosOYDPlusCompleta = Nothing

                    DiccionarioCombosOYDPlusCompleta = objDiccionarioCompleto
                    DiccionarioCombosOYDPlus = objDiccionario

                    If lo.UserState = "CANCELARREGISTRO" Then
                        If Not IsNothing(TesoreriaOrdenesPlusAnterior) Then
                            'TesoreriaOrdenesPlusCE_Selected = TesoreriaOrdenesPlusAnterior
                        End If
                    Else
                        If logNuevoRegistro Or logEditarRegistro Then
                            'HabilitarNegocio = False
                            'CargarParametrosReceptorOYDPLUS(String.Empty)
                        Else
                            ObtenerValoresCombos(True)
                        End If
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Señor Usuario, usted no tiene receptores asociados.", "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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

    Private Sub _SelectedDocumentos_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _SelectedDocumentos.PropertyChanged
        If Not IsNothing(_SelectedDocumentos) Then
            If e.PropertyName = "Generar" Then
                If strEstado = GSTR_PENDIENTE_Plus Then
                    If _SelectedDocumentos.Generar = True And SeleccionarTodos = False Then
                        IsBusy = True
                        CalcularValorTotal()
                        dcProxy.TempValidarEstadoOrdens.Clear()
                        dcProxy.Load(dcProxy.ValidarEstadoOrdenTesoreriaEncabezadoQuery(String.Format("%{0}%", _SelectedDocumentos.lngID.ToString), _SelectedDocumentos.strValorEstado, Program.Usuario, GSTR_T, Program.HashConexion), AddressOf TerminoBloqueoTesorero, "")
                    ElseIf _SelectedDocumentos.Generar = False Then
                        CalcularValorTotal()
                        IsBusy = True
                        'dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(_SelectedDocumentos.lngIDEncabezado, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf TerminoDesbloqueoTesorero, String.Empty)
                        dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(_SelectedDocumentos.lngID, GSTR_T, Program.Usuario, Program.HashConexion, AddressOf TerminoDesbloqueoTesorero, String.Empty) 'DEMC20190610 Se corrige llamado a sp de cancelar edicion ya que se debe mandar el id del detalle y el retorno "T"
                    End If
                Else
                    CalcularValorTotal()
                End If
            End If
        End If

    End Sub

    Private Sub TerminoBloqueoTesoreroTodos(ByVal lo As LoadOperation(Of TempValidarEstadoOrden))
        Try
            Dim intIDDetalle As Integer = 0

            If Not lo.HasError Then

                Dim logValidarExistenciaCheque As Boolean = True

                If lo.Entities.Count > 0 Then
                    For Each li In lo.Entities
                        intIDDetalle = li.intIDDocumento
                        If li.logExitoso = False Then
                            If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                                Editando = False
                                logNoDesbloquear = True
                                For Each x In ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = intIDDetalle)
                                    x.Generar = False
                                Next
                            Else
                                Editando = False
                                logNoDesbloquear = True
                                For Each x In ListaResultadosDocumentos.Where(Function(i) i.lngID = intIDDetalle)
                                    x.Generar = False
                                Next
                            End If
                            mostrarMensaje(li.strMensajeValidacion, Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logValidarExistenciaCheque = False

                        Else
                            If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                                For Each x In ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = intIDDetalle)
                                    x.Generar = True
                                Next
                                logNoDesbloquear = True
                                IsBusy = False
                            Else
                                For Each x In ListaResultadosDocumentos.Where(Function(i) i.lngID = intIDDetalle)
                                    x.Generar = True
                                Next
                                logNoDesbloquear = True
                                IsBusy = False
                            End If
                        End If
                    Next
                    IsBusy = False
                Else
                    logNoDesbloquear = False
                    IsBusy = False
                End If

                If logValidarExistenciaCheque And strTipoNegocio = GSTR_ORDENRECIBO Then
                    ValidarExistenciaChequeOrdenesRecibo()
                ElseIf logValidarExistenciaCheque And logEsFondosOYD And strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION) Then
                    ValidarExistenciaChequeOrdenesRecibo()
                Else
                    IsBusy = False
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Se presento un problema al bloquear el registro a Generar", "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al bloquear el registro a Generar", Me.ToString(), "TerminoBloqueoTesorero", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        CalcularValorTotal()
    End Sub

    Private Sub TerminoBloqueoTesorero(ByVal lo As LoadOperation(Of TempValidarEstadoOrden))
        Try
            If Not lo.HasError Then
                Dim logValidarExistenciaCheque As Boolean = True

                If lo.Entities.Count > 0 Then
                    For Each li In lo.Entities
                        If li.logExitoso = False Then

                            Editando = False

                            logNoDesbloquear = True

                            If strTipoNegocio = GSTR_ORDENGIRO Or strTipoNegocio = GSTR_ORDENFONDOS Then
                                SelectedDocumentos.Generar = False
                            ElseIf strTipoNegocio = GSTR_ORDENRECIBO Then
                                SelectedDocumentosRecibo.Generar = False
                            End If

                            mostrarMensaje(li.strMensajeValidacion, Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logValidarExistenciaCheque = False
                        End If
                    Next
                Else
                    logNoDesbloquear = False
                End If

                If logValidarExistenciaCheque And strTipoNegocio = GSTR_ORDENRECIBO Then
                    ValidarExistenciaChequeOrdenesRecibo()
                ElseIf logValidarExistenciaCheque And logEsFondosOYD And strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION) Then
                    ValidarExistenciaChequeOrdenesRecibo()
                ElseIf strTipoNegocio = GSTR_ORDENFONDOS And logEsFondosOYD And strTipoPagoPlus = GSTR_TRASLADOFONDOS Then
                    VerificarCarteraTraslados()
                Else
                    IsBusy = False
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Se presento un problema al bloquear el registro a Generar", "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al bloquear el registro a Generar", Me.ToString(), "TerminoBloqueoTesorero", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoBloqueoTesoreroEdicionDetalle(ByVal lo As LoadOperation(Of TempValidarEstadoOrden))
        Try
            If Not lo.HasError Then
                Dim logContinuarMostrarDetalle As Boolean = True

                If lo.Entities.Count > 0 Then
                    For Each li In lo.Entities
                        If li.logExitoso = False Then
                            logContinuarMostrarDetalle = False
                            mostrarMensaje(li.strMensajeValidacion, Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Next
                End If
                If logContinuarMostrarDetalle Then
                    IsBusy = False
                    Dim lngidEncabezado As Integer = CInt(lo.UserState.ToString().Split("-")(0))
                    Dim plngIdDetalle As Integer = CInt(lo.UserState.ToString().Split("-")(1))
                    If strTipoPagoPlus = GSTR_ORDENFONDOS_CANCELACION Then 'Si es cancelacion edita la forma de pago equivale al valor cero
                        If strFormaPagoCancelacion = GSTR_CHEQUE Or strFormaPagoCancelacion = GSTR_CHEQUE_GERENCIA Then
                            LevantarModalCheque(plngIdDetalle, lngidEncabezado)
                        ElseIf strFormaPagoCancelacion = GSTR_TRANSFERENCIA Then
                            LevantarModalTransferencia(plngIdDetalle, lngidEncabezado)
                        ElseIf strFormaPagoCancelacion = GSTR_OYD Then
                            LevantarModalOYD(plngIdDetalle, lngidEncabezado)
                        ElseIf strFormaPagoCancelacion = GSTR_TRASLADOFONDOS Then
                            LevantarModalTrasladoFondos(plngIdDetalle, lngidEncabezado)
                        End If
                    Else
                        If strTipoPagoPlus = GSTR_CHEQUE Or strTipoPagoPlus = GSTR_CHEQUE_GERENCIA Then
                            LevantarModalCheque(plngIdDetalle, lngidEncabezado)
                        ElseIf strTipoPagoPlus = GSTR_TRANSFERENCIA Then
                            LevantarModalTransferencia(plngIdDetalle, lngidEncabezado)
                        ElseIf strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS Then
                            LevantarModalCarterasColectivas(plngIdDetalle, lngidEncabezado)
                        ElseIf strTipoPagoPlus = GSTR_TRASLADOFONDOS Then
                            LevantarModalTrasladoFondos(plngIdDetalle, lngidEncabezado)
                        ElseIf strTipoPagoPlus = GSTR_INTERNOS Then
                            LevantarModalInternos(plngIdDetalle, lngidEncabezado)
                        ElseIf strTipoPagoPlus = GSTR_OYD Then
                            LevantarModalOYD(plngIdDetalle, lngidEncabezado)
                        End If
                    End If
                Else
                    IsBusy = False
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Se presento un problema al bloquear el registro a Generar", "Ordenes Tesoreria", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al bloquear el registro a Generar", Me.ToString(), "TerminoBloqueoTesorero", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub VerificarCarteraTraslados()
        Try
            Dim intCantidadCarteras As Integer = 0
            Dim strCarteraValidacion As String = String.Empty

            'S-45953: se realiza validacion de lista resultado documentos no este vacia y evitar error de objeto sin instancia
            If Not IsNothing(ListaResultadosDocumentos) Then
                For Each li In ListaResultadosDocumentos
                    If li.Generar Then
                        If li.strNombreCarteraColectivaDetalle <> strCarteraValidacion Then
                            strCarteraValidacion = li.strNombreCarteraColectivaDetalle
                            intCantidadCarteras += 1
                        End If
                    End If
                Next
            End If


            If intCantidadCarteras > 1 Then
                mostrarMensaje("No se pueden seleccionar registros con diferente Fondo Destino.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ListaConsecutivosNotasDestino = Nothing
                ListaConsecutivosEgresosDestino = Nothing
                ListaConsecutivosRecibosDestino = Nothing
                strConsecutivoNOTAS_PLUS_Destino = String.Empty
                strConsecutivoCE_PLUS_Destino = String.Empty
                strConsecutivoRC_PLUS_Destino = String.Empty
                CarteraColectivaDestino = String.Empty
                IdBanco_FondosOYD = Nothing
                DescripcionBanco_FondosOYD = String.Empty

                IsBusy = False
            Else
                CarteraColectivaDestino = strCarteraValidacion
                ConsultarConsecutivos(CarteraColectivaDestino, "CONSECUTIVOSDESTINOTRASLADO")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar la cartera de los registro a Generar", Me.ToString(), "VerificarCarteraTraslados", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Async Sub TerminoPreguntarConfirmacionRechazar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then

                IsBusy = True
                Dim logLlamoGeneracion As Boolean = False

                Dim objID As String = ""
                Dim logpermitir As Boolean = True

                If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                    If Not IsNothing(ListaResultadosDocumentosRecibo) Then
                        If ListaResultadosDocumentosRecibo.Count > 0 Then
                            For Each li In ListaResultadosDocumentosRecibo
                                If li.Generar Then
                                    IsBusy = True
                                    If logpermitir Then
                                        objID = String.Format("{0}", li.lngIDEncabezado)
                                        logpermitir = False
                                    Else
                                        objID = String.Format("{0}|{1}", objID, li.lngIDEncabezado)
                                    End If
                                Else
                                    li.Generar = False
                                End If
                            Next

                            Await GenerarRechazoDocumento(objID, objResultado.Observaciones)
                        End If
                    End If
                Else
                    If Not IsNothing(ListaResultadosDocumentos) Then
                        If ListaResultadosDocumentos.Count > 0 Then
                            For Each li In ListaResultadosDocumentos
                                If li.Generar Then
                                    IsBusy = True
                                    If logpermitir Then
                                        objID = String.Format("{0}", li.lngID)
                                        logpermitir = False
                                    Else
                                        objID = String.Format("{0}|{1}", objID, li.lngID)
                                    End If
                                Else
                                    li.Generar = False
                                End If
                            Next

                            If logEsFondosOYD = True Then
                                logLlamoGeneracion = True
                                SenderAux = sender
                                strIdDetalle = objID
                                dcProxy.VerificarRechazoOrdenFondosOyD(objID, strTipoNegocio, Program.Usuario, Program.HashConexion, AddressOf TerminoVerificarRechazarDetalle, "")
                            Else
                                Await GenerarRechazoDocumento(objID, objResultado.Observaciones)
                            End If
                        End If
                    End If
                End If

                If logLlamoGeneracion = False Then
                    IsBusy = False
                End If
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Rechazar Documento", Me.ToString(), "TerminoPreguntarConfirmacionRechazar", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Async Function GenerarRechazoDocumento(ByVal pstrIDRegistros As String, ByVal pstrObservaciones As String) As Task(Of Boolean)
        Try
            Dim objRespuesta As LoadOperation(Of tblRespuestaValidacionesTesoreria) = Nothing
            Dim objProxy As OYDPLUSTesoreriaDomainContext

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                objProxy = New OYDPLUSTesoreriaDomainContext()
            Else
                objProxy = New OYDPLUSTesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If

            DirectCast(objProxy.DomainClient, WebDomainClient(Of OYDPLUSTesoreriaDomainContext.IOYDPLUSTesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            objRespuesta = Await objProxy.Load(objProxy.Tesorero_RechazarRegistrosQuery(strTipoNegocio, strTipoPagoPlus, pstrIDRegistros, pstrObservaciones, Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRespuesta Is Nothing Then
                If objRespuesta.HasError = False Then
                    Dim logDocumentosRechazadosExitosamente As Boolean = False
                    Dim logTieneErrores As Boolean = False
                    Dim strMensajeExistoso As String = "¡Documento(s) rechazado(s) con éxito!"
                    Dim strMensajeError As String = "¡Algunos Documento(s) presentaron un problema al rechazar!"

                    For Each li In objRespuesta.Entities.ToList
                        If li.Exitoso Then
                            logDocumentosRechazadosExitosamente = True
                            strMensajeExistoso = String.Format("{0}{1}{2}", strMensajeExistoso, vbCrLf, li.Mensaje)
                        Else
                            logTieneErrores = True
                            strMensajeError = String.Format("{0}{1}{2}", strMensajeError, vbCrLf, li.Mensaje)
                        End If
                    Next

                    If logDocumentosRechazadosExitosamente Then
                        ConsultarDocumentos()
                        mostrarMensaje(strMensajeExistoso, "Rechazo de Documentos", A2Utilidades.wppMensajes.TiposMensaje.Exito)

                        If logTieneErrores Then
                            mostrarMensaje(strMensajeError, "Rechazo de Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If

                        LimpiarDetalle()
                        LimpiarCampos()
                        Return True
                    Else
                        mostrarMensaje(strMensajeError, "Rechazo de Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return False
                    End If
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Generar Documento(s)",
                                                        Me.ToString(), "GenerarRechazoDocumento", Application.Current.ToString(), Program.Maquina, objRespuesta.Error)
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Rechazar Documento", Me.ToString(), "GenerarRechazoDocumento", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Public Async Sub TerminoPreguntarAutogestionDocumento(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                IsBusy = True
                Dim objRespuesta As LoadOperation(Of tblRespuestaValidacionesTesoreria)
                Dim objProxy As OYDPLUSTesoreriaDomainContext

                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    objProxy = New OYDPLUSTesoreriaDomainContext()
                Else
                    objProxy = New OYDPLUSTesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                End If

                DirectCast(objProxy.DomainClient, WebDomainClient(Of OYDPLUSTesoreriaDomainContext.IOYDPLUSTesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

                objRespuesta = Await objProxy.Load(objProxy.Tesorero_AutogestionarDocumentoQuery(plngIdEncabezadoAutogestion, objResultado.Observaciones, Program.Maquina, Program.Usuario, Program.UsuarioWindows, Program.HashConexion)).AsTask()

                If Not objRespuesta Is Nothing Then
                    If objRespuesta.HasError = False Then

                        Dim logMostrarMensajeExitoso As Boolean = True

                        If objRespuesta.Entities.Count > 0 Then
                            If objRespuesta.Entities.Where(Function(i) i.Exitoso).Count > 0 Then
                                mostrarMensaje(objRespuesta.Entities.Where(Function(i) i.Exitoso).First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                                ConsultarRegistrosTesorero(String.Empty)
                            Else
                                If objRespuesta.Entities.Where(Function(i) i.Documento = "NODISPONIBLE").Count > 0 Then
                                    mostrarMensaje(objRespuesta.Entities.Where(Function(i) i.Documento = "NODISPONIBLE").First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    ConsultarRegistrosTesorero(String.Empty)
                                Else
                                    mostrarMensaje(objRespuesta.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                            End If
                        Else
                            IsBusy = False
                            ConsultarRegistrosTesorero(String.Empty)
                        End If
                    Else
                        IsBusy = False
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Generar Documento(s)",
                                                    Me.ToString(), "GenerarDocumentos", Application.Current.ToString(), Program.Maquina, objRespuesta.Error)
                    End If
                Else
                    IsBusy = False
                End If
            Else
                IsBusy = True
                objOrdenTesoreria = New OrdenGiroConsultaView(plngIdEncabezadoAutogestion, plogEditarOrdenAutogestion, plogPendientePorAprobarAutogestion, Me)
                objOrdenTesoreria.Width = Application.Current.MainWindow.ActualWidth
                Program.Modal_OwnerMainWindowsPrincipal(objOrdenTesoreria)
                objOrdenTesoreria.ShowDialog()
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Rechazar Documento", Me.ToString(), "TerminoPreguntarConfirmacionRechazar", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Async Sub TerminoVerificarRechazarDetalle(lo As InvokeOperation(Of String))
        Try
            Dim strMsg As String = String.Empty

            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(SenderAux, A2Utilidades.wppMensajePregunta)

            If Not lo.HasError Then
                If Not String.IsNullOrEmpty(lo.Value.ToString()) Then
                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, lo.Value.ToString())

                    If Not String.IsNullOrEmpty(strMsg) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    Await GenerarRechazoDocumento(strIdDetalle, objResultado.Observaciones)
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Rechazar el registro",
                                            Me.ToString(), "TerminoVerificarRechazarDetalle", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la verificación de encargos creados de la lista de Ordenes Tesoreria Plus ",
                                                             Me.ToString(), "TerminoVerificarBorrarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Sub TerminoTraerTesoreriaOrdenesDetalleTransferencias(ByVal lo As LoadOperation(Of TesoreriaOyDPlusTransferenciasRecibo))
        Try
            If Not lo.HasError Then
                ListaTesoreriaOrdenesPlusRC_Detalle_Transferencias = dcProxy.TesoreriaOyDPlusTransferenciasRecibos.ToList
                dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_Cheques_ReciboQuery(SelectedDocumentosRecibo.lngIDEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleCheques, lo.UserState)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Transferencias",
                                                 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleTransferencias", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria  Detalle Plus Transferencias",
                                                             Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleTransferencias", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub TerminoTraerTesoreriaOrdenesDetalleCheques(ByVal lo As LoadOperation(Of TesoreriaOyDPlusChequesRecibo))
        Try
            If Not lo.HasError Then
                ListaTesoreriaOrdenesPlusRC_Detalle_Cheques = dcProxy.TesoreriaOyDPlusChequesRecibos.ToList
                dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_CargosPagosA_ReciboQuery(SelectedDocumentosRecibo.lngIDEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleCargarPagosA, lo.UserState)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
                                                 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
                                                             Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub TerminoTraerTesoreriaOrdenesDetalleCargarPagosA(ByVal lo As LoadOperation(Of TesoreriaOyDPlusCargosPagosARecibo))
        Try
            If Not lo.HasError Then
                ListatesoreriaordenesplusRC_Detalle_CargarPagosA = dcProxy.TesoreriaOyDPlusCargosPagosARecibos.ToList
                dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_ConsignacionesReciboQuery(SelectedDocumentosRecibo.lngIDEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleConsignaciones, lo.UserState)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
                                                 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Cheques",
                                                             Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleCheques", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub TerminoTraerTesoreriaOrdenesDetalleConsignaciones(ByVal lo As LoadOperation(Of TesoreriaOyDPlusConsignacionesRecibo))
        Try
            If Not lo.HasError Then
                ListatesoreriaordenesplusRC_Detalle_Consignaciones = dcProxy.TesoreriaOyDPlusConsignacionesRecibos.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus Consignaciones",
                                                 Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleConsignaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Tesoreria Detalle Plus consignaciones",
                                                             Me.ToString(), "TerminoTraerTesoreriaOrdenesDetalleConsignaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub TerminoValidarExistenciaChequeOrdenRecibo(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If Not lo.HasError Then

                MostrarBancosRecibosCheques()

                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la existencia del cheque en las ordenes de recibos.",
                                                 Me.ToString(), "TerminoValidarExistenciaChequeOrdenRecibo", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la existencia del cheque en las ordenes de recibos.",
                                                             Me.ToString(), "TerminoValidarExistenciaChequeOrdenRecibo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGenerarArchivoCartaGerencia(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
        Try
            If lo.HasError = False Then
                Program.VisorArchivosWeb_DescargarURL(lo.Entities.First.RutaArchivoPlano)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de la carta de gerencia",
                                     Me.ToString(), "TerminoGenerarArchivoCartaGerencia", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de la carta de gerencia",
                                     Me.ToString(), "TerminoGenerarArchivoCartaGerencia", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoTraerConsultarConsecutivos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                Dim objListaConsecutivosNotas As New List(Of OYDUtilidades.ItemCombo)
                Dim objListaConsecutivosEgresos As New List(Of OYDUtilidades.ItemCombo)
                Dim objListaConsecutivosRecibos As New List(Of OYDUtilidades.ItemCombo)
                Dim objListaConsecutivosNotasDestino As New List(Of OYDUtilidades.ItemCombo)
                Dim objListaConsecutivosEgresosDestino As New List(Of OYDUtilidades.ItemCombo)
                Dim objListaConsecutivosRecibosDestino As New List(Of OYDUtilidades.ItemCombo)

                For Each li In lo.Entities.ToList
                    If li.Categoria = "NOTAS" Then
                        objListaConsecutivosNotas.Add(li)
                    ElseIf li.Categoria = "EGRESOS" Then
                        objListaConsecutivosEgresos.Add(li)
                    ElseIf li.Categoria = "CAJA" Then
                        objListaConsecutivosRecibos.Add(li)
                    ElseIf li.Categoria = "NOTAS_FONDO" Then
                        objListaConsecutivosNotasDestino.Add(li)
                    ElseIf li.Categoria = "EGRESOS_FONDO" Then
                        objListaConsecutivosEgresosDestino.Add(li)
                    ElseIf li.Categoria = "CAJA_FONDO" Then
                        objListaConsecutivosRecibosDestino.Add(li)
                    End If
                Next

                If strTipoNegocio = GSTR_ORDENGIRO Then
                    If strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS Then
                        If logEsFondosOYD Then
                            ListaConsecutivosEgresosOrigen = objListaConsecutivosEgresos
                            ListaConsecutivosRecibosDestino = objListaConsecutivosRecibosDestino
                        Else
                            ListaConsecutivosNotasOrigen = objListaConsecutivosNotas
                        End If
                    Else
                        ListaConsecutivosEgresosOrigen = objListaConsecutivosEgresos
                        ListaConsecutivosNotasOrigen = objListaConsecutivosNotas
                    End If
                ElseIf strTipoNegocio = GSTR_ORDENRECIBO Then
                    ListaConsecutivosRecibosOrigen = objListaConsecutivosRecibos
                ElseIf strTipoNegocio = GSTR_ORDENFONDOS Then
                    If strTipoPagoPlus = GSTR_OYD Then
                        If logEsFondosOYD Then
                            ListaConsecutivosEgresosOrigen = objListaConsecutivosEgresosDestino
                            ListaConsecutivosRecibosDestino = objListaConsecutivosRecibos
                        Else
                            ListaConsecutivosNotasOrigen = objListaConsecutivosNotas
                        End If
                    Else
                        If logEsFondosOYD Then
                            If strTipoPagoPlus = GSTR_TRASLADOFONDOS Then
                                If lo.UserState = "CONSECUTIVOSDESTINOTRASLADO" Then
                                    ListaConsecutivosNotasDestino = objListaConsecutivosNotasDestino
                                    ListaConsecutivosEgresosDestino = objListaConsecutivosEgresosDestino
                                    ListaConsecutivosRecibosDestino = objListaConsecutivosRecibosDestino
                                    IsBusy = False
                                Else
                                    ListaConsecutivosNotasOrigen = objListaConsecutivosNotasDestino
                                    ListaConsecutivosEgresosOrigen = objListaConsecutivosEgresosDestino
                                    ListaConsecutivosRecibosOrigen = objListaConsecutivosRecibosDestino

                                    ListaConsecutivosNotasDestino = Nothing
                                    ListaConsecutivosEgresosDestino = Nothing
                                    ListaConsecutivosRecibosDestino = Nothing
                                    strConsecutivoNOTAS_PLUS_Destino = String.Empty
                                    strConsecutivoCE_PLUS_Destino = String.Empty
                                    strConsecutivoRC_PLUS_Destino = String.Empty
                                End If
                            Else
                                ListaConsecutivosNotasOrigen = objListaConsecutivosNotasDestino
                                ListaConsecutivosNotasDestino = objListaConsecutivosNotasDestino
                                ListaConsecutivosEgresosOrigen = objListaConsecutivosEgresosDestino
                                ListaConsecutivosEgresosDestino = objListaConsecutivosEgresosDestino
                                ListaConsecutivosRecibosOrigen = objListaConsecutivosRecibosDestino
                                ListaConsecutivosRecibosDestino = objListaConsecutivosRecibosDestino
                            End If
                        Else
                            ListaConsecutivosNotasOrigen = objListaConsecutivosNotas
                            ListaConsecutivosNotasDestino = objListaConsecutivosNotas
                            ListaConsecutivosEgresosOrigen = objListaConsecutivosEgresos
                            ListaConsecutivosEgresosDestino = objListaConsecutivosEgresos
                            ListaConsecutivosRecibosOrigen = objListaConsecutivosRecibos
                            ListaConsecutivosRecibosDestino = objListaConsecutivosRecibos
                        End If
                    End If
                End If

                If VerConsecutivosNOTA = Visibility.Visible Then
                    If Not IsNothing(_ListaConsecutivosNotasOrigen) Then
                        If _ListaConsecutivosNotasOrigen.Count = 1 Then
                            strConsecutivoNOTAS_PLUS_Origen = String.Empty
                            strConsecutivoNOTAS_PLUS_Origen = _ListaConsecutivosNotasOrigen.First.ID
                        End If
                    End If
                End If

                If VerConsecutivosEgreso = Visibility.Visible Then
                    If Not IsNothing(_ListaConsecutivosEgresosOrigen) Then
                        If _ListaConsecutivosEgresosOrigen.Count = 1 Then
                            strConsecutivoCE_PLUS_Origen = String.Empty
                            strConsecutivoCE_PLUS_Origen = _ListaConsecutivosEgresosOrigen.First.ID
                        End If
                    End If
                End If

                If VerConsecutivosRecibo = Visibility.Visible Then
                    If Not IsNothing(_ListaConsecutivosRecibosOrigen) Then
                        If _ListaConsecutivosRecibosOrigen.Count = 1 Then
                            strConsecutivoRC_PLUS_Origen = String.Empty
                            strConsecutivoRC_PLUS_Origen = _ListaConsecutivosRecibosOrigen.First.ID
                        End If
                    End If
                End If

                If VerConsecutivosNOTADestino = Visibility.Visible Then
                    If Not IsNothing(_ListaConsecutivosNotasDestino) Then
                        If _ListaConsecutivosNotasDestino.Count = 1 Then
                            strConsecutivoNOTAS_PLUS_Destino = String.Empty
                            strConsecutivoNOTAS_PLUS_Destino = _ListaConsecutivosNotasDestino.First.ID
                        End If
                    End If
                End If

                If VerConsecutivosEgresoDestino = Visibility.Visible Then
                    If Not IsNothing(_ListaConsecutivosEgresosDestino) Then
                        If _ListaConsecutivosEgresosDestino.Count = 1 Then
                            strConsecutivoCE_PLUS_Destino = String.Empty
                            strConsecutivoCE_PLUS_Destino = _ListaConsecutivosEgresosDestino.First.ID
                        End If
                    End If
                End If

                If VerConsecutivosReciboDestino = Visibility.Visible Then
                    If Not IsNothing(_ListaConsecutivosRecibosDestino) Then
                        If _ListaConsecutivosRecibosDestino.Count = 1 Then
                            strConsecutivoRC_PLUS_Destino = String.Empty
                            strConsecutivoRC_PLUS_Destino = _ListaConsecutivosRecibosDestino.First.ID
                        End If
                    End If
                End If

                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los consecutivos",
                                                 Me.ToString(), "TerminoTraerConsultarConsecutivos", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los consecutivos",
                                                 Me.ToString(), "TerminoTraerConsultarConsecutivos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"
    Private Sub ConsultarRegistrosTesorero(ByVal pstrUserState As String)
        Try
            IsBusy = True
            If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                If Not IsNothing(dcProxy.tblTesorero_Registros_RCs) Then
                    dcProxy.tblTesorero_Registros_RCs.Clear()
                End If
                dcProxy.Load(dcProxy.Tesorero_ConsultarRegistros_RCQuery(ItemReceptor, strTipoNegocio, strEstado, strTipoPagoPlus, Fecha, Integer.Parse(strSUCURSAL_PLUS), Program.Usuario, CarteraColectiva, Program.HashConexion), AddressOf TerminoConsultarDocumentosRecibo, pstrUserState)
            Else
                If Not IsNothing(dcProxy.tblTesorero_Registros_CEs) Then
                    dcProxy.tblTesorero_Registros_CEs.Clear()
                End If
                dcProxy.Load(dcProxy.Tesorero_ConsultarRegistros_CEQuery(ItemReceptor, strTipoNegocio, strEstado, strTipoPagoPlus, Fecha, Integer.Parse(strSUCURSAL_PLUS), Program.Usuario, CarteraColectiva, Program.HashConexion), AddressOf TerminoConsultarDocumentos, pstrUserState)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros",
                                     Me.ToString(), "ConsultarRegistrosTesorero", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub VerOrdenTesoreria(ByVal plngIdEncabezado As Integer, ByVal plogEditarOrden As Boolean, ByVal plogPendientePorAprobar As Boolean)
        Try
            If Not IsNothing(plngIdEncabezado) Then
                If plogPendientePorAprobar And plogFuncionalidadAutogestionDocumentos Then
                    plngIdEncabezadoAutogestion = plngIdEncabezado
                    plogEditarOrdenAutogestion = plogEditarOrden
                    plogPendientePorAprobarAutogestion = plogPendientePorAprobar
                    mostrarMensajePregunta("¿El documento se encuentra pendiente por aprobar, desea autogestionar el documento?",
                                               Program.TituloSistema,
                                               "AUTOGESTIONARDOCUMENTO",
                                               AddressOf TerminoPreguntarAutogestionDocumento,
                                               False, String.Empty, False, True, True, False)
                Else
                    IsBusy = True
                    objOrdenTesoreria = New OrdenGiroConsultaView(plngIdEncabezado, plogEditarOrden, plogPendientePorAprobar, Me)
                    objOrdenTesoreria.Width = Application.Current.MainWindow.ActualWidth
                    Program.Modal_OwnerMainWindowsPrincipal(objOrdenTesoreria)
                    objOrdenTesoreria.ShowDialog()
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "VerOrdenTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub EditarDetalle(ByVal plngIdDetalle As Integer, ByVal plogSeleccionado As Boolean)
        Try
            If Not IsNothing(plngIdDetalle) Then
                If ListaResultadosDocumentos.Where(Function(i) i.lngID = plngIdDetalle).FirstOrDefault.strValorEstado = "I" Or ListaResultadosDocumentos.Where(Function(i) i.lngID = plngIdDetalle).FirstOrDefault.strValorEstado = "V" Then
                    mostrarMensaje("El detalle no se puede editar porque la orden se encuentra en un estado que requiere confirmación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                Dim lngidEncabezado = ListaResultadosDocumentos.Where(Function(i) i.lngID = plngIdDetalle).FirstOrDefault.lngIDEncabezado
                strFormaPagoCancelacion = ListaResultadosDocumentos.Where(Function(i) i.lngID = plngIdDetalle).FirstOrDefault.strFormaPago

                If strEstado = GSTR_PENDIENTE_Plus Then
                    If plogSeleccionado Then
                        Dim strDatosDetalle As String = String.Format("{0}-{1}-{2}", lngidEncabezado, plngIdDetalle, strEstado)
                        IsBusy = True
                        dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(_SelectedDocumentos.lngID, GSTR_T, Program.Usuario, Program.HashConexion, AddressOf TerminoDesbloqueoTesoreroEdicionDetalle, strDatosDetalle)
                    Else
                        Dim strDatosDetalle As String = String.Format("{0}-{1}", lngidEncabezado, plngIdDetalle)
                        IsBusy = True
                        dcProxy.TempValidarEstadoOrdens.Clear()
                        dcProxy.Load(dcProxy.ValidarEstadoOrdenTesoreriaEncabezadoQuery(plngIdDetalle, strEstado, Program.Usuario, "D", Program.HashConexion), AddressOf TerminoBloqueoTesoreroEdicionDetalle, strDatosDetalle)
                    End If
                Else
                    IsBusy = False
                    mostrarMensaje("¡Solo se pueden editar registros con estado pendiente!", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "EditarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LevantarModalCheque(ByVal pintIDDetalle As Integer, ByVal pintIDEncabezado As Integer)
        Try
            Dim objDetalle As New OrdenPago_DetalleChequesView(pintIDDetalle, pintIDEncabezado)
            AddHandler objDetalle.OrdenPago_FinalizoGuardarRegistro_BaseDatos, AddressOf TerminoGuadarDetalleTesoreria
            AddHandler objDetalle.OrdenPago_CancelarGuardarRegistro, AddressOf TerminoCancelarDetalleTesoreria
            Program.Modal_OwnerMainWindowsPrincipal(objDetalle)
            objDetalle.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "LevantarModalCheque", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LevantarModalTransferencia(ByVal pintIDDetalle As Integer, ByVal pintIDEncabezado As Integer)
        Try
            Dim objDetalle As New OrdenPago_DetalleTransferenciaView(pintIDDetalle, pintIDEncabezado)
            AddHandler objDetalle.OrdenPago_FinalizoGuardarRegistro_BaseDatos, AddressOf TerminoGuadarDetalleTesoreria
            AddHandler objDetalle.OrdenPago_CancelarGuardarRegistro, AddressOf TerminoCancelarDetalleTesoreria
            Program.Modal_OwnerMainWindowsPrincipal(objDetalle)
            objDetalle.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "LevantarModalCheque", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LevantarModalCarterasColectivas(ByVal pintIDDetalle As Integer, ByVal pintIDEncabezado As Integer)
        Try
            Dim objDetalle As New OrdenPago_DetalleCarteraColectivaView(pintIDDetalle, pintIDEncabezado)
            AddHandler objDetalle.OrdenPago_FinalizoGuardarRegistro_BaseDatos, AddressOf TerminoGuadarDetalleTesoreria
            AddHandler objDetalle.OrdenPago_CancelarGuardarRegistro, AddressOf TerminoCancelarDetalleTesoreria
            Program.Modal_OwnerMainWindowsPrincipal(objDetalle)
            objDetalle.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "LevantarModalCheque", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LevantarModalInternos(ByVal pintIDDetalle As Integer, ByVal pintIDEncabezado As Integer)
        Try
            Dim objDetalle As New OrdenPago_DetalleInternosView(pintIDDetalle, pintIDEncabezado)
            AddHandler objDetalle.OrdenPago_FinalizoGuardarRegistro_BaseDatos, AddressOf TerminoGuadarDetalleTesoreria
            AddHandler objDetalle.OrdenPago_CancelarGuardarRegistro, AddressOf TerminoCancelarDetalleTesoreria
            Program.Modal_OwnerMainWindowsPrincipal(objDetalle)
            objDetalle.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "LevantarModalCheque", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LevantarModalTrasladoFondos(ByVal pintIDDetalle As Integer, ByVal pintIDEncabezado As Integer)
        Try
            Dim objDetalle As New OrdenPago_DetalleTrasladoFondosView(pintIDDetalle, pintIDEncabezado)
            AddHandler objDetalle.OrdenPago_FinalizoGuardarRegistro_BaseDatos, AddressOf TerminoGuadarDetalleTesoreria
            AddHandler objDetalle.OrdenPago_CancelarGuardarRegistro, AddressOf TerminoCancelarDetalleTesoreria
            Program.Modal_OwnerMainWindowsPrincipal(objDetalle)
            objDetalle.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "LevantarModalCheque", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LevantarModalOYD(ByVal pintIDDetalle As Integer, ByVal pintIDEncabezado As Integer)
        Try
            Dim objDetalle As New OrdenPago_DetalleOYDView(pintIDDetalle, pintIDEncabezado)
            AddHandler objDetalle.OrdenPago_FinalizoGuardarRegistro_BaseDatos, AddressOf TerminoGuadarDetalleTesoreria
            AddHandler objDetalle.OrdenPago_CancelarGuardarRegistro, AddressOf TerminoCancelarDetalleTesoreria
            Program.Modal_OwnerMainWindowsPrincipal(objDetalle)
            objDetalle.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "LevantarModalCheque", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGuadarDetalleTesoreria(ByVal pintIDDetalle As Integer, ByVal pintIDEncabezado As Integer)
        Try
            ConsultarRegistrosTesorero(String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del detalle.", Me.ToString(), "TerminoGuadarDetalleTesoreria", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCancelarDetalleTesoreria(ByVal pintIDDetalle As Integer, ByVal pintIDEncabezado As Integer)
        Try
            ConsultarRegistrosTesorero(String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del detalle.", Me.ToString(), "TerminoCancelarDetalleTesoreria", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EditarRecibo(ByVal plngIdRecibo As Integer)
        Try
            IsBusy = True
            objOrdenRecibo = New OrdenReciboConsultaView(plngIdRecibo, Me)
            Program.Modal_OwnerMainWindowsPrincipal(objOrdenRecibo)
            objOrdenRecibo.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Ver la Orden",
                                     Me.ToString(), "EditarRecibo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Sub LimpiarCampos()
        Try
            SeleccionarTodos = False
            strConsecutivoCE_PLUS_Origen = String.Empty
            strConsecutivoNOTAS_PLUS_Origen = String.Empty
            strConsecutivoRC_PLUS_Origen = String.Empty
            strConsecutivoCE_PLUS_Destino = String.Empty
            strConsecutivoNOTAS_PLUS_Destino = String.Empty
            strConsecutivoRC_PLUS_Destino = String.Empty

            DescripcionBanco = String.Empty
            DescripcionBancoFondo = String.Empty
            DescripcionBancoFondoDestino = String.Empty
            IdBancoFondo = Nothing
            IdBancoFondoDestino = Nothing
            IdBanco = Nothing
            ConsultarSaldoBanco = False
            TotalGenerar = 0
            IdBanco_FondosOYD = Nothing
            DescripcionBanco_FondosOYD = Nothing
            ConsultarSaldoBanco_FondosOYD = False

            OcultarCamposConsecutivosBancos()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los bancos",
                                     Me.ToString(), "LimpiarCampos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub OcultarCamposConsecutivosBancos()
        Try
            If strTipoPagoPlus = GSTR_CHEQUE Then
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed

                If strTipoNegocio = GSTR_ORDENFONDOS Then
                    VerTabCheque = Visibility.Collapsed
                    VerTabChequeFondo = Visibility.Visible
                    VerConsecutivosEgreso = Visibility.Collapsed

                    If logEsFondosOYD Then
                        VerBanco = Visibility.Visible
                        VerConsecutivosEgreso = Visibility.Visible
                        VerBancoFondos = Visibility.Collapsed
                        TituloConsecutivoOrigen = "Consecutivo fondo"
                        TituloBancoOrigen = "Banco fondo"
                        TituloSaldoBancoOrigen = "Saldo banco fondo"
                        AgrupamientoFirmaOrigen = "cartera"
                        AgrupamientoFirmaDestino = "cartera"
                    Else
                        VerBanco = Visibility.Collapsed
                        VerBancoFondos = Visibility.Visible
                        TituloConsecutivoOrigen = "Consecutivo"
                        TituloBancoOrigen = "Banco"
                        TituloSaldoBancoOrigen = "Saldo banco"
                        AgrupamientoFirmaOrigen = "firma"
                        AgrupamientoFirmaDestino = "firma"
                    End If
                Else
                    VerTabCheque = Visibility.Visible
                    VerTabChequeFondo = Visibility.Collapsed
                    VerConsecutivosEgreso = Visibility.Visible
                    VerBanco = Visibility.Visible
                    VerBancoFondos = Visibility.Collapsed
                    TituloConsecutivoOrigen = "Consecutivo"
                    TituloBancoOrigen = "Banco"
                    TituloSaldoBancoOrigen = "Saldo banco"
                    AgrupamientoFirmaOrigen = "firma"
                End If

                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed
                VerConsecutivosReciboDestino = Visibility.Collapsed
                VerBancoFondosOYD = Visibility.Collapsed

            ElseIf strTipoPagoPlus = GSTR_TRANSFERENCIA Then
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed

                If strTipoNegocio = GSTR_ORDENFONDOS Then
                    VerTabTransferencia = Visibility.Collapsed
                    VerTabTransferenciaFondo = Visibility.Visible
                Else
                    VerTabTransferencia = Visibility.Visible
                    VerTabTransferenciaFondo = Visibility.Collapsed
                End If

                If strTipoNegocio = GSTR_ORDENFONDOS And logEsFondosOYD = False Then
                    VerBanco = Visibility.Collapsed
                    VerConsecutivosEgreso = Visibility.Collapsed
                    VerBancoFondos = Visibility.Visible
                    If logEsFondosUnity Then
                        VerBotonAgregarArchivo = Visibility.Collapsed
                    Else
                        VerBotonAgregarArchivo = Visibility.Visible
                    End If
                Else
                    VerConsecutivosEgreso = Visibility.Visible
                    VerBanco = Visibility.Visible
                    VerBancoFondos = Visibility.Collapsed

                    If logEsFondosOYD And strTipoNegocio = GSTR_ORDENFONDOS Then
                        TituloConsecutivoOrigen = "Consecutivo fondo"
                        TituloBancoOrigen = "Banco fondo"
                        TituloSaldoBancoOrigen = "Saldo banco fondo"
                        AgrupamientoFirmaOrigen = "cartera"
                        AgrupamientoFirmaDestino = "cartera"
                    Else
                        TituloConsecutivoOrigen = "Consecutivo"
                        TituloBancoOrigen = "Banco"
                        TituloSaldoBancoOrigen = "Saldo banco"
                        AgrupamientoFirmaOrigen = "firma"
                        AgrupamientoFirmaDestino = "firma"
                    End If
                End If

                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed
                VerConsecutivosReciboDestino = Visibility.Collapsed
                VerBancoFondosOYD = Visibility.Collapsed

            ElseIf strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS Then
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Visible
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed

                If logEsFondosOYD Then
                    VerConsecutivosEgreso = Visibility.Visible
                    VerConsecutivosNOTA = Visibility.Collapsed
                    VerConsecutivosReciboDestino = Visibility.Visible
                    VerBanco = Visibility.Visible
                    VerBancoFondosOYD = Visibility.Visible
                    VerBancoFondos = Visibility.Collapsed
                    TituloConsecutivoOrigen = "Consecutivo origen firma"
                    TituloConsecutivoDestino = "Consecutivo destino fondo"
                    TituloBancoOrigen = "Banco origen firma"
                    TituloSaldoBancoOrigen = "Saldo banco origen firma"
                    TituloBancoDestino = "Banco destino fondo"
                    TituloSaldoBancoDestino = "Saldo banco destino fondo"
                    AgrupamientoFirmaOrigen = "firma"
                    AgrupamientoFirmaDestino = "cartera"
                Else
                    VerConsecutivosEgreso = Visibility.Collapsed
                    VerConsecutivosNOTA = Visibility.Visible
                    VerConsecutivosReciboDestino = Visibility.Collapsed
                    VerBancoFondosOYD = Visibility.Collapsed
                    VerBancoFondos = Visibility.Visible
                    TituloConsecutivoOrigen = "Consecutivo"
                    TituloBancoOrigen = "Banco"
                    TituloSaldoBancoOrigen = "Saldo banco"
                    AgrupamientoFirmaOrigen = "firma"
                    AgrupamientoFirmaDestino = "firma"
                    VerBanco = Visibility.Visible
                End If

                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed

            ElseIf strTipoPagoPlus = GSTR_TRASLADOFONDOS Then
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Visible
                VerTabInternos = Visibility.Collapsed
                VerConsecutivosEgreso = Visibility.Visible
                VerConsecutivosNOTA = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                If logEsFondosOYD Then
                    VerBanco = Visibility.Visible
                Else
                    VerBanco = Visibility.Collapsed
                    VerConsecutivosEgreso = Visibility.Collapsed
                End If
                VerConsecutivosRecibo = Visibility.Collapsed

                If logEsFondosOYD Then
                    VerConsecutivosReciboDestino = Visibility.Visible
                    VerBancoFondosOYD = Visibility.Visible
                    VerBancoFondos = Visibility.Collapsed
                    TituloConsecutivoOrigen = "Consecutivo origen fondo"
                    TituloConsecutivoDestino = "Consecutivo destino fondo"
                    TituloBancoOrigen = "Banco origen fondo"
                    TituloSaldoBancoOrigen = "Saldo banco origen fondo"
                    TituloBancoDestino = "Banco destino fondo"
                    TituloSaldoBancoDestino = "Saldo banco destino fondo"
                    AgrupamientoFirmaOrigen = "cartera"
                    AgrupamientoFirmaDestino = "cartera"
                    '  VerBancoFondosOYD = Visibility.Collapsed
                    VerBancoFondosDestino = Visibility.Collapsed
                Else
                    VerConsecutivosReciboDestino = Visibility.Collapsed
                    VerBancoFondosOYD = Visibility.Collapsed
                    VerBancoFondos = Visibility.Visible
                    TituloConsecutivoOrigen = "Consecutivo"
                    TituloBancoOrigen = "Banco"
                    TituloSaldoBancoOrigen = "Saldo banco"
                    AgrupamientoFirmaOrigen = "firma"
                    AgrupamientoFirmaDestino = "firma"
                    VerBancoFondosDestino = Visibility.Visible
                    VerBanco = Visibility.Collapsed
                    VerConsecutivosEgreso = Visibility.Collapsed
                    VerConsecutivosNOTA = Visibility.Collapsed
                End If
                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed

            ElseIf strTipoPagoPlus = GSTR_INTERNOS Then
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Visible
                VerConsecutivosEgreso = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Visible
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                VerBanco = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed
                VerConsecutivosReciboDestino = Visibility.Collapsed
                VerBancoFondosOYD = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed
                TituloConsecutivoOrigen = "Consecutivo"
                TituloBancoOrigen = "Banco"
                TituloSaldoBancoOrigen = "Saldo banco"
                AgrupamientoFirmaOrigen = "firma"
                AgrupamientoFirmaDestino = "firma"
            ElseIf strTipoPagoPlus = GSTR_CHEQUE_GERENCIA Then
                VerConsecutivosEgreso = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed

                If strTipoNegocio = GSTR_ORDENFONDOS Then
                    VerTabCheque = Visibility.Collapsed
                    VerTabChequeFondo = Visibility.Visible
                Else
                    VerTabCheque = Visibility.Visible
                    VerTabChequeFondo = Visibility.Collapsed
                End If

                If strTipoNegocio = GSTR_ORDENFONDOS And logEsFondosOYD = False Then
                    VerBanco = Visibility.Collapsed
                    VerBancoFondos = Visibility.Visible
                    If logEsFondosUnity Then
                        VerBotonAgregarArchivo = Visibility.Collapsed
                    Else
                        VerBotonAgregarArchivo = Visibility.Visible
                    End If
                    VerConsecutivosNOTA = Visibility.Collapsed
                    TituloConsecutivoOrigen = "Consecutivo"
                    TituloBancoOrigen = "Banco"
                    TituloSaldoBancoOrigen = "Saldo banco"
                    AgrupamientoFirmaOrigen = "firma"
                    AgrupamientoFirmaDestino = "firma"
                Else
                    VerBancoFondos = Visibility.Collapsed
                    VerConsecutivosNOTA = Visibility.Visible
                    VerBanco = Visibility.Visible
                    If logEsFondosOYD And strTipoNegocio = GSTR_ORDENFONDOS Then
                        TituloConsecutivoOrigen = "Consecutivo fondo"
                        TituloBancoOrigen = "Banco fondo"
                        TituloSaldoBancoOrigen = "Saldo banco fondo"
                        AgrupamientoFirmaOrigen = "cartera"
                        AgrupamientoFirmaDestino = "cartera"
                    Else
                        TituloConsecutivoOrigen = "Consecutivo"
                        TituloBancoOrigen = "Banco"
                        TituloSaldoBancoOrigen = "Saldo banco"
                        AgrupamientoFirmaOrigen = "firma"
                        AgrupamientoFirmaDestino = "firma"
                    End If

                End If

                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed
                VerConsecutivosReciboDestino = Visibility.Collapsed
                VerBancoFondosOYD = Visibility.Collapsed

            ElseIf strTipoPagoPlus = GSTR_BLOQUEO_RECURSOS Then
                VerConsecutivosEgreso = Visibility.Collapsed
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerTabBloqueos = Visibility.Visible
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerTabRecibos = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Collapsed
                VerBanco = Visibility.Collapsed
                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed
                VerConsecutivosReciboDestino = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed
                VerBancoFondosOYD = Visibility.Collapsed
                TituloConsecutivoOrigen = "Consecutivo"
                TituloBancoOrigen = "Banco"
                TituloSaldoBancoOrigen = "Saldo banco"
                AgrupamientoFirmaOrigen = "firma"
                AgrupamientoFirmaDestino = "firma"
            ElseIf strTipoPagoPlus = GSTR_OYD Then
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Visible
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerBanco = Visibility.Visible
                VerConsecutivosRecibo = Visibility.Collapsed

                If _strTipoNegocio = GSTR_ORDENFONDOS And logEsFondosOYD = False And _strEstado = GSTR_PENDIENTE_Plus Then
                    If DiccionarioCombosOYDPlus.ContainsKey("CTA_FIRMA_OYD_FONDOS") Then
                        IdBanco = DiccionarioCombosOYDPlus("CTA_FIRMA_OYD_FONDOS").FirstOrDefault.Retorno
                        DescripcionBanco = DiccionarioCombosOYDPlus("CTA_FIRMA_OYD_FONDOS").FirstOrDefault.Descripcion
                    End If
                End If

                If logEsFondosOYD Then
                    VerConsecutivosNOTA = Visibility.Collapsed
                    VerConsecutivosNOTADestino = Visibility.Collapsed
                    VerConsecutivosEgreso = Visibility.Visible
                    VerBancoFondosOYD = Visibility.Visible
                    VerBancoFondos = Visibility.Collapsed
                    VerConsecutivosReciboDestino = Visibility.Visible
                    TituloConsecutivoOrigen = "Consecutivo origen fondo"
                    TituloConsecutivoDestino = "Consecutivo destino firma"
                    TituloBancoOrigen = "Banco origen fondo"
                    TituloSaldoBancoOrigen = "Saldo banco origen firma"
                    TituloBancoDestino = "Banco destino fondo"
                    TituloSaldoBancoDestino = "Saldo banco destino firma"
                    AgrupamientoFirmaOrigen = "cartera"
                    AgrupamientoFirmaDestino = "firma"
                Else
                    VerConsecutivosNOTA = Visibility.Visible
                    VerConsecutivosEgreso = Visibility.Collapsed
                    VerBancoFondosOYD = Visibility.Collapsed
                    VerBancoFondos = Visibility.Visible
                    VerConsecutivosNOTADestino = Visibility.Collapsed
                    VerConsecutivosReciboDestino = Visibility.Collapsed
                    TituloConsecutivoOrigen = "Consecutivo"
                    TituloBancoOrigen = "Banco"
                    TituloSaldoBancoOrigen = "Saldo banco"
                    AgrupamientoFirmaOrigen = "firma"
                    AgrupamientoFirmaDestino = "firma"
                    If logEsFondosUnity Then
                        VerBotonAgregarArchivo = Visibility.Collapsed
                    Else
                        VerBotonAgregarArchivo = Visibility.Visible
                    End If
                End If
                VerConsecutivosEgresoDestino = Visibility.Collapsed

            ElseIf strTipoPagoPlus = GSTR_OPERACIONES_ESPECIALES Then
                VerConsecutivosEgreso = Visibility.Collapsed
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Visible
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerBanco = Visibility.Collapsed
                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed
                VerConsecutivosReciboDestino = Visibility.Collapsed
                VerBancoFondosOYD = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed
                TituloConsecutivoOrigen = "Consecutivo"
                TituloBancoOrigen = "Banco"
                TituloSaldoBancoOrigen = "Saldo banco"
                AgrupamientoFirmaOrigen = "firma"
                AgrupamientoFirmaDestino = "firma"
            ElseIf strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION Then
                VerConsecutivosEgreso = Visibility.Collapsed
                VerTabCheque = Visibility.Collapsed
                VerTabChequeFondo = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Visible
                VerBanco = Visibility.Collapsed

                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed

                VerConsecutivosRecibo = Visibility.Collapsed

                If logValidarCamposObligatorios = False Then
                    If logEsFondosOYD Then
                        VerBancoFondosOYD = Visibility.Visible
                        VerBanco = Visibility.Collapsed
                    Else
                        VerBancoFondosOYD = Visibility.Collapsed
                        VerBanco = Visibility.Visible
                    End If
                Else
                    VerBancoFondosOYD = Visibility.Collapsed
                    VerBanco = Visibility.Collapsed
                End If

                If logEsFondosOYD Then
                    TituloConsecutivoOrigen = "Consecutivo fondo"
                    TituloBancoOrigen = "Banco fondo"
                    TituloSaldoBancoOrigen = "Saldo banco fondo"
                    VerConsecutivosReciboDestino = Visibility.Visible
                    AgrupamientoFirmaOrigen = "cartera"
                    AgrupamientoFirmaDestino = "cartera"
                    VerBancoFondos = Visibility.Collapsed
                Else
                    TituloConsecutivoOrigen = "Consecutivo"
                    TituloBancoOrigen = "Banco"
                    TituloSaldoBancoOrigen = "Saldo banco"
                    AgrupamientoFirmaOrigen = "firma"
                    AgrupamientoFirmaDestino = "firma"
                    VerConsecutivosReciboDestino = Visibility.Collapsed
                    VerBancoFondos = Visibility.Visible
                    VerBanco = Visibility.Collapsed
                End If
            ElseIf strTipoPagoPlus = GSTR_ORDENFONDOS_CANCELACION Then

                If logEsFondosUnity Then
                    VerConsecutivosEgreso = Visibility.Collapsed
                Else
                    VerConsecutivosEgreso = Visibility.Visible
                End If

                VerTabCheque = Visibility.Visible
                VerTabChequeFondo = Visibility.Collapsed
                VerTabTransferencia = Visibility.Collapsed
                VerTabTransferenciaFondo = Visibility.Collapsed
                VerTabCarterasColectivas = Visibility.Collapsed
                VerTabTrasladoFondos = Visibility.Collapsed
                VerTabInternos = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Collapsed
                VerTabBloqueos = Visibility.Collapsed
                VerTabOYD = Visibility.Collapsed
                VerTabOperacionesEspeciales = Visibility.Collapsed
                VerTabAdicionConstitucion = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed

                If logEsFondosOYD Then
                    VerBancoFondos = Visibility.Collapsed
                    VerConsecutivosNOTADestino = Visibility.Collapsed
                    VerConsecutivosEgresoDestino = Visibility.Collapsed
                    VerConsecutivosReciboDestino = Visibility.Collapsed
                    VerBancoFondosOYD = Visibility.Collapsed
                    TituloConsecutivoOrigen = "Consecutivo"
                    TituloBancoOrigen = "Banco"
                    TituloSaldoBancoOrigen = "Saldo banco"
                    AgrupamientoFirmaOrigen = "firma"
                    AgrupamientoFirmaDestino = "firma"
                    VerBanco = Visibility.Collapsed
                Else
                    VerBancoFondos = Visibility.Visible
                    If Not IsNothing(ListaResultadosDocumentos) Then
                        If ListaResultadosDocumentos.Count > 0 Then
                            If ListaResultadosDocumentos.Where(Function(x) x.strFormaPago = GSTR_OYD).Count > 0 Then
                                VerConsecutivosEgreso = Visibility.Collapsed
                                VerConsecutivosNOTA = Visibility.Visible
                                VerConsecutivosEgreso = Visibility.Collapsed
                                VerBancoFondosOYD = Visibility.Collapsed
                                VerBancoFondos = Visibility.Visible
                                VerConsecutivosNOTADestino = Visibility.Collapsed
                                VerConsecutivosReciboDestino = Visibility.Collapsed
                                TituloConsecutivoOrigen = "Consecutivo"
                                TituloBancoOrigen = "Banco"
                                TituloSaldoBancoOrigen = "Saldo banco"
                                AgrupamientoFirmaOrigen = "firma"
                                VerBanco = Visibility.Visible
                            End If
                        End If
                    End If
                End If

                VerBanco = Visibility.Collapsed

            ElseIf strTipoNegocio = GSTR_ORDENRECIBO Then
                TituloConsecutivoOrigen = "Consecutivo"
                TituloBancoOrigen = "Banco"
                TituloSaldoBancoOrigen = "Saldo banco"
                AgrupamientoFirmaOrigen = "firma"
                AgrupamientoFirmaDestino = "firma"
                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed
                VerConsecutivosReciboDestino = Visibility.Collapsed
                VerBancoFondosOYD = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Collapsed
                VerConsecutivosEgreso = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Visible
            Else
                VerConsecutivosEgreso = Visibility.Collapsed
                VerConsecutivosEgresoDestino = Visibility.Collapsed
                VerConsecutivosNOTA = Visibility.Collapsed
                VerConsecutivosNOTADestino = Visibility.Collapsed
                VerConsecutivosRecibo = Visibility.Collapsed
                VerConsecutivosReciboDestino = Visibility.Collapsed
                VerBanco = Visibility.Collapsed
                VerBancoFondos = Visibility.Collapsed
                VerBancoFondosOYD = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ocultar los campos",
                                    Me.ToString(), "OcultarCamposConsecutivosBancos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub VerDetalles(Optional strOpcion As String = "")
        Try
            If Not IsNothing(SelectedDocumentosRecibo) Then
                If SelectedDocumentosRecibo.lngIDEncabezado > 0 Then
                    dcProxy.TesoreroDetalleRecibos.Clear()
                    IsBusy = True
                    If ItemReceptor <> GSTR_TODOS Then
                        dcProxy.Load(dcProxy.TesoreroListarDetallesReciboQuery(ItemReceptor, strTipoNegocio, strEstado, SelectedDocumentosRecibo.lngIDEncabezado, Fecha, Integer.Parse(strSUCURSAL_PLUS), Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarDocumentosDetalleRecibo, strOpcion)
                    Else
                        dcProxy.Load(dcProxy.TesoreroListarDetallesReciboQuery(GSTR_ID_TODOS, strTipoNegocio, strEstado, SelectedDocumentosRecibo.lngIDEncabezado, Fecha, Integer.Parse(strSUCURSAL_PLUS), Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarDocumentosDetalleRecibo, strOpcion)
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ver los detalles", Me.ToString(), "VerDetalles", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSRUTA) As String
        Return String.Format("[{0}]", pintTipoCampo.ToString)
    End Function
    Public Sub MostrarCartaGenerencia()
        Try
            If Not IsNothing(SelectedDocumentos) Then
                If Not String.IsNullOrEmpty(SelectedDocumentos.urlCarta) Then
                    IsBusy = True
                    Dim strRutaReporte As String = STR_RUTAREPORTECARTA
                    Dim strRutaServidor As String = Program.ServidorReportes
                    Dim strNombreReporte As String = Program.CarpetaReportes
                    Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString

                    If Right(strRutaServidor, 1) = "/" Then
                        strRutaServidor = Left(strRutaServidor, Len(strRutaServidor) - 1)
                    End If

                    If Left(strNombreReporte, 1) <> "/" Then
                        strNombreReporte = "/" & strNombreReporte
                    End If

                    If Right(strNombreReporte, 1) <> "/" Then
                        strNombreReporte = strNombreReporte & "/"
                    End If

                    strNombreReporte = strNombreReporte & "Documento desde html"

                    strRutaReporte = strRutaReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.SERVIDOR), strRutaServidor)
                    strRutaReporte = strRutaReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.REPORTE), strNombreReporte)
                    strRutaReporte = strRutaReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.TEXTO), SelectedDocumentos.urlCarta)
                    strRutaReporte = strRutaReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.CULTURA), Program.CulturaReportes)

                    Program.VisorArchivosWeb_CargarURL(strRutaReporte)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ver los detalles", Me.ToString(), "MostrarCartaGenerencia", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub DesbloquerDetalles()
        Try
            If Not IsNothing(ListaResultadosDocumentos) Then
                For Each li In ListaResultadosDocumentos
                    dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(li.lngID, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf TerminoDesbloqueoTesorero, String.Empty)
                Next
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al desbloquear los detalles", Me.ToString(), "DesbloquerDetalles", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub CerrarDetallesRecibos()
        Try
            objWppDetallesRecibo.Close()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar los detalles de recibo", Me.ToString(), "CerrarDetallesRecibos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub RechazarDocumentos()

        Try
            If IsNothing(ListaResultadosDocumentos) And IsNothing(ListaResultadosDocumentosRecibo) Then
                mostrarMensaje("Para rechazar documento(s) debe seleccionar mínimo 1 registro.", "Rechazar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                    If ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count > 0 Then
                        mostrarMensajePregunta("¿Está Seguro que desea Rechazar Documento(s)?",
                                               Program.TituloSistema,
                                               "RECHAZARORDEN",
                                               AddressOf TerminoPreguntarConfirmacionRechazar,
                                               False, String.Empty, False, True, True, False)

                    End If
                Else
                    If ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count > 0 Then
                        mostrarMensajePregunta("¿Está Seguro que desea Rechazar Documento(s)?",
                                               Program.TituloSistema,
                                               "RECHAZARORDEN",
                                               AddressOf TerminoPreguntarConfirmacionRechazar,
                                               False, String.Empty, False, True, True, False)

                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Rechazar Documento", Me.ToString(), "RechazarDocumentos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Function ValidarGeneracionDocumentosAgregarArchivo() As Boolean
        Try
            If IsNothing(Fecha) Then
                mostrarMensaje("Debe de seleccionar la fecha para poder generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If Fecha.Date > dtmFechaServidor.Date Then
                mostrarMensaje("La fecha para generar los documentos no puede ser mayor a la fecha del sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            Dim logRetorno As Boolean = True
            Dim strNombreArchivo As String = String.Empty

            If strTipoNegocio = GSTR_ORDENFONDOS Then
                If strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION Then
                    If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                        If _ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count = 0 Then
                            IsBusy = False
                            mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logRetorno = False
                        Else
                            Return True
                        End If
                    Else
                        IsBusy = False
                        mostrarMensaje("No se puede Generar documentos ya que no existe ningun registro a generar", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    End If
                Else
                    If Not IsNothing(_ListaResultadosDocumentos) Then
                        If _ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count = 0 Then
                            IsBusy = False
                            mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logRetorno = False
                        ElseIf strTipoPagoPlus = GSTR_OYD And String.IsNullOrEmpty(strConsecutivoNOTAS_PLUS_Origen) Then
                            IsBusy = False
                            mostrarMensaje("Para generar documentos se debe seleccionar un consecutivo.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logRetorno = False
                        ElseIf strEstado = GSTR_PENDIENTE_Plus And strTipoPagoPlus = GSTR_OYD And (IsNothing(IdBanco) Or IsNothing(IdBancoFondo)) Then
                            IsBusy = False
                            mostrarMensaje("Para generar documentos se debe seleccionar los bancos.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logRetorno = False
                        ElseIf strEstado = GSTR_PENDIENTE_Plus And (strTipoPagoPlus = GSTR_TRANSFERENCIA And IsNothing(IdBancoFondo) Or
                                                                strTipoPagoPlus = GSTR_CHEQUE And IsNothing(IdBancoFondo) Or strTipoPagoPlus = GSTR_CHEQUE_GERENCIA And IsNothing(IdBancoFondo)) Then
                            IsBusy = False
                            mostrarMensaje("Para generar documentos se debe seleccionar el banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logRetorno = False
                        ElseIf strEstado = GSTR_PENDIENTE_Plus And strTipoPagoPlus = GSTR_ORDENFONDOS_CANCELACION And IsNothing(IdBancoFondo) Then
                            IsBusy = False
                            mostrarMensaje("Para generar documentos se debe seleccionar el banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logRetorno = False
                        Else
                            Return True
                        End If
                    Else
                        IsBusy = False
                        mostrarMensaje("No se puede Generar documentos ya que no existe ningun registro a generar", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    End If
                End If
            ElseIf strTipoNegocio = GSTR_ORDENGIRO Then
                If Not IsNothing(_ListaResultadosDocumentos) Then
                    If _ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count = 0 Then
                        mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    ElseIf strTipoPagoPlus <> GSTR_BLOQUEO_RECURSOS And
                        ((VerConsecutivosEgreso = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoCE_PLUS_Origen)) Or
                         (VerConsecutivosNOTA = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoNOTAS_PLUS_Origen))) Then
                        IsBusy = False
                        mostrarMensaje("Para generar documentos se debe seleccionar un consecutivo.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    ElseIf strEstado = GSTR_PENDIENTE_Plus And strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS And (IsNothing(IdBanco) Or IsNothing(IdBancoFondo)) Then
                        IsBusy = False
                        mostrarMensaje("Para generar documentos se debe seleccionar los bancos.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    ElseIf strTipoPagoPlus <> GSTR_BLOQUEO_RECURSOS And IsNothing(IdBanco) And (_ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strFormaPago <> GSTR_INTERNOS).Count > 0) Then
                        IsBusy = False
                        mostrarMensaje("Para generar documentos se debe seleccionar un banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    Else
                        Return True
                    End If
                Else
                    IsBusy = False
                    mostrarMensaje("No se puede Generar documentos ya que no existe  ningun registro a generar", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logRetorno = False
                End If
            ElseIf strTipoNegocio = GSTR_ORDENRECIBO Then
                If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                    If _ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count = 0 Then
                        IsBusy = False
                        mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    ElseIf String.IsNullOrEmpty(strConsecutivoRC_PLUS_Origen) Then
                        IsBusy = False
                        mostrarMensaje("¡Para generar documentos se debe seleccionar un consecutivo, y que existan documentos a generar!", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    ElseIf String.IsNullOrEmpty(strSUCURSAL_PLUS) Then
                        IsBusy = False
                        mostrarMensaje("¡Para generar documentos se debe de existir una sucursal, y que existan documentos a generar!", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    ElseIf IsNothing(IdBanco) And logHayCheque Then
                        IsBusy = False
                        mostrarMensaje("Para generar documentos se debe seleccionar el banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logRetorno = False
                    Else
                        Return True
                    End If
                Else
                    IsBusy = False
                    mostrarMensaje("No se puede Generar documentos ya que no existe ningun registro a generar", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logRetorno = False
                End If
            End If

            If logRetorno Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar el archivo a crear.", Me.ToString(), "ValidarGeneracionDocumentos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return False
        End Try
    End Function

    Public Async Sub ValidarGeneracionDocumentos()
        Try
            IsBusy = True
            If IsNothing(Fecha) Then
                mostrarMensaje("Debe de seleccionar la fecha para poder generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If Fecha.Date > dtmFechaServidor.Date Then
                mostrarMensaje("La fecha para generar los documentos no puede ser mayor a la fecha del sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            Dim strNombreArchivo As String = String.Empty
            If strTipoNegocio = GSTR_ORDENFONDOS Then
                If strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION Then
                    If Not IsNothing(ListaResultadosDocumentosRecibo) Then
                        If ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar And (i.strValorEstado = "V" Or i.strValorEstado = "I")).Count > 0 Then
                            mostrarMensaje("No se puede Generar documentos ya que se han seleccionado registros con un estado no valido para la generación.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                Else
                    If Not IsNothing(_ListaResultadosDocumentos) Then
                        If _ListaResultadosDocumentos.Where(Function(i) i.Generar And (i.strValorEstado = "V" Or i.strValorEstado = "I")).Count > 0 Then
                            mostrarMensaje("No se puede Generar documentos ya que se han seleccionado registros con un estado no valido para la generación.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                End If
            ElseIf strTipoNegocio = GSTR_ORDENGIRO Then
                If Not IsNothing(_ListaResultadosDocumentos) Then
                    If _ListaResultadosDocumentos.Where(Function(i) i.Generar And (i.strValorEstado = "V" Or i.strValorEstado = "I")).Count > 0 Then
                        mostrarMensaje("No se puede Generar documentos ya que se han seleccionado registros con un estado no valido para la generación.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            Else
                If Not IsNothing(ListaResultadosDocumentosRecibo) Then
                    If ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar And (i.strValorEstado = "V" Or i.strValorEstado = "I")).Count > 0 Then
                        mostrarMensaje("No se puede Generar documentos ya que se han seleccionado registros con un estado no valido para la generación.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            End If

            If strTipoNegocio = GSTR_ORDENFONDOS Then
                If strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION Then
                    If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                        If logEsFondosOYD Then
                            If _ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count = 0 Then
                                mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                            ElseIf strTipoPagoPlus <> GSTR_BLOQUEO_RECURSOS And
                                ((VerConsecutivosEgreso = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoCE_PLUS_Origen)) Or
                                 (VerConsecutivosNOTA = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoNOTAS_PLUS_Origen)) Or
                                 (VerConsecutivosNOTADestino = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoNOTAS_PLUS_Destino)) Or
                                 (VerConsecutivosEgresoDestino = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoCE_PLUS_Destino)) Or
                                 (VerConsecutivosReciboDestino = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoRC_PLUS_Destino))) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar los consecutivos.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            ElseIf (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION _
                                    Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION) _
                                And IsNothing(IdBanco_FondosOYD) _
                                And VerBancoFondosOYD = Visibility.Visible Then

                                If logValidarCamposObligatorios And logHayCheque Then
                                    IsBusy = False
                                    mostrarMensaje("Para generar documentos se debe seleccionar un banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ElseIf logValidarCamposObligatorios = False Then
                                    IsBusy = False
                                    mostrarMensaje("Para generar documentos se debe seleccionar un banco. Recuerde que el banco solo se tomara en cuenta para los registros que no tienen banco destino desde el registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Else
                                    Await GenerarDocumentos(String.Empty)
                                End If
                            Else
                                Await GenerarDocumentos(String.Empty)
                            End If
                        Else
                            If _ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count = 0 Then
                                IsBusy = False
                                mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else
                                Await GenerarDocumentos(String.Empty)
                            End If
                        End If
                    Else
                        IsBusy = False
                        mostrarMensaje("No se puede Generar documentos ya que no existe ningun registro a generar", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    If Not IsNothing(_ListaResultadosDocumentos) Then
                        If logEsFondosOYD Then
                            If _ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count = 0 Then
                                mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                            ElseIf strTipoPagoPlus <> GSTR_BLOQUEO_RECURSOS And
                                ((VerConsecutivosEgreso = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoCE_PLUS_Origen)) Or
                                 (VerConsecutivosNOTA = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoNOTAS_PLUS_Origen)) Or
                                 (VerConsecutivosNOTADestino = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoNOTAS_PLUS_Destino)) Or
                                 (VerConsecutivosEgresoDestino = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoCE_PLUS_Destino)) Or
                                 (VerConsecutivosReciboDestino = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoRC_PLUS_Destino))) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar los consecutivos.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            ElseIf (strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS Or strTipoPagoPlus = GSTR_OYD) And (IsNothing(IdBanco) Or IsNothing(IdBanco_FondosOYD)) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar los bancos.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            ElseIf strTipoPagoPlus <> GSTR_BLOQUEO_RECURSOS _
                                And strTipoPagoPlus <> GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION _
                                And strTipoPagoPlus <> GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION _
                                And IsNothing(IdBanco) _
                                And (_ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strFormaPago <> GSTR_INTERNOS).Count > 0) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar un banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else
                                Await GenerarDocumentos(String.Empty)
                            End If
                        Else
                            If _ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count = 0 Then
                                IsBusy = False
                                mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            ElseIf strTipoPagoPlus = GSTR_OYD And (String.IsNullOrEmpty(strConsecutivoCE_PLUS_Origen) And String.IsNullOrEmpty(strConsecutivoNOTAS_PLUS_Origen)) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar un consecutivo.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            ElseIf strTipoPagoPlus = GSTR_OYD And (IsNothing(IdBanco) Or IsNothing(IdBancoFondo)) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar los bancos.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            ElseIf (strTipoPagoPlus = GSTR_TRANSFERENCIA And IsNothing(IdBancoFondo)) Or (strTipoPagoPlus = GSTR_CHEQUE And IsNothing(IdBancoFondo) Or (strTipoPagoPlus = GSTR_CHEQUE_GERENCIA And IsNothing(IdBancoFondo))) And (logEsFondosUnity) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar el banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            ElseIf strTipoPagoPlus = GSTR_ORDENFONDOS_CANCELACION And IsNothing(IdBancoFondo) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar el banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            ElseIf strTipoPagoPlus = GSTR_ORDENFONDOS_CANCELACION And logEsFondosUnity Then
                                If Not IsNothing(ListaResultadosDocumentos) Then
                                    If ListaResultadosDocumentos.Count > 0 Then
                                        If ListaResultadosDocumentos.Where(Function(x) x.strFormaPago = GSTR_OYD).Count > 0 And IsNothing(IdBanco) Then
                                            IsBusy = False
                                            mostrarMensaje("Para generar documentos se debe seleccionar el banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        Else
                                            Await GenerarDocumentos(String.Empty)
                                        End If
                                    End If
                                End If
                            ElseIf strTipoPagoPlus = GSTR_TRASLADOFONDOS And strEstado = "P" And (IsNothing(IdBancoFondo) Or IsNothing(IdBancoFondoDestino)) Then
                                IsBusy = False
                                mostrarMensaje("Para generar documentos se debe seleccionar el banco y banco destino.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else

                                If strTipoPagoPlus = GSTR_OPERACIONES_ESPECIALES Then
                                    Await GenerarDocumentos(String.Empty)
                                Else
                                    Await GenerarDocumentos(String.Empty)
                                End If
                            End If
                        End If
                    Else
                        IsBusy = False
                        mostrarMensaje("No se puede Generar documentos ya que no existe ningun registro a generar", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            ElseIf strTipoNegocio = GSTR_ORDENGIRO Then
                If Not IsNothing(_ListaResultadosDocumentos) Then
                    If _ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count = 0 Then
                        IsBusy = False
                        mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf strTipoPagoPlus <> GSTR_BLOQUEO_RECURSOS And
                        ((VerConsecutivosEgreso = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoCE_PLUS_Origen)) Or
                         (VerConsecutivosNOTA = Visibility.Visible And String.IsNullOrEmpty(strConsecutivoNOTAS_PLUS_Origen))) Then
                        IsBusy = False
                        mostrarMensaje("Para generar documentos se debe seleccionar un consecutivo.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf (logEsFondosOYD And strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS And (IsNothing(IdBanco) Or IsNothing(IdBanco_FondosOYD))) _
                        Or (logEsFondosOYD = False And strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS And (IsNothing(IdBanco) Or IsNothing(IdBancoFondo))) Then
                        IsBusy = False
                        mostrarMensaje("Para generar documentos se debe seleccionar los bancos.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf strTipoPagoPlus <> GSTR_BLOQUEO_RECURSOS And IsNothing(IdBanco) And (_ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strFormaPago <> GSTR_INTERNOS).Count > 0) Then
                        IsBusy = False
                        mostrarMensaje("Para generar documentos se debe seleccionar un banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        Await GenerarDocumentos(String.Empty)
                    End If
                Else
                    IsBusy = False
                    mostrarMensaje("No se puede Generar documentos ya que no existe  ningun registro a generar", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            ElseIf strTipoNegocio = GSTR_ORDENRECIBO Then
                If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                    If _ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count = 0 Then
                        IsBusy = False
                        mostrarMensaje("No se puede Generar documentos ya que no se ha seleccionado ningun registro.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf String.IsNullOrEmpty(strConsecutivoRC_PLUS_Origen) Then
                        IsBusy = False
                        mostrarMensaje("¡Para generar documentos se debe seleccionar un consecutivo, y que existan documentos a generar!", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf String.IsNullOrEmpty(strSUCURSAL_PLUS) Then
                        IsBusy = False
                        mostrarMensaje("¡Para generar documentos se debe de existir una sucursal, y que existan documentos a generar!", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf IsNothing(IdBanco) And logHayCheque Then
                        IsBusy = False
                        mostrarMensaje("Para generar documentos se debe seleccionar el banco.", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        Await GenerarDocumentos(String.Empty)
                    End If
                Else
                    IsBusy = False
                    mostrarMensaje("No se puede Generar documentos ya que no existe ningun registro a generar", "Generar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar el archivo a crear.", Me.ToString(), "ValidarGeneracionDocumentos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Async Function GenerarDocumentos(ByVal pstrNombreArchivoGuardado As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Dim strRegistros As String = ""

        Try
            If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                    For Each li In _ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar)
                        strRegistros = strRegistros & li.lngIDEncabezado & ","
                    Next

                    If Not String.IsNullOrEmpty(strRegistros) Then
                        If strRegistros.Substring(Len(strRegistros) - 1, 1) = "," Then
                            strRegistros = strRegistros.Substring(0, Len(strRegistros) - 1)
                        End If
                    End If
                End If
            Else
                If Not IsNothing(_ListaResultadosDocumentos) Then
                    For Each li In _ListaResultadosDocumentos.Where(Function(i) i.Generar)
                        strRegistros = strRegistros & li.lngID & ","
                    Next

                    If Not String.IsNullOrEmpty(strRegistros) Then
                        If strRegistros.Substring(Len(strRegistros) - 1, 1) = "," Then
                            strRegistros = strRegistros.Substring(0, Len(strRegistros) - 1)
                        End If
                    End If
                End If
            End If

            If Not String.IsNullOrEmpty(strRegistros) Then
                Dim objRespuesta As LoadOperation(Of tblRespuestaValidacionesTesoreria)
                Dim objProxy As OYDPLUSTesoreriaDomainContext
                Dim strConsecutivoCE As String = String.Empty
                Dim strConsecutivoRC As String = String.Empty
                Dim strConsecutivoNota As String = String.Empty
                Dim intIDBanco As Nullable(Of Integer) = Nothing
                Dim intIDBancoFondo As Nullable(Of Integer) = Nothing
                Dim intIDBancoDestinoFondo As Nullable(Of Integer) = Nothing
                Dim strMensajeErrores As String = String.Empty

                intIDBanco = IdBanco
                intIDBancoFondo = IdBancoFondo
                intIDBancoDestinoFondo = IdBancoFondoDestino

                strConsecutivoCE = strConsecutivoCE_PLUS_Origen
                strConsecutivoNota = strConsecutivoNOTAS_PLUS_Origen
                strConsecutivoRC = strConsecutivoRC_PLUS_Origen

                If logEsFondosOYD Then
                    If (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION Or strTipoPagoPlus = GSTR_TRASLADOFONDOS Or strTipoPagoPlus = GSTR_OYD)) _
                    Or (strTipoNegocio = GSTR_ORDENGIRO And strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS) Then
                        strConsecutivoRC = strConsecutivoRC_PLUS_Destino
                    End If

                    If (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_TRASLADOFONDOS Or strTipoPagoPlus = GSTR_OYD)) _
                    Or (strTipoNegocio = GSTR_ORDENGIRO And strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS) Then
                        intIDBancoFondo = IdBancoFondo
                        intIDBancoDestinoFondo = IdBanco_FondosOYD
                    End If

                    If strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION) Then
                        intIDBancoFondo = IdBanco_FondosOYD
                    End If
                End If

                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    objProxy = New OYDPLUSTesoreriaDomainContext()
                Else
                    objProxy = New OYDPLUSTesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                End If

                DirectCast(objProxy.DomainClient, WebDomainClient(Of OYDPLUSTesoreriaDomainContext.IOYDPLUSTesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

                objRespuesta = Await objProxy.Load(objProxy.Tesorero_GenerarRegistrosQuery(strTipoNegocio, strTipoPagoPlus, strRegistros, strConsecutivoNota, strConsecutivoCE, strConsecutivoRC, intIDBanco, intIDBancoFondo, intIDBancoDestinoFondo, Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRespuesta Is Nothing Then
                    If objRespuesta.HasError = False Then

                        Dim logMostrarMensajeExitoso As Boolean = True

                        If objRespuesta.Entities.Count > 0 Then
                            For Each li In objRespuesta.Entities
                                If logEsFondosUnity And li.logFinonset Then
                                    strMensajeErrores = String.Format("{0}{1}{2}", strMensajeErrores, vbCrLf, "- " + li.Mensaje)
                                Else
                                    If li.Exitoso = False Then
                                        If String.IsNullOrEmpty(strMensajeErrores) Then
                                            strMensajeErrores = li.Mensaje
                                        Else
                                            strMensajeErrores = String.Format("{0}{1}{2}", strMensajeErrores, vbCrLf, li.Mensaje)
                                        End If
                                    ElseIf li.logFinonset = True Then
                                        logMostrarMensajeExitoso = False
                                        mostrarMensaje(li.Mensaje, "", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                                    End If
                                End If

                                If Not String.IsNullOrEmpty(strMensajeErrores) Then
                                    logMostrarMensajeExitoso = False
                                    mostrarMensaje(strMensajeErrores, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                            Next
                        Else
                            IsBusy = False
                        End If

                        If logMostrarMensajeExitoso Then
                            mostrarMensaje("Se actualizaron los registros exitosamente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                            LimpiarCampos()
                        End If

                        logRealizarConsultaSeleccionarTodos = False
                        SeleccionarTodos = False
                        logRealizarConsultaSeleccionarTodos = True
                        ConsultarRegistrosTesorero(String.Empty)

                    Else
                        IsBusy = False
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Generar Documento(s)",
                                                    Me.ToString(), "GenerarDocumentos", Application.Current.ToString(), Program.Maquina, objRespuesta.Error)
                    End If
                End If
            Else
                mostrarMensaje("No se pueden generar los documentos ya que no se ha seleccionado ningun registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            Return logResultado
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar documentos", Me.ToString(),
                                                         "GenerarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        Finally
            IsBusy = False
        End Try
    End Function

    Public Sub ConsultarDocumentos()
        Try
            IsBusy = True
            SeleccionarTodos = False
            logNoDesbloquear = False
            LimpiarDetalle()
            LimpiarCampos()
            CalcularValorTotal()

            If String.IsNullOrEmpty(strTipoNegocio) Then
                mostrarMensaje("¡Para realizar la consulta de documentos seleccione tipo de negocio", "Consultar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If strTipoNegocio <> GSTR_ORDENRECIBO _
                And String.IsNullOrEmpty(strTipoPagoPlus) Then
                mostrarMensaje("¡Para realizar la consulta de documentos seleccione forma de pago", "Consultar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If logEsFondosOYD = False Then
                If strTipoNegocio = GSTR_ORDENRECIBO Then
                    If String.IsNullOrEmpty(strEstado) _
                    Or IsNothing(ItemReceptor) _
                    Or IsNothing(Fecha) Then
                        mostrarMensaje("¡Para realizar la consulta de documentos seleccione receptor, estado, y fecha", "Consultar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                Else
                    If String.IsNullOrEmpty(strEstado) _
                    Or IsNothing(ItemReceptor) _
                    Or String.IsNullOrEmpty(strTipoPagoPlus) _
                    Or IsNothing(Fecha) Then
                        Exit Sub
                        mostrarMensaje("¡Para realizar la consulta de documentos seleccione receptor, estado, forma de pago y fecha", "Consultar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            Else
                If strTipoNegocio = GSTR_ORDENFONDOS _
                    Or (strTipoNegocio = GSTR_ORDENGIRO And strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS) Then
                    If String.IsNullOrEmpty(strEstado) _
                        Or IsNothing(ItemReceptor) _
                        Or String.IsNullOrEmpty(strTipoPagoPlus) _
                        Or IsNothing(Fecha) _
                        Or String.IsNullOrEmpty(CarteraColectiva) Then
                        mostrarMensaje("¡Para realizar la consulta de documentos seleccione fondo, receptor, estado, forma de pago y fecha", "Consultar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                ElseIf strTipoNegocio = GSTR_ORDENGIRO And strTipoPagoPlus <> GSTR_CARTERASCOLECTIVAS Then
                    If String.IsNullOrEmpty(strEstado) _
                        Or IsNothing(ItemReceptor) _
                        Or String.IsNullOrEmpty(strTipoPagoPlus) _
                        Or IsNothing(Fecha) Then
                        mostrarMensaje("¡Para realizar la consulta de documentos seleccione receptor, estado, forma de pago y fecha", "Consultar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                ElseIf strTipoNegocio = GSTR_ORDENRECIBO Then
                    If String.IsNullOrEmpty(strEstado) _
                        Or IsNothing(ItemReceptor) _
                        Or IsNothing(Fecha) Then
                        mostrarMensaje("¡Para realizar la consulta de documentos seleccione receptor, estado, y fecha", "Consultar Documentos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            End If

            OcultarCamposConsecutivosBancos()

            ConsultarRegistrosTesorero(String.Empty)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Consulta de Documentos",
                                 Me.ToString(), "ConsultarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConsultarConsecutivos(ByVal pCarteraColectiva As String, Optional ByVal pUserState As String = "CONSECUTIVOS")
        Try
            If Not IsNothing(objProxy.ItemCombos) Then
                objProxy.ItemCombos.Clear()
            End If

            objProxy.Load(objProxy.cargarCombosCondicionalQuery("CONSECUTIVOS_TESORERO_PLUS", pCarteraColectiva, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsultarConsecutivos, pUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los combos especificos del fondo.", Me.ToString, "consultarCombosEspecificosFondo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Function PuedeEjecutar(ByVal s As String) As Boolean
        Return True
    End Function

    Public Sub ObtenerValoresDefectoOYDPLUS(ByVal pstrOpcion As String)
        Try

            Select Case pstrOpcion.ToUpper
                Case "COMBOSRECEPTOR"
                    If DiccionarioCombosOYDPlus.ContainsKey("NOMHOJAEXCEL") Then
                        If DiccionarioCombosOYDPlus("NOMHOJAEXCEL").Count > 0 Then
                            str_NombreHojaExcel = DiccionarioCombosOYDPlus("NOMHOJAEXCEL").FirstOrDefault.Retorno
                        End If
                    End If
                    If DiccionarioCombosOYDPlus.ContainsKey("ESTADOS_TESORERO") Then
                        If DiccionarioCombosOYDPlus("ESTADOS_TESORERO").Count > 1 Then
                            For Each li In DiccionarioCombosOYDPlus("ESTADOS_TESORERO")
                                If li.Retorno = "P" Then
                                    strEstado = li.Retorno
                                End If

                            Next
                        End If
                    End If
                    If DiccionarioCombosOYDPlus.ContainsKey("A2_UTILIZAUNITY") Then
                        If DiccionarioCombosOYDPlus("A2_UTILIZAUNITY").Count > 0 Then
                            If DiccionarioCombosOYDPlus("A2_UTILIZAUNITY").First.Retorno = "SI" Then
                                logEsFondosUnity = True
                            Else
                                logEsFondosUnity = False
                            End If
                        End If
                    End If
                    If DiccionarioCombosOYDPlus.ContainsKey("SUCURSAL_PLUS") Then
                        If DiccionarioCombosOYDPlus("SUCURSAL_PLUS").Count = 1 Then
                            For Each li In DiccionarioCombosOYDPlus("SUCURSAL_PLUS")
                                strSUCURSAL_PLUS = li.Retorno
                            Next
                        Else
                            For Each li In DiccionarioCombosOYDPlus("SUCURSAL_PLUS")
                                If li.Retorno = "-1" Then
                                    strSUCURSAL_PLUS = li.Retorno
                                End If
                            Next
                        End If
                        If DiccionarioCombosOYDPlus("SUCURSAL_PLUS").Count > 1 Then
                            For Each li In DiccionarioCombosOYDPlus("SUCURSAL_PLUS")
                                If li.Retorno = "TD" Then
                                    strSUCURSAL_PLUS = li.Retorno
                                End If
                            Next
                        End If
                        If DiccionarioCombosOYDPlus.ContainsKey("TIPOPAGOPLUS") Then
                            For Each li In DiccionarioCombosOYDPlus("TIPOPAGOPLUS").Where(Function(i) i.Retorno <> GSTR_TODOS_RETORNO)
                                objListaGiro.add(li)
                            Next
                        End If
                        If DiccionarioCombosOYDPlus.ContainsKey("TIPOPAGOPLUSFONDOS") Then
                            For Each li In DiccionarioCombosOYDPlus("TIPOPAGOPLUSFONDOS").Where(Function(i) i.Retorno <> GSTR_TODOS_RETORNO)
                                objListaFondos.add(li)
                            Next
                        End If
                        If DiccionarioCombosOYDPlus.ContainsKey("TIPOPAGOPLUS") Then
                            For Each li In DiccionarioCombosOYDPlus("TIPOPAGOPLUS")
                                objListaRecibo.add(li)
                            Next
                        End If
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores por defecto.",
                                 Me.ToString(), "ObtenerValoresDefecto", Application.Current.ToString(), Program.Maquina, ex)

        End Try

        IsBusy = False
    End Sub

    Public Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean)
        Try
            Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
            Dim objListaCategoria As New List(Of OYDPLUSUtilidades.CombosReceptor)
            Dim objListaCategoria1 As New List(Of OYDPLUSUtilidades.CombosReceptor)

            For Each li In DiccionarioCombosOYDPlusCompleta
                objDiccionario.Add(li.Key, li.Value)
            Next


            If ValoresCompletos Then

                '************************************************************************************
                If Not IsNothing(objListaCategoria) Then
                    objListaCategoria.Clear()
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
                            STRPARAMETRO_FINONSET = "NO"
                        End If
                    End If
                End If

                If objDiccionario.ContainsKey("A2_UTILIZAUNITY") Then
                    If objDiccionario("A2_UTILIZAUNITY").Count > 0 Then
                        If objDiccionario("A2_UTILIZAUNITY").First.Retorno = "SI" Then
                            logEsFondosUnity = True
                            STRPARAMETRO_FINONSET = "SI"
                        Else
                            logEsFondosUnity = False
                            STRPARAMETRO_FINONSET = "NO"
                        End If
                    End If
                End If

                If objDiccionario.ContainsKey("OYDPLUS_ORDENRECAUDO_DATOSOBLIGATORIOS") Then
                    If objDiccionario("OYDPLUS_ORDENRECAUDO_DATOSOBLIGATORIOS").Count > 0 Then
                        If objDiccionario("OYDPLUS_ORDENRECAUDO_DATOSOBLIGATORIOS").First.Retorno = "SI" Then
                            logValidarCamposObligatorios = True
                        Else
                            logValidarCamposObligatorios = False
                        End If
                    End If
                End If

                If objDiccionario.ContainsKey("AUTORIZACIONES_TESOREROAUTOGESTIONA") Then
                    If objDiccionario("AUTORIZACIONES_TESOREROAUTOGESTIONA").Count > 0 Then
                        If objDiccionario("AUTORIZACIONES_TESOREROAUTOGESTIONA").First.Retorno = "SI" Then
                            plogFuncionalidadAutogestionDocumentos = True
                        Else
                            plogFuncionalidadAutogestionDocumentos = False
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
        End Try
    End Sub

    Public Sub CargarConfiguracionReceptorOYDPLUS(ByVal pUserState As String)
        Try
            If logNuevoRegistro Then
                IsBusy = True
            End If
            If Not IsNothing(objProxyPLUS.tblConfiguracionesAdicionalesReceptors) Then
                objProxyPLUS.tblConfiguracionesAdicionalesReceptors.Clear()
            End If
            'CargarParametrosReceptorOYDPLUS(String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar la configuración del receptor.",
                                 Me.ToString(), "CargarConfiguracionReceptor", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub CargarCombosOYDPLUS(ByVal pstrIDReceptor As String, ByVal pstrUserState As String)

        Try
            If logNuevoRegistro Or logEditarRegistro Then

            End If

            If Not IsNothing(objProxyPLUS.CombosReceptors) Then
                objProxyPLUS.CombosReceptors.Clear()
            End If
            objProxyPLUS.Load(objProxyPLUS.OYDPLUS_ConsultarCombosReceptorQuery(pstrIDReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYD, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.",
                                 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Sub LimpiarDetalle()
        Try
            ListaResultadosDocumentos = Nothing
            ListaResultadosDocumentosRecibo = Nothing
            IdBanco = Nothing
            DescripcionBanco = Nothing
            CalcularValorTotal()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los detalles de la consulta.",
                                Me.ToString(), "LimpiarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CalcularValorTotal()
        Try
            TotalGenerar = 0

            If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
                If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                    For Each li In _ListaResultadosDocumentosRecibo
                        If li.Generar Then
                            TotalGenerar += li.curValor
                        End If
                    Next
                End If

            Else
                If Not IsNothing(_ListaResultadosDocumentos) Then
                    For Each li In _ListaResultadosDocumentos
                        If li.Generar Then
                            TotalGenerar += li.curValor
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular el valor total.",
                                Me.ToString(), "CalcularValorTotal", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDetallesRecibos()
        Try
            IsBusy = True

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

            dcProxy.Load(dcProxy.TesoreriaOrdenesDetalleListar_TransferenciasReciboQuery(SelectedDocumentosRecibo.lngIDEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaOrdenesDetalleTransferencias, "CAMBIOSELECTED")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras se consultaba el Detalle de la Orden ", Me.ToString(), "ConsultarDetallesRecibos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ValidarExistenciaChequeOrdenesRecibo()
        Try
            Dim strIDOrdenesSeleccionadas As String = String.Empty

            If logEsFondosOYD And strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION) Then
                If Not IsNothing(_ListaResultadosDocumentos) Then
                    For Each li In _ListaResultadosDocumentos
                        If li.Generar Then
                            If String.IsNullOrEmpty(strIDOrdenesSeleccionadas) Then
                                strIDOrdenesSeleccionadas = li.lngIDEncabezado
                            Else
                                strIDOrdenesSeleccionadas = String.Format("{0},{1}", strIDOrdenesSeleccionadas, li.lngIDEncabezado)
                            End If
                        End If
                    Next
                End If
            Else
                If Not IsNothing(_ListaResultadosDocumentosRecibo) Then
                    For Each li In _ListaResultadosDocumentosRecibo
                        If li.Generar Then
                            If String.IsNullOrEmpty(strIDOrdenesSeleccionadas) Then
                                strIDOrdenesSeleccionadas = li.lngIDEncabezado
                            Else
                                strIDOrdenesSeleccionadas = String.Format("{0},{1}", strIDOrdenesSeleccionadas, li.lngIDEncabezado)
                            End If
                        End If
                    Next
                End If
            End If

            If Not String.IsNullOrEmpty(strIDOrdenesSeleccionadas) Then
                IsBusy = True
                dcProxy.ValidarExistenciaChequesOrdenesRecibo(strIDOrdenesSeleccionadas, Program.Usuario, Program.HashConexion, AddressOf TerminoValidarExistenciaChequeOrdenRecibo, "")
            Else
                IsBusy = False
                logHayCheque = False

                MostrarBancosRecibosCheques()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras se consultaba el Detalle de la Orden ", Me.ToString(), "LlamarDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CambiarColorFondoTextoBuscador()
        Try
            Dim colorFondo As Color
            colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            FondoTextoBuscadoresHabilitado = New SolidColorBrush(colorFondo)
            colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            FondoTextoBuscadoresDesHabilitado = New SolidColorBrush(colorFondo)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el color de fondo del texto.", Me.ToString(), "CambiarColorFondoTextoBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub MostrarBancosRecibosCheques()
        If logHayCheque Then
            If strTipoNegocio = GSTR_ORDENRECIBO Then
                VerBancoFondosOYD = Visibility.Collapsed
                VerBanco = Visibility.Visible
            Else
                If logEsFondosOYD Then
                    VerBancoFondosOYD = Visibility.Visible
                    VerBanco = Visibility.Collapsed
                Else
                    VerBancoFondosOYD = Visibility.Collapsed
                    VerBanco = Visibility.Visible
                End If
            End If
        Else
            If logValidarCamposObligatorios Then
                VerBancoFondosOYD = Visibility.Collapsed
                VerBanco = Visibility.Collapsed
            Else
                If strTipoNegocio = GSTR_ORDENRECIBO Then
                    VerBancoFondosOYD = Visibility.Collapsed
                    VerBanco = Visibility.Visible
                Else
                    If logEsFondosOYD Then
                        VerBancoFondosOYD = Visibility.Visible
                        VerBanco = Visibility.Collapsed
                    Else
                        VerBancoFondosOYD = Visibility.Collapsed
                        VerBanco = Visibility.Visible
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#Region "Temporizador"

    Private _myDispatcherTimerTesorero As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador()
        Try
            If Not IsNothing(_myDispatcherTimerTesorero) Then
                _myDispatcherTimerTesorero.Stop()
                RemoveHandler _myDispatcherTimerTesorero.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerTesorero = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al parar el temporizador.", Me.ToString(), "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ReiniciaTimer()
        Try
            If Program.Recarga_Automatica_Activa Then
                If _myDispatcherTimerTesorero Is Nothing Then
                    _myDispatcherTimerTesorero = New System.Windows.Threading.DispatcherTimer
                    _myDispatcherTimerTesorero.Interval = New TimeSpan(0, 0, 0, 0, Program.Par_lapso_recarga)
                    AddHandler _myDispatcherTimerTesorero.Tick, AddressOf Me.Each_Tick
                End If
                _myDispatcherTimerTesorero.Start()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar el temporizador.", Me.ToString(), "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' recarga de ordenes cada que se cumple el tiempo del temporizador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Each_Tick(sender As Object, e As EventArgs)
        Dim logDebeConsultar As Boolean = True
        If strTipoNegocio = GSTR_ORDENRECIBO Or (strTipoNegocio = GSTR_ORDENFONDOS And (strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION)) Then
            If Not IsNothing(ListaResultadosDocumentosRecibo) Then
                For Each item In ListaResultadosDocumentosRecibo
                    If item.Generar Then
                        logDebeConsultar = False
                        Exit For
                    End If
                Next
                If logDebeConsultar Then
                    ConsultarDocumentos()
                End If
            End If
        Else
            If Not IsNothing(ListaResultadosDocumentos) Then
                For Each item In ListaResultadosDocumentos
                    If item.Generar Then
                        logDebeConsultar = False
                        Exit For
                    End If
                Next
                If logDebeConsultar Then
                    ConsultarDocumentos()
                End If
            End If
        End If
    End Sub

#End Region

#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub _SelectedDocumentosRecibo_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _SelectedDocumentosRecibo.PropertyChanged
        Try
            If Not IsNothing(_SelectedDocumentosRecibo) Then

                If e.PropertyName = "Generar" Then
                    If _SelectedDocumentosRecibo.Generar = True And SeleccionarTodos = False Then
                        IsBusy = True
                        CalcularValorTotal()
                        dcProxy.TempValidarEstadoOrdens.Clear()
                        dcProxy.Load(dcProxy.ValidarEstadoOrdenTesoreriaEncabezadoQuery(String.Format("%{0}%", _SelectedDocumentosRecibo.lngIDEncabezado.ToString), _SelectedDocumentosRecibo.strValorEstado, Program.Usuario, "TE", Program.HashConexion), AddressOf TerminoBloqueoTesorero, "")
                    ElseIf _SelectedDocumentosRecibo.Generar = False Then
                        IsBusy = True
                        CalcularValorTotal()
                        'JFSB 20171026 Se envia la contante GSTR_T en ves de GSTR_OT
                        dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(_SelectedDocumentosRecibo.lngIDEncabezado, GSTR_T, Program.Usuario, Program.HashConexion, AddressOf TerminoDesbloqueoTesorero, String.Empty)
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Mientras se cambiaban las propiedades. ", Me.ToString(), "_SelectedDocumentosRecibo_PropertyChanged", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

End Class

Public Class clsItemsAcumuladosArchivo
    Public intIDEncabezado As Integer
    Public intIDDetalle As Integer
End Class